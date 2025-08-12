namespace DiversityCollection.Tasks
{
    partial class UserControlImages
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.userControlImage = new DiversityWorkbench.UserControls.UserControlImage();
            this.tableLayoutPanelImage = new System.Windows.Forms.TableLayoutPanel();
            this.labelCollectionImageNotes = new System.Windows.Forms.Label();
            this.textBoxCollectionImageNotes = new System.Windows.Forms.TextBox();
            this.collectionTaskImageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetCollectionTask = new DiversityCollection.Tasks.DataSetCollectionTask();
            this.labelLogInsertedWhen = new System.Windows.Forms.Label();
            this.textBoxLogInsertedWhen = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelImageList = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxCollectionImage = new System.Windows.Forms.ListBox();
            this.toolStripCollectionImage = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonImageNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImageDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImageDescription = new System.Windows.Forms.ToolStripButton();
            this.imageListCollectionTaskImages = new System.Windows.Forms.ImageList(this.components);
            this.collectionTaskImageTableAdapter = new DiversityCollection.Tasks.DataSetCollectionTaskTableAdapters.CollectionTaskImageTableAdapter();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripButtonImageDetails = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionTaskImageBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionTask)).BeginInit();
            this.tableLayoutPanelImageList.SuspendLayout();
            this.toolStripCollectionImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.userControlImage);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanelImage);
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanelImageList);
            this.splitContainer.Size = new System.Drawing.Size(626, 366);
            this.splitContainer.SplitterDistance = 421;
            this.splitContainer.TabIndex = 0;
            // 
            // userControlImage
            // 
            this.userControlImage.AutorotationEnabled = false;
            this.userControlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.userControlImage.ImagePath = "";
            this.userControlImage.Location = new System.Drawing.Point(0, 0);
            this.userControlImage.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Unknown;
            this.userControlImage.Name = "userControlImage";
            this.userControlImage.Size = new System.Drawing.Size(421, 366);
            this.userControlImage.TabIndex = 0;
            // 
            // tableLayoutPanelImage
            // 
            this.tableLayoutPanelImage.ColumnCount = 1;
            this.tableLayoutPanelImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImage.Controls.Add(this.labelCollectionImageNotes, 0, 0);
            this.tableLayoutPanelImage.Controls.Add(this.textBoxCollectionImageNotes, 0, 1);
            this.tableLayoutPanelImage.Controls.Add(this.labelLogInsertedWhen, 0, 2);
            this.tableLayoutPanelImage.Controls.Add(this.textBoxLogInsertedWhen, 0, 3);
            this.tableLayoutPanelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelImage.Enabled = false;
            this.tableLayoutPanelImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.tableLayoutPanelImage.Location = new System.Drawing.Point(76, 0);
            this.tableLayoutPanelImage.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanelImage.Name = "tableLayoutPanelImage";
            this.tableLayoutPanelImage.RowCount = 4;
            this.tableLayoutPanelImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImage.Size = new System.Drawing.Size(125, 366);
            this.tableLayoutPanelImage.TabIndex = 3;
            // 
            // labelCollectionImageNotes
            // 
            this.labelCollectionImageNotes.AccessibleName = "CollectionImage.Notes";
            this.labelCollectionImageNotes.AutoSize = true;
            this.labelCollectionImageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionImageNotes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelCollectionImageNotes.Location = new System.Drawing.Point(0, 3);
            this.labelCollectionImageNotes.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.labelCollectionImageNotes.Name = "labelCollectionImageNotes";
            this.labelCollectionImageNotes.Size = new System.Drawing.Size(125, 17);
            this.labelCollectionImageNotes.TabIndex = 8;
            this.labelCollectionImageNotes.Text = "Notes:";
            this.labelCollectionImageNotes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxCollectionImageNotes
            // 
            this.textBoxCollectionImageNotes.AccessibleName = "CollectionImage.Notes";
            this.textBoxCollectionImageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.collectionTaskImageBindingSource, "Notes", true));
            this.textBoxCollectionImageNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionImageNotes.Location = new System.Drawing.Point(0, 20);
            this.textBoxCollectionImageNotes.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.textBoxCollectionImageNotes.Multiline = true;
            this.textBoxCollectionImageNotes.Name = "textBoxCollectionImageNotes";
            this.textBoxCollectionImageNotes.Size = new System.Drawing.Size(125, 302);
            this.textBoxCollectionImageNotes.TabIndex = 9;
            // 
            // collectionTaskImageBindingSource
            // 
            this.collectionTaskImageBindingSource.DataMember = "CollectionTaskImage";
            this.collectionTaskImageBindingSource.DataSource = this.dataSetCollectionTask;
            // 
            // dataSetCollectionTask
            // 
            this.dataSetCollectionTask.DataSetName = "DataSetCollectionTask";
            this.dataSetCollectionTask.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // labelLogInsertedWhen
            // 
            this.labelLogInsertedWhen.AutoSize = true;
            this.labelLogInsertedWhen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLogInsertedWhen.Location = new System.Drawing.Point(3, 323);
            this.labelLogInsertedWhen.Name = "labelLogInsertedWhen";
            this.labelLogInsertedWhen.Size = new System.Drawing.Size(119, 20);
            this.labelLogInsertedWhen.TabIndex = 10;
            this.labelLogInsertedWhen.Text = "Cre.date:";
            this.labelLogInsertedWhen.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxLogInsertedWhen
            // 
            this.textBoxLogInsertedWhen.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.collectionTaskImageBindingSource, "LogInsertedWhen", true));
            this.textBoxLogInsertedWhen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogInsertedWhen.Location = new System.Drawing.Point(0, 343);
            this.textBoxLogInsertedWhen.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxLogInsertedWhen.Name = "textBoxLogInsertedWhen";
            this.textBoxLogInsertedWhen.ReadOnly = true;
            this.textBoxLogInsertedWhen.Size = new System.Drawing.Size(125, 20);
            this.textBoxLogInsertedWhen.TabIndex = 11;
            // 
            // tableLayoutPanelImageList
            // 
            this.tableLayoutPanelImageList.ColumnCount = 1;
            this.tableLayoutPanelImageList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImageList.Controls.Add(this.listBoxCollectionImage, 0, 0);
            this.tableLayoutPanelImageList.Controls.Add(this.toolStripCollectionImage, 0, 1);
            this.tableLayoutPanelImageList.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanelImageList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelImageList.Name = "tableLayoutPanelImageList";
            this.tableLayoutPanelImageList.RowCount = 2;
            this.tableLayoutPanelImageList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelImageList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImageList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelImageList.Size = new System.Drawing.Size(76, 366);
            this.tableLayoutPanelImageList.TabIndex = 2;
            // 
            // listBoxCollectionImage
            // 
            this.listBoxCollectionImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCollectionImage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxCollectionImage.FormattingEnabled = true;
            this.listBoxCollectionImage.IntegralHeight = false;
            this.listBoxCollectionImage.ItemHeight = 50;
            this.listBoxCollectionImage.Location = new System.Drawing.Point(0, 0);
            this.listBoxCollectionImage.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.listBoxCollectionImage.Name = "listBoxCollectionImage";
            this.listBoxCollectionImage.ScrollAlwaysVisible = true;
            this.listBoxCollectionImage.Size = new System.Drawing.Size(73, 341);
            this.listBoxCollectionImage.TabIndex = 0;
            this.listBoxCollectionImage.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxCollectionImage_DrawItem);
            this.listBoxCollectionImage.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBoxCollectionImage_MeasureItem);
            this.listBoxCollectionImage.SelectedIndexChanged += new System.EventHandler(this.listBoxCollectionImage_SelectedIndexChanged);
            // 
            // toolStripCollectionImage
            // 
            this.toolStripCollectionImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripCollectionImage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripCollectionImage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonImageNew,
            this.toolStripButtonImageDelete,
            this.toolStripButtonImageDescription,
            this.toolStripButtonImageDetails});
            this.toolStripCollectionImage.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripCollectionImage.Location = new System.Drawing.Point(0, 341);
            this.toolStripCollectionImage.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStripCollectionImage.Name = "toolStripCollectionImage";
            this.toolStripCollectionImage.Size = new System.Drawing.Size(73, 25);
            this.toolStripCollectionImage.TabIndex = 7;
            this.toolStripCollectionImage.Text = "toolStrip1";
            // 
            // toolStripButtonImageNew
            // 
            this.toolStripButtonImageNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImageNew.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonImageNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImageNew.Name = "toolStripButtonImageNew";
            this.toolStripButtonImageNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonImageNew.Text = "Insert a new image";
            this.toolStripButtonImageNew.Click += new System.EventHandler(this.toolStripButtonImageNew_Click);
            // 
            // toolStripButtonImageDelete
            // 
            this.toolStripButtonImageDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImageDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonImageDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImageDelete.Name = "toolStripButtonImageDelete";
            this.toolStripButtonImageDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonImageDelete.Text = "Delete the selected image";
            this.toolStripButtonImageDelete.Click += new System.EventHandler(this.toolStripButtonImageDelete_Click);
            // 
            // toolStripButtonImageDescription
            // 
            this.toolStripButtonImageDescription.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImageDescription.Image = global::DiversityCollection.Resource.Properties;
            this.toolStripButtonImageDescription.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImageDescription.Name = "toolStripButtonImageDescription";
            this.toolStripButtonImageDescription.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonImageDescription.Text = "Edit the description of the image";
            this.toolStripButtonImageDescription.Visible = false;
            // 
            // imageListCollectionTaskImages
            // 
            this.imageListCollectionTaskImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            this.imageListCollectionTaskImages.ImageSize = new System.Drawing.Size(50, 50);
            this.imageListCollectionTaskImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // collectionTaskImageTableAdapter
            // 
            this.collectionTaskImageTableAdapter.ClearBeforeFill = true;
            // 
            // toolStripButtonImageDetails
            // 
            this.toolStripButtonImageDetails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImageDetails.Image = global::DiversityCollection.Resource.Lupe;
            this.toolStripButtonImageDetails.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImageDetails.Name = "toolStripButtonImageDetails";
            this.toolStripButtonImageDetails.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonImageDetails.Text = "Add or show detail images";
            this.toolStripButtonImageDetails.Click += new System.EventHandler(this.toolStripButtonImageDetails_Click);
            // 
            // UserControlImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "UserControlImages";
            this.Size = new System.Drawing.Size(626, 366);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanelImage.ResumeLayout(false);
            this.tableLayoutPanelImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionTaskImageBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionTask)).EndInit();
            this.tableLayoutPanelImageList.ResumeLayout(false);
            this.tableLayoutPanelImageList.PerformLayout();
            this.toolStripCollectionImage.ResumeLayout(false);
            this.toolStripCollectionImage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private DiversityWorkbench.UserControls.UserControlImage userControlImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImageList;
        private System.Windows.Forms.ListBox listBoxCollectionImage;
        private System.Windows.Forms.ToolStrip toolStripCollectionImage;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImage;
        private System.Windows.Forms.Label labelCollectionImageNotes;
        private System.Windows.Forms.TextBox textBoxCollectionImageNotes;
        private DataSetCollectionTask dataSetCollectionTask;
        private System.Windows.Forms.ImageList imageListCollectionTaskImages;
        private System.Windows.Forms.BindingSource collectionTaskImageBindingSource;
        private DataSetCollectionTaskTableAdapters.CollectionTaskImageTableAdapter collectionTaskImageTableAdapter;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelLogInsertedWhen;
        private System.Windows.Forms.TextBox textBoxLogInsertedWhen;
        private System.Windows.Forms.ToolStripButton toolStripButtonImageDetails;
    }
}
