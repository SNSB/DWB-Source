namespace DiversityCollection
{
    partial class FormManagers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManagers));
            this.tableLayoutPanelManagers = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxUser = new System.Windows.Forms.ListBox();
            this.listBoxAdministratingCollection = new System.Windows.Forms.ListBox();
            this.buttonCollectionAdd = new System.Windows.Forms.Button();
            this.listBoxCollectionCurator = new System.Windows.Forms.ListBox();
            this.buttonCollectionRemove = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelCollectionCurator = new System.Windows.Forms.Label();
            this.labelCollections = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCurators = new System.Windows.Forms.TabPage();
            this.tabPageExternal = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelExternal = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxExternalUser = new System.Windows.Forms.ListBox();
            this.listBoxExternalAdminColl = new System.Windows.Forms.ListBox();
            this.buttonExternalAdd = new System.Windows.Forms.Button();
            this.listBoxExternalCredential = new System.Windows.Forms.ListBox();
            this.buttonExternalRemove = new System.Windows.Forms.Button();
            this.labelCredentialList = new System.Windows.Forms.Label();
            this.labelExternalCollection = new System.Windows.Forms.Label();
            this.comboBoxExternalCollection = new System.Windows.Forms.ComboBox();
            this.labelExternalUser = new System.Windows.Forms.Label();
            this.labelExternalAdminColl = new System.Windows.Forms.Label();
            this.dataSetTransaction = new DiversityCollection.DataSetTransaction();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelManagers.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageCurators.SuspendLayout();
            this.tabPageExternal.SuspendLayout();
            this.tableLayoutPanelExternal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelCurators
            // 
            this.tableLayoutPanelManagers.ColumnCount = 5;
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelManagers.Controls.Add(this.listBoxUser, 0, 1);
            this.tableLayoutPanelManagers.Controls.Add(this.listBoxAdministratingCollection, 4, 1);
            this.tableLayoutPanelManagers.Controls.Add(this.buttonCollectionAdd, 3, 1);
            this.tableLayoutPanelManagers.Controls.Add(this.listBoxCollectionCurator, 2, 1);
            this.tableLayoutPanelManagers.Controls.Add(this.buttonCollectionRemove, 3, 2);
            this.tableLayoutPanelManagers.Controls.Add(this.labelUser, 0, 0);
            this.tableLayoutPanelManagers.Controls.Add(this.labelCollectionCurator, 2, 0);
            this.tableLayoutPanelManagers.Controls.Add(this.labelCollections, 4, 0);
            this.tableLayoutPanelManagers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelManagers.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelManagers.Name = "tableLayoutPanelCurators";
            this.tableLayoutPanelManagers.RowCount = 3;
            this.tableLayoutPanelManagers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelManagers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelManagers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelManagers.Size = new System.Drawing.Size(897, 550);
            this.tableLayoutPanelManagers.TabIndex = 4;
            // 
            // listBoxUser
            // 
            this.listBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUser.FormattingEnabled = true;
            this.listBoxUser.IntegralHeight = false;
            this.listBoxUser.Location = new System.Drawing.Point(615, 16);
            this.listBoxUser.Name = "listBoxUser";
            this.tableLayoutPanelManagers.SetRowSpan(this.listBoxUser, 2);
            this.listBoxUser.Size = new System.Drawing.Size(279, 531);
            this.listBoxUser.TabIndex = 0;
            this.toolTip.SetToolTip(this.listBoxUser, "The list of the users in the database");
            this.listBoxUser.SelectedIndexChanged += new System.EventHandler(this.listBoxUser_SelectedIndexChanged);
            // 
            // listBoxAdministratingCollection
            // 
            this.listBoxAdministratingCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAdministratingCollection.FormattingEnabled = true;
            this.listBoxAdministratingCollection.IntegralHeight = false;
            this.listBoxAdministratingCollection.Location = new System.Drawing.Point(3, 16);
            this.listBoxAdministratingCollection.Name = "listBoxAdministratingCollection";
            this.tableLayoutPanelManagers.SetRowSpan(this.listBoxAdministratingCollection, 2);
            this.listBoxAdministratingCollection.Size = new System.Drawing.Size(281, 531);
            this.listBoxAdministratingCollection.TabIndex = 2;
            this.toolTip.SetToolTip(this.listBoxAdministratingCollection, "The list of the collections in the database");
            // 
            // buttonCollectionAdd
            // 
            this.buttonCollectionAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCollectionAdd.Location = new System.Drawing.Point(287, 255);
            this.buttonCollectionAdd.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonCollectionAdd.Name = "buttonCollectionAdd";
            this.buttonCollectionAdd.Size = new System.Drawing.Size(20, 23);
            this.buttonCollectionAdd.TabIndex = 1;
            this.buttonCollectionAdd.Text = "<";
            this.toolTip.SetToolTip(this.buttonCollectionAdd, "Move the selected collection to the list that can be administrated by a user");
            this.buttonCollectionAdd.UseVisualStyleBackColor = true;
            this.buttonCollectionAdd.Click += new System.EventHandler(this.buttonCollectionAdd_Click);
            // 
            // listBoxCollectionCurator
            // 
            this.listBoxCollectionCurator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCollectionCurator.FormattingEnabled = true;
            this.listBoxCollectionCurator.IntegralHeight = false;
            this.listBoxCollectionCurator.Location = new System.Drawing.Point(310, 16);
            this.listBoxCollectionCurator.Name = "listBoxCollectionCurator";
            this.tableLayoutPanelManagers.SetRowSpan(this.listBoxCollectionCurator, 2);
            this.listBoxCollectionCurator.Size = new System.Drawing.Size(279, 531);
            this.listBoxCollectionCurator.TabIndex = 1;
            this.toolTip.SetToolTip(this.listBoxCollectionCurator, "The list of the collections for which the selected user is a curator");
            // 
            // buttonCollectionRemove
            // 
            this.buttonCollectionRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonCollectionRemove.Location = new System.Drawing.Point(287, 284);
            this.buttonCollectionRemove.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonCollectionRemove.Name = "buttonCollectionRemove";
            this.buttonCollectionRemove.Size = new System.Drawing.Size(20, 23);
            this.buttonCollectionRemove.TabIndex = 2;
            this.buttonCollectionRemove.Text = ">";
            this.toolTip.SetToolTip(this.buttonCollectionRemove, "Remove the selected collection from the list that can be administrated by a curat" +
                    "or");
            this.buttonCollectionRemove.UseVisualStyleBackColor = true;
            this.buttonCollectionRemove.Click += new System.EventHandler(this.buttonCollectionRemove_Click);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUser.Location = new System.Drawing.Point(615, 0);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(279, 13);
            this.labelUser.TabIndex = 3;
            this.labelUser.Text = "Collection managers";
            this.labelUser.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelCollectionCurator
            // 
            this.labelCollectionCurator.AutoSize = true;
            this.labelCollectionCurator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionCurator.Location = new System.Drawing.Point(310, 0);
            this.labelCollectionCurator.Name = "labelCollectionCurator";
            this.labelCollectionCurator.Size = new System.Drawing.Size(279, 13);
            this.labelCollectionCurator.TabIndex = 4;
            this.labelCollectionCurator.Text = "Collections administrated by a collection manager";
            this.labelCollectionCurator.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelCollections
            // 
            this.labelCollections.AutoSize = true;
            this.labelCollections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollections.Location = new System.Drawing.Point(3, 0);
            this.labelCollections.Name = "labelCollections";
            this.labelCollections.Size = new System.Drawing.Size(281, 13);
            this.labelCollections.TabIndex = 5;
            this.labelCollections.Text = "Collections";
            this.labelCollections.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageCurators);
            this.tabControl.Controls.Add(this.tabPageExternal);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(2, 2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(911, 582);
            this.tabControl.TabIndex = 5;
            // 
            // tabPageCurators
            // 
            this.tabPageCurators.Controls.Add(this.tableLayoutPanelManagers);
            this.tabPageCurators.Location = new System.Drawing.Point(4, 22);
            this.tabPageCurators.Name = "tabPageCurators";
            this.tabPageCurators.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCurators.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabPageCurators.Size = new System.Drawing.Size(903, 556);
            this.tabPageCurators.TabIndex = 0;
            this.tabPageCurators.Text = "Collection managers";
            this.tabPageCurators.UseVisualStyleBackColor = true;
            // 
            // tabPageExternal
            // 
            this.tabPageExternal.Controls.Add(this.tableLayoutPanelExternal);
            this.tabPageExternal.Location = new System.Drawing.Point(4, 22);
            this.tabPageExternal.Name = "tabPageExternal";
            this.tabPageExternal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExternal.Size = new System.Drawing.Size(903, 556);
            this.tabPageExternal.TabIndex = 1;
            this.tabPageExternal.Text = "External request credentials";
            this.tabPageExternal.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelExternal
            // 
            this.tableLayoutPanelExternal.ColumnCount = 5;
            this.tableLayoutPanelExternal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelExternal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelExternal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelExternal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelExternal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelExternal.Controls.Add(this.listBoxExternalUser, 0, 1);
            this.tableLayoutPanelExternal.Controls.Add(this.listBoxExternalAdminColl, 4, 1);
            this.tableLayoutPanelExternal.Controls.Add(this.buttonExternalAdd, 3, 3);
            this.tableLayoutPanelExternal.Controls.Add(this.listBoxExternalCredential, 2, 3);
            this.tableLayoutPanelExternal.Controls.Add(this.buttonExternalRemove, 3, 4);
            this.tableLayoutPanelExternal.Controls.Add(this.labelCredentialList, 2, 2);
            this.tableLayoutPanelExternal.Controls.Add(this.labelExternalCollection, 2, 0);
            this.tableLayoutPanelExternal.Controls.Add(this.comboBoxExternalCollection, 2, 1);
            this.tableLayoutPanelExternal.Controls.Add(this.labelExternalUser, 0, 0);
            this.tableLayoutPanelExternal.Controls.Add(this.labelExternalAdminColl, 4, 0);
            this.tableLayoutPanelExternal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelExternal.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelExternal.Name = "tableLayoutPanelExternal";
            this.tableLayoutPanelExternal.RowCount = 5;
            this.tableLayoutPanelExternal.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelExternal.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelExternal.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelExternal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelExternal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelExternal.Size = new System.Drawing.Size(897, 550);
            this.tableLayoutPanelExternal.TabIndex = 5;
            // 
            // listBoxExternalUser
            // 
            this.listBoxExternalUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxExternalUser.FormattingEnabled = true;
            this.listBoxExternalUser.IntegralHeight = false;
            this.listBoxExternalUser.Location = new System.Drawing.Point(3, 16);
            this.listBoxExternalUser.Name = "listBoxExternalUser";
            this.tableLayoutPanelExternal.SetRowSpan(this.listBoxExternalUser, 4);
            this.listBoxExternalUser.Size = new System.Drawing.Size(279, 531);
            this.listBoxExternalUser.TabIndex = 0;
            this.listBoxExternalUser.SelectedIndexChanged += new System.EventHandler(this.listBoxExternalUser_SelectedIndexChanged);
            // 
            // listBoxExternalAdminColl
            // 
            this.listBoxExternalAdminColl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxExternalAdminColl.FormattingEnabled = true;
            this.listBoxExternalAdminColl.IntegralHeight = false;
            this.listBoxExternalAdminColl.Location = new System.Drawing.Point(613, 16);
            this.listBoxExternalAdminColl.Name = "listBoxExternalAdminColl";
            this.tableLayoutPanelExternal.SetRowSpan(this.listBoxExternalAdminColl, 4);
            this.listBoxExternalAdminColl.Size = new System.Drawing.Size(281, 531);
            this.listBoxExternalAdminColl.TabIndex = 2;
            // 
            // buttonExternalAdd
            // 
            this.buttonExternalAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonExternalAdd.Location = new System.Drawing.Point(590, 275);
            this.buttonExternalAdd.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonExternalAdd.Name = "buttonExternalAdd";
            this.buttonExternalAdd.Size = new System.Drawing.Size(20, 23);
            this.buttonExternalAdd.TabIndex = 1;
            this.buttonExternalAdd.Text = "<";
            this.buttonExternalAdd.UseVisualStyleBackColor = true;
            this.buttonExternalAdd.Click += new System.EventHandler(this.buttonExternalAdd_Click);
            // 
            // listBoxExternalCredential
            // 
            this.listBoxExternalCredential.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxExternalCredential.FormattingEnabled = true;
            this.listBoxExternalCredential.IntegralHeight = false;
            this.listBoxExternalCredential.Location = new System.Drawing.Point(308, 56);
            this.listBoxExternalCredential.Name = "listBoxExternalCredential";
            this.tableLayoutPanelExternal.SetRowSpan(this.listBoxExternalCredential, 2);
            this.listBoxExternalCredential.Size = new System.Drawing.Size(279, 491);
            this.listBoxExternalCredential.TabIndex = 1;
            // 
            // buttonExternalRemove
            // 
            this.buttonExternalRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonExternalRemove.Location = new System.Drawing.Point(590, 304);
            this.buttonExternalRemove.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonExternalRemove.Name = "buttonExternalRemove";
            this.buttonExternalRemove.Size = new System.Drawing.Size(20, 23);
            this.buttonExternalRemove.TabIndex = 2;
            this.buttonExternalRemove.Text = ">";
            this.buttonExternalRemove.UseVisualStyleBackColor = true;
            this.buttonExternalRemove.Click += new System.EventHandler(this.buttonExternalRemove_Click);
            // 
            // labelCredentialList
            // 
            this.labelCredentialList.AutoSize = true;
            this.labelCredentialList.Location = new System.Drawing.Point(308, 40);
            this.labelCredentialList.Name = "labelCredentialList";
            this.labelCredentialList.Size = new System.Drawing.Size(97, 13);
            this.labelCredentialList.TabIndex = 3;
            this.labelCredentialList.Text = "Credentials for user";
            // 
            // labelExternalCollection
            // 
            this.labelExternalCollection.AutoSize = true;
            this.labelExternalCollection.Location = new System.Drawing.Point(308, 0);
            this.labelExternalCollection.Name = "labelExternalCollection";
            this.labelExternalCollection.Size = new System.Drawing.Size(93, 13);
            this.labelExternalCollection.TabIndex = 4;
            this.labelExternalCollection.Text = "External collection";
            // 
            // comboBoxExternalCollection
            // 
            this.comboBoxExternalCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxExternalCollection.FormattingEnabled = true;
            this.comboBoxExternalCollection.Location = new System.Drawing.Point(308, 16);
            this.comboBoxExternalCollection.Name = "comboBoxExternalCollection";
            this.comboBoxExternalCollection.Size = new System.Drawing.Size(279, 21);
            this.comboBoxExternalCollection.TabIndex = 5;
            // 
            // labelExternalUser
            // 
            this.labelExternalUser.AutoSize = true;
            this.labelExternalUser.Location = new System.Drawing.Point(3, 0);
            this.labelExternalUser.Name = "labelExternalUser";
            this.labelExternalUser.Size = new System.Drawing.Size(68, 13);
            this.labelExternalUser.TabIndex = 6;
            this.labelExternalUser.Text = "External user";
            // 
            // labelExternalAdminColl
            // 
            this.labelExternalAdminColl.AutoSize = true;
            this.labelExternalAdminColl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExternalAdminColl.Location = new System.Drawing.Point(613, 0);
            this.labelExternalAdminColl.Name = "labelExternalAdminColl";
            this.labelExternalAdminColl.Size = new System.Drawing.Size(281, 13);
            this.labelExternalAdminColl.TabIndex = 7;
            this.labelExternalAdminColl.Text = "Available collections";
            // 
            // dataSetTransaction
            // 
            this.dataSetTransaction.DataSetName = "DataSetTransaction";
            this.dataSetTransaction.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FormCurators
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 586);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCurators";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administation of collection managers and external request credentials";
            this.tableLayoutPanelManagers.ResumeLayout(false);
            this.tableLayoutPanelManagers.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageCurators.ResumeLayout(false);
            this.tabPageExternal.ResumeLayout(false);
            this.tableLayoutPanelExternal.ResumeLayout(false);
            this.tableLayoutPanelExternal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCurators;
        private System.Windows.Forms.ListBox listBoxUser;
        private System.Windows.Forms.ListBox listBoxAdministratingCollection;
        private System.Windows.Forms.Button buttonCollectionAdd;
        private System.Windows.Forms.ListBox listBoxCollectionCurator;
        private System.Windows.Forms.Button buttonCollectionRemove;
        private DataSetTransaction dataSetTransaction;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageCurators;
        private System.Windows.Forms.TabPage tabPageExternal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelExternal;
        private System.Windows.Forms.ListBox listBoxExternalUser;
        private System.Windows.Forms.ListBox listBoxExternalAdminColl;
        private System.Windows.Forms.Button buttonExternalAdd;
        private System.Windows.Forms.ListBox listBoxExternalCredential;
        private System.Windows.Forms.Button buttonExternalRemove;
        private System.Windows.Forms.Label labelCredentialList;
        private System.Windows.Forms.Label labelExternalCollection;
        private System.Windows.Forms.ComboBox comboBoxExternalCollection;
        private System.Windows.Forms.Label labelExternalUser;
        private System.Windows.Forms.Label labelExternalAdminColl;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelCollectionCurator;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelCollections;
    }
}