namespace DiversityWorkbench.Forms
{
    partial class FormEditXmlContent
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelContent = new System.Windows.Forms.Label();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.buttonMergeTemplates = new System.Windows.Forms.Button();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.treeView, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelContent, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxContent, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonMergeTemplates, 2, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(284, 291);
            this.tableLayoutPanelMain.TabIndex = 2;
            // 
            // treeView
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.treeView, 3);
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(3, 31);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(278, 191);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(86, 28);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Edit the template";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelContent.Location = new System.Drawing.Point(3, 231);
            this.labelContent.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(47, 60);
            this.labelContent.TabIndex = 4;
            this.labelContent.Text = "Content:";
            // 
            // textBoxContent
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxContent, 2);
            this.textBoxContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxContent.Location = new System.Drawing.Point(56, 228);
            this.textBoxContent.Multiline = true;
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(225, 60);
            this.textBoxContent.TabIndex = 5;
            this.textBoxContent.Text = "Content";
            this.textBoxContent.Leave += new System.EventHandler(this.textBoxContent_Leave);
            // 
            // buttonMergeTemplates
            // 
            this.buttonMergeTemplates.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonMergeTemplates.Location = new System.Drawing.Point(185, 3);
            this.buttonMergeTemplates.Name = "buttonMergeTemplates";
            this.buttonMergeTemplates.Size = new System.Drawing.Size(96, 22);
            this.buttonMergeTemplates.TabIndex = 6;
            this.buttonMergeTemplates.Text = "Get missing fields";
            this.buttonMergeTemplates.UseVisualStyleBackColor = true;
            this.buttonMergeTemplates.Visible = false;
            this.buttonMergeTemplates.Click += new System.EventHandler(this.buttonMergeTemplates_Click);
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 291);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(284, 27);
            this.userControlDialogPanel.TabIndex = 3;
            // 
            // FormEditXmlContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 318);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Name = "FormEditXmlContent";
            this.ShowIcon = false;
            this.Text = "FormEditXmlContent";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditXmlContent_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.Button buttonMergeTemplates;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}