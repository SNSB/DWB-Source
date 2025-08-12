using DiversityCollection.Tasks.Taxa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class FormIPM_Details : Form
    {
        private TaxonRecord _record;
        private RecordData _recordData;
        public FormIPM_Details(TaxonRecord record, System.Drawing.Image image, RecordData recordData, string Where, string date)
        {
            InitializeComponent();
            this.labelWhenContent.Text = date;
            this.labelWhereContent.Text = Where;
            this.pictureBoxHeader.Image = image;
            _record = record;
            _recordData = recordData;
            this.initForm();
        }

        private void initForm()
        {
            this.labelHeader.Text = _record.DisplayText;
            this.textBoxNumberValue.Text = _recordData.Count.ToString();
            this.textBoxNotes.Text = _recordData.Notes;
            this.comboBoxResult.Visible = _record.States.Count > 0;
            this.labelResult.Visible = _record.States.Count > 0;
            if (_record.States.Count > 0 )
            {
                 foreach(string state in _record.States)
                {
                    this.comboBoxResult.Items.Add(state);
                }
            }
            if (_record.StageID == 2)
            {

            }
            if (_recordData.State != null && _recordData.State.Length > 0)
            {
                for (int i = 0; i < this.comboBoxResult.Items.Count; i++)
                {
                    if (_recordData.State == this.comboBoxResult.Items[i].ToString())
                    {
                        this.comboBoxResult.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        public RecordData Data()
        {
            RecordData recordData = new RecordData();
            double Count;
            if (double.TryParse(this.textBoxNumberValue.Text, out Count))
                recordData.Count = Count;
            else recordData.Count = null;
            recordData.State = this.comboBoxResult.Text;
            recordData.Notes = this.textBoxNotes.Text.Replace("\r\n", " ");
            return recordData;
        }

        private void textBoxNumberValue_TextChanged(object sender, EventArgs e)
        {
            double test;
            if (this.textBoxNumberValue.Text.Length > 0 && !double.TryParse(this.textBoxNumberValue.Text, out test))
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxNumberValue.Text + " is not a numeric value");
                this.textBoxNumberValue.Text = "";
            }
        }

        #region Help
        private void FormIPM_Details_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            string _baseUrl = global::DiversityCollection.Properties.Settings.Default.DiversityCollectionManualUrl;
            Help.ShowHelp(this, _baseUrl + "/editing_dc/tasks_dc/ipm_dc/ipm_pests_dc/index.html");
        }

        #endregion
    }
}
