using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormStatistics : Form
    {
        #region Parameter

        private int _ProjectID;
        private System.Data.DataTable _dtProject;
        private System.Data.DataTable _dtProjectActivity;
        private System.Collections.Generic.List<int> _IDs;
        private string _IDlist;
        private string _SqlForIDs;
        
        #endregion

        #region Construction and Form
        
        public FormStatistics()
        {
            InitializeComponent();
            this.initForm();
        }

        public FormStatistics(System.Collections.Generic.List<int> IDs) : this()
        {
            this._IDs = IDs;
            this.initQuery();
        }

        public FormStatistics(string SqlIDs) : this()
        {
            this._SqlForIDs = SqlIDs;
            this.initQuery();
        }


        private void initForm()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Login = "";
            string SqlVersion = "select user_name()";
            Microsoft.Data.SqlClient.SqlConnection ConVersion = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            Microsoft.Data.SqlClient.SqlCommand CVersion = new Microsoft.Data.SqlClient.SqlCommand(SqlVersion, ConVersion);
            try
            {
                ConVersion.Open();
                Login = CVersion.ExecuteScalar().ToString();
                ConVersion.Close();
            }
            catch { }


            try
            {
                //User
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter("", DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandText = "SELECT U.CombinedNameCache AS [User name], U.LoginName AS [Login name], U.AgentURI AS [Link to DiversityAgents], COUNT(*) AS [Number of specimens (last active)], MAX(S.LogUpdatedWhen) " +
                    "AS [Last active at specimen], COUNT(DISTINCT E.CollectionEventID) AS [Number of events (last active)], MAX(E.LogUpdatedWhen) AS [Last active at events] " +
                    "FROM CollectionEvent E RIGHT OUTER JOIN " +
                    "CollectionSpecimen S ON E.CollectionEventID = S.CollectionEventID RIGHT OUTER JOIN " +
                    "UserProxy U ON S.LogUpdatedBy = U.LoginName ";
                if (Login != "dbo")
                    ad.SelectCommand.CommandText += "WHERE U.LoginName = '" + Login + "' ";
                ad.SelectCommand.CommandText += "GROUP BY U.CombinedNameCache, U.LoginName, U.AgentURI " +
                    "ORDER BY U.CombinedNameCache";
                System.Data.DataTable dtUser = new DataTable();
                ad.Fill(dtUser);
                this.dataGridViewUser.DataSource = dtUser;
                this.dataGridViewUser.ReadOnly = true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Retrieving statistics for user failed:\r\n" + ex.Message);
            }
            try
            {
                //Project
                this.comboBoxProjectDetails.DataSource = DiversityCollection.LookupTable.DtProjectList;
                this.comboBoxProjectDetails.DisplayMember = "Project";
                this.comboBoxProjectDetails.ValueMember = "ProjectID";


            }
            catch (System.Exception ex)
            {
            }

            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryTransactionInstitution.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;

            System.DateTime From = System.DateTime.Parse(System.DateTime.Now.Year.ToString() + "-01-01");
            System.DateTime Until = System.DateTime.Parse(System.DateTime.Now.Year.ToString() + "-12-31");
            this.dateTimePickerTransactionFrom.Value = From;
            this.dateTimePickerTransactionUntil.Value = Until;

            string Path = Folder.Transaction(Folder.TransactionFolder.Statistics) + "Statistics.xslt";
            System.IO.FileInfo fi = new System.IO.FileInfo(Path);
            if (fi.Exists)
                this.textBoxTransactionSchema.Text = fi.FullName;

            Path = Folder.Transaction(Folder.TransactionFolder.Schemas) + "StatisticsCountry.xslt";
            System.IO.FileInfo fc = new System.IO.FileInfo(Path);
            if (fc.Exists)
                this.textBoxTransactionAddressSourceSchema.Text = fc.FullName;

            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityAgents"].ServerConnectionList())
            {
                if (KV.Value.LinkedServer.Length == 0 && !KV.Value.IsAddedRemoteConnection)
                    this.comboBoxTransactionAddressSource.Items.Add(KV.Value.DatabaseName);
            }


            if (this.comboBoxTransactionContentRestrictToMaterial.DataSource == null)
            {
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTransactionContentRestrictToMaterial, "CollMaterialCategory_Enum", DiversityWorkbench.Settings.Connection, true);
            }
            if (this.comboBoxTransactionContentRestrictToTransaction.DataSource == null)
            {
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTransactionContentRestrictToTransaction, "CollTransactionType_Enum", DiversityWorkbench.Settings.Connection, true);
            }

            this.initHistory();

            this.initTransactionHistory();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        
        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        private void FormStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this._sqlDataAdapterQuery != null)
                {
                    if (this._SqlConnectionQuery != null)
                    {
                        if (this._SqlConnectionQuery.State == ConnectionState.Open)
                            this._SqlConnectionQuery.Close();
                        this._SqlConnectionQuery.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Projects
        
        private void dataGridViewProjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.setProjectChart();
        }

        private void setProjectChart()
        {
            return;

            try
            {
                int CurrentRow = this.dataGridViewProjects.SelectedCells[0].RowIndex;
                int CurrentProject = int.Parse(this._dtProject.Rows[CurrentRow][1].ToString());
                //Activity displayed with chart
                string SQL = "SELECT CAST(YEAR(CollectionSpecimen.LogUpdatedWhen) AS varchar) + '-' + CASE WHEN month(CollectionSpecimen.LogUpdatedWhen) " +
                "< 10 THEN '0' ELSE '' END + CAST(MONTH(CollectionSpecimen.LogUpdatedWhen) AS varchar) AS Month, COUNT(*) AS Activity " +
                "FROM CollectionSpecimen INNER JOIN " +
                "CollectionProject ON CollectionSpecimen.CollectionSpecimenID = CollectionProject.CollectionSpecimenID " +
                "WHERE CollectionProject.ProjectID =  " +
                "GROUP BY CAST(YEAR(CollectionSpecimen.LogUpdatedWhen) AS varchar) + '-' + CASE WHEN month(CollectionSpecimen.LogUpdatedWhen) " +
                "< 10 THEN '0' ELSE '' END + CAST(MONTH(CollectionSpecimen.LogUpdatedWhen) AS varchar) " +
                "ORDER BY Month";
                if (this._dtProjectActivity == null)
                    this._dtProjectActivity = new DataTable();
                else this._dtProjectActivity.Clear();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(this._dtProjectActivity);

            }
            catch { }
        }

        #endregion

        #region Project

        private string _ProjectErrors;
        
        private void comboBoxProjectDetails_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.progressBarProject.Maximum = 22;
                this.progressBarProject.Value = 0;
                _ProjectErrors = "";

                this._ProjectID = (int)this.comboBoxProjectDetails.SelectedValue;
                _dtProjectImages = null;
                this.setProjectState("Collection Date Range");
                this.getCollectionDateRange();

                this.setProjectState("Collectors");
                this.getCollectors();

                this.setProjectState("Coordinates");
                this.getCoordinates();

                this.setProjectState("Countries");
                this.getCountries();

                this.setProjectState("Families");
                this.getFamilies();

                this.setProjectState("Orders");
                this.getOrders();

                this.setProjectState("Taxa");
                this.getTaxa();

                this.setProjectState("Taxa hierarchy");
                this.getTaxaHierarchy();

                this.setProjectState("Types");
                this.getTypes();

                this.setProjectState("Taxonomic Groups");
                this.getTaxonomicGroups();

                this.setProjectState("Taxonomic Group Taxa");
                this.getTaxonomicGroupTaxa();

                this.setProjectState("Parts");
                this.getParts();

                this.setProjectState("Taxon Data");
                this.getTaxonData();

                this.setProjectState("Taxon List");
                this.getTaxonList();

                this.setProjectState("Images");
                this.getImageData();

                this.setProjectState("Taxon images");
                this.getTaxonImages();

                this.setProjectState("Unit In Parts In Detail");
                this.getUnitInPartsInDetail();

                this.setProjectState("Unit In Parts Summary");
                this.getUnitInPartsSummary();

                this.setProjectState("Unit In Parts With Geo Detail");
                this.getUnitInPartsWithGeoDetail();

                this.setProjectState("Unit In Parts With Geo Summary");
                this.getUnitInPartsWithGeoSummary();

                this.setProjectState("Cache Database");
                this.initCacheDatabase();

                this.setProjectState("Cache Unit In Parts");
                this.getCacheUnitInParts();

                this.setProjectState("Cache Unit In Parts Geo");
                this.getCacheUnitInPartsGeo();

                this.setProjectState("Projects In Project");
                this.getProjectsInProject();

                this.setProjectState("");
                this.Cursor = System.Windows.Forms.Cursors.Default;
                if (this._ProjectErrors.Length > 0)
                    System.Windows.Forms.MessageBox.Show(this._ProjectErrors, "Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (System.Exception ex) { }
        }

        private void setProjectState(string State)
        {
            this.labelProjectState.Text = State;
            if (this.progressBarProject.Value < this.progressBarProject.Maximum)
                this.progressBarProject.Value++;
            Application.DoEvents();
        }

        /*
         * - Countries
        - Taxa
        - Families
        - Orders
        - Images
        - Collectors
        - in welchem Zeitraum wurden in diesem Projekt Belege gesammelt? (Col.
        date)
        - wieviele sind georef. (unabhängig von der Art der Georef.) - kann
        man über Anwesenheit von Long., etc. rausfinden, fände ich hier aber
        noch schöner
        Es gibt natürlich noch viel mehr Möglichkeiten, z.B. wieviele
        Processing Types wurden angewendet oder wieviele verschiedene
        Imagetypes gibt es, aber das denke ich ist so unwahrscheinlich, dass
        es sich nicht lohnt, dafür die Abfragezeit zu verlängern. Das ist jedoch lediglich mein Gedanke.


        Und noch Vorschläge von Frau Triebel:

        Verschiedene Genera?

        Höhe, Tiefe?

        Typen?

         * */

        #region Geo
        private void getCountries()
        {
            try
            {
                string SQL = "SELECT E.CountryCache AS Country, COUNT(*) AS [No] " +
                    "FROM         CollectionEvent AS E INNER JOIN " +
                    "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                    "GROUP BY E.CountryCache " +
                    "ORDER BY E.CountryCache";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewCountry.DataSource = dt;
                this.labelCountryNr.Text = dt.Rows.Count.ToString();
                this.dataGridViewCountry.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getCountries():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getCoordinates()
        {
            try
            {
                string SQL = "SELECT L.DisplayText AS [Localisation system], CASE WHEN E.AverageLatitudeCache <> 0 AND E.AverageLongitudeCache <> 0 THEN 'Georeference' ELSE '-' END AS [Has Geography], count(*) As No " +
                    "FROM CollectionEventLocalisation AS E INNER JOIN " +
                    "LocalisationSystem AS L ON E.LocalisationSystemID = L.LocalisationSystemID INNER JOIN " +
                    "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                    "GROUP BY L.DisplayText, CASE WHEN E.AverageLatitudeCache <> 0 AND E.AverageLongitudeCache <> 0 THEN 'Georeference' ELSE '-' END " +
                    "ORDER BY L.DisplayText";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewLocalisation.DataSource = dt;
                this.dataGridViewLocalisation.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getCoordinates():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getCollectionDateRange()
        {
            try
            {
                string SQL = "SELECT convert( nvarchar(10), MIN(E.CollectionDate), 120) AS [From], convert( nvarchar(10), MAX(E.CollectionDate), 120) AS Until " +
                    "FROM         CollectionEvent AS E INNER JOIN " +
                    "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                    "AND (NOT (E.CollectionDate IS NULL))";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewDateRange.DataSource = dt;
                this.dataGridViewDateRange.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getCollectionDateRange():\r\n" + ex.Message + "\r\n";
            }

        }

        #endregion

        #region Taxa

        private void getFamilies()
        {
            try
            {
                string SQL = "SELECT  CASE WHEN I.FamilyCache IS NULL THEN '' ELSE I.FamilyCache END AS Family, COUNT(*) AS No " +
                     "FROM IdentificationUnit AS I INNER JOIN " +
                     "CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID " +
                     "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                     "GROUP BY CASE WHEN I.FamilyCache IS NULL THEN '' ELSE I.FamilyCache END " +
                     "ORDER BY [Family]";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewFamily.DataSource = dt;
                this.labelFamilyCount.Text = dt.Rows.Count.ToString();
                this.dataGridViewFamily.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getFamilies():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getOrders()
        {
            try
            {
                string SQL = "SELECT  CASE WHEN  I.OrderCache IS NULL THEN '' ELSE I.OrderCache END AS [Order], COUNT(*) AS No " +
                "FROM IdentificationUnit AS I INNER JOIN " +
                "CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID " +
                "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                "GROUP BY  CASE WHEN  I.OrderCache IS NULL THEN '' ELSE I.OrderCache END " +
                "ORDER BY [Order]";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewOrder.DataSource = dt;
                this.labelOrderCount.Text = dt.Rows.Count.ToString();
                this.dataGridViewOrder.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getOrders():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getTaxonomicGroups()
        {
            try
            {
                string SQL = "SELECT I.TaxonomicGroup AS [Taxonomic group], COUNT(*) AS No " +
                     "FROM IdentificationUnit AS I INNER JOIN " +
                     "CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID " +
                     "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                     "GROUP BY I.TaxonomicGroup " +
                     "ORDER BY [Taxonomic group]";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewGroups.DataSource = dt;
                this.labelGroupCount.Text = dt.Rows.Count.ToString();
                this.dataGridViewGroups.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getTaxonomicGroups():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getTaxonomicGroupTaxa()
        {
            try
            {
                string SQL = "SELECT I.TaxonomicGroup AS [Taxonomic group], COUNT(Distinct LastIdentificationCache) AS Taxa " +
                     "FROM IdentificationUnit AS I INNER JOIN " +
                     "CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID " +
                     "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                     "GROUP BY I.TaxonomicGroup " +
                     "ORDER BY [Taxonomic group]";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewGroupTaxa.DataSource = dt;
                this.labelGroupCount.Text = dt.Rows.Count.ToString();
                this.dataGridViewGroupTaxa.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getTaxonomicGroupTaxa():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getTaxa()
        {
            try
            {
                string SQL = "SELECT  I.TaxonomicGroup AS [Taxonomic group], I.LastIdentificationCache AS Taxon, COUNT(*) AS No " +
               "FROM IdentificationUnit AS I INNER JOIN " +
               "CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID " +
               "WHERE P.ProjectID = " + this._ProjectID.ToString() + " AND I.LastIdentificationCache <> '' " +
               "GROUP BY I.TaxonomicGroup,  I.LastIdentificationCache " +
               "ORDER BY [Taxon]";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewTaxa.DataSource = dt;
                this.labelTaxaCount.Text = dt.Rows.Count.ToString();
                this.dataGridViewTaxa.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getTaxa():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getTaxaHierarchy()
        {
            try
            {
                string SQL = "SELECT  I.TaxonomicGroup AS [Taxonomic group], HierarchyCache AS Hierarchy, OrderCache AS [Order], FamilyCache AS Family, I.LastIdentificationCache AS Taxon, COUNT(*) AS No " +
               "FROM IdentificationUnit AS I INNER JOIN " +
               "CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID " +
               "WHERE P.ProjectID = " + this._ProjectID.ToString() + " AND I.LastIdentificationCache <> '' " +
               "GROUP BY I.TaxonomicGroup, I.HierarchyCache, I.OrderCache, I.FamilyCache,  I.LastIdentificationCache " +
               "ORDER BY I.TaxonomicGroup, I.HierarchyCache, I.OrderCache, I.FamilyCache,  I.LastIdentificationCache";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewProjectTaxaHierarchy.DataSource = dt;
                this.dataGridViewProjectTaxaHierarchy.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getTaxa():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getTypes()
        {
            try
            {
                string SQL = "SELECT     I.TypeStatus AS Type, COUNT(*) AS No " +
                    "FROM         Identification AS I INNER JOIN " +
                    "CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                    "GROUP BY I.TypeStatus " +
                    "HAVING      (NOT (I.TypeStatus IS NULL)) " +
                    "ORDER BY Type";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewType.DataSource = dt;
                int i = 0;
                int Types = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (int.TryParse(R[1].ToString(), out i))
                        Types += i;
                }
                this.labelTypeCount.Text = Types.ToString();
                this.dataGridViewType.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getTypes():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getTaxonData()
        {
            try
            {
                string SQL = "SELECT I.TaxonomicName AS [Taxonomic name], COUNT(DISTINCT I.IdentificationUnitID) AS Recurrence, CONVERT(nvarchar(10), MIN(E.CollectionDate), 111) AS [First date], CONVERT(nvarchar(10), MAX(E.CollectionDate), 111) AS [Last date], MIN(L.AverageLatitudeCache) AS South, " +
                    "MAX(L.AverageLatitudeCache) AS North, MIN(L.AverageLongitudeCache) AS West, MAX(L.AverageLongitudeCache) AS East " +
                    "FROM         CollectionSpecimen AS S INNER JOIN " +
                    "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                    "Identification AS I INNER JOIN " +
                    "CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID ON S.CollectionSpecimenID = P.CollectionSpecimenID LEFT OUTER JOIN " +
                    "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID  " +
                    "WHERE (P.ProjectID = " + this._ProjectID.ToString() + ") " +
                    "GROUP BY I.TaxonomicName " +
                    "ORDER BY I.TaxonomicName";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewTaxonData.DataSource = dt;
                this.dataGridViewTaxonData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getTaxonData():\r\n" + ex.Message + "\r\n";
            }
        }

        #endregion

        #region TaxonList

        private void getTaxonList()
        {
#if !DEBUG
            this.tabControlProject.TabPages.Remove(this.tabPageTaxonList);
            
#endif
        }

        /*
         * 
SELECT        U.OrderCache, U.FamilyCache, SUBSTRING(U.LastIdentificationCache, 1, CHARINDEX(' ', U.LastIdentificationCache)) AS Genus, U.LastIdentificationCache, MAX(I.NameURI) AS NameURI, E.CollectionDate, COUNT(*)
FROM            CollectionEvent AS E INNER JOIN
CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID INNER JOIN
IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID INNER JOIN
Identification AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID AND U.IdentificationUnitID = I.IdentificationUnitID AND U.LastIdentificationCache = I.TaxonomicName INNER JOIN
CollectionProject ON S.CollectionSpecimenID = CollectionProject.CollectionSpecimenID
WHERE        (CollectionProject.ProjectID = 952) AND (NOT (I.NameURI IS NULL))
GROUP BY E.CollectionDate, U.FamilyCache, U.OrderCache, U.LastIdentificationCache, SUBSTRING(U.LastIdentificationCache, 1, CHARINDEX(' ', U.LastIdentificationCache))
HAVING        (NOT (E.CollectionDate IS NULL)) AND U.OrderCache = 'Hymenoptera' AND U.LastIdentificationCache LIKE '% %'
ORDER BY U.OrderCache, U.FamilyCache, U.LastIdentificationCache


SELECT        U.LastIdentificationCache, MAX(I.NameURI) AS NameURI
FROM            IdentificationUnit AS U INNER JOIN
Identification AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID AND U.IdentificationUnitID = I.IdentificationUnitID AND U.LastIdentificationCache = I.TaxonomicName INNER JOIN
CollectionProject ON U.CollectionSpecimenID = CollectionProject.CollectionSpecimenID
WHERE        (CollectionProject.ProjectID = 952)
GROUP BY U.OrderCache, U.LastIdentificationCache, SUBSTRING(U.LastIdentificationCache, 1, CHARINDEX(' ', U.LastIdentificationCache))
HAVING        (U.OrderCache = 'Hymenoptera') AND (U.LastIdentificationCache LIKE '% %') AND (MAX(I.NameURI) LIKE '%diversityworkbench.de%')
ORDER BY U.OrderCache, U.LastIdentificationCache


            SELECT        U.OrderCache
FROM            IdentificationUnit AS U INNER JOIN
CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID
WHERE        (P.ProjectID = 952)
GROUP BY U.OrderCache
HAVING        (U.OrderCache <> N'')
ORDER BY U.OrderCache


            */

        private void buttonTaxonListStart_Click(object sender, EventArgs e)
        {
            string SQL = "";
        }

        private void buttonTaxonListExport_Click(object sender, EventArgs e)
        {

        }

#endregion

#region Agents

        private void getCollectors()
        {
            try
            {
                string SQL = "SELECT     A.CollectorsName AS Collector, COUNT(*) AS No " +
                    "FROM         CollectionAgent AS A INNER JOIN " +
                    "CollectionProject AS P ON A.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString() + " " +
                    "GROUP BY A.CollectorsName";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewCollectors.DataSource = dt;
                this.labelCollectorNumber.Text = dt.Rows.Count.ToString();
                this.dataGridViewCollectors.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getCollectors():\r\n" + ex.Message + "\r\n";
            }
        }

        #endregion

        #region Parts

        private void getParts()
        {
            try
            {
                string SQL = "SELECT C.CollectionName AS [Collection name], S.MaterialCategory AS [Material category], COUNT(*) AS No " +
                    "FROM CollectionSpecimenPart AS S INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID LEFT OUTER JOIN " +
                    "Collection AS C ON S.CollectionID = C.CollectionID " +
                    "WHERE  P.ProjectID = " + this._ProjectID.ToString() + " " +
                    "GROUP BY S.MaterialCategory, C.CollectionName " +
                    "ORDER BY [Collection name], [Material category]";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewParts.DataSource = dt;
                this.dataGridViewParts.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                int No = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    int i;
                    if (int.TryParse(R["No"].ToString(), out i))
                        No += i;
                }
                this.labelPartsTotal.Text = No.ToString();
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getParts():\r\n" + ex.Message + "\r\n";
            }
        }

        #endregion

        #region Images

        private static System.Data.DataTable _dtProjectImages;

        private System.Data.DataTable DtProjectImages
        {
            get
            {
                if (_dtProjectImages == null)
                {
                    string SQL = "SELECT DISTINCT U.TaxonomicGroup AS [Tax.Gr.], U.OrderCache AS [Order], U.FamilyCache AS Family, " +
                    " CASE WHEN U.LastIdentificationCache LIKE '% %'THEN SUBSTRING(U.LastIdentificationCache, 1, CHARINDEX(' ', U.LastIdentificationCache)) ELSE U.LastIdentificationCache END AS Genus, " +
                    " U.LastIdentificationCache AS Taxon, I.URI " +
                    " FROM IdentificationUnit AS U INNER JOIN CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                    " INNER JOIN CollectionSpecimenImage AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID AND U.IdentificationUnitID = I.IdentificationUnitID " +
                    " WHERE (P.ProjectID = " + this._ProjectID.ToString() + ") AND U.LastIdentificationCache not in (SELECT Code FROM [dbo].[CollTaxonomicGroup_Enum]) " +
                    " UNION " +
                    " SELECT DISTINCT U.TaxonomicGroup AS [Tax.Gr.], U.OrderCache AS [Order], U.FamilyCache AS Family, " +
                    " CASE WHEN U.LastIdentificationCache LIKE '% %'THEN SUBSTRING(U.LastIdentificationCache, 1, CHARINDEX(' ', U.LastIdentificationCache)) ELSE U.LastIdentificationCache END AS Genus, " +
                    " U.LastIdentificationCache AS Taxon, I.URI " +
                    " FROM IdentificationUnit AS U INNER JOIN CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                    " INNER JOIN CollectionSpecimenImage AS I ON U.CollectionSpecimenID = I.CollectionSpecimenID " +
                    " WHERE (P.ProjectID = " + this._ProjectID.ToString() + ") AND U.LastIdentificationCache not in (SELECT Code FROM [dbo].[CollTaxonomicGroup_Enum]) " +
                    " AND EXISTS (SELECT * FROM IdentificationUnit L WHERE L.CollectionSpecimenID = U.CollectionSpecimenID GROUP BY L.CollectionSpecimenID HAVING COUNT(*) = 1) " +
                    " ORDER BY U.TaxonomicGroup, U.OrderCache, U.FamilyCache, U.LastIdentificationCache";
                    _dtProjectImages = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                    ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    ad.Fill(_dtProjectImages);
                }
                return _dtProjectImages;
            }
        }


        private void getImageData()
        {
            try
            {
                string SQL = "SELECT COUNT (*) AS [Number] " +
                ",case when URI like 'http://%' then substring (URI, 1, 8) + SUBSTRING (URI, 9, charindex('/', substring(URI, 9, 500))) else NULL end as [Server] " +
                ",[ImageType] " +
                "FROM [CollectionSpecimenImage] I, CollectionProject P " +
                "WHERE I.CollectionSpecimenID = P.CollectionSpecimenID and P.ProjectID = " + this._ProjectID.ToString() + " " +
                "GROUP BY [ImageType], case when URI like 'http://%' then substring (URI, 1, 8) + SUBSTRING (URI, 9, charindex('/', substring(URI, 9, 500))) else NULL end";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewProjectImages.DataSource = dt;
                this.dataGridViewProjectImages.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                SQL = "SELECT COUNT (*) AS [Number] " +
                "FROM [CollectionSpecimenImage] I, CollectionProject P " +
                "WHERE I.CollectionSpecimenID = P.CollectionSpecimenID and P.ProjectID = " + this._ProjectID.ToString();
                string Nr = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this.labelProjectImagesNumber.Text = "Number of images: " + Nr;
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getImageData():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getTaxonImages()
        {
            try
            {
                this.dataGridViewProjectImagesTaxa.DataSource = DtProjectImages;
                this.dataGridViewProjectImagesTaxa.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                this.labelProjectImagesTaxa.Text = "Number of taxon images: " + DtProjectImages.Rows.Count.ToString();
                this.dataGridViewProjectImagesTaxa.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getImageData():\r\n" + ex.Message + "\r\n";
            }
        }

        private void dataGridViewProjectImagesTaxa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (DtProjectImages != null)
                {
                    //System.Data.DataRow R = DtProjectImages.Rows[e.RowIndex];
                    //this.userControlProjectImage.ImagePath = R["URI"].ToString();
                    string URI = this.dataGridViewProjectImagesTaxa.Rows[e.RowIndex].Cells[5].Value.ToString();
                    this.userControlProjectImage.ImagePath = URI;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Projects

        private void getProjectsInProject()
        {
            try
            {
                string SQL = "SELECT PP.Project, COUNT(*) AS Specimens " +
                    "FROM CollectionProject AS C1 INNER JOIN " +
                    "CollectionProject AS C2 ON C1.CollectionSpecimenID = C2.CollectionSpecimenID INNER JOIN " +
                    "ProjectProxy AS PP ON C2.ProjectID = PP.ProjectID " +
                    "WHERE (C1.ProjectID = " + this._ProjectID.ToString() + ") " +
                    "GROUP BY PP.Project";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewProjectsInProject.DataSource = dt;
                this.dataGridViewProjectsInProject.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                SQL = "SELECT        PP.Project, COUNT(*) AS Organisms " +
                    "FROM            CollectionProject AS C1 INNER JOIN " +
                    "CollectionProject AS C2 ON C1.CollectionSpecimenID = C2.CollectionSpecimenID INNER JOIN " +
                    "ProjectProxy AS PP ON C2.ProjectID = PP.ProjectID INNER JOIN " +
                    "IdentificationUnit AS U ON C2.CollectionSpecimenID = U.CollectionSpecimenID " +
                    "WHERE        (C1.ProjectID = " + this._ProjectID.ToString() + ") " +
                    "GROUP BY PP.Project";
                System.Data.DataTable dtU = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter adU = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                adU.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                adU.Fill(dtU);
                this.dataGridViewProjectsInProjectUnit.DataSource = dtU;
                this.dataGridViewProjectsInProjectUnit.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getProjectsInProject():\r\n" + ex.Message + "\r\n";
            }
        }

        #endregion

        #region UnitInParts - guess what is there

        private void getUnitInPartsInDetail()
        {
            try
            {
                string SQL = "SELECT        COUNT(*) AS Total, " +
                    "CASE WHEN U.DataWithholdingReason IS NULL OR U.DataWithholdingReason = '' THEN '' ELSE U.DataWithholdingReason END AS [Unit withholding reason], " +
                    "CASE WHEN SP.DataWithholdingReason IS NULL OR SP.DataWithholdingReason = '' THEN '' ELSE SP.DataWithholdingReason END AS [Part withholding reason],  " +
                    "CASE WHEN S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '' THEN '' ELSE S.DataWithholdingReason END AS [Specimen withholding reason] " +
                    "FROM            CollectionSpecimen AS S INNER JOIN " +
                    "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID LEFT OUTER JOIN " +
                    "CollectionSpecimenPart AS SP INNER JOIN " +
                    "IdentificationUnitInPart AS UP ON SP.CollectionSpecimenID = UP.CollectionSpecimenID AND SP.SpecimenPartID = UP.SpecimenPartID ON  " +
                    "S.CollectionSpecimenID = SP.CollectionSpecimenID AND U.CollectionSpecimenID = UP.CollectionSpecimenID AND  " +
                    "U.IdentificationUnitID = UP.IdentificationUnitID INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID  " +
                    "GROUP BY CASE WHEN U.DataWithholdingReason IS NULL OR U.DataWithholdingReason = '' THEN '' ELSE U.DataWithholdingReason END,  " +
                    "CASE WHEN SP.DataWithholdingReason IS NULL OR SP.DataWithholdingReason = '' THEN '' ELSE SP.DataWithholdingReason END,  " +
                    "CASE WHEN S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '' THEN '' ELSE S.DataWithholdingReason END, P.ProjectID " +
                    "HAVING        (P.ProjectID = " + this._ProjectID.ToString() + ")";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewUnitInPartDetail.DataSource = dt;
                this.dataGridViewUnitInPartDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getUnitInPartsInDetail():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getUnitInPartsSummary()
        {
            try
            {
                string SQL = "SELECT COUNT(*) AS Total, " +
                    "CASE WHEN U.DataWithholdingReason IS NULL OR U.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END AS [Unit withholding reason],  " +
                    "CASE WHEN SP.DataWithholdingReason IS NULL OR SP.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END AS [Part withholding reason],  " +
                    "CASE WHEN S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END AS [Specimen withholding reason] " +
                    "FROM CollectionSpecimen AS S INNER JOIN " +
                    "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID LEFT OUTER JOIN " +
                    "CollectionSpecimenPart AS SP INNER JOIN " +
                    "IdentificationUnitInPart AS UP ON SP.CollectionSpecimenID = UP.CollectionSpecimenID AND SP.SpecimenPartID = UP.SpecimenPartID ON  " +
                    "S.CollectionSpecimenID = SP.CollectionSpecimenID AND U.CollectionSpecimenID = UP.CollectionSpecimenID AND  " +
                    "U.IdentificationUnitID = UP.IdentificationUnitID INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID  " +
                    "GROUP BY CASE WHEN U.DataWithholdingReason IS NULL OR U.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END, " +
                    "CASE WHEN SP.DataWithholdingReason IS NULL OR SP.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END, " +
                    "CASE WHEN S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END, P.ProjectID " +
                    "HAVING (P.ProjectID = " + this._ProjectID.ToString() + ")";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewUnitInPartSummary.DataSource = dt;
                this.dataGridViewUnitInPartSummary.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getUnitInPartsSummary():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getUnitInPartsWithGeoSummary()
        {
            try
            {
                string SQL = "SELECT COUNT(*) AS Total,  " +
                    "CASE WHEN U.DataWithholdingReason IS NULL OR U.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END AS [Unit withholding reason],  " +
                    "CASE WHEN SP.DataWithholdingReason IS NULL OR SP.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END AS [Part withholding reason],  " +
                    "CASE WHEN S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END AS [Specimen withholding reason]  " +
                    "FROM CollectionSpecimen AS S  " +
                    "INNER JOIN IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID  " +
                    "LEFT OUTER JOIN CollectionSpecimenPart AS SP INNER JOIN IdentificationUnitInPart AS UP  " +
                    "ON SP.CollectionSpecimenID = UP.CollectionSpecimenID AND SP.SpecimenPartID = UP.SpecimenPartID  " +
                    "ON  S.CollectionSpecimenID = SP.CollectionSpecimenID AND U.CollectionSpecimenID = UP.CollectionSpecimenID AND  U.IdentificationUnitID = UP.IdentificationUnitID  " +
                    "INNER JOIN CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID   " +
                    "WHERE P.ProjectID = " + this._ProjectID.ToString() + " AND exists (select * from CollectionEventLocalisation L where L.CollectionEventID = S.CollectionEventID and not L.Geography is null)  " +
                    "GROUP BY CASE WHEN U.DataWithholdingReason IS NULL OR U.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END,  " +
                    "CASE WHEN SP.DataWithholdingReason IS NULL OR SP.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END,   " +
                    "CASE WHEN S.DataWithholdingReason IS NULL OR S.DataWithholdingReason = '' THEN '' ELSE 'Withhold' END ";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewUnitInPartSummaryGeo.DataSource = dt;
                this.dataGridViewUnitInPartSummaryGeo.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getUnitInPartsWithGeoSummary():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getUnitInPartsWithGeoDetail()
        {
            try
            {
                string SQL = "SELECT COUNT(*) AS Total, CASE WHEN U.DataWithholdingReason IS NULL OR " +
                    "U.DataWithholdingReason = '' THEN '' ELSE U.DataWithholdingReason END AS [Unit withholding reason], CASE WHEN SP.DataWithholdingReason IS NULL OR " +
                    "SP.DataWithholdingReason = '' THEN '' ELSE SP.DataWithholdingReason END AS [Part withholding reason], CASE WHEN S.DataWithholdingReason IS NULL OR " +
                    "S.DataWithholdingReason = '' THEN '' ELSE S.DataWithholdingReason END AS [Specimen withholding reason], LocalisationSystem.LocalisationSystemName AS [Localisation system] " +
                    "FROM CollectionEventLocalisation AS L INNER JOIN " +
                    "CollectionSpecimen AS S INNER JOIN " +
                    "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID LEFT OUTER JOIN " +
                    "CollectionSpecimenPart AS SP INNER JOIN " +
                    "IdentificationUnitInPart AS UP ON SP.CollectionSpecimenID = UP.CollectionSpecimenID AND SP.SpecimenPartID = UP.SpecimenPartID ON  " +
                    "S.CollectionSpecimenID = SP.CollectionSpecimenID AND U.CollectionSpecimenID = UP.CollectionSpecimenID AND  " +
                    "U.IdentificationUnitID = UP.IdentificationUnitID INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID ON L.CollectionEventID = S.CollectionEventID INNER JOIN " +
                    "LocalisationSystem ON L.LocalisationSystemID = LocalisationSystem.LocalisationSystemID " +
                    "WHERE (P.ProjectID = " + this._ProjectID.ToString() + ") AND (NOT (L.Geography IS NULL)) " +
                    "GROUP BY CASE WHEN U.DataWithholdingReason IS NULL OR " +
                    "U.DataWithholdingReason = '' THEN '' ELSE U.DataWithholdingReason END, CASE WHEN SP.DataWithholdingReason IS NULL OR " +
                    "SP.DataWithholdingReason = '' THEN '' ELSE SP.DataWithholdingReason END, CASE WHEN S.DataWithholdingReason IS NULL OR " +
                    "S.DataWithholdingReason = '' THEN '' ELSE S.DataWithholdingReason END, LocalisationSystem.LocalisationSystemName";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewUnitInPartDetailGeo.DataSource = dt;
                this.dataGridViewUnitInPartDetailGeo.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getUnitInPartsWithGeoDetail():\r\n" + ex.Message + "\r\n";
            }
        }

        #endregion

        #region UnitInParts - exported

        private void getCacheUnitInParts()
        {
            if (!CacheDatabaseProjectIsPublished())
                return;
            try
            {
                System.Data.DataRowView r = (System.Data.DataRowView)this.comboBoxProjectDetails.SelectedItem;
                string Schema = "Project_" + r[0].ToString();
                string SQL = "SELECT        COUNT(*) AS Total " +
                    "FROM  [" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "].[" + Schema + "].CacheIdentificationUnitInPart U INNER JOIN " +
                    "[" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "].[" + Schema + "].CacheCollectionProject P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE        (P.ProjectID = " + this._ProjectID.ToString() + ")";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewUnitInPartCacheDetail.DataSource = dt;
                this.dataGridViewUnitInPartCacheDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getCacheUnitInParts():\r\n" + ex.Message + "\r\n";
            }
        }

        private void getCacheUnitInPartsGeo()
        {
            if (!CacheDatabaseProjectIsPublished())
                return;
            try
            {
                System.Data.DataRowView r = (System.Data.DataRowView)this.comboBoxProjectDetails.SelectedItem;
                string Schema = "Project_" + r[0].ToString();
                string SQL = "SELECT COUNT(*) AS Total " +
                    "FROM            [" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "].[" + Schema + "].CacheIdentificationUnitGeoAnalysis AS U INNER JOIN " +
                    "[" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "].[" + Schema + "].CacheCollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                    "[" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "].[" + Schema + "].CacheCollectionSpecimen AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID " +
                    "WHERE        (P.ProjectID = " + this._ProjectID.ToString() + ") AND (U.Geography <> '')";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);
                this.dataGridViewUnitInPartCacheDetailGeo.DataSource = dt;
                this.dataGridViewUnitInPartCacheDetailGeo.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            }
            catch (Exception ex)
            {
                _ProjectErrors += "\r\n getCacheUnitInPartsGeo():\r\n" + ex.Message + "\r\n";
            }
        }


        #region CacheDatabase

        private string initCacheDatabase()
        {
            string Message = "";
            try
            {
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length == 0)
                {
                    string SQL = "SELECT DatabaseName, Server, Port, Version FROM CacheDatabase2";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    System.Data.DataTable dt = new DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        System.Data.DataRow R = dt.Rows[0];
                        DiversityCollection.CacheDatabase.CacheDB.DatabaseServer = R["Server"].ToString();
                        DiversityCollection.CacheDatabase.CacheDB.DatabaseName = R["DatabaseName"].ToString();
                        DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort = int.Parse(R["Port"].ToString());
                        DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion = R["Version"].ToString();
                    }
                    else
                    {
                        this.labelUnitInPartMayBe.Text = "No cache database available";
                        this.labelUnitInPartMayBe.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.labelUnitInPartMayBe.Text = "No access to cache database:\r\n" + ex.Message;
                this.labelUnitInPartMayBe.ForeColor = System.Drawing.Color.Red;
                //this.labelUnitInPartMayBe.Height = 40;
                //System.Windows.Forms.MessageBox.Show("No cache databases available");
            }
            return Message;
        }

        private bool CacheDatabaseProjectIsPublished()
        {
            System.Data.DataRowView r = (System.Data.DataRowView)this.comboBoxProjectDetails.SelectedItem;
            string Schema = "Project_" + r[0].ToString();
            string SQL = "SELECT COUNT(*) AS Anzahl " +
                "FROM  [" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "].[dbo].ProjectPublished P " +
                "WHERE        (P.ProjectID = " + this._ProjectID.ToString() + ")";
            int Anzahl = 0;
            int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true), out Anzahl);
            return Anzahl > 0;
        }

        #endregion

        #endregion

        private void buttonExportProjectStatistics_Click(object sender, EventArgs e)
        {
            if (this._ProjectID == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return;
            }
            string Date = System.DateTime.Now.ToString("yyyyMMdd");//.Year.ToString();
            //if (System.DateTime.Now.Month < 10)
            //    Date += "0";
            //Date += System.DateTime.Now.Month.ToString();
            //if (System.DateTime.Now.Day < 10)
            //    Date += "0";
            //Date += System.DateTime.Now.Day.ToString();
            System.IO.FileInfo FI = new System.IO.FileInfo(Folder.Statistics() + "Statistics_for_Project_" + this.comboBoxProjectDetails.Text + "_" + Date + ".txt");
            System.IO.StreamWriter sw;
            sw = new System.IO.StreamWriter(FI.FullName, false, System.Text.Encoding.UTF8);
            try
            {
                // LOCALITY
                // Countries
                sw.WriteLine("Locality and date");
                string Line = "Country\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewCountry.Rows.Count; i++)
                {
                    Line = this.dataGridViewCountry.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewCountry.Rows[i].Cells[1].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");

                // Localisation
                sw.WriteLine("Localisation");
                Line = "Localisation\tHas Geography\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewLocalisation.Rows.Count; i++)
                {
                    Line = this.dataGridViewLocalisation.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewLocalisation.Rows[i].Cells[1].Value.ToString() + "\t" + this.dataGridViewLocalisation.Rows[i].Cells[2].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");
                // Date range
                sw.WriteLine("Date range");
                Line = "From\tUntil";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewDateRange.Rows.Count; i++)
                {
                    Line = this.dataGridViewDateRange.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewDateRange.Rows[i].Cells[1].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");

                // COLLECTORS
                sw.WriteLine("Collectors");
                Line = "Collector\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewCollectors.Rows.Count; i++)
                {
                    Line = this.dataGridViewCollectors.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewCollectors.Rows[i].Cells[1].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");

                // TAXONOMY
                // Taxonomic groups
                sw.WriteLine("Taxonomic groups");
                Line = "Taxonomic group\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewGroups.Rows.Count; i++)
                {
                    Line = this.dataGridViewGroups.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewGroups.Rows[i].Cells[1].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");
                Line = "Taxonomic group\tTaxa";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewGroupTaxa.Rows.Count; i++)
                {
                    Line = this.dataGridViewGroupTaxa.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewGroupTaxa.Rows[i].Cells[1].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");

                // Orders
                sw.WriteLine("Orders");
                Line = "Order\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewOrder.Rows.Count; i++)
                {
                    Line = this.dataGridViewOrder.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewOrder.Rows[i].Cells[1].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");
                // Families
                sw.WriteLine("Families");
                Line = "Family\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewFamily.Rows.Count; i++)
                {
                    Line = this.dataGridViewFamily.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewFamily.Rows[i].Cells[1].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");
                // Taxa
                sw.WriteLine("Taxa");
                Line = "Taxonomic group\tTaxon\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewTaxa.Rows.Count; i++)
                {
                    Line = this.dataGridViewTaxa.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewTaxa.Rows[i].Cells[1].Value.ToString() + "\t" + this.dataGridViewTaxa.Rows[i].Cells[2].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");
                // Types
                sw.WriteLine("Types");
                Line = "Type\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewType.Rows.Count; i++)
                {
                    Line = this.dataGridViewType.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewType.Rows[i].Cells[1].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");
                // TAXA
                sw.WriteLine("Taxa - distribution");
                Line = "Taxonomic name\tRecurrence\tFirst date\tLast date\tSouth\tNorth\tWest\tEast";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewTaxonData.Rows.Count; i++)
                {
                    Line = this.dataGridViewTaxonData.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewTaxonData.Rows[i].Cells[1].Value.ToString() + "\t" + this.dataGridViewTaxonData.Rows[i].Cells[2].Value.ToString() + "\t" + this.dataGridViewTaxonData.Rows[i].Cells[3].Value.ToString() + "\t" + this.dataGridViewTaxonData.Rows[i].Cells[4].Value.ToString() + "\t" + this.dataGridViewTaxonData.Rows[i].Cells[5].Value.ToString() + "\t" + this.dataGridViewTaxonData.Rows[i].Cells[6].Value.ToString() + "\t" + this.dataGridViewTaxonData.Rows[i].Cells[7].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");

                // STORAGE
                sw.WriteLine("Storage");
                Line = "Collection\tMaterial\tNo";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewParts.Rows.Count; i++)
                {
                    Line = this.dataGridViewParts.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewParts.Rows[i].Cells[1].Value.ToString() + "\t" + this.dataGridViewParts.Rows[i].Cells[2].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");

                // IMAGES
                sw.WriteLine("Images");
                Line = "No\tServer\tImage type";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewProjectImages.Rows.Count; i++)
                {
                    Line = this.dataGridViewProjectImages.Rows[i].Cells[0].Value.ToString() + "\t" + this.dataGridViewProjectImages.Rows[i].Cells[1].Value.ToString() + "\t" + this.dataGridViewProjectImages.Rows[i].Cells[2].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");

                sw.WriteLine("Taxon images");
                Line = "Tax.Gr.\tOrder\tFamily\tGenus\tTaxon\tImage";
                sw.WriteLine(Line);
                for (int i = 0; i < this.dataGridViewProjectImagesTaxa.Rows.Count; i++)
                {
                    Line = this.dataGridViewProjectImagesTaxa.Rows[i].Cells[0].Value.ToString() + "\t" + 
                        this.dataGridViewProjectImagesTaxa.Rows[i].Cells[1].Value.ToString() + "\t" + 
                        this.dataGridViewProjectImagesTaxa.Rows[i].Cells[2].Value.ToString() + "\t" +
                        this.dataGridViewProjectImagesTaxa.Rows[i].Cells[3].Value.ToString() + "\t" +
                        this.dataGridViewProjectImagesTaxa.Rows[i].Cells[4].Value.ToString() + "\t" +
                        this.dataGridViewProjectImagesTaxa.Rows[i].Cells[5].Value.ToString();
                    sw.WriteLine(Line);
                }
                sw.WriteLine("");


                // PROJECTS

                System.Windows.Forms.MessageBox.Show("Statistics exported to " + FI.FullName);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }

#endregion

#region Query

        private void initQuery()
        {
            string Message = "";
            try
            {
                Message += this.getQueryCollectionDateRange();//lang
                Message += this.getQueryCollectors();
                Message += this.getQueryCoordinates();
                Message += this.getQueryCountries();
                Message += this.getQueryImages();
                Message += this.getQueryFamilies();
                Message += this.getQueryOrders();
                Message += this.getQueryTaxa();
                Message += this.getQueryTypes();
                Message += this.getQueryTaxonomicGroups();
                Message += this.getQueryTaxonomicGroupTaxa();
                Message += this.getQueryParts();
                Message += this.getQueryTaxonData();
                Message += this.getQueryProjectsSpecimen();
                Message += this.getQueryProjectsUnit();
                string CacheDBavailalble = this.initCacheDatabase();
                if (CacheDBavailalble != null && CacheDBavailalble.Length > 0)
                {
                    Message += CacheDBavailalble;
                    this.tableLayoutPanelUnitInPart.Enabled = false;
                }
                else
                {
                    Message += this.initCacheProjectList();
                }

                this.getQueryCacheUnitInParts();
                this.getQueryCacheUnitInPartsGeo();
            }
            catch (System.Exception ex) { }
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
        }

        private string IDlist
        {
            get
            {
                if (this._IDlist == null)
                {
                    this._IDlist = "";
                    for (int i = 0; i < this._IDs.Count; i++)
                    {
                        if (i > 0) this._IDlist += ", ";
                        this._IDlist += this._IDs[i].ToString();
                    }
                    if (this._IDlist.Trim().Length == 0)
                        this._IDlist = "-1";
                }
                return this._IDlist;
            }
        }

        /*
         * - Countries
            - Taxa
            - Families
            - Orders
            - Images
            - Collectors
            - in welchem Zeitraum wurden in diesem Projekt Belege gesammelt? (Col.
            date)
            - wieviele sind georef. (unabhängig von der Art der Georef.) - kann
            man über Anwesenheit von Long., etc. rausfinden, fände ich hier aber
            noch schöner
            Es gibt natürlich noch viel mehr Möglichkeiten, z.B. wieviele
            Processing Types wurden angewendet oder wieviele verschiedene
            Imagetypes gibt es, aber das denke ich ist so unwahrscheinlich, dass
            es sich nicht lohnt, dafür die Abfragezeit zu verlängern. Das ist jedoch lediglich mein Gedanke.


            Und noch Vorschläge von Frau Triebel:

            Verschiedene Genera?

            Höhe, Tiefe?

            Typen?

         * */

        private string getQueryCollectionDateRange(int? Timeout = null)
        {
            string Message = "";
            try
            {
                //string SQL = "SELECT convert( nvarchar(10), MIN(E.CollectionDate), 120) AS [From], convert( nvarchar(10), MAX(E.CollectionDate), 120) AS Until " +
                //    "FROM         CollectionEvent AS E INNER JOIN " +
                //    "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID  " +
                //    "WHERE S.CollectionSpecimenID IN (" + this.IDlist + ") " +
                //    "AND (NOT (E.CollectionDate IS NULL))";
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT convert( nvarchar(10), MIN(E.CollectionDate), 120) AS [From], convert( nvarchar(10), MAX(E.CollectionDate), 120) AS Until " +
                    "FROM         CollectionEvent AS E INNER JOIN " +
                    "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID  " +
                    "INNER JOIN #TempIDs T ON S.CollectionSpecimenID = T.CollectionSpecimenID  " +
                    "WHERE (NOT (E.CollectionDate IS NULL))";
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);

                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase; 
                //ad.Fill(dt);
                this.dataGridViewQueryDateRange.DataSource = dt;
                this.dataGridViewQueryDateRange.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex) 
            {
                Message = "Retrieving statistics for date range failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryCountries(int? Timeout = null)
        {
            string Message = "";
            try
            {
                //string SQL = "SELECT E.CountryCache AS Country, COUNT(*) AS [No] " +
                //    "FROM         CollectionEvent AS E INNER JOIN " +
                //    "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID  " +
                //    "WHERE S.CollectionSpecimenID IN (" + this.IDlist + ") " +
                //    "GROUP BY E.CountryCache " +
                //    "ORDER BY E.CountryCache";
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT E.CountryCache AS Country, COUNT(*) AS [No] " +
                    "FROM         CollectionEvent AS E INNER JOIN " +
                    "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID  " +
                    "INNER JOIN #TempIDs T ON S.CollectionSpecimenID = T.CollectionSpecimenID " +
                    "GROUP BY E.CountryCache " +
                    "ORDER BY E.CountryCache";
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //ad.Fill(dt);
                this.dataGridViewQueryCountries.DataSource = dt;
                this.labelQueryCountryNr.Text = dt.Rows.Count.ToString();
                this.dataGridViewQueryCountries.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex) 
            {
                Message = "Retrieving statistics for countries failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryCoordinates(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT L.DisplayText AS [Localisation system], CASE WHEN E.AverageLatitudeCache <> 0 AND E.AverageLongitudeCache <> 0 THEN 'Georeference' ELSE '-' END AS [Has Geography], count(*) As No " +
                    "FROM CollectionEventLocalisation AS E INNER JOIN " +
                    "LocalisationSystem AS L ON E.LocalisationSystemID = L.LocalisationSystemID INNER JOIN " +
                    "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID " +
                    "INNER JOIN #TempIDs T ON S.CollectionSpecimenID = T.CollectionSpecimenID  " +
                    "GROUP BY L.DisplayText, CASE WHEN E.AverageLatitudeCache <> 0 AND E.AverageLongitudeCache <> 0 THEN 'Georeference' ELSE '-' END " +
                    "ORDER BY L.DisplayText";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //ad.Fill(dt);
                this.dataGridViewQueryLocalisation.DataSource = dt;
                this.dataGridViewQueryLocalisation.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex) 
            {
                Message = "Retrieving statistics for localisation failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryImages(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT COUNT (*) AS [Number] " +
                ",case when URI like 'http://%' then substring (URI, 1, 8) + SUBSTRING (URI, 9, charindex('/', substring(URI, 9, 500))) else NULL end as [Server] " +
                ",[ImageType] " +
                "FROM [CollectionSpecimenImage] I, CollectionProject P " +
                "INNER JOIN #TempIDs T ON T.CollectionSpecimenID = P.CollectionSpecimenID  " +
                "WHERE I.CollectionSpecimenID = P.CollectionSpecimenID " +
                "GROUP BY [ImageType], case when URI like 'http://%' then substring (URI, 1, 8) + SUBSTRING (URI, 9, charindex('/', substring(URI, 9, 500))) else NULL end";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //ad.Fill(dt);
                this.dataGridViewQueryImages.DataSource = dt;
                this.dataGridViewQueryImages.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                SQL = "SELECT COUNT (*) AS [Number] " +
                "FROM [CollectionSpecimenImage] I, CollectionProject P " +
                "INNER JOIN #TempIDs T ON T.CollectionSpecimenID = P.CollectionSpecimenID  " +
                "WHERE I.CollectionSpecimenID = P.CollectionSpecimenID ";// and I.CollectionSpecimenID IN (" + this.IDlist + ") ";
                string Nr = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this._SqlConnectionQuery);
                if (Nr.Length == 0)
                    Message = "Retrieving statistics for images failed";
                else
                    this.labelQueryImages.Text = "Number of images: " + Nr;
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for images failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryFamilies(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT    I.FamilyCache AS Family, COUNT(*) AS No " +
                     "FROM         IdentificationUnit AS I  " +
                    //"WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                    "INNER JOIN #TempIDs T ON I.CollectionSpecimenID = T.CollectionSpecimenID  " +
                     "GROUP BY I.FamilyCache " +
                     "ORDER BY [Family]";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryFamily.DataSource = dt;
                this.labelQueryFamilyNr.Text = dt.Rows.Count.ToString();
                this.dataGridViewQueryFamily.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for families failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryOrders(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT    I.OrderCache AS [Order], COUNT(*) AS No " +
                "FROM         IdentificationUnit AS I  " +
                //    "WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                    "INNER JOIN #TempIDs T ON I.CollectionSpecimenID = T.CollectionSpecimenID  " +
                "GROUP BY I.OrderCache " +
                "ORDER BY [Order]";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryOrder.DataSource = dt;
                this.labelQueryOrderCount.Text = dt.Rows.Count.ToString();
                this.dataGridViewQueryOrder.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for orders failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryTaxonomicGroups(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT I.TaxonomicGroup AS [Taxonomic group], COUNT(*) AS No " +
                     "FROM IdentificationUnit AS I  " +
                    //"WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                    "INNER JOIN #TempIDs T ON I.CollectionSpecimenID = T.CollectionSpecimenID  " +
                     "GROUP BY I.TaxonomicGroup " +
                     "ORDER BY [Taxonomic group]";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryTaxonomicGroups.DataSource = dt;
                this.labelQueryTaxonomicGroupCount.Text = dt.Rows.Count.ToString();
                this.dataGridViewQueryTaxonomicGroups.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for date range failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryTaxonomicGroupTaxa(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT I.TaxonomicGroup AS [Taxonomic group], COUNT(Distinct LastIdentificationCache) AS Taxa " +
                     "FROM IdentificationUnit AS I " +
                    //"WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                    "INNER JOIN #TempIDs T ON I.CollectionSpecimenID = T.CollectionSpecimenID  " +
                     "GROUP BY I.TaxonomicGroup " +
                     "ORDER BY [Taxonomic group]";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryGroupTaxa.DataSource = dt;
                this.labelQueryTaxonomicGroupCount.Text = dt.Rows.Count.ToString();
                this.dataGridViewQueryGroupTaxa.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for taxonomic groups failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryTaxa(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT  I.TaxonomicGroup AS [Taxonomic group], I.LastIdentificationCache AS Taxon, COUNT(*) AS No " +
               "FROM IdentificationUnit AS I  " +
                //     "WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                "INNER JOIN #TempIDs T ON I.CollectionSpecimenID = T.CollectionSpecimenID  " +
                "GROUP BY I.TaxonomicGroup,  I.LastIdentificationCache " +
                "ORDER BY [Taxon]";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryTaxa.DataSource = dt;
                this.labelQueryTaxaNr.Text = dt.Rows.Count.ToString();
                this.dataGridViewQueryTaxa.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for taxa failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryTypes(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT     I.TypeStatus AS Type, COUNT(*) AS No " +
                    "FROM         Identification AS I  " +
                    //"WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                    "INNER JOIN #TempIDs T ON I.CollectionSpecimenID = T.CollectionSpecimenID  " +
                    "GROUP BY I.TypeStatus " +
                    "HAVING      (NOT (I.TypeStatus IS NULL)) " +
                    "ORDER BY Type";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryType.DataSource = dt;
                this.dataGridViewQueryType.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for types failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryCollectors(int? Timeout = null)
        {
            string Message = "";
            try
            {
                //string SQL = "SELECT     A.CollectorsName AS Collector, COUNT(*) AS No " +
                //    "FROM         CollectionAgent AS A  " +
                //    "WHERE A.CollectionSpecimenID IN (" + this.IDlist + ") " +
                //    "GROUP BY A.CollectorsName";
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT     A.CollectorsName AS Collector, COUNT(*) AS No " +
                    "FROM         CollectionAgent AS A, #TempIDs T " +
                    "WHERE A.CollectionSpecimenID = T.CollectionSpecimenID " +
                    "GROUP BY A.CollectorsName";
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryCollectors.DataSource = dt;
                this.labelQueryCollectorsNr.Text = dt.Rows.Count.ToString();
                this.dataGridViewQueryCollectors.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Retrieving statistics for collectors failed:\r\n" + ex.Message);
            }
            return Message;
        }

        private string getQueryParts(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT C.CollectionName AS [Collection name], S.MaterialCategory AS [Material category], COUNT(*) AS No " +
                    "FROM CollectionSpecimenPart AS S LEFT OUTER JOIN " +
                    "Collection AS C ON S.CollectionID = C.CollectionID " +
                    //"WHERE S.CollectionSpecimenID IN (" + this.IDlist + ") " +
                    "INNER JOIN #TempIDs T ON S.CollectionSpecimenID = T.CollectionSpecimenID  " +
                    "GROUP BY S.MaterialCategory, C.CollectionName " +
                    "ORDER BY [Collection name], [Material category]";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryParts.DataSource = dt;
                this.dataGridViewQueryParts.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                int No = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    int i;
                    if (int.TryParse(R["No"].ToString(), out i))
                        No += i;
                }
                this.labelQueryPartsNr.Text = No.ToString();
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for parts failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryTaxonData(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT I.TaxonomicName AS [Taxonomic name], COUNT(DISTINCT I.IdentificationUnitID) AS Recurrence, " +
                    "CONVERT(nvarchar(10), MIN(E.CollectionDate), 111) AS [First date], CONVERT(nvarchar(10), " +
                    "MAX(E.CollectionDate), 111) AS [Last date], MIN(L.AverageLatitudeCache) AS South, " +
                    "MAX(L.AverageLatitudeCache) AS North, MIN(L.AverageLongitudeCache) AS West, MAX(L.AverageLongitudeCache) AS East " +
                    "FROM CollectionSpecimen AS S INNER JOIN " +
                    "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID INNER JOIN " +
                    "Identification AS I ON S.CollectionSpecimenID = I.CollectionSpecimenID LEFT OUTER JOIN " +
                    "CollectionEventLocalisation AS L ON E.CollectionEventID = L.CollectionEventID " +
                    //"WHERE (S.CollectionSpecimenID IN (" + this.IDlist + ")) " +
                    "INNER JOIN #TempIDs T ON S.CollectionSpecimenID = T.CollectionSpecimenID  " +
                    "GROUP BY I.TaxonomicName " +
                    "ORDER BY I.TaxonomicName";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryTaxonData.DataSource = dt;
                this.dataGridViewQueryTaxonData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for taxon data failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryProjectsSpecimen(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT P.Project, COUNT(*) AS Specimens " +
                    "FROM CollectionProject S INNER JOIN " +
                    "ProjectProxy P ON S.ProjectID = P.ProjectID " +
                    //"WHERE (S.CollectionSpecimenID IN (" + this.IDlist + ")) " +
                    "INNER JOIN #TempIDs T ON S.CollectionSpecimenID = T.CollectionSpecimenID  " +
                    "GROUP BY P.Project";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryProjectsPerSpecimen.DataSource = dt;
                this.dataGridViewQueryProjectsPerSpecimen.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for specimen per project failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryProjectsUnit(int? Timeout = null)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT P.Project, COUNT(*) AS Organisms " +
                    "FROM IdentificationUnit U, CollectionProject S INNER JOIN " +
                    "ProjectProxy P ON S.ProjectID = P.ProjectID " +
                    "INNER JOIN #TempIDs T ON S.CollectionSpecimenID = T.CollectionSpecimenID  " +
                    "WHERE U.CollectionSpecimenID = S.CollectionSpecimenID " + //AND (S.CollectionSpecimenID IN (" + this.IDlist + ")) " +
                    "GROUP BY P.Project";
                System.Data.DataTable dt = new DataTable();
                this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                //ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
                this.dataGridViewQueryProjectsPerUnit.DataSource = dt;
                this.dataGridViewQueryProjectsPerUnit.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                Message = "Retrieving statistics for organisms per project failed:\r\n" + ex.Message;
            }
            return Message;
        }

        private string initCacheProjectList()
        {
            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length == 0)
                return "";
            try
            {
                
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQueryUnitInPartProject.SelectedItem;
                string SQL = "SELECT ProjectID, Project FROM ProjectPublished ORDER BY Project";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDBWithTimeOut(3));
                ad.SelectCommand.CommandTimeout = 3;
                ad.Fill(dt);
                this.comboBoxQueryUnitInPartProject.DataSource = dt;
                this.comboBoxQueryUnitInPartProject.DisplayMember = "Project";
                this.comboBoxQueryUnitInPartProject.ValueMember = "ProjectID";
            }
            catch (Exception ex)
            {
                this.labelUnitInPartMayBe.Text = "No access to cache database:\r\n" + ex.Message;
                this.labelUnitInPartMayBe.ForeColor = System.Drawing.Color.Red;
                //Message = "No access to cache database:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryCacheUnitInParts(int? Timeout = null)
        {
            string Message = "";
            try
            {
                if (this.comboBoxQueryUnitInPartProject.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQueryUnitInPartProject.SelectedItem;
                    string Schema = "Project_" + R["Project"].ToString();
                    //string SQL = "SELECT        COUNT(*) AS Total " +
                    //    "FROM  " + Schema + ".CacheIdentificationUnitInPart U " +
                    //    "WHERE     (U.CollectionSpecimenID IN (" + this.IDlist + "))";
                    string SQL = "SELECT        COUNT(*) AS Total " +
                        "FROM  " + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "." + Schema + ".CacheIdentificationUnitInPart U, #TempIDs T " +
                        "WHERE  U.CollectionSpecimenID = T.CollectionSpecimenID";
                    System.Data.DataTable dt = new DataTable();
                    this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                    //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    //ad.Fill(dt);
                    if (Message.Length > 0)
                    {
                        this.labelUnitInPartMayBe.Text = "No access to cache database:\r\n" + Message;
                        this.labelUnitInPartMayBe.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.dataGridViewQueryUnitInPartCache.DataSource = dt;
                        this.dataGridViewQueryUnitInPartCache.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                }
            }
            catch (Exception ex)
            {
                this.labelUnitInPartMayBe.Text = "No access to cache database:\r\n" + ex.Message;
                this.labelUnitInPartMayBe.ForeColor = System.Drawing.Color.Red;
                //Message = "No access to cache database:\r\n" + ex.Message;
            }
            return Message;
        }

        private string getQueryCacheUnitInPartsGeo(int? Timeout = null)
        {
            string Message = "";
            try
            {
                if (this.comboBoxQueryUnitInPartProject.SelectedItem != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxQueryUnitInPartProject.SelectedItem;
                    string Schema = "Project_" + R["Project"].ToString();
                    string SQL = "SELECT COUNT(*) AS Total " +
                        "FROM " + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "." + Schema + ".CacheCollectionEventLocalisation AS L, " + Schema + ".CacheIdentificationUnitInPart AS U INNER JOIN " +
                        DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "." + Schema + ".CollectionSpecimenCache AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID " +
                        "INNER JOIN #TempIDs T ON S.CollectionSpecimenID = T.CollectionSpecimenID  " +
                        "WHERE L.CollectionEventID = S.CollectionEventID " + // AND (S.CollectionSpecimenID IN (" + this.IDlist + ")) " 
                        "AND (L.AverageLatitudeCache <> 0) AND (L.AverageLongitudeCache <> 0)";
                    System.Data.DataTable dt = new DataTable();
                    this.FillTableForQuery(ref dt, SQL, ref Message, Timeout);
                    //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    //if (Timeout != null)
                    //    ad.SelectCommand.CommandTimeout = (int)Timeout;
                    //else
                    //    ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                    //ad.Fill(dt);
                    if (Message.Length > 0)
                    {
                        this.labelUnitInPartMayBe.Text = "No access to cache database:\r\n" + Message;
                        this.labelUnitInPartMayBe.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.dataGridViewQueryUnitInPartCacheGeoDetail.DataSource = dt;
                        this.dataGridViewQueryUnitInPartCacheGeoDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    }
                }
            }
            catch (Exception ex)
            {
                this.labelUnitInPartMayBe.Text = "No access to cache database:\r\n" + ex.Message;
                this.labelUnitInPartMayBe.ForeColor = System.Drawing.Color.Red;
                //Message = "No access to cache database:\r\n" + ex.Message;
            }
            return Message;
        }

        private Microsoft.Data.SqlClient.SqlConnection _SqlConnectionQuery;
        private Microsoft.Data.SqlClient.SqlConnection SqlConnectionQuery
        {
            get
            {
                if (this._SqlConnectionQuery == null)
                {
                    this._SqlConnectionQuery = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                }
                return this._SqlConnectionQuery;
            }
        }

        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterQuery;

        private void FillTableForQuery(ref System.Data.DataTable dt, string SQL, ref string Message, int? Timeout = null)
        {
            try
            {
                if (this._sqlDataAdapterQuery == null)
                {
                    this._sqlDataAdapterQuery = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.SqlConnectionQuery);
                    string SqlTempTable = "CREATE TABLE #TempIDs (CollectionSpecimenID int NOT NULL PRIMARY KEY)";
                    Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(SqlTempTable, this.SqlConnectionQuery);
                    if (this.SqlConnectionQuery.State == ConnectionState.Closed)
                        this.SqlConnectionQuery.Open();
                    command.ExecuteNonQuery();
                    if (this._SqlForIDs != null && this._SqlForIDs.Length > 0)
                    {
                        string SqlIDs = _SqlForIDs.Substring(0, _SqlForIDs.IndexOf(" AS ID") + 6) + _SqlForIDs.Substring(_SqlForIDs.IndexOf(" FROM "));
                        if (SqlIDs.IndexOf(" ORDER BY ") > -1)
                            SqlIDs = SqlIDs.Substring(0, SqlIDs.IndexOf("ORDER BY "));
                        command.CommandText = "INSERT INTO #TempIDs (CollectionSpecimenID) " + SqlIDs;
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        foreach(int ID in this._IDs)
                        {
                            command.CommandText = "INSERT INTO #TempIDs (CollectionSpecimenID) Values (" + ID.ToString() + ")";
                            command.ExecuteNonQuery();
                        }

                    }
                }
                else
                    this._sqlDataAdapterQuery.SelectCommand.CommandText = SQL;
                if (Timeout != null)
                    this._sqlDataAdapterQuery.SelectCommand.CommandTimeout = (int)Timeout;
                else
                    this._sqlDataAdapterQuery.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                this._sqlDataAdapterQuery.Fill(dt);

                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                //if (Timeout != null)
                //    ad.SelectCommand.CommandTimeout = (int)Timeout;
                //else
                //    ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                //ad.Fill(dt);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }


#endregion

#region TaxonAnalysis

        private void comboBoxQueryTaxonAnalysisSource_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxQueryTaxonAnalysisSource.Items.Count == 0)
            {
                foreach (DiversityWorkbench.DatabaseService DS in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].DatabaseServices())
                {
                    this.comboBoxQueryTaxonAnalysisSource.Items.Add(DS.DisplayText);
                }
            }
        }

        private void comboBoxQueryTaxonAnalysisSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Prefix = "dbo.";
                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList()[this.comboBoxQueryTaxonAnalysisSource.Text];
                if (SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                else Prefix = "[" + SC.DatabaseName + "].dbo.";
                string SQL = "SELECT [AnalysisID] " +
                    ",[DisplayText] + case when A.Description <> '' then ' (= ' + A.Description + ')' else '' end as Analysis " +
                    "FROM " + Prefix + "[TaxonNameListAnalysisCategory] A";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxQueryTaxonAnalysis.DataSource = dt;
                this.comboBoxQueryTaxonAnalysis.DisplayMember = "Analysis";
                this.comboBoxQueryTaxonAnalysis.ValueMember = "AnalysisID";
                this.comboBoxQueryTaxonAnalysis.Enabled = true;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                this.comboBoxQueryTaxonAnalysis.Enabled = false;
            }
        }

        private void comboBoxQueryTaxonAnalysis_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBoxQueryTaxonAnalysisResult.Text = "";
            this.textBoxTaxonAnalysisGetResultTaxa.Text = "";
            this.textBoxQueryTaxonAnalysisGetResultNumberOfUnits.Text = "";
            this.dataGridViewQueryTaxonAnalysisList.DataSource = null;
            this.buttonQueryTaxonAnalysisGetResult.Enabled = true;
            this.buttonTaxonAnalysisGetResultTaxa.Enabled = true;
            this.buttonQueryTaxonAnalysisGetResultNumberOfUnits.Enabled = true;

        }

        private void buttonQueryTaxonAnalysisGetResultNumberOfUnits_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList()[this.comboBoxQueryTaxonAnalysisSource.Text];
                string Prefix = "dbo.";
                if (SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                else Prefix = "[" + SC.DatabaseName + "].dbo.";
                string SQL = "select avg(cast(A.[AnalysisValue] as float) * cast(case when U.NumberOfUnits is null then 1 else U.NumberOfUnits end as float)) " +
                    " from " + Prefix + "[TaxonNameListAnalysis] A,  " +
                " " + Prefix + "[TaxonNameListAnalysisCategory] C, Identification I, IdentificationUnit U " +
                " WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                " and I.NameURI = '" + SC.BaseURL + "' + CAST(A.NameID as varchar) " +
                " and A.[AnalysisID] = " + this.comboBoxQueryTaxonAnalysis.SelectedValue.ToString() +
                " and A.AnalysisID = C.AnalysisID " +
                " and isnumeric(A.[AnalysisValue]) = 1  " +
                " and try_parse(A.[AnalysisValue] as decimal) is not null " +
                " and I.IdentificationUnitID = U.IdentificationUnitID " +
                " and I.TaxonomicName = U.LastIdentificationCache ";

                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this.textBoxQueryTaxonAnalysisGetResultNumberOfUnits.Text = Result;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonQueryTaxonAnalysisGetResult_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList()[this.comboBoxQueryTaxonAnalysisSource.Text];
                string Prefix = "dbo.";
                if (SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                else Prefix = "[" + SC.DatabaseName + "].dbo.";
                string SQL = "select avg(cast(A.[AnalysisValue] as float)) " +
                    " from " + Prefix + "[TaxonNameListAnalysis] A,  " +
                " " + Prefix + "[TaxonNameListAnalysisCategory] C, Identification I, IdentificationUnit U " +
                " WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                " and I.NameURI = '" + SC.BaseURL + "' + CAST(A.NameID as varchar) " +
                " and A.[AnalysisID] = " + this.comboBoxQueryTaxonAnalysis.SelectedValue.ToString() +
                " and A.AnalysisID = C.AnalysisID " +
                " and isnumeric(A.[AnalysisValue]) = 1  " +
                " and try_parse(A.[AnalysisValue] as decimal) is not null " +
                " and I.IdentificationUnitID = U.IdentificationUnitID " +
                " and I.TaxonomicName = U.LastIdentificationCache ";

                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this.textBoxQueryTaxonAnalysisResult.Text = Result;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void buttonTaxonAnalysisGetResultTaxa_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList()[this.comboBoxQueryTaxonAnalysisSource.Text];
                string Prefix = "dbo.";
                if (SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                else Prefix = "[" + SC.DatabaseName + "].dbo.";
                string SQL = "select distinct rtrim(substring(I.NameURI, " + (SC.BaseURL.Length + 1).ToString() + ", 255)) AS NameID " +
                    " from Identification I, IdentificationUnit U " +
                " WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") " +
                " and I.NameURI LIKE '" + SC.BaseURL + "%' " +
                " and I.IdentificationUnitID = U.IdentificationUnitID " +
                " and I.TaxonomicName = U.LastIdentificationCache ";

                System.Data.DataTable dtNameIDs = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtNameIDs);
                string NameIDs = "";
                foreach (System.Data.DataRow R in dtNameIDs.Rows)
                {
                    if (NameIDs.Length > 0) NameIDs += ", ";
                    NameIDs += R[0].ToString();
                }

                SQL = "select avg(cast(A.[AnalysisValue] as float)) " +
                    " from " + Prefix + "[TaxonNameListAnalysis] A " +
                    " where A.[AnalysisID] = " + this.comboBoxQueryTaxonAnalysis.SelectedValue.ToString() +
                    " and isnumeric(A.[AnalysisValue]) = 1 " +
                    " and try_parse(A.[AnalysisValue] as decimal) is not null " +
                    " and A.NameID IN(" + NameIDs + ")";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                this.textBoxTaxonAnalysisGetResultTaxa.Text = Result;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonQueryTaxonAnalysisGetList_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList()[this.comboBoxQueryTaxonAnalysisSource.Text];
                string Prefix = "dbo.";
                if (SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                else Prefix = "[" + SC.DatabaseName + "].dbo.";

                string SQL = "select A.[AnalysisValue] AS [" + this.comboBoxQueryTaxonAnalysis.Text.Replace("'", "''") + "], " +
                    " sum(case when U.NumberOfUnits is null then 1 else U.NumberOfUnits end) AS Individuals, " +
                    " count(*) AS Occurences, count(distinct I.NameURI) as Taxa " +
                    " from " + Prefix + "[TaxonNameListAnalysis] A, Identification I, IdentificationUnit U " +
                    " WHERE I.CollectionSpecimenID IN (" + this.IDlist + ") and I.NameURI = '" + SC.BaseURL + "' + CAST(A.NameID as varchar) " +
                    " and A.[AnalysisID] = " + this.comboBoxQueryTaxonAnalysis.SelectedValue.ToString() +
                    " and I.IdentificationUnitID = U.IdentificationUnitID " +
                    " and I.TaxonomicName = U.LastIdentificationCache " +
                    " group by A.[AnalysisValue] " +
                    " order by A.[AnalysisValue]";
                System.Data.DataTable dtList = new DataTable();
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtList, ref Message);
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //ad.Fill(dtList);
                this.dataGridViewQueryTaxonAnalysisList.DataSource = dtList;
                this.dataGridViewQueryTaxonAnalysisList.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#endregion

#region Transaction

#region Institution

        private void buttonTransactionSchemaOpen_Click(object sender, EventArgs e)
        {
            string Path = Folder.Transaction(Folder.TransactionFolder.Statistics);// ....StartupPath + "\\Transaction\\Schemas\\Statistics";
            if (this.textBoxTransactionSchema.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxTransactionSchema.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory = Path;
            this.openFileDialog.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.textBoxTransactionSchema.Tag = this.openFileDialog.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxTransactionSchema.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string _XmlTransaction = "";

        private void buttonTransactionStart_Click(object sender, EventArgs e)
        {
            if (this.userControlModuleRelatedEntryTransactionInstitution.textBoxValue.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select an institution");
                return;
            }

            string Path = Folder.Transaction() + "Statistics.XML";
            System.IO.FileInfo XML = new System.IO.FileInfo(Path);
            System.IO.FileInfo Schema = new System.IO.FileInfo(this.textBoxTransactionSchema.Text);
            this._XmlTransaction = DiversityCollection.Transaction.XmlStatistics(
                this.userControlModuleRelatedEntryTransactionInstitution.textBoxValue.Text,
                this.userControlModuleRelatedEntryTransactionInstitution.labelURI.Text,
                XML,
                Schema,
                this.dateTimePickerTransactionFrom.Value,
                this.dateTimePickerTransactionUntil.Value);
            if (this._XmlTransaction.Length > 0)
            {
                System.Uri U = new Uri(this._XmlTransaction);
                this.userControlWebViewTransaction.Url = null;
                this.userControlWebViewTransaction.Navigate(U);
                //this.web BrowserTransaction.Url = U;
            }
            else
                System.Windows.Forms.MessageBox.Show("Creation of statistics failed");

        }
        
#region Country
        
        private void buttonTransactionAddressSourceSchema_Click(object sender, EventArgs e)
        {
            string Path = Folder.Transaction(Folder.TransactionFolder.Statistics);
            if (this.textBoxTransactionAddressSourceSchema.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxTransactionAddressSourceSchema.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory = Path;
            this.openFileDialog.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.textBoxTransactionAddressSourceSchema.Tag = this.openFileDialog.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxTransactionAddressSourceSchema.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonTransactionAddressSourceStart_Click(object sender, EventArgs e)
        {
            if (this.textBoxTransactionAddressSourceSchema.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid schema");
                return;
            }
            if (this.userControlModuleRelatedEntryTransactionInstitution.textBoxValue.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select an institution");
                return;
            }
            if (this.comboBoxTransactionAddressSource.SelectedItem.ToString().Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a source for the addresses");
                return;
            }

            string Path = Folder.Transaction() + "StatisticsCountry.XML";
            System.IO.FileInfo XML = new System.IO.FileInfo(Path);
            System.IO.FileInfo Schema = new System.IO.FileInfo(this.textBoxTransactionAddressSourceSchema.Text);
            this._XmlTransaction = DiversityCollection.Transaction.XmlStatistics(
                this.userControlModuleRelatedEntryTransactionInstitution.textBoxValue.Text,
                this.comboBoxTransactionAddressSource.SelectedItem.ToString(),
                this.userControlModuleRelatedEntryTransactionInstitution.labelURI.Text,
                XML,
                Schema,
                this.dateTimePickerTransactionFrom.Value,
                this.dateTimePickerTransactionUntil.Value);
            if (this._XmlTransaction.Length > 0)
            {
                System.Uri U = new Uri(this._XmlTransaction);
                this.userControlWebViewTransactionCountry.Url = null;
                this.userControlWebViewTransactionCountry.Navigate(U);
                //this.web BrowserTransactionCountry.Url = U;
            }
            else
                System.Windows.Forms.MessageBox.Show("Creation of statistics failed");
        }
        
#endregion

#endregion

#region Content
        
        private void buttonTransactionContentListResults_Click(object sender, EventArgs e)
        {
            this.dataGridViewTransactionContent.DataSource = null;
            this.dataGridViewTransactionContent.Columns.Clear();
            string SQL = "SELECT S.CollectionSpecimenID, T.TransactionTitle AS [Transaction], T.TransactionType AS [Transaction type], ";
            if (this.radioButtonTransactionContentReturned.Checked)
                SQL += "TR.TransactionTitle AS [Return], "; //TR.TransactionType AS ReturnType, ";
            SQL += "S.AccessionNumber, U.LastIdentificationCache AS [Last identification], " +
                "SP.MaterialCategory AS [Material category], SP.StorageLocation AS [Storage location] " +
                "FROM CollectionSpecimenTransaction AS ST INNER JOIN " +
                "[Transaction] AS T ON ST.TransactionID = T.TransactionID INNER JOIN " +
                "IdentificationUnit AS U ON ST.CollectionSpecimenID = U.CollectionSpecimenID INNER JOIN " +
                "CollectionSpecimenPart AS SP ON ST.CollectionSpecimenID = SP.CollectionSpecimenID AND ST.SpecimenPartID = SP.SpecimenPartID INNER JOIN " +
                "CollectionProject AS P ON ST.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                "CollectionSpecimen AS S ON U.CollectionSpecimenID = S.CollectionSpecimenID AND SP.CollectionSpecimenID = S.CollectionSpecimenID AND  " +
                "P.CollectionSpecimenID = S.CollectionSpecimenID LEFT OUTER JOIN " +
                "[Transaction] AS TR ON ST.TransactionReturnID = TR.TransactionID AND T.TransactionID = TR.ParentTransactionID " +
                "WHERE 1 = 1 ";
            if (this.comboBoxTransactionContentRestrictToCollection.DataSource != null && this.comboBoxTransactionContentRestrictToCollection.SelectedValue != null)
            {
                SQL += " AND SP.CollectionID = " + this.comboBoxTransactionContentRestrictToCollection.SelectedValue.ToString();
            }
            if (this.comboBoxTransactionContentRestrictToMaterial.DataSource != null
                && this.comboBoxTransactionContentRestrictToMaterial.SelectedValue != null
                && this.comboBoxTransactionContentRestrictToMaterial.SelectedValue.ToString().Length > 0)
            {
                SQL += " AND SP.MaterialCategory = '" + this.comboBoxTransactionContentRestrictToMaterial.SelectedValue.ToString() + "'";
            }
            if (this.comboBoxTransactionContentRestrictToTransaction.DataSource != null
                && this.comboBoxTransactionContentRestrictToTransaction.SelectedValue != null
                && this.comboBoxTransactionContentRestrictToTransaction.SelectedValue.ToString().Length > 0)
            {
                SQL += " AND T.TransactionType = '" + this.comboBoxTransactionContentRestrictToTransaction.SelectedValue.ToString() + "'";
            }
            if (this.comboBoxTransactionContentRestrictToProject.DataSource != null
                && this.comboBoxTransactionContentRestrictToProject.SelectedValue != null
                && this.comboBoxTransactionContentRestrictToProject.SelectedValue.ToString().Length > 0)
            {
                SQL += " AND P.ProjectID = " + this.comboBoxTransactionContentRestrictToProject.SelectedValue.ToString();
            }
            if (this.radioButtonTransactionContentReturned.Checked)
            {
                SQL += " AND TR.TransactionType = 'return'";
            }
            else if (this.radioButtonTransactionContentNotReturned.Checked)
            {
                SQL += " AND TR.TransactionType IS NULL ";
            }
            if (this.textBoxTransactionContentRestrictToLastIdent.Text.Length > 0)
            {
                SQL += " AND U.LastIdentificationCache LIKE '" + this.textBoxTransactionContentRestrictToLastIdent.Text + "%'";
            }
            try
            {
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.dataGridViewTransactionContent.DataSource = dt;
                this.dataGridViewTransactionContent.Columns[0].Visible = false;
            }
            catch (System.Exception ex) { }
        }

        private void comboBoxTransactionContentRestrictToCollection_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxTransactionContentRestrictToCollection.DataSource == null)
            {
                this.comboBoxTransactionContentRestrictToCollection.DataSource = DiversityCollection.LookupTable.DtCollectionWithHierarchy;
                this.comboBoxTransactionContentRestrictToCollection.DisplayMember = "DisplayText";
                this.comboBoxTransactionContentRestrictToCollection.ValueMember = "CollectionID";
            }
        }

        private void comboBoxTransactionContentRestrictToMaterial_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxTransactionContentRestrictToMaterial.DataSource == null)
            {
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTransactionContentRestrictToMaterial, "CollMaterialCategory_Enum", DiversityWorkbench.Settings.Connection, true);
            }
        }

        private void comboBoxTransactionContentRestrictToTransaction_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxTransactionContentRestrictToTransaction.DataSource == null)
            {
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTransactionContentRestrictToTransaction, "CollTransactionType_Enum", DiversityWorkbench.Settings.Connection, true);
            }
        }

        private void comboBoxTransactionContentRestrictToProject_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxTransactionContentRestrictToProject.DataSource == null)
            {
                System.Data.DataTable dtProject = DiversityCollection.LookupTable.DtProjectList;
                System.Data.DataRow R = dtProject.NewRow();
                R[0] = System.DBNull.Value;
                R[1] = System.DBNull.Value;
                dtProject.Rows.Add(R);
                this.comboBoxTransactionContentRestrictToProject.DataSource = dtProject;
                this.comboBoxTransactionContentRestrictToProject.DisplayMember = "Project";
                this.comboBoxTransactionContentRestrictToProject.ValueMember = "ProjectID";
            }
        }

        private void buttonTransactionContentViewDataset_Click(object sender, EventArgs e)
        {
            int ID;
            if (int.TryParse(this.dataGridViewTransactionContent.Rows[this.dataGridViewTransactionContent.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out ID))
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                DiversityCollection.Forms.FormCollectionSpecimen f = new Forms.FormCollectionSpecimen(ID, false, false);
                f.Width = this.Width - 10;
                f.Height = this.Height - 10;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
            }
        }

        #endregion

        #region History
        /*
         * gewünschte Ausbage war ab 2020, Land, Stadt, Name, Jahr, Anzahl, Typ ...
         * 
        SELECT CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN Ato.Country ELSE Afrom.Country END AS Country,
                  CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN Ato.City ELSE Afrom.City END AS City,
                  CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN T .ToTransactionPartnerName ELSE T.FromTransactionPartnerName END AS TransactionPartner, YEAR(T.BeginDate)
                  AS Jahr, T.TransactionType, Count(*) AS Transaktionen, SUM(T.NumberOfUnits) AS NumberOfUnits
        FROM            TransactionList_H7 AS T INNER JOIN
                          DiversityAgents.dbo.AgentContactInformation AS Afrom ON T.FromTransactionPartnerAgentURI = DiversityAgents.dbo.BaseURL() + CAST(Afrom.AgentID AS varchar) INNER JOIN

                          DiversityAgents.dbo.AgentContactInformation AS Ato ON T.ToTransactionPartnerAgentURI = DiversityAgents.dbo.BaseURL() + CAST(Ato.AgentID AS varchar)
        WHERE        ((T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%') OR
                          (T.ToTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%'))
						         AND YEAR(T.BeginDate) >= 2020
        GROUP BY YEAR(T.BeginDate), T.TransactionType, T.FromTransactionPartnerName, T.ToTransactionPartnerName, Ato.Country, Afrom.Country, Ato.City, Afrom.City
        HAVING(T.TransactionType IN (N'loan', N'return', N'exchange'))

        UNION

        SELECT        CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN Ato.Country ELSE Afrom.Country END AS Country,
                                CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN Ato.City ELSE Afrom.City END AS City,
                                CASE WHEN T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%' THEN T .ToTransactionPartnerName ELSE T.FromTransactionPartnerName END AS TransactionPartner, YEAR(T.BeginDate)
                                AS Jahr, R.TransactionType, COUNT(*) AS Transaktionen, SUM(T.NumberOfUnits) AS NumberOfUnits
        FROM            TransactionList_H7 AS T INNER JOIN
                                DiversityAgents.dbo.AgentContactInformation AS Afrom ON T.FromTransactionPartnerAgentURI = DiversityAgents.dbo.BaseURL() + CAST(Afrom.AgentID AS varchar) INNER JOIN

                                DiversityAgents.dbo.AgentContactInformation AS Ato ON T.ToTransactionPartnerAgentURI = DiversityAgents.dbo.BaseURL() + CAST(Ato.AgentID AS varchar) INNER JOIN

                                [Transaction] AS R ON T.TransactionID = R.ParentTransactionID
        WHERE        (T.FromTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%') AND(YEAR(T.BeginDate) >= 2020) OR
                               (YEAR(T.BeginDate) >= 2020) AND(T.ToTransactionPartnerName LIKE '%Botanische Staatssammlung M%nchen%')
        GROUP BY YEAR(T.BeginDate), T.TransactionType, T.FromTransactionPartnerName, T.ToTransactionPartnerName, Ato.Country, Afrom.Country, Ato.City, Afrom.City, R.TransactionType
        HAVING(R.TransactionType = N'return') AND(T.TransactionType = N'loan')
        ORDER BY Country, City, TransactionPartner, Jahr
        */

        private void initTransactionHistory()
        {
            // wird in FormTransaction umgesetzt
            this.tabControlTransaction.TabPages.Remove(this.tabPageTransactionHistory);
        }

        private void buttonTransactionHistoryStart_Click(object sender, EventArgs e)
        {

        }

        #endregion


        #endregion

        #region Projects

        #region Numbers

        private void buttonGetProjectSpecimen_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Login = "";
            string SqlVersion = "select user_name()";
            Microsoft.Data.SqlClient.SqlConnection ConVersion = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            Microsoft.Data.SqlClient.SqlCommand CVersion = new Microsoft.Data.SqlClient.SqlCommand(SqlVersion, ConVersion);
            try
            {
                ConVersion.Open();
                Login = CVersion.ExecuteScalar().ToString();
                ConVersion.Close();
            }
            catch { }
            //Projects
            string SQL = "SELECT PP.Project, PP.ProjectID AS [Project ID], COUNT(DISTINCT P.CollectionSpecimenID) " +
                "AS [Number of specimens], MIN(S.LogCreatedWhen) AS [Start date], MAX(S.LogUpdatedWhen) AS [Last changes in specimens] " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "ProjectProxy AS PP INNER JOIN " +
                "CollectionProject AS P ON PP.ProjectID = P.ProjectID ON S.CollectionSpecimenID = P.CollectionSpecimenID ";
            if (Login != "dbo")
                SQL += "WHERE PP.ProjectID IN (SELECT ProjectID FROM ProjectList)";
            SQL += "GROUP BY PP.ProjectID, PP.Project " +
                "ORDER BY PP.Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
            try
            {
                _dtProject = new DataTable();
                ad.Fill(_dtProject);
                this.dataGridViewProjects.DataSource = _dtProject;
                this.dataGridViewProjects.ReadOnly = true;
                this.dataGridViewProjects.AutoResizeColumns();
                //this.dataGridViewProjects.ColumnHeadersHeight = 70;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Retrieving statistics for projects failed:\r\n" + ex.Message);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonGetProjectEvents_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Login = "";
            string SqlVersion = "select user_name()";
            Microsoft.Data.SqlClient.SqlConnection ConVersion = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            Microsoft.Data.SqlClient.SqlCommand CVersion = new Microsoft.Data.SqlClient.SqlCommand(SqlVersion, ConVersion);
            try
            {
                ConVersion.Open();
                Login = CVersion.ExecuteScalar().ToString();
                ConVersion.Close();
            }
            catch { }
            //Projects
            string SQL = "SELECT PP.Project, COUNT(DISTINCT S.CollectionEventID) AS [Collection events], " +
                " CASE WHEN MAX(E.LogUpdatedWhen) > MAX(L.LogUpdatedWhen) OR " +
                "MAX(L.LogUpdatedWhen) IS NULL THEN MAX(E.LogUpdatedWhen) ELSE MAX(L.LogUpdatedWhen) END AS [Last changes in collection events] " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "ProjectProxy AS PP INNER JOIN " +
                "CollectionProject AS P ON PP.ProjectID = P.ProjectID ON S.CollectionSpecimenID = P.CollectionSpecimenID LEFT OUTER JOIN " +
                "CollectionEventLocalisation AS L RIGHT OUTER JOIN " +
                "CollectionEvent AS E ON L.CollectionEventID = E.CollectionEventID ON S.CollectionEventID = E.CollectionEventID ";
            if (Login != "dbo")
                SQL += "WHERE PP.ProjectID IN (SELECT ProjectID FROM ProjectList)";
            SQL += "GROUP BY PP.ProjectID, PP.Project " +
                "ORDER BY PP.Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
            try
            {
                _dtProject = new DataTable();
                ad.Fill(_dtProject);
                this.dataGridViewProjectEvents.DataSource = _dtProject;
                this.dataGridViewProjectEvents.ReadOnly = true;
                this.dataGridViewProjectEvents.AutoResizeColumns();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Retrieving statistics for projects failed:\r\n" + ex.Message);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonGetProjectUnits_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Login = "";
            string SqlVersion = "select user_name()";
            Microsoft.Data.SqlClient.SqlConnection ConVersion = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            Microsoft.Data.SqlClient.SqlCommand CVersion = new Microsoft.Data.SqlClient.SqlCommand(SqlVersion, ConVersion);
            try
            {
                ConVersion.Open();
                Login = CVersion.ExecuteScalar().ToString();
                ConVersion.Close();
            }
            catch { }
            //Projects
            string SQL = "SELECT PP.Project, COUNT(U.IdentificationUnitID) AS [Number of organisms] " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "ProjectProxy AS PP INNER JOIN " +
                "CollectionProject AS P ON PP.ProjectID = P.ProjectID ON S.CollectionSpecimenID = P.CollectionSpecimenID LEFT OUTER JOIN " +
                "IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID ";
            if (Login != "dbo")
                SQL += "WHERE PP.ProjectID IN (SELECT ProjectID FROM ProjectList)";
            SQL += "GROUP BY PP.ProjectID, PP.Project " +
                "ORDER BY PP.Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
            try
            {
                _dtProject = new DataTable();
                ad.Fill(_dtProject);
                this.dataGridViewProjectUnits.DataSource = _dtProject;
                this.dataGridViewProjectUnits.ReadOnly = true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Retrieving statistics for projects failed:\r\n" + ex.Message);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonGetProjectAnalysis_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Login = "";
            string SqlVersion = "select user_name()";
            Microsoft.Data.SqlClient.SqlConnection ConVersion = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            Microsoft.Data.SqlClient.SqlCommand CVersion = new Microsoft.Data.SqlClient.SqlCommand(SqlVersion, ConVersion);
            try
            {
                ConVersion.Open();
                Login = CVersion.ExecuteScalar().ToString();
                ConVersion.Close();
            }
            catch { }
            //Projects
            string SQL = "SELECT PP.Project, COUNT(A.AnalysisID) AS Analysis " +
                //", COUNT(DISTINCT A.AnalysisDate)AS [Analysis dates] " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "ProjectProxy AS PP INNER JOIN " +
                "CollectionProject AS P ON PP.ProjectID = P.ProjectID ON S.CollectionSpecimenID = P.CollectionSpecimenID LEFT OUTER JOIN " +
                "IdentificationUnitAnalysis AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID ";
            if (Login != "dbo")
                SQL += "WHERE PP.ProjectID IN (SELECT ProjectID FROM ProjectList)";
            SQL += "GROUP BY PP.ProjectID, PP.Project " +
                "ORDER BY PP.Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
            ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
            try
            {
                _dtProject = new DataTable();
                ad.Fill(_dtProject);
                this.dataGridViewProjectAnalysis.DataSource = _dtProject;
                this.dataGridViewProjectAnalysis.ReadOnly = true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Retrieving statistics for project analysis failed:\r\n" + ex.Message);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        
#endregion

#region Withholding

        private void buttonGetProjectsSpecimenWithhold_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT PP.Project, S.DataWithholdingReason, COUNT(*) AS Number " +
                "FROM CollectionProject AS CP INNER JOIN " +
                "CollectionSpecimen AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                "ProjectProxy AS PP ON CP.ProjectID = PP.ProjectID " +
                "GROUP BY S.DataWithholdingReason, PP.Project " +
                "HAVING (S.DataWithholdingReason <> N'') " +
                "ORDER BY PP.Project";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.dataGridViewProjectsSpecimenWithhold.DataSource = dt;
            ad.SelectCommand.CommandText = "SELECT PP.Project, COUNT(*) AS Number " +
                "FROM CollectionProject AS CP INNER JOIN " +
                "CollectionSpecimen AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                "ProjectProxy AS PP ON CP.ProjectID = PP.ProjectID " +
                "WHERE (S.DataWithholdingReason <> N'') " +
                "GROUP BY PP.Project " +
                "ORDER BY PP.Project";
            System.Data.DataTable dtSum = new DataTable();
            ad.Fill(dtSum);
            this.dataGridViewProjectsSpecimenWithholdSummary.DataSource = dtSum;
        }
        
        private void buttonGetProjectsEventWithhold_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT PP.Project, E.DataWithholdingReason, COUNT(distinct E.CollectionEventID) AS Number " +
                "FROM CollectionProject AS CP INNER JOIN " +
                "CollectionSpecimen AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                "ProjectProxy AS PP ON CP.ProjectID = PP.ProjectID INNER JOIN " +
                "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID " +
                "GROUP BY E.DataWithholdingReason, PP.Project " +
                "HAVING (E.DataWithholdingReason <> N'') " +
                "ORDER BY PP.Project";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.dataGridViewProjectsEventWithhold.DataSource = dt;
            ad.SelectCommand.CommandText = "SELECT PP.Project, COUNT(distinct E.CollectionEventID) AS Number " +
                "FROM CollectionProject AS CP INNER JOIN " +
                "CollectionSpecimen AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                "ProjectProxy AS PP ON CP.ProjectID = PP.ProjectID INNER JOIN " +
                "CollectionEvent AS E ON S.CollectionEventID = E.CollectionEventID " +
                "WHERE (E.DataWithholdingReason <> N'') " +
                "GROUP BY PP.Project " +
                "ORDER BY PP.Project";
            System.Data.DataTable dtSum = new DataTable();
            
            ad.Fill(dtSum);
            this.dataGridViewProjectsEventWithholdSummary.DataSource = dtSum;
        }

        private void buttonGetProjectsOrganismWithhold_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT PP.Project, S.DataWithholdingReason, COUNT(*) AS Number " +
                "FROM CollectionProject AS CP INNER JOIN " +
                "IdentificationUnit AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                "ProjectProxy AS PP ON CP.ProjectID = PP.ProjectID " +
                "GROUP BY S.DataWithholdingReason, PP.Project " +
                "HAVING (S.DataWithholdingReason <> N'') " +
                "ORDER BY PP.Project";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.dataGridViewProjectsOrganismWithhold.DataSource = dt;
            ad.SelectCommand.CommandText = "SELECT PP.Project, COUNT(*) AS Number " +
                "FROM CollectionProject AS CP INNER JOIN " +
                "IdentificationUnit AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                "ProjectProxy AS PP ON CP.ProjectID = PP.ProjectID " +
                "WHERE (S.DataWithholdingReason <> N'') " +
                "GROUP BY PP.Project " +
                "ORDER BY PP.Project";
            System.Data.DataTable dtSum = new DataTable();
            ad.Fill(dtSum);
            this.dataGridViewProjectsOrganismWithholdSummary.DataSource = dtSum;
        }

        private void buttonGetProjectsAgentWithhold_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT PP.Project, S.DataWithholdingReason, COUNT(*) AS Number " +
                "FROM CollectionProject AS CP INNER JOIN " +
                "CollectionAgent AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                "ProjectProxy AS PP ON CP.ProjectID = PP.ProjectID " +
                "GROUP BY S.DataWithholdingReason, PP.Project " +
                "HAVING (S.DataWithholdingReason <> N'') " +
                "ORDER BY PP.Project";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.dataGridViewProjectsAgentWithhold.DataSource = dt;
            ad.SelectCommand.CommandText = "SELECT PP.Project, COUNT(*) AS Number " +
                "FROM CollectionProject AS CP INNER JOIN " +
                "CollectionAgent AS S ON CP.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                "ProjectProxy AS PP ON CP.ProjectID = PP.ProjectID " +
                "WHERE (S.DataWithholdingReason <> N'') " +
                "GROUP BY PP.Project " +
                "ORDER BY PP.Project";
            System.Data.DataTable dtSum = new DataTable();
            ad.Fill(dtSum);
            this.dataGridViewProjectsAgentWithholdSummary.DataSource = dtSum;
        }

        #endregion

        #endregion

        #region History

        private void initHistory()
        {
            this.chartHistory.Series.Clear();
            this.chartHistory.Titles.Clear();
            this.comboBoxHistoryProjectFilter.DataSource = DiversityCollection.LookupTable.DtProjectList;
            this.comboBoxHistoryProjectFilter.DisplayMember = "Project";
            this.comboBoxHistoryProjectFilter.ValueMember = "ProjectID";

        }
        private void buttonHistoryCreate_Click(object sender, EventArgs e)
        {
            if (this.chartHistory.Series.FindByName(this.comboBoxHistoryProjectFilter.Text) != null)
                return;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string SQL = this.HistorySQL();
            if (SQL.Length == 0) System.Windows.Forms.MessageBox.Show("Please insert a filter");
            else this.GenerateHistoryGraph(SQL);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private string HistorySQL()
        {
            string SQL = "";
            if (this.comboBoxHistoryProjectFilter.Text.Length > 0)
            {
                SQL = "declare @Institution nvarchar(50);" +
                    "set @Institution = '" + this.comboBoxHistoryProjectFilter.Text.Replace("'", "''") + "'" +
                    "declare @Verlauf TABLE([Institution][nvarchar](50) COLLATE Latin1_General_CI_AS NULL , " +
                    "[Jahr] [int] NULL, " +
                    "[Anzahl] [int] NULL); " +
                    "INSERT INTO @Verlauf(Institution, Jahr, Anzahl) " +
                    "SELECT @Institution AS Institution, year(S.LogCreatedWhen) AS Jahr, COUNT(*) AS Anzahl " +
                    "FROM CollectionProject AS SP INNER JOIN " +
                    "ProjectProxy AS P ON SP.ProjectID = P.ProjectID INNER JOIN " +
                    "CollectionSpecimen AS S ON SP.CollectionSpecimenID = S.CollectionSpecimenID " +
                    "WHERE        (S.LogCreatedWhen IS NOT NULL) AND P.Project LIKE @Institution + '%' " +
                    "GROUP BY  year(S.LogCreatedWhen); ";
                if (!this.checkBoxHistoryRestrictToInserts.Checked)
                    SQL += "INSERT INTO @Verlauf (Institution, Jahr, Anzahl) " +
                    "SELECT        @Institution AS Institution, YEAR(L.LogDate) AS Jahr, COUNT(*) AS Anzahl " +
                    "FROM            CollectionSpecimen_log AS L INNER JOIN " +
                    "ProjectProxy AS P INNER JOIN " +
                    "CollectionProject AS CP ON P.ProjectID = CP.ProjectID ON L.CollectionSpecimenID = CP.CollectionSpecimenID " +
                    "WHERE P.Project LIKE @Institution + '%' " +
                    "GROUP BY YEAR(L.LogDate); ";
                SQL += "SELECT V.Jahr, SUM(V.Anzahl) AS Anzahl FROM @Verlauf AS V GROUP BY V.Jahr ORDER BY V.Jahr;";
            }
            return SQL;
        }

        private void GenerateHistoryGraph(string SQL)
        {
            System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            this.chartHistory.DataSource = dt;
            this.chartHistory.Series.Add(this.comboBoxHistoryProjectFilter.Text);
            foreach(System.Data.DataRow R in dt.Rows)
            {
                double Jahr;
                double Anzahl;
                if (double.TryParse(R[0].ToString(), out Jahr) && double.TryParse(R[1].ToString(), out Anzahl))
                    this.chartHistory.Series[this.comboBoxHistoryProjectFilter.Text].Points.AddXY(Jahr, Anzahl);
            }
            //this.chartHistory.Titles.Clear();
            //this.chartHistory.Titles.Add(this.textBoxHistoryProjectFilter.Text);
        }

        #endregion

        private void buttonHistoryReset_Click(object sender, EventArgs e)
        {
            this.initHistory();
        }

    }
}

