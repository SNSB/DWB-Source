//#define CollectionLocactionIDAvailable


using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class LookupTable
    {
        #region Lookup tables

        #region Global functions

        public static void ResetLookupTables()
        {
            ResetAnalysis();
            ResetCollection();
            ResetCountry();
            ResetCountryFromGazetteer();
            ResetDtMaterialCategories();
            ResetDtRetrievalType();
            ResetDtTaxonomicGroups();
            ResetExternalDatasource();
            ResetEventPropertyList();
            ResetLocalisationSystem();
            ResetManagedCollections();
            ResetMaterialCategoryList();
            ResetMethod();
            ResetProcessing();
            ResetProjectList();
            //ResetRegulation();
            ResetTaxonomicGroupList();
            ResetTransaction();
            ResetUser();
        }

        public static void ResetLanguageVariantLookupTables()
        {
            ResetTaxonomicGroupList();
            ResetMaterialCategoryList();
            ResetLocalisationSystem();
            ResetEventPropertyList();
        }
        
        #endregion

        #region Analysis

        private static System.Data.DataTable _DtAnalysis;

        public static System.Data.DataTable DtAnalysis
        {
            get
            {
                if (LookupTable._DtAnalysis == null)
                {
                    string SQL = "SELECT AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI " +
                        "FROM Analysis";
                    LookupTable._DtAnalysis = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtAnalysis);
                }
                return LookupTable._DtAnalysis;
            }
            //set { LookupTable._DtAnalysis = value; }
        }

        private static System.Data.DataTable _DtAnalysisHierarchy;

        public static System.Data.DataTable DtAnalysisHierarchy
        {
            get
            {
                if (LookupTable._DtAnalysisHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS AnalysisID, NULL AS AnalysisParentID, NULL AS DisplayText, NULL AS Description, " +
                        "NULL AS MeasurementUnit, NULL AS Notes, NULL AS AnalysisURI, " +
                        "NULL AS OnlyHierarchy, NULL AS HierarchyDisplayText " +
                        "UNION " +
                        "SELECT AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy, HierarchyDisplayText " +
                        "FROM AnalysisHierarchyAll() " +
                        "ORDER BY HierarchyDisplayText";
                    LookupTable._DtAnalysisHierarchy = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtAnalysisHierarchy);
                }
                return LookupTable._DtAnalysisHierarchy;
            }
        }

        public static System.Data.DataTable Analysis(string TaxonomicGroup)
        {
            System.Data.DataTable DT = DiversityCollection.LookupTable.DtAnalysis.Clone();
            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtAnalysisTaxonomicGroup.Rows)
            {
                if (R["TaxonomicGroup"].ToString() == TaxonomicGroup)
                {
                    foreach (System.Data.DataRow RA in DiversityCollection.LookupTable.DtAnalysis.Rows)
                    {
                        if (RA["AnalysisID"].ToString() == R["AnalysisID"].ToString())
                        {
                            System.Data.DataRow RN = DT.NewRow();
                            foreach (System.Data.DataColumn C in DiversityCollection.LookupTable._DtAnalysis.Columns)
                                RN[C.ColumnName] = RA[C.ColumnName];
                            DT.Rows.Add(RN);
                        }
                    }
                }
            }
            return DT;
        }

        public static System.Data.DataTable Analysis(string TaxonomicGroup, bool IncludeParents)
        {
            System.Collections.Generic.List<string> AnalysisIdList = new List<string>();
            System.Collections.Generic.List<string> AnalysisIdTaxGroupList = new List<string>();
            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtAnalysisTaxonomicGroup.Rows)
            {
                if (R["TaxonomicGroup"].ToString() == TaxonomicGroup)
                {
                    AnalysisIdTaxGroupList.Add(R["AnalysisID"].ToString());
                    if (!AnalysisIdList.Contains(R["AnalysisID"].ToString()))
                    {
                        if (IncludeParents)
                            DiversityCollection.LookupTable.AddAnalysisParentIDsToList(R["AnalysisID"].ToString(), ref AnalysisIdList);
                        else
                            AnalysisIdList.Add(R["AnalysisID"].ToString());
                    }
                }
            }
            System.Data.DataTable DT = DiversityCollection.LookupTable.DtAnalysis.Copy();
            System.Type typeString = System.Type.GetType("System.String");
            System.Data.DataColumn C = new System.Data.DataColumn("TaxonomicGroup", typeString);
            DT.Columns.Add(C);
            foreach (System.Data.DataRow R in DT.Rows)
            {
                if (!AnalysisIdList.Contains(R["AnalysisID"].ToString()))
                    R.Delete();
                else
                {
                    if (AnalysisIdTaxGroupList.Contains(R["AnalysisID"].ToString()))
                        R["TaxonomicGroup"] = TaxonomicGroup;
                }
            }
            DT.AcceptChanges();
            return DT;
        }

        public static string AnalysisTitle(int AnalysisID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtAnalysis.Select("AnalysisID = " + AnalysisID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["DisplayText"].ToString();
            return Title;
        }

        private static void AddAnalysisParentIDsToList(string AnalysisID, ref System.Collections.Generic.List<string> AnalysisIdList)
        {
            if (!AnalysisIdList.Contains(AnalysisID))
                AnalysisIdList.Add(AnalysisID);
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtAnalysis.Select("AnalysisID = " + AnalysisID);
            if (rr.Length > 0)
            {
                if (!rr[0]["AnalysisParentID"].Equals(System.DBNull.Value))
                    DiversityCollection.LookupTable.AddAnalysisParentIDsToList(rr[0]["AnalysisParentID"].ToString(), ref AnalysisIdList);
            }
        }

        private static System.Data.DataTable _DtAnalysisTaxonomicGroup;

        public static string AnalysisMeasurementUnit(string AnalysisID)
        {
            string MU = "";
            foreach (System.Data.DataRow R in LookupTable.DtAnalysis.Rows)
            {
                if (AnalysisID.ToString() == R["AnalysisID"].ToString())
                {
                    MU = R["MeasurementUnit"].ToString();
                    break;
                }
            }
            return MU;
        }

        public static System.Data.DataTable DtAnalysisTaxonomicGroup
        {
            get
            {
                if (LookupTable._DtAnalysisTaxonomicGroup == null)
                {
                    string SQL = "SELECT AnalysisID, TaxonomicGroup FROM AnalysisTaxonomicGroup";
                    LookupTable._DtAnalysisTaxonomicGroup = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtAnalysisTaxonomicGroup);
                }
                return LookupTable._DtAnalysisTaxonomicGroup;
            }
        }

        public static System.Data.DataTable AnalysisForUnit(int UnitID)
        {
            string SQL = "SELECT * FROM dbo.AnalysisListForUnit(" + UnitID.ToString() + ") ORDER BY DisplayText";
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dt);
            }
            catch(System.Exception ex) 
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dt;
        }

        public static void ResetAnalysis()
        {
            DiversityCollection.LookupTable._DtAnalysis = null;
            DiversityCollection.LookupTable._DtAnalysisTaxonomicGroup = null;
            DiversityCollection.LookupTable._DtAnalysisHierarchy = null;
        }

        #region Analysis Result
        
        public static bool AnalysisResultsAreRestrictedToList(int AnalysisID)
        {
            bool Restricted = false;
            string SQL = "SELECT AnalysisResult, CASE WHEN DisplayText IS NULL THEN AnalysisResult ELSE DisplayText END AS DisplayText FROM dbo.AnalysisResult WHERE AnalysisID = " + AnalysisID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dt);
                if (dt.Rows.Count > 0) Restricted = true;
                else Restricted = false;
            }
            catch { Restricted = false; }
            return Restricted;
        }

        public static System.Data.DataTable AnalysisResults(int AnalysisID, ref bool RestrictToList)
        {
            string SQL = "SELECT AnalysisResult, CASE WHEN DisplayText IS NULL THEN AnalysisResult ELSE DisplayText END AS DisplayText FROM dbo.AnalysisResult WHERE AnalysisID = " + AnalysisID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dt);
                if (dt.Rows.Count > 0) RestrictToList = true;
                else RestrictToList = false;
            }
            catch { RestrictToList = false; }
            return dt;
        }

        public static System.Data.DataTable AnalysisResults(int AnalysisID)
        {
            string SQL = "SELECT AnalysisResult, CASE WHEN DisplayText IS NULL THEN AnalysisResult ELSE DisplayText END AS DisplayText FROM dbo.AnalysisResult WHERE AnalysisID = " + AnalysisID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dt);
            }
            catch { }
            return dt;
        }

        public static System.Data.DataTable AnalysisResults()
        {
            string SQL = "SELECT NULL AS AnalysisID, NULL AS AnalysisResult, NULL AS DisplayText UNION " +
                "SELECT AnalysisID, AnalysisResult, CASE WHEN DisplayText IS NULL THEN AnalysisResult ELSE DisplayText END AS DisplayText FROM dbo.AnalysisResult " +
                "ORDER BY DisplayText";
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dt);
            }
            catch (System.Exception ex) { }
            return dt;
        }

        public static void AnalysisResultsFill(int AnalysisID, System.Data.DataTable DT)
        {
            string SQL = "SELECT AnalysisResult, CASE WHEN DisplayText IS NULL THEN AnalysisResult ELSE DisplayText END AS DisplayText FROM dbo.AnalysisResult WHERE AnalysisID = " + AnalysisID.ToString();
            DT.Clear();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(DT);
            }
            catch { }
        }

        public static string AnalysisResultsDisplayText(int AnalysisID, string ResultValue)
        {
            string SQL = "SELECT DisplayText FROM dbo.AnalysisResult WHERE AnalysisID = " + AnalysisID.ToString() + " AND AnalysisResult = '" + ResultValue + "'";
            string DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL); //System.Data.DataTable dt = new System.Data.DataTable();
            if (DisplayText.Length == 0)
                DisplayText = ResultValue;
            return DisplayText;
        }
        
        #endregion

        #endregion

        #region Transaction

        public enum TransactionDirection { From, To};

        private static System.Data.DataTable _DtTransaction;

        public static System.Data.DataTable DtTransaction
        {
            get
            {
                if (LookupTable._DtTransaction == null)
                {
                    string SQL = "SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, " +
                        "ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, " +
                        "MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, " +
                        "FromTransactionNumber, ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, " +
                        "ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, " +
                        "AgreedEndDate, ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI " +
                        "FROM [Transaction]";
                    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        LookupTable._DtTransaction = new System.Data.DataTable();
                        try
                        {
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            ad.Fill(LookupTable._DtTransaction);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
                return LookupTable._DtTransaction;
            }
            //set { LookupTable._DtTransaction = value; }
        }

        private static System.Data.DataTable _DtTransactionHierarchy;

        public static System.Data.DataTable DtTransactionHierarchy
        {
            get
            {
                if (LookupTable._DtTransactionHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "exec DBO.sp_TransactionHierarchyAll 1";
                    //string SQL = "SELECT NULL AS TransactionID, NULL AS ParentTransactionID, NULL AS TransactionType, NULL AS TransactionTitle, NULL AS ReportingCategory, NULL AS AdministratingCollectionID, NULL AS MaterialDescription, NULL AS MaterialCategory, " +
                    //    "NULL AS MaterialCollectors, NULL AS FromCollectionID, NULL AS FromTransactionPartnerName, NULL AS FromTransactionPartnerAgentURI, NULL AS FromTransactionNumber, NULL AS ToCollectionID, " +
                    //    "NULL AS ToTransactionPartnerName, NULL AS ToTransactionPartnerAgentURI, NULL AS ToTransactionNumber, NULL AS NumberOfUnits, NULL AS Investigator, NULL AS TransactionComment, NULL AS BeginDate, NULL AS AgreedEndDate, " +
                    //    "NULL AS ActualEndDate, NULL AS InternalNotes, NULL AS ResponsibleName, NULL AS ResponsibleAgentURI, NULL AS HierarchyDisplayText " +
                    //    "UNION " +
                    //    "SELECT TransactionID, ParentTransactionID, TransactionType, TransactionTitle, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, " +
                    //    "MaterialCollectors, FromCollectionID, FromTransactionPartnerName, FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, " +
                    //    "ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, BeginDate, AgreedEndDate, " +
                    //    "ActualEndDate, InternalNotes, ResponsibleName, ResponsibleAgentURI, DisplayText AS HierarchyDisplayText " +
                    //    "FROM TransactionHierarchyAll() " +
                    //    "ORDER BY HierarchyDisplayText";
                    LookupTable._DtTransactionHierarchy = new System.Data.DataTable();
                    try
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(LookupTable._DtTransactionHierarchy);
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return LookupTable._DtTransactionHierarchy;
            }
        }

        public static string TransactionType(int TransactionID)
        {
            string Type = "";
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransactionHierarchy.Select("TransactionID = " + TransactionID.ToString());
            if (RR.Length > 0)
                Type = RR[0]["TransactionType"].ToString();
            return Type;
        }

        public static bool TransactionInActivePeriod(int TransactionID)
        {
            bool IsActive = false;
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
            if (RR.Length > 0 && !RR[0]["BeginDate"].Equals(System.DBNull.Value) || !RR[0]["AgreedEndDate"].Equals(System.DBNull.Value))
            {
                System.DateTime Begin = System.DateTime.Now;
                System.DateTime End = System.DateTime.Now.AddDays(1);
                if (!RR[0]["BeginDate"].Equals(System.DBNull.Value) && RR[0]["BeginDate"].ToString().Length > 0)
                    System.DateTime.TryParse(RR[0]["BeginDate"].ToString(), out Begin);
                if (!RR[0]["AgreedEndDate"].Equals(System.DBNull.Value) && RR[0]["AgreedEndDate"].ToString().Length > 0)
                    System.DateTime.TryParse(RR[0]["AgreedEndDate"].ToString(), out End);
                if (Begin <= System.DateTime.Now && End > System.DateTime.Now)
                    IsActive = true;
            }
            return IsActive;
        }

        public static string TransactionTitle(int TransactionID)
        {
            string Title = "";
            try
            {
                System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
                if (rr.Length > 0)
                {
                    string TransactionType = rr[0]["TransactionType"].ToString();
                    if(TransactionType.ToLower() == "regulation")
                    {
                        return rr[0]["TransactionTitle"].ToString();
                    }
                    TransactionDisplaySorter TS = TransactionDisplaySorter.BeginDate;
                    if (TransactionDisplaySorting().ContainsKey(TransactionType))
                        TS = TransactionDisplaySorting()[TransactionType];
                    else
                        TS = TransactionDisplaySorterParent(TransactionID);
                    string TransactionTitle = "";
                    if (TransactionType == "regulation")
                    {
                        string SQL = "SELECT TOP 1 TransactionTitle FROM TransactionRegulation WHERE TransactionID = " + TransactionID.ToString();
                        TransactionTitle = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL); ;
                    }
                    else
                        TransactionTitle = rr[0]["TransactionTitle"].ToString();
                    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeParentTitle)
                    {
                        int ParentID = -1;
                        int.TryParse(rr[0]["ParentTransactionID"].ToString(), out ParentID);
                        while (ParentID > -1)
                        {
                            System.Data.DataRow[] rrP = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + ParentID.ToString());
                            if (rrP.Length > 0)
                            {
                                TransactionTitle = rrP[0]["TransactionTitle"].ToString() + " - " + TransactionTitle;
                                if (!int.TryParse(rrP[0]["ParentTransactionID"].ToString(), out ParentID))
                                    ParentID = -1;
                            }
                            else
                                ParentID = -1;
                        }
                    }
                    string BeginDate = DiversityCollection.LookupTable.TransactionDateAsString(TransactionID, "BeginDate", false);
                    string AgreedEndDate = DiversityCollection.LookupTable.TransactionDateAsString(TransactionID, "AgreedEndDate", false);
                    string Date = BeginDate;
                    if (AgreedEndDate.Length > 0)
                    {
                        if (Date.Length > 0) Date += " ";
                        Date += "- " + AgreedEndDate;
                    }
                    //if (Date.Length == 0)
                    //    Date = "?";
                    switch (TS)
                    {
                        case TransactionDisplaySorter.TransactionTitle:
                            Title = TransactionTitle;
                            if (Date.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeDateInTitle)
                                Title += "; " + Date;
                            break;
                        case TransactionDisplaySorter.BeginDate:
                            if (Date.Length > 0)
                                Title = Date + "; ";
                            Title += TransactionTitle;
                            break;
                        default:
                            Title = TransactionTitle;
                            if (Date.Length > 0 && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeDateInTitle)
                                Title += "; " + Date;
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return Title;
        }

        public static string TransactionDateAsString(int TransactionID, string DataColumn, bool IncludeTime)
        {
            string Date = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
            if (rr.Length > 0)
            {
                if (!rr[0][DataColumn].Equals(System.DBNull.Value))
                {
                    System.DateTime d;
                    if (System.DateTime.TryParse(rr[0][DataColumn].ToString(), out d))
                    {
                        if (IncludeTime)
                            Date = d.ToString("yyyy-MM-dd HH:mm:ss");
                        else
                            Date = d.ToString("yyyy-MM-dd");
                        //Date = d.Year.ToString() + "-";
                        //if(d.Month < 10)
                        //    Date += "0";
                        //Date += d.Month.ToString() + "-";
                        //if (d.Day < 10)
                        //    Date += "0";
                        //Date += d.Day.ToString();
                        //if (IncludeTime)
                        //{
                        //    Date += " ";
                        //    if (d.Hour < 10)
                        //        Date += "0";
                        //    Date += d.Hour.ToString() + ":";
                        //    if (d.Minute < 10)
                        //        Date += "0";
                        //    Date += d.Minute.ToString() + ":";
                        //    if (d.Second < 10)
                        //        Date += "0";
                        //    Date += d.Second.ToString();
                        //}
                    }
                }
            }
            return Date;
        }

        /// <summary>
        /// URI of the link to DiversityAgents of the current transaction 
        /// or the first transaction in the hierarchy above it with an URI linked to DiversityAgents
        /// </summary>
        /// <param name="TransactionID">The ID of the transaction</param>
        /// <param name="Direction">The direction, either from of to</param>
        /// <returns>The URI</returns>
        public static string TransactionAgentURI(int TransactionID, TransactionDirection Direction)
        {
            string URI = "";
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
            switch (Direction)
            {
                case TransactionDirection.To:
                    if (!RR[0]["ToTransactionPartnerAgentURI"].Equals(System.DBNull.Value)) 
                        return  RR[0]["FromTransactionPartnerAgentURI"].ToString();
                    break;
                case TransactionDirection.From:
                    if(!RR[0]["FromTransactionPartnerAgentURI"].Equals(System.DBNull.Value))
                        return RR[0]["FromTransactionPartnerAgentURI"].ToString();
                    break;
            }

            if (!RR[0]["ParentTransactionID"].Equals(System.DBNull.Value))
            {
                int? ParentTransactionID = int.Parse(RR[0]["ParentTransactionID"].ToString());
                while (ParentTransactionID != null)
                {
                    System.Data.DataRow[] RRParent = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + ParentTransactionID.ToString());
                    switch (Direction)
                    {
                        case TransactionDirection.To:
                            if (!RRParent[0]["ToTransactionPartnerAgentURI"].Equals(System.DBNull.Value))
                                return RRParent[0]["FromTransactionPartnerAgentURI"].ToString();
                            break;
                        case TransactionDirection.From:
                            if (!RRParent[0]["FromTransactionPartnerAgentURI"].Equals(System.DBNull.Value))
                                return RRParent[0]["FromTransactionPartnerAgentURI"].ToString();
                            break;
                    }
                    int ID;
                    if (!int.TryParse(RRParent[0]["ParentTransactionID"].ToString(), out ID)
                        || RRParent[0]["ParentTransactionID"].Equals(System.DBNull.Value))
                    {
                        ParentTransactionID = null;
                        return URI;
                    }
                    else
                        ParentTransactionID = ID;
                }
            }
            return URI;
        }

        public static int? TransactionAdministratingCollectionID(int TransactionID)
        {
            int? CollectionID = null;
            int CollID = 0;
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
            if (!RR[0]["AdministratingCollectionID"].Equals(System.DBNull.Value))
            {
                if (int.TryParse(RR[0]["AdministratingCollectionID"].ToString(), out CollID))
                    return CollID;
                else return null;
            }
            if (!RR[0]["ParentTransactionID"].Equals(System.DBNull.Value))
            {
                int? ParentTransactionID = int.Parse(RR[0]["ParentTransactionID"].ToString());
                int iCurrent = 0;
                while (ParentTransactionID != null && iCurrent < DiversityCollection.LookupTable.DtTransaction.Rows.Count)
                {
                    System.Data.DataRow[] RRParent = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + ParentTransactionID.ToString());
                    if (!RRParent[0]["AdministratingCollectionID"].Equals(System.DBNull.Value))
                    {
                        if (int.TryParse(RRParent[0]["AdministratingCollectionID"].ToString(), out CollID))
                            return CollID;
                        else return null;
                    }
                    iCurrent++;
                }
            }
            return CollectionID;
        }

        public static string TransactionNumber(int TransactionID, int CollectionID)
        {
            string Number = "";
            int? FromCollectionID = null;
            int? ToCollectionID = null;
            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtTransaction.Rows)
            {
                if (R["TransactionID"].ToString() == TransactionID.ToString())
                {
                    if (!R["ToCollectionID"].Equals(System.DBNull.Value)) ToCollectionID = int.Parse(R["ToCollectionID"].ToString());
                    if (!R["FromCollectionID"].Equals(System.DBNull.Value)) FromCollectionID = int.Parse(R["FromCollectionID"].ToString());
                    if (R["ToCollectionID"].ToString() == CollectionID.ToString())
                        Number = R["ToTransactionNumber"].ToString();
                    else if (R["FromCollectionID"].ToString() == CollectionID.ToString())
                        Number = R["FromTransactionNumber"].ToString();
                    if (Number.Length == 0)
                    {
                        if (ToCollectionID != null && IsChildOfCollection((int)CollectionID, (int)ToCollectionID))
                        {
                            Number = R["ToTransactionNumber"].ToString();
                        }
                        else if (FromCollectionID != null && IsChildOfCollection((int)CollectionID, (int)FromCollectionID))
                        {
                            Number = R["FromTransactionNumber"].ToString();
                        }
                    }
                    break;
                }
            }
            return Number;
        }

        public static string TransactionAgentURI(int TransactionID, bool IsFrom)
        {
            string URI = "";
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
            if (RR.Length > 0)
            {
                if (IsFrom && !RR[0]["FromTransactionPartnerAgentURI"].Equals(System.DBNull.Value)) URI = RR[0]["FromTransactionPartnerAgentURI"].ToString();
                else if (!IsFrom && !RR[0]["ToTransactionPartnerAgentURI"].Equals(System.DBNull.Value)) URI = RR[0]["ToTransactionPartnerAgentURI"].ToString();
                if (URI.Length == 0)
                {
                    //while (URI.Length == 0)
                        URI = DiversityCollection.LookupTable.TransactionAgentURI(TransactionID, IsFrom, ref URI);
                }
            }
            if (URI.Length == 0 && RR.Length > 0)
            {
                if (IsFrom && !RR[0]["FromCollectionID"].Equals(System.DBNull.Value)) URI = DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(int.Parse(RR[0]["FromCollectionID"].ToString()));
                else if (!IsFrom && !RR[0]["ToCollectionID"].Equals(System.DBNull.Value)) URI = DiversityCollection.LookupTable.CollectionAdministrativeContactAgentURI(int.Parse(RR[0]["ToCollectionID"].ToString()));
            }
            return URI;
        }

        public static string TransactionAgentURI(int TransactionID, bool IsFrom, ref string URI)
        {
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
            if (RR.Length > 0)
            {
                if (IsFrom && !RR[0]["FromTransactionPartnerAgentURI"].Equals(System.DBNull.Value)) URI = RR[0]["FromTransactionPartnerAgentURI"].ToString();
                else if (!IsFrom && !RR[0]["ToTransactionPartnerAgentURI"].Equals(System.DBNull.Value)) URI = RR[0]["ToTransactionPartnerAgentURI"].ToString();
                if (URI.Length > 0)
                    return URI;
                else if (URI.Length == 0 && !RR[0]["ParentTransactionID"].Equals(System.DBNull.Value))
                {
                    int ParentTransactionID = 0;
                    if (int.TryParse(RR[0]["ParentTransactionID"].ToString(), out ParentTransactionID))
                    {
                        URI = DiversityCollection.LookupTable.TransactionAgentURI(ParentTransactionID, IsFrom, ref URI);
                    }
                }
            }
            return URI;
        }

        public static int? TransactionCollectionID(int TransactionID, bool IsFrom)
        {
            int? CollectionID = null;
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
            if (RR.Length > 0)
            {
                if (IsFrom && !RR[0]["FromCollectionID"].Equals(System.DBNull.Value)) CollectionID = int.Parse(RR[0]["FromCollectionID"].ToString());
                else if (!IsFrom && !RR[0]["ToCollectionID"].Equals(System.DBNull.Value)) CollectionID = int.Parse(RR[0]["ToCollectionID"].ToString());
                if (CollectionID == null)
                {
                    CollectionID = DiversityCollection.LookupTable.TransactionCollectionID(TransactionID, IsFrom, ref CollectionID);
                }
            }
            return CollectionID;
        }

        public static int? TransactionCollectionID(int TransactionID, bool IsFrom, ref int? CollectionID)
        {
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransaction.Select("TransactionID = " + TransactionID.ToString());
            if (RR.Length > 0)
            {
                if (IsFrom && !RR[0]["FromCollectionID"].Equals(System.DBNull.Value)) CollectionID = int.Parse(RR[0]["FromCollectionID"].ToString());
                else if (!IsFrom && !RR[0]["ToCollectionID"].Equals(System.DBNull.Value)) CollectionID = int.Parse(RR[0]["ToCollectionID"].ToString());
                if (CollectionID != null)
                    return CollectionID;
                else if (CollectionID == null && !RR[0]["ParentTransactionID"].Equals(System.DBNull.Value))
                {
                    int ParentTransactionID = 0;
                    if (int.TryParse(RR[0]["ParentTransactionID"].ToString(), out ParentTransactionID))
                    {
                        CollectionID = DiversityCollection.LookupTable.TransactionCollectionID(ParentTransactionID, IsFrom, ref CollectionID);
                    }
                }
            }
            return CollectionID;
        }

        public static void ResetTransaction()
        {
            DiversityCollection.LookupTable._DtTransaction = null;
            DiversityCollection.LookupTable._DtTransactionHierarchy = null;
        }

        public enum TransactionDisplaySorter { BeginDate, AccessionNumber, TransactionTitle }
        private static System.Collections.Generic.Dictionary<string, TransactionDisplaySorter> _TransactionDisplaySorting;
        public static System.Collections.Generic.Dictionary<string, TransactionDisplaySorter> TransactionDisplaySorting()
        {
            if (DiversityCollection.LookupTable._TransactionDisplaySorting == null)
            {
                DiversityCollection.LookupTable._TransactionDisplaySorting = new Dictionary<string, TransactionDisplaySorter>();
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence != null &&
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence.Count > 0)
                {
                    foreach (string s in DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence)
                    {
                        string[] ss = s.Split(new char[] { '|' });
                        TransactionDisplaySorter TS = TransactionDisplaySorter.BeginDate;
                        if (ss[1].ToString() == TransactionDisplaySorter.AccessionNumber.ToString())
                            TS = TransactionDisplaySorter.AccessionNumber;
                        else if (ss[1].ToString() == TransactionDisplaySorter.TransactionTitle.ToString())
                            TS = TransactionDisplaySorter.TransactionTitle;
                        if (!DiversityCollection.LookupTable._TransactionDisplaySorting.ContainsKey(ss[0]))
                            DiversityCollection.LookupTable._TransactionDisplaySorting.Add(ss[0].ToString(), TS);
                    }
                }
                else
                {
                    DiversityCollection.LookupTable._TransactionDisplaySorting = new Dictionary<string, TransactionDisplaySorter>();
                    string SQL = "SELECT Code, DisplayOrder, InternalNotes " +
                        "FROM  CollTransactionType_Enum " +
                        "WHERE (DisplayEnable = 1) " +
                        "and not DisplayOrder is null " +
                        "and Code not in ('return', 'forwarding', 'transaction group') " +
                        "ORDER BY DisplayOrder";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        SQL = "SELECT Code, ROW_NUMBER() over (ORDER BY Code) AS Number, InternalNotes " +
                            "FROM CollTransactionType_Enum " +
                            "WHERE Code not in ('return', 'forwarding', 'transaction group') " +
                            "ORDER BY Code";
                        ad.SelectCommand.CommandText = SQL;
                        ad.Fill(dt);
                    }
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        TransactionDisplaySorter TS = TransactionDisplaySorter.BeginDate;
                        if (R[2].ToString() == TransactionDisplaySorter.AccessionNumber.ToString())
                            TS = TransactionDisplaySorter.AccessionNumber;
                        else if (R[2].ToString() == TransactionDisplaySorter.TransactionTitle.ToString())
                            TS = TransactionDisplaySorter.TransactionTitle;
                        DiversityCollection.LookupTable._TransactionDisplaySorting.Add(R[0].ToString(), TS);
                    }
                }
            }
            return DiversityCollection.LookupTable._TransactionDisplaySorting;
        }

        public static void TransactionDisplaySortingSetSorter(string Type, TransactionDisplaySorter Sorter)
        {
            DiversityCollection.LookupTable._TransactionDisplaySorting[Type] = Sorter;
        }

        public static TransactionDisplaySorter TransactionDisplaySorterParent(int TransactionID)
        {
            TransactionDisplaySorter Sorter = TransactionDisplaySorter.BeginDate;
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTransactionHierarchy.Select("TransactionID = " + TransactionID.ToString());
            if (TransactionDisplaySorting().ContainsKey(RR[0]["TransactionType"].ToString()))
                Sorter = TransactionDisplaySorting()[RR[0]["TransactionType"].ToString()];
            else if (!RR[0]["ParentTransactionID"].Equals(System.DBNull.Value))
            {
                System.Data.DataRow[] rrP = DiversityCollection.LookupTable.DtTransactionHierarchy.Select("TransactionID = " + RR[0]["ParentTransactionID"].ToString());
                if (TransactionDisplaySorting().ContainsKey(rrP[0]["TransactionType"].ToString()))
                    Sorter = TransactionDisplaySorting()[rrP[0]["TransactionType"].ToString()];
                else
                    Sorter = TransactionDisplaySorterParent(int.Parse(rrP[0]["TransactionID"].ToString()));
            }
            return Sorter;
        }

        public static int TransactionDisplaySortingMove(string Type, bool Upwards)
        {
            System.Collections.Generic.SortedDictionary<int, string> NewDisplaySorting = new SortedDictionary<int,string>();
            int NewPosition = 0;
            int i = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, TransactionDisplaySorter> KV in _TransactionDisplaySorting )
            {
                if (Type == KV.Key)
                {
                    NewPosition = i / 2;
                    if (Upwards)
                    {
                        NewDisplaySorting.Add(i - 3, KV.Key);
                        NewPosition--;
                    }
                    else
                    {
                        NewDisplaySorting.Add(i + 3, KV.Key);
                        NewPosition++;
                    }
                }
                else
                    NewDisplaySorting.Add(i, KV.Key);
                i = i+2;
            }
            System.Collections.Generic.Dictionary<string, TransactionDisplaySorter> NewTransactionDisplaySorting = new Dictionary<string, TransactionDisplaySorter>();
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in NewDisplaySorting)
                NewTransactionDisplaySorting.Add(KV.Value, _TransactionDisplaySorting[KV.Value]);
            _TransactionDisplaySorting.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, TransactionDisplaySorter> KV in NewTransactionDisplaySorting)
                _TransactionDisplaySorting.Add(KV.Key, KV.Value);
            return NewPosition;
        }

        public static void TransactionDisplaySortingReset()
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence = null;
            DiversityCollection.LookupTable._TransactionDisplaySorting = null;
        }

        public static void TransactionDisplaySortingSave()
        {
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence == null)
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence = new System.Collections.Specialized.StringCollection();
            else
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, TransactionDisplaySorter> KV in DiversityCollection.LookupTable.TransactionDisplaySorting())
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence.Add(KV.Key + "|" + KV.Value.ToString());
        }

        #endregion

        #region Processing

        private static System.Data.DataTable _DtProcessing;

        public static System.Data.DataTable DtProcessing
        {
            get
            {
                if (LookupTable._DtProcessing == null)
                {
                    string SQL = "SELECT ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI " +
                        "FROM Processing";
                    LookupTable._DtProcessing = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtProcessing);
                }
                return LookupTable._DtProcessing;
            }
            //set { LookupTable._Processing = value; }
        }

        private static System.Data.DataTable _DtProcessingHierarchy;

        public static System.Data.DataTable DtProcessingHierarchy
        {
            get
            {
                if (LookupTable._DtProcessingHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS ProcessingID, NULL AS ProcessingParentID, NULL AS DisplayText, NULL AS Description, " +
                        "NULL AS Notes, NULL AS ProcessingURI, " +
                        "NULL AS OnlyHierarchy, NULL AS HierarchyDisplayText " +
                        "UNION " +
                        "SELECT ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI, OnlyHierarchy, HierarchyDisplayText " +
                        "FROM ProcessingHierarchyAll() " +
                        "ORDER BY HierarchyDisplayText";
                    LookupTable._DtProcessingHierarchy = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtProcessingHierarchy);
                }
                return LookupTable._DtProcessingHierarchy;
            }
        }

        public static string ProcessingName(int ProcessingID)
        {
            string Name = "";
            try
            {
                foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtProcessing.Rows)
                {
                    if (R["ProcessingID"].ToString() == ProcessingID.ToString())
                    {
                        Name = R["DisplayText"].ToString();
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Name;
        }

        public static System.Data.DataTable Processing(string MaterialCategory)
        {
            System.Data.DataTable DT = DiversityCollection.LookupTable.DtProcessing.Clone();
            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtProcessingMaterialCategory.Rows)
            {
                if (R["MaterialCategory"].ToString() == MaterialCategory)
                {
                    foreach (System.Data.DataRow RA in DiversityCollection.LookupTable.DtProcessing.Rows)
                    {
                        if (RA["ProcessingID"].ToString() == R["ProcessingID"].ToString())
                        {
                            System.Data.DataRow RN = DT.NewRow();
                            foreach (System.Data.DataColumn C in DiversityCollection.LookupTable._DtProcessing.Columns)
                                RN[C.ColumnName] = RA[C.ColumnName];
                            DT.Rows.Add(RN);
                        }
                    }
                }
            }
            return DT;
        }

        public static System.Data.DataTable Processing(string MaterialCategory, bool IncludeParents)
        {
            System.Collections.Generic.List<string> ProcessingIdList = new List<string>();
            System.Collections.Generic.List<string> ProcessingIdMatCatList = new List<string>();
            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtProcessingMaterialCategory.Rows)
            {
                if (R["MaterialCategory"].ToString() == MaterialCategory)
                {
                    ProcessingIdMatCatList.Add(R["ProcessingID"].ToString());
                    if (!ProcessingIdList.Contains(R["ProcessingID"].ToString()))
                    {
                        if (IncludeParents)
                            DiversityCollection.LookupTable.AddProcessingParentIDsToList(R["ProcessingID"].ToString(), ref ProcessingIdList);
                        else
                            ProcessingIdList.Add(R["ProcessingID"].ToString());
                    }
                }
            }
            System.Data.DataTable DT = DiversityCollection.LookupTable.DtProcessing.Copy();
            System.Type typeString = System.Type.GetType("System.String");
            System.Data.DataColumn C = new System.Data.DataColumn("MaterialCategory", typeString);
            DT.Columns.Add(C);
            foreach (System.Data.DataRow R in DT.Rows)
            {
                if (!ProcessingIdList.Contains(R["ProcessingID"].ToString()))
                    R.Delete();
                else
                {
                    if (ProcessingIdMatCatList.Contains(R["ProcessingID"].ToString()))
                        R["MaterialCategory"] = MaterialCategory;
                }
            }
            DT.AcceptChanges();
            return DT;
        }

        private static void AddProcessingParentIDsToList(string ProcessingID, ref System.Collections.Generic.List<string> ProcessingIdList)
        {
            if (!ProcessingIdList.Contains(ProcessingID))
                ProcessingIdList.Add(ProcessingID);
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtProcessing.Select("ProcessingID = " + ProcessingID);
            if (rr.Length > 0)
            {
                if (!rr[0]["ProcessingParentID"].Equals(System.DBNull.Value))
                    DiversityCollection.LookupTable.AddProcessingParentIDsToList(rr[0]["ProcessingParentID"].ToString(), ref ProcessingIdList);
            }
        }

        private static System.Data.DataTable _DtProcessingMaterialCategory;

        public static System.Data.DataTable DtProcessingMaterialCategory
        {
            get
            {
                if (LookupTable._DtProcessingMaterialCategory == null)
                {
                    string SQL = "SELECT ProcessingID, MaterialCategory FROM ProcessingMaterialCategory";
                    LookupTable._DtProcessingMaterialCategory = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtProcessingMaterialCategory);
                }
                return LookupTable._DtProcessingMaterialCategory;
            }
        }

        public static void ResetProcessing()
        {
            DiversityCollection.LookupTable._DtProcessing = null;
            DiversityCollection.LookupTable._DtProcessingMaterialCategory = null;
            DiversityCollection.LookupTable._DtProcessingHierarchy = null;
        }

        public static System.Data.DataTable ProcessingForPart(int CollectionSpecimenID, int? PartID)
        {
            string SQL = "SELECT * FROM dbo.ProcessingListForPart(" + CollectionSpecimenID.ToString() + ", ";
            if (PartID == null) SQL += " NULL)";
            else SQL += PartID.ToString() + ")";
            SQL += " ORDER BY DisplayText";
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dt);
            }
            catch { }
            return dt;
        }
        
        #endregion

        #region Tool

        private static System.Data.DataTable _DtTool;

        public static System.Data.DataTable DtTool
        {
            get
            {
                if (LookupTable._DtTool == null)
                {
                    string SQL = "SELECT ToolID, ToolParentID, Name, Description, ToolUsageTemplate, Notes, ToolURI " +
                        "FROM Tool";
                    LookupTable._DtTool = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtTool);
                }
                return LookupTable._DtTool;
            }
            //set { LookupTable._DtTool = value; }
        }

        private static System.Data.DataTable _DtToolHierarchy;

        public static System.Data.DataTable DtToolHierarchy
        {
            get
            {
                if (LookupTable._DtToolHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS ToolID, NULL AS ToolParentID, NULL AS Name, NULL AS Description, " +
                        "NULL AS ToolUsageTemplate, NULL AS Notes, NULL AS ToolURI, NULL AS OnlyHierarchy, " +
                        "NULL AS HierarchyDisplayText " +
                        "UNION ALL " +
                        "SELECT ToolID, ToolParentID, Name, Description, ToolUsageTemplate, Notes, ToolURI, OnlyHierarchy, HierarchyDisplayText " +
                        "FROM dbo.ToolHierarchyAll() " +
                        "ORDER BY HierarchyDisplayText";
                    LookupTable._DtToolHierarchy = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtToolHierarchy);
                }
                return LookupTable._DtToolHierarchy;
            }
        }

        public static System.Data.DataTable ToolsForProcessing(int ProcessingID)
        {
            System.Data.DataTable dtProcessing = new System.Data.DataTable();
            string SQL = "SELECT T.ToolID, T.ToolParentID, T.Name, T.Description, T.ToolURI, T.ToolUsageTemplate, T.Notes " +
                "FROM ToolForProcessing AS P INNER JOIN " +
                "Tool AS T ON P.ToolID = T.ToolID " +
                "WHERE P.ProcessingID = " + ProcessingID.ToString();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtProcessing);
            return dtProcessing;
        }

        public static string ToolTitle(int ToolID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTool.Select("ToolID = " + ToolID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["Name"].ToString();
            return Title;
        }

        private static void AddToolParentIDsToList(string ToolID, ref System.Collections.Generic.List<string> ToolIdList)
        {
            if (!ToolIdList.Contains(ToolID))
                ToolIdList.Add(ToolID);
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTool.Select("ToolID = " + ToolID);
            if (rr.Length > 0)
            {
                if (!rr[0]["ToolParentID"].Equals(System.DBNull.Value))
                    DiversityCollection.LookupTable.AddToolParentIDsToList(rr[0]["ToolParentID"].ToString(), ref ToolIdList);
            }
        }

        public static void ResetTool()
        {
            DiversityCollection.LookupTable._DtTool = null;
            DiversityCollection.LookupTable._DtToolHierarchy = null;
        }

        #endregion

        #region Method

        private static System.Data.DataTable _DtMethod;

        public static System.Data.DataTable DtMethod
        {
            get
            {
                if (LookupTable._DtMethod == null)
                {
                    string SQL = "SELECT MethodID, MethodParentID, DisplayText, Description, Notes, MethodURI, ForCollectionEvent, OnlyHierarchy " +
                        "FROM Method";
                    LookupTable._DtMethod = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtMethod);
                }
                return LookupTable._DtMethod;
            }
        }

        private static System.Data.DataTable _DtMethodHierarchy;

        public static System.Data.DataTable DtMethodHierarchy
        {
            get
            {
                if (LookupTable._DtMethodHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS MethodID, NULL AS MethodParentID, NULL AS DisplayText, NULL AS Description, " +
                        "NULL AS Notes, NULL AS MethodURI, NULL AS ForCollectionEvent, NULL AS OnlyHierarchy, " +
                        "NULL AS HierarchyDisplayText " +
                        "UNION ALL " +
                        "SELECT MethodID, MethodParentID, DisplayText, Description, Notes, MethodURI, ForCollectionEvent, OnlyHierarchy, HierarchyDisplayText " +
                        "FROM dbo.MethodHierarchyAll() " +
                        "ORDER BY HierarchyDisplayText";
                    LookupTable._DtMethodHierarchy = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtMethodHierarchy);
                    }
                    catch (System.Exception ex) { }
                }
                return LookupTable._DtMethodHierarchy;
            }
        }

        public static string MethodTitle(int MethodID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtMethod.Select("MethodID = " + MethodID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["Name"].ToString();
            return Title;
        }

        private static void AddMethodParentIDsToList(string MethodID, ref System.Collections.Generic.List<string> MethodIdList)
        {
            if (!MethodIdList.Contains(MethodID))
                MethodIdList.Add(MethodID);
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtMethod.Select("MethodID = " + MethodID);
            if (rr.Length > 0)
            {
                if (!rr[0]["MethodParentID"].Equals(System.DBNull.Value))
                    DiversityCollection.LookupTable.AddMethodParentIDsToList(rr[0]["MethodParentID"].ToString(), ref MethodIdList);
            }
        }

        public static void ResetMethod()
        {
            DiversityCollection.LookupTable._DtMethod = null;
            DiversityCollection.LookupTable._DtMethodHierarchy = null;
        }

        #endregion

        #region Method for Events

        private static System.Data.DataTable _DtMethodEvent;

        public static System.Data.DataTable DtMethodEvent
        {
            get
            {
                if (LookupTable._DtMethodEvent == null)
                {
                    string SQL = "SELECT MethodID, MethodParentID, DisplayText, Description, Notes, MethodURI " +
                        "FROM Method WHERE ForCollectionEvent = 1";
                    LookupTable._DtMethodEvent = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtMethodEvent);
                }
                return LookupTable._DtMethodEvent;
            }
        }

        public int MethodEventID(string DisplayText)
        {
            int MethodID = -1;
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtMethod.Select("DisplayText = '" + DisplayText.ToString() + "'");
            if (rr.Length > 0)
                int.TryParse(rr[0][0].ToString(), out MethodID);
            return MethodID;
        }


        #endregion

        #region Task

        private static System.Data.DataTable _DtTask;

        public static System.Data.DataTable DtTask
        {
            get
            {
                if (LookupTable._DtTask == null)
                {
                    string SQL = "SELECT TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle,  ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, DescriptionType, NotesType, UriType, ResponsibleType, MetricType, MetricUnit " +
                        "FROM Task";
                    LookupTable._DtTask = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtTask);
                }
                return LookupTable._DtTask;
            }
        }

        private static System.Data.DataTable _DtTaskHierarchy;

        public static System.Data.DataTable DtTaskHierarchy
        {
            get
            {
                if (LookupTable._DtTaskHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS TaskID, NULL AS TaskParentID, NULL AS DisplayText, NULL AS Description, " +
                        "NULL AS Notes, NULL AS TaskURI, NULL AS Type, NULL AS ModuleTitle, NULL AS ModuleType, NULL AS SpecimenPartType, NULL AS TransactionType, NULL AS ResultType, " +
                        "NULL AS DateType, NULL AS DateBeginType, NULL AS DateEndType, " +
                        "NULL AS NumberType, NULL AS BoolType, NULL AS DescriptionType, NULL AS NotesType, NULL AS UriType, NULL AS ResponsibleType, NULL AS MetricType, NULL AS HierarchyDisplayText " +
                        "UNION ALL " +
                        "SELECT TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, TransactionType, ResultType, DateType, DateBeginType, DateEndType, " +
                        "NumberType, BoolType, DescriptionType, NotesType, UriType, ResponsibleType, MetricType, HierarchyDisplayText " +
                        "FROM dbo.TaskHierarchyAll() " +
                        "ORDER BY HierarchyDisplayText";
                    LookupTable._DtTaskHierarchy = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtTaskHierarchy);
                    }
                    catch (System.Exception ex) { }
                }
                return LookupTable._DtTaskHierarchy;
            }
        }

        public static string TaskDisplayText(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["DisplayText"].ToString();
            return Title;
        }

        public static string TaskType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
            {
                Title = rr[0]["Type"].ToString();
            }
            return Title;
        }

        private static System.Data.DataTable _dtTaskType_Enum;
        
        public static System.Collections.Generic.List<string> TaskTypes(string Type, bool GetChildren = false)
        {
            if (_dtTaskType_Enum == null)
            {
                _dtTaskType_Enum = new System.Data.DataTable();
                string SQL = "SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, ParentCode FROM [dbo].[TaskType_Enum]";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _dtTaskType_Enum);
            }
            System.Collections.Generic.List<string> Types = new List<string>();
            string FilterColumn = "Code";
            if (GetChildren) FilterColumn = "ParentCode";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable._dtTaskType_Enum.Select(FilterColumn + " = '" + Type + "'");
            if (rr.Length > 0)
            {
                foreach (System.Data.DataRow r in rr)
                {
                    string T = r[0].ToString();
                    if (!Types.Contains(T))
                        Types.Add(T);
                    foreach (string t in TaskTypes(T, true))
                    {
                        if (!Types.Contains(t))
                            Types.Add(t);
                    }
                }
            }
            return Types;
        }

        private static System.Collections.Generic.List<string> _ModuleRelatedTaskTypes;
        public static bool TaskTypeIsModuleRelated(string Type)
        {
            if (_ModuleRelatedTaskTypes == null)
            {
                _ModuleRelatedTaskTypes = new List<string>();
                string SQL = "SELECT [Code] FROM [dbo].[TaskType_Enum] WHERE ParentCode = 'DiversityWorkbench' OR Code = 'DiversityWorkbench'";
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach(System.Data.DataRow R in dt.Rows)
                {
                    _ModuleRelatedTaskTypes.Add(R[0].ToString());
                }
            }
            return _ModuleRelatedTaskTypes.Contains(Type);
        }

        public static string TaskModuleTitle(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["ModuleTitle"].ToString();
            return Title;
        }
        public static string TaskModuleType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["ModuleType"].ToString();
            return Title;
        }

        public static string TaskSpecimenPartType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["SpecimenPartType"].ToString();
            return Title;
        }

        public static string TaskTransactionType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["TransactionType"].ToString();
            return Title;
        }


        public static string TaskResultType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["ResultType"].ToString();
            return Title;
        }

        public static string TaskDateType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["DateType"].ToString();
            return Title;
        }

        public static string TaskDateBeginType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["DateBeginType"].ToString();
            return Title;
        }

        public static string TaskDateEndType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["DateEndType"].ToString();
            return Title;
        }

        public static string TaskNumberType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["NumberType"].ToString();
            return Title;
        }

        public static string TaskBoolType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["BoolType"].ToString();
            return Title;
        }

        public static string TaskUriType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["UriType"].ToString();
            return Title;
        }

        public static string TaskDescriptionType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["DescriptionType"].ToString();
            return Title;
        }

        public static string TaskNotesType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["NotesType"].ToString();
            return Title;
        }

        public static string TaskResponsibleType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["ResponsibleType"].ToString();
            return Title;
        }

        public static string TaskMetricType(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["MetricType"].ToString();
            return Title;
        }

        public static string TaskMetricUnit(int TaskID)
        {
            string Title = "";
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID.ToString());
            if (rr.Length > 0)
                Title = rr[0]["MetricUnit"].ToString();
            return Title;
        }

        private static void AddTaskParentIDsToList(string TaskID, ref System.Collections.Generic.List<string> TaskIdList)
        {
            if (!TaskIdList.Contains(TaskID))
                TaskIdList.Add(TaskID);
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtTask.Select("TaskID = " + TaskID);
            if (rr.Length > 0)
            {
                if (!rr[0]["TaskParentID"].Equals(System.DBNull.Value))
                    DiversityCollection.LookupTable.AddTaskParentIDsToList(rr[0]["TaskParentID"].ToString(), ref TaskIdList);
            }
        }

        public static void ResetTask()
        {
            DiversityCollection.LookupTable._DtTask = null;
            DiversityCollection.LookupTable._DtTaskHierarchy = null;
        }

        public static System.Data.DataTable DtTaskResult(int TaskID)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            string SQL = "SELECT NULL AS Result UNION SELECT Result " +
                " FROM dbo.TaskResult WHERE TaskID = " + TaskID.ToString() +
                " ORDER BY Result";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dataTable);
                if (dataTable.Rows.Count == 1)
                    dataTable.Clear();
            }
            catch (System.Exception ex) { }
            return dataTable;
        }

        public static System.Data.DataTable DtTaskModule(int TaskID)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            string SQL = "SELECT NULL AS DisplayText UNION SELECT DisplayText " +
                " FROM dbo.TaskModule WHERE TaskID = " + TaskID.ToString() +
                " ORDER BY DisplayText";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dataTable);
                if (dataTable.Rows.Count == 1)
                    dataTable.Clear();
            }
            catch (System.Exception ex) { }
            return dataTable;
        }


        #endregion

        #region CollectionTask

        private static System.Data.DataTable _DtCollectionTask;

        public static System.Data.DataTable DtCollectionTask
        {
            get
            {
                if (LookupTable._DtCollectionTask == null)
                {
                    string SQL = "SELECT CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, TaskStart, TaskEnd, Value, URL, Description, Notes " +
                        "FROM CollectionTask";
                    LookupTable._DtCollectionTask = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtCollectionTask);
                }
                return LookupTable._DtCollectionTask;
            }
        }

        private static System.Data.DataTable _DtCollectionTaskHierarchy;

        public static System.Data.DataTable DtCollectionTaskHierarchy
        {
            get
            {
                if (LookupTable._DtCollectionTaskHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS CollectionTaskID, NULL AS CollectionTaskParentID, NULL AS CollectionID, NULL AS TaskID, NULL AS TaskStart, NULL AS TaskEnd, NULL AS Value, NULL AS URL, NULL AS Description, NULL AS Notes, " +
                        "NULL AS HierarchyDisplayText " +
                        "UNION ALL " +
                        "SELECT CollectionTaskID, CollectionTaskParentID, CollectionID, TaskID, TaskStart, TaskEnd, Value, URL, Description, Notes, HierarchyDisplayText " +
                        "FROM dbo.CollectionTaskHierarchyAll() " +
                        "ORDER BY HierarchyDisplayText";
                    LookupTable._DtCollectionTaskHierarchy = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtCollectionTaskHierarchy);
                    }
                    catch (System.Exception ex) { }
                }
                return LookupTable._DtCollectionTaskHierarchy;
            }
        }

        private static void AddCollectionTaskParentIDsToList(string TaskID, ref System.Collections.Generic.List<string> TaskIdList)
        {
            if (!TaskIdList.Contains(TaskID))
                TaskIdList.Add(TaskID);
            System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtCollectionTask.Select("CollectionTaskID = " + TaskID);
            if (rr.Length > 0)
            {
                if (!rr[0]["CollectionTaskParentID"].Equals(System.DBNull.Value))
                    DiversityCollection.LookupTable.AddTaskParentIDsToList(rr[0]["CollectionTaskParentID"].ToString(), ref TaskIdList);
            }
        }

        public static void ResetCollectionTask()
        {
            DiversityCollection.LookupTable._DtCollectionTask = null;
            DiversityCollection.LookupTable._DtCollectionTaskHierarchy = null;
        }

        #endregion

        #region Localisation

        private static System.Data.DataTable _DtLocalisationSystem;

        public static System.Data.DataTable DtLocalisationSystem
        {
            get
            {
                if (LookupTable._DtLocalisationSystem == null)
                {
                    string SQL = "SELECT LocalisationSystemID, LocalisationSystemParentID, LocalisationSystemName, DefaultMeasurementUnit, " +
                        "DefaultAccuracyOfLocalisation, ParsingMethodName, DisplayText, DisplayEnable, DisplayOrder, " +
                        "Description, DisplayTextLocation1, DescriptionLocation1, DisplayTextLocation2, " +
                        "DescriptionLocation2 " +
                        "FROM LocalisationSystem WHERE (DisplayEnable = 1) ORDER BY LocalisationSystemName";
                    LookupTable._DtLocalisationSystem = new System.Data.DataTable();
                    // using functions in DWB to ensure errorlog
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref LookupTable._DtLocalisationSystem);
                    //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    //ad.Fill(LookupTable._DtLocalisationSystem);
                    foreach (System.Data.DataRow R in LookupTable._DtLocalisationSystem.Rows)
                    {
                        System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                        Entity = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID." + R[0].ToString());
                        if (Entity["DisplayTextOK"] == "True")
                            R["DisplayText"] = Entity["DisplayText"];
                        if (Entity["DescriptionOK"] == "True")
                            R["Description"] = Entity["Description"];
                    }
                }
                return LookupTable._DtLocalisationSystem;
            }
            //set { LookupTable._Localisation = value; }
        }

        /// <summary>
        /// returns a new datatable, e.g. for datasource of comboboxes
        /// </summary>
        public static System.Data.DataTable DtLocalisationSystemNew
        {
            get
            {
                System.Data.DataTable dt = LookupTable.DtLocalisationSystem.Copy();
                return dt;
            }
        }
        
        public static string ParsingMethodName(int LocalisationSystemID)
        {
            string Method = "";
            foreach (System.Data.DataRow R in LookupTable.DtLocalisationSystem.Rows)
            {
                if (LocalisationSystemID.ToString() == R["LocalisationSystemID"].ToString())
                {
                    Method = R["ParsingMethodName"].ToString();
                    break;
                }
            }
            return Method;
        }

        public static bool LocalisationVisible(int LocalisationSystemID)
        {
            bool OK = true;
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Length > 0)
            for (int i = 0; i < LookupTable.DtLocalisationSystem.Rows.Count; i++)
            {
                if (LookupTable.DtLocalisationSystem.Rows[i]["LocalisationSystemID"].ToString() == LocalisationSystemID.ToString())
                {
                    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Length > i &&
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Substring(i, 1) == "0")
                        return false;
                    else return true;
                }
            }
            return OK;
        }

        public static string LocalisationMeasurementUnit(int LocalisationSystemID)
        {
            string MeasurementUnit = "";
            foreach (System.Data.DataRow R in LookupTable.DtLocalisationSystem.Rows)
            {
                if (LocalisationSystemID.ToString() == R["LocalisationSystemID"].ToString())
                {
                    if (R["DefaultMeasurementUnit"].ToString().Length > 0 && !R["DefaultMeasurementUnit"].Equals(System.DBNull.Value))
                        MeasurementUnit = R["DefaultMeasurementUnit"].ToString();
                    else
                        MeasurementUnit = "";
                    break;
                }
            }
            return MeasurementUnit;
        }

        public static string LocalisationSystemName(int LocalisationSystemID)
        {
            string Name = "";
            System.Data.DataRow[] RR = LookupTable.DtLocalisationSystem.Select("LocalisationSystemID = " + LocalisationSystemID.ToString());
            if (RR.Length == 1)
            {

            }
            foreach (System.Data.DataRow R in LookupTable.DtLocalisationSystem.Rows)
            {
                if (LocalisationSystemID.ToString() == R["LocalisationSystemID"].ToString())
                {
                    Name = R["LocalisationSystemName"].ToString();
                    break;
                }
            }
            return Name;
        }

        public static bool LocalisationHasCoordinates(int LocalisationSystemID)
        {
            bool OK = true;
            System.Data.DataRow[] RR = LookupTable.DtLocalisationSystem.Select("LocalisationSystemID = " + LocalisationSystemID.ToString(), "");
            if (RR.Length > 0)
            {
                if (RR[0]["ParsingMethodName"].ToString() == "Coordinates"
                    || RR[0]["ParsingMethodName"].ToString() == "MTB"
                    || RR[0]["ParsingMethodName"].ToString() == "Gazetteer"
                    || RR[0]["ParsingMethodName"].ToString() == "SamplingPlot")
                    return true;
                else
                    return false;
            }
            //else if (LocalisationSystemID == 1) return true;
            else return false;
        }

        public static string LocalisationSystemParingMethod(int LocalisationSystemID)
        {
            return LookupTable.ParsingMethodName(LocalisationSystemID);
        }

        // returns a table containing the Display texts for the Location columns for a LocalisationSystem
        public static System.Data.DataTable DtLocationColumns(int LocalisationSystemID)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataColumn c1 = new System.Data.DataColumn("ColumnName", System.Type.GetType("System.String"));
            System.Data.DataColumn c2 = new System.Data.DataColumn("DisplayText", System.Type.GetType("System.String"));
            dt.Columns.Add(c1);
            dt.Columns.Add(c2);
            System.Data.DataRow[] rr = LookupTable.DtLocalisationSystem.Select("LocalisationSystemID = " + LocalisationSystemID);
            if (rr.Length > 0)
            {
                System.Data.DataRow r1 = dt.NewRow();
                r1[0] = "Location1";
                r1[1] = rr[0]["DisplayTextLocation1"].ToString();
                dt.Rows.Add(r1);
                System.Data.DataRow r2 = dt.NewRow();
                r2[0] = "Location2";
                r2[1] = rr[0]["DisplayTextLocation2"].ToString();
                dt.Rows.Add(r2);
            }
            return dt;
        }

        // returns a table containing the Display texts for the Location columns for the LocalisationSystems for named areas
        public static System.Data.DataTable DtLocationNameAeras()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                System.Data.DataColumn c1 = new System.Data.DataColumn("LocalisationSystemID", System.Type.GetType("System.Int32"));
                System.Data.DataColumn c2 = new System.Data.DataColumn("DisplayText", System.Type.GetType("System.String"));
                dt.Columns.Add(c1);
                dt.Columns.Add(c2);
                foreach (System.Data.DataRow R in LookupTable.DtLocalisationSystem.Rows)
                {
                    if (R["LocalisationSystemID"].ToString() == "7"
                        || R["LocalisationSystemID"].ToString() == "18"
                        || R["LocalisationSystemID"].ToString() == "19"
                        || R["LocalisationSystemID"].ToString() == "20"
                        || R["LocalisationSystemID"].ToString() == "21")
                    {
                        System.Data.DataRow r1 = dt.NewRow();
                        r1[0] = int.Parse(R["LocalisationSystemID"].ToString());
                        r1[1] = R["DisplayText"].ToString() + " [ID = " + R["LocalisationSystemID"].ToString() + "]";
                        dt.Rows.Add(r1);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }

        public static System.Collections.Generic.Dictionary<string, string> LocalisationSystemInfo(string Code)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtLocalisationSystem.Select("LocalisationSystemID = " + Code);
            if (RR.Length > 0)
            {
                foreach (System.Data.DataColumn C in RR[0].Table.Columns)
                    Dict.Add(C.ColumnName, RR[0][C.ColumnName].ToString());
            }
            return Dict;
        }

        public static void ResetLocalisationSystem()
        {
            DiversityCollection.LookupTable._DtLocalisationSystem = null;
        }

        public enum CoordinateSystem { WGS84, PotsdamDatum, GaussKrüger, UTM };//, Merchich };
        private static System.Collections.Generic.Dictionary<CoordinateSystem, int> _CoordinateLocalisationSystemIDs;
        public static System.Collections.Generic.Dictionary<CoordinateSystem, int> CoordinateLocalisationSystemIDs
        {
            get
            {
                if (LookupTable._CoordinateLocalisationSystemIDs == null)
                {
                    LookupTable._CoordinateLocalisationSystemIDs = new Dictionary<CoordinateSystem, int>();
                    LookupTable._CoordinateLocalisationSystemIDs.Add(CoordinateSystem.WGS84, 8);
                    LookupTable._CoordinateLocalisationSystemIDs.Add(CoordinateSystem.GaussKrüger, 2);
                    LookupTable._CoordinateLocalisationSystemIDs.Add(CoordinateSystem.PotsdamDatum, 12);
                    LookupTable._CoordinateLocalisationSystemIDs.Add(CoordinateSystem.UTM, 22);
                }
                return LookupTable._CoordinateLocalisationSystemIDs;
            }
        }

        #endregion

        #region Event property

        private static System.Data.DataTable _DtProperty;

        public static System.Data.DataTable DtProperty
        {
            get
            {
                if (LookupTable._DtProperty == null)
                {
                    string SQL = "SELECT PropertyID, PropertyParentID, PropertyName, DefaultAccuracyOfProperty, " +
                        "DefaultMeasurementUnit, ParsingMethodName, DisplayText, DisplayEnabled, DisplayOrder, Description, PropertyURI " +
                        "FROM Property WHERE (DisplayEnabled = 1) ORDER BY PropertyName";
                    LookupTable._DtProperty = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtProperty);
                }
                return LookupTable._DtProperty;
            }
            //set { LookupTable._Property = value; }
        }
        
        public static string ParsingMethodNameProperty(int PropertyID)
        {
            string Method = "";
            foreach (System.Data.DataRow R in LookupTable.DtProperty.Rows)
            {
                if (PropertyID.ToString() == R["PropertyID"].ToString())
                {
                    Method = R["ParsingMethodName"].ToString();
                    break;
                }
            }
            return Method;
        }

        public static bool PropertyVisible(int PropertyID)
        {
            bool OK = true;
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Length > 0)
                for (int i = 0; i < LookupTable.DtProperty.Rows.Count; i++)
                {
                    if (DiversityCollection.LookupTable.DtProperty.Rows[i]["PropertyID"].ToString() == PropertyID.ToString())
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Length > i &&
                            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Substring(i, 1) == "0")
                            return false;
                        break;
                    }
                }
            return OK;
        }

        public static string PropertyName(int PropertyID)
        {
            string Name = "";
            foreach (System.Data.DataRow R in LookupTable.DtProperty.Rows)
            {
                if (PropertyID.ToString() == R["PropertyID"].ToString())
                {
                    Name = R["PropertyName"].ToString();
                    break;
                }
            }
            return Name;
        }

        public static string PropertyURI(int PropertyID)
        {
            string URI = "";
            foreach (System.Data.DataRow R in LookupTable.DtProperty.Rows)
            {
                if (PropertyID.ToString() == R["PropertyID"].ToString())
                {
                    URI = R["PropertyURI"].ToString();
                    break;
                }
            }
            return URI;
        }

        public static string PropertyParsingMethod(int PropertyID)
        {
            string Name = "";
            foreach (System.Data.DataRow R in LookupTable.DtProperty.Rows)
            {
                if (PropertyID.ToString() == R["PropertyID"].ToString())
                {
                    Name = R["ParsingMethodName"].ToString();
                    break;
                }
            }
            return Name;
        }

        public static System.Collections.Generic.Dictionary<string, string> PropertyInfo(string Code)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtProperty.Select("PropertyID = " + Code);
            if (RR.Length > 0)
            {
                foreach (System.Data.DataColumn C in RR[0].Table.Columns)
                    Dict.Add(C.ColumnName, RR[0][C.ColumnName].ToString());
            }
            return Dict;
        }

        public static void ResetEventPropertyList()
        {
            DiversityCollection.LookupTable._DtProperty = null;
        }

        #endregion

        #region ScientificTermSource

        private static System.Data.DataTable _DtScientificTermSource;

        public static System.Data.DataTable DtScientificTermSource
        {
            get
            {
                if (LookupTable._DtScientificTermSource == null)
                {
                    string SQL = "SELECT CAST('' AS nvarchar(500)) AS URI, CAST(0 AS int) AS ID,  " +
                        "CAST('' AS nvarchar(500)) AS Source";
                    LookupTable._DtProperty = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtScientificTermSource);
                    _DtScientificTermSource.Clear();
                    foreach(System.Data.DataRow R in DtProperty.Rows)
                    {

                    }
                }
                return LookupTable._DtScientificTermSource;
            }
        }

        public static string ScientificTermURI(string Source, int ID)
        {
            string URI = "";
            foreach (System.Data.DataRow R in LookupTable.DtScientificTermSource.Rows)
            {
                if (Source == R["Source"].ToString() && ID.ToString() == R["ID"].ToString())
                {
                    URI = R["URI"].ToString();
                    break;
                }
            }
            if (URI.Length == 0)
            {

            }
            return URI;
        }

        #endregion

        #region Country

        private static System.Data.DataTable _DtCountryGazetteer;
        private static System.Data.DataTable _DtCountryAll;

        public static System.Data.DataTable DtCountryGazetteer()
        {
            if (LookupTable._DtCountryGazetteer == null)
            {
                LookupTable._DtCountryGazetteer = DiversityWorkbench.Gazetteer.Countries();
            }
            return LookupTable._DtCountryGazetteer;
        }


        public static System.Data.DataTable DtCountryAll()
        {
            if (LookupTable._DtCountryAll == null)
            {
                try
                {
                    LookupTable._DtCountryAll = LookupTable.DtCountryGazetteer().Copy();
                    foreach (System.Data.DataRow R in LookupTable.DtCountryLocal.Rows)
                    {
                        System.Data.DataRow[] rr = LookupTable._DtCountryAll.Select(LookupTable._DtCountryAll.Columns[0].ColumnName + " = '" + R[0].ToString().Replace("'", "''") + "'");
                        if (rr.Length == 0)
                        {
                            System.Data.DataRow Rnew = LookupTable._DtCountryAll.NewRow();
                            Rnew[0] = R[0];
                            Rnew[1] = R[0];
                            LookupTable._DtCountryAll.Rows.Add(Rnew);
                        }
                        else
                        {
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
            return LookupTable._DtCountryAll;
        }

        public static void ResetCountryFromGazetteer()
        {
            LookupTable._DtCountryGazetteer = null;
            LookupTable._DtCountryAll = null;
        }

        public static void ResetCountry()
        {
            LookupTable._DtCountryAll = null;
            LookupTable._DtCountryGazetteer = null;
            LookupTable._DtCountryLocal = null;
        }

        
        private static System.Data.DataTable _DtCountryLocal;
        public static System.Data.DataTable DtCountryLocal
        {
            get
            {
                if (DiversityCollection.LookupTable._DtCountryLocal == null)
                {
                    LookupTable._DtCountryLocal = new System.Data.DataTable();
                    string SQL = "SELECT '' AS Country UNION SELECT DISTINCT CountryCache AS Country " +
                        "FROM         CollectionEvent " +
                        "WHERE     (CountryCache <> N'') " +
                        "ORDER BY Country";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtCountryLocal);
                }
                return LookupTable._DtCountryLocal;
            }
        }
        

        #endregion

        #region Collection

        private static System.Data.DataTable _DtCollection;

        public static System.Data.DataTable DtCollection
        {
            get
            {
                if (LookupTable._DtCollection == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS CollectionID, NULL AS CollectionParentID, NULL AS CollectionName, NULL AS CollectionAcronym, " +
                        "NULL AS AdministrativeContactName, NULL AS AdministrativeContactAgentURI, NULL AS Description, " +
                        "NULL AS Location, NULL AS LocationPlan, NULL AS LocationPlanWidth, NULL AS LocationPlanDate, NULL AS LocationHeight, NULL AS CollectionOwner, NULL AS DisplayOrder, NULL AS Type, NULL AS LocationParentID " + 
                        "UNION " +
                        "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
                        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                        "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, CollectionOwner, DisplayOrder, Type, LocationParentID " + 
                        "FROM dbo.Collection WHERE (Type IS NULL OR Type <> 'regulation') " +
                        "ORDER BY CollectionName";
//#if CollectionLocactionIDAvailable
//                    SQL = "SELECT NULL AS CollectionID, NULL AS CollectionParentID, NULL AS CollectionName, NULL AS CollectionAcronym, " +
//                        "NULL AS AdministrativeContactName, NULL AS AdministrativeContactAgentURI, NULL AS Description, " +
//                        "NULL AS Location, NULL AS LocationPlan, NULL AS LocationPlanWidth, NULL AS LocationHeight, NULL AS CollectionOwner, NULL AS DisplayOrder, NULL AS CollectionLocationID " +
//                        "UNION " +
//                        "SELECT CollectionID, CollectionParentID, CollectionName, CollectionAcronym, " +
//                        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
//                        "Location, LocationPlan, LocationPlanWidth, LocationHeight, CollectionOwner, DisplayOrder, CollectionLocationID" +  
//                        "FROM dbo.Collection WHERE (Type IS NULL OR Type <> 'regulation') " +
//                        "ORDER BY CollectionName";
//#endif
                    LookupTable._DtCollection = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtCollection);
                }
                return LookupTable._DtCollection;
            }
            //set { LookupTable._Property = value; }
        }

        private static System.Data.DataTable _DtCollectionWithHierarchy;

        public static System.Data.DataTable DtCollectionWithHierarchy
        {
            get
            {
                if (LookupTable._DtCollectionWithHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS CollectionName, NULL AS CollectionID, NULL AS CollectionParentID, NULL AS CollectionAcronym, " +
                        "NULL AS AdministrativeContactName, NULL AS AdministrativeContactAgentURI, NULL AS Description, " +
                        "NULL AS Location, NULL AS LocationPlan, NULL AS LocationPlanWidth, NULL AS LocationPlanDate, NULL AS LocationHeight, " +
                        "NULL AS CollectionOwner, NULL AS DisplayOrder, NULL AS Type, NULL AS LocationParentID, NULL AS DisplayText " +
                        "UNION " +
                        "SELECT CollectionName, CollectionID, CollectionParentID, CollectionAcronym, " +
                        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                        "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, " +
                        "CollectionOwner, DisplayOrder, Type, LocationParentID, DisplayText " +
                        "FROM CollectionHierarchyAll() WHERE (Type IS NULL OR Type <> 'regulation') " +
                        "ORDER BY DisplayText";
                    LookupTable._DtCollectionWithHierarchy = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtCollectionWithHierarchy);
                }
                return LookupTable._DtCollectionWithHierarchy;
            }
            //set { LookupTable._Property = value; }
        }

        private static System.Data.DataTable _DtCollectionLocationWithHierarchy;

        public static System.Data.DataTable DtCollectionLocationWithHierarchy
        {
            get
            {
                if (LookupTable._DtCollectionLocationWithHierarchy == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS CollectionName, NULL AS CollectionID, NULL AS CollectionParentID, NULL AS CollectionAcronym, " +
                        "NULL AS AdministrativeContactName, NULL AS AdministrativeContactAgentURI, NULL AS Description, " +
                        "NULL AS Location, NULL AS LocationPlan, NULL AS LocationPlanWidth, NULL AS LocationPlanDate, NULL AS LocationHeight, " +
                        "NULL AS CollectionOwner, NULL AS DisplayOrder, NULL AS Type, NULL AS LocationParentID, NULL AS DisplayText " +
                        "UNION " +
                        "SELECT CollectionName, CollectionID, CollectionParentID, CollectionAcronym, " +
                        "AdministrativeContactName, AdministrativeContactAgentURI, Description, " +
                        "Location, LocationPlan, LocationPlanWidth, LocationPlanDate, LocationHeight, " +
                        "CollectionOwner, DisplayOrder, Type, LocationParentID, DisplayText " +
                        "FROM CollectionLocationAll() WHERE (Type IS NULL OR Type <> 'regulation') " +
                        "ORDER BY DisplayText";
                    LookupTable._DtCollectionLocationWithHierarchy = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtCollectionLocationWithHierarchy);
                    }
                    catch(System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return LookupTable._DtCollectionLocationWithHierarchy;
            }
            //set { LookupTable._Property = value; }
        }

        public static void DtCollectionLocationWithHierarchyReset() { _DtCollectionLocationWithHierarchy = null; }

        public static string CollectionName(int CollectionID)
        {
            string Name = "";
            try
            {
                foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtCollection.Rows)
                {
                    if (R["CollectionID"].ToString() == CollectionID.ToString())
                    {
                        Name = R["CollectionName"].ToString();
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Name;
        }

        public static string CollectionType(int CollectionID)
        {
            string Type = "";
            try
            {
                foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtCollection.Rows)
                {
                    if (R["CollectionID"].ToString() == CollectionID.ToString())
                    {
                        Type = R["Type"].ToString();
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Type;
        }

        public static string CollectionNameHierarchy(int CollectionID)
        {
            string Collection = "";
            try
            {
                if (DiversityCollection.LookupTable.DtCollectionWithHierarchy != null)
                {
                    System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtCollectionWithHierarchy.Select("CollectionID = " + CollectionID.ToString());
                    Collection = rr[0]["DisplayText"].ToString();
                }
                else if (DiversityCollection.LookupTable.DtCollection != null)
                {
                    System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID.ToString());
                    if (rr.Length > 0)
                    {
                        Collection = rr[0]["CollectionName"].ToString();
                        int ParentID = -1;
                        int.TryParse(rr[0]["CollectionParentID"].ToString(), out ParentID);
                        while (ParentID > -1)
                        {
                            System.Data.DataRow[] rrP = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + ParentID.ToString());
                            if (rrP.Length > 0)
                                Collection = rrP[0]["CollectionName"].ToString() + " - " + Collection;
                            else
                                ParentID = -1;
                            if (!int.TryParse(rrP[0]["CollectionParentID"].ToString(), out ParentID))
                                ParentID = -1;
                        }
                    }
                }
            }
            catch { }
            return Collection;
        }

        public static string CollectionOwner(int CollectionID)
        {
            string Owner = "";
            int? ParentID = CollectionID;
            if (DiversityCollection.LookupTable.DtCollection != null)
            {
                try
                {
                    System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID.ToString());
                    if (rr.Length == 0) return "";
                    while (ParentID != null)
                    {
                        try
                        {
                            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtCollection.Rows)
                            {
                                if (!R["CollectionOwner"].Equals(System.DBNull.Value) && R["CollectionID"].ToString() == ParentID.ToString())
                                {
                                    if (R["CollectionOwner"].ToString().Length > 0)
                                    {
                                        Owner = R["CollectionOwner"].ToString();
                                        ParentID = null;
                                        break;
                                    }
                                }
                                if (R["CollectionID"].ToString() == ParentID.ToString())
                                {
                                    Owner = R["CollectionOwner"].ToString();
                                    if (R["CollectionParentID"].Equals(System.DBNull.Value))
                                        ParentID = null;
                                    else if (ParentID == int.Parse(R["CollectionParentID"].ToString()))
                                        ParentID = null;
                                    else
                                        ParentID = int.Parse(R["CollectionParentID"].ToString());
                                    break;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID);
                    foreach (System.Data.DataRow R in RR)
                    {
                        if (R["CollectionOwner"].Equals(System.DBNull.Value) || R["CollectionOwner"].ToString().Length == 0)
                            R["CollectionOwner"] = Owner;
                    }
                }
                catch { }
            }
            return Owner;
        }

        public static string CollectionOwnerAcronym(int CollectionID)
        {
            string Acronym = "";
            int? ParentID = CollectionID;
            if (DiversityCollection.LookupTable.DtCollection != null)
            {
                try
                {
                    System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID.ToString());
                    if (rr.Length == 0) return "";
                    while (ParentID != null)
                    {
                        try
                        {
                            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtCollection.Rows)
                            {
                                if (!R["CollectionOwner"].Equals(System.DBNull.Value) && R["CollectionID"].ToString() == ParentID.ToString())
                                {
                                    if (R["CollectionAcronym"].ToString().Length > 0)
                                    {
                                        Acronym = R["CollectionAcronym"].ToString();
                                        ParentID = null;
                                        break;
                                    }
                                }
                                if (R["CollectionID"].ToString() == ParentID.ToString())
                                {
                                    Acronym = R["CollectionAcronym"].ToString();
                                    if (R["CollectionParentID"].Equals(System.DBNull.Value))
                                        ParentID = null;
                                    else if (ParentID == int.Parse(R["CollectionParentID"].ToString()))
                                        ParentID = null;
                                    else
                                        ParentID = int.Parse(R["CollectionParentID"].ToString());
                                    break;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    //System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID);
                    //foreach (System.Data.DataRow R in RR)
                    //{
                    //    if (R["CollectionOwner"].Equals(System.DBNull.Value) || R["CollectionOwner"].ToString().Length == 0)
                    //        R["CollectionAcronym"] = Acronym;
                    //}
                }
                catch { }
            }
            return Acronym;
        }

        public static string CollectionOwnerAgentURI(int CollectionID)
        {
            string URI = "";
            string Owner = "";
            int? ParentID = CollectionID;
            if (DiversityCollection.LookupTable.DtCollection != null)
            {
                try
                {
                    System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID.ToString());
                    if (rr.Length == 0) return "";
                    while (ParentID != null)
                    {
                        try
                        {
                            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtCollection.Rows)
                            {
                                if (!R["CollectionOwner"].Equals(System.DBNull.Value) 
                                    && R["CollectionID"].ToString() == ParentID.ToString()
                                    && R["CollectionOwner"].ToString().Length > 0)
                                {
                                    if (R["CollectionOwner"].ToString().Length > 0)
                                    {
                                        URI = R["AdministrativeContactAgentURI"].ToString();
                                        ParentID = null;
                                        return URI;
                                    }
                                }
                                if (R["CollectionID"].ToString() == ParentID.ToString())
                                {
                                    if (R["CollectionParentID"].Equals(System.DBNull.Value))
                                        ParentID = null;
                                    else if (ParentID == int.Parse(R["CollectionParentID"].ToString()))
                                        ParentID = null;
                                    else
                                        ParentID = int.Parse(R["CollectionParentID"].ToString());
                                    break;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
                catch { }
            }
            return URI;
        }

        public static string CollectionAdministrativeContactAgentURI(int CollectionID)
        {
            string URI = "";
            int? ParentID = CollectionID;
            if (DiversityCollection.LookupTable.DtCollection != null)
            {
                try
                {
                    System.Data.DataRow[] rr = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID.ToString());
                    if (rr.Length == 0) return "";
                    while (ParentID != null)
                    {
                        try
                        {
                            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtCollection.Rows)
                            {
                                if (!R["AdministrativeContactAgentURI"].Equals(System.DBNull.Value) && R["CollectionID"].ToString() == ParentID.ToString())
                                {
                                    if (R["AdministrativeContactAgentURI"].ToString().Length > 0)
                                    {
                                        URI = R["AdministrativeContactAgentURI"].ToString();
                                        ParentID = null;
                                        break;
                                    }
                                }
                                if (R["CollectionID"].ToString() == ParentID.ToString())
                                {
                                    URI = R["AdministrativeContactAgentURI"].ToString();
                                    if (R["CollectionParentID"].Equals(System.DBNull.Value))
                                        ParentID = null;
                                    else if (ParentID == int.Parse(R["CollectionParentID"].ToString()))
                                        ParentID = null;
                                    else
                                        ParentID = int.Parse(R["CollectionParentID"].ToString());
                                    break;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtCollection.Select("CollectionID = " + CollectionID);
                    foreach (System.Data.DataRow R in RR)
                    {
                        if (R["AdministrativeContactAgentURI"].Equals(System.DBNull.Value) || R["AdministrativeContactAgentURI"].ToString().Length == 0)
                            R["AdministrativeContactAgentURI"] = URI;
                    }
                }
                catch { }
            }
            return URI;
        }

        public static bool IsChildOfCollection(int CollectionIDChild, int CollectionIDParent)
        {
            bool OK = false;
            if (CollectionIDChild == CollectionIDParent) return true;
            System.Data.DataRow[] RR = DtCollection.Select("CollectionID = " + CollectionIDChild.ToString());
            if (RR.Length > 0)
            {
                System.Data.DataRow R = RR[0];
                if (R["CollectionParentID"].Equals(System.DBNull.Value)) return false;
                else
                {
                    int? ParentID = CollectionIDChild;
                    int Counter = 0;
                    while (ParentID != null && Counter < DtCollection.Rows.Count)
                    {
                        System.Data.DataRow[] rr = DtCollection.Select("CollectionID = " + ParentID.ToString());
                        if (rr.Length > 0)
                        {
                            System.Data.DataRow r = rr[0];
                            if (r["CollectionParentID"].Equals(System.DBNull.Value)) return false;
                            else
                            {
                                ParentID = int.Parse(r["CollectionParentID"].ToString());
                                if (ParentID == CollectionIDParent) return true;
                            }
                        }
                        else
                            return false;
                        Counter++;
                    }
                }
            }
            return OK;
        }

        public static void ResetCollection()
        {
            DiversityCollection.LookupTable._DtCollection = null;
            DiversityCollection.LookupTable._DtCollectionWithHierarchy = null;
            DiversityCollection.LookupTable._DtCollectionLocationWithHierarchy = null;
        }

        #endregion

        #region Managed collections

        private static System.Data.DataTable _DtManagedCollections;

        public static System.Data.DataTable DtManagedCollections
        {
            get
            {
                try
                {
                    if (LookupTable._DtManagedCollections == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        string SQL = "SELECT * FROM dbo.ManagerCollectionList() " +
                            "ORDER BY CollectionName";
                        LookupTable._DtManagedCollections = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(LookupTable._DtManagedCollections);
                    }
                }
                catch { }
                return LookupTable._DtManagedCollections;
            }
        }

        public static bool IsManagedCollection(int CollectionID)
        {
            try
            {
                bool OK = false;
                System.Data.DataRow[] RR = DtManagedCollections.Select("CollectionID = " + CollectionID.ToString());
                if (RR.Length > 0)
                    return true;
                return OK;
            }
            catch { return false; }
        }

        public static void ResetManagedCollections()
        {
            DiversityCollection.LookupTable._DtManagedCollections = null;
        }

        #endregion

        #region Identification unit
        private static System.Data.DataTable _DtIdentificationUnit;

        public static System.Data.DataTable DtIdentificationUnit
        {
            get { return LookupTable._DtIdentificationUnit; }
            set { LookupTable._DtIdentificationUnit = value; }
        }
        
        #endregion

        #region Project

        public static void ResetProjectList()
        {
            DiversityCollection.LookupTable._DtProject = null;
            DiversityCollection.LookupTable._DtProjectList = null;
        }

        private static System.Data.DataTable _DtProject;

        public static System.Data.DataTable DtProject
        {
            get
            {
                if (LookupTable._DtProject == null)
                {
                    string SQL = "SELECT ProjectProxy.ProjectID, CAST(NULL as int) AS ProjectParentID, ProjectProxy.Project, ProjectProxy.Project AS ProjectTitle, " +
                        "CAST(NULL as nvarchar(2000)) AS ProjectDescription, CAST(NULL AS nvarchar(255)) AS ProjectEditors,  " +
                        "CAST(NULL AS nvarchar(255)) AS ProjectNotes, CAST(NULL AS nvarchar(255)) AS ProjectCopyright, CAST(NULL AS nvarchar(255)) AS ProjectVersion, ProjectURI " +
                        "FROM ProjectProxy INNER JOIN " +
                        "ProjectUser ON ProjectProxy.ProjectID = ProjectUser.ProjectID " +
                        "WHERE (ProjectUser.LoginName = USER_NAME()) " +
                        "ORDER BY ProjectProxy.Project";
                    LookupTable._DtProject = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtProject);
                    try 
                    { 
                        foreach (System.Data.DataRow R in LookupTable._DtProject.Rows)
                        {
                            if (!R["ProjectURI"].Equals(System.DBNull.Value) && R["ProjectURI"].ToString().Length > 0)
                            {
                                string URI = R["ProjectURI"].ToString();
                                string ProjectDatabase = URI.Substring(URI.IndexOf("/Projects"));
                                ProjectDatabase = ProjectDatabase.Substring(1);
                                ProjectDatabase = "Diversity" + ProjectDatabase.Substring(0, ProjectDatabase.IndexOf("/"));
                                SQL = "SELECT P.ProjectID, P.ProjectParentID, P.Project, " +
                                    "P.ProjectTitle, P.ProjectDescription, P.ProjectEditors,  " +
                                    "P.ProjectNotes, P.ProjectCopyright, P.ProjectVersion " +
                                    "FROM " + ProjectDatabase + ".dbo.Project P " +
                                    "WHERE P.ProjectID = " + R["ProjectID"].ToString();
                                System.Data.DataTable dtProject = new System.Data.DataTable();
                                ad.SelectCommand.CommandText = SQL;
                                ad.Fill(dtProject);
                                if (dtProject.Rows.Count == 1)
                                {
                                    R["ProjectParentID"] = dtProject.Rows[0]["ProjectParentID"];
                                    R["ProjectTitle"] = dtProject.Rows[0]["ProjectTitle"];
                                    R["ProjectDescription"] = dtProject.Rows[0]["ProjectDescription"];
                                    R["ProjectEditors"] = dtProject.Rows[0]["ProjectEditors"];
                                    R["ProjectNotes"] = dtProject.Rows[0]["ProjectNotes"];
                                    R["ProjectCopyright"] = dtProject.Rows[0]["ProjectCopyright"];
                                    R["ProjectVersion"] = dtProject.Rows[0]["ProjectVersion"];
                                }
                            }
                        }
                    }
                    catch (System.Exception ex) { }
                }
                return LookupTable._DtProject;
            }
        }

        public static string ProjectName(int ProjectID)
        {
            string Name = "";
            try
            {
                foreach (System.Data.DataRow R in LookupTable.DtProjectList.Rows)
                {
                    if (ProjectID.ToString() == R["ProjectID"].ToString())
                    {
                        Name = R["Project"].ToString();
                        break;
                    }
                }
            }
            catch { }
            return Name;
        }

        public static string ProjectTitle(int ProjectID)
        {
            string P = "";
            foreach (System.Data.DataRow R in LookupTable.DtProject.Rows)
            {
                if (R["ProjectID"].ToString() == ProjectID.ToString())
                {
                    if (!R["ProjectTitle"].Equals(System.DBNull.Value) && R["ProjectTitle"].ToString().Length > 0)
                        P = R["ProjectTitle"].ToString();
                    else
                        P = R["Project"].ToString();
                    break;
                }
            }
            return P;
        }

        public static string ProjectParentTitle(int ProjectID)
        {
            string P = "";
            foreach (System.Data.DataRow R in LookupTable.DtProject.Rows)
            {
                if (R["ProjectID"].ToString() == ProjectID.ToString())
                {
                    if (R["ProjectParentID"].Equals(System.DBNull.Value))
                    {
                        if (!R["ProjectTitle"].Equals(System.DBNull.Value) && R["ProjectTitle"].ToString().Length > 0)
                            P = R["ProjectTitle"].ToString();
                        else
                            P = R["Project"].ToString();
                        break;
                    }
                    else
                    {
                        P = LookupTable.ProjectTitle(int.Parse(R["ProjectID"].ToString()));
                    }
                }
            }
            return P;
        }

        private static System.Data.DataTable _DtProjectList;

        public static System.Data.DataTable DtProjectList
        {
            get
            {
                if (DiversityCollection.LookupTable._DtProjectList == null)
                {
                    DiversityCollection.LookupTable._DtProjectList = new System.Data.DataTable();
                    string SQL = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtProjectList);
                    }
                    catch { }
                }
                return DiversityCollection.LookupTable._DtProjectList;
            }
        }

        private static System.Data.DataTable _DtProjectNoAccessList;

        public static System.Data.DataTable DtProjectNoAccessList
        {
            get 
            {
                if (LookupTable._DtProjectNoAccessList == null)
                {
                    DiversityCollection.LookupTable._DtProjectNoAccessList = new System.Data.DataTable();
                    string SQL = "SELECT Project, ProjectID FROM ProjectProxy P WHERE P.ProjectID NOT IN (SELECT L.ProjectID FROM ProjectList L) ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtProjectNoAccessList);
                    }
                    catch { }
                }
                return LookupTable._DtProjectNoAccessList; 
            }
            //set { LookupTable._DtProjectNoAccessList = value; }
        }

        #endregion

        #region User

        private static System.Data.DataTable _DtUser;

        public static System.Data.DataTable DtUser
        {
            get
            {
                if (LookupTable._DtUser == null)
                {
                    string SQL = "SELECT LoginName, CombinedNameCache, UserURI " +
                        "FROM UserProxy ORDER BY CombinedNameCache";
                    LookupTable._DtUser = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtUser);
                }
                return LookupTable._DtUser;
            }
        }

        private static string _CurrentUser;

        public static string CurrentUser
        {
            get
            {
                if (LookupTable._CurrentUser == null)
                {
                    string SQL = "SELECT USER_NAME()";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand COM = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    string User = COM.ExecuteScalar().ToString();
                    con.Close();
                    if (User == "dbo") 
                        LookupTable._CurrentUser = "";
                    else
                    {
                        foreach (System.Data.DataRow R in LookupTable.DtUser.Rows)
                        {
                            if (User == R["LoginName"].ToString())
                            {
                                LookupTable._CurrentUser = R["CombinedNameCache"].ToString();
                                break;
                            }
                        }
                    }
                }
                return LookupTable._CurrentUser;
            }
        }

        public static string CombinedNameCache(string LoginName)
        {
            string Name = "";
            foreach (System.Data.DataRow R in LookupTable.DtUser.Rows)
            {
                if (LoginName.ToString() == R["LoginName"].ToString())
                {
                    Name = R["CombinedNameCache"].ToString();
                    break;
                }
            }
            return Name;
        }

        public static void ResetUser()
        {
            LookupTable._DtUser = null;
        }
        
        #endregion

        #region Collection Type
        
        private static System.Data.DataTable _DtCollectionTypes;

        public static System.Data.DataTable DtCollectionTypes
        {
            get
            {
                if (LookupTable._DtCollectionTypes == null)
                {
                    string SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollCollectionType_Enum' and C.COLUMN_NAME = 'Icon'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1")
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, Icon FROM CollCollectionType_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    else
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, NULL AS Icon FROM CollCollectionType_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    LookupTable._DtCollectionTypes = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtCollectionTypes);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                return LookupTable._DtCollectionTypes;
            }
            //set { LookupTable._DtAnalysis = value; }
        }

        #endregion

        #region Task Type

        private static System.Data.DataTable _DtTaskTypes;

        public static System.Data.DataTable DtTaskTypes
        {
            get
            {
                if (LookupTable._DtTaskTypes == null)
                {
                    string SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'TaskType_Enum' and C.COLUMN_NAME = 'Icon'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1")
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, Icon FROM TaskType_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    else
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, NULL AS Icon FROM TaskType_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    LookupTable._DtTaskTypes = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtTaskTypes);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                return LookupTable._DtTaskTypes;
            }
        }

        public static void ResetTaskTypes()
        {
            LookupTable._DtTaskTypes = null;
        }

        #endregion

        #region Taxonomic groups

        private static System.Data.DataTable _DtTaxonomicGroups;

        public static System.Data.DataTable DtTaxonomicGroups
        {
            get
            {
                if (LookupTable._DtTaxonomicGroups == null)
                {
                    string SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollTaxonomicGroup_Enum' and C.COLUMN_NAME = 'Icon'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1")
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, Icon FROM CollTaxonomicGroup_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    else
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, NULL AS Icon FROM CollTaxonomicGroup_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    LookupTable._DtTaxonomicGroups = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtTaxonomicGroups);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                return LookupTable._DtTaxonomicGroups;
            }
            //set { LookupTable._DtAnalysis = value; }
        }

        public static string TaxonomicGroupVisibleParent(string Code)
        {
            string Parent = "";
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTaxonomicGroups.Select("Code = '" + Code + "'");
            if (RR.Length > 0)
            {
                if (!RR[0]["ParentCode"].Equals(System.DBNull.Value))
                {
                    System.Data.DataRow[] RRParent = DiversityCollection.LookupTable.DtTaxonomicGroupList.Select("Code = '" + RR[0]["ParentCode"].ToString() + "'");
                    if (RRParent.Length > 0)
                    {
                        Parent = RRParent[0]["Code"].ToString();
                        return Parent;
                    }
                    Parent = DiversityCollection.LookupTable.TaxonomicGroupVisibleParent(RR[0]["ParentCode"].ToString());
                }
            }
            return Parent;
        }

        public static void ResetDtTaxonomicGroups()
        {
            LookupTable._DtTaxonomicGroups = null;
        }

        private static System.Data.DataTable _DtTaxonomicGroupList;

        public static System.Collections.Generic.Dictionary<string, string> TaxonomicGroupInfo(string Code)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTaxonomicGroups.Select("Code = '" + Code + "'");
            if (RR.Length > 0)
            {
                foreach(System.Data.DataColumn C in RR[0].Table.Columns)
                    Dict.Add(C.ColumnName, RR[0][C.ColumnName].ToString());
            }
            return Dict;
        }

        /// <summary>
        /// The list of taxonomic groups that were selected by the user for display in the interface
        /// </summary>
        public static System.Data.DataTable DtTaxonomicGroupList
        {
            get
            {
                if (LookupTable._DtTaxonomicGroupList == null 
                    || LookupTable._DtTaxonomicGroupList.Rows.Count == 0)
                {
                    string SQL = "SELECT Code, Description, DisplayText, ParentCode FROM CollTaxonomicGroup_Enum " +
                        "WHERE (DisplayEnable = 1) " +
                        "ORDER BY DisplayOrder, DisplayText";
                    LookupTable._DtTaxonomicGroupList = new System.Data.DataTable();
                    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(LookupTable._DtTaxonomicGroupList);
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups.Length == 0)
                            return LookupTable._DtTaxonomicGroupList;
                        LookupTable._DtTaxonomicGroupList.Clear();
                        int i = 0;
                        foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtTaxonomicGroups.Rows)
                        {
                            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups.Length > i)
                            {
                                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups.Substring(i, 1) == "1")
                                {
                                    System.Data.DataRow Rnew = LookupTable._DtTaxonomicGroupList.NewRow();
                                    Rnew["Code"] = R["Code"].ToString();
                                    Rnew["Description"] = R["Description"].ToString();
                                    Rnew["DisplayText"] = R["DisplayText"].ToString();
                                    Rnew["ParentCode"] = R["ParentCode"].ToString();
                                    LookupTable._DtTaxonomicGroupList.Rows.Add(Rnew);
                                }
                                i++;
                            }
                        }
                    }
                }
                return LookupTable._DtTaxonomicGroupList;
            }
        }

        public static void ResetTaxonomicGroupList() { LookupTable._DtTaxonomicGroupList = null; }

        #endregion

        #region Material categories

        private static System.Data.DataTable _DtMaterialCategories;

        public static System.Data.DataTable DtMaterialCategories
        {
            get
            {
                if (LookupTable._DtMaterialCategories == null)
                {
                    string SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollMaterialCategory_Enum' and C.COLUMN_NAME = 'Icon'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1")
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, Icon FROM CollMaterialCategory_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    else
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, NULL AS Icon FROM CollMaterialCategory_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    LookupTable._DtMaterialCategories = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtMaterialCategories);
                }
                return LookupTable._DtMaterialCategories;
            }
        }

        public static void ResetDtMaterialCategories()
        {
            LookupTable._DtMaterialCategories = null;
        }

        private static System.Data.DataTable _DtMaterialCategoryList;

        /// <summary>
        /// The list of taxonomic groups that were selected by the user for display in the interface
        /// </summary>
        public static System.Data.DataTable DtMaterialCategoryList
        {
            get
            {
                if (LookupTable._DtMaterialCategoryList == null)
                {
                    string SQL = "SELECT Code, Description, DisplayText, ParentCode FROM CollMaterialCategory_Enum " +
                        "WHERE (DisplayEnable = 1) " +
                        "ORDER BY DisplayOrder, DisplayText";
                    LookupTable._DtMaterialCategoryList = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtMaterialCategoryList);
                    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories.Length == 0)
                        return LookupTable._DtMaterialCategoryList;
                    LookupTable._DtMaterialCategoryList.Clear();
                    int i = 0;
                    foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtMaterialCategories.Rows)
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories.Length > i)
                        {
                            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories.Substring(i, 1) == "1")
                            {
                                System.Data.DataRow Rnew = LookupTable._DtMaterialCategoryList.NewRow();
                                Rnew["Code"] = R["Code"].ToString();
                                Rnew["Description"] = R["Description"].ToString();
                                Rnew["DisplayText"] = R["DisplayText"].ToString();
                                Rnew["ParentCode"] = R["ParentCode"].ToString();
                                LookupTable._DtMaterialCategoryList.Rows.Add(Rnew);
                            }
                            i++;
                        }
                    }
                }
                return LookupTable._DtMaterialCategoryList;
            }
        }

        public static string MaterialCategoryVisibleParent(string Code)
        {
            string Parent = "";
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtMaterialCategories.Select("Code = '" + Code + "'");
            if (RR.Length > 0)
            {
                if (!RR[0]["ParentCode"].Equals(System.DBNull.Value))
                {
                    System.Data.DataRow[] RRParent = DiversityCollection.LookupTable.DtMaterialCategoryList.Select("Code = '" + RR[0]["ParentCode"].ToString() + "'");
                    if (RRParent.Length > 0)
                    {
                        Parent = RRParent[0]["Code"].ToString();
                        return Parent;
                    }
                    Parent = DiversityCollection.LookupTable.MaterialCategoryVisibleParent(RR[0]["ParentCode"].ToString());
                }
            }
            return Parent;
        }

        public static void ResetMaterialCategoryList() { LookupTable._DtMaterialCategoryList = null; }

        public static System.Collections.Generic.Dictionary<string, string> MaterialCategoryInfo(string Code)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtMaterialCategories.Select("Code = '" + Code + "'");
            if (RR.Length > 0)
            {
                foreach (System.Data.DataColumn C in RR[0].Table.Columns)
                    Dict.Add(C.ColumnName, RR[0][C.ColumnName].ToString());
            }
            return Dict;
        }
        #endregion

        #region Retrieval Type

        private static System.Data.DataTable _DtRetrievalType;

        public static System.Data.DataTable DtRetrievalType
        {
            get
            {
                if (LookupTable._DtRetrievalType == null)
                {
                    string SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'CollRetrievalType_Enum' and C.COLUMN_NAME = 'Icon'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result == "1")
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, Icon FROM CollRetrievalType_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    else
                        SQL = "SELECT Code, Description, DisplayText, ParentCode, NULL AS Icon FROM CollRetrievalType_Enum " +
                            "WHERE (DisplayEnable = 1) " +
                            "ORDER BY DisplayOrder, DisplayText";
                    LookupTable._DtRetrievalType = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtRetrievalType);
                }
                return LookupTable._DtRetrievalType;
            }
        }

        public static void ResetDtRetrievalType()
        {
            LookupTable._DtRetrievalType = null;
        }
        
        #endregion

        #region External Datasource

        private static System.Data.DataTable _DtExternalDatasource;

        public static System.Data.DataTable DtExternalDatasource
        {
            get
            {
                if (LookupTable._DtExternalDatasource == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT ExternalDatasourceID, ExternalDatasourceName, ExternalDatasourceVersion, Rights, ExternalDatasourceAuthors, ExternalDatasourceURI, " +
                        "ExternalDatasourceInstitution, InternalNotes, ExternalAttribute_NameID, PreferredSequence, Disabled " +
                        "FROM CollectionExternalDatasource ORDER BY ExternalDatasourceName";
                    LookupTable._DtExternalDatasource = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtExternalDatasource);
                }
                return LookupTable._DtExternalDatasource;
            }
        }

        private static System.Data.DataTable _DtExternalDatasourceWithNull;
        public static System.Data.DataTable DtExternalDatasourceWithNull()
        {
            if (LookupTable._DtExternalDatasourceWithNull == null && DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                string SQL = "SELECT NULL AS ExternalDatasourceID, NULL AS ExternalDatasourceName, NULL AS ExternalDatasourceVersion, " +
                    "NULL AS Rights, NULL AS ExternalDatasourceAuthors, NULL AS ExternalDatasourceURI, " +
                    "NULL AS ExternalDatasourceInstitution, NULL AS InternalNotes, NULL AS ExternalAttribute_NameID, NULL AS PreferredSequence, NULL AS Disabled " +
                    "UNION " +
                    "SELECT ExternalDatasourceID, ExternalDatasourceName, ExternalDatasourceVersion, Rights, ExternalDatasourceAuthors, ExternalDatasourceURI, " +
                    "ExternalDatasourceInstitution, InternalNotes, ExternalAttribute_NameID, PreferredSequence, Disabled " +
                    "FROM CollectionExternalDatasource";
                LookupTable._DtExternalDatasourceWithNull = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(LookupTable._DtExternalDatasourceWithNull);
            }
            return LookupTable._DtExternalDatasourceWithNull;
            //System.Data.DataTable dt = LookupTable.DtExternalDatasource.Copy();
            //System.Data.DataRow R = dt.NewRow();
            //R[0] = System.DBNull.Value;
            //R[1] = System.DBNull.Value;
            //dt.Rows.Add(R);
            //return dt;
        }

        public static void ResetExternalDatasource()
        {
            LookupTable._DtExternalDatasource = null;
        }

        #endregion

        #region Regulation

        //private static System.Data.DataTable _DtRegulation;

        //public static System.Data.DataTable DtRegulation
        //{
        //    get
        //    {
        //        if (LookupTable._DtRegulation == null)// && DiversityWorkbench.Forms.FormFunctions.Permissions("Regulation", "UPDATE"))
        //        {
        //            string SQL = "SELECT Regulation, ParentRegulation, [Type], ProjectURI, [Status], Notes, HierarchyOnly " +
        //                "FROM Regulation";
        //            LookupTable._DtRegulation = new System.Data.DataTable();
        //            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //            ad.Fill(LookupTable._DtRegulation);
        //        }
        //        return LookupTable._DtRegulation;
        //    }
        //}

        //public static string RegulationType(string Regulation)
        //{
        //    System.Data.DataRow[] rr = DtRegulation.Select("Regulation = '" + Regulation.Replace("'", "''") + "'", "");
        //    if (rr.Length > 0)
        //        return rr[0]["Type"].ToString();
        //    else return "";
        //}

        //public static void ResetRegulation()
        //{
        //    LookupTable._DtRegulation = null;
        //}

        #endregion

        #region Common

        public enum Lookup { TaxonomicGroup, MaterialCategory, Localisation, CollectionSiteProperty }

        private static System.Collections.Generic.Dictionary<Lookup, System.Collections.Generic.List<string>> _ClientSelection;

        public static System.Collections.Generic.List<string> ClientSelection(Lookup lookup)
        {
            if (_ClientSelection == null) _ClientSelection = new Dictionary<Lookup, List<string>>();
            if (!_ClientSelection.ContainsKey(lookup))
            {
                System.Collections.Generic.List<string> list = new List<string>();
                switch (lookup)
                {
                    case Lookup.TaxonomicGroup:
                        foreach (System.Data.DataRow R in DtTaxonomicGroupList.Rows)
                            list.Add(R["Code"].ToString());
                        break;
                    case Lookup.MaterialCategory:
                        foreach (System.Data.DataRow R in DtMaterialCategoryList.Rows)
                            list.Add(R["Code"].ToString());
                        break;
                    case Lookup.Localisation:
                        foreach (System.Data.DataRow R in DtLocalisationSystem.Rows)
                        {
                            int i;
                            if (int.TryParse(R["LocalisationSystemID"].ToString(), out i) && LocalisationVisible(i))
                                list.Add(i.ToString());
                        }
                        break;
                    case Lookup.CollectionSiteProperty:
                        foreach (System.Data.DataRow R in DtProperty.Rows)
                        {
                            int i;
                            if (int.TryParse(R["PropertyID"].ToString(), out i) && PropertyVisible(i))
                                list.Add(i.ToString());
                        }
                        break;
                }
                _ClientSelection.Add(lookup, list);
            }
            return _ClientSelection[lookup];
        }

        public static void ClientSelection_Reset(Lookup lookup)
        {
            if (_ClientSelection.ContainsKey(lookup))
            {
                _ClientSelection.Remove(lookup);
            }
        }

        public static string LookupTable_Name(Lookup lookup)
        {
            string TableName = "";
            switch (lookup)
            {
                case Lookup.TaxonomicGroup:
                    TableName = "CollTaxonomicGroup_Enum";
                    break;
                case Lookup.MaterialCategory:
                    TableName = "CollMaterialCategory_Enum";
                    break;
                case Lookup.Localisation:
                    TableName = "LocalisationSystem";
                    break;
                case Lookup.CollectionSiteProperty:
                    TableName = "Property";
                    break;
            }
            return TableName;
        }

        public static System.Data.DataTable LookupTable_Table(Lookup lookup)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            switch (lookup)
            {
                case Lookup.TaxonomicGroup:
                    dt = DtTaxonomicGroups;
                    break;
                case Lookup.MaterialCategory:
                    dt = DtMaterialCategories;
                    break;
                case Lookup.Localisation:
                    dt = DtLocalisationSystem;
                    break;
                case Lookup.CollectionSiteProperty:
                    dt = DtProperty;
                    break;
            }
            return dt;
        }

        #endregion

        #endregion

        #region Scalars

        private static string _BaseURL;

        public static string BaseURL
        {
            get 
            {
                if (DiversityCollection.LookupTable._BaseURL == null)
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand COM = new Microsoft.Data.SqlClient.SqlCommand("SELECT dbo.BaseURL()", con);
                    try
                    {
                        con.Open();
                        DiversityCollection.LookupTable._BaseURL = COM.ExecuteScalar().ToString();
                    }
                    catch (Exception)
                    {
                        DiversityCollection.LookupTable._BaseURL = "";
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return LookupTable._BaseURL; 
            }
            //set { LookupTable._BaseURL = value; }
        }

        #endregion

        #region enum

        public static System.Data.DataTable GetEntityContentForEnumTable(string EnumTableName, bool IncludeNull, string ValueColumn, string OrderColumn, string DisplayColumn, string DescriptionColumn, string Restriction)
        {
            System.Data.DataTable DT = new System.Data.DataTable();
            string SQL = "";
            if (DiversityWorkbench.Entity.EntityTablesExist)
            {
                SQL += "declare @Enum Table (" + 
                    ValueColumn + " nvarchar(50), " +
                    DisplayColumn + " nvarchar(1000) NULL, " +
                    DescriptionColumn + " nvarchar(1000) NULL, " +
                    OrderColumn + " nvarchar(1000) NULL) ";
                if (IncludeNull) SQL += "insert @Enum (" + ValueColumn + ", " + DisplayColumn + ", " + DescriptionColumn + ", " + OrderColumn + ") Values (NULL, NULL, NULL, NULL) ";
                SQL += "insert @Enum (" + ValueColumn + ", " + DisplayColumn + ", " + DescriptionColumn + ", " + OrderColumn + ") " +
                    "SELECT  Enum." + ValueColumn + ", cast(Enum." + DisplayColumn + " as nvarchar(1000)) AS " + DisplayColumn + ", " +
                    "cast(Enum." + DescriptionColumn + " as nvarchar(1000)) AS " + DescriptionColumn + ", Enum." + OrderColumn + " " +
                    "FROM " + EnumTableName + " AS Enum " +
                    "update E set E." + DisplayColumn + " = R." + DisplayColumn +
                    ", E." + DescriptionColumn + " = CASE when R." + DescriptionColumn + " Is null then E." + DescriptionColumn + " else R." + DescriptionColumn + " end " +
                    "From  @Enum AS E, EntityRepresentation AS R " +
                    "where '" + EnumTableName + ".Code.' + E.Code = R.Entity " +
                    "and R.LanguageCode = N'" + DiversityWorkbench.Settings.Language + "' AND not R.DisplayText is null " +
                    "Select * from @Enum";
            }
            else
            {
                if (IncludeNull) SQL += "SELECT NULL AS " + ValueColumn + ", NULL AS " + DisplayColumn + ", NULL AS " + DescriptionColumn + ", NULL as " + OrderColumn + "  UNION ";
                SQL += " SELECT " + ValueColumn + ", " + DisplayColumn + ", " + DescriptionColumn + ", " + OrderColumn + " FROM dbo." + EnumTableName +
                    Restriction;
            }
            SQL += " ORDER BY " + OrderColumn + "";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                a.Fill(DT);
            }
            catch { }

            return DT;
        }
        #endregion

        #region Autocomplete

        /// <summary>
        /// Exclusion of columns
        /// </summary>
        /// <param name="Column">Default = Notes, "" = any column with Notes in the name, otherwise the column with the given name (you may use [,] separated list)</param>
        public static void AutoCompletionExcludeNotes(string Column = "Notes")
        {
            string SQL = "select c.TABLE_NAME, c.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS c " +
                "inner join INFORMATION_SCHEMA.TABLES t on c.TABLE_NAME = t.TABLE_NAME and t.TABLE_TYPE = 'BASE TABLE' " +
                "and t.TABLE_NAME not like '%_log' " +
                "and t.TABLE_NAME not like '%_enum' " +
                "where c.COLUMN_NAME ";
            if (Column.Length > 0)
                SQL += " in ('" + Column + "') ";
            else
                SQL += " like '%Notes%' ";
            SQL += "order by c.TABLE_NAME, c.COLUMN_NAME";
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionExclusionAdd(R[0].ToString(), R[1].ToString());
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}
