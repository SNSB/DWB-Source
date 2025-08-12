namespace DiversityWorkbench.UserControls
{
    partial class UserControlQueryOrderColumn
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
            this.labelDisplayText = new System.Windows.Forms.Label();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.maskedTextBoxWidth = new System.Windows.Forms.MaskedTextBox();
            this.pictureBoxWidth = new System.Windows.Forms.PictureBox();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSwitchDescAsc = new System.Windows.Forms.Button();
            this.panelRemeberQuerySettingsSpacer = new System.Windows.Forms.Panel();
            this.panelOptimizingSpacer = new System.Windows.Forms.Panel();
            this.panelSpacer1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWidth)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDisplayText
            // 
            this.labelDisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDisplayText.Location = new System.Drawing.Point(41, 0);
            this.labelDisplayText.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.labelDisplayText.Name = "labelDisplayText";
            this.labelDisplayText.Size = new System.Drawing.Size(78, 14);
            this.labelDisplayText.TabIndex = 0;
            this.labelDisplayText.Text = "label1";
            this.labelDisplayText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonRemove
            // 
            this.buttonRemove.FlatAppearance.BorderSize = 0;
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.Image = global::DiversityWorkbench.Properties.Resources.Minus;
            this.buttonRemove.Location = new System.Drawing.Point(119, 0);
            this.buttonRemove.Margin = new System.Windows.Forms.Padding(0);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(18, 14);
            this.buttonRemove.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonRemove, "Remove this column from the list");
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // maskedTextBoxWidth
            // 
            this.maskedTextBoxWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.maskedTextBoxWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maskedTextBoxWidth.Location = new System.Drawing.Point(137, 0);
            this.maskedTextBoxWidth.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBoxWidth.Mask = "00";
            this.maskedTextBoxWidth.Name = "maskedTextBoxWidth";
            this.maskedTextBoxWidth.Size = new System.Drawing.Size(14, 13);
            this.maskedTextBoxWidth.TabIndex = 2;
            this.maskedTextBoxWidth.Text = "20";
            this.toolTip.SetToolTip(this.maskedTextBoxWidth, "Set width of column in result list");
            this.maskedTextBoxWidth.TextChanged += new System.EventHandler(this.maskedTextBoxWidth_TextChanged);
            // 
            // pictureBoxWidth
            // 
            this.pictureBoxWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxWidth.Image = global::DiversityWorkbench.Properties.Resources.ArrowWidthSmall;
            this.pictureBoxWidth.Location = new System.Drawing.Point(151, 0);
            this.pictureBoxWidth.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxWidth.Name = "pictureBoxWidth";
            this.pictureBoxWidth.Size = new System.Drawing.Size(14, 14);
            this.pictureBoxWidth.TabIndex = 3;
            this.pictureBoxWidth.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxWidth, "set width to max. of current data selection");
            this.pictureBoxWidth.Click += new System.EventHandler(this.pictureBoxWidth_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUp.FlatAppearance.BorderSize = 0;
            this.buttonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUp.Image = global::DiversityWorkbench.Properties.Resources.ArrowUp;
            this.buttonUp.Location = new System.Drawing.Point(10, 0);
            this.buttonUp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(15, 14);
            this.buttonUp.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonUp, "move column up");
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDown.FlatAppearance.BorderSize = 0;
            this.buttonDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDown.Image = global::DiversityWorkbench.Properties.Resources.ArrowDown;
            this.buttonDown.Location = new System.Drawing.Point(25, 0);
            this.buttonDown.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(15, 14);
            this.buttonDown.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonDown, "move column down");
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Visible = false;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 10;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.labelDisplayText, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.maskedTextBoxWidth, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonRemove, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBoxWidth, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonSwitchDescAsc, 8, 0);
            this.tableLayoutPanel.Controls.Add(this.panelRemeberQuerySettingsSpacer, 7, 0);
            this.tableLayoutPanel.Controls.Add(this.panelOptimizingSpacer, 9, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonUp, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonDown, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.panelSpacer1, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(225, 14);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // buttonSwitchDescAsc
            // 
            this.buttonSwitchDescAsc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSwitchDescAsc.Image = global::DiversityWorkbench.Properties.Resources.ArrowDownSmall;
            this.buttonSwitchDescAsc.Location = new System.Drawing.Point(183, 0);
            this.buttonSwitchDescAsc.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSwitchDescAsc.Name = "buttonSwitchDescAsc";
            this.buttonSwitchDescAsc.Size = new System.Drawing.Size(22, 14);
            this.buttonSwitchDescAsc.TabIndex = 4;
            this.buttonSwitchDescAsc.UseVisualStyleBackColor = true;
            this.buttonSwitchDescAsc.Click += new System.EventHandler(this.buttonSwitchDescAsc_Click);
            // 
            // panelRemeberQuerySettingsSpacer
            // 
            this.panelRemeberQuerySettingsSpacer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRemeberQuerySettingsSpacer.Location = new System.Drawing.Point(165, 0);
            this.panelRemeberQuerySettingsSpacer.Margin = new System.Windows.Forms.Padding(0);
            this.panelRemeberQuerySettingsSpacer.Name = "panelRemeberQuerySettingsSpacer";
            this.panelRemeberQuerySettingsSpacer.Size = new System.Drawing.Size(18, 14);
            this.panelRemeberQuerySettingsSpacer.TabIndex = 5;
            // 
            // panelOptimizingSpacer
            // 
            this.panelOptimizingSpacer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOptimizingSpacer.Location = new System.Drawing.Point(208, 3);
            this.panelOptimizingSpacer.Name = "panelOptimizingSpacer";
            this.panelOptimizingSpacer.Size = new System.Drawing.Size(14, 8);
            this.panelOptimizingSpacer.TabIndex = 6;
            // 
            // panelSpacer1
            // 
            this.panelSpacer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSpacer1.Location = new System.Drawing.Point(0, 0);
            this.panelSpacer1.Margin = new System.Windows.Forms.Padding(0);
            this.panelSpacer1.Name = "panelSpacer1";
            this.panelSpacer1.Size = new System.Drawing.Size(10, 14);
            this.panelSpacer1.TabIndex = 9;
            // 
            // UserControlQueryOrderColumn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserControlQueryOrderColumn";
            this.Size = new System.Drawing.Size(225, 14);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWidth)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.Label labelDisplayText;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxWidth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBoxWidth;
        private System.Windows.Forms.Button buttonSwitchDescAsc;
        private System.Windows.Forms.Panel panelRemeberQuerySettingsSpacer;
        private System.Windows.Forms.Panel panelOptimizingSpacer;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Panel panelSpacer1;
    }
}
