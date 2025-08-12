namespace DiversityWorkbench.XslEditor
{
    partial class UserControlXslTableRow
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
            this.panel = new System.Windows.Forms.Panel();
            this.buttonOptions = new System.Windows.Forms.Button();
            this.maskedTextBoxHeight = new System.Windows.Forms.MaskedTextBox();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.buttonOptions);
            this.panel.Controls.Add(this.maskedTextBoxHeight);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(40, 14);
            this.panel.TabIndex = 2;
            // 
            // buttonOptions
            // 
            this.buttonOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOptions.Location = new System.Drawing.Point(20, 0);
            this.buttonOptions.Name = "buttonOptions";
            this.buttonOptions.Size = new System.Drawing.Size(18, 13);
            this.buttonOptions.TabIndex = 1;
            this.buttonOptions.UseVisualStyleBackColor = true;
            // 
            // maskedTextBoxHeight
            // 
            this.maskedTextBoxHeight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.maskedTextBoxHeight.Dock = System.Windows.Forms.DockStyle.Left;
            this.maskedTextBoxHeight.Location = new System.Drawing.Point(0, 0);
            this.maskedTextBoxHeight.Mask = "000";
            this.maskedTextBoxHeight.Name = "maskedTextBoxHeight";
            this.maskedTextBoxHeight.Size = new System.Drawing.Size(20, 13);
            this.maskedTextBoxHeight.TabIndex = 0;
            this.maskedTextBoxHeight.Text = "30";
            // 
            // UserControlXslTableRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Name = "UserControlXslTableRow";
            this.Size = new System.Drawing.Size(40, 14);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        public System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxHeight;
    }
}
