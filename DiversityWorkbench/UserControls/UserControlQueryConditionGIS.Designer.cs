namespace DiversityWorkbench.UserControls
{
    partial class UserControlQueryConditionGIS
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
            this.linkLabelValue = new System.Windows.Forms.LinkLabel();
            this.comboBoxQueryCondition = new System.Windows.Forms.ComboBox();
            this.buttonGetGeography = new System.Windows.Forms.Button();
            this.textBoxQueryCondition = new System.Windows.Forms.TextBox();
            this.labelNull = new System.Windows.Forms.Label();
            this.buttonQueryConditionOperator = new System.Windows.Forms.Button();
            this.comboBoxQueryConditionOperator = new System.Windows.Forms.ComboBox();
            this.buttonCondition = new System.Windows.Forms.Button();
            this.toolTipQueryCondition = new System.Windows.Forms.ToolTip(this.components);
            this.buttonGetGazetteer = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLabelValue
            // 
            this.tableLayoutPanel.SetColumnSpan(this.linkLabelValue, 2);
            this.linkLabelValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelValue.Location = new System.Drawing.Point(104, 22);
            this.linkLabelValue.Name = "linkLabelValue";
            this.linkLabelValue.Size = new System.Drawing.Size(76, 16);
            this.linkLabelValue.TabIndex = 22;
            this.linkLabelValue.TabStop = true;
            this.linkLabelValue.Text = "POINT(0.0 0.0)";
            this.linkLabelValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabelValue.Visible = false;
            this.linkLabelValue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelValue_LinkClicked);
            // 
            // comboBoxQueryCondition
            // 
            this.comboBoxQueryCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxQueryCondition.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxQueryCondition.FormattingEnabled = true;
            this.comboBoxQueryCondition.ItemHeight = 12;
            this.comboBoxQueryCondition.Location = new System.Drawing.Point(183, 22);
            this.comboBoxQueryCondition.Margin = new System.Windows.Forms.Padding(0);
            this.comboBoxQueryCondition.Name = "comboBoxQueryCondition";
            this.comboBoxQueryCondition.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.comboBoxQueryCondition.Size = new System.Drawing.Size(40, 20);
            this.comboBoxQueryCondition.TabIndex = 13;
            this.comboBoxQueryCondition.Text = "km";
            this.comboBoxQueryCondition.Visible = false;
            // 
            // buttonGetGeography
            // 
            this.buttonGetGeography.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGetGeography.FlatAppearance.BorderSize = 0;
            this.buttonGetGeography.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetGeography.Image = global::DiversityWorkbench.Properties.Resources.Earth;
            this.buttonGetGeography.Location = new System.Drawing.Point(101, 0);
            this.buttonGetGeography.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGetGeography.Name = "buttonGetGeography";
            this.buttonGetGeography.Size = new System.Drawing.Size(41, 22);
            this.buttonGetGeography.TabIndex = 21;
            this.toolTipQueryCondition.SetToolTip(this.buttonGetGeography, "Get a geography from a local file or via manual editing");
            this.buttonGetGeography.UseVisualStyleBackColor = true;
            this.buttonGetGeography.Click += new System.EventHandler(this.buttonGetGeography_Click);
            // 
            // textBoxQueryCondition
            // 
            this.textBoxQueryCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxQueryCondition.Location = new System.Drawing.Point(183, 0);
            this.textBoxQueryCondition.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxQueryCondition.Name = "textBoxQueryCondition";
            this.textBoxQueryCondition.Size = new System.Drawing.Size(40, 20);
            this.textBoxQueryCondition.TabIndex = 14;
            this.textBoxQueryCondition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelNull
            // 
            this.labelNull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNull.Location = new System.Drawing.Point(226, 0);
            this.labelNull.Name = "labelNull";
            this.tableLayoutPanel.SetRowSpan(this.labelNull, 2);
            this.labelNull.Size = new System.Drawing.Size(41, 38);
            this.labelNull.TabIndex = 19;
            this.labelNull.Text = "missing";
            this.labelNull.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNull.Visible = false;
            // 
            // buttonQueryConditionOperator
            // 
            this.buttonQueryConditionOperator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonQueryConditionOperator.FlatAppearance.BorderSize = 0;
            this.buttonQueryConditionOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQueryConditionOperator.Location = new System.Drawing.Point(84, 0);
            this.buttonQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0);
            this.buttonQueryConditionOperator.Name = "buttonQueryConditionOperator";
            this.tableLayoutPanel.SetRowSpan(this.buttonQueryConditionOperator, 2);
            this.buttonQueryConditionOperator.Size = new System.Drawing.Size(17, 38);
            this.buttonQueryConditionOperator.TabIndex = 11;
            this.buttonQueryConditionOperator.TabStop = false;
            this.buttonQueryConditionOperator.Text = "O";
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
            this.comboBoxQueryConditionOperator.Location = new System.Drawing.Point(67, 6);
            this.comboBoxQueryConditionOperator.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.comboBoxQueryConditionOperator.Name = "comboBoxQueryConditionOperator";
            this.tableLayoutPanel.SetRowSpan(this.comboBoxQueryConditionOperator, 2);
            this.comboBoxQueryConditionOperator.Size = new System.Drawing.Size(17, 21);
            this.comboBoxQueryConditionOperator.TabIndex = 10;
            this.comboBoxQueryConditionOperator.TabStop = false;
            this.comboBoxQueryConditionOperator.SelectedIndexChanged += new System.EventHandler(this.comboBoxQueryConditionOperator_SelectedIndexChanged);
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
            this.buttonCondition.Size = new System.Drawing.Size(67, 38);
            this.buttonCondition.TabIndex = 9;
            this.buttonCondition.TabStop = false;
            this.buttonCondition.Text = "Condition";
            this.buttonCondition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCondition.UseVisualStyleBackColor = true;
            // 
            // buttonGetGazetteer
            // 
            this.buttonGetGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGetGazetteer.FlatAppearance.BorderSize = 0;
            this.buttonGetGazetteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetGazetteer.Image = global::DiversityWorkbench.Properties.Resources.DiversityWorkbench16;
            this.buttonGetGazetteer.Location = new System.Drawing.Point(142, 0);
            this.buttonGetGazetteer.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGetGazetteer.Name = "buttonGetGazetteer";
            this.buttonGetGazetteer.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.buttonGetGazetteer.Size = new System.Drawing.Size(41, 22);
            this.buttonGetGazetteer.TabIndex = 23;
            this.toolTipQueryCondition.SetToolTip(this.buttonGetGazetteer, "Get a geography from DiversityGazetteer");
            this.buttonGetGazetteer.UseVisualStyleBackColor = true;
            this.buttonGetGazetteer.Click += new System.EventHandler(this.buttonGetGazetteer_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 7;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.textBoxQueryCondition, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonCondition, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxQueryCondition, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonQueryConditionOperator, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxQueryConditionOperator, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.linkLabelValue, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonGetGeography, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.labelNull, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonGetGazetteer, 4, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.82353F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.17647F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(270, 38);
            this.tableLayoutPanel.TabIndex = 10;
            // 
            // UserControlQueryConditionGIS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlQueryConditionGIS";
            this.Size = new System.Drawing.Size(270, 38);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxQueryCondition;
        private System.Windows.Forms.Label labelNull;
        private System.Windows.Forms.ComboBox comboBoxQueryCondition;
        private System.Windows.Forms.Button buttonQueryConditionOperator;
        private System.Windows.Forms.ComboBox comboBoxQueryConditionOperator;
        private System.Windows.Forms.Button buttonCondition;
        private System.Windows.Forms.ToolTip toolTipQueryCondition;
        private System.Windows.Forms.Button buttonGetGeography;
        private System.Windows.Forms.LinkLabel linkLabelValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonGetGazetteer;
    }
}
