namespace DiversityCollection.CacheDatabase
{
    partial class UserControlTable
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataGridViewSource = new System.Windows.Forms.DataGridView();
            this.labelSource = new System.Windows.Forms.Label();
            this.dataGridViewCache = new System.Windows.Forms.DataGridView();
            this.labelCache = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCache)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dataGridViewSource);
            this.splitContainer.Panel1.Controls.Add(this.labelSource);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataGridViewCache);
            this.splitContainer.Panel2.Controls.Add(this.labelCache);
            this.splitContainer.Size = new System.Drawing.Size(416, 313);
            this.splitContainer.SplitterDistance = 207;
            this.splitContainer.TabIndex = 0;
            // 
            // dataGridViewSource
            // 
            this.dataGridViewSource.AllowUserToAddRows = false;
            this.dataGridViewSource.AllowUserToDeleteRows = false;
            this.dataGridViewSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSource.Location = new System.Drawing.Point(0, 13);
            this.dataGridViewSource.Name = "dataGridViewSource";
            this.dataGridViewSource.ReadOnly = true;
            this.dataGridViewSource.Size = new System.Drawing.Size(207, 300);
            this.dataGridViewSource.TabIndex = 1;
            // 
            // labelSource
            // 
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSource.Location = new System.Drawing.Point(0, 0);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(207, 13);
            this.labelSource.TabIndex = 0;
            this.labelSource.Text = "Source";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridViewCache
            // 
            this.dataGridViewCache.AllowUserToAddRows = false;
            this.dataGridViewCache.AllowUserToDeleteRows = false;
            this.dataGridViewCache.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCache.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCache.Location = new System.Drawing.Point(0, 13);
            this.dataGridViewCache.Name = "dataGridViewCache";
            this.dataGridViewCache.ReadOnly = true;
            this.dataGridViewCache.Size = new System.Drawing.Size(205, 300);
            this.dataGridViewCache.TabIndex = 2;
            // 
            // labelCache
            // 
            this.labelCache.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCache.Location = new System.Drawing.Point(0, 0);
            this.labelCache.Name = "labelCache";
            this.labelCache.Size = new System.Drawing.Size(205, 13);
            this.labelCache.TabIndex = 1;
            this.labelCache.Text = "Cache content";
            this.labelCache.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 313);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(416, 10);
            this.progressBar.TabIndex = 1;
            // 
            // UserControlTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.progressBar);
            this.Name = "UserControlTable";
            this.Size = new System.Drawing.Size(416, 323);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCache)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dataGridViewSource;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.DataGridView dataGridViewCache;
        private System.Windows.Forms.Label labelCache;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}
