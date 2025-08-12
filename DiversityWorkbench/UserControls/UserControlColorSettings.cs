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
    public partial class UserControlColorSettings : UserControl
    {
        #region Construction

        public UserControlColorSettings()
        {
            InitializeComponent();
        }

        #endregion

        #region Setting the colors

        private void buttonReadOnly_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void buttonInapplicable_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void buttonServiceLink_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void buttonCalculated_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void buttonOptional_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void buttonHidden_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void buttonNoRestrictions_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void buttonUserDefined_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void buttonVisible_Click(object sender, EventArgs e)
        {
            this.setColor((System.Windows.Forms.Button)sender);
        }

        private void setColor(System.Windows.Forms.Button Button)
        {
            this.colorDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            this.colorDialog.AllowFullOpen = true;
            // Allows the user to get help. (The default is false.)
            this.colorDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            this.colorDialog.Color = Button.BackColor;

            // Update the text box color if the user clicks OK 
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
                Button.BackColor = this.colorDialog.Color;
        }

        #endregion

        #region Interface

        public System.Drawing.Color ColorReadOnly
        {
            get
            {
                return this.buttonReadOnly.BackColor;
            }
        }

        public System.Drawing.Color ColorInapplicable
        {
            get
            {
                return this.buttonInapplicable.BackColor;
            }
        }

        //public System.Drawing.Color ColorVisible
        //{
        //    get
        //    {
        //        return this.buttonVisible.BackColor;
        //    }
        //}

        //public System.Drawing.Color ColorUserDefined
        //{
        //    get
        //    {
        //        return this.buttonUserDefined.BackColor;
        //    }
        //}

        //public System.Drawing.Color ColorNoRestrictions
        //{
        //    get
        //    {
        //        return this.buttonNoRestrictions.BackColor;
        //    }
        //}

        //public System.Drawing.Color ColorHidden
        //{
        //    get
        //    {
        //        return this.buttonHidden.BackColor;
        //    }
        //}

        //public System.Drawing.Color ColorOptional
        //{
        //    get
        //    {
        //        return this.buttonOptional.BackColor;
        //    }
        //}

        public System.Drawing.Color ColorCalculated
        {
            get
            {
                return this.buttonCalculated.BackColor;
            }
        }

        public System.Drawing.Color ColorServiceLink
        {
            get
            {
                return this.buttonServiceLink.BackColor;
            }
        }

        public bool HideReadOnly { get { return !this.checkBoxHideReadOnly.Checked; } }

        public bool HideInapplicable { get { return !this.checkBoxHideInapplicable.Checked; } }

        //public bool HideHidden { get { return !this.checkBoxHideHidden.Checked; } }

        //public bool HideOptional { get { return !this.checkBoxHideOptional.Checked; } }

        public bool HideCalculated { get { return !this.checkBoxHideCalculated.Checked; } }

        public bool HideServiceLink { get { return !this.checkBoxHideServiceLink.Checked; } }

        public string ColorCode(string Usage)
        {
            string ColorCode = "";
            if (Usage.IndexOf(" ") > -1)
                Usage = Usage.Replace(' ', '_');
            try
            {
                switch (Usage.ToLower())
                {
                    case "inapplicable":
                        ColorCode = "#" + this.buttonInapplicable.BackColor.R.ToString("X2") +
                            this.buttonInapplicable.BackColor.G.ToString("X2") +
                            this.buttonInapplicable.BackColor.B.ToString("X2");
                        break;
                    case "read_only":
                        ColorCode = "#" + this.buttonReadOnly.BackColor.R.ToString("X2") +
                            this.buttonReadOnly.BackColor.G.ToString("X2") +
                            this.buttonReadOnly.BackColor.B.ToString("X2");
                        break;
                    case "calculated":
                        ColorCode = "#" + this.buttonCalculated.BackColor.R.ToString("X2") +
                            this.buttonCalculated.BackColor.G.ToString("X2") +
                            this.buttonCalculated.BackColor.B.ToString("X2");
                        break;
                    //case "hidden":
                    //    ColorCode = "#" + this.buttonHidden.BackColor.R.ToString("X2") +
                    //        this.buttonHidden.BackColor.G.ToString("X2") +
                    //        this.buttonHidden.BackColor.B.ToString("X2");
                    //    break;
                    //case "no_restrictions":
                    //    ColorCode = "#" + this.buttonNoRestrictions.BackColor.R.ToString("X2") +
                    //        this.buttonNoRestrictions.BackColor.G.ToString("X2") +
                    //        this.buttonNoRestrictions.BackColor.B.ToString("X2");
                    //    break;
                    //case "optional":
                    //    ColorCode = "#" + this.buttonOptional.BackColor.R.ToString("X2") +
                    //        this.buttonOptional.BackColor.G.ToString("X2") +
                    //        this.buttonOptional.BackColor.B.ToString("X2");
                    //    break;
                    case "service_link":
                        ColorCode = "#" + this.buttonServiceLink.BackColor.R.ToString("X2") +
                            this.buttonServiceLink.BackColor.G.ToString("X2") +
                            this.buttonServiceLink.BackColor.B.ToString("X2");
                        break;
                        //case "user_defined":
                        //    ColorCode = "#" + this.buttonUserDefined.BackColor.R.ToString("X2") +
                        //        this.buttonUserDefined.BackColor.G.ToString("X2") +
                        //        this.buttonUserDefined.BackColor.B.ToString("X2");
                        //    break;
                        //case "visible":
                        //    ColorCode = "#" + this.buttonVisible.BackColor.R.ToString("X2") +
                        //        this.buttonVisible.BackColor.G.ToString("X2") +
                        //        this.buttonVisible.BackColor.B.ToString("X2");
                        //    break;
                }

            }
            catch { }
            return ColorCode;
        }

        public string ColorCode(DiversityWorkbench.Entity.Accessibility Accessibility,
            DiversityWorkbench.Entity.Determination Determination,
            DiversityWorkbench.Entity.Visibility Visibility)
        {
            string ColorCode = "";
            if (Accessibility == DiversityWorkbench.Entity.Accessibility.inapplicable)
            {
                ColorCode = "#" + this.buttonInapplicable.BackColor.R.ToString("X2") +
                    this.buttonInapplicable.BackColor.G.ToString("X2") +
                    this.buttonInapplicable.BackColor.B.ToString("X2");
                return ColorCode;
            }
            else if (Accessibility == DiversityWorkbench.Entity.Accessibility.read_only)
            {
                int iRed = this.buttonReadOnly.BackColor.R;// / 2 + this.buttonVisible.BackColor.R / 2;
                int iGreen = this.buttonReadOnly.BackColor.G;// / 2 + this.buttonVisible.BackColor.R / 2;
                int iBlue = this.buttonReadOnly.BackColor.B;// / 2 + this.buttonVisible.BackColor.R / 2;

                //if (Visibility == DiversityWorkbench.Entity.Visibility.hidden)
                //{
                //    iRed = this.buttonReadOnly.BackColor.R / 2 + this.buttonHidden.BackColor.R / 2;
                //    iGreen = this.buttonReadOnly.BackColor.G / 2 + this.buttonHidden.BackColor.G / 2;
                //    iBlue = this.buttonReadOnly.BackColor.B / 2 + this.buttonHidden.BackColor.B / 2;

                //    //ColorCode = "#" + int.Parse(((this.buttonReadOnly.BackColor.R + this.buttonHidden.BackColor.R)/2).ToString("X2")) +
                //    //    this.buttonInapplicable.BackColor.G.ToString("X2") +
                //    //    this.buttonInapplicable.BackColor.B.ToString("X2");
                //    //return ColorCode;
                //}
                //else if (Visibility == DiversityWorkbench.Entity.Visibility.optional)
                //{
                //    iRed = this.buttonReadOnly.BackColor.R / 2 + this.buttonOptional.BackColor.R / 2;
                //    iGreen = this.buttonReadOnly.BackColor.G / 2 + this.buttonOptional.BackColor.G / 2;
                //    iBlue = this.buttonReadOnly.BackColor.B / 2 + this.buttonOptional.BackColor.B / 2;

                //    //ColorCode = "#" + int.Parse(((this.buttonReadOnly.BackColor.R + this.buttonOptional.BackColor.R) / 2).ToString("X2")) +
                //    //    this.buttonInapplicable.BackColor.G.ToString("X2") +
                //    //    this.buttonInapplicable.BackColor.B.ToString("X2");
                //    //return ColorCode;
                //}
                //else
                //{
                //    iRed = this.buttonReadOnly.BackColor.R/2 + this.buttonVisible.BackColor.R/2;
                //    iGreen = this.buttonReadOnly.BackColor.G/2 + this.buttonVisible.BackColor.G/2;
                //    iBlue = this.buttonReadOnly.BackColor.B/2 + this.buttonVisible.BackColor.B/2;

                ColorCode = "#" + iRed.ToString("X2") +
                    iGreen.ToString("X2") +
                    iBlue.ToString("X2");
                return ColorCode;
                //}
            }
            else
            {
                int iRed = 255;// this.buttonUserDefined.BackColor.R;
                int iGreen = 255;//this.buttonUserDefined.BackColor.G;
                int iBlue = 255;//this.buttonUserDefined.BackColor.B;
                switch (Determination)
                {
                    //case DiversityWorkbench.Entity.Determination.user_defined:
                    //    iRed = this.buttonUserDefined.BackColor.R;
                    //    iGreen = this.buttonUserDefined.BackColor.G;
                    //    iBlue = this.buttonUserDefined.BackColor.B;
                    ////if (Visibility == DiversityWorkbench.Entity.Visibility.hidden)
                    ////{
                    ////    iRed = this.buttonHidden.BackColor.R;
                    ////    iGreen = this.buttonHidden.BackColor.G;
                    ////    iBlue = this.buttonHidden.BackColor.B;
                    ////}
                    ////else if (Visibility == DiversityWorkbench.Entity.Visibility.optional)
                    ////{
                    ////    iRed = this.buttonOptional.BackColor.R;
                    ////    iGreen = this.buttonOptional.BackColor.G;
                    ////    iBlue = this.buttonOptional.BackColor.B;
                    ////}
                    //break;
                    case DiversityWorkbench.Entity.Determination.service_link:
                        iRed = this.buttonServiceLink.BackColor.R;
                        iGreen = this.buttonServiceLink.BackColor.G;
                        iBlue = this.buttonServiceLink.BackColor.B;
                        //if (Visibility == DiversityWorkbench.Entity.Visibility.hidden)
                        //{
                        //    iRed = this.buttonServiceLink.BackColor.R / 2 + this.buttonHidden.BackColor.R / 2;
                        //    iGreen = this.buttonServiceLink.BackColor.G / 2 + this.buttonHidden.BackColor.G / 2;
                        //    iBlue = this.buttonServiceLink.BackColor.B / 2 + this.buttonHidden.BackColor.B / 2;
                        //}
                        //else if (Visibility == DiversityWorkbench.Entity.Visibility.optional)
                        //{
                        //    iRed = this.buttonServiceLink.BackColor.R / 2 + this.buttonOptional.BackColor.R / 2;
                        //    iGreen = this.buttonServiceLink.BackColor.G / 2 + this.buttonOptional.BackColor.G / 2;
                        //    iBlue = this.buttonServiceLink.BackColor.B / 2 + this.buttonOptional.BackColor.B / 2;
                        //}
                        break;
                    case DiversityWorkbench.Entity.Determination.calculated:
                        iRed = this.buttonCalculated.BackColor.R;
                        iGreen = this.buttonCalculated.BackColor.G;
                        iBlue = this.buttonCalculated.BackColor.B;
                        //if (Visibility == DiversityWorkbench.Entity.Visibility.hidden)
                        //{
                        //    iRed = this.buttonCalculated.BackColor.R / 2 + this.buttonHidden.BackColor.R / 2;
                        //    iGreen = this.buttonCalculated.BackColor.G / 2 + this.buttonHidden.BackColor.G / 2;
                        //    iBlue = this.buttonCalculated.BackColor.B / 2 + this.buttonHidden.BackColor.B / 2;
                        //}
                        //else if (Visibility == DiversityWorkbench.Entity.Visibility.optional)
                        //{
                        //    iRed = this.buttonCalculated.BackColor.R / 2 + this.buttonOptional.BackColor.R / 2;
                        //    iGreen = this.buttonCalculated.BackColor.G / 2 + this.buttonOptional.BackColor.G / 2;
                        //    iBlue = this.buttonCalculated.BackColor.B / 2 + this.buttonOptional.BackColor.B / 2;
                        //}
                        break;
                }
                ColorCode = "#" + iRed.ToString("X2") +
                    iGreen.ToString("X2") +
                    iBlue.ToString("X2");
                return ColorCode;


            }
            //if (Usage.IndexOf(" ") > -1)
            //    Usage = Usage.Replace(' ', '_');
            try
            {
                //switch (Usage.ToLower())
                //{
                //    case "inapplicable":
                //        ColorCode = "#" + this.buttonInapplicable.BackColor.R.ToString("X2") +
                //            this.buttonInapplicable.BackColor.G.ToString("X2") +
                //            this.buttonInapplicable.BackColor.B.ToString("X2");
                //        break;
                //    case "read_only":
                //        ColorCode = "#" + this.buttonReadOnly.BackColor.R.ToString("X2") +
                //            this.buttonReadOnly.BackColor.G.ToString("X2") +
                //            this.buttonReadOnly.BackColor.B.ToString("X2");
                //        break;
                //    case "calculated":
                //        ColorCode = "#" + this.buttonCalculated.BackColor.R.ToString("X2") +
                //            this.buttonCalculated.BackColor.G.ToString("X2") +
                //            this.buttonCalculated.BackColor.B.ToString("X2");
                //        break;
                //    case "hidden":
                //        ColorCode = "#" + this.buttonHidden.BackColor.R.ToString("X2") +
                //            this.buttonHidden.BackColor.G.ToString("X2") +
                //            this.buttonHidden.BackColor.B.ToString("X2");
                //        break;
                //    case "no_restrictions":
                //        ColorCode = "#" + this.buttonNoRestrictions.BackColor.R.ToString("X2") +
                //            this.buttonNoRestrictions.BackColor.G.ToString("X2") +
                //            this.buttonNoRestrictions.BackColor.B.ToString("X2");
                //        break;
                //    case "optional":
                //        ColorCode = "#" + this.buttonOptional.BackColor.R.ToString("X2") +
                //            this.buttonOptional.BackColor.G.ToString("X2") +
                //            this.buttonOptional.BackColor.B.ToString("X2");
                //        break;
                //    case "service_link":
                //        ColorCode = "#" + this.buttonServiceLink.BackColor.R.ToString("X2") +
                //            this.buttonServiceLink.BackColor.G.ToString("X2") +
                //            this.buttonServiceLink.BackColor.B.ToString("X2");
                //        break;
                //    case "user_defined":
                //        ColorCode = "#" + this.buttonUserDefined.BackColor.R.ToString("X2") +
                //            this.buttonUserDefined.BackColor.G.ToString("X2") +
                //            this.buttonUserDefined.BackColor.B.ToString("X2");
                //        break;
                //    case "visible":
                //        ColorCode = "#" + this.buttonVisible.BackColor.R.ToString("X2") +
                //            this.buttonVisible.BackColor.G.ToString("X2") +
                //            this.buttonVisible.BackColor.B.ToString("X2");
                //        break;
                //}

            }
            catch { }
            return ColorCode;
        }

        public string ColorCode(DiversityWorkbench.Entity.Accessibility Accessibility,
            DiversityWorkbench.Entity.Determination Determination)
        {
            string ColorCode = "";
            if (Accessibility == DiversityWorkbench.Entity.Accessibility.inapplicable)
            {
                ColorCode = "#" + this.buttonInapplicable.BackColor.R.ToString("X2") +
                    this.buttonInapplicable.BackColor.G.ToString("X2") +
                    this.buttonInapplicable.BackColor.B.ToString("X2");
                return ColorCode;
            }
            else if (Accessibility == DiversityWorkbench.Entity.Accessibility.read_only)
            {
                int iRed = this.buttonReadOnly.BackColor.R;
                int iGreen = this.buttonReadOnly.BackColor.G;
                int iBlue = this.buttonReadOnly.BackColor.B;

                ColorCode = "#" + iRed.ToString("X2") +
                    iGreen.ToString("X2") +
                    iBlue.ToString("X2");
                return ColorCode;
            }
            else
            {
                int iRed = 255;
                int iGreen = 255;
                int iBlue = 255;
                switch (Determination)
                {
                    case DiversityWorkbench.Entity.Determination.service_link:
                        iRed = this.buttonServiceLink.BackColor.R;
                        iGreen = this.buttonServiceLink.BackColor.G;
                        iBlue = this.buttonServiceLink.BackColor.B;
                        break;
                    case DiversityWorkbench.Entity.Determination.calculated:
                        iRed = this.buttonCalculated.BackColor.R;
                        iGreen = this.buttonCalculated.BackColor.G;
                        iBlue = this.buttonCalculated.BackColor.B;
                        break;
                }
                ColorCode = "#" + iRed.ToString("X2") +
                    iGreen.ToString("X2") +
                    iBlue.ToString("X2");
                return ColorCode;
            }
            return ColorCode;
        }

        public string ColorCodeText(string Usage)
        {
            if (Usage == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
            {
                string ColorCode = "";
                ColorCode = "#" + this.buttonInapplicable.ForeColor.R.ToString("X2") +
                    this.buttonInapplicable.ForeColor.G.ToString("X2") +
                    this.buttonInapplicable.ForeColor.B.ToString("X2");
                return ColorCode;
            }
            else return "#000000";
        }

        #endregion

    }
}
