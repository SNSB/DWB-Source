namespace DiversityCollection.UserControls
{
    partial class UserControlSpecimenList
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
            this.groupBoxSpecimenList = new System.Windows.Forms.GroupBox();
            this.listBoxSpecimenList = new System.Windows.Forms.ListBox();
            this.toolStripSpecimenList = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTransfer = new System.Windows.Forms.ToolStripButton();
            this.groupBoxSpecimenList.SuspendLayout();
            this.toolStripSpecimenList.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSpecimenList
            // 
            this.groupBoxSpecimenList.Controls.Add(this.listBoxSpecimenList);
            this.groupBoxSpecimenList.Controls.Add(this.toolStripSpecimenList);
            this.groupBoxSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSpecimenList.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSpecimenList.Name = "groupBoxSpecimenList";
            this.groupBoxSpecimenList.Size = new System.Drawing.Size(150, 413);
            this.groupBoxSpecimenList.TabIndex = 0;
            this.groupBoxSpecimenList.TabStop = false;
            this.groupBoxSpecimenList.Text = "Specimen";
            // 
            // listBoxSpecimenList
            // 
            this.listBoxSpecimenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSpecimenList.FormattingEnabled = true;
            this.listBoxSpecimenList.IntegralHeight = false;
            this.listBoxSpecimenList.Location = new System.Drawing.Point(3, 16);
            this.listBoxSpecimenList.Name = "listBoxSpecimenList";
            this.listBoxSpecimenList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSpecimenList.Size = new System.Drawing.Size(144, 371);
            this.listBoxSpecimenList.TabIndex = 1;
            // 
            // toolStripSpecimenList
            // 
            this.toolStripSpecimenList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripSpecimenList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFind,
            this.toolStripButtonTransfer,
            this.toolStripButtonDelete});
            this.toolStripSpecimenList.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripSpecimenList.Location = new System.Drawing.Point(3, 387);
            this.toolStripSpecimenList.Name = "toolStripSpecimenList";
            this.toolStripSpecimenList.Size = new System.Drawing.Size(144, 23);
            this.toolStripSpecimenList.TabIndex = 0;
            this.toolStripSpecimenList.Text = "toolStrip1";
            // 
            // toolStripButtonFind
            // 
            this.toolStripButtonFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFind.Image = global::DiversityCollection.Resource.Search;
            this.toolStripButtonFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFind.Name = "toolStripButtonFind";
            this.toolStripButtonFind.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonFind.Text = "Change to this specimen in main form";
            this.toolStripButtonFind.Click += new System.EventHandler(this.toolStripButtonFind_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonDelete.Text = "Remove the selected specimen from the list";
            this.toolStripButtonDelete.Visible = false;
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonTransfer
            // 
            this.toolStripButtonTransfer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTransfer.Image = global::DiversityCollection.Resource.Arrow;
            this.toolStripButtonTransfer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTransfer.Name = "toolStripButtonTransfer";
            this.toolStripButtonTransfer.Size = new System.Drawing.Size(23, 20);
            this.toolStripButtonTransfer.Text = "Transfer";
            this.toolStripButtonTransfer.Visible = false;
            // 
            // UserControlSpecimenList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSpecimenList);
            this.Name = "UserControlSpecimenList";
            this.Size = new System.Drawing.Size(150, 413);
            this.groupBoxSpecimenList.ResumeLayout(false);
            this.groupBoxSpecimenList.PerformLayout();
            this.toolStripSpecimenList.ResumeLayout(false);
            this.toolStripSpecimenList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSpecimenList;
        private System.Windows.Forms.ToolStrip toolStripSpecimenList;
        private System.Windows.Forms.ToolStripButton toolStripButtonFind;
        public System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        public System.Windows.Forms.ListBox listBoxSpecimenList;
        public System.Windows.Forms.ToolStripButton toolStripButtonTransfer;
    }
}
