namespace DiversityCollection.Tasks
{
    partial class FormIPM_Details
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIPM_Details));
            this.userControlDialogPanel1 = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelNumberValue = new System.Windows.Forms.Label();
            this.textBoxNumberValue = new System.Windows.Forms.TextBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.comboBoxResult = new System.Windows.Forms.ComboBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.pictureBoxHeader = new System.Windows.Forms.PictureBox();
            this.labelWhere = new System.Windows.Forms.Label();
            this.labelWhereContent = new System.Windows.Forms.Label();
            this.labelWhen = new System.Windows.Forms.Label();
            this.labelWhenContent = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // userControlDialogPanel1
            // 
            this.userControlDialogPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel1.Location = new System.Drawing.Point(0, 199);
            this.userControlDialogPanel1.Name = "userControlDialogPanel1";
            this.userControlDialogPanel1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel1.Size = new System.Drawing.Size(292, 27);
            this.userControlDialogPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelNumberValue, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxNumberValue, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelResult, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxResult, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelNotes, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.textBoxNotes, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelHeader, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBoxHeader, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelWhere, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelWhereContent, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelWhen, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelWhenContent, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(292, 199);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // labelNumberValue
            // 
            this.labelNumberValue.AutoSize = true;
            this.labelNumberValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumberValue.Location = new System.Drawing.Point(3, 92);
            this.labelNumberValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelNumberValue.Name = "labelNumberValue";
            this.labelNumberValue.Size = new System.Drawing.Size(38, 26);
            this.labelNumberValue.TabIndex = 0;
            this.labelNumberValue.Text = "Count:";
            this.labelNumberValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNumberValue
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxNumberValue, 2);
            this.textBoxNumberValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNumberValue.Location = new System.Drawing.Point(44, 95);
            this.textBoxNumberValue.Name = "textBoxNumberValue";
            this.textBoxNumberValue.Size = new System.Drawing.Size(245, 20);
            this.textBoxNumberValue.TabIndex = 1;
            this.textBoxNumberValue.TextChanged += new System.EventHandler(this.textBoxNumberValue_TextChanged);
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResult.Location = new System.Drawing.Point(3, 118);
            this.labelResult.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(38, 27);
            this.labelResult.TabIndex = 2;
            this.labelResult.Text = "State:";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxResult
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboBoxResult, 2);
            this.comboBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxResult.FormattingEnabled = true;
            this.comboBoxResult.Location = new System.Drawing.Point(44, 121);
            this.comboBoxResult.Name = "comboBoxResult";
            this.comboBoxResult.Size = new System.Drawing.Size(245, 21);
            this.comboBoxResult.TabIndex = 3;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelNotes.Location = new System.Drawing.Point(3, 151);
            this.labelNotes.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(38, 13);
            this.labelNotes.TabIndex = 4;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNotes
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxNotes, 2);
            this.textBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNotes.Location = new System.Drawing.Point(44, 148);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(245, 48);
            this.textBoxNotes.TabIndex = 5;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.BackColor = System.Drawing.SystemColors.HighlightText;
            this.tableLayoutPanel1.SetColumnSpan(this.labelHeader, 2);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 3);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(186, 50);
            this.labelHeader.TabIndex = 6;
            this.labelHeader.Text = "Taxon";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxHeader
            // 
            this.pictureBoxHeader.BackColor = System.Drawing.SystemColors.HighlightText;
            this.pictureBoxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxHeader.Location = new System.Drawing.Point(189, 3);
            this.pictureBoxHeader.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pictureBoxHeader.Name = "pictureBoxHeader";
            this.pictureBoxHeader.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxHeader.TabIndex = 7;
            this.pictureBoxHeader.TabStop = false;
            // 
            // labelWhere
            // 
            this.labelWhere.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelWhere, 2);
            this.labelWhere.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWhere.Location = new System.Drawing.Point(3, 56);
            this.labelWhere.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.labelWhere.Name = "labelWhere";
            this.labelWhere.Size = new System.Drawing.Size(186, 13);
            this.labelWhere.TabIndex = 8;
            this.labelWhere.Text = "Where:";
            this.labelWhere.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelWhereContent
            // 
            this.labelWhereContent.AutoSize = true;
            this.labelWhereContent.BackColor = System.Drawing.SystemColors.HighlightText;
            this.tableLayoutPanel1.SetColumnSpan(this.labelWhereContent, 2);
            this.labelWhereContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWhereContent.Location = new System.Drawing.Point(3, 72);
            this.labelWhereContent.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelWhereContent.Name = "labelWhereContent";
            this.labelWhereContent.Size = new System.Drawing.Size(186, 20);
            this.labelWhereContent.TabIndex = 9;
            this.labelWhereContent.Text = "?";
            this.labelWhereContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelWhen
            // 
            this.labelWhen.AutoSize = true;
            this.labelWhen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWhen.Location = new System.Drawing.Point(192, 56);
            this.labelWhen.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.labelWhen.Name = "labelWhen";
            this.labelWhen.Size = new System.Drawing.Size(100, 13);
            this.labelWhen.TabIndex = 10;
            this.labelWhen.Text = "When:";
            this.labelWhen.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelWhenContent
            // 
            this.labelWhenContent.AutoSize = true;
            this.labelWhenContent.BackColor = System.Drawing.SystemColors.HighlightText;
            this.labelWhenContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWhenContent.Location = new System.Drawing.Point(189, 72);
            this.labelWhenContent.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelWhenContent.Name = "labelWhenContent";
            this.labelWhenContent.Size = new System.Drawing.Size(100, 20);
            this.labelWhenContent.TabIndex = 11;
            this.labelWhenContent.Text = "?";
            this.labelWhenContent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormIPM_Details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 226);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.userControlDialogPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormIPM_Details";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IPM";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormIPM_Details_HelpRequested);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelNumberValue;
        private System.Windows.Forms.TextBox textBoxNumberValue;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.ComboBox comboBoxResult;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.PictureBox pictureBoxHeader;
        private System.Windows.Forms.Label labelWhere;
        private System.Windows.Forms.Label labelWhereContent;
        private System.Windows.Forms.Label labelWhen;
        private System.Windows.Forms.Label labelWhenContent;
    }
}