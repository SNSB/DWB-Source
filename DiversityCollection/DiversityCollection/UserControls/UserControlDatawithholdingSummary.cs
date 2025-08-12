using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControlDatawithholdingSummary : UserControl
    {
        private string _Display;

        public UserControlDatawithholdingSummary()
        {
            InitializeComponent();
        }

        //public void initUserControl(System.Drawing.Image Image, string Display, int NumberToPublish, int NumberBlocked)
        //{
        //    this.pictureBoxSummary.Image = Image;
        //    this.labelSummaryToPublish.Text = "Number of published " + Display + ": " + NumberToPublish.ToString();
        //    this.labelSummaryBlocked.Text = "Number of blocked " + Display + ": " + NumberBlocked.ToString();
        //}

        public void initUserControl(int NumberToPublish, int NumberBlocked, string Display)
        {
            this._Display = Display;
            string DisplayText = "-";
            if (NumberToPublish > 0)
                DisplayText = NumberToPublish.ToString() + " " + this._Display + " published";
            this.labelSummaryToPublish.Text = DisplayText;
            DisplayText = "-";
            if (NumberBlocked > 0)
                DisplayText = NumberBlocked.ToString() + " " + this._Display + " blocked";
            this.labelSummaryBlocked.Text = DisplayText;
            //this.labelSummaryToPublish.Text = "Number of published " + this._Display + ": " + NumberToPublish.ToString();
            //this.labelSummaryBlocked.Text = "Number of blocked " + this._Display + ": " + NumberBlocked.ToString();
        }

        public void initUserControl(System.Drawing.Image Image)
        {
            this.pictureBoxSummary.Image = Image;
        }
    
    }
}
