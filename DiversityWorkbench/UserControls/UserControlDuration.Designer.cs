namespace DiversityWorkbench.UserControls
{
    partial class UserControlDuration
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelSeparator = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.maskedTextBoxSecond = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxMinute = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxYear = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxMonth = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxDay = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBoxHour = new System.Windows.Forms.MaskedTextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.labelSeparator, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.textBox, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.maskedTextBoxSecond, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.maskedTextBoxMinute, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.maskedTextBoxYear, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.maskedTextBoxMonth, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.maskedTextBoxDay, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.maskedTextBoxHour, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(444, 62);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelSeparator
            // 
            this.labelSeparator.AutoSize = true;
            this.labelSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSeparator.Location = new System.Drawing.Point(199, 3);
            this.labelSeparator.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.labelSeparator.Name = "labelSeparator";
            this.labelSeparator.Size = new System.Drawing.Size(10, 17);
            this.labelSeparator.TabIndex = 6;
            this.labelSeparator.Text = ":";
            // 
            // textBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBox, 7);
            this.textBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox.Location = new System.Drawing.Point(0, 42);
            this.textBox.Margin = new System.Windows.Forms.Padding(0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(444, 20);
            this.textBox.TabIndex = 13;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // maskedTextBoxSecond
            // 
            this.maskedTextBoxSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxSecond.Location = new System.Drawing.Point(365, 0);
            this.maskedTextBoxSecond.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxSecond.Mask = "00";
            this.maskedTextBoxSecond.Name = "maskedTextBoxSecond";
            this.maskedTextBoxSecond.Size = new System.Drawing.Size(79, 20);
            this.maskedTextBoxSecond.TabIndex = 14;
            this.toolTip.SetToolTip(this.maskedTextBoxSecond, "Seconds");
            this.maskedTextBoxSecond.Leave += new System.EventHandler(this.maskedTextBoxSecond_Leave);
            // 
            // maskedTextBoxMinute
            // 
            this.maskedTextBoxMinute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxMinute.Location = new System.Drawing.Point(287, 0);
            this.maskedTextBoxMinute.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxMinute.Mask = "00";
            this.maskedTextBoxMinute.Name = "maskedTextBoxMinute";
            this.maskedTextBoxMinute.Size = new System.Drawing.Size(78, 20);
            this.maskedTextBoxMinute.TabIndex = 15;
            this.toolTip.SetToolTip(this.maskedTextBoxMinute, "Minutes");
            this.maskedTextBoxMinute.Leave += new System.EventHandler(this.maskedTextBoxMinute_Leave);
            // 
            // maskedTextBoxYear
            // 
            this.maskedTextBoxYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxYear.Location = new System.Drawing.Point(0, 0);
            this.maskedTextBoxYear.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxYear.Mask = "0";
            this.maskedTextBoxYear.Name = "maskedTextBoxYear";
            this.maskedTextBoxYear.Size = new System.Drawing.Size(43, 20);
            this.maskedTextBoxYear.TabIndex = 16;
            this.toolTip.SetToolTip(this.maskedTextBoxYear, "Years");
            this.maskedTextBoxYear.Leave += new System.EventHandler(this.maskedTextBoxYear_Leave);
            // 
            // maskedTextBoxMonth
            // 
            this.maskedTextBoxMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxMonth.Location = new System.Drawing.Point(43, 0);
            this.maskedTextBoxMonth.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxMonth.Mask = "00";
            this.maskedTextBoxMonth.Name = "maskedTextBoxMonth";
            this.maskedTextBoxMonth.Size = new System.Drawing.Size(78, 20);
            this.maskedTextBoxMonth.TabIndex = 17;
            this.toolTip.SetToolTip(this.maskedTextBoxMonth, "Months");
            this.maskedTextBoxMonth.Leave += new System.EventHandler(this.maskedTextBoxMonth_Leave);
            // 
            // maskedTextBoxDay
            // 
            this.maskedTextBoxDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxDay.Location = new System.Drawing.Point(121, 0);
            this.maskedTextBoxDay.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxDay.Mask = "00";
            this.maskedTextBoxDay.Name = "maskedTextBoxDay";
            this.maskedTextBoxDay.Size = new System.Drawing.Size(78, 20);
            this.maskedTextBoxDay.TabIndex = 18;
            this.toolTip.SetToolTip(this.maskedTextBoxDay, "Days");
            this.maskedTextBoxDay.Leave += new System.EventHandler(this.maskedTextBoxDay_Leave);
            // 
            // maskedTextBoxHour
            // 
            this.maskedTextBoxHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxHour.Location = new System.Drawing.Point(209, 0);
            this.maskedTextBoxHour.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxHour.Mask = "00";
            this.maskedTextBoxHour.Name = "maskedTextBoxHour";
            this.maskedTextBoxHour.Size = new System.Drawing.Size(78, 20);
            this.maskedTextBoxHour.TabIndex = 19;
            this.toolTip.SetToolTip(this.maskedTextBoxHour, "Hours");
            this.maskedTextBoxHour.Leave += new System.EventHandler(this.maskedTextBoxHour_Leave);
            // 
            // UserControlDuration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlDuration";
            this.Size = new System.Drawing.Size(444, 62);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelSeparator;
        public System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSecond;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxMinute;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxYear;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxMonth;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDay;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxHour;
    }
}
