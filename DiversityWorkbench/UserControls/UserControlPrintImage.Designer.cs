namespace DiversityWorkbench.UserControls
{
    partial class UserControlPrintImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlPrintImage));
            this.buttonPrintImage = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.SuspendLayout();
            // 
            // buttonPrintImage
            // 
            this.buttonPrintImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPrintImage.Image = ((System.Drawing.Image)(resources.GetObject("buttonPrintImage.Image")));
            this.buttonPrintImage.Location = new System.Drawing.Point(0, 0);
            this.buttonPrintImage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPrintImage.Name = "buttonPrintImage";
            this.buttonPrintImage.Size = new System.Drawing.Size(23, 23);
            this.buttonPrintImage.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonPrintImage, "Print image");
            this.buttonPrintImage.UseVisualStyleBackColor = true;
            this.buttonPrintImage.Click += new System.EventHandler(this.buttonPrintImage_Click);
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            // 
            // printDialog
            // 
            this.printDialog.UseEXDialog = true;
            // 
            // UserControlPrintImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonPrintImage);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserControlPrintImage";
            this.Size = new System.Drawing.Size(23, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPrintImage;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintDialog printDialog;
    }
}
