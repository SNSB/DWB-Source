using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormImage : Form
    {
        public FormImage(string URI)
        {
            InitializeComponent();
            this.userControlImage.setMarkingArea(false);
            this.userControlImage.ImagePath = URI;
            string Title = URI;
            try
            {
                System.Uri u = new Uri(URI, UriKind.Absolute);
                System.Uri u2 = new Uri(u, URI);
                Title = u.LocalPath;
                //Title = u.GetComponents(UriComponents.AbsoluteUri, UriFormat.Unescaped);
            }
            catch
            {
                try
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(URI);
                    Title = f.Name;
                }
                catch { }
            }
            this.Text = Title;
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

        //public void setParentForm(System.Windows.Forms.Form form)
        //{
        //    this.Parent = form;
        //}

        #endregion

        #region Events

        private void FormImage_Shown(object sender, EventArgs e)
        {
            this.userControlImage.AdaptImage();
        }

        #endregion
    }
}