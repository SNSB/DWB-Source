namespace DiversityWorkbench.Import
{
    partial class UserControlTransformationFilter
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
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.comboBoxFilterOperator = new System.Windows.Forms.ComboBox();
            this.buttonFilterColumn = new System.Windows.Forms.Button();
            this.labelFilterOperator = new System.Windows.Forms.Label();
            this.buttonRemoveFilter = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.textBoxFilter, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxFilterOperator, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonFilterColumn, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelFilterOperator, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonRemoveFilter, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(245, 27);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxFilter.Location = new System.Drawing.Point(173, 1);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(49, 20);
            this.textBoxFilter.TabIndex = 6;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // comboBoxFilterOperator
            // 
            this.comboBoxFilterOperator.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBoxFilterOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilterOperator.FormattingEnabled = true;
            this.comboBoxFilterOperator.Location = new System.Drawing.Point(133, 1);
            this.comboBoxFilterOperator.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
            this.comboBoxFilterOperator.Name = "comboBoxFilterOperator";
            this.comboBoxFilterOperator.Size = new System.Drawing.Size(40, 21);
            this.comboBoxFilterOperator.TabIndex = 5;
            this.comboBoxFilterOperator.DropDown += new System.EventHandler(this.comboBoxFilterOperator_DropDown);
            this.comboBoxFilterOperator.SelectionChangeCommitted += new System.EventHandler(this.comboBoxFilterOperator_SelectionChangeCommitted);
            // 
            // buttonFilterColumn
            // 
            this.buttonFilterColumn.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonFilterColumn.Image = global::DiversityWorkbench.Properties.Resources.MarkColumn;
            this.buttonFilterColumn.Location = new System.Drawing.Point(105, 0);
            this.buttonFilterColumn.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.buttonFilterColumn.Name = "buttonFilterColumn";
            this.buttonFilterColumn.Size = new System.Drawing.Size(28, 24);
            this.buttonFilterColumn.TabIndex = 4;
            this.buttonFilterColumn.UseVisualStyleBackColor = true;
            this.buttonFilterColumn.Click += new System.EventHandler(this.buttonFilterColumn_Click);
            // 
            // labelFilterOperator
            // 
            this.labelFilterOperator.AutoSize = true;
            this.labelFilterOperator.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelFilterOperator.Location = new System.Drawing.Point(3, 4);
            this.labelFilterOperator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.labelFilterOperator.Name = "labelFilterOperator";
            this.labelFilterOperator.Size = new System.Drawing.Size(99, 13);
            this.labelFilterOperator.TabIndex = 1;
            this.labelFilterOperator.Text = "if content in column";
            this.labelFilterOperator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonRemoveFilter
            // 
            this.buttonRemoveFilter.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemoveFilter.Location = new System.Drawing.Point(222, 0);
            this.buttonRemoveFilter.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.buttonRemoveFilter.Name = "buttonRemoveFilter";
            this.buttonRemoveFilter.Size = new System.Drawing.Size(23, 23);
            this.buttonRemoveFilter.TabIndex = 7;
            this.buttonRemoveFilter.UseVisualStyleBackColor = true;
            this.buttonRemoveFilter.Click += new System.EventHandler(this.buttonRemoveFilter_Click);
            // 
            // UserControlTransformationFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTransformationFilter";
            this.Size = new System.Drawing.Size(245, 27);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelFilterOperator;
        private System.Windows.Forms.Button buttonFilterColumn;
        private System.Windows.Forms.ComboBox comboBoxFilterOperator;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Button buttonRemoveFilter;
    }
}
