using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlPrintImage : UserControl
    {
        #region Parameter
        Image _Image;

        public Image Image
        {
            get { return _Image; }
            set { _Image = value; }
        }
        #endregion

        #region Construction
        public UserControlPrintImage()
        {
            InitializeComponent();

            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
        }

        public UserControlPrintImage(Image image)
            : this()
        {
            Image = image;
        }
        #endregion

        #region Events
        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (Image != null)
            {
                float x = e.PageSettings.PrintableArea.X;
                float y = e.PageSettings.PrintableArea.Y;
                float pWidth = e.PageSettings.Landscape ? e.PageSettings.PrintableArea.Height : e.PageSettings.PrintableArea.Width;
                float pHeight = e.PageSettings.Landscape ? e.PageSettings.PrintableArea.Width : e.PageSettings.PrintableArea.Height;
                float wScale = pWidth / Image.Width;
                float hScale = pHeight / Image.Height;
                float factor = wScale < hScale ? wScale : hScale;
                float iWidth = factor * Image.Width - x;
                float iHeight = factor * Image.Height - y;
                x = (pWidth - iWidth) / 2;
                y = (pHeight - iHeight) / 2;
                e.Graphics.DrawImage(Image, x, y, iWidth, iHeight);
            }
        }

        private void buttonPrintImage_Click(object sender, EventArgs e)
        {
            this.printDialog.Document = this.printDocument;
            if (this.printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.printDocument.Print();
            }
        }
        #endregion
    }
}
