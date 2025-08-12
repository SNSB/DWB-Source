namespace DiversityWorkbench.XslEditor
{
    partial class UserControlXslAttributeCondition
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
            this.comboBoxBoolOperator = new System.Windows.Forms.ComboBox();
            this.comboBoxValue = new System.Windows.Forms.ComboBox();
            this.comboBoxOperator = new System.Windows.Forms.ComboBox();
            this.comboBoxCompareTo = new System.Windows.Forms.ComboBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonBracketOpen = new System.Windows.Forms.Button();
            this.buttonBracketClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.comboBoxBoolOperator, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxValue, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxOperator, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxCompareTo, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonRemove, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonBracketOpen, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonBracketClose, 5, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(200, 22);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // comboBoxBoolOperator
            // 
            this.comboBoxBoolOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxBoolOperator.FormattingEnabled = true;
            this.comboBoxBoolOperator.Items.AddRange(new object[] {
            "and",
            "or",
            "and not",
            "or not"});
            this.comboBoxBoolOperator.Location = new System.Drawing.Point(0, 0);
            this.comboBoxBoolOperator.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxBoolOperator.Name = "comboBoxBoolOperator";
            this.comboBoxBoolOperator.Size = new System.Drawing.Size(60, 21);
            this.comboBoxBoolOperator.TabIndex = 0;
            // 
            // comboBoxValue
            // 
            this.comboBoxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxValue.FormattingEnabled = true;
            this.comboBoxValue.Location = new System.Drawing.Point(74, 0);
            this.comboBoxValue.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(42, 21);
            this.comboBoxValue.TabIndex = 1;
            this.comboBoxValue.DropDown += new System.EventHandler(this.comboBoxValue_DropDown);
            // 
            // comboBoxOperator
            // 
            this.comboBoxOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxOperator.FormattingEnabled = true;
            this.comboBoxOperator.Items.AddRange(new object[] {
            "=",
            "≠",
            "<",
            ">"});
            this.comboBoxOperator.Location = new System.Drawing.Point(116, 0);
            this.comboBoxOperator.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxOperator.Name = "comboBoxOperator";
            this.comboBoxOperator.Size = new System.Drawing.Size(30, 21);
            this.comboBoxOperator.TabIndex = 2;
            // 
            // comboBoxCompareTo
            // 
            this.comboBoxCompareTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCompareTo.FormattingEnabled = true;
            this.comboBoxCompareTo.Location = new System.Drawing.Point(146, 0);
            this.comboBoxCompareTo.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxCompareTo.Name = "comboBoxCompareTo";
            this.comboBoxCompareTo.Size = new System.Drawing.Size(18, 21);
            this.comboBoxCompareTo.TabIndex = 3;
            this.comboBoxCompareTo.DropDown += new System.EventHandler(this.comboBoxCompareTo_DropDown);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonRemove.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonRemove.Location = new System.Drawing.Point(178, 0);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(22, 22);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Visible = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonBracketOpen
            // 
            this.buttonBracketOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBracketOpen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBracketOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBracketOpen.Location = new System.Drawing.Point(60, 0);
            this.buttonBracketOpen.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBracketOpen.Name = "buttonBracketOpen";
            this.buttonBracketOpen.Size = new System.Drawing.Size(14, 22);
            this.buttonBracketOpen.TabIndex = 5;
            this.buttonBracketOpen.Text = "(";
            this.buttonBracketOpen.UseVisualStyleBackColor = true;
            this.buttonBracketOpen.Click += new System.EventHandler(this.buttonBracketOpen_Click);
            // 
            // buttonBracketClose
            // 
            this.buttonBracketClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBracketClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBracketClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBracketClose.Location = new System.Drawing.Point(164, 0);
            this.buttonBracketClose.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBracketClose.Name = "buttonBracketClose";
            this.buttonBracketClose.Size = new System.Drawing.Size(14, 22);
            this.buttonBracketClose.TabIndex = 6;
            this.buttonBracketClose.Text = ")";
            this.buttonBracketClose.UseVisualStyleBackColor = true;
            this.buttonBracketClose.Click += new System.EventHandler(this.buttonBracketClose_Click);
            // 
            // UserControlXslAttributeCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlXslAttributeCondition";
            this.Size = new System.Drawing.Size(200, 22);
            this.Leave += new System.EventHandler(this.UserControlXslAttributeCondition_Leave);
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ComboBox comboBoxBoolOperator;
        private System.Windows.Forms.ComboBox comboBoxValue;
        private System.Windows.Forms.ComboBox comboBoxOperator;
        private System.Windows.Forms.ComboBox comboBoxCompareTo;
        private System.Windows.Forms.Button buttonBracketOpen;
        private System.Windows.Forms.Button buttonBracketClose;
        public System.Windows.Forms.Button buttonRemove;
    }
}
