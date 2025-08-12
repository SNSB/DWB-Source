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
    public partial class FormIdentificationUnitGridModeSetSettings : Form
    {
        private System.Data.DataTable _dtAnalysis;
        private int? _ProjectID;

        public int? ProjectID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }

        public FormIdentificationUnitGridModeSetSettings()
        {
            InitializeComponent();
            this.initForm();
        }

        private void initForm()
        {
            this.FillAnalysisList();
            this.setAnalysisList();
            if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate.Year != 1) 
                this.dateTimePickerAnalysisEndDate.Value = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate;
            if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate.Year != 1)
                this.dateTimePickerAnalysisStartDate.Value = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate;
            //this.checkBoxUseAnalysisTypes.Checked = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisIDs;
            this.checkBoxUseAnalysisEndDate.Checked = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisEndDate;
            this.checkBoxUseAnalysisStartDate.Checked = DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisStartDate;
        }

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
            if (DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs != null
                && DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs.Count > 0)
            {
                string SQL = "";
                foreach (string s in DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisIDs)
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

        public System.Collections.Generic.Dictionary<int, string> AnalysisIDs
        {
            get
            {
                System.Collections.Generic.Dictionary<int, string> _AnalysisIDs = new Dictionary<int, string>();
                foreach (System.Data.DataRow R in this._dtAnalysis.Rows)
                    _AnalysisIDs.Add(int.Parse(R[0].ToString()), R[1].ToString());
                return _AnalysisIDs;
            }
        }

        public System.Data.DataTable DtAnalysis
        {
            get { return this._dtAnalysis; }
        }

        private void buttonAnalysisIDsAdd_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT DisplayText, AnalysisID FROM Analysis ORDER BY DisplayText";
            if (this.ProjectID != null)
                SQL = "SELECT DisplayText, AnalysisID FROM dbo.AnalysisProjectList(" + this.ProjectID.ToString() + ") ORDER BY DisplayText";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No analysis types are defined for the current project");
                return;
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "AnalysisID", "Analysis", "Select an analysis");
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

        private void FormIdentificationUnitGridModeSetSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisEndDate = this.dateTimePickerAnalysisEndDate.Value;
                DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.AnalysisStartDate = this.dateTimePickerAnalysisStartDate.Value;
                DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisEndDate = this.checkBoxUseAnalysisEndDate.Checked;
                DiversityCollection.Forms.FormIdentificationUnitGridModeSettings.Default.UseAnalysisStartDate = this.checkBoxUseAnalysisStartDate.Checked;
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
        
    }
}
