namespace DiversityCollection.Forms
{
    partial class FormImportWizardTransformation
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.userControlDialogPanel = new DiversityWorkbench.UserControlDialogPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelRegex = new System.Windows.Forms.Label();
            this.textBoxRegex = new System.Windows.Forms.TextBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewTransformation = new System.Windows.Forms.DataGridView();
            this.dataSetImportWizard = new DiversityCollection.Datasets.DataSetImportWizard();
            this.sourceValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.databaseValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelReplacement = new System.Windows.Forms.Label();
            this.textBoxReplacement = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransformation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportWizard)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 5;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.userControlDialogPanel, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelRegex, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxRegex, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonTest, 4, 1);
            this.tableLayoutPanelMain.Controls.Add(this.dataGridViewTransformation, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelReplacement, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxReplacement, 3, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(366, 264);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // userControlDialogPanel
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.userControlDialogPanel, 5);
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDialogPanel.Location = new System.Drawing.Point(3, 234);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(360, 27);
            this.userControlDialogPanel.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 5);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 3);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(360, 13);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Use a regular expression to transform the data in the source";
            // 
            // labelRegex
            // 
            this.labelRegex.AutoSize = true;
            this.labelRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRegex.Location = new System.Drawing.Point(3, 19);
            this.labelRegex.Name = "labelRegex";
            this.labelRegex.Size = new System.Drawing.Size(97, 26);
            this.labelRegex.TabIndex = 2;
            this.labelRegex.Text = "Regular expession:";
            this.labelRegex.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxRegex
            // 
            this.textBoxRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRegex.Location = new System.Drawing.Point(106, 22);
            this.textBoxRegex.Name = "textBoxRegex";
            this.textBoxRegex.Size = new System.Drawing.Size(109, 20);
            this.textBoxRegex.TabIndex = 3;
            // 
            // buttonTest
            // 
            this.buttonTest.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTest.Location = new System.Drawing.Point(348, 19);
            this.buttonTest.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(14, 23);
            this.buttonTest.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonTest, "Test the transformation");
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // dataGridViewTransformation
            // 
            this.dataGridViewTransformation.AutoGenerateColumns = false;
            this.dataGridViewTransformation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTransformation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sourceValueDataGridViewTextBoxColumn,
            this.databaseValueDataGridViewTextBoxColumn});
            this.tableLayoutPanelMain.SetColumnSpan(this.dataGridViewTransformation, 5);
            this.dataGridViewTransformation.DataMember = "DataTableTranslation";
            this.dataGridViewTransformation.DataSource = this.dataSetImportWizard;
            this.dataGridViewTransformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTransformation.Location = new System.Drawing.Point(3, 48);
            this.dataGridViewTransformation.Name = "dataGridViewTransformation";
            this.dataGridViewTransformation.Size = new System.Drawing.Size(360, 180);
            this.dataGridViewTransformation.TabIndex = 5;
            // 
            // dataSetImportWizard
            // 
            this.dataSetImportWizard.DataSetName = "DataSetImportWizard";
            this.dataSetImportWizard.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sourceValueDataGridViewTextBoxColumn
            // 
            this.sourceValueDataGridViewTextBoxColumn.DataPropertyName = "SourceValue";
            this.sourceValueDataGridViewTextBoxColumn.HeaderText = "Value in the source";
            this.sourceValueDataGridViewTextBoxColumn.Name = "sourceValueDataGridViewTextBoxColumn";
            this.sourceValueDataGridViewTextBoxColumn.Width = 150;
            // 
            // databaseValueDataGridViewTextBoxColumn
            // 
            this.databaseValueDataGridViewTextBoxColumn.DataPropertyName = "DatabaseValue";
            this.databaseValueDataGridViewTextBoxColumn.HeaderText = "Transformed value";
            this.databaseValueDataGridViewTextBoxColumn.Name = "databaseValueDataGridViewTextBoxColumn";
            this.databaseValueDataGridViewTextBoxColumn.Width = 150;
            // 
            // labelReplacement
            // 
            this.labelReplacement.AutoSize = true;
            this.labelReplacement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReplacement.Location = new System.Drawing.Point(221, 19);
            this.labelReplacement.Name = "labelReplacement";
            this.labelReplacement.Size = new System.Drawing.Size(64, 26);
            this.labelReplacement.TabIndex = 6;
            this.labelReplacement.Text = "Replace by:";
            this.labelReplacement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxReplacement
            // 
            this.textBoxReplacement.Location = new System.Drawing.Point(291, 22);
            this.textBoxReplacement.Name = "textBoxReplacement";
            this.textBoxReplacement.Size = new System.Drawing.Size(51, 20);
            this.textBoxReplacement.TabIndex = 7;
            // 
            // FormImportWizardTransformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 264);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "FormImportWizardTransformation";
            this.Text = "FormImportWizardTransformation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormImportWizardTransformation_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTransformation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportWizard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DiversityWorkbench.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelRegex;
        private System.Windows.Forms.TextBox textBoxRegex;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DataGridView dataGridViewTransformation;
        private DiversityCollection.Datasets.DataSetImportWizard dataSetImportWizard;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn databaseValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label labelReplacement;
        private System.Windows.Forms.TextBox textBoxReplacement;
    }
}