namespace DiversityCollection.Forms
{
    partial class FormPresetStorageUnit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPresetStorageUnit));
            this.comboBoxMaterialCategory = new System.Windows.Forms.ComboBox();
            this.labelTaxonomicGroup = new System.Windows.Forms.Label();
            this.tableLayoutPanelStorage = new System.Windows.Forms.TableLayoutPanel();
            this.labelCollection = new System.Windows.Forms.Label();
            this.comboBoxCollection = new System.Windows.Forms.ComboBox();
            this.labelMaterialCategory = new System.Windows.Forms.Label();
            this.comboBoxTaxonomicGroup = new System.Windows.Forms.ComboBox();
            this.labelStorageLocation = new System.Windows.Forms.Label();
            this.textBoxStorageLocation = new System.Windows.Forms.TextBox();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelStorage.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxMaterialCategory
            // 
            this.comboBoxMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxMaterialCategory.FormattingEnabled = true;
            this.comboBoxMaterialCategory.Location = new System.Drawing.Point(101, 30);
            this.comboBoxMaterialCategory.MaxDropDownItems = 20;
            this.comboBoxMaterialCategory.Name = "comboBoxMaterialCategory";
            this.comboBoxMaterialCategory.Size = new System.Drawing.Size(301, 21);
            this.comboBoxMaterialCategory.TabIndex = 3;
            // 
            // labelTaxonomicGroup
            // 
            this.labelTaxonomicGroup.AutoSize = true;
            this.labelTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaxonomicGroup.Location = new System.Drawing.Point(3, 54);
            this.labelTaxonomicGroup.Name = "labelTaxonomicGroup";
            this.labelTaxonomicGroup.Size = new System.Drawing.Size(92, 27);
            this.labelTaxonomicGroup.TabIndex = 4;
            this.labelTaxonomicGroup.Text = "Taxonomic group:";
            this.labelTaxonomicGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanelStorage
            // 
            this.tableLayoutPanelStorage.ColumnCount = 2;
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStorage.Controls.Add(this.labelCollection, 0, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxCollection, 1, 0);
            this.tableLayoutPanelStorage.Controls.Add(this.labelMaterialCategory, 0, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxMaterialCategory, 1, 1);
            this.tableLayoutPanelStorage.Controls.Add(this.labelTaxonomicGroup, 0, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.comboBoxTaxonomicGroup, 1, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.labelStorageLocation, 0, 3);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxStorageLocation, 1, 3);
            this.tableLayoutPanelStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelStorage.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelStorage.Name = "tableLayoutPanelStorage";
            this.tableLayoutPanelStorage.RowCount = 4;
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelStorage.Size = new System.Drawing.Size(405, 107);
            this.tableLayoutPanelStorage.TabIndex = 3;
            // 
            // labelCollection
            // 
            this.labelCollection.AutoSize = true;
            this.labelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollection.Location = new System.Drawing.Point(3, 0);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(92, 27);
            this.labelCollection.TabIndex = 0;
            this.labelCollection.Text = "Collection:";
            this.labelCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxCollection
            // 
            this.comboBoxCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollection.FormattingEnabled = true;
            this.comboBoxCollection.Location = new System.Drawing.Point(101, 3);
            this.comboBoxCollection.MaxDropDownItems = 20;
            this.comboBoxCollection.Name = "comboBoxCollection";
            this.comboBoxCollection.Size = new System.Drawing.Size(301, 21);
            this.comboBoxCollection.TabIndex = 1;
            // 
            // labelMaterialCategory
            // 
            this.labelMaterialCategory.AutoSize = true;
            this.labelMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterialCategory.Location = new System.Drawing.Point(3, 27);
            this.labelMaterialCategory.Name = "labelMaterialCategory";
            this.labelMaterialCategory.Size = new System.Drawing.Size(92, 27);
            this.labelMaterialCategory.TabIndex = 2;
            this.labelMaterialCategory.Text = "Material category:";
            this.labelMaterialCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxTaxonomicGroup
            // 
            this.comboBoxTaxonomicGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTaxonomicGroup.FormattingEnabled = true;
            this.comboBoxTaxonomicGroup.Location = new System.Drawing.Point(101, 57);
            this.comboBoxTaxonomicGroup.MaxDropDownItems = 20;
            this.comboBoxTaxonomicGroup.Name = "comboBoxTaxonomicGroup";
            this.comboBoxTaxonomicGroup.Size = new System.Drawing.Size(301, 21);
            this.comboBoxTaxonomicGroup.TabIndex = 5;
            // 
            // labelStorageLocation
            // 
            this.labelStorageLocation.AutoSize = true;
            this.labelStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStorageLocation.Location = new System.Drawing.Point(3, 81);
            this.labelStorageLocation.Name = "labelStorageLocation";
            this.labelStorageLocation.Size = new System.Drawing.Size(92, 26);
            this.labelStorageLocation.TabIndex = 6;
            this.labelStorageLocation.Text = "Storage location:";
            this.labelStorageLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxStorageLocation
            // 
            this.textBoxStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStorageLocation.Location = new System.Drawing.Point(101, 84);
            this.textBoxStorageLocation.Name = "textBoxStorageLocation";
            this.textBoxStorageLocation.Size = new System.Drawing.Size(301, 20);
            this.textBoxStorageLocation.TabIndex = 7;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 107);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(405, 27);
            this.userControlDialogPanel.TabIndex = 2;
            // 
            // FormPresetStorageUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 134);
            this.Controls.Add(this.tableLayoutPanelStorage);
            this.Controls.Add(this.userControlDialogPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPresetStorageUnit";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Set storage unit";
            this.tableLayoutPanelStorage.ResumeLayout(false);
            this.tableLayoutPanelStorage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMaterialCategory;
        private System.Windows.Forms.Label labelTaxonomicGroup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStorage;
        private System.Windows.Forms.Label labelCollection;
        private System.Windows.Forms.ComboBox comboBoxCollection;
        private System.Windows.Forms.Label labelMaterialCategory;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.ComboBox comboBoxTaxonomicGroup;
        private System.Windows.Forms.Label labelStorageLocation;
        private System.Windows.Forms.TextBox textBoxStorageLocation;
    }
}