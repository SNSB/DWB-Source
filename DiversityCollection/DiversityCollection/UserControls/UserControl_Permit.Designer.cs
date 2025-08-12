namespace DiversityCollection.UserControls
{
    partial class UserControl_Permit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Permit));
            this.tabControlTransaction = new System.Windows.Forms.TabControl();
            this.tabPageDataEntry = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelCollection = new System.Windows.Forms.TableLayoutPanel();
            this.labelTransactionTitle = new System.Windows.Forms.Label();
            this.textBoxTransactionTitle = new System.Windows.Forms.TextBox();
            this.transactionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetTransaction = new DiversityCollection.Datasets.DataSetTransaction();
            this.labelBeginDate = new System.Windows.Forms.Label();
            this.labelInternalNotes = new System.Windows.Forms.Label();
            this.textBoxInternalNotes = new System.Windows.Forms.TextBox();
            this.labelResponsible = new System.Windows.Forms.Label();
            this.textBoxInvestigator = new System.Windows.Forms.TextBox();
            this.labelInvestigator = new System.Windows.Forms.Label();
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelFrom = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxFromTransactionNumber = new System.Windows.Forms.TextBox();
            this.labelFromTransactionNumber = new System.Windows.Forms.Label();
            this.textBoxFromTransactionPartnerName = new System.Windows.Forms.TextBox();
            this.groupBoxTo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelTo = new System.Windows.Forms.TableLayoutPanel();
            this.labelToRecipient = new System.Windows.Forms.Label();
            this.comboBoxToRecipient = new System.Windows.Forms.ComboBox();
            this.textBoxToTransactionPartnerName = new System.Windows.Forms.TextBox();
            this.labelDateSupplement = new System.Windows.Forms.Label();
            this.textBoxDateSupplement = new System.Windows.Forms.TextBox();
            this.textBoxBeginDate = new System.Windows.Forms.TextBox();
            this.textBoxActualEndDate = new System.Windows.Forms.TextBox();
            this.labelActualEndDate = new System.Windows.Forms.Label();
            this.textBoxAgreedEndDate = new System.Windows.Forms.TextBox();
            this.labelAgreedEndDate = new System.Windows.Forms.Label();
            this.textBoxResponsible = new System.Windows.Forms.TextBox();
            this.groupBoxMaterial = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelMaterial = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxMaterial = new System.Windows.Forms.TextBox();
            this.labelMaterialCategory = new System.Windows.Forms.Label();
            this.labelMaterialCollectors = new System.Windows.Forms.Label();
            this.textBoxMaterialCollectors = new System.Windows.Forms.TextBox();
            this.labelMaterialSource = new System.Windows.Forms.Label();
            this.comboBoxMaterialSource = new System.Windows.Forms.ComboBox();
            this.labelMaterialDescription = new System.Windows.Forms.Label();
            this.textBoxMaterialDescription = new System.Windows.Forms.TextBox();
            this.labelNumberOfUnits = new System.Windows.Forms.Label();
            this.textBoxNumberOfUnits = new System.Windows.Forms.TextBox();
            this.labelTransactionID = new System.Windows.Forms.Label();
            this.textBoxTransactionID = new System.Windows.Forms.TextBox();
            this.tabPageDocuments = new System.Windows.Forms.TabPage();
            this.splitContainerDocuments = new System.Windows.Forms.SplitContainer();
            this.listBoxTransactionDocuments = new System.Windows.Forms.ListBox();
            this.transactionDocumentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainerTransactionDocumentData = new System.Windows.Forms.SplitContainer();
            this.splitContainerTransactionDocuments = new System.Windows.Forms.SplitContainer();
            this.panelHistoryWebbrowser = new System.Windows.Forms.Panel();
            this.webBrowserTransactionDocuments = new System.Windows.Forms.WebBrowser();
            this.panelHistoryImage = new System.Windows.Forms.Panel();
            this.pictureBoxTransactionDocuments = new System.Windows.Forms.PictureBox();
            this.toolStripDocumentImage = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDocumentZoomAdapt = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDocumentZoom100Percent = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelTransactionDocument = new System.Windows.Forms.TableLayoutPanel();
            this.labelTransactionDocumentDisplayText = new System.Windows.Forms.Label();
            this.textBoxTransactionDocumentInternalNotes = new System.Windows.Forms.TextBox();
            this.textBoxTransactionDocumentDisplayText = new System.Windows.Forms.TextBox();
            this.labelDocumentNotes = new System.Windows.Forms.Label();
            this.labelDocumentType = new System.Windows.Forms.Label();
            this.comboBoxDocumentType = new System.Windows.Forms.ComboBox();
            this.tabPagePayment = new System.Windows.Forms.TabPage();
            this.splitContainerPayment = new System.Windows.Forms.SplitContainer();
            this.listBoxPayment = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelPayment = new System.Windows.Forms.TableLayoutPanel();
            this.labelPaymentIdentifier = new System.Windows.Forms.Label();
            this.textBoxPaymentIdentifier = new System.Windows.Forms.TextBox();
            this.labelPaymentAmount = new System.Windows.Forms.Label();
            this.textBoxPaymentAmount = new System.Windows.Forms.TextBox();
            this.labelPaymentCurrency = new System.Windows.Forms.Label();
            this.labelPaymentForeignAmount = new System.Windows.Forms.Label();
            this.textBoxPaymentForeignAmount = new System.Windows.Forms.TextBox();
            this.textBoxPaymentForeignAmountCurrency = new System.Windows.Forms.TextBox();
            this.labelPaymentPayer = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryPaymentPayer = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryPaymentRecipient = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelPaymentRecipient = new System.Windows.Forms.Label();
            this.labelPaymentDate = new System.Windows.Forms.Label();
            this.labelPaymentDateSupplement = new System.Windows.Forms.Label();
            this.textBoxPaymentDateSupplement = new System.Windows.Forms.TextBox();
            this.labelPaymentNotes = new System.Windows.Forms.Label();
            this.textBoxPaymentNotes = new System.Windows.Forms.TextBox();
            this.labelPaymentURI = new System.Windows.Forms.Label();
            this.textBoxPaymentURI = new System.Windows.Forms.TextBox();
            this.textBoxPaymentDate = new System.Windows.Forms.TextBox();
            this.tabPageAgents = new System.Windows.Forms.TabPage();
            this.splitContainerAgents = new System.Windows.Forms.SplitContainer();
            this.listBoxAgents = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelAgents = new System.Windows.Forms.TableLayoutPanel();
            this.userControlModuleRelatedEntryAgent = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelAgentRole = new System.Windows.Forms.Label();
            this.labelAgentNotes = new System.Windows.Forms.Label();
            this.textBoxAgentNotes = new System.Windows.Forms.TextBox();
            this.comboBoxAgentRole = new System.Windows.Forms.ComboBox();
            this.tabPageIdentifier = new System.Windows.Forms.TabPage();
            this.splitContainerIdentifier = new System.Windows.Forms.SplitContainer();
            this.listBoxIdentifier = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelIdentifier = new System.Windows.Forms.TableLayoutPanel();
            this.labelIdentifierType = new System.Windows.Forms.Label();
            this.labelIdentifier = new System.Windows.Forms.Label();
            this.textBoxIdentifierType = new System.Windows.Forms.TextBox();
            this.textBoxIdentifier = new System.Windows.Forms.TextBox();
            this.buttonIdenitifierURL = new System.Windows.Forms.Button();
            this.labelIdenitifierURL = new System.Windows.Forms.Label();
            this.textBoxIdenitifierURL = new System.Windows.Forms.TextBox();
            this.labelIdenitifierNotes = new System.Windows.Forms.Label();
            this.textBoxIdenitifierNotes = new System.Windows.Forms.TextBox();
            this.transactionDocumentTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.TransactionDocumentTableAdapter();
            this.transactionTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.TransactionTableAdapter();
            this.tabControlTransaction.SuspendLayout();
            this.tabPageDataEntry.SuspendLayout();
            this.tableLayoutPanelCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transactionBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).BeginInit();
            this.groupBoxFrom.SuspendLayout();
            this.tableLayoutPanelFrom.SuspendLayout();
            this.groupBoxTo.SuspendLayout();
            this.tableLayoutPanelTo.SuspendLayout();
            this.groupBoxMaterial.SuspendLayout();
            this.tableLayoutPanelMaterial.SuspendLayout();
            this.tabPageDocuments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDocuments)).BeginInit();
            this.splitContainerDocuments.Panel1.SuspendLayout();
            this.splitContainerDocuments.Panel2.SuspendLayout();
            this.splitContainerDocuments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transactionDocumentBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTransactionDocumentData)).BeginInit();
            this.splitContainerTransactionDocumentData.Panel1.SuspendLayout();
            this.splitContainerTransactionDocumentData.Panel2.SuspendLayout();
            this.splitContainerTransactionDocumentData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTransactionDocuments)).BeginInit();
            this.splitContainerTransactionDocuments.Panel1.SuspendLayout();
            this.splitContainerTransactionDocuments.Panel2.SuspendLayout();
            this.splitContainerTransactionDocuments.SuspendLayout();
            this.panelHistoryWebbrowser.SuspendLayout();
            this.panelHistoryImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTransactionDocuments)).BeginInit();
            this.toolStripDocumentImage.SuspendLayout();
            this.tableLayoutPanelTransactionDocument.SuspendLayout();
            this.tabPagePayment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPayment)).BeginInit();
            this.splitContainerPayment.Panel1.SuspendLayout();
            this.splitContainerPayment.Panel2.SuspendLayout();
            this.splitContainerPayment.SuspendLayout();
            this.tableLayoutPanelPayment.SuspendLayout();
            this.tabPageAgents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAgents)).BeginInit();
            this.splitContainerAgents.Panel1.SuspendLayout();
            this.splitContainerAgents.Panel2.SuspendLayout();
            this.splitContainerAgents.SuspendLayout();
            this.tableLayoutPanelAgents.SuspendLayout();
            this.tabPageIdentifier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerIdentifier)).BeginInit();
            this.splitContainerIdentifier.Panel1.SuspendLayout();
            this.splitContainerIdentifier.Panel2.SuspendLayout();
            this.splitContainerIdentifier.SuspendLayout();
            this.tableLayoutPanelIdentifier.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // tabControlTransaction
            // 
            this.tabControlTransaction.Controls.Add(this.tabPageDataEntry);
            this.tabControlTransaction.Controls.Add(this.tabPageDocuments);
            this.tabControlTransaction.Controls.Add(this.tabPagePayment);
            this.tabControlTransaction.Controls.Add(this.tabPageAgents);
            this.tabControlTransaction.Controls.Add(this.tabPageIdentifier);
            this.tabControlTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlTransaction.Location = new System.Drawing.Point(0, 0);
            this.tabControlTransaction.Name = "tabControlTransaction";
            this.tabControlTransaction.SelectedIndex = 0;
            this.tabControlTransaction.Size = new System.Drawing.Size(610, 353);
            this.tabControlTransaction.TabIndex = 3;
            // 
            // tabPageDataEntry
            // 
            this.tabPageDataEntry.Controls.Add(this.tableLayoutPanelCollection);
            this.tabPageDataEntry.ImageIndex = 0;
            this.tabPageDataEntry.Location = new System.Drawing.Point(4, 22);
            this.tabPageDataEntry.Name = "tabPageDataEntry";
            this.tabPageDataEntry.Size = new System.Drawing.Size(602, 327);
            this.tabPageDataEntry.TabIndex = 0;
            this.tabPageDataEntry.Text = "Details";
            this.tabPageDataEntry.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelCollection
            // 
            this.tableLayoutPanelCollection.ColumnCount = 8;
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCollection.Controls.Add(this.labelTransactionTitle, 0, 0);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxTransactionTitle, 1, 0);
            this.tableLayoutPanelCollection.Controls.Add(this.labelBeginDate, 0, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.labelInternalNotes, 0, 10);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxInternalNotes, 1, 10);
            this.tableLayoutPanelCollection.Controls.Add(this.labelResponsible, 0, 9);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxInvestigator, 5, 9);
            this.tableLayoutPanelCollection.Controls.Add(this.labelInvestigator, 4, 9);
            this.tableLayoutPanelCollection.Controls.Add(this.groupBoxFrom, 0, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.groupBoxTo, 4, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.labelDateSupplement, 2, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxDateSupplement, 3, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxBeginDate, 1, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxActualEndDate, 7, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.labelActualEndDate, 6, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxAgreedEndDate, 5, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.labelAgreedEndDate, 4, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxResponsible, 1, 9);
            this.tableLayoutPanelCollection.Controls.Add(this.groupBoxMaterial, 0, 1);
            this.tableLayoutPanelCollection.Controls.Add(this.labelTransactionID, 6, 0);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxTransactionID, 7, 0);
            this.tableLayoutPanelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCollection.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelCollection.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanelCollection.Name = "tableLayoutPanelCollection";
            this.tableLayoutPanelCollection.RowCount = 11;
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCollection.Size = new System.Drawing.Size(602, 327);
            this.tableLayoutPanelCollection.TabIndex = 0;
            // 
            // labelTransactionTitle
            // 
            this.labelTransactionTitle.AccessibleName = "Transaction.TransactionTitle";
            this.labelTransactionTitle.AutoSize = true;
            this.labelTransactionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransactionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTransactionTitle.Location = new System.Drawing.Point(3, 3);
            this.labelTransactionTitle.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelTransactionTitle.Name = "labelTransactionTitle";
            this.labelTransactionTitle.Size = new System.Drawing.Size(55, 23);
            this.labelTransactionTitle.TabIndex = 2;
            this.labelTransactionTitle.Text = "Title:";
            this.labelTransactionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTransactionTitle
            // 
            this.textBoxTransactionTitle.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxTransactionTitle, 5);
            this.textBoxTransactionTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "TransactionTitle", true));
            this.textBoxTransactionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTransactionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTransactionTitle.Location = new System.Drawing.Point(58, 3);
            this.textBoxTransactionTitle.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.textBoxTransactionTitle.MaxLength = 200;
            this.textBoxTransactionTitle.Name = "textBoxTransactionTitle";
            this.textBoxTransactionTitle.ReadOnly = true;
            this.textBoxTransactionTitle.Size = new System.Drawing.Size(406, 20);
            this.textBoxTransactionTitle.TabIndex = 3;
            // 
            // transactionBindingSource
            // 
            this.transactionBindingSource.DataMember = "Transaction";
            this.transactionBindingSource.DataSource = this.dataSetTransaction;
            // 
            // dataSetTransaction
            // 
            this.dataSetTransaction.DataSetName = "DataSetTransaction";
            this.dataSetTransaction.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelBeginDate
            // 
            this.labelBeginDate.AccessibleName = "Transaction.BeginDate";
            this.labelBeginDate.AutoSize = true;
            this.labelBeginDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBeginDate.Location = new System.Drawing.Point(3, 153);
            this.labelBeginDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelBeginDate.Name = "labelBeginDate";
            this.labelBeginDate.Size = new System.Drawing.Size(55, 23);
            this.labelBeginDate.TabIndex = 4;
            this.labelBeginDate.Text = "Begin:";
            this.labelBeginDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelInternalNotes
            // 
            this.labelInternalNotes.AccessibleName = "Transaction.InternalNotes";
            this.labelInternalNotes.AutoSize = true;
            this.labelInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInternalNotes.Location = new System.Drawing.Point(3, 208);
            this.labelInternalNotes.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelInternalNotes.Name = "labelInternalNotes";
            this.labelInternalNotes.Size = new System.Drawing.Size(55, 119);
            this.labelInternalNotes.TabIndex = 19;
            this.labelInternalNotes.Text = "Int. notes:";
            this.labelInternalNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxInternalNotes
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxInternalNotes, 7);
            this.textBoxInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "InternalNotes", true));
            this.textBoxInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInternalNotes.Location = new System.Drawing.Point(58, 202);
            this.textBoxInternalNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxInternalNotes.Multiline = true;
            this.textBoxInternalNotes.Name = "textBoxInternalNotes";
            this.textBoxInternalNotes.ReadOnly = true;
            this.textBoxInternalNotes.Size = new System.Drawing.Size(541, 122);
            this.textBoxInternalNotes.TabIndex = 20;
            // 
            // labelResponsible
            // 
            this.labelResponsible.AccessibleName = "Transaction.ResponsibleName";
            this.labelResponsible.AutoSize = true;
            this.labelResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResponsible.Location = new System.Drawing.Point(3, 182);
            this.labelResponsible.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelResponsible.Name = "labelResponsible";
            this.labelResponsible.Size = new System.Drawing.Size(55, 20);
            this.labelResponsible.TabIndex = 21;
            this.labelResponsible.Text = "Respons.:";
            this.labelResponsible.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxInvestigator
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxInvestigator, 3);
            this.textBoxInvestigator.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "Investigator", true));
            this.textBoxInvestigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInvestigator.Location = new System.Drawing.Point(358, 179);
            this.textBoxInvestigator.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.textBoxInvestigator.MaxLength = 200;
            this.textBoxInvestigator.Name = "textBoxInvestigator";
            this.textBoxInvestigator.ReadOnly = true;
            this.textBoxInvestigator.Size = new System.Drawing.Size(241, 20);
            this.textBoxInvestigator.TabIndex = 25;
            // 
            // labelInvestigator
            // 
            this.labelInvestigator.AccessibleName = "Transaction.Investigator";
            this.labelInvestigator.AutoSize = true;
            this.labelInvestigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInvestigator.Location = new System.Drawing.Point(316, 176);
            this.labelInvestigator.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelInvestigator.Name = "labelInvestigator";
            this.labelInvestigator.Size = new System.Drawing.Size(42, 26);
            this.labelInvestigator.TabIndex = 24;
            this.labelInvestigator.Text = "Invest.:";
            this.labelInvestigator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBoxFrom
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.groupBoxFrom, 4);
            this.groupBoxFrom.Controls.Add(this.tableLayoutPanelFrom);
            this.groupBoxFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFrom.Location = new System.Drawing.Point(3, 89);
            this.groupBoxFrom.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBoxFrom.Name = "groupBoxFrom";
            this.groupBoxFrom.Size = new System.Drawing.Size(307, 64);
            this.groupBoxFrom.TabIndex = 64;
            this.groupBoxFrom.TabStop = false;
            this.groupBoxFrom.Text = "From";
            // 
            // tableLayoutPanelFrom
            // 
            this.tableLayoutPanelFrom.ColumnCount = 2;
            this.tableLayoutPanelFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelFrom.Controls.Add(this.textBoxFromTransactionNumber, 1, 1);
            this.tableLayoutPanelFrom.Controls.Add(this.labelFromTransactionNumber, 0, 1);
            this.tableLayoutPanelFrom.Controls.Add(this.textBoxFromTransactionPartnerName, 0, 0);
            this.tableLayoutPanelFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelFrom.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelFrom.Name = "tableLayoutPanelFrom";
            this.tableLayoutPanelFrom.RowCount = 2;
            this.tableLayoutPanelFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelFrom.Size = new System.Drawing.Size(301, 45);
            this.tableLayoutPanelFrom.TabIndex = 0;
            // 
            // textBoxFromTransactionNumber
            // 
            this.textBoxFromTransactionNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "FromTransactionNumber", true));
            this.textBoxFromTransactionNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFromTransactionNumber.Location = new System.Drawing.Point(53, 23);
            this.textBoxFromTransactionNumber.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxFromTransactionNumber.MaxLength = 50;
            this.textBoxFromTransactionNumber.Name = "textBoxFromTransactionNumber";
            this.textBoxFromTransactionNumber.ReadOnly = true;
            this.textBoxFromTransactionNumber.Size = new System.Drawing.Size(245, 20);
            this.textBoxFromTransactionNumber.TabIndex = 45;
            // 
            // labelFromTransactionNumber
            // 
            this.labelFromTransactionNumber.AccessibleName = "Transaction.FromTransactionNumber";
            this.labelFromTransactionNumber.AutoSize = true;
            this.labelFromTransactionNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFromTransactionNumber.Location = new System.Drawing.Point(3, 26);
            this.labelFromTransactionNumber.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelFromTransactionNumber.Name = "labelFromTransactionNumber";
            this.labelFromTransactionNumber.Size = new System.Drawing.Size(47, 13);
            this.labelFromTransactionNumber.TabIndex = 56;
            this.labelFromTransactionNumber.Text = "Number:";
            this.labelFromTransactionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxFromTransactionPartnerName
            // 
            this.tableLayoutPanelFrom.SetColumnSpan(this.textBoxFromTransactionPartnerName, 2);
            this.textBoxFromTransactionPartnerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "FromTransactionPartnerName", true));
            this.textBoxFromTransactionPartnerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFromTransactionPartnerName.Location = new System.Drawing.Point(3, 0);
            this.textBoxFromTransactionPartnerName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxFromTransactionPartnerName.Name = "textBoxFromTransactionPartnerName";
            this.textBoxFromTransactionPartnerName.ReadOnly = true;
            this.textBoxFromTransactionPartnerName.Size = new System.Drawing.Size(295, 20);
            this.textBoxFromTransactionPartnerName.TabIndex = 57;
            // 
            // groupBoxTo
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.groupBoxTo, 4);
            this.groupBoxTo.Controls.Add(this.tableLayoutPanelTo);
            this.groupBoxTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTo.Location = new System.Drawing.Point(316, 89);
            this.groupBoxTo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBoxTo.Name = "groupBoxTo";
            this.groupBoxTo.Size = new System.Drawing.Size(283, 64);
            this.groupBoxTo.TabIndex = 65;
            this.groupBoxTo.TabStop = false;
            this.groupBoxTo.Text = "To";
            // 
            // tableLayoutPanelTo
            // 
            this.tableLayoutPanelTo.ColumnCount = 2;
            this.tableLayoutPanelTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTo.Controls.Add(this.labelToRecipient, 0, 1);
            this.tableLayoutPanelTo.Controls.Add(this.comboBoxToRecipient, 1, 1);
            this.tableLayoutPanelTo.Controls.Add(this.textBoxToTransactionPartnerName, 0, 0);
            this.tableLayoutPanelTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelTo.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelTo.Name = "tableLayoutPanelTo";
            this.tableLayoutPanelTo.RowCount = 2;
            this.tableLayoutPanelTo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTo.Size = new System.Drawing.Size(277, 45);
            this.tableLayoutPanelTo.TabIndex = 0;
            // 
            // labelToRecipient
            // 
            this.labelToRecipient.AccessibleName = "Transaction.ToRecipient";
            this.labelToRecipient.AutoSize = true;
            this.labelToRecipient.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelToRecipient.Location = new System.Drawing.Point(3, 26);
            this.labelToRecipient.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelToRecipient.Name = "labelToRecipient";
            this.labelToRecipient.Size = new System.Drawing.Size(55, 13);
            this.labelToRecipient.TabIndex = 58;
            this.labelToRecipient.Text = "Recipient:";
            // 
            // comboBoxToRecipient
            // 
            this.comboBoxToRecipient.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "ToRecipient", true));
            this.comboBoxToRecipient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxToRecipient.Enabled = false;
            this.comboBoxToRecipient.FormattingEnabled = true;
            this.comboBoxToRecipient.Location = new System.Drawing.Point(61, 23);
            this.comboBoxToRecipient.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.comboBoxToRecipient.Name = "comboBoxToRecipient";
            this.comboBoxToRecipient.Size = new System.Drawing.Size(213, 21);
            this.comboBoxToRecipient.TabIndex = 59;
            // 
            // textBoxToTransactionPartnerName
            // 
            this.tableLayoutPanelTo.SetColumnSpan(this.textBoxToTransactionPartnerName, 2);
            this.textBoxToTransactionPartnerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "ToTransactionPartnerName", true));
            this.textBoxToTransactionPartnerName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxToTransactionPartnerName.Location = new System.Drawing.Point(3, 0);
            this.textBoxToTransactionPartnerName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxToTransactionPartnerName.Name = "textBoxToTransactionPartnerName";
            this.textBoxToTransactionPartnerName.ReadOnly = true;
            this.textBoxToTransactionPartnerName.Size = new System.Drawing.Size(271, 20);
            this.textBoxToTransactionPartnerName.TabIndex = 60;
            // 
            // labelDateSupplement
            // 
            this.labelDateSupplement.AutoSize = true;
            this.labelDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDateSupplement.Location = new System.Drawing.Point(167, 153);
            this.labelDateSupplement.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDateSupplement.Name = "labelDateSupplement";
            this.labelDateSupplement.Size = new System.Drawing.Size(40, 23);
            this.labelDateSupplement.TabIndex = 68;
            this.labelDateSupplement.Text = "Suppl.:";
            this.labelDateSupplement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxDateSupplement
            // 
            this.textBoxDateSupplement.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "DateSupplement", true));
            this.textBoxDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDateSupplement.Location = new System.Drawing.Point(207, 156);
            this.textBoxDateSupplement.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.textBoxDateSupplement.Name = "textBoxDateSupplement";
            this.textBoxDateSupplement.ReadOnly = true;
            this.textBoxDateSupplement.Size = new System.Drawing.Size(103, 20);
            this.textBoxDateSupplement.TabIndex = 69;
            // 
            // textBoxBeginDate
            // 
            this.textBoxBeginDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "BeginDate", true));
            this.textBoxBeginDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxBeginDate.Location = new System.Drawing.Point(58, 156);
            this.textBoxBeginDate.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.textBoxBeginDate.Name = "textBoxBeginDate";
            this.textBoxBeginDate.ReadOnly = true;
            this.textBoxBeginDate.Size = new System.Drawing.Size(103, 20);
            this.textBoxBeginDate.TabIndex = 72;
            // 
            // textBoxActualEndDate
            // 
            this.textBoxActualEndDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "ActualEndDate", true));
            this.textBoxActualEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxActualEndDate.Location = new System.Drawing.Point(495, 156);
            this.textBoxActualEndDate.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.textBoxActualEndDate.Name = "textBoxActualEndDate";
            this.textBoxActualEndDate.ReadOnly = true;
            this.textBoxActualEndDate.Size = new System.Drawing.Size(104, 20);
            this.textBoxActualEndDate.TabIndex = 74;
            // 
            // labelActualEndDate
            // 
            this.labelActualEndDate.AccessibleName = "Transaction.ActualEndDate";
            this.labelActualEndDate.AutoSize = true;
            this.labelActualEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelActualEndDate.Location = new System.Drawing.Point(464, 153);
            this.labelActualEndDate.Margin = new System.Windows.Forms.Padding(0);
            this.labelActualEndDate.Name = "labelActualEndDate";
            this.labelActualEndDate.Size = new System.Drawing.Size(31, 23);
            this.labelActualEndDate.TabIndex = 29;
            this.labelActualEndDate.Text = "Prol.:";
            this.labelActualEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxAgreedEndDate
            // 
            this.textBoxAgreedEndDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "AgreedEndDate", true));
            this.textBoxAgreedEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAgreedEndDate.Location = new System.Drawing.Point(358, 156);
            this.textBoxAgreedEndDate.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.textBoxAgreedEndDate.Name = "textBoxAgreedEndDate";
            this.textBoxAgreedEndDate.ReadOnly = true;
            this.textBoxAgreedEndDate.Size = new System.Drawing.Size(103, 20);
            this.textBoxAgreedEndDate.TabIndex = 73;
            // 
            // labelAgreedEndDate
            // 
            this.labelAgreedEndDate.AccessibleName = "Transaction.AgreedEndDate";
            this.labelAgreedEndDate.AutoSize = true;
            this.labelAgreedEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAgreedEndDate.Location = new System.Drawing.Point(316, 153);
            this.labelAgreedEndDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAgreedEndDate.Name = "labelAgreedEndDate";
            this.labelAgreedEndDate.Size = new System.Drawing.Size(42, 23);
            this.labelAgreedEndDate.TabIndex = 13;
            this.labelAgreedEndDate.Text = "End:";
            this.labelAgreedEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxResponsible
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxResponsible, 3);
            this.textBoxResponsible.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "ResponsibleName", true));
            this.textBoxResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResponsible.Location = new System.Drawing.Point(58, 179);
            this.textBoxResponsible.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxResponsible.Name = "textBoxResponsible";
            this.textBoxResponsible.ReadOnly = true;
            this.textBoxResponsible.Size = new System.Drawing.Size(252, 20);
            this.textBoxResponsible.TabIndex = 75;
            // 
            // groupBoxMaterial
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.groupBoxMaterial, 8);
            this.groupBoxMaterial.Controls.Add(this.tableLayoutPanelMaterial);
            this.groupBoxMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMaterial.Location = new System.Drawing.Point(3, 29);
            this.groupBoxMaterial.Name = "groupBoxMaterial";
            this.groupBoxMaterial.Size = new System.Drawing.Size(596, 54);
            this.groupBoxMaterial.TabIndex = 76;
            this.groupBoxMaterial.TabStop = false;
            this.groupBoxMaterial.Text = "Material";
            // 
            // tableLayoutPanelMaterial
            // 
            this.tableLayoutPanelMaterial.ColumnCount = 5;
            this.tableLayoutPanelMaterial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelMaterial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMaterial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMaterial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelMaterial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelMaterial.Controls.Add(this.textBoxMaterial, 0, 1);
            this.tableLayoutPanelMaterial.Controls.Add(this.labelMaterialCategory, 0, 0);
            this.tableLayoutPanelMaterial.Controls.Add(this.labelMaterialCollectors, 1, 0);
            this.tableLayoutPanelMaterial.Controls.Add(this.textBoxMaterialCollectors, 1, 1);
            this.tableLayoutPanelMaterial.Controls.Add(this.labelMaterialSource, 2, 0);
            this.tableLayoutPanelMaterial.Controls.Add(this.comboBoxMaterialSource, 2, 1);
            this.tableLayoutPanelMaterial.Controls.Add(this.labelMaterialDescription, 3, 0);
            this.tableLayoutPanelMaterial.Controls.Add(this.textBoxMaterialDescription, 3, 1);
            this.tableLayoutPanelMaterial.Controls.Add(this.labelNumberOfUnits, 4, 0);
            this.tableLayoutPanelMaterial.Controls.Add(this.textBoxNumberOfUnits, 4, 1);
            this.tableLayoutPanelMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMaterial.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelMaterial.Name = "tableLayoutPanelMaterial";
            this.tableLayoutPanelMaterial.RowCount = 2;
            this.tableLayoutPanelMaterial.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMaterial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMaterial.Size = new System.Drawing.Size(590, 35);
            this.tableLayoutPanelMaterial.TabIndex = 0;
            // 
            // textBoxMaterial
            // 
            this.textBoxMaterial.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "MaterialCategory", true));
            this.textBoxMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMaterial.Location = new System.Drawing.Point(3, 13);
            this.textBoxMaterial.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxMaterial.Name = "textBoxMaterial";
            this.textBoxMaterial.ReadOnly = true;
            this.textBoxMaterial.Size = new System.Drawing.Size(171, 20);
            this.textBoxMaterial.TabIndex = 0;
            // 
            // labelMaterialCategory
            // 
            this.labelMaterialCategory.AccessibleName = "Transaction.MaterialCategory";
            this.labelMaterialCategory.AutoSize = true;
            this.labelMaterialCategory.Location = new System.Drawing.Point(3, 0);
            this.labelMaterialCategory.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelMaterialCategory.Name = "labelMaterialCategory";
            this.labelMaterialCategory.Size = new System.Drawing.Size(49, 13);
            this.labelMaterialCategory.TabIndex = 37;
            this.labelMaterialCategory.Text = "Category";
            this.labelMaterialCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMaterialCollectors
            // 
            this.labelMaterialCollectors.AccessibleName = "Transaction.MaterialCollectors";
            this.labelMaterialCollectors.AutoSize = true;
            this.labelMaterialCollectors.Location = new System.Drawing.Point(180, 0);
            this.labelMaterialCollectors.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelMaterialCollectors.Name = "labelMaterialCollectors";
            this.labelMaterialCollectors.Size = new System.Drawing.Size(53, 13);
            this.labelMaterialCollectors.TabIndex = 53;
            this.labelMaterialCollectors.Text = "Collectors";
            this.labelMaterialCollectors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMaterialCollectors
            // 
            this.textBoxMaterialCollectors.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "MaterialCollectors", true));
            this.textBoxMaterialCollectors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMaterialCollectors.Location = new System.Drawing.Point(177, 13);
            this.textBoxMaterialCollectors.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxMaterialCollectors.Name = "textBoxMaterialCollectors";
            this.textBoxMaterialCollectors.ReadOnly = true;
            this.textBoxMaterialCollectors.Size = new System.Drawing.Size(115, 20);
            this.textBoxMaterialCollectors.TabIndex = 54;
            // 
            // labelMaterialSource
            // 
            this.labelMaterialSource.AutoSize = true;
            this.labelMaterialSource.Location = new System.Drawing.Point(298, 0);
            this.labelMaterialSource.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelMaterialSource.Name = "labelMaterialSource";
            this.labelMaterialSource.Size = new System.Drawing.Size(41, 13);
            this.labelMaterialSource.TabIndex = 67;
            this.labelMaterialSource.Text = "Source";
            this.labelMaterialSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxMaterialSource
            // 
            this.comboBoxMaterialSource.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "MaterialSource", true));
            this.comboBoxMaterialSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxMaterialSource.Enabled = false;
            this.comboBoxMaterialSource.FormattingEnabled = true;
            this.comboBoxMaterialSource.Location = new System.Drawing.Point(295, 13);
            this.comboBoxMaterialSource.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.comboBoxMaterialSource.Name = "comboBoxMaterialSource";
            this.comboBoxMaterialSource.Size = new System.Drawing.Size(115, 21);
            this.comboBoxMaterialSource.TabIndex = 66;
            // 
            // labelMaterialDescription
            // 
            this.labelMaterialDescription.AccessibleName = "Transaction.MaterialDescription";
            this.labelMaterialDescription.AutoSize = true;
            this.labelMaterialDescription.Location = new System.Drawing.Point(413, 0);
            this.labelMaterialDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaterialDescription.Name = "labelMaterialDescription";
            this.labelMaterialDescription.Size = new System.Drawing.Size(60, 13);
            this.labelMaterialDescription.TabIndex = 35;
            this.labelMaterialDescription.Text = "Description";
            this.labelMaterialDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxMaterialDescription
            // 
            this.textBoxMaterialDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "MaterialDescription", true));
            this.textBoxMaterialDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMaterialDescription.Location = new System.Drawing.Point(413, 13);
            this.textBoxMaterialDescription.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxMaterialDescription.Name = "textBoxMaterialDescription";
            this.textBoxMaterialDescription.ReadOnly = true;
            this.textBoxMaterialDescription.Size = new System.Drawing.Size(115, 20);
            this.textBoxMaterialDescription.TabIndex = 36;
            // 
            // labelNumberOfUnits
            // 
            this.labelNumberOfUnits.AccessibleName = "Transaction.NumberOfUnits";
            this.labelNumberOfUnits.AutoSize = true;
            this.labelNumberOfUnits.Location = new System.Drawing.Point(534, 0);
            this.labelNumberOfUnits.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelNumberOfUnits.Name = "labelNumberOfUnits";
            this.labelNumberOfUnits.Size = new System.Drawing.Size(31, 13);
            this.labelNumberOfUnits.TabIndex = 6;
            this.labelNumberOfUnits.Text = "Units";
            this.labelNumberOfUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxNumberOfUnits
            // 
            this.textBoxNumberOfUnits.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "NumberOfUnits", true));
            this.textBoxNumberOfUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNumberOfUnits.Location = new System.Drawing.Point(531, 13);
            this.textBoxNumberOfUnits.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxNumberOfUnits.Name = "textBoxNumberOfUnits";
            this.textBoxNumberOfUnits.ReadOnly = true;
            this.textBoxNumberOfUnits.Size = new System.Drawing.Size(56, 20);
            this.textBoxNumberOfUnits.TabIndex = 7;
            // 
            // labelTransactionID
            // 
            this.labelTransactionID.AutoSize = true;
            this.labelTransactionID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransactionID.Location = new System.Drawing.Point(467, 0);
            this.labelTransactionID.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelTransactionID.Name = "labelTransactionID";
            this.labelTransactionID.Size = new System.Drawing.Size(28, 26);
            this.labelTransactionID.TabIndex = 77;
            this.labelTransactionID.Text = "ID:";
            this.labelTransactionID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTransactionID
            // 
            this.textBoxTransactionID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionBindingSource, "TransactionID", true));
            this.textBoxTransactionID.Location = new System.Drawing.Point(495, 3);
            this.textBoxTransactionID.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxTransactionID.Name = "textBoxTransactionID";
            this.textBoxTransactionID.ReadOnly = true;
            this.textBoxTransactionID.Size = new System.Drawing.Size(72, 20);
            this.textBoxTransactionID.TabIndex = 78;
            // 
            // tabPageDocuments
            // 
            this.tabPageDocuments.Controls.Add(this.splitContainerDocuments);
            this.tabPageDocuments.ImageIndex = 7;
            this.tabPageDocuments.Location = new System.Drawing.Point(4, 22);
            this.tabPageDocuments.Name = "tabPageDocuments";
            this.tabPageDocuments.Size = new System.Drawing.Size(602, 327);
            this.tabPageDocuments.TabIndex = 5;
            this.tabPageDocuments.Text = "Saved documents";
            this.tabPageDocuments.UseVisualStyleBackColor = true;
            // 
            // splitContainerDocuments
            // 
            this.splitContainerDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDocuments.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDocuments.Name = "splitContainerDocuments";
            // 
            // splitContainerDocuments.Panel1
            // 
            this.splitContainerDocuments.Panel1.Controls.Add(this.listBoxTransactionDocuments);
            // 
            // splitContainerDocuments.Panel2
            // 
            this.splitContainerDocuments.Panel2.Controls.Add(this.splitContainerTransactionDocumentData);
            this.splitContainerDocuments.Size = new System.Drawing.Size(602, 327);
            this.splitContainerDocuments.SplitterDistance = 97;
            this.splitContainerDocuments.TabIndex = 1;
            // 
            // listBoxTransactionDocuments
            // 
            this.listBoxTransactionDocuments.DataSource = this.transactionDocumentBindingSource;
            this.listBoxTransactionDocuments.DisplayMember = "DisplayText";
            this.listBoxTransactionDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTransactionDocuments.FormattingEnabled = true;
            this.listBoxTransactionDocuments.IntegralHeight = false;
            this.listBoxTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            this.listBoxTransactionDocuments.Name = "listBoxTransactionDocuments";
            this.listBoxTransactionDocuments.Size = new System.Drawing.Size(97, 327);
            this.listBoxTransactionDocuments.TabIndex = 0;
            this.listBoxTransactionDocuments.ValueMember = "Date";
            // 
            // transactionDocumentBindingSource
            // 
            this.transactionDocumentBindingSource.DataMember = "TransactionDocument";
            this.transactionDocumentBindingSource.DataSource = this.dataSetTransaction;
            // 
            // splitContainerTransactionDocumentData
            // 
            this.splitContainerTransactionDocumentData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTransactionDocumentData.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTransactionDocumentData.Name = "splitContainerTransactionDocumentData";
            this.splitContainerTransactionDocumentData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerTransactionDocumentData.Panel1
            // 
            this.splitContainerTransactionDocumentData.Panel1.Controls.Add(this.splitContainerTransactionDocuments);
            // 
            // splitContainerTransactionDocumentData.Panel2
            // 
            this.splitContainerTransactionDocumentData.Panel2.Controls.Add(this.tableLayoutPanelTransactionDocument);
            this.splitContainerTransactionDocumentData.Size = new System.Drawing.Size(501, 327);
            this.splitContainerTransactionDocumentData.SplitterDistance = 246;
            this.splitContainerTransactionDocumentData.TabIndex = 1;
            // 
            // splitContainerTransactionDocuments
            // 
            this.splitContainerTransactionDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTransactionDocuments.Name = "splitContainerTransactionDocuments";
            // 
            // splitContainerTransactionDocuments.Panel1
            // 
            this.splitContainerTransactionDocuments.Panel1.Controls.Add(this.panelHistoryWebbrowser);
            // 
            // splitContainerTransactionDocuments.Panel2
            // 
            this.splitContainerTransactionDocuments.Panel2.Controls.Add(this.panelHistoryImage);
            this.splitContainerTransactionDocuments.Panel2.Controls.Add(this.toolStripDocumentImage);
            this.splitContainerTransactionDocuments.Size = new System.Drawing.Size(501, 246);
            this.splitContainerTransactionDocuments.SplitterDistance = 243;
            this.splitContainerTransactionDocuments.TabIndex = 0;
            this.splitContainerTransactionDocuments.Visible = false;
            // 
            // panelHistoryWebbrowser
            // 
            this.panelHistoryWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelHistoryWebbrowser.Controls.Add(this.webBrowserTransactionDocuments);
            this.panelHistoryWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistoryWebbrowser.Location = new System.Drawing.Point(0, 0);
            this.panelHistoryWebbrowser.Name = "panelHistoryWebbrowser";
            this.panelHistoryWebbrowser.Size = new System.Drawing.Size(243, 246);
            this.panelHistoryWebbrowser.TabIndex = 1;
            // 
            // webBrowserTransactionDocuments
            // 
            this.webBrowserTransactionDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            this.webBrowserTransactionDocuments.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserTransactionDocuments.Name = "webBrowserTransactionDocuments";
            this.webBrowserTransactionDocuments.Size = new System.Drawing.Size(239, 242);
            this.webBrowserTransactionDocuments.TabIndex = 0;
            // 
            // panelHistoryImage
            // 
            this.panelHistoryImage.AutoScroll = true;
            this.panelHistoryImage.Controls.Add(this.pictureBoxTransactionDocuments);
            this.panelHistoryImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistoryImage.Location = new System.Drawing.Point(0, 0);
            this.panelHistoryImage.Name = "panelHistoryImage";
            this.panelHistoryImage.Size = new System.Drawing.Size(254, 246);
            this.panelHistoryImage.TabIndex = 1;
            // 
            // pictureBoxTransactionDocuments
            // 
            this.pictureBoxTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTransactionDocuments.Name = "pictureBoxTransactionDocuments";
            this.pictureBoxTransactionDocuments.Size = new System.Drawing.Size(2000, 2000);
            this.pictureBoxTransactionDocuments.TabIndex = 0;
            this.pictureBoxTransactionDocuments.TabStop = false;
            // 
            // toolStripDocumentImage
            // 
            this.toolStripDocumentImage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripDocumentImage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDocumentImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDocumentZoomAdapt,
            this.toolStripButtonDocumentZoom100Percent});
            this.toolStripDocumentImage.Location = new System.Drawing.Point(0, 221);
            this.toolStripDocumentImage.Name = "toolStripDocumentImage";
            this.toolStripDocumentImage.Size = new System.Drawing.Size(254, 25);
            this.toolStripDocumentImage.TabIndex = 2;
            this.toolStripDocumentImage.Text = "toolStrip1";
            this.toolStripDocumentImage.Visible = false;
            // 
            // toolStripButtonDocumentZoomAdapt
            // 
            this.toolStripButtonDocumentZoomAdapt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDocumentZoomAdapt.Image = global::DiversityCollection.Resource.ZoomAdapt;
            this.toolStripButtonDocumentZoomAdapt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDocumentZoomAdapt.Name = "toolStripButtonDocumentZoomAdapt";
            this.toolStripButtonDocumentZoomAdapt.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDocumentZoomAdapt.Text = "Adapt size of image to available space";
            // 
            // toolStripButtonDocumentZoom100Percent
            // 
            this.toolStripButtonDocumentZoom100Percent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDocumentZoom100Percent.Image = global::DiversityCollection.Resource.Zoom100;
            this.toolStripButtonDocumentZoom100Percent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDocumentZoom100Percent.Name = "toolStripButtonDocumentZoom100Percent";
            this.toolStripButtonDocumentZoom100Percent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDocumentZoom100Percent.Text = "toolStripButton2";
            // 
            // tableLayoutPanelTransactionDocument
            // 
            this.tableLayoutPanelTransactionDocument.ColumnCount = 4;
            this.tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTransactionDocument.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelTransactionDocument.Controls.Add(this.labelTransactionDocumentDisplayText, 0, 0);
            this.tableLayoutPanelTransactionDocument.Controls.Add(this.textBoxTransactionDocumentInternalNotes, 1, 1);
            this.tableLayoutPanelTransactionDocument.Controls.Add(this.textBoxTransactionDocumentDisplayText, 1, 0);
            this.tableLayoutPanelTransactionDocument.Controls.Add(this.labelDocumentNotes, 0, 1);
            this.tableLayoutPanelTransactionDocument.Controls.Add(this.labelDocumentType, 2, 0);
            this.tableLayoutPanelTransactionDocument.Controls.Add(this.comboBoxDocumentType, 3, 0);
            this.tableLayoutPanelTransactionDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTransactionDocument.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTransactionDocument.Name = "tableLayoutPanelTransactionDocument";
            this.tableLayoutPanelTransactionDocument.RowCount = 2;
            this.tableLayoutPanelTransactionDocument.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTransactionDocument.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTransactionDocument.Size = new System.Drawing.Size(501, 77);
            this.tableLayoutPanelTransactionDocument.TabIndex = 5;
            this.tableLayoutPanelTransactionDocument.Visible = false;
            // 
            // labelTransactionDocumentDisplayText
            // 
            this.labelTransactionDocumentDisplayText.AutoSize = true;
            this.labelTransactionDocumentDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransactionDocumentDisplayText.Location = new System.Drawing.Point(3, 0);
            this.labelTransactionDocumentDisplayText.Name = "labelTransactionDocumentDisplayText";
            this.labelTransactionDocumentDisplayText.Size = new System.Drawing.Size(64, 27);
            this.labelTransactionDocumentDisplayText.TabIndex = 0;
            this.labelTransactionDocumentDisplayText.Text = "Display text:";
            this.labelTransactionDocumentDisplayText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTransactionDocumentInternalNotes
            // 
            this.tableLayoutPanelTransactionDocument.SetColumnSpan(this.textBoxTransactionDocumentInternalNotes, 3);
            this.textBoxTransactionDocumentInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionDocumentBindingSource, "InternalNotes", true));
            this.textBoxTransactionDocumentInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTransactionDocumentInternalNotes.Location = new System.Drawing.Point(73, 30);
            this.textBoxTransactionDocumentInternalNotes.Multiline = true;
            this.textBoxTransactionDocumentInternalNotes.Name = "textBoxTransactionDocumentInternalNotes";
            this.textBoxTransactionDocumentInternalNotes.ReadOnly = true;
            this.textBoxTransactionDocumentInternalNotes.Size = new System.Drawing.Size(425, 44);
            this.textBoxTransactionDocumentInternalNotes.TabIndex = 1;
            // 
            // textBoxTransactionDocumentDisplayText
            // 
            this.textBoxTransactionDocumentDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionDocumentBindingSource, "DisplayText", true));
            this.textBoxTransactionDocumentDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTransactionDocumentDisplayText.Location = new System.Drawing.Point(73, 3);
            this.textBoxTransactionDocumentDisplayText.Name = "textBoxTransactionDocumentDisplayText";
            this.textBoxTransactionDocumentDisplayText.ReadOnly = true;
            this.textBoxTransactionDocumentDisplayText.Size = new System.Drawing.Size(189, 20);
            this.textBoxTransactionDocumentDisplayText.TabIndex = 1;
            // 
            // labelDocumentNotes
            // 
            this.labelDocumentNotes.AutoSize = true;
            this.labelDocumentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDocumentNotes.Location = new System.Drawing.Point(3, 27);
            this.labelDocumentNotes.Name = "labelDocumentNotes";
            this.labelDocumentNotes.Size = new System.Drawing.Size(64, 50);
            this.labelDocumentNotes.TabIndex = 2;
            this.labelDocumentNotes.Text = "Notes:";
            this.labelDocumentNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDocumentType
            // 
            this.labelDocumentType.AutoSize = true;
            this.labelDocumentType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDocumentType.Location = new System.Drawing.Point(268, 0);
            this.labelDocumentType.Name = "labelDocumentType";
            this.labelDocumentType.Size = new System.Drawing.Size(34, 27);
            this.labelDocumentType.TabIndex = 3;
            this.labelDocumentType.Text = "Type:";
            this.labelDocumentType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxDocumentType
            // 
            this.comboBoxDocumentType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.transactionDocumentBindingSource, "DocumentType", true));
            this.comboBoxDocumentType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDocumentType.Enabled = false;
            this.comboBoxDocumentType.FormattingEnabled = true;
            this.comboBoxDocumentType.Location = new System.Drawing.Point(308, 3);
            this.comboBoxDocumentType.Name = "comboBoxDocumentType";
            this.comboBoxDocumentType.Size = new System.Drawing.Size(190, 21);
            this.comboBoxDocumentType.TabIndex = 4;
            // 
            // tabPagePayment
            // 
            this.tabPagePayment.Controls.Add(this.splitContainerPayment);
            this.tabPagePayment.ImageIndex = 13;
            this.tabPagePayment.Location = new System.Drawing.Point(4, 22);
            this.tabPagePayment.Name = "tabPagePayment";
            this.tabPagePayment.Size = new System.Drawing.Size(602, 327);
            this.tabPagePayment.TabIndex = 14;
            this.tabPagePayment.Text = "Payments";
            this.tabPagePayment.UseVisualStyleBackColor = true;
            // 
            // splitContainerPayment
            // 
            this.splitContainerPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerPayment.Location = new System.Drawing.Point(0, 0);
            this.splitContainerPayment.Name = "splitContainerPayment";
            // 
            // splitContainerPayment.Panel1
            // 
            this.splitContainerPayment.Panel1.Controls.Add(this.listBoxPayment);
            // 
            // splitContainerPayment.Panel2
            // 
            this.splitContainerPayment.Panel2.Controls.Add(this.tableLayoutPanelPayment);
            this.splitContainerPayment.Panel2.Enabled = false;
            this.splitContainerPayment.Size = new System.Drawing.Size(602, 327);
            this.splitContainerPayment.SplitterDistance = 104;
            this.splitContainerPayment.TabIndex = 0;
            // 
            // listBoxPayment
            // 
            this.listBoxPayment.DisplayMember = "Identifier";
            this.listBoxPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxPayment.FormattingEnabled = true;
            this.listBoxPayment.Location = new System.Drawing.Point(0, 0);
            this.listBoxPayment.Name = "listBoxPayment";
            this.listBoxPayment.Size = new System.Drawing.Size(104, 327);
            this.listBoxPayment.TabIndex = 1;
            this.listBoxPayment.ValueMember = "PaymentID";
            // 
            // tableLayoutPanelPayment
            // 
            this.tableLayoutPanelPayment.ColumnCount = 6;
            this.tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPayment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentIdentifier, 0, 0);
            this.tableLayoutPanelPayment.Controls.Add(this.textBoxPaymentIdentifier, 1, 0);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentAmount, 0, 2);
            this.tableLayoutPanelPayment.Controls.Add(this.textBoxPaymentAmount, 2, 2);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentCurrency, 3, 2);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentForeignAmount, 0, 3);
            this.tableLayoutPanelPayment.Controls.Add(this.textBoxPaymentForeignAmount, 2, 3);
            this.tableLayoutPanelPayment.Controls.Add(this.textBoxPaymentForeignAmountCurrency, 3, 3);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentPayer, 0, 4);
            this.tableLayoutPanelPayment.Controls.Add(this.userControlModuleRelatedEntryPaymentPayer, 1, 4);
            this.tableLayoutPanelPayment.Controls.Add(this.userControlModuleRelatedEntryPaymentRecipient, 1, 5);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentRecipient, 0, 5);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentDate, 0, 6);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentDateSupplement, 3, 6);
            this.tableLayoutPanelPayment.Controls.Add(this.textBoxPaymentDateSupplement, 4, 6);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentNotes, 0, 7);
            this.tableLayoutPanelPayment.Controls.Add(this.textBoxPaymentNotes, 1, 7);
            this.tableLayoutPanelPayment.Controls.Add(this.labelPaymentURI, 0, 1);
            this.tableLayoutPanelPayment.Controls.Add(this.textBoxPaymentURI, 1, 1);
            this.tableLayoutPanelPayment.Controls.Add(this.textBoxPaymentDate, 1, 6);
            this.tableLayoutPanelPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelPayment.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelPayment.Name = "tableLayoutPanelPayment";
            this.tableLayoutPanelPayment.RowCount = 8;
            this.tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelPayment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPayment.Size = new System.Drawing.Size(494, 327);
            this.tableLayoutPanelPayment.TabIndex = 0;
            // 
            // labelPaymentIdentifier
            // 
            this.labelPaymentIdentifier.AutoSize = true;
            this.labelPaymentIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentIdentifier.Location = new System.Drawing.Point(3, 0);
            this.labelPaymentIdentifier.Name = "labelPaymentIdentifier";
            this.labelPaymentIdentifier.Size = new System.Drawing.Size(70, 26);
            this.labelPaymentIdentifier.TabIndex = 0;
            this.labelPaymentIdentifier.Text = "Identifier:";
            this.labelPaymentIdentifier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentIdentifier
            // 
            this.tableLayoutPanelPayment.SetColumnSpan(this.textBoxPaymentIdentifier, 5);
            this.textBoxPaymentIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPaymentIdentifier.Location = new System.Drawing.Point(79, 3);
            this.textBoxPaymentIdentifier.Name = "textBoxPaymentIdentifier";
            this.textBoxPaymentIdentifier.Size = new System.Drawing.Size(412, 20);
            this.textBoxPaymentIdentifier.TabIndex = 0;
            // 
            // labelPaymentAmount
            // 
            this.labelPaymentAmount.AutoSize = true;
            this.tableLayoutPanelPayment.SetColumnSpan(this.labelPaymentAmount, 2);
            this.labelPaymentAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentAmount.Location = new System.Drawing.Point(3, 52);
            this.labelPaymentAmount.Name = "labelPaymentAmount";
            this.labelPaymentAmount.Size = new System.Drawing.Size(97, 26);
            this.labelPaymentAmount.TabIndex = 2;
            this.labelPaymentAmount.Text = "Amount:";
            this.labelPaymentAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentAmount
            // 
            this.textBoxPaymentAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPaymentAmount.Location = new System.Drawing.Point(106, 55);
            this.textBoxPaymentAmount.Name = "textBoxPaymentAmount";
            this.textBoxPaymentAmount.Size = new System.Drawing.Size(200, 20);
            this.textBoxPaymentAmount.TabIndex = 2;
            this.textBoxPaymentAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelPaymentCurrency
            // 
            this.labelPaymentCurrency.AutoSize = true;
            this.labelPaymentCurrency.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelPaymentCurrency.Location = new System.Drawing.Point(312, 52);
            this.labelPaymentCurrency.Name = "labelPaymentCurrency";
            this.labelPaymentCurrency.Size = new System.Drawing.Size(13, 26);
            this.labelPaymentCurrency.TabIndex = 4;
            this.labelPaymentCurrency.Text = "€";
            this.labelPaymentCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPaymentForeignAmount
            // 
            this.labelPaymentForeignAmount.AutoSize = true;
            this.tableLayoutPanelPayment.SetColumnSpan(this.labelPaymentForeignAmount, 2);
            this.labelPaymentForeignAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentForeignAmount.Location = new System.Drawing.Point(3, 78);
            this.labelPaymentForeignAmount.Name = "labelPaymentForeignAmount";
            this.labelPaymentForeignAmount.Size = new System.Drawing.Size(97, 26);
            this.labelPaymentForeignAmount.TabIndex = 5;
            this.labelPaymentForeignAmount.Text = "in foreign currency:";
            this.labelPaymentForeignAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentForeignAmount
            // 
            this.textBoxPaymentForeignAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPaymentForeignAmount.Location = new System.Drawing.Point(106, 81);
            this.textBoxPaymentForeignAmount.Name = "textBoxPaymentForeignAmount";
            this.textBoxPaymentForeignAmount.Size = new System.Drawing.Size(200, 20);
            this.textBoxPaymentForeignAmount.TabIndex = 3;
            this.textBoxPaymentForeignAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxPaymentForeignAmountCurrency
            // 
            this.tableLayoutPanelPayment.SetColumnSpan(this.textBoxPaymentForeignAmountCurrency, 2);
            this.textBoxPaymentForeignAmountCurrency.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxPaymentForeignAmountCurrency.Location = new System.Drawing.Point(312, 81);
            this.textBoxPaymentForeignAmountCurrency.Name = "textBoxPaymentForeignAmountCurrency";
            this.textBoxPaymentForeignAmountCurrency.Size = new System.Drawing.Size(1, 20);
            this.textBoxPaymentForeignAmountCurrency.TabIndex = 4;
            // 
            // labelPaymentPayer
            // 
            this.labelPaymentPayer.AutoSize = true;
            this.labelPaymentPayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentPayer.Location = new System.Drawing.Point(3, 104);
            this.labelPaymentPayer.Name = "labelPaymentPayer";
            this.labelPaymentPayer.Size = new System.Drawing.Size(70, 28);
            this.labelPaymentPayer.TabIndex = 8;
            this.labelPaymentPayer.Text = "Payed by:";
            this.labelPaymentPayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryPaymentPayer
            // 
            this.userControlModuleRelatedEntryPaymentPayer.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelPayment.SetColumnSpan(this.userControlModuleRelatedEntryPaymentPayer, 5);
            this.userControlModuleRelatedEntryPaymentPayer.DependsOnUri = "";
            this.userControlModuleRelatedEntryPaymentPayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryPaymentPayer.Domain = "";
            this.userControlModuleRelatedEntryPaymentPayer.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryPaymentPayer.Location = new System.Drawing.Point(79, 107);
            this.userControlModuleRelatedEntryPaymentPayer.Module = null;
            this.userControlModuleRelatedEntryPaymentPayer.Name = "userControlModuleRelatedEntryPaymentPayer";
            this.userControlModuleRelatedEntryPaymentPayer.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryPaymentPayer.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryPaymentPayer.ShowInfo = false;
            this.userControlModuleRelatedEntryPaymentPayer.Size = new System.Drawing.Size(412, 22);
            this.userControlModuleRelatedEntryPaymentPayer.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryPaymentPayer.TabIndex = 5;
            // 
            // userControlModuleRelatedEntryPaymentRecipient
            // 
            this.userControlModuleRelatedEntryPaymentRecipient.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelPayment.SetColumnSpan(this.userControlModuleRelatedEntryPaymentRecipient, 5);
            this.userControlModuleRelatedEntryPaymentRecipient.DependsOnUri = "";
            this.userControlModuleRelatedEntryPaymentRecipient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryPaymentRecipient.Domain = "";
            this.userControlModuleRelatedEntryPaymentRecipient.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryPaymentRecipient.Location = new System.Drawing.Point(79, 135);
            this.userControlModuleRelatedEntryPaymentRecipient.Module = null;
            this.userControlModuleRelatedEntryPaymentRecipient.Name = "userControlModuleRelatedEntryPaymentRecipient";
            this.userControlModuleRelatedEntryPaymentRecipient.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryPaymentRecipient.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryPaymentRecipient.ShowInfo = false;
            this.userControlModuleRelatedEntryPaymentRecipient.Size = new System.Drawing.Size(412, 22);
            this.userControlModuleRelatedEntryPaymentRecipient.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryPaymentRecipient.TabIndex = 6;
            // 
            // labelPaymentRecipient
            // 
            this.labelPaymentRecipient.AutoSize = true;
            this.labelPaymentRecipient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentRecipient.Location = new System.Drawing.Point(3, 132);
            this.labelPaymentRecipient.Name = "labelPaymentRecipient";
            this.labelPaymentRecipient.Size = new System.Drawing.Size(70, 28);
            this.labelPaymentRecipient.TabIndex = 11;
            this.labelPaymentRecipient.Text = "Received by:";
            this.labelPaymentRecipient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPaymentDate
            // 
            this.labelPaymentDate.AutoSize = true;
            this.labelPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentDate.Location = new System.Drawing.Point(3, 160);
            this.labelPaymentDate.Name = "labelPaymentDate";
            this.labelPaymentDate.Size = new System.Drawing.Size(70, 26);
            this.labelPaymentDate.TabIndex = 12;
            this.labelPaymentDate.Text = "Date:";
            this.labelPaymentDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPaymentDateSupplement
            // 
            this.labelPaymentDateSupplement.AutoSize = true;
            this.labelPaymentDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentDateSupplement.Location = new System.Drawing.Point(312, 160);
            this.labelPaymentDateSupplement.Name = "labelPaymentDateSupplement";
            this.labelPaymentDateSupplement.Size = new System.Drawing.Size(40, 26);
            this.labelPaymentDateSupplement.TabIndex = 8;
            this.labelPaymentDateSupplement.Text = "Suppl.:";
            this.labelPaymentDateSupplement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentDateSupplement
            // 
            this.tableLayoutPanelPayment.SetColumnSpan(this.textBoxPaymentDateSupplement, 2);
            this.textBoxPaymentDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPaymentDateSupplement.Location = new System.Drawing.Point(358, 163);
            this.textBoxPaymentDateSupplement.Name = "textBoxPaymentDateSupplement";
            this.textBoxPaymentDateSupplement.Size = new System.Drawing.Size(133, 20);
            this.textBoxPaymentDateSupplement.TabIndex = 7;
            // 
            // labelPaymentNotes
            // 
            this.labelPaymentNotes.AutoSize = true;
            this.labelPaymentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentNotes.Location = new System.Drawing.Point(3, 192);
            this.labelPaymentNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelPaymentNotes.Name = "labelPaymentNotes";
            this.labelPaymentNotes.Size = new System.Drawing.Size(70, 135);
            this.labelPaymentNotes.TabIndex = 16;
            this.labelPaymentNotes.Text = "Notes:";
            this.labelPaymentNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxPaymentNotes
            // 
            this.tableLayoutPanelPayment.SetColumnSpan(this.textBoxPaymentNotes, 5);
            this.textBoxPaymentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPaymentNotes.Location = new System.Drawing.Point(79, 189);
            this.textBoxPaymentNotes.Multiline = true;
            this.textBoxPaymentNotes.Name = "textBoxPaymentNotes";
            this.textBoxPaymentNotes.Size = new System.Drawing.Size(412, 135);
            this.textBoxPaymentNotes.TabIndex = 9;
            // 
            // labelPaymentURI
            // 
            this.labelPaymentURI.AutoSize = true;
            this.labelPaymentURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPaymentURI.Location = new System.Drawing.Point(3, 26);
            this.labelPaymentURI.Name = "labelPaymentURI";
            this.labelPaymentURI.Size = new System.Drawing.Size(70, 26);
            this.labelPaymentURI.TabIndex = 18;
            this.labelPaymentURI.Text = "Ext. admin.:";
            this.labelPaymentURI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPaymentURI
            // 
            this.tableLayoutPanelPayment.SetColumnSpan(this.textBoxPaymentURI, 5);
            this.textBoxPaymentURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPaymentURI.Location = new System.Drawing.Point(79, 29);
            this.textBoxPaymentURI.Name = "textBoxPaymentURI";
            this.textBoxPaymentURI.Size = new System.Drawing.Size(412, 20);
            this.textBoxPaymentURI.TabIndex = 1;
            // 
            // textBoxPaymentDate
            // 
            this.tableLayoutPanelPayment.SetColumnSpan(this.textBoxPaymentDate, 2);
            this.textBoxPaymentDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPaymentDate.Location = new System.Drawing.Point(79, 163);
            this.textBoxPaymentDate.Name = "textBoxPaymentDate";
            this.textBoxPaymentDate.Size = new System.Drawing.Size(227, 20);
            this.textBoxPaymentDate.TabIndex = 23;
            // 
            // tabPageAgents
            // 
            this.tabPageAgents.Controls.Add(this.splitContainerAgents);
            this.tabPageAgents.ImageIndex = 14;
            this.tabPageAgents.Location = new System.Drawing.Point(4, 22);
            this.tabPageAgents.Name = "tabPageAgents";
            this.tabPageAgents.Size = new System.Drawing.Size(602, 327);
            this.tabPageAgents.TabIndex = 15;
            this.tabPageAgents.Text = "Agents";
            this.tabPageAgents.UseVisualStyleBackColor = true;
            // 
            // splitContainerAgents
            // 
            this.splitContainerAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerAgents.Location = new System.Drawing.Point(0, 0);
            this.splitContainerAgents.Name = "splitContainerAgents";
            // 
            // splitContainerAgents.Panel1
            // 
            this.splitContainerAgents.Panel1.Controls.Add(this.listBoxAgents);
            // 
            // splitContainerAgents.Panel2
            // 
            this.splitContainerAgents.Panel2.Controls.Add(this.tableLayoutPanelAgents);
            this.splitContainerAgents.Panel2.Enabled = false;
            this.splitContainerAgents.Size = new System.Drawing.Size(602, 327);
            this.splitContainerAgents.SplitterDistance = 199;
            this.splitContainerAgents.TabIndex = 0;
            // 
            // listBoxAgents
            // 
            this.listBoxAgents.DisplayMember = "AgentName";
            this.listBoxAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAgents.FormattingEnabled = true;
            this.listBoxAgents.Location = new System.Drawing.Point(0, 0);
            this.listBoxAgents.Name = "listBoxAgents";
            this.listBoxAgents.Size = new System.Drawing.Size(199, 327);
            this.listBoxAgents.TabIndex = 0;
            this.listBoxAgents.ValueMember = "TransactionAgentID";
            // 
            // tableLayoutPanelAgents
            // 
            this.tableLayoutPanelAgents.ColumnCount = 2;
            this.tableLayoutPanelAgents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAgents.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAgents.Controls.Add(this.userControlModuleRelatedEntryAgent, 0, 0);
            this.tableLayoutPanelAgents.Controls.Add(this.labelAgentRole, 0, 1);
            this.tableLayoutPanelAgents.Controls.Add(this.labelAgentNotes, 0, 2);
            this.tableLayoutPanelAgents.Controls.Add(this.textBoxAgentNotes, 1, 2);
            this.tableLayoutPanelAgents.Controls.Add(this.comboBoxAgentRole, 1, 1);
            this.tableLayoutPanelAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAgents.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelAgents.Name = "tableLayoutPanelAgents";
            this.tableLayoutPanelAgents.RowCount = 3;
            this.tableLayoutPanelAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAgents.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAgents.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAgents.Size = new System.Drawing.Size(399, 327);
            this.tableLayoutPanelAgents.TabIndex = 0;
            // 
            // userControlModuleRelatedEntryAgent
            // 
            this.userControlModuleRelatedEntryAgent.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelAgents.SetColumnSpan(this.userControlModuleRelatedEntryAgent, 2);
            this.userControlModuleRelatedEntryAgent.DependsOnUri = "";
            this.userControlModuleRelatedEntryAgent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryAgent.Domain = "";
            this.userControlModuleRelatedEntryAgent.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryAgent.Location = new System.Drawing.Point(3, 3);
            this.userControlModuleRelatedEntryAgent.Module = null;
            this.userControlModuleRelatedEntryAgent.Name = "userControlModuleRelatedEntryAgent";
            this.userControlModuleRelatedEntryAgent.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryAgent.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryAgent.ShowInfo = false;
            this.userControlModuleRelatedEntryAgent.Size = new System.Drawing.Size(393, 22);
            this.userControlModuleRelatedEntryAgent.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryAgent.TabIndex = 0;
            // 
            // labelAgentRole
            // 
            this.labelAgentRole.AutoSize = true;
            this.labelAgentRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAgentRole.Location = new System.Drawing.Point(3, 28);
            this.labelAgentRole.Name = "labelAgentRole";
            this.labelAgentRole.Size = new System.Drawing.Size(38, 27);
            this.labelAgentRole.TabIndex = 1;
            this.labelAgentRole.Text = "Role:";
            this.labelAgentRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAgentNotes
            // 
            this.labelAgentNotes.AutoSize = true;
            this.labelAgentNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelAgentNotes.Location = new System.Drawing.Point(3, 61);
            this.labelAgentNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelAgentNotes.Name = "labelAgentNotes";
            this.labelAgentNotes.Size = new System.Drawing.Size(38, 13);
            this.labelAgentNotes.TabIndex = 2;
            this.labelAgentNotes.Text = "Notes:";
            // 
            // textBoxAgentNotes
            // 
            this.textBoxAgentNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAgentNotes.Location = new System.Drawing.Point(47, 58);
            this.textBoxAgentNotes.Multiline = true;
            this.textBoxAgentNotes.Name = "textBoxAgentNotes";
            this.textBoxAgentNotes.Size = new System.Drawing.Size(349, 266);
            this.textBoxAgentNotes.TabIndex = 3;
            // 
            // comboBoxAgentRole
            // 
            this.comboBoxAgentRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAgentRole.FormattingEnabled = true;
            this.comboBoxAgentRole.Location = new System.Drawing.Point(47, 31);
            this.comboBoxAgentRole.Name = "comboBoxAgentRole";
            this.comboBoxAgentRole.Size = new System.Drawing.Size(349, 21);
            this.comboBoxAgentRole.TabIndex = 4;
            // 
            // tabPageIdentifier
            // 
            this.tabPageIdentifier.Controls.Add(this.splitContainerIdentifier);
            this.tabPageIdentifier.ImageIndex = 15;
            this.tabPageIdentifier.Location = new System.Drawing.Point(4, 22);
            this.tabPageIdentifier.Name = "tabPageIdentifier";
            this.tabPageIdentifier.Size = new System.Drawing.Size(602, 327);
            this.tabPageIdentifier.TabIndex = 16;
            this.tabPageIdentifier.Text = "Identifier";
            this.tabPageIdentifier.UseVisualStyleBackColor = true;
            // 
            // splitContainerIdentifier
            // 
            this.splitContainerIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerIdentifier.Location = new System.Drawing.Point(0, 0);
            this.splitContainerIdentifier.Name = "splitContainerIdentifier";
            // 
            // splitContainerIdentifier.Panel1
            // 
            this.splitContainerIdentifier.Panel1.Controls.Add(this.listBoxIdentifier);
            // 
            // splitContainerIdentifier.Panel2
            // 
            this.splitContainerIdentifier.Panel2.Controls.Add(this.tableLayoutPanelIdentifier);
            this.splitContainerIdentifier.Size = new System.Drawing.Size(602, 327);
            this.splitContainerIdentifier.SplitterDistance = 199;
            this.splitContainerIdentifier.TabIndex = 0;
            // 
            // listBoxIdentifier
            // 
            this.listBoxIdentifier.DisplayMember = "Identifier";
            this.listBoxIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxIdentifier.FormattingEnabled = true;
            this.listBoxIdentifier.IntegralHeight = false;
            this.listBoxIdentifier.Location = new System.Drawing.Point(0, 0);
            this.listBoxIdentifier.Name = "listBoxIdentifier";
            this.listBoxIdentifier.Size = new System.Drawing.Size(199, 327);
            this.listBoxIdentifier.TabIndex = 0;
            this.listBoxIdentifier.ValueMember = "ID";
            // 
            // tableLayoutPanelIdentifier
            // 
            this.tableLayoutPanelIdentifier.ColumnCount = 3;
            this.tableLayoutPanelIdentifier.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelIdentifier.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelIdentifier.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelIdentifier.Controls.Add(this.labelIdentifierType, 0, 0);
            this.tableLayoutPanelIdentifier.Controls.Add(this.labelIdentifier, 0, 1);
            this.tableLayoutPanelIdentifier.Controls.Add(this.textBoxIdentifierType, 1, 0);
            this.tableLayoutPanelIdentifier.Controls.Add(this.textBoxIdentifier, 1, 1);
            this.tableLayoutPanelIdentifier.Controls.Add(this.buttonIdenitifierURL, 2, 2);
            this.tableLayoutPanelIdentifier.Controls.Add(this.labelIdenitifierURL, 0, 2);
            this.tableLayoutPanelIdentifier.Controls.Add(this.textBoxIdenitifierURL, 1, 2);
            this.tableLayoutPanelIdentifier.Controls.Add(this.labelIdenitifierNotes, 0, 3);
            this.tableLayoutPanelIdentifier.Controls.Add(this.textBoxIdenitifierNotes, 1, 3);
            this.tableLayoutPanelIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelIdentifier.Enabled = false;
            this.tableLayoutPanelIdentifier.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelIdentifier.Name = "tableLayoutPanelIdentifier";
            this.tableLayoutPanelIdentifier.RowCount = 5;
            this.tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelIdentifier.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelIdentifier.Size = new System.Drawing.Size(399, 327);
            this.tableLayoutPanelIdentifier.TabIndex = 0;
            // 
            // labelIdentifierType
            // 
            this.labelIdentifierType.AutoSize = true;
            this.labelIdentifierType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIdentifierType.Location = new System.Drawing.Point(3, 0);
            this.labelIdentifierType.Name = "labelIdentifierType";
            this.labelIdentifierType.Size = new System.Drawing.Size(50, 26);
            this.labelIdentifierType.TabIndex = 0;
            this.labelIdentifierType.Text = "Type:";
            this.labelIdentifierType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelIdentifier
            // 
            this.labelIdentifier.AutoSize = true;
            this.labelIdentifier.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelIdentifier.Location = new System.Drawing.Point(3, 32);
            this.labelIdentifier.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelIdentifier.Name = "labelIdentifier";
            this.labelIdentifier.Size = new System.Drawing.Size(50, 13);
            this.labelIdentifier.TabIndex = 1;
            this.labelIdentifier.Text = "Identifier:";
            this.labelIdentifier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIdentifierType
            // 
            this.tableLayoutPanelIdentifier.SetColumnSpan(this.textBoxIdentifierType, 2);
            this.textBoxIdentifierType.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxIdentifierType.Location = new System.Drawing.Point(59, 3);
            this.textBoxIdentifierType.Name = "textBoxIdentifierType";
            this.textBoxIdentifierType.ReadOnly = true;
            this.textBoxIdentifierType.Size = new System.Drawing.Size(337, 20);
            this.textBoxIdentifierType.TabIndex = 2;
            // 
            // textBoxIdentifier
            // 
            this.tableLayoutPanelIdentifier.SetColumnSpan(this.textBoxIdentifier, 2);
            this.textBoxIdentifier.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxIdentifier.Location = new System.Drawing.Point(59, 29);
            this.textBoxIdentifier.Name = "textBoxIdentifier";
            this.textBoxIdentifier.ReadOnly = true;
            this.textBoxIdentifier.Size = new System.Drawing.Size(337, 20);
            this.textBoxIdentifier.TabIndex = 3;
            // 
            // buttonIdenitifierURL
            // 
            this.buttonIdenitifierURL.Image = global::DiversityCollection.Resource.Browse;
            this.buttonIdenitifierURL.Location = new System.Drawing.Point(374, 53);
            this.buttonIdenitifierURL.Margin = new System.Windows.Forms.Padding(1);
            this.buttonIdenitifierURL.Name = "buttonIdenitifierURL";
            this.buttonIdenitifierURL.Size = new System.Drawing.Size(24, 24);
            this.buttonIdenitifierURL.TabIndex = 4;
            this.buttonIdenitifierURL.UseVisualStyleBackColor = true;
            // 
            // labelIdenitifierURL
            // 
            this.labelIdenitifierURL.AutoSize = true;
            this.labelIdenitifierURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIdenitifierURL.Location = new System.Drawing.Point(3, 52);
            this.labelIdenitifierURL.Name = "labelIdenitifierURL";
            this.labelIdenitifierURL.Size = new System.Drawing.Size(50, 26);
            this.labelIdenitifierURL.TabIndex = 5;
            this.labelIdenitifierURL.Text = "URL:";
            this.labelIdenitifierURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIdenitifierURL
            // 
            this.textBoxIdenitifierURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxIdenitifierURL.Location = new System.Drawing.Point(59, 55);
            this.textBoxIdenitifierURL.Name = "textBoxIdenitifierURL";
            this.textBoxIdenitifierURL.Size = new System.Drawing.Size(311, 20);
            this.textBoxIdenitifierURL.TabIndex = 6;
            // 
            // labelIdenitifierNotes
            // 
            this.labelIdenitifierNotes.AutoSize = true;
            this.labelIdenitifierNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelIdenitifierNotes.Location = new System.Drawing.Point(3, 84);
            this.labelIdenitifierNotes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelIdenitifierNotes.Name = "labelIdenitifierNotes";
            this.labelIdenitifierNotes.Size = new System.Drawing.Size(50, 13);
            this.labelIdenitifierNotes.TabIndex = 7;
            this.labelIdenitifierNotes.Text = "Notes:";
            this.labelIdenitifierNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxIdenitifierNotes
            // 
            this.tableLayoutPanelIdentifier.SetColumnSpan(this.textBoxIdenitifierNotes, 2);
            this.textBoxIdenitifierNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxIdenitifierNotes.Location = new System.Drawing.Point(59, 81);
            this.textBoxIdenitifierNotes.Multiline = true;
            this.textBoxIdenitifierNotes.Name = "textBoxIdenitifierNotes";
            this.tableLayoutPanelIdentifier.SetRowSpan(this.textBoxIdenitifierNotes, 2);
            this.textBoxIdenitifierNotes.Size = new System.Drawing.Size(337, 243);
            this.textBoxIdenitifierNotes.TabIndex = 8;
            // 
            // transactionDocumentTableAdapter
            // 
            this.transactionDocumentTableAdapter.ClearBeforeFill = true;
            // 
            // transactionTableAdapter
            // 
            this.transactionTableAdapter.ClearBeforeFill = true;
            // 
            // UserControl_Permit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlTransaction);
            this.Name = "UserControl_Permit";
            this.Size = new System.Drawing.Size(610, 353);
            this.tabControlTransaction.ResumeLayout(false);
            this.tabPageDataEntry.ResumeLayout(false);
            this.tableLayoutPanelCollection.ResumeLayout(false);
            this.tableLayoutPanelCollection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transactionBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).EndInit();
            this.groupBoxFrom.ResumeLayout(false);
            this.tableLayoutPanelFrom.ResumeLayout(false);
            this.tableLayoutPanelFrom.PerformLayout();
            this.groupBoxTo.ResumeLayout(false);
            this.tableLayoutPanelTo.ResumeLayout(false);
            this.tableLayoutPanelTo.PerformLayout();
            this.groupBoxMaterial.ResumeLayout(false);
            this.tableLayoutPanelMaterial.ResumeLayout(false);
            this.tableLayoutPanelMaterial.PerformLayout();
            this.tabPageDocuments.ResumeLayout(false);
            this.splitContainerDocuments.Panel1.ResumeLayout(false);
            this.splitContainerDocuments.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDocuments)).EndInit();
            this.splitContainerDocuments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.transactionDocumentBindingSource)).EndInit();
            this.splitContainerTransactionDocumentData.Panel1.ResumeLayout(false);
            this.splitContainerTransactionDocumentData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTransactionDocumentData)).EndInit();
            this.splitContainerTransactionDocumentData.ResumeLayout(false);
            this.splitContainerTransactionDocuments.Panel1.ResumeLayout(false);
            this.splitContainerTransactionDocuments.Panel2.ResumeLayout(false);
            this.splitContainerTransactionDocuments.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTransactionDocuments)).EndInit();
            this.splitContainerTransactionDocuments.ResumeLayout(false);
            this.panelHistoryWebbrowser.ResumeLayout(false);
            this.panelHistoryImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTransactionDocuments)).EndInit();
            this.toolStripDocumentImage.ResumeLayout(false);
            this.toolStripDocumentImage.PerformLayout();
            this.tableLayoutPanelTransactionDocument.ResumeLayout(false);
            this.tableLayoutPanelTransactionDocument.PerformLayout();
            this.tabPagePayment.ResumeLayout(false);
            this.splitContainerPayment.Panel1.ResumeLayout(false);
            this.splitContainerPayment.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPayment)).EndInit();
            this.splitContainerPayment.ResumeLayout(false);
            this.tableLayoutPanelPayment.ResumeLayout(false);
            this.tableLayoutPanelPayment.PerformLayout();
            this.tabPageAgents.ResumeLayout(false);
            this.splitContainerAgents.Panel1.ResumeLayout(false);
            this.splitContainerAgents.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerAgents)).EndInit();
            this.splitContainerAgents.ResumeLayout(false);
            this.tableLayoutPanelAgents.ResumeLayout(false);
            this.tableLayoutPanelAgents.PerformLayout();
            this.tabPageIdentifier.ResumeLayout(false);
            this.splitContainerIdentifier.Panel1.ResumeLayout(false);
            this.splitContainerIdentifier.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerIdentifier)).EndInit();
            this.splitContainerIdentifier.ResumeLayout(false);
            this.tableLayoutPanelIdentifier.ResumeLayout(false);
            this.tableLayoutPanelIdentifier.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlTransaction;
        private System.Windows.Forms.TabPage tabPageDataEntry;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCollection;
        private System.Windows.Forms.Label labelTransactionTitle;
        private System.Windows.Forms.TextBox textBoxTransactionTitle;
        private System.Windows.Forms.Label labelBeginDate;
        private System.Windows.Forms.Label labelInternalNotes;
        private System.Windows.Forms.TextBox textBoxInternalNotes;
        private System.Windows.Forms.Label labelResponsible;
        private System.Windows.Forms.TextBox textBoxInvestigator;
        private System.Windows.Forms.Label labelInvestigator;
        private System.Windows.Forms.GroupBox groupBoxFrom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelFrom;
        private System.Windows.Forms.TextBox textBoxFromTransactionNumber;
        private System.Windows.Forms.Label labelFromTransactionNumber;
        private System.Windows.Forms.TextBox textBoxFromTransactionPartnerName;
        private System.Windows.Forms.GroupBox groupBoxTo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTo;
        private System.Windows.Forms.Label labelToRecipient;
        private System.Windows.Forms.ComboBox comboBoxToRecipient;
        private System.Windows.Forms.TextBox textBoxToTransactionPartnerName;
        private System.Windows.Forms.Label labelDateSupplement;
        private System.Windows.Forms.TextBox textBoxDateSupplement;
        private System.Windows.Forms.TextBox textBoxBeginDate;
        private System.Windows.Forms.TextBox textBoxActualEndDate;
        private System.Windows.Forms.Label labelActualEndDate;
        private System.Windows.Forms.TextBox textBoxAgreedEndDate;
        private System.Windows.Forms.Label labelAgreedEndDate;
        private System.Windows.Forms.TextBox textBoxResponsible;
        private System.Windows.Forms.GroupBox groupBoxMaterial;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMaterial;
        private System.Windows.Forms.TextBox textBoxMaterial;
        private System.Windows.Forms.Label labelMaterialCategory;
        private System.Windows.Forms.Label labelMaterialCollectors;
        private System.Windows.Forms.TextBox textBoxMaterialCollectors;
        private System.Windows.Forms.Label labelMaterialSource;
        private System.Windows.Forms.ComboBox comboBoxMaterialSource;
        private System.Windows.Forms.Label labelMaterialDescription;
        private System.Windows.Forms.TextBox textBoxMaterialDescription;
        private System.Windows.Forms.Label labelNumberOfUnits;
        private System.Windows.Forms.TextBox textBoxNumberOfUnits;
        private System.Windows.Forms.Label labelTransactionID;
        private System.Windows.Forms.TextBox textBoxTransactionID;
        private System.Windows.Forms.TabPage tabPageDocuments;
        private System.Windows.Forms.SplitContainer splitContainerDocuments;
        private System.Windows.Forms.ListBox listBoxTransactionDocuments;
        private System.Windows.Forms.SplitContainer splitContainerTransactionDocumentData;
        private System.Windows.Forms.SplitContainer splitContainerTransactionDocuments;
        private System.Windows.Forms.Panel panelHistoryWebbrowser;
        private System.Windows.Forms.WebBrowser webBrowserTransactionDocuments;
        private System.Windows.Forms.Panel panelHistoryImage;
        private System.Windows.Forms.PictureBox pictureBoxTransactionDocuments;
        private System.Windows.Forms.ToolStrip toolStripDocumentImage;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentZoomAdapt;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentZoom100Percent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransactionDocument;
        private System.Windows.Forms.Label labelTransactionDocumentDisplayText;
        private System.Windows.Forms.TextBox textBoxTransactionDocumentInternalNotes;
        private System.Windows.Forms.TextBox textBoxTransactionDocumentDisplayText;
        private System.Windows.Forms.Label labelDocumentNotes;
        private System.Windows.Forms.Label labelDocumentType;
        private System.Windows.Forms.ComboBox comboBoxDocumentType;
        private System.Windows.Forms.TabPage tabPagePayment;
        private System.Windows.Forms.SplitContainer splitContainerPayment;
        private System.Windows.Forms.ListBox listBoxPayment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPayment;
        private System.Windows.Forms.Label labelPaymentIdentifier;
        private System.Windows.Forms.TextBox textBoxPaymentIdentifier;
        private System.Windows.Forms.Label labelPaymentAmount;
        private System.Windows.Forms.TextBox textBoxPaymentAmount;
        private System.Windows.Forms.Label labelPaymentCurrency;
        private System.Windows.Forms.Label labelPaymentForeignAmount;
        private System.Windows.Forms.TextBox textBoxPaymentForeignAmount;
        private System.Windows.Forms.TextBox textBoxPaymentForeignAmountCurrency;
        private System.Windows.Forms.Label labelPaymentPayer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryPaymentPayer;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryPaymentRecipient;
        private System.Windows.Forms.Label labelPaymentRecipient;
        private System.Windows.Forms.Label labelPaymentDate;
        private System.Windows.Forms.Label labelPaymentDateSupplement;
        private System.Windows.Forms.TextBox textBoxPaymentDateSupplement;
        private System.Windows.Forms.Label labelPaymentNotes;
        private System.Windows.Forms.TextBox textBoxPaymentNotes;
        private System.Windows.Forms.Label labelPaymentURI;
        private System.Windows.Forms.TextBox textBoxPaymentURI;
        private System.Windows.Forms.TextBox textBoxPaymentDate;
        private System.Windows.Forms.TabPage tabPageAgents;
        private System.Windows.Forms.SplitContainer splitContainerAgents;
        private System.Windows.Forms.ListBox listBoxAgents;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAgents;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryAgent;
        private System.Windows.Forms.Label labelAgentRole;
        private System.Windows.Forms.Label labelAgentNotes;
        private System.Windows.Forms.TextBox textBoxAgentNotes;
        private System.Windows.Forms.ComboBox comboBoxAgentRole;
        private System.Windows.Forms.TabPage tabPageIdentifier;
        private System.Windows.Forms.SplitContainer splitContainerIdentifier;
        private System.Windows.Forms.ListBox listBoxIdentifier;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIdentifier;
        private System.Windows.Forms.Label labelIdentifierType;
        private System.Windows.Forms.Label labelIdentifier;
        private System.Windows.Forms.TextBox textBoxIdentifierType;
        private System.Windows.Forms.TextBox textBoxIdentifier;
        private System.Windows.Forms.Button buttonIdenitifierURL;
        private System.Windows.Forms.Label labelIdenitifierURL;
        private System.Windows.Forms.TextBox textBoxIdenitifierURL;
        private System.Windows.Forms.Label labelIdenitifierNotes;
        private System.Windows.Forms.TextBox textBoxIdenitifierNotes;
        private Datasets.DataSetTransaction dataSetTransaction;
        private System.Windows.Forms.BindingSource transactionBindingSource;
        private Datasets.DataSetTransactionTableAdapters.TransactionDocumentTableAdapter transactionDocumentTableAdapter;
        private Datasets.DataSetTransactionTableAdapters.TransactionTableAdapter transactionTableAdapter;
        private System.Windows.Forms.BindingSource transactionDocumentBindingSource;
    }
}
