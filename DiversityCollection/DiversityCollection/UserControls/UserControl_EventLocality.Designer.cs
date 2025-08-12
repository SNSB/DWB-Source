namespace DiversityCollection.UserControls
{
    partial class UserControl_EventLocality
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_EventLocality));
            this.groupBoxEventLocalisation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelGeography = new System.Windows.Forms.TableLayoutPanel();
            this.labelLocalisationDate = new System.Windows.Forms.Label();
            this.textBoxLocationDirection = new System.Windows.Forms.TextBox();
            this.labelLocationDirection = new System.Windows.Forms.Label();
            this.textBoxLocationDistance = new System.Windows.Forms.TextBox();
            this.labelLocationDistance = new System.Windows.Forms.Label();
            this.textBoxLocationAccuracy = new System.Windows.Forms.TextBox();
            this.labelLocationAccuracy = new System.Windows.Forms.Label();
            this.textBoxGeographyDeterminationDate = new System.Windows.Forms.TextBox();
            this.textBoxLocationNotes = new System.Windows.Forms.TextBox();
            this.userControlModuleRelatedEntryLocalisationResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelLocationNotes = new System.Windows.Forms.Label();
            this.labelLocalisationResponsible = new System.Windows.Forms.Label();
            this.dateTimePickerLocalisationDate = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanelGeographyCache = new System.Windows.Forms.TableLayoutPanel();
            this.labelLongitudeCache = new System.Windows.Forms.Label();
            this.textBoxLongitudeCache = new System.Windows.Forms.TextBox();
            this.labelLatitudeCache = new System.Windows.Forms.Label();
            this.textBoxLatitudeCache = new System.Windows.Forms.TextBox();
            this.labelAltitudeCache = new System.Windows.Forms.Label();
            this.textBoxAltitudeCache = new System.Windows.Forms.TextBox();
            this.textBoxGeography = new System.Windows.Forms.TextBox();
            this.labelLocalisationRecordingMethod = new System.Windows.Forms.Label();
            this.comboBoxLocalisationRecordingMethod = new System.Windows.Forms.ComboBox();
            this.buttonEditGeography = new System.Windows.Forms.Button();
            this.tableLayoutPanelGeographyLocation = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelGeographyLocationStandard = new System.Windows.Forms.TableLayoutPanel();
            this.labelLocation1 = new System.Windows.Forms.Label();
            this.textBoxLocation1 = new System.Windows.Forms.TextBox();
            this.labelLocation2 = new System.Windows.Forms.Label();
            this.textBoxLocation2 = new System.Windows.Forms.TextBox();
            this.buttonGetCoordinatesFromGoogleMaps = new System.Windows.Forms.Button();
            this.buttonCoordinatesConvert = new System.Windows.Forms.Button();
            this.tableLayoutPanelGeographyLocationCustom = new System.Windows.Forms.TableLayoutPanel();
            this.labelLocation1Custom = new System.Windows.Forms.Label();
            this.labelLocation2Custom = new System.Windows.Forms.Label();
            this.textBoxLocation1a = new System.Windows.Forms.TextBox();
            this.textBoxLocation1b = new System.Windows.Forms.TextBox();
            this.textBoxLocation1c = new System.Windows.Forms.TextBox();
            this.textBoxLocation2a = new System.Windows.Forms.TextBox();
            this.textBoxLocation2b = new System.Windows.Forms.TextBox();
            this.textBoxLocation2c = new System.Windows.Forms.TextBox();
            this.buttonGeographyCustomSave = new System.Windows.Forms.Button();
            this.comboBoxLocation1a = new System.Windows.Forms.ComboBox();
            this.comboBoxLocation2a = new System.Windows.Forms.ComboBox();
            this.comboBoxGeographyLocationUnits = new System.Windows.Forms.ComboBox();
            this.userControlModuleRelatedEntryGazetteer = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.pictureBoxEventLocalisation = new System.Windows.Forms.PictureBox();
            this.userControlModuleRelatedEntrySamplingPlot = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.groupBoxEventLocalisation.SuspendLayout();
            this.tableLayoutPanelGeography.SuspendLayout();
            this.tableLayoutPanelGeographyCache.SuspendLayout();
            this.tableLayoutPanelGeographyLocation.SuspendLayout();
            this.tableLayoutPanelGeographyLocationStandard.SuspendLayout();
            this.tableLayoutPanelGeographyLocationCustom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEventLocalisation)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxEventLocalisation
            // 
            this.groupBoxEventLocalisation.AccessibleName = "CollectionEventLocalisation";
            this.groupBoxEventLocalisation.Controls.Add(this.tableLayoutPanelGeography);
            this.groupBoxEventLocalisation.Controls.Add(this.tableLayoutPanelGeographyLocation);
            this.groupBoxEventLocalisation.Controls.Add(this.userControlModuleRelatedEntryGazetteer);
            this.groupBoxEventLocalisation.Controls.Add(this.pictureBoxEventLocalisation);
            this.groupBoxEventLocalisation.Controls.Add(this.userControlModuleRelatedEntrySamplingPlot);
            this.groupBoxEventLocalisation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEventLocalisation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEventLocalisation.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBoxEventLocalisation.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEventLocalisation.MinimumSize = new System.Drawing.Size(0, 108);
            this.groupBoxEventLocalisation.Name = "groupBoxEventLocalisation";
            this.groupBoxEventLocalisation.Size = new System.Drawing.Size(512, 286);
            this.groupBoxEventLocalisation.TabIndex = 5;
            this.groupBoxEventLocalisation.TabStop = false;
            this.groupBoxEventLocalisation.Text = "Localisation of the collection event";
            // 
            // tableLayoutPanelGeography
            // 
            this.tableLayoutPanelGeography.ColumnCount = 9;
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeography.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeography.Controls.Add(this.labelLocalisationDate, 4, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.textBoxLocationDirection, 5, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.labelLocationDirection, 4, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.textBoxLocationDistance, 3, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.labelLocationDistance, 2, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.textBoxLocationAccuracy, 1, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.labelLocationAccuracy, 0, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.textBoxGeographyDeterminationDate, 5, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.textBoxLocationNotes, 1, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.userControlModuleRelatedEntryLocalisationResponsible, 1, 2);
            this.tableLayoutPanelGeography.Controls.Add(this.labelLocationNotes, 0, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.labelLocalisationResponsible, 0, 2);
            this.tableLayoutPanelGeography.Controls.Add(this.dateTimePickerLocalisationDate, 6, 1);
            this.tableLayoutPanelGeography.Controls.Add(this.tableLayoutPanelGeographyCache, 7, 0);
            this.tableLayoutPanelGeography.Controls.Add(this.labelLocalisationRecordingMethod, 0, 3);
            this.tableLayoutPanelGeography.Controls.Add(this.comboBoxLocalisationRecordingMethod, 1, 3);
            this.tableLayoutPanelGeography.Controls.Add(this.buttonEditGeography, 6, 0);
            this.tableLayoutPanelGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGeography.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelGeography.Location = new System.Drawing.Point(3, 85);
            this.tableLayoutPanelGeography.Name = "tableLayoutPanelGeography";
            this.tableLayoutPanelGeography.RowCount = 4;
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanelGeography.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeography.Size = new System.Drawing.Size(506, 198);
            this.tableLayoutPanelGeography.TabIndex = 2;
            // 
            // labelLocalisationDate
            // 
            this.labelLocalisationDate.AccessibleName = "CollectionEventLocalisation.DeterminationDate";
            this.labelLocalisationDate.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelLocalisationDate.Location = new System.Drawing.Point(267, 25);
            this.labelLocalisationDate.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelLocalisationDate.Name = "labelLocalisationDate";
            this.labelLocalisationDate.Size = new System.Drawing.Size(41, 19);
            this.labelLocalisationDate.TabIndex = 21;
            this.labelLocalisationDate.Text = "Date:";
            this.labelLocalisationDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLocationDirection
            // 
            this.textBoxLocationDirection.AccessibleName = "CollectionEventLocalisation.DirectionToLocation";
            this.textBoxLocationDirection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocationDirection.Location = new System.Drawing.Point(308, 0);
            this.textBoxLocationDirection.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxLocationDirection.Name = "textBoxLocationDirection";
            this.textBoxLocationDirection.Size = new System.Drawing.Size(77, 20);
            this.textBoxLocationDirection.TabIndex = 20;
            // 
            // labelLocationDirection
            // 
            this.labelLocationDirection.AccessibleName = "CollectionEventLocalisation.DirectionToLocation";
            this.labelLocationDirection.AutoSize = true;
            this.labelLocationDirection.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelLocationDirection.Location = new System.Drawing.Point(267, 3);
            this.labelLocationDirection.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelLocationDirection.Name = "labelLocationDirection";
            this.labelLocationDirection.Size = new System.Drawing.Size(41, 19);
            this.labelLocationDirection.TabIndex = 19;
            this.labelLocationDirection.Text = "Direct.:";
            this.labelLocationDirection.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLocationDistance
            // 
            this.textBoxLocationDistance.AccessibleName = "CollectionEventLocalisation.DistanceToLocation";
            this.textBoxLocationDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocationDistance.Location = new System.Drawing.Point(184, 0);
            this.textBoxLocationDistance.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxLocationDistance.Name = "textBoxLocationDistance";
            this.textBoxLocationDistance.Size = new System.Drawing.Size(77, 20);
            this.textBoxLocationDistance.TabIndex = 18;
            // 
            // labelLocationDistance
            // 
            this.labelLocationDistance.AccessibleName = "CollectionEventLocalisation.DistanceToLocation";
            this.labelLocationDistance.AutoSize = true;
            this.labelLocationDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocationDistance.Location = new System.Drawing.Point(153, 3);
            this.labelLocationDistance.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelLocationDistance.Name = "labelLocationDistance";
            this.labelLocationDistance.Size = new System.Drawing.Size(31, 19);
            this.labelLocationDistance.TabIndex = 17;
            this.labelLocationDistance.Text = "Dist.:";
            this.labelLocationDistance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxLocationAccuracy
            // 
            this.textBoxLocationAccuracy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocationAccuracy.Location = new System.Drawing.Point(70, 0);
            this.textBoxLocationAccuracy.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxLocationAccuracy.Name = "textBoxLocationAccuracy";
            this.textBoxLocationAccuracy.Size = new System.Drawing.Size(77, 20);
            this.textBoxLocationAccuracy.TabIndex = 16;
            // 
            // labelLocationAccuracy
            // 
            this.labelLocationAccuracy.AccessibleName = "CollectionEventLocalisation.LocationAccuracy";
            this.labelLocationAccuracy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocationAccuracy.Location = new System.Drawing.Point(3, 3);
            this.labelLocationAccuracy.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelLocationAccuracy.Name = "labelLocationAccuracy";
            this.labelLocationAccuracy.Size = new System.Drawing.Size(67, 19);
            this.labelLocationAccuracy.TabIndex = 6;
            this.labelLocationAccuracy.Text = "Accuracy:";
            this.labelLocationAccuracy.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxGeographyDeterminationDate
            // 
            this.textBoxGeographyDeterminationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxGeographyDeterminationDate.Location = new System.Drawing.Point(308, 22);
            this.textBoxGeographyDeterminationDate.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxGeographyDeterminationDate.Name = "textBoxGeographyDeterminationDate";
            this.textBoxGeographyDeterminationDate.Size = new System.Drawing.Size(80, 20);
            this.textBoxGeographyDeterminationDate.TabIndex = 24;
            this.textBoxGeographyDeterminationDate.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxGeographyDeterminationDate_Validating);
            // 
            // textBoxLocationNotes
            // 
            this.tableLayoutPanelGeography.SetColumnSpan(this.textBoxLocationNotes, 3);
            this.textBoxLocationNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocationNotes.Location = new System.Drawing.Point(70, 22);
            this.textBoxLocationNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxLocationNotes.Name = "textBoxLocationNotes";
            this.textBoxLocationNotes.Size = new System.Drawing.Size(191, 20);
            this.textBoxLocationNotes.TabIndex = 22;
            // 
            // userControlModuleRelatedEntryLocalisationResponsible
            // 
            this.userControlModuleRelatedEntryLocalisationResponsible.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelGeography.SetColumnSpan(this.userControlModuleRelatedEntryLocalisationResponsible, 6);
            this.userControlModuleRelatedEntryLocalisationResponsible.DependsOnUri = "";
            this.userControlModuleRelatedEntryLocalisationResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryLocalisationResponsible.Domain = "";
            this.userControlModuleRelatedEntryLocalisationResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryLocalisationResponsible.Location = new System.Drawing.Point(70, 44);
            this.userControlModuleRelatedEntryLocalisationResponsible.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.userControlModuleRelatedEntryLocalisationResponsible.Module = null;
            this.userControlModuleRelatedEntryLocalisationResponsible.Name = "userControlModuleRelatedEntryLocalisationResponsible";
            this.userControlModuleRelatedEntryLocalisationResponsible.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryLocalisationResponsible.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryLocalisationResponsible.ShowInfo = false;
            this.userControlModuleRelatedEntryLocalisationResponsible.Size = new System.Drawing.Size(332, 22);
            this.userControlModuleRelatedEntryLocalisationResponsible.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryLocalisationResponsible.TabIndex = 25;
            // 
            // labelLocationNotes
            // 
            this.labelLocationNotes.AccessibleName = "CollectionEventLocalisation.LocationNotes";
            this.labelLocationNotes.AutoSize = true;
            this.labelLocationNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocationNotes.Location = new System.Drawing.Point(3, 25);
            this.labelLocationNotes.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelLocationNotes.Name = "labelLocationNotes";
            this.labelLocationNotes.Size = new System.Drawing.Size(64, 19);
            this.labelLocationNotes.TabIndex = 23;
            this.labelLocationNotes.Text = "Notes:";
            this.labelLocationNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelLocalisationResponsible
            // 
            this.labelLocalisationResponsible.AccessibleName = "CollectionEventLocalisation.ResponsibleName";
            this.labelLocalisationResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocalisationResponsible.Location = new System.Drawing.Point(3, 47);
            this.labelLocalisationResponsible.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelLocalisationResponsible.Name = "labelLocalisationResponsible";
            this.labelLocalisationResponsible.Size = new System.Drawing.Size(67, 19);
            this.labelLocalisationResponsible.TabIndex = 15;
            this.labelLocalisationResponsible.Text = "Respons.:";
            this.labelLocalisationResponsible.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dateTimePickerLocalisationDate
            // 
            this.dateTimePickerLocalisationDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerLocalisationDate.Location = new System.Drawing.Point(388, 22);
            this.dateTimePickerLocalisationDate.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.dateTimePickerLocalisationDate.Name = "dateTimePickerLocalisationDate";
            this.dateTimePickerLocalisationDate.Size = new System.Drawing.Size(14, 20);
            this.dateTimePickerLocalisationDate.TabIndex = 32;
            this.dateTimePickerLocalisationDate.CloseUp += new System.EventHandler(this.dateTimePickerLocalisationDate_CloseUp);
            // 
            // tableLayoutPanelGeographyCache
            // 
            this.tableLayoutPanelGeographyCache.ColumnCount = 2;
            this.tableLayoutPanelGeography.SetColumnSpan(this.tableLayoutPanelGeographyCache, 2);
            this.tableLayoutPanelGeographyCache.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyCache.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeographyCache.Controls.Add(this.labelLongitudeCache, 0, 2);
            this.tableLayoutPanelGeographyCache.Controls.Add(this.textBoxLongitudeCache, 1, 2);
            this.tableLayoutPanelGeographyCache.Controls.Add(this.labelLatitudeCache, 0, 1);
            this.tableLayoutPanelGeographyCache.Controls.Add(this.textBoxLatitudeCache, 1, 1);
            this.tableLayoutPanelGeographyCache.Controls.Add(this.labelAltitudeCache, 0, 0);
            this.tableLayoutPanelGeographyCache.Controls.Add(this.textBoxAltitudeCache, 1, 0);
            this.tableLayoutPanelGeographyCache.Controls.Add(this.textBoxGeography, 0, 3);
            this.tableLayoutPanelGeographyCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGeographyCache.Location = new System.Drawing.Point(408, 3);
            this.tableLayoutPanelGeographyCache.Name = "tableLayoutPanelGeographyCache";
            this.tableLayoutPanelGeographyCache.RowCount = 4;
            this.tableLayoutPanelGeography.SetRowSpan(this.tableLayoutPanelGeographyCache, 4);
            this.tableLayoutPanelGeographyCache.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeographyCache.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeographyCache.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeographyCache.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeographyCache.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelGeographyCache.Size = new System.Drawing.Size(95, 192);
            this.tableLayoutPanelGeographyCache.TabIndex = 33;
            // 
            // labelLongitudeCache
            // 
            this.labelLongitudeCache.AccessibleName = "CollectionEventLocalisation.AverageLongitudeCache";
            this.labelLongitudeCache.AutoSize = true;
            this.labelLongitudeCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLongitudeCache.Location = new System.Drawing.Point(0, 26);
            this.labelLongitudeCache.Margin = new System.Windows.Forms.Padding(0);
            this.labelLongitudeCache.Name = "labelLongitudeCache";
            this.labelLongitudeCache.Size = new System.Drawing.Size(37, 13);
            this.labelLongitudeCache.TabIndex = 0;
            this.labelLongitudeCache.Text = "Long.:";
            // 
            // textBoxLongitudeCache
            // 
            this.textBoxLongitudeCache.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLongitudeCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLongitudeCache.Location = new System.Drawing.Point(37, 26);
            this.textBoxLongitudeCache.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxLongitudeCache.Name = "textBoxLongitudeCache";
            this.textBoxLongitudeCache.ReadOnly = true;
            this.textBoxLongitudeCache.Size = new System.Drawing.Size(58, 13);
            this.textBoxLongitudeCache.TabIndex = 31;
            // 
            // labelLatitudeCache
            // 
            this.labelLatitudeCache.AccessibleName = "CollectionEventLocalisation.AverageLatitudeCache";
            this.labelLatitudeCache.AutoSize = true;
            this.labelLatitudeCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLatitudeCache.Location = new System.Drawing.Point(0, 13);
            this.labelLatitudeCache.Margin = new System.Windows.Forms.Padding(0);
            this.labelLatitudeCache.Name = "labelLatitudeCache";
            this.labelLatitudeCache.Size = new System.Drawing.Size(37, 13);
            this.labelLatitudeCache.TabIndex = 28;
            this.labelLatitudeCache.Text = "Lat.:";
            this.labelLatitudeCache.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLatitudeCache
            // 
            this.textBoxLatitudeCache.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLatitudeCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLatitudeCache.Location = new System.Drawing.Point(37, 13);
            this.textBoxLatitudeCache.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxLatitudeCache.Name = "textBoxLatitudeCache";
            this.textBoxLatitudeCache.ReadOnly = true;
            this.textBoxLatitudeCache.Size = new System.Drawing.Size(58, 13);
            this.textBoxLatitudeCache.TabIndex = 29;
            // 
            // labelAltitudeCache
            // 
            this.labelAltitudeCache.AccessibleName = "CollectionEventLocalisation.AverageAltitudeCache";
            this.labelAltitudeCache.AutoSize = true;
            this.labelAltitudeCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAltitudeCache.Location = new System.Drawing.Point(0, 0);
            this.labelAltitudeCache.Margin = new System.Windows.Forms.Padding(0);
            this.labelAltitudeCache.Name = "labelAltitudeCache";
            this.labelAltitudeCache.Size = new System.Drawing.Size(37, 13);
            this.labelAltitudeCache.TabIndex = 26;
            this.labelAltitudeCache.Text = "Alt.:";
            this.labelAltitudeCache.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxAltitudeCache
            // 
            this.textBoxAltitudeCache.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxAltitudeCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAltitudeCache.Location = new System.Drawing.Point(37, 0);
            this.textBoxAltitudeCache.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxAltitudeCache.Name = "textBoxAltitudeCache";
            this.textBoxAltitudeCache.ReadOnly = true;
            this.textBoxAltitudeCache.Size = new System.Drawing.Size(58, 13);
            this.textBoxAltitudeCache.TabIndex = 27;
            // 
            // textBoxGeography
            // 
            this.textBoxGeography.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelGeographyCache.SetColumnSpan(this.textBoxGeography, 2);
            this.textBoxGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxGeography.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGeography.ForeColor = System.Drawing.Color.Blue;
            this.textBoxGeography.Location = new System.Drawing.Point(0, 39);
            this.textBoxGeography.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxGeography.Multiline = true;
            this.textBoxGeography.Name = "textBoxGeography";
            this.textBoxGeography.ReadOnly = true;
            this.textBoxGeography.Size = new System.Drawing.Size(95, 153);
            this.textBoxGeography.TabIndex = 32;
            // 
            // labelLocalisationRecordingMethod
            // 
            this.labelLocalisationRecordingMethod.AccessibleName = "CollectionEventLocalisation.RecordingMethod";
            this.labelLocalisationRecordingMethod.AutoSize = true;
            this.labelLocalisationRecordingMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocalisationRecordingMethod.Location = new System.Drawing.Point(3, 72);
            this.labelLocalisationRecordingMethod.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelLocalisationRecordingMethod.Name = "labelLocalisationRecordingMethod";
            this.labelLocalisationRecordingMethod.Size = new System.Drawing.Size(64, 126);
            this.labelLocalisationRecordingMethod.TabIndex = 34;
            this.labelLocalisationRecordingMethod.Text = "Method:";
            this.labelLocalisationRecordingMethod.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxLocalisationRecordingMethod
            // 
            this.tableLayoutPanelGeography.SetColumnSpan(this.comboBoxLocalisationRecordingMethod, 6);
            this.comboBoxLocalisationRecordingMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLocalisationRecordingMethod.FormattingEnabled = true;
            this.comboBoxLocalisationRecordingMethod.Location = new System.Drawing.Point(70, 66);
            this.comboBoxLocalisationRecordingMethod.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxLocalisationRecordingMethod.Name = "comboBoxLocalisationRecordingMethod";
            this.comboBoxLocalisationRecordingMethod.Size = new System.Drawing.Size(335, 21);
            this.comboBoxLocalisationRecordingMethod.TabIndex = 35;
            this.comboBoxLocalisationRecordingMethod.DropDown += new System.EventHandler(this.comboBoxLocalisationRecordingMethod_DropDown);
            // 
            // buttonEditGeography
            // 
            this.buttonEditGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEditGeography.FlatAppearance.BorderSize = 0;
            this.buttonEditGeography.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditGeography.Image = global::DiversityCollection.Resource.Edit1;
            this.buttonEditGeography.Location = new System.Drawing.Point(388, 0);
            this.buttonEditGeography.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditGeography.Name = "buttonEditGeography";
            this.buttonEditGeography.Size = new System.Drawing.Size(17, 22);
            this.buttonEditGeography.TabIndex = 36;
            this.toolTip.SetToolTip(this.buttonEditGeography, "Edit geography");
            this.buttonEditGeography.UseVisualStyleBackColor = true;
            this.buttonEditGeography.Click += new System.EventHandler(this.buttonEditGeography_Click);
            // 
            // tableLayoutPanelGeographyLocation
            // 
            this.tableLayoutPanelGeographyLocation.ColumnCount = 2;
            this.tableLayoutPanelGeographyLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeographyLocation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocation.Controls.Add(this.tableLayoutPanelGeographyLocationStandard, 0, 0);
            this.tableLayoutPanelGeographyLocation.Controls.Add(this.tableLayoutPanelGeographyLocationCustom, 0, 1);
            this.tableLayoutPanelGeographyLocation.Controls.Add(this.comboBoxGeographyLocationUnits, 1, 0);
            this.tableLayoutPanelGeographyLocation.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelGeographyLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelGeographyLocation.Location = new System.Drawing.Point(3, 60);
            this.tableLayoutPanelGeographyLocation.Name = "tableLayoutPanelGeographyLocation";
            this.tableLayoutPanelGeographyLocation.RowCount = 2;
            this.tableLayoutPanelGeographyLocation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeographyLocation.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeographyLocation.Size = new System.Drawing.Size(506, 25);
            this.tableLayoutPanelGeographyLocation.TabIndex = 3;
            // 
            // tableLayoutPanelGeographyLocationStandard
            // 
            this.tableLayoutPanelGeographyLocationStandard.ColumnCount = 6;
            this.tableLayoutPanelGeographyLocationStandard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelGeographyLocationStandard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelGeographyLocationStandard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationStandard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelGeographyLocationStandard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationStandard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationStandard.Controls.Add(this.labelLocation1, 0, 0);
            this.tableLayoutPanelGeographyLocationStandard.Controls.Add(this.textBoxLocation1, 1, 0);
            this.tableLayoutPanelGeographyLocationStandard.Controls.Add(this.labelLocation2, 2, 0);
            this.tableLayoutPanelGeographyLocationStandard.Controls.Add(this.textBoxLocation2, 3, 0);
            this.tableLayoutPanelGeographyLocationStandard.Controls.Add(this.buttonGetCoordinatesFromGoogleMaps, 4, 0);
            this.tableLayoutPanelGeographyLocationStandard.Controls.Add(this.buttonCoordinatesConvert, 5, 0);
            this.tableLayoutPanelGeographyLocationStandard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGeographyLocationStandard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelGeographyLocationStandard.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelGeographyLocationStandard.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelGeographyLocationStandard.Name = "tableLayoutPanelGeographyLocationStandard";
            this.tableLayoutPanelGeographyLocationStandard.RowCount = 1;
            this.tableLayoutPanelGeographyLocationStandard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeographyLocationStandard.Size = new System.Drawing.Size(428, 28);
            this.tableLayoutPanelGeographyLocationStandard.TabIndex = 0;
            // 
            // labelLocation1
            // 
            this.labelLocation1.AutoSize = true;
            this.labelLocation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocation1.Location = new System.Drawing.Point(3, 0);
            this.labelLocation1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelLocation1.Name = "labelLocation1";
            this.labelLocation1.Size = new System.Drawing.Size(67, 28);
            this.labelLocation1.TabIndex = 0;
            this.labelLocation1.Text = "Location 1:";
            this.labelLocation1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLocation1
            // 
            this.textBoxLocation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation1.Location = new System.Drawing.Point(70, 3);
            this.textBoxLocation1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxLocation1.Name = "textBoxLocation1";
            this.textBoxLocation1.Size = new System.Drawing.Size(119, 20);
            this.textBoxLocation1.TabIndex = 1;
            this.textBoxLocation1.Leave += new System.EventHandler(this.textBoxLocation1_Leave);
            // 
            // labelLocation2
            // 
            this.labelLocation2.AutoSize = true;
            this.labelLocation2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLocation2.Location = new System.Drawing.Point(195, 0);
            this.labelLocation2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelLocation2.Name = "labelLocation2";
            this.labelLocation2.Size = new System.Drawing.Size(60, 28);
            this.labelLocation2.TabIndex = 2;
            this.labelLocation2.Text = "Location 2:";
            this.labelLocation2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLocation2
            // 
            this.textBoxLocation2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation2.Location = new System.Drawing.Point(255, 3);
            this.textBoxLocation2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxLocation2.Name = "textBoxLocation2";
            this.textBoxLocation2.Size = new System.Drawing.Size(119, 20);
            this.textBoxLocation2.TabIndex = 3;
            this.textBoxLocation2.Leave += new System.EventHandler(this.textBoxLocation2_Leave);
            // 
            // buttonGetCoordinatesFromGoogleMaps
            // 
            this.buttonGetCoordinatesFromGoogleMaps.Image = global::DiversityCollection.Resource.CoordinateCross;
            this.buttonGetCoordinatesFromGoogleMaps.Location = new System.Drawing.Point(377, 2);
            this.buttonGetCoordinatesFromGoogleMaps.Margin = new System.Windows.Forms.Padding(0, 2, 0, 3);
            this.buttonGetCoordinatesFromGoogleMaps.Name = "buttonGetCoordinatesFromGoogleMaps";
            this.buttonGetCoordinatesFromGoogleMaps.Size = new System.Drawing.Size(23, 23);
            this.buttonGetCoordinatesFromGoogleMaps.TabIndex = 4;
            this.buttonGetCoordinatesFromGoogleMaps.UseVisualStyleBackColor = true;
            this.buttonGetCoordinatesFromGoogleMaps.Click += new System.EventHandler(this.buttonGetCoordinatesFromGoogleMaps_Click);
            // 
            // buttonCoordinatesConvert
            // 
            this.buttonCoordinatesConvert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCoordinatesConvert.Image = global::DiversityCollection.Resource.Replace;
            this.buttonCoordinatesConvert.Location = new System.Drawing.Point(400, 1);
            this.buttonCoordinatesConvert.Margin = new System.Windows.Forms.Padding(0, 1, 3, 3);
            this.buttonCoordinatesConvert.Name = "buttonCoordinatesConvert";
            this.buttonCoordinatesConvert.Size = new System.Drawing.Size(25, 24);
            this.buttonCoordinatesConvert.TabIndex = 5;
            this.buttonCoordinatesConvert.UseVisualStyleBackColor = true;
            this.buttonCoordinatesConvert.Visible = false;
            this.buttonCoordinatesConvert.Click += new System.EventHandler(this.buttonCoordinatesConvert_Click);
            // 
            // tableLayoutPanelGeographyLocationCustom
            // 
            this.tableLayoutPanelGeographyLocationCustom.ColumnCount = 11;
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationCustom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.labelLocation1Custom, 0, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.labelLocation2Custom, 5, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.textBoxLocation1a, 1, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.textBoxLocation1b, 2, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.textBoxLocation1c, 3, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.textBoxLocation2a, 6, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.textBoxLocation2b, 7, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.textBoxLocation2c, 8, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.buttonGeographyCustomSave, 10, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.comboBoxLocation1a, 4, 0);
            this.tableLayoutPanelGeographyLocationCustom.Controls.Add(this.comboBoxLocation2a, 9, 0);
            this.tableLayoutPanelGeographyLocationCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGeographyLocationCustom.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanelGeographyLocationCustom.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelGeographyLocationCustom.Name = "tableLayoutPanelGeographyLocationCustom";
            this.tableLayoutPanelGeographyLocationCustom.RowCount = 1;
            this.tableLayoutPanelGeographyLocationCustom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeographyLocationCustom.Size = new System.Drawing.Size(428, 46);
            this.tableLayoutPanelGeographyLocationCustom.TabIndex = 1;
            this.tableLayoutPanelGeographyLocationCustom.Visible = false;
            // 
            // labelLocation1Custom
            // 
            this.labelLocation1Custom.AutoSize = true;
            this.labelLocation1Custom.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLocation1Custom.Location = new System.Drawing.Point(3, 6);
            this.labelLocation1Custom.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelLocation1Custom.Name = "labelLocation1Custom";
            this.labelLocation1Custom.Size = new System.Drawing.Size(67, 13);
            this.labelLocation1Custom.TabIndex = 0;
            this.labelLocation1Custom.Text = "Location 1:";
            this.labelLocation1Custom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLocation2Custom
            // 
            this.labelLocation2Custom.AutoSize = true;
            this.labelLocation2Custom.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLocation2Custom.Location = new System.Drawing.Point(208, 6);
            this.labelLocation2Custom.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelLocation2Custom.Name = "labelLocation2Custom";
            this.labelLocation2Custom.Size = new System.Drawing.Size(60, 13);
            this.labelLocation2Custom.TabIndex = 1;
            this.labelLocation2Custom.Text = "Location 2:";
            this.labelLocation2Custom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLocation1a
            // 
            this.textBoxLocation1a.BackColor = System.Drawing.Color.Aquamarine;
            this.textBoxLocation1a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation1a.Location = new System.Drawing.Point(70, 3);
            this.textBoxLocation1a.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxLocation1a.MinimumSize = new System.Drawing.Size(21, 4);
            this.textBoxLocation1a.Name = "textBoxLocation1a";
            this.textBoxLocation1a.Size = new System.Drawing.Size(69, 20);
            this.textBoxLocation1a.TabIndex = 2;
            this.textBoxLocation1a.Tag = "";
            // 
            // textBoxLocation1b
            // 
            this.textBoxLocation1b.BackColor = System.Drawing.Color.Aquamarine;
            this.textBoxLocation1b.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation1b.Location = new System.Drawing.Point(139, 3);
            this.textBoxLocation1b.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxLocation1b.MinimumSize = new System.Drawing.Size(18, 4);
            this.textBoxLocation1b.Name = "textBoxLocation1b";
            this.textBoxLocation1b.Size = new System.Drawing.Size(18, 20);
            this.textBoxLocation1b.TabIndex = 3;
            // 
            // textBoxLocation1c
            // 
            this.textBoxLocation1c.BackColor = System.Drawing.Color.Aquamarine;
            this.textBoxLocation1c.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation1c.Location = new System.Drawing.Point(157, 3);
            this.textBoxLocation1c.Margin = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.textBoxLocation1c.Name = "textBoxLocation1c";
            this.textBoxLocation1c.Size = new System.Drawing.Size(27, 20);
            this.textBoxLocation1c.TabIndex = 4;
            // 
            // textBoxLocation2a
            // 
            this.textBoxLocation2a.BackColor = System.Drawing.Color.Aquamarine;
            this.textBoxLocation2a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation2a.Location = new System.Drawing.Point(268, 3);
            this.textBoxLocation2a.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxLocation2a.MinimumSize = new System.Drawing.Size(21, 4);
            this.textBoxLocation2a.Name = "textBoxLocation2a";
            this.textBoxLocation2a.Size = new System.Drawing.Size(69, 20);
            this.textBoxLocation2a.TabIndex = 5;
            // 
            // textBoxLocation2b
            // 
            this.textBoxLocation2b.BackColor = System.Drawing.Color.Aquamarine;
            this.textBoxLocation2b.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation2b.Location = new System.Drawing.Point(337, 3);
            this.textBoxLocation2b.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxLocation2b.MinimumSize = new System.Drawing.Size(18, 4);
            this.textBoxLocation2b.Name = "textBoxLocation2b";
            this.textBoxLocation2b.Size = new System.Drawing.Size(18, 20);
            this.textBoxLocation2b.TabIndex = 6;
            // 
            // textBoxLocation2c
            // 
            this.textBoxLocation2c.BackColor = System.Drawing.Color.Aquamarine;
            this.textBoxLocation2c.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLocation2c.Location = new System.Drawing.Point(355, 3);
            this.textBoxLocation2c.Margin = new System.Windows.Forms.Padding(0, 3, 1, 3);
            this.textBoxLocation2c.Name = "textBoxLocation2c";
            this.textBoxLocation2c.Size = new System.Drawing.Size(27, 20);
            this.textBoxLocation2c.TabIndex = 7;
            // 
            // buttonGeographyCustomSave
            // 
            this.buttonGeographyCustomSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonGeographyCustomSave.Image = global::DiversityCollection.Resource.Save;
            this.buttonGeographyCustomSave.Location = new System.Drawing.Point(403, 2);
            this.buttonGeographyCustomSave.Margin = new System.Windows.Forms.Padding(0, 2, 0, 1);
            this.buttonGeographyCustomSave.Name = "buttonGeographyCustomSave";
            this.buttonGeographyCustomSave.Padding = new System.Windows.Forms.Padding(0, 0, 2, 2);
            this.buttonGeographyCustomSave.Size = new System.Drawing.Size(25, 23);
            this.buttonGeographyCustomSave.TabIndex = 8;
            this.buttonGeographyCustomSave.UseVisualStyleBackColor = true;
            this.buttonGeographyCustomSave.Click += new System.EventHandler(this.buttonGeographyCustomSave_Click);
            // 
            // comboBoxLocation1a
            // 
            this.comboBoxLocation1a.BackColor = System.Drawing.Color.Aquamarine;
            this.comboBoxLocation1a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLocation1a.FormattingEnabled = true;
            this.comboBoxLocation1a.Location = new System.Drawing.Point(185, 3);
            this.comboBoxLocation1a.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.comboBoxLocation1a.Name = "comboBoxLocation1a";
            this.comboBoxLocation1a.Size = new System.Drawing.Size(20, 21);
            this.comboBoxLocation1a.TabIndex = 9;
            this.comboBoxLocation1a.Visible = false;
            // 
            // comboBoxLocation2a
            // 
            this.comboBoxLocation2a.BackColor = System.Drawing.Color.Aquamarine;
            this.comboBoxLocation2a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxLocation2a.FormattingEnabled = true;
            this.comboBoxLocation2a.Location = new System.Drawing.Point(383, 3);
            this.comboBoxLocation2a.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.comboBoxLocation2a.Name = "comboBoxLocation2a";
            this.comboBoxLocation2a.Size = new System.Drawing.Size(20, 21);
            this.comboBoxLocation2a.TabIndex = 10;
            this.comboBoxLocation2a.Visible = false;
            // 
            // comboBoxGeographyLocationUnits
            // 
            this.comboBoxGeographyLocationUnits.FormattingEnabled = true;
            this.comboBoxGeographyLocationUnits.Location = new System.Drawing.Point(431, 3);
            this.comboBoxGeographyLocationUnits.Name = "comboBoxGeographyLocationUnits";
            this.tableLayoutPanelGeographyLocation.SetRowSpan(this.comboBoxGeographyLocationUnits, 2);
            this.comboBoxGeographyLocationUnits.Size = new System.Drawing.Size(72, 21);
            this.comboBoxGeographyLocationUnits.TabIndex = 2;
            this.comboBoxGeographyLocationUnits.SelectedIndexChanged += new System.EventHandler(this.comboBoxGeographyLocationUnits_SelectedIndexChanged);
            // 
            // userControlModuleRelatedEntryGazetteer
            // 
            this.userControlModuleRelatedEntryGazetteer.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryGazetteer.DependsOnUri = "";
            this.userControlModuleRelatedEntryGazetteer.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlModuleRelatedEntryGazetteer.Domain = "";
            this.userControlModuleRelatedEntryGazetteer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlModuleRelatedEntryGazetteer.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryGazetteer.Location = new System.Drawing.Point(3, 38);
            this.userControlModuleRelatedEntryGazetteer.Module = null;
            this.userControlModuleRelatedEntryGazetteer.Name = "userControlModuleRelatedEntryGazetteer";
            this.userControlModuleRelatedEntryGazetteer.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryGazetteer.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryGazetteer.ShowInfo = false;
            this.userControlModuleRelatedEntryGazetteer.Size = new System.Drawing.Size(506, 22);
            this.userControlModuleRelatedEntryGazetteer.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryGazetteer.TabIndex = 5;
            // 
            // pictureBoxEventLocalisation
            // 
            this.pictureBoxEventLocalisation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxEventLocalisation.Image = global::DiversityCollection.Resource.Localisation;
            this.pictureBoxEventLocalisation.Location = new System.Drawing.Point(492, 1);
            this.pictureBoxEventLocalisation.Name = "pictureBoxEventLocalisation";
            this.pictureBoxEventLocalisation.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxEventLocalisation.TabIndex = 4;
            this.pictureBoxEventLocalisation.TabStop = false;
            // 
            // userControlModuleRelatedEntrySamplingPlot
            // 
            this.userControlModuleRelatedEntrySamplingPlot.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntrySamplingPlot.DependsOnUri = "";
            this.userControlModuleRelatedEntrySamplingPlot.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlModuleRelatedEntrySamplingPlot.Domain = "";
            this.userControlModuleRelatedEntrySamplingPlot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlModuleRelatedEntrySamplingPlot.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntrySamplingPlot.Location = new System.Drawing.Point(3, 16);
            this.userControlModuleRelatedEntrySamplingPlot.Module = null;
            this.userControlModuleRelatedEntrySamplingPlot.Name = "userControlModuleRelatedEntrySamplingPlot";
            this.userControlModuleRelatedEntrySamplingPlot.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntrySamplingPlot.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntrySamplingPlot.ShowInfo = false;
            this.userControlModuleRelatedEntrySamplingPlot.Size = new System.Drawing.Size(506, 22);
            this.userControlModuleRelatedEntrySamplingPlot.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntrySamplingPlot.TabIndex = 6;
            // 
            // UserControl_EventLocality
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxEventLocalisation);
            this.Name = "UserControl_EventLocality";
            this.Size = new System.Drawing.Size(512, 286);
            this.groupBoxEventLocalisation.ResumeLayout(false);
            this.tableLayoutPanelGeography.ResumeLayout(false);
            this.tableLayoutPanelGeography.PerformLayout();
            this.tableLayoutPanelGeographyCache.ResumeLayout(false);
            this.tableLayoutPanelGeographyCache.PerformLayout();
            this.tableLayoutPanelGeographyLocation.ResumeLayout(false);
            this.tableLayoutPanelGeographyLocationStandard.ResumeLayout(false);
            this.tableLayoutPanelGeographyLocationStandard.PerformLayout();
            this.tableLayoutPanelGeographyLocationCustom.ResumeLayout(false);
            this.tableLayoutPanelGeographyLocationCustom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEventLocalisation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxEventLocalisation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeography;
        private System.Windows.Forms.Label labelLocalisationDate;
        private System.Windows.Forms.TextBox textBoxLocationDirection;
        private System.Windows.Forms.Label labelLocationDirection;
        private System.Windows.Forms.TextBox textBoxLocationDistance;
        private System.Windows.Forms.Label labelLocationDistance;
        private System.Windows.Forms.TextBox textBoxLocationAccuracy;
        private System.Windows.Forms.Label labelLocationAccuracy;
        private System.Windows.Forms.TextBox textBoxGeographyDeterminationDate;
        private System.Windows.Forms.TextBox textBoxLocationNotes;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryLocalisationResponsible;
        private System.Windows.Forms.Label labelLocationNotes;
        private System.Windows.Forms.Label labelLocalisationResponsible;
        private System.Windows.Forms.DateTimePicker dateTimePickerLocalisationDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeographyCache;
        private System.Windows.Forms.Label labelLongitudeCache;
        private System.Windows.Forms.TextBox textBoxLongitudeCache;
        private System.Windows.Forms.Label labelLatitudeCache;
        private System.Windows.Forms.TextBox textBoxLatitudeCache;
        private System.Windows.Forms.Label labelAltitudeCache;
        private System.Windows.Forms.TextBox textBoxAltitudeCache;
        private System.Windows.Forms.TextBox textBoxGeography;
        private System.Windows.Forms.Label labelLocalisationRecordingMethod;
        private System.Windows.Forms.ComboBox comboBoxLocalisationRecordingMethod;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeographyLocation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeographyLocationStandard;
        private System.Windows.Forms.Label labelLocation1;
        private System.Windows.Forms.TextBox textBoxLocation1;
        private System.Windows.Forms.Label labelLocation2;
        private System.Windows.Forms.TextBox textBoxLocation2;
        private System.Windows.Forms.Button buttonGetCoordinatesFromGoogleMaps;
        private System.Windows.Forms.Button buttonCoordinatesConvert;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeographyLocationCustom;
        private System.Windows.Forms.Label labelLocation1Custom;
        private System.Windows.Forms.Label labelLocation2Custom;
        private System.Windows.Forms.TextBox textBoxLocation1a;
        private System.Windows.Forms.TextBox textBoxLocation1b;
        private System.Windows.Forms.TextBox textBoxLocation1c;
        private System.Windows.Forms.TextBox textBoxLocation2a;
        private System.Windows.Forms.TextBox textBoxLocation2b;
        private System.Windows.Forms.TextBox textBoxLocation2c;
        private System.Windows.Forms.Button buttonGeographyCustomSave;
        private System.Windows.Forms.ComboBox comboBoxLocation1a;
        private System.Windows.Forms.ComboBox comboBoxLocation2a;
        private System.Windows.Forms.ComboBox comboBoxGeographyLocationUnits;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryGazetteer;
        private System.Windows.Forms.PictureBox pictureBoxEventLocalisation;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntrySamplingPlot;
        private System.Windows.Forms.Button buttonEditGeography;
    }
}
