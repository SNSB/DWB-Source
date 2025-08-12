namespace DiversityWorkbench.UserControls
{
    partial class UserControlColorSettings
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
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonInapplicable = new System.Windows.Forms.Button();
            this.buttonReadOnly = new System.Windows.Forms.Button();
            this.checkBoxHideInapplicable = new System.Windows.Forms.CheckBox();
            this.checkBoxHideReadOnly = new System.Windows.Forms.CheckBox();
            this.buttonServiceLink = new System.Windows.Forms.Button();
            this.buttonCalculated = new System.Windows.Forms.Button();
            this.checkBoxHideServiceLink = new System.Windows.Forms.CheckBox();
            this.checkBoxHideCalculated = new System.Windows.Forms.CheckBox();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Controls.Add(this.buttonInapplicable, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonReadOnly, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxHideInapplicable, 3, 0);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxHideReadOnly, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonServiceLink, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCalculated, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxHideServiceLink, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxHideCalculated, 3, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(194, 65);
            this.tableLayoutPanelMain.TabIndex = 7;
            // 
            // buttonInapplicable
            // 
            this.buttonInapplicable.BackColor = System.Drawing.Color.LightGray;
            this.buttonInapplicable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonInapplicable.ForeColor = System.Drawing.Color.DimGray;
            this.buttonInapplicable.Location = new System.Drawing.Point(98, 1);
            this.buttonInapplicable.Margin = new System.Windows.Forms.Padding(1);
            this.buttonInapplicable.Name = "buttonInapplicable";
            this.buttonInapplicable.Size = new System.Drawing.Size(74, 30);
            this.buttonInapplicable.TabIndex = 6;
            this.buttonInapplicable.Text = "Inapplicable";
            this.buttonInapplicable.UseVisualStyleBackColor = false;
            // 
            // buttonReadOnly
            // 
            this.buttonReadOnly.BackColor = System.Drawing.Color.Pink;
            this.buttonReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReadOnly.Location = new System.Drawing.Point(1, 1);
            this.buttonReadOnly.Margin = new System.Windows.Forms.Padding(1);
            this.buttonReadOnly.Name = "buttonReadOnly";
            this.buttonReadOnly.Size = new System.Drawing.Size(74, 30);
            this.buttonReadOnly.TabIndex = 8;
            this.buttonReadOnly.Text = "Read only";
            this.buttonReadOnly.UseVisualStyleBackColor = false;
            // 
            // checkBoxHideInapplicable
            // 
            this.checkBoxHideInapplicable.AutoSize = true;
            this.checkBoxHideInapplicable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxHideInapplicable.Location = new System.Drawing.Point(176, 3);
            this.checkBoxHideInapplicable.Name = "checkBoxHideInapplicable";
            this.checkBoxHideInapplicable.Size = new System.Drawing.Size(15, 26);
            this.checkBoxHideInapplicable.TabIndex = 10;
            this.checkBoxHideInapplicable.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideReadOnly
            // 
            this.checkBoxHideReadOnly.AutoSize = true;
            this.checkBoxHideReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxHideReadOnly.Location = new System.Drawing.Point(79, 3);
            this.checkBoxHideReadOnly.Name = "checkBoxHideReadOnly";
            this.checkBoxHideReadOnly.Size = new System.Drawing.Size(15, 26);
            this.checkBoxHideReadOnly.TabIndex = 11;
            this.checkBoxHideReadOnly.UseVisualStyleBackColor = true;
            // 
            // buttonServiceLink
            // 
            this.buttonServiceLink.BackColor = System.Drawing.Color.Aqua;
            this.buttonServiceLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonServiceLink.Location = new System.Drawing.Point(1, 33);
            this.buttonServiceLink.Margin = new System.Windows.Forms.Padding(1);
            this.buttonServiceLink.Name = "buttonServiceLink";
            this.buttonServiceLink.Size = new System.Drawing.Size(74, 31);
            this.buttonServiceLink.TabIndex = 23;
            this.buttonServiceLink.Text = "Service link";
            this.buttonServiceLink.UseVisualStyleBackColor = false;
            // 
            // buttonCalculated
            // 
            this.buttonCalculated.BackColor = System.Drawing.Color.Yellow;
            this.buttonCalculated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCalculated.Location = new System.Drawing.Point(98, 33);
            this.buttonCalculated.Margin = new System.Windows.Forms.Padding(1);
            this.buttonCalculated.Name = "buttonCalculated";
            this.buttonCalculated.Size = new System.Drawing.Size(74, 31);
            this.buttonCalculated.TabIndex = 26;
            this.buttonCalculated.Text = "Calculated";
            this.buttonCalculated.UseVisualStyleBackColor = false;
            // 
            // checkBoxHideServiceLink
            // 
            this.checkBoxHideServiceLink.AutoSize = true;
            this.checkBoxHideServiceLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxHideServiceLink.Location = new System.Drawing.Point(79, 35);
            this.checkBoxHideServiceLink.Name = "checkBoxHideServiceLink";
            this.checkBoxHideServiceLink.Size = new System.Drawing.Size(15, 27);
            this.checkBoxHideServiceLink.TabIndex = 27;
            this.checkBoxHideServiceLink.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideCalculated
            // 
            this.checkBoxHideCalculated.AutoSize = true;
            this.checkBoxHideCalculated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxHideCalculated.Location = new System.Drawing.Point(176, 35);
            this.checkBoxHideCalculated.Name = "checkBoxHideCalculated";
            this.checkBoxHideCalculated.Size = new System.Drawing.Size(15, 27);
            this.checkBoxHideCalculated.TabIndex = 28;
            this.checkBoxHideCalculated.UseVisualStyleBackColor = true;
            // 
            // UserControlColorSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "UserControlColorSettings";
            this.Size = new System.Drawing.Size(194, 65);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonInapplicable;
        private System.Windows.Forms.Button buttonReadOnly;
        private System.Windows.Forms.CheckBox checkBoxHideInapplicable;
        private System.Windows.Forms.CheckBox checkBoxHideReadOnly;
        private System.Windows.Forms.Button buttonServiceLink;
        private System.Windows.Forms.Button buttonCalculated;
        private System.Windows.Forms.CheckBox checkBoxHideServiceLink;
        private System.Windows.Forms.CheckBox checkBoxHideCalculated;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
