namespace DiversityWorkbench.Forms
{
    partial class FormStableIdentifier
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStableIdentifier));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTest = new System.Windows.Forms.Button();
            this.pictureBoxQRcode = new System.Windows.Forms.PictureBox();
            this.textBoxBase = new System.Windows.Forms.TextBox();
            this.projectProxyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetStableIdentifier = new DiversityWorkbench.Datasets.DataSetStableIdentifier();
            this.labelBaseURL = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.labelProject = new System.Windows.Forms.Label();
            this.buttonCopyBasicUrlToAll = new System.Windows.Forms.Button();
            this.buttonRemoveAll = new System.Windows.Forms.Button();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.listBoxIdentifier = new System.Windows.Forms.ListBox();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.projectProxyTableAdapter = new DiversityWorkbench.Datasets.DataSetStableIdentifierTableAdapters.ProjectProxyTableAdapter();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelAll = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonSetForDatabase = new System.Windows.Forms.Button();
            this.labelProjects = new System.Windows.Forms.Label();
            this.tableLayoutPanelIdservice = new System.Windows.Forms.TableLayoutPanel();
            this.labelIdService = new System.Windows.Forms.Label();
            this.buttonIdService = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRcode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectProxyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetStableIdentifier)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelAll.SuspendLayout();
            this.tableLayoutPanelIdservice.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.buttonTest, 1, 5);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxQRcode, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxBase, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelBaseURL, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelType, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxType, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelProject, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCopyBasicUrlToAll, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRemoveAll, 1, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(257, 262);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // buttonTest
            // 
            this.buttonTest.Image = global::DiversityWorkbench.Properties.Resources.QRcode;
            this.buttonTest.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonTest.Location = new System.Drawing.Point(218, 154);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(36, 38);
            this.buttonTest.TabIndex = 1;
            this.buttonTest.Text = "Test";
            this.buttonTest.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip.SetToolTip(this.buttonTest, "Generate a QR-code for the basic URL");
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // pictureBoxQRcode
            // 
            this.pictureBoxQRcode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxQRcode.Location = new System.Drawing.Point(3, 154);
            this.pictureBoxQRcode.Name = "pictureBoxQRcode";
            this.pictureBoxQRcode.Size = new System.Drawing.Size(212, 105);
            this.pictureBoxQRcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxQRcode.TabIndex = 2;
            this.pictureBoxQRcode.TabStop = false;
            // 
            // textBoxBase
            // 
            this.textBoxBase.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.projectProxyBindingSource, "StableIdentifierBase", true));
            this.textBoxBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxBase.Location = new System.Drawing.Point(3, 88);
            this.textBoxBase.Name = "textBoxBase";
            this.textBoxBase.Size = new System.Drawing.Size(212, 20);
            this.textBoxBase.TabIndex = 0;
            // 
            // projectProxyBindingSource
            // 
            this.projectProxyBindingSource.DataMember = "ProjectProxy";
            this.projectProxyBindingSource.DataSource = this.dataSetStableIdentifier;
            // 
            // dataSetStableIdentifier
            // 
            this.dataSetStableIdentifier.DataSetName = "DataSetStableIdentifier";
            this.dataSetStableIdentifier.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelBaseURL
            // 
            this.labelBaseURL.AutoSize = true;
            this.labelBaseURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBaseURL.Location = new System.Drawing.Point(3, 20);
            this.labelBaseURL.Name = "labelBaseURL";
            this.labelBaseURL.Size = new System.Drawing.Size(212, 65);
            this.labelBaseURL.TabIndex = 3;
            this.labelBaseURL.Text = resources.GetString("labelBaseURL.Text");
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(3, 111);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(103, 13);
            this.labelType.TabIndex = 4;
            this.labelType.Text = "Type of the identifier";
            // 
            // comboBoxType
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxType, 2);
            this.comboBoxType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.projectProxyBindingSource, "StableIdentifierTypeID", true));
            this.comboBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(3, 127);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(251, 21);
            this.comboBoxType.TabIndex = 5;
            this.toolTip.SetToolTip(this.comboBoxType, "The type of the identifier");
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.projectProxyBindingSource, "Project", true));
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProject.Location = new System.Drawing.Point(3, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(212, 20);
            this.labelProject.TabIndex = 6;
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCopyBasicUrlToAll
            // 
            this.buttonCopyBasicUrlToAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCopyBasicUrlToAll.Image = global::DiversityWorkbench.Properties.Resources.Append;
            this.buttonCopyBasicUrlToAll.Location = new System.Drawing.Point(218, 85);
            this.buttonCopyBasicUrlToAll.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonCopyBasicUrlToAll.Name = "buttonCopyBasicUrlToAll";
            this.tableLayoutPanelMain.SetRowSpan(this.buttonCopyBasicUrlToAll, 2);
            this.buttonCopyBasicUrlToAll.Size = new System.Drawing.Size(36, 39);
            this.buttonCopyBasicUrlToAll.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonCopyBasicUrlToAll, "Copy the current basic URL to all projects in the database");
            this.buttonCopyBasicUrlToAll.UseVisualStyleBackColor = true;
            this.buttonCopyBasicUrlToAll.Click += new System.EventHandler(this.buttonCopyBasicUrlToAll_Click);
            // 
            // buttonRemoveAll
            // 
            this.buttonRemoveAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonRemoveAll.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemoveAll.Location = new System.Drawing.Point(218, 61);
            this.buttonRemoveAll.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonRemoveAll.Name = "buttonRemoveAll";
            this.buttonRemoveAll.Size = new System.Drawing.Size(36, 24);
            this.buttonRemoveAll.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonRemoveAll, "Remove all Basic URLs");
            this.buttonRemoveAll.UseVisualStyleBackColor = true;
            this.buttonRemoveAll.Click += new System.EventHandler(this.buttonRemoveAll_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(3, 71);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.listBoxIdentifier);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelMain);
            this.splitContainerMain.Size = new System.Drawing.Size(425, 262);
            this.splitContainerMain.SplitterDistance = 164;
            this.splitContainerMain.TabIndex = 2;
            // 
            // listBoxIdentifier
            // 
            this.listBoxIdentifier.DataSource = this.projectProxyBindingSource;
            this.listBoxIdentifier.DisplayMember = "Project";
            this.listBoxIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxIdentifier.FormattingEnabled = true;
            this.listBoxIdentifier.Location = new System.Drawing.Point(0, 0);
            this.listBoxIdentifier.Name = "listBoxIdentifier";
            this.listBoxIdentifier.Size = new System.Drawing.Size(164, 262);
            this.listBoxIdentifier.TabIndex = 0;
            this.listBoxIdentifier.ValueMember = "ProjectID";
            this.listBoxIdentifier.SelectedIndexChanged += new System.EventHandler(this.listBoxIdentifier_SelectedIndexChanged);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 408);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(431, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // projectProxyTableAdapter
            // 
            this.projectProxyTableAdapter.ClearBeforeFill = true;
            // 
            // tableLayoutPanelAll
            // 
            this.tableLayoutPanelAll.ColumnCount = 1;
            this.tableLayoutPanelAll.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAll.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelAll.Controls.Add(this.splitContainerMain, 0, 3);
            this.tableLayoutPanelAll.Controls.Add(this.buttonSetForDatabase, 0, 1);
            this.tableLayoutPanelAll.Controls.Add(this.labelProjects, 0, 2);
            this.tableLayoutPanelAll.Controls.Add(this.tableLayoutPanelIdservice, 0, 4);
            this.tableLayoutPanelAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAll.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelAll.Name = "tableLayoutPanelAll";
            this.tableLayoutPanelAll.RowCount = 5;
            this.tableLayoutPanelAll.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAll.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAll.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAll.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAll.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAll.Size = new System.Drawing.Size(431, 408);
            this.tableLayoutPanelAll.TabIndex = 3;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 3);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(425, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "The basis of the stable identifier can be set for the whole database and for ever" +
    "y project";
            // 
            // buttonSetForDatabase
            // 
            this.buttonSetForDatabase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonSetForDatabase.Image = global::DiversityWorkbench.Properties.Resources.QRcode;
            this.buttonSetForDatabase.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSetForDatabase.Location = new System.Drawing.Point(3, 22);
            this.buttonSetForDatabase.Name = "buttonSetForDatabase";
            this.buttonSetForDatabase.Size = new System.Drawing.Size(425, 24);
            this.buttonSetForDatabase.TabIndex = 1;
            this.buttonSetForDatabase.Text = "Set for the whole database";
            this.buttonSetForDatabase.UseVisualStyleBackColor = true;
            this.buttonSetForDatabase.Click += new System.EventHandler(this.buttonSetForDatabase_Click);
            // 
            // labelProjects
            // 
            this.labelProjects.AutoSize = true;
            this.labelProjects.Location = new System.Drawing.Point(3, 55);
            this.labelProjects.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelProjects.Name = "labelProjects";
            this.labelProjects.Size = new System.Drawing.Size(129, 13);
            this.labelProjects.TabIndex = 2;
            this.labelProjects.Text = "Set basis for every project";
            // 
            // tableLayoutPanelIdservice
            // 
            this.tableLayoutPanelIdservice.ColumnCount = 1;
            this.tableLayoutPanelIdservice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelIdservice.Controls.Add(this.labelIdService, 0, 0);
            this.tableLayoutPanelIdservice.Controls.Add(this.buttonIdService, 0, 1);
            this.tableLayoutPanelIdservice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelIdservice.Location = new System.Drawing.Point(3, 339);
            this.tableLayoutPanelIdservice.Name = "tableLayoutPanelIdservice";
            this.tableLayoutPanelIdservice.RowCount = 2;
            this.tableLayoutPanelIdservice.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelIdservice.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelIdservice.Size = new System.Drawing.Size(425, 66);
            this.tableLayoutPanelIdservice.TabIndex = 3;
            // 
            // labelIdService
            // 
            this.labelIdService.AutoSize = true;
            this.labelIdService.Location = new System.Drawing.Point(3, 0);
            this.labelIdService.Name = "labelIdService";
            this.labelIdService.Size = new System.Drawing.Size(419, 26);
            this.labelIdService.TabIndex = 0;
            this.labelIdService.Text = "The role StableIDServices and a login idservice for the access via a REST webserv" +
    "ice is missing";
            this.labelIdService.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonIdService
            // 
            this.buttonIdService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonIdService.Image = global::DiversityWorkbench.Properties.Resources.Login;
            this.buttonIdService.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.buttonIdService.Location = new System.Drawing.Point(3, 29);
            this.buttonIdService.Name = "buttonIdService";
            this.buttonIdService.Size = new System.Drawing.Size(419, 34);
            this.buttonIdService.TabIndex = 1;
            this.buttonIdService.Text = "      Create a group StableIDServices and a login idservice for the access via a " +
    "REST webservice";
            this.buttonIdService.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.buttonIdService.UseVisualStyleBackColor = true;
            this.buttonIdService.Click += new System.EventHandler(this.buttonIdService_Click);
            // 
            // FormStableIdentifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 435);
            this.Controls.Add(this.tableLayoutPanelAll);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormStableIdentifier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administrate stable identifier";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStableIdentifier_FormClosing);
            this.Load += new System.EventHandler(this.FormStableIdentifier_Load);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRcode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectProxyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetStableIdentifier)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelAll.ResumeLayout(false);
            this.tableLayoutPanelAll.PerformLayout();
            this.tableLayoutPanelIdservice.ResumeLayout(false);
            this.tableLayoutPanelIdservice.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TextBox textBoxBase;
        private System.Windows.Forms.Button buttonTest;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.PictureBox pictureBoxQRcode;
        private System.Windows.Forms.ListBox listBoxIdentifier;
        private System.Windows.Forms.Label labelBaseURL;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label labelProject;
        private Datasets.DataSetStableIdentifier dataSetStableIdentifier;
        private System.Windows.Forms.BindingSource projectProxyBindingSource;
        private Datasets.DataSetStableIdentifierTableAdapters.ProjectProxyTableAdapter projectProxyTableAdapter;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonCopyBasicUrlToAll;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonRemoveAll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAll;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonSetForDatabase;
        private System.Windows.Forms.Label labelProjects;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIdservice;
        private System.Windows.Forms.Label labelIdService;
        private System.Windows.Forms.Button buttonIdService;
    }
}