namespace DiversityCollection.UserControls
{
    partial class UserControl_Specimen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Specimen));
            this.groupBoxAccession = new System.Windows.Forms.GroupBox();
            this.pictureBoxSpecimen = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelAccession = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxExternalSource = new System.Windows.Forms.ComboBox();
            this.comboBoxCollectionSpecimenDataWithholdingReason = new System.Windows.Forms.ComboBox();
            this.textBoxExternalSourceID = new System.Windows.Forms.TextBox();
            this.labelExternalSourceID = new System.Windows.Forms.Label();
            this.labelCollectionSpecimenDataWithholdingReason = new System.Windows.Forms.Label();
            this.comboBoxAccessionDateCategory = new System.Windows.Forms.ComboBox();
            this.labelAccessionDateCategory = new System.Windows.Forms.Label();
            this.labelAccessionDate = new System.Windows.Forms.Label();
            this.labelSpecimenReference = new System.Windows.Forms.Label();
            this.textBoxDepositorsNr = new System.Windows.Forms.TextBox();
            this.labelDepositorsNr = new System.Windows.Forms.Label();
            this.labelDepositor = new System.Windows.Forms.Label();
            this.buttonFindNextAccessionNumber = new System.Windows.Forms.Button();
            this.textBoxAccessionNumber = new System.Windows.Forms.TextBox();
            this.labelAccessionNumber = new System.Windows.Forms.Label();
            this.labelExternalSource = new System.Windows.Forms.Label();
            this.labelRevisionState = new System.Windows.Forms.Label();
            this.comboBoxRevisionState = new System.Windows.Forms.ComboBox();
            this.buttonGoToDuplicateAccessionNumber = new System.Windows.Forms.Button();
            this.labelSpecimenReferenceDetails = new System.Windows.Forms.Label();
            this.textBoxSpecimenReferenceDetails = new System.Windows.Forms.TextBox();
            this.buttonTemplateSpecimenSet = new System.Windows.Forms.Button();
            this.buttonTemplateSpecimenEdit = new System.Windows.Forms.Button();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelExsiccataSeries = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxExsiccata = new System.Windows.Forms.PictureBox();
            this.labelExsiccataAbbreviation = new System.Windows.Forms.Label();
            this.buttonAccessionNumberEdit = new System.Windows.Forms.Button();
            this.userControlModuleRelatedEntrySpecimenReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlDatePanelAccessionDate = new DiversityWorkbench.UserControls.UserControlDatePanel();
            this.userControlModuleRelatedEntryDepositor = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlHierarchySelectorAccessionDateCategory = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            this.userControlModuleRelatedEntryExsiccate = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.groupBoxAccession.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimen)).BeginInit();
            this.tableLayoutPanelAccession.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelExsiccataSeries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExsiccata)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxAccession
            // 
            this.groupBoxAccession.AccessibleName = "CollectionSpecimen";
            this.groupBoxAccession.Controls.Add(this.pictureBoxSpecimen);
            this.groupBoxAccession.Controls.Add(this.tableLayoutPanelAccession);
            this.groupBoxAccession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAccession.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxAccession.ForeColor = System.Drawing.Color.Black;
            this.groupBoxAccession.Location = new System.Drawing.Point(0, 0);
            this.groupBoxAccession.MinimumSize = new System.Drawing.Size(0, 150);
            this.groupBoxAccession.Name = "groupBoxAccession";
            this.groupBoxAccession.Size = new System.Drawing.Size(671, 150);
            this.groupBoxAccession.TabIndex = 29;
            this.groupBoxAccession.TabStop = false;
            this.groupBoxAccession.Text = "Collection specimen";
            // 
            // pictureBoxSpecimen
            // 
            this.pictureBoxSpecimen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxSpecimen.Image = global::DiversityCollection.Resource.CollectionSpecimen;
            this.pictureBoxSpecimen.Location = new System.Drawing.Point(643, 1);
            this.pictureBoxSpecimen.Name = "pictureBoxSpecimen";
            this.pictureBoxSpecimen.Size = new System.Drawing.Size(16, 15);
            this.pictureBoxSpecimen.TabIndex = 1;
            this.pictureBoxSpecimen.TabStop = false;
            // 
            // tableLayoutPanelAccession
            // 
            this.tableLayoutPanelAccession.ColumnCount = 8;
            this.tableLayoutPanelAccession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanelAccession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAccession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAccession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelAccession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAccession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanelAccession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanelAccession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelAccession.Controls.Add(this.comboBoxExternalSource, 2, 2);
            this.tableLayoutPanelAccession.Controls.Add(this.comboBoxCollectionSpecimenDataWithholdingReason, 6, 4);
            this.tableLayoutPanelAccession.Controls.Add(this.textBoxExternalSourceID, 5, 2);
            this.tableLayoutPanelAccession.Controls.Add(this.labelExternalSourceID, 4, 2);
            this.tableLayoutPanelAccession.Controls.Add(this.labelCollectionSpecimenDataWithholdingReason, 4, 4);
            this.tableLayoutPanelAccession.Controls.Add(this.userControlModuleRelatedEntrySpecimenReference, 2, 5);
            this.tableLayoutPanelAccession.Controls.Add(this.comboBoxAccessionDateCategory, 6, 3);
            this.tableLayoutPanelAccession.Controls.Add(this.labelAccessionDateCategory, 5, 3);
            this.tableLayoutPanelAccession.Controls.Add(this.labelAccessionDate, 1, 3);
            this.tableLayoutPanelAccession.Controls.Add(this.labelSpecimenReference, 1, 5);
            this.tableLayoutPanelAccession.Controls.Add(this.textBoxDepositorsNr, 5, 1);
            this.tableLayoutPanelAccession.Controls.Add(this.labelDepositorsNr, 4, 1);
            this.tableLayoutPanelAccession.Controls.Add(this.labelDepositor, 1, 1);
            this.tableLayoutPanelAccession.Controls.Add(this.buttonFindNextAccessionNumber, 5, 0);
            this.tableLayoutPanelAccession.Controls.Add(this.textBoxAccessionNumber, 2, 0);
            this.tableLayoutPanelAccession.Controls.Add(this.labelAccessionNumber, 1, 0);
            this.tableLayoutPanelAccession.Controls.Add(this.userControlDatePanelAccessionDate, 2, 3);
            this.tableLayoutPanelAccession.Controls.Add(this.userControlModuleRelatedEntryDepositor, 2, 1);
            this.tableLayoutPanelAccession.Controls.Add(this.userControlHierarchySelectorAccessionDateCategory, 7, 3);
            this.tableLayoutPanelAccession.Controls.Add(this.labelExternalSource, 1, 2);
            this.tableLayoutPanelAccession.Controls.Add(this.labelRevisionState, 1, 4);
            this.tableLayoutPanelAccession.Controls.Add(this.comboBoxRevisionState, 2, 4);
            this.tableLayoutPanelAccession.Controls.Add(this.buttonGoToDuplicateAccessionNumber, 4, 0);
            this.tableLayoutPanelAccession.Controls.Add(this.labelSpecimenReferenceDetails, 5, 5);
            this.tableLayoutPanelAccession.Controls.Add(this.textBoxSpecimenReferenceDetails, 6, 5);
            this.tableLayoutPanelAccession.Controls.Add(this.buttonTemplateSpecimenSet, 0, 0);
            this.tableLayoutPanelAccession.Controls.Add(this.buttonTemplateSpecimenEdit, 0, 1);
            this.tableLayoutPanelAccession.Controls.Add(this.buttonAccessionNumberEdit, 3, 0);
            this.tableLayoutPanelAccession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAccession.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelAccession.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelAccession.Name = "tableLayoutPanelAccession";
            this.tableLayoutPanelAccession.RowCount = 8;
            this.tableLayoutPanelAccession.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAccession.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAccession.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAccession.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAccession.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAccession.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAccession.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelAccession.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAccession.Size = new System.Drawing.Size(665, 131);
            this.tableLayoutPanelAccession.TabIndex = 0;
            // 
            // comboBoxExternalSource
            // 
            this.comboBoxExternalSource.AccessibleName = "CollectionSpecimen.ExternalDatasourceID";
            this.tableLayoutPanelAccession.SetColumnSpan(this.comboBoxExternalSource, 2);
            this.comboBoxExternalSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxExternalSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExternalSource.FormattingEnabled = true;
            this.comboBoxExternalSource.Location = new System.Drawing.Point(74, 47);
            this.comboBoxExternalSource.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.comboBoxExternalSource.Name = "comboBoxExternalSource";
            this.comboBoxExternalSource.Size = new System.Drawing.Size(407, 21);
            this.comboBoxExternalSource.TabIndex = 0;
            // 
            // comboBoxCollectionSpecimenDataWithholdingReason
            // 
            this.tableLayoutPanelAccession.SetColumnSpan(this.comboBoxCollectionSpecimenDataWithholdingReason, 2);
            this.comboBoxCollectionSpecimenDataWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollectionSpecimenDataWithholdingReason.FormattingEnabled = true;
            this.comboBoxCollectionSpecimenDataWithholdingReason.Location = new System.Drawing.Point(579, 97);
            this.comboBoxCollectionSpecimenDataWithholdingReason.Margin = new System.Windows.Forms.Padding(0, 3, 3, 1);
            this.comboBoxCollectionSpecimenDataWithholdingReason.Name = "comboBoxCollectionSpecimenDataWithholdingReason";
            this.comboBoxCollectionSpecimenDataWithholdingReason.Size = new System.Drawing.Size(83, 21);
            this.comboBoxCollectionSpecimenDataWithholdingReason.TabIndex = 3;
            this.comboBoxCollectionSpecimenDataWithholdingReason.DropDown += new System.EventHandler(this.comboBoxCollectionSpecimenDataWithholdingReason_DropDown);
            // 
            // textBoxExternalSourceID
            // 
            this.textBoxExternalSourceID.AccessibleName = "CollectionSpecimen.ExternalIdentifier";
            this.tableLayoutPanelAccession.SetColumnSpan(this.textBoxExternalSourceID, 3);
            this.textBoxExternalSourceID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExternalSourceID.Location = new System.Drawing.Point(543, 47);
            this.textBoxExternalSourceID.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxExternalSourceID.Name = "textBoxExternalSourceID";
            this.textBoxExternalSourceID.Size = new System.Drawing.Size(119, 20);
            this.textBoxExternalSourceID.TabIndex = 2;
            // 
            // labelExternalSourceID
            // 
            this.labelExternalSourceID.AccessibleName = "CollectionSpecimen.ExternalIdentifier";
            this.labelExternalSourceID.AutoSize = true;
            this.labelExternalSourceID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExternalSourceID.Location = new System.Drawing.Point(487, 47);
            this.labelExternalSourceID.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelExternalSourceID.Name = "labelExternalSourceID";
            this.labelExternalSourceID.Size = new System.Drawing.Size(56, 24);
            this.labelExternalSourceID.TabIndex = 1;
            this.labelExternalSourceID.Text = "Ext. ID.:";
            this.labelExternalSourceID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCollectionSpecimenDataWithholdingReason
            // 
            this.labelCollectionSpecimenDataWithholdingReason.AccessibleName = "CollectionSpecimen.DataWithholdingReason";
            this.labelCollectionSpecimenDataWithholdingReason.AutoSize = true;
            this.tableLayoutPanelAccession.SetColumnSpan(this.labelCollectionSpecimenDataWithholdingReason, 2);
            this.labelCollectionSpecimenDataWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionSpecimenDataWithholdingReason.Location = new System.Drawing.Point(487, 94);
            this.labelCollectionSpecimenDataWithholdingReason.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCollectionSpecimenDataWithholdingReason.Name = "labelCollectionSpecimenDataWithholdingReason";
            this.labelCollectionSpecimenDataWithholdingReason.Size = new System.Drawing.Size(92, 25);
            this.labelCollectionSpecimenDataWithholdingReason.TabIndex = 2;
            this.labelCollectionSpecimenDataWithholdingReason.Text = "Withhold. reason:";
            this.labelCollectionSpecimenDataWithholdingReason.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxAccessionDateCategory
            // 
            this.comboBoxAccessionDateCategory.AccessibleName = "CollectionSpecimen.AccessionDateCategory";
            this.comboBoxAccessionDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAccessionDateCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAccessionDateCategory.DropDownWidth = 200;
            this.comboBoxAccessionDateCategory.Location = new System.Drawing.Point(579, 71);
            this.comboBoxAccessionDateCategory.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.comboBoxAccessionDateCategory.MaxDropDownItems = 20;
            this.comboBoxAccessionDateCategory.Name = "comboBoxAccessionDateCategory";
            this.comboBoxAccessionDateCategory.Size = new System.Drawing.Size(68, 21);
            this.comboBoxAccessionDateCategory.TabIndex = 15;
            // 
            // labelAccessionDateCategory
            // 
            this.labelAccessionDateCategory.AccessibleName = "CollectionSpecimen.AccessionDateCategory";
            this.labelAccessionDateCategory.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAccessionDateCategory.Location = new System.Drawing.Point(546, 71);
            this.labelAccessionDateCategory.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAccessionDateCategory.Name = "labelAccessionDateCategory";
            this.labelAccessionDateCategory.Size = new System.Drawing.Size(33, 20);
            this.labelAccessionDateCategory.TabIndex = 40;
            this.labelAccessionDateCategory.Text = "Cat.:";
            this.labelAccessionDateCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAccessionDate
            // 
            this.labelAccessionDate.AccessibleName = "CollectionSpecimen.AccessionDate";
            this.labelAccessionDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAccessionDate.Location = new System.Drawing.Point(18, 71);
            this.labelAccessionDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAccessionDate.Name = "labelAccessionDate";
            this.labelAccessionDate.Size = new System.Drawing.Size(56, 20);
            this.labelAccessionDate.TabIndex = 39;
            this.labelAccessionDate.Text = "Acc.date:";
            this.labelAccessionDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSpecimenReference
            // 
            this.labelSpecimenReference.AccessibleName = "CollectionSpecimen.ReferenceTitle";
            this.labelSpecimenReference.AutoSize = true;
            this.labelSpecimenReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpecimenReference.Location = new System.Drawing.Point(18, 122);
            this.labelSpecimenReference.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelSpecimenReference.Name = "labelSpecimenReference";
            this.labelSpecimenReference.Size = new System.Drawing.Size(56, 22);
            this.labelSpecimenReference.TabIndex = 4;
            this.labelSpecimenReference.Text = "Ref.:";
            this.labelSpecimenReference.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelSpecimenReference.Visible = false;
            // 
            // textBoxDepositorsNr
            // 
            this.textBoxDepositorsNr.AccessibleName = "CollectionSpecimen.DepositorsAccessionNumber";
            this.tableLayoutPanelAccession.SetColumnSpan(this.textBoxDepositorsNr, 3);
            this.textBoxDepositorsNr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDepositorsNr.Location = new System.Drawing.Point(543, 23);
            this.textBoxDepositorsNr.Margin = new System.Windows.Forms.Padding(0, 0, 3, 1);
            this.textBoxDepositorsNr.Name = "textBoxDepositorsNr";
            this.textBoxDepositorsNr.Size = new System.Drawing.Size(119, 20);
            this.textBoxDepositorsNr.TabIndex = 17;
            // 
            // labelDepositorsNr
            // 
            this.labelDepositorsNr.AccessibleName = "CollectionSpecimen.DepositorsAccessionNumber";
            this.labelDepositorsNr.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDepositorsNr.Location = new System.Drawing.Point(487, 23);
            this.labelDepositorsNr.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDepositorsNr.Name = "labelDepositorsNr";
            this.labelDepositorsNr.Size = new System.Drawing.Size(56, 20);
            this.labelDepositorsNr.TabIndex = 37;
            this.labelDepositorsNr.Text = "Dep.No:";
            this.labelDepositorsNr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDepositor
            // 
            this.labelDepositor.AccessibleName = "CollectionSpecimen.DepositorsName";
            this.labelDepositor.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDepositor.Location = new System.Drawing.Point(18, 23);
            this.labelDepositor.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDepositor.Name = "labelDepositor";
            this.labelDepositor.Size = new System.Drawing.Size(56, 20);
            this.labelDepositor.TabIndex = 36;
            this.labelDepositor.Text = "Depositor:";
            this.labelDepositor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonFindNextAccessionNumber
            // 
            this.buttonFindNextAccessionNumber.AccessibleName = "CollectionSpecimen.AccessionNumber";
            this.tableLayoutPanelAccession.SetColumnSpan(this.buttonFindNextAccessionNumber, 3);
            this.buttonFindNextAccessionNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonFindNextAccessionNumber.Location = new System.Drawing.Point(546, 0);
            this.buttonFindNextAccessionNumber.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.buttonFindNextAccessionNumber.Name = "buttonFindNextAccessionNumber";
            this.buttonFindNextAccessionNumber.Size = new System.Drawing.Size(116, 21);
            this.buttonFindNextAccessionNumber.TabIndex = 33;
            this.buttonFindNextAccessionNumber.Text = "Find next No.";
            this.buttonFindNextAccessionNumber.Click += new System.EventHandler(this.buttonFindNextAccessionNumber_Click);
            // 
            // textBoxAccessionNumber
            // 
            this.textBoxAccessionNumber.AccessibleName = "CollectionSpecimen.AccessionNumber";
            this.textBoxAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAccessionNumber.Location = new System.Drawing.Point(74, 0);
            this.textBoxAccessionNumber.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.textBoxAccessionNumber.Name = "textBoxAccessionNumber";
            this.textBoxAccessionNumber.Size = new System.Drawing.Size(392, 20);
            this.textBoxAccessionNumber.TabIndex = 12;
            this.textBoxAccessionNumber.TextChanged += new System.EventHandler(this.textBoxAccessionNumber_TextChanged);
            this.textBoxAccessionNumber.Leave += new System.EventHandler(this.textBoxAccessionNumber_Leave);
            // 
            // labelAccessionNumber
            // 
            this.labelAccessionNumber.AccessibleName = "CollectionSpecimen.AccessionNumber";
            this.labelAccessionNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAccessionNumber.Location = new System.Drawing.Point(18, 0);
            this.labelAccessionNumber.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAccessionNumber.Name = "labelAccessionNumber";
            this.labelAccessionNumber.Size = new System.Drawing.Size(56, 20);
            this.labelAccessionNumber.TabIndex = 15;
            this.labelAccessionNumber.Text = "Number:";
            this.labelAccessionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelExternalSource
            // 
            this.labelExternalSource.AccessibleName = "CollectionSpecimen.ExternalDatasourceID";
            this.labelExternalSource.AutoSize = true;
            this.labelExternalSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExternalSource.Location = new System.Drawing.Point(15, 47);
            this.labelExternalSource.Margin = new System.Windows.Forms.Padding(0);
            this.labelExternalSource.Name = "labelExternalSource";
            this.labelExternalSource.Size = new System.Drawing.Size(59, 24);
            this.labelExternalSource.TabIndex = 43;
            this.labelExternalSource.Text = "Source:";
            this.labelExternalSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelRevisionState
            // 
            this.labelRevisionState.AccessibleName = "CollectionSpecimen.LabelTranscriptionState";
            this.labelRevisionState.AutoSize = true;
            this.labelRevisionState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRevisionState.Location = new System.Drawing.Point(18, 97);
            this.labelRevisionState.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelRevisionState.Name = "labelRevisionState";
            this.labelRevisionState.Size = new System.Drawing.Size(53, 22);
            this.labelRevisionState.TabIndex = 44;
            this.labelRevisionState.Text = "State:";
            this.labelRevisionState.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxRevisionState
            // 
            this.comboBoxRevisionState.AccessibleName = "CollectionSpecimen.LabelTranscriptionState";
            this.tableLayoutPanelAccession.SetColumnSpan(this.comboBoxRevisionState, 2);
            this.comboBoxRevisionState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxRevisionState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRevisionState.DropDownWidth = 150;
            this.comboBoxRevisionState.FormattingEnabled = true;
            this.comboBoxRevisionState.Location = new System.Drawing.Point(74, 95);
            this.comboBoxRevisionState.Margin = new System.Windows.Forms.Padding(0, 1, 3, 3);
            this.comboBoxRevisionState.MaxDropDownItems = 20;
            this.comboBoxRevisionState.Name = "comboBoxRevisionState";
            this.comboBoxRevisionState.Size = new System.Drawing.Size(407, 21);
            this.comboBoxRevisionState.TabIndex = 45;
            // 
            // buttonGoToDuplicateAccessionNumber
            // 
            this.buttonGoToDuplicateAccessionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGoToDuplicateAccessionNumber.Image = global::DiversityCollection.Resource.Search;
            this.buttonGoToDuplicateAccessionNumber.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonGoToDuplicateAccessionNumber.Location = new System.Drawing.Point(484, 0);
            this.buttonGoToDuplicateAccessionNumber.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGoToDuplicateAccessionNumber.Name = "buttonGoToDuplicateAccessionNumber";
            this.buttonGoToDuplicateAccessionNumber.Size = new System.Drawing.Size(59, 23);
            this.buttonGoToDuplicateAccessionNumber.TabIndex = 46;
            this.buttonGoToDuplicateAccessionNumber.Text = "Dupl.";
            this.buttonGoToDuplicateAccessionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonGoToDuplicateAccessionNumber.UseVisualStyleBackColor = true;
            this.buttonGoToDuplicateAccessionNumber.Visible = false;
            this.buttonGoToDuplicateAccessionNumber.Click += new System.EventHandler(this.buttonGoToDuplicateAccessionNumber_Click);
            // 
            // labelSpecimenReferenceDetails
            // 
            this.labelSpecimenReferenceDetails.AccessibleName = "CollectionSpecimen.ReferenceDetails";
            this.labelSpecimenReferenceDetails.AutoSize = true;
            this.labelSpecimenReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpecimenReferenceDetails.Location = new System.Drawing.Point(543, 122);
            this.labelSpecimenReferenceDetails.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.labelSpecimenReferenceDetails.Name = "labelSpecimenReferenceDetails";
            this.labelSpecimenReferenceDetails.Size = new System.Drawing.Size(36, 22);
            this.labelSpecimenReferenceDetails.TabIndex = 47;
            this.labelSpecimenReferenceDetails.Text = "Pag.:";
            this.labelSpecimenReferenceDetails.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelSpecimenReferenceDetails.Visible = false;
            // 
            // textBoxSpecimenReferenceDetails
            // 
            this.tableLayoutPanelAccession.SetColumnSpan(this.textBoxSpecimenReferenceDetails, 2);
            this.textBoxSpecimenReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSpecimenReferenceDetails.Location = new System.Drawing.Point(579, 119);
            this.textBoxSpecimenReferenceDetails.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxSpecimenReferenceDetails.Name = "textBoxSpecimenReferenceDetails";
            this.textBoxSpecimenReferenceDetails.Size = new System.Drawing.Size(83, 20);
            this.textBoxSpecimenReferenceDetails.TabIndex = 48;
            this.textBoxSpecimenReferenceDetails.Visible = false;
            // 
            // buttonTemplateSpecimenSet
            // 
            this.buttonTemplateSpecimenSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplateSpecimenSet.FlatAppearance.BorderSize = 0;
            this.buttonTemplateSpecimenSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTemplateSpecimenSet.Image = global::DiversityCollection.Resource.Template;
            this.buttonTemplateSpecimenSet.Location = new System.Drawing.Point(0, 0);
            this.buttonTemplateSpecimenSet.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTemplateSpecimenSet.Name = "buttonTemplateSpecimenSet";
            this.buttonTemplateSpecimenSet.Size = new System.Drawing.Size(15, 23);
            this.buttonTemplateSpecimenSet.TabIndex = 49;
            this.buttonTemplateSpecimenSet.UseVisualStyleBackColor = true;
            this.buttonTemplateSpecimenSet.Click += new System.EventHandler(this.buttonTemplateSpecimenSet_Click);
            // 
            // buttonTemplateSpecimenEdit
            // 
            this.buttonTemplateSpecimenEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTemplateSpecimenEdit.FlatAppearance.BorderSize = 0;
            this.buttonTemplateSpecimenEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTemplateSpecimenEdit.Image = global::DiversityCollection.Resource.TemplateEditor;
            this.buttonTemplateSpecimenEdit.Location = new System.Drawing.Point(0, 23);
            this.buttonTemplateSpecimenEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTemplateSpecimenEdit.Name = "buttonTemplateSpecimenEdit";
            this.buttonTemplateSpecimenEdit.Size = new System.Drawing.Size(15, 24);
            this.buttonTemplateSpecimenEdit.TabIndex = 50;
            this.buttonTemplateSpecimenEdit.UseVisualStyleBackColor = true;
            this.buttonTemplateSpecimenEdit.Click += new System.EventHandler(this.buttonTemplateSpecimenEdit_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxAccession);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelExsiccataSeries);
            this.splitContainerMain.Size = new System.Drawing.Size(671, 165);
            this.splitContainerMain.SplitterDistance = 134;
            this.splitContainerMain.TabIndex = 30;
            // 
            // tableLayoutPanelExsiccataSeries
            // 
            this.tableLayoutPanelExsiccataSeries.ColumnCount = 3;
            this.tableLayoutPanelExsiccataSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelExsiccataSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExsiccataSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExsiccataSeries.Controls.Add(this.pictureBoxExsiccata, 0, 0);
            this.tableLayoutPanelExsiccataSeries.Controls.Add(this.userControlModuleRelatedEntryExsiccate, 2, 0);
            this.tableLayoutPanelExsiccataSeries.Controls.Add(this.labelExsiccataAbbreviation, 1, 0);
            this.tableLayoutPanelExsiccataSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelExsiccataSeries.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelExsiccataSeries.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelExsiccataSeries.Name = "tableLayoutPanelExsiccataSeries";
            this.tableLayoutPanelExsiccataSeries.RowCount = 1;
            this.tableLayoutPanelExsiccataSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExsiccataSeries.Size = new System.Drawing.Size(671, 27);
            this.tableLayoutPanelExsiccataSeries.TabIndex = 2;
            // 
            // pictureBoxExsiccata
            // 
            this.pictureBoxExsiccata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxExsiccata.Image = global::DiversityCollection.Resource.IndExs;
            this.pictureBoxExsiccata.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxExsiccata.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.pictureBoxExsiccata.Name = "pictureBoxExsiccata";
            this.pictureBoxExsiccata.Size = new System.Drawing.Size(17, 24);
            this.pictureBoxExsiccata.TabIndex = 7;
            this.pictureBoxExsiccata.TabStop = false;
            // 
            // labelExsiccataAbbreviation
            // 
            this.labelExsiccataAbbreviation.AccessibleName = "CollectionSpecimen.ExsiccataAbbreviation";
            this.labelExsiccataAbbreviation.AutoSize = true;
            this.labelExsiccataAbbreviation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExsiccataAbbreviation.Location = new System.Drawing.Point(23, 5);
            this.labelExsiccataAbbreviation.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.labelExsiccataAbbreviation.Name = "labelExsiccataAbbreviation";
            this.labelExsiccataAbbreviation.Size = new System.Drawing.Size(86, 22);
            this.labelExsiccataAbbreviation.TabIndex = 1;
            this.labelExsiccataAbbreviation.Text = "Exsiccata series:";
            this.labelExsiccataAbbreviation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonAccessionNumberEdit
            // 
            this.buttonAccessionNumberEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAccessionNumberEdit.Enabled = false;
            this.buttonAccessionNumberEdit.FlatAppearance.BorderSize = 0;
            this.buttonAccessionNumberEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAccessionNumberEdit.Image = global::DiversityCollection.Resource.Edit1;
            this.buttonAccessionNumberEdit.Location = new System.Drawing.Point(466, 0);
            this.buttonAccessionNumberEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAccessionNumberEdit.Name = "buttonAccessionNumberEdit";
            this.buttonAccessionNumberEdit.Size = new System.Drawing.Size(18, 23);
            this.buttonAccessionNumberEdit.TabIndex = 51;
            this.buttonAccessionNumberEdit.UseVisualStyleBackColor = true;
            this.buttonAccessionNumberEdit.Click += new System.EventHandler(this.buttonAccessionNumberEdit_Click);
            // 
            // userControlModuleRelatedEntrySpecimenReference
            // 
            this.userControlModuleRelatedEntrySpecimenReference.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelAccession.SetColumnSpan(this.userControlModuleRelatedEntrySpecimenReference, 3);
            this.userControlModuleRelatedEntrySpecimenReference.DependsOnUri = "";
            this.userControlModuleRelatedEntrySpecimenReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntrySpecimenReference.Domain = "";
            this.userControlModuleRelatedEntrySpecimenReference.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntrySpecimenReference.Location = new System.Drawing.Point(74, 119);
            this.userControlModuleRelatedEntrySpecimenReference.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.userControlModuleRelatedEntrySpecimenReference.Module = null;
            this.userControlModuleRelatedEntrySpecimenReference.Name = "userControlModuleRelatedEntrySpecimenReference";
            this.userControlModuleRelatedEntrySpecimenReference.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntrySpecimenReference.ShowInfo = false;
            this.userControlModuleRelatedEntrySpecimenReference.Size = new System.Drawing.Size(466, 22);
            this.userControlModuleRelatedEntrySpecimenReference.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntrySpecimenReference.TabIndex = 0;
            this.userControlModuleRelatedEntrySpecimenReference.Visible = false;
            // 
            // userControlDatePanelAccessionDate
            // 
            this.userControlDatePanelAccessionDate.AccessibleName = "CollectionSpecimen.AccessionDate";
            this.tableLayoutPanelAccession.SetColumnSpan(this.userControlDatePanelAccessionDate, 3);
            this.userControlDatePanelAccessionDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatePanelAccessionDate.Location = new System.Drawing.Point(74, 71);
            this.userControlDatePanelAccessionDate.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlDatePanelAccessionDate.Name = "userControlDatePanelAccessionDate";
            this.userControlDatePanelAccessionDate.Size = new System.Drawing.Size(469, 22);
            this.userControlDatePanelAccessionDate.TabIndex = 14;
            // 
            // userControlModuleRelatedEntryDepositor
            // 
            this.userControlModuleRelatedEntryDepositor.AccessibleName = "CollectionSpecimen.DepositorsName";
            this.userControlModuleRelatedEntryDepositor.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelAccession.SetColumnSpan(this.userControlModuleRelatedEntryDepositor, 2);
            this.userControlModuleRelatedEntryDepositor.DependsOnUri = "";
            this.userControlModuleRelatedEntryDepositor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryDepositor.Domain = "";
            this.userControlModuleRelatedEntryDepositor.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryDepositor.Location = new System.Drawing.Point(74, 23);
            this.userControlModuleRelatedEntryDepositor.Margin = new System.Windows.Forms.Padding(0, 0, 3, 1);
            this.userControlModuleRelatedEntryDepositor.Module = null;
            this.userControlModuleRelatedEntryDepositor.Name = "userControlModuleRelatedEntryDepositor";
            this.userControlModuleRelatedEntryDepositor.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryDepositor.ShowInfo = false;
            this.userControlModuleRelatedEntryDepositor.Size = new System.Drawing.Size(407, 23);
            this.userControlModuleRelatedEntryDepositor.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryDepositor.TabIndex = 16;
            // 
            // userControlHierarchySelectorAccessionDateCategory
            // 
            this.userControlHierarchySelectorAccessionDateCategory.AccessibleName = "CollectionSpecimen.AccessionDateCategory";
            this.userControlHierarchySelectorAccessionDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlHierarchySelectorAccessionDateCategory.Location = new System.Drawing.Point(647, 71);
            this.userControlHierarchySelectorAccessionDateCategory.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.userControlHierarchySelectorAccessionDateCategory.Name = "userControlHierarchySelectorAccessionDateCategory";
            this.userControlHierarchySelectorAccessionDateCategory.Size = new System.Drawing.Size(15, 20);
            this.userControlHierarchySelectorAccessionDateCategory.TabIndex = 42;
            // 
            // userControlModuleRelatedEntryExsiccate
            // 
            this.userControlModuleRelatedEntryExsiccate.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryExsiccate.DependsOnUri = "";
            this.userControlModuleRelatedEntryExsiccate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryExsiccate.Domain = "";
            this.userControlModuleRelatedEntryExsiccate.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryExsiccate.Location = new System.Drawing.Point(109, 0);
            this.userControlModuleRelatedEntryExsiccate.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.userControlModuleRelatedEntryExsiccate.Module = null;
            this.userControlModuleRelatedEntryExsiccate.Name = "userControlModuleRelatedEntryExsiccate";
            this.userControlModuleRelatedEntryExsiccate.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryExsiccate.ShowInfo = false;
            this.userControlModuleRelatedEntryExsiccate.Size = new System.Drawing.Size(559, 27);
            this.userControlModuleRelatedEntryExsiccate.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryExsiccate.TabIndex = 0;
            // 
            // UserControl_Specimen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.MinimumSize = new System.Drawing.Size(0, 135);
            this.Name = "UserControl_Specimen";
            this.Size = new System.Drawing.Size(671, 165);
            this.groupBoxAccession.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimen)).EndInit();
            this.tableLayoutPanelAccession.ResumeLayout(false);
            this.tableLayoutPanelAccession.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelExsiccataSeries.ResumeLayout(false);
            this.tableLayoutPanelExsiccataSeries.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExsiccata)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxAccession;
        private System.Windows.Forms.PictureBox pictureBoxSpecimen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAccession;
        private System.Windows.Forms.ComboBox comboBoxExternalSource;
        private System.Windows.Forms.ComboBox comboBoxCollectionSpecimenDataWithholdingReason;
        private System.Windows.Forms.TextBox textBoxExternalSourceID;
        private System.Windows.Forms.Label labelExternalSourceID;
        private System.Windows.Forms.Label labelCollectionSpecimenDataWithholdingReason;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntrySpecimenReference;
        private System.Windows.Forms.ComboBox comboBoxAccessionDateCategory;
        private System.Windows.Forms.Label labelAccessionDateCategory;
        private System.Windows.Forms.Label labelAccessionDate;
        private System.Windows.Forms.Label labelSpecimenReference;
        private System.Windows.Forms.TextBox textBoxDepositorsNr;
        private System.Windows.Forms.Label labelDepositorsNr;
        private System.Windows.Forms.Label labelDepositor;
        private System.Windows.Forms.Button buttonFindNextAccessionNumber;
        private System.Windows.Forms.TextBox textBoxAccessionNumber;
        private System.Windows.Forms.Label labelAccessionNumber;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelAccessionDate;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryDepositor;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorAccessionDateCategory;
        private System.Windows.Forms.Label labelExternalSource;
        private System.Windows.Forms.Label labelRevisionState;
        private System.Windows.Forms.ComboBox comboBoxRevisionState;
        private System.Windows.Forms.Button buttonGoToDuplicateAccessionNumber;
        private System.Windows.Forms.Label labelSpecimenReferenceDetails;
        private System.Windows.Forms.TextBox textBoxSpecimenReferenceDetails;
        private System.Windows.Forms.Button buttonTemplateSpecimenSet;
        private System.Windows.Forms.Button buttonTemplateSpecimenEdit;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelExsiccataSeries;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryExsiccate;
        private System.Windows.Forms.Label labelExsiccataAbbreviation;
        private System.Windows.Forms.PictureBox pictureBoxExsiccata;
        private System.Windows.Forms.Button buttonAccessionNumberEdit;
    }
}
