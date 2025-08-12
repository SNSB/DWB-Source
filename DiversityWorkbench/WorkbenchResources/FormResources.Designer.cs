namespace DiversityWorkbench.WorkbenchResources
{
    partial class FormResources
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResources));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.treeViewCurrent = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.treeViewNew = new System.Windows.Forms.TreeView();
            this.radioButtonApplication = new System.Windows.Forms.RadioButton();
            this.radioButtonHome = new System.Windows.Forms.RadioButton();
            this.radioButtonMyDocuments = new System.Windows.Forms.RadioButton();
            this.splitContainerTrees = new System.Windows.Forms.SplitContainer();
            this.labelNew = new System.Windows.Forms.Label();
            this.tableLayoutPanelTreeNew = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelTreeCurrent = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTrees)).BeginInit();
            this.splitContainerTrees.Panel1.SuspendLayout();
            this.splitContainerTrees.Panel2.SuspendLayout();
            this.splitContainerTrees.SuspendLayout();
            this.tableLayoutPanelTreeNew.SuspendLayout();
            this.tableLayoutPanelTreeCurrent.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonApplication, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonHome, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.radioButtonMyDocuments, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerTrees, 1, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(798, 422);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(3, 3);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(158, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "Directory for the resources";
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrent.Location = new System.Drawing.Point(3, 0);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(342, 13);
            this.labelCurrent.TabIndex = 1;
            this.labelCurrent.Text = "Current location of the Resources";
            // 
            // treeViewCurrent
            // 
            this.treeViewCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCurrent.ImageIndex = 0;
            this.treeViewCurrent.ImageList = this.imageListTree;
            this.treeViewCurrent.Location = new System.Drawing.Point(3, 16);
            this.treeViewCurrent.Name = "treeViewCurrent";
            this.treeViewCurrent.SelectedImageIndex = 0;
            this.treeViewCurrent.Size = new System.Drawing.Size(342, 397);
            this.treeViewCurrent.TabIndex = 3;
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "NULL.ico");
            this.imageListTree.Images.SetKeyName(1, "Folder.ico");
            this.imageListTree.Images.SetKeyName(2, "FolderGray.ico");
            // 
            // treeViewNew
            // 
            this.treeViewNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewNew.ImageIndex = 0;
            this.treeViewNew.ImageList = this.imageListTree;
            this.treeViewNew.Location = new System.Drawing.Point(3, 16);
            this.treeViewNew.Name = "treeViewNew";
            this.treeViewNew.SelectedImageIndex = 0;
            this.treeViewNew.Size = new System.Drawing.Size(270, 397);
            this.treeViewNew.TabIndex = 4;
            // 
            // radioButtonApplication
            // 
            this.radioButtonApplication.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButtonApplication.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench_3;
            this.radioButtonApplication.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonApplication.Location = new System.Drawing.Point(3, 22);
            this.radioButtonApplication.Name = "radioButtonApplication";
            this.radioButtonApplication.Size = new System.Drawing.Size(158, 30);
            this.radioButtonApplication.TabIndex = 5;
            this.radioButtonApplication.TabStop = true;
            this.radioButtonApplication.Text = "       Application";
            this.toolTip.SetToolTip(this.radioButtonApplication, "Same directory as APPLICATION directory");
            this.radioButtonApplication.UseVisualStyleBackColor = true;
            // 
            // radioButtonHome
            // 
            this.radioButtonHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonHome.Image = global::DiversityWorkbench.Properties.Resources.Home;
            this.radioButtonHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonHome.Location = new System.Drawing.Point(3, 58);
            this.radioButtonHome.Name = "radioButtonHome";
            this.radioButtonHome.Size = new System.Drawing.Size(158, 30);
            this.radioButtonHome.TabIndex = 6;
            this.radioButtonHome.TabStop = true;
            this.radioButtonHome.Text = "      Home";
            this.toolTip.SetToolTip(this.radioButtonHome, "HOME directory of the user");
            this.radioButtonHome.UseVisualStyleBackColor = true;
            // 
            // radioButtonMyDocuments
            // 
            this.radioButtonMyDocuments.Dock = System.Windows.Forms.DockStyle.Top;
            this.radioButtonMyDocuments.Image = global::DiversityWorkbench.Properties.Resources.MyDocuments;
            this.radioButtonMyDocuments.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButtonMyDocuments.Location = new System.Drawing.Point(3, 94);
            this.radioButtonMyDocuments.Name = "radioButtonMyDocuments";
            this.radioButtonMyDocuments.Size = new System.Drawing.Size(158, 30);
            this.radioButtonMyDocuments.TabIndex = 7;
            this.radioButtonMyDocuments.TabStop = true;
            this.radioButtonMyDocuments.Text = "     My Documents";
            this.toolTip.SetToolTip(this.radioButtonMyDocuments, "MY DOCUMENTS directory of the user");
            this.radioButtonMyDocuments.UseVisualStyleBackColor = true;
            // 
            // splitContainerTrees
            // 
            this.splitContainerTrees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTrees.Location = new System.Drawing.Point(167, 3);
            this.splitContainerTrees.Name = "splitContainerTrees";
            // 
            // splitContainerTrees.Panel1
            // 
            this.splitContainerTrees.Panel1.Controls.Add(this.tableLayoutPanelTreeCurrent);
            // 
            // splitContainerTrees.Panel2
            // 
            this.splitContainerTrees.Panel2.Controls.Add(this.tableLayoutPanelTreeNew);
            this.tableLayoutPanelMain.SetRowSpan(this.splitContainerTrees, 4);
            this.splitContainerTrees.Size = new System.Drawing.Size(628, 416);
            this.splitContainerTrees.SplitterDistance = 348;
            this.splitContainerTrees.TabIndex = 8;
            // 
            // labelNew
            // 
            this.labelNew.AutoSize = true;
            this.labelNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNew.Location = new System.Drawing.Point(3, 0);
            this.labelNew.Name = "labelNew";
            this.labelNew.Size = new System.Drawing.Size(270, 13);
            this.labelNew.TabIndex = 2;
            this.labelNew.Text = "Resources";
            // 
            // tableLayoutPanelTreeNew
            // 
            this.tableLayoutPanelTreeNew.ColumnCount = 1;
            this.tableLayoutPanelTreeNew.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTreeNew.Controls.Add(this.labelNew, 0, 0);
            this.tableLayoutPanelTreeNew.Controls.Add(this.treeViewNew, 0, 1);
            this.tableLayoutPanelTreeNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTreeNew.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTreeNew.Name = "tableLayoutPanelTreeNew";
            this.tableLayoutPanelTreeNew.RowCount = 2;
            this.tableLayoutPanelTreeNew.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTreeNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTreeNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTreeNew.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTreeNew.Size = new System.Drawing.Size(276, 416);
            this.tableLayoutPanelTreeNew.TabIndex = 3;
            // 
            // tableLayoutPanelTreeCurrent
            // 
            this.tableLayoutPanelTreeCurrent.ColumnCount = 1;
            this.tableLayoutPanelTreeCurrent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTreeCurrent.Controls.Add(this.labelCurrent, 0, 0);
            this.tableLayoutPanelTreeCurrent.Controls.Add(this.treeViewCurrent, 0, 1);
            this.tableLayoutPanelTreeCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTreeCurrent.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTreeCurrent.Name = "tableLayoutPanelTreeCurrent";
            this.tableLayoutPanelTreeCurrent.RowCount = 2;
            this.tableLayoutPanelTreeCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTreeCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTreeCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTreeCurrent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTreeCurrent.Size = new System.Drawing.Size(348, 416);
            this.tableLayoutPanelTreeCurrent.TabIndex = 0;
            // 
            // FormResources
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 422);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormResources";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resources for the application";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.splitContainerTrees.Panel1.ResumeLayout(false);
            this.splitContainerTrees.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTrees)).EndInit();
            this.splitContainerTrees.ResumeLayout(false);
            this.tableLayoutPanelTreeNew.ResumeLayout(false);
            this.tableLayoutPanelTreeNew.PerformLayout();
            this.tableLayoutPanelTreeCurrent.ResumeLayout(false);
            this.tableLayoutPanelTreeCurrent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.Label labelNew;
        private System.Windows.Forms.TreeView treeViewCurrent;
        private System.Windows.Forms.TreeView treeViewNew;
        private System.Windows.Forms.RadioButton radioButtonApplication;
        private System.Windows.Forms.RadioButton radioButtonHome;
        private System.Windows.Forms.RadioButton radioButtonMyDocuments;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.SplitContainer splitContainerTrees;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTreeNew;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTreeCurrent;
    }
}