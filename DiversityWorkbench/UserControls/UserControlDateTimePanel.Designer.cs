namespace DiversityWorkbench.UserControls
{
    partial class UserControlDateTimePanel
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelDate = new System.Windows.Forms.TableLayoutPanel();
            this.maskedTextBoxDate = new System.Windows.Forms.MaskedTextBox();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTime = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanelDate.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelDate
            // 
            this.tableLayoutPanelDate.ColumnCount = 6;
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDate.Controls.Add(this.maskedTextBoxDate, 0, 0);
            this.tableLayoutPanelDate.Controls.Add(this.dateTimePickerDate, 1, 0);
            this.tableLayoutPanelDate.Controls.Add(this.dateTimePickerTime, 4, 0);
            this.tableLayoutPanelDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelDate.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDate.Name = "tableLayoutPanelDate";
            this.tableLayoutPanelDate.RowCount = 1;
            this.tableLayoutPanelDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDate.Size = new System.Drawing.Size(154, 21);
            this.tableLayoutPanelDate.TabIndex = 1;
            // 
            // maskedTextBoxDate
            // 
            this.maskedTextBoxDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxDate.Location = new System.Drawing.Point(0, 0);
            this.maskedTextBoxDate.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxDate.Mask = "00/00/0000";
            this.maskedTextBoxDate.Name = "maskedTextBoxDate";
            this.maskedTextBoxDate.Size = new System.Drawing.Size(62, 20);
            this.maskedTextBoxDate.TabIndex = 13;
            this.maskedTextBoxDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maskedTextBoxDate.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.maskedTextBoxDate.ValidatingType = typeof(System.DateTime);
            this.maskedTextBoxDate.TextChanged += new System.EventHandler(this.maskedTextBoxDate_TextChanged);
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerDate.Location = new System.Drawing.Point(62, 0);
            this.dateTimePickerDate.Margin = new System.Windows.Forms.Padding(0);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(16, 20);
            this.dateTimePickerDate.TabIndex = 16;
            this.dateTimePickerDate.CloseUp += new System.EventHandler(this.dateTimePickerDate_ValueChanged);
            // 
            // dateTimePickerTime
            // 
            this.dateTimePickerTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerTime.Location = new System.Drawing.Point(88, 0);
            this.dateTimePickerTime.Margin = new System.Windows.Forms.Padding(0);
            this.dateTimePickerTime.Name = "dateTimePickerTime";
            this.dateTimePickerTime.ShowUpDown = true;
            this.dateTimePickerTime.Size = new System.Drawing.Size(65, 20);
            this.dateTimePickerTime.TabIndex = 18;
            this.dateTimePickerTime.ValueChanged += new System.EventHandler(this.dateTimePickerTime_ValueChanged);
            // 
            // UserControlDateTimePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelDate);
            this.Name = "UserControlDateTimePanel";
            this.Size = new System.Drawing.Size(154, 21);
            this.EnabledChanged += new System.EventHandler(this.UserControlDateTimePanel_EnabledChanged);
            this.tableLayoutPanelDate.ResumeLayout(false);
            this.tableLayoutPanelDate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDate;
        public System.Windows.Forms.MaskedTextBox maskedTextBoxDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerTime;
    }
}
