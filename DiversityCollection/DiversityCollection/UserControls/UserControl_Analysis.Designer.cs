namespace DiversityCollection.UserControls
{
    partial class UserControl_Analysis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Analysis));
            this.groupBoxAnalysis = new System.Windows.Forms.GroupBox();
            this.pictureBoxAnalysis = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelAnalysis = new System.Windows.Forms.TableLayoutPanel();
            this.labelAnalysisNumber = new System.Windows.Forms.Label();
            this.textBoxAnalysisNumber = new System.Windows.Forms.TextBox();
            this.labelAnalysisURI = new System.Windows.Forms.Label();
            this.buttonAnalysisURIOpen = new System.Windows.Forms.Button();
            this.buttonAnalysisOpen = new System.Windows.Forms.Button();
            this.userControlModuleRelatedEntryAnalysisResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelAnalysisResponsible = new System.Windows.Forms.Label();
            this.labelAnalysisSpecimenPart = new System.Windows.Forms.Label();
            this.comboBoxAnalysisSpecimenPart = new System.Windows.Forms.ComboBox();
            this.textBoxAnalysisURI = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelResults = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxAnalysisResult = new System.Windows.Forms.ComboBox();
            this.textBoxAnalysisResult = new System.Windows.Forms.TextBox();
            this.labelAnalysisResult = new System.Windows.Forms.Label();
            this.labelAnalysisMeasurementUnit = new System.Windows.Forms.Label();
            this.labelAnalysisNotes = new System.Windows.Forms.Label();
            this.textBoxAnalysisNotes = new System.Windows.Forms.TextBox();
            this.buttonEditSequence = new System.Windows.Forms.Button();
            this.userControlRichEditSequence = new DiversityWorkbench.UserControls.UserControlRichEdit();
            this.labelAnalysisDate = new System.Windows.Forms.Label();
            this.dateTimePickerAnalysisDate = new System.Windows.Forms.DateTimePicker();
            this.textBoxAnalysisDate = new System.Windows.Forms.TextBox();
            this.groupBoxAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnalysis)).BeginInit();
            this.tableLayoutPanelAnalysis.SuspendLayout();
            this.tableLayoutPanelResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxAnalysis
            // 
            this.groupBoxAnalysis.AccessibleName = "Analysis";
            this.groupBoxAnalysis.Controls.Add(this.pictureBoxAnalysis);
            this.groupBoxAnalysis.Controls.Add(this.tableLayoutPanelAnalysis);
            this.groupBoxAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxAnalysis.ForeColor = System.Drawing.Color.Blue;
            this.groupBoxAnalysis.Location = new System.Drawing.Point(0, 0);
            this.groupBoxAnalysis.MinimumSize = new System.Drawing.Size(0, 120);
            this.groupBoxAnalysis.Name = "groupBoxAnalysis";
            this.groupBoxAnalysis.Size = new System.Drawing.Size(599, 279);
            this.groupBoxAnalysis.TabIndex = 2;
            this.groupBoxAnalysis.TabStop = false;
            this.groupBoxAnalysis.Text = "Analysis";
            // 
            // pictureBoxAnalysis
            // 
            this.pictureBoxAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxAnalysis.Image = global::DiversityCollection.Resource.Analysis;
            this.pictureBoxAnalysis.Location = new System.Drawing.Point(580, 1);
            this.pictureBoxAnalysis.Name = "pictureBoxAnalysis";
            this.pictureBoxAnalysis.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxAnalysis.TabIndex = 1;
            this.pictureBoxAnalysis.TabStop = false;
            // 
            // tableLayoutPanelAnalysis
            // 
            this.tableLayoutPanelAnalysis.ColumnCount = 8;
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelAnalysisNumber, 0, 0);
            this.tableLayoutPanelAnalysis.Controls.Add(this.textBoxAnalysisNumber, 2, 0);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelAnalysisURI, 0, 1);
            this.tableLayoutPanelAnalysis.Controls.Add(this.buttonAnalysisURIOpen, 7, 1);
            this.tableLayoutPanelAnalysis.Controls.Add(this.buttonAnalysisOpen, 7, 0);
            this.tableLayoutPanelAnalysis.Controls.Add(this.userControlModuleRelatedEntryAnalysisResponsible, 2, 3);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelAnalysisResponsible, 0, 3);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelAnalysisSpecimenPart, 0, 2);
            this.tableLayoutPanelAnalysis.Controls.Add(this.comboBoxAnalysisSpecimenPart, 2, 2);
            this.tableLayoutPanelAnalysis.Controls.Add(this.textBoxAnalysisURI, 2, 1);
            this.tableLayoutPanelAnalysis.Controls.Add(this.tableLayoutPanelResults, 0, 4);
            this.tableLayoutPanelAnalysis.Controls.Add(this.labelAnalysisDate, 4, 0);
            this.tableLayoutPanelAnalysis.Controls.Add(this.dateTimePickerAnalysisDate, 6, 0);
            this.tableLayoutPanelAnalysis.Controls.Add(this.textBoxAnalysisDate, 5, 0);
            this.tableLayoutPanelAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelAnalysis.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelAnalysis.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelAnalysis.Name = "tableLayoutPanelAnalysis";
            this.tableLayoutPanelAnalysis.RowCount = 5;
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAnalysis.Size = new System.Drawing.Size(593, 260);
            this.tableLayoutPanelAnalysis.TabIndex = 0;
            // 
            // labelAnalysisNumber
            // 
            this.labelAnalysisNumber.AccessibleName = "IdentificationUnitAnalysis.AnalysisNumber";
            this.labelAnalysisNumber.AutoSize = true;
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.labelAnalysisNumber, 2);
            this.labelAnalysisNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisNumber.Location = new System.Drawing.Point(3, 5);
            this.labelAnalysisNumber.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.labelAnalysisNumber.Name = "labelAnalysisNumber";
            this.labelAnalysisNumber.Size = new System.Drawing.Size(55, 19);
            this.labelAnalysisNumber.TabIndex = 0;
            this.labelAnalysisNumber.Text = "Nr. of an.:";
            this.labelAnalysisNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxAnalysisNumber
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.textBoxAnalysisNumber, 2);
            this.textBoxAnalysisNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnalysisNumber.Location = new System.Drawing.Point(58, 1);
            this.textBoxAnalysisNumber.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.textBoxAnalysisNumber.Multiline = true;
            this.textBoxAnalysisNumber.Name = "textBoxAnalysisNumber";
            this.textBoxAnalysisNumber.Size = new System.Drawing.Size(115, 21);
            this.textBoxAnalysisNumber.TabIndex = 1;
            // 
            // labelAnalysisURI
            // 
            this.labelAnalysisURI.AccessibleName = "IdentificationUnitAnalysis.ExternalAnalysisURI";
            this.labelAnalysisURI.AutoSize = true;
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.labelAnalysisURI, 2);
            this.labelAnalysisURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisURI.Location = new System.Drawing.Point(3, 29);
            this.labelAnalysisURI.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.labelAnalysisURI.Name = "labelAnalysisURI";
            this.labelAnalysisURI.Size = new System.Drawing.Size(55, 19);
            this.labelAnalysisURI.TabIndex = 4;
            this.labelAnalysisURI.Text = "URI:";
            this.labelAnalysisURI.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // buttonAnalysisURIOpen
            // 
            this.buttonAnalysisURIOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAnalysisURIOpen.Image = ((System.Drawing.Image)(resources.GetObject("buttonAnalysisURIOpen.Image")));
            this.buttonAnalysisURIOpen.Location = new System.Drawing.Point(569, 24);
            this.buttonAnalysisURIOpen.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonAnalysisURIOpen.Name = "buttonAnalysisURIOpen";
            this.buttonAnalysisURIOpen.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.buttonAnalysisURIOpen.Size = new System.Drawing.Size(24, 23);
            this.buttonAnalysisURIOpen.TabIndex = 6;
            this.buttonAnalysisURIOpen.UseVisualStyleBackColor = true;
            this.buttonAnalysisURIOpen.Click += new System.EventHandler(this.buttonAnalysisURIOpen_Click);
            // 
            // buttonAnalysisOpen
            // 
            this.buttonAnalysisOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAnalysisOpen.Image = global::DiversityCollection.Resource.Analysis;
            this.buttonAnalysisOpen.Location = new System.Drawing.Point(569, 0);
            this.buttonAnalysisOpen.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonAnalysisOpen.Name = "buttonAnalysisOpen";
            this.buttonAnalysisOpen.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.buttonAnalysisOpen.Size = new System.Drawing.Size(24, 23);
            this.buttonAnalysisOpen.TabIndex = 9;
            this.buttonAnalysisOpen.UseVisualStyleBackColor = true;
            this.buttonAnalysisOpen.Click += new System.EventHandler(this.buttonAnalysisOpen_Click);
            // 
            // userControlModuleRelatedEntryAnalysisResponsible
            // 
            this.userControlModuleRelatedEntryAnalysisResponsible.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.userControlModuleRelatedEntryAnalysisResponsible, 6);
            this.userControlModuleRelatedEntryAnalysisResponsible.DependsOnUri = "";
            this.userControlModuleRelatedEntryAnalysisResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryAnalysisResponsible.Domain = "";
            this.userControlModuleRelatedEntryAnalysisResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryAnalysisResponsible.Location = new System.Drawing.Point(58, 75);
            this.userControlModuleRelatedEntryAnalysisResponsible.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryAnalysisResponsible.Module = null;
            this.userControlModuleRelatedEntryAnalysisResponsible.Name = "userControlModuleRelatedEntryAnalysisResponsible";
            this.userControlModuleRelatedEntryAnalysisResponsible.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryAnalysisResponsible.ShowInfo = false;
            this.userControlModuleRelatedEntryAnalysisResponsible.Size = new System.Drawing.Size(535, 24);
            this.userControlModuleRelatedEntryAnalysisResponsible.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryAnalysisResponsible.TabIndex = 11;
            // 
            // labelAnalysisResponsible
            // 
            this.labelAnalysisResponsible.AccessibleName = "IdentificationUnitAnalysis.ResponsibleName";
            this.labelAnalysisResponsible.AutoSize = true;
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.labelAnalysisResponsible, 2);
            this.labelAnalysisResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisResponsible.Location = new System.Drawing.Point(3, 81);
            this.labelAnalysisResponsible.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelAnalysisResponsible.Name = "labelAnalysisResponsible";
            this.labelAnalysisResponsible.Size = new System.Drawing.Size(55, 19);
            this.labelAnalysisResponsible.TabIndex = 12;
            this.labelAnalysisResponsible.Text = "Respons.:";
            this.labelAnalysisResponsible.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelAnalysisSpecimenPart
            // 
            this.labelAnalysisSpecimenPart.AccessibleName = "IdentificationUnitAnalysis.SpecimenPartID";
            this.labelAnalysisSpecimenPart.AutoSize = true;
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.labelAnalysisSpecimenPart, 2);
            this.labelAnalysisSpecimenPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisSpecimenPart.Location = new System.Drawing.Point(3, 54);
            this.labelAnalysisSpecimenPart.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelAnalysisSpecimenPart.Name = "labelAnalysisSpecimenPart";
            this.labelAnalysisSpecimenPart.Size = new System.Drawing.Size(55, 21);
            this.labelAnalysisSpecimenPart.TabIndex = 16;
            this.labelAnalysisSpecimenPart.Text = "Part:";
            this.labelAnalysisSpecimenPart.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxAnalysisSpecimenPart
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.comboBoxAnalysisSpecimenPart, 6);
            this.comboBoxAnalysisSpecimenPart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAnalysisSpecimenPart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnalysisSpecimenPart.FormattingEnabled = true;
            this.comboBoxAnalysisSpecimenPart.Location = new System.Drawing.Point(58, 51);
            this.comboBoxAnalysisSpecimenPart.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.comboBoxAnalysisSpecimenPart.Name = "comboBoxAnalysisSpecimenPart";
            this.comboBoxAnalysisSpecimenPart.Size = new System.Drawing.Size(535, 21);
            this.comboBoxAnalysisSpecimenPart.TabIndex = 22;
            this.comboBoxAnalysisSpecimenPart.SelectionChangeCommitted += new System.EventHandler(this.comboBoxAnalysisSpecimenPart_SelectionChangeCommitted);
            // 
            // textBoxAnalysisURI
            // 
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.textBoxAnalysisURI, 5);
            this.textBoxAnalysisURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnalysisURI.Location = new System.Drawing.Point(58, 25);
            this.textBoxAnalysisURI.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
            this.textBoxAnalysisURI.Multiline = true;
            this.textBoxAnalysisURI.Name = "textBoxAnalysisURI";
            this.textBoxAnalysisURI.Size = new System.Drawing.Size(511, 20);
            this.textBoxAnalysisURI.TabIndex = 23;
            // 
            // tableLayoutPanelResults
            // 
            this.tableLayoutPanelResults.ColumnCount = 3;
            this.tableLayoutPanelAnalysis.SetColumnSpan(this.tableLayoutPanelResults, 8);
            this.tableLayoutPanelResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelResults.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelResults.Controls.Add(this.comboBoxAnalysisResult, 1, 0);
            this.tableLayoutPanelResults.Controls.Add(this.textBoxAnalysisResult, 1, 1);
            this.tableLayoutPanelResults.Controls.Add(this.labelAnalysisResult, 0, 0);
            this.tableLayoutPanelResults.Controls.Add(this.labelAnalysisMeasurementUnit, 2, 0);
            this.tableLayoutPanelResults.Controls.Add(this.labelAnalysisNotes, 0, 4);
            this.tableLayoutPanelResults.Controls.Add(this.textBoxAnalysisNotes, 1, 4);
            this.tableLayoutPanelResults.Controls.Add(this.buttonEditSequence, 0, 3);
            this.tableLayoutPanelResults.Controls.Add(this.userControlRichEditSequence, 1, 2);
            this.tableLayoutPanelResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelResults.Location = new System.Drawing.Point(0, 100);
            this.tableLayoutPanelResults.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelResults.Name = "tableLayoutPanelResults";
            this.tableLayoutPanelResults.RowCount = 5;
            this.tableLayoutPanelResults.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelResults.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelResults.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelResults.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelResults.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelResults.Size = new System.Drawing.Size(593, 160);
            this.tableLayoutPanelResults.TabIndex = 24;
            // 
            // comboBoxAnalysisResult
            // 
            this.comboBoxAnalysisResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAnalysisResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAnalysisResult.DropDownWidth = 160;
            this.comboBoxAnalysisResult.FormattingEnabled = true;
            this.comboBoxAnalysisResult.Location = new System.Drawing.Point(50, 0);
            this.comboBoxAnalysisResult.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.comboBoxAnalysisResult.Name = "comboBoxAnalysisResult";
            this.comboBoxAnalysisResult.Size = new System.Drawing.Size(543, 21);
            this.comboBoxAnalysisResult.TabIndex = 19;
            // 
            // textBoxAnalysisResult
            // 
            this.textBoxAnalysisResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnalysisResult.Location = new System.Drawing.Point(50, 24);
            this.textBoxAnalysisResult.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxAnalysisResult.Multiline = true;
            this.textBoxAnalysisResult.Name = "textBoxAnalysisResult";
            this.textBoxAnalysisResult.Size = new System.Drawing.Size(543, 50);
            this.textBoxAnalysisResult.TabIndex = 20;
            // 
            // labelAnalysisResult
            // 
            this.labelAnalysisResult.AccessibleName = "IdentificationUnitAnalysis.AnalysisResult";
            this.labelAnalysisResult.AutoSize = true;
            this.labelAnalysisResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAnalysisResult.Location = new System.Drawing.Point(3, 5);
            this.labelAnalysisResult.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.labelAnalysisResult.Name = "labelAnalysisResult";
            this.tableLayoutPanelResults.SetRowSpan(this.labelAnalysisResult, 3);
            this.labelAnalysisResult.Size = new System.Drawing.Size(47, 105);
            this.labelAnalysisResult.TabIndex = 2;
            this.labelAnalysisResult.Text = "Result:";
            this.labelAnalysisResult.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelAnalysisMeasurementUnit
            // 
            this.labelAnalysisMeasurementUnit.AccessibleName = "Analysis.MeasurementUnit";
            this.labelAnalysisMeasurementUnit.AutoSize = true;
            this.labelAnalysisMeasurementUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisMeasurementUnit.Location = new System.Drawing.Point(593, 3);
            this.labelAnalysisMeasurementUnit.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.labelAnalysisMeasurementUnit.Name = "labelAnalysisMeasurementUnit";
            this.tableLayoutPanelResults.SetRowSpan(this.labelAnalysisMeasurementUnit, 4);
            this.labelAnalysisMeasurementUnit.Size = new System.Drawing.Size(1, 127);
            this.labelAnalysisMeasurementUnit.TabIndex = 18;
            this.labelAnalysisMeasurementUnit.Tag = "";
            this.labelAnalysisMeasurementUnit.Visible = false;
            // 
            // labelAnalysisNotes
            // 
            this.labelAnalysisNotes.AccessibleName = "IdentificationUnitAnalysis.Notes";
            this.labelAnalysisNotes.AutoSize = true;
            this.labelAnalysisNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisNotes.Location = new System.Drawing.Point(3, 135);
            this.labelAnalysisNotes.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.labelAnalysisNotes.Name = "labelAnalysisNotes";
            this.labelAnalysisNotes.Size = new System.Drawing.Size(47, 25);
            this.labelAnalysisNotes.TabIndex = 7;
            this.labelAnalysisNotes.Text = "Notes:";
            this.labelAnalysisNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxAnalysisNotes
            // 
            this.tableLayoutPanelResults.SetColumnSpan(this.textBoxAnalysisNotes, 2);
            this.textBoxAnalysisNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnalysisNotes.Location = new System.Drawing.Point(50, 131);
            this.textBoxAnalysisNotes.Margin = new System.Windows.Forms.Padding(0, 1, 0, 2);
            this.textBoxAnalysisNotes.MinimumSize = new System.Drawing.Size(4, 20);
            this.textBoxAnalysisNotes.Multiline = true;
            this.textBoxAnalysisNotes.Name = "textBoxAnalysisNotes";
            this.textBoxAnalysisNotes.Size = new System.Drawing.Size(543, 27);
            this.textBoxAnalysisNotes.TabIndex = 8;
            // 
            // buttonEditSequence
            // 
            this.buttonEditSequence.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonEditSequence.FlatAppearance.BorderSize = 0;
            this.buttonEditSequence.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditSequence.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEditSequence.Image = global::DiversityCollection.Resource.DNAedit;
            this.buttonEditSequence.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonEditSequence.Location = new System.Drawing.Point(0, 110);
            this.buttonEditSequence.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditSequence.Name = "buttonEditSequence";
            this.buttonEditSequence.Size = new System.Drawing.Size(50, 20);
            this.buttonEditSequence.TabIndex = 21;
            this.buttonEditSequence.Text = "DNA";
            this.buttonEditSequence.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEditSequence.UseVisualStyleBackColor = true;
            this.buttonEditSequence.Visible = false;
            this.buttonEditSequence.Click += new System.EventHandler(this.buttonEditSequence_Click);
            // 
            // userControlRichEditSequence
            // 
            this.userControlRichEditSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichEditSequence.EditText = "";
            this.userControlRichEditSequence.Location = new System.Drawing.Point(50, 77);
            this.userControlRichEditSequence.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.userControlRichEditSequence.Name = "userControlRichEditSequence";
            this.userControlRichEditSequence.ReadOnly = false;
            this.tableLayoutPanelResults.SetRowSpan(this.userControlRichEditSequence, 2);
            this.userControlRichEditSequence.ShowMenu = false;
            this.userControlRichEditSequence.ShowStatus = true;
            this.userControlRichEditSequence.Size = new System.Drawing.Size(543, 50);
            this.userControlRichEditSequence.TabIndex = 22;
            this.userControlRichEditSequence.Visible = false;
            // 
            // labelAnalysisDate
            // 
            this.labelAnalysisDate.AccessibleName = "IdentificationUnitAnalysis.AnalysisDate";
            this.labelAnalysisDate.AutoSize = true;
            this.labelAnalysisDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAnalysisDate.Location = new System.Drawing.Point(179, 6);
            this.labelAnalysisDate.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelAnalysisDate.Name = "labelAnalysisDate";
            this.labelAnalysisDate.Size = new System.Drawing.Size(33, 18);
            this.labelAnalysisDate.TabIndex = 13;
            this.labelAnalysisDate.Text = "Date:";
            this.labelAnalysisDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dateTimePickerAnalysisDate
            // 
            this.dateTimePickerAnalysisDate.Location = new System.Drawing.Point(551, 3);
            this.dateTimePickerAnalysisDate.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerAnalysisDate.Name = "dateTimePickerAnalysisDate";
            this.dateTimePickerAnalysisDate.Size = new System.Drawing.Size(15, 20);
            this.dateTimePickerAnalysisDate.TabIndex = 15;
            this.dateTimePickerAnalysisDate.CloseUp += new System.EventHandler(this.dateTimePickerAnalysisDate_CloseUp);
            // 
            // textBoxAnalysisDate
            // 
            this.textBoxAnalysisDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnalysisDate.Location = new System.Drawing.Point(212, 3);
            this.textBoxAnalysisDate.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxAnalysisDate.Name = "textBoxAnalysisDate";
            this.textBoxAnalysisDate.Size = new System.Drawing.Size(339, 20);
            this.textBoxAnalysisDate.TabIndex = 14;
            this.textBoxAnalysisDate.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxAnalysisDate_Validating);
            // 
            // UserControl_Analysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxAnalysis);
            this.Name = "UserControl_Analysis";
            this.Size = new System.Drawing.Size(599, 279);
            this.groupBoxAnalysis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAnalysis)).EndInit();
            this.tableLayoutPanelAnalysis.ResumeLayout(false);
            this.tableLayoutPanelAnalysis.PerformLayout();
            this.tableLayoutPanelResults.ResumeLayout(false);
            this.tableLayoutPanelResults.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxAnalysis;
        private System.Windows.Forms.PictureBox pictureBoxAnalysis;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAnalysis;
        private System.Windows.Forms.Label labelAnalysisNumber;
        private System.Windows.Forms.TextBox textBoxAnalysisNumber;
        private System.Windows.Forms.Label labelAnalysisResult;
        private System.Windows.Forms.Label labelAnalysisURI;
        private System.Windows.Forms.Button buttonAnalysisURIOpen;
        private System.Windows.Forms.Label labelAnalysisNotes;
        private System.Windows.Forms.TextBox textBoxAnalysisNotes;
        private System.Windows.Forms.Button buttonAnalysisOpen;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryAnalysisResponsible;
        private System.Windows.Forms.Label labelAnalysisResponsible;
        private System.Windows.Forms.Label labelAnalysisDate;
        private System.Windows.Forms.TextBox textBoxAnalysisDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerAnalysisDate;
        private System.Windows.Forms.Label labelAnalysisSpecimenPart;
        private System.Windows.Forms.Label labelAnalysisMeasurementUnit;
        private System.Windows.Forms.TextBox textBoxAnalysisResult;
        private System.Windows.Forms.ComboBox comboBoxAnalysisResult;
        private System.Windows.Forms.ComboBox comboBoxAnalysisSpecimenPart;
        private System.Windows.Forms.TextBox textBoxAnalysisURI;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResults;
        private System.Windows.Forms.Button buttonEditSequence;
        private DiversityWorkbench.UserControls.UserControlRichEdit userControlRichEditSequence;
    }
}
