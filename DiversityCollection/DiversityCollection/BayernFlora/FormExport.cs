using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.BayernFlora
{
    public partial class FormExport : Form, InterfaceExport
    {
        public FormExport()
        {
            InitializeComponent();
            Taxa.InitCacheDB();
            this.initForm();
        }

        private void initForm()
        {
            this.setProjectSource();
            this.setAnalysisSource();
            this.textBoxTaxaCount.Text = Taxa.Count().ToString();
        }

        public void setProgress(int Value)
        {
            if (this.progressBar.Maximum >= Value)
            {
                this.progressBar.Value = Value;
                if (Value % 100 == 0)
                {
                    this.textBoxTaxaCount.Text = Value.ToString();
                    System.Windows.Forms.Application.DoEvents();
                }
            }
        }

        public void setMax(int Value)
        {
            this.progressBar.Maximum = Value;
        }


        private void setProjectSource()
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            this.comboBoxProject.DataSource = dt;
            this.comboBoxProject.DisplayMember = "Project";
            this.comboBoxProject.ValueMember = "ProjectID";
        }

        private void setAnalysisSource()
        {
            this.comboBoxAnalysis.DataSource = Taxa.DtAnalysisFromCacheDB();
            this.comboBoxAnalysis.DisplayMember = "DisplayText";
            this.comboBoxAnalysis.ValueMember = "AnalysisID";
        }

        private void comboBoxAnalysis_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT AnalysisValue FROM TaxonAnalysisCategoryValue AS A WHERE (AnalysisID = " + this.comboBoxAnalysis.SelectedValue.ToString() + ") ORDER BY AnalysisValue";
            string Message = "";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
            this.checkedListBoxAnalysis.Items.Clear();
            foreach (System.Data.DataRow R in dt.Rows)
            {
                this.checkedListBoxAnalysis.Items.Add(R[0].ToString(), false);
            }
        }

        private System.Data.DataTable _DtTaxCount;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.checkBoxUseAnalysis.Checked 
                && this.checkedListBoxAnalysis.CheckedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select at least one analysis result from the list");
                return;
            }
            this.progressBar.Value = 0;
            if (this._AnalysisHasChanged)
            {
                this.labelMessage.Text = "Erfassung der Taxa";
                System.Windows.Forms.Application.DoEvents();
                if (this.checkBoxUseAnalysis.Checked)
                {
                    System.Collections.Generic.List<string> AnalysisResults = new List<string>();
                    foreach (System.Object o in this.checkedListBoxAnalysis.CheckedItems)
                    {
                        AnalysisResults.Add(o.ToString());
                    }
                    this.textBoxTaxaCount.Text = Taxa.InitTaxa(this, int.Parse(this.comboBoxAnalysis.SelectedValue.ToString()), AnalysisResults).ToString();
                }
                else
                {
                    this.textBoxTaxaCount.Text = Taxa.InitTaxa(this).ToString();
                }
            }
            else if (Taxa.Count() == 0)
            {
                this.labelMessage.Text = "Erfassung der Taxa";
                this.textBoxTaxaCount.Text = Taxa.InitTaxa(this).ToString();
            }
            System.Windows.Forms.Application.DoEvents();
            this.labelMessage.Text = "Suche nach Quadranten";
            System.Windows.Forms.Application.DoEvents();
            string SQL = "SELECT DISTINCT L.Location1 AS TK25, SUBSTRING(L.Location2, 1, 1) AS Quadrant, 0 AS Taxa " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                "CollectionEventLocalisation AS L ON S.CollectionEventID = L.CollectionEventID INNER JOIN " +
                "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID " + 
                "WHERE (L.LocalisationSystemID = 3) " +
                "AND (P.ProjectID = " + this.comboBoxProject.SelectedValue.ToString() + ") " +
                "AND case when E.CollectionYear is null then E.CollectionEndYear else " +
                " case when E.CollectionEndYear is null then E.CollectionYear else " +
                " cast((E.CollectionEndYear + E.CollectionYear) / 2 as int) end end " + 
                "BETWEEN " + this.numericUpDownYearFrom.Value.ToString() + " AND " + this.numericUpDownYearUntil.Value.ToString();
            string Message = "";
            this._DtTaxCount = new DataTable();
            this.dataGridView.SuspendLayout();
            this.dataGridView.DataSource = null;
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtTaxCount, ref Message);
            this.labelMessage.Text = this._DtTaxCount.Rows.Count.ToString() + " Quadranten gefunden";
            this.progressBar.Maximum = this._DtTaxCount.Rows.Count;
            this.progressBar.Value = 0;
            foreach (System.Data.DataRow R in this._DtTaxCount.Rows)
            {
                this.labelMessage.Text = "Auswertung für " + R[0].ToString() + "/" + R[1].ToString();
                System.Windows.Forms.Application.DoEvents();
                R[2] = this.Auswertung(R[0].ToString(), R[1].ToString());
                this.progressBar.Value++;
            }
            this.dataGridView.DataSource = this._DtTaxCount;
            this.dataGridView.ResumeLayout();
        }

        private int Auswertung(string TK, string Quadrant)
        {
            // getting all taxa for the Quadrant
            int iTax = 0;
            string baseUrl = global::DiversityCollection.Properties.Settings.Default.TNTTaxonName_Plants_Url;
            string SQL = "DECLARE @BaseURL varchar(50); " +
                "SET @BaseURL = " + baseUrl + "; " +
                "SELECT DISTINCT rtrim(substring(I.NameURI, len(@BaseURL) + 2, 255)) " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                "CollectionEventLocalisation AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID INNER JOIN " +
                "Identification AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID AND I.IdentificationUnitID = U.IdentificationUnitID " +
                "WHERE (E.LocalisationSystemID = 3) AND (P.ProjectID = " + this.comboBoxProject.SelectedValue.ToString() + ") " +
                "AND (SUBSTRING(E.Location2, 1, 1) = '" + Quadrant + "') AND (E.Location1 = N'" + TK + "') AND (I.NameURI LIKE @BaseURL + '%') " +
                "AND exists (select * from Identification L where I.IdentificationUnitID = L.IdentificationUnitID and I.CollectionSpecimenID = L.CollectionSpecimenID " +
                " group by L.IdentificationUnitID having I.IdentificationSequence = max(L.IdentificationSequence))";
            System.Data.DataTable dtTaxa = new DataTable();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTaxa, ref Message);

            // removing duplicates defined by the hierarchy
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>> Taxa = new Dictionary<int, List<int>>();
            foreach (System.Data.DataRow R in dtTaxa.Rows)
            {
                // ignore empty
                if (R[0].Equals(System.DBNull.Value) || R[0].ToString().Length == 0)
                {
                    continue;
                }
                int NameID;
                if (int.TryParse(R[0].ToString(), out NameID))
                {
                    if (!BayernFlora.Taxa.TaxonInList(NameID))
                        continue;
                    System.Collections.Generic.List<int> SubTaxa = BayernFlora.Taxa.SubNameID(NameID);
                    bool TaxonContained = false;
                    foreach (int ID in SubTaxa)
                    {
                        if (Taxa.ContainsKey(ID))
                        {
                            TaxonContained = true;
                            break;
                        }
                    }
                    if (TaxonContained)
                        continue;
                    Taxa.Add(NameID, SubTaxa);
                }
                else
                {
                }
            } 
            System.Collections.Generic.List<int> TaxaToRemove = new List<int>();
            foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.List<int>> KV in Taxa)
            {
                foreach (int ID in KV.Value)
                {
                    if (Taxa.ContainsKey(ID))
                        TaxaToRemove.Add(ID);
                }
            }
            foreach (int T in TaxaToRemove)
            {
                if (Taxa.ContainsKey(T))
                    Taxa.Remove(T);
            }
            iTax = Taxa.Count;
            return iTax;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter sw = null;
            try
            {
                string ExportFile = Folder.Export()  + "BayernFlora_Export.txt";
                sw = new System.IO.StreamWriter(ExportFile, false, System.Text.Encoding.UTF8);
                string Line = "";
                // Header
                foreach (System.Data.DataColumn DC in this._DtTaxCount.Columns)
                {
                    Line += DC.ColumnName + "\t";
                }
                sw.WriteLine(Line);
                //Data
                foreach (System.Data.DataRow R in this._DtTaxCount.Rows)
                {
                    Line = "";
                    foreach (System.Data.DataColumn DC in this._DtTaxCount.Columns)
                    {
                        Line += R[DC.ColumnName].ToString() + "\t";
                    }
                    sw.WriteLine(Line);
                }
                System.Windows.Forms.MessageBox.Show("Data exported to " + ExportFile);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }

        }

        private void buttonAnalysisAll_Click(object sender, EventArgs e)
        {
            foreach (System.Object o in this.checkedListBoxAnalysis.Items)
            {
            }
        }

        private void buttonAnalysisNone_Click(object sender, EventArgs e)
        {

        }

        private bool _AnalysisHasChanged = false;

        private void checkBoxUseAnalysis_CheckedChanged(object sender, EventArgs e)
        {
            this._AnalysisHasChanged = true;
            if (checkBoxUseAnalysis.Checked)
            {
                this.comboBoxAnalysis.Enabled = true;
                this.checkedListBoxAnalysis.Enabled = true;
            }
            else
            {
                this.comboBoxAnalysis.Enabled = false;
                this.checkedListBoxAnalysis.Enabled = false;
            }
        }


        private void checkedListBoxAnalysis_Click(object sender, EventArgs e)
        {
            this._AnalysisHasChanged = true;
        }

    }
}
