namespace DiversityCollection.UserControls
{
    partial class UserControl_Processing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Processing));
            this.groupBoxProcessing = new System.Windows.Forms.GroupBox();
            this.pictureBoxProcessing = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelProcessing = new System.Windows.Forms.TableLayoutPanel();
            this.labelProcessingResponsible = new System.Windows.Forms.Label();
            this.textBoxProcessingProtocoll = new System.Windows.Forms.TextBox();
            this.labelProcessingProtocoll = new System.Windows.Forms.Label();
            this.dateTimePickerProcessingDate = new System.Windows.Forms.DateTimePicker();
            this.labelProcessingDate = new System.Windows.Forms.Label();
            this.labelProcessingDuration = new System.Windows.Forms.Label();
            this.labelProcessingNotes = new System.Windows.Forms.Label();
            this.textBoxProcessingNotes = new System.Windows.Forms.TextBox();
            this.dateTimePickerProcessingDuration = new System.Windows.Forms.DateTimePicker();
            this.userControlModuleRelatedEntryProcessingResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.buttonProcessingDateDelete = new System.Windows.Forms.Button();
            this.textBoxSpecimenProcessingID = new System.Windows.Forms.TextBox();
            this.labelSpecimenProcessingID = new System.Windows.Forms.Label();
            this.userControlDurationProcessing = new DiversityWorkbench.UserControls.UserControlDuration();
            this.buttonProcessingDurationType = new System.Windows.Forms.Button();
            this.groupBoxProcessing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProcessing)).BeginInit();
            this.tableLayoutPanelProcessing.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxProcessing
            // 
            this.groupBoxProcessing.AccessibleName = "CollectionSpecimenProcessing";
            this.groupBoxProcessing.Controls.Add(this.pictureBoxProcessing);
            this.groupBoxProcessing.Controls.Add(this.tableLayoutPanelProcessing);
            this.groupBoxProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxProcessing.ForeColor = System.Drawing.Color.Blue;
            this.groupBoxProcessing.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProcessing.MinimumSize = new System.Drawing.Size(0, 107);
            this.groupBoxProcessing.Name = "groupBoxProcessing";
            this.groupBoxProcessing.Padding = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.groupBoxProcessing.Size = new System.Drawing.Size(644, 268);
            this.groupBoxProcessing.TabIndex = 2;
            this.groupBoxProcessing.TabStop = false;
            this.groupBoxProcessing.Text = "Processing";
            // 
            // pictureBoxProcessing
            // 
            this.pictureBoxProcessing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxProcessing.Image = global::DiversityCollection.Resource.Processing;
            this.pictureBoxProcessing.Location = new System.Drawing.Point(622, 1);
            this.pictureBoxProcessing.Name = "pictureBoxProcessing";
            this.pictureBoxProcessing.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxProcessing.TabIndex = 1;
            this.pictureBoxProcessing.TabStop = false;
            // 
            // tableLayoutPanelProcessing
            // 
            this.tableLayoutPanelProcessing.ColumnCount = 8;
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 19F));
            this.tableLayoutPanelProcessing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelProcessing.Controls.Add(this.labelProcessingResponsible, 0, 2);
            this.tableLayoutPanelProcessing.Controls.Add(this.textBoxProcessingProtocoll, 2, 0);
            this.tableLayoutPanelProcessing.Controls.Add(this.labelProcessingProtocoll, 0, 0);
            this.tableLayoutPanelProcessing.Controls.Add(this.dateTimePickerProcessingDate, 2, 1);
            this.tableLayoutPanelProcessing.Controls.Add(this.labelProcessingDate, 0, 1);
            this.tableLayoutPanelProcessing.Controls.Add(this.labelProcessingDuration, 4, 1);
            this.tableLayoutPanelProcessing.Controls.Add(this.labelProcessingNotes, 0, 3);
            this.tableLayoutPanelProcessing.Controls.Add(this.textBoxProcessingNotes, 0, 4);
            this.tableLayoutPanelProcessing.Controls.Add(this.dateTimePickerProcessingDuration, 6, 1);
            this.tableLayoutPanelProcessing.Controls.Add(this.userControlModuleRelatedEntryProcessingResponsible, 2, 2);
            this.tableLayoutPanelProcessing.Controls.Add(this.buttonProcessingDateDelete, 3, 1);
            this.tableLayoutPanelProcessing.Controls.Add(this.textBoxSpecimenProcessingID, 5, 3);
            this.tableLayoutPanelProcessing.Controls.Add(this.labelSpecimenProcessingID, 4, 3);
            this.tableLayoutPanelProcessing.Controls.Add(this.userControlDurationProcessing, 5, 1);
            this.tableLayoutPanelProcessing.Controls.Add(this.buttonProcessingDurationType, 7, 1);
            this.tableLayoutPanelProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelProcessing.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanelProcessing.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanelProcessing.Name = "tableLayoutPanelProcessing";
            this.tableLayoutPanelProcessing.RowCount = 5;
            this.tableLayoutPanelProcessing.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcessing.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcessing.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelProcessing.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanelProcessing.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelProcessing.Size = new System.Drawing.Size(638, 250);
            this.tableLayoutPanelProcessing.TabIndex = 0;
            // 
            // labelProcessingResponsible
            // 
            this.labelProcessingResponsible.AccessibleName = "CollectionSpecimenProcessing.ResponsibleName";
            this.labelProcessingResponsible.AutoSize = true;
            this.tableLayoutPanelProcessing.SetColumnSpan(this.labelProcessingResponsible, 2);
            this.labelProcessingResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcessingResponsible.Location = new System.Drawing.Point(3, 49);
            this.labelProcessingResponsible.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelProcessingResponsible.Name = "labelProcessingResponsible";
            this.labelProcessingResponsible.Size = new System.Drawing.Size(55, 24);
            this.labelProcessingResponsible.TabIndex = 27;
            this.labelProcessingResponsible.Text = "Respons.:";
            this.labelProcessingResponsible.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxProcessingProtocoll
            // 
            this.tableLayoutPanelProcessing.SetColumnSpan(this.textBoxProcessingProtocoll, 6);
            this.textBoxProcessingProtocoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcessingProtocoll.ForeColor = System.Drawing.Color.Blue;
            this.textBoxProcessingProtocoll.Location = new System.Drawing.Point(58, 3);
            this.textBoxProcessingProtocoll.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxProcessingProtocoll.Name = "textBoxProcessingProtocoll";
            this.textBoxProcessingProtocoll.Size = new System.Drawing.Size(577, 20);
            this.textBoxProcessingProtocoll.TabIndex = 26;
            // 
            // labelProcessingProtocoll
            // 
            this.labelProcessingProtocoll.AccessibleName = "CollectionSpecimenProcessing.Protocoll";
            this.labelProcessingProtocoll.AutoSize = true;
            this.tableLayoutPanelProcessing.SetColumnSpan(this.labelProcessingProtocoll, 2);
            this.labelProcessingProtocoll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcessingProtocoll.Location = new System.Drawing.Point(3, 0);
            this.labelProcessingProtocoll.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelProcessingProtocoll.Name = "labelProcessingProtocoll";
            this.labelProcessingProtocoll.Size = new System.Drawing.Size(55, 26);
            this.labelProcessingProtocoll.TabIndex = 25;
            this.labelProcessingProtocoll.Text = "Protocoll:";
            this.labelProcessingProtocoll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerProcessingDate
            // 
            this.dateTimePickerProcessingDate.CalendarForeColor = System.Drawing.Color.Blue;
            this.dateTimePickerProcessingDate.CalendarTitleForeColor = System.Drawing.Color.Blue;
            this.dateTimePickerProcessingDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerProcessingDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerProcessingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerProcessingDate.Location = new System.Drawing.Point(58, 26);
            this.dateTimePickerProcessingDate.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.dateTimePickerProcessingDate.Name = "dateTimePickerProcessingDate";
            this.dateTimePickerProcessingDate.Size = new System.Drawing.Size(214, 20);
            this.dateTimePickerProcessingDate.TabIndex = 0;
            this.dateTimePickerProcessingDate.TabStop = false;
            this.dateTimePickerProcessingDate.CloseUp += new System.EventHandler(this.dateTimePickerProcessingDate_CloseUp);
            this.dateTimePickerProcessingDate.DropDown += new System.EventHandler(this.dateTimePickerProcessingDate_DropDown);
            // 
            // labelProcessingDate
            // 
            this.labelProcessingDate.AccessibleName = "CollectionSpecimenProcessing.ProcessingDate";
            this.labelProcessingDate.AutoSize = true;
            this.tableLayoutPanelProcessing.SetColumnSpan(this.labelProcessingDate, 2);
            this.labelProcessingDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcessingDate.Location = new System.Drawing.Point(3, 26);
            this.labelProcessingDate.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelProcessingDate.Name = "labelProcessingDate";
            this.labelProcessingDate.Size = new System.Drawing.Size(55, 23);
            this.labelProcessingDate.TabIndex = 1;
            this.labelProcessingDate.Text = "Date:";
            this.labelProcessingDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProcessingDuration
            // 
            this.labelProcessingDuration.AccessibleName = "CollectionSpecimenProcessing.ProcessingDuration";
            this.labelProcessingDuration.AutoSize = true;
            this.labelProcessingDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcessingDuration.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelProcessingDuration.Location = new System.Drawing.Point(288, 26);
            this.labelProcessingDuration.Margin = new System.Windows.Forms.Padding(0);
            this.labelProcessingDuration.Name = "labelProcessingDuration";
            this.labelProcessingDuration.Size = new System.Drawing.Size(50, 23);
            this.labelProcessingDuration.TabIndex = 2;
            this.labelProcessingDuration.Text = "Duration:";
            this.labelProcessingDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelProcessingDuration.Click += new System.EventHandler(this.labelProcessingDuration_Click);
            // 
            // labelProcessingNotes
            // 
            this.labelProcessingNotes.AccessibleName = "CollectionSpecimenProcessing.Notes";
            this.labelProcessingNotes.AutoSize = true;
            this.tableLayoutPanelProcessing.SetColumnSpan(this.labelProcessingNotes, 2);
            this.labelProcessingNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProcessingNotes.Location = new System.Drawing.Point(3, 73);
            this.labelProcessingNotes.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelProcessingNotes.Name = "labelProcessingNotes";
            this.labelProcessingNotes.Size = new System.Drawing.Size(55, 13);
            this.labelProcessingNotes.TabIndex = 3;
            this.labelProcessingNotes.Text = "Notes:";
            this.labelProcessingNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxProcessingNotes
            // 
            this.tableLayoutPanelProcessing.SetColumnSpan(this.textBoxProcessingNotes, 8);
            this.textBoxProcessingNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProcessingNotes.ForeColor = System.Drawing.Color.Blue;
            this.textBoxProcessingNotes.Location = new System.Drawing.Point(3, 86);
            this.textBoxProcessingNotes.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
            this.textBoxProcessingNotes.Multiline = true;
            this.textBoxProcessingNotes.Name = "textBoxProcessingNotes";
            this.textBoxProcessingNotes.Size = new System.Drawing.Size(632, 163);
            this.textBoxProcessingNotes.TabIndex = 23;
            // 
            // dateTimePickerProcessingDuration
            // 
            this.dateTimePickerProcessingDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerProcessingDuration.Location = new System.Drawing.Point(600, 26);
            this.dateTimePickerProcessingDuration.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.dateTimePickerProcessingDuration.Name = "dateTimePickerProcessingDuration";
            this.dateTimePickerProcessingDuration.Size = new System.Drawing.Size(18, 20);
            this.dateTimePickerProcessingDuration.TabIndex = 29;
            this.dateTimePickerProcessingDuration.CloseUp += new System.EventHandler(this.dateTimePickerProcessingDuration_CloseUp);
            // 
            // userControlModuleRelatedEntryProcessingResponsible
            // 
            this.userControlModuleRelatedEntryProcessingResponsible.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelProcessing.SetColumnSpan(this.userControlModuleRelatedEntryProcessingResponsible, 6);
            this.userControlModuleRelatedEntryProcessingResponsible.DependsOnUri = "";
            this.userControlModuleRelatedEntryProcessingResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryProcessingResponsible.Domain = "";
            this.userControlModuleRelatedEntryProcessingResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryProcessingResponsible.Location = new System.Drawing.Point(58, 49);
            this.userControlModuleRelatedEntryProcessingResponsible.Margin = new System.Windows.Forms.Padding(0, 0, 3, 2);
            this.userControlModuleRelatedEntryProcessingResponsible.Module = null;
            this.userControlModuleRelatedEntryProcessingResponsible.Name = "userControlModuleRelatedEntryProcessingResponsible";
            this.userControlModuleRelatedEntryProcessingResponsible.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryProcessingResponsible.ShowInfo = false;
            this.userControlModuleRelatedEntryProcessingResponsible.Size = new System.Drawing.Size(577, 22);
            this.userControlModuleRelatedEntryProcessingResponsible.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryProcessingResponsible.TabIndex = 30;
            // 
            // buttonProcessingDateDelete
            // 
            this.buttonProcessingDateDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProcessingDateDelete.FlatAppearance.BorderSize = 0;
            this.buttonProcessingDateDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProcessingDateDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonProcessingDateDelete.Location = new System.Drawing.Point(272, 26);
            this.buttonProcessingDateDelete.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.buttonProcessingDateDelete.Name = "buttonProcessingDateDelete";
            this.buttonProcessingDateDelete.Size = new System.Drawing.Size(16, 21);
            this.buttonProcessingDateDelete.TabIndex = 36;
            this.buttonProcessingDateDelete.UseVisualStyleBackColor = true;
            this.buttonProcessingDateDelete.Click += new System.EventHandler(this.buttonProcessingDateDelete_Click);
            // 
            // textBoxSpecimenProcessingID
            // 
            this.textBoxSpecimenProcessingID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelProcessing.SetColumnSpan(this.textBoxSpecimenProcessingID, 3);
            this.textBoxSpecimenProcessingID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSpecimenProcessingID.ForeColor = System.Drawing.Color.Blue;
            this.textBoxSpecimenProcessingID.Location = new System.Drawing.Point(338, 73);
            this.textBoxSpecimenProcessingID.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.textBoxSpecimenProcessingID.Name = "textBoxSpecimenProcessingID";
            this.textBoxSpecimenProcessingID.ReadOnly = true;
            this.textBoxSpecimenProcessingID.Size = new System.Drawing.Size(297, 13);
            this.textBoxSpecimenProcessingID.TabIndex = 37;
            // 
            // labelSpecimenProcessingID
            // 
            this.labelSpecimenProcessingID.AutoSize = true;
            this.labelSpecimenProcessingID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpecimenProcessingID.Location = new System.Drawing.Point(291, 73);
            this.labelSpecimenProcessingID.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSpecimenProcessingID.Name = "labelSpecimenProcessingID";
            this.labelSpecimenProcessingID.Size = new System.Drawing.Size(47, 13);
            this.labelSpecimenProcessingID.TabIndex = 38;
            this.labelSpecimenProcessingID.Text = "ID:";
            this.labelSpecimenProcessingID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlDurationProcessing
            // 
            this.userControlDurationProcessing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDurationProcessing.Location = new System.Drawing.Point(338, 26);
            this.userControlDurationProcessing.Margin = new System.Windows.Forms.Padding(0);
            this.userControlDurationProcessing.Name = "userControlDurationProcessing";
            this.userControlDurationProcessing.Size = new System.Drawing.Size(262, 23);
            this.userControlDurationProcessing.TabIndex = 39;
            // 
            // buttonProcessingDurationType
            // 
            this.buttonProcessingDurationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonProcessingDurationType.FlatAppearance.BorderSize = 0;
            this.buttonProcessingDurationType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProcessingDurationType.Image = global::DiversityCollection.Resource.ISO;
            this.buttonProcessingDurationType.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonProcessingDurationType.Location = new System.Drawing.Point(618, 26);
            this.buttonProcessingDurationType.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.buttonProcessingDurationType.Name = "buttonProcessingDurationType";
            this.buttonProcessingDurationType.Size = new System.Drawing.Size(19, 23);
            this.buttonProcessingDurationType.TabIndex = 40;
            this.toolTip.SetToolTip(this.buttonProcessingDurationType, "Save duration in ISO format for period. Click to change for date format");
            this.buttonProcessingDurationType.UseVisualStyleBackColor = true;
            this.buttonProcessingDurationType.Click += new System.EventHandler(this.buttonProcessingDurationType_Click);
            // 
            // UserControl_Processing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxProcessing);
            this.Name = "UserControl_Processing";
            this.Size = new System.Drawing.Size(644, 268);
            this.groupBoxProcessing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProcessing)).EndInit();
            this.tableLayoutPanelProcessing.ResumeLayout(false);
            this.tableLayoutPanelProcessing.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProcessing;
        private System.Windows.Forms.PictureBox pictureBoxProcessing;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProcessing;
        private System.Windows.Forms.Label labelProcessingResponsible;
        private System.Windows.Forms.Label labelProcessingProtocoll;
        private System.Windows.Forms.DateTimePicker dateTimePickerProcessingDate;
        private System.Windows.Forms.Label labelProcessingDate;
        private System.Windows.Forms.Label labelProcessingDuration;
        private System.Windows.Forms.Label labelProcessingNotes;
        private System.Windows.Forms.TextBox textBoxProcessingNotes;
        private System.Windows.Forms.DateTimePicker dateTimePickerProcessingDuration;
        private System.Windows.Forms.Button buttonProcessingDateDelete;
        private System.Windows.Forms.TextBox textBoxSpecimenProcessingID;
        private System.Windows.Forms.Label labelSpecimenProcessingID;
        private DiversityWorkbench.UserControls.UserControlDuration userControlDurationProcessing;
        private System.Windows.Forms.Button buttonProcessingDurationType;
        private System.Windows.Forms.TextBox textBoxProcessingProtocoll;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryProcessingResponsible;
    }
}
