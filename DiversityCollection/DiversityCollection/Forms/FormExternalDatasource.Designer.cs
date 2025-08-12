namespace DiversityCollection.Forms
{
    partial class FormExternalDatasource
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExternalDatasource));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.externalDatasourceIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalDatasourceNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalDatasourceVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rightsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalDatasourceAuthorsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalDatasourceURIDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalDatasourceInstitutionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.internalNotesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalAttributeNameIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.preferredSequenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.disabledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataSetExternalDatasource = new DiversityCollection.Datasets.DataSetExternalDatasource();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetExternalDatasource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AutoGenerateColumns = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.externalDatasourceIDDataGridViewTextBoxColumn,
            this.externalDatasourceNameDataGridViewTextBoxColumn,
            this.externalDatasourceVersionDataGridViewTextBoxColumn,
            this.rightsDataGridViewTextBoxColumn,
            this.externalDatasourceAuthorsDataGridViewTextBoxColumn,
            this.externalDatasourceURIDataGridViewTextBoxColumn,
            this.externalDatasourceInstitutionDataGridViewTextBoxColumn,
            this.internalNotesDataGridViewTextBoxColumn,
            this.externalAttributeNameIDDataGridViewTextBoxColumn,
            this.preferredSequenceDataGridViewTextBoxColumn,
            this.disabledDataGridViewCheckBoxColumn});
            this.dataGridView.DataMember = "CollectionExternalDatasource";
            this.dataGridView.DataSource = this.dataSetExternalDatasource;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(878, 319);
            this.dataGridView.TabIndex = 2;
            // 
            // externalDatasourceIDDataGridViewTextBoxColumn
            // 
            this.externalDatasourceIDDataGridViewTextBoxColumn.DataPropertyName = "ExternalDatasourceID";
            this.externalDatasourceIDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.externalDatasourceIDDataGridViewTextBoxColumn.Name = "externalDatasourceIDDataGridViewTextBoxColumn";
            this.externalDatasourceIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.externalDatasourceIDDataGridViewTextBoxColumn.Width = 43;
            // 
            // externalDatasourceNameDataGridViewTextBoxColumn
            // 
            this.externalDatasourceNameDataGridViewTextBoxColumn.DataPropertyName = "ExternalDatasourceName";
            this.externalDatasourceNameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.externalDatasourceNameDataGridViewTextBoxColumn.Name = "externalDatasourceNameDataGridViewTextBoxColumn";
            this.externalDatasourceNameDataGridViewTextBoxColumn.Width = 60;
            // 
            // externalDatasourceVersionDataGridViewTextBoxColumn
            // 
            this.externalDatasourceVersionDataGridViewTextBoxColumn.DataPropertyName = "ExternalDatasourceVersion";
            this.externalDatasourceVersionDataGridViewTextBoxColumn.HeaderText = "Version";
            this.externalDatasourceVersionDataGridViewTextBoxColumn.Name = "externalDatasourceVersionDataGridViewTextBoxColumn";
            this.externalDatasourceVersionDataGridViewTextBoxColumn.Width = 67;
            // 
            // rightsDataGridViewTextBoxColumn
            // 
            this.rightsDataGridViewTextBoxColumn.DataPropertyName = "Rights";
            this.rightsDataGridViewTextBoxColumn.HeaderText = "Rights";
            this.rightsDataGridViewTextBoxColumn.Name = "rightsDataGridViewTextBoxColumn";
            this.rightsDataGridViewTextBoxColumn.Width = 62;
            // 
            // externalDatasourceAuthorsDataGridViewTextBoxColumn
            // 
            this.externalDatasourceAuthorsDataGridViewTextBoxColumn.DataPropertyName = "ExternalDatasourceAuthors";
            this.externalDatasourceAuthorsDataGridViewTextBoxColumn.HeaderText = "Authors";
            this.externalDatasourceAuthorsDataGridViewTextBoxColumn.Name = "externalDatasourceAuthorsDataGridViewTextBoxColumn";
            this.externalDatasourceAuthorsDataGridViewTextBoxColumn.Width = 68;
            // 
            // externalDatasourceURIDataGridViewTextBoxColumn
            // 
            this.externalDatasourceURIDataGridViewTextBoxColumn.DataPropertyName = "ExternalDatasourceURI";
            this.externalDatasourceURIDataGridViewTextBoxColumn.HeaderText = "URI";
            this.externalDatasourceURIDataGridViewTextBoxColumn.Name = "externalDatasourceURIDataGridViewTextBoxColumn";
            this.externalDatasourceURIDataGridViewTextBoxColumn.Width = 51;
            // 
            // externalDatasourceInstitutionDataGridViewTextBoxColumn
            // 
            this.externalDatasourceInstitutionDataGridViewTextBoxColumn.DataPropertyName = "ExternalDatasourceInstitution";
            this.externalDatasourceInstitutionDataGridViewTextBoxColumn.HeaderText = "Institution";
            this.externalDatasourceInstitutionDataGridViewTextBoxColumn.Name = "externalDatasourceInstitutionDataGridViewTextBoxColumn";
            this.externalDatasourceInstitutionDataGridViewTextBoxColumn.Width = 77;
            // 
            // internalNotesDataGridViewTextBoxColumn
            // 
            this.internalNotesDataGridViewTextBoxColumn.DataPropertyName = "InternalNotes";
            this.internalNotesDataGridViewTextBoxColumn.HeaderText = "InternalNotes";
            this.internalNotesDataGridViewTextBoxColumn.Name = "internalNotesDataGridViewTextBoxColumn";
            this.internalNotesDataGridViewTextBoxColumn.Width = 95;
            // 
            // externalAttributeNameIDDataGridViewTextBoxColumn
            // 
            this.externalAttributeNameIDDataGridViewTextBoxColumn.DataPropertyName = "ExternalAttribute_NameID";
            this.externalAttributeNameIDDataGridViewTextBoxColumn.HeaderText = "ExternalAttribute_NameID";
            this.externalAttributeNameIDDataGridViewTextBoxColumn.Name = "externalAttributeNameIDDataGridViewTextBoxColumn";
            this.externalAttributeNameIDDataGridViewTextBoxColumn.Width = 154;
            // 
            // preferredSequenceDataGridViewTextBoxColumn
            // 
            this.preferredSequenceDataGridViewTextBoxColumn.DataPropertyName = "PreferredSequence";
            this.preferredSequenceDataGridViewTextBoxColumn.HeaderText = "PreferredSequence";
            this.preferredSequenceDataGridViewTextBoxColumn.Name = "preferredSequenceDataGridViewTextBoxColumn";
            this.preferredSequenceDataGridViewTextBoxColumn.Width = 124;
            // 
            // disabledDataGridViewCheckBoxColumn
            // 
            this.disabledDataGridViewCheckBoxColumn.DataPropertyName = "Disabled";
            this.disabledDataGridViewCheckBoxColumn.HeaderText = "Disabled";
            this.disabledDataGridViewCheckBoxColumn.Name = "disabledDataGridViewCheckBoxColumn";
            this.disabledDataGridViewCheckBoxColumn.Width = 54;
            // 
            // dataSetExternalDatasource
            // 
            this.dataSetExternalDatasource.DataSetName = "DataSetExternalDatasource";
            this.dataSetExternalDatasource.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FormExternalDatasource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 319);
            this.Controls.Add(this.dataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExternalDatasource";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Administration of external datasources";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormExternalDatasource_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetExternalDatasource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private Datasets.DataSetExternalDatasource dataSetExternalDatasource;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalDatasourceIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalDatasourceNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalDatasourceVersionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rightsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalDatasourceAuthorsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalDatasourceURIDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalDatasourceInstitutionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn internalNotesDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalAttributeNameIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn preferredSequenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn disabledDataGridViewCheckBoxColumn;

    }
}