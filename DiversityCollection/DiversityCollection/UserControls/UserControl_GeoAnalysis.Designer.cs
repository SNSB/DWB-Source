namespace DiversityCollection.UserControls
{
    partial class UserControl_GeoAnalysis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_GeoAnalysis));
            this.groupBoxGeoAnalysis = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelGeoAnalysis = new System.Windows.Forms.TableLayoutPanel();
            this.labelGeography = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryGeoAnalysisResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelGeoAnalysisResponsible = new System.Windows.Forms.Label();
            this.labelGeoAnalysisNotes = new System.Windows.Forms.Label();
            this.textBoxGeoAnalysisNotes = new System.Windows.Forms.TextBox();
            this.labelGeoAnalysisDate = new System.Windows.Forms.Label();
            this.dateTimePickerGeoAnalysisDate = new System.Windows.Forms.DateTimePicker();
            this.buttonGeoAnalysisSet = new System.Windows.Forms.Button();
            this.textBoxGeoAnalysisGeography = new System.Windows.Forms.TextBox();
            this.labelLat = new System.Windows.Forms.Label();
            this.labelLong = new System.Windows.Forms.Label();
            this.labelLongitude = new System.Windows.Forms.Label();
            this.labelLatitude = new System.Windows.Forms.Label();
            this.buttonEditGeography = new System.Windows.Forms.Button();
            this.groupBoxGeoAnalysis.SuspendLayout();
            this.tableLayoutPanelGeoAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxGeoAnalysis
            // 
            this.groupBoxGeoAnalysis.AccessibleName = "IdentificationUnitGeoAnalysis";
            this.groupBoxGeoAnalysis.Controls.Add(this.tableLayoutPanelGeoAnalysis);
            this.groupBoxGeoAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxGeoAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxGeoAnalysis.ForeColor = System.Drawing.Color.Blue;
            this.groupBoxGeoAnalysis.Location = new System.Drawing.Point(0, 0);
            this.groupBoxGeoAnalysis.MinimumSize = new System.Drawing.Size(0, 92);
            this.groupBoxGeoAnalysis.Name = "groupBoxGeoAnalysis";
            this.groupBoxGeoAnalysis.Size = new System.Drawing.Size(674, 211);
            this.groupBoxGeoAnalysis.TabIndex = 1;
            this.groupBoxGeoAnalysis.TabStop = false;
            this.groupBoxGeoAnalysis.Text = "Geo analysis";
            // 
            // tableLayoutPanelGeoAnalysis
            // 
            this.tableLayoutPanelGeoAnalysis.ColumnCount = 5;
            this.tableLayoutPanelGeoAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeoAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeoAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeoAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeoAnalysis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.labelGeography, 0, 2);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.userControlModuleRelatedEntryGeoAnalysisResponsible, 1, 5);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.labelGeoAnalysisResponsible, 0, 5);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.labelGeoAnalysisNotes, 0, 4);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.textBoxGeoAnalysisNotes, 1, 4);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.labelGeoAnalysisDate, 0, 0);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.dateTimePickerGeoAnalysisDate, 1, 0);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.buttonGeoAnalysisSet, 4, 0);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.textBoxGeoAnalysisGeography, 1, 2);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.labelLat, 2, 0);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.labelLong, 2, 1);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.labelLongitude, 3, 1);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.labelLatitude, 3, 0);
            this.tableLayoutPanelGeoAnalysis.Controls.Add(this.buttonEditGeography, 0, 3);
            this.tableLayoutPanelGeoAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelGeoAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelGeoAnalysis.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanelGeoAnalysis.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelGeoAnalysis.Name = "tableLayoutPanelGeoAnalysis";
            this.tableLayoutPanelGeoAnalysis.RowCount = 6;
            this.tableLayoutPanelGeoAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeoAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeoAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeoAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeoAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelGeoAnalysis.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelGeoAnalysis.Size = new System.Drawing.Size(668, 192);
            this.tableLayoutPanelGeoAnalysis.TabIndex = 0;
            // 
            // labelGeography
            // 
            this.labelGeography.AccessibleName = "IdentificationUnitGeoAnalysis.Geography";
            this.labelGeography.AutoSize = true;
            this.labelGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeography.ForeColor = System.Drawing.Color.Blue;
            this.labelGeography.Location = new System.Drawing.Point(3, 32);
            this.labelGeography.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelGeography.Name = "labelGeography";
            this.labelGeography.Size = new System.Drawing.Size(62, 13);
            this.labelGeography.TabIndex = 24;
            this.labelGeography.Text = "Geography:";
            this.labelGeography.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userControlModuleRelatedEntryGeoAnalysisResponsible
            // 
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelGeoAnalysis.SetColumnSpan(this.userControlModuleRelatedEntryGeoAnalysisResponsible, 4);
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.DependsOnUri = "";
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.Domain = "";
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.ForeColor = System.Drawing.Color.Blue;
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.Location = new System.Drawing.Point(65, 169);
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.Module = null;
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.Name = "userControlModuleRelatedEntryGeoAnalysisResponsible";
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.ShowInfo = false;
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.Size = new System.Drawing.Size(603, 22);
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryGeoAnalysisResponsible.TabIndex = 14;
            // 
            // labelGeoAnalysisResponsible
            // 
            this.labelGeoAnalysisResponsible.AccessibleName = "IdentificationUnitGeoAnalysis.ResponsibleName";
            this.labelGeoAnalysisResponsible.AutoSize = true;
            this.labelGeoAnalysisResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoAnalysisResponsible.ForeColor = System.Drawing.Color.Blue;
            this.labelGeoAnalysisResponsible.Location = new System.Drawing.Point(3, 175);
            this.labelGeoAnalysisResponsible.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelGeoAnalysisResponsible.Name = "labelGeoAnalysisResponsible";
            this.labelGeoAnalysisResponsible.Size = new System.Drawing.Size(62, 17);
            this.labelGeoAnalysisResponsible.TabIndex = 13;
            this.labelGeoAnalysisResponsible.Text = "Respons.:";
            this.labelGeoAnalysisResponsible.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelGeoAnalysisNotes
            // 
            this.labelGeoAnalysisNotes.AccessibleName = "IdentificationUnitGeoAnalysis.Notes";
            this.labelGeoAnalysisNotes.AutoSize = true;
            this.labelGeoAnalysisNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoAnalysisNotes.ForeColor = System.Drawing.Color.Blue;
            this.labelGeoAnalysisNotes.Location = new System.Drawing.Point(3, 78);
            this.labelGeoAnalysisNotes.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelGeoAnalysisNotes.Name = "labelGeoAnalysisNotes";
            this.labelGeoAnalysisNotes.Size = new System.Drawing.Size(62, 91);
            this.labelGeoAnalysisNotes.TabIndex = 15;
            this.labelGeoAnalysisNotes.Text = "Notes:";
            this.labelGeoAnalysisNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxGeoAnalysisNotes
            // 
            this.textBoxGeoAnalysisNotes.AcceptsTab = true;
            this.tableLayoutPanelGeoAnalysis.SetColumnSpan(this.textBoxGeoAnalysisNotes, 4);
            this.textBoxGeoAnalysisNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxGeoAnalysisNotes.Location = new System.Drawing.Point(65, 75);
            this.textBoxGeoAnalysisNotes.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxGeoAnalysisNotes.Multiline = true;
            this.textBoxGeoAnalysisNotes.Name = "textBoxGeoAnalysisNotes";
            this.textBoxGeoAnalysisNotes.Size = new System.Drawing.Size(600, 91);
            this.textBoxGeoAnalysisNotes.TabIndex = 16;
            // 
            // labelGeoAnalysisDate
            // 
            this.labelGeoAnalysisDate.AccessibleName = "IdentificationUnitGeoAnalysis.AnalysisDate";
            this.labelGeoAnalysisDate.AutoSize = true;
            this.labelGeoAnalysisDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGeoAnalysisDate.ForeColor = System.Drawing.Color.Blue;
            this.labelGeoAnalysisDate.Location = new System.Drawing.Point(3, 0);
            this.labelGeoAnalysisDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelGeoAnalysisDate.Name = "labelGeoAnalysisDate";
            this.tableLayoutPanelGeoAnalysis.SetRowSpan(this.labelGeoAnalysisDate, 2);
            this.labelGeoAnalysisDate.Size = new System.Drawing.Size(62, 29);
            this.labelGeoAnalysisDate.TabIndex = 17;
            this.labelGeoAnalysisDate.Text = "Date:";
            this.labelGeoAnalysisDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerGeoAnalysisDate
            // 
            this.dateTimePickerGeoAnalysisDate.CustomFormat = "yyyy.MM.dd HH:mm:ss";
            this.dateTimePickerGeoAnalysisDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerGeoAnalysisDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerGeoAnalysisDate.Location = new System.Drawing.Point(65, 3);
            this.dateTimePickerGeoAnalysisDate.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.dateTimePickerGeoAnalysisDate.Name = "dateTimePickerGeoAnalysisDate";
            this.tableLayoutPanelGeoAnalysis.SetRowSpan(this.dateTimePickerGeoAnalysisDate, 2);
            this.dateTimePickerGeoAnalysisDate.Size = new System.Drawing.Size(140, 20);
            this.dateTimePickerGeoAnalysisDate.TabIndex = 19;
            // 
            // buttonGeoAnalysisSet
            // 
            this.buttonGeoAnalysisSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGeoAnalysisSet.Image = global::DiversityCollection.Resource.GeoAnalysis;
            this.buttonGeoAnalysisSet.Location = new System.Drawing.Point(642, 3);
            this.buttonGeoAnalysisSet.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonGeoAnalysisSet.Name = "buttonGeoAnalysisSet";
            this.tableLayoutPanelGeoAnalysis.SetRowSpan(this.buttonGeoAnalysisSet, 2);
            this.buttonGeoAnalysisSet.Size = new System.Drawing.Size(23, 23);
            this.buttonGeoAnalysisSet.TabIndex = 21;
            this.toolTip.SetToolTip(this.buttonGeoAnalysisSet, "Set the geography");
            this.buttonGeoAnalysisSet.UseVisualStyleBackColor = true;
            this.buttonGeoAnalysisSet.Click += new System.EventHandler(this.buttonGeoAnalysisSet_Click);
            // 
            // textBoxGeoAnalysisGeography
            // 
            this.textBoxGeoAnalysisGeography.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelGeoAnalysis.SetColumnSpan(this.textBoxGeoAnalysisGeography, 4);
            this.textBoxGeoAnalysisGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxGeoAnalysisGeography.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.textBoxGeoAnalysisGeography.Location = new System.Drawing.Point(68, 32);
            this.textBoxGeoAnalysisGeography.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxGeoAnalysisGeography.Multiline = true;
            this.textBoxGeoAnalysisGeography.Name = "textBoxGeoAnalysisGeography";
            this.textBoxGeoAnalysisGeography.ReadOnly = true;
            this.tableLayoutPanelGeoAnalysis.SetRowSpan(this.textBoxGeoAnalysisGeography, 2);
            this.textBoxGeoAnalysisGeography.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxGeoAnalysisGeography.Size = new System.Drawing.Size(597, 40);
            this.textBoxGeoAnalysisGeography.TabIndex = 23;
            this.textBoxGeoAnalysisGeography.Text = "Lat/Long";
            this.textBoxGeoAnalysisGeography.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelLat
            // 
            this.labelLat.AutoSize = true;
            this.labelLat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLat.ForeColor = System.Drawing.Color.Blue;
            this.labelLat.Location = new System.Drawing.Point(211, 0);
            this.labelLat.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelLat.Name = "labelLat";
            this.labelLat.Size = new System.Drawing.Size(37, 13);
            this.labelLat.TabIndex = 25;
            this.labelLat.Text = "Lat.:";
            this.labelLat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLong
            // 
            this.labelLong.AutoSize = true;
            this.labelLong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLong.ForeColor = System.Drawing.Color.Blue;
            this.labelLong.Location = new System.Drawing.Point(211, 13);
            this.labelLong.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelLong.Name = "labelLong";
            this.labelLong.Size = new System.Drawing.Size(37, 16);
            this.labelLong.TabIndex = 26;
            this.labelLong.Text = "Long.:";
            this.labelLong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLongitude
            // 
            this.labelLongitude.AutoSize = true;
            this.labelLongitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLongitude.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labelLongitude.Location = new System.Drawing.Point(251, 13);
            this.labelLongitude.Name = "labelLongitude";
            this.labelLongitude.Size = new System.Drawing.Size(388, 16);
            this.labelLongitude.TabIndex = 27;
            this.labelLongitude.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLatitude.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labelLatitude.Location = new System.Drawing.Point(251, 0);
            this.labelLatitude.Name = "labelLatitude";
            this.labelLatitude.Size = new System.Drawing.Size(388, 13);
            this.labelLatitude.TabIndex = 28;
            this.labelLatitude.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonEditGeography
            // 
            this.buttonEditGeography.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonEditGeography.FlatAppearance.BorderSize = 0;
            this.buttonEditGeography.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditGeography.Image = global::DiversityCollection.Resource.Edit1;
            this.buttonEditGeography.Location = new System.Drawing.Point(43, 45);
            this.buttonEditGeography.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditGeography.Name = "buttonEditGeography";
            this.buttonEditGeography.Size = new System.Drawing.Size(22, 27);
            this.buttonEditGeography.TabIndex = 29;
            this.buttonEditGeography.UseVisualStyleBackColor = true;
            this.buttonEditGeography.Click += new System.EventHandler(this.buttonEditGeography_Click);
            // 
            // UserControl_GeoAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxGeoAnalysis);
            this.Name = "UserControl_GeoAnalysis";
            this.Size = new System.Drawing.Size(674, 211);
            this.groupBoxGeoAnalysis.ResumeLayout(false);
            this.tableLayoutPanelGeoAnalysis.ResumeLayout(false);
            this.tableLayoutPanelGeoAnalysis.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxGeoAnalysis;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelGeoAnalysis;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryGeoAnalysisResponsible;
        private System.Windows.Forms.Label labelGeoAnalysisResponsible;
        private System.Windows.Forms.Label labelGeoAnalysisNotes;
        private System.Windows.Forms.TextBox textBoxGeoAnalysisNotes;
        private System.Windows.Forms.Label labelGeoAnalysisDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerGeoAnalysisDate;
        private System.Windows.Forms.Button buttonGeoAnalysisSet;
        private System.Windows.Forms.Label labelGeography;
        private System.Windows.Forms.TextBox textBoxGeoAnalysisGeography;
        private System.Windows.Forms.Label labelLat;
        private System.Windows.Forms.Label labelLong;
        private System.Windows.Forms.Label labelLongitude;
        private System.Windows.Forms.Label labelLatitude;
        private System.Windows.Forms.Button buttonEditGeography;
    }
}
