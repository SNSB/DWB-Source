using DiversityWorkbench.DwbManual;
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
    public partial class FormExportBIB : Form
    {

        #region Parameter
        private System.Collections.Generic.List<int> _IDs;
        private System.Data.DataTable _dtKopfIDs;
        
        #endregion

        #region Construction, Form and Button Events
        /// <summary>
        /// Export der fuer den Botanischen Informationsknoten Bayern relevanten Felder:
        /// Taxnr aus DiversityTaxonNames
        /// Quadrant
        /// Datum t m j
        /// Floristischer status
        /// Fundort
        /// Finder
        /// </summary>
        public FormExportBIB(System.Collections.Generic.List<int> IDs)
        {
            InitializeComponent();

            try
            {
                this._IDs = IDs;
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
                System.Uri U = new Uri("http://www.bayernflora.de/de/index.php");
                this.webBrowserBIB.Url = U;
                this.textBoxExportFile.Text = Folder.Export() + "BIB_Kopfdaten.txt";
                this.textBoxExportFileUnits.Text = Folder.Export() + "BIB_Sippendaten.txt";
            }
            catch (Exception ex)
            {
            }
        }

        private void buttonStartExport_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (!this.ExportData())
                System.Windows.Forms.MessageBox.Show("An error occured while generating the export");

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

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

        private void buttonOpenFileUnit_Click(object sender, EventArgs e)
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
                    this.textBoxExportFileUnits.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Export

        private bool ExportData()
        {
            bool OK = true;
            this.labelHeader.Text = "Creating the header file";

            string SQL = "SELECT DISTINCT E.CollectionEventID AS KopfID " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                "Identification AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID INNER JOIN " +
                "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID " +
                "WHERE (L.LocalisationSystemID = 8) AND (NOT (L.Geography IS NULL))" +
                "AND S.CollectionSpecimenID IN (" + this.IDlist + ") ORDER BY E.CollectionEventID";
            this._dtKopfIDs = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(300000));
            ad.Fill(this._dtKopfIDs);

            this.progressBar.Value = 0;
            this.progressBar.Maximum = this._dtKopfIDs.Rows.Count;

            System.IO.StreamWriter swKopf;
            swKopf = new System.IO.StreamWriter(this.textBoxExportFile.Text, false, System.Text.Encoding.UTF8);

            System.IO.StreamWriter swSippen;
            swSippen = new System.IO.StreamWriter(this.textBoxExportFileUnits.Text, false, System.Text.Encoding.UTF8);

            this.InitHeaederFile(swKopf);
            this.InitUnitFile(swSippen);
            try
            {
                foreach (System.Data.DataRow R in this._dtKopfIDs.Rows)
                {
                    int CollectionEventID;
                    if (int.TryParse(R[0].ToString(), out CollectionEventID))
                    {
                        if (this.AddHeaderData(CollectionEventID, swKopf))
                        {
                            if (!this.AddUnitData(CollectionEventID, swSippen))
                                OK = false;
                        }
                        else OK = false;
                    }
                    this.progressBar.Value++;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            swKopf.Flush();
            swKopf.Close();
            swSippen.Flush();
            swSippen.Close();

            if (OK) this.labelHeader.Text = "Export finished";

            return OK;
        }

        private string IDlist
        {
            get
            {
                if (_IDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select at least one item for the export");
                    return "";
                }
                string IDlist = "";
                foreach (int i in _IDs)
                    IDlist += i.ToString() + ", ";
                IDlist = IDlist.Substring(0, IDlist.Length - 2);
                return IDlist;
            }
        }

        private bool InitHeaederFile(System.IO.StreamWriter sw)
        {
            this.textBox.Text = "ID-Kopf\tMTB\tQuadrant\tTag\tMonat\tJahr\tFundort\tFinder\r\n";
            sw.WriteLine(this.textBox.Text);
            return true;
        }

        private bool InitUnitFile(System.IO.StreamWriter sw)
        {
            this.textBoxUnit.Text = "ID-Kopf\tTaxNr\tFloristischerStatus\r\n";
            sw.WriteLine(this.textBoxUnit.Text);
            return true;
        }

        private bool AddHeaderData(int CollectionEventID, System.IO.StreamWriter sw)
        {
            bool OK = true;
            try
            {
                this.textBox.Text += "\r\n";

                string SQL = "SELECT DISTINCT E.CollectionEventID AS KopfID, '' AS MTB, '' AS Quadrant, E.CollectionDay AS Tag, E.CollectionMonth AS Monat, E.CollectionYear AS Jahr, " +
                    "E.LocalityDescription AS Fundort, I.ResponsibleName AS Finder " +
                    "FROM CollectionSpecimen AS S INNER JOIN " +
                    "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                    "Identification AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID INNER JOIN " +
                    "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID " +
                    "WHERE (L.LocalisationSystemID = 8) AND (NOT (L.Geography IS NULL))" +
                    "AND E.CollectionEventID = " + CollectionEventID.ToString();
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(300000));
                ad.Fill(dt);
                //this.progressBar.Maximum = dt.Rows.Count;
                System.Data.DataTable dtMTB = new DataTable();
                System.Data.DataTable dtAnalysis = new DataTable();
                SQL = "SELECT SUBSTRING(Q.PlotIdentifier, 6, 4) AS MTB, SUBSTRING(Q.PlotIdentifier, 11, 1) AS Quadrant " +
                    "FROM CollectionSpecimen AS S INNER JOIN " +
                    "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                    "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID CROSS JOIN " +
                    "DiversitySamplingPlots.dbo.SamplingPlot AS Q INNER JOIN " +
                    "DiversitySamplingPlots.dbo.SamplingProject AS P ON Q.PlotID = P.PlotID " +
                    "WHERE (P.ProjectID = 908) AND (Q.PlotIdentifier LIKE N'%/_') AND (L.Geography.STDisjoint(Q.PlotGeography_Cache) = 0) AND (L.LocalisationSystemID = 8) AND " +
                    "(NOT (L.Geography IS NULL)) AND S.CollectionEventID = " + CollectionEventID.ToString();
                dtMTB.Clear();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dtMTB);
                if (dtMTB.Rows.Count > 0)
                {
                    dt.Rows[0]["MTB"] = dtMTB.Rows[0]["MTB"].ToString();
                    dt.Rows[0]["Quadrant"] = dtMTB.Rows[0]["Quadrant"].ToString();
                }
                if (dt.Rows[0]["MTB"].ToString().Length > 0)
                {
                    string line = dt.Rows[0]["KopfID"].ToString() + "\t"
                        + dt.Rows[0]["MTB"].ToString() + "\t"
                        + dt.Rows[0]["Quadrant"].ToString() + "\t"
                        + dt.Rows[0]["Tag"].ToString() + "\t"
                        + dt.Rows[0]["Monat"].ToString() + "\t"
                        + dt.Rows[0]["Jahr"].ToString() + "\t"
                        + dt.Rows[0]["Fundort"].ToString().Replace("\r\n", " ") + "\t"
                        + dt.Rows[0]["Finder"].ToString();
                    this.textBox.Text += line + "\r\n";
                    sw.WriteLine(line);
                    line = "";
                }
                //this.progressBar.Value++;
                //sw.Flush();
                //sw.Close();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                this.labelHeader.Text = "Error while creating the export of the headers";
                OK = false;
            }
            
            return OK;
        }

        private bool AddUnitData(int CollectionEventID, System.IO.StreamWriter sw)
        {
            bool OK = true;
            try
            {
                this.textBoxUnit.Text += "\r\n";

                string SQL = "SELECT E.CollectionEventID AS KopfID, Ex.ExternalNameURI AS TaxNr, ''  AS FloristischerStatus,  S.CollectionSpecimenID AS ID, I.IdentificationUnitID AS UnitID " +
                    "FROM CollectionSpecimen AS S INNER JOIN " +
                    "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                    "Identification AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID INNER JOIN " +
                    "DiversityTaxonNames_Plants.dbo.TaxonName AS T INNER JOIN " +
                    "DiversityTaxonNames_Plants.dbo.TaxonNameExternalID AS Ex ON T.NameID = Ex.NameID ON I.NameURI = DiversityTaxonNames_Plants.dbo.BaseURL() " +
                    " + CAST(T.NameID AS varchar) INNER JOIN " +
                    "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID " +
                    "WHERE (Ex.ExternalDatabaseID = 142) AND (L.LocalisationSystemID = 8) AND (NOT (L.Geography IS NULL))" +
                    "AND E.CollectionEventID = " + CollectionEventID.ToString();
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(300000));
                ad.Fill(dt);

                System.Data.DataTable dtAnalysis = new DataTable();

                foreach (System.Data.DataRow R in dt.Rows)
                {
                    SQL = "SELECT A.AnalysisResult AS FloristischerStatus " +
                        "FROM IdentificationUnitAnalysis AS A INNER JOIN " +
                        "Identification AS I ON A.CollectionSpecimenID = I.CollectionSpecimenID AND A.IdentificationUnitID = I.IdentificationUnitID " +
                        "WHERE (A.AnalysisID = 600) AND I.IdentificationUnitID = " + R["UnitID"].ToString();
                    dtAnalysis.Clear();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtAnalysis);
                    if (dtAnalysis.Rows.Count > 0)
                    {
                        R["FloristischerStatus"] = dtAnalysis.Rows[0]["FloristischerStatus"].ToString();
                    }
                    string line = R["KopfID"].ToString() + "\t"
                        + R["TaxNr"].ToString() + "\t"
                        + R["FloristischerStatus"];
                    this.textBoxUnit.Text += line + "\r\n";
                    sw.WriteLine(line);
                    line = "";
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                System.Windows.Forms.MessageBox.Show(ex.Message);
                this.labelHeader.Text = "Error while creating the export of the taxa";
                return OK;
            }
            return OK;
        }
        
        #endregion

        #region Old functions

        private bool GenerateHeaederFile()
        {
            bool OK = true;
            try
            {
                this.labelHeader.Text = "Creating the header file";
                this.progressBar.Value = 0;
                this.textBox.Text = "ID-Kopf\tMTB\tQuadrant\tTag\tMonat\tJahr\tFundort\tFinder\r\n";
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

                string SQL = "SELECT DISTINCT E.CollectionEventID AS KopfID, '' AS MTB, '' AS Quadrant, E.CollectionDay AS Tag, E.CollectionMonth AS Monat, E.CollectionYear AS Jahr, " +
                    "E.LocalityDescription AS Fundort, I.ResponsibleName AS Finder " +
                    "FROM CollectionSpecimen AS S INNER JOIN " +
                    "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                    "Identification AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID INNER JOIN " +
                    "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID " +
                    "WHERE (L.LocalisationSystemID = 8) AND (NOT (L.Geography IS NULL))" +
                    "AND S.CollectionSpecimenID IN (" + IDlist + ") ORDER BY E.CollectionEventID";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(300000));
                ad.Fill(dt);
                //this.progressBar.Maximum = dt.Rows.Count;
                System.Data.DataTable dtMTB = new DataTable();
                System.Data.DataTable dtAnalysis = new DataTable();
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    SQL = "SELECT SUBSTRING(Q.PlotIdentifier, 6, 4) AS MTB, SUBSTRING(Q.PlotIdentifier, 11, 1) AS Quadrant " +
                        "FROM CollectionSpecimen AS S INNER JOIN " +
                        "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                        "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID CROSS JOIN " +
                        "DiversitySamplingPlots.dbo.SamplingPlot AS Q INNER JOIN " +
                        "DiversitySamplingPlots.dbo.SamplingProject AS P ON Q.PlotID = P.PlotID " +
                        "WHERE (P.ProjectID = 908) AND (Q.PlotIdentifier LIKE N'%/_') AND (L.Geography.STDisjoint(Q.PlotGeography_Cache) = 0) AND (L.LocalisationSystemID = 8) AND " +
                        "(NOT (L.Geography IS NULL)) AND S.CollectionEventID = " + R["KopfID"].ToString();
                    dtMTB.Clear();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtMTB);
                    if (dtMTB.Rows.Count > 0)
                    {
                        R["MTB"] = dtMTB.Rows[0]["MTB"].ToString();
                        R["Quadrant"] = dtMTB.Rows[0]["Quadrant"].ToString();
                    }
                    if (R["MTB"].ToString().Length > 0)
                    {
                        string line = R["KopfID"].ToString() + "\t"
                            + R["MTB"].ToString() + "\t"
                            + R["Quadrant"].ToString() + "\t"
                            + R["Tag"].ToString() + "\t"
                            + R["Monat"].ToString() + "\t"
                            + R["Jahr"].ToString() + "\t"
                            + R["Fundort"].ToString().Replace("\r\n", " ") + "\t"
                            + R["Finder"].ToString();
                        this.textBox.Text += line + "\r\n";
                        sw.WriteLine(line);
                        line = "";
                    }
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

        private bool GenerateUnitFile()
        {
            bool OK = true;
            try
            {
                this.labelHeader.Text = "Creating the taxa file";
                this.progressBar.Value = 0;
                this.textBoxUnit.Text = "ID-Kopf\tTaxNr\tFloristischerStatus\r\n";
                System.IO.StreamWriter sw;
                sw = new System.IO.StreamWriter(this.textBoxExportFileUnits.Text, false, System.Text.Encoding.UTF8);
                sw.WriteLine(this.textBoxUnit.Text);
                this.textBoxUnit.Text += "\r\n";
                if (_IDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please select at least one item for the export");
                    return false;
                }
                string IDlist = "";
                foreach (int i in _IDs)
                    IDlist += i.ToString() + ", ";
                IDlist = IDlist.Substring(0, IDlist.Length - 2);

                string SQL = "SELECT E.CollectionEventID AS KopfID, Ex.ExternalNameURI AS TaxNr, ''  AS FloristischerStatus,  S.CollectionSpecimenID AS ID, I.IdentificationUnitID AS UnitID " +
                    "FROM CollectionSpecimen AS S INNER JOIN " +
                    "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                    "Identification AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID INNER JOIN " +
                    "DiversityTaxonNames_Plants.dbo.TaxonName AS T INNER JOIN " +
                    "DiversityTaxonNames_Plants.dbo.TaxonNameExternalID AS Ex ON T.NameID = Ex.NameID ON I.NameURI = DiversityTaxonNames_Plants.dbo.BaseURL() " +
                    " + CAST(T.NameID AS varchar) INNER JOIN " +
                    "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID " +
                    "WHERE (Ex.ExternalDatabaseID = 142) AND (L.LocalisationSystemID = 8) AND (NOT (L.Geography IS NULL))" +
                    "AND S.CollectionSpecimenID IN (" + IDlist + ") --ORDER BY E.CollectionEventID"; // Order By fuehrt zu Timeout
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(300000));
                ad.Fill(dt);
                this.progressBar.Maximum = dt.Rows.Count;
                //System.Data.DataTable dtMTB = new DataTable();
                System.Data.DataTable dtAnalysis = new DataTable();
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    SQL = "SELECT A.AnalysisResult AS FloristischerStatus " +
                        "FROM IdentificationUnitAnalysis AS A INNER JOIN " +
                        "Identification AS I ON A.CollectionSpecimenID = I.CollectionSpecimenID AND A.IdentificationUnitID = I.IdentificationUnitID " +
                        "WHERE (A.AnalysisID = 600) AND I.IdentificationUnitID = " + R["UnitID"].ToString();
                    dtAnalysis.Clear();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtAnalysis);
                    if (dtAnalysis.Rows.Count > 0)
                    {
                        R["FloristischerStatus"] = dtAnalysis.Rows[0]["FloristischerStatus"].ToString();
                    }
                    string line = R["KopfID"].ToString() + "\t"
                        + R["TaxNr"].ToString() + "\t"
                        + R["FloristischerStatus"];
                    this.textBoxUnit.Text += line + "\r\n";
                    sw.WriteLine(line);
                    line = "";
                    this.progressBar.Value++;
                }
                //System.Data.DataView V = new DataView(dt, "", "KopfID", DataViewRowState.Unchanged);
                //for (int i = 0; i < V.Table.Rows.Count; i++)
                //{
                //}
                //foreach (System.Data.DataRow R in V.Table.Rows)
                //{
                //    string line = R["KopfID"].ToString() + "\t"
                //        + R["TaxNr"].ToString() + "\t"
                //        + R["FloristischerStatus"];
                //    this.textBoxUnit.Text += line + "\r\n";
                //    sw.WriteLine(line);
                //    line = "";
                //    this.progressBar.Value++;
                //}
                sw.Flush();
                sw.Close();
            }
            catch (System.Exception ex)
            {
                OK = false;
                System.Windows.Forms.MessageBox.Show(ex.Message);
                this.labelHeader.Text = "Error while creating the export of the taxa";
                return OK;
            }
            this.labelHeader.Text = "Export finished";
            return OK;
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
