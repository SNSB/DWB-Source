namespace DiversityCollection.UserControls
{
    partial class UserControl_Identification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Identification));
            groupBoxOverviewIdentification = new System.Windows.Forms.GroupBox();
            pictureBoxIdentification = new System.Windows.Forms.PictureBox();
            tableLayoutPanelIdentification = new System.Windows.Forms.TableLayoutPanel();
            labelTaxonomicName = new System.Windows.Forms.Label();
            labelVernacularTerm = new System.Windows.Forms.Label();
            labelTypeNotes = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryTaxonomicName = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            comboBoxIdentificationQualifier = new System.Windows.Forms.ComboBox();
            comboBoxIdentificationCategory = new System.Windows.Forms.ComboBox();
            comboBoxTypeStatus = new System.Windows.Forms.ComboBox();
            comboBoxIdentificationDateCategory = new System.Windows.Forms.ComboBox();
            labelIdentificationQualifier = new System.Windows.Forms.Label();
            labelIdentificationCategory = new System.Windows.Forms.Label();
            labelTypeStatus = new System.Windows.Forms.Label();
            labelIdentificationDateCategory = new System.Windows.Forms.Label();
            labelIdentificationDate = new System.Windows.Forms.Label();
            labelIdentificationResponsible = new System.Windows.Forms.Label();
            labelIdentificationReference = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryIdentificationReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            textBoxIdentificationNotes = new System.Windows.Forms.TextBox();
            labelIdentificationNotes = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryIdentificationResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            userControlDatePanelIdentificationDate = new DiversityWorkbench.UserControls.UserControlDatePanel();
            textBoxTypeNotes = new System.Windows.Forms.TextBox();
            comboBoxVernacularTerm = new System.Windows.Forms.ComboBox();
            userControlHierarchySelectorIdentificationQualifier = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            userControlHierarchySelectorIdentificationCategory = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            userControlHierarchySelectorTypeStatus = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            userControlHierarchySelectorIdentificationDateCategory = new DiversityWorkbench.UserControls.UserControlHierarchySelector();
            buttonCollectorIsResponsible = new System.Windows.Forms.Button();
            labelIdentificationReferenceDetails = new System.Windows.Forms.Label();
            textBoxIdentificationReferenceDetails = new System.Windows.Forms.TextBox();
            panelTemplateIdentification = new System.Windows.Forms.Panel();
            buttonTemplateIdentificationEdit = new System.Windows.Forms.Button();
            buttonTemplateIdentificationSet = new System.Windows.Forms.Button();
            labelIdentificationScientificTerm = new System.Windows.Forms.Label();
            userControlModuleRelatedEntryIdentificationScientificTerm = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            groupBoxOverviewIdentification.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxIdentification).BeginInit();
            tableLayoutPanelIdentification.SuspendLayout();
            panelTemplateIdentification.SuspendLayout();
            SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            imageListDataWithholding.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageListDataWithholding.ImageStream");
            imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxOverviewIdentification
            // 
            groupBoxOverviewIdentification.AccessibleName = "Identification";
            groupBoxOverviewIdentification.BackColor = System.Drawing.SystemColors.Control;
            groupBoxOverviewIdentification.Controls.Add(pictureBoxIdentification);
            groupBoxOverviewIdentification.Controls.Add(tableLayoutPanelIdentification);
            groupBoxOverviewIdentification.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxOverviewIdentification.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            groupBoxOverviewIdentification.ForeColor = System.Drawing.Color.Black;
            groupBoxOverviewIdentification.Location = new System.Drawing.Point(0, 0);
            groupBoxOverviewIdentification.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            groupBoxOverviewIdentification.Name = "groupBoxOverviewIdentification";
            groupBoxOverviewIdentification.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            groupBoxOverviewIdentification.Size = new System.Drawing.Size(849, 297);
            groupBoxOverviewIdentification.TabIndex = 2;
            groupBoxOverviewIdentification.TabStop = false;
            groupBoxOverviewIdentification.Text = "Identification / Name changes";
            // 
            // pictureBoxIdentification
            // 
            pictureBoxIdentification.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            pictureBoxIdentification.Image = Resource.Identification;
            pictureBoxIdentification.Location = new System.Drawing.Point(824, 1);
            pictureBoxIdentification.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pictureBoxIdentification.Name = "pictureBoxIdentification";
            pictureBoxIdentification.Size = new System.Drawing.Size(22, 24);
            pictureBoxIdentification.TabIndex = 1;
            pictureBoxIdentification.TabStop = false;
            // 
            // tableLayoutPanelIdentification
            // 
            tableLayoutPanelIdentification.ColumnCount = 7;
            tableLayoutPanelIdentification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            tableLayoutPanelIdentification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            tableLayoutPanelIdentification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanelIdentification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableLayoutPanelIdentification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            tableLayoutPanelIdentification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            tableLayoutPanelIdentification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            tableLayoutPanelIdentification.Controls.Add(labelTaxonomicName, 1, 0);
            tableLayoutPanelIdentification.Controls.Add(labelVernacularTerm, 1, 1);
            tableLayoutPanelIdentification.Controls.Add(labelTypeNotes, 1, 4);
            tableLayoutPanelIdentification.Controls.Add(userControlModuleRelatedEntryTaxonomicName, 2, 0);
            tableLayoutPanelIdentification.Controls.Add(comboBoxIdentificationQualifier, 5, 1);
            tableLayoutPanelIdentification.Controls.Add(comboBoxIdentificationCategory, 5, 3);
            tableLayoutPanelIdentification.Controls.Add(comboBoxTypeStatus, 5, 4);
            tableLayoutPanelIdentification.Controls.Add(comboBoxIdentificationDateCategory, 5, 5);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationQualifier, 4, 1);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationCategory, 4, 3);
            tableLayoutPanelIdentification.Controls.Add(labelTypeStatus, 4, 4);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationDateCategory, 4, 5);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationDate, 1, 5);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationResponsible, 1, 3);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationReference, 0, 7);
            tableLayoutPanelIdentification.Controls.Add(userControlModuleRelatedEntryIdentificationReference, 2, 7);
            tableLayoutPanelIdentification.Controls.Add(textBoxIdentificationNotes, 2, 6);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationNotes, 1, 6);
            tableLayoutPanelIdentification.Controls.Add(userControlModuleRelatedEntryIdentificationResponsible, 2, 3);
            tableLayoutPanelIdentification.Controls.Add(userControlDatePanelIdentificationDate, 2, 5);
            tableLayoutPanelIdentification.Controls.Add(textBoxTypeNotes, 2, 4);
            tableLayoutPanelIdentification.Controls.Add(comboBoxVernacularTerm, 2, 1);
            tableLayoutPanelIdentification.Controls.Add(userControlHierarchySelectorIdentificationQualifier, 6, 1);
            tableLayoutPanelIdentification.Controls.Add(userControlHierarchySelectorIdentificationCategory, 6, 3);
            tableLayoutPanelIdentification.Controls.Add(userControlHierarchySelectorTypeStatus, 6, 4);
            tableLayoutPanelIdentification.Controls.Add(userControlHierarchySelectorIdentificationDateCategory, 6, 5);
            tableLayoutPanelIdentification.Controls.Add(buttonCollectorIsResponsible, 3, 3);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationReferenceDetails, 4, 7);
            tableLayoutPanelIdentification.Controls.Add(textBoxIdentificationReferenceDetails, 5, 7);
            tableLayoutPanelIdentification.Controls.Add(panelTemplateIdentification, 0, 0);
            tableLayoutPanelIdentification.Controls.Add(labelIdentificationScientificTerm, 1, 2);
            tableLayoutPanelIdentification.Controls.Add(userControlModuleRelatedEntryIdentificationScientificTerm, 2, 2);
            tableLayoutPanelIdentification.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelIdentification.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            tableLayoutPanelIdentification.Location = new System.Drawing.Point(5, 20);
            tableLayoutPanelIdentification.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanelIdentification.Name = "tableLayoutPanelIdentification";
            tableLayoutPanelIdentification.RowCount = 9;
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelIdentification.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanelIdentification.Size = new System.Drawing.Size(839, 273);
            tableLayoutPanelIdentification.TabIndex = 0;
            // 
            // labelTaxonomicName
            // 
            labelTaxonomicName.AccessibleName = "Identification.TaxonomicName";
            labelTaxonomicName.AutoSize = true;
            labelTaxonomicName.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTaxonomicName.Location = new System.Drawing.Point(22, 8);
            labelTaxonomicName.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelTaxonomicName.Name = "labelTaxonomicName";
            labelTaxonomicName.Size = new System.Drawing.Size(85, 33);
            labelTaxonomicName.TabIndex = 0;
            labelTaxonomicName.Text = "Tax.name:";
            labelTaxonomicName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelVernacularTerm
            // 
            labelVernacularTerm.AccessibleName = "Identification.VernacularTerm";
            labelVernacularTerm.AutoSize = true;
            labelVernacularTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            labelVernacularTerm.Location = new System.Drawing.Point(22, 49);
            labelVernacularTerm.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelVernacularTerm.Name = "labelVernacularTerm";
            labelVernacularTerm.Size = new System.Drawing.Size(85, 33);
            labelVernacularTerm.TabIndex = 2;
            labelVernacularTerm.Text = "Vern.term:";
            labelVernacularTerm.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTypeNotes
            // 
            labelTypeNotes.AccessibleName = "Identification.TypeNotes";
            labelTypeNotes.AutoSize = true;
            labelTypeNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTypeNotes.Location = new System.Drawing.Point(22, 188);
            labelTypeNotes.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelTypeNotes.Name = "labelTypeNotes";
            labelTypeNotes.Size = new System.Drawing.Size(85, 45);
            labelTypeNotes.TabIndex = 3;
            labelTypeNotes.Text = "Type notes:";
            labelTypeNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userControlModuleRelatedEntryTaxonomicName
            // 
            userControlModuleRelatedEntryTaxonomicName.CanDeleteConnectionToModule = true;
            tableLayoutPanelIdentification.SetColumnSpan(userControlModuleRelatedEntryTaxonomicName, 5);
            userControlModuleRelatedEntryTaxonomicName.DependsOnUri = "";
            userControlModuleRelatedEntryTaxonomicName.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryTaxonomicName.Domain = "";
            userControlModuleRelatedEntryTaxonomicName.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryTaxonomicName.Location = new System.Drawing.Point(112, 0);
            userControlModuleRelatedEntryTaxonomicName.Margin = new System.Windows.Forms.Padding(0);
            userControlModuleRelatedEntryTaxonomicName.Module = null;
            userControlModuleRelatedEntryTaxonomicName.Name = "userControlModuleRelatedEntryTaxonomicName";
            userControlModuleRelatedEntryTaxonomicName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryTaxonomicName.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryTaxonomicName.ShowInfo = false;
            userControlModuleRelatedEntryTaxonomicName.Size = new System.Drawing.Size(727, 41);
            userControlModuleRelatedEntryTaxonomicName.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryTaxonomicName.TabIndex = 4;
            // 
            // comboBoxIdentificationQualifier
            // 
            comboBoxIdentificationQualifier.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxIdentificationQualifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxIdentificationQualifier.FormattingEnabled = true;
            comboBoxIdentificationQualifier.Location = new System.Drawing.Point(632, 41);
            comboBoxIdentificationQualifier.Margin = new System.Windows.Forms.Padding(0);
            comboBoxIdentificationQualifier.MaxDropDownItems = 20;
            comboBoxIdentificationQualifier.Name = "comboBoxIdentificationQualifier";
            comboBoxIdentificationQualifier.Size = new System.Drawing.Size(174, 25);
            comboBoxIdentificationQualifier.TabIndex = 6;
            // 
            // comboBoxIdentificationCategory
            // 
            comboBoxIdentificationCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxIdentificationCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxIdentificationCategory.FormattingEnabled = true;
            comboBoxIdentificationCategory.Location = new System.Drawing.Point(632, 127);
            comboBoxIdentificationCategory.Margin = new System.Windows.Forms.Padding(0);
            comboBoxIdentificationCategory.Name = "comboBoxIdentificationCategory";
            comboBoxIdentificationCategory.Size = new System.Drawing.Size(174, 25);
            comboBoxIdentificationCategory.TabIndex = 8;
            // 
            // comboBoxTypeStatus
            // 
            comboBoxTypeStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxTypeStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxTypeStatus.FormattingEnabled = true;
            comboBoxTypeStatus.Location = new System.Drawing.Point(632, 180);
            comboBoxTypeStatus.Margin = new System.Windows.Forms.Padding(0);
            comboBoxTypeStatus.Name = "comboBoxTypeStatus";
            comboBoxTypeStatus.Size = new System.Drawing.Size(174, 25);
            comboBoxTypeStatus.TabIndex = 10;
            // 
            // comboBoxIdentificationDateCategory
            // 
            comboBoxIdentificationDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxIdentificationDateCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxIdentificationDateCategory.FormattingEnabled = true;
            comboBoxIdentificationDateCategory.Location = new System.Drawing.Point(632, 233);
            comboBoxIdentificationDateCategory.Margin = new System.Windows.Forms.Padding(0);
            comboBoxIdentificationDateCategory.Name = "comboBoxIdentificationDateCategory";
            comboBoxIdentificationDateCategory.Size = new System.Drawing.Size(174, 25);
            comboBoxIdentificationDateCategory.TabIndex = 12;
            // 
            // labelIdentificationQualifier
            // 
            labelIdentificationQualifier.AccessibleName = "Identification.IdentificationQualifier";
            labelIdentificationQualifier.AutoSize = true;
            labelIdentificationQualifier.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationQualifier.Location = new System.Drawing.Point(551, 49);
            labelIdentificationQualifier.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelIdentificationQualifier.Name = "labelIdentificationQualifier";
            labelIdentificationQualifier.Size = new System.Drawing.Size(76, 33);
            labelIdentificationQualifier.TabIndex = 10;
            labelIdentificationQualifier.Text = "Qualifier:";
            labelIdentificationQualifier.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIdentificationCategory
            // 
            labelIdentificationCategory.AccessibleName = "Identification.IdentificationCategory";
            labelIdentificationCategory.AutoSize = true;
            labelIdentificationCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationCategory.Location = new System.Drawing.Point(551, 135);
            labelIdentificationCategory.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelIdentificationCategory.Name = "labelIdentificationCategory";
            labelIdentificationCategory.Size = new System.Drawing.Size(76, 45);
            labelIdentificationCategory.TabIndex = 11;
            labelIdentificationCategory.Text = "Category:";
            labelIdentificationCategory.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelTypeStatus
            // 
            labelTypeStatus.AccessibleName = "Identification.TypeStatus";
            labelTypeStatus.AutoSize = true;
            labelTypeStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTypeStatus.Location = new System.Drawing.Point(551, 188);
            labelTypeStatus.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelTypeStatus.Name = "labelTypeStatus";
            labelTypeStatus.Size = new System.Drawing.Size(76, 45);
            labelTypeStatus.TabIndex = 12;
            labelTypeStatus.Text = "Type stat.:";
            labelTypeStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIdentificationDateCategory
            // 
            labelIdentificationDateCategory.AccessibleName = "Identification.IdentificationDateCategory";
            labelIdentificationDateCategory.AutoSize = true;
            labelIdentificationDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationDateCategory.Location = new System.Drawing.Point(551, 241);
            labelIdentificationDateCategory.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelIdentificationDateCategory.Name = "labelIdentificationDateCategory";
            labelIdentificationDateCategory.Size = new System.Drawing.Size(76, 45);
            labelIdentificationDateCategory.TabIndex = 13;
            labelIdentificationDateCategory.Text = "Date cat.:";
            labelIdentificationDateCategory.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIdentificationDate
            // 
            labelIdentificationDate.AccessibleName = "Identification.IdentificationDate";
            labelIdentificationDate.AutoSize = true;
            labelIdentificationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationDate.Location = new System.Drawing.Point(22, 241);
            labelIdentificationDate.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelIdentificationDate.Name = "labelIdentificationDate";
            labelIdentificationDate.Size = new System.Drawing.Size(85, 45);
            labelIdentificationDate.TabIndex = 14;
            labelIdentificationDate.Text = "Date:";
            labelIdentificationDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIdentificationResponsible
            // 
            labelIdentificationResponsible.AccessibleName = "Identification.ResponsibleName";
            labelIdentificationResponsible.AutoSize = true;
            labelIdentificationResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationResponsible.Location = new System.Drawing.Point(22, 135);
            labelIdentificationResponsible.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelIdentificationResponsible.Name = "labelIdentificationResponsible";
            labelIdentificationResponsible.Size = new System.Drawing.Size(85, 45);
            labelIdentificationResponsible.TabIndex = 15;
            labelIdentificationResponsible.Text = "Respons.:";
            labelIdentificationResponsible.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelIdentificationReference
            // 
            labelIdentificationReference.AccessibleName = "Identification.ReferenceTitle";
            labelIdentificationReference.AutoSize = true;
            tableLayoutPanelIdentification.SetColumnSpan(labelIdentificationReference, 2);
            labelIdentificationReference.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationReference.Location = new System.Drawing.Point(5, 250);
            labelIdentificationReference.Margin = new System.Windows.Forms.Padding(5, 8, 5, 0);
            labelIdentificationReference.Name = "labelIdentificationReference";
            labelIdentificationReference.Size = new System.Drawing.Size(102, 28);
            labelIdentificationReference.TabIndex = 16;
            labelIdentificationReference.Text = "Reference:";
            labelIdentificationReference.TextAlign = System.Drawing.ContentAlignment.TopRight;
            labelIdentificationReference.Visible = false;
            // 
            // userControlModuleRelatedEntryIdentificationReference
            // 
            userControlModuleRelatedEntryIdentificationReference.CanDeleteConnectionToModule = true;
            tableLayoutPanelIdentification.SetColumnSpan(userControlModuleRelatedEntryIdentificationReference, 2);
            userControlModuleRelatedEntryIdentificationReference.DependsOnUri = "";
            userControlModuleRelatedEntryIdentificationReference.Dock = System.Windows.Forms.DockStyle.Top;
            userControlModuleRelatedEntryIdentificationReference.Domain = "";
            userControlModuleRelatedEntryIdentificationReference.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryIdentificationReference.Location = new System.Drawing.Point(112, 242);
            userControlModuleRelatedEntryIdentificationReference.Margin = new System.Windows.Forms.Padding(0);
            userControlModuleRelatedEntryIdentificationReference.Module = null;
            userControlModuleRelatedEntryIdentificationReference.Name = "userControlModuleRelatedEntryIdentificationReference";
            userControlModuleRelatedEntryIdentificationReference.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryIdentificationReference.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryIdentificationReference.ShowInfo = false;
            userControlModuleRelatedEntryIdentificationReference.Size = new System.Drawing.Size(434, 36);
            userControlModuleRelatedEntryIdentificationReference.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryIdentificationReference.TabIndex = 13;
            userControlModuleRelatedEntryIdentificationReference.Visible = false;
            // 
            // textBoxIdentificationNotes
            // 
            tableLayoutPanelIdentification.SetColumnSpan(textBoxIdentificationNotes, 5);
            textBoxIdentificationNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxIdentificationNotes.Location = new System.Drawing.Point(112, 286);
            textBoxIdentificationNotes.Margin = new System.Windows.Forms.Padding(0);
            textBoxIdentificationNotes.Multiline = true;
            textBoxIdentificationNotes.Name = "textBoxIdentificationNotes";
            textBoxIdentificationNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxIdentificationNotes.Size = new System.Drawing.Size(727, 1);
            textBoxIdentificationNotes.TabIndex = 14;
            // 
            // labelIdentificationNotes
            // 
            labelIdentificationNotes.AccessibleName = "Identification.Notes";
            labelIdentificationNotes.AutoSize = true;
            labelIdentificationNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationNotes.Location = new System.Drawing.Point(22, 290);
            labelIdentificationNotes.Margin = new System.Windows.Forms.Padding(5, 4, 5, 0);
            labelIdentificationNotes.Name = "labelIdentificationNotes";
            labelIdentificationNotes.Size = new System.Drawing.Size(85, 1);
            labelIdentificationNotes.TabIndex = 19;
            labelIdentificationNotes.Text = "Notes:";
            labelIdentificationNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userControlModuleRelatedEntryIdentificationResponsible
            // 
            userControlModuleRelatedEntryIdentificationResponsible.CanDeleteConnectionToModule = true;
            userControlModuleRelatedEntryIdentificationResponsible.DependsOnUri = "";
            userControlModuleRelatedEntryIdentificationResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryIdentificationResponsible.Domain = "";
            userControlModuleRelatedEntryIdentificationResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryIdentificationResponsible.Location = new System.Drawing.Point(112, 127);
            userControlModuleRelatedEntryIdentificationResponsible.Margin = new System.Windows.Forms.Padding(0);
            userControlModuleRelatedEntryIdentificationResponsible.Module = null;
            userControlModuleRelatedEntryIdentificationResponsible.Name = "userControlModuleRelatedEntryIdentificationResponsible";
            userControlModuleRelatedEntryIdentificationResponsible.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryIdentificationResponsible.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryIdentificationResponsible.ShowInfo = false;
            userControlModuleRelatedEntryIdentificationResponsible.Size = new System.Drawing.Size(408, 53);
            userControlModuleRelatedEntryIdentificationResponsible.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryIdentificationResponsible.TabIndex = 11;
            // 
            // userControlDatePanelIdentificationDate
            // 
            tableLayoutPanelIdentification.SetColumnSpan(userControlDatePanelIdentificationDate, 2);
            userControlDatePanelIdentificationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlDatePanelIdentificationDate.Location = new System.Drawing.Point(112, 233);
            userControlDatePanelIdentificationDate.Margin = new System.Windows.Forms.Padding(0);
            userControlDatePanelIdentificationDate.Name = "userControlDatePanelIdentificationDate";
            userControlDatePanelIdentificationDate.Size = new System.Drawing.Size(434, 53);
            userControlDatePanelIdentificationDate.TabIndex = 7;
            // 
            // textBoxTypeNotes
            // 
            textBoxTypeNotes.AcceptsTab = true;
            tableLayoutPanelIdentification.SetColumnSpan(textBoxTypeNotes, 2);
            textBoxTypeNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTypeNotes.Location = new System.Drawing.Point(112, 180);
            textBoxTypeNotes.Margin = new System.Windows.Forms.Padding(0);
            textBoxTypeNotes.Name = "textBoxTypeNotes";
            textBoxTypeNotes.Size = new System.Drawing.Size(434, 23);
            textBoxTypeNotes.TabIndex = 9;
            // 
            // comboBoxVernacularTerm
            // 
            tableLayoutPanelIdentification.SetColumnSpan(comboBoxVernacularTerm, 2);
            comboBoxVernacularTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxVernacularTerm.FormattingEnabled = true;
            comboBoxVernacularTerm.Location = new System.Drawing.Point(112, 41);
            comboBoxVernacularTerm.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            comboBoxVernacularTerm.Name = "comboBoxVernacularTerm";
            comboBoxVernacularTerm.Size = new System.Drawing.Size(434, 25);
            comboBoxVernacularTerm.TabIndex = 20;
            comboBoxVernacularTerm.DropDown += comboBoxVernacularTerm_DropDown;
            comboBoxVernacularTerm.SelectionChangeCommitted += comboBoxVernacularTerm_SelectionChangeCommitted;
            // 
            // userControlHierarchySelectorIdentificationQualifier
            // 
            userControlHierarchySelectorIdentificationQualifier.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlHierarchySelectorIdentificationQualifier.Location = new System.Drawing.Point(806, 41);
            userControlHierarchySelectorIdentificationQualifier.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            userControlHierarchySelectorIdentificationQualifier.Name = "userControlHierarchySelectorIdentificationQualifier";
            userControlHierarchySelectorIdentificationQualifier.Size = new System.Drawing.Size(33, 37);
            userControlHierarchySelectorIdentificationQualifier.TabIndex = 21;
            // 
            // userControlHierarchySelectorIdentificationCategory
            // 
            userControlHierarchySelectorIdentificationCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlHierarchySelectorIdentificationCategory.Location = new System.Drawing.Point(806, 127);
            userControlHierarchySelectorIdentificationCategory.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            userControlHierarchySelectorIdentificationCategory.Name = "userControlHierarchySelectorIdentificationCategory";
            userControlHierarchySelectorIdentificationCategory.Size = new System.Drawing.Size(33, 49);
            userControlHierarchySelectorIdentificationCategory.TabIndex = 22;
            // 
            // userControlHierarchySelectorTypeStatus
            // 
            userControlHierarchySelectorTypeStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlHierarchySelectorTypeStatus.Location = new System.Drawing.Point(806, 180);
            userControlHierarchySelectorTypeStatus.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            userControlHierarchySelectorTypeStatus.Name = "userControlHierarchySelectorTypeStatus";
            userControlHierarchySelectorTypeStatus.Size = new System.Drawing.Size(33, 49);
            userControlHierarchySelectorTypeStatus.TabIndex = 23;
            // 
            // userControlHierarchySelectorIdentificationDateCategory
            // 
            userControlHierarchySelectorIdentificationDateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlHierarchySelectorIdentificationDateCategory.Location = new System.Drawing.Point(806, 233);
            userControlHierarchySelectorIdentificationDateCategory.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            userControlHierarchySelectorIdentificationDateCategory.Name = "userControlHierarchySelectorIdentificationDateCategory";
            userControlHierarchySelectorIdentificationDateCategory.Size = new System.Drawing.Size(33, 49);
            userControlHierarchySelectorIdentificationDateCategory.TabIndex = 24;
            // 
            // buttonCollectorIsResponsible
            // 
            buttonCollectorIsResponsible.Dock = System.Windows.Forms.DockStyle.Top;
            buttonCollectorIsResponsible.Enabled = false;
            buttonCollectorIsResponsible.Image = Resource.Agent;
            buttonCollectorIsResponsible.Location = new System.Drawing.Point(520, 127);
            buttonCollectorIsResponsible.Margin = new System.Windows.Forms.Padding(0);
            buttonCollectorIsResponsible.Name = "buttonCollectorIsResponsible";
            buttonCollectorIsResponsible.Size = new System.Drawing.Size(26, 32);
            buttonCollectorIsResponsible.TabIndex = 25;
            buttonCollectorIsResponsible.UseVisualStyleBackColor = true;
            buttonCollectorIsResponsible.Click += buttonCollectorIsResponsible_Click;
            // 
            // labelIdentificationReferenceDetails
            // 
            labelIdentificationReferenceDetails.AccessibleName = "Identification.ReferenceDetails";
            labelIdentificationReferenceDetails.AutoSize = true;
            labelIdentificationReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationReferenceDetails.Location = new System.Drawing.Point(551, 242);
            labelIdentificationReferenceDetails.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            labelIdentificationReferenceDetails.Name = "labelIdentificationReferenceDetails";
            labelIdentificationReferenceDetails.Size = new System.Drawing.Size(76, 36);
            labelIdentificationReferenceDetails.TabIndex = 26;
            labelIdentificationReferenceDetails.Text = "Details:";
            labelIdentificationReferenceDetails.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            labelIdentificationReferenceDetails.Visible = false;
            // 
            // textBoxIdentificationReferenceDetails
            // 
            tableLayoutPanelIdentification.SetColumnSpan(textBoxIdentificationReferenceDetails, 2);
            textBoxIdentificationReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxIdentificationReferenceDetails.Location = new System.Drawing.Point(632, 242);
            textBoxIdentificationReferenceDetails.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            textBoxIdentificationReferenceDetails.Name = "textBoxIdentificationReferenceDetails";
            textBoxIdentificationReferenceDetails.Size = new System.Drawing.Size(207, 23);
            textBoxIdentificationReferenceDetails.TabIndex = 27;
            textBoxIdentificationReferenceDetails.Visible = false;
            // 
            // panelTemplateIdentification
            // 
            panelTemplateIdentification.Controls.Add(buttonTemplateIdentificationEdit);
            panelTemplateIdentification.Controls.Add(buttonTemplateIdentificationSet);
            panelTemplateIdentification.Dock = System.Windows.Forms.DockStyle.Top;
            panelTemplateIdentification.Location = new System.Drawing.Point(0, 0);
            panelTemplateIdentification.Margin = new System.Windows.Forms.Padding(0);
            panelTemplateIdentification.Name = "panelTemplateIdentification";
            tableLayoutPanelIdentification.SetRowSpan(panelTemplateIdentification, 5);
            panelTemplateIdentification.Size = new System.Drawing.Size(17, 77);
            panelTemplateIdentification.TabIndex = 30;
            // 
            // buttonTemplateIdentificationEdit
            // 
            buttonTemplateIdentificationEdit.Dock = System.Windows.Forms.DockStyle.Top;
            buttonTemplateIdentificationEdit.FlatAppearance.BorderSize = 0;
            buttonTemplateIdentificationEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonTemplateIdentificationEdit.Image = Resource.TemplateEditor;
            buttonTemplateIdentificationEdit.Location = new System.Drawing.Point(0, 36);
            buttonTemplateIdentificationEdit.Margin = new System.Windows.Forms.Padding(0);
            buttonTemplateIdentificationEdit.Name = "buttonTemplateIdentificationEdit";
            buttonTemplateIdentificationEdit.Size = new System.Drawing.Size(17, 37);
            buttonTemplateIdentificationEdit.TabIndex = 29;
            buttonTemplateIdentificationEdit.UseVisualStyleBackColor = true;
            buttonTemplateIdentificationEdit.Click += buttonTemplateIdentificationEdit_Click;
            // 
            // buttonTemplateIdentificationSet
            // 
            buttonTemplateIdentificationSet.Dock = System.Windows.Forms.DockStyle.Top;
            buttonTemplateIdentificationSet.FlatAppearance.BorderSize = 0;
            buttonTemplateIdentificationSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonTemplateIdentificationSet.ForeColor = System.Drawing.SystemColors.Control;
            buttonTemplateIdentificationSet.Image = Resource.Template;
            buttonTemplateIdentificationSet.Location = new System.Drawing.Point(0, 0);
            buttonTemplateIdentificationSet.Margin = new System.Windows.Forms.Padding(0);
            buttonTemplateIdentificationSet.Name = "buttonTemplateIdentificationSet";
            buttonTemplateIdentificationSet.Size = new System.Drawing.Size(17, 36);
            buttonTemplateIdentificationSet.TabIndex = 28;
            buttonTemplateIdentificationSet.UseVisualStyleBackColor = true;
            buttonTemplateIdentificationSet.Click += buttonTemplateIdentificationSet_Click;
            // 
            // labelIdentificationScientificTerm
            // 
            labelIdentificationScientificTerm.AutoSize = true;
            labelIdentificationScientificTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            labelIdentificationScientificTerm.Location = new System.Drawing.Point(17, 82);
            labelIdentificationScientificTerm.Margin = new System.Windows.Forms.Padding(0);
            labelIdentificationScientificTerm.Name = "labelIdentificationScientificTerm";
            labelIdentificationScientificTerm.Size = new System.Drawing.Size(95, 45);
            labelIdentificationScientificTerm.TabIndex = 31;
            labelIdentificationScientificTerm.Text = "Name/Term:";
            labelIdentificationScientificTerm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            labelIdentificationScientificTerm.Visible = false;
            // 
            // userControlModuleRelatedEntryIdentificationScientificTerm
            // 
            userControlModuleRelatedEntryIdentificationScientificTerm.CanDeleteConnectionToModule = true;
            tableLayoutPanelIdentification.SetColumnSpan(userControlModuleRelatedEntryIdentificationScientificTerm, 5);
            userControlModuleRelatedEntryIdentificationScientificTerm.DependsOnUri = "";
            userControlModuleRelatedEntryIdentificationScientificTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            userControlModuleRelatedEntryIdentificationScientificTerm.Domain = "";
            userControlModuleRelatedEntryIdentificationScientificTerm.LinkDeleteConnectionToModuleToTableGrant = false;
            userControlModuleRelatedEntryIdentificationScientificTerm.Location = new System.Drawing.Point(112, 82);
            userControlModuleRelatedEntryIdentificationScientificTerm.Margin = new System.Windows.Forms.Padding(0);
            userControlModuleRelatedEntryIdentificationScientificTerm.Module = null;
            userControlModuleRelatedEntryIdentificationScientificTerm.Name = "userControlModuleRelatedEntryIdentificationScientificTerm";
            userControlModuleRelatedEntryIdentificationScientificTerm.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            userControlModuleRelatedEntryIdentificationScientificTerm.ShowHtmlUnitValues = false;
            userControlModuleRelatedEntryIdentificationScientificTerm.ShowInfo = false;
            userControlModuleRelatedEntryIdentificationScientificTerm.Size = new System.Drawing.Size(727, 45);
            userControlModuleRelatedEntryIdentificationScientificTerm.SupressEmptyRemoteValues = false;
            userControlModuleRelatedEntryIdentificationScientificTerm.TabIndex = 32;
            userControlModuleRelatedEntryIdentificationScientificTerm.Visible = false;
            // 
            // UserControl_Identification
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(groupBoxOverviewIdentification);
            Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            Name = "UserControl_Identification";
            Size = new System.Drawing.Size(849, 297);
            groupBoxOverviewIdentification.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxIdentification).EndInit();
            tableLayoutPanelIdentification.ResumeLayout(false);
            tableLayoutPanelIdentification.PerformLayout();
            panelTemplateIdentification.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxOverviewIdentification;
        private System.Windows.Forms.PictureBox pictureBoxIdentification;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelIdentification;
        private System.Windows.Forms.Label labelTaxonomicName;
        private System.Windows.Forms.Label labelVernacularTerm;
        private System.Windows.Forms.Label labelTypeNotes;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryTaxonomicName;
        private System.Windows.Forms.ComboBox comboBoxIdentificationQualifier;
        private System.Windows.Forms.ComboBox comboBoxIdentificationCategory;
        private System.Windows.Forms.ComboBox comboBoxTypeStatus;
        private System.Windows.Forms.ComboBox comboBoxIdentificationDateCategory;
        private System.Windows.Forms.Label labelIdentificationQualifier;
        private System.Windows.Forms.Label labelIdentificationCategory;
        private System.Windows.Forms.Label labelTypeStatus;
        private System.Windows.Forms.Label labelIdentificationDateCategory;
        private System.Windows.Forms.Label labelIdentificationDate;
        private System.Windows.Forms.Label labelIdentificationResponsible;
        private System.Windows.Forms.Label labelIdentificationReference;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryIdentificationReference;
        private System.Windows.Forms.TextBox textBoxIdentificationNotes;
        private System.Windows.Forms.Label labelIdentificationNotes;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryIdentificationResponsible;
        private DiversityWorkbench.UserControls.UserControlDatePanel userControlDatePanelIdentificationDate;
        private System.Windows.Forms.TextBox textBoxTypeNotes;
        private System.Windows.Forms.ComboBox comboBoxVernacularTerm;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorIdentificationQualifier;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorIdentificationCategory;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorTypeStatus;
        private DiversityWorkbench.UserControls.UserControlHierarchySelector userControlHierarchySelectorIdentificationDateCategory;
        private System.Windows.Forms.Button buttonCollectorIsResponsible;
        private System.Windows.Forms.Label labelIdentificationReferenceDetails;
        private System.Windows.Forms.TextBox textBoxIdentificationReferenceDetails;
        private System.Windows.Forms.Panel panelTemplateIdentification;
        private System.Windows.Forms.Button buttonTemplateIdentificationEdit;
        private System.Windows.Forms.Button buttonTemplateIdentificationSet;
        private System.Windows.Forms.Label labelIdentificationScientificTerm;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryIdentificationScientificTerm;
    }
}
