using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlDuration : UserControl
    {
        #region Parameter
        private int _Year = 0;
        private int _Month = 0;
        private int _Day = 0;
        private int _hour = 0;
        private int _minute = 0;
        private int _second = 0;
        private bool _IsIsoFormat = false;
        private bool _UseIsoFormat = true;
        #endregion

        #region Construction
        public UserControlDuration()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties

        public string DisplayText(string IsoDatePeriod, bool Short = true)
        {
            string Display = "";
            return Display;
        }

        public void setUseIsoFormat(bool UseFormat)
        {
            this._UseIsoFormat = UseFormat;
            this.setControls();
        }

        #endregion

        #region Conversion
        //private string Duration()
        //{
        //    int.TryParse(this.maskedTextBoxMinute.Text, out _minute);
        //    int.TryParse(this.maskedTextBoxMinute.Text, out _minute);
        //    int.TryParse(this.maskedTextBoxMinute.Text, out _minute);
        //    int.TryParse(this.maskedTextBoxMinute.Text, out _minute);
        //    int.TryParse(this.maskedTextBoxMinute.Text, out _minute);
        //    int.TryParse(this.maskedTextBoxSecond.Text, out _second);
        //    if (_IsIsoFormat)
        //    {
        //        string IsoFormat = "P";
        //        if (_Year > 0) IsoFormat += _Year.ToString() + "Y";
        //        if (_Month > 0) IsoFormat += _Month.ToString() + "M";
        //        if (_Day > 0) IsoFormat += _Day.ToString() + "D";
        //        if (_hour > 0 || _minute > 0 || _second > 0) IsoFormat += "T";
        //        if (_hour > 0) IsoFormat += _hour.ToString() + "H";
        //        if (_minute > 0) IsoFormat += _minute.ToString() + "M";
        //        if (_second > 0) IsoFormat += _second.ToString() + "S";
        //        return IsoFormat;
        //    }
        //    else
        //        return this.textBox.Text;
        //}

        //        private bool ConvertToIsoFormat(string Text)
        //        {
        //            bool IsIso = true;
        //            IsIso = Text.StartsWith("P") || Text.Length == 0;
        //            string Time = "";
        //            string Date = "";
        //            if (IsIso)
        //            {
        //                // Splitting Date and Time
        //                if (Text.IndexOf("T") > -1) // Time is given
        //                {
        //                    string[] tt = Text.Split(new char[] { 'T' });
        //                    if (tt.Length == 0)
        //                        return false;
        //                    else if (tt.Length == 1)
        //                    {
        //                        Time = tt[0];
        //                    }
        //                    else if (tt.Length == 2)
        //                    {
        //                        Date = tt[0];
        //                        Time = tt[1];
        //                    }
        //                    else return false;
        //                }
        //                // Parsing Date
        //                if (Date.Length > 0)
        //                {
        //                    IsIso = this.GetPart(DurationPart.Y, ref _Year, ref Date);
        //                    if (!IsIso) return false;
        //                    IsIso = this.GetPart(DurationPart.M, ref _Month, ref Date);
        //                    if (!IsIso) return false;
        //                    IsIso = this.GetPart(DurationPart.D, ref _Day, ref Date);
        //                }
        //                // Parsing Time
        //                if (Time.Length > 0)
        //                {
        //                    IsIso = this.GetPart(DurationPart.h, ref _hour, ref Time);
        //                    if (!IsIso) return false;
        //                    IsIso = this.GetPart(DurationPart.m, ref _minute, ref Time);
        //                    if (!IsIso) return false;
        //                    IsIso = this.GetPart(DurationPart.s, ref _second, ref Time);
        //                }
        //            }
        //#if !DEBUG
        //            return false;
        //#endif
        //            return IsIso;
        //        }

        //private enum DurationPart { Y, M, D, h, m, s }

        //private bool GetPart(DurationPart Part, ref int Value, ref string Text)
        //{
        //    bool OK = true;
        //    try
        //    {
        //        if (Text.IndexOf(Part.ToString().ToUpper()) > -1)
        //        {
        //            string[] tt = Text.Split(new char[] { Part.ToString().ToUpper()[0] });
        //            OK = int.TryParse(tt[0], out Value);
        //            if (OK && tt.Length > 1)
        //                Text = tt[1];
        //            else Text = "";
        //        }
        //    }
        //    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); OK = false; }
        //    return OK;
        //}

        #endregion

        #region Controls

        //public void setControls(string Text)
        //{
        //    this._IsIsoFormat = DiversityWorkbench.Forms.FormFunctions.IsToIsoFormatPeriod(Text, ref _Year, ref _Month, ref _Day, ref _hour, ref _minute, ref _second); // this.ConvertToIsoFormat(Text);
        //    this.setControls();
        //}

        private void setControls()
        {
            _Year = 0;
            _Month = 0;
            _Day = 0;
            _hour = 0;
            _minute = 0;
            _second = 0;
            this.setMaskedTextBoxValue(this.maskedTextBoxYear, _Year);
            this.setMaskedTextBoxValue(this.maskedTextBoxMonth, _Month);
            this.setMaskedTextBoxValue(this.maskedTextBoxDay, _Day);
            this.setMaskedTextBoxValue(this.maskedTextBoxHour, _hour);
            this.setMaskedTextBoxValue(this.maskedTextBoxMinute, _minute);
            this.setMaskedTextBoxValue(this.maskedTextBoxSecond, _second);

            if (_UseIsoFormat)
                _IsIsoFormat = DiversityWorkbench.Forms.FormFunctions.IsIsoFormatPeriod(this.textBox.Text, ref _Year, ref _Month, ref _Day, ref _hour, ref _minute, ref _second);
            else
                _IsIsoFormat = false;
            this.maskedTextBoxYear.Visible = this._IsIsoFormat;
            this.maskedTextBoxMonth.Visible = this._IsIsoFormat;
            this.maskedTextBoxDay.Visible = this._IsIsoFormat;
            this.maskedTextBoxHour.Visible = this._IsIsoFormat;
            this.maskedTextBoxMinute.Visible = this._IsIsoFormat;
            this.maskedTextBoxSecond.Visible = this._IsIsoFormat;
            this.labelSeparator.Visible = this._IsIsoFormat;
            if (_IsIsoFormat) { this.textBox.Dock = DockStyle.Bottom; this.textBox.Margin = new Padding(0, 10, 0, 0); }
            else { this.textBox.Dock = DockStyle.Top; this.textBox.Margin = new Padding(0); }
            if (_IsIsoFormat)
            {
                this.setMaskedTextBoxValue(this.maskedTextBoxYear, _Year);
                this.setMaskedTextBoxValue(this.maskedTextBoxMonth, _Month);
                this.setMaskedTextBoxValue(this.maskedTextBoxDay, _Day);
                this.setMaskedTextBoxValue(this.maskedTextBoxHour, _hour);
                this.setMaskedTextBoxValue(this.maskedTextBoxMinute, _minute);
                this.setMaskedTextBoxValue(this.maskedTextBoxSecond, _second);
            }
        }

        private void setMaskedTextBoxValue(System.Windows.Forms.MaskedTextBox textBox, int Value)
        {
            if (Value > 0)
                textBox.Text = Value.ToString();
            else textBox.Text = "";
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            this.setControls();
            //if (this.textBox.DataBindings.Count > 0)
            //{
            //    System.Windows.Forms.Binding binding = this.textBox.DataBindings[0];
            //    if (binding.DataSource.GetType() == typeof()
            //}
        }


        private void maskedTextBoxYear_Leave(object sender, EventArgs e)
        {
            int.TryParse(this.maskedTextBoxYear.Text, out _Year);
            this.textBox.Text = DiversityWorkbench.Forms.FormFunctions.IsoFormatPeriod(_Year, _Month, _Day, _hour, _minute, _second);// this.Duration();
        }

        private void maskedTextBoxMonth_Leave(object sender, EventArgs e)
        {
            int.TryParse(this.maskedTextBoxMonth.Text, out _Month);
            this.textBox.Text = DiversityWorkbench.Forms.FormFunctions.IsoFormatPeriod(_Year, _Month, _Day, _hour, _minute, _second);// this.Duration();
        }

        private void maskedTextBoxDay_Leave(object sender, EventArgs e)
        {
            int.TryParse(this.maskedTextBoxDay.Text, out _Day);
            this.textBox.Text = DiversityWorkbench.Forms.FormFunctions.IsoFormatPeriod(_Year, _Month, _Day, _hour, _minute, _second);// this.Duration();
        }

        private void maskedTextBoxHour_Leave(object sender, EventArgs e)
        {
            int.TryParse(this.maskedTextBoxHour.Text, out _hour);
            this.textBox.Text = DiversityWorkbench.Forms.FormFunctions.IsoFormatPeriod(_Year, _Month, _Day, _hour, _minute, _second);// this.Duration();
        }

        private void maskedTextBoxMinute_Leave(object sender, EventArgs e)
        {
            int.TryParse(this.maskedTextBoxMinute.Text, out _minute);
            this.textBox.Text = DiversityWorkbench.Forms.FormFunctions.IsoFormatPeriod(_Year, _Month, _Day, _hour, _minute, _second);// this.Duration();
        }
        private void maskedTextBoxSecond_Leave(object sender, EventArgs e)
        {
            int.TryParse(this.maskedTextBoxSecond.Text, out _second);
            this.textBox.Text = DiversityWorkbench.Forms.FormFunctions.IsoFormatPeriod(_Year, _Month, _Day, _hour, _minute, _second);// this.Duration();
        }

        #endregion

    }
}
