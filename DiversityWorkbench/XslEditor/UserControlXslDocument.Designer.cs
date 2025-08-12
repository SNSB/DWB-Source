namespace DiversityWorkbench.XslEditor
{
    partial class UserControlXslDocument
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
            this.splitContainerDocument = new System.Windows.Forms.SplitContainer();
            this.treeViewDocument = new System.Windows.Forms.TreeView();
            this.toolStripDocument = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDocumentAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDocumentDelete = new System.Windows.Forms.ToolStripButton();
            this.splitContainerDocumentNode = new System.Windows.Forms.SplitContainer();
            this.treeViewNode = new System.Windows.Forms.TreeView();
            this.toolStripNodes = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNodeNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNodeCopy = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNodeAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNodeUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNodeDown = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTableEditor = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNodeDelete = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanelNodeDetails = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripNodeAttributes = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNodeAttributeNew = new System.Windows.Forms.ToolStripButton();
            this.labelNodeContent = new System.Windows.Forms.Label();
            this.labelNodeAttributes = new System.Windows.Forms.Label();
            this.panelNodeAttributes = new System.Windows.Forms.Panel();
            this.textBoxNodeContent = new System.Windows.Forms.TextBox();
            this.labelNodeValue = new System.Windows.Forms.Label();
            this.panelNodeValues = new System.Windows.Forms.Panel();
            this.toolStripNodeValues = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNodeValuesAdd = new System.Windows.Forms.ToolStripButton();
            this.labelIncludedFile = new System.Windows.Forms.Label();
            this.splitContainerDocument.Panel1.SuspendLayout();
            this.splitContainerDocument.Panel2.SuspendLayout();
            this.splitContainerDocument.SuspendLayout();
            this.toolStripDocument.SuspendLayout();
            this.splitContainerDocumentNode.Panel1.SuspendLayout();
            this.splitContainerDocumentNode.Panel2.SuspendLayout();
            this.splitContainerDocumentNode.SuspendLayout();
            this.toolStripNodes.SuspendLayout();
            this.tableLayoutPanelNodeDetails.SuspendLayout();
            this.toolStripNodeAttributes.SuspendLayout();
            this.toolStripNodeValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerDocument
            // 
            this.splitContainerDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDocument.Location = new System.Drawing.Point(0, 20);
            this.splitContainerDocument.Name = "splitContainerDocument";
            // 
            // splitContainerDocument.Panel1
            // 
            this.splitContainerDocument.Panel1.Controls.Add(this.treeViewDocument);
            this.splitContainerDocument.Panel1.Controls.Add(this.toolStripDocument);
            // 
            // splitContainerDocument.Panel2
            // 
            this.splitContainerDocument.Panel2.Controls.Add(this.splitContainerDocumentNode);
            this.splitContainerDocument.Size = new System.Drawing.Size(906, 533);
            this.splitContainerDocument.SplitterDistance = 186;
            this.splitContainerDocument.TabIndex = 1;
            // 
            // treeViewDocument
            // 
            this.treeViewDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDocument.Location = new System.Drawing.Point(0, 0);
            this.treeViewDocument.Name = "treeViewDocument";
            this.treeViewDocument.ShowPlusMinus = false;
            this.treeViewDocument.ShowRootLines = false;
            this.treeViewDocument.Size = new System.Drawing.Size(186, 508);
            this.treeViewDocument.TabIndex = 1;
            this.treeViewDocument.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDocument_AfterSelect);
            // 
            // toolStripDocument
            // 
            this.toolStripDocument.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripDocument.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDocument.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDocumentAdd,
            this.toolStripButtonDocumentDelete});
            this.toolStripDocument.Location = new System.Drawing.Point(0, 508);
            this.toolStripDocument.Name = "toolStripDocument";
            this.toolStripDocument.Size = new System.Drawing.Size(186, 25);
            this.toolStripDocument.TabIndex = 0;
            this.toolStripDocument.Text = "toolStrip1";
            // 
            // toolStripButtonDocumentAdd
            // 
            this.toolStripButtonDocumentAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDocumentAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonDocumentAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDocumentAdd.Name = "toolStripButtonDocumentAdd";
            this.toolStripButtonDocumentAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDocumentAdd.Text = "Add a new node";
            this.toolStripButtonDocumentAdd.Click += new System.EventHandler(this.toolStripButtonDocumentAdd_Click);
            // 
            // toolStripButtonDocumentDelete
            // 
            this.toolStripButtonDocumentDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDocumentDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonDocumentDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDocumentDelete.Name = "toolStripButtonDocumentDelete";
            this.toolStripButtonDocumentDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDocumentDelete.Text = "Delete the selected node";
            this.toolStripButtonDocumentDelete.Click += new System.EventHandler(this.toolStripButtonDocumentDelete_Click);
            // 
            // splitContainerDocumentNode
            // 
            this.splitContainerDocumentNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDocumentNode.Location = new System.Drawing.Point(0, 0);
            this.splitContainerDocumentNode.Name = "splitContainerDocumentNode";
            // 
            // splitContainerDocumentNode.Panel1
            // 
            this.splitContainerDocumentNode.Panel1.Controls.Add(this.treeViewNode);
            this.splitContainerDocumentNode.Panel1.Controls.Add(this.toolStripNodes);
            // 
            // splitContainerDocumentNode.Panel2
            // 
            this.splitContainerDocumentNode.Panel2.Controls.Add(this.tableLayoutPanelNodeDetails);
            this.splitContainerDocumentNode.Size = new System.Drawing.Size(716, 533);
            this.splitContainerDocumentNode.SplitterDistance = 245;
            this.splitContainerDocumentNode.TabIndex = 1;
            // 
            // treeViewNode
            // 
            this.treeViewNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewNode.Location = new System.Drawing.Point(0, 0);
            this.treeViewNode.Name = "treeViewNode";
            this.treeViewNode.Size = new System.Drawing.Size(245, 508);
            this.treeViewNode.TabIndex = 0;
            this.treeViewNode.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewNode_AfterSelect);
            // 
            // toolStripNodes
            // 
            this.toolStripNodes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripNodes.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripNodes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNodeNew,
            this.toolStripButtonNodeCopy,
            this.toolStripButtonNodeAdd,
            this.toolStripButtonNodeUp,
            this.toolStripButtonNodeDown,
            this.toolStripButtonTableEditor,
            this.toolStripButtonNodeDelete});
            this.toolStripNodes.Location = new System.Drawing.Point(0, 508);
            this.toolStripNodes.Name = "toolStripNodes";
            this.toolStripNodes.Size = new System.Drawing.Size(245, 25);
            this.toolStripNodes.TabIndex = 1;
            this.toolStripNodes.Text = "toolStrip1";
            // 
            // toolStripButtonNodeNew
            // 
            this.toolStripButtonNodeNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNodeNew.Image = global::DiversityWorkbench.ResourceWorkbench.New;
            this.toolStripButtonNodeNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNodeNew.Name = "toolStripButtonNodeNew";
            this.toolStripButtonNodeNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNodeNew.Text = "Add a new node on behind the selected node";
            this.toolStripButtonNodeNew.Click += new System.EventHandler(this.toolStripButtonNodeNew_Click);
            // 
            // toolStripButtonNodeCopy
            // 
            this.toolStripButtonNodeCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNodeCopy.Image = global::DiversityWorkbench.ResourceWorkbench.Copy;
            this.toolStripButtonNodeCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNodeCopy.Name = "toolStripButtonNodeCopy";
            this.toolStripButtonNodeCopy.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNodeCopy.Text = "Create a copy of the selected node";
            this.toolStripButtonNodeCopy.Click += new System.EventHandler(this.toolStripButtonNodeCopy_Click);
            // 
            // toolStripButtonNodeAdd
            // 
            this.toolStripButtonNodeAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNodeAdd.Image = global::DiversityWorkbench.Properties.Resources.AddChild;
            this.toolStripButtonNodeAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNodeAdd.Name = "toolStripButtonNodeAdd";
            this.toolStripButtonNodeAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNodeAdd.Text = "Add a new node as child of the current node";
            this.toolStripButtonNodeAdd.Click += new System.EventHandler(this.toolStripButtonNodeAdd_Click);
            // 
            // toolStripButtonNodeUp
            // 
            this.toolStripButtonNodeUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNodeUp.Image = global::DiversityWorkbench.Properties.Resources.ArrowUpSmall;
            this.toolStripButtonNodeUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNodeUp.Name = "toolStripButtonNodeUp";
            this.toolStripButtonNodeUp.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNodeUp.Text = "Move the selected node upwards";
            this.toolStripButtonNodeUp.Visible = false;
            this.toolStripButtonNodeUp.Click += new System.EventHandler(this.toolStripButtonNodeUp_Click);
            // 
            // toolStripButtonNodeDown
            // 
            this.toolStripButtonNodeDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNodeDown.Image = global::DiversityWorkbench.Properties.Resources.ArrowDownSmall;
            this.toolStripButtonNodeDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNodeDown.Name = "toolStripButtonNodeDown";
            this.toolStripButtonNodeDown.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNodeDown.Text = "Move the selected node downwards";
            this.toolStripButtonNodeDown.Visible = false;
            this.toolStripButtonNodeDown.Click += new System.EventHandler(this.toolStripButtonNodeDown_Click);
            // 
            // toolStripButtonTableEditor
            // 
            this.toolStripButtonTableEditor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTableEditor.Image = global::DiversityWorkbench.Properties.Resources.EditInSpeadsheet;
            this.toolStripButtonTableEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTableEditor.Name = "toolStripButtonTableEditor";
            this.toolStripButtonTableEditor.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonTableEditor.Text = "Edit table";
            this.toolStripButtonTableEditor.Visible = false;
            this.toolStripButtonTableEditor.Click += new System.EventHandler(this.toolStripButtonTableEditor_Click);
            // 
            // toolStripButtonNodeDelete
            // 
            this.toolStripButtonNodeDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNodeDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonNodeDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNodeDelete.Name = "toolStripButtonNodeDelete";
            this.toolStripButtonNodeDelete.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNodeDelete.Text = "toolStripButton1";
            this.toolStripButtonNodeDelete.Click += new System.EventHandler(this.toolStripButtonNodeDelete_Click);
            // 
            // tableLayoutPanelNodeDetails
            // 
            this.tableLayoutPanelNodeDetails.ColumnCount = 1;
            this.tableLayoutPanelNodeDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNodeDetails.Controls.Add(this.toolStripNodeAttributes, 0, 4);
            this.tableLayoutPanelNodeDetails.Controls.Add(this.labelNodeContent, 0, 0);
            this.tableLayoutPanelNodeDetails.Controls.Add(this.labelNodeAttributes, 0, 2);
            this.tableLayoutPanelNodeDetails.Controls.Add(this.panelNodeAttributes, 0, 3);
            this.tableLayoutPanelNodeDetails.Controls.Add(this.textBoxNodeContent, 0, 1);
            this.tableLayoutPanelNodeDetails.Controls.Add(this.labelNodeValue, 0, 5);
            this.tableLayoutPanelNodeDetails.Controls.Add(this.panelNodeValues, 0, 6);
            this.tableLayoutPanelNodeDetails.Controls.Add(this.toolStripNodeValues, 0, 7);
            this.tableLayoutPanelNodeDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelNodeDetails.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelNodeDetails.Name = "tableLayoutPanelNodeDetails";
            this.tableLayoutPanelNodeDetails.RowCount = 8;
            this.tableLayoutPanelNodeDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNodeDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNodeDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNodeDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNodeDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNodeDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNodeDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelNodeDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelNodeDetails.Size = new System.Drawing.Size(467, 533);
            this.tableLayoutPanelNodeDetails.TabIndex = 1;
            // 
            // toolStripNodeAttributes
            // 
            this.toolStripNodeAttributes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripNodeAttributes.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripNodeAttributes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNodeAttributeNew});
            this.toolStripNodeAttributes.Location = new System.Drawing.Point(0, 141);
            this.toolStripNodeAttributes.Name = "toolStripNodeAttributes";
            this.toolStripNodeAttributes.Size = new System.Drawing.Size(467, 25);
            this.toolStripNodeAttributes.TabIndex = 12;
            this.toolStripNodeAttributes.Text = "toolStrip1";
            // 
            // toolStripButtonNodeAttributeNew
            // 
            this.toolStripButtonNodeAttributeNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNodeAttributeNew.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonNodeAttributeNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNodeAttributeNew.Name = "toolStripButtonNodeAttributeNew";
            this.toolStripButtonNodeAttributeNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNodeAttributeNew.Text = "Add a new attribute";
            this.toolStripButtonNodeAttributeNew.Click += new System.EventHandler(this.toolStripButtonNodeAttributeNew_Click);
            // 
            // labelNodeContent
            // 
            this.labelNodeContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNodeContent.Location = new System.Drawing.Point(3, 0);
            this.labelNodeContent.Name = "labelNodeContent";
            this.labelNodeContent.Size = new System.Drawing.Size(461, 20);
            this.labelNodeContent.TabIndex = 7;
            this.labelNodeContent.Text = "Content";
            this.labelNodeContent.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelNodeAttributes
            // 
            this.labelNodeAttributes.AutoSize = true;
            this.labelNodeAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNodeAttributes.Location = new System.Drawing.Point(3, 92);
            this.labelNodeAttributes.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelNodeAttributes.Name = "labelNodeAttributes";
            this.labelNodeAttributes.Size = new System.Drawing.Size(461, 13);
            this.labelNodeAttributes.TabIndex = 5;
            this.labelNodeAttributes.Text = "Attributes";
            this.labelNodeAttributes.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // panelNodeAttributes
            // 
            this.panelNodeAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNodeAttributes.Location = new System.Drawing.Point(3, 108);
            this.panelNodeAttributes.Name = "panelNodeAttributes";
            this.panelNodeAttributes.Size = new System.Drawing.Size(461, 30);
            this.panelNodeAttributes.TabIndex = 6;
            // 
            // textBoxNodeContent
            // 
            this.textBoxNodeContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNodeContent.Location = new System.Drawing.Point(3, 23);
            this.textBoxNodeContent.Multiline = true;
            this.textBoxNodeContent.Name = "textBoxNodeContent";
            this.textBoxNodeContent.Size = new System.Drawing.Size(461, 60);
            this.textBoxNodeContent.TabIndex = 8;
            this.textBoxNodeContent.Leave += new System.EventHandler(this.textBoxNodeContent_Leave);
            // 
            // labelNodeValue
            // 
            this.labelNodeValue.AutoSize = true;
            this.labelNodeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNodeValue.Location = new System.Drawing.Point(3, 166);
            this.labelNodeValue.Name = "labelNodeValue";
            this.labelNodeValue.Size = new System.Drawing.Size(461, 13);
            this.labelNodeValue.TabIndex = 9;
            this.labelNodeValue.Text = "Value";
            this.labelNodeValue.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // panelNodeValues
            // 
            this.panelNodeValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNodeValues.Location = new System.Drawing.Point(3, 182);
            this.panelNodeValues.Name = "panelNodeValues";
            this.panelNodeValues.Size = new System.Drawing.Size(461, 323);
            this.panelNodeValues.TabIndex = 10;
            // 
            // toolStripNodeValues
            // 
            this.toolStripNodeValues.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripNodeValues.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNodeValuesAdd});
            this.toolStripNodeValues.Location = new System.Drawing.Point(0, 508);
            this.toolStripNodeValues.Name = "toolStripNodeValues";
            this.toolStripNodeValues.Size = new System.Drawing.Size(467, 25);
            this.toolStripNodeValues.TabIndex = 11;
            this.toolStripNodeValues.Text = "toolStrip1";
            // 
            // toolStripButtonNodeValuesAdd
            // 
            this.toolStripButtonNodeValuesAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNodeValuesAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonNodeValuesAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonNodeValuesAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNodeValuesAdd.Name = "toolStripButtonNodeValuesAdd";
            this.toolStripButtonNodeValuesAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNodeValuesAdd.Text = "Add a new value";
            this.toolStripButtonNodeValuesAdd.Click += new System.EventHandler(this.toolStripButtonNodeValuesAdd_Click);
            // 
            // labelIncludedFile
            // 
            this.labelIncludedFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelIncludedFile.Location = new System.Drawing.Point(0, 0);
            this.labelIncludedFile.Name = "labelIncludedFile";
            this.labelIncludedFile.Size = new System.Drawing.Size(906, 20);
            this.labelIncludedFile.TabIndex = 2;
            this.labelIncludedFile.Text = "Included file:";
            this.labelIncludedFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserControlXslDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerDocument);
            this.Controls.Add(this.labelIncludedFile);
            this.Name = "UserControlXslDocument";
            this.Size = new System.Drawing.Size(906, 553);
            this.splitContainerDocument.Panel1.ResumeLayout(false);
            this.splitContainerDocument.Panel1.PerformLayout();
            this.splitContainerDocument.Panel2.ResumeLayout(false);
            this.splitContainerDocument.ResumeLayout(false);
            this.toolStripDocument.ResumeLayout(false);
            this.toolStripDocument.PerformLayout();
            this.splitContainerDocumentNode.Panel1.ResumeLayout(false);
            this.splitContainerDocumentNode.Panel1.PerformLayout();
            this.splitContainerDocumentNode.Panel2.ResumeLayout(false);
            this.splitContainerDocumentNode.ResumeLayout(false);
            this.toolStripNodes.ResumeLayout(false);
            this.toolStripNodes.PerformLayout();
            this.tableLayoutPanelNodeDetails.ResumeLayout(false);
            this.tableLayoutPanelNodeDetails.PerformLayout();
            this.toolStripNodeAttributes.ResumeLayout(false);
            this.toolStripNodeAttributes.PerformLayout();
            this.toolStripNodeValues.ResumeLayout(false);
            this.toolStripNodeValues.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerDocument;
        private System.Windows.Forms.TreeView treeViewDocument;
        private System.Windows.Forms.ToolStrip toolStripDocument;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonDocumentDelete;
        private System.Windows.Forms.SplitContainer splitContainerDocumentNode;
        private System.Windows.Forms.TreeView treeViewNode;
        private System.Windows.Forms.ToolStrip toolStripNodes;
        private System.Windows.Forms.ToolStripButton toolStripButtonNodeNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonNodeCopy;
        private System.Windows.Forms.ToolStripButton toolStripButtonNodeAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonNodeUp;
        private System.Windows.Forms.ToolStripButton toolStripButtonNodeDown;
        private System.Windows.Forms.ToolStripButton toolStripButtonTableEditor;
        private System.Windows.Forms.ToolStripButton toolStripButtonNodeDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNodeDetails;
        private System.Windows.Forms.Label labelNodeContent;
        private System.Windows.Forms.Label labelNodeAttributes;
        private System.Windows.Forms.Panel panelNodeAttributes;
        private System.Windows.Forms.TextBox textBoxNodeContent;
        private System.Windows.Forms.Label labelNodeValue;
        private System.Windows.Forms.Panel panelNodeValues;
        private System.Windows.Forms.ToolStrip toolStripNodeValues;
        private System.Windows.Forms.ToolStripButton toolStripButtonNodeValuesAdd;
        private System.Windows.Forms.ToolStrip toolStripNodeAttributes;
        private System.Windows.Forms.ToolStripButton toolStripButtonNodeAttributeNew;
        private System.Windows.Forms.Label labelIncludedFile;
    }
}
