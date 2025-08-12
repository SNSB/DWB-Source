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
    public partial class FormTransferHistory : Form
    {

        #region Parameter

        private int? _ProjectID = null;
        private string _Source = null;
        private string _SourceView = null;
        private int? _TargetID = null;
        private string _Package = null;
        
        #endregion

        #region Construction

        public FormTransferHistory(int ProjectID, int? TargetID = null, string Package = "")
        {
            InitializeComponent();
            try
            {
                this._ProjectID = ProjectID;
                this._Package = Package;
                string SQL = "SELECT Project FROM ProjectPublished AS P WHERE ProjectID = " + this._ProjectID.ToString();
                this.textBoxSource.Text = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                this._TargetID = TargetID;
                if (this._TargetID != null)
                {
                    this.labelDatabase.Visible = true;
                    this.textBoxDatabase.Visible = true;
                    this.textBoxDatabase.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;// DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                }
                this.initForm();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public FormTransferHistory(string Source, string SourceView, int? TargetID = null)
        {
            InitializeComponent();
            try
            {
                this.textBoxSource.Text = Source + ": " + SourceView;
                this._Source = Source;
                this._SourceView = SourceView;
                this._TargetID = TargetID;
                if (this._TargetID != null)
                {
                    this.labelDatabase.Visible = true;
                    this.textBoxDatabase.Visible = true;
                    this.textBoxDatabase.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;// DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                }
                this.initForm();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                string SQL = "SELECT convert(nvarchar(50), [TransferDate], 120) AS TransferDate " +
                ",CASE WHEN U.CombinedNameCache <> U.LoginName AND U.CombinedNameCache <> '' THEN U.CombinedNameCache ELSE U.LoginName END AS Responsible " +
                ",T.[Settings] ";
                if (this._Source == null)
                    SQL += ", Package ";
                SQL += "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.UserProxy U, [dbo].";
                if (this._Source != null)
                {
                    SQL += "[SourceTransfer] T WHERE T.Source = '" + this._Source + "' AND T.SourceView = '" + this._SourceView + "'";
                }
                else
                {
                    SQL += "[ProjectTransfer] T WHERE T.ProjectID = " + this._ProjectID.ToString();
                }
                if (this._TargetID != null)
                    SQL += " AND T.TargetID = " + this._TargetID.ToString();
                else
                    SQL += " AND T.TargetID IS NULL ";
                if (this._Package != null && this._Package.Length > 0)
                    SQL += " AND T.Package = '" + this._Package + "'";
                SQL += " AND T.ResponsibleUserID = U.ID ORDER BY TransferDate";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(this.dataSetTransferHistory.TransferHistory);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void listBoxTransfer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.treeViewSettings.Nodes.Clear();
                System.Data.DataRow R = this.dataSetTransferHistory.TransferHistory.Rows[this.transferHistoryBindingSource.Position];
                string Settings = R["Settings"].ToString();
                if (R.Table.Columns.Contains("Package") && R["Package"].ToString().Length > 0)
                {
                    this.labelPackage.Visible = true;
                    this.textBoxPackage.Visible = true;
                    this.textBoxPackage.Text = R["Package"].ToString();
                }
                else
                {
                    this.labelPackage.Visible = false;
                    this.textBoxPackage.Visible = false;
                }
                DiversityWorkbench.Forms.FormFunctions.JsonToTreeView(this.treeViewSettings, Settings);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void FormTransferHistory_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetTransferHistory.TransferHistory". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.transferHistoryTableAdapter.Fill(this.dataSetTransferHistory.TransferHistory);
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
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
