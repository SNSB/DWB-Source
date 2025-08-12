namespace DiversityCollection.Forms
{
    partial class FormNewStorage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewStorage));
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelStorage = new System.Windows.Forms.TableLayoutPanel();
            this.labelCollection = new System.Windows.Forms.Label();
            this.comboBoxCollection = new System.Windows.Forms.ComboBox();
            this.labelMaterialCategory = new System.Windows.Forms.Label();
            this.comboBoxMaterialCategory = new System.Windows.Forms.ComboBox();
            this.labelStorageLocation = new System.Windows.Forms.Label();
            this.textBoxStorageLocation = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelStorage.SuspendLayout();
            this.SuspendLayout();
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(4, 85);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(377, 27);
            this.userControlDialogPanel.TabIndex = 0;
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
            this.tableLayoutPanelStorage.Controls.Add(this.labelStorageLocation, 0, 2);
            this.tableLayoutPanelStorage.Controls.Add(this.textBoxStorageLocation, 1, 2);
            this.tableLayoutPanelStorage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelStorage.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanelStorage.Name = "tableLayoutPanelStorage";
            this.tableLayoutPanelStorage.RowCount = 3;
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStorage.Size = new System.Drawing.Size(377, 81);
            this.tableLayoutPanelStorage.TabIndex = 1;
            // 
            // labelCollection
            // 
            this.labelCollection.AutoSize = true;
            this.labelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollection.Location = new System.Drawing.Point(3, 0);
            this.labelCollection.Name = "labelCollection";
            this.labelCollection.Size = new System.Drawing.Size(91, 27);
            this.labelCollection.TabIndex = 0;
            this.labelCollection.Text = "Collection:";
            this.labelCollection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxCollection
            // 
            this.comboBoxCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCollection.FormattingEnabled = true;
            this.comboBoxCollection.Location = new System.Drawing.Point(100, 3);
            this.comboBoxCollection.Name = "comboBoxCollection";
            this.comboBoxCollection.Size = new System.Drawing.Size(274, 21);
            this.comboBoxCollection.TabIndex = 1;
            // 
            // labelMaterialCategory
            // 
            this.labelMaterialCategory.AutoSize = true;
            this.labelMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaterialCategory.Location = new System.Drawing.Point(3, 27);
            this.labelMaterialCategory.Name = "labelMaterialCategory";
            this.labelMaterialCategory.Size = new System.Drawing.Size(91, 27);
            this.labelMaterialCategory.TabIndex = 2;
            this.labelMaterialCategory.Text = "Material category:";
            this.labelMaterialCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxMaterialCategory
            // 
            this.comboBoxMaterialCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxMaterialCategory.FormattingEnabled = true;
            this.comboBoxMaterialCategory.Location = new System.Drawing.Point(100, 30);
            this.comboBoxMaterialCategory.Name = "comboBoxMaterialCategory";
            this.comboBoxMaterialCategory.Size = new System.Drawing.Size(274, 21);
            this.comboBoxMaterialCategory.TabIndex = 3;
            // 
            // labelStorageLocation
            // 
            this.labelStorageLocation.AutoSize = true;
            this.labelStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStorageLocation.Location = new System.Drawing.Point(3, 54);
            this.labelStorageLocation.Name = "labelStorageLocation";
            this.labelStorageLocation.Size = new System.Drawing.Size(91, 27);
            this.labelStorageLocation.TabIndex = 4;
            this.labelStorageLocation.Text = "Storage location:";
            this.labelStorageLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxStorageLocation
            // 
            this.textBoxStorageLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStorageLocation.Location = new System.Drawing.Point(100, 57);
            this.textBoxStorageLocation.Name = "textBoxStorageLocation";
            this.textBoxStorageLocation.Size = new System.Drawing.Size(274, 20);
            this.textBoxStorageLocation.TabIndex = 5;
            // 
            // FormNewStorage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 116);
            this.Controls.Add(this.tableLayoutPanelStorage);
            this.Controls.Add(this.userControlDialogPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormNewStorage";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " New storage location";
            this.tableLayoutPanelStorage.ResumeLayout(false);
            this.tableLayoutPanelStorage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelStorage;
        private System.Windows.Forms.Label labelCollection;
        private System.Windows.Forms.ComboBox comboBoxCollection;
        private System.Windows.Forms.Label labelMaterialCategory;
        private System.Windows.Forms.ComboBox comboBoxMaterialCategory;
        private System.Windows.Forms.Label labelStorageLocation;
        private System.Windows.Forms.TextBox textBoxStorageLocation;
    }
}