namespace DiversityWorkbench.XslEditor
{
    partial class UserControlXslTableColumn
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
            this.maskedTextBoxWidth = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // maskedTextBoxWidth
            // 
            this.maskedTextBoxWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.maskedTextBoxWidth.Dock = System.Windows.Forms.DockStyle.Left;
            this.maskedTextBoxWidth.Location = new System.Drawing.Point(0, 0);
            this.maskedTextBoxWidth.Mask = "000";
            this.maskedTextBoxWidth.Name = "maskedTextBoxWidth";
            this.maskedTextBoxWidth.Size = new System.Drawing.Size(20, 13);
            this.maskedTextBoxWidth.TabIndex = 1;
            this.maskedTextBoxWidth.TextChanged += new System.EventHandler(this.maskedTextBoxWidth_TextChanged);
            // 
            // UserControlXslTableColumn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.maskedTextBoxWidth);
            this.Name = "UserControlXslTableColumn";
            this.Size = new System.Drawing.Size(20, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox maskedTextBoxWidth;
    }
}
