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
    public partial class FormGetProject : Form
    {
        #region Construction

        public FormGetProject(System.Data.DataTable dtProjects)
        {
            InitializeComponent();
            this.comboBoxProjects.DataSource = dtProjects;
            this.comboBoxProjects.DisplayMember = "Project";
            this.comboBoxProjects.ValueMember = "ProjectID";
        }
        #endregion

        #region Properties

        public int ProjectID { get { return int.Parse(this.comboBoxProjects.SelectedValue.ToString()); } }
        public string Project { get { return this.comboBoxProjects.SelectedText; } }

        #endregion

        private void FormGetProject_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, "https://www.diversityworkbench.de/manual/dwb/diversityagents/editingdata/projects/index.html");
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