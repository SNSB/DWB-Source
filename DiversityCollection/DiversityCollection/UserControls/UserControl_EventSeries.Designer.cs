namespace DiversityCollection.UserControls
{
    partial class UserControl_EventSeries
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_EventSeries));
            this.groupBoxEventSeries = new System.Windows.Forms.GroupBox();
            this.textBoxEventSeriesID = new System.Windows.Forms.TextBox();
            this.pictureBoxEventSeries = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelOverviewEventSeries = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCollectionEventSeriesEndDelete = new System.Windows.Forms.Button();
            this.labelOverviewEventSeriesCode = new System.Windows.Forms.Label();
            this.textBoxOverviewEventSeriesCode = new System.Windows.Forms.TextBox();
            this.labelOverviewEventSeriesDescription = new System.Windows.Forms.Label();
            this.textBoxOverviewEventSeriesDescription = new System.Windows.Forms.TextBox();
            this.labelOverviewEventSeriesNotes = new System.Windows.Forms.Label();
            this.textBoxOverviewEventSeriesNotes = new System.Windows.Forms.TextBox();
            this.dateTimePickerCollectionEventSeriesStart = new System.Windows.Forms.DateTimePicker();
            this.labelCollectionEventSeriesEnd = new System.Windows.Forms.Label();
            this.dateTimePickerCollectionEventSeriesEnd = new System.Windows.Forms.DateTimePicker();
            this.buttonEventSeriesSetGeo = new System.Windows.Forms.Button();
            this.textBoxGeography = new System.Windows.Forms.TextBox();
            this.labelSeriesStart = new System.Windows.Forms.Label();
            this.buttonCollectionEventSeriesStartDelete = new System.Windows.Forms.Button();
            this.buttonEditGeography = new System.Windows.Forms.Button();
            this.textBoxDateSupplement = new System.Windows.Forms.TextBox();
            this.labelDateSupplement = new System.Windows.Forms.Label();
            this.groupBoxEventSeries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEventSeries)).BeginInit();
            this.tableLayoutPanelOverviewEventSeries.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxEventSeries
            // 
            this.groupBoxEventSeries.AccessibleName = "CollectionEventSeries";
            this.groupBoxEventSeries.Controls.Add(this.textBoxEventSeriesID);
            this.groupBoxEventSeries.Controls.Add(this.pictureBoxEventSeries);
            this.groupBoxEventSeries.Controls.Add(this.tableLayoutPanelOverviewEventSeries);
            this.groupBoxEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEventSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEventSeries.ForeColor = System.Drawing.Color.Blue;
            this.groupBoxEventSeries.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEventSeries.Name = "groupBoxEventSeries";
            this.groupBoxEventSeries.Size = new System.Drawing.Size(580, 361);
            this.groupBoxEventSeries.TabIndex = 5;
            this.groupBoxEventSeries.TabStop = false;
            this.groupBoxEventSeries.Text = "Event series";
            // 
            // textBoxEventSeriesID
            // 
            this.textBoxEventSeriesID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEventSeriesID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEventSeriesID.ForeColor = System.Drawing.Color.Gray;
            this.textBoxEventSeriesID.Location = new System.Drawing.Point(488, 5);
            this.textBoxEventSeriesID.Name = "textBoxEventSeriesID";
            this.textBoxEventSeriesID.ReadOnly = true;
            this.textBoxEventSeriesID.Size = new System.Drawing.Size(70, 13);
            this.textBoxEventSeriesID.TabIndex = 5;
            this.textBoxEventSeriesID.Text = "SeriesID";
            this.textBoxEventSeriesID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBoxEventSeries
            // 
            this.pictureBoxEventSeries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxEventSeries.Image = global::DiversityCollection.Resource.EventSeries;
            this.pictureBoxEventSeries.Location = new System.Drawing.Point(561, 3);
            this.pictureBoxEventSeries.Name = "pictureBoxEventSeries";
            this.pictureBoxEventSeries.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxEventSeries.TabIndex = 4;
            this.pictureBoxEventSeries.TabStop = false;
            // 
            // tableLayoutPanelOverviewEventSeries
            // 
            this.tableLayoutPanelOverviewEventSeries.ColumnCount = 5;
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.tableLayoutPanelOverviewEventSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.buttonCollectionEventSeriesEndDelete, 4, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelOverviewEventSeriesCode, 0, 0);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxOverviewEventSeriesCode, 1, 0);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelOverviewEventSeriesDescription, 0, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxOverviewEventSeriesDescription, 1, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelOverviewEventSeriesNotes, 0, 3);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxOverviewEventSeriesNotes, 1, 3);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.dateTimePickerCollectionEventSeriesStart, 3, 0);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelCollectionEventSeriesEnd, 2, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.dateTimePickerCollectionEventSeriesEnd, 3, 1);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.buttonEventSeriesSetGeo, 0, 4);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxGeography, 1, 4);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelSeriesStart, 2, 0);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.buttonCollectionEventSeriesStartDelete, 4, 0);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.buttonEditGeography, 0, 5);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.textBoxDateSupplement, 3, 2);
            this.tableLayoutPanelOverviewEventSeries.Controls.Add(this.labelDateSupplement, 2, 2);
            this.tableLayoutPanelOverviewEventSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelOverviewEventSeries.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelOverviewEventSeries.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelOverviewEventSeries.Name = "tableLayoutPanelOverviewEventSeries";
            this.tableLayoutPanelOverviewEventSeries.RowCount = 6;
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelOverviewEventSeries.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelOverviewEventSeries.Size = new System.Drawing.Size(574, 342);
            this.tableLayoutPanelOverviewEventSeries.TabIndex = 3;
            // 
            // buttonCollectionEventSeriesEndDelete
            // 
            this.buttonCollectionEventSeriesEndDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCollectionEventSeriesEndDelete.FlatAppearance.BorderSize = 0;
            this.buttonCollectionEventSeriesEndDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCollectionEventSeriesEndDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonCollectionEventSeriesEndDelete.Location = new System.Drawing.Point(554, 26);
            this.buttonCollectionEventSeriesEndDelete.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.buttonCollectionEventSeriesEndDelete.Name = "buttonCollectionEventSeriesEndDelete";
            this.buttonCollectionEventSeriesEndDelete.Size = new System.Drawing.Size(17, 20);
            this.buttonCollectionEventSeriesEndDelete.TabIndex = 19;
            this.buttonCollectionEventSeriesEndDelete.UseVisualStyleBackColor = true;
            this.buttonCollectionEventSeriesEndDelete.Click += new System.EventHandler(this.buttonCollectionEventSeriesEndDelete_Click);
            // 
            // labelOverviewEventSeriesCode
            // 
            this.labelOverviewEventSeriesCode.AccessibleName = "CollectionEventSeries.SeriesCode";
            this.labelOverviewEventSeriesCode.AutoSize = true;
            this.labelOverviewEventSeriesCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewEventSeriesCode.ForeColor = System.Drawing.Color.Blue;
            this.labelOverviewEventSeriesCode.Location = new System.Drawing.Point(3, 0);
            this.labelOverviewEventSeriesCode.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelOverviewEventSeriesCode.Name = "labelOverviewEventSeriesCode";
            this.labelOverviewEventSeriesCode.Size = new System.Drawing.Size(67, 26);
            this.labelOverviewEventSeriesCode.TabIndex = 2;
            this.labelOverviewEventSeriesCode.Text = "Code:";
            this.labelOverviewEventSeriesCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxOverviewEventSeriesCode
            // 
            this.textBoxOverviewEventSeriesCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverviewEventSeriesCode.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOverviewEventSeriesCode.Location = new System.Drawing.Point(70, 3);
            this.textBoxOverviewEventSeriesCode.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxOverviewEventSeriesCode.Name = "textBoxOverviewEventSeriesCode";
            this.textBoxOverviewEventSeriesCode.Size = new System.Drawing.Size(340, 20);
            this.textBoxOverviewEventSeriesCode.TabIndex = 5;
            // 
            // labelOverviewEventSeriesDescription
            // 
            this.labelOverviewEventSeriesDescription.AccessibleName = "CollectionEventSeries.Description";
            this.labelOverviewEventSeriesDescription.AutoSize = true;
            this.labelOverviewEventSeriesDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewEventSeriesDescription.ForeColor = System.Drawing.Color.Blue;
            this.labelOverviewEventSeriesDescription.Location = new System.Drawing.Point(3, 29);
            this.labelOverviewEventSeriesDescription.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelOverviewEventSeriesDescription.Name = "labelOverviewEventSeriesDescription";
            this.labelOverviewEventSeriesDescription.Size = new System.Drawing.Size(67, 20);
            this.labelOverviewEventSeriesDescription.TabIndex = 4;
            this.labelOverviewEventSeriesDescription.Text = "Description:";
            this.labelOverviewEventSeriesDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxOverviewEventSeriesDescription
            // 
            this.textBoxOverviewEventSeriesDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverviewEventSeriesDescription.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOverviewEventSeriesDescription.Location = new System.Drawing.Point(70, 26);
            this.textBoxOverviewEventSeriesDescription.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxOverviewEventSeriesDescription.Multiline = true;
            this.textBoxOverviewEventSeriesDescription.Name = "textBoxOverviewEventSeriesDescription";
            this.tableLayoutPanelOverviewEventSeries.SetRowSpan(this.textBoxOverviewEventSeriesDescription, 2);
            this.textBoxOverviewEventSeriesDescription.Size = new System.Drawing.Size(340, 43);
            this.textBoxOverviewEventSeriesDescription.TabIndex = 3;
            // 
            // labelOverviewEventSeriesNotes
            // 
            this.labelOverviewEventSeriesNotes.AccessibleName = "CollectionEventSeries.Notes";
            this.labelOverviewEventSeriesNotes.AutoSize = true;
            this.labelOverviewEventSeriesNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOverviewEventSeriesNotes.ForeColor = System.Drawing.Color.Blue;
            this.labelOverviewEventSeriesNotes.Location = new System.Drawing.Point(3, 75);
            this.labelOverviewEventSeriesNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelOverviewEventSeriesNotes.Name = "labelOverviewEventSeriesNotes";
            this.labelOverviewEventSeriesNotes.Size = new System.Drawing.Size(67, 120);
            this.labelOverviewEventSeriesNotes.TabIndex = 6;
            this.labelOverviewEventSeriesNotes.Text = "Notes:";
            this.labelOverviewEventSeriesNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxOverviewEventSeriesNotes
            // 
            this.tableLayoutPanelOverviewEventSeries.SetColumnSpan(this.textBoxOverviewEventSeriesNotes, 4);
            this.textBoxOverviewEventSeriesNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOverviewEventSeriesNotes.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOverviewEventSeriesNotes.Location = new System.Drawing.Point(70, 72);
            this.textBoxOverviewEventSeriesNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxOverviewEventSeriesNotes.Multiline = true;
            this.textBoxOverviewEventSeriesNotes.Name = "textBoxOverviewEventSeriesNotes";
            this.textBoxOverviewEventSeriesNotes.Size = new System.Drawing.Size(501, 120);
            this.textBoxOverviewEventSeriesNotes.TabIndex = 7;
            // 
            // dateTimePickerCollectionEventSeriesStart
            // 
            this.dateTimePickerCollectionEventSeriesStart.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dateTimePickerCollectionEventSeriesStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerCollectionEventSeriesStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerCollectionEventSeriesStart.Location = new System.Drawing.Point(448, 3);
            this.dateTimePickerCollectionEventSeriesStart.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.dateTimePickerCollectionEventSeriesStart.Name = "dateTimePickerCollectionEventSeriesStart";
            this.dateTimePickerCollectionEventSeriesStart.Size = new System.Drawing.Size(106, 20);
            this.dateTimePickerCollectionEventSeriesStart.TabIndex = 9;
            this.dateTimePickerCollectionEventSeriesStart.CloseUp += new System.EventHandler(this.dateTimePickerCollectionEventSeriesStart_CloseUp);
            this.dateTimePickerCollectionEventSeriesStart.DropDown += new System.EventHandler(this.dateTimePickerCollectionEventSeriesStart_DropDown);
            // 
            // labelCollectionEventSeriesEnd
            // 
            this.labelCollectionEventSeriesEnd.AccessibleName = "CollectionEventSeries.DateEnd";
            this.labelCollectionEventSeriesEnd.AutoSize = true;
            this.labelCollectionEventSeriesEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionEventSeriesEnd.Location = new System.Drawing.Point(416, 29);
            this.labelCollectionEventSeriesEnd.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelCollectionEventSeriesEnd.Name = "labelCollectionEventSeriesEnd";
            this.labelCollectionEventSeriesEnd.Size = new System.Drawing.Size(32, 20);
            this.labelCollectionEventSeriesEnd.TabIndex = 10;
            this.labelCollectionEventSeriesEnd.Text = "End:";
            this.labelCollectionEventSeriesEnd.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dateTimePickerCollectionEventSeriesEnd
            // 
            this.dateTimePickerCollectionEventSeriesEnd.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dateTimePickerCollectionEventSeriesEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerCollectionEventSeriesEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerCollectionEventSeriesEnd.Location = new System.Drawing.Point(448, 26);
            this.dateTimePickerCollectionEventSeriesEnd.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.dateTimePickerCollectionEventSeriesEnd.Name = "dateTimePickerCollectionEventSeriesEnd";
            this.dateTimePickerCollectionEventSeriesEnd.Size = new System.Drawing.Size(106, 20);
            this.dateTimePickerCollectionEventSeriesEnd.TabIndex = 12;
            this.dateTimePickerCollectionEventSeriesEnd.CloseUp += new System.EventHandler(this.dateTimePickerCollectionEventSeriesEnd_CloseUp);
            this.dateTimePickerCollectionEventSeriesEnd.DropDown += new System.EventHandler(this.dateTimePickerCollectionEventSeriesEnd_DropDown);
            // 
            // buttonEventSeriesSetGeo
            // 
            this.buttonEventSeriesSetGeo.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonEventSeriesSetGeo.Image = global::DiversityCollection.Resource.Localisation;
            this.buttonEventSeriesSetGeo.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonEventSeriesSetGeo.Location = new System.Drawing.Point(0, 195);
            this.buttonEventSeriesSetGeo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.buttonEventSeriesSetGeo.Name = "buttonEventSeriesSetGeo";
            this.buttonEventSeriesSetGeo.Size = new System.Drawing.Size(70, 40);
            this.buttonEventSeriesSetGeo.TabIndex = 15;
            this.buttonEventSeriesSetGeo.Text = "Geography:";
            this.buttonEventSeriesSetGeo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.buttonEventSeriesSetGeo.UseVisualStyleBackColor = true;
            this.buttonEventSeriesSetGeo.Click += new System.EventHandler(this.buttonEventSeriesSetGeo_Click);
            // 
            // textBoxGeography
            // 
            this.textBoxGeography.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelOverviewEventSeries.SetColumnSpan(this.textBoxGeography, 4);
            this.textBoxGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxGeography.ForeColor = System.Drawing.Color.Blue;
            this.textBoxGeography.Location = new System.Drawing.Point(73, 198);
            this.textBoxGeography.Multiline = true;
            this.textBoxGeography.Name = "textBoxGeography";
            this.textBoxGeography.ReadOnly = true;
            this.tableLayoutPanelOverviewEventSeries.SetRowSpan(this.textBoxGeography, 2);
            this.textBoxGeography.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxGeography.Size = new System.Drawing.Size(498, 141);
            this.textBoxGeography.TabIndex = 16;
            // 
            // labelSeriesStart
            // 
            this.labelSeriesStart.AutoSize = true;
            this.labelSeriesStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSeriesStart.Location = new System.Drawing.Point(416, 0);
            this.labelSeriesStart.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSeriesStart.Name = "labelSeriesStart";
            this.labelSeriesStart.Size = new System.Drawing.Size(32, 26);
            this.labelSeriesStart.TabIndex = 17;
            this.labelSeriesStart.Text = "Start:";
            this.labelSeriesStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonCollectionEventSeriesStartDelete
            // 
            this.buttonCollectionEventSeriesStartDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCollectionEventSeriesStartDelete.FlatAppearance.BorderSize = 0;
            this.buttonCollectionEventSeriesStartDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCollectionEventSeriesStartDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonCollectionEventSeriesStartDelete.Location = new System.Drawing.Point(554, 3);
            this.buttonCollectionEventSeriesStartDelete.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonCollectionEventSeriesStartDelete.Name = "buttonCollectionEventSeriesStartDelete";
            this.buttonCollectionEventSeriesStartDelete.Size = new System.Drawing.Size(17, 20);
            this.buttonCollectionEventSeriesStartDelete.TabIndex = 18;
            this.buttonCollectionEventSeriesStartDelete.UseVisualStyleBackColor = true;
            this.buttonCollectionEventSeriesStartDelete.Click += new System.EventHandler(this.buttonCollectionEventSeriesStartDelete_Click);
            // 
            // buttonEditGeography
            // 
            this.buttonEditGeography.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonEditGeography.FlatAppearance.BorderSize = 0;
            this.buttonEditGeography.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditGeography.Image = global::DiversityCollection.Resource.Edit1;
            this.buttonEditGeography.Location = new System.Drawing.Point(45, 318);
            this.buttonEditGeography.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditGeography.Name = "buttonEditGeography";
            this.buttonEditGeography.Size = new System.Drawing.Size(25, 24);
            this.buttonEditGeography.TabIndex = 20;
            this.buttonEditGeography.UseVisualStyleBackColor = true;
            this.buttonEditGeography.Click += new System.EventHandler(this.buttonEditGeography_Click);
            // 
            // textBoxDateSupplement
            // 
            this.tableLayoutPanelOverviewEventSeries.SetColumnSpan(this.textBoxDateSupplement, 2);
            this.textBoxDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDateSupplement.Location = new System.Drawing.Point(448, 49);
            this.textBoxDateSupplement.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxDateSupplement.Name = "textBoxDateSupplement";
            this.textBoxDateSupplement.Size = new System.Drawing.Size(123, 20);
            this.textBoxDateSupplement.TabIndex = 22;
            // 
            // labelDateSupplement
            // 
            this.labelDateSupplement.AutoSize = true;
            this.labelDateSupplement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDateSupplement.Location = new System.Drawing.Point(413, 49);
            this.labelDateSupplement.Margin = new System.Windows.Forms.Padding(0);
            this.labelDateSupplement.Name = "labelDateSupplement";
            this.labelDateSupplement.Size = new System.Drawing.Size(35, 23);
            this.labelDateSupplement.TabIndex = 23;
            this.labelDateSupplement.Text = "Sup.:";
            this.labelDateSupplement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UserControl_EventSeries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxEventSeries);
            this.Name = "UserControl_EventSeries";
            this.Size = new System.Drawing.Size(580, 361);
            this.groupBoxEventSeries.ResumeLayout(false);
            this.groupBoxEventSeries.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEventSeries)).EndInit();
            this.tableLayoutPanelOverviewEventSeries.ResumeLayout(false);
            this.tableLayoutPanelOverviewEventSeries.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxEventSeries;
        private System.Windows.Forms.TextBox textBoxEventSeriesID;
        private System.Windows.Forms.PictureBox pictureBoxEventSeries;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelOverviewEventSeries;
        private System.Windows.Forms.Label labelOverviewEventSeriesCode;
        private System.Windows.Forms.TextBox textBoxOverviewEventSeriesCode;
        private System.Windows.Forms.Label labelOverviewEventSeriesDescription;
        private System.Windows.Forms.TextBox textBoxOverviewEventSeriesDescription;
        private System.Windows.Forms.Label labelOverviewEventSeriesNotes;
        private System.Windows.Forms.TextBox textBoxOverviewEventSeriesNotes;
        private System.Windows.Forms.DateTimePicker dateTimePickerCollectionEventSeriesStart;
        private System.Windows.Forms.Label labelCollectionEventSeriesEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerCollectionEventSeriesEnd;
        private System.Windows.Forms.Button buttonEventSeriesSetGeo;
        private System.Windows.Forms.TextBox textBoxGeography;
        private System.Windows.Forms.Label labelSeriesStart;
        private System.Windows.Forms.Button buttonCollectionEventSeriesEndDelete;
        private System.Windows.Forms.Button buttonCollectionEventSeriesStartDelete;
        private System.Windows.Forms.Button buttonEditGeography;
        private System.Windows.Forms.TextBox textBoxDateSupplement;
        private System.Windows.Forms.Label labelDateSupplement;
    }
}
