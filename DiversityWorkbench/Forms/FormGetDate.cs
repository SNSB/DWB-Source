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
    public partial class FormGetDate : Form
    {
        public FormGetDate(bool ShowTime, bool ForRange = false, bool ShowCheckbox = false, DateTime? Start = null, DateTime? End = null, string Title = "", string Header = "", bool OptionalTime = false)
        {
            InitializeComponent();
            this.dateTimePickerTime.Visible = ShowTime;
            bool HeaderVisible = (!ShowTime && OptionalTime) || (Header.Length > 0);
            if (!ForRange)
            {
                if (HeaderVisible)
                    this.Height = (int)((float)107 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                else
                    this.Height = (int)((float)85 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            }
            else
            {
                if (HeaderVisible)
                    this.Height = (int)((float)130 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                else
                    this.Height = (int)((float)107 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                this.labelEnd.Visible = true;
                this.labelStart.Visible = true;
                this.dateTimePickerDateEnd.Visible = true;
                this.dateTimePickerTimeEnd.Visible = ShowTime;
                this.dateTimePickerDate.ShowCheckBox = ShowCheckbox;
                this.dateTimePickerDateEnd.ShowCheckBox = ShowCheckbox;
            }
            if (Start != null)
            {
                DateTime _start = (DateTime)Start;
                this.dateTimePickerDate.Value = _start;
            }
            else
            {
                this.dateTimePickerDate.Checked = false;
            }
            if (End != null)
            {
                DateTime _end = (DateTime)End;
                this.dateTimePickerDateEnd.Value = _end;
            }
            else
            {
                this.dateTimePickerDateEnd.Checked = false;
            }
            // Text for form
            if (Title.Length > 0)
                this.Text = Title;

            // label with header
            this.labelHeader.Text = Header;
            this.labelHeader.Visible = (Header.Length > 0);
            if (Header.Length > 0)
            {
                int Returns = this.labelHeader.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length;
                this.Height += 13 * (Returns - 1);
            }

            // Showing the button for opt time in
            this.buttonShowTime.Visible = (!ShowTime && OptionalTime);
        }

        public void SetTitle(string Title) { this.Text = Title; }

        public System.DateTime Date
        {
            get
            {
                return this.dateTimePickerDate.Value.Date;
            }
        }

        public bool DateSelected
        {
            get
            {
                if (this.dateTimePickerDate.ShowCheckBox && !this.dateTimePickerDate.Checked)
                    return false;
                else return true;
            }
        }

        public bool DateEndSelected
        {
            get
            {
                if (this.dateTimePickerDateEnd.ShowCheckBox && !this.dateTimePickerDateEnd.Checked)
                    return false;
                else return true;
            }
        }


        public System.DateTime EndDate
        {
            get { return this.dateTimePickerDateEnd.Value.Date; }
        }

        //public System.DateTime Time { get { return this.dateTimePickerTime.Value.ToShortTimeString(); } }

        public System.DateTime DateTime
        {
            get
            {
                System.DateTime DT = this.dateTimePickerDate.Value;
                return DT;
            }
        }

        public System.DateTime EndDateTime
        {
            get
            {
                System.DateTime DT = this.dateTimePickerTimeEnd.Value;
                return DT;
            }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void buttonShowTime_Click(object sender, EventArgs e)
        {
            this.buttonShowTime.Visible = false;
            this.dateTimePickerTime.Visible = true;
            this.dateTimePickerTimeEnd.Visible = this.dateTimePickerDateEnd.Visible;
        }

        //private bool _DateSeleted = false;
        //private bool _DateEndSelected = false;
        //private bool _TimeSelected = false;
        //private bool _TimeEndSelected = false;

        //private void dateTimePickerDate_ValueChanged(object sender, EventArgs e)
        //{
        //    _DateSeleted = true;
        //}

        //private void dateTimePickerTime_ValueChanged(object sender, EventArgs e)
        //{
        //    _TimeSelected = true;
        //}

        //private void dateTimePickerDateEnd_ValueChanged(object sender, EventArgs e)
        //{
        //    _DateEndSelected = true;
        //}

        //private void dateTimePickerTimeEnd_ValueChanged(object sender, EventArgs e)
        //{
        //    _TimeEndSelected = true;
        //}
    }
}
