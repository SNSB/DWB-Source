namespace DiversityWorkbench.Spreadsheet
{
    partial class MapSymbols
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSymbols));
            this.imageListSymbol = new System.Windows.Forms.ImageList(this.components);
            // 
            // imageListSymbol
            // 
            this.imageListSymbol.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSymbol.ImageStream")));
            this.imageListSymbol.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSymbol.Images.SetKeyName(0, "Circle.ico");
            this.imageListSymbol.Images.SetKeyName(1, "CircleFilled.ico");
            this.imageListSymbol.Images.SetKeyName(2, "Diamond.ico");
            this.imageListSymbol.Images.SetKeyName(3, "DiamondFilled.ico");
            this.imageListSymbol.Images.SetKeyName(4, "Square.ico");
            this.imageListSymbol.Images.SetKeyName(5, "SquareFilled.ico");
            this.imageListSymbol.Images.SetKeyName(6, "Dreieck.ico");
            this.imageListSymbol.Images.SetKeyName(7, "DreieckVoll.ico");
            this.imageListSymbol.Images.SetKeyName(8, "Pyramide.ico");
            this.imageListSymbol.Images.SetKeyName(9, "PyramideVoll.ico");
            this.imageListSymbol.Images.SetKeyName(10, "Cross.ico");
            this.imageListSymbol.Images.SetKeyName(11, "X.ico");
            this.imageListSymbol.Images.SetKeyName(12, "Minus.ico");
            this.imageListSymbol.Images.SetKeyName(13, "Fragezeichen.ico");
            this.imageListSymbol.Images.SetKeyName(14, "Delete.ico");
            this.imageListSymbol.Images.SetKeyName(15, "KreisPunkt.ico");
            this.imageListSymbol.Images.SetKeyName(16, "QuadratKlein.ico");

        }

        #endregion

        private System.Windows.Forms.ImageList imageListSymbol;
    }
}
