using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.CacheDatabase
{

    //ToDo - Auslagern der Funktionalität von UserControlLookupSource

    public class LookupSource
    {

        protected enum SubsetTable
        {
            Agent, AgentContactInformation, AgentImage, AgentIdentifier,
            ReferenceTitle, ReferenceRelator,
            Gazetteer, GazetteerExternalDatabase,
            ScientificTerm,
            TaxonSynonymy, TaxonAnalysis, TaxonAnalysisCategory, TaxonAnalysisCategoryValue, TaxonCommonName, TaxonList, TaxonNameExternalDatabase, TaxonNameExternalID,
            SamplingPlot, SamplingPlotLocalisation, SamplingPlotProperty, procSamplingPlotLocalisationHierarchy, procSamplingPlotPropertyHierarchy
        }

        protected DiversityWorkbench.ServerConnection _ServerConn;
        protected DiversityWorkbench.ServerConnection ServerConn(string SourceDatabase)
        {
            if (this._ServerConn == null || SourceDatabase != this._SourceDatabase)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._Module].ServerConnections())
                {
                    if (KV.Value.DisplayText == SourceDatabase)
                    {
                        this._ServerConn = KV.Value;
                        this._SourceDatabase = SourceDatabase;
                        break;
                    }
                }
            }
            return this._ServerConn;
        }

        protected string _SourceDatabase = "";

        protected string _Module = "";

        protected string _MainTable = "";

        protected string _ProjectTable = "";

        protected string _PrimaryKey = "";

        protected System.Collections.Generic.Dictionary<string, object> _TransferHistory = new Dictionary<string, object>();
        public System.Collections.Generic.Dictionary<string, object> TransferHistory() { return this._TransferHistory; }

        protected string _Report = "";
        public string Report() { return this._Report; }

        protected string _Error = "";
        public string Error() { return this._Error; }

        protected System.Collections.Generic.Dictionary<SubsetTable, string> _Subsets;
        protected System.Collections.Generic.Dictionary<SubsetTable, string> Subsets() { return this._Subsets; }

        public System.Collections.Generic.List<string> SourceList()
        {
        System.Collections.Generic.List<string> SourceList = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._Module].ServerConnections())
            {
                SourceList.Add(KV.Value.DisplayText);
            }
            //if (DiversityWorkbench.LinkedServer.LinkedServers().Count > 0)
            //{
            //}
            return SourceList;
        }

        protected System.Data.DataTable DtProjects(DiversityWorkbench.ServerConnection S)
        {
            string PrefixDB = "";
            if (S.LinkedServer.Length > 0)
                PrefixDB = "[" + S.LinkedServer + "].";
            PrefixDB += S.DatabaseName + ".dbo.";
            System.Data.DataTable dtProject = new System.Data.DataTable();
            string SQL = "SELECT ProjectID, Project FROM " + PrefixDB + "ProjectList ORDER BY Project";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
            ad.Fill(dtProject);
            return dtProject;
        }

        public bool CreateSourceViews()
        {
            bool OK = false;

            try
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this.SourceList(), "Source", "Please select a source from the list", true);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    string Source = f.String;
                    DiversityWorkbench.ServerConnection S = this.ServerConn(Source); // null;
                    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this._Module].ServerConnections())
                    //{
                    //    if (KV.Value.DisplayText == Source)
                    //    {
                    //        S = KV.Value;
                    //        break;
                    //    }
                    //}
                    if (S != null)
                    {
                        string PrefixDB = "";
                        if (S.LinkedServer.Length > 0)
                            PrefixDB = "[" + S.LinkedServer + "].";
                        PrefixDB += S.DatabaseName + ".dbo.";
                        System.Data.DataTable dtProject = this.DtProjects(S); // new System.Data.DataTable();
                        //string SQL = "SELECT ProjectID, Project FROM " + PrefixDB + "ProjectList ORDER BY Project";
                        //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, S.ConnectionStringForDB(S.DatabaseName, S.ModuleName));
                        //ad.Fill(dtProject);
                        DiversityWorkbench.Forms.FormGetStringFromList fProject = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "Project", "Please select the project");
                        fProject.ShowDialog();
                        if (fProject.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            int ProjectID;
                            if (int.TryParse(fProject.SelectedValue, out ProjectID))
                            {
                                string Project = fProject.SelectedString;
                                string View = this.CreateSourceViews(S, fProject.SelectedString, ProjectID);
                                if (View.Length > 0)
                                {
                                    //string SQL = "INSERT INTO ReferenceTitleSource (SourceView, Source, SourceID, LinkedServerName, DatabaseName, Subsets) " +
                                    //    "VALUES ('" + View + "', '" + Project + "', " + ProjectID.ToString() + ", '" + S.LinkedServer + "', '" + S.DatabaseName + "' " +
                                    //    ", '" + this.SourceSubsets(UserControlLookupSource.SubsetTable.ReferenceTitle) + "')";
                                    //if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                                    //    this.initReferenceTitleSources();
                                }
                            }
                        }
                    }
                }
                OK = true;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        protected string CreateSourceViews(DiversityWorkbench.ServerConnection ServerConn, string Project, int ProjectID)
        {
            // Check if Database is prepared
            if (!this.DatabaseIsPreparedForUsageAsSouce(ServerConn.DatabaseName))
                return "";

            // Try to get a name for the basic view
            string View = this.ViewName(Project);
            if (View.Length == 0)
                return "";

            // ServerConn.DatabaseName.Replace("Diversity", "") + "_";
            //if (ServerConn.LinkedServer.Length > 0)
            //{
            //    string LinkedServerMarker = ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
            //    if (!View.EndsWith("_" + LinkedServerMarker))
            //        View += ServerConn.LinkedServer.Substring(0, ServerConn.LinkedServer.IndexOf(".")) + "_";
            //}
            //View += Project;
            //View = this.ConvertToSqlName(View);
            //// Check if there are sources in table ...Source with the same name and change name if needed
            //string SQL = "SELECT COUNT(*) FROM " + this._MainTable + "Source S WHERE S.SourceName LIKE '" + View + "%'";
            //string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            //if (Result != "0" && Result != "")
            //{
            //    int i = int.Parse(Result);
            //    View += "_" + (i + 1).ToString();
            //}
            string PrefixDB = "";
            if (ServerConn.LinkedServer.Length > 0)
                PrefixDB = "[" + ServerConn.LinkedServer + "].";
            PrefixDB += ServerConn.DatabaseName + ".dbo.";
            //string Database = ServerConn.DatabaseName;
            //string SQL = "SELECT U.BaseURL FROM " + PrefixDB + "ViewBaseURL AS U";
            //string BaseURL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            //if (BaseURL.Length == 0)
            //{
            //    System.Windows.Forms.MessageBox.Show("This database is not prepared for usage by cache databases. Turn to your administrator for an update");
            //    return "";
            //}

            // Check if the view allready exists and remove it after OK
            //SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
            //string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            //if (Check != "0")
            //{
            //    if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            //    {
            //        SQL = "DROP VIEW [dbo].[" + View + "]";
            //        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            //            System.Windows.Forms.MessageBox.Show("Old view removed");
            //        else
            //        {
            //            System.Windows.Forms.MessageBox.Show("Removal failed");
            //            return "";
            //        }
            //    }
            //    else
            //        return "";
            //}

            // Create the basic view
            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT B.BaseURL, R.RefType, R.RefID, R.RefDescription_Cache, R.Title, R.DateYear, R.DateMonth, R.DateDay, R.DateSuppl, R.SourceTitle, R.SeriesTitle, R.Periodical, R.Volume, R.Issue, R.Pages, R.Publisher, " +
                "R.PublPlace, R.Edition, R.DateYear2, R.DateMonth2, R.DateDay2, R.DateSuppl2, R.ISSN_ISBN, R.Miscellaneous1, R.Miscellaneous2, R.Miscellaneous3, R.UserDef1, R.UserDef2, R.UserDef3, R.UserDef4,  " +
                "R.UserDef5, R.WebLinks, R.LinkToPDF, R.LinkToFullText, R.RelatedLinks, R.LinkToImages, R.SourceRefID, R.Language, R.CitationText, R.CitationFrom, R.LogInsertedWhen AS LogUpdatedWhen, P.ProjectID " +
                "FROM " + PrefixDB + "ReferenceTitle AS R, " +
                PrefixDB + "ReferenceProject AS P, " +
                PrefixDB + "ViewBaseURL AS B " +
                "WHERE R.RefID = P.RefID AND P.ProjectID = " + ProjectID.ToString();

            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
            {
                SQL = "GRANT SELECT ON " + View + " TO CacheUser";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    //if (this.CreateReferenceRelatorSource(PrefixDB, View, ProjectID))
                    //    return View;
                    //else
                    //    return "";
                }
                else
                    return "";
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Message);
                return "";
            }
            return View;
        }

        #region Auxillary

        protected bool DatabaseIsPreparedForUsageAsSouce(string DatabaseName)
        {
            bool OK = true;
            try
            {
                string PrefixDB = "";
                if (this.ServerConn(this._Module).LinkedServer.Length > 0)
                    PrefixDB = "[" + this.ServerConn(this._Module).LinkedServer + "].";
                PrefixDB += this.ServerConn(this._Module).DatabaseName + ".dbo.";
                string Database = this.ServerConn(this._Module).DatabaseName;
                string SQL = "SELECT U.BaseURL FROM " + PrefixDB + "ViewBaseURL AS U";
                string BaseURL = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (BaseURL.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("This database is not prepared for usage by cache databases. Turn to your administrator for an update");
                    OK = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        protected string ViewName(string Project)
        {
            string View = "";

            try
            {
                View = this.ServerConn(this._Module).DatabaseName.Replace("Diversity", "") + "_";
                if (this.ServerConn(this._Module).LinkedServer.Length > 0)
                {
                    string LinkedServerMarker = this.ServerConn(this._Module).LinkedServer.Substring(0, this.ServerConn(this._Module).LinkedServer.IndexOf(".")) + "_";
                    if (!View.EndsWith("_" + LinkedServerMarker))
                        View += this.ServerConn(this._Module).LinkedServer.Substring(0, this.ServerConn(this._Module).LinkedServer.IndexOf(".")) + "_";
                }
                View += Project;
                View = this.ConvertToSqlName(View);
                // Check if there are sources in table ReferenceTitleSource with the same name and change name if needed
                string SQL = "SELECT COUNT(*) FROM " + this._MainTable + " Source S WHERE S.SourceName LIKE '" + View + "%'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result != "0" && Result != "")
                {
                    int i = int.Parse(Result);
                    View += "_" + (i + 1).ToString();
                }
                // Check if the view allready exists and remove it after OK
                SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS V where V.TABLE_NAME = '" + View + "'";
                string Check = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Check != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show("The view\r\n" + View + "\r\nis allready present. Should it be replaced by the new version?", "Remove previous view", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        SQL = "DROP VIEW [dbo].[" + View + "]";
                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                            System.Windows.Forms.MessageBox.Show("Old view removed");
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Removal failed");
                            View = "";
                        }
                    }
                    else
                        View = "";
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                View = "";
            }
            return View;
        }

        protected string ConvertToSqlName(string Name)
        {
            string Result = "";

            try
            {
                bool ToUpper = true;
                foreach (char c in Name)
                {
                    int i = (int)c;
                    if (i == 32) ToUpper = true;
                    if ((i > 10 && i < 48) || (i > 58 && i < 65) || (i > 90 && i < 95) || (i > 95 && i < 97) || i > 122)
                        continue;
                    if (ToUpper) Result += c.ToString().ToUpper();
                    else Result += c.ToString();
                    ToUpper = false;
                    if (Result.Length > 100) break;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Result;
        }

        private string TableColumns(string Table, string View = "", string Target = "", string Alias = "")
        {
            string SQL = "";
            try
            {
                if (View.Length > 0 && Target.Length > 0 && Alias.Length > 0)
                {
                    string SqlColumns = "select v.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS v, INFORMATION_SCHEMA.COLUMNS t " +
                        "where v.TABLE_NAME = '" + View + "' " +
                        "and t.TABLE_NAME = '" + Target + "' " +
                        "and v.COLUMN_NAME = t.COLUMN_NAME";
                    System.Data.DataTable dtV = new System.Data.DataTable();
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SqlColumns, ref dtV, ref Message);
                    foreach (System.Data.DataRow R in dtV.Rows)
                    {
                        if (SQL.Length > 0)
                            SQL += ", ";
                        SQL += Alias + ".[" + R[0].ToString() + "]";
                    }
                }
                else
                {
                    string SqlColumns = "select t.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS t " +
                        "where t.TABLE_NAME = '" + Table + "' ";
                    System.Data.DataTable dtV = new System.Data.DataTable();
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SqlColumns, ref dtV, ref Message);
                    foreach (System.Data.DataRow R in dtV.Rows)
                    {
                        if (SQL.Length > 0)
                            SQL += ", ";
                        SQL += "[" + R[0].ToString() + "]";
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                SQL = "";
            }
            return SQL;
        }

        #endregion

        }
    }
