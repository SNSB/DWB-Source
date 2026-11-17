using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversitySamplingPlots
{
    class LookupTable
    {
        #region Lookup tables

        #region Global functions

        public static void ResetLookupTables()
        {
            ResetEventPropertyList();
        }

        public static void ResetLanguageVariantLookupTables()
        {
            ResetLocalisationSystem();
            ResetEventPropertyList();
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
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(LookupTable._DtLocalisationSystem);
                }
                return LookupTable._DtLocalisationSystem;
            }
            //set { LookupTable._Localisation = value; }
        }

        // returns a new datatable, e.g. for datasource of comboboxes
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

        //public static bool LocalisationVisible(int LocalisationSystemID)
        //{
        //    bool OK = true;
        //    if (DiversitySamplingPlots.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Length > 0)
        //        for (int i = 0; i < LookupTable.DtLocalisationSystem.Rows.Count; i++)
        //        {
        //            if (LookupTable.DtLocalisationSystem.Rows[i]["LocalisationSystemID"].ToString() == LocalisationSystemID.ToString())
        //            {
        //                if (DiversitySamplingPlots.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Length > i &&
        //                    DiversitySamplingPlots.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Substring(i, 1) == "0")
        //                    return false;
        //                else return true;
        //            }
        //        }
        //    return OK;
        //}

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

        public static System.Collections.Generic.Dictionary<string, string> LocalisationSystemInfo(string Code)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            System.Data.DataRow[] RR = DiversitySamplingPlots.LookupTable.DtLocalisationSystem.Select("LocalisationSystemID = " + Code);
            if (RR.Length > 0)
            {
                foreach (System.Data.DataColumn C in RR[0].Table.Columns)
                    Dict.Add(C.ColumnName, RR[0][C.ColumnName].ToString());
            }
            return Dict;
        }

        public static void ResetLocalisationSystem()
        {
            DiversitySamplingPlots.LookupTable._DtLocalisationSystem = null;
        }

        public enum CoordinateSystem { WGS84, PotsdamDatum, GaussKrüger };
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
                        "DefaultMeasurementUnit, ParsingMethodName, DisplayText, DisplayEnabled, DisplayOrder, Description " +
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

        //public static bool PropertyVisible(int PropertyID)
        //{
        //    bool OK = true;
        //    if (DiversitySamplingPlots.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Length > 0)
        //        for (int i = 0; i < LookupTable.DtProperty.Rows.Count; i++)
        //        {
        //            if (DiversitySamplingPlots.LookupTable.DtProperty.Rows[i]["PropertyID"].ToString() == PropertyID.ToString())
        //            {
        //                if (DiversitySamplingPlots.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Length > i &&
        //                    DiversitySamplingPlots.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Substring(i, 1) == "0")
        //                    return false;
        //                break;
        //            }
        //        }
        //    return OK;
        //}

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

        public static System.Collections.Generic.Dictionary<string, string> PropertyInfo(string Code)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            System.Data.DataRow[] RR = DiversitySamplingPlots.LookupTable.DtProperty.Select("PropertyID = " + Code);
            if (RR.Length > 0)
            {
                foreach (System.Data.DataColumn C in RR[0].Table.Columns)
                    Dict.Add(C.ColumnName, RR[0][C.ColumnName].ToString());
            }
            return Dict;
        }

        public static void ResetEventPropertyList()
        {
            DiversitySamplingPlots.LookupTable._DtProperty = null;
        }

        #endregion

        #region Project

        private static System.Data.DataTable _DtProject;

        public static System.Data.DataTable DtProject
        {
            get
            {
                if (LookupTable._DtProject == null)
                {
                    bool OK = true;
                    string SQL = "SELECT DiversityProjects.dbo.Project.ProjectID, DiversityProjects.dbo.Project.ProjectParentID, DiversityProjects.dbo.Project.Project, " +
                        "DiversityProjects.dbo.Project.ProjectTitle, DiversityProjects.dbo.Project.ProjectDescription, DiversityProjects.dbo.Project.ProjectEditors,  " +
                        "DiversityProjects.dbo.Project.ProjectNotes, DiversityProjects.dbo.Project.ProjectCopyright, DiversityProjects.dbo.Project.ProjectVersion " +
                        "FROM DiversityProjects.dbo.Project INNER JOIN " +
                        "ProjectUser ON DiversityProjects.dbo.Project.ProjectID = ProjectUser.ProjectID " +
                        "WHERE (ProjectUser.LoginName = USER_NAME()) " +
                        "ORDER BY DiversityProjects.dbo.Project.Project";
                    LookupTable._DtProject = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(LookupTable._DtProject);
                    }
                    catch (System.Exception ex)
                    {
                        //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        OK = false;
                    }
                    if (!OK)
                    {
                        SQL = "SELECT ProjectProxy.ProjectID, NULL AS ProjectParentID, ProjectProxy.Project, ProjectProxy.Project AS ProjectTitle, " +
                            "NULL AS ProjectDescription, NULL AS ProjectEditors,  " +
                            "NULL AS ProjectNotes, NULL AS ProjectCopyright, NULL AS ProjectVersion " +
                            "FROM ProjectProxy INNER JOIN " +
                            "ProjectUser ON ProjectProxy.ProjectID = ProjectUser.ProjectID " +
                            "WHERE (ProjectUser.LoginName = USER_NAME()) " +
                            "ORDER BY ProjectProxy.Project";
                        ad.SelectCommand.CommandText = SQL;
                        ad.Fill(LookupTable._DtProject);
                    }
                }
                return LookupTable._DtProject;
            }
        }

        public static string ProjectName(int ProjectID)
        {
            string Name = "";
            foreach (System.Data.DataRow R in LookupTable.DtProject.Rows)
            {
                if (ProjectID.ToString() == R["ProjectID"].ToString())
                {
                    Name = R["Project"].ToString();
                    break;
                }
            }
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

        #endregion

        #region Scalars

        private static string _BaseURL;

        public static string BaseURL
        {
            get
            {
                if (DiversitySamplingPlots.LookupTable._BaseURL == null)
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand COM = new Microsoft.Data.SqlClient.SqlCommand("SELECT dbo.BaseURL()", con);
                    try
                    {
                        con.Open();
                        DiversitySamplingPlots.LookupTable._BaseURL = COM.ExecuteScalar().ToString();
                    }
                    catch (Exception)
                    {
                        DiversitySamplingPlots.LookupTable._BaseURL = "";
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

    }
}
