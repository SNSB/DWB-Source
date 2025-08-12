namespace DiversityCollection.Forms
{
    partial class FormXmlExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormXmlExport));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelTechnicalContact = new System.Windows.Forms.Label();
            this.textBoxTechnicalContact1 = new System.Windows.Forms.TextBox();
            this.labelCollection = new System.Windows.Forms.Label();
            this.labelContentContact = new System.Windows.Forms.Label();
            this.textBoxContentContact1 = new System.Windows.Forms.TextBox();
            this.textBoxTechnicalContact2 = new System.Windows.Forms.TextBox();
            this.textBoxContentContact2 = new System.Windows.Forms.TextBox();
            this.labelOtherProviders = new System.Windows.Forms.Label();
            this.textBoxOtherProviders = new System.Windows.Forms.TextBox();
            this.labelIconURI = new System.Windows.Forms.Label();
            this.textBoxIconURI = new System.Windows.Forms.TextBox();
            this.labelMetadata = new System.Windows.Forms.Label();
            this.labelScope = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxScope = new System.Windows.Forms.TextBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.dateTimePickerDateIssued = new System.Windows.Forms.DateTimePicker();
            this.comboBoxCollection = new System.Windows.Forms.ComboBox();
            this.labelExportFile = new System.Windows.Forms.Label();
            this.textBoxExportFile = new System.Windows.Forms.TextBox();
            this.labelDatasetGUID = new System.Windows.Forms.Label();
            this.textBoxDatasetGUID = new System.Windows.Forms.TextBox();
            this.buttonOpenIconURI = new System.Windows.Forms.Button();
            this.buttonOpenExportFile = new System.Windows.Forms.Button();
            this.panelIconURI = new System.Windows.Forms.Panel();
            this.webBrowserIconURI = new System.Windows.Forms.WebBrowser();
            this.labelHeader = new System.Windows.Forms.Label();
            this.userControlHierarchySelectorCollection = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            this.buttonCreateGuid = new System.Windows.Forms.Button();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            this.panelIconURI.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 5;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.labelTechnicalContact, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTechnicalContact1, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelCollection, 0, 11);
            this.tableLayoutPanelMain.Controls.Add(this.labelContentContact, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxContentContact1, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTechnicalContact2, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxContentContact2, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelOtherProviders, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxOtherProviders, 1, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelIconURI, 0, 7);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxIconURI, 1, 7);
            this.tableLayoutPanelMain.Controls.Add(this.labelMetadata, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.labelScope, 0, 8);
            this.tableLayoutPanelMain.Controls.Add(this.buttonStart, 3, 13);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxScope, 1, 8);
            this.tableLayoutPanelMain.Controls.Add(this.labelVersion, 0, 9);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxVersion, 1, 9);
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerDateIssued, 3, 9);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxCollection, 1, 11);
            this.tableLayoutPanelMain.Controls.Add(this.labelExportFile, 0, 12);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxExportFile, 1, 12);
            this.tableLayoutPanelMain.Controls.Add(this.labelDatasetGUID, 0, 10);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxDatasetGUID, 1, 10);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOpenIconURI, 2, 7);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOpenExportFile, 4, 12);
            this.tableLayoutPanelMain.Controls.Add(this.panelIconURI, 3, 6);
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.userControlHierarchySelectorCollection, 4, 11);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCreateGuid, 3, 10);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 4, 0);
            this.tableLayoutPanelMain.Controls.Add(this.webBrowser, 0, 14);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 15;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(712, 547);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // labelTechnicalContact
            // 
            this.labelTechnicalContact.AutoSize = true;
            this.labelTechnicalContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTechnicalContact.Location = new System.Drawing.Point(3, 22);
            this.labelTechnicalContact.Name = "labelTechnicalContact";
            this.labelTechnicalContact.Size = new System.Drawing.Size(101, 23);
            this.labelTechnicalContact.TabIndex = 0;
            this.labelTechnicalContact.Text = "Technical contacts:";
            this.labelTechnicalContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTechnicalContact1
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxTechnicalContact1, 4);
            this.textBoxTechnicalContact1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTechnicalContact1.Location = new System.Drawing.Point(110, 25);
            this.textBoxTechnicalContact1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxTechnicalContact1.Name = "textBoxTechnicalContact1";
            this.textBoxTechnicalContact1.Size = new System.Drawing.Size(599, 20);
            this.textBoxTechnicalContact1.TabIndex = 1;
            this.toolTip.SetToolTip(this.textBoxTechnicalContact1, resources.GetString("textBoxTechnicalContact1.ToolTip"));
            // 
            // labelCollection
            // 
            this.labelCollection.AutoSize = true;
            this.labelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollection.Location = new System.Drawing.Point(3, 261);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(101, 30);
            this.labelCollection.TabIndex = 5;
            this.labelCollection.Text = "Collection:";
            this.labelCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelContentContact
            // 
            this.labelContentContact.AutoSize = true;
            this.labelContentContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelContentContact.Location = new System.Drawing.Point(3, 68);
            this.labelContentContact.Name = "labelContentContact";
            this.labelContentContact.Size = new System.Drawing.Size(101, 23);
            this.labelContentContact.TabIndex = 2;
            this.labelContentContact.Text = "Content contacts:";
            this.labelContentContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxContentContact1
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxContentContact1, 4);
            this.textBoxContentContact1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxContentContact1.Location = new System.Drawing.Point(110, 71);
            this.textBoxContentContact1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxContentContact1.Name = "textBoxContentContact1";
            this.textBoxContentContact1.Size = new System.Drawing.Size(599, 20);
            this.textBoxContentContact1.TabIndex = 3;
            this.toolTip.SetToolTip(this.textBoxContentContact1, resources.GetString("textBoxContentContact1.ToolTip"));
            // 
            // textBoxTechnicalContact2
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxTechnicalContact2, 4);
            this.textBoxTechnicalContact2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTechnicalContact2.Location = new System.Drawing.Point(110, 45);
            this.textBoxTechnicalContact2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxTechnicalContact2.Name = "textBoxTechnicalContact2";
            this.textBoxTechnicalContact2.Size = new System.Drawing.Size(599, 20);
            this.textBoxTechnicalContact2.TabIndex = 7;
            this.toolTip.SetToolTip(this.textBoxTechnicalContact2, resources.GetString("textBoxTechnicalContact2.ToolTip"));
            // 
            // textBoxContentContact2
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxContentContact2, 4);
            this.textBoxContentContact2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxContentContact2.Location = new System.Drawing.Point(110, 91);
            this.textBoxContentContact2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxContentContact2.Name = "textBoxContentContact2";
            this.textBoxContentContact2.Size = new System.Drawing.Size(599, 20);
            this.textBoxContentContact2.TabIndex = 8;
            this.toolTip.SetToolTip(this.textBoxContentContact2, resources.GetString("textBoxContentContact2.ToolTip"));
            // 
            // labelOtherProviders
            // 
            this.labelOtherProviders.AutoSize = true;
            this.labelOtherProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOtherProviders.Location = new System.Drawing.Point(3, 114);
            this.labelOtherProviders.Name = "labelOtherProviders";
            this.labelOtherProviders.Size = new System.Drawing.Size(101, 26);
            this.labelOtherProviders.TabIndex = 6;
            this.labelOtherProviders.Text = "Other providers:";
            this.labelOtherProviders.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxOtherProviders
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxOtherProviders, 4);
            this.textBoxOtherProviders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOtherProviders.Location = new System.Drawing.Point(110, 117);
            this.textBoxOtherProviders.Name = "textBoxOtherProviders";
            this.textBoxOtherProviders.Size = new System.Drawing.Size(599, 20);
            this.textBoxOtherProviders.TabIndex = 9;
            this.toolTip.SetToolTip(this.textBoxOtherProviders, "Other known on-line providers of this dataset.");
            // 
            // labelIconURI
            // 
            this.labelIconURI.AutoSize = true;
            this.labelIconURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIconURI.Location = new System.Drawing.Point(3, 153);
            this.labelIconURI.Name = "labelIconURI";
            this.labelIconURI.Size = new System.Drawing.Size(101, 26);
            this.labelIconURI.TabIndex = 11;
            this.labelIconURI.Text = "Icon URI:";
            this.labelIconURI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIconURI
            // 
            this.textBoxIconURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxIconURI.Location = new System.Drawing.Point(110, 156);
            this.textBoxIconURI.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxIconURI.Name = "textBoxIconURI";
            this.textBoxIconURI.Size = new System.Drawing.Size(469, 20);
            this.textBoxIconURI.TabIndex = 12;
            this.toolTip.SetToolTip(this.textBoxIconURI, "The URI of an icon/logo symbolizing the project.");
            // 
            // labelMetadata
            // 
            this.labelMetadata.AutoSize = true;
            this.labelMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMetadata.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMetadata.Location = new System.Drawing.Point(3, 140);
            this.labelMetadata.Name = "labelMetadata";
            this.labelMetadata.Size = new System.Drawing.Size(101, 13);
            this.labelMetadata.TabIndex = 13;
            this.labelMetadata.Text = "Metadata";
            this.labelMetadata.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelScope
            // 
            this.labelScope.AutoSize = true;
            this.labelScope.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelScope.Location = new System.Drawing.Point(3, 179);
            this.labelScope.Name = "labelScope";
            this.labelScope.Size = new System.Drawing.Size(101, 27);
            this.labelScope.TabIndex = 14;
            this.labelScope.Text = "Scope:";
            this.labelScope.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonStart
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonStart, 2);
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonStart.Location = new System.Drawing.Point(609, 321);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 22);
            this.buttonStart.TabIndex = 4;
            this.buttonStart.Text = "Start export";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxScope
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxScope, 2);
            this.textBoxScope.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxScope.Location = new System.Drawing.Point(110, 182);
            this.textBoxScope.Name = "textBoxScope";
            this.textBoxScope.Size = new System.Drawing.Size(493, 20);
            this.textBoxScope.TabIndex = 15;
            this.toolTip.SetToolTip(this.textBoxScope, "A collection of taxon names of higher rank describing the taxonomic scope of the " +
        "source queried. Use space as separator.");
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVersion.Location = new System.Drawing.Point(3, 206);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(101, 26);
            this.labelVersion.TabIndex = 16;
            this.labelVersion.Text = "Version:";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxVersion
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxVersion, 2);
            this.textBoxVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVersion.Location = new System.Drawing.Point(110, 209);
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.Size = new System.Drawing.Size(493, 20);
            this.textBoxVersion.TabIndex = 17;
            this.toolTip.SetToolTip(this.textBoxVersion, "Number of current version (particularly for citing purposes)");
            // 
            // dateTimePickerDateIssued
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.dateTimePickerDateIssued, 2);
            this.dateTimePickerDateIssued.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerDateIssued.Location = new System.Drawing.Point(606, 209);
            this.dateTimePickerDateIssued.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerDateIssued.Name = "dateTimePickerDateIssued";
            this.dateTimePickerDateIssued.Size = new System.Drawing.Size(103, 20);
            this.dateTimePickerDateIssued.TabIndex = 18;
            this.toolTip.SetToolTip(this.dateTimePickerDateIssued, resources.GetString("dateTimePickerDateIssued.ToolTip"));
            // 
            // comboBoxCollection
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.comboBoxCollection, 3);
            this.comboBoxCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollection.FormattingEnabled = true;
            this.comboBoxCollection.Location = new System.Drawing.Point(110, 264);
            this.comboBoxCollection.Name = "comboBoxCollection";
            this.comboBoxCollection.Size = new System.Drawing.Size(572, 21);
            this.comboBoxCollection.TabIndex = 19;
            // 
            // labelExportFile
            // 
            this.labelExportFile.AutoSize = true;
            this.labelExportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExportFile.Location = new System.Drawing.Point(3, 291);
            this.labelExportFile.Name = "labelExportFile";
            this.labelExportFile.Size = new System.Drawing.Size(101, 27);
            this.labelExportFile.TabIndex = 20;
            this.labelExportFile.Text = "Export file:";
            this.labelExportFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExportFile
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxExportFile, 3);
            this.textBoxExportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExportFile.Location = new System.Drawing.Point(110, 294);
            this.textBoxExportFile.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxExportFile.Name = "textBoxExportFile";
            this.textBoxExportFile.ReadOnly = true;
            this.textBoxExportFile.Size = new System.Drawing.Size(575, 20);
            this.textBoxExportFile.TabIndex = 21;
            // 
            // labelDatasetGUID
            // 
            this.labelDatasetGUID.AutoSize = true;
            this.labelDatasetGUID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDatasetGUID.Location = new System.Drawing.Point(3, 232);
            this.labelDatasetGUID.Name = "labelDatasetGUID";
            this.labelDatasetGUID.Size = new System.Drawing.Size(101, 29);
            this.labelDatasetGUID.TabIndex = 22;
            this.labelDatasetGUID.Text = "Dataset GUID:";
            this.labelDatasetGUID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDatasetGUID
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxDatasetGUID, 2);
            this.textBoxDatasetGUID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDatasetGUID.Location = new System.Drawing.Point(110, 235);
            this.textBoxDatasetGUID.Name = "textBoxDatasetGUID";
            this.textBoxDatasetGUID.Size = new System.Drawing.Size(493, 20);
            this.textBoxDatasetGUID.TabIndex = 23;
            this.toolTip.SetToolTip(this.textBoxDatasetGUID, "A globally unique identifier for the entire data collection the present dataset w" +
        "as derived from. The exact format and/or semantics are still under discussion.");
            // 
            // buttonOpenIconURI
            // 
            this.buttonOpenIconURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOpenIconURI.Image = global::DiversityCollection.Resource.Browse;
            this.buttonOpenIconURI.Location = new System.Drawing.Point(579, 154);
            this.buttonOpenIconURI.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.buttonOpenIconURI.Name = "buttonOpenIconURI";
            this.buttonOpenIconURI.Size = new System.Drawing.Size(24, 24);
            this.buttonOpenIconURI.TabIndex = 24;
            this.buttonOpenIconURI.UseVisualStyleBackColor = true;
            this.buttonOpenIconURI.Click += new System.EventHandler(this.buttonOpenIconURI_Click);
            // 
            // buttonOpenExportFile
            // 
            this.buttonOpenExportFile.Image = global::DiversityCollection.Resource.Open;
            this.buttonOpenExportFile.Location = new System.Drawing.Point(685, 292);
            this.buttonOpenExportFile.Margin = new System.Windows.Forms.Padding(0, 1, 3, 3);
            this.buttonOpenExportFile.Name = "buttonOpenExportFile";
            this.buttonOpenExportFile.Size = new System.Drawing.Size(24, 23);
            this.buttonOpenExportFile.TabIndex = 25;
            this.buttonOpenExportFile.UseVisualStyleBackColor = true;
            this.buttonOpenExportFile.Click += new System.EventHandler(this.buttonOpenExportFile_Click);
            // 
            // panelIconURI
            // 
            this.panelIconURI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanelMain.SetColumnSpan(this.panelIconURI, 2);
            this.panelIconURI.Controls.Add(this.webBrowserIconURI);
            this.panelIconURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelIconURI.Location = new System.Drawing.Point(606, 140);
            this.panelIconURI.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.panelIconURI.Name = "panelIconURI";
            this.tableLayoutPanelMain.SetRowSpan(this.panelIconURI, 3);
            this.panelIconURI.Size = new System.Drawing.Size(103, 66);
            this.panelIconURI.TabIndex = 26;
            // 
            // webBrowserIconURI
            // 
            this.webBrowserIconURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserIconURI.Location = new System.Drawing.Point(0, 0);
            this.webBrowserIconURI.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserIconURI.Name = "webBrowserIconURI";
            this.webBrowserIconURI.ScrollBarsEnabled = false;
            this.webBrowserIconURI.Size = new System.Drawing.Size(99, 62);
            this.webBrowserIconURI.TabIndex = 10;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 4);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(3, 0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(679, 22);
            this.labelHeader.TabIndex = 27;
            this.labelHeader.Text = "Create an export file according to the schema ABCD 2.06";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlHierarchySelectorCollection
            // 
            this.userControlHierarchySelectorCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlHierarchySelectorCollection.Location = new System.Drawing.Point(688, 264);
            this.userControlHierarchySelectorCollection.Name = "userControlHierarchySelectorCollection";
            this.userControlHierarchySelectorCollection.Size = new System.Drawing.Size(21, 24);
            this.userControlHierarchySelectorCollection.TabIndex = 28;
            // 
            // buttonCreateGuid
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonCreateGuid, 2);
            this.buttonCreateGuid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateGuid.Location = new System.Drawing.Point(609, 235);
            this.buttonCreateGuid.Name = "buttonCreateGuid";
            this.buttonCreateGuid.Size = new System.Drawing.Size(100, 23);
            this.buttonCreateGuid.TabIndex = 29;
            this.buttonCreateGuid.Text = "create GUID";
            this.buttonCreateGuid.UseVisualStyleBackColor = true;
            this.buttonCreateGuid.Click += new System.EventHandler(this.buttonCreateGuid_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(685, 0);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(24, 22);
            this.buttonFeedback.TabIndex = 30;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send feedback");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // webBrowser
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.webBrowser, 5);
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(3, 349);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(706, 195);
            this.webBrowser.TabIndex = 31;
            // 
            // FormXmlExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 547);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.helpProvider.SetHelpKeyword(this, "ABCD");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Name = "FormXmlExport";
            this.helpProvider.SetShowHelp(this, true);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XML Export";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormXmlExport_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.panelIconURI.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelTechnicalContact;
        private System.Windows.Forms.TextBox textBoxTechnicalContact1;
        private System.Windows.Forms.Label labelContentContact;
        private System.Windows.Forms.TextBox textBoxContentContact1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelCollection;
        private System.Windows.Forms.Label labelOtherProviders;
        private System.Windows.Forms.TextBox textBoxTechnicalContact2;
        private System.Windows.Forms.TextBox textBoxContentContact2;
        private System.Windows.Forms.TextBox textBoxOtherProviders;
        private System.Windows.Forms.WebBrowser webBrowserIconURI;
        private System.Windows.Forms.Label labelIconURI;
        private System.Windows.Forms.TextBox textBoxIconURI;
        private System.Windows.Forms.Label labelMetadata;
        private System.Windows.Forms.Label labelScope;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox textBoxScope;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.DateTimePicker dateTimePickerDateIssued;
        private System.Windows.Forms.ComboBox comboBoxCollection;
        private System.Windows.Forms.Label labelExportFile;
        private System.Windows.Forms.TextBox textBoxExportFile;
        private System.Windows.Forms.Label labelDatasetGUID;
        private System.Windows.Forms.TextBox textBoxDatasetGUID;
        private System.Windows.Forms.Button buttonOpenIconURI;
        private System.Windows.Forms.Button buttonOpenExportFile;
        private System.Windows.Forms.Panel panelIconURI;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorCollection;
        private System.Windows.Forms.Button buttonCreateGuid;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.WebBrowser webBrowser;

    }
}