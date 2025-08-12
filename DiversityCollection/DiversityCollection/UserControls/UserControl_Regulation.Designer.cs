namespace DiversityCollection.UserControls
{
    partial class UserControl_Regulation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Regulation));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerTransactionDocuments = new System.Windows.Forms.SplitContainer();
            this.panelHistoryWebbrowser = new System.Windows.Forms.Panel();
            this.splitContainerDocumentBrowser = new System.Windows.Forms.SplitContainer();
            this.webBrowserTransactionDocuments = new System.Windows.Forms.WebBrowser();
            this.labelMessage = new System.Windows.Forms.Label();
            this.panelHistoryImage = new System.Windows.Forms.Panel();
            this.pictureBoxTransactionDocuments = new System.Windows.Forms.PictureBox();
            this.toolStripDocumentImage = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDocumentZoomAdapt = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDocumentZoom100Percent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDocument = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDocumentRemove = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorTransaction = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.labelBeginDate = new System.Windows.Forms.Label();
            this.labelTransactionTitle = new System.Windows.Forms.Label();
            this.labelAgreedEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerAgreedEndDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerBeginDate = new System.Windows.Forms.DateTimePicker();
            this.labelResponsibleName = new System.Windows.Forms.Label();
            this.labelInternalNotes = new System.Windows.Forms.Label();
            this.textBoxResponsibleName = new System.Windows.Forms.TextBox();
            this.textBoxInternalNotes = new System.Windows.Forms.TextBox();
            this.buttonAdministratingCollectionID = new System.Windows.Forms.Button();
            this.labelAdministratingCollectionID = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTransactionDocuments)).BeginInit();
            this.splitContainerTransactionDocuments.Panel1.SuspendLayout();
            this.splitContainerTransactionDocuments.Panel2.SuspendLayout();
            this.splitContainerTransactionDocuments.SuspendLayout();
            this.panelHistoryWebbrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDocumentBrowser)).BeginInit();
            this.splitContainerDocumentBrowser.Panel1.SuspendLayout();
            this.splitContainerDocumentBrowser.Panel2.SuspendLayout();
            this.splitContainerDocumentBrowser.SuspendLayout();
            this.panelHistoryImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTransactionDocuments)).BeginInit();
            this.toolStripDocumentImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorTransaction)).BeginInit();
            this.bindingNavigatorTransaction.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerTransactionDocuments, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.bindingNavigatorTransaction, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelBeginDate, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelTransactionTitle, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelAgreedEndDate, 2, 2);
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerAgreedEndDate, 3, 2);
            this.tableLayoutPanelMain.Controls.Add(this.dateTimePickerBeginDate, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelResponsibleName, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.labelInternalNotes, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxResponsibleName, 1, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxInternalNotes, 1, 4);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAdministratingCollectionID, 2, 5);
            this.tableLayoutPanelMain.Controls.Add(this.labelAdministratingCollectionID, 3, 5);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(355, 345);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // splitContainerTransactionDocuments
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.splitContainerTransactionDocuments, 4);
            this.splitContainerTransactionDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTransactionDocuments.Location = new System.Drawing.Point(3, 25);
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
            this.splitContainerTransactionDocuments.Size = new System.Drawing.Size(349, 219);
            this.splitContainerTransactionDocuments.SplitterDistance = 170;
            this.splitContainerTransactionDocuments.TabIndex = 1;
            // 
            // panelHistoryWebbrowser
            // 
            this.panelHistoryWebbrowser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelHistoryWebbrowser.Controls.Add(this.splitContainerDocumentBrowser);
            this.panelHistoryWebbrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistoryWebbrowser.Location = new System.Drawing.Point(0, 0);
            this.panelHistoryWebbrowser.Name = "panelHistoryWebbrowser";
            this.panelHistoryWebbrowser.Size = new System.Drawing.Size(170, 219);
            this.panelHistoryWebbrowser.TabIndex = 1;
            // 
            // splitContainerDocumentBrowser
            // 
            this.splitContainerDocumentBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDocumentBrowser.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDocumentBrowser.Name = "splitContainerDocumentBrowser";
            this.splitContainerDocumentBrowser.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerDocumentBrowser.Panel1
            // 
            this.splitContainerDocumentBrowser.Panel1.Controls.Add(this.webBrowserTransactionDocuments);
            // 
            // splitContainerDocumentBrowser.Panel2
            // 
            this.splitContainerDocumentBrowser.Panel2.Controls.Add(this.labelMessage);
            this.splitContainerDocumentBrowser.Panel2Collapsed = true;
            this.splitContainerDocumentBrowser.Size = new System.Drawing.Size(166, 215);
            this.splitContainerDocumentBrowser.SplitterDistance = 111;
            this.splitContainerDocumentBrowser.TabIndex = 1;
            // 
            // webBrowserTransactionDocuments
            // 
            this.webBrowserTransactionDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserTransactionDocuments.Location = new System.Drawing.Point(0, 0);
            this.webBrowserTransactionDocuments.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserTransactionDocuments.Name = "webBrowserTransactionDocuments";
            this.webBrowserTransactionDocuments.Size = new System.Drawing.Size(166, 215);
            this.webBrowserTransactionDocuments.TabIndex = 0;
            // 
            // labelMessage
            // 
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessage.ForeColor = System.Drawing.Color.Red;
            this.labelMessage.Location = new System.Drawing.Point(0, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(150, 46);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "No Documents available";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelHistoryImage
            // 
            this.panelHistoryImage.AutoScroll = true;
            this.panelHistoryImage.Controls.Add(this.pictureBoxTransactionDocuments);
            this.panelHistoryImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistoryImage.Location = new System.Drawing.Point(0, 0);
            this.panelHistoryImage.Name = "panelHistoryImage";
            this.panelHistoryImage.Size = new System.Drawing.Size(175, 219);
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
            this.toolStripButtonDocumentZoom100Percent,
            this.toolStripSeparatorDocument,
            this.toolStripButtonDocumentRemove});
            this.toolStripDocumentImage.Location = new System.Drawing.Point(0, 141);
            this.toolStripDocumentImage.Name = "toolStripDocumentImage";
            this.toolStripDocumentImage.Size = new System.Drawing.Size(84, 25);
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
            // toolStripSeparatorDocument
            // 
            this.toolStripSeparatorDocument.Name = "toolStripSeparatorDocument";
            this.toolStripSeparatorDocument.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonDocumentRemove
            // 
            this.toolStripButtonDocumentRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDocumentRemove.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDocumentRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDocumentRemove.Name = "toolStripButtonDocumentRemove";
            this.toolStripButtonDocumentRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDocumentRemove.Text = "toolStripButton1";
            // 
            // bindingNavigatorTransaction
            // 
            this.bindingNavigatorTransaction.AddNewItem = this.bindingNavigatorAddNewItem;
            this.tableLayoutPanelMain.SetColumnSpan(this.bindingNavigatorTransaction, 2);
            this.bindingNavigatorTransaction.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigatorTransaction.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindingNavigatorTransaction.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bindingNavigatorTransaction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigatorTransaction.Location = new System.Drawing.Point(0, 316);
            this.bindingNavigatorTransaction.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigatorTransaction.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigatorTransaction.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigatorTransaction.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigatorTransaction.Name = "bindingNavigatorTransaction";
            this.bindingNavigatorTransaction.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigatorTransaction.Size = new System.Drawing.Size(191, 25);
            this.bindingNavigatorTransaction.TabIndex = 2;
            this.bindingNavigatorTransaction.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorAddNewItem.Text = "Neu hinzufügen";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(44, 22);
            this.bindingNavigatorCountItem.Text = "von {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Die Gesamtanzahl der Elemente.";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorDeleteItem.Text = "Löschen";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Erste verschieben";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Vorherige verschieben";
            this.bindingNavigatorMovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Aktuelle Position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorMoveNextItem.Text = "Nächste verschieben";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 20);
            this.bindingNavigatorMoveLastItem.Text = "Letzte verschieben";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // labelBeginDate
            // 
            this.labelBeginDate.AutoSize = true;
            this.labelBeginDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBeginDate.Location = new System.Drawing.Point(3, 247);
            this.labelBeginDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelBeginDate.Name = "labelBeginDate";
            this.labelBeginDate.Size = new System.Drawing.Size(56, 23);
            this.labelBeginDate.TabIndex = 4;
            this.labelBeginDate.Text = "Valid from:";
            this.labelBeginDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTransactionTitle
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.labelTransactionTitle, 4);
            this.labelTransactionTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTransactionTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTransactionTitle.Image = global::DiversityCollection.Resource.Paragraph;
            this.labelTransactionTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTransactionTitle.Location = new System.Drawing.Point(3, 3);
            this.labelTransactionTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelTransactionTitle.Name = "labelTransactionTitle";
            this.labelTransactionTitle.Size = new System.Drawing.Size(349, 16);
            this.labelTransactionTitle.TabIndex = 5;
            this.labelTransactionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAgreedEndDate
            // 
            this.labelAgreedEndDate.AutoSize = true;
            this.labelAgreedEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAgreedEndDate.Location = new System.Drawing.Point(191, 247);
            this.labelAgreedEndDate.Margin = new System.Windows.Forms.Padding(0);
            this.labelAgreedEndDate.Name = "labelAgreedEndDate";
            this.labelAgreedEndDate.Size = new System.Drawing.Size(31, 23);
            this.labelAgreedEndDate.TabIndex = 6;
            this.labelAgreedEndDate.Text = "until:";
            this.labelAgreedEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerAgreedEndDate
            // 
            this.dateTimePickerAgreedEndDate.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerAgreedEndDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerAgreedEndDate.Enabled = false;
            this.dateTimePickerAgreedEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerAgreedEndDate.Location = new System.Drawing.Point(222, 247);
            this.dateTimePickerAgreedEndDate.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.dateTimePickerAgreedEndDate.Name = "dateTimePickerAgreedEndDate";
            this.dateTimePickerAgreedEndDate.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerAgreedEndDate.TabIndex = 7;
            // 
            // dateTimePickerBeginDate
            // 
            this.dateTimePickerBeginDate.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerBeginDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerBeginDate.Enabled = false;
            this.dateTimePickerBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerBeginDate.Location = new System.Drawing.Point(59, 247);
            this.dateTimePickerBeginDate.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.dateTimePickerBeginDate.Name = "dateTimePickerBeginDate";
            this.dateTimePickerBeginDate.Size = new System.Drawing.Size(129, 20);
            this.dateTimePickerBeginDate.TabIndex = 3;
            // 
            // labelResponsibleName
            // 
            this.labelResponsibleName.AutoSize = true;
            this.labelResponsibleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResponsibleName.Location = new System.Drawing.Point(3, 270);
            this.labelResponsibleName.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelResponsibleName.Name = "labelResponsibleName";
            this.labelResponsibleName.Size = new System.Drawing.Size(56, 23);
            this.labelResponsibleName.TabIndex = 8;
            this.labelResponsibleName.Text = "Respons.:";
            this.labelResponsibleName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelInternalNotes
            // 
            this.labelInternalNotes.AutoSize = true;
            this.labelInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInternalNotes.Location = new System.Drawing.Point(3, 293);
            this.labelInternalNotes.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelInternalNotes.Name = "labelInternalNotes";
            this.labelInternalNotes.Size = new System.Drawing.Size(56, 23);
            this.labelInternalNotes.TabIndex = 10;
            this.labelInternalNotes.Text = "Notes:";
            this.labelInternalNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxResponsibleName
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxResponsibleName, 3);
            this.textBoxResponsibleName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResponsibleName.Location = new System.Drawing.Point(59, 270);
            this.textBoxResponsibleName.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxResponsibleName.Name = "textBoxResponsibleName";
            this.textBoxResponsibleName.ReadOnly = true;
            this.textBoxResponsibleName.Size = new System.Drawing.Size(293, 20);
            this.textBoxResponsibleName.TabIndex = 9;
            // 
            // textBoxInternalNotes
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxInternalNotes, 3);
            this.textBoxInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInternalNotes.Location = new System.Drawing.Point(59, 293);
            this.textBoxInternalNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxInternalNotes.Name = "textBoxInternalNotes";
            this.textBoxInternalNotes.ReadOnly = true;
            this.textBoxInternalNotes.Size = new System.Drawing.Size(293, 20);
            this.textBoxInternalNotes.TabIndex = 11;
            // 
            // buttonAdministratingCollectionID
            // 
            this.buttonAdministratingCollectionID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAdministratingCollectionID.FlatAppearance.BorderSize = 0;
            this.buttonAdministratingCollectionID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdministratingCollectionID.Image = global::DiversityCollection.Resource.CollectionManager;
            this.buttonAdministratingCollectionID.Location = new System.Drawing.Point(194, 319);
            this.buttonAdministratingCollectionID.Name = "buttonAdministratingCollectionID";
            this.buttonAdministratingCollectionID.Size = new System.Drawing.Size(25, 23);
            this.buttonAdministratingCollectionID.TabIndex = 12;
            this.toolTip.SetToolTip(this.buttonAdministratingCollectionID, "Collection managers");
            this.buttonAdministratingCollectionID.UseVisualStyleBackColor = true;
            this.buttonAdministratingCollectionID.Click += new System.EventHandler(this.buttonAdministratingCollectionID_Click);
            // 
            // labelAdministratingCollectionID
            // 
            this.labelAdministratingCollectionID.AutoSize = true;
            this.labelAdministratingCollectionID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAdministratingCollectionID.Location = new System.Drawing.Point(225, 316);
            this.labelAdministratingCollectionID.Name = "labelAdministratingCollectionID";
            this.labelAdministratingCollectionID.Size = new System.Drawing.Size(127, 29);
            this.labelAdministratingCollectionID.TabIndex = 13;
            this.labelAdministratingCollectionID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.labelAdministratingCollectionID, "Administrating collection");
            // 
            // UserControl_Regulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControl_Regulation";
            this.Size = new System.Drawing.Size(355, 345);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.splitContainerTransactionDocuments.Panel1.ResumeLayout(false);
            this.splitContainerTransactionDocuments.Panel2.ResumeLayout(false);
            this.splitContainerTransactionDocuments.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTransactionDocuments)).EndInit();
            this.splitContainerTransactionDocuments.ResumeLayout(false);
            this.panelHistoryWebbrowser.ResumeLayout(false);
            this.splitContainerDocumentBrowser.Panel1.ResumeLayout(false);
            this.splitContainerDocumentBrowser.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDocumentBrowser)).EndInit();
            this.splitContainerDocumentBrowser.ResumeLayout(false);
            this.panelHistoryImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTransactionDocuments)).EndInit();
            this.toolStripDocumentImage.ResumeLayout(false);
            this.toolStripDocumentImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorTransaction)).EndInit();
            this.bindingNavigatorTransaction.ResumeLayout(false);
            this.bindingNavigatorTransaction.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.SplitContainer splitContainerTransactionDocuments;
        private System.Windows.Forms.Panel panelHistoryWebbrowser;
        private System.Windows.Forms.SplitContainer splitContainerDocumentBrowser;
        private System.Windows.Forms.WebBrowser webBrowserTransactionDocuments;
        private System.Windows.Forms.Panel panelHistoryImage;
        private System.Windows.Forms.PictureBox pictureBoxTransactionDocuments;
        private System.Windows.Forms.ToolStrip toolStripDocumentImage;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentZoomAdapt;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentZoom100Percent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDocument;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentRemove;
        private System.Windows.Forms.BindingNavigator bindingNavigatorTransaction;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.DateTimePicker dateTimePickerBeginDate;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label labelBeginDate;
        private System.Windows.Forms.Label labelTransactionTitle;
        private System.Windows.Forms.Label labelAgreedEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerAgreedEndDate;
        private System.Windows.Forms.Label labelResponsibleName;
        private System.Windows.Forms.Label labelInternalNotes;
        private System.Windows.Forms.TextBox textBoxResponsibleName;
        private System.Windows.Forms.TextBox textBoxInternalNotes;
        private System.Windows.Forms.Button buttonAdministratingCollectionID;
        private System.Windows.Forms.Label labelAdministratingCollectionID;
    }
}
