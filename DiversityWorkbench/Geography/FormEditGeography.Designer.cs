namespace DiversityWorkbench.Geography
{
    partial class FormEditGeography
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditGeography));
            this.dataColumnAltitudeDeg = new System.Data.DataColumn();
            this.dataColumnLatitudeMin = new System.Data.DataColumn();
            this.dataColumnLatitudeDeg = new System.Data.DataColumn();
            this.dataColumnLongitudeSec = new System.Data.DataColumn();
            this.dataColumnLongitudeMin = new System.Data.DataColumn();
            this.dataColumnLongitudeDeg = new System.Data.DataColumn();
            this.dataTableDegree = new System.Data.DataTable();
            this.dataColumnLatitudeSec = new System.Data.DataColumn();
            this.dataColumnAltitude = new System.Data.DataColumn();
            this.dataColumnLatitude = new System.Data.DataColumn();
            this.dataColumnLongitude = new System.Data.DataColumn();
            this.dataTableNumeric = new System.Data.DataTable();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.comboBoxNumericOrDegMinSec = new System.Windows.Forms.ComboBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.dataGridViewGeography = new System.Windows.Forms.DataGridView();
            this.ColumnLatitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLongitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAlti = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelDisplay = new System.Windows.Forms.Label();
            this.groupBoxType = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelType = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonTypePoint = new System.Windows.Forms.RadioButton();
            this.radioButtonTypeLine = new System.Windows.Forms.RadioButton();
            this.radioButtonTypePolygon = new System.Windows.Forms.RadioButton();
            this.radioButtonTypeMultipoint = new System.Windows.Forms.RadioButton();
            this.groupBoxDisplay = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelDisplay = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonDisplayNumeric = new System.Windows.Forms.RadioButton();
            this.radioButtonDisplayDegree = new System.Windows.Forms.RadioButton();
            this.checkBoxIncludeAltitude = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataSet = new System.Data.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableDegree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeography)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBoxType.SuspendLayout();
            this.tableLayoutPanelType.SuspendLayout();
            this.groupBoxDisplay.SuspendLayout();
            this.tableLayoutPanelDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dataColumnAltitudeDeg
            // 
            this.dataColumnAltitudeDeg.ColumnName = "Altitude";
            this.dataColumnAltitudeDeg.DataType = typeof(decimal);
            this.dataColumnAltitudeDeg.DefaultValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // dataColumnLatitudeMin
            // 
            this.dataColumnLatitudeMin.Caption = "Min \'";
            this.dataColumnLatitudeMin.ColumnName = "MinLat";
            this.dataColumnLatitudeMin.DataType = typeof(short);
            this.dataColumnLatitudeMin.DefaultValue = ((short)(0));
            // 
            // dataColumnLatitudeDeg
            // 
            this.dataColumnLatitudeDeg.Caption = "Latitude °";
            this.dataColumnLatitudeDeg.ColumnName = "LatitudeDeg";
            this.dataColumnLatitudeDeg.DataType = typeof(short);
            this.dataColumnLatitudeDeg.DefaultValue = ((short)(0));
            // 
            // dataColumnLongitudeSec
            // 
            this.dataColumnLongitudeSec.Caption = "Sec \'\'";
            this.dataColumnLongitudeSec.ColumnName = "SecLong";
            this.dataColumnLongitudeSec.DataType = typeof(decimal);
            this.dataColumnLongitudeSec.DefaultValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // dataColumnLongitudeMin
            // 
            this.dataColumnLongitudeMin.Caption = "Min \'";
            this.dataColumnLongitudeMin.ColumnName = "MinLong";
            this.dataColumnLongitudeMin.DataType = typeof(short);
            this.dataColumnLongitudeMin.DefaultValue = ((short)(0));
            // 
            // dataColumnLongitudeDeg
            // 
            this.dataColumnLongitudeDeg.Caption = "Longitude °";
            this.dataColumnLongitudeDeg.ColumnName = "Longitude";
            this.dataColumnLongitudeDeg.DataType = typeof(short);
            this.dataColumnLongitudeDeg.DefaultValue = ((short)(0));
            // 
            // dataTableDegree
            // 
            this.dataTableDegree.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumnLongitudeDeg,
            this.dataColumnLongitudeMin,
            this.dataColumnLongitudeSec,
            this.dataColumnLatitudeDeg,
            this.dataColumnLatitudeMin,
            this.dataColumnLatitudeSec,
            this.dataColumnAltitudeDeg});
            this.dataTableDegree.TableName = "TableDegree";
            // 
            // dataColumnLatitudeSec
            // 
            this.dataColumnLatitudeSec.Caption = "Lat \'\'";
            this.dataColumnLatitudeSec.ColumnName = "SecLat";
            this.dataColumnLatitudeSec.DataType = typeof(decimal);
            this.dataColumnLatitudeSec.DefaultValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // dataColumnAltitude
            // 
            this.dataColumnAltitude.ColumnName = "Altitude";
            this.dataColumnAltitude.DataType = typeof(double);
            this.dataColumnAltitude.DefaultValue = 0D;
            // 
            // dataColumnLatitude
            // 
            this.dataColumnLatitude.ColumnName = "Latitude";
            this.dataColumnLatitude.DataType = typeof(double);
            this.dataColumnLatitude.DefaultValue = 0D;
            // 
            // dataColumnLongitude
            // 
            this.dataColumnLongitude.ColumnName = "Longitude";
            this.dataColumnLongitude.DataType = typeof(double);
            this.dataColumnLongitude.DefaultValue = 0D;
            // 
            // dataTableNumeric
            // 
            this.dataTableNumeric.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumnLongitude,
            this.dataColumnLatitude,
            this.dataColumnAltitude});
            this.dataTableNumeric.TableName = "TableNumeric";
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Location = new System.Drawing.Point(3, 71);
            this.labelType.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(34, 27);
            this.labelType.TabIndex = 2;
            this.labelType.Text = "Type:";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelType.Visible = false;
            // 
            // comboBoxType
            // 
            this.comboBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "POINT",
            "MULTIPOINT",
            "LINESTRING",
            "POLYGON"});
            this.comboBoxType.Location = new System.Drawing.Point(37, 74);
            this.comboBoxType.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(140, 21);
            this.comboBoxType.TabIndex = 3;
            this.toolTip.SetToolTip(this.comboBoxType, "Type of the geography, e.g. POINT");
            this.comboBoxType.Visible = false;
            // 
            // comboBoxNumericOrDegMinSec
            // 
            this.comboBoxNumericOrDegMinSec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxNumericOrDegMinSec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNumericOrDegMinSec.FormattingEnabled = true;
            this.comboBoxNumericOrDegMinSec.Items.AddRange(new object[] {
            "numeric",
            "deg min sec"});
            this.comboBoxNumericOrDegMinSec.Location = new System.Drawing.Point(227, 74);
            this.comboBoxNumericOrDegMinSec.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxNumericOrDegMinSec.Name = "comboBoxNumericOrDegMinSec";
            this.comboBoxNumericOrDegMinSec.Size = new System.Drawing.Size(141, 21);
            this.comboBoxNumericOrDegMinSec.TabIndex = 4;
            this.comboBoxNumericOrDegMinSec.Visible = false;
            this.comboBoxNumericOrDegMinSec.SelectedIndexChanged += new System.EventHandler(this.comboBoxNumericOrDegMinSec_SelectedIndexChanged);
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelHeader, 3);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(221, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Edit geography";
            this.toolTip.SetToolTip(this.labelHeader, "Point");
            // 
            // dataGridViewGeography
            // 
            this.dataGridViewGeography.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGeography.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLatitude,
            this.ColumnLongitude,
            this.ColumnAlti});
            this.tableLayoutPanel.SetColumnSpan(this.dataGridViewGeography, 4);
            this.dataGridViewGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewGeography.Location = new System.Drawing.Point(3, 101);
            this.dataGridViewGeography.Name = "dataGridViewGeography";
            this.dataGridViewGeography.Size = new System.Drawing.Size(365, 224);
            this.dataGridViewGeography.TabIndex = 1;
            this.dataGridViewGeography.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGeography_CellLeave);
            this.dataGridViewGeography.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewGeography_CellValidating);
            this.dataGridViewGeography.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewGeography_RowLeave);
            this.dataGridViewGeography.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewGeography_RowValidating);
            // 
            // ColumnLatitude
            // 
            this.ColumnLatitude.HeaderText = "Latitude";
            this.ColumnLatitude.Name = "ColumnLatitude";
            // 
            // ColumnLongitude
            // 
            this.ColumnLongitude.HeaderText = "Longitude";
            this.ColumnLongitude.Name = "ColumnLongitude";
            // 
            // ColumnAlti
            // 
            this.ColumnAlti.HeaderText = "Altitude";
            this.ColumnAlti.Name = "ColumnAlti";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.dataGridViewGeography, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelType, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBoxType, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.comboBoxNumericOrDegMinSec, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.labelDisplay, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.groupBoxType, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.groupBoxDisplay, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.checkBoxIncludeAltitude, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonCancel, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.buttonOK, 3, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(371, 357);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // labelDisplay
            // 
            this.labelDisplay.AutoSize = true;
            this.labelDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplay.Location = new System.Drawing.Point(183, 71);
            this.labelDisplay.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDisplay.Name = "labelDisplay";
            this.labelDisplay.Size = new System.Drawing.Size(44, 27);
            this.labelDisplay.TabIndex = 5;
            this.labelDisplay.Text = "Display:";
            this.labelDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelDisplay.Visible = false;
            // 
            // groupBoxType
            // 
            this.tableLayoutPanel.SetColumnSpan(this.groupBoxType, 2);
            this.groupBoxType.Controls.Add(this.tableLayoutPanelType);
            this.groupBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxType.Location = new System.Drawing.Point(3, 28);
            this.groupBoxType.MinimumSize = new System.Drawing.Size(136, 0);
            this.groupBoxType.Name = "groupBoxType";
            this.groupBoxType.Size = new System.Drawing.Size(174, 40);
            this.groupBoxType.TabIndex = 6;
            this.groupBoxType.TabStop = false;
            this.groupBoxType.Text = "Type";
            // 
            // tableLayoutPanelType
            // 
            this.tableLayoutPanelType.ColumnCount = 4;
            this.tableLayoutPanelType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelType.Controls.Add(this.radioButtonTypePoint, 0, 0);
            this.tableLayoutPanelType.Controls.Add(this.radioButtonTypeLine, 2, 0);
            this.tableLayoutPanelType.Controls.Add(this.radioButtonTypePolygon, 3, 0);
            this.tableLayoutPanelType.Controls.Add(this.radioButtonTypeMultipoint, 1, 0);
            this.tableLayoutPanelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelType.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelType.Name = "tableLayoutPanelType";
            this.tableLayoutPanelType.RowCount = 1;
            this.tableLayoutPanelType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelType.Size = new System.Drawing.Size(168, 21);
            this.tableLayoutPanelType.TabIndex = 0;
            // 
            // radioButtonTypePoint
            // 
            this.radioButtonTypePoint.AutoSize = true;
            this.radioButtonTypePoint.Checked = true;
            this.radioButtonTypePoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonTypePoint.ForeColor = System.Drawing.Color.Red;
            this.radioButtonTypePoint.Location = new System.Drawing.Point(3, 3);
            this.radioButtonTypePoint.Name = "radioButtonTypePoint";
            this.radioButtonTypePoint.Size = new System.Drawing.Size(32, 15);
            this.radioButtonTypePoint.TabIndex = 0;
            this.radioButtonTypePoint.TabStop = true;
            this.radioButtonTypePoint.Text = "•";
            this.radioButtonTypePoint.UseVisualStyleBackColor = true;
            this.radioButtonTypePoint.Click += new System.EventHandler(this.radioButtonTypePoint_Click);
            // 
            // radioButtonTypeLine
            // 
            this.radioButtonTypeLine.AutoSize = true;
            this.radioButtonTypeLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonTypeLine.ForeColor = System.Drawing.Color.Red;
            this.radioButtonTypeLine.Location = new System.Drawing.Point(87, 3);
            this.radioButtonTypeLine.Name = "radioButtonTypeLine";
            this.radioButtonTypeLine.Size = new System.Drawing.Size(28, 15);
            this.radioButtonTypeLine.TabIndex = 2;
            this.radioButtonTypeLine.Text = "|";
            this.toolTip.SetToolTip(this.radioButtonTypeLine, "Line");
            this.radioButtonTypeLine.UseVisualStyleBackColor = true;
            this.radioButtonTypeLine.Click += new System.EventHandler(this.radioButtonTypeLine_Click);
            // 
            // radioButtonTypePolygon
            // 
            this.radioButtonTypePolygon.AutoSize = true;
            this.radioButtonTypePolygon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonTypePolygon.ForeColor = System.Drawing.Color.Red;
            this.radioButtonTypePolygon.Location = new System.Drawing.Point(129, 3);
            this.radioButtonTypePolygon.Name = "radioButtonTypePolygon";
            this.radioButtonTypePolygon.Size = new System.Drawing.Size(33, 15);
            this.radioButtonTypePolygon.TabIndex = 3;
            this.radioButtonTypePolygon.Text = "□";
            this.toolTip.SetToolTip(this.radioButtonTypePolygon, "Polygon");
            this.radioButtonTypePolygon.UseVisualStyleBackColor = true;
            this.radioButtonTypePolygon.Click += new System.EventHandler(this.radioButtonTypePolygon_Click);
            // 
            // radioButtonTypeMultipoint
            // 
            this.radioButtonTypeMultipoint.AutoSize = true;
            this.radioButtonTypeMultipoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonTypeMultipoint.ForeColor = System.Drawing.Color.Red;
            this.radioButtonTypeMultipoint.Location = new System.Drawing.Point(45, 3);
            this.radioButtonTypeMultipoint.Name = "radioButtonTypeMultipoint";
            this.radioButtonTypeMultipoint.Size = new System.Drawing.Size(29, 15);
            this.radioButtonTypeMultipoint.TabIndex = 1;
            this.radioButtonTypeMultipoint.Text = "⁞";
            this.toolTip.SetToolTip(this.radioButtonTypeMultipoint, "Many points");
            this.radioButtonTypeMultipoint.UseVisualStyleBackColor = true;
            this.radioButtonTypeMultipoint.Click += new System.EventHandler(this.radioButtonTypeMultipoint_Click);
            // 
            // groupBoxDisplay
            // 
            this.tableLayoutPanel.SetColumnSpan(this.groupBoxDisplay, 2);
            this.groupBoxDisplay.Controls.Add(this.tableLayoutPanelDisplay);
            this.groupBoxDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDisplay.Location = new System.Drawing.Point(183, 28);
            this.groupBoxDisplay.Name = "groupBoxDisplay";
            this.groupBoxDisplay.Size = new System.Drawing.Size(185, 40);
            this.groupBoxDisplay.TabIndex = 7;
            this.groupBoxDisplay.TabStop = false;
            this.groupBoxDisplay.Text = "Display";
            // 
            // tableLayoutPanelDisplay
            // 
            this.tableLayoutPanelDisplay.ColumnCount = 2;
            this.tableLayoutPanelDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDisplay.Controls.Add(this.radioButtonDisplayNumeric, 0, 0);
            this.tableLayoutPanelDisplay.Controls.Add(this.radioButtonDisplayDegree, 1, 0);
            this.tableLayoutPanelDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDisplay.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelDisplay.Name = "tableLayoutPanelDisplay";
            this.tableLayoutPanelDisplay.RowCount = 1;
            this.tableLayoutPanelDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDisplay.Size = new System.Drawing.Size(179, 21);
            this.tableLayoutPanelDisplay.TabIndex = 0;
            // 
            // radioButtonDisplayNumeric
            // 
            this.radioButtonDisplayNumeric.AutoSize = true;
            this.radioButtonDisplayNumeric.Checked = true;
            this.radioButtonDisplayNumeric.ForeColor = System.Drawing.Color.Blue;
            this.radioButtonDisplayNumeric.Location = new System.Drawing.Point(3, 3);
            this.radioButtonDisplayNumeric.Name = "radioButtonDisplayNumeric";
            this.radioButtonDisplayNumeric.Size = new System.Drawing.Size(64, 15);
            this.radioButtonDisplayNumeric.TabIndex = 0;
            this.radioButtonDisplayNumeric.TabStop = true;
            this.radioButtonDisplayNumeric.Text = "8.63406";
            this.toolTip.SetToolTip(this.radioButtonDisplayNumeric, "numeric");
            this.radioButtonDisplayNumeric.UseVisualStyleBackColor = true;
            this.radioButtonDisplayNumeric.Click += new System.EventHandler(this.radioButtonDisplayNumeric_Click);
            // 
            // radioButtonDisplayDegree
            // 
            this.radioButtonDisplayDegree.AutoSize = true;
            this.radioButtonDisplayDegree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonDisplayDegree.ForeColor = System.Drawing.Color.Blue;
            this.radioButtonDisplayDegree.Location = new System.Drawing.Point(92, 3);
            this.radioButtonDisplayDegree.Name = "radioButtonDisplayDegree";
            this.radioButtonDisplayDegree.Size = new System.Drawing.Size(84, 15);
            this.radioButtonDisplayDegree.TabIndex = 1;
            this.radioButtonDisplayDegree.Text = "8° 31\' 2.6\'\'";
            this.toolTip.SetToolTip(this.radioButtonDisplayDegree, "degree, minutes, seconds");
            this.radioButtonDisplayDegree.UseVisualStyleBackColor = true;
            this.radioButtonDisplayDegree.Click += new System.EventHandler(this.radioButtonDisplayDegree_Click);
            // 
            // checkBoxIncludeAltitude
            // 
            this.checkBoxIncludeAltitude.AutoSize = true;
            this.checkBoxIncludeAltitude.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxIncludeAltitude.Location = new System.Drawing.Point(307, 3);
            this.checkBoxIncludeAltitude.Name = "checkBoxIncludeAltitude";
            this.checkBoxIncludeAltitude.Size = new System.Drawing.Size(61, 19);
            this.checkBoxIncludeAltitude.TabIndex = 8;
            this.checkBoxIncludeAltitude.Text = "Altitude";
            this.toolTip.SetToolTip(this.checkBoxIncludeAltitude, "include values for the altitude");
            this.checkBoxIncludeAltitude.UseVisualStyleBackColor = true;
            this.checkBoxIncludeAltitude.Click += new System.EventHandler(this.checkBoxIncludeAltitude_Click);
            // 
            // buttonCancel
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonCancel, 2);
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCancel.Location = new System.Drawing.Point(3, 331);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOK.Location = new System.Drawing.Point(293, 331);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // dataSet
            // 
            this.dataSet.DataSetName = "NewDataSet";
            this.dataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTableNumeric,
            this.dataTableDegree});
            // 
            // FormEditGeography
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 357);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEditGeography";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit geography";
            ((System.ComponentModel.ISupportInitialize)(this.dataTableDegree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTableNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeography)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBoxType.ResumeLayout(false);
            this.tableLayoutPanelType.ResumeLayout(false);
            this.tableLayoutPanelType.PerformLayout();
            this.groupBoxDisplay.ResumeLayout(false);
            this.tableLayoutPanelDisplay.ResumeLayout(false);
            this.tableLayoutPanelDisplay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Data.DataColumn dataColumnAltitudeDeg;
        private System.Data.DataColumn dataColumnLatitudeMin;
        private System.Data.DataColumn dataColumnLatitudeDeg;
        private System.Data.DataColumn dataColumnLongitudeSec;
        private System.Data.DataColumn dataColumnLongitudeMin;
        private System.Data.DataColumn dataColumnLongitudeDeg;
        private System.Data.DataTable dataTableDegree;
        private System.Data.DataColumn dataColumnLatitudeSec;
        private System.Data.DataColumn dataColumnAltitude;
        private System.Data.DataColumn dataColumnLatitude;
        private System.Data.DataColumn dataColumnLongitude;
        private System.Data.DataTable dataTableNumeric;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ComboBox comboBoxNumericOrDegMinSec;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.DataGridView dataGridViewGeography;
        private System.Data.DataSet dataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLatitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLongitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAlti;
        private System.Windows.Forms.Label labelDisplay;
        private System.Windows.Forms.GroupBox groupBoxType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelType;
        private System.Windows.Forms.RadioButton radioButtonTypePoint;
        private System.Windows.Forms.RadioButton radioButtonTypeLine;
        private System.Windows.Forms.RadioButton radioButtonTypePolygon;
        private System.Windows.Forms.RadioButton radioButtonTypeMultipoint;
        private System.Windows.Forms.GroupBox groupBoxDisplay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDisplay;
        private System.Windows.Forms.RadioButton radioButtonDisplayNumeric;
        private System.Windows.Forms.RadioButton radioButtonDisplayDegree;
        private System.Windows.Forms.CheckBox checkBoxIncludeAltitude;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}