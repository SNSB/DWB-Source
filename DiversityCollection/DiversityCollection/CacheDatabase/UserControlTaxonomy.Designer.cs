namespace DiversityCollection.CacheDatabase
{
    partial class UserControlTaxonomy
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxDatabase = new System.Windows.Forms.TextBox();
            this.labelCountCacheDB = new System.Windows.Forms.Label();
            this.buttonCreateView = new System.Windows.Forms.Button();
            this.textBoxView = new System.Windows.Forms.TextBox();
            this.buttonTestView = new System.Windows.Forms.Button();
            this.buttonTransferToCache = new System.Windows.Forms.Button();
            this.buttonViewCacheDB = new System.Windows.Forms.Button();
            this.buttonTransferToPostgres = new System.Windows.Forms.Button();
            this.buttonViewPostgres = new System.Windows.Forms.Button();
            this.labelCountPostgres = new System.Windows.Forms.Label();
            this.buttonDeleteSource = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 13;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.textBoxDatabase, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCountCacheDB, 7, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonCreateView, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxView, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonTestView, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferToCache, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonViewCacheDB, 8, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonTransferToPostgres, 10, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonViewPostgres, 12, 0);
            this.tableLayoutPanel.Controls.Add(this.labelCountPostgres, 11, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonDeleteSource, 4, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpProvider.SetHelpNavigator(this.tableLayoutPanel, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.helpProvider.SetShowHelp(this.tableLayoutPanel, true);
            this.tableLayoutPanel.Size = new System.Drawing.Size(850, 52);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // textBoxDatabase
            // 
            this.textBoxDatabase.BackColor = System.Drawing.Color.Khaki;
            this.textBoxDatabase.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDatabase.Location = new System.Drawing.Point(0, 0);
            this.textBoxDatabase.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxDatabase.Multiline = true;
            this.textBoxDatabase.Name = "textBoxDatabase";
            this.textBoxDatabase.ReadOnly = true;
            this.tableLayoutPanel.SetRowSpan(this.textBoxDatabase, 2);
            this.textBoxDatabase.Size = new System.Drawing.Size(232, 46);
            this.textBoxDatabase.TabIndex = 0;
            // 
            // labelCountCacheDB
            // 
            this.labelCountCacheDB.AutoSize = true;
            this.labelCountCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.labelCountCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountCacheDB.Location = new System.Drawing.Point(422, 0);
            this.labelCountCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.labelCountCacheDB.Name = "labelCountCacheDB";
            this.tableLayoutPanel.SetRowSpan(this.labelCountCacheDB, 2);
            this.labelCountCacheDB.Size = new System.Drawing.Size(174, 46);
            this.labelCountCacheDB.TabIndex = 3;
            this.labelCountCacheDB.Text = "label1";
            this.labelCountCacheDB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCreateView
            // 
            this.buttonCreateView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateView.Location = new System.Drawing.Point(238, 22);
            this.buttonCreateView.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCreateView.Name = "buttonCreateView";
            this.buttonCreateView.Size = new System.Drawing.Size(111, 24);
            this.buttonCreateView.TabIndex = 1;
            this.buttonCreateView.Text = "Create view for data";
            this.buttonCreateView.UseVisualStyleBackColor = true;
            this.buttonCreateView.Click += new System.EventHandler(this.buttonCreateView_Click);
            // 
            // textBoxView
            // 
            this.textBoxView.BackColor = System.Drawing.Color.Khaki;
            this.textBoxView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel.SetColumnSpan(this.textBoxView, 3);
            this.textBoxView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxView.Location = new System.Drawing.Point(238, 0);
            this.textBoxView.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.textBoxView.Multiline = true;
            this.textBoxView.Name = "textBoxView";
            this.textBoxView.ReadOnly = true;
            this.textBoxView.Size = new System.Drawing.Size(158, 19);
            this.textBoxView.TabIndex = 2;
            this.textBoxView.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonTestView
            // 
            this.buttonTestView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTestView.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonTestView.Location = new System.Drawing.Point(349, 22);
            this.buttonTestView.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTestView.Name = "buttonTestView";
            this.buttonTestView.Size = new System.Drawing.Size(24, 24);
            this.buttonTestView.TabIndex = 4;
            this.buttonTestView.UseVisualStyleBackColor = true;
            this.buttonTestView.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonTransferToCache
            // 
            this.buttonTransferToCache.BackColor = System.Drawing.Color.Thistle;
            this.buttonTransferToCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferToCache.FlatAppearance.BorderSize = 0;
            this.buttonTransferToCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferToCache.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTransferToCache.Location = new System.Drawing.Point(402, 0);
            this.buttonTransferToCache.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferToCache.Name = "buttonTransferToCache";
            this.tableLayoutPanel.SetRowSpan(this.buttonTransferToCache, 2);
            this.buttonTransferToCache.Size = new System.Drawing.Size(20, 46);
            this.buttonTransferToCache.TabIndex = 6;
            this.buttonTransferToCache.UseVisualStyleBackColor = false;
            this.buttonTransferToCache.Click += new System.EventHandler(this.buttonTransferToCache_Click);
            // 
            // buttonViewCacheDB
            // 
            this.buttonViewCacheDB.BackColor = System.Drawing.Color.Thistle;
            this.buttonViewCacheDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewCacheDB.FlatAppearance.BorderSize = 0;
            this.buttonViewCacheDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewCacheDB.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewCacheDB.Location = new System.Drawing.Point(596, 0);
            this.buttonViewCacheDB.Margin = new System.Windows.Forms.Padding(0);
            this.buttonViewCacheDB.Name = "buttonViewCacheDB";
            this.tableLayoutPanel.SetRowSpan(this.buttonViewCacheDB, 2);
            this.buttonViewCacheDB.Size = new System.Drawing.Size(24, 46);
            this.buttonViewCacheDB.TabIndex = 7;
            this.buttonViewCacheDB.UseVisualStyleBackColor = false;
            this.buttonViewCacheDB.Click += new System.EventHandler(this.buttonViewCacheDB_Click);
            // 
            // buttonTransferToPostgres
            // 
            this.buttonTransferToPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonTransferToPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTransferToPostgres.FlatAppearance.BorderSize = 0;
            this.buttonTransferToPostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransferToPostgres.Image = global::DiversityCollection.Resource.ArrowNext1;
            this.buttonTransferToPostgres.Location = new System.Drawing.Point(626, 0);
            this.buttonTransferToPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransferToPostgres.Name = "buttonTransferToPostgres";
            this.tableLayoutPanel.SetRowSpan(this.buttonTransferToPostgres, 2);
            this.buttonTransferToPostgres.Size = new System.Drawing.Size(20, 46);
            this.buttonTransferToPostgres.TabIndex = 8;
            this.buttonTransferToPostgres.UseVisualStyleBackColor = false;
            this.buttonTransferToPostgres.Click += new System.EventHandler(this.buttonTransferToPostgres_Click);
            // 
            // buttonViewPostgres
            // 
            this.buttonViewPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonViewPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonViewPostgres.FlatAppearance.BorderSize = 0;
            this.buttonViewPostgres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonViewPostgres.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonViewPostgres.Location = new System.Drawing.Point(820, 0);
            this.buttonViewPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.buttonViewPostgres.Name = "buttonViewPostgres";
            this.tableLayoutPanel.SetRowSpan(this.buttonViewPostgres, 2);
            this.buttonViewPostgres.Size = new System.Drawing.Size(30, 46);
            this.buttonViewPostgres.TabIndex = 9;
            this.buttonViewPostgres.UseVisualStyleBackColor = false;
            this.buttonViewPostgres.Click += new System.EventHandler(this.buttonViewPostgres_Click);
            // 
            // labelCountPostgres
            // 
            this.labelCountPostgres.AutoSize = true;
            this.labelCountPostgres.BackColor = System.Drawing.Color.LightSteelBlue;
            this.labelCountPostgres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCountPostgres.Location = new System.Drawing.Point(646, 0);
            this.labelCountPostgres.Margin = new System.Windows.Forms.Padding(0);
            this.labelCountPostgres.Name = "labelCountPostgres";
            this.tableLayoutPanel.SetRowSpan(this.labelCountPostgres, 2);
            this.labelCountPostgres.Size = new System.Drawing.Size(174, 46);
            this.labelCountPostgres.TabIndex = 10;
            this.labelCountPostgres.Text = "label1";
            this.labelCountPostgres.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonDeleteSource
            // 
            this.buttonDeleteSource.Image = global::DiversityCollection.Resource.Delete;
            this.buttonDeleteSource.Location = new System.Drawing.Point(373, 22);
            this.buttonDeleteSource.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteSource.Name = "buttonDeleteSource";
            this.buttonDeleteSource.Size = new System.Drawing.Size(23, 23);
            this.buttonDeleteSource.TabIndex = 5;
            this.buttonDeleteSource.UseVisualStyleBackColor = true;
            this.buttonDeleteSource.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // UserControlTaxonomy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTaxonomy";
            this.Size = new System.Drawing.Size(850, 52);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textBoxDatabase;
        private System.Windows.Forms.Label labelCountCacheDB;
        private System.Windows.Forms.Button buttonCreateView;
        private System.Windows.Forms.TextBox textBoxView;
        private System.Windows.Forms.Button buttonTestView;
        private System.Windows.Forms.Button buttonDeleteSource;
        private System.Windows.Forms.Button buttonTransferToCache;
        private System.Windows.Forms.Button buttonViewCacheDB;
        private System.Windows.Forms.Button buttonTransferToPostgres;
        private System.Windows.Forms.Button buttonViewPostgres;
        private System.Windows.Forms.Label labelCountPostgres;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}
