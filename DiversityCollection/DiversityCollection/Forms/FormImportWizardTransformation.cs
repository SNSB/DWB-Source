using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormImportWizardTransformation : Form
    {
        DiversityCollection.Import_Column _IC;

        public FormImportWizardTransformation(DiversityCollection.Import_Column IC)
        {
            InitializeComponent();
            this._IC = IC;
            this.initForm();
        }

        private void initForm()
        {
            foreach (string s in this._IC.ValueList)
            {
                System.Data.DataRow R = this.dataSetImportWizard.DataTableTranslation.NewDataTableTranslationRow();
                R[0] = s;
                this.dataSetImportWizard.DataTableTranslation.Rows.Add(R);
            }
        }

        private void FormImportWizardTransformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                this._IC.RegularExpressionPattern = this.textBoxRegex.Text;
                this._IC.RegularExpressionReplacement = this.textBoxReplacement.Text;
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(this.textBoxRegex.Text);
            string Replacement = this.textBoxReplacement.Text;
            //this._IC.RegularExpressionPattern = rx.ToString();
            foreach (System.Data.DataRow R in this.dataSetImportWizard.DataTableTranslation.Rows)
            {
                string Value = R[0].ToString();
                R[1] = rx.Replace(Value, Replacement);
                //R[1] = rx.ToString();
            }

        }
    }
}
