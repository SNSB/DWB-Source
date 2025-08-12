namespace DiversityCollection.Forms
{
    partial class FormExportSeSam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportSeSam));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.checkedListBoxNote = new System.Windows.Forms.CheckedListBox();
            this.labelIncludeNotes = new System.Windows.Forms.Label();
            this.labelIsFemale = new System.Windows.Forms.Label();
            this.labelIsMale = new System.Windows.Forms.Label();
            this.labelIsWorker = new System.Windows.Forms.Label();
            this.checkedListBoxWorker = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxMale = new System.Windows.Forms.CheckedListBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxExportResult = new System.Windows.Forms.TextBox();
            this.checkedListBoxFemale = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.checkedListBoxNote, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelIncludeNotes, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelIsFemale, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelIsMale, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.labelIsWorker, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.checkedListBoxWorker, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.checkedListBoxMale, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.textBoxExportResult, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonStart, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.checkedListBoxFemale, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(851, 448);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 4);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(9, 9);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(9);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(833, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Export the selected data into a SESAM compatible format";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkedListBoxNote
            // 
            this.checkedListBoxNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxNote.FormattingEnabled = true;
            this.checkedListBoxNote.Location = new System.Drawing.Point(3, 47);
            this.checkedListBoxNote.Name = "checkedListBoxNote";
            this.checkedListBoxNote.Size = new System.Drawing.Size(277, 69);
            this.checkedListBoxNote.TabIndex = 1;
            // 
            // labelIncludeNotes
            // 
            this.labelIncludeNotes.AutoSize = true;
            this.labelIncludeNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIncludeNotes.Location = new System.Drawing.Point(3, 31);
            this.labelIncludeNotes.Name = "labelIncludeNotes";
            this.labelIncludeNotes.Size = new System.Drawing.Size(277, 13);
            this.labelIncludeNotes.TabIndex = 2;
            this.labelIncludeNotes.Text = "Include into notes";
            // 
            // labelIsFemale
            // 
            this.labelIsFemale.AutoSize = true;
            this.labelIsFemale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIsFemale.Location = new System.Drawing.Point(286, 31);
            this.labelIsFemale.Name = "labelIsFemale";
            this.labelIsFemale.Size = new System.Drawing.Size(183, 13);
            this.labelIsFemale.TabIndex = 3;
            this.labelIsFemale.Text = "Regard as female";
            // 
            // labelIsMale
            // 
            this.labelIsMale.AutoSize = true;
            this.labelIsMale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIsMale.Location = new System.Drawing.Point(664, 31);
            this.labelIsMale.Name = "labelIsMale";
            this.labelIsMale.Size = new System.Drawing.Size(184, 13);
            this.labelIsMale.TabIndex = 4;
            this.labelIsMale.Text = "Regard as male";
            // 
            // labelIsWorker
            // 
            this.labelIsWorker.AutoSize = true;
            this.labelIsWorker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIsWorker.Location = new System.Drawing.Point(475, 31);
            this.labelIsWorker.Name = "labelIsWorker";
            this.labelIsWorker.Size = new System.Drawing.Size(183, 13);
            this.labelIsWorker.TabIndex = 5;
            this.labelIsWorker.Text = "Regard as female worker";
            // 
            // checkedListBoxWorker
            // 
            this.checkedListBoxWorker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxWorker.FormattingEnabled = true;
            this.checkedListBoxWorker.Location = new System.Drawing.Point(475, 47);
            this.checkedListBoxWorker.Name = "checkedListBoxWorker";
            this.checkedListBoxWorker.Size = new System.Drawing.Size(183, 69);
            this.checkedListBoxWorker.TabIndex = 7;
            // 
            // checkedListBoxMale
            // 
            this.checkedListBoxMale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxMale.FormattingEnabled = true;
            this.checkedListBoxMale.Location = new System.Drawing.Point(664, 47);
            this.checkedListBoxMale.Name = "checkedListBoxMale";
            this.checkedListBoxMale.Size = new System.Drawing.Size(184, 69);
            this.checkedListBoxMale.TabIndex = 8;
            // 
            // buttonStart
            // 
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonStart.Image = global::DiversityCollection.Resource.Export;
            this.buttonStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStart.Location = new System.Drawing.Point(3, 122);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(88, 23);
            this.buttonStart.TabIndex = 9;
            this.buttonStart.Text = "Start export";
            this.buttonStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxExportResult
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxExportResult, 4);
            this.textBoxExportResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExportResult.Enabled = false;
            this.textBoxExportResult.Location = new System.Drawing.Point(3, 151);
            this.textBoxExportResult.Multiline = true;
            this.textBoxExportResult.Name = "textBoxExportResult";
            this.textBoxExportResult.Size = new System.Drawing.Size(845, 294);
            this.textBoxExportResult.TabIndex = 10;
            // 
            // checkedListBoxFemale
            // 
            this.checkedListBoxFemale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxFemale.FormattingEnabled = true;
            this.checkedListBoxFemale.Location = new System.Drawing.Point(286, 47);
            this.checkedListBoxFemale.Name = "checkedListBoxFemale";
            this.checkedListBoxFemale.Size = new System.Drawing.Size(183, 69);
            this.checkedListBoxFemale.TabIndex = 11;
            // 
            // FormExportSeSam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 448);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExportSeSam";
            this.Text = "FormExportSeSam";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.CheckedListBox checkedListBoxNote;
        private System.Windows.Forms.Label labelIncludeNotes;
        private System.Windows.Forms.Label labelIsFemale;
        private System.Windows.Forms.Label labelIsMale;
        private System.Windows.Forms.Label labelIsWorker;
        private System.Windows.Forms.CheckedListBox checkedListBoxWorker;
        private System.Windows.Forms.CheckedListBox checkedListBoxMale;
        private System.Windows.Forms.TextBox textBoxExportResult;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.CheckedListBox checkedListBoxFemale;
    }
}