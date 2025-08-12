namespace DiversityCollection.UserControls
{
    partial class UserControl_Relation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Relation));
            this.splitContainerOverviewRelation = new System.Windows.Forms.SplitContainer();
            this.groupBoxSpecimenRelation = new System.Windows.Forms.GroupBox();
            this.pictureBoxSpecimenRelation = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelRelation = new System.Windows.Forms.TableLayoutPanel();
            this.labelRelatedSpecimenDescription = new System.Windows.Forms.Label();
            this.textBoxRelatedSpecimenDescription = new System.Windows.Forms.TextBox();
            this.labelRelatedSpecimenCollectionID = new System.Windows.Forms.Label();
            this.labelRelationNotes = new System.Windows.Forms.Label();
            this.textBoxRelationNotes = new System.Windows.Forms.TextBox();
            this.labelRelatedSpecimen = new System.Windows.Forms.Label();
            this.textBoxRelatedSpecimenURL = new System.Windows.Forms.TextBox();
            this.buttonRelatedSpecimenURL = new System.Windows.Forms.Button();
            this.comboBoxRelatedSpecimenCollectionID = new System.Windows.Forms.ComboBox();
            this.labelRelatedSpecimenRelationType = new System.Windows.Forms.Label();
            this.comboBoxRelatedSpecimenRelationType = new System.Windows.Forms.ComboBox();
            this.buttonSaveUri = new System.Windows.Forms.Button();
            this.groupBoxSpecimenRelationInvers = new System.Windows.Forms.GroupBox();
            this.pictureBoxSpecimenRelationInvers = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelRelatedSpecimenInvers = new System.Windows.Forms.TableLayoutPanel();
            this.labelRelatedSpecimenInversDescripition = new System.Windows.Forms.Label();
            this.textBoxRelatedSpecimenInversDescripition = new System.Windows.Forms.TextBox();
            this.labelRelatedSpecimenInversCollection = new System.Windows.Forms.Label();
            this.labelRelatedSpecimenInversNotes = new System.Windows.Forms.Label();
            this.textBoxRelatedSpecimenInversNotes = new System.Windows.Forms.TextBox();
            this.labelRelatedSpecimenDisplayText = new System.Windows.Forms.Label();
            this.textBoxRelatedSpecimenInversDisplayText = new System.Windows.Forms.TextBox();
            this.labelRelatedSpecimenInversRelationType = new System.Windows.Forms.Label();
            this.buttonChangeToRelatedSpecimen = new System.Windows.Forms.Button();
            this.textBoxRelatedSpecimenInversCollection = new System.Windows.Forms.TextBox();
            this.textBoxRelatedSpecimenInversRelationType = new System.Windows.Forms.TextBox();
            this.userControlModuleRelatedEntryRelatedSpecimen = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlHierarchySelectorRelatedSpecimenCollectionID = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewRelation)).BeginInit();
            this.splitContainerOverviewRelation.Panel1.SuspendLayout();
            this.splitContainerOverviewRelation.Panel2.SuspendLayout();
            this.splitContainerOverviewRelation.SuspendLayout();
            this.groupBoxSpecimenRelation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimenRelation)).BeginInit();
            this.tableLayoutPanelRelation.SuspendLayout();
            this.groupBoxSpecimenRelationInvers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimenRelationInvers)).BeginInit();
            this.tableLayoutPanelRelatedSpecimenInvers.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // splitContainerOverviewRelation
            // 
            this.splitContainerOverviewRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOverviewRelation.Location = new System.Drawing.Point(0, 0);
            this.splitContainerOverviewRelation.Name = "splitContainerOverviewRelation";
            // 
            // splitContainerOverviewRelation.Panel1
            // 
            this.splitContainerOverviewRelation.Panel1.Controls.Add(this.groupBoxSpecimenRelation);
            // 
            // splitContainerOverviewRelation.Panel2
            // 
            this.splitContainerOverviewRelation.Panel2.Controls.Add(this.groupBoxSpecimenRelationInvers);
            this.splitContainerOverviewRelation.Size = new System.Drawing.Size(1004, 375);
            this.splitContainerOverviewRelation.SplitterDistance = 576;
            this.splitContainerOverviewRelation.TabIndex = 33;
            // 
            // groupBoxSpecimenRelation
            // 
            this.groupBoxSpecimenRelation.AccessibleName = "CollectionSpecimenRelation";
            this.groupBoxSpecimenRelation.Controls.Add(this.pictureBoxSpecimenRelation);
            this.groupBoxSpecimenRelation.Controls.Add(this.tableLayoutPanelRelation);
            this.groupBoxSpecimenRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSpecimenRelation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSpecimenRelation.ForeColor = System.Drawing.Color.DarkMagenta;
            this.groupBoxSpecimenRelation.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSpecimenRelation.MinimumSize = new System.Drawing.Size(0, 166);
            this.groupBoxSpecimenRelation.Name = "groupBoxSpecimenRelation";
            this.groupBoxSpecimenRelation.Padding = new System.Windows.Forms.Padding(3, 16, 3, 3);
            this.groupBoxSpecimenRelation.Size = new System.Drawing.Size(576, 375);
            this.groupBoxSpecimenRelation.TabIndex = 31;
            this.groupBoxSpecimenRelation.TabStop = false;
            this.groupBoxSpecimenRelation.Text = "Relation to another specimen";
            // 
            // pictureBoxSpecimenRelation
            // 
            this.pictureBoxSpecimenRelation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxSpecimenRelation.Image = global::DiversityCollection.Resource.Relation;
            this.pictureBoxSpecimenRelation.Location = new System.Drawing.Point(559, 15);
            this.pictureBoxSpecimenRelation.Name = "pictureBoxSpecimenRelation";
            this.pictureBoxSpecimenRelation.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSpecimenRelation.TabIndex = 31;
            this.pictureBoxSpecimenRelation.TabStop = false;
            // 
            // tableLayoutPanelRelation
            // 
            this.tableLayoutPanelRelation.ColumnCount = 4;
            this.tableLayoutPanelRelation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelRelation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRelation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRelation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRelation.Controls.Add(this.labelRelatedSpecimenDescription, 1, 3);
            this.tableLayoutPanelRelation.Controls.Add(this.textBoxRelatedSpecimenDescription, 1, 4);
            this.tableLayoutPanelRelation.Controls.Add(this.labelRelatedSpecimenCollectionID, 2, 5);
            this.tableLayoutPanelRelation.Controls.Add(this.labelRelationNotes, 1, 7);
            this.tableLayoutPanelRelation.Controls.Add(this.textBoxRelationNotes, 1, 8);
            this.tableLayoutPanelRelation.Controls.Add(this.userControlModuleRelatedEntryRelatedSpecimen, 1, 2);
            this.tableLayoutPanelRelation.Controls.Add(this.labelRelatedSpecimen, 1, 0);
            this.tableLayoutPanelRelation.Controls.Add(this.textBoxRelatedSpecimenURL, 1, 1);
            this.tableLayoutPanelRelation.Controls.Add(this.buttonRelatedSpecimenURL, 3, 1);
            this.tableLayoutPanelRelation.Controls.Add(this.comboBoxRelatedSpecimenCollectionID, 2, 6);
            this.tableLayoutPanelRelation.Controls.Add(this.labelRelatedSpecimenRelationType, 1, 5);
            this.tableLayoutPanelRelation.Controls.Add(this.comboBoxRelatedSpecimenRelationType, 1, 6);
            this.tableLayoutPanelRelation.Controls.Add(this.userControlHierarchySelectorRelatedSpecimenCollectionID, 3, 6);
            this.tableLayoutPanelRelation.Controls.Add(this.buttonSaveUri, 0, 1);
            this.tableLayoutPanelRelation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRelation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelRelation.Location = new System.Drawing.Point(3, 29);
            this.tableLayoutPanelRelation.Name = "tableLayoutPanelRelation";
            this.tableLayoutPanelRelation.RowCount = 9;
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRelation.Size = new System.Drawing.Size(570, 343);
            this.tableLayoutPanelRelation.TabIndex = 30;
            // 
            // labelRelatedSpecimenDescription
            // 
            this.labelRelatedSpecimenDescription.AccessibleName = "CollectionSpecimenRelation.RelatedSpecimenDescription";
            this.labelRelatedSpecimenDescription.AutoSize = true;
            this.tableLayoutPanelRelation.SetColumnSpan(this.labelRelatedSpecimenDescription, 3);
            this.labelRelatedSpecimenDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimenDescription.Location = new System.Drawing.Point(3, 60);
            this.labelRelatedSpecimenDescription.Name = "labelRelatedSpecimenDescription";
            this.labelRelatedSpecimenDescription.Size = new System.Drawing.Size(564, 13);
            this.labelRelatedSpecimenDescription.TabIndex = 4;
            this.labelRelatedSpecimenDescription.Text = "Description:";
            this.labelRelatedSpecimenDescription.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxRelatedSpecimenDescription
            // 
            this.tableLayoutPanelRelation.SetColumnSpan(this.textBoxRelatedSpecimenDescription, 3);
            this.textBoxRelatedSpecimenDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRelatedSpecimenDescription.Location = new System.Drawing.Point(3, 73);
            this.textBoxRelatedSpecimenDescription.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxRelatedSpecimenDescription.Multiline = true;
            this.textBoxRelatedSpecimenDescription.Name = "textBoxRelatedSpecimenDescription";
            this.textBoxRelatedSpecimenDescription.Size = new System.Drawing.Size(564, 107);
            this.textBoxRelatedSpecimenDescription.TabIndex = 19;
            // 
            // labelRelatedSpecimenCollectionID
            // 
            this.labelRelatedSpecimenCollectionID.AccessibleName = "CollectionSpecimenRelation.RelatedSpecimenCollectionID";
            this.labelRelatedSpecimenCollectionID.AutoSize = true;
            this.tableLayoutPanelRelation.SetColumnSpan(this.labelRelatedSpecimenCollectionID, 2);
            this.labelRelatedSpecimenCollectionID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimenCollectionID.Location = new System.Drawing.Point(278, 183);
            this.labelRelatedSpecimenCollectionID.Name = "labelRelatedSpecimenCollectionID";
            this.labelRelatedSpecimenCollectionID.Size = new System.Drawing.Size(289, 13);
            this.labelRelatedSpecimenCollectionID.TabIndex = 8;
            this.labelRelatedSpecimenCollectionID.Text = "Collection:";
            this.labelRelatedSpecimenCollectionID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelRelationNotes
            // 
            this.labelRelationNotes.AccessibleName = "CollectionSpecimenRelation.Notes";
            this.labelRelationNotes.AutoSize = true;
            this.tableLayoutPanelRelation.SetColumnSpan(this.labelRelationNotes, 3);
            this.labelRelationNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelationNotes.Location = new System.Drawing.Point(3, 220);
            this.labelRelationNotes.Name = "labelRelationNotes";
            this.labelRelationNotes.Size = new System.Drawing.Size(564, 13);
            this.labelRelationNotes.TabIndex = 9;
            this.labelRelationNotes.Text = "Notes:";
            this.labelRelationNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxRelationNotes
            // 
            this.tableLayoutPanelRelation.SetColumnSpan(this.textBoxRelationNotes, 3);
            this.textBoxRelationNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRelationNotes.Location = new System.Drawing.Point(3, 233);
            this.textBoxRelationNotes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxRelationNotes.Multiline = true;
            this.textBoxRelationNotes.Name = "textBoxRelationNotes";
            this.textBoxRelationNotes.Size = new System.Drawing.Size(564, 107);
            this.textBoxRelationNotes.TabIndex = 21;
            // 
            // labelRelatedSpecimen
            // 
            this.labelRelatedSpecimen.AccessibleName = "CollectionSpecimenRelation.RelatedSpecimenURI";
            this.labelRelatedSpecimen.AutoSize = true;
            this.tableLayoutPanelRelation.SetColumnSpan(this.labelRelatedSpecimen, 3);
            this.labelRelatedSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimen.Location = new System.Drawing.Point(3, 0);
            this.labelRelatedSpecimen.Name = "labelRelatedSpecimen";
            this.labelRelatedSpecimen.Size = new System.Drawing.Size(564, 13);
            this.labelRelatedSpecimen.TabIndex = 13;
            this.labelRelatedSpecimen.Text = "Identifier or URL of related specimen:";
            this.labelRelatedSpecimen.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxRelatedSpecimenURL
            // 
            this.tableLayoutPanelRelation.SetColumnSpan(this.textBoxRelatedSpecimenURL, 2);
            this.textBoxRelatedSpecimenURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRelatedSpecimenURL.Location = new System.Drawing.Point(3, 15);
            this.textBoxRelatedSpecimenURL.Margin = new System.Windows.Forms.Padding(3, 2, 0, 3);
            this.textBoxRelatedSpecimenURL.Name = "textBoxRelatedSpecimenURL";
            this.textBoxRelatedSpecimenURL.Size = new System.Drawing.Size(547, 20);
            this.textBoxRelatedSpecimenURL.TabIndex = 14;
            this.textBoxRelatedSpecimenURL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxRelatedSpecimenURL_KeyUp);
            // 
            // buttonRelatedSpecimenURL
            // 
            this.buttonRelatedSpecimenURL.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRelatedSpecimenURL.FlatAppearance.BorderSize = 0;
            this.buttonRelatedSpecimenURL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRelatedSpecimenURL.Image = global::DiversityCollection.Resource.Browse;
            this.buttonRelatedSpecimenURL.Location = new System.Drawing.Point(550, 13);
            this.buttonRelatedSpecimenURL.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRelatedSpecimenURL.Name = "buttonRelatedSpecimenURL";
            this.buttonRelatedSpecimenURL.Size = new System.Drawing.Size(20, 24);
            this.buttonRelatedSpecimenURL.TabIndex = 15;
            this.buttonRelatedSpecimenURL.UseVisualStyleBackColor = true;
            this.buttonRelatedSpecimenURL.Click += new System.EventHandler(this.buttonRelatedSpecimenURL_Click);
            // 
            // comboBoxRelatedSpecimenCollectionID
            // 
            this.comboBoxRelatedSpecimenCollectionID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxRelatedSpecimenCollectionID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRelatedSpecimenCollectionID.FormattingEnabled = true;
            this.comboBoxRelatedSpecimenCollectionID.Location = new System.Drawing.Point(278, 196);
            this.comboBoxRelatedSpecimenCollectionID.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
            this.comboBoxRelatedSpecimenCollectionID.Name = "comboBoxRelatedSpecimenCollectionID";
            this.comboBoxRelatedSpecimenCollectionID.Size = new System.Drawing.Size(272, 21);
            this.comboBoxRelatedSpecimenCollectionID.TabIndex = 22;
            // 
            // labelRelatedSpecimenRelationType
            // 
            this.labelRelatedSpecimenRelationType.AccessibleName = "CollectionSpecimenRelation.RelationType";
            this.labelRelatedSpecimenRelationType.AutoSize = true;
            this.labelRelatedSpecimenRelationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimenRelationType.Location = new System.Drawing.Point(3, 183);
            this.labelRelatedSpecimenRelationType.Name = "labelRelatedSpecimenRelationType";
            this.labelRelatedSpecimenRelationType.Size = new System.Drawing.Size(269, 13);
            this.labelRelatedSpecimenRelationType.TabIndex = 23;
            this.labelRelatedSpecimenRelationType.Text = "Relation type:";
            this.labelRelatedSpecimenRelationType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxRelatedSpecimenRelationType
            // 
            this.comboBoxRelatedSpecimenRelationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxRelatedSpecimenRelationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRelatedSpecimenRelationType.FormattingEnabled = true;
            this.comboBoxRelatedSpecimenRelationType.Location = new System.Drawing.Point(3, 196);
            this.comboBoxRelatedSpecimenRelationType.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.comboBoxRelatedSpecimenRelationType.Name = "comboBoxRelatedSpecimenRelationType";
            this.comboBoxRelatedSpecimenRelationType.Size = new System.Drawing.Size(269, 21);
            this.comboBoxRelatedSpecimenRelationType.TabIndex = 24;
            // 
            // buttonSaveUri
            // 
            this.buttonSaveUri.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveUri.Enabled = false;
            this.buttonSaveUri.FlatAppearance.BorderSize = 0;
            this.buttonSaveUri.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveUri.Location = new System.Drawing.Point(0, 13);
            this.buttonSaveUri.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSaveUri.Name = "buttonSaveUri";
            this.buttonSaveUri.Size = new System.Drawing.Size(1, 25);
            this.buttonSaveUri.TabIndex = 26;
            this.buttonSaveUri.UseVisualStyleBackColor = true;
            this.buttonSaveUri.Visible = false;
            this.buttonSaveUri.Click += new System.EventHandler(this.buttonSaveUri_Click);
            // 
            // groupBoxSpecimenRelationInvers
            // 
            this.groupBoxSpecimenRelationInvers.Controls.Add(this.pictureBoxSpecimenRelationInvers);
            this.groupBoxSpecimenRelationInvers.Controls.Add(this.tableLayoutPanelRelatedSpecimenInvers);
            this.groupBoxSpecimenRelationInvers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSpecimenRelationInvers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSpecimenRelationInvers.ForeColor = System.Drawing.Color.Gray;
            this.groupBoxSpecimenRelationInvers.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSpecimenRelationInvers.MinimumSize = new System.Drawing.Size(0, 166);
            this.groupBoxSpecimenRelationInvers.Name = "groupBoxSpecimenRelationInvers";
            this.groupBoxSpecimenRelationInvers.Size = new System.Drawing.Size(424, 375);
            this.groupBoxSpecimenRelationInvers.TabIndex = 32;
            this.groupBoxSpecimenRelationInvers.TabStop = false;
            this.groupBoxSpecimenRelationInvers.Text = "Relation from other specimen";
            // 
            // pictureBoxSpecimenRelationInvers
            // 
            this.pictureBoxSpecimenRelationInvers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxSpecimenRelationInvers.Image = global::DiversityCollection.Resource.RelationInversGrey;
            this.pictureBoxSpecimenRelationInvers.Location = new System.Drawing.Point(402, 1);
            this.pictureBoxSpecimenRelationInvers.Name = "pictureBoxSpecimenRelationInvers";
            this.pictureBoxSpecimenRelationInvers.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSpecimenRelationInvers.TabIndex = 31;
            this.pictureBoxSpecimenRelationInvers.TabStop = false;
            // 
            // tableLayoutPanelRelatedSpecimenInvers
            // 
            this.tableLayoutPanelRelatedSpecimenInvers.ColumnCount = 3;
            this.tableLayoutPanelRelatedSpecimenInvers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRelatedSpecimenInvers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRelatedSpecimenInvers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.labelRelatedSpecimenInversDescripition, 0, 2);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.textBoxRelatedSpecimenInversDescripition, 0, 3);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.labelRelatedSpecimenInversCollection, 0, 4);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.labelRelatedSpecimenInversNotes, 0, 6);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.textBoxRelatedSpecimenInversNotes, 0, 7);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.labelRelatedSpecimenDisplayText, 0, 0);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.textBoxRelatedSpecimenInversDisplayText, 0, 1);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.labelRelatedSpecimenInversRelationType, 1, 4);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.buttonChangeToRelatedSpecimen, 2, 1);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.textBoxRelatedSpecimenInversCollection, 0, 5);
            this.tableLayoutPanelRelatedSpecimenInvers.Controls.Add(this.textBoxRelatedSpecimenInversRelationType, 1, 5);
            this.tableLayoutPanelRelatedSpecimenInvers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRelatedSpecimenInvers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelRelatedSpecimenInvers.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelRelatedSpecimenInvers.Name = "tableLayoutPanelRelatedSpecimenInvers";
            this.tableLayoutPanelRelatedSpecimenInvers.RowCount = 8;
            this.tableLayoutPanelRelatedSpecimenInvers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelatedSpecimenInvers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelatedSpecimenInvers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelatedSpecimenInvers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRelatedSpecimenInvers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelatedSpecimenInvers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelatedSpecimenInvers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRelatedSpecimenInvers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRelatedSpecimenInvers.Size = new System.Drawing.Size(418, 356);
            this.tableLayoutPanelRelatedSpecimenInvers.TabIndex = 30;
            // 
            // labelRelatedSpecimenInversDescripition
            // 
            this.labelRelatedSpecimenInversDescripition.AccessibleName = "CollectionSpecimenRelation.RelatedSpecimenDescription";
            this.labelRelatedSpecimenInversDescripition.AutoSize = true;
            this.labelRelatedSpecimenInversDescripition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimenInversDescripition.Location = new System.Drawing.Point(3, 38);
            this.labelRelatedSpecimenInversDescripition.Name = "labelRelatedSpecimenInversDescripition";
            this.labelRelatedSpecimenInversDescripition.Size = new System.Drawing.Size(193, 13);
            this.labelRelatedSpecimenInversDescripition.TabIndex = 4;
            this.labelRelatedSpecimenInversDescripition.Text = "Description:";
            this.labelRelatedSpecimenInversDescripition.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxRelatedSpecimenInversDescripition
            // 
            this.textBoxRelatedSpecimenInversDescripition.AcceptsReturn = true;
            this.tableLayoutPanelRelatedSpecimenInvers.SetColumnSpan(this.textBoxRelatedSpecimenInversDescripition, 3);
            this.textBoxRelatedSpecimenInversDescripition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRelatedSpecimenInversDescripition.Location = new System.Drawing.Point(3, 51);
            this.textBoxRelatedSpecimenInversDescripition.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxRelatedSpecimenInversDescripition.Multiline = true;
            this.textBoxRelatedSpecimenInversDescripition.Name = "textBoxRelatedSpecimenInversDescripition";
            this.textBoxRelatedSpecimenInversDescripition.ReadOnly = true;
            this.textBoxRelatedSpecimenInversDescripition.Size = new System.Drawing.Size(412, 125);
            this.textBoxRelatedSpecimenInversDescripition.TabIndex = 19;
            // 
            // labelRelatedSpecimenInversCollection
            // 
            this.labelRelatedSpecimenInversCollection.AccessibleName = "CollectionSpecimenRelation.RelatedSpecimenCollectionID";
            this.labelRelatedSpecimenInversCollection.AutoSize = true;
            this.labelRelatedSpecimenInversCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimenInversCollection.Location = new System.Drawing.Point(3, 179);
            this.labelRelatedSpecimenInversCollection.Name = "labelRelatedSpecimenInversCollection";
            this.labelRelatedSpecimenInversCollection.Size = new System.Drawing.Size(193, 13);
            this.labelRelatedSpecimenInversCollection.TabIndex = 8;
            this.labelRelatedSpecimenInversCollection.Text = "Collection:";
            this.labelRelatedSpecimenInversCollection.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelRelatedSpecimenInversNotes
            // 
            this.labelRelatedSpecimenInversNotes.AccessibleName = "CollectionSpecimenRelation.Notes";
            this.labelRelatedSpecimenInversNotes.AutoSize = true;
            this.labelRelatedSpecimenInversNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimenInversNotes.Location = new System.Drawing.Point(3, 215);
            this.labelRelatedSpecimenInversNotes.Name = "labelRelatedSpecimenInversNotes";
            this.labelRelatedSpecimenInversNotes.Size = new System.Drawing.Size(193, 13);
            this.labelRelatedSpecimenInversNotes.TabIndex = 9;
            this.labelRelatedSpecimenInversNotes.Text = "Notes:";
            this.labelRelatedSpecimenInversNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxRelatedSpecimenInversNotes
            // 
            this.tableLayoutPanelRelatedSpecimenInvers.SetColumnSpan(this.textBoxRelatedSpecimenInversNotes, 3);
            this.textBoxRelatedSpecimenInversNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRelatedSpecimenInversNotes.Location = new System.Drawing.Point(3, 228);
            this.textBoxRelatedSpecimenInversNotes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxRelatedSpecimenInversNotes.Multiline = true;
            this.textBoxRelatedSpecimenInversNotes.Name = "textBoxRelatedSpecimenInversNotes";
            this.textBoxRelatedSpecimenInversNotes.ReadOnly = true;
            this.textBoxRelatedSpecimenInversNotes.Size = new System.Drawing.Size(412, 125);
            this.textBoxRelatedSpecimenInversNotes.TabIndex = 21;
            // 
            // labelRelatedSpecimenDisplayText
            // 
            this.labelRelatedSpecimenDisplayText.AccessibleName = "CollectionSpecimenRelation.RelatedSpecimenDisplayText";
            this.labelRelatedSpecimenDisplayText.AutoSize = true;
            this.labelRelatedSpecimenDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimenDisplayText.Location = new System.Drawing.Point(3, 0);
            this.labelRelatedSpecimenDisplayText.Name = "labelRelatedSpecimenDisplayText";
            this.labelRelatedSpecimenDisplayText.Size = new System.Drawing.Size(193, 13);
            this.labelRelatedSpecimenDisplayText.TabIndex = 13;
            this.labelRelatedSpecimenDisplayText.Text = "Specimen:";
            this.labelRelatedSpecimenDisplayText.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxRelatedSpecimenInversDisplayText
            // 
            this.tableLayoutPanelRelatedSpecimenInvers.SetColumnSpan(this.textBoxRelatedSpecimenInversDisplayText, 2);
            this.textBoxRelatedSpecimenInversDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRelatedSpecimenInversDisplayText.Location = new System.Drawing.Point(3, 14);
            this.textBoxRelatedSpecimenInversDisplayText.Margin = new System.Windows.Forms.Padding(3, 1, 0, 3);
            this.textBoxRelatedSpecimenInversDisplayText.Name = "textBoxRelatedSpecimenInversDisplayText";
            this.textBoxRelatedSpecimenInversDisplayText.ReadOnly = true;
            this.textBoxRelatedSpecimenInversDisplayText.Size = new System.Drawing.Size(395, 20);
            this.textBoxRelatedSpecimenInversDisplayText.TabIndex = 14;
            // 
            // labelRelatedSpecimenInversRelationType
            // 
            this.labelRelatedSpecimenInversRelationType.AccessibleName = "CollectionSpecimenRelation.RelationType";
            this.labelRelatedSpecimenInversRelationType.AutoSize = true;
            this.labelRelatedSpecimenInversRelationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRelatedSpecimenInversRelationType.Location = new System.Drawing.Point(202, 179);
            this.labelRelatedSpecimenInversRelationType.Name = "labelRelatedSpecimenInversRelationType";
            this.labelRelatedSpecimenInversRelationType.Size = new System.Drawing.Size(193, 13);
            this.labelRelatedSpecimenInversRelationType.TabIndex = 23;
            this.labelRelatedSpecimenInversRelationType.Text = "Relation type:";
            this.labelRelatedSpecimenInversRelationType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // buttonChangeToRelatedSpecimen
            // 
            this.buttonChangeToRelatedSpecimen.FlatAppearance.BorderSize = 0;
            this.buttonChangeToRelatedSpecimen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChangeToRelatedSpecimen.Image = global::DiversityCollection.Resource.Search;
            this.buttonChangeToRelatedSpecimen.Location = new System.Drawing.Point(398, 13);
            this.buttonChangeToRelatedSpecimen.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.buttonChangeToRelatedSpecimen.Name = "buttonChangeToRelatedSpecimen";
            this.buttonChangeToRelatedSpecimen.Size = new System.Drawing.Size(17, 22);
            this.buttonChangeToRelatedSpecimen.TabIndex = 25;
            this.buttonChangeToRelatedSpecimen.UseVisualStyleBackColor = true;
            this.buttonChangeToRelatedSpecimen.Click += new System.EventHandler(this.buttonChangeToRelatedSpecimen_Click);
            // 
            // textBoxRelatedSpecimenInversCollection
            // 
            this.textBoxRelatedSpecimenInversCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRelatedSpecimenInversCollection.Location = new System.Drawing.Point(3, 192);
            this.textBoxRelatedSpecimenInversCollection.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxRelatedSpecimenInversCollection.Name = "textBoxRelatedSpecimenInversCollection";
            this.textBoxRelatedSpecimenInversCollection.ReadOnly = true;
            this.textBoxRelatedSpecimenInversCollection.Size = new System.Drawing.Size(193, 20);
            this.textBoxRelatedSpecimenInversCollection.TabIndex = 26;
            // 
            // textBoxRelatedSpecimenInversRelationType
            // 
            this.tableLayoutPanelRelatedSpecimenInvers.SetColumnSpan(this.textBoxRelatedSpecimenInversRelationType, 2);
            this.textBoxRelatedSpecimenInversRelationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRelatedSpecimenInversRelationType.Location = new System.Drawing.Point(202, 192);
            this.textBoxRelatedSpecimenInversRelationType.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxRelatedSpecimenInversRelationType.Name = "textBoxRelatedSpecimenInversRelationType";
            this.textBoxRelatedSpecimenInversRelationType.ReadOnly = true;
            this.textBoxRelatedSpecimenInversRelationType.Size = new System.Drawing.Size(213, 20);
            this.textBoxRelatedSpecimenInversRelationType.TabIndex = 27;
            // 
            // userControlModuleRelatedEntryRelatedSpecimen
            // 
            this.userControlModuleRelatedEntryRelatedSpecimen.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelRelation.SetColumnSpan(this.userControlModuleRelatedEntryRelatedSpecimen, 3);
            this.userControlModuleRelatedEntryRelatedSpecimen.DependsOnUri = "";
            this.userControlModuleRelatedEntryRelatedSpecimen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryRelatedSpecimen.Domain = "";
            this.userControlModuleRelatedEntryRelatedSpecimen.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryRelatedSpecimen.Location = new System.Drawing.Point(3, 38);
            this.userControlModuleRelatedEntryRelatedSpecimen.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.userControlModuleRelatedEntryRelatedSpecimen.Module = null;
            this.userControlModuleRelatedEntryRelatedSpecimen.Name = "userControlModuleRelatedEntryRelatedSpecimen";
            this.userControlModuleRelatedEntryRelatedSpecimen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryRelatedSpecimen.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryRelatedSpecimen.ShowInfo = false;
            this.userControlModuleRelatedEntryRelatedSpecimen.Size = new System.Drawing.Size(564, 22);
            this.userControlModuleRelatedEntryRelatedSpecimen.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryRelatedSpecimen.TabIndex = 16;
            this.userControlModuleRelatedEntryRelatedSpecimen.Visible = false;
            // 
            // userControlHierarchySelectorRelatedSpecimenCollectionID
            // 
            this.userControlHierarchySelectorRelatedSpecimenCollectionID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlHierarchySelectorRelatedSpecimenCollectionID.Location = new System.Drawing.Point(550, 196);
            this.userControlHierarchySelectorRelatedSpecimenCollectionID.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.userControlHierarchySelectorRelatedSpecimenCollectionID.Name = "userControlHierarchySelectorRelatedSpecimenCollectionID";
            this.userControlHierarchySelectorRelatedSpecimenCollectionID.Size = new System.Drawing.Size(17, 21);
            this.userControlHierarchySelectorRelatedSpecimenCollectionID.TabIndex = 25;
            // 
            // UserControl_Relation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerOverviewRelation);
            this.Name = "UserControl_Relation";
            this.Size = new System.Drawing.Size(1004, 375);
            this.splitContainerOverviewRelation.Panel1.ResumeLayout(false);
            this.splitContainerOverviewRelation.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOverviewRelation)).EndInit();
            this.splitContainerOverviewRelation.ResumeLayout(false);
            this.groupBoxSpecimenRelation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimenRelation)).EndInit();
            this.tableLayoutPanelRelation.ResumeLayout(false);
            this.tableLayoutPanelRelation.PerformLayout();
            this.groupBoxSpecimenRelationInvers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpecimenRelationInvers)).EndInit();
            this.tableLayoutPanelRelatedSpecimenInvers.ResumeLayout(false);
            this.tableLayoutPanelRelatedSpecimenInvers.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerOverviewRelation;
        private System.Windows.Forms.GroupBox groupBoxSpecimenRelation;
        private System.Windows.Forms.PictureBox pictureBoxSpecimenRelation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRelation;
        private System.Windows.Forms.Label labelRelatedSpecimenDescription;
        private System.Windows.Forms.TextBox textBoxRelatedSpecimenDescription;
        private System.Windows.Forms.Label labelRelatedSpecimenCollectionID;
        private System.Windows.Forms.Label labelRelationNotes;
        private System.Windows.Forms.TextBox textBoxRelationNotes;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryRelatedSpecimen;
        private System.Windows.Forms.Label labelRelatedSpecimen;
        private System.Windows.Forms.TextBox textBoxRelatedSpecimenURL;
        private System.Windows.Forms.Button buttonRelatedSpecimenURL;
        private System.Windows.Forms.ComboBox comboBoxRelatedSpecimenCollectionID;
        private System.Windows.Forms.Label labelRelatedSpecimenRelationType;
        private System.Windows.Forms.ComboBox comboBoxRelatedSpecimenRelationType;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorRelatedSpecimenCollectionID;
        private System.Windows.Forms.GroupBox groupBoxSpecimenRelationInvers;
        private System.Windows.Forms.PictureBox pictureBoxSpecimenRelationInvers;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRelatedSpecimenInvers;
        private System.Windows.Forms.Label labelRelatedSpecimenInversDescripition;
        private System.Windows.Forms.TextBox textBoxRelatedSpecimenInversDescripition;
        private System.Windows.Forms.Label labelRelatedSpecimenInversCollection;
        private System.Windows.Forms.Label labelRelatedSpecimenInversNotes;
        private System.Windows.Forms.TextBox textBoxRelatedSpecimenInversNotes;
        private System.Windows.Forms.Label labelRelatedSpecimenDisplayText;
        private System.Windows.Forms.TextBox textBoxRelatedSpecimenInversDisplayText;
        private System.Windows.Forms.Label labelRelatedSpecimenInversRelationType;
        private System.Windows.Forms.Button buttonChangeToRelatedSpecimen;
        private System.Windows.Forms.TextBox textBoxRelatedSpecimenInversCollection;
        private System.Windows.Forms.TextBox textBoxRelatedSpecimenInversRelationType;
        private System.Windows.Forms.Button buttonSaveUri;
    }
}
