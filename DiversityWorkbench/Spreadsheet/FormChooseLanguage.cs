using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormChooseLanguage : Form
    {
        #region Parameter

        private string _LanguageCode = "en-US";
        
        #endregion

        #region Construction

        public FormChooseLanguage(string LanguageCode)
        {
            InitializeComponent();
            this._LanguageCode = LanguageCode;
            this.initForm();
        }
        
        #endregion

        #region Form

        private void initForm()
        {
            switch (_LanguageCode)
            {
                case "en-US":
                    this.radioButtonEN.Checked = true;
                    break;
                case "de-DE":
                    this.radioButtonDE.Checked = true;
                    break;
            }
        }

        private void radioButtonDE_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDE.Checked)
            {
                this._LanguageCode = "de-DE";
                this.initForm();
            }
        }

        private void radioButtonEN_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonEN.Checked)
            {
                this._LanguageCode = "en-US";
                this.initForm();
            }
        }
        
        #endregion

        #region Interface

        public string LanguageCode() { return this._LanguageCode; }
        
        #endregion

    }
}
