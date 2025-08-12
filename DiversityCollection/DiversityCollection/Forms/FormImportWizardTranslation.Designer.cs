namespace DiversityCollection.Forms
{
    partial class FormImportWizardTranslation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportWizardTranslation));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.userControlDialogPanel = new DiversityWorkbench.UserControlDialogPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.dataGridViewTranslation = new System.Windows.Forms.DataGridView();
            this.dataSetImportWizard = new DiversityCollection.Datasets.DataSetImportWizard();
            this.sourceValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.databaseValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTranslation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportWizard)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.userControlDialogPanel, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.dataGridViewTranslation, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(377, 449);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDialogPanel.Location = new System.Drawing.Point(3, 423);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(371, 23);
            this.userControlDialogPanel.TabIndex = 2;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Location = new System.Drawing.Point(3, 3);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(360, 13);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Specify the transation of values found in the file into values in the database";
            // 
            // dataGridViewTranslation
            // 
            this.dataGridViewTranslation.AutoGenerateColumns = false;
            this.dataGridViewTranslation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTranslation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sourceValueDataGridViewTextBoxColumn,
            this.databaseValueDataGridViewTextBoxColumn});
            this.dataGridViewTranslation.DataMember = "DataTableTranslation";
            this.dataGridViewTranslation.DataSource = this.dataSetImportWizard;
            this.dataGridViewTranslation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTranslation.Location = new System.Drawing.Point(3, 22);
            this.dataGridViewTranslation.Name = "dataGridViewTranslation";
            this.dataGridViewTranslation.Size = new System.Drawing.Size(371, 395);
            this.dataGridViewTranslation.TabIndex = 4;
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
            this.databaseValueDataGridViewTextBoxColumn.DataSource = this.dataSetImportWizard;
            this.databaseValueDataGridViewTextBoxColumn.DisplayMember = "DataTableEnumeration.DisplayText";
            this.databaseValueDataGridViewTextBoxColumn.HeaderText = "Value in the database";
            this.databaseValueDataGridViewTextBoxColumn.Name = "databaseValueDataGridViewTextBoxColumn";
            this.databaseValueDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.databaseValueDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.databaseValueDataGridViewTextBoxColumn.ValueMember = "DataTableEnumeration.Key";
            this.databaseValueDataGridViewTextBoxColumn.Width = 150;
            // 
            // FormImportWizardTranslation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 449);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImportWizardTranslation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Translation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormImportWizardTranslation_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTranslation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetImportWizard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private DiversityWorkbench.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.DataGridView dataGridViewTranslation;
        private DiversityCollection.Datasets.DataSetImportWizard dataSetImportWizard;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn databaseValueDataGridViewTextBoxColumn;
    }
}