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
    public partial class UserControlHtmlColors : UserControl
    {
        #region Parameter
        // HTML color codes
        private string _TitleCC = "#000080";
        public string TitleCC
        {
            get { return _TitleCC; }
            set
            {
                _TitleCC = value;
                setKeys();
            }
        }

        private string _TextCC = "#000000";
        public string TextCC
        {
            get { return _TextCC; }
            set
            {
                _TextCC = value;
                setKeys();
            }
        }

        private string _LinkCC = "#0000FF";
        public string LinkCC
        {
            get { return _LinkCC; }
            set
            {
                _LinkCC = value;
                setKeys();
            }
        }

        private string _VisitedCC = "#800080";
        public string VisitedCC
        {
            get { return _VisitedCC; }
            set
            {
                _VisitedCC = value;
                setKeys();
            }
        }

        private string _ActiveCC = "#FF00FF";
        public string ActiveCC
        {
            get { return _ActiveCC; }
            set
            {
                _ActiveCC = value;
                setKeys();
            }
        }

        private string _BackCC = "#FFFFFF";
        public string BackCC
        {
            get { return _BackCC; }
            set
            {
                _BackCC = value;
                setKeys();
            }
        }
        #endregion

        #region Construction
        public UserControlHtmlColors()
        {
            InitializeComponent();
            setKeys();
        }
        #endregion

        #region Private
        private void setKeys()
        {
            int r, g, b;

            // Title
            r = Convert.ToInt32(_TitleCC.Substring(1, 2), 16);
            g = Convert.ToInt32(_TitleCC.Substring(3, 2), 16);
            b = Convert.ToInt32(_TitleCC.Substring(5, 2), 16);
            this.buttonTitle.ForeColor = Color.FromArgb(r, g, b);

            // Text
            r = Convert.ToInt32(_TextCC.Substring(1, 2), 16);
            g = Convert.ToInt32(_TextCC.Substring(3, 2), 16);
            b = Convert.ToInt32(_TextCC.Substring(5, 2), 16);
            this.buttonText.ForeColor = Color.FromArgb(r, g, b);

            // Link
            r = Convert.ToInt32(_LinkCC.Substring(1, 2), 16);
            g = Convert.ToInt32(_LinkCC.Substring(3, 2), 16);
            b = Convert.ToInt32(_LinkCC.Substring(5, 2), 16);
            this.buttonLink.ForeColor = Color.FromArgb(r, g, b);

            // Visisted link
            r = Convert.ToInt32(_VisitedCC.Substring(1, 2), 16);
            g = Convert.ToInt32(_VisitedCC.Substring(3, 2), 16);
            b = Convert.ToInt32(_VisitedCC.Substring(5, 2), 16);
            this.buttonVLink.ForeColor = Color.FromArgb(r, g, b);

            // Active Link
            r = Convert.ToInt32(_ActiveCC.Substring(1, 2), 16);
            g = Convert.ToInt32(_ActiveCC.Substring(3, 2), 16);
            b = Convert.ToInt32(_ActiveCC.Substring(5, 2), 16);
            this.buttonALink.ForeColor = Color.FromArgb(r, g, b);

            // Background
            r = Convert.ToInt32(_BackCC.Substring(1, 2), 16);
            g = Convert.ToInt32(_BackCC.Substring(3, 2), 16);
            b = Convert.ToInt32(_BackCC.Substring(5, 2), 16);
            this.buttonBackground.BackColor = Color.FromArgb(r, g, b);
            this.buttonText.BackColor = Color.FromArgb(r, g, b);
            this.buttonLink.BackColor = Color.FromArgb(r, g, b);
            this.buttonVLink.BackColor = Color.FromArgb(r, g, b);
            this.buttonALink.BackColor = Color.FromArgb(r, g, b);
            this.buttonTitle.BackColor = Color.FromArgb(r, g, b);
        }

        private string askColor(Color value)
        {
            this.colorDialog.Color = value;
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
                value = this.colorDialog.Color;
            string result = string.Format("#{0,2:x2}{1,2:x2}{2,2:x2}", value.R, value.G, value.B);
            return result;
        }

        private void buttonTitle_Click(object sender, EventArgs e)
        {
            _TitleCC = askColor(this.buttonTitle.ForeColor);
            setKeys();
        }

        private void buttonText_Click(object sender, EventArgs e)
        {
            _TextCC = askColor(this.buttonText.ForeColor);
            setKeys();
        }

        private void buttonBackground_Click(object sender, EventArgs e)
        {
            _BackCC = askColor(this.buttonBackground.BackColor);
            setKeys();
        }

        private void buttonLink_Click(object sender, EventArgs e)
        {
            _LinkCC = askColor(this.buttonLink.ForeColor);
            setKeys();
        }

        private void buttonVLink_Click(object sender, EventArgs e)
        {
            _VisitedCC = askColor(this.buttonVLink.ForeColor);
            setKeys();
        }

        private void buttonALink_Click(object sender, EventArgs e)
        {
            _ActiveCC = askColor(this.buttonALink.ForeColor);
            setKeys();
        }
        #endregion
    }
}
