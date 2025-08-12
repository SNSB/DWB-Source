namespace DiversityWorkbench.UserControls
{
    partial class UserControlWebserviceOption
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
            this.labelOption = new System.Windows.Forms.Label();
            this.checkBoxOption = new System.Windows.Forms.CheckBox();
            this.textBoxOption = new System.Windows.Forms.TextBox();
            this.comboBoxOption = new System.Windows.Forms.ComboBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.buttonShowDescription = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelOption, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxOption, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxOption, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.comboBoxOption, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.labelDescription, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonShowDescription, 5, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(407, 26);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelOption
            // 
            this.labelOption.AutoSize = true;
            this.labelOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOption.Location = new System.Drawing.Point(3, 0);
            this.labelOption.Name = "labelOption";
            this.labelOption.Size = new System.Drawing.Size(38, 26);
            this.labelOption.TabIndex = 0;
            this.labelOption.Text = "Option";
            this.labelOption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxOption
            // 
            this.checkBoxOption.AutoSize = true;
            this.checkBoxOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxOption.Location = new System.Drawing.Point(47, 3);
            this.checkBoxOption.Name = "checkBoxOption";
            this.checkBoxOption.Size = new System.Drawing.Size(80, 20);
            this.checkBoxOption.TabIndex = 1;
            this.checkBoxOption.Text = "checkBox1";
            this.checkBoxOption.UseVisualStyleBackColor = true;
            // 
            // textBoxOption
            // 
            this.textBoxOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOption.Location = new System.Drawing.Point(133, 3);
            this.textBoxOption.Name = "textBoxOption";
            this.textBoxOption.Size = new System.Drawing.Size(100, 20);
            this.textBoxOption.TabIndex = 2;
            // 
            // comboBoxOption
            // 
            this.comboBoxOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxOption.FormattingEnabled = true;
            this.comboBoxOption.Location = new System.Drawing.Point(239, 3);
            this.comboBoxOption.Name = "comboBoxOption";
            this.comboBoxOption.Size = new System.Drawing.Size(121, 21);
            this.comboBoxOption.TabIndex = 3;
            this.toolTip.SetToolTip(this.comboBoxOption, "Choose among the available values");
            this.comboBoxOption.SelectedIndexChanged += new System.EventHandler(this.comboBoxOption_SelectedIndexChanged);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Location = new System.Drawing.Point(363, 0);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(28, 26);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description";
            // 
            // buttonShowDescription
            // 
            this.buttonShowDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonShowDescription.FlatAppearance.BorderSize = 0;
            this.buttonShowDescription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShowDescription.Image = global::DiversityWorkbench.Properties.Resources.Manual;
            this.buttonShowDescription.Location = new System.Drawing.Point(391, 0);
            this.buttonShowDescription.Margin = new System.Windows.Forms.Padding(0);
            this.buttonShowDescription.Name = "buttonShowDescription";
            this.buttonShowDescription.Size = new System.Drawing.Size(16, 26);
            this.buttonShowDescription.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonShowDescription, "Show description of the option");
            this.buttonShowDescription.UseVisualStyleBackColor = true;
            this.buttonShowDescription.Click += new System.EventHandler(this.buttonShowDescription_Click);
            // 
            // UserControlWebserviceOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlWebserviceOption";
            this.Size = new System.Drawing.Size(407, 26);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelOption;
        private System.Windows.Forms.CheckBox checkBoxOption;
        private System.Windows.Forms.TextBox textBoxOption;
        private System.Windows.Forms.ComboBox comboBoxOption;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonShowDescription;
    }
}
