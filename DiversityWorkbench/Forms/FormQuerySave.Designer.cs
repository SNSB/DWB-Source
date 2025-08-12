namespace DiversityWorkbench.Forms
{
    partial class FormQuerySave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuerySave));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageListTreeNodes = new System.Windows.Forms.ImageList(this.components);
            this.labelQuery = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxQuery = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelQueryTable = new System.Windows.Forms.Label();
            this.labelTable = new System.Windows.Forms.Label();
            this.buttonSaveQuery = new System.Windows.Forms.Button();
            this.labelGroup = new System.Windows.Forms.Label();
            this.buttonSaveGroup = new System.Windows.Forms.Button();
            this.textBoxGroup = new System.Windows.Forms.TextBox();
            this.textBoxSQL = new System.Windows.Forms.TextBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonQueryGroupNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.treeView, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelQuery, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelDescription, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.textBoxQuery, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonCancel, 0, 7);
            this.tableLayoutPanel.Controls.Add(this.buttonOK, 1, 7);
            this.tableLayoutPanel.Controls.Add(this.labelQueryTable, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.labelTable, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.buttonSaveQuery, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.labelGroup, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonSaveGroup, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxGroup, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxSQL, 0, 6);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // treeView
            // 
            this.tableLayoutPanel.SetColumnSpan(this.treeView, 3);
            resources.ApplyResources(this.treeView, "treeView");
            this.treeView.ImageList = this.imageListTreeNodes;
            this.treeView.Name = "treeView";
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageListTreeNodes
            // 
            this.imageListTreeNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeNodes.ImageStream")));
            this.imageListTreeNodes.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeNodes.Images.SetKeyName(0, "FilterGroup.ico");
            this.imageListTreeNodes.Images.SetKeyName(1, "Filter.ico");
            this.imageListTreeNodes.Images.SetKeyName(2, "FilterOptimized.ico");
            // 
            // labelQuery
            // 
            resources.ApplyResources(this.labelQuery, "labelQuery");
            this.labelQuery.Name = "labelQuery";
            // 
            // labelDescription
            // 
            resources.ApplyResources(this.labelDescription, "labelDescription");
            this.labelDescription.Name = "labelDescription";
            // 
            // textBoxQuery
            // 
            resources.ApplyResources(this.textBoxQuery, "textBoxQuery");
            this.textBoxQuery.Name = "textBoxQuery";
            this.toolTip.SetToolTip(this.textBoxQuery, resources.GetString("textBoxQuery.ToolTip"));
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxDescription, 2);
            resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
            this.textBoxDescription.Name = "textBoxDescription";
            this.toolTip.SetToolTip(this.textBoxDescription, resources.GetString("textBoxDescription.ToolTip"));
            // 
            // labelHeader
            // 
            resources.ApplyResources(this.labelHeader, "labelHeader");
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Name = "labelHeader";
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonOK, 2);
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelQueryTable
            // 
            resources.ApplyResources(this.labelQueryTable, "labelQueryTable");
            this.tableLayoutPanel.SetColumnSpan(this.labelQueryTable, 2);
            this.labelQueryTable.Name = "labelQueryTable";
            // 
            // labelTable
            // 
            resources.ApplyResources(this.labelTable, "labelTable");
            this.labelTable.Name = "labelTable";
            // 
            // buttonSaveQuery
            // 
            resources.ApplyResources(this.buttonSaveQuery, "buttonSaveQuery");
            this.buttonSaveQuery.FlatAppearance.BorderSize = 0;
            this.buttonSaveQuery.Image = global::DiversityWorkbench.Properties.Resources.Save;
            this.buttonSaveQuery.Name = "buttonSaveQuery";
            this.toolTip.SetToolTip(this.buttonSaveQuery, resources.GetString("buttonSaveQuery.ToolTip"));
            this.buttonSaveQuery.UseVisualStyleBackColor = true;
            this.buttonSaveQuery.Click += new System.EventHandler(this.buttonSaveQuery_Click);
            // 
            // labelGroup
            // 
            resources.ApplyResources(this.labelGroup, "labelGroup");
            this.labelGroup.Name = "labelGroup";
            // 
            // buttonSaveGroup
            // 
            resources.ApplyResources(this.buttonSaveGroup, "buttonSaveGroup");
            this.buttonSaveGroup.FlatAppearance.BorderSize = 0;
            this.buttonSaveGroup.Image = global::DiversityWorkbench.Properties.Resources.Save;
            this.buttonSaveGroup.Name = "buttonSaveGroup";
            this.toolTip.SetToolTip(this.buttonSaveGroup, resources.GetString("buttonSaveGroup.ToolTip"));
            this.buttonSaveGroup.UseVisualStyleBackColor = true;
            this.buttonSaveGroup.Click += new System.EventHandler(this.buttonSaveGroup_Click);
            // 
            // textBoxGroup
            // 
            resources.ApplyResources(this.textBoxGroup, "textBoxGroup");
            this.textBoxGroup.Name = "textBoxGroup";
            // 
            // textBoxSQL
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxSQL, 3);
            resources.ApplyResources(this.textBoxSQL, "textBoxSQL");
            this.textBoxSQL.Name = "textBoxSQL";
            this.textBoxSQL.ReadOnly = true;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonQueryGroupNew,
            this.toolStripButtonDelete});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // toolStripButtonQueryGroupNew
            // 
            resources.ApplyResources(this.toolStripButtonQueryGroupNew, "toolStripButtonQueryGroupNew");
            this.toolStripButtonQueryGroupNew.ForeColor = System.Drawing.Color.Gray;
            this.toolStripButtonQueryGroupNew.Image = global::DiversityWorkbench.ResourceWorkbench.FilterGroup;
            this.toolStripButtonQueryGroupNew.Name = "toolStripButtonQueryGroupNew";
            this.toolStripButtonQueryGroupNew.Click += new System.EventHandler(this.toolStripButtonQueryGroupNew_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityWorkbench.ResourceWorkbench.Delete;
            resources.ApplyResources(this.toolStripButtonDelete, "toolStripButtonDelete");
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // FormQuerySave
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.toolStrip);
            this.helpProvider.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.helpProvider.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.helpProvider.SetHelpString(this, resources.GetString("$this.HelpString"));
            this.Name = "FormQuerySave";
            this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label labelQuery;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxQuery;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonQueryGroupNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelQueryTable;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Button buttonSaveQuery;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.Button buttonSaveGroup;
        private System.Windows.Forms.TextBox textBoxGroup;
        private System.Windows.Forms.TextBox textBoxSQL;
        private System.Windows.Forms.ImageList imageListTreeNodes;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}