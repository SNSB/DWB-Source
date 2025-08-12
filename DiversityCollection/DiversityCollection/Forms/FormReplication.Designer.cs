namespace DiversityCollection.Forms
{
    partial class FormReplication
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReplication));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.buttonStartDownload = new System.Windows.Forms.Button();
            this.labelProject = new System.Windows.Forms.Label();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.pictureBoxServer = new System.Windows.Forms.PictureBox();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.checkBoxEntity = new System.Windows.Forms.CheckBox();
            this.checkBoxProjects = new System.Windows.Forms.CheckBox();
            this.checkBoxEnum = new System.Windows.Forms.CheckBox();
            this.checkBoxBasicData = new System.Windows.Forms.CheckBox();
            this.checkBoxData = new System.Windows.Forms.CheckBox();
            this.buttonClean = new System.Windows.Forms.Button();
            this.buttonRequery = new System.Windows.Forms.Button();
            this.panelSubscriber = new System.Windows.Forms.Panel();
            this.pictureBoxSubscriber = new System.Windows.Forms.PictureBox();
            this.labelSubscriberDatabase = new System.Windows.Forms.Label();
            this.panelPublisher = new System.Windows.Forms.Panel();
            this.pictureBoxPublisher = new System.Windows.Forms.PictureBox();
            this.labelServerDatabase = new System.Windows.Forms.Label();
            this.labelHeaderSubscriber = new System.Windows.Forms.Label();
            this.labelHeaderPublisher = new System.Windows.Forms.Label();
            this.buttonStartMerge = new System.Windows.Forms.Button();
            this.tabControlTableList = new System.Windows.Forms.TabControl();
            this.tabPageTables = new System.Windows.Forms.TabPage();
            this.panelTableList = new System.Windows.Forms.Panel();
            this.tabPageReport = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelReport = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxReport = new System.Windows.Forms.TextBox();
            this.labelReportFile = new System.Windows.Forms.Label();
            this.checkBoxIgnoreConflicts = new System.Windows.Forms.CheckBox();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.buttonClearFilter = new System.Windows.Forms.Button();
            this.imageListConnection = new System.Windows.Forms.ImageList(this.components);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListDirection = new System.Windows.Forms.ImageList(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxServer)).BeginInit();
            this.panelSubscriber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubscriber)).BeginInit();
            this.panelPublisher.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPublisher)).BeginInit();
            this.tabControlTableList.SuspendLayout();
            this.tabPageTables.SuspendLayout();
            this.tabPageReport.SuspendLayout();
            this.tableLayoutPanelReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 7;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxProject, 4, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStartDownload, 5, 7);
            this.tableLayoutPanelMain.Controls.Add(this.labelProject, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUpload, 5, 8);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxServer, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonAll, 1, 5);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonNone, 2, 5);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxEntity, 5, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxProjects, 6, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxEnum, 3, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxBasicData, 3, 5);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxData, 5, 5);
            this.tableLayoutPanelMain.Controls.Add(this.buttonClean, 2, 8);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRequery, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.panelSubscriber, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.panelPublisher, 4, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeaderSubscriber, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeaderPublisher, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStartMerge, 2, 7);
            this.tableLayoutPanelMain.Controls.Add(this.tabControlTableList, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxIgnoreConflicts, 4, 7);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 6, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonClearFilter, 6, 5);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 9;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(854, 544);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // comboBoxProject
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxProject, 2);
            this.comboBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(250, 53);
            this.comboBoxProject.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(507, 21);
            this.comboBoxProject.TabIndex = 22;
            this.comboBoxProject.Visible = false;
            this.comboBoxProject.SelectionChangeCommitted += new System.EventHandler(this.comboBoxProject_SelectionChangeCommitted);
            // 
            // buttonStartDownload
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonStartDownload, 2);
            this.buttonStartDownload.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStartDownload.Image = global::DiversityCollection.Resource.Download;
            this.buttonStartDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartDownload.Location = new System.Drawing.Point(746, 485);
            this.buttonStartDownload.Name = "buttonStartDownload";
            this.buttonStartDownload.Size = new System.Drawing.Size(105, 25);
            this.buttonStartDownload.TabIndex = 29;
            this.buttonStartDownload.Text = "Start download";
            this.buttonStartDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStartDownload.UseVisualStyleBackColor = true;
            this.buttonStartDownload.Click += new System.EventHandler(this.buttonStartDownload_Click);
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelProject, 3);
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Location = new System.Drawing.Point(3, 50);
            this.labelProject.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(231, 27);
            this.labelProject.TabIndex = 28;
            this.labelProject.Text = "Project:";
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonUpload
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonUpload, 2);
            this.buttonUpload.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonUpload.Image = global::DiversityCollection.Resource.Upload;
            this.buttonUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonUpload.Location = new System.Drawing.Point(760, 516);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(91, 25);
            this.buttonUpload.TabIndex = 31;
            this.buttonUpload.Text = "Start upload";
            this.buttonUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // pictureBoxServer
            // 
            this.pictureBoxServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxServer.Image = global::DiversityCollection.Resource.Database;
            this.pictureBoxServer.Location = new System.Drawing.Point(234, 27);
            this.pictureBoxServer.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
            this.pictureBoxServer.Name = "pictureBoxServer";
            this.pictureBoxServer.Size = new System.Drawing.Size(16, 23);
            this.pictureBoxServer.TabIndex = 32;
            this.pictureBoxServer.TabStop = false;
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.radioButtonAll.Location = new System.Drawing.Point(49, 104);
            this.radioButtonAll.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(67, 23);
            this.radioButtonAll.TabIndex = 34;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All tables";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            this.radioButtonAll.Click += new System.EventHandler(this.radioButtonAll_Click);
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Dock = System.Windows.Forms.DockStyle.Left;
            this.radioButtonNone.Location = new System.Drawing.Point(122, 104);
            this.radioButtonNone.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(49, 23);
            this.radioButtonNone.TabIndex = 35;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Text = "none";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            this.radioButtonNone.Click += new System.EventHandler(this.radioButtonNone_Click);
            // 
            // checkBoxEntity
            // 
            this.checkBoxEntity.AutoSize = true;
            this.checkBoxEntity.Checked = true;
            this.checkBoxEntity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEntity.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxEntity.Location = new System.Drawing.Point(508, 87);
            this.checkBoxEntity.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxEntity.Name = "checkBoxEntity";
            this.checkBoxEntity.Size = new System.Drawing.Size(249, 17);
            this.checkBoxEntity.TabIndex = 36;
            this.checkBoxEntity.Text = "Descriptions (Entity)";
            this.checkBoxEntity.UseVisualStyleBackColor = true;
            // 
            // checkBoxProjects
            // 
            this.checkBoxProjects.AutoSize = true;
            this.checkBoxProjects.Checked = true;
            this.checkBoxProjects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxProjects.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxProjects.Location = new System.Drawing.Point(763, 87);
            this.checkBoxProjects.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxProjects.Name = "checkBoxProjects";
            this.checkBoxProjects.Size = new System.Drawing.Size(88, 17);
            this.checkBoxProjects.TabIndex = 37;
            this.checkBoxProjects.Text = "Project, User";
            this.checkBoxProjects.UseVisualStyleBackColor = true;
            // 
            // checkBoxEnum
            // 
            this.checkBoxEnum.AutoSize = true;
            this.checkBoxEnum.Checked = true;
            this.checkBoxEnum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxEnum, 2);
            this.checkBoxEnum.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxEnum.Location = new System.Drawing.Point(237, 87);
            this.checkBoxEnum.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxEnum.Name = "checkBoxEnum";
            this.checkBoxEnum.Size = new System.Drawing.Size(265, 17);
            this.checkBoxEnum.TabIndex = 38;
            this.checkBoxEnum.Text = "Definitions (*_Enum)";
            this.checkBoxEnum.UseVisualStyleBackColor = true;
            // 
            // checkBoxBasicData
            // 
            this.checkBoxBasicData.AutoSize = true;
            this.checkBoxBasicData.Checked = true;
            this.checkBoxBasicData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxBasicData, 2);
            this.checkBoxBasicData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxBasicData.Location = new System.Drawing.Point(237, 104);
            this.checkBoxBasicData.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxBasicData.Name = "checkBoxBasicData";
            this.checkBoxBasicData.Size = new System.Drawing.Size(265, 23);
            this.checkBoxBasicData.TabIndex = 39;
            this.checkBoxBasicData.Text = "Basic data";
            this.checkBoxBasicData.UseVisualStyleBackColor = true;
            // 
            // checkBoxData
            // 
            this.checkBoxData.AutoSize = true;
            this.checkBoxData.Checked = true;
            this.checkBoxData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxData.Location = new System.Drawing.Point(508, 104);
            this.checkBoxData.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxData.Name = "checkBoxData";
            this.checkBoxData.Size = new System.Drawing.Size(249, 23);
            this.checkBoxData.TabIndex = 40;
            this.checkBoxData.Text = "Data";
            this.checkBoxData.UseVisualStyleBackColor = true;
            // 
            // buttonClean
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonClean, 2);
            this.buttonClean.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClean.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClean.ForeColor = System.Drawing.Color.Red;
            this.buttonClean.Image = global::DiversityCollection.Resource.CleanDatabase;
            this.buttonClean.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonClean.Location = new System.Drawing.Point(122, 516);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(125, 25);
            this.buttonClean.TabIndex = 41;
            this.buttonClean.Text = "Clean database";
            this.buttonClean.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClean.UseVisualStyleBackColor = true;
            this.buttonClean.Visible = false;
            this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
            // 
            // buttonRequery
            // 
            this.buttonRequery.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRequery.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRequery.Location = new System.Drawing.Point(3, 103);
            this.buttonRequery.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.buttonRequery.Name = "buttonRequery";
            this.tableLayoutPanelMain.SetRowSpan(this.buttonRequery, 2);
            this.buttonRequery.Size = new System.Drawing.Size(43, 24);
            this.buttonRequery.TabIndex = 42;
            this.toolTip.SetToolTip(this.buttonRequery, "Reload table list");
            this.buttonRequery.UseVisualStyleBackColor = true;
            this.buttonRequery.Click += new System.EventHandler(this.buttonRequery_Click);
            // 
            // panelSubscriber
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.panelSubscriber, 3);
            this.panelSubscriber.Controls.Add(this.pictureBoxSubscriber);
            this.panelSubscriber.Controls.Add(this.labelSubscriberDatabase);
            this.panelSubscriber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSubscriber.Location = new System.Drawing.Point(0, 20);
            this.panelSubscriber.Margin = new System.Windows.Forms.Padding(0);
            this.panelSubscriber.Name = "panelSubscriber";
            this.panelSubscriber.Padding = new System.Windows.Forms.Padding(0, 14, 0, 0);
            this.panelSubscriber.Size = new System.Drawing.Size(234, 30);
            this.panelSubscriber.TabIndex = 44;
            // 
            // pictureBoxSubscriber
            // 
            this.pictureBoxSubscriber.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxSubscriber.Image = global::DiversityCollection.Resource.Laptop;
            this.pictureBoxSubscriber.Location = new System.Drawing.Point(151, 14);
            this.pictureBoxSubscriber.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxSubscriber.Name = "pictureBoxSubscriber";
            this.pictureBoxSubscriber.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSubscriber.TabIndex = 44;
            this.pictureBoxSubscriber.TabStop = false;
            // 
            // labelSubscriberDatabase
            // 
            this.labelSubscriberDatabase.AutoSize = true;
            this.labelSubscriberDatabase.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelSubscriberDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubscriberDatabase.Location = new System.Drawing.Point(167, 14);
            this.labelSubscriberDatabase.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelSubscriberDatabase.Name = "labelSubscriberDatabase";
            this.labelSubscriberDatabase.Size = new System.Drawing.Size(67, 13);
            this.labelSubscriberDatabase.TabIndex = 43;
            this.labelSubscriberDatabase.Text = "Subscriber";
            this.labelSubscriberDatabase.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // panelPublisher
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.panelPublisher, 3);
            this.panelPublisher.Controls.Add(this.pictureBoxPublisher);
            this.panelPublisher.Controls.Add(this.labelServerDatabase);
            this.panelPublisher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPublisher.Location = new System.Drawing.Point(250, 20);
            this.panelPublisher.Margin = new System.Windows.Forms.Padding(0);
            this.panelPublisher.Name = "panelPublisher";
            this.panelPublisher.Size = new System.Drawing.Size(604, 30);
            this.panelPublisher.TabIndex = 45;
            // 
            // pictureBoxPublisher
            // 
            this.pictureBoxPublisher.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxPublisher.Image = global::DiversityCollection.Resource.Server1;
            this.pictureBoxPublisher.Location = new System.Drawing.Point(59, 0);
            this.pictureBoxPublisher.Name = "pictureBoxPublisher";
            this.pictureBoxPublisher.Size = new System.Drawing.Size(16, 30);
            this.pictureBoxPublisher.TabIndex = 34;
            this.pictureBoxPublisher.TabStop = false;
            // 
            // labelServerDatabase
            // 
            this.labelServerDatabase.AutoSize = true;
            this.labelServerDatabase.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelServerDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelServerDatabase.Location = new System.Drawing.Point(0, 0);
            this.labelServerDatabase.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelServerDatabase.Name = "labelServerDatabase";
            this.labelServerDatabase.Size = new System.Drawing.Size(59, 13);
            this.labelServerDatabase.TabIndex = 33;
            this.labelServerDatabase.Text = "Publisher";
            // 
            // labelHeaderSubscriber
            // 
            this.labelHeaderSubscriber.AutoSize = true;
            this.labelHeaderSubscriber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeaderSubscriber.Location = new System.Drawing.Point(122, 0);
            this.labelHeaderSubscriber.Name = "labelHeaderSubscriber";
            this.labelHeaderSubscriber.Size = new System.Drawing.Size(109, 20);
            this.labelHeaderSubscriber.TabIndex = 46;
            this.labelHeaderSubscriber.Text = "Subscriber";
            this.labelHeaderSubscriber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeaderPublisher
            // 
            this.labelHeaderPublisher.AutoSize = true;
            this.labelHeaderPublisher.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeaderPublisher.Location = new System.Drawing.Point(253, 0);
            this.labelHeaderPublisher.Name = "labelHeaderPublisher";
            this.labelHeaderPublisher.Size = new System.Drawing.Size(249, 20);
            this.labelHeaderPublisher.TabIndex = 47;
            this.labelHeaderPublisher.Text = "Publisher";
            this.labelHeaderPublisher.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonStartMerge
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonStartMerge, 2);
            this.buttonStartMerge.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStartMerge.Image = global::DiversityCollection.Resource.Merge;
            this.buttonStartMerge.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartMerge.Location = new System.Drawing.Point(162, 485);
            this.buttonStartMerge.Name = "buttonStartMerge";
            this.buttonStartMerge.Size = new System.Drawing.Size(85, 25);
            this.buttonStartMerge.TabIndex = 48;
            this.buttonStartMerge.Text = "Start merge";
            this.buttonStartMerge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStartMerge.UseVisualStyleBackColor = true;
            this.buttonStartMerge.Visible = false;
            this.buttonStartMerge.Click += new System.EventHandler(this.buttonStartMerge_Click);
            // 
            // tabControlTableList
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.tabControlTableList, 7);
            this.tabControlTableList.Controls.Add(this.tabPageTables);
            this.tabControlTableList.Controls.Add(this.tabPageReport);
            this.tabControlTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTableList.Location = new System.Drawing.Point(3, 130);
            this.tabControlTableList.Name = "tabControlTableList";
            this.tabControlTableList.SelectedIndex = 0;
            this.tabControlTableList.Size = new System.Drawing.Size(848, 349);
            this.tabControlTableList.TabIndex = 49;
            // 
            // tabPageTables
            // 
            this.tabPageTables.Controls.Add(this.panelTableList);
            this.tabPageTables.Location = new System.Drawing.Point(4, 22);
            this.tabPageTables.Name = "tabPageTables";
            this.tabPageTables.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTables.Size = new System.Drawing.Size(840, 323);
            this.tabPageTables.TabIndex = 0;
            this.tabPageTables.Text = "Tables";
            this.tabPageTables.UseVisualStyleBackColor = true;
            // 
            // panelTableList
            // 
            this.panelTableList.AutoScroll = true;
            this.panelTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableList.Location = new System.Drawing.Point(3, 3);
            this.panelTableList.Name = "panelTableList";
            this.panelTableList.Size = new System.Drawing.Size(834, 317);
            this.panelTableList.TabIndex = 18;
            // 
            // tabPageReport
            // 
            this.tabPageReport.Controls.Add(this.tableLayoutPanelReport);
            this.tabPageReport.Location = new System.Drawing.Point(4, 22);
            this.tabPageReport.Name = "tabPageReport";
            this.tabPageReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReport.Size = new System.Drawing.Size(840, 323);
            this.tabPageReport.TabIndex = 1;
            this.tabPageReport.Text = "Report";
            this.tabPageReport.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelReport
            // 
            this.tableLayoutPanelReport.ColumnCount = 2;
            this.tableLayoutPanelReport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelReport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelReport.Controls.Add(this.textBoxReport, 0, 0);
            this.tableLayoutPanelReport.Controls.Add(this.labelReportFile, 0, 1);
            this.tableLayoutPanelReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReport.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelReport.Name = "tableLayoutPanelReport";
            this.tableLayoutPanelReport.RowCount = 2;
            this.tableLayoutPanelReport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelReport.Size = new System.Drawing.Size(834, 317);
            this.tableLayoutPanelReport.TabIndex = 0;
            // 
            // textBoxReport
            // 
            this.tableLayoutPanelReport.SetColumnSpan(this.textBoxReport, 2);
            this.textBoxReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReport.Location = new System.Drawing.Point(3, 3);
            this.textBoxReport.Multiline = true;
            this.textBoxReport.Name = "textBoxReport";
            this.textBoxReport.ReadOnly = true;
            this.textBoxReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxReport.Size = new System.Drawing.Size(828, 281);
            this.textBoxReport.TabIndex = 0;
            // 
            // labelReportFile
            // 
            this.labelReportFile.AutoSize = true;
            this.tableLayoutPanelReport.SetColumnSpan(this.labelReportFile, 2);
            this.labelReportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReportFile.Location = new System.Drawing.Point(3, 287);
            this.labelReportFile.Name = "labelReportFile";
            this.labelReportFile.Size = new System.Drawing.Size(828, 30);
            this.labelReportFile.TabIndex = 1;
            // 
            // checkBoxIgnoreConflicts
            // 
            this.checkBoxIgnoreConflicts.AutoSize = true;
            this.checkBoxIgnoreConflicts.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxIgnoreConflicts.Location = new System.Drawing.Point(253, 485);
            this.checkBoxIgnoreConflicts.MinimumSize = new System.Drawing.Size(0, 30);
            this.checkBoxIgnoreConflicts.Name = "checkBoxIgnoreConflicts";
            this.tableLayoutPanelMain.SetRowSpan(this.checkBoxIgnoreConflicts, 2);
            this.checkBoxIgnoreConflicts.Size = new System.Drawing.Size(98, 56);
            this.checkBoxIgnoreConflicts.TabIndex = 50;
            this.checkBoxIgnoreConflicts.Text = "Ignore conflicts";
            this.checkBoxIgnoreConflicts.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreConflicts.Visible = false;
            this.checkBoxIgnoreConflicts.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreConflicts_CheckedChanged);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(831, 0);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(23, 20);
            this.buttonFeedback.TabIndex = 51;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // buttonClearFilter
            // 
            this.buttonClearFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonClearFilter.FlatAppearance.BorderSize = 0;
            this.buttonClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearFilter.Image = global::DiversityCollection.Resource.FilterClear;
            this.buttonClearFilter.Location = new System.Drawing.Point(760, 104);
            this.buttonClearFilter.Margin = new System.Windows.Forms.Padding(0);
            this.buttonClearFilter.Name = "buttonClearFilter";
            this.buttonClearFilter.Size = new System.Drawing.Size(94, 23);
            this.buttonClearFilter.TabIndex = 52;
            this.toolTip.SetToolTip(this.buttonClearFilter, "Clear all filters");
            this.buttonClearFilter.UseVisualStyleBackColor = true;
            this.buttonClearFilter.Visible = false;
            this.buttonClearFilter.Click += new System.EventHandler(this.buttonClearFilter_Click);
            // 
            // imageListConnection
            // 
            this.imageListConnection.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListConnection.ImageStream")));
            this.imageListConnection.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListConnection.Images.SetKeyName(0, "Database.ico");
            this.imageListConnection.Images.SetKeyName(1, "NoDatabase.ico");
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Download.ico");
            this.imageList.Images.SetKeyName(1, "Upload.ico");
            this.imageList.Images.SetKeyName(2, "CleanDatabase.ico");
            this.imageList.Images.SetKeyName(3, "Merge.ico");
            this.imageList.Images.SetKeyName(4, "Arrow.ico");
            this.imageList.Images.SetKeyName(5, "ReplicationList.ico");
            this.imageList.Images.SetKeyName(6, "Server.ico");
            this.imageList.Images.SetKeyName(7, "Labtop.ico");
            // 
            // imageListDirection
            // 
            this.imageListDirection.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDirection.ImageStream")));
            this.imageListDirection.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListDirection.Images.SetKeyName(0, "Download.ico");
            this.imageListDirection.Images.SetKeyName(1, "Upload.ico");
            this.imageListDirection.Images.SetKeyName(2, "CleanDatabase.ico");
            this.imageListDirection.Images.SetKeyName(3, "Merge.ico");
            // 
            // FormReplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 544);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this, "Replication");
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormReplication";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Replication";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormReplication_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxServer)).EndInit();
            this.panelSubscriber.ResumeLayout(false);
            this.panelSubscriber.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSubscriber)).EndInit();
            this.panelPublisher.ResumeLayout(false);
            this.panelPublisher.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPublisher)).EndInit();
            this.tabControlTableList.ResumeLayout(false);
            this.tabPageTables.ResumeLayout(false);
            this.tabPageReport.ResumeLayout(false);
            this.tableLayoutPanelReport.ResumeLayout(false);
            this.tableLayoutPanelReport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageListConnection;
        private System.Windows.Forms.Panel panelTableList;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.Button buttonStartDownload;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.PictureBox pictureBoxServer;
        private System.Windows.Forms.Label labelServerDatabase;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.CheckBox checkBoxEntity;
        private System.Windows.Forms.CheckBox checkBoxProjects;
        private System.Windows.Forms.CheckBox checkBoxEnum;
        private System.Windows.Forms.CheckBox checkBoxBasicData;
        private System.Windows.Forms.CheckBox checkBoxData;
        private System.Windows.Forms.Button buttonClean;
        private System.Windows.Forms.Button buttonRequery;
        private System.Windows.Forms.Label labelSubscriberDatabase;
        private System.Windows.Forms.ImageList imageListDirection;
        private System.Windows.Forms.Panel panelSubscriber;
        private System.Windows.Forms.PictureBox pictureBoxSubscriber;
        private System.Windows.Forms.Panel panelPublisher;
        private System.Windows.Forms.PictureBox pictureBoxPublisher;
        private System.Windows.Forms.Label labelHeaderSubscriber;
        private System.Windows.Forms.Label labelHeaderPublisher;
        private System.Windows.Forms.Button buttonStartMerge;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TabControl tabControlTableList;
        private System.Windows.Forms.TabPage tabPageTables;
        private System.Windows.Forms.TabPage tabPageReport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReport;
        private System.Windows.Forms.TextBox textBoxReport;
        private System.Windows.Forms.Label labelReportFile;
        private System.Windows.Forms.CheckBox checkBoxIgnoreConflicts;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonClearFilter;
    }
}