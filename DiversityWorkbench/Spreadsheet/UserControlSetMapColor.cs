using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class UserControlSetMapColor : UserControl
    {

        #region Parameter

        //private System.Drawing.Color _Color;
        //System.Windows.Forms.GroupBox _GroupBox;
        System.Windows.Forms.Panel _Panel;
        private Sheet _Sheet;
        private MapColor _MapColor;

        #endregion

        #region Construction

        public UserControlSetMapColor(System.Windows.Forms.Panel Panel, ref Sheet Sheet, ref MapColor MapColor)
        {
            InitializeComponent();

            this._Panel = Panel;
            this._Sheet = Sheet;
            this._MapColor = MapColor;
            this.initControl();
        }

        #endregion

        private void initControl()
        {
            this.fillColorList();

            this.toolStripComboBoxOperator.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in MapColor.Operators())// this.Operators())
            {
                this.toolStripComboBoxOperator.Items.Add(KV.Key);
                if (KV.Key == this._MapColor.Operator)
                    this.toolStripComboBoxOperator.SelectedItem = KV.Key;
            }
            this.toolStripTextBoxFrom.Text = this._MapColor.LowerValue;
            this.toolStripTextBoxTo.Text = this._MapColor.UpperValue;
            this.toolStripLabelSortingValue.Text = this._MapColor.SortingValue().ToString();
            this.initSortingValue();
        }

        private void fillColorList()
        {
            foreach (System.Collections.Generic.KeyValuePair<System.Windows.Media.Brush, System.Drawing.Color> KV in MapColor.BrushColors())
            {
                ToolStripItem T = this.toolStripDropDownButtonColor.DropDownItems.Add(KV.Value.Name.ToString());
                T.BackColor = KV.Value;
                T.Tag = KV.Key;
            }
            foreach (System.Windows.Forms.ToolStripDropDownItem DD in this.toolStripDropDownButtonColor.DropDownItems)
            {
                DD.DisplayStyle = ToolStripItemDisplayStyle.Image;
                DD.Width = 30;
                DD.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);
                DD.AutoSize = false;
                if (DD.Tag == this._MapColor.Brush)
                    this.toolStripMenuItemColor_Click(DD, null);
            }
            //this.toolStripMenuItemColor_Click(this.toolStripDropDownButtonColor.DropDownItems[0], null);
        }

        private void toolStripMenuItemColor_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem D = (System.Windows.Forms.ToolStripMenuItem)sender;

            this.toolStripDropDownButtonColor.BackColor = D.BackColor;
            this.toolStripDropDownButtonColor.Text = "     ";
            this.toolStripDropDownButtonColor.Tag = D.Tag;
            this._MapColor.Brush = (System.Windows.Media.Brush)D.Tag;
        }

        public System.Windows.Media.Brush Brush() { return this._MapColor.Brush; }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            this.RemoveColorControl();
        }

        public void RemoveColorControl()
        {
            this._Panel.Controls.Remove(this);
            this._Sheet.MapColors().Remove(this._MapColor);
            this.Dispose();
        }

        private void toolStripComboBoxOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._MapColor.Operator = this.toolStripComboBoxOperator.SelectedItem.ToString();
            this.toolStripComboBoxOperator.ToolTipText = MapColor.Operators()[this._MapColor.Operator];// this.Operators()[Operator];
            switch (this._MapColor.Operator)
            {
                case "-":
                    this.toolStripTextBoxFrom.Visible = true;
                    this.toolStripLabelBetween.Text = "-";
                    this.toolStripLabelBetween.Visible = true;
                    this.toolStripTextBoxTo.Visible = true;
                    break;
                case "∅":
                    this.toolStripTextBoxFrom.Visible = false;
                    this.toolStripTextBoxTo.Visible = false;
                    this.toolStripLabelBetween.Visible = true;
                    this.toolStripLabelBetween.Text = "Missing";
                    break;
                default:
                    this.toolStripTextBoxFrom.Visible = true;
                    this.toolStripLabelBetween.Visible = false;
                    this.toolStripTextBoxTo.Visible = false;
                    this._MapColor.UpperValue = "";
                    int i;
                    if (!int.TryParse(this._MapColor.LowerValue, out i) && this._MapColor.Operator != "=")
                        this.toolStripComboBoxOperator.BackColor = System.Drawing.Color.Pink;
                    else
                        this.toolStripComboBoxOperator.BackColor = System.Drawing.SystemColors.Window;
                    break;
            }
            this.initSortingValue();
        }

        private void toolStripTextBoxFrom_TextChanged(object sender, EventArgs e)
        {
            if (this._MapColor.LowerValue != this.toolStripTextBoxFrom.Text)
                this._MapColor.LowerValue = this.toolStripTextBoxFrom.Text;
            bool OK = true;
            if (this._MapColor.UpperValue.Length > 0)
            {
                double Upper = 1.0;
                double Lower = 1.0;
                if (double.TryParse(this._MapColor.LowerValue, out Lower))
                {
                    if (double.TryParse(this._MapColor.UpperValue, out Upper)
                    && Upper < Lower)
                        OK = false;
                }
                else
                {
                    this._MapColor.SetSortingValue(this._MapColor.SortingValue(), this._MapColor.SortingValueFixed());
                    if (this._MapColor.Operator != "=")
                        this.toolStripComboBoxOperator.BackColor = System.Drawing.Color.Pink;
                }
            }
            else
            {
                double Lower = 1.0;
                if (!double.TryParse(this._MapColor.LowerValue, out Lower))
                {
                    this._MapColor.SetSortingValue(this._MapColor.SortingValue(), this._MapColor.SortingValueFixed());
                    if (this._MapColor.Operator != "=")
                        this.toolStripComboBoxOperator.BackColor = System.Drawing.Color.Pink;
                }
            }
            if (OK)
            {
                this.toolStripTextBoxFrom.BackColor = System.Drawing.SystemColors.Window;
                this.toolStripComboBoxOperator.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                this.toolStripTextBoxFrom.BackColor = System.Drawing.Color.Pink;
                //this.toolStripComboBoxOperator.BackColor = System.Drawing.Color.Pink;
            }
            this.initSortingValue();
        }

        private void toolStripTextBoxTo_TextChanged(object sender, EventArgs e)
        {
            if (this._MapColor.UpperValue != this.toolStripTextBoxTo.Text)
                this._MapColor.UpperValue = this.toolStripTextBoxTo.Text;
            bool OK = true;
            if (this._MapColor.UpperValue.Length > 0)
            {
                double Upper = 1.0;
                double Lower = 1.0;
                if (double.TryParse(this._MapColor.UpperValue, out Upper)
                    && double.TryParse(this._MapColor.LowerValue, out Lower)
                    && Upper < Lower)
                    OK = false;
            }
            if (OK)
                this.toolStripTextBoxTo.BackColor = System.Drawing.SystemColors.Window;
            else
                this.toolStripTextBoxTo.BackColor = System.Drawing.Color.Pink;
            this.initSortingValue();
        }

        private void toolStripButtonSortingValue_Click(object sender, EventArgs e)
        {
            int? i = (int)this._MapColor.SortingValue();
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(i, "Sorting value", "Please enter a value for the sorting");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (f.Integer != null)
                {
                    foreach (MapColor MC in this._Sheet.MapColors())
                    {
                        if (MC.SortingValue() == (int)f.Integer)
                        {
                            System.Windows.Forms.MessageBox.Show("The value " + f.Integer.ToString() + " is already used");
                            return;
                        }
                    }
                    this._MapColor.SetSortingValue((int)f.Integer, true);
                    this.initSortingValue();
                }
                else
                {
                    _MapColor.ResetSortingValue();
                    this.initSortingValue();
                }
            }
        }

        private void initSortingValue()
        {
            this.toolStripLabelSortingValue.Text = this._MapColor.SortingValue().ToString();
            if (this._MapColor.SortingValueFixed())
            {
                this.toolStripButtonSortingValue.Visible = false;// true;
                this.toolStripLabelSortingValue.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                this.toolStripButtonSortingValue.Visible = false;// true;// false;
                this.toolStripLabelSortingValue.ForeColor = System.Drawing.SystemColors.ControlDark;
            }
        }

    }
}
