namespace DiversityCollection.Tasks
{
    partial class FormPrometheus
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrometheus));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageConfiguration = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelConfiguration = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxYamlFile = new System.Windows.Forms.TextBox();
            this.buttonYamlOpen = new System.Windows.Forms.Button();
            this.textBoxYaml = new System.Windows.Forms.TextBox();
            this.buttonYamlSave = new System.Windows.Forms.Button();
            this.tabPageStarting = new System.Windows.Forms.TabPage();
            this.tabPageImport = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelImportSource = new System.Windows.Forms.TableLayoutPanel();
            this.labelImportSource = new System.Windows.Forms.Label();
            this.tabControlImport = new System.Windows.Forms.TabControl();
            this.tabPageImportImport = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelImport = new System.Windows.Forms.TableLayoutPanel();
            this.labelImportResolution = new System.Windows.Forms.Label();
            this.buttonImport = new System.Windows.Forms.Button();
            this.numericUpDownImportRange = new System.Windows.Forms.NumericUpDown();
            this.comboBoxImportRangeUnit = new System.Windows.Forms.ComboBox();
            this.numericUpDownImportResolution = new System.Windows.Forms.NumericUpDown();
            this.comboBoxImportResolutionUnit = new System.Windows.Forms.ComboBox();
            this.dataGridViewImport = new System.Windows.Forms.DataGridView();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonImportReadFromPrometheus = new System.Windows.Forms.Button();
            this.dateTimePickerImportStart = new System.Windows.Forms.DateTimePicker();
            this.radioButtonImportStart = new System.Windows.Forms.RadioButton();
            this.radioButtonImportDuration = new System.Windows.Forms.RadioButton();
            this.labelImportRounding = new System.Windows.Forms.Label();
            this.numericUpDownImportRounding = new System.Windows.Forms.NumericUpDown();
            this.tabPageImportPrometheus = new System.Windows.Forms.TabPage();
            this.webView2Prometheus = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.imageListTab = new System.Windows.Forms.ImageList(this.components);
            this.buttonImportSearchSource = new System.Windows.Forms.Button();
            this.textBoxImportSource = new System.Windows.Forms.TextBox();
            this.comboBoxImportPrometheusApiLinks = new System.Windows.Forms.ComboBox();
            this.buttonImportPrometheusApiLinkRemove = new System.Windows.Forms.Button();
            this.tabPageAlerting = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelAlert = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxAlertSource = new System.Windows.Forms.ComboBox();
            this.labelAlertSource = new System.Windows.Forms.Label();
            this.buttonAlertSearchSource = new System.Windows.Forms.Button();
            this.buttonAlertDelete = new System.Windows.Forms.Button();
            this.dataGridViewAlerts = new System.Windows.Forms.DataGridView();
            this.buttonAlertAdd = new System.Windows.Forms.Button();
            this.tabPageSensors = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSensors = new System.Windows.Forms.TableLayoutPanel();
            this.treeViewSensor = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.labelSensorIP = new System.Windows.Forms.Label();
            this.labelSensorPort = new System.Windows.Forms.Label();
            this.maskedTextBoxSensorPort = new System.Windows.Forms.MaskedTextBox();
            this.buttonSensorSearch = new System.Windows.Forms.Button();
            this.buttonSensorOK = new System.Windows.Forms.Button();
            this.comboBoxSensorStatistics = new System.Windows.Forms.ComboBox();
            this.labelSensorStatistics = new System.Windows.Forms.Label();
            this.comboBoxSensorDomainFilter = new System.Windows.Forms.ComboBox();
            this.labelSensorDomainFilter = new System.Windows.Forms.Label();
            this.textBoxSensorIP = new System.Windows.Forms.TextBox();
            this.labelSensorSensorFilter = new System.Windows.Forms.Label();
            this.textBoxSensorSensorFilter = new System.Windows.Forms.TextBox();
            this.tabPageSearch = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSearchSensor = new System.Windows.Forms.TableLayoutPanel();
            this.labelSearchSensor = new System.Windows.Forms.Label();
            this.comboBoxSearchSensor = new System.Windows.Forms.ComboBox();
            this.labelSearchStatistics = new System.Windows.Forms.Label();
            this.comboBoxSearchStatistics = new System.Windows.Forms.ComboBox();
            this.buttonSearchOK = new System.Windows.Forms.Button();
            this.labelSearchMetric = new System.Windows.Forms.Label();
            this.comboBoxSearchMetric = new System.Windows.Forms.ComboBox();
            this.labelSearchFilter = new System.Windows.Forms.Label();
            this.comboBoxSearchFilter = new System.Windows.Forms.ComboBox();
            this.treeViewSearch = new System.Windows.Forms.TreeView();
            this.tableLayoutPanelSearchIP = new System.Windows.Forms.TableLayoutPanel();
            this.labelSearchIP = new System.Windows.Forms.Label();
            this.labelSearchPort = new System.Windows.Forms.Label();
            this.textBoxSearchPort = new System.Windows.Forms.TextBox();
            this.textBoxSearchIP = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.openFileDialogYaml = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl.SuspendLayout();
            this.tabPageConfiguration.SuspendLayout();
            this.tableLayoutPanelConfiguration.SuspendLayout();
            this.tabPageImport.SuspendLayout();
            this.tableLayoutPanelImportSource.SuspendLayout();
            this.tabControlImport.SuspendLayout();
            this.tabPageImportImport.SuspendLayout();
            this.tableLayoutPanelImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportResolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportRounding)).BeginInit();
            this.tabPageImportPrometheus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView2Prometheus)).BeginInit();
            this.tabPageAlerting.SuspendLayout();
            this.tableLayoutPanelAlert.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAlerts)).BeginInit();
            this.tabPageSensors.SuspendLayout();
            this.tableLayoutPanelSensors.SuspendLayout();
            this.tabPageSearch.SuspendLayout();
            this.tableLayoutPanelSearchSensor.SuspendLayout();
            this.tableLayoutPanelSearchIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageConfiguration);
            this.tabControl.Controls.Add(this.tabPageStarting);
            this.tabControl.Controls.Add(this.tabPageImport);
            this.tabControl.Controls.Add(this.tabPageAlerting);
            this.tabControl.Controls.Add(this.tabPageSensors);
            this.tabControl.Controls.Add(this.tabPageSearch);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageListTab;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(385, 461);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageConfiguration
            // 
            this.tabPageConfiguration.Controls.Add(this.tableLayoutPanelConfiguration);
            this.tabPageConfiguration.ImageIndex = 2;
            this.tabPageConfiguration.Location = new System.Drawing.Point(4, 23);
            this.tabPageConfiguration.Name = "tabPageConfiguration";
            this.tabPageConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfiguration.Size = new System.Drawing.Size(377, 434);
            this.tabPageConfiguration.TabIndex = 0;
            this.tabPageConfiguration.Text = "Configuration";
            this.tabPageConfiguration.UseVisualStyleBackColor = true;
            this.tabPageConfiguration.UseWaitCursor = true;
            // 
            // tableLayoutPanelConfiguration
            // 
            this.tableLayoutPanelConfiguration.ColumnCount = 3;
            this.tableLayoutPanelConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelConfiguration.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelConfiguration.Controls.Add(this.textBoxYamlFile, 0, 0);
            this.tableLayoutPanelConfiguration.Controls.Add(this.buttonYamlOpen, 1, 0);
            this.tableLayoutPanelConfiguration.Controls.Add(this.textBoxYaml, 0, 1);
            this.tableLayoutPanelConfiguration.Controls.Add(this.buttonYamlSave, 2, 0);
            this.tableLayoutPanelConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelConfiguration.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelConfiguration.Name = "tableLayoutPanelConfiguration";
            this.tableLayoutPanelConfiguration.RowCount = 2;
            this.tableLayoutPanelConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelConfiguration.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelConfiguration.Size = new System.Drawing.Size(371, 428);
            this.tableLayoutPanelConfiguration.TabIndex = 0;
            this.tableLayoutPanelConfiguration.UseWaitCursor = true;
            // 
            // textBoxYamlFile
            // 
            this.textBoxYamlFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxYamlFile.Location = new System.Drawing.Point(3, 3);
            this.textBoxYamlFile.Name = "textBoxYamlFile";
            this.textBoxYamlFile.ReadOnly = true;
            this.textBoxYamlFile.Size = new System.Drawing.Size(304, 20);
            this.textBoxYamlFile.TabIndex = 0;
            this.textBoxYamlFile.UseWaitCursor = true;
            // 
            // buttonYamlOpen
            // 
            this.buttonYamlOpen.Image = global::DiversityCollection.Resource.Open;
            this.buttonYamlOpen.Location = new System.Drawing.Point(313, 3);
            this.buttonYamlOpen.Name = "buttonYamlOpen";
            this.buttonYamlOpen.Size = new System.Drawing.Size(24, 24);
            this.buttonYamlOpen.TabIndex = 1;
            this.buttonYamlOpen.UseVisualStyleBackColor = true;
            this.buttonYamlOpen.UseWaitCursor = true;
            this.buttonYamlOpen.Click += new System.EventHandler(this.buttonYamlOpen_Click);
            // 
            // textBoxYaml
            // 
            this.tableLayoutPanelConfiguration.SetColumnSpan(this.textBoxYaml, 3);
            this.textBoxYaml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxYaml.Location = new System.Drawing.Point(3, 33);
            this.textBoxYaml.Multiline = true;
            this.textBoxYaml.Name = "textBoxYaml";
            this.textBoxYaml.Size = new System.Drawing.Size(365, 392);
            this.textBoxYaml.TabIndex = 2;
            this.textBoxYaml.UseWaitCursor = true;
            // 
            // buttonYamlSave
            // 
            this.buttonYamlSave.Image = global::DiversityCollection.Resource.Save;
            this.buttonYamlSave.Location = new System.Drawing.Point(343, 3);
            this.buttonYamlSave.Name = "buttonYamlSave";
            this.buttonYamlSave.Size = new System.Drawing.Size(25, 23);
            this.buttonYamlSave.TabIndex = 3;
            this.buttonYamlSave.UseVisualStyleBackColor = true;
            this.buttonYamlSave.UseWaitCursor = true;
            this.buttonYamlSave.Click += new System.EventHandler(this.buttonYamlSave_Click);
            // 
            // tabPageStarting
            // 
            this.tabPageStarting.ImageIndex = 3;
            this.tabPageStarting.Location = new System.Drawing.Point(4, 23);
            this.tabPageStarting.Name = "tabPageStarting";
            this.tabPageStarting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStarting.Size = new System.Drawing.Size(377, 434);
            this.tabPageStarting.TabIndex = 1;
            this.tabPageStarting.Text = "Starting";
            this.tabPageStarting.UseVisualStyleBackColor = true;
            // 
            // tabPageImport
            // 
            this.tabPageImport.Controls.Add(this.tableLayoutPanelImportSource);
            this.tabPageImport.ImageIndex = 1;
            this.tabPageImport.Location = new System.Drawing.Point(4, 23);
            this.tabPageImport.Name = "tabPageImport";
            this.tabPageImport.Size = new System.Drawing.Size(377, 434);
            this.tabPageImport.TabIndex = 2;
            this.tabPageImport.Text = "Import";
            this.tabPageImport.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelImportSource
            // 
            this.tableLayoutPanelImportSource.ColumnCount = 5;
            this.tableLayoutPanelImportSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImportSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImportSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelImportSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImportSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImportSource.Controls.Add(this.labelImportSource, 0, 0);
            this.tableLayoutPanelImportSource.Controls.Add(this.tabControlImport, 0, 2);
            this.tableLayoutPanelImportSource.Controls.Add(this.buttonImportSearchSource, 2, 0);
            this.tableLayoutPanelImportSource.Controls.Add(this.textBoxImportSource, 1, 0);
            this.tableLayoutPanelImportSource.Controls.Add(this.comboBoxImportPrometheusApiLinks, 0, 1);
            this.tableLayoutPanelImportSource.Controls.Add(this.buttonImportPrometheusApiLinkRemove, 4, 1);
            this.tableLayoutPanelImportSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImportSource.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelImportSource.Name = "tableLayoutPanelImportSource";
            this.tableLayoutPanelImportSource.RowCount = 3;
            this.tableLayoutPanelImportSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImportSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImportSource.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImportSource.Size = new System.Drawing.Size(377, 434);
            this.tableLayoutPanelImportSource.TabIndex = 2;
            // 
            // labelImportSource
            // 
            this.labelImportSource.AutoSize = true;
            this.labelImportSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImportSource.Location = new System.Drawing.Point(3, 0);
            this.labelImportSource.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelImportSource.Name = "labelImportSource";
            this.labelImportSource.Size = new System.Drawing.Size(44, 29);
            this.labelImportSource.TabIndex = 0;
            this.labelImportSource.Text = "Source:";
            this.labelImportSource.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tabControlImport
            // 
            this.tableLayoutPanelImportSource.SetColumnSpan(this.tabControlImport, 5);
            this.tabControlImport.Controls.Add(this.tabPageImportImport);
            this.tabControlImport.Controls.Add(this.tabPageImportPrometheus);
            this.tabControlImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlImport.ImageList = this.imageListTab;
            this.tabControlImport.Location = new System.Drawing.Point(3, 56);
            this.tabControlImport.Name = "tabControlImport";
            this.tabControlImport.SelectedIndex = 0;
            this.tabControlImport.Size = new System.Drawing.Size(371, 375);
            this.tabControlImport.TabIndex = 1;
            // 
            // tabPageImportImport
            // 
            this.tabPageImportImport.Controls.Add(this.tableLayoutPanelImport);
            this.tabPageImportImport.ImageIndex = 1;
            this.tabPageImportImport.Location = new System.Drawing.Point(4, 23);
            this.tabPageImportImport.Name = "tabPageImportImport";
            this.tabPageImportImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImportImport.Size = new System.Drawing.Size(363, 348);
            this.tabPageImportImport.TabIndex = 0;
            this.tabPageImportImport.Text = "Import";
            this.tabPageImportImport.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelImport
            // 
            this.tableLayoutPanelImport.ColumnCount = 3;
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImport.Controls.Add(this.labelImportResolution, 0, 5);
            this.tableLayoutPanelImport.Controls.Add(this.buttonImport, 0, 9);
            this.tableLayoutPanelImport.Controls.Add(this.numericUpDownImportRange, 0, 4);
            this.tableLayoutPanelImport.Controls.Add(this.comboBoxImportRangeUnit, 1, 4);
            this.tableLayoutPanelImport.Controls.Add(this.numericUpDownImportResolution, 0, 6);
            this.tableLayoutPanelImport.Controls.Add(this.comboBoxImportResolutionUnit, 1, 6);
            this.tableLayoutPanelImport.Controls.Add(this.dataGridViewImport, 2, 0);
            this.tableLayoutPanelImport.Controls.Add(this.buttonImportReadFromPrometheus, 0, 0);
            this.tableLayoutPanelImport.Controls.Add(this.dateTimePickerImportStart, 0, 2);
            this.tableLayoutPanelImport.Controls.Add(this.radioButtonImportStart, 0, 1);
            this.tableLayoutPanelImport.Controls.Add(this.radioButtonImportDuration, 0, 3);
            this.tableLayoutPanelImport.Controls.Add(this.labelImportRounding, 0, 7);
            this.tableLayoutPanelImport.Controls.Add(this.numericUpDownImportRounding, 0, 8);
            this.tableLayoutPanelImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImport.Enabled = false;
            this.tableLayoutPanelImport.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelImport.Name = "tableLayoutPanelImport";
            this.tableLayoutPanelImport.RowCount = 10;
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImport.Size = new System.Drawing.Size(357, 342);
            this.tableLayoutPanelImport.TabIndex = 0;
            // 
            // labelImportResolution
            // 
            this.labelImportResolution.AutoSize = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.labelImportResolution, 2);
            this.labelImportResolution.Location = new System.Drawing.Point(3, 139);
            this.labelImportResolution.Name = "labelImportResolution";
            this.labelImportResolution.Size = new System.Drawing.Size(60, 13);
            this.labelImportResolution.TabIndex = 5;
            this.labelImportResolution.Text = "Resolution:";
            // 
            // buttonImport
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.buttonImport, 2);
            this.buttonImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImport.Image = global::DiversityCollection.Resource.Import;
            this.buttonImport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonImport.Location = new System.Drawing.Point(3, 299);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(120, 40);
            this.buttonImport.TabIndex = 10;
            this.buttonImport.Text = "Import data in database";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // numericUpDownImportRange
            // 
            this.numericUpDownImportRange.Location = new System.Drawing.Point(3, 115);
            this.numericUpDownImportRange.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownImportRange.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownImportRange.Name = "numericUpDownImportRange";
            this.numericUpDownImportRange.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownImportRange.TabIndex = 4;
            this.numericUpDownImportRange.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBoxImportRangeUnit
            // 
            this.comboBoxImportRangeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxImportRangeUnit.FormattingEnabled = true;
            this.comboBoxImportRangeUnit.Location = new System.Drawing.Point(59, 115);
            this.comboBoxImportRangeUnit.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxImportRangeUnit.Name = "comboBoxImportRangeUnit";
            this.comboBoxImportRangeUnit.Size = new System.Drawing.Size(64, 21);
            this.comboBoxImportRangeUnit.TabIndex = 3;
            // 
            // numericUpDownImportResolution
            // 
            this.numericUpDownImportResolution.Location = new System.Drawing.Point(3, 155);
            this.numericUpDownImportResolution.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownImportResolution.Name = "numericUpDownImportResolution";
            this.numericUpDownImportResolution.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownImportResolution.TabIndex = 6;
            this.numericUpDownImportResolution.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBoxImportResolutionUnit
            // 
            this.comboBoxImportResolutionUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxImportResolutionUnit.FormattingEnabled = true;
            this.comboBoxImportResolutionUnit.Location = new System.Drawing.Point(59, 155);
            this.comboBoxImportResolutionUnit.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxImportResolutionUnit.Name = "comboBoxImportResolutionUnit";
            this.comboBoxImportResolutionUnit.Size = new System.Drawing.Size(64, 21);
            this.comboBoxImportResolutionUnit.TabIndex = 7;
            // 
            // dataGridViewImport
            // 
            this.dataGridViewImport.AllowUserToAddRows = false;
            this.dataGridViewImport.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewImport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDate,
            this.ColumnValue});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImport.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewImport.Location = new System.Drawing.Point(129, 3);
            this.dataGridViewImport.Name = "dataGridViewImport";
            this.dataGridViewImport.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImport.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewImport.RowHeadersVisible = false;
            this.tableLayoutPanelImport.SetRowSpan(this.dataGridViewImport, 10);
            this.dataGridViewImport.Size = new System.Drawing.Size(225, 336);
            this.dataGridViewImport.TabIndex = 9;
            // 
            // ColumnDate
            // 
            this.ColumnDate.HeaderText = "Date";
            this.ColumnDate.Name = "ColumnDate";
            this.ColumnDate.ReadOnly = true;
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.ReadOnly = true;
            // 
            // buttonImportReadFromPrometheus
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.buttonImportReadFromPrometheus, 2);
            this.buttonImportReadFromPrometheus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImportReadFromPrometheus.Image = global::DiversityCollection.Resource.Prometheus;
            this.buttonImportReadFromPrometheus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonImportReadFromPrometheus.Location = new System.Drawing.Point(3, 3);
            this.buttonImportReadFromPrometheus.Name = "buttonImportReadFromPrometheus";
            this.buttonImportReadFromPrometheus.Size = new System.Drawing.Size(120, 40);
            this.buttonImportReadFromPrometheus.TabIndex = 8;
            this.buttonImportReadFromPrometheus.Text = "Read data from Prometheus";
            this.buttonImportReadFromPrometheus.UseVisualStyleBackColor = true;
            this.buttonImportReadFromPrometheus.Click += new System.EventHandler(this.buttonImportReadFromPrometheus_Click);
            // 
            // dateTimePickerImportStart
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.dateTimePickerImportStart, 2);
            this.dateTimePickerImportStart.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerImportStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerImportStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerImportStart.Location = new System.Drawing.Point(3, 69);
            this.dateTimePickerImportStart.Name = "dateTimePickerImportStart";
            this.dateTimePickerImportStart.Size = new System.Drawing.Size(120, 20);
            this.dateTimePickerImportStart.TabIndex = 12;
            // 
            // radioButtonImportStart
            // 
            this.radioButtonImportStart.AutoSize = true;
            this.radioButtonImportStart.Checked = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.radioButtonImportStart, 2);
            this.radioButtonImportStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonImportStart.Location = new System.Drawing.Point(3, 49);
            this.radioButtonImportStart.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.radioButtonImportStart.Name = "radioButtonImportStart";
            this.radioButtonImportStart.Size = new System.Drawing.Size(120, 17);
            this.radioButtonImportStart.TabIndex = 13;
            this.radioButtonImportStart.TabStop = true;
            this.radioButtonImportStart.Text = "Start";
            this.radioButtonImportStart.UseVisualStyleBackColor = true;
            // 
            // radioButtonImportDuration
            // 
            this.radioButtonImportDuration.AutoSize = true;
            this.tableLayoutPanelImport.SetColumnSpan(this.radioButtonImportDuration, 2);
            this.radioButtonImportDuration.Location = new System.Drawing.Point(3, 95);
            this.radioButtonImportDuration.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.radioButtonImportDuration.Name = "radioButtonImportDuration";
            this.radioButtonImportDuration.Size = new System.Drawing.Size(65, 17);
            this.radioButtonImportDuration.TabIndex = 14;
            this.radioButtonImportDuration.Text = "Duration";
            this.radioButtonImportDuration.UseVisualStyleBackColor = true;
            // 
            // labelImportRounding
            // 
            this.labelImportRounding.AutoSize = true;
            this.labelImportRounding.Location = new System.Drawing.Point(3, 179);
            this.labelImportRounding.Name = "labelImportRounding";
            this.labelImportRounding.Size = new System.Drawing.Size(53, 13);
            this.labelImportRounding.TabIndex = 15;
            this.labelImportRounding.Text = "Rounding";
            // 
            // numericUpDownImportRounding
            // 
            this.tableLayoutPanelImport.SetColumnSpan(this.numericUpDownImportRounding, 2);
            this.numericUpDownImportRounding.Location = new System.Drawing.Point(3, 202);
            this.numericUpDownImportRounding.Name = "numericUpDownImportRounding";
            this.numericUpDownImportRounding.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownImportRounding.TabIndex = 16;
            this.numericUpDownImportRounding.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tabPageImportPrometheus
            // 
            this.tabPageImportPrometheus.Controls.Add(this.webView2Prometheus);
            this.tabPageImportPrometheus.ImageIndex = 4;
            this.tabPageImportPrometheus.Location = new System.Drawing.Point(4, 23);
            this.tabPageImportPrometheus.Name = "tabPageImportPrometheus";
            this.tabPageImportPrometheus.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImportPrometheus.Size = new System.Drawing.Size(363, 348);
            this.tabPageImportPrometheus.TabIndex = 1;
            this.tabPageImportPrometheus.Text = "Prometheus";
            this.tabPageImportPrometheus.UseVisualStyleBackColor = true;
            // 
            // webView2Prometheus
            // 
            this.webView2Prometheus.AllowExternalDrop = true;
            this.webView2Prometheus.CreationProperties = null;
            this.webView2Prometheus.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView2Prometheus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView2Prometheus.Location = new System.Drawing.Point(3, 3);
            this.webView2Prometheus.Name = "webView2Prometheus";
            this.webView2Prometheus.Size = new System.Drawing.Size(357, 342);
            this.webView2Prometheus.TabIndex = 0;
            this.webView2Prometheus.ZoomFactor = 1D;
            // 
            // imageListTab
            // 
            this.imageListTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTab.ImageStream")));
            this.imageListTab.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTab.Images.SetKeyName(0, "Alarm1.ico");
            this.imageListTab.Images.SetKeyName(1, "Import.ico");
            this.imageListTab.Images.SetKeyName(2, "Settings.ico");
            this.imageListTab.Images.SetKeyName(3, "Start.ico");
            this.imageListTab.Images.SetKeyName(4, "Prometheus.ico");
            this.imageListTab.Images.SetKeyName(5, "Find.ico");
            this.imageListTab.Images.SetKeyName(6, "Sensor.ico");
            // 
            // buttonImportSearchSource
            // 
            this.tableLayoutPanelImportSource.SetColumnSpan(this.buttonImportSearchSource, 3);
            this.buttonImportSearchSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImportSearchSource.Image = global::DiversityCollection.Resource.Search;
            this.buttonImportSearchSource.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonImportSearchSource.Location = new System.Drawing.Point(214, 2);
            this.buttonImportSearchSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 1);
            this.buttonImportSearchSource.Name = "buttonImportSearchSource";
            this.buttonImportSearchSource.Size = new System.Drawing.Size(160, 26);
            this.buttonImportSearchSource.TabIndex = 11;
            this.buttonImportSearchSource.Text = "Search prometheus source";
            this.buttonImportSearchSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonImportSearchSource.UseVisualStyleBackColor = true;
            this.buttonImportSearchSource.Click += new System.EventHandler(this.buttonImportSearchSource_Click);
            // 
            // textBoxImportSource
            // 
            this.textBoxImportSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxImportSource.Location = new System.Drawing.Point(47, 3);
            this.textBoxImportSource.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxImportSource.Name = "textBoxImportSource";
            this.textBoxImportSource.Size = new System.Drawing.Size(161, 20);
            this.textBoxImportSource.TabIndex = 1;
            this.textBoxImportSource.Visible = false;
            this.textBoxImportSource.TextChanged += new System.EventHandler(this.textBoxImportSource_TextChanged);
            // 
            // comboBoxImportPrometheusApiLinks
            // 
            this.tableLayoutPanelImportSource.SetColumnSpan(this.comboBoxImportPrometheusApiLinks, 4);
            this.comboBoxImportPrometheusApiLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxImportPrometheusApiLinks.FormattingEnabled = true;
            this.comboBoxImportPrometheusApiLinks.Location = new System.Drawing.Point(3, 29);
            this.comboBoxImportPrometheusApiLinks.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.comboBoxImportPrometheusApiLinks.Name = "comboBoxImportPrometheusApiLinks";
            this.comboBoxImportPrometheusApiLinks.Size = new System.Drawing.Size(351, 21);
            this.comboBoxImportPrometheusApiLinks.TabIndex = 12;
            this.comboBoxImportPrometheusApiLinks.SelectionChangeCommitted += new System.EventHandler(this.comboBoxImportPrometheusApiLinks_SelectionChangeCommitted);
            // 
            // buttonImportPrometheusApiLinkRemove
            // 
            this.buttonImportPrometheusApiLinkRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImportPrometheusApiLinkRemove.FlatAppearance.BorderSize = 0;
            this.buttonImportPrometheusApiLinkRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImportPrometheusApiLinkRemove.Image = global::DiversityCollection.Resource.Delete;
            this.buttonImportPrometheusApiLinkRemove.Location = new System.Drawing.Point(360, 29);
            this.buttonImportPrometheusApiLinkRemove.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonImportPrometheusApiLinkRemove.Name = "buttonImportPrometheusApiLinkRemove";
            this.buttonImportPrometheusApiLinkRemove.Size = new System.Drawing.Size(14, 21);
            this.buttonImportPrometheusApiLinkRemove.TabIndex = 13;
            this.toolTip.SetToolTip(this.buttonImportPrometheusApiLinkRemove, "Remove current source from list");
            this.buttonImportPrometheusApiLinkRemove.UseVisualStyleBackColor = true;
            this.buttonImportPrometheusApiLinkRemove.Click += new System.EventHandler(this.buttonImportPrometheusApiLinkRemove_Click);
            // 
            // tabPageAlerting
            // 
            this.tabPageAlerting.Controls.Add(this.tableLayoutPanelAlert);
            this.tabPageAlerting.ImageIndex = 0;
            this.tabPageAlerting.Location = new System.Drawing.Point(4, 23);
            this.tabPageAlerting.Name = "tabPageAlerting";
            this.tabPageAlerting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAlerting.Size = new System.Drawing.Size(377, 434);
            this.tabPageAlerting.TabIndex = 3;
            this.tabPageAlerting.Text = "Alerting";
            this.tabPageAlerting.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelAlert
            // 
            this.tableLayoutPanelAlert.ColumnCount = 5;
            this.tableLayoutPanelAlert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAlert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAlert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAlert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAlert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAlert.Controls.Add(this.comboBoxAlertSource, 0, 1);
            this.tableLayoutPanelAlert.Controls.Add(this.labelAlertSource, 0, 0);
            this.tableLayoutPanelAlert.Controls.Add(this.buttonAlertSearchSource, 2, 0);
            this.tableLayoutPanelAlert.Controls.Add(this.buttonAlertDelete, 4, 1);
            this.tableLayoutPanelAlert.Controls.Add(this.dataGridViewAlerts, 0, 2);
            this.tableLayoutPanelAlert.Controls.Add(this.buttonAlertAdd, 3, 1);
            this.tableLayoutPanelAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAlert.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelAlert.Name = "tableLayoutPanelAlert";
            this.tableLayoutPanelAlert.RowCount = 3;
            this.tableLayoutPanelAlert.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAlert.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAlert.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAlert.Size = new System.Drawing.Size(371, 428);
            this.tableLayoutPanelAlert.TabIndex = 3;
            // 
            // comboBoxAlertSource
            // 
            this.tableLayoutPanelAlert.SetColumnSpan(this.comboBoxAlertSource, 3);
            this.comboBoxAlertSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAlertSource.FormattingEnabled = true;
            this.comboBoxAlertSource.Location = new System.Drawing.Point(3, 29);
            this.comboBoxAlertSource.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.comboBoxAlertSource.Name = "comboBoxAlertSource";
            this.comboBoxAlertSource.Size = new System.Drawing.Size(331, 21);
            this.comboBoxAlertSource.TabIndex = 15;
            // 
            // labelAlertSource
            // 
            this.labelAlertSource.AutoSize = true;
            this.labelAlertSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAlertSource.Location = new System.Drawing.Point(3, 0);
            this.labelAlertSource.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAlertSource.Name = "labelAlertSource";
            this.labelAlertSource.Size = new System.Drawing.Size(44, 29);
            this.labelAlertSource.TabIndex = 0;
            this.labelAlertSource.Text = "Source:";
            this.labelAlertSource.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // buttonAlertSearchSource
            // 
            this.tableLayoutPanelAlert.SetColumnSpan(this.buttonAlertSearchSource, 3);
            this.buttonAlertSearchSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAlertSearchSource.Image = global::DiversityCollection.Resource.Search;
            this.buttonAlertSearchSource.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAlertSearchSource.Location = new System.Drawing.Point(208, 2);
            this.buttonAlertSearchSource.Margin = new System.Windows.Forms.Padding(3, 2, 3, 1);
            this.buttonAlertSearchSource.Name = "buttonAlertSearchSource";
            this.buttonAlertSearchSource.Size = new System.Drawing.Size(160, 26);
            this.buttonAlertSearchSource.TabIndex = 11;
            this.buttonAlertSearchSource.Text = "Search prometheus source";
            this.buttonAlertSearchSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonAlertSearchSource.UseVisualStyleBackColor = true;
            // 
            // buttonAlertDelete
            // 
            this.buttonAlertDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAlertDelete.FlatAppearance.BorderSize = 0;
            this.buttonAlertDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAlertDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonAlertDelete.Location = new System.Drawing.Point(357, 29);
            this.buttonAlertDelete.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAlertDelete.Name = "buttonAlertDelete";
            this.buttonAlertDelete.Size = new System.Drawing.Size(14, 24);
            this.buttonAlertDelete.TabIndex = 13;
            this.toolTip.SetToolTip(this.buttonAlertDelete, "Remove current source from list");
            this.buttonAlertDelete.UseVisualStyleBackColor = true;
            this.buttonAlertDelete.Click += new System.EventHandler(this.buttonAlertDelete_Click);
            // 
            // dataGridViewAlerts
            // 
            this.dataGridViewAlerts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelAlert.SetColumnSpan(this.dataGridViewAlerts, 5);
            this.dataGridViewAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewAlerts.Location = new System.Drawing.Point(3, 56);
            this.dataGridViewAlerts.Name = "dataGridViewAlerts";
            this.dataGridViewAlerts.Size = new System.Drawing.Size(365, 369);
            this.dataGridViewAlerts.TabIndex = 14;
            // 
            // buttonAlertAdd
            // 
            this.buttonAlertAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAlertAdd.FlatAppearance.BorderSize = 0;
            this.buttonAlertAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAlertAdd.Image = global::DiversityCollection.Resource.Add1;
            this.buttonAlertAdd.Location = new System.Drawing.Point(337, 29);
            this.buttonAlertAdd.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAlertAdd.Name = "buttonAlertAdd";
            this.buttonAlertAdd.Size = new System.Drawing.Size(20, 24);
            this.buttonAlertAdd.TabIndex = 16;
            this.buttonAlertAdd.UseVisualStyleBackColor = true;
            this.buttonAlertAdd.Click += new System.EventHandler(this.buttonAlertAdd_Click);
            // 
            // tabPageSensors
            // 
            this.tabPageSensors.Controls.Add(this.tableLayoutPanelSensors);
            this.tabPageSensors.ImageIndex = 6;
            this.tabPageSensors.Location = new System.Drawing.Point(4, 23);
            this.tabPageSensors.Name = "tabPageSensors";
            this.tabPageSensors.Size = new System.Drawing.Size(377, 434);
            this.tabPageSensors.TabIndex = 5;
            this.tabPageSensors.Text = "Sensors";
            this.tabPageSensors.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSensors
            // 
            this.tableLayoutPanelSensors.ColumnCount = 5;
            this.tableLayoutPanelSensors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSensors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSensors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSensors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSensors.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSensors.Controls.Add(this.treeViewSensor, 0, 2);
            this.tableLayoutPanelSensors.Controls.Add(this.labelSensorIP, 0, 0);
            this.tableLayoutPanelSensors.Controls.Add(this.labelSensorPort, 2, 0);
            this.tableLayoutPanelSensors.Controls.Add(this.maskedTextBoxSensorPort, 3, 0);
            this.tableLayoutPanelSensors.Controls.Add(this.buttonSensorSearch, 4, 1);
            this.tableLayoutPanelSensors.Controls.Add(this.buttonSensorOK, 3, 3);
            this.tableLayoutPanelSensors.Controls.Add(this.comboBoxSensorStatistics, 1, 3);
            this.tableLayoutPanelSensors.Controls.Add(this.labelSensorStatistics, 0, 3);
            this.tableLayoutPanelSensors.Controls.Add(this.comboBoxSensorDomainFilter, 1, 1);
            this.tableLayoutPanelSensors.Controls.Add(this.labelSensorDomainFilter, 0, 1);
            this.tableLayoutPanelSensors.Controls.Add(this.textBoxSensorIP, 1, 0);
            this.tableLayoutPanelSensors.Controls.Add(this.labelSensorSensorFilter, 2, 1);
            this.tableLayoutPanelSensors.Controls.Add(this.textBoxSensorSensorFilter, 3, 1);
            this.tableLayoutPanelSensors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSensors.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSensors.Name = "tableLayoutPanelSensors";
            this.tableLayoutPanelSensors.RowCount = 4;
            this.tableLayoutPanelSensors.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSensors.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSensors.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSensors.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSensors.Size = new System.Drawing.Size(377, 434);
            this.tableLayoutPanelSensors.TabIndex = 0;
            // 
            // treeViewSensor
            // 
            this.tableLayoutPanelSensors.SetColumnSpan(this.treeViewSensor, 5);
            this.treeViewSensor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSensor.ImageIndex = 0;
            this.treeViewSensor.ImageList = this.imageListTree;
            this.treeViewSensor.Location = new System.Drawing.Point(3, 56);
            this.treeViewSensor.Name = "treeViewSensor";
            this.treeViewSensor.SelectedImageIndex = 0;
            this.treeViewSensor.Size = new System.Drawing.Size(371, 346);
            this.treeViewSensor.TabIndex = 7;
            this.treeViewSensor.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewSensor_AfterSelect);
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "Prometheus.ico");
            this.imageListTree.Images.SetKeyName(1, "FilterBig.ico");
            this.imageListTree.Images.SetKeyName(2, "Sensor.ico");
            this.imageListTree.Images.SetKeyName(3, "Battery.ico");
            this.imageListTree.Images.SetKeyName(4, "SensorHum.ico");
            this.imageListTree.Images.SetKeyName(5, "SensorTemp.ico");
            // 
            // labelSensorIP
            // 
            this.labelSensorIP.AutoSize = true;
            this.labelSensorIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSensorIP.Image = global::DiversityCollection.Resource.Prometheus;
            this.labelSensorIP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSensorIP.Location = new System.Drawing.Point(3, 0);
            this.labelSensorIP.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSensorIP.Name = "labelSensorIP";
            this.labelSensorIP.Size = new System.Drawing.Size(52, 26);
            this.labelSensorIP.TabIndex = 0;
            this.labelSensorIP.Text = "      IP:";
            this.labelSensorIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSensorPort
            // 
            this.labelSensorPort.AutoSize = true;
            this.labelSensorPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSensorPort.Location = new System.Drawing.Point(190, 0);
            this.labelSensorPort.Name = "labelSensorPort";
            this.labelSensorPort.Size = new System.Drawing.Size(29, 26);
            this.labelSensorPort.TabIndex = 1;
            this.labelSensorPort.Text = "Port:";
            this.labelSensorPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBoxSensorPort
            // 
            this.tableLayoutPanelSensors.SetColumnSpan(this.maskedTextBoxSensorPort, 2);
            this.maskedTextBoxSensorPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxSensorPort.Location = new System.Drawing.Point(222, 3);
            this.maskedTextBoxSensorPort.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.maskedTextBoxSensorPort.Mask = "0000";
            this.maskedTextBoxSensorPort.Name = "maskedTextBoxSensorPort";
            this.maskedTextBoxSensorPort.Size = new System.Drawing.Size(152, 20);
            this.maskedTextBoxSensorPort.TabIndex = 3;
            // 
            // buttonSensorSearch
            // 
            this.buttonSensorSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSensorSearch.FlatAppearance.BorderSize = 0;
            this.buttonSensorSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSensorSearch.Image = global::DiversityCollection.Resource.Search;
            this.buttonSensorSearch.Location = new System.Drawing.Point(354, 26);
            this.buttonSensorSearch.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonSensorSearch.Name = "buttonSensorSearch";
            this.buttonSensorSearch.Size = new System.Drawing.Size(20, 27);
            this.buttonSensorSearch.TabIndex = 6;
            this.buttonSensorSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSensorSearch.UseVisualStyleBackColor = true;
            this.buttonSensorSearch.Click += new System.EventHandler(this.buttonSensorSearch_Click);
            // 
            // buttonSensorOK
            // 
            this.tableLayoutPanelSensors.SetColumnSpan(this.buttonSensorOK, 2);
            this.buttonSensorOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSensorOK.Enabled = false;
            this.buttonSensorOK.Location = new System.Drawing.Point(299, 408);
            this.buttonSensorOK.Name = "buttonSensorOK";
            this.buttonSensorOK.Size = new System.Drawing.Size(75, 23);
            this.buttonSensorOK.TabIndex = 10;
            this.buttonSensorOK.Text = "OK";
            this.buttonSensorOK.UseVisualStyleBackColor = true;
            this.buttonSensorOK.Click += new System.EventHandler(this.buttonSensorOK_Click);
            // 
            // comboBoxSensorStatistics
            // 
            this.tableLayoutPanelSensors.SetColumnSpan(this.comboBoxSensorStatistics, 2);
            this.comboBoxSensorStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSensorStatistics.FormattingEnabled = true;
            this.comboBoxSensorStatistics.Location = new System.Drawing.Point(55, 408);
            this.comboBoxSensorStatistics.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxSensorStatistics.Name = "comboBoxSensorStatistics";
            this.comboBoxSensorStatistics.Size = new System.Drawing.Size(164, 21);
            this.comboBoxSensorStatistics.TabIndex = 9;
            // 
            // labelSensorStatistics
            // 
            this.labelSensorStatistics.AutoSize = true;
            this.labelSensorStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSensorStatistics.Location = new System.Drawing.Point(3, 405);
            this.labelSensorStatistics.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSensorStatistics.Name = "labelSensorStatistics";
            this.labelSensorStatistics.Size = new System.Drawing.Size(52, 29);
            this.labelSensorStatistics.TabIndex = 8;
            this.labelSensorStatistics.Text = "Statistics:";
            this.labelSensorStatistics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxSensorDomainFilter
            // 
            this.comboBoxSensorDomainFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSensorDomainFilter.FormattingEnabled = true;
            this.comboBoxSensorDomainFilter.Location = new System.Drawing.Point(55, 29);
            this.comboBoxSensorDomainFilter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxSensorDomainFilter.Name = "comboBoxSensorDomainFilter";
            this.comboBoxSensorDomainFilter.Size = new System.Drawing.Size(129, 21);
            this.comboBoxSensorDomainFilter.TabIndex = 5;
            // 
            // labelSensorDomainFilter
            // 
            this.labelSensorDomainFilter.AutoSize = true;
            this.labelSensorDomainFilter.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelSensorDomainFilter.Image = global::DiversityCollection.Resource.Filter;
            this.labelSensorDomainFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSensorDomainFilter.Location = new System.Drawing.Point(5, 26);
            this.labelSensorDomainFilter.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSensorDomainFilter.Name = "labelSensorDomainFilter";
            this.labelSensorDomainFilter.Size = new System.Drawing.Size(50, 27);
            this.labelSensorDomainFilter.TabIndex = 4;
            this.labelSensorDomainFilter.Text = "      Filter:";
            this.labelSensorDomainFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSensorIP
            // 
            this.textBoxSensorIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSensorIP.Location = new System.Drawing.Point(55, 3);
            this.textBoxSensorIP.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxSensorIP.Name = "textBoxSensorIP";
            this.textBoxSensorIP.Size = new System.Drawing.Size(129, 20);
            this.textBoxSensorIP.TabIndex = 12;
            // 
            // labelSensorSensorFilter
            // 
            this.labelSensorSensorFilter.AutoSize = true;
            this.labelSensorSensorFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSensorSensorFilter.Image = global::DiversityCollection.Resource.Sensor;
            this.labelSensorSensorFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelSensorSensorFilter.Location = new System.Drawing.Point(190, 26);
            this.labelSensorSensorFilter.Name = "labelSensorSensorFilter";
            this.labelSensorSensorFilter.Size = new System.Drawing.Size(29, 27);
            this.labelSensorSensorFilter.TabIndex = 13;
            this.labelSensorSensorFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSensorSensorFilter
            // 
            this.textBoxSensorSensorFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSensorSensorFilter.Location = new System.Drawing.Point(222, 29);
            this.textBoxSensorSensorFilter.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxSensorSensorFilter.Name = "textBoxSensorSensorFilter";
            this.textBoxSensorSensorFilter.Size = new System.Drawing.Size(132, 20);
            this.textBoxSensorSensorFilter.TabIndex = 14;
            // 
            // tabPageSearch
            // 
            this.tabPageSearch.Controls.Add(this.tableLayoutPanelSearchSensor);
            this.tabPageSearch.Controls.Add(this.tableLayoutPanelSearchIP);
            this.tabPageSearch.ImageIndex = 5;
            this.tabPageSearch.Location = new System.Drawing.Point(4, 23);
            this.tabPageSearch.Name = "tabPageSearch";
            this.tabPageSearch.Size = new System.Drawing.Size(377, 434);
            this.tabPageSearch.TabIndex = 4;
            this.tabPageSearch.Text = "Search";
            this.tabPageSearch.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSearchSensor
            // 
            this.tableLayoutPanelSearchSensor.ColumnCount = 4;
            this.tableLayoutPanelSearchSensor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelSearchSensor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelSearchSensor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelSearchSensor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelSearchSensor.Controls.Add(this.labelSearchSensor, 0, 0);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.comboBoxSearchSensor, 1, 0);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.labelSearchStatistics, 2, 1);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.comboBoxSearchStatistics, 3, 1);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.buttonSearchOK, 3, 4);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.labelSearchMetric, 0, 1);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.comboBoxSearchMetric, 1, 1);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.labelSearchFilter, 2, 0);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.comboBoxSearchFilter, 3, 0);
            this.tableLayoutPanelSearchSensor.Controls.Add(this.treeViewSearch, 0, 3);
            this.tableLayoutPanelSearchSensor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSearchSensor.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanelSearchSensor.Name = "tableLayoutPanelSearchSensor";
            this.tableLayoutPanelSearchSensor.RowCount = 5;
            this.tableLayoutPanelSearchSensor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchSensor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchSensor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanelSearchSensor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchSensor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSearchSensor.Size = new System.Drawing.Size(377, 407);
            this.tableLayoutPanelSearchSensor.TabIndex = 1;
            // 
            // labelSearchSensor
            // 
            this.labelSearchSensor.AutoSize = true;
            this.labelSearchSensor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSearchSensor.Location = new System.Drawing.Point(0, 0);
            this.labelSearchSensor.Margin = new System.Windows.Forms.Padding(0);
            this.labelSearchSensor.Name = "labelSearchSensor";
            this.labelSearchSensor.Size = new System.Drawing.Size(50, 27);
            this.labelSearchSensor.TabIndex = 0;
            this.labelSearchSensor.Text = "Sensor:";
            this.labelSearchSensor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxSearchSensor
            // 
            this.comboBoxSearchSensor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSearchSensor.FormattingEnabled = true;
            this.comboBoxSearchSensor.Location = new System.Drawing.Point(53, 3);
            this.comboBoxSearchSensor.Name = "comboBoxSearchSensor";
            this.comboBoxSearchSensor.Size = new System.Drawing.Size(180, 21);
            this.comboBoxSearchSensor.TabIndex = 1;
            this.comboBoxSearchSensor.SelectionChangeCommitted += new System.EventHandler(this.comboBoxSearchSensor_SelectionChangeCommitted);
            // 
            // labelSearchStatistics
            // 
            this.labelSearchStatistics.AutoSize = true;
            this.labelSearchStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSearchStatistics.Location = new System.Drawing.Point(236, 27);
            this.labelSearchStatistics.Margin = new System.Windows.Forms.Padding(0);
            this.labelSearchStatistics.Name = "labelSearchStatistics";
            this.labelSearchStatistics.Size = new System.Drawing.Size(60, 27);
            this.labelSearchStatistics.TabIndex = 2;
            this.labelSearchStatistics.Text = "Statistics:";
            this.labelSearchStatistics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxSearchStatistics
            // 
            this.comboBoxSearchStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSearchStatistics.FormattingEnabled = true;
            this.comboBoxSearchStatistics.Location = new System.Drawing.Point(299, 30);
            this.comboBoxSearchStatistics.Name = "comboBoxSearchStatistics";
            this.comboBoxSearchStatistics.Size = new System.Drawing.Size(75, 21);
            this.comboBoxSearchStatistics.TabIndex = 3;
            // 
            // buttonSearchOK
            // 
            this.buttonSearchOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSearchOK.Enabled = false;
            this.buttonSearchOK.Location = new System.Drawing.Point(299, 381);
            this.buttonSearchOK.Name = "buttonSearchOK";
            this.buttonSearchOK.Size = new System.Drawing.Size(75, 23);
            this.buttonSearchOK.TabIndex = 4;
            this.buttonSearchOK.Text = "OK";
            this.buttonSearchOK.UseVisualStyleBackColor = true;
            this.buttonSearchOK.Click += new System.EventHandler(this.buttonSearchOK_Click);
            // 
            // labelSearchMetric
            // 
            this.labelSearchMetric.AutoSize = true;
            this.labelSearchMetric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSearchMetric.Location = new System.Drawing.Point(3, 27);
            this.labelSearchMetric.Name = "labelSearchMetric";
            this.labelSearchMetric.Size = new System.Drawing.Size(44, 27);
            this.labelSearchMetric.TabIndex = 5;
            this.labelSearchMetric.Text = "Metric:";
            this.labelSearchMetric.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxSearchMetric
            // 
            this.comboBoxSearchMetric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxSearchMetric.FormattingEnabled = true;
            this.comboBoxSearchMetric.Location = new System.Drawing.Point(53, 30);
            this.comboBoxSearchMetric.Name = "comboBoxSearchMetric";
            this.comboBoxSearchMetric.Size = new System.Drawing.Size(180, 21);
            this.comboBoxSearchMetric.TabIndex = 6;
            this.comboBoxSearchMetric.SelectionChangeCommitted += new System.EventHandler(this.comboBoxSearchMetric_SelectionChangeCommitted);
            // 
            // labelSearchFilter
            // 
            this.labelSearchFilter.AutoSize = true;
            this.labelSearchFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSearchFilter.Location = new System.Drawing.Point(239, 0);
            this.labelSearchFilter.Name = "labelSearchFilter";
            this.labelSearchFilter.Size = new System.Drawing.Size(54, 27);
            this.labelSearchFilter.TabIndex = 7;
            this.labelSearchFilter.Text = "Filter:";
            this.labelSearchFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxSearchFilter
            // 
            this.comboBoxSearchFilter.FormattingEnabled = true;
            this.comboBoxSearchFilter.Location = new System.Drawing.Point(299, 3);
            this.comboBoxSearchFilter.Name = "comboBoxSearchFilter";
            this.comboBoxSearchFilter.Size = new System.Drawing.Size(75, 21);
            this.comboBoxSearchFilter.TabIndex = 8;
            this.comboBoxSearchFilter.Text = "IPM";
            // 
            // treeViewSearch
            // 
            this.tableLayoutPanelSearchSensor.SetColumnSpan(this.treeViewSearch, 4);
            this.treeViewSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSearch.Location = new System.Drawing.Point(3, 61);
            this.treeViewSearch.Name = "treeViewSearch";
            this.treeViewSearch.Size = new System.Drawing.Size(371, 314);
            this.treeViewSearch.TabIndex = 9;
            this.treeViewSearch.Visible = false;
            // 
            // tableLayoutPanelSearchIP
            // 
            this.tableLayoutPanelSearchIP.ColumnCount = 5;
            this.tableLayoutPanelSearchIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSearchIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelSearchIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanelSearchIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelSearchIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelSearchIP.Controls.Add(this.labelSearchIP, 1, 0);
            this.tableLayoutPanelSearchIP.Controls.Add(this.labelSearchPort, 3, 0);
            this.tableLayoutPanelSearchIP.Controls.Add(this.textBoxSearchPort, 4, 0);
            this.tableLayoutPanelSearchIP.Controls.Add(this.textBoxSearchIP, 2, 0);
            this.tableLayoutPanelSearchIP.Controls.Add(this.buttonSearch, 0, 0);
            this.tableLayoutPanelSearchIP.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSearchIP.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSearchIP.Name = "tableLayoutPanelSearchIP";
            this.tableLayoutPanelSearchIP.RowCount = 1;
            this.tableLayoutPanelSearchIP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSearchIP.Size = new System.Drawing.Size(377, 27);
            this.tableLayoutPanelSearchIP.TabIndex = 0;
            // 
            // labelSearchIP
            // 
            this.labelSearchIP.AutoSize = true;
            this.labelSearchIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSearchIP.Location = new System.Drawing.Point(23, 0);
            this.labelSearchIP.Name = "labelSearchIP";
            this.labelSearchIP.Size = new System.Drawing.Size(24, 27);
            this.labelSearchIP.TabIndex = 0;
            this.labelSearchIP.Text = "IP:";
            this.labelSearchIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSearchPort
            // 
            this.labelSearchPort.AutoSize = true;
            this.labelSearchPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSearchPort.Location = new System.Drawing.Point(239, 0);
            this.labelSearchPort.Name = "labelSearchPort";
            this.labelSearchPort.Size = new System.Drawing.Size(54, 27);
            this.labelSearchPort.TabIndex = 1;
            this.labelSearchPort.Text = "Port:";
            this.labelSearchPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSearchPort
            // 
            this.textBoxSearchPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearchPort.Location = new System.Drawing.Point(299, 3);
            this.textBoxSearchPort.Name = "textBoxSearchPort";
            this.textBoxSearchPort.Size = new System.Drawing.Size(75, 20);
            this.textBoxSearchPort.TabIndex = 2;
            this.textBoxSearchPort.Text = "9090";
            this.textBoxSearchPort.Leave += new System.EventHandler(this.textBoxSearchPort_Leave);
            // 
            // textBoxSearchIP
            // 
            this.textBoxSearchIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSearchIP.Location = new System.Drawing.Point(53, 3);
            this.textBoxSearchIP.Name = "textBoxSearchIP";
            this.textBoxSearchIP.Size = new System.Drawing.Size(180, 20);
            this.textBoxSearchIP.TabIndex = 3;
            this.textBoxSearchIP.Leave += new System.EventHandler(this.textBoxSearchIP_Leave);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearch.FlatAppearance.BorderSize = 0;
            this.buttonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSearch.Image = global::DiversityCollection.Resource.Search;
            this.buttonSearch.Location = new System.Drawing.Point(0, 0);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(20, 27);
            this.buttonSearch.TabIndex = 4;
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // openFileDialogYaml
            // 
            this.openFileDialogYaml.FileName = "openFileDialogYaml";
            // 
            // FormPrometheus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 461);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPrometheus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prometheus";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrometheus_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabPageConfiguration.ResumeLayout(false);
            this.tableLayoutPanelConfiguration.ResumeLayout(false);
            this.tableLayoutPanelConfiguration.PerformLayout();
            this.tabPageImport.ResumeLayout(false);
            this.tableLayoutPanelImportSource.ResumeLayout(false);
            this.tableLayoutPanelImportSource.PerformLayout();
            this.tabControlImport.ResumeLayout(false);
            this.tabPageImportImport.ResumeLayout(false);
            this.tableLayoutPanelImport.ResumeLayout(false);
            this.tableLayoutPanelImport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportResolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportRounding)).EndInit();
            this.tabPageImportPrometheus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.webView2Prometheus)).EndInit();
            this.tabPageAlerting.ResumeLayout(false);
            this.tableLayoutPanelAlert.ResumeLayout(false);
            this.tableLayoutPanelAlert.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAlerts)).EndInit();
            this.tabPageSensors.ResumeLayout(false);
            this.tableLayoutPanelSensors.ResumeLayout(false);
            this.tableLayoutPanelSensors.PerformLayout();
            this.tabPageSearch.ResumeLayout(false);
            this.tableLayoutPanelSearchSensor.ResumeLayout(false);
            this.tableLayoutPanelSearchSensor.PerformLayout();
            this.tableLayoutPanelSearchIP.ResumeLayout(false);
            this.tableLayoutPanelSearchIP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageConfiguration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelConfiguration;
        private System.Windows.Forms.TextBox textBoxYamlFile;
        private System.Windows.Forms.Button buttonYamlOpen;
        private System.Windows.Forms.TabPage tabPageStarting;
        private System.Windows.Forms.OpenFileDialog openFileDialogYaml;
        private System.Windows.Forms.TextBox textBoxYaml;
        private System.Windows.Forms.Button buttonYamlSave;
        private System.Windows.Forms.TabPage tabPageImport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImport;
        private System.Windows.Forms.Label labelImportSource;
        private System.Windows.Forms.Label labelImportResolution;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.NumericUpDown numericUpDownImportRange;
        private System.Windows.Forms.ComboBox comboBoxImportRangeUnit;
        private System.Windows.Forms.NumericUpDown numericUpDownImportResolution;
        private System.Windows.Forms.ComboBox comboBoxImportResolutionUnit;
        private System.Windows.Forms.TextBox textBoxImportSource;
        private System.Windows.Forms.DataGridView dataGridViewImport;
        private System.Windows.Forms.Button buttonImportReadFromPrometheus;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.Button buttonImportSearchSource;
        private System.Windows.Forms.TabPage tabPageAlerting;
        private System.Windows.Forms.ImageList imageListTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImportSource;
        private System.Windows.Forms.TabControl tabControlImport;
        private System.Windows.Forms.TabPage tabPageImportImport;
        private System.Windows.Forms.TabPage tabPageImportPrometheus;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2Prometheus;
        private System.Windows.Forms.TabPage tabPageSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchIP;
        private System.Windows.Forms.Label labelSearchIP;
        private System.Windows.Forms.Label labelSearchPort;
        private System.Windows.Forms.TextBox textBoxSearchPort;
        private System.Windows.Forms.TextBox textBoxSearchIP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSearchSensor;
        private System.Windows.Forms.Label labelSearchSensor;
        private System.Windows.Forms.ComboBox comboBoxSearchSensor;
        private System.Windows.Forms.Label labelSearchStatistics;
        private System.Windows.Forms.ComboBox comboBoxSearchStatistics;
        private System.Windows.Forms.Button buttonSearchOK;
        private System.Windows.Forms.Label labelSearchMetric;
        private System.Windows.Forms.ComboBox comboBoxSearchMetric;
        private System.Windows.Forms.ComboBox comboBoxImportPrometheusApiLinks;
        private System.Windows.Forms.Button buttonImportPrometheusApiLinkRemove;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelSearchFilter;
        private System.Windows.Forms.ComboBox comboBoxSearchFilter;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAlert;
        private System.Windows.Forms.Label labelAlertSource;
        private System.Windows.Forms.Button buttonAlertSearchSource;
        private System.Windows.Forms.Button buttonAlertDelete;
        private System.Windows.Forms.ComboBox comboBoxAlertSource;
        private System.Windows.Forms.DataGridView dataGridViewAlerts;
        private System.Windows.Forms.Button buttonAlertAdd;
        private System.Windows.Forms.DateTimePicker dateTimePickerImportStart;
        private System.Windows.Forms.RadioButton radioButtonImportStart;
        private System.Windows.Forms.RadioButton radioButtonImportDuration;
        private System.Windows.Forms.Label labelImportRounding;
        private System.Windows.Forms.NumericUpDown numericUpDownImportRounding;
        private System.Windows.Forms.TreeView treeViewSearch;
        private System.Windows.Forms.TabPage tabPageSensors;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSensors;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.Label labelSensorIP;
        private System.Windows.Forms.Label labelSensorPort;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxSensorPort;
        private System.Windows.Forms.Button buttonSensorSearch;
        private System.Windows.Forms.TreeView treeViewSensor;
        private System.Windows.Forms.Button buttonSensorOK;
        private System.Windows.Forms.ComboBox comboBoxSensorStatistics;
        private System.Windows.Forms.Label labelSensorStatistics;
        private System.Windows.Forms.ComboBox comboBoxSensorDomainFilter;
        private System.Windows.Forms.Label labelSensorDomainFilter;
        private System.Windows.Forms.TextBox textBoxSensorIP;
        private System.Windows.Forms.Label labelSensorSensorFilter;
        private System.Windows.Forms.TextBox textBoxSensorSensorFilter;
    }
}