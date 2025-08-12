using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Perception;
using DiversityWorkbench;
using DiversityWorkbench.Forms;
using DWBServices;
using DWBServices.WebServices;
using DWBServices.WebServices.TaxonomicServices;
using Microsoft.Web.WebView2.Core.Raw;
using static DiversityCollection.CacheDatabase.UserControlLookupSource;
using Microsoft.SqlServer.Management.Smo.Wmi;
using Windows.ApplicationModel.Contacts;

namespace DiversityCollection.CacheDatabase;

internal class CacheDBWebServiceHandler
{
    private IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api;
    private DwbServiceEnums.DwbService currentDwbService;

    private void SetDwbWebservice(IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> dwbWebservice,
        DwbServiceEnums.DwbService currentService)
    {
        _api = dwbWebservice;
        currentDwbService = currentService;
    }

    private IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> GetDwbWebservice()
    {
        return _api;
    }

    public async Task<(bool, string, string)> ForWebservice_TransferToCacheAsync(string sourceView, string sourceTable,
        TypeOfSource typeOfSource)
    {
        var ok = true;
        var report = "";
        var message = "";
        using (var progressDialog = new WebServiceProgressDialog("Initializing web service transfer..."))
        {
            progressDialog.Show();
            try
            {
                var transferActive = CacheDB.SetTransferActive(sourceTable,
                    CacheDB.DatabaseName,
                    sourceView,
                    false);
                if (transferActive.Length > 0)
                {
                    report += transferActive;
                    return (false, message, report);
                }

                var targetTable = "";
                var linkColumn = "";
                var baseUrl = "";
                switch (typeOfSource)
                {
                    case TypeOfSource.Taxa:
                        targetTable = "CacheIdentification";
                        linkColumn = "NameURI";
                        // Dynamically set the web service and base URL
                        if (Enum.TryParse<DwbServiceEnums.DwbService>(sourceView, out var dwbService))
                        {
                            var webservice = DwbServiceProviderAccessor.GetDwbWebservice(dwbService);
                            if (webservice != null)
                            {
                                SetDwbWebservice(webservice, dwbService);
                                baseUrl = webservice.GetBaseAddress();
                            }
                            else
                            {
                                throw new InvalidOperationException("The web service could not be retrieved.");
                            }
                        }

                        break;
                    default:
                        throw new NotSupportedException($"Unsupported source type: {typeOfSource}");
                }

                // retrieve projects 
                var dictProject = GetProjects(targetTable, ref message);

                var selectedProjects = ShowProjectSelectionDialog(dictProject);
                if (selectedProjects.Count > 0)
                {
                    message = sourceView + ": starting transfer";
                    if (CacheDB.ProcessOnly)
                        CacheDB.LogEvent(ToString(), "TransferToCache(bool ProcessOnly, string Report)", message);
                    else
                        message = "";

                    var iAdded = 0;
                    var iPresent = 0;
                    var updateResult = await ProcessSelectedProjectsAsync(selectedProjects, targetTable, linkColumn,
                        baseUrl,
                        typeOfSource, sourceView, iAdded, iPresent, message, progressDialog);
                    iAdded = updateResult.Item2;
                    iPresent = updateResult.Item3;
                    message = updateResult.Item4;
                    message = iAdded + " datasets added, " + iPresent + " datasets present";
                    // TODO this.labelCountCacheDB.Text = Message;
                    message += "\r\nto MS-SQL Server Database " +
                               CacheDB.DatabaseName +
                               "\r\non Server " + CacheDB.DatabaseServer;
                }
            }
            catch (Exception ex)
            {
                ok = false;
                report += ex.Message;
            }
            finally
            {
                progressDialog.Close();
            }
        }

        return (ok, message, report);
    }

    private Dictionary<string, bool> GetProjects(string targetTable, ref string message)
    {
        var sql = $"SELECT SUBSTRING(T.TABLE_SCHEMA, 9, 255) FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_NAME = '{targetTable}' ORDER BY T.TABLE_NAME";
        var dtProjects = new DataTable();
        CacheDB.ExecuteSqlFillTableInCacheDB(sql, ref dtProjects, ref message);
        var dictProject = new Dictionary<string, bool>();
        foreach (DataRow row in dtProjects.Rows)
        {
            var key = row[0]?.ToString() ?? string.Empty;
            dictProject.Add(key, false);
        }
        return dictProject;
    }

    private List<string> ShowProjectSelectionDialog(Dictionary<string, bool> projects)
    {
        var form = new FormGetMultiFromList("Projects", "Please select the project that should be included", projects);
        form.ShowDialog();
        if (form.DialogResult == DialogResult.OK && form.SelectedItems().Count > 0)
        {
            return form.SelectedItems();
        }
        return new List<string>();
    }

    private async Task<(bool, int, int, string)> ProcessSelectedProjectsAsync(
        List<string> selectedProjects,
        string targetTable,
        string linkColumn,
        string baseUrl,
        TypeOfSource typeOfSource,
        string sourceView,
        int iAdded,
        int iPresent,
        string message,
        WebServiceProgressDialog progressDialog)
    {
        bool ok = false;
        foreach (var project in selectedProjects)
        {
            progressDialog.UpdateMessage($"Processing project: {project}");
            var sql =
                $"SELECT DISTINCT {linkColumn} FROM Project_{project}.{targetTable} T WHERE T.{linkColumn} LIKE '{baseUrl}%'";
            var dtName = new DataTable();
            CacheDB.ExecuteSqlFillTableInCacheDB(sql, ref dtName, ref message);
            var i = 1;
            foreach (DataRow row in dtName.Rows)
            {
                switch (typeOfSource)
                {
                    case TypeOfSource.Taxa:
                        var result = await ForWebservice_MapAndTransferTaxa(row[0].ToString(), sourceView, iAdded, iPresent, progressDialog);
                        ok = result.Item1;
                        iAdded = result.Item2;
                        iPresent = result.Item3;
                        break;
                }

                //Application.DoEvents(); // Allow UI updates
                i++;
            }
        }
        return (ok, iAdded, iPresent, message);
    }

    private bool UpdateCacheDBWithWebServiceDataTaxa<T>(string URL, string sourceView, T clientModel, ref int iAdded, ref int iPresent)
        where T : DwbEntity
    {
        if (clientModel == null)
            throw new ArgumentNullException(nameof(clientModel));
        var whereClause = $" WHERE NameID = -1 AND BaseURL = '{URL}' AND SourceView = '{sourceView}'";
        // Check if the record already exists in the database
        var sql = $"SELECT COUNT(*) FROM TaxonSynonymy {whereClause}";
        var result = CacheDB.ExecuteSqlSkalarInCacheDB(sql);
        if (result != "0")
        {
            iPresent++;
            return true; // record already exists
        }

        // Ensure the client model is NOT of type TaxonomicEntity
        if (clientModel is not TaxonomicEntity)
            throw new InvalidOperationException("The clientModel must not be of type TaxonomicEntity.");

        // Map the client model to its API entity model
        dynamic mappedClientModel = clientModel.GetMappedApiEntityModel();
        if (mappedClientModel == null)
            return false;
        // throw new InvalidOperationException("Mapped client model cannot be null.");
        // Get the mapping between properties and database columns
        var propertyToColumnMapping = mappedClientModel.GetPropertyToTaxonSynonymyMapping();
        if (propertyToColumnMapping == null)
            return false;
        // throw new InvalidOperationException("Property-to-column mapping is missing or empty.");
        // Prepare the values to insert into the database
        var columnValues = new Dictionary<string, string>();
        foreach (var property in typeof(TaxonomicEntity).GetProperties())
            if (propertyToColumnMapping.TryGetValue(property.Name, out string columnName))
            {
                var value = property.GetValue(mappedClientModel) as string;
                if (!string.IsNullOrEmpty(value)) columnValues[columnName] = value;
            }

        // Construct the SQL INSERT statement
        var columns = string.Join(", ", columnValues.Keys);
        var values =
            string.Join(", ", columnValues.Values.Select(v => $"'{v.Replace("'", "''")}'")); // Escape single quotes
        sql = $"INSERT INTO TaxonSynonymy (NameID, BaseURL, NameURI, SourceView, {columns}) " +
              $"VALUES (-1, '{URL.Replace("'", "''")}', '{URL.Replace("'", "''")}', '{sourceView.Replace("'", "''")}', {values})";
        iAdded++;
        return DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(sql);
    }

    private async Task<(bool, int, int)> ForWebservice_MapAndTransferTaxa(string URL, string sourceView, int iAdded, int iPresent, WebServiceProgressDialog webServiceProgressDialog)
    {
        var OK = true;
        try
        {
            var _api =
                GetDwbWebservice();
            if (_api == null || string.IsNullOrEmpty(URL))
            {
                MessageBox.Show("No webservice defined");
                return (false, iAdded, iPresent);
            }
            // Update the progress dialog message
            webServiceProgressDialog.UpdateMessage($"Retrieving data from the web service for URL: \r\n\r\n {URL}");
            
            var tt = await _api.CallWebServiceAsync<object>(URL);
            if (tt != null)
            {
                var clientEntity = _api.GetDwbApiDetailModel(tt);
                OK = UpdateCacheDBWithWebServiceDataTaxa(URL, sourceView, clientEntity, ref iAdded, ref iPresent);
            }
        }
        catch (DataMappingException dataMappingException)
        {
            MessageBox.Show(
                "The mapping of the web service data did not work for url:\r\n\r\n" +
                URL +
                "\r\n\r\n Error Details:\r\n" + dataMappingException.Message,
                "Data Mapping Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            ExceptionHandling.WriteToErrorLogFile(dataMappingException);
            OK = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "An error occurred while retrieving data from the web service with url:\r\n\r\n" +
                URL +
                "\r\n\r\n Returned error message:" + ex.Message, "Failed connection to web service",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            ExceptionHandling.WriteToErrorLogFile(ex);
            OK = false;
        }

        return (OK, iAdded, iPresent);
    }
}