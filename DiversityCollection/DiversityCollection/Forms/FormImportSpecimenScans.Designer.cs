namespace DiversityCollection.Forms
{
    partial class FormImportSpecimenScans
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportSpecimenScans));
            this.labelHeader = new System.Windows.Forms.Label();
            this.groupBoxImageSource = new System.Windows.Forms.GroupBox();
            this.labelOriginal = new System.Windows.Forms.Label();
            this.buttonFolderOriginal = new System.Windows.Forms.Button();
            this.labelFolderOriginal = new System.Windows.Forms.Label();
            this.textBoxFolderOriginal = new System.Windows.Forms.TextBox();
            this.groupBoxSecurityChecks = new System.Windows.Forms.GroupBox();
            this.checkBoxCheckURIs = new System.Windows.Forms.CheckBox();
            this.numericUpDownSubfolder = new System.Windows.Forms.NumericUpDown();
            this.checkBoxSubfolder = new System.Windows.Forms.CheckBox();
            this.checkBoxCutAfterSeparator = new System.Windows.Forms.CheckBox();
            this.checkBoxOverwriteImages = new System.Windows.Forms.CheckBox();
            this.textBoxAccessionSeparator = new System.Windows.Forms.TextBox();
            this.textBoxBarcodeStart = new System.Windows.Forms.TextBox();
            this.textBoxBarcodeLength = new System.Windows.Forms.TextBox();
            this.checkBoxBarcodeStart = new System.Windows.Forms.CheckBox();
            this.labelBarcode = new System.Windows.Forms.Label();
            this.checkBoxBarcodeLength = new System.Windows.Forms.CheckBox();
            this.dataGridViewImport = new System.Windows.Forms.DataGridView();
            this.ColumnOK = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnAccessionNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPathInDB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnError = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAppend = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBoxDatabase = new System.Windows.Forms.GroupBox();
            this.checkBoxMoveTogether = new System.Windows.Forms.CheckBox();
            this.userControlModuleRelatedEntryDepositor = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelDepositor = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryIdentifiedBy = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelIdentifiedBy = new System.Windows.Forms.Label();
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
            this.checkBoxAppendImages = new System.Windows.Forms.CheckBox();
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
            this.checkBoxCreateLog = new System.Windows.Forms.CheckBox();
            this.textBoxLogfile = new System.Windows.Forms.TextBox();
            this.labelLogfile = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.openFileDialogImport = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialogImport = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.labelDeprecated = new System.Windows.Forms.Label();
            this.groupBoxImageSource.SuspendLayout();
            this.groupBoxSecurityChecks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubfolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImport)).BeginInit();
            this.groupBoxDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHeader
            // 
            this.labelHeader.Location = new System.Drawing.Point(9, 48);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(744, 16);
            this.labelHeader.TabIndex = 72;
            this.labelHeader.Text = "Import of image files of specimens. The file names should correspond to the acces" +
    "sion number of the specimen, e.g. M-00134403.jpg";
            // 
            // groupBoxImageSource
            // 
            this.groupBoxImageSource.Controls.Add(this.labelOriginal);
            this.groupBoxImageSource.Controls.Add(this.buttonFolderOriginal);
            this.groupBoxImageSource.Controls.Add(this.labelFolderOriginal);
            this.groupBoxImageSource.Controls.Add(this.textBoxFolderOriginal);
            this.groupBoxImageSource.Location = new System.Drawing.Point(12, 67);
            this.groupBoxImageSource.Name = "groupBoxImageSource";
            this.groupBoxImageSource.Size = new System.Drawing.Size(992, 51);
            this.groupBoxImageSource.TabIndex = 71;
            this.groupBoxImageSource.TabStop = false;
            this.groupBoxImageSource.Text = "Image source";
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
            this.buttonFolderOriginal.Image = global::DiversityCollection.Resource.Open;
            this.buttonFolderOriginal.Location = new System.Drawing.Point(165, 19);
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
            // groupBoxSecurityChecks
            // 
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxCheckURIs);
            this.groupBoxSecurityChecks.Controls.Add(this.numericUpDownSubfolder);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxSubfolder);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxCutAfterSeparator);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxOverwriteImages);
            this.groupBoxSecurityChecks.Controls.Add(this.textBoxAccessionSeparator);
            this.groupBoxSecurityChecks.Controls.Add(this.textBoxBarcodeStart);
            this.groupBoxSecurityChecks.Controls.Add(this.textBoxBarcodeLength);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxBarcodeStart);
            this.groupBoxSecurityChecks.Controls.Add(this.labelBarcode);
            this.groupBoxSecurityChecks.Controls.Add(this.checkBoxBarcodeLength);
            this.groupBoxSecurityChecks.Location = new System.Drawing.Point(12, 124);
            this.groupBoxSecurityChecks.Name = "groupBoxSecurityChecks";
            this.groupBoxSecurityChecks.Size = new System.Drawing.Size(992, 55);
            this.groupBoxSecurityChecks.TabIndex = 73;
            this.groupBoxSecurityChecks.TabStop = false;
            this.groupBoxSecurityChecks.Text = "Import options and security checks";
            // 
            // checkBoxCheckURIs
            // 
            this.checkBoxCheckURIs.Checked = true;
            this.checkBoxCheckURIs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCheckURIs.Location = new System.Drawing.Point(328, 14);
            this.checkBoxCheckURIs.Name = "checkBoxCheckURIs";
            this.checkBoxCheckURIs.Size = new System.Drawing.Size(104, 37);
            this.checkBoxCheckURIs.TabIndex = 60;
            this.checkBoxCheckURIs.Text = "Check the URI of the images";
            this.checkBoxCheckURIs.UseVisualStyleBackColor = true;
            // 
            // numericUpDownSubfolder
            // 
            this.numericUpDownSubfolder.Location = new System.Drawing.Point(253, 22);
            this.numericUpDownSubfolder.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDownSubfolder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSubfolder.Name = "numericUpDownSubfolder";
            this.numericUpDownSubfolder.Size = new System.Drawing.Size(35, 20);
            this.numericUpDownSubfolder.TabIndex = 59;
            this.numericUpDownSubfolder.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // checkBoxSubfolder
            // 
            this.checkBoxSubfolder.Checked = true;
            this.checkBoxSubfolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSubfolder.Location = new System.Drawing.Point(134, 16);
            this.checkBoxSubfolder.Name = "checkBoxSubfolder";
            this.checkBoxSubfolder.Size = new System.Drawing.Size(122, 33);
            this.checkBoxSubfolder.TabIndex = 58;
            this.checkBoxSubfolder.Text = "Place images in subfolder of length:";
            this.checkBoxSubfolder.UseVisualStyleBackColor = true;
            // 
            // checkBoxCutAfterSeparator
            // 
            this.checkBoxCutAfterSeparator.Checked = true;
            this.checkBoxCutAfterSeparator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCutAfterSeparator.Location = new System.Drawing.Point(461, 11);
            this.checkBoxCutAfterSeparator.Name = "checkBoxCutAfterSeparator";
            this.checkBoxCutAfterSeparator.Size = new System.Drawing.Size(117, 42);
            this.checkBoxCutAfterSeparator.TabIndex = 57;
            this.checkBoxCutAfterSeparator.Text = "Separator for accession number:";
            this.checkBoxCutAfterSeparator.UseVisualStyleBackColor = true;
            // 
            // checkBoxOverwriteImages
            // 
            this.checkBoxOverwriteImages.Location = new System.Drawing.Point(8, 15);
            this.checkBoxOverwriteImages.Name = "checkBoxOverwriteImages";
            this.checkBoxOverwriteImages.Size = new System.Drawing.Size(110, 35);
            this.checkBoxOverwriteImages.TabIndex = 45;
            this.checkBoxOverwriteImages.Text = "Overwrite existing images";
            // 
            // textBoxAccessionSeparator
            // 
            this.textBoxAccessionSeparator.Location = new System.Drawing.Point(584, 22);
            this.textBoxAccessionSeparator.Name = "textBoxAccessionSeparator";
            this.textBoxAccessionSeparator.Size = new System.Drawing.Size(24, 20);
            this.textBoxAccessionSeparator.TabIndex = 50;
            this.textBoxAccessionSeparator.Text = "_";
            this.textBoxAccessionSeparator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxBarcodeStart
            // 
            this.textBoxBarcodeStart.BackColor = System.Drawing.Color.Pink;
            this.textBoxBarcodeStart.Location = new System.Drawing.Point(795, 22);
            this.textBoxBarcodeStart.Name = "textBoxBarcodeStart";
            this.textBoxBarcodeStart.Size = new System.Drawing.Size(48, 20);
            this.textBoxBarcodeStart.TabIndex = 49;
            this.textBoxBarcodeStart.Text = "M-";
            this.textBoxBarcodeStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxBarcodeLength
            // 
            this.textBoxBarcodeLength.BackColor = System.Drawing.Color.Pink;
            this.textBoxBarcodeLength.Location = new System.Drawing.Point(938, 21);
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
            this.checkBoxBarcodeStart.Location = new System.Drawing.Point(712, 20);
            this.checkBoxBarcodeStart.Name = "checkBoxBarcodeStart";
            this.checkBoxBarcodeStart.Size = new System.Drawing.Size(88, 24);
            this.checkBoxBarcodeStart.TabIndex = 47;
            this.checkBoxBarcodeStart.Text = "Check start:";
            // 
            // labelBarcode
            // 
            this.labelBarcode.Location = new System.Drawing.Point(614, 11);
            this.labelBarcode.Name = "labelBarcode";
            this.labelBarcode.Size = new System.Drawing.Size(92, 38);
            this.labelBarcode.TabIndex = 54;
            this.labelBarcode.Text = "Checks for the accession Nr.:";
            this.labelBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxBarcodeLength
            // 
            this.checkBoxBarcodeLength.Checked = true;
            this.checkBoxBarcodeLength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBarcodeLength.Location = new System.Drawing.Point(849, 20);
            this.checkBoxBarcodeLength.Name = "checkBoxBarcodeLength";
            this.checkBoxBarcodeLength.Size = new System.Drawing.Size(96, 24);
            this.checkBoxBarcodeLength.TabIndex = 46;
            this.checkBoxBarcodeLength.Text = "Check length:";
            // 
            // dataGridViewImport
            // 
            this.dataGridViewImport.AllowUserToAddRows = false;
            this.dataGridViewImport.AllowUserToResizeRows = false;
            this.dataGridViewImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewImport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnOK,
            this.ColumnAccessionNumber,
            this.ColumnSource,
            this.ColumnPathInDB,
            this.ColumnError,
            this.ColumnAppend});
            this.dataGridViewImport.Location = new System.Drawing.Point(12, 185);
            this.dataGridViewImport.Name = "dataGridViewImport";
            this.dataGridViewImport.RowHeadersWidth = 24;
            this.dataGridViewImport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewImport.Size = new System.Drawing.Size(992, 291);
            this.dataGridViewImport.TabIndex = 74;
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
            // ColumnSource
            // 
            this.ColumnSource.Frozen = true;
            this.ColumnSource.HeaderText = "Source file";
            this.ColumnSource.Name = "ColumnSource";
            this.ColumnSource.ReadOnly = true;
            this.ColumnSource.Width = 300;
            // 
            // ColumnPathInDB
            // 
            this.ColumnPathInDB.Frozen = true;
            this.ColumnPathInDB.HeaderText = "Path in database";
            this.ColumnPathInDB.Name = "ColumnPathInDB";
            this.ColumnPathInDB.Width = 350;
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
            // groupBoxDatabase
            // 
            this.groupBoxDatabase.Controls.Add(this.checkBoxMoveTogether);
            this.groupBoxDatabase.Controls.Add(this.userControlModuleRelatedEntryDepositor);
            this.groupBoxDatabase.Controls.Add(this.labelDepositor);
            this.groupBoxDatabase.Controls.Add(this.userControlModuleRelatedEntryIdentifiedBy);
            this.groupBoxDatabase.Controls.Add(this.labelIdentifiedBy);
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
            this.groupBoxDatabase.Location = new System.Drawing.Point(10, 482);
            this.groupBoxDatabase.Name = "groupBoxDatabase";
            this.groupBoxDatabase.Size = new System.Drawing.Size(994, 175);
            this.groupBoxDatabase.TabIndex = 75;
            this.groupBoxDatabase.TabStop = false;
            this.groupBoxDatabase.Text = "Database";
            // 
            // checkBoxMoveTogether
            // 
            this.checkBoxMoveTogether.AutoSize = true;
            this.checkBoxMoveTogether.Checked = true;
            this.checkBoxMoveTogether.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMoveTogether.Location = new System.Drawing.Point(6, 135);
            this.checkBoxMoveTogether.Name = "checkBoxMoveTogether";
            this.checkBoxMoveTogether.Size = new System.Drawing.Size(270, 17);
            this.checkBoxMoveTogether.TabIndex = 99;
            this.checkBoxMoveTogether.Text = "Import identical accession numbers into one dataset";
            this.checkBoxMoveTogether.UseVisualStyleBackColor = true;
            // 
            // userControlModuleRelatedEntryDepositor
            // 
            this.userControlModuleRelatedEntryDepositor.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryDepositor.DependsOnUri = "";
            this.userControlModuleRelatedEntryDepositor.Domain = "";
            this.userControlModuleRelatedEntryDepositor.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDepositor.Location = new System.Drawing.Point(423, 40);
            this.userControlModuleRelatedEntryDepositor.Module = null;
            this.userControlModuleRelatedEntryDepositor.Name = "userControlModuleRelatedEntryDepositor";
            this.userControlModuleRelatedEntryDepositor.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDepositor.ShowInfo = false;
            this.userControlModuleRelatedEntryDepositor.Size = new System.Drawing.Size(198, 22);
            this.userControlModuleRelatedEntryDepositor.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDepositor.TabIndex = 98;
            // 
            // labelDepositor
            // 
            this.labelDepositor.AutoSize = true;
            this.labelDepositor.Location = new System.Drawing.Point(362, 44);
            this.labelDepositor.Name = "labelDepositor";
            this.labelDepositor.Size = new System.Drawing.Size(55, 13);
            this.labelDepositor.TabIndex = 97;
            this.labelDepositor.Text = "Depositor:";
            // 
            // userControlModuleRelatedEntryIdentifiedBy
            // 
            this.userControlModuleRelatedEntryIdentifiedBy.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryIdentifiedBy.DependsOnUri = "";
            this.userControlModuleRelatedEntryIdentifiedBy.Domain = "";
            this.userControlModuleRelatedEntryIdentifiedBy.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryIdentifiedBy.Location = new System.Drawing.Point(616, 94);
            this.userControlModuleRelatedEntryIdentifiedBy.Module = null;
            this.userControlModuleRelatedEntryIdentifiedBy.Name = "userControlModuleRelatedEntryIdentifiedBy";
            this.userControlModuleRelatedEntryIdentifiedBy.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryIdentifiedBy.ShowInfo = false;
            this.userControlModuleRelatedEntryIdentifiedBy.Size = new System.Drawing.Size(152, 22);
            this.userControlModuleRelatedEntryIdentifiedBy.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryIdentifiedBy.TabIndex = 96;
            // 
            // labelIdentifiedBy
            // 
            this.labelIdentifiedBy.AutoSize = true;
            this.labelIdentifiedBy.Location = new System.Drawing.Point(563, 98);
            this.labelIdentifiedBy.Name = "labelIdentifiedBy";
            this.labelIdentifiedBy.Size = new System.Drawing.Size(48, 13);
            this.labelIdentifiedBy.TabIndex = 95;
            this.labelIdentifiedBy.Text = "Ident.by:";
            // 
            // labelIdentification
            // 
            this.labelIdentification.AutoSize = true;
            this.labelIdentification.Location = new System.Drawing.Point(133, 98);
            this.labelIdentification.Name = "labelIdentification";
            this.labelIdentification.Size = new System.Drawing.Size(70, 13);
            this.labelIdentification.TabIndex = 94;
            this.labelIdentification.Text = "Identification:";
            this.labelIdentification.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryIdentification
            // 
            this.userControlModuleRelatedEntryIdentification.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryIdentification.DependsOnUri = "";
            this.userControlModuleRelatedEntryIdentification.Domain = "";
            this.userControlModuleRelatedEntryIdentification.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryIdentification.Location = new System.Drawing.Point(223, 94);
            this.userControlModuleRelatedEntryIdentification.Module = null;
            this.userControlModuleRelatedEntryIdentification.Name = "userControlModuleRelatedEntryIdentification";
            this.userControlModuleRelatedEntryIdentification.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryIdentification.ShowInfo = false;
            this.userControlModuleRelatedEntryIdentification.Size = new System.Drawing.Size(334, 22);
            this.userControlModuleRelatedEntryIdentification.SupressEmptyRemoteValues = false;
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
            this.userControlModuleRelatedEntryGazetteer.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryGazetteer.DependsOnUri = "";
            this.userControlModuleRelatedEntryGazetteer.Domain = "";
            this.userControlModuleRelatedEntryGazetteer.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryGazetteer.Location = new System.Drawing.Point(223, 15);
            this.userControlModuleRelatedEntryGazetteer.Module = null;
            this.userControlModuleRelatedEntryGazetteer.Name = "userControlModuleRelatedEntryGazetteer";
            this.userControlModuleRelatedEntryGazetteer.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryGazetteer.ShowInfo = false;
            this.userControlModuleRelatedEntryGazetteer.Size = new System.Drawing.Size(398, 22);
            this.userControlModuleRelatedEntryGazetteer.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryGazetteer.TabIndex = 91;
            // 
            // userControlModuleRelatedEntryExsiccate
            // 
            this.userControlModuleRelatedEntryExsiccate.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryExsiccate.DependsOnUri = "";
            this.userControlModuleRelatedEntryExsiccate.Domain = "";
            this.userControlModuleRelatedEntryExsiccate.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryExsiccate.Location = new System.Drawing.Point(423, 119);
            this.userControlModuleRelatedEntryExsiccate.Module = null;
            this.userControlModuleRelatedEntryExsiccate.Name = "userControlModuleRelatedEntryExsiccate";
            this.userControlModuleRelatedEntryExsiccate.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryExsiccate.ShowInfo = false;
            this.userControlModuleRelatedEntryExsiccate.Size = new System.Drawing.Size(345, 22);
            this.userControlModuleRelatedEntryExsiccate.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryExsiccate.TabIndex = 90;
            // 
            // labelExsiccate
            // 
            this.labelExsiccate.AutoSize = true;
            this.labelExsiccate.Location = new System.Drawing.Point(362, 123);
            this.labelExsiccate.Name = "labelExsiccate";
            this.labelExsiccate.Size = new System.Drawing.Size(56, 13);
            this.labelExsiccate.TabIndex = 89;
            this.labelExsiccate.Text = "Exsiccata:";
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
            this.labelCollector.Location = new System.Drawing.Point(133, 44);
            this.labelCollector.Name = "labelCollector";
            this.labelCollector.Size = new System.Drawing.Size(51, 13);
            this.labelCollector.TabIndex = 86;
            this.labelCollector.Text = "Collector:";
            this.labelCollector.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryCollector
            // 
            this.userControlModuleRelatedEntryCollector.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryCollector.DependsOnUri = "";
            this.userControlModuleRelatedEntryCollector.Domain = "";
            this.userControlModuleRelatedEntryCollector.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryCollector.Location = new System.Drawing.Point(190, 40);
            this.userControlModuleRelatedEntryCollector.Module = null;
            this.userControlModuleRelatedEntryCollector.Name = "userControlModuleRelatedEntryCollector";
            this.userControlModuleRelatedEntryCollector.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryCollector.ShowInfo = false;
            this.userControlModuleRelatedEntryCollector.Size = new System.Drawing.Size(173, 22);
            this.userControlModuleRelatedEntryCollector.SupressEmptyRemoteValues = false;
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
            this.textBoxWebFolder.Location = new System.Drawing.Point(423, 149);
            this.textBoxWebFolder.Name = "textBoxWebFolder";
            this.textBoxWebFolder.Size = new System.Drawing.Size(511, 20);
            this.textBoxWebFolder.TabIndex = 78;
            // 
            // buttonBrowseUrlFolder
            // 
            this.buttonBrowseUrlFolder.Image = global::DiversityCollection.Resource.Browse;
            this.buttonBrowseUrlFolder.Location = new System.Drawing.Point(940, 147);
            this.buttonBrowseUrlFolder.Name = "buttonBrowseUrlFolder";
            this.buttonBrowseUrlFolder.Size = new System.Drawing.Size(45, 24);
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
            // checkBoxAppendImages
            // 
            this.checkBoxAppendImages.AutoSize = true;
            this.checkBoxAppendImages.Location = new System.Drawing.Point(6, 118);
            this.checkBoxAppendImages.Name = "checkBoxAppendImages";
            this.checkBoxAppendImages.Size = new System.Drawing.Size(244, 17);
            this.checkBoxAppendImages.TabIndex = 55;
            this.checkBoxAppendImages.Text = "Append images if accession number is present";
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
            this.checkBoxImageAsUri.Size = new System.Drawing.Size(428, 20);
            this.checkBoxImageAsUri.TabIndex = 46;
            this.checkBoxImageAsUri.Text = "Import the image not with the original path as specified above but as URL. BaseUR" +
    "L:";
            // 
            // comboBoxCollection
            // 
            this.comboBoxCollection.BackColor = System.Drawing.Color.Pink;
            this.comboBoxCollection.Location = new System.Drawing.Point(190, 70);
            this.comboBoxCollection.MaxDropDownItems = 20;
            this.comboBoxCollection.Name = "comboBoxCollection";
            this.comboBoxCollection.Size = new System.Drawing.Size(173, 21);
            this.comboBoxCollection.TabIndex = 36;
            // 
            // labelCollection
            // 
            this.labelCollection.Location = new System.Drawing.Point(133, 68);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(57, 23);
            this.labelCollection.TabIndex = 37;
            this.labelCollection.Text = "Collection:";
            this.labelCollection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.comboBoxMaterialCategory.Location = new System.Drawing.Point(423, 68);
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
            // 
            // checkBoxCreateLog
            // 
            this.checkBoxCreateLog.AutoSize = true;
            this.checkBoxCreateLog.Checked = true;
            this.checkBoxCreateLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCreateLog.Enabled = false;
            this.checkBoxCreateLog.Location = new System.Drawing.Point(10, 665);
            this.checkBoxCreateLog.Name = "checkBoxCreateLog";
            this.checkBoxCreateLog.Size = new System.Drawing.Size(90, 17);
            this.checkBoxCreateLog.TabIndex = 80;
            this.checkBoxCreateLog.Text = "Create log file";
            this.checkBoxCreateLog.UseVisualStyleBackColor = true;
            // 
            // textBoxLogfile
            // 
            this.textBoxLogfile.Enabled = false;
            this.textBoxLogfile.Location = new System.Drawing.Point(196, 663);
            this.textBoxLogfile.Name = "textBoxLogfile";
            this.textBoxLogfile.Size = new System.Drawing.Size(802, 20);
            this.textBoxLogfile.TabIndex = 78;
            this.textBoxLogfile.Text = "ImportError.txt";
            // 
            // labelLogfile
            // 
            this.labelLogfile.Location = new System.Drawing.Point(108, 661);
            this.labelLogfile.Name = "labelLogfile";
            this.labelLogfile.Size = new System.Drawing.Size(96, 23);
            this.labelLogfile.TabIndex = 79;
            this.labelLogfile.Text = "Logfile for report:";
            this.labelLogfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialogImport
            // 
            this.openFileDialogImport.FileName = "openFileDialogImport";
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(980, 41);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(24, 23);
            this.buttonFeedback.TabIndex = 81;
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // labelDeprecated
            // 
            this.labelDeprecated.AutoSize = true;
            this.labelDeprecated.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeprecated.ForeColor = System.Drawing.Color.Red;
            this.labelDeprecated.Location = new System.Drawing.Point(8, 18);
            this.labelDeprecated.Name = "labelDeprecated";
            this.labelDeprecated.Size = new System.Drawing.Size(664, 20);
            this.labelDeprecated.TabIndex = 82;
            this.labelDeprecated.Text = "Deprecated - only kept for compatibility reasons. Please use import wizard instea" +
    "d";
            // 
            // FormImportSpecimenScans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 694);
            this.Controls.Add(this.labelDeprecated);
            this.Controls.Add(this.buttonFeedback);
            this.Controls.Add(this.checkBoxCreateLog);
            this.Controls.Add(this.textBoxLogfile);
            this.Controls.Add(this.labelLogfile);
            this.Controls.Add(this.groupBoxDatabase);
            this.Controls.Add(this.dataGridViewImport);
            this.Controls.Add(this.groupBoxSecurityChecks);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.groupBoxImageSource);
            this.helpProvider.SetHelpKeyword(this, "Import images");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImportSpecimenScans";
            this.helpProvider.SetShowHelp(this, true);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import scans of specimens";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormImportSpecimenScans_FormClosing);
            this.groupBoxImageSource.ResumeLayout(false);
            this.groupBoxImageSource.PerformLayout();
            this.groupBoxSecurityChecks.ResumeLayout(false);
            this.groupBoxSecurityChecks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubfolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewImport)).EndInit();
            this.groupBoxDatabase.ResumeLayout(false);
            this.groupBoxDatabase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.GroupBox groupBoxImageSource;
        private System.Windows.Forms.Label labelOriginal;
        private System.Windows.Forms.Button buttonFolderOriginal;
        private System.Windows.Forms.Label labelFolderOriginal;
        private System.Windows.Forms.TextBox textBoxFolderOriginal;
        private System.Windows.Forms.GroupBox groupBoxSecurityChecks;
        private System.Windows.Forms.CheckBox checkBoxCutAfterSeparator;
        private System.Windows.Forms.CheckBox checkBoxOverwriteImages;
        private System.Windows.Forms.TextBox textBoxAccessionSeparator;
        private System.Windows.Forms.TextBox textBoxBarcodeStart;
        private System.Windows.Forms.TextBox textBoxBarcodeLength;
        private System.Windows.Forms.CheckBox checkBoxBarcodeStart;
        private System.Windows.Forms.Label labelBarcode;
        private System.Windows.Forms.CheckBox checkBoxBarcodeLength;
        private System.Windows.Forms.DataGridView dataGridViewImport;
        private System.Windows.Forms.GroupBox groupBoxDatabase;
        private System.Windows.Forms.Label labelIdentification;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryIdentification;
        private System.Windows.Forms.Label labelGazetteer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryGazetteer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryExsiccate;
        private System.Windows.Forms.Label labelExsiccate;
        private System.Windows.Forms.Label labelLabelType;
        private System.Windows.Forms.ComboBox comboBoxLabelType;
        private System.Windows.Forms.Label labelCollector;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryCollector;
        private System.Windows.Forms.Label labelAccessionDate;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelAccessionDate;
        private System.Windows.Forms.Label labelCollectionDate;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelCollectionDate;
        private System.Windows.Forms.Button buttonTestImport;
        private System.Windows.Forms.TextBox textBoxWebFolder;
        private System.Windows.Forms.Button buttonBrowseUrlFolder;
        private System.Windows.Forms.Button buttonStartImport;
        private System.Windows.Forms.Label labelImageType;
        private System.Windows.Forms.ComboBox comboBoxImageType;
        private System.Windows.Forms.CheckBox checkBoxAppendImages;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.CheckBox checkBoxImageAsUri;
        private System.Windows.Forms.ComboBox comboBoxCollection;
        private System.Windows.Forms.Label labelCollection;
        private System.Windows.Forms.Label labelMaterial;
        private System.Windows.Forms.ComboBox comboBoxMaterialCategory;
        private System.Windows.Forms.Label labelProject;
        private System.Windows.Forms.ComboBox comboBoxProject;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.ComboBox comboBoxTaxonomicGroup;
        private System.Windows.Forms.CheckBox checkBoxCreateLog;
        private System.Windows.Forms.TextBox textBoxLogfile;
        private System.Windows.Forms.Label labelLogfile;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.OpenFileDialog openFileDialogImport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogImport;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAccessionNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPathInDB;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnError;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnAppend;
        private System.Windows.Forms.CheckBox checkBoxSubfolder;
        private System.Windows.Forms.NumericUpDown numericUpDownSubfolder;
        private System.Windows.Forms.CheckBox checkBoxCheckURIs;
        private System.Windows.Forms.Label labelIdentifiedBy;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryIdentifiedBy;
        private System.Windows.Forms.Button buttonFeedback;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDepositor;
        private System.Windows.Forms.Label labelDepositor;
        private System.Windows.Forms.CheckBox checkBoxMoveTogether;
        private System.Windows.Forms.Label labelDeprecated;
    }
}