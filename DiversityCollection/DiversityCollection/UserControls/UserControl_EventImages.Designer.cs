namespace DiversityCollection.UserControls
{
    partial class UserControl_EventImages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_EventImages));
            this.imageListForm = new System.Windows.Forms.ImageList(this.components);
            this.imageListEventImages = new System.Windows.Forms.ImageList(this.components);
            this.groupBoxEventImages = new System.Windows.Forms.GroupBox();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.splitContainerImagesEventDetails = new System.Windows.Forms.SplitContainer();
            this.userControlImageEventImage = new DiversityWorkbench.UserControls.UserControlImage();
            this.tableLayoutPanelEventImages = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripEventImages = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSwitchBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEventImageNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEventImageDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenEventImageModule = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEventImageDescription = new System.Windows.Forms.ToolStripButton();
            this.listBoxEventImages = new System.Windows.Forms.ListBox();
            this.comboBoxEventImageDataWithholdingReason = new System.Windows.Forms.ComboBox();
            this.pictureBoxWithholdEventImage = new System.Windows.Forms.PictureBox();
            this.tabControlEventImageDetails = new System.Windows.Forms.TabControl();
            this.tabPageEventImageDetails = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelEventImageDetails = new System.Windows.Forms.TableLayoutPanel();
            this.labelEventImageTitle = new System.Windows.Forms.Label();
            this.textBoxEventImageTitle = new System.Windows.Forms.TextBox();
            this.textBoxEventImageNotes = new System.Windows.Forms.TextBox();
            this.textBoxEventImageInternalNotes = new System.Windows.Forms.TextBox();
            this.labelEventImageInternalNotes = new System.Windows.Forms.Label();
            this.labelEventImageNotes = new System.Windows.Forms.Label();
            this.comboBoxEventImageType = new System.Windows.Forms.ComboBox();
            this.labelEventImageType = new System.Windows.Forms.Label();
            this.labelEventImageCreator = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryEventImageCreator = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.tabPageEventImageLicense = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelEventImageLicense = new System.Windows.Forms.TableLayoutPanel();
            this.labelEventImageIPR = new System.Windows.Forms.Label();
            this.textBoxEventImageIPR = new System.Windows.Forms.TextBox();
            this.labelEventImageCopyrightStatement = new System.Windows.Forms.Label();
            this.textBoxEventImageCopyrightStatement = new System.Windows.Forms.TextBox();
            this.groupBoxEventImageLicenseDetails = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelEventImageLicenseDetails = new System.Windows.Forms.TableLayoutPanel();
            this.labelEventImageLicenseType = new System.Windows.Forms.Label();
            this.textBoxEventImageLicenseType = new System.Windows.Forms.TextBox();
            this.labelEventImageLicenseHolder = new System.Windows.Forms.Label();
            this.textBoxEventImageLicenseYear = new System.Windows.Forms.TextBox();
            this.labelEventImageLicenseYear = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryEventImageLicenseHolder = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.tabPageEventImageExif = new System.Windows.Forms.TabPage();
            this.userControlXMLTreeEventImageExif = new DiversityWorkbench.UserControls.UserControlXMLTree();
            this.groupBoxEventImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImagesEventDetails)).BeginInit();
            this.splitContainerImagesEventDetails.Panel1.SuspendLayout();
            this.splitContainerImagesEventDetails.Panel2.SuspendLayout();
            this.splitContainerImagesEventDetails.SuspendLayout();
            this.tableLayoutPanelEventImages.SuspendLayout();
            this.toolStripEventImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWithholdEventImage)).BeginInit();
            this.tabControlEventImageDetails.SuspendLayout();
            this.tabPageEventImageDetails.SuspendLayout();
            this.tableLayoutPanelEventImageDetails.SuspendLayout();
            this.tabPageEventImageLicense.SuspendLayout();
            this.tableLayoutPanelEventImageLicense.SuspendLayout();
            this.groupBoxEventImageLicenseDetails.SuspendLayout();
            this.tableLayoutPanelEventImageLicenseDetails.SuspendLayout();
            this.tabPageEventImageExif.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // imageListForm
            // 
            this.imageListForm.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListForm.ImageStream")));
            this.imageListForm.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListForm.Images.SetKeyName(0, "");
            // 
            // imageListEventImages
            // 
            this.imageListEventImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListEventImages.ImageSize = new System.Drawing.Size(50, 50);
            this.imageListEventImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // groupBoxEventImages
            // 
            this.groupBoxEventImages.AccessibleName = "CollectionEventImage";
            this.groupBoxEventImages.Controls.Add(this.pictureBoxIcon);
            this.groupBoxEventImages.Controls.Add(this.splitContainerImagesEventDetails);
            this.groupBoxEventImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEventImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEventImages.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEventImages.Name = "groupBoxEventImages";
            this.groupBoxEventImages.Size = new System.Drawing.Size(922, 451);
            this.groupBoxEventImages.TabIndex = 5;
            this.groupBoxEventImages.TabStop = false;
            this.groupBoxEventImages.Text = "Resources of the collection event";
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxIcon.Image = global::DiversityCollection.Resource.Event;
            this.pictureBoxIcon.Location = new System.Drawing.Point(905, 0);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxIcon.TabIndex = 5;
            this.pictureBoxIcon.TabStop = false;
            // 
            // splitContainerImagesEventDetails
            // 
            this.splitContainerImagesEventDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerImagesEventDetails.Location = new System.Drawing.Point(3, 16);
            this.splitContainerImagesEventDetails.Name = "splitContainerImagesEventDetails";
            // 
            // splitContainerImagesEventDetails.Panel1
            // 
            this.splitContainerImagesEventDetails.Panel1.Controls.Add(this.userControlImageEventImage);
            // 
            // splitContainerImagesEventDetails.Panel2
            // 
            this.splitContainerImagesEventDetails.Panel2.Controls.Add(this.tableLayoutPanelEventImages);
            this.splitContainerImagesEventDetails.Size = new System.Drawing.Size(916, 432);
            this.splitContainerImagesEventDetails.SplitterDistance = 472;
            this.splitContainerImagesEventDetails.TabIndex = 4;
            // 
            // userControlImageEventImage
            // 
            this.userControlImageEventImage.AutorotationEnabled = false;
            this.userControlImageEventImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlImageEventImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlImageEventImage.ImagePath = "";
            this.userControlImageEventImage.Location = new System.Drawing.Point(0, 0);
            this.userControlImageEventImage.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.userControlImageEventImage.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Unknown;
            this.userControlImageEventImage.Name = "userControlImageEventImage";
            this.userControlImageEventImage.Size = new System.Drawing.Size(472, 432);
            this.userControlImageEventImage.TabIndex = 0;
            // 
            // tableLayoutPanelEventImages
            // 
            this.tableLayoutPanelEventImages.ColumnCount = 3;
            this.tableLayoutPanelEventImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanelEventImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanelEventImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventImages.Controls.Add(this.toolStripEventImages, 0, 1);
            this.tableLayoutPanelEventImages.Controls.Add(this.listBoxEventImages, 0, 0);
            this.tableLayoutPanelEventImages.Controls.Add(this.comboBoxEventImageDataWithholdingReason, 2, 1);
            this.tableLayoutPanelEventImages.Controls.Add(this.pictureBoxWithholdEventImage, 1, 1);
            this.tableLayoutPanelEventImages.Controls.Add(this.tabControlEventImageDetails, 1, 0);
            this.tableLayoutPanelEventImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEventImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelEventImages.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEventImages.Name = "tableLayoutPanelEventImages";
            this.tableLayoutPanelEventImages.RowCount = 2;
            this.tableLayoutPanelEventImages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImages.Size = new System.Drawing.Size(440, 432);
            this.tableLayoutPanelEventImages.TabIndex = 3;
            // 
            // toolStripEventImages
            // 
            this.toolStripEventImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripEventImages.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEventImages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSwitchBrowser,
            this.toolStripButtonEventImageNew,
            this.toolStripButtonEventImageDelete,
            this.toolStripButtonOpenEventImageModule,
            this.toolStripButtonEventImageDescription});
            this.toolStripEventImages.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripEventImages.Location = new System.Drawing.Point(0, 407);
            this.toolStripEventImages.Name = "toolStripEventImages";
            this.toolStripEventImages.Size = new System.Drawing.Size(88, 25);
            this.toolStripEventImages.TabIndex = 3;
            this.toolStripEventImages.Text = "toolStrip1";
            // 
            // toolStripButtonSwitchBrowser
            // 
            this.toolStripButtonSwitchBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSwitchBrowser.Image = global::DiversityCollection.Resource.ExternerBrowserSmall;
            this.toolStripButtonSwitchBrowser.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSwitchBrowser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSwitchBrowser.Name = "toolStripButtonSwitchBrowser";
            this.toolStripButtonSwitchBrowser.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSwitchBrowser.Text = "Change to browser";
            this.toolStripButtonSwitchBrowser.Click += new System.EventHandler(this.toolStripButtonSwitchBrowser_Click);
            // 
            // toolStripButtonEventImageNew
            // 
            this.toolStripButtonEventImageNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEventImageNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEventImageNew.Image")));
            this.toolStripButtonEventImageNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEventImageNew.Name = "toolStripButtonEventImageNew";
            this.toolStripButtonEventImageNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEventImageNew.Text = "Enter a new image for the event";
            this.toolStripButtonEventImageNew.Click += new System.EventHandler(this.toolStripButtonEventImageNew_Click);
            // 
            // toolStripButtonEventImageDelete
            // 
            this.toolStripButtonEventImageDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEventImageDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEventImageDelete.Image")));
            this.toolStripButtonEventImageDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEventImageDelete.Name = "toolStripButtonEventImageDelete";
            this.toolStripButtonEventImageDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEventImageDelete.Text = "delete the selected image";
            this.toolStripButtonEventImageDelete.Click += new System.EventHandler(this.toolStripButtonEventImageDelete_Click);
            // 
            // toolStripButtonOpenEventImageModule
            // 
            this.toolStripButtonOpenEventImageModule.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenEventImageModule.Enabled = false;
            this.toolStripButtonOpenEventImageModule.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenEventImageModule.Image")));
            this.toolStripButtonOpenEventImageModule.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenEventImageModule.Name = "toolStripButtonOpenEventImageModule";
            this.toolStripButtonOpenEventImageModule.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonOpenEventImageModule.Text = "Open DiversityResources";
            this.toolStripButtonOpenEventImageModule.Visible = false;
            // 
            // toolStripButtonEventImageDescription
            // 
            this.toolStripButtonEventImageDescription.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEventImageDescription.Image = global::DiversityCollection.Resource.Properties;
            this.toolStripButtonEventImageDescription.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEventImageDescription.Name = "toolStripButtonEventImageDescription";
            this.toolStripButtonEventImageDescription.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonEventImageDescription.Text = "Show the EXIF description of the selected image";
            // 
            // listBoxEventImages
            // 
            this.listBoxEventImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxEventImages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxEventImages.FormattingEnabled = true;
            this.listBoxEventImages.IntegralHeight = false;
            this.listBoxEventImages.ItemHeight = 50;
            this.listBoxEventImages.Location = new System.Drawing.Point(0, 0);
            this.listBoxEventImages.Margin = new System.Windows.Forms.Padding(0);
            this.listBoxEventImages.Name = "listBoxEventImages";
            this.listBoxEventImages.ScrollAlwaysVisible = true;
            this.listBoxEventImages.Size = new System.Drawing.Size(88, 407);
            this.listBoxEventImages.TabIndex = 2;
            this.listBoxEventImages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxEventImages_DrawItem);
            this.listBoxEventImages.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBoxEventImages_MeasureItem);
            this.listBoxEventImages.SelectedIndexChanged += new System.EventHandler(this.listBoxEventImages_SelectedIndexChanged);
            // 
            // comboBoxEventImageDataWithholdingReason
            // 
            this.comboBoxEventImageDataWithholdingReason.AccessibleName = "CollectionEventSeriesImage.DataWithholdingReason";
            this.comboBoxEventImageDataWithholdingReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxEventImageDataWithholdingReason.FormattingEnabled = true;
            this.comboBoxEventImageDataWithholdingReason.Location = new System.Drawing.Point(104, 407);
            this.comboBoxEventImageDataWithholdingReason.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxEventImageDataWithholdingReason.Name = "comboBoxEventImageDataWithholdingReason";
            this.comboBoxEventImageDataWithholdingReason.Size = new System.Drawing.Size(336, 21);
            this.comboBoxEventImageDataWithholdingReason.TabIndex = 9;
            this.comboBoxEventImageDataWithholdingReason.DropDown += new System.EventHandler(this.comboBoxEventImageDataWithholdingReason_DropDown);
            this.comboBoxEventImageDataWithholdingReason.TextChanged += new System.EventHandler(this.comboBoxEventImageDataWithholdingReason_TextChanged);
            // 
            // pictureBoxWithholdEventImage
            // 
            this.pictureBoxWithholdEventImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxWithholdEventImage.Image = global::DiversityCollection.Resource.Stop3;
            this.pictureBoxWithholdEventImage.Location = new System.Drawing.Point(88, 410);
            this.pictureBoxWithholdEventImage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBoxWithholdEventImage.Name = "pictureBoxWithholdEventImage";
            this.pictureBoxWithholdEventImage.Size = new System.Drawing.Size(16, 22);
            this.pictureBoxWithholdEventImage.TabIndex = 22;
            this.pictureBoxWithholdEventImage.TabStop = false;
            // 
            // tabControlEventImageDetails
            // 
            this.tableLayoutPanelEventImages.SetColumnSpan(this.tabControlEventImageDetails, 2);
            this.tabControlEventImageDetails.Controls.Add(this.tabPageEventImageDetails);
            this.tabControlEventImageDetails.Controls.Add(this.tabPageEventImageLicense);
            this.tabControlEventImageDetails.Controls.Add(this.tabPageEventImageExif);
            this.tabControlEventImageDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEventImageDetails.Location = new System.Drawing.Point(88, 0);
            this.tabControlEventImageDetails.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlEventImageDetails.Name = "tabControlEventImageDetails";
            this.tabControlEventImageDetails.SelectedIndex = 0;
            this.tabControlEventImageDetails.Size = new System.Drawing.Size(352, 407);
            this.tabControlEventImageDetails.TabIndex = 27;
            // 
            // tabPageEventImageDetails
            // 
            this.tabPageEventImageDetails.Controls.Add(this.tableLayoutPanelEventImageDetails);
            this.tabPageEventImageDetails.Location = new System.Drawing.Point(4, 22);
            this.tabPageEventImageDetails.Name = "tabPageEventImageDetails";
            this.tabPageEventImageDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEventImageDetails.Size = new System.Drawing.Size(344, 381);
            this.tabPageEventImageDetails.TabIndex = 0;
            this.tabPageEventImageDetails.Text = "Details";
            this.tabPageEventImageDetails.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEventImageDetails
            // 
            this.tableLayoutPanelEventImageDetails.ColumnCount = 2;
            this.tableLayoutPanelEventImageDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventImageDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.labelEventImageTitle, 0, 0);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.textBoxEventImageTitle, 0, 1);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.textBoxEventImageNotes, 1, 5);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.textBoxEventImageInternalNotes, 1, 6);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.labelEventImageInternalNotes, 0, 6);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.labelEventImageNotes, 0, 5);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.comboBoxEventImageType, 1, 4);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.labelEventImageType, 0, 4);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.labelEventImageCreator, 0, 2);
            this.tableLayoutPanelEventImageDetails.Controls.Add(this.userControlModuleRelatedEntryEventImageCreator, 0, 3);
            this.tableLayoutPanelEventImageDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEventImageDetails.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelEventImageDetails.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelEventImageDetails.Name = "tableLayoutPanelEventImageDetails";
            this.tableLayoutPanelEventImageDetails.RowCount = 7;
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEventImageDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEventImageDetails.Size = new System.Drawing.Size(338, 375);
            this.tableLayoutPanelEventImageDetails.TabIndex = 0;
            // 
            // labelEventImageTitle
            // 
            this.labelEventImageTitle.AccessibleName = "CollectionEventSeriesImage.Title";
            this.labelEventImageTitle.AutoSize = true;
            this.tableLayoutPanelEventImageDetails.SetColumnSpan(this.labelEventImageTitle, 2);
            this.labelEventImageTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventImageTitle.Location = new System.Drawing.Point(3, 0);
            this.labelEventImageTitle.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventImageTitle.Name = "labelEventImageTitle";
            this.labelEventImageTitle.Size = new System.Drawing.Size(335, 13);
            this.labelEventImageTitle.TabIndex = 20;
            this.labelEventImageTitle.Text = "Title:";
            this.labelEventImageTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxEventImageTitle
            // 
            this.textBoxEventImageTitle.AccessibleName = "CollectionEventSeriesImage.Title";
            this.tableLayoutPanelEventImageDetails.SetColumnSpan(this.textBoxEventImageTitle, 2);
            this.textBoxEventImageTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventImageTitle.Location = new System.Drawing.Point(0, 13);
            this.textBoxEventImageTitle.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEventImageTitle.Name = "textBoxEventImageTitle";
            this.textBoxEventImageTitle.Size = new System.Drawing.Size(338, 20);
            this.textBoxEventImageTitle.TabIndex = 21;
            // 
            // textBoxEventImageNotes
            // 
            this.textBoxEventImageNotes.AccessibleName = "CollectionEventSeriesImage.Notes";
            this.textBoxEventImageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventImageNotes.Location = new System.Drawing.Point(42, 89);
            this.textBoxEventImageNotes.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEventImageNotes.Multiline = true;
            this.textBoxEventImageNotes.Name = "textBoxEventImageNotes";
            this.textBoxEventImageNotes.Size = new System.Drawing.Size(296, 143);
            this.textBoxEventImageNotes.TabIndex = 7;
            // 
            // textBoxEventImageInternalNotes
            // 
            this.textBoxEventImageInternalNotes.AccessibleName = "CollectionEventSeriesImage.InternalNotes";
            this.textBoxEventImageInternalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventImageInternalNotes.Location = new System.Drawing.Point(42, 232);
            this.textBoxEventImageInternalNotes.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEventImageInternalNotes.Multiline = true;
            this.textBoxEventImageInternalNotes.Name = "textBoxEventImageInternalNotes";
            this.textBoxEventImageInternalNotes.Size = new System.Drawing.Size(296, 143);
            this.textBoxEventImageInternalNotes.TabIndex = 11;
            // 
            // labelEventImageInternalNotes
            // 
            this.labelEventImageInternalNotes.AccessibleName = "CollectionEventSeriesImage.InternalNotes";
            this.labelEventImageInternalNotes.AutoSize = true;
            this.labelEventImageInternalNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEventImageInternalNotes.Location = new System.Drawing.Point(3, 232);
            this.labelEventImageInternalNotes.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventImageInternalNotes.Name = "labelEventImageInternalNotes";
            this.labelEventImageInternalNotes.Size = new System.Drawing.Size(39, 13);
            this.labelEventImageInternalNotes.TabIndex = 10;
            this.labelEventImageInternalNotes.Text = "Int. N.:";
            this.labelEventImageInternalNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEventImageNotes
            // 
            this.labelEventImageNotes.AccessibleName = "CollectionEventSeriesImage.Notes";
            this.labelEventImageNotes.AutoSize = true;
            this.labelEventImageNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEventImageNotes.Location = new System.Drawing.Point(3, 92);
            this.labelEventImageNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelEventImageNotes.Name = "labelEventImageNotes";
            this.labelEventImageNotes.Size = new System.Drawing.Size(39, 13);
            this.labelEventImageNotes.TabIndex = 6;
            this.labelEventImageNotes.Text = "Notes:";
            this.labelEventImageNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxEventImageType
            // 
            this.comboBoxEventImageType.AccessibleName = "CollectionEventSeriesImage.ImageType";
            this.comboBoxEventImageType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxEventImageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEventImageType.FormattingEnabled = true;
            this.comboBoxEventImageType.Location = new System.Drawing.Point(42, 68);
            this.comboBoxEventImageType.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxEventImageType.Name = "comboBoxEventImageType";
            this.comboBoxEventImageType.Size = new System.Drawing.Size(296, 21);
            this.comboBoxEventImageType.TabIndex = 5;
            // 
            // labelEventImageType
            // 
            this.labelEventImageType.AccessibleName = "CollectionEventSeriesImage.ImageType";
            this.labelEventImageType.AutoSize = true;
            this.labelEventImageType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventImageType.Location = new System.Drawing.Point(3, 68);
            this.labelEventImageType.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventImageType.Name = "labelEventImageType";
            this.labelEventImageType.Size = new System.Drawing.Size(39, 21);
            this.labelEventImageType.TabIndex = 4;
            this.labelEventImageType.Text = "Type:";
            this.labelEventImageType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEventImageCreator
            // 
            this.labelEventImageCreator.AccessibleName = "CollectionEventSeriesImage.CreatorAgent";
            this.labelEventImageCreator.AutoSize = true;
            this.tableLayoutPanelEventImageDetails.SetColumnSpan(this.labelEventImageCreator, 2);
            this.labelEventImageCreator.Location = new System.Drawing.Point(3, 33);
            this.labelEventImageCreator.Name = "labelEventImageCreator";
            this.labelEventImageCreator.Size = new System.Drawing.Size(44, 13);
            this.labelEventImageCreator.TabIndex = 19;
            this.labelEventImageCreator.Text = "Creator:";
            this.labelEventImageCreator.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // userControlModuleRelatedEntryEventImageCreator
            // 
            this.userControlModuleRelatedEntryEventImageCreator.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelEventImageDetails.SetColumnSpan(this.userControlModuleRelatedEntryEventImageCreator, 2);
            this.userControlModuleRelatedEntryEventImageCreator.DependsOnUri = "";
            this.userControlModuleRelatedEntryEventImageCreator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryEventImageCreator.Domain = "";
            this.userControlModuleRelatedEntryEventImageCreator.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryEventImageCreator.Location = new System.Drawing.Point(3, 46);
            this.userControlModuleRelatedEntryEventImageCreator.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.userControlModuleRelatedEntryEventImageCreator.Module = null;
            this.userControlModuleRelatedEntryEventImageCreator.Name = "userControlModuleRelatedEntryEventImageCreator";
            this.userControlModuleRelatedEntryEventImageCreator.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryEventImageCreator.ShowInfo = false;
            this.userControlModuleRelatedEntryEventImageCreator.Size = new System.Drawing.Size(332, 22);
            this.userControlModuleRelatedEntryEventImageCreator.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryEventImageCreator.TabIndex = 18;
            // 
            // tabPageEventImageLicense
            // 
            this.tabPageEventImageLicense.Controls.Add(this.tableLayoutPanelEventImageLicense);
            this.tabPageEventImageLicense.Location = new System.Drawing.Point(4, 22);
            this.tabPageEventImageLicense.Name = "tabPageEventImageLicense";
            this.tabPageEventImageLicense.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEventImageLicense.Size = new System.Drawing.Size(344, 381);
            this.tabPageEventImageLicense.TabIndex = 1;
            this.tabPageEventImageLicense.Text = "License";
            this.tabPageEventImageLicense.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEventImageLicense
            // 
            this.tableLayoutPanelEventImageLicense.ColumnCount = 1;
            this.tableLayoutPanelEventImageLicense.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventImageLicense.Controls.Add(this.labelEventImageIPR, 0, 0);
            this.tableLayoutPanelEventImageLicense.Controls.Add(this.textBoxEventImageIPR, 0, 1);
            this.tableLayoutPanelEventImageLicense.Controls.Add(this.labelEventImageCopyrightStatement, 0, 2);
            this.tableLayoutPanelEventImageLicense.Controls.Add(this.textBoxEventImageCopyrightStatement, 0, 3);
            this.tableLayoutPanelEventImageLicense.Controls.Add(this.groupBoxEventImageLicenseDetails, 0, 5);
            this.tableLayoutPanelEventImageLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEventImageLicense.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelEventImageLicense.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelEventImageLicense.Name = "tableLayoutPanelEventImageLicense";
            this.tableLayoutPanelEventImageLicense.RowCount = 6;
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEventImageLicense.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEventImageLicense.Size = new System.Drawing.Size(338, 375);
            this.tableLayoutPanelEventImageLicense.TabIndex = 0;
            // 
            // labelEventImageIPR
            // 
            this.labelEventImageIPR.AccessibleName = "CollectionEventSeriesImage.IPR";
            this.labelEventImageIPR.AutoSize = true;
            this.labelEventImageIPR.Location = new System.Drawing.Point(3, 0);
            this.labelEventImageIPR.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventImageIPR.Name = "labelEventImageIPR";
            this.labelEventImageIPR.Size = new System.Drawing.Size(28, 13);
            this.labelEventImageIPR.TabIndex = 16;
            this.labelEventImageIPR.Text = "IPR:";
            this.labelEventImageIPR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEventImageIPR
            // 
            this.textBoxEventImageIPR.AccessibleName = "CollectionEventSeriesImage.IPR";
            this.textBoxEventImageIPR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventImageIPR.Location = new System.Drawing.Point(0, 13);
            this.textBoxEventImageIPR.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEventImageIPR.Multiline = true;
            this.textBoxEventImageIPR.Name = "textBoxEventImageIPR";
            this.textBoxEventImageIPR.Size = new System.Drawing.Size(338, 20);
            this.textBoxEventImageIPR.TabIndex = 17;
            // 
            // labelEventImageCopyrightStatement
            // 
            this.labelEventImageCopyrightStatement.AccessibleName = "CollectionEventSeriesImage.CopyrightStatement";
            this.labelEventImageCopyrightStatement.AutoSize = true;
            this.labelEventImageCopyrightStatement.Location = new System.Drawing.Point(3, 33);
            this.labelEventImageCopyrightStatement.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventImageCopyrightStatement.Name = "labelEventImageCopyrightStatement";
            this.labelEventImageCopyrightStatement.Size = new System.Drawing.Size(54, 13);
            this.labelEventImageCopyrightStatement.TabIndex = 14;
            this.labelEventImageCopyrightStatement.Text = "Copyright:";
            this.labelEventImageCopyrightStatement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEventImageCopyrightStatement
            // 
            this.textBoxEventImageCopyrightStatement.AccessibleName = "CollectionEventSeriesImage.CopyrightStatement";
            this.textBoxEventImageCopyrightStatement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventImageCopyrightStatement.Location = new System.Drawing.Point(0, 46);
            this.textBoxEventImageCopyrightStatement.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEventImageCopyrightStatement.Multiline = true;
            this.textBoxEventImageCopyrightStatement.Name = "textBoxEventImageCopyrightStatement";
            this.textBoxEventImageCopyrightStatement.Size = new System.Drawing.Size(338, 20);
            this.textBoxEventImageCopyrightStatement.TabIndex = 15;
            // 
            // groupBoxEventImageLicenseDetails
            // 
            this.groupBoxEventImageLicenseDetails.Controls.Add(this.tableLayoutPanelEventImageLicenseDetails);
            this.groupBoxEventImageLicenseDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEventImageLicenseDetails.Location = new System.Drawing.Point(0, 66);
            this.groupBoxEventImageLicenseDetails.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxEventImageLicenseDetails.Name = "groupBoxEventImageLicenseDetails";
            this.groupBoxEventImageLicenseDetails.Size = new System.Drawing.Size(338, 309);
            this.groupBoxEventImageLicenseDetails.TabIndex = 27;
            this.groupBoxEventImageLicenseDetails.TabStop = false;
            this.groupBoxEventImageLicenseDetails.Text = "License";
            // 
            // tableLayoutPanelEventImageLicenseDetails
            // 
            this.tableLayoutPanelEventImageLicenseDetails.ColumnCount = 2;
            this.tableLayoutPanelEventImageLicenseDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventImageLicenseDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventImageLicenseDetails.Controls.Add(this.labelEventImageLicenseType, 0, 0);
            this.tableLayoutPanelEventImageLicenseDetails.Controls.Add(this.textBoxEventImageLicenseType, 1, 0);
            this.tableLayoutPanelEventImageLicenseDetails.Controls.Add(this.labelEventImageLicenseHolder, 0, 1);
            this.tableLayoutPanelEventImageLicenseDetails.Controls.Add(this.textBoxEventImageLicenseYear, 1, 3);
            this.tableLayoutPanelEventImageLicenseDetails.Controls.Add(this.labelEventImageLicenseYear, 0, 3);
            this.tableLayoutPanelEventImageLicenseDetails.Controls.Add(this.userControlModuleRelatedEntryEventImageLicenseHolder, 0, 2);
            this.tableLayoutPanelEventImageLicenseDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEventImageLicenseDetails.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelEventImageLicenseDetails.Name = "tableLayoutPanelEventImageLicenseDetails";
            this.tableLayoutPanelEventImageLicenseDetails.RowCount = 4;
            this.tableLayoutPanelEventImageLicenseDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageLicenseDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageLicenseDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventImageLicenseDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventImageLicenseDetails.Size = new System.Drawing.Size(332, 290);
            this.tableLayoutPanelEventImageLicenseDetails.TabIndex = 0;
            // 
            // labelEventImageLicenseType
            // 
            this.labelEventImageLicenseType.AccessibleName = "CollectionEventSeriesImage.LicenseType";
            this.labelEventImageLicenseType.AutoSize = true;
            this.labelEventImageLicenseType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventImageLicenseType.Location = new System.Drawing.Point(3, 0);
            this.labelEventImageLicenseType.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventImageLicenseType.Name = "labelEventImageLicenseType";
            this.labelEventImageLicenseType.Size = new System.Drawing.Size(34, 24);
            this.labelEventImageLicenseType.TabIndex = 12;
            this.labelEventImageLicenseType.Text = "Type:";
            this.labelEventImageLicenseType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEventImageLicenseType
            // 
            this.textBoxEventImageLicenseType.AccessibleName = "CollectionEventSeriesImage.LicenseType";
            this.textBoxEventImageLicenseType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventImageLicenseType.Location = new System.Drawing.Point(37, 0);
            this.textBoxEventImageLicenseType.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEventImageLicenseType.Multiline = true;
            this.textBoxEventImageLicenseType.Name = "textBoxEventImageLicenseType";
            this.textBoxEventImageLicenseType.Size = new System.Drawing.Size(295, 24);
            this.textBoxEventImageLicenseType.TabIndex = 13;
            // 
            // labelEventImageLicenseHolder
            // 
            this.labelEventImageLicenseHolder.AccessibleName = "CollectionEventSeriesImage.LicenseHolder";
            this.labelEventImageLicenseHolder.AutoSize = true;
            this.tableLayoutPanelEventImageLicenseDetails.SetColumnSpan(this.labelEventImageLicenseHolder, 2);
            this.labelEventImageLicenseHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventImageLicenseHolder.Location = new System.Drawing.Point(3, 24);
            this.labelEventImageLicenseHolder.Name = "labelEventImageLicenseHolder";
            this.labelEventImageLicenseHolder.Size = new System.Drawing.Size(326, 13);
            this.labelEventImageLicenseHolder.TabIndex = 23;
            this.labelEventImageLicenseHolder.Text = "Holder:";
            // 
            // textBoxEventImageLicenseYear
            // 
            this.textBoxEventImageLicenseYear.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxEventImageLicenseYear.Location = new System.Drawing.Point(37, 59);
            this.textBoxEventImageLicenseYear.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEventImageLicenseYear.Name = "textBoxEventImageLicenseYear";
            this.textBoxEventImageLicenseYear.Size = new System.Drawing.Size(295, 20);
            this.textBoxEventImageLicenseYear.TabIndex = 26;
            // 
            // labelEventImageLicenseYear
            // 
            this.labelEventImageLicenseYear.AccessibleName = "CollectionEventSeriesImage.LicenseYear";
            this.labelEventImageLicenseYear.AutoSize = true;
            this.labelEventImageLicenseYear.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelEventImageLicenseYear.Location = new System.Drawing.Point(0, 62);
            this.labelEventImageLicenseYear.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.labelEventImageLicenseYear.Name = "labelEventImageLicenseYear";
            this.labelEventImageLicenseYear.Size = new System.Drawing.Size(37, 13);
            this.labelEventImageLicenseYear.TabIndex = 25;
            this.labelEventImageLicenseYear.Text = "Year:";
            this.labelEventImageLicenseYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryEventImageLicenseHolder
            // 
            this.userControlModuleRelatedEntryEventImageLicenseHolder.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelEventImageLicenseDetails.SetColumnSpan(this.userControlModuleRelatedEntryEventImageLicenseHolder, 2);
            this.userControlModuleRelatedEntryEventImageLicenseHolder.DependsOnUri = "";
            this.userControlModuleRelatedEntryEventImageLicenseHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryEventImageLicenseHolder.Domain = "";
            this.userControlModuleRelatedEntryEventImageLicenseHolder.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryEventImageLicenseHolder.Location = new System.Drawing.Point(0, 37);
            this.userControlModuleRelatedEntryEventImageLicenseHolder.Margin = new System.Windows.Forms.Padding(0);
            this.userControlModuleRelatedEntryEventImageLicenseHolder.Module = null;
            this.userControlModuleRelatedEntryEventImageLicenseHolder.Name = "userControlModuleRelatedEntryEventImageLicenseHolder";
            this.userControlModuleRelatedEntryEventImageLicenseHolder.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryEventImageLicenseHolder.ShowInfo = false;
            this.userControlModuleRelatedEntryEventImageLicenseHolder.Size = new System.Drawing.Size(332, 22);
            this.userControlModuleRelatedEntryEventImageLicenseHolder.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryEventImageLicenseHolder.TabIndex = 24;
            // 
            // tabPageEventImageExif
            // 
            this.tabPageEventImageExif.Controls.Add(this.userControlXMLTreeEventImageExif);
            this.tabPageEventImageExif.Location = new System.Drawing.Point(4, 22);
            this.tabPageEventImageExif.Name = "tabPageEventImageExif";
            this.tabPageEventImageExif.Size = new System.Drawing.Size(344, 381);
            this.tabPageEventImageExif.TabIndex = 2;
            this.tabPageEventImageExif.Text = "EXIF";
            this.tabPageEventImageExif.UseVisualStyleBackColor = true;
            // 
            // userControlXMLTreeEventImageExif
            // 
            this.userControlXMLTreeEventImageExif.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlXMLTreeEventImageExif.Location = new System.Drawing.Point(0, 0);
            this.userControlXMLTreeEventImageExif.Name = "userControlXMLTreeEventImageExif";
            this.userControlXMLTreeEventImageExif.Size = new System.Drawing.Size(344, 381);
            this.userControlXMLTreeEventImageExif.TabIndex = 0;
            this.userControlXMLTreeEventImageExif.XML = "";
            // 
            // UserControl_EventImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxEventImages);
            this.Name = "UserControl_EventImages";
            this.Size = new System.Drawing.Size(922, 451);
            this.groupBoxEventImages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.splitContainerImagesEventDetails.Panel1.ResumeLayout(false);
            this.splitContainerImagesEventDetails.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerImagesEventDetails)).EndInit();
            this.splitContainerImagesEventDetails.ResumeLayout(false);
            this.tableLayoutPanelEventImages.ResumeLayout(false);
            this.tableLayoutPanelEventImages.PerformLayout();
            this.toolStripEventImages.ResumeLayout(false);
            this.toolStripEventImages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWithholdEventImage)).EndInit();
            this.tabControlEventImageDetails.ResumeLayout(false);
            this.tabPageEventImageDetails.ResumeLayout(false);
            this.tableLayoutPanelEventImageDetails.ResumeLayout(false);
            this.tableLayoutPanelEventImageDetails.PerformLayout();
            this.tabPageEventImageLicense.ResumeLayout(false);
            this.tableLayoutPanelEventImageLicense.ResumeLayout(false);
            this.tableLayoutPanelEventImageLicense.PerformLayout();
            this.groupBoxEventImageLicenseDetails.ResumeLayout(false);
            this.tableLayoutPanelEventImageLicenseDetails.ResumeLayout(false);
            this.tableLayoutPanelEventImageLicenseDetails.PerformLayout();
            this.tabPageEventImageExif.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxEventImages;
        private System.Windows.Forms.SplitContainer splitContainerImagesEventDetails;
        private DiversityWorkbench.UserControls.UserControlImage userControlImageEventImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEventImages;
        private System.Windows.Forms.ToolStrip toolStripEventImages;
        private System.Windows.Forms.ToolStripButton toolStripButtonEventImageNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonEventImageDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenEventImageModule;
        private System.Windows.Forms.ToolStripButton toolStripButtonEventImageDescription;
        private System.Windows.Forms.ListBox listBoxEventImages;
        private System.Windows.Forms.ComboBox comboBoxEventImageDataWithholdingReason;
        private System.Windows.Forms.PictureBox pictureBoxWithholdEventImage;
        private System.Windows.Forms.ImageList imageListForm;
        private System.Windows.Forms.ImageList imageListEventImages;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.TabControl tabControlEventImageDetails;
        private System.Windows.Forms.TabPage tabPageEventImageDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEventImageDetails;
        private System.Windows.Forms.Label labelEventImageTitle;
        private System.Windows.Forms.TextBox textBoxEventImageTitle;
        private System.Windows.Forms.TextBox textBoxEventImageNotes;
        private System.Windows.Forms.TextBox textBoxEventImageInternalNotes;
        private System.Windows.Forms.Label labelEventImageInternalNotes;
        private System.Windows.Forms.Label labelEventImageNotes;
        private System.Windows.Forms.ComboBox comboBoxEventImageType;
        private System.Windows.Forms.Label labelEventImageType;
        private System.Windows.Forms.Label labelEventImageCreator;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryEventImageCreator;
        private System.Windows.Forms.TabPage tabPageEventImageLicense;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEventImageLicense;
        private System.Windows.Forms.Label labelEventImageIPR;
        private System.Windows.Forms.TextBox textBoxEventImageIPR;
        private System.Windows.Forms.Label labelEventImageCopyrightStatement;
        private System.Windows.Forms.TextBox textBoxEventImageCopyrightStatement;
        private System.Windows.Forms.GroupBox groupBoxEventImageLicenseDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEventImageLicenseDetails;
        private System.Windows.Forms.Label labelEventImageLicenseType;
        private System.Windows.Forms.TextBox textBoxEventImageLicenseType;
        private System.Windows.Forms.Label labelEventImageLicenseHolder;
        private System.Windows.Forms.TextBox textBoxEventImageLicenseYear;
        private System.Windows.Forms.Label labelEventImageLicenseYear;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryEventImageLicenseHolder;
        private System.Windows.Forms.TabPage tabPageEventImageExif;
        private DiversityWorkbench.UserControls.UserControlXMLTree userControlXMLTreeEventImageExif;
        private System.Windows.Forms.ToolStripButton toolStripButtonSwitchBrowser;
    }
}
