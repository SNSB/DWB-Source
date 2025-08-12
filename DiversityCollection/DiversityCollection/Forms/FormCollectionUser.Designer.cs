namespace DiversityCollection.Forms
{
    partial class FormCollectionUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCollectionUser));
            tableLayoutPanelUser = new System.Windows.Forms.TableLayoutPanel();
            listBoxUser = new System.Windows.Forms.ListBox();
            listBoxCollection = new System.Windows.Forms.ListBox();
            buttonCollectionAdd = new System.Windows.Forms.Button();
            listBoxCollectionUser = new System.Windows.Forms.ListBox();
            buttonCollectionRemove = new System.Windows.Forms.Button();
            labelUser = new System.Windows.Forms.Label();
            labelCollectionManager = new System.Windows.Forms.Label();
            labelCollections = new System.Windows.Forms.Label();
            labelForRestrictionOnly = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanelUser.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelUser
            // 
            tableLayoutPanelUser.ColumnCount = 5;
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelUser.Controls.Add(listBoxUser, 0, 2);
            tableLayoutPanelUser.Controls.Add(listBoxCollection, 4, 2);
            tableLayoutPanelUser.Controls.Add(buttonCollectionAdd, 3, 2);
            tableLayoutPanelUser.Controls.Add(listBoxCollectionUser, 2, 2);
            tableLayoutPanelUser.Controls.Add(buttonCollectionRemove, 3, 3);
            tableLayoutPanelUser.Controls.Add(labelUser, 0, 1);
            tableLayoutPanelUser.Controls.Add(labelCollectionManager, 2, 1);
            tableLayoutPanelUser.Controls.Add(labelCollections, 4, 1);
            tableLayoutPanelUser.Controls.Add(labelForRestrictionOnly, 0, 0);
            tableLayoutPanelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpNavigator(tableLayoutPanelUser, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProvider.SetHelpString(tableLayoutPanelUser, "Collection user");
            tableLayoutPanelUser.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelUser.Name = "tableLayoutPanelUser";
            tableLayoutPanelUser.RowCount = 4;
            tableLayoutPanelUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanelUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            helpProvider.SetShowHelp(tableLayoutPanelUser, true);
            tableLayoutPanelUser.Size = new System.Drawing.Size(674, 397);
            tableLayoutPanelUser.TabIndex = 5;
            // 
            // listBoxUser
            // 
            listBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxUser.FormattingEnabled = true;
            listBoxUser.IntegralHeight = false;
            listBoxUser.ItemHeight = 15;
            listBoxUser.Location = new System.Drawing.Point(4, 92);
            listBoxUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxUser.Name = "listBoxUser";
            tableLayoutPanelUser.SetRowSpan(listBoxUser, 2);
            listBoxUser.Size = new System.Drawing.Size(117, 302);
            listBoxUser.TabIndex = 0;
            listBoxUser.SelectedIndexChanged += listBoxUser_SelectedIndexChanged;
            // 
            // listBoxCollection
            // 
            listBoxCollection.BackColor = System.Drawing.Color.LightPink;
            listBoxCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxCollection.FormattingEnabled = true;
            listBoxCollection.IntegralHeight = false;
            listBoxCollection.ItemHeight = 15;
            listBoxCollection.Location = new System.Drawing.Point(426, 92);
            listBoxCollection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxCollection.Name = "listBoxCollection";
            tableLayoutPanelUser.SetRowSpan(listBoxCollection, 2);
            listBoxCollection.Size = new System.Drawing.Size(244, 302);
            listBoxCollection.TabIndex = 2;
            // 
            // buttonCollectionAdd
            // 
            buttonCollectionAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonCollectionAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonCollectionAdd.ForeColor = System.Drawing.Color.Green;
            buttonCollectionAdd.Location = new System.Drawing.Point(399, 213);
            buttonCollectionAdd.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonCollectionAdd.Name = "buttonCollectionAdd";
            buttonCollectionAdd.Size = new System.Drawing.Size(23, 27);
            buttonCollectionAdd.TabIndex = 1;
            buttonCollectionAdd.Text = "<";
            toolTip.SetToolTip(buttonCollectionAdd, "Allow user to access the selected collection");
            buttonCollectionAdd.UseVisualStyleBackColor = true;
            buttonCollectionAdd.Click += buttonCollectionAdd_Click;
            // 
            // listBoxCollectionUser
            // 
            listBoxCollectionUser.BackColor = System.Drawing.Color.LightGreen;
            listBoxCollectionUser.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxCollectionUser.FormattingEnabled = true;
            listBoxCollectionUser.IntegralHeight = false;
            listBoxCollectionUser.ItemHeight = 15;
            listBoxCollectionUser.Location = new System.Drawing.Point(152, 92);
            listBoxCollectionUser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBoxCollectionUser.Name = "listBoxCollectionUser";
            tableLayoutPanelUser.SetRowSpan(listBoxCollectionUser, 2);
            listBoxCollectionUser.Size = new System.Drawing.Size(243, 302);
            listBoxCollectionUser.TabIndex = 1;
            // 
            // buttonCollectionRemove
            // 
            buttonCollectionRemove.Dock = System.Windows.Forms.DockStyle.Top;
            buttonCollectionRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            buttonCollectionRemove.ForeColor = System.Drawing.Color.Red;
            buttonCollectionRemove.Location = new System.Drawing.Point(399, 246);
            buttonCollectionRemove.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            buttonCollectionRemove.Name = "buttonCollectionRemove";
            buttonCollectionRemove.Size = new System.Drawing.Size(23, 27);
            buttonCollectionRemove.TabIndex = 2;
            buttonCollectionRemove.Text = ">";
            toolTip.SetToolTip(buttonCollectionRemove, "Remove the selected collection from the accessible collections list");
            buttonCollectionRemove.UseVisualStyleBackColor = true;
            buttonCollectionRemove.Click += buttonCollectionRemove_Click;
            // 
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Dock = System.Windows.Forms.DockStyle.Fill;
            labelUser.Location = new System.Drawing.Point(4, 30);
            labelUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelUser.Name = "labelUser";
            labelUser.Size = new System.Drawing.Size(117, 59);
            labelUser.TabIndex = 3;
            labelUser.Text = "User";
            labelUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCollectionManager
            // 
            labelCollectionManager.AutoSize = true;
            labelCollectionManager.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCollectionManager.Location = new System.Drawing.Point(152, 37);
            labelCollectionManager.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            labelCollectionManager.Name = "labelCollectionManager";
            labelCollectionManager.Size = new System.Drawing.Size(243, 45);
            labelCollectionManager.TabIndex = 4;
            labelCollectionManager.Text = "For users with restricted access: Collections accessible by a user (including all  subordinate collections)";
            labelCollectionManager.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCollections
            // 
            labelCollections.AutoSize = true;
            labelCollections.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCollections.Location = new System.Drawing.Point(426, 30);
            labelCollections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCollections.Name = "labelCollections";
            labelCollections.Size = new System.Drawing.Size(244, 59);
            labelCollections.TabIndex = 5;
            labelCollections.Text = "Available collections";
            labelCollections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelForRestrictionOnly
            // 
            labelForRestrictionOnly.AutoSize = true;
            tableLayoutPanelUser.SetColumnSpan(labelForRestrictionOnly, 5);
            labelForRestrictionOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            labelForRestrictionOnly.ForeColor = System.Drawing.Color.Red;
            labelForRestrictionOnly.Location = new System.Drawing.Point(4, 0);
            labelForRestrictionOnly.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelForRestrictionOnly.Name = "labelForRestrictionOnly";
            labelForRestrictionOnly.Size = new System.Drawing.Size(666, 30);
            labelForRestrictionOnly.TabIndex = 6;
            labelForRestrictionOnly.Text = "If the access of users should be restricted to certain collections, use the lists below. If the access should not be restricted remove any entry from the list of accessible collections for this user";
            labelForRestrictionOnly.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormCollectionUser
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(674, 397);
            Controls.Add(tableLayoutPanelUser);
            helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            helpProvider.SetHelpString(this, "Collection user");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormCollectionUser";
            helpProvider.SetShowHelp(this, true);
            Text = "Restriction of the access for user to collections";
            FormClosing += FormCollectionUser_FormClosing;
            tableLayoutPanelUser.ResumeLayout(false);
            tableLayoutPanelUser.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUser;
        private System.Windows.Forms.ListBox listBoxUser;
        private System.Windows.Forms.ListBox listBoxCollection;
        private System.Windows.Forms.Button buttonCollectionAdd;
        private System.Windows.Forms.ListBox listBoxCollectionUser;
        private System.Windows.Forms.Button buttonCollectionRemove;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelCollectionManager;
        private System.Windows.Forms.Label labelCollections;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label labelForRestrictionOnly;
    }
}