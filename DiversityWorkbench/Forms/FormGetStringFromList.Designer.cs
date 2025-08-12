namespace DiversityWorkbench.Forms
{
    partial class FormGetStringFromList
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.labelTextBox = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(4, 90);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(392, 29);
            this.userControlDialogPanel1.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(386, 13);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "select";
            // 
            // comboBox
            // 
            this.comboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(3, 25);
            this.comboBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.comboBox.MaxDropDownItems = 20;
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(386, 21);
            this.comboBox.Sorted = true;
            this.comboBox.TabIndex = 2;
            // 
            // labelTextBox
            // 
            this.labelTextBox.AutoSize = true;
            this.labelTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTextBox.Location = new System.Drawing.Point(3, 46);
            this.labelTextBox.Name = "labelTextBox";
            this.labelTextBox.Size = new System.Drawing.Size(386, 13);
            this.labelTextBox.TabIndex = 3;
            this.labelTextBox.Text = "type";
            this.labelTextBox.Visible = false;
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Location = new System.Drawing.Point(3, 62);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(386, 20);
            this.textBox.TabIndex = 4;
            this.textBox.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.comboBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelTextBox, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(392, 86);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // FormGetStringFromList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 123);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.userControlDialogPanel1);
            this.Name = "FormGetStringFromList";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select from the list";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Label labelTextBox;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}