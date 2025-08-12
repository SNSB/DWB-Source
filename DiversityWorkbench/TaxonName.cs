#define UseGetDataFunction
using DWBServices.WebServices.TaxonomicServices;
using System;
using System.Collections.Generic;
using DWBServices.WebServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DiversityWorkbench
{

    public enum TaxonomicGroups { Myxomycetes, Plants, Fungi, IndexFungorum, Vertebrata, Fossils, Athropoda }

    public class TaxonName : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {

        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryConditionProject;
        private System.Collections.Generic.Dictionary<string, string> _ServiceList;

        #endregion

        #region Construction

        public TaxonName(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityTaxonNames"; }

        /// <summary>
        /// Additional services provided by webservices by external providers
        /// </summary>
        /// <returns></returns>
        public override System.Collections.Generic.Dictionary<string, string> AdditionalServicesOfModule()
        {
            if (this._ServiceList == null)
            {
                this._ServiceList = new Dictionary<string, string>();
                // Iterate over all ServiceTypes and add them to _ServiceList
                var serviceTypeNameDictionary = DwbServiceEnums.TaxonomicServiceInfoDictionary();
                foreach (var entry in serviceTypeNameDictionary)
                {
                    if (entry.Key != DwbServiceEnums.DwbService.None)
                    {
                        _ServiceList.Add(entry.Key.ToString(), entry.Value.Name);
                    }
                }

                _ServiceList.Add("CacheDB", "Cache database");
            }

            return _ServiceList;
        }
     
        protected override System.Collections.Generic.Dictionary<string, string> AdditionalServiceURIsOfModule()
        {
            System.Collections.Generic.Dictionary<string, string> _Add = new Dictionary<string, string>();
            var serviceTypeNameDictionary = DwbServiceEnums.TaxonomicServiceInfoDictionary();
            foreach (var entry in serviceTypeNameDictionary)
            {
                if (entry.Key != DwbServiceEnums.DwbService.None)
                {
                    _Add.Add(entry.Key.ToString(), entry.Value.Url);
                }
            }
            return _Add;
        }

        public override List<DiversityWorkbench.DatabaseService> DatabaseServices()
        {
            List<DiversityWorkbench.DatabaseService> ds = new List<DatabaseService>();
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in
                         DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()]
                             .ServerConnectionList())
                {
                    if (KV.Value.ConnectionIsValid)
                    {
                        DiversityWorkbench.DatabaseService d = new DatabaseService(KV.Key);
                        d.IsWebservice = false;
                        if (KV.Value.DatabaseServer != DiversityWorkbench.Settings.DatabaseServer)
                        {
                            d.Server = KV.Value.DatabaseServer;
                        }

                        ds.Add(d);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            // Create and add DatabaseServices for each ServiceType in the enum
            foreach (DwbServiceEnums.DwbService serviceType in System.Enum.GetValues(
                         typeof(DwbServiceEnums.DwbService)))
            {
                if (DwbServiceEnums.TaxonomicServiceInfoDictionary().TryGetValue(serviceType, out DwbServiceEnums.DwbServiceInfo serviceInfo))
                {
                    if (serviceInfo.Type == DwbServiceEnums.DwbServiceType.None)
                    {
                        continue;
                    }
                    DiversityWorkbench.DatabaseService service = new DatabaseService(serviceType.ToString())
                    {
                        IsWebservice = true,
                        DisplayUnitdataInTreeview = true,
                        WebService = serviceType
                    };
                    ds.Add(service);
                }
            }

            return ds;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            switch (Domain)
            {
                case "ProjectProxy":
                    Values = ProjectProxyValues(ID);
                    break;
                case "TaxonNameListProjectProxy":
                    Values = TaxonNameListProjectProxyValues(ID);
                    break;
                default:
                    Values = UnitValues(ID);
                    break;
            }
            return Values;
        }

        private System.Collections.Generic.Dictionary<string, string> ProjectProxyValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT dbo.BaseURL() + 'ProjectProxy/' + CAST(ProjectID AS varchar) AS _URI, Project AS _DisplayText, " +
                    "ProjectID " +
                    "FROM [ProjectProxy] AS T " +
                    "WHERE ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT TOP (1) substring([BaseURL], 1, len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500))) AS Server, " +
                    "replace(substring([BaseURL],  len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500)) + 1, 500), '/', '') AS [Database] " +
                    "FROM [ViewBaseURL] ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT COUNT(*) AS [Number of taxa] " +
                    "FROM TaxonNameProject " +
                    "GROUP BY ProjectID " +
                    "HAVING ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT COUNT(*) AS [Number of accepted names] " +
                    "FROM TaxonAcceptedName " +
                    "GROUP BY ProjectID " +
                    "HAVING ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT COUNT(*) AS [Number of synonyms] " +
                    "FROM TaxonSynonymy " +
                    "GROUP BY ProjectID " +
                    "HAVING ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT case when U.CombinedNameCache <> '' then U.CombinedNameCache else P.LoginName end + CASE WHEN P.ReadOnly = 1 then ' (Read only)' else '' end AS [User] " +
                    "FROM ProjectUser AS P INNER JOIN UserProxy AS U ON P.LoginName = U.LoginName " +
                    "WHERE P.ProjectID = " + ID.ToString() +
                    "ORDER BY [User] ";
                this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        private System.Collections.Generic.Dictionary<string, string> TaxonNameListProjectProxyValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT dbo.BaseURL() + 'TaxonNameListProjectProxy/' + CAST(L.ProjectID AS varchar) AS _URI, L.Project AS _DisplayText, " +
                    "ProjectID, L.TaxonomicGroup, L.DisplayText, L.DefaultProjectID, P.Project AS DefaultProject " +
                    "FROM TaxonNameListProjectProxy AS L LEFT OUTER JOIN ProjectProxy AS P ON L.DefaultProjectID = P.ProjectID " +
                    "WHERE L.ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT TOP (1) substring([BaseURL], 1, len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500))) AS Server, " +
                    "replace(substring([BaseURL],  len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500)) + 1, 500), '/', '') AS [Database] " +
                    "FROM  [ViewBaseURL] ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT COUNT(*) AS [Number of taxa] " +
                    "FROM  TaxonNameList " +
                    "GROUP BY ProjectID " +
                    "HAVING ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT PlaceNameCache AS Areas " +
                    "FROM  TaxonNameListArea " +
                    "WHERE ProjectID = " + ID.ToString() +
                    "ORDER BY Areas ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT case when U.CombinedNameCache <> '' then U.CombinedNameCache else P.LoginName end AS [User] " +
                    "FROM TaxonNameListUser AS P INNER JOIN UserProxy AS U ON P.LoginName = U.LoginName " +
                    "WHERE P.ProjectID = " + ID.ToString() +
                    "ORDER BY [User] ";
                this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                try
                {
                    string Prefix = this._ServerConnection.DatabaseName + ".dbo.";
                    if (this._ServerConnection.LinkedServer.Length > 0)
                        Prefix = "[" + this._ServerConnection.LinkedServer + "]." + Prefix;
                    string SQL = "";

                    SQL = "SELECT MAX(U.BaseURL) + CAST(T.NameID AS VARCHAR) AS _URI, " +
                        "MAX(U.BaseURL) + CAST(T.NameID AS VARCHAR) AS Link, T.TaxonNameCache AS _DisplayText, T.NameID AS ID, " +
                        "T.TaxonNameCache AS [Taxonomic name], T1.TaxonNameCache AS Basionym, T.TaxonomicRank AS Rank, " +
                        "T.GenusOrSupragenericName AS Genus, T.InfragenericEpithet AS [Infrageneric epithet], " +
                        "T.SpeciesEpithet AS [Species epithet], T.InfraspecificEpithet AS [Infraspecific epithet], " +
                        "case when T.NonNomenclaturalNameSuffix <> '' then T.NonNomenclaturalNameSuffix else " +
                        "case when T.CombiningAuthors <> '' then '(' else '' end + T.BasionymAuthors + case when T.CombiningAuthors <> '' then ')' else '' end + " +
                        "case when T.CombiningAuthors <> '' then ' ' + T.CombiningAuthors else '' end end AS Authors, T.NomenclaturalStatus AS [Nomenclatural status], " +
                        "case when T.ReferenceTitle is null then '' else T.ReferenceTitle end +  " +
                        "case when T.Volume is null then '' else '. ' + T.Volume end + case when T.Issue is null then '' else '. ' + T.Issue end +   " +
                        "case when T.Pages is null then '' else ', ' + T.Pages end +  " +
                        "case when T.YearOfPubl is null then '' else '. ' + cast(T.YearOfPubl as varchar) end AS Publication, " +
                        "case when T.IgnoreButKeepForReference = 0 then 'Valid name' else case when MAX(T2.TaxonNameCache) is null  " +
                        "then 'Is ignored, do not use' else 'Is replaced by: ' + MAX(T2.TaxonNameCache) + ' (ID: ' + cast(MAX(T2.NameID) as varchar) + ')' end end AS [Valid or ignored] " +
                        "FROM " +
                        Prefix + "ViewBaseURL AS U, " +
                        Prefix + "TaxonSynonymy AS S INNER JOIN  " +
                        Prefix + "TaxonName T2 ON S.SynNameID = T2.NameID RIGHT OUTER JOIN " +
                        Prefix + "TaxonName T1 RIGHT OUTER JOIN " +
                        Prefix + "TaxonName AS T ON T1.NameID = T.BasedOnNameID ON S.NameID = T.NameID " +
                        "WHERE (T.NameID = " + ID.ToString() + ") " +
                        "GROUP BY T.NameID, T.TaxonNameCache, T1.TaxonNameCache, T.Issue, T.IgnoreButKeepForReference, T.NomenclaturalStatus, " +
                        "T.TaxonomicRank,  T.ReferenceTitle, T.Volume, T.Pages, T.YearOfPubl, T.GenusOrSupragenericName, T.InfragenericEpithet " +
                        ", T.SpeciesEpithet, T.InfraspecificEpithet, T.BasionymAuthors, T.CombiningAuthors, T.NonNomenclaturalNameSuffix";
                    this.getDataFromTable(SQL, ref Values);

                    if (this._ServerConnection.LinkedServer.Length == 0)
                    {
                        SQL = "SELECT TaxonomicRankDisplayText + ':   ' + rtrim(TaxonNameCache) + '\r\n' AS [Hierarchy ranks], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] (" + ID.ToString() + " ,NULL) ORDER BY Seq DESC";
                        this.getDataFromTable(SQL, ref Values);
                    }
                    else
                    {
                        //SQL = "SELECT TaxonomicRankDisplayText + ':   ' + rtrim(TaxonNameCache) + '\r\n' AS [Hierarchy ranks], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] (" + ID.ToString() + " ,NULL) ORDER BY Seq DESC";
                        //this.getDataFromTable(SQL, ref Values);
                    }

                    bool GotData = false;
                    if (this._ServerConnection.LinkedServer.Length == 0)
                    {
                        SQL = "SELECT substring(TaxonNameCache, 1, charindex(' ', TaxonNameCache)) as [Family] FROM [dbo].[HierarchySuperiorList] " +
                            "(" + ID.ToString() + " , dbo.DefaultProjectID()) where TaxonomicRank = 'fam.'";
                        GotData = this.getDataFromTable(SQL, ref Values);
                    }
                    if (!GotData)
                    {
                        SQL = "SELECT MIN(F.Family) as Family FROM " + Prefix + "[TaxonFamily] F " +
                            " where F.NameID = " + ID.ToString();
                        this.getDataFromTable(SQL, ref Values);
                    }

                    GotData = false;
                    if (this._ServerConnection.LinkedServer.Length == 0)
                    {
                        SQL = "SELECT substring(TaxonNameCache, 1, charindex(' ', TaxonNameCache)) as [Order] FROM [dbo].[HierarchySuperiorList] " +
                            "(" + ID.ToString() + " , dbo.DefaultProjectID()) where TaxonomicRank = 'ord.'";
                        GotData = this.getDataFromTable(SQL, ref Values);
                    }
                    if (!GotData)
                    {
                        SQL = "SELECT MIN(O.[Order]) as [Order] FROM " + Prefix + "[TaxonOrder] O " +
                           " where O.NameID = " + ID.ToString();
                        this.getDataFromTable(SQL, ref Values);
                    }

                    GotData = false;
                    if (this._ServerConnection.LinkedServer.Length == 0)
                    {
                        SQL = "SELECT Taxon + CASE WHEN Seq > 1 THEN ' | ' ELSE '' END AS [Hierarchy], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] " +
                            "(" + ID.ToString() + " , dbo.DefaultProjectID()) ORDER BY Seq DESC";
                        GotData = this.getDataFromTable(SQL, ref Values);
                    }
                    if (!GotData)
                    {
                        SQL = "SELECT TOP 1 h.[HierarchyListCache] AS [Hierarchy] FROM " + Prefix + "[TaxonHierarchy] h where h.NameID = " + ID.ToString() + " AND h.[HierarchyListCache] <> '' ";
                        this.getDataFromTable(SQL, ref Values);
                    }

                    SQL = "SELECT N.TaxonNameCache + '\r\n' AS Synonym, S.SynType AS [Synonymy type] " +
                        "FROM " + Prefix + "TaxonSynonymy S INNER JOIN " +
                        Prefix + "TaxonName N ON S.SynNameID = N.NameID " +
                        "WHERE (S.IgnoreButKeepForReference = 0)  " +
                        "AND  (S.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    SQL = "SELECT N.TaxonNameCache AS [Accepted name] " +
                            "FROM " + Prefix + "TaxonAcceptedName A INNER JOIN " +
                            Prefix + "TaxonName N ON A.NameID = N.NameID INNER JOIN " +
                            Prefix + "TaxonSynonymy S ON A.NameID = S.SynNameID " +
                            "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0)  " +
                            "AND  (S.NameID = " + ID.ToString() + ")" +
                            "UNION " +
                            "SELECT N.TaxonNameCache AS [Accepted name] " +
                            "FROM " + Prefix + "TaxonAcceptedName A INNER JOIN " +
                            Prefix + "TaxonName N ON A.NameID = N.NameID " +
                            "WHERE (A.IgnoreButKeepForReference = 0)  " +
                            "AND (A.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    // CommonName
                    SQL = "SELECT C.CommonName AS [Common name], L.DisplayText AS Language " +
                        "FROM " + Prefix + "TaxonCommonName C INNER JOIN " +
                        Prefix + "LanguageCode_Enum L ON C.LanguageCode = L.Code " +
                        "WHERE NameID = " + ID.ToString();
                    this.getDataFromTable(SQL, ref Values);

                    // Geography
                    SQL = "SELECT PlaceNameCache AS Place " +
                        " FROM " + Prefix + "TaxonGeography " +
                        "WHERE NameID = " + ID.ToString();
                    this.getDataFromTable(SQL, ref Values);

                    // Typification
                    SQL = "SELECT T.Typification  " +
                            "FROM " + Prefix + "TaxonNameTypification AS T  " +
                            "WHERE (T.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    // Project
                    SQL = "SELECT P.Project " +
                        "FROM " + Prefix + "TaxonNameProject T INNER JOIN " +
                        Prefix + "ProjectList P ON T.ProjectID = P.ProjectID " +
                        "WHERE (T.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    // List
                    SQL = "SELECT P.Project AS List " +
                        "FROM " + Prefix + "TaxonNameList L INNER JOIN " +
                        Prefix + "TaxonNameListProjectProxy P ON L.ProjectID = P.ProjectID " +
                        "WHERE (L.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    // Analysis
                    SQL = "SELECT P.DisplayText + ': '  + C.DisplayText + case when A.AnalysisValue <> C.DisplayText " +
                            "and A.AnalysisValue <> '' then ' = ' + A.AnalysisValue else case when A.Notes <> '' then ' = ' + " +
                            "cast (A.Notes as nvarchar(4000)) " +
                            "else '' end end AS Analysis  " +
                            "FROM " + Prefix + "TaxonNameListAnalysis AS A INNER JOIN " +
                            Prefix + "TaxonNameListProjectProxy AS P ON A.ProjectID = P.ProjectID INNER JOIN " +
                            Prefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = C.AnalysisID " +
                            "WHERE (A.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    // Distribution
                    SQL = "SELECT P.DisplayText + ': '  + D.PlaceNameCache AS Distribution  " +
                            "FROM " + Prefix + "TaxonNameListDistribution AS D INNER JOIN " +
                            Prefix + "TaxonNameListProjectProxy AS P ON D.ProjectID = P.ProjectID  " +
                            "WHERE (D.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    // CollectionSpecimen
                    SQL = "SELECT P.DisplayText + ': '  + S.DisplayText AS [Collection specimen]  " +
                            "FROM " + Prefix + "TaxonNameListCollectionSpecimen AS S INNER JOIN " +
                            Prefix + "TaxonNameListProjectProxy AS P ON S.ProjectID = P.ProjectID  " +
                            "WHERE (S.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    // Reference
                    SQL = "SELECT P.DisplayText + ': '  + R.TaxonNameListRefText AS [List reference]  " +
                            "FROM " + Prefix + "TaxonNameListReference AS R INNER JOIN " +
                            Prefix + "TaxonNameListProjectProxy AS P ON R.ProjectID = P.ProjectID  " +
                            "WHERE (R.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);


                    SQL = "SELECT D.ExternalDatabaseName + CASE WHEN D.ExternalDatabaseVersion IS NULL OR " +
                        "LEN(D.ExternalDatabaseVersion) " +
                        "> 50 THEN '' ELSE '. Vers.: ' + D.ExternalDatabaseVersion END + CASE WHEN D.ExternalDatabaseAuthors " +
                        "IS NULL THEN '' ELSE '. Aut.: ' + D.ExternalDatabaseAuthors END + '\r\n' AS [External database], " +
                        "I.ExternalNameURI AS [External name URI] " +
                        "FROM  " + Prefix + "TaxonNameExternalID I INNER JOIN " +
                        Prefix + "TaxonNameExternalDatabase D ON I.ExternalDatabaseID = D.ExternalDatabaseID " +
                        "WHERE (I.NameID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
                    this._UnitValues.Clear();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                    {
                        this._UnitValues.Add(P.Key, P.Value);
                    }

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return Values;
        }


        #region UnitValues for many items
        //public override System.Collections.Generic.Dictionary<int, string> UnitValues(System.Collections.Generic.List<int> IDs, int ProjectID)
        //{
        //    System.Collections.Generic.Dictionary<int, string> Values = new Dictionary<int, string>();

        //    if (this._ServerConnection.ConnectionString.Length > 0)
        //    {
        //        try
        //        {
        //            // getting the name ids
        //            string NameIDs = IDsAsString(IDs);

        //            // getting the datatables
        //            System.Data.DataTable dtTaxa = this.DtTaxa(NameIDs, ProjectID);
        //            System.Data.DataTable dtTaxaHierarchy = this.DtTaxaHierarchy(ProjectID);
        //            System.Data.DataTable dtTaxaCommon = this.DtTaxaCommonNames(NameIDs);

        //            System.Collections.Generic.Dictionary<int, Taxon> taxa = new Dictionary<int, Taxon>();
        //            foreach(int ID in IDs)
        //            {
        //                Taxon tax = new Taxon();
        //                tax.NameID = ID;
        //                System.Data.DataRow[] dataRows = dtTaxa.Select("NameID = " + ID.ToString());
        //                if (dataRows != null && dataRows.Length > 0 ) 
        //                { 
        //                    System.Data.DataRow row = dataRows[0];
        //                    tax.TaxonName = row["Taxonomic name"].ToString();
        //                    tax.URI = row["Link"].ToString();
        //                }
        //                taxa.Add(ID, tax);
        //                string JsonString = JsonSerializer.Serialize(tax);
        //                Values.Add(ID, JsonString);
        //            }


        //            //this.getDataFromTable(SQL, ref Values);

        //            //if (this._ServerConnection.LinkedServer.Length == 0)
        //            //{
        //            //    SQL = "SELECT TaxonomicRankDisplayText + ':   ' + rtrim(TaxonNameCache) + '\r\n' AS [Hierarchy ranks], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] (" + ID.ToString() + " ,NULL) ORDER BY Seq DESC";
        //            //    this.getDataFromTable(SQL, ref Values);
        //            //}
        //            //else
        //            //{
        //            //    //SQL = "SELECT TaxonomicRankDisplayText + ':   ' + rtrim(TaxonNameCache) + '\r\n' AS [Hierarchy ranks], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] (" + ID.ToString() + " ,NULL) ORDER BY Seq DESC";
        //            //    //this.getDataFromTable(SQL, ref Values);
        //            //}

        //            //bool GotData = false;
        //            //if (this._ServerConnection.LinkedServer.Length == 0)
        //            //{
        //            //    SQL = "SELECT substring(TaxonNameCache, 1, charindex(' ', TaxonNameCache)) as [Family] FROM [dbo].[HierarchySuperiorList] " +
        //            //        "(" + ID.ToString() + " , dbo.DefaultProjectID()) where TaxonomicRank = 'fam.'";
        //            //    GotData = this.getDataFromTable(SQL, ref Values);
        //            //}
        //            //if (!GotData)
        //            //{
        //            //    SQL = "SELECT MIN(F.Family) as Family FROM " + Prefix + "[TaxonFamily] F " +
        //            //        " where F.NameID = " + ID.ToString();
        //            //    this.getDataFromTable(SQL, ref Values);
        //            //}

        //            //GotData = false;
        //            //if (this._ServerConnection.LinkedServer.Length == 0)
        //            //{
        //            //    SQL = "SELECT substring(TaxonNameCache, 1, charindex(' ', TaxonNameCache)) as [Order] FROM [dbo].[HierarchySuperiorList] " +
        //            //        "(" + ID.ToString() + " , dbo.DefaultProjectID()) where TaxonomicRank = 'ord.'";
        //            //    GotData = this.getDataFromTable(SQL, ref Values);
        //            //}
        //            //if (!GotData)
        //            //{
        //            //    SQL = "SELECT MIN(O.[Order]) as [Order] FROM " + Prefix + "[TaxonOrder] O " +
        //            //       " where O.NameID = " + ID.ToString();
        //            //    this.getDataFromTable(SQL, ref Values);
        //            //}

        //            //GotData = false;
        //            //if (this._ServerConnection.LinkedServer.Length == 0)
        //            //{
        //            //    SQL = "SELECT Taxon + CASE WHEN Seq > 1 THEN ' | ' ELSE '' END AS [Hierarchy], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] " +
        //            //        "(" + ID.ToString() + " , dbo.DefaultProjectID()) ORDER BY Seq DESC";
        //            //    GotData = this.getDataFromTable(SQL, ref Values);
        //            //}
        //            //if (!GotData)
        //            //{
        //            //    SQL = "SELECT TOP 1 h.[HierarchyListCache] AS [Hierarchy] FROM " + Prefix + "[TaxonHierarchy] h where h.NameID = " + ID.ToString() + " AND h.[HierarchyListCache] <> '' ";
        //            //    this.getDataFromTable(SQL, ref Values);
        //            //}

        //            //SQL = "SELECT N.TaxonNameCache + '\r\n' AS Synonym, S.SynType AS [Synonymy type] " +
        //            //    "FROM " + Prefix + "TaxonSynonymy S INNER JOIN " +
        //            //    Prefix + "TaxonName N ON S.SynNameID = N.NameID " +
        //            //    "WHERE (S.IgnoreButKeepForReference = 0)  " +
        //            //    "AND  (S.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //SQL = "SELECT N.TaxonNameCache AS [Accepted name] " +
        //            //        "FROM " + Prefix + "TaxonAcceptedName A INNER JOIN " +
        //            //        Prefix + "TaxonName N ON A.NameID = N.NameID INNER JOIN " +
        //            //        Prefix + "TaxonSynonymy S ON A.NameID = S.SynNameID " +
        //            //        "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0)  " +
        //            //        "AND  (S.NameID = " + ID.ToString() + ")" +
        //            //        "UNION " +
        //            //        "SELECT N.TaxonNameCache AS [Accepted name] " +
        //            //        "FROM " + Prefix + "TaxonAcceptedName A INNER JOIN " +
        //            //        Prefix + "TaxonName N ON A.NameID = N.NameID " +
        //            //        "WHERE (A.IgnoreButKeepForReference = 0)  " +
        //            //        "AND (A.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// CommonName
        //            //SQL = "SELECT C.CommonName AS [Common name], L.DisplayText AS Language " +
        //            //    "FROM " + Prefix + "TaxonCommonName C INNER JOIN " +
        //            //    Prefix + "LanguageCode_Enum L ON C.LanguageCode = L.Code " +
        //            //    "WHERE NameID = " + ID.ToString();
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// Geography
        //            //SQL = "SELECT PlaceNameCache AS Place " +
        //            //    " FROM " + Prefix + "TaxonGeography " +
        //            //    "WHERE NameID = " + ID.ToString();
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// Typification
        //            //SQL = "SELECT T.Typification  " +
        //            //        "FROM " + Prefix + "TaxonNameTypification AS T  " +
        //            //        "WHERE (T.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// Project
        //            //SQL = "SELECT P.Project " +
        //            //    "FROM " + Prefix + "TaxonNameProject T INNER JOIN " +
        //            //    Prefix + "ProjectList P ON T.ProjectID = P.ProjectID " +
        //            //    "WHERE (T.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// List
        //            //SQL = "SELECT P.Project AS List " +
        //            //    "FROM " + Prefix + "TaxonNameList L INNER JOIN " +
        //            //    Prefix + "TaxonNameListProjectProxy P ON L.ProjectID = P.ProjectID " +
        //            //    "WHERE (L.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// Analysis
        //            //SQL = "SELECT P.DisplayText + ': '  + C.DisplayText + case when A.AnalysisValue <> C.DisplayText " +
        //            //        "and A.AnalysisValue <> '' then ' = ' + A.AnalysisValue else case when A.Notes <> '' then ' = ' + " +
        //            //        "cast (A.Notes as nvarchar(4000)) " +
        //            //        "else '' end end AS Analysis  " +
        //            //        "FROM " + Prefix + "TaxonNameListAnalysis AS A INNER JOIN " +
        //            //        Prefix + "TaxonNameListProjectProxy AS P ON A.ProjectID = P.ProjectID INNER JOIN " +
        //            //        Prefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = C.AnalysisID " +
        //            //        "WHERE (A.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// Distribution
        //            //SQL = "SELECT P.DisplayText + ': '  + D.PlaceNameCache AS Distribution  " +
        //            //        "FROM " + Prefix + "TaxonNameListDistribution AS D INNER JOIN " +
        //            //        Prefix + "TaxonNameListProjectProxy AS P ON D.ProjectID = P.ProjectID  " +
        //            //        "WHERE (D.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// CollectionSpecimen
        //            //SQL = "SELECT P.DisplayText + ': '  + S.DisplayText AS [Collection specimen]  " +
        //            //        "FROM " + Prefix + "TaxonNameListCollectionSpecimen AS S INNER JOIN " +
        //            //        Prefix + "TaxonNameListProjectProxy AS P ON S.ProjectID = P.ProjectID  " +
        //            //        "WHERE (S.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //// Reference
        //            //SQL = "SELECT P.DisplayText + ': '  + R.TaxonNameListRefText AS [List reference]  " +
        //            //        "FROM " + Prefix + "TaxonNameListReference AS R INNER JOIN " +
        //            //        Prefix + "TaxonNameListProjectProxy AS P ON R.ProjectID = P.ProjectID  " +
        //            //        "WHERE (R.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);


        //            //SQL = "SELECT D.ExternalDatabaseName + CASE WHEN D.ExternalDatabaseVersion IS NULL OR " +
        //            //    "LEN(D.ExternalDatabaseVersion) " +
        //            //    "> 50 THEN '' ELSE '. Vers.: ' + D.ExternalDatabaseVersion END + CASE WHEN D.ExternalDatabaseAuthors " +
        //            //    "IS NULL THEN '' ELSE '. Aut.: ' + D.ExternalDatabaseAuthors END + '\r\n' AS [External database], " +
        //            //    "I.ExternalNameURI AS [External name URI] " +
        //            //    "FROM  " + Prefix + "TaxonNameExternalID I INNER JOIN " +
        //            //    Prefix + "TaxonNameExternalDatabase D ON I.ExternalDatabaseID = D.ExternalDatabaseID " +
        //            //    "WHERE (I.NameID = " + ID.ToString() + ")";
        //            //this.getDataFromTable(SQL, ref Values);

        //            //if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
        //            //this._UnitValues.Clear();
        //            //foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
        //            //{
        //            //    this._UnitValues.Add(P.Key, P.Value);
        //            //}

        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //    }
        //    return Values;
        //}

        //private System.Data.DataTable DtTaxa(string NameIDs, int ProjectID)
        //{
        //    string SQL = "SELECT MAX(U.BaseURL) + CAST(T.NameID AS VARCHAR) AS _URI, " +
        //        "MAX(U.BaseURL) + CAST(T.NameID AS VARCHAR) AS Link, T.TaxonNameCache AS _DisplayText, T.NameID AS ID, " +
        //        "T.TaxonNameCache AS [Taxonomic name], T1.TaxonNameCache AS Basionym, T.TaxonomicRank AS Rank, " +
        //        "T.GenusOrSupragenericName AS Genus, T.InfragenericEpithet AS [Infrageneric epithet], " +
        //        "T.SpeciesEpithet AS [Species epithet], T.InfraspecificEpithet AS [Infraspecific epithet], " +
        //        "case when T.NonNomenclaturalNameSuffix <> '' then T.NonNomenclaturalNameSuffix else " +
        //        "case when T.CombiningAuthors <> '' then '(' else '' end + T.BasionymAuthors + case when T.CombiningAuthors <> '' then ')' else '' end + " +
        //        "case when T.CombiningAuthors <> '' then ' ' + T.CombiningAuthors else '' end end AS Authors, T.NomenclaturalStatus AS [Nomenclatural status], " +
        //        "case when T.ReferenceTitle is null then '' else T.ReferenceTitle end +  " +
        //        "case when T.Volume is null then '' else '. ' + T.Volume end + case when T.Issue is null then '' else '. ' + T.Issue end +   " +
        //        "case when T.Pages is null then '' else ', ' + T.Pages end +  " +
        //        "case when T.YearOfPubl is null then '' else '. ' + cast(T.YearOfPubl as varchar) end AS Publication, " +
        //        "case when T.IgnoreButKeepForReference = 0 then 'Valid name' else case when MAX(T2.TaxonNameCache) is null  " +
        //        "then 'Is ignored, do not use' else 'Is replaced by: ' + MAX(T2.TaxonNameCache) + ' (ID: ' + cast(MAX(T2.NameID) as varchar) + ')' end end AS [Valid or ignored] " +
        //        "FROM " +
        //        Prefix + "ViewBaseURL AS U, " +
        //        Prefix + "TaxonSynonymy AS S INNER JOIN  " +
        //        Prefix + "TaxonName T2 ON S.SynNameID = T2.NameID RIGHT OUTER JOIN " +
        //        Prefix + "TaxonName T1 RIGHT OUTER JOIN " +
        //        Prefix + "TaxonName AS T ON T1.NameID = T.BasedOnNameID ON S.NameID = T.NameID INNER JOIN " +
        //        Prefix + "TaxonNameProject AS P ON P.NameID = T.NameID AND P.ProjectID = " + ProjectID.ToString() + " LEFT OUTER JOIN " +
        //        Prefix + "TaxonHierarchy AS H ON T.NameID = H.NameID AND H.ProjectID  = " + ProjectID.ToString() + " ON S.NameID = T.NameID INNER JOIN " +
        //        " WHERE (T.NameID IN ( " + NameIDs + ")) " +
        //        " GROUP BY T.NameID, T.TaxonNameCache, T1.TaxonNameCache, T.Issue, T.IgnoreButKeepForReference, T.NomenclaturalStatus, " +
        //        " T.TaxonomicRank,  T.ReferenceTitle, T.Volume, T.Pages, T.YearOfPubl, T.GenusOrSupragenericName, T.InfragenericEpithet " +
        //        ", T.SpeciesEpithet, T.InfraspecificEpithet, T.BasionymAuthors, T.CombiningAuthors, T.NonNomenclaturalNameSuffix";
        //    System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
        //    return dtTaxa;
        //}

        //private System.Data.DataTable DtTaxaHierarchy(string NameIDs, int ProjectID)
        //{
        //    string SQL = "SELECT H.NameID, H.NameParentID, h.[HierarchyListCache] AS [Hierarchy] FROM " + Prefix + "[TaxonHierarchy] h " +
        //        "where h.IgnoreButKeepForReference = 0 and h.NameID IN ( " + NameIDs + ") AND h.ProjectID = " + ProjectID.ToString();
        //    System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
        //    return dtTaxa;
        //}




        #endregion

        public override bool DoesExist(int ID)
        {
            bool Exists = false;
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                try
                {
                    string Prefix = "";
                    if (this._ServerConnection.LinkedServer.Length > 0)
                        Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
                    else
                        Prefix = "dbo.";

                    string SQL = "SELECT COUNT(*) FROM " + Prefix + MainTable() +
                        " AS T WHERE (T.NameID = " + ID.ToString() + ")";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    string testF = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    con.Close();
                    con.Dispose();
                    if (testF != string.Empty && int.Parse(testF) > 0)
                        Exists = true;
                    else
                    {
                        Exists = base.DoesExist(ID);
                    }
                    
                    //if (!Exists)
                    //    Exists = base.DoesExist(ID);
                }
                catch (Exception ex)
                {
                    Exists = base.DoesExist(ID);
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return Exists;
        }

        public string MainTable() { return "TaxonName"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[10];
            QueryDisplayColumns[0].DisplayText = "Taxonomic Name";
            QueryDisplayColumns[0].DisplayColumn = "Display";
            QueryDisplayColumns[0].OrderColumn = "TaxonNameCache";

            QueryDisplayColumns[1].DisplayText = "Epithet";
            QueryDisplayColumns[1].DisplayColumn = "DisplayEpithet";

            QueryDisplayColumns[2].DisplayText = "ID";
            QueryDisplayColumns[2].DisplayColumn = "NameID";

            QueryDisplayColumns[3].DisplayText = "Genus";
            QueryDisplayColumns[3].DisplayColumn = "GenusOrSupragenericName";

            QueryDisplayColumns[4].DisplayText = "Rank";
            QueryDisplayColumns[4].DisplayColumn = "TaxonomicRank";

            QueryDisplayColumns[5].DisplayText = "Reference";
            QueryDisplayColumns[5].DisplayColumn = "ReferenceTitle";

            QueryDisplayColumns[6].DisplayText = "Year";
            QueryDisplayColumns[6].DisplayColumn = "YearOfPubl";

            QueryDisplayColumns[7].DisplayText = "Family";
            QueryDisplayColumns[7].DisplayColumn = "Family";
            QueryDisplayColumns[7].TableName = "TaxonFamily";

            QueryDisplayColumns[8].DisplayText = "Order";
            QueryDisplayColumns[8].DisplayColumn = "Order";
            QueryDisplayColumns[8].TableName = "TaxonOrder";

            QueryDisplayColumns[9].DisplayText = "Common Name";
            QueryDisplayColumns[9].DisplayColumn = "CommonName";
            QueryDisplayColumns[9].TableName = "TaxonCommonName";

            for (int i = 0; i < QueryDisplayColumns.Length; i++)
            {
                if (QueryDisplayColumns[i].OrderColumn == null)
                    QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].IdentityColumn == null)
                    QueryDisplayColumns[i].IdentityColumn = "NameID";
                if (QueryDisplayColumns[i].TableName == null)
                    QueryDisplayColumns[i].TableName = "TaxonName_Indicated";
                if (QueryDisplayColumns[i].TipText == null)
                    QueryDisplayColumns[i].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(QueryDisplayColumns[i].TableName, QueryDisplayColumns[i].DisplayColumn);
                QueryDisplayColumns[i].Module = "DiversityTaxonNames";
            }

            //QueryDisplayColumns[0].DisplayText = "Taxonomic Name indicated";
            //QueryDisplayColumns[0].DisplayColumn = "Display";
            //QueryDisplayColumns[0].OrderColumn = "TaxonNameCache";
            //QueryDisplayColumns[0].IdentityColumn = "NameID";
            //QueryDisplayColumns[0].TableName = "TaxonName_Indicated";

            //QueryDisplayColumns[1].DisplayText = "Taxonomic Name";
            //QueryDisplayColumns[1].DisplayColumn = "Display";
            //QueryDisplayColumns[1].OrderColumn = "TaxonNameCache";
            //QueryDisplayColumns[1].IdentityColumn = "NameID";
            //QueryDisplayColumns[1].TableName = "TaxonName_Core";

            //QueryDisplayColumns[2].DisplayText = "Epithet indicated";
            //QueryDisplayColumns[2].DisplayColumn = "DisplayEpithet";
            //QueryDisplayColumns[2].OrderColumn = "DisplayEpithet";
            //QueryDisplayColumns[2].IdentityColumn = "NameID";
            //QueryDisplayColumns[2].TableName = "TaxonName_Indicated";

            //QueryDisplayColumns[3].DisplayText = "Epithet";
            //QueryDisplayColumns[3].DisplayColumn = "DisplayEpithet";
            //QueryDisplayColumns[3].OrderColumn = "DisplayEpithet";
            //QueryDisplayColumns[3].IdentityColumn = "NameID";
            //QueryDisplayColumns[3].TableName = "TaxonName_Core";

            //QueryDisplayColumns[4].DisplayText = "ID";
            //QueryDisplayColumns[4].DisplayColumn = "NameID";
            //QueryDisplayColumns[4].OrderColumn = "NameID";
            //QueryDisplayColumns[4].IdentityColumn = "NameID";
            //QueryDisplayColumns[4].TableName = "TaxonName_Indicated";

            return QueryDisplayColumns;
        }

        private List<DiversityWorkbench.QueryCondition> _cachedQueryConditions;

        public List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            // TODO Ariane maybe we can cache the QueryConditions?
            //if (_cachedQueryConditions == null)
            //{
            _cachedQueryConditions = GenerateQueryConditions();
            //}
            return _cachedQueryConditions;
        }
        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> GenerateQueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
            string SQL = "";
            string Description = "";
            try
            {
                string ConnectionString = this._ServerConnection.ConnectionString;
#if DEBUG
                if (this._ServerConnection.DatabaseName.IndexOf(this._ServerConnection.ModuleName) == -1)
                    return QueryConditions;
#endif
#if !UseGetDataFunction
                if (ConnectionString.Length == 0)
                {
                    if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityTaxonNames"))
                    {
                        if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList().Count > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].ServerConnectionList())
                            {
                                ConnectionString = KV.Value.ConnectionString;
                                if (ConnectionString.Length > 0)
                                    break;
                            }
                        }
                    }
                }
#endif

                //if (ConnectionString.Length == 0)
                // Toni 20210727 Markus, please Check!!! Remote query to linked server mixes up with calling database when statements above are uncommented!!!
                ConnectionString = this.ServerConnection.ConnectionString;
#if !UseGetDataFunction
                if (!ConnectionString.Contains(this.ServerConnection.DatabaseName))
                {
                    if (this.ServerConnection.DatabaseServer.Length > 0)
                    {
                        this.ServerConnection.setConnection("");
                    }
                    ConnectionString = this.ServerConnection.ConnectionString;
                    if (ConnectionString.Length == 0)
                        return QueryConditions;
                }
#endif


                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);

                bool LokalIsLinkedServer = IsLinkedServer; // Vermeidet Verwirrung, wo lese ich den Linked Server???
                string LocalPreFix = Prefix;
                if (DiversityWorkbench.Settings.ModuleName != "DiversityTaxonNames")
                {
                    if (LokalIsLinkedServer) // Toni 20210729 sieh wirr au, aber scheint zu stimmen....
                        LocalPreFix = "[" + this._ServerConnection.LinkedServer + "]." + this.ServerConnection.DatabaseName + ".dbo."; // Kann man das nicht weglassen???
                    else LocalPreFix = "dbo.";
                }

                #region Project

                System.Data.DataTable dtProject = new System.Data.DataTable();
                try
                {
                    SQL = "SELECT ProjectList.ProjectID AS Value, ProjectList.Project AS Display FROM " + LocalPreFix + "ProjectList " +
                       "ORDER BY Display";
#if UseGetDataFunction
                    this.GetData(ref dtProject, SQL);
#else
                    a.SelectCommand.CommandText = SQL;
                    a.Fill(dtProject);
#endif
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

                System.Collections.Generic.List<DiversityWorkbench.QueryField> Fields = new List<QueryField>();
                DiversityWorkbench.QueryField CSANr = new QueryField("TaxonNameProject", "ProjectID", "NameID");
                DiversityWorkbench.QueryField CPANr = new QueryField("TaxonName_Indicated", "ProjectID", "NameID");
                Fields.Add(CSANr);
                Fields.Add(CPANr);
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameProject", "ProjectID");
                this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, Fields, "Project", "Project", "Project", Description, dtProject, true, true);
                this._QueryConditionProject.ServerConnection = this._ServerConnection;
                this._QueryConditionProject.Table = "TaxonNameProject";
                QueryConditions.Add(this._QueryConditionProject);

                #endregion

                #region TaxonName

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "TaxonNameCache");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "TaxonName", "NameID", "TaxonNameCache", "Taxonomic name", "Name", "Taxonomic name", Description);
                QueryConditions.Add(q0);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "NameID");
                DiversityWorkbench.QueryCondition q10 = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "NameID", "Taxonomic name", "NameID", "NameID", Description, false, false, true, false);
                QueryConditions.Add(q10);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "IgnoreButKeepForReference");
                DiversityWorkbench.QueryCondition qIgnoreButKeepForReference = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "IgnoreButKeepForReference", "Taxonomic name", "Ignored", "Ignored but kept for reference", Description, false, false, false, true);
                QueryConditions.Add(qIgnoreButKeepForReference);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "GenusOrSupragenericName");
                DiversityWorkbench.QueryCondition qGenusOrSupragenericName = new DiversityWorkbench.QueryCondition(true, "TaxonName", "NameID", "GenusOrSupragenericName", "Taxonomic name", "Genus", "Genus or suprageneric name", Description);
                QueryConditions.Add(qGenusOrSupragenericName);


                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "InfragenericEpithet");
                DiversityWorkbench.QueryCondition qInfragenericEpithet = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "InfragenericEpithet", "Taxonomic name", "Infra.gen.", "Infrageneric epithet", Description);
                QueryConditions.Add(qInfragenericEpithet);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "SpeciesEpithet");
                DiversityWorkbench.QueryCondition qSpecies = new DiversityWorkbench.QueryCondition(true, "TaxonName", "NameID", "SpeciesEpithet", "Taxonomic name", "Species", "Species epithet", Description);
                QueryConditions.Add(qSpecies);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "InfraspecificEpithet");
                DiversityWorkbench.QueryCondition qInfraspecificEpithet = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "InfraspecificEpithet", "Taxonomic name", "Infraspec.", "Infraspecific epithet", Description);
                QueryConditions.Add(qInfraspecificEpithet);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "IsHybrid");
                DiversityWorkbench.QueryCondition q4 = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "IsHybrid", "Taxonomic name", "Hybrid", "Is a hybrid", Description, false, false, false, true);
                QueryConditions.Add(q4);

                System.Data.DataTable dtCreationType = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code AS [Value], ParentCode AS [ParentValue], DisplayText AS Display, DisplayOrder " +
                    "FROM " + LocalPreFix + "TaxonNameCreationType_Enum " +
                    "ORDER BY Display ";
#if UseGetDataFunction
                this.GetData(ref dtCreationType, SQL);
#else

                Microsoft.Data.SqlClient.SqlDataAdapter aCreationType = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aCreationType.Fill(dtCreationType); }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
#endif
                if (dtCreationType.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    System.Data.DataColumn DisplayOrder = new System.Data.DataColumn("DisplayOrder");
                    dtCreationType.Columns.Add(Value);
                    dtCreationType.Columns.Add(ParentValue);
                    dtCreationType.Columns.Add(Display);
                    dtCreationType.Columns.Add(DisplayOrder);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FFCreationType = new List<QueryField>();
                DiversityWorkbench.QueryField CS_CreationType = new QueryField("TaxonName", "CreationType", "NameID");
                FFCreationType.Add(CS_CreationType);
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "CreationType");
                DiversityWorkbench.QueryCondition qCreationType = new DiversityWorkbench.QueryCondition(true, FFCreationType, "Taxonomic name", "CreationType", "Creation type", Description, dtCreationType, false, "DisplayOrder", "ParentValue", "Display", "Value");
                QueryConditions.Add(qCreationType);



                System.Data.DataTable dtRank = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code AS [Value], ParentCode AS [ParentValue], DisplayText AS Display, DisplayOrder " +
                    "FROM " + LocalPreFix + "TaxonNameTaxonomicRank_Enum " +
                    "ORDER BY Display ";
#if UseGetDataFunction
                this.GetData(ref dtRank, SQL);
#else
                Microsoft.Data.SqlClient.SqlDataAdapter aRank = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aRank.Fill(dtRank); }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
#endif
                if (dtRank.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    System.Data.DataColumn DisplayOrder = new System.Data.DataColumn("DisplayOrder");
                    dtRank.Columns.Add(Value);
                    dtRank.Columns.Add(ParentValue);
                    dtRank.Columns.Add(Display);
                    dtRank.Columns.Add(DisplayOrder);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FF = new List<QueryField>();
                DiversityWorkbench.QueryField CS_C = new QueryField("TaxonName", "TaxonomicRank", "NameID");
                FF.Add(CS_C);
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "TaxonomicRank");
                DiversityWorkbench.QueryCondition qRank = new DiversityWorkbench.QueryCondition(true, FF, "Taxonomic name", "Rank", "Taxonomic rank", Description, dtRank, false, "DisplayOrder", "ParentValue", "Display", "Value");
                QueryConditions.Add(qRank);


                // Basionym
                SQL = " FROM " + LocalPreFix + "TaxonName INNER JOIN " + LocalPreFix + "TaxonName AS B ON TaxonName.NameID = B.BasedOnNameID ";

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "BasedOnNameID");
                DiversityWorkbench.QueryCondition qBasionym = new DiversityWorkbench.QueryCondition(false, "TaxonName", "B.NameID", true, SQL, "TaxonNameCache", "Taxonomic name", "Basionym", "Basionym", Description, "BasedOnNameID");
                QueryConditions.Add(qBasionym);

                // NomenclaturalStatus
                System.Data.DataTable dtNomenclaturalStatus = new System.Data.DataTable();
                if (ConnectionString.Length > 0)
                {
                    SQL = "SELECT Code AS [Value], Description AS Display, DisplayOrder FROM " + LocalPreFix + "TaxonNameNomenclaturalStatus_Enum " +
                        "UNION " +
                        "SELECT NULL AS [Value], NULL AS Display, NULL AS DisplayOrder " +
                        "ORDER BY DisplayOrder";
#if UseGetDataFunction
                    this.GetData(ref dtNomenclaturalStatus, SQL);
#else
                    Microsoft.Data.SqlClient.SqlDataAdapter aStatus = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        try { aStatus.Fill(dtNomenclaturalStatus); }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
#endif
                }
                if (dtNomenclaturalStatus.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtNomenclaturalStatus.Columns.Add(Value);
                    dtNomenclaturalStatus.Columns.Add(Display);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "NomenclaturalStatus");
                DiversityWorkbench.QueryCondition qNomenclaturalStatus = new DiversityWorkbench.QueryCondition(true, "TaxonName", "NameID", "NomenclaturalStatus", "Taxonomic name", "Status", "Nomenclatural status", Description, dtNomenclaturalStatus, false);
                QueryConditions.Add(qNomenclaturalStatus);

                // Authors
                #region Authors

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "BasionymAuthors");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "TaxonName", "NameID", "BasionymAuthors", "Authors", "Bas.auth.", "Authors of the basionym", Description);
                QueryConditions.Add(q1);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "IsRecombination");
                DiversityWorkbench.QueryCondition qIsRecombination = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "IsRecombination", "Authors", "Recomb.", "Is a recombination", Description, false, false, false, true);
                QueryConditions.Add(qIsRecombination);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "CombiningAuthors");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "TaxonName", "NameID", "CombiningAuthors", "Authors", "Comb.auth.", "Combining authors", Description);
                QueryConditions.Add(q2);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "PublishingAuthors");
                DiversityWorkbench.QueryCondition qPublishingAuthors = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "PublishingAuthors", "Authors", "Pub.auth.", "Publishing authors", Description);
                QueryConditions.Add(qPublishingAuthors);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "NonNomenclaturalNameSuffix");
                DiversityWorkbench.QueryCondition q7 = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "NonNomenclaturalNameSuffix", "Authors", "Name suff.", "NonNomenclaturalNameSuffix", Description);
                QueryConditions.Add(q7);

                #endregion

                // Publication
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "ReferenceTitle");
                DiversityWorkbench.QueryCondition q5 = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "ReferenceTitle", "Publication", "Reference", "Title of the reference", Description);
                QueryConditions.Add(q5);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "YearOfPubl");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "YearOfPubl", "Publication", "Year of publ.", "Year of publication", Description, false, true, false, false);
                QueryConditions.Add(q3);

                // Revision
                System.Data.DataTable dtRevisionLevel = new System.Data.DataTable();
                if (this.ServerConnection.ConnectionString.Length > 0)
                {
                    SQL = "SELECT Code AS [Value], Description AS Display, DisplayOrder FROM " + LocalPreFix + "TaxonNameRevisionLevel_Enum " +
                        "UNION " +
                        "SELECT NULL AS [Value], NULL AS Display, NULL AS DisplayOrder " +
                        "ORDER BY DisplayOrder";
#if UseGetDataFunction
                    this.GetData(ref dtRevisionLevel, SQL);
#else
                    Microsoft.Data.SqlClient.SqlDataAdapter aRevision = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        try { aRevision.Fill(dtRevisionLevel); }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
#endif
                }
                if (dtRevisionLevel.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtRevisionLevel.Columns.Add(Value);
                    dtRevisionLevel.Columns.Add(Display);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "RevisionLevel");
                DiversityWorkbench.QueryCondition q8 = new DiversityWorkbench.QueryCondition(true, "TaxonName", "NameID", "RevisionLevel", "Revision", "Level", "Revision level", Description, dtRevisionLevel, false);
                QueryConditions.Add(q8);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "Protologue");
                DiversityWorkbench.QueryCondition qProtologue = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "Protologue", "Taxonomic name", "Protologue", "Protologue", Description);
                QueryConditions.Add(qProtologue);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "OriginalOrthography");
                DiversityWorkbench.QueryCondition qOriginalOrthography = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "OriginalOrthography", "Taxonomic name", "Ori.Ortho.", "Original orthography", Description);
                QueryConditions.Add(qOriginalOrthography);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "NomenclaturalComment");
                DiversityWorkbench.QueryCondition qNomenclaturalComment = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "NomenclaturalComment", "Taxonomic name", "Nomencl.comment", "Nomenclatural comment", Description);
                QueryConditions.Add(qNomenclaturalComment);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "TypistNotes");
                DiversityWorkbench.QueryCondition qTypistNotes = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "TypistNotes", "Taxonomic name", "Notes", "Typist notes", Description);
                QueryConditions.Add(qTypistNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "DataWithholdingReason", "Taxonomic name", "Withhold.", "Data withholding reason", Description);
                QueryConditions.Add(qDataWithholdingReason);

                System.Data.DataTable dtUserInserted = new System.Data.DataTable();
                if (!LokalIsLinkedServer)
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                        "SELECT DISTINCT T.LogInsertedBy AS Value, CASE WHEN U.CombinedNameCache IS NULL THEN T.LogInsertedBy ELSE U.CombinedNameCache END AS Display " +
                        "FROM " + LocalPreFix + "TaxonName AS T LEFT OUTER JOIN " + LocalPreFix + "UserProxy AS U ON T.LogInsertedBy = U.LoginName " +
                        "ORDER BY Display";
                else
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                        "SELECT DISTINCT T.LogInsertedBy AS Value, T.LogInsertedBy AS Display " +
                        "FROM " + LocalPreFix + "TaxonName AS T " +
                        "ORDER BY Display";
#if UseGetDataFunction
                this.GetData(ref dtUserInserted, SQL);
#else
                Microsoft.Data.SqlClient.SqlDataAdapter aUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aUser.Fill(dtUserInserted); }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
#endif
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "LogInsertedBy");
                DiversityWorkbench.QueryCondition qLogCreatedBy = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "LogInsertedBy", "Taxonomic name", "Creat. by", "The user that created the dataset", Description, dtUserInserted, false);
                QueryConditions.Add(qLogCreatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "LogInsertedWhen");
                DiversityWorkbench.QueryCondition qLogCreatedWhen = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "LogInsertedWhen", "Taxonomic name", "Creat. date", "The date when the dataset was created", Description, true);
                QueryConditions.Add(qLogCreatedWhen);

                System.Data.DataTable dtUserUpdated = new System.Data.DataTable();
                if (!LokalIsLinkedServer)
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                        "SELECT DISTINCT T.LogUpdatedBy AS Value, CASE WHEN U.CombinedNameCache IS NULL THEN T.LogUpdatedBy ELSE U.CombinedNameCache END AS Display " +
                        "FROM " + LocalPreFix + "TaxonName AS T LEFT OUTER JOIN " + LocalPreFix + "UserProxy AS U ON T.LogUpdatedBy = U.LoginName " +
                        "ORDER BY Display";
                else
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                        "SELECT DISTINCT T.LogUpdatedBy AS Value, T.LogUpdatedBy AS Display " +
                        "FROM " + LocalPreFix + "TaxonName AS T " +
                        "ORDER BY Display";
#if UseGetDataFunction
                this.GetData(ref dtUserUpdated, SQL);
#else
                aUser.SelectCommand.CommandText = SQL;
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aUser.Fill(dtUserUpdated); }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
#endif
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "LogUpdatedBy");
                DiversityWorkbench.QueryCondition qLogUpdatedBy = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "LogUpdatedBy", "Taxonomic name", "Changed by", "The last user that changed the dataset", Description, dtUserUpdated, false);
                QueryConditions.Add(qLogUpdatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "LogUpdatedWhen");
                DiversityWorkbench.QueryCondition qLogUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "LogUpdatedWhen", "Taxonomic name", "Changed at", "The last date when the dataset was changed", Description, true);
                QueryConditions.Add(qLogUpdatedWhen);

                #endregion

                #region Typification

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameTypification", "TypificationReferenceTitle");
                DiversityWorkbench.QueryCondition qTypificationReferenceTitle = new DiversityWorkbench.QueryCondition(false, "TaxonNameTypification", "NameID", "TypificationReferenceTitle", "Typification", "Reference", "Ref.", Description);
                QueryConditions.Add(qTypificationReferenceTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameTypification", "Typification");
                DiversityWorkbench.QueryCondition qTypification = new DiversityWorkbench.QueryCondition(false, "TaxonNameTypification", "NameID", "Typification", "Typification", "Typification", "Typification", Description);
                QueryConditions.Add(qTypification);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameTypification", "TypeSubstrate");
                DiversityWorkbench.QueryCondition qTypeSubstrate = new DiversityWorkbench.QueryCondition(false, "TaxonNameTypification", "NameID", "TypeSubstrate", "Typification", "Type substrate", "Substrate", Description);
                QueryConditions.Add(qTypeSubstrate);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameTypification", "TypeLocality");
                DiversityWorkbench.QueryCondition qTypeLocality = new DiversityWorkbench.QueryCondition(false, "TaxonNameTypification", "NameID", "TypeLocality", "Typification", "Type locality", "Locality", Description);
                QueryConditions.Add(qTypeLocality);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "TypificationDetails");
                DiversityWorkbench.QueryCondition qTypificationDetails = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "TypificationDetails", "Typification", "Details", "Typification details", Description);
                QueryConditions.Add(qTypificationDetails);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonName", "TypificationNotes");
                DiversityWorkbench.QueryCondition qTypificationNotes = new DiversityWorkbench.QueryCondition(false, "TaxonName", "NameID", "TypificationNotes", "Typification", "Notes", "Typification notes", Description);
                QueryConditions.Add(qTypificationNotes);

                #endregion

                #region ACCEPTED NAME

                Description = "If any accepted name is present";
                DiversityWorkbench.QueryCondition qAcceptedName = new DiversityWorkbench.QueryCondition(true, "TaxonAcceptedName", "NameID", "Accepted name", "Presence", "Accepted name present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qAcceptedName);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "IgnoreButKeepForReference");
                DiversityWorkbench.QueryCondition qIgnoreButKeepForReferenceAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "IgnoreButKeepForReference", "Accepted name", "Ignored", "Ignored but kept for reference", Description, false, false, false, true);
                qIgnoreButKeepForReferenceAN.RestrictToProject = true;
                QueryConditions.Add(qIgnoreButKeepForReferenceAN);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "ConceptSuffix");
                DiversityWorkbench.QueryCondition qConceptSuffixAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "ConceptSuffix", "Accepted name", "Con.suff.", "Concept suffix", Description);
                qConceptSuffixAN.RestrictToProject = true;
                QueryConditions.Add(qConceptSuffixAN);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "ConceptNotes");
                DiversityWorkbench.QueryCondition qConceptNotesAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "ConceptNotes", "Accepted name", "Con.notes", "Concept notes", Description);
                qConceptNotesAN.RestrictToProject = true;
                QueryConditions.Add(qConceptNotesAN);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "RefText");
                DiversityWorkbench.QueryCondition qRefTextAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "RefText", "Accepted name", "Reference", "Reference", Description);
                qRefTextAN.RestrictToProject = true;
                QueryConditions.Add(qRefTextAN);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "TypistsNotes");
                DiversityWorkbench.QueryCondition qTypistsNotesAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "TypistsNotes", "Accepted name", "Notes", "Typists notes", Description);
                qTypistsNotesAN.RestrictToProject = true;
                QueryConditions.Add(qTypistsNotesAN);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "LogInsertedBy");
                DiversityWorkbench.QueryCondition qLogCreatedByAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "LogInsertedBy", "Accepted name", "Creat. by", "The user that created the dataset", Description, dtUserInserted, false);
                QueryConditions.Add(qLogCreatedByAN);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "LogInsertedWhen");
                DiversityWorkbench.QueryCondition qLogCreatedWhenAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "LogInsertedWhen", "Accepted name", "Creat. date", "The date when the dataset was created", Description, true);
                QueryConditions.Add(qLogCreatedWhenAN);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "LogUpdatedBy");
                DiversityWorkbench.QueryCondition qLogUpdatedByAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "LogUpdatedBy", "Accepted name", "Changed by", "The last user that changed the dataset", Description, dtUserUpdated, false);
                QueryConditions.Add(qLogUpdatedByAN);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonAcceptedName", "LogUpdatedWhen");
                DiversityWorkbench.QueryCondition qLogUpdatedWhenAN = new DiversityWorkbench.QueryCondition(false, "TaxonAcceptedName", "NameID", "LogUpdatedWhen", "Accepted name", "Changed at", "The last date when the dataset was changed", Description, true);
                QueryConditions.Add(qLogUpdatedWhenAN);

                #endregion

                #region Synonymy

                // SYNONYMY
                Description = "If any synonym is present";
                DiversityWorkbench.QueryCondition qSynonym = new DiversityWorkbench.QueryCondition(false, "TaxonSynonymy", "NameID", "Synonymy", "Presence", "Synonym present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qSynonym);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonSynonymy", "IgnoreButKeepForReference");
                DiversityWorkbench.QueryCondition qIgnoreSynonym = new DiversityWorkbench.QueryCondition(false, "TaxonSynonymy", "NameID", "IgnoreButKeepForReference", "Synonymy", "Ignored", "Ignored but kept for reference", Description, false, false, false, true);
                qIgnoreSynonym.RestrictToProject = true;
                QueryConditions.Add(qIgnoreSynonym);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonSynonymy", "ConceptSuffix");
                DiversityWorkbench.QueryCondition qConceptSuffix = new DiversityWorkbench.QueryCondition(false, "TaxonSynonymy", "NameID", "ConceptSuffix", "Synonymy", "Con.suff.", "Concept suffix", Description);
                qConceptSuffix.RestrictToProject = true;
                QueryConditions.Add(qConceptSuffix);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonSynonymy", "ConceptNotes");
                DiversityWorkbench.QueryCondition qConceptNotes = new DiversityWorkbench.QueryCondition(false, "TaxonSynonymy", "NameID", "ConceptNotes", "Synonymy", "Con.notes", "Concept notes", Description);
                qConceptNotes.RestrictToProject = true;
                QueryConditions.Add(qConceptNotes);

                System.Data.DataTable dtSynType = new System.Data.DataTable();
                if (ConnectionString.Length > 0)
                {
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                        "SELECT Code AS [Value], DisplayText AS Display " +
                        "FROM " + LocalPreFix + "TaxonNameSynonymisationType_Enum " +
                        "ORDER BY Display ";
#if UseGetDataFunction
                    this.GetData(ref dtSynType, SQL);
#else
                    Microsoft.Data.SqlClient.SqlDataAdapter aSynType = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        try { aSynType.Fill(dtSynType); }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
#endif
                }
                if (dtSynType.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtSynType.Columns.Add(Value);
                    dtSynType.Columns.Add(Display);
                }
                DiversityWorkbench.QueryCondition qSynType = new DiversityWorkbench.QueryCondition(false, "TaxonSynonymy", "NameID", "SynType", "Synonymy", "Syn. type", "Synonymy type", Description, dtSynType, false);
                qSynType.RestrictToProject = true;
                QueryConditions.Add(qSynType);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonSynonymy", "SynRefText");
                DiversityWorkbench.QueryCondition qRefTextSyn = new DiversityWorkbench.QueryCondition(false, "TaxonSynonymy", "NameID", "SynRefText", "Synonymy", "Reference", "Reference", Description);
                qRefTextSyn.RestrictToProject = true;
                QueryConditions.Add(qRefTextSyn);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonSynonymy", "TypistsNotes");
                DiversityWorkbench.QueryCondition qTypistsNotesSyn = new DiversityWorkbench.QueryCondition(false, "TaxonSynonymy", "NameID", "TypistsNotes", "Synonymy", "Notes", "Typists notes", Description);
                qTypistsNotesSyn.RestrictToProject = true;
                QueryConditions.Add(qTypistsNotesSyn);

                #endregion

                #region COMMON NAME
                // COMMON NAME
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonCommonName", "CommonName");
                DiversityWorkbench.QueryCondition qCommonName = new DiversityWorkbench.QueryCondition(false, "TaxonCommonName", "NameID", "CommonName", "Common name", "Name", "Common name", Description);
                QueryConditions.Add(qCommonName);

                System.Data.DataTable dtLanguageCode = new System.Data.DataTable();
                if (ConnectionString.Length > 0)
                {
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                        "SELECT Code AS [Value], DisplayText AS Display " +
                        "FROM " + LocalPreFix + "LanguageCode_Enum " +
                        "ORDER BY Display ";
#if UseGetDataFunction
                    this.GetData(ref dtLanguageCode, SQL);
#else
                    Microsoft.Data.SqlClient.SqlDataAdapter aLanguageCode = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        try { aLanguageCode.Fill(dtLanguageCode); }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
#endif
                }
                if (dtLanguageCode.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtLanguageCode.Columns.Add(Value);
                    dtLanguageCode.Columns.Add(Display);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonCommonName", "LanguageCode");
                DiversityWorkbench.QueryCondition qLanguageCode = new DiversityWorkbench.QueryCondition(false, "TaxonCommonName", "NameID", "LanguageCode", "Common name", "Lang.", "Language code", Description, dtLanguageCode, false);
                QueryConditions.Add(qLanguageCode);

                #endregion

                #region Geography

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonGeography", "PlaceNameCache");
                DiversityWorkbench.QueryCondition qPlaceNameCache = new DiversityWorkbench.QueryCondition(true, "TaxonGeography", "NameID", "PlaceNameCache", "Geography", "Place", "Place name", Description);
                QueryConditions.Add(qPlaceNameCache);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonGeography", "PlaceURI");
                DiversityWorkbench.QueryCondition qPlaceURI = new DiversityWorkbench.QueryCondition(false, "TaxonGeography", "NameID", "PlaceURI", "Geography", "URI", "URI", Description);
                QueryConditions.Add(qPlaceURI);

                #endregion

                #region References
                // References
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameReference", "TaxonNameRefText");
                DiversityWorkbench.QueryCondition qTaxonNameReference = new DiversityWorkbench.QueryCondition(false, "TaxonNameReference", "NameID", "TaxonNameRefText", "Reference", "Reference", "Reference", Description);
                QueryConditions.Add(qTaxonNameReference);

                #endregion

                #region TaxonNameResource
                // Resource
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameResource", "URI");
                DiversityWorkbench.QueryCondition qTaxonNameResource = new DiversityWorkbench.QueryCondition(false, "TaxonNameResource", "NameID", "URI", "Resource", "Resource", "Resource", Description);
                QueryConditions.Add(qTaxonNameResource);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameResource", "Title");
                DiversityWorkbench.QueryCondition qTaxonNameResourceTitle = new DiversityWorkbench.QueryCondition(false, "TaxonNameResource", "NameID", "Title", "Resource", "Title", "Resource title", Description);
                QueryConditions.Add(qTaxonNameResourceTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameResource", "Notes");
                DiversityWorkbench.QueryCondition qTaxonNameResourceNotes = new DiversityWorkbench.QueryCondition(false, "TaxonNameResource", "NameID", "Notes", "Resource", "Notes", "Resource notes", Description);
                QueryConditions.Add(qTaxonNameResourceNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameResource", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qTaxonNameResourceDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "TaxonNameResource", "NameID", "DataWithholdingReason", "Resource", "Withholding", "Resource withholding reason", Description);
                QueryConditions.Add(qTaxonNameResourceDataWithholdingReason);

                #endregion

                #region EXTERNAL DATABASE

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameExternalID", "ExternalNameURI");
                DiversityWorkbench.QueryCondition qExternalNameURI = new DiversityWorkbench.QueryCondition(false, "TaxonNameExternalID", "NameID", "ExternalNameURI", "External source", "Ext. URI", "External URI", Description);
                QueryConditions.Add(qExternalNameURI);

                System.Data.DataTable dtExternalDB = new System.Data.DataTable();
                if (ConnectionString.Length > 0)
                {
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                        "SELECT ExternalDatabaseID, ExternalDatabaseName + CASE WHEN [ExternalDatabaseVersion] IS NULL " +
                        "THEN '' ELSE '. Vers.: ' + [ExternalDatabaseVersion] END + CASE WHEN [ExternalDatabaseAuthors] IS NULL " +
                        "THEN '' ELSE '. Aut.: ' + [ExternalDatabaseAuthors] END + CASE WHEN [ExternalDatabaseInstitution] IS NULL " +
                        "THEN '' ELSE ' Inst.: ' + [ExternalDatabaseInstitution] END AS Display " +
                        "FROM " + LocalPreFix + "TaxonNameExternalDatabase " +
                        "ORDER BY Display ";
#if UseGetDataFunction
                    this.GetData(ref dtExternalDB, SQL);
#else
                    Microsoft.Data.SqlClient.SqlDataAdapter aExternalDB = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        try { aExternalDB.Fill(dtExternalDB); }
                        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    }
#endif
                }
                if (dtExternalDB.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtExternalDB.Columns.Add(Value);
                    dtExternalDB.Columns.Add(Display);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonCommonName", "LanguageCode");
                DiversityWorkbench.QueryCondition qExternalDB = new DiversityWorkbench.QueryCondition(false, "TaxonNameExternalID", "NameID", "ExternalDatabaseID", "External source", "Ext.DB", "External database", Description, dtExternalDB, false);
                QueryConditions.Add(qExternalDB);

                #endregion

                #region Taxon list

                Description = "If any list is present";
#if DEBUG
                //DiversityWorkbench.QueryCondition qListPresent = new DiversityWorkbench.QueryCondition(true, "TaxonNameList_Core", "NameID", "Taxon list", "Presence", "Taxon list present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                //QueryConditions.Add(qListPresent);
#else
#endif
                DiversityWorkbench.QueryCondition qListPresent = new DiversityWorkbench.QueryCondition(true, "TaxonNameList", "NameID", "Taxon list", "Presence", "Taxon list present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qListPresent);

                bool ContainsTaxonList = true;
                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameList", "ProjectID");
                // not for linked server
                if (this._ServerConnection.LinkedServer != null && this._ServerConnection.LinkedServer.Length > 0)
                {
                    //SQL = "SELECT COUNT(*) FROM TaxonNameList";
                    Description = "Each project may contain one taxon list. Refers to the common project definition in the DiversityProjects module.";
                }
                if (Description.Length == 0) ContainsTaxonList = false;
                if (ContainsTaxonList)
                {
                    System.Data.DataTable dtNameLists = new System.Data.DataTable();
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS Value, ";
                    if (this._ServerConnection.LinkedServer.Length > 0)
                        SQL += "Project AS Display FROM " + LocalPreFix + "TaxonNameListProjectProxy ";
                    else
                        SQL += " Datasource AS Display FROM dbo.TaxonListsForUser(USER_NAME()) ";
                    SQL += "ORDER BY Display";
#if UseGetDataFunction
                    this.GetData(ref dtNameLists, SQL);
#else
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dtNameLists);
#endif

#if DEBUG
                    //DiversityWorkbench.QueryCondition qTaxonNameList = new DiversityWorkbench.QueryCondition(false, "TaxonNameList_Core", "NameID", "ListID", "Taxon list", "Tax.list", "Taxon list", Description, dtNameLists, true);
                    //QueryConditions.Add(qTaxonNameList);
#else
#endif
                    DiversityWorkbench.QueryCondition qTaxonNameList = new DiversityWorkbench.QueryCondition(false, "TaxonNameList", "NameID", "ProjectID", "Taxon list", "Tax.list", "Taxon list", Description, dtNameLists, true);
                    QueryConditions.Add(qTaxonNameList);

                    if (this._ServerConnection.LinkedServer != null && this._ServerConnection.LinkedServer.Length > 0)
                        Description = "Free text, esp. where a TaxonNameListRefURI is missing. Source publication where distribution is published (not publication of name!)";
                    else
                        Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameListReference", "TaxonNameListRefText");
                    DiversityWorkbench.QueryCondition qTaxonNameListRefText = new DiversityWorkbench.QueryCondition(false, "TaxonNameListReference", "NameID", "TaxonNameListRefText", "Taxon list", "Ref.", "Reference", Description);
                    QueryConditions.Add(qTaxonNameListRefText);

                    if (this._ServerConnection.LinkedServer != null && this._ServerConnection.LinkedServer.Length > 0)
                        Description = "The name of the place";
                    else
                        Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameListDistribution", "PlaceNameCache");
                    DiversityWorkbench.QueryCondition qDistributionPlaceNameCache = new DiversityWorkbench.QueryCondition(false, "TaxonNameListDistribution", "NameID", "PlaceNameCache", "Taxon list", "Place", "Place name", Description);
                    QueryConditions.Add(qDistributionPlaceNameCache);

                    System.Data.DataTable dtAnalysis = new System.Data.DataTable();
                    if (ConnectionString.Length > 0)
                    {
                        SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                            "SELECT AnalysisID AS Value, DisplayText AS Display " +
                            "FROM " + LocalPreFix + "TaxonNameListAnalysisCategory " +
                            "ORDER BY Display ";
#if UseGetDataFunction
                        this.GetData(ref dtAnalysis, SQL);
#else
                        Microsoft.Data.SqlClient.SqlDataAdapter aAnalysis = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                        if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                        {
                            try { aAnalysis.Fill(dtAnalysis); }
                            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                        }
#endif
                    }
                    try
                    {
                        if (dtAnalysis.Columns.Count == 0)
                        {
                            System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                            System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                            dtExternalDB.Columns.Add(Value);
                            dtExternalDB.Columns.Add(Display);
                        }
                    }
                    catch { }
                    if (this._ServerConnection.LinkedServer != null && this._ServerConnection.LinkedServer.Length > 0)
                        Description = "Analysis values for list entries in the database";
                    else
                        Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameListAnalysis", "AnalysisID");
                    DiversityWorkbench.QueryCondition qAnalysisID = new DiversityWorkbench.QueryCondition(false, "TaxonNameListAnalysis", "NameID", "AnalysisID", "Taxon list", "Analysis", "Analysis", Description, dtAnalysis, false);
                    QueryConditions.Add(qAnalysisID);

                    if (this._ServerConnection.LinkedServer != null && this._ServerConnection.LinkedServer.Length > 0)
                        Description = "The result of the analysis";
                    else
                        Description = DiversityWorkbench.Functions.ColumnDescription("TaxonNameListAnalysis", "AnalysisValue");
                    DiversityWorkbench.QueryCondition qAnalysisValue = new DiversityWorkbench.QueryCondition(false, "TaxonNameListAnalysis", "NameID", "AnalysisValue", "Taxon list", "Value", "Analysis value", Description);
                    QueryConditions.Add(qAnalysisValue);
                }

                #endregion

                #region Hierarchy

                Description = "If any hierarchy is present";
                DiversityWorkbench.QueryCondition qHierarchyPresent = new DiversityWorkbench.QueryCondition(true, "TaxonHierarchy", "NameID", "Hierarchy", "Presence", "Hierarchy present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qHierarchyPresent);

                Description = DiversityWorkbench.Functions.ColumnDescription("TaxonHierarchy", "HierarchyListCache");
                DiversityWorkbench.QueryCondition qHierarchy = new DiversityWorkbench.QueryCondition(false, "TaxonHierarchy", "NameID", "HierarchyListCache", "Hierarchy", "Hierarchy", "Hierarchy", Description);
                QueryConditions.Add(qHierarchy);






                //System.Data.DataTable dtFamily = new System.Data.DataTable();
                //if (ConnectionString.Length > 0)
                //{
                //    SQL = "SELECT NULL AS [Value], NULL AS Display, NULL AS DisplayOrder UNION " +
                //        "SELECT DISTINCT NameID AS Value, TaxonNameCache AS Display, TaxonNameCache AS DisplayOrder " +
                //        "FROM TaxonName AS T " +
                //        "WHERE (IgnoreButKeepForReference = 0) AND (TaxonomicRank = 'fam.')";

                //    a.SelectCommand.CommandText = SQL;
                //    a.Fill(dtFamily);
                //}
                //if (dtFamily.Columns.Count == 0)
                //{
                //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                //    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                //    dtFamily.Columns.Add(Value);
                //    dtFamily.Columns.Add(Display);
                //}
                //Description = "All taxa within the selected family (only for small amounts of data)";
                //DiversityWorkbench.QueryCondition qFamily = new DiversityWorkbench.QueryCondition(false, "dbo.HierarchyFamilyList(dbo.DefaultProjectID())", true, "NameID", "Family", "Hierarchy (for small stock)", "Family", "Family", Description, dtFamily, false);
                //qFamily.RestrictToProject = true;
                //qFamily.SourceIsFunction = true;
                //QueryConditions.Add(qFamily);

                //Description = DiversityWorkbench.Functions.ColumnDescription("TaxonHierarchy", "HierarchyListCache");
                //DiversityWorkbench.QueryCondition qHierarchy = new DiversityWorkbench.QueryCondition(false, "TaxonHierarchy", "NameID", "HierarchyListCache", "Hierarchy", "Hierarchy", "Hierarchy", Description);
                //QueryConditions.Add(qHierarchy);

                //System.Data.DataTable dtFamily = new System.Data.DataTable();
                //if (ConnectionString.Length > 0)
                //{
                //    SQL = "SELECT NULL AS [Value], NULL AS Display, NULL AS DisplayOrder UNION " +
                //        "SELECT DISTINCT GenusOrSupragenericName, GenusOrSupragenericName AS Expr1, GenusOrSupragenericName AS Expr2 " +
                //        "FROM TaxonName AS T " +
                //        "WHERE (IgnoreButKeepForReference = 0) AND (TaxonomicRank = 'fam.')";

                //    a.SelectCommand.CommandText = SQL;
                //    a.Fill(dtFamily);
                //}
                //if (dtFamily.Columns.Count == 0)
                //{
                //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                //    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                //    dtFamily.Columns.Add(Value);
                //    dtFamily.Columns.Add(Display);
                //}
                //Description = "All taxa within the selected family (only for small amounts of data)";
                //DiversityWorkbench.QueryCondition qFamily = new DiversityWorkbench.QueryCondition(false, "dbo.HierarchyFamilyList(dbo.DefaultProjectID())", true, "NameID", "Family", "Hierarchy (for small stock)", "Family", "Family", Description, dtFamily, false);
                //QueryConditions.Add(qFamily);

                //System.Data.DataTable dtOrder = new System.Data.DataTable();
                //if (ConnectionString.Length > 0)
                //{
                //    SQL = "SELECT NULL AS [Value], NULL AS Display, NULL AS DisplayOrder UNION " +
                //        "SELECT DISTINCT GenusOrSupragenericName, GenusOrSupragenericName AS Expr1, GenusOrSupragenericName AS Expr2 " +
                //        "FROM TaxonName AS T " +
                //        "WHERE (IgnoreButKeepForReference = 0) AND (TaxonomicRank = 'ord.')";

                //    a.SelectCommand.CommandText = SQL;
                //    a.Fill(dtOrder);
                //}
                //if (dtOrder.Columns.Count == 0)
                //{
                //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                //    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                //    dtOrder.Columns.Add(Value);
                //    dtOrder.Columns.Add(Display);
                //}
                //Description = "All taxa within the selected order (only for small amounts of data)";
                //DiversityWorkbench.QueryCondition qOrder = new DiversityWorkbench.QueryCondition(false, "dbo.HierarchyOrderList(dbo.DefaultProjectID())", true, "NameID", "[Order]", "Hierarchy (for small stock)", "Order", "Order", Description, dtOrder, false);
                //QueryConditions.Add(qOrder);

                #endregion

                #region Taxa

                DiversityWorkbench.QueryCondition _qTaxonSelection = new QueryCondition();
                _qTaxonSelection.QueryType = QueryCondition.QueryTypes.Module;
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                _qTaxonSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
                _qTaxonSelection.Entity = "TaxonName.TaxonNameCache";
                _qTaxonSelection.DisplayText = "Taxa";
                _qTaxonSelection.DisplayLongText = "Selection of taxa";
                _qTaxonSelection.Table = "TaxonName";
                _qTaxonSelection.IdentityColumn = "NameID";
                _qTaxonSelection.Column = "NameID";
                _qTaxonSelection.UpperValue = "TaxonomicName";
                _qTaxonSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qTaxonSelection.QueryGroup = "Taxa";
                _qTaxonSelection.Description = "All taxa from the list";
                _qTaxonSelection.QueryType = QueryCondition.QueryTypes.Module;
                QueryConditions.Add(_qTaxonSelection);

                #endregion


            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return QueryConditions;
        }

        //private void GetData(ref System.Data.DataTable DT, string SQL)
        //{
        //    if (this._ServerConnection.ConnectionString.Length > 0)
        //    {
        //        Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
        //        try { a.Fill(DT); }
        //        catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        //    }
        //}

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList = new List<QueryCondition>();
            PredefinedQueryPersistentConditionList.Add(this._QueryConditionProject);
            return PredefinedQueryPersistentConditionList;
        }

        public override void BuildUnitTree(int ID, System.Windows.Forms.TreeView TreeView)
        {
            TreeView.Nodes.Clear();
            TreeView.Nodes.Clear();
            System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode("ID: " + ID.ToString());
            TreeView.Nodes.Add(N);
        }

        #region ProjectValues

        public class Taxon
        {
            public int NameID { get; set; }

            public string URI { get; set; }
            public string TaxonName { get; set; }
            public string Reference { get; set; }
            public System.Collections.Generic.Dictionary<int, string> Hierarchy { get; set; }

            public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> CommonNames { get; set; }

            public System.Collections.Generic.Dictionary<string, string> Geography { get; set; }
            public System.Collections.Generic.Dictionary<string, string> Typification { get; set; }

            public System.Collections.Generic.List<TaxonResource> Resources { get; set; }

            public System.Collections.Generic.List<TaxonAnalysis> Analysis { get; set; }

            public System.Collections.Generic.Dictionary<string, string> ExternalIDs { get; set; }

            public System.Collections.Generic.Dictionary<int, ProjectTaxaSynonymyType> Synonymy { get; set; }

        }

        //this.getDataFromTable(SQL, ref Values);

        //if (this._ServerConnection.LinkedServer.Length == 0)
        //{
        //    SQL = "SELECT TaxonomicRankDisplayText + ':   ' + rtrim(TaxonNameCache) + '\r\n' AS [Hierarchy ranks], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] (" + ID.ToString() + " ,NULL) ORDER BY Seq DESC";
        //    this.getDataFromTable(SQL, ref Values);
        //}
        //else
        //{
        //    //SQL = "SELECT TaxonomicRankDisplayText + ':   ' + rtrim(TaxonNameCache) + '\r\n' AS [Hierarchy ranks], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] (" + ID.ToString() + " ,NULL) ORDER BY Seq DESC";
        //    //this.getDataFromTable(SQL, ref Values);
        //}

        //bool GotData = false;
        //if (this._ServerConnection.LinkedServer.Length == 0)
        //{
        //    SQL = "SELECT substring(TaxonNameCache, 1, charindex(' ', TaxonNameCache)) as [Family] FROM [dbo].[HierarchySuperiorList] " +
        //        "(" + ID.ToString() + " , dbo.DefaultProjectID()) where TaxonomicRank = 'fam.'";
        //    GotData = this.getDataFromTable(SQL, ref Values);
        //}
        //if (!GotData)
        //{
        //    SQL = "SELECT MIN(F.Family) as Family FROM " + Prefix + "[TaxonFamily] F " +
        //        " where F.NameID = " + ID.ToString();
        //    this.getDataFromTable(SQL, ref Values);
        //}

        //GotData = false;
        //if (this._ServerConnection.LinkedServer.Length == 0)
        //{
        //    SQL = "SELECT substring(TaxonNameCache, 1, charindex(' ', TaxonNameCache)) as [Order] FROM [dbo].[HierarchySuperiorList] " +
        //        "(" + ID.ToString() + " , dbo.DefaultProjectID()) where TaxonomicRank = 'ord.'";
        //    GotData = this.getDataFromTable(SQL, ref Values);
        //}
        //if (!GotData)
        //{
        //    SQL = "SELECT MIN(O.[Order]) as [Order] FROM " + Prefix + "[TaxonOrder] O " +
        //       " where O.NameID = " + ID.ToString();
        //    this.getDataFromTable(SQL, ref Values);
        //}

        //GotData = false;
        //if (this._ServerConnection.LinkedServer.Length == 0)
        //{
        //    SQL = "SELECT Taxon + CASE WHEN Seq > 1 THEN ' | ' ELSE '' END AS [Hierarchy], Seq AS _Ord FROM [dbo].[HierarchySuperiorList] " +
        //        "(" + ID.ToString() + " , dbo.DefaultProjectID()) ORDER BY Seq DESC";
        //    GotData = this.getDataFromTable(SQL, ref Values);
        //}
        //if (!GotData)
        //{
        //    SQL = "SELECT TOP 1 h.[HierarchyListCache] AS [Hierarchy] FROM " + Prefix + "[TaxonHierarchy] h where h.NameID = " + ID.ToString() + " AND h.[HierarchyListCache] <> '' ";
        //    this.getDataFromTable(SQL, ref Values);
        //}

        //SQL = "SELECT N.TaxonNameCache + '\r\n' AS Synonym, S.SynType AS [Synonymy type] " +
        //    "FROM " + Prefix + "TaxonSynonymy S INNER JOIN " +
        //    Prefix + "TaxonName N ON S.SynNameID = N.NameID " +
        //    "WHERE (S.IgnoreButKeepForReference = 0)  " +
        //    "AND  (S.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //SQL = "SELECT N.TaxonNameCache AS [Accepted name] " +
        //        "FROM " + Prefix + "TaxonAcceptedName A INNER JOIN " +
        //        Prefix + "TaxonName N ON A.NameID = N.NameID INNER JOIN " +
        //        Prefix + "TaxonSynonymy S ON A.NameID = S.SynNameID " +
        //        "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0)  " +
        //        "AND  (S.NameID = " + ID.ToString() + ")" +
        //        "UNION " +
        //        "SELECT N.TaxonNameCache AS [Accepted name] " +
        //        "FROM " + Prefix + "TaxonAcceptedName A INNER JOIN " +
        //        Prefix + "TaxonName N ON A.NameID = N.NameID " +
        //        "WHERE (A.IgnoreButKeepForReference = 0)  " +
        //        "AND (A.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //// CommonName
        //SQL = "SELECT C.CommonName AS [Common name], L.DisplayText AS Language " +
        //    "FROM " + Prefix + "TaxonCommonName C INNER JOIN " +
        //    Prefix + "LanguageCode_Enum L ON C.LanguageCode = L.Code " +
        //    "WHERE NameID = " + ID.ToString();
        //this.getDataFromTable(SQL, ref Values);

        //// Geography
        //SQL = "SELECT PlaceNameCache AS Place " +
        //    " FROM " + Prefix + "TaxonGeography " +
        //    "WHERE NameID = " + ID.ToString();
        //this.getDataFromTable(SQL, ref Values);

        //// Typification
        //SQL = "SELECT T.Typification  " +
        //        "FROM " + Prefix + "TaxonNameTypification AS T  " +
        //        "WHERE (T.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //// Project
        //SQL = "SELECT P.Project " +
        //    "FROM " + Prefix + "TaxonNameProject T INNER JOIN " +
        //    Prefix + "ProjectList P ON T.ProjectID = P.ProjectID " +
        //    "WHERE (T.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //// List
        //SQL = "SELECT P.Project AS List " +
        //    "FROM " + Prefix + "TaxonNameList L INNER JOIN " +
        //    Prefix + "TaxonNameListProjectProxy P ON L.ProjectID = P.ProjectID " +
        //    "WHERE (L.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //// Analysis
        //SQL = "SELECT P.DisplayText + ': '  + C.DisplayText + case when A.AnalysisValue <> C.DisplayText " +
        //        "and A.AnalysisValue <> '' then ' = ' + A.AnalysisValue else case when A.Notes <> '' then ' = ' + " +
        //        "cast (A.Notes as nvarchar(4000)) " +
        //        "else '' end end AS Analysis  " +
        //        "FROM " + Prefix + "TaxonNameListAnalysis AS A INNER JOIN " +
        //        Prefix + "TaxonNameListProjectProxy AS P ON A.ProjectID = P.ProjectID INNER JOIN " +
        //        Prefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = C.AnalysisID " +
        //        "WHERE (A.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //// Distribution
        //SQL = "SELECT P.DisplayText + ': '  + D.PlaceNameCache AS Distribution  " +
        //        "FROM " + Prefix + "TaxonNameListDistribution AS D INNER JOIN " +
        //        Prefix + "TaxonNameListProjectProxy AS P ON D.ProjectID = P.ProjectID  " +
        //        "WHERE (D.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //// CollectionSpecimen
        //SQL = "SELECT P.DisplayText + ': '  + S.DisplayText AS [Collection specimen]  " +
        //        "FROM " + Prefix + "TaxonNameListCollectionSpecimen AS S INNER JOIN " +
        //        Prefix + "TaxonNameListProjectProxy AS P ON S.ProjectID = P.ProjectID  " +
        //        "WHERE (S.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //// Reference
        //SQL = "SELECT P.DisplayText + ': '  + R.TaxonNameListRefText AS [List reference]  " +
        //        "FROM " + Prefix + "TaxonNameListReference AS R INNER JOIN " +
        //        Prefix + "TaxonNameListProjectProxy AS P ON R.ProjectID = P.ProjectID  " +
        //        "WHERE (R.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);


        //SQL = "SELECT D.ExternalDatabaseName + CASE WHEN D.ExternalDatabaseVersion IS NULL OR " +
        //    "LEN(D.ExternalDatabaseVersion) " +
        //    "> 50 THEN '' ELSE '. Vers.: ' + D.ExternalDatabaseVersion END + CASE WHEN D.ExternalDatabaseAuthors " +
        //    "IS NULL THEN '' ELSE '. Aut.: ' + D.ExternalDatabaseAuthors END + '\r\n' AS [External database], " +
        //    "I.ExternalNameURI AS [External name URI] " +
        //    "FROM  " + Prefix + "TaxonNameExternalID I INNER JOIN " +
        //    Prefix + "TaxonNameExternalDatabase D ON I.ExternalDatabaseID = D.ExternalDatabaseID " +
        //    "WHERE (I.NameID = " + ID.ToString() + ")";
        //this.getDataFromTable(SQL, ref Values);

        //if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
        //this._UnitValues.Clear();
        //foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
        //{
        //    this._UnitValues.Add(P.Key, P.Value);
        //}

        private System.Collections.Generic.Dictionary<int, Taxon> _ProjectValues;

        public System.Collections.Generic.Dictionary<int, Taxon> ProjectTaxa(int ProjectID)
        {
            _ProjectValues = new Dictionary<int, Taxon>();

            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                try
                {
                    // getting the datatables
                    //System.Data.DataTable dtTaxaHierarchy = this.DtTaxaHierarchy(ProjectID);
                    _ProjectTaxaHierarchy = new Dictionary<int, int>();
                    _ProjectTaxaHierarchyNames = new Dictionary<int, string>();
                    this.initProjectTaxaSynonymy(ProjectID);

                    _Taxa = new Dictionary<int, Taxon>();
                    _TaxonNames = new Dictionary<int, string>();
                    foreach (System.Data.DataRow R in this.DtTaxa(ProjectID).Rows)
                    {
                        int NameID;
                        if (int.TryParse(R["NameID"].ToString(), out NameID))
                        {
                            Taxon tax = new Taxon();
                            tax.NameID = NameID;
                            tax.TaxonName = R["TaxonNameCache"].ToString();
                            _TaxonNames.Add(NameID, tax.TaxonName);
                            tax.URI = BaseURL + NameID.ToString();
                            _Taxa.Add(NameID, tax);
                            this.setProjectTaxaHierarchy(NameID, R);
                            _ProjectValues.Add(NameID, tax);
                            this.setProjectTaxaSynonymy(NameID);
                        }
                    }
                    this.setProjectTaxaHierarchy();
                    this.setProjectCommonNames(ProjectID);
                    this.setProjectGeography(ProjectID);
                    this.setProjectTaxaTypification(ProjectID);
                    this.setProjectTaxaAnalysis(ProjectID);
                    this.setProjectTaxaResources(ProjectID);
                    this.setProjectTaxaExternalIDs(ProjectID);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return _ProjectValues;
        }

        public override System.Collections.Generic.Dictionary<int, System.Object> ModuleProjectValues(int ProjectID)
        {
            _ProjectValues = ProjectTaxa(ProjectID);

            System.Collections.Generic.Dictionary<int, System.Object> Values = new Dictionary<int, object>();
            foreach (System.Collections.Generic.KeyValuePair<int, Taxon> KV in _ProjectValues)
            {
                Values.Add(KV.Key, (System.Object)KV.Value);
            }
            return Values;
        }

        private System.Collections.Generic.Dictionary<int, Taxon> _Taxa;
        private System.Collections.Generic.Dictionary<int, string> _TaxonNames;

        #region Synonymy

        private System.Data.DataTable DtProjectSynonymy(int ProjectID)
        {
            string SQL = "SELECT NameID, SynNameID, SynType FROM " +
                    Prefix + "TaxoSynonymy S WHERE S.IgnoreButKeepForReference = 0 AND S.ProjectID = " + ProjectID.ToString() +
                    " UNION " +
                    "SELECT T.BasedOnNameID, T.NameID, 'homotypic' FROM TaxonName AS T INNER JOIN " +
                    Prefix + "TaxonNameProject AS P ON P.NameID = T.NameID AND T.IgnoreButKeepForReference = 0 AND (T.DataWithholdingReason = '' OR T.DataWithholdingReason IS NULL) AND P.ProjectID = " + ProjectID.ToString() +
                    "WHERE  NOT BasedOnNameID IS NULL" +
                    " UNION " +
                    "SELECT T.NameID, T.NameID, 'accepted' FROM TaxonAcceptedName AS T WHERE T.IgnoreButKeepForReference = 0 AND T.ProjectID = " + ProjectID.ToString();
            System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dtTaxa;
        }

        private void initProjectTaxaSynonymy(int ProjectID)
        {
            _ProjectTaxaSynonymyType = new Dictionary<int, Dictionary<int, ProjectTaxaSynonymyType>>();
            foreach (System.Data.DataRow R in DtProjectSynonymy(ProjectID).Rows)
            {
                int NameID;
                int SynNameID;
                if (int.TryParse(R["NameID"].ToString(), out NameID) &&
                    int.TryParse(R["SynNameID"].ToString(), out SynNameID))
                {
                    if (!_ProjectTaxaSynonymyType.ContainsKey(NameID))
                        _ProjectTaxaSynonymyType.Add(NameID, new Dictionary<int, ProjectTaxaSynonymyType>());
                    if (!_ProjectTaxaSynonymyType[NameID].ContainsKey(SynNameID))
                        _ProjectTaxaSynonymyType[NameID].Add(SynNameID, projectTaxaSynonymyType(R));
                }
            }
        }

        private ProjectTaxaSynonymyType projectTaxaSynonymyType(System.Data.DataRow R)
        {
            ProjectTaxaSynonymyType type = TaxonName.ProjectTaxaSynonymyType.Unknown;
            string Type = R["SynType"].ToString().ToLower();
            switch (Type)
            {
                case "duplicate": type = ProjectTaxaSynonymyType.Duplicate; break;
                case "heterotypic": type = ProjectTaxaSynonymyType.Heterotypic; break;
                case "homotypic": type = ProjectTaxaSynonymyType.Homotypic; break;
                case "isonym": type = ProjectTaxaSynonymyType.Isonym; break;
                case "orthographic variant": type = ProjectTaxaSynonymyType.OrtograhicVariant; break;
                case "accepted": type = ProjectTaxaSynonymyType.Accepted; break;
            }
            return type;
        }

        private void setProjectTaxaSynonymy(int NameID)
        {
            if (_ProjectTaxaSynonymyType != null && _ProjectTaxaSynonymyType.ContainsKey(NameID))
            {
                if (_Taxa[NameID].Synonymy == null) { _Taxa[NameID].Synonymy = new Dictionary<int, ProjectTaxaSynonymyType> { }; };
                foreach (System.Collections.Generic.KeyValuePair<int, ProjectTaxaSynonymyType> KV in _ProjectTaxaSynonymyType[NameID])
                {
                    _Taxa[NameID].Synonymy.Add(KV.Key, KV.Value);
                }
            }
        }
        private void setProjectTaxaSynonymy()
        {
            foreach (System.Collections.Generic.KeyValuePair<int, System.Collections.Generic.Dictionary<int, ProjectTaxaSynonymyType>> KV in _ProjectTaxaSynonymyType)
            {
                //if (_ProjectTaxaSynonymyType.ContainsKey(NameID))
                //{
                //    tax.Synonymy = _ProjectTaxaSynonymyType[NameID];
                //}
            }
        }

        //private void setProjectTaxaSynonymy(int NameID, System.Data.DataRow R)
        //{
        //    int SynNameID;
        //    if (int.TryParse(R["SynNameID"].ToString(), out SynNameID))
        //    {
        //        _ProjectTaxaHierarchy.Add(NameID, SynNameID);
        //        string HierarchyName = R["GenusOrSupragenericName"].ToString();
        //        if (!R["InfragenericEpithet"].Equals(System.DBNull.Value) && R["InfragenericEpithet"].ToString().Length > 0)
        //            HierarchyName += " " + R["InfragenericEpithet"].ToString();
        //        if (!R["SpeciesEpithet"].Equals(System.DBNull.Value) && R["SpeciesEpithet"].ToString().Length > 0)
        //            HierarchyName += " " + R["SpeciesEpithet"].ToString();
        //        _ProjectTaxaHierarchyNames.Add(NameID, HierarchyName);
        //    }
        //}

        //private System.Collections.Generic.Dictionary<int, string> ProjectTaxonSynonymy(int NameID)
        //{
        //    System.Collections.Generic.Dictionary<int, string> Hierarchy = new Dictionary<int, string>();
        //    if (_ProjectTaxaHierarchy.ContainsKey(NameID))
        //    {
        //        int ParentID = _ProjectTaxaHierarchy[NameID];
        //        Hierarchy.Add(ParentID, _ProjectTaxaHierarchyNames[ParentID]);
        //        while (_ProjectTaxaHierarchy.ContainsKey(ParentID))
        //        {
        //            ParentID = _ProjectTaxaHierarchy[ParentID];
        //            Hierarchy.Add(ParentID, _ProjectTaxaHierarchyNames[ParentID]);
        //        }
        //    }
        //    return Hierarchy;
        //}

        //private System.Collections.Generic.Dictionary<int, int> _ProjectTaxaSynonymy;

        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, ProjectTaxaSynonymyType>> _ProjectTaxaSynonymyType;
        public enum ProjectTaxaSynonymyType { Accepted, Homotypic, Heterotypic, Duplicate, Isonym, OrtograhicVariant, Unknown }

        #endregion

        #region Common Names
        private System.Data.DataTable DtProjectTaxaCommonNames(int ProjectID)
        {
            string SQL = "SELECT C.NameID, C.CommonName, C.LanguageCode, C.SubjectContext FROM " +
                Prefix + "TaxonCommonName C INNER JOIN " +
                Prefix + "TaxonNameProject AS P ON P.NameID = C.NameID AND P.ProjectID = " + ProjectID.ToString();
            System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dtTaxa;
        }

        private void setProjectCommonNames(int ProjectID)
        {
            foreach (System.Data.DataRow R in this.DtProjectTaxaCommonNames(ProjectID).Rows)
            {
                int NameID;
                if (int.TryParse(R["NameID"].ToString(), out NameID))
                {
                    if (_Taxa.ContainsKey(NameID))
                    {
                        if (_Taxa[NameID].CommonNames == null) _Taxa[NameID].CommonNames = new Dictionary<string, List<string>>();
                        string Code = R["LanguageCode"].ToString();
                        string CommonName = R["CommonName"].ToString();
                        if (!_Taxa[NameID].CommonNames.ContainsKey(Code))
                            _Taxa[NameID].CommonNames.Add(Code, new List<string>());
                        if (!_Taxa[NameID].CommonNames[Code].Contains(CommonName))
                            _Taxa[NameID].CommonNames[Code].Add(CommonName);
                    }
                }
            }
        }

        #endregion

        #region Geography

        private void setProjectGeography(int ProjectID)
        {
            foreach (System.Data.DataRow R in this.DtProjectTaxaGeography(ProjectID).Rows)
            {
                int NameID;
                if (int.TryParse(R["NameID"].ToString(), out NameID))
                {
                    if (_Taxa.ContainsKey(NameID))
                    {
                        if (_Taxa[NameID].Geography == null) _Taxa[NameID].Geography = new Dictionary<string, string>();
                        string PlaceURI = R["PlaceURI"].ToString();
                        string PlaceNameCache = R["PlaceNameCache"].ToString();
                        if (!_Taxa[NameID].Geography.ContainsKey(PlaceURI))
                            _Taxa[NameID].Geography.Add(PlaceURI, PlaceNameCache);
                    }
                }
            }
        }

        private System.Data.DataTable DtProjectTaxaGeography(int ProjectID)
        {
            string SQL = "SELECT NameID, PlaceURI, PlaceNameCache FROM " +
                Prefix + "TaxonGeography G INNER JOIN " +
                Prefix + "TaxonNameProject AS P ON P.NameID = G.NameID AND P.ProjectID = " + ProjectID.ToString();
            System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dtTaxa;
        }

        #endregion

        #region Typification

        private void setProjectTaxaTypification(int ProjectID)
        {
            foreach (System.Data.DataRow R in this.DtProjectTaxaTypes(ProjectID).Rows)
            {
                int NameID;
                if (int.TryParse(R["NameID"].ToString(), out NameID))
                {
                    if (_Taxa.ContainsKey(NameID))
                    {
                        if (_Taxa[NameID].Typification == null) _Taxa[NameID].Typification = new Dictionary<string, string>();
                        string TypificationReferenceTitle = R["TypificationReferenceTitle"].ToString();
                        string Typification = R["Typification"].ToString();
                        if (!_Taxa[NameID].Typification.ContainsKey(TypificationReferenceTitle))
                            _Taxa[NameID].Typification.Add(TypificationReferenceTitle, Typification);
                    }
                }
            }
        }

        private System.Data.DataTable DtProjectTaxaTypes(int ProjectID)
        {
            string SQL = "SELECT T.NameID, T.TypificationReferenceTitle, T.Typification FROM  " +
                Prefix + "TaxonNameTypification AS T  INNER JOIN " +
                Prefix + "TaxonNameProject AS P ON P.NameID = T.NameID AND P.ProjectID = " + ProjectID.ToString();
            System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dtTaxa;
        }

        #endregion

        #region Hierarchy
        private void setProjectTaxaHierarchy(int NameID, System.Data.DataRow R)
        {
            int NameParentID;
            if (int.TryParse(R["NameParentID"].ToString(), out NameParentID))
            {
                _ProjectTaxaHierarchy.Add(NameID, NameParentID);
                string HierarchyName = R["GenusOrSupragenericName"].ToString();
                if (!R["InfragenericEpithet"].Equals(System.DBNull.Value) && R["InfragenericEpithet"].ToString().Length > 0)
                    HierarchyName += " " + R["InfragenericEpithet"].ToString();
                if (!R["SpeciesEpithet"].Equals(System.DBNull.Value) && R["SpeciesEpithet"].ToString().Length > 0)
                    HierarchyName += " " + R["SpeciesEpithet"].ToString();
                _ProjectTaxaHierarchyNames.Add(NameID, HierarchyName);
            }
        }

        private void setProjectTaxaHierarchy()
        {
            foreach (System.Collections.Generic.KeyValuePair<int, Taxon> KV in _Taxa)
            {
                KV.Value.Hierarchy = ProjectTaxonHierarchy(KV.Key);
            }
        }

        private System.Collections.Generic.Dictionary<int, string> ProjectTaxonHierarchy(int NameID)
        {
            System.Collections.Generic.Dictionary<int, string> Hierarchy = new Dictionary<int, string>();
            if (_ProjectTaxaHierarchy.ContainsKey(NameID))
            {
                int ParentID = _ProjectTaxaHierarchy[NameID];
                Hierarchy.Add(ParentID, _ProjectTaxaHierarchyNames[ParentID]);
                while (_ProjectTaxaHierarchy.ContainsKey(ParentID))
                {
                    ParentID = _ProjectTaxaHierarchy[ParentID];
                    Hierarchy.Add(ParentID, _ProjectTaxaHierarchyNames[ParentID]);
                }
            }
            return Hierarchy;
        }

        private System.Collections.Generic.Dictionary<int, int> _ProjectTaxaHierarchy;
        private System.Collections.Generic.Dictionary<int, string> _ProjectTaxaHierarchyNames;

        #endregion

        #region Analysis

        private void setProjectTaxaAnalysis(int ProjectID)
        {
            foreach (System.Data.DataRow R in this.DtTaxaLists(ProjectID).Rows)
            {
                int NameID;
                if (int.TryParse(R["NameID"].ToString(), out NameID))
                {
                    if (_Taxa.ContainsKey(NameID))
                    {
                        if (_Taxa[NameID].Analysis == null) _Taxa[NameID].Analysis = new List<TaxonAnalysis>();
                        TaxonAnalysis taxonAnalysis = new TaxonAnalysis();
                        taxonAnalysis.Analysis = R["Analysis"].ToString();
                        taxonAnalysis.Value = R["AnalysisValue"].ToString();
                        taxonAnalysis.List = R["List"].ToString();
                        taxonAnalysis.ListID = int.Parse(R["ListID"].ToString());
                        _Taxa[NameID].Analysis.Add(taxonAnalysis);
                    }
                }
            }
        }


        private System.Data.DataTable DtTaxaLists(int ProjectID)
        {
            string SQL = "SELECT A.NameID, A.AnalysisValue, C.DisplayText AS Analysis, L.Project AS List, L.ProjectID AS ListID FROM  " +
                Prefix + "TaxonNameListAnalysis AS A INNER JOIN " +
                Prefix + "TaxonNameListAnalysisCategory AS C ON A.AnalysisID = C.AnalysisID INNER JOIN " +
                Prefix + "TaxonNameProject AS P ON A.NameID = P.NameID INNER JOIN " +
                Prefix + "TaxonNameListProjectProxy AS L ON A.ProjectID = L.ProjectID " +
                " WHERE P.ProjectID  = " + ProjectID.ToString();
            System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dtTaxa;
        }

        public struct TaxonAnalysis
        {
            public string Analysis;
            public string Value;
            public string List;
            public int ListID;
        }

        #endregion

        #region Resources

        public enum ResourceType { audio, drawing, image, information, preview, supportingfiles, video, none }

        private void setProjectTaxaResources(int ProjectID)
        {
            foreach (System.Data.DataRow R in this.DtTaxaResources(ProjectID).Rows)
            {
                int NameID;
                if (int.TryParse(R["NameID"].ToString(), out NameID))
                {
                    if (_Taxa.ContainsKey(NameID))
                    {
                        if (_Taxa[NameID].Resources == null) _Taxa[NameID].Resources = new List<TaxonResource>();
                        TaxonResource taxonResource = new TaxonResource();
                        taxonResource.ResourceType = TaxonResourceType(R["ResourceType"].ToString());
                        taxonResource.Title = R["Title"].ToString();
                        taxonResource.URI = R["URI"].ToString();
                        _Taxa[NameID].Resources.Add(taxonResource);
                    }
                }
            }
        }

        private ResourceType TaxonResourceType(string resourceType)
        {
            switch (resourceType.ToLower())
            {
                case "audio": return ResourceType.audio;
                case "image": return ResourceType.image;
                case "information": return ResourceType.information;
                case "preview": return ResourceType.preview;
                case "supporting files": return ResourceType.supportingfiles;
                case "video": return ResourceType.video;
                default: return ResourceType.none;
            }
        }

        public struct TaxonResource
        {
            public string URI;
            public string Title;
            public ResourceType ResourceType;
        }

        private System.Data.DataTable DtTaxaResources(int ProjectID)
        {
            string SQL = "SELECT NameID, URI, Title, ResourceType FROM  " +
                Prefix + "TaxonNameResource WHERE (ProjectID = " + ProjectID.ToString() + ") AND (DataWithholdingReason = N'' OR DataWithholdingReason IS NULL) " +
                "ORDER BY DisplayOrder ";
            System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dtTaxa;
        }


        #endregion

        #region External IDs

        private void setProjectTaxaExternalIDs(int ProjectID)
        {
            foreach (System.Data.DataRow R in this.DtTaxaExternalIDs(ProjectID).Rows)
            {
                int NameID;
                if (int.TryParse(R["NameID"].ToString(), out NameID))
                {
                    if (_Taxa.ContainsKey(NameID))
                    {
                        if (_Taxa[NameID].ExternalIDs == null) _Taxa[NameID].ExternalIDs = new Dictionary<string, string>();
                        string ExternalNameURI = R["ExternalNameURI"].ToString();
                        string ExternalDatabaseName = R["ExternalDatabaseName"].ToString();
                        if (!_Taxa[NameID].ExternalIDs.ContainsKey(ExternalNameURI))
                            _Taxa[NameID].ExternalIDs.Add(ExternalNameURI, ExternalDatabaseName);
                    }
                }
            }
        }


        private System.Data.DataTable DtTaxaExternalIDs(int ProjectID)
        {
            string SQL = "SELECT E.NameID, E.ExternalNameURI, D.ExternalDatabaseName FROM  " +
                Prefix + "TaxonNameExternalID AS E INNER JOIN " +
                Prefix + "TaxonNameExternalDatabase AS D ON E.ExternalDatabaseID = D.ExternalDatabaseID INNER JOIN" +
                Prefix + "TaxonNameProject AS P ON E.NameID = P.NameID" +
                "WHERE P.ProjectID = " + ProjectID.ToString(); ;
            System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dtTaxa;
        }

        #endregion

        #region Data from DB
        private System.Data.DataTable DtTaxa(int ProjectID)
        {
            string SQL = "SELECT T.NameID, " +
                "T.TaxonNameCache, T.BasedOnNameID, T.TaxonomicRank, " +
                "T.GenusOrSupragenericName, T.InfragenericEpithet, " +
                "T.SpeciesEpithet, T.InfraspecificEpithet, " +
                "case when T.NonNomenclaturalNameSuffix <> '' then T.NonNomenclaturalNameSuffix else " +
                "case when T.CombiningAuthors <> '' then '(' else '' end + T.BasionymAuthors + case when T.CombiningAuthors <> '' then ')' else '' end + " +
                "case when T.CombiningAuthors <> '' then ' ' + T.CombiningAuthors else '' end end AS Authors, T.NomenclaturalStatus, " +
                "case when T.ReferenceTitle is null then '' else T.ReferenceTitle end +  " +
                "case when T.Volume is null then '' else '. ' + T.Volume end + case when T.Issue is null then '' else '. ' + T.Issue end +   " +
                "case when T.Pages is null then '' else ', ' + T.Pages end +  " +
                "case when T.YearOfPubl is null then '' else '. ' + cast(T.YearOfPubl as varchar) end AS Publication, " +
                "H.NameParentID, A.NameID AS NameIDAccepted " +
                "FROM " +
                Prefix + "TaxonName AS T INNER JOIN " +
                Prefix + "TaxonNameProject AS P ON P.NameID = T.NameID AND T.IgnoreButKeepForReference = 0 AND (T.DataWithholdingReason = '' OR T.DataWithholdingReason IS NULL) AND P.ProjectID = " + ProjectID.ToString() + " LEFT OUTER JOIN " +
                Prefix + "TaxonHierarchy AS H ON T.NameID = H.NameID AND H.IgnoreButKeepForReference = 0 AND H.ProjectID  =  P.ProjectID LEFT OUTER JOIN " +
                Prefix + "TaxonAcceptedName AS A ON T.NameID = A.NameID AND A.IgnoreButKeepForReference = 0 AND A.ProjectID  = P.ProjectID ";
            System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dtTaxa;
        }

        //private System.Data.DataTable DtTaxaHierarchy(int ProjectID)
        //{
        //    string SQL = "SELECT H.NameID, H.NameParentID, T.GenusOrSupragenericName FROM " + Prefix + "[TaxonHierarchy] H INNER JOIN " +
        //        Prefix + "[TaxonName] T ON T.NameID = H.NameID AND T.IgnoreButKeepForReference = 0 AND (T.DataWithholdingReason = '' OR T.DataWithholdingReason IS NULL)" +
        //        "AND H.IgnoreButKeepForReference = 0 AND h.ProjectID = " + ProjectID.ToString();
        //    System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
        //    return dtTaxa;
        //}

        //private System.Data.DataTable DtTaxaAnalysis(int ProjectID)
        //{
        //    string SQL = "SELECT L.NameID, P.Project AS List FROM  " +
        //        Prefix + "TaxonNameListAnalysis A ON L.ProjectID = A.ProjectID AND L.NameID = A.NameID " +
        //        Prefix + "TaxonNameListAnalysisCategory C ON L.AnalysisID = C.AnalysisID AND  " +
        //        "WHERE NameID  IN ( " + NameIDs + ")";
        //    System.Data.DataTable dtTaxa = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
        //    return dtTaxa;
        //}

        private string _BaseURL;
        private string BaseURL
        {
            get
            {
                if (_BaseURL == null)
                {
                    string SQL = "SELECT MAX(U.BaseURL) FROM " +
                        Prefix + "ViewBaseURL AS U";
                    _BaseURL = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                return _BaseURL;
            }
        }

        #endregion



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
                case ModuleType.Gazetteer:
                    this._BackLinkImages.Images.Add("Country", DiversityWorkbench.Properties.Resources.Country);
                    this._BackLinkImages.Images.Add("Event", DiversityWorkbench.Properties.Resources.Event);
                    break;
                case ModuleType.Projects:
                    this._BackLinkImages.Images.Add("Project", DiversityWorkbench.Properties.Resources.Project);
                    this._BackLinkImages.Images.Add("List", DiversityWorkbench.Properties.Resources.Checklist);
                    break;
                case ModuleType.References:
                    this._BackLinkImages.Images.Add("Accepted name", DiversityWorkbench.Properties.Resources.NameAccepted);
                    this._BackLinkImages.Images.Add("Synonym", DiversityWorkbench.Properties.Resources.NameSynonym);
                    this._BackLinkImages.Images.Add("Hierarchy", DiversityWorkbench.Properties.Resources.Hierarchy);
                    this._BackLinkImages.Images.Add("Typification", DiversityWorkbench.Properties.Resources.NameType);
                    this._BackLinkImages.Images.Add("Reference", DiversityWorkbench.Properties.Resources.References);
                    this._BackLinkImages.Images.Add("Common name", DiversityWorkbench.Properties.Resources.NameCommon);
                    this._BackLinkImages.Images.Add("List", DiversityWorkbench.Properties.Resources.Checklist);
                    break;
            }
            return this._BackLinkImages;
        }


        public override System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BackLinkServerConnectionDomains(string URI, ModuleType CallingModule, bool IncludeEmpty = false, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BLD = new Dictionary<ServerConnection, List<BackLinkDomain>>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this.BackLinkConnections(ModuleType.TaxonNames))
            {
                switch (CallingModule)
                {
                    case ModuleType.Gazetteer:
                        System.Collections.Generic.List<BackLinkDomain> _G = this.BackLinkDomainGazetteer(KV.Value, URI);
                        if (_G.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _G);
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

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainGazetteer(ServerConnection SC, string URI)
        {
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain ESIC = this.BackLinkDomain(SC, URI, "Geography", "TaxonGeography", "PlaceURI", 2);
            if (ESIC.DtItems.Rows.Count > 0)
                Links.Add(ESIC);
            DiversityWorkbench.BackLinkDomain ESIL = this.BackLinkDomain(SC, URI, "List area", "TaxonNameListArea", "PlaceURI", 2);
            if (ESIC.DtItems.Rows.Count > 0)
                Links.Add(ESIL);
            DiversityWorkbench.BackLinkDomain Ident = this.BackLinkDomain(SC, URI, "List distribution", "TaxonNameListDistribution", "PlaceURI", 3);
            if (Ident.DtItems.Rows.Count > 0)
                Links.Add(Ident);
            return Links;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainProject(ServerConnection SC, string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();

            // Check presence of column ProjectURI
            string Check = "select count(*) from [" + SC.LinkedServer + "].[" + SC.DatabaseName + "].information_schema.columns C " +
                "where c.table_name = 'ProjectList' " +
                "and c.column_name = 'ProjectURI'";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Check);

            if (Result != "0")
            {

                // Project
                DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain("Project", "ProjectProxy", "ProjectURI", 2);
                string Prefix = "dbo.";
                if (SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "]." + Prefix;
                string SQL = "SELECT 'First of ' + CAST(COUNT(*) as varchar) + ' taxa' AS DisplayText, " +
                    "MIN(S.NameID) AS ID ";
                if (SC.LinkedServer.Length > 0)
                    SQL += "FROM " + Prefix + "ProjectList AS T ";
                else
                    SQL += "FROM " + Prefix + "ProjectProxy AS T ";
                SQL += "INNER JOIN " + Prefix + "TaxonNameProject AS S ON T.ProjectID = S.ProjectID " +
                    "WHERE(T.ProjectURI = '" + URI + "') " +
                    "GROUP BY T.Project, T.ProjectURI ";
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

                // List
                DiversityWorkbench.BackLinkDomain BackLinkList = new BackLinkDomain("List", "TaxonNameListProjectProxy", "ProjectURI", 3);
                SQL = "SELECT 'First of ' + CAST(COUNT(*) as varchar) + ' taxa' AS DisplayText, " +
                    "MIN(S.NameID) AS ID " +
                    "FROM " + Prefix + "TaxonNameListProjectProxy AS T " +
                    "INNER JOIN " + Prefix + "TaxonNameList AS S ON T.ProjectID = S.ProjectID " +
                    "WHERE(T.ProjectURI = '" + URI + "') " +
                    "GROUP BY T.Project, T.ProjectURI ";
                ad.SelectCommand.CommandText = SQL;
                try
                {
                    ad.Fill(BackLinkList.DtItems);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                if (BackLinkList.DtItems.Rows.Count > 0)
                    Links.Add(BackLinkList);
            }

            return Links;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainReferences(ServerConnection SC, string URI)
        {
            System.Collections.Generic.List<BackLinkDomain> Refs = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain Accepted = this.BackLinkDomain(SC, URI, "Accepted name", "TaxonAcceptedName", "RefURI", 2);
            if (Accepted.DtItems.Rows.Count > 0)
                Refs.Add(Accepted);
            DiversityWorkbench.BackLinkDomain Synonym = this.BackLinkDomain(SC, URI, "Synonymy", "TaxonSynonymy", "SynRefURI", 3);
            if (Synonym.DtItems.Rows.Count > 0)
                Refs.Add(Synonym);
            DiversityWorkbench.BackLinkDomain Hierarchy = this.BackLinkDomain(SC, URI, "Hierarchy", "TaxonHierarchy", "HierarchyRefURI", 4);
            if (Hierarchy.DtItems.Rows.Count > 0)
                Refs.Add(Hierarchy);
            DiversityWorkbench.BackLinkDomain Typification = this.BackLinkDomain(SC, URI, "Typification", "TaxonNameTypification", "TypificationReferenceURI", 5);
            if (Typification.DtItems.Rows.Count > 0)
                Refs.Add(Typification);
            DiversityWorkbench.BackLinkDomain Reference = this.BackLinkDomain(SC, URI, "Reference", "TaxonNameReference", "TaxonNameRefURI", 6);
            if (Reference.DtItems.Rows.Count > 0)
                Refs.Add(Reference);
            DiversityWorkbench.BackLinkDomain Common = this.BackLinkDomain(SC, URI, "Common name", "TaxonCommonName", "ReferenceURI", 7);
            if (Common.DtItems.Rows.Count > 0)
                Refs.Add(Common);
            DiversityWorkbench.BackLinkDomain List = this.BackLinkDomain(SC, URI, "List", "TaxonNameListReference", "TaxonNameListRefURI", 8);
            if (List.DtItems.Rows.Count > 0)
                Refs.Add(List);
            return Refs;
        }

        private DiversityWorkbench.BackLinkDomain BackLinkDomain(ServerConnection SC, string URI, string DisplayText, string Table, string LinkColumn, int ImageKey, System.Collections.Generic.List<string> Restrictions = null)
        {
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain(DisplayText, Table, LinkColumn, ImageKey);
            string Prefix = "[" + SC.DatabaseName + "].dbo."; // Toni 20210727 database name added
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "]." + Prefix;
            string SQL = "SELECT TaxonNameCache AS DisplayText, " +
                "S.NameID AS ID " +
                "FROM " + Prefix + Table + " AS T " +
                "INNER JOIN " + Prefix + "TaxonName AS S ON T.NameID = S.NameID " +
                "WHERE(T." + LinkColumn + " = '" + URI + "') ";
            if (Restrictions != null)
            {
                foreach (string R in Restrictions)
                {
                    SQL += " AND " + R;
                }
            }
            SQL += " GROUP BY S.NameID, S.TaxonNameCache ";
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

        private static bool _FromCacheDB = false;
        public void FromCacheDB(bool GetDataFromCacheDB) { _FromCacheDB = GetDataFromCacheDB; }

        /// <summary>
        /// Getting the taxa underneath a given taxon including the given taxon
        /// </summary>
        /// <param name="URL">The URL of the taxon (= BaseURL + NameID)</param>
        /// <returns>A dictionary containing the URLs and the names</returns>
        public static System.Collections.Generic.Dictionary<string, string> SubTaxa(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartTaxon(URL);
            getSubTaxa(ref dt);
            getTaxa(ref DD, dt);
            return DD;
        }

        /// <summary>
        /// Getting the synonyms of a given taxon
        /// </summary>
        /// <param name="URL">The URL of the taxon (= BaseURL + NameID)</param>
        /// <returns>A dictionary containing the URLs and the names</returns>
        public static System.Collections.Generic.Dictionary<string, string> Synonyms(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartTaxon(URL);
            getSynonyms(ref dt);
            getTaxa(ref DD, dt);
            return DD;
        }

        /// <summary>
        /// Getting the taxa underneath a given taxon including the given taxon and all synonyms
        /// </summary>
        /// <param name="URL">The URL of the taxon (= BaseURL + NameID)</param>
        /// <returns>A dictionary containing the URLs and the names</returns>
        public static System.Collections.Generic.Dictionary<string, string> SubTaxaSynonyms(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartTaxon(URL);
            getSynonyms(ref dt);
            getSubTaxa(ref dt);
            getSynonyms(ref dt);
            getTaxa(ref DD, dt);
            return DD;
        }

        public static string AcceptedName(string URL, ref string UrlAcceptedName)
        {
            string AcceptedName = "";
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartTaxon(URL);
            AcceptedName = getAcceptedName(ref dt, ref UrlAcceptedName);
            return AcceptedName;
        }

        #region Auxillary

        private static int? _ProjectID;
        private static int ProjectID()
        {
            if (_ProjectID == null)
            {
                string SQL = "SELECT ProjectID, Project FROM " + _SC.Prefix() + "ProjectList ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 1)
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Project", "Please select a project");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        int i;
                        if (int.TryParse(f.SelectedValue, out i))
                            _ProjectID = i;
                    }
                }
                else
                    _ProjectID = int.Parse(dt.Rows[0][0].ToString());
            }
            return (int)_ProjectID;
        }

        private static int ProjectID(string IDs)
        {
            if (_ProjectID == null)
            {
                string SQL = "SELECT DISTINCT ProjectID FROM" + _SC.Prefix() + "TaxonNameProject P WHERE NameID IN (" + IDs + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                    _ProjectID = int.Parse(dt.Rows[0][0].ToString());
                else
                {
                    dt = new System.Data.DataTable();
                    SQL = "SELECT ProjectID, Project FROM " + _SC.Prefix() + "ProjectList ORDER BY Project";
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dt);
                    if (dt.Rows.Count > 1)
                    {
                        DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Project", "Please select a project");
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            int i;
                            if (int.TryParse(f.SelectedValue, out i))
                                _ProjectID = i;
                        }
                    }
                    else
                        _ProjectID = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return (int)_ProjectID;
        }

        private static DiversityWorkbench.ServerConnection _SC;

        private static System.Data.DataTable getStartTaxon(string URL)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                // resetting the project
                _ProjectID = null;
                // getting the server connection for the URL
                setServerConnection(URL);
                // Inserting the ID to start the query
                string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(URL);
                string SQL = "SELECT NameID FROM " + _SC.Prefix() + "TaxonName N WHERE NameID = " + ID + " " +
                    "AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL)";
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

        public static void getTaxa(ref System.Collections.Generic.Dictionary<string, string> DD, System.Data.DataTable DT)
        {
            string SQL = "";
            try
            {
                foreach (System.Data.DataRow R in DT.Rows)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += R[0].ToString();
                }
                SQL = "SELECT U.BaseURL + cast(T.NameID as varchar) AS URL, T.TaxonNameCache AS Taxon FROM " + _SC.Prefix() + "TaxonName T, " + _SC.Prefix() + "ViewBaseURL U WHERE T.NameID IN (" + SQL + ")";
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

        private static void getSubTaxa(ref System.Data.DataTable DT)
        {
            string IDs = "";
            foreach (System.Data.DataRow R in DT.Rows)
            {
                if (IDs.Length > 0) IDs += ", ";
                IDs += R[0].ToString();
            }
            string SQL = "SELECT H.NameID FROM " + _SC.Prefix() + "TaxonHierarchy H, " + _SC.Prefix() + "TaxonName N WHERE H.NameID = N.NameID " +
                " AND H.NameParentID IN ( " + IDs + ") " +
                " AND H.ProjectID = " + ProjectID(IDs) + " AND H.IgnoreButKeepForReference = 0 AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                " AND H.NameID NOT IN (" + IDs + ")";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ad.Fill(DT);
                getSubTaxa(ref DT);
            }
        }

        private static void getSynonyms(ref System.Data.DataTable DT, bool RestrictProjects = false)
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
                string sProjectID = "";
                sProjectID = ProjectID(IDs).ToString();
                //if (RestrictProjects)
                //    sProjectID = ProjectID(IDs).ToString();
                //else
                //    sProjectID = ProjectID().ToString();
                // getting the names that are synonmys to the names in the table
                string SQL = "SELECT S.NameID FROM " + _SC.Prefix() + "TaxonSynonymy S, " + _SC.Prefix() + "TaxonName N WHERE S.NameID = N.NameID " +
                    " AND S.SynNameID IN( " + IDs + ") " +
                    " AND S.ProjectID = " + sProjectID + " AND S.IgnoreButKeepForReference = 0 AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                    " AND S.NameID NOT IN (" + IDs + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ad.Fill(DT);
                    getSynonyms(ref DT);
                }
                // getting the synonyms of the names in the table
                dt.Clear();
                ad.SelectCommand.CommandText = "SELECT S.SynNameID AS NameID FROM " + _SC.Prefix() + "TaxonSynonymy S, " + _SC.Prefix() + "TaxonName N WHERE S.NameID = N.NameID " +
                    " AND S.NameID IN ( " + IDs + ") " +
                    " AND S.ProjectID = " + sProjectID + " AND S.IgnoreButKeepForReference = 0 AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                    " AND S.SynNameID NOT IN (" + IDs + ")";
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ad.Fill(DT);
                    getSynonyms(ref DT, RestrictProjects);
                }
                // getting the names that are based on the names in the table
                dt.Clear();
                ad.SelectCommand.CommandText = "SELECT N.NameID FROM " + _SC.Prefix() + "TaxonName N " +
                    " WHERE N.BasedOnNameID IN ( " + IDs + ") " +
                    " AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                    " AND N.NameID NOT IN (" + IDs + ")";
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ad.Fill(DT);
                    getSynonyms(ref DT, RestrictProjects);
                }
                // getting the Basionyms for the names in the table
                dt.Clear();
                ad.SelectCommand.CommandText = "SELECT N.BasedOnNameID AS NameID FROM " + _SC.Prefix() + "TaxonName N " +
                    " WHERE N.NameID IN ( " + IDs + ") " +
                    " AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                    " AND N.BasedOnNameID NOT IN (" + IDs + ")";
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    ad.Fill(DT);
                    getSynonyms(ref DT, RestrictProjects);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private static string getAcceptedName(ref System.Data.DataTable DT, ref string UrlAcceptedName)
        {
            string Taxon = "";
            try
            {
                if (_SC != null)
                {
                    getSynonyms(ref DT, true);
                    string IDs = "";
                    foreach (System.Data.DataRow R in DT.Rows)
                    {
                        if (IDs.Length > 0) IDs += ", ";
                        IDs += R[0].ToString();
                    }
                    System.Data.DataTable dtProjectID = new System.Data.DataTable();
                    string SQL = "SELECT DISTINCT A.ProjectID FROM " + _SC.Prefix() + "TaxonAcceptedName A, " + _SC.Prefix() + "TaxonName N " +
                        " WHERE A.NameID = N.NameID " +
                        " AND A.IgnoreButKeepForReference = 0 AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                        " AND A.NameID IN (" + IDs + ")";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                    ad.Fill(dtProjectID);
                    string sProjectID = "";
                    if (dtProjectID.Rows.Count > 1)
                        sProjectID = ProjectID().ToString();
                    else
                        sProjectID = dtProjectID.Rows[0][0].ToString();
                    // getting the names that are accepted names in the table
                    SQL = "SELECT N.TaxonNameCache AS Taxon, A.NameID FROM " + _SC.Prefix() + "TaxonAcceptedName A, " + _SC.Prefix() + "TaxonName N WHERE A.NameID = N.NameID " +
                        " AND A.ProjectID = " + sProjectID + " AND A.IgnoreButKeepForReference = 0 AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                        " AND A.NameID IN (" + IDs + ")";
                    // Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                    ad.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Taxon = dt.Rows[0][0].ToString();
                        UrlAcceptedName = _SC.BaseURL + dt.Rows[0][1].ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return Taxon;
        }

        #endregion

        #endregion

        #region Chart

        public static void ResetChart(string URI = "")
        {
            if (_Charts != null)
            {
                if (URI.Length > 0)
                {
                    if (_Charts.ContainsKey(URI))
                        _Charts.Remove(URI);
                }
                else
                {
                    _Charts.Clear();
                }
            }
        }

        private static System.Collections.Generic.Dictionary<string, Chart> _Charts;

        public static System.Collections.Generic.Dictionary<int, string> Sections(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, string> Sect = TaxonName.ChartSections(ProjectID, SC);
            return Sect;
        }

        public static Chart GetChart(string URI, int? ListID = null, int? Height = null, int? Width = null, bool ResetChart = false)//, int? ColumnWidth = null
        {
            if (_Charts == null)
                _Charts = new Dictionary<string, Chart>();

            string ChartMarker = URI;
            if (ListID != null)
                ChartMarker += "-" + ListID.ToString();

            if (_Charts.ContainsKey(ChartMarker))
            {
                if (Width != null && _Charts[ChartMarker].WindowWidth() != (int)Width)
                    _Charts.Remove(ChartMarker);
                else
                    return _Charts[ChartMarker];
            }

            DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);
            if (SC == null)
                return null;

            // Getting the ProjectID
            int ProjectID = 0;
            if (!int.TryParse(URI.Substring(URI.LastIndexOf("/") + 1), out ProjectID))
                return null;

            try
            {
                // Getting the colors
                System.Collections.Generic.Dictionary<int, System.Drawing.Color> ChartColors = TaxonName.ChartColors(ProjectID, SC); //  new Dictionary<int, System.Drawing.Color>();

                System.Collections.Generic.Dictionary<int, System.Drawing.Color> ForeColors = TaxonName.ForeColors(ProjectID, SC); //  new Dictionary<int, System.Drawing.Color>();

                // Getting the images                                                                                                                                                // Getting the images
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> ChartImages = TaxonName.ChartImages(ProjectID, SC);

                // IDs within the section - if a section has been selected
                string SQL = "";
                System.Collections.Generic.List<int> IDsInSection = new List<int>();
                string Section = "";
                if (ListID != null && ListID > 0)
                {
                    SQL = "SELECT NameID FROM " + SC.Prefix() + "TaxonNameList WHERE (ProjectID = " + ListID.ToString() + ")";
                    System.Data.DataTable dtSection = new System.Data.DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSection, SC.ConnectionString);
                    foreach (System.Data.DataRow R in dtSection.Rows)
                    {
                        int id;
                        if (int.TryParse(R[0].ToString(), out id))
                            IDsInSection.Add(id);
                    }
                    SQL = "SELECT Project FROM " + SC.Prefix() + "TaxonNameListProjectProxy WHERE(ProjectID = " + ListID.ToString() + ")";
                    Section = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, SC.ConnectionString);
                }

                // Getting the data
                System.Data.DataTable dtTaxon = TaxonName.ChartData(ProjectID, SC);

                // getting the chart
                System.Collections.Generic.List<string> SortingColumns = new List<string>();
                SortingColumns.Add("TaxonNameCache");

                // Title of the chart
                SQL = "SELECT Project FROM " + SC.Prefix() + "ProjectProxy WHERE (ProjectID = " + ProjectID.ToString() + ")";
                //Markus 22.3.2023: Check for linked server and change target in case
                if (SC.LinkedServer.Length > 0)
                    SQL = "SELECT Project FROM " + SC.Prefix() + "ProjectList WHERE (ProjectID = " + ProjectID.ToString() + ")";

                string Title = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, SC.ConnectionString);

                // additional infos shown in the title
                System.Collections.Generic.Dictionary<int, string> Titles = ChartTitles(ProjectID, SC);

                // getting the BaseURL
                string BaseURL = DiversityWorkbench.WorkbenchUnit.getBaseURIfromURI(URI);

                DiversityWorkbench.Chart C = new DiversityWorkbench.Chart(BaseURL, dtTaxon, "NameID", "NameParentID", "TaxonNameCache",
                    ChartColors, SortingColumns, ChartImages,
                    null,
                    ListID, IDsInSection, Section,
                    Width, Height, //ColumnWidth, 
                    Title, ForeColors, Titles);

                if (!_Charts.ContainsKey(ChartMarker))
                    _Charts.Add(ChartMarker, C);

                return C;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return null;
        }

        private static System.Collections.Generic.Dictionary<int, string> ChartTitles(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, string> Titles = new Dictionary<int, string>();
            try
            {
                System.Data.DataTable dtSyn = new System.Data.DataTable();
                string SQL = "SELECT A.NameID, N.TaxonNameCache " +
                    "FROM " + SC.Prefix() + "TaxonAcceptedName AS A INNER JOIN " +
                    "" + SC.Prefix() + "TaxonSynonymy AS S ON A.ProjectID = S.ProjectID AND A.NameID = S.SynNameID INNER JOIN " +
                    "" + SC.Prefix() + "TaxonName AS N ON S.NameID = N.NameID AND S.ProjectID =  " + ProjectID.ToString() + " " +
                    "WHERE(A.ProjectID = " + ProjectID.ToString() + ") AND(A.IgnoreButKeepForReference = 0) AND(S.IgnoreButKeepForReference = 0) " +
                    "ORDER BY N.TaxonNameCache";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSyn, SC.ConnectionString);
                foreach (System.Data.DataRow R in dtSyn.Rows)
                {
                    int NameID;
                    if (int.TryParse(R[0].ToString(), out NameID))
                    {
                        if (Titles.ContainsKey(NameID))
                        {
                            Titles[NameID] += ",   " + R[1].ToString();
                        }
                        else
                        {
                            Titles.Add(NameID, "Synonyms: " + R[1].ToString());
                        }

                    }
                }
                System.Data.DataTable dtCN = new System.Data.DataTable();
                SQL = "SELECT N.NameID, N.CommonName " +
                    "FROM " + SC.Prefix() + "TaxonCommonName AS N INNER JOIN " + SC.Prefix() + "TaxonNameProject P ON P.NameID = N.NameID AND P.ProjectID =  " + ProjectID.ToString() + " " +
                    "ORDER BY N.CommonName";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtCN, SC.ConnectionString);
                foreach (System.Data.DataRow R in dtCN.Rows)
                {
                    int NameID;
                    if (int.TryParse(R[0].ToString(), out NameID))
                    {
                        if (Titles.ContainsKey(NameID))
                        {
                            if (Titles[NameID].IndexOf("Common names:") == -1)
                                Titles[NameID] += ";   Common names: " + R[1].ToString();
                            else
                                Titles[NameID] += ",   " + R[1].ToString();
                        }
                        else
                        {
                            Titles.Add(NameID, "Common names: " + R[1].ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Titles;
        }


        private static System.Collections.Generic.Dictionary<int, System.Drawing.Color> ChartColors(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, System.Drawing.Color> Colors = new System.Collections.Generic.Dictionary<int, System.Drawing.Color>();
            return Colors;
        }

        private static System.Collections.Generic.Dictionary<int, System.Drawing.Color> ForeColors(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, System.Drawing.Color> Colors = new System.Collections.Generic.Dictionary<int, System.Drawing.Color>();
            try
            {
                string SQL = "SELECT A.NameID, -16744448 AS ARGB " +
                    "FROM " + SC.Prefix() + "TaxonName AS N INNER JOIN " +
                    SC.Prefix() + "TaxonNameTaxonomicRank_Enum AS R ON N.TaxonomicRank = R.Code INNER JOIN " +
                    SC.Prefix() + "TaxonAcceptedName AS A ON N.NameID = A.NameID " +
                    "WHERE (ProjectID =  " + ProjectID.ToString() +
                    ") AND (A.IgnoreButKeepForReference = 0) AND (R.DisplayOrder < 200)";
                System.Data.DataTable dtColor = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtColor, SC.ConnectionString);
                foreach (System.Data.DataRow R in dtColor.Rows)
                {
                    int ID;
                    if (!int.TryParse(R[0].ToString(), out ID))
                        continue;
                    if (Colors.ContainsKey(ID))
                        continue;
                    int Color;
                    if (int.TryParse(R[1].ToString(), out Color))
                    {
                        System.Drawing.SolidBrush S = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(Color));
                        Colors.Add(ID, S.Color);
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Colors;
        }

        private static int? _ListID = null;
        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> ChartImages(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> Images = new Dictionary<int, System.Collections.Generic.List<ChartImage>>();
            try
            {
                int Height = Chart.ImageMaxHeight;
                int Width = Chart.ImageMaxWidth;
                string SQL = "SELECT NameID, URI AS ImageUri, Title, DisplayOrder " +
                    "FROM " + SC.Prefix() + "ViewTaxonNameResource " +
                    "WHERE (ProjectID = " + ProjectID.ToString() + " ) AND (DataWithholdingReason = '' OR DataWithholdingReason IS NULL) " +
                    "ORDER BY DisplayOrder";
                System.Data.DataTable dtImage = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtImage, SC.ConnectionString);
                Images = Chart.getImages(dtImage);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Images;
        }


        private static System.Collections.Generic.Dictionary<int, string> ChartSections(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, string> Sections = new Dictionary<int, string>();
            try
            {
                string SQL = "SELECT S.ProjectID, 'List ' + S.Project " +
                    "FROM " + SC.Prefix() + "TaxonNameListProjectProxy AS S " +
                    "ORDER BY S.Project";
                System.Data.DataTable dtSection = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSection, SC.ConnectionString);
                foreach (System.Data.DataRow R in dtSection.Rows)
                {
                    int ID;
                    if (!int.TryParse(R[0].ToString(), out ID))
                        continue;
                    if (Sections.ContainsKey(ID))
                        continue;
                    Sections.Add(ID, R[1].ToString());
                }
                if (Sections.Count > 0)
                {
                    string Terminology = "";
                    SQL = "SELECT ' ' + Project FROM " + SC.Prefix() + "ProjectProxy WHERE ProjectID = " + ProjectID.ToString();
                    Terminology = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    Sections.Add(-1, Terminology);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Sections;
        }


        private static System.Data.DataTable ChartData(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            // MW - Bugfix getting values from linked server 27.2.23
            string SqlProject = "SELECT Project FROM " + SC.Prefix() + "ProjectList " +
                " WHERE ProjectID = " + ProjectID.ToString();
            string Project = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlProject, SC.ConnectionString).Replace(" ", "_");
            System.Data.DataTable dtTaxon = new System.Data.DataTable(Project);
            try
            {
                string SQL = "SELECT H.NameID, " +
                    "case when H.NameParentID < 0 then null else H.NameParentID end as NameParentID, " +
                    "case when S.Prefix <> '' then ' = ' else '' end + case when R.DisplayOrder < 200 then N.TaxonNameCache else " +
                    "case when R.DisplayOrder < 270 then N.InfragenericEpithet else N.GenusOrSupragenericName end " +
                    "end as TaxonNameCache " +
                    "FROM " + SC.Prefix() + "TaxonHierarchy AS H INNER JOIN " +
                    "" + SC.Prefix() + "TaxonName AS N ON H.NameID = N.NameID " +
                    "INNER JOIN " + SC.Prefix() + "TaxonNameTaxonomicRank_Enum AS R ON N.TaxonomicRank = R.Code " +
                    "LEFT OUTER JOIN " + SC.Prefix() + "TaxonSynonymy_Indicated S ON S.NameID = N.NameID AND S.ProjectID = H.ProjectID " +
                    "WHERE(H.ProjectID = " + ProjectID.ToString() + ") AND (H.IgnoreButKeepForReference = 0) " +
                    "ORDER BY N.TaxonNameCache";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTaxon, SC.ConnectionString);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtTaxon;
        }

        #endregion


    }
}
