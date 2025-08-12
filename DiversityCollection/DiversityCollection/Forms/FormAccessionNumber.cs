using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormAccessionNumber : Form
    {
        #region Construction
        //private string _AccessionNumber = "";
        /// <summary>
        /// searching for a free accession number
        /// </summary>
        /// <param name="InitialString">The initials of a new accession number</param>
        //public FormAccessionNumber(string InitialString)
        //{
        //    InitializeComponent();
        //    this.textBoxStart.Text = InitialString;
        //    this.InitAutoCompletion();
        //}

        /// <summary>
        /// searching for a free accession number
        /// </summary>
        /// <param name="InitialString">The initials of a new accession number</param>
        /// <param name="IncludeSpecimen">If the specimen must be included in the search</param>
        /// <param name="IncludeParts">If the parts must be included in the search</param>
        public FormAccessionNumber(string InitialString, bool IncludeSpecimen, bool IncludeParts)
        {
            InitializeComponent();
            this.textBoxStart.Text = InitialString;
            this.InitAutoCompletion();
            if (IncludeSpecimen)
            {
                this.checkBoxIncludeSpecimen.Checked = true;
                this.checkBoxIncludeSpecimen.Enabled = false;
            }
            if (IncludeParts)
            {
                this.checkBoxIncludeParts.Checked = true;
                this.checkBoxIncludeParts.Enabled = false;
            }
            if (!IncludeParts && !IncludeSpecimen)
                this.checkBoxIncludeSpecimen.Checked = true;
        }

        public FormAccessionNumber(string HeaderInfo, string InitialString, bool IncludeSpecimen, bool IncludeParts)
        {
            InitializeComponent();
            this.labelHeader.Text = HeaderInfo;
            this.textBoxStart.Text = InitialString;
            this.InitAutoCompletion();
            if (IncludeSpecimen)
            {
                this.checkBoxIncludeSpecimen.Checked = true;
                this.checkBoxIncludeSpecimen.Enabled = false;
            }
            if (IncludeParts)
            {
                this.checkBoxIncludeParts.Checked = true;
                this.checkBoxIncludeParts.Enabled = false;
            }
            if (!IncludeParts && !IncludeSpecimen)
                this.checkBoxIncludeSpecimen.Checked = true;
        }

        /// <summary>
        /// searching for a free accession number
        /// </summary>
        /// <param name="InitialString">The initials of a new accession number</param>
        /// <param name="StartAutoSearch">If the search should be started without user interaction</param>
        /// <param name="IncludeSpecimen">If the specimen must be included in the search</param>
        /// <param name="IncludeParts">If the parts must be included in the search</param>
        public FormAccessionNumber(string InitialString, bool StartAutoSearch, bool IncludeSpecimen, bool IncludeParts)
            : this(InitialString, IncludeSpecimen, IncludeParts)
        {
            this.textBoxStart.Text = InitialString;
            this.getNextFreeAccessionNumber(InitialString, StartAutoSearch);
        }

        #endregion

        #region events
        private string getNextFreeAccessionNumber(string AccNr, bool AsAutosearch)
        {
            string NextAccNr = "";
            string SQL = "SELECT [dbo].[NextFreeAccNumber] ('" + AccNr + "'";
            if (this.checkBoxIncludeSpecimen.Checked) SQL += ", 1";
            else SQL += ", 0";
            if (this.checkBoxIncludeParts.Checked) SQL += ", 1)";
            else SQL += ", 0)";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                NextAccNr = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (AsAutosearch && NextAccNr.Length > 0)
                {
                    this.textBoxResult.Text = NextAccNr;
                    this.labelResult.Text = "Next free accession number:";
                }
                else
                {
                    SQL = "SELECT [dbo].[NextFreeAccNr] ('" + AccNr + "')";
                    C.CommandText = SQL;
                    NextAccNr = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    if (NextAccNr.Length > 0)
                    {
                        this.textBoxResult.Text = NextAccNr;
                        this.labelResult.Text = "Next free accession number:";
                    }
                }
            }
            catch
            {
                if (!AsAutosearch)
                    System.Windows.Forms.MessageBox.Show("The program could not find the next free number");
            }
            con.Close();
            return NextAccNr;
        }

        private void InitAutoCompletion()
        {
            string SQL = "SELECT MIN(AccessionNumber) AS AccessionNumber " +
                "FROM  CollectionSpecimen_Core " +
                "WHERE (LEN(AccessionNumber) > 1) " +
                "GROUP BY LEN(AccessionNumber), SUBSTRING(AccessionNumber, 1, 1) " +
                "ORDER BY AccessionNumber";
            System.Data.DataTable DT = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(DT);
            System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
            foreach (System.Data.DataRow R in DT.Rows)
                StringCollection.Add(R[0].ToString());

            this.textBoxStart.AutoCompleteCustomSource = StringCollection;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.textBoxResult.Text = this.getNextFreeAccessionNumber(this.textBoxStart.Text, false);
            if (this.textBoxResult.Text.Length == 0)
                this.labelResult.Text = "No valid accession number could be found";
            else
                this.labelResult.Text = "Next free accession number:";
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region Properties

        public string AccessionNumber
        {
            get
            {
                return this.textBoxResult.Text;
            }
        }

        public bool IncludeSpecimenInSearch()
        { return this.checkBoxIncludeSpecimen.Checked; }
        public bool IncludePartInSearch()
        { return this.checkBoxIncludeParts.Checked; }

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