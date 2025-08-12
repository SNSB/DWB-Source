namespace DiversityCollection.UserControls
{
    partial class UserControl_Event
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Event));
            this.groupBoxEvent = new System.Windows.Forms.GroupBox();
            this.pictureBoxEvent = new System.Windows.Forms.PictureBox();
            this.splitContainerOverviewEventData = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelEvent = new System.Windows.Forms.TableLayoutPanel();
            this.userControlModuleRelatedEntryEventReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlDatePanelEventDate = new DiversityWorkbench.UserControls.UserControlDatePanel();
            this.labelEventDate = new System.Windows.Forms.Label();
            this.labelCollectionTime = new System.Windows.Forms.Label();
            this.textBoxCollectionTime = new System.Windows.Forms.TextBox();
            this.labelCollectionTimeSpan = new System.Windows.Forms.Label();
            this.textBoxCollectionTimeSpan = new System.Windows.Forms.TextBox();
            this.labelCollectionDateCategory = new System.Windows.Forms.Label();
            this.comboBoxCollectionDateCategory = new System.Windows.Forms.ComboBox();
            this.labelDataWithholdingReasonEvent = new System.Windows.Forms.Label();
            this.labelCollectingMethod = new System.Windows.Forms.Label();
            this.textBoxCollectingMethod = new System.Windows.Forms.TextBox();
            this.labelEventReference = new System.Windows.Forms.Label();
            this.labelEventNotes = new System.Windows.Forms.Label();
            this.textBoxEventNotes = new System.Windows.Forms.TextBox();
            this.labelCountryCache = new System.Windows.Forms.Label();
            this.textBoxCountryCache = new System.Windows.Forms.TextBox();
            this.labelCollectorsEventNumber = new System.Windows.Forms.Label();
            this.textBoxCollectorsEventNumber = new System.Windows.Forms.TextBox();
            this.comboBoxDataWithholdingReasonEvent = new System.Windows.Forms.ComboBox();
            this.buttonCountryEditing = new System.Windows.Forms.Button();
            this.userControlHierarchySelectorCollectionDateCategory = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            this.labelEventReferenceDetails = new System.Windows.Forms.Label();
            this.textBoxEventReferenceDetails = new System.Windows.Forms.TextBox();
            this.pictureBoxWithholdingReasonEvent = new System.Windows.Forms.PictureBox();
            this.labelEventEnd = new System.Windows.Forms.Label();
            this.userControlDatePanelEventEnd = new DiversityWorkbench.UserControls.UserControlDatePanel();
            this.buttonWithholdingReasonCollectionDate = new System.Windows.Forms.Button();
            this.splitContainerOverviewEventDescriptions = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelLocalityDescription = new System.Windows.Forms.TableLayoutPanel();
            this.labelLocalityDescriptionVerbatim = new System.Windows.Forms.Label();
            this.labelLocalityDescription = new System.Windows.Forms.Label();
            this.textBoxEventLocality = new System.Windows.Forms.TextBox();
            this.textBoxLocalityDescriptionVerbatim = new System.Windows.Forms.TextBox();
            this.textBoxHabitatDesciption = new System.Windows.Forms.TextBox();
            this.labelHabitatDescription = new System.Windows.Forms.Label();
            this.groupBoxEvent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEvent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewEventData)).BeginInit();
            this.splitContainerOverviewEventData.Panel1.SuspendLayout();
            this.splitContainerOverviewEventData.Panel2.SuspendLayout();
            this.splitContainerOverviewEventData.SuspendLayout();
            this.tableLayoutPanelEvent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWithholdingReasonEvent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewEventDescriptions)).BeginInit();
            this.splitContainerOverviewEventDescriptions.Panel1.SuspendLayout();
            this.splitContainerOverviewEventDescriptions.Panel2.SuspendLayout();
            this.splitContainerOverviewEventDescriptions.SuspendLayout();
            this.tableLayoutPanelLocalityDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxEvent
            // 
            this.groupBoxEvent.AccessibleName = "CollectionEvent";
            this.groupBoxEvent.Controls.Add(this.pictureBoxEvent);
            this.groupBoxEvent.Controls.Add(this.splitContainerOverviewEventData);
            this.groupBoxEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEvent.ForeColor = System.Drawing.Color.Black;
            this.groupBoxEvent.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEvent.MinimumSize = new System.Drawing.Size(0, 200);
            this.groupBoxEvent.Name = "groupBoxEvent";
            this.groupBoxEvent.Size = new System.Drawing.Size(608, 300);
            this.groupBoxEvent.TabIndex = 0;
            this.groupBoxEvent.TabStop = false;
            this.groupBoxEvent.Text = "Collection event";
            // 
            // pictureBoxEvent
            // 
            this.pictureBoxEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxEvent.Image = global::DiversityCollection.Resource.Event;
            this.pictureBoxEvent.Location = new System.Drawing.Point(588, 1);
            this.pictureBoxEvent.Name = "pictureBoxEvent";
            this.pictureBoxEvent.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxEvent.TabIndex = 1;
            this.pictureBoxEvent.TabStop = false;
            // 
            // splitContainerOverviewEventData
            // 
            this.splitContainerOverviewEventData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOverviewEventData.Location = new System.Drawing.Point(3, 16);
            this.splitContainerOverviewEventData.Name = "splitContainerOverviewEventData";
            this.splitContainerOverviewEventData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOverviewEventData.Panel1
            // 
            this.splitContainerOverviewEventData.Panel1.Controls.Add(this.tableLayoutPanelEvent);
            // 
            // splitContainerOverviewEventData.Panel2
            // 
            this.splitContainerOverviewEventData.Panel2.Controls.Add(this.splitContainerOverviewEventDescriptions);
            this.splitContainerOverviewEventData.Size = new System.Drawing.Size(602, 281);
            this.splitContainerOverviewEventData.SplitterDistance = 144;
            this.splitContainerOverviewEventData.TabIndex = 0;
            this.splitContainerOverviewEventData.TabStop = false;
            this.splitContainerOverviewEventData.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerOverviewEventData_SplitterMoved);
            // 
            // tableLayoutPanelEvent
            // 
            this.tableLayoutPanelEvent.ColumnCount = 7;
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEvent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelEvent.Controls.Add(this.userControlModuleRelatedEntryEventReference, 1, 2);
            this.tableLayoutPanelEvent.Controls.Add(this.userControlDatePanelEventDate, 1, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.labelEventDate, 0, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectionTime, 2, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCollectionTime, 3, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectionTimeSpan, 4, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCollectionTimeSpan, 5, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectionDateCategory, 4, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.comboBoxCollectionDateCategory, 5, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.labelDataWithholdingReasonEvent, 3, 3);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectingMethod, 3, 5);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCollectingMethod, 4, 5);
            this.tableLayoutPanelEvent.Controls.Add(this.labelEventReference, 0, 2);
            this.tableLayoutPanelEvent.Controls.Add(this.labelEventNotes, 0, 4);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxEventNotes, 1, 4);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCountryCache, 0, 5);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCountryCache, 1, 5);
            this.tableLayoutPanelEvent.Controls.Add(this.labelCollectorsEventNumber, 0, 3);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxCollectorsEventNumber, 1, 3);
            this.tableLayoutPanelEvent.Controls.Add(this.comboBoxDataWithholdingReasonEvent, 4, 3);
            this.tableLayoutPanelEvent.Controls.Add(this.buttonCountryEditing, 2, 5);
            this.tableLayoutPanelEvent.Controls.Add(this.userControlHierarchySelectorCollectionDateCategory, 6, 0);
            this.tableLayoutPanelEvent.Controls.Add(this.labelEventReferenceDetails, 4, 2);
            this.tableLayoutPanelEvent.Controls.Add(this.textBoxEventReferenceDetails, 5, 2);
            this.tableLayoutPanelEvent.Controls.Add(this.pictureBoxWithholdingReasonEvent, 6, 3);
            this.tableLayoutPanelEvent.Controls.Add(this.labelEventEnd, 0, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.userControlDatePanelEventEnd, 1, 1);
            this.tableLayoutPanelEvent.Controls.Add(this.buttonWithholdingReasonCollectionDate, 6, 1);
            this.tableLayoutPanelEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelEvent.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelEvent.Name = "tableLayoutPanelEvent";
            this.tableLayoutPanelEvent.RowCount = 7;
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEvent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEvent.Size = new System.Drawing.Size(602, 144);
            this.tableLayoutPanelEvent.TabIndex = 22;
            // 
            // userControlModuleRelatedEntryEventReference
            // 
            this.userControlModuleRelatedEntryEventReference.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelEvent.SetColumnSpan(this.userControlModuleRelatedEntryEventReference, 3);
            this.userControlModuleRelatedEntryEventReference.DependsOnUri = "";
            this.userControlModuleRelatedEntryEventReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryEventReference.Domain = "";
            this.userControlModuleRelatedEntryEventReference.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryEventReference.Location = new System.Drawing.Point(49, 50);
            this.userControlModuleRelatedEntryEventReference.Margin = new System.Windows.Forms.Padding(0, 3, 3, 2);
            this.userControlModuleRelatedEntryEventReference.Module = null;
            this.userControlModuleRelatedEntryEventReference.Name = "userControlModuleRelatedEntryEventReference";
            this.userControlModuleRelatedEntryEventReference.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryEventReference.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryEventReference.ShowInfo = false;
            this.userControlModuleRelatedEntryEventReference.Size = new System.Drawing.Size(373, 21);
            this.userControlModuleRelatedEntryEventReference.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryEventReference.TabIndex = 7;
            this.userControlModuleRelatedEntryEventReference.TabStop = false;
            // 
            // userControlDatePanelEventDate
            // 
            this.tableLayoutPanelEvent.SetColumnSpan(this.userControlDatePanelEventDate, 3);
            this.userControlDatePanelEventDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatePanelEventDate.Location = new System.Drawing.Point(49, 3);
            this.userControlDatePanelEventDate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.userControlDatePanelEventDate.Name = "userControlDatePanelEventDate";
            this.userControlDatePanelEventDate.Size = new System.Drawing.Size(376, 21);
            this.userControlDatePanelEventDate.TabIndex = 0;
            // 
            // labelEventDate
            // 
            this.labelEventDate.AccessibleName = "CollectionEvent.CollectionDate";
            this.labelEventDate.AutoSize = true;
            this.labelEventDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventDate.Location = new System.Drawing.Point(3, 0);
            this.labelEventDate.Name = "labelEventDate";
            this.labelEventDate.Size = new System.Drawing.Size(43, 27);
            this.labelEventDate.TabIndex = 1;
            this.labelEventDate.Text = "Date:";
            this.labelEventDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCollectionTime
            // 
            this.labelCollectionTime.AccessibleName = "CollectionEvent.CollectionTime";
            this.labelCollectionTime.AutoSize = true;
            this.labelCollectionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionTime.Location = new System.Drawing.Point(322, 27);
            this.labelCollectionTime.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCollectionTime.Name = "labelCollectionTime";
            this.labelCollectionTime.Size = new System.Drawing.Size(33, 20);
            this.labelCollectionTime.TabIndex = 4;
            this.labelCollectionTime.Text = "Time:";
            this.labelCollectionTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionTime
            // 
            this.textBoxCollectionTime.AccessibleName = "CollectionEvent.CollectionTime";
            this.textBoxCollectionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionTime.Location = new System.Drawing.Point(355, 27);
            this.textBoxCollectionTime.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxCollectionTime.Name = "textBoxCollectionTime";
            this.textBoxCollectionTime.Size = new System.Drawing.Size(70, 20);
            this.textBoxCollectionTime.TabIndex = 5;
            this.textBoxCollectionTime.TabStop = false;
            // 
            // labelCollectionTimeSpan
            // 
            this.labelCollectionTimeSpan.AccessibleName = "CollectionEvent.CollectionTimeSpan";
            this.labelCollectionTimeSpan.AutoSize = true;
            this.labelCollectionTimeSpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionTimeSpan.Location = new System.Drawing.Point(428, 27);
            this.labelCollectionTimeSpan.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCollectionTimeSpan.Name = "labelCollectionTimeSpan";
            this.labelCollectionTimeSpan.Size = new System.Drawing.Size(52, 20);
            this.labelCollectionTimeSpan.TabIndex = 6;
            this.labelCollectionTimeSpan.Text = "T.span:";
            this.labelCollectionTimeSpan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionTimeSpan
            // 
            this.textBoxCollectionTimeSpan.AccessibleName = "CollectionEvent.CollectionTimeSpan";
            this.textBoxCollectionTimeSpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionTimeSpan.Location = new System.Drawing.Point(480, 27);
            this.textBoxCollectionTimeSpan.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxCollectionTimeSpan.Name = "textBoxCollectionTimeSpan";
            this.textBoxCollectionTimeSpan.Size = new System.Drawing.Size(101, 20);
            this.textBoxCollectionTimeSpan.TabIndex = 6;
            this.textBoxCollectionTimeSpan.TabStop = false;
            // 
            // labelCollectionDateCategory
            // 
            this.labelCollectionDateCategory.AccessibleName = "CollectionEvent.CollectionDateCategory";
            this.labelCollectionDateCategory.AutoSize = true;
            this.labelCollectionDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionDateCategory.Location = new System.Drawing.Point(428, 3);
            this.labelCollectionDateCategory.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelCollectionDateCategory.Name = "labelCollectionDateCategory";
            this.labelCollectionDateCategory.Size = new System.Drawing.Size(52, 24);
            this.labelCollectionDateCategory.TabIndex = 1;
            this.labelCollectionDateCategory.Text = "Category:";
            this.labelCollectionDateCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxCollectionDateCategory
            // 
            this.comboBoxCollectionDateCategory.AccessibleName = "CollectionEvent.CollectionDateCategory";
            this.comboBoxCollectionDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollectionDateCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCollectionDateCategory.FormattingEnabled = true;
            this.comboBoxCollectionDateCategory.Location = new System.Drawing.Point(480, 3);
            this.comboBoxCollectionDateCategory.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.comboBoxCollectionDateCategory.Name = "comboBoxCollectionDateCategory";
            this.comboBoxCollectionDateCategory.Size = new System.Drawing.Size(104, 21);
            this.comboBoxCollectionDateCategory.TabIndex = 2;
            this.comboBoxCollectionDateCategory.TabStop = false;
            // 
            // labelDataWithholdingReasonEvent
            // 
            this.labelDataWithholdingReasonEvent.AccessibleName = "CollectionEvent.DataWithholdingReason";
            this.labelDataWithholdingReasonEvent.AutoSize = true;
            this.labelDataWithholdingReasonEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDataWithholdingReasonEvent.Location = new System.Drawing.Point(358, 73);
            this.labelDataWithholdingReasonEvent.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelDataWithholdingReasonEvent.Name = "labelDataWithholdingReasonEvent";
            this.labelDataWithholdingReasonEvent.Size = new System.Drawing.Size(67, 24);
            this.labelDataWithholdingReasonEvent.TabIndex = 9;
            this.labelDataWithholdingReasonEvent.Text = "Withhold.R.:";
            this.labelDataWithholdingReasonEvent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCollectingMethod
            // 
            this.labelCollectingMethod.AccessibleName = "CollectionEvent.CollectingMethod";
            this.labelCollectingMethod.AutoSize = true;
            this.labelCollectingMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectingMethod.Location = new System.Drawing.Point(358, 125);
            this.labelCollectingMethod.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelCollectingMethod.Name = "labelCollectingMethod";
            this.labelCollectingMethod.Size = new System.Drawing.Size(67, 19);
            this.labelCollectingMethod.TabIndex = 16;
            this.labelCollectingMethod.Text = "Coll.meth.:";
            this.labelCollectingMethod.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxCollectingMethod
            // 
            this.textBoxCollectingMethod.AccessibleName = "CollectionEvent.CollectingMethod";
            this.tableLayoutPanelEvent.SetColumnSpan(this.textBoxCollectingMethod, 3);
            this.textBoxCollectingMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectingMethod.Location = new System.Drawing.Point(425, 119);
            this.textBoxCollectingMethod.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxCollectingMethod.Multiline = true;
            this.textBoxCollectingMethod.Name = "textBoxCollectingMethod";
            this.textBoxCollectingMethod.Size = new System.Drawing.Size(177, 22);
            this.textBoxCollectingMethod.TabIndex = 14;
            this.textBoxCollectingMethod.TabStop = false;
            // 
            // labelEventReference
            // 
            this.labelEventReference.AccessibleName = "CollectionEvent.ReferenceTitle";
            this.labelEventReference.AutoSize = true;
            this.labelEventReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventReference.Location = new System.Drawing.Point(3, 47);
            this.labelEventReference.Name = "labelEventReference";
            this.labelEventReference.Size = new System.Drawing.Size(43, 26);
            this.labelEventReference.TabIndex = 8;
            this.labelEventReference.Text = "Ref.:";
            this.labelEventReference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEventNotes
            // 
            this.labelEventNotes.AccessibleName = "CollectionEvent.Notes";
            this.labelEventNotes.AutoSize = true;
            this.labelEventNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventNotes.Location = new System.Drawing.Point(3, 103);
            this.labelEventNotes.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelEventNotes.Name = "labelEventNotes";
            this.labelEventNotes.Size = new System.Drawing.Size(46, 16);
            this.labelEventNotes.TabIndex = 17;
            this.labelEventNotes.Text = "Notes:";
            this.labelEventNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxEventNotes
            // 
            this.textBoxEventNotes.AccessibleName = "CollectionEvent.Notes";
            this.tableLayoutPanelEvent.SetColumnSpan(this.textBoxEventNotes, 6);
            this.textBoxEventNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventNotes.Location = new System.Drawing.Point(49, 97);
            this.textBoxEventNotes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxEventNotes.Multiline = true;
            this.textBoxEventNotes.Name = "textBoxEventNotes";
            this.textBoxEventNotes.Size = new System.Drawing.Size(553, 19);
            this.textBoxEventNotes.TabIndex = 11;
            // 
            // labelCountryCache
            // 
            this.labelCountryCache.AccessibleName = "CollectionEvent.CountryCache";
            this.labelCountryCache.AutoSize = true;
            this.labelCountryCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountryCache.Location = new System.Drawing.Point(3, 119);
            this.labelCountryCache.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCountryCache.Name = "labelCountryCache";
            this.labelCountryCache.Size = new System.Drawing.Size(46, 25);
            this.labelCountryCache.TabIndex = 18;
            this.labelCountryCache.Text = "Country:";
            this.labelCountryCache.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCountryCache
            // 
            this.textBoxCountryCache.AccessibleName = "CollectionEvent.CountryCache";
            this.textBoxCountryCache.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCountryCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCountryCache.Location = new System.Drawing.Point(49, 124);
            this.textBoxCountryCache.Margin = new System.Windows.Forms.Padding(0, 5, 0, 3);
            this.textBoxCountryCache.Name = "textBoxCountryCache";
            this.textBoxCountryCache.ReadOnly = true;
            this.textBoxCountryCache.Size = new System.Drawing.Size(270, 13);
            this.textBoxCountryCache.TabIndex = 12;
            this.textBoxCountryCache.TabStop = false;
            // 
            // labelCollectorsEventNumber
            // 
            this.labelCollectorsEventNumber.AccessibleName = "CollectionEvent.CollectorsEventNumber";
            this.labelCollectorsEventNumber.AutoSize = true;
            this.labelCollectorsEventNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectorsEventNumber.Location = new System.Drawing.Point(3, 73);
            this.labelCollectorsEventNumber.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelCollectorsEventNumber.Name = "labelCollectorsEventNumber";
            this.labelCollectorsEventNumber.Size = new System.Drawing.Size(46, 24);
            this.labelCollectorsEventNumber.TabIndex = 20;
            this.labelCollectorsEventNumber.Text = "No.:";
            this.labelCollectorsEventNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectorsEventNumber
            // 
            this.textBoxCollectorsEventNumber.AccessibleName = "CollectionEvent.CollectorsEventNumber";
            this.tableLayoutPanelEvent.SetColumnSpan(this.textBoxCollectorsEventNumber, 2);
            this.textBoxCollectorsEventNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectorsEventNumber.Location = new System.Drawing.Point(49, 73);
            this.textBoxCollectorsEventNumber.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxCollectorsEventNumber.Name = "textBoxCollectorsEventNumber";
            this.textBoxCollectorsEventNumber.Size = new System.Drawing.Size(306, 20);
            this.textBoxCollectorsEventNumber.TabIndex = 9;
            this.textBoxCollectorsEventNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxCollectorsEventNumber_KeyDown);
            this.textBoxCollectorsEventNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCollectorsEventNumber_KeyPress);
            this.textBoxCollectorsEventNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxCollectorsEventNumber_KeyUp);
            // 
            // comboBoxDataWithholdingReasonEvent
            // 
            this.tableLayoutPanelEvent.SetColumnSpan(this.comboBoxDataWithholdingReasonEvent, 2);
            this.comboBoxDataWithholdingReasonEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxDataWithholdingReasonEvent.DropDownWidth = 200;
            this.comboBoxDataWithholdingReasonEvent.FormattingEnabled = true;
            this.comboBoxDataWithholdingReasonEvent.Location = new System.Drawing.Point(425, 73);
            this.comboBoxDataWithholdingReasonEvent.Margin = new System.Windows.Forms.Padding(0, 0, 1, 3);
            this.comboBoxDataWithholdingReasonEvent.Name = "comboBoxDataWithholdingReasonEvent";
            this.comboBoxDataWithholdingReasonEvent.Size = new System.Drawing.Size(158, 21);
            this.comboBoxDataWithholdingReasonEvent.TabIndex = 10;
            this.comboBoxDataWithholdingReasonEvent.DropDown += new System.EventHandler(this.comboBoxDataWithholdingReasonEvent_DropDown);
            this.comboBoxDataWithholdingReasonEvent.TextChanged += new System.EventHandler(this.comboBoxDataWithholdingReasonEvent_TextChanged);
            // 
            // buttonCountryEditing
            // 
            this.buttonCountryEditing.AccessibleName = "CollectionEvent.CountryCache";
            this.buttonCountryEditing.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCountryEditing.FlatAppearance.BorderSize = 0;
            this.buttonCountryEditing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCountryEditing.Image = global::DiversityCollection.Resource.Edit1;
            this.buttonCountryEditing.Location = new System.Drawing.Point(319, 119);
            this.buttonCountryEditing.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCountryEditing.Name = "buttonCountryEditing";
            this.buttonCountryEditing.Size = new System.Drawing.Size(16, 25);
            this.buttonCountryEditing.TabIndex = 13;
            this.buttonCountryEditing.TabStop = false;
            this.buttonCountryEditing.UseVisualStyleBackColor = true;
            this.buttonCountryEditing.Click += new System.EventHandler(this.buttonCountryEditing_Click);
            // 
            // userControlHierarchySelectorCollectionDateCategory
            // 
            this.userControlHierarchySelectorCollectionDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlHierarchySelectorCollectionDateCategory.Location = new System.Drawing.Point(584, 3);
            this.userControlHierarchySelectorCollectionDateCategory.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.userControlHierarchySelectorCollectionDateCategory.Name = "userControlHierarchySelectorCollectionDateCategory";
            this.userControlHierarchySelectorCollectionDateCategory.Size = new System.Drawing.Size(18, 21);
            this.userControlHierarchySelectorCollectionDateCategory.TabIndex = 3;
            this.userControlHierarchySelectorCollectionDateCategory.TabStop = false;
            // 
            // labelEventReferenceDetails
            // 
            this.labelEventReferenceDetails.AccessibleName = "CollectionEvent.ReferenceDetails";
            this.labelEventReferenceDetails.AutoSize = true;
            this.labelEventReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventReferenceDetails.Location = new System.Drawing.Point(428, 47);
            this.labelEventReferenceDetails.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventReferenceDetails.Name = "labelEventReferenceDetails";
            this.labelEventReferenceDetails.Size = new System.Drawing.Size(52, 26);
            this.labelEventReferenceDetails.TabIndex = 25;
            this.labelEventReferenceDetails.Text = "Details:";
            this.labelEventReferenceDetails.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEventReferenceDetails
            // 
            this.tableLayoutPanelEvent.SetColumnSpan(this.textBoxEventReferenceDetails, 2);
            this.textBoxEventReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventReferenceDetails.Location = new System.Drawing.Point(480, 50);
            this.textBoxEventReferenceDetails.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxEventReferenceDetails.Name = "textBoxEventReferenceDetails";
            this.textBoxEventReferenceDetails.Size = new System.Drawing.Size(122, 20);
            this.textBoxEventReferenceDetails.TabIndex = 8;
            this.textBoxEventReferenceDetails.TabStop = false;
            // 
            // pictureBoxWithholdingReasonEvent
            // 
            this.pictureBoxWithholdingReasonEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxWithholdingReasonEvent.Image = global::DiversityCollection.Resource.Stop3;
            this.pictureBoxWithholdingReasonEvent.Location = new System.Drawing.Point(584, 76);
            this.pictureBoxWithholdingReasonEvent.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.pictureBoxWithholdingReasonEvent.Name = "pictureBoxWithholdingReasonEvent";
            this.pictureBoxWithholdingReasonEvent.Size = new System.Drawing.Size(18, 21);
            this.pictureBoxWithholdingReasonEvent.TabIndex = 27;
            this.pictureBoxWithholdingReasonEvent.TabStop = false;
            // 
            // labelEventEnd
            // 
            this.labelEventEnd.AccessibleName = "CollectionEvent.CollectionEnd";
            this.labelEventEnd.AutoSize = true;
            this.labelEventEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventEnd.Location = new System.Drawing.Point(3, 27);
            this.labelEventEnd.Name = "labelEventEnd";
            this.labelEventEnd.Size = new System.Drawing.Size(43, 20);
            this.labelEventEnd.TabIndex = 28;
            this.labelEventEnd.Text = "End:";
            this.labelEventEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlDatePanelEventEnd
            // 
            this.userControlDatePanelEventEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDatePanelEventEnd.Location = new System.Drawing.Point(49, 27);
            this.userControlDatePanelEventEnd.Margin = new System.Windows.Forms.Padding(0);
            this.userControlDatePanelEventEnd.Name = "userControlDatePanelEventEnd";
            this.userControlDatePanelEventEnd.Size = new System.Drawing.Size(270, 20);
            this.userControlDatePanelEventEnd.TabIndex = 4;
            // 
            // buttonWithholdingReasonCollectionDate
            // 
            this.buttonWithholdingReasonCollectionDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonWithholdingReasonCollectionDate.FlatAppearance.BorderSize = 0;
            this.buttonWithholdingReasonCollectionDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWithholdingReasonCollectionDate.Image = global::DiversityCollection.Resource.Stop3Grey;
            this.buttonWithholdingReasonCollectionDate.Location = new System.Drawing.Point(584, 27);
            this.buttonWithholdingReasonCollectionDate.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWithholdingReasonCollectionDate.Name = "buttonWithholdingReasonCollectionDate";
            this.buttonWithholdingReasonCollectionDate.Size = new System.Drawing.Size(18, 20);
            this.buttonWithholdingReasonCollectionDate.TabIndex = 30;
            this.buttonWithholdingReasonCollectionDate.TabStop = false;
            this.buttonWithholdingReasonCollectionDate.UseVisualStyleBackColor = true;
            this.buttonWithholdingReasonCollectionDate.Click += new System.EventHandler(this.buttonWithholdingReasonCollectionDate_Click);
            // 
            // splitContainerOverviewEventDescriptions
            // 
            this.splitContainerOverviewEventDescriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOverviewEventDescriptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.splitContainerOverviewEventDescriptions.Location = new System.Drawing.Point(0, 0);
            this.splitContainerOverviewEventDescriptions.Name = "splitContainerOverviewEventDescriptions";
            // 
            // splitContainerOverviewEventDescriptions.Panel1
            // 
            this.splitContainerOverviewEventDescriptions.Panel1.Controls.Add(this.tableLayoutPanelLocalityDescription);
            // 
            // splitContainerOverviewEventDescriptions.Panel2
            // 
            this.splitContainerOverviewEventDescriptions.Panel2.Controls.Add(this.textBoxHabitatDesciption);
            this.splitContainerOverviewEventDescriptions.Panel2.Controls.Add(this.labelHabitatDescription);
            this.splitContainerOverviewEventDescriptions.Size = new System.Drawing.Size(602, 133);
            this.splitContainerOverviewEventDescriptions.SplitterDistance = 362;
            this.splitContainerOverviewEventDescriptions.TabIndex = 0;
            this.splitContainerOverviewEventDescriptions.TabStop = false;
            // 
            // tableLayoutPanelLocalityDescription
            // 
            this.tableLayoutPanelLocalityDescription.ColumnCount = 2;
            this.tableLayoutPanelLocalityDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelLocalityDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelLocalityDescription.Controls.Add(this.labelLocalityDescriptionVerbatim, 1, 0);
            this.tableLayoutPanelLocalityDescription.Controls.Add(this.labelLocalityDescription, 0, 0);
            this.tableLayoutPanelLocalityDescription.Controls.Add(this.textBoxEventLocality, 0, 1);
            this.tableLayoutPanelLocalityDescription.Controls.Add(this.textBoxLocalityDescriptionVerbatim, 1, 1);
            this.tableLayoutPanelLocalityDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLocalityDescription.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLocalityDescription.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.tableLayoutPanelLocalityDescription.Name = "tableLayoutPanelLocalityDescription";
            this.tableLayoutPanelLocalityDescription.RowCount = 2;
            this.tableLayoutPanelLocalityDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelLocalityDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLocalityDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLocalityDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelLocalityDescription.Size = new System.Drawing.Size(362, 133);
            this.tableLayoutPanelLocalityDescription.TabIndex = 24;
            // 
            // labelLocalityDescriptionVerbatim
            // 
            this.labelLocalityDescriptionVerbatim.AccessibleName = "CollectionEvent.LocalityVerbatim";
            this.labelLocalityDescriptionVerbatim.AutoSize = true;
            this.labelLocalityDescriptionVerbatim.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelLocalityDescriptionVerbatim.Location = new System.Drawing.Point(217, 0);
            this.labelLocalityDescriptionVerbatim.Margin = new System.Windows.Forms.Padding(0);
            this.labelLocalityDescriptionVerbatim.Name = "labelLocalityDescriptionVerbatim";
            this.labelLocalityDescriptionVerbatim.Size = new System.Drawing.Size(48, 13);
            this.labelLocalityDescriptionVerbatim.TabIndex = 23;
            this.labelLocalityDescriptionVerbatim.Text = "Verbatim";
            this.labelLocalityDescriptionVerbatim.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelLocalityDescription
            // 
            this.labelLocalityDescription.AccessibleName = "CollectionEvent.LocalityDescription";
            this.labelLocalityDescription.AutoSize = true;
            this.labelLocalityDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocalityDescription.Location = new System.Drawing.Point(0, 0);
            this.labelLocalityDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelLocalityDescription.Name = "labelLocalityDescription";
            this.labelLocalityDescription.Size = new System.Drawing.Size(217, 13);
            this.labelLocalityDescription.TabIndex = 22;
            this.labelLocalityDescription.Text = "Description of the locality";
            this.labelLocalityDescription.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxEventLocality
            // 
            this.textBoxEventLocality.AccessibleName = "CollectionEvent.LocalityDescription";
            this.textBoxEventLocality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventLocality.Location = new System.Drawing.Point(0, 13);
            this.textBoxEventLocality.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxEventLocality.Multiline = true;
            this.textBoxEventLocality.Name = "textBoxEventLocality";
            this.textBoxEventLocality.Size = new System.Drawing.Size(217, 120);
            this.textBoxEventLocality.TabIndex = 0;
            // 
            // textBoxLocalityDescriptionVerbatim
            // 
            this.textBoxLocalityDescriptionVerbatim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocalityDescriptionVerbatim.Location = new System.Drawing.Point(217, 13);
            this.textBoxLocalityDescriptionVerbatim.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxLocalityDescriptionVerbatim.Multiline = true;
            this.textBoxLocalityDescriptionVerbatim.Name = "textBoxLocalityDescriptionVerbatim";
            this.textBoxLocalityDescriptionVerbatim.Size = new System.Drawing.Size(145, 120);
            this.textBoxLocalityDescriptionVerbatim.TabIndex = 1;
            // 
            // textBoxHabitatDesciption
            // 
            this.textBoxHabitatDesciption.AccessibleName = "CollectionEvent.HabitatDescription";
            this.textBoxHabitatDesciption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHabitatDesciption.Location = new System.Drawing.Point(0, 13);
            this.textBoxHabitatDesciption.Multiline = true;
            this.textBoxHabitatDesciption.Name = "textBoxHabitatDesciption";
            this.textBoxHabitatDesciption.Size = new System.Drawing.Size(236, 120);
            this.textBoxHabitatDesciption.TabIndex = 0;
            // 
            // labelHabitatDescription
            // 
            this.labelHabitatDescription.AccessibleName = "CollectionEvent.HabitatDescription";
            this.labelHabitatDescription.AutoSize = true;
            this.labelHabitatDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHabitatDescription.Location = new System.Drawing.Point(0, 0);
            this.labelHabitatDescription.Name = "labelHabitatDescription";
            this.labelHabitatDescription.Size = new System.Drawing.Size(125, 13);
            this.labelHabitatDescription.TabIndex = 3;
            this.labelHabitatDescription.Text = "Description of the habitat";
            // 
            // UserControl_Event
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxEvent);
            this.Name = "UserControl_Event";
            this.Size = new System.Drawing.Size(608, 300);
            this.SizeChanged += new System.EventHandler(this.UserControl_Event_SizeChanged);
            this.groupBoxEvent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEvent)).EndInit();
            this.splitContainerOverviewEventData.Panel1.ResumeLayout(false);
            this.splitContainerOverviewEventData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewEventData)).EndInit();
            this.splitContainerOverviewEventData.ResumeLayout(false);
            this.tableLayoutPanelEvent.ResumeLayout(false);
            this.tableLayoutPanelEvent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWithholdingReasonEvent)).EndInit();
            this.splitContainerOverviewEventDescriptions.Panel1.ResumeLayout(false);
            this.splitContainerOverviewEventDescriptions.Panel2.ResumeLayout(false);
            this.splitContainerOverviewEventDescriptions.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewEventDescriptions)).EndInit();
            this.splitContainerOverviewEventDescriptions.ResumeLayout(false);
            this.tableLayoutPanelLocalityDescription.ResumeLayout(false);
            this.tableLayoutPanelLocalityDescription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxEvent;
        private System.Windows.Forms.PictureBox pictureBoxEvent;
        private System.Windows.Forms.SplitContainer splitContainerOverviewEventData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEvent;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryEventReference;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelEventDate;
        private System.Windows.Forms.Label labelEventDate;
        private System.Windows.Forms.Label labelCollectionTime;
        private System.Windows.Forms.TextBox textBoxCollectionTime;
        private System.Windows.Forms.Label labelCollectionTimeSpan;
        private System.Windows.Forms.TextBox textBoxCollectionTimeSpan;
        private System.Windows.Forms.Label labelCollectionDateCategory;
        private System.Windows.Forms.ComboBox comboBoxCollectionDateCategory;
        private System.Windows.Forms.Label labelDataWithholdingReasonEvent;
        private System.Windows.Forms.Label labelCollectingMethod;
        private System.Windows.Forms.TextBox textBoxCollectingMethod;
        private System.Windows.Forms.Label labelEventReference;
        private System.Windows.Forms.Label labelEventNotes;
        private System.Windows.Forms.TextBox textBoxEventNotes;
        private System.Windows.Forms.Label labelCountryCache;
        private System.Windows.Forms.TextBox textBoxCountryCache;
        private System.Windows.Forms.Label labelCollectorsEventNumber;
        private System.Windows.Forms.TextBox textBoxCollectorsEventNumber;
        private System.Windows.Forms.ComboBox comboBoxDataWithholdingReasonEvent;
        private System.Windows.Forms.Button buttonCountryEditing;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorCollectionDateCategory;
        private System.Windows.Forms.Label labelEventReferenceDetails;
        private System.Windows.Forms.TextBox textBoxEventReferenceDetails;
        private System.Windows.Forms.PictureBox pictureBoxWithholdingReasonEvent;
        private System.Windows.Forms.Label labelEventEnd;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelEventEnd;
        private System.Windows.Forms.Button buttonWithholdingReasonCollectionDate;
        private System.Windows.Forms.SplitContainer splitContainerOverviewEventDescriptions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLocalityDescription;
        private System.Windows.Forms.Label labelLocalityDescriptionVerbatim;
        private System.Windows.Forms.Label labelLocalityDescription;
        private System.Windows.Forms.TextBox textBoxEventLocality;
        private System.Windows.Forms.TextBox textBoxLocalityDescriptionVerbatim;
        private System.Windows.Forms.TextBox textBoxHabitatDesciption;
        private System.Windows.Forms.Label labelHabitatDescription;
    }
}
