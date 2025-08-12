namespace DiversityWorkbench.Forms
{
    partial class FormSelectXmlTemplate
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.listBoxSourceList = new System.Windows.Forms.ListBox();
            this.treeView = new System.Windows.Forms.TreeView();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.listBoxSourceList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.treeView);
            this.splitContainerMain.Size = new System.Drawing.Size(323, 236);
            this.splitContainerMain.SplitterDistance = 107;
            this.splitContainerMain.TabIndex = 1;
            // 
            // listBoxSourceList
            // 
            this.listBoxSourceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSourceList.FormattingEnabled = true;
            this.listBoxSourceList.IntegralHeight = false;
            this.listBoxSourceList.Location = new System.Drawing.Point(0, 0);
            this.listBoxSourceList.Name = "listBoxSourceList";
            this.listBoxSourceList.Size = new System.Drawing.Size(107, 236);
            this.listBoxSourceList.TabIndex = 0;
            this.listBoxSourceList.SelectedIndexChanged += new System.EventHandler(this.listBoxSourceList_SelectedIndexChanged);
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(212, 236);
            this.treeView.TabIndex = 0;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 236);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(323, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // FormSelectXmlTemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 263);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Name = "FormSelectXmlTemplate";
            this.ShowIcon = false;
            this.Text = "Select XML template";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ListBox listBoxSourceList;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}