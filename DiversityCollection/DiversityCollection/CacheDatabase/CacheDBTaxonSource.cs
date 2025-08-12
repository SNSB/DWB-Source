using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DiversityCollection.CacheDatabase
{
    public class CacheDBTaxonSource
    {
        #region Parameter

        private string _DiversityTaxonNamesConnectionString;
        private string _DiversityTaxonNamesDataSource;
        private string _DiversityTaxonNamesDataBase;
        private string _BaseURL;
        private System.Data.DataTable _DataTable;
        private System.Collections.Generic.Dictionary<int, string> _ProjectIDs;
        private System.Data.DataTable _DtTaxonSynonymy;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTaxonSynonoymy;

        public string Datasource
        {
            get
            {
                return this._DiversityTaxonNamesDataSource + " " + this._DiversityTaxonNamesDataBase;
            }
        }

        public System.Data.DataTable DataTable
        {
            get
            {
                if (this._DataTable == null)
                {
                    this._DataTable = new DataTable();
                    this.RefershDataTable();
                }
                return _DataTable;
            }
        }

        public void RefershDataTable()
        {
            this.DataTable.Clear();
            string SQL = "SELECT NameURI, AcceptedName, SynNameURI, SynonymName, TaxonomicRank, GenusOrSupragenericName, SpeciesGenusNameURI, TaxonNameSinAuthor, " +
                "LogInsertedWhen, ProjectID " +
                "FROM TaxonSynonymy " +
                "WHERE (NameURI LIKE '" + BaseURL + "%') " +
                "ORDER BY AcceptedName";
            this._SqlDataAdapterTaxonSynonoymy = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            this._SqlDataAdapterTaxonSynonoymy.Fill(this.DataTable);
            Microsoft.Data.SqlClient.SqlCommandBuilder CB = new Microsoft.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterTaxonSynonoymy);
        }

        #endregion

        #region Construction

        public CacheDBTaxonSource(string DataSource, string DatabaseName, System.Collections.Generic.Dictionary<int, string> ProjectIDs, System.Data.DataTable DtTaxonSynonymy)
        {
            this._DiversityTaxonNamesDataSource = DataSource;
            this._DiversityTaxonNamesDataBase = DatabaseName;
            this._ProjectIDs = ProjectIDs;
            this._DtTaxonSynonymy = DtTaxonSynonymy;
        }

        #endregion

        public void ClearCurrentData()
        {
            // Removing the current data
            this.DataTable.Clear();
            string SQL = "DELETE FROM TaxonSynonymy WHERE NameURI LIKE '" + this.BaseURL + "%'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            C.ExecuteNonQuery();
            con.Close();
        }

        public System.Data.DataTable RequerySource()
        {
            // Removing the current data
            this.DataTable.Clear();
            string SQL = "DELETE FROM TaxonSynonymy WHERE NameURI LIKE '" + this.BaseURL + "%'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            C.ExecuteNonQuery();
            con.Close();

            // getting the new data from the source
            System.Collections.Generic.List<int> HandledProjectIDs = new List<int>();
            //SQL = "";
            //int ProjectID = 0;
            Microsoft.Data.SqlClient.SqlConnection conSource = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringSource);
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in this._ProjectIDs)
            {
                try
                {
                    SQL = DiversityCollection.CacheDatabase.CacheDBTaxonSource.SqlTaxonSynonymy(this._DiversityTaxonNamesDataBase, KV.Key, HandledProjectIDs);
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringSource);
                    //ad.Fill(this._DtTaxonSynonymy);
                    ad.Fill(this.DataTable);
                }
                catch (System.Exception ex) { }
            }

            System.Data.DataTable DtExport = this.DataTable.Copy();
            DtExport.Columns.Add("Error", typeof(string));

            if (this._SqlDataAdapterTaxonSynonoymy.UpdateCommand == null)
            {
                //Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterTaxonSynonoymy);
                try
                {
                    con.Open();
                    System.Collections.Generic.List<System.Data.DataRow> RowsToDelete = new List<DataRow>();
                    foreach (System.Data.DataRow R in DtExport.Rows)
                    {
                        SQL = "INSERT INTO TaxonSynonymy " +
                            "(NameURI, AcceptedName, SynNameURI, SynonymName, TaxonomicRank, GenusOrSupragenericName, SpeciesGenusNameURI, TaxonNameSinAuthor, ProjectID) " +
                            "VALUES     ('" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(R["NameURI"].ToString()) +
                            "', '" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(R["AcceptedName"].ToString()) +
                            "', '" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(R["SynNameURI"].ToString()) +
                            "', '" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(R["SynonymName"].ToString()) +
                            "', '" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(R["TaxonomicRank"].ToString()) +
                            "', '" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(R["GenusOrSupragenericName"].ToString()) +
                            "', '" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(R["SpeciesGenusNameURI"].ToString()) +
                            "', '" + DiversityWorkbench.Forms.FormFunctions.SqlRemoveHyphens(R["TaxonNameSinAuthor"].ToString()) +
                            "', " + R["ProjectID"].ToString() + ")";
                        C.CommandText = SQL;
                        try
                        {
                            C.ExecuteNonQuery();
                            RowsToDelete.Add(R);
                        }
                        catch (System.Exception ex)
                        {
                            R["Error"] = ex.Message;
                        }
                    }
                    con.Close();
                    foreach (System.Data.DataRow R in RowsToDelete)
                    {
                        R.BeginEdit();
                        R.Delete();
                        R.EndEdit();
                        R.AcceptChanges();
                    }
                }
                catch (System.Exception ex) { }
            }
            else
            {
                DtExport.Clear();
                this._SqlDataAdapterTaxonSynonoymy.Update(this.DataTable);
            }
            return DtExport;
            //this.RefershDataTable();
        }

        public string BaseURL
        {
            get
            {
                if (this._BaseURL == null || this._BaseURL.Length == 0)
                {
                    string SQL = "SELECT dbo.BaseURL()";
                    try
                    {
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringSource);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        this._BaseURL = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return this._BaseURL;
            }
        }

        private string ConnectionStringSource
        {
            get
            {
                if (this._DiversityTaxonNamesConnectionString == null || this._DiversityTaxonNamesConnectionString.Length == 0)
                {

                    DiversityWorkbench.ServerConnection S = new DiversityWorkbench.ServerConnection();
                    S.DatabaseServer = this._DiversityTaxonNamesDataSource.Substring(0, this._DiversityTaxonNamesDataSource.IndexOf(','));
                    S.DatabaseName = this._DiversityTaxonNamesDataBase;
                    S.DatabaseServerPort = int.Parse(this._DiversityTaxonNamesDataSource.Substring(this._DiversityTaxonNamesDataSource.IndexOf(',') + 1));
                    S.ModuleName = "DiversityTaxonNames";
                    S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                    if (!S.IsTrustedConnection)
                    {
                        S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                        S.DatabasePassword = DiversityWorkbench.Settings.Password;
                    }
                    bool OK = S.ConnectionIsValid;
                    if (!S.ConnectionIsValid)
                    {
                        DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(S);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                        }
                    }
                    this._DiversityTaxonNamesConnectionString = S.ConnectionString;
                }
                return this._DiversityTaxonNamesConnectionString;
            }
        }

        public static string SqlTaxonSynonymy(string Database, int ProjectID, System.Collections.Generic.List<int> HandeledProjectIDs)
        {
            string ProjectList = "";
            foreach (int i in HandeledProjectIDs)
            {
                if (ProjectList.Length > 0) ProjectList += ", ";
                ProjectList += i.ToString();
            }
            string SQL = "SELECT TOP 100 PERCENT " + Database + ".dbo.BaseURL() + cast(T.NameID as varchar) AS NameURI, T.TaxonNameCache AS AcceptedName, " + Database + ".dbo.BaseURL() + cast(T.NameID as varchar) AS SynNameURI,  " +
                " T.TaxonNameCache AS SynonymName,  " +
                " T.TaxonomicRank, T.GenusOrSupragenericName,  " +
                Database + ".dbo.BaseURL() + cast(T.SpeciesGenusNameID as varchar) AS SpeciesGenusNameURI,  " +
                " T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                " T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                " T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL " +
                " OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                " T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix " +
                " IS NULL THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, " + ProjectID.ToString() + " AS ProjectID " +
                " FROM         " + Database + ".dbo.TaxonName T INNER JOIN " +
                Database + ".dbo.TaxonAcceptedName A ON T.NameID = A.NameID " +
                " WHERE     (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString();
            if (ProjectList.Length > 0)
                SQL += " AND T.NameID NOT IN (SELECT     TP.NameID " +
                " FROM         TaxonName AS TP INNER JOIN " +
                " TaxonNameProject AS P ON TP.NameID = P.NameID " +
                " WHERE     (TP.IgnoreButKeepForReference = 0) AND (P.ProjectID IN (" + ProjectList + "))) ";
            SQL += " UNION " +
                " SELECT     TOP 100 PERCENT " + Database + ".dbo.BaseURL() + cast(T.NameID as varchar), T.TaxonNameCache AS AcceptedName,  " +
                Database + ".dbo.BaseURL() + cast(T1.NameID as varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName,  " +
                " T.TaxonomicRank, T.GenusOrSupragenericName,  " +
                Database + ".dbo.BaseURL() + cast(T.SpeciesGenusNameID as varchar),  " +
                " T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                " T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                " T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL " +
                " OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                " T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix " +
                " IS NULL THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, " + ProjectID.ToString() + " AS ProjectID " +
                " FROM         " + Database + ".dbo.TaxonSynonymy S INNER JOIN " +
                Database + ".dbo.TaxonName T ON S.SynNameID = T.NameID INNER JOIN " +
                Database + ".dbo.TaxonAcceptedName A ON T.NameID = A.NameID INNER JOIN " +
                Database + ".dbo.TaxonName T1 ON S.NameID = T1.NameID " +
                " WHERE     (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString();
            if (ProjectList.Length > 0)
                SQL += " AND T.NameID NOT IN (SELECT     TP.NameID " +
                " FROM         TaxonName AS TP INNER JOIN " +
                " TaxonNameProject AS P ON TP.NameID = P.NameID " +
                " WHERE     (TP.IgnoreButKeepForReference = 0) AND (P.ProjectID IN (" + ProjectList + "))) ";
            SQL += " UNION " +
                " SELECT     TOP 100 PERCENT " + Database + ".dbo.BaseURL() + cast(T.NameID as varchar), T.TaxonNameCache AS AcceptedName,  " +
                Database + ".dbo.BaseURL() + cast(T.NameID as varchar) AS SynNameID, T.TaxonNameCache AS SynonymName,  " +
                " T.TaxonomicRank, T.GenusOrSupragenericName,  " +
                Database + ".dbo.BaseURL() + cast(T.SpeciesGenusNameID as varchar),  " +
                " T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                " T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                " T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL " +
                " OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                " T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix " +
                " IS NULL THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, " + ProjectID.ToString() + " AS ProjectID " +
                " FROM         " + Database + ".dbo.TaxonName T " +
                " WHERE     NameID NOT IN " +
                " (SELECT     NameID " +
                " FROM          " + Database + ".dbo.TaxonSynonymy) AND NameID NOT IN " +
                " (SELECT     NameID " +
                " FROM          " + Database + ".dbo.TaxonAcceptedName) AND NameID NOT IN " +
                " (SELECT     SynNameID " +
                " FROM          " + Database + ".dbo.TaxonSynonymy) ";
            if (ProjectList.Length > 0)
                SQL += " AND T.NameID NOT IN (SELECT     TP.NameID " +
                " FROM         TaxonName AS TP INNER JOIN " +
                " TaxonNameProject AS P ON TP.NameID = P.NameID " +
                " WHERE     (TP.IgnoreButKeepForReference = 0) AND (P.ProjectID IN (" + ProjectList + "))) ";
            SQL += " UNION " +
                " SELECT     TOP 100 PERCENT " + Database + ".dbo.BaseURL() + cast(T.NameID as varchar), T.TaxonNameCache AS AcceptedName,  " +
                Database + ".dbo.BaseURL() + cast(T1.NameID as varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName,  " +
                " T.TaxonomicRank, T.GenusOrSupragenericName, " +
                Database + ".dbo.BaseURL() + cast(T.SpeciesGenusNameID as varchar) ,  " +
                " T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                " T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                " T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL " +
                " OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                " T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix " +
                " IS NULL THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, " + ProjectID.ToString() + " AS ProjectID " +
                " FROM         " + Database + ".dbo.TaxonAcceptedName A INNER JOIN " +
                Database + ".dbo.TaxonName T ON A.NameID = T.NameID INNER JOIN " +
                Database + ".dbo.TaxonSynonymy S1 ON T.NameID = S1.SynNameID INNER JOIN " +
                Database + ".dbo.TaxonSynonymy S ON S1.NameID = S.SynNameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                Database + ".dbo.TaxonName T1 ON S.NameID = T1.NameID " +
                " WHERE     (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND  " +
                " (S1.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString();
            if (ProjectList.Length > 0)
                SQL += " AND T.NameID NOT IN (SELECT     TP.NameID " +
                " FROM         TaxonName AS TP INNER JOIN " +
                " TaxonNameProject AS P ON TP.NameID = P.NameID " +
                " WHERE     (TP.IgnoreButKeepForReference = 0) AND (P.ProjectID IN (" + ProjectList + "))) ";
            SQL += " UNION " +
                " SELECT     TOP 100 PERCENT " + Database + ".dbo.BaseURL() + cast(T.NameID as varchar), T.TaxonNameCache AS AcceptedName,  " +
                Database + ".dbo.BaseURL() + cast(T1.NameID as varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName,  " +
                " T.TaxonomicRank, T.GenusOrSupragenericName,  " +
                Database + ".dbo.BaseURL() + cast(T.SpeciesGenusNameID as varchar),  " +
                " T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                " T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                " T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL " +
                " OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                " T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix " +
                " IS NULL THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, " + ProjectID.ToString() + " AS ProjectID " +
                " FROM         " + Database + ".dbo.TaxonSynonymy S2 INNER JOIN " +
                Database + ".dbo.TaxonAcceptedName A INNER JOIN " +
                Database + ".dbo.TaxonName T ON A.NameID = T.NameID ON S2.SynNameID = T.NameID INNER JOIN " +
                Database + ".dbo.TaxonSynonymy S INNER JOIN " +
                Database + ".dbo.TaxonSynonymy S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                Database + ".dbo.TaxonName T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID " +
                " WHERE     (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND  " +
                " (S1.IgnoreButKeepForReference = 0) AND (S2.IgnoreButKeepForReference = 0) AND A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString();
            if (ProjectList.Length > 0)
                SQL += " AND T.NameID NOT IN (SELECT     TP.NameID " +
                " FROM         TaxonName AS TP INNER JOIN " +
                " TaxonNameProject AS P ON TP.NameID = P.NameID " +
                " WHERE     (TP.IgnoreButKeepForReference = 0) AND (P.ProjectID IN (" + ProjectList + "))) ";
            SQL += " UNION " +
                " SELECT     TOP 100 PERCENT " + Database + ".dbo.BaseURL() + cast(T.NameID as varchar), T.TaxonNameCache AS AcceptedName,  " +
                Database + ".dbo.BaseURL() + cast(T1.NameID as varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName,  " +
                " T.TaxonomicRank, T.GenusOrSupragenericName,  " +
                Database + ".dbo.BaseURL() + cast(T.SpeciesGenusNameID as varchar),  " +
                " T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                " T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                " T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL " +
                " OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                " T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix " +
                " IS NULL THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, " + ProjectID.ToString() + " AS ProjectID " +
                " FROM         " + Database + ".dbo.TaxonSynonymy S3 INNER JOIN " +
                Database + ".dbo.TaxonAcceptedName A INNER JOIN " +
                Database + ".dbo.TaxonName T ON A.NameID = T.NameID ON S3.SynNameID = T.NameID INNER JOIN " +
                Database + ".dbo.TaxonSynonymy S2 INNER JOIN " +
                Database + ".dbo.TaxonSynonymy S INNER JOIN " +
                Database + ".dbo.TaxonSynonymy S1 ON S.SynNameID = S1.NameID  AND S.ProjectID = S1.ProjectID INNER JOIN " +
                Database + ".dbo.TaxonName T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID ON S3.NameID = S2.SynNameID AND S3.ProjectID = S1.ProjectID " +
                " WHERE     (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T.IgnoreButKeepForReference = 0) AND  " +
                " (S1.IgnoreButKeepForReference = 0) AND (S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) " +
                " AND A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString();
            if (ProjectList.Length > 0)
                SQL += " AND T.NameID NOT IN (SELECT     TP.NameID " +
                " FROM         TaxonName AS TP INNER JOIN " +
                " TaxonNameProject AS P ON TP.NameID = P.NameID " +
                " WHERE     (TP.IgnoreButKeepForReference = 0) AND (P.ProjectID IN (" + ProjectList + "))) ";
            SQL += " UNION " +
                " SELECT     TOP 100 PERCENT " + Database + ".dbo.BaseURL() + cast(T.NameID as varchar), T.TaxonNameCache AS AcceptedName,  " +
                Database + ".dbo.BaseURL() + cast(T1.NameID as varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName,  " +
                " T.TaxonomicRank, T.GenusOrSupragenericName,  " +
                Database + ".dbo.BaseURL() + cast(T.SpeciesGenusNameID as varchar),  " +
                " T.GenusOrSupragenericName + CASE WHEN T.InfragenericEpithet IS NULL OR " +
                " T.InfragenericEpithet = '' THEN '' ELSE ' ' + T.TaxonomicRank + ' ' + T.InfragenericEpithet END + CASE WHEN T.SpeciesEpithet IS NULL OR " +
                " T.SpeciesEpithet = '' THEN '' ELSE ' ' + T.SpeciesEpithet END + CASE WHEN T.InfraspecificEpithet IS NULL OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet <> T.InfraspecificEpithet THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.InfraspecificEpithet IS NULL " +
                " OR " +
                " T.InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T.SpeciesEpithet = T.InfraspecificEpithet AND NOT T.InfraspecificEpithet IS NULL AND  " +
                " T.InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T.TaxonomicRank IS NULL OR " +
                " T.TaxonomicRank = '' THEN '' ELSE T.TaxonomicRank + ' ' END + T.InfraspecificEpithet ELSE '' END END + CASE WHEN T.NonNomenclaturalNameSuffix " +
                " IS NULL THEN '' ELSE ' ' + T.NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, " + ProjectID.ToString() + " AS ProjectID " +
                " FROM         " + Database + ".dbo.TaxonSynonymy S1 INNER JOIN " +
                Database + ".dbo.TaxonName T1 ON S1.NameID = T1.NameID INNER JOIN " +
                Database + ".dbo.TaxonSynonymy S INNER JOIN " +
                Database + ".dbo.TaxonName T ON S.SynNameID = T.NameID INNER JOIN " +
                Database + ".dbo.TaxonAcceptedName A ON T.NameID = A.NameID ON S1.SynNameID = S.NameID AND S.ProjectID = S1.ProjectID " +
                " WHERE     (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND  " +
                " (S1.SynType = N'duplicate') OR " +
                " (T.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'isonym')  " +
                " AND A.ProjectID = " + ProjectID.ToString() + " AND S.ProjectID = " + ProjectID.ToString();
            if (ProjectList.Length > 0)
                SQL += " AND T.NameID NOT IN (SELECT     TP.NameID " +
                " FROM         TaxonName AS TP INNER JOIN " +
                " TaxonNameProject AS P ON TP.NameID = P.NameID " +
                " WHERE     (TP.IgnoreButKeepForReference = 0) AND (P.ProjectID IN (" + ProjectList + "))) ";
            SQL += " ORDER BY AcceptedName, SynonymName ";
            return SQL;
        }

    }

}
