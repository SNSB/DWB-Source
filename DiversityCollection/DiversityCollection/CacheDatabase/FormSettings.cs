using DiversityWorkbench.DwbManual;
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
    public partial class FormSettings : Form
    {

        #region Construction

        public FormSettings()
        {
            InitializeComponent();
            this.initForm();
        }
        
        #endregion

        #region Form

        private void initForm()
        {
            int TimeOut = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB / 60;
            this.textBoxTimeout.Text = TimeOut.ToString();
            this.textBoxCacheChunkLimit.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkLimitCacheDB.ToString();
            this.textBoxCacheChunkSize.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizeCacheDB.ToString();

            this.checkBoxUseChunksForPostgres.Checked = DiversityCollection.CacheDatabase.CacheDBsettings.Default.UseChunksForPostgres;
            this.textBoxPostgresChunkLimit.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkLimitPostgres.ToString();
            this.textBoxPostgresChunkSize.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizePostgres.ToString();
            TimeOut = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres / 60;
            this.textBoxTimeoutPostgres.Text = TimeOut.ToString();
            this.initLogEventControls();
            this.initStopOnErrorControls();
            this.initPostgresControlsForSourceTransfer();
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
                DiversityCollection.CacheDatabase.CacheDBsettings.Default.Save();
        }
        
        #endregion

        #region SQL db transfer

        private void textBoxTimeout_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(this.textBoxTimeout.Text, out i))
                DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB = i * 60;
            else
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxTimeout.Text + " is not a valid value");
                int TimeOut = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB / 60;
                this.textBoxTimeout.Text = TimeOut.ToString();
            }
        }

        private void textBoxCacheChunkLimit_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(this.textBoxCacheChunkLimit.Text, out i))
                DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkLimitCacheDB = i;
            else
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxCacheChunkLimit.Text + " is not a valid value");
                this.textBoxCacheChunkLimit.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkLimitCacheDB.ToString();
            }
        }

        private void textBoxCacheChunkSize_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(this.textBoxCacheChunkSize.Text, out i))
                DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizeCacheDB = i;
            else
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxCacheChunkSize.Text + " is not a valid value");
                this.textBoxCacheChunkSize.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizeCacheDB.ToString();
            }
        }
        
        #endregion

        #region Postgres

        private void textBoxPostgresChunkLimit_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(this.textBoxPostgresChunkLimit.Text, out i))
                DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkLimitPostgres = i;
            else
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxPostgresChunkLimit.Text + " is not a valid value");
                this.textBoxPostgresChunkLimit.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkLimitPostgres.ToString();
            }
        }

        private void textBoxPostgresChunkSize_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(this.textBoxPostgresChunkSize.Text, out i))
                DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizePostgres = i;
            else
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxPostgresChunkSize.Text + " is not a valid value");
                this.textBoxPostgresChunkSize.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.ChunkSizePostgres.ToString();
            }
        }

        private void textBoxTimeoutPostgres_TextChanged(object sender, EventArgs e)
        {
            int i;
            if (int.TryParse(this.textBoxTimeoutPostgres.Text, out i))
                DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres = i * 60;
            else
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxTimeoutPostgres.Text + " is not a valid value");
                int TimeOut = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutPostgres / 60;
                this.textBoxTimeoutPostgres.Text = TimeOut.ToString();
            }
        }

        /// <summary>
        /// If the transfer to the postgres database should use chunks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxUseChunksForPostgres_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.UseChunksForPostgres = !DiversityCollection.CacheDatabase.CacheDBsettings.Default.UseChunksForPostgres;
            this.checkBoxUseChunksForPostgres.Checked = DiversityCollection.CacheDatabase.CacheDBsettings.Default.UseChunksForPostgres;
        }

        //private void checkBoxTransferPostgresToFile_Click(object sender, EventArgs e)
        //{
        //    DiversityCollection.CacheDatabase.CacheDBsettings.Default.TransferPostgresToFile = !DiversityCollection.CacheDatabase.CacheDBsettings.Default.TransferPostgresToFile;
        //    this.checkBoxTransferPostgresToFile.Checked = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TransferPostgresToFile;
        //}

        #endregion

        #region Defaults
        private void buttonDefaults_Click(object sender, EventArgs e)
        {
            this.textBoxCacheChunkLimit.Text = "1000000";
            this.textBoxCacheChunkSize.Text = "100000";
            this.textBoxPostgresChunkLimit.Text = "100000";
            this.textBoxPostgresChunkSize.Text = "10000";
            this.textBoxTimeout.Text = "0";
            this.textBoxTimeoutPostgres.Text = "0";
        }

        #endregion

        #region Logging

        private void checkBoxLogEvents_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents = !DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents;
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.Save();
            this.initLogEventControls();
        }

        private void initLogEventControls()
        {
            this.checkBoxLogEvents.Checked = DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents;
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
                this.pictureBoxLogEvents.Image = DiversityCollection.Resource.List;
            else
                this.pictureBoxLogEvents.Image = DiversityCollection.Resource.ListNot;
        }

        #endregion

        #region Stop on error
        private void checkBoxStopOnError_Click(object sender, EventArgs e)
        {
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError = !DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError;
            DiversityCollection.CacheDatabase.CacheDBsettings.Default.Save();
            this.initStopOnErrorControls();
        }

        private void initStopOnErrorControls()
        {
            this.checkBoxStopOnError.Checked = DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError;
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.StopOnError)
                this.pictureBoxStopOnError.Image = DiversityCollection.Resource.ArrowStop;
            else
                this.pictureBoxStopOnError.Image = DiversityCollection.Resource.ArrowNextNext;
        }


        #endregion

        #region BulkTransfer

        private void buttonPostgresTransferDirectory_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Transfer directory", "Please enter path of the transfer directory on the postgres server", this.textBoxPostgresTransferDirectory.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory = f.String;
                this.initPostgresControlsForSourceTransfer();
            }
        }

        private void buttonPostgresBashFile_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Bash file", "Please enter path of the bash file for conversion of the exported files", this.textBoxPostgresBashFile.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile = f.String;
                this.initPostgresControlsForSourceTransfer();
            }
        }

        private void buttonPostgresMountPoint_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Bash file", "Please enter the mount point name of the transfer folder", this.textBoxPostgresMountPoint.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                DiversityCollection.CacheDatabase.CacheDB.BulkTransferMountPoint = f.String;
                this.initPostgresControlsForSourceTransfer();
            }
        }

        private void initPostgresControlsForSourceTransfer()
        {
            try
            {

                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                {
                    this.buttonPostgresBashFile.Enabled = true;
                    this.buttonPostgresTransferDirectory.Enabled = true;
                    this.textBoxPostgresBashFile.Enabled = true;
                    this.textBoxPostgresTransferDirectory.Enabled = true;
                    this.textBoxPostgresTransferDirectory.Text = DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory; // _SourceTransferDirectory;
                    this.textBoxPostgresBashFile.Text = DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile;// _BashFile;
                    this.textBoxPostgresMountPoint.Text = DiversityCollection.CacheDatabase.CacheDB.BulkTransferMountPoint;// _BashFile;
                }
                else
                {
                    this.buttonPostgresBashFile.Enabled = false;
                    this.buttonPostgresTransferDirectory.Enabled = false;
                    this.textBoxPostgresBashFile.Enabled = false;
                    this.textBoxPostgresTransferDirectory.Enabled = false;
                    this.textBoxPostgresMountPoint.Enabled = false;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
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
