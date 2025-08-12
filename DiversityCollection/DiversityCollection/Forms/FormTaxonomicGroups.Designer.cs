namespace DiversityCollection.Forms
{
    partial class FormTaxonomicGroups
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTaxonomicGroups));
            this.treeViewTaxonomicGroups = new System.Windows.Forms.TreeView();
            this.labelHeader = new System.Windows.Forms.Label();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // treeViewTaxonomicGroups
            // 
            this.treeViewTaxonomicGroups.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewTaxonomicGroups.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewTaxonomicGroups.CheckBoxes = true;
            this.treeViewTaxonomicGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTaxonomicGroups.Location = new System.Drawing.Point(8, 63);
            this.treeViewTaxonomicGroups.Name = "treeViewTaxonomicGroups";
            this.treeViewTaxonomicGroups.ShowLines = false;
            this.treeViewTaxonomicGroups.ShowNodeToolTips = true;
            this.treeViewTaxonomicGroups.ShowPlusMinus = false;
            this.treeViewTaxonomicGroups.ShowRootLines = false;
            this.treeViewTaxonomicGroups.Size = new System.Drawing.Size(198, 242);
            this.treeViewTaxonomicGroups.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeader.Location = new System.Drawing.Point(8, 8);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(198, 55);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Choose the taxonomic groups that should be visible in the menu for creating new u" +
                "nits in a specimen";
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(8, 305);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(198, 27);
            this.userControlDialogPanel.TabIndex = 2;
            // 
            // FormTaxonomicGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 340);
            this.Controls.Add(this.treeViewTaxonomicGroups);
            this.Controls.Add(this.userControlDialogPanel);
            this.Controls.Add(this.labelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTaxonomicGroups";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Taxonomic groups";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewTaxonomicGroups;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ToolTip toolTip;
    }
}