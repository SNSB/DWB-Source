namespace DiversityWorkbench.UserControls
{
    partial class UserControlQueryConditionXML
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
            this.textBoxQueryCondition = new System.Windows.Forms.TextBox();
            this.buttonCondition = new System.Windows.Forms.Button();
            this.buttonQueryConditionOperator = new System.Windows.Forms.Button();
            this.comboBoxQueryConditionOperator = new System.Windows.Forms.ComboBox();
            this.labelNull = new System.Windows.Forms.Label();
            this.menuStripXMLTemplate = new System.Windows.Forms.MenuStrip();
            this.textBoxXMLTemplate = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Controls.Add(this.textBoxQueryCondition, 4, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonCondition, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonQueryConditionOperator, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.comboBoxQueryConditionOperator, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelNull, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.menuStripXMLTemplate, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxXMLTemplate, 3, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.82353F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(472, 39);
            this.tableLayoutPanel.TabIndex = 11;
            // 
            // textBoxQueryCondition
            // 
            this.textBoxQueryCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxQueryCondition.Location = new System.Drawing.Point(385, 19);
            this.textBoxQueryCondition.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxQueryCondition.Name = "textBoxQueryCondition";
            this.textBoxQueryCondition.Size = new System.Drawing.Size(40, 20);
            this.textBoxQueryCondition.TabIndex = 14;
            this.textBoxQueryCondition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonCondition
            // 
            this.buttonCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCondition.FlatAppearance.BorderSize = 0;
            this.buttonCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCondition.Location = new System.Drawing.Point(0, 0);
            this.buttonCondition.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCondition.Name = "buttonCondition";
            this.tableLayoutPanel.SetRowSpan(this.buttonCondition, 2);
            this.buttonCondition.Size = new System.Drawing.Size(67, 39);
            this.buttonCondition.TabIndex = 9;
            this.buttonCondition.TabStop = false;
            this.buttonCondition.Text = "Condition";
            this.buttonCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCondition.UseVisualStyleBackColor = true;
            // 
            // buttonQueryConditionOperator
            // 
            this.buttonQueryConditionOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonQueryConditionOperator.FlatAppearance.BorderSize = 0;
            this.buttonQueryConditionOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQueryConditionOperator.Location = new System.Drawing.Point(84, 19);
            this.buttonQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            this.buttonQueryConditionOperator.Name = "buttonQueryConditionOperator";
            this.buttonQueryConditionOperator.Size = new System.Drawing.Size(17, 20);
            this.buttonQueryConditionOperator.TabIndex = 11;
            this.buttonQueryConditionOperator.TabStop = false;
            this.buttonQueryConditionOperator.Text = "O";
            this.buttonQueryConditionOperator.UseVisualStyleBackColor = true;
            // 
            // comboBoxQueryConditionOperator
            // 
            this.comboBoxQueryConditionOperator.BackColor = System.Drawing.SystemColors.Control;
            this.comboBoxQueryConditionOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxQueryConditionOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxQueryConditionOperator.DropDownWidth = 20;
            this.comboBoxQueryConditionOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxQueryConditionOperator.FormattingEnabled = true;
            this.comboBoxQueryConditionOperator.Items.AddRange(new object[] {
            "~",
            "=",
            "≠",
            "Ø",
            "•",
            "—",
            "<",
            ">"});
            this.comboBoxQueryConditionOperator.Location = new System.Drawing.Point(67, 19);
            this.comboBoxQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxQueryConditionOperator.Name = "comboBoxQueryConditionOperator";
            this.comboBoxQueryConditionOperator.Size = new System.Drawing.Size(17, 21);
            this.comboBoxQueryConditionOperator.TabIndex = 10;
            this.comboBoxQueryConditionOperator.TabStop = false;
            // 
            // labelNull
            // 
            this.labelNull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNull.Location = new System.Drawing.Point(428, 19);
            this.labelNull.Name = "labelNull";
            this.labelNull.Size = new System.Drawing.Size(41, 20);
            this.labelNull.TabIndex = 19;
            this.labelNull.Text = "missing";
            this.labelNull.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNull.Visible = false;
            // 
            // menuStripXMLTemplate
            // 
            this.menuStripXMLTemplate.Location = new System.Drawing.Point(67, 0);
            this.menuStripXMLTemplate.Name = "menuStripXMLTemplate";
            this.menuStripXMLTemplate.Size = new System.Drawing.Size(17, 19);
            this.menuStripXMLTemplate.TabIndex = 20;
            this.menuStripXMLTemplate.Text = "menuStrip1";
            // 
            // textBoxXMLTemplate
            // 
            this.textBoxXMLTemplate.Location = new System.Drawing.Point(104, 3);
            this.textBoxXMLTemplate.Name = "textBoxXMLTemplate";
            this.textBoxXMLTemplate.Size = new System.Drawing.Size(100, 20);
            this.textBoxXMLTemplate.TabIndex = 21;
            // 
            // UserControlQueryConditionXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlQueryConditionXML";
            this.Size = new System.Drawing.Size(472, 39);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textBoxQueryCondition;
        private System.Windows.Forms.Button buttonCondition;
        private System.Windows.Forms.Button buttonQueryConditionOperator;
        private System.Windows.Forms.ComboBox comboBoxQueryConditionOperator;
        private System.Windows.Forms.Label labelNull;
        private System.Windows.Forms.MenuStrip menuStripXMLTemplate;
        private System.Windows.Forms.TextBox textBoxXMLTemplate;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
