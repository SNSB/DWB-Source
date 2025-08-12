using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class FormTransferTimer : Form
    {
        public FormTransferTimer(bool IncludeCacheDB, bool IncludePostgres, string Timesteps)
        {
            InitializeComponent();
            this.buttonTransferCacheDB.Visible = IncludeCacheDB;
            this.buttonTransferPostgres.Visible = IncludePostgres;
            this.labelTimesteps.Text = Timesteps;
        }

        private void buttonStopTimer_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.Close();
        }

        #region Public functions

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
