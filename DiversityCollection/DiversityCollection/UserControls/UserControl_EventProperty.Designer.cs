namespace DiversityCollection.UserControls
{
    partial class UserControl_EventProperty
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_EventProperty));
            this.groupBoxEventProperty = new System.Windows.Forms.GroupBox();
            this.pictureBoxEventProperty = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelEventProperty = new System.Windows.Forms.TableLayoutPanel();
            this.userControlModuleRelatedEntryScientificTerm = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelEventPropertyValue = new System.Windows.Forms.Label();
            this.textBoxEventPropertyValue = new System.Windows.Forms.TextBox();
            this.labelHabitatResponsible = new System.Windows.Forms.Label();
            this.userControlModuleRelatedEntryEventPropertyResponsible = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.labelEventPropertyNotes = new System.Windows.Forms.Label();
            this.textBoxEventPropertyNotes = new System.Windows.Forms.TextBox();
            this.labelEventPropertyHierarchyCache = new System.Windows.Forms.Label();
            this.textBoxEventPropertyHierarchyCache = new System.Windows.Forms.TextBox();
            this.labelEventPropertyAverageValueCache = new System.Windows.Forms.Label();
            this.textBoxEventPropertyAverageValueCache = new System.Windows.Forms.TextBox();
            this.buttonEventPropertyHierarchyCacheEdit = new System.Windows.Forms.Button();
            this.groupBoxEventProperty.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEventProperty)).BeginInit();
            this.tableLayoutPanelEventProperty.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxEventProperty
            // 
            this.groupBoxEventProperty.AccessibleName = "CollectionEventProperty";
            this.groupBoxEventProperty.Controls.Add(this.pictureBoxEventProperty);
            this.groupBoxEventProperty.Controls.Add(this.tableLayoutPanelEventProperty);
            this.groupBoxEventProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxEventProperty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEventProperty.ForeColor = System.Drawing.Color.DarkGreen;
            this.groupBoxEventProperty.Location = new System.Drawing.Point(0, 0);
            this.groupBoxEventProperty.MinimumSize = new System.Drawing.Size(0, 126);
            this.groupBoxEventProperty.Name = "groupBoxEventProperty";
            this.groupBoxEventProperty.Size = new System.Drawing.Size(556, 283);
            this.groupBoxEventProperty.TabIndex = 2;
            this.groupBoxEventProperty.TabStop = false;
            this.groupBoxEventProperty.Text = "Property of the collection site";
            // 
            // pictureBoxEventProperty
            // 
            this.pictureBoxEventProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxEventProperty.Image = global::DiversityCollection.Resource.EventProperty;
            this.pictureBoxEventProperty.Location = new System.Drawing.Point(537, 1);
            this.pictureBoxEventProperty.Name = "pictureBoxEventProperty";
            this.pictureBoxEventProperty.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxEventProperty.TabIndex = 1;
            this.pictureBoxEventProperty.TabStop = false;
            // 
            // tableLayoutPanelEventProperty
            // 
            this.tableLayoutPanelEventProperty.ColumnCount = 4;
            this.tableLayoutPanelEventProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventProperty.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEventProperty.Controls.Add(this.userControlModuleRelatedEntryScientificTerm, 0, 0);
            this.tableLayoutPanelEventProperty.Controls.Add(this.labelEventPropertyValue, 0, 2);
            this.tableLayoutPanelEventProperty.Controls.Add(this.textBoxEventPropertyValue, 1, 2);
            this.tableLayoutPanelEventProperty.Controls.Add(this.labelHabitatResponsible, 0, 3);
            this.tableLayoutPanelEventProperty.Controls.Add(this.userControlModuleRelatedEntryEventPropertyResponsible, 1, 3);
            this.tableLayoutPanelEventProperty.Controls.Add(this.labelEventPropertyNotes, 0, 4);
            this.tableLayoutPanelEventProperty.Controls.Add(this.textBoxEventPropertyNotes, 1, 4);
            this.tableLayoutPanelEventProperty.Controls.Add(this.labelEventPropertyHierarchyCache, 0, 1);
            this.tableLayoutPanelEventProperty.Controls.Add(this.textBoxEventPropertyHierarchyCache, 1, 1);
            this.tableLayoutPanelEventProperty.Controls.Add(this.labelEventPropertyAverageValueCache, 2, 2);
            this.tableLayoutPanelEventProperty.Controls.Add(this.textBoxEventPropertyAverageValueCache, 3, 2);
            this.tableLayoutPanelEventProperty.Controls.Add(this.buttonEventPropertyHierarchyCacheEdit, 2, 1);
            this.tableLayoutPanelEventProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEventProperty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelEventProperty.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelEventProperty.Name = "tableLayoutPanelEventProperty";
            this.tableLayoutPanelEventProperty.RowCount = 5;
            this.tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEventProperty.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEventProperty.Size = new System.Drawing.Size(550, 264);
            this.tableLayoutPanelEventProperty.TabIndex = 0;
            // 
            // userControlModuleRelatedEntryScientificTerm
            // 
            this.userControlModuleRelatedEntryScientificTerm.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelEventProperty.SetColumnSpan(this.userControlModuleRelatedEntryScientificTerm, 4);
            this.userControlModuleRelatedEntryScientificTerm.DependsOnUri = "";
            this.userControlModuleRelatedEntryScientificTerm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryScientificTerm.Domain = "";
            this.userControlModuleRelatedEntryScientificTerm.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryScientificTerm.Location = new System.Drawing.Point(2, 3);
            this.userControlModuleRelatedEntryScientificTerm.Margin = new System.Windows.Forms.Padding(2, 3, 3, 0);
            this.userControlModuleRelatedEntryScientificTerm.Module = null;
            this.userControlModuleRelatedEntryScientificTerm.Name = "userControlModuleRelatedEntryScientificTerm";
            this.userControlModuleRelatedEntryScientificTerm.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryScientificTerm.ShowInfo = false;
            this.userControlModuleRelatedEntryScientificTerm.Size = new System.Drawing.Size(545, 23);
            this.userControlModuleRelatedEntryScientificTerm.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryScientificTerm.TabIndex = 0;
            // 
            // labelEventPropertyValue
            // 
            this.labelEventPropertyValue.AccessibleName = "CollectionEventProperty.PropertyValue";
            this.labelEventPropertyValue.AutoSize = true;
            this.labelEventPropertyValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventPropertyValue.Location = new System.Drawing.Point(3, 46);
            this.labelEventPropertyValue.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventPropertyValue.Name = "labelEventPropertyValue";
            this.labelEventPropertyValue.Size = new System.Drawing.Size(55, 23);
            this.labelEventPropertyValue.TabIndex = 1;
            this.labelEventPropertyValue.Text = "Value:";
            this.labelEventPropertyValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelEventPropertyValue.Visible = false;
            // 
            // textBoxEventPropertyValue
            // 
            this.textBoxEventPropertyValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventPropertyValue.Location = new System.Drawing.Point(58, 46);
            this.textBoxEventPropertyValue.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxEventPropertyValue.Name = "textBoxEventPropertyValue";
            this.textBoxEventPropertyValue.Size = new System.Drawing.Size(463, 20);
            this.textBoxEventPropertyValue.TabIndex = 2;
            this.textBoxEventPropertyValue.Visible = false;
            // 
            // labelHabitatResponsible
            // 
            this.labelHabitatResponsible.AccessibleName = "CollectionEventProperty.ResponsibleName";
            this.labelHabitatResponsible.AutoSize = true;
            this.labelHabitatResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHabitatResponsible.Location = new System.Drawing.Point(3, 69);
            this.labelHabitatResponsible.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelHabitatResponsible.Name = "labelHabitatResponsible";
            this.labelHabitatResponsible.Size = new System.Drawing.Size(55, 24);
            this.labelHabitatResponsible.TabIndex = 3;
            this.labelHabitatResponsible.Text = "Respons.:";
            this.labelHabitatResponsible.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // userControlModuleRelatedEntryEventPropertyResponsible
            // 
            this.userControlModuleRelatedEntryEventPropertyResponsible.CanDeleteConnectionToModule = true;
            this.tableLayoutPanelEventProperty.SetColumnSpan(this.userControlModuleRelatedEntryEventPropertyResponsible, 3);
            this.userControlModuleRelatedEntryEventPropertyResponsible.DependsOnUri = "";
            this.userControlModuleRelatedEntryEventPropertyResponsible.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryEventPropertyResponsible.Domain = "";
            this.userControlModuleRelatedEntryEventPropertyResponsible.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryEventPropertyResponsible.Location = new System.Drawing.Point(58, 69);
            this.userControlModuleRelatedEntryEventPropertyResponsible.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.userControlModuleRelatedEntryEventPropertyResponsible.Module = null;
            this.userControlModuleRelatedEntryEventPropertyResponsible.Name = "userControlModuleRelatedEntryEventPropertyResponsible";
            this.userControlModuleRelatedEntryEventPropertyResponsible.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryEventPropertyResponsible.ShowInfo = false;
            this.userControlModuleRelatedEntryEventPropertyResponsible.Size = new System.Drawing.Size(489, 21);
            this.userControlModuleRelatedEntryEventPropertyResponsible.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryEventPropertyResponsible.TabIndex = 4;
            // 
            // labelEventPropertyNotes
            // 
            this.labelEventPropertyNotes.AccessibleName = "CollectionEventProperty.Notes";
            this.labelEventPropertyNotes.AutoSize = true;
            this.labelEventPropertyNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventPropertyNotes.Location = new System.Drawing.Point(3, 96);
            this.labelEventPropertyNotes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.labelEventPropertyNotes.Name = "labelEventPropertyNotes";
            this.labelEventPropertyNotes.Size = new System.Drawing.Size(55, 168);
            this.labelEventPropertyNotes.TabIndex = 5;
            this.labelEventPropertyNotes.Text = "Notes:";
            this.labelEventPropertyNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxEventPropertyNotes
            // 
            this.tableLayoutPanelEventProperty.SetColumnSpan(this.textBoxEventPropertyNotes, 3);
            this.textBoxEventPropertyNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventPropertyNotes.Location = new System.Drawing.Point(58, 93);
            this.textBoxEventPropertyNotes.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.textBoxEventPropertyNotes.Multiline = true;
            this.textBoxEventPropertyNotes.Name = "textBoxEventPropertyNotes";
            this.textBoxEventPropertyNotes.Size = new System.Drawing.Size(489, 168);
            this.textBoxEventPropertyNotes.TabIndex = 6;
            // 
            // labelEventPropertyHierarchyCache
            // 
            this.labelEventPropertyHierarchyCache.AccessibleName = "CollectionEventProperty.PropertyHierarchyCache";
            this.labelEventPropertyHierarchyCache.AutoSize = true;
            this.labelEventPropertyHierarchyCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventPropertyHierarchyCache.Location = new System.Drawing.Point(3, 26);
            this.labelEventPropertyHierarchyCache.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelEventPropertyHierarchyCache.Name = "labelEventPropertyHierarchyCache";
            this.labelEventPropertyHierarchyCache.Size = new System.Drawing.Size(55, 20);
            this.labelEventPropertyHierarchyCache.TabIndex = 7;
            this.labelEventPropertyHierarchyCache.Text = "Hierarchy:";
            this.labelEventPropertyHierarchyCache.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxEventPropertyHierarchyCache
            // 
            this.textBoxEventPropertyHierarchyCache.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEventPropertyHierarchyCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventPropertyHierarchyCache.Location = new System.Drawing.Point(61, 29);
            this.textBoxEventPropertyHierarchyCache.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxEventPropertyHierarchyCache.Name = "textBoxEventPropertyHierarchyCache";
            this.textBoxEventPropertyHierarchyCache.ReadOnly = true;
            this.textBoxEventPropertyHierarchyCache.Size = new System.Drawing.Size(463, 13);
            this.textBoxEventPropertyHierarchyCache.TabIndex = 8;
            // 
            // labelEventPropertyAverageValueCache
            // 
            this.labelEventPropertyAverageValueCache.AccessibleName = "CollectionEventProperty.AverageValueCache";
            this.labelEventPropertyAverageValueCache.AutoSize = true;
            this.labelEventPropertyAverageValueCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventPropertyAverageValueCache.Location = new System.Drawing.Point(527, 46);
            this.labelEventPropertyAverageValueCache.Name = "labelEventPropertyAverageValueCache";
            this.labelEventPropertyAverageValueCache.Size = new System.Drawing.Size(1, 23);
            this.labelEventPropertyAverageValueCache.TabIndex = 9;
            this.labelEventPropertyAverageValueCache.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelEventPropertyAverageValueCache.Visible = false;
            // 
            // textBoxEventPropertyAverageValueCache
            // 
            this.textBoxEventPropertyAverageValueCache.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEventPropertyAverageValueCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEventPropertyAverageValueCache.Location = new System.Drawing.Point(533, 49);
            this.textBoxEventPropertyAverageValueCache.Name = "textBoxEventPropertyAverageValueCache";
            this.textBoxEventPropertyAverageValueCache.ReadOnly = true;
            this.textBoxEventPropertyAverageValueCache.Size = new System.Drawing.Size(14, 13);
            this.textBoxEventPropertyAverageValueCache.TabIndex = 10;
            this.textBoxEventPropertyAverageValueCache.Visible = false;
            // 
            // buttonEventPropertyHierarchyCacheEdit
            // 
            this.tableLayoutPanelEventProperty.SetColumnSpan(this.buttonEventPropertyHierarchyCacheEdit, 2);
            this.buttonEventPropertyHierarchyCacheEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEventPropertyHierarchyCacheEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEventPropertyHierarchyCacheEdit.ForeColor = System.Drawing.Color.Transparent;
            this.buttonEventPropertyHierarchyCacheEdit.Image = global::DiversityCollection.Resource.Edit1;
            this.buttonEventPropertyHierarchyCacheEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonEventPropertyHierarchyCacheEdit.Location = new System.Drawing.Point(524, 26);
            this.buttonEventPropertyHierarchyCacheEdit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEventPropertyHierarchyCacheEdit.Name = "buttonEventPropertyHierarchyCacheEdit";
            this.buttonEventPropertyHierarchyCacheEdit.Size = new System.Drawing.Size(26, 20);
            this.buttonEventPropertyHierarchyCacheEdit.TabIndex = 11;
            this.buttonEventPropertyHierarchyCacheEdit.UseVisualStyleBackColor = true;
            this.buttonEventPropertyHierarchyCacheEdit.Click += new System.EventHandler(this.buttonEventPropertyHierarchyCacheEdit_Click);
            // 
            // UserControl_EventProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxEventProperty);
            this.Name = "UserControl_EventProperty";
            this.Size = new System.Drawing.Size(556, 283);
            this.groupBoxEventProperty.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEventProperty)).EndInit();
            this.tableLayoutPanelEventProperty.ResumeLayout(false);
            this.tableLayoutPanelEventProperty.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxEventProperty;
        private System.Windows.Forms.PictureBox pictureBoxEventProperty;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEventProperty;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryScientificTerm;
        private System.Windows.Forms.Label labelEventPropertyValue;
        private System.Windows.Forms.TextBox textBoxEventPropertyValue;
        private System.Windows.Forms.Label labelHabitatResponsible;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryEventPropertyResponsible;
        private System.Windows.Forms.Label labelEventPropertyNotes;
        private System.Windows.Forms.TextBox textBoxEventPropertyNotes;
        private System.Windows.Forms.Label labelEventPropertyHierarchyCache;
        private System.Windows.Forms.TextBox textBoxEventPropertyHierarchyCache;
        private System.Windows.Forms.Label labelEventPropertyAverageValueCache;
        private System.Windows.Forms.TextBox textBoxEventPropertyAverageValueCache;
        private System.Windows.Forms.Button buttonEventPropertyHierarchyCacheEdit;
    }
}
