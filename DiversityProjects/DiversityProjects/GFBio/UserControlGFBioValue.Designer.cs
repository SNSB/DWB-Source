namespace DiversityProjects.GFBio
{
    partial class UserControlGFBioValue
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
            this.labelTable = new System.Windows.Forms.Label();
            this.labelColumn = new System.Windows.Forms.Label();
            this.labelSetting = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.textBoxTable = new System.Windows.Forms.TextBox();
            this.textBoxColumn = new System.Windows.Forms.TextBox();
            this.textBoxSetting = new System.Windows.Forms.TextBox();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelTable, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelColumn, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelSetting, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelValue, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxTable, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxColumn, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxSetting, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.textBoxValue, 3, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(571, 23);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.Location = new System.Drawing.Point(3, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(34, 13);
            this.labelTable.TabIndex = 0;
            this.labelTable.Text = "Table";
            this.labelTable.Visible = false;
            // 
            // labelColumn
            // 
            this.labelColumn.AutoSize = true;
            this.labelColumn.Location = new System.Drawing.Point(123, 0);
            this.labelColumn.Name = "labelColumn";
            this.labelColumn.Size = new System.Drawing.Size(42, 13);
            this.labelColumn.TabIndex = 1;
            this.labelColumn.Text = "Column";
            this.labelColumn.Visible = false;
            // 
            // labelSetting
            // 
            this.labelSetting.AutoSize = true;
            this.labelSetting.Location = new System.Drawing.Point(243, 0);
            this.labelSetting.Name = "labelSetting";
            this.labelSetting.Size = new System.Drawing.Size(91, 13);
            this.labelSetting.TabIndex = 2;
            this.labelSetting.Text = "Setting / Identifier";
            this.labelSetting.Visible = false;
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(363, 0);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(34, 13);
            this.labelValue.TabIndex = 3;
            this.labelValue.Text = "Value";
            this.labelValue.Visible = false;
            // 
            // textBoxTable
            // 
            this.textBoxTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTable.Location = new System.Drawing.Point(3, 16);
            this.textBoxTable.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxTable.Name = "textBoxTable";
            this.textBoxTable.ReadOnly = true;
            this.textBoxTable.Size = new System.Drawing.Size(117, 20);
            this.textBoxTable.TabIndex = 4;
            // 
            // textBoxColumn
            // 
            this.textBoxColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxColumn.Location = new System.Drawing.Point(120, 16);
            this.textBoxColumn.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxColumn.Name = "textBoxColumn";
            this.textBoxColumn.ReadOnly = true;
            this.textBoxColumn.Size = new System.Drawing.Size(120, 20);
            this.textBoxColumn.TabIndex = 5;
            // 
            // textBoxSetting
            // 
            this.textBoxSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSetting.Location = new System.Drawing.Point(240, 16);
            this.textBoxSetting.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxSetting.Name = "textBoxSetting";
            this.textBoxSetting.ReadOnly = true;
            this.textBoxSetting.Size = new System.Drawing.Size(120, 20);
            this.textBoxSetting.TabIndex = 6;
            // 
            // textBoxValue
            // 
            this.textBoxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxValue.Location = new System.Drawing.Point(360, 16);
            this.textBoxValue.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.ReadOnly = true;
            this.textBoxValue.Size = new System.Drawing.Size(208, 20);
            this.textBoxValue.TabIndex = 7;
            // 
            // UserControlGFBioValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlGFBioValue";
            this.Size = new System.Drawing.Size(571, 23);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Label labelColumn;
        private System.Windows.Forms.Label labelSetting;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.TextBox textBoxTable;
        private System.Windows.Forms.TextBox textBoxColumn;
        private System.Windows.Forms.TextBox textBoxSetting;
        private System.Windows.Forms.TextBox textBoxValue;
    }
}
