namespace DiversityCollection.Forms
{
    partial class FormImportImages
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
            this.groupBoxImageSource = new System.Windows.Forms.GroupBox();
            this.checkBoxArchiveImages = new System.Windows.Forms.CheckBox();
            this.buttonFolderArchive = new System.Windows.Forms.Button();
            this.textBoxFolderArchive = new System.Windows.Forms.TextBox();
            this.labelOriginal = new System.Windows.Forms.Label();
            this.buttonFolderOriginal = new System.Windows.Forms.Button();
            this.labelFolderOriginal = new System.Windows.Forms.Label();
            this.textBoxFolderOriginal = new System.Windows.Forms.TextBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.groupBoxSecurityChecks = new System.Windows.Forms.GroupBox();
            this.checkBoxCutAfterSeparator = new System.Windows.Forms.CheckBox();
            this.checkBoxAppendDateToFilename = new System.Windows.Forms.CheckBox();
            this.checkBoxOverwriteImages = new System.Windows.Forms.CheckBox();
            this.textBoxAccessionSeparator = new System.Windows.Forms.TextBox();
            this.textBoxBarcodeStart = new System.Windows.Forms.TextBox();
            this.textBoxBarcodeLength = new System.Windows.Forms.TextBox();
            this.checkBoxBarcodeStart = new System.Windows.Forms.CheckBox();
            this.labelBarcode = new System.Windows.Forms.Label();
            this.checkBoxBarcodeLength = new System.Windows.Forms.CheckBox();
            this.checkBoxAppendImages = new System.Windows.Forms.CheckBox();
            this.groupBoxImageTarget = new System.Windows.Forms.GroupBox();
            this.textBoxSubdirectory = new System.Windows.Forms.TextBox();
            this.labelSubdirectory = new System.Windows.Forms.Label();
            this.numericUpDownSubdirectry = new System.Windows.Forms.NumericUpDown();
            this.checkBoxSubdirectory = new System.Windows.Forms.CheckBox();
            this.radioButtonArchiveImage = new System.Windows.Forms.RadioButton();
            this.radioButtonPreviewImage = new System.Windows.Forms.RadioButton();
            this.labelChooseImageForImport = new System.Windows.Forms.Label();
            this.radioButtonWebImage = new System.Windows.Forms.RadioButton();
            this.radioButtonOriginalImage = new System.Windows.Forms.RadioButton();
            this.dataGridViewImport = new System.Windows.Forms.DataGridView();
            this.ColumnOK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnAccessionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnOriginal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnArchive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWeb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPreview = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAppend = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.openFileDialogImport = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialogImport = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxDatabase = new System.Windows.Forms.GroupBox();
            this.labelIdentification = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryIdentification = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelGazetteer = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryGazetteer = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryExsiccate = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelExsiccate = new System.Windows.Forms.Label();
            this.labelLabelType = new System.Windows.Forms.Label();
            this.comboBoxLabelType = new System.Windows.Forms.ComboBox();
            this.labelCollector = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryCollector = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelAccessionDate = new System.Windows.Forms.Label();
            this.userControlDatePanelAccessionDate = new DiversityWorkbench.UserControls.UserControlDatePanel();
            this.labelCollectionDate = new System.Windows.Forms.Label();
            this.userControlDatePanelCollectionDate = new DiversityWorkbench.UserControls.UserControlDatePanel();
            this.buttonTestImport = new System.Windows.Forms.Button();
            this.textBoxWebFolder = new System.Windows.Forms.TextBox();
            this.buttonBrowseUrlFolder = new System.Windows.Forms.Button();
            this.buttonStartImport = new System.Windows.Forms.Button();
            this.labelImageType = new System.Windows.Forms.Label();
            this.comboBoxImageType = new System.Windows.Forms.ComboBox();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.checkBoxImageAsUri = new System.Windows.Forms.CheckBox();
            this.comboBoxCollection = new System.Windows.Forms.ComboBox();
            this.labelCollection = new System.Windows.Forms.Label();
            this.labelMaterial = new System.Windows.Forms.Label();
            this.comboBoxMaterialCategory = new System.Windows.Forms.ComboBox();
            this.labelProject = new System.Windows.Forms.Label();
            this.comboBoxProject = new System.Windows.Forms.ComboBox();
            this.labelGroup = new System.Windows.Forms.Label();
            this.comboBoxTaxonomicGroup = new System.Windows.Forms.ComboBox();
            this.textBoxLogfile = new System.Windows.Forms.TextBox();
            this.labelLogfile = new System.Windows.Forms.Label();
            this.checkBoxCreateLog = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.dataSetCollectionSpecimen = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            this.collectionSpecimenBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.collectionAgentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.identificationUnitBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.collectionGeographyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.identificationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.collectionEventBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBoxImageSource.SuspendLayout();
            this.groupBoxSecurityChecks.SuspendLayout();
            this.groupBoxImageTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubdirectry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImport)).BeginInit();
            this.groupBoxDatabase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionSpecimen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionSpecimenBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionAgentBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.identificationUnitBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionGeographyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.identificationBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionEventBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxImageSource
            // 
            this.groupBoxImageSource.Controls.Add(this.checkBoxArchiveImages);
            this.groupBoxImageSource.Controls.Add(this.buttonFolderArchive);
            this.groupBoxImageSource.Controls.Add(this.textBoxFolderArchive);
            this.groupBoxImageSource.Controls.Add(this.labelOriginal);
            this.groupBoxImageSource.Controls.Add(this.buttonFolderOriginal);
            this.groupBoxImageSource.Controls.Add(this.labelFolderOriginal);
            this.groupBoxImageSource.Controls.Add(this.textBoxFolderOriginal);
            this.groupBoxImageSource.Location = new System.Drawing.Point(12, 28);
            this.groupBoxImageSource.Name = "groupBoxImageSource";
            this.groupBoxImageSource.Size = new System.Drawing.Size(992, 75);
            this.groupBoxImageSource.TabIndex = 65;
            this.groupBoxImageSource.TabStop = false;
            this.groupBoxImageSource.Text = "Image source";
            // 
            // checkBoxArchiveImages
            // 
            this.checkBoxArchiveImages.AutoSize = true;
            this.checkBoxArchiveImages.Location = new System.Drawing.Point(192, 48);
            this.checkBoxArchiveImages.Name = "checkBoxArchiveImages";
            this.checkBoxArchiveImages.Size = new System.Drawing.Size(141, 17);
            this.checkBoxArchiveImages.TabIndex = 14;
            this.checkBoxArchiveImages.Text = "Archive images in folder:";
            this.checkBoxArchiveImages.UseVisualStyleBackColor = true;
            this.checkBoxArchiveImages.CheckedChanged += new System.EventHandler(this.checkBoxArchiveImages_CheckedChanged);
            // 
            // buttonFolderArchive
            // 
            this.buttonFolderArchive.Image = global::DiversityCollection.Resource.Folder;
            this.buttonFolderArchive.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonFolderArchive.Location = new System.Drawing.Point(962, 44);
            this.buttonFolderArchive.Name = "buttonFolderArchive";
            this.buttonFolderArchive.Size = new System.Drawing.Size(24, 24);
            this.buttonFolderArchive.TabIndex = 13;
            this.buttonFolderArchive.Click += new System.EventHandler(this.buttonFolderArchive_Click);
            // 
            // textBoxFolderArchive
            // 
            this.textBoxFolderArchive.AcceptsReturn = true;
            this.textBoxFolderArchive.BackColor = System.Drawing.Color.Pink;
            this.textBoxFolderArchive.Location = new System.Drawing.Point(340, 46);
            this.textBoxFolderArchive.Name = "textBoxFolderArchive";
            this.textBoxFolderArchive.Size = new System.Drawing.Size(616, 20);
            this.textBoxFolderArchive.TabIndex = 12;
            this.textBoxFolderArchive.TextChanged += new System.EventHandler(this.textBoxFolderArchive_TextChanged);
            // 
            // labelOriginal
            // 
            this.labelOriginal.Location = new System.Drawing.Point(5, 16);
            this.labelOriginal.Name = "labelOriginal";
            this.labelOriginal.Size = new System.Drawing.Size(154, 27);
            this.labelOriginal.TabIndex = 10;
            this.labelOriginal.Text = "Select the images that should be imported into the database";
            // 
            // buttonFolderOriginal
            // 
            this.buttonFolderOriginal.Image = global::DiversityCollection.Resource.OpenFolder;
            this.buttonFolderOriginal.Location = new System.Drawing.Point(60, 46);
            this.buttonFolderOriginal.Name = "buttonFolderOriginal";
            this.buttonFolderOriginal.Size = new System.Drawing.Size(24, 24);
            this.buttonFolderOriginal.TabIndex = 9;
            this.buttonFolderOriginal.Click += new System.EventHandler(this.buttonFolderOriginal_Click);
            // 
            // labelFolderOriginal
            // 
            this.labelFolderOriginal.Location = new System.Drawing.Point(206, 15);
            this.labelFolderOriginal.Name = "labelFolderOriginal";
            this.labelFolderOriginal.Size = new System.Drawing.Size(128, 24);
            this.labelFolderOriginal.TabIndex = 0;
            this.labelFolderOriginal.Text = "Folder of original images";
            this.labelFolderOriginal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxFolderOriginal
            // 
            this.textBoxFolderOriginal.BackColor = System.Drawing.Color.Pink;
            this.textBoxFolderOriginal.Location = new System.Drawing.Point(340, 19);
            this.textBoxFolderOriginal.Name = "textBoxFolderOriginal";
            this.textBoxFolderOriginal.Size = new System.Drawing.Size(646, 20);
            this.textBoxFolderOriginal.TabIndex = 1;
            // 
            // labelHeader
            // 
            this.labelHeader.Location = new System.Drawing.Point(12, 9);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(744, 16);
            this.labelHeader.TabIndex = 70;
            this.labelHeader.Text = "Import of image files of specimen. The file names should correspond to the access" +
                "ion number of the specimen, e.g. M-00134403.tif";
            // 
            // groupBoxSecurityChecks
            // 
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxCutAfterSeparator);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxAppendDateToFilename);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxOverwriteImages);
            this.groupBoxSecurityChecks.Controls.Add(this.textBoxAccessionSeparator);
            this.groupBoxSecurityChecks.Controls.Add(this.textBoxBarcodeStart);
            this.groupBoxSecurityChecks.Controls.Add(this.textBoxBarcodeLength);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxBarcodeStart);
            this.groupBoxSecurityChecks.Controls.Add(this.labelBarcode);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxBarcodeLength);
            this.groupBoxSecurityChecks.Location = new System.Drawing.Point(12, 109);
            this.groupBoxSecurityChecks.Name = "groupBoxSecurityChecks";
            this.groupBoxSecurityChecks.Size = new System.Drawing.Size(992, 43);
            this.groupBoxSecurityChecks.TabIndex = 71;
            this.groupBoxSecurityChecks.TabStop = false;
            this.groupBoxSecurityChecks.Text = "Import options and security checks";
            // 
            // checkBoxCutAfterSeparator
            // 
            this.checkBoxCutAfterSeparator.AutoSize = true;
            this.checkBoxCutAfterSeparator.Checked = true;
            this.checkBoxCutAfterSeparator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCutAfterSeparator.Location = new System.Drawing.Point(369, 18);
            this.checkBoxCutAfterSeparator.Name = "checkBoxCutAfterSeparator";
            this.checkBoxCutAfterSeparator.Size = new System.Drawing.Size(179, 17);
            this.checkBoxCutAfterSeparator.TabIndex = 57;
            this.checkBoxCutAfterSeparator.Text = "Separator for accession number:";
            this.checkBoxCutAfterSeparator.UseVisualStyleBackColor = true;
            this.checkBoxCutAfterSeparator.CheckedChanged += new System.EventHandler(this.checkBoxCutAfterSeparator_CheckedChanged);
            // 
            // checkBoxAppendDateToFilename
            // 
            this.checkBoxAppendDateToFilename.Location = new System.Drawing.Point(171, 14);
            this.checkBoxAppendDateToFilename.Name = "checkBoxAppendDateToFilename";
            this.checkBoxAppendDateToFilename.Size = new System.Drawing.Size(192, 24);
            this.checkBoxAppendDateToFilename.TabIndex = 56;
            this.checkBoxAppendDateToFilename.Text = "Append date and time to filename";
            // 
            // checkBoxOverwriteImages
            // 
            this.checkBoxOverwriteImages.Location = new System.Drawing.Point(8, 14);
            this.checkBoxOverwriteImages.Name = "checkBoxOverwriteImages";
            this.checkBoxOverwriteImages.Size = new System.Drawing.Size(168, 24);
            this.checkBoxOverwriteImages.TabIndex = 45;
            this.checkBoxOverwriteImages.Text = "Overwrite existing images";
            // 
            // textBoxAccessionSeparator
            // 
            this.textBoxAccessionSeparator.Location = new System.Drawing.Point(553, 16);
            this.textBoxAccessionSeparator.Name = "textBoxAccessionSeparator";
            this.textBoxAccessionSeparator.Size = new System.Drawing.Size(24, 20);
            this.textBoxAccessionSeparator.TabIndex = 50;
            this.textBoxAccessionSeparator.Text = "_";
            this.textBoxAccessionSeparator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxBarcodeStart
            // 
            this.textBoxBarcodeStart.BackColor = System.Drawing.Color.Pink;
            this.textBoxBarcodeStart.Location = new System.Drawing.Point(796, 14);
            this.textBoxBarcodeStart.Name = "textBoxBarcodeStart";
            this.textBoxBarcodeStart.Size = new System.Drawing.Size(48, 20);
            this.textBoxBarcodeStart.TabIndex = 49;
            this.textBoxBarcodeStart.Text = "M-";
            this.textBoxBarcodeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxBarcodeLength
            // 
            this.textBoxBarcodeLength.BackColor = System.Drawing.Color.Pink;
            this.textBoxBarcodeLength.Location = new System.Drawing.Point(940, 14);
            this.textBoxBarcodeLength.Name = "textBoxBarcodeLength";
            this.textBoxBarcodeLength.Size = new System.Drawing.Size(24, 20);
            this.textBoxBarcodeLength.TabIndex = 48;
            this.textBoxBarcodeLength.Text = "9";
            this.textBoxBarcodeLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxBarcodeStart
            // 
            this.checkBoxBarcodeStart.Checked = true;
            this.checkBoxBarcodeStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBarcodeStart.Location = new System.Drawing.Point(712, 14);
            this.checkBoxBarcodeStart.Name = "checkBoxBarcodeStart";
            this.checkBoxBarcodeStart.Size = new System.Drawing.Size(88, 24);
            this.checkBoxBarcodeStart.TabIndex = 47;
            this.checkBoxBarcodeStart.Text = "check start:";
            this.checkBoxBarcodeStart.CheckedChanged += new System.EventHandler(this.checkBoxBarcodeStart_CheckedChanged);
            // 
            // labelBarcode
            // 
            this.labelBarcode.Location = new System.Drawing.Point(621, 13);
            this.labelBarcode.Name = "labelBarcode";
            this.labelBarcode.Size = new System.Drawing.Size(88, 24);
            this.labelBarcode.TabIndex = 54;
            this.labelBarcode.Text = "Accession Nr.:";
            this.labelBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxBarcodeLength
            // 
            this.checkBoxBarcodeLength.Checked = true;
            this.checkBoxBarcodeLength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBarcodeLength.Location = new System.Drawing.Point(852, 14);
            this.checkBoxBarcodeLength.Name = "checkBoxBarcodeLength";
            this.checkBoxBarcodeLength.Size = new System.Drawing.Size(96, 24);
            this.checkBoxBarcodeLength.TabIndex = 46;
            this.checkBoxBarcodeLength.Text = "check length:";
            this.checkBoxBarcodeLength.CheckedChanged += new System.EventHandler(this.checkBoxBarcodeLength_CheckedChanged);
            // 
            // checkBoxAppendImages
            // 
            this.checkBoxAppendImages.Location = new System.Drawing.Point(6, 105);
            this.checkBoxAppendImages.Name = "checkBoxAppendImages";
            this.checkBoxAppendImages.Size = new System.Drawing.Size(114, 43);
            this.checkBoxAppendImages.TabIndex = 55;
            this.checkBoxAppendImages.Text = "Append images if accession number is present";
            // 
            // groupBoxImageTarget
            // 
            this.groupBoxImageTarget.Controls.Add(this.textBoxSubdirectory);
            this.groupBoxImageTarget.Controls.Add(this.labelSubdirectory);
            this.groupBoxImageTarget.Controls.Add(this.numericUpDownSubdirectry);
            this.groupBoxImageTarget.Controls.Add(this.checkBoxSubdirectory);
            this.groupBoxImageTarget.Controls.Add(this.radioButtonArchiveImage);
            this.groupBoxImageTarget.Controls.Add(this.radioButtonPreviewImage);
            this.groupBoxImageTarget.Controls.Add(this.labelChooseImageForImport);
            this.groupBoxImageTarget.Controls.Add(this.radioButtonWebImage);
            this.groupBoxImageTarget.Controls.Add(this.radioButtonOriginalImage);
            this.groupBoxImageTarget.Location = new System.Drawing.Point(12, 158);
            this.groupBoxImageTarget.Name = "groupBoxImageTarget";
            this.groupBoxImageTarget.Size = new System.Drawing.Size(992, 68);
            this.groupBoxImageTarget.TabIndex = 72;
            this.groupBoxImageTarget.TabStop = false;
            this.groupBoxImageTarget.Text = "Image target";
            // 
            // textBoxSubdirectory
            // 
            this.textBoxSubdirectory.Location = new System.Drawing.Point(528, 17);
            this.textBoxSubdirectory.Name = "textBoxSubdirectory";
            this.textBoxSubdirectory.ReadOnly = true;
            this.textBoxSubdirectory.Size = new System.Drawing.Size(237, 20);
            this.textBoxSubdirectory.TabIndex = 73;
            // 
            // labelSubdirectory
            // 
            this.labelSubdirectory.AutoSize = true;
            this.labelSubdirectory.Location = new System.Drawing.Point(397, 22);
            this.labelSubdirectory.Name = "labelSubdirectory";
            this.labelSubdirectory.Size = new System.Drawing.Size(130, 13);
            this.labelSubdirectory.TabIndex = 72;
            this.labelSubdirectory.Text = "Examples for subdirectory:";
            // 
            // numericUpDownSubdirectry
            // 
            this.numericUpDownSubdirectry.BackColor = System.Drawing.Color.Pink;
            this.numericUpDownSubdirectry.Location = new System.Drawing.Point(335, 18);
            this.numericUpDownSubdirectry.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownSubdirectry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSubdirectry.Name = "numericUpDownSubdirectry";
            this.numericUpDownSubdirectry.Size = new System.Drawing.Size(43, 20);
            this.numericUpDownSubdirectry.TabIndex = 71;
            this.numericUpDownSubdirectry.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // checkBoxSubdirectory
            // 
            this.checkBoxSubdirectory.AutoSize = true;
            this.checkBoxSubdirectory.Location = new System.Drawing.Point(8, 19);
            this.checkBoxSubdirectory.Name = "checkBoxSubdirectory";
            this.checkBoxSubdirectory.Size = new System.Drawing.Size(319, 17);
            this.checkBoxSubdirectory.TabIndex = 70;
            this.checkBoxSubdirectory.Text = "Place images in subdirectories according to accession number";
            this.checkBoxSubdirectory.UseVisualStyleBackColor = true;
            this.checkBoxSubdirectory.CheckedChanged += new System.EventHandler(this.checkBoxSubdirectory_CheckedChanged);
            // 
            // radioButtonArchiveImage
            // 
            this.radioButtonArchiveImage.AutoSize = true;
            this.radioButtonArchiveImage.Location = new System.Drawing.Point(273, 45);
            this.radioButtonArchiveImage.Name = "radioButtonArchiveImage";
            this.radioButtonArchiveImage.Size = new System.Drawing.Size(61, 17);
            this.radioButtonArchiveImage.TabIndex = 79;
            this.radioButtonArchiveImage.Text = "Archice";
            this.radioButtonArchiveImage.UseVisualStyleBackColor = true;
            // 
            // radioButtonPreviewImage
            // 
            this.radioButtonPreviewImage.Location = new System.Drawing.Point(423, 45);
            this.radioButtonPreviewImage.Name = "radioButtonPreviewImage";
            this.radioButtonPreviewImage.Size = new System.Drawing.Size(64, 17);
            this.radioButtonPreviewImage.TabIndex = 49;
            this.radioButtonPreviewImage.Text = "Preview";
            // 
            // labelChooseImageForImport
            // 
            this.labelChooseImageForImport.Location = new System.Drawing.Point(5, 47);
            this.labelChooseImageForImport.Name = "labelChooseImageForImport";
            this.labelChooseImageForImport.Size = new System.Drawing.Size(184, 15);
            this.labelChooseImageForImport.TabIndex = 48;
            this.labelChooseImageForImport.Text = "Choose image for import in database";
            // 
            // radioButtonWebImage
            // 
            this.radioButtonWebImage.Location = new System.Drawing.Point(353, 45);
            this.radioButtonWebImage.Name = "radioButtonWebImage";
            this.radioButtonWebImage.Size = new System.Drawing.Size(64, 17);
            this.radioButtonWebImage.TabIndex = 44;
            this.radioButtonWebImage.Text = "Web";
            // 
            // radioButtonOriginalImage
            // 
            this.radioButtonOriginalImage.Checked = true;
            this.radioButtonOriginalImage.Location = new System.Drawing.Point(195, 45);
            this.radioButtonOriginalImage.Name = "radioButtonOriginalImage";
            this.radioButtonOriginalImage.Size = new System.Drawing.Size(64, 17);
            this.radioButtonOriginalImage.TabIndex = 45;
            this.radioButtonOriginalImage.TabStop = true;
            this.radioButtonOriginalImage.Text = "Original";
            // 
            // dataGridViewImport
            // 
            this.dataGridViewImport.AllowUserToAddRows = false;
            this.dataGridViewImport.AllowUserToResizeRows = false;
            this.dataGridViewImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewImport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnOK,
            this.ColumnAccessionNumber,
            this.ColumnOriginal,
            this.ColumnArchive,
            this.ColumnWeb,
            this.ColumnPreview,
            this.ColumnError,
            this.ColumnAppend});
            this.dataGridViewImport.Location = new System.Drawing.Point(12, 237);
            this.dataGridViewImport.Name = "dataGridViewImport";
            this.dataGridViewImport.RowHeadersWidth = 24;
            this.dataGridViewImport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewImport.Size = new System.Drawing.Size(992, 267);
            this.dataGridViewImport.TabIndex = 73;
            this.dataGridViewImport.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewImport_CellClick);
            // 
            // ColumnOK
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.NullValue = false;
            this.ColumnOK.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnOK.Frozen = true;
            this.ColumnOK.HeaderText = "OK";
            this.ColumnOK.Name = "ColumnOK";
            this.ColumnOK.ReadOnly = true;
            this.ColumnOK.Width = 24;
            // 
            // ColumnAccessionNumber
            // 
            this.ColumnAccessionNumber.Frozen = true;
            this.ColumnAccessionNumber.HeaderText = "Acc. Nr.";
            this.ColumnAccessionNumber.MinimumWidth = 80;
            this.ColumnAccessionNumber.Name = "ColumnAccessionNumber";
            this.ColumnAccessionNumber.Width = 80;
            // 
            // ColumnOriginal
            // 
            this.ColumnOriginal.Frozen = true;
            this.ColumnOriginal.HeaderText = "Original";
            this.ColumnOriginal.Name = "ColumnOriginal";
            this.ColumnOriginal.ReadOnly = true;
            // 
            // ColumnArchive
            // 
            this.ColumnArchive.Frozen = true;
            this.ColumnArchive.HeaderText = "Archive";
            this.ColumnArchive.Name = "ColumnArchive";
            this.ColumnArchive.Width = 180;
            // 
            // ColumnWeb
            // 
            this.ColumnWeb.Frozen = true;
            this.ColumnWeb.HeaderText = "Web";
            this.ColumnWeb.Name = "ColumnWeb";
            this.ColumnWeb.Width = 180;
            // 
            // ColumnPreview
            // 
            this.ColumnPreview.Frozen = true;
            this.ColumnPreview.HeaderText = "Preview";
            this.ColumnPreview.Name = "ColumnPreview";
            this.ColumnPreview.Width = 180;
            // 
            // ColumnError
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Red;
            this.ColumnError.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnError.Frozen = true;
            this.ColumnError.HeaderText = "Error";
            this.ColumnError.Name = "ColumnError";
            this.ColumnError.ReadOnly = true;
            this.ColumnError.Width = 180;
            // 
            // ColumnAppend
            // 
            this.ColumnAppend.Frozen = true;
            this.ColumnAppend.HeaderText = "Append";
            this.ColumnAppend.Name = "ColumnAppend";
            this.ColumnAppend.ReadOnly = true;
            this.ColumnAppend.Width = 25;
            // 
            // openFileDialogImport
            // 
            this.openFileDialogImport.FileName = "openFileDialogImport";
            // 
            // groupBoxDatabase
            // 
            this.groupBoxDatabase.Controls.Add(this.labelIdentification);
            this.groupBoxDatabase.Controls.Add(this.userControlModuleRelatedEntryIdentification);
            this.groupBoxDatabase.Controls.Add(this.labelGazetteer);
            this.groupBoxDatabase.Controls.Add(this.userControlModuleRelatedEntryGazetteer);
            this.groupBoxDatabase.Controls.Add(this.userControlModuleRelatedEntryExsiccate);
            this.groupBoxDatabase.Controls.Add(this.labelExsiccate);
            this.groupBoxDatabase.Controls.Add(this.labelLabelType);
            this.groupBoxDatabase.Controls.Add(this.comboBoxLabelType);
            this.groupBoxDatabase.Controls.Add(this.labelCollector);
            this.groupBoxDatabase.Controls.Add(this.userControlModuleRelatedEntryCollector);
            this.groupBoxDatabase.Controls.Add(this.labelAccessionDate);
            this.groupBoxDatabase.Controls.Add(this.userControlDatePanelAccessionDate);
            this.groupBoxDatabase.Controls.Add(this.labelCollectionDate);
            this.groupBoxDatabase.Controls.Add(this.userControlDatePanelCollectionDate);
            this.groupBoxDatabase.Controls.Add(this.buttonTestImport);
            this.groupBoxDatabase.Controls.Add(this.textBoxWebFolder);
            this.groupBoxDatabase.Controls.Add(this.buttonBrowseUrlFolder);
            this.groupBoxDatabase.Controls.Add(this.buttonStartImport);
            this.groupBoxDatabase.Controls.Add(this.labelImageType);
            this.groupBoxDatabase.Controls.Add(this.comboBoxImageType);
            this.groupBoxDatabase.Controls.Add(this.checkBoxAppendImages);
            this.groupBoxDatabase.Controls.Add(this.labelDatabase);
            this.groupBoxDatabase.Controls.Add(this.checkBoxImageAsUri);
            this.groupBoxDatabase.Controls.Add(this.comboBoxCollection);
            this.groupBoxDatabase.Controls.Add(this.labelCollection);
            this.groupBoxDatabase.Controls.Add(this.labelMaterial);
            this.groupBoxDatabase.Controls.Add(this.comboBoxMaterialCategory);
            this.groupBoxDatabase.Controls.Add(this.labelProject);
            this.groupBoxDatabase.Controls.Add(this.comboBoxProject);
            this.groupBoxDatabase.Controls.Add(this.labelGroup);
            this.groupBoxDatabase.Controls.Add(this.comboBoxTaxonomicGroup);
            this.groupBoxDatabase.Enabled = false;
            this.groupBoxDatabase.Location = new System.Drawing.Point(12, 510);
            this.groupBoxDatabase.Name = "groupBoxDatabase";
            this.groupBoxDatabase.Size = new System.Drawing.Size(994, 175);
            this.groupBoxDatabase.TabIndex = 74;
            this.groupBoxDatabase.TabStop = false;
            this.groupBoxDatabase.Text = "Database";
            // 
            // labelIdentification
            // 
            this.labelIdentification.AutoSize = true;
            this.labelIdentification.Location = new System.Drawing.Point(150, 98);
            this.labelIdentification.Name = "labelIdentification";
            this.labelIdentification.Size = new System.Drawing.Size(70, 13);
            this.labelIdentification.TabIndex = 94;
            this.labelIdentification.Text = "Identification:";
            this.labelIdentification.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryIdentification
            // 
            this.userControlModuleRelatedEntryIdentification.Location = new System.Drawing.Point(223, 94);
            this.userControlModuleRelatedEntryIdentification.Module = null;
            this.userControlModuleRelatedEntryIdentification.Name = "userControlModuleRelatedEntryIdentification";
            this.userControlModuleRelatedEntryIdentification.Size = new System.Drawing.Size(545, 22);
            this.userControlModuleRelatedEntryIdentification.TabIndex = 93;
            // 
            // labelGazetteer
            // 
            this.labelGazetteer.AutoSize = true;
            this.labelGazetteer.Location = new System.Drawing.Point(133, 19);
            this.labelGazetteer.Name = "labelGazetteer";
            this.labelGazetteer.Size = new System.Drawing.Size(87, 13);
            this.labelGazetteer.TabIndex = 92;
            this.labelGazetteer.Text = "Locality or place:";
            this.labelGazetteer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryGazetteer
            // 
            this.userControlModuleRelatedEntryGazetteer.Location = new System.Drawing.Point(223, 15);
            this.userControlModuleRelatedEntryGazetteer.Module = null;
            this.userControlModuleRelatedEntryGazetteer.Name = "userControlModuleRelatedEntryGazetteer";
            this.userControlModuleRelatedEntryGazetteer.Size = new System.Drawing.Size(398, 22);
            this.userControlModuleRelatedEntryGazetteer.TabIndex = 91;
            // 
            // userControlModuleRelatedEntryExsiccate
            // 
            this.userControlModuleRelatedEntryExsiccate.Location = new System.Drawing.Point(223, 119);
            this.userControlModuleRelatedEntryExsiccate.Module = null;
            this.userControlModuleRelatedEntryExsiccate.Name = "userControlModuleRelatedEntryExsiccate";
            this.userControlModuleRelatedEntryExsiccate.Size = new System.Drawing.Size(545, 22);
            this.userControlModuleRelatedEntryExsiccate.TabIndex = 90;
            // 
            // labelExsiccate
            // 
            this.labelExsiccate.AutoSize = true;
            this.labelExsiccate.Location = new System.Drawing.Point(150, 123);
            this.labelExsiccate.Name = "labelExsiccate";
            this.labelExsiccate.Size = new System.Drawing.Size(56, 13);
            this.labelExsiccate.TabIndex = 89;
            this.labelExsiccate.Text = "Exsiccate:";
            this.labelExsiccate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLabelType
            // 
            this.labelLabelType.AutoSize = true;
            this.labelLabelType.Location = new System.Drawing.Point(794, 126);
            this.labelLabelType.Name = "labelLabelType";
            this.labelLabelType.Size = new System.Drawing.Size(59, 13);
            this.labelLabelType.TabIndex = 88;
            this.labelLabelType.Text = "Label type:";
            this.labelLabelType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxLabelType
            // 
            this.comboBoxLabelType.BackColor = System.Drawing.SystemColors.Window;
            this.comboBoxLabelType.FormattingEnabled = true;
            this.comboBoxLabelType.Location = new System.Drawing.Point(859, 120);
            this.comboBoxLabelType.Name = "comboBoxLabelType";
            this.comboBoxLabelType.Size = new System.Drawing.Size(126, 21);
            this.comboBoxLabelType.TabIndex = 87;
            // 
            // labelCollector
            // 
            this.labelCollector.AutoSize = true;
            this.labelCollector.Location = new System.Drawing.Point(169, 41);
            this.labelCollector.Name = "labelCollector";
            this.labelCollector.Size = new System.Drawing.Size(51, 13);
            this.labelCollector.TabIndex = 86;
            this.labelCollector.Text = "Collector:";
            this.labelCollector.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryCollector
            // 
            this.userControlModuleRelatedEntryCollector.Location = new System.Drawing.Point(223, 41);
            this.userControlModuleRelatedEntryCollector.Module = null;
            this.userControlModuleRelatedEntryCollector.Name = "userControlModuleRelatedEntryCollector";
            this.userControlModuleRelatedEntryCollector.Size = new System.Drawing.Size(398, 22);
            this.userControlModuleRelatedEntryCollector.TabIndex = 85;
            // 
            // labelAccessionDate
            // 
            this.labelAccessionDate.AutoSize = true;
            this.labelAccessionDate.Location = new System.Drawing.Point(635, 44);
            this.labelAccessionDate.Name = "labelAccessionDate";
            this.labelAccessionDate.Size = new System.Drawing.Size(83, 13);
            this.labelAccessionDate.TabIndex = 84;
            this.labelAccessionDate.Text = "Accession date:";
            this.labelAccessionDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlDatePanelAccessionDate
            // 
            this.userControlDatePanelAccessionDate.Location = new System.Drawing.Point(724, 41);
            this.userControlDatePanelAccessionDate.Name = "userControlDatePanelAccessionDate";
            this.userControlDatePanelAccessionDate.Size = new System.Drawing.Size(261, 21);
            this.userControlDatePanelAccessionDate.TabIndex = 83;
            // 
            // labelCollectionDate
            // 
            this.labelCollectionDate.AutoSize = true;
            this.labelCollectionDate.Location = new System.Drawing.Point(638, 19);
            this.labelCollectionDate.Name = "labelCollectionDate";
            this.labelCollectionDate.Size = new System.Drawing.Size(80, 13);
            this.labelCollectionDate.TabIndex = 82;
            this.labelCollectionDate.Text = "Collection date:";
            this.labelCollectionDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlDatePanelCollectionDate
            // 
            this.userControlDatePanelCollectionDate.Location = new System.Drawing.Point(724, 15);
            this.userControlDatePanelCollectionDate.Name = "userControlDatePanelCollectionDate";
            this.userControlDatePanelCollectionDate.Size = new System.Drawing.Size(261, 21);
            this.userControlDatePanelCollectionDate.TabIndex = 81;
            // 
            // buttonTestImport
            // 
            this.buttonTestImport.Location = new System.Drawing.Point(6, 50);
            this.buttonTestImport.Name = "buttonTestImport";
            this.buttonTestImport.Size = new System.Drawing.Size(88, 23);
            this.buttonTestImport.TabIndex = 80;
            this.buttonTestImport.Text = "Test Import";
            this.buttonTestImport.UseVisualStyleBackColor = true;
            this.buttonTestImport.Click += new System.EventHandler(this.buttonTestImport_Click);
            // 
            // textBoxWebFolder
            // 
            this.textBoxWebFolder.BackColor = System.Drawing.Color.Pink;
            this.textBoxWebFolder.Location = new System.Drawing.Point(498, 149);
            this.textBoxWebFolder.Name = "textBoxWebFolder";
            this.textBoxWebFolder.Size = new System.Drawing.Size(436, 20);
            this.textBoxWebFolder.TabIndex = 78;
            // 
            // buttonBrowseUrlFolder
            // 
            this.buttonBrowseUrlFolder.Location = new System.Drawing.Point(940, 147);
            this.buttonBrowseUrlFolder.Name = "buttonBrowseUrlFolder";
            this.buttonBrowseUrlFolder.Size = new System.Drawing.Size(45, 23);
            this.buttonBrowseUrlFolder.TabIndex = 77;
            this.buttonBrowseUrlFolder.UseVisualStyleBackColor = true;
            this.buttonBrowseUrlFolder.Click += new System.EventHandler(this.buttonBrowseUrlFolder_Click);
            // 
            // buttonStartImport
            // 
            this.buttonStartImport.Enabled = false;
            this.buttonStartImport.Location = new System.Drawing.Point(6, 79);
            this.buttonStartImport.Name = "buttonStartImport";
            this.buttonStartImport.Size = new System.Drawing.Size(88, 23);
            this.buttonStartImport.TabIndex = 76;
            this.buttonStartImport.Text = "Start Import";
            this.buttonStartImport.UseVisualStyleBackColor = true;
            this.buttonStartImport.Click += new System.EventHandler(this.buttonStartImport_Click);
            // 
            // labelImageType
            // 
            this.labelImageType.Location = new System.Drawing.Point(776, 66);
            this.labelImageType.Name = "labelImageType";
            this.labelImageType.Size = new System.Drawing.Size(77, 23);
            this.labelImageType.TabIndex = 75;
            this.labelImageType.Text = "Image type:";
            this.labelImageType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxImageType
            // 
            this.comboBoxImageType.BackColor = System.Drawing.Color.Pink;
            this.comboBoxImageType.Location = new System.Drawing.Point(859, 66);
            this.comboBoxImageType.MaxDropDownItems = 20;
            this.comboBoxImageType.Name = "comboBoxImageType";
            this.comboBoxImageType.Size = new System.Drawing.Size(126, 21);
            this.comboBoxImageType.TabIndex = 74;
            // 
            // labelDatabase
            // 
            this.labelDatabase.Location = new System.Drawing.Point(6, 15);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(114, 26);
            this.labelDatabase.TabIndex = 72;
            this.labelDatabase.Text = "Parameters for the import in the database";
            // 
            // checkBoxImageAsUri
            // 
            this.checkBoxImageAsUri.Checked = true;
            this.checkBoxImageAsUri.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxImageAsUri.Location = new System.Drawing.Point(6, 150);
            this.checkBoxImageAsUri.Name = "checkBoxImageAsUri";
            this.checkBoxImageAsUri.Size = new System.Drawing.Size(494, 20);
            this.checkBoxImageAsUri.TabIndex = 46;
            this.checkBoxImageAsUri.Text = "Import the image not with the local path as specified in Image target above but a" +
                "s URL. BaseURL:";
            this.checkBoxImageAsUri.CheckedChanged += new System.EventHandler(this.checkBoxImageAsUri_CheckedChanged);
            // 
            // comboBoxCollection
            // 
            this.comboBoxCollection.BackColor = System.Drawing.Color.Pink;
            this.comboBoxCollection.Location = new System.Drawing.Point(223, 66);
            this.comboBoxCollection.MaxDropDownItems = 20;
            this.comboBoxCollection.Name = "comboBoxCollection";
            this.comboBoxCollection.Size = new System.Drawing.Size(140, 21);
            this.comboBoxCollection.TabIndex = 36;
            // 
            // labelCollection
            // 
            this.labelCollection.Location = new System.Drawing.Point(156, 64);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(64, 23);
            this.labelCollection.TabIndex = 37;
            this.labelCollection.Text = "Collection:";
            this.labelCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMaterial
            // 
            this.labelMaterial.Location = new System.Drawing.Point(369, 68);
            this.labelMaterial.Name = "labelMaterial";
            this.labelMaterial.Size = new System.Drawing.Size(48, 23);
            this.labelMaterial.TabIndex = 39;
            this.labelMaterial.Text = "Material:";
            this.labelMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxMaterialCategory
            // 
            this.comboBoxMaterialCategory.BackColor = System.Drawing.Color.Pink;
            this.comboBoxMaterialCategory.Location = new System.Drawing.Point(423, 66);
            this.comboBoxMaterialCategory.MaxDropDownItems = 20;
            this.comboBoxMaterialCategory.Name = "comboBoxMaterialCategory";
            this.comboBoxMaterialCategory.Size = new System.Drawing.Size(134, 21);
            this.comboBoxMaterialCategory.TabIndex = 38;
            // 
            // labelProject
            // 
            this.labelProject.Location = new System.Drawing.Point(563, 68);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(47, 23);
            this.labelProject.TabIndex = 41;
            this.labelProject.Text = "Project:";
            this.labelProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxProject
            // 
            this.comboBoxProject.BackColor = System.Drawing.Color.Pink;
            this.comboBoxProject.Location = new System.Drawing.Point(616, 68);
            this.comboBoxProject.MaxDropDownItems = 20;
            this.comboBoxProject.Name = "comboBoxProject";
            this.comboBoxProject.Size = new System.Drawing.Size(152, 21);
            this.comboBoxProject.TabIndex = 40;
            // 
            // labelGroup
            // 
            this.labelGroup.Location = new System.Drawing.Point(774, 92);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(77, 23);
            this.labelGroup.TabIndex = 43;
            this.labelGroup.Text = "Taxon. group:";
            this.labelGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxTaxonomicGroup
            // 
            this.comboBoxTaxonomicGroup.BackColor = System.Drawing.Color.Pink;
            this.comboBoxTaxonomicGroup.Location = new System.Drawing.Point(859, 95);
            this.comboBoxTaxonomicGroup.MaxDropDownItems = 20;
            this.comboBoxTaxonomicGroup.Name = "comboBoxTaxonomicGroup";
            this.comboBoxTaxonomicGroup.Size = new System.Drawing.Size(126, 21);
            this.comboBoxTaxonomicGroup.TabIndex = 42;
            this.comboBoxTaxonomicGroup.SelectedIndexChanged += new System.EventHandler(this.comboBoxTaxonomicGroup_SelectedIndexChanged);
            // 
            // textBoxLogfile
            // 
            this.textBoxLogfile.Enabled = false;
            this.textBoxLogfile.Location = new System.Drawing.Point(204, 691);
            this.textBoxLogfile.Name = "textBoxLogfile";
            this.textBoxLogfile.Size = new System.Drawing.Size(802, 20);
            this.textBoxLogfile.TabIndex = 75;
            this.textBoxLogfile.Text = "ImportError.txt";
            // 
            // labelLogfile
            // 
            this.labelLogfile.Location = new System.Drawing.Point(116, 689);
            this.labelLogfile.Name = "labelLogfile";
            this.labelLogfile.Size = new System.Drawing.Size(96, 23);
            this.labelLogfile.TabIndex = 76;
            this.labelLogfile.Text = "Logfile for report:";
            this.labelLogfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxCreateLog
            // 
            this.checkBoxCreateLog.AutoSize = true;
            this.checkBoxCreateLog.Checked = true;
            this.checkBoxCreateLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCreateLog.Enabled = false;
            this.checkBoxCreateLog.Location = new System.Drawing.Point(18, 693);
            this.checkBoxCreateLog.Name = "checkBoxCreateLog";
            this.checkBoxCreateLog.Size = new System.Drawing.Size(90, 17);
            this.checkBoxCreateLog.TabIndex = 77;
            this.checkBoxCreateLog.Text = "Create log file";
            this.checkBoxCreateLog.UseVisualStyleBackColor = true;
            // 
            // dataSetCollectionSpecimen
            // 
            this.dataSetCollectionSpecimen.DataSetName = "DataSetCollectionSpecimen";
            this.dataSetCollectionSpecimen.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FormImportImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 719);
            this.Controls.Add(this.checkBoxCreateLog);
            this.Controls.Add(this.textBoxLogfile);
            this.Controls.Add(this.labelLogfile);
            this.Controls.Add(this.groupBoxDatabase);
            this.Controls.Add(this.dataGridViewImport);
            this.Controls.Add(this.groupBoxImageTarget);
            this.Controls.Add(this.groupBoxSecurityChecks);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.groupBoxImageSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.helpProvider.SetHelpKeyword(this, "Import scans");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Name = "FormImportImages";
            this.helpProvider.SetShowHelp(this, true);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import specimen scans";
            this.groupBoxImageSource.ResumeLayout(false);
            this.groupBoxImageSource.PerformLayout();
            this.groupBoxSecurityChecks.ResumeLayout(false);
            this.groupBoxSecurityChecks.PerformLayout();
            this.groupBoxImageTarget.ResumeLayout(false);
            this.groupBoxImageTarget.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubdirectry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImport)).EndInit();
            this.groupBoxDatabase.ResumeLayout(false);
            this.groupBoxDatabase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionSpecimen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionSpecimenBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionAgentBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.identificationUnitBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionGeographyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.identificationBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionEventBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxImageSource;
        private System.Windows.Forms.Label labelOriginal;
        private System.Windows.Forms.Button buttonFolderOriginal;
        private System.Windows.Forms.Label labelFolderOriginal;
        private System.Windows.Forms.TextBox textBoxFolderOriginal;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.GroupBox groupBoxSecurityChecks;
        private System.Windows.Forms.CheckBox checkBoxCutAfterSeparator;
        private System.Windows.Forms.CheckBox checkBoxAppendDateToFilename;
        private System.Windows.Forms.CheckBox checkBoxAppendImages;
        private System.Windows.Forms.CheckBox checkBoxOverwriteImages;
        private System.Windows.Forms.TextBox textBoxAccessionSeparator;
        private System.Windows.Forms.TextBox textBoxBarcodeStart;
        private System.Windows.Forms.TextBox textBoxBarcodeLength;
        private System.Windows.Forms.CheckBox checkBoxBarcodeStart;
        private System.Windows.Forms.Label labelBarcode;
        private System.Windows.Forms.CheckBox checkBoxBarcodeLength;
        private System.Windows.Forms.GroupBox groupBoxImageTarget;
        private System.Windows.Forms.DataGridView dataGridViewImport;
        private System.Windows.Forms.OpenFileDialog openFileDialogImport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogImport;
        private System.Windows.Forms.GroupBox groupBoxDatabase;
        private System.Windows.Forms.Label labelImageType;
        private System.Windows.Forms.ComboBox comboBoxImageType;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.RadioButton radioButtonPreviewImage;
        private System.Windows.Forms.Label labelChooseImageForImport;
        private System.Windows.Forms.CheckBox checkBoxImageAsUri;
        private System.Windows.Forms.RadioButton radioButtonOriginalImage;
        private System.Windows.Forms.RadioButton radioButtonWebImage;
        private System.Windows.Forms.ComboBox comboBoxCollection;
        private System.Windows.Forms.Label labelCollection;
        private System.Windows.Forms.Label labelMaterial;
        private System.Windows.Forms.ComboBox comboBoxMaterialCategory;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.ComboBox comboBoxTaxonomicGroup;
        private System.Windows.Forms.TextBox textBoxLogfile;
        private System.Windows.Forms.Label labelLogfile;
        private System.Windows.Forms.TextBox textBoxFolderArchive;
        private System.Windows.Forms.Button buttonFolderArchive;
        private System.Windows.Forms.Button buttonStartImport;
        private System.Windows.Forms.CheckBox checkBoxCreateLog;
        private System.Windows.Forms.Button buttonBrowseUrlFolder;
        private System.Windows.Forms.TextBox textBoxWebFolder;
        private System.Windows.Forms.RadioButton radioButtonArchiveImage;
        private System.Windows.Forms.Button buttonTestImport;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAccessionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOriginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnArchive;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWeb;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPreview;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnError;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnAppend;
        private System.Windows.Forms.CheckBox checkBoxSubdirectory;
        private System.Windows.Forms.Label labelSubdirectory;
        private System.Windows.Forms.NumericUpDown numericUpDownSubdirectry;
        private System.Windows.Forms.TextBox textBoxSubdirectory;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelCollectionDate;
        private System.Windows.Forms.Label labelAccessionDate;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelAccessionDate;
        private System.Windows.Forms.Label labelCollectionDate;
        private System.Windows.Forms.Label labelCollector;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryCollector;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryExsiccate;
        private System.Windows.Forms.Label labelExsiccate;
        private System.Windows.Forms.Label labelLabelType;
        private System.Windows.Forms.ComboBox comboBoxLabelType;
        private System.Windows.Forms.Label labelGazetteer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryGazetteer;
        private System.Windows.Forms.Label labelIdentification;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryIdentification;
        private System.Windows.Forms.BindingSource identificationUnitBindingSource;
        private System.Windows.Forms.BindingSource collectionSpecimenBindingSource;
        private System.Windows.Forms.BindingSource collectionAgentBindingSource;
        private System.Windows.Forms.BindingSource collectionGeographyBindingSource;
        private System.Windows.Forms.BindingSource identificationBindingSource;
        private System.Windows.Forms.BindingSource collectionEventBindingSource;
        private Datasets.DataSetCollectionSpecimen dataSetCollectionSpecimen;
        private System.Windows.Forms.CheckBox checkBoxArchiveImages;
    }
}