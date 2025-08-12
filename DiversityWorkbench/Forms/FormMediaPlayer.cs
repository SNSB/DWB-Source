using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormMediaPlayer : Form
    {
        private FormMediaPlayer()
        {
            InitializeComponent();
        }

        public FormMediaPlayer(string URL)
        {
            InitializeComponent();
            this.axWindowsMediaPlayer.URL = URL;
            try
            {
                if (URL.StartsWith("http:"))
                {
                    System.Uri U = new Uri(URL);
                    System.IO.FileInfo F = new System.IO.FileInfo(U.LocalPath);
                    this.Text = F.Name;// +" - " + this.Text;
                }
                else
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(URL);
                    this.Text = f.Name;// +" - " + this.Text;
                }
            }
            catch { }
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
