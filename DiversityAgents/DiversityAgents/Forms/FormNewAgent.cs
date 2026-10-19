using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityAgents.Forms
{
    public partial class FormNewAgent : Form
    {
        public FormNewAgent()
        {
            InitializeComponent();
            string SQL = "SELECT Code, DisplayText, DisplayOrder " +
                "FROM  dbo.AgentType_Enum " +
                "ORDER BY DisplayOrder";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxType.DataSource = dt;
            this.comboBoxType.DisplayMember = "DisplayText";
            this.comboBoxType.ValueMember = "Code";
            this.setOK();
        }

        public string GivenName
        {
            get { return this.textBoxGivenName.Text; }
        }

        public string InheritedName { get { return this.textBoxInheritedName.Text; } }

        public string Type
        {
            get { return this.comboBoxType.SelectedValue.ToString(); }
        }

        private void textBoxGivenName_TextChanged(object sender, EventArgs e)
        {
            this.setOK();
        }

        private void textBoxInheritedName_TextChanged(object sender, EventArgs e)
        {
            this.setOK();
        }

        private void setOK()
        {
            if (this.textBoxGivenName.Text.Length > 0 || this.textBoxInheritedName.Text.Length > 0)
                this.userControlDialogPanel.buttonOK.Enabled = true;
            else this.userControlDialogPanel.buttonOK.Enabled = false;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxType.SelectedItem;
            if (R["Code"].ToString().ToLower() == "person")
            {
                this.labelInheritedName.Visible = true;
                this.textBoxInheritedName.Visible = true;
                this.labelGivenName.Text = "First name";
            }
            else
            {
                this.labelInheritedName.Visible = false;
                this.textBoxInheritedName.Visible = false;
                this.textBoxInheritedName.Text = "";
                this.labelGivenName.Text = "Name";
            }
        }

        private void FormNewAgent_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, "https://www.diversityworkbench.de/manual/dwb/diversityagents/editingdata/index.html");
        }

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
