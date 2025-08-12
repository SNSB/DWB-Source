using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{


    public partial class FormCollectionEventSeries : Form
    {
        #region Parameter
        private DiversityCollection.CollectionEventSeries _CollectionEventSeries;
        #endregion
        
        #region Construction

        public FormCollectionEventSeries()
        {
            InitializeComponent();
            this.splitContainerData.Panel2Collapsed = true;
            this.splitContainerMain.Panel2.Visible = false;
            this.initForm();
            this.panelHeader.Visible = true;
            this.userControlDialogPanel.Visible = true;
        }

        public FormCollectionEventSeries(int? ItemID)
            : this()
        {
            if (ItemID != null)
                this._CollectionEventSeries.setItem((int)ItemID);
            this.userControlDialogPanel.Visible = true;
            this.panelHeader.Visible = true;
        }

        public FormCollectionEventSeries(string SQLRestriction)
            : this()
        {
            this.userControlQueryList.setQueryRestriction(SQLRestriction, "#");//.QueryRestriction = SQLRestriction;
            this.userControlDialogPanel.Visible = false;
            this.panelHeader.Visible = true;
        }


        #endregion

        #region Form

        private void initForm()
        {
            System.Data.DataSet Dataset = this.dataSetCollectionEventSeries;
            if (this._CollectionEventSeries == null)
                this._CollectionEventSeries = new CollectionEventSeries(ref Dataset, this.dataSetCollectionEventSeries.CollectionEventSeries,
                    ref this.treeViewCollection, this, this.userControlQueryList, this.splitContainerMain,
                    this.splitContainerData, this.toolStripButtonSpecimenList, //this.imageListSpecimenList,
                    this.userControlSpecimenList, this.helpProvider, this.toolTip, ref this.collectionEventSeriesBindingSource);
            this._CollectionEventSeries.initForm();
            this._CollectionEventSeries.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
            this._CollectionEventSeries.setToolStripButtonNewEvent(this.toolStripButtonNew);
            this._CollectionEventSeries.setToolbarPermission(ref this.toolStripButtonDelete, "CollectionEventSeries", "Delete");
            this._CollectionEventSeries.setToolbarPermission(ref this.toolStripButtonNew, "CollectionEventSeries", "Insert");
            this.initRemoteModules();
            this.userControlSpecimenList.toolStripButtonDelete.Visible = false;
        }

        #endregion

        #region Modules

        private void initRemoteModules()
        {
            try
            {
                // Agents
                //DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                //this.userControlModuleRelatedEntryAdministrativeContactName.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                //this.userControlModuleRelatedEntryAdministrativeContactName.bindToData("Collection", "AdministrativeContactName", "AdministrativeContactAgentURI", this.collectionEventSeriesBindingSource);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Properties
        public int? ID 
        { 
            get 
            {
                int id = 0;
                if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count == 0) return null;
                if (int.TryParse(this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[this.collectionEventSeriesBindingSource.Position][0].ToString(), out id))
                    return id;
                else 
                    return null;
                //return int.Parse(this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[this.collectionEventSeriesBindingSource.Position][0].ToString()); 
            } 
        }
        public string DisplayText { get { return this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[this.collectionEventSeriesBindingSource.Position][2].ToString(); } }
        public bool ChangeToSpecimen { get { return this.userControlSpecimenList.ChangeToSpecimen; } }
        public int CollectionSpecimenID { get { return this.userControlSpecimenList.CollectionSpecimenID; } }
        public string ColumnCode { get { return this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[this.collectionEventSeriesBindingSource.Position][3].ToString(); } }
        public string ColumnNotes { get { return this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[this.collectionEventSeriesBindingSource.Position][4].ToString(); } }
        #endregion

        #region Designer

        private Panel panelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private SplitContainer splitContainerMain;
        private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryList;
        private SplitContainer splitContainerData;
        private GroupBox groupBoxCollectionEventSeries;
        private TableLayoutPanel tableLayoutPanelHierarchy;
        private TreeView treeViewCollection;
        private ToolStrip toolStripHierarchy;
        private ToolStripButton toolStripButtonNew;
        private ToolStripButton toolStripButtonDelete;
        private ToolStripButton toolStripButtonSpecimenList;
        private DiversityCollection.UserControls.UserControlSpecimenList userControlSpecimenList;
        private SplitContainer splitContainerHierarchy;
        private SplitContainer splitContainerEventSeries;
        private GroupBox groupBoxEventSeries;
        private PictureBox pictureBoxEventSeries;
        private TableLayoutPanel tableLayoutPanelOverviewEventSeries;
        private Label labelOverviewEventSeriesDate;
        private TextBox textBoxOverviewEventSeriesDateStart;
        private Label labelOverviewEventSeriesCode;
        private TextBox textBoxOverviewEventSeriesCode;
        private Label labelOverviewEventSeriesDescription;
        private TextBox textBoxOverviewEventSeriesDescription;
        private Label labelOverviewEventSeriesNotes;
        private TextBox textBoxOverviewEventSeriesNotes;
        private GroupBox groupBoxEvent;
        private PictureBox pictureBoxEvent;
        private SplitContainer splitContainerOverviewEventData;
        private TableLayoutPanel tableLayoutPanelEvent;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryEventReference;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelEventDate;
        private Label labelEventDate;
        private Label labelCollectionTime;
        private TextBox textBoxCollectionTime;
        private Label labelCollectionTimeSpan;
        private TextBox textBoxCollectionTimeSpan;
        private Label labelCollectionDateCategory;
        private ComboBox comboBoxCollectionDateCategory;
        private Label labelDataWithholdingReasonEvent;
        private Label labelCollectingMethod;
        private TextBox textBoxCollectingMethod;
        private Label labelEventReference;
        private Label labelEventNotes;
        private TextBox textBoxEventNotes;
        private Label labelCountryCache;
        private TextBox textBoxCountryCache;
        private Label labelCollectorsEventNumber;
        private TextBox textBoxCollectorsEventNumber;
        private ComboBox comboBoxDataWithholdingReasonEvent;
        private SplitContainer splitContainerOverviewEventDescriptions;
        private TextBox textBoxEventLocality;
        private Label labelLocalityDescription;
        private TextBox textBoxHabitatDesciption;
        private Label labelHabitatDescription;
        private Datasets.DataSetCollectionEventSeries dataSetCollectionEventSeries;
        private HelpProvider helpProvider;
        private ImageList imageListSpecimenList;
        private IContainer components;
        private ToolTip toolTip;
        private BindingSource collectionEventSeriesBindingSource;
        private DiversityCollection.Datasets.DataSetCollectionEventSeriesTableAdapters.CollectionEventSeriesTableAdapter collectionEventSeriesTableAdapter;
        private TextBox textBoxOverviewEventSeriesDateEnd;
        private DateTimePicker dateTimePickerOverviewEventSeriesDateStart;
        private DateTimePicker dateTimePickerOverviewEventSeriesDateEnd;
        private Label labelEndDate;
        private Button buttonHistory;
        private Button buttonFeedback;
        private Label labelHeader;
    
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCollectionEventSeries));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            this.splitContainerData = new System.Windows.Forms.SplitContainer();
            this.groupBoxCollectionEventSeries = new System.Windows.Forms.GroupBox();
            this.splitContainerHierarchy = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelHierarchy = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewCollection = new System.Windows.Forms.TreeView();
            this.toolStripHierarchy = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSpecimenList = new System.Windows.Forms.ToolStripButton();
            this.splitContainerEventSeries = new System.Windows.Forms.SplitContainer();
            this.groupBoxEventSeries = new System.Windows.Forms.GroupBox();
            this.pictureBoxEventSeries = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelOverviewEventSeries = new System.Windows.Forms.TableLayoutPanel();
            this.labelOverviewEventSeriesDate = new System.Windows.Forms.Label();
            this.textBoxOverviewEventSeriesDateStart = new System.Windows.Forms.TextBox();
            this.collectionEventSeriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetCollectionEventSeries = new DiversityCollection.Datasets.DataSetCollectionEventSeries();
            this.labelOverviewEventSeriesCode = new System.Windows.Forms.Label();
            this.textBoxOverviewEventSeriesCode = new System.Windows.Forms.TextBox();
            this.labelOverviewEventSeriesDescription = new System.Windows.Forms.Label();
            this.textBoxOverviewEventSeriesDescription = new System.Windows.Forms.TextBox();
            this.labelOverviewEventSeriesNotes = new System.Windows.Forms.Label();
            this.textBoxOverviewEventSeriesNotes = new System.Windows.Forms.TextBox();
            this.textBoxOverviewEventSeriesDateEnd = new System.Windows.Forms.TextBox();
            this.dateTimePickerOverviewEventSeriesDateStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerOverviewEventSeriesDateEnd = new System.Windows.Forms.DateTimePicker();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.groupBoxEvent = new System.Windows.Forms.GroupBox();
            this.pictureBoxEvent = new System.Windows.Forms.PictureBox();
            this.splitContainerOverviewEventData = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelEvent = new System.Windows.Forms.TableLayoutPanel();
            this.userControlModuleRelatedEntryEventReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlDatePanelEventDate = new DiversityWorkbench.UserControls.UserControlDatePanel();
            this.labelEventDate = new System.Windows.Forms.Label();
            this.labelCollectionTime = new System.Windows.Forms.Label();
            this.textBoxCollectionTime = new System.Windows.Forms.TextBox();
            this.labelCollectionTimeSpan = new System.Windows.Forms.Label();
            this.textBoxCollectionTimeSpan = new System.Windows.Forms.TextBox();
            this.labelCollectionDateCategory = new System.Windows.Forms.Label();
            this.comboBoxCollectionDateCategory = new System.Windows.Forms.ComboBox();
            this.labelDataWithholdingReasonEvent = new System.Windows.Forms.Label();
            this.labelCollectingMethod = new System.Windows.Forms.Label();
            this.textBoxCollectingMethod = new System.Windows.Forms.TextBox();
            this.labelEventReference = new System.Windows.Forms.Label();
            this.labelEventNotes = new System.Windows.Forms.Label();
            this.textBoxEventNotes = new System.Windows.Forms.TextBox();
            this.labelCountryCache = new System.Windows.Forms.Label();
            this.textBoxCountryCache = new System.Windows.Forms.TextBox();
            this.labelCollectorsEventNumber = new System.Windows.Forms.Label();
            this.textBoxCollectorsEventNumber = new System.Windows.Forms.TextBox();
            this.comboBoxDataWithholdingReasonEvent = new System.Windows.Forms.ComboBox();
            this.splitContainerOverviewEventDescriptions = new System.Windows.Forms.SplitContainer();
            this.textBoxEventLocality = new System.Windows.Forms.TextBox();
            this.labelLocalityDescription = new System.Windows.Forms.Label();
            this.textBoxHabitatDesciption = new System.Windows.Forms.TextBox();
            this.labelHabitatDescription = new System.Windows.Forms.Label();
            this.userControlSpecimenList = new DiversityCollection.UserControls.UserControlSpecimenList();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.imageListSpecimenList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.collectionEventSeriesTableAdapter = new DiversityCollection.Datasets.DataSetCollectionEventSeriesTableAdapters.CollectionEventSeriesTableAdapter();
            this.buttonHistory = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.splitContainerData.Panel1.SuspendLayout();
            this.splitContainerData.Panel2.SuspendLayout();
            this.splitContainerData.SuspendLayout();
            this.groupBoxCollectionEventSeries.SuspendLayout();
            this.splitContainerHierarchy.Panel1.SuspendLayout();
            this.splitContainerHierarchy.Panel2.SuspendLayout();
            this.splitContainerHierarchy.SuspendLayout();
            this.tableLayoutPanelHierarchy.SuspendLayout();
            this.toolStripHierarchy.SuspendLayout();
            this.splitContainerEventSeries.Panel1.SuspendLayout();
            this.splitContainerEventSeries.Panel2.SuspendLayout();
            this.splitContainerEventSeries.SuspendLayout();
            this.groupBoxEventSeries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEventSeries)).BeginInit();
            this.tableLayoutPanelOverviewEventSeries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionEventSeriesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionEventSeries)).BeginInit();
            this.groupBoxEvent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEvent)).BeginInit();
            this.splitContainerOverviewEventData.Panel1.SuspendLayout();
            this.splitContainerOverviewEventData.Panel2.SuspendLayout();
            this.splitContainerOverviewEventData.SuspendLayout();
            this.tableLayoutPanelEvent.SuspendLayout();
            this.splitContainerOverviewEventDescriptions.Panel1.SuspendLayout();
            this.splitContainerOverviewEventDescriptions.Panel2.SuspendLayout();
            this.splitContainerOverviewEventDescriptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.labelHeader);
            this.panelHeader.Controls.Add(this.buttonHistory);
            this.panelHeader.Controls.Add(this.buttonFeedback);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(831, 24);
            this.panelHeader.TabIndex = 7;
            this.panelHeader.Visible = false;
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(783, 24);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Select a collection event series";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 564);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(831, 27);
            this.userControlDialogPanel.TabIndex = 8;
            this.userControlDialogPanel.Visible = false;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.userControlQueryList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerData);
            this.splitContainerMain.Size = new System.Drawing.Size(831, 540);
            this.splitContainerMain.SplitterDistance = 276;
            this.splitContainerMain.TabIndex = 9;
            // 
            // userControlQueryList
            // 
            this.userControlQueryList.Connection = null;
            this.userControlQueryList.DisplayTextSelectedItem = "";
            this.userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlQueryList.IDisNumeric = true;
            this.userControlQueryList.ImageList = null;
            this.userControlQueryList.IsPredefinedQuery = false;
            this.userControlQueryList.Location = new System.Drawing.Point(0, 0);
            this.userControlQueryList.MaximalNumberOfResults = 100;
            this.userControlQueryList.Name = "userControlQueryList";
            this.userControlQueryList.ProjectID = -1;
            this.userControlQueryList.QueryConditionVisiblity = "";
            this.userControlQueryList.QueryDisplayColumns = null;
            this.userControlQueryList.QueryMainTableLocal = null;
            this.userControlQueryList.QueryRestriction = "";
            this.userControlQueryList.RememberQuerySettingsIdentifier = "QueryList";
            this.userControlQueryList.SelectedProjectID = null;
            this.userControlQueryList.Size = new System.Drawing.Size(276, 540);
            this.userControlQueryList.TabIndex = 2;
            this.userControlQueryList.TableColors = null;
            this.userControlQueryList.TableImageIndex = null;
            this.userControlQueryList.WhereClause = null;
            // 
            // splitContainerData
            // 
            this.splitContainerData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerData.Location = new System.Drawing.Point(0, 0);
            this.splitContainerData.Name = "splitContainerData";
            // 
            // splitContainerData.Panel1
            // 
            this.splitContainerData.Panel1.Controls.Add(this.groupBoxCollectionEventSeries);
            // 
            // splitContainerData.Panel2
            // 
            this.splitContainerData.Panel2.Controls.Add(this.userControlSpecimenList);
            this.splitContainerData.Size = new System.Drawing.Size(551, 540);
            this.splitContainerData.SplitterDistance = 376;
            this.splitContainerData.TabIndex = 2;
            // 
            // groupBoxCollectionEventSeries
            // 
            this.groupBoxCollectionEventSeries.Controls.Add(this.splitContainerHierarchy);
            this.groupBoxCollectionEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxCollectionEventSeries.Location = new System.Drawing.Point(0, 0);
            this.groupBoxCollectionEventSeries.Name = "groupBoxCollectionEventSeries";
            this.groupBoxCollectionEventSeries.Size = new System.Drawing.Size(376, 540);
            this.groupBoxCollectionEventSeries.TabIndex = 1;
            this.groupBoxCollectionEventSeries.TabStop = false;
            this.groupBoxCollectionEventSeries.Text = "Collection event series";
            // 
            // splitContainerHierarchy
            // 
            this.splitContainerHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerHierarchy.Location = new System.Drawing.Point(3, 16);
            this.splitContainerHierarchy.Name = "splitContainerHierarchy";
            this.splitContainerHierarchy.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerHierarchy.Panel1
            // 
            this.splitContainerHierarchy.Panel1.Controls.Add(this.tableLayoutPanelHierarchy);
            // 
            // splitContainerHierarchy.Panel2
            // 
            this.splitContainerHierarchy.Panel2.Controls.Add(this.splitContainerEventSeries);
            this.splitContainerHierarchy.Size = new System.Drawing.Size(370, 521);
            this.splitContainerHierarchy.SplitterDistance = 176;
            this.splitContainerHierarchy.TabIndex = 1;
            // 
            // tableLayoutPanelHierarchy
            // 
            this.tableLayoutPanelHierarchy.ColumnCount = 2;
            this.tableLayoutPanelHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelHierarchy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelHierarchy.Controls.Add(this.treeViewCollection, 0, 0);
            this.tableLayoutPanelHierarchy.Controls.Add(this.toolStripHierarchy, 1, 0);
            this.tableLayoutPanelHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelHierarchy.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelHierarchy.Name = "tableLayoutPanelHierarchy";
            this.tableLayoutPanelHierarchy.RowCount = 1;
            this.tableLayoutPanelHierarchy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHierarchy.Size = new System.Drawing.Size(370, 176);
            this.tableLayoutPanelHierarchy.TabIndex = 0;
            // 
            // treeViewCollection
            // 
            this.treeViewCollection.AllowDrop = true;
            this.treeViewCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCollection.Location = new System.Drawing.Point(3, 3);
            this.treeViewCollection.Name = "treeViewCollection";
            this.treeViewCollection.Size = new System.Drawing.Size(344, 170);
            this.treeViewCollection.TabIndex = 0;
            // 
            // toolStripHierarchy
            // 
            this.toolStripHierarchy.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripHierarchy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonDelete,
            this.toolStripButtonSpecimenList});
            this.toolStripHierarchy.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripHierarchy.Location = new System.Drawing.Point(350, 0);
            this.toolStripHierarchy.Name = "toolStripHierarchy";
            this.toolStripHierarchy.Size = new System.Drawing.Size(20, 176);
            this.toolStripHierarchy.TabIndex = 1;
            this.toolStripHierarchy.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNew.Image = global::DiversityCollection.Resource.New1;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonNew.Text = "Insert a new analysis dependent on the selected analysis";
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDelete.Text = "delete the selected analysis";
            // 
            // toolStripButtonSpecimenList
            // 
            this.toolStripButtonSpecimenList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSpecimenList.Image = global::DiversityCollection.Resource.List;
            this.toolStripButtonSpecimenList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSpecimenList.Name = "toolStripButtonSpecimenList";
            this.toolStripButtonSpecimenList.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonSpecimenList.Text = "Show specimen list";
            // 
            // splitContainerEventSeries
            // 
            this.splitContainerEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerEventSeries.Location = new System.Drawing.Point(0, 0);
            this.splitContainerEventSeries.Name = "splitContainerEventSeries";
            // 
            // splitContainerEventSeries.Panel1
            // 
            this.splitContainerEventSeries.Panel1.Controls.Add(this.groupBoxEventSeries);
            // 
            // splitContainerEventSeries.Panel2
            // 
            this.splitContainerEventSeries.Panel2.Controls.Add(this.groupBoxEvent);
            this.splitContainerEventSeries.Panel2Collapsed = true;
            this.splitContainerEventSeries.Size = new System.Drawing.Size(370, 341);
            this.splitContainerEventSeries.SplitterDistance = 295;
            this.splitContainerEventSeries.TabIndex = 0;
            // 
            // groupBoxEventSeries
            // 
            this.groupBoxEventSeries.Controls.Add(this.pictureBoxEventSeries);
            this.groupBoxEventSeries.Controls.Add(this.tableLayoutPanelOverviewEventSeries);
            this.groupBoxEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEventSeries.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEventSeries.Name = "groupBoxEventSeries";
            this.groupBoxEventSeries.Size = new System.Drawing.Size(370, 341);
            this.groupBoxEventSeries.TabIndex = 5;
            this.groupBoxEventSeries.TabStop = false;
            this.groupBoxEventSeries.Text = "Event series";
            // 
            // pictureBoxEventSeries
            // 
            this.pictureBoxEventSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxEventSeries.Image = global::DiversityCollection.Resource.EventSeries;
            this.pictureBoxEventSeries.Location = new System.Drawing.Point(351, 3);
            this.pictureBoxEventSeries.Name = "pictureBoxEventSeries";
            this.pictureBoxEventSeries.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxEventSeries.TabIndex = 4;
            this.pictureBoxEventSeries.TabStop = false;
            // 
            // tableLayoutPanelOverviewEventSeries
            // 
            this.tableLayoutPanelOverviewEventSeries.ColumnCount = 4;
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelOverviewEventSeriesDate, 0, 0);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxOverviewEventSeriesDateStart, 0, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelOverviewEventSeriesCode, 0, 4);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxOverviewEventSeriesCode, 0, 5);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelOverviewEventSeriesDescription, 0, 2);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxOverviewEventSeriesDescription, 0, 3);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelOverviewEventSeriesNotes, 0, 6);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxOverviewEventSeriesNotes, 0, 7);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxOverviewEventSeriesDateEnd, 2, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.dateTimePickerOverviewEventSeriesDateStart, 1, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.dateTimePickerOverviewEventSeriesDateEnd, 3, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelEndDate, 2, 0);
            this.tableLayoutPanelOverviewEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOverviewEventSeries.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelOverviewEventSeries.Name = "tableLayoutPanelOverviewEventSeries";
            this.tableLayoutPanelOverviewEventSeries.RowCount = 8;
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelOverviewEventSeries.Size = new System.Drawing.Size(364, 322);
            this.tableLayoutPanelOverviewEventSeries.TabIndex = 3;
            // 
            // labelOverviewEventSeriesDate
            // 
            this.labelOverviewEventSeriesDate.AutoSize = true;
            this.labelOverviewEventSeriesDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewEventSeriesDate.ForeColor = System.Drawing.Color.Blue;
            this.labelOverviewEventSeriesDate.Location = new System.Drawing.Point(3, 0);
            this.labelOverviewEventSeriesDate.Name = "labelOverviewEventSeriesDate";
            this.labelOverviewEventSeriesDate.Size = new System.Drawing.Size(156, 13);
            this.labelOverviewEventSeriesDate.TabIndex = 0;
            this.labelOverviewEventSeriesDate.Text = "Start date";
            this.labelOverviewEventSeriesDate.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxOverviewEventSeriesDateStart
            // 
            this.textBoxOverviewEventSeriesDateStart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.collectionEventSeriesBindingSource, "DateStart", true));
            this.textBoxOverviewEventSeriesDateStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverviewEventSeriesDateStart.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOverviewEventSeriesDateStart.Location = new System.Drawing.Point(3, 16);
            this.textBoxOverviewEventSeriesDateStart.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxOverviewEventSeriesDateStart.Name = "textBoxOverviewEventSeriesDateStart";
            this.textBoxOverviewEventSeriesDateStart.Size = new System.Drawing.Size(159, 20);
            this.textBoxOverviewEventSeriesDateStart.TabIndex = 1;
            // 
            // collectionEventSeriesBindingSource
            // 
            this.collectionEventSeriesBindingSource.DataMember = "CollectionEventSeries";
            this.collectionEventSeriesBindingSource.DataSource = this.dataSetCollectionEventSeries;
            // 
            // dataSetCollectionEventSeries
            // 
            this.dataSetCollectionEventSeries.DataSetName = "DataSetCollectionEventSeries";
            this.dataSetCollectionEventSeries.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelOverviewEventSeriesCode
            // 
            this.labelOverviewEventSeriesCode.AutoSize = true;
            this.labelOverviewEventSeriesCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewEventSeriesCode.ForeColor = System.Drawing.Color.Blue;
            this.labelOverviewEventSeriesCode.Location = new System.Drawing.Point(3, 161);
            this.labelOverviewEventSeriesCode.Name = "labelOverviewEventSeriesCode";
            this.labelOverviewEventSeriesCode.Size = new System.Drawing.Size(156, 13);
            this.labelOverviewEventSeriesCode.TabIndex = 2;
            this.labelOverviewEventSeriesCode.Text = "Code";
            this.labelOverviewEventSeriesCode.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxOverviewEventSeriesCode
            // 
            this.tableLayoutPanelOverviewEventSeries.SetColumnSpan(this.textBoxOverviewEventSeriesCode, 4);
            this.textBoxOverviewEventSeriesCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.collectionEventSeriesBindingSource, "SeriesCode", true));
            this.textBoxOverviewEventSeriesCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverviewEventSeriesCode.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOverviewEventSeriesCode.Location = new System.Drawing.Point(3, 177);
            this.textBoxOverviewEventSeriesCode.Name = "textBoxOverviewEventSeriesCode";
            this.textBoxOverviewEventSeriesCode.Size = new System.Drawing.Size(358, 20);
            this.textBoxOverviewEventSeriesCode.TabIndex = 5;
            // 
            // labelOverviewEventSeriesDescription
            // 
            this.labelOverviewEventSeriesDescription.AutoSize = true;
            this.labelOverviewEventSeriesDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewEventSeriesDescription.ForeColor = System.Drawing.Color.Blue;
            this.labelOverviewEventSeriesDescription.Location = new System.Drawing.Point(3, 39);
            this.labelOverviewEventSeriesDescription.Name = "labelOverviewEventSeriesDescription";
            this.labelOverviewEventSeriesDescription.Size = new System.Drawing.Size(156, 13);
            this.labelOverviewEventSeriesDescription.TabIndex = 4;
            this.labelOverviewEventSeriesDescription.Text = "Description";
            this.labelOverviewEventSeriesDescription.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxOverviewEventSeriesDescription
            // 
            this.tableLayoutPanelOverviewEventSeries.SetColumnSpan(this.textBoxOverviewEventSeriesDescription, 4);
            this.textBoxOverviewEventSeriesDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.collectionEventSeriesBindingSource, "Description", true));
            this.textBoxOverviewEventSeriesDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverviewEventSeriesDescription.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOverviewEventSeriesDescription.Location = new System.Drawing.Point(3, 55);
            this.textBoxOverviewEventSeriesDescription.Multiline = true;
            this.textBoxOverviewEventSeriesDescription.Name = "textBoxOverviewEventSeriesDescription";
            this.textBoxOverviewEventSeriesDescription.Size = new System.Drawing.Size(358, 103);
            this.textBoxOverviewEventSeriesDescription.TabIndex = 3;
            // 
            // labelOverviewEventSeriesNotes
            // 
            this.labelOverviewEventSeriesNotes.AutoSize = true;
            this.labelOverviewEventSeriesNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewEventSeriesNotes.ForeColor = System.Drawing.Color.Blue;
            this.labelOverviewEventSeriesNotes.Location = new System.Drawing.Point(3, 200);
            this.labelOverviewEventSeriesNotes.Name = "labelOverviewEventSeriesNotes";
            this.labelOverviewEventSeriesNotes.Size = new System.Drawing.Size(156, 13);
            this.labelOverviewEventSeriesNotes.TabIndex = 6;
            this.labelOverviewEventSeriesNotes.Text = "Notes";
            this.labelOverviewEventSeriesNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxOverviewEventSeriesNotes
            // 
            this.tableLayoutPanelOverviewEventSeries.SetColumnSpan(this.textBoxOverviewEventSeriesNotes, 4);
            this.textBoxOverviewEventSeriesNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.collectionEventSeriesBindingSource, "Notes", true));
            this.textBoxOverviewEventSeriesNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverviewEventSeriesNotes.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOverviewEventSeriesNotes.Location = new System.Drawing.Point(3, 216);
            this.textBoxOverviewEventSeriesNotes.Multiline = true;
            this.textBoxOverviewEventSeriesNotes.Name = "textBoxOverviewEventSeriesNotes";
            this.textBoxOverviewEventSeriesNotes.Size = new System.Drawing.Size(358, 103);
            this.textBoxOverviewEventSeriesNotes.TabIndex = 7;
            // 
            // textBoxOverviewEventSeriesDateEnd
            // 
            this.textBoxOverviewEventSeriesDateEnd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.collectionEventSeriesBindingSource, "DateEnd", true));
            this.textBoxOverviewEventSeriesDateEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverviewEventSeriesDateEnd.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOverviewEventSeriesDateEnd.Location = new System.Drawing.Point(185, 16);
            this.textBoxOverviewEventSeriesDateEnd.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxOverviewEventSeriesDateEnd.Name = "textBoxOverviewEventSeriesDateEnd";
            this.textBoxOverviewEventSeriesDateEnd.Size = new System.Drawing.Size(159, 20);
            this.textBoxOverviewEventSeriesDateEnd.TabIndex = 8;
            // 
            // dateTimePickerOverviewEventSeriesDateStart
            // 
            this.dateTimePickerOverviewEventSeriesDateStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerOverviewEventSeriesDateStart.Location = new System.Drawing.Point(162, 16);
            this.dateTimePickerOverviewEventSeriesDateStart.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerOverviewEventSeriesDateStart.Name = "dateTimePickerOverviewEventSeriesDateStart";
            this.dateTimePickerOverviewEventSeriesDateStart.Size = new System.Drawing.Size(17, 20);
            this.dateTimePickerOverviewEventSeriesDateStart.TabIndex = 9;
            this.dateTimePickerOverviewEventSeriesDateStart.CloseUp += new System.EventHandler(this.dateTimePickerOverviewEventSeriesDateStart_CloseUp);
            this.dateTimePickerOverviewEventSeriesDateStart.DropDown += new System.EventHandler(this.dateTimePickerOverviewEventSeriesDateStart_DropDown);
            // 
            // dateTimePickerOverviewEventSeriesDateEnd
            // 
            this.dateTimePickerOverviewEventSeriesDateEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerOverviewEventSeriesDateEnd.Location = new System.Drawing.Point(344, 16);
            this.dateTimePickerOverviewEventSeriesDateEnd.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerOverviewEventSeriesDateEnd.Name = "dateTimePickerOverviewEventSeriesDateEnd";
            this.dateTimePickerOverviewEventSeriesDateEnd.Size = new System.Drawing.Size(17, 20);
            this.dateTimePickerOverviewEventSeriesDateEnd.TabIndex = 10;
            this.dateTimePickerOverviewEventSeriesDateEnd.CloseUp += new System.EventHandler(this.dateTimePickerOverviewEventSeriesDateEnd_CloseUp);
            this.dateTimePickerOverviewEventSeriesDateEnd.DropDown += new System.EventHandler(this.dateTimePickerOverviewEventSeriesDateEnd_DropDown);
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.tableLayoutPanelOverviewEventSeries.SetColumnSpan(this.labelEndDate, 2);
            this.labelEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEndDate.ForeColor = System.Drawing.Color.Blue;
            this.labelEndDate.Location = new System.Drawing.Point(185, 0);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(176, 13);
            this.labelEndDate.TabIndex = 11;
            this.labelEndDate.Text = "End";
            this.labelEndDate.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // groupBoxEvent
            // 
            this.groupBoxEvent.Controls.Add(this.pictureBoxEvent);
            this.groupBoxEvent.Controls.Add(this.splitContainerOverviewEventData);
            this.groupBoxEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEvent.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEvent.Name = "groupBoxEvent";
            this.groupBoxEvent.Size = new System.Drawing.Size(96, 100);
            this.groupBoxEvent.TabIndex = 2;
            this.groupBoxEvent.TabStop = false;
            this.groupBoxEvent.Text = "Collection event";
            // 
            // pictureBoxEvent
            // 
            this.pictureBoxEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxEvent.Image = global::DiversityCollection.Resource.Event;
            this.pictureBoxEvent.Location = new System.Drawing.Point(77, 1);
            this.pictureBoxEvent.Name = "pictureBoxEvent";
            this.pictureBoxEvent.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxEvent.TabIndex = 1;
            this.pictureBoxEvent.TabStop = false;
            // 
            // splitContainerOverviewEventData
            // 
            this.splitContainerOverviewEventData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOverviewEventData.Location = new System.Drawing.Point(3, 16);
            this.splitContainerOverviewEventData.Name = "splitContainerOverviewEventData";
            this.splitContainerOverviewEventData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOverviewEventData.Panel1
            // 
            this.splitContainerOverviewEventData.Panel1.Controls.Add(this.tableLayoutPanelEvent);
            // 
            // splitContainerOverviewEventData.Panel2
            // 
            this.splitContainerOverviewEventData.Panel2.Controls.Add(this.splitContainerOverviewEventDescriptions);
            this.splitContainerOverviewEventData.Size = new System.Drawing.Size(90, 81);
            this.splitContainerOverviewEventData.SplitterDistance = 36;
            this.splitContainerOverviewEventData.TabIndex = 0;
            // 
            // tableLayoutPanelEvent
            // 
            this.tableLayoutPanelEvent.ColumnCount = 6;
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEvent.Controls.Add(this.userControlModuleRelatedEntryEventReference, 1, 2);
            this.tableLayoutPanelEvent.Controls.Add(this.userControlDatePanelEventDate, 1, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.labelEventDate, 0, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectionTime, 2, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCollectionTime, 3, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectionTimeSpan, 4, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCollectionTimeSpan, 5, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectionDateCategory, 4, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.comboBoxCollectionDateCategory, 5, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.labelDataWithholdingReasonEvent, 3, 3);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectingMethod, 0, 5);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCollectingMethod, 1, 5);
            this.tableLayoutPanelEvent.Controls.Add(this.labelEventReference, 0, 2);
            this.tableLayoutPanelEvent.Controls.Add(this.labelEventNotes, 0, 4);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxEventNotes, 1, 4);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCountryCache, 0, 3);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCountryCache, 1, 3);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectorsEventNumber, 0, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCollectorsEventNumber, 1, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.comboBoxDataWithholdingReasonEvent, 4, 3);
            this.tableLayoutPanelEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEvent.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEvent.Name = "tableLayoutPanelEvent";
            this.tableLayoutPanelEvent.RowCount = 6;
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEvent.Size = new System.Drawing.Size(90, 36);
            this.tableLayoutPanelEvent.TabIndex = 22;
            // 
            // userControlModuleRelatedEntryEventReference
            // 
            this.userControlModuleRelatedEntryEventReference.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelEvent.SetColumnSpan(this.userControlModuleRelatedEntryEventReference, 5);
            this.userControlModuleRelatedEntryEventReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryEventReference.Domain = "";
            this.userControlModuleRelatedEntryEventReference.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryEventReference.Location = new System.Drawing.Point(59, 50);
            this.userControlModuleRelatedEntryEventReference.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.userControlModuleRelatedEntryEventReference.Module = null;
            this.userControlModuleRelatedEntryEventReference.Name = "userControlModuleRelatedEntryEventReference";
            this.userControlModuleRelatedEntryEventReference.Size = new System.Drawing.Size(28, 21);
            this.userControlModuleRelatedEntryEventReference.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryEventReference.TabIndex = 11;
            // 
            // userControlDatePanelEventDate
            // 
            this.tableLayoutPanelEvent.SetColumnSpan(this.userControlDatePanelEventDate, 3);
            this.userControlDatePanelEventDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatePanelEventDate.Location = new System.Drawing.Point(59, 3);
            this.userControlDatePanelEventDate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.userControlDatePanelEventDate.Name = "userControlDatePanelEventDate";
            this.userControlDatePanelEventDate.Size = new System.Drawing.Size(1, 21);
            this.userControlDatePanelEventDate.TabIndex = 0;
            // 
            // labelEventDate
            // 
            this.labelEventDate.AutoSize = true;
            this.labelEventDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventDate.Location = new System.Drawing.Point(3, 0);
            this.labelEventDate.Name = "labelEventDate";
            this.labelEventDate.Size = new System.Drawing.Size(53, 27);
            this.labelEventDate.TabIndex = 1;
            this.labelEventDate.Text = "Date:";
            this.labelEventDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCollectionTime
            // 
            this.labelCollectionTime.AutoSize = true;
            this.labelCollectionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionTime.Location = new System.Drawing.Point(-141, 27);
            this.labelCollectionTime.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCollectionTime.Name = "labelCollectionTime";
            this.labelCollectionTime.Size = new System.Drawing.Size(33, 20);
            this.labelCollectionTime.TabIndex = 2;
            this.labelCollectionTime.Text = "Time:";
            this.labelCollectionTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionTime
            // 
            this.textBoxCollectionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionTime.Location = new System.Drawing.Point(-108, 27);
            this.textBoxCollectionTime.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxCollectionTime.Name = "textBoxCollectionTime";
            this.textBoxCollectionTime.Size = new System.Drawing.Size(70, 20);
            this.textBoxCollectionTime.TabIndex = 5;
            // 
            // labelCollectionTimeSpan
            // 
            this.labelCollectionTimeSpan.AutoSize = true;
            this.labelCollectionTimeSpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionTimeSpan.Location = new System.Drawing.Point(-35, 27);
            this.labelCollectionTimeSpan.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCollectionTimeSpan.Name = "labelCollectionTimeSpan";
            this.labelCollectionTimeSpan.Size = new System.Drawing.Size(52, 20);
            this.labelCollectionTimeSpan.TabIndex = 4;
            this.labelCollectionTimeSpan.Text = "T.span:";
            this.labelCollectionTimeSpan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionTimeSpan
            // 
            this.textBoxCollectionTimeSpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionTimeSpan.Location = new System.Drawing.Point(17, 27);
            this.textBoxCollectionTimeSpan.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxCollectionTimeSpan.Name = "textBoxCollectionTimeSpan";
            this.textBoxCollectionTimeSpan.Size = new System.Drawing.Size(70, 20);
            this.textBoxCollectionTimeSpan.TabIndex = 7;
            // 
            // labelCollectionDateCategory
            // 
            this.labelCollectionDateCategory.AutoSize = true;
            this.labelCollectionDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionDateCategory.Location = new System.Drawing.Point(-35, 3);
            this.labelCollectionDateCategory.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelCollectionDateCategory.Name = "labelCollectionDateCategory";
            this.labelCollectionDateCategory.Size = new System.Drawing.Size(52, 24);
            this.labelCollectionDateCategory.TabIndex = 6;
            this.labelCollectionDateCategory.Text = "Category:";
            this.labelCollectionDateCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxCollectionDateCategory
            // 
            this.comboBoxCollectionDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollectionDateCategory.FormattingEnabled = true;
            this.comboBoxCollectionDateCategory.Location = new System.Drawing.Point(17, 3);
            this.comboBoxCollectionDateCategory.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxCollectionDateCategory.Name = "comboBoxCollectionDateCategory";
            this.comboBoxCollectionDateCategory.Size = new System.Drawing.Size(70, 21);
            this.comboBoxCollectionDateCategory.TabIndex = 3;
            // 
            // labelDataWithholdingReasonEvent
            // 
            this.labelDataWithholdingReasonEvent.AutoSize = true;
            this.labelDataWithholdingReasonEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDataWithholdingReasonEvent.Location = new System.Drawing.Point(-105, 73);
            this.labelDataWithholdingReasonEvent.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDataWithholdingReasonEvent.Name = "labelDataWithholdingReasonEvent";
            this.labelDataWithholdingReasonEvent.Size = new System.Drawing.Size(67, 24);
            this.labelDataWithholdingReasonEvent.TabIndex = 9;
            this.labelDataWithholdingReasonEvent.Text = "Withhold.R.:";
            this.labelDataWithholdingReasonEvent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCollectingMethod
            // 
            this.labelCollectingMethod.AutoSize = true;
            this.labelCollectingMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectingMethod.Location = new System.Drawing.Point(3, 73);
            this.labelCollectingMethod.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelCollectingMethod.Name = "labelCollectingMethod";
            this.labelCollectingMethod.Size = new System.Drawing.Size(56, 1);
            this.labelCollectingMethod.TabIndex = 16;
            this.labelCollectingMethod.Text = "Coll.meth.:";
            this.labelCollectingMethod.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxCollectingMethod
            // 
            this.tableLayoutPanelEvent.SetColumnSpan(this.textBoxCollectingMethod, 5);
            this.textBoxCollectingMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectingMethod.Location = new System.Drawing.Point(59, 67);
            this.textBoxCollectingMethod.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxCollectingMethod.Multiline = true;
            this.textBoxCollectingMethod.Name = "textBoxCollectingMethod";
            this.textBoxCollectingMethod.Size = new System.Drawing.Size(28, 1);
            this.textBoxCollectingMethod.TabIndex = 15;
            // 
            // labelEventReference
            // 
            this.labelEventReference.AutoSize = true;
            this.labelEventReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventReference.Location = new System.Drawing.Point(3, 47);
            this.labelEventReference.Name = "labelEventReference";
            this.labelEventReference.Size = new System.Drawing.Size(53, 26);
            this.labelEventReference.TabIndex = 8;
            this.labelEventReference.Text = "Ref.:";
            this.labelEventReference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEventNotes
            // 
            this.labelEventNotes.AutoSize = true;
            this.labelEventNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventNotes.Location = new System.Drawing.Point(3, 103);
            this.labelEventNotes.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelEventNotes.Name = "labelEventNotes";
            this.labelEventNotes.Size = new System.Drawing.Size(56, 1);
            this.labelEventNotes.TabIndex = 17;
            this.labelEventNotes.Text = "Notes:";
            this.labelEventNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxEventNotes
            // 
            this.tableLayoutPanelEvent.SetColumnSpan(this.textBoxEventNotes, 5);
            this.textBoxEventNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventNotes.Location = new System.Drawing.Point(59, 97);
            this.textBoxEventNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxEventNotes.Multiline = true;
            this.textBoxEventNotes.Name = "textBoxEventNotes";
            this.textBoxEventNotes.Size = new System.Drawing.Size(28, 1);
            this.textBoxEventNotes.TabIndex = 13;
            // 
            // labelCountryCache
            // 
            this.labelCountryCache.AutoSize = true;
            this.labelCountryCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountryCache.Location = new System.Drawing.Point(3, 73);
            this.labelCountryCache.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCountryCache.Name = "labelCountryCache";
            this.labelCountryCache.Size = new System.Drawing.Size(56, 24);
            this.labelCountryCache.TabIndex = 18;
            this.labelCountryCache.Text = "Country:";
            this.labelCountryCache.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCountryCache
            // 
            this.textBoxCountryCache.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelEvent.SetColumnSpan(this.textBoxCountryCache, 2);
            this.textBoxCountryCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCountryCache.Location = new System.Drawing.Point(59, 78);
            this.textBoxCountryCache.Margin = new System.Windows.Forms.Padding(0, 5, 0, 3);
            this.textBoxCountryCache.Name = "textBoxCountryCache";
            this.textBoxCountryCache.ReadOnly = true;
            this.textBoxCountryCache.Size = new System.Drawing.Size(1, 13);
            this.textBoxCountryCache.TabIndex = 19;
            this.textBoxCountryCache.TabStop = false;
            // 
            // labelCollectorsEventNumber
            // 
            this.labelCollectorsEventNumber.AutoSize = true;
            this.labelCollectorsEventNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectorsEventNumber.Location = new System.Drawing.Point(3, 27);
            this.labelCollectorsEventNumber.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCollectorsEventNumber.Name = "labelCollectorsEventNumber";
            this.labelCollectorsEventNumber.Size = new System.Drawing.Size(56, 20);
            this.labelCollectorsEventNumber.TabIndex = 20;
            this.labelCollectorsEventNumber.Text = "Nr.:";
            this.labelCollectorsEventNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectorsEventNumber
            // 
            this.textBoxCollectorsEventNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectorsEventNumber.Location = new System.Drawing.Point(59, 27);
            this.textBoxCollectorsEventNumber.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxCollectorsEventNumber.Name = "textBoxCollectorsEventNumber";
            this.textBoxCollectorsEventNumber.Size = new System.Drawing.Size(1, 20);
            this.textBoxCollectorsEventNumber.TabIndex = 12;
            // 
            // comboBoxDataWithholdingReasonEvent
            // 
            this.tableLayoutPanelEvent.SetColumnSpan(this.comboBoxDataWithholdingReasonEvent, 2);
            this.comboBoxDataWithholdingReasonEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDataWithholdingReasonEvent.FormattingEnabled = true;
            this.comboBoxDataWithholdingReasonEvent.Location = new System.Drawing.Point(-38, 73);
            this.comboBoxDataWithholdingReasonEvent.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.comboBoxDataWithholdingReasonEvent.Name = "comboBoxDataWithholdingReasonEvent";
            this.comboBoxDataWithholdingReasonEvent.Size = new System.Drawing.Size(125, 21);
            this.comboBoxDataWithholdingReasonEvent.TabIndex = 21;
            // 
            // splitContainerOverviewEventDescriptions
            // 
            this.splitContainerOverviewEventDescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOverviewEventDescriptions.Location = new System.Drawing.Point(0, 0);
            this.splitContainerOverviewEventDescriptions.Name = "splitContainerOverviewEventDescriptions";
            // 
            // splitContainerOverviewEventDescriptions.Panel1
            // 
            this.splitContainerOverviewEventDescriptions.Panel1.Controls.Add(this.textBoxEventLocality);
            this.splitContainerOverviewEventDescriptions.Panel1.Controls.Add(this.labelLocalityDescription);
            // 
            // splitContainerOverviewEventDescriptions.Panel2
            // 
            this.splitContainerOverviewEventDescriptions.Panel2.Controls.Add(this.textBoxHabitatDesciption);
            this.splitContainerOverviewEventDescriptions.Panel2.Controls.Add(this.labelHabitatDescription);
            this.splitContainerOverviewEventDescriptions.Size = new System.Drawing.Size(90, 41);
            this.splitContainerOverviewEventDescriptions.SplitterDistance = 44;
            this.splitContainerOverviewEventDescriptions.TabIndex = 0;
            // 
            // textBoxEventLocality
            // 
            this.textBoxEventLocality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventLocality.Location = new System.Drawing.Point(0, 13);
            this.textBoxEventLocality.Multiline = true;
            this.textBoxEventLocality.Name = "textBoxEventLocality";
            this.textBoxEventLocality.Size = new System.Drawing.Size(44, 28);
            this.textBoxEventLocality.TabIndex = 21;
            // 
            // labelLocalityDescription
            // 
            this.labelLocalityDescription.AutoSize = true;
            this.labelLocalityDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLocalityDescription.Location = new System.Drawing.Point(0, 0);
            this.labelLocalityDescription.Name = "labelLocalityDescription";
            this.labelLocalityDescription.Size = new System.Drawing.Size(125, 13);
            this.labelLocalityDescription.TabIndex = 22;
            this.labelLocalityDescription.Text = "Description of the locality";
            // 
            // textBoxHabitatDesciption
            // 
            this.textBoxHabitatDesciption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHabitatDesciption.Location = new System.Drawing.Point(0, 13);
            this.textBoxHabitatDesciption.Multiline = true;
            this.textBoxHabitatDesciption.Name = "textBoxHabitatDesciption";
            this.textBoxHabitatDesciption.Size = new System.Drawing.Size(42, 28);
            this.textBoxHabitatDesciption.TabIndex = 2;
            // 
            // labelHabitatDescription
            // 
            this.labelHabitatDescription.AutoSize = true;
            this.labelHabitatDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHabitatDescription.Location = new System.Drawing.Point(0, 0);
            this.labelHabitatDescription.Name = "labelHabitatDescription";
            this.labelHabitatDescription.Size = new System.Drawing.Size(125, 13);
            this.labelHabitatDescription.TabIndex = 3;
            this.labelHabitatDescription.Text = "Description of the habitat";
            // 
            // userControlSpecimenList
            // 
            this.userControlSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSpecimenList.Location = new System.Drawing.Point(0, 0);
            this.userControlSpecimenList.Name = "userControlSpecimenList";
            this.userControlSpecimenList.Size = new System.Drawing.Size(171, 540);
            this.userControlSpecimenList.TabIndex = 0;
            // 
            // imageListSpecimenList
            // 
            this.imageListSpecimenList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSpecimenList.ImageStream")));
            this.imageListSpecimenList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSpecimenList.Images.SetKeyName(0, "List.ico");
            this.imageListSpecimenList.Images.SetKeyName(1, "ListNot.ico");
            // 
            // collectionEventSeriesTableAdapter
            // 
            this.collectionEventSeriesTableAdapter.ClearBeforeFill = true;
            // 
            // buttonHistory
            // 
            this.buttonHistory.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonHistory.FlatAppearance.BorderSize = 0;
            this.buttonHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHistory.Image = global::DiversityCollection.Resource.History;
            this.buttonHistory.Location = new System.Drawing.Point(783, 0);
            this.buttonHistory.Name = "buttonHistory";
            this.buttonHistory.Size = new System.Drawing.Size(24, 24);
            this.buttonHistory.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonHistory, "Show the history of the selected dataset");
            this.buttonHistory.UseVisualStyleBackColor = true;
            this.buttonHistory.Click += new System.EventHandler(this.buttonHistory_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(807, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(24, 24);
            this.buttonFeedback.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback to the software developer");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // FormCollectionEventSeries
            // 
            this.ClientSize = new System.Drawing.Size(831, 591);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Controls.Add(this.panelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCollectionEventSeries";
            this.Text = "Collection event series";
            this.Load += new System.EventHandler(this.FormCollectionEventSeries_Load);
            this.panelHeader.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerData.Panel1.ResumeLayout(false);
            this.splitContainerData.Panel2.ResumeLayout(false);
            this.splitContainerData.ResumeLayout(false);
            this.groupBoxCollectionEventSeries.ResumeLayout(false);
            this.splitContainerHierarchy.Panel1.ResumeLayout(false);
            this.splitContainerHierarchy.Panel2.ResumeLayout(false);
            this.splitContainerHierarchy.ResumeLayout(false);
            this.tableLayoutPanelHierarchy.ResumeLayout(false);
            this.tableLayoutPanelHierarchy.PerformLayout();
            this.toolStripHierarchy.ResumeLayout(false);
            this.toolStripHierarchy.PerformLayout();
            this.splitContainerEventSeries.Panel1.ResumeLayout(false);
            this.splitContainerEventSeries.Panel2.ResumeLayout(false);
            this.splitContainerEventSeries.ResumeLayout(false);
            this.groupBoxEventSeries.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEventSeries)).EndInit();
            this.tableLayoutPanelOverviewEventSeries.ResumeLayout(false);
            this.tableLayoutPanelOverviewEventSeries.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionEventSeriesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionEventSeries)).EndInit();
            this.groupBoxEvent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEvent)).EndInit();
            this.splitContainerOverviewEventData.Panel1.ResumeLayout(false);
            this.splitContainerOverviewEventData.Panel2.ResumeLayout(false);
            this.splitContainerOverviewEventData.ResumeLayout(false);
            this.tableLayoutPanelEvent.ResumeLayout(false);
            this.tableLayoutPanelEvent.PerformLayout();
            this.splitContainerOverviewEventDescriptions.Panel1.ResumeLayout(false);
            this.splitContainerOverviewEventDescriptions.Panel1.PerformLayout();
            this.splitContainerOverviewEventDescriptions.Panel2.ResumeLayout(false);
            this.splitContainerOverviewEventDescriptions.Panel2.PerformLayout();
            this.splitContainerOverviewEventDescriptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #region Events

        private void FormCollectionEventSeries_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionEventSeries.CollectionEventSeries". Sie können sie bei Bedarf verschieben oder entfernen.
            // this.collectionEventSeriesTableAdapter.Fill(this.dataSetCollectionEventSeries.CollectionEventSeries);

        }

        private void dateTimePickerOverviewEventSeriesDateStart_CloseUp(object sender, EventArgs e)
        {
            if (this.collectionEventSeriesBindingSource.Current != null)
            {
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionEventSeriesBindingSource.Current;
                    R["DateStart"] = this.dateTimePickerOverviewEventSeriesDateStart.Value.Date.ToShortDateString();
                    this.textBoxOverviewEventSeriesDateStart.Text = this.dateTimePickerOverviewEventSeriesDateStart.Value.Date.ToShortDateString();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void dateTimePickerOverviewEventSeriesDateEnd_CloseUp(object sender, EventArgs e)
        {
            if (this.collectionEventSeriesBindingSource.Current != null)
            {
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionEventSeriesBindingSource.Current;
                    R["DateEnd"] = this.dateTimePickerOverviewEventSeriesDateEnd.Value.Date.ToShortDateString();
                    this.textBoxOverviewEventSeriesDateEnd.Text = this.dateTimePickerOverviewEventSeriesDateEnd.Value.Date.ToShortDateString();
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void dateTimePickerOverviewEventSeriesDateStart_DropDown(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.collectionEventSeriesBindingSource.Current;
            if (!R["DateStart"].Equals(System.DBNull.Value))
            {
                System.DateTime Start;
                if (System.DateTime.TryParse(R["DateStart"].ToString(), out Start))
                    this.dateTimePickerOverviewEventSeriesDateStart.Value = Start;
            }
        }

        private void dateTimePickerOverviewEventSeriesDateEnd_DropDown(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.collectionEventSeriesBindingSource.Current;
            if (!R["DateEnd"].Equals(System.DBNull.Value))
            {
                System.DateTime Start;
                if (System.DateTime.TryParse(R["DateEnd"].ToString(), out Start))
                    this.dateTimePickerOverviewEventSeriesDateEnd.Value = Start;
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (this.dataSetCollectionEventSeries.CollectionEventSeries.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select any data");
                return;
            }
            string Title = "History of event with ID: " + this.dataSetCollectionEventSeries.CollectionEventSeries.Rows[0]["SeriesID"].ToString();
            try
            {
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();
                LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.ID, "SeriesID", this.dataSetCollectionEventSeries.CollectionEventSeries.TableName, ""));
                LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.ID, "SeriesID", this.dataSetCollectionEventSeries.CollectionEventSeriesImage.TableName, ""));

                DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());

                f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "History");
                f.ShowDialog();
            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}