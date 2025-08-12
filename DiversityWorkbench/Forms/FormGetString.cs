using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetString : Form
    {
        /// <summary>
        /// Dialog to get a string
        /// </summary>
        /// <param name="Title">The title of the form</param>
        /// <param name="Header">The text of the comment to inform the user</param>
        /// <param name="Original">The original text if available</param>
        public FormGetString(string Title, string Header, string Original, System.Drawing.Image image = null)
        {
            InitializeComponent();
            try
            {
                if (Title.Length > 0) this.Text = Title;
                if (Header.Length > 0) this.labelHeader.Text = Header;
                if (this.labelHeader.Height > 13)
                    this.Height += this.labelHeader.Height - 13;
                if (Original.Length > 0) this.textBoxString.Text = Original;
                if (image != null)
                {
                    System.Drawing.Bitmap bitmap = new Bitmap(image, 16, 16);
                    this.Icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
                    this.ShowIcon = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// The Text entered by the user
        /// </summary>
        public string String { 
            get { return this.textBoxString.Text; }
            set { this.textBoxString.Text = value; }
        }

        private void FormGetString_Shown(object sender, EventArgs e)
        {
            this.textBoxString.Focus();
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

        /// <summary>
        /// if it should be possible that the string contains returns
        /// </summary>
        /// <param name="DoAccept"></param>
        public void AcceptReturns(bool DoAccept)
        {
            this.textBoxString.AcceptsReturn = DoAccept;
            this.textBoxString.Multiline = DoAccept;
        }

        #endregion


    }
}