namespace DiversityCollection.Forms
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
            this.listBoxCollectionManager = new System.Windows.Forms.ListBox();
            this.buttonCollectionRemove = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelCollectionManager = new System.Windows.Forms.Label();
            this.labelCollections = new System.Windows.Forms.Label();
            this.toolStripManagers = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelManagerListBy = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxManagerListBy = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripButtonSynchronizeLogins = new System.Windows.Forms.ToolStripButton();
            this.buttonFeedback = new System.Windows.Forms.Button();
            this.dataSetTransaction = new DiversityCollection.Datasets.DataSetTransaction();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.buttonCollectionDetails = new System.Windows.Forms.Button();
            this.labelCollectionDetails = new System.Windows.Forms.Label();
            this.tableLayoutPanelManagers.SuspendLayout();
            this.toolStripManagers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelManagers
            // 
            this.tableLayoutPanelManagers.ColumnCount = 6;
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelManagers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelManagers.Controls.Add(this.listBoxUser, 0, 1);
            this.tableLayoutPanelManagers.Controls.Add(this.listBoxAdministratingCollection, 4, 1);
            this.tableLayoutPanelManagers.Controls.Add(this.buttonCollectionAdd, 3, 1);
            this.tableLayoutPanelManagers.Controls.Add(this.listBoxCollectionManager, 2, 1);
            this.tableLayoutPanelManagers.Controls.Add(this.buttonCollectionRemove, 3, 2);
            this.tableLayoutPanelManagers.Controls.Add(this.labelUser, 0, 0);
            this.tableLayoutPanelManagers.Controls.Add(this.labelCollectionManager, 2, 0);
            this.tableLayoutPanelManagers.Controls.Add(this.labelCollections, 4, 0);
            this.tableLayoutPanelManagers.Controls.Add(this.toolStripManagers, 0, 3);
            this.tableLayoutPanelManagers.Controls.Add(this.buttonFeedback, 5, 0);
            this.tableLayoutPanelManagers.Controls.Add(this.buttonCollectionDetails, 5, 3);
            this.tableLayoutPanelManagers.Controls.Add(this.labelCollectionDetails, 4, 3);
            this.tableLayoutPanelManagers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelManagers.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanelManagers.Name = "tableLayoutPanelManagers";
            this.tableLayoutPanelManagers.RowCount = 4;
            this.tableLayoutPanelManagers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelManagers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelManagers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelManagers.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelManagers.Size = new System.Drawing.Size(880, 457);
            this.tableLayoutPanelManagers.TabIndex = 4;
            // 
            // listBoxUser
            // 
            this.listBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUser.FormattingEnabled = true;
            this.listBoxUser.IntegralHeight = false;
            this.listBoxUser.Location = new System.Drawing.Point(3, 19);
            this.listBoxUser.Name = "listBoxUser";
            this.tableLayoutPanelManagers.SetRowSpan(this.listBoxUser, 2);
            this.listBoxUser.Size = new System.Drawing.Size(157, 410);
            this.listBoxUser.TabIndex = 0;
            this.toolTip.SetToolTip(this.listBoxUser, "The list of the users in the database");
            this.listBoxUser.SelectedIndexChanged += new System.EventHandler(this.listBoxUser_SelectedIndexChanged);
            // 
            // listBoxAdministratingCollection
            // 
            this.listBoxAdministratingCollection.BackColor = System.Drawing.Color.Pink;
            this.tableLayoutPanelManagers.SetColumnSpan(this.listBoxAdministratingCollection, 2);
            this.listBoxAdministratingCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAdministratingCollection.FormattingEnabled = true;
            this.listBoxAdministratingCollection.IntegralHeight = false;
            this.listBoxAdministratingCollection.Location = new System.Drawing.Point(532, 19);
            this.listBoxAdministratingCollection.Name = "listBoxAdministratingCollection";
            this.tableLayoutPanelManagers.SetRowSpan(this.listBoxAdministratingCollection, 2);
            this.listBoxAdministratingCollection.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxAdministratingCollection.Size = new System.Drawing.Size(345, 410);
            this.listBoxAdministratingCollection.TabIndex = 2;
            this.toolTip.SetToolTip(this.listBoxAdministratingCollection, "The list of the collections in the database");
            this.listBoxAdministratingCollection.SelectedIndexChanged += new System.EventHandler(this.listBoxAdministratingCollection_SelectedIndexChanged);
            // 
            // buttonCollectionAdd
            // 
            this.buttonCollectionAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCollectionAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCollectionAdd.ForeColor = System.Drawing.Color.Green;
            this.buttonCollectionAdd.Location = new System.Drawing.Point(509, 198);
            this.buttonCollectionAdd.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonCollectionAdd.Name = "buttonCollectionAdd";
            this.buttonCollectionAdd.Size = new System.Drawing.Size(20, 23);
            this.buttonCollectionAdd.TabIndex = 1;
            this.buttonCollectionAdd.Text = "<";
            this.toolTip.SetToolTip(this.buttonCollectionAdd, "Move the selected collection to the list that can be administrated by a user");
            this.buttonCollectionAdd.UseVisualStyleBackColor = true;
            this.buttonCollectionAdd.Click += new System.EventHandler(this.buttonCollectionAdd_Click);
            // 
            // listBoxCollectionManager
            // 
            this.listBoxCollectionManager.BackColor = System.Drawing.Color.LightGreen;
            this.listBoxCollectionManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCollectionManager.FormattingEnabled = true;
            this.listBoxCollectionManager.IntegralHeight = false;
            this.listBoxCollectionManager.Location = new System.Drawing.Point(186, 19);
            this.listBoxCollectionManager.Name = "listBoxCollectionManager";
            this.tableLayoutPanelManagers.SetRowSpan(this.listBoxCollectionManager, 3);
            this.listBoxCollectionManager.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxCollectionManager.Size = new System.Drawing.Size(320, 435);
            this.listBoxCollectionManager.TabIndex = 1;
            this.toolTip.SetToolTip(this.listBoxCollectionManager, "The list of the collections for which the selected user is a Manager");
            // 
            // buttonCollectionRemove
            // 
            this.buttonCollectionRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonCollectionRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCollectionRemove.ForeColor = System.Drawing.Color.Red;
            this.buttonCollectionRemove.Location = new System.Drawing.Point(509, 227);
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
            this.labelUser.Location = new System.Drawing.Point(3, 0);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(157, 16);
            this.labelUser.TabIndex = 3;
            this.labelUser.Text = "Collection managers";
            this.labelUser.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelCollectionManager
            // 
            this.labelCollectionManager.AutoSize = true;
            this.labelCollectionManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionManager.Location = new System.Drawing.Point(186, 0);
            this.labelCollectionManager.Name = "labelCollectionManager";
            this.labelCollectionManager.Size = new System.Drawing.Size(320, 16);
            this.labelCollectionManager.TabIndex = 4;
            this.labelCollectionManager.Text = "Collections administrated by  manager";
            this.labelCollectionManager.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelCollections
            // 
            this.labelCollections.AutoSize = true;
            this.labelCollections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollections.Location = new System.Drawing.Point(532, 0);
            this.labelCollections.Name = "labelCollections";
            this.labelCollections.Size = new System.Drawing.Size(320, 16);
            this.labelCollections.TabIndex = 5;
            this.labelCollections.Text = "Available collections";
            this.labelCollections.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolStripManagers
            // 
            this.toolStripManagers.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripManagers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelManagerListBy,
            this.toolStripComboBoxManagerListBy,
            this.toolStripButtonSynchronizeLogins});
            this.toolStripManagers.Location = new System.Drawing.Point(0, 432);
            this.toolStripManagers.Name = "toolStripManagers";
            this.toolStripManagers.Size = new System.Drawing.Size(163, 25);
            this.toolStripManagers.TabIndex = 6;
            this.toolStripManagers.Text = "toolStrip1";
            // 
            // toolStripLabelManagerListBy
            // 
            this.toolStripLabelManagerListBy.Name = "toolStripLabelManagerListBy";
            this.toolStripLabelManagerListBy.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabelManagerListBy.Text = "List by:";
            // 
            // toolStripComboBoxManagerListBy
            // 
            this.toolStripComboBoxManagerListBy.DropDownWidth = 90;
            this.toolStripComboBoxManagerListBy.Items.AddRange(new object[] {
            "Login",
            "Agent name"});
            this.toolStripComboBoxManagerListBy.Name = "toolStripComboBoxManagerListBy";
            this.toolStripComboBoxManagerListBy.Size = new System.Drawing.Size(90, 25);
            this.toolStripComboBoxManagerListBy.Text = "Agent name";
            this.toolStripComboBoxManagerListBy.DropDownClosed += new System.EventHandler(this.toolStripComboBoxManagerListBy_DropDownClosed);
            this.toolStripComboBoxManagerListBy.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxManagerListBy_SelectedIndexChanged);
            // 
            // toolStripButtonSynchronizeLogins
            // 
            this.toolStripButtonSynchronizeLogins.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSynchronizeLogins.Image = global::DiversityCollection.Resource.SynchronizeAgent;
            this.toolStripButtonSynchronizeLogins.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSynchronizeLogins.Name = "toolStripButtonSynchronizeLogins";
            this.toolStripButtonSynchronizeLogins.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSynchronizeLogins.Text = "Synchronize logins with server logins";
            this.toolStripButtonSynchronizeLogins.Click += new System.EventHandler(this.toolStripButtonSynchronizeLogins_Click);
            // 
            // buttonFeedback
            // 
            this.buttonFeedback.FlatAppearance.BorderSize = 0;
            this.buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFeedback.Image = global::DiversityCollection.Resource.Feedback;
            this.buttonFeedback.Location = new System.Drawing.Point(855, 0);
            this.buttonFeedback.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFeedback.Name = "buttonFeedback";
            this.buttonFeedback.Size = new System.Drawing.Size(21, 16);
            this.buttonFeedback.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonFeedback, "Send a feedback to the software developer");
            this.buttonFeedback.UseVisualStyleBackColor = true;
            this.buttonFeedback.Click += new System.EventHandler(this.buttonFeedback_Click);
            // 
            // dataSetTransaction
            // 
            this.dataSetTransaction.DataSetName = "DataSetTransaction";
            this.dataSetTransaction.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // buttonCollectionDetails
            // 
            this.buttonCollectionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCollectionDetails.FlatAppearance.BorderSize = 0;
            this.buttonCollectionDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCollectionDetails.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonCollectionDetails.Location = new System.Drawing.Point(855, 432);
            this.buttonCollectionDetails.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCollectionDetails.Name = "buttonCollectionDetails";
            this.buttonCollectionDetails.Size = new System.Drawing.Size(25, 25);
            this.buttonCollectionDetails.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonCollectionDetails, "Show details for collection");
            this.buttonCollectionDetails.UseVisualStyleBackColor = true;
            this.buttonCollectionDetails.Click += new System.EventHandler(this.buttonCollectionDetails_Click);
            // 
            // labelCollectionDetails
            // 
            this.labelCollectionDetails.AutoSize = true;
            this.labelCollectionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionDetails.Location = new System.Drawing.Point(532, 432);
            this.labelCollectionDetails.Name = "labelCollectionDetails";
            this.labelCollectionDetails.Size = new System.Drawing.Size(320, 25);
            this.labelCollectionDetails.TabIndex = 9;
            // 
            // FormManagers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.tableLayoutPanelManagers);
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpString(this, "Collection manager");
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormManagers";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.helpProvider.SetShowHelp(this, true);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administation of collection managers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormManagers_FormClosing);
            this.tableLayoutPanelManagers.ResumeLayout(false);
            this.tableLayoutPanelManagers.PerformLayout();
            this.toolStripManagers.ResumeLayout(false);
            this.toolStripManagers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelManagers;
        private System.Windows.Forms.ListBox listBoxUser;
        private System.Windows.Forms.ListBox listBoxAdministratingCollection;
        private System.Windows.Forms.Button buttonCollectionAdd;
        private System.Windows.Forms.ListBox listBoxCollectionManager;
        private System.Windows.Forms.Button buttonCollectionRemove;
        private Datasets.DataSetTransaction dataSetTransaction;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelCollectionManager;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelCollections;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.ToolStrip toolStripManagers;
        private System.Windows.Forms.ToolStripLabel toolStripLabelManagerListBy;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxManagerListBy;
        private System.Windows.Forms.ToolStripButton toolStripButtonSynchronizeLogins;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.Button buttonCollectionDetails;
        private System.Windows.Forms.Label labelCollectionDetails;
    }
}