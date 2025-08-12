namespace DiversityCollection.UserControls
{
    partial class UserControlDatawithholdingSummary
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
            this.tableLayoutPanelSummarySpecimen = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxSummary = new System.Windows.Forms.PictureBox();
            this.labelSummaryToPublish = new System.Windows.Forms.Label();
            this.pictureBoxSummaryBlocked = new System.Windows.Forms.PictureBox();
            this.labelSummaryBlocked = new System.Windows.Forms.Label();
            this.tableLayoutPanelSummarySpecimen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSummaryBlocked)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelSummarySpecimen
            // 
            this.tableLayoutPanelSummarySpecimen.ColumnCount = 4;
            this.tableLayoutPanelSummarySpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSummarySpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSummarySpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSummarySpecimen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSummarySpecimen.Controls.Add(this.pictureBoxSummary, 0, 0);
            this.tableLayoutPanelSummarySpecimen.Controls.Add(this.labelSummaryToPublish, 1, 0);
            this.tableLayoutPanelSummarySpecimen.Controls.Add(this.pictureBoxSummaryBlocked, 2, 0);
            this.tableLayoutPanelSummarySpecimen.Controls.Add(this.labelSummaryBlocked, 3, 0);
            this.tableLayoutPanelSummarySpecimen.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSummarySpecimen.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSummarySpecimen.Name = "tableLayoutPanelSummarySpecimen";
            this.tableLayoutPanelSummarySpecimen.RowCount = 1;
            this.tableLayoutPanelSummarySpecimen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSummarySpecimen.Size = new System.Drawing.Size(585, 22);
            this.tableLayoutPanelSummarySpecimen.TabIndex = 2;
            // 
            // pictureBoxSummary
            // 
            this.pictureBoxSummary.Image = global::DiversityCollection.Resource.CollectionSpecimen;
            this.pictureBoxSummary.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxSummary.Name = "pictureBoxSummary";
            this.pictureBoxSummary.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSummary.TabIndex = 0;
            this.pictureBoxSummary.TabStop = false;
            // 
            // labelSummaryToPublish
            // 
            this.labelSummaryToPublish.AutoSize = true;
            this.labelSummaryToPublish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSummaryToPublish.ForeColor = System.Drawing.Color.Green;
            this.labelSummaryToPublish.Location = new System.Drawing.Point(25, 0);
            this.labelSummaryToPublish.Name = "labelSummaryToPublish";
            this.labelSummaryToPublish.Size = new System.Drawing.Size(264, 22);
            this.labelSummaryToPublish.TabIndex = 1;
            this.labelSummaryToPublish.Text = "label1";
            this.labelSummaryToPublish.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxSummaryBlocked
            // 
            this.pictureBoxSummaryBlocked.Image = global::DiversityCollection.Resource.Stop3;
            this.pictureBoxSummaryBlocked.Location = new System.Drawing.Point(295, 3);
            this.pictureBoxSummaryBlocked.Name = "pictureBoxSummaryBlocked";
            this.pictureBoxSummaryBlocked.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxSummaryBlocked.TabIndex = 2;
            this.pictureBoxSummaryBlocked.TabStop = false;
            // 
            // labelSummaryBlocked
            // 
            this.labelSummaryBlocked.AutoSize = true;
            this.labelSummaryBlocked.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSummaryBlocked.ForeColor = System.Drawing.Color.Red;
            this.labelSummaryBlocked.Location = new System.Drawing.Point(317, 0);
            this.labelSummaryBlocked.Name = "labelSummaryBlocked";
            this.labelSummaryBlocked.Size = new System.Drawing.Size(265, 22);
            this.labelSummaryBlocked.TabIndex = 3;
            this.labelSummaryBlocked.Text = "label1";
            this.labelSummaryBlocked.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UserControlDatawithholdingSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelSummarySpecimen);
            this.Name = "UserControlDatawithholdingSummary";
            this.Size = new System.Drawing.Size(585, 22);
            this.tableLayoutPanelSummarySpecimen.ResumeLayout(false);
            this.tableLayoutPanelSummarySpecimen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSummaryBlocked)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSummarySpecimen;
        private System.Windows.Forms.PictureBox pictureBoxSummary;
        private System.Windows.Forms.Label labelSummaryToPublish;
        private System.Windows.Forms.PictureBox pictureBoxSummaryBlocked;
        private System.Windows.Forms.Label labelSummaryBlocked;
    }
}
