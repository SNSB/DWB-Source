namespace DiversityCollection.Forms
{
    partial class FormGetPermit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGetPermit));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            this.userControlPermit = new DiversityCollection.UserControls.UserControlPermit();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
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
            this.splitContainerMain.Panel1.Controls.Add(this.userControlQueryList);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.userControlPermit);
            this.splitContainerMain.Size = new System.Drawing.Size(751, 488);
            this.splitContainerMain.SplitterDistance = 250;
            this.splitContainerMain.TabIndex = 0;
            // 
            // userControlQueryList
            // 
            this.userControlQueryList.Connection = null;
            this.userControlQueryList.DisplayTextSelectedItem = "";
            this.userControlQueryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlQueryList.IDisNumeric = true;
            this.userControlQueryList.ImageList = null;
            this.userControlQueryList.IsPredefinedQuery = false;
            this.userControlQueryList.Location = new System.Drawing.Point(0, 0);
            this.userControlQueryList.MaximalNumberOfResults = 100;
            this.userControlQueryList.Name = "userControlQueryList";
            this.userControlQueryList.ProjectID = -1;
            this.userControlQueryList.QueryConditionVisiblity = "";
            this.userControlQueryList.QueryDisplayColumns = null;
            this.userControlQueryList.QueryMainTableLocal = null;
            this.userControlQueryList.QueryRestriction = "";
            this.userControlQueryList.RememberQuerySettingsIdentifier = "QueryList";
            this.userControlQueryList.SelectedProjectID = null;
            this.userControlQueryList.Size = new System.Drawing.Size(250, 488);
            this.userControlQueryList.TabIndex = 0;
            this.userControlQueryList.TableColors = null;
            this.userControlQueryList.TableImageIndex = null;
            this.userControlQueryList.WhereClause = null;
            // 
            // userControlPermit
            // 
            this.userControlPermit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlPermit.Location = new System.Drawing.Point(0, 0);
            this.userControlPermit.Name = "userControlPermit";
            this.userControlPermit.Size = new System.Drawing.Size(497, 488);
            this.userControlPermit.TabIndex = 0;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 488);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(751, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormGetPermit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 515);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGetPermit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Get a permit";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryList;
        private UserControls.UserControlPermit userControlPermit;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
    }
}