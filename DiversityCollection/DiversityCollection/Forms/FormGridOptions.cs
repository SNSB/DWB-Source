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
    public partial class FormGridOptions : Form
    {

        #region Parameter and properties

        private System.Data.DataTable _dtAnalysis;
        private int _ProjectID;
        System.DateTime _AnalysisStartDate;

        public System.DateTime? AnalysisStartDate
        {
            get 
            {
                if (this._AnalysisStartDate.Year != 1 &&
                    this.checkBoxUseAnalysisStartDate.Checked)
                    return _AnalysisStartDate;
                else return null;
            }
            //set { _AnalysisStartDate = value; }
        }
        System.DateTime _AnalysisEndDate;

        public System.DateTime? AnalysisEndDate
        {
            get 
            {
                if (this._AnalysisEndDate.Year != 1 &&
                    this.checkBoxUseAnalysisEndDate.Checked)
                    return _AnalysisEndDate;
                else return null;
            }
            //set { _AnalysisEndDate = value; }
        }
        System.Collections.Specialized.StringCollection _AnalysisIDs;

        public System.Collections.Specialized.StringCollection AnalysisIDs
        {
            get 
            {
                if (this._AnalysisIDs == null)
                    this._AnalysisIDs = new System.Collections.Specialized.StringCollection();
                else this._AnalysisIDs.Clear();
                try
                {
                    foreach (DiversityCollection.Forms.AnalysisEntry A in this.AnalysisList)
                        this._AnalysisIDs.Add(A.AnalysisID.ToString());
                }
                catch (System.Exception ex) { }
                return _AnalysisIDs; 
            }
            //set { _AnalysisIDs = value; }
        }

        public int ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }
        
        public System.Collections.Generic.List<DiversityCollection.Forms.AnalysisEntry> AnalysisList
        {
            get
            {
                System.Collections.Generic.List<DiversityCollection.Forms.AnalysisEntry> List = new List<DiversityCollection.Forms.AnalysisEntry>();
                foreach (System.Data.DataRow R in this._dtAnalysis.Rows)
                {
                    try
                    {
                        DiversityCollection.Forms.AnalysisEntry AE = new DiversityCollection.Forms.AnalysisEntry();
                        AE.AnalysisID = int.Parse(R[1].ToString());
                        AE.AnalysisType = R[0].ToString();
                        List.Add(AE);
                    }
                    catch (System.Exception ex) { }
                }
                return List;
            }
        }

        #endregion

        #region Construction

        public FormGridOptions(
            System.DateTime AnalysisStartDate,
            System.DateTime AnalysisEndDate, 
            System.Collections.Specialized.StringCollection AnalysisIDs,
            int ProjectID)
        {
            InitializeComponent();
            this._AnalysisEndDate = AnalysisEndDate;
            this._AnalysisStartDate = AnalysisStartDate;
            this._AnalysisIDs = AnalysisIDs;
            this._ProjectID = ProjectID;
            this.initForm();
        }
        
        #endregion

        #region Form

        private void initForm()
        {
            this.textBoxProject.Text = DiversityCollection.LookupTable.ProjectName(this._ProjectID);
            this.FillAnalysisList();
            this.setAnalysisList();
            //if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.Year != 1)
            //    this.dateTimePickerAnalysisEndDate.Value = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate;
            //if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.Year != 1)
            //    this.dateTimePickerAnalysisStartDate.Value = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate;
            //this.checkBoxUseAnalysisTypes.Checked = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisIDs;
            //this.checkBoxUseAnalysisEndDate.Checked = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisEndDate;
            //this.checkBoxUseAnalysisStartDate.Checked = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisStartDate;
        }

        private void FormIdentificationUnitGridModeSetSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                //DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate = this.dateTimePickerAnalysisEndDate.Value;
                //DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate = this.dateTimePickerAnalysisStartDate.Value;
                //DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisEndDate = this.checkBoxUseAnalysisEndDate.Checked;
                //DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisStartDate = this.checkBoxUseAnalysisStartDate.Checked;
                //DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisIDs = true;// this.checkBoxUseAnalysisTypes.Checked;
                System.Collections.Specialized.StringCollection ss = new System.Collections.Specialized.StringCollection();
                foreach (System.Data.DataRow R in this._dtAnalysis.Rows)
                {
                    if (R.RowState != DataRowState.Deleted)
                        ss.Add(R[1].ToString());
                }
                DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs = ss;
                DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.Save();
            }
        }
        
        #endregion    
    
        private void setAnalysisList()
        {
            if (this._dtAnalysis.Rows.Count > 0)
            {
                this.listBoxAnalysisTypes.DataSource = this._dtAnalysis;
                this.listBoxAnalysisTypes.DisplayMember = "DisplayText";
                this.listBoxAnalysisTypes.ValueMember = "AnalysisID";
            }
        }

        private void FillAnalysisList()
        {
            this._dtAnalysis = new DataTable();
            if (this._AnalysisIDs != null)
            {
                string SQL = "";
                foreach (string s in this._AnalysisIDs)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += s;
                }
                SQL = "SELECT DisplayText, AnalysisID FROM Analysis WHERE AnalysisID IN (" + SQL + ") ORDER BY DisplayText";
                this._dtAnalysis.Clear();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._dtAnalysis);
            }
        }

        public System.Data.DataTable DtAnalysis
        {
            get { return this._dtAnalysis; }
        }

        private void buttonAnalysisIDsAdd_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT A.AnalysisID, A.AnalysisParentID, A.DisplayText FROM dbo.AnalysisProjectList(" + this.ProjectID.ToString() + ") A ORDER BY A.DisplayText";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No analysis types are defined for the current project");
                return;
            }
            DiversityWorkbench.Forms.FormGetItemFromHierarchy f = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(dt, "AnalysisID", "AnalysisParentID", "DisplayText", "AnalysisID", "Analysis", "Select an analysis");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (this._dtAnalysis.Rows.Count == 0)
                {
                    SQL = "SELECT DisplayText, AnalysisID FROM Analysis WHERE AnalysisID = " + f.SelectedValue;
                    ad.SelectCommand.CommandText = SQL; ;
                    ad.Fill(this._dtAnalysis);
                    this.setAnalysisList();
                }
                else
                {
                    System.Data.DataRow R = this._dtAnalysis.NewRow();
                    R[1] = int.Parse(f.SelectedValue);
                    R[0] = f.SelectedString;
                    System.Data.DataRow[] rr = this._dtAnalysis.Select("AnalysisID = " + f.SelectedValue);
                    if (rr.Length == 0)
                        this._dtAnalysis.Rows.Add(R);
                    else
                        System.Windows.Forms.MessageBox.Show("The analysis\r\n" + f.SelectedString + "\r\nis allready in the list");
                }
            }
        }

        private void buttonAnalysisIDsRemove_Click(object sender, EventArgs e)
        {
            if (this.listBoxAnalysisTypes.SelectedIndex > -1)
                this._dtAnalysis.Rows[this.listBoxAnalysisTypes.SelectedIndex].Delete();
        }

        #region setting the dates

        private void checkBoxUseAnalysisStartDate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxUseAnalysisStartDate.Checked)
                this._AnalysisStartDate = this.dateTimePickerAnalysisStartDate.Value;
        }

        private void checkBoxUseAnalysisEndDate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.dateTimePickerAnalysisEndDate.Checked)
                this._AnalysisEndDate = this.dateTimePickerAnalysisEndDate.Value;
        }

        private void dateTimePickerAnalysisStartDate_ValueChanged(object sender, EventArgs e)
        {
            if (this.checkBoxUseAnalysisStartDate.Checked)
                this._AnalysisStartDate = this.dateTimePickerAnalysisStartDate.Value;
        }

        private void dateTimePickerAnalysisEndDate_ValueChanged(object sender, EventArgs e)
        {
            if (this.dateTimePickerAnalysisEndDate.Checked)
                this._AnalysisEndDate = this.dateTimePickerAnalysisEndDate.Value;
        }

        #endregion

    }
}
