namespace DiversityWorkbench.Forms
{
    partial class FormQueryLoad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQueryLoad));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelQuery = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxQuery = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelTable = new System.Windows.Forms.Label();
            this.labelQueryTable = new System.Windows.Forms.Label();
            this.textBoxSQL = new System.Windows.Forms.TextBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.tableLayoutPanel.SetColumnSpan(this.treeView, 2);
            resources.ApplyResources(this.treeView, "treeView");
            this.treeView.ImageList = this.imageListTreeView;
            this.treeView.Name = "treeView";
            this.helpProvider.SetShowHelp(this.treeView, ((bool)(resources.GetObject("treeView.ShowHelp"))));
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "FilterGroup.ico");
            this.imageListTreeView.Images.SetKeyName(1, "Filter.ico");
            this.imageListTreeView.Images.SetKeyName(2, "FilterOptimized.ico");
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.treeView, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelQuery, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelDescription, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxQuery, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelTable, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelQueryTable, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxSQL, 0, 4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.helpProvider.SetShowHelp(this.tableLayoutPanel, ((bool)(resources.GetObject("tableLayoutPanel.ShowHelp"))));
            // 
            // labelQuery
            // 
            resources.ApplyResources(this.labelQuery, "labelQuery");
            this.labelQuery.Name = "labelQuery";
            this.helpProvider.SetShowHelp(this.labelQuery, ((bool)(resources.GetObject("labelQuery.ShowHelp"))));
            // 
            // labelDescription
            // 
            resources.ApplyResources(this.labelDescription, "labelDescription");
            this.labelDescription.Name = "labelDescription";
            this.helpProvider.SetShowHelp(this.labelDescription, ((bool)(resources.GetObject("labelDescription.ShowHelp"))));
            // 
            // textBoxQuery
            // 
            resources.ApplyResources(this.textBoxQuery, "textBoxQuery");
            this.textBoxQuery.Name = "textBoxQuery";
            this.textBoxQuery.ReadOnly = true;
            this.helpProvider.SetShowHelp(this.textBoxQuery, ((bool)(resources.GetObject("textBoxQuery.ShowHelp"))));
            // 
            // textBoxDescription
            // 
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.helpProvider.SetShowHelp(this.textBoxDescription, ((bool)(resources.GetObject("textBoxDescription.ShowHelp"))));
            // 
            // labelTable
            // 
            resources.ApplyResources(this.labelTable, "labelTable");
            this.labelTable.Name = "labelTable";
            this.helpProvider.SetShowHelp(this.labelTable, ((bool)(resources.GetObject("labelTable.ShowHelp"))));
            // 
            // labelQueryTable
            // 
            resources.ApplyResources(this.labelQueryTable, "labelQueryTable");
            this.labelQueryTable.Name = "labelQueryTable";
            this.helpProvider.SetShowHelp(this.labelQueryTable, ((bool)(resources.GetObject("labelQueryTable.ShowHelp"))));
            // 
            // textBoxSQL
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxSQL, 2);
            resources.ApplyResources(this.textBoxSQL, "textBoxSQL");
            this.textBoxSQL.Name = "textBoxSQL";
            this.textBoxSQL.ReadOnly = true;
            this.helpProvider.SetShowHelp(this.textBoxSQL, ((bool)(resources.GetObject("textBoxSQL.ShowHelp"))));
            // 
            // userControlDialogPanel
            // 
            resources.ApplyResources(this.userControlDialogPanel, "userControlDialogPanel");
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.helpProvider.SetShowHelp(this.userControlDialogPanel, ((bool)(resources.GetObject("userControlDialogPanel.ShowHelp"))));
            // 
            // FormQueryLoad
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel);
            this.Name = "FormQueryLoad";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelQuery;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxQuery;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Label labelQueryTable;
        private System.Windows.Forms.TextBox textBoxSQL;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ImageList imageListTreeView;
        private System.Windows.Forms.ToolTip toolTip;
    }
}