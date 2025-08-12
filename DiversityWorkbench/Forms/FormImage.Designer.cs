namespace DiversityWorkbench.Forms
{
    partial class FormImage
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImage));
            this.userControlImage = new DiversityWorkbench.UserControls.UserControlImage();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // userControlImage
            // 
            this.userControlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userControlImage.ImagePath = "";
            this.userControlImage.Location = new System.Drawing.Point(0, 0);
            this.userControlImage.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.userControlImage.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Unknown;
            this.userControlImage.Name = "userControlImage";
            this.userControlImage.Size = new System.Drawing.Size(1056, 746);
            this.userControlImage.TabIndex = 0;
            // 
            // FormImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 746);
            this.Controls.Add(this.userControlImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormImage";
            this.Shown += new System.EventHandler(this.FormImage_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlImage userControlImage;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}