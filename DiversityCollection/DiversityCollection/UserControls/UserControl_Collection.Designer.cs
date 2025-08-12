namespace DiversityCollection.UserControls
{
    partial class UserControl_Collection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Collection));
            this.groupBoxCollection = new System.Windows.Forms.GroupBox();
            this.pictureBoxCollection = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelCollection = new System.Windows.Forms.TableLayoutPanel();
            this.labelCollectionName = new System.Windows.Forms.Label();
            this.buttonOpenFormCollection = new System.Windows.Forms.Button();
            this.textBoxCollectionName = new System.Windows.Forms.TextBox();
            this.labelCollectionAcronym = new System.Windows.Forms.Label();
            this.textBoxCollectionAcronym = new System.Windows.Forms.TextBox();
            this.labelCollectionContact = new System.Windows.Forms.Label();
            this.labelCollectionDescription = new System.Windows.Forms.Label();
            this.textBoxCollectionDescription = new System.Windows.Forms.TextBox();
            this.labelCollectionLocation = new System.Windows.Forms.Label();
            this.textBoxCollectionLocation = new System.Windows.Forms.TextBox();
            this.labelCollectionOwner = new System.Windows.Forms.Label();
            this.textBoxCollectionOwner = new System.Windows.Forms.TextBox();
            this.groupBoxDefaultCollection = new System.Windows.Forms.GroupBox();
            this.textBoxDefaultCollection = new System.Windows.Forms.TextBox();
            this.buttonEditDefaultCollection = new System.Windows.Forms.Button();
            this.userControlModuleRelatedEntryCollectionContact = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.groupBoxCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCollection)).BeginInit();
            this.tableLayoutPanelCollection.SuspendLayout();
            this.groupBoxDefaultCollection.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxCollection
            // 
            this.groupBoxCollection.AccessibleName = "Collection";
            this.groupBoxCollection.Controls.Add(this.pictureBoxCollection);
            this.groupBoxCollection.Controls.Add(this.tableLayoutPanelCollection);
            this.groupBoxCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxCollection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCollection.ForeColor = System.Drawing.Color.Brown;
            this.groupBoxCollection.Location = new System.Drawing.Point(0, 0);
            this.groupBoxCollection.MinimumSize = new System.Drawing.Size(0, 176);
            this.groupBoxCollection.Name = "groupBoxCollection";
            this.groupBoxCollection.Size = new System.Drawing.Size(569, 181);
            this.groupBoxCollection.TabIndex = 1;
            this.groupBoxCollection.TabStop = false;
            this.groupBoxCollection.Text = "Collection";
            // 
            // pictureBoxCollection
            // 
            this.pictureBoxCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCollection.Image = global::DiversityCollection.Resource.Collection;
            this.pictureBoxCollection.Location = new System.Drawing.Point(549, 1);
            this.pictureBoxCollection.Name = "pictureBoxCollection";
            this.pictureBoxCollection.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxCollection.TabIndex = 1;
            this.pictureBoxCollection.TabStop = false;
            // 
            // tableLayoutPanelCollection
            // 
            this.tableLayoutPanelCollection.ColumnCount = 3;
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelCollection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelCollection.Controls.Add(this.labelCollectionName, 0, 0);
            this.tableLayoutPanelCollection.Controls.Add(this.buttonOpenFormCollection, 2, 1);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxCollectionName, 1, 0);
            this.tableLayoutPanelCollection.Controls.Add(this.labelCollectionAcronym, 0, 1);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxCollectionAcronym, 1, 1);
            this.tableLayoutPanelCollection.Controls.Add(this.labelCollectionContact, 0, 2);
            this.tableLayoutPanelCollection.Controls.Add(this.userControlModuleRelatedEntryCollectionContact, 1, 2);
            this.tableLayoutPanelCollection.Controls.Add(this.labelCollectionDescription, 0, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxCollectionDescription, 1, 3);
            this.tableLayoutPanelCollection.Controls.Add(this.labelCollectionLocation, 0, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxCollectionLocation, 1, 4);
            this.tableLayoutPanelCollection.Controls.Add(this.labelCollectionOwner, 0, 5);
            this.tableLayoutPanelCollection.Controls.Add(this.textBoxCollectionOwner, 1, 5);
            this.tableLayoutPanelCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCollection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelCollection.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelCollection.Name = "tableLayoutPanelCollection";
            this.tableLayoutPanelCollection.RowCount = 6;
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCollection.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelCollection.Size = new System.Drawing.Size(563, 162);
            this.tableLayoutPanelCollection.TabIndex = 0;
            // 
            // labelCollectionName
            // 
            this.labelCollectionName.AccessibleName = "Collection.CollectionName";
            this.labelCollectionName.AutoSize = true;
            this.labelCollectionName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionName.Location = new System.Drawing.Point(3, 0);
            this.labelCollectionName.Name = "labelCollectionName";
            this.labelCollectionName.Size = new System.Drawing.Size(52, 26);
            this.labelCollectionName.TabIndex = 0;
            this.labelCollectionName.Text = "Name:";
            this.labelCollectionName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonOpenFormCollection
            // 
            this.buttonOpenFormCollection.Image = global::DiversityCollection.Resource.Collection;
            this.buttonOpenFormCollection.Location = new System.Drawing.Point(537, 29);
            this.buttonOpenFormCollection.Name = "buttonOpenFormCollection";
            this.buttonOpenFormCollection.Size = new System.Drawing.Size(23, 23);
            this.buttonOpenFormCollection.TabIndex = 1;
            this.buttonOpenFormCollection.UseVisualStyleBackColor = true;
            this.buttonOpenFormCollection.Click += new System.EventHandler(this.buttonOpenFormCollection_Click);
            // 
            // textBoxCollectionName
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxCollectionName, 2);
            this.textBoxCollectionName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionName.Location = new System.Drawing.Point(61, 3);
            this.textBoxCollectionName.Name = "textBoxCollectionName";
            this.textBoxCollectionName.Size = new System.Drawing.Size(499, 20);
            this.textBoxCollectionName.TabIndex = 2;
            this.textBoxCollectionName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxCollectionName_KeyUp);
            // 
            // labelCollectionAcronym
            // 
            this.labelCollectionAcronym.AccessibleName = "Collection.CollectionAcronym";
            this.labelCollectionAcronym.AutoSize = true;
            this.labelCollectionAcronym.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionAcronym.Location = new System.Drawing.Point(3, 26);
            this.labelCollectionAcronym.Name = "labelCollectionAcronym";
            this.labelCollectionAcronym.Size = new System.Drawing.Size(52, 29);
            this.labelCollectionAcronym.TabIndex = 3;
            this.labelCollectionAcronym.Text = "Acronym:";
            this.labelCollectionAcronym.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionAcronym
            // 
            this.textBoxCollectionAcronym.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionAcronym.Location = new System.Drawing.Point(61, 29);
            this.textBoxCollectionAcronym.Name = "textBoxCollectionAcronym";
            this.textBoxCollectionAcronym.Size = new System.Drawing.Size(470, 20);
            this.textBoxCollectionAcronym.TabIndex = 4;
            // 
            // labelCollectionContact
            // 
            this.labelCollectionContact.AccessibleName = "Collection.AdministrativeContactName";
            this.labelCollectionContact.AutoSize = true;
            this.labelCollectionContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionContact.Location = new System.Drawing.Point(3, 55);
            this.labelCollectionContact.Name = "labelCollectionContact";
            this.labelCollectionContact.Size = new System.Drawing.Size(52, 28);
            this.labelCollectionContact.TabIndex = 5;
            this.labelCollectionContact.Text = "Contact:";
            this.labelCollectionContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCollectionDescription
            // 
            this.labelCollectionDescription.AccessibleName = "Collection.Description";
            this.labelCollectionDescription.AutoSize = true;
            this.labelCollectionDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionDescription.Location = new System.Drawing.Point(3, 89);
            this.labelCollectionDescription.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelCollectionDescription.Name = "labelCollectionDescription";
            this.labelCollectionDescription.Size = new System.Drawing.Size(52, 20);
            this.labelCollectionDescription.TabIndex = 7;
            this.labelCollectionDescription.Text = "Descript.:";
            this.labelCollectionDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxCollectionDescription
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxCollectionDescription, 2);
            this.textBoxCollectionDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionDescription.Location = new System.Drawing.Point(61, 86);
            this.textBoxCollectionDescription.Multiline = true;
            this.textBoxCollectionDescription.Name = "textBoxCollectionDescription";
            this.textBoxCollectionDescription.Size = new System.Drawing.Size(499, 20);
            this.textBoxCollectionDescription.TabIndex = 8;
            // 
            // labelCollectionLocation
            // 
            this.labelCollectionLocation.AccessibleName = "Collection.Location";
            this.labelCollectionLocation.AutoSize = true;
            this.labelCollectionLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionLocation.Location = new System.Drawing.Point(3, 115);
            this.labelCollectionLocation.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.labelCollectionLocation.Name = "labelCollectionLocation";
            this.labelCollectionLocation.Size = new System.Drawing.Size(52, 20);
            this.labelCollectionLocation.TabIndex = 9;
            this.labelCollectionLocation.Text = "Location:";
            this.labelCollectionLocation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxCollectionLocation
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxCollectionLocation, 2);
            this.textBoxCollectionLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionLocation.Location = new System.Drawing.Point(61, 112);
            this.textBoxCollectionLocation.Multiline = true;
            this.textBoxCollectionLocation.Name = "textBoxCollectionLocation";
            this.textBoxCollectionLocation.Size = new System.Drawing.Size(499, 20);
            this.textBoxCollectionLocation.TabIndex = 10;
            // 
            // labelCollectionOwner
            // 
            this.labelCollectionOwner.AccessibleName = "Collection.CollectionOwner";
            this.labelCollectionOwner.AutoSize = true;
            this.labelCollectionOwner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionOwner.Location = new System.Drawing.Point(3, 135);
            this.labelCollectionOwner.Name = "labelCollectionOwner";
            this.labelCollectionOwner.Size = new System.Drawing.Size(52, 27);
            this.labelCollectionOwner.TabIndex = 11;
            this.labelCollectionOwner.Text = "Owner:";
            this.labelCollectionOwner.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCollectionOwner
            // 
            this.tableLayoutPanelCollection.SetColumnSpan(this.textBoxCollectionOwner, 2);
            this.textBoxCollectionOwner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxCollectionOwner.Location = new System.Drawing.Point(61, 138);
            this.textBoxCollectionOwner.Name = "textBoxCollectionOwner";
            this.textBoxCollectionOwner.Size = new System.Drawing.Size(499, 20);
            this.textBoxCollectionOwner.TabIndex = 12;
            // 
            // groupBoxDefaultCollection
            // 
            this.groupBoxDefaultCollection.Controls.Add(this.textBoxDefaultCollection);
            this.groupBoxDefaultCollection.Controls.Add(this.buttonEditDefaultCollection);
            this.groupBoxDefaultCollection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxDefaultCollection.ForeColor = System.Drawing.Color.Brown;
            this.groupBoxDefaultCollection.Location = new System.Drawing.Point(0, 181);
            this.groupBoxDefaultCollection.Name = "groupBoxDefaultCollection";
            this.groupBoxDefaultCollection.Size = new System.Drawing.Size(569, 39);
            this.groupBoxDefaultCollection.TabIndex = 2;
            this.groupBoxDefaultCollection.TabStop = false;
            this.groupBoxDefaultCollection.Text = "Default collection for new specimen parts";
            // 
            // textBoxDefaultCollection
            // 
            this.textBoxDefaultCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDefaultCollection.ForeColor = System.Drawing.Color.Brown;
            this.textBoxDefaultCollection.Location = new System.Drawing.Point(3, 16);
            this.textBoxDefaultCollection.Name = "textBoxDefaultCollection";
            this.textBoxDefaultCollection.ReadOnly = true;
            this.textBoxDefaultCollection.Size = new System.Drawing.Size(539, 20);
            this.textBoxDefaultCollection.TabIndex = 1;
            // 
            // buttonEditDefaultCollection
            // 
            this.buttonEditDefaultCollection.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonEditDefaultCollection.FlatAppearance.BorderSize = 0;
            this.buttonEditDefaultCollection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditDefaultCollection.Image = global::DiversityCollection.Resource.Edit1;
            this.buttonEditDefaultCollection.Location = new System.Drawing.Point(542, 16);
            this.buttonEditDefaultCollection.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonEditDefaultCollection.Name = "buttonEditDefaultCollection";
            this.buttonEditDefaultCollection.Size = new System.Drawing.Size(24, 20);
            this.buttonEditDefaultCollection.TabIndex = 0;
            this.buttonEditDefaultCollection.UseVisualStyleBackColor = true;
            // 
            // userControlModuleRelatedEntryCollectionContact
            // 
            this.userControlModuleRelatedEntryCollectionContact.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelCollection.SetColumnSpan(this.userControlModuleRelatedEntryCollectionContact, 2);
            this.userControlModuleRelatedEntryCollectionContact.DependsOnUri = "";
            this.userControlModuleRelatedEntryCollectionContact.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryCollectionContact.Domain = "";
            this.userControlModuleRelatedEntryCollectionContact.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryCollectionContact.Location = new System.Drawing.Point(61, 58);
            this.userControlModuleRelatedEntryCollectionContact.Module = null;
            this.userControlModuleRelatedEntryCollectionContact.Name = "userControlModuleRelatedEntryCollectionContact";
            this.userControlModuleRelatedEntryCollectionContact.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryCollectionContact.ShowInfo = false;
            this.userControlModuleRelatedEntryCollectionContact.Size = new System.Drawing.Size(499, 22);
            this.userControlModuleRelatedEntryCollectionContact.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryCollectionContact.TabIndex = 6;
            // 
            // UserControl_Collection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCollection);
            this.Controls.Add(this.groupBoxDefaultCollection);
            this.Name = "UserControl_Collection";
            this.Size = new System.Drawing.Size(569, 220);
            this.groupBoxCollection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCollection)).EndInit();
            this.tableLayoutPanelCollection.ResumeLayout(false);
            this.tableLayoutPanelCollection.PerformLayout();
            this.groupBoxDefaultCollection.ResumeLayout(false);
            this.groupBoxDefaultCollection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCollection;
        private System.Windows.Forms.PictureBox pictureBoxCollection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCollection;
        private System.Windows.Forms.Label labelCollectionName;
        private System.Windows.Forms.Button buttonOpenFormCollection;
        private System.Windows.Forms.TextBox textBoxCollectionName;
        private System.Windows.Forms.Label labelCollectionAcronym;
        private System.Windows.Forms.TextBox textBoxCollectionAcronym;
        private System.Windows.Forms.Label labelCollectionContact;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryCollectionContact;
        private System.Windows.Forms.Label labelCollectionDescription;
        private System.Windows.Forms.TextBox textBoxCollectionDescription;
        private System.Windows.Forms.Label labelCollectionLocation;
        private System.Windows.Forms.TextBox textBoxCollectionLocation;
        private System.Windows.Forms.Label labelCollectionOwner;
        private System.Windows.Forms.TextBox textBoxCollectionOwner;
        private System.Windows.Forms.GroupBox groupBoxDefaultCollection;
        private System.Windows.Forms.TextBox textBoxDefaultCollection;
        private System.Windows.Forms.Button buttonEditDefaultCollection;
    }
}
