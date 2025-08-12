namespace DiversityCollection.Forms
{
    partial class FormCacheDBExport
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCacheDBExport));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageCollection = new System.Windows.Forms.TabPage();
            this.tabControlSpecimenProjects = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.progressBarCollectionTransfer = new System.Windows.Forms.ProgressBar();
            this.tabPageTaxa = new System.Windows.Forms.TabPage();
            this.tabControlTaxa = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainerStart = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelStart = new System.Windows.Forms.TableLayoutPanel();
            this.labelSelectCacheDB = new System.Windows.Forms.Label();
            this.listBoxSelectCacheDB = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelTransfer = new System.Windows.Forms.TableLayoutPanel();
            this.labelCacheDB = new System.Windows.Forms.Label();
            this.textBoxCacheDB = new System.Windows.Forms.TextBox();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.splitContainerVersions = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelVersion1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonV1StartCollectionTransfer = new System.Windows.Forms.Button();
            this.buttonV1StartTaxonTransfer = new System.Windows.Forms.Button();
            this.listBoxV1ProjectPublished = new System.Windows.Forms.ListBox();
            this.listBoxV1ProjectsUnpublished = new System.Windows.Forms.ListBox();
            this.buttonV1PublishProject = new System.Windows.Forms.Button();
            this.buttonV1UnpublishProject = new System.Windows.Forms.Button();
            this.labelProjectsUnpublished = new System.Windows.Forms.Label();
            this.labelProjectsPublished = new System.Windows.Forms.Label();
            this.labelV1LastSpecimenTransfer = new System.Windows.Forms.Label();
            this.labelV1CurrentSpecimenNumber = new System.Windows.Forms.Label();
            this.labelV1LastTaxonTransfer = new System.Windows.Forms.Label();
            this.labelV1CurrentTaxa = new System.Windows.Forms.Label();
            this.buttonV1CheckSpecimen = new System.Windows.Forms.Button();
            this.buttonV1CheckTaxa = new System.Windows.Forms.Button();
            this.panelV1Update = new System.Windows.Forms.Panel();
            this.labelV1Update = new System.Windows.Forms.Label();
            this.buttonV1Update = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataSetCacheDB = new DiversityCollection.CacheDatabase.DataSetCacheDB();
            this.tabControlMain.SuspendLayout();
            this.tabPageCollection.SuspendLayout();
            this.tabControlSpecimenProjects.SuspendLayout();
            this.tabPageTaxa.SuspendLayout();
            this.tabControlTaxa.SuspendLayout();
            this.splitContainerStart.Panel1.SuspendLayout();
            this.splitContainerStart.Panel2.SuspendLayout();
            this.splitContainerStart.SuspendLayout();
            this.tableLayoutPanelStart.SuspendLayout();
            this.tableLayoutPanelTransfer.SuspendLayout();
            this.splitContainerVersions.Panel1.SuspendLayout();
            this.splitContainerVersions.Panel2.SuspendLayout();
            this.splitContainerVersions.SuspendLayout();
            this.tableLayoutPanelVersion1.SuspendLayout();
            this.panelV1Update.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCacheDB)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageCollection);
            this.tabControlMain.Controls.Add(this.tabPageTaxa);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(253, 365);
            this.tabControlMain.TabIndex = 20;
            // 
            // tabPageCollection
            // 
            this.tabPageCollection.Controls.Add(this.tabControlSpecimenProjects);
            this.tabPageCollection.Controls.Add(this.progressBarCollectionTransfer);
            this.tabPageCollection.Location = new System.Drawing.Point(4, 22);
            this.tabPageCollection.Name = "tabPageCollection";
            this.tabPageCollection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCollection.Size = new System.Drawing.Size(245, 339);
            this.tabPageCollection.TabIndex = 1;
            this.tabPageCollection.Text = "Collection data";
            this.tabPageCollection.UseVisualStyleBackColor = true;
            // 
            // tabControlSpecimenProjects
            // 
            this.tabControlSpecimenProjects.Controls.Add(this.tabPage3);
            this.tabControlSpecimenProjects.Controls.Add(this.tabPage4);
            this.tabControlSpecimenProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSpecimenProjects.Location = new System.Drawing.Point(3, 3);
            this.tabControlSpecimenProjects.Name = "tabControlSpecimenProjects";
            this.tabControlSpecimenProjects.SelectedIndex = 0;
            this.tabControlSpecimenProjects.Size = new System.Drawing.Size(239, 319);
            this.tabControlSpecimenProjects.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(231, 293);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(231, 293);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // progressBarCollectionTransfer
            // 
            this.progressBarCollectionTransfer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarCollectionTransfer.Location = new System.Drawing.Point(3, 322);
            this.progressBarCollectionTransfer.Name = "progressBarCollectionTransfer";
            this.progressBarCollectionTransfer.Size = new System.Drawing.Size(239, 14);
            this.progressBarCollectionTransfer.TabIndex = 5;
            // 
            // tabPageTaxa
            // 
            this.tabPageTaxa.Controls.Add(this.tabControlTaxa);
            this.tabPageTaxa.Location = new System.Drawing.Point(4, 22);
            this.tabPageTaxa.Name = "tabPageTaxa";
            this.tabPageTaxa.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTaxa.Size = new System.Drawing.Size(245, 339);
            this.tabPageTaxa.TabIndex = 0;
            this.tabPageTaxa.Text = "Taxonomy";
            this.tabPageTaxa.UseVisualStyleBackColor = true;
            // 
            // tabControlTaxa
            // 
            this.tabControlTaxa.Controls.Add(this.tabPage1);
            this.tabControlTaxa.Controls.Add(this.tabPage2);
            this.tabControlTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTaxa.Location = new System.Drawing.Point(3, 3);
            this.tabControlTaxa.Name = "tabControlTaxa";
            this.tabControlTaxa.SelectedIndex = 0;
            this.tabControlTaxa.Size = new System.Drawing.Size(239, 333);
            this.tabControlTaxa.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(231, 307);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(231, 307);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Replace.ico");
            this.imageList.Images.SetKeyName(1, "CacheDB.ico");
            this.imageList.Images.SetKeyName(2, "Delete.ico");
            // 
            // splitContainerStart
            // 
            this.splitContainerStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerStart.Location = new System.Drawing.Point(0, 0);
            this.splitContainerStart.Name = "splitContainerStart";
            // 
            // splitContainerStart.Panel1
            // 
            this.splitContainerStart.Panel1.Controls.Add(this.tableLayoutPanelStart);
            // 
            // splitContainerStart.Panel2
            // 
            this.splitContainerStart.Panel2.Controls.Add(this.tableLayoutPanelTransfer);
            this.splitContainerStart.Size = new System.Drawing.Size(856, 400);
            this.splitContainerStart.SplitterDistance = 285;
            this.splitContainerStart.TabIndex = 21;
            // 
            // tableLayoutPanelStart
            // 
            this.tableLayoutPanelStart.ColumnCount = 3;
            this.tableLayoutPanelStart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelStart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelStart.Controls.Add(this.labelSelectCacheDB, 1, 1);
            this.tableLayoutPanelStart.Controls.Add(this.listBoxSelectCacheDB, 1, 2);
            this.tableLayoutPanelStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelStart.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelStart.Name = "tableLayoutPanelStart";
            this.tableLayoutPanelStart.RowCount = 4;
            this.tableLayoutPanelStart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelStart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelStart.Size = new System.Drawing.Size(285, 400);
            this.tableLayoutPanelStart.TabIndex = 1;
            // 
            // labelSelectCacheDB
            // 
            this.tableLayoutPanelStart.SetColumnSpan(this.labelSelectCacheDB, 2);
            this.labelSelectCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSelectCacheDB.Location = new System.Drawing.Point(42, 134);
            this.labelSelectCacheDB.Name = "labelSelectCacheDB";
            this.labelSelectCacheDB.Size = new System.Drawing.Size(240, 30);
            this.labelSelectCacheDB.TabIndex = 0;
            this.labelSelectCacheDB.Text = "Please select the cache database to which the data should be transferred";
            this.labelSelectCacheDB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // listBoxSelectCacheDB
            // 
            this.listBoxSelectCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSelectCacheDB.FormattingEnabled = true;
            this.listBoxSelectCacheDB.Location = new System.Drawing.Point(42, 167);
            this.listBoxSelectCacheDB.Name = "listBoxSelectCacheDB";
            this.listBoxSelectCacheDB.Size = new System.Drawing.Size(200, 96);
            this.listBoxSelectCacheDB.TabIndex = 1;
            this.listBoxSelectCacheDB.Click += new System.EventHandler(this.listBoxSelectCacheDB_Click);
            // 
            // tableLayoutPanelTransfer
            // 
            this.tableLayoutPanelTransfer.ColumnCount = 3;
            this.tableLayoutPanelTransfer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTransfer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTransfer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTransfer.Controls.Add(this.labelCacheDB, 0, 0);
            this.tableLayoutPanelTransfer.Controls.Add(this.textBoxCacheDB, 1, 0);
            this.tableLayoutPanelTransfer.Controls.Add(this.buttonFeedback, 2, 0);
            this.tableLayoutPanelTransfer.Controls.Add(this.splitContainerVersions, 0, 1);
            this.tableLayoutPanelTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTransfer.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTransfer.Name = "tableLayoutPanelTransfer";
            this.tableLayoutPanelTransfer.RowCount = 2;
            this.tableLayoutPanelTransfer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTransfer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTransfer.Size = new System.Drawing.Size(567, 400);
            this.tableLayoutPanelTransfer.TabIndex = 21;
            // 
            // labelCacheDB
            // 
            this.labelCacheDB.AutoSize = true;
            this.labelCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCacheDB.Location = new System.Drawing.Point(3, 0);
            this.labelCacheDB.Name = "labelCacheDB";
            this.labelCacheDB.Size = new System.Drawing.Size(85, 29);
            this.labelCacheDB.TabIndex = 21;
            this.labelCacheDB.Text = "Transfer data to:";
            this.labelCacheDB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCacheDB
            // 
            this.textBoxCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCacheDB.Location = new System.Drawing.Point(94, 3);
            this.textBoxCacheDB.Name = "textBoxCacheDB";
            this.textBoxCacheDB.ReadOnly = true;
            this.textBoxCacheDB.Size = new System.Drawing.Size(440, 20);
            this.textBoxCacheDB.TabIndex = 22;
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(540, 3);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(24, 23);
            this.buttonFeedback.TabIndex = 23;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // splitContainerVersions
            // 
            this.tableLayoutPanelTransfer.SetColumnSpan(this.splitContainerVersions, 3);
            this.splitContainerVersions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerVersions.Location = new System.Drawing.Point(3, 32);
            this.splitContainerVersions.Name = "splitContainerVersions";
            // 
            // splitContainerVersions.Panel1
            // 
            this.splitContainerVersions.Panel1.Controls.Add(this.tableLayoutPanelVersion1);
            // 
            // splitContainerVersions.Panel2
            // 
            this.splitContainerVersions.Panel2.Controls.Add(this.tabControlMain);
            this.splitContainerVersions.Size = new System.Drawing.Size(561, 365);
            this.splitContainerVersions.SplitterDistance = 304;
            this.splitContainerVersions.TabIndex = 24;
            // 
            // tableLayoutPanelVersion1
            // 
            this.tableLayoutPanelVersion1.ColumnCount = 5;
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.77778F));
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.tableLayoutPanelVersion1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.77778F));
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1StartCollectionTransfer, 1, 4);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1StartTaxonTransfer, 1, 6);
            this.tableLayoutPanelVersion1.Controls.Add(this.listBoxV1ProjectPublished, 3, 2);
            this.tableLayoutPanelVersion1.Controls.Add(this.listBoxV1ProjectsUnpublished, 0, 2);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1PublishProject, 2, 2);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1UnpublishProject, 2, 3);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelProjectsUnpublished, 0, 1);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelProjectsPublished, 3, 1);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelV1LastSpecimenTransfer, 0, 5);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelV1CurrentSpecimenNumber, 3, 5);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelV1LastTaxonTransfer, 0, 7);
            this.tableLayoutPanelVersion1.Controls.Add(this.labelV1CurrentTaxa, 3, 7);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1CheckSpecimen, 2, 5);
            this.tableLayoutPanelVersion1.Controls.Add(this.buttonV1CheckTaxa, 2, 7);
            this.tableLayoutPanelVersion1.Controls.Add(this.panelV1Update, 0, 0);
            this.tableLayoutPanelVersion1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.tableLayoutPanelVersion1, "Cache database");
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelVersion1, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tableLayoutPanelVersion1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelVersion1.Name = "tableLayoutPanelVersion1";
            this.tableLayoutPanelVersion1.RowCount = 8;
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelVersion1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.helpProvider.SetShowHelp(this.tableLayoutPanelVersion1, true);
            this.tableLayoutPanelVersion1.Size = new System.Drawing.Size(304, 365);
            this.tableLayoutPanelVersion1.TabIndex = 0;
            // 
            // buttonV1StartCollectionTransfer
            // 
            this.tableLayoutPanelVersion1.SetColumnSpan(this.buttonV1StartCollectionTransfer, 3);
            this.buttonV1StartCollectionTransfer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonV1StartCollectionTransfer.Image = global::DiversityCollection.Resource.CollectionSpecimen;
            this.buttonV1StartCollectionTransfer.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonV1StartCollectionTransfer.Location = new System.Drawing.Point(80, 220);
            this.buttonV1StartCollectionTransfer.Name = "buttonV1StartCollectionTransfer";
            this.buttonV1StartCollectionTransfer.Size = new System.Drawing.Size(142, 40);
            this.buttonV1StartCollectionTransfer.TabIndex = 0;
            this.buttonV1StartCollectionTransfer.Text = "Transfer collection data";
            this.buttonV1StartCollectionTransfer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.buttonV1StartCollectionTransfer, "Transfer collection data for all published projects");
            this.buttonV1StartCollectionTransfer.UseVisualStyleBackColor = true;
            this.buttonV1StartCollectionTransfer.Click += new System.EventHandler(this.buttonV1StartCollectionTransfer_Click);
            // 
            // buttonV1StartTaxonTransfer
            // 
            this.tableLayoutPanelVersion1.SetColumnSpan(this.buttonV1StartTaxonTransfer, 3);
            this.buttonV1StartTaxonTransfer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonV1StartTaxonTransfer.Image = global::DiversityCollection.Resource.Identification;
            this.buttonV1StartTaxonTransfer.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonV1StartTaxonTransfer.Location = new System.Drawing.Point(80, 297);
            this.buttonV1StartTaxonTransfer.Name = "buttonV1StartTaxonTransfer";
            this.buttonV1StartTaxonTransfer.Size = new System.Drawing.Size(142, 40);
            this.buttonV1StartTaxonTransfer.TabIndex = 1;
            this.buttonV1StartTaxonTransfer.Text = "Transfer taxonomic names";
            this.buttonV1StartTaxonTransfer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.buttonV1StartTaxonTransfer, "Transfer taxonomic names into cache database");
            this.buttonV1StartTaxonTransfer.UseVisualStyleBackColor = true;
            this.buttonV1StartTaxonTransfer.Click += new System.EventHandler(this.buttonV1StartTaxonTransfer_Click);
            // 
            // listBoxV1ProjectPublished
            // 
            this.listBoxV1ProjectPublished.BackColor = System.Drawing.Color.LightGreen;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.listBoxV1ProjectPublished, 2);
            this.listBoxV1ProjectPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxV1ProjectPublished.FormattingEnabled = true;
            this.listBoxV1ProjectPublished.Location = new System.Drawing.Point(166, 55);
            this.listBoxV1ProjectPublished.Name = "listBoxV1ProjectPublished";
            this.tableLayoutPanelVersion1.SetRowSpan(this.listBoxV1ProjectPublished, 2);
            this.listBoxV1ProjectPublished.Size = new System.Drawing.Size(135, 152);
            this.listBoxV1ProjectPublished.TabIndex = 2;
            // 
            // listBoxV1ProjectsUnpublished
            // 
            this.listBoxV1ProjectsUnpublished.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.listBoxV1ProjectsUnpublished, 2);
            this.listBoxV1ProjectsUnpublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxV1ProjectsUnpublished.FormattingEnabled = true;
            this.listBoxV1ProjectsUnpublished.Location = new System.Drawing.Point(3, 55);
            this.listBoxV1ProjectsUnpublished.Name = "listBoxV1ProjectsUnpublished";
            this.tableLayoutPanelVersion1.SetRowSpan(this.listBoxV1ProjectsUnpublished, 2);
            this.listBoxV1ProjectsUnpublished.Size = new System.Drawing.Size(133, 152);
            this.listBoxV1ProjectsUnpublished.TabIndex = 3;
            // 
            // buttonV1PublishProject
            // 
            this.buttonV1PublishProject.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonV1PublishProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonV1PublishProject.ForeColor = System.Drawing.Color.Green;
            this.buttonV1PublishProject.Location = new System.Drawing.Point(142, 105);
            this.buttonV1PublishProject.Name = "buttonV1PublishProject";
            this.buttonV1PublishProject.Size = new System.Drawing.Size(18, 23);
            this.buttonV1PublishProject.TabIndex = 4;
            this.buttonV1PublishProject.Text = ">";
            this.toolTip.SetToolTip(this.buttonV1PublishProject, "Publish selected project");
            this.buttonV1PublishProject.UseVisualStyleBackColor = true;
            this.buttonV1PublishProject.Click += new System.EventHandler(this.buttonV1PublishProject_Click);
            // 
            // buttonV1UnpublishProject
            // 
            this.buttonV1UnpublishProject.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonV1UnpublishProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonV1UnpublishProject.ForeColor = System.Drawing.Color.Red;
            this.buttonV1UnpublishProject.Location = new System.Drawing.Point(142, 134);
            this.buttonV1UnpublishProject.Name = "buttonV1UnpublishProject";
            this.buttonV1UnpublishProject.Size = new System.Drawing.Size(18, 23);
            this.buttonV1UnpublishProject.TabIndex = 5;
            this.buttonV1UnpublishProject.Text = "<";
            this.toolTip.SetToolTip(this.buttonV1UnpublishProject, "Remove selected project from published list");
            this.buttonV1UnpublishProject.UseVisualStyleBackColor = true;
            this.buttonV1UnpublishProject.Click += new System.EventHandler(this.buttonV1UnpublishProject_Click);
            // 
            // labelProjectsUnpublished
            // 
            this.labelProjectsUnpublished.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelProjectsUnpublished, 2);
            this.labelProjectsUnpublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectsUnpublished.Location = new System.Drawing.Point(3, 32);
            this.labelProjectsUnpublished.Name = "labelProjectsUnpublished";
            this.labelProjectsUnpublished.Size = new System.Drawing.Size(133, 20);
            this.labelProjectsUnpublished.TabIndex = 6;
            this.labelProjectsUnpublished.Text = "Unpublished projects";
            this.labelProjectsUnpublished.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProjectsPublished
            // 
            this.labelProjectsPublished.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelProjectsPublished, 2);
            this.labelProjectsPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjectsPublished.Location = new System.Drawing.Point(166, 32);
            this.labelProjectsPublished.Name = "labelProjectsPublished";
            this.labelProjectsPublished.Size = new System.Drawing.Size(135, 20);
            this.labelProjectsPublished.TabIndex = 7;
            this.labelProjectsPublished.Text = "Published projects";
            this.labelProjectsPublished.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelV1LastSpecimenTransfer
            // 
            this.labelV1LastSpecimenTransfer.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelV1LastSpecimenTransfer, 2);
            this.labelV1LastSpecimenTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1LastSpecimenTransfer.Location = new System.Drawing.Point(3, 263);
            this.labelV1LastSpecimenTransfer.Name = "labelV1LastSpecimenTransfer";
            this.labelV1LastSpecimenTransfer.Size = new System.Drawing.Size(133, 24);
            this.labelV1LastSpecimenTransfer.TabIndex = 8;
            this.labelV1LastSpecimenTransfer.Text = "Last transfer:";
            this.labelV1LastSpecimenTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelV1CurrentSpecimenNumber
            // 
            this.labelV1CurrentSpecimenNumber.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelV1CurrentSpecimenNumber, 2);
            this.labelV1CurrentSpecimenNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1CurrentSpecimenNumber.Location = new System.Drawing.Point(166, 263);
            this.labelV1CurrentSpecimenNumber.Name = "labelV1CurrentSpecimenNumber";
            this.labelV1CurrentSpecimenNumber.Size = new System.Drawing.Size(135, 24);
            this.labelV1CurrentSpecimenNumber.TabIndex = 9;
            this.labelV1CurrentSpecimenNumber.Text = "Current specimens:";
            this.labelV1CurrentSpecimenNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelV1LastTaxonTransfer
            // 
            this.labelV1LastTaxonTransfer.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelV1LastTaxonTransfer, 2);
            this.labelV1LastTaxonTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1LastTaxonTransfer.Location = new System.Drawing.Point(3, 340);
            this.labelV1LastTaxonTransfer.Name = "labelV1LastTaxonTransfer";
            this.labelV1LastTaxonTransfer.Size = new System.Drawing.Size(133, 25);
            this.labelV1LastTaxonTransfer.TabIndex = 10;
            this.labelV1LastTaxonTransfer.Text = "Last transfer:";
            this.labelV1LastTaxonTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelV1CurrentTaxa
            // 
            this.labelV1CurrentTaxa.AutoSize = true;
            this.tableLayoutPanelVersion1.SetColumnSpan(this.labelV1CurrentTaxa, 2);
            this.labelV1CurrentTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1CurrentTaxa.Location = new System.Drawing.Point(166, 340);
            this.labelV1CurrentTaxa.Name = "labelV1CurrentTaxa";
            this.labelV1CurrentTaxa.Size = new System.Drawing.Size(135, 25);
            this.labelV1CurrentTaxa.TabIndex = 11;
            this.labelV1CurrentTaxa.Text = "Current taxa:";
            this.labelV1CurrentTaxa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonV1CheckSpecimen
            // 
            this.buttonV1CheckSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonV1CheckSpecimen.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonV1CheckSpecimen.Location = new System.Drawing.Point(139, 263);
            this.buttonV1CheckSpecimen.Margin = new System.Windows.Forms.Padding(0);
            this.buttonV1CheckSpecimen.Name = "buttonV1CheckSpecimen";
            this.buttonV1CheckSpecimen.Size = new System.Drawing.Size(24, 24);
            this.buttonV1CheckSpecimen.TabIndex = 12;
            this.toolTip.SetToolTip(this.buttonV1CheckSpecimen, "Inspect the specimen of the selected project");
            this.buttonV1CheckSpecimen.UseVisualStyleBackColor = true;
            this.buttonV1CheckSpecimen.Click += new System.EventHandler(this.buttonV1CheckSpecimen_Click);
            // 
            // buttonV1CheckTaxa
            // 
            this.buttonV1CheckTaxa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonV1CheckTaxa.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonV1CheckTaxa.Location = new System.Drawing.Point(139, 340);
            this.buttonV1CheckTaxa.Margin = new System.Windows.Forms.Padding(0);
            this.buttonV1CheckTaxa.Name = "buttonV1CheckTaxa";
            this.buttonV1CheckTaxa.Size = new System.Drawing.Size(24, 25);
            this.buttonV1CheckTaxa.TabIndex = 13;
            this.toolTip.SetToolTip(this.buttonV1CheckTaxa, "Inspect the taxa in the cache database");
            this.buttonV1CheckTaxa.UseVisualStyleBackColor = true;
            this.buttonV1CheckTaxa.Click += new System.EventHandler(this.buttonV1CheckTaxa_Click);
            // 
            // panelV1Update
            // 
            this.tableLayoutPanelVersion1.SetColumnSpan(this.panelV1Update, 5);
            this.panelV1Update.Controls.Add(this.labelV1Update);
            this.panelV1Update.Controls.Add(this.buttonV1Update);
            this.panelV1Update.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelV1Update.Location = new System.Drawing.Point(3, 3);
            this.panelV1Update.Name = "panelV1Update";
            this.panelV1Update.Size = new System.Drawing.Size(298, 26);
            this.panelV1Update.TabIndex = 14;
            // 
            // labelV1Update
            // 
            this.labelV1Update.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV1Update.Location = new System.Drawing.Point(0, 0);
            this.labelV1Update.Name = "labelV1Update";
            this.labelV1Update.Size = new System.Drawing.Size(272, 26);
            this.labelV1Update.TabIndex = 0;
            this.labelV1Update.Text = "Update to version";
            this.labelV1Update.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelV1Update.Visible = false;
            // 
            // buttonV1Update
            // 
            this.buttonV1Update.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonV1Update.Image = global::DiversityCollection.Resource.UpdateDatabase;
            this.buttonV1Update.Location = new System.Drawing.Point(272, 0);
            this.buttonV1Update.Name = "buttonV1Update";
            this.buttonV1Update.Size = new System.Drawing.Size(26, 26);
            this.buttonV1Update.TabIndex = 1;
            this.buttonV1Update.UseVisualStyleBackColor = true;
            this.buttonV1Update.Visible = false;
            this.buttonV1Update.Click += new System.EventHandler(this.buttonV1Update_Click);
            // 
            // dataSetCacheDB
            // 
            this.dataSetCacheDB.DataSetName = "DataSetCacheDB";
            this.dataSetCacheDB.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FormCacheDBExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 400);
            this.Controls.Add(this.splitContainerStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCacheDBExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export to cache database";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageCollection.ResumeLayout(false);
            this.tabControlSpecimenProjects.ResumeLayout(false);
            this.tabPageTaxa.ResumeLayout(false);
            this.tabControlTaxa.ResumeLayout(false);
            this.splitContainerStart.Panel1.ResumeLayout(false);
            this.splitContainerStart.Panel2.ResumeLayout(false);
            this.splitContainerStart.ResumeLayout(false);
            this.tableLayoutPanelStart.ResumeLayout(false);
            this.tableLayoutPanelTransfer.ResumeLayout(false);
            this.tableLayoutPanelTransfer.PerformLayout();
            this.splitContainerVersions.Panel1.ResumeLayout(false);
            this.splitContainerVersions.Panel2.ResumeLayout(false);
            this.splitContainerVersions.ResumeLayout(false);
            this.tableLayoutPanelVersion1.ResumeLayout(false);
            this.tableLayoutPanelVersion1.PerformLayout();
            this.panelV1Update.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCacheDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageTaxa;
        private System.Windows.Forms.TabControl tabControlTaxa;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPageCollection;
        private CacheDatabase.DataSetCacheDB dataSetCacheDB;
        private System.Windows.Forms.TabControl tabControlSpecimenProjects;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.SplitContainer splitContainerStart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStart;
        private System.Windows.Forms.Label labelSelectCacheDB;
        private System.Windows.Forms.ListBox listBoxSelectCacheDB;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransfer;
        private System.Windows.Forms.Label labelCacheDB;
        private System.Windows.Forms.TextBox textBoxCacheDB;
        private System.Windows.Forms.ProgressBar progressBarCollectionTransfer;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.SplitContainer splitContainerVersions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelVersion1;
        private System.Windows.Forms.Button buttonV1StartCollectionTransfer;
        private System.Windows.Forms.Button buttonV1StartTaxonTransfer;
        private System.Windows.Forms.ListBox listBoxV1ProjectPublished;
        private System.Windows.Forms.ListBox listBoxV1ProjectsUnpublished;
        private System.Windows.Forms.Button buttonV1PublishProject;
        private System.Windows.Forms.Button buttonV1UnpublishProject;
        private System.Windows.Forms.Label labelProjectsUnpublished;
        private System.Windows.Forms.Label labelProjectsPublished;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelV1LastSpecimenTransfer;
        private System.Windows.Forms.Label labelV1CurrentSpecimenNumber;
        private System.Windows.Forms.Label labelV1LastTaxonTransfer;
        private System.Windows.Forms.Label labelV1CurrentTaxa;
        private System.Windows.Forms.Button buttonV1CheckSpecimen;
        private System.Windows.Forms.Button buttonV1CheckTaxa;
        private System.Windows.Forms.Panel panelV1Update;
        private System.Windows.Forms.Label labelV1Update;
        private System.Windows.Forms.Button buttonV1Update;
    }
}