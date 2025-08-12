using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using DWBServices;
using DWBServices.WebServices;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;
using Microsoft.Extensions.DependencyInjection;
using NotSupportedException = System.NotSupportedException;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormFixedSourceQuery : Form
    {
        #region Parameter

        private DiversityWorkbench.ServerConnection _FixSourceServerConnection;
        private DiversityWorkbench.IWorkbenchUnit _FixSourceIWorkbenchUnit;
        private string _FixSourceSQL = "";
        private System.Data.DataTable _FixSourceTable;
        private Microsoft.Data.SqlClient.SqlDataAdapter _FixSourceDataAdapter = null;
        private Microsoft.Data.SqlClient.SqlDataAdapter FixSourceDataAdapter
        {
            get
            {
                if (this._FixSourceDataAdapter == null)
                {
                    try
                    {
                        this._FixSourceDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(this._FixSourceSQL, DiversityWorkbench.Settings.ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return this._FixSourceDataAdapter;
            }
        }
        private string _FixSourceListInDatabase = "";
        private bool _FixSourceIsListInDatabase = false;
        private Sheet _Sheet;
        private System.Windows.Forms.DataGridViewCell _Cell;
        private DWBServices.WebServices.DwbServiceEnums.DwbService _FixSourceWebservice;
        private System.Collections.Generic.Dictionary<string, string> _FixedSourceValues;
        private System.Collections.Generic.List<string> _Settings;

        #endregion

        #region Construction and form

        public FormFixedSourceQuery(System.Collections.Generic.List<string> Settings, DiversityWorkbench.ServerConnection FixSourceServerConnection, DWBServices.WebServices.DwbServiceEnums.DwbService FixSourceWebservice, DiversityWorkbench.IWorkbenchUnit FixSourceIWorkbenchUnit, System.Windows.Forms.DataGridViewCell Cell, Sheet Sheet)
        {
            InitializeComponent();
            this._Settings = Settings;
            this._FixSourceServerConnection = FixSourceServerConnection;
            this._FixSourceWebservice = FixSourceWebservice;
            this._FixSourceIWorkbenchUnit = FixSourceIWorkbenchUnit;
            this._Sheet = Sheet;
            this._Cell = Cell;
            this.initForm();
        }

        private void initForm()
        {
            try 
            {
                foreach (string S in this._Settings)
                {
                    if (this.labelSettings.Text.Length > 0)
                        this.labelSettings.Text += " - ";
                    this.labelSettings.Text += S;
                }

                if (this._FixSourceIWorkbenchUnit == null)
                {
                    DataColumn DC = this._Sheet.SelectedColumns()[this._Cell.ColumnIndex];
                    switch (DC.RemoteLinks[0].LinkedToModule)
                    {
                        case RemoteLink.LinkedModule.DiversityTaxonNames:
                            DiversityWorkbench.TaxonName T = new TaxonName(DiversityWorkbench.Settings.ServerConnection);
                            this._FixSourceIWorkbenchUnit = T;
                            break;
                    }
                }
                this.InitFixSourceControls();


                this.Text = "Set value for " + this._Sheet.SelectedColumns()[this._Cell.ColumnIndex].DisplayText;
                string SetSourceInfo = "Change the soure for:";
                foreach (string S in this._Settings)
                    SetSourceInfo += "\r\n" + S;
                this.toolTip.SetToolTip(this.buttonSetSource, SetSourceInfo);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Events

        private void InitFixSourceControls()
        {
            this.comboBoxQuery.BackColor = System.Drawing.Color.SkyBlue;
            this.buttonSetSource.Image = DiversityWorkbench.Properties.Resources.Pin_3;
            this.buttonRemoveSource.Visible = false;
            // having a server connection
            if (this._FixSourceServerConnection != null && this._FixSourceServerConnection.DisplayText != null)
            {
                this.labelSource.Text = "Source:\r\n" + _FixSourceServerConnection.DisplayText;
                if (_FixSourceServerConnection.Project != null && _FixSourceServerConnection.Project.Length > 0)
                    this.labelSource.Text += "\r\nProject: " + _FixSourceServerConnection.Project;
                this.buttonRemoveSource.Visible = true;
            }
            else if (this._FixSourceWebservice != null)
            {
                this.labelSource.Text = "Source:\r\n" + this._FixSourceWebservice.ToString();
                this.buttonRemoveSource.Visible = true;
            }
            // nothing
            else
            {
                this.labelSource.Text = "No source selected";
                this.comboBoxQuery.BackColor = System.Drawing.Color.White;
                this.buttonSetSource.Image = DiversityWorkbench.Properties.Resources.Pin_3Gray;
            }
        }

        private void buttonSetSource_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.WorkbenchUnit.FixedSourceSetParameters(
                ref this._FixSourceIWorkbenchUnit
                , ref this._FixSourceServerConnection
                , ref this._FixSourceWebservice
                //, ref this._FixSourceWebserviceOptions
                , ref this.buttonSetSource
                , ref this.comboBoxQuery
                , this.toolTip);
            this.InitFixSourceControls();
            DiversityWorkbench.WorkbenchUnit.SaveSetting(this._Settings, this._FixSourceServerConnection, this._FixSourceWebservice);
        }

        private void buttonRemoveSource_Click(object sender, EventArgs e)
        {
            this._FixSourceServerConnection = null;
            this._FixSourceWebservice = DwbServiceEnums.DwbService.None;
            this.comboBoxQuery.DataSource = null;
            this.InitFixSourceControls();
        }

        private async void comboBoxQuery_DropDown(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                string SQL = "";
                if (this._FixSourceServerConnection != null)
                {
                    if (this._FixSourceServerConnection.ModuleName == "DiversityCollectionCache" ||
                        this._FixSourceServerConnection.DatabaseName.IndexOf("DiversityCollectionCache") > -1)
                    {
                        this._FixSourceServerConnection.ModuleName = "DiversityCollectionCache";
                        SQL = "SELECT [TaxonName] AS DisplayText " +
                              ",[NameURI] AS URI " +
                              ",[BaseURL] " +
                              ",[NameID] AS ID   " +
                              "FROM [" + this._FixSourceServerConnection.DatabaseName + "].[dbo].[TaxonSynonymy] ";
                        if (this.comboBoxQuery.Text.Length > 0)
                            SQL += "WHERE TaxonName LIKE '" + this.comboBoxQuery.Text + "%' ";
                        SQL += "ORDER BY DisplayText";
                    }
                    else
                    {
                        SQL = Sheet.SqlForLinkedSource(this.comboBoxQuery.Text, this._FixSourceServerConnection,
                            this._FixSourceIWorkbenchUnit, this._FixSourceListInDatabase);
                        if (SQL.Length == 0)
                        {
                            try
                            {
                                string Target = "SqlForLinkedSource(" + this.comboBoxQuery.Text
                                                                      + ", " + this._FixSourceServerConnection
                                                                          .ConnectionString
                                                                      + ", " + this._FixSourceIWorkbenchUnit
                                                                          .getServerConnection().DatabaseName
                                                                      + ", " + this._FixSourceListInDatabase;
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(
                                    "FormFixedSourceQuery.comboBoxQuery_DropDown()", Target,
                                    "No SQL for linked source");
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }

                    this._FixSourceTable = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad =
                        new Microsoft.Data.SqlClient.SqlDataAdapter(SQL,
                            this._FixSourceServerConnection.ConnectionString);
                    try
                    {
                        ad.Fill(this._FixSourceTable);
                        this.comboBoxQuery.DataSource = this._FixSourceTable;
                        this.comboBoxQuery.DisplayMember = "DisplayText";
                        this.comboBoxQuery.ValueMember = "URI";
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }

                else if (this._FixSourceWebservice != DwbServiceEnums.DwbService.None)
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    //if (this._FixSourceWebserviceOptions != null && this._FixSourceWebserviceOptions.Count > 0)
                    //    this._FixSourceWebservice.getQueryResults(this.comboBoxQuery.Text, 50, ref dt, this._FixSourceWebserviceOptions);
                    //else
                    //this._FixSourceWebservice.getQueryResults(this.comboBoxQuery.Text, 50, ref dt);

                    if (string.IsNullOrEmpty(comboBoxQuery.Text))
                    {
                        MessageBox.Show("Please enter at least one character");
                        return;
                    }

                    IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                        DwbServiceProviderAccessor.GetDwbWebservice(_FixSourceWebservice);
                    var searchUrl = CreateSearchUrl(comboBoxQuery.Text, _api, _FixSourceWebservice);
                    if (_api == null || string.IsNullOrEmpty(searchUrl))
                    {
                        MessageBox.Show("No webservice defined");
                        return;
                    }

                    try
                    {
                        var tt = await _api.CallWebServiceAsync<object>(searchUrl,
                            DwbServiceEnums.HttpAction.GET);
                        if (tt != null)
                        {
                            var clientSearchModel = _api.GetDwbApiSearchResultModel(tt);
                            ReadDwbSearchModelInQueryTable(clientSearchModel, ref dt);
                        }
                    }
                    catch (Exception ioe)
                    {
                        MessageBox.Show(
                            "The record details cannot be displayed because the web service response is invalid.\r\n\r\n  " +
                            "For more details on the error, see the error log file.\r\n\r\n",
                            "Data Mapping Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        ExceptionHandling.WriteToErrorLogFile(
                            "FormFixedSourceQuery - comboBoxQuery_DropDown, Exception exception: " +
                            ioe);
                    }

                    this.comboBoxQuery.DataSource = dt;
                    this.comboBoxQuery.DisplayMember = "_DisplayText";
                    this.comboBoxQuery.ValueMember = "_URI";
                }
                else if (this._FixSourceSQL != null && this._FixSourceSQL.Length > 0)
                {
                    SQL = this._FixSourceSQL;
                    if (this.comboBoxQuery.Text.Length > 0)
                    {
                        string DisplayColumn = this._Sheet.SelectedColumns()[this._Cell.ColumnIndex].RemoteLinks[0]
                            .RemoteColumnBindings[0].Column.Name;

                        SQL += " AND " + DisplayColumn + " LIKE '" + this.comboBoxQuery.Text + "%' ORDER BY " +
                               DisplayColumn;
                        this.FixSourceDataAdapter.SelectCommand.CommandText = SQL;
                        this.FixSourceDataAdapter.Fill(this._FixSourceTable);
                        this.comboBoxQuery.DataSource = this._FixSourceTable;
                        this.comboBoxQuery.DisplayMember = "DisplayText";
                        this.comboBoxQuery.ValueMember = "URI";
                    }
                    else
                        System.Windows.Forms.MessageBox.Show(
                            "Please give at least the initial characters of the searched item");
                }
            }
            catch (NotSupportedException notSupported)
            {
                MessageBox.Show("The webservice is not supported: " + notSupported.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(notSupported);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private string CreateSearchUrl(string userInput, IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api, DwbServiceEnums.DwbService dwbService)
        {
            const int offset = 0; // default TODO Ariane if we want to add paging, then we can get/set the offset here
            const int MaxRecords = 50; // default TODO Ariane

            var queryRestriction = QueryRestriction(userInput);
            try
            {
                return _api?.DwbApiQueryUrlString(dwbService, queryRestriction, offset, MaxRecords) ?? string.Empty;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private string QueryRestriction(string userInputText)
        {
            // TODO Ariane do we need more?
            string Restriction = userInputText;

            Restriction = HttpUtility.UrlEncode(Restriction);
            return Restriction;
        }

        private void ReadDwbSearchModelInQueryTable(DwbSearchResult result, ref System.Data.DataTable dtQuery)
        {
            try
            {
                if (result is null)
                    return;

                dtQuery.Columns.Clear();
                var columns = new[]
                {
                    new { Name = "URI", Type = "System.String" },
                    new { Name = "DisplayText", Type = "System.String" }
                };
                foreach (var column in columns)
                {
                    dtQuery.Columns.Add(new System.Data.DataColumn(column.Name, Type.GetType(column.Type)));
                }

                foreach (var item in result.DwbApiSearchResponse)
                {
                    var row = dtQuery.NewRow();
                    row["URI"] = item._URL;
                    row["DisplayText"] = item._DisplayText;
                    dtQuery.Rows.Add(row);
                }

            }
            catch (Exception ex)
            {
#if DEBUG

                Console.WriteLine(ex.StackTrace);
#endif 
            }
        }
        private void comboBoxQuery_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQuery.SelectedItem;
            this._FixedSourceValues = new Dictionary<string, string>();
            foreach (System.Data.DataColumn C in R.Row.Table.Columns)
            {
                if (!R[C.ColumnName].Equals(System.DBNull.Value) && R[C.ColumnName].ToString().Length > 0)
                {
                    this._FixedSourceValues.Add(C.ColumnName, R[C.ColumnName].ToString());
                }
            }
            if (this._FixedSourceValues.ContainsKey("ID"))
            {
                int ID;
                if (int.TryParse(this._FixedSourceValues["ID"], out ID))
                {
                    if (this._FixSourceServerConnection.ModuleName != "DiversityCollectionCache") // Markus 20.1.2020 otherwise it will be reset to any database from the module
                        this._FixSourceIWorkbenchUnit.setServerConnection(this._FixSourceServerConnection);
                    else
                    {
                        DiversityWorkbench.ServerConnection SC = new ServerConnection(this._FixSourceServerConnection.ConnectionString);
                        this._FixSourceIWorkbenchUnit.setServerConnection(SC);
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._FixSourceIWorkbenchUnit.UnitValues(ID))
                    {
                        if (!this._FixedSourceValues.ContainsKey(KV.Key) && !KV.Key.StartsWith("_"))
                        {
                            this._FixedSourceValues.Add(KV.Key, KV.Value);
                        }
                    }
                }

            }
        }

        private void buttonInfoSettings_Click(object sender, EventArgs e)
        {
            FormUserSettings f = new FormUserSettings(this._Settings);
            //FormFixedSources f = new FormFixedSources(this._Sheet, this._Settings);
            f.ShowDialog();
        }

        #endregion

        #region Interface

        public System.Collections.Generic.Dictionary<string, string> FixedSourceValues()
        {
            if (this._FixedSourceValues == null)
                this._FixedSourceValues = new Dictionary<string, string>();
            return this._FixedSourceValues;
        }

        public DiversityWorkbench.ServerConnection FixSourceServerConnection() { return this._FixSourceServerConnection; }
        public DwbServiceEnums.DwbService FixSourceWebservice() { return this._FixSourceWebservice; }

        public string EnteredText() { return this.comboBoxQuery.Text; }
        public int SelectedIndex() { return this.comboBoxQuery.SelectedIndex; }

        #endregion

    }
}
