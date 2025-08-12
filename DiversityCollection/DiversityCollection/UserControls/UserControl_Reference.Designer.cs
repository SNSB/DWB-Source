namespace DiversityCollection.UserControls
{
    partial class UserControl_Reference
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Reference));
            this.groupBoxReference = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelReference = new System.Windows.Forms.TableLayoutPanel();
            this.userControlModuleRelatedEntryReference = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.userControlModuleRelatedEntryReferenceResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelReferenceDetails = new System.Windows.Forms.Label();
            this.labelReferenceNotes = new System.Windows.Forms.Label();
            this.textBoxReferenceNotes = new System.Windows.Forms.TextBox();
            this.textBoxReferenceDetails = new System.Windows.Forms.TextBox();
            this.labelReferenceResponsible = new System.Windows.Forms.Label();
            this.pictureBoxReference = new System.Windows.Forms.PictureBox();
            this.groupBoxReference.SuspendLayout();
            this.tableLayoutPanelReference.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReference)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxReference
            // 
            this.groupBoxReference.Controls.Add(this.tableLayoutPanelReference);
            this.groupBoxReference.Controls.Add(this.pictureBoxReference);
            this.groupBoxReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxReference.Location = new System.Drawing.Point(0, 0);
            this.groupBoxReference.Name = "groupBoxReference";
            this.groupBoxReference.Size = new System.Drawing.Size(662, 339);
            this.groupBoxReference.TabIndex = 1;
            this.groupBoxReference.TabStop = false;
            this.groupBoxReference.Text = "Reference";
            // 
            // tableLayoutPanelReference
            // 
            this.tableLayoutPanelReference.ColumnCount = 2;
            this.tableLayoutPanelReference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelReference.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReference.Controls.Add(this.userControlModuleRelatedEntryReference, 0, 0);
            this.tableLayoutPanelReference.Controls.Add(this.userControlModuleRelatedEntryReferenceResponsible, 1, 2);
            this.tableLayoutPanelReference.Controls.Add(this.labelReferenceDetails, 0, 1);
            this.tableLayoutPanelReference.Controls.Add(this.labelReferenceNotes, 0, 3);
            this.tableLayoutPanelReference.Controls.Add(this.textBoxReferenceNotes, 0, 4);
            this.tableLayoutPanelReference.Controls.Add(this.textBoxReferenceDetails, 1, 1);
            this.tableLayoutPanelReference.Controls.Add(this.labelReferenceResponsible, 0, 2);
            this.tableLayoutPanelReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelReference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelReference.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelReference.Name = "tableLayoutPanelReference";
            this.tableLayoutPanelReference.RowCount = 5;
            this.tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelReference.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelReference.Size = new System.Drawing.Size(656, 320);
            this.tableLayoutPanelReference.TabIndex = 1;
            // 
            // userControlModuleRelatedEntryReference
            // 
            this.userControlModuleRelatedEntryReference.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelReference.SetColumnSpan(this.userControlModuleRelatedEntryReference, 2);
            this.userControlModuleRelatedEntryReference.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryReference.Domain = "";
            this.userControlModuleRelatedEntryReference.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryReference.Location = new System.Drawing.Point(3, 3);
            this.userControlModuleRelatedEntryReference.Module = null;
            this.userControlModuleRelatedEntryReference.Name = "userControlModuleRelatedEntryReference";
            this.userControlModuleRelatedEntryReference.ShowInfo = false;
            this.userControlModuleRelatedEntryReference.Size = new System.Drawing.Size(650, 22);
            this.userControlModuleRelatedEntryReference.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryReference.TabIndex = 0;
            // 
            // userControlModuleRelatedEntryReferenceResponsible
            // 
            this.userControlModuleRelatedEntryReferenceResponsible.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryReferenceResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryReferenceResponsible.Domain = "";
            this.userControlModuleRelatedEntryReferenceResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryReferenceResponsible.Location = new System.Drawing.Point(58, 57);
            this.userControlModuleRelatedEntryReferenceResponsible.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.userControlModuleRelatedEntryReferenceResponsible.Module = null;
            this.userControlModuleRelatedEntryReferenceResponsible.Name = "userControlModuleRelatedEntryReferenceResponsible";
            this.userControlModuleRelatedEntryReferenceResponsible.ShowInfo = false;
            this.userControlModuleRelatedEntryReferenceResponsible.Size = new System.Drawing.Size(595, 22);
            this.userControlModuleRelatedEntryReferenceResponsible.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryReferenceResponsible.TabIndex = 1;
            // 
            // labelReferenceDetails
            // 
            this.labelReferenceDetails.AutoSize = true;
            this.labelReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReferenceDetails.Location = new System.Drawing.Point(3, 28);
            this.labelReferenceDetails.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelReferenceDetails.Name = "labelReferenceDetails";
            this.labelReferenceDetails.Size = new System.Drawing.Size(55, 26);
            this.labelReferenceDetails.TabIndex = 2;
            this.labelReferenceDetails.Text = "Details:";
            this.labelReferenceDetails.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelReferenceNotes
            // 
            this.labelReferenceNotes.AutoSize = true;
            this.labelReferenceNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReferenceNotes.Location = new System.Drawing.Point(3, 82);
            this.labelReferenceNotes.Name = "labelReferenceNotes";
            this.labelReferenceNotes.Size = new System.Drawing.Size(52, 13);
            this.labelReferenceNotes.TabIndex = 3;
            this.labelReferenceNotes.Text = "Notes";
            // 
            // textBoxReferenceNotes
            // 
            this.tableLayoutPanelReference.SetColumnSpan(this.textBoxReferenceNotes, 2);
            this.textBoxReferenceNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReferenceNotes.Location = new System.Drawing.Point(3, 98);
            this.textBoxReferenceNotes.Multiline = true;
            this.textBoxReferenceNotes.Name = "textBoxReferenceNotes";
            this.textBoxReferenceNotes.Size = new System.Drawing.Size(650, 219);
            this.textBoxReferenceNotes.TabIndex = 4;
            // 
            // textBoxReferenceDetails
            // 
            this.textBoxReferenceDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxReferenceDetails.Location = new System.Drawing.Point(58, 31);
            this.textBoxReferenceDetails.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxReferenceDetails.Name = "textBoxReferenceDetails";
            this.textBoxReferenceDetails.Size = new System.Drawing.Size(595, 20);
            this.textBoxReferenceDetails.TabIndex = 5;
            // 
            // labelReferenceResponsible
            // 
            this.labelReferenceResponsible.AutoSize = true;
            this.labelReferenceResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReferenceResponsible.Location = new System.Drawing.Point(3, 54);
            this.labelReferenceResponsible.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelReferenceResponsible.Name = "labelReferenceResponsible";
            this.labelReferenceResponsible.Size = new System.Drawing.Size(55, 28);
            this.labelReferenceResponsible.TabIndex = 6;
            this.labelReferenceResponsible.Text = "Respons.:";
            this.labelReferenceResponsible.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBoxReference
            // 
            this.pictureBoxReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxReference.Image = global::DiversityCollection.Resource.References;
            this.pictureBoxReference.Location = new System.Drawing.Point(640, 0);
            this.pictureBoxReference.Name = "pictureBoxReference";
            this.pictureBoxReference.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxReference.TabIndex = 0;
            this.pictureBoxReference.TabStop = false;
            // 
            // UserControl_Reference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxReference);
            this.Name = "UserControl_Reference";
            this.Size = new System.Drawing.Size(662, 339);
            this.groupBoxReference.ResumeLayout(false);
            this.tableLayoutPanelReference.ResumeLayout(false);
            this.tableLayoutPanelReference.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReference)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxReference;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelReference;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryReference;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryReferenceResponsible;
        private System.Windows.Forms.Label labelReferenceDetails;
        private System.Windows.Forms.Label labelReferenceNotes;
        private System.Windows.Forms.TextBox textBoxReferenceNotes;
        private System.Windows.Forms.TextBox textBoxReferenceDetails;
        private System.Windows.Forms.Label labelReferenceResponsible;
        private System.Windows.Forms.PictureBox pictureBoxReference;
    }
}
