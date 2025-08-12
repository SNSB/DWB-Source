namespace DiversityCollection.UserControls
{
    partial class UserControl_Exsiccata
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Exsiccata));
            this.splitContainerExsiccata = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanelExsiccataSeries = new System.Windows.Forms.TableLayoutPanel();
            this.userControlModuleRelatedEntryExsiccate = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelExsiccataAbbreviation = new System.Windows.Forms.Label();
            this.tableLayoutPanelExsiccataUnit = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxExsiccataNumber = new System.Windows.Forms.TextBox();
            this.labelExsiccataNumber = new System.Windows.Forms.Label();
            this.labelExsiccataIdentification = new System.Windows.Forms.Label();
            this.comboBoxExsiccataIdentification = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerExsiccata)).BeginInit();
            this.splitContainerExsiccata.Panel1.SuspendLayout();
            this.splitContainerExsiccata.Panel2.SuspendLayout();
            this.splitContainerExsiccata.SuspendLayout();
            this.tableLayoutPanelExsiccataSeries.SuspendLayout();
            this.tableLayoutPanelExsiccataUnit.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // splitContainerExsiccata
            // 
            this.splitContainerExsiccata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerExsiccata.Location = new System.Drawing.Point(0, 0);
            this.splitContainerExsiccata.Name = "splitContainerExsiccata";
            this.splitContainerExsiccata.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerExsiccata.Panel1
            // 
            this.splitContainerExsiccata.Panel1.Controls.Add(this.tableLayoutPanelExsiccataSeries);
            // 
            // splitContainerExsiccata.Panel2
            // 
            this.splitContainerExsiccata.Panel2.Controls.Add(this.tableLayoutPanelExsiccataUnit);
            this.splitContainerExsiccata.Size = new System.Drawing.Size(658, 150);
            this.splitContainerExsiccata.SplitterDistance = 66;
            this.splitContainerExsiccata.TabIndex = 0;
            // 
            // tableLayoutPanelExsiccataSeries
            // 
            this.tableLayoutPanelExsiccataSeries.ColumnCount = 2;
            this.tableLayoutPanelExsiccataSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExsiccataSeries.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExsiccataSeries.Controls.Add(this.userControlModuleRelatedEntryExsiccate, 1, 0);
            this.tableLayoutPanelExsiccataSeries.Controls.Add(this.labelExsiccataAbbreviation, 0, 0);
            this.tableLayoutPanelExsiccataSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelExsiccataSeries.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelExsiccataSeries.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelExsiccataSeries.Name = "tableLayoutPanelExsiccataSeries";
            this.tableLayoutPanelExsiccataSeries.RowCount = 1;
            this.tableLayoutPanelExsiccataSeries.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelExsiccataSeries.Size = new System.Drawing.Size(658, 66);
            this.tableLayoutPanelExsiccataSeries.TabIndex = 1;
            // 
            // userControlModuleRelatedEntryExsiccate
            // 
            this.userControlModuleRelatedEntryExsiccate.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryExsiccate.DependsOnUri = "";
            this.userControlModuleRelatedEntryExsiccate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryExsiccate.Domain = "";
            this.userControlModuleRelatedEntryExsiccate.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryExsiccate.Location = new System.Drawing.Point(89, 0);
            this.userControlModuleRelatedEntryExsiccate.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.userControlModuleRelatedEntryExsiccate.Module = null;
            this.userControlModuleRelatedEntryExsiccate.Name = "userControlModuleRelatedEntryExsiccate";
            this.userControlModuleRelatedEntryExsiccate.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryExsiccate.ShowInfo = false;
            this.userControlModuleRelatedEntryExsiccate.Size = new System.Drawing.Size(566, 66);
            this.userControlModuleRelatedEntryExsiccate.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryExsiccate.TabIndex = 0;
            // 
            // labelExsiccataAbbreviation
            // 
            this.labelExsiccataAbbreviation.AccessibleName = "CollectionSpecimen.ExsiccataAbbreviation";
            this.labelExsiccataAbbreviation.AutoSize = true;
            this.labelExsiccataAbbreviation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExsiccataAbbreviation.Location = new System.Drawing.Point(3, 5);
            this.labelExsiccataAbbreviation.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.labelExsiccataAbbreviation.Name = "labelExsiccataAbbreviation";
            this.labelExsiccataAbbreviation.Size = new System.Drawing.Size(86, 61);
            this.labelExsiccataAbbreviation.TabIndex = 1;
            this.labelExsiccataAbbreviation.Text = "Exsiccata series:";
            this.labelExsiccataAbbreviation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tableLayoutPanelExsiccataUnit
            // 
            this.tableLayoutPanelExsiccataUnit.ColumnCount = 4;
            this.tableLayoutPanelExsiccataUnit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExsiccataUnit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExsiccataUnit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelExsiccataUnit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanelExsiccataUnit.Controls.Add(this.textBoxExsiccataNumber, 3, 0);
            this.tableLayoutPanelExsiccataUnit.Controls.Add(this.labelExsiccataNumber, 2, 0);
            this.tableLayoutPanelExsiccataUnit.Controls.Add(this.labelExsiccataIdentification, 0, 0);
            this.tableLayoutPanelExsiccataUnit.Controls.Add(this.comboBoxExsiccataIdentification, 1, 0);
            this.tableLayoutPanelExsiccataUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelExsiccataUnit.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelExsiccataUnit.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelExsiccataUnit.Name = "tableLayoutPanelExsiccataUnit";
            this.tableLayoutPanelExsiccataUnit.RowCount = 1;
            this.tableLayoutPanelExsiccataUnit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelExsiccataUnit.Size = new System.Drawing.Size(658, 80);
            this.tableLayoutPanelExsiccataUnit.TabIndex = 1;
            // 
            // textBoxExsiccataNumber
            // 
            this.textBoxExsiccataNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExsiccataNumber.Location = new System.Drawing.Point(598, 0);
            this.textBoxExsiccataNumber.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxExsiccataNumber.Name = "textBoxExsiccataNumber";
            this.textBoxExsiccataNumber.Size = new System.Drawing.Size(57, 20);
            this.textBoxExsiccataNumber.TabIndex = 5;
            // 
            // labelExsiccataNumber
            // 
            this.labelExsiccataNumber.AccessibleName = "IdentificationUnit.ExsiccataNumber";
            this.labelExsiccataNumber.AutoSize = true;
            this.labelExsiccataNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExsiccataNumber.Location = new System.Drawing.Point(548, 5);
            this.labelExsiccataNumber.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.labelExsiccataNumber.Name = "labelExsiccataNumber";
            this.labelExsiccataNumber.Size = new System.Drawing.Size(50, 75);
            this.labelExsiccataNumber.TabIndex = 4;
            this.labelExsiccataNumber.Text = "Exs. No.:";
            this.labelExsiccataNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelExsiccataIdentification
            // 
            this.labelExsiccataIdentification.AccessibleName = "IdentificationUnit.ExsiccataIdentification";
            this.labelExsiccataIdentification.AutoSize = true;
            this.labelExsiccataIdentification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExsiccataIdentification.Location = new System.Drawing.Point(3, 5);
            this.labelExsiccataIdentification.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
            this.labelExsiccataIdentification.Name = "labelExsiccataIdentification";
            this.labelExsiccataIdentification.Size = new System.Drawing.Size(85, 75);
            this.labelExsiccataIdentification.TabIndex = 2;
            this.labelExsiccataIdentification.Text = "Exsiccata ident.:";
            this.labelExsiccataIdentification.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxExsiccataIdentification
            // 
            this.comboBoxExsiccataIdentification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxExsiccataIdentification.FormattingEnabled = true;
            this.comboBoxExsiccataIdentification.Location = new System.Drawing.Point(88, 0);
            this.comboBoxExsiccataIdentification.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.comboBoxExsiccataIdentification.Name = "comboBoxExsiccataIdentification";
            this.comboBoxExsiccataIdentification.Size = new System.Drawing.Size(454, 21);
            this.comboBoxExsiccataIdentification.TabIndex = 3;
            // 
            // UserControl_Exsiccata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerExsiccata);
            this.Name = "UserControl_Exsiccata";
            this.Size = new System.Drawing.Size(658, 150);
            this.splitContainerExsiccata.Panel1.ResumeLayout(false);
            this.splitContainerExsiccata.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerExsiccata)).EndInit();
            this.splitContainerExsiccata.ResumeLayout(false);
            this.tableLayoutPanelExsiccataSeries.ResumeLayout(false);
            this.tableLayoutPanelExsiccataSeries.PerformLayout();
            this.tableLayoutPanelExsiccataUnit.ResumeLayout(false);
            this.tableLayoutPanelExsiccataUnit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerExsiccata;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelExsiccataSeries;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryExsiccate;
        private System.Windows.Forms.Label labelExsiccataAbbreviation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelExsiccataUnit;
        private System.Windows.Forms.TextBox textBoxExsiccataNumber;
        private System.Windows.Forms.Label labelExsiccataNumber;
        private System.Windows.Forms.Label labelExsiccataIdentification;
        private System.Windows.Forms.ComboBox comboBoxExsiccataIdentification;

    }
}
