using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormDatawithholding : Form
    {

        #region Parameter

        System.Collections.Generic.List<int> _IDs;
        private Microsoft.Data.SqlClient.SqlConnection _SqlConnection;
        private int? _ProjectID = null; 
        
        #endregion

        #region Construction

        public FormDatawithholding(System.Collections.Generic.List<int> IDs)
        {
            InitializeComponent();
            this._IDs = IDs;
            string Message = "";
            this.initConnection();
            if (this.initIDs(ref Message))
                this.initForm();
            else
            {
                System.Windows.Forms.MessageBox.Show(Message, "Init failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        public FormDatawithholding(int ProjectID)
        {
            InitializeComponent();
            this.initConnection();
            this._ProjectID = ProjectID;
            string Message = "";
            string SQL = "INSERT INTO #CollectionSpecimenIDs(CollectionSpecimenID)  SELECT CollectionSpecimenID FROM CollectionProject WHERE ProjectID = " + ProjectID.ToString();
            if (this.SqlExecuteNonQuery(SQL, ref Message)) // DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, SqlConnection, ref Message))
            {
                SQL = "SELECT Project FROM ProjectProxy WHERE ProjectID = " + ProjectID.ToString();
                string Project = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this.labelHeader.Text = "Handling the datawithholding reasons for project " + Project;

                this.initForm();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Message);
            }

            //System.Data.DataTable dt = new DataTable();
            //if (DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message))
            //{
            //    this._IDs = new List<int>();
            //    foreach (System.Data.DataRow R in dt.Rows)
            //        this._IDs.Add(int.Parse(R[0].ToString()));
            //}
        }


        #endregion

        #region Auxillary
        
        private string _ListOfIDs;
        private string ListOfIDs
        {
            get
            {
                if (this._ListOfIDs == null)
                {
                    this._ListOfIDs = "";
                    foreach (int i in this._IDs)
                    {
                        if (this._ListOfIDs.Length > 0)
                            this._ListOfIDs += ", ";
                        this._ListOfIDs += i.ToString();
                    }
                }
                return this._ListOfIDs;
            }
        }

        private bool initIDs(ref string Message)
        {
            bool OK = true;
            foreach (int i in this._IDs)
            {
                string SQL = "INSERT INTO #CollectionSpecimenIDs(CollectionSpecimenID) VALUES (" + i.ToString() + ")";
                if (!this.SqlExecuteNonQuery(SQL, ref Message))
                {
                    OK = false;
                    break;
                }
            }
            return OK;
        }

        #endregion

        #region Form
        private void initForm()
        {
            try
            {
                if (this._SqlConnection != null && this._ListOfIDs == null) this._ListOfIDs = "";
                DiversityCollection.UserControls.UserControlDatawithholding DP = new UserControls.UserControlDatawithholding();
                DP.UCSummary = this.userControlDatawithholdingSummaryPart;
                DP.Dock = DockStyle.Fill;
                DP.initUserControl("CollectionSpecimenPart", this.ListOfIDs, this._SqlConnection);
                this.tabPagePart.Controls.Add(DP);
                this.userControlDatawithholdingSummaryPart.initUserControl(this.imageList.Images[this.tabPagePart.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DU = new UserControls.UserControlDatawithholding();
                DU.UCSummary = this.userControlDatawithholdingSummaryUnit;
                DU.Dock = DockStyle.Fill;
                DU.initUserControl("IdentificationUnit", this.ListOfIDs, this._SqlConnection);
                this.tabPageUnit.Controls.Add(DU);
                this.userControlDatawithholdingSummaryUnit.initUserControl(this.imageList.Images[this.tabPageUnit.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DA = new UserControls.UserControlDatawithholding();
                DA.UCSummary = this.userControlDatawithholdingSummaryAgent;
                DA.Dock = DockStyle.Fill;
                DA.initUserControl("CollectionAgent", this.ListOfIDs, this._SqlConnection);
                this.tabPageAgent.Controls.Add(DA);
                this.userControlDatawithholdingSummaryAgent.initUserControl(this.imageList.Images[this.tabPageAgent.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DE = new UserControls.UserControlDatawithholding();
                DE.UCSummary = this.userControlDatawithholdingSummaryEvent;
                DE.Dock = DockStyle.Fill;
                DE.initUserControl("CollectionEvent", this.ListOfIDs, this._SqlConnection);
                this.tabPageEvent.Controls.Add(DE);
                this.userControlDatawithholdingSummaryEvent.initUserControl(this.imageList.Images[this.tabPageEvent.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DDate = new UserControls.UserControlDatawithholding();
                DDate.DataWithholdingReasonColumn = "DataWithholdingReasonDate";
                DDate.UCSummary = this.userControlDatawithholdingSummaryCollectionDate;
                DDate.Dock = DockStyle.Fill;
                DDate.initUserControl("CollectionEvent", this.ListOfIDs, this._SqlConnection);
                this.tabPageCollectionDate.Controls.Add(DDate);
                this.userControlDatawithholdingSummaryCollectionDate.initUserControl(this.imageList.Images[this.tabPageCollectionDate.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DS = new UserControls.UserControlDatawithholding();
                DS.UCSummary = this.userControlDatawithholdingSummarySpecimen;
                DS.Dock = DockStyle.Fill;
                DS.initUserControl("CollectionSpecimen", this.ListOfIDs, this._SqlConnection);
                this.tabPageSpecimen.Controls.Add(DS);
                this.userControlDatawithholdingSummarySpecimen.initUserControl(this.imageList.Images[this.tabPageSpecimen.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DIC = new UserControls.UserControlDatawithholding();
                DIC.UCSummary = this.userControlDatawithholdingSummaryImagesCollection;
                DIC.Dock = DockStyle.Fill;
                DIC.initUserControl("CollectionImage", this.ListOfIDs, this._SqlConnection);
                this.tabPageImagesCollection.Controls.Add(DIC);
                this.userControlDatawithholdingSummaryImagesCollection.initUserControl(this.imageList.Images[this.tabPageImagesCollection.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DIE = new UserControls.UserControlDatawithholding();
                DIE.UCSummary = this.userControlDatawithholdingSummaryImagesEvent;
                DIE.Dock = DockStyle.Fill;
                DIE.initUserControl("CollectionEventImage", this.ListOfIDs, this._SqlConnection);
                this.tabPageImagesEvent.Controls.Add(DIE);
                this.userControlDatawithholdingSummaryImagesEvent.initUserControl(this.imageList.Images[this.tabPageImagesEvent.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DIES = new UserControls.UserControlDatawithholding();
                DIES.UCSummary = this.userControlDatawithholdingSummaryImagesSeries;
                DIES.Dock = DockStyle.Fill;
                DIES.initUserControl("CollectionEventSeriesImage", this.ListOfIDs, this._SqlConnection);
                this.tabPageImagesSeries.Controls.Add(DIES);
                this.userControlDatawithholdingSummaryImagesSeries.initUserControl(this.imageList.Images[this.tabPageImagesSeries.ImageIndex]);

                DiversityCollection.UserControls.UserControlDatawithholding DIS = new UserControls.UserControlDatawithholding();
                DIS.UCSummary = this.userControlDatawithholdingSummaryImagesSpecimen;
                DIS.Dock = DockStyle.Fill;
                DIS.initUserControl("CollectionSpecimenImage", this.ListOfIDs, this._SqlConnection);
                this.tabPageImagesSpecimen.Controls.Add(DIS);
                this.userControlDatawithholdingSummaryImagesSpecimen.initUserControl(this.imageList.Images[this.tabPageImagesSpecimen.ImageIndex]);

                string SQL = "SELECT COUNT(*) FROM CollectionSpecimen S WHERE S.CollectionSpecimenID NOT IN ( " +
                    "SELECT     T.CollectionSpecimenID " +
                    "FROM         CollectionSpecimenTransaction AS T INNER JOIN " +
                    "[Transaction] AS TR ON T.TransactionID = TR.TransactionID " +
                    "WHERE     (TR.TransactionType = N'embargo') " +
                    "AND (TR.BeginDate < GETDATE() OR TR.BeginDate IS NULL) " +
                    "AND (TR.AgreedEndDate > GETDATE() OR TR.AgreedEndDate IS NULL)) ";
                if (this._SqlConnection != null)
                    SQL += "AND S.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM #CollectionSpecimenIDs)";
                else
                    SQL += "AND S.CollectionSpecimenID IN (" + this.ListOfIDs + ")";
                string NumberNotInEmbargo = this.SqlExecuteScalar(SQL);// DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this.textBoxSummaryNoEmbargo.Text = "NO embargo: " + NumberNotInEmbargo + " specimen";

                SQL = "SELECT TR.TransactionTitle, convert(varchar(10), TR.BeginDate, 102) AS BeginDate, convert(varchar(10), TR.AgreedEndDate, 102) AS AgreedEndDate, " +
                    "COUNT(DISTINCT T.CollectionSpecimenID) AS NumberOfSpecimen " +
                    "FROM CollectionSpecimenTransaction AS T INNER JOIN " +
                    "[Transaction] AS TR ON T.TransactionID = TR.TransactionID " +
                    "WHERE     (TR.TransactionType = N'embargo')  " +
                    "AND (TR.BeginDate < GETDATE() OR TR.BeginDate IS NULL) " +
                    "AND (TR.AgreedEndDate > GETDATE() OR TR.AgreedEndDate IS NULL) ";
                if (this._SqlConnection != null)
                    SQL += "AND T.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM #CollectionSpecimenIDs)";
                else
                    SQL += "AND T.CollectionSpecimenID IN (" + this.ListOfIDs + ") ";
                SQL += "GROUP BY TR.BeginDate, TR.AgreedEndDate, TR.TransactionTitle";
                System.Data.DataTable dt = new DataTable();
                this.SqlFillTable(ref dt, SQL);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    System.Windows.Forms.TextBox T = new TextBox();
                    T.BorderStyle = BorderStyle.FixedSingle;
                    T.Dock = DockStyle.Top;
                    T.ForeColor = System.Drawing.Color.Red;
                    T.Text = R["TransactionTitle"].ToString();
                    if (!R["BeginDate"].Equals(System.DBNull.Value))
                        T.Text += "   starting: " + R["BeginDate"].ToString();
                    if (!R["AgreedEndDate"].Equals(System.DBNull.Value))
                        T.Text += "   ending: " + R["AgreedEndDate"].ToString();
                    T.Text += "   Number of specimen: " + R["NumberOfSpecimen"].ToString();
                    this.panelEmbargo.Controls.Add(T);
                }

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                null,
                null);
        }

        private void FormDatawithholding_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._SqlConnection != null && this._SqlConnection.State == ConnectionState.Open)
            {
                _SqlConnection.Close();
                _SqlConnection.Dispose();
            }    
        }
        #endregion

        #region SQL

        private Microsoft.Data.SqlClient.SqlConnection SqlConnection 
        { 
            get 
            {
                if (_SqlConnection == null)
                {
                    _SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                    _SqlConnection.Open();
                }
                return _SqlConnection;
            } 
        }

        private void initConnection()
        {
            string SQL = "create table #CollectionSpecimenIDs (CollectionSpecimenID int primary key)";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, SqlConnection);
        }

        private bool SqlExecuteNonQuery(string SQL, ref string Message)
        {
            bool OK = true;
            try
            {
                if (this._SqlConnection != null) OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, _SqlConnection, ref Message);
                else OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (System.Exception ex)
            {
                OK = false;
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return OK;
        }

        private void SqlFillTable(ref System.Data.DataTable dt, string SQL)
        {
            Microsoft.Data.SqlClient.SqlDataAdapter ad;
            if (this._SqlConnection != null) ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._SqlConnection);
            else ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dt);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private string SqlExecuteScalar(string SQL)
        {
            string Result = "";
            try
            {
                if (this._SqlConnection != null) Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, _SqlConnection);
                else Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return Result;
        }



        #endregion

    }
}
