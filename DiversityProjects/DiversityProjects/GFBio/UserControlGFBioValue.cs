using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityProjects.GFBio
{
    public partial class UserControlGFBioValue : UserControl
    {
        public UserControlGFBioValue(GFBioValue Value)
        {
            InitializeComponent();
            try
            {
                this.textBoxColumn.Text = Value.Column;
                if (Value.Setting != null && Value.Setting.Length > 0)
                    this.textBoxSetting.Text = Value.Setting;
                else if (Value.Identifier != null && Value.Identifier.Length > 0)
                {
                    this.textBoxSetting.Text = Value.Identifier;
                    string Error = "";
                    string Message = "";
                    if (!Project.IdentifierTypeAvailable(Value, ref Error, ref Message))
                        this.textBoxValue.BackColor = System.Drawing.Color.Pink;
                }
                this.textBoxTable.Text = Value.Table;
                this.textBoxValue.Text = Value.Value;
                if (this.textBoxValue.Text.Trim().Length == 0 || this.textBoxValue.Text.Trim().ToLower() == "null")
                {
                    this.textBoxValue.BackColor = System.Drawing.Color.White;
                    this.textBoxTable.BackColor = System.Drawing.Color.White;
                    this.textBoxSetting.BackColor = System.Drawing.Color.White;
                    this.textBoxColumn.BackColor = System.Drawing.Color.White;

                    this.textBoxValue.ForeColor = System.Drawing.Color.Gray;
                    this.textBoxTable.ForeColor = System.Drawing.Color.Gray;
                    this.textBoxSetting.ForeColor = System.Drawing.Color.Gray;
                    this.textBoxColumn.ForeColor = System.Drawing.Color.Gray;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void IsHeader()
        {
            this.labelColumn.Visible = true;
            this.labelSetting.Visible = true;
            this.labelTable.Visible = true;
            this.labelValue.Visible = true;
            this.Height = 38;
        }
    }
}
