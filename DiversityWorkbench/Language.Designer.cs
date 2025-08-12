
namespace DiversityWorkbench
{
    partial class Language
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Language));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Language.ico");
            this.imageList.Images.SetKeyName(1, "China.ico");
            this.imageList.Images.SetKeyName(2, "Deutsch.ico");
            this.imageList.Images.SetKeyName(3, "English.ico");
            this.imageList.Images.SetKeyName(4, "Frankreich.ico");
            this.imageList.Images.SetKeyName(5, "Italien.ico");
            this.imageList.Images.SetKeyName(6, "Latein.ico");
            this.imageList.Images.SetKeyName(7, "Spanien.ico");
            this.imageList.Images.SetKeyName(8, "USA.ico");

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
    }
}
