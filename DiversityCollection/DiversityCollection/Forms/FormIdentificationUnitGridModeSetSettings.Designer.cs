namespace DiversityCollection.Forms
{
    partial class FormIdentificationUnitGridModeSetSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIdentificationUnitGridModeSetSettings));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.dateTimePickerAnalysisEndDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerAnalysisStartDate = new System.Windows.Forms.DateTimePicker();
            this.listBoxAnalysisTypes = new System.Windows.Forms.ListBox();
            this.buttonAnalysisIDsAdd = new System.Windows.Forms.Button();
            this.buttonAnalysisIDsRemove = new System.Windows.Forms.Button();
            this.checkBoxUseAnalysisStartDate = new System.Windows.Forms.CheckBox();
            this.checkBoxUseAnalysisEndDate = new System.Windows.Forms.CheckBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerAnalysisEndDate, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerAnalysisStartDate, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxAnalysisTypes, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAnalysisIDsAdd, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAnalysisIDsRemove, 2, 3);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxUseAnalysisStartDate, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxUseAnalysisEndDate, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(269, 168);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // dateTimePickerAnalysisEndDate
            // 
            this.dateTimePickerAnalysisEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerAnalysisEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerAnalysisEndDate.Location = new System.Drawing.Point(144, 29);
            this.dateTimePickerAnalysisEndDate.Name = "dateTimePickerAnalysisEndDate";
            this.dateTimePickerAnalysisEndDate.Size = new System.Drawing.Size(92, 20);
            this.dateTimePickerAnalysisEndDate.TabIndex = 4;
            // 
            // dateTimePickerAnalysisStartDate
            // 
            this.dateTimePickerAnalysisStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerAnalysisStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerAnalysisStartDate.Location = new System.Drawing.Point(144, 3);
            this.dateTimePickerAnalysisStartDate.Name = "dateTimePickerAnalysisStartDate";
            this.dateTimePickerAnalysisStartDate.Size = new System.Drawing.Size(92, 20);
            this.dateTimePickerAnalysisStartDate.TabIndex = 1;
            // 
            // listBoxAnalysisTypes
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.listBoxAnalysisTypes, 2);
            this.listBoxAnalysisTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAnalysisTypes.FormattingEnabled = true;
            this.listBoxAnalysisTypes.IntegralHeight = false;
            this.listBoxAnalysisTypes.Location = new System.Drawing.Point(3, 55);
            this.listBoxAnalysisTypes.Name = "listBoxAnalysisTypes";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxAnalysisTypes, 2);
            this.listBoxAnalysisTypes.Size = new System.Drawing.Size(233, 110);
            this.listBoxAnalysisTypes.TabIndex = 8;
            // 
            // buttonAnalysisIDsAdd
            // 
            this.buttonAnalysisIDsAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonAnalysisIDsAdd.Image = global::DiversityCollection.Resource.Add1;
            this.buttonAnalysisIDsAdd.Location = new System.Drawing.Point(242, 55);
            this.buttonAnalysisIDsAdd.Name = "buttonAnalysisIDsAdd";
            this.buttonAnalysisIDsAdd.Size = new System.Drawing.Size(24, 25);
            this.buttonAnalysisIDsAdd.TabIndex = 9;
            this.buttonAnalysisIDsAdd.UseVisualStyleBackColor = true;
            this.buttonAnalysisIDsAdd.Click += new System.EventHandler(this.buttonAnalysisIDsAdd_Click);
            // 
            // buttonAnalysisIDsRemove
            // 
            this.buttonAnalysisIDsRemove.Image = global::DiversityCollection.Resource.Delete;
            this.buttonAnalysisIDsRemove.Location = new System.Drawing.Point(242, 142);
            this.buttonAnalysisIDsRemove.Name = "buttonAnalysisIDsRemove";
            this.buttonAnalysisIDsRemove.Size = new System.Drawing.Size(24, 23);
            this.buttonAnalysisIDsRemove.TabIndex = 10;
            this.buttonAnalysisIDsRemove.UseVisualStyleBackColor = true;
            this.buttonAnalysisIDsRemove.Click += new System.EventHandler(this.buttonAnalysisIDsRemove_Click);
            // 
            // checkBoxUseAnalysisStartDate
            // 
            this.checkBoxUseAnalysisStartDate.AutoSize = true;
            this.checkBoxUseAnalysisStartDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxUseAnalysisStartDate.Location = new System.Drawing.Point(3, 3);
            this.checkBoxUseAnalysisStartDate.Name = "checkBoxUseAnalysisStartDate";
            this.checkBoxUseAnalysisStartDate.Size = new System.Drawing.Size(135, 20);
            this.checkBoxUseAnalysisStartDate.TabIndex = 11;
            this.checkBoxUseAnalysisStartDate.Text = "Use analysis start date:";
            this.checkBoxUseAnalysisStartDate.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseAnalysisEndDate
            // 
            this.checkBoxUseAnalysisEndDate.AutoSize = true;
            this.checkBoxUseAnalysisEndDate.Location = new System.Drawing.Point(3, 29);
            this.checkBoxUseAnalysisEndDate.Name = "checkBoxUseAnalysisEndDate";
            this.checkBoxUseAnalysisEndDate.Size = new System.Drawing.Size(133, 17);
            this.checkBoxUseAnalysisEndDate.TabIndex = 12;
            this.checkBoxUseAnalysisEndDate.Text = "Use analysis end date:";
            this.checkBoxUseAnalysisEndDate.UseVisualStyleBackColor = true;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 168);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(269, 27);
            this.userControlDialogPanel.TabIndex = 1;
            // 
            // FormIdentificationUnitGridModeSetSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 195);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormIdentificationUnitGridModeSetSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options for the analysis";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormIdentificationUnitGridModeSetSettings_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.DateTimePicker dateTimePickerAnalysisEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerAnalysisStartDate;
        private System.Windows.Forms.ListBox listBoxAnalysisTypes;
        private System.Windows.Forms.Button buttonAnalysisIDsAdd;
        private System.Windows.Forms.Button buttonAnalysisIDsRemove;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkBoxUseAnalysisStartDate;
        private System.Windows.Forms.CheckBox checkBoxUseAnalysisEndDate;
    }
}