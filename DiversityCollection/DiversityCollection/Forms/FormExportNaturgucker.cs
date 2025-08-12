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
    public partial class FormExportNaturgucker : Form
    {
        #region Parameter

        private System.Collections.Generic.List<int> _IDs;
        private int _ProjectID;
        private System.Data.DataTable _dtAnalysis;

        #endregion

        #region Form

        public FormExportNaturgucker(System.Collections.Generic.List<int> IDs, int ProjectID)
        {
            InitializeComponent();
            try
            {
                this._IDs = IDs;
                this._ProjectID = ProjectID;
                this.textBoxExportFile.Text = Folder.Export() + "Naturgucker.txt";

                if (this._IDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select at least one item for the export");
                    this.Close();
                }
                this.initForm();
                // online manual
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            }
            catch { }
        }

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }


        private void initForm()
        {

            try
            {
                this.initAnalysis();
                this.initTaxonomicGroups();

                System.Uri U = new Uri("http://www.naturgucker.de");
                this.webBrowser.Url = U;
                if (this.webBrowser.Url == null)
                {
                    this.splitContainer.Panel2Collapsed = true;
                    this.webBrowser.Visible = false;
                }
            }
            catch (System.Exception ex) { }
        }

        private void initTaxonomicGroups()
        {
            try
            {
                string SQL = "SELECT IdentificationUnit.TaxonomicGroup " +
                    " FROM CollectionProject INNER JOIN " +
                    " IdentificationUnit ON CollectionProject.CollectionSpecimenID = IdentificationUnit.CollectionSpecimenID " +
                    " WHERE CollectionProject.ProjectID =  " + this._ProjectID.ToString() +
                    " GROUP BY IdentificationUnit.TaxonomicGroup " +
                    " HAVING  (NOT (IdentificationUnit.TaxonomicGroup IS NULL)) " +
                    " ORDER BY IdentificationUnit.TaxonomicGroup";
                System.Data.DataTable dtTaxonomicGroups = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtTaxonomicGroups);
                foreach (System.Data.DataRow R in dtTaxonomicGroups.Rows)
                {
                    bool OK = true;
                    if (R[0].ToString() == "fungus" && this._ProjectID == 373) OK = false;
                    this.checkedListBoxTaxonomicGroups.Items.Add(R[0].ToString(), OK);
                }

            }
            catch { }
        }

        private void initAnalysis()
        {
            try
            {
                string SQL = "SELECT A.DisplayText, A.AnalysisID " +
                    "FROM CollectionProject AS P INNER JOIN " +
                    "IdentificationUnit AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID INNER JOIN " +
                    "IdentificationUnitAnalysis AS UA ON U.CollectionSpecimenID = UA.CollectionSpecimenID " +
                    "AND U.IdentificationUnitID = UA.IdentificationUnitID INNER JOIN " +
                    "Analysis AS A ON UA.AnalysisID = A.AnalysisID " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString() +
                    "GROUP BY A.DisplayText, A.AnalysisID " +
                    "ORDER BY A.DisplayText";
                this._dtAnalysis = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(_dtAnalysis);
                foreach (System.Data.DataRow R in _dtAnalysis.Rows)
                {
                    bool OK = true;
                    this.checkedListBoxAnalysis.Items.Add(R[0].ToString(), OK);
                }
            }
            catch { }
        }

        #endregion

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory = Folder.Export();
            this.openFileDialog.Filter = "Text Files|*.txt";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxExportFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonStartExport_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (!this.ExportObservations())
                System.Windows.Forms.MessageBox.Show("An error occured while generating the export");
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        /// <summary>
        /// Export the observations of the current project of all taxa of the selected taxonomic groups
        /// including 
        /// Coordinates (WGS84)
        /// Species
        /// Collector
        /// Date
        /// </summary>
        private bool ExportObservations()
        {
            bool OK = true;
            try
            {
                this.textBox.Text = "Art\tBreitengrad\tLaengengrad\tSammler\tTag\tMonat\tJahr\tBild\t";
                for (int i = 0; i < this.AnalysisList.Count; i++)
                {
                    this.textBox.Text += this.AnalysisList[i] + "\t";
                }
                this.textBox.Text += "ID\r\n";
                System.IO.StreamWriter sw;
                sw = new System.IO.StreamWriter(this.textBoxExportFile.Text, false, System.Text.Encoding.UTF8);
                sw.WriteLine(this.textBox.Text);
                this.textBox.Text += "\r\n";
                if (_IDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select at least one item for the export");
                    return false;
                }
                string IDlist = "";
                foreach (int i in _IDs)
                    IDlist += i.ToString() + ", ";
                IDlist = IDlist.Substring(0, IDlist.Length - 2);

                System.Collections.Generic.Dictionary<int, string> AnalysisDict = new Dictionary<int, string>();
                //string AnalysisFields = "";
                //try
                //{
                //    for (int i = 0; i < this.checkedListBoxAnalysis.CheckedItems.Count; i++)
                //    {
                //        string x = this.checkedListBoxAnalysis.CheckedItems[i].GetType().ToString();
                //    }
                //    //foreach (System.Object o in this.checkedListBoxAnalysis.CheckedItems)
                //    //{
                //    //    System.Data.DataRow R = (System.Data.DataRow)o;
                //    //    AnalysisDict.Add(int.Parse(R["AnalysisID"].ToString()), R["DisplayText"].ToString());
                //    //}
                //}
                //catch { }
                //if (AnalysisDict.Count > 0)
                //{
                //    foreach (System.Collections.Generic.KeyValuePair<int, string> KV in AnalysisDict)
                //        AnalysisFields += KV.Value + ", ";
                //}
                //if (AnalysisFields.Length > 0 && AnalysisFields.EndsWith(", "))
                //    AnalysisFields = AnalysisFields.Substring(0, AnalysisFields.Length - 2);

                string SQL = "SELECT U.LastIdentificationCache AS Art, " +
                    " L.AverageLatitudeCache AS Breitengrad, L.AverageLongitudeCache AS Laengengrad, " +
                    " A.CollectorsName AS Sammler, " +
                    " E.CollectionDay AS Tag, E.CollectionMonth AS Monat, E.CollectionYear AS Jahr, " +
                    " MIN(CASE WHEN I.URI IS NULL THEN '' ELSE I.URI END) AS Bild, " +
                    this.AnalysisFields +
                    " U.IdentificationUnitID AS ID " +
                    " FROM CollectionProject AS P INNER JOIN " +
                    " CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                    " IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID INNER JOIN " +
                    " CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                    " CollectionAgent AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID INNER JOIN " +
                    " CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID LEFT OUTER JOIN " +
                    " CollectionSpecimenImage AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID " +
                    " WHERE P.ProjectID = " + this._ProjectID.ToString() + 
                    " AND (U.TaxonomicGroup IN (N" + this.TaxonomicGroups +
                    " )) AND (L.LocalisationSystemID = 8) " +
                    " AND S.CollectionSpecimenID IN (" + IDlist + ")" +
                    " GROUP BY U.LastIdentificationCache, L.AverageLatitudeCache, L.AverageLongitudeCache, A.CollectorsName, E.CollectionDay, E.CollectionMonth, E.CollectionYear, " +
                    " U.IdentificationUnitID " +
                    " HAVING (L.AverageLatitudeCache <> 0) AND (L.AverageLongitudeCache <> 0)";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(300000));
                ad.Fill(dt);
                this.progressBar.Maximum = dt.Rows.Count;
                this.progressBar.Value = 0;
                System.Data.DataTable dtMTB = new DataTable();
                System.Data.DataTable dtAnalysis = new DataTable();
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    this.setAnalysisResult(R);
                    string line = R["Art"].ToString() + "\t"
                        + R["Breitengrad"].ToString() + "\t"
                        + R["Laengengrad"].ToString() + "\t"
                        + R["Sammler"].ToString() + "\t"
                        + R["Tag"].ToString() + "\t"
                        + R["Monat"].ToString() + "\t"
                        + R["Jahr"].ToString() + "\t"
                        + R["Bild"].ToString().Replace("\r\n", " ") + "\t";
                    for (int i = 0; i < this.AnalysisList.Count; i++)
                    {
                        line += R[this.AnalysisList[i]].ToString() + "\t";
                    }
                    line += R["ID"].ToString();
                    this.textBox.Text += line + "\r\n";
                    sw.WriteLine(line);
                    line = "";
                    this.progressBar.Value++;
                }
                sw.Flush();
                sw.Close();
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private string TaxonomicGroups
        {
            get
            {
                if (this.checkedListBoxTaxonomicGroups.CheckedItems.Count > 0)
                {
                    string T = "";
                    for (int i = 0; i < this.checkedListBoxTaxonomicGroups.CheckedItems.Count; i++)
                    {
                        if (i > 0) T += ",";
                        T += "'" + this.checkedListBoxTaxonomicGroups.CheckedItems[i].ToString() + "'";
                    }
                    return T;
                }
                else return "";
            }
        }

        private string AnalysisFields
        {
            get
            {
                string AnalysisFields = "";
                try
                {
                    for (int i = 0; i < this.AnalysisList.Count; i++)
                    {
                        AnalysisFields += " '' AS [" + this.AnalysisList[i] + "], ";
                    }
                }
                catch { }
                return AnalysisFields;
            }
        }

        private System.Collections.Generic.List<string> AnalysisList
        {
            get
            {
                System.Collections.Generic.List<string> _List = new List<string>();
                try
                {
                    for (int i = 0; i < this.checkedListBoxAnalysis.CheckedItems.Count; i++)
                    {
                        _List.Add(this.checkedListBoxAnalysis.CheckedItems[i].ToString());
                    }
                }
                catch { }
                return _List;
            }
        }

        private void setAnalysisResult(System.Data.DataRow R)
        {
            try
            {
                string SQL = "";
                string IdentificationUnitID = R["ID"].ToString();
                string AnalysisResult = "";
                string AnalysisID = "";
                for (int i = 0; i < this.AnalysisList.Count; i++)
                {
                    string Selection = "DisplayText = '" + this.AnalysisList[i].ToString() + "'";
                    System.Data.DataRow[] RR = this._dtAnalysis.Select(Selection, "DisplayText");
                    if (RR.Length == 1)
                    {
                        AnalysisID = RR[0]["AnalysisID"].ToString();
                        SQL = "SELECT MAX(UA.AnalysisResult) AS AnalysisResult " +
                            "FROM IdentificationUnitAnalysis AS UA INNER JOIN " +
                            "Analysis AS A ON UA.AnalysisID = A.AnalysisID " +
                            "WHERE UA.IdentificationUnitID = " + IdentificationUnitID + " " +
                            "AND A.AnalysisID = " + AnalysisID + " " +
                            "GROUP BY A.AnalysisID";
                        AnalysisResult = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }
                    R[this.AnalysisList[i]] = AnalysisResult;
                }
            }
            catch { }
        }

    }
}
