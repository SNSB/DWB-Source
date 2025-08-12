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
    public partial class FormInfo : Form
    {

        #region Construction

        /// <summary>
        /// Form to display an information
        /// </summary>
        /// <param name="Title">the title of the form</param>
        /// <param name="Info">The displayed information</param>
        public FormInfo(string Title, string Info)
        {
            InitializeComponent();
            this.Text = Title;
            this.textBox.Text = Info;
        }

        #endregion

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
