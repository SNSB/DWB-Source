using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.CacheDatabase.Diagnostics
{

    public struct Group
    {
        public string Name;
        public System.Collections.Generic.List<object> Content;
        public Group(string Name)
        {
            this.Name = Name;
            this.Content = new List<object>();
        }
    }

    public struct Result
    {
        public string Title;
        public string Description;
        public string DescriptionForSelection;
        public string Selection;
        public string ToolTip;
        public bool OK;
        public bool ForPostgres;
        public Result(string Title)
        {
            this.Title = Title;
            this.Description = "";
            this.DescriptionForSelection = "";
            this.Selection = "";
            this.ToolTip = "";
            OK = true;
            ForPostgres = false;
        }
    }

    public class Target
    {
        #region Parameter

        protected string _Project;
        protected int _ProjectID;
        protected string _ProjectDB;
        protected string _PostgresDatabase;

        #endregion

        #region Common

        protected string ProjectDB()
        {
            string SQL = "SELECT [dbo].[ProjectsDatabase] ()";
            _ProjectDB = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            return this._ProjectDB;
        }

        protected string PostgresDatabase()
        {
            if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null &&
                DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name).Length > 0)
                this._PostgresDatabase = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
            else
                this._PostgresDatabase = "";
            return this._PostgresDatabase;

        }

        public enum TargetType { ABCD }

        public int TableColumnCount = 9;

        protected System.Collections.Generic.List<Group> _DiagnosticGroups;
        protected System.Collections.Generic.List<Group> DiagnosticGroups()
        {
            if (this._DiagnosticGroups == null)
                this._DiagnosticGroups = new List<Group>();
            return this._DiagnosticGroups;
        }

        public bool StartDiagnostics(DiversityWorkbench.Forms.FormStarting formStarting = null)
        {
            bool OK = true;
            this._DiagnosticGroups = null;
            this._DiagnosticGroups = this.TestResults(formStarting);
            this.initHtml5();
            return OK;
        }

        public virtual System.Collections.Generic.List<Group> TestResults(DiversityWorkbench.Forms.FormStarting formStarting = null)
        {
            return this.DiagnosticGroups();
        }

        protected string _ResultFile;
        public string ResultFile()
        {
            if (this._ResultFile == null)
                this._ResultFile = DiversityCollection.CacheDatabase.CacheDB.DiagnosticsDirectory() + DiversityCollection.CacheDatabase.CacheDB.DatabaseName + "_Diagnostics.htm";
            System.IO.FileInfo f = new System.IO.FileInfo(this._ResultFile);
            return f.FullName;
        }

        #endregion

        #region HTML

        private System.Uri initHtml5()
        {
            try
            {
                System.Text.StringBuilder builder = new StringBuilder();
                System.IO.StringWriter stringWriter = new System.IO.StringWriter(builder);
                using (System.Web.UI.HtmlTextWriter HWriter = new System.Web.UI.HtmlTextWriter(stringWriter))
                {
                    HWriter.WriteLine("<!DOCTYPE html>");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Html);
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Head);
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Style);
                    HWriter.WriteLine("table, th, td { border: 0px solid black; border-collapse: collapse; font-family: Arial; text-align: left; } ");
                    HWriter.WriteLine("th, td {padding: 5px;}");
                    HWriter.RenderEndTag(); // Style
                    HWriter.RenderEndTag(); // Head
                    HWriter.WriteLine();
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Body);
                    // Header
                    HWriter.AddStyleAttribute("font-weight", "bold");
                    HWriter.AddStyleAttribute("font-family", "Arial");
                    HWriter.AddStyleAttribute("font-size", "20 px");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Span);
                    HWriter.WriteLine("Diagnostics for project " + this._Project);
                    HWriter.RenderEndTag();
                    HWriter.WriteLine();
                    //HWriter.WriteBreak();
                    HWriter.WriteBreak();
                    // Table
                    HWriter.AddStyleAttribute("width", "100%");
                    HWriter.AddStyleAttribute("margin", "0px");
                    HWriter.AddStyleAttribute("overflow", "hidden");
                    HWriter.AddStyleAttribute("table-layout", "fixed");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Table);
                    // Table head
                    this.Html5writeTableHeader(HWriter);
                    // Table body
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tbody);
                    for (int i = 0; i < this.DiagnosticGroups().Count; i++)
                    {
                        this.Html5writeTableRow(HWriter, this.DiagnosticGroups()[i]);
                        HWriter.WriteLine();
                    }
                    HWriter.RenderEndTag(); // Tbody
                    HWriter.RenderEndTag(); // Table
                    HWriter.WriteLine();


                    HWriter.WriteLine();
                    HWriter.RenderEndTag(); // Body
                    HWriter.RenderEndTag(); // Html
                }

                System.IO.File.WriteAllText(this.ResultFile(), stringWriter.ToString());
                System.Uri uTest = new Uri(this.ResultFile());
                return uTest;

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return null;
        }

        private void Html5writeTableHeader(System.Web.UI.HtmlTextWriter HWriter)
        {
            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Thead);
            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
            for (int i = 0; i < this.TableColumnCount; i++)
            {
                HWriter.AddAttribute("visible", "false");
                if (i < this.TableColumnCount - 1)
                    HWriter.AddAttribute("width", "16px");
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Th);
                HWriter.RenderEndTag(); // Th
                HWriter.WriteLine();
            }
            HWriter.RenderEndTag(); // Tr
            HWriter.RenderEndTag(); // Thead
            HWriter.WriteLine();
        }

        private void Html5writeTableRow(System.Web.UI.HtmlTextWriter HWriter, Group G, int Level = 0)
        {
            try
            {
                // separater for basic groups
                if (Level == 0)
                    this.Html5writeEmptyRow(HWriter, 10);

                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
                if (Level > 0)
                    this.Html5writeTableRowPrefixColumns(HWriter, Level, true);

                HWriter.AddAttribute("colspan", (this.TableColumnCount - Level).ToString());
                HWriter.AddStyleAttribute("background-color", "#EEEEEE");
                HWriter.AddStyleAttribute("color", "black");
                int Size = 20 - Level * 2;
                HWriter.AddStyleAttribute("font-size", Size.ToString() + "px");
                HWriter.AddStyleAttribute("font-weight", "bold");
                if (Level == 0)
                    HWriter.AddStyleAttribute("text-decoration", "underline");
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                HWriter.WriteEncodedText(G.Name);
                HWriter.RenderEndTag(); // Td
                HWriter.WriteLine();

                HWriter.RenderEndTag(); // Tr
                HWriter.WriteLine();

                for (int i = 0; i < G.Content.Count; i++)
                {
                    if (G.Content[i].GetType() == typeof(Group))
                    {
                        // Row for distance to previous group
                        this.Html5writeEmptyRow(HWriter, 5);

                        this.Html5writeTableRow(HWriter, (Group)G.Content[i], Level + 1);
                    }
                    else if (G.Content[i].GetType() == typeof(Result))
                    {
                        Result Res = (Result)G.Content[i];

                        // Line for title
                        HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
                        if (Level > 0)
                            this.Html5writeTableRowPrefixColumns(HWriter, Level);

                        HWriter.WriteLine();

                        if (Res.ForPostgres)
                        {

                            HWriter.AddAttribute("colspan", (this.TableColumnCount - Level).ToString());
                            HWriter.AddStyleAttribute("font-weight", "bold");
                            //HWriter.AddStyleAttribute("text-decoration", "underline");
                            HWriter.AddStyleAttribute("background-color", "white");
                            HWriter.AddStyleAttribute("color", "#336699");
                            HWriter.AddStyleAttribute("font-size", "14px");
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                            HWriter.WriteEncodedText(Res.Title);

                            HWriter.AddAttribute("title", "Postgres database on server " + DiversityWorkbench.PostgreSQL.Connection.CurrentServer().Name);
                            HWriter.AddStyleAttribute("color", "#336699");
                            HWriter.AddStyleAttribute("font-size", "14px");
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Span);
                            HWriter.Write("&nbsp;&nbsp;&#128024;");
                            HWriter.RenderEndTag(); // span
                            HWriter.WriteLine();


                            HWriter.RenderEndTag(); // Td
                            HWriter.WriteLine();
                        }
                        else
                        {
                            HWriter.AddAttribute("colspan", (this.TableColumnCount - Level).ToString());
                            HWriter.AddStyleAttribute("background-color", "white");
                            HWriter.AddStyleAttribute("color", "black");
                            HWriter.AddStyleAttribute("font-size", "14px");
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                            if (Res.Title == null)
                                Res.Title = "";
                            if (Res.Title.StartsWith(" "))
                            {
                                foreach(char C in Res.Title)
                                {
                                    if (C == ' ')
                                        HWriter.Write("&nbsp;");
                                    else
                                        break;
                                }
                            }
                            HWriter.WriteEncodedText(Res.Title);
                            if (Res.ToolTip.Length > 0)
                                this.Html5writeHelpTooltip(HWriter, Res.ToolTip);
                            HWriter.RenderEndTag(); // Td
                            HWriter.WriteLine();
                        }

                        HWriter.RenderEndTag(); // Tr
                        HWriter.WriteLine();

                        // Line for content
                        HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr); 
                        if (Level > 0)
                            this.Html5writeTableRowPrefixColumns(HWriter, Level + 1);

                        HWriter.WriteLine();

                        string BackgroundColor = "white";
                        string ForeColor = "green";
                        if (!Res.OK)
                        {
                            if (Res.ForPostgres)
                                BackgroundColor = "LightSteelBlue";
                            else
                                BackgroundColor = "Thistle";
                            ForeColor = "red";
                        }
                        HWriter.AddStyleAttribute("background-color", BackgroundColor);
                        HWriter.AddStyleAttribute("color", ForeColor);
                        HWriter.AddStyleAttribute("font-size", "12px");
                        HWriter.AddStyleAttribute("font-weight", "bold");
                        HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                        if (Res.OK)
                            HWriter.Write("&#10004;");
                        else
                            HWriter.WriteEncodedText("X");
                        HWriter.RenderEndTag(); // Td
                        HWriter.WriteLine();

                        HWriter.AddAttribute("colspan", (this.TableColumnCount - Level - 2).ToString());
                        HWriter.AddStyleAttribute("background-color", BackgroundColor);
                        HWriter.AddStyleAttribute("color", ForeColor);
                        HWriter.AddStyleAttribute("font-size", "12px");
                        HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                        HWriter.WriteEncodedText(Res.Description);
                        HWriter.RenderEndTag(); // Td
                        HWriter.WriteLine();

                        HWriter.RenderEndTag(); // Tr
                        HWriter.WriteLine();

                        if (Res.DescriptionForSelection.Length > 0 && Res.Selection.Length > 0)
                        {
                            // Line for DescriptionForSelection
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
                            if (Level > 0)
                                this.Html5writeTableRowPrefixColumns(HWriter, Level + 1);

                            HWriter.WriteLine();

                            BackgroundColor = "Khaki";
                            ForeColor = "red";
                            HWriter.AddStyleAttribute("background-color", BackgroundColor);
                            HWriter.AddStyleAttribute("color", ForeColor);
                            HWriter.AddStyleAttribute("font-size", "12px");
                            HWriter.AddStyleAttribute("font-weight", "bold");
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                            HWriter.WriteEncodedText("X");
                            HWriter.RenderEndTag(); // Td
                            HWriter.WriteLine();

                            HWriter.AddAttribute("colspan", (this.TableColumnCount - Level - 2).ToString());
                            HWriter.AddStyleAttribute("background-color", BackgroundColor);
                            HWriter.AddStyleAttribute("color", ForeColor);
                            HWriter.AddStyleAttribute("font-size", "12px");
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                            HWriter.WriteEncodedText(Res.DescriptionForSelection);
                            HWriter.RenderEndTag(); // Td
                            HWriter.WriteLine();

                            // Line for Selection
                            HWriter.RenderEndTag(); // Tr
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
                            if (Level > 0)
                                this.Html5writeTableRowPrefixColumns(HWriter, Level + 1);
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                            HWriter.AddStyleAttribute("background-color", BackgroundColor);
                            HWriter.RenderEndTag(); // Td
                            HWriter.WriteLine();

                            HWriter.AddAttribute("colspan", (this.TableColumnCount - Level - 2).ToString());
                            HWriter.AddStyleAttribute("background-color", BackgroundColor);
                            HWriter.AddStyleAttribute("color", ForeColor);
                            HWriter.AddStyleAttribute("font-size", "12px");
                            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                            HWriter.WriteEncodedText(Res.Selection);
                            HWriter.RenderEndTag(); // Td
                            HWriter.WriteLine();

                            HWriter.RenderEndTag(); // Tr
                            HWriter.WriteLine();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void Html5writeEmptyRow(System.Web.UI.HtmlTextWriter HWriter, int Height)
        {
            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
            HWriter.AddAttribute("colspan", this.TableColumnCount.ToString());
            HWriter.AddStyleAttribute("font-size", Height.ToString() + "px");
            HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
            HWriter.Write("&nbsp;");
            HWriter.RenderEndTag(); // Td
            HWriter.WriteLine();
            HWriter.RenderEndTag(); // Tr
            HWriter.WriteLine();

        }

        private void Html5writeTableRowPrefixColumns(System.Web.UI.HtmlTextWriter HWriter, int Level, bool IsHeader = false)
        {
            try
            {
                for(int i = 0; i < Level; i++)
                {
                    if (IsHeader)
                        HWriter.AddAttribute("text-align", "center");
                    HWriter.AddStyleAttribute("color", "gray");
                    HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
                    if (IsHeader)
                        HWriter.Write("&bull;");
                    HWriter.RenderEndTag(); // Td
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void Html5writeHelpTooltip(System.Web.UI.HtmlTextWriter HWriter, string HelpText)
        {
            try
            {
                HWriter.AddStyleAttribute("background-color", "white");
                HWriter.AddStyleAttribute("color", "blue");
                HWriter.AddStyleAttribute("font-size", "14px");
                //HWriter.AddStyleAttribute("text-decoration", "underline");
                HWriter.AddStyleAttribute("font-weight", "bold");
                HWriter.AddAttribute("title", HelpText);
                HWriter.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Span);
                HWriter.Write("&nbsp;&nbsp;?");
                HWriter.RenderEndTag(); // Span
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}
