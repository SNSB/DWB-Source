namespace DiversityWorkbench.Forms
{
    partial class FormConnectionAdministration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConnectionAdministration));
            this.treeViewConnections = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonLinkedServer = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.buttonLoadAdded = new System.Windows.Forms.Button();
            this.buttonRequery = new System.Windows.Forms.Button();
            this.buttonUpdateBacklinkModule = new System.Windows.Forms.Button();
            this.contextMenuStripBacklinkUpdates = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.contextMenuStripBacklinkUpdates.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewConnections
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.treeViewConnections, 10);
            resources.ApplyResources(this.treeViewConnections, "treeViewConnections");
            this.treeViewConnections.ImageList = this.imageList;
            this.treeViewConnections.Name = "treeViewConnections";
            this.treeViewConnections.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewConnections_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "DiversityWorkbench16.ico");
            this.imageList.Images.SetKeyName(1, "Database.ico");
            this.imageList.Images.SetKeyName(2, "Browse.ico");
            this.imageList.Images.SetKeyName(3, "ServerLinked.ico");
            this.imageList.Images.SetKeyName(4, "DatabaseGrey.ico");
            this.imageList.Images.SetKeyName(5, "Server2.ico");
            this.imageList.Images.SetKeyName(6, "Agent.ico");
            this.imageList.Images.SetKeyName(7, "Link.ico");
            this.imageList.Images.SetKeyName(8, "CacheDB.ico");
            // 
            // tableLayoutPanelMain
            // 
            resources.ApplyResources(this.tableLayoutPanelMain, "tableLayoutPanelMain");
            this.tableLayoutPanelMain.Controls.Add(this.treeViewConnections, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonConnect, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRemove, 7, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReset, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAdd, 6, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonLinkedServer, 8, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 9, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonLoadAdded, 5, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRequery, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUpdateBacklinkModule, 4, 1);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, ((bool)(resources.GetObject("tableLayoutPanelMain.ShowHelp"))));
            // 
            // labelHeader
            // 
            resources.ApplyResources(this.labelHeader, "labelHeader");
            this.labelHeader.Name = "labelHeader";
            this.tableLayoutPanelMain.SetRowSpan(this.labelHeader, 2);
            this.helpProvider.SetShowHelp(this.labelHeader, ((bool)(resources.GetObject("labelHeader.ShowHelp"))));
            // 
            // buttonConnect
            // 
            resources.ApplyResources(this.buttonConnect, "buttonConnect");
            this.buttonConnect.FlatAppearance.BorderSize = 0;
            this.buttonConnect.Image = global::DiversityWorkbench.ResourceWorkbench.Database;
            this.buttonConnect.Name = "buttonConnect";
            this.helpProvider.SetShowHelp(this.buttonConnect, ((bool)(resources.GetObject("buttonConnect.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonConnect, resources.GetString("buttonConnect.ToolTip"));
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonRemove
            // 
            resources.ApplyResources(this.buttonRemove, "buttonRemove");
            this.buttonRemove.FlatAppearance.BorderSize = 0;
            this.buttonRemove.Image = global::DiversityWorkbench.ResourceWorkbench.Delete;
            this.buttonRemove.Name = "buttonRemove";
            this.helpProvider.SetShowHelp(this.buttonRemove, ((bool)(resources.GetObject("buttonRemove.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonRemove, resources.GetString("buttonRemove.ToolTip"));
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonReset
            // 
            resources.ApplyResources(this.buttonReset, "buttonReset");
            this.buttonReset.FlatAppearance.BorderSize = 0;
            this.buttonReset.Image = global::DiversityWorkbench.ResourceWorkbench.Undo;
            this.buttonReset.Name = "buttonReset";
            this.helpProvider.SetShowHelp(this.buttonReset, ((bool)(resources.GetObject("buttonReset.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonReset, resources.GetString("buttonReset.ToolTip"));
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonAdd
            // 
            resources.ApplyResources(this.buttonAdd, "buttonAdd");
            this.buttonAdd.FlatAppearance.BorderSize = 0;
            this.buttonAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAdd.Name = "buttonAdd";
            this.toolTip.SetToolTip(this.buttonAdd, resources.GetString("buttonAdd.ToolTip"));
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonLinkedServer
            // 
            resources.ApplyResources(this.buttonLinkedServer, "buttonLinkedServer");
            this.buttonLinkedServer.FlatAppearance.BorderSize = 0;
            this.buttonLinkedServer.Image = global::DiversityWorkbench.Properties.Resources.ServerLinked;
            this.buttonLinkedServer.Name = "buttonLinkedServer";
            this.toolTip.SetToolTip(this.buttonLinkedServer, resources.GetString("buttonLinkedServer.ToolTip"));
            this.buttonLinkedServer.UseVisualStyleBackColor = true;
            this.buttonLinkedServer.Click += new System.EventHandler(this.buttonLinkedServer_Click);
            // 
            // buttonFeedback
            // 
            resources.ApplyResources(this.buttonFeedback, "buttonFeedback");
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Name = "buttonFeedback";
            this.toolTip.SetToolTip(this.buttonFeedback, resources.GetString("buttonFeedback.ToolTip"));
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // buttonLoadAdded
            // 
            resources.ApplyResources(this.buttonLoadAdded, "buttonLoadAdded");
            this.buttonLoadAdded.FlatAppearance.BorderSize = 0;
            this.buttonLoadAdded.Image = global::DiversityWorkbench.Properties.Resources.LoadDataSource;
            this.buttonLoadAdded.Name = "buttonLoadAdded";
            this.helpProvider.SetShowHelp(this.buttonLoadAdded, ((bool)(resources.GetObject("buttonLoadAdded.ShowHelp"))));
            this.toolTip.SetToolTip(this.buttonLoadAdded, resources.GetString("buttonLoadAdded.ToolTip"));
            this.buttonLoadAdded.UseVisualStyleBackColor = true;
            this.buttonLoadAdded.Click += new System.EventHandler(this.buttonLoadAdded_Click);
            // 
            // buttonRequery
            // 
            resources.ApplyResources(this.buttonRequery, "buttonRequery");
            this.buttonRequery.FlatAppearance.BorderSize = 0;
            this.buttonRequery.Image = global::DiversityWorkbench.Properties.Resources.Transfrom;
            this.buttonRequery.Name = "buttonRequery";
            this.toolTip.SetToolTip(this.buttonRequery, resources.GetString("buttonRequery.ToolTip"));
            this.buttonRequery.UseVisualStyleBackColor = true;
            this.buttonRequery.Click += new System.EventHandler(this.buttonRequery_Click);
            // 
            // buttonUpdateBacklinkModule
            // 
            this.buttonUpdateBacklinkModule.ContextMenuStrip = this.contextMenuStripBacklinkUpdates;
            resources.ApplyResources(this.buttonUpdateBacklinkModule, "buttonUpdateBacklinkModule");
            this.buttonUpdateBacklinkModule.FlatAppearance.BorderSize = 0;
            this.buttonUpdateBacklinkModule.Image = global::DiversityWorkbench.Properties.Resources.DatabaseUpdate;
            this.buttonUpdateBacklinkModule.Name = "buttonUpdateBacklinkModule";
            this.toolTip.SetToolTip(this.buttonUpdateBacklinkModule, resources.GetString("buttonUpdateBacklinkModule.ToolTip"));
            this.buttonUpdateBacklinkModule.UseVisualStyleBackColor = true;
            this.buttonUpdateBacklinkModule.Click += new System.EventHandler(this.buttonUpdateBacklinkModule_Click);
            // 
            // contextMenuStripBacklinkUpdates
            // 
            this.contextMenuStripBacklinkUpdates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showSettingsToolStripMenuItem,
            this.resetSettingsToolStripMenuItem});
            this.contextMenuStripBacklinkUpdates.Name = "contextMenuStripBacklinkUpdates";
            resources.ApplyResources(this.contextMenuStripBacklinkUpdates, "contextMenuStripBacklinkUpdates");
            // 
            // showSettingsToolStripMenuItem
            // 
            this.showSettingsToolStripMenuItem.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.showSettingsToolStripMenuItem.Name = "showSettingsToolStripMenuItem";
            resources.ApplyResources(this.showSettingsToolStripMenuItem, "showSettingsToolStripMenuItem");
            this.showSettingsToolStripMenuItem.Click += new System.EventHandler(this.showSettingsToolStripMenuItem_Click);
            // 
            // resetSettingsToolStripMenuItem
            // 
            this.resetSettingsToolStripMenuItem.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.resetSettingsToolStripMenuItem.Name = "resetSettingsToolStripMenuItem";
            resources.ApplyResources(this.resetSettingsToolStripMenuItem, "resetSettingsToolStripMenuItem");
            this.resetSettingsToolStripMenuItem.Click += new System.EventHandler(this.resetSettingsToolStripMenuItem_Click);
            // 
            // FormConnectionAdministration
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.helpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.helpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.Name = "FormConnectionAdministration";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConnectionAdministration_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.contextMenuStripBacklinkUpdates.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewConnections;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonLinkedServer;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonLoadAdded;
        private System.Windows.Forms.Button buttonRequery;
        private System.Windows.Forms.Button buttonUpdateBacklinkModule;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBacklinkUpdates;
        private System.Windows.Forms.ToolStripMenuItem showSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetSettingsToolStripMenuItem;
    }
}