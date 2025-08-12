namespace DiversityWorkbench.UserControls
{
    partial class UserControlDatePanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelDate = new System.Windows.Forms.TableLayoutPanel();
            this.maskedTextBoxYear = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxMonth = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxDay = new System.Windows.Forms.MaskedTextBox();
            this.textBoxSupplement = new System.Windows.Forms.TextBox();
            this.labelSupplement = new System.Windows.Forms.Label();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanelDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelDate
            // 
            this.tableLayoutPanelDate.ColumnCount = 6;
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDate.Controls.Add(this.maskedTextBoxYear, 0, 0);
            this.tableLayoutPanelDate.Controls.Add(this.maskedTextBoxMonth, 0, 0);
            this.tableLayoutPanelDate.Controls.Add(this.maskedTextBoxDay, 0, 0);
            this.tableLayoutPanelDate.Controls.Add(this.textBoxSupplement, 5, 0);
            this.tableLayoutPanelDate.Controls.Add(this.labelSupplement, 4, 0);
            this.tableLayoutPanelDate.Controls.Add(this.dateTimePickerDate, 3, 0);
            this.tableLayoutPanelDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDate.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDate.Name = "tableLayoutPanelDate";
            this.tableLayoutPanelDate.RowCount = 1;
            this.tableLayoutPanelDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDate.Size = new System.Drawing.Size(261, 21);
            this.tableLayoutPanelDate.TabIndex = 0;
            // 
            // maskedTextBoxYear
            // 
            this.maskedTextBoxYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxYear.Location = new System.Drawing.Point(40, 0);
            this.maskedTextBoxYear.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxYear.Mask = "9999";
            this.maskedTextBoxYear.Name = "maskedTextBoxYear";
            this.maskedTextBoxYear.Size = new System.Drawing.Size(34, 20);
            this.maskedTextBoxYear.TabIndex = 13;
            this.maskedTextBoxYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxYear.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBoxYear_Validating);
            // 
            // maskedTextBoxMonth
            // 
            this.maskedTextBoxMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxMonth.Location = new System.Drawing.Point(20, 0);
            this.maskedTextBoxMonth.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxMonth.Mask = "99";
            this.maskedTextBoxMonth.Name = "maskedTextBoxMonth";
            this.maskedTextBoxMonth.Size = new System.Drawing.Size(20, 20);
            this.maskedTextBoxMonth.TabIndex = 12;
            this.maskedTextBoxMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxMonth.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBoxMonth_Validating);
            // 
            // maskedTextBoxDay
            // 
            this.maskedTextBoxDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxDay.Location = new System.Drawing.Point(0, 0);
            this.maskedTextBoxDay.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxDay.Mask = "99";
            this.maskedTextBoxDay.Name = "maskedTextBoxDay";
            this.maskedTextBoxDay.Size = new System.Drawing.Size(20, 20);
            this.maskedTextBoxDay.TabIndex = 11;
            this.maskedTextBoxDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxDay.Validating += new System.ComponentModel.CancelEventHandler(this.maskedTextBoxDay_Validating);
            // 
            // textBoxSupplement
            // 
            this.textBoxSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSupplement.Location = new System.Drawing.Point(136, 0);
            this.textBoxSupplement.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxSupplement.Name = "textBoxSupplement";
            this.textBoxSupplement.Size = new System.Drawing.Size(125, 20);
            this.textBoxSupplement.TabIndex = 14;
            // 
            // labelSupplement
            // 
            this.labelSupplement.AutoSize = true;
            this.labelSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSupplement.Location = new System.Drawing.Point(93, 0);
            this.labelSupplement.Name = "labelSupplement";
            this.labelSupplement.Size = new System.Drawing.Size(40, 21);
            this.labelSupplement.TabIndex = 15;
            this.labelSupplement.Text = "Suppl.:";
            this.labelSupplement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerDate.Location = new System.Drawing.Point(74, 0);
            this.dateTimePickerDate.Margin = new System.Windows.Forms.Padding(0);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(16, 20);
            this.dateTimePickerDate.TabIndex = 16;
            this.dateTimePickerDate.CloseUp += new System.EventHandler(this.dateTimePickerDate_CloseUp);
            // 
            // UserControlDatePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelDate);
            this.Name = "UserControlDatePanel";
            this.Size = new System.Drawing.Size(261, 21);
            this.tableLayoutPanelDate.ResumeLayout(false);
            this.tableLayoutPanelDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDate;
        private System.Windows.Forms.Label labelSupplement;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxDay;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxMonth;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxYear;
        public System.Windows.Forms.TextBox textBoxSupplement;
    }
}
