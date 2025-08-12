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
    public partial class UserControlWebserviceOption : UserControl
    {
        private DiversityWorkbench.WebserviceQueryOption _Option;

        public UserControlWebserviceOption(DiversityWorkbench.WebserviceQueryOption Option)
        {
            InitializeComponent();
            this._Option = Option;
            this.labelOption.Text = Option.Name();
            this.checkBoxOption.Visible = false;
            this.textBoxOption.Visible = false;
            foreach (string s in Option.OptionValues())
                this.comboBoxOption.Items.Add(s);
            if (Option.Description != null)
            {
                this.labelDescription.Text = Option.Description;
                this.toolTip.SetToolTip(this.labelDescription, Option.Description);
                this.buttonShowDescription.Visible = true;
            }
            else
            {
                this.buttonShowDescription.Visible = false;
                this.labelDescription.Text = "";
            }
        }

        public string OptionValue()
        {
            if (this._Option.Value != null)
                return this._Option.Value;
            else return "";
        }

        public string OptionName()
        {
            return this._Option.Name();
        }

        private void comboBoxOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            this._Option.Value = this.comboBoxOption.SelectedItem.ToString();
        }

        private void buttonShowDescription_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Description", this._Option.Description, true);
            f.ShowDialog();
        }
    }
}
