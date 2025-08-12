namespace DiversityCollection.Forms
{
    partial class FormCollectionEventSeriesGridMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCollectionEventSeriesGridMode));
            tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            textBoxHeaderSeriesID = new System.Windows.Forms.TextBox();
            labelHeaderID = new System.Windows.Forms.Label();
            buttonHeaderShowTree = new System.Windows.Forms.Button();
            buttonHeaderShowImage = new System.Windows.Forms.Button();
            buttonFeedback = new System.Windows.Forms.Button();
            labelDescription = new System.Windows.Forms.Label();
            labelCode = new System.Windows.Forms.Label();
            textBoxHeaderCode = new System.Windows.Forms.TextBox();
            textBoxHeaderDescription = new System.Windows.Forms.TextBox();
            buttonHeaderShowGeography = new System.Windows.Forms.Button();
            buttonHistory = new System.Windows.Forms.Button();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            splitContainerTreeAndImage = new System.Windows.Forms.SplitContainer();
            userControlEventSeriesTree = new DiversityCollection.UserControls.UserControlEventSeriesTree();
            splitContainerImageAndGeography = new System.Windows.Forms.SplitContainer();
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
            collectionEventSeriesImageBindingSource = new System.Windows.Forms.BindingSource(components);
            dataSetCollectionEventSeries = new DiversityCollection.Datasets.DataSetCollectionEventSeries();
            labelSpecimenImageNotes = new System.Windows.Forms.Label();
            textBoxImageNotes = new System.Windows.Forms.TextBox();
            labelSpecimenImageWithholdingReason = new System.Windows.Forms.Label();
            comboBoxImageWithholdingReason = new System.Windows.Forms.ComboBox();
            elementHost = new System.Windows.Forms.Integration.ElementHost();
            toolStripGIS = new System.Windows.Forms.ToolStrip();
            toolStripButtonGeographySave = new System.Windows.Forms.ToolStripButton();
            dataGridView = new System.Windows.Forms.DataGridView();
            seriesIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            seriesParentIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            seriesCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            notesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dateStartDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            dateEndDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            collectionEventSeriesBindingSource = new System.Windows.Forms.BindingSource(components);
            toolTip = new System.Windows.Forms.ToolTip(components);
            imageListDataset = new System.Windows.Forms.ImageList(components);
            imageListForm = new System.Windows.Forms.ImageList(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            collectionEventSeriesTableAdapter = new DiversityCollection.Datasets.DataSetCollectionEventSeriesTableAdapters.CollectionEventSeriesTableAdapter();
            collectionEventSeriesImageTableAdapter = new DiversityCollection.Datasets.DataSetCollectionEventSeriesTableAdapters.CollectionEventSeriesImageTableAdapter();
            tableLayoutPanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerTreeAndImage).BeginInit();
            splitContainerTreeAndImage.Panel1.SuspendLayout();
            splitContainerTreeAndImage.Panel2.SuspendLayout();
            splitContainerTreeAndImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerImageAndGeography).BeginInit();
            splitContainerImageAndGeography.Panel1.SuspendLayout();
            splitContainerImageAndGeography.Panel2.SuspendLayout();
            splitContainerImageAndGeography.SuspendLayout();
            groupBoxImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerImage).BeginInit();
            splitContainerImage.Panel1.SuspendLayout();
            splitContainerImage.Panel2.SuspendLayout();
            splitContainerImage.SuspendLayout();
            panelSpecimenImageList.SuspendLayout();
            toolStripSpecimenImage.SuspendLayout();
            tableLayoutPanelSpecimenImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)collectionEventSeriesImageBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionEventSeries).BeginInit();
            toolStripGIS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)collectionEventSeriesBindingSource).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanelHeader
            // 
            tableLayoutPanelHeader.ColumnCount = 11;
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderSeriesID, 5, 0);
            tableLayoutPanelHeader.Controls.Add(labelHeaderID, 4, 0);
            tableLayoutPanelHeader.Controls.Add(buttonHeaderShowTree, 6, 0);
            tableLayoutPanelHeader.Controls.Add(buttonHeaderShowImage, 7, 0);
            tableLayoutPanelHeader.Controls.Add(buttonFeedback, 10, 0);
            tableLayoutPanelHeader.Controls.Add(labelDescription, 2, 0);
            tableLayoutPanelHeader.Controls.Add(labelCode, 0, 0);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderCode, 1, 0);
            tableLayoutPanelHeader.Controls.Add(textBoxHeaderDescription, 3, 0);
            tableLayoutPanelHeader.Controls.Add(buttonHeaderShowGeography, 8, 0);
            tableLayoutPanelHeader.Controls.Add(buttonHistory, 9, 0);
            tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            tableLayoutPanelHeader.RowCount = 1;
            tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelHeader.Size = new System.Drawing.Size(1100, 28);
            tableLayoutPanelHeader.TabIndex = 2;
            // 
            // textBoxHeaderSeriesID
            // 
            textBoxHeaderSeriesID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderSeriesID.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxHeaderSeriesID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            textBoxHeaderSeriesID.Location = new System.Drawing.Point(875, 7);
            textBoxHeaderSeriesID.Margin = new System.Windows.Forms.Padding(4, 7, 4, 3);
            textBoxHeaderSeriesID.Name = "textBoxHeaderSeriesID";
            textBoxHeaderSeriesID.ReadOnly = true;
            textBoxHeaderSeriesID.Size = new System.Drawing.Size(70, 13);
            textBoxHeaderSeriesID.TabIndex = 21;
            textBoxHeaderSeriesID.Text = "0000";
            // 
            // labelHeaderID
            // 
            labelHeaderID.AutoSize = true;
            labelHeaderID.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeaderID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            labelHeaderID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelHeaderID.Location = new System.Drawing.Point(850, 3);
            labelHeaderID.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            labelHeaderID.Name = "labelHeaderID";
            labelHeaderID.Size = new System.Drawing.Size(21, 22);
            labelHeaderID.TabIndex = 20;
            labelHeaderID.Text = "ID:";
            labelHeaderID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonHeaderShowTree
            // 
            buttonHeaderShowTree.BackColor = System.Drawing.Color.Red;
            buttonHeaderShowTree.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHeaderShowTree.FlatAppearance.BorderSize = 0;
            buttonHeaderShowTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonHeaderShowTree.Image = Resource.EventSeriesHierarchy;
            buttonHeaderShowTree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonHeaderShowTree.Location = new System.Drawing.Point(950, 1);
            buttonHeaderShowTree.Margin = new System.Windows.Forms.Padding(1);
            buttonHeaderShowTree.Name = "buttonHeaderShowTree";
            buttonHeaderShowTree.Size = new System.Drawing.Size(21, 26);
            buttonHeaderShowTree.TabIndex = 19;
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
            buttonHeaderShowImage.Location = new System.Drawing.Point(973, 1);
            buttonHeaderShowImage.Margin = new System.Windows.Forms.Padding(1);
            buttonHeaderShowImage.Name = "buttonHeaderShowImage";
            buttonHeaderShowImage.Size = new System.Drawing.Size(21, 26);
            buttonHeaderShowImage.TabIndex = 18;
            buttonHeaderShowImage.UseVisualStyleBackColor = false;
            buttonHeaderShowImage.Click += buttonHeaderShowImage_Click;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.Image = Resource.Feedback;
            buttonFeedback.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            buttonFeedback.Location = new System.Drawing.Point(1053, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(47, 28);
            buttonFeedback.TabIndex = 17;
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // labelDescription
            // 
            labelDescription.AutoSize = true;
            labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDescription.Location = new System.Drawing.Point(276, 0);
            labelDescription.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new System.Drawing.Size(38, 28);
            labelDescription.TabIndex = 22;
            labelDescription.Text = "Desc.:";
            labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCode
            // 
            labelCode.AutoSize = true;
            labelCode.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCode.Location = new System.Drawing.Point(4, 0);
            labelCode.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelCode.Name = "labelCode";
            labelCode.Size = new System.Drawing.Size(38, 28);
            labelCode.TabIndex = 23;
            labelCode.Text = "Code:";
            labelCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxHeaderCode
            // 
            textBoxHeaderCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderCode.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxHeaderCode.Location = new System.Drawing.Point(46, 7);
            textBoxHeaderCode.Margin = new System.Windows.Forms.Padding(4, 7, 4, 3);
            textBoxHeaderCode.Name = "textBoxHeaderCode";
            textBoxHeaderCode.ReadOnly = true;
            textBoxHeaderCode.Size = new System.Drawing.Size(222, 16);
            textBoxHeaderCode.TabIndex = 24;
            textBoxHeaderCode.Text = "Code";
            // 
            // textBoxHeaderDescription
            // 
            textBoxHeaderDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBoxHeaderDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxHeaderDescription.Location = new System.Drawing.Point(318, 7);
            textBoxHeaderDescription.Margin = new System.Windows.Forms.Padding(4, 7, 4, 3);
            textBoxHeaderDescription.Name = "textBoxHeaderDescription";
            textBoxHeaderDescription.ReadOnly = true;
            textBoxHeaderDescription.Size = new System.Drawing.Size(528, 16);
            textBoxHeaderDescription.TabIndex = 25;
            textBoxHeaderDescription.Text = "Description";
            // 
            // buttonHeaderShowGeography
            // 
            buttonHeaderShowGeography.BackColor = System.Drawing.SystemColors.Control;
            buttonHeaderShowGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHeaderShowGeography.FlatAppearance.BorderSize = 0;
            buttonHeaderShowGeography.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonHeaderShowGeography.Image = Resource.Localisation;
            buttonHeaderShowGeography.Location = new System.Drawing.Point(996, 1);
            buttonHeaderShowGeography.Margin = new System.Windows.Forms.Padding(1);
            buttonHeaderShowGeography.Name = "buttonHeaderShowGeography";
            buttonHeaderShowGeography.Size = new System.Drawing.Size(21, 26);
            buttonHeaderShowGeography.TabIndex = 26;
            buttonHeaderShowGeography.UseVisualStyleBackColor = false;
            buttonHeaderShowGeography.Click += buttonHeaderShowGeography_Click;
            // 
            // buttonHistory
            // 
            buttonHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonHistory.Image = Resource.History;
            buttonHistory.Location = new System.Drawing.Point(1018, 0);
            buttonHistory.Margin = new System.Windows.Forms.Padding(0);
            buttonHistory.Name = "buttonHistory";
            buttonHistory.Size = new System.Drawing.Size(35, 28);
            buttonHistory.TabIndex = 27;
            toolTip.SetToolTip(buttonHistory, "Open the history tables for the event series");
            buttonHistory.UseVisualStyleBackColor = true;
            buttonHistory.Click += buttonHistory_Click;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 28);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerMain.Name = "splitContainerMain";
            splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(splitContainerTreeAndImage);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(dataGridView);
            splitContainerMain.Size = new System.Drawing.Size(1100, 614);
            splitContainerMain.SplitterDistance = 318;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 3;
            // 
            // splitContainerTreeAndImage
            // 
            splitContainerTreeAndImage.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerTreeAndImage.Location = new System.Drawing.Point(0, 0);
            splitContainerTreeAndImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerTreeAndImage.Name = "splitContainerTreeAndImage";
            // 
            // splitContainerTreeAndImage.Panel1
            // 
            splitContainerTreeAndImage.Panel1.Controls.Add(userControlEventSeriesTree);
            // 
            // splitContainerTreeAndImage.Panel2
            // 
            splitContainerTreeAndImage.Panel2.Controls.Add(splitContainerImageAndGeography);
            splitContainerTreeAndImage.Size = new System.Drawing.Size(1100, 318);
            splitContainerTreeAndImage.SplitterDistance = 498;
            splitContainerTreeAndImage.SplitterWidth = 5;
            splitContainerTreeAndImage.TabIndex = 0;
            // 
            // userControlEventSeriesTree
            // 
            userControlEventSeriesTree.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlEventSeriesTree.Location = new System.Drawing.Point(0, 0);
            userControlEventSeriesTree.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlEventSeriesTree.Name = "userControlEventSeriesTree";
            userControlEventSeriesTree.Size = new System.Drawing.Size(498, 318);
            userControlEventSeriesTree.TabIndex = 0;
            // 
            // splitContainerImageAndGeography
            // 
            splitContainerImageAndGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerImageAndGeography.Location = new System.Drawing.Point(0, 0);
            splitContainerImageAndGeography.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerImageAndGeography.Name = "splitContainerImageAndGeography";
            // 
            // splitContainerImageAndGeography.Panel1
            // 
            splitContainerImageAndGeography.Panel1.Controls.Add(groupBoxImage);
            // 
            // splitContainerImageAndGeography.Panel2
            // 
            splitContainerImageAndGeography.Panel2.Controls.Add(elementHost);
            splitContainerImageAndGeography.Panel2.Controls.Add(toolStripGIS);
            splitContainerImageAndGeography.Panel2Collapsed = true;
            splitContainerImageAndGeography.Size = new System.Drawing.Size(597, 318);
            splitContainerImageAndGeography.SplitterDistance = 425;
            splitContainerImageAndGeography.SplitterWidth = 5;
            splitContainerImageAndGeography.TabIndex = 4;
            // 
            // groupBoxImage
            // 
            groupBoxImage.AccessibleName = "CollectionEventSeriesImages";
            groupBoxImage.Controls.Add(splitContainerImage);
            groupBoxImage.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            groupBoxImage.Location = new System.Drawing.Point(0, 0);
            groupBoxImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxImage.Name = "groupBoxImage";
            groupBoxImage.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            groupBoxImage.Size = new System.Drawing.Size(597, 318);
            groupBoxImage.TabIndex = 3;
            groupBoxImage.TabStop = false;
            groupBoxImage.Tag = "CollectionEventSeries images";
            groupBoxImage.Text = "Event series images";
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
            splitContainerImage.Size = new System.Drawing.Size(589, 299);
            splitContainerImage.SplitterDistance = 471;
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
            userControlImage.Size = new System.Drawing.Size(471, 299);
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
            panelSpecimenImageList.Size = new System.Drawing.Size(113, 158);
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
            listBoxImage.Size = new System.Drawing.Size(89, 158);
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
            toolStripSpecimenImage.Location = new System.Drawing.Point(89, 0);
            toolStripSpecimenImage.Name = "toolStripSpecimenImage";
            toolStripSpecimenImage.Size = new System.Drawing.Size(24, 158);
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
            tableLayoutPanelSpecimenImage.Location = new System.Drawing.Point(0, 158);
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
            tableLayoutPanelSpecimenImage.Size = new System.Drawing.Size(113, 141);
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
            labelSpecimenImageType.Size = new System.Drawing.Size(113, 13);
            labelSpecimenImageType.TabIndex = 3;
            labelSpecimenImageType.Text = "Type:";
            labelSpecimenImageType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxImageType
            // 
            comboBoxImageType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", collectionEventSeriesImageBindingSource, "ImageType", true));
            comboBoxImageType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxImageType.FormattingEnabled = true;
            comboBoxImageType.Location = new System.Drawing.Point(0, 13);
            comboBoxImageType.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            comboBoxImageType.Name = "comboBoxImageType";
            comboBoxImageType.Size = new System.Drawing.Size(113, 21);
            comboBoxImageType.TabIndex = 5;
            // 
            // collectionEventSeriesImageBindingSource
            // 
            collectionEventSeriesImageBindingSource.DataMember = "CollectionEventSeriesImage";
            collectionEventSeriesImageBindingSource.DataSource = dataSetCollectionEventSeries;
            // 
            // dataSetCollectionEventSeries
            // 
            dataSetCollectionEventSeries.DataSetName = "DataSetCollectionEventSeries";
            dataSetCollectionEventSeries.Namespace = "http://tempuri.org/DataSetCollectionEventSeries.xsd";
            dataSetCollectionEventSeries.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelSpecimenImageNotes
            // 
            labelSpecimenImageNotes.AutoSize = true;
            labelSpecimenImageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSpecimenImageNotes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            labelSpecimenImageNotes.Location = new System.Drawing.Point(0, 71);
            labelSpecimenImageNotes.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            labelSpecimenImageNotes.Name = "labelSpecimenImageNotes";
            labelSpecimenImageNotes.Size = new System.Drawing.Size(113, 13);
            labelSpecimenImageNotes.TabIndex = 8;
            labelSpecimenImageNotes.Text = "Notes:";
            labelSpecimenImageNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxImageNotes
            // 
            textBoxImageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", collectionEventSeriesImageBindingSource, "Notes", true));
            textBoxImageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxImageNotes.Location = new System.Drawing.Point(0, 84);
            textBoxImageNotes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            textBoxImageNotes.Multiline = true;
            textBoxImageNotes.Name = "textBoxImageNotes";
            textBoxImageNotes.Size = new System.Drawing.Size(113, 56);
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
            labelSpecimenImageWithholdingReason.Size = new System.Drawing.Size(113, 13);
            labelSpecimenImageWithholdingReason.TabIndex = 12;
            labelSpecimenImageWithholdingReason.Text = "Withh.:";
            labelSpecimenImageWithholdingReason.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxImageWithholdingReason
            // 
            comboBoxImageWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", collectionEventSeriesImageBindingSource, "DataWithholdingReason", true));
            comboBoxImageWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxImageWithholdingReason.FormattingEnabled = true;
            comboBoxImageWithholdingReason.Location = new System.Drawing.Point(0, 48);
            comboBoxImageWithholdingReason.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            comboBoxImageWithholdingReason.Name = "comboBoxImageWithholdingReason";
            comboBoxImageWithholdingReason.Size = new System.Drawing.Size(113, 21);
            comboBoxImageWithholdingReason.TabIndex = 13;
            // 
            // elementHost
            // 
            elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            elementHost.Location = new System.Drawing.Point(0, 0);
            elementHost.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            elementHost.Name = "elementHost";
            elementHost.Size = new System.Drawing.Size(96, 75);
            elementHost.TabIndex = 0;
            elementHost.Text = "elementHost1";
            // 
            // toolStripGIS
            // 
            toolStripGIS.Dock = System.Windows.Forms.DockStyle.Bottom;
            toolStripGIS.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStripGIS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonGeographySave });
            toolStripGIS.Location = new System.Drawing.Point(0, 75);
            toolStripGIS.Name = "toolStripGIS";
            toolStripGIS.Size = new System.Drawing.Size(96, 25);
            toolStripGIS.TabIndex = 1;
            toolStripGIS.Text = "toolStrip1";
            toolStripGIS.ItemClicked += toolStripGIS_ItemClicked;
            // 
            // toolStripButtonGeographySave
            // 
            toolStripButtonGeographySave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButtonGeographySave.Image = Resource.Save;
            toolStripButtonGeographySave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toolStripButtonGeographySave.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonGeographySave.Name = "toolStripButtonGeographySave";
            toolStripButtonGeographySave.Size = new System.Drawing.Size(23, 22);
            toolStripButtonGeographySave.Text = "Save changes in the geography";
            toolStripButtonGeographySave.Click += toolStripButtonGeographySave_Click;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AutoGenerateColumns = false;
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { seriesIDDataGridViewTextBoxColumn, seriesParentIDDataGridViewTextBoxColumn, descriptionDataGridViewTextBoxColumn, seriesCodeDataGridViewTextBoxColumn, notesDataGridViewTextBoxColumn, dateStartDataGridViewTextBoxColumn, dateEndDataGridViewTextBoxColumn });
            dataGridView.DataSource = collectionEventSeriesBindingSource;
            dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView.Location = new System.Drawing.Point(0, 0);
            dataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new System.Drawing.Size(1100, 291);
            dataGridView.TabIndex = 0;
            dataGridView.CellClick += dataGridView_CellClick;
            dataGridView.DataError += dataGridView_DataError;
            dataGridView.EditingControlShowing += dataGridView_EditingControlShowing;
            dataGridView.RowEnter += dataGridView_RowEnter;
            dataGridView.RowPostPaint += dataGridView_RowPostPaint;
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;
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
            descriptionDataGridViewTextBoxColumn.Width = 400;
            // 
            // seriesCodeDataGridViewTextBoxColumn
            // 
            seriesCodeDataGridViewTextBoxColumn.DataPropertyName = "SeriesCode";
            seriesCodeDataGridViewTextBoxColumn.HeaderText = "Series code";
            seriesCodeDataGridViewTextBoxColumn.Name = "seriesCodeDataGridViewTextBoxColumn";
            // 
            // notesDataGridViewTextBoxColumn
            // 
            notesDataGridViewTextBoxColumn.DataPropertyName = "Notes";
            notesDataGridViewTextBoxColumn.HeaderText = "Notes";
            notesDataGridViewTextBoxColumn.Name = "notesDataGridViewTextBoxColumn";
            notesDataGridViewTextBoxColumn.Width = 200;
            // 
            // dateStartDataGridViewTextBoxColumn
            // 
            dateStartDataGridViewTextBoxColumn.DataPropertyName = "DateStart";
            dateStartDataGridViewTextBoxColumn.HeaderText = "Date start";
            dateStartDataGridViewTextBoxColumn.Name = "dateStartDataGridViewTextBoxColumn";
            dateStartDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            dateStartDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dateEndDataGridViewTextBoxColumn
            // 
            dateEndDataGridViewTextBoxColumn.DataPropertyName = "DateEnd";
            dateEndDataGridViewTextBoxColumn.HeaderText = "Date end";
            dateEndDataGridViewTextBoxColumn.Name = "dateEndDataGridViewTextBoxColumn";
            dateEndDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            dateEndDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // collectionEventSeriesBindingSource
            // 
            collectionEventSeriesBindingSource.DataMember = "CollectionEventSeries";
            collectionEventSeriesBindingSource.DataSource = dataSetCollectionEventSeries;
            // 
            // imageListDataset
            // 
            imageListDataset.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListDataset.ImageSize = new System.Drawing.Size(50, 50);
            imageListDataset.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageListForm
            // 
            imageListForm.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListForm.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListForm.ImageStream");
            imageListForm.TransparentColor = System.Drawing.Color.Transparent;
            imageListForm.Images.SetKeyName(0, "");
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(0, 642);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(1100, 31);
            userControlDialogPanel.TabIndex = 1;
            // 
            // collectionEventSeriesTableAdapter
            // 
            collectionEventSeriesTableAdapter.ClearBeforeFill = true;
            // 
            // collectionEventSeriesImageTableAdapter
            // 
            collectionEventSeriesImageTableAdapter.ClearBeforeFill = true;
            // 
            // FormCollectionEventSeriesGridMode
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1100, 673);
            Controls.Add(splitContainerMain);
            Controls.Add(tableLayoutPanelHeader);
            Controls.Add(userControlDialogPanel);
            helpProvider.SetHelpKeyword(this, "gridseries_dc");
            helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCollectionEventSeriesGridMode";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Collection event series - grid mode";
            FormClosing += FormCollectionEventSeriesGridMode_FormClosing;
            Load += FormCollectionEventSeriesGridMode_Load;
            SizeChanged += FormCollectionEventSeriesGridMode_SizeChanged;
            KeyDown += Form_KeyDown;
            tableLayoutPanelHeader.ResumeLayout(false);
            tableLayoutPanelHeader.PerformLayout();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerTreeAndImage.Panel1.ResumeLayout(false);
            splitContainerTreeAndImage.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerTreeAndImage).EndInit();
            splitContainerTreeAndImage.ResumeLayout(false);
            splitContainerImageAndGeography.Panel1.ResumeLayout(false);
            splitContainerImageAndGeography.Panel2.ResumeLayout(false);
            splitContainerImageAndGeography.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerImageAndGeography).EndInit();
            splitContainerImageAndGeography.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)collectionEventSeriesImageBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionEventSeries).EndInit();
            toolStripGIS.ResumeLayout(false);
            toolStripGIS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)collectionEventSeriesBindingSource).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private DiversityCollection.Datasets.DataSetCollectionEventSeries dataSetCollectionEventSeries;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerTreeAndImage;
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
        private System.Windows.Forms.BindingSource collectionEventSeriesBindingSource;
        private DiversityCollection.Datasets.DataSetCollectionEventSeriesTableAdapters.CollectionEventSeriesTableAdapter collectionEventSeriesTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn seriesIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seriesParentIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seriesCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn notesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dateStartDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn dateEndDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageListDataset;
        private System.Windows.Forms.ImageList imageListForm;
        private System.Windows.Forms.BindingSource collectionEventSeriesImageBindingSource;
        private DiversityCollection.Datasets.DataSetCollectionEventSeriesTableAdapters.CollectionEventSeriesImageTableAdapter collectionEventSeriesImageTableAdapter;
        private DiversityCollection.UserControls.UserControlEventSeriesTree userControlEventSeriesTree;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonHeaderShowImage;
        private System.Windows.Forms.Button buttonHeaderShowTree;
        private System.Windows.Forms.Label labelHeaderID;
        private System.Windows.Forms.TextBox textBoxHeaderSeriesID;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.TextBox textBoxHeaderCode;
        private System.Windows.Forms.TextBox textBoxHeaderDescription;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonHeaderShowGeography;
        private System.Windows.Forms.SplitContainer splitContainerImageAndGeography;
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private WpfSamplingPlotPage.WpfControl wpfControl;
        private System.Windows.Forms.ToolStrip toolStripGIS;
        private System.Windows.Forms.ToolStripButton toolStripButtonGeographySave;
        private System.Windows.Forms.Button buttonHistory;
    }
}