namespace DiversityWorkbench.CacheDB.ReplaceDB
{
    partial class Replacement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Replacement));
            this.imageListState = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageListState
            // 
            this.imageListState.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListState.ImageStream")));
            this.imageListState.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListState.Images.SetKeyName(0, "Add.ico");
            this.imageListState.Images.SetKeyName(1, "OK.ico");
            this.imageListState.Images.SetKeyName(2, "Conflict3.ico");
            this.imageListState.Images.SetKeyName(3, "Delete.ico");
            this.imageListState.Images.SetKeyName(4, "Update.ico");
            this.imageListState.Images.SetKeyName(5, "Error.ico");

        }

        #endregion

        private System.Windows.Forms.ImageList imageListState;
    }
}
