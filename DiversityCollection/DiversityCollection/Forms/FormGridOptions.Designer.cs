namespace DiversityCollection.Forms
{
    partial class FormGridOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridOptions));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.dateTimePickerAnalysisEndDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerAnalysisStartDate = new System.Windows.Forms.DateTimePicker();
            this.listBoxAnalysisTypes = new System.Windows.Forms.ListBox();
            this.buttonAnalysisIDsAdd = new System.Windows.Forms.Button();
            this.buttonAnalysisIDsRemove = new System.Windows.Forms.Button();
            this.checkBoxUseAnalysisStartDate = new System.Windows.Forms.CheckBox();
            this.checkBoxUseAnalysisEndDate = new System.Windows.Forms.CheckBox();
            this.labelProject = new System.Windows.Forms.Label();
            this.textBoxProject = new System.Windows.Forms.TextBox();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerAnalysisEndDate, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerAnalysisStartDate, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxAnalysisTypes, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAnalysisIDsAdd, 3, 3);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAnalysisIDsRemove, 3, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxUseAnalysisStartDate, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxUseAnalysisEndDate, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelProject, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxProject, 1, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 5;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(284, 198);
            this.tableLayoutPanelMain.TabIndex = 3;
            // 
            // dateTimePickerAnalysisEndDate
            // 
            this.dateTimePickerAnalysisEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerAnalysisEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerAnalysisEndDate.Location = new System.Drawing.Point(144, 49);
            this.dateTimePickerAnalysisEndDate.Name = "dateTimePickerAnalysisEndDate";
            this.dateTimePickerAnalysisEndDate.Size = new System.Drawing.Size(107, 20);
            this.dateTimePickerAnalysisEndDate.TabIndex = 4;
            this.dateTimePickerAnalysisEndDate.ValueChanged += new System.EventHandler(this.dateTimePickerAnalysisEndDate_ValueChanged);
            // 
            // dateTimePickerAnalysisStartDate
            // 
            this.dateTimePickerAnalysisStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerAnalysisStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerAnalysisStartDate.Location = new System.Drawing.Point(144, 23);
            this.dateTimePickerAnalysisStartDate.Name = "dateTimePickerAnalysisStartDate";
            this.dateTimePickerAnalysisStartDate.Size = new System.Drawing.Size(107, 20);
            this.dateTimePickerAnalysisStartDate.TabIndex = 1;
            this.dateTimePickerAnalysisStartDate.ValueChanged += new System.EventHandler(this.dateTimePickerAnalysisStartDate_ValueChanged);
            // 
            // listBoxAnalysisTypes
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.listBoxAnalysisTypes, 3);
            this.listBoxAnalysisTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAnalysisTypes.FormattingEnabled = true;
            this.listBoxAnalysisTypes.IntegralHeight = false;
            this.listBoxAnalysisTypes.Location = new System.Drawing.Point(3, 75);
            this.listBoxAnalysisTypes.Name = "listBoxAnalysisTypes";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxAnalysisTypes, 2);
            this.listBoxAnalysisTypes.Size = new System.Drawing.Size(248, 120);
            this.listBoxAnalysisTypes.TabIndex = 8;
            // 
            // buttonAnalysisIDsAdd
            // 
            this.buttonAnalysisIDsAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonAnalysisIDsAdd.Image = global::DiversityCollection.Resource.Add1;
            this.buttonAnalysisIDsAdd.Location = new System.Drawing.Point(257, 75);
            this.buttonAnalysisIDsAdd.Name = "buttonAnalysisIDsAdd";
            this.buttonAnalysisIDsAdd.Size = new System.Drawing.Size(24, 25);
            this.buttonAnalysisIDsAdd.TabIndex = 9;
            this.buttonAnalysisIDsAdd.UseVisualStyleBackColor = true;
            this.buttonAnalysisIDsAdd.Click += new System.EventHandler(this.buttonAnalysisIDsAdd_Click);
            // 
            // buttonAnalysisIDsRemove
            // 
            this.buttonAnalysisIDsRemove.Image = global::DiversityCollection.Resource.Delete;
            this.buttonAnalysisIDsRemove.Location = new System.Drawing.Point(257, 172);
            this.buttonAnalysisIDsRemove.Name = "buttonAnalysisIDsRemove";
            this.buttonAnalysisIDsRemove.Size = new System.Drawing.Size(24, 23);
            this.buttonAnalysisIDsRemove.TabIndex = 10;
            this.buttonAnalysisIDsRemove.UseVisualStyleBackColor = true;
            this.buttonAnalysisIDsRemove.Click += new System.EventHandler(this.buttonAnalysisIDsRemove_Click);
            // 
            // checkBoxUseAnalysisStartDate
            // 
            this.checkBoxUseAnalysisStartDate.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxUseAnalysisStartDate, 2);
            this.checkBoxUseAnalysisStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxUseAnalysisStartDate.Location = new System.Drawing.Point(3, 23);
            this.checkBoxUseAnalysisStartDate.Name = "checkBoxUseAnalysisStartDate";
            this.checkBoxUseAnalysisStartDate.Size = new System.Drawing.Size(135, 20);
            this.checkBoxUseAnalysisStartDate.TabIndex = 11;
            this.checkBoxUseAnalysisStartDate.Text = "Use analysis start date:";
            this.checkBoxUseAnalysisStartDate.UseVisualStyleBackColor = true;
            this.checkBoxUseAnalysisStartDate.CheckedChanged += new System.EventHandler(this.checkBoxUseAnalysisStartDate_CheckedChanged);
            // 
            // checkBoxUseAnalysisEndDate
            // 
            this.checkBoxUseAnalysisEndDate.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxUseAnalysisEndDate, 2);
            this.checkBoxUseAnalysisEndDate.Location = new System.Drawing.Point(3, 49);
            this.checkBoxUseAnalysisEndDate.Name = "checkBoxUseAnalysisEndDate";
            this.checkBoxUseAnalysisEndDate.Size = new System.Drawing.Size(133, 17);
            this.checkBoxUseAnalysisEndDate.TabIndex = 12;
            this.checkBoxUseAnalysisEndDate.Text = "Use analysis end date:";
            this.checkBoxUseAnalysisEndDate.UseVisualStyleBackColor = true;
            this.checkBoxUseAnalysisEndDate.CheckedChanged += new System.EventHandler(this.checkBoxUseAnalysisEndDate_CheckedChanged);
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Location = new System.Drawing.Point(3, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(43, 20);
            this.labelProject.TabIndex = 13;
            this.labelProject.Text = "Project:";
            // 
            // textBoxProject
            // 
            this.textBoxProject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxProject, 3);
            this.textBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProject.Location = new System.Drawing.Point(52, 3);
            this.textBoxProject.Name = "textBoxProject";
            this.textBoxProject.ReadOnly = true;
            this.textBoxProject.Size = new System.Drawing.Size(229, 13);
            this.textBoxProject.TabIndex = 14;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 198);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(284, 27);
            this.userControlDialogPanel.TabIndex = 2;
            // 
            // FormGridOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 225);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormGridOptions";
            this.Text = "Set options";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.DateTimePicker dateTimePickerAnalysisEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerAnalysisStartDate;
        private System.Windows.Forms.ListBox listBoxAnalysisTypes;
        private System.Windows.Forms.Button buttonAnalysisIDsAdd;
        private System.Windows.Forms.Button buttonAnalysisIDsRemove;
        private System.Windows.Forms.CheckBox checkBoxUseAnalysisStartDate;
        private System.Windows.Forms.CheckBox checkBoxUseAnalysisEndDate;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.TextBox textBoxProject;
    }
}