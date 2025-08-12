namespace DiversityWorkbench.Export
{
    partial class FormTranslationSource
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTranslationSource));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelSource = new System.Windows.Forms.Label();
            this.radioButtonGetDataFromDatabase = new System.Windows.Forms.RadioButton();
            this.radioButtonGetDataFromProject = new System.Windows.Forms.RadioButton();
            this.radioButtonGetDataFromQuery = new System.Windows.Forms.RadioButton();
            this.buttonTest = new System.Windows.Forms.Button();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.listBoxTest = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 183F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.labelSource, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonGetDataFromDatabase, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonGetDataFromProject, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonGetDataFromQuery, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonTest, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.userControlDialogPanel, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxTest, 1, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 5;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(458, 331);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelSource
            // 
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.Location = new System.Drawing.Point(3, 0);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(177, 29);
            this.labelSource.TabIndex = 3;
            this.labelSource.Text = "Please decide where the data for the translation should be taken from";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButtonGetDataFromDatabase
            // 
            this.radioButtonGetDataFromDatabase.AutoSize = true;
            this.radioButtonGetDataFromDatabase.Location = new System.Drawing.Point(3, 78);
            this.radioButtonGetDataFromDatabase.Name = "radioButtonGetDataFromDatabase";
            this.radioButtonGetDataFromDatabase.Size = new System.Drawing.Size(167, 17);
            this.radioButtonGetDataFromDatabase.TabIndex = 2;
            this.radioButtonGetDataFromDatabase.Text = "Get data from whole database";
            this.radioButtonGetDataFromDatabase.UseVisualStyleBackColor = true;
            // 
            // radioButtonGetDataFromProject
            // 
            this.radioButtonGetDataFromProject.AutoSize = true;
            this.radioButtonGetDataFromProject.Location = new System.Drawing.Point(3, 55);
            this.radioButtonGetDataFromProject.Name = "radioButtonGetDataFromProject";
            this.radioButtonGetDataFromProject.Size = new System.Drawing.Size(124, 17);
            this.radioButtonGetDataFromProject.TabIndex = 1;
            this.radioButtonGetDataFromProject.Text = "Get data from project";
            this.radioButtonGetDataFromProject.UseVisualStyleBackColor = true;
            // 
            // radioButtonGetDataFromQuery
            // 
            this.radioButtonGetDataFromQuery.AutoSize = true;
            this.radioButtonGetDataFromQuery.Checked = true;
            this.radioButtonGetDataFromQuery.Location = new System.Drawing.Point(3, 32);
            this.radioButtonGetDataFromQuery.Name = "radioButtonGetDataFromQuery";
            this.radioButtonGetDataFromQuery.Size = new System.Drawing.Size(118, 17);
            this.radioButtonGetDataFromQuery.TabIndex = 0;
            this.radioButtonGetDataFromQuery.TabStop = true;
            this.radioButtonGetDataFromQuery.Text = "Get data from query";
            this.radioButtonGetDataFromQuery.UseVisualStyleBackColor = true;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(186, 3);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 4;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // userControlDialogPanel
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.userControlDialogPanel, 2);
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDialogPanel.Location = new System.Drawing.Point(3, 303);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(452, 25);
            this.userControlDialogPanel.TabIndex = 5;
            // 
            // listBoxTest
            // 
            this.listBoxTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTest.FormattingEnabled = true;
            this.listBoxTest.Location = new System.Drawing.Point(186, 32);
            this.listBoxTest.Name = "listBoxTest";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxTest, 3);
            this.listBoxTest.Size = new System.Drawing.Size(269, 265);
            this.listBoxTest.TabIndex = 6;
            // 
            // FormTranslationSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 331);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTranslationSource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Translation source";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.RadioButton radioButtonGetDataFromDatabase;
        private System.Windows.Forms.RadioButton radioButtonGetDataFromProject;
        private System.Windows.Forms.RadioButton radioButtonGetDataFromQuery;
        private System.Windows.Forms.Button buttonTest;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ListBox listBoxTest;
    }
}