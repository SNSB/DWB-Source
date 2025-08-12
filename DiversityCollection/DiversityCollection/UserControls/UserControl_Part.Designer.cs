namespace DiversityCollection.UserControls
{
    partial class UserControl_Part
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Part));
            this.groupBoxPart = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelStorage = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxSpecimenPartID = new System.Windows.Forms.TextBox();
            this.textBoxStorageNotes = new System.Windows.Forms.TextBox();
            this.labelStorageNotes = new System.Windows.Forms.Label();
            this.comboBoxMaterialCategory = new System.Windows.Forms.ComboBox();
            this.pictureBoxSpecimenPart = new System.Windows.Forms.PictureBox();
            this.labelMaterialCategory = new System.Windows.Forms.Label();
            this.labelStorageLocation = new System.Windows.Forms.Label();
            this.labelStock = new System.Windows.Forms.Label();
            this.textBoxStock = new System.Windows.Forms.TextBox();
            this.comboBoxCollection = new System.Windows.Forms.ComboBox();
            this.labelCollection = new System.Windows.Forms.Label();
            this.comboBoxStorageLocation = new System.Windows.Forms.ComboBox();
            this.textBoxStorageLocation = new System.Windows.Forms.TextBox();
            this.comboBoxStorageNotes = new System.Windows.Forms.ComboBox();
            this.labelAccessionNumberPart = new System.Windows.Forms.Label();
            this.textBoxAccessionNumberPart = new System.Windows.Forms.TextBox();
            this.labelPartSublabel = new System.Windows.Forms.Label();
            this.textBoxPartSublabel = new System.Windows.Forms.TextBox();
            this.labelPreparationMethod = new System.Windows.Forms.Label();
            this.comboBoxPreparationMethod = new System.Windows.Forms.ComboBox();
            this.textBoxPreparationMethod = new System.Windows.Forms.TextBox();
            this.labelPreparationDate = new System.Windows.Forms.Label();
            this.dateTimePickerPreparationDate = new System.Windows.Forms.DateTimePicker();
            this.userControlHierarchySelectorCollection = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            this.buttonRestrictImagesToCurrrentPart = new System.Windows.Forms.Button();
            this.labelStorageContainer = new System.Windows.Forms.Label();
            this.comboBoxStorageContainer = new System.Windows.Forms.ComboBox();
            this.labelStockUnit = new System.Windows.Forms.Label();
            this.comboBoxStockUnit = new System.Windows.Forms.ComboBox();
            this.labelPreparationResponsible = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryPreparationResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.textBoxPartWithhold = new System.Windows.Forms.TextBox();
            this.buttonStockHistoryInfo = new System.Windows.Forms.Button();
            this.buttonFindNextAccessionNumberPart = new System.Windows.Forms.Button();
            this.pictureBoxWithholdPart = new System.Windows.Forms.PictureBox();
            this.buttonTemplatePartSet = new System.Windows.Forms.Button();
            this.buttonTemplatePartEdit = new System.Windows.Forms.Button();
            this.buttonSetStorageLocationSource = new System.Windows.Forms.Button();
            this.buttonSetMaterialCategoryRange = new System.Windows.Forms.Button();
            this.labelSpecimenPartID = new System.Windows.Forms.Label();
            this.buttonPreparationDateDelete = new System.Windows.Forms.Button();
            this.tableLayoutPanelIdentificationUnitPart = new System.Windows.Forms.TableLayoutPanel();
            this.labelIdentificationUnitInPartDescription = new System.Windows.Forms.Label();
            this.comboBoxIdentificationUnitInPartDescription = new System.Windows.Forms.ComboBox();
            this.pictureBoxIdentificationUnitInPartDescription = new System.Windows.Forms.PictureBox();
            this.groupBoxPart.SuspendLayout();
            this.tableLayoutPanelStorage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimenPart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWithholdPart)).BeginInit();
            this.tableLayoutPanelIdentificationUnitPart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIdentificationUnitInPartDescription)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxPart
            // 
            this.groupBoxPart.AccessibleName = "CollectionSpecimenPart";
            this.groupBoxPart.Controls.Add(this.tableLayoutPanelStorage);
            this.groupBoxPart.Controls.Add(this.tableLayoutPanelIdentificationUnitPart);
            this.groupBoxPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxPart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPart.ForeColor = System.Drawing.Color.Black;
            this.groupBoxPart.Location = new System.Drawing.Point(0, 0);
            this.groupBoxPart.MinimumSize = new System.Drawing.Size(0, 160);
            this.groupBoxPart.Name = "groupBoxPart";
            this.groupBoxPart.Padding = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.groupBoxPart.Size = new System.Drawing.Size(679, 312);
            this.groupBoxPart.TabIndex = 33;
            this.groupBoxPart.TabStop = false;
            this.groupBoxPart.Text = "Specimen part";
            // 
            // tableLayoutPanelStorage
            // 
            this.tableLayoutPanelStorage.ColumnCount = 13;
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxSpecimenPartID, 10, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxStorageNotes, 3, 5);
            this.tableLayoutPanelStorage.Controls.Add(this.labelStorageNotes, 1, 5);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxMaterialCategory, 2, 4);
            this.tableLayoutPanelStorage.Controls.Add(this.pictureBoxSpecimenPart, 0, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.labelMaterialCategory, 1, 4);
            this.tableLayoutPanelStorage.Controls.Add(this.labelStorageLocation, 1, 3);
            this.tableLayoutPanelStorage.Controls.Add(this.labelStock, 5, 4);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxStock, 6, 4);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxCollection, 2, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.labelCollection, 1, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxStorageLocation, 2, 3);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxStorageLocation, 3, 3);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxStorageNotes, 2, 5);
            this.tableLayoutPanelStorage.Controls.Add(this.labelAccessionNumberPart, 1, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxAccessionNumberPart, 2, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.labelPartSublabel, 5, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxPartSublabel, 6, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.labelPreparationMethod, 1, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxPreparationMethod, 2, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxPreparationMethod, 3, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.labelPreparationDate, 5, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.dateTimePickerPreparationDate, 6, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.userControlHierarchySelectorCollection, 4, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.buttonRestrictImagesToCurrrentPart, 12, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.labelStorageContainer, 6, 3);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxStorageContainer, 7, 3);
            this.tableLayoutPanelStorage.Controls.Add(this.labelStockUnit, 9, 4);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxStockUnit, 11, 4);
            this.tableLayoutPanelStorage.Controls.Add(this.labelPreparationResponsible, 5, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.userControlModuleRelatedEntryPreparationResponsible, 6, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxPartWithhold, 9, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.buttonStockHistoryInfo, 8, 4);
            this.tableLayoutPanelStorage.Controls.Add(this.buttonFindNextAccessionNumberPart, 4, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.pictureBoxWithholdPart, 8, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.buttonTemplatePartSet, 0, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.buttonTemplatePartEdit, 0, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.buttonSetStorageLocationSource, 0, 3);
            this.tableLayoutPanelStorage.Controls.Add(this.buttonSetMaterialCategoryRange, 0, 4);
            this.tableLayoutPanelStorage.Controls.Add(this.labelSpecimenPartID, 9, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.buttonPreparationDateDelete, 8, 1);
            this.tableLayoutPanelStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelStorage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelStorage.Location = new System.Drawing.Point(3, 18);
            this.tableLayoutPanelStorage.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelStorage.Name = "tableLayoutPanelStorage";
            this.tableLayoutPanelStorage.RowCount = 7;
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.Size = new System.Drawing.Size(673, 265);
            this.tableLayoutPanelStorage.TabIndex = 1;
            // 
            // textBoxSpecimenPartID
            // 
            this.textBoxSpecimenPartID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelStorage.SetColumnSpan(this.textBoxSpecimenPartID, 3);
            this.textBoxSpecimenPartID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSpecimenPartID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSpecimenPartID.Location = new System.Drawing.Point(613, 27);
            this.textBoxSpecimenPartID.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.textBoxSpecimenPartID.Name = "textBoxSpecimenPartID";
            this.textBoxSpecimenPartID.ReadOnly = true;
            this.textBoxSpecimenPartID.Size = new System.Drawing.Size(60, 13);
            this.textBoxSpecimenPartID.TabIndex = 5;
            this.textBoxSpecimenPartID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxSpecimenPartID.ReadOnlyChanged += new System.EventHandler(this.textBoxSpecimenPartID_ReadOnlyChanged);
            // 
            // textBoxStorageNotes
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.textBoxStorageNotes, 10);
            this.textBoxStorageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStorageNotes.Location = new System.Drawing.Point(99, 121);
            this.textBoxStorageNotes.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxStorageNotes.Multiline = true;
            this.textBoxStorageNotes.Name = "textBoxStorageNotes";
            this.textBoxStorageNotes.Size = new System.Drawing.Size(574, 144);
            this.textBoxStorageNotes.TabIndex = 19;
            this.textBoxStorageNotes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxStorageNotes_KeyDown);
            this.textBoxStorageNotes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxStorageNotes_KeyPress);
            this.textBoxStorageNotes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxStorageNotes_KeyUp);
            // 
            // labelStorageNotes
            // 
            this.labelStorageNotes.AccessibleName = "CollectionSpecimenPart.Notes";
            this.labelStorageNotes.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStorageNotes.Location = new System.Drawing.Point(18, 124);
            this.labelStorageNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelStorageNotes.Name = "labelStorageNotes";
            this.labelStorageNotes.Size = new System.Drawing.Size(62, 141);
            this.labelStorageNotes.TabIndex = 9;
            this.labelStorageNotes.Text = "Notes:";
            this.labelStorageNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxMaterialCategory
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.comboBoxMaterialCategory, 3);
            this.comboBoxMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxMaterialCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMaterialCategory.Location = new System.Drawing.Point(80, 97);
            this.comboBoxMaterialCategory.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.comboBoxMaterialCategory.Name = "comboBoxMaterialCategory";
            this.comboBoxMaterialCategory.Size = new System.Drawing.Size(376, 21);
            this.comboBoxMaterialCategory.TabIndex = 17;
            // 
            // pictureBoxSpecimenPart
            // 
            this.pictureBoxSpecimenPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSpecimenPart.Image = global::DiversityCollection.Resource.Specimen;
            this.pictureBoxSpecimenPart.Location = new System.Drawing.Point(0, 3);
            this.pictureBoxSpecimenPart.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBoxSpecimenPart.Name = "pictureBoxSpecimenPart";
            this.pictureBoxSpecimenPart.Size = new System.Drawing.Size(15, 20);
            this.pictureBoxSpecimenPart.TabIndex = 2;
            this.pictureBoxSpecimenPart.TabStop = false;
            // 
            // labelMaterialCategory
            // 
            this.labelMaterialCategory.AccessibleName = "CollectionSpecimenPart.MaterialCategory";
            this.labelMaterialCategory.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelMaterialCategory.Location = new System.Drawing.Point(18, 97);
            this.labelMaterialCategory.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelMaterialCategory.Name = "labelMaterialCategory";
            this.labelMaterialCategory.Size = new System.Drawing.Size(62, 24);
            this.labelMaterialCategory.TabIndex = 6;
            this.labelMaterialCategory.Text = "Mat. cat..:";
            this.labelMaterialCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelStorageLocation
            // 
            this.labelStorageLocation.AccessibleName = "CollectionSpecimenPart.StorageLocation";
            this.labelStorageLocation.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStorageLocation.Location = new System.Drawing.Point(18, 73);
            this.labelStorageLocation.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelStorageLocation.Name = "labelStorageLocation";
            this.labelStorageLocation.Size = new System.Drawing.Size(62, 24);
            this.labelStorageLocation.TabIndex = 3;
            this.labelStorageLocation.Text = "Stor. loc.:";
            this.labelStorageLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelStock
            // 
            this.labelStock.AccessibleName = "CollectionSpecimenPart.Stock";
            this.labelStock.AutoSize = true;
            this.labelStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStock.Location = new System.Drawing.Point(462, 97);
            this.labelStock.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelStock.Name = "labelStock";
            this.labelStock.Size = new System.Drawing.Size(38, 24);
            this.labelStock.TabIndex = 13;
            this.labelStock.Text = "Stock:";
            this.labelStock.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxStock
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.textBoxStock, 2);
            this.textBoxStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStock.Location = new System.Drawing.Point(500, 97);
            this.textBoxStock.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxStock.Name = "textBoxStock";
            this.textBoxStock.Size = new System.Drawing.Size(74, 20);
            this.textBoxStock.TabIndex = 18;
            this.textBoxStock.TextChanged += new System.EventHandler(this.textBoxStock_TextChanged);
            // 
            // comboBoxCollection
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.comboBoxCollection, 2);
            this.comboBoxCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCollection.FormattingEnabled = true;
            this.comboBoxCollection.Location = new System.Drawing.Point(80, 23);
            this.comboBoxCollection.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxCollection.Name = "comboBoxCollection";
            this.comboBoxCollection.Size = new System.Drawing.Size(361, 21);
            this.comboBoxCollection.TabIndex = 15;
            // 
            // labelCollection
            // 
            this.labelCollection.AccessibleName = "CollectionSpecimenPart.CollectionID";
            this.labelCollection.AutoSize = true;
            this.labelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollection.Location = new System.Drawing.Point(18, 23);
            this.labelCollection.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(62, 26);
            this.labelCollection.TabIndex = 16;
            this.labelCollection.Text = "Collection:";
            this.labelCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxStorageLocation
            // 
            this.comboBoxStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStorageLocation.DropDownWidth = 300;
            this.comboBoxStorageLocation.FormattingEnabled = true;
            this.comboBoxStorageLocation.Location = new System.Drawing.Point(80, 73);
            this.comboBoxStorageLocation.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxStorageLocation.Name = "comboBoxStorageLocation";
            this.comboBoxStorageLocation.Size = new System.Drawing.Size(19, 21);
            this.comboBoxStorageLocation.TabIndex = 18;
            this.comboBoxStorageLocation.DropDown += new System.EventHandler(this.comboBoxStorageLocation_DropDown);
            this.comboBoxStorageLocation.SelectionChangeCommitted += new System.EventHandler(this.comboBoxStorageLocation_SelectionChangeCommitted);
            // 
            // textBoxStorageLocation
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.textBoxStorageLocation, 3);
            this.textBoxStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStorageLocation.Location = new System.Drawing.Point(99, 73);
            this.textBoxStorageLocation.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxStorageLocation.Name = "textBoxStorageLocation";
            this.textBoxStorageLocation.Size = new System.Drawing.Size(398, 20);
            this.textBoxStorageLocation.TabIndex = 16;
            // 
            // comboBoxStorageNotes
            // 
            this.comboBoxStorageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStorageNotes.FormattingEnabled = true;
            this.comboBoxStorageNotes.Location = new System.Drawing.Point(80, 121);
            this.comboBoxStorageNotes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxStorageNotes.Name = "comboBoxStorageNotes";
            this.comboBoxStorageNotes.Size = new System.Drawing.Size(19, 21);
            this.comboBoxStorageNotes.TabIndex = 21;
            this.comboBoxStorageNotes.DropDown += new System.EventHandler(this.comboBoxStorageNotes_DropDown);
            this.comboBoxStorageNotes.SelectionChangeCommitted += new System.EventHandler(this.comboBoxStorageNotes_SelectionChangeCommitted);
            // 
            // labelAccessionNumberPart
            // 
            this.labelAccessionNumberPart.AccessibleName = "CollectionSpecimenPart.AccessionNumber";
            this.labelAccessionNumberPart.AutoSize = true;
            this.labelAccessionNumberPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAccessionNumberPart.Location = new System.Drawing.Point(18, 0);
            this.labelAccessionNumberPart.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAccessionNumberPart.Name = "labelAccessionNumberPart";
            this.labelAccessionNumberPart.Size = new System.Drawing.Size(62, 23);
            this.labelAccessionNumberPart.TabIndex = 22;
            this.labelAccessionNumberPart.Text = "Acc.No.:";
            this.labelAccessionNumberPart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxAccessionNumberPart
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.textBoxAccessionNumberPart, 2);
            this.textBoxAccessionNumberPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAccessionNumberPart.Location = new System.Drawing.Point(80, 0);
            this.textBoxAccessionNumberPart.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxAccessionNumberPart.Name = "textBoxAccessionNumberPart";
            this.textBoxAccessionNumberPart.Size = new System.Drawing.Size(361, 20);
            this.textBoxAccessionNumberPart.TabIndex = 23;
            // 
            // labelPartSublabel
            // 
            this.labelPartSublabel.AccessibleName = "CollectionSpecimenPart.PartSublabel";
            this.labelPartSublabel.AutoSize = true;
            this.labelPartSublabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPartSublabel.Location = new System.Drawing.Point(462, 0);
            this.labelPartSublabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelPartSublabel.Name = "labelPartSublabel";
            this.labelPartSublabel.Size = new System.Drawing.Size(38, 23);
            this.labelPartSublabel.TabIndex = 24;
            this.labelPartSublabel.Text = "Part:";
            this.labelPartSublabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPartSublabel
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.textBoxPartSublabel, 2);
            this.textBoxPartSublabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPartSublabel.Location = new System.Drawing.Point(500, 0);
            this.textBoxPartSublabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxPartSublabel.Name = "textBoxPartSublabel";
            this.textBoxPartSublabel.Size = new System.Drawing.Size(71, 20);
            this.textBoxPartSublabel.TabIndex = 25;
            // 
            // labelPreparationMethod
            // 
            this.labelPreparationMethod.AccessibleName = "CollectionSpecimenPart.PreparationMethod";
            this.labelPreparationMethod.AutoSize = true;
            this.labelPreparationMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPreparationMethod.Location = new System.Drawing.Point(18, 49);
            this.labelPreparationMethod.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelPreparationMethod.Name = "labelPreparationMethod";
            this.labelPreparationMethod.Size = new System.Drawing.Size(62, 24);
            this.labelPreparationMethod.TabIndex = 26;
            this.labelPreparationMethod.Text = "Preparat.:";
            this.labelPreparationMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxPreparationMethod
            // 
            this.comboBoxPreparationMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxPreparationMethod.DropDownWidth = 300;
            this.comboBoxPreparationMethod.FormattingEnabled = true;
            this.comboBoxPreparationMethod.Location = new System.Drawing.Point(80, 49);
            this.comboBoxPreparationMethod.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxPreparationMethod.Name = "comboBoxPreparationMethod";
            this.comboBoxPreparationMethod.Size = new System.Drawing.Size(19, 21);
            this.comboBoxPreparationMethod.TabIndex = 27;
            this.comboBoxPreparationMethod.DropDown += new System.EventHandler(this.comboBoxPreparationMethod_DropDown);
            this.comboBoxPreparationMethod.SelectionChangeCommitted += new System.EventHandler(this.comboBoxPreparationMethod_SelectionChangeCommitted);
            // 
            // textBoxPreparationMethod
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.textBoxPreparationMethod, 2);
            this.textBoxPreparationMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPreparationMethod.Location = new System.Drawing.Point(99, 49);
            this.textBoxPreparationMethod.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxPreparationMethod.Name = "textBoxPreparationMethod";
            this.textBoxPreparationMethod.Size = new System.Drawing.Size(357, 20);
            this.textBoxPreparationMethod.TabIndex = 28;
            // 
            // labelPreparationDate
            // 
            this.labelPreparationDate.AccessibleName = "CollectionSpecimenPart.PreparationDate";
            this.labelPreparationDate.AutoSize = true;
            this.labelPreparationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPreparationDate.Location = new System.Drawing.Point(462, 23);
            this.labelPreparationDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.labelPreparationDate.Name = "labelPreparationDate";
            this.labelPreparationDate.Size = new System.Drawing.Size(38, 23);
            this.labelPreparationDate.TabIndex = 29;
            this.labelPreparationDate.Text = "Date:";
            this.labelPreparationDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerPreparationDate
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.dateTimePickerPreparationDate, 2);
            this.dateTimePickerPreparationDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerPreparationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerPreparationDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerPreparationDate.Location = new System.Drawing.Point(500, 23);
            this.dateTimePickerPreparationDate.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.dateTimePickerPreparationDate.Name = "dateTimePickerPreparationDate";
            this.dateTimePickerPreparationDate.Size = new System.Drawing.Size(74, 20);
            this.dateTimePickerPreparationDate.TabIndex = 31;
            this.dateTimePickerPreparationDate.CloseUp += new System.EventHandler(this.dateTimePickerPreparationDate_CloseUp);
            // 
            // userControlHierarchySelectorCollection
            // 
            this.userControlHierarchySelectorCollection.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlHierarchySelectorCollection.Location = new System.Drawing.Point(441, 23);
            this.userControlHierarchySelectorCollection.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.userControlHierarchySelectorCollection.Name = "userControlHierarchySelectorCollection";
            this.userControlHierarchySelectorCollection.Size = new System.Drawing.Size(15, 21);
            this.userControlHierarchySelectorCollection.TabIndex = 32;
            // 
            // buttonRestrictImagesToCurrrentPart
            // 
            this.buttonRestrictImagesToCurrrentPart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRestrictImagesToCurrrentPart.Image = global::DiversityCollection.Resource.Icones;
            this.buttonRestrictImagesToCurrrentPart.Location = new System.Drawing.Point(652, 0);
            this.buttonRestrictImagesToCurrrentPart.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRestrictImagesToCurrrentPart.Name = "buttonRestrictImagesToCurrrentPart";
            this.buttonRestrictImagesToCurrrentPart.Size = new System.Drawing.Size(21, 23);
            this.buttonRestrictImagesToCurrrentPart.TabIndex = 33;
            this.buttonRestrictImagesToCurrrentPart.UseVisualStyleBackColor = true;
            this.buttonRestrictImagesToCurrrentPart.Click += new System.EventHandler(this.buttonRestrictImagesToCurrrentPart_Click);
            // 
            // labelStorageContainer
            // 
            this.labelStorageContainer.AccessibleName = "CollectionSpecimenPart.StorageContainer";
            this.labelStorageContainer.AutoSize = true;
            this.labelStorageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStorageContainer.Location = new System.Drawing.Point(503, 73);
            this.labelStorageContainer.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelStorageContainer.Name = "labelStorageContainer";
            this.labelStorageContainer.Size = new System.Drawing.Size(35, 24);
            this.labelStorageContainer.TabIndex = 34;
            this.labelStorageContainer.Text = "Cont.:";
            this.labelStorageContainer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxStorageContainer
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.comboBoxStorageContainer, 6);
            this.comboBoxStorageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStorageContainer.FormattingEnabled = true;
            this.comboBoxStorageContainer.Location = new System.Drawing.Point(538, 73);
            this.comboBoxStorageContainer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxStorageContainer.Name = "comboBoxStorageContainer";
            this.comboBoxStorageContainer.Size = new System.Drawing.Size(135, 21);
            this.comboBoxStorageContainer.TabIndex = 35;
            this.comboBoxStorageContainer.DropDown += new System.EventHandler(this.comboBoxStorageContainer_DropDown);
            // 
            // labelStockUnit
            // 
            this.labelStockUnit.AccessibleName = "CollectionSpecimenPart.StockUnit";
            this.labelStockUnit.AutoSize = true;
            this.tableLayoutPanelStorage.SetColumnSpan(this.labelStockUnit, 2);
            this.labelStockUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStockUnit.Location = new System.Drawing.Point(598, 97);
            this.labelStockUnit.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelStockUnit.Name = "labelStockUnit";
            this.labelStockUnit.Size = new System.Drawing.Size(34, 24);
            this.labelStockUnit.TabIndex = 36;
            this.labelStockUnit.Text = "Unit:";
            this.labelStockUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxStockUnit
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.comboBoxStockUnit, 2);
            this.comboBoxStockUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStockUnit.FormattingEnabled = true;
            this.comboBoxStockUnit.Location = new System.Drawing.Point(632, 97);
            this.comboBoxStockUnit.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxStockUnit.Name = "comboBoxStockUnit";
            this.comboBoxStockUnit.Size = new System.Drawing.Size(41, 21);
            this.comboBoxStockUnit.TabIndex = 37;
            this.comboBoxStockUnit.DropDown += new System.EventHandler(this.comboBoxStockUnit_DropDown);
            // 
            // labelPreparationResponsible
            // 
            this.labelPreparationResponsible.AccessibleName = "CollectionSpecimenPart.ResponsibleName";
            this.labelPreparationResponsible.AutoSize = true;
            this.labelPreparationResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPreparationResponsible.Location = new System.Drawing.Point(459, 49);
            this.labelPreparationResponsible.Margin = new System.Windows.Forms.Padding(0);
            this.labelPreparationResponsible.Name = "labelPreparationResponsible";
            this.labelPreparationResponsible.Size = new System.Drawing.Size(41, 24);
            this.labelPreparationResponsible.TabIndex = 38;
            this.labelPreparationResponsible.Text = "Resp.:";
            this.labelPreparationResponsible.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryPreparationResponsible
            // 
            this.userControlModuleRelatedEntryPreparationResponsible.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelStorage.SetColumnSpan(this.userControlModuleRelatedEntryPreparationResponsible, 7);
            this.userControlModuleRelatedEntryPreparationResponsible.DependsOnUri = "";
            this.userControlModuleRelatedEntryPreparationResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryPreparationResponsible.Domain = "";
            this.userControlModuleRelatedEntryPreparationResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryPreparationResponsible.Location = new System.Drawing.Point(500, 49);
            this.userControlModuleRelatedEntryPreparationResponsible.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.userControlModuleRelatedEntryPreparationResponsible.Module = null;
            this.userControlModuleRelatedEntryPreparationResponsible.Name = "userControlModuleRelatedEntryPreparationResponsible";
            this.userControlModuleRelatedEntryPreparationResponsible.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryPreparationResponsible.ShowInfo = false;
            this.userControlModuleRelatedEntryPreparationResponsible.Size = new System.Drawing.Size(170, 24);
            this.userControlModuleRelatedEntryPreparationResponsible.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryPreparationResponsible.TabIndex = 39;
            // 
            // textBoxPartWithhold
            // 
            this.tableLayoutPanelStorage.SetColumnSpan(this.textBoxPartWithhold, 3);
            this.textBoxPartWithhold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPartWithhold.Location = new System.Drawing.Point(596, 0);
            this.textBoxPartWithhold.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.textBoxPartWithhold.Name = "textBoxPartWithhold";
            this.textBoxPartWithhold.Size = new System.Drawing.Size(55, 20);
            this.textBoxPartWithhold.TabIndex = 41;
            this.textBoxPartWithhold.TextChanged += new System.EventHandler(this.textBoxPartWithhold_TextChanged);
            this.textBoxPartWithhold.Leave += new System.EventHandler(this.textBoxPartWithhold_Leave);
            // 
            // buttonStockHistoryInfo
            // 
            this.buttonStockHistoryInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStockHistoryInfo.Image = global::DiversityCollection.Resource.Volume;
            this.buttonStockHistoryInfo.Location = new System.Drawing.Point(574, 97);
            this.buttonStockHistoryInfo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.buttonStockHistoryInfo.Name = "buttonStockHistoryInfo";
            this.buttonStockHistoryInfo.Size = new System.Drawing.Size(21, 22);
            this.buttonStockHistoryInfo.TabIndex = 42;
            this.buttonStockHistoryInfo.UseVisualStyleBackColor = true;
            this.buttonStockHistoryInfo.Click += new System.EventHandler(this.buttonStockHistoryInfo_Click);
            // 
            // buttonFindNextAccessionNumberPart
            // 
            this.buttonFindNextAccessionNumberPart.Image = global::DiversityCollection.Resource.CollectionSpecimen;
            this.buttonFindNextAccessionNumberPart.Location = new System.Drawing.Point(441, 0);
            this.buttonFindNextAccessionNumberPart.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFindNextAccessionNumberPart.Name = "buttonFindNextAccessionNumberPart";
            this.buttonFindNextAccessionNumberPart.Size = new System.Drawing.Size(18, 20);
            this.buttonFindNextAccessionNumberPart.TabIndex = 43;
            this.buttonFindNextAccessionNumberPart.UseVisualStyleBackColor = true;
            this.buttonFindNextAccessionNumberPart.Click += new System.EventHandler(this.buttonFindNextAccessionNumberPart_Click);
            // 
            // pictureBoxWithholdPart
            // 
            this.pictureBoxWithholdPart.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBoxWithholdPart.Image = global::DiversityCollection.Resource.Stop3;
            this.pictureBoxWithholdPart.Location = new System.Drawing.Point(579, 3);
            this.pictureBoxWithholdPart.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBoxWithholdPart.Name = "pictureBoxWithholdPart";
            this.pictureBoxWithholdPart.Size = new System.Drawing.Size(16, 20);
            this.pictureBoxWithholdPart.TabIndex = 44;
            this.pictureBoxWithholdPart.TabStop = false;
            // 
            // buttonTemplatePartSet
            // 
            this.buttonTemplatePartSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplatePartSet.FlatAppearance.BorderSize = 0;
            this.buttonTemplatePartSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTemplatePartSet.Image = global::DiversityCollection.Resource.Template;
            this.buttonTemplatePartSet.Location = new System.Drawing.Point(0, 23);
            this.buttonTemplatePartSet.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTemplatePartSet.Name = "buttonTemplatePartSet";
            this.buttonTemplatePartSet.Size = new System.Drawing.Size(15, 26);
            this.buttonTemplatePartSet.TabIndex = 45;
            this.buttonTemplatePartSet.UseVisualStyleBackColor = true;
            this.buttonTemplatePartSet.Click += new System.EventHandler(this.buttonTemplatePartSet_Click);
            // 
            // buttonTemplatePartEdit
            // 
            this.buttonTemplatePartEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplatePartEdit.FlatAppearance.BorderSize = 0;
            this.buttonTemplatePartEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTemplatePartEdit.Image = global::DiversityCollection.Resource.TemplateEditor;
            this.buttonTemplatePartEdit.Location = new System.Drawing.Point(0, 49);
            this.buttonTemplatePartEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTemplatePartEdit.Name = "buttonTemplatePartEdit";
            this.buttonTemplatePartEdit.Size = new System.Drawing.Size(15, 24);
            this.buttonTemplatePartEdit.TabIndex = 46;
            this.buttonTemplatePartEdit.UseVisualStyleBackColor = true;
            this.buttonTemplatePartEdit.Click += new System.EventHandler(this.buttonTemplatePartEdit_Click);
            // 
            // buttonSetStorageLocationSource
            // 
            this.buttonSetStorageLocationSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetStorageLocationSource.FlatAppearance.BorderSize = 0;
            this.buttonSetStorageLocationSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetStorageLocationSource.Image = global::DiversityCollection.Resource.SettingsNarrow;
            this.buttonSetStorageLocationSource.Location = new System.Drawing.Point(0, 73);
            this.buttonSetStorageLocationSource.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetStorageLocationSource.Name = "buttonSetStorageLocationSource";
            this.buttonSetStorageLocationSource.Size = new System.Drawing.Size(15, 24);
            this.buttonSetStorageLocationSource.TabIndex = 47;
            this.buttonSetStorageLocationSource.UseVisualStyleBackColor = true;
            this.buttonSetStorageLocationSource.Click += new System.EventHandler(this.buttonSetStorageLocationSource_Click);
            // 
            // buttonSetMaterialCategoryRange
            // 
            this.buttonSetMaterialCategoryRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSetMaterialCategoryRange.FlatAppearance.BorderSize = 0;
            this.buttonSetMaterialCategoryRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetMaterialCategoryRange.Image = global::DiversityCollection.Resource.SettingsNarrow;
            this.buttonSetMaterialCategoryRange.Location = new System.Drawing.Point(0, 97);
            this.buttonSetMaterialCategoryRange.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetMaterialCategoryRange.Name = "buttonSetMaterialCategoryRange";
            this.buttonSetMaterialCategoryRange.Size = new System.Drawing.Size(15, 24);
            this.buttonSetMaterialCategoryRange.TabIndex = 48;
            this.buttonSetMaterialCategoryRange.UseVisualStyleBackColor = true;
            this.buttonSetMaterialCategoryRange.Click += new System.EventHandler(this.buttonSetMaterialCategoryRange_Click);
            // 
            // labelSpecimenPartID
            // 
            this.labelSpecimenPartID.AutoSize = true;
            this.labelSpecimenPartID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpecimenPartID.Location = new System.Drawing.Point(595, 23);
            this.labelSpecimenPartID.Margin = new System.Windows.Forms.Padding(0);
            this.labelSpecimenPartID.Name = "labelSpecimenPartID";
            this.labelSpecimenPartID.Size = new System.Drawing.Size(18, 26);
            this.labelSpecimenPartID.TabIndex = 49;
            this.labelSpecimenPartID.Text = "ID";
            this.labelSpecimenPartID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonPreparationDateDelete
            // 
            this.buttonPreparationDateDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPreparationDateDelete.FlatAppearance.BorderSize = 0;
            this.buttonPreparationDateDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPreparationDateDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonPreparationDateDelete.Location = new System.Drawing.Point(574, 23);
            this.buttonPreparationDateDelete.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.buttonPreparationDateDelete.Name = "buttonPreparationDateDelete";
            this.buttonPreparationDateDelete.Size = new System.Drawing.Size(18, 23);
            this.buttonPreparationDateDelete.TabIndex = 50;
            this.toolTip.SetToolTip(this.buttonPreparationDateDelete, "Remove preparation date");
            this.buttonPreparationDateDelete.UseVisualStyleBackColor = true;
            this.buttonPreparationDateDelete.Click += new System.EventHandler(this.buttonPreparationDateDelete_Click);
            // 
            // tableLayoutPanelIdentificationUnitPart
            // 
            this.tableLayoutPanelIdentificationUnitPart.ColumnCount = 3;
            this.tableLayoutPanelIdentificationUnitPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanelIdentificationUnitPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelIdentificationUnitPart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelIdentificationUnitPart.Controls.Add(this.labelIdentificationUnitInPartDescription, 1, 0);
            this.tableLayoutPanelIdentificationUnitPart.Controls.Add(this.comboBoxIdentificationUnitInPartDescription, 2, 0);
            this.tableLayoutPanelIdentificationUnitPart.Controls.Add(this.pictureBoxIdentificationUnitInPartDescription, 0, 0);
            this.tableLayoutPanelIdentificationUnitPart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelIdentificationUnitPart.Location = new System.Drawing.Point(3, 283);
            this.tableLayoutPanelIdentificationUnitPart.Name = "tableLayoutPanelIdentificationUnitPart";
            this.tableLayoutPanelIdentificationUnitPart.RowCount = 1;
            this.tableLayoutPanelIdentificationUnitPart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelIdentificationUnitPart.Size = new System.Drawing.Size(673, 26);
            this.tableLayoutPanelIdentificationUnitPart.TabIndex = 4;
            this.tableLayoutPanelIdentificationUnitPart.Visible = false;
            // 
            // labelIdentificationUnitInPartDescription
            // 
            this.labelIdentificationUnitInPartDescription.AccessibleName = "IdentificationUnitInPart.Description";
            this.labelIdentificationUnitInPartDescription.AutoSize = true;
            this.labelIdentificationUnitInPartDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIdentificationUnitInPartDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIdentificationUnitInPartDescription.Location = new System.Drawing.Point(16, 0);
            this.labelIdentificationUnitInPartDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelIdentificationUnitInPartDescription.Name = "labelIdentificationUnitInPartDescription";
            this.labelIdentificationUnitInPartDescription.Size = new System.Drawing.Size(52, 26);
            this.labelIdentificationUnitInPartDescription.TabIndex = 0;
            this.labelIdentificationUnitInPartDescription.Text = "Descript.:";
            this.labelIdentificationUnitInPartDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelIdentificationUnitInPartDescription.Visible = false;
            // 
            // comboBoxIdentificationUnitInPartDescription
            // 
            this.comboBoxIdentificationUnitInPartDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxIdentificationUnitInPartDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxIdentificationUnitInPartDescription.FormattingEnabled = true;
            this.comboBoxIdentificationUnitInPartDescription.Location = new System.Drawing.Point(68, 3);
            this.comboBoxIdentificationUnitInPartDescription.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.comboBoxIdentificationUnitInPartDescription.Name = "comboBoxIdentificationUnitInPartDescription";
            this.comboBoxIdentificationUnitInPartDescription.Size = new System.Drawing.Size(605, 21);
            this.comboBoxIdentificationUnitInPartDescription.TabIndex = 1;
            this.comboBoxIdentificationUnitInPartDescription.Visible = false;
            this.comboBoxIdentificationUnitInPartDescription.DropDown += new System.EventHandler(this.comboBoxIdentificationUnitInPartDescription_DropDown);
            // 
            // pictureBoxIdentificationUnitInPartDescription
            // 
            this.pictureBoxIdentificationUnitInPartDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxIdentificationUnitInPartDescription.Location = new System.Drawing.Point(0, 3);
            this.pictureBoxIdentificationUnitInPartDescription.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.pictureBoxIdentificationUnitInPartDescription.Name = "pictureBoxIdentificationUnitInPartDescription";
            this.pictureBoxIdentificationUnitInPartDescription.Size = new System.Drawing.Size(16, 20);
            this.pictureBoxIdentificationUnitInPartDescription.TabIndex = 2;
            this.pictureBoxIdentificationUnitInPartDescription.TabStop = false;
            this.pictureBoxIdentificationUnitInPartDescription.Visible = false;
            // 
            // UserControl_Part
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPart);
            this.Name = "UserControl_Part";
            this.Size = new System.Drawing.Size(679, 312);
            this.groupBoxPart.ResumeLayout(false);
            this.tableLayoutPanelStorage.ResumeLayout(false);
            this.tableLayoutPanelStorage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimenPart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWithholdPart)).EndInit();
            this.tableLayoutPanelIdentificationUnitPart.ResumeLayout(false);
            this.tableLayoutPanelIdentificationUnitPart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIdentificationUnitInPartDescription)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStorage;
        private System.Windows.Forms.TextBox textBoxSpecimenPartID;
        private System.Windows.Forms.TextBox textBoxStorageNotes;
        private System.Windows.Forms.Label labelStorageNotes;
        private System.Windows.Forms.ComboBox comboBoxMaterialCategory;
        private System.Windows.Forms.PictureBox pictureBoxSpecimenPart;
        private System.Windows.Forms.Label labelMaterialCategory;
        private System.Windows.Forms.Label labelStorageLocation;
        private System.Windows.Forms.Label labelStock;
        private System.Windows.Forms.TextBox textBoxStock;
        private System.Windows.Forms.ComboBox comboBoxCollection;
        private System.Windows.Forms.Label labelCollection;
        private System.Windows.Forms.ComboBox comboBoxStorageLocation;
        private System.Windows.Forms.TextBox textBoxStorageLocation;
        private System.Windows.Forms.ComboBox comboBoxStorageNotes;
        private System.Windows.Forms.Label labelAccessionNumberPart;
        private System.Windows.Forms.TextBox textBoxAccessionNumberPart;
        private System.Windows.Forms.Label labelPartSublabel;
        private System.Windows.Forms.TextBox textBoxPartSublabel;
        private System.Windows.Forms.Label labelPreparationMethod;
        private System.Windows.Forms.ComboBox comboBoxPreparationMethod;
        private System.Windows.Forms.TextBox textBoxPreparationMethod;
        private System.Windows.Forms.Label labelPreparationDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerPreparationDate;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorCollection;
        private System.Windows.Forms.Label labelStorageContainer;
        private System.Windows.Forms.ComboBox comboBoxStorageContainer;
        private System.Windows.Forms.Label labelStockUnit;
        private System.Windows.Forms.ComboBox comboBoxStockUnit;
        private System.Windows.Forms.Label labelPreparationResponsible;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryPreparationResponsible;
        private System.Windows.Forms.TextBox textBoxPartWithhold;
        private System.Windows.Forms.Button buttonStockHistoryInfo;
        private System.Windows.Forms.Button buttonFindNextAccessionNumberPart;
        private System.Windows.Forms.PictureBox pictureBoxWithholdPart;
        private System.Windows.Forms.Button buttonTemplatePartSet;
        private System.Windows.Forms.Button buttonTemplatePartEdit;
        private System.Windows.Forms.Button buttonSetStorageLocationSource;
        private System.Windows.Forms.Button buttonSetMaterialCategoryRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIdentificationUnitPart;
        private System.Windows.Forms.Label labelIdentificationUnitInPartDescription;
        private System.Windows.Forms.ComboBox comboBoxIdentificationUnitInPartDescription;
        private System.Windows.Forms.PictureBox pictureBoxIdentificationUnitInPartDescription;
        public System.Windows.Forms.Button buttonRestrictImagesToCurrrentPart;
        private System.Windows.Forms.Label labelSpecimenPartID;
        private System.Windows.Forms.Button buttonPreparationDateDelete;
    }
}
