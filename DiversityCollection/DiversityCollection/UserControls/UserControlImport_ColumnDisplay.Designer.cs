namespace DiversityCollection.UserControls
{
    partial class UserControlImport_ColumnDisplay
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
            this.labelMapTable = new System.Windows.Forms.Label();
            this.textBoxMapTable = new System.Windows.Forms.TextBox();
            this.labelMapColumn = new System.Windows.Forms.Label();
            this.textBoxMapColumn = new System.Windows.Forms.TextBox();
            this.labelSequence = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.textBoxSequence = new System.Windows.Forms.TextBox();
            this.labelPreset = new System.Windows.Forms.Label();
            this.labelEntity = new System.Windows.Forms.Label();
            this.textBoxEntity = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 11;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelMapTable, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxMapTable, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelMapColumn, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxMapColumn, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.labelSequence, 8, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonDelete, 10, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxSequence, 9, 0);
            this.tableLayoutPanel.Controls.Add(this.labelPreset, 7, 0);
            this.tableLayoutPanel.Controls.Add(this.labelEntity, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxEntity, 6, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(630, 21);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // labelMapTable
            // 
            this.labelMapTable.AutoSize = true;
            this.labelMapTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMapTable.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelMapTable.Location = new System.Drawing.Point(3, 0);
            this.labelMapTable.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelMapTable.Name = "labelMapTable";
            this.labelMapTable.Size = new System.Drawing.Size(37, 21);
            this.labelMapTable.TabIndex = 4;
            this.labelMapTable.Text = "Table:";
            this.labelMapTable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMapTable
            // 
            this.textBoxMapTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMapTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMapTable.Location = new System.Drawing.Point(40, 4);
            this.textBoxMapTable.Margin = new System.Windows.Forms.Padding(0, 4, 3, 3);
            this.textBoxMapTable.Name = "textBoxMapTable";
            this.textBoxMapTable.ReadOnly = true;
            this.textBoxMapTable.Size = new System.Drawing.Size(150, 13);
            this.textBoxMapTable.TabIndex = 5;
            // 
            // labelMapColumn
            // 
            this.labelMapColumn.AutoSize = true;
            this.labelMapColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMapColumn.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelMapColumn.Location = new System.Drawing.Point(196, 0);
            this.labelMapColumn.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelMapColumn.Name = "labelMapColumn";
            this.labelMapColumn.Size = new System.Drawing.Size(45, 21);
            this.labelMapColumn.TabIndex = 8;
            this.labelMapColumn.Text = "Column:";
            this.labelMapColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMapColumn
            // 
            this.textBoxMapColumn.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMapColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMapColumn.Location = new System.Drawing.Point(241, 4);
            this.textBoxMapColumn.Margin = new System.Windows.Forms.Padding(0, 4, 3, 3);
            this.textBoxMapColumn.Name = "textBoxMapColumn";
            this.textBoxMapColumn.ReadOnly = true;
            this.textBoxMapColumn.Size = new System.Drawing.Size(150, 13);
            this.textBoxMapColumn.TabIndex = 9;
            // 
            // labelSequence
            // 
            this.labelSequence.AutoSize = true;
            this.labelSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSequence.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelSequence.Location = new System.Drawing.Point(557, 0);
            this.labelSequence.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelSequence.Name = "labelSequence";
            this.labelSequence.Size = new System.Drawing.Size(32, 21);
            this.labelSequence.TabIndex = 10;
            this.labelSequence.Text = "Seq.:";
            this.labelSequence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(609, 0);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(21, 21);
            this.buttonDelete.TabIndex = 11;
            this.toolTip.SetToolTip(this.buttonDelete, "Remove this definition from the import");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // textBoxSequence
            // 
            this.textBoxSequence.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSequence.Location = new System.Drawing.Point(589, 4);
            this.textBoxSequence.Margin = new System.Windows.Forms.Padding(0, 4, 3, 3);
            this.textBoxSequence.Name = "textBoxSequence";
            this.textBoxSequence.ReadOnly = true;
            this.textBoxSequence.Size = new System.Drawing.Size(17, 13);
            this.textBoxSequence.TabIndex = 12;
            // 
            // labelPreset
            // 
            this.labelPreset.AutoSize = true;
            this.labelPreset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPreset.Location = new System.Drawing.Point(537, 0);
            this.labelPreset.Name = "labelPreset";
            this.labelPreset.Size = new System.Drawing.Size(14, 21);
            this.labelPreset.TabIndex = 13;
            this.labelPreset.Text = "   ";
            // 
            // labelEntity
            // 
            this.labelEntity.AutoSize = true;
            this.labelEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEntity.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.labelEntity.Location = new System.Drawing.Point(397, 0);
            this.labelEntity.Name = "labelEntity";
            this.labelEntity.Size = new System.Drawing.Size(14, 21);
            this.labelEntity.TabIndex = 14;
            this.labelEntity.Text = "=";
            this.labelEntity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxEntity
            // 
            this.textBoxEntity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEntity.Location = new System.Drawing.Point(414, 4);
            this.textBoxEntity.Margin = new System.Windows.Forms.Padding(0, 4, 3, 3);
            this.textBoxEntity.Name = "textBoxEntity";
            this.textBoxEntity.ReadOnly = true;
            this.textBoxEntity.Size = new System.Drawing.Size(117, 13);
            this.textBoxEntity.TabIndex = 15;
            // 
            // UserControlImport_ColumnDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlImport_ColumnDisplay";
            this.Size = new System.Drawing.Size(630, 21);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelMapTable;
        private System.Windows.Forms.TextBox textBoxMapTable;
        private System.Windows.Forms.Label labelMapColumn;
        private System.Windows.Forms.TextBox textBoxMapColumn;
        private System.Windows.Forms.Label labelSequence;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TextBox textBoxSequence;
        private System.Windows.Forms.Label labelPreset;
        private System.Windows.Forms.Label labelEntity;
        private System.Windows.Forms.TextBox textBoxEntity;
    }
}
