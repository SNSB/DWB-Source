using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class Terminology : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Construction
        public Terminology(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityScientificTerms"; }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            this._UnitValues = new Dictionary<string, string>();

            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT U.BaseURL + 'Terminology/' + CAST(TerminologyID AS varchar) AS _URI, DisplayText AS _DisplayText, " +
                    "DisplayText, Description, ExternalDatabase, ExternalDatabaseVersion, ExternalDatabaseAuthors, ExternalDatabaseURI, " +
                    "ExternalDatabaseInstitution, ExternalAttribute_NameID, Rights, DefaultLanguageCode, TerminologyID " +
                    "FROM   " + Prefix + "Terminology AS T, " + Prefix + "ViewBaseURL U " +
                    "WHERE   TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT   DisplayText AS Property /*, Description, Datatype*/ " +
                    "FROM            " + Prefix + "TerminologyProperty AS P " +
                    "WHERE        TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT        Reference, ReferenceDetails " +
                    "FROM            " + Prefix + "TerminologyReference AS R " +
                    "WHERE        TerminologyID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);
            }
            return this._UnitValues;
        }

        public string MainTable() { return "Terminology"; }
        
        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
            QueryDisplayColumns[0].DisplayText = "DisplayText";
            QueryDisplayColumns[0].DisplayColumn = "DisplayText";
            QueryDisplayColumns[0].OrderColumn = "DisplayText";
            QueryDisplayColumns[0].IdentityColumn = "TerminologyID";
            QueryDisplayColumns[0].TableName = "Terminology";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            string Database = "DiversityScientificTerms";
            try
            {
                Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnection.DatabaseName;
            }
            catch { }

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string Description = "Scientific terminology";
            DiversityWorkbench.QueryCondition qTerminology = new DiversityWorkbench.QueryCondition(true, "Terminology", "TerminologyID", "DisplayText", "Terminology", "Terminology", "Scientific terminology", Description);
            QueryConditions.Add(qTerminology);

            Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "ExternalDatabase");
            DiversityWorkbench.QueryCondition qExternalDatabase = new DiversityWorkbench.QueryCondition(true, "Terminology", "TerminologyID", "ExternalDatabase", "Terminology", "Ext. Database", "External database", Description);
            QueryConditions.Add(qExternalDatabase);

            Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "DefaultLanguageCode");
            DiversityWorkbench.QueryCondition qLanguageCode = new DiversityWorkbench.QueryCondition(true, "Terminology", "TerminologyID", "DefaultLanguageCode", "Terminology", "Language", "2-letter ISO code of the language", Description, "LanguageCode_Enum", Database);
            QueryConditions.Add(qLanguageCode);

            // DATABASE
            //System.Data.DataTable dtTerminology = new System.Data.DataTable();
            //string SQL = "SELECT TerminologyID AS [Value], Terminology.DisplayText AS Display " +
            //        "FROM Terminology " +
            //        "ORDER BY DisplayText";
            //if (this._ServerConnection.ConnectionString.Length > 0)
            //{
            //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            //    try { a.Fill(dtTerminology); }
            //    catch { }
            //}
            //if (dtTerminology.Columns.Count == 0)
            //{
            //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
            //    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
            //    dtTerminology.Columns.Add(Value);
            //    dtTerminology.Columns.Add(Display);
            //}
            //SQL = "FROM Terminology INNER JOIN " +
            //    "Term ON Terminology.TerminologyID = Term.TerminologyID ";
            //Description = DiversityWorkbench.Functions.ColumnDescription("Terminology", "DisplayText");
            //DiversityWorkbench.QueryCondition qTerminology = new DiversityWorkbench.QueryCondition(true, "Terminology", "TerminologyID", true, SQL, "Terminology.TerminologyID", "Terminology", "Terminology", "Terminology", Description, dtTerminology, false);
            //QueryConditions.Add(qTerminology);

            return QueryConditions;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID)
        {
            return this.UnitValues(ID);
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns(string Domain)
        {
            return this.QueryDisplayColumns();
        }
        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions(string Domain)
        {
            return this.QueryConditions();
        }


        #endregion

        #region Properties

        //public override DiversityWorkbench.ServerConnection ServerConnection
        //{
        //    get { return _ServerConnection; }
        //    set 
        //    {
        //        if (value != null)
        //            this._ServerConnection = value;
        //        else
        //        {
        //            this._ServerConnection = new ServerConnection();
        //            this._ServerConnection.DatabaseServer = "127.0.0.1";
        //            this._ServerConnection.IsTrustedConnection = true;
        //        }
        //        this._ServerConnection.ModuleName = "DiversityScientificTerms";
        //        this._ServerConnection.DatabaseName = "DiversityScientificTerms";
        //    }
        //}

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

        public static System.Collections.Generic.Dictionary<int, string> Sections(int TerminologyID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, string> Sect = Terminology.getSections(TerminologyID, SC);
            return Sect;
        }

        public static Chart GetChart(string URI, int? SectionID = null, int? Height = null, int? Width = null, bool ResetChart = false)//), int? ColumnWidth = null
        {
            if (_Charts == null)
                _Charts = new Dictionary<string, Chart>();

            string ChartMarker = URI;
            if (SectionID != null)
                ChartMarker += "-" + SectionID.ToString();

            if (_Charts.ContainsKey(ChartMarker))
            {
                if (ResetChart)
                    _Charts.Remove(ChartMarker);
                else if (Width != null && _Charts[ChartMarker].WindowWidth() != (int)Width)
                    _Charts.Remove(ChartMarker);
                else
                    return _Charts[ChartMarker];
            }

            DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);
            if (SC == null)
                return null;

            // Getting the TerminologyID
            int TerminologyID = 0;
            if (!int.TryParse(URI.Substring(URI.LastIndexOf("/") + 1), out TerminologyID))
                return null;

            try
            {
                // Getting the colors
                System.Collections.Generic.Dictionary<int, System.Drawing.Color> ChartColors = Terminology.ChartColors(TerminologyID, SC); //  new Dictionary<int, System.Drawing.Color>();
                System.Collections.Generic.List<int> InheritColors = Terminology.InheritColors(TerminologyID, SC);

                // Getting the images                                                                                                                                                // Getting the images
                System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> ChartImages = Terminology.ChartImages(TerminologyID, SC);

                // Getting the groups
                //System.Collections.Generic.List<int> ChartGroups = Terminology.IDsOfGroups(TerminologyID, SC);// Terminology.ChartGroups(TerminologyID, SC);

                System.Collections.Generic.List<int> IDsInSection = new List<int>();
                string Section = "";
                if (SectionID != null && SectionID > 0)
                {
                    string Prefix = "dbo.";
                    if (SC.LinkedServer != null && SC.LinkedServer.Length > 0)
                        Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                    string SQL = "SELECT TermID FROM " + Prefix + "SectionTerm WHERE(TerminologyID = " + TerminologyID.ToString() + ") AND(SectionID = " + SectionID.ToString() + ")";
                    System.Data.DataTable dtSection = new System.Data.DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSection, SC.ConnectionString);
                    foreach (System.Data.DataRow R in dtSection.Rows)
                    {
                        int id;
                        if (int.TryParse(R[0].ToString(), out id))
                            IDsInSection.Add(id);
                    }
                    SQL = "SELECT DisplayText FROM " + Prefix + "Section WHERE(TerminologyID = " + TerminologyID.ToString() + ") AND(SectionID = " + SectionID.ToString() + ")";
                    Section = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, SC.ConnectionString);
                }

                // Getting the data
                System.Data.DataTable dtTerm = Terminology.ChartData(TerminologyID, SC);

                // RepresentationIDs
                System.Collections.Generic.Dictionary<int, int> RepIDs = new Dictionary<int, int>();
                foreach(System.Data.DataRow R in dtTerm.Rows)
                {
                    int RepID;
                    int TermID;
                    if (int.TryParse(R["RepresentationID"].ToString(), out RepID) &&
                        int.TryParse(R["TermID"].ToString(), out TermID))
                    {
                        if (!RepIDs.ContainsKey(TermID))
                            RepIDs.Add(TermID, RepID);
                    }
                }

                // getting the chart
                System.Collections.Generic.List<string> SortingColumns = new List<string>();
                SortingColumns.Add("DisplayOrder");
                SortingColumns.Add("DisplayText");

                // additional infos shown in the title
                System.Collections.Generic.Dictionary<int, string> Titles = ChartTitles(TerminologyID, SC);

                // getting the BaseURL
                string BaseURL = DiversityWorkbench.WorkbenchUnit.getBaseURIfromURI(URI);

                DiversityWorkbench.Chart C = new DiversityWorkbench.Chart(BaseURL, dtTerm, "TermID", "BroaderTermID", "DisplayText",
                    ChartColors, SortingColumns, ChartImages,
                    RepIDs,
                    SectionID, IDsInSection, Section,
                    Width, Height, //null, 
                    "", null, Titles, InheritColors);
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

        private static System.Collections.Generic.Dictionary<int, string> ChartTitles(int TerminologyID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, string> Titles = new Dictionary<int, string>();
            try
            {
                System.Data.DataTable dtTerm = new System.Data.DataTable();
                string Prefix = "dbo.";
                if (SC.LinkedServer != null && SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                string SQL = "SELECT DISTINCT T.TermID, R.DisplayText " +
                    "FROM " + Prefix + "Term AS T INNER JOIN " +
                    "" + Prefix + "TermRepresentation AS R ON T.TerminologyID = R.TerminologyID AND T.TermID = R.TermID AND T.PreferredRepresentationID<> R.RepresentationID " +
                    "WHERE (T.TerminologyID = " + TerminologyID.ToString() + ") AND (T.IsRankingTerm = 0 OR T.IsRankingTerm is null) " +
                    "ORDER BY R.DisplayText";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTerm, SC.ConnectionString);
                foreach(System.Data.DataRow R in dtTerm.Rows)
                {
                    int TermID;
                    if (int.TryParse(R[0].ToString(), out TermID))
                    {
                        if (Titles.ContainsKey(TermID))
                        {
                            Titles[TermID] += "; " + R[1].ToString();
                        }
                        else
                        {
                            Titles.Add(TermID, "= " + R[1].ToString());
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

        private static System.Collections.Generic.Dictionary<int, System.Drawing.Color> ChartColors(int TerminologyID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, System.Drawing.Color> Colors = new System.Collections.Generic.Dictionary<int, System.Drawing.Color>();
            try
            {
                string Prefix = "dbo.";
                if (SC.LinkedServer != null && SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                string SQL = "SELECT TermID, MAX(DisplayARGB) AS DisplayARGB " +
                    "FROM " + Prefix + "TermRepresentation AS R " +
                    "WHERE (TerminologyID = " + TerminologyID.ToString() + ") and DisplayARGB <> '' " +
                    "GROUP BY TermID";
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


        private static System.Collections.Generic.List<int> InheritColors(int TerminologyID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.List<int> Colors = new System.Collections.Generic.List<int>();
            try
            {
                string Prefix = "dbo.";
                if (SC.LinkedServer != null && SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                string SQL = "SELECT TermID " +
                    "FROM " + Prefix + "TermRepresentation AS R " +
                    "WHERE (TerminologyID = " + TerminologyID.ToString() + ") and DisplayInheritARGB = 1 " +
                    "GROUP BY TermID";
                System.Data.DataTable dtColor = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtColor, SC.ConnectionString);
                foreach (System.Data.DataRow R in dtColor.Rows)
                {
                    int ID;
                    if (!int.TryParse(R[0].ToString(), out ID))
                        continue;
                    if (Colors.Contains(ID))
                        continue;
                    Colors.Add(ID);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Colors;
        }

        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> ChartImages(int TerminologyID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> Images = new Dictionary<int, System.Collections.Generic.List<ChartImage>>();
            try
            {
                int Height = Chart.ImageMaxHeight;
                int Width = Chart.ImageMaxWidth;
                string Prefix = "dbo.";
                if (SC.LinkedServer != null && SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                string SQL = "SELECT T.TermID, R.Uri, R.Title, R.DisplayOrder " +
                    " FROM " + Prefix + "ViewTermResource AS R INNER JOIN " + Prefix + "TermRepresentation T ON R.RepresentationID = T.RepresentationID  " +
                    " WHERE (DataWithholdingReason IS NULL OR DataWithholdingReason = '') AND (R.URI <> '') AND T.TerminologyID = " + TerminologyID.ToString() +
                    " ORDER BY R.DisplayOrder";
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



        public static System.Collections.Generic.Dictionary<int, string> getSections(int TerminologyID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, string> Sections = new Dictionary<int, string>();
            try
            {
                string Prefix = "dbo.";
                if (SC.LinkedServer != null && SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                string SQL = "SELECT S.SectionID, 'Section ' + S.DisplayText " +
                    "FROM " + Prefix + "Section AS S WHERE (S.TerminologyID = " + TerminologyID.ToString() + ") " +
                    "ORDER BY S.DisplayText";
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
                    SQL = "SELECT ' ' + DisplayText FROM " + Prefix + "Terminology WHERE TerminologyID = " + TerminologyID.ToString();
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


        private static System.Data.DataTable ChartData(int TerminologyID, DiversityWorkbench.ServerConnection SC)
        {
            string Prefix = "dbo.";
            if (SC.LinkedServer != null && SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
            // get the name of the terminology
            string Terminology = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT DisplayText FROM " + Prefix + "Terminology " +
                " WHERE TerminologyID = " + TerminologyID.ToString(), SC.ConnectionString).Replace(" ", "_");
            System.Data.DataTable dtTerm = new System.Data.DataTable(Terminology);

            try
            {
                string SQL = "SELECT R.RepresentationID, R.TermID, T.BroaderTermID, R.DisplayText, R.DisplayOrder " +
                    "FROM " + Prefix + "TermRepresentation AS R INNER JOIN " +
                    "" + Prefix + "Term AS T ON R.TerminologyID = T.TerminologyID AND R.TermID = T.TermID AND R.RepresentationID = T.PreferredRepresentationID " +
                    "WHERE (T.TerminologyID = " + TerminologyID.ToString() + ") AND (T.IsRankingTerm = 0 OR T.IsRankingTerm is null) " +
                    "ORDER BY R.DisplayOrder, R.DisplayText";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTerm, SC.ConnectionString);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtTerm;
        }

        #endregion


    }
}
