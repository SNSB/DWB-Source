using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormProgress : Form
    {
        #region Parameter
        private string _MessageText;
        private bool _Abort;
        #endregion

        #region Construction
        public FormProgress(string messageText, int maxNo, bool showAbort = true)
        {
            InitializeComponent();
            _MessageText = messageText;
            _Abort = false;
            this.buttonAbort.Visible = showAbort;
            this.progressBar.Value = 0;
            this.progressBar.Step = 1;
            this.progressBar.Maximum = maxNo;
            this.labelTitle.Text = string.Format("{0} ({1}/{2})", _MessageText, this.progressBar.Value, this.progressBar.Maximum);
        }
        #endregion

        #region Public
        /// <summary>
        /// Increases progress bar value and actualizes display
        /// CAUTION: Calls DoEvents! Block possible concurring events!
        /// </summary>
        /// <returns>'true' if action shall be aborted</returns>
        public bool PerformStep()
        {
            this.progressBar.PerformStep();
            this.labelTitle.Text = string.Format("{0} ({1}/{2})", _MessageText, this.progressBar.Value, this.progressBar.Maximum);
            Application.DoEvents();
            return _Abort;
        }

        /// <summary>
        /// Sets message text and increases progress bar value and actualizes display
        /// CAUTION: Calls DoEvents! Block possible concurring events!
        /// </summary>
        /// <param name="messageText"></param>
        /// <returns>'true' if action shall be aborted</returns>
        public bool PerformStep(string messageText)
        {
            _MessageText = messageText;
            return PerformStep();
        }
        #endregion

        #region Events
        private void buttonAbort_Click(object sender, EventArgs e)
        {
            _Abort = true;
        }

        private void FormProgress_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
        }
        #endregion
    }
}
