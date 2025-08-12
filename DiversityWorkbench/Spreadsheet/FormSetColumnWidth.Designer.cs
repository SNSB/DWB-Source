namespace DiversityWorkbench.Spreadsheet
{
    partial class FormSetColumnWidth
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetColumnWidth));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelMaxWidth = new System.Windows.Forms.Label();
            this.numericUpDownMaxWidth = new System.Windows.Forms.NumericUpDown();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.radioButtonHeader = new System.Windows.Forms.RadioButton();
            this.radioButtonContentAndHeader = new System.Windows.Forms.RadioButton();
            this.radioButtonContent = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelMaxWidth, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownMaxWidth, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.radioButtonHeader, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.userControlDialogPanel, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.radioButtonContentAndHeader, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.radioButtonContent, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(165, 154);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Location = new System.Drawing.Point(3, 3);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(156, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Setting the width for all columns";
            // 
            // labelMaxWidth
            // 
            this.labelMaxWidth.AutoSize = true;
            this.labelMaxWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxWidth.Location = new System.Drawing.Point(3, 19);
            this.labelMaxWidth.Name = "labelMaxWidth";
            this.labelMaxWidth.Size = new System.Drawing.Size(76, 26);
            this.labelMaxWidth.TabIndex = 1;
            this.labelMaxWidth.Text = "Maximal width:";
            this.labelMaxWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDownMaxWidth
            // 
            this.numericUpDownMaxWidth.Location = new System.Drawing.Point(85, 22);
            this.numericUpDownMaxWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMaxWidth.Name = "numericUpDownMaxWidth";
            this.numericUpDownMaxWidth.Size = new System.Drawing.Size(77, 20);
            this.numericUpDownMaxWidth.TabIndex = 2;
            this.numericUpDownMaxWidth.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // userControlDialogPanel
            // 
            this.tableLayoutPanel.SetColumnSpan(this.userControlDialogPanel, 2);
            this.userControlDialogPanel.Location = new System.Drawing.Point(3, 126);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(159, 25);
            this.userControlDialogPanel.TabIndex = 3;
            // 
            // radioButtonHeader
            // 
            this.radioButtonHeader.AutoSize = true;
            this.radioButtonHeader.Checked = true;
            this.tableLayoutPanel.SetColumnSpan(this.radioButtonHeader, 2);
            this.radioButtonHeader.Location = new System.Drawing.Point(3, 48);
            this.radioButtonHeader.Name = "radioButtonHeader";
            this.radioButtonHeader.Size = new System.Drawing.Size(96, 17);
            this.radioButtonHeader.TabIndex = 4;
            this.radioButtonHeader.TabStop = true;
            this.radioButtonHeader.Text = "Column header";
            this.radioButtonHeader.UseVisualStyleBackColor = true;
            // 
            // radioButtonContentAndHeader
            // 
            this.radioButtonContentAndHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.radioButtonContentAndHeader, 2);
            this.radioButtonContentAndHeader.Location = new System.Drawing.Point(3, 94);
            this.radioButtonContentAndHeader.Name = "radioButtonContentAndHeader";
            this.radioButtonContentAndHeader.Size = new System.Drawing.Size(119, 17);
            this.radioButtonContentAndHeader.TabIndex = 5;
            this.radioButtonContentAndHeader.Text = "Content and header";
            this.radioButtonContentAndHeader.UseVisualStyleBackColor = true;
            // 
            // radioButtonContent
            // 
            this.radioButtonContent.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.radioButtonContent, 2);
            this.radioButtonContent.Location = new System.Drawing.Point(3, 71);
            this.radioButtonContent.Name = "radioButtonContent";
            this.radioButtonContent.Size = new System.Drawing.Size(62, 17);
            this.radioButtonContent.TabIndex = 6;
            this.radioButtonContent.Text = "Content";
            this.radioButtonContent.UseVisualStyleBackColor = true;
            // 
            // FormSetColumnWidth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(165, 154);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetColumnWidth";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set column width";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaxWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelMaxWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxWidth;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.RadioButton radioButtonHeader;
        private System.Windows.Forms.RadioButton radioButtonContentAndHeader;
        private System.Windows.Forms.RadioButton radioButtonContent;
    }
}