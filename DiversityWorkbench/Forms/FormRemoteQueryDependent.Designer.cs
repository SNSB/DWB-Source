namespace DiversityWorkbench.Forms
{
    partial class FormRemoteQueryDependent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRemoteQueryDependent));
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.userControlQueryList = new DiversityWorkbench.UserControls.UserControlQueryList();
            this.panelUnit = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 407);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(499, 27);
            this.userControlDialogPanel.TabIndex = 0;
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
            this.splitContainerMain.Panel2.Controls.Add(this.panelUnit);
            this.splitContainerMain.Size = new System.Drawing.Size(499, 407);
            this.splitContainerMain.SplitterDistance = 166;
            this.splitContainerMain.TabIndex = 1;
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
            this.userControlQueryList.Size = new System.Drawing.Size(166, 407);
            this.userControlQueryList.TabIndex = 0;
            this.userControlQueryList.TableColors = null;
            this.userControlQueryList.TableImageIndex = null;
            this.userControlQueryList.WhereClause = null;
            // 
            // panelUnit
            // 
            this.panelUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelUnit.Location = new System.Drawing.Point(0, 0);
            this.panelUnit.Name = "panelUnit";
            this.panelUnit.Size = new System.Drawing.Size(329, 407);
            this.panelUnit.TabIndex = 0;
            // 
            // FormRemoteQueryDependent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 434);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRemoteQueryDependent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remote Query";
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private UserControls.UserControlQueryList userControlQueryList;
        private System.Windows.Forms.Panel panelUnit;
    }
}