namespace DiversityWorkbench.UserControls
{
    partial class UserControlQueryConditionHierarchy
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
            this.buttonCondition = new System.Windows.Forms.Button();
            this.buttonQueryConditionOperator = new System.Windows.Forms.Button();
            this.comboBoxQueryConditionOperator = new System.Windows.Forms.ComboBox();
            this.labelNull = new System.Windows.Forms.Label();
            this.comboBoxQueryCondition = new System.Windows.Forms.ComboBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolTipQueryCondition = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.buttonCondition, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonQueryConditionOperator, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxQueryConditionOperator, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelNull, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxQueryCondition, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.toolStrip, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(270, 22);
            this.tableLayoutPanel.TabIndex = 11;
            // 
            // buttonCondition
            // 
            this.buttonCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCondition.FlatAppearance.BorderSize = 0;
            this.buttonCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCondition.Location = new System.Drawing.Point(0, 0);
            this.buttonCondition.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCondition.Name = "buttonCondition";
            this.buttonCondition.Size = new System.Drawing.Size(67, 22);
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
            this.buttonQueryConditionOperator.Location = new System.Drawing.Point(84, 0);
            this.buttonQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            this.buttonQueryConditionOperator.Name = "buttonQueryConditionOperator";
            this.buttonQueryConditionOperator.Size = new System.Drawing.Size(17, 22);
            this.buttonQueryConditionOperator.TabIndex = 11;
            this.buttonQueryConditionOperator.TabStop = false;
            this.buttonQueryConditionOperator.Text = "∆";
            this.buttonQueryConditionOperator.UseVisualStyleBackColor = true;
            this.buttonQueryConditionOperator.TextChanged += new System.EventHandler(this.buttonQueryConditionOperator_TextChanged);
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
            this.comboBoxQueryConditionOperator.Location = new System.Drawing.Point(67, 0);
            this.comboBoxQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxQueryConditionOperator.Name = "comboBoxQueryConditionOperator";
            this.comboBoxQueryConditionOperator.Size = new System.Drawing.Size(17, 21);
            this.comboBoxQueryConditionOperator.TabIndex = 10;
            this.comboBoxQueryConditionOperator.TabStop = false;
            this.comboBoxQueryConditionOperator.SelectedIndexChanged += new System.EventHandler(this.comboBoxQueryConditionOperator_SelectedIndexChanged);
            // 
            // labelNull
            // 
            this.labelNull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNull.Location = new System.Drawing.Point(226, 0);
            this.labelNull.Name = "labelNull";
            this.labelNull.Size = new System.Drawing.Size(41, 22);
            this.labelNull.TabIndex = 19;
            this.labelNull.Text = "missing";
            this.labelNull.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNull.Visible = false;
            // 
            // comboBoxQueryCondition
            // 
            this.comboBoxQueryCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxQueryCondition.DropDownWidth = 200;
            this.comboBoxQueryCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxQueryCondition.FormattingEnabled = true;
            this.comboBoxQueryCondition.ItemHeight = 13;
            this.comboBoxQueryCondition.Location = new System.Drawing.Point(101, 0);
            this.comboBoxQueryCondition.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxQueryCondition.MaxDropDownItems = 20;
            this.comboBoxQueryCondition.Name = "comboBoxQueryCondition";
            this.comboBoxQueryCondition.Size = new System.Drawing.Size(1, 21);
            this.comboBoxQueryCondition.TabIndex = 13;
            this.comboBoxQueryCondition.Visible = false;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton});
            this.toolStrip.Location = new System.Drawing.Point(102, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(121, 22);
            this.toolStrip.TabIndex = 20;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripDropDownButton
            // 
            this.toolStripDropDownButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton.Name = "toolStripDropDownButton";
            this.toolStripDropDownButton.Size = new System.Drawing.Size(13, 19);
            // 
            // UserControlQueryConditionHierarchy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlQueryConditionHierarchy";
            this.Size = new System.Drawing.Size(270, 22);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonCondition;
        private System.Windows.Forms.ComboBox comboBoxQueryCondition;
        private System.Windows.Forms.Button buttonQueryConditionOperator;
        private System.Windows.Forms.ComboBox comboBoxQueryConditionOperator;
        private System.Windows.Forms.Label labelNull;
        private System.Windows.Forms.ToolTip toolTipQueryCondition;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton;
    }
}
