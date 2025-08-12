namespace DiversityCollection.CacheDatabase
{
    partial class UserControlLookupSourceMissing
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
            this.buttonRemoveSource = new System.Windows.Forms.Button();
            this.labelMissingSource = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.Controls.Add(this.buttonRemoveSource, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.labelMissingSource, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelCount, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(799, 38);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // buttonRemoveSource
            // 
            this.buttonRemoveSource.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonRemoveSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemoveSource.Enabled = false;
            this.buttonRemoveSource.FlatAppearance.BorderSize = 0;
            this.buttonRemoveSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemoveSource.Image = global::DiversityCollection.Resource.Delete;
            this.buttonRemoveSource.Location = new System.Drawing.Point(774, 3);
            this.buttonRemoveSource.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRemoveSource.Name = "buttonRemoveSource";
            this.buttonRemoveSource.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonRemoveSource.Size = new System.Drawing.Size(25, 33);
            this.buttonRemoveSource.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonRemoveSource, "Remove all datasets related to this source view");
            this.buttonRemoveSource.UseVisualStyleBackColor = false;
            this.buttonRemoveSource.Click += new System.EventHandler(this.buttonRemoveSource_Click);
            // 
            // labelMissingSource
            // 
            this.labelMissingSource.BackColor = System.Drawing.Color.LightSteelBlue;
            this.labelMissingSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMissingSource.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelMissingSource.Location = new System.Drawing.Point(580, 3);
            this.labelMissingSource.Margin = new System.Windows.Forms.Padding(0);
            this.labelMissingSource.Name = "labelMissingSource";
            this.labelMissingSource.Size = new System.Drawing.Size(144, 33);
            this.labelMissingSource.TabIndex = 6;
            this.labelMissingSource.Text = "DiversityCollectionCache";
            this.labelMissingSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCount
            // 
            this.labelCount.BackColor = System.Drawing.Color.LightSteelBlue;
            this.labelCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCount.Location = new System.Drawing.Point(724, 3);
            this.labelCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(50, 33);
            this.labelCount.TabIndex = 8;
            this.labelCount.Text = "0";
            this.labelCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(5, 3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(572, 33);
            this.labelHeader.TabIndex = 9;
            this.labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UserControlLookupSourceMissing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlLookupSourceMissing";
            this.Size = new System.Drawing.Size(799, 38);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonRemoveSource;
        private System.Windows.Forms.Label labelMissingSource;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
