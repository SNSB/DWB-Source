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
    public partial class UserControlDateTimePanel : UserControl
    {
        #region Parameter
        private System.Windows.Forms.BindingSource _BindingSource;
        private string _DateTimeColumn;
        private DateTime _DateTimeValue;
        private DateTime DateTimeValue
        {
            get { return _DateTimeValue; }
            set
            {
                _DateTimeValue = value;
                bool enabled = this.dateTimePickerDate.Enabled;
                this.dateTimePickerDate.Enabled = false;
                this.dateTimePickerTime.Enabled = false;
                this.dateTimePickerDate.Value = _DateTimeValue;
                this.dateTimePickerTime.Value = _DateTimeValue;
                this.dateTimePickerTime.Enabled = enabled;
                this.dateTimePickerDate.Enabled = enabled;
                //this.maskedTextBoxDate.Text = _DateTimeValue.ToString("ddMMyyyy");
            }
        }

        public string DateTimeString
        {
            get
            {
                if (this.maskedTextBoxDate.Text == "")
                    return "";
                return DateTimeValue.ToString("yyyy-MM-ddThh:mm:ss");
            }
            set
            {
                try
                {
                    if (value != "")
                        DateTimeValue = System.Xml.XmlConvert.ToDateTime(value, "yyyy-MM-ddThh:mm:ss");
                    else
                        this.maskedTextBoxDate.Text = "";
                }
                catch (Exception)
                { }
            }
        }

        private bool _ReadOnly = false;
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                this.maskedTextBoxDate.ReadOnly = _ReadOnly;
                this.dateTimePickerDate.Enabled = !_ReadOnly;
                this.dateTimePickerTime.Enabled = !_ReadOnly;
            }
        }

        public override Color ForeColor
        {
            get { return this.maskedTextBoxDate.ForeColor; }
            set { this.maskedTextBoxDate.ForeColor = value; }
        }

        public event EventHandler DateTimeChanged;
        #endregion

        #region Construction
        public UserControlDateTimePanel()
        {
            InitializeComponent();
            this.maskedTextBoxDate.Text = "";
            DateTimeValue = DateTime.Now;
            this.dateTimePickerTime.Visible = false;
        }
        #endregion

        #region Public
        public void SetDataBinding(System.Windows.Forms.BindingSource bindingSource, string dateTimeColumn)
        {
            try
            {
                _DateTimeColumn = dateTimeColumn;
                _BindingSource = bindingSource;
                _BindingSource.CurrentItemChanged += _BindingSource_CurrentItemChanged;
                _BindingSource_CurrentItemChanged(null, null);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void _BindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            //this.SuspendLayout();
            //this.Enabled = false;
            if (_BindingSource != null && _BindingSource.Current != null && ((DataRowView)_BindingSource.Current)[_DateTimeColumn] != DBNull.Value)
            {
                DateTime newDateTime = (DateTime)(((DataRowView)_BindingSource.Current).Row[_DateTimeColumn]);
                if (DateTimeValue.ToString() != newDateTime.ToString())
                    DateTimeValue = newDateTime;
                this.maskedTextBoxDate.Text = this.dateTimePickerDate.Value.ToString("ddMMyyyy");
            }
            else
            {
                if (this.maskedTextBoxDate.Text != "")
                    this.maskedTextBoxDate.Text = "";
                DateTimeValue = DateTime.Now;
            }
            //this.Enabled = true;
            //this.ResumeLayout();
        }
        #endregion

        #region Events
        private void maskedTextBoxDate_TextChanged(object sender, EventArgs e)
        {
            if (this.maskedTextBoxDate.Text == "")
            {
                this.dateTimePickerTime.Visible = false;
                if (_BindingSource != null && _BindingSource.Current != null && ((DataRowView)_BindingSource.Current)[_DateTimeColumn] != DBNull.Value)
                {
                    ((DataRowView)_BindingSource.Current)[_DateTimeColumn] = DBNull.Value;
                }
                if (this.Enabled && DateTimeChanged != null)
                    DateTimeChanged(this, new EventArgs());
            }
            else if (this.maskedTextBoxDate.MaskCompleted)
            {
                this.dateTimePickerTime.Visible = true;
                DateTime dt;
                if (DateTime.TryParse(this.maskedTextBoxDate.MaskedTextProvider.ToDisplayString(), out dt))
                {
                    DateTimeValue = new DateTime(dt.Year, dt.Month, dt.Day, this.dateTimePickerTime.Value.Hour, this.dateTimePickerTime.Value.Minute, this.dateTimePickerTime.Value.Second);
                    this.dateTimePickerTime.Visible = true;
                    if (_BindingSource != null && _BindingSource.Current != null)
                    {
                        if (((DataRowView)_BindingSource.Current).Row[_DateTimeColumn] == DBNull.Value)
                        {
                            ((DataRowView)_BindingSource.Current).Row[_DateTimeColumn] = DateTimeValue;
                        }
                        else
                        {
                            dt = (DateTime)(((DataRowView)_BindingSource.Current).Row[_DateTimeColumn]);
                            if (dt.CompareTo(DateTimeValue) != 0)
                            {
                                ((DataRowView)_BindingSource.Current).Row[_DateTimeColumn] = DateTimeValue;
                            }
                        }
                    }
                    if (this.Enabled && DateTimeChanged != null)
                        DateTimeChanged(this, new EventArgs());
                }
            }
        }

        private void dateTimePickerDate_ValueChanged(object sender, EventArgs e)
        {
            if (!_ReadOnly && this.dateTimePickerDate.Enabled)
            {
                string newDate = this.dateTimePickerDate.Value.ToString("ddMMyyyy");
                if (this.maskedTextBoxDate.Text != newDate)
                    this.maskedTextBoxDate.Text = newDate;
            }
        }

        private void dateTimePickerTime_ValueChanged(object sender, EventArgs e)
        {
            if (!_ReadOnly && this.dateTimePickerTime.Enabled)
            {
                maskedTextBoxDate_TextChanged(sender, e);
            }
        }

        private void UserControlDateTimePanel_EnabledChanged(object sender, EventArgs e)
        {
            this.dateTimePickerDate.Enabled = Enabled && !_ReadOnly;
            this.dateTimePickerTime.Enabled = Enabled && !_ReadOnly;
        }
        #endregion
    }
}
