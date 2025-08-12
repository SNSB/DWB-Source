namespace DiversityCollection.BayernFlora
{
    partial class FormExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExport));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.comboBoxAnalysis = new System.Windows.Forms.ComboBox();
            this.checkedListBoxAnalysis = new System.Windows.Forms.CheckedListBox();
            this.labelProject = new System.Windows.Forms.Label();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelMessage = new System.Windows.Forms.Label();
            this.buttonExport = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.checkBoxNoAnalysis = new System.Windows.Forms.CheckBox();
            this.labelYearFrom = new System.Windows.Forms.Label();
            this.labelYearUntil = new System.Windows.Forms.Label();
            this.numericUpDownYearFrom = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownYearUntil = new System.Windows.Forms.NumericUpDown();
            this.buttonAnalysisAll = new System.Windows.Forms.Button();
            this.buttonAnalysisNone = new System.Windows.Forms.Button();
            this.checkBoxUseAnalysis = new System.Windows.Forms.CheckBox();
            this.labelTaxaCount = new System.Windows.Forms.Label();
            this.textBoxTaxaCount = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYearFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYearUntil)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxAnalysis, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.checkedListBoxAnalysis, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelProject, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxProject, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStart, 3, 2);
            this.tableLayoutPanelMain.Controls.Add(this.progressBar, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelMessage, 3, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonExport, 3, 6);
            this.tableLayoutPanelMain.Controls.Add(this.dataGridView, 1, 5);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxNoAnalysis, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.labelYearFrom, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelYearUntil, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.numericUpDownYearFrom, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.numericUpDownYearUntil, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAnalysisAll, 1, 6);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAnalysisNone, 2, 6);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxUseAnalysis, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelTaxaCount, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTaxaCount, 2, 4);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 7;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(743, 637);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 4);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(6, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(731, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Export der Anzahl der Sippen je Quadrant";
            // 
            // comboBoxAnalysis
            // 
            this.comboBoxAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAnalysis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnalysis.Enabled = false;
            this.comboBoxAnalysis.FormattingEnabled = true;
            this.comboBoxAnalysis.Location = new System.Drawing.Point(3, 87);
            this.comboBoxAnalysis.Name = "comboBoxAnalysis";
            this.comboBoxAnalysis.Size = new System.Drawing.Size(150, 21);
            this.comboBoxAnalysis.TabIndex = 2;
            this.comboBoxAnalysis.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAnalysis_SelectionChangeCommitted);
            // 
            // checkedListBoxAnalysis
            // 
            this.checkedListBoxAnalysis.CheckOnClick = true;
            this.checkedListBoxAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxAnalysis.Enabled = false;
            this.checkedListBoxAnalysis.FormattingEnabled = true;
            this.checkedListBoxAnalysis.Location = new System.Drawing.Point(3, 114);
            this.checkedListBoxAnalysis.Name = "checkedListBoxAnalysis";
            this.checkedListBoxAnalysis.Size = new System.Drawing.Size(150, 491);
            this.checkedListBoxAnalysis.TabIndex = 3;
            this.checkedListBoxAnalysis.Click += new System.EventHandler(this.checkedListBoxAnalysis_Click);
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Location = new System.Drawing.Point(3, 25);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(40, 13);
            this.labelProject.TabIndex = 4;
            this.labelProject.Text = "Projekt";
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProject.DropDownWidth = 300;
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(3, 41);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(150, 21);
            this.comboBoxProject.TabIndex = 5;
            // 
            // buttonStart
            // 
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStart.Location = new System.Drawing.Point(621, 41);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(119, 23);
            this.buttonStart.TabIndex = 6;
            this.buttonStart.Text = "Auswertung starten";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // progressBar
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.progressBar, 3);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(159, 67);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(581, 17);
            this.progressBar.TabIndex = 7;
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Location = new System.Drawing.Point(279, 84);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(461, 27);
            this.labelMessage.TabIndex = 8;
            this.labelMessage.Text = "...";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonExport
            // 
            this.buttonExport.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExport.Image = global::DiversityCollection.Resource.Export;
            this.buttonExport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExport.Location = new System.Drawing.Point(621, 611);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(119, 23);
            this.buttonExport.TabIndex = 9;
            this.buttonExport.Text = "Export starten";
            this.buttonExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelMain.SetColumnSpan(this.dataGridView, 3);
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(159, 114);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(581, 491);
            this.dataGridView.TabIndex = 10;
            // 
            // checkBoxNoAnalysis
            // 
            this.checkBoxNoAnalysis.AutoSize = true;
            this.checkBoxNoAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxNoAnalysis.Location = new System.Drawing.Point(3, 611);
            this.checkBoxNoAnalysis.Name = "checkBoxNoAnalysis";
            this.checkBoxNoAnalysis.Size = new System.Drawing.Size(150, 23);
            this.checkBoxNoAnalysis.TabIndex = 11;
            this.checkBoxNoAnalysis.Text = "Include missing";
            this.checkBoxNoAnalysis.UseVisualStyleBackColor = true;
            this.checkBoxNoAnalysis.Visible = false;
            // 
            // labelYearFrom
            // 
            this.labelYearFrom.AutoSize = true;
            this.labelYearFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelYearFrom.Location = new System.Drawing.Point(159, 25);
            this.labelYearFrom.Name = "labelYearFrom";
            this.labelYearFrom.Size = new System.Drawing.Size(54, 13);
            this.labelYearFrom.TabIndex = 12;
            this.labelYearFrom.Text = "Jahr von";
            // 
            // labelYearUntil
            // 
            this.labelYearUntil.AutoSize = true;
            this.labelYearUntil.Location = new System.Drawing.Point(219, 25);
            this.labelYearUntil.Name = "labelYearUntil";
            this.labelYearUntil.Size = new System.Drawing.Size(20, 13);
            this.labelYearUntil.TabIndex = 13;
            this.labelYearUntil.Text = "bis";
            // 
            // numericUpDownYearFrom
            // 
            this.numericUpDownYearFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownYearFrom.Location = new System.Drawing.Point(159, 41);
            this.numericUpDownYearFrom.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numericUpDownYearFrom.Name = "numericUpDownYearFrom";
            this.numericUpDownYearFrom.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownYearFrom.TabIndex = 14;
            this.numericUpDownYearFrom.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            // 
            // numericUpDownYearUntil
            // 
            this.numericUpDownYearUntil.Location = new System.Drawing.Point(219, 41);
            this.numericUpDownYearUntil.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numericUpDownYearUntil.Name = "numericUpDownYearUntil";
            this.numericUpDownYearUntil.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownYearUntil.TabIndex = 15;
            this.numericUpDownYearUntil.Value = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            // 
            // buttonAnalysisAll
            // 
            this.buttonAnalysisAll.Image = global::DiversityCollection.Resource.CheckYes;
            this.buttonAnalysisAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAnalysisAll.Location = new System.Drawing.Point(159, 611);
            this.buttonAnalysisAll.Name = "buttonAnalysisAll";
            this.buttonAnalysisAll.Size = new System.Drawing.Size(54, 23);
            this.buttonAnalysisAll.TabIndex = 16;
            this.buttonAnalysisAll.Text = "All";
            this.buttonAnalysisAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAnalysisAll.UseVisualStyleBackColor = true;
            this.buttonAnalysisAll.Visible = false;
            this.buttonAnalysisAll.Click += new System.EventHandler(this.buttonAnalysisAll_Click);
            // 
            // buttonAnalysisNone
            // 
            this.buttonAnalysisNone.Image = global::DiversityCollection.Resource.CheckNo;
            this.buttonAnalysisNone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAnalysisNone.Location = new System.Drawing.Point(219, 611);
            this.buttonAnalysisNone.Name = "buttonAnalysisNone";
            this.buttonAnalysisNone.Size = new System.Drawing.Size(54, 23);
            this.buttonAnalysisNone.TabIndex = 17;
            this.buttonAnalysisNone.Text = "None";
            this.buttonAnalysisNone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAnalysisNone.UseVisualStyleBackColor = true;
            this.buttonAnalysisNone.Visible = false;
            this.buttonAnalysisNone.Click += new System.EventHandler(this.buttonAnalysisNone_Click);
            // 
            // checkBoxUseAnalysis
            // 
            this.checkBoxUseAnalysis.AutoSize = true;
            this.checkBoxUseAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxUseAnalysis.Location = new System.Drawing.Point(3, 67);
            this.checkBoxUseAnalysis.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.checkBoxUseAnalysis.Name = "checkBoxUseAnalysis";
            this.checkBoxUseAnalysis.Size = new System.Drawing.Size(150, 17);
            this.checkBoxUseAnalysis.TabIndex = 18;
            this.checkBoxUseAnalysis.Text = "Auf Analyse einschränken";
            this.checkBoxUseAnalysis.UseVisualStyleBackColor = true;
            this.checkBoxUseAnalysis.CheckedChanged += new System.EventHandler(this.checkBoxUseAnalysis_CheckedChanged);
            // 
            // labelTaxaCount
            // 
            this.labelTaxaCount.AutoSize = true;
            this.labelTaxaCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxaCount.Location = new System.Drawing.Point(159, 84);
            this.labelTaxaCount.Name = "labelTaxaCount";
            this.labelTaxaCount.Size = new System.Drawing.Size(54, 27);
            this.labelTaxaCount.TabIndex = 19;
            this.labelTaxaCount.Text = "Taxa:";
            this.labelTaxaCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTaxaCount
            // 
            this.textBoxTaxaCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTaxaCount.Location = new System.Drawing.Point(219, 87);
            this.textBoxTaxaCount.Name = "textBoxTaxaCount";
            this.textBoxTaxaCount.ReadOnly = true;
            this.textBoxTaxaCount.Size = new System.Drawing.Size(54, 20);
            this.textBoxTaxaCount.TabIndex = 20;
            // 
            // FormExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 637);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export - Sippen je Quadrant";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYearFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYearUntil)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.ComboBox comboBoxAnalysis;
        private System.Windows.Forms.CheckedListBox checkedListBoxAnalysis;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.CheckBox checkBoxNoAnalysis;
        private System.Windows.Forms.Label labelYearFrom;
        private System.Windows.Forms.Label labelYearUntil;
        private System.Windows.Forms.NumericUpDown numericUpDownYearFrom;
        private System.Windows.Forms.NumericUpDown numericUpDownYearUntil;
        private System.Windows.Forms.Button buttonAnalysisAll;
        private System.Windows.Forms.Button buttonAnalysisNone;
        private System.Windows.Forms.CheckBox checkBoxUseAnalysis;
        private System.Windows.Forms.Label labelTaxaCount;
        private System.Windows.Forms.TextBox textBoxTaxaCount;
    }
}