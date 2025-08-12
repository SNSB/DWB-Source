namespace DiversityCollection.CacheDatabase
{
    partial class UserControlTaxonSource
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxDatabase = new System.Windows.Forms.TextBox();
            this.labelViewCount = new System.Windows.Forms.Label();
            this.buttonCreateView = new System.Windows.Forms.Button();
            this.textBoxView = new System.Windows.Forms.TextBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.textBoxDatabase, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelViewCount, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonCreateView, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxView, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonTest, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonDelete, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(900, 30);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // textBoxDatabase
            // 
            this.textBoxDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDatabase.Location = new System.Drawing.Point(3, 5);
            this.textBoxDatabase.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.textBoxDatabase.Name = "textBoxDatabase";
            this.textBoxDatabase.ReadOnly = true;
            this.textBoxDatabase.Size = new System.Drawing.Size(250, 20);
            this.textBoxDatabase.TabIndex = 0;
            // 
            // labelViewCount
            // 
            this.labelViewCount.AutoSize = true;
            this.labelViewCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelViewCount.Location = new System.Drawing.Point(661, 0);
            this.labelViewCount.Name = "labelViewCount";
            this.labelViewCount.Size = new System.Drawing.Size(236, 30);
            this.labelViewCount.TabIndex = 3;
            this.labelViewCount.Text = "label1";
            this.labelViewCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonCreateView
            // 
            this.buttonCreateView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCreateView.Location = new System.Drawing.Point(485, 3);
            this.buttonCreateView.Name = "buttonCreateView";
            this.buttonCreateView.Size = new System.Drawing.Size(111, 24);
            this.buttonCreateView.TabIndex = 1;
            this.buttonCreateView.Text = "Create view for data";
            this.buttonCreateView.UseVisualStyleBackColor = true;
            this.buttonCreateView.Click += new System.EventHandler(this.buttonCreateView_Click);
            // 
            // textBoxView
            // 
            this.textBoxView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxView.Location = new System.Drawing.Point(259, 5);
            this.textBoxView.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.textBoxView.Name = "textBoxView";
            this.textBoxView.ReadOnly = true;
            this.textBoxView.Size = new System.Drawing.Size(220, 20);
            this.textBoxView.TabIndex = 2;
            // 
            // buttonTest
            // 
            this.buttonTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTest.Image = global::DiversityCollection.Resource.Lupe;
            this.buttonTest.Location = new System.Drawing.Point(602, 3);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(24, 24);
            this.buttonTest.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonTest, "Inspect the data as provided by the source");
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(632, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(23, 23);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // UserControlTaxonSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTaxonSource";
            this.Size = new System.Drawing.Size(900, 30);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textBoxDatabase;
        private System.Windows.Forms.Label labelViewCount;
        private System.Windows.Forms.Button buttonCreateView;
        private System.Windows.Forms.TextBox textBoxView;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonDelete;
    }
}
