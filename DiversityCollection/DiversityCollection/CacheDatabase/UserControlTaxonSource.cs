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
    public partial class UserControlTaxonSource : UserControl
    {
        private System.Data.DataRow _Row;
        public DiversityCollection.CacheDatabase.InterfaceCacheDB _Interface;

        public UserControlTaxonSource(System.Data.DataRow R,  InterfaceCacheDB Interface)
        {
            InitializeComponent();
            this._Row = R;
            this._Interface = Interface;
            this.initControl();
        }

        private void initControl()
        {
            this.labelViewCount.Text = "";
            this.textBoxDatabase.Text = this._Row["DatabaseName"].ToString();
            if (this._Row["SourceName"].Equals(System.DBNull.Value))
            {
                this.buttonCreateView.Visible = true;
                //this.textBoxView.Visible = false;
                this.buttonTest.Visible = false;
                this.buttonDelete.Visible = false;
            }
            else
            {
                this.buttonCreateView.Visible = false;
                this.textBoxView.Text = this._Row["SourceName"].ToString();
                //this.textBoxView.Visible = true;
                this.buttonTest.Visible = true;
                this.buttonDelete.Visible = true;
            }
        }

        private void buttonCreateView_Click(object sender, EventArgs e)
        {
            string View = this.CreateTaxonSource(this.textBoxDatabase.Text);
            if (View.Length > 0)
            {
                this._Row["SourceName"] = View;
                string SQL = "UPDATE TaxonSynonymySource SET SourceName = '" + View + "'  WHERE DatabaseName = '" + this._Row["DatabaseName"].ToString() + "'";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.initControl();
            }
        }

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

        private string CreateTaxonSource(string Database)
        {
            string TaxonomicRange = Database.Substring(Database.IndexOf('_') + 1);
            string View = "TaxonSynonymy_" + TaxonomicRange;
            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT        TOP 100 PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .NameID AS varchar) AS NameURI, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .NameID AS varchar) AS SynNameURI, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .SpeciesGenusNameID AS varchar) AS SpeciesGenusNameURI,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName T INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName A ON T .NameID = A.NameID " +
                "WHERE        (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP 100 PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .NameID AS varchar), T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .NameID AS varchar) AS SynNameID, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .SpeciesGenusNameID AS varchar),  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, NULL " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName T " +
                "WHERE        NameID NOT IN " +
                "(SELECT        NameID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy) AND NameID NOT IN " +
                "(SELECT        NameID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName) AND NameID NOT IN " +
                "(SELECT        SynNameID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON T .NameID = S1.SynNameID AND A.ProjectID = S1.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S ON S1.NameID = S.SynNameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S2.SynNameID = T .NameID AND S2.ProjectID = A.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S3 INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S3.SynNameID = T .NameID AND S3.ProjectID = A.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID ON  " +
                "S3.NameID = S2.SynNameID AND S3.ProjectID = S2.ProjectID " +
                "WHERE        (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT        TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM            DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S1.NameID = T1.NameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID ON S1.SynNameID = S.NameID AND  " +
                "S1.ProjectID = S.ProjectID " +
                "WHERE        (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'duplicate') OR " +
                "(T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'isonym') " +
                "ORDER BY AcceptedName, SynonymName";//; GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CollectionCacheUser";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                SQL = "GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CollectionCacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    return View;
                else
                    return "";
            }
            else return "";
        }

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
        

    }
}
