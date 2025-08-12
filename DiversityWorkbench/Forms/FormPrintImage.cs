using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormPrintImage : Form
    {
        private System.Drawing.Image _Image;

        public FormPrintImage(System.Drawing.Image I)
        {
            InitializeComponent();
            this._Image = I;
            this.pictureBox.Image = this._Image;
        }

        public FormPrintImage(System.Drawing.Image I, int WidthParent, int HeightParent) : this(I)
        {
            this.initForm(WidthParent, HeightParent);
        }

        private void initForm(int WidthParent, int HeightParent)
        {
            DiversityWorkbench.UserControls.UserControlPrintImage U = new UserControls.UserControlPrintImage(this._Image);
            this.Controls.Add(U);
            U.Dock = DockStyle.Bottom;
            this.pictureBox.Dock = DockStyle.Fill;
            this.pictureBox.BringToFront();
            int Width = this._Image.Width + (this.Width - this.pictureBox.Width);
            int Height = this._Image.Height + (this.Height - this.pictureBox.Height);
            if (Width < WidthParent - 10)
                this.Width = Width;
            if (Height < HeightParent - 10)
                this.Height = Height;
        }

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion


    }
}
