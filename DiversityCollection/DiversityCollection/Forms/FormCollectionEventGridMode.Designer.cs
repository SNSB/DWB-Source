namespace DiversityCollection.Forms
{
    partial class FormCollectionEventGridMode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Data withholding reason for collection event");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Country");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Locality description");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Habitat description");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Collectors event number");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Collecting method");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Collection event notes");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Collection event", new System.Windows.Forms.TreeNode[] { treeNode1, treeNode2, treeNode3, treeNode4, treeNode5, treeNode6, treeNode7 });
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Collection day");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Collection month");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Collection year");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Collection date supplement");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Collection time");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Collection time span");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Collection date and time", new System.Windows.Forms.TreeNode[] { treeNode9, treeNode10, treeNode11, treeNode12, treeNode13, treeNode14 });
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Named area");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Link to Gazetteer");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Remove link to Gazetteer");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Distance to location");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Direction to location");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Named area", new System.Windows.Forms.TreeNode[] { treeNode16, treeNode17, treeNode18, treeNode19, treeNode20 });
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Longitude");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Latitude");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Accuracy of coordinates");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Link to GoogleMaps");
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Coordinates WGS84", new System.Windows.Forms.TreeNode[] { treeNode22, treeNode23, treeNode24, treeNode25 });
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Altitude from");
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Altitude to");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Altitude accuracy");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Altitude (mNN)", new System.Windows.Forms.TreeNode[] { treeNode27, treeNode28, treeNode29 });
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("MTB");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Quadrant");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("Notes for MTB");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Accuracy of MTB");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("MTB (= TK25)", new System.Windows.Forms.TreeNode[] { treeNode31, treeNode32, treeNode33, treeNode34 });
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Sampling plot");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Link to SamplingPlots");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Remove link to SamplingPlots");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Sampling plot", new System.Windows.Forms.TreeNode[] { treeNode36, treeNode37, treeNode38 });
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Localisation", new System.Windows.Forms.TreeNode[] { treeNode21, treeNode26, treeNode30, treeNode35, treeNode39 });
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Geographic region");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Lithostratigraphy");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("Chronostratigraphy");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Collection site properties", new System.Windows.Forms.TreeNode[] { treeNode41, treeNode42, treeNode43 });
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCollectionEventGridMode));
            tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            textBoxHeaderNumber = new System.Windows.Forms.TextBox();
            firstLinesEventBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetCollectionEventGridMode = new DiversityCollection.Datasets.DataSetCollectionEventGridMode();
            textBoxHeaderTitle = new System.Windows.Forms.TextBox();
            labelHeaderNumber = new System.Windows.Forms.Label();
            labelHeaderID = new System.Windows.Forms.Label();
            labelHeaderVersion = new System.Windows.Forms.Label();
            tableLayoutPanelHeaderImageVisibility = new System.Windows.Forms.TableLayoutPanel();
            buttonHeaderShowTree = new System.Windows.Forms.Button();
            buttonHeaderShowImage = new System.Windows.Forms.Button();
            buttonHeaderShowSelectionTree = new System.Windows.Forms.Button();
            buttonFeedback = new System.Windows.Forms.Button();
            textBoxHeaderEventID = new System.Windows.Forms.TextBox();
            textBoxHeaderVersionEvent = new System.Windows.Forms.TextBox();
            collectionEventBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetCollectionSpecimen = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            labelHeaderEventID = new System.Windows.Forms.Label();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            splitContainerTreeView = new System.Windows.Forms.SplitContainer();
            splitContainerTrees = new System.Windows.Forms.SplitContainer();
            groupBoxGridModeOptions = new System.Windows.Forms.GroupBox();
            tableLayoutPanelGridModeOptions = new System.Windows.Forms.TableLayoutPanel();
            buttonGridRequery = new System.Windows.Forms.Button();
            treeViewGridModeFieldSelector = new System.Windows.Forms.TreeView();
            buttonGridModeUpdateColumnSettings = new System.Windows.Forms.Button();
            buttonResetSequence = new System.Windows.Forms.Button();
            splitContainerEventSeries = new System.Windows.Forms.SplitContainer();
            userControlEventSeriesTree = new DiversityCollection.UserControls.UserControlEventSeriesTree();
            dataGridViewEventSeries = new System.Windows.Forms.DataGridView();
            seriesIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            seriesParentIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            seriesCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dateStartDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            dateEndDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            collectionEventSeriesBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetCollectionEventSeries = new DiversityCollection.Datasets.DataSetCollectionEventSeries();
            groupBoxImage = new System.Windows.Forms.GroupBox();
            splitContainerImage = new System.Windows.Forms.SplitContainer();
            userControlImage = new DiversityWorkbench.UserControls.UserControlImage();
            panelSpecimenImageList = new System.Windows.Forms.Panel();
            listBoxImage = new System.Windows.Forms.ListBox();
            toolStripSpecimenImage = new System.Windows.Forms.ToolStrip();
            toolStripButtonImageNew = new System.Windows.Forms.ToolStripButton();
            toolStripButtonImageDelete = new System.Windows.Forms.ToolStripButton();
            tableLayoutPanelSpecimenImage = new System.Windows.Forms.TableLayoutPanel();
            labelSpecimenImageType = new System.Windows.Forms.Label();
            comboBoxImageType = new System.Windows.Forms.ComboBox();
            collectionEventImageBindingSource = new System.Windows.Forms.BindingSource(components);
            labelSpecimenImageNotes = new System.Windows.Forms.Label();
            textBoxImageNotes = new System.Windows.Forms.TextBox();
            labelSpecimenImageWithholdingReason = new System.Windows.Forms.Label();
            comboBoxImageWithholdingReason = new System.Windows.Forms.ComboBox();
            dataGridView = new System.Windows.Forms.DataGridView();
            collectionEventIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            datawithholdingreasonforcollectioneventDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectorseventnumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectiondayDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectionmonthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectionyearDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectiondatesupplementDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectiontimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectiontimespanDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            countryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            localitydescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            habitatdescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectingmethodDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            collectioneventnotesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            namedareaDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            namedAreaLocation2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            removelinktogazetteerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            distancetolocationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            directiontolocationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            longitudeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            latitudeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            coordinatesaccuracyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            linktoGoogleMapsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            altitudefromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            altitudetoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            altitudeaccuracyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            mTBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            quadrantDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            notesforMTBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            MTB_accuracy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            samplingplotDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            linktoSamplingPlotsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            removelinktoSamplingPlotsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            accuracyofsamplingplotDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            latitudeofsamplingplotDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            longitudeofsamplingplotDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            geographicregionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            lithostratigraphyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            chronostratigraphyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            collectionEventIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            coordinatesAverageLatitudeCacheDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            coordinatesAverageLongitudeCacheDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            coordinatesLocationNotesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            geographicRegionPropertyURIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            lithostratigraphyPropertyURIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            chronostratigraphyPropertyURIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            namedAverageLatitudeCacheDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            namedAverageLongitudeCacheDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            lithostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            chronostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            averageAltitudeCacheDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            tableLayoutPanelGridModeParameter = new System.Windows.Forms.TableLayoutPanel();
            labelGridViewReplaceColumn = new System.Windows.Forms.Label();
            buttonGridModeReplace = new System.Windows.Forms.Button();
            textBoxGridModeReplaceWith = new System.Windows.Forms.TextBox();
            labelGridModeReplaceWith = new System.Windows.Forms.Label();
            textBoxGridModeReplace = new System.Windows.Forms.TextBox();
            labelGridViewReplaceColumnName = new System.Windows.Forms.Label();
            buttonGridModeFind = new System.Windows.Forms.Button();
            radioButtonGridModeReplace = new System.Windows.Forms.RadioButton();
            radioButtonGridModeInsert = new System.Windows.Forms.RadioButton();
            buttonGridModeSave = new System.Windows.Forms.Button();
            radioButtonGridModeRemove = new System.Windows.Forms.RadioButton();
            buttonGridModeInsert = new System.Windows.Forms.Button();
            buttonGridModeRemove = new System.Windows.Forms.Button();
            buttonGridModeAppend = new System.Windows.Forms.Button();
            radioButtonGridModeAppend = new System.Windows.Forms.RadioButton();
            buttonGridModeCopy = new System.Windows.Forms.Button();
            buttonSaveAll = new System.Windows.Forms.Button();
            labelGridCounter = new System.Windows.Forms.Label();
            buttonGridModeUndo = new System.Windows.Forms.Button();
            buttonGridModeUndoSingleLine = new System.Windows.Forms.Button();
            buttonGridModePrint = new System.Windows.Forms.Button();
            progressBarSaveAll = new System.Windows.Forms.ProgressBar();
            comboBoxReplace = new System.Windows.Forms.ComboBox();
            comboBoxReplaceWith = new System.Windows.Forms.ComboBox();
            buttonMarkWholeColumn = new System.Windows.Forms.Button();
            buttonGridModeDelete = new System.Windows.Forms.Button();
            buttonOptRowHeight = new System.Windows.Forms.Button();
            buttonOptColumnWidth = new System.Windows.Forms.Button();
            contextMenuStripDataGrid = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItemCopyFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            imageListEventImages = new System.Windows.Forms.ImageList(components);
            imageListForm = new System.Windows.Forms.ImageList(components);
            firstLinesEventTableAdapter = new DiversityCollection.Datasets.DataSetCollectionEventGridModeTableAdapters.FirstLinesEvent_2TableAdapter();
            collectionEventImageTableAdapter = new DiversityCollection.Datasets.DataSetCollectionSpecimenTableAdapters.CollectionEventImageTableAdapter();
            collectionEventTableAdapter = new DiversityCollection.Datasets.DataSetCollectionSpecimenTableAdapters.CollectionEventTableAdapter();
            collectionEventSeriesTableAdapter = new DiversityCollection.Datasets.DataSetCollectionEventSeriesTableAdapters.CollectionEventSeriesTableAdapter();
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            tableLayoutPanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)firstLinesEventBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionEventGridMode).BeginInit();
            tableLayoutPanelHeaderImageVisibility.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)collectionEventBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionSpecimen).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTreeView).BeginInit();
            splitContainerTreeView.Panel1.SuspendLayout();
            splitContainerTreeView.Panel2.SuspendLayout();
            splitContainerTreeView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTrees).BeginInit();
            splitContainerTrees.Panel1.SuspendLayout();
            splitContainerTrees.Panel2.SuspendLayout();
            splitContainerTrees.SuspendLayout();
            groupBoxGridModeOptions.SuspendLayout();
            tableLayoutPanelGridModeOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerEventSeries).BeginInit();
            splitContainerEventSeries.Panel1.SuspendLayout();
            splitContainerEventSeries.Panel2.SuspendLayout();
            splitContainerEventSeries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEventSeries).BeginInit();
            ((System.ComponentModel.ISupportInitialize)collectionEventSeriesBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionEventSeries).BeginInit();
            groupBoxImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerImage).BeginInit();
            splitContainerImage.Panel1.SuspendLayout();
            splitContainerImage.Panel2.SuspendLayout();
            splitContainerImage.SuspendLayout();
            panelSpecimenImageList.SuspendLayout();
            toolStripSpecimenImage.SuspendLayout();
            tableLayoutPanelSpecimenImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)collectionEventImageBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            tableLayoutPanelGridModeParameter.SuspendLayout();
            contextMenuStripDataGrid.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelHeader
            // 
            tableLayoutPanelHeader.ColumnCount = 12;
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderNumber, 1, 1);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderTitle, 3, 0);
            tableLayoutPanelHeader.Controls.Add(labelHeaderNumber, 1, 0);
            tableLayoutPanelHeader.Controls.Add(labelHeaderID, 4, 1);
            tableLayoutPanelHeader.Controls.Add(labelHeaderVersion, 6, 0);
            tableLayoutPanelHeader.Controls.Add(tableLayoutPanelHeaderImageVisibility, 10, 0);
            tableLayoutPanelHeader.Controls.Add(buttonFeedback, 11, 0);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderEventID, 5, 1);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderVersionEvent, 6, 1);
            tableLayoutPanelHeader.Controls.Add(labelHeaderEventID, 5, 0);
            tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            tableLayoutPanelHeader.RowCount = 2;
            tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeader.Size = new System.Drawing.Size(1188, 40);
            tableLayoutPanelHeader.TabIndex = 3;
            // 
            // textBoxHeaderNumber
            // 
            textBoxHeaderNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", firstLinesEventBindingSource, "Collectors_event_number", true));
            textBoxHeaderNumber.Dock = System.Windows.Forms.DockStyle.Bottom;
            textBoxHeaderNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            textBoxHeaderNumber.Location = new System.Drawing.Point(0, 24);
            textBoxHeaderNumber.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            textBoxHeaderNumber.Name = "textBoxHeaderNumber";
            textBoxHeaderNumber.ReadOnly = true;
            textBoxHeaderNumber.Size = new System.Drawing.Size(267, 13);
            textBoxHeaderNumber.TabIndex = 3;
            textBoxHeaderNumber.TabStop = false;
            textBoxHeaderNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // firstLinesEventBindingSource
            // 
            firstLinesEventBindingSource.DataMember = "FirstLinesEvent_2";
            firstLinesEventBindingSource.DataSource = dataSetCollectionEventGridMode;
            // 
            // dataSetCollectionEventGridMode
            // 
            dataSetCollectionEventGridMode.DataSetName = "DataSetCollectionEventGridMode";
            dataSetCollectionEventGridMode.Namespace = "http://tempuri.org/DataSetCollectionEventGridMode.xsd";
            dataSetCollectionEventGridMode.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // textBoxHeaderTitle
            // 
            textBoxHeaderTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxHeaderTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            textBoxHeaderTitle.Location = new System.Drawing.Point(271, 1);
            textBoxHeaderTitle.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            textBoxHeaderTitle.Multiline = true;
            textBoxHeaderTitle.Name = "textBoxHeaderTitle";
            textBoxHeaderTitle.ReadOnly = true;
            tableLayoutPanelHeader.SetRowSpan(textBoxHeaderTitle, 2);
            textBoxHeaderTitle.Size = new System.Drawing.Size(615, 38);
            textBoxHeaderTitle.TabIndex = 4;
            textBoxHeaderTitle.TabStop = false;
            textBoxHeaderTitle.Text = "Taxonomic name";
            textBoxHeaderTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelHeaderNumber
            // 
            labelHeaderNumber.AccessibleDescription = "CollectionEvent.CollectorsEventNumber";
            labelHeaderNumber.AutoSize = true;
            labelHeaderNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            labelHeaderNumber.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelHeaderNumber.Location = new System.Drawing.Point(0, 0);
            labelHeaderNumber.Margin = new System.Windows.Forms.Padding(0);
            labelHeaderNumber.Name = "labelHeaderNumber";
            labelHeaderNumber.Size = new System.Drawing.Size(267, 13);
            labelHeaderNumber.TabIndex = 6;
            labelHeaderNumber.Text = "Event.No.";
            labelHeaderNumber.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelHeaderID
            // 
            labelHeaderID.AutoSize = true;
            labelHeaderID.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            labelHeaderID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelHeaderID.Location = new System.Drawing.Point(890, 13);
            labelHeaderID.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            labelHeaderID.Name = "labelHeaderID";
            labelHeaderID.Size = new System.Drawing.Size(21, 24);
            labelHeaderID.TabIndex = 7;
            labelHeaderID.Text = "ID:";
            labelHeaderID.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelHeaderVersion
            // 
            labelHeaderVersion.AutoSize = true;
            tableLayoutPanelHeader.SetColumnSpan(labelHeaderVersion, 2);
            labelHeaderVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            labelHeaderVersion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelHeaderVersion.Location = new System.Drawing.Point(1016, 0);
            labelHeaderVersion.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelHeaderVersion.Name = "labelHeaderVersion";
            labelHeaderVersion.Size = new System.Drawing.Size(48, 13);
            labelHeaderVersion.TabIndex = 8;
            labelHeaderVersion.Text = "Version";
            labelHeaderVersion.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tableLayoutPanelHeaderImageVisibility
            // 
            tableLayoutPanelHeaderImageVisibility.ColumnCount = 3;
            tableLayoutPanelHeaderImageVisibility.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelHeaderImageVisibility.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelHeaderImageVisibility.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            tableLayoutPanelHeaderImageVisibility.Controls.Add(buttonHeaderShowTree, 0, 1);
            tableLayoutPanelHeaderImageVisibility.Controls.Add(buttonHeaderShowImage, 2, 1);
            tableLayoutPanelHeaderImageVisibility.Controls.Add(buttonHeaderShowSelectionTree, 0, 1);
            tableLayoutPanelHeaderImageVisibility.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelHeaderImageVisibility.Location = new System.Drawing.Point(1064, 0);
            tableLayoutPanelHeaderImageVisibility.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            tableLayoutPanelHeaderImageVisibility.Name = "tableLayoutPanelHeaderImageVisibility";
            tableLayoutPanelHeaderImageVisibility.RowCount = 2;
            tableLayoutPanelHeader.SetRowSpan(tableLayoutPanelHeaderImageVisibility, 2);
            tableLayoutPanelHeaderImageVisibility.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelHeaderImageVisibility.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeaderImageVisibility.Size = new System.Drawing.Size(76, 40);
            tableLayoutPanelHeaderImageVisibility.TabIndex = 15;
            // 
            // buttonHeaderShowTree
            // 
            buttonHeaderShowTree.BackColor = System.Drawing.SystemColors.Control;
            buttonHeaderShowTree.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHeaderShowTree.FlatAppearance.BorderSize = 0;
            buttonHeaderShowTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonHeaderShowTree.Image = Resource.EventSeriesHierarchy;
            buttonHeaderShowTree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonHeaderShowTree.Location = new System.Drawing.Point(26, 6);
            buttonHeaderShowTree.Margin = new System.Windows.Forms.Padding(1, 6, 1, 6);
            buttonHeaderShowTree.Name = "buttonHeaderShowTree";
            buttonHeaderShowTree.Size = new System.Drawing.Size(23, 28);
            buttonHeaderShowTree.TabIndex = 4;
            buttonHeaderShowTree.UseVisualStyleBackColor = false;
            buttonHeaderShowTree.Click += buttonHeaderShowTree_Click;
            // 
            // buttonHeaderShowImage
            // 
            buttonHeaderShowImage.BackColor = System.Drawing.Color.Red;
            buttonHeaderShowImage.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHeaderShowImage.FlatAppearance.BorderSize = 0;
            buttonHeaderShowImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonHeaderShowImage.Image = Resource.Icones;
            buttonHeaderShowImage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonHeaderShowImage.Location = new System.Drawing.Point(51, 6);
            buttonHeaderShowImage.Margin = new System.Windows.Forms.Padding(1, 6, 1, 6);
            buttonHeaderShowImage.Name = "buttonHeaderShowImage";
            buttonHeaderShowImage.Size = new System.Drawing.Size(24, 28);
            buttonHeaderShowImage.TabIndex = 2;
            buttonHeaderShowImage.UseVisualStyleBackColor = false;
            buttonHeaderShowImage.Click += buttonHeaderShowSpecimenImage_Click;
            // 
            // buttonHeaderShowSelectionTree
            // 
            buttonHeaderShowSelectionTree.BackColor = System.Drawing.Color.Red;
            buttonHeaderShowSelectionTree.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHeaderShowSelectionTree.FlatAppearance.BorderSize = 0;
            buttonHeaderShowSelectionTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonHeaderShowSelectionTree.Image = Resource.BrowserOptions;
            buttonHeaderShowSelectionTree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonHeaderShowSelectionTree.Location = new System.Drawing.Point(0, 6);
            buttonHeaderShowSelectionTree.Margin = new System.Windows.Forms.Padding(0, 6, 1, 6);
            buttonHeaderShowSelectionTree.Name = "buttonHeaderShowSelectionTree";
            buttonHeaderShowSelectionTree.Size = new System.Drawing.Size(24, 28);
            buttonHeaderShowSelectionTree.TabIndex = 3;
            buttonHeaderShowSelectionTree.UseVisualStyleBackColor = false;
            buttonHeaderShowSelectionTree.Click += buttonHeaderShowSelectionTree_Click;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.Image = Resource.Feedback;
            buttonFeedback.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonFeedback.Location = new System.Drawing.Point(1144, 8);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 8, 4, 6);
            buttonFeedback.Name = "buttonFeedback";
            tableLayoutPanelHeader.SetRowSpan(buttonFeedback, 2);
            buttonFeedback.Size = new System.Drawing.Size(40, 26);
            buttonFeedback.TabIndex = 16;
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // textBoxHeaderEventID
            // 
            textBoxHeaderEventID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderEventID.DataBindings.Add(new System.Windows.Forms.Binding("Text", firstLinesEventBindingSource, "CollectionEventID", true));
            textBoxHeaderEventID.Dock = System.Windows.Forms.DockStyle.Bottom;
            textBoxHeaderEventID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            textBoxHeaderEventID.Location = new System.Drawing.Point(915, 24);
            textBoxHeaderEventID.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxHeaderEventID.Name = "textBoxHeaderEventID";
            textBoxHeaderEventID.ReadOnly = true;
            textBoxHeaderEventID.Size = new System.Drawing.Size(93, 13);
            textBoxHeaderEventID.TabIndex = 17;
            textBoxHeaderEventID.Text = "0000";
            textBoxHeaderEventID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxHeaderVersionEvent
            // 
            textBoxHeaderVersionEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tableLayoutPanelHeader.SetColumnSpan(textBoxHeaderVersionEvent, 2);
            textBoxHeaderVersionEvent.DataBindings.Add(new System.Windows.Forms.Binding("Text", collectionEventBindingSource, "Version", true));
            textBoxHeaderVersionEvent.Dock = System.Windows.Forms.DockStyle.Bottom;
            textBoxHeaderVersionEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            textBoxHeaderVersionEvent.Location = new System.Drawing.Point(1012, 24);
            textBoxHeaderVersionEvent.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            textBoxHeaderVersionEvent.Name = "textBoxHeaderVersionEvent";
            textBoxHeaderVersionEvent.ReadOnly = true;
            textBoxHeaderVersionEvent.Size = new System.Drawing.Size(52, 13);
            textBoxHeaderVersionEvent.TabIndex = 18;
            textBoxHeaderVersionEvent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // collectionEventBindingSource
            // 
            collectionEventBindingSource.DataMember = "CollectionEvent";
            collectionEventBindingSource.DataSource = dataSetCollectionSpecimen;
            // 
            // dataSetCollectionSpecimen
            // 
            dataSetCollectionSpecimen.DataSetName = "DataSetCollectionSpecimen";
            dataSetCollectionSpecimen.Namespace = "http://tempuri.org/DataSetCollectionSpecimen.xsd";
            dataSetCollectionSpecimen.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelHeaderEventID
            // 
            labelHeaderEventID.AccessibleName = "CollectionEvent";
            labelHeaderEventID.AutoSize = true;
            labelHeaderEventID.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderEventID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            labelHeaderEventID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelHeaderEventID.Location = new System.Drawing.Point(915, 0);
            labelHeaderEventID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeaderEventID.Name = "labelHeaderEventID";
            labelHeaderEventID.Size = new System.Drawing.Size(93, 13);
            labelHeaderEventID.TabIndex = 20;
            labelHeaderEventID.Text = "Event";
            labelHeaderEventID.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 40);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(splitContainerTreeView);
            splitContainerMain.Panel1.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(dataGridView);
            splitContainerMain.Panel2.Controls.Add(tableLayoutPanelGridModeParameter);
            splitContainerMain.Panel2.Padding = new System.Windows.Forms.Padding(5, 0, 5, 2);
            splitContainerMain.Size = new System.Drawing.Size(1188, 667);
            splitContainerMain.SplitterDistance = 270;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 4;
            // 
            // splitContainerTreeView
            // 
            splitContainerTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerTreeView.Location = new System.Drawing.Point(5, 0);
            splitContainerTreeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerTreeView.Name = "splitContainerTreeView";
            // 
            // splitContainerTreeView.Panel1
            // 
            splitContainerTreeView.Panel1.Controls.Add(splitContainerTrees);
            // 
            // splitContainerTreeView.Panel2
            // 
            splitContainerTreeView.Panel2.Controls.Add(groupBoxImage);
            splitContainerTreeView.Size = new System.Drawing.Size(1178, 270);
            splitContainerTreeView.SplitterDistance = 630;
            splitContainerTreeView.SplitterWidth = 5;
            splitContainerTreeView.TabIndex = 0;
            // 
            // splitContainerTrees
            // 
            splitContainerTrees.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerTrees.Location = new System.Drawing.Point(0, 0);
            splitContainerTrees.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerTrees.Name = "splitContainerTrees";
            // 
            // splitContainerTrees.Panel1
            // 
            splitContainerTrees.Panel1.Controls.Add(groupBoxGridModeOptions);
            // 
            // splitContainerTrees.Panel2
            // 
            splitContainerTrees.Panel2.Controls.Add(splitContainerEventSeries);
            splitContainerTrees.Size = new System.Drawing.Size(630, 270);
            splitContainerTrees.SplitterDistance = 310;
            splitContainerTrees.SplitterWidth = 5;
            splitContainerTrees.TabIndex = 4;
            // 
            // groupBoxGridModeOptions
            // 
            groupBoxGridModeOptions.AccessibleName = "Select_the_displayed_fields";
            groupBoxGridModeOptions.Controls.Add(tableLayoutPanelGridModeOptions);
            groupBoxGridModeOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxGridModeOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            groupBoxGridModeOptions.Location = new System.Drawing.Point(0, 0);
            groupBoxGridModeOptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxGridModeOptions.Name = "groupBoxGridModeOptions";
            groupBoxGridModeOptions.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxGridModeOptions.Size = new System.Drawing.Size(310, 270);
            groupBoxGridModeOptions.TabIndex = 3;
            groupBoxGridModeOptions.TabStop = false;
            groupBoxGridModeOptions.Text = "Select the displayed fields";
            // 
            // tableLayoutPanelGridModeOptions
            // 
            tableLayoutPanelGridModeOptions.ColumnCount = 3;
            tableLayoutPanelGridModeOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelGridModeOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeOptions.Controls.Add(buttonGridRequery, 0, 1);
            tableLayoutPanelGridModeOptions.Controls.Add(treeViewGridModeFieldSelector, 0, 0);
            tableLayoutPanelGridModeOptions.Controls.Add(buttonGridModeUpdateColumnSettings, 2, 1);
            tableLayoutPanelGridModeOptions.Controls.Add(buttonResetSequence, 1, 1);
            tableLayoutPanelGridModeOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelGridModeOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            tableLayoutPanelGridModeOptions.Location = new System.Drawing.Point(4, 16);
            tableLayoutPanelGridModeOptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelGridModeOptions.Name = "tableLayoutPanelGridModeOptions";
            tableLayoutPanelGridModeOptions.RowCount = 2;
            tableLayoutPanelGridModeOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelGridModeOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelGridModeOptions.Size = new System.Drawing.Size(302, 251);
            tableLayoutPanelGridModeOptions.TabIndex = 2;
            // 
            // buttonGridRequery
            // 
            buttonGridRequery.AccessibleName = "Requery";
            buttonGridRequery.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridRequery.Location = new System.Drawing.Point(4, 206);
            buttonGridRequery.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonGridRequery.Name = "buttonGridRequery";
            buttonGridRequery.Size = new System.Drawing.Size(76, 42);
            buttonGridRequery.TabIndex = 0;
            buttonGridRequery.Text = "Requery";
            toolTip.SetToolTip(buttonGridRequery, "Requery the data from the database without saving the current changes");
            buttonGridRequery.UseVisualStyleBackColor = true;
            buttonGridRequery.Click += buttonGridRequery_Click;
            // 
            // treeViewGridModeFieldSelector
            // 
            treeViewGridModeFieldSelector.CheckBoxes = true;
            tableLayoutPanelGridModeOptions.SetColumnSpan(treeViewGridModeFieldSelector, 3);
            treeViewGridModeFieldSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewGridModeFieldSelector.Location = new System.Drawing.Point(4, 3);
            treeViewGridModeFieldSelector.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            treeViewGridModeFieldSelector.Name = "treeViewGridModeFieldSelector";
            treeNode1.Name = "node_Data_withholding_reason_for_collection_event";
            treeNode1.Tag = "CollectionEvent;;;DataWithholdingReason;Data_withholding_reason_for_collection_event";
            treeNode1.Text = "Data withholding reason for collection event";
            treeNode2.Checked = true;
            treeNode2.Name = "NodeCountry";
            treeNode2.Tag = "CollectionEvent;;;CountryCache;Country";
            treeNode2.Text = "Country";
            treeNode3.Checked = true;
            treeNode3.Name = "NodeLocalityDescription";
            treeNode3.Tag = "CollectionEvent;;;LocalityDescription;Locality_description";
            treeNode3.Text = "Locality description";
            treeNode4.Name = "NodeHabitatDespription";
            treeNode4.Tag = "CollectionEvent;;;HabitatDescription;Habitat_description";
            treeNode4.Text = "Habitat description";
            treeNode5.Name = "NodeCollectorsEventNumber";
            treeNode5.Tag = "CollectionEvent;;;CollectorsEventNumber;Collectors_event_number";
            treeNode5.Text = "Collectors event number";
            treeNode6.Name = "NodeCollectingMethod";
            treeNode6.Tag = "CollectionEvent;;;CollectingMethod;Collecting_method";
            treeNode6.Text = "Collecting method";
            treeNode7.Name = "Node_Collection_event_notes";
            treeNode7.Tag = "CollectionEvent;;;Notes;Collection_event_notes";
            treeNode7.Text = "Collection event notes";
            treeNode8.Name = "NodeEvent";
            treeNode8.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode8.Tag = "CollectionEvent";
            treeNode8.Text = "Collection event";
            treeNode9.Checked = true;
            treeNode9.Name = "NodeCollectionDay";
            treeNode9.Tag = "CollectionEvent;;;CollectionDay;Collection_day";
            treeNode9.Text = "Collection day";
            treeNode10.Checked = true;
            treeNode10.Name = "NodeCollectionMonth";
            treeNode10.Tag = "CollectionEvent;;;CollectionMonth;Collection_month";
            treeNode10.Text = "Collection month";
            treeNode11.Checked = true;
            treeNode11.Name = "NodeCollectionYear";
            treeNode11.Tag = "CollectionEvent;;;CollectionYear;Collection_year";
            treeNode11.Text = "Collection year";
            treeNode12.Name = "Node_Collection_date_supplement";
            treeNode12.Tag = "CollectionEvent;;;CollectionDateSupplement;Collection_date_supplement";
            treeNode12.Text = "Collection date supplement";
            treeNode13.Name = "Node_Collection_time";
            treeNode13.Tag = "CollectionEvent;;;CollectionTime;Collection_time";
            treeNode13.Text = "Collection time";
            treeNode14.Name = "Node_Collection_time_span";
            treeNode14.Tag = "CollectionEvent;;;CollectionTimeSpan;Collection_time_span";
            treeNode14.Text = "Collection time span";
            treeNode15.Name = "NodeCollectionDate";
            treeNode15.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode15.Tag = "CollectionEvent.CollectionDate";
            treeNode15.Text = "Collection date and time";
            treeNode16.Name = "NodeNamedArea";
            treeNode16.Tag = "CollectionEventLocalisation;NamedArea;LocalisationSystemID=7;Location1;Named_area";
            treeNode16.Text = "Named area";
            treeNode17.ForeColor = System.Drawing.Color.Blue;
            treeNode17.Name = "NodeGazetteer";
            treeNode17.Tag = "CollectionEventLocalisation;NamedArea;LocalisationSystemID=7;Location2;NamedAreaLocation2";
            treeNode17.Text = "Link to Gazetteer";
            treeNode18.ForeColor = System.Drawing.Color.Red;
            treeNode18.Name = "NodeRemoveLinkToGazetteer";
            treeNode18.Tag = "CollectionEventLocalisation;NamedArea;LocalisationSystemID=7;Location2;Remove_link_to_gazetteer";
            treeNode18.Text = "Remove link to Gazetteer";
            treeNode19.Name = "Node_Distance_to_location";
            treeNode19.Tag = "CollectionEventLocalisation;NamedArea;LocalisationSystemID=7;DistanceToLocation;Distance_to_location";
            treeNode19.Text = "Distance to location";
            treeNode20.Name = "Node_Direction_to_location";
            treeNode20.Tag = "CollectionEventLocalisation;NamedArea;LocalisationSystemID=7;DirectionToLocation;Direction_to_location";
            treeNode20.Text = "Direction to location";
            treeNode21.ImageKey = "(Standard)";
            treeNode21.Name = "NodeNamedArea";
            treeNode21.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode21.Tag = "LocalisationSystem.LocalisationSystemID.7";
            treeNode21.Text = "Named area";
            treeNode22.Name = "NodeLongitude";
            treeNode22.Tag = "CollectionEventLocalisation;CoordinatesWGS84;LocalisationSystemID=8;Location1;Longitude";
            treeNode22.Text = "Longitude";
            treeNode23.Name = "NodeLatitude";
            treeNode23.Tag = "CollectionEventLocalisation;CoordinatesWGS84;LocalisationSystemID=8;Location2;Latitude";
            treeNode23.Text = "Latitude";
            treeNode24.Name = "NodeCoordinatesAccuracy";
            treeNode24.Tag = "CollectionEventLocalisation;CoordinatesWGS84;LocalisationSystemID=8;LocationAccuracy;Coordinates_accuracy";
            treeNode24.Text = "Accuracy of coordinates";
            treeNode25.ForeColor = System.Drawing.Color.Blue;
            treeNode25.Name = "NodeGoogleMaps";
            treeNode25.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            treeNode25.Tag = "CollectionEventLocalisation;CoordinatesWGS84;LocalisationSystemID=8;Notes;Link_to_GoogleMaps";
            treeNode25.Text = "Link to GoogleMaps";
            treeNode26.Name = "NodeCoordinates";
            treeNode26.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode26.Tag = "LocalisationSystem.LocalisationSystemID.8";
            treeNode26.Text = "Coordinates WGS84";
            treeNode27.Name = "NodeAltitudeFrom";
            treeNode27.Tag = "CollectionEventLocalisation;Altitude;LocalisationSystemID=4;Location1;Altitude_from";
            treeNode27.Text = "Altitude from";
            treeNode28.Name = "NodeAltitudeTo";
            treeNode28.Tag = "CollectionEventLocalisation;Altitude;LocalisationSystemID=4;Location2;Altitude_to";
            treeNode28.Text = "Altitude to";
            treeNode29.Name = "NodeAltitudeAccuracy";
            treeNode29.Tag = "CollectionEventLocalisation;Altitude;LocalisationSystemID=4;LocationAccuracy;Altitude_accuracy";
            treeNode29.Text = "Altitude accuracy";
            treeNode30.Name = "NodeAltitide";
            treeNode30.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode30.Tag = "LocalisationSystem.LocalisationSystemID.4";
            treeNode30.Text = "Altitude (mNN)";
            treeNode31.Name = "NodeMTB";
            treeNode31.Tag = "CollectionEventLocalisation;MTB;LocalisationSystemID=3;Location1;MTB";
            treeNode31.Text = "MTB";
            treeNode32.Name = "NodeQuadrant";
            treeNode32.Tag = "CollectionEventLocalisation;MTB;LocalisationSystemID=3;Location2;Quadrant";
            treeNode32.Text = "Quadrant";
            treeNode33.Name = "Node_Notes_for_MTB";
            treeNode33.Tag = "CollectionEventLocalisation;MTB;LocalisationSystemID=3;LocationNotes;Notes_for_MTB";
            treeNode33.Text = "Notes for MTB";
            treeNode34.Name = "NodeMtbAccuracy";
            treeNode34.Tag = "CollectionEventLocalisation;MTB;LocalisationSystemID=3;LocationAccuracy;MTB_accuracy";
            treeNode34.Text = "Accuracy of MTB";
            treeNode35.Name = "NodeMTB";
            treeNode35.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode35.Tag = "LocalisationSystem.LocalisationSystemID.3";
            treeNode35.Text = "MTB (= TK25)";
            treeNode36.Name = "NodeSamplingPlotDisplayText";
            treeNode36.Tag = "CollectionEventLocalisation;SamplingPlot;LocalisationSystemID=13;Location1;Sampling_plot";
            treeNode36.Text = "Sampling plot";
            treeNode37.ForeColor = System.Drawing.Color.Blue;
            treeNode37.Name = "NodeLinkForSamplingPlots";
            treeNode37.Tag = "CollectionEventLocalisation;SamplingPlot;LocalisationSystemID=13;Location2;Link_to_SamplingPlots";
            treeNode37.Text = "Link to SamplingPlots";
            treeNode38.ForeColor = System.Drawing.Color.Red;
            treeNode38.Name = "NodeRemoveLinkToSamplingPlots";
            treeNode38.Tag = "CollectionEventLocalisation;SamplingPlot;LocalisationSystemID=13;Location2;Remove_link_to_SamplingPlots";
            treeNode38.Text = "Remove link to SamplingPlots";
            treeNode39.Name = "NodeSamplingPlot";
            treeNode39.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode39.Tag = "LocalisationSystem.LocalisationSystemID.13";
            treeNode39.Text = "Sampling plot";
            treeNode40.ForeColor = System.Drawing.SystemColors.ControlText;
            treeNode40.Name = "NodeCoordinates";
            treeNode40.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode40.Tag = "CollectionEventLocalisation";
            treeNode40.Text = "Localisation";
            treeNode41.ForeColor = System.Drawing.Color.Blue;
            treeNode41.Name = "NodeGeographicRegion";
            treeNode41.Tag = "CollectionEventProperty;GeographicRegion;PropertyID=10;DisplayText;Geographic_region";
            treeNode41.Text = "Geographic region";
            treeNode42.ForeColor = System.Drawing.Color.Blue;
            treeNode42.Name = "NodeLithostratigraphy";
            treeNode42.Tag = "CollectionEventProperty;Lithostratigraphy;PropertyID=30;DisplayText;Lithostratigraphy";
            treeNode42.Text = "Lithostratigraphy";
            treeNode43.ForeColor = System.Drawing.Color.Blue;
            treeNode43.Name = "NodeChronostratigraphy";
            treeNode43.Tag = "CollectionEventProperty;Chronostratigraphy;PropertyID=20;DisplayText;Chronostratigraphy";
            treeNode43.Text = "Chronostratigraphy";
            treeNode44.Name = "NodeCollectionSiteProperties";
            treeNode44.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline);
            treeNode44.Tag = "CollectionEventProperty";
            treeNode44.Text = "Collection site properties";
            treeViewGridModeFieldSelector.Nodes.AddRange(new System.Windows.Forms.TreeNode[] { treeNode8, treeNode15, treeNode40, treeNode44 });
            treeViewGridModeFieldSelector.ShowNodeToolTips = true;
            treeViewGridModeFieldSelector.Size = new System.Drawing.Size(294, 197);
            treeViewGridModeFieldSelector.TabIndex = 1;
            treeViewGridModeFieldSelector.AfterCheck += treeViewGridModeFieldSelector_AfterCheck;
            // 
            // buttonGridModeUpdateColumnSettings
            // 
            buttonGridModeUpdateColumnSettings.AccessibleName = "Set_columns";
            buttonGridModeUpdateColumnSettings.Dock = System.Windows.Forms.DockStyle.Right;
            buttonGridModeUpdateColumnSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeUpdateColumnSettings.Location = new System.Drawing.Point(199, 206);
            buttonGridModeUpdateColumnSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonGridModeUpdateColumnSettings.Name = "buttonGridModeUpdateColumnSettings";
            buttonGridModeUpdateColumnSettings.Size = new System.Drawing.Size(99, 42);
            buttonGridModeUpdateColumnSettings.TabIndex = 8;
            buttonGridModeUpdateColumnSettings.Text = "Set columns";
            toolTip.SetToolTip(buttonGridModeUpdateColumnSettings, "Set the columns according to the settings in the tree above");
            buttonGridModeUpdateColumnSettings.UseVisualStyleBackColor = true;
            buttonGridModeUpdateColumnSettings.Click += buttonGridModeUpdateColumnSettings_Click;
            // 
            // buttonResetSequence
            // 
            buttonResetSequence.AccessibleName = "Reset_sequence";
            buttonResetSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonResetSequence.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonResetSequence.Location = new System.Drawing.Point(88, 206);
            buttonResetSequence.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonResetSequence.Name = "buttonResetSequence";
            buttonResetSequence.Size = new System.Drawing.Size(103, 42);
            buttonResetSequence.TabIndex = 9;
            buttonResetSequence.Text = "Reset sequence";
            toolTip.SetToolTip(buttonResetSequence, "Reset the sequence of the columns to the default sequence");
            buttonResetSequence.UseVisualStyleBackColor = true;
            buttonResetSequence.Click += buttonResetSequence_Click;
            // 
            // splitContainerEventSeries
            // 
            splitContainerEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerEventSeries.Location = new System.Drawing.Point(0, 0);
            splitContainerEventSeries.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerEventSeries.Name = "splitContainerEventSeries";
            splitContainerEventSeries.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerEventSeries.Panel1
            // 
            splitContainerEventSeries.Panel1.Controls.Add(userControlEventSeriesTree);
            // 
            // splitContainerEventSeries.Panel2
            // 
            splitContainerEventSeries.Panel2.Controls.Add(dataGridViewEventSeries);
            splitContainerEventSeries.Size = new System.Drawing.Size(315, 270);
            splitContainerEventSeries.SplitterDistance = 166;
            splitContainerEventSeries.SplitterWidth = 5;
            splitContainerEventSeries.TabIndex = 1;
            // 
            // userControlEventSeriesTree
            // 
            userControlEventSeriesTree.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlEventSeriesTree.Location = new System.Drawing.Point(0, 0);
            userControlEventSeriesTree.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlEventSeriesTree.Name = "userControlEventSeriesTree";
            userControlEventSeriesTree.Size = new System.Drawing.Size(315, 166);
            userControlEventSeriesTree.TabIndex = 0;
            // 
            // dataGridViewEventSeries
            // 
            dataGridViewEventSeries.AllowUserToAddRows = false;
            dataGridViewEventSeries.AutoGenerateColumns = false;
            dataGridViewEventSeries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewEventSeries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { seriesIDDataGridViewTextBoxColumn, seriesParentIDDataGridViewTextBoxColumn, descriptionDataGridViewTextBoxColumn, seriesCodeDataGridViewTextBoxColumn, notesDataGridViewTextBoxColumn, dateStartDataGridViewTextBoxColumn, dateEndDataGridViewTextBoxColumn });
            dataGridViewEventSeries.DataSource = collectionEventSeriesBindingSource;
            dataGridViewEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewEventSeries.Location = new System.Drawing.Point(0, 0);
            dataGridViewEventSeries.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridViewEventSeries.Name = "dataGridViewEventSeries";
            dataGridViewEventSeries.RowHeadersVisible = false;
            dataGridViewEventSeries.Size = new System.Drawing.Size(315, 99);
            dataGridViewEventSeries.TabIndex = 1;
            dataGridViewEventSeries.CellClick += dataGridViewEventSeries_CellClick;
            dataGridViewEventSeries.DataError += dataGridViewEventSeries_DataError;
            dataGridViewEventSeries.SizeChanged += dataGridViewEventSeries_SizeChanged;
            // 
            // seriesIDDataGridViewTextBoxColumn
            // 
            seriesIDDataGridViewTextBoxColumn.DataPropertyName = "SeriesID";
            seriesIDDataGridViewTextBoxColumn.HeaderText = "SeriesID";
            seriesIDDataGridViewTextBoxColumn.Name = "seriesIDDataGridViewTextBoxColumn";
            seriesIDDataGridViewTextBoxColumn.ReadOnly = true;
            seriesIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // seriesParentIDDataGridViewTextBoxColumn
            // 
            seriesParentIDDataGridViewTextBoxColumn.DataPropertyName = "SeriesParentID";
            seriesParentIDDataGridViewTextBoxColumn.HeaderText = "SeriesParentID";
            seriesParentIDDataGridViewTextBoxColumn.Name = "seriesParentIDDataGridViewTextBoxColumn";
            seriesParentIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            descriptionDataGridViewTextBoxColumn.Width = 80;
            // 
            // seriesCodeDataGridViewTextBoxColumn
            // 
            seriesCodeDataGridViewTextBoxColumn.DataPropertyName = "SeriesCode";
            seriesCodeDataGridViewTextBoxColumn.HeaderText = "Code";
            seriesCodeDataGridViewTextBoxColumn.Name = "seriesCodeDataGridViewTextBoxColumn";
            seriesCodeDataGridViewTextBoxColumn.Width = 40;
            // 
            // notesDataGridViewTextBoxColumn
            // 
            notesDataGridViewTextBoxColumn.DataPropertyName = "Notes";
            notesDataGridViewTextBoxColumn.HeaderText = "Notes";
            notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            notesDataGridViewTextBoxColumn.Width = 60;
            // 
            // dateStartDataGridViewTextBoxColumn
            // 
            dateStartDataGridViewTextBoxColumn.DataPropertyName = "DateStart";
            dateStartDataGridViewTextBoxColumn.HeaderText = "Start";
            dateStartDataGridViewTextBoxColumn.Name = "dateStartDataGridViewTextBoxColumn";
            dateStartDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            dateStartDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            dateStartDataGridViewTextBoxColumn.Width = 70;
            // 
            // dateEndDataGridViewTextBoxColumn
            // 
            dateEndDataGridViewTextBoxColumn.DataPropertyName = "DateEnd";
            dateEndDataGridViewTextBoxColumn.HeaderText = "End";
            dateEndDataGridViewTextBoxColumn.Name = "dateEndDataGridViewTextBoxColumn";
            dateEndDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            dateEndDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            dateEndDataGridViewTextBoxColumn.Width = 70;
            // 
            // collectionEventSeriesBindingSource
            // 
            collectionEventSeriesBindingSource.DataMember = "CollectionEventSeries";
            collectionEventSeriesBindingSource.DataSource = dataSetCollectionEventSeries;
            // 
            // dataSetCollectionEventSeries
            // 
            dataSetCollectionEventSeries.DataSetName = "DataSetCollectionEventSeries";
            dataSetCollectionEventSeries.Namespace = "http://tempuri.org/DataSetCollectionEventSeries.xsd";
            dataSetCollectionEventSeries.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // groupBoxImage
            // 
            groupBoxImage.AccessibleName = "CollectionEventImages";
            groupBoxImage.Controls.Add(splitContainerImage);
            groupBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            groupBoxImage.Location = new System.Drawing.Point(0, 0);
            groupBoxImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxImage.Name = "groupBoxImage";
            groupBoxImage.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxImage.Size = new System.Drawing.Size(543, 270);
            groupBoxImage.TabIndex = 2;
            groupBoxImage.TabStop = false;
            groupBoxImage.Tag = "CollectionEvent images";
            groupBoxImage.Text = "Event images";
            // 
            // splitContainerImage
            // 
            splitContainerImage.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerImage.Location = new System.Drawing.Point(4, 16);
            splitContainerImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerImage.Name = "splitContainerImage";
            // 
            // splitContainerImage.Panel1
            // 
            splitContainerImage.Panel1.Controls.Add(userControlImage);
            // 
            // splitContainerImage.Panel2
            // 
            splitContainerImage.Panel2.Controls.Add(panelSpecimenImageList);
            splitContainerImage.Panel2.Controls.Add(tableLayoutPanelSpecimenImage);
            splitContainerImage.Size = new System.Drawing.Size(535, 251);
            splitContainerImage.SplitterDistance = 458;
            splitContainerImage.SplitterWidth = 5;
            splitContainerImage.TabIndex = 0;
            // 
            // userControlImage
            // 
            userControlImage.AutorotationEnabled = false;
            userControlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            userControlImage.ImagePath = "";
            userControlImage.Location = new System.Drawing.Point(0, 0);
            userControlImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            userControlImage.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Image;
            userControlImage.Name = "userControlImage";
            userControlImage.Size = new System.Drawing.Size(458, 251);
            userControlImage.TabIndex = 0;
            // 
            // panelSpecimenImageList
            // 
            panelSpecimenImageList.Controls.Add(listBoxImage);
            panelSpecimenImageList.Controls.Add(toolStripSpecimenImage);
            panelSpecimenImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSpecimenImageList.Location = new System.Drawing.Point(0, 0);
            panelSpecimenImageList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelSpecimenImageList.Name = "panelSpecimenImageList";
            panelSpecimenImageList.Size = new System.Drawing.Size(72, 110);
            panelSpecimenImageList.TabIndex = 1;
            // 
            // listBoxImage
            // 
            listBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxImage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            listBoxImage.FormattingEnabled = true;
            listBoxImage.IntegralHeight = false;
            listBoxImage.ItemHeight = 50;
            listBoxImage.Location = new System.Drawing.Point(0, 0);
            listBoxImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxImage.Name = "listBoxImage";
            listBoxImage.ScrollAlwaysVisible = true;
            listBoxImage.Size = new System.Drawing.Size(48, 110);
            listBoxImage.TabIndex = 0;
            listBoxImage.DrawItem += listBoxImage_DrawItem;
            listBoxImage.MeasureItem += listBoxImage_MeasureItem;
            listBoxImage.SelectedIndexChanged += listBoxImage_SelectedIndexChanged;
            // 
            // toolStripSpecimenImage
            // 
            toolStripSpecimenImage.Dock = System.Windows.Forms.DockStyle.Right;
            toolStripSpecimenImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonImageNew, toolStripButtonImageDelete });
            toolStripSpecimenImage.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            toolStripSpecimenImage.Location = new System.Drawing.Point(48, 0);
            toolStripSpecimenImage.Name = "toolStripSpecimenImage";
            toolStripSpecimenImage.Size = new System.Drawing.Size(24, 110);
            toolStripSpecimenImage.TabIndex = 7;
            toolStripSpecimenImage.Text = "toolStrip1";
            // 
            // toolStripButtonImageNew
            // 
            toolStripButtonImageNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonImageNew.Image = Resource.New1;
            toolStripButtonImageNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonImageNew.Name = "toolStripButtonImageNew";
            toolStripButtonImageNew.Size = new System.Drawing.Size(23, 20);
            toolStripButtonImageNew.Text = "Insert a new image";
            toolStripButtonImageNew.Click += toolStripButtonImageNew_Click;
            // 
            // toolStripButtonImageDelete
            // 
            toolStripButtonImageDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonImageDelete.Image = Resource.Delete;
            toolStripButtonImageDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonImageDelete.Name = "toolStripButtonImageDelete";
            toolStripButtonImageDelete.Size = new System.Drawing.Size(23, 20);
            toolStripButtonImageDelete.Text = "Delete the selected image";
            toolStripButtonImageDelete.Click += toolStripButtonImageDelete_Click;
            // 
            // tableLayoutPanelSpecimenImage
            // 
            tableLayoutPanelSpecimenImage.ColumnCount = 1;
            tableLayoutPanelSpecimenImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelSpecimenImage.Controls.Add(labelSpecimenImageType, 0, 0);
            tableLayoutPanelSpecimenImage.Controls.Add(comboBoxImageType, 0, 1);
            tableLayoutPanelSpecimenImage.Controls.Add(labelSpecimenImageNotes, 0, 4);
            tableLayoutPanelSpecimenImage.Controls.Add(textBoxImageNotes, 0, 5);
            tableLayoutPanelSpecimenImage.Controls.Add(labelSpecimenImageWithholdingReason, 0, 2);
            tableLayoutPanelSpecimenImage.Controls.Add(comboBoxImageWithholdingReason, 0, 3);
            tableLayoutPanelSpecimenImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            tableLayoutPanelSpecimenImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            tableLayoutPanelSpecimenImage.Location = new System.Drawing.Point(0, 110);
            tableLayoutPanelSpecimenImage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            tableLayoutPanelSpecimenImage.Name = "tableLayoutPanelSpecimenImage";
            tableLayoutPanelSpecimenImage.RowCount = 6;
            tableLayoutPanelSpecimenImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSpecimenImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSpecimenImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSpecimenImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSpecimenImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelSpecimenImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSpecimenImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelSpecimenImage.Size = new System.Drawing.Size(72, 141);
            tableLayoutPanelSpecimenImage.TabIndex = 0;
            // 
            // labelSpecimenImageType
            // 
            labelSpecimenImageType.AutoSize = true;
            labelSpecimenImageType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSpecimenImageType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelSpecimenImageType.Location = new System.Drawing.Point(0, 0);
            labelSpecimenImageType.Margin = new System.Windows.Forms.Padding(0);
            labelSpecimenImageType.Name = "labelSpecimenImageType";
            labelSpecimenImageType.Size = new System.Drawing.Size(112, 13);
            labelSpecimenImageType.TabIndex = 3;
            labelSpecimenImageType.Text = "Type:";
            labelSpecimenImageType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxImageType
            // 
            comboBoxImageType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", collectionEventImageBindingSource, "ImageType", true));
            comboBoxImageType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxImageType.FormattingEnabled = true;
            comboBoxImageType.Location = new System.Drawing.Point(0, 13);
            comboBoxImageType.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            comboBoxImageType.Name = "comboBoxImageType";
            comboBoxImageType.Size = new System.Drawing.Size(112, 21);
            comboBoxImageType.TabIndex = 5;
            // 
            // collectionEventImageBindingSource
            // 
            collectionEventImageBindingSource.DataMember = "CollectionEventImage";
            collectionEventImageBindingSource.DataSource = dataSetCollectionSpecimen;
            // 
            // labelSpecimenImageNotes
            // 
            labelSpecimenImageNotes.AutoSize = true;
            labelSpecimenImageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSpecimenImageNotes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelSpecimenImageNotes.Location = new System.Drawing.Point(0, 71);
            labelSpecimenImageNotes.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            labelSpecimenImageNotes.Name = "labelSpecimenImageNotes";
            labelSpecimenImageNotes.Size = new System.Drawing.Size(112, 13);
            labelSpecimenImageNotes.TabIndex = 8;
            labelSpecimenImageNotes.Text = "Notes:";
            labelSpecimenImageNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxImageNotes
            // 
            textBoxImageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", collectionEventImageBindingSource, "Notes", true));
            textBoxImageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxImageNotes.Location = new System.Drawing.Point(0, 84);
            textBoxImageNotes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            textBoxImageNotes.Multiline = true;
            textBoxImageNotes.Name = "textBoxImageNotes";
            textBoxImageNotes.Size = new System.Drawing.Size(112, 56);
            textBoxImageNotes.TabIndex = 9;
            // 
            // labelSpecimenImageWithholdingReason
            // 
            labelSpecimenImageWithholdingReason.AutoSize = true;
            labelSpecimenImageWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSpecimenImageWithholdingReason.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelSpecimenImageWithholdingReason.Location = new System.Drawing.Point(0, 35);
            labelSpecimenImageWithholdingReason.Margin = new System.Windows.Forms.Padding(0);
            labelSpecimenImageWithholdingReason.Name = "labelSpecimenImageWithholdingReason";
            labelSpecimenImageWithholdingReason.Size = new System.Drawing.Size(112, 13);
            labelSpecimenImageWithholdingReason.TabIndex = 12;
            labelSpecimenImageWithholdingReason.Text = "Withh.:";
            labelSpecimenImageWithholdingReason.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxImageWithholdingReason
            // 
            comboBoxImageWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", collectionEventImageBindingSource, "DataWithholdingReason", true));
            comboBoxImageWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxImageWithholdingReason.FormattingEnabled = true;
            comboBoxImageWithholdingReason.Location = new System.Drawing.Point(0, 48);
            comboBoxImageWithholdingReason.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            comboBoxImageWithholdingReason.Name = "comboBoxImageWithholdingReason";
            comboBoxImageWithholdingReason.Size = new System.Drawing.Size(112, 21);
            comboBoxImageWithholdingReason.TabIndex = 13;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToOrderColumns = true;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnHeadersHeight = 60;
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { collectionEventIDDataGridViewTextBoxColumn, datawithholdingreasonforcollectioneventDataGridViewTextBoxColumn, collectorseventnumberDataGridViewTextBoxColumn, collectiondayDataGridViewTextBoxColumn, collectionmonthDataGridViewTextBoxColumn, collectionyearDataGridViewTextBoxColumn, collectiondatesupplementDataGridViewTextBoxColumn, collectiontimeDataGridViewTextBoxColumn, collectiontimespanDataGridViewTextBoxColumn, countryDataGridViewTextBoxColumn, localitydescriptionDataGridViewTextBoxColumn, habitatdescriptionDataGridViewTextBoxColumn, collectingmethodDataGridViewTextBoxColumn, collectioneventnotesDataGridViewTextBoxColumn, namedareaDataGridViewTextBoxColumn, namedAreaLocation2DataGridViewTextBoxColumn, removelinktogazetteerDataGridViewTextBoxColumn, distancetolocationDataGridViewTextBoxColumn, directiontolocationDataGridViewTextBoxColumn, longitudeDataGridViewTextBoxColumn, latitudeDataGridViewTextBoxColumn, coordinatesaccuracyDataGridViewTextBoxColumn, linktoGoogleMapsDataGridViewTextBoxColumn, altitudefromDataGridViewTextBoxColumn, altitudetoDataGridViewTextBoxColumn, altitudeaccuracyDataGridViewTextBoxColumn, mTBDataGridViewTextBoxColumn, quadrantDataGridViewTextBoxColumn, notesforMTBDataGridViewTextBoxColumn, MTB_accuracy, samplingplotDataGridViewTextBoxColumn, linktoSamplingPlotsDataGridViewTextBoxColumn, removelinktoSamplingPlotsDataGridViewTextBoxColumn, accuracyofsamplingplotDataGridViewTextBoxColumn, latitudeofsamplingplotDataGridViewTextBoxColumn, longitudeofsamplingplotDataGridViewTextBoxColumn, geographicregionDataGridViewTextBoxColumn, lithostratigraphyDataGridViewTextBoxColumn, chronostratigraphyDataGridViewTextBoxColumn, collectionEventIDDataGridViewTextBoxColumn1, coordinatesAverageLatitudeCacheDataGridViewTextBoxColumn, coordinatesAverageLongitudeCacheDataGridViewTextBoxColumn, coordinatesLocationNotesDataGridViewTextBoxColumn, geographicRegionPropertyURIDataGridViewTextBoxColumn, lithostratigraphyPropertyURIDataGridViewTextBoxColumn, chronostratigraphyPropertyURIDataGridViewTextBoxColumn, namedAverageLatitudeCacheDataGridViewTextBoxColumn, namedAverageLongitudeCacheDataGridViewTextBoxColumn, lithostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn, chronostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn, averageAltitudeCacheDataGridViewTextBoxColumn });
            dataGridView.DataSource = firstLinesEventBindingSource;
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(5, 32);
            dataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 20;
            dataGridView.Size = new System.Drawing.Size(1178, 358);
            dataGridView.TabIndex = 3;
            dataGridView.CellClick += dataGridView_CellClick;
            dataGridView.CellLeave += dataGridView_CellLeave;
            dataGridView.CellMouseUp += dataGridView_CellMouseUp;
            dataGridView.ColumnHeaderMouseClick += dataGridView_ColumnHeaderMouseClick;
            dataGridView.DataError += dataGridView_DataError;
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.RowEnter += dataGridView_RowEnter;
            dataGridView.RowHeaderMouseClick += dataGridView_RowHeaderMouseClick;
            dataGridView.RowPostPaint += dataGridView_RowPostPaint;
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;
            // 
            // collectionEventIDDataGridViewTextBoxColumn
            // 
            collectionEventIDDataGridViewTextBoxColumn.DataPropertyName = "CollectionEventID";
            collectionEventIDDataGridViewTextBoxColumn.HeaderText = "CollectionEventID";
            collectionEventIDDataGridViewTextBoxColumn.Name = "collectionEventIDDataGridViewTextBoxColumn";
            // 
            // datawithholdingreasonforcollectioneventDataGridViewTextBoxColumn
            // 
            datawithholdingreasonforcollectioneventDataGridViewTextBoxColumn.DataPropertyName = "Data_withholding_reason_for_collection_event";
            datawithholdingreasonforcollectioneventDataGridViewTextBoxColumn.HeaderText = "Data_withholding_reason_for_collection_event";
            datawithholdingreasonforcollectioneventDataGridViewTextBoxColumn.Name = "datawithholdingreasonforcollectioneventDataGridViewTextBoxColumn";
            // 
            // collectorseventnumberDataGridViewTextBoxColumn
            // 
            collectorseventnumberDataGridViewTextBoxColumn.DataPropertyName = "Collectors_event_number";
            collectorseventnumberDataGridViewTextBoxColumn.HeaderText = "Collectors_event_number";
            collectorseventnumberDataGridViewTextBoxColumn.Name = "collectorseventnumberDataGridViewTextBoxColumn";
            // 
            // collectiondayDataGridViewTextBoxColumn
            // 
            collectiondayDataGridViewTextBoxColumn.DataPropertyName = "Collection_day";
            collectiondayDataGridViewTextBoxColumn.HeaderText = "Collection_day";
            collectiondayDataGridViewTextBoxColumn.Name = "collectiondayDataGridViewTextBoxColumn";
            // 
            // collectionmonthDataGridViewTextBoxColumn
            // 
            collectionmonthDataGridViewTextBoxColumn.DataPropertyName = "Collection_month";
            collectionmonthDataGridViewTextBoxColumn.HeaderText = "Collection_month";
            collectionmonthDataGridViewTextBoxColumn.Name = "collectionmonthDataGridViewTextBoxColumn";
            // 
            // collectionyearDataGridViewTextBoxColumn
            // 
            collectionyearDataGridViewTextBoxColumn.DataPropertyName = "Collection_year";
            collectionyearDataGridViewTextBoxColumn.HeaderText = "Collection_year";
            collectionyearDataGridViewTextBoxColumn.Name = "collectionyearDataGridViewTextBoxColumn";
            // 
            // collectiondatesupplementDataGridViewTextBoxColumn
            // 
            collectiondatesupplementDataGridViewTextBoxColumn.DataPropertyName = "Collection_date_supplement";
            collectiondatesupplementDataGridViewTextBoxColumn.HeaderText = "Collection_date_supplement";
            collectiondatesupplementDataGridViewTextBoxColumn.Name = "collectiondatesupplementDataGridViewTextBoxColumn";
            // 
            // collectiontimeDataGridViewTextBoxColumn
            // 
            collectiontimeDataGridViewTextBoxColumn.DataPropertyName = "Collection_time";
            collectiontimeDataGridViewTextBoxColumn.HeaderText = "Collection_time";
            collectiontimeDataGridViewTextBoxColumn.Name = "collectiontimeDataGridViewTextBoxColumn";
            // 
            // collectiontimespanDataGridViewTextBoxColumn
            // 
            collectiontimespanDataGridViewTextBoxColumn.DataPropertyName = "Collection_time_span";
            collectiontimespanDataGridViewTextBoxColumn.HeaderText = "Collection_time_span";
            collectiontimespanDataGridViewTextBoxColumn.Name = "collectiontimespanDataGridViewTextBoxColumn";
            // 
            // countryDataGridViewTextBoxColumn
            // 
            countryDataGridViewTextBoxColumn.DataPropertyName = "Country";
            countryDataGridViewTextBoxColumn.HeaderText = "Country";
            countryDataGridViewTextBoxColumn.Name = "countryDataGridViewTextBoxColumn";
            // 
            // localitydescriptionDataGridViewTextBoxColumn
            // 
            localitydescriptionDataGridViewTextBoxColumn.DataPropertyName = "Locality_description";
            localitydescriptionDataGridViewTextBoxColumn.HeaderText = "Locality_description";
            localitydescriptionDataGridViewTextBoxColumn.Name = "localitydescriptionDataGridViewTextBoxColumn";
            // 
            // habitatdescriptionDataGridViewTextBoxColumn
            // 
            habitatdescriptionDataGridViewTextBoxColumn.DataPropertyName = "Habitat_description";
            habitatdescriptionDataGridViewTextBoxColumn.HeaderText = "Habitat_description";
            habitatdescriptionDataGridViewTextBoxColumn.Name = "habitatdescriptionDataGridViewTextBoxColumn";
            // 
            // collectingmethodDataGridViewTextBoxColumn
            // 
            collectingmethodDataGridViewTextBoxColumn.DataPropertyName = "Collecting_method";
            collectingmethodDataGridViewTextBoxColumn.HeaderText = "Collecting_method";
            collectingmethodDataGridViewTextBoxColumn.Name = "collectingmethodDataGridViewTextBoxColumn";
            // 
            // collectioneventnotesDataGridViewTextBoxColumn
            // 
            collectioneventnotesDataGridViewTextBoxColumn.DataPropertyName = "Collection_event_notes";
            collectioneventnotesDataGridViewTextBoxColumn.HeaderText = "Collection_event_notes";
            collectioneventnotesDataGridViewTextBoxColumn.Name = "collectioneventnotesDataGridViewTextBoxColumn";
            // 
            // namedareaDataGridViewTextBoxColumn
            // 
            namedareaDataGridViewTextBoxColumn.DataPropertyName = "Named_area";
            namedareaDataGridViewTextBoxColumn.HeaderText = "Named_area";
            namedareaDataGridViewTextBoxColumn.Name = "namedareaDataGridViewTextBoxColumn";
            // 
            // namedAreaLocation2DataGridViewTextBoxColumn
            // 
            namedAreaLocation2DataGridViewTextBoxColumn.DataPropertyName = "NamedAreaLocation2";
            namedAreaLocation2DataGridViewTextBoxColumn.HeaderText = "Link to gazetteer";
            namedAreaLocation2DataGridViewTextBoxColumn.Name = "namedAreaLocation2DataGridViewTextBoxColumn";
            namedAreaLocation2DataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            namedAreaLocation2DataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // removelinktogazetteerDataGridViewTextBoxColumn
            // 
            removelinktogazetteerDataGridViewTextBoxColumn.DataPropertyName = "Remove_link_to_gazetteer";
            removelinktogazetteerDataGridViewTextBoxColumn.HeaderText = "Remove_link_to_gazetteer";
            removelinktogazetteerDataGridViewTextBoxColumn.Name = "removelinktogazetteerDataGridViewTextBoxColumn";
            removelinktogazetteerDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            removelinktogazetteerDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            removelinktogazetteerDataGridViewTextBoxColumn.Text = "X";
            removelinktogazetteerDataGridViewTextBoxColumn.UseColumnTextForButtonValue = true;
            // 
            // distancetolocationDataGridViewTextBoxColumn
            // 
            distancetolocationDataGridViewTextBoxColumn.DataPropertyName = "Distance_to_location";
            distancetolocationDataGridViewTextBoxColumn.HeaderText = "Distance_to_location";
            distancetolocationDataGridViewTextBoxColumn.Name = "distancetolocationDataGridViewTextBoxColumn";
            // 
            // directiontolocationDataGridViewTextBoxColumn
            // 
            directiontolocationDataGridViewTextBoxColumn.DataPropertyName = "Direction_to_location";
            directiontolocationDataGridViewTextBoxColumn.HeaderText = "Direction_to_location";
            directiontolocationDataGridViewTextBoxColumn.Name = "directiontolocationDataGridViewTextBoxColumn";
            // 
            // longitudeDataGridViewTextBoxColumn
            // 
            longitudeDataGridViewTextBoxColumn.DataPropertyName = "Longitude";
            longitudeDataGridViewTextBoxColumn.HeaderText = "Longitude";
            longitudeDataGridViewTextBoxColumn.Name = "longitudeDataGridViewTextBoxColumn";
            // 
            // latitudeDataGridViewTextBoxColumn
            // 
            latitudeDataGridViewTextBoxColumn.DataPropertyName = "Latitude";
            latitudeDataGridViewTextBoxColumn.HeaderText = "Latitude";
            latitudeDataGridViewTextBoxColumn.Name = "latitudeDataGridViewTextBoxColumn";
            // 
            // coordinatesaccuracyDataGridViewTextBoxColumn
            // 
            coordinatesaccuracyDataGridViewTextBoxColumn.DataPropertyName = "Coordinates_accuracy";
            coordinatesaccuracyDataGridViewTextBoxColumn.HeaderText = "Coordinates_accuracy";
            coordinatesaccuracyDataGridViewTextBoxColumn.Name = "coordinatesaccuracyDataGridViewTextBoxColumn";
            // 
            // linktoGoogleMapsDataGridViewTextBoxColumn
            // 
            linktoGoogleMapsDataGridViewTextBoxColumn.DataPropertyName = "Link_to_GoogleMaps";
            linktoGoogleMapsDataGridViewTextBoxColumn.HeaderText = "Link_to_GoogleMaps";
            linktoGoogleMapsDataGridViewTextBoxColumn.Name = "linktoGoogleMapsDataGridViewTextBoxColumn";
            linktoGoogleMapsDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            linktoGoogleMapsDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // altitudefromDataGridViewTextBoxColumn
            // 
            altitudefromDataGridViewTextBoxColumn.DataPropertyName = "Altitude_from";
            altitudefromDataGridViewTextBoxColumn.HeaderText = "Altitude_from";
            altitudefromDataGridViewTextBoxColumn.Name = "altitudefromDataGridViewTextBoxColumn";
            // 
            // altitudetoDataGridViewTextBoxColumn
            // 
            altitudetoDataGridViewTextBoxColumn.DataPropertyName = "Altitude_to";
            altitudetoDataGridViewTextBoxColumn.HeaderText = "Altitude_to";
            altitudetoDataGridViewTextBoxColumn.Name = "altitudetoDataGridViewTextBoxColumn";
            // 
            // altitudeaccuracyDataGridViewTextBoxColumn
            // 
            altitudeaccuracyDataGridViewTextBoxColumn.DataPropertyName = "Altitude_accuracy";
            altitudeaccuracyDataGridViewTextBoxColumn.HeaderText = "Altitude_accuracy";
            altitudeaccuracyDataGridViewTextBoxColumn.Name = "altitudeaccuracyDataGridViewTextBoxColumn";
            // 
            // mTBDataGridViewTextBoxColumn
            // 
            mTBDataGridViewTextBoxColumn.DataPropertyName = "MTB";
            mTBDataGridViewTextBoxColumn.HeaderText = "MTB";
            mTBDataGridViewTextBoxColumn.Name = "mTBDataGridViewTextBoxColumn";
            // 
            // quadrantDataGridViewTextBoxColumn
            // 
            quadrantDataGridViewTextBoxColumn.DataPropertyName = "Quadrant";
            quadrantDataGridViewTextBoxColumn.HeaderText = "Quadrant";
            quadrantDataGridViewTextBoxColumn.Name = "quadrantDataGridViewTextBoxColumn";
            // 
            // notesforMTBDataGridViewTextBoxColumn
            // 
            notesforMTBDataGridViewTextBoxColumn.DataPropertyName = "Notes_for_MTB";
            notesforMTBDataGridViewTextBoxColumn.HeaderText = "Notes_for_MTB";
            notesforMTBDataGridViewTextBoxColumn.Name = "notesforMTBDataGridViewTextBoxColumn";
            // 
            // MTB_accuracy
            // 
            MTB_accuracy.DataPropertyName = "MTB_accuracy";
            MTB_accuracy.HeaderText = "MTB_accuracy";
            MTB_accuracy.Name = "MTB_accuracy";
            // 
            // samplingplotDataGridViewTextBoxColumn
            // 
            samplingplotDataGridViewTextBoxColumn.DataPropertyName = "Sampling_plot";
            samplingplotDataGridViewTextBoxColumn.HeaderText = "Sampling_plot";
            samplingplotDataGridViewTextBoxColumn.Name = "samplingplotDataGridViewTextBoxColumn";
            // 
            // linktoSamplingPlotsDataGridViewTextBoxColumn
            // 
            linktoSamplingPlotsDataGridViewTextBoxColumn.DataPropertyName = "Link_to_SamplingPlots";
            linktoSamplingPlotsDataGridViewTextBoxColumn.HeaderText = "Link_to_SamplingPlots";
            linktoSamplingPlotsDataGridViewTextBoxColumn.Name = "linktoSamplingPlotsDataGridViewTextBoxColumn";
            linktoSamplingPlotsDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            linktoSamplingPlotsDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // removelinktoSamplingPlotsDataGridViewTextBoxColumn
            // 
            removelinktoSamplingPlotsDataGridViewTextBoxColumn.DataPropertyName = "Remove_link_to_SamplingPlots";
            removelinktoSamplingPlotsDataGridViewTextBoxColumn.HeaderText = "Remove_link_to_SamplingPlots";
            removelinktoSamplingPlotsDataGridViewTextBoxColumn.Name = "removelinktoSamplingPlotsDataGridViewTextBoxColumn";
            removelinktoSamplingPlotsDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            removelinktoSamplingPlotsDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            removelinktoSamplingPlotsDataGridViewTextBoxColumn.ToolTipText = "X";
            removelinktoSamplingPlotsDataGridViewTextBoxColumn.UseColumnTextForButtonValue = true;
            // 
            // accuracyofsamplingplotDataGridViewTextBoxColumn
            // 
            accuracyofsamplingplotDataGridViewTextBoxColumn.DataPropertyName = "Accuracy_of_sampling_plot";
            accuracyofsamplingplotDataGridViewTextBoxColumn.HeaderText = "Accuracy_of_sampling_plot";
            accuracyofsamplingplotDataGridViewTextBoxColumn.Name = "accuracyofsamplingplotDataGridViewTextBoxColumn";
            // 
            // latitudeofsamplingplotDataGridViewTextBoxColumn
            // 
            latitudeofsamplingplotDataGridViewTextBoxColumn.DataPropertyName = "Latitude_of_sampling_plot";
            latitudeofsamplingplotDataGridViewTextBoxColumn.HeaderText = "Latitude_of_sampling_plot";
            latitudeofsamplingplotDataGridViewTextBoxColumn.Name = "latitudeofsamplingplotDataGridViewTextBoxColumn";
            // 
            // longitudeofsamplingplotDataGridViewTextBoxColumn
            // 
            longitudeofsamplingplotDataGridViewTextBoxColumn.DataPropertyName = "Longitude_of_sampling_plot";
            longitudeofsamplingplotDataGridViewTextBoxColumn.HeaderText = "Longitude_of_sampling_plot";
            longitudeofsamplingplotDataGridViewTextBoxColumn.Name = "longitudeofsamplingplotDataGridViewTextBoxColumn";
            // 
            // geographicregionDataGridViewTextBoxColumn
            // 
            geographicregionDataGridViewTextBoxColumn.DataPropertyName = "Geographic_region";
            geographicregionDataGridViewTextBoxColumn.HeaderText = "Geographic_region";
            geographicregionDataGridViewTextBoxColumn.Name = "geographicregionDataGridViewTextBoxColumn";
            geographicregionDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            geographicregionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lithostratigraphyDataGridViewTextBoxColumn
            // 
            lithostratigraphyDataGridViewTextBoxColumn.DataPropertyName = "Lithostratigraphy";
            lithostratigraphyDataGridViewTextBoxColumn.HeaderText = "Lithostratigraphy";
            lithostratigraphyDataGridViewTextBoxColumn.Name = "lithostratigraphyDataGridViewTextBoxColumn";
            lithostratigraphyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            lithostratigraphyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // chronostratigraphyDataGridViewTextBoxColumn
            // 
            chronostratigraphyDataGridViewTextBoxColumn.DataPropertyName = "Chronostratigraphy";
            chronostratigraphyDataGridViewTextBoxColumn.HeaderText = "Chronostratigraphy";
            chronostratigraphyDataGridViewTextBoxColumn.Name = "chronostratigraphyDataGridViewTextBoxColumn";
            chronostratigraphyDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            chronostratigraphyDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // collectionEventIDDataGridViewTextBoxColumn1
            // 
            collectionEventIDDataGridViewTextBoxColumn1.DataPropertyName = "_CollectionEventID";
            collectionEventIDDataGridViewTextBoxColumn1.HeaderText = "_CollectionEventID";
            collectionEventIDDataGridViewTextBoxColumn1.Name = "collectionEventIDDataGridViewTextBoxColumn1";
            // 
            // coordinatesAverageLatitudeCacheDataGridViewTextBoxColumn
            // 
            coordinatesAverageLatitudeCacheDataGridViewTextBoxColumn.DataPropertyName = "_CoordinatesAverageLatitudeCache";
            coordinatesAverageLatitudeCacheDataGridViewTextBoxColumn.HeaderText = "_CoordinatesAverageLatitudeCache";
            coordinatesAverageLatitudeCacheDataGridViewTextBoxColumn.Name = "coordinatesAverageLatitudeCacheDataGridViewTextBoxColumn";
            // 
            // coordinatesAverageLongitudeCacheDataGridViewTextBoxColumn
            // 
            coordinatesAverageLongitudeCacheDataGridViewTextBoxColumn.DataPropertyName = "_CoordinatesAverageLongitudeCache";
            coordinatesAverageLongitudeCacheDataGridViewTextBoxColumn.HeaderText = "_CoordinatesAverageLongitudeCache";
            coordinatesAverageLongitudeCacheDataGridViewTextBoxColumn.Name = "coordinatesAverageLongitudeCacheDataGridViewTextBoxColumn";
            // 
            // coordinatesLocationNotesDataGridViewTextBoxColumn
            // 
            coordinatesLocationNotesDataGridViewTextBoxColumn.DataPropertyName = "_CoordinatesLocationNotes";
            coordinatesLocationNotesDataGridViewTextBoxColumn.HeaderText = "_CoordinatesLocationNotes";
            coordinatesLocationNotesDataGridViewTextBoxColumn.Name = "coordinatesLocationNotesDataGridViewTextBoxColumn";
            // 
            // geographicRegionPropertyURIDataGridViewTextBoxColumn
            // 
            geographicRegionPropertyURIDataGridViewTextBoxColumn.DataPropertyName = "_GeographicRegionPropertyURI";
            geographicRegionPropertyURIDataGridViewTextBoxColumn.HeaderText = "_GeographicRegionPropertyURI";
            geographicRegionPropertyURIDataGridViewTextBoxColumn.Name = "geographicRegionPropertyURIDataGridViewTextBoxColumn";
            // 
            // lithostratigraphyPropertyURIDataGridViewTextBoxColumn
            // 
            lithostratigraphyPropertyURIDataGridViewTextBoxColumn.DataPropertyName = "_LithostratigraphyPropertyURI";
            lithostratigraphyPropertyURIDataGridViewTextBoxColumn.HeaderText = "_LithostratigraphyPropertyURI";
            lithostratigraphyPropertyURIDataGridViewTextBoxColumn.Name = "lithostratigraphyPropertyURIDataGridViewTextBoxColumn";
            // 
            // chronostratigraphyPropertyURIDataGridViewTextBoxColumn
            // 
            chronostratigraphyPropertyURIDataGridViewTextBoxColumn.DataPropertyName = "_ChronostratigraphyPropertyURI";
            chronostratigraphyPropertyURIDataGridViewTextBoxColumn.HeaderText = "_ChronostratigraphyPropertyURI";
            chronostratigraphyPropertyURIDataGridViewTextBoxColumn.Name = "chronostratigraphyPropertyURIDataGridViewTextBoxColumn";
            // 
            // namedAverageLatitudeCacheDataGridViewTextBoxColumn
            // 
            namedAverageLatitudeCacheDataGridViewTextBoxColumn.DataPropertyName = "_NamedAverageLatitudeCache";
            namedAverageLatitudeCacheDataGridViewTextBoxColumn.HeaderText = "_NamedAverageLatitudeCache";
            namedAverageLatitudeCacheDataGridViewTextBoxColumn.Name = "namedAverageLatitudeCacheDataGridViewTextBoxColumn";
            // 
            // namedAverageLongitudeCacheDataGridViewTextBoxColumn
            // 
            namedAverageLongitudeCacheDataGridViewTextBoxColumn.DataPropertyName = "_NamedAverageLongitudeCache";
            namedAverageLongitudeCacheDataGridViewTextBoxColumn.HeaderText = "_NamedAverageLongitudeCache";
            namedAverageLongitudeCacheDataGridViewTextBoxColumn.Name = "namedAverageLongitudeCacheDataGridViewTextBoxColumn";
            // 
            // lithostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn
            // 
            lithostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn.DataPropertyName = "_LithostratigraphyPropertyHierarchyCache";
            lithostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn.HeaderText = "_LithostratigraphyPropertyHierarchyCache";
            lithostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn.Name = "lithostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn";
            // 
            // chronostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn
            // 
            chronostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn.DataPropertyName = "_ChronostratigraphyPropertyHierarchyCache";
            chronostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn.HeaderText = "_ChronostratigraphyPropertyHierarchyCache";
            chronostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn.Name = "chronostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn";
            // 
            // averageAltitudeCacheDataGridViewTextBoxColumn
            // 
            averageAltitudeCacheDataGridViewTextBoxColumn.DataPropertyName = "_AverageAltitudeCache";
            averageAltitudeCacheDataGridViewTextBoxColumn.HeaderText = "_AverageAltitudeCache";
            averageAltitudeCacheDataGridViewTextBoxColumn.Name = "averageAltitudeCacheDataGridViewTextBoxColumn";
            // 
            // tableLayoutPanelGridModeParameter
            // 
            tableLayoutPanelGridModeParameter.ColumnCount = 29;
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelGridModeParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableLayoutPanelGridModeParameter.Controls.Add(labelGridViewReplaceColumn, 9, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeReplace, 20, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(textBoxGridModeReplaceWith, 18, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(labelGridModeReplaceWith, 17, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(textBoxGridModeReplace, 15, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(labelGridViewReplaceColumnName, 10, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeFind, 7, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(radioButtonGridModeReplace, 14, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(radioButtonGridModeInsert, 12, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeSave, 3, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(radioButtonGridModeRemove, 11, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeInsert, 21, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeRemove, 23, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeAppend, 22, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(radioButtonGridModeAppend, 13, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeCopy, 5, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonSaveAll, 0, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(labelGridCounter, 26, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeUndo, 2, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeUndoSingleLine, 4, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModePrint, 8, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(progressBarSaveAll, 1, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(comboBoxReplace, 16, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(comboBoxReplaceWith, 19, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonMarkWholeColumn, 25, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonGridModeDelete, 6, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonOptRowHeight, 27, 0);
            tableLayoutPanelGridModeParameter.Controls.Add(buttonOptColumnWidth, 28, 0);
            tableLayoutPanelGridModeParameter.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelGridModeParameter.Location = new System.Drawing.Point(5, 0);
            tableLayoutPanelGridModeParameter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelGridModeParameter.Name = "tableLayoutPanelGridModeParameter";
            tableLayoutPanelGridModeParameter.RowCount = 1;
            tableLayoutPanelGridModeParameter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelGridModeParameter.Size = new System.Drawing.Size(1178, 32);
            tableLayoutPanelGridModeParameter.TabIndex = 2;
            // 
            // labelGridViewReplaceColumn
            // 
            labelGridViewReplaceColumn.AutoSize = true;
            labelGridViewReplaceColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            labelGridViewReplaceColumn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelGridViewReplaceColumn.Location = new System.Drawing.Point(361, 0);
            labelGridViewReplaceColumn.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelGridViewReplaceColumn.Name = "labelGridViewReplaceColumn";
            labelGridViewReplaceColumn.Size = new System.Drawing.Size(63, 32);
            labelGridViewReplaceColumn.TabIndex = 3;
            labelGridViewReplaceColumn.Text = "In Column";
            labelGridViewReplaceColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonGridModeReplace
            // 
            buttonGridModeReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModeReplace.Enabled = false;
            buttonGridModeReplace.Image = Resource.Replace;
            buttonGridModeReplace.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeReplace.Location = new System.Drawing.Point(830, 2);
            buttonGridModeReplace.Margin = new System.Windows.Forms.Padding(1, 2, 0, 2);
            buttonGridModeReplace.Name = "buttonGridModeReplace";
            buttonGridModeReplace.Size = new System.Drawing.Size(28, 28);
            buttonGridModeReplace.TabIndex = 7;
            toolTip.SetToolTip(buttonGridModeReplace, "Replace the text in the content in the selected fields");
            buttonGridModeReplace.UseVisualStyleBackColor = true;
            buttonGridModeReplace.Click += buttonGridModeReplace_Click;
            // 
            // textBoxGridModeReplaceWith
            // 
            textBoxGridModeReplaceWith.AcceptsReturn = true;
            textBoxGridModeReplaceWith.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxGridModeReplaceWith.Location = new System.Drawing.Point(783, 3);
            textBoxGridModeReplaceWith.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            textBoxGridModeReplaceWith.Name = "textBoxGridModeReplaceWith";
            textBoxGridModeReplaceWith.Size = new System.Drawing.Size(23, 23);
            textBoxGridModeReplaceWith.TabIndex = 6;
            // 
            // labelGridModeReplaceWith
            // 
            labelGridModeReplaceWith.AccessibleName = "With";
            labelGridModeReplaceWith.AutoSize = true;
            labelGridModeReplaceWith.Dock = System.Windows.Forms.DockStyle.Fill;
            labelGridModeReplaceWith.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelGridModeReplaceWith.Location = new System.Drawing.Point(751, 0);
            labelGridModeReplaceWith.Margin = new System.Windows.Forms.Padding(0);
            labelGridModeReplaceWith.Name = "labelGridModeReplaceWith";
            labelGridModeReplaceWith.Size = new System.Drawing.Size(32, 32);
            labelGridModeReplaceWith.TabIndex = 4;
            labelGridModeReplaceWith.Text = "With";
            labelGridModeReplaceWith.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxGridModeReplace
            // 
            textBoxGridModeReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxGridModeReplace.Location = new System.Drawing.Point(705, 3);
            textBoxGridModeReplace.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            textBoxGridModeReplace.Name = "textBoxGridModeReplace";
            textBoxGridModeReplace.Size = new System.Drawing.Size(23, 23);
            textBoxGridModeReplace.TabIndex = 5;
            // 
            // labelGridViewReplaceColumnName
            // 
            labelGridViewReplaceColumnName.AutoSize = true;
            labelGridViewReplaceColumnName.Dock = System.Windows.Forms.DockStyle.Fill;
            labelGridViewReplaceColumnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline);
            labelGridViewReplaceColumnName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelGridViewReplaceColumnName.Location = new System.Drawing.Point(424, 0);
            labelGridViewReplaceColumnName.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            labelGridViewReplaceColumnName.Name = "labelGridViewReplaceColumnName";
            labelGridViewReplaceColumnName.Size = new System.Drawing.Size(14, 32);
            labelGridViewReplaceColumnName.TabIndex = 10;
            labelGridViewReplaceColumnName.Text = "?";
            labelGridViewReplaceColumnName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonGridModeFind
            // 
            buttonGridModeFind.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModeFind.Image = Resource.Search;
            buttonGridModeFind.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeFind.Location = new System.Drawing.Point(297, 3);
            buttonGridModeFind.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonGridModeFind.Name = "buttonGridModeFind";
            buttonGridModeFind.Size = new System.Drawing.Size(27, 26);
            buttonGridModeFind.TabIndex = 9;
            toolTip.SetToolTip(buttonGridModeFind, "Close form an change to current dataset in the main form");
            buttonGridModeFind.UseVisualStyleBackColor = true;
            buttonGridModeFind.Click += buttonGridModeFind_Click;
            // 
            // radioButtonGridModeReplace
            // 
            radioButtonGridModeReplace.AccessibleName = "Replace";
            radioButtonGridModeReplace.AutoSize = true;
            radioButtonGridModeReplace.Checked = true;
            radioButtonGridModeReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonGridModeReplace.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            radioButtonGridModeReplace.Location = new System.Drawing.Point(639, 3);
            radioButtonGridModeReplace.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            radioButtonGridModeReplace.Name = "radioButtonGridModeReplace";
            radioButtonGridModeReplace.Size = new System.Drawing.Size(66, 26);
            radioButtonGridModeReplace.TabIndex = 13;
            radioButtonGridModeReplace.TabStop = true;
            radioButtonGridModeReplace.Text = "Replace";
            radioButtonGridModeReplace.UseVisualStyleBackColor = true;
            radioButtonGridModeReplace.CheckedChanged += radioButtonGridModeReplace_CheckedChanged;
            // 
            // radioButtonGridModeInsert
            // 
            radioButtonGridModeInsert.AccessibleName = "Insert";
            radioButtonGridModeInsert.AutoSize = true;
            radioButtonGridModeInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonGridModeInsert.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            radioButtonGridModeInsert.Location = new System.Drawing.Point(518, 3);
            radioButtonGridModeInsert.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            radioButtonGridModeInsert.Name = "radioButtonGridModeInsert";
            radioButtonGridModeInsert.Size = new System.Drawing.Size(54, 26);
            radioButtonGridModeInsert.TabIndex = 14;
            radioButtonGridModeInsert.Text = "Insert";
            radioButtonGridModeInsert.UseVisualStyleBackColor = true;
            radioButtonGridModeInsert.CheckedChanged += radioButtonGridModeInsert_CheckedChanged;
            // 
            // buttonGridModeSave
            // 
            buttonGridModeSave.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModeSave.Image = Resource.Save;
            buttonGridModeSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeSave.Location = new System.Drawing.Point(179, 2);
            buttonGridModeSave.Margin = new System.Windows.Forms.Padding(4, 2, 0, 2);
            buttonGridModeSave.Name = "buttonGridModeSave";
            buttonGridModeSave.Size = new System.Drawing.Size(27, 28);
            buttonGridModeSave.TabIndex = 15;
            toolTip.SetToolTip(buttonGridModeSave, "Save changes in current item");
            buttonGridModeSave.UseVisualStyleBackColor = true;
            buttonGridModeSave.Click += buttonGridModeSave_Click;
            // 
            // radioButtonGridModeRemove
            // 
            radioButtonGridModeRemove.AccessibleName = "Remove";
            radioButtonGridModeRemove.AutoSize = true;
            radioButtonGridModeRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonGridModeRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            radioButtonGridModeRemove.Location = new System.Drawing.Point(446, 3);
            radioButtonGridModeRemove.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            radioButtonGridModeRemove.Name = "radioButtonGridModeRemove";
            radioButtonGridModeRemove.Size = new System.Drawing.Size(68, 26);
            radioButtonGridModeRemove.TabIndex = 16;
            radioButtonGridModeRemove.Text = "Remove";
            radioButtonGridModeRemove.UseVisualStyleBackColor = true;
            radioButtonGridModeRemove.CheckedChanged += radioButtonGridModeRemove_CheckedChanged;
            // 
            // buttonGridModeInsert
            // 
            buttonGridModeInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModeInsert.Enabled = false;
            buttonGridModeInsert.Image = Resource.Insert1;
            buttonGridModeInsert.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeInsert.Location = new System.Drawing.Point(859, 2);
            buttonGridModeInsert.Margin = new System.Windows.Forms.Padding(1, 2, 0, 2);
            buttonGridModeInsert.Name = "buttonGridModeInsert";
            buttonGridModeInsert.Size = new System.Drawing.Size(28, 28);
            buttonGridModeInsert.TabIndex = 17;
            toolTip.SetToolTip(buttonGridModeInsert, "Insert the text at the beginning of the content in the selected fields");
            buttonGridModeInsert.UseCompatibleTextRendering = true;
            buttonGridModeInsert.UseVisualStyleBackColor = true;
            buttonGridModeInsert.Visible = false;
            buttonGridModeInsert.Click += buttonGridModeInsert_Click;
            // 
            // buttonGridModeRemove
            // 
            buttonGridModeRemove.Dock = System.Windows.Forms.DockStyle.Left;
            buttonGridModeRemove.Enabled = false;
            buttonGridModeRemove.Image = Resource.Radierer;
            buttonGridModeRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeRemove.Location = new System.Drawing.Point(917, 2);
            buttonGridModeRemove.Margin = new System.Windows.Forms.Padding(1, 2, 0, 2);
            buttonGridModeRemove.Name = "buttonGridModeRemove";
            buttonGridModeRemove.Size = new System.Drawing.Size(28, 28);
            buttonGridModeRemove.TabIndex = 18;
            toolTip.SetToolTip(buttonGridModeRemove, "Remove the content in the selected fields");
            buttonGridModeRemove.UseVisualStyleBackColor = true;
            buttonGridModeRemove.Visible = false;
            buttonGridModeRemove.Click += buttonGridModeRemove_Click;
            // 
            // buttonGridModeAppend
            // 
            buttonGridModeAppend.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModeAppend.Enabled = false;
            buttonGridModeAppend.Image = Resource.Append;
            buttonGridModeAppend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeAppend.Location = new System.Drawing.Point(888, 2);
            buttonGridModeAppend.Margin = new System.Windows.Forms.Padding(1, 2, 0, 2);
            buttonGridModeAppend.Name = "buttonGridModeAppend";
            buttonGridModeAppend.Size = new System.Drawing.Size(28, 28);
            buttonGridModeAppend.TabIndex = 19;
            toolTip.SetToolTip(buttonGridModeAppend, "Append the text to the content in the selected fields");
            buttonGridModeAppend.UseVisualStyleBackColor = true;
            buttonGridModeAppend.Visible = false;
            buttonGridModeAppend.Click += buttonGridModeAppend_Click;
            // 
            // radioButtonGridModeAppend
            // 
            radioButtonGridModeAppend.AccessibleName = "Append";
            radioButtonGridModeAppend.AutoSize = true;
            radioButtonGridModeAppend.Dock = System.Windows.Forms.DockStyle.Fill;
            radioButtonGridModeAppend.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            radioButtonGridModeAppend.Location = new System.Drawing.Point(572, 3);
            radioButtonGridModeAppend.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            radioButtonGridModeAppend.Name = "radioButtonGridModeAppend";
            radioButtonGridModeAppend.Size = new System.Drawing.Size(67, 26);
            radioButtonGridModeAppend.TabIndex = 20;
            radioButtonGridModeAppend.TabStop = true;
            radioButtonGridModeAppend.Text = "Append";
            radioButtonGridModeAppend.UseVisualStyleBackColor = true;
            radioButtonGridModeAppend.CheckedChanged += radioButtonGridModeAppend_CheckedChanged;
            // 
            // buttonGridModeCopy
            // 
            buttonGridModeCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModeCopy.Enabled = false;
            buttonGridModeCopy.Image = Resource.Copy1;
            buttonGridModeCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeCopy.Location = new System.Drawing.Point(238, 3);
            buttonGridModeCopy.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonGridModeCopy.Name = "buttonGridModeCopy";
            buttonGridModeCopy.Size = new System.Drawing.Size(27, 26);
            buttonGridModeCopy.TabIndex = 21;
            toolTip.SetToolTip(buttonGridModeCopy, "Copy current collection event");
            buttonGridModeCopy.UseVisualStyleBackColor = true;
            buttonGridModeCopy.Click += buttonGridModeCopy_Click;
            // 
            // buttonSaveAll
            // 
            buttonSaveAll.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonSaveAll.Image = Resource.SaveAll;
            buttonSaveAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonSaveAll.Location = new System.Drawing.Point(0, 2);
            buttonSaveAll.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            buttonSaveAll.Name = "buttonSaveAll";
            buttonSaveAll.Size = new System.Drawing.Size(28, 28);
            buttonSaveAll.TabIndex = 22;
            toolTip.SetToolTip(buttonSaveAll, "Save changes for all datasets");
            buttonSaveAll.UseVisualStyleBackColor = true;
            buttonSaveAll.Click += buttonSaveAll_Click;
            // 
            // labelGridCounter
            // 
            labelGridCounter.AutoSize = true;
            labelGridCounter.Dock = System.Windows.Forms.DockStyle.Fill;
            labelGridCounter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelGridCounter.Location = new System.Drawing.Point(1109, 0);
            labelGridCounter.Margin = new System.Windows.Forms.Padding(0);
            labelGridCounter.Name = "labelGridCounter";
            labelGridCounter.Size = new System.Drawing.Size(13, 32);
            labelGridCounter.TabIndex = 23;
            labelGridCounter.Text = "0";
            labelGridCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonGridModeUndo
            // 
            buttonGridModeUndo.Dock = System.Windows.Forms.DockStyle.Left;
            buttonGridModeUndo.Image = Resource.UndoAll;
            buttonGridModeUndo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeUndo.Location = new System.Drawing.Point(145, 2);
            buttonGridModeUndo.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
            buttonGridModeUndo.Name = "buttonGridModeUndo";
            buttonGridModeUndo.Size = new System.Drawing.Size(28, 28);
            buttonGridModeUndo.TabIndex = 24;
            toolTip.SetToolTip(buttonGridModeUndo, "Reset changes in all datasets");
            buttonGridModeUndo.UseVisualStyleBackColor = true;
            buttonGridModeUndo.Click += buttonGridModeUndo_Click;
            // 
            // buttonGridModeUndoSingleLine
            // 
            buttonGridModeUndoSingleLine.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModeUndoSingleLine.Image = Properties.Resources.Undo;
            buttonGridModeUndoSingleLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeUndoSingleLine.Location = new System.Drawing.Point(206, 2);
            buttonGridModeUndoSingleLine.Margin = new System.Windows.Forms.Padding(0, 2, 4, 2);
            buttonGridModeUndoSingleLine.Name = "buttonGridModeUndoSingleLine";
            buttonGridModeUndoSingleLine.Size = new System.Drawing.Size(28, 28);
            buttonGridModeUndoSingleLine.TabIndex = 25;
            toolTip.SetToolTip(buttonGridModeUndoSingleLine, "Reset changes in current item");
            buttonGridModeUndoSingleLine.UseVisualStyleBackColor = true;
            buttonGridModeUndoSingleLine.Click += buttonGridModeUndoSingleLine_Click;
            // 
            // buttonGridModePrint
            // 
            buttonGridModePrint.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModePrint.Image = Resource.Export;
            buttonGridModePrint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModePrint.Location = new System.Drawing.Point(328, 2);
            buttonGridModePrint.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            buttonGridModePrint.Name = "buttonGridModePrint";
            buttonGridModePrint.Size = new System.Drawing.Size(29, 28);
            buttonGridModePrint.TabIndex = 26;
            toolTip.SetToolTip(buttonGridModePrint, "Export the datagrid");
            buttonGridModePrint.UseVisualStyleBackColor = true;
            buttonGridModePrint.Click += buttonGridModePrint_Click;
            // 
            // progressBarSaveAll
            // 
            progressBarSaveAll.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBarSaveAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            progressBarSaveAll.Location = new System.Drawing.Point(28, 3);
            progressBarSaveAll.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            progressBarSaveAll.Name = "progressBarSaveAll";
            progressBarSaveAll.Size = new System.Drawing.Size(117, 26);
            progressBarSaveAll.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            progressBarSaveAll.TabIndex = 27;
            progressBarSaveAll.Visible = false;
            // 
            // comboBoxReplace
            // 
            comboBoxReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxReplace.FormattingEnabled = true;
            comboBoxReplace.Location = new System.Drawing.Point(728, 3);
            comboBoxReplace.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            comboBoxReplace.Name = "comboBoxReplace";
            comboBoxReplace.Size = new System.Drawing.Size(23, 23);
            comboBoxReplace.TabIndex = 28;
            comboBoxReplace.Visible = false;
            // 
            // comboBoxReplaceWith
            // 
            comboBoxReplaceWith.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxReplaceWith.FormattingEnabled = true;
            comboBoxReplaceWith.Location = new System.Drawing.Point(806, 3);
            comboBoxReplaceWith.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            comboBoxReplaceWith.Name = "comboBoxReplaceWith";
            comboBoxReplaceWith.Size = new System.Drawing.Size(23, 23);
            comboBoxReplaceWith.TabIndex = 29;
            comboBoxReplaceWith.Visible = false;
            // 
            // buttonMarkWholeColumn
            // 
            buttonMarkWholeColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonMarkWholeColumn.Image = Resource.MarkColumn;
            buttonMarkWholeColumn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonMarkWholeColumn.Location = new System.Drawing.Point(1080, 0);
            buttonMarkWholeColumn.Margin = new System.Windows.Forms.Padding(0);
            buttonMarkWholeColumn.Name = "buttonMarkWholeColumn";
            buttonMarkWholeColumn.Size = new System.Drawing.Size(29, 32);
            buttonMarkWholeColumn.TabIndex = 30;
            toolTip.SetToolTip(buttonMarkWholeColumn, "Select whole datacolumn");
            buttonMarkWholeColumn.UseVisualStyleBackColor = true;
            buttonMarkWholeColumn.Click += buttonMarkWholeColumn_Click;
            // 
            // buttonGridModeDelete
            // 
            buttonGridModeDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonGridModeDelete.Enabled = false;
            buttonGridModeDelete.Image = Resource.Delete;
            buttonGridModeDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonGridModeDelete.Location = new System.Drawing.Point(266, 3);
            buttonGridModeDelete.Margin = new System.Windows.Forms.Padding(1, 3, 0, 3);
            buttonGridModeDelete.Name = "buttonGridModeDelete";
            buttonGridModeDelete.Size = new System.Drawing.Size(27, 26);
            buttonGridModeDelete.TabIndex = 31;
            toolTip.SetToolTip(buttonGridModeDelete, "Delete current collection event");
            buttonGridModeDelete.UseVisualStyleBackColor = true;
            buttonGridModeDelete.Visible = false;
            buttonGridModeDelete.Click += buttonGridModeDelete_Click;
            // 
            // buttonOptRowHeight
            // 
            buttonOptRowHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonOptRowHeight.Image = Resource.OptRowHeight;
            buttonOptRowHeight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonOptRowHeight.Location = new System.Drawing.Point(1122, 0);
            buttonOptRowHeight.Margin = new System.Windows.Forms.Padding(0);
            buttonOptRowHeight.Name = "buttonOptRowHeight";
            buttonOptRowHeight.Size = new System.Drawing.Size(28, 32);
            buttonOptRowHeight.TabIndex = 32;
            toolTip.SetToolTip(buttonOptRowHeight, "Optimize column height");
            buttonOptRowHeight.UseVisualStyleBackColor = true;
            buttonOptRowHeight.Click += buttonOptRowHeight_Click;
            // 
            // buttonOptColumnWidth
            // 
            buttonOptColumnWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonOptColumnWidth.Image = Resource.OptColumnWidth;
            buttonOptColumnWidth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonOptColumnWidth.Location = new System.Drawing.Point(1150, 0);
            buttonOptColumnWidth.Margin = new System.Windows.Forms.Padding(0);
            buttonOptColumnWidth.Name = "buttonOptColumnWidth";
            buttonOptColumnWidth.Size = new System.Drawing.Size(28, 32);
            buttonOptColumnWidth.TabIndex = 33;
            toolTip.SetToolTip(buttonOptColumnWidth, "Optimize column width");
            buttonOptColumnWidth.UseVisualStyleBackColor = true;
            buttonOptColumnWidth.Click += buttonOptColumnWidth_Click;
            // 
            // contextMenuStripDataGrid
            // 
            contextMenuStripDataGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItemCopyFromClipboard });
            contextMenuStripDataGrid.Name = "contextMenuStripDataGrid";
            contextMenuStripDataGrid.Size = new System.Drawing.Size(186, 26);
            // 
            // toolStripMenuItemCopyFromClipboard
            // 
            toolStripMenuItemCopyFromClipboard.Image = Resource.Clipboard;
            toolStripMenuItemCopyFromClipboard.Name = "toolStripMenuItemCopyFromClipboard";
            toolStripMenuItemCopyFromClipboard.Size = new System.Drawing.Size(185, 22);
            toolStripMenuItemCopyFromClipboard.Text = "Insert from clipboard";
            // 
            // imageListEventImages
            // 
            imageListEventImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListEventImages.ImageSize = new System.Drawing.Size(50, 50);
            imageListEventImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageListForm
            // 
            imageListForm.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListForm.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListForm.ImageStream");
            imageListForm.TransparentColor = System.Drawing.Color.Transparent;
            imageListForm.Images.SetKeyName(0, "");
            // 
            // firstLinesEventTableAdapter
            // 
            firstLinesEventTableAdapter.ClearBeforeFill = true;
            // 
            // collectionEventImageTableAdapter
            // 
            collectionEventImageTableAdapter.ClearBeforeFill = true;
            // 
            // collectionEventTableAdapter
            // 
            collectionEventTableAdapter.ClearBeforeFill = true;
            // 
            // collectionEventSeriesTableAdapter
            // 
            collectionEventSeriesTableAdapter.ClearBeforeFill = true;
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(0, 707);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(1188, 31);
            userControlDialogPanel.TabIndex = 5;
            // 
            // FormCollectionEventGridMode
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1188, 738);
            Controls.Add(splitContainerMain);
            Controls.Add(userControlDialogPanel);
            Controls.Add(tableLayoutPanelHeader);
            helpProvider.SetHelpKeyword(this, "gridevent_dc");
            helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCollectionEventGridMode";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Collection event - spreadsheet mode";
            FormClosing += FormCollectionEventGridMode_FormClosing;
            Load += FormCollectionEventGridMode_Load;
            KeyDown += Form_KeyDown;
            tableLayoutPanelHeader.ResumeLayout(false);
            tableLayoutPanelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)firstLinesEventBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionEventGridMode).EndInit();
            tableLayoutPanelHeaderImageVisibility.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)collectionEventBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionSpecimen).EndInit();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerTreeView.Panel1.ResumeLayout(false);
            splitContainerTreeView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerTreeView).EndInit();
            splitContainerTreeView.ResumeLayout(false);
            splitContainerTrees.Panel1.ResumeLayout(false);
            splitContainerTrees.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerTrees).EndInit();
            splitContainerTrees.ResumeLayout(false);
            groupBoxGridModeOptions.ResumeLayout(false);
            tableLayoutPanelGridModeOptions.ResumeLayout(false);
            splitContainerEventSeries.Panel1.ResumeLayout(false);
            splitContainerEventSeries.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerEventSeries).EndInit();
            splitContainerEventSeries.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewEventSeries).EndInit();
            ((System.ComponentModel.ISupportInitialize)collectionEventSeriesBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionEventSeries).EndInit();
            groupBoxImage.ResumeLayout(false);
            splitContainerImage.Panel1.ResumeLayout(false);
            splitContainerImage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerImage).EndInit();
            splitContainerImage.ResumeLayout(false);
            panelSpecimenImageList.ResumeLayout(false);
            panelSpecimenImageList.PerformLayout();
            toolStripSpecimenImage.ResumeLayout(false);
            toolStripSpecimenImage.PerformLayout();
            tableLayoutPanelSpecimenImage.ResumeLayout(false);
            tableLayoutPanelSpecimenImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)collectionEventImageBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            tableLayoutPanelGridModeParameter.ResumeLayout(false);
            tableLayoutPanelGridModeParameter.PerformLayout();
            contextMenuStripDataGrid.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.TextBox textBoxHeaderNumber;
        private System.Windows.Forms.TextBox textBoxHeaderTitle;
        private System.Windows.Forms.Label labelHeaderNumber;
        private System.Windows.Forms.Label labelHeaderID;
        private System.Windows.Forms.Label labelHeaderVersion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeaderImageVisibility;
        private System.Windows.Forms.Button buttonHeaderShowImage;
        private System.Windows.Forms.Button buttonHeaderShowSelectionTree;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.TextBox textBoxHeaderEventID;
        private System.Windows.Forms.TextBox textBoxHeaderVersionEvent;
        private System.Windows.Forms.Label labelHeaderEventID;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerTreeView;
        private System.Windows.Forms.GroupBox groupBoxGridModeOptions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGridModeOptions;
        private System.Windows.Forms.Button buttonGridRequery;
        private System.Windows.Forms.TreeView treeViewGridModeFieldSelector;
        private System.Windows.Forms.Button buttonGridModeUpdateColumnSettings;
        private System.Windows.Forms.Button buttonResetSequence;
        private System.Windows.Forms.GroupBox groupBoxImage;
        private System.Windows.Forms.SplitContainer splitContainerImage;
        private DiversityWorkbench.UserControls.UserControlImage userControlImage;
        private System.Windows.Forms.Panel panelSpecimenImageList;
        private System.Windows.Forms.ListBox listBoxImage;
        private System.Windows.Forms.ToolStrip toolStripSpecimenImage;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSpecimenImage;
        private System.Windows.Forms.Label labelSpecimenImageType;
        private System.Windows.Forms.ComboBox comboBoxImageType;
        private System.Windows.Forms.Label labelSpecimenImageNotes;
        private System.Windows.Forms.TextBox textBoxImageNotes;
        private System.Windows.Forms.Label labelSpecimenImageWithholdingReason;
        private System.Windows.Forms.ComboBox comboBoxImageWithholdingReason;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGridModeParameter;
        private System.Windows.Forms.Label labelGridViewReplaceColumn;
        private System.Windows.Forms.Button buttonGridModeReplace;
        private System.Windows.Forms.TextBox textBoxGridModeReplaceWith;
        private System.Windows.Forms.Label labelGridModeReplaceWith;
        private System.Windows.Forms.TextBox textBoxGridModeReplace;
        private System.Windows.Forms.Label labelGridViewReplaceColumnName;
        private System.Windows.Forms.Button buttonGridModeFind;
        private System.Windows.Forms.RadioButton radioButtonGridModeReplace;
        private System.Windows.Forms.RadioButton radioButtonGridModeInsert;
        private System.Windows.Forms.Button buttonGridModeSave;
        private System.Windows.Forms.RadioButton radioButtonGridModeRemove;
        private System.Windows.Forms.Button buttonGridModeInsert;
        private System.Windows.Forms.Button buttonGridModeRemove;
        private System.Windows.Forms.Button buttonGridModeAppend;
        private System.Windows.Forms.RadioButton radioButtonGridModeAppend;
        private System.Windows.Forms.Button buttonGridModeCopy;
        private System.Windows.Forms.Button buttonSaveAll;
        private System.Windows.Forms.Label labelGridCounter;
        private System.Windows.Forms.Button buttonGridModeUndo;
        private System.Windows.Forms.Button buttonGridModeUndoSingleLine;
        private System.Windows.Forms.Button buttonGridModePrint;
        private System.Windows.Forms.ProgressBar progressBarSaveAll;
        private System.Windows.Forms.ComboBox comboBoxReplace;
        private System.Windows.Forms.ComboBox comboBoxReplaceWith;
        private System.Windows.Forms.Button buttonMarkWholeColumn;
        private System.Windows.Forms.Button buttonGridModeDelete;
        private System.Windows.Forms.Button buttonOptRowHeight;
        private System.Windows.Forms.Button buttonOptColumnWidth;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private DiversityCollection.Datasets.DataSetCollectionSpecimen dataSetCollectionSpecimen;
        private DiversityCollection.Datasets.DataSetCollectionEventGridMode dataSetCollectionEventGridMode;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDataGrid;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopyFromClipboard;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ImageList imageListEventImages;
        private System.Windows.Forms.ImageList imageListForm;
        private System.Windows.Forms.BindingSource firstLinesEventBindingSource;
        private DiversityCollection.Datasets.DataSetCollectionEventGridModeTableAdapters.FirstLinesEvent_2TableAdapter firstLinesEventTableAdapter;
        private System.Windows.Forms.BindingSource collectionEventImageBindingSource;
        private DiversityCollection.Datasets.DataSetCollectionSpecimenTableAdapters.CollectionEventImageTableAdapter collectionEventImageTableAdapter;
        private System.Windows.Forms.BindingSource collectionEventBindingSource;
        private DiversityCollection.Datasets.DataSetCollectionSpecimenTableAdapters.CollectionEventTableAdapter collectionEventTableAdapter;
        private System.Windows.Forms.Button buttonHeaderShowTree;
        private System.Windows.Forms.SplitContainer splitContainerTrees;
        private DiversityCollection.Datasets.DataSetCollectionEventSeries dataSetCollectionEventSeries;
        private DiversityCollection.UserControls.UserControlEventSeriesTree userControlEventSeriesTree;
        private System.Windows.Forms.SplitContainer splitContainerEventSeries;
        private System.Windows.Forms.DataGridView dataGridViewEventSeries;
        private System.Windows.Forms.BindingSource collectionEventSeriesBindingSource;
        private DiversityCollection.Datasets.DataSetCollectionEventSeriesTableAdapters.CollectionEventSeriesTableAdapter collectionEventSeriesTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn seriesIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seriesParentIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seriesCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dateStartDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dateEndDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectionEventIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn datawithholdingreasonforcollectioneventDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectorseventnumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectiondayDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectionmonthDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectionyearDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectiondatesupplementDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectiontimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectiontimespanDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn countryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn localitydescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn habitatdescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectingmethodDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectioneventnotesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn namedareaDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn namedAreaLocation2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn removelinktogazetteerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn distancetolocationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn directiontolocationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn longitudeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn latitudeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn coordinatesaccuracyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn linktoGoogleMapsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altitudefromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altitudetoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altitudeaccuracyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mTBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quadrantDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesforMTBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MTB_accuracy;
        private System.Windows.Forms.DataGridViewTextBoxColumn samplingplotDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn linktoSamplingPlotsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn removelinktoSamplingPlotsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accuracyofsamplingplotDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn latitudeofsamplingplotDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn longitudeofsamplingplotDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn geographicregionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn lithostratigraphyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn chronostratigraphyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn collectionEventIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn coordinatesAverageLatitudeCacheDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn coordinatesAverageLongitudeCacheDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn coordinatesLocationNotesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn geographicRegionPropertyURIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lithostratigraphyPropertyURIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chronostratigraphyPropertyURIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn namedAverageLatitudeCacheDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn namedAverageLongitudeCacheDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lithostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chronostratigraphyPropertyHierarchyCacheDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn averageAltitudeCacheDataGridViewTextBoxColumn;
    }
}