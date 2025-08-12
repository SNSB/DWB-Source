namespace DiversityWorkbench.Forms
{
    partial class FormApplicationSearchSelectionStrings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApplicationSearchSelectionStrings));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.dataGridViewTable = new System.Windows.Forms.DataGridView();
            this.userNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.userProxyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetApplication = new DiversityWorkbench.Datasets.DataSetApplication();
            this.sQLStringIdentifierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemTableDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sQLStringDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanelTest = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTestCount = new System.Windows.Forms.Button();
            this.labelTestCount = new System.Windows.Forms.Label();
            this.textBoxTestCount = new System.Windows.Forms.TextBox();
            this.buttonTestQuery = new System.Windows.Forms.Button();
            this.labelTestQuery = new System.Windows.Forms.Label();
            this.dataGridViewQuery = new System.Windows.Forms.DataGridView();
            this.buttonSaveSQL = new System.Windows.Forms.Button();
            this.textBoxSQL = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonNewEntry = new System.Windows.Forms.Button();
            this.buttonCopyQuery = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSaveAll = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.userProxyTableAdapter = new DiversityWorkbench.Datasets.DataSetApplicationTableAdapters.UserProxyTableAdapter();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userProxyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetApplication)).BeginInit();
            this.tableLayoutPanelTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewQuery)).BeginInit();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 31);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.dataGridViewTable);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelTest);
            this.splitContainerMain.Size = new System.Drawing.Size(1005, 551);
            this.splitContainerMain.SplitterDistance = 270;
            this.splitContainerMain.TabIndex = 0;
            // 
            // dataGridViewTable
            // 
            this.dataGridViewTable.AllowUserToAddRows = false;
            this.dataGridViewTable.AllowUserToDeleteRows = false;
            this.dataGridViewTable.AutoGenerateColumns = false;
            this.dataGridViewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userNameDataGridViewTextBoxColumn,
            this.sQLStringIdentifierDataGridViewTextBoxColumn,
            this.itemTableDataGridViewTextBoxColumn,
            this.sQLStringDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.dataGridViewTable.DataMember = "ApplicationSearchSelectionStrings";
            this.dataGridViewTable.DataSource = this.dataSetApplication;
            this.dataGridViewTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTable.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTable.Name = "dataGridViewTable";
            this.dataGridViewTable.RowHeadersWidth = 31;
            this.dataGridViewTable.Size = new System.Drawing.Size(1005, 270);
            this.dataGridViewTable.TabIndex = 0;
            this.dataGridViewTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTable_CellClick);
            this.dataGridViewTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewTable_DataError);
            // 
            // userNameDataGridViewTextBoxColumn
            // 
            this.userNameDataGridViewTextBoxColumn.DataPropertyName = "UserName";
            this.userNameDataGridViewTextBoxColumn.DataSource = this.userProxyBindingSource;
            this.userNameDataGridViewTextBoxColumn.DisplayMember = "CombinedNameCache";
            this.userNameDataGridViewTextBoxColumn.HeaderText = "UserName";
            this.userNameDataGridViewTextBoxColumn.Name = "userNameDataGridViewTextBoxColumn";
            this.userNameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.userNameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.userNameDataGridViewTextBoxColumn.ValueMember = "LoginName";
            this.userNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // userProxyBindingSource
            // 
            this.userProxyBindingSource.DataMember = "UserProxy";
            this.userProxyBindingSource.DataSource = this.dataSetApplication;
            // 
            // dataSetApplication
            // 
            this.dataSetApplication.DataSetName = "DataSetApplication";
            this.dataSetApplication.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sQLStringIdentifierDataGridViewTextBoxColumn
            // 
            this.sQLStringIdentifierDataGridViewTextBoxColumn.DataPropertyName = "SQLStringIdentifier";
            this.sQLStringIdentifierDataGridViewTextBoxColumn.HeaderText = "SQLStringIdentifier";
            this.sQLStringIdentifierDataGridViewTextBoxColumn.Name = "sQLStringIdentifierDataGridViewTextBoxColumn";
            this.sQLStringIdentifierDataGridViewTextBoxColumn.Width = 150;
            // 
            // itemTableDataGridViewTextBoxColumn
            // 
            this.itemTableDataGridViewTextBoxColumn.DataPropertyName = "ItemTable";
            this.itemTableDataGridViewTextBoxColumn.HeaderText = "ItemTable";
            this.itemTableDataGridViewTextBoxColumn.Name = "itemTableDataGridViewTextBoxColumn";
            this.itemTableDataGridViewTextBoxColumn.Width = 150;
            // 
            // sQLStringDataGridViewTextBoxColumn
            // 
            this.sQLStringDataGridViewTextBoxColumn.DataPropertyName = "SQLString";
            this.sQLStringDataGridViewTextBoxColumn.HeaderText = "SQLString";
            this.sQLStringDataGridViewTextBoxColumn.Name = "sQLStringDataGridViewTextBoxColumn";
            this.sQLStringDataGridViewTextBoxColumn.Width = 300;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Width = 200;
            // 
            // tableLayoutPanelTest
            // 
            this.tableLayoutPanelTest.ColumnCount = 3;
            this.tableLayoutPanelTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTest.Controls.Add(this.buttonTestCount, 0, 2);
            this.tableLayoutPanelTest.Controls.Add(this.labelTestCount, 1, 2);
            this.tableLayoutPanelTest.Controls.Add(this.textBoxTestCount, 2, 2);
            this.tableLayoutPanelTest.Controls.Add(this.buttonTestQuery, 0, 3);
            this.tableLayoutPanelTest.Controls.Add(this.labelTestQuery, 1, 3);
            this.tableLayoutPanelTest.Controls.Add(this.dataGridViewQuery, 1, 4);
            this.tableLayoutPanelTest.Controls.Add(this.buttonSaveSQL, 0, 0);
            this.tableLayoutPanelTest.Controls.Add(this.textBoxSQL, 1, 0);
            this.tableLayoutPanelTest.Controls.Add(this.textBoxDescription, 1, 1);
            this.tableLayoutPanelTest.Controls.Add(this.labelDescription, 0, 1);
            this.tableLayoutPanelTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTest.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTest.Name = "tableLayoutPanelTest";
            this.tableLayoutPanelTest.RowCount = 5;
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTest.Size = new System.Drawing.Size(1005, 277);
            this.tableLayoutPanelTest.TabIndex = 0;
            // 
            // buttonTestCount
            // 
            this.buttonTestCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonTestCount.Location = new System.Drawing.Point(3, 120);
            this.buttonTestCount.Name = "buttonTestCount";
            this.buttonTestCount.Size = new System.Drawing.Size(75, 23);
            this.buttonTestCount.TabIndex = 0;
            this.buttonTestCount.Text = "Test Count";
            this.toolTip.SetToolTip(this.buttonTestCount, "Test the number of results");
            this.buttonTestCount.UseVisualStyleBackColor = true;
            this.buttonTestCount.Click += new System.EventHandler(this.buttonTestCount_Click);
            // 
            // labelTestCount
            // 
            this.labelTestCount.AutoSize = true;
            this.labelTestCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTestCount.Location = new System.Drawing.Point(84, 117);
            this.labelTestCount.Name = "labelTestCount";
            this.labelTestCount.Size = new System.Drawing.Size(103, 29);
            this.labelTestCount.TabIndex = 1;
            this.labelTestCount.Text = "Select count(*) from ";
            this.labelTestCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxTestCount
            // 
            this.textBoxTestCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTestCount.Location = new System.Drawing.Point(193, 120);
            this.textBoxTestCount.Name = "textBoxTestCount";
            this.textBoxTestCount.ReadOnly = true;
            this.textBoxTestCount.Size = new System.Drawing.Size(809, 20);
            this.textBoxTestCount.TabIndex = 2;
            // 
            // buttonTestQuery
            // 
            this.buttonTestQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonTestQuery.Location = new System.Drawing.Point(3, 149);
            this.buttonTestQuery.Name = "buttonTestQuery";
            this.tableLayoutPanelTest.SetRowSpan(this.buttonTestQuery, 2);
            this.buttonTestQuery.Size = new System.Drawing.Size(75, 23);
            this.buttonTestQuery.TabIndex = 3;
            this.buttonTestQuery.Text = "Test Query";
            this.toolTip.SetToolTip(this.buttonTestQuery, "Test the result of the query");
            this.buttonTestQuery.UseVisualStyleBackColor = true;
            this.buttonTestQuery.Click += new System.EventHandler(this.buttonTestQuery_Click);
            // 
            // labelTestQuery
            // 
            this.labelTestQuery.AutoSize = true;
            this.tableLayoutPanelTest.SetColumnSpan(this.labelTestQuery, 2);
            this.labelTestQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTestQuery.Location = new System.Drawing.Point(84, 146);
            this.labelTestQuery.Name = "labelTestQuery";
            this.labelTestQuery.Size = new System.Drawing.Size(918, 13);
            this.labelTestQuery.TabIndex = 4;
            this.labelTestQuery.Text = "Select * from";
            // 
            // dataGridViewQuery
            // 
            this.dataGridViewQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelTest.SetColumnSpan(this.dataGridViewQuery, 2);
            this.dataGridViewQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewQuery.Location = new System.Drawing.Point(84, 162);
            this.dataGridViewQuery.Name = "dataGridViewQuery";
            this.dataGridViewQuery.Size = new System.Drawing.Size(918, 112);
            this.dataGridViewQuery.TabIndex = 5;
            // 
            // buttonSaveSQL
            // 
            this.buttonSaveSQL.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSaveSQL.Image = global::DiversityWorkbench.Properties.Resources.Save;
            this.buttonSaveSQL.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSaveSQL.Location = new System.Drawing.Point(3, 3);
            this.buttonSaveSQL.Name = "buttonSaveSQL";
            this.buttonSaveSQL.Size = new System.Drawing.Size(75, 88);
            this.buttonSaveSQL.TabIndex = 6;
            this.buttonSaveSQL.Text = "Save changes in selected query";
            this.buttonSaveSQL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolTip.SetToolTip(this.buttonSaveSQL, "Save changes of the selected query (SQL and description)");
            this.buttonSaveSQL.UseVisualStyleBackColor = false;
            this.buttonSaveSQL.Click += new System.EventHandler(this.buttonSaveSQL_Click);
            // 
            // textBoxSQL
            // 
            this.tableLayoutPanelTest.SetColumnSpan(this.textBoxSQL, 2);
            this.textBoxSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSQL.Location = new System.Drawing.Point(84, 3);
            this.textBoxSQL.Multiline = true;
            this.textBoxSQL.Name = "textBoxSQL";
            this.textBoxSQL.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxSQL.Size = new System.Drawing.Size(918, 88);
            this.textBoxSQL.TabIndex = 7;
            this.textBoxSQL.TextChanged += new System.EventHandler(this.textBoxSQL_TextChanged);
            this.textBoxSQL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxSQL_KeyUp);
            // 
            // textBoxDescription
            // 
            this.tableLayoutPanelTest.SetColumnSpan(this.textBoxDescription, 2);
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(84, 97);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(918, 17);
            this.textBoxDescription.TabIndex = 8;
            this.textBoxDescription.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxDescription_KeyUp);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(3, 100);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 17);
            this.labelDescription.TabIndex = 9;
            this.labelDescription.Text = "Description:";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(844, 31);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Setting the search strings for the user";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonNewEntry
            // 
            this.buttonNewEntry.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonNewEntry.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonNewEntry.Location = new System.Drawing.Point(884, 3);
            this.buttonNewEntry.Name = "buttonNewEntry";
            this.buttonNewEntry.Size = new System.Drawing.Size(25, 25);
            this.buttonNewEntry.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonNewEntry, "Add a new query");
            this.buttonNewEntry.UseVisualStyleBackColor = true;
            this.buttonNewEntry.Click += new System.EventHandler(this.buttonNewEntry_Click);
            // 
            // buttonCopyQuery
            // 
            this.buttonCopyQuery.Image = global::DiversityWorkbench.Properties.Resources.Copy2;
            this.buttonCopyQuery.Location = new System.Drawing.Point(853, 3);
            this.buttonCopyQuery.Name = "buttonCopyQuery";
            this.buttonCopyQuery.Size = new System.Drawing.Size(25, 23);
            this.buttonCopyQuery.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonCopyQuery, "Copy the selected query");
            this.buttonCopyQuery.UseVisualStyleBackColor = true;
            this.buttonCopyQuery.Click += new System.EventHandler(this.buttonCopyQuery_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(977, 3);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(25, 23);
            this.buttonFeedback.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback to the developer");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(915, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(25, 25);
            this.buttonDelete.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete the selected query");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSaveAll
            // 
            this.buttonSaveAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveAll.Image = global::DiversityWorkbench.Properties.Resources.Save;
            this.buttonSaveAll.Location = new System.Drawing.Point(946, 3);
            this.buttonSaveAll.Name = "buttonSaveAll";
            this.buttonSaveAll.Size = new System.Drawing.Size(25, 25);
            this.buttonSaveAll.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonSaveAll, "Save all changes");
            this.buttonSaveAll.UseVisualStyleBackColor = true;
            this.buttonSaveAll.Click += new System.EventHandler(this.buttonSaveAll_Click);
            // 
            // userProxyTableAdapter
            // 
            this.userProxyTableAdapter.ClearBeforeFill = true;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 582);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(1005, 27);
            this.userControlDialogPanel.TabIndex = 2;
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.ColumnCount = 6;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelHeader.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonNewEntry, 2, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonCopyQuery, 1, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonFeedback, 5, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonDelete, 3, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.buttonSaveAll, 4, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 1;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(1005, 31);
            this.tableLayoutPanelHeader.TabIndex = 3;
            // 
            // FormApplicationSearchSelectionStrings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 609);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.tableLayoutPanelHeader);
            this.Controls.Add(this.userControlDialogPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormApplicationSearchSelectionStrings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Application Search Selection Strings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormApplicationSearchSelectionStrings_FormClosing);
            this.Load += new System.EventHandler(this.FormApplicationSearchSelectionStrings_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userProxyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetApplication)).EndInit();
            this.tableLayoutPanelTest.ResumeLayout(false);
            this.tableLayoutPanelTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewQuery)).EndInit();
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.DataGridView dataGridViewTable;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTest;
        private System.Windows.Forms.Button buttonTestCount;
        private System.Windows.Forms.Label labelTestCount;
        private System.Windows.Forms.TextBox textBoxTestCount;
        private System.Windows.Forms.Button buttonTestQuery;
        private System.Windows.Forms.Label labelTestQuery;
        private System.Windows.Forms.DataGridView dataGridViewQuery;
        private System.Windows.Forms.Button buttonSaveSQL;
        private System.Windows.Forms.TextBox textBoxSQL;
        private System.Windows.Forms.TextBox textBoxDescription;
        private Datasets.DataSetApplication dataSetApplication;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.BindingSource userProxyBindingSource;
        private DiversityWorkbench.Datasets.DataSetApplicationTableAdapters.UserProxyTableAdapter userProxyTableAdapter;
        private System.Windows.Forms.DataGridViewComboBoxColumn userNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sQLStringIdentifierDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemTableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sQLStringDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.Button buttonNewEntry;
        private System.Windows.Forms.Button buttonCopyQuery;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonSaveAll;
    }
}