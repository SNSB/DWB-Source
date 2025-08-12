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
    public partial class FormTaxonList : Form
    {
        #region Parameter

        private int _CollectionSpecimenID;
        private int _CollectionEventID;
        private int _SeriesID;
        private string _PlotURI;
        private System.Collections.Generic.List<int> _CollectionEventIDsInSeries;
        private System.Collections.Generic.List<string> _PlotURIsInPlot;
        private System.Collections.Generic.Dictionary<string, int> _Taxa;
        private enum TaxonListBase { Series, Event, Specimen, Plot };
        private TaxonListBase _TaxonListBase;
        private System.Data.DataRow _R;
        System.Data.DataTable _dtPlots;
        System.Data.DataTable _dtTaxa;

        private static bool _CountUnits = true;
        private static bool _IncludeGender = false;
        private static bool _IncludeLists = false;

        private string _AnalysisDescription = "";
        
        #endregion

        #region Construction and Form

        public FormTaxonList(System.Data.DataRow R)
        {
            InitializeComponent();
            this._R = R;
            this.initForm();
            this.DisplayList();
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }

        private void initForm()
        {
            Microsoft.Data.SqlClient.SqlConnection s = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTaxonomicGroup, "CollTaxonomicGroup_Enum", s, true, true);
            string Table = this._R.Table.TableName;
            switch (Table)
            {
                case "CollectionEventSeries":
                    this._TaxonListBase = TaxonListBase.Series;
                    string Header = " for series ";
                    if (this._R["SeriesCode"].Equals(System.DBNull.Value) || this._R["SeriesCode"].ToString().Length == 0)
                        Header += this._R[2].ToString();
                    else Header += this._R["SeriesCode"].ToString();
                    this.labelHeader.Text += Header; 
                    break;
                case "CollectionEventList":
                    this._TaxonListBase = TaxonListBase.Event;
                    this.labelHeader.Text += " for colletion event " + this._R["DisplayText"].ToString();
                    break;
                case "CollectionEvent":
                    this._TaxonListBase = TaxonListBase.Event;
                    this.labelHeader.Text += " for colletion event " + this._R["LocalityDescription"].ToString();
                    break;
                case "CollectionSpecimenList":
                case "CollectionSpecimen":
                    this._TaxonListBase = TaxonListBase.Specimen;
                    this.labelHeader.Text += " for collection specimen " + this._R["AccessionNumber"].ToString();
                    break;
                case "SamplingPlot":
                    DiversityWorkbench.SamplingPlot S = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                    this._TaxonListBase = TaxonListBase.Plot;
                    int ID;
                    if (int.TryParse(this._R["PlotID"].ToString().Substring(S.ServerConnection.BaseURL.Length), out ID))
                    {
                        this._dtPlots = S.SamplingPlotChildHierarchy(ID);
                        this.labelHeader.Text += " for sampling plot " + this._R["DisplayText"].ToString();
                    }
                    else
                    {
                        string URI = this._R["PlotID"].ToString();
                        if (int.TryParse(DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI), out ID))
                        {
                        }
                    }
                    break;
            }
            switch (this._TaxonListBase)
            {
                case TaxonListBase.Series:
                    if (this._R.Table.Columns.Contains("SeriesID"))
                        this._SeriesID = int.Parse(this._R["SeriesID"].ToString());
                    this.pictureBoxSource.Image = DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImage.EventSeries];
                    break;
                case TaxonListBase.Event:
                    this._CollectionEventID = int.Parse(this._R["CollectionEventID"].ToString());
                    this.pictureBoxSource.Image = DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImage.Event];
                    break;
                case TaxonListBase.Specimen:
                    this._CollectionSpecimenID = int.Parse(this._R["CollectionSpecimenID"].ToString());
                    this.pictureBoxSource.Image = DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImage.Specimen];
                    break;
                case TaxonListBase.Plot:
                    this._PlotURI = this._R["PlotID"].ToString();
                    this.pictureBoxSource.Image = DiversityCollection.Specimen.ImageList.Images[(int)DiversityCollection.Specimen.OverviewImage.SamplingPlot];
                    break;
            }
            this.checkBoxIncludeGender.Checked = _IncludeGender;
            this.checkBoxIncludeTaxonLists.Checked = _IncludeLists;
            this.radioButtonCountUnits.Checked = _CountUnits;
            this.radioButtonCountNumberOfUnits.Checked = !_CountUnits;
        }

        #endregion

        #region Events
        private System.Collections.Generic.List<int> CollectionEventIDsInSeries()
        {
            if (this._CollectionEventIDsInSeries == null)
                this._CollectionEventIDsInSeries = new List<int>();
            try
            {
                this._CollectionEventIDsInSeries.Clear();
                string SQL = "SELECT SeriesID FROM [dbo].[EventSeriesChildNodes] (" + this._SeriesID.ToString() + ") AS S";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                string SeriesIDs = this._SeriesID.ToString();
                foreach (System.Data.DataRow R in dt.Rows)
                    SeriesIDs += ", " + R[0].ToString();

                SQL = "SELECT CollectionEventID FROM CollectionEvent WHERE SeriesID IN ( " + SeriesIDs + ")";
                System.Data.DataTable dtEvent = new DataTable();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dtEvent);
                foreach (System.Data.DataRow R in dtEvent.Rows)
                    this._CollectionEventIDsInSeries.Add(int.Parse(R[0].ToString()));
            }
            catch { } 
            return this._CollectionEventIDsInSeries;
        }

        private System.Collections.Generic.List<string> PlotURIsInPlot(string PlotURI)
        {
            if (this._PlotURIsInPlot == null)
                this._PlotURIsInPlot = new List<string>();
            try
            {
                if (!this._PlotURIsInPlot.Contains(PlotURI))
                {
                    this._PlotURIsInPlot.Add(PlotURI);
                }
                foreach (System.Data.DataRow R in this._dtPlots.Rows)
                {
                    if (!this._PlotURIsInPlot.Contains(R[0].ToString()))
                    {
                        this._PlotURIsInPlot.Add(R[0].ToString());
                    }
                }
            }
            catch { }
            return this._PlotURIsInPlot;
        }

        private void FillTaxa()
        {
            string TimeRestriction = this.TimeRestriction();
            string SQL = "SELECT U.TaxonomicGroup AS [Group], U.LastIdentificationCache AS [Taxon] ";
            if (this.checkBoxIncludeGender.Checked)
            {
                SQL += ", U.Gender AS Sex ";
            }
            if (this.checkBoxIncludeTaxonLists.Checked && this.ProjectID != null && this.AnalysisID != null)
            {
                SQL += ", A.AnalysisValue AS Analysis ";
            }
            SQL += ", ";
            if (this.radioButtonCountNumberOfUnits.Checked)
                SQL += "SUM(U.NumberOfUnits) ";
            else
                SQL += "COUNT(*)";
            SQL += " AS Number FROM IdentificationUnit AS U ";
            if (this.checkBoxIncludeTaxonLists.Checked && this.ProjectID != null && this.AnalysisID != null)
            {
                SQL += ", [FirstLinesIdentification] AS I ";
                SQL += ", " + this.TaxonNamesDatabase + ".dbo.TaxonNameListAnalysis AS A ";
            }
            bool isLinkedServer = false;
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
            {
                if (this.TaxonNamesDatabase.IndexOf(KV.Key) > -1)
                {
                    isLinkedServer = true;
                    break;
                }
            }
            if (isLinkedServer)
            {
                SQL += ", " + this.TaxonNamesDatabase + ".dbo.ViewBaseURL AS B ";
            }
            switch (this._TaxonListBase)
            { 
                case TaxonListBase.Series:
                    string EventIDs = "";
                    foreach (int i in this.CollectionEventIDsInSeries())
                    {
                        if (EventIDs.Length > 0) EventIDs += ", ";
                        EventIDs += i.ToString();
                    }
                    SQL += ", CollectionEvent AS E, " +
                    "CollectionSpecimen AS S " +
                    "WHERE (E.CollectionEventID IN ( " + EventIDs + ")) AND E.CollectionEventID = S.CollectionEventID AND  S.CollectionSpecimenID = U.CollectionSpecimenID ";
                    break;
                case TaxonListBase.Event:
                    SQL += ", CollectionEvent AS E, " +
                    "CollectionSpecimen AS S " +
                    "WHERE (E.CollectionEventID = " + this._CollectionEventID.ToString() + ") AND  E.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = U.CollectionSpecimenID ";
                    break;
                case TaxonListBase.Specimen:
                    SQL += ", CollectionSpecimen AS S  ";
                    if (TimeRestriction.Length > 0)
                        SQL += " INNER JOIN CollectionEvent AS E ON E.CollectionEventID = S.CollectionEventID ";
                    SQL += "WHERE (S.CollectionSpecimenID = " + this._CollectionSpecimenID.ToString() + ") AND S.CollectionSpecimenID = U.CollectionSpecimenID  ";
                    break;
                case TaxonListBase.Plot:
                    string PlotURIs = "";
                    foreach (string s in this.PlotURIsInPlot(this._PlotURI))
                    {
                        if (PlotURIs.Length > 0) PlotURIs += ",";
                        PlotURIs += "'" + s + "'";
                    }
                    SQL += ", CollectionEventLocalisation AS L, " +
                    "CollectionSpecimen AS S ";
                     if (TimeRestriction.Length > 0)
                        SQL += " INNER JOIN CollectionEvent AS E ON E.CollectionEventID = S.CollectionEventID ";
                    SQL += "WHERE (L.Location2 IN ( " + PlotURIs + ")) AND L.LocalisationSystemID = 13 AND L.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = U.CollectionSpecimenID ";
                    break;
            }
            if (this.checkBoxIncludeTaxonLists.Checked && this.ProjectID != null && this.AnalysisID != null)
            {
                SQL += " AND I.IdentificationUnitID = U.IdentificationUnitID AND NOT I.NameURI IS NULL ";
                if (isLinkedServer)
                    SQL += " AND  I.NameURI = B.BaseURL + CAST(A.NameID AS VARCHAR) ";
                else
                    SQL += " AND  I.NameURI = " + this.TaxonNamesDatabase + ".dbo.BaseURL() + CAST(A.NameID AS VARCHAR) ";
                SQL += " AND A.ProjectID = " + this.ProjectID.ToString() + " AND A.AnalysisID = " + this.AnalysisID.ToString();
            }
            if (this.comboBoxTaxonomicGroup.Text.Length > 0 && this.comboBoxTaxonomicGroup.SelectedIndex > 0)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxTaxonomicGroup.SelectedItem;
                SQL += " AND (U.TaxonomicGroup = N'" + R[0].ToString() + "') ";
            }
            SQL += TimeRestriction;
            SQL += " GROUP BY U.LastIdentificationCache, U.TaxonomicGroup ";
            if (this.checkBoxIncludeGender.Checked)
            {
                SQL += ", U.Gender ";
            }
            if (this.checkBoxIncludeTaxonLists.Checked && this.ProjectID != null && this.AnalysisID != null)
            {
                SQL += ", A.AnalysisValue ";
            }
            SQL += " ORDER BY U.TaxonomicGroup, U.LastIdentificationCache ";
            //if (this._dtTaxa == null)
            //    this._dtTaxa = new DataTable();
            //else this._dtTaxa.Clear();
            this._dtTaxa = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtTaxa);
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(this._dtTaxa);
        }

        private string TimeRestriction()
        {
            string SQL = "";
            if (this.checkBoxFrom.Checked)
            {
                switch (this._TaxonListBase)
                {
                    //case TaxonListBase.Specimen:
                    //case TaxonListBase.Plot:
                    //    SQL = " AND (S.LogCreatedWhen >= CONVERT(DATETIME, '" + this.dateTimePickerFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                    //    break;
                    default:
                        SQL = " AND (E.CollectionDate >= CONVERT(DATETIME, '" + this.dateTimePickerFrom.Value.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                        break;
                }
            }
            if (this.checkBoxUntil.Checked)
            {
                switch (this._TaxonListBase)
                {
                    //case TaxonListBase.Specimen:
                    //case TaxonListBase.Plot:
                    //    SQL += " AND (S.LogCreatedWhen <= CONVERT(DATETIME, '" + this.dateTimePickerUntil.Value.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                    //    break;
                    default:
                        SQL += " AND (E.CollectionDate <= CONVERT(DATETIME, '" + this.dateTimePickerUntil.Value.ToString("yyyy-MM-dd") + " 00:00:00', 102)) ";
                        break;
                }
            }
            return SQL;
        }

        private void DisplayList()
        {
            this.FillTaxa();
            if (this._dtTaxa.Rows.Count == 0 
                && !DiversityWorkbench.Settings.LoadConnections 
                && DiversityWorkbench.LinkedServer.LinkedServers().Count > 0 
                && DiversityWorkbench.LinkedServer.LinkedServers().First().Value.DatabaseList("DiversityTaxonNames").Count > 0)
            {
                System.Windows.Forms.MessageBox.Show("Please reload connections in Connection - Connection administration");
            }
            this.dataGridViewTaxa.DataSource = this._dtTaxa;
            this.dataGridViewTaxa.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            string Month = System.DateTime.Now.Month.ToString();
            if (Month.Length == 1) Month = "0" + Month;

            string Day = System.DateTime.Now.Day.ToString();
            if (Day.Length == 1) Day = "0" + Day;

            string Hour = System.DateTime.Now.Hour.ToString();
            if (Hour.Length == 1) Hour = "0" + Hour;

            string Minute = System.DateTime.Now.Minute.ToString();
            if (Minute.Length == 1) Minute = "0" + Minute;

            string Second = System.DateTime.Now.Second.ToString();
            if (Second.Length == 1) Second = "0" + Second;

            string GridViewExportFile = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.FolderType.Export) +  System.Windows.Forms.Application.ProductName.ToString()
                + "_TaxonList_" + System.DateTime.Now.Year.ToString() + Month + Day
                + "_" + Hour + Minute + Second + ".txt";
            this.textBoxTaxonListFile.Text = GridViewExportFile;
        }

        private void comboBoxTaxonomicGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string TaxonomicGroup = "";
            if (this.comboBoxTaxonomicGroup.SelectedIndex > -1)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxTaxonomicGroup.SelectedItem;
                TaxonomicGroup = R[0].ToString();
            }

            if (TaxonomicGroup.Length > 0)
            {
                this.checkBoxIncludeTaxonLists.Visible = true;
                this.pictureBoxTaxonomicGroup.Image = DiversityCollection.Specimen.TaxonImage(false, TaxonomicGroup);
            }
            else
            {
                this.checkBoxIncludeTaxonLists.Checked = false;
                this.checkBoxIncludeTaxonLists.Visible = false;
                this.pictureBoxTaxonomicGroup.Image = null;
                this.labelDatabase.Visible = false;
                this.comboBoxDatabase.Visible = false;
            }
        }

        private void checkBoxIncludeTaxonLists_CheckedChanged(object sender, EventArgs e)
        {
            _IncludeLists = this.checkBoxIncludeTaxonLists.Checked;
            this.setDatabaseSource();
            this.labelDatabase.Visible = this.checkBoxIncludeTaxonLists.Checked;
            this.comboBoxDatabase.Visible = this.checkBoxIncludeTaxonLists.Checked;
            this.labelAnalysis.Visible = false;
            this.labelTaxonLists.Visible = false;
            this.comboBoxTaxonList.Visible = false;
            this.comboBoxAnalysis.Visible = false;
        }

        private void comboBoxDatabase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.setTaxonListSource();
            this.comboBoxTaxonList.Visible = true;
            this.labelTaxonLists.Visible = true;
        }

        private void comboBoxTaxonList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.setAnalysisSource();
            this.labelAnalysis.Visible = true;
            this.comboBoxAnalysis.Visible = true;
        }

        private void setDatabaseSource()
        {
            foreach (string D in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].DatabaseList())
            {
                if (!this.comboBoxDatabase.Items.Contains(D))
                    this.comboBoxDatabase.Items.Add(D);
            }

            //string SQL = "SELECT     name " +
            //    "FROM         sys.databases AS d " +
            //    "WHERE     (name LIKE 'DiversityTaxonNames%')";
            //System.Data.DataTable dt = new DataTable();
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(dt);
            //foreach (System.Data.DataRow R in dt.Rows)
            //{
            //    try
            //    {
            //        SQL = "SELECT " + R[0].ToString()+ ".dbo.DiversityWorkbenchModule()";
            //        string Module = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //        if (Module == "DiversityTaxonNames")
            //            this.comboBoxDatabase.Items.Add(R[0].ToString());
            //    }
            //    catch { }
            //}
        }

        private void setTaxonListSource()
        {
            if (this.TaxonNamesDatabase.Length > 0)
            {
                string SQL = "SELECT ProjectID, Project " +
                    "FROM " + this.TaxonNamesDatabase + ".dbo.TaxonNameListProjectProxy " +
                    "ORDER BY Project";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxTaxonList.DataSource = dt;
                this.comboBoxTaxonList.DisplayMember = "Project";
                this.comboBoxTaxonList.ValueMember = "ProjectID";
            }
        }

        private string TaxonNamesDatabase
        {
            get
            {
                try
                {
                    if (this.comboBoxDatabase.SelectedItem != null)
                        return this.comboBoxDatabase.SelectedItem.ToString();
                }
                catch { return ""; }
                return "";
            }
        }

        private int? ProjectID
        {
            get
            {
                if (this.comboBoxTaxonList.SelectedIndex > -1 &&
                    this.comboBoxTaxonList.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxTaxonList.SelectedItem;
                    int ProjectID;
                    if (int.TryParse(R["ProjectID"].ToString(), out ProjectID))
                        return ProjectID;
                }
                return null;
            }
        }

        #region Analysis
        private void setAnalysisSource()
        {
            if (this.TaxonNamesDatabase.Length > 0)
            {
                string SQL = "SELECT C.AnalysisID, C.DisplayText, MIN(C.Description) AS Description " +
                    "FROM " + this.TaxonNamesDatabase + ".dbo.TaxonNameListAnalysisCategory AS C INNER JOIN " +
                    this.TaxonNamesDatabase + ".dbo.TaxonNameListAnalysis AS A ON C.AnalysisID = A.AnalysisID " +
                    "WHERE (A.ProjectID = " + this.ProjectID.ToString() + ") " +
                    "GROUP BY C.AnalysisID, C.DisplayText " +
                    "ORDER BY C.DisplayText";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxAnalysis.DataSource = dt;
                this.comboBoxAnalysis.DisplayMember = "DisplayText";
                this.comboBoxAnalysis.ValueMember = "AnalysisID";
            }
        }

        private int? AnalysisID
        {
            get
            {
                if (this.comboBoxAnalysis.SelectedIndex > -1 &&
                    this.comboBoxAnalysis.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxAnalysis.SelectedItem;
                    int AnalysisID;
                    if (int.TryParse(R["AnalysisID"].ToString(), out AnalysisID))
                        return AnalysisID;
                }
                return null;
            }
        }

        public string AnalysisDescription
        {
            get => _AnalysisDescription;
            set
            {
                _AnalysisDescription = value;
                this.labelAnalysisDescription.Text = value;
                this.labelAnalysisDescription.Visible = value.Length > 0;
            }
        }

        private void comboBoxAnalysis_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxAnalysis.SelectedItem;
                AnalysisDescription = R["Description"].ToString();
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        private void buttonRequeryTaxonList_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.DisplayList();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter sw;
            //string GridViewExportFile = this.textBoxTaxonListFile.Text;
            if (System.IO.File.Exists(this.textBoxTaxonListFile.Text))
                sw = new System.IO.StreamWriter(this.textBoxTaxonListFile.Text, true);
            else
                sw = new System.IO.StreamWriter(this.textBoxTaxonListFile.Text);
            try
            {
                sw.WriteLine(this.labelHeader.Text);
                sw.WriteLine();
                sw.WriteLine("User:\t" + System.Environment.UserName);
                sw.Write("Date:\t");
                sw.WriteLine(DateTime.Now);
                sw.WriteLine();
                if (this.comboBoxTaxonomicGroup.Text.Length > 0 && this.comboBoxTaxonomicGroup.SelectedIndex > 0)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxTaxonomicGroup.SelectedItem;
                    sw.WriteLine("For taxonomic group " + R[0].ToString());
                }
                if (TimeRestriction().Length > 0)
                {
                    string TimeRes =  "";
                    if (this.checkBoxFrom.Checked)
                    {
                        TimeRes = "Form:\t" + this.dateTimePickerFrom.Value.ToString("yyyy-MM-dd");
                        sw.WriteLine(TimeRes);
                    }
                    if (this.checkBoxUntil.Checked)
                    {
                        TimeRes = "Until:\t" + this.dateTimePickerUntil.Value.ToString("yyyy-MM-dd");
                        sw.WriteLine(TimeRes);
                    }
                }
                string Analysis = "";
                if (this.checkBoxIncludeTaxonLists.Checked && this.ProjectID != null && this.AnalysisID != null)
                {
                    if (this.comboBoxAnalysis.SelectedIndex > -1 &&
                        this.comboBoxAnalysis.SelectedItem != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxAnalysis.SelectedItem;
                        if (!(R["DisplayText"].Equals(System.DBNull.Value) && R["DisplayText"].ToString().Length > 0))
                            Analysis = R["DisplayText"].ToString();
                        if (_AnalysisDescription.Length > 0)
                            sw.WriteLine("Analysis:\t" + _AnalysisDescription);
                    }
                }
                sw.WriteLine();
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewTaxa.Columns)
                {
                    if (C.Visible)
                    {
                        if (C.DataPropertyName == "AnalysisValue")
                            sw.Write(Analysis + "\t");
                        else
                            sw.Write(C.DataPropertyName + "\t");
                    }
                }
                sw.WriteLine();
                sw.WriteLine();
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewTaxa.Rows)
                {
                    foreach (System.Windows.Forms.DataGridViewCell Cell in R.Cells)
                    {
                        if (this.dataGridViewTaxa.Columns[Cell.ColumnIndex].Visible)
                        {
                            if (Cell.Value == null)
                                sw.Write("\t");
                            else
                                sw.Write(Cell.Value.ToString() + "\t");
                        }
                    }
                    sw.WriteLine();
                }
                System.Windows.Forms.MessageBox.Show("Data were exported to " + this.textBoxTaxonListFile.Text);
            }
            catch
            {
            }
            finally
            {
                sw.Close();
            }
        }

        private void FormTaxonList_FormClosing(object sender, FormClosingEventArgs e)
        {
            _CountUnits = this.radioButtonCountUnits.Checked;
            _IncludeLists = this.checkBoxIncludeTaxonLists.Checked;
            _IncludeGender = this.checkBoxIncludeGender.Checked;
        }

        #endregion

    }
}
