namespace DiversityWorkbench.Forms
{
    partial class FormEntityInsertPK
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEntityInsertPK));
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelDisplayText = new System.Windows.Forms.Label();
            this.comboBoxDisplayText = new System.Windows.Forms.ComboBox();
            this.labelAbbreviation = new System.Windows.Forms.Label();
            this.comboBoxAbbreviation = new System.Windows.Forms.ComboBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.comboBoxDescription = new System.Windows.Forms.ComboBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(0, 99);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(326, 27);
            this.userControlDialogPanel1.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(320, 18);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Assign the columns in the table to the columns for the entity representation";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelDisplayText, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBoxDisplayText, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelAbbreviation, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBoxAbbreviation, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelDescription, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.comboBoxDescription, 1, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(326, 99);
            this.tableLayoutPanel.TabIndex = 2;
            // 
            // labelDisplayText
            // 
            this.labelDisplayText.AutoSize = true;
            this.labelDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayText.Location = new System.Drawing.Point(3, 18);
            this.labelDisplayText.Name = "labelDisplayText";
            this.labelDisplayText.Size = new System.Drawing.Size(66, 27);
            this.labelDisplayText.TabIndex = 2;
            this.labelDisplayText.Text = "Display text";
            this.labelDisplayText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDisplayText
            // 
            this.comboBoxDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDisplayText.FormattingEnabled = true;
            this.comboBoxDisplayText.Location = new System.Drawing.Point(75, 21);
            this.comboBoxDisplayText.Name = "comboBoxDisplayText";
            this.comboBoxDisplayText.Size = new System.Drawing.Size(248, 21);
            this.comboBoxDisplayText.TabIndex = 3;
            // 
            // labelAbbreviation
            // 
            this.labelAbbreviation.AutoSize = true;
            this.labelAbbreviation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAbbreviation.Location = new System.Drawing.Point(3, 45);
            this.labelAbbreviation.Name = "labelAbbreviation";
            this.labelAbbreviation.Size = new System.Drawing.Size(66, 27);
            this.labelAbbreviation.TabIndex = 4;
            this.labelAbbreviation.Text = "Abbreviation";
            this.labelAbbreviation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxAbbreviation
            // 
            this.comboBoxAbbreviation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAbbreviation.FormattingEnabled = true;
            this.comboBoxAbbreviation.Location = new System.Drawing.Point(75, 48);
            this.comboBoxAbbreviation.Name = "comboBoxAbbreviation";
            this.comboBoxAbbreviation.Size = new System.Drawing.Size(248, 21);
            this.comboBoxAbbreviation.TabIndex = 5;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(3, 72);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(66, 27);
            this.labelDescription.TabIndex = 6;
            this.labelDescription.Text = "Description";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDescription
            // 
            this.comboBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDescription.FormattingEnabled = true;
            this.comboBoxDescription.Location = new System.Drawing.Point(75, 75);
            this.comboBoxDescription.Name = "comboBoxDescription";
            this.comboBoxDescription.Size = new System.Drawing.Size(248, 21);
            this.comboBoxDescription.TabIndex = 7;
            // 
            // FormEntityInsertPK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 126);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.userControlDialogPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEntityInsertPK";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert the primary key of a table";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelDisplayText;
        private System.Windows.Forms.ComboBox comboBoxDisplayText;
        private System.Windows.Forms.Label labelAbbreviation;
        private System.Windows.Forms.ComboBox comboBoxAbbreviation;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.ComboBox comboBoxDescription;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}