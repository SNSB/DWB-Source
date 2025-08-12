namespace DiversityCollection.Forms
{
    partial class FormDatawithholding
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatawithholding));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageSummary = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSummaryImages = new System.Windows.Forms.TableLayoutPanel();
            this.userControlDatawithholdingSummaryImagesCollection = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.textBoxSummaryNoEmbargo = new System.Windows.Forms.TextBox();
            this.pictureBoxSummaryImages = new System.Windows.Forms.PictureBox();
            this.labelSummaryImages = new System.Windows.Forms.Label();
            this.userControlDatawithholdingSummaryImagesSeries = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.userControlDatawithholdingSummaryImagesEvent = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.userControlDatawithholdingSummaryImagesSpecimen = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.pictureBoxSummaryEmbargo = new System.Windows.Forms.PictureBox();
            this.panelEmbargo = new System.Windows.Forms.Panel();
            this.userControlDatawithholdingSummaryUnit = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.userControlDatawithholdingSummaryPart = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.userControlDatawithholdingSummaryAgent = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.userControlDatawithholdingSummarySpecimen = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.userControlDatawithholdingSummaryCollectionDate = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.userControlDatawithholdingSummaryEvent = new DiversityCollection.UserControls.UserControlDatawithholdingSummary();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.tabPageCollectionDate = new System.Windows.Forms.TabPage();
            this.tabPageSpecimen = new System.Windows.Forms.TabPage();
            this.tabPageAgent = new System.Windows.Forms.TabPage();
            this.tabPageUnit = new System.Windows.Forms.TabPage();
            this.tabPagePart = new System.Windows.Forms.TabPage();
            this.tabPageImage = new System.Windows.Forms.TabPage();
            this.tabControlImages = new System.Windows.Forms.TabControl();
            this.tabPageImagesSeries = new System.Windows.Forms.TabPage();
            this.tabPageImagesEvent = new System.Windows.Forms.TabPage();
            this.tabPageImagesSpecimen = new System.Windows.Forms.TabPage();
            this.tabPageImagesCollection = new System.Windows.Forms.TabPage();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageSummary.SuspendLayout();
            this.tableLayoutPanelSummaryImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSummaryImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSummaryEmbargo)).BeginInit();
            this.tabPageImage.SuspendLayout();
            this.tabControlImages.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tabControlMain, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonFeedback, 1, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(703, 409);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(6, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(661, 17);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Handling the datawithholding reasons for the selected data";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControlMain
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.tabControlMain, 2);
            this.tabControlMain.Controls.Add(this.tabPageSummary);
            this.tabControlMain.Controls.Add(this.tabPageEvent);
            this.tabControlMain.Controls.Add(this.tabPageCollectionDate);
            this.tabControlMain.Controls.Add(this.tabPageSpecimen);
            this.tabControlMain.Controls.Add(this.tabPageAgent);
            this.tabControlMain.Controls.Add(this.tabPageUnit);
            this.tabControlMain.Controls.Add(this.tabPagePart);
            this.tabControlMain.Controls.Add(this.tabPageImage);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.ImageList = this.imageList;
            this.tabControlMain.Location = new System.Drawing.Point(3, 32);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(697, 374);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageSummary
            // 
            this.tabPageSummary.Controls.Add(this.tableLayoutPanelSummaryImages);
            this.tabPageSummary.Controls.Add(this.userControlDatawithholdingSummaryUnit);
            this.tabPageSummary.Controls.Add(this.userControlDatawithholdingSummaryPart);
            this.tabPageSummary.Controls.Add(this.userControlDatawithholdingSummaryAgent);
            this.tabPageSummary.Controls.Add(this.userControlDatawithholdingSummarySpecimen);
            this.tabPageSummary.Controls.Add(this.userControlDatawithholdingSummaryCollectionDate);
            this.tabPageSummary.Controls.Add(this.userControlDatawithholdingSummaryEvent);
            this.tabPageSummary.Location = new System.Drawing.Point(4, 23);
            this.tabPageSummary.Name = "tabPageSummary";
            this.tabPageSummary.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.tabPageSummary.Size = new System.Drawing.Size(689, 347);
            this.tabPageSummary.TabIndex = 4;
            this.tabPageSummary.Text = "Summary";
            this.tabPageSummary.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelSummaryImages
            // 
            this.tableLayoutPanelSummaryImages.ColumnCount = 3;
            this.tableLayoutPanelSummaryImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSummaryImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSummaryImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSummaryImages.Controls.Add(this.userControlDatawithholdingSummaryImagesCollection, 1, 5);
            this.tableLayoutPanelSummaryImages.Controls.Add(this.textBoxSummaryNoEmbargo, 1, 7);
            this.tableLayoutPanelSummaryImages.Controls.Add(this.pictureBoxSummaryImages, 0, 1);
            this.tableLayoutPanelSummaryImages.Controls.Add(this.labelSummaryImages, 1, 1);
            this.tableLayoutPanelSummaryImages.Controls.Add(this.userControlDatawithholdingSummaryImagesSeries, 1, 2);
            this.tableLayoutPanelSummaryImages.Controls.Add(this.userControlDatawithholdingSummaryImagesEvent, 1, 3);
            this.tableLayoutPanelSummaryImages.Controls.Add(this.userControlDatawithholdingSummaryImagesSpecimen, 1, 4);
            this.tableLayoutPanelSummaryImages.Controls.Add(this.pictureBoxSummaryEmbargo, 0, 7);
            this.tableLayoutPanelSummaryImages.Controls.Add(this.panelEmbargo, 2, 7);
            this.tableLayoutPanelSummaryImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSummaryImages.Location = new System.Drawing.Point(0, 141);
            this.tableLayoutPanelSummaryImages.Name = "tableLayoutPanelSummaryImages";
            this.tableLayoutPanelSummaryImages.RowCount = 8;
            this.tableLayoutPanelSummaryImages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSummaryImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSummaryImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSummaryImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSummaryImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSummaryImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSummaryImages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSummaryImages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSummaryImages.Size = new System.Drawing.Size(689, 206);
            this.tableLayoutPanelSummaryImages.TabIndex = 3;
            // 
            // userControlDatawithholdingSummaryImagesCollection
            // 
            this.tableLayoutPanelSummaryImages.SetColumnSpan(this.userControlDatawithholdingSummaryImagesCollection, 2);
            this.userControlDatawithholdingSummaryImagesCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatawithholdingSummaryImagesCollection.Location = new System.Drawing.Point(25, 129);
            this.userControlDatawithholdingSummaryImagesCollection.Name = "userControlDatawithholdingSummaryImagesCollection";
            this.userControlDatawithholdingSummaryImagesCollection.Size = new System.Drawing.Size(661, 22);
            this.userControlDatawithholdingSummaryImagesCollection.TabIndex = 5;
            // 
            // textBoxSummaryNoEmbargo
            // 
            this.textBoxSummaryNoEmbargo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSummaryNoEmbargo.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxSummaryNoEmbargo.ForeColor = System.Drawing.Color.Green;
            this.textBoxSummaryNoEmbargo.Location = new System.Drawing.Point(25, 174);
            this.textBoxSummaryNoEmbargo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxSummaryNoEmbargo.Multiline = true;
            this.textBoxSummaryNoEmbargo.Name = "textBoxSummaryNoEmbargo";
            this.textBoxSummaryNoEmbargo.Size = new System.Drawing.Size(108, 29);
            this.textBoxSummaryNoEmbargo.TabIndex = 0;
            // 
            // pictureBoxSummaryImages
            // 
            this.pictureBoxSummaryImages.Image = global::DiversityCollection.Resource.Icones;
            this.pictureBoxSummaryImages.Location = new System.Drawing.Point(3, 23);
            this.pictureBoxSummaryImages.Name = "pictureBoxSummaryImages";
            this.pictureBoxSummaryImages.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSummaryImages.TabIndex = 0;
            this.pictureBoxSummaryImages.TabStop = false;
            // 
            // labelSummaryImages
            // 
            this.labelSummaryImages.AutoSize = true;
            this.tableLayoutPanelSummaryImages.SetColumnSpan(this.labelSummaryImages, 2);
            this.labelSummaryImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSummaryImages.Location = new System.Drawing.Point(25, 20);
            this.labelSummaryImages.Name = "labelSummaryImages";
            this.labelSummaryImages.Size = new System.Drawing.Size(41, 22);
            this.labelSummaryImages.TabIndex = 1;
            this.labelSummaryImages.Text = "Images";
            this.labelSummaryImages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userControlDatawithholdingSummaryImagesSeries
            // 
            this.tableLayoutPanelSummaryImages.SetColumnSpan(this.userControlDatawithholdingSummaryImagesSeries, 2);
            this.userControlDatawithholdingSummaryImagesSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatawithholdingSummaryImagesSeries.Location = new System.Drawing.Point(25, 45);
            this.userControlDatawithholdingSummaryImagesSeries.Name = "userControlDatawithholdingSummaryImagesSeries";
            this.userControlDatawithholdingSummaryImagesSeries.Size = new System.Drawing.Size(661, 22);
            this.userControlDatawithholdingSummaryImagesSeries.TabIndex = 2;
            // 
            // userControlDatawithholdingSummaryImagesEvent
            // 
            this.tableLayoutPanelSummaryImages.SetColumnSpan(this.userControlDatawithholdingSummaryImagesEvent, 2);
            this.userControlDatawithholdingSummaryImagesEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatawithholdingSummaryImagesEvent.Location = new System.Drawing.Point(25, 73);
            this.userControlDatawithholdingSummaryImagesEvent.Name = "userControlDatawithholdingSummaryImagesEvent";
            this.userControlDatawithholdingSummaryImagesEvent.Size = new System.Drawing.Size(661, 22);
            this.userControlDatawithholdingSummaryImagesEvent.TabIndex = 3;
            // 
            // userControlDatawithholdingSummaryImagesSpecimen
            // 
            this.tableLayoutPanelSummaryImages.SetColumnSpan(this.userControlDatawithholdingSummaryImagesSpecimen, 2);
            this.userControlDatawithholdingSummaryImagesSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatawithholdingSummaryImagesSpecimen.Location = new System.Drawing.Point(25, 101);
            this.userControlDatawithholdingSummaryImagesSpecimen.Name = "userControlDatawithholdingSummaryImagesSpecimen";
            this.userControlDatawithholdingSummaryImagesSpecimen.Size = new System.Drawing.Size(661, 22);
            this.userControlDatawithholdingSummaryImagesSpecimen.TabIndex = 4;
            // 
            // pictureBoxSummaryEmbargo
            // 
            this.pictureBoxSummaryEmbargo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxSummaryEmbargo.Image = global::DiversityCollection.Resource.TransactionEmbargo;
            this.pictureBoxSummaryEmbargo.InitialImage = global::DiversityCollection.Resource.TransactionEmbargo;
            this.pictureBoxSummaryEmbargo.Location = new System.Drawing.Point(3, 177);
            this.pictureBoxSummaryEmbargo.Name = "pictureBoxSummaryEmbargo";
            this.pictureBoxSummaryEmbargo.Size = new System.Drawing.Size(16, 26);
            this.pictureBoxSummaryEmbargo.TabIndex = 6;
            this.pictureBoxSummaryEmbargo.TabStop = false;
            // 
            // panelEmbargo
            // 
            this.panelEmbargo.AutoScroll = true;
            this.panelEmbargo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmbargo.Location = new System.Drawing.Point(136, 174);
            this.panelEmbargo.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmbargo.Name = "panelEmbargo";
            this.panelEmbargo.Size = new System.Drawing.Size(553, 32);
            this.panelEmbargo.TabIndex = 7;
            // 
            // userControlDatawithholdingSummaryUnit
            // 
            this.userControlDatawithholdingSummaryUnit.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDatawithholdingSummaryUnit.Location = new System.Drawing.Point(0, 119);
            this.userControlDatawithholdingSummaryUnit.Name = "userControlDatawithholdingSummaryUnit";
            this.userControlDatawithholdingSummaryUnit.Size = new System.Drawing.Size(689, 22);
            this.userControlDatawithholdingSummaryUnit.TabIndex = 5;
            // 
            // userControlDatawithholdingSummaryPart
            // 
            this.userControlDatawithholdingSummaryPart.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDatawithholdingSummaryPart.Location = new System.Drawing.Point(0, 97);
            this.userControlDatawithholdingSummaryPart.Name = "userControlDatawithholdingSummaryPart";
            this.userControlDatawithholdingSummaryPart.Size = new System.Drawing.Size(689, 22);
            this.userControlDatawithholdingSummaryPart.TabIndex = 4;
            // 
            // userControlDatawithholdingSummaryAgent
            // 
            this.userControlDatawithholdingSummaryAgent.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDatawithholdingSummaryAgent.Location = new System.Drawing.Point(0, 75);
            this.userControlDatawithholdingSummaryAgent.Name = "userControlDatawithholdingSummaryAgent";
            this.userControlDatawithholdingSummaryAgent.Size = new System.Drawing.Size(689, 22);
            this.userControlDatawithholdingSummaryAgent.TabIndex = 2;
            // 
            // userControlDatawithholdingSummarySpecimen
            // 
            this.userControlDatawithholdingSummarySpecimen.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDatawithholdingSummarySpecimen.Location = new System.Drawing.Point(0, 53);
            this.userControlDatawithholdingSummarySpecimen.Name = "userControlDatawithholdingSummarySpecimen";
            this.userControlDatawithholdingSummarySpecimen.Size = new System.Drawing.Size(689, 22);
            this.userControlDatawithholdingSummarySpecimen.TabIndex = 1;
            // 
            // userControlDatawithholdingSummaryCollectionDate
            // 
            this.userControlDatawithholdingSummaryCollectionDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDatawithholdingSummaryCollectionDate.Location = new System.Drawing.Point(0, 31);
            this.userControlDatawithholdingSummaryCollectionDate.Name = "userControlDatawithholdingSummaryCollectionDate";
            this.userControlDatawithholdingSummaryCollectionDate.Size = new System.Drawing.Size(689, 22);
            this.userControlDatawithholdingSummaryCollectionDate.TabIndex = 6;
            // 
            // userControlDatawithholdingSummaryEvent
            // 
            this.userControlDatawithholdingSummaryEvent.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDatawithholdingSummaryEvent.Location = new System.Drawing.Point(0, 9);
            this.userControlDatawithholdingSummaryEvent.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.userControlDatawithholdingSummaryEvent.Name = "userControlDatawithholdingSummaryEvent";
            this.userControlDatawithholdingSummaryEvent.Size = new System.Drawing.Size(689, 22);
            this.userControlDatawithholdingSummaryEvent.TabIndex = 0;
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.ImageIndex = 0;
            this.tabPageEvent.Location = new System.Drawing.Point(4, 23);
            this.tabPageEvent.Name = "tabPageEvent";
            this.tabPageEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEvent.Size = new System.Drawing.Size(689, 347);
            this.tabPageEvent.TabIndex = 0;
            this.tabPageEvent.Text = "Collection events";
            this.tabPageEvent.UseVisualStyleBackColor = true;
            // 
            // tabPageCollectionDate
            // 
            this.tabPageCollectionDate.ImageIndex = 8;
            this.tabPageCollectionDate.Location = new System.Drawing.Point(4, 23);
            this.tabPageCollectionDate.Name = "tabPageCollectionDate";
            this.tabPageCollectionDate.Size = new System.Drawing.Size(689, 347);
            this.tabPageCollectionDate.TabIndex = 7;
            this.tabPageCollectionDate.Text = "Collection date";
            this.tabPageCollectionDate.UseVisualStyleBackColor = true;
            // 
            // tabPageSpecimen
            // 
            this.tabPageSpecimen.ImageIndex = 1;
            this.tabPageSpecimen.Location = new System.Drawing.Point(4, 23);
            this.tabPageSpecimen.Name = "tabPageSpecimen";
            this.tabPageSpecimen.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSpecimen.Size = new System.Drawing.Size(689, 347);
            this.tabPageSpecimen.TabIndex = 1;
            this.tabPageSpecimen.Text = "Specimens";
            this.tabPageSpecimen.UseVisualStyleBackColor = true;
            // 
            // tabPageAgent
            // 
            this.tabPageAgent.ImageIndex = 2;
            this.tabPageAgent.Location = new System.Drawing.Point(4, 23);
            this.tabPageAgent.Name = "tabPageAgent";
            this.tabPageAgent.Size = new System.Drawing.Size(689, 347);
            this.tabPageAgent.TabIndex = 2;
            this.tabPageAgent.Text = "Agents";
            this.tabPageAgent.UseVisualStyleBackColor = true;
            // 
            // tabPageUnit
            // 
            this.tabPageUnit.ImageIndex = 7;
            this.tabPageUnit.Location = new System.Drawing.Point(4, 23);
            this.tabPageUnit.Name = "tabPageUnit";
            this.tabPageUnit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUnit.Size = new System.Drawing.Size(689, 347);
            this.tabPageUnit.TabIndex = 6;
            this.tabPageUnit.Text = "Organism";
            this.tabPageUnit.UseVisualStyleBackColor = true;
            // 
            // tabPagePart
            // 
            this.tabPagePart.ImageIndex = 3;
            this.tabPagePart.Location = new System.Drawing.Point(4, 23);
            this.tabPagePart.Name = "tabPagePart";
            this.tabPagePart.Size = new System.Drawing.Size(689, 347);
            this.tabPagePart.TabIndex = 5;
            this.tabPagePart.Text = "Part";
            this.tabPagePart.UseVisualStyleBackColor = true;
            // 
            // tabPageImage
            // 
            this.tabPageImage.Controls.Add(this.tabControlImages);
            this.tabPageImage.ImageIndex = 4;
            this.tabPageImage.Location = new System.Drawing.Point(4, 23);
            this.tabPageImage.Name = "tabPageImage";
            this.tabPageImage.Size = new System.Drawing.Size(689, 347);
            this.tabPageImage.TabIndex = 3;
            this.tabPageImage.Text = "Images";
            this.tabPageImage.UseVisualStyleBackColor = true;
            // 
            // tabControlImages
            // 
            this.tabControlImages.Controls.Add(this.tabPageImagesSeries);
            this.tabControlImages.Controls.Add(this.tabPageImagesEvent);
            this.tabControlImages.Controls.Add(this.tabPageImagesSpecimen);
            this.tabControlImages.Controls.Add(this.tabPageImagesCollection);
            this.tabControlImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlImages.ImageList = this.imageList;
            this.tabControlImages.Location = new System.Drawing.Point(0, 0);
            this.tabControlImages.Name = "tabControlImages";
            this.tabControlImages.SelectedIndex = 0;
            this.tabControlImages.Size = new System.Drawing.Size(689, 347);
            this.tabControlImages.TabIndex = 0;
            // 
            // tabPageImagesSeries
            // 
            this.tabPageImagesSeries.ImageIndex = 6;
            this.tabPageImagesSeries.Location = new System.Drawing.Point(4, 23);
            this.tabPageImagesSeries.Name = "tabPageImagesSeries";
            this.tabPageImagesSeries.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImagesSeries.Size = new System.Drawing.Size(681, 320);
            this.tabPageImagesSeries.TabIndex = 0;
            this.tabPageImagesSeries.Text = "Images of event series";
            this.tabPageImagesSeries.UseVisualStyleBackColor = true;
            // 
            // tabPageImagesEvent
            // 
            this.tabPageImagesEvent.ImageIndex = 0;
            this.tabPageImagesEvent.Location = new System.Drawing.Point(4, 23);
            this.tabPageImagesEvent.Name = "tabPageImagesEvent";
            this.tabPageImagesEvent.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImagesEvent.Size = new System.Drawing.Size(681, 320);
            this.tabPageImagesEvent.TabIndex = 1;
            this.tabPageImagesEvent.Text = "Images of collection events";
            this.tabPageImagesEvent.UseVisualStyleBackColor = true;
            // 
            // tabPageImagesSpecimen
            // 
            this.tabPageImagesSpecimen.ImageIndex = 1;
            this.tabPageImagesSpecimen.Location = new System.Drawing.Point(4, 23);
            this.tabPageImagesSpecimen.Name = "tabPageImagesSpecimen";
            this.tabPageImagesSpecimen.Size = new System.Drawing.Size(681, 320);
            this.tabPageImagesSpecimen.TabIndex = 2;
            this.tabPageImagesSpecimen.Text = "Images of specimens";
            this.tabPageImagesSpecimen.UseVisualStyleBackColor = true;
            // 
            // tabPageImagesCollection
            // 
            this.tabPageImagesCollection.ImageIndex = 5;
            this.tabPageImagesCollection.Location = new System.Drawing.Point(4, 23);
            this.tabPageImagesCollection.Name = "tabPageImagesCollection";
            this.tabPageImagesCollection.Size = new System.Drawing.Size(681, 320);
            this.tabPageImagesCollection.TabIndex = 3;
            this.tabPageImagesCollection.Text = "Images of collections";
            this.tabPageImagesCollection.UseVisualStyleBackColor = true;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Event.ico");
            this.imageList.Images.SetKeyName(1, "CollectionSpecimen.ico");
            this.imageList.Images.SetKeyName(2, "Agent.ico");
            this.imageList.Images.SetKeyName(3, "Specimen.ico");
            this.imageList.Images.SetKeyName(4, "Icones.ico");
            this.imageList.Images.SetKeyName(5, "Collection.ico");
            this.imageList.Images.SetKeyName(6, "EventSeries.ico");
            this.imageList.Images.SetKeyName(7, "Plant.ico");
            this.imageList.Images.SetKeyName(8, "Time.ico");
            this.imageList.Images.SetKeyName(9, "Project.ico");
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(676, 3);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(24, 23);
            this.buttonFeedback.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonFeedback, "Sending feedback to the administrator");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // FormDatawithholding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 409);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDatawithholding";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Withholding data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDatawithholding_FormClosing);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSummary.ResumeLayout(false);
            this.tableLayoutPanelSummaryImages.ResumeLayout(false);
            this.tableLayoutPanelSummaryImages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSummaryImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSummaryEmbargo)).EndInit();
            this.tabPageImage.ResumeLayout(false);
            this.tabControlImages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageEvent;
        private System.Windows.Forms.TabPage tabPageSpecimen;
        private System.Windows.Forms.TabPage tabPageAgent;
        private System.Windows.Forms.TabPage tabPageImage;
        private System.Windows.Forms.TabControl tabControlImages;
        private System.Windows.Forms.TabPage tabPageImagesSeries;
        private System.Windows.Forms.TabPage tabPageImagesEvent;
        private System.Windows.Forms.TabPage tabPageImagesSpecimen;
        private System.Windows.Forms.TabPage tabPageImagesCollection;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.TabPage tabPageSummary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSummaryImages;
        private System.Windows.Forms.PictureBox pictureBoxSummaryImages;
        private System.Windows.Forms.Label labelSummaryImages;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryImagesSeries;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryImagesEvent;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryImagesSpecimen;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryAgent;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummarySpecimen;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryEvent;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryImagesCollection;
        private System.Windows.Forms.PictureBox pictureBoxSummaryEmbargo;
        private System.Windows.Forms.Panel panelEmbargo;
        private System.Windows.Forms.TextBox textBoxSummaryNoEmbargo;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryPart;
        private System.Windows.Forms.TabPage tabPagePart;
        private System.Windows.Forms.TabPage tabPageUnit;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryUnit;
        private System.Windows.Forms.TabPage tabPageCollectionDate;
        private UserControls.UserControlDatawithholdingSummary userControlDatawithholdingSummaryCollectionDate;
    }
}