using ABI.System;
using DWBServices.WebServices;
using DWBServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DWBServices.WebServices.TaxonomicServices;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using System.Windows.Forms.DataVisualization.Charting;
using Cursor = System.Windows.Forms.Cursor;

namespace DiversityWorkbench.Export
{
    public class TableColumnUnitValue// : Export.iTableColumn
    {
        #region Properties
        
        private Export.TableColumn _TableColumn;

        public Export.TableColumn TableColumn
        {
            get { return _TableColumn; }
            set { _TableColumn = value; }
        }

        private string _DiversityWorkbenchModuleBaseUri;

        public string DiversityWorkbenchModuleBaseUri
        {
            get { return _DiversityWorkbenchModuleBaseUri; }
            set { _DiversityWorkbenchModuleBaseUri = value; }
        }

        private string _LinkedDiversityWorkbenchModuleBaseUri;

        public string LinkedDiversityWorkbenchModuleBaseUri
        {
            get { return _LinkedDiversityWorkbenchModuleBaseUri; }
            set { _LinkedDiversityWorkbenchModuleBaseUri = value; }
        }

        private string _SourceDisplayText;

        public string SourceDisplayText
        {
            get { return _SourceDisplayText; }
            set { _SourceDisplayText = value; }
        }

        private DiversityWorkbench.DatabaseService _DatabaseService;

        public DiversityWorkbench.DatabaseService DatabaseService
        {
            get { return _DatabaseService; }
            set { _DatabaseService = value; }
        }

        private string _UnitValue;

        public string UnitValue
        {
            get { return _UnitValue; }
            set { _UnitValue = value; }
        }

        private string _LinkedUnitValue;

        public string LinkedUnitValue
        {
            get { return _LinkedUnitValue; }
            set { _LinkedUnitValue = value; }
        }

        public async System.Threading.Tasks.Task<string> GetSourceValue(string CurrentValue)
        {
            // TODO Ariane Change this to only sistinguish between database and webservice
            if (CurrentValue.Length == 0)
                return "";
            string Value = "";
            if (this._DiversityWorkbenchModuleBaseUri == null)
            {
                string SQL = "SELECT " + this._UnitValue + " FROM [" + this._TableColumn.ForeignRelationTable + "] AS T WHERE T." + this._TableColumn.ForeignRelationColumn + " = '" + CurrentValue + "'";
                Value = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            }
            else
            {
                int iTest = 0;
                if (CurrentValue.StartsWith(this._DiversityWorkbenchModuleBaseUri))
                {
                    if (DiversityWorkbench.WorkbenchUnit.getBaseURIfromURI(CurrentValue) == this._DiversityWorkbenchModuleBaseUri)
                    {
                        DiversityWorkbench.WorkbenchUnit.ServiceType ServiceType = DiversityWorkbench.WorkbenchUnit.getServiceType(this._DiversityWorkbenchModuleBaseUri);
                        if (ServiceType == WorkbenchUnit.ServiceType.WorkbenchModule)
                        {
                            DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(CurrentValue);
                            System.Collections.Generic.Dictionary<string, string> Values;
                            this._TableColumn.IWorkbenchUnit.setServerConnection(SC);
                            string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(CurrentValue);
                            Values = this._TableColumn.IWorkbenchUnit.UnitValues(int.Parse(ID));
                            if (Values.ContainsKey(this._UnitValue))
                                Value = Values[this._UnitValue];
                        }
                        else
                        {
                            if (ServiceType == WorkbenchUnit.ServiceType.WebService)
                            {
                                    string ValueForLinkedUnitValue = "";
                                    if (DwbServiceProviderAccessor.Instance == null)
                                    {
                                        MessageBox.Show("The webservice is not available -- FormRemoteQuery 379");
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("The webservice is not available --FormRemoteQuery 379.. dwbServiceProvider is null ");
                                    }
                                    string ServiceName = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(CurrentValue);
                                    DwbServiceEnums.DwbService service = DwbServiceEnums.DwbService.None;
                                    if (Enum.TryParse(ServiceName, true, out DwbServiceEnums.DwbService result))
                                    {
                                        service = result;
                                    }
                                    IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                                        DwbServiceProviderAccessor.GetDwbWebservice(service);

                                    if (!_api.IsValidUrl(CurrentValue))
                                    {
                                        MessageBox.Show("The selected value is not a valid URI or unit ID. URI: " + CurrentValue, "Invalid Selection",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("TableColumnUnitValue, 250: The selected value is not a valid URI or unit ID. URI: " + CurrentValue);
                                        return Value;
                                    }
                                    try{
                                        DwbEntity clientEntity = await getApiDetailModel(_api, CurrentValue);
                                        if (clientEntity == null)
                                        {
                                            return "";
                                        }
                                        string NameOfTaxon = clientEntity.GetMappedApiEntityModel().GetDisplayText() ?? string.Empty;
                                        Value = NameOfTaxon;// TaxonSourceValue(Link, IF);
                                    }
                                    catch (System.Exception ex)
                                    {
                                        MessageBox.Show("An error occurred when trying to synchronize with the webservice: " + ex.Message, "SynchronizeError", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                        Value = "";
                                    }
                            }
                        }
                    }
                }
                else if (int.TryParse(CurrentValue, out iTest))
                {
                    if (this._TableColumn.ForeignRelationTable != null &&
                        this._TableColumn.ForeignRelationColumn != null &&
                        this._TableColumn.ColumnName == "ProjectID")
                    {
                        System.Collections.Generic.Dictionary<string, string> Values;
                        Values = this._TableColumn.IWorkbenchUnit.UnitValues(iTest);
                        if (Values.ContainsKey(this._UnitValue))
                            Value = Values[this._UnitValue];
                    }
                }
            }
            if (this.LinkedDiversityWorkbenchModuleBaseUri != null && this.LinkedUnitValue != null && this.UnitValue.StartsWith("Link to "))
            {
                string LinkedValue = "";
                string[] Links = Value.Split(new char[] { ',' });
                System.Collections.Generic.List<string> ModuleLinks = new List<string>();
                foreach (string L in Links)
                {
                    if (L.Trim().StartsWith(this.LinkedDiversityWorkbenchModuleBaseUri))
                        ModuleLinks.Add(L.Trim());
                }
                foreach (string CurrentLink in ModuleLinks)
                {
                    DiversityWorkbench.WorkbenchUnit.ServiceType ServiceType = DiversityWorkbench.WorkbenchUnit.getServiceType(CurrentLink);
                    if (ServiceType == WorkbenchUnit.ServiceType.WorkbenchModule)
                    {
                        DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(CurrentLink);
                        System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
                        DiversityWorkbench.IWorkbenchUnit _IWorkbenchUnit = null;
                        string Module = this.UnitValue.Substring(this.UnitValue.IndexOf("Diversity"));
                        switch (Module)
                        {
                            case "DiversityAgents":
                                Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = A;
                                break;
                            case "DiversityCollection":
                                CollectionSpecimen C = new CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = C;
                                break;
                            case "DiversityDescriptions":
                                Description D = new Description(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = D;
                                break;
                            case "DiversityExsiccatae":
                                Exsiccate E = new Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = E;
                                break;
                            case "DiversityGazetteer":
                                Gazetteer G = new Gazetteer(SC);
                                _IWorkbenchUnit = G;
                                break;
                            case "DiversityProjects":
                                Project P = new Project(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = P;
                                break;
                            case "DiversityReferences":
                                Reference R = new Reference(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = R;
                                break;
                            case "DiversitySamplingPlots":
                                SamplingPlot SP = new SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = SP;
                                break;
                            case "DiversityScientificTerms":
                                ScientificTerm ST = new ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = ST;
                                break;
                            case "DiversityTaxonNames":
                                TaxonName T = new TaxonName(DiversityWorkbench.Settings.ServerConnection);
                                _IWorkbenchUnit = T;
                                break;
                        }
                        _IWorkbenchUnit.setServerConnection(SC);
                        string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(CurrentLink);
                        Values = _IWorkbenchUnit.UnitValues(int.Parse(ID));
                        if (Values.ContainsKey(this.LinkedUnitValue))
                        {
                            string ValueForLinkedUnitValue = Values[this.LinkedUnitValue];
                            if (ValueForLinkedUnitValue.Length > 0)
                            {
                                if (LinkedValue.Length > 0)
                                    LinkedValue += " | ";
                                LinkedValue += ValueForLinkedUnitValue;
                            }
                        }
                    }
                    else
                    {
                        if (ServiceType == WorkbenchUnit.ServiceType.WebService)
                        {
                            string ValueForLinkedUnitValue = "";
                            if (DwbServiceProviderAccessor.Instance == null)
                            {
                                MessageBox.Show("The webservice is not available -- FormRemoteQuery 379");
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("The webservice is not available --FormRemoteQuery 379.. dwbServiceProvider is null ");
                            }
                            string ServiceName = DiversityWorkbench.WorkbenchUnit.getDatabaseNameFromURI(CurrentLink);
                            DwbServiceEnums.DwbService service = DwbServiceEnums.DwbService.None;
                            if (Enum.TryParse(ServiceName, true, out DwbServiceEnums.DwbService result))
                            {
                                service = result;
                            }
                            IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                                DwbServiceProviderAccessor.GetDwbWebservice(service);

                            if (!_api.IsValidUrl(CurrentLink))
                            {
                                MessageBox.Show("The selected value is not a valid URI or unit ID. URI: " + CurrentLink, "Invalid Selection",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("TableColumnUnitValue, 250: The selected value is not a valid URI or unit ID. URI: " + CurrentLink);
                                continue;
                            }

                            DwbEntity clientEntity = await getApiDetailModel(_api, CurrentLink);
                            if (clientEntity != null)
                            {
                                string NameOfTaxon = clientEntity.GetMappedApiEntityModel().GetDisplayText() ??
                                                     string.Empty;
                                ValueForLinkedUnitValue = NameOfTaxon; // TaxonSourceValue(Link, IF);
                            }

                            if (ValueForLinkedUnitValue.Length > 0)
                            {
                                if (LinkedValue.Length > 0)
                                    LinkedValue += " | ";
                                LinkedValue += ValueForLinkedUnitValue;
                            }
                        }
                    }
                }
                Value = LinkedValue;
            }
            return Value;
        }
       
        private async System.Threading.Tasks.Task<DwbEntity> getApiDetailModel(IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api, string nameUri)
        {
            try
            {
                var tt = await _api.CallWebServiceAsync<object>(
                    nameUri,
                    DwbServiceEnums.HttpAction.GET);
                DwbEntity clientEntity = _api.GetDwbApiDetailModel(tt);
                return clientEntity;
            }
            catch (ArgumentException aEx)
            {
                MessageBox.Show(
                    "The web service call is incorrect.\r\n\r\n  " +
                    "For more details on the error, see the error log file.\r\n\r\n",
                    "Web service Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ExceptionHandling.WriteToErrorLogFile(
                    "TableColumnUnitValues - getApiDetailModel, ArgumentException exception: " +
                    aEx);
                return null;
            }
            catch (InvalidOperationException ioEx)
            {
                MessageBox.Show(
                    "The web service call is incorrect.\r\n\r\n  " +
                    "For more details on the error, see the error log file.\r\n\r\n",
                    "Web service Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ExceptionHandling.WriteToErrorLogFile(
                    "TableColumnUnitValues - getApiDetailModel, Exception exception: " +
                    ioEx);
                return null;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    "The web service call is incorrect.\r\n\r\n  " +
                    "For more details on the error, see the error log file.\r\n\r\n",
                    "Web service Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ExceptionHandling.WriteToErrorLogFile(
                    "TableColumnUnitValues - getApiDetailModel, Exception exception: " +
                    ex);
                return null;
            }
        }

        private void ReadDwbDetailModelInQueryTable(DwbEntity dwbEntity, ref System.Data.DataTable dtQuery)
        {
            try
            {
                if (dwbEntity is null)
                    return;
                if (dwbEntity is TaxonomicEntity)
                {
                    TaxonomicEntity taxonomicEntity = (TaxonomicEntity)dwbEntity;

                    dtQuery.Columns.Clear();
                    var columns = new[]
                    {
                        new { Name = "_URI", Type = "System.String" },
                        new { Name = "_DisplayText", Type = "System.String" },
                        new { Name = "Family", Type = "System.String" },
                        new { Name = "Order", Type = "System.String" },
                        new { Name = "Hierarchy", Type = "System.String" },
                    };
                    foreach (var column in columns)
                    {
                        dtQuery.Columns.Add(new System.Data.DataColumn(column.Name, System.Type.GetType(column.Type)));
                    }

                    var row = dtQuery.NewRow();
                    row["_URI"] = taxonomicEntity._URL;
                    row["_DisplayText"] = taxonomicEntity._DisplayText;
                    row["Family"] = taxonomicEntity.Family;
                    row["Order"] = taxonomicEntity.Order;
                    row["Hierarchy"] = taxonomicEntity.Hierarchy;
                    dtQuery.Rows.Add(row);
                }
                else
                {
                    // TODO implement other service mappings
                }
            }
            catch (System.Exception ex)
            {
#if DEBUG

                Console.WriteLine(ex.StackTrace);
#endif 
            }
        }
        #endregion

        #region Construction

        public TableColumnUnitValue(Export.TableColumn TC, string UnitValue)
        {
            this._TableColumn = TC;
            this._UnitValue = UnitValue;
            this._SourceDisplayText = TC.ForeignRelationTable;
        }

        public TableColumnUnitValue(Export.TableColumn TC, string BaseURI, string UnitValue, string Source)
        {
            this._TableColumn = TC;
            this._UnitValue = UnitValue;
            this._DiversityWorkbenchModuleBaseUri = BaseURI;
            this._SourceDisplayText = Source;
        }

        #endregion

    }
}
