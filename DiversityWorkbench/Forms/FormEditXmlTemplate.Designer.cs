namespace DiversityWorkbench.Forms
{
    partial class FormEditXmlTemplate
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.buttonAddNode = new System.Windows.Forms.Button();
            this.buttonRemoveNode = new System.Windows.Forms.Button();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonSearchTemplate = new System.Windows.Forms.Button();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.treeView, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAddNode, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRemoveNode, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonSearchTemplate, 1, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(284, 235);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // treeView
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.treeView, 2);
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(3, 31);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(278, 170);
            this.treeView.TabIndex = 0;
            // 
            // buttonAddNode
            // 
            this.buttonAddNode.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonAddNode.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAddNode.Location = new System.Drawing.Point(3, 207);
            this.buttonAddNode.Name = "buttonAddNode";
            this.buttonAddNode.Size = new System.Drawing.Size(25, 25);
            this.buttonAddNode.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonAddNode, "Add a new node");
            this.buttonAddNode.UseVisualStyleBackColor = true;
            this.buttonAddNode.Click += new System.EventHandler(this.buttonAddNode_Click);
            // 
            // buttonRemoveNode
            // 
            this.buttonRemoveNode.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonRemoveNode.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemoveNode.Location = new System.Drawing.Point(256, 207);
            this.buttonRemoveNode.Name = "buttonRemoveNode";
            this.buttonRemoveNode.Size = new System.Drawing.Size(25, 25);
            this.buttonRemoveNode.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonRemoveNode, "Remove the selected node");
            this.buttonRemoveNode.UseVisualStyleBackColor = true;
            this.buttonRemoveNode.Click += new System.EventHandler(this.buttonRemoveNode_Click);
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(136, 28);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Edit the template";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSearchTemplate
            // 
            this.buttonSearchTemplate.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSearchTemplate.Location = new System.Drawing.Point(187, 3);
            this.buttonSearchTemplate.Name = "buttonSearchTemplate";
            this.buttonSearchTemplate.Size = new System.Drawing.Size(94, 22);
            this.buttonSearchTemplate.TabIndex = 4;
            this.buttonSearchTemplate.Text = "Search template";
            this.toolTip.SetToolTip(this.buttonSearchTemplate, "Search a template from other projects");
            this.buttonSearchTemplate.UseVisualStyleBackColor = true;
            this.buttonSearchTemplate.Visible = false;
            this.buttonSearchTemplate.Click += new System.EventHandler(this.buttonSearchTemplate_Click);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 235);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(284, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormEditXmlTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Name = "FormEditXmlTemplate";
            this.ShowIcon = false;
            this.Text = "Edit XML template";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditXmlTemplate_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button buttonAddNode;
        private System.Windows.Forms.Button buttonRemoveNode;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelHeader;
        public System.Windows.Forms.Button buttonSearchTemplate;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}