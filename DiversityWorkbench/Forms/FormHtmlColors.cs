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
    public partial class FormHtmlColors : Form
    {
        #region Parameter
        // HTML color codes
        public string TitleCC
        {
            get { return this.userControlHtmlColors.TitleCC; }
            set { this.userControlHtmlColors.TitleCC = value; }
        }

        public string TextCC
        {
            get { return this.userControlHtmlColors.TextCC; }
            set { this.userControlHtmlColors.TextCC = value; }
        }

        public string LinkCC
        {
            get { return this.userControlHtmlColors.LinkCC; }
            set { this.userControlHtmlColors.LinkCC = value; }
        }

        public string VisitedCC
        {
            get { return this.userControlHtmlColors.VisitedCC; }
            set { this.userControlHtmlColors.VisitedCC = value; }
        }

        public string ActiveCC
        {
            get { return this.userControlHtmlColors.ActiveCC; }
            set { this.userControlHtmlColors.ActiveCC = value; }
        }

        public string BackCC
        {
            get { return this.userControlHtmlColors.BackCC; }
            set { this.userControlHtmlColors.BackCC = value; }
        }
        #endregion

        #region Construction
        public FormHtmlColors()
        {
            InitializeComponent();
        }

        public FormHtmlColors(string titleCC, string textCC, string linkCC, string visitedCC, string activeCC, string backCC)
            :this()
        {
            TitleCC = titleCC;
            TextCC = textCC;
            LinkCC = linkCC;
            VisitedCC = visitedCC;
            ActiveCC = activeCC;
            BackCC = backCC;
        }
        #endregion
    }
}
