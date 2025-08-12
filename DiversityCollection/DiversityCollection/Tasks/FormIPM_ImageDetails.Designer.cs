namespace DiversityCollection.Tasks
{
    partial class FormIPM_ImageDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormIPM_ImageDetails));
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.userControlPlanMasterImage = new DiversityCollection.Tasks.UserControlPlan();
            this.tableLayoutPanelDetailImages = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxDetailImages = new System.Windows.Forms.ListBox();
            this.toolStripDetailImages = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDetailImageAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDetailImageDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDetailImageSave = new System.Windows.Forms.ToolStripButton();
            this.userControlImageDetailImage = new DiversityWorkbench.UserControls.UserControlImage();
            this.listBoxCollectionTasks = new System.Windows.Forms.ListBox();
            this.toolStripHeader = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFeedback = new System.Windows.Forms.ToolStripButton();
            this.collectionTaskBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetCollectionTask = new DiversityCollection.Datasets.DataSetCollectionTask();
            this.imageListDetailImages = new System.Windows.Forms.ImageList(this.components);
            this.collectionTaskTableAdapter = new DiversityCollection.Datasets.DataSetCollectionTaskTableAdapters.CollectionTaskTableAdapter();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanelDetailImages.SuspendLayout();
            this.toolStripDetailImages.SuspendLayout();
            this.toolStripHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionTaskBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionTask)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 30);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.userControlPlanMasterImage);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tableLayoutPanelDetailImages);
            this.splitContainerMain.Size = new System.Drawing.Size(924, 547);
            this.splitContainerMain.SplitterDistance = 562;
            this.splitContainerMain.TabIndex = 0;
            // 
            // userControlPlanMasterImage
            // 
            this.userControlPlanMasterImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlPlanMasterImage.Location = new System.Drawing.Point(0, 0);
            this.userControlPlanMasterImage.Name = "userControlPlanMasterImage";
            this.userControlPlanMasterImage.Size = new System.Drawing.Size(562, 547);
            this.userControlPlanMasterImage.TabIndex = 0;
            // 
            // tableLayoutPanelDetailImages
            // 
            this.tableLayoutPanelDetailImages.ColumnCount = 3;
            this.tableLayoutPanelDetailImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDetailImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDetailImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDetailImages.Controls.Add(this.listBoxDetailImages, 0, 5);
            this.tableLayoutPanelDetailImages.Controls.Add(this.toolStripDetailImages, 2, 5);
            this.tableLayoutPanelDetailImages.Controls.Add(this.userControlImageDetailImage, 0, 1);
            this.tableLayoutPanelDetailImages.Controls.Add(this.listBoxCollectionTasks, 0, 0);
            this.tableLayoutPanelDetailImages.Controls.Add(this.toolStripHeader, 2, 0);
            this.tableLayoutPanelDetailImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDetailImages.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDetailImages.Name = "tableLayoutPanelDetailImages";
            this.tableLayoutPanelDetailImages.RowCount = 6;
            this.tableLayoutPanelDetailImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDetailImages.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDetailImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDetailImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDetailImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDetailImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDetailImages.Size = new System.Drawing.Size(358, 547);
            this.tableLayoutPanelDetailImages.TabIndex = 0;
            // 
            // listBoxDetailImages
            // 
            this.tableLayoutPanelDetailImages.SetColumnSpan(this.listBoxDetailImages, 2);
            this.listBoxDetailImages.ColumnWidth = 55;
            this.listBoxDetailImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDetailImages.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxDetailImages.FormattingEnabled = true;
            this.listBoxDetailImages.HorizontalScrollbar = true;
            this.listBoxDetailImages.IntegralHeight = false;
            this.listBoxDetailImages.ItemHeight = 50;
            this.listBoxDetailImages.Location = new System.Drawing.Point(3, 464);
            this.listBoxDetailImages.MultiColumn = true;
            this.listBoxDetailImages.Name = "listBoxDetailImages";
            this.listBoxDetailImages.ScrollAlwaysVisible = true;
            this.listBoxDetailImages.Size = new System.Drawing.Size(328, 80);
            this.listBoxDetailImages.TabIndex = 0;
            this.listBoxDetailImages.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxDetailImages_DrawItem);
            this.listBoxDetailImages.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBoxDetailImages_MeasureItem);
            this.listBoxDetailImages.SelectedIndexChanged += new System.EventHandler(this.listBoxDetailImages_SelectedIndexChanged);
            // 
            // toolStripDetailImages
            // 
            this.toolStripDetailImages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDetailImageAdd,
            this.toolStripButtonDetailImageDelete,
            this.toolStripButtonDetailImageSave});
            this.toolStripDetailImages.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripDetailImages.Location = new System.Drawing.Point(334, 461);
            this.toolStripDetailImages.Name = "toolStripDetailImages";
            this.toolStripDetailImages.Size = new System.Drawing.Size(24, 67);
            this.toolStripDetailImages.TabIndex = 1;
            this.toolStripDetailImages.Text = "toolStrip1";
            // 
            // toolStripButtonDetailImageAdd
            // 
            this.toolStripButtonDetailImageAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDetailImageAdd.Image = global::DiversityCollection.Resource.Add1;
            this.toolStripButtonDetailImageAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDetailImageAdd.Name = "toolStripButtonDetailImageAdd";
            this.toolStripButtonDetailImageAdd.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDetailImageAdd.Text = "Add a detail image";
            this.toolStripButtonDetailImageAdd.Click += new System.EventHandler(this.toolStripButtonDetailImageAdd_Click);
            // 
            // toolStripButtonDetailImageDelete
            // 
            this.toolStripButtonDetailImageDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDetailImageDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDetailImageDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDetailImageDelete.Name = "toolStripButtonDetailImageDelete";
            this.toolStripButtonDetailImageDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDetailImageDelete.Text = "Remove the selected image";
            this.toolStripButtonDetailImageDelete.Click += new System.EventHandler(this.toolStripButtonDetailImageDelete_Click);
            // 
            // toolStripButtonDetailImageSave
            // 
            this.toolStripButtonDetailImageSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDetailImageSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonDetailImageSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonDetailImageSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDetailImageSave.Name = "toolStripButtonDetailImageSave";
            this.toolStripButtonDetailImageSave.Size = new System.Drawing.Size(23, 18);
            this.toolStripButtonDetailImageSave.Text = "Save geometry for current image";
            this.toolStripButtonDetailImageSave.Click += new System.EventHandler(this.toolStripButtonDetailImageSave_Click);
            // 
            // userControlImageDetailImage
            // 
            this.userControlImageDetailImage.AutorotationEnabled = false;
            this.tableLayoutPanelDetailImages.SetColumnSpan(this.userControlImageDetailImage, 3);
            this.userControlImageDetailImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlImageDetailImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.userControlImageDetailImage.ImagePath = "";
            this.userControlImageDetailImage.Location = new System.Drawing.Point(3, 59);
            this.userControlImageDetailImage.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Unknown;
            this.userControlImageDetailImage.Name = "userControlImageDetailImage";
            this.userControlImageDetailImage.Size = new System.Drawing.Size(352, 399);
            this.userControlImageDetailImage.TabIndex = 2;
            // 
            // listBoxCollectionTasks
            // 
            this.tableLayoutPanelDetailImages.SetColumnSpan(this.listBoxCollectionTasks, 2);
            this.listBoxCollectionTasks.ColumnWidth = 10;
            this.listBoxCollectionTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCollectionTasks.FormattingEnabled = true;
            this.listBoxCollectionTasks.IntegralHeight = false;
            this.listBoxCollectionTasks.Location = new System.Drawing.Point(3, 3);
            this.listBoxCollectionTasks.Name = "listBoxCollectionTasks";
            this.listBoxCollectionTasks.ScrollAlwaysVisible = true;
            this.listBoxCollectionTasks.Size = new System.Drawing.Size(328, 50);
            this.listBoxCollectionTasks.TabIndex = 8;
            this.listBoxCollectionTasks.SelectedIndexChanged += new System.EventHandler(this.listBoxCollectionTasks_SelectedIndexChanged);
            // 
            // toolStripHeader
            // 
            this.toolStripHeader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFeedback});
            this.toolStripHeader.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.toolStripHeader.Location = new System.Drawing.Point(334, 0);
            this.toolStripHeader.Name = "toolStripHeader";
            this.toolStripHeader.Size = new System.Drawing.Size(24, 23);
            this.toolStripHeader.TabIndex = 9;
            this.toolStripHeader.Text = "toolStrip1";
            // 
            // toolStripButtonFeedback
            // 
            this.toolStripButtonFeedback.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonFeedback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.toolStripButtonFeedback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFeedback.Name = "toolStripButtonFeedback";
            this.toolStripButtonFeedback.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonFeedback.Text = "Send a feedback";
            this.toolStripButtonFeedback.Click += new System.EventHandler(this.toolStripButtonFeedback_Click);
            // 
            // collectionTaskBindingSource
            // 
            this.collectionTaskBindingSource.DataMember = "CollectionTask";
            this.collectionTaskBindingSource.DataSource = this.dataSetCollectionTask;
            // 
            // dataSetCollectionTask
            // 
            this.dataSetCollectionTask.DataSetName = "DataSetCollectionTask";
            this.dataSetCollectionTask.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // imageListDetailImages
            // 
            this.imageListDetailImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDetailImages.ImageSize = new System.Drawing.Size(50, 50);
            this.imageListDetailImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // collectionTaskTableAdapter
            // 
            this.collectionTaskTableAdapter.ClearBeforeFill = true;
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.ForeColor = System.Drawing.Color.Green;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(924, 30);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Trap";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormIPM_ImageDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 577);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.labelHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormIPM_ImageDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IPM: Detail images";
            this.Load += new System.EventHandler(this.FormIPM_ImageDetails_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanelDetailImages.ResumeLayout(false);
            this.tableLayoutPanelDetailImages.PerformLayout();
            this.toolStripDetailImages.ResumeLayout(false);
            this.toolStripDetailImages.PerformLayout();
            this.toolStripHeader.ResumeLayout(false);
            this.toolStripHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.collectionTaskBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionTask)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDetailImages;
        private System.Windows.Forms.ListBox listBoxDetailImages;
        private System.Windows.Forms.ToolStrip toolStripDetailImages;
        private System.Windows.Forms.ToolStripButton toolStripButtonDetailImageAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonDetailImageDelete;
        private DiversityWorkbench.UserControls.UserControlImage userControlImageDetailImage;
        private UserControlPlan userControlPlanMasterImage;
        private System.Windows.Forms.ImageList imageListDetailImages;
        private Datasets.DataSetCollectionTask dataSetCollectionTask;
        private System.Windows.Forms.BindingSource collectionTaskBindingSource;
        private Datasets.DataSetCollectionTaskTableAdapters.CollectionTaskTableAdapter collectionTaskTableAdapter;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ListBox listBoxCollectionTasks;
        private System.Windows.Forms.ToolStrip toolStripHeader;
        private System.Windows.Forms.ToolStripButton toolStripButtonFeedback;
        private System.Windows.Forms.ToolStripButton toolStripButtonDetailImageSave;
        private System.Windows.Forms.Label labelHeader;
    }
}