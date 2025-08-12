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
    public partial class FormPostgresTargets : Form
    {
        private int _ProjectID;
        public FormPostgresTargets(int ProjectID, string Project)
        {
            InitializeComponent();
            this.Text += " in project " + Project;
            this._ProjectID = ProjectID;
            this.InitForm();
        }

        private void InitForm()
        {
            try
            {
                string SQL = "SELECT " +
                    "T.[Server],  " +
                    "T.Port,  " +
                    "T.DatabaseName AS [Postgres database],  " +
                    "PTP.Package, " +
                    "convert(varchar(19), PT.LastUpdatedWhen, 121) AS LastUpdate,  " +
                    "PT.TransferProtocol as Protocol,  " +
                    "PT.TransferErrors AS Errors,  " +
                    "PT.TransferIsExecutedBy AS Responsible,  " +
                    "CASE when PT.IncludeInTransfer = 1 then " +
                    "case when PT.TransferDays like '%0%' then 'Su ' else '.' end + " +
                    "case when PT.TransferDays like '%1%' then 'Mo ' else '.' end + " +
                    "case when PT.TransferDays like '%2%' then 'Tu ' else '.' end + " +
                    "case when PT.TransferDays like '%3%' then 'We ' else '.' end + " +
                    "case when PT.TransferDays like '%4%' then 'Th ' else '.' end + " +
                    "case when PT.TransferDays like '%5%' then 'Fr ' else '.' end + " +
                    "case when PT.TransferDays like '%6%' then 'Sa' else '.' end " +
                    " else '' end AS [Scheduler days], " +
                    "CASE when PT.IncludeInTransfer = 1 AND PT.CompareLogDate = 1 then '+' else '' end AS CompareDate,  " +
                    "CASE when PT.IncludeInTransfer = 1 then convert(varchar(10), PT.TransferTime, 108) else '' end as [Time]  " +
                    "FROM ProjectTarget AS PT INNER JOIN " +
                    "Target AS T ON PT.TargetID = T.TargetID INNER JOIN " +
                    "ProjectPublished AS PP ON PT.ProjectID = PP.ProjectID LEFT OUTER JOIN " +
                    "ProjectTargetPackage AS PTP ON PT.ProjectID = PTP.ProjectID AND PT.TargetID = PTP.TargetID " +
                    "WHERE PP.ProjectID = " + this._ProjectID.ToString() +
                    " ORDER BY T.[Server], T.Port, T.DatabaseName";
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                if (dt.Rows.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("No targets for postgres defined");
                    this.Close();
                }
                this.dataGridView.DataSource = dt;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i == 5) continue;
                    this.dataGridView.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader);
                    this.dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
                }
                this.Height = 80 + dt.Rows.Count * 23;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

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
