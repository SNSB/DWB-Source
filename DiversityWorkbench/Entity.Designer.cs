namespace DiversityWorkbench
{
    partial class Entity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Entity));
            this.imageListLanguage = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageListLanguage
            // 
            this.imageListLanguage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLanguage.ImageStream")));
            this.imageListLanguage.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLanguage.Images.SetKeyName(0, "USA.ico");
            this.imageListLanguage.Images.SetKeyName(1, "Deutsch.ico");
            this.imageListLanguage.Images.SetKeyName(2, "English.ico");

        }

        #endregion

        public System.Windows.Forms.ImageList imageListLanguage;

    }
}
