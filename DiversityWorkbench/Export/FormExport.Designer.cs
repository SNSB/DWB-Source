namespace DiversityWorkbench.Export
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExport));
            this.groupBoxSourceTables = new System.Windows.Forms.GroupBox();
            this.splitContainerSource = new System.Windows.Forms.SplitContainer();
            this.panelSourceTables = new System.Windows.Forms.Panel();
            this.groupBoxTableColumns = new System.Windows.Forms.GroupBox();
            this.panelSourceTableColumns = new System.Windows.Forms.Panel();
            this.labelColumnsOfTable = new System.Windows.Forms.Label();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.groupBoxFileColumns = new System.Windows.Forms.GroupBox();
            this.panelFileColumns = new System.Windows.Forms.Panel();
            this.tabControlSchemaResult = new System.Windows.Forms.TabControl();
            this.tabPageTest = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTest = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownTestExport = new System.Windows.Forms.NumericUpDown();
            this.buttonTestExport = new System.Windows.Forms.Button();
            this.labelTestExportLines = new System.Windows.Forms.Label();
            this.dataGridViewTestExport = new System.Windows.Forms.DataGridView();
            this.buttonShowTestExport = new System.Windows.Forms.Button();
            this.buttonShowTestExportSQL = new System.Windows.Forms.Button();
            this.tabPageExport = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelExport = new System.Windows.Forms.TableLayoutPanel();
            this.labelExportFileNameExtension = new System.Windows.Forms.Label();
            this.textBoxExportFileName = new System.Windows.Forms.TextBox();
            this.labelDirectoryForExortFile = new System.Windows.Forms.Label();
            this.buttonSetDirectoryForExortFile = new System.Windows.Forms.Button();
            this.buttonStartExport = new System.Windows.Forms.Button();
            this.checkBoxExportIncludeSchema = new System.Windows.Forms.CheckBox();
            this.buttonExportOpenFile = new System.Windows.Forms.Button();
            this.textBoxExport = new System.Windows.Forms.TextBox();
            this.progressBarExport = new System.Windows.Forms.ProgressBar();
            this.labelExportProgress = new System.Windows.Forms.Label();
            this.comboBoxExportFileNameExtension = new System.Windows.Forms.ComboBox();
            this.tabPageSQLite = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSQLite = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSQLiteExport = new System.Windows.Forms.Button();
            this.labelSQLiteDB = new System.Windows.Forms.Label();
            this.textBoxSQLiteDB = new System.Windows.Forms.TextBox();
            this.labelSQLiteDirectory = new System.Windows.Forms.Label();
            this.labelSQLiteExtension = new System.Windows.Forms.Label();
            this.progressBarSQLite = new System.Windows.Forms.ProgressBar();
            this.buttonSQLiteView = new System.Windows.Forms.Button();
            this.tabPageSchema = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSchema = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxSchema = new System.Windows.Forms.TextBox();
            this.buttonOpenSchema = new System.Windows.Forms.Button();
            this.buttonSaveSchema = new System.Windows.Forms.Button();
            this.webBrowserSchema = new System.Windows.Forms.WebBrowser();
            this.buttonShowSchema = new System.Windows.Forms.Button();
            this.buttonResetSchema = new System.Windows.Forms.Button();
            this.checkBoxSchemaDescription = new System.Windows.Forms.CheckBox();
            this.textBoxSchemaDescription = new System.Windows.Forms.TextBox();
            this.tabPageHeader = new System.Windows.Forms.TabPage();
            this.imageListTab = new System.Windows.Forms.ImageList(this.components);
            this.imageListSourceTables = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.checkBoxGetSql = new System.Windows.Forms.CheckBox();
            this.groupBoxSourceTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSource)).BeginInit();
            this.splitContainerSource.Panel1.SuspendLayout();
            this.splitContainerSource.Panel2.SuspendLayout();
            this.splitContainerSource.SuspendLayout();
            this.groupBoxTableColumns.SuspendLayout();
            this.groupBoxFileColumns.SuspendLayout();
            this.tabControlSchemaResult.SuspendLayout();
            this.tabPageTest.SuspendLayout();
            this.tableLayoutPanelTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTestExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTestExport)).BeginInit();
            this.tabPageExport.SuspendLayout();
            this.tableLayoutPanelExport.SuspendLayout();
            this.tabPageSQLite.SuspendLayout();
            this.tableLayoutPanelSQLite.SuspendLayout();
            this.tabPageSchema.SuspendLayout();
            this.tableLayoutPanelSchema.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSourceTables
            // 
            this.groupBoxSourceTables.Controls.Add(this.splitContainerSource);
            this.groupBoxSourceTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSourceTables.Location = new System.Drawing.Point(3, 3);
            this.groupBoxSourceTables.Name = "groupBoxSourceTables";
            this.groupBoxSourceTables.Size = new System.Drawing.Size(928, 152);
            this.groupBoxSourceTables.TabIndex = 1;
            this.groupBoxSourceTables.TabStop = false;
            this.groupBoxSourceTables.Text = "Tables";
            // 
            // splitContainerSource
            // 
            this.splitContainerSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSource.Location = new System.Drawing.Point(3, 16);
            this.splitContainerSource.Name = "splitContainerSource";
            // 
            // splitContainerSource.Panel1
            // 
            this.splitContainerSource.Panel1.Controls.Add(this.panelSourceTables);
            // 
            // splitContainerSource.Panel2
            // 
            this.splitContainerSource.Panel2.Controls.Add(this.groupBoxTableColumns);
            this.splitContainerSource.Panel2.Controls.Add(this.labelColumnsOfTable);
            this.splitContainerSource.Size = new System.Drawing.Size(922, 133);
            this.splitContainerSource.SplitterDistance = 481;
            this.splitContainerSource.TabIndex = 1;
            // 
            // panelSourceTables
            // 
            this.panelSourceTables.AutoScroll = true;
            this.panelSourceTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSourceTables.Location = new System.Drawing.Point(0, 0);
            this.panelSourceTables.Name = "panelSourceTables";
            this.panelSourceTables.Size = new System.Drawing.Size(481, 133);
            this.panelSourceTables.TabIndex = 0;
            // 
            // groupBoxTableColumns
            // 
            this.groupBoxTableColumns.Controls.Add(this.panelSourceTableColumns);
            this.groupBoxTableColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTableColumns.Location = new System.Drawing.Point(0, 13);
            this.groupBoxTableColumns.Name = "groupBoxTableColumns";
            this.groupBoxTableColumns.Size = new System.Drawing.Size(437, 120);
            this.groupBoxTableColumns.TabIndex = 1;
            this.groupBoxTableColumns.TabStop = false;
            this.groupBoxTableColumns.Text = "Columns";
            // 
            // panelSourceTableColumns
            // 
            this.panelSourceTableColumns.AutoScroll = true;
            this.panelSourceTableColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSourceTableColumns.Location = new System.Drawing.Point(3, 16);
            this.panelSourceTableColumns.Name = "panelSourceTableColumns";
            this.panelSourceTableColumns.Size = new System.Drawing.Size(431, 101);
            this.panelSourceTableColumns.TabIndex = 0;
            // 
            // labelColumnsOfTable
            // 
            this.labelColumnsOfTable.AutoSize = true;
            this.labelColumnsOfTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelColumnsOfTable.Location = new System.Drawing.Point(0, 0);
            this.labelColumnsOfTable.Name = "labelColumnsOfTable";
            this.labelColumnsOfTable.Size = new System.Drawing.Size(34, 13);
            this.labelColumnsOfTable.TabIndex = 2;
            this.labelColumnsOfTable.Text = "Table";
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFeedback.Image = global::DiversityWorkbench.Properties.Resources.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(908, 1);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(25, 23);
            this.buttonFeedback.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // groupBoxFileColumns
            // 
            this.groupBoxFileColumns.Controls.Add(this.panelFileColumns);
            this.groupBoxFileColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFileColumns.Location = new System.Drawing.Point(3, 161);
            this.groupBoxFileColumns.Name = "groupBoxFileColumns";
            this.groupBoxFileColumns.Size = new System.Drawing.Size(928, 166);
            this.groupBoxFileColumns.TabIndex = 0;
            this.groupBoxFileColumns.TabStop = false;
            this.groupBoxFileColumns.Text = "Columns in exported file";
            // 
            // panelFileColumns
            // 
            this.panelFileColumns.AutoScroll = true;
            this.panelFileColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFileColumns.Location = new System.Drawing.Point(3, 16);
            this.panelFileColumns.Margin = new System.Windows.Forms.Padding(0);
            this.panelFileColumns.Name = "panelFileColumns";
            this.panelFileColumns.Size = new System.Drawing.Size(922, 147);
            this.panelFileColumns.TabIndex = 0;
            // 
            // tabControlSchemaResult
            // 
            this.tabControlSchemaResult.Controls.Add(this.tabPageTest);
            this.tabControlSchemaResult.Controls.Add(this.tabPageExport);
            this.tabControlSchemaResult.Controls.Add(this.tabPageSQLite);
            this.tabControlSchemaResult.Controls.Add(this.tabPageSchema);
            this.tabControlSchemaResult.Controls.Add(this.tabPageHeader);
            this.tabControlSchemaResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSchemaResult.ImageList = this.imageListTab;
            this.tabControlSchemaResult.Location = new System.Drawing.Point(0, 0);
            this.tabControlSchemaResult.Name = "tabControlSchemaResult";
            this.tabControlSchemaResult.SelectedIndex = 0;
            this.tabControlSchemaResult.Size = new System.Drawing.Size(934, 231);
            this.tabControlSchemaResult.TabIndex = 0;
            // 
            // tabPageTest
            // 
            this.tabPageTest.Controls.Add(this.tableLayoutPanelTest);
            this.tabPageTest.ImageIndex = 0;
            this.tabPageTest.Location = new System.Drawing.Point(4, 23);
            this.tabPageTest.Name = "tabPageTest";
            this.tabPageTest.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTest.Size = new System.Drawing.Size(926, 204);
            this.tabPageTest.TabIndex = 1;
            this.tabPageTest.Text = "Test";
            this.tabPageTest.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTest
            // 
            this.tableLayoutPanelTest.ColumnCount = 4;
            this.tableLayoutPanelTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTest.Controls.Add(this.numericUpDownTestExport, 0, 1);
            this.tableLayoutPanelTest.Controls.Add(this.buttonTestExport, 0, 2);
            this.tableLayoutPanelTest.Controls.Add(this.labelTestExportLines, 0, 0);
            this.tableLayoutPanelTest.Controls.Add(this.dataGridViewTestExport, 3, 0);
            this.tableLayoutPanelTest.Controls.Add(this.buttonShowTestExport, 2, 3);
            this.tableLayoutPanelTest.Controls.Add(this.buttonShowTestExportSQL, 0, 3);
            this.tableLayoutPanelTest.Controls.Add(this.checkBoxGetSql, 1, 3);
            this.tableLayoutPanelTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTest.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelTest.Name = "tableLayoutPanelTest";
            this.tableLayoutPanelTest.RowCount = 4;
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelTest.Size = new System.Drawing.Size(920, 198);
            this.tableLayoutPanelTest.TabIndex = 0;
            this.toolTip.SetToolTip(this.tableLayoutPanelTest, "Show SQL of current test");
            // 
            // numericUpDownTestExport
            // 
            this.tableLayoutPanelTest.SetColumnSpan(this.numericUpDownTestExport, 3);
            this.numericUpDownTestExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownTestExport.Location = new System.Drawing.Point(3, 13);
            this.numericUpDownTestExport.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.numericUpDownTestExport.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTestExport.Name = "numericUpDownTestExport";
            this.numericUpDownTestExport.Size = new System.Drawing.Size(96, 20);
            this.numericUpDownTestExport.TabIndex = 1;
            this.numericUpDownTestExport.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numericUpDownTestExport, "The number of lines for the test of the export");
            this.numericUpDownTestExport.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonTestExport
            // 
            this.tableLayoutPanelTest.SetColumnSpan(this.buttonTestExport, 3);
            this.buttonTestExport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonTestExport.Image = global::DiversityWorkbench.Properties.Resources.ExportTest;
            this.buttonTestExport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonTestExport.Location = new System.Drawing.Point(3, 151);
            this.buttonTestExport.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonTestExport.Name = "buttonTestExport";
            this.buttonTestExport.Size = new System.Drawing.Size(96, 23);
            this.buttonTestExport.TabIndex = 0;
            this.buttonTestExport.Text = "Test export";
            this.buttonTestExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonTestExport, "Test the export of the data");
            this.buttonTestExport.UseVisualStyleBackColor = true;
            this.buttonTestExport.Click += new System.EventHandler(this.buttonTestExport_Click);
            // 
            // labelTestExportLines
            // 
            this.labelTestExportLines.AutoSize = true;
            this.tableLayoutPanelTest.SetColumnSpan(this.labelTestExportLines, 3);
            this.labelTestExportLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTestExportLines.Location = new System.Drawing.Point(0, 0);
            this.labelTestExportLines.Margin = new System.Windows.Forms.Padding(0);
            this.labelTestExportLines.Name = "labelTestExportLines";
            this.labelTestExportLines.Size = new System.Drawing.Size(102, 13);
            this.labelTestExportLines.TabIndex = 2;
            this.labelTestExportLines.Text = "Lines for test export:";
            this.labelTestExportLines.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataGridViewTestExport
            // 
            this.dataGridViewTestExport.AllowUserToAddRows = false;
            this.dataGridViewTestExport.AllowUserToDeleteRows = false;
            this.dataGridViewTestExport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTestExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTestExport.Location = new System.Drawing.Point(102, 0);
            this.dataGridViewTestExport.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewTestExport.Name = "dataGridViewTestExport";
            this.dataGridViewTestExport.ReadOnly = true;
            this.dataGridViewTestExport.RowHeadersVisible = false;
            this.tableLayoutPanelTest.SetRowSpan(this.dataGridViewTestExport, 4);
            this.dataGridViewTestExport.Size = new System.Drawing.Size(818, 198);
            this.dataGridViewTestExport.TabIndex = 3;
            // 
            // buttonShowTestExport
            // 
            this.buttonShowTestExport.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonShowTestExport.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonShowTestExport.Location = new System.Drawing.Point(73, 174);
            this.buttonShowTestExport.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonShowTestExport.Name = "buttonShowTestExport";
            this.buttonShowTestExport.Size = new System.Drawing.Size(26, 24);
            this.buttonShowTestExport.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonShowTestExport, "Show test result in separate window");
            this.buttonShowTestExport.UseVisualStyleBackColor = true;
            this.buttonShowTestExport.Click += new System.EventHandler(this.buttonShowTestExport_Click);
            // 
            // buttonShowTestExportSQL
            // 
            this.buttonShowTestExportSQL.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonShowTestExportSQL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonShowTestExportSQL.Location = new System.Drawing.Point(3, 174);
            this.buttonShowTestExportSQL.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonShowTestExportSQL.Name = "buttonShowTestExportSQL";
            this.buttonShowTestExportSQL.Size = new System.Drawing.Size(43, 24);
            this.buttonShowTestExportSQL.TabIndex = 5;
            this.buttonShowTestExportSQL.Text = "SQL";
            this.buttonShowTestExportSQL.UseVisualStyleBackColor = true;
            this.buttonShowTestExportSQL.Click += new System.EventHandler(this.buttonShowTestExportSQL_Click);
            // 
            // tabPageExport
            // 
            this.tabPageExport.Controls.Add(this.tableLayoutPanelExport);
            this.tabPageExport.ImageIndex = 1;
            this.tabPageExport.Location = new System.Drawing.Point(4, 23);
            this.tabPageExport.Name = "tabPageExport";
            this.tabPageExport.Size = new System.Drawing.Size(926, 204);
            this.tabPageExport.TabIndex = 3;
            this.tabPageExport.Text = "Export";
            this.tabPageExport.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelExport
            // 
            this.tableLayoutPanelExport.ColumnCount = 7;
            this.tableLayoutPanelExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExport.Controls.Add(this.labelExportFileNameExtension, 4, 0);
            this.tableLayoutPanelExport.Controls.Add(this.textBoxExportFileName, 2, 0);
            this.tableLayoutPanelExport.Controls.Add(this.labelDirectoryForExortFile, 1, 0);
            this.tableLayoutPanelExport.Controls.Add(this.buttonSetDirectoryForExortFile, 0, 0);
            this.tableLayoutPanelExport.Controls.Add(this.buttonStartExport, 6, 0);
            this.tableLayoutPanelExport.Controls.Add(this.checkBoxExportIncludeSchema, 5, 0);
            this.tableLayoutPanelExport.Controls.Add(this.buttonExportOpenFile, 6, 3);
            this.tableLayoutPanelExport.Controls.Add(this.textBoxExport, 0, 1);
            this.tableLayoutPanelExport.Controls.Add(this.progressBarExport, 0, 4);
            this.tableLayoutPanelExport.Controls.Add(this.labelExportProgress, 0, 3);
            this.tableLayoutPanelExport.Controls.Add(this.comboBoxExportFileNameExtension, 3, 0);
            this.tableLayoutPanelExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelExport.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelExport.Name = "tableLayoutPanelExport";
            this.tableLayoutPanelExport.RowCount = 5;
            this.tableLayoutPanelExport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelExport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelExport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelExport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelExport.Size = new System.Drawing.Size(926, 204);
            this.tableLayoutPanelExport.TabIndex = 0;
            // 
            // labelExportFileNameExtension
            // 
            this.labelExportFileNameExtension.AutoSize = true;
            this.labelExportFileNameExtension.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExportFileNameExtension.Location = new System.Drawing.Point(326, 0);
            this.labelExportFileNameExtension.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelExportFileNameExtension.Name = "labelExportFileNameExtension";
            this.labelExportFileNameExtension.Size = new System.Drawing.Size(404, 29);
            this.labelExportFileNameExtension.TabIndex = 8;
            this.labelExportFileNameExtension.Text = ".txt";
            this.labelExportFileNameExtension.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelExportFileNameExtension.Visible = false;
            // 
            // textBoxExportFileName
            // 
            this.textBoxExportFileName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExportFileName.Location = new System.Drawing.Point(80, 3);
            this.textBoxExportFileName.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxExportFileName.Name = "textBoxExportFileName";
            this.textBoxExportFileName.Size = new System.Drawing.Size(200, 20);
            this.textBoxExportFileName.TabIndex = 3;
            this.textBoxExportFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.textBoxExportFileName, "The name of the exported file");
            // 
            // labelDirectoryForExortFile
            // 
            this.labelDirectoryForExortFile.AutoSize = true;
            this.labelDirectoryForExortFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDirectoryForExortFile.Location = new System.Drawing.Point(31, 0);
            this.labelDirectoryForExortFile.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDirectoryForExortFile.Name = "labelDirectoryForExortFile";
            this.labelDirectoryForExortFile.Size = new System.Drawing.Size(49, 29);
            this.labelDirectoryForExortFile.TabIndex = 5;
            this.labelDirectoryForExortFile.Text = "Directory";
            this.labelDirectoryForExortFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.labelDirectoryForExortFile, "The directory where the file will be exported to");
            // 
            // buttonSetDirectoryForExortFile
            // 
            this.buttonSetDirectoryForExortFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSetDirectoryForExortFile.Image = global::DiversityWorkbench.Properties.Resources.OpenFolder;
            this.buttonSetDirectoryForExortFile.Location = new System.Drawing.Point(3, 3);
            this.buttonSetDirectoryForExortFile.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.buttonSetDirectoryForExortFile.Name = "buttonSetDirectoryForExortFile";
            this.buttonSetDirectoryForExortFile.Size = new System.Drawing.Size(25, 23);
            this.buttonSetDirectoryForExortFile.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonSetDirectoryForExortFile, "Set the directory where the exported file should be placed");
            this.buttonSetDirectoryForExortFile.UseVisualStyleBackColor = true;
            this.buttonSetDirectoryForExortFile.Click += new System.EventHandler(this.buttonSetDirectoryForExortFile_Click);
            // 
            // buttonStartExport
            // 
            this.buttonStartExport.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonStartExport.Image = global::DiversityWorkbench.Properties.Resources.ExportWizard;
            this.buttonStartExport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStartExport.Location = new System.Drawing.Point(840, 0);
            this.buttonStartExport.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStartExport.Name = "buttonStartExport";
            this.buttonStartExport.Size = new System.Drawing.Size(86, 29);
            this.buttonStartExport.TabIndex = 4;
            this.buttonStartExport.Text = "Export data";
            this.buttonStartExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonStartExport, "Start the export into the file");
            this.buttonStartExport.UseVisualStyleBackColor = true;
            this.buttonStartExport.Click += new System.EventHandler(this.buttonStartExport_Click);
            // 
            // checkBoxExportIncludeSchema
            // 
            this.checkBoxExportIncludeSchema.AutoSize = true;
            this.checkBoxExportIncludeSchema.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxExportIncludeSchema.Location = new System.Drawing.Point(736, 9);
            this.checkBoxExportIncludeSchema.Name = "checkBoxExportIncludeSchema";
            this.checkBoxExportIncludeSchema.Size = new System.Drawing.Size(101, 17);
            this.checkBoxExportIncludeSchema.TabIndex = 9;
            this.checkBoxExportIncludeSchema.Text = "Include schema";
            this.toolTip.SetToolTip(this.checkBoxExportIncludeSchema, "Create a schema file along with the export");
            this.checkBoxExportIncludeSchema.UseVisualStyleBackColor = true;
            // 
            // buttonExportOpenFile
            // 
            this.buttonExportOpenFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExportOpenFile.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonExportOpenFile.Location = new System.Drawing.Point(902, 179);
            this.buttonExportOpenFile.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExportOpenFile.Name = "buttonExportOpenFile";
            this.tableLayoutPanelExport.SetRowSpan(this.buttonExportOpenFile, 2);
            this.buttonExportOpenFile.Size = new System.Drawing.Size(24, 25);
            this.buttonExportOpenFile.TabIndex = 10;
            this.toolTip.SetToolTip(this.buttonExportOpenFile, "Open the exported file");
            this.buttonExportOpenFile.UseVisualStyleBackColor = true;
            this.buttonExportOpenFile.Click += new System.EventHandler(this.buttonExportOpenFile_Click);
            // 
            // textBoxExport
            // 
            this.tableLayoutPanelExport.SetColumnSpan(this.textBoxExport, 7);
            this.textBoxExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExport.Location = new System.Drawing.Point(3, 32);
            this.textBoxExport.Multiline = true;
            this.textBoxExport.Name = "textBoxExport";
            this.textBoxExport.ReadOnly = true;
            this.tableLayoutPanelExport.SetRowSpan(this.textBoxExport, 2);
            this.textBoxExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExport.Size = new System.Drawing.Size(920, 144);
            this.textBoxExport.TabIndex = 11;
            // 
            // progressBarExport
            // 
            this.tableLayoutPanelExport.SetColumnSpan(this.progressBarExport, 6);
            this.progressBarExport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarExport.Location = new System.Drawing.Point(3, 195);
            this.progressBarExport.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.progressBarExport.Name = "progressBarExport";
            this.progressBarExport.Size = new System.Drawing.Size(834, 9);
            this.progressBarExport.TabIndex = 12;
            // 
            // labelExportProgress
            // 
            this.labelExportProgress.AutoSize = true;
            this.tableLayoutPanelExport.SetColumnSpan(this.labelExportProgress, 5);
            this.labelExportProgress.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelExportProgress.Location = new System.Drawing.Point(3, 179);
            this.labelExportProgress.Name = "labelExportProgress";
            this.labelExportProgress.Size = new System.Drawing.Size(48, 13);
            this.labelExportProgress.TabIndex = 13;
            this.labelExportProgress.Text = "Progress";
            this.labelExportProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxExportFileNameExtension
            // 
            this.comboBoxExportFileNameExtension.FormattingEnabled = true;
            this.comboBoxExportFileNameExtension.Items.AddRange(new object[] {
            ".txt",
            ".xml"});
            this.comboBoxExportFileNameExtension.Location = new System.Drawing.Point(283, 3);
            this.comboBoxExportFileNameExtension.Name = "comboBoxExportFileNameExtension";
            this.comboBoxExportFileNameExtension.Size = new System.Drawing.Size(40, 21);
            this.comboBoxExportFileNameExtension.TabIndex = 14;
            this.comboBoxExportFileNameExtension.Text = ".txt";
            // 
            // tabPageSQLite
            // 
            this.tabPageSQLite.Controls.Add(this.tableLayoutPanelSQLite);
            this.tabPageSQLite.ImageIndex = 3;
            this.tabPageSQLite.Location = new System.Drawing.Point(4, 23);
            this.tabPageSQLite.Name = "tabPageSQLite";
            this.tabPageSQLite.Size = new System.Drawing.Size(926, 204);
            this.tabPageSQLite.TabIndex = 4;
            this.tabPageSQLite.Text = "Export to SQLite";
            this.tabPageSQLite.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSQLite
            // 
            this.tableLayoutPanelSQLite.ColumnCount = 4;
            this.tableLayoutPanelSQLite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSQLite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSQLite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSQLite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSQLite.Controls.Add(this.buttonSQLiteExport, 0, 1);
            this.tableLayoutPanelSQLite.Controls.Add(this.labelSQLiteDB, 0, 0);
            this.tableLayoutPanelSQLite.Controls.Add(this.textBoxSQLiteDB, 2, 0);
            this.tableLayoutPanelSQLite.Controls.Add(this.labelSQLiteDirectory, 1, 0);
            this.tableLayoutPanelSQLite.Controls.Add(this.labelSQLiteExtension, 3, 0);
            this.tableLayoutPanelSQLite.Controls.Add(this.progressBarSQLite, 0, 2);
            this.tableLayoutPanelSQLite.Controls.Add(this.buttonSQLiteView, 3, 1);
            this.tableLayoutPanelSQLite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSQLite.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSQLite.Name = "tableLayoutPanelSQLite";
            this.tableLayoutPanelSQLite.RowCount = 3;
            this.tableLayoutPanelSQLite.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSQLite.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSQLite.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSQLite.Size = new System.Drawing.Size(926, 204);
            this.tableLayoutPanelSQLite.TabIndex = 0;
            // 
            // buttonSQLiteExport
            // 
            this.tableLayoutPanelSQLite.SetColumnSpan(this.buttonSQLiteExport, 3);
            this.buttonSQLiteExport.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSQLiteExport.Image = global::DiversityWorkbench.Properties.Resources.SQLite;
            this.buttonSQLiteExport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSQLiteExport.Location = new System.Drawing.Point(3, 29);
            this.buttonSQLiteExport.Name = "buttonSQLiteExport";
            this.buttonSQLiteExport.Size = new System.Drawing.Size(86, 24);
            this.buttonSQLiteExport.TabIndex = 0;
            this.buttonSQLiteExport.Text = "Export data";
            this.buttonSQLiteExport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSQLiteExport.UseVisualStyleBackColor = true;
            this.buttonSQLiteExport.Click += new System.EventHandler(this.buttonSQLiteExport_Click);
            // 
            // labelSQLiteDB
            // 
            this.labelSQLiteDB.AutoSize = true;
            this.labelSQLiteDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSQLiteDB.Location = new System.Drawing.Point(3, 0);
            this.labelSQLiteDB.Name = "labelSQLiteDB";
            this.labelSQLiteDB.Size = new System.Drawing.Size(56, 26);
            this.labelSQLiteDB.TabIndex = 1;
            this.labelSQLiteDB.Text = "Database:";
            this.labelSQLiteDB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxSQLiteDB
            // 
            this.textBoxSQLiteDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSQLiteDB.Location = new System.Drawing.Point(96, 3);
            this.textBoxSQLiteDB.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxSQLiteDB.Name = "textBoxSQLiteDB";
            this.textBoxSQLiteDB.Size = new System.Drawing.Size(100, 20);
            this.textBoxSQLiteDB.TabIndex = 2;
            this.textBoxSQLiteDB.Text = "DWBexport";
            this.textBoxSQLiteDB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxSQLiteDB.TextChanged += new System.EventHandler(this.textBoxSQLiteDB_TextChanged);
            // 
            // labelSQLiteDirectory
            // 
            this.labelSQLiteDirectory.AutoSize = true;
            this.labelSQLiteDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSQLiteDirectory.Location = new System.Drawing.Point(65, 0);
            this.labelSQLiteDirectory.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSQLiteDirectory.Name = "labelSQLiteDirectory";
            this.labelSQLiteDirectory.Size = new System.Drawing.Size(31, 26);
            this.labelSQLiteDirectory.TabIndex = 4;
            this.labelSQLiteDirectory.Text = "C:/...";
            this.labelSQLiteDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSQLiteExtension
            // 
            this.labelSQLiteExtension.AutoSize = true;
            this.labelSQLiteExtension.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSQLiteExtension.Location = new System.Drawing.Point(196, 0);
            this.labelSQLiteExtension.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelSQLiteExtension.Name = "labelSQLiteExtension";
            this.labelSQLiteExtension.Size = new System.Drawing.Size(34, 26);
            this.labelSQLiteExtension.TabIndex = 5;
            this.labelSQLiteExtension.Text = ".sqlite";
            this.labelSQLiteExtension.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBarSQLite
            // 
            this.tableLayoutPanelSQLite.SetColumnSpan(this.progressBarSQLite, 4);
            this.progressBarSQLite.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBarSQLite.Location = new System.Drawing.Point(3, 191);
            this.progressBarSQLite.Name = "progressBarSQLite";
            this.progressBarSQLite.Size = new System.Drawing.Size(920, 10);
            this.progressBarSQLite.TabIndex = 6;
            // 
            // buttonSQLiteView
            // 
            this.buttonSQLiteView.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSQLiteView.Enabled = false;
            this.buttonSQLiteView.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonSQLiteView.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSQLiteView.Location = new System.Drawing.Point(199, 29);
            this.buttonSQLiteView.Name = "buttonSQLiteView";
            this.buttonSQLiteView.Size = new System.Drawing.Size(86, 24);
            this.buttonSQLiteView.TabIndex = 3;
            this.buttonSQLiteView.Text = "View export";
            this.buttonSQLiteView.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSQLiteView.UseVisualStyleBackColor = true;
            this.buttonSQLiteView.Click += new System.EventHandler(this.buttonSQLiteView_Click);
            // 
            // tabPageSchema
            // 
            this.tabPageSchema.Controls.Add(this.tableLayoutPanelSchema);
            this.tabPageSchema.ImageIndex = 2;
            this.tabPageSchema.Location = new System.Drawing.Point(4, 23);
            this.tabPageSchema.Name = "tabPageSchema";
            this.tabPageSchema.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSchema.Size = new System.Drawing.Size(926, 204);
            this.tabPageSchema.TabIndex = 0;
            this.tabPageSchema.Text = "Schema";
            this.tabPageSchema.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSchema
            // 
            this.tableLayoutPanelSchema.ColumnCount = 5;
            this.tableLayoutPanelSchema.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanelSchema.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelSchema.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelSchema.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSchema.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSchema.Controls.Add(this.textBoxSchema, 3, 0);
            this.tableLayoutPanelSchema.Controls.Add(this.buttonOpenSchema, 0, 0);
            this.tableLayoutPanelSchema.Controls.Add(this.buttonSaveSchema, 4, 0);
            this.tableLayoutPanelSchema.Controls.Add(this.webBrowserSchema, 3, 1);
            this.tableLayoutPanelSchema.Controls.Add(this.buttonShowSchema, 2, 0);
            this.tableLayoutPanelSchema.Controls.Add(this.buttonResetSchema, 1, 0);
            this.tableLayoutPanelSchema.Controls.Add(this.checkBoxSchemaDescription, 0, 1);
            this.tableLayoutPanelSchema.Controls.Add(this.textBoxSchemaDescription, 0, 2);
            this.tableLayoutPanelSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSchema.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelSchema.Name = "tableLayoutPanelSchema";
            this.tableLayoutPanelSchema.RowCount = 3;
            this.tableLayoutPanelSchema.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSchema.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSchema.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSchema.Size = new System.Drawing.Size(920, 198);
            this.tableLayoutPanelSchema.TabIndex = 0;
            // 
            // textBoxSchema
            // 
            this.textBoxSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSchema.Location = new System.Drawing.Point(78, 3);
            this.textBoxSchema.Name = "textBoxSchema";
            this.textBoxSchema.Size = new System.Drawing.Size(809, 20);
            this.textBoxSchema.TabIndex = 0;
            // 
            // buttonOpenSchema
            // 
            this.buttonOpenSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenSchema.Image = global::DiversityWorkbench.ResourceWorkbench.Open;
            this.buttonOpenSchema.Location = new System.Drawing.Point(3, 3);
            this.buttonOpenSchema.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.buttonOpenSchema.Name = "buttonOpenSchema";
            this.buttonOpenSchema.Size = new System.Drawing.Size(24, 24);
            this.buttonOpenSchema.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonOpenSchema, "Open an existing schema");
            this.buttonOpenSchema.UseVisualStyleBackColor = true;
            this.buttonOpenSchema.Click += new System.EventHandler(this.buttonOpenSchema_Click);
            // 
            // buttonSaveSchema
            // 
            this.buttonSaveSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveSchema.Image = global::DiversityWorkbench.ResourceWorkbench.Save;
            this.buttonSaveSchema.Location = new System.Drawing.Point(893, 3);
            this.buttonSaveSchema.Name = "buttonSaveSchema";
            this.buttonSaveSchema.Size = new System.Drawing.Size(24, 24);
            this.buttonSaveSchema.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonSaveSchema, "Save the current settings as a schema");
            this.buttonSaveSchema.UseVisualStyleBackColor = true;
            this.buttonSaveSchema.Click += new System.EventHandler(this.buttonSaveSchema_Click);
            // 
            // webBrowserSchema
            // 
            this.tableLayoutPanelSchema.SetColumnSpan(this.webBrowserSchema, 2);
            this.webBrowserSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserSchema.Location = new System.Drawing.Point(78, 33);
            this.webBrowserSchema.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserSchema.Name = "webBrowserSchema";
            this.tableLayoutPanelSchema.SetRowSpan(this.webBrowserSchema, 2);
            this.webBrowserSchema.Size = new System.Drawing.Size(839, 162);
            this.webBrowserSchema.TabIndex = 3;
            // 
            // buttonShowSchema
            // 
            this.buttonShowSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowSchema.Image = global::DiversityWorkbench.Properties.Resources.Lupe;
            this.buttonShowSchema.Location = new System.Drawing.Point(51, 3);
            this.buttonShowSchema.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonShowSchema.Name = "buttonShowSchema";
            this.buttonShowSchema.Size = new System.Drawing.Size(24, 24);
            this.buttonShowSchema.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonShowSchema, "Open the schema in a separate window");
            this.buttonShowSchema.UseVisualStyleBackColor = true;
            this.buttonShowSchema.Click += new System.EventHandler(this.buttonShowSchema_Click);
            // 
            // buttonResetSchema
            // 
            this.buttonResetSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonResetSchema.Image = global::DiversityWorkbench.ResourceWorkbench.Undo;
            this.buttonResetSchema.Location = new System.Drawing.Point(27, 3);
            this.buttonResetSchema.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonResetSchema.Name = "buttonResetSchema";
            this.buttonResetSchema.Size = new System.Drawing.Size(24, 24);
            this.buttonResetSchema.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonResetSchema, "Reset to default");
            this.buttonResetSchema.UseVisualStyleBackColor = true;
            this.buttonResetSchema.Click += new System.EventHandler(this.buttonResetSchema_Click);
            // 
            // checkBoxSchemaDescription
            // 
            this.checkBoxSchemaDescription.AutoSize = true;
            this.tableLayoutPanelSchema.SetColumnSpan(this.checkBoxSchemaDescription, 3);
            this.checkBoxSchemaDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxSchemaDescription.Enabled = false;
            this.checkBoxSchemaDescription.Location = new System.Drawing.Point(3, 33);
            this.checkBoxSchemaDescription.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.checkBoxSchemaDescription.Name = "checkBoxSchemaDescription";
            this.checkBoxSchemaDescription.Size = new System.Drawing.Size(72, 17);
            this.checkBoxSchemaDescription.TabIndex = 6;
            this.checkBoxSchemaDescription.Text = "In. desc.";
            this.toolTip.SetToolTip(this.checkBoxSchemaDescription, "Include description in the schema file");
            this.checkBoxSchemaDescription.UseVisualStyleBackColor = true;
            // 
            // textBoxSchemaDescription
            // 
            this.tableLayoutPanelSchema.SetColumnSpan(this.textBoxSchemaDescription, 3);
            this.textBoxSchemaDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSchemaDescription.Enabled = false;
            this.textBoxSchemaDescription.Location = new System.Drawing.Point(3, 50);
            this.textBoxSchemaDescription.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxSchemaDescription.Multiline = true;
            this.textBoxSchemaDescription.Name = "textBoxSchemaDescription";
            this.textBoxSchemaDescription.Size = new System.Drawing.Size(69, 145);
            this.textBoxSchemaDescription.TabIndex = 7;
            this.toolTip.SetToolTip(this.textBoxSchemaDescription, "the description of the schema");
            // 
            // tabPageHeader
            // 
            this.tabPageHeader.Location = new System.Drawing.Point(4, 23);
            this.tabPageHeader.Name = "tabPageHeader";
            this.tabPageHeader.Size = new System.Drawing.Size(926, 204);
            this.tabPageHeader.TabIndex = 2;
            this.tabPageHeader.Text = "Header";
            this.tabPageHeader.UseVisualStyleBackColor = true;
            // 
            // imageListTab
            // 
            this.imageListTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTab.ImageStream")));
            this.imageListTab.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTab.Images.SetKeyName(0, "ExportTest.ico");
            this.imageListTab.Images.SetKeyName(1, "ExportWizard.ico");
            this.imageListTab.Images.SetKeyName(2, "Settings.ico");
            this.imageListTab.Images.SetKeyName(3, "SQLite.ico");
            // 
            // imageListSourceTables
            // 
            this.imageListSourceTables.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListSourceTables.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListSourceTables.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxSourceTables, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxFileColumns, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(934, 330);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanelMain);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tabControlSchemaResult);
            this.splitContainerMain.Size = new System.Drawing.Size(934, 565);
            this.splitContainerMain.SplitterDistance = 330;
            this.splitContainerMain.TabIndex = 2;
            // 
            // checkBoxGetSql
            // 
            this.checkBoxGetSql.AutoSize = true;
            this.checkBoxGetSql.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxGetSql.Location = new System.Drawing.Point(49, 177);
            this.checkBoxGetSql.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.checkBoxGetSql.Name = "checkBoxGetSql";
            this.checkBoxGetSql.Size = new System.Drawing.Size(15, 18);
            this.checkBoxGetSql.TabIndex = 6;
            this.toolTip.SetToolTip(this.checkBoxGetSql, "If SQL commands should be documented");
            this.checkBoxGetSql.UseVisualStyleBackColor = true;
            this.checkBoxGetSql.CheckedChanged += new System.EventHandler(this.checkBoxGetSql_CheckedChanged);
            // 
            // FormExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 565);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.splitContainerMain);
            this.helpProvider.SetHelpKeyword(this, "Export wizard");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExport";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export";
            this.SizeChanged += new System.EventHandler(this.FormExport_SizeChanged);
            this.groupBoxSourceTables.ResumeLayout(false);
            this.splitContainerSource.Panel1.ResumeLayout(false);
            this.splitContainerSource.Panel2.ResumeLayout(false);
            this.splitContainerSource.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSource)).EndInit();
            this.splitContainerSource.ResumeLayout(false);
            this.groupBoxTableColumns.ResumeLayout(false);
            this.groupBoxFileColumns.ResumeLayout(false);
            this.tabControlSchemaResult.ResumeLayout(false);
            this.tabPageTest.ResumeLayout(false);
            this.tableLayoutPanelTest.ResumeLayout(false);
            this.tableLayoutPanelTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTestExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTestExport)).EndInit();
            this.tabPageExport.ResumeLayout(false);
            this.tableLayoutPanelExport.ResumeLayout(false);
            this.tableLayoutPanelExport.PerformLayout();
            this.tabPageSQLite.ResumeLayout(false);
            this.tableLayoutPanelSQLite.ResumeLayout(false);
            this.tableLayoutPanelSQLite.PerformLayout();
            this.tabPageSchema.ResumeLayout(false);
            this.tableLayoutPanelSchema.ResumeLayout(false);
            this.tableLayoutPanelSchema.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListSourceTables;
        private System.Windows.Forms.Panel panelSourceTables;
        private System.Windows.Forms.SplitContainer splitContainerSource;
        private System.Windows.Forms.Panel panelSourceTableColumns;
        private System.Windows.Forms.GroupBox groupBoxTableColumns;
        private System.Windows.Forms.GroupBox groupBoxSourceTables;
        private System.Windows.Forms.TabControl tabControlSchemaResult;
        private System.Windows.Forms.TabPage tabPageSchema;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSchema;
        private System.Windows.Forms.TextBox textBoxSchema;
        private System.Windows.Forms.Button buttonOpenSchema;
        private System.Windows.Forms.Button buttonSaveSchema;
        private System.Windows.Forms.WebBrowser webBrowserSchema;
        private System.Windows.Forms.TabPage tabPageTest;
        private System.Windows.Forms.GroupBox groupBoxFileColumns;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTest;
        private System.Windows.Forms.NumericUpDown numericUpDownTestExport;
        private System.Windows.Forms.Button buttonTestExport;
        private System.Windows.Forms.Label labelTestExportLines;
        private System.Windows.Forms.Button buttonStartExport;
        private System.Windows.Forms.TextBox textBoxExportFileName;
        private System.Windows.Forms.Label labelDirectoryForExortFile;
        private System.Windows.Forms.Button buttonSetDirectoryForExortFile;
        private System.Windows.Forms.Label labelExportFileNameExtension;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TabPage tabPageHeader;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonShowSchema;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Panel panelFileColumns;
        private System.Windows.Forms.TabPage tabPageExport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelExport;
        private System.Windows.Forms.ImageList imageListTab;
        private System.Windows.Forms.CheckBox checkBoxExportIncludeSchema;
        private System.Windows.Forms.Button buttonExportOpenFile;
        private System.Windows.Forms.DataGridView dataGridViewTestExport;
        private System.Windows.Forms.Button buttonShowTestExport;
        private System.Windows.Forms.Button buttonResetSchema;
        private System.Windows.Forms.TextBox textBoxExport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ProgressBar progressBarExport;
        private System.Windows.Forms.Label labelExportProgress;
        private System.Windows.Forms.TabPage tabPageSQLite;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSQLite;
        private System.Windows.Forms.Button buttonSQLiteExport;
        private System.Windows.Forms.Label labelSQLiteDB;
        private System.Windows.Forms.TextBox textBoxSQLiteDB;
        private System.Windows.Forms.Button buttonSQLiteView;
        private System.Windows.Forms.Label labelSQLiteDirectory;
        private System.Windows.Forms.Label labelSQLiteExtension;
        private System.Windows.Forms.ProgressBar progressBarSQLite;
        private System.Windows.Forms.CheckBox checkBoxSchemaDescription;
        private System.Windows.Forms.TextBox textBoxSchemaDescription;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Label labelColumnsOfTable;
        private System.Windows.Forms.ComboBox comboBoxExportFileNameExtension;
        private System.Windows.Forms.Button buttonShowTestExportSQL;
        private System.Windows.Forms.CheckBox checkBoxGetSql;
    }
}