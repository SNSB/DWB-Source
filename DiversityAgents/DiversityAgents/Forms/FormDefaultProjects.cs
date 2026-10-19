using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityAgents.Forms
{
    public partial class FormDefaultProjects : Form
    {
        public FormDefaultProjects()
        {
            InitializeComponent();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();// ...Windows.Forms.Application.StartupPath + "\\DiversityAgents.chm";
        }

        private void FormDefaultProjects_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetProject.ProjectProxy". Sie können sie bei Bedarf verschieben oder entfernen.
            this.projectProxyTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            this.projectProxyTableAdapter.Fill(this.dataSetProject.ProjectProxy);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetProject.UserProxy". Sie können sie bei Bedarf verschieben oder entfernen.
            this.userProxyTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            this.userProxyTableAdapter.Fill(this.dataSetProject.UserProxy);

        }

        private void FormDefaultProjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.userProxyTableAdapter.Adapter.Update(this.dataSetProject.UserProxy);
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
