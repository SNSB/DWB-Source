namespace DiversityCollection.CacheDatabase.Packages
{
    partial class FormPackageExport
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPackageExport));
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            labelHeader = new System.Windows.Forms.Label();
            listBoxViews = new System.Windows.Forms.ListBox();
            dataGridViewView = new System.Windows.Forms.DataGridView();
            buttonExportSqlite = new System.Windows.Forms.Button();
            buttonExportXML = new System.Windows.Forms.Button();
            textBoxSQLiteDatabase = new System.Windows.Forms.TextBox();
            progressBar = new System.Windows.Forms.ProgressBar();
            labelSQLiteDatabase = new System.Windows.Forms.Label();
            labelSQLiteTransferStep = new System.Windows.Forms.Label();
            labelSchema = new System.Windows.Forms.Label();
            comboBoxSchema = new System.Windows.Forms.ComboBox();
            buttonOpenDirectory = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewView).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 4;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.Controls.Add(labelHeader, 0, 0);
            tableLayoutPanelMain.Controls.Add(listBoxViews, 0, 3);
            tableLayoutPanelMain.Controls.Add(dataGridViewView, 1, 1);
            tableLayoutPanelMain.Controls.Add(buttonExportSqlite, 0, 5);
            tableLayoutPanelMain.Controls.Add(buttonExportXML, 0, 4);
            tableLayoutPanelMain.Controls.Add(textBoxSQLiteDatabase, 2, 5);
            tableLayoutPanelMain.Controls.Add(progressBar, 2, 6);
            tableLayoutPanelMain.Controls.Add(labelSQLiteDatabase, 1, 5);
            tableLayoutPanelMain.Controls.Add(labelSQLiteTransferStep, 0, 6);
            tableLayoutPanelMain.Controls.Add(labelSchema, 0, 1);
            tableLayoutPanelMain.Controls.Add(comboBoxSchema, 0, 2);
            tableLayoutPanelMain.Controls.Add(buttonOpenDirectory, 3, 0);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 7;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            tableLayoutPanelMain.Size = new System.Drawing.Size(797, 512);
            tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelHeader, 3);
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(4, 3);
            labelHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(766, 21);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "label1";
            // 
            // listBoxViews
            // 
            listBoxViews.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxViews.FormattingEnabled = true;
            listBoxViews.ItemHeight = 15;
            listBoxViews.Location = new System.Drawing.Point(4, 74);
            listBoxViews.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxViews.Name = "listBoxViews";
            listBoxViews.Size = new System.Drawing.Size(175, 355);
            listBoxViews.TabIndex = 1;
            listBoxViews.SelectedIndexChanged += listBoxViews_SelectedIndexChanged;
            // 
            // dataGridViewView
            // 
            dataGridViewView.AllowUserToAddRows = false;
            dataGridViewView.AllowUserToDeleteRows = false;
            dataGridViewView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanelMain.SetColumnSpan(dataGridViewView, 3);
            dataGridViewView.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewView.Location = new System.Drawing.Point(187, 30);
            dataGridViewView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGridViewView.Name = "dataGridViewView";
            dataGridViewView.ReadOnly = true;
            dataGridViewView.RowHeadersVisible = false;
            tableLayoutPanelMain.SetRowSpan(dataGridViewView, 4);
            dataGridViewView.Size = new System.Drawing.Size(606, 432);
            dataGridViewView.TabIndex = 2;
            // 
            // buttonExportSqlite
            // 
            buttonExportSqlite.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonExportSqlite.Image = Resource.SQLite;
            buttonExportSqlite.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonExportSqlite.Location = new System.Drawing.Point(4, 468);
            buttonExportSqlite.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonExportSqlite.Name = "buttonExportSqlite";
            buttonExportSqlite.Size = new System.Drawing.Size(175, 27);
            buttonExportSqlite.TabIndex = 3;
            buttonExportSqlite.Text = "Export to SQLite";
            buttonExportSqlite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonExportSqlite.UseVisualStyleBackColor = true;
            buttonExportSqlite.Click += buttonExportSqlite_Click;
            // 
            // buttonExportXML
            // 
            buttonExportXML.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonExportXML.Location = new System.Drawing.Point(4, 435);
            buttonExportXML.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonExportXML.Name = "buttonExportXML";
            buttonExportXML.Size = new System.Drawing.Size(175, 27);
            buttonExportXML.TabIndex = 4;
            buttonExportXML.Text = "Export to XML";
            buttonExportXML.UseVisualStyleBackColor = true;
            buttonExportXML.Click += buttonExportXML_Click;
            // 
            // textBoxSQLiteDatabase
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxSQLiteDatabase, 2);
            textBoxSQLiteDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxSQLiteDatabase.Location = new System.Drawing.Point(321, 468);
            textBoxSQLiteDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxSQLiteDatabase.Name = "textBoxSQLiteDatabase";
            textBoxSQLiteDatabase.ReadOnly = true;
            textBoxSQLiteDatabase.Size = new System.Drawing.Size(472, 23);
            textBoxSQLiteDatabase.TabIndex = 5;
            toolTip.SetToolTip(textBoxSQLiteDatabase, "The name of the database");
            // 
            // progressBar
            // 
            tableLayoutPanelMain.SetColumnSpan(progressBar, 2);
            progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBar.Location = new System.Drawing.Point(321, 498);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(472, 11);
            progressBar.TabIndex = 6;
            // 
            // labelSQLiteDatabase
            // 
            labelSQLiteDatabase.AutoSize = true;
            labelSQLiteDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSQLiteDatabase.Location = new System.Drawing.Point(187, 465);
            labelSQLiteDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSQLiteDatabase.Name = "labelSQLiteDatabase";
            labelSQLiteDatabase.Size = new System.Drawing.Size(126, 33);
            labelSQLiteDatabase.TabIndex = 7;
            labelSQLiteDatabase.Text = "Name of the datebase:";
            labelSQLiteDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSQLiteTransferStep
            // 
            labelSQLiteTransferStep.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelSQLiteTransferStep, 2);
            labelSQLiteTransferStep.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSQLiteTransferStep.Location = new System.Drawing.Point(4, 498);
            labelSQLiteTransferStep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSQLiteTransferStep.Name = "labelSQLiteTransferStep";
            labelSQLiteTransferStep.Size = new System.Drawing.Size(309, 14);
            labelSQLiteTransferStep.TabIndex = 8;
            labelSQLiteTransferStep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSchema
            // 
            labelSchema.AutoSize = true;
            labelSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSchema.Location = new System.Drawing.Point(4, 27);
            labelSchema.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSchema.Name = "labelSchema";
            labelSchema.Size = new System.Drawing.Size(175, 15);
            labelSchema.TabIndex = 9;
            labelSchema.Text = "Schema";
            // 
            // comboBoxSchema
            // 
            comboBoxSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxSchema.FormattingEnabled = true;
            comboBoxSchema.Location = new System.Drawing.Point(4, 45);
            comboBoxSchema.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxSchema.Name = "comboBoxSchema";
            comboBoxSchema.Size = new System.Drawing.Size(175, 23);
            comboBoxSchema.TabIndex = 10;
            toolTip.SetToolTip(comboBoxSchema, "The source schema for the export");
            comboBoxSchema.SelectionChangeCommitted += comboBoxSchema_SelectionChangeCommitted;
            // 
            // buttonOpenDirectory
            // 
            buttonOpenDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonOpenDirectory.FlatAppearance.BorderSize = 0;
            buttonOpenDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonOpenDirectory.Image = Resource.Open;
            buttonOpenDirectory.Location = new System.Drawing.Point(774, 0);
            buttonOpenDirectory.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonOpenDirectory.Name = "buttonOpenDirectory";
            buttonOpenDirectory.Size = new System.Drawing.Size(19, 27);
            buttonOpenDirectory.TabIndex = 11;
            toolTip.SetToolTip(buttonOpenDirectory, "Open export directory");
            buttonOpenDirectory.UseVisualStyleBackColor = true;
            buttonOpenDirectory.Click += buttonOpenDirectory_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // FormPackageExport
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(797, 512);
            Controls.Add(tableLayoutPanelMain);
            helpProvider.SetHelpKeyword(this, "cachedatabase_packages_dc#export");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormPackageExport";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Package export";
            KeyDown += Form_KeyDown;
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewView).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.ListBox listBoxViews;
        private System.Windows.Forms.DataGridView dataGridViewView;
        private System.Windows.Forms.Button buttonExportSqlite;
        private System.Windows.Forms.Button buttonExportXML;
        private System.Windows.Forms.TextBox textBoxSQLiteDatabase;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelSQLiteDatabase;
        private System.Windows.Forms.Label labelSQLiteTransferStep;
        private System.Windows.Forms.Label labelSchema;
        private System.Windows.Forms.ComboBox comboBoxSchema;
        private System.Windows.Forms.Button buttonOpenDirectory;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}