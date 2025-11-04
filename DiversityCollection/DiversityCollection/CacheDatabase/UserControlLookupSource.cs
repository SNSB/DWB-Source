#define AgentIdentifierIncluded

using DWBServices.WebServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace DiversityCollection.CacheDatabase
{

    public partial class UserControlLookupSource : UserControl, InterfaceLookupSource, InterfaceTransfer
    {

        #region Parameter and properties

        private static System.Collections.Generic.Dictionary<TypeOfSource, int?> _LookupSourceVersion;
        public static System.Collections.Generic.Dictionary<TypeOfSource, int?> LookupSourceVersion
        {
            get
            {
                if (_LookupSourceVersion == null)
                {
                    _LookupSourceVersion = new Dictionary<TypeOfSource, int?>();
                    _LookupSourceVersion.Add(TypeOfSource.Agents, 4);
                    _LookupSourceVersion.Add(TypeOfSource.Descriptions, null);
                    _LookupSourceVersion.Add(TypeOfSource.Gazetteer, null);
                    _LookupSourceVersion.Add(TypeOfSource.Plots, 2);
                    _LookupSourceVersion.Add(TypeOfSource.References, 1);
                    _LookupSourceVersion.Add(TypeOfSource.ScientificTerms, 3); // Markus 10.4.25: Hochsetzen nach aenderung
                    _LookupSourceVersion.Add(TypeOfSource.Taxa, 3); // Markus 2.4.25: Hochsetzen nach aenderung in CommonName
                }
                return _LookupSourceVersion;
            }
        }

        private bool _NeedsRecreation = false;


        public enum TypeOfSource { Taxa, ScientificTerms, Agents, Plots, References, Descriptions, Gazetteer };

        private TypeOfSource _TypeOfSource = TypeOfSource.Taxa;
        public  UserControlLookupSource.TypeOfSource SourceType()
        {
            return this._TypeOfSource;
        }

        private readonly string MainTableGazetteer = "Gazetteer";
        private readonly string MainTableScientificTerms = "ScientificTerm";
        private readonly string MainTableTaxa = "TaxonSynonymy";
        private readonly string MainTableAgents = "Agent";
        private readonly string MainTableReference = "ReferenceTitle";
        private readonly string MainTablePlots = "SamplingPlot";

        private System.Collections.Generic.Dictionary<string, object> _TransferHistory = new Dictionary<string, object>();

        private System.Data.DataRow _Row;
        public DiversityCollection.CacheDatabase.InterfaceCacheDatabase _Interface;

        private string _SourceView = "";

        private bool _IsWebservice = false;

        private readonly CacheDBWebServiceHandler _webServiceHandler;
        public string SourceView
        {
            get { return _SourceView; }
            set
            {
                _SourceView = value;
                this.textBoxView.Text = value;
            }
        }

        public enum SubsetTable {
            Agent, AgentContactInformation, AgentImage,
#if AgentIdentifierIncluded
            AgentIdentifier,
#endif
            Description,
            ReferenceTitle, ReferenceRelator, 
            Gazetteer, GazetteerExternalDatabase, 
            ScientificTerm,  
            TaxonSynonymy, TaxonAnalysis, TaxonAnalysisCategory, TaxonAnalysisCategoryValue, TaxonCommonName, TaxonList, TaxonNameExternalDatabase, TaxonNameExternalID, procTaxonNameHierarchy, TaxonHierarchy,
            SamplingPlot, SamplingPlotLocalisation, SamplingPlotProperty, procSamplingPlotLocalisationHierarchy, procSamplingPlotPropertyHierarchy
        }

        private System.Collections.Generic.Dictionary<SubsetTable, string> _Subsets;
        private System.Collections.Generic.Dictionary<SubsetTable, string> Subsets
        {
            get 
            {
                if (this._Subsets == null)
                {
                    this._Subsets = new Dictionary<SubsetTable, string>();
                    switch (this._TypeOfSource)
                    {
                        case TypeOfSource.Taxa:
                            this._Subsets.Add(SubsetTable.TaxonAnalysis, "_LA");
                            this._Subsets.Add(SubsetTable.TaxonAnalysisCategory, "_LAC");
                            this._Subsets.Add(SubsetTable.TaxonAnalysisCategoryValue, "_LACV");
                            this._Subsets.Add(SubsetTable.TaxonCommonName, "_C");
                            this._Subsets.Add(SubsetTable.TaxonList, "_L");
                            this._Subsets.Add(SubsetTable.TaxonNameExternalDatabase, "_E");
                            this._Subsets.Add(SubsetTable.TaxonNameExternalID, "_EID");
                            this._Subsets.Add(SubsetTable.TaxonHierarchy, "_H");
                            this._Subsets.Add(SubsetTable.procTaxonNameHierarchy, "_PH"); //#102
                            break;
                        case TypeOfSource.Agents:
                            this._Subsets.Add(SubsetTable.AgentContactInformation, "_C");
                            this._Subsets.Add(SubsetTable.AgentImage, "_I");
#if AgentIdentifierIncluded
                            this._Subsets.Add(SubsetTable.AgentIdentifier, "_ID");
#endif
                            break;
                        case TypeOfSource.References:
                            this._Subsets.Add(SubsetTable.ReferenceRelator, "_R");
                            break;
                        case TypeOfSource.Gazetteer:
                            this._Subsets.Add(SubsetTable.GazetteerExternalDatabase, "_E");
                            break;
                        case TypeOfSource.Plots:
                            this._Subsets.Add(SubsetTable.SamplingPlotLocalisation, "_L");
                            this._Subsets.Add(SubsetTable.SamplingPlotProperty, "_P");
                            this._Subsets.Add(SubsetTable.procSamplingPlotLocalisationHierarchy, "_L_h");
                            this._Subsets.Add(SubsetTable.procSamplingPlotPropertyHierarchy, "_P_h");
                            break;
                    }
                }
                return _Subsets; 
            }
            //set { _Subsets = value; }
        }

        public System.Collections.Generic.Dictionary<SubsetTable, string> SubsetTables()
        {
            return this.Subsets;
        }

        private System.Collections.Generic.List<SubsetTable> _SubsetRoutines;
        private System.Collections.Generic.List<SubsetTable> SubsetRoutines()
        {
            if (this._SubsetRoutines == null)
            {
                this._SubsetRoutines = new List<SubsetTable>();
                this._SubsetRoutines.Add(SubsetTable.procSamplingPlotLocalisationHierarchy);
                this._SubsetRoutines.Add(SubsetTable.procSamplingPlotPropertyHierarchy);
                this._SubsetRoutines.Add(SubsetTable.procTaxonNameHierarchy);
            }
            return this._SubsetRoutines;
        }
       
        private string _BaseURL;
        private string BaseURL()
        {
            if (this._BaseURL == null)
            {
                string Database = this._Row["DatabaseName"].ToString();
                if (Database.IndexOf("[") > -1)
                    this._BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT TOP 1 [BaseURL]  FROM " + Database + ".[dbo].[ViewBaseURL]");
                else
                    this._BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT " + Database + ".dbo.BaseURL()");
            }
            return this._BaseURL;
        }

#endregion

#region Construction

        public UserControlLookupSource(System.Data.DataRow R, InterfaceCacheDatabase Interface, TypeOfSource SourceType)
        {
            InitializeComponent();
            this._Row = R;
            this._Interface = Interface;
            this._TypeOfSource = SourceType;
            this._SourceView = "";
            this.SourceView = this._Row["SourceView"].ToString();
            if (this._Row["SourceID"].Equals(System.DBNull.Value) &&
                this._Row["DatabaseName"].Equals(System.DBNull.Value))
                _IsWebservice = true;
            if (_IsWebservice)
            {
                this.ForWebservice_InitControl();
                _webServiceHandler = new CacheDBWebServiceHandler();
            }
            else
            {
                this.textBoxDatabase.Text = this._Row["DatabaseName"].ToString();
                this.textBoxServer.Text = this._Row["LinkedServerName"].ToString();
                if (this.textBoxServer.Text.Length == 0)
                    this.textBoxServer.Text = "local";
                this.textBoxSource.Text = this._Row["Source"].ToString();
            }
            this.initControl();
        }

#endregion

#region Control

        private void initControl()
        {
            this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(80);
            this.labelCountCacheDB.Text = "";
            this.labelCountPostgres.Text = "";
            switch (this._TypeOfSource)
            {
                case TypeOfSource.ScientificTerms:
                    break;
            }
            this.initPostgresControls();
            this.setCacheDatabaseControls();
            this.initScheduleTransferControls();
            this.CheckVersion();
        }

        private void CheckVersion()
        {
            if (!this._IsWebservice && LookupSourceVersion[this._TypeOfSource] != null)
            {
                string SQL = "SELECT TOP 1 Version FROM " + this.MainTable() + "Source WHERE SourceView LIKE '%" + this._SourceView + "'";
                string Version = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                int iVersion;
                if (int.TryParse(Version, out iVersion))
                {
                    if (iVersion < LookupSourceVersion[this._TypeOfSource])
                    {
                        this.labelView.ForeColor = System.Drawing.Color.Red;
                        this.labelView.Text = "Needs recreation to version " + LookupSourceVersion[this._TypeOfSource].ToString();
                        this._NeedsRecreation = true;
                        this.buttonRecreateView.Enabled = true;
                        this.buttonRecreateView.Image = DiversityCollection.Resource.Update;
                    }
                    else
                    {
                        this.labelView.ForeColor = System.Drawing.Color.Gray;
                        this.labelView.Text = "View"; // Needs recreation to version " + LookupSourceVersion[this._TypeOfSource].ToString();
                        this._NeedsRecreation = false;
                        this.buttonRecreateView.Enabled = false;
                        this.buttonRecreateView.Image = null;
                    }
                }
                else
                {
                    if (LookupSourceVersion[this._TypeOfSource].ToString().Length > 0)
                    {
                        Version = LookupSourceVersion[this._TypeOfSource].ToString();
                        if (int.TryParse(Version, out iVersion))
                        {
                            this.labelView.ForeColor = System.Drawing.Color.Red;
                            this.labelView.Text = "Needs recreation to version " + LookupSourceVersion[this._TypeOfSource].ToString();
                            this._NeedsRecreation = true;
                            this.buttonRecreateView.Enabled = true;
                            this.buttonRecreateView.Image = DiversityCollection.Resource.Update;
                        }
                    }
                }
            }
        }

        public enum Stati { RemovingData, RemovalFailed, CheckingConflicts, Conflicts, DataRemoved, SearchingSource, AddingSource, Recreated, RecreationFailed }

        public void setState(Stati State, string Message = "")
        {
            switch (State)
            {
                case Stati.RemovingData:
                    this.textBoxView.Text = "Removing content";
                    break;
                case Stati.RemovalFailed:
                    this.textBoxView.Text = "Removing failed";
                    break;
                case Stati.CheckingConflicts:
                    this.textBoxSource.Text = "Checking conflicts";
                    break;
                case Stati.Conflicts:
                    if (Message.Length > 0)
                        this.textBoxSource.Text = Message;
                    else
                        this.textBoxSource.Text = "No conflicts detected";
                    break;
                case Stati.DataRemoved:
                    this.textBoxView.Text = "Data removed";
                    break;
                case Stati.SearchingSource:
                    if (Message.Length == 0)
                        this.textBoxView.Text = "Searching source";
                    else
                    {
                        this.textBoxView.Text = "Source detected";
                        this.textBoxSource.Text = Message;
                    }
                    break;
                case Stati.AddingSource:
                    if (Message.Length == 0)
                        this.textBoxView.Text = "Adding source";
                    else
                        this.textBoxView.Text = "Adding " + Message;
                    break;
                case Stati.Recreated:
                    if (Message.Length == 0)
                        this.textBoxView.Text = "Source recreated";
                    else
                    {
                        this.textBoxView.Text = this.SourceView;
                        this.textBoxSource.Text = Message;
                    }
                    break;
                case Stati.RecreationFailed:
                    this.textBoxView.Text = "Recreation failed";
                    break;
            }
            System.Windows.Forms.Application.DoEvents();
        }

#endregion

#region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        public bool TransferData(ref string Report, bool TransferFromSourceToCache, bool TransferFromCacheToPostgres, /*bool ProcessOnly,*/ InterfaceCacheDatabase InterfaceCacheDB)//, bool FilterForUpdate)
        {
            bool OK = true;
            try
            {
                if (this.IncludeInTransferCacheDB)
                {
                    if (TransferFromSourceToCache)
                        OK = this.TransferToCache(ref Report, InterfaceCacheDB);
                    if (this.IncludeInTransferPostgres)
                    {
                        if (TransferFromCacheToPostgres)
                            OK = this.TransferToPostgres(ref Report, InterfaceCacheDB);
                    }
                }
                else if (this.IncludeInTransferPostgres)
                {
                    if (TransferFromCacheToPostgres)
                        OK = this.TransferToPostgres(ref Report, InterfaceCacheDB);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        public void initCacheDBControls()
        {
            this.setCacheDatabaseControls();
        }

        public string DisplayText() { return this.SourceView; }

#endregion

#region Common informations
        
        private string MainTable()
        {
            string TableName = "";
            switch (this._TypeOfSource)
            {
                case TypeOfSource.Agents:
                    TableName = this.MainTableAgents;
                    break;
                case TypeOfSource.Gazetteer:
                    TableName = this.MainTableGazetteer;
                    break;
                case TypeOfSource.References:
                    TableName = this.MainTableReference;
                    break;
                case TypeOfSource.ScientificTerms:
                    TableName = this.MainTableScientificTerms;
                    break;
                case TypeOfSource.Taxa:
                    TableName = this.MainTableTaxa;
                    break;
                case TypeOfSource.Plots:
                    TableName = this.MainTablePlots;
                    break;
            }
            return TableName;
        }

        private string PrimaryKey()
        {
            string PK = "";
            switch (this._TypeOfSource)
            {
                case TypeOfSource.Agents:
                    PK = "AgentID";
                    break;
                case TypeOfSource.Gazetteer:
                case TypeOfSource.Taxa:
                    PK = "NameID";
                    break;
                case TypeOfSource.References:
                    PK = "RefID";
                    break;
                case TypeOfSource.ScientificTerms:
                    PK = "RepresentationID";
                    break;
                case TypeOfSource.Plots:
                    PK = "PlotID";
                    break;
            }
            return PK;
        }


#endregion

#region View and transfer webservice

    /// <summary>
    /// Handles the click event of the "Test" button, initiating the transfer of data 
    /// from a web service to the cache database or displaying a form for manual interaction 
    /// if the web service is not enabled.
    /// </summary>
    /// <param name="sender">The source of the event, typically the button that was clicked.</param>
    /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
    /// <remarks>
    /// This method performs various operations such as resetting transfer errors, 
    /// invoking web service handlers, writing transfer protocols, and handling exceptions.
    /// If the web service is disabled, it opens a form for manual data interaction.
    /// </remarks>
    private async void buttonTest_Click(object sender, EventArgs e)
    {
        if (this._IsWebservice)
        {
            if (testForCompetingTransfer())
                return;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string message = "";
            string report = "";
            try
            {
                /// Reset TransferError
                this.ResetTransferErrors();
                this._TransferHistory = new Dictionary<string, object>();
                var result = await _webServiceHandler.ForWebservice_TransferToCacheAsync(this._SourceView,
                    this.SourcesTable(), _TypeOfSource);
                message = result.Item2 ?? string.Empty;
                report = result.Item3 ?? string.Empty;
                this.WriteTransferProtocol(report, true);
                this.WriteReport(message, report);

                if (!(string.IsNullOrEmpty(message)))
                {
                    this.labelCountCacheDB.Text = message.Contains("datasets present", StringComparison.OrdinalIgnoreCase)
                        ? message[..(message.IndexOf("datasets present", StringComparison.OrdinalIgnoreCase) + "datasets present".Length)]
                        : message;
                    }

                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    System.Windows.Forms.MessageBox.Show(result.Item2);
                else if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
                {
                    CacheDB.LogEvent(this.Name.ToString(), "TransferToCache(bool ProcessOnly, string Report)",
                        message);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("An error occurred when trying to transfer the Webservice data to the Cache DB! \r\n\r\n" +
                                "\r\n Returned error message:" + ex.Message, "Failed transfer", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                report += "\r\n" + ex.Message;
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                DiversityCollection.CacheDatabase.CacheDB.SetTransferFinished(
                    this.SourcesTable(), false, this._SourceView, report, "");
                this.WriteHistory(HistoryTarget.DataToCache, null);
            }
        }
        else
        {
            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(false, "dbo", this.SourceView);
            f.ShowDialog();
        }
    }

    private bool testForCompetingTransfer()
    {
        string CompetingTransfer = this.CompetingTransfer(false);
        if (CompetingTransfer.Length > 0)
        {
            System.Windows.Forms.MessageBox.Show(
                "Competing transfer:\r\n\r\n" + CompetingTransfer + "\r\n\r\nmust be finished first",
                "Competing transfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (System.Windows.Forms.MessageBox.Show(
                    "If you are sure, that there is no competing transfer, you can remove the entry for the competing transfer.\r\n\r\nDo you want to remove the entry for the competing transfer:\r\n\r\n" +
                    CompetingTransfer + "?\r\n\r\n", "Competing transfer", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DiversityCollection.CacheDatabase.CacheDB.SetTransferFinished(
                    this.SourcesTable(), false, this._SourceView, "", "");
            }

            return true;
        }

        return false;
    }

#endregion

#region Removing a source

        /// <summary>
        /// Deleting the created view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string Message = "Do you want to remove the source for " + this.textBoxSource.Text + "?";
            if (System.Windows.Forms.MessageBox.Show(Message, "Remove source?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            this.RemoveSource();
            //string View = this.textBoxView.Text;
            //if (View.Length > 0)
            //{
            //    string SQL = "";
            //    if (!this.RemoveSourceDataFromTables())
            //    {
            //        System.Windows.Forms.MessageBox.Show("Deleting canceled");
            //        return;
            //    }

            //    SQL = "DELETE " + this.MainTable() + "Source WHERE SourceView = '" + this._SourceView + "'";
            //    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            //    {
            //        System.Data.DataTable dt = new DataTable();
            //        SQL = "select t.TABLE_NAME from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME like '" + this._SourceView + "%'";
            //        string Message = "";
            //        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
            //        if (Message.Length == 0)
            //        {
            //            bool OK = true;
            //            foreach (System.Data.DataRow R in dt.Rows)
            //            {
            //                SQL = "DROP VIEW [dbo].[" + R[0].ToString() + "]";
            //                if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            //                {
            //                    OK = false;
            //                }
            //            }
            //            if (OK)
            //            {
            //                switch (this._TypeOfSource)
            //                {
            //                    case TypeOfSource.Agents:
            //                        this._Interface.initAgentSources();
            //                        break;
            //                    case TypeOfSource.Gazetteer:
            //                        this._Interface.initGazetteerSources();
            //                        break;
            //                    case TypeOfSource.Plots:
            //                        this._Interface.initPlotSources();
            //                        break;
            //                    case TypeOfSource.References:
            //                        this._Interface.initReferenceTitleSources();
            //                        break;
            //                    case TypeOfSource.ScientificTerms:
            //                        this._Interface.initTermSources();
            //                        break;
            //                    case TypeOfSource.Taxa:
            //                        this._Interface.initTaxonSources();
            //                        break;
            //                }

            //            }
            //        }
            //    }
            //}
        }

        private bool RemoveSource(bool ForRecreation = false)
        {
            bool OK = true;
            string View = this.textBoxView.Text;
            if (View.Length > 0)
            {
                string SQL = "";
                if (!this.RemoveSourceDataFromTables(ForRecreation))
                {
                    System.Windows.Forms.MessageBox.Show("Deleting canceled");
                    return false;
                }

                SQL = "DELETE " + this.MainTable() + "Source WHERE SourceView = '" + this._SourceView + "'";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    System.Data.DataTable dt = new DataTable();
                    SQL = "select t.TABLE_NAME from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME like '" + this._SourceView + "%'";
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (Message.Length == 0)
                    {
                        OK = true;
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            SQL = "DROP VIEW [dbo].[" + R[0].ToString() + "]";
                            if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                            {
                                OK = false;
                            }
                        }
                        if (OK && !ForRecreation)
                        {
                            switch (this._TypeOfSource)
                            {
                                case TypeOfSource.Agents:
                                    this._Interface.initAgentSources();
                                    break;
                                case TypeOfSource.Gazetteer:
                                    this._Interface.initGazetteerSources();
                                    break;
                                case TypeOfSource.Plots:
                                    this._Interface.initPlotSources();
                                    break;
                                case TypeOfSource.References:
                                    this._Interface.initReferenceTitleSources();
                                    break;
                                case TypeOfSource.ScientificTerms:
                                    this._Interface.initTermSources();
                                    break;
                                case TypeOfSource.Taxa:
                                    this._Interface.initTaxonSources();
                                    break;
                            }

                        }
                    }
                }
            }
            return OK;
        }

        private bool RemoveSourceDataFromTables(bool ForRecreation = false)
        {
            bool OK = true;
            //bool DoDelete = false;
            System.Collections.Generic.List<string> Tables = new List<string>();
            System.Collections.Generic.List<string> Routines = new List<string>();
            switch (this._TypeOfSource)
            { 
                case TypeOfSource.Taxa:
                    Tables.Add(this.MainTableTaxa);
                    break;
                case TypeOfSource.Gazetteer:
                    Tables.Add(this.MainTableGazetteer);
                    break;
                case TypeOfSource.ScientificTerms:
                    Tables.Add(this.MainTableScientificTerms);
                    break;
                case TypeOfSource.References:
                    Tables.Add(this.MainTableReference);
                    break;
                case TypeOfSource.Agents:
                    Tables.Add(this.MainTableAgents);
                    break;
                case TypeOfSource.Plots:
                    Tables.Add(this.MainTablePlots);
                    break;
            }
            Tables.Add(Tables[0] + "SourceView");
            // getting the tables
            foreach (System.Collections.Generic.KeyValuePair<SubsetTable, string> KV in this.Subsets)
            {
                if (!this.SubsetRoutines().Contains(KV.Key))
                    Tables.Add(KV.Key.ToString());
            }
            // getting the numbers
            string Numbers = "Do you want to remove the data from the following tables:\r\n";
            System.Collections.Generic.List<string> EmptyTables = new List<string>();
            foreach (string T in Tables)
            {
                string SQL = "SELECT COUNT(*) FROM " + T + " WHERE SourceView = '" + this._SourceView + "'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result != "0")
                {
                    Numbers += "\r\n" + T + ": " + Result;
                }
                else
                    EmptyTables.Add(T);
            }

            // removing the empty tables
            foreach (string T in EmptyTables)
                Tables.Remove(T);

            if (Tables.Count > 0)
            {
                if (!ForRecreation)
                {
                    // asking for permission to delete
                    if (System.Windows.Forms.MessageBox.Show(Numbers + "\r\n\r\ndatasets of the source\r\n" + this._SourceView + ".\r\n\r\nShould these be removed?", "Remove old data", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
                        return false;
                }
                else
                {
                    this.setState(Stati.RemovingData);
                }

                // Showing the conflicts
                if (ForRecreation)
                {
                    this.setState(Stati.CheckingConflicts);
                }
                System.Collections.Generic.List<string> Conflicts = this.ConflictingSources();
                string Conflict = "";
                if (Conflicts.Count > 0)
                {
                    foreach (string C in Conflicts)
                        Conflict += "\r\n" + C;
                    if (System.Windows.Forms.MessageBox.Show("The following sources need an update of the content after removal of the data:\r\n" + Conflict + ".\r\n\r\nRemove data anyway?", "Update of data needed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return false;
                    string ConflictMessage = Conflicts.Count.ToString() + " conflicts detected";
                    this.setState(Stati.Conflicts, ConflictMessage);
                }
                else if (ForRecreation)
                {
                    this.setState(Stati.Conflicts);
                }

                // removing the data
                string Message = "";
                string Exception = "";
                foreach (string T in Tables)
                {
                    string SQL = this.SqlDelete(T);
                    if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Exception))
                    {
                        // checking existence - older version may miss new tables
                        SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_NAME = '" + T + "'";
                        string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                        if (Result != "0")
                            OK = false;
                        //if (ForRecreation)
                        //{
                        //}
                        //else
                        //    OK = false;
                    }
                }
                if (!OK)
                {
                    Message = "Removal of data failed: " + Message + "\r\n" + Exception;
                    if ((!ForRecreation))
                        this.setState(Stati.RemovalFailed);
                }
                else
                {
                    Message = "Data removed";
                    if ((!ForRecreation))
                        this.setState(Stati.DataRemoved);
                }
                if (Conflict.Length > 0)
                    Message += "\r\n\r\nPlease update the content of the following sources:\r\n\r\n" + Conflict;
                System.Windows.Forms.MessageBox.Show(Message);
                return OK;
            }
            else
                return true;
        }

        private System.Collections.Generic.List<string> ConflictingSources(string Table = "")
        {
            System.Collections.Generic.List<string> Conflicts = new List<string>();
            if (Table.Length == 0)
                Table = this.MainTable();
            string SQL = "DECLARE @BaseURL varchar(500); " +
                "SET @BaseURL = (SELECT MIN(BaseURL) FROM " + this._SourceView + "); ";
            SQL += "SELECT DISTINCT SourceView FROM " + Table + "SourceView T WHERE EXISTS (SELECT * FROM " + Table + " T INNER JOIN " + Table + "SourceView S " +
                    " ON T." + this.PrimaryKey() + " = S." + this.PrimaryKey() +
                    " AND T.BaseURL = S.BaseURL " +
                    " AND T.BaseURL = @BaseURL " +
                    " AND S.SourceView <> '" + this._SourceView + "'); ";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
                Conflicts.Add(R[0].ToString());
            return Conflicts;
        }

        private string SqlDelete(string Table = "")
        {
            if (Table.Length == 0)
                Table = this.MainTable();
            string SQL = "DELETE T FROM " + Table + " T WHERE SourceView = '" + this._SourceView + "' ";
            return SQL;
        }


#endregion

#region Recreation of a source
        private void buttonRecreateView_Click(object sender, EventArgs e)
        {
//#if !DEBUG
//            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
//            return;
//#endif
            System.Collections.Generic.Dictionary<string, string> Parameters = this.SourceParameters();
            if (!this.RemoveSource(true))
            {
                this.setState(Stati.RemovalFailed);
                //System.Windows.Forms.MessageBox.Show("Removal failed");
                return;
            }
            switch(this._TypeOfSource)
            {
                case TypeOfSource.Agents:
                    break;
            }
            string Source = this._Interface.AddSource(this, Parameters);
            if (Source.Length > 0)
            {
                this.SourceView = Source;
                this.setState(Stati.Recreated, Parameters["Project"]);
                this.initControl();
                this.textBoxView.Focus(); 
            }
            else
            {
                this.setState(Stati.RecreationFailed);
            }
        }

        //private DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable RecreationTable()
        //{
        //    DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable T = SubsetTable.Agent;
        //    switch (this._TypeOfSource)
        //    {
        //        case TypeOfSource.Agents:
        //            T= UserControlLookupSource.SubsetTable.Agent;
        //            break;
        //    }
        //    return T;
        //}

        //private DiversityWorkbench.WorkbenchUnit.ModuleType RecreationModule()
        //{
        //    DiversityWorkbench.WorkbenchUnit.ModuleType M = DiversityWorkbench.WorkbenchUnit.ModuleType.Agents;
        //    switch (this._TypeOfSource)
        //    {
        //        case TypeOfSource.Agents:
        //            M = DiversityWorkbench.WorkbenchUnit.ModuleType.Agents;
        //            break;
        //    }
        //    return M;
        //}

        //private System.Collections.Generic.Dictionary<DiversityCollection.CacheDatabase.UserControlLookupSource.SubsetTable, string> RecreationDataTables()
        //{
        //    return this.Subsets;
        //}

        private System.Collections.Generic.Dictionary<string, string> SourceParameters()
        {
            System.Collections.Generic.Dictionary<string, string> Parameters = new Dictionary<string, string>();
            try
            {
                Parameters.Add("Source", this.SourceView);
                string SQL = "SELECT TOP (1) Source " +
                    "FROM " + this.MainTable() + "Source AS S " +
                    "WHERE(SourceView = '" + this.SourceView + "')";
                string Project = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);

                SQL = "SELECT TOP (1) SourceID " +
                    "FROM " + this.MainTable() + "Source AS S " +
                    "WHERE(SourceView = '" + this.SourceView + "')";
                string ProjectID = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);

                SQL = "SELECT TOP (1) LinkedServerName " +
                    "FROM " + this.MainTable() + "Source AS S " +
                    "WHERE(SourceView = '" + this.SourceView + "')";
                string LinkedServerName = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);

                SQL = "SELECT TOP (1) DatabaseName " +
                    "FROM " + this.MainTable() + "Source AS S " +
                    "WHERE(SourceView = '" + this.SourceView + "')";
                string DatabaseName = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);

                if (Project.Length > 0 && ProjectID.Length > 0 && DatabaseName.Length > 0)
                {
                    Parameters.Add("Project", Project);
                    Parameters.Add("ProjectID", ProjectID);
                    Parameters.Add("Database", DatabaseName);
                    if (LinkedServerName.Length > 0)
                        Parameters.Add("LinkedServer", LinkedServerName);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Parameters;
        }


#endregion

#region Report

        private string _ReportFile;

        public string ReportFile
        {
            get 
            {
                if (_ReportFile == null)
                {
                    // Markus 3.6.24: Remove : from path and \\\\
                    _ReportFile = DiversityCollection.CacheDatabase.CacheDB.ReportsDirectory() + "Datatransfer_" + this._TypeOfSource.ToString() + "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss").Replace(":", "");//.Year.ToString();
                    //if (System.DateTime.Now.Month < 10) _ReportFile += "0";
                    //_ReportFile += System.DateTime.Now.Month.ToString();
                    //if (System.DateTime.Now.Day < 10) _ReportFile += "0";
                    //_ReportFile += System.DateTime.Now.Day.ToString() + "_";
                    //if (System.DateTime.Now.Hour < 10) _ReportFile += "0";
                    //_ReportFile += System.DateTime.Now.Hour.ToString();
                    //if (System.DateTime.Now.Minute < 10) _ReportFile += "0";
                    //_ReportFile += System.DateTime.Now.Minute.ToString();
                    //if (System.DateTime.Now.Second < 10) _ReportFile += "0";
                    //_ReportFile += System.DateTime.Now.Second.ToString();
                    _ReportFile += ".txt";
                }
                return _ReportFile; 
            }
        }

        private void WriteReport(string Message, string Report)
        {
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
            {
                System.IO.StreamWriter sw;
                if (System.IO.File.Exists(this.ReportFile))
                    sw = new System.IO.StreamWriter(ReportFile, true, System.Text.Encoding.UTF8);
                else
                    sw = new System.IO.StreamWriter(ReportFile, false, System.Text.Encoding.UTF8);
                try
                {
                    sw.WriteLine("Data transfer to Cache database");
                    sw.WriteLine();
                    sw.WriteLine("Started by:\t" + System.Environment.UserName);
                    sw.Write("Started at:\t");
                    sw.WriteLine(DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString());
                    sw.WriteLine();
                    sw.Write(Message);
                    sw.WriteLine();
                    sw.Write(Report);
                    sw.WriteLine();
                    sw.WriteLine();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                finally
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        
#endregion

#region Cache database

        private bool IsCacheDatabase()
        {
            bool IsCacheDB = true;
            try
            {
                string sql = "SELECT diversityworkbenchmodule();";
                string result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(sql);
                if (result != "DiversityCollectionCache")
                    IsCacheDB = false;
            }
            catch (System.Exception ex)
            {
                IsCacheDB = false;
            }
            return IsCacheDB;
        }

        private System.Collections.Generic.Dictionary<SubsetTable, string> _TransferredSubsets;
        private System.Collections.Generic.Dictionary<SubsetTable, string> TransferredSubsets()
        {
            if (this._TransferredSubsets == null)
            {
                _TransferredSubsets = new Dictionary<SubsetTable, string>();
                string SQL = "SELECT Subsets FROM " + this.MainTable() + "Source";
                SQL += " WHERE SourceView = '" + this._SourceView + "'";
                string Message = "";
                string Subsets = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
                string[] SS = Subsets.Split(new char[] { '|' });
                for (int i = 0; i < SS.Length; i++)
                {
                    switch (SS[i])
                    {
                        // Taxon
                        case "TaxonAnalysis":
                            this._TransferredSubsets.Add(SubsetTable.TaxonAnalysis, this.Subsets[SubsetTable.TaxonAnalysis]);
                            break;
                        case "TaxonAnalysisCategory":
                            this._TransferredSubsets.Add(SubsetTable.TaxonAnalysisCategory, this.Subsets[SubsetTable.TaxonAnalysisCategory]);
                            break;
                        case "TaxonAnalysisCategoryValue":
                            this._TransferredSubsets.Add(SubsetTable.TaxonAnalysisCategoryValue, this.Subsets[SubsetTable.TaxonAnalysisCategoryValue]);
                            break;
                        case "TaxonCommonName":
                            this._TransferredSubsets.Add(SubsetTable.TaxonCommonName, this.Subsets[SubsetTable.TaxonCommonName]);
                            break;
                        case "TaxonList":
                            this._TransferredSubsets.Add(SubsetTable.TaxonList, this.Subsets[SubsetTable.TaxonList]);
                            break;
                        case "TaxonNameExternalDatabase":
                            this._TransferredSubsets.Add(SubsetTable.TaxonNameExternalDatabase, this.Subsets[SubsetTable.TaxonNameExternalDatabase]);
                            break;
                        case "TaxonNameExternalID":
                            this._TransferredSubsets.Add(SubsetTable.TaxonNameExternalID, this.Subsets[SubsetTable.TaxonNameExternalID]);
                            break;
                        case "procTaxonNameHierarchy":
                            this._TransferredSubsets.Add(SubsetTable.procTaxonNameHierarchy, this.Subsets[SubsetTable.procTaxonNameHierarchy]);
                            break;
                        // Issue #102
                        case "TaxonHierarchy":
                            this._TransferredSubsets.Add(SubsetTable.TaxonHierarchy, this.Subsets[SubsetTable.TaxonHierarchy]);
                            break;
                        // Agent
                        case "AgentContactInformation":
                            this._TransferredSubsets.Add(SubsetTable.AgentContactInformation, this.Subsets[SubsetTable.AgentContactInformation]);
                            break;
                        case "AgentImage":
                            this._TransferredSubsets.Add(SubsetTable.AgentImage, this.Subsets[SubsetTable.AgentImage]);
                            break;
#if AgentIdentifierIncluded
                        case "AgentIdentifier":
                            this._TransferredSubsets.Add(SubsetTable.AgentIdentifier, this.Subsets[SubsetTable.AgentIdentifier]);
                            break;
#endif


                        // Reference
                        case "ReferenceRelator":
                            this._TransferredSubsets.Add(SubsetTable.ReferenceRelator, this.Subsets[SubsetTable.ReferenceRelator]);
                            break;

                            // Plot
                        case "SamplingPlotLocalisation":
                            this._TransferredSubsets.Add(SubsetTable.SamplingPlotLocalisation, this.Subsets[SubsetTable.SamplingPlotLocalisation]);
                            break;
                        case "SamplingPlotProperty":
                            this._TransferredSubsets.Add(SubsetTable.SamplingPlotProperty, this.Subsets[SubsetTable.SamplingPlotProperty]);
                            break;
                        case "procSamplingPlotLocalisationHierarchy":
                            this._TransferredSubsets.Add(SubsetTable.procSamplingPlotLocalisationHierarchy, this.Subsets[SubsetTable.procSamplingPlotLocalisationHierarchy]);
                            break;
                        case "procSamplingPlotPropertyHierarchy":
                            this._TransferredSubsets.Add(SubsetTable.procSamplingPlotPropertyHierarchy, this.Subsets[SubsetTable.procSamplingPlotPropertyHierarchy]);
                            break;
                    }
                }
            }
            return _TransferredSubsets;
        }

        public static System.Collections.Generic.List<SubsetTable> SourceSubsets(SubsetTable Table)
        {
            System.Collections.Generic.List<SubsetTable> Tables = new List<SubsetTable>();
            switch(Table)
            {
                case SubsetTable.Agent:
                    Tables.Add(SubsetTable.AgentContactInformation);
                    Tables.Add(SubsetTable.AgentImage);
#if AgentIdentifierIncluded
                    Tables.Add(SubsetTable.AgentIdentifier);
#endif
                    break;
                case SubsetTable.Gazetteer:
                    Tables.Add(SubsetTable.GazetteerExternalDatabase);
                    break;
                case SubsetTable.ReferenceTitle:
                    Tables.Add(SubsetTable.ReferenceRelator);
                    break;
                case SubsetTable.SamplingPlot:
                    Tables.Add(SubsetTable.SamplingPlotLocalisation);
                    Tables.Add(SubsetTable.SamplingPlotProperty);
                    break;
                case SubsetTable.TaxonSynonymy:
                    Tables.Add(SubsetTable.TaxonAnalysis);
                    Tables.Add(SubsetTable.TaxonAnalysisCategory);
                    Tables.Add(SubsetTable.TaxonAnalysisCategoryValue);
                    Tables.Add(SubsetTable.TaxonCommonName);
                    Tables.Add(SubsetTable.TaxonList);
                    Tables.Add(SubsetTable.TaxonNameExternalDatabase);
                    Tables.Add(SubsetTable.TaxonNameExternalID);
                    //#102
                    Tables.Add(SubsetTable.TaxonHierarchy);
                    Tables.Add(SubsetTable.procTaxonNameHierarchy);
                    break;
            }
            return Tables;
        }

        private bool CheckConsistencyOfViews()
        {
            bool OK = true;
            try
            {
                string SQL = "";
                string Message = "";
                switch (this._TypeOfSource)
                {
                    case TypeOfSource.Agents:
                        SQL = "SELECT TOP 1 " + this.TableColumns(SubsetTable.Agent) + " FROM  " + SubsetTable.Agent.ToString() + " T; ";
                        System.Data.DataTable dtAgent = new DataTable();
                        OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtAgent, ref Message);
                        if (OK && Message.Length == 0)
                        {
                            SQL = "SELECT TOP 1 " + this.TableColumns(SubsetTable.AgentContactInformation) + " FROM  " + SubsetTable.AgentContactInformation.ToString() + " T; ";
                            System.Data.DataTable dtContact = new DataTable();
                            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtContact, ref Message);
                        }
                        else OK = false;
                        break;
                }

            }
            catch (Exception)
            {
                OK = false;
            } 
            return OK;
        }

#region Transfer data

        /// <summary>
        /// Determines if there is a competing transfer operation currently in progress.
        /// </summary>
        /// <param name="ForPostgres">
        /// A boolean value indicating whether the check should be performed for a Postgres database.
        /// </param>
        /// <returns>
        /// A string representing the identifier of the competing transfer operation if one exists; 
        /// otherwise, an empty string.
        /// </returns>
        /// <remarks>
        /// This method queries the database to check if any transfer operation is currently being executed.
        /// If a competing transfer is found, its identifier is returned. If no competing transfer exists, 
        /// an empty string is returned.
        /// </remarks>
        /// <exception cref="System.Exception">
        /// Thrown if an error occurs while executing the SQL query.
        /// </exception>
        private string CompetingTransfer(bool ForPostgres)
        {
            string CurrentTransfer = "";
            string SQL = "SELECT MAX(TransferIsExecutedBy) FROM " + this.MainTable() + "Source";
            if (ForPostgres)
                SQL += "Target";
            SQL += " WHERE NOT TransferIsExecutedBy IS NULL";
            try
            {
                CurrentTransfer = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return CurrentTransfer;
        }

        private void buttonTransferToCache_Click(object sender, EventArgs e)
        {
            string CompetingTransfer = this.CompetingTransfer(false);
            if (CompetingTransfer.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Competing transfer:\r\n\r\n" + CompetingTransfer + "\r\n\r\nmust be finished first", "Competing transfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Report = "";
            this.TransferToCache(ref Report, null);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private bool TransferToCache(ref string Report, InterfaceCacheDatabase InterfaceCacheDB)
        {
            ///ToDo: Darf nur false liefern wenn was schief geht
            bool OK = true;
            /// Reset TransferError
            this.ResetTransferErrors();

            string Message = "";

            //string ProblemsSolved = "";

            // if the date of the last update in the data should be checked
            if (this.CompareLogDateCacheDB)//.FilterSourceForUpdate)
            {
                if (!this.DataAreUpdatedInSource(/*ProcessOnly, */ref Report))
                {
                    Message = "No data transferred data:\r\nData not updated in \t" + this._SourceView;
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    {
                        Report += "\r\n" + Message;
                        if (InterfaceCacheDB != null)
                            InterfaceCacheDB.ShowTransferState(Message);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(Message);
                    }
                    return OK;
                }
            }

            string Errors = "";
            this._TransferHistory = new Dictionary<string, object>();
            try
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                {
                    Message = this._SourceView + ": starting transfer";
                    CacheDB.LogEvent(this.Name.ToString(), "TransferToCache(bool ProcessOnly, ref string Report)", Message);
                    //DiversityWorkbench.ExceptionHandling.WriteToEventLog(this.Name.ToString(), "TransferToCache(bool ProcessOnly, ref string Report)", Message, DiversityWorkbench.Settings.ModuleName);
                    if (InterfaceCacheDB != null)
                        InterfaceCacheDB.ShowTransferState(Message);
                }
                Message = "";
                string SQL = "";

                // Check if the scheduled time is reached
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && !DiversityCollection.CacheDatabase.CacheDB.SchedulerPlanedTimeReached(
                    this.SourcesTable(), this._SourceView, null, null, ref Errors, ref Message))
                    return OK;

                string TransferActive = DiversityCollection.CacheDatabase.CacheDB.SetTransferActive(this.SourcesTable(),
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseName,
                    this._SourceView,
                    false);
                if (TransferActive.Length > 0)
                {
                    Report += TransferActive;
                    return false;
                }
                string SqlRestriction = this.SqlRestrictionImport(InterfaceCacheDB, ref Message);
                Message = "";


#region old
                //switch (this._TypeOfSource)
                //{
                //    case TypeOfSource.Taxa:
                //        SQL = "";
                //        if (true)//DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                //        {
                //            // Checking for pro parte synonyms
                //            Message = "Checking for pro parte synonyms";
                //            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //                this.TransferSetMessage(Message);
                //            if (InterfaceCacheDB != null)
                //                InterfaceCacheDB.ShowTransferState(Message);
                //            SQL = "SELECT T.NameID FROM " + this._SourceView + " T group by  T.NameID having count(*) > 1";
                //            System.Data.DataTable dtProParte = new DataTable();
                //            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtProParte, ref Message);
                //            string ProParteIDs = "";
                //            if (dtProParte.Rows.Count > 0)
                //            {
                //                foreach (System.Data.DataRow R in dtProParte.Rows)
                //                {
                //                    if (ProParteIDs.Length > 0) ProParteIDs += ", ";
                //                    ProParteIDs += R[0].ToString();
                //                }
                //            }

                //            Message = "Transfer " + this.MainTableTaxa;
                //            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //                this.TransferSetMessage(Message);
                //            if (InterfaceCacheDB != null)
                //                InterfaceCacheDB.ShowTransferState(Message);
                //            SQL = "DELETE T FROM " + this.MainTableTaxa + " T WHERE SourceView = '" + this._SourceView + "'; " +
                //                "INSERT INTO " + this.MainTableTaxa +
                //                "(" + this.TableColumns(SubsetTable.TaxonSynonymy) + ", SourceView) " +
                //                "SELECT T." + this.TableColumns(SubsetTable.TaxonSynonymy).Replace("BaseURL", "T.BaseURL") + 
                //                 ", '" + this._SourceView + "' " +
                //                "FROM " + this._SourceView + " T LEFT OUTER JOIN " + this._SourceView + "_H H ON T.NameID = H.NameID";
                //            if (ProParteIDs.Length > 0)
                //                SQL += " WHERE T.NameID NOT IN (" + ProParteIDs + ")";
                //            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors))
                //            {
                //                if (dtProParte.Rows.Count > 0)
                //                {
                //                    foreach (System.Data.DataRow R in dtProParte.Rows)
                //                    {
                //                        SQL = "INSERT INTO " + this.MainTableTaxa + " (" + this.TableColumns(SubsetTable.TaxonSynonymy) + ", SourceView) " +
                //                            "SELECT TOP 1 " + this.TableColumns(SubsetTable.TaxonSynonymy, "", "", "T") + ", '" + this._SourceView + "' " +
                //                            "FROM " + this._SourceView + " T LEFT OUTER JOIN " + this._SourceView + "_H H ON T.NameID = H.NameID " +
                //                            "WHERE T.NameID = " + R[0].ToString();
                //                        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Errors);
                //                    }
                //                    ProblemsSolved = "The following NameIDs contained pro parte synonymy and where reduced to the first relation: " + ProParteIDs;
                //                    this._TransferHistory.Add("ProParte", dtProParte.Rows.Count);
                //                }
                //                SQL = "SELECT COUNT(*) FROM " + this.MainTableTaxa;
                //                string NumberMainTable = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
                //                Report = "\r\nTransferred data:\r\n" + NumberMainTable + "\t" + this.MainTableTaxa;
                //                this._TransferHistory.Add(this.MainTableTaxa, int.Parse(NumberMainTable));
                //                if (this.TransferSubsets(ref Message, ref Report, /*ProcessOnly, */InterfaceCacheDB))
                //                {
                //                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //                        this.setCacheDatabaseControls();
                //                }
                //            }
                //            else
                //            {
                //                Message = Errors;
                //            }
                //        }
                //        break;
                //    //case TypeOfSource.ScientificTerms:
                //    //    SQL = "DELETE T FROM " + this.MainTableScientificTerms + " T WHERE SourceView = '" + this._SourceView + "'; " +
                //    //        "INSERT INTO " + this.MainTableScientificTerms + " " +
                //    //        "(" + this.TableColumns(SubsetTable.ScientificTerm) + ", SourceView) " +
                //    //        "SELECT " + this.TableColumns(SubsetTable.ScientificTerm) + ", '" + this._SourceView + "' AS SourceView " +
                //    //        "FROM " + this._SourceView;
                //    //    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors))
                //    //    {
                //    //        SQL = "SELECT COUNT(*) FROM " + this.MainTableScientificTerms;
                //    //        Report = "\r\nTransferred data:\r\n" + DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message) + "\t" + this.MainTableScientificTerms;
                //    //        if (this.TransferSubsets(ref Message, ref Report, /*ProcessOnly,*/ InterfaceCacheDB))
                //    //        {
                //    //            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //    //                this.setCacheDatabaseControls();
                //    //        }
                //    //    }
                //    //    else
                //    //    {
                //    //        Message = Errors;
                //    //    }
                //    //    break;
                //    //case TypeOfSource.Gazetteer:
                //    //    SQL = "DELETE T FROM " + this.MainTableGazetteer + " T WHERE SourceView = '" + this._SourceView + "'; " +
                //    //        "INSERT INTO " + this.MainTableGazetteer + " " +
                //    //        "(" + this.TableColumns(SubsetTable.Gazetteer) + ", SourceView) " +
                //    //        "SELECT DISTINCT " + this.TableColumns(SubsetTable.Gazetteer) + ", '" + this._SourceView + "' AS SourceView " +
                //    //        "FROM " + this._SourceView;
                //    //    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors))
                //    //    {
                //    //        SQL = "SELECT COUNT(*) FROM " + this.MainTableGazetteer + " WHERE SourceView = '" + SourceView + "'";
                //    //        Report = "\r\nTransferred data:\r\n" + DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message) + "\t" + this.MainTableGazetteer;
                //    //        if (this.TransferSubsets(ref Message, ref Report, /*ProcessOnly,*/ InterfaceCacheDB))
                //    //        {
                //    //            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //    //                this.setCacheDatabaseControls();
                //    //        }
                //    //    }
                //    //    else
                //    //    {
                //    //        Message = Errors;
                //    //    }
                //    //    break;
                //    //case TypeOfSource.Agents:
                //    //    SQL = "DELETE T FROM " + this.MainTableAgents + " T WHERE SourceView = '" + this._SourceView + "'; " +
                //    //        "INSERT INTO " + this.MainTableAgents + " (" + this.TableColumns(SubsetTable.Agent) + ", SourceView) " +
                //    //        "SELECT " + this.TableColumns(SubsetTable.Agent) + ", '" + this._SourceView + "' AS SourceView " +
                //    //        "FROM " + this._SourceView + "";
                //    //    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors))
                //    //    {
                //    //        SQL = "SELECT COUNT(*) FROM " + this.MainTableAgents;
                //    //        Report = "\r\nTransferred data:\r\n" + DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message) + "\t" + this.MainTableAgents;
                //    //        if (this.TransferSubsets(ref Message, ref Report, /*ProcessOnly,*/ InterfaceCacheDB))
                //    //        {
                //    //            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //    //                this.setCacheDatabaseControls();
                //    //        }
                //    //    }
                //    //    else
                //    //    {
                //    //        Message = Errors;
                //    //    }
                //    //    break;
                //    //case TypeOfSource.References:
                //    //    SQL = "DELETE T FROM " + this.MainTableReference + " T WHERE SourceView = '" + this._SourceView + "'; " +
                //    //        "INSERT INTO " + this.MainTableReference + " " +
                //    //        "(" + this.TableColumns(SubsetTable.ReferenceTitle) + ", SourceView) " +
                //    //        "SELECT " + this.TableColumns(SubsetTable.ReferenceTitle) + ", '" + this._SourceView + "' AS SourceView " +
                //    //        "FROM " + this._SourceView;
                //    //    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors))
                //    //    {
                //    //        SQL = "SELECT COUNT(*) FROM " + this.MainTableReference;
                //    //        Report = "\r\nTransferred data:\r\n" + DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message) + "\t" + this.MainTableReference;
                //    //        if (this.TransferSubsets(ref Message, ref Report, /*ProcessOnly,*/ InterfaceCacheDB))
                //    //        {
                //    //            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //    //                this.setCacheDatabaseControls();
                //    //        }
                //    //    }
                //    //    else
                //    //    {
                //    //        Message = Errors;
                //    //    }
                //    //    break;
                //    //case TypeOfSource.Plots:
                //        //SQL = "DELETE T FROM " + this.MainTable() + " T WHERE SourceView = '" + this._SourceView + "'; " +
                //        //    "INSERT INTO " + this.MainTable() + " (" + this.TableColumnsMainTable() + ", SourceView) " +
                //        //    "SELECT " + this.TableColumnsMainTable() + ", '" + this._SourceView + "' AS SourceView " +
                //        //    "FROM " + this._SourceView + " AS V WHERE NOT EXISTS (SELECT * FROM " + this.MainTablePlots + " AS T WHERE T.BaseURL = V.BaseURL AND T.PlotID = V.PlotID)";
                //        //if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors))
                //        //{
                //        //    SQL = "SELECT COUNT(*) FROM " + this.MainTable();
                //        //    Report = "\r\nTransferred data:\r\n" + DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message) + "\t" + this.MainTable();
                //        //    if (this.TransferSubsets(ref Message, ref Report, /*ProcessOnly,*/ InterfaceCacheDB))
                //        //    {
                //        //        if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //        //            this.setCacheDatabaseControls();
                //        //    }
                //        //}
                //        //else
                //        //{
                //        //    Message += Errors;
                //        //}
                //        //break;
                //    default:
                //        SQL = "DECLARE @BaseURL varchar(500); " +
                //            "SET @BaseURL = (SELECT MIN(BaseURL) FROM " + this._SourceView + "); " +
                //            "DELETE T FROM " + this.MainTable() + "SourceView T WHERE SourceView = '" + this._SourceView + "'; " +
                //            "DELETE T FROM " + this.MainTable() + " T WHERE SourceView = '" + this._SourceView + "' " +
                //            " AND NOT EXISTS (SELECT * FROM " + this.MainTable() + " T INNER JOIN " + this.MainTable() + "SourceView S " +
                //            " ON T." + this.PrimaryKey() + " = S." + this.PrimaryKey() +
                //            " AND T.BaseURL = S.BaseURL " +
                //            " AND T.BaseURL = @BaseURL " +
                //            " AND S.SourceView <> '" + this._SourceView + "'); " +
                //            "INSERT INTO " + this.MainTable() + " (" + this.TableColumnsMainTable() + ", SourceView) " +
                //            " SELECT DISTINCT " + this.TableColumnsMainTable() + ", '" + this._SourceView + "' AS SourceView " +
                //            " FROM " + this._SourceView +
                //            " WHERE NOT EXISTS (SELECT * FROM " + this.MainTable() + " T INNER JOIN " + this._SourceView + " V " +
                //            " ON T.BaseURL = V.BaseURL AND T.BaseURL = @BaseURL AND T." + this.PrimaryKey() + " = V." + this.PrimaryKey() + "); " +
                //            "INSERT INTO " + this.MainTable() + "SourceView (BaseURL, " + this.PrimaryKey() + ", SourceView) " +
                //            " SELECT DISTINCT BaseURL, " + this.PrimaryKey() + ", '" + this._SourceView + "' AS SourceView " +
                //            " FROM " + this._SourceView + "; ";
                //        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors))
                //        {
                //            SQL = "SELECT COUNT(*) FROM " + this.MainTable();
                //            Report = "\r\nTransferred data:\r\n" + DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message) + "\t" + this.MainTable();
                //            if (this.TransferSubsets(ref Message, ref Report, /*ProcessOnly,*/ InterfaceCacheDB))
                //            {
                //                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                //                    this.setCacheDatabaseControls();
                //            }
                //        }
                //        else
                //        {
                //            Message += Errors;
                //        }
                //        break;
                //}


#endregion

                if (false) // alte Version 
                {

                    SQL = "DECLARE @BaseURL varchar(500); " +
                    "SET @BaseURL = (SELECT MIN(BaseURL) FROM " + this._SourceView + "); " +
                        "DELETE T FROM " + this.MainTable() + "SourceView T WHERE SourceView = '" + this._SourceView + "'; " +
                        "DELETE T FROM " + this.MainTable() + " T WHERE SourceView = '" + this._SourceView + "'; ";
                    //" +
                    //" AND NOT EXISTS (SELECT * FROM " + this.MainTable() + " T INNER JOIN " + this.MainTable() + "SourceView S " +
                    //" ON T." + this.PrimaryKey() + " = S." + this.PrimaryKey() +
                    //" AND T.BaseURL = S.BaseURL " +
                    //" AND T.BaseURL = @BaseURL " +
                    //" AND S.SourceView <> '" + this._SourceView + "'); ";
                    //switch(this._TypeOfSource)
                    //{
                    //    case TypeOfSource.Plots:
                    //        SQL += "DELETE T FROM " + this.MainTable() + " T WHERE SourceView = '" + this._SourceView.Replace("SamplingPlots", "Plots") + "'; ";
                    //        break;
                    //    case TypeOfSource.ScientificTerms:
                    //        SQL += "DELETE T FROM " + this.MainTable() + " T WHERE SourceView = '" + this._SourceView.Replace("ScientificTerms", "Terms") + "'; ";
                    //        break;
                    //    case TypeOfSource.Taxa:
                    //        SQL += "DELETE T FROM " + this.MainTable() + " T WHERE SourceView = '" + this._SourceView.Replace("TaxonNames", "Names") + "'; ";
                    //        break;
                    //}
                    SQL += "INSERT INTO " + this.MainTable() + " (" + this.TableColumnsMainTable() + ", SourceView) " +
                     " SELECT DISTINCT " + this.TableColumnsMainTable() + ", '" + this._SourceView + "' AS SourceView " +
                     " FROM " + this._SourceView + " AS T " +
                     " WHERE NOT EXISTS (SELECT * FROM " + this.MainTable() + " M INNER JOIN " + this._SourceView + " V " +
                     " ON M.BaseURL = V.BaseURL AND M.BaseURL = @BaseURL AND M." + this.PrimaryKey() + " = V." + this.PrimaryKey() + "" +
                     " WHERE T." + this.PrimaryKey() + " = M." + this.PrimaryKey() + " AND T.BaseURL = M.BaseURL) ";
                    if (SqlRestriction.Length > 0)
                        SQL += " AND " + SqlRestriction;
                    SQL += "; " +
                        "INSERT INTO " + this.MainTable() + "SourceView (BaseURL, " + this.PrimaryKey() + ", SourceView) " +
                        " SELECT DISTINCT BaseURL, " + this.PrimaryKey() + ", '" + this._SourceView + "' AS SourceView " +
                        " FROM " + this._SourceView;
                    if (SqlRestriction.Length > 0)
                        SQL += " T WHERE " + SqlRestriction;
                    SQL += "; ";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors))
                    {
                        SQL = "SELECT COUNT(*) FROM " + this.MainTable();
                        Report = "\r\nTransferred data:\r\n" + DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message) + "\t" + this.MainTable();
                        if (this.TransferSubsets(ref Message, ref Report, /*ProcessOnly,*/ InterfaceCacheDB))
                        {
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                this.setCacheDatabaseControls();
                        }
                    }
                    else
                    {
                        Message += Errors;
                    }
                }
                else // neue Version - erst SourceView dann Haupttabelle dann Subsets - alle werden ersetzt
                {
                    if (OK)
                    {
                        // getting the possible conflicts: A dataset may be removed while still included in another project in the source and has to be moved to this project
                        // these are stored in a local table
                        SQL = "SELECT DISTINCT C.SourceView " +
                            " FROM " + this.MainTable() + "SourceView T INNER JOIN " + this.MainTable() + "SourceView C " +
                            " ON T." + this.PrimaryKey() + " = C." + this.PrimaryKey() +
                            " AND T.BaseURL = C.BaseURL AND T.BaseURL = @BaseURL " +
                            " AND T.SourceView = '" + this._SourceView + "' AND C.SourceView <> T.SourceView;";
                        System.Data.DataTable dtConflicts = new DataTable();
                        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtConflicts, ref Message);

                        // Getting the BaseURL
                        SQL = "SELECT MIN(BaseURL) FROM " + this._SourceView + "; ";
                        string BaseURL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);

                        // getting the conflicts
                        SQL = "BEGIN TRY CREATE TABLE #Conflicts (BaseURL varchar(500) NULL, ID int NULL, SourceView varchar(128) NULL); END TRY BEGIN CATCH TRUNCATE TABLE #Conflicts; END CATCH; " +
                            "INSERT INTO #Conflicts (BaseURL, ID,  SourceView) " +
                            "SELECT '" + BaseURL + "', C." + this.PrimaryKey() + ", MIN(C.SourceView) " +
                            "FROM " + this.MainTable() + "SourceView T INNER JOIN " + this.MainTable() + "SourceView C " +
                            " ON T." + this.PrimaryKey() + " = C." + this.PrimaryKey() +
                            " AND T.BaseURL COLLATE DATABASE_DEFAULT = C.BaseURL COLLATE DATABASE_DEFAULT AND T.BaseURL = '" + BaseURL + "' " +
                            " AND T.SourceView = '" + this._SourceView + "' AND C.SourceView COLLATE DATABASE_DEFAULT <> T.SourceView COLLATE DATABASE_DEFAULT " +
                            " AND NOT EXISTS (SELECT * FROM " + this._SourceView + " AS M WHERE M." + this.PrimaryKey() + " = T." + this.PrimaryKey() + " AND M.BaseURL COLLATE DATABASE_DEFAULT = T.BaseURL COLLATE DATABASE_DEFAULT AND T.SourceView = '" + this._SourceView + "')" +
                            " GROUP BY C." + this.PrimaryKey() + "; ";

                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, true))
                        {
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                            {
                                Message = "Transfer failed - could not create conflict table";
                                System.Windows.Forms.MessageBox.Show(Message);
                                InterfaceCacheDB.ShowTransferState(Message);

                            }
                            return false;
                        }

                        // Removing data from the source view table
                        SQL = "DELETE T FROM " + this.MainTable() + "SourceView T WHERE SourceView = '" + this._SourceView + "'; ";
                        // Saving remote results in temp local table

                        // inserting data into source view
                        SQL += "INSERT INTO " + this.MainTable() + "SourceView (BaseURL, " + this.PrimaryKey() + ", SourceView) " +
                            " SELECT DISTINCT BaseURL, " + this.PrimaryKey() + ", '" + this._SourceView + "' AS SourceView " +
                            " FROM " + this._SourceView;
                        if (SqlRestriction.Length > 0)
                            SQL += " T WHERE " + SqlRestriction;
                        SQL += "; ";

                        // moving conflicts to alternative
                        SQL += "UPDATE T SET T.SourceView = C.SourceView " +
                            "FROM " + this.MainTable() + " T INNER JOIN #Conflicts C " +
                            " ON T." + this.PrimaryKey() + " = C.ID" +
                            " AND T.BaseURL COLLATE DATABASE_DEFAULT = C.BaseURL COLLATE DATABASE_DEFAULT AND T.BaseURL = '" + BaseURL + "' " +
                            " AND T.SourceView = '" + this._SourceView + "' AND C.SourceView COLLATE DATABASE_DEFAULT <> T.SourceView COLLATE DATABASE_DEFAULT; ";
                        // Removing data from the main table
                        SQL += "DELETE T FROM " + this.MainTable() + " T INNER JOIN " + this.MainTable() + "SourceView SV " +
                                "ON T." + this.PrimaryKey() + " = SV." + this.PrimaryKey() + " AND T.BaseURL COLLATE DATABASE_DEFAULT = SV.BaseURL COLLATE DATABASE_DEFAULT " +
                                "AND SV.SourceView = '" + this._SourceView + "'; ";
                        // Markus 24.7.24
                        // Cache the remote data from the view in a local temporary table
                        SQL += " SELECT DISTINCT " + this.TableColumnsMainTable() + ", '" + this._SourceView + "' AS SourceView INTO #" + this._SourceView + "_Table " +
                         " FROM " + this._SourceView + " AS T ";
                        // Inserting the new data into main table and avoiding conflicts (that should not happen any more)
                        SQL += "INSERT INTO " + this.MainTable() + " (" + this.TableColumnsMainTable() + ", SourceView) " +
                         " SELECT DISTINCT " + this.TableColumnsMainTable() + ", '" + this._SourceView + "' AS SourceView " +
                         " FROM #" + this._SourceView + "_Table AS T " +
                         " WHERE NOT EXISTS (SELECT * FROM " + this.MainTable() + " M INNER JOIN " + this._SourceView + " V " +
                         " ON M.BaseURL COLLATE DATABASE_DEFAULT = V.BaseURL COLLATE DATABASE_DEFAULT AND M.BaseURL = '" + BaseURL + "' AND M." + this.PrimaryKey() + " = V." + this.PrimaryKey() + "" +
                         " WHERE T." + this.PrimaryKey() + " = M." + this.PrimaryKey() + " AND T.BaseURL COLLATE DATABASE_DEFAULT = M.BaseURL COLLATE DATABASE_DEFAULT) ";
                        if (SqlRestriction.Length > 0)
                            SQL += " AND " + SqlRestriction;
                        SQL += "; DROP TABLE #" + this._SourceView + "_Table ";

                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Message, ref Errors, true))
                        {
                            // Removing subsets - rely on SourceView
                            OK = this.TransferSubsets(ref Message, ref Report, InterfaceCacheDB);
                            if (OK)
                            {
                                SQL = "SELECT COUNT(*) FROM " + this.MainTable() + "SourceView;";
                                Report = "\r\nTransferred data:\r\n" + DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message) + "\t" + this.MainTable();
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                    this.setCacheDatabaseControls();
                            }
                        }
                        else
                        {
                            Message += Errors;
                        }
                    }
                }


                if (Message.Length > 0)
                {
                    OK = false;
                    Message = "Transfer failed: " + Message;
                    this.WriteTransferErrors(Message);
                    if (InterfaceCacheDB != null)
                        InterfaceCacheDB.ShowTransferState(Message);
#if DEBUG
                    System.Windows.Forms.MessageBox.Show(Message);
#endif
                }
                else
                {
                    Message = "Data transferred";
                    this.WriteTransferProtocol(Report, true);
                    if (InterfaceCacheDB != null)
                        InterfaceCacheDB.ShowTransferState(Message);
                }

                Message += "\r\nto MS-SQL Server Database " + DiversityCollection.CacheDatabase.CacheDB.DatabaseName +
                    "\r\non Server " + DiversityCollection.CacheDatabase.CacheDB.DatabaseServer;
                this.WriteReport(Message, Report);

                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    System.Windows.Forms.MessageBox.Show(Message);
                else if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
                {
                    CacheDB.LogEvent(this.Name.ToString(), "TransferToCache(bool ProcessOnly, ref string Report)", Message);
                    //DiversityWorkbench.ExceptionHandling.WriteToEventLog(this.Name.ToString(), "TransferToCache(bool ProcessOnly, ref string Report)", Message, DiversityWorkbench.Settings.ModuleName);
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                DiversityCollection.CacheDatabase.CacheDB.SetTransferFinished(
                    this.SourcesTable(), false, this._SourceView, Report, Errors);
                this.WriteHistory(HistoryTarget.DataToCache, null);
            }
            return OK;
        }


        private string SqlRestrictionImport(InterfaceCacheDatabase InterfaceCacheDB, ref string Message)
        {
            string SQL = "";
            switch (this._TypeOfSource)
            {
                case TypeOfSource.Taxa:
                    Message = "Checking for pro parte synonyms";
                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        this.TransferSetMessage(Message);
                    if (InterfaceCacheDB != null)
                        InterfaceCacheDB.ShowTransferState(Message);
                    SQL = "SELECT T.NameID FROM " + this._SourceView + " T group by  T.NameID having count(*) > 1";
                    System.Data.DataTable dtProParte = new DataTable();
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtProParte, ref Message);
                    string ProParteIDs = "";
                    if (dtProParte.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dtProParte.Rows)
                        {
                            if (ProParteIDs.Length > 0) ProParteIDs += ", ";
                            ProParteIDs += R[0].ToString();
                        }
                        SQL = " T." + this.PrimaryKey() + " NOT IN (" + ProParteIDs + ")";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                        {
                            this.TransferSetMessage("ProParte: " + dtProParte.Rows.Count.ToString());
                            System.Windows.Forms.MessageBox.Show("The following NameIDs contained pro parte synonymy and where reduced to the first relation: " + ProParteIDs);
                        }
                        this._TransferHistory.Add("ProParte", dtProParte.Rows.Count);
                        Message += "\r\n" + dtProParte.Rows.Count.ToString() + " ProParte Synonmys\r\n";
                    }
                    else
                        SQL = "";
                    break;
            }
            return SQL;
        }

        private string TableColumnsMainTable()
        {
            if (this.MainTable() == this.MainTableAgents)
                return TableColumns(SubsetTable.Agent);
            else if (this.MainTable() == this.MainTableGazetteer)
                return TableColumns(SubsetTable.Gazetteer);
            else if (this.MainTable() == this.MainTablePlots)
                return TableColumns(SubsetTable.SamplingPlot);
            else if (this.MainTable() == this.MainTableReference)
                return TableColumns(SubsetTable.ReferenceTitle);
            else if (this.MainTable() == this.MainTableScientificTerms)
                return TableColumns(SubsetTable.ScientificTerm);
            else if (this.MainTable() == this.MainTableTaxa)
                return TableColumns(SubsetTable.TaxonSynonymy);

            return "";
        }

        private string TableColumns(string View, string Target, string Alias)
        {
            string SQL = "";
            string SqlColumns = "select v.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS v, INFORMATION_SCHEMA.COLUMNS t " +
                "where v.TABLE_NAME = '" + View + "' " +
                "and t.TABLE_NAME = '" + Target + "' " +
                "and v.COLUMN_NAME = t.COLUMN_NAME";
            System.Data.DataTable dtV = new DataTable();
            string Message = "";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SqlColumns, ref dtV, ref Message);
            if (dtV.Rows.Count == 0)
            {
                string SqlTest = "select count(*) from INFORMATION_SCHEMA.COLUMNS v where v.TABLE_NAME = '" + View + "'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlTest);
                if (Result == "0" || Result == "")
                {
                    SQL = "";
                    System.Windows.Forms.MessageBox.Show("The view " + View + " is missing.\r\nSource either must be recreated or may need an update to the current version");
                }
            }
            else
            {
                foreach (System.Data.DataRow R in dtV.Rows)
                {
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += Alias + ".[" + R[0].ToString() + "]";
                }
            }
            return SQL;
        }

        private string TableColumns(SubsetTable Table, string View = "", string Target = "", string Alias = "")
        {
            string SQL = "";
            if (View.Length > 0 && Target.Length > 0 && Alias.Length > 0)
            {
                string SqlColumns = "select v.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS v, INFORMATION_SCHEMA.COLUMNS t " +
                    "where v.TABLE_NAME = '" + View + "' " +
                    "and t.TABLE_NAME = '" + Target + "' " +
                    "and v.COLUMN_NAME = t.COLUMN_NAME";
                System.Data.DataTable dtV = new DataTable();
                string Message = "";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SqlColumns, ref dtV, ref Message);
                foreach(System.Data.DataRow R in dtV.Rows)
                {
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += Alias + ".[" + R[0].ToString() + "]";
                }
                return SQL;
            }
            switch (Table)
            {
                case SubsetTable.Agent:
                    SQL = "BaseURL, AgentID, AgentParentID, AgentName, AgentTitle, GivenName, GivenNamePostfix, InheritedNamePrefix, InheritedName, " +
                            "InheritedNamePostfix, Abbreviation, AgentType, AgentRole, AgentGender, Description, OriginalSpelling, Notes, ValidFromDate, ValidUntilDate, SynonymToAgentID, ProjectID";
                    break;
                case SubsetTable.AgentContactInformation:
                    SQL = "AgentID, DisplayOrder, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, Notes, ValidFrom, ValidUntil";
                    break;
                case SubsetTable.AgentImage:
                    SQL = "AgentID, URI, Type, Sequence, Description";
                    break;
#if AgentIdentifierIncluded
                case SubsetTable.AgentIdentifier:
                    SQL = "AgentID, Identifier, IdentifierURI, Type, Notes";
                    break;
#endif
                case SubsetTable.Gazetteer:
                    SQL = "BaseURL, NameID, Name, LanguageCode, PlaceID, PlaceType, PreferredName, PreferredNameID, PreferredNameLanguageCode, ExternalNameID, ExternalDatabaseID, ProjectID";
                    break;
                case SubsetTable.GazetteerExternalDatabase:
                    SQL = "ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, ExternalAttribute_NameID, ExternalAttribute_PlaceID, ExternalCoordinatePrecision";
                    break;
                case SubsetTable.ReferenceRelator:
                    SQL = "RefID, Role, Sequence, Name, AgentURI, SortLabel, Address";
                    break;
                case SubsetTable.ReferenceTitle:
                    SQL = "BaseURL, RefType, RefID, RefDescription_Cache, Title, DateYear, DateMonth, DateDay, DateSuppl, SourceTitle, SeriesTitle, Periodical, Volume, Issue, Pages, Publisher, PublPlace, Edition, DateYear2, " +
                            "DateMonth2, DateDay2, DateSuppl2, ISSN_ISBN, Miscellaneous1, Miscellaneous2, Miscellaneous3, UserDef1, UserDef2, UserDef3, UserDef4, UserDef5, WebLinks, LinkToPDF, LinkToFullText, RelatedLinks,  " +
                            "LinkToImages, SourceRefID, Language, CitationText, CitationFrom, ProjectID";
                    break;
                case SubsetTable.SamplingPlot:
                    SQL = "BaseURL, PlotID, PartOfPlotID, PlotIdentifier, PlotDescription, PlotGeography_Cache, PlotType, CountryCache, ProjectID";
                    break;
                case SubsetTable.SamplingPlotLocalisation:
                    SQL = "PlotID, LocalisationSystemID, Location1, Location2, LocationAccuracy, LocationNotes, Geography, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache";
                    break;
                case SubsetTable.SamplingPlotProperty:
                    SQL = "PlotID, PropertyID, DisplayText, PropertyURI, PropertyHierarchyCache, PropertyValue, Notes, AverageValueCache";
                    break;
                case SubsetTable.ScientificTerm:
                    // Markus 28.3.25: #49 - ExternalID added
                    SQL = "BaseURL, RepresentationID, RepresentationURI, DisplayText, HierarchyCache, HierarchyCacheDown, RankingTerm, ExternalID";
                    break;
                case SubsetTable.TaxonAnalysis:
                    SQL = "NameID, ProjectID, AnalysisID, AnalysisValue, Notes";
                    break;
                case SubsetTable.TaxonAnalysisCategory:
                    SQL = "AnalysisID, AnalysisParentID, DisplayText, [Description], AnalysisURI, ReferenceTitle, ReferenceURI, SortingID";
                    break;
                case SubsetTable.TaxonAnalysisCategoryValue:
                    SQL = "AnalysisID, AnalysisValue, [Description], DisplayText, DisplayOrder, Notes";
                    break;
                case SubsetTable.TaxonCommonName:
                    SQL = "NameID, CommonName, LanguageCode, CountryCode";
                    break;
                case SubsetTable.TaxonList:
                    SQL = "ProjectID, Project, DisplayText";
                    break;
                case SubsetTable.TaxonNameExternalDatabase:
                    SQL = "ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, ExternalDatabaseURI, ExternalDatabaseInstitution, ExternalAttribute_NameID";
                    break;
                case SubsetTable.TaxonNameExternalID:
                    SQL = "NameID, ExternalDatabaseID, ExternalNameURI";
                    break;
                case SubsetTable.TaxonSynonymy:
                    SQL = "NameID, BaseURL, TaxonName, AcceptedNameID, AcceptedName, TaxonomicRank, SpeciesGenusNameID, GenusOrSupragenericName, " +
                         "TaxonNameSinAuthor, AcceptedNameSinAuthor, ProjectID";
                    break;
            }
            if (Alias.Length > 0)
            {
                SQL = SQL.Replace(", ", ", " + Alias + ".");
            }
            return SQL;
        }

        private bool TransferSubsets(ref string Message, ref string Report, InterfaceCacheDatabase InterfaceCacheDB, bool RelyOnSourceView = true)
        {
            bool OK = true;
            try
            {
                string SQL = "SELECT MIN(BaseURL) FROM " + this._SourceView + "; ";
                string BaseURL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);

                foreach (System.Collections.Generic.KeyValuePair<SubsetTable, string> KV in this.TransferredSubsets())
                {
                    if (this.SubsetRoutines().Contains(KV.Key))
                    {
                        SQL = "exec dbo." + KV.Key.ToString() + " '" + this._SourceView + "'";
                        if (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                            OK = false;
                    }
                    else
                    {
                        string FieldList = this.TableColumns(this._SourceView + KV.Value, KV.Key.ToString(), "V");// "";
                        // Markus 21.7.22: empty field list e.g. if view is still missing
                        if (FieldList.Length == 0)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("CacheDB - LookupSource - TransferSubsets(ref string Message, ref string Report, InterfaceCacheDatabase InterfaceCacheDB, bool RelyOnSourceView = true)", this._SourceView + KV.Value, "No fields for view found");
                            continue;
                        }

                        string InterfaceMessage = "Transfer " + KV.Key.ToString();
                        if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                            this.TransferSetMessage(InterfaceMessage);
                        if (InterfaceCacheDB != null)
                            InterfaceCacheDB.ShowTransferState(InterfaceMessage);

                        // Markus 25.2.2021 - Tables not linked to main table should be imported with distinct to avoid duplicates
                        bool IsLinkedToMainTable = true;

                        // Markus 24.08.2020: getting pk to main table
                        System.Collections.Generic.Dictionary<string, string> PkMaintable = new Dictionary<string, string>();
                        string SqlPK = "select c.COLUMN_NAME, c.COLLATION_NAME " +
                            "from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE u inner join INFORMATION_SCHEMA.COLUMNS c on u.COLUMN_NAME = c.COLUMN_NAME and u.TABLE_NAME = c.TABLE_NAME " +
                            "where c.TABLE_NAME = '" + this.MainTable() + "' " +
                            "and u.CONSTRAINT_NAME like 'PK_%'";
                        System.Data.DataTable dtPK = new DataTable();
                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SqlPK, ref dtPK, ref Message)
                            && dtPK.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow R in dtPK.Rows)
                            {
                                if (!PkMaintable.ContainsKey(R[0].ToString()))
                                    PkMaintable.Add(R[0].ToString(), R[1].ToString());
                            }
                        }
                        foreach(System.Collections.Generic.KeyValuePair<string, string> PK in PkMaintable)
                        {
                            if (FieldList.IndexOf("[" + PK.Key + "]") == -1)
                            {
                                IsLinkedToMainTable = false;
                                break;
                            }
                        }

                        if (IsLinkedToMainTable)
                        {
                            // moving conflicts to alternative
                            SQL = "UPDATE T SET T.SourceView = C.SourceView " +
                                "FROM " + KV.Key + " T INNER JOIN #Conflicts C " +
                                " ON T." + this.PrimaryKey() + " = C.ID " +
                                " AND T.BaseURL COLLATE DATABASE_DEFAULT = C.BaseURL COLLATE DATABASE_DEFAULT AND T.BaseURL = '" + BaseURL + "' " +
                                " AND T.SourceView = '" + this._SourceView + "' AND C.SourceView COLLATE DATABASE_DEFAULT <> T.SourceView COLLATE DATABASE_DEFAULT; ";
                        }
                        else
                            SQL = "";

                        SQL += "DELETE T FROM " + KV.Key + " T WHERE SourceView = '" + this._SourceView + "';";
                        // Markus 24.7.24
                        // Creating local temp table for remote view
                        SQL += " SELECT ";
                        if (!IsLinkedToMainTable)
                            SQL += " DISTINCT ";
                        SQL += FieldList + " INTO #" + this._SourceView + KV.Value + "_Table " +
                        " FROM " + this._SourceView + KV.Value + " AS V "; 

                        SQL += " INSERT INTO " + KV.Key + " (" + FieldList + ", SourceView) " +
                        " SELECT ";
                        if (!IsLinkedToMainTable)
                            SQL += " DISTINCT ";
                        SQL += FieldList + ", '" + this._SourceView + "' " +
                        " FROM #" + this._SourceView + KV.Value + "_Table AS V ";

                        // getting PK of target table
                        dtPK.Clear();
                        System.Collections.Generic.Dictionary<string, string> PkSubsetTable = new Dictionary<string, string>();
                        SqlPK = "select c.COLUMN_NAME, c.COLLATION_NAME " +
                            "from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE u inner join INFORMATION_SCHEMA.COLUMNS c on u.COLUMN_NAME = c.COLUMN_NAME and u.TABLE_NAME = c.TABLE_NAME " +
                            "where c.TABLE_NAME = '" + KV.Key + "' " +
                            "and (u.CONSTRAINT_NAME like 'PK[_]%' OR u.CONSTRAINT_NAME like '%[_]PK')";
                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SqlPK, ref dtPK, ref Message)
                            && dtPK.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow R in dtPK.Rows)
                            {
                                if (!PkSubsetTable.ContainsKey(R[0].ToString()))
                                    PkSubsetTable.Add(R[0].ToString(), R[1].ToString());
                            }
                        }

                        //Markus 20.07.2020: getting PK for comparision with existing data
                        if (PkSubsetTable.Count > 0)
                        {
                            SqlPK = "";
                            foreach (System.Collections.Generic.KeyValuePair<string, string> PK in PkSubsetTable)
                            {
                                if (SqlPK.Length > 0)
                                    SqlPK += " AND ";
                                SqlPK += " V." + PK.Key;
                                if (PK.Value.Length > 0)
                                    SqlPK += " COLLATE DATABASE_DEFAULT";
                                SqlPK += " = T." + PK.Key;
                                if (PK.Value.Length > 0)
                                    SqlPK += " COLLATE DATABASE_DEFAULT";
                            }
                        }
                        else
                        {
                            // checking existence of table
                            string SqlExistence = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_NAME = '" + KV.Key + "'";
                            string iCount = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SqlExistence);
                            if (iCount == "0")
                                continue;
                        }



                        // getting link to main table only if possible
                        System.Collections.Generic.List<string> LinkColumns = new List<string>();
                        foreach(System.Collections.Generic.KeyValuePair<string, string> PK in PkMaintable)
                        {
                            if (PkSubsetTable.ContainsKey(PK.Key) && !LinkColumns.Contains(PK.Key))
                                LinkColumns.Add(PK.Key);
                        }
                        if (LinkColumns.Count > 0)
                        {
                            SQL += " INNER JOIN " + this.MainTable() + " AS M ON ";
                            for (int i = 0; i < LinkColumns.Count; i++)
                            {
                                if (i > 0)
                                    SQL += " AND ";
                                SQL += " V." + LinkColumns[i];
                                if (PkMaintable[LinkColumns[i]].Length > 0)
                                    SQL += " COLLATE DATABASE_DEFAULT";
                                SQL += " = M." + LinkColumns[i];
                                if (PkMaintable[LinkColumns[i]].Length > 0)
                                    SQL += " COLLATE DATABASE_DEFAULT";
                            }
                        }

                        //Markus 20.07.2020: getting PK for comparision with existing data
                        if(PkSubsetTable.Count > 0)
                        {
                            SqlPK = "";
                            foreach(System.Collections.Generic.KeyValuePair<string, string> PK in PkSubsetTable)
                            {
                                if (SqlPK.Length > 0)
                                    SqlPK += " AND ";
                                SqlPK += " V." + PK.Key;
                                if (PK.Value.Length > 0)
                                    SqlPK += " COLLATE DATABASE_DEFAULT";
                                SqlPK += " = T." + PK.Key;
                                if (PK.Value.Length > 0)
                                    SqlPK += " COLLATE DATABASE_DEFAULT";
                            }
                            SQL += " WHERE NOT EXISTS (SELECT * FROM " + KV.Key + " AS T WHERE " + SqlPK + ")";
                        }
                        else
                        {

                        }
                        // Markus 24.7.24: Removing temp table
                        SQL += "; DROP TABLE #" + this._SourceView + KV.Value + "_Table; ";

                        string Result = "";
                        if(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlTransactionNonQueryInCacheDB(SQL, ref Result, ref Message, true))
                        {
                            if (Result.Length > 0 || Message.Length > 0) // (!DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                            {
                                OK = false;
                                this.WriteTransferErrors(SQL + "\r\n" + Message + Result);
                                if (this._TransferHistory.ContainsKey(KV.Key.ToString()))
                                    this._TransferHistory[KV.Key.ToString()] += "\r\n" + Message + Result;
                                else
                                    this._TransferHistory.Add(KV.Key.ToString(), Message + Result);
                            }
                            else
                            {
                                SQL = "SELECT COUNT(*) FROM " + KV.Key;
                                string NumberInTable = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Message);
                                Report += "\r\n" + NumberInTable + "\t" + KV.Key;
                                if (this._TransferHistory.ContainsKey(KV.Key.ToString()))
                                {
                                    if (this._TransferHistory[KV.Key.ToString()].ToString() != NumberInTable)
                                        this._TransferHistory[KV.Key.ToString()] += "\r\nCount: " + int.Parse(NumberInTable);

                                }
                                else
                                    this._TransferHistory.Add(KV.Key.ToString(), int.Parse(NumberInTable));
                            }
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show(Message);
                        }
                        if (InterfaceCacheDB != null)
                        {
                            if (Message.Length > 0)
                                InterfaceCacheDB.ShowTransferState("Transfer failed");
                            else
                                InterfaceCacheDB.ShowTransferState(Result + " transferred");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        private void TransferSetMessage(string Message)
        {
            this.labelCountCacheDB.Text = Message;
            Application.DoEvents();
        }

#endregion

#region Check for updates

        private bool DataAreUpdatedInSource(/*bool ProcessOnly, */ref string Report)
        {
            bool DataAreUpdated = false;
            try
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    CacheDB.LogEvent(this.Name.ToString(), "DataAreUpdatedInSource(bool ProcessOnly, ref string Report)", this._SourceView + ": Checking for update");
                    //DiversityWorkbench.ExceptionHandling.WriteToEventLog(this.Name.ToString(), "DataAreUpdatedInSource(bool ProcessOnly, ref string Report)", this._SourceView + ": Checking for update", DiversityWorkbench.Settings.ModuleName);
                System.DateTime LastUpdateInSource = System.DateTime.Now;
                System.DateTime LastTransfer = System.DateTime.Now;
                string Message = "";
                string MainSourceTable = ""; ;

                switch (this._TypeOfSource)
                {
                    case TypeOfSource.Taxa:
                        MainSourceTable = this.MainTableTaxa;
                        break;
                    case TypeOfSource.ScientificTerms:
                        MainSourceTable = this.MainTableScientificTerms;
                        break;
                    case TypeOfSource.Gazetteer:
                        MainSourceTable = this.MainTableGazetteer;
                        break;
                    case TypeOfSource.Agents:
                        MainSourceTable = this.MainTableAgents;
                        break;
                    case TypeOfSource.References:
                        MainSourceTable = this.MainTableReference;
                        break;
                    case TypeOfSource.Plots:
                        MainSourceTable = this.MainTablePlots;
                        break;
                }
                // Check if any data for this view had been imported so far
                string SQL = "SELECT COUNT(*) FROM " + MainSourceTable + " T WHERE T.SourceView = '" + this._SourceView + "'"; ;
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result == "0")
                {
                    if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    {
                        CacheDB.LogEvent(this.Name.ToString(), "TransferToCache(bool ProcessOnly, ref string Report)", "New transfer of data");
                        //DiversityWorkbench.ExceptionHandling.WriteToEventLog(this.Name.ToString(), "TransferToCache(bool ProcessOnly, ref string Report)", "New transfer of data", DiversityWorkbench.Settings.ModuleName);
                        Report += "New transfer of data";
                    }
                    return true;
                }
                // Get the last date for the transfer
                SQL = "SELECT LastUpdatedWhen FROM " + this.SourcesTable() + " WHERE SourceView = '" + this._SourceView + "'";
                if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastTransfer))
                {
                    // get the last data of changes in the source
                    SQL = "SELECT MAX(T.LogUpdatedWhen) FROM " + this._SourceView + " T";
                    string Error = "";
                    string UpdateInSource = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, ref Error);
                    if (Error.Length > 0)
                    {
                        Message = "\r\nSource " + this._SourceView + " may be out of date. Please reinstall this source. Error detected: " + Error;
                    }
                    else
                    {
                        if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastUpdateInSource))
                        {
                            // compare the dates
                            if (LastUpdateInSource.CompareTo(LastTransfer) < 0)
                            {
                                if (DataAreUpdatedInSourceSubsets(ref Message, ref Report, LastTransfer))
                                    DataAreUpdated = true;
                            }
                            else
                                DataAreUpdated = true;
                        }
                    }
                }
                else
                {
                    // the query above failed - try to get the date of the last transfer from the main table for the source
                    SQL = "SELECT MAX(T.LogInsertedWhen) FROM " + MainSourceTable + " T WHERE T.SourceView = '" + this._SourceView + "'";
                    if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastTransfer))
                    {
                        SQL = "SELECT MAX(T.LogUpdatedWhen) FROM " + this._SourceView + " T";
                        if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastUpdateInSource))
                        {
                            if (LastUpdateInSource.CompareTo(LastTransfer) < 0)
                            {
                                if (DataAreUpdatedInSourceSubsets(ref Message, ref Report, LastTransfer))
                                    DataAreUpdated = true;
                            }
                            else
                                DataAreUpdated = true;
                        }
                    }
                }

                if (Message.Length > 0)
                {
                    DataAreUpdated = false;
                    Message = "Could not check for update: " + Message;
                }
                else if (!DataAreUpdated)
                    Message = "No data transfer needed.\r\nData in cache database are up to date.\r\nSource:\t" + LastUpdateInSource.ToShortDateString() + "\r\nCache:\t" + LastTransfer.ToShortDateString();

                this.WriteReport(Message, Report);

                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    System.Windows.Forms.MessageBox.Show(Message);
                else
                {
                    CacheDB.LogEvent(this.Name.ToString(), "TransferToCache(bool ProcessOnly, ref string Report)", Message);
                    //DiversityWorkbench.ExceptionHandling.WriteToEventLog(this.Name.ToString(), "TransferToCache(bool ProcessOnly, ref string Report)", Message, DiversityWorkbench.Settings.ModuleName);
                }
            }
            catch (System.Exception ex)
            {
                DataAreUpdated = false;
            }
            return DataAreUpdated;
        }

        private bool DataAreUpdatedInSourceSubsets(ref string Message, ref string Report, System.DateTime LastTransfer)
        {
            bool OK = false;
            System.DateTime LastUpdateInSource;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<SubsetTable, string> KV in this.TransferredSubsets())
                {
                    string SQL = "SELECT MAX(T.LogUpdatedWhen) FROM " + this._SourceView + KV.Value + " T";
                    if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastUpdateInSource))
                    {
                        if (LastUpdateInSource.CompareTo(LastTransfer) > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }
        
#endregion

        private void buttonViewCacheDB_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Table = "";
            switch (this._TypeOfSource)
            {
                case TypeOfSource.ScientificTerms:
                    Table = "ScientificTerm";
                    break;
                case TypeOfSource.Taxa:
                    if (this._IsWebservice)
                    {
                        Table = "TaxonSynonymy";
                    }
                    else
                    {
                        Table = "Taxon";
                    }
                    break;
                case TypeOfSource.Agents:
                    Table = "Agent";
                    break;
                case TypeOfSource.Gazetteer:
                    Table = "Gazetteer";
                    break;
                case TypeOfSource.References:
                    Table = "Reference";
                    break;
                case TypeOfSource.Plots:
                    Table = "SamplingPlot";
                    break;
            }
            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(false, "dbo", Table, "SourceView", "=", this._SourceView);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            f.ShowDialog();


            //switch (this._TypeOfSource)
            //{
            //    case TypeOfSource.ScientificTerms:
            //        DiversityCollection.CacheDatabase.FormViewContent fTerm = new FormViewContent(false, "dbo", "ScientificTerm", "SourceView", "=", this._SourceView);
            //        fTerm.ShowDialog();
            //        break;
            //    case TypeOfSource.Taxa:
            //        if (this._IsWebservice)
            //        {
            //            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(false, "dbo", "TaxonSynonymy", "SourceView", "=", this._SourceView);
            //            f.ShowDialog();
            //        }
            //        else
            //        {
            //            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(false, "dbo", "Taxon", "SourceView", "=", this._SourceView);
            //            f.ShowDialog();
            //        }
            //        break;
            //    case TypeOfSource.Agents:
            //        DiversityCollection.CacheDatabase.FormViewContent fAgent = new FormViewContent(false, "dbo", "Agent", "SourceView", "=", this._SourceView);
            //        fAgent.ShowDialog();
            //        break;
            //    case TypeOfSource.Gazetteer:
            //        DiversityCollection.CacheDatabase.FormViewContent fGazetteer = new FormViewContent(false, "dbo", "Gazetteer", "SourceView", "=", this._SourceView);
            //        fGazetteer.ShowDialog();
            //        break;
            //    case TypeOfSource.References:
            //        DiversityCollection.CacheDatabase.FormViewContent fRef = new FormViewContent(false, "dbo", "Reference", "SourceView", "=", this._SourceView);
            //        fRef.ShowDialog();
            //        break;
            //    case TypeOfSource.Plots:
            //        DiversityCollection.CacheDatabase.FormViewContent fPlot = new FormViewContent(false, "dbo", "SamplingPlot", "SourceView", "=", this._SourceView);
            //        fPlot.ShowDialog();
            //        break;
            //}
        }

        private int _CountCacheDB = 0;

        private void setCacheDatabaseControls()
        {
            string SQL = "";
            string Result = "";
            //switch (this._TypeOfSource)
            //{
            //    case TypeOfSource.Taxa:
            //        SQL = "SELECT COUNT(*) FROM TaxonSynonymy WHERE SourceView = '" + this._SourceView + "'";
            //        break;
            //    case TypeOfSource.ScientificTerms:
            //        SQL = "SELECT COUNT(*) FROM ScientificTerm WHERE SourceView = '" + this._SourceView + "'";
            //        break;
            //    case TypeOfSource.Gazetteer:
            //        SQL = "SELECT COUNT(*) FROM Gazetteer WHERE SourceView = '" + this._SourceView + "'";
            //        break;
            //    case TypeOfSource.Agents:
            //        SQL = "SELECT COUNT(*) FROM Agent WHERE SourceView = '" + this._SourceView + "'";
            //        break;
            //    case TypeOfSource.References:
            //        SQL = "SELECT COUNT(*) FROM ReferenceTitle WHERE SourceView = '" + this._SourceView + "'";
            //        break;
            //    case TypeOfSource.Plots:
            //        SQL = "SELECT COUNT(*) FROM SamplingPlot WHERE SourceView = '" + this._SourceView + "'";
            //        break;
            //}
            SQL = "SELECT COUNT(*) FROM " + this.MainTable() + "SourceView WHERE SourceView = '" + this._SourceView + "'";

            Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result != "0" && Result != "")
            {
                this.labelCountCacheDB.Text = "Nr. of datasets: " + Result + "\r\nLast transfer:\r\n";
                //switch (this._TypeOfSource)
                //{
                //    case TypeOfSource.Taxa:
                //        SQL = "SELECT CONVERT(nvarchar(50), MAX(T.LogInsertedWhen), 120) AS LastUpdate FROM TaxonSynonymy T WHERE T.SourceView = '" + this._SourceView + "'";
                //        break;
                //    case TypeOfSource.ScientificTerms:
                //        SQL = "SELECT CONVERT(nvarchar(50), MAX(T.LogInsertedWhen), 120) AS LastUpdate FROM ScientificTerm T WHERE T.SourceView = '" + this._SourceView + "'";
                //        break;
                //    case TypeOfSource.Gazetteer:
                //        SQL = "SELECT CONVERT(nvarchar(50), MAX(T.LogInsertedWhen), 120) AS LastUpdate FROM Gazetteer T WHERE T.SourceView = '" + this._SourceView + "'";
                //        break;
                //    case TypeOfSource.Agents:
                //        SQL = "SELECT CONVERT(nvarchar(50), MAX(T.LogInsertedWhen), 120) AS LastUpdate FROM Agent T WHERE T.SourceView = '" + this._SourceView + "'";
                //        break;
                //    case TypeOfSource.References:
                //        SQL = "SELECT CONVERT(nvarchar(50), MAX(T.LogInsertedWhen), 120) AS LastUpdate FROM ReferenceTitle T WHERE T.SourceView = '" + this._SourceView + "'";
                //        break;
                //    case TypeOfSource.Plots:
                //        SQL = "SELECT CONVERT(nvarchar(50), MAX(T.LogInsertedWhen), 120) AS LastUpdate FROM SamplingPlot T WHERE T.SourceView = '" + this._SourceView + "'";
                //        break;
                //}
                SQL = "SELECT CONVERT(nvarchar(50), MAX(T.LogInsertedWhen), 120) AS LastUpdate FROM " + this.MainTable() + "SourceView T WHERE T.SourceView = '" + this._SourceView + "'";
                var result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                this.labelCountCacheDB.Text += result;
                int.TryParse(Result, out this._CountCacheDB);
            }
            else this.labelCountCacheDB.Text = "No data";

            if (this.Subsets.Count == 0)
                this.buttonSetSubsets.Enabled = false;
        }

        private void buttonSetSubsets_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, bool> Subsets = new Dictionary<string, bool>();
                foreach (System.Collections.Generic.KeyValuePair<SubsetTable, string> KV in this.Subsets)
                {
                    if (this.TransferredSubsets().ContainsKey(KV.Key))
                        Subsets.Add(KV.Key.ToString(), true);
                    else
                        Subsets.Add(KV.Key.ToString(), false);
                }
                DiversityWorkbench.Forms.FormGetSelectionFromCheckedList f = new DiversityWorkbench.Forms.FormGetSelectionFromCheckedList(Subsets, "Transferred subsets", "Please select the additional data that should be transferred");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    string SS = "";
                    System.Data.DataTable dt = f.SelectedItems;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        if (SS.Length > 0) SS += "|";
                        SS += R[0].ToString();
                    }
                    string SQL = "UPDATE S SET Subsets = '" + SS + "' FROM " + this.MainTable() + "Source AS S WHERE SourceView = '" + this._SourceView + "'";
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
                    this._TransferredSubsets = null;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonTransferToCacheFilter_Click(object sender, EventArgs e)
        {
            this.CompareLogDateCacheDB = !this.CompareLogDateCacheDB;
            DiversityCollection.CacheDatabase.CacheDB.initScheduleControls(this.SourcesTable(),
                this._SourceView,
                null,
                this.checkBoxIncludeInTransfer,
                this.buttonTransferToCacheFilter,
                this.buttonTransferSettings,
                this.buttonTransferProtocol,
                this.buttonTransferErrors,
                this.buttonTransferToCache,
                this.labelCountCacheDB,
                this.toolTip);
        }

#region Schedule based transfer

        private void buttonTransferToPostgresFilter_Click(object sender, EventArgs e)
        {
            bool Compare = this.CompareLogDatePostgres;
            this.CompareLogDatePostgres = !Compare;
            DiversityCollection.CacheDatabase.CacheDB.initScheduleControls(
                this.SourcesTable() + "Target", 
                this._SourceView, 
                null, 
                this.checkBoxPostgresIncludeInTransfer, 
                this.buttonTransferToPostgresFilter,
                this.buttonPostgresTransferSettings, 
                this.buttonPostgresTransferProtocol, 
                this.buttonPostgresTransferErrors, 
                this.buttonTransferToPostgres,
                this.labelCountPostgres,
                this.toolTip);
        }

        private void initScheduleTransferControls()
        {
            try
            {
                if (this._IsWebservice)
                {
                    this.ForWebservice_InitControl();
                }
                else
                {
                    DiversityCollection.CacheDatabase.CacheDB.initScheduleControls(
                        this.SourcesTable(),
                        this.SourceView,
                        null,
                        this.checkBoxIncludeInTransfer,
                        this.buttonTransferToCacheFilter,
                        this.buttonTransferSettings,
                        this.buttonTransferProtocol,
                        this.buttonTransferErrors,
                        this.buttonTransferToCache,
                        this.labelCountCacheDB,
                        this.toolTip);
                }
            }
            catch (System.Exception ex) { }
        }

        private void WriteTransferProtocol(string Protocol, bool TransferSuccessful)
        {
            try
            {
                string SQL = "UPDATE P SET P.TransferProtocol = '" + Protocol + "' ";
                if (TransferSuccessful)
                    SQL += ", P.LastUpdatedWhen = getdate() ";
                SQL += " FROM " + this.SourcesTable() + " P WHERE P.SourceView = '" + this._SourceView.ToString() + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ResetTransferErrors()
        {
            try
            {
                string SQL = "UPDATE P SET P.TransferErrors = ''" +
                    " FROM " + this.SourcesTable() + " P WHERE P.SourceView = '" + this._SourceView.ToString() + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void WriteTransferErrors(string Errors)
        {
            try
            {
                string CurrentDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString() 
                    + " " + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + ":" + System.DateTime.Now.Second.ToString();
                string SQL = "UPDATE P SET P.TransferErrors = CASE WHEN P.TransferErrors IS NULL THEN '' ELSE P.TransferErrors END + '\r\n" + CurrentDate + "\r\n" + Errors.Replace("'", "''") + "' " +
                    " FROM " + this.SourcesTable() + " P WHERE P.SourceView = '" + this._SourceView.ToString() + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void checkBoxIncludeInTransfer_Click(object sender, EventArgs e)
        {
            IncludeInTransferCacheDB = !IncludeInTransferCacheDB;
            DiversityCollection.CacheDatabase.CacheDB.initScheduleControls(
                this.SourcesTable(), 
                this._SourceView, 
                null, 
                this.checkBoxIncludeInTransfer, 
                this.buttonTransferToCacheFilter, 
                this.buttonTransferSettings, 
                this.buttonTransferProtocol, 
                this.buttonTransferErrors, 
                this.buttonTransferToCache,
                this.labelCountCacheDB,
                this.toolTip);
        }

        private void buttonTransferProtocol_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT S.TransferProtocol FROM " + this.SourcesTable() + " AS S WHERE S.SourceView = '" + this._SourceView.ToString() + "'";
            string Protocol = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Protocol.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer protocol for " + this._SourceView, Protocol, true);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No protocol found");
        }

        private void buttonTransferErrors_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT S.TransferErrors FROM " + this.SourcesTable() + " AS S WHERE S.SourceView = '" + this._SourceView.ToString() + "'";
            string Protocol = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Protocol.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer errors for " + this._SourceView, Protocol, true);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No errors found");
        }

        private void buttonTransferSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._IsWebservice)
                {
                    this.ForWebservice_ClearErrorLog();
                }
                else
                {
                    DiversityCollection.CacheDatabase.FormScheduleTransferSettings f = new FormScheduleTransferSettings(this.SourcesTable(), this.SourceView, "");
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        this.initControl();
                        if (this.CompetingTransfer(false).Length == 0)
                            this.buttonTransferToCache.Enabled = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private bool IncludeInTransferCacheDB
        {
            get
            {
                string SQL = "SELECT IncludeInTransfer FROM " + this.SourcesTable() + " WHERE SourceView = '" + this.SourceView + "'";
                bool DoCompare = false;
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                bool.TryParse(Result, out DoCompare);
                return DoCompare;
            }
            set
            {
                string SQL = "SELECT COUNT(*) ";
                SQL += " FROM " + this.SourcesTable() + " P WHERE P.SourceView = '" + this._SourceView.ToString() + "' ";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result != "1")
                {
                    SQL = "INSERT INTO " + this.SourcesTable() + " (SourceView, IncludeInTransfer) VALUES ('" + this._SourceView + "', ";
                    if (value)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += ")";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                }
                else
                {
                    SQL = "UPDATE P SET IncludeInTransfer = ";
                    if (value)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += " FROM " + this.SourcesTable() + " P  WHERE SourceView = '" + this.SourceView + "'";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                }
            }
        }

        private bool CompareLogDateCacheDB
        {
            get
            {
                string SQL = "SELECT CompareLogDate FROM " + this.SourcesTable() + " WHERE SourceView = '" + this.SourceView + "'";
                bool DoCompare = false;
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                bool.TryParse(Result, out DoCompare);
                return DoCompare;
            }
            set
            {
                string SQL = "SELECT COUNT(*) ";
                SQL += " FROM " + this.SourcesTable() + " P WHERE P.SourceView = '" + this._SourceView.ToString() + "' ";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result != "1")
                {
                    SQL = "INSERT INTO " + this.SourcesTable() + " (SourceView, CompareLogDate) VALUES ('" + this._SourceView + "', ";
                    if (value)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += ")";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                }
                else
                {
                    SQL = "UPDATE P SET CompareLogDate = ";
                    if (value)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += " FROM " + this.SourcesTable() + " P  WHERE SourceView = '" + this.SourceView + "'";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                }
            }
        }

#endregion

#endregion

#region Postgres

        private int _CountPostgres = 0;

        private string SourcesTable()
        {
            string TableName = this.MainTable();
            return TableName + "Source";
        }

        public void initPostgresControls()
        {
            try
            {
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(90);
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                {
                    if (this.IsCacheDatabase())
                    {
                        //this.buttonPostgresTransferViaFileSettings.Enabled = !CacheDB.IsSqlServerExpress;

                        this.buttonTransferToPostgres.Enabled = true;
                        this.buttonViewPostgres.Enabled = true;
                        this.buttonHistoryInPostgres.Enabled = true;
                        string SqlExists = "select count(*) from information_schema.Tables T " +
                            "where T.table_type = 'BASE TABLE' " +
                            "and T.table_schema = 'public' " +
                            "and T.table_name = '";
                        string SqlCount = "SELECT COUNT(*) FROM ";
                        string SqlDate = "SELECT to_char(MAX(T.\"LogInsertedWhen\"), 'YYYY-MM-DD HH24:MI:SS') AS \"LastUpdate\" FROM ";
                        switch (this._TypeOfSource)
                        {
                            case TypeOfSource.Taxa:
                                SqlExists += "TaxonSynonymy";
                                SqlCount += "\"TaxonSynonymy\"";
                                SqlDate += "\"TaxonSynonymy\"";
                                break;
                            case TypeOfSource.ScientificTerms:
                                SqlExists += "ScientificTerm";
                                SqlCount += "\"ScientificTerm\"";
                                SqlDate += "\"ScientificTerm\"";
                                break;
                            case TypeOfSource.Gazetteer:
                                SqlExists += "Gazetteer";
                                SqlCount += "\"Gazetteer\"";
                                SqlDate += "\"Gazetteer\"";
                                break;
                            case TypeOfSource.Agents:
                                SqlExists += "Agent";
                                SqlCount += "\"Agent\"";
                                SqlDate += "\"Agent\"";
                                break;
                            case TypeOfSource.References:
                                SqlExists += "ReferenceTitle";
                                SqlCount += "\"ReferenceTitle\"";
                                SqlDate += "\"ReferenceTitle\"";
                                break;
                            case TypeOfSource.Plots:
                                SqlExists += "SamplingPlot";
                                SqlCount += "\"SamplingPlot\"";
                                SqlDate += "\"SamplingPlot\"";
                                break;
                        }
                        SqlExists += "';";
                        SqlCount += " T WHERE \"SourceView\" = '" + this._SourceView + "'";
                        SqlDate += " T WHERE \"SourceView\" = '" + this._SourceView + "'";
                        string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SqlExists);
                        if (Result != "0" && Result != "")
                        {
                            Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SqlCount);
                            if (Result != "0" && Result != "")
                            {
                                this.labelCountPostgres.Text = "Nr. of datasets: " + Result + "\r\nLast transfer:\r\n";
                                this.labelCountPostgres.Text += DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SqlDate);
                                int.TryParse(Result, out this._CountPostgres);
                                if (this._CountCacheDB != this._CountPostgres)
                                    this.buttonViewPostgresCompetingSources.Enabled = true;
                            }
                            else
                            {
                                this.labelCountPostgres.Text = "No data";
                            }
                        }
                        else
                        {
                            this.labelCountPostgres.Text = "No table - update needed";
                        }
                        this.initPostgresOtherTargets();
                    }
                    else
                    {
                        this.buttonTransferToPostgres.Enabled = false;
                        this.buttonViewPostgres.Enabled = false;
                        this.buttonHistoryInPostgres.Enabled = false;
                        this.labelCountPostgres.Text = "Not connected";
                    }
                }
                else
                {
                    this.buttonTransferToPostgres.Enabled = false;
                    this.buttonViewPostgres.Enabled = false;
                    this.buttonTransferToPostgresFilter.Enabled = false;
                    this.buttonHistoryInPostgres.Enabled = false;
                    this.labelCountPostgres.Text = "Not connected";
                    //this.buttonPostgresTransferViaFileSettings.Enabled = false;
                }
                this.initPostgresScheduleTransferControls();
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void initPostgresOtherTargets()
        {
            try 
            {
                string Message = "";
                foreach (System.Windows.Forms.Control C in this.panelPostgresTargets.Controls)
                    C.Dispose();
                this.panelPostgresTargets.Controls.Clear();
                this.panelPostgresTargets.Height = 0;
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(80);
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null) // #99
                {
                    string SQL = "SELECT Target FROM " + this.SourcesTable() + "Target p WHERE SourceView = '" + this._SourceView + "' AND p.Target <> '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                    System.Data.DataTable dt = new DataTable();
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            DiversityCollection.CacheDatabase.UserControlLookupSourceTarget U = new UserControlLookupSourceTarget(R[0].ToString(), this.SourcesTable() + "Target", this._SourceView);
                            this.panelPostgresTargets.Controls.Add(U);
                            U.Dock = DockStyle.Top;
                            U.BringToFront();
                            this.panelPostgresTargets.Height += DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(U.Height);
                        }
                        this.Height += this.panelPostgresTargets.Height;
                    }
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonViewPostgres_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this._TypeOfSource)
                {
                    case TypeOfSource.Taxa:
                        if (this._IsWebservice)
                        {
                            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(true, "public", "TaxonSynonymy", "SourceView", this._SourceView);
                            f.ShowDialog();
                        }
                        else
                        {
                            DiversityCollection.CacheDatabase.FormViewContent f = new FormViewContent(true, "public", "Taxon");
                            f.ShowDialog();
                        }
                        break;
                    case TypeOfSource.ScientificTerms:
                        DiversityCollection.CacheDatabase.FormViewContent fTerm = new FormViewContent(true, "public", "ScientificTerm");
                        fTerm.ShowDialog();
                        break;
                    case TypeOfSource.Gazetteer:
                        DiversityCollection.CacheDatabase.FormViewContent fGaz = new FormViewContent(true, "public", "Gazetteer");
                        fGaz.ShowDialog();
                        break;
                    case TypeOfSource.Agents:
                        DiversityCollection.CacheDatabase.FormViewContent fA = new FormViewContent(true, "public", "Agent");
                        fA.ShowDialog();
                        break;
                    case TypeOfSource.References:
                        DiversityCollection.CacheDatabase.FormViewContent fR = new FormViewContent(true, "public", "Reference");
                        fR.ShowDialog();
                        break;
                    case TypeOfSource.Plots:
                        DiversityCollection.CacheDatabase.FormViewContent fP = new FormViewContent(true, "public", "SamplingPlot");
                        fP.ShowDialog();
                        break;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#region Transfer data

        private void buttonTransferToPostgres_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                string CompetingTransfer = this.CompetingTransfer(true);
                if (CompetingTransfer.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show("Competing transfer:\r\n\r\n" + CompetingTransfer + "\r\n\r\nmust be finished first", "Competing transfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string Report = "";
                this.TransferToPostgres(ref Report, null);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.initPostgresControls();
        }

        private bool TransferToPostgres(ref string Report, InterfaceCacheDatabase InterfaceCacheDB)
        {
            bool OK = true;
            this._TransferHistory.Clear();
            if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
            {
                string StartingTarget = "";
                try
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() == null)
                    {
                        if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)
                            return true;
                        else
                            return false;
                    }

                    // only the given database will be used for Process only transfers
                    if (true)//false)
                    {
                        StartingTarget = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                        string Message = "";
                        string SQL = "SELECT Target " +
                            "FROM " + this.SourcesTable() + "Target " +
                            "WHERE SourceView = '" + this._SourceView + "' AND IncludeInTransfer = 1";

                        System.Data.DataTable dtTargets = new DataTable();
                        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtTargets, ref Message);
                        if (dtTargets.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow R in dtTargets.Rows)
                            {
                                string CurrentDB = R["Target"].ToString();
                                DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(CurrentDB);
                                string Error = "";
                                string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar("SELECT 1", ref Error);
                                if (Error.Length > 0 || Result != "1")
                                {
                                    Report += "\r\n" + CurrentDB + " is not available for transfer of " + this._SourceView + " on server " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "\r\n";
                                }
                                else
                                {
                                    if (DiversityWorkbench.PostgreSQL.Connection.Databases().ContainsKey(CurrentDB))
                                    {
                                        OK = this.TransferToPostgresDatabase(ref Report);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                if (StartingTarget.Length > 0)
                {
                    if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() == null)
                        DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(StartingTarget);
                    else if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name != StartingTarget)
                        DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(StartingTarget);
                }
            }
            else
            {
                OK = this.TransferToPostgresDatabase(ref Report);
            }
            return OK;
        }

        public bool TransferToPostgresDatabase(ref string Report)
        {
            bool OK = true;
            string Error = "";
            this.ResetTransferErrorsPostgres();
            string Message = "";

            // Check if the data have been changed
            if (this.CompareLogDatePostgres)//.FilterCacheForUpdate)
            {
                if (!this.DataAreUpdatedInCache(ref Report))
                {
                    Message = "No data transferred: CacheDB data not updated in " + this.MainTable() + " for " + this._SourceView;
                    DiversityCollection.CacheDatabase.CacheDB.LogEvent("", "", Message);
                    Report += "\r\nNo data transferred:\r\nCacheDB data not updated in \t" + this.MainTable();
                    return OK;
                }
            }
            // Check if any data had been transferred to Postgres so far
            if (this.AnyDataTransferredSoFar(ref Report))
            {
                // Check if the scheduled time is reached
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly && !DiversityCollection.CacheDatabase.CacheDB.SchedulerPlanedTimeReached(
                    this.SourcesTable() + "Target",
                    this._SourceView,
                    null,
                    null, //DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name, 
                    ref Error,
                    ref Message))
                {
                    Report += "\r\nScheduled time not reached";
                    if (Message.Length > 0)
                    {
                        Report += "\r\n" + Message;
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("TransferToPostgresDatabase(bool ProcessOnly, ref string Report)", this._SourceView, Message);
                    }
                    return OK;
                }
            }

            Report = "\r\nTransferred data:\r\n";
            System.Collections.Generic.List<string> SuppressedColumns = new List<string>();
            SuppressedColumns.Add("LogInsertedWhen");
            try
            {
                string ActiveTransfer = DiversityCollection.CacheDatabase.CacheDB.SetTransferActive(
                    this.SourcesTable() + "Target",
                    DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name,
                    null,
                    null,
                    this._SourceView,
                    true);
                if (ActiveTransfer.Length > 0)
                {
                    return false;
                }

                if (this._IsWebservice)
                {
                    switch (this._TypeOfSource)
                    {
                        case TypeOfSource.Taxa:
                            DiversityCollection.CacheDatabase.TransferStep TS = new TransferStep("TaxonSynonymy", null, "TaxonSynonymy", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            // Transfer of TaxonSynonymy
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonSynonymy");
                            OK = TS.TransferData();
                            Error = TS.Errors();
                            if (Error.Length > 0)
                            {
                                this.WriteTransferErrorsPostgres(Error);
                                this._TransferHistory.Add(TS.TableName(), Error);
                            }
                            break;
                    }
                }
                else
                {
                    switch (this._TypeOfSource)
                    {
                        case TypeOfSource.Taxa:
                            DiversityCollection.CacheDatabase.TransferStep TS = new TransferStep("TaxonSynonymy", null, "TaxonSynonymy", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TS.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TC = new TransferStep("TaxonCommonName", null, "TaxonCommonName", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TC.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TL = new TransferStep("TaxonList", null, "TaxonList", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TL.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TA = new TransferStep("TaxonAnalysis", null, "TaxonAnalysis", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TA.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TAC = new TransferStep("TaxonAnalysisCategory", null, "TaxonAnalysisCategory", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TAC.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TACV = new TransferStep("TaxonAnalysisCategoryValue", null, "TaxonAnalysisCategoryValue", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TACV.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TE = new TransferStep("TaxonNameExternalDatabase", null, "TaxonNameExternalDatabase", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TE.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TED = new TransferStep("TaxonNameExternalID", null, "TaxonNameExternalID", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TED.I_Transfer = this;

                            // Transfer of TaxonSynonymy
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonSynonymy");
                            OK = TS.TransferData();
                            Error = TS.Errors();
                            if (Error.Length > 0)
                            {
                                this.WriteTransferErrorsPostgres(Error);
                                this._TransferHistory.Add(TS.TableName(), Error);
                            }
                            else
                            {
                                this._TransferHistory.Add(TS.TableName(), TS.TotalCount);
                            }

                            // Transfer of TaxonCommonName
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonCommonName");
                                OK = TC.TransferData();
                                Error = TC.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TC.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TC.TableName(), TC.TotalCount);
                                Report += TS.Report();
                            }
                            else
                                Message = TS.Report();

                            // Transfer of TaxonList
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonList");
                                OK = TL.TransferData();
                                Error = TL.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TL.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TL.TableName(), TL.TotalCount);
                                Report += TC.Report();
                            }
                            else
                                Message = TC.Report();

                            // Transfer of TaxonAnalysis
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonAnalysis");
                                OK = TA.TransferData();
                                Error = TA.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TA.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TA.TableName(), TA.TotalCount);
                                Report += TL.Report();
                            }
                            else
                                Message = TL.Report();

                            // Transfer of TaxonAnalysisCategory
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonAnalysisCategory");
                                OK = TAC.TransferData();
                                Error = TAC.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TAC.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TAC.TableName(), TAC.TotalCount);
                                Report += TA.Report();
                            }
                            else
                                Message = TA.Report();

                            // Transfer of TaxonAnalysisCategoryValue
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonAnalysisCategoryValue");
                                OK = TACV.TransferData();
                                Error = TACV.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TACV.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TACV.TableName(), TACV.TotalCount);
                                Report += TACV.Report();
                            }
                            else
                                Message = TAC.Report();

                            // Transfer of TaxonNameExternalDatabase
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonNameExternalDatabase");
                                OK = TE.TransferData();
                                Error = TE.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TE.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TE.TableName(), TE.TotalCount);
                                Report += TACV.Report();
                            }
                            else
                                Message = TACV.Report();

                            // Transfer of TaxonNameExternalID
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer TaxonNameExternalID");
                                OK = TED.TransferData();
                                Error = TED.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TED.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TED.TableName(), TED.TotalCount);
                                Report += TE.Report();
                            }
                            else
                                Message = TED.Report();

                            break;

                        case TypeOfSource.ScientificTerms:
                            DiversityCollection.CacheDatabase.TransferStep TST = new TransferStep("ScientificTerm", null, "ScientificTerm", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TST.I_Transfer = this;
                            OK = TST.TransferData();
                            Error = TST.Errors();
                            if (Error.Length > 0)
                            {
                                this.WriteTransferErrorsPostgres(Error);
                                this._TransferHistory.Add(TST.TableName(), Error);
                            }
                            else
                                this._TransferHistory.Add(TST.TableName(), TST.TotalCount);
                            Report = TST.Report();
                            //this.TransferTermsToPostgres();
                            break;
                        case TypeOfSource.Gazetteer:
                            DiversityCollection.CacheDatabase.TransferStep TGaz = new TransferStep("Gazetteer", null, "Gazetteer", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TGaz.I_Transfer = this;
                            OK = TGaz.TransferData();
                            Error = TGaz.Errors();
                            if (Error.Length > 0)
                            {
                                this.WriteTransferErrorsPostgres(Error);
                                this._TransferHistory.Add(TGaz.TableName(), Error);
                            }
                            else
                                this._TransferHistory.Add(TGaz.TableName(), TGaz.TotalCount);
                            Report = TGaz.Report();
                            //this.TransferGazetteerToPostgres();
                            break;
                        case TypeOfSource.Agents:
                            DiversityCollection.CacheDatabase.TransferStep TAg = new TransferStep("Agent", null, "Agent", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TAg.PostgresRole = "CacheAdmin";
                            TAg.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TAgCo = new TransferStep("AgentContactInformation", null, "AgentContactInformation", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TAgCo.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TAgIm = new TransferStep("AgentImage", null, "AgentImage", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TAgIm.I_Transfer = this;
#if AgentIdentifierIncluded
                            DiversityCollection.CacheDatabase.TransferStep TAgId = new TransferStep("AgentIdentifier", null, "AgentIdentifier", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TAgId.I_Transfer = this;
#endif
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer Agent");
                            OK = TAg.TransferData();
                            Error = TAg.Errors();
                            if (Error.Length > 0)
                            {
                                this.WriteTransferErrorsPostgres(Error);
                                this._TransferHistory.Add(TAg.TableName(), Error);
                            }
                            else
                                this._TransferHistory.Add(TAg.TableName(), TAg.TotalCount);
                            Report = TAg.Report();
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer AgentContactInformation");
                                OK = TAgCo.TransferData();
                                Error = TAgCo.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TAgCo.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TAgCo.TableName(), TAgCo.TotalCount);
                                if (OK)
                                {
                                    OK = TAgIm.TransferData();
                                    Error = TAgIm.Errors();
                                    if (Error.Length > 0)
                                    {
                                        this.WriteTransferErrorsPostgres(Error);
                                        this._TransferHistory.Add(TAgIm.TableName(), Error);
                                    }
                                    else
                                        this._TransferHistory.Add(TAgIm.TableName(), TAgIm.TotalCount);
                                    Report += TAgIm.Report();
                                }
#if AgentIdentifierIncluded
                                if (OK)
                                {
                                    OK = TAgId.TransferData();
                                    Error = TAgId.Errors();
                                    if (Error.Length > 0)
                                    {
                                        this.WriteTransferErrorsPostgres(Error);
                                        this._TransferHistory.Add(TAgId.TableName(), Error);
                                    }
                                    else
                                        this._TransferHistory.Add(TAgId.TableName(), TAgId.TotalCount);
                                    Report += TAgId.Report();
                                }
#endif
                            }
                            //this.TransferAgentToPostgres();
                            break;
                        case TypeOfSource.References:
                            DiversityCollection.CacheDatabase.TransferStep TRef = new TransferStep("Reference", null, "ReferenceTitle", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TRef.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TRefRel = new TransferStep("ReferenceRelator", null, "ReferenceRelator", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TRefRel.I_Transfer = this;
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer Reference");
                            OK = TRef.TransferData();
                            Error = TRef.Errors();
                            if (Error.Length > 0)
                            {
                                this.WriteTransferErrorsPostgres(Error);
                                this._TransferHistory.Add(TRef.TableName(), Error);
                            }
                            else
                                this._TransferHistory.Add(TRef.TableName(), TRef.TotalCount);
                            Report = TRef.Report();
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer ReferenceRelator");
                                OK = TRefRel.TransferData();
                                Error = TRefRel.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TRefRel.TableName(), Error);
                                }
                                else
                                    this._TransferHistory.Add(TRefRel.TableName(), TRefRel.TotalCount);
                                if (OK)
                                    Report += TRefRel.Report();
                            }
                            break;
                        case TypeOfSource.Plots:
                            DiversityCollection.CacheDatabase.TransferStep TP = new TransferStep("SamplingPlot", null, "SamplingPlot", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TP.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TPL = new TransferStep("SamplingPlotLocalisation", null, "SamplingPlotLocalisation", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TPL.I_Transfer = this;
                            DiversityCollection.CacheDatabase.TransferStep TPP = new TransferStep("SamplingPlotProperty", null, "SamplingPlotProperty", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
                            TPP.I_Transfer = this;
                            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer Plot");
                            OK = TP.TransferData();
                            Error = TP.Errors();
                            if (Error.Length > 0)
                            {
                                this.WriteTransferErrorsPostgres(Error);
                                this._TransferHistory.Add(TP.TableName(), Error);
                            }
                            else
                                this._TransferHistory.Add(TP.TableName(), TP.TotalCount);
                            Report = TP.Report();
                            if (OK)
                            {
                                if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                    this.TransferToPostgresSetMessage("Transfer SamplingPlotLocalisation");
                                OK = TPL.TransferData();
                                Error = TPL.Errors();
                                if (Error.Length > 0)
                                {
                                    this.WriteTransferErrorsPostgres(Error);
                                    this._TransferHistory.Add(TPL.TableName(), Error);
                                }
                                else
                                {
                                    this._TransferHistory.Add(TPL.TableName(), TPL.TotalCount);
                                }
                                if (OK)
                                {
                                    Report += TPL.Report();
                                    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                                        this.TransferToPostgresSetMessage("Transfer SamplingPlotProperty");
                                    OK = TPP.TransferData();
                                    Error = TPL.Errors();
                                    if (Error.Length > 0)
                                    {
                                        this.WriteTransferErrorsPostgres(Error);
                                        this._TransferHistory.Add(TPP.TableName(), Error);
                                    }
                                    else
                                    {
                                        this._TransferHistory.Add(TPP.TableName(), TPP.TotalCount);
                                    }
                                    if (OK)
                                    {
                                        Report += TPP.Report();
                                    }
                                }
                            }
                            //this.TransferAgentToPostgres();
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                Message += ex.Message;
            }
            finally
            {
                if (!OK) Error = Message;
                DiversityCollection.CacheDatabase.CacheDB.SetTransferFinished(
                    this.SourcesTable() + "Target",
                    DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name,
                    null,
                    null,
                    this._SourceView,
                    Report,
                    Error);
                this.WriteHistory(HistoryTarget.CacheToPostgres, DiversityCollection.CacheDatabase.CacheDB.TargetID());
            }

            if (OK)
            {
                Message = "Data transferred";
                //this.WriteTransferProtocolPostgres(Report, true);
            }
            else
            {
                Message = "Transfer failed:\r\n" + Message;
                //this.WriteTransferErrorsPostgres(Message);
            }

            Message += "\r\nto Postgres database " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name +
                "\r\non Server " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name;
            this.WriteReport(Message, Report);

            if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
            {
                System.Windows.Forms.MessageBox.Show(Message);
            }
            return OK;
        }


        private bool TransferToPostgresDatabase_Agents(ref string Report)
        {
            bool OK = true;
            //string Error = "";
            //DiversityCollection.CacheDatabase.TransferStep TAg = new TransferStep("Agent", null, "Agent", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
            //DiversityCollection.CacheDatabase.TransferStep TAgCo = new TransferStep("AgentContactInformation", null, "AgentContactInformation", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
            //DiversityCollection.CacheDatabase.TransferStep TAgIm = new TransferStep("AgentImage", null, "AgentImage", "dbo", "public", true, true, SuppressedColumns, this.SourceView);
            //if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer Agent");
            //OK = TAg.TransferData();
            //Error = TAg.Errors();
            //if (Error.Length > 0)
            //{
            //    this.WriteTransferErrorsPostgres(Error);
            //    this._TransferHistory.Add(TAg.TableName(), Error);
            //}
            //else
            //    this._TransferHistory.Add(TAg.TableName(), TAg.TotalCount);
            //Report = TAg.Report();
            //if (OK)
            //{
            //    if (!DiversityCollection.CacheDatabase.CacheDB.ProcessOnly) this.TransferToPostgresSetMessage("Transfer AgentContactInformation");
            //    OK = TAgCo.TransferData();
            //    Error = TAgCo.Errors();
            //    if (Error.Length > 0)
            //    {
            //        this.WriteTransferErrorsPostgres(Error);
            //        this._TransferHistory.Add(TAgCo.TableName(), Error);
            //    }
            //    else
            //        this._TransferHistory.Add(TAgCo.TableName(), TAgCo.TotalCount);
            //    if (OK)
            //    {
            //        OK = TAgIm.TransferData();
            //        Error = TAgIm.Errors();
            //        if (Error.Length > 0)
            //        {
            //            this.WriteTransferErrorsPostgres(Error);
            //            this._TransferHistory.Add(TAgIm.TableName(), Error);
            //        }
            //        else
            //            this._TransferHistory.Add(TAgIm.TableName(), TAgIm.TotalCount);
            //        Report += TAgIm.Report();
            //    }
            //}
            return OK;
        }

        private bool AnyDataTransferredSoFar(ref string Report)
        {
            bool OK = false;
            string SQL = "SELECT COUNT(*) FROM ";
            switch (this._TypeOfSource)
            {
                case TypeOfSource.Agents:
                    SQL += "\"Agent\"";
                    break;
                //case TypeOfSource.Descriptions:
                //    SQL += "\"Agent\"";
                //    break;
                case TypeOfSource.Gazetteer:
                    SQL += "\"Gazetteer\"";
                    break;
                case TypeOfSource.References:
                    SQL += "\"ReferenceTitle\"";
                    break;
                case TypeOfSource.ScientificTerms:
                    SQL += "\"ScientificTerm\"";
                    break;
                case TypeOfSource.Taxa:
                    SQL += "\"TaxonSynonymy\"";
                    break;
                case TypeOfSource.Plots:
                    SQL += "\"SamplingPlot\"";
                    break;
            }
            SQL += " WHERE \"SourceView\" = '" + this._SourceView + "'";
            string Error = "";
            string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Error);
            if (Error.Length > 0)
                Report += "\r\nCould not check table for source " + this._TypeOfSource.ToString() + " and view " + this._SourceView;
            else if (Result != "0" && Result.Length > 0)
                OK = true;
            return OK;
        }
        
        private void WriteTransferProtocolPostgres(string Protocol, bool TransferSuccessful)
        {
            try
            {
                string SQL = "UPDATE P SET P.TransferProtocol = '" + Protocol + "' ";
                if (TransferSuccessful)
                    SQL += ", P.LastUpdatedWhen = getdate() ";
                SQL += " FROM " + this.SourcesTable() + "Target P WHERE P.SourceView = '" + this._SourceView.ToString() + "' " +
                    "AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' ";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ResetTransferErrorsPostgres()
        {
            try
            {
                string CurrentDB = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                string SQL = "UPDATE P SET P.TransferErrors = '' " +
                    " FROM " + this.SourcesTable() + "Target P WHERE P.SourceView = '" + this._SourceView.ToString() + "' " +
                    " AND Target = '" + CurrentDB + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void WriteTransferErrorsPostgres(string Errors)
        {
            try
            {
                string SQL = "UPDATE P SET P.TransferErrors = CASE WHEN P.TransferErrors IS NULL THEN '' ELSE P.TransferErrors END + '\r\n" + Errors.Replace("'", "''") + "' " +
                    " FROM " + this.SourcesTable() + "Target P WHERE P.SourceView = '" + this._SourceView.ToString() + "' " +
                    " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "', ";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void TransferToPostgresSetMessage(string Message)
        {
            this.labelCountPostgres.Text = Message;
            Application.DoEvents();
        }

#endregion

#region Transfer via bcp

        private string _TransferDirectory = "";

        private void buttonPostgresTransferViaFileSettings_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;

            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Transfer directory", "Please enter path of the transfer directory", this._TransferDirectory);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                //this.textBoxPostgresTransferDirectory.Text = f.String;
                string SQL = "UPDATE P SET P.[TransferDirectory] = '" + f.String + "' " +
                    " FROM [dbo].[ProjectTarget] P, [Target] T " +
                    " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                    " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                    " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
                    " AND T.TargetID = P.TargetID ";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                //this.initPostgresForFileTransfer();
                this.initPostgresControls();
            }
        }

#endregion

#region Competing sources

        System.Collections.Generic.Dictionary<string, int> _CompetingSources;

        private void buttonViewPostgresCompetingSources_Click(object sender, EventArgs e)
        {
            string SqlCount = "SELECT \"SourceView\", COUNT(*) FROM \"public\".\"";
            switch (this._TypeOfSource)
            {
                case TypeOfSource.Agents:
                    SqlCount += "Agent";
                    break;
                case TypeOfSource.Gazetteer:
                    SqlCount += "Gazetteer";
                    break;
                case TypeOfSource.References:
                    SqlCount += "ReferenceTitle";
                    break;
                case TypeOfSource.ScientificTerms:
                    SqlCount += "ScientificTerm";
                    break;
                case TypeOfSource.Taxa:
                    SqlCount += "TaxonSynonymy";
                    break;
            }
            SqlCount += "\"";
            if (this.SourceView != null && this.SourceView.Length > 0)
                SqlCount += " WHERE \"SourceView\" <> '" + this.SourceView + "' GROUP BY \"SourceView\" ORDER BY \"SourceView\"";
            System.Data.DataTable dtCompete = new DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SqlCount, ref dtCompete, ref Message);
            if (dtCompete.Rows.Count > 0 && Message.Length == 0)
            {
                Message = "Competing sources:";
                foreach (System.Data.DataRow R in dtCompete.Rows)
                {
                    Message += "\r\n" + R[0].ToString() + ": " + R[1].ToString();
                }
                System.Windows.Forms.MessageBox.Show(Message);
            }
        }
        
#endregion

#endregion

#region Check for updates

        private bool DataAreUpdatedInCache(ref string Report)
        {
            bool DataAreUpdated = true;
            try
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                {
                    CacheDB.LogEvent(this.Name.ToString(), "DataAreUpdatedInCache(bool ProcessOnly, ref string Report)", this._SourceView + ": Checking for update");
                }
                string SQL = "SELECT CASE WHEN T.[LastUpdatedWhen] IS NULL THEN 1 ELSE CASE WHEN T.[LastUpdatedWhen] < S.[LastUpdatedWhen] THEN 1 ELSE 0 END END AS DataNeedUpdate " +
                    "FROM [dbo].[" + this.SourcesTable() + "Target] T, [dbo].[" + this.SourcesTable() + "] S " +
                    "WHERE T.SourceView = S.SourceView " +
                    "AND T.SourceView = '" + this._SourceView + "' ";
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                    SQL += "AND T.Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result != "0")
                    DataAreUpdated = true;
                else
                    DataAreUpdated = false;
                if (DiversityCollection.CacheDatabase.CacheDB.ProcessOnly)
                {
                    CacheDB.LogEvent("", "", "   Result: " + DataAreUpdated.ToString());
                }
            }
            catch (System.Exception ex)
            {
                DataAreUpdated = false;
            }
            return DataAreUpdated;
        }

        private bool DataAreUpdatedInCacheSubsets(ref string Message, ref string Report, System.DateTime LastTransfer)
        {
            bool OK = false;
            System.DateTime LastUpdateInSource;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<SubsetTable, string> KV in this.TransferredSubsets())
                {
                    string SQL = "SELECT MAX(T.LogUpdatedWhen) FROM " + this._SourceView + KV.Value + " T";
                    if (System.DateTime.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out LastUpdateInSource))
                    {
                        if (LastUpdateInSource.CompareTo(LastTransfer) > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

#endregion

#region Postgres scheduler

        private void initPostgresScheduleTransferControls()
        {
            try
            {
                DiversityCollection.CacheDatabase.CacheDB.initScheduleControls(
                    this.SourcesTable() + "Target",
                    this.SourceView,
                    null,
                    this.checkBoxPostgresIncludeInTransfer,
                    this.buttonTransferToPostgresFilter,
                    this.buttonPostgresTransferSettings,
                    this.buttonPostgresTransferProtocol,
                    this.buttonPostgresTransferErrors,
                    this.buttonTransferToPostgres,
                    this.labelCountPostgres,
                    this.toolTip);
            }
            catch (System.Exception ex) { }
        }

        private void checkBoxPostgresIncludeInTransfer_Click(object sender, EventArgs e)
        {
            this.IncludeInTransferPostgres = !this.IncludeInTransferPostgres;
            DiversityCollection.CacheDatabase.CacheDB.initScheduleControls(
                this.SourcesTable() + "Target",
                this._SourceView,
                null,
                this.checkBoxPostgresIncludeInTransfer,
                this.buttonTransferToPostgresFilter,
                this.buttonPostgresTransferSettings,
                this.buttonPostgresTransferProtocol,
                this.buttonPostgresTransferErrors,
                this.buttonTransferToPostgres,
                this.labelCountPostgres,
                this.toolTip);
        }

        private void buttonPostgresTransferSettings_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.CacheDatabase.FormScheduleTransferSettings f = new FormScheduleTransferSettings(
                    this.SourcesTable() + "Target", 
                    this.SourceView, 
                    DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this.initPostgresControls();
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonPostgresTransferProtocol_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT S.TransferProtocol FROM " + this.SourcesTable() + "Target AS S WHERE S.SourceView = '" + this._SourceView.ToString() + "' " +
                " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
            string Protocol = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Protocol.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer protocol for " + this._SourceView, Protocol, true);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No errors found");
        }

        private void buttonPostgresTransferErrors_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT S.TransferErrors FROM " + this.SourcesTable() + "Target AS S WHERE S.SourceView = '" + this._SourceView.ToString() + "' " +
                " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
            string Protocol = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Protocol.Length > 0)
            {
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Transfer errors for " + this._SourceView, Protocol, true);
                f.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("No errors found");
        }

        private bool IncludeInTransferPostgres
        {
            get
            {
                bool DoInclude = false;
                string SQL = "SELECT IncludeInTransfer FROM " + this.SourcesTable() + "Target WHERE SourceView = '" + this.SourceView + "'";
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                {
                    SQL += " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                    string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    bool.TryParse(Result, out DoInclude);
                }
                return DoInclude;
            }
            set
            {
                string SQL = "SELECT COUNT(*) FROM " + this.SourcesTable() + "Target P  WHERE SourceView = '" + this.SourceView + "' " +
                    " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result == "0")
                {
                    SQL = "INSERT INTO " + this.SourcesTable() + "Target (SourceView, Target, IncludeInTransfer) " +
                        "VALUES ('" + this._SourceView + "', '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "', ";
                    if (value)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += ")";
                }
                else
                {
                    SQL = "UPDATE P SET IncludeInTransfer = ";
                    if (value)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += " FROM " + this.SourcesTable() + "Target P  WHERE SourceView = '" + this.SourceView + "' " +
                        " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                }
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
        }

        private bool CompareLogDatePostgres
        {
            get
            {
                bool DoCompare = false;
                string SQL = "SELECT CompareLogDate FROM " + this.SourcesTable() + "Target WHERE SourceView = '" + this.SourceView + "' ";
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                {
                    SQL += " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                    // Markus 28.3.25: Check existence. Issue #36
                    string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, true);
                    bool.TryParse(Result, out DoCompare);
                }
                return DoCompare;
            }
            set
            {
                string SQL = "SELECT COUNT(*) FROM " + this.SourcesTable() + "Target P  WHERE SourceView = '" + this.SourceView + "' " +
                    " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result == "0")
                {
                    SQL = "INSERT INTO " + this.SourcesTable() + "Target (SourceView, Target, CompareLogDate) " +
                        "VALUES ('" + this._SourceView + "', '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "', ";
                    if (value)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += ")";
                }
                else
                {
                    SQL = "UPDATE P SET CompareLogDate = ";
                    if (value)
                        SQL += "1";
                    else
                        SQL += "0";
                    SQL += " FROM " + this.SourcesTable() + "Target P  WHERE SourceView = '" + this.SourceView + "' " +
                        " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                }
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
        }

#endregion

#region History

        private enum HistoryTarget { DataToCache, CacheToPostgres }

        private void WriteHistory(HistoryTarget Target, int? TargetID)
        {
            try
            {
                int UserID = int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT dbo.UserID()"));
                string SQL = "INSERT INTO SourceTransfer (Source, SourceView, ResponsibleUserID, TargetID, Settings) " +
                    "VALUES ('" + this._TypeOfSource.ToString() + "', '" + this.SourceView + "', " + UserID.ToString() + ", ";
                if (Target == HistoryTarget.DataToCache)
                    SQL += "NULL, '" + this.SettingsForHistory(Target) + "'";
                else
                    SQL += TargetID.ToString() + ", '" + this.SettingsForHistory(Target) + "'";
                SQL += ")";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string SettingsForHistory(HistoryTarget Target)
        {
            string Settings = "";
            try
            {
                // Dictionary for all infos
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<object>> D_History = new Dictionary<string, System.Collections.Generic.List<object>>();

                // Versions
                System.Collections.Generic.List<object> OVers = new List<object>();
                OVers.Add(this.HistoryVersions(Target));
                D_History.Add("Versions", OVers);

                // Data
                System.Collections.Generic.List<object> L_Data = new List<object>();
                L_Data.Add(this._TransferHistory);
                D_History.Add("Data", L_Data);
                
                Settings = System.Text.Json.JsonSerializer.Serialize(D_History);
            }
            catch (System.Exception ex)
            {
            }
            return Settings;
        }

        private System.Collections.Generic.Dictionary<string, string> HistoryVersions(HistoryTarget Target)
        {
            System.Collections.Generic.Dictionary<string, string> D_Ver = new Dictionary<string, string>();
            // CacheDB
            string SQL = "SELECT dbo.Version()";
            string V = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            D_Ver.Add("CacheDB", V);

            if (Target == HistoryTarget.DataToCache)
            {
                // DB
                SQL = "SELECT dbo.Version()";
                V = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                D_Ver.Add("Database", V);
            }
            else
            {
                // Postgres DB
                SQL = "SELECT public.version()";
                V = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                D_Ver.Add("PostgresDB", V);
            }
            return D_Ver;
        }

        private void buttonHistoryInCacheDB_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.CacheDatabase.FormTransferHistory f = new FormTransferHistory(this._TypeOfSource.ToString(), this._SourceView);
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonHistoryInPostgres_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.CacheDatabase.FormTransferHistory f = new FormTransferHistory(this._TypeOfSource.ToString(), this._SourceView, DiversityCollection.CacheDatabase.CacheDB.TargetID());
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
        }


#endregion

#region Webservice

        public static System.Collections.Generic.List<string> TaxaWebserviceSourceList()
        {
            System.Collections.Generic.List<string> webserviceList = new List<string>();
            // Create and add Services for each ServiceType in the enum
            foreach (DwbServiceEnums.DwbService serviceName in System.Enum.GetValues(
                         typeof(DwbServiceEnums.DwbService)))
            {
                if (DwbServiceEnums.TaxonomicServiceInfoDictionary().TryGetValue(serviceName, out DwbServiceEnums.DwbServiceInfo serviceInfo))
                {
                    if (serviceInfo.Type == DwbServiceEnums.DwbServiceType.None)
                    {
                        continue;
                    }
                    webserviceList.Add(serviceName.ToString());
                }
            }
            return webserviceList;
        }

        private void ForWebservice_InitControl()
        {
            try
            {
                this.labelDatabase.Text = "";
                this.labelView.Text = "Webservice";
                this.labelSource.Text = "";
                this.labelServer.Text = "";
                this.buttonTestView.Image = DiversityCollection.Resource.Import;
                this.buttonTestView.ImageAlign = ContentAlignment.MiddleCenter;
                this.toolTip.SetToolTip(this.buttonTestView, "Import data from webservice");
                this.checkBoxIncludeInTransfer.Enabled = false;
                this.buttonTransferSettings.Image = null;
                this.buttonTransferToCache.Enabled = false;
                this.buttonTransferToCache.Image = null;
                this.buttonSetSubsets.Enabled = false;
                this.buttonSetSubsets.Image = null;
                this.buttonTransferToCacheFilter.Image = null;
                this.buttonTransferToCacheFilter.Enabled = false;
                if (this.ForWebservice_NoCompetingTransfer())
                    this.ForWebservice_InitCountCache();

                this.checkBoxIncludeInTransfer.FlatStyle = FlatStyle.Flat;
                this.checkBoxIncludeInTransfer.FlatAppearance.BorderColor = System.Drawing.Color.Thistle;
                this.checkBoxIncludeInTransfer.FlatAppearance.BorderSize = 0;
                this.labelDatabase.BackColor = System.Drawing.Color.Transparent;
                this.textBoxDatabase.BackColor = System.Drawing.Color.White;
                this.labelServer.BackColor = System.Drawing.Color.Transparent;
                this.textBoxServer.BackColor = System.Drawing.Color.White;
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool ForWebservice_NoCompetingTransfer()
        {
            string CompetingTransfer = this.CompetingTransfer(false);
            if (CompetingTransfer.Length > 0)
            {
                this.buttonTransferSettings.Image = DiversityCollection.Resource.Delete;
                this.toolTip.SetToolTip(this.buttonTransferSettings, "Delete the error protocol and the entry of the current transfer");
                this.labelCountCacheDB.Text = CompetingTransfer;
                this.labelCountCacheDB.BackColor = System.Drawing.Color.Red;
                return false;
            }
            else
            {
                this.labelCountCacheDB.BackColor = System.Drawing.Color.Thistle;
                return true;
            }
        }

        private void ForWebservice_InitCountCache()
        {
            string SQL = "";
            switch (this._TypeOfSource)
            {
                case TypeOfSource.Taxa:
                    SQL = "SELECT COUNT(*) FROM TaxonSynonymy WHERE SourceView = '" + this._SourceView + "'";
                    break;
            }
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result == "0")
                this.labelCountCacheDB.Text = "no data";
            else
                this.labelCountCacheDB.Text = Result + " datasets";
        }

        private void ForWebservice_ClearErrorLog()
        {
            if (System.Windows.Forms.MessageBox.Show("Do you really want to delete the error protocol and the entry of the current transfer?", "Delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string SourceTable = "";
                switch(this._TypeOfSource)
                {
                    case TypeOfSource.Taxa:
                        SourceTable = "TaxonSynonymySource";
                        break;
                }
                string SQL = "UPDATE PT SET PT.TransferErrors = NULL, PT.TransferIsExecutedBy = NULL FROM " + SourceTable + " PT WHERE 1 = 1 ";
                if (this._SourceView.Length > 0)
                    SQL += " AND SourceView = '" + this._SourceView + "'";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.ForWebservice_InitControl();
            }
        }

#endregion

#region Interface transferstep
        public void SetMessage(string Message)
        {
            ;
        }

        public void SetInfo(string Info)
        {
            ;
        }

        public void SetTransferState(TransferStep.TransferState State)
        {
            this.progressBar.Visible = false;
        }

        public void SetTransferStart()
        {
            this.progressBar.Value = 0;
            this.progressBar.Visible = true;
            Application.DoEvents();
        }

        public void SetDoTransfer(bool DoTransfer)
        {
            ;
        }

        public void SetTransferProgress(int PercentReached)
        {
            if (!this.progressBar.Visible) this.progressBar.Visible = true;
            if (this.progressBar.Value != PercentReached)
            {
                if (PercentReached < 100) this.progressBar.Value = PercentReached;
                else this.progressBar.Value = this.progressBar.Maximum;
                Application.DoEvents();
            }
        }

#endregion

#region Context menus
        private void recreateSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        private void clearCompetingTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you really want to delete the error protocol and the entry of the current transfer?", "Delete", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                string SQL = "UPDATE T SET T.TransferErrors = NULL, T.TransferIsExecutedBy = NULL FROM " + this.MainTable() + "Source T WHERE SourceView = '" + this.SourceView + "';";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    this.initControl();
                }
            }
        }

#endregion

    }
}
