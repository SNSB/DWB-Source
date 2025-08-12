namespace DiversityWorkbench.UserControls
{
    partial class UserControlXMLTree
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.labelContent = new System.Windows.Forms.Label();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.labelNode = new System.Windows.Forms.Label();
            this.buttonRemoveNode = new System.Windows.Forms.Button();
            this.buttonAddNode = new System.Windows.Forms.Button();
            this.textBoxNode = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Controls.Add(this.treeView, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.labelContent, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxContent, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelNode, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonRemoveNode, 3, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonAddNode, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxNode, 1, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(380, 304);
            this.tableLayoutPanelMain.TabIndex = 3;
            // 
            // treeView
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.treeView, 4);
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(3, 3);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(374, 234);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelContent.Location = new System.Drawing.Point(3, 272);
            this.labelContent.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(47, 32);
            this.labelContent.TabIndex = 4;
            this.labelContent.Text = "Content:";
            // 
            // textBoxContent
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxContent, 3);
            this.textBoxContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxContent.Location = new System.Drawing.Point(50, 269);
            this.textBoxContent.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxContent.Multiline = true;
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(327, 32);
            this.textBoxContent.TabIndex = 5;
            // 
            // labelNode
            // 
            this.labelNode.AutoSize = true;
            this.labelNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNode.Location = new System.Drawing.Point(3, 240);
            this.labelNode.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelNode.Name = "labelNode";
            this.labelNode.Size = new System.Drawing.Size(47, 26);
            this.labelNode.TabIndex = 8;
            this.labelNode.Text = "Node:";
            this.labelNode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonRemoveNode
            // 
            this.buttonRemoveNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemoveNode.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemoveNode.Location = new System.Drawing.Point(355, 241);
            this.buttonRemoveNode.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.buttonRemoveNode.Name = "buttonRemoveNode";
            this.buttonRemoveNode.Size = new System.Drawing.Size(22, 22);
            this.buttonRemoveNode.TabIndex = 7;
            this.buttonRemoveNode.UseVisualStyleBackColor = true;
            this.buttonRemoveNode.Click += new System.EventHandler(this.buttonRemoveNode_Click);
            // 
            // buttonAddNode
            // 
            this.buttonAddNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddNode.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAddNode.Location = new System.Drawing.Point(327, 241);
            this.buttonAddNode.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.buttonAddNode.Name = "buttonAddNode";
            this.buttonAddNode.Size = new System.Drawing.Size(22, 22);
            this.buttonAddNode.TabIndex = 6;
            this.buttonAddNode.UseVisualStyleBackColor = true;
            this.buttonAddNode.Click += new System.EventHandler(this.buttonAddNode_Click);
            // 
            // textBoxNode
            // 
            this.textBoxNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNode.Location = new System.Drawing.Point(50, 243);
            this.textBoxNode.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxNode.Name = "textBoxNode";
            this.textBoxNode.Size = new System.Drawing.Size(271, 20);
            this.textBoxNode.TabIndex = 9;
            // 
            // UserControlXMLTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlXMLTree";
            this.Size = new System.Drawing.Size(380, 304);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.Button buttonAddNode;
        private System.Windows.Forms.Button buttonRemoveNode;
        private System.Windows.Forms.Label labelNode;
        private System.Windows.Forms.TextBox textBoxNode;
    }
}
