using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace DiversityCollection.CacheDatabase
{
    public partial class UserControlTaxonomy : UserControl, InterfaceTaxonomy
    {

        #region Paramter and properties

        public enum TypeOfSource { Taxa, ScientificTerms };

        private TypeOfSource _TypeOfSource = TypeOfSource.Taxa;

        private System.Data.DataRow _Row;
        public DiversityCollection.CacheDatabase.InterfaceCacheDatabase _Interface;
        private string _SourceName = "";

        public string SourceName
        {
            get { return _SourceName; }
            set
            {
                _SourceName = value;
                this.textBoxView.Text = value;
            }
        }
        
        private string _BaseURL;
        private string BaseURL()
        {
            if (this._BaseURL == null)
            {
                string Database = this._Row["DatabaseName"].ToString();
                if (Database.IndexOf("[") > -1)
                    this._BaseURL = DiversityWorkbench.FormFunctions.SqlExecuteScalar("SELECT TOP 1 [BaseURL]  FROM " + Database + ".[dbo].[ViewBaseURL]");
                else
                    this._BaseURL = DiversityWorkbench.FormFunctions.SqlExecuteScalar("SELECT " + Database + ".dbo.BaseURL()");
            }
            return this._BaseURL;
        }

        #endregion

        #region Construction

        public UserControlTaxonomy(System.Data.DataRow R, InterfaceCacheDatabase Interface)
        {
            InitializeComponent();
            this._Row = R;
            this._Interface = Interface;
            this.initControl();
        }

        public UserControlTaxonomy(System.Data.DataRow R, InterfaceCacheDatabase Interface, TypeOfSource SourceType)
        {
            InitializeComponent();
            this._Row = R;
            this._Interface = Interface;
            this._TypeOfSource = SourceType;
            this.initControl();
        }

        #endregion

        #region Control

        private void initControl()
        {
            this.labelCountCacheDB.Text = "";
            this.labelCountPostgres.Text = "";
            this.textBoxDatabase.Text = this._Row["DatabaseName"].ToString().Replace("].", "]\r\n");

            if (this._Row["SourceName"].Equals(System.DBNull.Value) || this._Row["SourceName"].ToString().Length == 0)
            {
                this._SourceName = this._Row["SourceName"].ToString();
                this.buttonCreateView.Visible = true;
                this.buttonTestView.Visible = false;
                this.buttonDeleteSource.Visible = false;
            }
            else
            {
                this.buttonCreateView.Visible = false;
                this.textBoxView.Text = this._Row["SourceName"].ToString();
                this.buttonTestView.Visible = true;
                this.buttonDeleteSource.Visible = true;
            }
            this.initPostgresControls();
            this.setCacheDatabaseControls();
        }
        
        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region View

        private void buttonCreateView_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._TypeOfSource == TypeOfSource.Taxa)
                {
                    string View = this.CreateTaxonSource(this.textBoxDatabase.Text.Replace("\r\n", "."));
                    if (View.Length > 0)
                    {
                        this._Row["SourceName"] = View;
                        string SQL = "UPDATE TaxonSynonymySource SET SourceName = '" + View + "'  WHERE DatabaseName = '" + this._Row["DatabaseName"].ToString() + "'";
                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                            this.initControl();
                    }
                }
                else if (this._TypeOfSource == TypeOfSource.ScientificTerms)
                {
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// Testing the view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTest_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT * FROM " + this.textBoxView.Text;
            System.Data.DataTable dt = new DataTable();
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            ad.Fill(dt);
            DiversityWorkbench.FormEditTable f = new DiversityWorkbench.FormEditTable(ad, this.textBoxView.Text, "Data from " + this.textBoxView.Text + " in database " + this.textBoxDatabase.Text, false);
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        /// <summary>
        /// Creating a view for the taxonomic names from a DiversityTaxonNames source either local or from a linked server
        /// </summary>
        /// <param name="Database">Name of the database, may include linked server as first part</param>
        /// <param name="ProjectID">ID of the project</param>
        /// <returns></returns>
        private string CreateTaxonSource(string Database, int ProjectID)
        {
            string TaxonomicRange = Database.Substring(Database.IndexOf('_') + 1);
            string View = "TaxonSynonymy_" + TaxonomicRange;
            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
            "AS " +
            "SELECT TOP 100 PERCENT T.NameID, '" + this.BaseURL() + "' AS BaseURL, T.TaxonNameCache AS TaxonName, " +
            "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, " +
            "T.TaxonomicRank, " +
            "T.GenusOrSupragenericName, T.SpeciesGenusNameID, " +
            "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND " +
            "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL " +
            "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
            "FROM " + Database + ".dbo.TaxonName T INNER JOIN " +
            "" + Database + ".dbo.TaxonAcceptedName A ON T.NameID = A.NameID " +
            "WHERE (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) " +
            "AND A.ProjectID = " + ProjectID.ToString() + " " +
            "UNION " +
            "SELECT TOP (100) PERCENT T1.NameID, '" + this.BaseURL() + "' AS BaseURL,  T1.TaxonNameCache AS TaxonName, " +
            "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, " +
            "T.TaxonomicRank, " +
            "T.GenusOrSupragenericName, T.SpeciesGenusNameID , " +
            "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND " +
            "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL " +
            "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
            "FROM " + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T ON S.SynNameID = T.NameID INNER JOIN " +
            "" + Database + ".dbo.TaxonAcceptedName AS A ON T.NameID = A.NameID AND S.ProjectID = A.ProjectID INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
            "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) " +
            "UNION " +
            "SELECT TOP 100 PERCENT T.NameID, '" + this.BaseURL() + "' AS BaseURL,  T.TaxonNameCache AS TaxonName, " +
            "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, " +
            "T.TaxonomicRank, " +
            "T.GenusOrSupragenericName, T.SpeciesGenusNameID, " +
            "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND " +
            "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL " +
            "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, NULL " +
            "FROM " + Database + ".dbo.TaxonName T " +
            "WHERE T.IgnoreButKeepForReference = 0 AND T.NameID NOT IN " +
            "(SELECT NameID " +
            "FROM " + Database + ".dbo.TaxonSynonymy) AND NameID NOT IN " +
            "(SELECT NameID " +
            "FROM " + Database + ".dbo.TaxonAcceptedName) AND NameID NOT IN " +
            "(SELECT SynNameID " +
            "FROM " + Database + ".dbo.TaxonSynonymy) " +
            "UNION " +
            "SELECT TOP (100) PERCENT T1.NameID,'" + this.BaseURL() + "' AS BaseURL,  T1.TaxonNameCache AS TaxonName, " +
            "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, " +
            "T.TaxonomicRank, " +
            "T.GenusOrSupragenericName, T.SpeciesGenusNameID , " +
            "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND " +
            "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL " +
            "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
            "FROM " + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T ON A.NameID = T.NameID INNER JOIN " +
            "" + Database + ".dbo.TaxonSynonymy AS S1 ON T.NameID = S1.SynNameID AND A.ProjectID = S1.ProjectID INNER JOIN " +
            "" + Database + ".dbo.TaxonSynonymy AS S ON S1.NameID = S.SynNameID INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
            "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) " +
            "AND A.ProjectID = " + ProjectID.ToString() + " " +
            "AND S.ProjectID = " + ProjectID.ToString() + " " +
            "AND S1.ProjectID = " + ProjectID.ToString() + " " +
            "UNION " +
            "SELECT TOP (100) PERCENT T1.NameID, '" + this.BaseURL() + "' AS BaseURL, T1.TaxonNameCache AS TaxonName, " +
            "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, " +
            "T.TaxonomicRank, " +
            "T.GenusOrSupragenericName, T.SpeciesGenusNameID , " +
            "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND " +
            "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL " +
            "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
            "FROM " + Database + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
            "" + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T ON A.NameID = T.NameID ON S2.SynNameID = T.NameID AND S2.ProjectID = A.ProjectID INNER JOIN " +
            "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
            "" + Database + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID " +
            "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND " +
            "(S2.IgnoreButKeepForReference = 0) " +
            "AND A.ProjectID = " + ProjectID.ToString() + " " +
            "AND S.ProjectID = " + ProjectID.ToString() + " " +
            "AND S1.ProjectID = " + ProjectID.ToString() + " " +
            "AND S2.ProjectID = " + ProjectID.ToString() + " " +
            "UNION " +
            "SELECT TOP (100) PERCENT T1.NameID,'" + this.BaseURL() + "' AS BaseURL, T1.TaxonNameCache AS TaxonName, " +
            "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, " +
            "T.TaxonomicRank, " +
            "T.GenusOrSupragenericName, T.SpeciesGenusNameID , " +
            "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND " +
            "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL " +
            "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
            "FROM " + Database + ".dbo.TaxonSynonymy AS S3 INNER JOIN " +
            "" + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T ON A.NameID = T.NameID ON S3.SynNameID = T.NameID AND S3.ProjectID = A.ProjectID INNER JOIN " +
            "" + Database + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
            "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
            "" + Database + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID ON " +
            "S3.NameID = S2.SynNameID AND S3.ProjectID = S2.ProjectID " +
            "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND " +
            "(S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) " +
            "AND A.ProjectID = " + ProjectID.ToString() + " " +
            "AND S1.ProjectID = " + ProjectID.ToString() + " " +
            "AND S2.ProjectID = " + ProjectID.ToString() + " " +
            "AND S3.ProjectID = " + ProjectID.ToString() + " " +
            "UNION " +
            "SELECT TOP (100) PERCENT T1.NameID, '" + this.BaseURL() + "' AS BaseURL, T1.TaxonNameCache AS TaxonName, " +
            "T.NameID AS AcceptedNameID, T.TaxonNameCache AS AcceptedName, " +
            "T.TaxonomicRank, " +
            "T.GenusOrSupragenericName, T.SpeciesGenusNameID , " +
            "T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
            "T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
            "T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
            "T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND " +
            "T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
            "T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix IS NULL " +
            "THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
            "FROM " + Database + ".dbo.TaxonSynonymy AS S1 INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T1 ON S1.NameID = T1.NameID INNER JOIN " +
            "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
            "" + Database + ".dbo.TaxonName AS T ON S.SynNameID = T.NameID INNER JOIN " +
            "" + Database + ".dbo.TaxonAcceptedName AS A ON T.NameID = A.NameID AND S.ProjectID = A.ProjectID ON S1.SynNameID = S.NameID AND " +
            "S1.ProjectID = S.ProjectID " +
            "WHERE (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'duplicate') OR " +
            "(T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'isonym') " +
            "AND A.ProjectID = " + ProjectID.ToString() + " " +
            "AND S.ProjectID = " + ProjectID.ToString() + " " +
            "AND S1.ProjectID = " + ProjectID.ToString() + " " +
            "ORDER BY AcceptedName, SynonymName";//; GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CollectionCacheUser";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                SQL = "GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    return View;
                else
                    return "";
            }
            else return "";
        }


        /// <summary>
        /// Creating a view for the taxonomic names from a DiversityTaxonNames source either local or from a linked server
        /// </summary>
        /// <param name="Database">Name of the database, may include linked server as first part</param>
        /// <returns></returns>
        private string CreateTaxonSource(string Database)
        {
            string TaxonomicRange = Database.Substring(Database.IndexOf('_') + 1);
            string View = "TaxonSynonymy_" + TaxonomicRange;
            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT        TOP 100 PERCENT '" + this.BaseURL() + "' + cast(T .NameID AS varchar) AS NameURI, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + cast(T .NameID AS varchar) AS SynNameURI, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + cast(T .SpeciesGenusNameID AS varchar) AS SpeciesGenusNameURI,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonName T INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName A ON T .NameID = A.NameID " +
                "WHERE        (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP 100 PERCENT '" + this.BaseURL() + "' + cast(T .NameID AS varchar), T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + cast(T .NameID AS varchar) AS SynNameID, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + cast(T .SpeciesGenusNameID AS varchar),  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, NULL " +
                "FROM            " + Database + ".dbo.TaxonName T " +
                "WHERE  T .IgnoreButKeepForReference = 0 AND  T.NameID NOT IN " +
                "(SELECT        NameID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy) AND NameID NOT IN " +
                "(SELECT        NameID " +
                "FROM            " + Database + ".dbo.TaxonAcceptedName) AND NameID NOT IN " +
                "(SELECT        SynNameID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON A.NameID = T .NameID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S1 ON T .NameID = S1.SynNameID AND A.ProjectID = S1.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S ON S1.NameID = S.SynNameID INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S2.SynNameID = T .NameID AND S2.ProjectID = A.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy AS S3 INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S3.SynNameID = T .NameID AND S3.ProjectID = A.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID ON  " +
                "S3.NameID = S2.SynNameID AND S3.ProjectID = S2.ProjectID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy AS S1 INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S1.NameID = T1.NameID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID ON S1.SynNameID = S.NameID AND  " +
                "S1.ProjectID = S.ProjectID " +
                "WHERE        (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'duplicate') OR " +
                "(T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'isonym') " +
                "ORDER BY AcceptedName, SynonymName";//; GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CollectionCacheUser";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                SQL = "GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    return View;
                else
                    return "";
            }
            else return "";
        }

        /// <summary>
        /// Creating a view for the scientific terms from a DiversityScientificTerms source either local or from a linked server
        /// </summary>
        /// <param name="Database">Name of the database, may include linked server as first part</param>
        /// <returns></returns>
        private string CreateScientificTermsSource(string Database, string Terminology, int TerminologyID)
        {
            string TaxonomicRange = Database.Substring(Database.IndexOf('_') + 1);
            string View = "ScientificTerms_";
            for (int i = 0; i < Terminology.Length; i++)
            {
                //if (ASCII(Terminology[i] <= 'a'
            }
            // +Terminology.Replace(" ", "").Replace("(", "").Replace(")", "") + "_" + TerminologyID.ToString();
            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT        TOP 100 PERCENT '" + this.BaseURL() + "' + cast(T .NameID AS varchar) AS NameURI, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + cast(T .NameID AS varchar) AS SynNameURI, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + cast(T .SpeciesGenusNameID AS varchar) AS SpeciesGenusNameURI,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonName T INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName A ON T .NameID = A.NameID " +
                "WHERE        (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP 100 PERCENT '" + this.BaseURL() + "' + cast(T .NameID AS varchar), T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + cast(T .NameID AS varchar) AS SynNameID, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + cast(T .SpeciesGenusNameID AS varchar),  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, NULL " +
                "FROM            " + Database + ".dbo.TaxonName T " +
                "WHERE  T .IgnoreButKeepForReference = 0 AND  T.NameID NOT IN " +
                "(SELECT        NameID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy) AND NameID NOT IN " +
                "(SELECT        NameID " +
                "FROM            " + Database + ".dbo.TaxonAcceptedName) AND NameID NOT IN " +
                "(SELECT        SynNameID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON A.NameID = T .NameID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S1 ON T .NameID = S1.SynNameID AND A.ProjectID = S1.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S ON S1.NameID = S.SynNameID INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S2.SynNameID = T .NameID AND S2.ProjectID = A.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy AS S3 INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S3.SynNameID = T .NameID AND S3.ProjectID = A.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID ON  " +
                "S3.NameID = S2.SynNameID AND S3.ProjectID = S2.ProjectID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT '" + this.BaseURL() + "' + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + this.BaseURL() + "' + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + this.BaseURL() + "' + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            " + Database + ".dbo.TaxonSynonymy AS S1 INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T1 ON S1.NameID = T1.NameID INNER JOIN " +
                "" + Database + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "" + Database + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "" + Database + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID ON S1.SynNameID = S.NameID AND  " +
                "S1.ProjectID = S.ProjectID " +
                "WHERE        (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'duplicate') OR " +
                "(T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'isonym') " +
                "ORDER BY AcceptedName, SynonymName";//; GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CollectionCacheUser";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                SQL = "GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    return View;
                else
                    return "";
            }
            else return "";
        }

        /// <summary>
        /// Deleting the created view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string View = this.textBoxView.Text;
            if (View.Length > 0)
            {
                string SQL = "DELETE TaxonSynonymySource WHERE DatabaseName = '" + this._Row["DatabaseName"].ToString() + "'";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    SQL = "DROP VIEW [dbo].[" + this.textBoxView.Text + "]";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        this._Interface.initTaxonSources();
                }
            }
        }
        
        #endregion

        #region Cache database

        private void buttonTransferToCache_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Message = "";
            string SQL = "DELETE T FROM TaxonSynonymy T WHERE NameURI LIKE '" + this.BaseURL() + "%'";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "INSERT INTO TaxonSynonymy (NameURI, AcceptedName, SynNameURI, SynonymName, TaxonomicRank, GenusOrSupragenericName, SpeciesGenusNameURI, TaxonNameSinAuthor, ProjectID) " +
                    "SELECT NameURI, AcceptedName, SynNameURI, SynonymName, TaxonomicRank, GenusOrSupragenericName, SpeciesGenusNameURI, TaxonNameSinAuthor, ProjectID " +
                    "FROM " + this.textBoxView.Text + "";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                {
                    this.setCacheDatabaseControls();
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonViewCacheDB_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string SQL = "SELECT * FROM TaxonSynonymy WHERE NameURI LIKE '" + this.BaseURL() + "%'";
            System.Data.DataTable dt = new DataTable();
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            ad.Fill(dt);
            DiversityWorkbench.FormEditTable f = new DiversityWorkbench.FormEditTable(ad, this.textBoxView.Text, "Data from " + this.textBoxView.Text + " in database " + this.textBoxDatabase.Text, false);
            f.StartPosition = FormStartPosition.CenterParent;
            this.Cursor = System.Windows.Forms.Cursors.Default;
            f.ShowDialog();
        }

        private void setCacheDatabaseControls()
        {
            string SQL = "SELECT COUNT(*) FROM TaxonSynonymy WHERE NameURI LIKE '" + this.BaseURL() + "%'";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result != "0")
            {
                this.labelCountCacheDB.Text = "Nr. of taxa:" + Result + "\r\nLast update:\r\n";
                SQL = "SELECT CONVERT(nvarchar(50), MAX(T.LogInsertedWhen), 120) AS LastUpdate FROM TaxonSynonymy T WHERE T.NameURI LIKE '" + this.BaseURL() + "%'";
                this.labelCountCacheDB.Text += DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            }
            else this.labelCountCacheDB.Text = "No data";
        }
        
        #endregion

        #region Postgres

        public void initPostgresControls()
        {
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                bool IsCacheDatabase = true;
                try
                {
                    string sql = "SELECT diversityworkbenchmodule();";
                    string result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(sql);
                    if (result != "DiversityCollectionCache")
                        IsCacheDatabase = false;
                }
                catch (System.Exception ex)
                {
                    IsCacheDatabase = false;
                }
                if (IsCacheDatabase)
                {
                    this.buttonTransferToPostgres.Enabled = true;
                    this.buttonViewPostgres.Enabled = true;
                    string SQL = "SELECT COUNT(*) FROM \"TaxonSynonymy\" WHERE \"NameURI\" LIKE '" + this.BaseURL() + "%'";
                    string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                    if (Result != "0" && Result != "")
                    {
                        this.labelCountPostgres.Text = "Nr. of taxa: " + Result + "\r\nLast update:\r\n";
                        SQL = "SELECT CONVERT(nvarchar(50), MAX(T.\"LogInsertedWhen\"), 120) AS LastUpdate FROM \"TaxonSynonymy\" T WHERE T.\"NameURI\" LIKE '" + this.BaseURL() + "%'";
                        this.labelCountPostgres.Text += DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    }
                    else
                    {
                        this.labelCountPostgres.Text = "No data";
                    }
                }
                else
                {
                    this.buttonTransferToPostgres.Enabled = false;
                    this.buttonViewPostgres.Enabled = false;
                    this.labelCountPostgres.Text = "Not connected";
                }
            }
            else
            {
                this.buttonTransferToPostgres.Enabled = false;
                this.buttonViewPostgres.Enabled = false;
                this.labelCountPostgres.Text = "Not connected";
            }
        }

        private void buttonTransferToPostgres_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Message = "";
            string SqlDel = "DELETE FROM \"TaxonSynonymy\" WHERE \"NameURI\" LIKE '" + this.BaseURL() + "%'";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SqlDel, ref Message))
            {
                string SqlPG = "SELECT \"NameURI\", \"AcceptedName\", \"SynNameURI\", \"SynonymName\", \"TaxonomicRank\", " +
                    "\"GenusOrSupragenericName\", \"SpeciesGenusNameURI\", \"TaxonNameSinAuthor\", " +
                    "\"LogInsertedWhen\", \"ProjectID\", \"Source\" " +
                    "FROM \"public\".\"TaxonSynonymy\"";
                string SQL = "SELECT NameURI, AcceptedName, SynNameURI, SynonymName, TaxonomicRank, GenusOrSupragenericName, SpeciesGenusNameURI, TaxonNameSinAuthor, " +
                    "LogInsertedWhen, ProjectID, Source " +
                    "FROM dbo.TaxonSynonymy " +
                    "WHERE (NameURI LIKE '" + this.BaseURL() + "%')";
                System.Data.DataTable dtSource = new DataTable();
                System.Data.SqlClient.SqlDataAdapter adTrans = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                adTrans.Fill(dtSource);

                // creating the target table
                Npgsql.NpgsqlDataAdapter adPG = new Npgsql.NpgsqlDataAdapter(SqlPG, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());//.Postgres.PostgresConnection().ConnectionString);
                System.Data.DataTable dtTarget = new DataTable();
                adPG.Fill(dtTarget);

                System.Collections.Generic.List<string> TransferColumns = new List<string>();
                TransferColumns.Add("NameURI");
                TransferColumns.Add("AcceptedName");
                TransferColumns.Add("SynNameURI");
                TransferColumns.Add("SynonymName");
                TransferColumns.Add("TaxonomicRank");
                TransferColumns.Add("GenusOrSupragenericName");
                TransferColumns.Add("SpeciesGenusNameURI");
                TransferColumns.Add("TaxonNameSinAuthor");
                TransferColumns.Add("LogInsertedWhen");
                TransferColumns.Add("ProjectID");
                TransferColumns.Add("Source");
                // transfer the data
                try
                {
                    //System.Collections.Generic.List<string> NameURIs = new List<string>();
                    foreach (System.Data.DataRow R in dtSource.Rows)
                    {
                        //if (NameURIs.Contains(R[0].ToString()))
                        //    continue;
                        //NameURIs.Add(R[0].ToString());
                        try
                        {
                            System.Data.DataRow Rnew = dtTarget.NewRow();
                            foreach (string s in TransferColumns)
                                Rnew[s] = R[s];
                            dtTarget.Rows.Add(Rnew);
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
                try
                {
                    Npgsql.NpgsqlCommandBuilder cb = new Npgsql.NpgsqlCommandBuilder(adPG);
                    adPG.Update(dtTarget);
                    this.initPostgresControls();
                }
                catch (System.Exception ex)
                {
                }
            }
            else
            {
                if (Message.Length > 0)
                    System.Windows.Forms.MessageBox.Show(Message);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonViewPostgres_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT * FROM \"TaxonSynonymy\" WHERE \"NameURI\" LIKE '" + this.BaseURL() + "%'";
            string Message = "";
            System.Data.DataTable dt = new DataTable();
            if (DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message))
            {
                DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent("Taxonomy", "Content of table TaxonSynonymy", dt);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }
            else if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
        }

        #endregion
    }
}
