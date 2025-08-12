namespace DiversityWorkbench.UserControls
{
    partial class UserControlColumnFilter
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
            this.labelColumn = new System.Windows.Forms.Label();
            this.comboBoxOperator = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelValues = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.labelBetween = new System.Windows.Forms.Label();
            this.textBoxFilterUpper = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanelValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.labelColumn, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxOperator, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelValues, 2, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(388, 27);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelColumn
            // 
            this.labelColumn.AutoSize = true;
            this.labelColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelColumn.Location = new System.Drawing.Point(3, 0);
            this.labelColumn.Name = "labelColumn";
            this.labelColumn.Size = new System.Drawing.Size(170, 27);
            this.labelColumn.TabIndex = 0;
            this.labelColumn.Text = "Column";
            this.labelColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.labelColumn, "Name of the table column");
            // 
            // comboBoxOperator
            // 
            this.comboBoxOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxOperator.FormattingEnabled = true;
            this.comboBoxOperator.Items.AddRange(new object[] {
            "=",
            "~",
            "≠",
            "<",
            ">",
            "-",
            "|",
            "Ø",
            "•"});
            this.comboBoxOperator.Location = new System.Drawing.Point(179, 3);
            this.comboBoxOperator.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.comboBoxOperator.Name = "comboBoxOperator";
            this.comboBoxOperator.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.comboBoxOperator.Size = new System.Drawing.Size(32, 21);
            this.comboBoxOperator.TabIndex = 1;
            this.comboBoxOperator.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperator_SelectedIndexChanged);
            // 
            // tableLayoutPanelValues
            // 
            this.tableLayoutPanelValues.ColumnCount = 3;
            this.tableLayoutPanelValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelValues.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelValues.Controls.Add(this.textBoxFilter, 0, 0);
            this.tableLayoutPanelValues.Controls.Add(this.labelBetween, 1, 0);
            this.tableLayoutPanelValues.Controls.Add(this.textBoxFilterUpper, 2, 0);
            this.tableLayoutPanelValues.Location = new System.Drawing.Point(211, 0);
            this.tableLayoutPanelValues.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelValues.Name = "tableLayoutPanelValues";
            this.tableLayoutPanelValues.RowCount = 1;
            this.tableLayoutPanelValues.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelValues.Size = new System.Drawing.Size(177, 27);
            this.tableLayoutPanelValues.TabIndex = 3;
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilter.Location = new System.Drawing.Point(0, 3);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(108, 20);
            this.textBoxFilter.TabIndex = 2;
            this.toolTip.SetToolTip(this.textBoxFilter, "Filter criteria for the column");
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            this.textBoxFilter.DoubleClick += new System.EventHandler(this.textBoxFilter_DoubleClick);
            // 
            // labelBetween
            // 
            this.labelBetween.AutoSize = true;
            this.labelBetween.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBetween.Location = new System.Drawing.Point(111, 0);
            this.labelBetween.Margin = new System.Windows.Forms.Padding(0);
            this.labelBetween.Name = "labelBetween";
            this.labelBetween.Size = new System.Drawing.Size(10, 27);
            this.labelBetween.TabIndex = 3;
            this.labelBetween.Text = "-";
            this.labelBetween.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelBetween.Visible = false;
            // 
            // textBoxFilterUpper
            // 
            this.textBoxFilterUpper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxFilterUpper.Location = new System.Drawing.Point(124, 3);
            this.textBoxFilterUpper.Name = "textBoxFilterUpper";
            this.textBoxFilterUpper.Size = new System.Drawing.Size(50, 20);
            this.textBoxFilterUpper.TabIndex = 4;
            this.toolTip.SetToolTip(this.textBoxFilterUpper, "Upper value of the filter");
            this.textBoxFilterUpper.Visible = false;
            this.textBoxFilterUpper.TextChanged += new System.EventHandler(this.textBoxFilterUpper_TextChanged);
            // 
            // UserControlColumnFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlColumnFilter";
            this.Size = new System.Drawing.Size(388, 27);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.tableLayoutPanelValues.ResumeLayout(false);
            this.tableLayoutPanelValues.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelColumn;
        private System.Windows.Forms.ComboBox comboBoxOperator;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelValues;
        private System.Windows.Forms.Label labelBetween;
        private System.Windows.Forms.TextBox textBoxFilterUpper;
    }
}
