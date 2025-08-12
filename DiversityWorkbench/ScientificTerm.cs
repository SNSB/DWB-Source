using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class ScientificTerm : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Parameter
        private System.Collections.Generic.Dictionary<string, string> _ServiceList;
        #endregion

        #region Construction

        public ScientificTerm(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        #endregion

        #region Interface

        #region Services etc.

        public override string ServiceName() { return "DiversityScientificTerms"; }

        public override System.Collections.Generic.Dictionary<string, string> AdditionalServicesOfModule()
        {
            if (this._ServiceList == null)
            {
                this._ServiceList = new Dictionary<string, string>();
                _ServiceList.Add("CacheDB", "Cache database");
                /// Markus 18.1.2018 - TODO: Wieder einbauen sobald es funktioniert: Marker = #MW20180118
//#if DEBUG
//                _ServiceList.Add("Litholex", "Litholex");
//#endif
//                _ServiceList.Add("gfbioRecordbasis", "gfbio Recordbasis");
//                _ServiceList.Add("gfbioEnvironmentOntology", "gfbio Environment Ontology");
//                _ServiceList.Add("gfbioKingdom", "gfbio Kingdom");
            }
            return _ServiceList;
        }

        // Ariane exclude Webserice.. for .NET 8
        //        protected override System.Collections.Generic.Dictionary<string, string> AdditionalServiceURIsOfModule()
        //        {
        //            System.Collections.Generic.Dictionary<string, string> _Add = new Dictionary<string, string>();
        //            _Add.Add("gfbioRecordbasis", DiversityWorkbench.WebserviceGfbioRecordBasis.UriBaseWeb);
        //            _Add.Add("gfbioEnvironmentOntology", DiversityWorkbench.WebserviceGfbioEnvironmentOntology.UriBaseWeb);
        //            _Add.Add("gfbioKingdom", DiversityWorkbench.WebserviceGfbioKingdom.UriBaseWeb);
        //            /// Markus 18.1.2018 - TODO: Wieder einbauen sobald es funktioniert: Marker = #MW20180118
        //#if DEBUG
        //            _Add.Add("Litholex", DiversityWorkbench.WebserviceLitholex.UriBaseWeb);
        //#endif
        //            return _Add;
        //        }

        public override System.Collections.Generic.List<DiversityWorkbench.DatabaseService> DatabaseServices()
        {
            System.Collections.Generic.List<DiversityWorkbench.DatabaseService> ds = new List<DatabaseService>();
            string Test = "";
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
                {
                    if (KV.Value.ConnectionIsValid && KV.Value.ModuleName == "DiversityScientificTerms")
                    {
                        string SQLDS = "SELECT DisplayText, TerminologyID FROM Terminology ORDER BY DisplayText";
                        if (KV.Value.LinkedServer.Length > 0)
                            SQLDS = "SELECT DisplayText, TerminologyID FROM [" + KV.Value.LinkedServer + "]." + KV.Value.DatabaseName + ".dbo.Terminology ORDER BY DisplayText";
                        Microsoft.Data.SqlClient.SqlDataAdapter adDS = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLDS, KV.Value.ConnectionString);
                        Test = KV.Value.ConnectionString;
                        System.Data.DataTable dtDS = new System.Data.DataTable();
                        adDS.Fill(dtDS);
                        foreach (System.Data.DataRow R in dtDS.Rows)
                        {
                            DiversityWorkbench.DatabaseService DS;
                            if (KV.Value.LinkedServer.Length > 0)
                                DS = new DatabaseService(KV.Value.DatabaseName, KV.Value.LinkedServer);
                            else
                                DS = new DatabaseService(KV.Value.DatabaseName);
                            DS.IsWebservice = false;
                            DS.IsListInDatabase = true;
                            DS.ListName = R["DisplayText"].ToString();
                            DS.RestrictionForListInDatabase = "TerminologyID = " + R["TerminologyID"].ToString();
                            // Check for any dependency
                            try
                            {
                                //string Prefix = "";
                                //if (DS.LinkedServer != null && DS.LinkedServer.Length > 0)
                                //    Prefix = "[" + DS.LinkedServer + "]." + DS.DatabaseName + ".dbo.";
                                string SQL = "SELECT COUNT(*) FROM " + KV.Value.Prefix().Replace(".dbo.", ".") + "INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_NAME = 'TermDependency'";
                                string SqlDependency = "SELECT COUNT(*) FROM " + KV.Value.Prefix() + "TermDependency D WHERE D.TerminologyID = " + R["TerminologyID"].ToString();
                                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(KV.Value.ConnectionString);
                                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                                con.Open();
                                string Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                if (Result == "1")
                                {
                                    C.CommandText = SqlDependency;
                                    Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                                }
                                con.Close();
                                if (Result.Length > 0 && Result != "0")
                                {
                                    DS.RestrictionForListInDatabase += " and T.TermID in (" +
                                        "select T.TermID from Term T " +
                                        "where(T.RankingTermID IS NULL AND T.IsRankingTerm <> 1) OR T.RankingTermID in ( " +
                                        "SELECT T.TermID from Term T inner join TermRepresentation r on T.TermID = R.TermID and T.TerminologyID = " + R["TerminologyID"].ToString() +
                                        " WHERE NOT EXISTS(SELECT * FROM TermDependency D WHERE D.TermID = T.TermID AND T.TerminologyID = D.TerminologyID) " +
                                        "AND T.IsRankingTerm = 1 and T.TerminologyID = " + R["TerminologyID"].ToString() + "))";
                                }
                            }
                            catch (System.Exception ex)
                            {
                                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                            }

                            if (KV.Value.DatabaseServer != DiversityWorkbench.Settings.DatabaseServer)
                                DS.Server = KV.Value.DatabaseServer;
                            ds.Add(DS);
                        }


                        /// Markus 18.1.2018 - TODO: Wieder einbauen sobald es funktioniert: Marker = #MW20180118
                        //DiversityWorkbench.DatabaseService d = new DatabaseService(KV.Key);
                        //d.IsWebservice = false;
                        //if (KV.Value.DatabaseServer != DiversityWorkbench.Settings.DatabaseServer)
                        //{
                        //    d.Server = KV.Value.DatabaseServer;
                        //}
                        //ds.Add(d);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            // Ariane exclude Webserice.. for .NET 8
//            DiversityWorkbench.DatabaseService gRB = new DatabaseService("gfbioRecordbasis");
//            gRB.IsWebservice = true;
//            gRB.DisplayUnitdataInTreeview = true;
//            gRB.WebService = Webservice.WebServices.gfbioRecordbasis;
//            ds.Add(gRB);

//            DiversityWorkbench.DatabaseService gEO = new DatabaseService("gfbioEnvironmentOntology");
//            gEO.IsWebservice = true;
//            gEO.DisplayUnitdataInTreeview = true;
//            gEO.WebService = Webservice.WebServices.gfbioEnvironmentOntology;
//            ds.Add(gEO);

//            DiversityWorkbench.DatabaseService gK = new DatabaseService("gfbioKingdom");
//            gK.IsWebservice = true;
//            gK.DisplayUnitdataInTreeview = true;
//            gK.WebService = Webservice.WebServices.gfbioKingdom;
//            ds.Add(gK);
//            /// Markus 18.1.2018 - TODO: Wieder einbauen sobald es funktioniert: Marker = #MW20180118
//#if DEBUG
//            DiversityWorkbench.DatabaseService LL = new DatabaseService("Litholex");
//            LL.IsWebservice = true;
//            LL.DisplayUnitdataInTreeview = true;
//            LL.WebService = Webservice.WebServices.Litholex;
//            ds.Add(LL);
//#endif
            return ds;
        }

        #endregion

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            switch (Domain)
            {
                case "Terminology":
                    Values = TerminologyValues(ID);
                    break;
                default:
                    Values = UnitValues(ID);
                    break;
            }
            return Values;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "";

                // Main data
                SQL = "SELECT TOP 1 U.BaseURL + CAST(R.RepresentationID AS VARCHAR) AS _URI, U.BaseURL + CAST(R.RepresentationID AS VARCHAR) AS Link, " +
                    "R.DisplayText AS _DisplayText, " +
                    "R.DisplayText AS Name, R.Description, R.ExternalID, " +
                    //"CASE WHEN R.HierarchyCache IS NULL OR R.HierarchyCache = '' THEN R.DisplayText ELSE R.HierarchyCache END AS DisplayTextHierarchy, " +
                    //"R.HierarchyCache, R.HierarchyCache AS HierarchyCacheUp, R.HierarchyCacheDown, " +
                    "CASE WHEN R.HierarchyCache IS NULL OR R.HierarchyCache = '' THEN R.DisplayText ELSE R.HierarchyCache END AS Hierarchy, " +
                    "R.HierarchyCacheDown AS [Hierarchy Top-Down], " +
                    "R.Notes, R.LanguageCode " +
                    ", RR.DisplayText AS RankingTerm " +
                    "FROM " + Prefix + "TermRepresentation AS R " +
                    "INNER JOIN " + Prefix + "Term T ON T.TermID = R.TermID AND R.TerminologyID = T.TerminologyID " +
                    "LEFT OUTER JOIN " + Prefix + "Term AS TR ON TR.TermID = T.RankingTermID AND TR.TerminologyID = T.TerminologyID " +
                    "LEFT OUTER JOIN " + Prefix + "TermRepresentation AS RR ON TR.TermID = RR.TermID AND TR.PreferredRepresentationID = RR.RepresentationID AND TR.TerminologyID = RR.TerminologyID " +
                    ", " + Prefix + "ViewBaseURL AS U " +
                    "WHERE R.RepresentationID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT S.DisplayText + ' (' + S.LanguageCode + ')' AS [Synonym] " +
                    "FROM  " + Prefix + "TermRepresentation AS R " +
                    "INNER JOIN " + Prefix + "TermRepresentation AS S ON R.RepresentationID <> S.RepresentationID AND R.TermID = S.TermID AND R.TerminologyID = S.TerminologyID " +
                    "WHERE R.RepresentationID = " + ID.ToString() +
                    "GROUP BY S.LanguageCode, S.DisplayText";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT P.DisplayText + ' (' + P.LanguageCode + ')' AS [Preferred name] " +
                    "FROM  " + Prefix + "TermRepresentation AS R " +
                    "INNER JOIN " + Prefix + "TermRepresentation AS P ON R.TermID = P.TermID AND R.TerminologyID = P.TerminologyID " +
                    "INNER JOIN " + Prefix + "Term AS T ON T.PreferredRepresentationID = P.RepresentationID AND R.TermID = T.TermID AND P.TerminologyID = T.TerminologyID " +
                    "WHERE R.RepresentationID = " + ID.ToString() +
                    "GROUP BY P.LanguageCode, P.DisplayText";
                this.getDataFromTable(SQL, ref Values);

                System.Data.DataTable dtProperties = new System.Data.DataTable();
                if (this._DatabaseServiceRestriction.Length > 0 && false)
                {
                    SQL = "SELECT Property, DisplayText FROM " + Prefix + "TerminologyProperty WHERE " + this._DatabaseServiceRestriction;

                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                    ad.Fill(dtProperties);
                    if (dtProperties.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dtProperties.Rows)
                        {
                            if (this.ServerConnection.LinkedServer.Length == 0)
                            {
                                SQL = "SELECT CASE WHEN TermProperty.TextValue IS NULL THEN CASE WHEN TermProperty.NumericValue IS NULL " +
                                    "THEN CAST(TermProperty.DateValue AS varchar) ELSE CAST(TermProperty.NumericValue AS varchar)  END ELSE TermProperty.TextValue END " +
                                    "AS '" + R["DisplayText"] + "' " +
                                    "FROM TermProperty RIGHT OUTER JOIN " +
                                    "TermRepresentation ON TermProperty.TerminologyID = TermRepresentation.TerminologyID AND " +
                                    "TermProperty.TermID = TermRepresentation.TermID " +
                                    "WHERE TermRepresentation.RepresentationID = " + ID.ToString() + " AND TermProperty.Property ='" + R["DisplayText"] + "' ";
                            }
                            else
                            {
                                SQL = "SELECT CASE WHEN P.TextValue IS NULL THEN CASE WHEN P.NumericValue IS NULL " +
                                    "THEN CAST(P.DateValue AS varchar) ELSE CAST(P.NumericValue AS varchar)  END ELSE P.TextValue END " +
                                    "AS '" + R["DisplayText"] + "' " +
                                    "FROM " + Prefix + "TermProperty AS P RIGHT OUTER JOIN " +
                                    Prefix + "TermRepresentation AS R " +
                                    "ON P.TerminologyID = R.TerminologyID AND " +
                                    "P.TermID = R.TermID " +
                                    "WHERE R.RepresentationID = " + ID.ToString() + " AND P.Property ='" + R["DisplayText"] + "' ";
                            }
                            this.getDataFromTable(SQL, ref Values);
                        }
                    }
                }
                else
                {
                    SQL = "SELECT DISTINCT  TP.Property " +
                        "FROM " + Prefix + "TerminologyProperty AS TP INNER JOIN " +
                        "" + Prefix + "TermProperty AS P ON TP.TerminologyID = P.TerminologyID AND TP.Property = P.Property INNER JOIN " +
                        "" + Prefix + "TermRepresentation AS R ON P.TerminologyID = R.TerminologyID AND P.TermID = R.TermID ";
                    if (ID > -1)
                        SQL += "WHERE (R.RepresentationID = " + ID.ToString() + ")";
                    //SELECT Property, DisplayText FROM " + Prefix + "TerminologyProperty WHERE " + this._DatabaseServiceRestriction;
                    //this.getDataFromTable(SQL, ref Values);

                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                    ad.Fill(dtProperties);
                    if (dtProperties.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow R in dtProperties.Rows)
                        {
                            if (ID == -1)
                                SQL = "SELECT '" + R[0].ToString() + "' AS [" + R[0].ToString() + "], CASE WHEN TP.Datatype = 'text' THEN P.TextValue ELSE " +
                                    "CASE WHEN TP.Datatype = 'numeric' THEN cast(P.NumericValue as varchar) ELSE " +
                                    "CASE WHEN TP.Datatype = 'date' THEN cast(P.DateValue as varchar) ELSE NULL END END END AS Value " +
                                    "FROM " + Prefix + "TerminologyProperty AS TP INNER JOIN " +
                                    "" + Prefix + "TermProperty AS P ON TP.TerminologyID = P.TerminologyID AND TP.Property = P.Property INNER JOIN " +
                                    "" + Prefix + "TermRepresentation AS R ON P.TerminologyID = R.TerminologyID AND P.TermID = R.TermID " +
                                    "WHERE (R.RepresentationID = " + ID.ToString() + ") AND TP.Property = '" + R[0].ToString() + "'";
                            else
                                SQL = "SELECT CASE WHEN TP.Datatype = 'text' THEN P.TextValue ELSE " +
                                    "CASE WHEN TP.Datatype = 'numeric' THEN cast(P.NumericValue as varchar) ELSE " +
                                    "CASE WHEN TP.Datatype = 'date' THEN cast(P.DateValue as varchar) ELSE NULL END END END AS [" + R[0].ToString() + "] " +
                                    "FROM " + Prefix + "TerminologyProperty AS TP INNER JOIN " +
                                    "" + Prefix + "TermProperty AS P ON TP.TerminologyID = P.TerminologyID AND TP.Property = P.Property INNER JOIN " +
                                    "" + Prefix + "TermRepresentation AS R ON P.TerminologyID = R.TerminologyID AND P.TermID = R.TermID " +
                                    "WHERE (R.RepresentationID = " + ID.ToString() + ") AND TP.Property = '" + R[0].ToString() + "'";
                            this.getDataFromTable(SQL, ref Values);
                        }
                    }
                }

                if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
                this._UnitValues.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                {
                    this._UnitValues.Add(P.Key, P.Value);
                }
            }
            return Values;
        }

        public string MainTable() { return "TermRepresentation"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[9];
            QueryDisplayColumns[0].DisplayColumn = "DisplayText";

            QueryDisplayColumns[1].DisplayText = "Hierarchy up";
            QueryDisplayColumns[1].DisplayColumn = "HierarchyCache";

            QueryDisplayColumns[2].DisplayText = "Hierarchy down";
            QueryDisplayColumns[2].DisplayColumn = "HierarchyCacheDown";

            QueryDisplayColumns[3].DisplayColumn = "Ranking";
            QueryDisplayColumns[3].TableName = "View_TermRanking";

            QueryDisplayColumns[4].DisplayText = "Ranking Hierarchy";
            QueryDisplayColumns[4].DisplayColumn = "RankingHierarchy";
            QueryDisplayColumns[4].TableName = "View_TermRankingHierarchy";

            QueryDisplayColumns[5].DisplayText = "Property";
            QueryDisplayColumns[5].DisplayColumn = "PropertyValue";
            QueryDisplayColumns[5].TableName = "View_TermProperty";

            QueryDisplayColumns[6].DisplayText = "Reference";
            QueryDisplayColumns[6].DisplayColumn = "Reference";
            QueryDisplayColumns[6].TableName = "View_TermReference";

            QueryDisplayColumns[7].DisplayText = "Resource";
            QueryDisplayColumns[7].DisplayColumn = "URI";
            QueryDisplayColumns[7].TableName = "View_TermResource";

            QueryDisplayColumns[8].DisplayText = "Section";
            QueryDisplayColumns[8].DisplayColumn = "Section";
            QueryDisplayColumns[8].TableName = "View_SectionTerm";

            for (int i = 0; i < QueryDisplayColumns.Length; i++)
            {
                if (QueryDisplayColumns[i].DisplayText == null)
                    QueryDisplayColumns[i].DisplayText = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].OrderColumn == null)
                    QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].IdentityColumn == null)
                    QueryDisplayColumns[i].IdentityColumn = "RepresentationID";
                if (QueryDisplayColumns[i].TableName == null)
                    QueryDisplayColumns[i].TableName = "TermRepresentation";
                if (QueryDisplayColumns[i].TipText == null)
                    QueryDisplayColumns[i].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(QueryDisplayColumns[i].TableName, QueryDisplayColumns[i].DisplayColumn);
                QueryDisplayColumns[i].Module = "DiversityScientificTerms";
            }

            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            string Database = "DiversityScientificTerms";
            try
            {
                Database = this._ServerConnection.DatabaseName;// DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnection.DatabaseName;
            }
            catch { }

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            #region Representation

            string Description = "Scientific term within a certain terminology";
            DiversityWorkbench.QueryCondition qTermRepresentation = new DiversityWorkbench.QueryCondition(true, "TermRepresentation", "RepresentationID", "DisplayText", "Term", "Term", "Scientific term", Description);
            QueryConditions.Add(qTermRepresentation);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "LanguageCode");
            DiversityWorkbench.QueryCondition qLanguageCode = new DiversityWorkbench.QueryCondition(true, "TermRepresentation", "RepresentationID", "LanguageCode", "Term", "Language", "2-letter ISO code of the language", Description, "LanguageCode_Enum", this._ServerConnection);
            QueryConditions.Add(qLanguageCode);

            System.Data.DataTable dtUser = new System.Data.DataTable();
            string SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT LoginName, CombinedNameCache " +
                "FROM " + Prefix + "ViewUserProxy " +
                "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter aUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aUser.Fill(dtUser); }
                catch (System.Exception ex) { }
            }

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "RepresentationID");
            DiversityWorkbench.QueryCondition qRepresentationID = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "RepresentationID", "Term", "ID", "Representation ID", Description);
            QueryConditions.Add(qRepresentationID);

            Description = DiversityWorkbench.Functions.ColumnDescription("Term", "TermID");
            DiversityWorkbench.QueryCondition qTermID = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "TermID", "Term", "TermID", "Term ID", Description);
            QueryConditions.Add(qTermID);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "ExternalID");
            DiversityWorkbench.QueryCondition qExternalID = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "ExternalID", "Term", "External ID", "Ext.ID", Description);
            QueryConditions.Add(qExternalID);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "Notes");
            DiversityWorkbench.QueryCondition qTermNotes = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "Notes", "Term", "Notes", "Notes", Description);
            QueryConditions.Add(qTermNotes);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "DisplayImageUri");
            DiversityWorkbench.QueryCondition qTermDisplayImageUri = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "DisplayImageUri", "Term", "Image", "Image", Description);
            QueryConditions.Add(qTermDisplayImageUri);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "DisplayOrder");
            DiversityWorkbench.QueryCondition qTermDisplayOrder = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "DisplayOrder", "Term", "Order", "Order", Description);
            QueryConditions.Add(qTermDisplayOrder);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "DisplayARGB");
            DiversityWorkbench.QueryCondition qTermDisplayARGB = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "DisplayARGB", "Term", "Color", "Color", Description);
            QueryConditions.Add(qTermDisplayARGB);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "DisplayInheritARGB");
            DiversityWorkbench.QueryCondition qTermDisplayInheritARGB = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "DisplayInheritARGB", "Term", "Inh.color", "Inherit color", Description);
            QueryConditions.Add(qTermDisplayInheritARGB);

            System.Data.DataTable dtTerminology = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS DisplayText UNION SELECT  T.TerminologyID, T.DisplayText " +
                "FROM " + Prefix + "Terminology AS T ORDER BY DisplayText";
            Microsoft.Data.SqlClient.SqlDataAdapter aTerminology = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aTerminology.Fill(dtTerminology); }
                catch (System.Exception ex) { }
            }
            if (this._DatabaseServiceRestriction != null && this._DatabaseServiceRestriction.IndexOf("TerminologyID = ") > -1)
            {
            }
            else if (dtTerminology.Rows.Count > 0)
            {
                Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "TerminologyID");
                DiversityWorkbench.QueryCondition qTerminology = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "TerminologyID", "TerminologyID", "Term", "Terminology", "Terminology", Description, dtTerminology, true);
                QueryConditions.Add(qTerminology);
            }

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "LogInsertedBy");
            DiversityWorkbench.QueryCondition qLogInsertedBy = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "LogInsertedBy", "Term", "Creat. by", "The user that created the dataset", Description, dtUser, false);
            QueryConditions.Add(qLogInsertedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "LogInsertedWhen");
            DiversityWorkbench.QueryCondition qLogInsertedWhen = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "LogInsertedWhen", "Term", "Creat. date", "The date when the dataset was created", Description, true);
            QueryConditions.Add(qLogInsertedWhen);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "LogUpdatedBy");
            DiversityWorkbench.QueryCondition qLogUpdatedBy = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "LogUpdatedBy", "Term", "Changed by", "The last user that changed the dataset", Description, dtUser, false);
            QueryConditions.Add(qLogUpdatedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermRepresentation", "LogUpdatedWhen");
            DiversityWorkbench.QueryCondition qLogUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "TermRepresentation", "RepresentationID", "LogUpdatedWhen", "Term", "Changed at", "The last date when the dataset was changed", Description, true);
            QueryConditions.Add(qLogUpdatedWhen);

            #endregion

            #region Term

            System.Data.DataTable dtRanking = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT  R.TermID, R.DisplayText AS Display " +
                "FROM " + Prefix + "TermRepresentation AS R INNER JOIN " +
                Prefix + "Term AS T ON R.TerminologyID = T.TerminologyID AND R.TermID = T.TermID " +
                "WHERE (T.IsRankingTerm = 1) ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter aRanking = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aRanking.Fill(dtRanking); }
                catch { }
            }

            SQL = " FROM " + Prefix + "TermRepresentation INNER JOIN " + Prefix + "Term T ON TermRepresentation.TermID = T.TermID ";

            Description = DiversityWorkbench.Functions.ColumnDescription("Term", "RankingTermID");
            DiversityWorkbench.QueryCondition qRanking = new DiversityWorkbench.QueryCondition(true, "Term", "RepresentationID", true, SQL, "RankingTermID", "Term", "Ranking", "Terms of a certain ranking", Description, dtRanking, false);
            QueryConditions.Add(qRanking);

            SQL = " FROM " + Prefix + "TermRepresentation INNER JOIN " + Prefix + "Term T ON TermRepresentation.TermID = T.TermID ";

            Description = DiversityWorkbench.Functions.ColumnDescription("Term", "IsRankingTerm");
            DiversityWorkbench.QueryCondition qIsRankingTerm = new DiversityWorkbench.QueryCondition(false, "Term", "RepresentationID", true, SQL, "IsRankingTerm", "Term", "Is ranking", "Is a ranking term", Description, false, false, false, true);
            qIsRankingTerm.ForeignKeyTable = "TermRepresentation";
            qIsRankingTerm.ForeignKey = "TermID";
            QueryConditions.Add(qIsRankingTerm);

            DiversityWorkbench.QueryCondition _qTermSelection = new QueryCondition();
            _qTermSelection.QueryType = QueryCondition.QueryTypes.Module;
            DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
            _qTermSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)ST;
            _qTermSelection.Entity = "TermRepresentation.DisplayText";
            _qTermSelection.DisplayText = "Terms";
            _qTermSelection.DisplayLongText = "Selection of terms";
            _qTermSelection.Table = "TermRepresentation";
            _qTermSelection.IdentityColumn = "RepresentationID";
            _qTermSelection.Column = "RepresentationID";
            _qTermSelection.UpperValue = "DisplayText";
            _qTermSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            _qTermSelection.QueryGroup = "Terms";
            _qTermSelection.Description = "All terms from the list";
            _qTermSelection.QueryType = QueryCondition.QueryTypes.Module;
            QueryConditions.Add(_qTermSelection);


#if DEBUG
            // war auskommentiert - Grund unklar
            //#else
            // funktioniert noch nicht

            System.Data.DataTable dtTermHierarchy = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display, NULL AS DisplayOrder, NULL AS ProjectID " +
                "UNION " +
                "SELECT R.TermID AS Value, tR.BroaderTermID AS ParentValue, R.DisplayText AS Display, R.DisplayText, R.TerminologyID AS ProjectID " +
                "FROM " + Prefix + "Term AS tR " +
                "INNER JOIN " + Prefix + " TermRepresentation AS R ON tR.TerminologyID = R.TerminologyID AND tR.TermID = R.TermID AND tR.PreferredRepresentationID = R.RepresentationID " +
                "WHERE (tR.IsRankingTerm = 0) ";
            if (this._DatabaseServiceRestriction != null && this._DatabaseServiceRestriction.IndexOf("TerminologyID = ") > -1)
                SQL += " AND tR." + this._DatabaseServiceRestriction;
            SQL += " ORDER BY Display ";
            Microsoft.Data.SqlClient.SqlDataAdapter aTermHierarchy = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aTermHierarchy.Fill(dtTermHierarchy); }
                catch (System.Exception ex) { }
            }
            if (dtTermHierarchy.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                System.Data.DataColumn DisplayOrder = new System.Data.DataColumn("DisplayOrder");
                System.Data.DataColumn ProjectID = new System.Data.DataColumn("ProjectID");
                dtTermHierarchy.Columns.Add(Value);
                dtTermHierarchy.Columns.Add(ParentValue);
                dtTermHierarchy.Columns.Add(Display);
                dtTermHierarchy.Columns.Add(DisplayOrder);
                dtTermHierarchy.Columns.Add(ProjectID);
            }

            System.Collections.Generic.List<DiversityWorkbench.QueryField> FFTermHierarchy = new List<QueryField>();
            DiversityWorkbench.QueryField CPC_Part = new QueryField("TermRepresentation", "TermID", "RepresentationID");
            FFTermHierarchy.Add(CPC_Part);
            Description = DiversityWorkbench.Functions.ColumnDescription("Term", "BroaderTermID");
            DiversityWorkbench.QueryCondition qTermHierarchy = new DiversityWorkbench.QueryCondition(false, FFTermHierarchy, "Term", "Hierarchy", "Hierarchy", Description, dtTermHierarchy, true, "DisplayOrder", "ParentValue", "Display", "Value");
            if (this._DatabaseServiceRestriction != null && this._DatabaseServiceRestriction.IndexOf("TerminologyID = ") > -1)
                qTermHierarchy.DependsOnCurrentProjectID = false;// true;
            else
                qTermHierarchy.DependsOnCurrentProjectID = true;
            QueryConditions.Add(qTermHierarchy);
#endif

            #endregion

            #region Property

            System.Data.DataTable dtProperty = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT R.Property, R.DisplayText " +
                "FROM " + Prefix + "TerminologyProperty AS R ORDER BY Display ";
            Microsoft.Data.SqlClient.SqlDataAdapter aProperty = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aProperty.Fill(dtProperty); }
                catch { }
            }

            SQL = " FROM " + Prefix + "TermRepresentation INNER JOIN " + Prefix + "TermProperty T ON TermRepresentation.TermID = T.TermID ";

            Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "Property");
            //DiversityWorkbench.QueryCondition qProperty = new DiversityWorkbench.QueryCondition(false, "TermProperty", "RepresentationID", true, SQL, "Property", "Property", "Property", "Property of a term", Description, dtProperty, false);
            DiversityWorkbench.QueryCondition qProperty = new DiversityWorkbench.QueryCondition(false, "TermProperty", "TermID", true, SQL, "Property", "Property", "Property", "Property of a term", Description, dtProperty, false);
            QueryConditions.Add(qProperty);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "TextValue");
            DiversityWorkbench.QueryCondition qPropertyTextValues = new DiversityWorkbench.QueryCondition(false, "TermProperty", "RepresentationID", true, SQL, "TextValue", "Property", "Text val.", "Text value of the property", Description, false, false, false, false, "TermID");
            QueryConditions.Add(qPropertyTextValues);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "NumericValue");
            DiversityWorkbench.QueryCondition qPropertyNumericValue = new DiversityWorkbench.QueryCondition(false, "TermProperty", "RepresentationID", true, SQL, "NumericValue", "Property", "Num.value", "Numeric value", Description, false, false, true, false, "TermID");
            QueryConditions.Add(qPropertyNumericValue);

            //Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "TextValue");
            //DiversityWorkbench.QueryCondition qPropertyTextValue = new DiversityWorkbench.QueryCondition(false, "TermProperty", "RepresentationID", true, SQL, "TextValue", "Property", "Txt.value", "Text value", Description, false, false, false, false, "TermID");
            //QueryConditions.Add(qPropertyTextValue);

            //Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "DateValue");
            //DiversityWorkbench.QueryCondition qPropertyDateValue = new DiversityWorkbench.QueryCondition(false, "TermProperty", "RepresentationID", true, SQL, "DateValue", "Property", "Date", "Date value", Description, false, false, false, false, "TermID");
            //QueryConditions.Add(qPropertyDateValue);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "Notes");
            DiversityWorkbench.QueryCondition qPropertyNotes = new DiversityWorkbench.QueryCondition(false, "TermProperty", "RepresentationID", true, SQL, "Notes", "Property", "Notes", "Notes of a property", Description, false, false, false, false, "TermID");
            QueryConditions.Add(qPropertyNotes);


            #endregion

            #region many properties

            //Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "TextValue");
            //DiversityWorkbench.QueryCondition qPropertyTextValue = new DiversityWorkbench.QueryCondition(false, "TermProperty", "RepresentationID", true, SQL, "TextValue", "Property", "Txt.value", "Text value", Description, false, false, false, false, "TermID");
            //QueryConditions.Add(qPropertyTextValue);

            //Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "TextValue");
            //DiversityWorkbench.QueryCondition qPropertyTextValue2 = new DiversityWorkbench.QueryCondition(false, "TermProperty", "RepresentationID", true, SQL, "TextValue", "Property", "Txt.value", "Text value", Description, false, false, false, false, "TermID");
            //QueryConditions.Add(qPropertyTextValue2);

            //System.Data.DataTable dtProperties = new System.Data.DataTable();
            //SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT DISTINCT R.TextValue, R.TextValue " +
            //    "FROM " + Prefix + "TermProperty AS R ORDER BY Display ";
            //Microsoft.Data.SqlClient.SqlDataAdapter aProperties = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            //{
            //    try { aProperties.Fill(dtProperties); }
            //    catch { }
            //}
            //Description = DiversityWorkbench.Functions.ColumnDescription("TermProperty", "TextValue");
            //DiversityWorkbench.QueryCondition qPropertyTextValues = new DiversityWorkbench.QueryCondition(true, "TermProperty", "RepresentationID", "TextValue", "Properties", "Property", "Property", Description, dtProperties, true);
            //qPropertyTextValues.IsSet = true;
            //QueryConditions.Add(qPropertyTextValues);
#if DEBUG
#endif
            SQL = " FROM " + Prefix + "TermRepresentation INNER JOIN " + Prefix + "View_TermProperty T ON TermRepresentation.TermID = T.TermID ";

            Description = DiversityWorkbench.Functions.ColumnDescription("View_TermProperty", "DisplayText");
            DiversityWorkbench.QueryCondition qPropertyValues = new DiversityWorkbench.QueryCondition(false, "View_TermProperty", "RepresentationID", true, SQL, "PropertyValue", "Properties", "Property", "Value of the property", Description, false, false, false, false, "TermID");
            qPropertyValues.IsSet = true;
            QueryConditions.Add(qPropertyValues);

            #endregion


            #region Reference

            Description = DiversityWorkbench.Functions.ColumnDescription("TermReference", "Reference");
            DiversityWorkbench.QueryCondition qReference = new DiversityWorkbench.QueryCondition(false, "TermReference", "RepresentationID", "Reference", "Reference", "Reference", "A reference", Description);
            QueryConditions.Add(qReference);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermReference", "InternalNotes");
            DiversityWorkbench.QueryCondition qInternalNotes = new DiversityWorkbench.QueryCondition(false, "TermReference", "RepresentationID", "InternalNotes", "Reference", "Notes", "Notes", Description);
            QueryConditions.Add(qInternalNotes);

            #endregion

            #region Resource

            Description = DiversityWorkbench.Functions.ColumnDescription("TermResource", "URI");
            DiversityWorkbench.QueryCondition qResourceURI = new DiversityWorkbench.QueryCondition(false, "TermResource", "RepresentationID", "URI", "Resource", "URI", "URI", Description);
            QueryConditions.Add(qResourceURI);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermResource", "Title");
            DiversityWorkbench.QueryCondition qResourceTitle = new DiversityWorkbench.QueryCondition(false, "TermResource", "RepresentationID", "Title", "Resource", "Title", "Title", Description);
            QueryConditions.Add(qResourceTitle);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermResource", "Notes");
            DiversityWorkbench.QueryCondition qResourceNotes = new DiversityWorkbench.QueryCondition(false, "TermResource", "RepresentationID", "Notes", "Resource", "Notes", "Notes", Description);
            QueryConditions.Add(qResourceNotes);

            Description = DiversityWorkbench.Functions.ColumnDescription("TermResource", "DataWithholdingReason");
            DiversityWorkbench.QueryCondition qResourceDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "TermResource", "RepresentationID", "DataWithholdingReason", "Resource", "Withhold.", "Withholding reason", Description);
            QueryConditions.Add(qResourceDataWithholdingReason);

            #endregion

            #region Section

            System.Data.DataTable dtSection = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT R.SectionID, R.DisplayText " +
                "FROM " + Prefix + "Section AS R ORDER BY Display ";
            Microsoft.Data.SqlClient.SqlDataAdapter aSection = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aSection.Fill(dtSection); }
                catch { }
            }

            SQL = " FROM " + Prefix + "TermRepresentation INNER JOIN " + Prefix + "SectionTerm T ON TermRepresentation.TermID = T.TermID ";

            Description = DiversityWorkbench.Functions.ColumnDescription("SectionTerm", "SectionID");
            DiversityWorkbench.QueryCondition qSection = new DiversityWorkbench.QueryCondition(false, "SectionTerm", "RepresentationID", true, SQL, "SectionID", "Section", "Section", "Section", Description, dtSection, false);
            QueryConditions.Add(qSection);

            #endregion            

            ///TODO: Auswahl der DB muss in Formular erfolgen - sonst Kollision mit externen Aufrufen!
            /*
            System.Data.DataTable dtTerminology = new System.Data.DataTable();
            string SQL = "SELECT TerminologyID AS [Value], Terminology.DisplayText AS Display " +
                    "FROM Terminology " +
                    "WHERE TerminologyID IN (SELECT TerminologyID FROM TerminologyID_UserAvailable) " +
                    "ORDER BY DisplayText";
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                try { a.Fill(dtTerminology); }
                catch { }
            }
            if (dtTerminology.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtTerminology.Columns.Add(Value);
                dtTerminology.Columns.Add(Display);
            }
            SQL = "FROM Terminology INNER JOIN " +
                "TermRepresentation ON Terminology.TerminologyID = TermRepresentation.TerminologyID ";
            Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "DisplayText");
            DiversityWorkbench.QueryCondition qTerminology = new DiversityWorkbench.QueryCondition(true, "TermRepresentation", "RepresentationID", true, SQL, "TerminologyID", "Terminology", "Terminology", "Terminology", Description, dtTerminology, false);
            QueryConditions.Add(qTerminology);
            */

            return QueryConditions;
        }

        #region Domains

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns(string Domain)
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns;
            switch (Domain)
            {
                case "Terminology":
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
                    QueryDisplayColumns[0].DisplayText = "DisplayText";
                    QueryDisplayColumns[0].DisplayColumn = "DisplayText";
                    QueryDisplayColumns[0].OrderColumn = "DisplayText";
                    QueryDisplayColumns[0].IdentityColumn = "TerminologyID";
                    QueryDisplayColumns[0].TableName = "Terminology";
                    break;
                default:
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[0];
                    break;
            }
            return QueryDisplayColumns;
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions(string Domain)
        {
            string Database = "DiversityScientificTerms";
            try
            {
                Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnection.DatabaseName;
            }
            catch { }

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
            switch (Domain)
            {
                case "Terminology":
                    #region Representation

                    string Description = "Scientific terminology";
                    DiversityWorkbench.QueryCondition qTerminology = new DiversityWorkbench.QueryCondition(true, "Terminology", "TerminologyID", "DisplayText", "Terminology", "Terminology", "Scientific terminology", Description);
                    QueryConditions.Add(qTerminology);

                    Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "ExternalDatabase");
                    DiversityWorkbench.QueryCondition qExternalDatabase = new DiversityWorkbench.QueryCondition(false, "Terminology", "TerminologyID", "ExternalDatabase", "Terminology", "Ext. database", "External database", Description);
                    QueryConditions.Add(qExternalDatabase);

                    Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "ExternalDatabaseAuthors");
                    DiversityWorkbench.QueryCondition qExternalDatabaseAuthors = new DiversityWorkbench.QueryCondition(false, "Terminology", "TerminologyID", "ExternalDatabaseAuthors", "Terminology", "Ext. DB aut.", "External database authors", Description);
                    QueryConditions.Add(qExternalDatabaseAuthors);

                    #endregion

                    break;
                default:
                    break;
            }
            return QueryConditions;
        }


        private System.Collections.Generic.Dictionary<string, string> TerminologyValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT U.BaseURL + 'Terminology/' + CAST(TerminologyID AS varchar) AS  _URI, T.DisplayText AS _DisplayText, " +
                    "DisplayText, Description, TerminologyID AS ID, ExternalDatabase, ExternalDatabaseVersion, ExternalDatabaseAuthors, ExternalDatabaseURI, " +
                    "ExternalDatabaseInstitution, ExternalAttribute_NameID, Rights, DefaultLanguageCode " +
                    "FROM " + Prefix + "Terminology AS T, " + Prefix + "ViewBaseURL U " +
                    "WHERE T. TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT TOP (1) substring([BaseURL], 1, len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500))) AS Server, " +
                    "replace(substring([BaseURL],  len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500)) + 1, 500), '/', '') AS [Database] " +
                    "FROM  [ViewBaseURL] ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT COUNT(*) AS [Number of terms] " +
                    "FROM  " + Prefix + "Term " +
                    "GROUP BY TerminologyID " +
                    "HAVING TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT DisplayText AS Property/*Property,  Description, Datatype ,*/ " +
                    "FROM " + Prefix + "TerminologyProperty AS P " +
                    "WHERE        TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT Reference, ReferenceDetails " +
                    "FROM " + Prefix + "TerminologyReference AS R " +
                    "WHERE TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        #endregion

        #region Dependent

        public override bool DependentQueryAvailable(string URI)
        {
            //if (this._ServerConnection == null)
            this._ServerConnection = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);

            string RepresentationID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI);
            // getting the TermID
            string TermID = this.SqlScalar("SELECT TermID FROM " + Prefix + "TermRepresentation WHERE RepresentationID = " + RepresentationID);
            // getting the TerminologyID
            string TerminologyID = this.SqlScalar("SELECT TerminologyID FROM " + Prefix + "TermRepresentation WHERE RepresentationID = " + RepresentationID);
            // getting the Terms superior to the given Term - these contain the dependency information
            string BroaderTermID = TermID;
            string SQL = BroaderTermID;
            while (BroaderTermID.Length > 0)
            {
                // getting the broader terms
                BroaderTermID = this.SqlScalar("SELECT BroaderTermID FROM " + Prefix + "Term WHERE TermID = " + BroaderTermID);
                if (BroaderTermID.Length > 0)
                    SQL += ", " + BroaderTermID;
                else
                    break;
            }
            SQL = "SELECT  COUNT(*)  " +
                "FROM " + Prefix + "TermDependency AS D INNER JOIN " +
                Prefix + "Term AS T ON D.TerminologyID = T.TerminologyID AND D.TermID = T.TermID INNER JOIN " +
                Prefix + "TermRepresentation AS R ON T.TerminologyID = R.TerminologyID AND T.TermID = R.TermID AND T.PreferredRepresentationID = R.RepresentationID " +
                "WHERE D.DependsOnTermID IN (" + SQL + ") AND D.TerminologyID = " + TerminologyID + " AND T.IsRankingTerm = 1";
            string Result = this.SqlScalar(SQL);
            if (Result == "0" || Result == "")
                return false;
            else
                return true;
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] DependentQueryDisplayColumns(string URI)
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[3];

            QueryDisplayColumns[0].DisplayText = "DisplayText";
            QueryDisplayColumns[0].DisplayColumn = "DisplayText";
            QueryDisplayColumns[0].OrderColumn = "DisplayText";
            QueryDisplayColumns[0].IdentityColumn = "RepresentationID";
            QueryDisplayColumns[0].TableName = "TermRepresentation";

            QueryDisplayColumns[1].DisplayText = "Hierarchy up";
            QueryDisplayColumns[1].DisplayColumn = "HierarchyCache";
            QueryDisplayColumns[1].OrderColumn = "HierarchyCache";
            QueryDisplayColumns[1].IdentityColumn = "RepresentationID";
            QueryDisplayColumns[1].TableName = "TermRepresentation";

            QueryDisplayColumns[2].DisplayText = "Hierarchy down";
            QueryDisplayColumns[2].DisplayColumn = "HierarchyCacheDown";
            QueryDisplayColumns[2].OrderColumn = "HierarchyCacheDown";
            QueryDisplayColumns[2].IdentityColumn = "RepresentationID";
            QueryDisplayColumns[2].TableName = "TermRepresentation";

            return QueryDisplayColumns;
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> DependentQueryConditions(string URI)
        {
            if (this._ServerConnection == null)
                this._ServerConnection = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);

            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            try
            {

                string RepresentationID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(URI);
                // getting the TermID
                string TermID = this.SqlScalar("SELECT TermID FROM " + Prefix + "TermRepresentation WHERE RepresentationID = " + RepresentationID);
                // getting the TerminologyID
                string TerminologyID = this.SqlScalar("SELECT TerminologyID FROM " + Prefix + "TermRepresentation WHERE RepresentationID = " + RepresentationID);
                // getting the Terms superior to the given Term - these contain the dependency information
                string BroaderTermID = TermID;
                string SQL = BroaderTermID;
                while (BroaderTermID.Length > 0)
                {
                    // getting the broader terms
                    BroaderTermID = this.SqlScalar("SELECT BroaderTermID FROM " + Prefix + "Term WHERE TermID = " + BroaderTermID);
                    if (BroaderTermID.Length > 0)
                        SQL += ", " + BroaderTermID;
                    else
                        break;
                }
                SQL = "/*SELECT  NULL AS [Value], NULL AS Display UNION*/ " +
                    "SELECT  R.TermID AS [Value], R.DisplayText AS Display  " +
                    "FROM " + Prefix + "TermDependency AS D INNER JOIN " +
                    Prefix + "Term AS T ON D.TerminologyID = T.TerminologyID AND D.TermID = T.TermID INNER JOIN " +
                    Prefix + "TermRepresentation AS R ON T.TerminologyID = R.TerminologyID AND T.TermID = R.TermID AND T.PreferredRepresentationID = R.RepresentationID " +
                    "WHERE D.DependsOnTermID IN (" + SQL + ") AND D.TerminologyID = " + TerminologyID + " AND T.IsRankingTerm = 1";
                System.Data.DataTable dtRanking = this.SqlTable(SQL);

                SQL = " FROM " + Prefix + "TermRepresentation T INNER JOIN " + Prefix + "Term E ON E.TermID = T.TermID ";

                string Description = DiversityWorkbench.Functions.ColumnDescription("Term", "RankingTermID");
                //DiversityWorkbench.QueryCondition qRanking = new DiversityWorkbench.QueryCondition(true, "Term", "TermID", true, SQL, "RankingTermID", "Term", "Type/rank", "Terms of a certain type or ranking", Description, dtRanking, false);
                //QueryConditions.Add(qRanking);

                //DiversityWorkbench.QueryCondition qRanking2 = new DiversityWorkbench.QueryCondition(true, "TermRepresentation", "RepresentationID", true, SQL, "RankingTermID", "Term", "Type/rank", "Terms of a certain type or ranking", Description, dtRanking, false);
                //qRanking2.IntermediateTable = "Term";
                //qRanking2.ForeignKeySecondColumn = "TermID";
                //QueryConditions.Add(qRanking2);

                //DiversityWorkbench.QueryCondition qRanking = new DiversityWorkbench.QueryCondition(true, "Term", "TermID", true, SQL, "RankingTermID", "Term", "Ranking", "Terms of a certain ranking", Description, dtRanking, false);

                DiversityWorkbench.QueryCondition qRanking = new DiversityWorkbench.QueryCondition(true, "Term", "RepresentationID", true, SQL, "RankingTermID", "Term", "Ranking", "Terms of a certain ranking", Description, dtRanking, false);
                qRanking.IntermediateTable = "Term";
                qRanking.ForeignKey = "TermID";
                QueryConditions.Add(qRanking);



                string SqlRanking = "";
                if (dtRanking != null && dtRanking.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dtRanking.Rows)
                    {
                        if (!R["Value"].Equals(System.DBNull.Value))
                        {
                            if (SqlRanking.Length > 0)
                                SqlRanking += ", ";
                            SqlRanking += R["Value"].ToString();
                        }
                    }
                }

                Description = "Scientific term within a certain terminology";
                DiversityWorkbench.QueryCondition qTermRepresentation = new DiversityWorkbench.QueryCondition(true, "TermRepresentation", "RepresentationID", "DisplayText", "Term", "Term", "Scientific term", Description);
                qTermRepresentation.SqlFromClause = " FROM " + Prefix + "TermRepresentation T INNER JOIN " + Prefix + "Term E ON E.TermID = T.TermID AND E.RankingTermID IN(" + SqlRanking + ")";
                QueryConditions.Add(qTermRepresentation);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return QueryConditions;
        }

        private System.Collections.Generic.Dictionary<string, string> DependentTermValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT U.BaseURL + 'Terminology/' + CAST(TerminologyID AS varchar) AS  _URI, T.DisplayText AS _DisplayText, " +
                    "DisplayText, Description, TerminologyID AS ID, ExternalDatabase, ExternalDatabaseVersion, ExternalDatabaseAuthors, ExternalDatabaseURI, " +
                    "ExternalDatabaseInstitution, ExternalAttribute_NameID, Rights, DefaultLanguageCode " +
                    "FROM " + Prefix + "Terminology AS T, " + Prefix + "ViewBaseURL U " +
                    "WHERE T. TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT TOP (1) substring([BaseURL], 1, len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500))) AS Server, " +
                    "replace(substring([BaseURL],  len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500)) + 1, 500), '/', '') AS [Database] " +
                    "FROM  [ViewBaseURL] ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT COUNT(*) AS [Number of terms] " +
                    "FROM  " + Prefix + "Term " +
                    "GROUP BY TerminologyID " +
                    "HAVING TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT DisplayText AS Property/*Property,  Description, Datatype ,*/ " +
                    "FROM " + Prefix + "TerminologyProperty AS P " +
                    "WHERE        TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT Reference, ReferenceDetails " +
                    "FROM " + Prefix + "TerminologyReference AS R " +
                    "WHERE TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        #endregion

        #endregion

        #region Backlinks

        public override System.Windows.Forms.ImageList BackLinkImages(ModuleType CallingModule)
        {
            if (this._BackLinkImages == null)
            {
                this._BackLinkImages = this.BackLinkImages();
            }
            switch (CallingModule)
            {
                case ModuleType.Agents:
                    this._BackLinkImages.Images.Add("Image creator", DiversityWorkbench.Properties.Resources.Image);
                    this._BackLinkImages.Images.Add("Image license holder", DiversityWorkbench.Properties.Resources.Document);
                    break;
                case ModuleType.Projects:
                    this._BackLinkImages.Images.Add("Project", DiversityWorkbench.Properties.Resources.Project);
                    break;
                case ModuleType.References:
                    this._BackLinkImages.Images.Add("TermReference", DiversityWorkbench.Properties.Resources.References);
                    break;
            }
            return this._BackLinkImages;
        }


        public override System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BackLinkServerConnectionDomains(string URI, ModuleType CallingModule, bool IncludeEmpty = false, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BLD = new Dictionary<ServerConnection, List<BackLinkDomain>>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this.BackLinkConnections(ModuleType.ScientificTerms))
            {
                switch (CallingModule)
                {
                    case ModuleType.Agents:
                        System.Collections.Generic.List<BackLinkDomain> _A = this.BackLinkDomainAgents(KV.Value, URI);
                        if (_A.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _A);
                        break;
                    case ModuleType.Projects:
                        System.Collections.Generic.List<BackLinkDomain> _P = this.BackLinkDomainProject(KV.Value, URI);
                        if (_P.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _P);
                        break;
                    case ModuleType.References:
                        System.Collections.Generic.List<BackLinkDomain> _R = this.BackLinkDomainReferences(KV.Value, URI);
                        if (_R.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _R);
                        break;
                }
            }
            return BLD;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainAgents(ServerConnection SC, string URI)
        {
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain ESIC = this.BackLinkDomain(SC, URI, "Resource creator", "TermResource", "CreatorAgentURI", 2);
            if (ESIC.DtItems.Rows.Count > 0)
                Links.Add(ESIC);
            DiversityWorkbench.BackLinkDomain ESIL = this.BackLinkDomain(SC, URI, "Resource license holder", "TermResource", "LicenseHolderAgentURI", 3);
            if (ESIC.DtItems.Rows.Count > 0)
                Links.Add(ESIL);
            return Links;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainProject(ServerConnection SC, string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();

            // Project
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain("Terminology", "Terminology", "ProjectURI", 2);
            string Prefix = "dbo.";
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "]." + Prefix;
            string SQL = "SELECT 'First of ' + CAST(COUNT(*) as varchar) + ' terms' AS DisplayText, " +
                "MIN(R.RepresentationID) AS ID " +
                "FROM " + Prefix + "Terminology AS T " +
                "INNER JOIN " + Prefix + "ProjectProxy AS P ON T.ProjectURI = P.ProjectURI " +
                "INNER JOIN " + Prefix + "TermRepresentation AS R ON T.TerminologyID = R.TerminologyID " +
                "WHERE(T.ProjectURI = '" + URI + "') " +
                "GROUP BY T.TerminologyID, T.ProjectURI ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
            try
            {
                ad.Fill(BackLink.DtItems);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (BackLink.DtItems.Rows.Count > 0)
                Links.Add(BackLink);

            return Links;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainReferences(ServerConnection SC, string URI)
        {
            System.Collections.Generic.List<BackLinkDomain> Refs = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain Terminoloy = this.BackLinkDomain(SC, URI, "Terminoloy", "TerminologyReference", "ReferenceURI", 2);
            if (Terminoloy.DtItems.Rows.Count > 0)
                Refs.Add(Terminoloy);
            DiversityWorkbench.BackLinkDomain Term = this.BackLinkDomain(SC, URI, "Term", "TermReference", "ReferenceURI", 2);
            if (Term.DtItems.Rows.Count > 0)
                Refs.Add(Term);
            return Refs;
        }

        private DiversityWorkbench.BackLinkDomain BackLinkDomain(ServerConnection SC, string URI, string DisplayText, string Table, string LinkColumn, int ImageKey, System.Collections.Generic.List<string> Restrictions = null)
        {
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain(DisplayText, Table, LinkColumn, ImageKey);
            string Prefix = "[" + SC.DatabaseName + "].dbo."; // Toni 20210727 database name added
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "]." + Prefix;
            string SQL = "SELECT S.DisplayText, " +
                (Table == "TerminologyReference" ? "S.TerminologyID AS ID " : "S.RepresentationID AS ID ") + // Toni 20210727 compare for table type 
                "FROM " + Prefix + Table + " AS T " +
                "INNER JOIN " + Prefix + "TermRepresentation AS S ON ";
            if (Table == "TerminologyReference")
                SQL += "T.TerminologyID = S.TerminologyID ";
            else
                SQL += "T.RepresentationID = S.RepresentationID ";
            SQL += "WHERE(T." + LinkColumn + " = '" + URI + "') ";
            if (Restrictions != null)
            {
                foreach (string R in Restrictions)
                {
                    SQL += " AND " + R;
                }
            }
            SQL += " GROUP BY S.DisplayText, ";
            if (Table == "TerminologyReference") // Toni 20210727 compare for table type
                SQL += "S.TerminologyID ";
            else
                SQL += "S.RepresentationID ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
            try
            {
                ad.Fill(BackLink.DtItems);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return BackLink;
        }

        #endregion

        #region static functions

        #region Synonymy

        ///// <summary>
        ///// Getting the terms underneath a given term including the given term
        ///// </summary>
        ///// <param name="URL">The URL of the term (= BaseURL + RepresentationID)</param>
        ///// <returns>A dictionary containing the URLs and the terms</returns>
        //public static System.Collections.Generic.Dictionary<string, string> SubPlots(string URL)
        //{
        //    System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
        //    System.Data.DataTable dt = getStartTerm(URL);
        //    getSubTerms(ref dt);
        //    getTerms(ref DD, dt);
        //    return DD;
        //}

        /// <summary>
        /// Getting the synonyms of a given term
        /// </summary>
        /// <param name="URL">The URL of the term (= BaseURL + RepresentationID)</param>
        /// <returns>A dictionary containing the URLs and the terms</returns>
        public static System.Collections.Generic.Dictionary<string, string> Synonyms(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartTerm(URL);
            getSynonyms(ref dt);
            getTerms(ref DD, dt);
            return DD;
        }

        public static System.Collections.Generic.Dictionary<string, string> SynonymsOfList(System.Collections.Generic.List<string> URLs)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartTerms(URLs);
            getSynonyms(ref dt);
            getTerms(ref DD, dt);
            return DD;
        }


        /// <summary>
        /// Getting the terms underneath a given term including the given term and all synonyms
        /// </summary>
        /// <param name="URL">The URL of the taxon (= BaseURL + NameID)</param>
        /// <returns>A dictionary containing the URLs and the names</returns>
        public static System.Collections.Generic.Dictionary<string, string> SubTermSynonyms(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartTerm(URL);
            getSubTerms(ref dt);
            getSynonyms(ref dt);
            getTerms(ref DD, dt);
            return DD;
        }

        public static string PreferredName(string URL, ref string UrlPreferredName)
        {
            string PreferredName = "";
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartTerm(URL);
            PreferredName = getPreferredName(ref dt, ref UrlPreferredName);
            return PreferredName;
        }

        #endregion

        #region Auxillary

        private static int? _TerminologyID;

        private static int TerminologyID(string IDs)
        {
            if (_TerminologyID == null)
            {
                string SQL = "SELECT DISTINCT TerminologyID FROM " + _SC.Prefix() + "TermRepresentation R WHERE RepresentationID IN (" + IDs + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                    _TerminologyID = int.Parse(dt.Rows[0][0].ToString());
                else
                {
                    dt = new System.Data.DataTable();
                    SQL = "SELECT TerminologyID, DisplayText FROM " + _SC.Prefix() + "Terminology ORDER BY DisplayText";
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dt);
                    if (dt.Rows.Count > 1)
                    {
                        DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Terminology", "TerminologyID", "Terminology", "Please select a terminology");
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            int i;
                            if (int.TryParse(f.SelectedValue, out i))
                                _TerminologyID = i;
                        }
                    }
                    else
                        _TerminologyID = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return (int)_TerminologyID;
        }

        private static DiversityWorkbench.ServerConnection _SC;

        private static System.Data.DataTable getStartTerm(string URL)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                // resetting the project
                _TerminologyID = null;
                // getting the server connection for the URL
                setServerConnection(URL);
                // Inserting the ID to start the query
                string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(URL);
                string SQL = "SELECT RepresentationID FROM " + _SC.Prefix() + "TermRepresentation N WHERE RepresentationID = " + ID;
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                ad.Fill(dt);
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }

        private static System.Data.DataTable getStartTerms(System.Collections.Generic.List<string> URLs)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                // resetting the project
                _TerminologyID = null;
                // getting the server connection for the URL
                setServerConnection(URLs[0]);
                // Inserting the ID to start the query
                string IDs = DiversityWorkbench.WorkbenchUnit.getIDFromURI(URLs[0]);
                if (URLs.Count > 1)
                {
                    for (int i = 1; i < URLs.Count; i++)
                    {
                        IDs += ", " + DiversityWorkbench.WorkbenchUnit.getIDFromURI(URLs[i]);
                    }
                }
                string SQL = "SELECT RepresentationID FROM " + _SC.Prefix() + "TermRepresentation N WHERE RepresentationID IN ( " + IDs + ") ";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                ad.Fill(dt);
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }


        public static void setServerConnection(string URL)
        {
            // getting the server connection for the URL
            _SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URL);
        }

        public static void getTerms(ref System.Collections.Generic.Dictionary<string, string> DD, System.Data.DataTable DT)
        {
            string SQL = "";
            try
            {
                foreach (System.Data.DataRow R in DT.Rows)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += R[0].ToString();
                }
                SQL = "SELECT U.BaseURL + cast(T.RepresentationID as varchar) AS URL, T.DisplayText AS Term FROM " + _SC.Prefix() + "TermRepresentation T, " + _SC.Prefix() + "ViewBaseURL U WHERE T.RepresentationID IN (" + SQL + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (!DD.ContainsKey(R[0].ToString()))
                        DD.Add(R[0].ToString(), R[1].ToString());
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private static void getSubTerms(ref System.Data.DataTable DT)
        {
            string IDs = "";
            foreach (System.Data.DataRow R in DT.Rows)
            {
                if (IDs.Length > 0) IDs += ", ";
                IDs += R[0].ToString();
            }
            string SQL = "SELECT R.RepresentationID " +
                " FROM " + _SC.Prefix() + "Term T, " +
                "" + _SC.Prefix() + "Term TP, " +
                "" + _SC.Prefix() + "TermRepresentation R, " +
                "" + _SC.Prefix() + "TermRepresentation RP " +
                " WHERE R.TermID = T.TermID " +
                " AND TP.TermID = RP.TermID " +
                " AND T.BroaderTermID = TP.TermID " +
                " AND T.TerminologyID = R.TerminologyID " +
                " AND T.TerminologyID = TP.TerminologyID " +
                " AND TP.TerminologyID = RP.TerminologyID " +
                " AND T.TerminologyID = " + TerminologyID(IDs) + " " +
                " AND RP.RepresentationID IN ( " + IDs + ") " +
                " AND R.RepresentationID NOT IN (" + IDs + ")";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ad.Fill(DT);
                getSubTerms(ref DT);
            }
        }

        private static void getSynonyms(ref System.Data.DataTable DT, bool RestricTerminology = false)
        {
            try
            {
                if (_SC == null)
                    return;

                string IDs = "";
                foreach (System.Data.DataRow R in DT.Rows)
                {
                    if (IDs.Length > 0) IDs += ", ";
                    IDs += R[0].ToString();
                }
                string sTerminologyID = "";
                sTerminologyID = TerminologyID(IDs).ToString();
                // getting the terms that are synonmys to the terms in the table
                string SQL = "SELECT R.RepresentationID FROM " + _SC.Prefix() + "Term T, " +
                    _SC.Prefix() + "TermRepresentation R, " +
                    _SC.Prefix() + "TermRepresentation S " +
                    "WHERE R.TermID = T.TermID  AND S.TermID = T.TermID " +
                    " AND S.RepresentationID IN( " + IDs + ") " +
                    " AND T.TerminologyID = " + sTerminologyID + " " +
                    " AND R.RepresentationID NOT IN (" + IDs + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ad.Fill(DT);
                    getSynonyms(ref DT);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static string getPreferredName(ref System.Data.DataTable DT, ref string UrlPreferredName)
        {
            string PreferredName = "";
            try
            {
                if (_SC != null)
                {
                    string IDs = "";
                    foreach (System.Data.DataRow R in DT.Rows)
                    {
                        if (IDs.Length > 0) IDs += ", ";
                        IDs += R[0].ToString();
                    }
                    System.Data.DataTable dtTerminologyID = new System.Data.DataTable();

                    string sTerminologyID = "";
                    string SQL = "SELECT TOP 1 R.TerminologyID FROM " + _SC.Prefix() + "TermRepresentation R" +
                        " WHERE R.RepresentationID IN (" + IDs + ") ";
                    sTerminologyID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);

                    SQL = "SELECT P.DisplayText, P.RepresentationID " +
                        "FROM " + _SC.Prefix() + "[TermRepresentation] P, " + _SC.Prefix() + "[TermRepresentation] R, " + _SC.Prefix() + "Term T " +
                        " WHERE P.TermID = R.TermID " +
                        " AND T.TermID = p.TermID " +
                        " AND T.PreferredRepresentationID = P.RepresentationID " +
                        " AND T.TerminologyID = " + sTerminologyID +
                        " AND R.RepresentationID IN (" + IDs + ")";

                    ad.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        PreferredName = dt.Rows[0][0].ToString();
                        UrlPreferredName = _SC.BaseURL + dt.Rows[0][1].ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return PreferredName;
        }


        #endregion


        #endregion

    }
}
