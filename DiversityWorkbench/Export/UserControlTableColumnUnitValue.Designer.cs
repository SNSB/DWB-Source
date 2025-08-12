namespace DiversityWorkbench.Export
{
    partial class UserControlTableColumnUnitValue
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
            this.labelUnitValue = new System.Windows.Forms.Label();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelSource = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelUnitValue
            // 
            this.labelUnitValue.AutoSize = true;
            this.labelUnitValue.Location = new System.Drawing.Point(26, 5);
            this.labelUnitValue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.labelUnitValue.Name = "labelUnitValue";
            this.labelUnitValue.Size = new System.Drawing.Size(35, 13);
            this.labelUnitValue.TabIndex = 0;
            this.labelUnitValue.Text = "label1";
            this.labelUnitValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.buttonAdd.Location = new System.Drawing.Point(0, 0);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(23, 23);
            this.buttonAdd.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonAdd, "Add this column to the export");
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // labelSource
            // 
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelSource.Location = new System.Drawing.Point(67, 0);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(306, 23);
            this.labelSource.TabIndex = 2;
            this.labelSource.Text = "label1";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelSource, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonAdd, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelUnitValue, 1, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(376, 23);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // UserControlTableColumnUnitValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTableColumnUnitValue";
            this.Size = new System.Drawing.Size(376, 23);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelUnitValue;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
