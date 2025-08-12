using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetInteger : Form
    {
        #region Construction

        public FormGetInteger(int? Number, string Title, string Header, string Mask = "")
        {
            InitializeComponent();
            if (Title.Length > 0)
                this.Text = Title;
            this.initForm(Header);
            if (Mask.Length > 0)
                this.maskedTextBox.Mask = Mask;
            if (Number != null) this.maskedTextBox.Text = Number.ToString();
        }

        private void initForm(string Header)
        {
            // Markus 14.6.24: Adaption to length of header text
            if (Header.Length > 0)
            {
                this.labelHeader.Text = Header;
                int Lines = Header.Split('\n').Length;
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(104 + Lines * 30);
            }
            else this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(104);
        }
        
        #endregion

        #region Interface

        public int? Integer
        {
            get
            {
                int i = 0;
                if (int.TryParse(this.maskedTextBox.Text, out i))
                    return i;
                return null;
            }
        }

        public void setIcon(System.Drawing.Icon Icon)
        {
            this.Icon = Icon;
            this.ShowIcon = true;
        }

        public void setIcon(System.Drawing.Bitmap bitmapIcon)
        {
            IntPtr Hicon = bitmapIcon.GetHicon();
            this.Icon = Icon.FromHandle(Hicon);
            this.ShowIcon = true;
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Option

        DialogResult _OptionDialogResult = DialogResult.Abort;

        public void setOption(DialogResult dialogResult, string Message, System.Drawing.Image image)
        {
            this._OptionDialogResult = dialogResult;
            this.buttonOption.Text = Message;
            this.buttonOption.Image = image;
            this.buttonOption.Visible = true;
            this.Height += this.buttonOption.Height + 6;
        }

        private void buttonOption_Click(object sender, EventArgs e)
        {
            this.DialogResult = this._OptionDialogResult;
            this.Close();
        }

        #endregion
    }
}