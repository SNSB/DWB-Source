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
    public partial class UserControlColumnFilter : UserControl
    {
        //DiversityWorkbench.Export.TableColumn _TableColumn;
        private System.Data.DataColumn _DataColumn;

        //public UserControlColumnFilter(DiversityWorkbench.Export.TableColumn T)
        //{
        //    InitializeComponent();
        //    this._TableColumn = T;
        //}

        /*        private enum Operators {\=,'~','≠','<','>','Ø','•'}

            =
        ~
        ≠
        <
        >
        Ø
        •
        */
        public UserControlColumnFilter(System.Data.DataColumn C)
        {
            InitializeComponent();
            this._DataColumn = C;
            this.labelColumn.Text = C.ColumnName;
            this.comboBoxOperator.Items.Clear();
            //this.comboBoxOperator.Items.Add("");
            this.comboBoxOperator.Items.Add("=");
            switch (C.DataType.Name.ToString().ToLower())
            {
                case "string":
                    this.comboBoxOperator.Items.Add("~");
                    this.comboBoxOperator.Items.Add("-");
                    this.comboBoxOperator.Items.Add("|");
                    break;
                case "int16":
                case "int32":
                    this.comboBoxOperator.Items.Add("≠");
                    this.comboBoxOperator.Items.Add(">");
                    this.comboBoxOperator.Items.Add("<");
                    this.comboBoxOperator.Items.Add("-");
                    this.comboBoxOperator.Items.Add("|");
                    break;
                case "boolean":
                    this.comboBoxOperator.Items.Add("≠");
                    break;
                default:
                    break;
            }
            this.comboBoxOperator.Items.Add("•");
            this.comboBoxOperator.Items.Add("Ø");
        }

        private void comboBoxOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxOperator.SelectedItem.ToString() == "•")
                {
                    this.textBoxFilter.Enabled = false;
                    this.textBoxFilter.Text = "present";
                }
                else if (this.comboBoxOperator.SelectedItem.ToString() == "Ø")
                {
                    this.textBoxFilter.Enabled = false;
                    this.textBoxFilter.Text = "missing";
                }
                else
                    this.textBoxFilter.Enabled = true;
                if (this.comboBoxOperator.SelectedItem.ToString() == "-")
                {
                    this.labelBetween.Visible = true;
                    this.textBoxFilterUpper.Visible = true;
                }
                else
                {
                    this.labelBetween.Visible = false;
                    this.textBoxFilterUpper.Visible = false;
                }
                string Operator = this.comboBoxOperator.SelectedItem.ToString();
                switch (Operator)
                {
                    case "=":
                        this.toolTip.SetToolTip(this.comboBoxOperator, DiversityWorkbench.UserControls.UserControlQueryConditionText.exactly_like_the_given_value);
                        break;
                    case ">":
                        this.toolTip.SetToolTip(this.comboBoxOperator, DiversityWorkbench.UserControls.UserControlQueryConditionText.bigger_than_the_given_value);
                        break;
                    case "<":
                        this.toolTip.SetToolTip(this.comboBoxOperator, DiversityWorkbench.UserControls.UserControlQueryConditionText.smaller_than_the_given_value);
                        break;
                    case "~":
                        this.toolTip.SetToolTip(this.comboBoxOperator, DiversityWorkbench.UserControls.UserControlQueryConditionText.similar_to_the_given_value);
                        break;
                    case "≠":
                        this.toolTip.SetToolTip(this.comboBoxOperator, DiversityWorkbench.UserControls.UserControlQueryConditionText.that_are_not_like_the_given_value);
                        break;
                    case "Ø":
                        this.toolTip.SetToolTip(this.comboBoxOperator, DiversityWorkbench.UserControls.UserControlQueryConditionText.where_a_value_is_missing);
                        break;
                    case "•":
                        this.toolTip.SetToolTip(this.comboBoxOperator, DiversityWorkbench.UserControls.UserControlQueryConditionText.where_a_value_is_present);
                        break;
                    case "|":
                    case "-":
                        this.toolTip.SetToolTip(this.comboBoxOperator, DiversityWorkbench.UserControls.UserControlQueryConditionText.within_the_range_of_the_given_values);
                        break;
                    default:
                        break;
                }
                if (this.comboBoxOperator.SelectedItem.ToString().Length > 0)
                    this.comboBoxOperator.BackColor = System.Drawing.SystemColors.Window;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public string Filter()
        {
            if (this.comboBoxOperator.SelectedItem == null && this.textBoxFilter.Text.Length > 0)
                System.Windows.Forms.MessageBox.Show("You missed to choose the operator for column\r\n" + this._DataColumn.ColumnName + "\r\nand the entered value\r\n" + this.textBoxFilter.Text);
            if (this.comboBoxOperator.SelectedItem == null || this.textBoxFilter.Text.Length == 0)
            {
                return "";
            }

            return this.FilterSQL;

            string Operator = this.comboBoxOperator.SelectedItem.ToString();
            string Filter = "";
            switch (Operator)
            {
                case "=":
                case ">":
                case "<":
                    Filter = this._DataColumn.ColumnName + " " + Operator;
                    if (this._DataColumn.DataType.ToString().ToLower() == "string")
                    {
                        Filter += " '" + this.textBoxFilter.Text + "'";
                    }
                    else Filter += " " + this.textBoxFilter.Text;
                    break;
                case "≠":
                    Filter = this._DataColumn.ColumnName;
                    if (this._DataColumn.DataType.ToString().ToLower() == "string")
                    {
                        Filter += " NOT LIKE '" + this.textBoxFilter.Text + "'";
                    }
                    else Filter += " <> " + this.textBoxFilter.Text;
                    break;
                case "~":
                    Filter = this._DataColumn.ColumnName + " LIKE '" + this.textBoxFilter.Text + "'";
                    break;
                case "•":
                    Filter = " NOT " + this._DataColumn.ColumnName + " IS NULL";
                    break;
                case "Ø":
                    Filter = this._DataColumn.ColumnName + " IS NULL";
                    break;
                case "|":
                    string[] Separators = new string[] { "\r\n" };
                    string[] FF = this.textBoxFilter.Text.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < FF.Length; i++)
                    {
                        if (Filter.Length > 0) Filter += ", ";
                        if (this._DataColumn.DataType.ToString().ToLower() == "string")
                        {
                            Filter += " '" + FF[i] + "'";
                        }
                        else Filter += " " + FF[i];
                    }
                    Filter = this._DataColumn.ColumnName + " IN (" + Filter + ")";
                    break;
                case "-":
                    if (this.textBoxFilterUpper.Text.Trim().Length > 0)
                    {
                        Filter = this._DataColumn.ColumnName + " BETWEEN ";
                        if (this._DataColumn.DataType.ToString().ToLower() == "string")
                        {
                            Filter += " '" + this.textBoxFilter.Text + "'";
                        }
                        else Filter += " " + this.textBoxFilter.Text;
                        Filter += " AND ";
                        if (this._DataColumn.DataType.ToString().ToLower() == "string")
                        {
                            Filter += " '" + this.textBoxFilterUpper.Text + "'";
                        }
                        else Filter += " " + this.textBoxFilterUpper.Text;
                    }
                    else if (this.textBoxFilterUpper.Text.Trim().Length == 0 &&
                        this.textBoxFilter.Text.Trim().Length > 0)
                        this.textBoxFilterUpper.BackColor = System.Drawing.Color.Pink;
                    break;
                default:
                    break;
            }
            return Filter;
        }

        public string ColumnName { get => this._DataColumn.ColumnName; }

        public string FilterOperator { get => this.comboBoxOperator.SelectedItem.ToString(); }

        public string FilterValue { get => this.textBoxFilter.Text; }

        public string FilterSQL
        {
            get
            {
                string Filter = "";
                switch (this.FilterOperator)
                {
                    case "=":
                    case ">":
                    case "<":
                        Filter = this._DataColumn.ColumnName + " " + this.FilterOperator;
                        if (this._DataColumn.DataType.ToString().ToLower() == "string")
                        {
                            Filter += " '" + this.FilterValue + "'";
                        }
                        else Filter += " " + this.FilterValue;
                        break;
                    case "≠":
                        Filter = this._DataColumn.ColumnName;
                        if (this._DataColumn.DataType.ToString().ToLower() == "string")
                        {
                            Filter += " NOT LIKE '" + this.FilterValue + "'";
                        }
                        else Filter += " <> " + this.FilterValue;
                        break;
                    case "~":
                        Filter = this._DataColumn.ColumnName + " LIKE '" + this.FilterValue + "'";
                        break;
                    case "•":
                        Filter = " NOT " + this._DataColumn.ColumnName + " IS NULL";
                        break;
                    case "Ø":
                        Filter = this._DataColumn.ColumnName + " IS NULL";
                        break;
                    case "|":
                        string[] Separators = new string[] { "\r\n" };
                        string[] FF = this.textBoxFilter.Text.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < FF.Length; i++)
                        {
                            if (Filter.Length > 0) Filter += ", ";
                            if (this._DataColumn.DataType.ToString().ToLower() == "string")
                            {
                                Filter += " '" + FF[i] + "'";
                            }
                            else Filter += " " + FF[i];
                        }
                        Filter = this._DataColumn.ColumnName + " IN (" + Filter + ")";
                        break;
                    case "-":
                        if (this.textBoxFilterUpper.Text.Trim().Length > 0)
                        {
                            Filter = this._DataColumn.ColumnName + " BETWEEN ";
                            if (this._DataColumn.DataType.ToString().ToLower() == "string")
                            {
                                Filter += " '" + this.FilterValue + "'";
                            }
                            else Filter += " " + this.FilterValue;
                            Filter += " AND ";
                            if (this._DataColumn.DataType.ToString().ToLower() == "string")
                            {
                                Filter += " '" + this.FilterValue + "'";
                            }
                            else Filter += " " + this.textBoxFilterUpper.Text;
                        }
                        else if (this.textBoxFilterUpper.Text.Trim().Length == 0 &&
                            this.textBoxFilter.Text.Trim().Length > 0)
                            this.textBoxFilterUpper.BackColor = System.Drawing.Color.Pink;
                        break;
                    default:
                        break;
                }
                return Filter;
            }
        }

        private void textBoxFilter_DoubleClick(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Edit filter for column " + this._DataColumn.ColumnName, this.textBoxFilter.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxFilter.Text = f.EditedText;
        }

        private void textBoxFilterUpper_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxFilterUpper.Text.Trim().Length == 0)
                this.textBoxFilterUpper.BackColor = System.Drawing.Color.Pink;
            else this.textBoxFilterUpper.BackColor = System.Drawing.Color.White;
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBoxFilter.Text.Length > 0 && (this.comboBoxOperator.SelectedItem == null || (this.comboBoxOperator.SelectedItem != null && this.comboBoxOperator.SelectedItem.ToString().Length == 0)))
                {
                    this.comboBoxOperator.BackColor = System.Drawing.Color.Pink;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
    }
}
