namespace DiversityWorkbench.Spreadsheet
{
    partial class UserControlMapLegend
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
            this.buttonSymbolOrColor = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.buttonSymbolOrColor, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelDescription, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(274, 16);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttonSymbolOrColor
            // 
            this.buttonSymbolOrColor.BackColor = System.Drawing.Color.Red;
            this.buttonSymbolOrColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonSymbolOrColor.FlatAppearance.BorderSize = 0;
            this.buttonSymbolOrColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSymbolOrColor.Location = new System.Drawing.Point(0, 0);
            this.buttonSymbolOrColor.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSymbolOrColor.Name = "buttonSymbolOrColor";
            this.buttonSymbolOrColor.Size = new System.Drawing.Size(16, 16);
            this.buttonSymbolOrColor.TabIndex = 0;
            this.buttonSymbolOrColor.UseVisualStyleBackColor = false;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(16, 0);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(258, 16);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UserControlMapLegend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlMapLegend";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(280, 22);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonSymbolOrColor;
        private System.Windows.Forms.Label labelDescription;
    }
}
