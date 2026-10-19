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
    public partial class FormContactsPublic : Form
    {
        public FormContactsPublic(string Title, int AgentID, int DisplayOrder)
        {
            InitializeComponent();
            System.Collections.Generic.List<int> Contacts = new List<int>();
            Contacts.Add(DisplayOrder);
            this.initForm(Title, AgentID, Contacts);
        }

        private void initForm(string Title, int AgentID, System.Collections.Generic.List<int> Contacts)
        {
            this.tabControl.TabPages.Clear();
            foreach(int C in Contacts)
            {
                DiversityAgents.UserControls.ContactInformation contactInformation = new UserControls.ContactInformation(AgentID, C);
                if (contactInformation.ContainsData())
                {
                    System.Windows.Forms.TabPage tabPage = new TabPage(Title);
                    tabPage.Controls.Add(contactInformation);
                    contactInformation.Dock = DockStyle.Fill;
                    this.tabControl.TabPages.Add(tabPage);
                }
            }
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
