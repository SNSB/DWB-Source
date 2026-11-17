using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversitySamplingPlots.Forms
{
    public partial class FormReplication : Form
    {
        #region Parameter

        private DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection _ReplicationDirection;

        private int _ProjectID;

        private System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlReplicateTable> _ReplicationControls;

        System.Data.DataTable _dtPlotID;

        #endregion

        #region Properties

        private int? RestrictionProject
        {
            get
            {
                if (this.comboBoxProject.Visible == false
                    || this.comboBoxProject.SelectedIndex == -1
                    || this.comboBoxProject.Text.Length == 0)
                    return null;
                else
                {
                    int i = 0;
                    if (int.TryParse(this.comboBoxProject.SelectedValue.ToString(), out i))
                        return i;
                    else return null;
                }
            }
        }

        #endregion

        #region Construction

        public FormReplication(DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection Direction, int? ProjectID, string ReplicationPublisherConnection)
        {
            try
            {
                InitializeComponent();
                this._ReplicationDirection = Direction;
                if (ProjectID != null)
                    this._ProjectID = (int)ProjectID;
                if (ReplicationPublisherConnection.Length > 0)
                    DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher = new Microsoft.Data.SqlClient.SqlConnection(ReplicationPublisherConnection);
                // test the connection
                bool ConnectionOK = true;
                try
                {
                    string SQL = "SELECT 1";
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);
                    string Result = C.ExecuteScalar().ToString();
                }
                catch (System.Exception ex)
                {
                    ConnectionOK = false;
                }
                if (!ConnectionOK)
                {
                    string Start = "Initial Catalog=";
                    string DB = ReplicationPublisherConnection.Substring(ReplicationPublisherConnection.IndexOf(Start) + Start.Length);
                    DB = DB.Substring(0, DB.IndexOf(";"));
                    System.Windows.Forms.MessageBox.Show("The connection to the publisher database\r\n" + DB + "\r\ncould not be established.\r\nPlease turn to your administrator");
                    this.Close();
                }
                this.initForm();

                // Manual
                this.KeyPreview = true;
                this.KeyDown += new KeyEventHandler(Form_KeyDown);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                string Server = "";
                if (DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher != null &&
                    DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.ConnectionString.Length > 0)
                {
                    this.toolTip.SetToolTip(this.pictureBoxServer, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.ConnectionString);// this._ReplicationPublisherConnectionString);
                    Server = DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.ConnectionString;// this._ReplicationPublisherConnectionString;
                    string DB = DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.ConnectionString;// this._ReplicationPublisherConnectionString;
                    if (DB.IndexOf("Initial Catalog=") > -1)
                    {
                        DB = DB.Substring(DB.IndexOf("Initial Catalog=") + 16);
                        DB = DB.Substring(0, DB.IndexOf(';'));
                        this.labelServerDatabase.Text = DB;
                    }
                    if (Server.IndexOf("Data Source=") > -1)
                    {
                        Server = Server.Substring(Server.IndexOf("Data Source=") + 12);
                        Server = Server.Substring(0, Server.IndexOf(','));
                    }
                }
                this.labelSubscriberDatabase.Text = DiversityWorkbench.Settings.DatabaseName;
                switch (this._ReplicationDirection)
                {
                    case DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Download:
                        this.Text = "Download data from server " + Server;
                        this.pictureBoxServer.Image = this.imageListDirection.Images[0];
                        this.initServerProjectList();
                        this.comboBoxProject_SelectionChangeCommitted(null, null);

                        this.comboBoxProject.Visible = true;
                        this.buttonUpload.Visible = false;
                        this.buttonClean.Visible = false;
                        this.buttonStartDownload.Visible = true;

                        string SQL = "SELECT COUNT(*) FROM SamplingPlotPointType_Enum";
                        if (int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL)) > 0)
                            this.checkBoxEnum.Checked = false;
                        //SQL = "SELECT COUNT(*) FROM Entity";
                        //if (int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL)) > 0)
                        //    this.checkBoxEntity.Checked = false;
                        SQL = "SELECT COUNT(*) FROM ProjectProxy";
                        if (int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL)) > 0)
                            this.checkBoxProjects.Checked = false;
                        SQL = "SELECT COUNT(*) FROM LocalisationSystem";
                        if (int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL)) > 0)
                            this.checkBoxBasicData.Checked = false;

                        this.checkBoxIgnoreConflicts.Text = "Force download, ignore conflicts";
                        this.checkBoxIgnoreConflicts.Visible = true;

                        break;

                    case DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Upload:
                        this.Text = "Upload data to server " + Server;
                        this.pictureBoxServer.Image = this.imageListDirection.Images[1];

                        this.comboBoxProject.Visible = false;
                        this.buttonStartDownload.Visible = false;
                        this.buttonUpload.Visible = true;
                        this.buttonClean.Visible = false;

                        SQL = "SELECT Project FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString();
                        string Project = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        this.labelProject.Text = this.labelProject.Text + " " + Project;
                        this.checkBoxEntity.Checked = false;
                        this.checkBoxEntity.Visible = false;
                        this.checkBoxEnum.Checked = false;
                        this.checkBoxEnum.Visible = false;
                        this.checkBoxProjects.Checked = false;
                        this.checkBoxProjects.Visible = false;
                        this.checkBoxBasicData.Checked = false;

                        this.checkBoxIgnoreConflicts.Text = "Force upload, ignore conflicts";
                        this.checkBoxIgnoreConflicts.Visible = true;
                        break;

                    case DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Merge:
                        this.Text = "Merge data with server " + Server;
                        this.pictureBoxServer.Image = this.imageListDirection.Images[3];

                        this.comboBoxProject.Visible = false;
                        this.buttonStartDownload.Visible = false;
                        this.buttonUpload.Visible = false;
                        this.buttonStartMerge.Visible = true;
                        this.buttonClean.Visible = false;

                        SQL = "SELECT Project FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString();
                        Project = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        this.labelProject.Text = this.labelProject.Text + " " + Project;
                        this.checkBoxEntity.Checked = false;
                        this.checkBoxEntity.Visible = false;
                        this.checkBoxEnum.Checked = false;
                        this.checkBoxEnum.Visible = false;
                        this.checkBoxProjects.Checked = false;
                        this.checkBoxProjects.Visible = false;
                        this.checkBoxBasicData.Checked = false;
                        break;

                    case DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Clean:
                        this.Text = "Clean local database";
                        this.labelHeaderPublisher.Visible = false;
                        this.labelHeaderSubscriber.Visible = false;
                        this.pictureBoxSubscriber.Visible = false;
                        this.labelSubscriberDatabase.ForeColor = System.Drawing.Color.Red;
                        this.labelSubscriberDatabase.TextAlign = ContentAlignment.BottomRight;
                        this.panelSubscriber.Padding = new Padding(0);
                        this.pictureBoxServer.Margin = new Padding(0);
                        this.pictureBoxServer.Dock = DockStyle.Top;
                        this.pictureBoxServer.Height = 16;
                        this.pictureBoxServer.Image = this.imageListDirection.Images[2];
                        this.pictureBoxServer.Dock = DockStyle.Fill;
                        this.panelPublisher.Visible = false;
                        this.labelServerDatabase.Visible = false;
                        this.labelSubscriberDatabase.Text = DiversityWorkbench.Settings.DatabaseName;
                        this.labelSubscriberDatabase.TextAlign = ContentAlignment.MiddleRight;

                        this.buttonStartDownload.Visible = false;
                        this.buttonUpload.Visible = false;
                        this.buttonClean.Visible = true;
                        this.labelProject.Visible = false;
                        this.comboBoxProject.Visible = false;

                        this.checkBoxEntity.Checked = false;
                        this.checkBoxEntity.Visible = true;
                        this.checkBoxEnum.Checked = false;
                        this.checkBoxEnum.Visible = true;
                        this.checkBoxProjects.Checked = false;
                        this.checkBoxProjects.Visible = true;
                        this.checkBoxData.Visible = true;
                        this.checkBoxData.Checked = true;
                        this.checkBoxBasicData.Visible = true;
                        this.checkBoxBasicData.Checked = true;
                        break;
                }
                this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversitySaplingPlots.chm";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FormReplication_FormClosing(object sender, FormClosingEventArgs e)
        {
            DiversitySamplingPlots.Forms.FormReplicationSettings.Default.Save();
            try
            {
                DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber.Close();
                DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber.Dispose();
                DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber = null;
                if (DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher != null)
                {
                    DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.Close();
                    DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.Dispose();
                    DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher = null;
                }
            }
            catch (System.Exception ex) { }
        }

        private void buttonRequery_Click(object sender, EventArgs e)
        {
            try
            {
                this.initReplicationControls();
                this.radioButtonAll.Checked = true;
                this.tabControlTableList.SelectedIndex = 0;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxProject_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._ProjectID = int.Parse(this.comboBoxProject.SelectedValue.ToString());
            this.panelTableList.Controls.Clear();

            //this.initReplicationControls();
        }

        private void radioButtonAll_Click(object sender, EventArgs e)
        {
            this.setSelectionForTables();
        }

        private void radioButtonNone_Click(object sender, EventArgs e)
        {
            this.setSelectionForTables();
        }

        #endregion

        #region Replication controls

        private void initReplicationControls()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.panelTableList.Controls.Clear();
            this._dtPlotID = null;

            this._ReplicationControls = new List<DiversityWorkbench.UserControls.UserControlReplicateTable>();

            try
            {
                this.AddUserControlReplicateTables(this._ReplicationDirection);//, Subscriber, Publisher);
                foreach (System.Windows.Forms.Control C in this.panelTableList.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        System.Windows.Forms.GroupBox G = (System.Windows.Forms.GroupBox)C;
                        System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlReplicateTable> _ControlsToRemove = new List<DiversityWorkbench.UserControls.UserControlReplicateTable>();
                        foreach (System.Windows.Forms.Control cc in G.Controls)
                        {
                            if (cc.GetType() == typeof(DiversityWorkbench.UserControls.UserControlReplicateTable))
                            {
                                DiversityWorkbench.UserControls.UserControlReplicateTable U = (DiversityWorkbench.UserControls.UserControlReplicateTable)cc;
                                if (U.NumberOfDatasets == 0)
                                    _ControlsToRemove.Add(U);
                            }
                        }

                        if (_ControlsToRemove.Count > 0)
                        {
                            for (int i = 0; i < _ControlsToRemove.Count; i++)
                            {
                                G.Controls.Remove(_ControlsToRemove[i]);
                                G.Height -= _ControlsToRemove[i].Height;
                                this._ReplicationControls.Remove(_ControlsToRemove[i]);
                            }
                        }
                        if (G.Controls.Count == 0)
                        {
                            System.Windows.Forms.Label L = new Label();
                            L.Text = "No data";
                            G.Controls.Add(L);
                            G.Height += 18;
                            L.Dock = DockStyle.Left;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void AddUserControlReplicateTables(DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection Direction)//, string Subscriber, string Publisher)
        {
            try
            {
                if (Direction == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Clean)
                {
                    if (this.checkBoxData.Checked)
                        this.AddUserControlReplicateTable(this.checkBoxData.Text, this._ReplicationDirection, this.TableDictionaryData);//, Subscriber, Publisher);
                    //if (this.checkBoxBasicData.Checked)
                    //    this.AddUserControlReplicateTable(this.checkBoxBasicData.Text, this._ReplicationDirection, this.TableDictionaryBasicData);//, Subscriber, Publisher);
                    if (this.checkBoxProjects.Checked)
                        this.AddUserControlReplicateTable(this.checkBoxProjects.Text, this._ReplicationDirection, this.TableDictionaryProjectAndUser);//, Subscriber, Publisher);
                    //if (this.checkBoxEntity.Checked)
                    //    this.AddUserControlReplicateTable(this.checkBoxEntity.Text, this._ReplicationDirection, this.TableDictionaryEntity);//, Subscriber, Publisher);
                    if (this.checkBoxEnum.Checked)
                        this.AddUserControlReplicateTable(this.checkBoxEnum.Text, this._ReplicationDirection, this.TableDictionaryEnum);//, Subscriber, Publisher);
                }
                else
                {
                    if (this.checkBoxEnum.Checked)
                        this.AddUserControlReplicateTable(this.checkBoxEnum.Text, this._ReplicationDirection, this.TableDictionaryEnum);//, Subscriber, Publisher);
                    //if (this.checkBoxEntity.Checked)
                    //    this.AddUserControlReplicateTable(this.checkBoxEntity.Text, this._ReplicationDirection, this.TableDictionaryEntity);//, Subscriber, Publisher);
                    if (this.checkBoxProjects.Checked)
                        this.AddUserControlReplicateTable(this.checkBoxProjects.Text, this._ReplicationDirection, this.TableDictionaryProjectAndUser);//, Subscriber, Publisher);
                    //if (this.checkBoxBasicData.Checked)
                    //    this.AddUserControlReplicateTable(this.checkBoxBasicData.Text, this._ReplicationDirection, this.TableDictionaryBasicData);//, Subscriber, Publisher);
                    if (this.checkBoxData.Checked)
                        this.AddUserControlReplicateTable(this.checkBoxData.Text, this._ReplicationDirection, this.TableDictionaryData);//, Subscriber, Publisher);
                }
            }
            catch (System.Exception ex) { }
        }

        private void AddUserControlReplicateTable(string Title,
            DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection Direction,
            System.Collections.Generic.Dictionary<string, string> TableDictionary)//,             string Subscriber, string Publisher)
        {
            try
            {
                System.Windows.Forms.GroupBox G = new GroupBox();
                G.Text = Title;
                G.Height = 20;
                System.Collections.Generic.List<string> TempList = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in TableDictionary)
                    TempList.Add(KV.Key);
                System.Collections.Generic.List<string> TableList = new List<string>();
                if (Direction == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Clean)
                {
                    for (int i = TempList.Count - 1; i > -1; i--)
                        TableList.Add(TempList[i]);
                }
                else
                {
                    foreach (string s in TempList)
                        TableList.Add(s);
                }
                foreach (string Table in TableList)
                {
                    if (this._ReplicationDirection == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Download &&
                        !DiversityWorkbench.Replication.UpdateAndInsertInSubscriberGranted(Table))
                        continue;
                    if ((this._ReplicationDirection == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Merge
                        || this._ReplicationDirection == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Upload)
                        &&
                        !DiversityWorkbench.Replication.UpdateAndInsertInPublisherGranted(Table))
                        continue;
                    string SqlRestriction = this.SqlTable(Table);
                    DiversityWorkbench.UserControls.UserControlReplicateTable U
                        = new DiversityWorkbench.UserControls.UserControlReplicateTable(Table, Direction, TableDictionary[Table], SqlRestriction);//, Subscriber, Publisher
                    try
                    {
                        //System.Drawing.Image I = DiversityCollection.Specimen.ImageForTable(Table, false);
                        //if (I != null)
                        //    U.TableImage = I;
                    }
                    catch (System.Exception ex) { }
                    G.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                    U.BringToFront();
                    G.Height += U.Height;
                    this._ReplicationControls.Add(U);
                }
                G.Dock = DockStyle.Top;
                this.panelTableList.Controls.Add(G);
                G.BringToFront();
            }
            catch (System.Exception ex) { }
        }

        private void setSelectionForTables()
        {
            try
            {
                foreach (DiversityWorkbench.UserControls.UserControlReplicateTable U in this._ReplicationControls)
                    U.TabelIsSelected = this.radioButtonAll.Checked;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void checkBoxIgnoreConflicts_CheckedChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.UserControls.ReplicationRow.IgnoreConflicts = this.checkBoxIgnoreConflicts.Checked;
            //foreach (DiversityWorkbench.UserControls.UserControlReplicateTable U in this._ReplicationControls)
            //    U.IgnoreConflicts = this.checkBoxIgnoreConflicts.Checked;
        }

        #endregion

        #region Table Dictionaries and Restrictions

        //private System.Collections.Generic.Dictionary<string, string> TableDictionaryEntity
        //{
        //    get
        //    {
        //        System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();

        //        Tables.Add("SamplingPlotPointType_Enum", "Code");
        //        Tables.Add("EntityContext_Enum", "Code");
        //        Tables.Add("EntityDetermination_Enum", "Code");
        //        Tables.Add("EntityLanguageCode_Enum", "Code");
        //        Tables.Add("EntityUsage_Enum", "Code");
        //        Tables.Add("EntityVisibility_Enum", "Code");

        //        Tables.Add("Entity", "");
        //        Tables.Add("EntityRepresentation", "");
        //        Tables.Add("EntityUsage", "");
        //        return Tables;
        //    }
        //}

        private System.Collections.Generic.Dictionary<string, string> TableDictionaryEnum
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();

                Tables.Add("SamplingPlotPointType_Enum", "Code");

                Tables.Add("LocalisationSystem", "LocalisationSystemID");
                Tables.Add("Property", "PropertyID");

                return Tables;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> TableDictionaryProjectAndUser
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();

                Tables.Add("ProjectProxy", "ProjectID");
                Tables.Add("UserProxy", "LoginName");
                Tables.Add("ProjectUser", "ProjectID");
                return Tables;
            }
        }

        //private System.Collections.Generic.Dictionary<string, string> TableDictionaryBasicData
        //{
        //    get
        //    {
        //        System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();

        //        Tables.Add("Analysis", "AnalysisID");
        //        Tables.Add("AnalysisResult", "AnalysisID");
        //        Tables.Add("AnalysisTaxonomicGroup", "AnalysisID");
        //        Tables.Add("ProjectAnalysis", "ProjectID");

        //        Tables.Add("Processing", "ProcessingID");
        //        Tables.Add("ProcessingMaterialCategory", "ProcessingID");
        //        Tables.Add("ProjectProcessing", "ProjectID");

        //        Tables.Add("Collection", "CollectionID");
        //        Tables.Add("CollectionImage", "CollectionID");
        //        Tables.Add("CollectionManager", "CollectionID");
        //        Tables.Add("CollectionRequester", "CollectionID");
        //        Tables.Add("ExternalRequestCredentials ", "CollectionID");

        //        Tables.Add("Transaction", "TransactionID");
        //        Tables.Add("TransactionDocument", "TransactionID");

        //        Tables.Add("CollectionExternalDatasource", "");

        //        return Tables;
        //    }
        //}


        private System.Collections.Generic.Dictionary<string, string> TableDictionaryData
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();

                Tables.Add("SamplingPlot", "PlotID");
                Tables.Add("SamplingPlotImage", "PlotID");
                Tables.Add("SamplingPlotLocalisation", "PlotID");
                Tables.Add("SamplingPlotProperty", "PlotID");
                Tables.Add("SamplingProject", "PlotID");
                return Tables;
            }
        }

        /// <summary>
        /// returns a SQL string for the retrieval of all datasets related to the selected project
        /// </summary>
        /// <param name="Table">Name of the table</param>
        /// <returns>SQL statement</returns>
        private string SqlTable(string Table)
        {
            string SQL = "";
            switch (Table)
            {
                case "SamplingProject":
                    //SQL = this._ProjectID.ToString();
                    //break;
                case "SamplingPlotImage":
                case "SamplingPlotLocalisation":
                case "SamplingPlotProperty":
                case "SamplingPlot":
                    if (this._dtPlotID == null)
                    {
                        SQL = "SELECT T.PlotID " +
                            "FROM SamplingProject AS T " +
                            "WHERE T.ProjectID = " + this._ProjectID.ToString();
                        this._dtPlotID = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad; // = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                        if (this._ReplicationDirection == DiversityWorkbench.UserControls.UserControlReplicateTable.ReplicationDirection.Download)
                            ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);
                        else
                            ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber);
                        ad.Fill(this._dtPlotID);
                        if (this._dtPlotID.Rows.Count > 0)
                        {
                            System.Data.DataTable dtHierarchy = new DataTable();
                            foreach (System.Data.DataRow R in this._dtPlotID.Rows)
                            {
                                SQL = "SELECT PlotID FROM dbo.SamplingPlotHierarchy(" + R[0].ToString() + ")";
                                ad.SelectCommand.CommandText = SQL;
                                ad.Fill(dtHierarchy);
                            }
                            if (dtHierarchy.Rows.Count > 0)
                            {
                                foreach (System.Data.DataRow R in dtHierarchy.Rows)
                                {
                                    System.Data.DataRow[] rr = this._dtPlotID.Select("PlotID = " + R[0].ToString());
                                    if (rr.Length == 0)
                                    {
                                        System.Data.DataRow Rnew = this._dtPlotID.NewRow();
                                        Rnew[0] = R[0];
                                        this._dtPlotID.Rows.Add(Rnew);
                                    }
                                }
                            }
                        }
                        ad.Dispose();
                    }
                    string PlotIDs = "";
                    foreach (System.Data.DataRow R in this._dtPlotID.Rows)
                    {
                        if (PlotIDs.Length > 0) PlotIDs += ", ";
                        PlotIDs += R[0].ToString();
                    }
                    if (PlotIDs.Length == 0)
                        SQL = "SELECT NULL";
                    else
                        SQL = PlotIDs;
                    break;

                    break;
            }
            return SQL;
        }

        #endregion

        #region Download

        private void initServerProjectList()
        {
            try
            {
                string SQL = "SELECT Project, ProjectID " +
                    "FROM ProjectList " +
                    "ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ReplicationPublisherConnectionString);
                System.Data.DataTable dtProject = new DataTable();
                ad.Fill(dtProject);
                ad.Dispose();
                this.comboBoxProject.DataSource = dtProject;
                this.comboBoxProject.DisplayMember = "Project";
                this.comboBoxProject.ValueMember = "ProjectID";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonStartDownload_Click(object sender, EventArgs e)
        {
            //Check if project is available in subscriber database
            try
            {
                bool OK = false;
                string SQL = "SELECT COUNT(*) FROM ProjectProxy WHERE ProjectID = " + this.comboBoxProject.SelectedValue.ToString();
                try
                {
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) != "1")
                    {
                        OK = false;
                        string Message = "This project is missing in the subscriber database. Do you want to create it?";
                        if (System.Windows.Forms.MessageBox.Show(Message, "Create project", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            System.Data.DataTable dt = new DataTable();
                            SQL = "SELECT ProjectID, Project, RowGUID, ProjectUri FROM ProjectProxy WHERE ProjectID = " + this.comboBoxProject.SelectedValue.ToString();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher);//, this._ReplicationPublisherConnectionString);
                            ad.Fill(dt);
                            ad.Dispose();
                            if (dt.Rows.Count == 1)
                            {
                                SQL = "INSERT INTO ProjectProxy " +
                                    "(ProjectID, Project, RowGUID, ProjectUri) " +
                                    "VALUES (" + dt.Rows[0][0].ToString() + ", '" + dt.Rows[0][1].ToString() + "', '" + dt.Rows[0][2].ToString() + "', '" + dt.Rows[0][3].ToString() + "')";
                                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                                {
                                    SQL = "INSERT INTO ProjectUser " +
                                        "(LoginName, ProjectID) " +
                                        "VALUES (USER_NAME(), " + dt.Rows[0][0].ToString() + ")";
                                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                                }
                            }
                        }
                    }
                    else OK = true;
                }
                catch (System.Exception ex)
                {
                    OK = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    return;
                }
                if (OK)
                {
                    this.textBoxReport.Text = "";
                    foreach (DiversityWorkbench.UserControls.UserControlReplicateTable U in this._ReplicationControls)
                    {
                        this.SuspendLayout();
                        U.ReplicateTable();
                        if (U.Messages.Count > 0)
                        {
                            this.textBoxReport.Text += "Table " + U.TableName + "\r\n";
                            foreach (string s in U.Messages)
                                this.textBoxReport.Text += s + "\r\n";
                            this.textBoxReport.Text += "\r\n";
                        }
                        this.ResumeLayout();
                    }
                    this.WriteReport();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Upload

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBoxReport.Text = "";
                foreach (DiversityWorkbench.UserControls.UserControlReplicateTable U in this._ReplicationControls)
                {
                    this.SuspendLayout();
                    U.ReplicateTable();
                    if (U.Messages.Count > 0)
                    {
                        this.textBoxReport.Text += "Table " + U.TableName + "\r\n";
                        foreach (string s in U.Messages)
                            this.textBoxReport.Text += s + "\r\n";
                        this.textBoxReport.Text += "\r\n";
                    }
                    this.ResumeLayout();
                }
                this.WriteReport();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Merge

        private void buttonStartMerge_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBoxReport.Text = "";
                foreach (DiversityWorkbench.UserControls.UserControlReplicateTable U in this._ReplicationControls)
                {
                    this.SuspendLayout();
                    U.ReplicateTable();
                    if (U.Messages.Count > 0)
                    {
                        this.textBoxReport.Text += "Table " + U.TableName + "\r\n";
                        foreach (string s in U.Messages)
                            this.textBoxReport.Text += s + "\r\n";
                        this.textBoxReport.Text += "\r\n";
                    }
                    this.ResumeLayout();
                }
                this.WriteReport();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Clean

        private void buttonClean_Click(object sender, EventArgs e)
        {
            try
            {
                this.textBoxReport.Text = "";
                foreach (DiversityWorkbench.UserControls.UserControlReplicateTable U in this._ReplicationControls)
                {
                    if (U.TabelIsSelected)
                    {
                        {
                            this.SuspendLayout();
                            U.ReplicateTable();
                            if (U.Messages.Count > 0)
                            {
                                this.textBoxReport.Text += "Table " + U.TableName + "\r\n";
                                foreach (string s in U.Messages)
                                    this.textBoxReport.Text += s + "\r\n";
                                this.textBoxReport.Text += "\r\n";
                            }
                            this.ResumeLayout();
                        }
                    }
                }
                this.WriteReport();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Report

        private void WriteReport()
        {
            string Reportfile = "";
            System.IO.StreamWriter sw;
            try
            {
                string ReportDirectory = System.Windows.Forms.Application.StartupPath + "\\ReplicationReports";
                System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(ReportDirectory);
                if (!D.Exists)
                    D.Create();
                Reportfile = D.FullName + "\\ReplicationReport_"
                    + System.DateTime.Now.Year.ToString();
                if (System.DateTime.Now.Month < 10)
                    Reportfile += "0";
                Reportfile += System.DateTime.Now.Month.ToString();
                if (System.DateTime.Now.Day < 10)
                    Reportfile += "0";
                Reportfile += System.DateTime.Now.Day.ToString();
                Reportfile += "_";
                if (System.DateTime.Now.Hour < 10)
                    Reportfile += "0";
                Reportfile += System.DateTime.Now.Hour.ToString();
                if (System.DateTime.Now.Minute < 10)
                    Reportfile += "0";
                Reportfile += System.DateTime.Now.Minute.ToString();
                if (System.DateTime.Now.Second < 10)
                    Reportfile += "0";
                Reportfile += System.DateTime.Now.Second.ToString();
                Reportfile += ".txt";
                if (System.IO.File.Exists(Reportfile))
                    sw = new System.IO.StreamWriter(Reportfile, true);
                else
                    sw = new System.IO.StreamWriter(Reportfile);
                string Report = this.textBoxReport.Text;
                string[] ReportLines = Report.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                sw.WriteLine("Replication started: " + System.DateTime.Now.ToShortDateString() + ":" + System.DateTime.Now.ToShortTimeString());
                string User = System.Environment.UserName;// DiversityCollection.LookupTable.CurrentUser;
                if (User.Length == 0)
                    User = System.Environment.UserName;
                sw.WriteLine("User: " + User);
                sw.WriteLine("Replication direction: " + this._ReplicationDirection);
                sw.WriteLine("Publisher: " + DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.Database + " on " + DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionPublisher.DataSource);
                sw.WriteLine("Subscriber: " + DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber.Database + " on " + DiversityWorkbench.UserControls.ReplicationRow.SqlConnectionSubscriber.DataSource);
                sw.WriteLine("");
                for (int i = 0; i < ReportLines.Length; i++)
                    sw.WriteLine(ReportLines[i]);
                this.labelReportFile.Text = "Report saved as: " + Reportfile;
                sw.Close();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Set help keywords for controls where it does not work automatically
        /// </summary>
        private void InitManualControls()
        {
            try
            {
                helpProvider.SetHelpKeyword(this, "replication_dsp");
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }



        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    this.InitManualControls();
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion



    }
}
