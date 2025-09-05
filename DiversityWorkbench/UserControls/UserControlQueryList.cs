using DiversityWorkbench.Forms;
using DiversityWorkbench.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public struct QueryDisplayColumn
    {
        public string DisplayText;
        public string IdentityColumn;
        public string DisplayColumn;
        public string OrderColumn;
        public string TableName;
        public string TipText;
        public string Module;
    }

    public struct QueryRestrictionItem
    {
        public string TableName;
        public string ColumnName;
        public string Restriction;
    }

    public struct OrderBy
    {
        public string DisplayText;
        public QueryDisplayColumn QueryOrderColumn;
        public UserControls.UserControlQueryOrderColumn QueryOrderColumnControl;
        //public int Index;
    }

    public partial class UserControlQueryList : UserControl
    {

        #region Parameter

        private string _QueryConditionVisiblity = "";
        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _QueryConditions;
        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _PredefinedQueryPersistentConditionList;

        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _ProjectDependentQueryConditions;

        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _CurrentQueryConditions;

        private System.Collections.Generic.List<QueryRestrictionItem> _QueryRestrictionList;

        private DiversityWorkbench.UserControls.QueryDisplayColumn[] DisplayColumns;

        private System.Data.DataTable dtQuery;
        private Microsoft.Data.SqlClient.SqlConnection sqlConnection;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapter;
        private int MaxNumOfResult;
        private string SqlWhereClause;
        private string _SqlRestriction = "";
        private string SqlQueryTable;
        //private int _ProjectID = 0;
        private int? _QueryMaxHeight = 0;
        private bool _IsPredefinedQuery = false;
        private bool _GetNext = false;
        private bool _GetPrevious = false;
        private string _GetNextRestriction = "";
        private int _GetNextStart = 1;
        private System.Collections.Generic.Stack<string> _RestrictionStackForNavigation = new Stack<string>();
        private bool _IDisNumeric = true;
        private System.Data.DataTable _DtDisplayColumns;
        private int? _NumberOfResultsets = null;
        private System.Windows.Forms.Control _FirstQueryControl;

        private System.Windows.Forms.ImageList _ImageList;
        private System.Collections.Generic.Dictionary<string, System.Drawing.Color> _TableColors;
        private System.Collections.Generic.Dictionary<string, int> _TableImageIndex;

        private DiversityWorkbench.ServerConnection _ServerConnection;
        private string _MainDataTable;
        private string _QueryMainTableLocal;
        private string _UsedByModule;
        private string _UsedByForm;

        private string _EmbeddedSQL;
        private string _EmbeddedPlaceholder;
        private string _EmbeddedConnectionString;

        public string QueryMainTableLocal
        {
            get { return _QueryMainTableLocal; }
            set { _QueryMainTableLocal = value; }
        }

        private bool _FormatDateToYearMonthDay = true;
        /// <summary>
        /// If in the result list values that are recognized as dates should be formatet to Year-Month-Day
        /// </summary>
        /// <param name="DoFormat">Do format date values</param>
        public void FormatDateToYearMonthDay(bool DoFormat)
        {
            this._FormatDateToYearMonthDay = DoFormat;
        }

        // The main form can subscribe to this event to receive notifications whenever it should suppress the selectedindexchanged event of the querylist.
        public event EventHandler<bool> SuppressSelectedIndexChangedEvent;
        
        #endregion

        #region Construction

        public UserControlQueryList()
        {
            InitializeComponent();
            // setting the Datasource for the list
            this.dtQuery = new DataTable("QueryResult");
            if (this._IDisNumeric)
            {
                System.Data.DataColumn columnID = new System.Data.DataColumn("ID", typeof(int));
                dtQuery.Columns.Add(columnID);
            }
            else
            {
                System.Data.DataColumn columnID = new System.Data.DataColumn("ID", typeof(string));
                dtQuery.Columns.Add(columnID);
            }
            System.Data.DataColumn columnDisplay = new System.Data.DataColumn("Display", typeof(string));
            dtQuery.Columns.Add(columnDisplay);
            System.Data.DataColumn columnOrderBy = new System.Data.DataColumn("OrderBy", typeof(string));
            dtQuery.Columns.Add(columnOrderBy);
            this.listBoxQueryResult.DataSource = dtQuery;
            this.listBoxQueryResult.DisplayMember = "Display";
            this.listBoxQueryResult.ValueMember = "ID";
            this.toolStripButtonConnection.Image = this.imageListConnectionState.Images[1];
            this.initControl();
        }

        private void initControl()
        {
            try
            {
                this.Optimizing_SetUsage(DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        public UserControlQueryList(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions)
            : this()
        {
            this.setQueryConditions(QueryConditions);
        }


        #endregion

        #region Connection and rights

        public string LinkedServer = "";
        public string LinkedServerDatabase = "";

        public void setConnection(string ConnectionString, string MainDataTable)
        {
            try
            {
                this._MainDataTable = MainDataTable;
                this.dtQuery.Clear();
                this.groupBoxQueryResults.Text = DiversityWorkbench.UserControls.UserControlQueryListText.Query_results;// this.Message("Query_results");
                this.IsPredefinedQuery = false;
                this.SqlWhereClause = "";
                DiversityWorkbench.Database.PrivacyConsent PC = Database.PrivacyConsent.undecided;
                if (ConnectionString != "" && DiversityWorkbench.Database.PrivacyConsentOK(ref PC))
                {
                    this.sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                    this.sqlDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter("", this.sqlConnection.ConnectionString);
                    this.setToolstripButtons();
                    this.toolStripButtonConnection.Image = this.imageListConnectionState.Images[0];
                    this.buttonQuery.Enabled = true;
                    this.buttonQueryAdd.Enabled = true;
                    this.buttonQueryRemove.Enabled = true;
                    this.buttonQueryNext.Enabled = true;
                    this.setVisibilityOfApplicationID();
                }
                else
                {
                    this.toolStripButtonCopy.Enabled = false;
                    this.toolStripButtonDelete.Enabled = false;
                    this.toolStripButtonNew.Enabled = false;
                    this.toolStripButtonOptions.Enabled = false;
                    this.toolStripButtonSave.Enabled = false;
                    this.toolStripButtonBacklinkUpdate.Enabled = false;
                    this.toolStripButtonUndo.Enabled = false;
                    this.buttonQuery.Enabled = false;
                    this.buttonQueryAdd.Enabled = false;
                    this.buttonQueryRemove.Enabled = false;
                    this.buttonQueryNext.Enabled = false;
                    this.dtQuery.Clear();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private bool PrivacyConsentOK()
        //{
        //    bool OK = true;
        //    if (DiversityWorkbench.Database.ColumnPrivacyConsentDoesExist())
        //    {
        //        switch (DiversityWorkbench.Database.PrivacyConsentState())
        //        {
        //            case UserSettings.PrivacyConsent.rejected:
        //            case UserSettings.PrivacyConsent.undecided:
        //                DiversityWorkbench.Forms.FormPrivacyConsent f = new DiversityWorkbench.Forms.FormPrivacyConsent();
        //                f.ShowDialog();
        //                if (f.PrivacyConsent() == UserSettings.PrivacyConsent.consented)
        //                    OK = true;
        //                else
        //                    OK = false;
        //                break;
        //            case UserSettings.PrivacyConsent.consented:
        //            case UserSettings.PrivacyConsent.missingUser:
        //                OK = true;
        //                break;
        //        }
        //    }
        //    return OK;
        //}

        private void setToolstripButtons()
        {
            this.toolStripButtonOptions.Enabled = true;
            string SQL = "";
            if (this.sqlConnection != null)
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.sqlConnection.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    bool OK;
                    // Check UPDATE
                    SQL = "IF PERMISSIONS(OBJECT_ID('" + this._MainDataTable + "'))&2=2 SELECT 'True' ELSE SELECT 'False'";
                    C.CommandText = SQL;
                    OK = System.Boolean.Parse(C.ExecuteScalar().ToString());
                    this.toolStripButtonSave.Enabled = OK;
                    this.toolStripButtonBacklinkUpdate.Enabled = OK;
                    this.toolStripButtonUndo.Enabled = OK;
                    // Check INSERT
                    SQL = "IF PERMISSIONS(OBJECT_ID('" + this._MainDataTable + "'))&8=8 SELECT 'True' ELSE SELECT 'False'";
                    C.CommandText = SQL;
                    OK = System.Boolean.Parse(C.ExecuteScalar().ToString());
                    this.toolStripButtonCopy.Enabled = OK;
                    this.toolStripButtonNew.Enabled = OK;
                    // Check DELETE
                    SQL = "IF PERMISSIONS(OBJECT_ID('" + this._MainDataTable + "'))&16=16 SELECT 'True' ELSE SELECT 'False'";
                    C.CommandText = SQL;
                    OK = System.Boolean.Parse(C.ExecuteScalar().ToString());
                    this.toolStripButtonDelete.Enabled = OK;
                    con.Close();
                }
                finally
                {
                    if (con.State.ToString() == "Open")
                        con.Close();
                }
            }
            else
            {
                this.toolStripButtonSave.Enabled = false;
                this.toolStripButtonBacklinkUpdate.Enabled = false;
                this.toolStripButtonUndo.Enabled = false;
                this.toolStripButtonCopy.Enabled = false;
                this.toolStripButtonNew.Enabled = false;
                this.toolStripButtonDelete.Enabled = false;
            }
        }

        public void setToolStripsButtonsForReadOnly(string MainTable, bool ReadOnly)
        {
            try
            {
                this._MainDataTable = MainTable;
                this.setToolstripButtons();
                if (ReadOnly)
                {
                    this.toolStripButtonSave.Enabled = false;
                    this.toolStripButtonBacklinkUpdate.Enabled = false;
                    this.toolStripButtonUndo.Enabled = false;
                    this.toolStripButtonCopy.Enabled = false;
                    if (this.dtQuery.Rows.Count == 0)
                    {
                        string SQL = "IF PERMISSIONS(OBJECT_ID('" + MainTable + "'))&8=8 SELECT 'True' ELSE SELECT 'False'";
                        // Markus 21.8.23: Bugfix, falls keine gültiger bool wert geliefert wird
                        bool ok = false;
                        if (bool.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString(), out ok))
                        {
                            //bool OK = ok; System.Boolean.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString());
                            this.toolStripButtonNew.Enabled = ok; // OK;
                        }
                    }
                    else
                        this.toolStripButtonNew.Enabled = false;
                    this.toolStripButtonDelete.Enabled = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Query: Orientation, Show / Hide

        public void switchOrientation()
        {
            if (this.splitContainerMain.Orientation == Orientation.Horizontal)
            {
                this.splitContainerMain.Orientation = Orientation.Vertical;
                this.toolStripButtonSwitchOrientation.Image = this.imageListOrientation.Images[1];
                this.toolStripButtonSwitchOrientation.Text = DiversityWorkbench.UserControls.UserControlQueryListText.Switch_to_horizontal_orientation;
                this.toolStripButtonSwitchOrientation.ToolTipText = DiversityWorkbench.UserControls.UserControlQueryListText.Switch_to_horizontal_orientation;
                this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.Size.Width / 2;
                this.buttonSetQueryConditionsUpDown.Visible = false;
                this.panelQueryConditions.Visible = true;
                this.splitContainerMain.Panel2Collapsed = false;
            }
            else
            {
                this.splitContainerMain.Orientation = Orientation.Horizontal;
                this.toolStripButtonSwitchOrientation.Image = this.imageListOrientation.Images[0];
                this.toolStripButtonSwitchOrientation.Text = DiversityWorkbench.UserControls.UserControlQueryListText.Switch_to_vertical_orientation;
                this.toolStripButtonSwitchOrientation.ToolTipText = DiversityWorkbench.UserControls.UserControlQueryListText.Switch_to_vertical_orientation;
                this.setQueryConditionsHeight();
                this.buttonSetQueryConditionsUpDown.Visible = true;
                if (bool.Parse(this.buttonSetQueryConditionsUpDown.Tag.ToString()))
                    this.panelQueryConditions.Visible = true;
                else
                    this.panelQueryConditions.Visible = false;
            }
        }

        private void setQueryConditionsHeight()
        {
            if (this.splitContainerMain.Orientation == Orientation.Horizontal)
            {
                if (!bool.Parse(this.buttonSetQueryConditionsUpDown.Tag.ToString()))
                {
                    this.splitContainerMain.SplitterDistance = this.splitContainerMain.Height - 20;
                }
                else
                {
                    if (this._QueryMaxHeight < (int)this.splitContainerMain.Size.Height / 1.5)
                        this.splitContainerMain.SplitterDistance = this.splitContainerMain.Size.Height - (int)this._QueryMaxHeight;
                    else
                    {
                        this.splitContainerMain.SplitterDistance = this.splitContainerMain.Size.Height - (int)(this.splitContainerMain.Size.Height / 1.5);
                    }
                }
            }
        }

        /// <summary>
        /// hiding or showing the query conditions - state is stored in the tag of the button:
        /// True meeans the conditions are shown
        /// False the conditions are hidden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetQueryConditionsUpDown_Click(object sender, EventArgs e)
        {
            if (this.buttonSetQueryConditionsUpDown.Tag == null)
                this.buttonSetQueryConditionsUpDown.Tag = true;
            if (bool.Parse(this.buttonSetQueryConditionsUpDown.Tag.ToString()))
            {
                this.buttonSetQueryConditionsUpDown.Tag = false;
                this.buttonSetQueryConditionsUpDown.Image = DiversityWorkbench.ResourceWorkbench.ArrowUp;
                this.toolTipQueryList.SetToolTip(this.buttonSetQueryConditionsUpDown, DiversityWorkbench.UserControls.UserControlQueryListText.Show_the_query_conditions);
                this.splitContainerMain.Panel2Collapsed = true;
            }
            else
            {
                this.buttonSetQueryConditionsUpDown.Tag = true;
                this.buttonSetQueryConditionsUpDown.Image = DiversityWorkbench.ResourceWorkbench.ArrowDown;
                this.toolTipQueryList.SetToolTip(this.buttonSetQueryConditionsUpDown, DiversityWorkbench.UserControls.UserControlQueryListText.Hide_the_query_conditions);
                this.splitContainerMain.Panel2Collapsed = false;
            }
            this.setQueryConditionsHeight();
        }

        private void buttonShowQueryConditions_Click(object sender, EventArgs e)
        {
            this.IsPredefinedQuery = false;
            this.SqlWhereClause = "";
            this.setQueryConditions(this._QueryConditions);
            this.buttonSetQueryConditionsUpDown.Tag = true;
            this.splitContainerMain.Panel2Collapsed = false;
            this.setQueryConditionsHeight();
            if (this.ModeOfControl == Mode.Embedded)
            {
                try
                {
                    string SQLupdate = this._EmbeddedSQL.Replace(this._EmbeddedPlaceholder, "");
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._EmbeddedConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQLupdate, con);
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                    this.dtQuery.Clear();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        #endregion

        #region QueryConditions

        private int QueryColumnHeight
        {
            get
            {
                return (int)(22.0 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            }
        }
        /// <summary>
        /// Providing a height depending on the type of usercontrol (Markus 2020-09-01 - for future use)
        /// </summary>
        /// <param name="userControlQueryCondition">The user control for which the height should be returned</param>
        /// <returns></returns>
        private int QueryControlHeight(DiversityWorkbench.IUserControlQueryCondition userControlQueryCondition, bool ForAdding = false)
        {
            int Height = QueryColumnHeight;
            switch (userControlQueryCondition.getCondition().QueryType)
            {
                case QueryCondition.QueryTypes.Module:
                    if (ForAdding)
                        Height = (int)(9.0 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    else
                        Height = (int)(31.0 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    break;
                default:
                    break;
            }
            return Height;
        }

        private void addQueryConditionSet(UserControls.UserControlQueryConditionSet Requestor)
        {
            if (Requestor.Parent != null && Requestor.Parent.GetType() == typeof(System.Windows.Forms.GroupBox))
            {
                System.Windows.Forms.GroupBox g = (System.Windows.Forms.GroupBox)Requestor.Parent;
                if (g.Controls.Count < 5) // maximal allowed number of elements ???
                {

                    //setQueryConditions(_QueryConditions);
                    int iTab = 0;
                    foreach (System.Windows.Forms.Control c in g.Controls)
                    {
                        if (iTab < c.TabIndex) iTab = c.TabIndex;
                    }
                    // include removable condition
                    DiversityWorkbench.UserControls.UserControlQueryConditionSet u =
                        new UserControls.UserControlQueryConditionSet(Requestor.getCondition(), DiversityWorkbench.Settings.ConnectionString, removeQueryConditionSet);
                    g.Controls.Add(u);
                    u.Dock = DockStyle.Top;
                    u.BringToFront();
                    u.TabIndex = iTab;
                    u.setUserControlQueryList(this);
                    g.Height += this.QueryControlHeight(u);
                    //g.Height += QueryColumnHeight;
                    this._QueryMaxHeight += this.QueryControlHeight(u);
                    //this._QueryMaxHeight += QueryColumnHeight;
                    setQueryConditionsHeight();
                }
            }
        }

        private void removeQueryConditionSet(UserControls.UserControlQueryConditionSet Requestor)
        {
            if (Requestor.Parent.GetType() == typeof(System.Windows.Forms.GroupBox))
            {
                System.Windows.Forms.GroupBox g = (System.Windows.Forms.GroupBox)Requestor.Parent;
                if (g.Controls.Count > 1) // at least one entry must be present
                {
                    // remove condition
                    g.Controls.Remove(Requestor);
                    Requestor.Dispose();
                    g.Height -= this.QueryControlHeight(Requestor);
                    this._QueryMaxHeight -= this.QueryControlHeight(Requestor);
                    //g.Height -= QueryColumnHeight;
                    //this._QueryMaxHeight -= QueryColumnHeight;
                    setQueryConditionsHeight();
                }
            }
        }

        public void setQueryConditions(string QueryTitle, string QueryTable, string WhereClause)
        {
            if (WhereClause.Length > 0)
            {
                try
                {
                    this.SqlQueryTable = QueryTable;
                    // MW 9.4.2015: Optimizing
                    if (UseOptimizing)
                        QueryMainTable = QueryTable;
                    this.SqlWhereClause = WhereClause;
                    this.dtQuery.Clear();
                    if (this.QueryDatabase(false))
                    {
                        int Height = 0;
                        this.panelQueryConditions.Controls.Clear();
                        if (WhereClause.Length > 0 && this._PredefinedQueryPersistentConditionList != null && this._PredefinedQueryPersistentConditionList.Count > 0)
                        {
                            Height = this.setPredefinedQueryPersistentConditions();
                        }
                        this.panelQueryConditions.Controls.Add(this.textBoxSQL);
                        this.textBoxSQL.Visible = true;
                        this.textBoxSQL.Text = "";
                        this.textBoxSQL.Dock = DockStyle.Fill;
                        if (QueryTitle.Length > 0)
                            this.textBoxSQL.Text = QueryTitle + "\r\n\r\n";
                        this.textBoxSQL.Text += WhereClause;
                        //if (IsPredefinedQuery)
                        //{
                        //    this.textBoxSQL.Text = QueryTitle + "\r\n\r\n" + WhereClause;
                        //}
                        //else
                        //    this.textBoxSQL.Text = QueryTitle + "\r\n\r\n" + WhereClause;
                        this.textBoxSQL.BringToFront();
                        double h = this.textBoxSQL.Font.SizeInPoints;
                        double l = (double)this.textBoxSQL.Text.Length * h / (double)this.textBoxSQL.Width;
                        Height += (int)(l * (h + 3));
                        if (Height > 200) Height = 200;
                        this._QueryMaxHeight = Height + 50;
                        this.setQueryConditionsHeight();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        public void setQueryConditions(string QueryTitle, string QueryTable, string WhereClause, string Description)
        {
            if (WhereClause.Length > 0 && this._PredefinedQueryPersistentConditionList != null && this._PredefinedQueryPersistentConditionList.Count > 0)
            {
                this.setPredefinedQueryPersistentConditions();
            }
            this.setQueryConditions(QueryTitle, QueryTable, WhereClause);
            if (WhereClause.Length > 0
                && this.textBoxSQL.Visible == true
                && this.textBoxSQL.Dock == DockStyle.Fill
                && Description.Length > 0)
            {
                try
                {
                    this.textBoxSQL.Text = "";
                    if (QueryTitle.Length > 0)
                        this.textBoxSQL.Text = QueryTitle + "\r\n\r\n";
                    if (Description.Length > 0)
                        this.textBoxSQL.Text += Description + "\r\n\r\n";
                    this.textBoxSQL.Text += WhereClause + "\r\n\r\n";
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        //public void setQueryConditions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions)
        //{

        //    // Start des Programms
        //    /* 1
        //     *  	DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.setSearchOptions() Zeile 2516 + 0x54 Bytes	C#
        //          DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.setDatabase() Zeile 3266 + 0x8 Bytes	C#
        //          DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.FormCollectionSpecimen() Zeile 161 + 0x8 Bytes	C#
        //     * 
        //     * 2
        //     *  	DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.setSearchOptions() Zeile 2516 + 0x54 Bytes	C#
        //          DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.initQuery() Zeile 2503 + 0x8 Bytes	C#
        //          DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.FormCollectionSpecimen() Zeile 165 + 0x8 Bytes	C#
        //    */

        //    // Verbindung zur Datenbank
        //    /* 	
        //     * DiversityWorkbench.dll!DiversityWorkbench.UserControls.UserControlQueryList.setQueryConditions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = Count = 115, string QueryConditionVisibility = "1001110000000000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000") Zeile 451 + 0xb Bytes	C#
        //        DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.setSearchOptions() Zeile 2519 + 0x54 Bytes	C#
        //        DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.initQuery() Zeile 2506 + 0x8 Bytes	C#
        //        DiversityCollection.exe!DiversityCollection.FormCollectionSpecimen.databaseToolStripMenuItem_Click(object sender = {System.Windows.Forms.ToolStripButton}, System.EventArgs e = {System.EventArgs}) Zeile 267 + 0x8 Bytes	C#
        //    */

        //    // Aenderung der Conditions
        //    /*
        //     *  	DiversityWorkbench.dll!DiversityWorkbench.UserControls.UserControlQueryList.setQueryConditions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = Count = 115, string QueryConditionVisibility = "1001110000000000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000") Zeile 459 + 0xb Bytes	C#
        //      DiversityWorkbench.dll!DiversityWorkbench.UserControls.UserControlQueryList.toolStripButtonOptions_Click(object sender = {Suchkriterien festlegen}, System.EventArgs e = {System.EventArgs}) Zeile 587 + 0x2d Bytes	C#
        //    */

        //    //this._FirstQueryControl = null;
        //    try
        //    {
        //        if (this._QueryConditionVisiblity.Length > 0)
        //        {
        //            int i = 0;
        //            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QCs = new List<QueryCondition>();
        //            foreach (DiversityWorkbench.QueryCondition QC in QueryConditions)
        //            {
        //                DiversityWorkbench.QueryCondition Qn = QC;
        //                if (this._QueryConditionVisiblity.Length <= i)
        //                    this._QueryConditionVisiblity += QC.SetCount.ToString();// "1";
        //                if (this._QueryConditionVisiblity[i].ToString() != "0")
        //                {
        //                    Qn.showCondition = true;
        //                }
        //                else
        //                    Qn.showCondition = false;
        //                i++;
        //                QCs.Add(Qn);
        //            }
        //            this._QueryConditions = QCs;
        //        }
        //        else
        //            this._QueryConditions = QueryConditions;
        //        this.panelQueryConditions.Controls.Clear();
        //        int iTab = 2;
        //        int H = QueryColumnHeight;
        //        this.textBoxSQL.Visible = false;
        //        System.Collections.Generic.List<string> QueryGroups = new List<string>();
        //        this.SuspendLayout();
        //        this.panelQueryConditions.Visible = false;

        //        this.ProjectDependentQueryConditions.Clear();
        //        this.panelQueryConditions.SuspendLayout();
        //        foreach (DiversityWorkbench.QueryCondition Q in this._QueryConditions)
        //        {
        //            try
        //            {
        //                if (Q.showCondition)
        //                {
        //                    if (!QueryGroups.Contains(Q.QueryGroup))
        //                    {
        //                        QueryGroups.Add(Q.QueryGroup);
        //                        System.Windows.Forms.GroupBox g = new GroupBox();
        //                        this.panelQueryConditions.Controls.Add(g);
        //                        if (iTab > -1) g.TabIndex = iTab;
        //                        iTab--;
        //                        g.Text = Q.QueryGroup;

        //                        if (this.TableColors != null &&
        //                            this.TableColors.ContainsKey(Q.Table))
        //                            g.ForeColor = this.TableColors[Q.Table];
        //                        if (this.TableImageIndex != null &&
        //                            this.ImageList != null &&
        //                            (this.TableImageIndex.ContainsKey(Q.Table) ||
        //                            this.TableImageIndex.ContainsKey(Q.QueryGroup.Replace(" ", ""))))
        //                        {
        //                            System.Windows.Forms.PictureBox p = new PictureBox();
        //                            if (this.TableImageIndex.ContainsKey(Q.QueryGroup.Replace(" ", "")))
        //                                p.Image = this.ImageList.Images[this.TableImageIndex[Q.QueryGroup.Replace(" ", "")]];
        //                            else if (this.TableImageIndex.ContainsKey(Q.QueryGroup))
        //                                p.Image = this.ImageList.Images[this.TableImageIndex[Q.QueryGroup]];
        //                            else
        //                            {
        //                                if (Q.QueryFields != null && Q.QueryFields.Count > 1 && Q.QueryFields[0].TableName != Q.QueryFields[1].TableName)
        //                                    p.Image = DiversityWorkbench.Properties.Resources.Filter;
        //                                else
        //                                    p.Image = this.ImageList.Images[this.TableImageIndex[Q.Table]];
        //                            }
        //                            p.Height = 16;
        //                            p.Width = 16;
        //                            p.Dock = DockStyle.Left;
        //                            System.Windows.Forms.Padding P = new Padding(0, -10, 0, 0);
        //                            p.Margin = P;
        //                            g.Controls.Add(p);
        //                        }
        //                        else
        //                        {
        //                        }

        //                        if (DiversityWorkbench.Entity.EntityTablesExist)
        //                        {
        //                            System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation(Q.QueryGroup);
        //                            if (Entity["DisplayGroup"].Length > 0)
        //                                g.AccessibleName = Entity["DisplayGroup"];
        //                            else
        //                            {
        //                                if (!Q.useGroupAsEntityForGroups)
        //                                    Entity = DiversityWorkbench.Entity.EntityInformation(Q.Table + "." + Q.QueryGroup);
        //                                else if (Entity["DisplayText"].Length > 0)
        //                                    g.Text = Entity["DisplayText"];
        //                                if (Entity["DisplayGroup"].Length > 0)
        //                                    g.AccessibleName = Entity["DisplayGroup"];
        //                                else
        //                                {
        //                                    string Table = Q.Table;
        //                                    if (Table.IndexOf("_Core") > -1)
        //                                        Table = Table.Substring(0, Table.IndexOf("_Core"));
        //                                    if (!Q.useGroupAsEntityForGroups)
        //                                        g.AccessibleName = Table;
        //                                    else
        //                                        g.AccessibleName = Entity["Entity"];
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            string Table = Q.Table;
        //                            if (Table.EndsWith("_Core"))
        //                                Table = Table.Substring(0, Table.IndexOf("_Core"));
        //                            g.AccessibleName = Table;
        //                        }

        //                        g.Dock = DockStyle.Top;
        //                        g.BringToFront();
        //                        H += this.fillQueryGroupBox(g);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            }
        //        }
        //        this.panelQueryConditions.ResumeLayout();
        //        //this.setQueryConditionsDisplayTexts();
        //        this.setEntity();
        //        this._QueryMaxHeight = H;
        //        this.setQueryConditionsHeight();
        //        this._QueryMaxHeight = H;
        //        this.panelQueryConditions.Visible = true;
        //        if (this._QueryConditions.Count == 0)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("DWB.UserControlQueryList: public void setQueryConditions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions) - this._QueryConditions are empty");
        //        }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    this.ResumeLayout();
        //}

        private void ProcessQueryConditions(Func<DiversityWorkbench.QueryCondition, bool> predicate, Action<DiversityWorkbench.QueryCondition> action)
        {
            if (_QueryConditions == null || _QueryConditions.Count == 0)
                return;
            foreach (var queryCondition in _QueryConditions)
            {
                if (predicate(queryCondition))
                {
                    try
                    {
                        action(queryCondition);
                    }
                    catch (Exception ex)
                    {
                        // Log exception instead of swallowing it silently
                        // LogException(ex);
                    }
                }
            }
        }
        
        public void setQueryConditions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions)
        {
            try
            {
                if (QueryConditions == null || QueryConditions.Count == 0)
                {
                    LogEmptyQueryConditions();
                    return;
                }
                // Filter and update query conditions visibility
                UpdateQueryConditionsVisibility(QueryConditions);
                // Clear and prepare the panel
                PrepareQueryConditionsPanel();
                // Group and create UI elements
                int totalHeight = QueryColumnHeight;
                int tabIndex = 2;
                var queryGroups = new HashSet<string>();
                var groupBoxes = new List<GroupBox>();
                ProcessQueryConditions(
                    Q => Q.showCondition,
                    Q =>
                    {
                        if (!queryGroups.Contains(Q.QueryGroup))
                        {
                            queryGroups.Add(Q.QueryGroup);
                            var groupBox = CreateGroupBoxForQueryCondition(Q, ref tabIndex);
                            groupBoxes.Add(groupBox);
                            totalHeight += fillQueryGroupBox(groupBox);
                        }
                    });
                // Add GroupBoxes to the panel in reverse order
                for (int i = groupBoxes.Count - 1; i >= 0; i--)
                {
                    this.panelQueryConditions.Controls.Add(groupBoxes[i]);
                }
                FinalizeQueryConditionsPanel(totalHeight);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        private void UpdateQueryConditionsVisibility(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions)
        {
            if (string.IsNullOrEmpty(this._QueryConditionVisiblity))
            {
                this._QueryConditions = QueryConditions;
                return;
            }
            var updatedConditions = new List<DiversityWorkbench.QueryCondition>();
            for (int i = 0; i < QueryConditions.Count; i++)
            {
                var condition = QueryConditions[i];
                if (this._QueryConditionVisiblity.Length <= i)
                {
                    this._QueryConditionVisiblity += condition.SetCount.ToString();
                }
                condition.showCondition = this._QueryConditionVisiblity[i] != '0';
                updatedConditions.Add(condition);
            }
            this._QueryConditions = updatedConditions;
        }
        private void PrepareQueryConditionsPanel()
        {
            this.panelQueryConditions.Controls.Clear();
            this.panelQueryConditions.Visible = false;
            this.SuspendLayout();
            this.panelQueryConditions.SuspendLayout();
            this.ProjectDependentQueryConditions.Clear();
        }

        private System.Windows.Forms.GroupBox CreateGroupBoxForQueryCondition(DiversityWorkbench.QueryCondition queryCondition, ref int tabIndex)
        {
            var groupBox = new System.Windows.Forms.GroupBox
            {
                Text = queryCondition.QueryGroup,
                Dock = DockStyle.Top,
                AccessibleName = GetAccessibleNameForQueryCondition(queryCondition)
            };
            if (tabIndex > -1) groupBox.TabIndex = tabIndex;
            tabIndex--;
            ApplyGroupBoxStyles(groupBox, queryCondition);
            //this.panelQueryConditions.Controls.Add(groupBox);
            return groupBox;
        }
       
        private string GetAccessibleNameForQueryCondition(DiversityWorkbench.QueryCondition queryCondition)
        {
            if (DiversityWorkbench.Entity.EntityTablesExist)
            {
                var entityInfo = DiversityWorkbench.Entity.EntityInformation(queryCondition.QueryGroup);
                return !string.IsNullOrEmpty(entityInfo["DisplayGroup"])
                    ? entityInfo["DisplayGroup"]
                    : queryCondition.Table;
            }
            return queryCondition.Table.EndsWith("_Core")
                ? queryCondition.Table.Substring(0, queryCondition.Table.IndexOf("_Core"))
                : queryCondition.Table;
        }
        private void ApplyGroupBoxStyles(GroupBox groupBox, DiversityWorkbench.QueryCondition queryCondition)
        {
            if (this.TableColors != null && this.TableColors.ContainsKey(queryCondition.Table))
            {
                groupBox.ForeColor = this.TableColors[queryCondition.Table];
            }
            if (this.TableImageIndex != null && this.ImageList != null)
            {
                var pictureBox = new PictureBox
                {
                    Height = 16,
                    Width = 16,
                    Dock = DockStyle.Left,
                    Margin = new Padding(0, -10, 0, 0),
                    Image = GetGroupBoxImage(queryCondition)
                };
                groupBox.Controls.Add(pictureBox);
            }
        }
        private Image GetGroupBoxImage(DiversityWorkbench.QueryCondition queryCondition)
        {
            if (this.TableImageIndex.ContainsKey(queryCondition.QueryGroup.Replace(" ", "")))
            {
                return this.ImageList.Images[this.TableImageIndex[queryCondition.QueryGroup.Replace(" ", "")]];
            }
            if (this.TableImageIndex.ContainsKey(queryCondition.QueryGroup))
            {
                return this.ImageList.Images[this.TableImageIndex[queryCondition.QueryGroup]];
            }
            if (queryCondition.QueryFields != null && queryCondition.QueryFields.Count > 1 &&
                queryCondition.QueryFields[0].TableName != queryCondition.QueryFields[1].TableName)
            {
                return DiversityWorkbench.Properties.Resources.Filter;
            }
            // #251
            if (this.TableImageIndex.ContainsKey(queryCondition.Table))
            {
                return this.ImageList.Images[this.TableImageIndex[queryCondition.Table]];
            }
            else
                return DiversityWorkbench.Properties.Resources.Filter; // Default image if no specific image is found
        }
        private void FinalizeQueryConditionsPanel(int totalHeight)
        {
            this.panelQueryConditions.ResumeLayout();
            this.ResumeLayout();
            this._QueryMaxHeight = totalHeight;
            this.setQueryConditionsHeight();
            this.panelQueryConditions.Visible = true;
            if (this._QueryConditions.Count == 0)
            {
                LogEmptyQueryConditions();
            }
        }
        private void LogEmptyQueryConditions()
        {
            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(
                "DWB.UserControlQueryList: public void setQueryConditions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions) - this._QueryConditions are empty");
        }


        public void setQueryConditions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions, string QueryConditionVisibility)
        {
            try
            {
                if (QueryConditionVisibility.Length == 0)
                {
                    foreach (DiversityWorkbench.QueryCondition Q in QueryConditions)
                    {
                        if (Q.showCondition)
                        {
                            this._QueryConditionVisiblity += Q.SetCount.ToString();// "1";
                        }
                        else this._QueryConditionVisiblity += "0";
                    }
                }
                else
                    this._QueryConditionVisiblity = QueryConditionVisibility;
                this.setQueryConditions(QueryConditions);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// setting the conditions that will be shown in addition to a predefined sql query
        /// </summary>
        /// <returns>the hight that the conditions need</returns>
        private int setPredefinedQueryPersistentConditions()
        {
            int H = 0;
            if (this._PredefinedQueryPersistentConditionList.Count > 0)
            {
                this.panelQueryConditions.Controls.Clear();
                int iTab = 2;
                H = QueryColumnHeight;
                System.Collections.Generic.List<string> QueryGroups = new List<string>();
                this.SuspendLayout();
                this.panelQueryConditions.Visible = false;
                foreach (DiversityWorkbench.QueryCondition Q in this._PredefinedQueryPersistentConditionList)
                {
                    if (!QueryGroups.Contains(Q.QueryGroup))
                    {
                        QueryGroups.Add(Q.QueryGroup);
                        System.Windows.Forms.GroupBox g = new GroupBox();
                        this.panelQueryConditions.Controls.Add(g);
                        if (iTab > -1) g.TabIndex = iTab;
                        iTab--;
                        g.Text = Q.QueryGroup;
                        g.Dock = DockStyle.Top;
                        g.BringToFront();
                        H += this.fillQueryGroupBox(g);
                    }
                }
                this._QueryMaxHeight = H;
                this.setQueryConditionsHeight();
                this._QueryMaxHeight = H;
                this.panelQueryConditions.Visible = true;
                this.textBoxSQL.BringToFront();
                this.splitContainerMain.Panel2Collapsed = false;
                this.panelQueryConditions.Visible = true;
                this.ResumeLayout();
            }
            return H;
        }

        private UserControl CreateControlForQueryCondition(DiversityWorkbench.QueryCondition Q)
        {
            switch (Q.QueryType)
            {
                case QueryCondition.QueryTypes.Geography:
                    DiversityWorkbench.UserControls.UserControlQueryConditionGIS u = new UserControls.UserControlQueryConditionGIS(Q, DiversityWorkbench.Settings.ConnectionString);
                    u.setUserControlQueryList(this);
                    return u;
                case QueryCondition.QueryTypes.Hierarchy:
                    var hierarchyControl = new UserControls.UserControlQueryConditionHierarchy(Q, DiversityWorkbench.Settings.ConnectionString);
                    hierarchyControl.setUserControlQueryList(this);
                    Q.iUserControlQueryCondition = hierarchyControl;
                    if (Q.DependsOnCurrentProjectID)
                    {
                        Q.RequeryHierarchySource();
                        ProjectDependentQueryConditions.Add(Q);
                    }
                    return hierarchyControl;
                case QueryCondition.QueryTypes.Annotation:
                case QueryCondition.QueryTypes.AnnotationReference:
                    UserControls.UserControlQueryConditionAnnotation anno = new UserControls.UserControlQueryConditionAnnotation(Q, DiversityWorkbench.Settings.ConnectionString);
                    anno.setUserControlQueryList(this);
                    return anno;
                case QueryCondition.QueryTypes.ReferencingTable:
                    DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable referencingTable = new UserControls.UserControlQueryConditionReferencingTable(Q, DiversityWorkbench.Settings.ConnectionString);
                    referencingTable.setUserControlQueryList(this);
                    return referencingTable;
                case QueryCondition.QueryTypes.Text when Q.IsSet:
                    return CreateTextQueryConditionControl(Q);
                case QueryCondition.QueryTypes.Module:
                    return new UserControls.UserControlQueryConditionModule(Q, DiversityWorkbench.Settings.ConnectionString);
                default:
                    var defaultControl = new UserControlQueryCondition(Q, DiversityWorkbench.Settings.ConnectionString);
                    if (Q.DependsOnCurrentProjectID)
                    {
                        Q.iUserControlQueryCondition = defaultControl;
                        ProjectDependentQueryConditions.Add(Q);
                    }
                    defaultControl.setUserControlQueryList(this);
                    if (Q.DependsOnCurrentProjectID)
                    {
                        Q.iUserControlQueryCondition = defaultControl;
                        this.ProjectDependentQueryConditions.Add(Q);
                    }
                    if (Q.Column == "ProjectID")
                    {
                        defaultControl.comboBoxQueryCondition.SelectedIndexChanged += this.comboBoxQueryConditionProject_SelectedIndexChanged;
                    }
                    return defaultControl;
            }
        }

        private UserControl CreateTextQueryConditionControl(DiversityWorkbench.QueryCondition Q)
        {
            // Simplified logic for text query conditions
            Q.SetPosition = 0;
            UserControls.UserControlQueryConditionSet u = new UserControls.UserControlQueryConditionSet(Q, DiversityWorkbench.Settings.ConnectionString, addQueryConditionSet, removeQueryConditionSet);
            u.setUserControlQueryList(this);
            return u;
        }

        private int GetControlHeight(UserControl control, DiversityWorkbench.QueryCondition Q)
        {
            //if (Q.QueryType == QueryCondition.QueryTypes.Hierarchy)
            //    return (int)(14.0 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);

            // #139
            if (Q.QueryType == QueryCondition.QueryTypes.Module)
                return DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(32);

            int Height = QueryColumnHeight;
            return Height;
            // return QueryControlHeight(control);
        }

        private int fillQueryGroupBox(System.Windows.Forms.GroupBox GroupBox)
        {
            int H = 20;
            int iTab = 0;
            if (GroupBox == null)
                return H;
            string groupBoxText = GroupBox.Text;
            string groupBoxAccessibleName = GroupBox.AccessibleName;
            ProcessQueryConditions(
                Q => Q.showCondition && (Q.QueryGroup == groupBoxText || Q.QueryGroup == groupBoxAccessibleName),
                Q =>
                {
                    UserControl control = CreateControlForQueryCondition(Q);
                    if (control != null)
                    {
                        GroupBox.Controls.Add(control);
                        control.Dock = DockStyle.Top;
                        control.BringToFront();
                        control.TabIndex = iTab++;
                        H += GetControlHeight(control, Q);
                    }
                });
            //foreach (var Q in _QueryConditions)
            //{
            //    if (!Q.showCondition ||
            //        !(Q.QueryGroup == groupBoxText || Q.QueryGroup == groupBoxAccessibleName))
            //        continue;
            //    try
            //    {
            //        UserControl control = CreateControlForQueryCondition(Q);
            //        if (control != null)
            //        {
            //            GroupBox.Controls.Add(control);
            //            control.Dock = DockStyle.Top;
            //            control.BringToFront();
            //            control.TabIndex = iTab++;
            //            H += GetControlHeight(control, Q);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // Log exception instead of swallowing it silently
            //        // LogException(ex);
            //    }
            //}
            GroupBox.Height = H;
            return H;
        }

        //private int fillQueryGroupBox(System.Windows.Forms.GroupBox GroupBox)
        //{
        //    int H = 20;
        //    int iTab = 0;
        //    if (_QueryConditions == null || _QueryConditions.Count == 0)
        //        return H;
        //    string groupBoxText = GroupBox.Text;
        //    string groupBoxAccessibleName = GroupBox.AccessibleName;
        //    foreach (var Q in _QueryConditions)
        //    {
        //        if (!Q.showCondition ||
        //            !(Q.QueryGroup == groupBoxText || Q.QueryGroup == groupBoxAccessibleName))
        //            continue;
        //        try
        //        {
        //            UserControl control = CreateControlForQueryCondition(Q);
        //            if (control != null)
        //            {
        //                GroupBox.Controls.Add(control);
        //                control.Dock = DockStyle.Top;
        //                control.BringToFront();
        //                control.TabIndex = iTab++;
        //                H += GetControlHeight(control, Q);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log exception instead of swallowing it silently
        //            // LogException(ex);
        //        }
        //    }
        //    GroupBox.Height = H;
        //    return H;
        //}


        private void comboBoxQueryConditionProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (DiversityWorkbench.QueryCondition Q in this.ProjectDependentQueryConditions)
            {
                Q.RequeryHierarchySource();
            }
        }

        private void textBoxQueryCondition_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                /// MW 21.4.2015: Suppress start of query if an autocomplete source is available
                /// otherwise adding of query results is not possible
                try
                {
                    if (sender.GetType() == typeof(System.Windows.Forms.TextBox))
                    {
                        System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
                        if (T.AutoCompleteCustomSource != null &&
                            T.AutoCompleteCustomSource.Count > 0 &&
                            T.AutoCompleteMode == AutoCompleteMode.SuggestAppend &&
                            T.AutoCompleteSource == AutoCompleteSource.CustomSource)
                            return;
                    }
                }
                catch (System.Exception ex) { }
                this.buttonQuery_Click(null, null);
            }
        }

        #endregion

        #region Choosing the active options

        public event EventHandler QueryConditionVisibilityUserChange;

        protected virtual void OnQueryConditionVisibilityUserChange(EventArgs e)
        {
            EventHandler handler = QueryConditionVisibilityUserChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void toolStripButtonOptions_Click(object sender, EventArgs e)
        {
            this._CurrentQueryConditions = this.CurrentQueryConditions;

            if (this._CurrentQueryConditions.Count == 0 && this.ParentForm != null)
            {

            }

            DiversityWorkbench.Forms.FormQueryOptions f;
            if (this._TableColors != null &&
                this._TableImageIndex != null &&
                this._ImageList != null)
                f = new DiversityWorkbench.Forms.FormQueryOptions(this._QueryConditions, this.MaximalNumberOfResults,
                    this.ImageList, this.TableColors, this.TableImageIndex);
            else
                f = new DiversityWorkbench.Forms.FormQueryOptions(this._QueryConditions, this.MaximalNumberOfResults);
            f.TopMost = true;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                DiversityWorkbench.WorkbenchSettings.Default.Save();
                this._QueryConditions = f.QueryConditionList;
                this.setQueryConditions(this._QueryConditions, f.QueryConditionVisibility);
                this.MaxNumOfResult = f.MaximalNumberOfResults;
                this.toolTipQueryList.SetToolTip(this.buttonQueryNext, DiversityWorkbench.UserControls.UserControlQueryListText.Get_next + " " + this.MaxNumOfResult.ToString() + " " + DiversityWorkbench.UserControls.UserControlQueryListText.items);
                this.toolTipQueryList.SetToolTip(this.buttonQueryPrevious, DiversityWorkbench.UserControls.UserControlQueryListText.Get_previous + " " + this.MaxNumOfResult.ToString() + " " + DiversityWorkbench.UserControls.UserControlQueryListText.items);
                this.buttonQueryNext.Visible = false;
                if (this._CurrentQueryConditions.Count > 0)
                {
                    this.TransferCurrentQueryConditions();
                }
                this.setEntity();

                if (this.RememberQuerySettings())
                    this.RememberQueryConditionSettings_SaveToFile();

                OnQueryConditionVisibilityUserChange(e);
            }
        }

        #endregion

        #region public Interface

        public void initEmbedment(string SQL, string PlaceHolder, string ConnectionString)
        {
            this._EmbeddedConnectionString = ConnectionString;
            this._EmbeddedPlaceholder = PlaceHolder;
            this._EmbeddedSQL = SQL;
            this.sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            this.sqlDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter("", DiversityWorkbench.Settings.ConnectionString);
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> getPredefinedQueryPersistentConditionList()
        {
            if (this._PredefinedQueryPersistentConditionList == null)
                this._PredefinedQueryPersistentConditionList = new List<QueryCondition>();
            return _PredefinedQueryPersistentConditionList;
        }

        public void setPredefinedQueryPersistentConditionList(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> List)
        {
            this._PredefinedQueryPersistentConditionList = List;
        }

        /// <summary>
        /// A list of restrictions for the query
        /// </summary>
        public System.Collections.Generic.List<QueryRestrictionItem> getQueryRestrictionList()
        {
            if (this._QueryRestrictionList == null) this._QueryRestrictionList = new List<QueryRestrictionItem>();
            return this._QueryRestrictionList;
        }

        /// <summary>
        /// A list of restrictions for the query
        /// </summary>
        public void setQueryRestrictionList(System.Collections.Generic.List<QueryRestrictionItem> QueryRestrictionList)
        {
            this._QueryRestrictionList = QueryRestrictionList;
        }

        public void StartQuery()
        {
            this.buttonQuery_Click(null, null);
            this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
        }

        public void RefreshItemDisplayText(System.Data.DataSet Dataset)
        {
            try
            {
                string QueryView = "";
                string QueryTable = "";
                string IdentityColumn = "";
                string DisplayColumn = "";
                foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                {
                    if (C.DisplayColumn == this.SelectedDisplayColumn)
                    {
                        QueryTable = C.TableName;
                        QueryView = QueryTable;
                        IdentityColumn = C.IdentityColumn;
                        DisplayColumn = C.DisplayColumn;
                        break;
                    }
                }
                if (QueryTable.IndexOf("_") > -1)
                    QueryTable = QueryTable.Substring(0, QueryTable.IndexOf("_"));
                // Toni 20201021 Actualize only currently selected entry of result list
                // If DisplayColumn contains a SELECT command, read value from database
                if (Dataset.Tables.Contains(QueryTable))
                {
                    if (Dataset.Tables.Contains(QueryTable) /*[QueryTable].Columns.Contains(DisplayColumn)*/ && Dataset.Tables[QueryTable].Columns.Contains(IdentityColumn))
                    {
                        // Find query result for current list entry
                        for (int j = 0; j < dtQuery.Rows.Count; j++)
                        {
                            if (this.listBoxQueryResult.SelectedValue != null && dtQuery.Rows[j]["ID"].ToString() == this.listBoxQueryResult.SelectedValue.ToString())
                            {
                                for (int i = 0; i < Dataset.Tables[QueryTable].Rows.Count; i++)
                                {
                                    try
                                    {
                                        if (Dataset.Tables[QueryTable].Rows[i][IdentityColumn].ToString() == dtQuery.Rows[j]["ID"].ToString())
                                        {
                                            string Display = "";
                                            if (Dataset.Tables[QueryTable].Columns.Contains(DisplayColumn))
                                                Display = Dataset.Tables[QueryTable].Rows[i][DisplayColumn].ToString();
                                            else if (DisplayColumn.ToLower().Contains("select"))
                                            {
                                                // Read display column from database
                                                string SQL = "SELECT " + DisplayColumn + " FROM " + QueryView + " WHERE " + IdentityColumn + "='" + dtQuery.Rows[j]["ID"].ToString() + "'";
                                                Display = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);

                                                // Preserve old value if access was not sussessful
                                                if (Display.Length == 0)
                                                    Display = dtQuery.Rows[j]["Display"].ToString();
                                            }
                                            if (Display.Length == 0 && QueryTable != QueryView) // Markus 30.1.2023: Bei abweichender sicht sollte diese noch geprüft werden
                                            {
                                                string SQL = "SELECT " + DisplayColumn + " FROM " + QueryView + " WHERE " + IdentityColumn + "='" + dtQuery.Rows[j]["ID"].ToString() + "'";
                                                Display = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                                            }
                                            if (Display.Length == 0)
                                                Display = "ID: " + Dataset.Tables[QueryTable].Rows[i][IdentityColumn].ToString();
                                            if (dtQuery.Rows[j]["Display"].ToString() != Display)
                                            {
                                                if (this.ManyOrderByColumns())
                                                {
                                                    ///ToDo: wenn nicht die Haupttabelle gewaehlt ist, e.g. Identification statt Specimen wird der Text fuer Specimen ermittelt, nicht fuer Identification
                                                    ///dafuer braeuchte es den gesamten Schlüssel von e.g. Identification und nicht nur die Haupt ID der Tabelle
                                                    /// Angabe existiert in QueryList nicht und kann daher nicht übergeben werden
                                                    //Display = this.ManyOrderByColumns_DisplayTextForRow(Dataset, QueryTable, i);

                                                    Display = this.ManyOrderByColumns_DisplayText(int.Parse(dtQuery.Rows[j]["ID"].ToString()));
                                                }
                                                dtQuery.Rows[j]["Display"] = Display;
                                            }
                                            return; // Actualize only one value
                                        }
                                    }
                                    catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                                }
                                // Terminate after match
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            // OLD code
            //foreach (System.Data.DataTable T in Dataset.Tables)
            //{
            //    if (QueryTable == T.TableName)
            //    {
            //        if (Dataset.Tables[QueryTable].Rows.Count > 0)
            //        {
            //            foreach (System.Data.DataColumn C in T.Columns)
            //            {
            //                if (DisplayColumn == C.ColumnName)
            //                {
            //                    if (!Dataset.Tables[QueryTable].Rows[0][DisplayColumn].Equals(System.DBNull.Value)
            //                        && Dataset.Tables[QueryTable].Rows[0][DisplayColumn].ToString().Length > 0)
            //                        this.DisplayTextSelectedItem = Dataset.Tables[QueryTable].Rows[0][DisplayColumn].ToString();
            //                    break;
            //                }
            //            }
            //        }
            //        break;
            //    }
            //}
        }

        public string QueryString()
        {
            string Query = "";
            try
            {
                if (this.sqlDataAdapter != null
                    && this.sqlDataAdapter.SelectCommand != null
                    && this.sqlDataAdapter.SelectCommand.CommandText != null)
                    Query = this.sqlDataAdapter.SelectCommand.CommandText;
            }
            catch { }
            return Query;
        }

        public bool IDisNumeric
        {
            get { return _IDisNumeric; }
            set
            {
                _IDisNumeric = value;
                if (!_IDisNumeric
                    && this.dtQuery.Columns[0].DataType.Name == "Int32")
                {
                    this.dtQuery.Columns[0].DataType = typeof(string);
                }
            }
        }

        public void setModuleAndForm(string Module, string Form)
        {
            this._UsedByModule = Module;
            this._UsedByForm = Form;
        }

        public void setEntity()
        {
            if (!DiversityWorkbench.Entity.EntityTablesExist) return;

            foreach (System.Windows.Forms.Control C in this.panelQueryConditions.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.GroupBox))
                {
                    if (C.AccessibleName != null && C.AccessibleName.Length > 0)
                    {
                        System.Collections.Generic.Dictionary<string, string> DictEntity = DiversityWorkbench.Entity.EntityInformation(C.AccessibleName);
                        if (DictEntity.ContainsKey("DisplayText") && DictEntity["DisplayTextOK"] == "True")
                        {
                            C.Text = DictEntity["DisplayText"];
                            //this.setQueryConditionsDisplayTexts(C);
                        }
                    }
                    foreach (System.Windows.Forms.Control CC in C.Controls)
                    {
                        if (CC.GetType() == typeof(DiversityWorkbench.IUserControlQueryCondition) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionSet) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionText) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionXML) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionGIS) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionHierarchy) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionModule) ||
                            CC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable))
                        {
                            try
                            {
                                DiversityWorkbench.IUserControlQueryCondition UCQC = (DiversityWorkbench.IUserControlQueryCondition)CC;
                                UCQC.setEntity();
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                }
            }
        }

        public void RefreshHierarchy()
        {
            foreach (DiversityWorkbench.QueryCondition Q in this.ProjectDependentQueryConditions)
            {
                Q.RequeryHierarchySource();
            }
        }

        public void setModeOfControl(Mode Mode)
        {
            switch (Mode)
            {
                case UserControlQueryList.Mode.Default:
                    break;
                case UserControlQueryList.Mode.Simple:
                    this.toolStripQueryList.Visible = false;
                    this.buttonFreeText.Visible = false;
                    this.buttonOptimize.Visible = false;
                    this.buttonQueryAdd.Visible = false;
                    this.buttonQueryClear.Visible = false;
                    this.buttonQueryKeep.Visible = false;
                    this.buttonQueryLoad.Visible = false;
                    this.buttonQueryRemember.Visible = false;
                    this.buttonQueryRemove.Visible = false;
                    this.buttonQuerySave.Visible = false;
                    this.buttonSelectAllItems.Visible = false;
                    this.buttonShowQueryConditions.Visible = false;
                    break;
                case Mode.Embedded:
                    this.ModeOfControl = Mode;

                    this.OptimizingAllow(true);
                    this.Optimizing_SetUsage(true);
                    this.buttonOptimize.Visible = true;
                    this.buttonOptimize.Enabled = false;

                    this.buttonQuerySave.Visible = true;
                    this.toolTipQueryList.SetToolTip(this.buttonShowQueryConditions, "Save current query and use it as filter");

                    //this.buttonShowQueryConditions.BackColor = System.Drawing.Color.Red;
                    this.buttonShowQueryConditions.Image = DiversityWorkbench.Properties.Resources.Delete;
                    this.toolTipQueryList.SetToolTip(this.buttonShowQueryConditions, "Remove current restriction");

                    this.toolStripQueryList.Visible = false;
                    this.buttonFreeText.Visible = false;
                    this.buttonQueryAdd.Visible = false;
                    this.buttonQueryClear.Visible = false;
                    this.buttonQueryKeep.Visible = false;
                    this.buttonQueryLoad.Visible = false;
                    this.buttonQueryRemember.Visible = false;
                    this.buttonQueryRemove.Visible = false;
                    this.buttonSelectAllItems.Visible = false;
                    this.buttonShowQueryConditions.Visible = false;
                    break;
            }
        }

        #endregion

        #region public Properties

        public enum Mode { Default, Simple, Embedded }
        public Mode ModeOfControl = Mode.Default;

        public bool IsPredefinedQuery
        {
            get { return _IsPredefinedQuery; }
            set
            {
                _IsPredefinedQuery = value;
                this.buttonShowQueryConditions.Visible = value;
                if (value)
                {
                    this._GetNext = false;
                    this._GetPrevious = false;
                    this.buttonQueryNext.Visible = false;
                    this.buttonQueryPrevious.Visible = false;
                    this._RestrictionStackForNavigation.Clear();
                }
                this.RememberQueryIsEnabled(!_IsPredefinedQuery);
                //if (this._AllowOptimizing)
                this.buttonOptimize.Enabled = !_IsPredefinedQuery;
                //else
                //    this.buttonOptimize.Enabled = false;
                this.buttonQueryAdd.Enabled = !_IsPredefinedQuery;
                this.buttonQueryClear.Enabled = !_IsPredefinedQuery;
                this.buttonQuerySave.Enabled = !_IsPredefinedQuery;
            }
        }

        public string QueryConditionVisiblity
        {
            get { return _QueryConditionVisiblity; }
            set { _QueryConditionVisiblity = value; }
        }

        public string QueryTableName
        {
            get
            {
                string QueryTable = "";
                foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                {
                    if (C.DisplayColumn == this.SelectedDisplayColumn)
                    {
                        QueryTable = C.TableName;
                        break;
                    }
                }
                return QueryTable;
            }
        }

        public System.Data.DataTable QueryTable
        {
            get
            {
                return this.dtQuery;
            }
        }

        public void setQueryRestriction(string SqlRestriction, string PrefixReplacement)
        {
            this._PrefixReplacement = PrefixReplacement;
            this._SqlRestriction = SqlRestriction;
        }

        private string _PrefixReplacement;

        /// <summary>
        /// give the column and the restriction, e.g.: TerminologyID IN (3, 40)
        /// </summary>
        public string QueryRestriction
        {
            get
            {
                string SQL = "";
                if (this._SqlRestriction.Length > 0)
                {
                    string Prefix = "";
                    if (this.LinkedServer.Length > 0)
                        Prefix = "[" + this.LinkedServer + "]." + this.LinkedServerDatabase + ".dbo.";
                    else Prefix = "dbo.";
                    SQL = " AND ";
                    // Markus 15.3.2023: Bugfix ManyOrderByColumns using deviating table alias
                    // Markus 30.5.23: nur wenn Optimizing
                    if ((this._ManyOrderByColumns_Allow || this.ManyOrderByColumns()) && this.OptimizingIsUsed())
                    {
                        string Column = this._SqlRestriction.Substring(0, this._SqlRestriction.IndexOf(" ")).Trim();
                        bool OK = false;
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.ManyOrderByColumns_TableAliases())
                        {
                            string sql = "select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = '" + KV.Key + "' and c.COLUMN_NAME = '" + Column + "'";
                            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql, true);
                            if (Result.Length > 0 && Result != "0")
                            {
                                if (!SQL.EndsWith(" AND "))
                                    SQL += " AND ";
                                SQL += KV.Value + "." + this._SqlRestriction.Replace(this._PrefixReplacement, Prefix);
                                OK = true;
                                //break;
                            }
                        }
                        if (!OK)
                        {
                            SQL += "T." + this._SqlRestriction.Replace(this._PrefixReplacement, Prefix);
                            //SQL += "T";
                        }
                    }
                    else
                        SQL += "T." + this._SqlRestriction.Replace(this._PrefixReplacement, Prefix);
                    //SQL += "." + this._SqlRestriction.Replace(this._PrefixReplacement, Prefix);
                }
                return SQL;
            }
            set
            {

                this._SqlRestriction = value;
            }
        }

        //public System.Collections.Generic.List<string> QueryRestrictions
        //{
        //    set
        //    {

        //    }
        //}

        #region ProjectID

        public int ProjectID
        {
            get
            {
                int? ProjectID = null;
                foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                {
                    if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        foreach (System.Windows.Forms.Control U in G.Controls)
                        {
                            try
                            {
                                if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryCondition C = (DiversityWorkbench.UserControls.UserControlQueryCondition)U;
                                    if (C.Condition().Column == "ProjectID" && C.ConditionValue().Length > 0)
                                    {
                                        ProjectID = System.Int32.Parse(C.ConditionValue());
                                        break;
                                    }
                                    else if (C.Condition().Column == "TerminologyID" && C.ConditionValue().Length > 0)
                                    {
                                        ProjectID = System.Int32.Parse(C.ConditionValue());
                                        break;
                                    }
                                }
                                else if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionSet))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryConditionSet C = (DiversityWorkbench.UserControls.UserControlQueryConditionSet)U;
                                    if (C.Condition().Column == "ProjectID" && C.ConditionValue().Length > 0)
                                    {
                                        ProjectID = System.Int32.Parse(C.ConditionValue());
                                        break;
                                    }
                                    else if (C.Condition().Column == "TerminologyID" && C.ConditionValue().Length > 0)
                                    {
                                        ProjectID = System.Int32.Parse(C.ConditionValue());
                                        break;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
                if (ProjectID == null)
                {
                    if (this._ServerConnection != null
                        && this._ServerConnection.CurrentProjectID != null)
                        ProjectID = (int)this._ServerConnection.CurrentProjectID;
                    else
                    {
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("SELECT dbo.DefaultProjectID()", con);
                        try
                        {
                            con.Open();
                            string getP = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            if (getP == string.Empty)
                                ProjectID = -1;
                            else
                                ProjectID = int.Parse(getP);
                            con.Close();
                        }
                        catch
                        {
                            ProjectID = -1;
                        }
                    }
                }
                return (int)ProjectID;
            }
            set
            {
                if (this._ServerConnection != null)
                {
                    this._ServerConnection.CurrentProjectID = value;
                }
                foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                {
                    if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        foreach (System.Windows.Forms.Control U in G.Controls)
                        {
                            try
                            {
                                if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                                {
                                    //DiversityWorkbench.IUserControlQueryCondition iC = (DiversityWorkbench.IUserControlQueryCondition)U;
                                    DiversityWorkbench.UserControls.UserControlQueryCondition C = (DiversityWorkbench.UserControls.UserControlQueryCondition)U;
                                    if (C.Condition().Column == "ProjectID" && C.ConditionValue().Length > 0)
                                    {
                                        QueryCondition QC = C.QueryCondition;
                                        if (QC.SelectFromList
                                            && QC.dtValues != null
                                            && QC.dtValues.Rows.Count > 0
                                            && QC.dtValues.Columns.Contains("Value"))
                                        {
                                            for (int i = 0; i < QC.dtValues.Rows.Count; i++)
                                            {
                                                if (QC.dtValues.Rows[i]["Value"].ToString() == value.ToString())
                                                {
                                                    QC.SelectedIndex = i;
                                                    break;
                                                }
                                            }
                                        }
                                        QC.Value = value.ToString();
                                        C.setConditionValues(QC);
                                        //iC.setConditionValues(QC);
                                        break;
                                    }
                                    else if (C.Condition().Column == "TerminologyID" && C.ConditionValue().Length > 0)
                                    {
                                        //ProjectID = System.Int32.Parse(C.ConditionValue());
                                        break;
                                    }
                                    else if (C.Condition().Column == "project_id")
                                    {
                                        QueryCondition QC = C.QueryCondition;
                                        if ((QC.SelectFromList || QC.SelectFromHierachy)
                                            && QC.dtValues != null
                                            && QC.dtValues.Rows.Count > 0
                                            && QC.dtValues.Columns.Contains("Value"))
                                        {
                                            for (int i = 0; i < QC.dtValues.Rows.Count; i++)
                                            {
                                                if (QC.dtValues.Rows[i]["Value"].ToString() == value.ToString())
                                                {
                                                    QC.SelectedIndex = i;
                                                    break;
                                                }
                                            }
                                        }
                                        QC.Value = value.ToString();
                                        QC.QueryConditionOperator = "=";
                                        C.setConditionValues(QC);
                                        //iC.setConditionValues(QC);
                                        break;
                                    }
                                }
                                else if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionSet))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryConditionSet C = (DiversityWorkbench.UserControls.UserControlQueryConditionSet)U;
                                    if (C.Condition().Column == "ProjectID" && C.ConditionValue().Length > 0)
                                    {
                                        //ProjectID = System.Int32.Parse(C.ConditionValue());
                                        break;
                                    }
                                    else if (C.Condition().Column == "TerminologyID" && C.ConditionValue().Length > 0)
                                    {
                                        //ProjectID = System.Int32.Parse(C.ConditionValue());
                                        break;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        public bool ProjectIsSelected
        {
            get
            {
                bool IsSelected = false;
                foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                {
                    if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        foreach (System.Windows.Forms.Control U in G.Controls)
                        {
                            try
                            {
                                if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryCondition C = (DiversityWorkbench.UserControls.UserControlQueryCondition)U;
                                    if (C.Condition().Column == "ProjectID" && C.ConditionValue().Length > 0)
                                    {
                                        IsSelected = true;
                                        break;
                                    }
                                }
                                else if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionSet))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryConditionSet C = (DiversityWorkbench.UserControls.UserControlQueryConditionSet)U;
                                    if (C.Condition().Column == "ProjectID" && C.ConditionValue().Length > 0)
                                    {
                                        IsSelected = true;
                                        break;
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
                return IsSelected;
            }
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> ProjectDependentQueryConditions
        {
            get
            {
                if (this._ProjectDependentQueryConditions == null)
                    this._ProjectDependentQueryConditions = new List<QueryCondition>();
                return _ProjectDependentQueryConditions;
            }
            // set { _ProjectDependentQueryConditions = value; }
        }

        public int? _SelectedProjectID = null;
        public int? SelectedProjectID
        {
            get
            {
                return this._SelectedProjectID;
            }
            set
            {
                this._SelectedProjectID = value;
            }
        }

        #endregion

        public int ID
        {
            get
            {
                int i = -1;
                if (this.listBoxQueryResult.SelectedIndex > -1)
                {
                    try
                    {
                        if (UseOptimizing)
                        {
                            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxQueryResult.SelectedItem;
                            i = System.Int32.Parse(R[0].ToString());
                        }
                        else
                            i = System.Int32.Parse(this.listBoxQueryResult.SelectedValue.ToString());
                    }
                    catch { }
                }
                return i;
            }
        }

        public string PK
        {
            get
            {
                string _PK = "";
                if (this.listBoxQueryResult.SelectedIndex > -1)
                {
                    try
                    {
                        _PK = this.listBoxQueryResult.SelectedValue.ToString();
                    }
                    catch { }
                }
                return _PK;
            }
        }

        public string WhereClause
        {
            set
            {
                this.SqlWhereClause = value;
            }
            get
            {
                return this.SqlWhereClause;
            }
        }

        public int MaximalNumberOfResults
        {
            set
            {
                try
                {
                    this.MaxNumOfResult = DiversityWorkbench.Settings.QueryMaxResults;
                    this.toolTipQueryList.SetToolTip(this.buttonQueryNext, DiversityWorkbench.UserControls.UserControlQueryListText.Get_next + " " + this.MaxNumOfResult.ToString() + " " + DiversityWorkbench.UserControls.UserControlQueryListText.items);
                }
                catch { }
            }
            get
            {
                if (this.MaxNumOfResult == 0)
                    this.MaxNumOfResult = DiversityWorkbench.Settings.QueryMaxResults;
                return this.MaxNumOfResult;
            }
        }

        public Microsoft.Data.SqlClient.SqlConnection Connection
        {
            get { return this.sqlConnection; }
            set
            {
                if (value != null)
                {
                    this.sqlConnection = value;
                    this.sqlDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter("", this.sqlConnection.ConnectionString);
                }
            }
        }

        /// Markus 27.2.2017 - offenbar nicht benutzt
        //public DiversityWorkbench.QueryCondition[] Conditions()
        //{
        //    int i = this._QueryConditions.Count;
        //    int c = 0;
        //    DiversityWorkbench.QueryCondition[] CC = new DiversityWorkbench.QueryCondition[i];
        //    foreach (System.Windows.Forms.Control g in this.groupBoxQueryConditions.Controls)
        //    {
        //        foreach (System.Windows.Forms.UserControl u in g.Controls)
        //        {
        //            if (typeof(DiversityWorkbench.UserControls.UserControlQueryCondition) == u.GetType() ||
        //                typeof(DiversityWorkbench.UserControls.UserControlQueryConditionGIS) == u.GetType() ||
        //                typeof(DiversityWorkbench.UserControls.UserControlQueryConditionHierarchy) == u.GetType())
        //            {
        //                DiversityWorkbench.IUserControlQueryCondition I = (DiversityWorkbench.IUserControlQueryCondition)u;
        //                CC[c] = I.Condition();
        //                c++;
        //            }
        //        }
        //    }
        //    return CC;
        //}


        public string OrderByColumn
        {
            get
            {
                string OrderColumn = "";
                foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                {
                    if (C.DisplayColumn == this.SelectedDisplayColumn)
                    {
                        OrderColumn = C.OrderColumn;
                        break;
                    }
                }
                return OrderColumn;
            }
        }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            set
            {
                try
                {
                    if (value != null)
                    {
                        this.DisplayColumns = value;
                        this._DtDisplayColumns = new DataTable();
                        System.Data.DataColumn C1 = new DataColumn("DisplayText", typeof(string));
                        System.Data.DataColumn C2 = new DataColumn("DisplayColumn", typeof(string));
                        System.Data.DataColumn C3 = new DataColumn("SourceTable", typeof(string));
                        this._DtDisplayColumns.Columns.Add(C1);
                        this._DtDisplayColumns.Columns.Add(C2);
                        this._DtDisplayColumns.Columns.Add(C3);
                        foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in value)
                        {
                            System.Data.DataRow R = this._DtDisplayColumns.NewRow();
                            R[0] = C.DisplayText;
                            R[1] = C.DisplayColumn;
                            R[2] = C.TableName;
                            this._DtDisplayColumns.Rows.Add(R);
                        }
                        this.comboBoxQueryColumn.DataSource = this._DtDisplayColumns;
                        this.comboBoxQueryColumn.DisplayMember = "DisplayText";
                        this.comboBoxQueryColumn.ValueMember = "DisplayColumn";

                        this.comboBoxQueryColumn.SelectedIndex = 0;
                    }

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            get { return this.DisplayColumns; }
        }

        public void SetQueryDisplayColumns(DiversityWorkbench.UserControls.QueryDisplayColumn[] Columns, string PrefixColumn, string PrefixTable)
        {
            try
            {
                if (Columns != null)
                {
                    this.DisplayColumns = Columns;
                    this._DtDisplayColumns = new DataTable();
                    System.Data.DataColumn C1 = new DataColumn("DisplayText", typeof(string));
                    System.Data.DataColumn C2 = new DataColumn("DisplayColumn", typeof(string));
                    System.Data.DataColumn C3 = new DataColumn("SourceTable", typeof(string));
                    this._DtDisplayColumns.Columns.Add(C1);
                    this._DtDisplayColumns.Columns.Add(C2);
                    this._DtDisplayColumns.Columns.Add(C3);
                    foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in Columns)
                    {
                        System.Data.DataRow R = this._DtDisplayColumns.NewRow();
                        R[0] = C.DisplayText;
                        R[1] = C.DisplayColumn;
                        R[2] = C.TableName;
                        this._DtDisplayColumns.Rows.Add(R);
                        if (C.DisplayColumn == PrefixColumn && C.TableName == PrefixTable)
                            this.InsertOrderByPrefixes(PrefixColumn, PrefixTable);
                    }
                    comboBoxQueryColumn.SelectedIndexChanged -= comboBoxQueryColumn_SelectedIndexChanged; // avoid -> Ariane 1.Aufruf QueryDatabase Ariane
                    this.comboBoxQueryColumn.DataSource = this._DtDisplayColumns; 
                    this.comboBoxQueryColumn.DisplayMember = "DisplayText";
                    this.comboBoxQueryColumn.ValueMember = "DisplayColumn";
                    this.comboBoxQueryColumn.ContextMenuStrip = this.contextMenuStripOrderBy;

                    this.comboBoxQueryColumn.SelectedIndex = 0;
                    comboBoxQueryColumn.SelectedIndexChanged += comboBoxQueryColumn_SelectedIndexChanged;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void InsertOrderByPrefixes(string PrefixColumn, string PrefixTable)
        {
            try
            {
                this._OrderByPrefixColumn = PrefixColumn;
                this._OrderByPrefixTable = PrefixTable;
                if (DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix != null)
                {
                    foreach (string P in DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix)
                    {
                        System.Data.DataRow R = this._DtDisplayColumns.NewRow();
                        R[0] = P + " (" + PrefixColumn + ")";
                        R[1] = PrefixColumn;
                        R[2] = PrefixTable;
                        this._DtDisplayColumns.Rows.Add(R);
                        if (this._OrderByPrefixes == null)
                            this._OrderByPrefixes = new Dictionary<string, string>();
                        if (!this._OrderByPrefixes.ContainsKey(P + " (" + PrefixColumn + ")"))
                            this._OrderByPrefixes.Add(P + " (" + PrefixColumn + ")", P);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        public DiversityWorkbench.UserControls.QueryDisplayColumn QueryDisplayColumn
        {
            get
            {
                if (this.comboBoxQueryColumn.SelectedIndex > -1)
                    return this.DisplayColumns[this.comboBoxQueryColumn.SelectedIndex];
                else if (this.QueryDisplayColumns.Length > 0)
                    return this.DisplayColumns[0];
                else
                {
                    DiversityWorkbench.UserControls.QueryDisplayColumn C = new QueryDisplayColumn();
                    return C;
                }
            }
        }

        public void AddListItem(int ID, string Display)
        {
            System.Data.DataRow dr = this.dtQuery.NewRow();
            dr["ID"] = ID;
            if (Display.Length == 0)
                Display = "ID: " + ID.ToString();
            dr["Display"] = Display;
            this.dtQuery.Rows.Add(dr);
            this.listBoxQueryResult.SelectionMode = SelectionMode.One;
            this.listBoxQueryResult.SelectedIndex = this.listBoxQueryResult.Items.Count - 1;
            this.listBoxQueryResult.SelectionMode = SelectionMode.MultiExtended;
            this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
        }

        public void SetItemList(System.Collections.Generic.Dictionary<int, string> ItemDictionary)
        {
            this.listBoxQueryResult.SuspendLayout();
            this.dtQuery.Clear();
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in ItemDictionary)
            {
                System.Data.DataRow dr = this.dtQuery.NewRow();
                dr["ID"] = KV.Key;
                string Display = KV.Value;
                if (Display.Length == 0)
                    Display = "ID: " + ID.ToString();
                dr["Display"] = Display;
                this.dtQuery.Rows.Add(dr);
            }
            this.listBoxQueryResult.SelectionMode = SelectionMode.One;
            this.listBoxQueryResult.SelectedIndex = 0;
            this.listBoxQueryResult.SelectionMode = SelectionMode.MultiExtended;
            this.groupBoxQueryResults.Text = "1 - " + ItemDictionary.Count.ToString();
            this.listBoxQueryResult.ResumeLayout();
            this.ResetListOfIDs();
        }

        public void AddListItem(string ID, string Display)
        {
            System.Data.DataRow dr = this.dtQuery.NewRow();
            dr["ID"] = ID;
            dr["Display"] = Display;
            this.dtQuery.Rows.Add(dr);
            this.listBoxQueryResult.SelectionMode = SelectionMode.One;
            this.listBoxQueryResult.SelectedIndex = this.listBoxQueryResult.Items.Count - 1;
            this.listBoxQueryResult.SelectionMode = SelectionMode.MultiExtended;
            this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
        }

        public void RemoveListItem(int ID)
        {
            try
            {
                // Temporarily disable the SelectedIndexChanged event in the main form to avoid exceptions leading to loss of permissions
                SuppressSelectedIndexChangedEvent?.Invoke(this, true);

                System.Data.DataRow[] rr = this.dtQuery.Select("ID = " + ID.ToString());
                foreach (System.Data.DataRow R in rr)
                {
                    R.Delete();
                }
                // Toni 20211029 Accept changes to remove invalid rows
                this.dtQuery.AcceptChanges();
                this.listBoxQueryResult.SelectedIndex = -1;
                this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                // re-enable
                SuppressSelectedIndexChangedEvent?.Invoke(this, false);
            }
        }

        public void RemoveListItem(string ID)
        {
            System.Data.DataRow[] rr = this.dtQuery.Select("ID = " + ID.ToString());
            foreach (System.Data.DataRow R in rr)
            {
                R.Delete();
            }
            this.listBoxQueryResult.SelectedIndex = -1;
            this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
        }

        private System.Collections.Generic.List<int> _ListOfIDs;
        public void ResetListOfIDs() { this._ListOfIDs = null; }
        public System.Collections.Generic.List<int> ListOfIDs
        {
            get
            {
                // Markus, 17.2.2016: Keep list of IDs - rebuild takes too long for large queries
                if (this._ListOfIDs == null || this._ListOfIDs.Count == 0)
                {
                    this._ListOfIDs = new List<int>();
                    foreach (System.Data.DataRow R in this.dtQuery.Rows)
                    {
                        if (R.RowState != DataRowState.Deleted)
                        {
                            if (!R["ID"].Equals(System.DBNull.Value) && this._ListOfIDs.IndexOf(int.Parse(R["ID"].ToString())) == -1)
                            {
                                int i;
                                if (int.TryParse(R["ID"].ToString(), out i))
                                {
                                    if (!this.ListOfBlockedIDs.Contains(i))
                                        this._ListOfIDs.Add(i);
                                }
                            }
                        }
                    }
                }
                return this._ListOfIDs;
            }
        }

        public string SqlForQuery()
        {
            if (this.sqlDataAdapter != null && this.sqlDataAdapter.SelectCommand != null)
                return this.sqlDataAdapter.SelectCommand.CommandText;
            else
                return "";
        }

        #region Blocked IDs, e.g. ReadOnly

        private System.Collections.Generic.List<int> _ListOfBlockedIDs;
        public System.Collections.Generic.List<int> ListOfBlockedIDs
        {
            get
            {
                if (this._ListOfBlockedIDs == null)
                {
                    this._ListOfBlockedIDs = new List<int>();
                    if (this._SqlForBlockedIDs != null && this._SqlForBlockedIDs.Length > 0)
                    {
                        System.Data.DataTable dt = new DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(this._SqlForBlockedIDs, DiversityWorkbench.Settings.ConnectionString);

                        try
                        {
                            ad.Fill(dt);
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                int i;
                                if (int.TryParse(R[0].ToString(), out i))
                                    this._ListOfBlockedIDs.Add(i);
                            }

                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
                return this._ListOfBlockedIDs;
            }
        }
        public void ResetListOfBlockedIDs()
        {
            this._ListOfBlockedIDs = null;
        }
        private string _SqlForBlockedIDs;
        public void SetSqlForBlockedIDs(string SQL)
        {
            this._SqlForBlockedIDs = SQL;
        }

        #endregion

        public System.Collections.Generic.List<int> ListOfSelectedIDs
        {
            get
            {
                System.Collections.Generic.List<int> L = new List<int>();
                foreach (System.Object O in this.listBoxQueryResult.SelectedItems)
                {
                    System.Data.DataRowView Rv = (System.Data.DataRowView)O;
                    if (Rv.Row.RowState != DataRowState.Deleted)
                    {
                        if (L.IndexOf(int.Parse(Rv.Row["ID"].ToString())) == -1)
                            L.Add(int.Parse(Rv.Row["ID"].ToString()));
                    }
                }
                return L;
            }
        }

        public System.Collections.Generic.List<string> ListOfPKs
        {
            get
            {
                System.Collections.Generic.List<string> L = new List<string>();
                foreach (System.Data.DataRow R in this.dtQuery.Rows)
                {
                    if (R.RowState != DataRowState.Deleted)
                    {
                        if (L.IndexOf(R["ID"].ToString()) == -1)
                            L.Add(R["ID"].ToString());
                    }
                }
                return L;
            }
        }
        
        public void RemoveSelectedListItem()
        {
            try
            {
                // Temporarily disable the SelectedIndexChanged event in the main form to avoid exceptions leading to loss of permissions
                SuppressSelectedIndexChangedEvent?.Invoke(this, true);

                System.Data.DataRowView r = (System.Data.DataRowView)this.listBoxQueryResult.SelectedItem;
                r.BeginEdit();
                r.Delete();
                r.EndEdit();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                // re-enable
                SuppressSelectedIndexChangedEvent?.Invoke(this, false);
            }
        }
        
        public string DisplayTextSelectedItem
        {
            get
            {
                string DisplayText = "";
                if (this.dtQuery != null)
                {
                    if (this.dtQuery.Rows.Count > 0 && this.listBoxQueryResult.SelectedIndex > -1)
                    {
                        try
                        {
                            DisplayText = this.dtQuery.Rows[this.listBoxQueryResult.SelectedIndex]["Display"].ToString();
                            System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxQueryResult.SelectedItem;
                            DisplayText = rv["Display"].ToString();
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
                return DisplayText;
            }
            set
            {
                if (this.listBoxQueryResult.SelectedIndex > -1)
                {
                    try
                    {
                        System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxQueryResult.SelectedItem;
                        rv["Display"] = value;
                        //this.listBoxQueryResult.Refresh();
                        //this.listBoxQueryResult.Update();
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }

        public void setQueryConditionVisibility()
        {
        }

        public void clearQueryList()
        {
            this.dtQuery.Clear();
            this.groupBoxQueryResults.Text = DiversityWorkbench.UserControls.UserControlQueryListText.Query_results;
        }

        public System.Windows.Forms.Control FirstQueryControl
        {
            get
            {
                return this._FirstQueryControl;
            }
        }

        public System.Windows.Forms.ImageList ImageList
        {
            get { return _ImageList; }
            set { _ImageList = value; }
        }

        public System.Collections.Generic.Dictionary<string, System.Drawing.Color> TableColors
        {
            get { return _TableColors; }
            set { _TableColors = value; }
        }

        public System.Collections.Generic.Dictionary<string, int> TableImageIndex
        {
            get { return _TableImageIndex; }
            set { _TableImageIndex = value; }
        }

        #endregion

        #region Query

        #region Button events

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this._GetNext = false;
                this._GetPrevious = false;
                this.buttonQueryPrevious.Visible = false;
                this._RestrictionStackForNavigation.Clear();
                dtQuery.Clear();
                this.QueryDatabase(false);
                this.listBoxQueryResult.Focus();
                this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void showSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = this.QueryString(this.MaximalNumberOfResults);
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("SQL", SQL, true);
            f.ShowDialog();
        }

        private void buttonQueryAdd_Click(object sender, EventArgs e)
        {
            // Toni: Restrict application data refresh
            _RefreshData = false;
            int i = this.listBoxQueryResult.Items.Count;
            bool IsSelectionModeOne = false;
            if (this.listBoxQueryResult.SelectionMode == SelectionMode.One)
                IsSelectionModeOne = true;
            this._GetNext = false;
            this._GetPrevious = false;
            this.buttonQueryPrevious.Visible = false;
            this._RestrictionStackForNavigation.Clear();
            this.QueryDatabase(true);
            // Toni: Re-activate application data refresh
            _RefreshData = true;
            if (i < this.listBoxQueryResult.Items.Count)
            {
                this.listBoxQueryResult.SelectionMode = SelectionMode.One;
                //this.listBoxQueryResult.SelectedIndex = i;
                this.listBoxQueryResult.SetSelected(i, true);
                if (!IsSelectionModeOne && this.listBoxQueryResult.SelectionMode != SelectionMode.MultiExtended)
                    this.listBoxQueryResult.SelectionMode = SelectionMode.MultiExtended;
            }

            this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
        }

        private void buttonQueryNext_Click(object sender, EventArgs e)
        {
            this._GetNext = true;
            this._GetPrevious = false;
            this._GetNextRestriction = this.RestrictionForNextDatasets;
            dtQuery.Clear();
            this.QueryDatabase(false);
            this.buttonQueryPrevious.Visible = true;
            if (this._RestrictionStackForNavigation.Count == 0) this._RestrictionStackForNavigation.Push("");
            this._RestrictionStackForNavigation.Push(this._GetNextRestriction);
            this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
        }

        private void buttonQueryPrevious_Click(object sender, EventArgs e)
        {
            if (this._RestrictionStackForNavigation.Count > 0)
            {
                this._GetNext = false;
                this._GetPrevious = true;
                if (this._GetNextRestriction == this._RestrictionStackForNavigation.Peek())
                    this._RestrictionStackForNavigation.Pop();
                this._GetNextRestriction = this._RestrictionStackForNavigation.Pop();
                dtQuery.Clear();
                this.QueryDatabase(false);
            }
            if (this._RestrictionStackForNavigation.Count == 0)
            {
                this.buttonQueryPrevious.Visible = false;
                this._GetNextStart = 1;
            }
            this.ResetListOfIDs();// Markus, 17.2.2016: Refresh list of IDs
        }

        private void buttonQueryClear_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
            {
                if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                {
                    foreach (System.Windows.Forms.Control U in G.Controls)
                    {
                        // Markus 11.6.24: Bilder ausschliessen
                        if (U.GetType() == typeof(System.Windows.Forms.PictureBox))
                            continue;
                        try
                        {
                            //if (U.GetType() == typeof(DiversityWorkbench.IUserControlQueryCondition))
                            {
                                DiversityWorkbench.IUserControlQueryCondition C = (DiversityWorkbench.IUserControlQueryCondition)U;
                                //DiversityWorkbench.UserControls.UserControlQueryCondition C = (DiversityWorkbench.UserControls.UserControlQueryCondition)U;
                                C.Clear();
                            }
                        }
                        catch { }
                    }
                }
            }
            this._GetPrevious = false;
            this._GetNext = false;
            this._RestrictionStackForNavigation.Clear();
            this.buttonQueryPrevious.Visible = false;
            this.buttonQueryNext.Visible = false;
        }

        /// <summary>
        /// Load a query that has been saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonQueryLoad_Click(object sender, EventArgs e)
        {
            if (OptimizingIsUsed())
            {
                //System.Windows.Forms.MessageBox.Show("Beta version for optimized queries");
            }
            string SQL = "SELECT CASE WHEN [Queries] IS NULL THEN '' ELSE [Queries] END AS Queries " +
                "FROM UserProxy " +
                "WHERE (LoginName = USER_NAME())";
            string XML = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, DiversityWorkbench.Settings.ConnectionString);
            if (XML.Length > 0)
            {
                DiversityWorkbench.Forms.FormQueryLoad f = new DiversityWorkbench.Forms.FormQueryLoad(XML);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK
                    && f.Query.Title != null
                    && f.Query.Title.Length > 0)
                {
                    this.IsPredefinedQuery = true;
                    this.setQueryConditions(f.Query.Title, f.Query.QueryTable, f.Query.SQL, f.Query.Description);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(DiversityWorkbench.UserControls.UserControlQueryListText.No_query_have_been_defined_so_far);//"No queries have been defined so far");
            }
        }

        /// <summary>
        /// Saving the current query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonQuerySave_Click(object sender, EventArgs e)
        {
            if (this.ModeOfControl == Mode.Embedded)
            {
                try
                {

                    string SQL = this.QueryStringWhereClause;
                    if (SQL.Length > 0)
                    {
                        SQL = " WHERE " + this._QueryConditions[0].IdentityColumn + " IN (SELECT T." + this._QueryConditions[0].IdentityColumn + SQL + ")";
                        string SQLupdate = this._EmbeddedSQL.Replace(this._EmbeddedPlaceholder, SQL.Replace("'", "''"));
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._EmbeddedConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQLupdate, con);
                        con.Open();
                        C.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        this.IsPredefinedQuery = true;
                        string QueryTable = "";
                        if (this.SqlQueryTable != null && this.SqlQueryTable.Length > 0)
                            QueryTable = SqlQueryTable;
                        else if (this.QueryTableName != null && this.QueryTableName.Length > 0)
                            QueryTable = this.QueryTableName;
                        else
                        {

                        }
                        this.setQueryConditions("", QueryTable, SQL, "");
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                if (OptimizingIsUsed())
                {
                    if (this.ModeOfControl != Mode.Embedded)
                        System.Windows.Forms.MessageBox.Show("Beta version for optimized queries");
                    //return;
                }
                string SQL = this.QueryStringWhereClause;
                if (SQL.Length == 0)
                {
                    if (System.Windows.Forms.MessageBox.Show(DiversityWorkbench.UserControls.UserControlQueryListText.No_query_have_been_defined_so_far + ". " + DiversityWorkbench.UserControls.UserControlQueryListText.Do_you_want_to_edit_the_saved_queries + "?", DiversityWorkbench.UserControls.UserControlQueryListText.No_query, MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                else if (OptimizingIsUsed())
                {
                    SQL = " WHERE " + this._QueryConditions[0].IdentityColumn + " IN (SELECT T." + this._QueryConditions[0].IdentityColumn + SQL + ")";
                }
                else
                {
                    if (SQL.IndexOf(" WHERE ") > -1)
                        SQL = SQL.Substring(SQL.IndexOf(" WHERE "));
                    if (SQL.IndexOf(" WHERE 1 = 1 AND ") > -1)
                        SQL = SQL.Replace(" WHERE 1 = 1 AND ", " WHERE ");
                    if (SQL == " WHERE 1 = 1")
                        SQL = "";
                }
                if (SQL.Length > 0)
                {
                    DiversityWorkbench.Forms.FormQuerySave f = new DiversityWorkbench.Forms.FormQuerySave(SQL, this.QueryTableName);
                    f.ShowDialog();
                }
                else
                    System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Forms.FormQuerySaveText.No_query_was_defined);//"No query was defined");

            }
        }

        private bool _RefreshData = true;

        /// <summary>
        /// Flag to disable the refresh of the data in the application in case of mass event (i.e. e.g. select all items in the list)
        /// MW 29.4.2015: Alter Name: UpdateMap
        /// </summary>
        public bool RefreshData
        {
            get { return _RefreshData; }
            //set { _RefreshData = value; }
        }

        private void buttonSelectAllItems_Click(object sender, EventArgs e)
        {
            this.SelectAllItems();
        }

        public void SelectAllItems()
        {
            try
            {
                // Disable the refresh of the data in the application before last Item has been selected! Each selection causes an event!
                _RefreshData = false;
                for (int i = 0; i < this.listBoxQueryResult.Items.Count; i++)
                {
                    // Check for last index
                    if (i == this.listBoxQueryResult.Items.Count - 1)
                    {
                        // Enable the refresh of the data in the application again for last index set!
                        _RefreshData = true;
                    }
                    this.listBoxQueryResult.SelectedItems.Add(this.listBoxQueryResult.Items[i]);
                }

            }
            catch { }
            // Enable the refresh of the data in the application again
            _RefreshData = true;
        }

        private void buttonQueryKeep_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.List<int> ii = new List<int>();
                if (this.listBoxQueryResult.SelectedIndices.Count > 0 && this.dtQuery.Rows.Count > 0)
                {
                    foreach (System.Object o in this.listBoxQueryResult.SelectedIndices)
                    {
                        int i = System.Int32.Parse(o.ToString());
                        ii.Add(i);
                    }
                    // Disable the refresh of the data in the application before last Item has been selected! Each selection causes an event!
                    _RefreshData = false;
                    int count = this.listBoxQueryResult.Items.Count - ii.Count;
                    System.Collections.Generic.List<System.Data.DataRowView> RowsToRemove = new List<DataRowView>();
                    for (int i = this.listBoxQueryResult.Items.Count - 1; i >= 0; i--)
                    {
                        if (!ii.Contains(i))
                        {
                            // Enable the refresh of the data in the application again for last index set!
                            if (count-- == 1)
                                _RefreshData = true;

                            System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxQueryResult.Items[i];
                            // Removing this item from the item list as well
                            try
                            {
                                int id = int.Parse(rv[0].ToString());
                                if (this.ListOfIDs.Contains(id))
                                    this.ListOfIDs.Remove(id);
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                            RowsToRemove.Add(rv);
                            //rv.Delete();
                        }
                    }
                    if (RowsToRemove.Count > 0)
                    {
                        _RefreshData = false;
                        for (int i = 0; i < RowsToRemove.Count; i++)
                        {
                            RowsToRemove[i].Delete();
                            if (i == RowsToRemove.Count - 1)
                                _RefreshData = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            // Enable the refresh of the data in the application again
            _RefreshData = true;
        }

        private void buttonQueryRemove_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Stack<int> ii = new Stack<int>();
                if (this.listBoxQueryResult.SelectedIndices.Count > 0 && this.dtQuery.Rows.Count > 0)
                {
                    foreach (System.Object o in this.listBoxQueryResult.SelectedIndices)
                    {
                        int i = System.Int32.Parse(o.ToString());
                        ii.Push(i);
                    }
                    // Disable the refresh of the data in the application before last Item has been selected! Each selection causes an event!
                    _RefreshData = false;
                    int count = ii.Count;
                    foreach (int i in ii)
                    {
                        // Enable the refresh of the data in the application again for last index set!
                        if (count-- == 1)
                            _RefreshData = true;
                        System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxQueryResult.Items[i];
                        // Removing this item from the item list as well
                        try
                        {
                            int id = int.Parse(rv[0].ToString());
                            if (this.ListOfIDs.Contains(id))
                                this.ListOfIDs.Remove(id);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                        rv.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            // Enable the refresh of the data in the application again
            _RefreshData = true;
        }

        private void buttonSwitchDescAsc_Click(object sender, EventArgs e)
        {
            if (this._Sorting == "ASC")// this.buttonSwitchDescAsc.Tag == null || this.buttonSwitchDescAsc.Tag.ToString() == "ASC")
            {
                this.setSorting("DESC");
                //this._Sorting = "DESC";// this.buttonSwitchDescAsc.Tag = "DESC";
                //this.buttonSwitchDescAsc.Image = this.imageListAscDesc.Images[0];
                //this.toolTipQueryList.SetToolTip(this.buttonSwitchDescAsc, "Current sorting is descending. Click to change to ascending");
            }
            else
            {
                this.setSorting("ASC");
                //this._Sorting = "ASC";// this.buttonSwitchDescAsc.Tag = "ASC";
                //this.buttonSwitchDescAsc.Image = this.imageListAscDesc.Images[1];
                //this.toolTipQueryList.SetToolTip(this.buttonSwitchDescAsc, "Current sorting is ascending. Click to change to descending");
            }
        }

        private void setSorting(string Sorting)
        {
            this._Sorting = Sorting.ToUpper();
            if (this._Sorting == "ASC")// this.buttonSwitchDescAsc.Tag == null || this.buttonSwitchDescAsc.Tag.ToString() == "ASC")
            {
                this.buttonSwitchDescAsc.Image = this.imageListAscDesc.Images[3];
                this.toolTipQueryList.SetToolTip(this.buttonSwitchDescAsc, "Current sorting is ascending. Click to change to descending");
                //this._Sorting = "DESC";// this.buttonSwitchDescAsc.Tag = "DESC";
            }
            else
            {
                //this._Sorting = "ASC";// this.buttonSwitchDescAsc.Tag = "ASC";
                this.buttonSwitchDescAsc.Image = this.imageListAscDesc.Images[2];
                this.toolTipQueryList.SetToolTip(this.buttonSwitchDescAsc, "Current sorting is descending. Click to change to ascending");
            }
        }

        private string _Sorting = "ASC";

        #endregion

        private string SelectedDisplayColumn
        {
            get
            {
                string Column = "";
                try
                {
                    if (this.comboBoxQueryColumn.SelectedIndex > -1)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQueryColumn.SelectedItem;
                        Column = R[1].ToString();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return Column;
            }
        }

        private string SelectedDisplayColumnTable
        {
            get
            {
                string Table = "";
                try
                {
                    if (this.comboBoxQueryColumn.SelectedIndex > -1)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQueryColumn.SelectedItem;
                        Table = R[2].ToString();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return Table;
            }
        }

        private DiversityWorkbench.UserControls.QueryDisplayColumn SelectedQueryDisplayColumn
        {
            get
            {
                foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                {
                    if (C.DisplayColumn == this.SelectedDisplayColumn && C.TableName == this.SelectedDisplayColumnTable)
                    {
                        return C;
                    }
                }
                DiversityWorkbench.UserControls.QueryDisplayColumn queryDisplayColumn = this.DisplayColumns[0];
                return queryDisplayColumn;
            }
        }

        private void comboBoxQueryColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonQueryNext.Visible = false;
            string TipText = "";
            foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
            {
                if (C.DisplayColumn == this.SelectedDisplayColumn && C.TableName == this.SelectedDisplayColumnTable)
                {
                    TipText = C.TipText;
                    // Markus 3.11.2022: setting module to ensure correct main table for queries
                    SetQueryMainTable(C);
                    //QueryMainTable = C.TableName;
                    _IdentityColumnOptimizing = C.IdentityColumn;
                    _DisplayColumnOptimizing = C.DisplayColumn;
                    OptimizingColumns[OptimizingColumn.DisplayText] = C.DisplayText;
                    OptimizingColumns[OptimizingColumn.DisplayColumn] = C.DisplayColumn;
                    OptimizingColumns[OptimizingColumn.IdentityColumn] = C.IdentityColumn;
                    OptimizingColumns[OptimizingColumn.OrderColumn] = C.OrderColumn;
                    OptimizingColumns[OptimizingColumn.Table] = C.TableName;
                    OptimizingColumns[OptimizingColumn.Module] = C.Module;
                    break;
                }
            }
            this.toolTipQueryList.SetToolTip(this.comboBoxQueryColumn, TipText);
            if (this._ManyOrderByColumns_Allow && this.ManyOrderByColumns_Controls != null)
            {
                this.ManyOrderByColumns_Reset();
                //this.ManyOrderByColumns_Sequence.Clear();
                //this.ManyOrderByColumns_Controls.Clear();
                this.ManyOrderByColumns_SetControls();
            }

        }

        public void SetQueryColumn(string DisplayText)
        {
            string Table = "";
            if (this.comboBoxQueryColumn.DataSource.GetType() == typeof(System.Data.DataTable))
            {
                System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxQueryColumn.DataSource;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() == DisplayText)
                    {
                        this.comboBoxQueryColumn.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private string QueryTableDefault()
        {
            string DefaultQueryTable = "";
            foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
            {
                DefaultQueryTable = C.TableName;
                break;
            }
            return DefaultQueryTable;
        }

        private string QueryTableCurrent()
        {
            string CurrentQueryTable = "";
            foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
            {
                if (C.DisplayColumn == this.SelectedDisplayColumn)
                {
                    CurrentQueryTable = C.TableName;
                    break;
                }
            }
            return CurrentQueryTable;
        }

        private bool QueryDatabase(bool Adding)
        {
            /// MW 17.04.2018 - Privacy consent
            DiversityWorkbench.Database.PrivacyConsent PC = Database.PrivacyConsent.undecided;
            if (!DiversityWorkbench.Database.PrivacyConsentOK(ref PC))
            {
                this.QueryTable.Clear();
                return false;
            }
            ///MW 9.4.2015: Optimizing
            if (UserControlQueryList.UseOptimizing)
            {
                this.OptimizingObjects_Reset();
                if (this.listBoxQueryResult.Items.Count == 0 && this.listBoxQueryResult.Sorted == false)
                    this.listBoxQueryResult.Sorted = true;
            }
            else
                this.listBoxQueryResult.Sorted = false;

            if (this.comboBoxQueryColumn.Text.Length == 0)
            {
                if (this._DtDisplayColumns != null)
                    System.Windows.Forms.MessageBox.Show(DiversityWorkbench.UserControls.UserControlQueryListText.Please_select_an_order_column);
                this.groupBoxQueryResults.Text = DiversityWorkbench.UserControls.UserControlQueryListText.Query_results;
                return false;
            }
            bool OK = false;
            string Header = "";
            string SQL = "";
            try
            {
                if (this.sqlDataAdapter != null && this.sqlConnection != null)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection();
                    if (this.sqlConnection == null)
                    {
                        con.ConnectionString = DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase);
                    }
                    else
                    {
                        con.ConnectionString = this.sqlConnection.ConnectionString;
                    }
                    SQL = this.QueryStringCount;
                    Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    /// Allow only 3 seconds here as it is of minor priority
                    //com.CommandTimeout = 3;// DiversityWorkbench.Settings.TimeoutDatabase;
                    con.Open();
                    Microsoft.Data.SqlClient.SqlTransaction T = con.BeginTransaction(IsolationLevel.ReadCommitted);
                    int i = 0;
                    try
                    {
                        // Markus 2020-02-28 - setting isolation level to read uncommitted
                        com.Transaction = T;
                        if (int.TryParse(com.ExecuteScalar().ToString(), out i))
                            this._NumberOfResultsets = i;
                        else
                        {
                            this._NumberOfResultsets = null;
                            i = -1;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        i = -1;
                        if (ex.Message.IndexOf("Timeout expired") > -1)
                            i = -2;
                    }
                    finally
                    {
                        if (T != null)
                            T.Commit();
                    }
                    if (i == 0)
                    {
                        this.buttonQueryNext.Visible = false;
                        if (Adding) Header = this.listBoxQueryResult.Items.Count.ToString() + "  +  " + DiversityWorkbench.UserControls.UserControlQueryListText.No_match;
                        else Header = DiversityWorkbench.UserControls.UserControlQueryListText.No_match;
                        if (this.sqlConnection.ConnectionString.Length > 0)
                        {
                            if (this.toolStripButtonNew.Enabled == false)
                            {
                                // Check INSERT
                                // #236
                                if (FormFunctions.getObjectPermissionCache(this._QueryMainTableLocal, FormFunctions.DatabaseGrant.Select) != null)
                                    OK = (bool)FormFunctions.getObjectPermissionCache(this._QueryMainTableLocal, FormFunctions.DatabaseGrant.Select);
                                else
                                {
                                    string SqlInsertPermission = "IF PERMISSIONS(OBJECT_ID('" + this._QueryMainTableLocal + "'))&8=8 SELECT 'True' ELSE SELECT 'False'";
                                    OK = System.Boolean.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlInsertPermission.ToString()));
                                }
                                this.toolStripButtonNew.Enabled = OK;
                            }
                        }
                    }
                    else
                    {
                        if (Adding) Header = this.listBoxQueryResult.Items.Count.ToString() + "  +  " + DiversityWorkbench.UserControls.UserControlQueryListText.Query_results + ": ";
                        else Header = DiversityWorkbench.UserControls.UserControlQueryListText.Query_results + "   ";
                        if (i > this.MaximalNumberOfResults)
                        {
                            if (this._GetNext)
                            {
                                this._GetNextStart += this.MaximalNumberOfResults;
                                int iAll = this._GetNextStart + i - 1;
                                Header = DiversityWorkbench.UserControls.UserControlQueryListText.items + " " + this._GetNextStart.ToString() + " - ";
                                Header += (this._GetNextStart + this.MaximalNumberOfResults - 1).ToString() + " " + DiversityWorkbench.UserControls.UserControlQueryListText.of + " " + iAll.ToString();
                            }
                            else if (this._GetPrevious)
                            {
                                if (this._GetNextRestriction.Length == 0)
                                    this._GetNextStart = 1;
                                else
                                    this._GetNextStart -= this.MaximalNumberOfResults;
                                int iAll = this._GetNextStart + i - 1;
                                Header = DiversityWorkbench.UserControls.UserControlQueryListText.items + " " + this._GetNextStart.ToString() + " - ";
                                Header += (this._GetNextStart + this.MaximalNumberOfResults - 1).ToString() + " " + DiversityWorkbench.UserControls.UserControlQueryListText.of + " " + iAll.ToString();
                            }
                            else
                            {
                                Header = Header + "1 - " + this.MaximalNumberOfResults.ToString() + " " + DiversityWorkbench.UserControls.UserControlQueryListText.of + " " + i.ToString();
                                this._GetNextStart = 1;
                            }
                            this.buttonQueryNext.Visible = true;
                        }
                        else
                        {
                            if (this._GetNext)
                            {
                                int iAll = -1;
                                //if (this._GetNextStart == 1) iAll--;
                                this._GetNextStart += this.MaximalNumberOfResults;
                                iAll += this._GetNextStart + i;
                                Header = DiversityWorkbench.UserControls.UserControlQueryListText.items + " " + this._GetNextStart.ToString() + " - ";
                                Header += iAll.ToString() + " " + DiversityWorkbench.UserControls.UserControlQueryListText.of + " " + iAll.ToString();
                            }
                            else if (this._GetPrevious)
                            {
                                int iAll = -1;
                            }
                            else if (i == -1)
                            {
                                // Markus 12.8.24 #2: Suppress Errormessage in header to not confuxe users as just the max count is missing
                                Header = Header + "1 - ... "; /// (" + UserControls.UserControlQueryListText.Error_in_Query + ")";
                            }
                            else if (i == -2)
                            {
                                Header = Header + "1 - max. " + this.MaximalNumberOfResults.ToString();
                            }
                            else
                            {
                                Header = Header + "1 - " + i.ToString();
                            }
                            this.buttonQueryNext.Visible = false;
                        }
                        ///MW 31.3.2015 - Optimierung der Abfragen - TODO
                        //string Query = this.QueryString(this.MaximalNumberOfResults);
                        //Query = this.OptimizeQuery(Query);
                        //this.sqlDataAdapter.SelectCommand.CommandText = Query;

                        this.sqlDataAdapter.SelectCommand.CommandText = this.QueryString(this.MaximalNumberOfResults);
                        this.sqlDataAdapter.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;


                        //int iii = this.dtQuery.Rows.Count;
#if DEBUG
                        // Markus 20.8.24: TestSql extracted for exception documentation
                        string TestSql = this.sqlDataAdapter.SelectCommand.CommandText;
                        try
                        {
                            System.Data.DataTable dt = new DataTable();
                            string ConnectionString = this.Connection.ConnectionString;
                            // only for debugging
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(this.sqlDataAdapter.SelectCommand.CommandText, this.Connection.ConnectionString);
                            // Markus 20.8.24: If Timeout is set to 0 it should not be used for chart
                            int TimeoutChart = DiversityWorkbench.Settings.TimeoutDatabase;
                            if (TimeoutChart == 0 || TimeoutChart > 10)
                            {
                                TimeoutChart = 10;
                            }
                            ad.SelectCommand.CommandTimeout = TimeoutChart;
                            //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(this.sqlDataAdapter.SelectCommand.CommandText, DiversityWorkbench.Settings.ConnectionString);
                            ad.Fill(dt);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, TestSql);
                        }
#endif
                        // Markus 2020-02-18 - Isolation level to read uncommited
                        if (this.sqlDataAdapter.SelectCommand.Connection.State == ConnectionState.Closed)
                            this.sqlDataAdapter.SelectCommand.Connection.Open();
                        // Markus 2022-08-04: Wenn trans schon da, dann nicht nochmal - sonst Fehler
                        if (this.sqlDataAdapter.SelectCommand.Transaction == null)
                        {
                            Microsoft.Data.SqlClient.SqlTransaction Trans = this.sqlDataAdapter.SelectCommand.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                            this.sqlDataAdapter.SelectCommand.Transaction = Trans;
                        }

                        // TODO: Optimieren - diese abfrage dauert zu lang
                        this.sqlDataAdapter.Fill(this.dtQuery);
                        if (this.sqlDataAdapter.SelectCommand.Connection.State == ConnectionState.Open)
                            this.sqlDataAdapter.SelectCommand.Connection.Close();
                    }
                    OK = true;
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException sex)
            {
                Header = DiversityWorkbench.UserControls.UserControlQueryListText.Error_in_Query;
                System.Windows.Forms.MessageBox.Show(sex.Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(sex, SQL);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.groupBoxQueryResults.Text = Header;
            }
            return OK;
        }

        /// <summary>
        /// Getting the where clause based on the interface IUserControlQueryCondition
        /// </summary>
        private string QueryStringWhereClause
        {
            get
            {
                string SQL = "";
                if (this._IsFreeText)
                {
                    SQL = this.QueryStringWhereClauseFreeText();
                }

                #region Predefined

                else if (this.IsPredefinedQuery)
                {
                    SQL = this.QueryStringWhereClausePredefined();
                    this.buttonQueryNext.Visible = false;
                }

                #endregion

                #region Not predefined query

                else
                {
                    if (UseOptimizing)
                    {
                        SQL = this.OptimizedQueryStringWhereClause();
                        //SQL = this.CheckFromClause(SQL);
                    }
                    else
                    {
                        string Prefix = "";
                        if (this.LinkedServer.Length > 0)
                            Prefix = "[" + this.LinkedServer + "]." + this.LinkedServerDatabase + ".dbo.";
                        else Prefix = "dbo.";

                        string QueryTable = "";
                        string IdentityColumn = "";
                        string DisplayColumn = "";
                        //bool ContainsAnnotation = false;
                        System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation> AnnotationControls = new List<UserControls.UserControlQueryConditionAnnotation>();
                        System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryCondition> FreeQueryControls = new List<UserControlQueryCondition>();

                        if (this.comboBoxQueryColumn.Text.Length > 0)
                        {
                            foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                            {
                                if (C.DisplayColumn == this.SelectedDisplayColumn)
                                {
                                    QueryTable = C.TableName;
                                    IdentityColumn = C.IdentityColumn;
                                    DisplayColumn = C.DisplayColumn;
                                    break;
                                }
                            }
                            if (QueryTable.Length > 0)
                            {
                                if (UseOptimizing)
                                    SQL += " FROM [" + QueryTable + "] AS T ";
                                else if (QueryTable.EndsWith("()"))
                                {
                                    SQL += " FROM " + Prefix + QueryTable + " AS T WHERE 1 = 1";
                                }
                                else
                                {
                                    SQL += " FROM " + Prefix;
                                    SQL += "[" + QueryTable + "] AS T WHERE 1 = 1";
                                }
                            }

                            #region Not optimzied, old version

                            //else
                            {
                                System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition> QueryConditions = new List<IUserControlQueryCondition>();
                                foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                                {
                                    if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                                    {
                                        foreach (System.Windows.Forms.Control U in G.Controls)
                                        {
                                            try
                                            {
                                                if (U.GetType() == typeof(System.Windows.Forms.PictureBox))
                                                    continue;
                                                DiversityWorkbench.IUserControlQueryCondition C = (DiversityWorkbench.IUserControlQueryCondition)U;
                                                string s = "";
                                                if (C.Condition().QueryFields != null && C.Condition().QueryFields.Count > 1)
                                                {
                                                    bool OK = false;
                                                    for (int i = 0; i < C.Condition().QueryFields.Count; i++)
                                                    {
                                                        if (C.SqlByIndex(i) != "")
                                                        {
                                                            if (i == 0)
                                                            {
                                                                s = " AND (";
                                                            }
                                                            if (i > 0)
                                                            {
                                                                if (C.Condition().CombineQueryFieldsWithAnd)
                                                                    s = " AND ";
                                                                else
                                                                    s = " OR ";
                                                            }
                                                            if (C.Condition().Table == QueryTable && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                                            {
                                                                // MW 9.4.2015: Optimizing
                                                                if (C.WhereClause() != "")
                                                                {
                                                                    //    if (UseOptimizing)
                                                                    //        s += C.WhereClause();
                                                                    //    else
                                                                    s += C.WhereClause();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                s += IdentityColumn + " IN (" + C.SqlByIndex(i) + ")";
                                                            }
                                                        }
                                                        if (s.Length > 0)
                                                        {
                                                            SQL += s;
                                                            OK = true;
                                                        }
                                                    }
                                                    if (OK) SQL += ")";
                                                }
                                                else
                                                {
                                                    if (C.Condition().Table == QueryTable && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                                    {
                                                        if (C.WhereClause() != "")
                                                            s = " AND " + C.WhereClause();
                                                    }
                                                    else
                                                    {
                                                        if (C.Condition().Table == QueryTable && C.WhereClause().Length > 0)
                                                        {
                                                            s = " AND " + C.WhereClause();
                                                        }
                                                        else if (QueryTable.IndexOf("_Core") > -1
                                                            && C.Condition().Table == QueryTable.Substring(0, QueryTable.IndexOf("_Core"))
                                                            && C.WhereClause().Length > 0)
                                                        {
                                                            s = " AND " + C.WhereClause();
                                                        }
                                                        else
                                                        {
                                                            if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                                                            {
                                                                if (C.WhereClause().Length > 0)
                                                                    s = " AND " + IdentityColumn + C.WhereClause();
                                                            }
                                                            else if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.ForeignKeyIsNull)
                                                            {
                                                                s = " AND " + IdentityColumn + C.WhereClause();
                                                            }
                                                            else if (C.SQL() != "")
                                                                s = " AND " + IdentityColumn + " IN (" + C.SQL() + ")";
                                                        }
                                                    }
                                                    if (s.Length > 0)
                                                        QueryConditions.Add(C);
                                                }
                                            }
                                            catch (System.Exception ex) { }
                                        }
                                    }
                                }

                                System.Collections.Generic.Dictionary<string, int> TableList = new Dictionary<string, int>();
                                System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition> CheckExistenceConditions = new List<IUserControlQueryCondition>();
                                foreach (DiversityWorkbench.IUserControlQueryCondition C in QueryConditions)
                                {
                                    if (C.Condition().CheckIfDataExist != QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                                    {
                                        if (!TableList.ContainsKey(C.Condition().Table))
                                            TableList.Add(C.Condition().Table, 1);
                                        else TableList[C.Condition().Table]++;
                                    }
                                    else if (C.Condition().Operator.Trim().Length > 0)
                                        CheckExistenceConditions.Add(C);
                                }

                                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in TableList)
                                {
                                    string SqlLocal = "";
                                    foreach (DiversityWorkbench.IUserControlQueryCondition C in QueryConditions)
                                    {
                                        if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                                            continue;
                                        if (C.Condition().Table == KV.Key)
                                        {
                                            if (SqlLocal.Length != 0) SqlLocal += " AND ";
                                            try
                                            {
                                                string s = "";
                                                if (C.Condition().QueryFields != null && C.Condition().QueryFields.Count > 1)
                                                {
                                                    bool OK = false;
                                                    for (int i = 0; i < C.Condition().QueryFields.Count; i++)
                                                    {
                                                        if (C.SqlByIndex(i) != "")
                                                        {
                                                            if (i == 0) s = " AND (";
                                                            if (i > 0)
                                                            {
                                                                if (C.Condition().CombineQueryFieldsWithAnd)
                                                                    s = " AND ";
                                                                else
                                                                    s = " OR ";
                                                            }
                                                            if (C.Condition().Table == QueryTable && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                                            {
                                                                if (C.WhereClause() != "")
                                                                    s += C.WhereClause();
                                                            }
                                                            else
                                                                s += C.SqlByIndex(i);
                                                            //s += IdentityColumn + " IN (" + C.SqlByIndex(i) + ")";
                                                        }
                                                        if (s.Length > 0)
                                                        {
                                                            SqlLocal += s;
                                                            OK = true;
                                                        }
                                                    }
                                                    if (OK) SqlLocal += ")";
                                                }
                                                else
                                                {
                                                    string BaseTable = QueryTable;
                                                    if (BaseTable.IndexOf("_Core") > -1)
                                                        BaseTable = BaseTable.Substring(0, BaseTable.IndexOf("_Core"));

                                                    if (C.Condition().Table == QueryTable
                                                        && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                                    {
                                                        if (C.WhereClause() != "")
                                                        {
                                                            s = " AND " + C.WhereClause();
                                                        }
                                                    }
                                                    else if (C.Condition().Table == BaseTable
                                                        && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                                    {
                                                        if (C.WhereClause() != "")
                                                        {
                                                            s = " AND " + C.WhereClause().Replace("[" + C.Condition().Table + "]", "[" + BaseTable + "_Core]");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (C.Condition().Table == QueryTable && C.WhereClause().Length > 0)
                                                        {
                                                            s = " AND " + C.WhereClause();
                                                        }
                                                        else
                                                        {
                                                            if (C.SQL() != "")
                                                            {
                                                                if (C.Condition().IsSet)
                                                                    s = " AND " + IdentityColumn + " IN (" + C.SQL() + ")";
                                                                else if (SqlLocal.Length > 0 && KV.Value > 1)
                                                                    s = C.WhereClause();
                                                                else if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                                                                    s = "";// AND " + IdentityColumn + C.WhereClause();
                                                                else if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.ForeignKeyIsNull)
                                                                    s = " AND " + C.WhereClause();
                                                                else
                                                                    s = C.SQL();

                                                            }
                                                            //s = " AND " + IdentityColumn + " IN (" + C.SQL + ")";
                                                        }
                                                    }
                                                    if (s.Length > 0)
                                                        SqlLocal += s;
                                                    if (SqlLocal.IndexOf(" AND  AND ") > -1)
                                                        SqlLocal = SqlLocal.Replace(" AND  AND ", " AND ");
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    if (SqlLocal.Length > 0)
                                    {
                                        if (SqlLocal.StartsWith(" AND "))
                                            SQL += SqlLocal;
                                        else
                                            SQL += " AND " + IdentityColumn + " IN (" + SqlLocal + ")";
                                    }
                                }

                                foreach (DiversityWorkbench.IUserControlQueryCondition C in CheckExistenceConditions)
                                {
                                    // MW 9.4.2015: Optimizing
                                    if (UseOptimizing && UserControlQueryList.TableAliases.Count > 0)
                                        SQL += " AND T." + IdentityColumn + C.WhereClause();
                                    else
                                        SQL += " AND " + IdentityColumn + C.WhereClause();
                                }
                            }

                            #endregion

                        }
                        this.SqlWhereClause = SQL;
                        if (this.QueryRestriction.Length > 0)
                        {
                            if (UseOptimizing && this.SqlWhereClause.Trim().EndsWith(" WHERE"))
                                SQL += " 1 = 1";
                            SQL += " " + this.QueryRestriction + " ";
                        }
                        if (this.getQueryRestrictionList().Count > 0)
                        {
                            foreach (DiversityWorkbench.UserControls.QueryRestrictionItem I in this.getQueryRestrictionList())
                            {
                                if (I.TableName == QueryTable)
                                    SQL += " AND T." + I.ColumnName + " " + I.Restriction + " ";
                                else
                                    SQL += " AND " + QueryTable + "." + IdentityColumn + " IN (SELECT " + IdentityColumn + " FROM " + I.TableName + " WHERE " + I.ColumnName + " " + I.Restriction + ") ";
                            }
                        }
                        if (this._GetNext || this._GetPrevious)
                        {
                            SQL += this._GetNextRestriction;
                        }
                    }
                }

                #endregion

                SQL = SQL.Replace("[[", "[");

                if (!DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing)
                {
                    // Bug fixing
                    SQL = SQL.Replace("]]", "]");
                    if (SQL.IndexOf(" AND  AND ") > -1)
                        SQL = SQL.Replace(" AND  AND ", " AND ");
                    if (SQL.IndexOf(" AND AND ") > -1)
                        SQL = SQL.Replace(" AND AND ", " AND ");
                    if (SQL.IndexOf(" AND ) AND ") > -1)
                        SQL = SQL.Replace(" AND ) AND ", " ) AND ");
                    SQL = SQL.Replace(" AND )", " )");
                }

                return SQL;
            }
        }

        /// <summary>
        /// Getting the where clause based on the interface IUserControlQueryCondition for predefined queries
        /// </summary>
        /// <returns></returns>
        private string QueryStringWhereClausePredefined()
        {
            string SQL = "";

            // Markus 31.1.2020: SelectedDisplayColumnTable has not been used
            string Table = this.SqlQueryTable.Replace("[", "").Replace("]", "");
            if (Table != this.SelectedDisplayColumnTable)
                Table = this.SelectedDisplayColumnTable;
            SQL = " FROM [" + Table + "] AS T " + this.SqlWhereClause;

            //if (this.SqlQueryTable.EndsWith("]"))
            //    SQL = " FROM " + this.SqlQueryTable + " AS T " + this.SqlWhereClause;
            //else
            //    SQL = " FROM [" + this.SqlQueryTable + "] AS T " + this.SqlWhereClause;

            // predefined query
            if (this._PredefinedQueryPersistentConditionList != null && this._PredefinedQueryPersistentConditionList.Count > 0 && this.comboBoxQueryColumn.Text.Length > 0)
            {

                string QueryTable = "";
                string IdentityColumn = "";
                string DisplayColumn = "";
                foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                {
                    if (C.DisplayColumn == this.SelectedDisplayColumn)
                    {
                        QueryTable = C.TableName;
                        IdentityColumn = C.IdentityColumn;
                        DisplayColumn = C.DisplayColumn;
                        break;
                    }
                }
                foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                {
                    if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        foreach (System.Windows.Forms.Control U in G.Controls)
                        {
                            try
                            {
                                DiversityWorkbench.IUserControlQueryCondition C = (DiversityWorkbench.IUserControlQueryCondition)U;
                                string s = "";
                                if (C.Condition().QueryFields.Count > 1)
                                {
                                    bool OK = false;
                                    for (int i = 0; i < C.Condition().QueryFields.Count; i++)
                                    {
                                        if (C.SqlByIndex(i) != "")
                                        {
                                            if (i == 0) s = " AND (";
                                            if (i > 0)
                                            {
                                                if (C.Condition().CombineQueryFieldsWithAnd)
                                                    s = " AND ";
                                                else
                                                    s = " OR ";
                                            }
                                            if (C.Condition().Table == QueryTable && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                            {
                                                if (C.WhereClause() != "")
                                                    s += C.WhereClause();
                                            }
                                            else
                                                s += IdentityColumn + " IN (" + C.SqlByIndex(i) + ")";
                                        }
                                        if (s.Length > 0)
                                        {
                                            SQL += s;
                                            OK = true;
                                        }
                                    }
                                    if (OK) SQL += ")";
                                }
                                else
                                {
                                    if (C.Condition().Table == QueryTable && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                    {
                                        if (C.WhereClause() != "")
                                            s = " AND " + C.WhereClause();
                                    }
                                    else
                                    {
                                        if (C.SQL() != "")
                                            s = " AND " + IdentityColumn + " IN (" + C.SQL() + ")";
                                    }
                                    if (s.Length > 0)
                                        SQL += s;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            return SQL;
        }

        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> CurrentQueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _Conditions = new List<QueryCondition>();
                if (this.IsPredefinedQuery)
                    return _Conditions;
                // not predefined query
                else
                {
                    System.Collections.Generic.Dictionary<string, int> SetPositions = new Dictionary<string, int>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<QueryCondition>> SetConditions = new Dictionary<string, List<QueryCondition>>();
                    try
                    {
                        foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                        {
                            if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                            {
                                foreach (System.Windows.Forms.Control U in G.Controls)
                                {
                                    try
                                    {
                                        if (U.GetType() == typeof(System.Windows.Forms.PictureBox))
                                            continue;
                                        DiversityWorkbench.IUserControlQueryCondition C = (DiversityWorkbench.IUserControlQueryCondition)U;
                                        if (C.WhereClause().Length > 0)
                                        {
                                            if (C.getCondition().IsSet)
                                            {
                                                DiversityWorkbench.UserControls.UserControlQueryConditionSet S = (DiversityWorkbench.UserControls.UserControlQueryConditionSet)C;
                                                DiversityWorkbench.QueryCondition Q = new QueryCondition(S.getCondition());
                                                if (!SetPositions.ContainsKey(Q.Table + "." + Q.Column))
                                                {
                                                    SetPositions.Add(Q.Table + "." + Q.Column, 0);
                                                    System.Collections.Generic.List<QueryCondition> QQ = new List<QueryCondition>();
                                                    QQ.Add(Q);
                                                    SetConditions.Add(Q.Table + "." + Q.Column, QQ);
                                                }
                                                else
                                                {
                                                    SetPositions[Q.Table + "." + Q.Column]++;
                                                    SetConditions[Q.Table + "." + Q.Column].Add(Q);
                                                }
                                                Q.SetPosition = SetPositions[Q.Table + "." + Q.Column];
                                                Q.SetCount = SetPositions[Q.Table + "." + Q.Column] + 1;
                                                //_Conditions.Add(Q);
                                            }
                                            else
                                                _Conditions.Add(C.getCondition());
                                            //    this.CurrentQuerySetSeletedIndices.Add(C.getCondition().SelectedIndex);
                                            //TestSelectedIndex.Add(C.getCondition().SelectedIndex);
                                        }
                                        else if (C.Condition().QueryConditionOperator == "•" && C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                                        {
                                            _Conditions.Add(C.getCondition());
                                        }
                                        else if (C.Condition().Operator == "•" && C.Condition().QueryType == QueryCondition.QueryTypes.ReferencingTable && !this.OptimizingIsUsed())
                                        {
                                            _Conditions.Add(C.getCondition());
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {
                                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    if (SetConditions.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<QueryCondition>> KV in SetConditions)
                        {
                            foreach (QueryCondition Q in KV.Value)
                            {
                                Q.SetCount = SetPositions[KV.Key] + 1;
                                _Conditions.Add(Q);
                            }
                            //_Conditions.Add(
                        }
                    }
                    //for (int i = 0; i < TestSelectedIndex.Count; i++)
                    //{
                    //    if (_Conditions[i].SelectedIndex != TestSelectedIndex[i])
                    //    {
                    //        _Conditions[i].SelectedIndex = TestSelectedIndex[i];
                    //    }
                    //}
                }
                return _Conditions;
            }
        }

        private void TransferCurrentQueryConditions()
        {
            if (this.IsPredefinedQuery)
                return;
            // not predefined query
            else
            {
                foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                {
                    if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        foreach (System.Windows.Forms.Control U in G.Controls)
                        {
                            try
                            {
                                DiversityWorkbench.IUserControlQueryCondition C = (DiversityWorkbench.IUserControlQueryCondition)U;
                                //int iSetPosition = 0;
                                foreach (DiversityWorkbench.QueryCondition QC in this._CurrentQueryConditions)
                                {
                                    int i = QC.SelectedIndex;
                                    if (C.Condition().Column == QC.Column &&
                                        C.Condition().Table == QC.Table &&
                                        C.Condition().SetCount == QC.SetCount)
                                    {
                                        if (QC.Entity != null &&
                                            QC.Entity.Length > 0 &&
                                            C.Condition().Entity != null &&
                                            C.Condition().Entity.Length > 0 &&
                                            QC.Entity != C.Condition().Entity)
                                            continue;
                                        C.setConditionValues(QC);
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private bool IfColumnExistsInQueryTable(string TableName, string ColumnName)
        {
            bool OK = false;
            string Error = "";
            string SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = '" + TableName + "' and c.COLUMN_NAME = '" + ColumnName + "'";//SELECT * FROM " + TableName + " WHERE 1 = 0";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
            if (Result == "1")
                OK = true;
            return OK;
        }

        private string QueryStringCount
        {
            get
            {
                this.OptimizingObjects_Reset();

                string SQL = this.QueryStringWhereClause;
                if (UseOptimizing)
                {
                    if (SQL.ToLower().IndexOf(" where ") == -1)
                        SQL = this.QueryStringWhereClause;
                    string IdentityColumn = "";
                    foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                    {
                        if (C.DisplayColumn == this.SelectedDisplayColumn)
                        {
                            IdentityColumn = C.IdentityColumn;
                            break;
                        }
                    }
                    SQL = "SELECT COUNT(DISTINCT T." + IdentityColumn + ") " + SQL;
                }
                else
                {
                    SQL = "SELECT COUNT(*) " + SQL;
                }
                return SQL;
            }
        }

        private string RestrictionForNextDatasets
        {
            get
            {
                string Restriction = "";
                if (!this._GetNext) return Restriction;
                string SQL = "";
                if (this.IsPredefinedQuery)
                {
                    return Restriction;
                }
                else
                {
                    string QueryTable = "";
                    string IdentityColumn = "";
                    string DisplayColumn = "";
                    string OrderColumn = "";
                    if (this.comboBoxQueryColumn.Text.Length > 0)
                    {
                        foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                        {
                            if (C.DisplayColumn == this.SelectedDisplayColumn)
                            {
                                QueryTable = C.TableName;
                                IdentityColumn = C.IdentityColumn;
                                DisplayColumn = C.DisplayColumn;
                                OrderColumn = C.OrderColumn;
                                break;
                            }
                        }
                    }
                    // for functions as source, an alias must be used
                    QueryTable = "T";
                    if (this.listBoxQueryResult.Items.Count > 0)
                    {
                        int i = this.listBoxQueryResult.Items.Count - 1;
                        System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxQueryResult.Items[this.listBoxQueryResult.Items.Count - 1];
                        string ID = R["ID"].ToString();
                        int id;
                        if (!int.TryParse(ID, out id))
                            ID = "'" + ID + "'";
                        string OrderBy = R["OrderBy"].ToString();
                        if (OrderBy.Length == 0 && this.ManyOrderByColumns())
                        {
                            if (R.Row.Table.Columns.Contains(OrderColumn))
                                OrderBy = R[OrderColumn].ToString();
                        }
                        OrderBy = "'" + OrderBy.Replace("'", "' + char(39) + '") + "'";
                        SQL = " AND (((" + QueryTable + "." + OrderColumn + " >= " + OrderBy + " " +
                            " AND " + QueryTable + "." + IdentityColumn + " >  " + ID + ") " +
                            " OR (" + QueryTable + "." + OrderColumn + " > " + OrderBy + " ))" +
                            " OR (" + QueryTable + "." + OrderColumn + " IS NULL AND " + QueryTable + "." + IdentityColumn + " > " + ID + "))";
                    }
                }
                return SQL;
            }
        }

        private string QueryString(int maxCount)
        {
            // MW 9.4.2015: Optimizing
            this.OptimizingObjects_Reset();

            string SQL = "";
            try
            {
                //if (UseOptimizing)
                SQL += "SELECT DISTINCT ";
                string QueryTable = "";
                string IdentityColumn = "";
                string DisplayColumn = "";
                string OrderColumn = "";
                if (this.comboBoxQueryColumn.Text.Length > 0)
                {
                    foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                    {
                        if (C.DisplayColumn == this.SelectedDisplayColumn)
                        {
                            QueryTable = C.TableName;
                            IdentityColumn = C.IdentityColumn;
                            DisplayColumn = C.DisplayColumn;
                            OrderColumn = C.OrderColumn;
                            if (this._OrderByPrefixes != null &&
                                this._OrderByPrefixes.Count > 0 &&
                                this._OrderByPrefixColumn == this.SelectedDisplayColumn &&
                                this._OrderByPrefixTable == this.SelectedDisplayColumnTable)
                            {
                                try
                                {
                                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQueryColumn.SelectedItem;
                                    if (this._OrderByPrefixes.ContainsKey(R[0].ToString()))
                                    {
                                        if (DiversityWorkbench.Forms.FormFunctions.SqlServerVersion() > 10)
                                            OrderColumn = "try_parse(rtrim(ltrim(substring(T.[" + C.OrderColumn + "], PatINDEX('%" + this._OrderByPrefixes[R[0].ToString()] + "%', T.[" + C.OrderColumn + "]) + len('" + this._OrderByPrefixes[R[0].ToString()] + "'), 500))) as int)";
                                        else
                                        {
                                            System.Windows.Forms.MessageBox.Show("Order column set to\r\n" + C.OrderColumn + "\r\ninstead of\r\n" + R[0].ToString() + ".\r\n\r\nYou need at least SqlServer 12 to use this function", "Order column changed", MessageBoxButtons.OK);
                                            OrderColumn = C.OrderColumn;
                                        }
                                    }
                                }
                                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                            }
                            break;
                        }
                    }
                    //if (QueryTable == "Agent" && this.IsPredefinedQuery) ;
                    //else
                    if (this._NumberOfResultsets > maxCount)
                        SQL += " TOP " + maxCount.ToString() + " ";
                    ///TODO: MW 31.3.2015 - hier muessen die Spalten durch das Alias T. ergaenzt werden um eine Optimierung der Abfragen zu ermoeglichen
                    if (QueryTable.Length > 0)
                    {
                        //MW 9.4.2015: Optimizing
                        string IdentityColumnAlias = IdentityColumn;
                        string DisplayAlias = DisplayColumn;
                        string OrderAlias = OrderColumn;
                        if (UseOptimizing)
                        {
                            // Markus 15.3.2023: Bugfix - Kollision von Spaltennamen mit geschützten Namen wie e.g. Order
                            IdentityColumnAlias = "T.[" + IdentityColumnAlias + "]";
                            if (DisplayAlias.IndexOf(OrderAlias) > 0)
                                DisplayAlias = DisplayAlias.Replace(OrderAlias, "T.[" + OrderAlias + "]");
                            else
                                DisplayAlias = "T.[" + DisplayAlias + "]";
                            if (OrderAlias.IndexOf("try_parse(rtrim(ltrim(substring(T.[") == -1)
                                OrderAlias = "T.[" + OrderAlias + "]";
                            this.OptimizingColumns[OptimizingColumn.DisplayText] = this.comboBoxQueryColumn.SelectedText;
                            this.OptimizingColumns[OptimizingColumn.DisplayColumn] = DisplayColumn;
                            this.OptimizingColumns[OptimizingColumn.DisplayAlias] = DisplayAlias;
                            this.OptimizingColumns[OptimizingColumn.IdentityColumn] = IdentityColumn;
                            this.OptimizingColumns[OptimizingColumn.IdentityAlias] = IdentityColumnAlias;
                            this.OptimizingColumns[OptimizingColumn.OrderColumn] = OrderColumn;
                            this.OptimizingColumns[OptimizingColumn.Table] = QueryTable;
                        }

                        // Markus 10.1.2022: Bugfix for predefined queries
                        if (!this.IsPredefinedQuery &&
                            (ManyOrderByColumns() ||
                            (this._QueryMainTableLocal != null
                            && ManyOrderByColumns_TableAliases().Count > 0
                            && ManyOrderByColumns_TableAliases().ContainsKey(this._QueryMainTableLocal)
                            && !UserControlQueryList.TableAliases.ContainsKey(this._QueryMainTableLocal)
                            && this._QueryMainTableLocal != UserControlQueryList.QueryMainTable
                            && UseOptimizing)))
                        {
                            SQL += ManyOrderByColumns_IDcolumn(IdentityColumn) + " AS ID, ";
                            // Markus 16.12.22 - Bugfix - die Spalte taucht in Order by auf und muss mit rein
                            SQL += IdentityColumnAlias + ", ";
                        }
                        else
                            SQL += IdentityColumnAlias + " AS ID, ";
                        if (ManyOrderByColumns())
                        {
                            string PerpareTableAlias = this.QueryStringWhereClause;
                            SQL += this.ManyOrderByColumns_ColumnClause();// DisplayAlias, IdentityColumnAlias);
                        }
                        else
                        {
                            if (QueryTable == "Agent" && this.IsPredefinedQuery)
                            {
                                SQL += " CASE WHEN " + DisplayAlias + " IS NULL OR LEN(" + DisplayAlias + ") = 0 THEN 'ID: ' + CAST(" + IdentityColumnAlias + " AS NVARCHAR) " +
                                    " ELSE " + DisplayAlias + " END AS Display ";
                            }
                            else
                            {
                                if (this._FormatDateToYearMonthDay)
                                {
                                    SQL += this.QueryStringForColumn(DisplayAlias, IdentityColumnAlias, 255) + " AS Display";
                                }
                                else
                                {
                                    SQL += " CASE WHEN " + DisplayAlias + " IS NULL OR LEN(" + DisplayAlias + ") = 0 " +
                                        " THEN 'ID: ' + CAST(" + IdentityColumnAlias + " AS NVARCHAR) " +
                                        " ELSE cast(" + DisplayAlias + " as nvarchar(255))  " +
                                        " END AS Display ";
                                }
                            }

                            SQL += " , " + OrderAlias + " AS OrderBy ";
                        }
                        SQL += this.QueryStringWhereClause;

                        // TODO: bei Einbau von ORDER BY kommt die Abfrage bei Agents nicht zurueck - Ursache unklar
                        if (QueryTable == "Agent"
                            && this.IsPredefinedQuery
                            && this._NumberOfResultsets > maxCount)
                        { }
                        else
                        {
                            this.listBoxQueryResult.Sorted = false;
                            if (!UseOptimizing)
                            {
                                SQL += " ORDER BY " + OrderAlias;
                                SQL += " " + this._Sorting;// this.buttonSwitchDescAsc.Tag.ToString();
                                if (OrderColumn != IdentityColumn)
                                    SQL += ", " + IdentityColumnAlias;
                            }
                            else
                            {
                                if (ManyOrderByColumns())
                                {
                                    SQL += " ORDER BY ";
                                    if (DisplayAlias.Length > 0 && DisplayAlias != OrderAlias)
                                        SQL += DisplayAlias;
                                    else SQL += OrderAlias;
                                    SQL += " " + this._Sorting;
                                    SQL += ", " + ManyOrderByColumns_OrderByClause();
                                }
                                else
                                {
                                    SQL += " ORDER BY " + OrderAlias;
                                    SQL += " " + this._Sorting;

                                }
                                if (OrderColumn != _IdentityColumnOptimizing && ManyOrderByColumns())
                                {
                                    SQL += ", " + this.ManyOrderByColumns_IDcolumn(IdentityColumn);
                                    //if (ManyOrderByColumns_TableAliases().ContainsKey(this._QueryMainTableLocal))
                                    //{
                                    //    string MainTableAlias = ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal];
                                    //    SQL += ", CASE WHEN " + IdentityColumnAlias + " IS NULL THEN " + MainTableAlias + "." + IdentityColumn + " ELSE " + IdentityColumnAlias + " END ";
                                    //}
                                    //else
                                    //    SQL += ", " + IdentityColumnAlias;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); SQL = ""; }
            return SQL;
        }



        private string QueryStringForColumn(string DisplayAlias, string IdentityColumnAlias, int Width)
        {
            return " CASE WHEN " + DisplayAlias + " IS NULL OR LEN(" + DisplayAlias + ") = 0 THEN 'ID: ' + CAST(" + IdentityColumnAlias + " AS NVARCHAR) " +
            " ELSE " +
            " case when isdate(" + DisplayAlias + ") = 1  " +
            " and ISNUMERIC(" + DisplayAlias + ") = 0 " +
            " and " + DisplayAlias + " not like '%-%/%' " +
            " and " + DisplayAlias + " not like '%/%-%' " +
            " and " + DisplayAlias + " not like '%.%-%' " +
            " and " + DisplayAlias + " not like '%.%/%' " +
            " then cast(Year(" + DisplayAlias + ") as nvarchar) + '-'  " +
            " + case when Month(" + DisplayAlias + ") < 10 then '0' else '' end " +
            " + cast(Month(" + DisplayAlias + ") as nvarchar) + '-'  " +
            " + case when Day(" + DisplayAlias + ") < 10 then '0' else '' end " +
            " + cast(Day(" + DisplayAlias + ") as nvarchar) " +
            " else cast(" + DisplayAlias + " as nvarchar(" + Width.ToString() + "))  " +
            " end " +
            " END ";
        }


        #endregion

        #region Backlinks

        /// <summary>
        /// ToDos:
        /// im Hauptformular:
        /// 
        /// private void moduleConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        /// {
        /// DiversityWorkbench.FormConnectionAdministration f = new DiversityWorkbench.FormConnectionAdministration(System.Windows.Forms.Application.StartupPath + "\\DiversityTaxonNames.chm", true);
        /// f.ShowDialog();
        /// }
        /// am Ende , true um die Option fuer die Auswahl der Datenbanken zu aktivieren
        /// 
        /// in 
        /// private bool setDatabase()
        /// this.userControlQueryList.BacklinkUpdateEnabled = true;
        /// einfuegen
        /// 
        /// in Update funktion
        /// am anfang:
        ///  System.Collections.Generic.Dictionary<string, string> PreviousContent = new Dictionary<string, string>();
        ///  if (this.dataSetAgent.Agent.Rows.Count > 0)
        ///  {
        ///        System.Data.DataRow Rprevious = this.dataSetAgent.Agent.Rows[0];
        ///        PreviousContent.Add("AgentName", Rprevious["AgentName"].ToString());
        ///   }
        ///   
        /// am ende:
        /// System.Collections.Generic.Dictionary<string, string> CurrentContent = new Dictionary<string, string>();
        /// System.Data.DataRow Rcurrent = this.dataSetAgent.Agent.Rows[0];
        /// CurrentContent.Add("AgentName", Rcurrent["AgentName"].ToString());
        ///  if (CurrentContent["AgentName"] != PreviousContent["AgentName"])
        ///  {
        ///      DiversityWorkbench.WorkbenchUnit.BacklinkAddID((int)this.ID);
        ///      this.userControlQueryList.BacklinkSetCount();
        ///  }
        /// 
        /// </summary>

        private bool _BacklinkUpdateEnabled = false;
        public bool BacklinkUpdateEnabled
        {
            get { return _BacklinkUpdateEnabled; }
            set
            {
                _BacklinkUpdateEnabled = value;
                this.toolStripButtonBacklinkUpdate.Visible = _BacklinkUpdateEnabled;
                if (_BacklinkUpdateEnabled)
                    this.toolStripQueryList.ContextMenuStrip = this.contextMenuStripBacklink;
                else
                    this.toolStripQueryList.ContextMenuStrip = null;
            }
        }

        //private string _BacklinkUri;
        //private System.Collections.Generic.List<int> _BacklinkIDs;
        private void toolStripButtonBacklinkUpdate_Click(object sender, EventArgs e)
        {
            string Message = "";
            // Markus 25.7.23: Different message if nothing has been updated
            if (this.BacklinkUpdate(ref Message))
            {
                string message = "Linked dataset updated" + Message;
                if (Message.Length == 0) message = "0 datasets updated";
                System.Windows.Forms.MessageBox.Show(message, "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string message = "Update of linked datasets failed" + Message;
                System.Windows.Forms.MessageBox.Show(message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.toolStripButtonBacklinkUpdate.BackColor = System.Drawing.SystemColors.Control;
        }

        private bool BacklinkUpdate(ref string Message)
        {
            return DiversityWorkbench.WorkbenchUnit.BacklinkUpdate(ref Message, DiversityWorkbench.Settings.ModuleName);
        }

        private bool BacklinkSetBaseUri(string URI)
        {
            return DiversityWorkbench.WorkbenchUnit.BacklinkSetBaseUri(URI);
        }
        public void BacklinkAddID(int ID)
        {
            DiversityWorkbench.WorkbenchUnit.BacklinkAddID(ID);
        }

        public bool BacklinkReset()
        {
            return DiversityWorkbench.WorkbenchUnit.BacklinkReset();
        }


        public void BacklinkSetCount()
        {
            if (DiversityWorkbench.WorkbenchUnit.BacklinkCount() > 0) this.toolStripButtonBacklinkUpdate.BackColor = System.Drawing.Color.Pink;
            else this.toolStripButtonBacklinkUpdate.BackColor = System.Drawing.SystemColors.Control;
        }

        private void insertCurrentIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.toolStripButtonBacklinkUpdate.Visible && this.ID > -1)
            {
                DiversityWorkbench.WorkbenchUnit.BacklinkAddID((int)this.ID);
                this.BacklinkSetCount();
            }
        }

        private void insertAllIDsInBacklinkListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.toolStripButtonBacklinkUpdate.Visible && this.ID > -1)
            {
                foreach (int id in this.ListOfIDs)
                    DiversityWorkbench.WorkbenchUnit.BacklinkAddID(id);
                this.BacklinkSetCount();
            }
        }


        private void resetBacklinkListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.toolStripButtonBacklinkUpdate.Visible && this.ID > -1)
            {
                DiversityWorkbench.WorkbenchUnit.BacklinkClear();
                this.BacklinkSetCount();
            }
        }

        private void showCurrentListOfIDsForBacklinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.WorkbenchUnit.BacklinkIDs != null)
            {
                System.Data.DataTable dt = new DataTable();
                System.Data.DataColumn dc = new DataColumn("ID of Agent", typeof(int));
                dt.Columns.Add(dc);
                foreach (int id in DiversityWorkbench.WorkbenchUnit.BacklinkIDs)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    dr[0] = id;
                    dt.Rows.Add(dr);
                }
                DiversityWorkbench.Forms.FormTableContent f = new Forms.FormTableContent("Backlink IDs", "IDs for update of linked datasets", dt);
                if (f.Height > this.Height)
                    f.Height = this.Height - 10;
                f.Width = 80;
                f.ShowDialog();
            }
        }

        #endregion

        #region Optimizing

        #region static objects

        #region usage

        //private static bool _OptimizedByDefault = false;

        //public static bool OptimizedByDefault { get => _OptimizedByDefault; set => _OptimizedByDefault = value; }

        private static bool _UseOptimizing = false;
        public static bool UseOptimizing
        {
            get { return DiversityWorkbench.UserControls.UserControlQueryList._UseOptimizing; }
            set
            {
                DiversityWorkbench.UserControls.UserControlQueryList._UseOptimizing = value;
            }
        }

        private static System.Collections.Generic.Dictionary<string, bool> _Optimizing_UsageByForm;

        public static bool OptimizingUsedByForm(string Form)
        {
            if (_Optimizing_UsageByForm == null) _Optimizing_UsageByForm = new Dictionary<string, bool>();
            if (_Optimizing_UsageByForm.ContainsKey(Form)) return _Optimizing_UsageByForm[Form];
            return false;
        }

        public static void OptimizingUsedByForm(string Form, bool Usage)
        {
            if (_Optimizing_UsageByForm == null) _Optimizing_UsageByForm = new Dictionary<string, bool>();
            if (_Optimizing_UsageByForm.ContainsKey(Form)) _Optimizing_UsageByForm[Form] = Usage;
            else _Optimizing_UsageByForm.Add(Form, Usage);
        }

        #endregion

        #region Dictionary based values

        private static System.Collections.Generic.Dictionary<string, string> _ModuleForm_IdentityColumnsOptimizing;
        private static System.Collections.Generic.Dictionary<string, string> _ModuleForm_DisplayColumnsOptimizing;
        private static System.Collections.Generic.Dictionary<string, string> _ModuleForm_QueryMainTablesOptimizing;
        public static string ModuleForm_IdentityColumnsOptimizing(string Module, string Form, string Column)
        {
            if (_ModuleForm_IdentityColumnsOptimizing == null) _ModuleForm_IdentityColumnsOptimizing = new Dictionary<string, string>();
            if (!_ModuleForm_IdentityColumnsOptimizing.ContainsKey(ModuleForm_Key(Module, Form))) _ModuleForm_IdentityColumnsOptimizing.Add(ModuleForm_Key(Module, Form), Column);
            return _ModuleForm_IdentityColumnsOptimizing[ModuleForm_Key(Module, Form)];
        }

        public static string ModuleForm_DisplayColumnsOptimizing(string Module, string Form, string Column)
        {
            if (_ModuleForm_DisplayColumnsOptimizing == null) _ModuleForm_DisplayColumnsOptimizing = new Dictionary<string, string>();
            if (!_ModuleForm_DisplayColumnsOptimizing.ContainsKey(ModuleForm_Key(Module, Form))) _ModuleForm_DisplayColumnsOptimizing.Add(ModuleForm_Key(Module, Form), Column);
            return _ModuleForm_DisplayColumnsOptimizing[ModuleForm_Key(Module, Form)];
        }

        public static string ModuleForm_QueryMainTableOptimizing(string Module, string Form, string Table)
        {
            if (_ModuleForm_QueryMainTablesOptimizing == null) _ModuleForm_QueryMainTablesOptimizing = new Dictionary<string, string>();
            if (!_ModuleForm_QueryMainTablesOptimizing.ContainsKey(ModuleForm_Key(Module, Form))) _ModuleForm_QueryMainTablesOptimizing.Add(ModuleForm_Key(Module, Form), Table);
            return _ModuleForm_QueryMainTablesOptimizing[ModuleForm_Key(Module, Form)];
        }

        private static string ModuleForm_Key(string Module, string Form) { return Module + "|" + Form; }

        #endregion

        private static string _QueryMainTableOptimizing;
        private static string _QueryMainTable;
        public static string QueryMainTable
        {
            get
            {
                if (DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing)
                {
                    if (_QueryMainTableDict != null)
                    {
                        // Markus 30.1.2023: Returning value if it exists
                        if (_QueryMainTableDict.ContainsKey(DiversityWorkbench.Settings.ModuleName))
                            return _QueryMainTableDict[DiversityWorkbench.Settings.ModuleName];
                    }
                    if (DiversityWorkbench.UserControls.UserControlQueryList._QueryMainTableOptimizing == null)
                    {
                    }
                    return DiversityWorkbench.UserControls.UserControlQueryList._QueryMainTableOptimizing;
                }
                else return DiversityWorkbench.UserControls.UserControlQueryList._QueryMainTable;
            }
            set
            {
                DiversityWorkbench.UserControls.UserControlQueryList._QueryMainTable = value;
                if (value.Length > 0)
                {
                    DiversityWorkbench.UserControls.UserControlQueryList._QueryMainTableOptimizing = value;
                    if (_QueryMainTableDict == null) _QueryMainTableDict = new Dictionary<string, string>();
                    if (!_QueryMainTableDict.ContainsKey(DiversityWorkbench.Settings.ModuleName))
                        _QueryMainTableDict.Add(DiversityWorkbench.Settings.ModuleName, value);
                    else
                        _QueryMainTableDict[DiversityWorkbench.Settings.ModuleName] = value; // Markus 30.1.2023: setting value if present
                }
            }
        }

        /// <summary>
        /// Setting the main query table in combination with the related module
        /// </summary>
        /// <param name="column">The query display column</param>
        public static void SetQueryMainTable(DiversityWorkbench.UserControls.QueryDisplayColumn column)
        {
            try
            {
                DiversityWorkbench.UserControls.UserControlQueryList._QueryMainTable = column.TableName;
                DiversityWorkbench.UserControls.UserControlQueryList._QueryMainTableOptimizing = column.TableName;
                if (_QueryMainTableDict == null) _QueryMainTableDict = new Dictionary<string, string>();
                if (column.Module != null && column.Module.Length > 0 && !_QueryMainTableDict.ContainsKey(column.Module))
                    _QueryMainTableDict.Add(column.Module, column.TableName);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private static System.Collections.Generic.Dictionary<string, string> _QueryMainTableDict;


        private static string _IdentityColumnOptimizing;
        private static string _DisplayColumnOptimizing;

        public static string IdentityColumnOptimizing { get { return _IdentityColumnOptimizing; } }

        private static System.Collections.Generic.Dictionary<string, string> _TableAliases;

        /// <summary>
        /// Dictionary containing the aliases for the tables
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, string> TableAliases
        {
            get
            {
                if (UserControlQueryList._TableAliases == null)
                    UserControlQueryList._TableAliases = new Dictionary<string, string>();
                return UserControlQueryList._TableAliases;
            }
            set { UserControlQueryList._TableAliases = value; }
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityWorkbench.QueryCondition>> _TableAliasesNotExists;
        /// <summary>
        /// Table aliases included in a non exists clause or count(*) for QueryType Count that should not appear in the from clause of the main statement
        /// e.g.: 
        /// SELECT * 
        /// FROM CollectionSpecimen_Core AS T , CollectionEventLocalisation AS T0 
        /// WHERE not exists (select * from CollectionEventLocalisation AS T0 where T.CollectionEventID = T0.CollectionEventID) 
        /// or 
        /// ... WHERE  T.CollectionSpecimenID
        /// IN (SELECT T0.CollectionSpecimenID FROM CollectionAgent AS T0 
        /// WHERE  T.CollectionSpecimenID = T0.CollectionSpecimenID
        /// GROUP BY T0.CollectionSpecimenID
        /// HAVING COUNT(*) = 2)
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityWorkbench.QueryCondition>> TableAliasesNotExists
        {
            get
            {
                if (UserControlQueryList._TableAliasesNotExists == null)
                {
                    UserControlQueryList._TableAliasesNotExists = new Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityWorkbench.QueryCondition>>();
                }
                return UserControlQueryList._TableAliasesNotExists;
            }
            set { UserControlQueryList._TableAliasesNotExists = value; }
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition>> _TableQueryConditions;

        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition>> TableQueryConditions
        {
            get
            {
                if (_TableQueryConditions == null)
                    UserControlQueryList._TableQueryConditions = new Dictionary<string, List<IUserControlQueryCondition>>();
                return UserControlQueryList._TableQueryConditions;
            }
            set { UserControlQueryList._TableQueryConditions = value; }
        }

        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionSet>>> _QueryConditionSets;

        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionSet>>> QueryConditionSets
        {
            get
            {
                if (UserControlQueryList._QueryConditionSets == null)
                    UserControlQueryList._QueryConditionSets = new Dictionary<string, Dictionary<string, System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionSet>>>();
                return UserControlQueryList._QueryConditionSets;
            }
            set { UserControlQueryList._QueryConditionSets = value; }
        }

        public enum QueryOperator { AND, OR }
        private static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, QueryOperator>>> _QueryConditionSetOperators;

        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, QueryOperator>>> QueryConditionSetOperators
        {
            get
            {
                if (UserControlQueryList._QueryConditionSetOperators == null)
                    UserControlQueryList._QueryConditionSetOperators = new System.Collections.Generic.Dictionary<string, Dictionary<string, Dictionary<string, QueryOperator>>>();
                return UserControlQueryList._QueryConditionSetOperators;
            }
            set { UserControlQueryList._QueryConditionSetOperators = value; }
        }


        private static System.Collections.Generic.Dictionary<DiversityWorkbench.UserControls.UserControlQueryConditionSet, string> _QueryConditionSetTableAliases;

        public static System.Collections.Generic.Dictionary<DiversityWorkbench.UserControls.UserControlQueryConditionSet, string> QueryConditionSetTableAliases
        {
            get
            {
                if (UserControlQueryList._QueryConditionSetTableAliases == null)
                    UserControlQueryList._QueryConditionSetTableAliases = new Dictionary<UserControls.UserControlQueryConditionSet, string>();
                return UserControlQueryList._QueryConditionSetTableAliases;
            }
            set { UserControlQueryList._QueryConditionSetTableAliases = value; }
        }

        #endregion


        #region Handling the optimizing
        /// <summary>
        /// Resetting the objects used for optimized version of the query
        /// </summary>
        private void OptimizingObjects_Reset()
        {
            try
            {
                UserControlQueryList.TableAliases = null;
                UserControlQueryList._TableQueryConditions = null;
                UserControlQueryList._TableAliasesNotExists = null;
                UserControlQueryList._QueryConditionSets = null;
                UserControlQueryList._QueryConditionSetTableAliases = null;
                this.ResetTempIdTable();
                this.OptimizingColumns = null;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private enum OptimizingColumn { Table, DisplayColumn, DisplayAlias, DisplayText, IdentityAlias, IdentityColumn, OrderColumn, Module }
        private System.Collections.Generic.Dictionary<OptimizingColumn, string> _OptimizingColumns;
        private Dictionary<OptimizingColumn, string> OptimizingColumns
        {
            get
            {
                if (_OptimizingColumns == null)
                {
                    _OptimizingColumns = new Dictionary<OptimizingColumn, string>();
                    _OptimizingColumns.Add(OptimizingColumn.DisplayText, "");
                    _OptimizingColumns.Add(OptimizingColumn.DisplayColumn, "");
                    _OptimizingColumns.Add(OptimizingColumn.DisplayAlias, "");
                    _OptimizingColumns.Add(OptimizingColumn.IdentityColumn, "");
                    _OptimizingColumns.Add(OptimizingColumn.IdentityAlias, "");
                    _OptimizingColumns.Add(OptimizingColumn.OrderColumn, "");
                    _OptimizingColumns.Add(OptimizingColumn.Table, "");
                    _OptimizingColumns.Add(OptimizingColumn.Module, "");
                }
                return _OptimizingColumns;
            }
            set => _OptimizingColumns = value;
        }

        /// <summary>
        /// Optimize query
        /// </summary>
        /// <param name="Query">The original select statement</param>
        /// <returns>An optimized select statement</returns>
        private string OptimizeQuery(string Query)
        {
            ///TODO: MW 31.3.2015 - In clauses must vanish and should be replaced by direct joins

            // Beispiel fuer Query
            //SELECT NameID AS ID,  CASE WHEN Display IS NULL OR LEN(Display) = 0 THEN 'ID: ' + CAST(NameID AS VARCHAR)  ELSE  case when (SELECT MIN(DATA_TYPE) AS DATA_TYPE  
            //FROM INFORMATION_SCHEMA.COLUMNS  WHERE (TABLE_NAME = 'TaxonName_Indicated') AND (COLUMN_NAME = 'Display')) in ('datetime', 'smalldatetime')  then cast(Year(Display) as varchar) + '-'   + 
            //case when Month(Display) < 10 then '0' else '' end  + cast(Month(Display) as varchar) + '-'   + case when Day(Display) < 10 then '0' else '' end  + cast(Day(Display) as varchar)  else 
            //cast(Display as varchar(255))   end  END AS Display  , TaxonNameCache AS OrderBy 
            //FROM TaxonName_Indicated AS T 
            //WHERE 1 = 1 
            //AND (NameID IN (SELECT NameID FROM TaxonNameProject AS T WHERE T.[ProjectID] = 54) AND T.[ProjectID] = 54) 
            //AND NameID IN (SELECT [NameID] FROM [TaxonAcceptedName]  WHERE NameID IN (SELECT [NameID] FROM [TaxonAcceptedName]))  
            //AND T.IgnoreButKeepForReference = 0   
            //ORDER BY TaxonNameCache ASC, NameID

            string OptimizedQuery = Query;
            try
            {
                string StartOfFromClause = " WHERE 1 = 1 AND ";
                if (OptimizedQuery.IndexOf(StartOfFromClause) > -1)
                {
                    string SelectClause = OptimizedQuery.Substring(0, OptimizedQuery.IndexOf(StartOfFromClause)); ;
                    string FromClause = SelectClause.Substring(SelectClause.LastIndexOf(" FROM "));
                    SelectClause = SelectClause.Substring(0, SelectClause.LastIndexOf(" FROM "));
                    string WhereClause = OptimizedQuery.Substring(OptimizedQuery.IndexOf(StartOfFromClause));
                    System.Collections.Generic.Dictionary<int, string> WhereClauses = new Dictionary<int, string>();
                    WhereClauses.Add(1, WhereClause.Substring(0, WhereClause.IndexOf(" AND ")));
                    WhereClause = WhereClause.Substring(WhereClause.IndexOf(" AND "));
                    string OrderClause = WhereClause.Substring(WhereClause.IndexOf(" ORDER BY "));
                    WhereClause = WhereClause.Substring(0, WhereClause.IndexOf(" ORDER BY "));
                    while (WhereClause.IndexOf(" AND ") > -1)
                    {
                        int iPosAnd = 0;
                        int BracketCounter = 0;
                        string WhereRest = WhereClause;
                        for (int i = 0; i < WhereClause.Length; i++)
                        {
                            if (WhereClause[i] == '(')
                                BracketCounter++;
                            if (WhereClause[i] == ')')
                                BracketCounter--;
                            if (BracketCounter == 0 && WhereClause.Substring(i).StartsWith(" AND ") && i > 0)
                            {
                                WhereRest = WhereClause.Substring(i);
                                string WhereCurrent = WhereClause.Substring(0, i);
                                WhereClauses.Add(WhereClauses.Count + 1, WhereCurrent);
                                WhereClause = WhereClause.Substring(i);
                                break;
                            }
                            if (i == WhereClause.Length - 1)
                            {
                                WhereClauses.Add(WhereClauses.Count + 1, WhereClause);
                                WhereClause = "";
                            }
                        }
                    }
                    foreach (System.Collections.Generic.KeyValuePair<int, string> KV in WhereClauses)
                    {
                        string Where = KV.Value;
                        string Table = "T" + KV.Key.ToString();
                        if (Where.StartsWith(" AND "))
                        {
                            ///TODO: Zerlegen und aufloesen der Teilabfragen + Korrektur des Selects falls Spalten in mehreren Tabellen auftauchen

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                OptimizedQuery = Query;
            }
            return OptimizedQuery;
        }

        private void buttonOptimize_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Settings.QueryOptimizedByDefault && DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing)
                DiversityWorkbench.Settings.QueryOptimizedByDefault = false;
            this.Optimizing_SetUsage(!DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing);
        }

        private void Optimizing_SetUsage(bool UseOptimizing)
        {
            if (DiversityWorkbench.Settings.QueryOptimizedByDefault)
            {
                DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing = true;
            }
            else
            {
                DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing = UseOptimizing;
                if (DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing)
                    this.buttonOptimize.BackColor = System.Drawing.Color.Yellow;
                else this.buttonOptimize.BackColor = System.Drawing.SystemColors.Control;
                this.Optimizing_UsedForQueryList = UseOptimizing;

                if (this.ManyOrderByColumns_Allow())// && UseOptimizing)
                {
                    this.ManyOrderByColumns_SetControls(); // dritter aufruf queryDatabase Ariane
                }
            }
//#if DEBUG
//            if (this.ManyOrderByColumns_Allow())// && UseOptimizing)
//            {
//                this.ManyOrderByColumns_SetControls(); // vierter aufruf im debug mode Ariane
//            }
//#endif
            // Auf Formular umgestellt um es fuer alle Formulare zu ermöglichen
            //this.initManyOrderColumns(DiversityWorkbench.Settings.ModuleName);
            //this.setManyOrderByColumnControls();
        }

        private bool _Optimizing_AllowedForQueryList = false;
        private bool _Optimizing_UsedForQueryList = false;
        /// <summary>
        /// If Optimizing is allowed - Button with runner visible
        /// </summary>
        /// <param name="IsAllowed">True = Do allow optimizing</param>
        public void OptimizingAllow(bool IsAllowed)
        {
            if (DiversityWorkbench.Settings.QueryOptimizedByDefault)
            {
                _Optimizing_AllowedForQueryList = true;
                this.buttonOptimize.Visible = false;
            }
            else
            {
                _Optimizing_AllowedForQueryList = IsAllowed;
                this.buttonOptimize.Visible = IsAllowed;
                if (!IsAllowed)
                    this.Optimizing_SetUsage(false);
            }
        }


        public void AllowOptimizing(bool Allow) { this.OptimizingAllow(Allow); }

        public bool Optimizing_AllowedForQueryList { get { return _Optimizing_AllowedForQueryList; } }
        public bool Optimizing_UsedForQueryList
        {
            get
            {
                if (DiversityWorkbench.Settings.QueryOptimizedByDefault)
                    return true;
                return _Optimizing_UsedForQueryList;
            }
            set
            {
                bool QueryOptimizedByDefault = false;
                if (bool.TryParse( DiversityWorkbench.Settings.QueryOptimizedByDefault.ToString(), out QueryOptimizedByDefault) && QueryOptimizedByDefault)
                    _Optimizing_UsedForQueryList = true;
                else
                {
                    if (Optimizing_AllowedForQueryList)
                    {
                        _Optimizing_UsedForQueryList = value;
                        //DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing = value;
                    }
                    if (!value && this._ManyOrderByColumns_Allow)
                    {
                        this.ManyOrderByColumns_Reset();
                    }
                }
            }
        }

        /// <summary>
        /// If the optimizing is available. Hide or shown the button for activation of the optimizing
        /// </summary>
        /// <param name="IsAvailable"></param>
        public void OptimizingAvailable(bool IsAvailable)
        {
            if (DiversityWorkbench.Settings.QueryOptimizedByDefault)
            {
                this.buttonOptimize.Visible = false;
                this.OptimizingAllow(true);
                this.Optimizing_SetUsage(true);
            }
            else
            {
                this.buttonOptimize.Visible = IsAvailable;
                this._Optimizing_AllowedForQueryList = IsAvailable;
                this.Optimizing_SetUsage(this.OptimizingIsUsed());
            }
        }

        //public void UseOptimizing(bool DoUseOptimizing)
        //{
        //    if (DoUseOptimizing)
        //    {
        //        this.buttonOptimize.BackColor = System.Drawing.Color.Yellow;
        //        DiversityWorkbench.UserControls.UserControlQueryList._UseOptimizing = true;
        //        this.buttonQueryLoad.Enabled = !DiversityWorkbench.UserControls.UserControlQueryList._UseOptimizing;
        //        this.buttonQuerySave.Enabled = !DiversityWorkbench.UserControls.UserControlQueryList._UseOptimizing;
        //        this.comboBoxQueryColumn.Enabled = !DiversityWorkbench.UserControls.UserControlQueryList._UseOptimizing;
        //    }
        //}

        public bool OptimizingIsUsed()
        {
            return DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing;
        }

        public string OptimizedWhereClause() { return this.OptimizedQueryStringWhereClause(); }

        /// <summary>
        /// Getting the where clause based on the interface IUserControlQueryCondition for optimized queries
        /// </summary>
        /// <returns></returns>
        private string OptimizedQueryStringWhereClause()
        {
            string SQL = "";

            if (this.comboBoxQueryColumn.Text.Length > 0)
            {
                // Getting the default names for Table etc.
                if (this.QueryMainTableLocal != null && UserControlQueryList.QueryMainTable != this.QueryMainTableLocal)
                    UserControlQueryList.QueryMainTable = this.QueryMainTableLocal;
                foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                {
                    if (C.DisplayColumn == this.SelectedDisplayColumn)
                    {
                        if (QueryMainTable.Length == 0 || (QueryMainTable != C.TableName && C.TableName.Length > 0))
                            QueryMainTable = C.TableName;
                        if (_IdentityColumnOptimizing == null || _IdentityColumnOptimizing != C.IdentityColumn)
                            _IdentityColumnOptimizing = C.IdentityColumn;
                        if (_DisplayColumnOptimizing == null)
                            _DisplayColumnOptimizing = C.DisplayColumn;
                        break;
                    }
                }

                #region Optimizing the query for better performance

                try
                {
                    // get the tables and the QueryConditions
                    this.OptimizedQueryAnalyseTables();

                    SQL = this.OptimizedQueryFromClause();

                    // Markus 12.8.24 #1:
                    // Bei e.g. Tabellen wie Identification in Kombination ohne Suchfeld in IdentificationUnit in DC wird Tabelle IdentificationUnit in die For Clausel genommen,
                    // aber nicht in die Where Klausel. Das führt zu Cross Joins
                    // Daher muss das in der Where Clause überprüft und evtl. nachgezogen werden
                    // Alternativ könnte man die nicht eingebundene Tabelle in der Funktion OptimizedQueryAnalyseTables ausschliessen
                    System.Collections.Generic.Dictionary<string, string> MissingJoins = new Dictionary<string, string>(); 
                    System.Collections.Generic.Dictionary<string, string> CurrentTables = new Dictionary<string, string>();
                    try
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> kv in UserControlQueryList.TableAliases)
                        {
                            MissingJoins.Add(kv.Key, kv.Value);
                            CurrentTables.Add(kv.Key, kv.Value);
                        }
                        MissingJoins.Add(UserControlQueryList.QueryMainTable, "T");
                        CurrentTables.Add(UserControlQueryList.QueryMainTable, "T");
                    }
                    catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

                    // Adding the Where clause
                    string WhereClause = "";
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition>> KV in UserControlQueryList.TableQueryConditions)
                    {
                        foreach (DiversityWorkbench.IUserControlQueryCondition QC in KV.Value)
                        {
                            try 
                            {
                                if (QC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                                {
                                    if (this._FreeQueryControls.Contains((DiversityWorkbench.UserControls.UserControlQueryCondition)QC))
                                    {
                                        continue;
                                    }
                                }
                                if (QC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation))
                                {
                                    if (this._AnnotationControls.Contains((DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation)QC))
                                    {
                                        continue;
                                    }
                                }
                                if (QC.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable))
                                {
                                    if (this._ReferencingTableControls.Contains((DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable)QC))
                                    {
                                        continue;
                                    }
                                }

                                bool IsAddedSetControl = false;
                                if (UserControlQueryList.QueryConditionSets.ContainsKey(QC.Condition().Table) &&
                                    UserControlQueryList.QueryConditionSets[QC.Condition().Table].ContainsKey(QC.Condition().Column))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryConditionSet S = (DiversityWorkbench.UserControls.UserControlQueryConditionSet)QC;
                                    if (UserControlQueryList.QueryConditionSets[QC.Condition().Table][QC.Condition().Column][0] != S &&
                                        UserControlQueryList.QueryConditionSetOperators.ContainsKey(DiversityWorkbench.Settings.ModuleName) &&
                                        UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName].ContainsKey(QC.getCondition().Table) &&
                                        UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][QC.getCondition().Table].ContainsKey(QC.getCondition().Column) &&
                                        UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][QC.Condition().Table][QC.Condition().Column] == QueryOperator.OR)
                                    {
                                        IsAddedSetControl = true;
                                    }
                                }

                                if (WhereClause.Length > 0 &&
                                    !IsAddedSetControl)
                                {
                                    // Markus 11.6.24: Include in inner Query
                                    //if(WhereClause.IndexOf(" EXISTS ") > -1 && WhereClause.EndsWith(")"))
                                    //{
                                    //    string newWhereClause = WhereClause.Substring(0, WhereClause.Length - 1);
                                    //    newWhereClause += " AND ";
                                    //    WhereClause = newWhereClause;
                                    //}
                                    //else
                                    WhereClause += " AND ";
                                }
                                else if (IsAddedSetControl)
                                    continue;

                                if (KV.Key == UserControlQueryList.QueryMainTable)
                                {
                                    string Q = QC.WhereClause();
                                    if (UserControlQueryList.TableAliases.ContainsKey(KV.Key))
                                        Q = Q.Replace(UserControlQueryList.TableAliases[KV.Key] + ".", "T.");
                                    else if (!Q.StartsWith("T.[") &&
                                        !Q.StartsWith("(T.") &&
                                        !QC.Condition().IsDate &&
                                        QC.Condition().Operator != "|" &&
                                        !Q.Trim().StartsWith("(") &&
                                        !Q.Trim().StartsWith("T." + QC.Condition().Column + " "))
                                        Q = " T." + QC.Condition().Column + " " + Q;
                                    if (WhereClause.IndexOf(Q) == -1)
                                        WhereClause += Q;
                                    else if (WhereClause.EndsWith(" AND "))
                                    {
                                        WhereClause = WhereClause.Substring(0, WhereClause.Length - 5);
                                    }
                                }
                                else
                                {
                                    string QCWhere = QC.WhereClause();
                                    string QCSql = QC.SqlByIndex(i);
                                    if (QC.Condition().QueryFields != null && QC.Condition().QueryFields.Count > 1)
                                    {
                                        WhereClause = QC.WhereClause();
                                        // Markus 16.3.2023: No reference to other tables
                                        if (ManyOrderByColumns())
                                        {
                                            foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> oderBy in this.ManyOrderByColumns_Controls)
                                            {
                                                foreach (DiversityWorkbench.QueryField QF in QC.Condition().QueryFields)
                                                {
                                                    if (QF.IdentityColumn == oderBy.Value.QueryOrderColumn.IdentityColumn &&
                                                        QF.TableName != oderBy.Value.QueryOrderColumn.TableName)
                                                    {
                                                        if (DiversityWorkbench.UserControls.UserControlQueryList.TableAliases.ContainsKey(QF.TableName) &&
                                                        ManyOrderByColumns_TableAliases(oderBy.Value.QueryOrderColumn.TableName).ContainsKey(oderBy.Value.QueryOrderColumn.TableName))
                                                        {
                                                            if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            WhereClause += DiversityWorkbench.UserControls.UserControlQueryList.TableAliases[QF.TableName] + "." + QF.IdentityColumn + " = ";
                                                            WhereClause += ManyOrderByColumns_TableAliases(oderBy.Value.QueryOrderColumn.TableName)[oderBy.Value.QueryOrderColumn.TableName];
                                                            WhereClause += "." + oderBy.Value.QueryOrderColumn.IdentityColumn;
                                                        }
                                                        else
                                                        {
                                                            if (!DiversityWorkbench.UserControls.UserControlQueryList.TableAliases.ContainsKey(QF.TableName) &&
                                                                !ManyOrderByColumns_TableAliases(oderBy.Value.QueryOrderColumn.TableName).ContainsKey(QF.TableName)
                                                                && QF.TableName != UserControlQueryList.QueryMainTable)
                                                            {
                                                                this.OptimizedQueryAnalyseTables();
                                                            }
                                                            //if (WhereClause.Length > 0) WhereClause += " AND ";
                                                            //WhereClause += DiversityWorkbench.UserControls.UserControlQueryList.TableAliases[QF.TableName] + "." + QF.IdentityColumn + " = ";
                                                            //WhereClause += ManyOrderByColumns_TableAliases(oderBy.Value.QueryOrderColumn.TableName)[oderBy.Value.QueryOrderColumn.TableName];
                                                            //WhereClause += "." + oderBy.Value.QueryOrderColumn.IdentityColumn;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (UserControlQueryList.QueryMainTable != this.QueryMainTableLocal && this.QueryMainTableLocal != null) // Markus 8.4.24: Bugfix wenn QueryMainTableLocal null 
                                        {
                                            foreach (DiversityWorkbench.QueryField QF in QC.Condition().QueryFields)
                                            {
                                                if (WhereClause.IndexOf(QF.IdentityColumn) == -1 && QCSql != null && QCSql.Length > 0 && this.QueryMainTableLocal != null) // Markus 8.4.24: Bugfix wenn QueryMainTableLocal null 
                                                {
                                                    if (WhereClause.Length > 0 && !WhereClause.EndsWith(" AND "))
                                                        WhereClause += " AND ";
                                                    if (QF.IdentityColumn == _IdentityColumnOptimizing
                                                        && UserControlQueryList.TableAliases.ContainsKey(QF.TableName)
                                                        && QF.TableName != this.QueryMainTableLocal)
                                                    {
                                                        if (this.QueryMainTableLocal != null) // Markus 8.4.24: Bugfix wenn QueryMainTableLocal null 
                                                        {
                                                            if (ManyOrderByColumns_TableAliases().ContainsKey(this.QueryMainTableLocal))
                                                            {
                                                                WhereClause += ManyOrderByColumns_TableAliases()[this.QueryMainTableLocal] + "." + _IdentityColumnOptimizing + " = " + UserControlQueryList.TableAliases[QF.TableName] + "." + _IdentityColumnOptimizing;
                                                            }
                                                            else if (UserControlQueryList.TableAliases.ContainsKey(this.QueryMainTableLocal))
                                                            {
                                                                WhereClause += UserControlQueryList.TableAliases[this.QueryMainTableLocal] + "." + _IdentityColumnOptimizing + " = " + UserControlQueryList.TableAliases[QF.TableName] + "." + _IdentityColumnOptimizing;
                                                            }
                                                        }
                                                    }
                                                    //WhereClause += QCSql;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (DiversityWorkbench.QueryField QF in QC.Condition().QueryFields)
                                            {
                                                if (WhereClause.IndexOf(QF.IdentityColumn) == -1 && QCSql != null && QCSql.Length > 0 && QF.TableName != this.QueryMainTableLocal)
                                                {
                                                    if (WhereClause.Length > 0 && !WhereClause.EndsWith(" AND "))
                                                        WhereClause += " AND ";
                                                    if (QF.IdentityColumn == _IdentityColumnOptimizing
                                                        && UserControlQueryList.TableAliases.ContainsKey(QF.TableName)
                                                        && QF.TableName != this.QueryMainTableLocal)
                                                    {
                                                        if (ManyOrderByColumns_TableAliases().ContainsKey(this.QueryMainTableLocal))
                                                        {
                                                            WhereClause += ManyOrderByColumns_TableAliases()[this.QueryMainTableLocal] + "." + _IdentityColumnOptimizing + " = " + UserControlQueryList.TableAliases[QF.TableName] + "." + _IdentityColumnOptimizing;
                                                        }
                                                        else if (UserControlQueryList.TableAliases.ContainsKey(this.QueryMainTableLocal))
                                                        {
                                                            WhereClause += UserControlQueryList.TableAliases[this.QueryMainTableLocal] + "." + _IdentityColumnOptimizing + " = " + UserControlQueryList.TableAliases[QF.TableName] + "." + _IdentityColumnOptimizing;
                                                        }
                                                        else if (UserControlQueryList.TableAliases.ContainsKey(QF.TableName) && UserControlQueryList.QueryMainTable == this.QueryMainTableLocal)
                                                        {
                                                            WhereClause += "T." + _IdentityColumnOptimizing + " = " + UserControlQueryList.TableAliases[QF.TableName] + "." + _IdentityColumnOptimizing;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (QCSql.IndexOf(" AND ") > -1)
                                        WhereClause += " T." + _IdentityColumnOptimizing + " " + QCSql;
                                    else
                                    {
                                        if (QC.Condition().OptimizingLinkColumns != null)
                                        {
                                            System.Collections.Generic.List<string> TableList = new List<string>();
                                            foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, string>> KVopt in QC.Condition().OptimizingLinkColumns)
                                            {
                                                TableList.Add(KVopt.Key);
                                                if (!UserControlQueryList.TableAliases.ContainsKey(KVopt.Key) && KVopt.Key != UserControlQueryList.QueryMainTable)
                                                {
                                                    UserControlQueryList.TableAliases.Add(KVopt.Key, "T" + UserControlQueryList.TableAliases.Count.ToString());
                                                    AliasCheckAdd(KVopt.Key + " AS T" + UserControlQueryList.TableAliases.Count.ToString());
                                                    SQL += ", " + KVopt.Key + " AS " + UserControlQueryList.TableAliases[KVopt.Key];
                                                }
                                            }
                                            for (int iTab = 0; iTab < TableList.Count; iTab++)
                                            {
                                                foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in QC.Condition().OptimizingLinkColumns[TableList[iTab]])
                                                {
                                                    string t = TableList[iTab];
                                                    System.Collections.Generic.Dictionary<string, string> cc = QC.Condition().OptimizingLinkColumns[TableList[iTab]];
                                                    string Alias = "";
                                                    if (TableList[iTab] == UserControlQueryList.QueryMainTable)
                                                        Alias = "T";
                                                    else
                                                        Alias = UserControlQueryList.TableAliases[TableList[iTab]];
                                                    if (iTab == 0)
                                                    {
                                                        string x = UserControlQueryList.TableAliases[QC.Condition().Table] + "." + KVcol.Key;
                                                        x += " = " + Alias + "." + KVcol.Value;
                                                        if (WhereClause.Length > 0)
                                                            WhereClause += " AND ";
                                                        WhereClause += " " + x + " ";
                                                    }
                                                    else
                                                    {
                                                        string x = UserControlQueryList.TableAliases[TableList[iTab - 1]] + "." + KVcol.Key;
                                                        x += " = " + Alias + "." + KVcol.Value;
                                                        if (WhereClause.Length > 0)
                                                            WhereClause += " AND ";
                                                        WhereClause += " " + x + " ";
                                                    }
                                                }
                                            }
                                            WhereClause += " AND " + QCWhere;
                                        }
                                        else
                                        {
                                            if (QC.Condition().ForeignKey != null
                                                && QC.Condition().ForeignKey.Length > 0
                                                && QC.Condition().ForeignKey != _IdentityColumnOptimizing
                                                && QC.Condition().Table != null
                                                && QC.Condition().Table == QueryMainTable)
                                                WhereClause += " T." + QC.Condition().ForeignKey + " " + QCSql;
                                            else
                                            {
                                                if (QC.Condition().SqlFromClausePostfix == null)
                                                {
                                                    if (QC.Condition().ForeignKey != null && QC.Condition().ForeignKey.Length > 0 && QCWhere.Length > 0)
                                                    {
                                                        bool ConditionFound = false;
                                                        if (QC.getCondition().IsSet)
                                                        {
                                                            DiversityWorkbench.UserControls.UserControlQueryConditionSet QS = (DiversityWorkbench.UserControls.UserControlQueryConditionSet)QC;
                                                            if (DiversityWorkbench.UserControls.UserControlQueryList.QueryConditionSetTableAliases.ContainsKey(QS))
                                                            {
                                                                WhereClause += " T." + QC.Condition().ForeignKey + " = " + UserControlQueryList.QueryConditionSetTableAliases[QS] + "." + QC.Condition().ForeignKey;
                                                                ConditionFound = true;
                                                            }
                                                        }
                                                        bool IsNotExists = false;
                                                        if (UserControlQueryList.TableAliasesNotExists.ContainsKey(QC.Condition().Table))
                                                        {
                                                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.QueryCondition> KVnotexists in UserControlQueryList.TableAliasesNotExists[QC.Condition().Table])
                                                            {
                                                                if (KVnotexists.Value.Restriction == QC.Condition().Restriction)
                                                                {
                                                                    IsNotExists = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (!IsNotExists && !ConditionFound && UserControlQueryList.TableAliases.ContainsKey(QC.Condition().Table))
                                                        {
                                                            string Where = "";
                                                            if (QC.Condition().IntermediateTable != null && QC.Condition().IntermediateTable.Length > 0)
                                                            {
                                                                string IntermediateTable = QC.Condition().IntermediateTable;
                                                                string TableAlias = UserControlQueryList.TableAliases[QC.Condition().Table];
                                                                string ForeignKey = QC.Condition().ForeignKey;
                                                                string IdentityColumn = QC.Condition().IdentityColumn;
                                                                if (ForeignKey == IdentityColumn && UserControlQueryList.IdentityColumnOptimizing == IdentityColumn)
                                                                    Where = " T." + IdentityColumn + " = " + TableAlias + "." + QC.Condition().IdentityColumn;
                                                                else
                                                                {
                                                                    if (!UserControlQueryList.TableAliases.ContainsKey(QC.Condition().IntermediateTable))
                                                                    {
                                                                        UserControlQueryList.TableAliases.Add(QC.Condition().IntermediateTable, "T" + UserControlQueryList.TableAliases.Count.ToString());
                                                                        AliasCheckAdd(QC.Condition().IntermediateTable + " AS T" + UserControlQueryList.TableAliases.Count.ToString());
                                                                    }
                                                                    string TableAliasIntermediate = UserControlQueryList.TableAliases[QC.Condition().IntermediateTable];
                                                                    Where = " T." + QC.Condition().IdentityColumn + " = " + UserControlQueryList.TableAliases[QC.Condition().IntermediateTable] + "." + QC.Condition().IdentityColumn +
                                                                        " AND " + UserControlQueryList.TableAliases[QC.Condition().Table] + "." + QC.Condition().ForeignKey + " = " + UserControlQueryList.TableAliases[QC.Condition().IntermediateTable] + "." + QC.Condition().ForeignKey;
                                                                }

                                                            }
                                                            else
                                                            {
                                                                if (this.ManyOrderByColumns())
                                                                {
                                                                    string alias = "T";
                                                                    string ConditionTable = QC.Condition().Table;
                                                                    if (this._QueryMainTableLocal != UserControlQueryList.QueryMainTable)
                                                                    {
                                                                        // change alias to ensure join
                                                                        if (this._QueryMainTableLocal != null)
                                                                            alias = ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal];
                                                                        else if (ManyOrderByColumns_TableAliases().ContainsKey(UserControlQueryList.QueryMainTable))
                                                                            alias = ManyOrderByColumns_TableAliases()[UserControlQueryList.QueryMainTable];
                                                                    }
                                                                    string AliasMain = UserControlQueryList.TableAliases[QC.Condition().Table];
                                                                    // return to T if same alias would be used
                                                                    if (AliasMain == alias)
                                                                    {
                                                                        if (ManyOrderByColumns_TableAliases().ContainsKey(UserControlQueryList.QueryMainTable) && ManyOrderByColumns_TableAliases()[UserControlQueryList.QueryMainTable] != alias)
                                                                            AliasMain = ManyOrderByColumns_TableAliases()[UserControlQueryList.QueryMainTable];
                                                                        else if (ManyOrderByColumns_TableAliases().ContainsKey(this._QueryMainTableLocal) && ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal] != alias)
                                                                            AliasMain = ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal];
                                                                        else AliasMain = "T";
                                                                    }
                                                                    Where = " " + alias + "." + QC.Condition().ForeignKey + " = " + AliasMain + "." + QC.Condition().ForeignKey;
                                                                }
                                                                else
                                                                {
                                                                    Where = " T.[" + QC.Condition().ForeignKey + "] = " + UserControlQueryList.TableAliases[QC.Condition().Table] + ".[" + QC.Condition().ForeignKey + "]";
                                                                    // Markus 27.3.23: Try to link to main table if possible
                                                                    System.Collections.Generic.List<string> PKs = this.IncludedPK(QC, Where);
                                                                    if (PKs.Count > 0)
                                                                    {
                                                                        foreach (string PK in PKs)
                                                                        {
                                                                            if (Where.IndexOf("[" + PK + "]") == -1)
                                                                            {
                                                                                Where = " T.[" + PK + "] = " + UserControlQueryList.TableAliases[QC.Condition().Table] + ".[" + PK + "] AND " + Where;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            WhereClause += Where;
                                                        }
                                                    }
                                                    else if (QCSql.Length > 0 || QC.getCondition().Operator != "Ø")
                                                        WhereClause += " T." + _IdentityColumnOptimizing + " " + QCSql;
                                                    else if (QC.Condition().ForeignKey.Length > 0 && QC.getCondition().Operator == "Ø")
                                                    {
                                                        WhereClause += " T." + QC.Condition().ForeignKey + " = " + UserControlQueryList.TableAliases[QC.Condition().Table] + "." + QC.Condition().ForeignKey;
                                                    }
                                                }
                                                else
                                                    WhereClause += " T." + _IdentityColumnOptimizing + " IN (SELECT " + _IdentityColumnOptimizing + QC.Condition().SqlFromClause + QCWhere + QC.Condition().SqlFromClausePostfix + ")";
                                            }
                                            if (QC.getCondition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                                            {
                                                if (QC.getCondition().Operator == "Ø")
                                                {
                                                    WhereClause += QCWhere;
                                                }
                                                else if (QC.Condition().QueryType == QueryCondition.QueryTypes.Count && QC.Condition().Value.Length > 0)
                                                {
                                                    WhereClause += QCWhere;
                                                }
                                                else if (WhereClause.IndexOf(" = ") > -1
                                                    && QC.getCondition().Operator == "•"
                                                    && QC.getCondition().ForeignKey != QC.getCondition().IdentityColumn
                                                    && QCWhere.Trim().StartsWith("="))
                                                {
                                                    WhereClause += " AND T." + QC.Condition().ForeignKey + QCWhere;
                                                }
                                                else if (!WhereClause.EndsWith(" = " + UserControlQueryList.TableAliases[KV.Key] + "." + _IdentityColumnOptimizing))
                                                {
                                                    // Markus 27.3.23 : bugfix for links to main table
                                                    string IdentityClause = " = " + UserControlQueryList.TableAliases[KV.Key] + ".[" + _IdentityColumnOptimizing + "]";
                                                    if (!WhereClause.Trim().EndsWith(IdentityClause))
                                                        WhereClause += " = " + UserControlQueryList.TableAliases[KV.Key] + ".[" + _IdentityColumnOptimizing + "]";
                                                }
                                                else if (QC.Condition().QueryType == QueryCondition.QueryTypes.Count)
                                                {
                                                    // MW 29.10.2018: new type count
                                                    WhereClause = " T." + QC.Condition().IdentityColumn + QCWhere;
                                                }
                                            }
                                            else if (QC.Condition().ForeignKey != null && QC.Condition().ForeignKey.Length > 0)
                                            {
                                                if (QC.Condition().ForeignKey == QC.Condition().IdentityColumn)
                                                {
                                                    //WhereClause += " = " + UserControlQueryList.TableAliases[KV.Key] + "." + QC.Condition().ForeignKey;
                                                }
                                                else if (QC.Condition().OptimizingLinkColumns != null && QC.Condition().OptimizingLinkColumns.Count > 0)
                                                {
                                                }
                                                WhereClause += " AND ";
                                                if (UserControlQueryList.QueryConditionSets.ContainsKey(QC.Condition().Table) &&
                                                    UserControlQueryList.QueryConditionSets[QC.Condition().Table].ContainsKey(QC.Condition().Column))
                                                {
                                                    DiversityWorkbench.UserControls.UserControlQueryConditionSet S = (DiversityWorkbench.UserControls.UserControlQueryConditionSet)QC;
                                                    if (DiversityWorkbench.UserControls.UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName].ContainsKey(QC.getCondition().Table) &&
                                                        DiversityWorkbench.UserControls.UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][QC.getCondition().Table].ContainsKey(QC.getCondition().Column) &&
                                                        DiversityWorkbench.UserControls.UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][QC.getCondition().Table][QC.getCondition().Column] == QueryOperator.OR)
                                                    {
                                                        if (UserControlQueryList.QueryConditionSets[QC.Condition().Table][QC.Condition().Column][0] == S)
                                                        {
                                                            WhereClause += UserControlQueryList.TableAliases[QC.Condition().Table] + "." + QC.Condition().Column;
                                                            WhereClause += " IN (";
                                                            string CC = "";
                                                            foreach (DiversityWorkbench.UserControls.UserControlQueryConditionSet US in UserControlQueryList.QueryConditionSets[QC.Condition().Table][QC.Condition().Column])
                                                            {
                                                                string Restriction = US.WhereClause();
                                                                if (Restriction.Length > 0) /// Markus 23.5.2016 - nur wenn was gewaehlt wurde beruecksichtigen, keine leeren Werte
                                                                {
                                                                    if (CC.Length > 0) CC += ", ";
                                                                    CC += US.WhereClause();
                                                                }
                                                            }
                                                            WhereClause += CC;
                                                            WhereClause += ")";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        WhereClause += S.WhereClause();
                                                    }
                                                }
                                                else
                                                    WhereClause += QCWhere;
                                            }
                                            else if (QC.Condition().SqlFromClausePostfix == null)// && KV.Key != "Annotation") // Markus 15.2.2018 - wird fuer Annotation unten nochmal gemacht
                                            {
                                                WhereClause += " = " + UserControlQueryList.TableAliases[KV.Key] + "." + _IdentityColumnOptimizing;
                                                WhereClause += " AND " + QCWhere;
                                            }
                                        }
                                    }
                                }
                                i++;
                            }
                            catch (Exception e) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(e); }
                        }


                        // Markus 12.8.24 #1
                        string table = KV.Key;
                        if (MissingJoins.ContainsKey(table)) 
                        { 
                            //string Key = MissingJoins.FirstOrDefault(c => c.Value == table).Key;
                            // If the table is present, it can be removed from the missing joins
                            MissingJoins.Remove(table);
                        }

                    }
                    // Markus 12.8.24 #1
                    if (MissingJoins.Count > 0)
                    {
                        // There are tables in the from clause that had not been taken into the where clause
                        try
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in MissingJoins)
                            {
                                if (WhereClause.IndexOf(" " + kvp.Value + ".") > -1)
                                {
                                    continue;
                                }
                                if (kvp.Value != null)
                                {
                                    string Parent = kvp.Key;
                                    if (Parent.IndexOf("_") > -1)
                                    {
                                        Parent = Parent.Substring(0, Parent.IndexOf("_"));
                                    }
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> currentTable in CurrentTables)
                                    {
                                        if (kvp.Value == currentTable.Value)
                                            continue;
                                        string Child = currentTable.Key;
                                        if (Child.IndexOf("_") > -1)
                                        {
                                            // Get base table of View
                                            Child = Child.Substring(0, Child.IndexOf("_"));
                                        }
                                        // Get links to other tables
                                        string SqlLinks = "SELECT DISTINCT " +
                                            "cr.name AS ReferencedColumn " +
                                            "FROM " +
                                            "sys.foreign_keys AS fk " +
                                            "INNER JOIN  " +
                                            "sys.tables AS tp ON fk.parent_object_id = tp.object_id " +
                                            "INNER JOIN " +
                                            "sys.tables AS tr ON fk.referenced_object_id = tr.object_id " +
                                            "INNER JOIN  " +
                                            "sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id " +
                                            "INNER JOIN  " +
                                            "sys.columns AS cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id " +
                                            "INNER JOIN  " +
                                            "sys.columns AS cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id " +
                                            "WHERE  " +
                                            "(tr.name = '" + Parent + "' AND tp.name = '" + Child + "') " +
                                            "OR (tp.name = '" + Parent + "' AND tr.name = '" + Child + "');";
                                        System.Data.DataTable dtCol = DiversityWorkbench.Forms.FormFunctions.DataTable(SqlLinks);
                                        string Join = "";
                                        foreach (System.Data.DataRow dataRow in dtCol.Rows)
                                        {
                                            if (Join.Length > 0)
                                                Join += " AND ";
                                            Join += kvp.Value + "." + dataRow[0].ToString() + " = " + currentTable.Value + "." + dataRow[0].ToString();
                                        }
                                        if (!WhereClause.Contains(Join))
                                        {
                                            WhereClause += " AND (" + Join + ") ";
                                        }
                                    }
                                }
                            }
                        }
                        catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }

                    SQL += " WHERE " + WhereClause;
                    if (this._AnnotationControls.Count > 0)
                    {
                        string AnnotationQueryString = this.OptimizedQueryAnnotations();
                        SQL += AnnotationQueryString;
                    }
                    if (this._ReferencingTableControls.Count > 0)
                    {
                        SQL += this.OptimizedQueryReferencingTables();
                    }
                    if (this._ModuleControls.Count > 0)
                    {
                        SQL += this.OptimziedQueryModules();
                    }
                    if (this._FreeQueryControls.Count > 0)
                    {
                        SQL += this.OptimizedQueryFreeQuery(i);
                    }
                    if (ManyOrderByColumns())
                    {
                        //if (WhereClause.Length > 0) SQL += " AND ";
                        //SQL += this.ManyOrderByColumns_WhereClause();
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }

                #endregion

            }
            this.SqlWhereClause = SQL;
            if (this.QueryRestriction.Length > 0)
            {
                if (UseOptimizing && this.SqlWhereClause.Trim().EndsWith(" WHERE"))
                    SQL += " 1 = 1";
                SQL += " " + this.QueryRestriction + " ";
            }
            if (this.getQueryRestrictionList().Count > 0)
            {
                SQL += this.OptimizedQueryRestrictions();
            }
            if (this._GetNext || this._GetPrevious)
            {
                SQL += this._GetNextRestriction;
            }

            // Bugfixing
            if (SQL.IndexOf("[[") > -1)
                SQL = SQL.Replace("[[", "[");
            if (SQL.Trim().EndsWith(" WHERE"))
                SQL = SQL.Trim().Substring(0, SQL.Trim().Length - 6);
            if (SQL.IndexOf(" AND  AND ") > -1)
                SQL = SQL.Replace(" AND  AND ", " AND ");
            if (SQL.IndexOf(" WHERE  AND ") > -1)
                SQL = SQL.Replace(" WHERE  AND ", " WHERE ");

            return SQL;
        }

        private System.Collections.Generic.List<string> IncludedPK(DiversityWorkbench.IUserControlQueryCondition QC, string Where)
        {
            System.Collections.Generic.List<string> PKlist = new List<string>();
            try
            {
                bool IncludePK = false;
                DiversityWorkbench.Data.Table table = new Data.Table(QC.Condition().Table);
                DiversityWorkbench.Data.Table maintable = new Data.Table(UserControlQueryList.QueryMainTable);

                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Data.Table.TableRelation> TableRelations = table.RelatedTables();
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Data.Table.TableRelation> TableParents = table.ParentTables();
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Data.Table.TableRelation> MainTableRelations = maintable.RelatedTables();
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Data.Table.TableRelation> MainTableParents = maintable.ParentTables();


                // Markus 24.5.23: Check if there is a relationship between the tables - same name of colums do not ensure any relationship
                if (TableRelations.ContainsKey(maintable.Name)
                    || TableParents.ContainsKey(maintable.Name)
                    || MainTableRelations.ContainsKey(table.Name)
                    || MainTableParents.ContainsKey(table.Name))
                {
                    foreach (string C in maintable.PrimaryKeyColumnList)
                    {
                        if (table.PrimaryKeyColumnList.Contains(C) && !PKlist.Contains(C))
                            PKlist.Add(C);
                    }
                    //IncludePK = true;
                }
                else
                {
                    if (maintable.ViewTables.Count > 0)
                    {
                        foreach (string Table in maintable.ViewTables)
                        {
                            DiversityWorkbench.Data.Table ViewTable = new Data.Table(Table);
                            if (TableRelations.ContainsKey(Table) ||
                            TableParents.ContainsKey(Table))
                            {
                                foreach (string C in table.PrimaryKeyColumnList)
                                {
                                    if (ViewTable.PrimaryKeyColumnList.Contains(C) && !PKlist.Contains(C))
                                        PKlist.Add(C);
                                }
                                //IncludePK = true;
                                //break;
                            }
                        }
                    }
                    if (!IncludePK && table.ViewTables.Count > 0)
                    {
                        foreach (string Table in table.ViewTables)
                        {
                            DiversityWorkbench.Data.Table ViewTable = new Data.Table(Table);
                            if (MainTableRelations.ContainsKey(Table) ||
                            MainTableParents.ContainsKey(Table))
                            {
                                foreach (string C in maintable.PrimaryKeyColumnList)
                                {
                                    if (ViewTable.PrimaryKeyColumnList.Contains(C) && !PKlist.Contains(C))
                                        PKlist.Add(C);
                                }
                                //IncludePK = true;
                                //break;
                            }
                        }
                    }
                }
                //if (IncludePK)
                //{
                //    foreach (string PK in maintable.PrimaryKeyColumnList)
                //    {
                //        if (table.PrimaryKeyColumnList.Contains(PK) && Where.IndexOf("[" + PK + "]") == -1 && PKlist.Contains(PK))
                //        {
                //            Where = " T.[" + PK + "] = " + UserControlQueryList.TableAliases[QC.Condition().Table] + ".[" + PK + "] AND " + Where;
                //        }
                //    }
                //}
                if (PKlist.Count == 0)
                {
                    string Message = "";
                    foreach (string PK in maintable.PrimaryKeyColumnList)
                    {
                        if (table.PrimaryKeyColumnList.Contains(PK) && Where.IndexOf("[" + PK + "]") == -1)
                        {
                            Message += "\r\n" + PK;
                        }
                    }
                    if (Message.Length > 0)
                    {
                        string Colums = Message;
                        Message = "No link detected between\r\n";
                        if (table.ViewTables.Count > 0)
                        {
                            for (int ii = 0; ii < table.ViewTables.Count; ii++)
                            {
                                if (ii > 0) Message += " + ";
                                Message += table.ViewTables[ii] + " ";
                            }
                            Message += " as view " + table.Name;
                        }
                        else Message += table.Name;
                        Message += " and\r\n";
                        if (maintable.ViewTables.Count > 0)
                        {
                            for (int ii = 0; ii < maintable.ViewTables.Count; ii++)
                            {
                                if (ii > 0) Message += " + ";
                                Message += maintable.ViewTables[ii] + " ";
                            }
                            Message += " as view " + maintable.Name;
                        }
                        else Message += table.Name;
                        Message += "\r\ninspite of corresponding columns:" + Colums;
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("UserControlQueryList", "OptimizedQueryStringWhereClause()", Message);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return PKlist;
        }

        private System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation> _AnnotationControls = new List<UserControls.UserControlQueryConditionAnnotation>();
        private System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable> _ReferencingTableControls = new List<UserControls.UserControlQueryConditionReferencingTable>();
        private System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionModule> _ModuleControls = new List<UserControls.UserControlQueryConditionModule>();
        private System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryCondition> _FreeQueryControls = new List<UserControlQueryCondition>();

        #region Markus 22.5.23 - Fehlersuche - kann wieder weg wenn gefunden. In der From klausel fehlen Tabellen, ebenso in der Where Klausel - offenbar werden teils nicht verwendete QueryOptions mit eingebunden

        private System.Collections.Generic.Dictionary<int, string> _AliasCheck = new Dictionary<int, string>();
        private void AliasCheckAdd(string TableAndAlias)
        {
            if (_AliasCheck == null)
                _AliasCheck = new Dictionary<int, string>();
            _AliasCheck.Add(_AliasCheck.Count, TableAndAlias);
        }

        private bool? _FixIfNeeded = null;
        private string CheckFromClause(string SQL, bool FixIfNeeded = false)
        {
            System.Collections.Generic.Dictionary<string, string> MissingTables = new Dictionary<string, string>();
            string MessageTables = "";
            string MessageFixMany = "";
            string MessageFixAlias = "";
            string MessageMany = "";
            string MessageAlias = "";
            string Prefix = "Error in FROM clause";
            if (_TableAliases != null && _TableAliases.Count > 0)
            {
                MessageTables += "\r\n_TableAliases:";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _TableAliases)
                {
                    MessageTables += "\r\n" + KV.Key + ": " + KV.Value;
                }
            }
            if (_ManyOrderByColumns_TableAliases != null && _ManyOrderByColumns_TableAliases.Count > 0)
            {
                MessageTables += "\r\n\r\n_ManyOrderByColumns_TableAliases:";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _ManyOrderByColumns_TableAliases)
                {
                    MessageTables += "\r\n" + KV.Key + ": " + KV.Value;
                }
            }

#if DEBUG
            // Markus 22.5.23: nur zum Test
            //MissingTables.Add("Identification_Core2", "T" + _TableAliases.Count.ToString());
            //MissingTables.Add("Identification_Core2", "M" + _ManyOrderByColumns_TableAliases.Count.ToString());
#endif
            if (_ManyOrderByColumns_TableAliases != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._ManyOrderByColumns_TableAliases)
                {
                    if (SQL.IndexOf(" AS " + KV.Value + " ") == -1)
                    {
                        string PrefixMany = Prefix + " according to many order columns:\r\n";
                        MessageMany = "\r\nmissing table " + KV.Key + " with alias " + KV.Value + "\r\n";
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("UserControlQueryList", "CheckFromClause", PrefixMany + SQL + ":\r\n" + MessageMany + MessageTables);
                        MissingTables.Add(KV.Key, KV.Value);
                        if (MessageFixMany.Length == 0) MessageFixMany = Prefix;
                        MessageFixMany += MessageMany;
                    }
                }
            }
            if (_TableAliases != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _TableAliases)
                {
                    if (SQL.IndexOf(" AS " + KV.Value) == -1)
                    {
                        string PrefixAlias = Prefix + " according to table aliases:\r\n";
                        MessageAlias = "\r\nmissing table " + KV.Key + " with alias " + KV.Value + "\r\n";
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("UserControlQueryList", "CheckFromClause", PrefixAlias + SQL + ":\r\n" + MessageAlias + "\r\n" + MessageTables);
                        if (!MissingTables.ContainsKey(KV.Key))
                            MissingTables.Add(KV.Key, KV.Value);
                        if (MessageFixAlias.Length == 0) MessageFixAlias = Prefix;
                        MessageFixAlias += MessageAlias;
                    }
                }
            }
#if DEBUG
            _FixIfNeeded = null;
#endif
            if (FixIfNeeded && _FixIfNeeded == null && (MessageFixMany.Length > 0 || MessageFixAlias.Length > 0))
            {
                switch (System.Windows.Forms.MessageBox.Show("Error in FROM clause:\r\n" + SQL + "\r\n" + MessageFixAlias + "\r\n" + MessageFixMany + "\r\n" + MessageTables + "\r\n\r\nShould this error be fixed?", "Error in query", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        _FixIfNeeded = true;
                        break;
                    case DialogResult.No:
                        _FixIfNeeded = false;
                        break;
                    default:
                        _FixIfNeeded = null;
                        break;
                }
            }
            if (SQL.Length > 0 && _FixIfNeeded != null && (bool)_FixIfNeeded && MissingTables.Count > 0)
            {
                string[] sql = SQL.Split(new char[] { ',' });
                string SqlFixed = "";
                string SqlMain = "";
                for (int i = 0; i < sql.Length; i++)
                {
                    if (sql[i].IndexOf(" FROM ") > -1)
                        SqlMain = sql[i].Replace(" FROM", " ");
                    else
                    {
                        if (SqlFixed.Length > 0) SqlFixed += ", ";
                        SqlFixed += sql[i];
                    }
                }

                SQL = " FROM " + SqlFixed + "," + SqlMain;

                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in MissingTables)
                {
                    if (KV.Value.StartsWith("M")) SQL += " LEFT OUTER ";
                    else SQL += " INNER ";
                    SQL += "JOIN " + KV.Key + " AS " + KV.Value + " ON ";
                    DiversityWorkbench.Data.Table Tmain = new Data.Table(_QueryMainTableOptimizing);
                    DiversityWorkbench.Data.Table Tmissing = new Data.Table(KV.Key);
                    string Join = "";
                    foreach (string PK in Tmain.PrimaryKeyColumnList)
                    {
                        if (Join.Length > 0) Join += " AND ";
                        if (Tmissing.PrimaryKeyColumnList.Contains(PK))
                        {
                            Join += " T." + PK + " = " + KV.Value + ". " + PK + " ";
                        }
                    }
                    SQL += Join;
                }
            }
            return SQL;
        }

        private string CheckFromClauseTable(
            string SQL,
            System.Collections.Generic.Dictionary<string, string> Tables,
            ref System.Collections.Generic.Dictionary<string, string> MissingTables,
            ref string Message,
            ref string MessageFix)
        {
            string _message = "";
            //if (Tables != null)
            //{
            //    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Tables)
            //    {
            //        if (SQL.IndexOf(" AS " + KV.Value + " ") == -1)
            //        {
            //            string PrefixMany = Prefix + " according to many order columns:\r\n";
            //            MessageMany = "missing table " + KV.Key + " with alias " + KV.Value + "\r\n";
            //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("UserControlQueryList", "CheckFromClause", PrefixMany + SQL + ":\r\n" + MessageMany + MessageTables);
            //            if (!MissingTables.ContainsKey(KV.Key))
            //                MissingTables.Add(KV.Key, KV.Value);
            //            if (MessageFixMany.Length == 0) MessageFixMany = Prefix;
            //            MessageFixMany += MessageMany;
            //        }
            //    }
            //}
            ////if (SQL.IndexOf(" AS " + Alias + " ") == -1)
            ////{
            ////    string PrefixAlias = Prefix + " according to table aliases:\r\n";
            ////    Message = "missing table " + Table + " with alias " + Alias + "\r\n";
            ////    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("UserControlQueryList", "CheckFromClause", PrefixAlias + SQL + ":\r\n" + Message + MessageTables);
            ////    if (!MissingTables.ContainsKey(Table))
            ////        MissingTables.Add(Table, Alias);
            ////    if (MessageFix.Length == 0) MessageFix = Prefix;
            ////    MessageFix += Message;
            ////}
            return _message;
        }

        #endregion

        private void OptimizedQueryAnalyseTables()
        {
            _AliasCheck = new Dictionary<int, string>();
            try
            {
                // Markus 15.2.2018 - Zuruecksetzen vor Suche
                if (this._AnnotationControls != null)
                    this._AnnotationControls.Clear();
                if (this._ReferencingTableControls != null)
                    this._ReferencingTableControls.Clear();
                if (this._ModuleControls != null)
                    this._ModuleControls.Clear();
                if (this._FreeQueryControls != null)
                    this._FreeQueryControls.Clear();
                if (this._FreeTextCheckBoxes != null)
                    this._FreeTextCheckBoxes.Clear();

                foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                {
                    if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        foreach (System.Windows.Forms.Control U in G.Controls)
                        {
                            try
                            {
                                if (U.GetType() != typeof(DiversityWorkbench.IUserControlQueryCondition)
                                    && U.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryCondition)
                                    && U.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryConditionGIS)
                                    && U.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation)
                                    && U.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable)
                                    && U.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryConditionHierarchy)
                                    && U.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryConditionXML)
                                    && U.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryConditionSet)
                                    && U.GetType() != typeof(DiversityWorkbench.UserControls.UserControlQueryConditionModule))
                                    continue;
                                DiversityWorkbench.IUserControlQueryCondition C = (DiversityWorkbench.IUserControlQueryCondition)U;
                                string Table = C.getCondition().Table;
                                if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryCondition UC = (DiversityWorkbench.UserControls.UserControlQueryCondition)U;
                                    if (UC.QueryCondition.QueryFields.Count > 0
                                        && UC.WhereClause().Length > 0
                                        && UC.QueryCondition.HierarchyColumn == null)
                                        this._FreeQueryControls.Add(UC);
                                }
                                if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation UA = (DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation)U;
                                    if (UA.WhereClause().Length > 0)
                                        this._AnnotationControls.Add(UA);
                                }
                                if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable))
                                {
                                    DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable UA = (DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable)U;
                                    if (UA.WhereClause().Length > 0)
                                        this._ReferencingTableControls.Add(UA);
                                }
                                else
                                {
                                    if (C.WhereClause().Length > 0)
                                    {
                                        if (!UserControlQueryList.TableAliases.ContainsKey(Table))
                                        {
                                            if (C.getCondition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable &&
                                                C.getCondition().Operator == "Ø")
                                            {
                                                // not adding alias for searching missing data
                                            }
                                            else if (C.Condition().SqlFromClausePostfix == null
                                                && (C.getCondition().QueryFields == null || C.getCondition().QueryFields.Count == 0)
                                                && C.getCondition().Table != UserControlQueryList._QueryMainTableOptimizing)
                                            {
                                                UserControlQueryList.TableAliases.Add(Table, "T" + UserControlQueryList.TableAliases.Count.ToString());
                                                AliasCheckAdd(Table + " AS T" + UserControlQueryList.TableAliases.Count.ToString());
                                            }
                                            System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition> QC = new List<IUserControlQueryCondition>();
                                            QC.Add(C);
                                            if (!UserControlQueryList.TableQueryConditions.ContainsKey(Table))
                                                UserControlQueryList.TableQueryConditions.Add(Table, QC);
                                            else
                                            {
                                                UserControlQueryList.TableQueryConditions[Table].Add(C);
                                            }
                                        }
                                        else
                                        {
                                            if (!UserControlQueryList.TableQueryConditions.ContainsKey(Table))
                                            {
                                                System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition> QC = new List<IUserControlQueryCondition>();
                                                QC.Add(C);
                                                if (!UserControlQueryList.TableQueryConditions.ContainsKey(Table))
                                                    UserControlQueryList.TableQueryConditions.Add(Table, QC);
                                                else
                                                {
                                                    UserControlQueryList.TableQueryConditions[Table].Add(C);
                                                }
                                            }
                                            else if (!UserControlQueryList.TableQueryConditions[Table].Contains(C))
                                                UserControlQueryList.TableQueryConditions[Table].Add(C);
                                        }
                                        if (U.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionSet))
                                        {
                                            DiversityWorkbench.UserControls.UserControlQueryConditionSet S = (DiversityWorkbench.UserControls.UserControlQueryConditionSet)U;
                                            if (UserControlQueryList.QueryConditionSets.ContainsKey(S.getCondition().Table))
                                            {
                                                if (UserControlQueryList.QueryConditionSets[S.getCondition().Table].ContainsKey(S.getCondition().Column))
                                                {
                                                    // only the control is missing
                                                    UserControlQueryList.QueryConditionSets[S.getCondition().Table][S.getCondition().Column].Add(S);
                                                }
                                                else
                                                {
                                                    // Column is missing
                                                    System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionSet> L = new List<UserControls.UserControlQueryConditionSet>();
                                                    L.Add(S);
                                                    UserControlQueryList.QueryConditionSets[S.getCondition().Table].Add(S.getCondition().Column, L);

                                                    // Adding the missing operator for the table.column
                                                    UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][S.getCondition().Table].Add(S.getCondition().Column, S.Operator);
                                                }
                                            }
                                            else
                                            {
                                                // Entry for table is missing

                                                // Adding the missing entry in the set
                                                System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionSet> L = new List<UserControls.UserControlQueryConditionSet>();
                                                L.Add(S);
                                                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<DiversityWorkbench.UserControls.UserControlQueryConditionSet>> D = new Dictionary<string, List<UserControls.UserControlQueryConditionSet>>();
                                                D.Add(S.getCondition().Column, L);
                                                UserControlQueryList.QueryConditionSets.Add(S.getCondition().Table, D);

                                                // Adding the missing operator for the table.column
                                                if (!UserControlQueryList.QueryConditionSetOperators.ContainsKey(DiversityWorkbench.Settings.ModuleName))
                                                {
                                                    System.Collections.Generic.Dictionary<string, DiversityWorkbench.UserControls.UserControlQueryList.QueryOperator> DO = new Dictionary<string, QueryOperator>();
                                                    DO.Add(S.getCondition().Column, S.Operator);
                                                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityWorkbench.UserControls.UserControlQueryList.QueryOperator>> DDO = new Dictionary<string, Dictionary<string, QueryOperator>>();
                                                    DDO.Add(S.getCondition().Table, DO);
                                                    UserControlQueryList.QueryConditionSetOperators.Add(DiversityWorkbench.Settings.ModuleName, DDO);
                                                }
                                            }

                                            // Adding missing entries for the set aliases
                                            if (!UserControlQueryList.QueryConditionSetTableAliases.ContainsKey(S))
                                            {
                                                int i = 0;
                                                // Added controls get a table alias like T0_s1 where TO is the alias for the first table and _s1 for added contols where e.g. 1 is the position of the control in a 0 based list
                                                foreach (System.Collections.Generic.KeyValuePair<DiversityWorkbench.UserControls.UserControlQueryConditionSet, string> KV in UserControlQueryList.QueryConditionSetTableAliases)
                                                {
                                                    if (KV.Key.getCondition().Table == S.getCondition().Table)
                                                        i++;
                                                }
                                                if (UserControlQueryList.TableAliases.ContainsKey(S.getCondition().Table))
                                                {
                                                    string Alias = UserControlQueryList.TableAliases[S.getCondition().Table] + "_s" + i.ToString();
                                                    if (i == 0)
                                                        Alias = UserControlQueryList.TableAliases[S.getCondition().Table];
                                                    UserControlQueryList.QueryConditionSetTableAliases.Add(S, Alias);
                                                }
                                            }
                                        }
                                        // Markus 9.6.2016 - Intermediate table may be missing here
                                        // Adding an intermediate table if missing
                                        if (C.Condition().IntermediateTable != null && C.Condition().IntermediateTable.Length > 0)
                                        {
                                            string IntermediateTable = C.Condition().IntermediateTable;
                                            if (!UserControlQueryList.TableAliases.ContainsKey(IntermediateTable)
                                                && C.getCondition().Operator != "Ø"
                                                && !UserControlQueryList.TableQueryConditions.ContainsKey(IntermediateTable))
                                            {
                                                UserControlQueryList.TableAliases.Add(IntermediateTable, "T" + UserControlQueryList.TableAliases.Count.ToString());
                                                AliasCheckAdd(IntermediateTable + " AS T" + UserControlQueryList.TableAliases.Count.ToString());
                                                System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition> QC = new List<IUserControlQueryCondition>();
                                                QC.Add(C);
                                                //UserControlQueryList.TableQueryConditions.Add(IntermediateTable, QC);
                                            }
                                        }
                                        // Markus 15.2.2018 - Freequery tables missing
                                        //if (C.Condition().QueryFields != null 
                                        //    && C.Condition().QueryFields.Count > 1
                                        //    && C.getCondition().Operator != "Ø")
                                        //{
                                        //    foreach (DiversityWorkbench.QueryField F in C.Condition().QueryFields)
                                        //    {
                                        //        if (F.TableName != UserControlQueryList.QueryMainTable)
                                        //        {
                                        //            if (!UserControlQueryList.TableQueryConditions.ContainsKey(F.TableName))
                                        //            {
                                        //                System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition> QC = new List<IUserControlQueryCondition>();
                                        //                QC.Add(C);
                                        //                if (!UserControlQueryList.TableQueryConditions.ContainsKey(F.TableName))
                                        //                    UserControlQueryList.TableQueryConditions.Add(F.TableName, QC);
                                        //                else
                                        //                {
                                        //                    UserControlQueryList.TableQueryConditions[F.TableName].Add(C);
                                        //                }
                                        //            }
                                        //            else if (!UserControlQueryList.TableQueryConditions[F.TableName].Contains(C))
                                        //                UserControlQueryList.TableQueryConditions[F.TableName].Add(C);
                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string OptimizedQueryFromClause()
        {
            string SQL = " FROM " + UserControlQueryList.QueryMainTable + " AS T ";
            try
            {
                if (ManyOrderByColumns())
                {
                    try
                    {
                        System.Collections.Generic.List<string> done = new List<string>();
                        foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                        {
                            if (!UserControlQueryList.TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName)
                                && KV.Value.QueryOrderColumn.TableName != UserControlQueryList._QueryMainTableOptimizing
                                && !done.Contains(KV.Value.QueryOrderColumn.TableName))
                            {
                                if (this._QueryMainTableLocal == UserControlQueryList._QueryMainTableOptimizing)
                                    SQL += " LEFT";
                                else
                                    SQL += " RIGHT";
                                SQL += " OUTER JOIN " + KV.Value.QueryOrderColumn.TableName + " AS " + ManyOrderByColumns_TableAliases(KV.Value.QueryOrderColumn.TableName)[KV.Value.QueryOrderColumn.TableName];// (UserControlQueryList.TableAliases.Count + i).ToString();
                                SQL += " ON " + this.ManyOrderByColumns_WhereClause(KV.Value.QueryOrderColumn.TableName);
                                done.Add(KV.Value.QueryOrderColumn.TableName);
                            }
                        }
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }

                // ensure presence of main table for many order columns
                if (ManyOrderByColumns_TableAliases().Count > 0
                    && !UserControlQueryList.TableAliases.ContainsKey(this._QueryMainTableLocal)
                    && ManyOrderByColumns_TableAliases().ContainsKey(this._QueryMainTableLocal)
                    && SQL.IndexOf(" AS " + ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal] + ".") == -1
                    && SQL.IndexOf(" " + this._QueryMainTableLocal + " AS " + ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal]) == -1)
                {
                    SQL += " RIGHT OUTER JOIN " + this._QueryMainTableLocal + " AS " + ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal]
                        + " ON T." + UserControlQueryList.IdentityColumnOptimizing + " = " + ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal] + "." + UserControlQueryList.IdentityColumnOptimizing;
                }

                if (this.LinkedServer.Length > 0)
                {
                    if (this.LinkedServerDatabase.StartsWith("[" + this.LinkedServer + "]."))
                        SQL = " FROM " + this.LinkedServerDatabase + ".dbo." + UserControlQueryList.QueryMainTable + " AS T ";
                    else
                        SQL = " FROM [" + this.LinkedServer + "]." + this.LinkedServerDatabase + ".dbo." + UserControlQueryList.QueryMainTable + " AS T ";
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in UserControlQueryList.TableAliases)
                {
                    if (KV.Key != UserControlQueryList.QueryMainTable)// && !NotExistsTableAliases.Contains(KV.Value))
                    {
                        if (this.LinkedServer.Length > 0)
                        {
                            if (this.LinkedServerDatabase.StartsWith("[" + this.LinkedServer + "]."))
                                SQL += ", " + this.LinkedServerDatabase + ".dbo." + KV.Key + " AS " + KV.Value;
                            else
                                SQL += ", [" + this.LinkedServer + "]." + this.LinkedServerDatabase + ".dbo." + KV.Key + " AS " + KV.Value;
                        }
                        else
                        {
                            if (!UserControlQueryList.TableAliasesNotExists.ContainsKey(KV.Key))
                                SQL += ", " + KV.Key + " AS " + KV.Value;
                            else
                            {
                                bool IsOnlyNonExists = true;
                                if (UserControlQueryList.TableQueryConditions.ContainsKey(KV.Key))
                                {
                                    foreach (DiversityWorkbench.IUserControlQueryCondition QC in UserControlQueryList.TableQueryConditions[KV.Key])
                                    {
                                        string Operator = QC.getCondition().QueryConditionOperator;
                                        if (Operator != "Ø" && Operator != "∉" && QC.Condition().QueryType != QueryCondition.QueryTypes.Count)
                                        {
                                            IsOnlyNonExists = false;
                                            break;
                                        }
                                    }
                                }
                                if (!IsOnlyNonExists)
                                    SQL += ", " + KV.Key + " AS " + KV.Value;
                            }
                        }
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<DiversityWorkbench.UserControls.UserControlQueryConditionSet, string> KV in UserControlQueryList.QueryConditionSetTableAliases)
                {
                    if (UserControlQueryList.QueryConditionSetOperators.ContainsKey(DiversityWorkbench.Settings.ModuleName) &&
                        UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName].ContainsKey(KV.Key.getCondition().Table) &&
                        UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][KV.Key.getCondition().Table].ContainsKey(KV.Key.getCondition().Column) &&
                        UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][KV.Key.getCondition().Table][KV.Key.getCondition().Column] == QueryOperator.OR)
                        continue;
                    if (!UserControlQueryList.TableAliases.ContainsValue(KV.Value))
                    {
                        if (this.LinkedServer.Length > 0)
                        {
                            SQL += ", [" + this.LinkedServer + "]." + this.LinkedServerDatabase + ".dbo." + KV.Key.getCondition().Table + " AS " + KV.Value;
                        }
                        else
                            SQL += ", " + KV.Key.getCondition().Table + " AS " + KV.Value;
                        AliasCheckAdd(KV.Key.getCondition().Table + " AS T" + UserControlQueryList.TableAliases.Count.ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            //SQL = this.CheckFromClause(SQL, true);
            return SQL;
        }

        private string OptimizedQueryFreeQuery(int i)
        {
            string SQL = "";
            if (this._FreeQueryControls.Count > 0)
            {
                bool OK = false;
                if (!SQL.EndsWith(" WHERE "))
                    SQL += " AND ";
                SQL += " ( ";
                foreach (DiversityWorkbench.UserControls.UserControlQueryCondition UC in this._FreeQueryControls)
                {
                    string s = "";
                    for (int iQF = 0; iQF < UC.QueryCondition.QueryFields.Count; iQF++)
                    {
                        if (UC.QueryCondition.QueryFields[iQF].TableName == UserControlQueryList._QueryMainTable)
                        {
                            string x = UC.SqlByIndex(iQF);// UC.WhereClause();
                            x = "T." + x.Substring(x.IndexOf('.') + 1);
                            if (s.Length > 0)
                                s += " OR ";
                            s += x;
                            //s += UC.. + "' ";
                        }
                        else
                        {
                            if (UC.SqlByIndex(i) != "")
                            {
                                if (iQF == 0)
                                {
                                    s = " AND (";
                                }
                                if (iQF > 0)
                                {
                                    //if (s.Length == 0)
                                    //    s = " (";
                                    //else
                                    if (UC.Condition().CombineQueryFieldsWithAnd)
                                        s += " AND ";
                                    else
                                        s += " OR ";
                                }
                                s += UC.SqlByIndex(iQF);

                            }
                        }
                    }
                    if (s.Length > 0)
                    {
                        SQL += s;
                        OK = true;
                    }
                }
                if (OK) SQL += ")";
            }
            return SQL;
        }

        private string OptimizedQueryAnnotations()
        {
            string SQL = "";
            foreach (DiversityWorkbench.UserControls.UserControlQueryConditionAnnotation UA in this._AnnotationControls)
            {
                if (!SQL.EndsWith(" WHERE "))
                    SQL += " AND ";
                SQL += "T." + _IdentityColumnOptimizing + " IN(" + UA.SQL() + ")";
            }
            return SQL;
        }

        private string OptimziedQueryModules()
        {
            string SQL = "";
            foreach (DiversityWorkbench.UserControls.UserControlQueryConditionModule UA in this._ModuleControls)
            {
                //if (!SQL.EndsWith(" WHERE "))
                //    SQL += " AND ";
                //SQL += "T." + _IdentityColumnOptimizing + " IN(" + UA.SQL() + ")";
            }
            return SQL;
        }

        private string OptimizedQueryReferencingTables()
        {
            string SQL = "";
            foreach (DiversityWorkbench.UserControls.UserControlQueryConditionReferencingTable UA in this._ReferencingTableControls)
            {
                if (UA.SQL().Length > 0)
                {
                    if (!SQL.EndsWith(" WHERE "))
                        SQL += " AND ";
                    SQL += "T." + _IdentityColumnOptimizing + " IN (" + UA.SQL() + ")";
                }
            }
            return SQL;
        }

        private string OptimizedQueryRestrictions()
        {
            string SQL = "";
            foreach (DiversityWorkbench.UserControls.QueryRestrictionItem I in this.getQueryRestrictionList())
            {
                if (I.TableName == QueryMainTable)
                    SQL += " AND T." + I.ColumnName + " " + I.Restriction + " ";
                else
                {
                    if (UserControlQueryList.TableAliases.ContainsKey(I.TableName))
                        SQL += " AND T." + _IdentityColumnOptimizing + " = " + UserControlQueryList.TableAliases[I.TableName] + "." + _IdentityColumnOptimizing + " AND " + UserControlQueryList.TableAliases[I.TableName] + "." + I.ColumnName + I.Restriction;
                    else
                        SQL += " AND " + QueryTable + "." + _IdentityColumnOptimizing + " IN (SELECT " + _IdentityColumnOptimizing + " FROM " + I.TableName + " WHERE " + I.ColumnName + " " + I.Restriction + ") ";
                }
            }
            return SQL;
        }

        #endregion

        #region Context menu

        private void toolStripMenuItemOptimizingAsDefault_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.QueryOptimizedByDefault = true;
            this.OptimizingAvailable(true);
        }

        #endregion

        #region Temporary table for IDs to speed up query esp. for table editors

        #region Connection for temporary IDs - the temporary table exists only within this connection


        private string GetTempIDConnectionString()
        {
            string connectionString = this._ServerConnection?.ConnectionString
                                      ?? DiversityWorkbench.Settings.ConnectionString;
            //}
            return connectionString;
        }

        private Microsoft.Data.SqlClient.SqlConnection _ConnectionTempIDs;

        /// <summary>
        /// access to the connection for temporary IDs in table #ID
        /// must be permanent to ensure existence of the table #ID
        /// #250
        /// </summary>
        /// <returns>the connection</returns>
        public Microsoft.Data.SqlClient.SqlConnection ConnectionTempIDs()
        {
            try
            {
                if (this._ConnectionTempIDs == null)
                {
                    // #250
                    string connectionString = this.GetTempIDConnectionString();

                    //string connectionString = null;
                    //if (this._ServerConnection != null)
                    //{
                    //    connectionString = this._ServerConnection.ConnectionString;
                    //}
                    //else if (!string.IsNullOrWhiteSpace(DiversityWorkbench.Settings.ConnectionString))
                    //    connectionString = DiversityWorkbench.Settings.ConnectionString;

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new InvalidOperationException("No valid connection string is available.");
                    }
                    this._ConnectionTempIDs = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                }

                // #250
                if (_ConnectionTempIDs.ConnectionString == null || _ConnectionTempIDs.ConnectionString.Length == 0)
                {
                    _ConnectionTempIDs.ConnectionString = this.GetTempIDConnectionString();
                }

                if (this._ConnectionTempIDs.State == ConnectionState.Closed)
                    this._ConnectionTempIDs.Open();
                return _ConnectionTempIDs;
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow it
                ExceptionHandling.WriteToErrorLogFile($"Error in ConnectionTempIDs: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Closes the connection for temporary IDs e.g. when the database is changed.
        /// </summary>
        public void CloseConnectionTempIDs()
        {
            if (this._ConnectionTempIDs != null)
            {
                if (this._ConnectionTempIDs.State == ConnectionState.Open)
                    this._ConnectionTempIDs.Close();
                this._ConnectionTempIDs.Dispose();
            }
        }

        #endregion

        private string QueryStringTempIDs
        {
            get
            {
                string SQL = "";
                if (UseOptimizing)
                {
                    string IdentityColumn = "";
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQueryColumn.SelectedItem;
                    string OrderColumn = R[1].ToString();
                    SQL = this.QueryStringWhereClause;
                    foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                    {
                        if (C.DisplayColumn == this.SelectedDisplayColumn)
                        {
                            IdentityColumn = C.IdentityColumn;
                            break;
                        }
                    }
                    // Markus 16.6.2020: OrderColumn um TabellenAlias T. erweitert
                    SQL = "SELECT DISTINCT TOP " + DiversityWorkbench.Settings.QueryMaxResults.ToString() + " T." + IdentityColumn + ", T." + OrderColumn + " " + SQL;
                    SQL += " ORDER BY T." + OrderColumn;
                }
                return SQL;
            }
        }

        public Microsoft.Data.SqlClient.SqlDataAdapter DataAdapterForTempIDs(string sql)
        {
            try
            {
                if (!this.InitTempIDs())
                {
                    return null;
                }
                // #250: Ensure that the temporary table #ID exists and is ready for use
                // Get the connection string
                //string connectionString = GetTempIDConnectionString();
                //if (string.IsNullOrWhiteSpace(connectionString))
                //    return null;

                //// Create and return the data adapter
                //var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);

                return new Microsoft.Data.SqlClient.SqlDataAdapter(sql, this.ConnectionTempIDs());
            }
            catch (Exception ex)
            {
                ExceptionHandling.WriteToErrorLogFile($"Error in SqlExecuteForTempIDs: {ex.Message}");
                return null;
            }
        }

        private bool InitTempIDs()
        {
            bool OK = true;
            if (this.ListOfIDs.Count > 0)
            {
                string Test = this.SqlExecuteScalarForTempIDs("SELECT COUNT(*) FROM #ID");
                if (Test.Length == 0 || Test == "0")
                {
                    this.SetTempIDs();
                }
            }
            return OK;
        }

        private bool SetTempIDs()
        {
            if (this.ResetTempIdTable())
            {
                string SQL = "INSERT INTO #ID " + this.QueryStringTempIDs;
                this.SqlExecuteForTempIDs(SQL);
            }
            bool OK = true;
            return OK;
        }

        private bool ResetTempIdTable()
        {
            try
            {
                // Check if the temporary table exists
                if (CheckTempIDExists())
                {
                    string truncateTableSql = "TRUNCATE TABLE #ID";
                    if (!SqlExecuteForTempIDs(truncateTableSql))
                    {
                        return false;
                    }
                }
                else
                {
                    // Create the temporary table if it does not exist
                    string createTableSql = "CREATE TABLE #ID (ID INT PRIMARY KEY, OrderColumn NVARCHAR(500) NULL)";
                    if (!SqlExecuteForTempIDs(createTableSql))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandling.WriteToErrorLogFile($"Error in ResetTempIdTable: {ex.Message}");
                return false;
            }
        }

        private bool CheckTempIDExists()
        {
            string checkTableSql = "IF OBJECT_ID('tempdb..#ID') IS NOT NULL SELECT 1 ELSE SELECT 0";
            try
            {
                // Get the connection string
                string connectionString = GetTempIDConnectionString();
                if (string.IsNullOrWhiteSpace(connectionString))
                    return false;

                // #250: Ensure that the temporary table #ID exists and is ready for use
                using (var command = new Microsoft.Data.SqlClient.SqlCommand(checkTableSql, this.ConnectionTempIDs()))
                {
                    var result = command.ExecuteScalar();
                    return result != null && (int)result == 1;
                }

                //using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                //{
                //    // Open the connection only if it is not already open
                //    if (connection.State == System.Data.ConnectionState.Closed)
                //    {
                //        connection.Open();
                //    }
                //    using (var command = new Microsoft.Data.SqlClient.SqlCommand(checkTableSql, connection))
                //    {
                //        var result = command.ExecuteScalar();
                //        return result != null && (int)result == 1;
                //    }
                //}
            }
            catch (Exception ex)
            {
                ExceptionHandling.WriteToErrorLogFile($"Error in CheckTempIDExists: {ex.Message}");
                return false;
            }
        }
        private bool SqlExecuteForTempIDs(string sql)
        {
            try
            {
                //// Get the connection string
                //string connectionString = GetTempIDConnectionString();
                //if (string.IsNullOrWhiteSpace(connectionString))
                //    return false;
                //using (var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                //{
                //    // Open the connection only if it is not already open
                //    if (connection.State == System.Data.ConnectionState.Closed)
                //    {
                //        connection.Open();
                //    }
                //    using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
                //    {
                //        command.ExecuteNonQuery();
                //        return true;
                //    }
                //}

                // #250: Ensure that the temporary table #ID exists and is ready for use
                using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, this.ConnectionTempIDs()))
                {
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.WriteToErrorLogFile($"Error in SqlExecuteForTempIDs: {ex.Message}");
                return false;
            }
        }

        private string SqlExecuteScalarForTempIDs(string SQL)
        {
            string Result = "";
            try
            {
                //// Get the connection string
                //string connectionString = GetTempIDConnectionString();
                //if (string.IsNullOrWhiteSpace(connectionString))
                //    return Result;
                //var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                //Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, connection);
                //Result = C.ExecuteScalar()?.ToString() ?? string.Empty;

                // #250: Using permanent connection to ensure that the temporary table #ID exists and is ready for use
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.ConnectionTempIDs());
                Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
            }
            catch (System.Exception ex)
            {
                ExceptionHandling.WriteToErrorLogFile($"Error in SqlExecuteForTempIDs: {ex.Message}");
                return Result;
            }
            return Result;
        }

        #endregion

        #endregion

        #region Free text query

        private bool _IsFreeText = false;

        private System.Collections.Generic.Dictionary<System.Windows.Forms.CheckBox, DiversityWorkbench.QueryCondition> _FreeTextCheckBoxes;

        private void buttonFreeText_Click(object sender, EventArgs e)
        {
            if (this._IsFreeText)
            {
                this._IsFreeText = false;
                this.buttonFreeText.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                this._IsFreeText = true;
                this.buttonFreeText.BackColor = System.Drawing.Color.Red;
            }
            if (this._IsFreeText)
            {
                this.textBoxFreeText.Visible = true;
                this.panelQueryConditions.Controls.Clear();
                this.setQueryConditionsForFreeText();
            }
            else
            {
                this.textBoxFreeText.Text = "";
                this.textBoxFreeText.Visible = false;
                this.setQueryConditions(this._QueryConditions);
            }
        }

        private void setQueryConditionsForFreeText()
        {
            this._FreeTextCheckBoxes = new Dictionary<CheckBox, QueryCondition>();
            //this._FirstQueryControl = null;
            if (this._QueryConditionVisiblity.Length > 0)
            {
                int i = 0;
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QCs = new List<QueryCondition>();
                foreach (DiversityWorkbench.QueryCondition QC in this._QueryConditions)
                {
                    DiversityWorkbench.QueryCondition Qn = QC;
                    if (this._QueryConditionVisiblity.Length <= i)
                        this._QueryConditionVisiblity += "1";
                    if (this._QueryConditionVisiblity[i].ToString() == "1")
                        Qn.showCondition = true;
                    else
                        Qn.showCondition = false;
                    i++;
                    QCs.Add(Qn);
                }
                this._QueryConditions = QCs;
            }

            this.panelQueryConditions.Controls.Clear();
            int iTab = 2;
            int H = QueryColumnHeight;
            this.textBoxSQL.Visible = false;
            System.Collections.Generic.List<string> QueryGroups = new List<string>();
            this.SuspendLayout();
            this.panelQueryConditions.Visible = false;

            //if (this._ProjectDependentQueryConditions == null)
            //    this._ProjectDependentQueryConditions = new List<QueryCondition>();
            this.ProjectDependentQueryConditions.Clear();


            foreach (DiversityWorkbench.QueryCondition Q in this._QueryConditions)
            {
                try
                {
                    if (Q.showCondition)
                    {
                        if (!QueryGroups.Contains(Q.QueryGroup))
                        {
                            QueryGroups.Add(Q.QueryGroup);
                            System.Windows.Forms.GroupBox g = new GroupBox();
                            this.panelQueryConditions.Controls.Add(g);
                            if (iTab > -1) g.TabIndex = iTab;
                            iTab--;
                            g.Text = Q.QueryGroup;

                            if (this.TableColors != null &&
                                this.TableColors.ContainsKey(Q.Table))
                                g.ForeColor = this.TableColors[Q.Table];
                            if (this.TableImageIndex != null &&
                                this.ImageList != null &&
                                (this.TableImageIndex.ContainsKey(Q.Table) ||
                                this.TableImageIndex.ContainsKey(Q.QueryGroup.Replace(" ", ""))))
                            {
                                System.Windows.Forms.PictureBox p = new PictureBox();
                                if (this.TableImageIndex.ContainsKey(Q.QueryGroup.Replace(" ", "")))
                                    p.Image = this.ImageList.Images[this.TableImageIndex[Q.QueryGroup.Replace(" ", "")]];
                                else
                                    p.Image = this.ImageList.Images[this.TableImageIndex[Q.Table]];
                                p.Height = 16;
                                p.Width = 16;
                                p.Dock = DockStyle.Left;
                                System.Windows.Forms.Padding P = new Padding(0, -10, 0, 0);
                                p.Margin = P;
                                g.Controls.Add(p);
                            }

                            if (DiversityWorkbench.Entity.EntityTablesExist)
                            {
                                System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation(Q.QueryGroup);
                                if (Entity["DisplayGroup"].Length > 0)
                                    g.AccessibleName = Entity["DisplayGroup"];
                                else
                                {
                                    if (!Q.useGroupAsEntityForGroups)
                                        Entity = DiversityWorkbench.Entity.EntityInformation(Q.Table + "." + Q.QueryGroup);
                                    else if (Entity["DisplayText"].Length > 0)
                                        g.Text = Entity["DisplayText"];
                                    if (Entity["DisplayGroup"].Length > 0)
                                        g.AccessibleName = Entity["DisplayGroup"];
                                    else
                                    {
                                        string Table = Q.Table;
                                        if (Table.EndsWith("_Core"))
                                            Table = Table.Substring(0, Table.IndexOf("_Core"));
                                        if (!Q.useGroupAsEntityForGroups)
                                            g.AccessibleName = Table;
                                        else
                                            g.AccessibleName = Entity["Entity"];
                                    }
                                }
                            }
                            else
                            {
                                string Table = Q.Table;
                                if (Table.EndsWith("_Core"))
                                    Table = Table.Substring(0, Table.IndexOf("_Core"));
                                g.AccessibleName = Table;
                            }

                            g.Dock = DockStyle.Top;
                            g.BringToFront();
                            H += this.fillQueryGroupBoxForFreeText(g);
                            if (H == 42)
                            {
                                this.panelQueryConditions.Controls.Remove(g);
                                g.Dispose();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            //this.setQueryConditionsDisplayTexts();
            this.setEntity();
            this._QueryMaxHeight = H;
            this.setQueryConditionsHeight();
            this._QueryMaxHeight = H;
            this.panelQueryConditions.Visible = true;
            this.ResumeLayout();
        }

        private int fillQueryGroupBoxForFreeText(System.Windows.Forms.GroupBox GroupBox)
        {
            int H = 20;
            try
            {
                int iTab = 0;
                foreach (DiversityWorkbench.QueryCondition Q in this._QueryConditions)
                {
                    if (Q.showCondition
                        && (Q.QueryGroup == GroupBox.Text
                        || (GroupBox.AccessibleName != null
                        && (Q.QueryGroup == GroupBox.AccessibleName))))
                    {

                        if (Q.QueryType == QueryCondition.QueryTypes.Text && !Q.IsBoolean && !Q.IsDate && !Q.IsDatetime && !Q.IsNumeric && !Q.IsXML && !Q.IsYear && !Q.SourceIsFunction
                            && Q.DisplayText != "Presence"
                            && !Q.DisplayText.EndsWith(" present"))// && Q.IsSet)
                        {
                            System.Windows.Forms.CheckBox CB = new CheckBox();
                            CB.Text = Q.DisplayText;
                            GroupBox.Controls.Add(CB);
                            CB.Dock = DockStyle.Top;
                            CB.BringToFront();
                            CB.TabIndex = iTab;
                            CB.Checked = true;
                            this._FreeTextCheckBoxes.Add(CB, Q);
                            iTab++;
                            H += 24;

                        }
                    }
                }
                GroupBox.Height = H;
            }
            catch (System.Exception ex) { }
            return H;
        }

        private string QueryStringWhereClauseFreeText()
        {
            string SQL = "";
            string _FromClause = "";
            string _WhereClause = "";
            string _WhereJoins = "";

            System.Collections.Generic.List<string> _FreeTextTables = new List<string>();
            string QueryTable = "";
            string IdentityColumn = "";
            string DisplayColumn = "";
            if (this.comboBoxQueryColumn.Text.Length > 0)
            {
                foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                {
                    if (C.DisplayColumn == this.SelectedDisplayColumn)
                    {
                        QueryTable = C.TableName;
                        IdentityColumn = C.IdentityColumn;
                        DisplayColumn = C.DisplayColumn;
                        break;
                    }
                }
                if (QueryTable.Length > 0)
                {
                    SQL += " FROM " + QueryTable + " AS T WHERE 1 = 1 AND (";
                }

                string SqlOr = "";
                foreach (System.Collections.Generic.KeyValuePair<System.Windows.Forms.CheckBox, DiversityWorkbench.QueryCondition> KV in this._FreeTextCheckBoxes)
                {
                    if (KV.Key.Checked)
                    {
                        if (SqlOr.Length > 0) SqlOr += " OR ";
                        if (KV.Value.Table == QueryTable)
                            SqlOr += "T";
                        else
                            SqlOr += "[" + KV.Value.Table + "]";
                        SqlOr += ".[" + KV.Value.Column + "] LIKE '" + this.textBoxFreeText.Text + "'";
                        if (!_FreeTextTables.Contains(KV.Value.Table))
                            _FreeTextTables.Add(KV.Value.Table);
                    }
                }
                SQL += SqlOr + ")";

                System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition> QueryConditions = new List<IUserControlQueryCondition>();
                foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                {
                    if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                    {
                        foreach (System.Windows.Forms.Control U in G.Controls)
                        {
                            try
                            {
                                if (U.GetType() == typeof(System.Windows.Forms.CheckBox))
                                {
                                    System.Windows.Forms.CheckBox CB = (System.Windows.Forms.CheckBox)U;
                                    if (CB.Checked)
                                    {

                                    }
                                }
                                else
                                    continue;

                                DiversityWorkbench.IUserControlQueryCondition C = (DiversityWorkbench.IUserControlQueryCondition)U;
                                string s = "";
                                if (C.Condition().QueryFields.Count > 1)
                                {
                                    bool OK = false;
                                    for (int i = 0; i < C.Condition().QueryFields.Count; i++)
                                    {
                                        if (C.SqlByIndex(i) != "")
                                        {
                                            if (i == 0) s = " AND (";
                                            if (i > 0)
                                            {
                                                if (C.Condition().CombineQueryFieldsWithAnd)
                                                    s = " AND ";
                                                else
                                                    s = " OR ";
                                            }
                                            if (C.Condition().Table == QueryTable && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                            {
                                                if (C.WhereClause() != "")
                                                    s += C.WhereClause();
                                            }
                                            else
                                                s += IdentityColumn + " IN (" + C.SqlByIndex(i) + ")";
                                        }
                                        if (s.Length > 0)
                                        {
                                            SQL += s;
                                            OK = true;
                                        }
                                    }
                                    if (OK) SQL += ")";
                                }
                                else
                                {
                                    if (C.Condition().Table == QueryTable && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                    {
                                        if (C.WhereClause() != "")
                                            s = " AND " + C.WhereClause();
                                    }
                                    else
                                    {
                                        if (C.Condition().Table == QueryTable && C.WhereClause().Length > 0)
                                        {
                                            s = " AND " + C.WhereClause();
                                        }
                                        else if (QueryTable.IndexOf("_Core") > -1
                                            && C.Condition().Table == QueryTable.Substring(0, QueryTable.IndexOf("_Core"))
                                            && C.WhereClause().Length > 0)
                                        {
                                            s = " AND " + C.WhereClause();
                                        }
                                        else
                                        {
                                            if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                                            {
                                                s = " AND " + IdentityColumn + C.WhereClause();
                                            }
                                            else if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.ForeignKeyIsNull)
                                            {
                                                s = " AND " + IdentityColumn + C.WhereClause();
                                            }
                                            else if (C.SQL() != "")
                                                s = " AND " + IdentityColumn + " IN (" + C.SQL() + ")";
                                        }
                                    }
                                    if (s.Length > 0)
                                        QueryConditions.Add(C);
                                }
                            }
                            catch (System.Exception ex) { }
                        }
                    }
                }

                System.Collections.Generic.Dictionary<string, int> TableList = new Dictionary<string, int>();
                System.Collections.Generic.List<DiversityWorkbench.IUserControlQueryCondition> CheckExistenceConditions = new List<IUserControlQueryCondition>();
                foreach (DiversityWorkbench.IUserControlQueryCondition C in QueryConditions)
                {
                    if (C.Condition().CheckIfDataExist != QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                    {
                        if (!TableList.ContainsKey(C.Condition().Table))
                            TableList.Add(C.Condition().Table, 1);
                        else TableList[C.Condition().Table]++;
                    }
                    else if (C.Condition().Operator.Trim().Length > 0)
                        CheckExistenceConditions.Add(C);
                }

                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in TableList)
                {
                    string SqlLocal = "";
                    foreach (DiversityWorkbench.IUserControlQueryCondition C in QueryConditions)
                    {
                        if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                            continue;
                        if (C.Condition().Table == KV.Key)
                        {
                            if (SqlLocal.Length != 0) SqlLocal += " AND ";
                            try
                            {
                                string s = "";
                                if (C.Condition().QueryFields.Count > 1)
                                {
                                    bool OK = false;
                                    for (int i = 0; i < C.Condition().QueryFields.Count; i++)
                                    {
                                        if (C.SqlByIndex(i) != "")
                                        {
                                            if (i == 0) s = " AND (";
                                            if (i > 0)
                                            {
                                                if (C.Condition().CombineQueryFieldsWithAnd)
                                                    s = " AND ";
                                                else
                                                    s = " OR ";
                                            }
                                            if (C.Condition().Table == QueryTable && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                            {
                                                if (C.WhereClause() != "")
                                                    s += C.WhereClause();
                                            }
                                            else
                                                s += C.SqlByIndex(i);
                                            //s += IdentityColumn + " IN (" + C.SqlByIndex(i) + ")";
                                        }
                                        if (s.Length > 0)
                                        {
                                            SqlLocal += s;
                                            OK = true;
                                        }
                                    }
                                    if (OK) SqlLocal += ")";
                                }
                                else
                                {
                                    string BaseTable = QueryTable;
                                    if (BaseTable.IndexOf("_Core") > -1)
                                        BaseTable = BaseTable.Substring(0, BaseTable.IndexOf("_Core"));

                                    if (C.Condition().Table == QueryTable
                                        && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                    {
                                        if (C.WhereClause() != "")
                                        {
                                            s = " AND " + C.WhereClause();
                                        }
                                    }
                                    else if (C.Condition().Table == BaseTable
                                        && this.IfColumnExistsInQueryTable(QueryTable, C.Condition().Column))
                                    {
                                        if (C.WhereClause() != "")
                                        {
                                            s = " AND " + C.WhereClause().Replace("[" + C.Condition().Table + "]", "[" + BaseTable + "_Core]");
                                        }
                                    }
                                    else
                                    {
                                        if (C.Condition().Table == QueryTable && C.WhereClause().Length > 0)
                                        {
                                            s = " AND " + C.WhereClause();
                                        }
                                        else
                                        {
                                            if (C.SQL() != "")
                                            {
                                                if (C.Condition().IsSet)
                                                    s = " AND " + IdentityColumn + " IN (" + C.SQL() + ")";
                                                else if (SqlLocal.Length > 0 && KV.Value > 1)
                                                    s = C.WhereClause();
                                                else if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
                                                    s = "";// AND " + IdentityColumn + C.WhereClause();
                                                else if (C.Condition().CheckIfDataExist == QueryCondition.CheckDataExistence.ForeignKeyIsNull)
                                                    s = " AND " + C.WhereClause();
                                                else
                                                    s = C.SQL();

                                            }
                                            //s = " AND " + IdentityColumn + " IN (" + C.SQL + ")";
                                        }
                                    }
                                    if (s.Length > 0)
                                        SqlLocal += s;
                                    if (SqlLocal.IndexOf(" AND  AND ") > -1)
                                        SqlLocal = SqlLocal.Replace(" AND  AND ", " AND ");
                                }
                            }
                            catch { }
                        }
                    }
                    if (SqlLocal.Length > 0)
                    {
                        if (SqlLocal.StartsWith(" AND "))
                            SQL += SqlLocal;
                        else
                            SQL += " AND " + IdentityColumn + " IN (" + SqlLocal + ")";
                    }
                }

                foreach (DiversityWorkbench.IUserControlQueryCondition C in CheckExistenceConditions)
                {
                    SQL += " AND " + IdentityColumn + C.WhereClause();
                }
            }
            this.SqlWhereClause = SQL;
            if (this.QueryRestriction.Length > 0) SQL += " " + this.QueryRestriction + " ";
            if (this.getQueryRestrictionList().Count > 0)
            {
                foreach (DiversityWorkbench.UserControls.QueryRestrictionItem I in this.getQueryRestrictionList())
                {
                    if (I.TableName == QueryTable)
                        SQL += " AND T." + I.ColumnName + " " + I.Restriction + " ";
                    else
                        SQL += " AND " + QueryTable + "." + IdentityColumn + " IN (SELECT " + IdentityColumn + " FROM " + I.TableName + " WHERE " + I.ColumnName + " " + I.Restriction + ") ";
                }
            }
            if (this._GetNext || this._GetPrevious)
            {
                SQL += this._GetNextRestriction;
            }
            return SQL;
        }

        #endregion

        #region Remember Settings

        #region Administration

        //private static bool _RememberQuerySettingsByDefault = false;
        //public static bool RememberQuerySettingsByDefault { get => _RememberQuerySettingsByDefault; set => _RememberQuerySettingsByDefault = value; }

        private bool _RememberQuerySettingsAvailable = false;
        /// <summary>
        /// If remembering the settigs for teh query is available = Button with pin visible
        /// </summary>
        /// <param name="IsAvailable">True if remembering ist possible</param>
        public void RememberSettingIsAvailable(bool IsAvailable)
        {
            if (DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault)
            {
                _RememberQuerySettingsAvailable = true;
                this.buttonQueryRemember.Visible = false;
            }
            else
            {
                _RememberQuerySettingsAvailable = IsAvailable;
                this.buttonQueryRemember.Visible = IsAvailable;
            }
        }

        public bool RememberQuerySettingsAvailable() { if (DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault) return true; else return _RememberQuerySettingsAvailable; }

        private bool _RememberQuerySettings = true;

        public bool RememberQuerySettings() { if (DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault) return true; else return this._RememberQuerySettings; }

        private string _RememberQuerySettingsIdentifier = "QueryList";

        public string RememberQuerySettingsIdentifier
        {
            get { return _RememberQuerySettingsIdentifier; }
            set { _RememberQuerySettingsIdentifier = value; }
        }

        private void buttonQueryRemember_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault && this._RememberQuerySettings)
                DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault = false;
            this.Remember(!this._RememberQuerySettings);
        }

        private void Remember(bool DoRemember)
        {
            this._RememberQuerySettings = DoRemember;
            if (this._RememberQuerySettings)
                this.buttonQueryRemember.Image = DiversityWorkbench.Properties.Resources.Pin_3;
            else this.buttonQueryRemember.Image = DiversityWorkbench.Properties.Resources.Pin_3Gray;
        }

        // switch this possibility in dependence of e.g. predefined queries
        private void RememberQueryIsEnabled(bool IsEnabled)
        {
            this.buttonQueryRemember.Enabled = IsEnabled;
            if (!IsEnabled)
                this.Remember(false);
        }

        #endregion

        #region File and directory

        public static string RememberDirectory()
        {
            return DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Query);
            //System.IO.DirectoryInfo RememberDirectory = new System.IO.DirectoryInfo(DiversityWorkbench.Settings.ResourcesDirectoryModule() + "\\Query");
            //if (!RememberDirectory.Exists)
            //{
            //    RememberDirectory.Create();
            //    RememberDirectory.Attributes = System.IO.FileAttributes.Directory | System.IO.FileAttributes.Hidden;
            //}
            //return RememberDirectory.FullName;
        }

        private string SettingsTargetDirectory()
        {
            System.IO.DirectoryInfo SettingsTargetDirectory = new System.IO.DirectoryInfo(RememberDirectory() + "\\" + this._RememberQuerySettingsIdentifier);
            if (!SettingsTargetDirectory.Exists)
            {
                SettingsTargetDirectory.Create();
            }
            return SettingsTargetDirectory.FullName;
        }

        public System.Collections.Generic.Dictionary<string, string> TargetSettings()
        {
            System.Collections.Generic.Dictionary<string, string> _Settings = new Dictionary<string, string>();
            System.IO.DirectoryInfo _TargetDirectory = new System.IO.DirectoryInfo(SettingsTargetDirectory());
            foreach (System.IO.FileInfo FI in _TargetDirectory.GetFiles())
            {
                if (FI.Extension == ".xml")
                    _Settings.Add(FI.FullName, FI.Name.Substring(0, FI.Name.Length - 4));
            }
            return _Settings;
        }

        private string SettingsFile()
        {
            string FileName = "";
            try
            {
                FileName = RememberDirectory();
                if (!FileName.EndsWith("\\"))
                    FileName += "\\";
                FileName += this._RememberQuerySettingsIdentifier + ".xml";
            }
            catch (System.Exception ex)
            {
            }
            return FileName;
        }

        private string SettingsFile(string RememberQuerySettingsIdentifier)
        {
            string FileName = "";
            try
            {
                FileName = RememberDirectory();
                if (!FileName.EndsWith("\\") && !RememberQuerySettingsIdentifier.StartsWith("\\"))
                    FileName += "\\";
                FileName += RememberQuerySettingsIdentifier + ".xml";
            }
            catch (System.Exception ex)
            {
            }
            return FileName;
        }

        ///// <summary>
        ///// An additional setting stored in the target directory
        ///// </summary>
        ///// <param name="Name">Name of the file without extension</param>
        ///// <returns>The directory and file name</returns>
        //private string SettingsFile(string Name)
        //{
        //    string FileName = "";
        //    try
        //    {
        //        FileName = SettingsTargetDirectory() + "\\" + Name + ".xml";
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //    return FileName;
        //}

        #endregion

        #region Reading

        public void RememberQueryConditionSettings_ReadFromFile()
        {
            try
            {
                this.RememberQueryConditionSettings_ReadFromFile(this._RememberQuerySettingsIdentifier);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string RememberIdentifierFileName(string RememberIdentifier)
        {
            string FileName = "";
            try
            {
                if (RememberIdentifier.Length == 0)
                    FileName = this.SettingsFile();
                else
                    FileName = this.SettingsFile(RememberIdentifier);
                System.IO.FileInfo F = new System.IO.FileInfo(FileName);
                if (F.Exists)
                    return F.FullName;
                else return "";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return "";
        }

        private int _RememberedIndex = 0;
        public int RememberedIndex() { return this._RememberedIndex; }

        public void RememberQueryConditionSettings_ReadFromFile(string RememberIdentifier, bool ShowMessage = true)
        {
            if (this.IsPredefinedQuery)
                return;
            // not predefined query
            else
            {
                try
                {
                    string FileName = this.RememberIdentifierFileName(RememberIdentifier);
                    if (FileName.Length == 0)
                    {
                        this.Remember(false);
                        return;
                    }
                    else
                        this.Remember(true);
                    // Reading the infos from the file
                    //int SelectedIndex = 0;
                    this.ReadQueryConditionSettings(FileName, ShowMessage);
                    // enter the values in the controls
                    bool ValueIsSet = false;
                    foreach (System.Windows.Forms.Control G in this.panelQueryConditions.Controls)
                    {
                        if (G.GetType() == typeof(System.Windows.Forms.GroupBox))
                        {
                            foreach (System.Windows.Forms.Control U in G.Controls)
                            {
                                try
                                {
                                    DiversityWorkbench.IUserControlQueryCondition C = (DiversityWorkbench.IUserControlQueryCondition)U;
                                    foreach (DiversityWorkbench.QueryCondition QC in this.RememberedQueryConditions())
                                    {
                                        int i = QC.SelectedIndex;
                                        if (C.Condition().Column == QC.Column &&
                                            C.Condition().Table == QC.Table &&
                                            C.Condition().SetCount == QC.SetCount)
                                        {
                                            if (QC.Entity != null &&
                                                QC.Entity.Length > 0 &&
                                                C.Condition().Entity != null &&
                                                C.Condition().Entity.Length > 0 &&
                                                QC.Entity != C.Condition().Entity)
                                                continue;
                                            C.setConditionValues(QC);
                                            ValueIsSet = true;
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    // start the query
                    if (ValueIsSet)
                    {
                        this.buttonQuery_Click(null, null);
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _RememberedQueryConditions;

        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> RememberedQueryConditions()
        {
            if (this._RememberedQueryConditions == null)
            {
                this._RememberedQueryConditions = new List<QueryCondition>();

            }
            return this._RememberedQueryConditions;
        }

        private void ReadQueryConditionSettings(string FileName, bool ShowMessage)
        {
            System.Xml.XmlReaderSettings xSettings = new System.Xml.XmlReaderSettings();
            System.Xml.Linq.XElement SettingsDocument = null;
            System.IO.FileInfo _SettingsFile = null;
            bool RememberIdentifierIsFitting = false;
            RememberedQueryConditions().Clear();
            try
            {
                xSettings.CheckCharacters = false;
                _SettingsFile = new System.IO.FileInfo(FileName);
                if (_SettingsFile.Exists)
                {
                    SettingsDocument = System.Xml.Linq.XElement.Load(System.Xml.XmlReader.Create(_SettingsFile.FullName, xSettings));
                    // Check for correct target
                    try
                    {
                        if (SettingsDocument.HasAttributes)
                        {
                            if (SettingsDocument.Attribute("RememberIdentifier").Value == this._RememberQuerySettingsIdentifier)
                            {
                                RememberIdentifierIsFitting = true;
                                IEnumerable<System.Xml.Linq.XAttribute> Att = SettingsDocument.Attributes();
                                foreach (System.Xml.Linq.XAttribute A in Att)
                                {
                                    switch (A.Name.LocalName)
                                    {
                                        case "SelectedDisplayColumnIndex":
                                            int iIndex = 0;
                                            string Value = SettingsDocument.Attribute("SelectedDisplayColumnIndex").Value;
                                            if (int.TryParse(Value, out iIndex) && iIndex > -1)
                                                this.comboBoxQueryColumn.SelectedIndex = int.Parse(SettingsDocument.Attribute("SelectedDisplayColumnIndex").Value);
                                            break;
                                        case "SelectedIndex":
                                            this._RememberedIndex = int.Parse(SettingsDocument.Attribute("SelectedIndex").Value);
                                            break;
                                        case "UseOptimizing":
                                            this.Optimizing_SetUsage(bool.Parse(SettingsDocument.Attribute("UseOptimizing").Value));
                                            break;
                                        case "Sorting":
                                            this.setSorting(SettingsDocument.Attribute("Sorting").Value);
                                            break;
                                        case "QueryConditionVisibility":
                                            if (SettingsDocument != null && SettingsDocument.HasAttributes && SettingsDocument.Attribute("QueryConditionVisiblity") != null && SettingsDocument.Attribute("QueryConditionVisiblity").Value != null)
                                            {
                                                try { this._QueryConditionVisiblity = SettingsDocument.Attribute("QueryConditionVisiblity").Value; }
                                                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                                            }
                                            break;
                                        case "MaxNumOfResult":
                                            this.MaxNumOfResult = int.Parse(SettingsDocument.Attribute("MaxNumOfResult").Value);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                //this._QueryConditionVisiblity = SettingsDocument.Attribute("QueryConditionVisiblity").Value;
                                //this.MaxNumOfResult = int.Parse(SettingsDocument.Attribute("MaxNumOfResult").Value);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    if (RememberIdentifierIsFitting)
                    {
                        IEnumerable<System.Xml.Linq.XElement> Sets = SettingsDocument.Elements();
                        // Read the entire XML
                        foreach (var Set in Sets)
                        {
                            DiversityWorkbench.QueryCondition QC = new QueryCondition();
                            this.RememberedQueryConditions().Add(QC);
                            if (Set.Name == "QueryCondition")
                            {
                                if (Set.HasAttributes)
                                {
                                    QC.Table = Set.Attribute("Table").Value;
                                    QC.Column = Set.Attribute("Column").Value;
                                    QC.SetCount = int.Parse(Set.Attribute("SetCount").Value);
                                    QC.SetPosition = int.Parse(Set.Attribute("SetPosition").Value);
                                    QC.IsSet = bool.Parse(Set.Attribute("IsSet").Value);
                                    if (Set.Attribute("QueryConditionOperator").Value.Length > 0)
                                        QC.QueryConditionOperator = Set.Attribute("QueryConditionOperator").Value;
                                    else
                                        QC.QueryConditionOperator = "~";
                                    if (QC.IsSet)
                                    {
                                        if (Set.Attribute("SetOperator").Value.Length > 0 && Set.Attribute("SetOperator").Value == "AND")
                                            QC.SetOperator = QueryOperator.AND;
                                        else
                                            QC.SetOperator = QueryOperator.OR;

                                        if (UserControlQueryList.QueryConditionSetOperators.ContainsKey(DiversityWorkbench.Settings.ModuleName))
                                        {
                                            if (UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName].ContainsKey(QC.Table))
                                            {
                                                if (UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][QC.Table].ContainsKey(QC.Column))
                                                    UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][QC.Table][QC.Column] = QC.SetOperator;
                                                else
                                                {
                                                    UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName][QC.Table].Add(QC.Column, QC.SetOperator);
                                                }
                                            }
                                            else
                                            {
                                                System.Collections.Generic.Dictionary<string, DiversityWorkbench.UserControls.UserControlQueryList.QueryOperator> DO = new Dictionary<string, UserControlQueryList.QueryOperator>();
                                                DO.Add(QC.Column, QC.SetOperator);
                                                UserControlQueryList.QueryConditionSetOperators[DiversityWorkbench.Settings.ModuleName].Add(QC.Table, DO);
                                            }
                                        }
                                        else
                                        {
                                            System.Collections.Generic.Dictionary<string, DiversityWorkbench.UserControls.UserControlQueryList.QueryOperator> DO = new Dictionary<string, UserControlQueryList.QueryOperator>();
                                            DO.Add(QC.Column, QC.SetOperator);
                                            System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityWorkbench.UserControls.UserControlQueryList.QueryOperator>> DDO = new Dictionary<string, Dictionary<string, UserControlQueryList.QueryOperator>>();
                                            DDO.Add(QC.Table, DO);
                                            UserControlQueryList.QueryConditionSetOperators.Add(DiversityWorkbench.Settings.ModuleName, DDO);
                                        }
                                    }
                                    QC.SelectFromList = bool.Parse(Set.Attribute("SelectFromList").Value);
                                    QC.SelectFromHierachy = bool.Parse(Set.Attribute("SelectFromHierachy").Value);
                                    QC.IsDate = bool.Parse(Set.Attribute("IsDate").Value);
                                    QC.IsYear = bool.Parse(Set.Attribute("IsYear").Value);
                                    QC.IsBoolean = bool.Parse(Set.Attribute("IsBoolean").Value);
                                }
                                foreach (var T in Set.Elements())
                                {
                                    if (T.Name == "Value")
                                    {
                                        if (T.Value.IndexOf("\n") > -1 && T.Value.IndexOf("\r\n") == -1)
                                            QC.Value = T.Value.Replace("\n", "\r\n").Replace("\r\r", "\r");
                                        else
                                            QC.Value = T.Value;
                                    }
                                    if (T.Name == "SelectedIndex")
                                        QC.SelectedIndex = int.Parse(T.Value);
                                    if (T.Name == "UpperValue")
                                        QC.UpperValue = T.Value;
                                    if (T.Name == "CheckState")
                                    {
                                        switch (T.Value)
                                        {
                                            case "0":
                                                QC.CheckState = CheckState.Unchecked;
                                                break;
                                            case "1":
                                                QC.CheckState = CheckState.Checked;
                                                break;
                                            case "2":
                                                QC.CheckState = CheckState.Indeterminate;
                                                break;
                                        }
                                    }
                                    if (T.Name == "Day")
                                        QC.Day = int.Parse(T.Value);
                                    if (T.Name == "Month")
                                        QC.Month = int.Parse(T.Value);
                                    if (T.Name == "Year")
                                        QC.Year = int.Parse(T.Value);
                                    if (T.Name == "ModuleValues")
                                    {
                                        string BaseURL = "http://" + T.Attribute("BaseURL").Value;
                                        IEnumerable<System.Xml.Linq.XElement> Values = T.Elements();
                                        if (QC.dtValues == null)
                                        {
                                            QC.dtValues = new DataTable();
                                            System.Data.DataColumn dcURL = new DataColumn("URL", typeof(string));
                                            System.Data.DataColumn dcDisplay = new DataColumn("Display", typeof(string));
                                            QC.dtValues.Columns.Add(dcURL);
                                            QC.dtValues.Columns.Add(dcDisplay);
                                        }
                                        foreach (var Value in Values)
                                        {
                                            /// The added ID_ must be removed and the BaseURL must be added to get a correct URL
                                            string URL = BaseURL + Value.Name.ToString().Substring(3);
                                            string Display = Value.Value.ToString();
                                            System.Data.DataRow R = QC.dtValues.NewRow();
                                            R[0] = URL;
                                            R[1] = Display;
                                            QC.dtValues.Rows.Add(R);
                                        }
                                    }
                                    if (T.Name == "Entity")
                                        QC.Entity = T.Value;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ShowMessage)
                            System.Windows.Forms.MessageBox.Show("Settings file does not correspond to " + this._RememberQuerySettingsIdentifier);
                    }
                }
            }
            catch (System.Exception ex)
            { }
            finally
            {
                xSettings.Reset();
                xSettings = null;
                SettingsDocument.RemoveAll();
                SettingsDocument = null;
                _SettingsFile = null;
            }
        }
        /*
         *             this.buttonQueryConditionOperator.Text = Condition.QueryConditionOperator;
            if (Condition.Value.Length > 0)
                this.textBoxQueryCondition.Text = Condition.Value;
            if (Condition.SelectFromList || Condition.SelectFromHierachy)
                this.comboBoxQueryCondition.SelectedIndex = Condition.SelectedIndex;
            if (Condition.IsDate || Condition.IsYear)
            {
                if (Condition.Day != null)
                    this.maskedTextBoxQueryConditionDay.Text = Condition.Day.ToString();
                if (Condition.Month != null)
                    this.maskedTextBoxQueryConditionMonth.Text = Condition.Month.ToString();
                if (Condition.Year != null)
                    this.maskedTextBoxQueryConditionYear.Text = Condition.Year.ToString();
            }
            if (this.buttonQueryConditionOperator.Text == "—")
                this.textBoxQueryConditionUpper.Text = Condition.UpperValue;
            if (Condition.IsBoolean)
                this.checkBoxQueryCondition.CheckState = Condition.CheckState;
            this.comboBoxQueryConditionOperator.Text = Condition.QueryConditionOperator;

         * */

        #endregion

        #region Write settings

        public bool RememberQueryConditionSettings_RemoveFile()
        {
            try
            {
                string FileName = SettingsFile();
                System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
                if (FI.Exists)
                    FI.Delete();
                return true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool RememberQueryConditionSettings_RemoveFile(string Name)
        {
            try
            {
                string FileName = SettingsFile(Name);
                System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
                if (FI.Exists)
                    FI.Delete();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public string RememberQueryConditionSettings_SaveToFile(string Form = "")
        {
            string FileName = "";
            if (Form.Length == 0) FileName = SettingsFile();
            else FileName = SettingsFile(Form);
            this.WriteRememberConditionSettings(FileName, true);
            return FileName;
        }

        /// <summary>
        /// Save an additional setting for the target
        /// </summary>
        /// <param name="Name">Name of the additional setting in the target directory</param>
        /// <returns>The full path and file name of the setting</returns>
        public string RememberQueryConditionSettings_SaveToFile(string Name, bool ShowMessage = true)
        {
            string FileName = this.SettingsFile(Name);
            this.WriteRememberConditionSettings(FileName, ShowMessage);
            return FileName;
        }

        private void WriteRememberConditionSettings(string FileName, bool ShowMessage)
        {
            if (FileIsLocked(FileName))
                return;

            System.Xml.XmlWriter W = null;
            try
            {
                this.OptimizingObjects_Reset();
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;
                W = System.Xml.XmlWriter.Create(FileName, settings);
                W.WriteStartDocument();
                W.WriteStartElement("QueryList");
                W.WriteAttributeString("RememberIdentifier", this._RememberQuerySettingsIdentifier);
                W.WriteAttributeString("SelectedDisplayColumnIndex", this.comboBoxQueryColumn.SelectedIndex.ToString());
                W.WriteAttributeString("SelectedIndex", this.listBoxQueryResult.SelectedIndex.ToString());
                W.WriteAttributeString("UseOptimizing", this.OptimizingIsUsed().ToString());
                W.WriteAttributeString("Sorting", this._Sorting);
                W.WriteAttributeString("QueryConditionVisibility", this._QueryConditionVisiblity);
                W.WriteAttributeString("MaxNumOfResult", this.MaxNumOfResult.ToString());
                // save if remember settings is activ
                W.WriteAttributeString("RememberQuerySettings", this.RememberQuerySettings().ToString());

                foreach (DiversityWorkbench.QueryCondition QC in this.CurrentQueryConditions)
                {
                    W.WriteStartElement("QueryCondition");
                    W.WriteAttributeString("Table", QC.Table);
                    W.WriteAttributeString("Column", QC.Column);
                    W.WriteAttributeString("IsSet", QC.IsSet.ToString());
                    W.WriteAttributeString("SetOperator", QC.SetOperator.ToString());
                    W.WriteAttributeString("SetCount", QC.SetCount.ToString());
                    W.WriteAttributeString("SetPosition", QC.SetPosition.ToString());
                    W.WriteAttributeString("QueryConditionOperator", QC.QueryConditionOperator);
                    W.WriteAttributeString("SelectFromList", QC.SelectFromList.ToString());
                    W.WriteAttributeString("SelectFromHierachy", QC.SelectFromHierachy.ToString());
                    W.WriteAttributeString("IsDate", QC.IsDate.ToString());
                    W.WriteAttributeString("IsYear", QC.IsYear.ToString());
                    W.WriteAttributeString("IsBoolean", QC.IsBoolean.ToString());
                    if (QC.Value != null && QC.Value.Length > 0)
                    {
                        W.WriteElementString("Value", QC.Value);
                    }
                    if (QC.UpperValue != null && QC.UpperValue.Length > 0 && (QC.QueryConditionOperator == "—" || QC.QueryType == QueryCondition.QueryTypes.Module))
                    {
                        W.WriteElementString("UpperValue", QC.UpperValue);
                    }
                    if ((QC.SelectFromList || QC.SelectFromHierachy) && QC.SelectedIndex > -1)
                    {
                        W.WriteElementString("SelectedIndex", QC.SelectedIndex.ToString());
                    }
                    if (QC.IsBoolean)
                    {
                        W.WriteElementString("CheckState", QC.CheckState.ToString());
                    }
                    if (QC.Day != null)
                    {
                        W.WriteElementString("Day", QC.Day.ToString());
                    }
                    if (QC.Month != null)
                    {
                        W.WriteElementString("Month", QC.Month.ToString());
                    }
                    if (QC.Year != null)
                    {
                        W.WriteElementString("Year", QC.Year.ToString());
                    }
                    if (QC.Entity != null && QC.Entity.Length > 0)
                    {
                        W.WriteElementString("Entity", QC.Entity);
                    }
                    if (QC.QueryType == QueryCondition.QueryTypes.Module && QC.dtValues != null && QC.dtValues.Rows.Count > 0)
                    {
                        W.WriteStartElement("ModuleValues");
                        /// the sign : is not allowed here - to avoid it, the content is reduced to the essential parts and redundant parts are added when recostructing the content
                        string BaseURL = QC.dtValues.Rows[0][0].ToString();
                        BaseURL = BaseURL.Substring(0, BaseURL.LastIndexOf('/'));
                        BaseURL = BaseURL.Substring(BaseURL.IndexOf("://") + 3);
                        W.WriteAttributeString("BaseURL", BaseURL + "/");
                        foreach (System.Data.DataRow R in QC.dtValues.Rows)
                        {
                            string ID = R[0].ToString();
                            ID = ID.Substring(ID.LastIndexOf('/') + 1);
                            /// A number is not allowed as a name - so ID_ is added and cut away when reconstructing the content
                            W.WriteElementString("ID_" + ID, R[1].ToString());
                        }
                        W.WriteEndElement();
                    }
                    W.WriteEndElement();//QueryCondition
                }

                W.WriteEndElement();//QueryList
                W.WriteEndDocument();
                W.Flush();
                W.Close();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //if (ShowMessage)
                //    System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
        }

        private bool FileIsLocked(string file)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(file);
            return FileIsLocked(fi);
        }


        private bool FileIsLocked(System.IO.FileInfo file)
        {
            try
            {
                // Toni 20210910: If file doe not exist, it is not locked (see comment of Markus at exception)
                if (!file.Exists)
                    return false;
                using (System.IO.FileStream stream = file.Open(System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (System.IO.IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        #endregion

        #region Context menu
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.RememberQuerySettings())
                {
                    System.Windows.Forms.MessageBox.Show("Remember settings is off. Please activate remembering for saving the settings");
                    return;
                }
                else
                {
                    if (this.RememberQueryConditionSettings_RemoveFile())
                    {
                        this.RememberQueryConditionSettings_SaveToFile();
                        this._RememberedQueryConditions = null;
                        this.RememberQueryConditionSettings_ReadFromFile();
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripMenuItemSetAsDefault_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault = true;
                this.RememberSettingIsAvailable(true);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #endregion

        #region Order by prefix

        private string _OrderByPrefixColumn;
        private string _OrderByPrefixTable;
        private System.Collections.Generic.Dictionary<string, string> _OrderByPrefixes;

        private void AddPrefixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.Forms.FormFunctions.SqlServerVersion() <= 10)
                System.Windows.Forms.MessageBox.Show("You need SqlServer version 12 to use this function");
            else
            {
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Prefix", "Please enter a prefix that should not be included for sorting", "");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityWorkbench.Settings.OrderByPrefixAdd(f.String);
                    this.SetQueryDisplayColumns(this.DisplayColumns, this._OrderByPrefixColumn, this._OrderByPrefixTable);
                }
            }
        }

        private void RemovePrefixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix != null)
            {
                System.Collections.Generic.List<string> PP = new List<string>();
                foreach (string P in DiversityWorkbench.WorkbenchSettings.Default.OrderByPrefix)
                    PP.Add(P);
                if (PP.Count > 0)
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(PP, "Remove Prefix", "Please select the prefix that should be removed", true);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        DiversityWorkbench.Settings.OrderByPrefixRemove(f.SelectedString);
                        this.SetQueryDisplayColumns(this.DisplayColumns, this._OrderByPrefixColumn, this._OrderByPrefixTable);
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("No prefix defined so far", "No Prefix", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                System.Windows.Forms.MessageBox.Show("No prefix defined so far", "No Prefix", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Many order by columns

        /**
         * ToDos bei Einbindung am Beispiel von DC
         * 
         * in Formular bei initQuery:
         * this.userControlQueryList.ManyOrderByColumns_Allow(true, DiversityWorkbench.Settings.ModuleName, "FormCollectionSpecimen", true);
         * 
         * in Copy Funktion:
         *  if (this.userControlQueryList.ManyOrderByColumns())
            {
                string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(SpecimenID);
                this.userControlQueryList.AddListItem(SpecimenID, Display);
            }
            else
                this.userControlQueryList.AddListItem(SpecimenID, AccessionNumber);

         * 
         * in Create Funktion:
         *  if (this.userControlQueryList.ManyOrderByColumns())
            {
                string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(SpecimenID);
                this.userControlQueryList.AddListItem(SpecimenID, Display);
            }
            else
            {
            ...

         * if missing new function:
         *         private void initQueryOptimizing()
            {
                DiversityWorkbench.UserControls.UserControlQueryList.QueryMainTable = "CollectionSpecimen_Core2";
                this.userControlQueryList.QueryMainTableLocal = "CollectionSpecimen_Core2";
            }
         * ref in function initQuery()
         * 
         * in FormClosing:
         * if (this.userControlQueryList.ManyOrderByColumns_Allow())
            {
                DiversityWorkbench.Settings.ManyOrderColumnSave(DiversityWorkbench.Settings.ModuleName, "FormCollectionSpecimen", this.userControlQueryList.ManyOrderByColumns_Widths());
            }
         * 
         * in initForm()
                this.userControlQueryList.ManyOrderByColumns_Allow(true);
                this.userControlQueryList.OptimizingAvailable(true);
                this.userControlQueryList.ManyOrderByColumns_InitControls();
         *
         * 
         * * */

        #region Parameter

        private Font _FontDefault = new System.Drawing.Font("Airal", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        private Font _FontManyOrderByColumns = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        private bool _ManyOrderByColumns_Allow = false;
        //private string _ManyOrderColumns_Form = "";
        private int _ManyOrderByColumns_FirstColumnWidth = 10;

        private System.Collections.Generic.Dictionary<string, string> _ManyOrderByColumns_TableAliases;
        private System.Collections.Generic.Dictionary<string, OrderBy> _ManyOrderByColumns_Controls;
        //private System.Collections.Generic.SortedDictionary<int, string> _ManyOrderByColumns_ControlSequence;
        private System.Collections.Generic.SortedDictionary<string, int> _ManyOrderByColumns_Sequence;

        #endregion

        #region SQL

        public string ManyOrderByColumns_DisplayText(int ID)
        {
            string Display = "";
            try
            {
                string Col = "SELECT " + this.ManyOrderByColumns_DisplayColumnClause();
                string From = this.ManyOrderByColumns_JoinClause(ID, OptimizingColumns[OptimizingColumn.IdentityColumn]);
                string Select = Col + From;
                // ToDo: UserControlQueryList.QueryMainTable wird bei Suche in Modul umgeschaltet - Dict mit Modulen anlegen und diese dann als Quelle nutzen
                if (UserControlQueryList.QueryMainTable == this.QueryMainTableLocal)
                {
                    if (From.IndexOf(" WHERE ") > -1) Select += " AND ";
                    else Select += " WHERE ";
                    Select += " T." + OptimizingColumns[OptimizingColumn.IdentityColumn] + " = " + ID.ToString();
                }
                Display = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Select);
                if (Display.Length == 0)
                {
                    string SQL = "SELECT " + this.ManyOrderByColumns_DisplayColumnClause() + " FROM " + OptimizingColumns[OptimizingColumn.Table] + " AS T " + this.ManyOrderByColumns_FromClause() + " WHERE " + this.ManyOrderByColumns_WhereClause() + " AND T." + OptimizingColumns[OptimizingColumn.IdentityColumn] + " = " + ID.ToString();
                    //SQL = "SELECT " + this.ManyOrderByColumns_DisplayColumnClause() + this.OptimizedQueryFromClause() + " WHERE T." + OptimizingColumns[OptimizingColumn.IdentityColumn] + " = " + ID.ToString(); ;
                    Display = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Display;
        }

        /// <summary>
        /// Getting Displaytext for a item - funktioniert noch nicht - nimmt ersten Datensatz
        /// </summary>
        /// <param name="Dataset"></param>
        /// <param name="Table"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public string ManyOrderByColumns_DisplayTextForRow(System.Data.DataSet Dataset, string Table, int Index)
        {
            string Display = "";
            try
            {
                string SQL = "SELECT " + this.ManyOrderByColumns_DisplayColumnClause() + " FROM " + OptimizingColumns[OptimizingColumn.Table] + " AS T " + this.ManyOrderByColumns_FromClause() + " WHERE " + this.ManyOrderByColumns_WhereClause() + " AND T." + OptimizingColumns[OptimizingColumn.IdentityColumn] + " = " + ID.ToString();
                // Get PK from Table
                DiversityWorkbench.Data.Table table = new Data.Table(Table);
                foreach (string PK in table.PrimaryKeyColumnList)
                {
                    SQL += " AND T." + PK + " = '" + Dataset.Tables[Table].Rows[Index][PK].ToString() + "'";
                }
                Display = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Display;
        }

        private string ManyOrderByColumns_ColumnClause() //string DisplayAlias, string IdentityColumnAlias)
        {
            _ManyOrderByColumns_TableAliases = null;
            string SQL = this.ManyOrderByColumns_DisplayColumnClause();
            SQL += this.ManyOrderByColumns_OrderColumnClause();
            return SQL;
        }

        private string ManyOrderByColumns_DisplayColumnClause()
        {
            string SQL = "";
            try
            {
                string Column = this.ManyOrderByColumns_DisplayColumnSQL(OptimizingColumns[OptimizingColumn.Table], OptimizingColumns[OptimizingColumn.DisplayColumn], OptimizingColumns[OptimizingColumn.DisplayAlias]);
                //SQL = "CAST(" + this.QueryStringForColumn(OptimizingColumns[OptimizingColumn.DisplayAlias], OptimizingColumns[OptimizingColumn.IdentityAlias], _ManyOrderByColumns_FirstColumnWidth) + " AS nchar(" + _ManyOrderByColumns_FirstColumnWidth.ToString() + ")) ";
                SQL = "CAST(CASE WHEN " + OptimizingColumns[OptimizingColumn.DisplayAlias] + " IS NULL THEN '' ELSE " + Column + " END AS nchar(" + _ManyOrderByColumns_FirstColumnWidth.ToString() + ")) ";
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KF in this.ManyOrderByColumns_Controls)
                {
                    // Marku 27.3.23: Bugfix for columns with reserverd names e.g. Order
                    string ColAlias = ManyOrderByColumns_TableAliases()[KF.Value.QueryOrderColumn.TableName] + ".[" + KF.Value.QueryOrderColumn.OrderColumn + "]";
                    ColAlias = this.ManyOrderByColumns_DisplayColumnSQL(KF.Value.QueryOrderColumn.TableName, KF.Value.QueryOrderColumn.OrderColumn, ColAlias);
                    SQL += " + ' | ' +  CAST(CASE WHEN " + ColAlias + " IS NULL THEN '' ELSE " + ColAlias + " END AS nchar(" + KF.Value.QueryOrderColumnControl.Width.ToString() + ")) ";
                }
                SQL += " AS Display";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return SQL;
        }

        private string ManyOrderByColumns_DisplayColumnSQL(string Table, string Column, string Alias)
        {
            string SQL = "  select c.DATA_TYPE from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = '" + Table + "' and c.COLUMN_NAME = '" + Column + "'";
            string DATA_TYPE = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
            if (DATA_TYPE.IndexOf("date") > -1)
                return "CONVERT(varchar(20), " + Alias + ", 120)";
            else
                return Alias;
        }

        private string ManyOrderByColumns_OrderColumnClause()
        {
            string SQL = "";
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KF in this.ManyOrderByColumns_Controls)
                {
                    SQL += " , " + ManyOrderByColumns_TableAliases()[KF.Value.QueryOrderColumn.TableName] + ".[" + KF.Value.QueryOrderColumn.OrderColumn + "]";
                }
                SQL += ", " + OptimizingColumns[OptimizingColumn.DisplayAlias] + " ";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return SQL;
        }

        private string ManyOrderByColumns_FromClause()
        {
            string SQL = "";
            try
            {
                System.Collections.Generic.List<string> done = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                {
                    if (!UserControlQueryList.TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName)
                        && KV.Value.QueryOrderColumn.TableName != UserControlQueryList._QueryMainTableOptimizing
                        && !done.Contains(KV.Value.QueryOrderColumn.TableName))
                    {
                        SQL += ", " + KV.Value.QueryOrderColumn.TableName + " AS " + ManyOrderByColumns_TableAliases(KV.Value.QueryOrderColumn.TableName)[KV.Value.QueryOrderColumn.TableName];// (UserControlQueryList.TableAliases.Count + i).ToString();
                        done.Add(KV.Value.QueryOrderColumn.TableName);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return SQL;
        }

        private string ManyOrderByColumns_JoinClause(int? ID = null, string PkColumnName = "")
        {
            //string SQL = " FROM " + UserControlQueryList.QueryMainTable + " AS T "; //this.OptimizedQueryFromClause();
            string SQL = " FROM " + this.QueryMainTableLocal + " AS T "; //this.OptimizedQueryFromClause();
            System.Collections.Generic.Dictionary<string, string> TabAli = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in UserControlQueryList.TableAliases)
                TabAli.Add(KV.Key, KV.Value);
            foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
            {
                string Table = KV.Value.QueryOrderColumn.TableName;
                if (KV.Value.QueryOrderColumn.TableName != null && !TabAli.ContainsKey(KV.Value.QueryOrderColumn.TableName) && KV.Value.QueryOrderColumn.TableName != this.QueryMainTableLocal)
                {
                    TabAli.Add(KV.Value.QueryOrderColumn.TableName, this.ManyOrderByColumns_TableAliases()[KV.Value.QueryOrderColumn.TableName]);
                }
            }
            DiversityWorkbench.Data.Table tableMain = new Data.Table(UserControlQueryList.QueryMainTable);
            // may interfere with previous query - check PK to correct
            if (tableMain.PrimaryKeyColumnList.Count == 0)
            {
                tableMain = new Data.Table(this._QueryMainTableLocal);
            }
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in TabAli)
            {
                DiversityWorkbench.Data.Table tab = new Data.Table(KV.Key);
                if (KV.Key == this.QueryMainTableLocal)
                    SQL += " RIGHT OUTER JOIN ";
                else
                    SQL += " LEFT OUTER JOIN ";
                SQL += KV.Key + " AS " + KV.Value + " ON ";
                int i = 0;
                foreach (string pk in tab.PrimaryKeyColumnList)
                {
                    if (tableMain.PrimaryKeyColumnList.Contains(pk))
                    {
                        if (i > 0) SQL += " AND ";
                        SQL += "T." + pk + " = " + KV.Value + "." + pk;
                        //if (ID != null && PkColumnName == pk && KV.Key == this.QueryMainTableLocal)
                        //    SQL += " AND T." + pk + " = " + ID.ToString();
                    }
                    i++;
                }
            }
            if (TabAli.ContainsKey(this.QueryMainTableLocal) && PkColumnName.Length > 0 && ID != null)
                SQL += " WHERE " + TabAli[this.QueryMainTableLocal] + "." + PkColumnName + " = " + ID.ToString();
            return SQL;

            SQL = "SELECT " + this.ManyOrderByColumns_DisplayColumnClause() + SQL;


            try
            {
                System.Collections.Generic.List<string> done = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                {
                    if (!UserControlQueryList.TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName)
                        && KV.Value.QueryOrderColumn.TableName != UserControlQueryList._QueryMainTableOptimizing
                        && !done.Contains(KV.Value.QueryOrderColumn.TableName))
                    {
                        SQL += " LEFT OUTER JOIN " + KV.Value.QueryOrderColumn.TableName + " AS " + ManyOrderByColumns_TableAliases(KV.Value.QueryOrderColumn.TableName)[KV.Value.QueryOrderColumn.TableName];// (UserControlQueryList.TableAliases.Count + i).ToString();
                        SQL += " ON " + this.ManyOrderByColumns_WhereClause(KV.Value.QueryOrderColumn.TableName);

                        SQL += ", " + KV.Value.QueryOrderColumn.TableName + " AS " + ManyOrderByColumns_TableAliases(KV.Value.QueryOrderColumn.TableName)[KV.Value.QueryOrderColumn.TableName];// (UserControlQueryList.TableAliases.Count + i).ToString();
                        done.Add(KV.Value.QueryOrderColumn.TableName);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return SQL;
        }

        private string ManyOrderByColumns_WhereClause(string TableName = "")
        {
            string SQL = "";
            try
            {
                int i = 1;
                System.Collections.Generic.Dictionary<string, string> Aliases = this.ManyOrderByColumns_TableAliases();
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                {
                    if (TableName.Length > 0 && KV.Value.QueryOrderColumn.TableName != TableName)
                        continue;
                    if (!UserControlQueryList.TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName) && KV.Value.QueryOrderColumn.TableName != UserControlQueryList._QueryMainTableOptimizing)
                    {
                        if (SQL.Length > 0) SQL += " AND ";
                        SQL += Aliases[KV.Value.QueryOrderColumn.TableName] + "." + KV.Value.QueryOrderColumn.IdentityColumn + " = T." + KV.Value.QueryOrderColumn.IdentityColumn;
                        i++;
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return SQL;
        }

        private string ManyOrderByColumns_OrderByClause()
        {
            string SQL = "";
            try
            {
                System.Collections.Generic.Dictionary<string, string> Aliases = this.ManyOrderByColumns_TableAliases();
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += Aliases[KV.Value.QueryOrderColumn.TableName] + ".[" + KV.Value.QueryOrderColumn.OrderColumn + "] " + KV.Value.QueryOrderColumnControl.Sorting;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return SQL;
        }

        private System.Collections.Generic.Dictionary<string, string> ManyOrderByColumns_TableAliases(string TableName = "")
        {
            if (_ManyOrderByColumns_TableAliases == null)
            {
                _ManyOrderByColumns_TableAliases = new Dictionary<string, string>();
            }
            if (_ManyOrderByColumns_TableAliases.Count == 0 || (TableName.Length > 0 && !_ManyOrderByColumns_TableAliases.ContainsKey(TableName)))
            {
                try
                {
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                    {
                        if (!UserControlQueryList.TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName) && KV.Value.QueryOrderColumn.TableName != UserControlQueryList._QueryMainTableOptimizing)
                        {
                            if (!_ManyOrderByColumns_TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName))
                            {
                                _ManyOrderByColumns_TableAliases.Add(KV.Value.QueryOrderColumn.TableName, "M" + i.ToString());
                                i++;
                            }
                        }
                        else if (!_ManyOrderByColumns_TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName))
                        {
                            if (UserControlQueryList.TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName))
                                _ManyOrderByColumns_TableAliases.Add(KV.Value.QueryOrderColumn.TableName, UserControlQueryList.TableAliases[KV.Value.QueryOrderColumn.TableName]);
                            else if (KV.Value.QueryOrderColumn.TableName == UserControlQueryList._QueryMainTableOptimizing && !_ManyOrderByColumns_TableAliases.ContainsKey(KV.Value.QueryOrderColumn.TableName))
                                _ManyOrderByColumns_TableAliases.Add(KV.Value.QueryOrderColumn.TableName, "T");
                        }
                        else
                        { }
                    }
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            if (this._QueryMainTableLocal != null
                && !_ManyOrderByColumns_TableAliases.ContainsKey(this._QueryMainTableLocal)
                && UserControlQueryList.QueryMainTable != this._QueryMainTableLocal
                && !UserControlQueryList.TableAliases.ContainsKey(this._QueryMainTableLocal))
            {
                _ManyOrderByColumns_TableAliases.Add(this._QueryMainTableLocal, "M" + _ManyOrderByColumns_TableAliases.Count.ToString());
            }
            return _ManyOrderByColumns_TableAliases;
        }

        private string ManyOrderByColumns_IDcolumn(string IdentityColumn)
        {
            string SQL = "";
            try
            {
                string MainTableAlias = "";
                if (ManyOrderByColumns_TableAliases().ContainsKey(this._QueryMainTableLocal))
                {
                    MainTableAlias = ManyOrderByColumns_TableAliases()[this._QueryMainTableLocal];
                    if (QueryStringWhereClause.IndexOf(" AS " + MainTableAlias) > -1)
                        SQL = " CASE WHEN T." + IdentityColumn + " IS NULL THEN " + MainTableAlias + "." + IdentityColumn + " ELSE T." + IdentityColumn + " END ";
                    else
                        SQL = " T." + IdentityColumn + " ";
                }
                else
                    SQL = " T." + IdentityColumn + " ";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); SQL = ""; }
            return SQL;

        }

        #endregion

        #region Setting the controls

        public void ManyOrderByColumns_SetSpacer()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
            {
                KV.Value.QueryOrderColumnControl.setSpacer(this);
            }
        }

        public void ManyOrderByColumns_Init() //string Module, string Form)
        {
            if (!this.RememberQuerySettings())
                return;
            if (this._UsedByForm == null || this._UsedByForm.Length == 0 || this._UsedByModule == null || this._UsedByModule.Length == 0)
                return;
            //if (Form.Length == 0)
            //    Form = _ManyOrderColumns_Form;
            //else if (Form.Length > 0 && _ManyOrderColumns_Form.Length == 0)
            //    _ManyOrderColumns_Form = Form;
            try
            {
                if (DiversityWorkbench.Settings.ManyOrderColumns(_UsedByModule, this._UsedByForm).Count > 0)
                {
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, int> KV in DiversityWorkbench.Settings.ManyOrderColumns(_UsedByModule, this._UsedByForm))
                    {
                        if (KV.Key.Length == 0)
                            continue;
                        if (i == 0)
                        {
                            if (this._DtDisplayColumns != null)
                            {
                                int ii = 0;
                                foreach (System.Data.DataRow R in this._DtDisplayColumns.Rows)
                                {
                                    if (R[1].ToString() == KV.Key)
                                    {
                                        this.comboBoxQueryColumn.SelectedIndex = ii;
                                        break;
                                    }
                                    ii++;
                                }
                            }
                            this.maskedTextBoxOrderByColumnWidth.Text = KV.Value.ToString();
                        }
                        else
                        {
                            OrderBy orderBy = new OrderBy();
                            orderBy.DisplayText = KV.Key;
                            //orderBy.Index = i;

                            // getting the column
                            QueryDisplayColumn queryDisplayColumn = this.ManyOrderByColumns_GetColumn(KV.Key);
                            orderBy.QueryOrderColumn = queryDisplayColumn;

                            // getting the control
                            UserControls.UserControlQueryOrderColumn orderColumnControl = new UserControls.UserControlQueryOrderColumn(queryDisplayColumn, this);
                            orderColumnControl.Width = KV.Value;
                            orderBy.QueryOrderColumnControl = orderColumnControl;

                            if (!this.ManyOrderByColumns_Controls.ContainsKey(KV.Key))
                            {
                                this.ManyOrderByColumns_Controls.Add(KV.Key, orderBy);
                                // Markus 26.4.23: Check content before insert
                                if (!this.ManyOrderByColumns_Sequence.ContainsKey(KV.Key))
                                    this.ManyOrderByColumns_Sequence.Add(KV.Key, this.ManyOrderByColumns_Sequence.Count * 2);
                            }
                        }
                        i++;
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private OrderBy ManyOrderByColumns_TopOrderBy()
        {
            OrderBy TopOrderBy = new OrderBy();
            TopOrderBy.DisplayText = OptimizingColumns[OptimizingColumn.DisplayText];
            TopOrderBy.QueryOrderColumn = SelectedQueryDisplayColumn;
            TopOrderBy.QueryOrderColumnControl = new UserControls.UserControlQueryOrderColumn(SelectedQueryDisplayColumn, this);
            TopOrderBy.QueryOrderColumnControl.Width = int.Parse(this.maskedTextBoxOrderByColumnWidth.Text);
            return TopOrderBy;
        }

        public void ManyOrderByColumns_ChangePosition(string Column, bool Up)
        {
            int Change = 0;
            if (this.ManyOrderByColumns_Sequence.ContainsKey(Column))
            {
                if (this.ManyOrderByColumns_Sequence[Column] == (this.ManyOrderByColumns_Sequence.Count - 1) * 2 && !Up)
                    return;

                bool SwitchTopColumn = false;
                if (this.ManyOrderByColumns_Sequence[Column] == 0 && Up)
                {
                    SwitchTopColumn = true;
                }

                if (SwitchTopColumn) Change = -2;
                else if (Up) Change = this.ManyOrderByColumns_Sequence[Column] - 3;
                else Change = this.ManyOrderByColumns_Sequence[Column] + 3;
                this.ManyOrderByColumns_Sequence[Column] = Change;

                System.Collections.Generic.SortedDictionary<int, string> Sorting = new SortedDictionary<int, string>();

                // reset sequence
                foreach (System.Collections.Generic.KeyValuePair<string, int> KV in this.ManyOrderByColumns_Sequence)
                {
                    if (KV.Key.Length == 0)
                        continue;
                    if (SwitchTopColumn && KV.Value == -2)
                    {
                        if (OptimizingColumns[OptimizingColumn.DisplayText].Length == 0)
                        {
                            OptimizingColumns[OptimizingColumn.DisplayText] = this._DtDisplayColumns.Rows[this.comboBoxQueryColumn.SelectedIndex][0].ToString();
                            //continue;
                        }
                        Sorting.Add(-2, OptimizingColumns[OptimizingColumn.DisplayText]);
                    }
                    else
                    {
                        int Key = KV.Value;
                        while (Sorting.ContainsKey(Key)) Key++;
                        Sorting.Add(Key, KV.Key);
                    }
                }
                this.ManyOrderByColumns_Sequence.Clear();

                // reset controls
                string TopOrderColumn = "";
                System.Collections.Generic.Dictionary<string, OrderBy> keyValuePairs = new Dictionary<string, OrderBy>();
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                {
                    if (SwitchTopColumn && i == 0)
                    {
                        keyValuePairs.Add(OptimizingColumns[OptimizingColumn.DisplayText], ManyOrderByColumns_TopOrderBy());
                        TopOrderColumn = KV.Key;
                        this.maskedTextBoxOrderByColumnWidth.Text = this.ManyOrderByColumns_Widths()[TopOrderColumn].ToString();
                    }
                    else
                    {
                        keyValuePairs.Add(KV.Key, KV.Value);
                    }
                    i++;
                }
                ManyOrderByColumns_Controls.Clear();

                if (SwitchTopColumn)
                {
                    for (int ii = 0; ii < this.DisplayColumns.Length; ii++)
                    {
                        if (this.DisplayColumns[ii].DisplayText == TopOrderColumn)
                        {
                            this.comboBoxQueryColumn.SelectedIndex = ii;
                            break;
                        }
                    }
                }

                // reset widths
                int TopOrderColumnWidth = 0;
                if (SwitchTopColumn)
                {
                    if (this.ManyOrderByColumns_Widths().ContainsKey(TopOrderColumn))
                    {
                        TopOrderColumnWidth = this.ManyOrderByColumns_Widths()[TopOrderColumn];

                    }
                }

                // filling dicts with new sequence
                i = 0;
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in Sorting)
                {
                    if (KV.Value.Length == 0)
                        continue;
                    if (KV.Value.Length > 0)
                        this.ManyOrderByColumns_Sequence.Add(KV.Value, this.ManyOrderByColumns_Sequence.Count * 2);
                    if (i == 0 && SwitchTopColumn)
                    {
                        if (KV.Key > 0 || KV.Key == -2)
                            this.ManyOrderByColumns_Controls.Add(KV.Value, keyValuePairs[KV.Value]);
                    }
                    else
                    {
                        if (KV.Value.Length > 0 && keyValuePairs.ContainsKey(KV.Value))
                            this.ManyOrderByColumns_Controls.Add(KV.Value, keyValuePairs[KV.Value]);
                    }
                    i++;
                }
                //if (SwitchTopColumn)
                //    this.ManyOrderByColumns_Init();
                //else
                this.ManyOrderByColumns_SetControls();
            }
        }

        /// <summary>
        /// If adding columns for the query result is allowed = + button for adding visible
        /// </summary>
        public bool ManyOrderByColumns_Allow()
        {
            return _ManyOrderByColumns_Allow;
        }

        public bool AllowManyOrderByColumns { set { _ManyOrderByColumns_Allow = value; } }

        public void ManyOrderByColumns_Allow(bool IsAllowed)
        {
            _ManyOrderByColumns_Allow = IsAllowed;
        }

        public void ManyOrderByColumns_InitControls()//string Module, string Form)
        {
            if (_UsedByForm == null || _UsedByForm.Length == 0 || _UsedByModule == null || _UsedByModule.Length == 0)
                return;
            if (_ManyOrderByColumns_Allow)
            {
                this.ManyOrderByColumns_Init();// _UsedByModule, _UsedByForm);
                this.ManyOrderByColumns_SetControls();
            }
        }


        public void ManyOrderByColumns_Allow(bool IsAllowed, string Module, string Form, bool SuspendLayout = false)
        {
            try
            {
                _ManyOrderByColumns_Allow = IsAllowed;
                _UsedByForm = Form;
                _UsedByModule = Module;
                if (_ManyOrderByColumns_Allow)
                {
                    this.ManyOrderByColumns_Init();// Module, Form);
                    this.ManyOrderByColumns_SetControls(SuspendLayout);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public void ManyOrderByColumns_RemoveColumn(string DisplayText)
        {
            if (DisplayText != null && this.ManyOrderByColumns_Controls.ContainsKey(DisplayText))
            {
                this.ManyOrderByColumns_Controls.Remove(DisplayText);
                this.ManyOrderByColumns_Sequence.Remove(DisplayText);
                this.ManyOrderByColumns_SetControls();
            }
            else if (DisplayText == null)
            {
                this.ManyOrderByColumns_SetControls();
            }
        }

        #endregion

        #region Infos about the Widths

        public int ManyOrderByColumns_Spacer1Width()
        {
            return this.labelQueryColumn.Width;
        }

        public System.Collections.Generic.Dictionary<string, int> ManyOrderByColumns_Widths()
        {
            System.Collections.Generic.Dictionary<string, int> Widths = new Dictionary<string, int>();
            // Toni 20230414 Untreated null pointer esception...
            if (UserControlQueryList._DisplayColumnOptimizing != null)
            {
                //string SQL = "SELECT ";
                Widths.Add(UserControlQueryList._DisplayColumnOptimizing, this._ManyOrderByColumns_FirstColumnWidth);
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                {
                    int iWidth = KV.Value.QueryOrderColumnControl.Width;
                    if (KV.Value.QueryOrderColumnControl.Sorting == "DESC")
                        iWidth = -iWidth;
                    Widths.Add(KV.Key, iWidth);
                    //if (SQL.Length > 0) SQL += ", ";
                    //SQL += " avg(len([" + KV.Value.QueryOrderColumn.OrderColumn + "])) + cast(stdev(len([" + KV.Value.QueryOrderColumn.OrderColumn + "])) as int) * 2 AS " + KV.Value.QueryOrderColumn.OrderColumn + " ";
                }
                //foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.QueryOrderColumns)
                //{
                //    SQL += " AND [" + KV.Value.QueryOrderColumn.OrderColumn + "] <> '' ";
                //}
            }
            return Widths;
        }

        #endregion

        #region Handling of the controls

        private void ManyOrderByColumns_Reset()
        {
            if (this._ManyOrderByColumns_TableAliases != null)
                this._ManyOrderByColumns_TableAliases.Clear();
            this.ManyOrderByColumns_Controls.Clear();
            this.ManyOrderByColumns_Sequence.Clear();
            this.panelOrderByColumns.Controls.Clear();
        }

        private Dictionary<string, OrderBy> ManyOrderByColumns_Controls
        {
            get
            {
                if (_ManyOrderByColumns_Controls == null) _ManyOrderByColumns_Controls = new Dictionary<string, OrderBy>();
                return _ManyOrderByColumns_Controls;
            }
            set => _ManyOrderByColumns_Controls = value;
        }

        private QueryDisplayColumn ManyOrderByColumns_GetColumn(string DisplayText)
        {
            if (this.DisplayColumns != null)
            {
                for (int i = 0; i < this.DisplayColumns.Length; i++)
                {
                    if (this.DisplayColumns[i].DisplayText == DisplayText)
                        return this.DisplayColumns[i];
                }
            }
            QueryDisplayColumn queryDisplayColumn = new QueryDisplayColumn();
            return queryDisplayColumn;
        }

        private System.Collections.Generic.List<string> ManyOrderByColumns_MissingColumns()
        {
            System.Collections.Generic.List<string> list = new List<string>();
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQueryColumn.SelectedItem;
                string MainTable = R[2].ToString();
                for (int i = 0; i < this.DisplayColumns.Length; i++)
                {
                    //if (!UserControlQueryList._TableAliases.ContainsKey(this.DisplayColumns[i].TableName) && this.DisplayColumns[i].TableName != MainTable)
                    //    continue;
                    if (!ManyOrderByColumns_Controls.ContainsKey(this.DisplayColumns[i].DisplayText) && !list.Contains(R[0].ToString()) && this.DisplayColumns[i].DisplayText != R[0].ToString())
                    {
                        list.Add(this.DisplayColumns[i].DisplayText);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return list;
        }

        /// <summary>
        /// If ManyOrderByColumns are used
        /// 
        /// 
        ///
        /// <list type="bullet">
        /// <listheader>ToDos bei Einbindung am Beispiel von DC</listheader>
        /// <item>
        /// in Formular bei initQuery:
        /// <code>
        /// this.userControlQueryList.ManyOrderByColumns_Allow(true, DiversityWorkbench.Settings.ModuleName, "FormCollectionSpecimen", true);
        /// </code>
        /// </item>
        /// <item>
        /// in Copy Funktion:
        /// <code>
        ///  if (this.userControlQueryList.ManyOrderByColumns())
        ///  {
        ///     string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(SpecimenID);
        ///     this.userControlQueryList.AddListItem(SpecimenID, Display);
        /// }
        ///    else
        ///        this.userControlQueryList.AddListItem(SpecimenID, AccessionNumber);
        /// </code>
        /// </item>
        /// <item>
        /// in Create Funktion:
        /// <code>
        ///  if (this.userControlQueryList.ManyOrderByColumns())
        ///    {
        ///     string Display = this.userControlQueryList.ManyOrderByColumns_DisplayText(SpecimenID);
        ///    this.userControlQueryList.AddListItem(SpecimenID, Display);
        /// }
        ///    else
        ///    {
        ///    ...
        ///    </code>
        /// </item>
        /// <item>
        /// if missing new function:
        /// <code>
        ///     private void initQueryOptimizing()
        ///     {
        ///         DiversityWorkbench.UserControls.UserControlQueryList.QueryMainTable = "CollectionSpecimen_Core2";
        ///        this.userControlQueryList.QueryMainTableLocal = "CollectionSpecimen_Core2";
        ///     }
        ///</code>
        ///<see cref=">"/>
        ///initQuery()
        /// </item>
        /// <item>
        /// in FormClosing:
        /// <code>
        /// if (this.userControlQueryList.ManyOrderByColumns_Allow())
        ///    {
        ///         DiversityWorkbench.Settings.ManyOrderColumnSave(DiversityWorkbench.Settings.ModuleName, "FormCollectionSpecimen", this.userControlQueryList.ManyOrderByColumns_Widths());
        ///     }
        ///</code>
        /// </item>
        /// <item>
        /// in initForm()
        /// <code>
        ///        this.userControlQueryList.ManyOrderByColumns_Allow(true);
        ///    this.userControlQueryList.OptimizingAvailable(true);
        ///    this.userControlQueryList.ManyOrderByColumns_InitControls();
        ///</code>
        /// </item>
        /// </list>
        /// </summary>
        /// <returns>True if ManyOrderByColumns are used</returns>
        public bool ManyOrderByColumns()
        {
            return _ManyOrderByColumns_Allow && (_Optimizing_UsedForQueryList || DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing) && this.ManyOrderByColumns_Controls.Count > 0;
        }

        private void ManyOrderByColumns_SetControls(bool SuspendLayout = false)
        {
            if (SuspendLayout) this.SuspendLayout();
            bool Allow = _ManyOrderByColumns_Allow;
            if (!this._Optimizing_UsedForQueryList && !DiversityWorkbench.UserControls.UserControlQueryList.UseOptimizing) Allow = false;
            this.buttonOrderByColumnAdd.Visible = Allow;
            this.maskedTextBoxOrderByColumnWidth.Visible = ManyOrderByColumns();
            this.pictureBoxOrderByColumnWidth.Visible = ManyOrderByColumns();
            if (!Allow) this.ManyOrderByColumns_Controls.Clear();

            if (ManyOrderByColumns())
            {
                this.listBoxQueryResult.Font = this._FontManyOrderByColumns;
                this.listBoxQueryResult.HorizontalScrollbar = true;
            }
            else
            {
                this.listBoxQueryResult.Font = this._FontDefault;
                dtQuery.Clear();
                this.QueryDatabase(false);
                this.listBoxQueryResult.HorizontalScrollbar = false;

            }

            int height = 14;
            this.panelOrderByColumns.Controls.Clear();
            this.panelOrderByColumns.Height = this.ManyOrderByColumns_Controls.Count * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(height);
            if (this.ManyOrderByColumns_Controls.Count == 0)
            {
                this.tableLayoutPanelQueryButtons.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(50);
                this.panelOrderByColumns.Visible = false;
            }
            else
            {
                this.tableLayoutPanelQueryButtons.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(50) + this.ManyOrderByColumns_Controls.Count * DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(height);
                this.panelOrderByColumns.Visible = true;
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, OrderBy> KV in this.ManyOrderByColumns_Controls)
                {
                    this.panelOrderByColumns.Controls.Add(KV.Value.QueryOrderColumnControl);
                    this.toolTipQueryList.SetToolTip(KV.Value.QueryOrderColumnControl.labelDisplayText, KV.Value.QueryOrderColumn.TipText);
                    this.panelOrderByColumns.Controls[i].Dock = DockStyle.Top;
                    this.panelOrderByColumns.Controls[i].Height = height;
                    this.panelOrderByColumns.Controls[i].BringToFront();
                    i++;
                }
            }
            if (SuspendLayout) this.ResumeLayout();
        }

        public SortedDictionary<string, int> ManyOrderByColumns_Sequence
        {
            get
            {
                if (_ManyOrderByColumns_Sequence == null) _ManyOrderByColumns_Sequence = new SortedDictionary<string, int>();
                return _ManyOrderByColumns_Sequence;
            }
            //set => _ManyOrderByColumns_Sequence = value;
        }



        #endregion

        #region Control events

        private void buttonOrderByColumnAdd_Click(object sender, EventArgs e)
        {
            if (this.ManyOrderByColumns_MissingColumns().Count > 0)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this.ManyOrderByColumns_MissingColumns(), "Order column", "Select a order column", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (!ManyOrderByColumns_Controls.ContainsKey(f.SelectedString) && f.SelectedString.Length > 0)
                    {
                        OrderBy orderBy = new OrderBy();
                        orderBy.DisplayText = f.SelectedString;

                        // getting the column
                        QueryDisplayColumn queryDisplayColumn = this.ManyOrderByColumns_GetColumn(f.SelectedString);
                        orderBy.QueryOrderColumn = queryDisplayColumn;

                        // getting the control
                        UserControls.UserControlQueryOrderColumn orderColumnControl = new UserControls.UserControlQueryOrderColumn(queryDisplayColumn, this);
                        orderBy.QueryOrderColumnControl = orderColumnControl;

                        this.ManyOrderByColumns_Controls.Add(f.SelectedString, orderBy);

                        //if (this._ManyOrderByColumns_ControlSequence == null) this._ManyOrderByColumns_ControlSequence = new SortedDictionary<int, string>();
                        //this._ManyOrderByColumns_ControlSequence.Add(this._ManyOrderByColumns_ControlSequence.Count, f.SelectedString);
                        if (!this.ManyOrderByColumns_Sequence.ContainsKey(f.SelectedValue))
                            this.ManyOrderByColumns_Sequence.Add(f.SelectedString, this.ManyOrderByColumns_Sequence.Count * 2);

                        this.ManyOrderByColumns_SetControls();
                    }
                }
            }
        }

        private void maskedTextBoxOrderByColumnWidth_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(this.maskedTextBoxOrderByColumnWidth.Text, out _ManyOrderByColumns_FirstColumnWidth);
        }

        private void pictureBoxOrderByColumnWidth_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListOfIDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    return;
                }
                string IDs = "";
                foreach (int ID in this.ListOfIDs)
                {
                    if (IDs.Length > 0) IDs += ", ";
                    IDs += ID.ToString();
                }
                // Check correct _DisplayColumnOptimizing
                string TableName = "";
                string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = '" + this.QueryMainTableLocal + "' AND C.COLUMN_NAME = '" + _DisplayColumnOptimizing + "'";
                int I;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out I) && I == 0)
                {
                    foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                    {
                        if (C.DisplayColumn == this.SelectedDisplayColumn && C.TableName == this.SelectedDisplayColumnTable)
                        {
                            _DisplayColumnOptimizing = C.DisplayColumn;
                            TableName = C.TableName;
                            break;
                        }
                    }
                }
                if (TableName.Length == 0)
                {
                    foreach (DiversityWorkbench.UserControls.QueryDisplayColumn C in this.DisplayColumns)
                    {
                        if (C.DisplayColumn == this.SelectedDisplayColumn && C.TableName == this.SelectedDisplayColumnTable)
                        {
                            _DisplayColumnOptimizing = C.DisplayColumn;
                            TableName = C.TableName;
                            break;
                        }
                    }
                }
                SQL = "SELECT MAX(LEN([" + _DisplayColumnOptimizing + "])) FROM [" + TableName + "] WHERE " + _IdentityColumnOptimizing + " IN (" + IDs + ")";
                int Max;
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out Max))
                {
                    if (Max > 99) Max = 99;
                    this.maskedTextBoxOrderByColumnWidth.Text = Max.ToString();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #endregion

        #region ApplicationID

        private void setVisibilityOfApplicationID()
        {
#if DEBUG
            if (DiversityWorkbench.Settings.ConnectionString != null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                string SQL = "select user_name()";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    string User = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    con.Close();
                    this.toolStripButtonApplicationID.Visible = (User == "dbo");
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
#endif
        }

        private void toolStripButtonApplicationID_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            int id = p.Id;
            string name = p.ProcessName;
            int DWBProcessID = DiversityWorkbench.Forms.FormFunctions.ProcessID();
            System.Windows.Forms.MessageBox.Show("Process: " + name + "; ID: " + DWBProcessID.ToString());
        }

        #endregion

    }


}
