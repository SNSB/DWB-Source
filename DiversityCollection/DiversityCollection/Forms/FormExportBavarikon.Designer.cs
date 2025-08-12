namespace DiversityCollection.Forms
{
    partial class FormExportBavarikon
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportBavarikon));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelProject = new System.Windows.Forms.Label();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.buttonOpenDirectory = new System.Windows.Forms.Button();
            this.labelDatenpaket = new System.Windows.Forms.Label();
            this.textBoxDatenpaket = new System.Windows.Forms.TextBox();
            this.labelHauptverantwortlichkeit = new System.Windows.Forms.Label();
            this.textBoxHauptverantwortlichkeit = new System.Windows.Forms.TextBox();
            this.labelProjektNummer = new System.Windows.Forms.Label();
            this.labelSchlagworte = new System.Windows.Forms.Label();
            this.toolStripSchlagwort = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSchlagwortAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSchlagwortDelete = new System.Windows.Forms.ToolStripButton();
            this.listBoxSchlagwort = new System.Windows.Forms.ListBox();
            this.labelGND_ID = new System.Windows.Forms.Label();
            this.textBoxGND_ID = new System.Windows.Forms.TextBox();
            this.textBoxProjektNummer = new System.Windows.Forms.TextBox();
            this.labelMaterial = new System.Windows.Forms.Label();
            this.textBoxMaterial = new System.Windows.Forms.TextBox();
            this.labelKategorie = new System.Windows.Forms.Label();
            this.listBoxKategorie = new System.Windows.Forms.ListBox();
            this.toolStripKategorie = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonKategorieAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonKategorieDelete = new System.Windows.Forms.ToolStripButton();
            this.labelObjekt = new System.Windows.Forms.Label();
            this.textBoxObjekt = new System.Windows.Forms.TextBox();
            this.tabControlData = new System.Windows.Forms.TabControl();
            this.tabPageXML = new System.Windows.Forms.TabPage();
            this.tabPageTable = new System.Windows.Forms.TabPage();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.groupBoxDatum = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelDatum = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonDatumFund = new System.Windows.Forms.RadioButton();
            this.radioButtonDatumNotes = new System.Windows.Forms.RadioButton();
            this.radioButtonDatumFundUndNotes = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxSingleFile = new System.Windows.Forms.CheckBox();
            this.userControlWebView = new DiversityWorkbench.UserControls.UserControlWebView();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanelMain.SuspendLayout();
            this.toolStripSchlagwort.SuspendLayout();
            this.toolStripKategorie.SuspendLayout();
            this.tabControlData.SuspendLayout();
            this.tabPageXML.SuspendLayout();
            this.tabPageTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBoxDatum.SuspendLayout();
            this.tableLayoutPanelDatum.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.labelProject, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.comboBoxProject, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonExport, 0, 16);
            this.tableLayoutPanelMain.Controls.Add(this.buttonOpenDirectory, 1, 16);
            this.tableLayoutPanelMain.Controls.Add(this.labelDatenpaket, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxDatenpaket, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelHauptverantwortlichkeit, 0, 9);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxHauptverantwortlichkeit, 0, 10);
            this.tableLayoutPanelMain.Controls.Add(this.labelProjektNummer, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelSchlagworte, 0, 7);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripSchlagwort, 0, 8);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxSchlagwort, 1, 7);
            this.tableLayoutPanelMain.Controls.Add(this.labelGND_ID, 0, 11);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxGND_ID, 1, 11);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxProjektNummer, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelMaterial, 0, 12);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxMaterial, 0, 13);
            this.tableLayoutPanelMain.Controls.Add(this.labelKategorie, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxKategorie, 1, 5);
            this.tableLayoutPanelMain.Controls.Add(this.toolStripKategorie, 0, 6);
            this.tableLayoutPanelMain.Controls.Add(this.labelObjekt, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxObjekt, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.tabControlData, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxDatum, 0, 14);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxSingleFile, 0, 15);
            this.tableLayoutPanelMain.Controls.Add(this.progressBar, 0, 17);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 18;
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
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(800, 471);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelProject
            // 
            this.labelProject.AutoSize = true;
            this.labelProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProject.Location = new System.Drawing.Point(3, 0);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(64, 27);
            this.labelProject.TabIndex = 0;
            this.labelProject.Text = "Project:";
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxProject.FormattingEnabled = true;
            this.comboBoxProject.Location = new System.Drawing.Point(73, 3);
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(200, 21);
            this.comboBoxProject.TabIndex = 1;
            // 
            // buttonExport
            // 
            this.buttonExport.Image = global::DiversityCollection.Resource.Export;
            this.buttonExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonExport.Location = new System.Drawing.Point(3, 436);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(64, 23);
            this.buttonExport.TabIndex = 2;
            this.buttonExport.Text = "      Export";
            this.buttonExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // buttonOpenDirectory
            // 
            this.buttonOpenDirectory.Image = global::DiversityCollection.Resource.Open;
            this.buttonOpenDirectory.Location = new System.Drawing.Point(73, 436);
            this.buttonOpenDirectory.Name = "buttonOpenDirectory";
            this.buttonOpenDirectory.Size = new System.Drawing.Size(25, 23);
            this.buttonOpenDirectory.TabIndex = 3;
            this.buttonOpenDirectory.UseVisualStyleBackColor = true;
            this.buttonOpenDirectory.Click += new System.EventHandler(this.buttonOpenDirectory_Click);
            // 
            // labelDatenpaket
            // 
            this.labelDatenpaket.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelDatenpaket, 2);
            this.labelDatenpaket.Location = new System.Drawing.Point(3, 53);
            this.labelDatenpaket.Name = "labelDatenpaket";
            this.labelDatenpaket.Size = new System.Drawing.Size(63, 13);
            this.labelDatenpaket.TabIndex = 5;
            this.labelDatenpaket.Text = "Datenpaket";
            // 
            // textBoxDatenpaket
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxDatenpaket, 2);
            this.textBoxDatenpaket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDatenpaket.Location = new System.Drawing.Point(3, 69);
            this.textBoxDatenpaket.Name = "textBoxDatenpaket";
            this.textBoxDatenpaket.Size = new System.Drawing.Size(270, 20);
            this.textBoxDatenpaket.TabIndex = 6;
            this.textBoxDatenpaket.Text = "Sammlung Pilzaquarelle von Konrad Schieferdecker (1902-1965)";
            // 
            // labelHauptverantwortlichkeit
            // 
            this.labelHauptverantwortlichkeit.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHauptverantwortlichkeit, 2);
            this.labelHauptverantwortlichkeit.Location = new System.Drawing.Point(3, 252);
            this.labelHauptverantwortlichkeit.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelHauptverantwortlichkeit.Name = "labelHauptverantwortlichkeit";
            this.labelHauptverantwortlichkeit.Size = new System.Drawing.Size(119, 13);
            this.labelHauptverantwortlichkeit.TabIndex = 7;
            this.labelHauptverantwortlichkeit.Text = "Hauptverantwortlichkeit";
            // 
            // textBoxHauptverantwortlichkeit
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxHauptverantwortlichkeit, 2);
            this.textBoxHauptverantwortlichkeit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHauptverantwortlichkeit.Location = new System.Drawing.Point(3, 268);
            this.textBoxHauptverantwortlichkeit.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.textBoxHauptverantwortlichkeit.Name = "textBoxHauptverantwortlichkeit";
            this.textBoxHauptverantwortlichkeit.Size = new System.Drawing.Size(270, 20);
            this.textBoxHauptverantwortlichkeit.TabIndex = 8;
            this.textBoxHauptverantwortlichkeit.Text = "Konrad Schieferdecker";
            // 
            // labelProjektNummer
            // 
            this.labelProjektNummer.AutoSize = true;
            this.labelProjektNummer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProjektNummer.Location = new System.Drawing.Point(3, 27);
            this.labelProjektNummer.Name = "labelProjektNummer";
            this.labelProjektNummer.Size = new System.Drawing.Size(64, 26);
            this.labelProjektNummer.TabIndex = 9;
            this.labelProjektNummer.Text = "Projekt Nr.";
            this.labelProjektNummer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSchlagworte
            // 
            this.labelSchlagworte.AutoSize = true;
            this.labelSchlagworte.Location = new System.Drawing.Point(3, 185);
            this.labelSchlagworte.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelSchlagworte.Name = "labelSchlagworte";
            this.labelSchlagworte.Size = new System.Drawing.Size(60, 13);
            this.labelSchlagworte.TabIndex = 10;
            this.labelSchlagworte.Text = "Schlagwort";
            // 
            // toolStripSchlagwort
            // 
            this.toolStripSchlagwort.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripSchlagwort.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSchlagwort.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSchlagwortAdd,
            this.toolStripButtonSchlagwortDelete});
            this.toolStripSchlagwort.Location = new System.Drawing.Point(46, 198);
            this.toolStripSchlagwort.Name = "toolStripSchlagwort";
            this.toolStripSchlagwort.Size = new System.Drawing.Size(24, 48);
            this.toolStripSchlagwort.TabIndex = 11;
            this.toolStripSchlagwort.Text = "toolStrip1";
            // 
            // toolStripButtonSchlagwortAdd
            // 
            this.toolStripButtonSchlagwortAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSchlagwortAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonSchlagwortAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSchlagwortAdd.Name = "toolStripButtonSchlagwortAdd";
            this.toolStripButtonSchlagwortAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonSchlagwortAdd.Text = "Schlagwort hinzufügen";
            this.toolStripButtonSchlagwortAdd.Click += new System.EventHandler(this.toolStripButtonSchlagwortAdd_Click);
            // 
            // toolStripButtonSchlagwortDelete
            // 
            this.toolStripButtonSchlagwortDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSchlagwortDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonSchlagwortDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSchlagwortDelete.Name = "toolStripButtonSchlagwortDelete";
            this.toolStripButtonSchlagwortDelete.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonSchlagwortDelete.Text = "Schlagwort löschen";
            this.toolStripButtonSchlagwortDelete.Click += new System.EventHandler(this.toolStripButtonSchlagwortDelete_Click);
            // 
            // listBoxSchlagwort
            // 
            this.listBoxSchlagwort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSchlagwort.FormattingEnabled = true;
            this.listBoxSchlagwort.IntegralHeight = false;
            this.listBoxSchlagwort.Location = new System.Drawing.Point(73, 182);
            this.listBoxSchlagwort.Name = "listBoxSchlagwort";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxSchlagwort, 2);
            this.listBoxSchlagwort.Size = new System.Drawing.Size(200, 61);
            this.listBoxSchlagwort.TabIndex = 12;
            // 
            // labelGND_ID
            // 
            this.labelGND_ID.AutoSize = true;
            this.labelGND_ID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGND_ID.Location = new System.Drawing.Point(3, 289);
            this.labelGND_ID.Name = "labelGND_ID";
            this.labelGND_ID.Size = new System.Drawing.Size(64, 23);
            this.labelGND_ID.TabIndex = 13;
            this.labelGND_ID.Text = "GND-ID:";
            this.labelGND_ID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxGND_ID
            // 
            this.textBoxGND_ID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxGND_ID.Location = new System.Drawing.Point(73, 289);
            this.textBoxGND_ID.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxGND_ID.Name = "textBoxGND_ID";
            this.textBoxGND_ID.Size = new System.Drawing.Size(200, 20);
            this.textBoxGND_ID.TabIndex = 14;
            this.textBoxGND_ID.Text = "1029298394";
            // 
            // textBoxProjektNummer
            // 
            this.textBoxProjektNummer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProjektNummer.Location = new System.Drawing.Point(73, 30);
            this.textBoxProjektNummer.Name = "textBoxProjektNummer";
            this.textBoxProjektNummer.Size = new System.Drawing.Size(200, 20);
            this.textBoxProjektNummer.TabIndex = 15;
            this.textBoxProjektNummer.Text = "0219";
            // 
            // labelMaterial
            // 
            this.labelMaterial.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelMaterial, 2);
            this.labelMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterial.Location = new System.Drawing.Point(3, 318);
            this.labelMaterial.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelMaterial.Name = "labelMaterial";
            this.labelMaterial.Size = new System.Drawing.Size(270, 13);
            this.labelMaterial.TabIndex = 16;
            this.labelMaterial.Text = "Material / Technik";
            // 
            // textBoxMaterial
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxMaterial, 2);
            this.textBoxMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMaterial.Location = new System.Drawing.Point(3, 334);
            this.textBoxMaterial.Name = "textBoxMaterial";
            this.textBoxMaterial.Size = new System.Drawing.Size(270, 20);
            this.textBoxMaterial.TabIndex = 17;
            this.textBoxMaterial.Text = "Aquarelltechnik auf Papier";
            // 
            // labelKategorie
            // 
            this.labelKategorie.AutoSize = true;
            this.labelKategorie.Location = new System.Drawing.Point(3, 118);
            this.labelKategorie.Name = "labelKategorie";
            this.labelKategorie.Size = new System.Drawing.Size(52, 13);
            this.labelKategorie.TabIndex = 18;
            this.labelKategorie.Text = "Kategorie";
            // 
            // listBoxKategorie
            // 
            this.listBoxKategorie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxKategorie.FormattingEnabled = true;
            this.listBoxKategorie.IntegralHeight = false;
            this.listBoxKategorie.Location = new System.Drawing.Point(73, 121);
            this.listBoxKategorie.Name = "listBoxKategorie";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxKategorie, 2);
            this.listBoxKategorie.Size = new System.Drawing.Size(200, 55);
            this.listBoxKategorie.TabIndex = 19;
            // 
            // toolStripKategorie
            // 
            this.toolStripKategorie.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripKategorie.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripKategorie.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonKategorieAdd,
            this.toolStripButtonKategorieDelete});
            this.toolStripKategorie.Location = new System.Drawing.Point(46, 131);
            this.toolStripKategorie.Name = "toolStripKategorie";
            this.toolStripKategorie.Size = new System.Drawing.Size(24, 48);
            this.toolStripKategorie.TabIndex = 20;
            this.toolStripKategorie.Text = "toolStrip1";
            // 
            // toolStripButtonKategorieAdd
            // 
            this.toolStripButtonKategorieAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonKategorieAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonKategorieAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonKategorieAdd.Name = "toolStripButtonKategorieAdd";
            this.toolStripButtonKategorieAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonKategorieAdd.Text = "Kategorie hinzufügen";
            this.toolStripButtonKategorieAdd.Click += new System.EventHandler(this.toolStripButtonKategorieAdd_Click);
            // 
            // toolStripButtonKategorieDelete
            // 
            this.toolStripButtonKategorieDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonKategorieDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonKategorieDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonKategorieDelete.Name = "toolStripButtonKategorieDelete";
            this.toolStripButtonKategorieDelete.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonKategorieDelete.Text = "Kategorie löschen";
            this.toolStripButtonKategorieDelete.Click += new System.EventHandler(this.toolStripButtonKategorieDelete_Click);
            // 
            // labelObjekt
            // 
            this.labelObjekt.AutoSize = true;
            this.labelObjekt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelObjekt.Location = new System.Drawing.Point(3, 92);
            this.labelObjekt.Name = "labelObjekt";
            this.labelObjekt.Size = new System.Drawing.Size(64, 26);
            this.labelObjekt.TabIndex = 21;
            this.labelObjekt.Text = "Objekt:";
            this.labelObjekt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxObjekt
            // 
            this.textBoxObjekt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxObjekt.Location = new System.Drawing.Point(73, 95);
            this.textBoxObjekt.Name = "textBoxObjekt";
            this.textBoxObjekt.Size = new System.Drawing.Size(200, 20);
            this.textBoxObjekt.TabIndex = 22;
            this.textBoxObjekt.Text = "Aquarell";
            // 
            // tabControlData
            // 
            this.tabControlData.Controls.Add(this.tabPageXML);
            this.tabControlData.Controls.Add(this.tabPageTable);
            this.tabControlData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlData.Location = new System.Drawing.Point(279, 3);
            this.tabControlData.Name = "tabControlData";
            this.tableLayoutPanelMain.SetRowSpan(this.tabControlData, 18);
            this.tabControlData.SelectedIndex = 0;
            this.tabControlData.Size = new System.Drawing.Size(518, 465);
            this.tabControlData.TabIndex = 23;
            // 
            // tabPageXML
            // 
            this.tabPageXML.Controls.Add(this.userControlWebView);
            this.tabPageXML.Location = new System.Drawing.Point(4, 22);
            this.tabPageXML.Name = "tabPageXML";
            this.tabPageXML.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageXML.Size = new System.Drawing.Size(510, 439);
            this.tabPageXML.TabIndex = 0;
            this.tabPageXML.Text = "XML";
            this.tabPageXML.UseVisualStyleBackColor = true;
            // 
            // tabPageTable
            // 
            this.tabPageTable.Controls.Add(this.dataGridView);
            this.tabPageTable.Location = new System.Drawing.Point(4, 22);
            this.tabPageTable.Name = "tabPageTable";
            this.tabPageTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTable.Size = new System.Drawing.Size(510, 439);
            this.tabPageTable.TabIndex = 1;
            this.tabPageTable.Text = "Table";
            this.tabPageTable.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.Size = new System.Drawing.Size(504, 433);
            this.dataGridView.TabIndex = 0;
            // 
            // groupBoxDatum
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.groupBoxDatum, 2);
            this.groupBoxDatum.Controls.Add(this.tableLayoutPanelDatum);
            this.groupBoxDatum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDatum.Location = new System.Drawing.Point(3, 360);
            this.groupBoxDatum.Name = "groupBoxDatum";
            this.groupBoxDatum.Size = new System.Drawing.Size(270, 40);
            this.groupBoxDatum.TabIndex = 24;
            this.groupBoxDatum.TabStop = false;
            this.groupBoxDatum.Text = "Quellen zur Ermittlung des Erstellungsdatums";
            // 
            // tableLayoutPanelDatum
            // 
            this.tableLayoutPanelDatum.ColumnCount = 3;
            this.tableLayoutPanelDatum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDatum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDatum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDatum.Controls.Add(this.radioButtonDatumFund, 0, 0);
            this.tableLayoutPanelDatum.Controls.Add(this.radioButtonDatumNotes, 1, 0);
            this.tableLayoutPanelDatum.Controls.Add(this.radioButtonDatumFundUndNotes, 2, 0);
            this.tableLayoutPanelDatum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDatum.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelDatum.Name = "tableLayoutPanelDatum";
            this.tableLayoutPanelDatum.RowCount = 1;
            this.tableLayoutPanelDatum.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDatum.Size = new System.Drawing.Size(264, 21);
            this.tableLayoutPanelDatum.TabIndex = 0;
            // 
            // radioButtonDatumFund
            // 
            this.radioButtonDatumFund.AutoSize = true;
            this.radioButtonDatumFund.Location = new System.Drawing.Point(3, 3);
            this.radioButtonDatumFund.Name = "radioButtonDatumFund";
            this.radioButtonDatumFund.Size = new System.Drawing.Size(49, 15);
            this.radioButtonDatumFund.TabIndex = 0;
            this.radioButtonDatumFund.Text = "Fund";
            this.radioButtonDatumFund.UseVisualStyleBackColor = true;
            // 
            // radioButtonDatumNotes
            // 
            this.radioButtonDatumNotes.AutoSize = true;
            this.radioButtonDatumNotes.Location = new System.Drawing.Point(58, 3);
            this.radioButtonDatumNotes.Name = "radioButtonDatumNotes";
            this.radioButtonDatumNotes.Size = new System.Drawing.Size(53, 15);
            this.radioButtonDatumNotes.TabIndex = 1;
            this.radioButtonDatumNotes.Text = "Notes";
            this.radioButtonDatumNotes.UseVisualStyleBackColor = true;
            // 
            // radioButtonDatumFundUndNotes
            // 
            this.radioButtonDatumFundUndNotes.AutoSize = true;
            this.radioButtonDatumFundUndNotes.Checked = true;
            this.radioButtonDatumFundUndNotes.Location = new System.Drawing.Point(117, 3);
            this.radioButtonDatumFundUndNotes.Name = "radioButtonDatumFundUndNotes";
            this.radioButtonDatumFundUndNotes.Size = new System.Drawing.Size(42, 15);
            this.radioButtonDatumFundUndNotes.TabIndex = 2;
            this.radioButtonDatumFundUndNotes.TabStop = true;
            this.radioButtonDatumFundUndNotes.Text = "Alle";
            this.radioButtonDatumFundUndNotes.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // checkBoxSingleFile
            // 
            this.checkBoxSingleFile.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.checkBoxSingleFile, 2);
            this.checkBoxSingleFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxSingleFile.Location = new System.Drawing.Point(3, 416);
            this.checkBoxSingleFile.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.checkBoxSingleFile.Name = "checkBoxSingleFile";
            this.checkBoxSingleFile.Size = new System.Drawing.Size(270, 17);
            this.checkBoxSingleFile.TabIndex = 25;
            this.checkBoxSingleFile.Text = "Single file";
            this.checkBoxSingleFile.UseVisualStyleBackColor = true;
            // 
            // userControlWebView
            // 
            this.userControlWebView.AllowScripting = false;
            this.userControlWebView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWebView.Location = new System.Drawing.Point(3, 3);
            this.userControlWebView.Margin = new System.Windows.Forms.Padding(0);
            this.userControlWebView.Name = "userControlWebView";
            this.userControlWebView.ScriptErrorsSuppressed = false;
            this.userControlWebView.Size = new System.Drawing.Size(504, 433);
            this.userControlWebView.TabIndex = 4;
            this.userControlWebView.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            // 
            // progressBar
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.progressBar, 2);
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(3, 462);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(270, 6);
            this.progressBar.TabIndex = 26;
            // 
            // FormExportBavarikon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 471);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormExportBavarikon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Bavarikon";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStripSchlagwort.ResumeLayout(false);
            this.toolStripSchlagwort.PerformLayout();
            this.toolStripKategorie.ResumeLayout(false);
            this.toolStripKategorie.PerformLayout();
            this.tabControlData.ResumeLayout(false);
            this.tabPageXML.ResumeLayout(false);
            this.tabPageTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBoxDatum.ResumeLayout(false);
            this.tableLayoutPanelDatum.ResumeLayout(false);
            this.tableLayoutPanelDatum.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonOpenDirectory;
        private DiversityWorkbench.UserControls.UserControlWebView userControlWebView;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label labelDatenpaket;
        private System.Windows.Forms.TextBox textBoxDatenpaket;
        private System.Windows.Forms.Label labelHauptverantwortlichkeit;
        private System.Windows.Forms.TextBox textBoxHauptverantwortlichkeit;
        private System.Windows.Forms.Label labelProjektNummer;
        private System.Windows.Forms.Label labelSchlagworte;
        private System.Windows.Forms.ToolStrip toolStripSchlagwort;
        private System.Windows.Forms.ToolStripButton toolStripButtonSchlagwortAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonSchlagwortDelete;
        private System.Windows.Forms.ListBox listBoxSchlagwort;
        private System.Windows.Forms.Label labelGND_ID;
        private System.Windows.Forms.TextBox textBoxGND_ID;
        private System.Windows.Forms.TextBox textBoxProjektNummer;
        private System.Windows.Forms.Label labelMaterial;
        private System.Windows.Forms.TextBox textBoxMaterial;
        private System.Windows.Forms.Label labelKategorie;
        private System.Windows.Forms.ListBox listBoxKategorie;
        private System.Windows.Forms.ToolStrip toolStripKategorie;
        private System.Windows.Forms.ToolStripButton toolStripButtonKategorieAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonKategorieDelete;
        private System.Windows.Forms.Label labelObjekt;
        private System.Windows.Forms.TextBox textBoxObjekt;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabControl tabControlData;
        private System.Windows.Forms.TabPage tabPageXML;
        private System.Windows.Forms.TabPage tabPageTable;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.GroupBox groupBoxDatum;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDatum;
        private System.Windows.Forms.RadioButton radioButtonDatumFund;
        private System.Windows.Forms.RadioButton radioButtonDatumNotes;
        private System.Windows.Forms.RadioButton radioButtonDatumFundUndNotes;
        private System.Windows.Forms.CheckBox checkBoxSingleFile;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}