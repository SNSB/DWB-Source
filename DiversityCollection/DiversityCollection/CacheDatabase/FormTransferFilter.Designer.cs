namespace DiversityCollection.CacheDatabase
{
    partial class FormTransferFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTransferFilter));
            tabControlMain = new System.Windows.Forms.TabControl();
            tabPageSpecimen = new System.Windows.Forms.TabPage();
            userControlQueryListSpecimen = new DiversityWorkbench.UserControls.UserControlQueryList();
            tabPageLocalisation = new System.Windows.Forms.TabPage();
            tableLayoutPanelProjectLocalisation = new System.Windows.Forms.TableLayoutPanel();
            labelCoordinatePrecision = new System.Windows.Forms.Label();
            numericUpDownCoordinatePrecision = new System.Windows.Forms.NumericUpDown();
            checkBoxCoordinatePrecision = new System.Windows.Forms.CheckBox();
            labelProjectLocalisationNotPublished = new System.Windows.Forms.Label();
            listBoxProjectLocalisationNotPublished = new System.Windows.Forms.ListBox();
            labelProjectLocalisationPublished = new System.Windows.Forms.Label();
            listBoxProjectLocalisationPubished = new System.Windows.Forms.ListBox();
            buttonProjectLocalisationPublish = new System.Windows.Forms.Button();
            buttonProjectLocalisationHide = new System.Windows.Forms.Button();
            buttonProjectLocalisationUp = new System.Windows.Forms.Button();
            buttonProjectLocalisationDown = new System.Windows.Forms.Button();
            labelProjectLocalisationHeader = new System.Windows.Forms.Label();
            tabPageSiteProperties = new System.Windows.Forms.TabPage();
            tableLayoutPanelEventProperty = new System.Windows.Forms.TableLayoutPanel();
            labelEventPropertyHeader = new System.Windows.Forms.Label();
            labelEventPropertyNotPublished = new System.Windows.Forms.Label();
            labelEventPropertyPublished = new System.Windows.Forms.Label();
            listBoxEventPropertyNotPublished = new System.Windows.Forms.ListBox();
            listBoxEventPropertyPublished = new System.Windows.Forms.ListBox();
            buttonEventPropertyAdd = new System.Windows.Forms.Button();
            buttonEventPropertyRemove = new System.Windows.Forms.Button();
            buttonEventPropertyTransferExisiting = new System.Windows.Forms.Button();
            tabPageTaxonomicGroups = new System.Windows.Forms.TabPage();
            tableLayoutPanelTaxonomicGroups = new System.Windows.Forms.TableLayoutPanel();
            labelProjectTaxonomicGroup = new System.Windows.Forms.Label();
            labelProjectTaxonomicGroupNotPublished = new System.Windows.Forms.Label();
            labelProjectTaxonomicGroupPublished = new System.Windows.Forms.Label();
            listBoxProjectTaxonomicGroupNotPublished = new System.Windows.Forms.ListBox();
            listBoxProjectTaxonomicGroupPublished = new System.Windows.Forms.ListBox();
            buttonProjectTaxonomicGroupPublished = new System.Windows.Forms.Button();
            buttonProjectTaxonomicGroupNotPublished = new System.Windows.Forms.Button();
            buttonProjectTaxonomicGroupTransferExisting = new System.Windows.Forms.Button();
            checkBoxProjectTaxonomicGroupPublishedRestricted = new System.Windows.Forms.CheckBox();
            tabPageMaterialCategories = new System.Windows.Forms.TabPage();
            tableLayoutPanelProjectMaterialCategory = new System.Windows.Forms.TableLayoutPanel();
            labelProjectMaterialCategoryHeader = new System.Windows.Forms.Label();
            labelProjectMaterialCategoryNotPublished = new System.Windows.Forms.Label();
            labelProjectMaterialCategoryPublished = new System.Windows.Forms.Label();
            listBoxProjectMaterialCategoryNotPublished = new System.Windows.Forms.ListBox();
            listBoxProjectMaterialCategoryPublished = new System.Windows.Forms.ListBox();
            buttonProjectMaterialCategoryPublishe = new System.Windows.Forms.Button();
            buttonProjectMaterialCategoryWithhold = new System.Windows.Forms.Button();
            buttonProjectMaterialCategoryTransferExisting = new System.Windows.Forms.Button();
            tabPageAnalysis = new System.Windows.Forms.TabPage();
            tableLayoutPanelAnalysis = new System.Windows.Forms.TableLayoutPanel();
            labelAnalysisHeader = new System.Windows.Forms.Label();
            labelAnalysisNotPublished = new System.Windows.Forms.Label();
            labelAnalysisPublished = new System.Windows.Forms.Label();
            listBoxAnalysisNotPublished = new System.Windows.Forms.ListBox();
            listBoxAnalysisPublished = new System.Windows.Forms.ListBox();
            buttonAnalysisPublish = new System.Windows.Forms.Button();
            buttonAnalysisBlock = new System.Windows.Forms.Button();
            buttonAnalysisTransferExisting = new System.Windows.Forms.Button();
            imageListTabcontrol = new System.Windows.Forms.ImageList(components);
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            buttonFeedback = new System.Windows.Forms.Button();
            tabControlMain.SuspendLayout();
            tabPageSpecimen.SuspendLayout();
            tabPageLocalisation.SuspendLayout();
            tableLayoutPanelProjectLocalisation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCoordinatePrecision).BeginInit();
            tabPageSiteProperties.SuspendLayout();
            tableLayoutPanelEventProperty.SuspendLayout();
            tabPageTaxonomicGroups.SuspendLayout();
            tableLayoutPanelTaxonomicGroups.SuspendLayout();
            tabPageMaterialCategories.SuspendLayout();
            tableLayoutPanelProjectMaterialCategory.SuspendLayout();
            tabPageAnalysis.SuspendLayout();
            tableLayoutPanelAnalysis.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageSpecimen);
            tabControlMain.Controls.Add(tabPageLocalisation);
            tabControlMain.Controls.Add(tabPageSiteProperties);
            tabControlMain.Controls.Add(tabPageTaxonomicGroups);
            tabControlMain.Controls.Add(tabPageMaterialCategories);
            tabControlMain.Controls.Add(tabPageAnalysis);
            tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tabControlMain, "cachedatabase_restrictions_dc");
            tabControlMain.ImageList = imageListTabcontrol;
            tabControlMain.Location = new System.Drawing.Point(0, 0);
            tabControlMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            helpProvider.SetShowHelp(tabControlMain, true);
            tabControlMain.Size = new System.Drawing.Size(768, 487);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageSpecimen
            // 
            tabPageSpecimen.Controls.Add(userControlQueryListSpecimen);
            helpProvider.SetHelpKeyword(tabPageSpecimen, "cachedatabase_restrictions_dc#specimen");
            tabPageSpecimen.ImageIndex = 4;
            tabPageSpecimen.Location = new System.Drawing.Point(4, 24);
            tabPageSpecimen.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageSpecimen.Name = "tabPageSpecimen";
            helpProvider.SetShowHelp(tabPageSpecimen, true);
            tabPageSpecimen.Size = new System.Drawing.Size(760, 459);
            tabPageSpecimen.TabIndex = 4;
            tabPageSpecimen.Text = "Specimen";
            tabPageSpecimen.UseVisualStyleBackColor = true;
            // 
            // userControlQueryListSpecimen
            // 
            userControlQueryListSpecimen.BacklinkUpdateEnabled = false;
            userControlQueryListSpecimen.Connection = null;
            userControlQueryListSpecimen.DisplayTextSelectedItem = "";
            userControlQueryListSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlQueryListSpecimen.IDisNumeric = true;
            userControlQueryListSpecimen.ImageList = null;
            userControlQueryListSpecimen.IsPredefinedQuery = false;
            userControlQueryListSpecimen.Location = new System.Drawing.Point(0, 0);
            userControlQueryListSpecimen.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlQueryListSpecimen.MaximalNumberOfResults = 100;
            userControlQueryListSpecimen.Name = "userControlQueryListSpecimen";
            userControlQueryListSpecimen.Optimizing_UsedForQueryList = false;
            userControlQueryListSpecimen.ProjectID = -1;
            userControlQueryListSpecimen.QueryConditionVisiblity = "";
            userControlQueryListSpecimen.QueryDisplayColumns = null;
            userControlQueryListSpecimen.QueryMainTableLocal = null;
            userControlQueryListSpecimen.QueryRestriction = "";
            userControlQueryListSpecimen.RememberQuerySettingsIdentifier = "QueryList";
            userControlQueryListSpecimen.SelectedProjectID = null;
            userControlQueryListSpecimen.Size = new System.Drawing.Size(760, 459);
            userControlQueryListSpecimen.TabIndex = 0;
            userControlQueryListSpecimen.TableColors = null;
            userControlQueryListSpecimen.TableImageIndex = null;
            userControlQueryListSpecimen.WhereClause = null;
            // 
            // tabPageLocalisation
            // 
            tabPageLocalisation.Controls.Add(tableLayoutPanelProjectLocalisation);
            tabPageLocalisation.ImageIndex = 0;
            tabPageLocalisation.Location = new System.Drawing.Point(4, 24);
            tabPageLocalisation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageLocalisation.Name = "tabPageLocalisation";
            tabPageLocalisation.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageLocalisation.Size = new System.Drawing.Size(760, 459);
            tabPageLocalisation.TabIndex = 0;
            tabPageLocalisation.Text = "Localisation";
            tabPageLocalisation.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProjectLocalisation
            // 
            tableLayoutPanelProjectLocalisation.ColumnCount = 5;
            tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanelProjectLocalisation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanelProjectLocalisation.Controls.Add(labelCoordinatePrecision, 3, 0);
            tableLayoutPanelProjectLocalisation.Controls.Add(numericUpDownCoordinatePrecision, 1, 0);
            tableLayoutPanelProjectLocalisation.Controls.Add(checkBoxCoordinatePrecision, 0, 0);
            tableLayoutPanelProjectLocalisation.Controls.Add(labelProjectLocalisationNotPublished, 0, 3);
            tableLayoutPanelProjectLocalisation.Controls.Add(listBoxProjectLocalisationNotPublished, 0, 4);
            tableLayoutPanelProjectLocalisation.Controls.Add(labelProjectLocalisationPublished, 3, 3);
            tableLayoutPanelProjectLocalisation.Controls.Add(listBoxProjectLocalisationPubished, 3, 4);
            tableLayoutPanelProjectLocalisation.Controls.Add(buttonProjectLocalisationPublish, 2, 4);
            tableLayoutPanelProjectLocalisation.Controls.Add(buttonProjectLocalisationHide, 2, 5);
            tableLayoutPanelProjectLocalisation.Controls.Add(buttonProjectLocalisationUp, 3, 6);
            tableLayoutPanelProjectLocalisation.Controls.Add(buttonProjectLocalisationDown, 4, 6);
            tableLayoutPanelProjectLocalisation.Controls.Add(labelProjectLocalisationHeader, 0, 2);
            tableLayoutPanelProjectLocalisation.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelProjectLocalisation.Location = new System.Drawing.Point(4, 3);
            tableLayoutPanelProjectLocalisation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelProjectLocalisation.Name = "tableLayoutPanelProjectLocalisation";
            tableLayoutPanelProjectLocalisation.RowCount = 7;
            tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectLocalisation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelProjectLocalisation.Size = new System.Drawing.Size(752, 453);
            tableLayoutPanelProjectLocalisation.TabIndex = 2;
            // 
            // labelCoordinatePrecision
            // 
            labelCoordinatePrecision.AutoSize = true;
            labelCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Left;
            labelCoordinatePrecision.Location = new System.Drawing.Point(391, 0);
            labelCoordinatePrecision.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            labelCoordinatePrecision.Name = "labelCoordinatePrecision";
            labelCoordinatePrecision.Size = new System.Drawing.Size(85, 29);
            labelCoordinatePrecision.TabIndex = 15;
            labelCoordinatePrecision.Text = "decimal places";
            labelCoordinatePrecision.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownCoordinatePrecision
            // 
            tableLayoutPanelProjectLocalisation.SetColumnSpan(numericUpDownCoordinatePrecision, 2);
            numericUpDownCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Left;
            numericUpDownCoordinatePrecision.Location = new System.Drawing.Point(361, 3);
            numericUpDownCoordinatePrecision.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            numericUpDownCoordinatePrecision.Name = "numericUpDownCoordinatePrecision";
            numericUpDownCoordinatePrecision.Size = new System.Drawing.Size(30, 23);
            numericUpDownCoordinatePrecision.TabIndex = 14;
            toolTip.SetToolTip(numericUpDownCoordinatePrecision, "The decimal places to which the coordinate prescision should be restricted");
            numericUpDownCoordinatePrecision.Click += numericUpDownCoordinatePrecision_Click;
            // 
            // checkBoxCoordinatePrecision
            // 
            checkBoxCoordinatePrecision.AutoSize = true;
            checkBoxCoordinatePrecision.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxCoordinatePrecision.Location = new System.Drawing.Point(168, 3);
            checkBoxCoordinatePrecision.Margin = new System.Windows.Forms.Padding(4, 3, 0, 3);
            checkBoxCoordinatePrecision.Name = "checkBoxCoordinatePrecision";
            checkBoxCoordinatePrecision.Size = new System.Drawing.Size(193, 23);
            checkBoxCoordinatePrecision.TabIndex = 9;
            checkBoxCoordinatePrecision.Text = "Restrict coordinate precision to:";
            checkBoxCoordinatePrecision.UseVisualStyleBackColor = true;
            checkBoxCoordinatePrecision.Click += checkBoxCoordinatePrecision_Click;
            // 
            // labelProjectLocalisationNotPublished
            // 
            labelProjectLocalisationNotPublished.AutoSize = true;
            tableLayoutPanelProjectLocalisation.SetColumnSpan(labelProjectLocalisationNotPublished, 2);
            labelProjectLocalisationNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectLocalisationNotPublished.Location = new System.Drawing.Point(4, 62);
            labelProjectLocalisationNotPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelProjectLocalisationNotPublished.Name = "labelProjectLocalisationNotPublished";
            labelProjectLocalisationNotPublished.Size = new System.Drawing.Size(360, 15);
            labelProjectLocalisationNotPublished.TabIndex = 0;
            labelProjectLocalisationNotPublished.Text = "Not published";
            // 
            // listBoxProjectLocalisationNotPublished
            // 
            listBoxProjectLocalisationNotPublished.BackColor = System.Drawing.Color.Pink;
            tableLayoutPanelProjectLocalisation.SetColumnSpan(listBoxProjectLocalisationNotPublished, 2);
            listBoxProjectLocalisationNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectLocalisationNotPublished.FormattingEnabled = true;
            listBoxProjectLocalisationNotPublished.ItemHeight = 15;
            listBoxProjectLocalisationNotPublished.Location = new System.Drawing.Point(4, 83);
            listBoxProjectLocalisationNotPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxProjectLocalisationNotPublished.Name = "listBoxProjectLocalisationNotPublished";
            tableLayoutPanelProjectLocalisation.SetRowSpan(listBoxProjectLocalisationNotPublished, 3);
            listBoxProjectLocalisationNotPublished.Size = new System.Drawing.Size(360, 367);
            listBoxProjectLocalisationNotPublished.TabIndex = 2;
            // 
            // labelProjectLocalisationPublished
            // 
            labelProjectLocalisationPublished.AutoSize = true;
            labelProjectLocalisationPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectLocalisationPublished.Location = new System.Drawing.Point(395, 62);
            labelProjectLocalisationPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelProjectLocalisationPublished.Name = "labelProjectLocalisationPublished";
            labelProjectLocalisationPublished.Size = new System.Drawing.Size(172, 15);
            labelProjectLocalisationPublished.TabIndex = 1;
            labelProjectLocalisationPublished.Text = "Published";
            // 
            // listBoxProjectLocalisationPubished
            // 
            listBoxProjectLocalisationPubished.BackColor = System.Drawing.Color.LightGreen;
            tableLayoutPanelProjectLocalisation.SetColumnSpan(listBoxProjectLocalisationPubished, 2);
            listBoxProjectLocalisationPubished.DisplayMember = "DisplayText";
            listBoxProjectLocalisationPubished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectLocalisationPubished.FormattingEnabled = true;
            listBoxProjectLocalisationPubished.ItemHeight = 15;
            listBoxProjectLocalisationPubished.Location = new System.Drawing.Point(395, 83);
            listBoxProjectLocalisationPubished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            listBoxProjectLocalisationPubished.Name = "listBoxProjectLocalisationPubished";
            tableLayoutPanelProjectLocalisation.SetRowSpan(listBoxProjectLocalisationPubished, 2);
            listBoxProjectLocalisationPubished.Size = new System.Drawing.Size(353, 347);
            listBoxProjectLocalisationPubished.TabIndex = 3;
            listBoxProjectLocalisationPubished.ValueMember = "LocalisationSystemID";
            // 
            // buttonProjectLocalisationPublish
            // 
            buttonProjectLocalisationPublish.BackColor = System.Drawing.Color.LightGreen;
            buttonProjectLocalisationPublish.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonProjectLocalisationPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectLocalisationPublish.ForeColor = System.Drawing.Color.Green;
            buttonProjectLocalisationPublish.Location = new System.Drawing.Point(368, 228);
            buttonProjectLocalisationPublish.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectLocalisationPublish.Name = "buttonProjectLocalisationPublish";
            buttonProjectLocalisationPublish.Size = new System.Drawing.Size(23, 27);
            buttonProjectLocalisationPublish.TabIndex = 4;
            buttonProjectLocalisationPublish.Text = ">";
            toolTip.SetToolTip(buttonProjectLocalisationPublish, "Publish selected localisation");
            buttonProjectLocalisationPublish.UseVisualStyleBackColor = false;
            buttonProjectLocalisationPublish.Click += buttonLocalisationPublished_Click;
            // 
            // buttonProjectLocalisationHide
            // 
            buttonProjectLocalisationHide.BackColor = System.Drawing.Color.Pink;
            buttonProjectLocalisationHide.Dock = System.Windows.Forms.DockStyle.Top;
            buttonProjectLocalisationHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectLocalisationHide.ForeColor = System.Drawing.Color.Red;
            buttonProjectLocalisationHide.Location = new System.Drawing.Point(368, 255);
            buttonProjectLocalisationHide.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectLocalisationHide.Name = "buttonProjectLocalisationHide";
            buttonProjectLocalisationHide.Size = new System.Drawing.Size(23, 27);
            buttonProjectLocalisationHide.TabIndex = 5;
            buttonProjectLocalisationHide.Text = "<";
            toolTip.SetToolTip(buttonProjectLocalisationHide, "Hide selected localisation");
            buttonProjectLocalisationHide.UseVisualStyleBackColor = false;
            buttonProjectLocalisationHide.Click += buttonLocalisationNotPublished_Click;
            // 
            // buttonProjectLocalisationUp
            // 
            buttonProjectLocalisationUp.Dock = System.Windows.Forms.DockStyle.Right;
            buttonProjectLocalisationUp.Image = Resource.ArrowUp;
            buttonProjectLocalisationUp.Location = new System.Drawing.Point(539, 430);
            buttonProjectLocalisationUp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            buttonProjectLocalisationUp.Name = "buttonProjectLocalisationUp";
            buttonProjectLocalisationUp.Size = new System.Drawing.Size(28, 20);
            buttonProjectLocalisationUp.TabIndex = 6;
            toolTip.SetToolTip(buttonProjectLocalisationUp, "Move selected locasation to the top of the list");
            buttonProjectLocalisationUp.UseVisualStyleBackColor = true;
            buttonProjectLocalisationUp.Click += buttonLocalisationPublishedUp_Click;
            // 
            // buttonProjectLocalisationDown
            // 
            buttonProjectLocalisationDown.Dock = System.Windows.Forms.DockStyle.Left;
            buttonProjectLocalisationDown.Image = Resource.ArrowDown;
            buttonProjectLocalisationDown.Location = new System.Drawing.Point(575, 430);
            buttonProjectLocalisationDown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 3);
            buttonProjectLocalisationDown.Name = "buttonProjectLocalisationDown";
            buttonProjectLocalisationDown.Size = new System.Drawing.Size(28, 20);
            buttonProjectLocalisationDown.TabIndex = 7;
            toolTip.SetToolTip(buttonProjectLocalisationDown, "Move selected localisation to the end of the list");
            buttonProjectLocalisationDown.UseVisualStyleBackColor = true;
            buttonProjectLocalisationDown.Click += buttonLocalisationPublishedDown_Click;
            // 
            // labelProjectLocalisationHeader
            // 
            labelProjectLocalisationHeader.AutoSize = true;
            tableLayoutPanelProjectLocalisation.SetColumnSpan(labelProjectLocalisationHeader, 5);
            labelProjectLocalisationHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectLocalisationHeader.Location = new System.Drawing.Point(4, 36);
            labelProjectLocalisationHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectLocalisationHeader.Name = "labelProjectLocalisationHeader";
            labelProjectLocalisationHeader.Size = new System.Drawing.Size(744, 23);
            labelProjectLocalisationHeader.TabIndex = 8;
            labelProjectLocalisationHeader.Text = "The localisation systems used to retrieve the coordinates and their sequence of retrieval";
            labelProjectLocalisationHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPageSiteProperties
            // 
            tabPageSiteProperties.Controls.Add(tableLayoutPanelEventProperty);
            tabPageSiteProperties.ImageIndex = 5;
            tabPageSiteProperties.Location = new System.Drawing.Point(4, 24);
            tabPageSiteProperties.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageSiteProperties.Name = "tabPageSiteProperties";
            tabPageSiteProperties.Size = new System.Drawing.Size(760, 459);
            tabPageSiteProperties.TabIndex = 5;
            tabPageSiteProperties.Text = "Site properties";
            tabPageSiteProperties.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEventProperty
            // 
            tableLayoutPanelEventProperty.ColumnCount = 4;
            tableLayoutPanelEventProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelEventProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelEventProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelEventProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelEventProperty.Controls.Add(labelEventPropertyHeader, 0, 0);
            tableLayoutPanelEventProperty.Controls.Add(labelEventPropertyNotPublished, 0, 1);
            tableLayoutPanelEventProperty.Controls.Add(labelEventPropertyPublished, 3, 1);
            tableLayoutPanelEventProperty.Controls.Add(listBoxEventPropertyNotPublished, 0, 2);
            tableLayoutPanelEventProperty.Controls.Add(listBoxEventPropertyPublished, 3, 2);
            tableLayoutPanelEventProperty.Controls.Add(buttonEventPropertyAdd, 2, 2);
            tableLayoutPanelEventProperty.Controls.Add(buttonEventPropertyRemove, 2, 3);
            tableLayoutPanelEventProperty.Controls.Add(buttonEventPropertyTransferExisiting, 2, 0);
            tableLayoutPanelEventProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelEventProperty.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelEventProperty.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelEventProperty.Name = "tableLayoutPanelEventProperty";
            tableLayoutPanelEventProperty.RowCount = 4;
            tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelEventProperty.Size = new System.Drawing.Size(760, 459);
            tableLayoutPanelEventProperty.TabIndex = 6;
            // 
            // labelEventPropertyHeader
            // 
            labelEventPropertyHeader.AutoSize = true;
            tableLayoutPanelEventProperty.SetColumnSpan(labelEventPropertyHeader, 2);
            labelEventPropertyHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelEventPropertyHeader.Location = new System.Drawing.Point(4, 3);
            labelEventPropertyHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelEventPropertyHeader.Name = "labelEventPropertyHeader";
            labelEventPropertyHeader.Size = new System.Drawing.Size(429, 27);
            labelEventPropertyHeader.TabIndex = 1;
            labelEventPropertyHeader.Text = "Site properties transfered into the cache database";
            labelEventPropertyHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelEventPropertyNotPublished
            // 
            labelEventPropertyNotPublished.AutoSize = true;
            tableLayoutPanelEventProperty.SetColumnSpan(labelEventPropertyNotPublished, 2);
            labelEventPropertyNotPublished.Location = new System.Drawing.Point(4, 33);
            labelEventPropertyNotPublished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelEventPropertyNotPublished.Name = "labelEventPropertyNotPublished";
            labelEventPropertyNotPublished.Size = new System.Drawing.Size(82, 15);
            labelEventPropertyNotPublished.TabIndex = 5;
            labelEventPropertyNotPublished.Text = "Not published";
            // 
            // labelEventPropertyPublished
            // 
            labelEventPropertyPublished.AutoSize = true;
            labelEventPropertyPublished.Location = new System.Drawing.Point(464, 33);
            labelEventPropertyPublished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelEventPropertyPublished.Name = "labelEventPropertyPublished";
            labelEventPropertyPublished.Size = new System.Drawing.Size(59, 15);
            labelEventPropertyPublished.TabIndex = 6;
            labelEventPropertyPublished.Text = "Published";
            // 
            // listBoxEventPropertyNotPublished
            // 
            listBoxEventPropertyNotPublished.BackColor = System.Drawing.Color.Pink;
            tableLayoutPanelEventProperty.SetColumnSpan(listBoxEventPropertyNotPublished, 2);
            listBoxEventPropertyNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxEventPropertyNotPublished.FormattingEnabled = true;
            listBoxEventPropertyNotPublished.ItemHeight = 15;
            listBoxEventPropertyNotPublished.Location = new System.Drawing.Point(4, 51);
            listBoxEventPropertyNotPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxEventPropertyNotPublished.Name = "listBoxEventPropertyNotPublished";
            tableLayoutPanelEventProperty.SetRowSpan(listBoxEventPropertyNotPublished, 2);
            listBoxEventPropertyNotPublished.Size = new System.Drawing.Size(429, 405);
            listBoxEventPropertyNotPublished.Sorted = true;
            listBoxEventPropertyNotPublished.TabIndex = 7;
            // 
            // listBoxEventPropertyPublished
            // 
            listBoxEventPropertyPublished.BackColor = System.Drawing.Color.LightGreen;
            listBoxEventPropertyPublished.DisplayMember = "TaxonomicGroup";
            listBoxEventPropertyPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxEventPropertyPublished.FormattingEnabled = true;
            listBoxEventPropertyPublished.ItemHeight = 15;
            listBoxEventPropertyPublished.Location = new System.Drawing.Point(464, 51);
            listBoxEventPropertyPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxEventPropertyPublished.Name = "listBoxEventPropertyPublished";
            tableLayoutPanelEventProperty.SetRowSpan(listBoxEventPropertyPublished, 2);
            listBoxEventPropertyPublished.Size = new System.Drawing.Size(292, 405);
            listBoxEventPropertyPublished.Sorted = true;
            listBoxEventPropertyPublished.TabIndex = 8;
            // 
            // buttonEventPropertyAdd
            // 
            buttonEventPropertyAdd.BackColor = System.Drawing.Color.LightGreen;
            buttonEventPropertyAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonEventPropertyAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonEventPropertyAdd.ForeColor = System.Drawing.Color.Green;
            buttonEventPropertyAdd.Location = new System.Drawing.Point(437, 226);
            buttonEventPropertyAdd.Margin = new System.Windows.Forms.Padding(0);
            buttonEventPropertyAdd.Name = "buttonEventPropertyAdd";
            buttonEventPropertyAdd.Size = new System.Drawing.Size(23, 27);
            buttonEventPropertyAdd.TabIndex = 9;
            buttonEventPropertyAdd.Text = ">";
            toolTip.SetToolTip(buttonEventPropertyAdd, "Publish the selected material category");
            buttonEventPropertyAdd.UseVisualStyleBackColor = false;
            buttonEventPropertyAdd.Click += buttonEventPropertyAdd_Click;
            // 
            // buttonEventPropertyRemove
            // 
            buttonEventPropertyRemove.BackColor = System.Drawing.Color.Pink;
            buttonEventPropertyRemove.Dock = System.Windows.Forms.DockStyle.Top;
            buttonEventPropertyRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonEventPropertyRemove.ForeColor = System.Drawing.Color.Red;
            buttonEventPropertyRemove.Location = new System.Drawing.Point(437, 253);
            buttonEventPropertyRemove.Margin = new System.Windows.Forms.Padding(0);
            buttonEventPropertyRemove.Name = "buttonEventPropertyRemove";
            buttonEventPropertyRemove.Size = new System.Drawing.Size(23, 27);
            buttonEventPropertyRemove.TabIndex = 10;
            buttonEventPropertyRemove.Text = "<";
            toolTip.SetToolTip(buttonEventPropertyRemove, "Hide the selected material category");
            buttonEventPropertyRemove.UseVisualStyleBackColor = false;
            buttonEventPropertyRemove.Click += buttonEventPropertyRemove_Click;
            // 
            // buttonEventPropertyTransferExisiting
            // 
            tableLayoutPanelEventProperty.SetColumnSpan(buttonEventPropertyTransferExisiting, 2);
            buttonEventPropertyTransferExisiting.Dock = System.Windows.Forms.DockStyle.Left;
            buttonEventPropertyTransferExisiting.Image = Resource.ArrowNext1;
            buttonEventPropertyTransferExisiting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonEventPropertyTransferExisiting.Location = new System.Drawing.Point(441, 3);
            buttonEventPropertyTransferExisiting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonEventPropertyTransferExisiting.Name = "buttonEventPropertyTransferExisiting";
            buttonEventPropertyTransferExisiting.Size = new System.Drawing.Size(127, 27);
            buttonEventPropertyTransferExisiting.TabIndex = 11;
            buttonEventPropertyTransferExisiting.Text = "Transfer existing";
            buttonEventPropertyTransferExisiting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonEventPropertyTransferExisiting, "Publish all material categories found in the project");
            buttonEventPropertyTransferExisiting.UseVisualStyleBackColor = true;
            buttonEventPropertyTransferExisiting.Click += buttonEventPropertyTransferExisiting_Click;
            // 
            // tabPageTaxonomicGroups
            // 
            tabPageTaxonomicGroups.Controls.Add(tableLayoutPanelTaxonomicGroups);
            tabPageTaxonomicGroups.ImageIndex = 1;
            tabPageTaxonomicGroups.Location = new System.Drawing.Point(4, 24);
            tabPageTaxonomicGroups.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageTaxonomicGroups.Name = "tabPageTaxonomicGroups";
            tabPageTaxonomicGroups.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageTaxonomicGroups.Size = new System.Drawing.Size(760, 459);
            tabPageTaxonomicGroups.TabIndex = 1;
            tabPageTaxonomicGroups.Text = "Taxonomic groups";
            tabPageTaxonomicGroups.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTaxonomicGroups
            // 
            tableLayoutPanelTaxonomicGroups.ColumnCount = 4;
            tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelTaxonomicGroups.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTaxonomicGroups.Controls.Add(labelProjectTaxonomicGroup, 0, 0);
            tableLayoutPanelTaxonomicGroups.Controls.Add(labelProjectTaxonomicGroupNotPublished, 0, 1);
            tableLayoutPanelTaxonomicGroups.Controls.Add(labelProjectTaxonomicGroupPublished, 3, 1);
            tableLayoutPanelTaxonomicGroups.Controls.Add(listBoxProjectTaxonomicGroupNotPublished, 0, 2);
            tableLayoutPanelTaxonomicGroups.Controls.Add(listBoxProjectTaxonomicGroupPublished, 3, 2);
            tableLayoutPanelTaxonomicGroups.Controls.Add(buttonProjectTaxonomicGroupPublished, 2, 2);
            tableLayoutPanelTaxonomicGroups.Controls.Add(buttonProjectTaxonomicGroupNotPublished, 2, 3);
            tableLayoutPanelTaxonomicGroups.Controls.Add(buttonProjectTaxonomicGroupTransferExisting, 2, 0);
            tableLayoutPanelTaxonomicGroups.Controls.Add(checkBoxProjectTaxonomicGroupPublishedRestricted, 3, 4);
            tableLayoutPanelTaxonomicGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelTaxonomicGroups.Location = new System.Drawing.Point(4, 3);
            tableLayoutPanelTaxonomicGroups.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelTaxonomicGroups.Name = "tableLayoutPanelTaxonomicGroups";
            tableLayoutPanelTaxonomicGroups.RowCount = 5;
            tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelTaxonomicGroups.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelTaxonomicGroups.Size = new System.Drawing.Size(752, 453);
            tableLayoutPanelTaxonomicGroups.TabIndex = 3;
            // 
            // labelProjectTaxonomicGroup
            // 
            labelProjectTaxonomicGroup.AutoSize = true;
            tableLayoutPanelTaxonomicGroups.SetColumnSpan(labelProjectTaxonomicGroup, 2);
            labelProjectTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectTaxonomicGroup.Location = new System.Drawing.Point(4, 3);
            labelProjectTaxonomicGroup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelProjectTaxonomicGroup.Name = "labelProjectTaxonomicGroup";
            labelProjectTaxonomicGroup.Size = new System.Drawing.Size(430, 27);
            labelProjectTaxonomicGroup.TabIndex = 1;
            labelProjectTaxonomicGroup.Text = "Taxonomic groups transfered into the cache database";
            labelProjectTaxonomicGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProjectTaxonomicGroupNotPublished
            // 
            labelProjectTaxonomicGroupNotPublished.AutoSize = true;
            tableLayoutPanelTaxonomicGroups.SetColumnSpan(labelProjectTaxonomicGroupNotPublished, 2);
            labelProjectTaxonomicGroupNotPublished.Location = new System.Drawing.Point(4, 33);
            labelProjectTaxonomicGroupNotPublished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectTaxonomicGroupNotPublished.Name = "labelProjectTaxonomicGroupNotPublished";
            labelProjectTaxonomicGroupNotPublished.Size = new System.Drawing.Size(82, 15);
            labelProjectTaxonomicGroupNotPublished.TabIndex = 5;
            labelProjectTaxonomicGroupNotPublished.Text = "Not published";
            // 
            // labelProjectTaxonomicGroupPublished
            // 
            labelProjectTaxonomicGroupPublished.AutoSize = true;
            labelProjectTaxonomicGroupPublished.Location = new System.Drawing.Point(465, 33);
            labelProjectTaxonomicGroupPublished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectTaxonomicGroupPublished.Name = "labelProjectTaxonomicGroupPublished";
            labelProjectTaxonomicGroupPublished.Size = new System.Drawing.Size(59, 15);
            labelProjectTaxonomicGroupPublished.TabIndex = 6;
            labelProjectTaxonomicGroupPublished.Text = "Published";
            // 
            // listBoxProjectTaxonomicGroupNotPublished
            // 
            listBoxProjectTaxonomicGroupNotPublished.BackColor = System.Drawing.Color.Pink;
            tableLayoutPanelTaxonomicGroups.SetColumnSpan(listBoxProjectTaxonomicGroupNotPublished, 2);
            listBoxProjectTaxonomicGroupNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectTaxonomicGroupNotPublished.FormattingEnabled = true;
            listBoxProjectTaxonomicGroupNotPublished.ItemHeight = 15;
            listBoxProjectTaxonomicGroupNotPublished.Location = new System.Drawing.Point(4, 51);
            listBoxProjectTaxonomicGroupNotPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxProjectTaxonomicGroupNotPublished.Name = "listBoxProjectTaxonomicGroupNotPublished";
            tableLayoutPanelTaxonomicGroups.SetRowSpan(listBoxProjectTaxonomicGroupNotPublished, 3);
            listBoxProjectTaxonomicGroupNotPublished.Size = new System.Drawing.Size(430, 399);
            listBoxProjectTaxonomicGroupNotPublished.Sorted = true;
            listBoxProjectTaxonomicGroupNotPublished.TabIndex = 7;
            // 
            // listBoxProjectTaxonomicGroupPublished
            // 
            listBoxProjectTaxonomicGroupPublished.BackColor = System.Drawing.Color.LightGreen;
            listBoxProjectTaxonomicGroupPublished.DisplayMember = "TaxonomicGroup";
            listBoxProjectTaxonomicGroupPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectTaxonomicGroupPublished.FormattingEnabled = true;
            listBoxProjectTaxonomicGroupPublished.IntegralHeight = false;
            listBoxProjectTaxonomicGroupPublished.ItemHeight = 15;
            listBoxProjectTaxonomicGroupPublished.Location = new System.Drawing.Point(465, 51);
            listBoxProjectTaxonomicGroupPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxProjectTaxonomicGroupPublished.Name = "listBoxProjectTaxonomicGroupPublished";
            tableLayoutPanelTaxonomicGroups.SetRowSpan(listBoxProjectTaxonomicGroupPublished, 2);
            listBoxProjectTaxonomicGroupPublished.Size = new System.Drawing.Size(283, 354);
            listBoxProjectTaxonomicGroupPublished.Sorted = true;
            listBoxProjectTaxonomicGroupPublished.TabIndex = 8;
            listBoxProjectTaxonomicGroupPublished.SelectedIndexChanged += listBoxProjectTaxonomicGroupPublished_SelectedIndexChanged;
            // 
            // buttonProjectTaxonomicGroupPublished
            // 
            buttonProjectTaxonomicGroupPublished.BackColor = System.Drawing.Color.LightGreen;
            buttonProjectTaxonomicGroupPublished.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonProjectTaxonomicGroupPublished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectTaxonomicGroupPublished.ForeColor = System.Drawing.Color.Green;
            buttonProjectTaxonomicGroupPublished.Location = new System.Drawing.Point(438, 201);
            buttonProjectTaxonomicGroupPublished.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectTaxonomicGroupPublished.Name = "buttonProjectTaxonomicGroupPublished";
            buttonProjectTaxonomicGroupPublished.Size = new System.Drawing.Size(23, 27);
            buttonProjectTaxonomicGroupPublished.TabIndex = 9;
            buttonProjectTaxonomicGroupPublished.Text = ">";
            toolTip.SetToolTip(buttonProjectTaxonomicGroupPublished, "Publish the selected taxonmic group");
            buttonProjectTaxonomicGroupPublished.UseVisualStyleBackColor = false;
            buttonProjectTaxonomicGroupPublished.Click += buttonProjectTaxonomicGroupPublished_Click;
            // 
            // buttonProjectTaxonomicGroupNotPublished
            // 
            buttonProjectTaxonomicGroupNotPublished.BackColor = System.Drawing.Color.Pink;
            buttonProjectTaxonomicGroupNotPublished.Dock = System.Windows.Forms.DockStyle.Top;
            buttonProjectTaxonomicGroupNotPublished.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectTaxonomicGroupNotPublished.ForeColor = System.Drawing.Color.Red;
            buttonProjectTaxonomicGroupNotPublished.Location = new System.Drawing.Point(438, 228);
            buttonProjectTaxonomicGroupNotPublished.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectTaxonomicGroupNotPublished.Name = "buttonProjectTaxonomicGroupNotPublished";
            buttonProjectTaxonomicGroupNotPublished.Size = new System.Drawing.Size(23, 27);
            buttonProjectTaxonomicGroupNotPublished.TabIndex = 10;
            buttonProjectTaxonomicGroupNotPublished.Text = "<";
            toolTip.SetToolTip(buttonProjectTaxonomicGroupNotPublished, "Hide the selected taxonomic group");
            buttonProjectTaxonomicGroupNotPublished.UseVisualStyleBackColor = false;
            buttonProjectTaxonomicGroupNotPublished.Click += buttonProjectTaxonomicGroupNotPublished_Click;
            // 
            // buttonProjectTaxonomicGroupTransferExisting
            // 
            tableLayoutPanelTaxonomicGroups.SetColumnSpan(buttonProjectTaxonomicGroupTransferExisting, 2);
            buttonProjectTaxonomicGroupTransferExisting.Dock = System.Windows.Forms.DockStyle.Left;
            buttonProjectTaxonomicGroupTransferExisting.Image = Resource.ArrowNext1;
            buttonProjectTaxonomicGroupTransferExisting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonProjectTaxonomicGroupTransferExisting.Location = new System.Drawing.Point(442, 3);
            buttonProjectTaxonomicGroupTransferExisting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonProjectTaxonomicGroupTransferExisting.Name = "buttonProjectTaxonomicGroupTransferExisting";
            buttonProjectTaxonomicGroupTransferExisting.Size = new System.Drawing.Size(127, 27);
            buttonProjectTaxonomicGroupTransferExisting.TabIndex = 11;
            buttonProjectTaxonomicGroupTransferExisting.Text = "Transfer existing";
            buttonProjectTaxonomicGroupTransferExisting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonProjectTaxonomicGroupTransferExisting, "Publish all taxonomic groups found within the project");
            buttonProjectTaxonomicGroupTransferExisting.UseVisualStyleBackColor = true;
            buttonProjectTaxonomicGroupTransferExisting.Click += buttonProjectTaxonomicGroupTransferExisting_Click;
            // 
            // checkBoxProjectTaxonomicGroupPublishedRestricted
            // 
            checkBoxProjectTaxonomicGroupPublishedRestricted.Location = new System.Drawing.Point(465, 411);
            checkBoxProjectTaxonomicGroupPublishedRestricted.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxProjectTaxonomicGroupPublishedRestricted.Name = "checkBoxProjectTaxonomicGroupPublishedRestricted";
            checkBoxProjectTaxonomicGroupPublishedRestricted.Size = new System.Drawing.Size(272, 38);
            checkBoxProjectTaxonomicGroupPublishedRestricted.TabIndex = 12;
            checkBoxProjectTaxonomicGroupPublishedRestricted.Text = "Restrict to last identification linked to a taxonomic thesaurus";
            toolTip.SetToolTip(checkBoxProjectTaxonomicGroupPublishedRestricted, "If the last identification of an organism must be linked to a taxonomic thesaurus");
            checkBoxProjectTaxonomicGroupPublishedRestricted.UseVisualStyleBackColor = true;
            checkBoxProjectTaxonomicGroupPublishedRestricted.Click += checkBoxProjectTaxonomicGroupPublishedRestricted_Click;
            // 
            // tabPageMaterialCategories
            // 
            tabPageMaterialCategories.Controls.Add(tableLayoutPanelProjectMaterialCategory);
            tabPageMaterialCategories.ImageIndex = 2;
            tabPageMaterialCategories.Location = new System.Drawing.Point(4, 24);
            tabPageMaterialCategories.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageMaterialCategories.Name = "tabPageMaterialCategories";
            tabPageMaterialCategories.Size = new System.Drawing.Size(760, 459);
            tabPageMaterialCategories.TabIndex = 2;
            tabPageMaterialCategories.Text = "Material categories";
            tabPageMaterialCategories.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelProjectMaterialCategory
            // 
            tableLayoutPanelProjectMaterialCategory.ColumnCount = 4;
            tableLayoutPanelProjectMaterialCategory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProjectMaterialCategory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectMaterialCategory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelProjectMaterialCategory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectMaterialCategory.Controls.Add(labelProjectMaterialCategoryHeader, 0, 0);
            tableLayoutPanelProjectMaterialCategory.Controls.Add(labelProjectMaterialCategoryNotPublished, 0, 1);
            tableLayoutPanelProjectMaterialCategory.Controls.Add(labelProjectMaterialCategoryPublished, 3, 1);
            tableLayoutPanelProjectMaterialCategory.Controls.Add(listBoxProjectMaterialCategoryNotPublished, 0, 2);
            tableLayoutPanelProjectMaterialCategory.Controls.Add(listBoxProjectMaterialCategoryPublished, 3, 2);
            tableLayoutPanelProjectMaterialCategory.Controls.Add(buttonProjectMaterialCategoryPublishe, 2, 2);
            tableLayoutPanelProjectMaterialCategory.Controls.Add(buttonProjectMaterialCategoryWithhold, 2, 3);
            tableLayoutPanelProjectMaterialCategory.Controls.Add(buttonProjectMaterialCategoryTransferExisting, 2, 0);
            tableLayoutPanelProjectMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelProjectMaterialCategory.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelProjectMaterialCategory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelProjectMaterialCategory.Name = "tableLayoutPanelProjectMaterialCategory";
            tableLayoutPanelProjectMaterialCategory.RowCount = 4;
            tableLayoutPanelProjectMaterialCategory.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjectMaterialCategory.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProjectMaterialCategory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectMaterialCategory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelProjectMaterialCategory.Size = new System.Drawing.Size(760, 459);
            tableLayoutPanelProjectMaterialCategory.TabIndex = 4;
            // 
            // labelProjectMaterialCategoryHeader
            // 
            labelProjectMaterialCategoryHeader.AutoSize = true;
            tableLayoutPanelProjectMaterialCategory.SetColumnSpan(labelProjectMaterialCategoryHeader, 2);
            labelProjectMaterialCategoryHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProjectMaterialCategoryHeader.Location = new System.Drawing.Point(4, 3);
            labelProjectMaterialCategoryHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelProjectMaterialCategoryHeader.Name = "labelProjectMaterialCategoryHeader";
            labelProjectMaterialCategoryHeader.Size = new System.Drawing.Size(435, 27);
            labelProjectMaterialCategoryHeader.TabIndex = 1;
            labelProjectMaterialCategoryHeader.Text = "Material categories transfered into the cache database";
            labelProjectMaterialCategoryHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProjectMaterialCategoryNotPublished
            // 
            labelProjectMaterialCategoryNotPublished.AutoSize = true;
            tableLayoutPanelProjectMaterialCategory.SetColumnSpan(labelProjectMaterialCategoryNotPublished, 2);
            labelProjectMaterialCategoryNotPublished.Location = new System.Drawing.Point(4, 33);
            labelProjectMaterialCategoryNotPublished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectMaterialCategoryNotPublished.Name = "labelProjectMaterialCategoryNotPublished";
            labelProjectMaterialCategoryNotPublished.Size = new System.Drawing.Size(82, 15);
            labelProjectMaterialCategoryNotPublished.TabIndex = 5;
            labelProjectMaterialCategoryNotPublished.Text = "Not published";
            // 
            // labelProjectMaterialCategoryPublished
            // 
            labelProjectMaterialCategoryPublished.AutoSize = true;
            labelProjectMaterialCategoryPublished.Location = new System.Drawing.Point(470, 33);
            labelProjectMaterialCategoryPublished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProjectMaterialCategoryPublished.Name = "labelProjectMaterialCategoryPublished";
            labelProjectMaterialCategoryPublished.Size = new System.Drawing.Size(59, 15);
            labelProjectMaterialCategoryPublished.TabIndex = 6;
            labelProjectMaterialCategoryPublished.Text = "Published";
            // 
            // listBoxProjectMaterialCategoryNotPublished
            // 
            listBoxProjectMaterialCategoryNotPublished.BackColor = System.Drawing.Color.Pink;
            tableLayoutPanelProjectMaterialCategory.SetColumnSpan(listBoxProjectMaterialCategoryNotPublished, 2);
            listBoxProjectMaterialCategoryNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectMaterialCategoryNotPublished.FormattingEnabled = true;
            listBoxProjectMaterialCategoryNotPublished.ItemHeight = 15;
            listBoxProjectMaterialCategoryNotPublished.Location = new System.Drawing.Point(4, 51);
            listBoxProjectMaterialCategoryNotPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxProjectMaterialCategoryNotPublished.Name = "listBoxProjectMaterialCategoryNotPublished";
            tableLayoutPanelProjectMaterialCategory.SetRowSpan(listBoxProjectMaterialCategoryNotPublished, 2);
            listBoxProjectMaterialCategoryNotPublished.Size = new System.Drawing.Size(435, 405);
            listBoxProjectMaterialCategoryNotPublished.Sorted = true;
            listBoxProjectMaterialCategoryNotPublished.TabIndex = 7;
            // 
            // listBoxProjectMaterialCategoryPublished
            // 
            listBoxProjectMaterialCategoryPublished.BackColor = System.Drawing.Color.LightGreen;
            listBoxProjectMaterialCategoryPublished.DisplayMember = "TaxonomicGroup";
            listBoxProjectMaterialCategoryPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxProjectMaterialCategoryPublished.FormattingEnabled = true;
            listBoxProjectMaterialCategoryPublished.ItemHeight = 15;
            listBoxProjectMaterialCategoryPublished.Location = new System.Drawing.Point(470, 51);
            listBoxProjectMaterialCategoryPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxProjectMaterialCategoryPublished.Name = "listBoxProjectMaterialCategoryPublished";
            tableLayoutPanelProjectMaterialCategory.SetRowSpan(listBoxProjectMaterialCategoryPublished, 2);
            listBoxProjectMaterialCategoryPublished.Size = new System.Drawing.Size(286, 405);
            listBoxProjectMaterialCategoryPublished.Sorted = true;
            listBoxProjectMaterialCategoryPublished.TabIndex = 8;
            // 
            // buttonProjectMaterialCategoryPublishe
            // 
            buttonProjectMaterialCategoryPublishe.BackColor = System.Drawing.Color.LightGreen;
            buttonProjectMaterialCategoryPublishe.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonProjectMaterialCategoryPublishe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectMaterialCategoryPublishe.ForeColor = System.Drawing.Color.Green;
            buttonProjectMaterialCategoryPublishe.Location = new System.Drawing.Point(443, 226);
            buttonProjectMaterialCategoryPublishe.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectMaterialCategoryPublishe.Name = "buttonProjectMaterialCategoryPublishe";
            buttonProjectMaterialCategoryPublishe.Size = new System.Drawing.Size(23, 27);
            buttonProjectMaterialCategoryPublishe.TabIndex = 9;
            buttonProjectMaterialCategoryPublishe.Text = ">";
            toolTip.SetToolTip(buttonProjectMaterialCategoryPublishe, "Publish the selected material category");
            buttonProjectMaterialCategoryPublishe.UseVisualStyleBackColor = false;
            buttonProjectMaterialCategoryPublishe.Click += buttonProjectMaterialCategoryPublishe_Click;
            // 
            // buttonProjectMaterialCategoryWithhold
            // 
            buttonProjectMaterialCategoryWithhold.BackColor = System.Drawing.Color.Pink;
            buttonProjectMaterialCategoryWithhold.Dock = System.Windows.Forms.DockStyle.Top;
            buttonProjectMaterialCategoryWithhold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonProjectMaterialCategoryWithhold.ForeColor = System.Drawing.Color.Red;
            buttonProjectMaterialCategoryWithhold.Location = new System.Drawing.Point(443, 253);
            buttonProjectMaterialCategoryWithhold.Margin = new System.Windows.Forms.Padding(0);
            buttonProjectMaterialCategoryWithhold.Name = "buttonProjectMaterialCategoryWithhold";
            buttonProjectMaterialCategoryWithhold.Size = new System.Drawing.Size(23, 27);
            buttonProjectMaterialCategoryWithhold.TabIndex = 10;
            buttonProjectMaterialCategoryWithhold.Text = "<";
            toolTip.SetToolTip(buttonProjectMaterialCategoryWithhold, "Hide the selected material category");
            buttonProjectMaterialCategoryWithhold.UseVisualStyleBackColor = false;
            buttonProjectMaterialCategoryWithhold.Click += buttonProjectMaterialCategoryWithhold_Click;
            // 
            // buttonProjectMaterialCategoryTransferExisting
            // 
            tableLayoutPanelProjectMaterialCategory.SetColumnSpan(buttonProjectMaterialCategoryTransferExisting, 2);
            buttonProjectMaterialCategoryTransferExisting.Dock = System.Windows.Forms.DockStyle.Left;
            buttonProjectMaterialCategoryTransferExisting.Image = Resource.ArrowNext1;
            buttonProjectMaterialCategoryTransferExisting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonProjectMaterialCategoryTransferExisting.Location = new System.Drawing.Point(447, 3);
            buttonProjectMaterialCategoryTransferExisting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonProjectMaterialCategoryTransferExisting.Name = "buttonProjectMaterialCategoryTransferExisting";
            buttonProjectMaterialCategoryTransferExisting.Size = new System.Drawing.Size(127, 27);
            buttonProjectMaterialCategoryTransferExisting.TabIndex = 11;
            buttonProjectMaterialCategoryTransferExisting.Text = "Transfer existing";
            buttonProjectMaterialCategoryTransferExisting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonProjectMaterialCategoryTransferExisting, "Publish all material categories found in the project");
            buttonProjectMaterialCategoryTransferExisting.UseVisualStyleBackColor = true;
            buttonProjectMaterialCategoryTransferExisting.Click += buttonProjectMaterialCategoryTransferExisting_Click;
            // 
            // tabPageAnalysis
            // 
            tabPageAnalysis.Controls.Add(tableLayoutPanelAnalysis);
            tabPageAnalysis.ImageIndex = 3;
            tabPageAnalysis.Location = new System.Drawing.Point(4, 24);
            tabPageAnalysis.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabPageAnalysis.Name = "tabPageAnalysis";
            tabPageAnalysis.Size = new System.Drawing.Size(760, 459);
            tabPageAnalysis.TabIndex = 3;
            tabPageAnalysis.Text = "Analysis";
            tabPageAnalysis.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelAnalysis
            // 
            tableLayoutPanelAnalysis.ColumnCount = 4;
            tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelAnalysis.Controls.Add(labelAnalysisHeader, 0, 0);
            tableLayoutPanelAnalysis.Controls.Add(labelAnalysisNotPublished, 0, 1);
            tableLayoutPanelAnalysis.Controls.Add(labelAnalysisPublished, 3, 1);
            tableLayoutPanelAnalysis.Controls.Add(listBoxAnalysisNotPublished, 0, 2);
            tableLayoutPanelAnalysis.Controls.Add(listBoxAnalysisPublished, 3, 2);
            tableLayoutPanelAnalysis.Controls.Add(buttonAnalysisPublish, 2, 2);
            tableLayoutPanelAnalysis.Controls.Add(buttonAnalysisBlock, 2, 3);
            tableLayoutPanelAnalysis.Controls.Add(buttonAnalysisTransferExisting, 2, 0);
            tableLayoutPanelAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelAnalysis.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelAnalysis.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelAnalysis.Name = "tableLayoutPanelAnalysis";
            tableLayoutPanelAnalysis.RowCount = 4;
            tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelAnalysis.Size = new System.Drawing.Size(760, 459);
            tableLayoutPanelAnalysis.TabIndex = 5;
            // 
            // labelAnalysisHeader
            // 
            labelAnalysisHeader.AutoSize = true;
            tableLayoutPanelAnalysis.SetColumnSpan(labelAnalysisHeader, 2);
            labelAnalysisHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelAnalysisHeader.Location = new System.Drawing.Point(4, 3);
            labelAnalysisHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            labelAnalysisHeader.Name = "labelAnalysisHeader";
            labelAnalysisHeader.Size = new System.Drawing.Size(421, 27);
            labelAnalysisHeader.TabIndex = 1;
            labelAnalysisHeader.Text = "Analysis transfered into the cache database";
            labelAnalysisHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAnalysisNotPublished
            // 
            labelAnalysisNotPublished.AutoSize = true;
            tableLayoutPanelAnalysis.SetColumnSpan(labelAnalysisNotPublished, 2);
            labelAnalysisNotPublished.Location = new System.Drawing.Point(4, 33);
            labelAnalysisNotPublished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelAnalysisNotPublished.Name = "labelAnalysisNotPublished";
            labelAnalysisNotPublished.Size = new System.Drawing.Size(82, 15);
            labelAnalysisNotPublished.TabIndex = 5;
            labelAnalysisNotPublished.Text = "Not published";
            // 
            // labelAnalysisPublished
            // 
            labelAnalysisPublished.AutoSize = true;
            labelAnalysisPublished.Location = new System.Drawing.Point(456, 33);
            labelAnalysisPublished.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelAnalysisPublished.Name = "labelAnalysisPublished";
            labelAnalysisPublished.Size = new System.Drawing.Size(59, 15);
            labelAnalysisPublished.TabIndex = 6;
            labelAnalysisPublished.Text = "Published";
            // 
            // listBoxAnalysisNotPublished
            // 
            listBoxAnalysisNotPublished.BackColor = System.Drawing.Color.Pink;
            tableLayoutPanelAnalysis.SetColumnSpan(listBoxAnalysisNotPublished, 2);
            listBoxAnalysisNotPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxAnalysisNotPublished.FormattingEnabled = true;
            listBoxAnalysisNotPublished.ItemHeight = 15;
            listBoxAnalysisNotPublished.Location = new System.Drawing.Point(4, 51);
            listBoxAnalysisNotPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxAnalysisNotPublished.Name = "listBoxAnalysisNotPublished";
            tableLayoutPanelAnalysis.SetRowSpan(listBoxAnalysisNotPublished, 2);
            listBoxAnalysisNotPublished.Size = new System.Drawing.Size(421, 405);
            listBoxAnalysisNotPublished.Sorted = true;
            listBoxAnalysisNotPublished.TabIndex = 7;
            // 
            // listBoxAnalysisPublished
            // 
            listBoxAnalysisPublished.BackColor = System.Drawing.Color.LightGreen;
            listBoxAnalysisPublished.DisplayMember = "TaxonomicGroup";
            listBoxAnalysisPublished.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxAnalysisPublished.FormattingEnabled = true;
            listBoxAnalysisPublished.ItemHeight = 15;
            listBoxAnalysisPublished.Location = new System.Drawing.Point(456, 51);
            listBoxAnalysisPublished.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxAnalysisPublished.Name = "listBoxAnalysisPublished";
            tableLayoutPanelAnalysis.SetRowSpan(listBoxAnalysisPublished, 2);
            listBoxAnalysisPublished.Size = new System.Drawing.Size(300, 405);
            listBoxAnalysisPublished.Sorted = true;
            listBoxAnalysisPublished.TabIndex = 8;
            // 
            // buttonAnalysisPublish
            // 
            buttonAnalysisPublish.BackColor = System.Drawing.Color.LightGreen;
            buttonAnalysisPublish.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonAnalysisPublish.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonAnalysisPublish.ForeColor = System.Drawing.Color.Green;
            buttonAnalysisPublish.Location = new System.Drawing.Point(429, 226);
            buttonAnalysisPublish.Margin = new System.Windows.Forms.Padding(0);
            buttonAnalysisPublish.Name = "buttonAnalysisPublish";
            buttonAnalysisPublish.Size = new System.Drawing.Size(23, 27);
            buttonAnalysisPublish.TabIndex = 9;
            buttonAnalysisPublish.Text = ">";
            toolTip.SetToolTip(buttonAnalysisPublish, "Publish the selected material category");
            buttonAnalysisPublish.UseVisualStyleBackColor = false;
            buttonAnalysisPublish.Click += buttonAnalysisPublish_Click;
            // 
            // buttonAnalysisBlock
            // 
            buttonAnalysisBlock.BackColor = System.Drawing.Color.Pink;
            buttonAnalysisBlock.Dock = System.Windows.Forms.DockStyle.Top;
            buttonAnalysisBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonAnalysisBlock.ForeColor = System.Drawing.Color.Red;
            buttonAnalysisBlock.Location = new System.Drawing.Point(429, 253);
            buttonAnalysisBlock.Margin = new System.Windows.Forms.Padding(0);
            buttonAnalysisBlock.Name = "buttonAnalysisBlock";
            buttonAnalysisBlock.Size = new System.Drawing.Size(23, 27);
            buttonAnalysisBlock.TabIndex = 10;
            buttonAnalysisBlock.Text = "<";
            toolTip.SetToolTip(buttonAnalysisBlock, "Hide the selected material category");
            buttonAnalysisBlock.UseVisualStyleBackColor = false;
            buttonAnalysisBlock.Click += buttonAnalysisBlock_Click;
            // 
            // buttonAnalysisTransferExisting
            // 
            tableLayoutPanelAnalysis.SetColumnSpan(buttonAnalysisTransferExisting, 2);
            buttonAnalysisTransferExisting.Dock = System.Windows.Forms.DockStyle.Left;
            buttonAnalysisTransferExisting.Image = Resource.ArrowNext1;
            buttonAnalysisTransferExisting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonAnalysisTransferExisting.Location = new System.Drawing.Point(433, 3);
            buttonAnalysisTransferExisting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAnalysisTransferExisting.Name = "buttonAnalysisTransferExisting";
            buttonAnalysisTransferExisting.Size = new System.Drawing.Size(127, 27);
            buttonAnalysisTransferExisting.TabIndex = 11;
            buttonAnalysisTransferExisting.Text = "Transfer existing";
            buttonAnalysisTransferExisting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            toolTip.SetToolTip(buttonAnalysisTransferExisting, "Publish all material categories found in the project");
            buttonAnalysisTransferExisting.UseVisualStyleBackColor = true;
            buttonAnalysisTransferExisting.Click += buttonAnalysisTransferExisting_Click;
            // 
            // imageListTabcontrol
            // 
            imageListTabcontrol.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageListTabcontrol.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListTabcontrol.ImageStream");
            imageListTabcontrol.TransparentColor = System.Drawing.Color.Transparent;
            imageListTabcontrol.Images.SetKeyName(0, "Localisation.ico");
            imageListTabcontrol.Images.SetKeyName(1, "Plant.ico");
            imageListTabcontrol.Images.SetKeyName(2, "Specimen.ico");
            imageListTabcontrol.Images.SetKeyName(3, "Analysis.ico");
            imageListTabcontrol.Images.SetKeyName(4, "CollectionSpecimen.ico");
            imageListTabcontrol.Images.SetKeyName(5, "EventProperty.ico");
            // 
            // buttonFeedback
            // 
            buttonFeedback.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonFeedback.Image = Resource.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(734, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(28, 27);
            buttonFeedback.TabIndex = 1;
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // FormTransferFilter
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(768, 487);
            Controls.Add(buttonFeedback);
            Controls.Add(tabControlMain);
            helpProvider.SetHelpKeyword(this, "cachedatabase_restrictions_dc");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormTransferFilter";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "FormTransferFilter";
            KeyDown += Form_KeyDown;
            tabControlMain.ResumeLayout(false);
            tabPageSpecimen.ResumeLayout(false);
            tabPageLocalisation.ResumeLayout(false);
            tableLayoutPanelProjectLocalisation.ResumeLayout(false);
            tableLayoutPanelProjectLocalisation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownCoordinatePrecision).EndInit();
            tabPageSiteProperties.ResumeLayout(false);
            tableLayoutPanelEventProperty.ResumeLayout(false);
            tableLayoutPanelEventProperty.PerformLayout();
            tabPageTaxonomicGroups.ResumeLayout(false);
            tableLayoutPanelTaxonomicGroups.ResumeLayout(false);
            tableLayoutPanelTaxonomicGroups.PerformLayout();
            tabPageMaterialCategories.ResumeLayout(false);
            tableLayoutPanelProjectMaterialCategory.ResumeLayout(false);
            tableLayoutPanelProjectMaterialCategory.PerformLayout();
            tabPageAnalysis.ResumeLayout(false);
            tableLayoutPanelAnalysis.ResumeLayout(false);
            tableLayoutPanelAnalysis.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageLocalisation;
        private System.Windows.Forms.TabPage tabPageTaxonomicGroups;
        private System.Windows.Forms.ImageList imageListTabcontrol;
        private System.Windows.Forms.TabPage tabPageMaterialCategories;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjectLocalisation;
        private System.Windows.Forms.Label labelProjectLocalisationNotPublished;
        private System.Windows.Forms.ListBox listBoxProjectLocalisationNotPublished;
        private System.Windows.Forms.Label labelProjectLocalisationPublished;
        private System.Windows.Forms.ListBox listBoxProjectLocalisationPubished;
        private System.Windows.Forms.Button buttonProjectLocalisationPublish;
        private System.Windows.Forms.Button buttonProjectLocalisationHide;
        private System.Windows.Forms.Button buttonProjectLocalisationUp;
        private System.Windows.Forms.Button buttonProjectLocalisationDown;
        private System.Windows.Forms.Label labelProjectLocalisationHeader;
        private System.Windows.Forms.CheckBox checkBoxCoordinatePrecision;
        private System.Windows.Forms.NumericUpDown numericUpDownCoordinatePrecision;
        private System.Windows.Forms.Label labelCoordinatePrecision;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProjectMaterialCategory;
        private System.Windows.Forms.Label labelProjectMaterialCategoryHeader;
        private System.Windows.Forms.Label labelProjectMaterialCategoryNotPublished;
        private System.Windows.Forms.Label labelProjectMaterialCategoryPublished;
        private System.Windows.Forms.ListBox listBoxProjectMaterialCategoryNotPublished;
        private System.Windows.Forms.ListBox listBoxProjectMaterialCategoryPublished;
        private System.Windows.Forms.Button buttonProjectMaterialCategoryPublishe;
        private System.Windows.Forms.Button buttonProjectMaterialCategoryWithhold;
        private System.Windows.Forms.Button buttonProjectMaterialCategoryTransferExisting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaxonomicGroups;
        private System.Windows.Forms.Label labelProjectTaxonomicGroup;
        private System.Windows.Forms.Label labelProjectTaxonomicGroupNotPublished;
        private System.Windows.Forms.Label labelProjectTaxonomicGroupPublished;
        private System.Windows.Forms.ListBox listBoxProjectTaxonomicGroupNotPublished;
        private System.Windows.Forms.ListBox listBoxProjectTaxonomicGroupPublished;
        private System.Windows.Forms.Button buttonProjectTaxonomicGroupPublished;
        private System.Windows.Forms.Button buttonProjectTaxonomicGroupNotPublished;
        private System.Windows.Forms.Button buttonProjectTaxonomicGroupTransferExisting;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.TabPage tabPageAnalysis;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAnalysis;
        private System.Windows.Forms.Label labelAnalysisHeader;
        private System.Windows.Forms.Label labelAnalysisNotPublished;
        private System.Windows.Forms.Label labelAnalysisPublished;
        private System.Windows.Forms.ListBox listBoxAnalysisNotPublished;
        private System.Windows.Forms.ListBox listBoxAnalysisPublished;
        private System.Windows.Forms.Button buttonAnalysisPublish;
        private System.Windows.Forms.Button buttonAnalysisBlock;
        private System.Windows.Forms.Button buttonAnalysisTransferExisting;
        private System.Windows.Forms.CheckBox checkBoxProjectTaxonomicGroupPublishedRestricted;
        private System.Windows.Forms.TabPage tabPageSpecimen;
        private DiversityWorkbench.UserControls.UserControlQueryList userControlQueryListSpecimen;
        private System.Windows.Forms.TabPage tabPageSiteProperties;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEventProperty;
        private System.Windows.Forms.Label labelEventPropertyHeader;
        private System.Windows.Forms.Label labelEventPropertyNotPublished;
        private System.Windows.Forms.Label labelEventPropertyPublished;
        private System.Windows.Forms.ListBox listBoxEventPropertyNotPublished;
        private System.Windows.Forms.ListBox listBoxEventPropertyPublished;
        private System.Windows.Forms.Button buttonEventPropertyAdd;
        private System.Windows.Forms.Button buttonEventPropertyRemove;
        private System.Windows.Forms.Button buttonEventPropertyTransferExisiting;
    }
}