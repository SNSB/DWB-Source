namespace DiversityCollection.UserControls
{
    partial class UserControlEventSeriesTree
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
            this.toolStripTree = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonInsertSeries = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSearchSpecimen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowUnit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonInsertEvent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorSave = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTaxonList = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelOrderBy = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxOrderBy = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonRebuildTree = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.treeViewData = new System.Windows.Forms.TreeView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataSetCollectionEventSeries = new DiversityCollection.Datasets.DataSetCollectionEventSeries();
            this.toolStripTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionEventSeries)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripTree
            // 
            this.toolStripTree.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripTree.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonInsertSeries,
            this.toolStripButtonSearchSpecimen,
            this.toolStripButtonShowUnit,
            this.toolStripButtonInsertEvent,
            this.toolStripSeparatorSave,
            this.toolStripButtonSave,
            this.toolStripButtonTaxonList,
            this.toolStripLabelOrderBy,
            this.toolStripComboBoxOrderBy,
            this.toolStripButtonRebuildTree,
            this.toolStripButtonDeleteItem});
            this.toolStripTree.Location = new System.Drawing.Point(0, 186);
            this.toolStripTree.Name = "toolStripTree";
            this.toolStripTree.Size = new System.Drawing.Size(333, 25);
            this.toolStripTree.TabIndex = 2;
            this.toolStripTree.Text = "toolStrip1";
            // 
            // toolStripButtonInsertSeries
            // 
            this.toolStripButtonInsertSeries.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInsertSeries.Image = global::DiversityCollection.Resource.EventSeries;
            this.toolStripButtonInsertSeries.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInsertSeries.Name = "toolStripButtonInsertSeries";
            this.toolStripButtonInsertSeries.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonInsertSeries.Text = "Insert a new event series";
            this.toolStripButtonInsertSeries.Click += new System.EventHandler(this.toolStripButtonInsertSeries_Click);
            // 
            // toolStripButtonSearchSpecimen
            // 
            this.toolStripButtonSearchSpecimen.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSearchSpecimen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSearchSpecimen.Enabled = false;
            this.toolStripButtonSearchSpecimen.Image = global::DiversityCollection.Resource.Search;
            this.toolStripButtonSearchSpecimen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearchSpecimen.Name = "toolStripButtonSearchSpecimen";
            this.toolStripButtonSearchSpecimen.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSearchSpecimen.Text = "Close window and change to the selected specimen in the main window";
            // 
            // toolStripButtonShowUnit
            // 
            this.toolStripButtonShowUnit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonShowUnit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowUnit.Image = global::DiversityCollection.Resource.Plant;
            this.toolStripButtonShowUnit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowUnit.Name = "toolStripButtonShowUnit";
            this.toolStripButtonShowUnit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowUnit.Text = "Hide or show the organisms";
            this.toolStripButtonShowUnit.ToolTipText = "Hide or show the organisms in the tree";
            this.toolStripButtonShowUnit.Click += new System.EventHandler(this.toolStripButtonShowUnit_Click);
            // 
            // toolStripButtonInsertEvent
            // 
            this.toolStripButtonInsertEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonInsertEvent.Image = global::DiversityCollection.Resource.Event;
            this.toolStripButtonInsertEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInsertEvent.Name = "toolStripButtonInsertEvent";
            this.toolStripButtonInsertEvent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonInsertEvent.Text = "Insert a new collection event";
            this.toolStripButtonInsertEvent.Click += new System.EventHandler(this.toolStripButtonInsertEvent_Click);
            // 
            // toolStripSeparatorSave
            // 
            this.toolStripSeparatorSave.Name = "toolStripSeparatorSave";
            this.toolStripSeparatorSave.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = global::DiversityCollection.Resource.Save;
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save changes and rebuild tree";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonTaxonList
            // 
            this.toolStripButtonTaxonList.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonTaxonList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTaxonList.Image = global::DiversityCollection.Resource.TaxonList;
            this.toolStripButtonTaxonList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTaxonList.Name = "toolStripButtonTaxonList";
            this.toolStripButtonTaxonList.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTaxonList.Text = "Create a taxon list for the selected item";
            this.toolStripButtonTaxonList.Click += new System.EventHandler(this.toolStripButtonTaxonList_Click);
            // 
            // toolStripLabelOrderBy
            // 
            this.toolStripLabelOrderBy.Name = "toolStripLabelOrderBy";
            this.toolStripLabelOrderBy.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabelOrderBy.Text = "Ord.by:";
            // 
            // toolStripComboBoxOrderBy
            // 
            this.toolStripComboBoxOrderBy.Name = "toolStripComboBoxOrderBy";
            this.toolStripComboBoxOrderBy.Size = new System.Drawing.Size(75, 25);
            this.toolStripComboBoxOrderBy.ToolTipText = "Choose the order within the tree";
            // 
            // toolStripButtonRebuildTree
            // 
            this.toolStripButtonRebuildTree.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRebuildTree.Image = global::DiversityCollection.Resource.Transfrom;
            this.toolStripButtonRebuildTree.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRebuildTree.Name = "toolStripButtonRebuildTree";
            this.toolStripButtonRebuildTree.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRebuildTree.Text = "Reload the tree";
            this.toolStripButtonRebuildTree.Click += new System.EventHandler(this.toolStripButtonRebuildTree_Click);
            // 
            // toolStripButtonDeleteItem
            // 
            this.toolStripButtonDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteItem.Enabled = false;
            this.toolStripButtonDeleteItem.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDeleteItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteItem.Name = "toolStripButtonDeleteItem";
            this.toolStripButtonDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDeleteItem.Text = "toolStripButton1";
            this.toolStripButtonDeleteItem.Visible = false;
            this.toolStripButtonDeleteItem.Click += new System.EventHandler(this.toolStripButtonDeleteItem_Click);
            // 
            // treeViewData
            // 
            this.treeViewData.AllowDrop = true;
            this.treeViewData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewData.Location = new System.Drawing.Point(0, 0);
            this.treeViewData.Name = "treeViewData";
            this.treeViewData.Size = new System.Drawing.Size(333, 186);
            this.treeViewData.TabIndex = 3;
            this.treeViewData.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewData_ItemDrag);
            this.treeViewData.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewData_AfterSelect);
            this.treeViewData.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewData_DragDrop);
            this.treeViewData.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewData_DragOver);
            // 
            // dataSetCollectionEventSeries
            // 
            this.dataSetCollectionEventSeries.DataSetName = "DataSetCollectionEventSeries";
            this.dataSetCollectionEventSeries.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // UserControlEventSeriesTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewData);
            this.Controls.Add(this.toolStripTree);
            this.Name = "UserControlEventSeriesTree";
            this.Size = new System.Drawing.Size(333, 211);
            this.toolStripTree.ResumeLayout(false);
            this.toolStripTree.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCollectionEventSeries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripTree;
        private System.Windows.Forms.TreeView treeViewData;
        private DiversityCollection.Datasets.DataSetCollectionEventSeries dataSetCollectionEventSeries;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripButton toolStripButtonInsertSeries;
        private System.Windows.Forms.ToolStripButton toolStripButtonInsertEvent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        public System.Windows.Forms.ToolStripButton toolStripButtonShowUnit;
        public System.Windows.Forms.ToolStripButton toolStripButtonSearchSpecimen;
        private System.Windows.Forms.ToolStripButton toolStripButtonTaxonList;
        private System.Windows.Forms.ToolStripLabel toolStripLabelOrderBy;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxOrderBy;
        private System.Windows.Forms.ToolStripButton toolStripButtonRebuildTree;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteItem;
    }
}
