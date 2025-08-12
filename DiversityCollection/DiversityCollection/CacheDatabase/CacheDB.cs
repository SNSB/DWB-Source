using DiversityWorkbench;
using System;
using System.Collections.Generic;
using System.Data;

namespace DiversityCollection.CacheDatabase
{
    public class CacheDB
    {
        ///TODO:
        /*
         * die Datenbank soll auf einem beliebigen Server liegen koennen. 
         * Dazu werden alle notwendigen Sichten und Prozeduren in der Quell DB angelegt und die Daten in Tabellen
         * auf der CacheDB importiert.
         * Die Sichten koennen per Skript angelegt werden
         * Die Tabellen per SqlCommands
         * Informationen die ausgelesen werden:
         * 
         * CollectionAgent
         * CollectionProject
         * CollectionSpecimen
         * CollectionSpecimenImage
         * CollectionStorage
         * IdentificationUnit
         * ...
         * 
         * Metadata
         * 
         * sonstige Tabellen:
         * 
         * ProjectDataType_Enum
         * ProjectProxy
         * ProjectTaxonomicGroup
         * 
         * DiversityTaxonNames
         * DiversityTaxonNamesProjectSequence
         * TaxonSynonymy
         * 
         * Grundlagen:
         * 
         * Tabelle Identification. Wird in CacheDB angelegt, dann von Collection aus gefuellt und per Update mit Werten der Tabelle TaxonSynonymy geaendert
         * */

        public enum TransferSetting {TaxonomicGroup, MaterialCategory, Localisation, Images, ImagesSpecimen, ImagesEvent, ImagesSeries};

        #region Properties

        public static string ReportsDirectory()
        {
            string Dir = Folder.CacheDB(Folder.CacheDBFolder.Reports); 
            System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(Dir);
            if (!D.Exists)
                D.Create();
            return Dir;
        }

        public static string DiagnosticsDirectory()
        {
            string Dir = Folder.CacheDB(Folder.CacheDBFolder.Diagnostics);
            System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(Dir);
            if (!D.Exists)
                D.Create();
            return Dir;
        }

        private static System.Collections.Generic.List<string> _CollectionTables;

        public static System.Collections.Generic.List<string> CollectionTables
        {
            get 
            {
                if (CacheDB._CollectionTables == null)
                {
                    CacheDB._CollectionTables = new List<string>();
                    CacheDB._CollectionTables.Add("CollectionAgent");
                    CacheDB._CollectionTables.Add("CollectionEvent");
                    CacheDB._CollectionTables.Add("CollectionEventImage");
                    CacheDB._CollectionTables.Add("CollectionEventLocalisation");
                    CacheDB._CollectionTables.Add("CollectionEventProperty");
                    CacheDB._CollectionTables.Add("CollectionEventSeries");
                    CacheDB._CollectionTables.Add("CollectionEventSeriesImage");
                    CacheDB._CollectionTables.Add("CollectionSpecimen");
                    CacheDB._CollectionTables.Add("CollectionSpecimenPart");
                    CacheDB._CollectionTables.Add("CollectionSpecimenRelation");
                    CacheDB._CollectionTables.Add("CollectionSpecimenReference");
                    CacheDB._CollectionTables.Add("Identification");
                    CacheDB._CollectionTables.Add("IdentificationUnit");
                    CacheDB._CollectionTables.Add("IdentificationUnitAnalysis");
                    CacheDB._CollectionTables.Add("IdentificationUnitInPart");
                }
                return CacheDB._CollectionTables; 
            }
            //set { CacheDB._CollectionTables = value; }
        }

        public static System.Collections.Generic.Dictionary<string, string> CollectionTableColumns(string TableName)
        {
            System.Collections.Generic.Dictionary<string, string> D = new Dictionary<string, string>();
            string SQL = "select C.COLUMN_NAME, C.DATA_TYPE from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = '" + TableName + "' " +
                "order by C.ORDINAL_POSITION";
            System.Data.DataTable DtSource = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(DtSource);
            System.Data.DataTable DtCache = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter adCache = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            adCache.Fill(DtCache);
            foreach (System.Data.DataRow R in DtSource.Rows)
            {
                System.Data.DataRow[] rr = DtCache.Select("COLUMN_NAME = '" + R[0].ToString() + "'");
                if (rr.Length > 0)
                    D.Add(R[0].ToString(), R[1].ToString());
            }
            return D;
        }

        public static System.Data.DataTable DataTable(string TableName, int ProjectID, bool IsFromSource)
        {
            System.Data.DataTable dt = new DataTable();
            try
            {
                string SQL = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityCollection.CacheDatabase.CacheDB.CollectionTableColumns(TableName))
                {
                    if (SQL.Length > 0)
                        SQL += ", ";
                    if (KV.Value == "datetime")
                        SQL += "CONVERT(DATETIME, T." + KV.Key + ", 102) AS " + KV.Key;
                    else if (TableName == "CollectionEventLocalisation"
                        && (KV.Key == "Location1"
                        || KV.Key == "Location2"
                        || KV.Key == "Geography"
                        || KV.Key == "AverageLatitudeCache"
                        || KV.Key == "AverageLongitudeCache"))
                    {
                        int? Precision = DiversityCollection.CacheDatabase.CacheDB.CoordinatePrecision(ProjectID);
                        if (Precision != null)
                        {
                            if (KV.Key == "Location1"
                               || KV.Key == "Location2")
                            {
                                SQL += "CASE WHEN  T.LocalisationSystemID IN (1, 2, 6, 8, 9, 12, 16) THEN NULL ELSE " + KV.Key + " END AS " + KV.Key;
                            }
                            else if (KV.Key == "Geography")
                            {
                                SQL += " NULL AS " + KV.Key;
                            }
                            else
                            {
                                SQL += "ROUND(" + KV.Key + ", " + Precision.ToString() + ") AS " + KV.Key;
                            }
                        }
                        else
                            SQL += "T." + KV.Key;
                    }
                    else
                        SQL += "T." + KV.Key;
                }
                SQL = "SELECT " + SQL + " FROM " + TableName + " AS T ";
                string TableAliasCollectionSpecimenID = "S";
                if (!IsFromSource)
                {
                    SQL += " WHERE ProjectID = " + ProjectID.ToString();
                }
                else
                {
                    SQL += ", CollectionProject AS P ";
                    if (TableName.IndexOf("Event") > -1)
                    {
                        if (TableName.IndexOf("Series") > -1)
                            SQL += ", CollectionEvent E, CollectionSpecimen AS S WHERE P.CollectionSpecimenID = S.CollectionSpecimenID " +
                            " AND E.CollectionEventID = S.CollectionEventID AND E.SeriesID = T.SeriesID ";
                        else
                        SQL += ", CollectionSpecimen AS S WHERE P.CollectionSpecimenID = S.CollectionSpecimenID " +
                            " AND T.CollectionEventID = S.CollectionEventID ";
                    }
                    else
                    {
                        TableAliasCollectionSpecimenID = "T";
                        if (TableName.IndexOf("Identification") > -1 && TableName != "IdentificationUnit")
                            SQL += " , IdentificationUnit U WHERE U.IdentificationUnitID = T.IdentificationUnitID AND  P.CollectionSpecimenID = T.CollectionSpecimenID ";
                        else
                            SQL += " WHERE P.CollectionSpecimenID = T.CollectionSpecimenID ";
                    }
                    SQL += " AND P.ProjectID = " + ProjectID.ToString();
                }
                if (IsFromSource)
                    SQL += DiversityCollection.CacheDatabase.CacheDB.DataTableRestriction(TableName, TableAliasCollectionSpecimenID, ProjectID);
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                if (IsFromSource)
                    ad.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                ad.SelectCommand.CommandTimeout = 9999;
                ad.Fill(dt);
            }
            catch (System.Exception ex) { }
            return dt;
        }

        public static int? CoordinatePrecision(int ProjectID)
        {
            string SQL = "SELECT CoordinatePrecision " +
                "FROM ProjectProxy AS P " +
                "WHERE ProjectID = " + ProjectID.ToString();
            int Precision;
            if (int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL).ToString(), out Precision))
                return Precision;
            return null;
        }

        private static string DataTableRestriction(string TableName, string TableAliasCollectionSpecimenID, int ProjectID)
        {
            string Restriction = "";
            // Embargo
            Restriction += " AND " + TableAliasCollectionSpecimenID + ".CollectionSpecimenID NOT IN (SELECT CP.CollectionSpecimenID " +
                "FROM CollectionSpecimenTransaction AS CST INNER JOIN " +
                "[Transaction] AS TR ON CST.TransactionID = TR.TransactionID INNER JOIN " +
                "CollectionProject AS CP ON CST.CollectionSpecimenID = CP.CollectionSpecimenID " +
                "WHERE (TR.TransactionType = N'embargo') AND (CP.ProjectID = " + ProjectID.ToString() + ") AND (TR.BeginDate IS NULL OR " +
                "TR.BeginDate <= GETDATE()) AND (TR.AgreedEndDate IS NULL OR " +
                "TR.AgreedEndDate >= GETDATE())) ";

            // DataWithholdingReason in Main table
            if (TableName != "CollectionSpecimen")
                Restriction += " AND " + TableAliasCollectionSpecimenID + ".CollectionSpecimenID IN (SELECT CS.CollectionSpecimenID " +
                    "FROM CollectionSpecimen CS, CollectionProject CP WHERE DataWithholdingReason = '' OR DataWithholdingReason IS NULL " +
                    "AND CS.CollectionSpecimenID = CP.CollectionSpecimenID AND CP.ProjectID = " + ProjectID.ToString() + ") ";

            // DataWithholdingReason
            string SQL = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                "WHERE (COLUMN_NAME = 'DataWithholdingReason') AND (TABLE_NAME = '" + TableName + "') ";
            string CheckTable = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (CheckTable == TableName)
                Restriction += " AND (T.DataWithholdingReason = '' OR T.DataWithholdingReason IS NULL) ";

            string Taxa = "";
            if (TableName.IndexOf("Identification") > -1)
                Taxa = DiversityCollection.CacheDatabase.CacheDB.TransferRestriction(ProjectID, "TaxonomicGroup");
            switch (TableName)
            {
                case "IdentificationUnit":
                    if (Taxa.Length > 0)
                        Restriction += " AND T.TaxonomicGroup IN (" + Taxa +") ";
                    break;
                case "Identification":
                case "IdentificationUnitAnalysis":
                    if (Taxa.Length > 0)
                        Restriction += " AND U.TaxonomicGroup IN (" + Taxa + ") ";
                    break;
                case "CollectionSpecimenPart":
                    string Mat = DiversityCollection.CacheDatabase.CacheDB.TransferRestriction(ProjectID, "MaterialCategory");
                    if (Mat.Length > 0)
                        Restriction += " AND T.MaterialCategory IN (" + Mat + ") ";
                    break;
                case "CollectionEventLocalisation":
                    string Loc = DiversityCollection.CacheDatabase.CacheDB.TransferRestriction(ProjectID, "Localisation");
                    if (Loc.Length > 0)
                        Restriction += " AND T.LocalisationSystemID IN (" + Loc + ") ";
                    break;
            }
            return Restriction;
        }

        public static string TransferRestriction(int ProjectID, string TransferSetting)
        {
            string Restriction = "";
            string SQL = "SELECT Value " +
                "FROM ProjectTransferSetting " +
                "WHERE (ProjectID = " + ProjectID.ToString() + ") AND (TransferSetting = N'" + TransferSetting + "') AND (TransferToCache = 1)";
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            ad.Fill(dt);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (Restriction.Length > 0)
                    Restriction += ", ";
                Restriction += "'" + R[0].ToString() + "'";
            }
            return Restriction;
        }

        public static System.Data.DataTable DtInterface()
        {
            System.Data.DataTable dt = new DataTable();
            try
            {
                string SQL = "SELECT InterfaceName FROM Interface ORDER BY InterfaceName";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dt);
            }
            catch (System.Exception ex) { }
            return dt;
        }

        public static System.Data.DataTable DtInterfaceTables(string Interface)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT TableName FROM InterfaceTable " +
                "WHERE (InterfaceName = N'" + Interface + "') ORDER BY TableName";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            ad.Fill(dt);
            return dt;
        }

        public static System.Data.DataTable DtInterfaceTable(string InterfaceTable, int ProjectID)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT * FROM " + InterfaceTable +
                " WHERE ProjectID = " + ProjectID.ToString();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dt);
            }
            catch (System.Exception ex) { }
            return dt;
        }

        public static bool HasAccessToDatabase(ref string Message)
        {
            bool HasAccess = false;
            string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.DOMAINS";
            Message = "";
            string Test = ExecuteSqlSkalarInCacheDB(SQL, ref Message);
            if (Message.Length == 0 && Test.Length > 0)
            {
                HasAccess = true;
            }
            return HasAccess;
        }

        private static string _DatabaseServer;
        public static string DatabaseServer
        {
            get
            {
                return DiversityCollection.CacheDatabase.CacheDB._DatabaseServer;
            }
            set
            {
                DiversityCollection.CacheDatabase.CacheDB._DatabaseServer = value;
            }
        }

        private static int _DatabaseServerPort;
        public static int DatabaseServerPort
        {
            get { return CacheDB._DatabaseServerPort; }
            set { CacheDB._DatabaseServerPort = value; }
        }

        private static string _DatabaseName;
        public static string DatabaseName
        {
            get
            {
                if (DiversityCollection.CacheDatabase.CacheDB._DatabaseName == null)
                    DiversityCollection.CacheDatabase.CacheDB._DatabaseName = "";
                return DiversityCollection.CacheDatabase.CacheDB._DatabaseName;
            }
            set
            {
                DiversityCollection.CacheDatabase.CacheDB._DatabaseName = value;
            }
        }

        public static string CacheDatabaseName()
        {
            if (_DatabaseName == null || _DatabaseName.Length == 0)
            {
                _DatabaseName = DiversityWorkbench.CacheDatabase.CollectionCacheDB;
                //string SQL = "SELECT DatabaseName FROM CacheDatabase 2";
                //_DatabaseName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            }
            return _DatabaseName;
        }

        #region Linked server - does not work for unkown reason ...

        //public static bool setPostgresLinkedServer(string LinkedServer)
        //{
        //    bool OK = true;
        //    string SQL = "SELECT COUNT(*) FROM PostgresDatabase " +
        //        " WHERE Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
        //        " AND Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() +
        //        " AND DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
        //    string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
        //    if (Result == "1")
        //    {
        //        SQL = "UPDATE PostgresDatabase SET LinkedServer = '" + LinkedServer + "' " +
        //        " WHERE Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
        //        " AND Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() +
        //        " AND DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
        //        OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        //    }
        //    else
        //    {
        //        SQL = "INSERT INTO PostgresDatabase (Server, Port, DatabaseName, LinkedServer) " +
        //            " VALUES ('" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "', " +
        //            DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() + ", '" +
        //            DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "', '" +
        //            LinkedServer + "')";
        //        OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        //    }
        //    return OK;
        //}

        //public static string getPostgresLinkedServer()
        //{
        //    string LS = "";
        //    string SQL = "SELECT MIN(LinkedServer) FROM PostgresDatabase " +
        //        " WHERE Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
        //        " AND Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() +
        //        " AND DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
        //    LS = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
        //    return LS;
        //}

        //public static string getLinkedServerForCurrentPostgresDatabase()
        //{
        //    string LinkedServer = "";
        //    string SQL = "";
        //    System.Collections.Generic.List<string> LS = new List<string>();
        //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.LinkedServer> KV in DiversityWorkbench.LinkedServer.LinkedServers())
        //    {
        //        if (KV.Value.Databases().Count == 0) // ensure it is no SQL-Server database
        //        {
        //            SQL = "select count(*) " +
        //                " from [" + KV.Key + "].[" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "].[public].[LinkedServerInfo] I " +
        //                " WHERE I.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
        //                " AND I.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() +
        //                " AND I.DatebaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
        //            string Message = "";
        //            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
        //            if (Message.Length == 0 && Result == "1") // ensure it is a postgres cache database
        //            {
        //                LinkedServer = KV.Key;
        //                break;
        //            }
        //        }
        //    }
        //    SQL = "UPDATE [Target] SET LinkedServer = '" + LinkedServer + "' WHERE TargetID = " + TargetID();
        //    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        //    return LinkedServer;
        //}
        
        #endregion

        private static string _DatabaseVersion;
        public static string DatabaseVersion
        {
            get
            {
                if ((_DatabaseVersion == null || _DatabaseVersion.Length < 8) && DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    string SQL = "SELECT dbo.Version()";
                    _DatabaseVersion = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                }
                return DiversityCollection.CacheDatabase.CacheDB._DatabaseVersion;
            }
            set
            {
                DiversityCollection.CacheDatabase.CacheDB._DatabaseVersion = value;
            }
        }

        private static string _ProjectsDatabase;
        public static string ProjectsDatabase
        {
            get
            {
                if (DiversityCollection.CacheDatabase.CacheDB._ProjectsDatabase == null)
                    DiversityCollection.CacheDatabase.CacheDB._ProjectsDatabase = "";
                return DiversityCollection.CacheDatabase.CacheDB._ProjectsDatabase;
            }
            set
            {
                DiversityCollection.CacheDatabase.CacheDB._ProjectsDatabase = value;
            }
        }

        private static string _ConnectionStringCacheDB;

        // #137
        ///<summary>
        ///Reset cache database connection string.
        ///</summary>
        public static void ResetConnectionStringCacheDB()
        {
            DiversityCollection.CacheDatabase.CacheDB._ConnectionStringCacheDB = null;
        }

        public static string ConnectionStringCacheDB
        {
            get
            {
                if (_ConnectionStringCacheDB == null || _ConnectionStringCacheDB.Length == 0)
                {
                    if (DiversityCollection.CacheDatabase.CacheDB._DatabaseServer == null
                        || DiversityCollection.CacheDatabase.CacheDB._DatabaseServer.Length == 0
                        || DiversityCollection.CacheDatabase.CacheDB._DatabaseName == null
                        || DiversityCollection.CacheDatabase.CacheDB._DatabaseName.Length == 0)
                        return "";
                }
                _ConnectionStringCacheDB = "Data Source=" + DiversityCollection.CacheDatabase.CacheDB.DatabaseServer + "," + DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort.ToString();
                _ConnectionStringCacheDB += ";initial catalog=" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + ";";
                if (DiversityWorkbench.Settings.IsTrustedConnection)
                    _ConnectionStringCacheDB += "Integrated Security=True";
                else
                {
                    if (DiversityWorkbench.Settings.DatabaseUser.Length > 0 && DiversityWorkbench.Settings.Password.Length > 0)
                        _ConnectionStringCacheDB += "user id=" + DiversityWorkbench.Settings.DatabaseUser + ";password=" + DiversityWorkbench.Settings.Password;
                    else _ConnectionStringCacheDB = "";
                }
                _ConnectionStringCacheDB += ";Connection Timeout=" + DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB.ToString() + ";";

                // MW 2018/10/01: Encripted connection
                if (_ConnectionStringCacheDB.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
                {
                    if (!_ConnectionStringCacheDB.EndsWith(";"))
                        _ConnectionStringCacheDB += ";";
                    _ConnectionStringCacheDB += "Encrypt=true;TrustServerCertificate=true";
                }

                return _ConnectionStringCacheDB;
            }
        }

        public static string ConnectionStringCacheDBWithTimeOut(int TimeOut)
        {
            if (_ConnectionStringCacheDB == null || _ConnectionStringCacheDB.Length == 0)
            {
                if (DiversityCollection.CacheDatabase.CacheDB._DatabaseServer == null
                    || DiversityCollection.CacheDatabase.CacheDB._DatabaseServer.Length == 0
                    || DiversityCollection.CacheDatabase.CacheDB._DatabaseName == null
                    || DiversityCollection.CacheDatabase.CacheDB._DatabaseName.Length == 0)
                    return "";
            }
            _ConnectionStringCacheDB = "Data Source=" + DiversityCollection.CacheDatabase.CacheDB.DatabaseServer + "," + DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort.ToString();
            _ConnectionStringCacheDB += ";initial catalog=" + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + ";";
            if (DiversityWorkbench.Settings.IsTrustedConnection)
                _ConnectionStringCacheDB += "Integrated Security=True";
            else
            {
                if (DiversityWorkbench.Settings.DatabaseUser.Length > 0 && DiversityWorkbench.Settings.Password.Length > 0)
                    _ConnectionStringCacheDB += "user id=" + DiversityWorkbench.Settings.DatabaseUser + ";password=" + DiversityWorkbench.Settings.Password;
                else _ConnectionStringCacheDB = "";
            }
            _ConnectionStringCacheDB += ";Connection Timeout=" + TimeOut.ToString() + ";";

            // MW 2018/10/01: Encripted connection
            if (_ConnectionStringCacheDB.Length > 0 && DiversityWorkbench.Settings.IsEncryptedConnection)
            {
                if (!_ConnectionStringCacheDB.EndsWith(";"))
                    _ConnectionStringCacheDB += ";";
                _ConnectionStringCacheDB += "Encrypt=true;TrustServerCertificate=true";
            }

            return _ConnectionStringCacheDB;
        }

        public static readonly string SqlDiversityTaxonNames = "SELECT DataSoure, DatabaseName, LastUpdate FROM DiversityTaxonNames ORDER BY DataSoure, DatabaseName";

        private static Microsoft.Data.SqlClient.SqlConnection _ConnGazetteer;
        public static Microsoft.Data.SqlClient.SqlConnection ConnectionGazetteer()
        {
            if (_ConnGazetteer == null)
            {
                DiversityWorkbench.WorkbenchUnit G = DiversityWorkbench.WorkbenchUnit.getWorkbenchUnit("DiversityGazetteer");
                DiversityWorkbench.ServerConnection SC = new DiversityWorkbench.ServerConnection();
                string ConnectionString = "";
                if (G.ServerConnectionList().Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in G.ServerConnectionList())
                    {
                        ConnectionString = KV.Value.ConnectionString;
                        SC = KV.Value;
                        break;
                    }
                }
                if (ConnectionString.Length > 0)
                {
                    _ConnGazetteer = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
                    string SQL = "SELECT COUNT(*) FROM GeoName WHERE PlaceID IS NULL";
                    if (SC.LinkedServer.Length > 0)
                        SQL = "SELECT COUNT(*) FROM [" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.GeoName WHERE PlaceID IS NULL";
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, _ConnGazetteer);
                    _ConnGazetteer.Open();
                    try
                    {
                        string x = C.ExecuteScalar().ToString();
                        _ConnGazetteer.Close();
                    }
                    catch (System.Exception ex)
                    {
                        _ConnGazetteer = null;
                        System.Windows.Forms.MessageBox.Show("No connection to Gazetteer established");
                    }
                }
            }
            return _ConnGazetteer;
        }


        #endregion

        #region Taxonomy

        private static System.Collections.Generic.Dictionary<string, CacheDBTaxonSource> _CacheDBTaxonSources;
        public static System.Collections.Generic.Dictionary<string, CacheDBTaxonSource> CacheDBTaxonSources
        {
            get
            {
                if (DiversityCollection.CacheDatabase.CacheDB._CacheDBTaxonSources == null)
                    DiversityCollection.CacheDatabase.CacheDB._CacheDBTaxonSources = new Dictionary<string, CacheDBTaxonSource>();
                return DiversityCollection.CacheDatabase.CacheDB._CacheDBTaxonSources;
            }
        }

        public static bool CacheDBTaxonSourcesAdd(string DataSource, string DataBase, System.Collections.Generic.Dictionary<int, string> ProjectIDs, System.Data.DataTable DtTaxonSynonymy)
        {
            bool OK = false;
            try
            {
                string Key = DataSource + "." + DataBase;
                DiversityCollection.CacheDatabase.CacheDBTaxonSource CTS = new CacheDBTaxonSource(DataSource, DataBase, ProjectIDs, DtTaxonSynonymy);
                DiversityCollection.CacheDatabase.CacheDB.CacheDBTaxonSources.Add(Key, CTS);
                OK = true;
            }
            catch (System.Exception ex) { }
            return OK;
        }

        public static DiversityCollection.CacheDatabase.CacheDBTaxonSource CacheDBTaxonSourceGet(string DataSource, string DataBase)
        {
            string Key = DataSource + "." + DataBase;
            return DiversityCollection.CacheDatabase.CacheDB.CacheDBTaxonSources[Key];
        }
        
        #endregion

        #region Projects
        
        private static System.Data.DataTable _DtProjects;
        public static System.Data.DataTable DtProjects
        {
            get
            {
                if (DiversityCollection.CacheDatabase.CacheDB._DtProjects == null)
                {
                    string SQL = "SELECT  ProjectID, Project, ProjectURI, CoordinatePrecision, LastUpdatedWhen, LastUpdatedBy " +
                        "FROM   ProjectPublished " +
                        "ORDER BY Project";
                    //string SQL = "SELECT  ProjectID, Project, ProjectURI, ProjectSettings, DataType, CoordinatePrecision " +
                    //    "FROM   ProjectProxy" +
                    //    "ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    try
                    {
                        DiversityCollection.CacheDatabase.CacheDB._DtProjects = new System.Data.DataTable();
                        ad.Fill(DiversityCollection.CacheDatabase.CacheDB._DtProjects);
                    }
                    catch (System.Exception ex)
                    {
                        DiversityCollection.CacheDatabase.CacheDB._DtProjects = null;
                    }
                }
                return DiversityCollection.CacheDatabase.CacheDB._DtProjects;
            }
        }

        public static void ResetDtProjects()
        {
            DiversityCollection.CacheDatabase.CacheDB._DtProjects = null;
        }

        public static System.Data.DataTable DtProjectTransferSetting(int ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting TransferSetting)
        {
            System.Data.DataTable dtSetting = new DataTable();
            try
            {
                string SQL = "SELECT ProjectID, TransferSetting, Value, DisplayText, TransferToCache " +
                    "FROM ProjectTransferSetting AS T " +
                    "WHERE (ProjectID = " + ProjectID.ToString() + ") AND (TransferSetting = '" + TransferSetting.ToString() + "') " +
                    "ORDER BY Value";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dtSetting);
            }
            catch (Exception ex)
            {
            }
            return dtSetting;
        }

        public static bool RemoveProjectTransferSetting(int ProjectID, string TransferSetting)
        {
            bool OK = true;
            try
            {
                string SQL = "DELETE T " +
                    "FROM ProjectTransferSetting AS T " +
                    "WHERE (ProjectID = " + ProjectID.ToString() + ") AND (TransferSetting = '" + TransferSetting + "') " ;
            }
            catch (Exception)
            {
            }
            return OK;
        }

        /// <summary>
        /// Add a setting for the transfer
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TransferSetting"></param>
        /// <returns></returns>
        public static bool AddProjectTransferSetting(int ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting TransferSetting)
        {
            bool OK = true;
            try
            {
                System.Data.DataTable dtSource = DiversityCollection.CacheDatabase.CacheDB.DtProjectTransferSettingSource(ProjectID, TransferSetting);
                string SQL = "";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                con.Open();
                foreach (System.Data.DataRow R in dtSource.Rows)
                {
                    SQL = "INSERT INTO ProjectTransferSetting " +
                        "(ProjectID, TransferSetting, Value, DisplayText, TransferToCache) " +
                        "VALUES (" + ProjectID.ToString() + ", '" + TransferSetting.ToString() + "', '" + R["Value"].ToString() + "', '" + R["DisplayText"].ToString() + "', 1)";
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        C.ExecuteNonQuery();
                    }
                    catch (System.Exception ex) { }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        /// <summary>
        /// If additional options where added since the last check, these can be added here
        /// </summary>
        /// <param name="ProjectID">The ID of the Project</param>
        /// <param name="TransferSetting"></param>
        /// <returns></returns>
        public static bool RefreshProjectTransferSetting(int ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting TransferSetting)
        {
            bool OK = true;
            try
            {
                System.Data.DataTable dtSource = DiversityCollection.CacheDatabase.CacheDB.DtProjectTransferSettingSource(ProjectID, TransferSetting);
                string SQL = "";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                con.Open();
                foreach (System.Data.DataRow R in dtSource.Rows)
                {
                    SQL = "INSERT INTO ProjectTransferSetting " +
                        "(ProjectID, TransferSetting, Value, DisplayText, TransferToCache) " +
                        "SELECT (" + ProjectID.ToString() + ", '" + TransferSetting.ToString() + "', '" + R["Value"].ToString() + "', '" + R["DisplayText"].ToString() + "', 1) " +
                        "WHERE NOT EXISTS (SELECT * FROM ProjectTransferSetting T WHERE T.ProjectID = "  + ProjectID.ToString() + " AND TransferSetting = '" + TransferSetting.ToString() + "' AND Value = '" + R["Value"].ToString() + "'";
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        C.ExecuteNonQuery();
                    }
                    catch (System.Exception ex) { }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        /// <summary>
        /// Table containg the content of the source data for transfer in the table ProjectTransferSetting
        /// </summary>
        /// <param name="ProjectID">The ID of the project</param>
        /// <param name="TransferSetting">The transfer setting</param>
        /// <returns></returns>
        private static System.Data.DataTable DtProjectTransferSettingSource(int ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting TransferSetting)
        {
            System.Data.DataTable dtSource = new DataTable();
            try
            {
                string SQL = "";
                switch (TransferSetting)
                {
                    case CacheDB.TransferSetting.TaxonomicGroup:
                        SQL = "SELECT DISTINCT    T.Code AS Value, T.DisplayText " +
                            "FROM         IdentificationUnit AS U INNER JOIN " +
                            "CollTaxonomicGroup_Enum AS T ON U.TaxonomicGroup = T.Code INNER JOIN " +
                            "CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                            "WHERE     (P.ProjectID = " + ProjectID.ToString() + ") " +
                            "ORDER BY T.DisplayText";
                        break;
                    case CacheDB.TransferSetting.MaterialCategory:
                        SQL = "SELECT  DISTINCT   M.Code AS Value, M.DisplayText " +
                            "FROM         CollectionProject AS P INNER JOIN " +
                            "CollectionSpecimenPart AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID INNER JOIN " +
                            "CollMaterialCategory_Enum AS M ON S.MaterialCategory = M.Code " +
                            "WHERE     (P.ProjectID = " + ProjectID.ToString() + ") " +
                            "ORDER BY M.DisplayText";
                        break;
                    case CacheDB.TransferSetting.Localisation:
                        SQL = "SELECT DISTINCT CAST(L.LocalisationSystemID AS VARCHAR) AS Value, L.DisplayText " +
                            "FROM         LocalisationSystem AS L INNER JOIN " +
                            "CollectionEventLocalisation AS E ON L.LocalisationSystemID = E.LocalisationSystemID INNER JOIN " +
                            "CollectionProject AS P INNER JOIN " +
                            "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID ON E.CollectionEventID = S.CollectionEventID " +
                            "WHERE     (P.ProjectID = " + ProjectID.ToString() + ")";
                        break;
                    case CacheDB.TransferSetting.ImagesSpecimen:
                        SQL = "SELECT  DISTINCT   E.Code AS Value, E.DisplayText " +
                            "FROM CollectionProject AS P INNER JOIN " +
                            "CollectionSpecimenImage AS I ON P.CollectionSpecimenID = I.CollectionSpecimenID INNER JOIN " +
                            "CollSpecimenImageType_Enum AS E ON I.ImageType = E.Code " +
                            "WHERE     (P.ProjectID = " + ProjectID.ToString() + ") " +
                            "ORDER BY E.DisplayText";
                        break;
                    case CacheDB.TransferSetting.ImagesEvent:
                        SQL = "SELECT DISTINCT E.Code AS Value, E.DisplayText " +
                            "FROM         CollectionSpecimen AS S INNER JOIN " +
                            "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                            "CollectionEventImage AS I INNER JOIN " +
                            "CollEventImageType_Enum AS E ON I.ImageType = E.Code ON S.CollectionEventID = I.CollectionEventID " +
                            "WHERE     (P.ProjectID = " + ProjectID.ToString() + ") " +
                            "ORDER BY E.DisplayText";
                        break;
                    case CacheDB.TransferSetting.ImagesSeries:
                        SQL = "SELECT DISTINCT E.Code AS Value, E.DisplayText " +
                            "FROM         CollectionEventSeriesImage AS I INNER JOIN " +
                            "CollEventSeriesImageType_Enum AS E ON I.ImageType = E.Code INNER JOIN " +
                            "CollectionSpecimen AS S INNER JOIN " +
                            "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                            "CollectionEvent AS C ON S.CollectionEventID = C.CollectionEventID ON I.SeriesID = C.SeriesID " +
                            "WHERE     (P.ProjectID = " + ProjectID.ToString() + ") " +
                            "ORDER BY E.DisplayText";
                        break;
                }
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtSource);
            }
            catch (Exception ex)
            {
            }
            return dtSource;
        }

        public static bool RemoveProjectTransferSetting(int ProjectID, DiversityCollection.CacheDatabase.CacheDB.TransferSetting TransferSetting)
        {
            bool OK = true;
            try
            {
                System.Data.DataTable dtSource = new DataTable();
                string SQL = "DELETE T FROM ProjectTransferSetting T WHERE T.ProjectID = " + ProjectID.ToString() + " AND TransferSetting = '" + TransferSetting.ToString() + "'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                con.Open();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    C.ExecuteNonQuery();
                }
                catch { }
                con.Close();
            }
            catch (Exception)
            {
                OK = false;
            }
            return OK;
        }

        #endregion

        #region User
        
        private static System.Data.DataTable _DtAdministrators;
        public static System.Data.DataTable DtAdministrators
        {
            get
            {
                if (DiversityCollection.CacheDatabase.CacheDB._DtAdministrators == null || DiversityCollection.CacheDatabase.CacheDB._DtAdministrators.Columns.Count == 0)
                {
                    string SQL = "SELECT   U.name " +
                        "FROM sys.sysmembers AS M INNER JOIN " +
                        "sys.sysusers AS U ON M.memberuid = U.uid INNER JOIN " +
                        "sys.sysusers AS R ON M.groupuid = R.uid " +
                        "WHERE     (R.name = 'CacheAdministrator')" +
                        "ORDER BY U.name";
                    DiversityCollection.CacheDatabase.CacheDB._DtAdministrators = new DataTable();
                    if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                        ad.Fill(DiversityCollection.CacheDatabase.CacheDB._DtAdministrators);
                    }
                }
                return DiversityCollection.CacheDatabase.CacheDB._DtAdministrators;
            }
        }

        private static System.Data.DataTable _DtUser;
        public static System.Data.DataTable DtUser
        {
            get
            {
                if (DiversityCollection.CacheDatabase.CacheDB._DtUser == null || DiversityCollection.CacheDatabase.CacheDB._DtUser.Columns.Count == 0)
                {
                    string SQL = "SELECT   U.name " +
                        "FROM         sys.sysmembers AS M INNER JOIN " +
                        "sys.sysusers AS U ON M.memberuid = U.uid INNER JOIN " +
                        "sys.sysusers AS R ON M.groupuid = R.uid " +
                        "WHERE     (R.name = 'CacheUser')" +
                        "ORDER BY U.name";
                    DiversityCollection.CacheDatabase.CacheDB._DtUser = new DataTable();
                    if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                        ad.Fill(DiversityCollection.CacheDatabase.CacheDB._DtUser);
                    }
                }
                return DiversityCollection.CacheDatabase.CacheDB._DtUser;
            }
        }

        private static System.Data.DataTable _DtLogins;
        public static System.Data.DataTable DtLogins
        {
            get
            {
                if (DiversityCollection.CacheDatabase.CacheDB._DtLogins == null)
                {
                    string SQL = "select u.name from sysusers " +
                        "u where u.hasdbaccess = 1 " +
                        "order by u.name ";
                    DiversityCollection.CacheDatabase.CacheDB._DtLogins = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(DiversityCollection.CacheDatabase.CacheDB._DtLogins);
                }
                return DiversityCollection.CacheDatabase.CacheDB._DtLogins;
            }
        }

        public static bool AddUserToAdmin(string User)
        {
            bool OK = false;
            string SQL = "IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + User + "') " +
                "DROP USER [" + User + "] ";
            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            if (OK)
            {
                SQL = "IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + User + "') " +
                    "CREATE USER [" + User + "] FOR LOGIN [" + User + "] WITH DEFAULT_SCHEMA=[dbo];";
                OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                if (OK)
                {
                    SQL = "EXEC sp_addrolemember N'CacheAdministrator', N'" + User + "'";
                    OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    if (OK) DiversityCollection.CacheDatabase.CacheDB._DtAdministrators = null;
                }
            }
            return OK;
        }

        public static bool AddUserToUser(string User)
        {
            bool OK = false;
            string SQL = "IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + User + "') " +
                "DROP USER [" + User + "] ";
            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            if (OK)
            {
                SQL = "IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + User + "') " +
                    "CREATE USER [" + User + "] FOR LOGIN [" + User + "] WITH DEFAULT_SCHEMA=[dbo];";
                OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                if (OK)
                {
                    SQL = "EXEC sp_addrolemember N'CacheUser', N'" + User + "'";
                    OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    if (OK) DiversityCollection.CacheDatabase.CacheDB._DtUser = null;
                }
            }
            return OK;
        }

        public static bool RemoveUserFromAdmin(string User)
        {
            bool OK = false;
            string SQL = "EXEC sp_droprolemember N'CacheAdministrator', N'" + User + "'";
            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            if (OK) DiversityCollection.CacheDatabase.CacheDB._DtAdministrators = null;
            return OK;
        }

        public static bool RemoveUserFromUser(string User)
        {
            bool OK = false;
            string SQL = "EXEC sp_droprolemember N'CacheUser', N'" + User + "'";
            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            if (OK) DiversityCollection.CacheDatabase.CacheDB._DtUser = null;
            return OK;
        }

        #endregion

        #region Auxillary

        #region Execute SQL

        private static Microsoft.Data.SqlClient.SqlConnection _ConnectionCacheDB;
        public static Microsoft.Data.SqlClient.SqlConnection ConnectionCacheDB()
        {
            if (_ConnectionCacheDB == null)
            {
                _ConnectionCacheDB = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            }
            return _ConnectionCacheDB;
        }

        public static bool ExecuteSqlNonQueryInCacheDB(string SQL, bool UseDefaultConnection = false)
        {
            bool OK = false;
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = null;
                if (UseDefaultConnection)
                    con = DiversityCollection.CacheDatabase.CacheDB.ConnectionCacheDB();
                else
                    con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                C.ExecuteNonQuery();
                if (!UseDefaultConnection)
                {
                    con.Close();
                    con.Dispose();
                }
                OK = true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return OK;
        }

        public static bool ExecuteSqlNonQueryInCacheDB(string SQL, ref string Message, bool Retry = false)
        {
            bool OK = false;
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                OK = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("Unable to write data to the transport connection") > -1 && !Retry)
                {
                    OK = ExecuteSqlNonQueryInCacheDB(SQL, ref Message, true);
                }
                else
                {
                    Message = ex.Message;
                    OK = false;
                }
            }
            return OK;
        }

        /// <summary>
        /// Execution of an SQL query encapsulated by a transaction and try catch
        /// </summary>
        /// <param name="SQL">the SQL command</param>
        /// <param name="Message">a message resulting from the catch block if the SQL command results in an error</param>
        /// <param name="Exception">a common exception message</param>
        /// <returns></returns>
        public static bool ExecuteSqlTransactionNonQueryInCacheDB(string SQL, ref string Message, ref string Exception, bool UseDefaultConnection = false)
        {
            bool OK = false;
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = null;
                if (UseDefaultConnection)
                    con = DiversityCollection.CacheDatabase.CacheDB.ConnectionCacheDB();
                else
                    con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                SQL = "BEGIN TRANSACTION [T1] " +
                    " BEGIN TRY " + SQL + "; SELECT ''; " +
                    " COMMIT TRANSACTION [T1]; " +
                    " END TRY " +
                    " BEGIN CATCH " +
                    " ROLLBACK TRANSACTION [T1]; select ERROR_MESSAGE() " +
                    " END CATCH";
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;// DiversityWorkbench.Settings.TimeoutDatabase;// 999999;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Message = C.ExecuteScalar()?.ToString() ?? "Error: CacheDB (line1106) SQL: " + SQL + " command returned null.";
                if (!UseDefaultConnection)
                {
                    con.Close();
                    con.Dispose();
                }
                if (Message.Length == 0)
                    OK = true;
            }
            catch (Exception ex)
            {
                Exception = ex.Message;
            }
            // Markus 9.4.2021 - Message wurde nicht übernommen - versuch per exception
            if (Message.Length > 0 && Exception.Length == 0)
                Exception = Message;
            return OK;
        }

        public static bool ExecuteSqlNonQueryInCacheDB(string SQL, byte MaxErrorClassAccepted)
        {
            bool OK = false;
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                OK = true;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                foreach (Microsoft.Data.SqlClient.SqlError Error in ex.Errors)
                {
                    if (Error.Class <= MaxErrorClassAccepted && Error.Class != 13 && Error.Class != 14 && Error.Class != 15)
                        OK = true;
                    else
                    {
                        OK = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return OK;
        }

        public enum ObjectType { Schema, Table , View, Function }

        /// <summary>
        /// Getting th
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ForPostgres"></param>
        /// <param name="Type"></param>
        /// <param name="Schema"></param>
        /// <returns></returns>
        public static bool ObjectExistsInDatabase(string Name, bool ForPostgres = false, ObjectType Type = ObjectType.Table, string Schema = "")
        {
            bool Exists = false;
            if (Schema.Length == 0)
            {
                if (ForPostgres)
                    Schema = "public";
                else
                    Schema = "dbo";
            }
            string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.";
            switch (Type)
            {
                case ObjectType.Schema:
                    SQL += "SCHEMATA S WHERE S.SCHEMA_NAME = '" + Name + "' ";
                    break;
                case ObjectType.Table:
                    SQL += "TABLES AS T " +
                        "WHERE TABLE_NAME = '" + Name + "' " +
                        "AND T.TABLE_SCHEMA = '" + Schema + "' " +
                        "AND T.TABLE_TYPE = 'BASE TABLE'";
                    break;
                case ObjectType.View:
                    SQL += "TABLES AS T " +
                        "WHERE TABLE_NAME = '" + Name + "' " +
                        "AND T.TABLE_SCHEMA = '" + Schema + "' " +
                        "AND T.TABLE_TYPE = 'VIEW'";
                    break;
                case ObjectType.Function:
                    SQL += "ROUTINES R WHERE R.ROUTINE_NAME = '" + Name + "' " +
                        "AND R.ROUTINE_SCHEMA = '" + Schema + "'";
                    break;
            }
            string Result = "";
            if (ForPostgres)
                Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
            else
                Result = ExecuteSqlSkalarInCacheDB(SQL);
            if (Result.Length > 0 && Result != "0")
                Exists = true;
            return Exists;
        }

        /// <summary>
        /// Execute query in cache database
        /// </summary>
        /// <param name="SQL">the </param>
        /// <param name="checkCount">Optional - if the query is simple enough to change it to select count(*) from ... to check if any data are present. Issue #36 </param>
        /// <returns></returns>
        //public static string ExecuteSqlSkalarInCacheDB(string SQL, bool checkCount = false)
        //{
        //    string Result = "";
        //    try
        //    {
        //        bool NoData = false;
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
        //        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        C.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
        //        con.Open();
        //        Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
        //        if (checkCount)
        //        {
        //            string SqlCount = "SELECT COUNT(*) " + SQL.Substring(SQL.IndexOf(" FROM "));
        //            Microsoft.Data.SqlClient.SqlCommand Check = new Microsoft.Data.SqlClient.SqlCommand(SqlCount, con);
        //            Result = Check.ExecuteScalar()?.ToString() ?? string.Empty;
        //            if (Result == "0")
        //            {
        //                NoData = true;
        //                Result = "";
        //            }
        //        }
        //        if (!NoData)
        //            Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
        //        con.Close();
        //        con.Dispose();
        //    }
        //    catch (Microsoft.Data.SqlClient.SqlException ex)
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //    }
        //    return Result;
        //}

        public static string ExecuteSqlSkalarInCacheDB(string sql, bool checkCount = false)
        {
            try
            {
                using (var connection = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB))
                {
                    connection.Open();
                    // If checkCount is true, execute the COUNT query
                    if (checkCount)
                    {
                        string countQuery = $"SELECT COUNT(*) {sql.Substring(sql.IndexOf(" FROM ", StringComparison.OrdinalIgnoreCase))}";
                        using (var countCommand = new Microsoft.Data.SqlClient.SqlCommand(countQuery, connection))
                        {
                            countCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                            var countResult = countCommand.ExecuteScalar()?.ToString() ?? string.Empty;
                            // If no data, return an empty string
                            if (countResult == "0")
                            {
                                return string.Empty;
                            }
                        }
                    }

                    using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                        return command.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                ExceptionHandling.WriteToErrorLogFile(ex);
                return string.Empty;
            }
            catch (Exception ex)
            {
                ExceptionHandling.WriteToErrorLogFile(ex);
                return string.Empty;
            }
        }
        public static string ExecuteSqlSkalarInCacheDB(string SQL, ref string Message)
        {
            string Result = "";
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                con.Open();
                Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
                con.Dispose();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Message = ex.Message;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return Result;
        }

        public static bool ExecuteSqlFillTableInCacheDB(string SQL, ref System.Data.DataTable DT, ref string Message)
        {
            bool OK = false;
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                ad.Fill(DT);
                OK = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return OK;
        }

        #endregion

        #region File transfer via bcp

        public static void ResetBulkTransfer()
        {
            _BulkTransferDirectory = null;
            _BulkTransferBashFile = null;
        }

        public static bool bcpCreateView(string SchemaName, string TableName, ref string Message, ref string SQL)
        {
            bool OK = true;
            // Get Table definition in Postgres
            SQL = "select c.column_name, c.data_type, c.ordinal_position from INFORMATION_SCHEMA.COLUMNS C " +
                "where c.table_schema = '" + SchemaName + "' " +
                "and c.table_name = '" + TableName + "' " +
                "order by c.ordinal_position";
            string SqlForMessage = SQL;
            System.Data.DataTable dtPostgres = new System.Data.DataTable();
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtPostgres, ref Message);
            if (!OK)
            {
                return OK;
            }

            // Remove previous entries
            SQL = "DELETE B FROM bcpPostgresTableDefinition B WHERE B.SchemaName = '" + SchemaName + "' " +
                " AND B.TableName = '" + TableName + "'";
            SqlForMessage += "\r\n\r\n" + SQL;
            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            if (!OK)
            {
                return OK;
            }

            // Insert definition
            foreach (System.Data.DataRow R in dtPostgres.Rows)
            {
                SQL = "INSERT INTO bcpPostgresTableDefinition(SchemaName, TableName, ColumnName, DataType, OrdinalPositon) " +
                    "VALUES ('" + SchemaName + "', '" + TableName + "', " +
                    "'" + R["column_name"].ToString() + "', " +
                    "'" + R["data_type"].ToString() + "', " +
                    R["ordinal_position"].ToString() + ")";
                OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
                if (!OK)
                {
                    return OK;
                }
            }

            // Create View
            SQL = "DECLARE @RC int " +
                "DECLARE @TableName varchar(200) " +
                "DECLARE @Schema varchar(200) " +
                "SET @TableName = '" + TableName + "' " +
                "SET @Schema = '" + SchemaName + "' " +
                "EXECUTE @RC = [dbo].[procBcpViewCreate] " +
                "@TableName, @Schema";
            SqlForMessage += "\r\n\r\n" + SQL;
            OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message);
            SQL = SqlForMessage;
            return OK;
        }

        private static bool? _IsSqlServerExpress;
        public static bool IsSqlServerExpress
        {
            get
            {
                if (_IsSqlServerExpress == null)
                {
                    string SQL = "SELECT @@VERSION";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result.IndexOf("Express ") > -1)
                        _IsSqlServerExpress = true;
                    else
                        _IsSqlServerExpress = false;
                }
                return (bool)_IsSqlServerExpress;
            }
        }

        private static string _BulkTransferDirectory;
        private static string _BulkTransferBashFile;

        public static string BulkTransferDirectory
        {
            get
            {
                if (_BulkTransferDirectory == null)
                {
                    if (IsSqlServerExpress)
                        _BulkTransferDirectory = "";
                    else
                    {
                        string SQL = "SELECT [TransferDirectory] " +
                            " FROM [dbo].[Target] T " +
                            " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                            " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                            " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "'";
                        _BulkTransferDirectory = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    }
                }
                return _BulkTransferDirectory;
            }
            set
            {
                if (IsSqlServerExpress)
                    _BulkTransferDirectory = "";
                else
                {
                    string SQL = "UPDATE T SET [TransferDirectory] = '" + value + "' " +
                        " FROM [dbo].[Target] T " +
                        " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                        " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                        " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "'";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    _BulkTransferDirectory = value;
                }
            }
        }

        public static string BulkTransferBashFile
        {
            get
            {
                if (_BulkTransferBashFile == null)
                {
                    if (IsSqlServerExpress)
                        _BulkTransferBashFile = "";
                    else
                    {
                        if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                        {
                            string SQL = "SELECT [BashFile] " +
                                " FROM [dbo].[Target] T " +
                                " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                                " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                                " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "'";
                            _BulkTransferBashFile = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                        }
                    }
                }
                return _BulkTransferBashFile;
            }
            set
            {
                if (IsSqlServerExpress)
                    _BulkTransferBashFile = "";
                else
                {
                    string SQL = "UPDATE T SET [BashFile] = '" + value + "' " +
                        " FROM [dbo].[Target] T " +
                        " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                        " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                        " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "'";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    _BulkTransferBashFile = value;
                }
            }
        }


        private static string _BulkTransferMountPoint;
        public static string BulkTransferMountPoint
        {
            get
            {
                if (_BulkTransferMountPoint == null)
                {
                    if (IsSqlServerExpress)
                        _BulkTransferMountPoint = "";
                    else
                    {
                        if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                        {
                            string SQL = "SELECT [MountPoint] " +
                                " FROM [dbo].[Target] T " +
                                " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                                " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                                " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "'";
                            _BulkTransferMountPoint = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                        }
                    }
                }
                return _BulkTransferMountPoint;
            }
            set
            {
                if (IsSqlServerExpress)
                    _BulkTransferMountPoint = "";
                else
                {
                    string SQL = "UPDATE T SET [MountPoint] = '" + value + "' " +
                        " FROM [dbo].[Target] T " +
                        " WHERE T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                        " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentPort().ToString() +
                        " AND T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "'";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    _BulkTransferMountPoint = value;
                }
            }
        }

        public static bool CanUseBulkTransfer
        {
            get
            {
                if (DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile != null &&
                    DiversityCollection.CacheDatabase.CacheDB.BulkTransferBashFile.Length > 0 &&
                    DiversityCollection.CacheDatabase.CacheDB.BulkTransferDirectory.Length > 0 &&
                    DiversityCollection.CacheDatabase.CacheDB.BulkTransferMountPoint != null &&
                    DiversityCollection.CacheDatabase.CacheDB.BulkTransferMountPoint.Length > 0)
                    return true;
                else
                    return false;
            }
        }


        #endregion

        private static System.Data.DataTable DatabaseList(string Module, string Restriction)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringMaster.Length > 0)
            {
                string SQL = "SELECT name as DatabaseName FROM sys.databases where name not in ( 'master', 'model', 'tempdb', 'msdb')";
                if (Restriction.Length > 0)
                    SQL += " AND name LIKE '" + Restriction + "'";
                SQL += " ORDER BY name";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringMaster);
                try
                {
                    ad.Fill(dt);
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                    return dt;
                }
            }
            if (dt.Rows.Count > 0)
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringMaster);
                con.Open();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("", con);
                if (Module.Length > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string SqlModule = "use " + R[0].ToString() + "; IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DiversityWorkbenchModule]') AND " +
                            "type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                            "BEGIN SELECT dbo.DiversityWorkbenchModule() END " +
                            "ELSE BEGIN SELECT NULL END";
                        try
                        {
                            C.CommandText = SqlModule;
                            if (Module != (C.ExecuteScalar()?.ToString() ?? string.Empty))
                            {
                                R.Delete();
                            }
                        }
                        catch { R.Delete(); }
                    }
                }
                con.Close();
            }
            return dt;
        }

        private static string ConnectionStringMaster
        {
            get
            {
                string conStr = "";
                if (DiversityWorkbench.Settings.DatabaseServer.Length > 0)
                {
                    conStr = "Data Source=" + DiversityWorkbench.Settings.DatabaseServer;
                    conStr += "," + DiversityWorkbench.Settings.DatabasePort;
                    conStr += ";initial catalog=master;";
                    if (DiversityWorkbench.Settings.IsTrustedConnection)
                        conStr += "Integrated Security=True";
                    else
                    {
                        if (DiversityWorkbench.Settings.DatabaseUser.Length > 0 && DiversityWorkbench.Settings.Password.Length > 0)
                            conStr += "user id=" + DiversityWorkbench.Settings.DatabaseUser + ";password=" + DiversityWorkbench.Settings.Password;
                        else conStr = "";
                    }
                }
                return conStr;
            }
        }

        #region Resetting

        public static void ResetConnection()
        {
            DiversityCollection.CacheDatabase.CacheDB._ProjectsDatabase = null;
            DiversityCollection.CacheDatabase.CacheDB._DatabaseServer = null;
            DiversityCollection.CacheDatabase.CacheDB._DatabaseName = null;
            DiversityCollection.CacheDatabase.CacheDB._DatabaseServerPort = 1433;
            DiversityCollection.CacheDatabase.CacheDB._IsSqlServerExpress = null;
            DiversityCollection.CacheDatabase.CacheDB.ResetBulkTransfer();
        }

        public static void ResetCacheDBServer()
        {
            string SQL = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CacheDB_Server]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                "DROP FUNCTION [dbo].[CacheDB_Server]";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            DiversityCollection.CacheDatabase.CacheDB._ConnectionStringCacheDB = "";
            DiversityCollection.CacheDatabase.CacheDB._DatabaseName = "";
            DiversityCollection.CacheDatabase.CacheDB._DtLogins = null;
        }

        public static void ResetCacheDB()
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try
                {
                    //Markus 30.4.2020 obsolet
                    //string SQL = "IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CacheDB_DatabaseName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                    //    "DROP FUNCTION [dbo].[CacheDB_DatabaseName]";
                    //DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    DiversityCollection.CacheDatabase.CacheDB._ConnectionStringCacheDB = "";
                    DiversityCollection.CacheDatabase.CacheDB._DatabaseName = "";
                    DiversityCollection.CacheDatabase.CacheDB._DtLogins = null;
                    DiversityCollection.CacheDatabase.CacheDB._IsSqlServerExpress = null;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        public static void ResetProjectDB()
        {
            DiversityCollection.CacheDatabase.CacheDB._ProjectsDatabase = "";
            string SQL = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CacheDB_ProjectDatabase]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT')) " +
                "DROP FUNCTION [dbo].[CacheDB_ProjectDatabase]";
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
                con.Dispose();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public static void ResetTaxonSources()
        {
            DiversityCollection.CacheDatabase.CacheDB._CacheDBTaxonSources = null;
        }
        
        #endregion

        #region Database roles

        private static System.Collections.Generic.List<string> _DatabaseRoles;

        /// <summary>
        /// Roles of the user within the current database
        /// </summary>
        /// <returns>List of the roles</returns>
        public static System.Collections.Generic.List<string> DatabaseRoles()
        {
            if (DiversityCollection.CacheDatabase.CacheDB._DatabaseRoles != null)
                return DiversityCollection.CacheDatabase.CacheDB._DatabaseRoles;
            DiversityCollection.CacheDatabase.CacheDB._DatabaseRoles = new System.Collections.Generic.List<string>();
            string SQL = "select pR.name " +
                "from sys.database_principals pR, sys.database_role_members, sys.database_principals pU  " +
                "where sys.database_role_members.role_principal_id = pR.principal_id  " +
                "and sys.database_role_members.member_principal_id = pU.principal_id " +
                "and pU.type <> 'R' " +
                "and pU.Name = User_Name()";
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                    DiversityCollection.CacheDatabase.CacheDB._DatabaseRoles.Add(R[0].ToString());
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return DiversityCollection.CacheDatabase.CacheDB._DatabaseRoles;
        }
        
        #endregion  

        #region Reporting
        
        public static bool ProcessOnly = false;
        public static bool IncludePostgres = true;
        public static bool IncludeCacheDB = true;

        /// <summary>
        /// For simulation - ignoring the scheduled time
        /// </summary>
        public static bool IsSimulation = false;

        /// <summary>
        /// Writes information to error log if parameter DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents is set to true
        /// </summary>
        /// <param name="Source">the source e.g. the function</param>
        /// <param name="Target"></param>
        /// <param name="Message"></param>
        public static void LogEvent(string Source, string Target, string Message, bool OmitUser = true, bool OmitMessagePrefix = true)
        {
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.LogEvents)
            {
                if (Source.Length > 0 && Target.Length > 0)
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(Source, Target, Message, OmitUser, OmitMessagePrefix);
                else
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(Message, OmitMessagePrefix);
            }
        }
        
        #endregion
      
        #endregion

        #region Auxillary for controls
        
        public static void initScheduleControls(
            string SourceTable, 
            string SourceView,
            int? ProjectID, 
            System.Windows.Forms.CheckBox checkBoxDoInclude, 
            System.Windows.Forms.Button buttonFilter,
            System.Windows.Forms.Button buttonSettings,
            System.Windows.Forms.Button buttonProtocol,
            System.Windows.Forms.Button buttonErrors,
            System.Windows.Forms.Button buttonTransfer,
            System.Windows.Forms.Label labelTransfer,
            System.Windows.Forms.ToolTip ToolTip)
        {
            try
            {
                string SQL = "SELECT TransferDays, TransferTime, IncludeInTransfer, TransferIsExecutedBy, CompareLogDate, TransferProtocol, TransferErrors " +
                    "FROM " + SourceTable + " AS PT WHERE 1 = 1 ";
                if (ProjectID != null)
                    SQL += " AND ProjectID = " + ProjectID.ToString();
                if (SourceTable.EndsWith("Target"))
                {
                    if (SourceTable == "ProjectTarget")
                    {
                        SQL += " AND TargetID = " + DiversityCollection.CacheDatabase.CacheDB.TargetID().ToString();
                    }
                    else
                    {
                        if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0 &&
                            DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                            SQL += " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                        else
                        {
                            buttonErrors.Enabled = false;
                            buttonProtocol.Enabled = false;
                            buttonSettings.Enabled = false;
                            checkBoxDoInclude.Enabled = false;
                            checkBoxDoInclude.Checked = false;
                            buttonFilter.Enabled = false;
                            return;
                        }
                    }
                }
                if (SourceView.Length > 0)
                    SQL += " AND SourceView = '" + SourceView + "'";
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                if (dt.Rows.Count > 0)
                {
                    // Days and time
                    string Days = dt.Rows[0]["TransferDays"].ToString();
                    string Time = dt.Rows[0]["TransferTime"].ToString();
                    Days = Days.Replace("0", "Su ");
                    Days = Days.Replace("1", "Mo ");
                    Days = Days.Replace("2", "Tu ");
                    Days = Days.Replace("3", "We ");
                    Days = Days.Replace("4", "Th ");
                    Days = Days.Replace("5", "Fr ");
                    Days = Days.Replace("6", "Sa ");
                    if (Days.Length > 0)
                        buttonSettings.Text = "        " + Days + Time;
                    else
                        buttonSettings.Text = "         NOTHING PLANNED";
                    // inclusion
                    string Include = dt.Rows[0]["IncludeInTransfer"].ToString();
                    bool DoInclude = false;
                    bool.TryParse(Include, out DoInclude);
                    checkBoxDoInclude.Enabled = true;
                    checkBoxDoInclude.Checked = DoInclude;
                    buttonSettings.Enabled = checkBoxDoInclude.Checked;
                    // Compare logdates
                    string Compare = dt.Rows[0]["CompareLogDate"].ToString();
                    bool DoCompare = false;
                    bool.TryParse(Compare, out DoCompare);
                    if (DoCompare)
                    {
                        buttonFilter.Image = DiversityCollection.Resource.Filter;
                        ToolTip.SetToolTip(buttonFilter, "Data filtered for updates later than last transfer");
                    }
                    else
                    {
                        buttonFilter.Image = DiversityCollection.Resource.FilterClear;
                        ToolTip.SetToolTip(buttonFilter, "Data not filtered for updates");
                    }
                    buttonFilter.Enabled = true;// checkBoxDoInclude.Checked;
                    // Protocol
                    if (!dt.Rows[0]["TransferProtocol"].Equals(System.DBNull.Value) && dt.Rows[0]["TransferProtocol"].ToString().Length > 0)
                        buttonProtocol.Enabled = true;
                    else buttonProtocol.Enabled = false;
                    // Errors
                    if (!dt.Rows[0]["TransferErrors"].Equals(System.DBNull.Value) && dt.Rows[0]["TransferErrors"].ToString().Length > 0)
                        buttonErrors.Enabled = true;
                    else buttonErrors.Enabled = false;
                    // Execution
                    if (!dt.Rows[0]["TransferIsExecutedBy"].Equals(System.DBNull.Value) && dt.Rows[0]["TransferIsExecutedBy"].ToString().Length > 0)
                    {
                        labelTransfer.Text = "Competing transfer active: " + dt.Rows[0]["TransferIsExecutedBy"].ToString();
                        labelTransfer.ForeColor = System.Drawing.Color.Red;
                        buttonTransfer.Enabled = false;
                        buttonTransfer.BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        //labelTransfer.Text = "";
                        labelTransfer.ForeColor = System.Drawing.SystemColors.WindowText;
                        //buttonTransfer.Enabled = true;
                        buttonTransfer.BackColor = labelTransfer.BackColor;
                    }
                }
                else
                {
                    buttonErrors.Enabled = false;
                    buttonProtocol.Enabled = false;
                    buttonSettings.Enabled = false;
                    checkBoxDoInclude.Enabled = true;
                    checkBoxDoInclude.Checked = false;
                    buttonFilter.Enabled = false;
                }
                checkBoxDoInclude.Visible = true;
                buttonSettings.Visible = true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region old version

        //public static bool SchedulerPlanedTimeReached(string SourceTable, string SourceView, int? ProjectID, string Target, ref string Error)
        //{
        //    bool PlanedTimeReached = false;
        //    string Message = "";
        //    string SQL = "";
        //    try
        //    {
        //        SQL = "SELECT LastUpdatedWhen, TransferDays, TransferTime, TransferIsExecutedBy " +
        //            "FROM " + SourceTable + " AS P WHERE IncludeInTransfer = 1 ";
        //        if (ProjectID != null)
        //            SQL += " AND ProjectID = " + ProjectID.ToString();
        //        if (SourceView.Length > 0)
        //            SQL += " AND SourceView = '" + SourceView + "'";
        //        if (Target.Length > 0)
        //            SQL += " AND Target = '" + Target + "'";
        //        System.Data.DataTable dtSource = new DataTable();
        //        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtSource, ref Message);
        //        if (dtSource.Rows.Count > 0)
        //        {
        //            if (dtSource.Rows[0]["TransferIsExecutedBy"].Equals(System.DBNull.Value) && dtSource.Rows[0]["TransferIsExecutedBy"].ToString().Length > 0)
        //            {
        //                Error = "Not transferred:\r\nCompeting transfer: " + dtSource.Rows[0]["TransferIsExecutedBy"].ToString() + "\r\n";
        //            }
        //            else
        //            {
        //                System.TimeSpan TS = new TimeSpan(7, 24, 0, 0, 0);
        //                System.DateTime DtLastUpdatedWhen = System.DateTime.Now.Subtract(TS);
        //                System.DateTime.TryParse(dtSource.Rows[0]["LastUpdatedWhen"].ToString(), out DtLastUpdatedWhen);
        //                System.Collections.Generic.List<DayOfWeek> Days = new List<DayOfWeek>();
        //                string sDays = dtSource.Rows[0]["TransferDays"].ToString();
        //                if (sDays.IndexOf("0") > -1) Days.Add(DayOfWeek.Sunday);
        //                if (sDays.IndexOf("1") > -1) Days.Add(DayOfWeek.Monday);
        //                if (sDays.IndexOf("2") > -1) Days.Add(DayOfWeek.Tuesday);
        //                if (sDays.IndexOf("3") > -1) Days.Add(DayOfWeek.Wednesday);
        //                if (sDays.IndexOf("4") > -1) Days.Add(DayOfWeek.Thursday);
        //                if (sDays.IndexOf("5") > -1) Days.Add(DayOfWeek.Friday);
        //                if (sDays.IndexOf("6") > -1) Days.Add(DayOfWeek.Saturday);
        //                if (Days.Count > 0)
        //                {
        //                    System.DateTime DtNow = System.DateTime.Now;
        //                    foreach (DayOfWeek D in Days)
        //                    {
        //                        int Hour = 0;
        //                        int.TryParse(dtSource.Rows[0]["TransferTime"].ToString().Substring(0, 2), out Hour);
        //                        int Minute = 0;
        //                        int.TryParse(dtSource.Rows[0]["TransferTime"].ToString().Substring(3, 2), out Minute);
        //                        int Second = 0;
        //                        int.TryParse(dtSource.Rows[0]["TransferTime"].ToString().Substring(6, 2), out Second);
        //                        System.DateTime NextPlannedDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, Hour, Minute, Second);
        //                        System.TimeSpan T = new TimeSpan(1, 0, 0, 0, 0);
        //                        while (NextPlannedDate.DayOfWeek != D)
        //                            NextPlannedDate = NextPlannedDate.Subtract(T);
        //                        if (NextPlannedDate > DtLastUpdatedWhen
        //                            && NextPlannedDate < System.DateTime.Now)
        //                        {
        //                            PlanedTimeReached = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
        //    }
        //    return PlanedTimeReached;
        //}

        //public static string SetTransferActive(string SourceTable, string Target, int? ProjectID, string SourceView, bool ForPostgres)
        //{
        //    string SQL = "";
        //    try
        //    {
        //        SQL = "SELECT P.TransferIsExecutedBy FROM " + SourceTable;
        //        if (Target.Length > 0 && !SourceTable.ToLower().EndsWith("target"))
        //            SQL += "Target";
        //        SQL += " P ";
        //        if (SourceView.Length > 0)
        //            SQL += " WHERE P.SourceView = '" + SourceView + "'";
        //        if (Target.Length > 0 && ForPostgres && DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
        //            SQL += " AND TargetID = ";
        //        string Result = "";
        //        try
        //        {
        //            Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
        //        }
        //        catch (System.Exception ex)
        //        {
        //        }
        //        if (Result.Length > 0)
        //            return Result;
        //        SQL = "UPDATE P SET P.TransferIsExecutedBy = " +
        //            "'" + System.DateTime.Now.Year.ToString() + "-" +
        //            System.DateTime.Now.Month.ToString() + "-" +
        //            System.DateTime.Now.Day.ToString() + " " +
        //            System.DateTime.Now.Hour.ToString() + ":" +
        //            System.DateTime.Now.Minute.ToString() + ". By: ' + SUSER_SNAME() + '. On: " + System.Windows.Forms.SystemInformation.ComputerName.ToString() + "' FROM " + SourceTable;
        //        if (Target.Length > 0 && !SourceTable.ToLower().EndsWith("target"))
        //            SQL += "Target";
        //        SQL += " P WHERE 1 = 1 ";
        //        if (SourceView.Length > 0)
        //            SQL += " AND P.SourceView = '" + SourceView + "'";
        //        if (Target.Length > 0 && ForPostgres && DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
        //            SQL += " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
        //        if (ProjectID != null)
        //            SQL += " AND ProjectID = " + ProjectID.ToString();
        //        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
        //    }
        //    return "";
        //}

        //public static void SetTransferFinished(string SourceTable, string Target, int? ProjectID, string SourceView, string Protocol, string Errors)
        //{
        //    string SQL = "";
        //    try
        //    {
        //        SQL = "UPDATE P SET P.TransferIsExecutedBy = NULL, TransferProtocol = '" + Protocol + "' ";
        //        if (Errors != null && Errors.Length > 0)
        //            SQL += ", TransferErrors = '" + Errors.Replace("'", "''") + "' ";
        //        else
        //            SQL += ", LastUpdatedWhen = getdate() ";
        //        SQL += " FROM " + SourceTable;
        //        if (Target != null && Target.Length > 0 && SourceView != null && SourceTable.Length > 0 && !SourceTable.ToLower().EndsWith("target"))
        //            SQL += "Target";
        //        SQL += " P WHERE 1 = 1 ";
        //        if (SourceView != null && SourceView.Length > 0)
        //            SQL += " AND P.SourceView = '" + SourceView + "'";
        //        if (Target != null && Target.Length > 0)
        //            SQL += " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
        //        if (ProjectID != null)
        //            SQL += " AND ProjectID = " + ProjectID.ToString();
        //        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
        //    }
        //}
        
        #endregion

        public static bool SchedulerPlanedTimeReached(string SourceTable, string SourceView, int? ProjectID, int? TargetID, ref string Error, ref string Message)
        {
            if (DiversityCollection.CacheDatabase.CacheDB.IsSimulation)
                return true;

            bool PlanedTimeReached = false;
            string SQL = "";
            try
            {
                SQL = "SELECT LastUpdatedWhen, TransferDays, TransferTime, TransferIsExecutedBy " +
                    "FROM " + SourceTable + " AS P WHERE IncludeInTransfer = 1 ";
                if (ProjectID != null)
                    SQL += " AND ProjectID = " + ProjectID.ToString();
                if (SourceView.Length > 0)
                    SQL += " AND SourceView = '" + SourceView + "'";
                if (TargetID != null)
                    SQL += " AND TargetID = " + TargetID.ToString();
                System.Data.DataTable dtSource = new DataTable();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtSource, ref Message);
                if (dtSource.Rows.Count > 0)
                {
                    if (dtSource.Rows[0]["TransferIsExecutedBy"].Equals(System.DBNull.Value) && dtSource.Rows[0]["TransferIsExecutedBy"].ToString().Length > 0)
                    {
                        Error = "Not transferred:\r\nCompeting transfer: " + dtSource.Rows[0]["TransferIsExecutedBy"].ToString() + "\r\n";
                    }
                    else
                    {
                        if (dtSource.Rows[0]["LastUpdatedWhen"].Equals(System.DBNull.Value) || dtSource.Rows[0]["LastUpdatedWhen"].ToString().Length == 0)
                            PlanedTimeReached = true;
                        else
                        {
                            System.TimeSpan TS = new TimeSpan(7, 24, 0, 0, 0);
                            System.DateTime DtLastUpdatedWhen = System.DateTime.Now.Subtract(TS);
                            System.DateTime LastCheckedDate = System.DateTime.Now;
                            System.DateTime.TryParse(dtSource.Rows[0]["LastUpdatedWhen"].ToString(), out DtLastUpdatedWhen);
                            System.Collections.Generic.List<DayOfWeek> Days = new List<DayOfWeek>();
                            string sDays = dtSource.Rows[0]["TransferDays"].ToString();
                            if (sDays.IndexOf("0") > -1) Days.Add(DayOfWeek.Sunday);
                            if (sDays.IndexOf("1") > -1) Days.Add(DayOfWeek.Monday);
                            if (sDays.IndexOf("2") > -1) Days.Add(DayOfWeek.Tuesday);
                            if (sDays.IndexOf("3") > -1) Days.Add(DayOfWeek.Wednesday);
                            if (sDays.IndexOf("4") > -1) Days.Add(DayOfWeek.Thursday);
                            if (sDays.IndexOf("5") > -1) Days.Add(DayOfWeek.Friday);
                            if (sDays.IndexOf("6") > -1) Days.Add(DayOfWeek.Saturday);
                            if (Days.Count > 0)
                            {
                                System.DateTime DtNow = System.DateTime.Now;
                                foreach (DayOfWeek D in Days)
                                {
                                    int Hour = 0;
                                    int.TryParse(dtSource.Rows[0]["TransferTime"].ToString().Substring(0, 2), out Hour);
                                    int Minute = 0;
                                    int.TryParse(dtSource.Rows[0]["TransferTime"].ToString().Substring(3, 2), out Minute);
                                    int Second = 0;
                                    int.TryParse(dtSource.Rows[0]["TransferTime"].ToString().Substring(6, 2), out Second);
                                    System.DateTime NextPlannedDate = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, Hour, Minute, Second);
                                    System.TimeSpan T = new TimeSpan(1, 0, 0, 0, 0);
                                    while (NextPlannedDate.DayOfWeek != D)
                                        NextPlannedDate = NextPlannedDate.Subtract(T);
                                    if (NextPlannedDate > DtLastUpdatedWhen
                                        && NextPlannedDate < System.DateTime.Now)
                                    {
                                        PlanedTimeReached = true;
                                        break;
                                    }
                                    LastCheckedDate = NextPlannedDate;
                                }
                            }
                            if (!PlanedTimeReached)
                            {
                                Message = "Last update: " + DtLastUpdatedWhen.ToShortDateString() + ":" + DtLastUpdatedWhen.ToShortTimeString() + ". Next planed date not reached: " + LastCheckedDate.ToShortDateString() + ":" + LastCheckedDate.ToShortTimeString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return PlanedTimeReached;
        }

        public static string SetTransferActive(string SourceTable, string Target, int? TargetID, int? ProjectID, string SourceView, bool ForPostgres)
        {
            string SQL = "";
            try
            {
                SQL = "SELECT P.TransferIsExecutedBy FROM " + SourceTable;
                if (Target.Length > 0 && !SourceTable.ToLower().EndsWith("target"))
                    SQL += "Target";
                SQL += " P ";
                if (SourceView.Length > 0)
                    SQL += " WHERE P.SourceView = '" + SourceView + "'";
                if (TargetID != null && ForPostgres && DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                {
                    // #191
                    if (SQL.IndexOf(" WHERE ") > -1)
                        SQL += " AND ";
                    else
                        SQL += " WHERE ";
                    SQL += "TargetID = " + TargetID.ToString();
                }
                string Result = "";
                try
                {
                    // Markus 28.3.25: Check existence. Issue #36
                    Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, true);
                }
                catch (System.Exception ex)
                {
                }
                if (Result.Length > 0)
                    return Result;
                SQL = "UPDATE P SET P.TransferIsExecutedBy = " +
                    "'" + System.DateTime.Now.Year.ToString() + "-" +
                    System.DateTime.Now.Month.ToString() + "-" +
                    System.DateTime.Now.Day.ToString() + " " +
                    System.DateTime.Now.Hour.ToString() + ":" +
                    System.DateTime.Now.Minute.ToString() + ". By: ' + SUSER_SNAME() + '. On: " + System.Windows.Forms.SystemInformation.ComputerName.ToString() + "' FROM " + SourceTable;
                if (Target.Length > 0 && !SourceTable.ToLower().EndsWith("target"))
                    SQL += "Target";
                SQL += " P WHERE 1 = 1 ";
                if (SourceView.Length > 0)
                    SQL += " AND P.SourceView = '" + SourceView + "'";
                //if (Target.Length > 0 && ForPostgres && DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                //    SQL += " AND Target = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                if (TargetID != null && ForPostgres && DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                    SQL += " AND TargetID = " + TargetID.ToString();
                if (ProjectID != null)
                    SQL += " AND ProjectID = " + ProjectID.ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return "";
        }

        public static string SetTransferActive(string SourceTable, string Target, string SourceView, bool ForPostgres)
        {
            string SQL = "";
            try
            {
                // Markus 3.6.24: Check existence of data in SourceTable to avoid exception
                string Result = "";
                SQL = "SELECT COUNT(*) FROM " + SourceTable;
                if (Target.Length > 0 && !SourceTable.ToLower().EndsWith("target"))
                    SQL += "Target";
                SQL += " P ";
                if (SourceView.Length > 0)
                    SQL += " WHERE P.SourceView = '" + SourceView + "'";
                Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result == "0")
                    Result = "";
                else
                {
                    SQL = "SELECT P.TransferIsExecutedBy FROM " + SourceTable;
                    if (Target.Length > 0 && !SourceTable.ToLower().EndsWith("target"))
                        SQL += "Target";
                    SQL += " P ";
                    if (SourceView.Length > 0)
                        SQL += " WHERE P.SourceView = '" + SourceView + "'";
                    try
                    {
                        Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                if (Result.Length > 0)
                    return Result;
                SQL = "UPDATE P SET P.TransferIsExecutedBy = " +
                    "'" + System.DateTime.Now.Year.ToString() + "-" +
                    System.DateTime.Now.Month.ToString() + "-" +
                    System.DateTime.Now.Day.ToString() + " " +
                    System.DateTime.Now.Hour.ToString() + ":" +
                    System.DateTime.Now.Minute.ToString() + ". By: ' + SUSER_SNAME() + '. On: " + System.Windows.Forms.SystemInformation.ComputerName.ToString() + "' FROM " + SourceTable;
                if (ForPostgres)
                    SQL += "Target";
                SQL += " P WHERE 1 = 1 ";
                if (SourceView.Length > 0)
                    SQL += " AND P.SourceView = '" + SourceView + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return "";
        }

        public static void SetTransferFinished(string SourceTable, string Target, int? TargetID, int? ProjectID, string SourceView, string Protocol, string Errors, bool DataHaveBeenTransferred = true)
        {
            string SQL = "";
            try
            {
                string CurrentDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString()
                    + " " + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + ":" + System.DateTime.Now.Second.ToString();
                SQL = "UPDATE P SET P.TransferIsExecutedBy = NULL, TransferProtocol = '" + Protocol.Replace("'", "''") + "' ";
                if (Errors != null && Errors.Length > 0)
                    SQL += ", TransferErrors = '" + CurrentDate + "\r\n" + Errors.Replace("'", "''") + "' ";
                else if (DataHaveBeenTransferred)
                    SQL += ", TransferErrors = '', LastUpdatedWhen = getdate() ";
                SQL += " FROM " + SourceTable;
                if (Target != null && Target.Length > 0 && SourceView != null && SourceTable.Length > 0 && !SourceTable.ToLower().EndsWith("target"))
                    SQL += "Target";
                SQL += " P WHERE 1 = 1 ";
                if (SourceView != null && SourceView.Length > 0)
                    SQL += " AND P.SourceView = '" + SourceView + "'";
                if (TargetID != null)
                    SQL += " AND TargetID = " + TargetID.ToString();
                if (ProjectID != null)
                    SQL += " AND ProjectID = " + ProjectID.ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        public static void SetTransferFinished(string SourceTable, bool ForPostgres, string SourceView, string Protocol, string Errors)
        {
            string SQL = "";
            try
            {
                string CurrentDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString()
                    + " " + System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString() + ":" + System.DateTime.Now.Second.ToString();
                SQL = "UPDATE P SET P.TransferIsExecutedBy = NULL, TransferProtocol = '" + Protocol + "' ";
                if (Errors != null && Errors.Length > 0)
                    SQL += ", TransferErrors = '" + CurrentDate + "\r\n" + Errors.Replace("'", "''") + "' ";
                else
                    SQL += ", TransferErrors = '', LastUpdatedWhen = getdate() ";
                SQL += " FROM " + SourceTable;
                if (ForPostgres)
                    SQL += "Target";
                SQL += " P WHERE 1 = 1 ";
                if (SourceView != null && SourceView.Length > 0)
                    SQL += " AND P.SourceView = '" + SourceView + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        public static int TargetID()
        {
            int TargetID = 0;
            try
            {
                string SQL = "SELECT COUNT(*) FROM [Target] T " +
                    " WHERE T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
                    " AND T.Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() +
                    " AND T.DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result == "0")
                {
                    SQL = "INSERT INTO [Target] (Server, Port, DatabaseName) " +
                        " VALUES ('" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "', " +
                        DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() + ", " +
                        " '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "')";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    SQL = "CREATE OR REPLACE VIEW public.\"LinkedServerInfo\" AS " +
                        "SELECT '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "'::character varying(50) AS \"Server\", " +
                        DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString() + " AS \"Port\", " +
                        "'" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "'::character varying(50) AS \"DatabaseName\"; " +
                        "ALTER TABLE public.\"LinkedServerInfo\" OWNER TO \"CacheAdmin\";";
                    DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                }
                SQL = "SELECT TargetID FROM Target T " +
                    "WHERE T.Server = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name + "' " +
                    "AND DatabaseName = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                    "AND Port = " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Port.ToString();
                // Markus 28.3.25: Check existence. Issue #36
                int.TryParse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL, true).ToString(), out TargetID);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return TargetID;
        }

        #endregion

        #region History
        
        public enum HistoryTarget { DataToCache, CacheToPostgres, Package }

        public static void WriteProjectTransferHistory(HistoryTarget Target, int ProjectID, string Schema, System.Collections.Generic.Dictionary<string, object> DataTransfer, int? TargetID = null, string Package = "")
        {
            if (Schema.Length == 0)
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("CacheDB", "WriteProjectTransferHistory", "Schema is missing: Target = " + Target.ToString());
            else
            {
                try
                {
                    int UserID = -1;
                    int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT dbo.UserID()"), out UserID);
                    string SQL = "INSERT INTO ProjectTransfer (ProjectID, ResponsibleUserID, TargetID, Settings";
                    if (Package.Length > 0)
                        SQL += ", Package";
                    SQL += ") VALUES (" + ProjectID.ToString() + ", " + UserID.ToString() + ", ";
                    if (TargetID == null)
                        SQL += "NULL";
                    else
                        SQL += TargetID.ToString();
                    SQL += ", '" + SettingsForHistory(Target, Schema, DataTransfer, ProjectID, TargetID) + "' ";
                    if (Package.Length > 0)
                        SQL += ", '" + Package + "'";
                    SQL += ")";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        public static string SettingsForHistory(HistoryTarget Target, string Schema, System.Collections.Generic.Dictionary<string, object> DataTransfer, int ProjectID = 0, int? TargetID = null)
        {
            string History = "";
            try
            {
                string SQL = "";
                // Dictionary for all infos
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<object>> D_History = new Dictionary<string, System.Collections.Generic.List<object>>();

                // Versions
                System.Collections.Generic.List<object> OVers = new List<object>();
                OVers.Add(HistoryVersions(Target, Schema));
                D_History.Add("Versions", OVers);

                if (Target == HistoryTarget.DataToCache)
                {
                    // Settings
                    System.Collections.Generic.Dictionary<string, int> D_Pre = new Dictionary<string, int>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Loc = new Dictionary<string, System.Collections.Generic.List<string>>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Tax = new Dictionary<string, System.Collections.Generic.List<string>>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Mat = new Dictionary<string, System.Collections.Generic.List<string>>();
                    System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> D_Ana = new Dictionary<string, System.Collections.Generic.List<string>>();

                    // CoordinatePrecision
                    SQL = "SELECT CoordinatePrecision FROM ProjectPublished WHERE ProjectID = " + ProjectID.ToString();
                    string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    int Pre;
                    if (int.TryParse(Result, out Pre))
                    {
                        D_Pre.Add("CoordinatePrecision", Pre);
                    }

                    // LocalisationSystemID
                    SQL = "SELECT S.DisplayText + ' [ID: ' + CAST (S.LocalisationSystemID AS varchar) + ']' FROM " + Schema + ".CacheLocalisationSystem AS L, " + DiversityWorkbench.Settings.DatabaseName + ".dbo.LocalisationSystem S " +
                        "WHERE L.LocalisationSystemID = S.LocalisationSystemID ORDER BY Sequence";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<string> Loc = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                            Loc.Add(R[0].ToString());
                        D_Loc.Add("LocalisationSystems", Loc);
                    }

                    // TaxonomicGroups
                    SQL = "SELECT TaxonomicGroup FROM " + Schema + ".ProjectTaxonomicGroup ORDER BY TaxonomicGroup";
                    dt = new DataTable();
                    Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<string> L = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                            L.Add(R[0].ToString());
                        D_Tax.Add("TaxonomicGroups", L);
                    }

                    // ProjectMaterialCategories
                    SQL = "SELECT MaterialCategory FROM " + Schema + ".ProjectMaterialCategory ORDER BY MaterialCategory";
                    dt = new DataTable();
                    Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<string> L = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                            L.Add(R[0].ToString());
                        D_Mat.Add("MaterialCategories", L);
                    }

                    // Analysis
                    SQL = "SELECT A.DisplayText + ' [ID: ' + CAST(A.AnalysisID as varchar) + ']' FROM " + Schema + ".ProjectAnalysis P, " + DiversityWorkbench.Settings.DatabaseName + ".dbo.Analysis A " +
                        "WHERE A.AnalysisID = P.AnalysisID ORDER BY A.DisplayText";
                    dt = new System.Data.DataTable();
                    Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        System.Collections.Generic.List<string> L = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                            L.Add(R[0].ToString());
                        D_Ana.Add("AnalysisIDs", L);
                    }

                    System.Collections.Generic.List<object> OO_settings = new List<object>();
                    if (D_Pre.Count > 0)
                        OO_settings.Add(D_Pre);
                    if (D_Ana.Count > 0)
                        OO_settings.Add(D_Ana);
                    if (D_Loc.Count > 0)
                        OO_settings.Add(D_Loc);
                    if (D_Mat.Count > 0)
                        OO_settings.Add(D_Mat);
                    if (D_Tax.Count > 0)
                        OO_settings.Add(D_Tax);
                    D_History.Add("Settings", OO_settings);

                }
                else
                {
                    System.Collections.Generic.List<object> OO_Packs = new List<object>();

                    // the dictionary for the packages including the add ons
                    System.Collections.Generic.Dictionary<string, string> D_Package = new Dictionary<string, string>();

                    System.Data.DataTable dt = new System.Data.DataTable();
                    string Message = "";
                    SQL = "SELECT \"Package\", \"Version\" " +
                        "FROM \"" + Schema + "\".\"Package\" ORDER BY \"Package\";";
                    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            D_Package.Add(R[0].ToString(), "Vers.: " + R[1].ToString());
                            SQL = "SELECT \"AddOn\", \"Version\" " +
                                " FROM \"" + Schema + "\".\"PackageAddOn\" " +
                                " WHERE \"Package\" = '" + R[0].ToString() + "';";
                            dt = new System.Data.DataTable();
                            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (System.Data.DataRow Ra in dt.Rows)
                                {
                                    //System.Collections.Generic.Dictionary<string, string> D_AddOn = new Dictionary<string, string>();
                                    D_Package.Add(Ra[0].ToString(), "Vers.: " + Ra[1].ToString());
                                }
                            }
                        }
                        OO_Packs.Add(D_Package);
                    }
                    if (OO_Packs.Count > 0)
                    {
                        D_History.Add("Packages", OO_Packs);
                    }

                }
                // Data
                System.Collections.Generic.List<object> L_Data = new List<object>();
                L_Data.Add(DataTransfer);
                D_History.Add("Data", L_Data);

                History = System.Text.Json.JsonSerializer.Serialize(D_History);
            }
            catch (System.Exception ex)
            {
            }
            return History;
        }

        public static System.Collections.Generic.Dictionary<string, string> HistoryVersions(HistoryTarget Target, string Schema, string Package = "")
        {
            System.Collections.Generic.Dictionary<string, string> D_Ver = new Dictionary<string, string>();
            // CacheDB
            string SQL = "SELECT dbo.Version()";
            string V = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            D_Ver.Add("CacheDB", V);
            // Project
            SQL = "select [" + Schema + "].version()";
            V = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            D_Ver.Add("Project", V);

            if (Target == HistoryTarget.DataToCache)
            {
                // DB
                SQL = "SELECT dbo.Version()";
                V = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                D_Ver.Add("Database", V);
            }
            else 
            {
                // Postgres DB
                SQL = "SELECT public.version()";
                V = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                D_Ver.Add("PostgresDB", V);
                // Postgres Project
                SQL = "SELECT \"" + Schema + "\".version()";
                V = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                D_Ver.Add("Postgres Schema", V);
                if (Target == HistoryTarget.Package && Package.Length > 0)
                {
                    // Package
                    string Message = "";
                    SQL = "SELECT \"Version\" " +
                        "FROM \"" + Schema + "\".\"Package\" WHERE \"Package\" = '" + Package + "'";
                    V = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
                    D_Ver.Add("Package", V);
                    // AddOns
                    SQL = "SELECT \"AddOn\", \"Version\" " +
                        " FROM \"" + Schema + "\".\"PackageAddOn\" " +
                        " WHERE \"Package\" = '" + Package + "';";
                    Message = "";
                    System.Data.DataTable dtAddOn = new DataTable();
                    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtAddOn, ref Message);
                    if (dtAddOn.Rows.Count > 0)
                    {
                        System.Collections.Generic.Dictionary<string, int> DAddOn = new Dictionary<string, int>();
                        foreach (System.Data.DataRow R in dtAddOn.Rows)
                        {
                            D_Ver.Add(R["AddOn"].ToString(), R["Version"].ToString());
                        }
                    }
                }
            }
            return D_Ver;
        }

        #endregion

    }

}
