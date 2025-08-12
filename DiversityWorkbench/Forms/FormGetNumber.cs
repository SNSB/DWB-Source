using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetNumber : Form
    {
        #region Constriction
        public FormGetNumber(double? Number, string Title, string Header, string Mask = "")
        {
            InitializeComponent();
            // try to set the mask
            try
            {
                if (Mask.Length > 0)
                    this.maskedTextBox.Mask = Mask;
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (Title.Length > 0)
                this.Text = Title;
            if (Header.Length > 0)
                this.labelHeader.Text = Header;
            if (Number != null) this.maskedTextBox.Text = Number.ToString();
        }
        #endregion

        #region Interface

        public double? Number
        {
            get
            {
                double i = 0.0;
                string Value = this.maskedTextBox.Text.Replace(" ", "");
                if (Value.StartsWith(".")) Value = "0" + Value;
                if (Value.EndsWith(".")) Value = Value.Replace(".", "");
                if (double.TryParse(Value, out i))
                    return i;
                return null;
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

        #endregion

    }
}
