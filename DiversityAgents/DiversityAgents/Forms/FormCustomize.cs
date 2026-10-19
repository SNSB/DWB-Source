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
    public partial class FormCustomize : Form
    {
        #region Construction & Form
        public FormCustomize()
        {
            InitializeComponent();
            this.initForm();
        }

        private void initForm()
        {
            this.initQuery();
            this.initLoadConnections();
        }

        private void FormCustomize_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        #endregion

        #region Query

        public bool QueryDefaultsChanged()
        {
            return _QueryDefaultOptimizedChanged != DiversityWorkbench.Settings.QueryOptimizedByDefault || _QueryDefaultRememberChanged != DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault;
        }

        private bool _QueryDefaultOptimizedChanged = false;
        private bool _QueryDefaultRememberChanged = false;

        private void initQuery()
        {
            this.setQueryRememberControl();
            this.setQueryOptimizedControl();
            _QueryDefaultOptimizedChanged = DiversityWorkbench.Settings.QueryOptimizedByDefault;
            _QueryDefaultRememberChanged = DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault;
        }

        private void setQueryOptimizedControl()
        {
            this.checkBoxQueryOptimized.Checked = DiversityWorkbench.Settings.QueryOptimizedByDefault;
            if (DiversityWorkbench.Settings.QueryOptimizedByDefault) this.pictureBoxQueryOptimized.BackColor = System.Drawing.Color.Yellow;
            else this.pictureBoxQueryOptimized.BackColor = System.Drawing.Color.Transparent;
        }

        private void setQueryRememberControl()
        {
            this.checkBoxQueryRemember.Checked = DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault;
            if (DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault) this.pictureBoxQueryRemember.BackColor = System.Drawing.Color.Yellow;
            else this.pictureBoxQueryRemember.BackColor = System.Drawing.Color.Transparent;
        }

        private void checkBoxQueryOptimized_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.QueryOptimizedByDefault = !DiversityWorkbench.Settings.QueryOptimizedByDefault;
            this.setQueryOptimizedControl();
        }

        private void checkBoxQueryRemember_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault = !DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault;
            this.setQueryRememberControl();
        }

        #endregion

        #region LoadConnections

        private void initLoadConnections()
        {
            this.checkBoxLoadConnections.Checked = DiversityWorkbench.Settings.LoadConnections;
        }

        private void checkBoxLoadConnections_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.LoadConnections = this.checkBoxLoadConnections.Checked;
        }

        #endregion

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
