namespace DiversityCollection.Tasks
{
    partial class UserControlChart
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlChart));
            this.buttonCreateChart = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageChart = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelChart = new System.Windows.Forms.TableLayoutPanel();
            this.chartTask = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonOpenChart = new System.Windows.Forms.Button();
            this.groupBoxExport = new System.Windows.Forms.GroupBox();
            this.buttonExportOpen = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonSaveImage = new System.Windows.Forms.Button();
            this.buttonPrometheus = new System.Windows.Forms.Button();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageRange = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelRange = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxStart = new System.Windows.Forms.CheckBox();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.checkBoxEnd = new System.Windows.Forms.CheckBox();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.tabPagePests = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelNumbers = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelNumberChartType = new System.Windows.Forms.TableLayoutPanel();
            this.labelChartType = new System.Windows.Forms.Label();
            this.radioButtonChartTypeColumns = new System.Windows.Forms.RadioButton();
            this.radioButtonBubble = new System.Windows.Forms.RadioButton();
            this.numericUpDownBubble = new System.Windows.Forms.NumericUpDown();
            this.checkedListBoxPests = new System.Windows.Forms.CheckedListBox();
            this.tableLayoutPanelNumberGrouping = new System.Windows.Forms.TableLayoutPanel();
            this.labelNumberGrouping = new System.Windows.Forms.Label();
            this.radioButtonNumberGroupingHour = new System.Windows.Forms.RadioButton();
            this.radioButtonNumberGroupingYear = new System.Windows.Forms.RadioButton();
            this.radioButtonNumberGroupingMonth = new System.Windows.Forms.RadioButton();
            this.radioButtonNumberGroupingDay = new System.Windows.Forms.RadioButton();
            this.radioButtonNumberGroupingWeek = new System.Windows.Forms.RadioButton();
            this.tabPageMetrics = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelMetric = new System.Windows.Forms.TableLayoutPanel();
            this.labelMetricGrouping = new System.Windows.Forms.Label();
            this.radioButtonMetricGroupingHour = new System.Windows.Forms.RadioButton();
            this.checkedListBoxMetrics = new System.Windows.Forms.CheckedListBox();
            this.radioButtonMetricGroupingDay = new System.Windows.Forms.RadioButton();
            this.radioButtonMetricGroupingWeek = new System.Windows.Forms.RadioButton();
            this.imageListTabs = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelCollection = new System.Windows.Forms.Label();
            this.tabControlMain.SuspendLayout();
            this.tabPageChart.SuspendLayout();
            this.tableLayoutPanelChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTask)).BeginInit();
            this.groupBoxExport.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.tabControlSettings.SuspendLayout();
            this.tabPageRange.SuspendLayout();
            this.tableLayoutPanelRange.SuspendLayout();
            this.tabPagePests.SuspendLayout();
            this.tableLayoutPanelNumbers.SuspendLayout();
            this.tableLayoutPanelNumberChartType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBubble)).BeginInit();
            this.tableLayoutPanelNumberGrouping.SuspendLayout();
            this.tabPageMetrics.SuspendLayout();
            this.tableLayoutPanelMetric.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCreateChart
            // 
            this.buttonCreateChart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCreateChart.FlatAppearance.BorderSize = 0;
            this.buttonCreateChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreateChart.Image = global::DiversityCollection.Resource.Graph;
            this.buttonCreateChart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCreateChart.Location = new System.Drawing.Point(3, 320);
            this.buttonCreateChart.Name = "buttonCreateChart";
            this.tableLayoutPanelChart.SetRowSpan(this.buttonCreateChart, 2);
            this.buttonCreateChart.Size = new System.Drawing.Size(26, 23);
            this.buttonCreateChart.TabIndex = 1;
            this.buttonCreateChart.TabStop = false;
            this.buttonCreateChart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCreateChart.UseVisualStyleBackColor = true;
            this.buttonCreateChart.Click += new System.EventHandler(this.buttonCreateChart_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageChart);
            this.tabControlMain.Controls.Add(this.tabPageSettings);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageListTabs;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(517, 373);
            this.tabControlMain.TabIndex = 8;
            // 
            // tabPageChart
            // 
            this.tabPageChart.Controls.Add(this.tableLayoutPanelChart);
            this.tabPageChart.ImageIndex = 0;
            this.tabPageChart.Location = new System.Drawing.Point(4, 23);
            this.tabPageChart.Name = "tabPageChart";
            this.tabPageChart.Size = new System.Drawing.Size(509, 346);
            this.tabPageChart.TabIndex = 0;
            this.tabPageChart.Text = "Chart";
            this.tabPageChart.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelChart
            // 
            this.tableLayoutPanelChart.ColumnCount = 6;
            this.tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelChart.Controls.Add(this.buttonCreateChart, 0, 1);
            this.tableLayoutPanelChart.Controls.Add(this.chartTask, 0, 0);
            this.tableLayoutPanelChart.Controls.Add(this.buttonOpenChart, 1, 1);
            this.tableLayoutPanelChart.Controls.Add(this.groupBoxExport, 5, 1);
            this.tableLayoutPanelChart.Controls.Add(this.progressBar, 2, 1);
            this.tableLayoutPanelChart.Controls.Add(this.buttonSaveImage, 4, 1);
            this.tableLayoutPanelChart.Controls.Add(this.buttonPrometheus, 3, 1);
            this.tableLayoutPanelChart.Controls.Add(this.labelCollection, 2, 2);
            this.tableLayoutPanelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelChart.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelChart.Name = "tableLayoutPanelChart";
            this.tableLayoutPanelChart.RowCount = 3;
            this.tableLayoutPanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelChart.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelChart.Size = new System.Drawing.Size(509, 346);
            this.tableLayoutPanelChart.TabIndex = 1;
            // 
            // chartTask
            // 
            chartArea1.Name = "ChartArea";
            this.chartTask.ChartAreas.Add(chartArea1);
            this.tableLayoutPanelChart.SetColumnSpan(this.chartTask, 6);
            this.chartTask.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartTask.Legends.Add(legend1);
            this.chartTask.Location = new System.Drawing.Point(3, 3);
            this.chartTask.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.chartTask.Name = "chartTask";
            this.chartTask.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Bright;
            series1.ChartArea = "ChartArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            series2.ChartArea = "ChartArea";
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.Label = "Gr.";
            series3.LabelToolTip = "Grundreinigung";
            series3.Legend = "Legend1";
            series3.MarkerImage = "D:\\Bilder\\Icons\\Overview\\Cleaning.ico";
            series3.Name = "Series3";
            series4.ChartArea = "ChartArea";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.Legend = "Legend1";
            series4.MarkerColor = System.Drawing.Color.Red;
            series4.MarkerImage = "D:\\Bilder\\Icons\\Overview\\Animal.ico";
            series4.Name = "Series4";
            this.chartTask.Series.Add(series1);
            this.chartTask.Series.Add(series2);
            this.chartTask.Series.Add(series3);
            this.chartTask.Series.Add(series4);
            this.chartTask.Size = new System.Drawing.Size(503, 308);
            this.chartTask.TabIndex = 0;
            this.chartTask.Text = "chart1";
            // 
            // buttonOpenChart
            // 
            this.buttonOpenChart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonOpenChart.FlatAppearance.BorderSize = 0;
            this.buttonOpenChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenChart.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonOpenChart.Location = new System.Drawing.Point(35, 320);
            this.buttonOpenChart.Name = "buttonOpenChart";
            this.tableLayoutPanelChart.SetRowSpan(this.buttonOpenChart, 2);
            this.buttonOpenChart.Size = new System.Drawing.Size(20, 23);
            this.buttonOpenChart.TabIndex = 3;
            this.buttonOpenChart.UseVisualStyleBackColor = true;
            this.buttonOpenChart.Click += new System.EventHandler(this.buttonOpenChart_Click);
            // 
            // groupBoxExport
            // 
            this.groupBoxExport.Controls.Add(this.buttonExportOpen);
            this.groupBoxExport.Controls.Add(this.buttonExport);
            this.groupBoxExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxExport.Location = new System.Drawing.Point(452, 311);
            this.groupBoxExport.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.groupBoxExport.Name = "groupBoxExport";
            this.groupBoxExport.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tableLayoutPanelChart.SetRowSpan(this.groupBoxExport, 2);
            this.groupBoxExport.Size = new System.Drawing.Size(54, 32);
            this.groupBoxExport.TabIndex = 5;
            this.groupBoxExport.TabStop = false;
            this.groupBoxExport.Text = "Export";
            // 
            // buttonExportOpen
            // 
            this.buttonExportOpen.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExportOpen.FlatAppearance.BorderSize = 0;
            this.buttonExportOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExportOpen.Image = global::DiversityCollection.Resource.Open;
            this.buttonExportOpen.Location = new System.Drawing.Point(26, 13);
            this.buttonExportOpen.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.buttonExportOpen.Name = "buttonExportOpen";
            this.buttonExportOpen.Size = new System.Drawing.Size(25, 16);
            this.buttonExportOpen.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonExportOpen, "Open exported file");
            this.buttonExportOpen.UseVisualStyleBackColor = true;
            this.buttonExportOpen.Click += new System.EventHandler(this.buttonExportOpen_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonExport.FlatAppearance.BorderSize = 0;
            this.buttonExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExport.Image = global::DiversityCollection.Resource.Export;
            this.buttonExport.Location = new System.Drawing.Point(3, 13);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(25, 16);
            this.buttonExport.TabIndex = 4;
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(61, 314);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(348, 10);
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // buttonSaveImage
            // 
            this.buttonSaveImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonSaveImage.FlatAppearance.BorderSize = 0;
            this.buttonSaveImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveImage.Image = global::DiversityCollection.Resource.Save;
            this.buttonSaveImage.Location = new System.Drawing.Point(432, 320);
            this.buttonSaveImage.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.buttonSaveImage.Name = "buttonSaveImage";
            this.tableLayoutPanelChart.SetRowSpan(this.buttonSaveImage, 2);
            this.buttonSaveImage.Size = new System.Drawing.Size(20, 23);
            this.buttonSaveImage.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonSaveImage, "Save as image");
            this.buttonSaveImage.UseVisualStyleBackColor = true;
            this.buttonSaveImage.Click += new System.EventHandler(this.buttonSaveImage_Click);
            // 
            // buttonPrometheus
            // 
            this.buttonPrometheus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPrometheus.FlatAppearance.BorderSize = 0;
            this.buttonPrometheus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrometheus.Image = global::DiversityCollection.Resource.Prometheus;
            this.buttonPrometheus.Location = new System.Drawing.Point(415, 320);
            this.buttonPrometheus.Name = "buttonPrometheus";
            this.tableLayoutPanelChart.SetRowSpan(this.buttonPrometheus, 2);
            this.buttonPrometheus.Size = new System.Drawing.Size(14, 23);
            this.buttonPrometheus.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonPrometheus, "Open sensor source");
            this.buttonPrometheus.UseVisualStyleBackColor = true;
            this.buttonPrometheus.Visible = false;
            this.buttonPrometheus.Click += new System.EventHandler(this.buttonPrometheus_Click);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.tabControlSettings);
            this.tabPageSettings.ImageIndex = 2;
            this.tabPageSettings.Location = new System.Drawing.Point(4, 23);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(509, 346);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Controls.Add(this.tabPageRange);
            this.tabControlSettings.Controls.Add(this.tabPagePests);
            this.tabControlSettings.Controls.Add(this.tabPageMetrics);
            this.tabControlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSettings.ImageList = this.imageListTabs;
            this.tabControlSettings.Location = new System.Drawing.Point(3, 3);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(503, 340);
            this.tabControlSettings.TabIndex = 9;
            // 
            // tabPageRange
            // 
            this.tabPageRange.Controls.Add(this.tableLayoutPanelRange);
            this.tabPageRange.ImageIndex = 3;
            this.tabPageRange.Location = new System.Drawing.Point(4, 23);
            this.tabPageRange.Name = "tabPageRange";
            this.tabPageRange.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRange.Size = new System.Drawing.Size(495, 313);
            this.tabPageRange.TabIndex = 0;
            this.tabPageRange.Text = "Range";
            this.tabPageRange.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelRange
            // 
            this.tableLayoutPanelRange.ColumnCount = 2;
            this.tableLayoutPanelRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRange.Controls.Add(this.checkBoxStart, 0, 0);
            this.tableLayoutPanelRange.Controls.Add(this.dateTimePickerStart, 0, 1);
            this.tableLayoutPanelRange.Controls.Add(this.checkBoxEnd, 1, 0);
            this.tableLayoutPanelRange.Controls.Add(this.dateTimePickerEnd, 1, 1);
            this.tableLayoutPanelRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRange.Enabled = false;
            this.tableLayoutPanelRange.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelRange.Name = "tableLayoutPanelRange";
            this.tableLayoutPanelRange.RowCount = 2;
            this.tableLayoutPanelRange.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRange.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRange.Size = new System.Drawing.Size(489, 307);
            this.tableLayoutPanelRange.TabIndex = 0;
            // 
            // checkBoxStart
            // 
            this.checkBoxStart.AutoSize = true;
            this.checkBoxStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxStart.Location = new System.Drawing.Point(3, 3);
            this.checkBoxStart.Name = "checkBoxStart";
            this.checkBoxStart.Size = new System.Drawing.Size(238, 17);
            this.checkBoxStart.TabIndex = 0;
            this.checkBoxStart.Text = "Restrict to dates starting with:";
            this.checkBoxStart.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimePickerStart.Location = new System.Drawing.Point(3, 26);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(238, 20);
            this.dateTimePickerStart.TabIndex = 1;
            // 
            // checkBoxEnd
            // 
            this.checkBoxEnd.AutoSize = true;
            this.checkBoxEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxEnd.Location = new System.Drawing.Point(247, 3);
            this.checkBoxEnd.Name = "checkBoxEnd";
            this.checkBoxEnd.Size = new System.Drawing.Size(239, 17);
            this.checkBoxEnd.TabIndex = 2;
            this.checkBoxEnd.Text = "Restrict to dates until:";
            this.checkBoxEnd.UseVisualStyleBackColor = true;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(247, 26);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(239, 20);
            this.dateTimePickerEnd.TabIndex = 3;
            // 
            // tabPagePests
            // 
            this.tabPagePests.Controls.Add(this.tableLayoutPanelNumbers);
            this.tabPagePests.ImageIndex = 4;
            this.tabPagePests.Location = new System.Drawing.Point(4, 23);
            this.tabPagePests.Name = "tabPagePests";
            this.tabPagePests.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePests.Size = new System.Drawing.Size(495, 313);
            this.tabPagePests.TabIndex = 1;
            this.tabPagePests.Text = "Pests";
            this.tabPagePests.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelNumbers
            // 
            this.tableLayoutPanelNumbers.ColumnCount = 2;
            this.tableLayoutPanelNumbers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNumbers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNumbers.Controls.Add(this.tableLayoutPanelNumberChartType, 0, 0);
            this.tableLayoutPanelNumbers.Controls.Add(this.checkedListBoxPests, 1, 0);
            this.tableLayoutPanelNumbers.Controls.Add(this.tableLayoutPanelNumberGrouping, 0, 1);
            this.tableLayoutPanelNumbers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelNumbers.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelNumbers.Name = "tableLayoutPanelNumbers";
            this.tableLayoutPanelNumbers.RowCount = 2;
            this.tableLayoutPanelNumbers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNumbers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNumbers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNumbers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelNumbers.Size = new System.Drawing.Size(489, 307);
            this.tableLayoutPanelNumbers.TabIndex = 1;
            // 
            // tableLayoutPanelNumberChartType
            // 
            this.tableLayoutPanelNumberChartType.ColumnCount = 4;
            this.tableLayoutPanelNumberChartType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNumberChartType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNumberChartType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNumberChartType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNumberChartType.Controls.Add(this.labelChartType, 0, 0);
            this.tableLayoutPanelNumberChartType.Controls.Add(this.radioButtonChartTypeColumns, 1, 0);
            this.tableLayoutPanelNumberChartType.Controls.Add(this.radioButtonBubble, 2, 0);
            this.tableLayoutPanelNumberChartType.Controls.Add(this.numericUpDownBubble, 3, 0);
            this.tableLayoutPanelNumberChartType.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelNumberChartType.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelNumberChartType.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelNumberChartType.Name = "tableLayoutPanelNumberChartType";
            this.tableLayoutPanelNumberChartType.RowCount = 1;
            this.tableLayoutPanelNumberChartType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNumberChartType.Size = new System.Drawing.Size(283, 26);
            this.tableLayoutPanelNumberChartType.TabIndex = 2;
            // 
            // labelChartType
            // 
            this.labelChartType.AutoSize = true;
            this.labelChartType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelChartType.Location = new System.Drawing.Point(3, 0);
            this.labelChartType.Name = "labelChartType";
            this.labelChartType.Size = new System.Drawing.Size(58, 26);
            this.labelChartType.TabIndex = 0;
            this.labelChartType.Text = "Chart type:";
            this.labelChartType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButtonChartTypeColumns
            // 
            this.radioButtonChartTypeColumns.AutoSize = true;
            this.radioButtonChartTypeColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonChartTypeColumns.Location = new System.Drawing.Point(67, 3);
            this.radioButtonChartTypeColumns.Name = "radioButtonChartTypeColumns";
            this.radioButtonChartTypeColumns.Size = new System.Drawing.Size(65, 20);
            this.radioButtonChartTypeColumns.TabIndex = 1;
            this.radioButtonChartTypeColumns.Text = "Columns";
            this.radioButtonChartTypeColumns.UseVisualStyleBackColor = true;
            // 
            // radioButtonBubble
            // 
            this.radioButtonBubble.AutoSize = true;
            this.radioButtonBubble.Checked = true;
            this.radioButtonBubble.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonBubble.Location = new System.Drawing.Point(138, 3);
            this.radioButtonBubble.Name = "radioButtonBubble";
            this.radioButtonBubble.Size = new System.Drawing.Size(58, 20);
            this.radioButtonBubble.TabIndex = 2;
            this.radioButtonBubble.TabStop = true;
            this.radioButtonBubble.Text = "Bubble";
            this.radioButtonBubble.UseVisualStyleBackColor = true;
            // 
            // numericUpDownBubble
            // 
            this.numericUpDownBubble.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownBubble.Location = new System.Drawing.Point(202, 3);
            this.numericUpDownBubble.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownBubble.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownBubble.Name = "numericUpDownBubble";
            this.numericUpDownBubble.Size = new System.Drawing.Size(27, 20);
            this.numericUpDownBubble.TabIndex = 3;
            this.numericUpDownBubble.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownBubble.Visible = false;
            // 
            // checkedListBoxPests
            // 
            this.checkedListBoxPests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxPests.FormattingEnabled = true;
            this.checkedListBoxPests.Location = new System.Drawing.Point(286, 3);
            this.checkedListBoxPests.Name = "checkedListBoxPests";
            this.tableLayoutPanelNumbers.SetRowSpan(this.checkedListBoxPests, 3);
            this.checkedListBoxPests.Size = new System.Drawing.Size(200, 301);
            this.checkedListBoxPests.TabIndex = 8;
            // 
            // tableLayoutPanelNumberGrouping
            // 
            this.tableLayoutPanelNumberGrouping.ColumnCount = 5;
            this.tableLayoutPanelNumberGrouping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNumberGrouping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNumberGrouping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNumberGrouping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelNumberGrouping.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNumberGrouping.Controls.Add(this.labelNumberGrouping, 0, 0);
            this.tableLayoutPanelNumberGrouping.Controls.Add(this.radioButtonNumberGroupingHour, 0, 1);
            this.tableLayoutPanelNumberGrouping.Controls.Add(this.radioButtonNumberGroupingYear, 4, 1);
            this.tableLayoutPanelNumberGrouping.Controls.Add(this.radioButtonNumberGroupingMonth, 3, 1);
            this.tableLayoutPanelNumberGrouping.Controls.Add(this.radioButtonNumberGroupingDay, 1, 1);
            this.tableLayoutPanelNumberGrouping.Controls.Add(this.radioButtonNumberGroupingWeek, 2, 1);
            this.tableLayoutPanelNumberGrouping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelNumberGrouping.Location = new System.Drawing.Point(0, 26);
            this.tableLayoutPanelNumberGrouping.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelNumberGrouping.Name = "tableLayoutPanelNumberGrouping";
            this.tableLayoutPanelNumberGrouping.RowCount = 2;
            this.tableLayoutPanelNumberGrouping.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNumberGrouping.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNumberGrouping.Size = new System.Drawing.Size(283, 46);
            this.tableLayoutPanelNumberGrouping.TabIndex = 0;
            // 
            // labelNumberGrouping
            // 
            this.labelNumberGrouping.AutoSize = true;
            this.tableLayoutPanelNumberGrouping.SetColumnSpan(this.labelNumberGrouping, 4);
            this.labelNumberGrouping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumberGrouping.Location = new System.Drawing.Point(3, 0);
            this.labelNumberGrouping.Name = "labelNumberGrouping";
            this.labelNumberGrouping.Size = new System.Drawing.Size(219, 13);
            this.labelNumberGrouping.TabIndex = 2;
            this.labelNumberGrouping.Text = "Group values according to";
            this.labelNumberGrouping.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButtonNumberGroupingHour
            // 
            this.radioButtonNumberGroupingHour.AutoSize = true;
            this.radioButtonNumberGroupingHour.Enabled = false;
            this.radioButtonNumberGroupingHour.Location = new System.Drawing.Point(3, 16);
            this.radioButtonNumberGroupingHour.Name = "radioButtonNumberGroupingHour";
            this.radioButtonNumberGroupingHour.Size = new System.Drawing.Size(48, 17);
            this.radioButtonNumberGroupingHour.TabIndex = 6;
            this.radioButtonNumberGroupingHour.Text = "Hour";
            this.radioButtonNumberGroupingHour.UseVisualStyleBackColor = true;
            // 
            // radioButtonNumberGroupingYear
            // 
            this.radioButtonNumberGroupingYear.AutoSize = true;
            this.radioButtonNumberGroupingYear.Enabled = false;
            this.radioButtonNumberGroupingYear.Location = new System.Drawing.Point(228, 16);
            this.radioButtonNumberGroupingYear.Name = "radioButtonNumberGroupingYear";
            this.radioButtonNumberGroupingYear.Size = new System.Drawing.Size(47, 17);
            this.radioButtonNumberGroupingYear.TabIndex = 5;
            this.radioButtonNumberGroupingYear.Text = "Year";
            this.radioButtonNumberGroupingYear.UseVisualStyleBackColor = true;
            // 
            // radioButtonNumberGroupingMonth
            // 
            this.radioButtonNumberGroupingMonth.AutoSize = true;
            this.radioButtonNumberGroupingMonth.Enabled = false;
            this.radioButtonNumberGroupingMonth.Location = new System.Drawing.Point(167, 16);
            this.radioButtonNumberGroupingMonth.Name = "radioButtonNumberGroupingMonth";
            this.radioButtonNumberGroupingMonth.Size = new System.Drawing.Size(55, 17);
            this.radioButtonNumberGroupingMonth.TabIndex = 4;
            this.radioButtonNumberGroupingMonth.Text = "Month";
            this.radioButtonNumberGroupingMonth.UseVisualStyleBackColor = true;
            // 
            // radioButtonNumberGroupingDay
            // 
            this.radioButtonNumberGroupingDay.AutoSize = true;
            this.radioButtonNumberGroupingDay.Checked = true;
            this.radioButtonNumberGroupingDay.Enabled = false;
            this.radioButtonNumberGroupingDay.Location = new System.Drawing.Point(57, 16);
            this.radioButtonNumberGroupingDay.Name = "radioButtonNumberGroupingDay";
            this.radioButtonNumberGroupingDay.Size = new System.Drawing.Size(44, 17);
            this.radioButtonNumberGroupingDay.TabIndex = 3;
            this.radioButtonNumberGroupingDay.TabStop = true;
            this.radioButtonNumberGroupingDay.Text = "Day";
            this.radioButtonNumberGroupingDay.UseVisualStyleBackColor = true;
            // 
            // radioButtonNumberGroupingWeek
            // 
            this.radioButtonNumberGroupingWeek.AutoSize = true;
            this.radioButtonNumberGroupingWeek.Enabled = false;
            this.radioButtonNumberGroupingWeek.Location = new System.Drawing.Point(107, 16);
            this.radioButtonNumberGroupingWeek.Name = "radioButtonNumberGroupingWeek";
            this.radioButtonNumberGroupingWeek.Size = new System.Drawing.Size(54, 17);
            this.radioButtonNumberGroupingWeek.TabIndex = 7;
            this.radioButtonNumberGroupingWeek.Text = "Week";
            this.radioButtonNumberGroupingWeek.UseVisualStyleBackColor = true;
            // 
            // tabPageMetrics
            // 
            this.tabPageMetrics.Controls.Add(this.tableLayoutPanelMetric);
            this.tabPageMetrics.ImageIndex = 1;
            this.tabPageMetrics.Location = new System.Drawing.Point(4, 23);
            this.tabPageMetrics.Name = "tabPageMetrics";
            this.tabPageMetrics.Size = new System.Drawing.Size(495, 313);
            this.tabPageMetrics.TabIndex = 2;
            this.tabPageMetrics.Text = "Metrics";
            this.tabPageMetrics.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelMetric
            // 
            this.tableLayoutPanelMetric.ColumnCount = 5;
            this.tableLayoutPanelMetric.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMetric.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMetric.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMetric.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMetric.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMetric.Controls.Add(this.labelMetricGrouping, 0, 0);
            this.tableLayoutPanelMetric.Controls.Add(this.radioButtonMetricGroupingHour, 0, 1);
            this.tableLayoutPanelMetric.Controls.Add(this.checkedListBoxMetrics, 4, 0);
            this.tableLayoutPanelMetric.Controls.Add(this.radioButtonMetricGroupingDay, 1, 1);
            this.tableLayoutPanelMetric.Controls.Add(this.radioButtonMetricGroupingWeek, 2, 1);
            this.tableLayoutPanelMetric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMetric.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMetric.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelMetric.Name = "tableLayoutPanelMetric";
            this.tableLayoutPanelMetric.RowCount = 2;
            this.tableLayoutPanelMetric.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMetric.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMetric.Size = new System.Drawing.Size(495, 313);
            this.tableLayoutPanelMetric.TabIndex = 1;
            // 
            // labelMetricGrouping
            // 
            this.labelMetricGrouping.AutoSize = true;
            this.tableLayoutPanelMetric.SetColumnSpan(this.labelMetricGrouping, 3);
            this.labelMetricGrouping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMetricGrouping.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMetricGrouping.Location = new System.Drawing.Point(3, 0);
            this.labelMetricGrouping.Name = "labelMetricGrouping";
            this.labelMetricGrouping.Size = new System.Drawing.Size(158, 13);
            this.labelMetricGrouping.TabIndex = 2;
            this.labelMetricGrouping.Text = "Group metric according to";
            this.labelMetricGrouping.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radioButtonMetricGroupingHour
            // 
            this.radioButtonMetricGroupingHour.AutoSize = true;
            this.radioButtonMetricGroupingHour.Enabled = false;
            this.radioButtonMetricGroupingHour.Location = new System.Drawing.Point(3, 16);
            this.radioButtonMetricGroupingHour.Name = "radioButtonMetricGroupingHour";
            this.radioButtonMetricGroupingHour.Size = new System.Drawing.Size(48, 17);
            this.radioButtonMetricGroupingHour.TabIndex = 6;
            this.radioButtonMetricGroupingHour.TabStop = true;
            this.radioButtonMetricGroupingHour.Text = "Hour";
            this.radioButtonMetricGroupingHour.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxMetrics
            // 
            this.checkedListBoxMetrics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBoxMetrics.FormattingEnabled = true;
            this.checkedListBoxMetrics.Location = new System.Drawing.Point(292, 3);
            this.checkedListBoxMetrics.Name = "checkedListBoxMetrics";
            this.tableLayoutPanelMetric.SetRowSpan(this.checkedListBoxMetrics, 2);
            this.checkedListBoxMetrics.Size = new System.Drawing.Size(200, 307);
            this.checkedListBoxMetrics.TabIndex = 8;
            // 
            // radioButtonMetricGroupingDay
            // 
            this.radioButtonMetricGroupingDay.AutoSize = true;
            this.radioButtonMetricGroupingDay.Checked = true;
            this.radioButtonMetricGroupingDay.Enabled = false;
            this.radioButtonMetricGroupingDay.Location = new System.Drawing.Point(57, 16);
            this.radioButtonMetricGroupingDay.Name = "radioButtonMetricGroupingDay";
            this.radioButtonMetricGroupingDay.Size = new System.Drawing.Size(44, 17);
            this.radioButtonMetricGroupingDay.TabIndex = 3;
            this.radioButtonMetricGroupingDay.TabStop = true;
            this.radioButtonMetricGroupingDay.Text = "Day";
            this.radioButtonMetricGroupingDay.UseVisualStyleBackColor = true;
            // 
            // radioButtonMetricGroupingWeek
            // 
            this.radioButtonMetricGroupingWeek.AutoSize = true;
            this.radioButtonMetricGroupingWeek.Enabled = false;
            this.radioButtonMetricGroupingWeek.Location = new System.Drawing.Point(107, 16);
            this.radioButtonMetricGroupingWeek.Name = "radioButtonMetricGroupingWeek";
            this.radioButtonMetricGroupingWeek.Size = new System.Drawing.Size(54, 17);
            this.radioButtonMetricGroupingWeek.TabIndex = 7;
            this.radioButtonMetricGroupingWeek.TabStop = true;
            this.radioButtonMetricGroupingWeek.Text = "Week";
            this.radioButtonMetricGroupingWeek.UseVisualStyleBackColor = true;
            // 
            // imageListTabs
            // 
            this.imageListTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTabs.ImageStream")));
            this.imageListTabs.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTabs.Images.SetKeyName(0, "Graph.ico");
            this.imageListTabs.Images.SetKeyName(1, "Parameter.ico");
            this.imageListTabs.Images.SetKeyName(2, "Settings.ico");
            this.imageListTabs.Images.SetKeyName(3, "Kalender.ico");
            this.imageListTabs.Images.SetKeyName(4, "Bug.ico");
            // 
            // labelCollection
            // 
            this.labelCollection.AutoSize = true;
            this.labelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollection.Location = new System.Drawing.Point(61, 327);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(348, 19);
            this.labelCollection.TabIndex = 9;
            this.labelCollection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserControlChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserControlChart";
            this.Size = new System.Drawing.Size(517, 373);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageChart.ResumeLayout(false);
            this.tableLayoutPanelChart.ResumeLayout(false);
            this.tableLayoutPanelChart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTask)).EndInit();
            this.groupBoxExport.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageRange.ResumeLayout(false);
            this.tableLayoutPanelRange.ResumeLayout(false);
            this.tableLayoutPanelRange.PerformLayout();
            this.tabPagePests.ResumeLayout(false);
            this.tableLayoutPanelNumbers.ResumeLayout(false);
            this.tableLayoutPanelNumberChartType.ResumeLayout(false);
            this.tableLayoutPanelNumberChartType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBubble)).EndInit();
            this.tableLayoutPanelNumberGrouping.ResumeLayout(false);
            this.tableLayoutPanelNumberGrouping.PerformLayout();
            this.tabPageMetrics.ResumeLayout(false);
            this.tableLayoutPanelMetric.ResumeLayout(false);
            this.tableLayoutPanelMetric.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTask;
        private System.Windows.Forms.Button buttonCreateChart;
        private System.Windows.Forms.Label labelNumberGrouping;
        private System.Windows.Forms.RadioButton radioButtonNumberGroupingMonth;
        private System.Windows.Forms.RadioButton radioButtonNumberGroupingYear;
        private System.Windows.Forms.RadioButton radioButtonNumberGroupingDay;
        private System.Windows.Forms.RadioButton radioButtonNumberGroupingHour;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNumberGrouping;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMetric;
        private System.Windows.Forms.Label labelMetricGrouping;
        private System.Windows.Forms.RadioButton radioButtonMetricGroupingHour;
        private System.Windows.Forms.RadioButton radioButtonMetricGroupingDay;
        private System.Windows.Forms.RadioButton radioButtonNumberGroupingWeek;
        private System.Windows.Forms.RadioButton radioButtonMetricGroupingWeek;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageChart;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.ImageList imageListTabs;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelChart;
        private System.Windows.Forms.CheckedListBox checkedListBoxMetrics;
        private System.Windows.Forms.CheckedListBox checkedListBoxPests;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNumberChartType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNumbers;
        private System.Windows.Forms.Label labelChartType;
        private System.Windows.Forms.RadioButton radioButtonChartTypeColumns;
        private System.Windows.Forms.RadioButton radioButtonBubble;
        private System.Windows.Forms.NumericUpDown numericUpDownBubble;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRange;
        private System.Windows.Forms.Button buttonOpenChart;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageRange;
        private System.Windows.Forms.TabPage tabPagePests;
        private System.Windows.Forms.TabPage tabPageMetrics;
        private System.Windows.Forms.CheckBox checkBoxStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.CheckBox checkBoxEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.GroupBox groupBoxExport;
        private System.Windows.Forms.Button buttonExportOpen;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button buttonSaveImage;
        private System.Windows.Forms.Button buttonPrometheus;
        private System.Windows.Forms.Label labelCollection;
    }
}
