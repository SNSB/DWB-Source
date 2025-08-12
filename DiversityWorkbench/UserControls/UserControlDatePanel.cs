using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlDatePanel : UserControl
    {
        #region Parameter

        private System.Windows.Forms.BindingSource _BindingSource;
        string _ColumnDay;
        string _ColumnMonth;
        string _ColumnYear;
        string _ColumnSupplement;

        #endregion

        #region Construction

        public UserControlDatePanel()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public void setDataBindings(System.Windows.Forms.BindingSource BindingSource, string ColumnDay, string ColumnMonth, string ColumnYear, string ColumnSupplement)
        {
            try
            {
                this._BindingSource = BindingSource;
                this._ColumnDay = ColumnDay;
                this._ColumnMonth = ColumnMonth;
                this._ColumnYear = ColumnYear;
                this._ColumnSupplement = ColumnSupplement;
                this.maskedTextBoxDay.DataBindings.Add(new System.Windows.Forms.Binding("Text", BindingSource, ColumnDay, true));
                this.maskedTextBoxMonth.DataBindings.Add(new System.Windows.Forms.Binding("Text", BindingSource, ColumnMonth, true));
                this.maskedTextBoxYear.DataBindings.Add(new System.Windows.Forms.Binding("Text", BindingSource, ColumnYear, true));
                if (ColumnSupplement.Length > 0)
                    this.textBoxSupplement.DataBindings.Add(new System.Windows.Forms.Binding("Text", BindingSource, ColumnSupplement, true));
                else
                {
                    this.textBoxSupplement.Visible = false;
                    this.labelSupplement.Visible = false;
                    int i = this.Width - this.textBoxSupplement.Width - this.labelSupplement.Width;
                    this.Width = i;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Events

        private void dateTimePickerDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (this.dateTimePickerDate.Value.Date.ToShortDateString() == System.DateTime.Now.ToShortDateString()
                    && (this.maskedTextBoxDay.Text.Length > 0
                    || this.maskedTextBoxMonth.Text.Length > 0
                    || this.maskedTextBoxYear.Text.Length > 0))
                {
                    string DateNew = this.dateTimePickerDate.Value.ToShortDateString();
                    string DateCur = this.maskedTextBoxDay.Text + "." + this.maskedTextBoxMonth.Text + "." + this.maskedTextBoxYear.Text;
                    System.DateTime DNew = new DateTime();
                    System.DateTime DCur = new DateTime();
                    if (System.DateTime.TryParse(DateNew, out DNew) && System.DateTime.TryParse(DateCur, out DCur))
                    {
                        if (DNew != DCur)
                        {
                            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkbenchMessages));
                            string Message = resources.GetString("Do_you_really_want_to_change_the_date") + "\r\n" + DateCur + " -> " + DateNew;
                            if (System.Windows.Forms.MessageBox.Show(Message, resources.GetString("Change_date"), MessageBoxButtons.YesNo) == DialogResult.No)
                                return;
                        }
                    }
                }
                this.maskedTextBoxDay.Text = this.dateTimePickerDate.Value.Day.ToString();
                this.maskedTextBoxMonth.Text = this.dateTimePickerDate.Value.Month.ToString();
                this.maskedTextBoxYear.Text = this.dateTimePickerDate.Value.Year.ToString();
                if (this._BindingSource != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                    R[this._ColumnDay] = this.dateTimePickerDate.Value.Day.ToString();
                    R[this._ColumnMonth] = this.dateTimePickerDate.Value.Month.ToString();
                    R[this._ColumnYear] = this.dateTimePickerDate.Value.Year.ToString();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void maskedTextBoxDay_Validating(object sender, CancelEventArgs e)
        {
            bool OK = true;
            if (this.maskedTextBoxDay.Text.Length == 0 && this._BindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                R.BeginEdit();
                R[this._ColumnDay] = System.DBNull.Value;
                R.EndEdit();
            }
            else
            {
                try
                {
                    if (this.maskedTextBoxDay.Text.Length > 0)
                    {
                        int Day = int.Parse(this.maskedTextBoxDay.Text);
                        if (Day < 1 || Day > 31)
                            OK = false;
                    }
                }
                catch
                {
                    OK = false;
                }
                if (!OK) System.Windows.Forms.MessageBox.Show(this.MessageNoValidDatePart(DatePart.Day), this.MessageNoDatePart(DatePart.Day));
            }
        }

        private void maskedTextBoxMonth_Validating(object sender, CancelEventArgs e)
        {
            bool OK = true;
            if (this.maskedTextBoxMonth.Text.Length == 0)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                R.BeginEdit();
                R[this._ColumnMonth] = System.DBNull.Value;
                R.EndEdit();
            }
            else
            {
                try
                {
                    int Month = int.Parse(this.maskedTextBoxMonth.Text);
                    if (Month < 1 || Month > 12)
                        OK = false;
                }
                catch
                {
                    OK = false;
                }
                if (!OK) System.Windows.Forms.MessageBox.Show(this.MessageNoValidDatePart(DatePart.Month), this.MessageNoDatePart(DatePart.Month));
            }
        }

        private void maskedTextBoxYear_Validating(object sender, CancelEventArgs e)
        {
            bool OK = true;
            if (this.maskedTextBoxYear.Text.Length == 0)
            {
                if (this._BindingSource != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._BindingSource.Current;
                    R.BeginEdit();
                    R[this._ColumnYear] = System.DBNull.Value;
                    R.EndEdit();
                }
            }
            else
            {
                try
                {
                    int Year = int.Parse(this.maskedTextBoxYear.Text);
                    int MaxYear = System.DateTime.Now.Year;
                    if (Year < 0 || Year > MaxYear)
                        OK = false;
                }
                catch
                {
                    OK = false;
                }
                if (!OK) System.Windows.Forms.MessageBox.Show(this.MessageNoValidDatePart(DatePart.Year), this.MessageNoDatePart(DatePart.Year));
            }
        }

        private string MessageNoValidDatePart(DatePart DatePart)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkbenchMessages));
            string Message = resources.GetString("Kein").Substring(0, 1).ToUpper()
                + resources.GetString("Kein").Substring(1) + " ";
            if (DatePart == DatePart.Year) Message += resources.GetString("Gueltiges");
            else Message += resources.GetString("Gueltiger");
            Message += " " + DatePart.ToString() + ". " + resources.GetString("Please_enter_a_correct_value") + " :";
            switch (DatePart)
            {
                case DatePart.Day:
                    Message += "1 - 31";
                    break;
                case DatePart.Month:
                    Message += "1 - 12";
                    break;
                case DatePart.Year:
                    Message += "1500 - " + System.DateTime.Now.Year.ToString();
                    break;
            }
            return Message;
        }

        private string MessageNoDatePart(DatePart DatePart)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkbenchMessages));
            string Message = resources.GetString("Kein").Substring(0, 1).ToUpper()
                 + resources.GetString("Kein").Substring(1) + " ";
            Message += " " + DatePart.ToString();
            return Message;
        }

        private enum DatePart { Day, Month, Year }

        #endregion

    }
}
