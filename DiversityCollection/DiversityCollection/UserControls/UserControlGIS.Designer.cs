namespace DiversityCollection.UserControls
{
    partial class UserControlGIS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlGIS));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButtonState = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemStateOrganism = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStateEvent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStateSeries = new System.Windows.Forms.ToolStripMenuItem();
            this.anyCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStateDistribution = new System.Windows.Forms.ToolStripMenuItem();
            this.distInclOrgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonEditMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemEditModeBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEditModeGisNoEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEditModeGIS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabelSymbol = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDropDownButtonSymbol = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripDropDownButtonColor = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripLabelSize = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxSize = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonLineThickness = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBoxThickness = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButtonAddToDistribution = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRequeryDistribution = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.numericUpDownZoomLevel = new System.Windows.Forms.NumericUpDown();
            this.splitContainerMaps = new System.Windows.Forms.SplitContainer();
            this.panelWebbrowser = new System.Windows.Forms.Panel();
            this.userControlWebView = new DiversityWorkbench.UserControls.UserControlWebView();
            this.panelForWpfControl = new System.Windows.Forms.Panel();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.wpfControl = new WpfSamplingPlotPage.WpfControl();
            this.buttonReload = new System.Windows.Forms.Button();
            this.checkBoxZoomLevel = new System.Windows.Forms.CheckBox();
            this.buttonMapIsFixed = new System.Windows.Forms.Button();
            this.buttonCustomizeMapLocalisations = new System.Windows.Forms.Button();
            this.checkBoxIncludeAll = new System.Windows.Forms.CheckBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.imageListSymbol = new System.Windows.Forms.ImageList(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.imageListHold = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoomLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaps)).BeginInit();
            this.splitContainerMaps.Panel1.SuspendLayout();
            this.splitContainerMaps.Panel2.SuspendLayout();
            this.splitContainerMaps.SuspendLayout();
            this.panelWebbrowser.SuspendLayout();
            this.panelForWpfControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 8;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.toolStripMain, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.numericUpDownZoomLevel, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerMaps, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReload, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxZoomLevel, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonMapIsFixed, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCustomizeMapLocalisations, 7, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxIncludeAll, 5, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpKeyword(this.tableLayoutPanelMain, "Maps");
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanelMain, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this.tableLayoutPanelMain, "Maps");
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.helpProvider.SetShowHelp(this.tableLayoutPanelMain, true);
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(813, 498);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // toolStripMain
            // 
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButtonState,
            this.toolStripDropDownButtonEditMode,
            this.toolStripLabelSymbol,
            this.toolStripDropDownButtonSymbol,
            this.toolStripDropDownButtonColor,
            this.toolStripLabelSize,
            this.toolStripTextBoxSize,
            this.toolStripButtonLineThickness,
            this.toolStripTextBoxThickness,
            this.toolStripButtonAddToDistribution,
            this.toolStripButtonRequeryDistribution,
            this.toolStripButtonSave});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(525, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripDropDownButtonState
            // 
            this.toolStripDropDownButtonState.AutoSize = false;
            this.toolStripDropDownButtonState.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemStateOrganism,
            this.toolStripMenuItemStateEvent,
            this.toolStripMenuItemStateSeries,
            this.anyCoordinatesToolStripMenuItem,
            this.toolStripMenuItemStateDistribution,
            this.distInclOrgToolStripMenuItem});
            this.toolStripDropDownButtonState.Image = global::DiversityCollection.Resource.Event;
            this.toolStripDropDownButtonState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonState.Name = "toolStripDropDownButtonState";
            this.toolStripDropDownButtonState.Size = new System.Drawing.Size(122, 22);
            this.toolStripDropDownButtonState.Text = "Collection event";
            // 
            // toolStripMenuItemStateOrganism
            // 
            this.toolStripMenuItemStateOrganism.Image = global::DiversityCollection.Resource.GeoAnalysis;
            this.toolStripMenuItemStateOrganism.Name = "toolStripMenuItemStateOrganism";
            this.toolStripMenuItemStateOrganism.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItemStateOrganism.Text = "Organism";
            this.toolStripMenuItemStateOrganism.Click += new System.EventHandler(this.toolStripMenuItemStateOrganism_Click);
            // 
            // toolStripMenuItemStateEvent
            // 
            this.toolStripMenuItemStateEvent.Image = global::DiversityCollection.Resource.Event;
            this.toolStripMenuItemStateEvent.Name = "toolStripMenuItemStateEvent";
            this.toolStripMenuItemStateEvent.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItemStateEvent.Text = "Collection event";
            this.toolStripMenuItemStateEvent.Click += new System.EventHandler(this.toolStripMenuItemStateEvent_Click);
            // 
            // toolStripMenuItemStateSeries
            // 
            this.toolStripMenuItemStateSeries.Image = global::DiversityCollection.Resource.EventSeries;
            this.toolStripMenuItemStateSeries.Name = "toolStripMenuItemStateSeries";
            this.toolStripMenuItemStateSeries.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItemStateSeries.Text = "Event series";
            this.toolStripMenuItemStateSeries.Click += new System.EventHandler(this.toolStripMenuItemStateSeries_Click);
            // 
            // anyCoordinatesToolStripMenuItem
            // 
            this.anyCoordinatesToolStripMenuItem.Image = global::DiversityCollection.Resource.AnyCoordinates;
            this.anyCoordinatesToolStripMenuItem.Name = "anyCoordinatesToolStripMenuItem";
            this.anyCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.anyCoordinatesToolStripMenuItem.Text = "Any coordinates";
            this.anyCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.anyCoordinatesToolStripMenuItem_Click);
            // 
            // toolStripMenuItemStateDistribution
            // 
            this.toolStripMenuItemStateDistribution.Image = global::DiversityCollection.Resource.DistributionMap;
            this.toolStripMenuItemStateDistribution.Name = "toolStripMenuItemStateDistribution";
            this.toolStripMenuItemStateDistribution.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItemStateDistribution.Text = "Distribution";
            this.toolStripMenuItemStateDistribution.Click += new System.EventHandler(this.toolStripMenuItemStateDistribution_Click);
            // 
            // distInclOrgToolStripMenuItem
            // 
            this.distInclOrgToolStripMenuItem.Image = global::DiversityCollection.Resource.DistributionOrganisms;
            this.distInclOrgToolStripMenuItem.Name = "distInclOrgToolStripMenuItem";
            this.distInclOrgToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.distInclOrgToolStripMenuItem.Text = "Dist. incl. org.";
            this.distInclOrgToolStripMenuItem.ToolTipText = "Distribution including the organisms";
            this.distInclOrgToolStripMenuItem.Click += new System.EventHandler(this.distInclOrgToolStripMenuItem_Click);
            // 
            // toolStripDropDownButtonEditMode
            // 
            this.toolStripDropDownButtonEditMode.AutoSize = false;
            this.toolStripDropDownButtonEditMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemEditModeBrowser,
            this.toolStripMenuItemEditModeGisNoEdit,
            this.toolStripMenuItemEditModeGIS});
            this.toolStripDropDownButtonEditMode.Image = global::DiversityCollection.Resource.Lupe;
            this.toolStripDropDownButtonEditMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonEditMode.Name = "toolStripDropDownButtonEditMode";
            this.toolStripDropDownButtonEditMode.Size = new System.Drawing.Size(100, 22);
            this.toolStripDropDownButtonEditMode.Text = "Browser";
            // 
            // toolStripMenuItemEditModeBrowser
            // 
            this.toolStripMenuItemEditModeBrowser.Image = global::DiversityCollection.Resource.Lupe;
            this.toolStripMenuItemEditModeBrowser.Name = "toolStripMenuItemEditModeBrowser";
            this.toolStripMenuItemEditModeBrowser.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItemEditModeBrowser.Text = "Browser";
            this.toolStripMenuItemEditModeBrowser.Visible = false;
            this.toolStripMenuItemEditModeBrowser.Click += new System.EventHandler(this.toolStripMenuItemEditModeBrowser_Click);
            // 
            // toolStripMenuItemEditModeGisNoEdit
            // 
            this.toolStripMenuItemEditModeGisNoEdit.Image = global::DiversityCollection.Resource.EditNo;
            this.toolStripMenuItemEditModeGisNoEdit.Name = "toolStripMenuItemEditModeGisNoEdit";
            this.toolStripMenuItemEditModeGisNoEdit.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItemEditModeGisNoEdit.Text = "GIS - View";
            this.toolStripMenuItemEditModeGisNoEdit.Click += new System.EventHandler(this.toolStripMenuItemEditModeGisNoEdit_Click);
            // 
            // toolStripMenuItemEditModeGIS
            // 
            this.toolStripMenuItemEditModeGIS.Image = global::DiversityCollection.Resource.Edit1;
            this.toolStripMenuItemEditModeGIS.Name = "toolStripMenuItemEditModeGIS";
            this.toolStripMenuItemEditModeGIS.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItemEditModeGIS.Text = "GIS - Edit";
            this.toolStripMenuItemEditModeGIS.Click += new System.EventHandler(this.toolStripMenuItemEditModeGIS_Click);
            // 
            // toolStripLabelSymbol
            // 
            this.toolStripLabelSymbol.Name = "toolStripLabelSymbol";
            this.toolStripLabelSymbol.Size = new System.Drawing.Size(50, 22);
            this.toolStripLabelSymbol.Text = "Symbol:";
            // 
            // toolStripDropDownButtonSymbol
            // 
            this.toolStripDropDownButtonSymbol.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonSymbol.Image = global::DiversityCollection.Resource.Pin1;
            this.toolStripDropDownButtonSymbol.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSymbol.Name = "toolStripDropDownButtonSymbol";
            this.toolStripDropDownButtonSymbol.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonSymbol.Text = "Symbol";
            this.toolStripDropDownButtonSymbol.ToolTipText = "The symbol used for display in the map";
            // 
            // toolStripDropDownButtonColor
            // 
            this.toolStripDropDownButtonColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButtonColor.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonColor.Image")));
            this.toolStripDropDownButtonColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonColor.Name = "toolStripDropDownButtonColor";
            this.toolStripDropDownButtonColor.Size = new System.Drawing.Size(49, 22);
            this.toolStripDropDownButtonColor.Text = "Color";
            this.toolStripDropDownButtonColor.ToolTipText = "Setting the color for the symbols";
            // 
            // toolStripLabelSize
            // 
            this.toolStripLabelSize.Name = "toolStripLabelSize";
            this.toolStripLabelSize.Size = new System.Drawing.Size(36, 22);
            this.toolStripLabelSize.Text = "Size x";
            // 
            // toolStripTextBoxSize
            // 
            this.toolStripTextBoxSize.Name = "toolStripTextBoxSize";
            this.toolStripTextBoxSize.Size = new System.Drawing.Size(30, 25);
            this.toolStripTextBoxSize.Text = "1.0";
            this.toolStripTextBoxSize.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripTextBoxSize.ToolTipText = "The factor for the scaling of the symbols";
            this.toolStripTextBoxSize.Leave += new System.EventHandler(this.toolStripTextBoxSize_Leave);
            // 
            // toolStripButtonLineThickness
            // 
            this.toolStripButtonLineThickness.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLineThickness.Image = global::DiversityCollection.Resource.LineThickness;
            this.toolStripButtonLineThickness.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLineThickness.Name = "toolStripButtonLineThickness";
            this.toolStripButtonLineThickness.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLineThickness.Text = "toolStripButton1";
            // 
            // toolStripTextBoxThickness
            // 
            this.toolStripTextBoxThickness.Name = "toolStripTextBoxThickness";
            this.toolStripTextBoxThickness.Size = new System.Drawing.Size(30, 25);
            this.toolStripTextBoxThickness.Text = "1.0";
            this.toolStripTextBoxThickness.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolStripTextBoxThickness.ToolTipText = "The thickness of the lines in the symbols";
            this.toolStripTextBoxThickness.Leave += new System.EventHandler(this.toolStripTextBoxThickness_Leave);
            // 
            // toolStripButtonAddToDistribution
            // 
            this.toolStripButtonAddToDistribution.Image = global::DiversityCollection.Resource.Add;
            this.toolStripButtonAddToDistribution.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddToDistribution.Name = "toolStripButtonAddToDistribution";
            this.toolStripButtonAddToDistribution.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonAddToDistribution.Text = "Add";
            this.toolStripButtonAddToDistribution.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButtonAddToDistribution.ToolTipText = "Add selected items to distribution map";
            this.toolStripButtonAddToDistribution.Click += new System.EventHandler(this.toolStripButtonAddToDistribution_Click);
            // 
            // toolStripButtonRequeryDistribution
            // 
            this.toolStripButtonRequeryDistribution.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRequeryDistribution.Image = global::DiversityCollection.Resource.Update;
            this.toolStripButtonRequeryDistribution.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRequeryDistribution.Name = "toolStripButtonRequeryDistribution";
            this.toolStripButtonRequeryDistribution.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRequeryDistribution.Text = "Start a new distribution";
            this.toolStripButtonRequeryDistribution.Visible = false;
            this.toolStripButtonRequeryDistribution.Click += new System.EventHandler(this.toolStripButtonRequeryDistribution_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "toolStripButton1";
            this.toolStripButtonSave.ToolTipText = "Save the changes of the geographical objects";
            this.toolStripButtonSave.Visible = false;
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // numericUpDownZoomLevel
            // 
            this.numericUpDownZoomLevel.Dock = System.Windows.Forms.DockStyle.Left;
            this.numericUpDownZoomLevel.Location = new System.Drawing.Point(653, 3);
            this.numericUpDownZoomLevel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.numericUpDownZoomLevel.Maximum = new decimal(new int[] {
            19,
            0,
            0,
            0});
            this.numericUpDownZoomLevel.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownZoomLevel.Name = "numericUpDownZoomLevel";
            this.numericUpDownZoomLevel.Size = new System.Drawing.Size(37, 20);
            this.numericUpDownZoomLevel.TabIndex = 3;
            this.numericUpDownZoomLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numericUpDownZoomLevel, "Set the zoom level within the maps. Low for an overview and high (max. 19) for de" +
        "tails");
            this.numericUpDownZoomLevel.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownZoomLevel.ValueChanged += new System.EventHandler(this.numericUpDownZoomLevel_ValueChanged);
            // 
            // splitContainerMaps
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.splitContainerMaps, 8);
            this.splitContainerMaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMaps.Location = new System.Drawing.Point(3, 30);
            this.splitContainerMaps.Name = "splitContainerMaps";
            // 
            // splitContainerMaps.Panel1
            // 
            this.splitContainerMaps.Panel1.Controls.Add(this.panelWebbrowser);
            // 
            // splitContainerMaps.Panel2
            // 
            this.splitContainerMaps.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainerMaps.Panel2.Controls.Add(this.panelForWpfControl);
            this.splitContainerMaps.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainerMaps.Panel2Collapsed = true;
            this.splitContainerMaps.Size = new System.Drawing.Size(807, 465);
            this.splitContainerMaps.SplitterDistance = 269;
            this.splitContainerMaps.TabIndex = 5;
            // 
            // panelWebbrowser
            // 
            this.panelWebbrowser.Controls.Add(this.userControlWebView);
            this.panelWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWebbrowser.Location = new System.Drawing.Point(0, 0);
            this.panelWebbrowser.Name = "panelWebbrowser";
            this.panelWebbrowser.Size = new System.Drawing.Size(807, 465);
            this.panelWebbrowser.TabIndex = 0;
            // 
            // userControlWebView
            // 
            this.userControlWebView.AllowScripting = false;
            this.userControlWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebView.Location = new System.Drawing.Point(0, 0);
            this.userControlWebView.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebView.Name = "userControlWebView";
            this.userControlWebView.ScriptErrorsSuppressed = false;
            this.userControlWebView.Size = new System.Drawing.Size(807, 465);
            this.userControlWebView.TabIndex = 1;
            this.userControlWebView.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // panelForWpfControl
            // 
            this.panelForWpfControl.Controls.Add(this.elementHost);
            this.panelForWpfControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForWpfControl.Location = new System.Drawing.Point(3, 3);
            this.panelForWpfControl.Name = "panelForWpfControl";
            this.panelForWpfControl.Size = new System.Drawing.Size(90, 94);
            this.panelForWpfControl.TabIndex = 2;
            // 
            // elementHost
            // 
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 0);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(90, 94);
            this.elementHost.TabIndex = 1;
            this.elementHost.Text = "elementHost";
            this.elementHost.Child = this.wpfControl;
            // 
            // buttonReload
            // 
            this.buttonReload.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonReload.FlatAppearance.BorderSize = 0;
            this.buttonReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReload.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonReload.Location = new System.Drawing.Point(696, 0);
            this.buttonReload.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(18, 24);
            this.buttonReload.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonReload, "Reload the map with the current settings");
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // checkBoxZoomLevel
            // 
            this.checkBoxZoomLevel.AutoSize = true;
            this.checkBoxZoomLevel.Checked = true;
            this.checkBoxZoomLevel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxZoomLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxZoomLevel.Location = new System.Drawing.Point(552, 3);
            this.checkBoxZoomLevel.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.checkBoxZoomLevel.Name = "checkBoxZoomLevel";
            this.checkBoxZoomLevel.Size = new System.Drawing.Size(101, 21);
            this.checkBoxZoomLevel.TabIndex = 7;
            this.checkBoxZoomLevel.Text = "Use zoom level:";
            this.checkBoxZoomLevel.UseVisualStyleBackColor = true;
            // 
            // buttonMapIsFixed
            // 
            this.buttonMapIsFixed.BackColor = System.Drawing.SystemColors.Control;
            this.buttonMapIsFixed.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonMapIsFixed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMapIsFixed.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonMapIsFixed.Image = global::DiversityCollection.Resource.HoldNot;
            this.buttonMapIsFixed.Location = new System.Drawing.Point(525, 0);
            this.buttonMapIsFixed.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.buttonMapIsFixed.Name = "buttonMapIsFixed";
            this.buttonMapIsFixed.Size = new System.Drawing.Size(24, 24);
            this.buttonMapIsFixed.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonMapIsFixed, "Map is NOT fixed");
            this.buttonMapIsFixed.UseVisualStyleBackColor = false;
            this.buttonMapIsFixed.Click += new System.EventHandler(this.buttonMapIsFixed_Click);
            // 
            // buttonCustomizeMapLocalisations
            // 
            this.buttonCustomizeMapLocalisations.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCustomizeMapLocalisations.FlatAppearance.BorderSize = 0;
            this.buttonCustomizeMapLocalisations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCustomizeMapLocalisations.Image = global::DiversityCollection.Resource.Settings;
            this.buttonCustomizeMapLocalisations.Location = new System.Drawing.Point(790, 0);
            this.buttonCustomizeMapLocalisations.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.buttonCustomizeMapLocalisations.Name = "buttonCustomizeMapLocalisations";
            this.buttonCustomizeMapLocalisations.Size = new System.Drawing.Size(20, 24);
            this.buttonCustomizeMapLocalisations.TabIndex = 9;
            this.toolTip.SetToolTip(this.buttonCustomizeMapLocalisations, "Set the localisation systems that should be displayed in the maps");
            this.buttonCustomizeMapLocalisations.UseVisualStyleBackColor = true;
            this.buttonCustomizeMapLocalisations.Visible = false;
            this.buttonCustomizeMapLocalisations.Click += new System.EventHandler(this.buttonCustomizeMapLocalisations_Click);
            // 
            // checkBoxIncludeAll
            // 
            this.checkBoxIncludeAll.Checked = true;
            this.checkBoxIncludeAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxIncludeAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxIncludeAll.Image = global::DiversityCollection.Resource.MarkColumn;
            this.checkBoxIncludeAll.Location = new System.Drawing.Point(717, 3);
            this.checkBoxIncludeAll.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.checkBoxIncludeAll.Name = "checkBoxIncludeAll";
            this.checkBoxIncludeAll.Size = new System.Drawing.Size(35, 24);
            this.checkBoxIncludeAll.TabIndex = 11;
            this.toolTip.SetToolTip(this.checkBoxIncludeAll, "Include all specimens from the query ");
            this.checkBoxIncludeAll.UseVisualStyleBackColor = true;
            // 
            // imageListSymbol
            // 
            this.imageListSymbol.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSymbol.ImageStream")));
            this.imageListSymbol.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSymbol.Images.SetKeyName(0, "Circle.ico");
            this.imageListSymbol.Images.SetKeyName(1, "CircleFilled.ico");
            this.imageListSymbol.Images.SetKeyName(2, "Cross.ico");
            this.imageListSymbol.Images.SetKeyName(3, "Diamond.ico");
            this.imageListSymbol.Images.SetKeyName(4, "DiamondFilled.ico");
            this.imageListSymbol.Images.SetKeyName(5, "Pin.ico");
            this.imageListSymbol.Images.SetKeyName(6, "Square.ico");
            this.imageListSymbol.Images.SetKeyName(7, "SquareFilled.ico");
            this.imageListSymbol.Images.SetKeyName(8, "X.ico");
            // 
            // imageListHold
            // 
            this.imageListHold.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListHold.ImageStream")));
            this.imageListHold.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListHold.Images.SetKeyName(0, "Hold.ico");
            this.imageListHold.Images.SetKeyName(1, "HoldNot.ico");
            // 
            // UserControlGIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlGIS";
            this.Size = new System.Drawing.Size(813, 498);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoomLevel)).EndInit();
            this.splitContainerMaps.Panel1.ResumeLayout(false);
            this.splitContainerMaps.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaps)).EndInit();
            this.splitContainerMaps.ResumeLayout(false);
            this.panelWebbrowser.ResumeLayout(false);
            this.panelForWpfControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.Integration.ElementHost elementHost;
        private WpfSamplingPlotPage.WpfControl wpfControl;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonState;
        private System.Windows.Forms.Panel panelForWpfControl;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStateOrganism;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStateEvent;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStateSeries;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStateDistribution;
        private System.Windows.Forms.NumericUpDown numericUpDownZoomLevel;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.SplitContainer splitContainerMaps;
        private System.Windows.Forms.Panel panelWebbrowser;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonEditMode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditModeBrowser;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditModeGisNoEdit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEditModeGIS;
        private System.Windows.Forms.ToolStripButton toolStripButtonRequeryDistribution;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddToDistribution;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonColor;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSymbol;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxThickness;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSize;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSize;
        private System.Windows.Forms.ImageList imageListSymbol;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSymbol;
        private System.Windows.Forms.ToolStripButton toolStripButtonLineThickness;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox checkBoxZoomLevel;
        private System.Windows.Forms.ToolStripMenuItem anyCoordinatesToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListHold;
        private System.Windows.Forms.Button buttonMapIsFixed;
        private System.Windows.Forms.Button buttonCustomizeMapLocalisations;
        private System.Windows.Forms.ToolStripMenuItem distInclOrgToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBoxIncludeAll;
        private DiversityWorkbench.UserControls.UserControlWebView userControlWebView;
    }
}
