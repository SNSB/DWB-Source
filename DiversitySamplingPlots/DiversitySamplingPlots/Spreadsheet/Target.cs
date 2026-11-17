using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversitySamplingPlots.Spreadsheet
{
    public class Target
    {

        public enum SheetTarget { Plot }
        private static SheetTarget _SheetTarget;
        private enum IncludedTable { WGS84, Gazetteer }

        public static DiversityWorkbench.Spreadsheet.Sheet TargetSheet(SheetTarget Target)
        {
            _SheetTarget = Target;

            DiversityWorkbench.Spreadsheet.Sheet Sheet;
            _TableAlias = null;
            switch (Target)
            {
                default:
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversitySamplingPlots: Plot", "Plots");
                    break;
            }

            Sheet.setProjectSqlSoure("SELECT Project, ProjectID FROM ProjectList ORDER BY Project");
            Sheet.setProjectReadOnlySqlSoure("SELECT P.Project, P.ProjectID " +
                "FROM dbo.ProjectProxy P WHERE NOT EXISTS (SELECT * FROM dbo.ProjectUser U WHERE U.ProjectID = P.ProjectID AND U.LoginName = USER_NAME()) " +
                "GROUP BY P.Project, P.ProjectID " +
                "ORDER BY P.Project");

            System.Collections.Generic.List<IncludedTable> Include = new List<IncludedTable>();
            Include.Add(IncludedTable.WGS84);
            DiversityWorkbench.Spreadsheet.DataTable Plot = Spreadsheet.Target.AddPlotTables(ref Sheet, Include);

            DiversitySamplingPlots.Spreadsheet.Target.AddProjectTable(ref Sheet, Plot);

            DiversityWorkbench.Spreadsheet.DataTable Image = null;

            switch (Target)
            {
                case SheetTarget.Plot:
                    Image = DiversitySamplingPlots.Spreadsheet.Target.AddImageTable(ref Sheet, Plot, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                    break;

            }

            Sheet.MasterQueryColumn = Plot.DataColumns()["PlotID"];

            return Sheet;

        }


        #region Detail tables

        private static DiversityWorkbench.Spreadsheet.DataTable AddPlotTables(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, System.Collections.Generic.List<IncludedTable> IncludedTables = null)
        {
            string Table = "SamplingPlot";
            DiversityWorkbench.Spreadsheet.DataTable Plot = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Plot", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
            try
            {
                Plot.setColorBack(TableColor(Plot.Name));
                System.Collections.Generic.List<string> Ce = new List<string>();
                Ce.Add("PlotIdentifier");
                //Event.DataColumns()["LocalityDescription"].DisplayText = "Locality";
                Plot.setDisplayedColumns(Ce);
                //Plot.DataColumns()["CollectionDate"].setReadOnly(true);
                //Plot.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
                //Plot.DataColumns()["SeriesID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT [SeriesID] as Value " +
                //",case when [DateStart] is null then '' else convert(varchar(10), [DateStart], 120) + case when [DateEnd] is null or cast([DateStart] as varchar(10)) = cast([DateEnd] as varchar(10)) then '' else ' - ' + convert(varchar(10), [DateEnd], 120) end + ': ' end + " +
                //"+ case when [SeriesCode] is null then '' else [SeriesCode] + '; ' end " +
                //"+ case when [Description] is null then '' else substring([Description], 1, 50) end AS Display " +
                //"FROM [dbo].[CollectionEventSeries] ORDER BY Display";
                Plot.AddImage("", DiversitySamplingPlots.Properties.Resources.SamplingPlot);
                Sheet.AddDataTable(Plot);

                Table = "SamplingPlotLocalisation";
                DiversityWorkbench.Spreadsheet.DataTable WGS84 = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "WGS84", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
                WGS84.setColorBack(TableColor(WGS84.Name));
                WGS84.TemplateAlias = WGS84.Alias();
                WGS84.setParentTable(Plot);
                WGS84.SetDescription("Localisation of the plot");
                if (IncludedTables != null && IncludedTables.Contains(IncludedTable.WGS84))
                {
                    System.Collections.Generic.List<string> Cw = new List<string>();
                    Cw.Add("Location1");
                    Cw.Add("Location2");
                    WGS84.setDisplayedColumns(Cw);
                }
                WGS84.DataColumns()["Location1"].DisplayText = "Long.";
                WGS84.DataColumns()["Location2"].DisplayText = "Lat.";
                WGS84.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

                for (int i = 2; i < 3/*22*/; i++)
                {
                    try
                    {
                        // ToDo
                        WGS84.AddImage(i.ToString(), DiversitySamplingPlots.Properties.Resources.Localisation);
                        //WGS84.AddImage(i.ToString(), DiversityCollection.Specimen.ImageForLocalisationSystem(i));
                    }
                    catch (System.Exception ex)
                    {
                    }
                }

                // the optional link to gazetteer
                // Values for the LocalisationSystemID
                string[] LocSysID = new string[5] { "7", "18", "19", "20", "21" };
                // the list of bindings for the gazetteer
                System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding> GazBindList = new List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding>();
                // the binding for the latitude
                DiversityWorkbench.Spreadsheet.RemoteColumnBinding GazLat = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                GazLat.Column = WGS84.DataColumns()["AverageLatitudeCache"];
                GazLat.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
                GazLat.RemoteParameter = "Latitude";
                GazBindList.Add(GazLat);
                // the binding for the longitude
                DiversityWorkbench.Spreadsheet.RemoteColumnBinding GazLong = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                GazLong.Column = WGS84.DataColumns()["AverageLongitudeCache"];
                GazLong.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
                GazLong.RemoteParameter = "Longitude";
                GazBindList.Add(GazLong);
                // the binding for the country
                DiversityWorkbench.Spreadsheet.RemoteColumnBinding GazCountry = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                GazCountry.Column = Plot.DataColumns()["CountryCache"];
                GazCountry.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.IfNotEmptyAskUser;
                GazCountry.RemoteParameter = "Country";
                GazBindList.Add(GazCountry);
                DiversityWorkbench.Spreadsheet.RemoteLink RemoteLinksGaz = new DiversityWorkbench.Spreadsheet.RemoteLink(
                    DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityGazetteer,
                    LocSysID,
                    GazBindList);

                // the optional link to SamplingPlots
                // the bindings for SamplingPlots
                System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding> PlotBind = new List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding>();
                // the list of bindings for the SamplingPlots
                System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding> PlotRemList = new List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding>();
                // the binding for the latitude
                DiversityWorkbench.Spreadsheet.RemoteColumnBinding PlotLat = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                PlotLat.Column = WGS84.DataColumns()["AverageLatitudeCache"];
                PlotLat.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
                PlotLat.RemoteParameter = "Latitude";
                PlotRemList.Add(PlotLat);
                // the binding for the latitude
                DiversityWorkbench.Spreadsheet.RemoteColumnBinding PlotLong = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                PlotLong.Column = WGS84.DataColumns()["AverageLongitudeCache"];
                PlotLong.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
                PlotLong.RemoteParameter = "Longitude";
                PlotRemList.Add(PlotLong);
                // the binding for the country
                DiversityWorkbench.Spreadsheet.RemoteColumnBinding PlotCountry = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                PlotCountry.Column = Plot.DataColumns()["CountryCache"];
                PlotCountry.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.IfNotEmptyAskUser;
                PlotCountry.RemoteParameter = "Country";

                PlotRemList.Add(PlotCountry);

                //string[] IDSP = new string[1] { "13" };
                //DiversityWorkbench.Spreadsheet.RemoteLink RemoteLinkPlot = new DiversityWorkbench.Spreadsheet.RemoteLink(
                //    DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversitySamplingPlots,
                //    IDSP,
                //    PlotRemList);

                // the optional links for column Location2
                System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteLink> OptionalLinks = new List<DiversityWorkbench.Spreadsheet.RemoteLink>();
                OptionalLinks.Add(RemoteLinksGaz);
                //OptionalLinks.Add(RemoteLinkPlot);

                WGS84.DataColumns()["Location2"].setRemoteLinks(true, "LocalisationSystemID", "Location1", OptionalLinks, DiversityWorkbench.Spreadsheet.DataColumn.LinkType.OptionalLinkToDiversityWorkbenchModule);

                System.Collections.Generic.List<string> RestWGS = new List<string>();
                RestWGS.Add("LocalisationSystemID");
                WGS84.RestrictionColumns = RestWGS;
                WGS84.DataColumns()["LocalisationSystemID"].RestrictionValue = "8";

                string SQL = "SELECT LocalisationSystemID AS Value, DisplayText AS Display " +
                    "FROM LocalisationSystem ORDER BY Display";
                WGS84.DataColumns()["LocalisationSystemID"].SqlLookupSource = SQL;
                Sheet.AddDataTable(WGS84);

                Table = "SamplingPlotResource";
                DiversityWorkbench.Spreadsheet.DataTable SamplingPlotResource = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Image of the Plot", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                SamplingPlotResource.setColorBack(TableColor(SamplingPlotResource.Name));
                SamplingPlotResource.AddImage("", DiversitySamplingPlots.Properties.Resources.Icones);
                SamplingPlotResource.SetDescription("The first image of the plot");
                SamplingPlotResource.setParentTable(Plot);
                SamplingPlotResource.DataColumns()["ResourceURI"].TypeOfLink = DiversityWorkbench.Spreadsheet.DataColumn.LinkType.Resource;
                if (Sheet.Target() == "Plot")
                {
                    System.Collections.Generic.List<string> Im = new List<string>();
                    Im.Add("ResourceURI");
                    SamplingPlotResource.setDisplayedColumns(Im);
                }
                SamplingPlotResource.SqlRestrictionClause = " exists (select * from [SamplingPlotResource] F where " + TableAlias(Table) + ".PlotID = F.PlotID " +
                    "group by F.PlotID " +
                    "having " + TableAlias(Table) + ".ResourceURI = min(f.ResourceURI)) ";
                Sheet.AddDataTable(SamplingPlotResource);

                Table = "SamplingPlotProperty";
                DiversityWorkbench.Spreadsheet.DataTable Eunis2012 = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "EUNIS 2012", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
                Eunis2012.setColorBack(TableColor(Eunis2012.Name));
                Eunis2012.TemplateAlias = Eunis2012.Alias();
                Eunis2012.setParentTable(Plot);
                Eunis2012.SetDescription("Property of the collection site");
                //System.Collections.Generic.List<string> CC = new List<string>();
                //CC.Add("DisplayText");
                //Eunis2012.setDisplayedColumns(CC);
                Eunis2012.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

                System.Collections.Generic.List<int> PropertyID = new List<int>();
                PropertyID.Add(1);
                PropertyID.Add(2);
                PropertyID.Add(10);
                PropertyID.Add(20);
                PropertyID.Add(30);
                PropertyID.Add(40);
                //PropertyID.Add(41);
                //PropertyID.Add(42);
                //PropertyID.Add(43);
                //PropertyID.Add(50);

                foreach (int i in PropertyID)
                {
                    try
                    {
                        // ToDo
                        Eunis2012.AddImage(i.ToString(), DiversitySamplingPlots.Properties.Resources.Habitat);
                        //Eunis2012.AddImage(i.ToString(), DiversityCollection.Specimen.ImageForCollectionEventProperty(i));
                    }
                    catch (System.Exception ex)
                    {
                    }
                }

                System.Collections.Generic.List<string> RestEunis = new List<string>();
                RestEunis.Add("PropertyID");
                Eunis2012.RestrictionColumns = RestEunis;
                Eunis2012.DataColumns()["PropertyID"].RestrictionValue = "2";

                SQL = "SELECT PropertyID AS Value, DisplayText AS Display " +
                    "FROM Property ORDER BY Display";
                Eunis2012.DataColumns()["PropertyID"].SqlLookupSource = SQL;
                Sheet.AddDataTable(Eunis2012);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Plot;
        }

        private static DiversityWorkbench.Spreadsheet.DataTable AddImageTable(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Parent, DiversityWorkbench.Spreadsheet.DataTable.TableType Type)
        {
            string Table = "SamplingPlotResource";
            string Title = "Plot image";
            DiversityWorkbench.Spreadsheet.DataTable Image = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, Title, Type, ref Sheet);
            Image.setParentTable(Parent);
            switch (Sheet.Target())
            {
                default:
                    Image.setColorBack(TableColor(Parent.Name));
                    Image.SetDescription("The first image of a plot");
                    break;
            }
            System.Collections.Generic.List<string> Ims = new List<string>();
            Ims.Add("ResourceURI");
            Image.DataColumns()["ResourceURI"].TypeOfLink = DiversityWorkbench.Spreadsheet.DataColumn.LinkType.Resource;
            if (Sheet.Target() != "Plot")
                Image.setDisplayedColumns(Ims);
            if (Type == DiversityWorkbench.Spreadsheet.DataTable.TableType.Single)
            {
                switch (Sheet.Target())
                {
                    default:
                        Image.SqlRestrictionClause = " exists (select * from [SamplingPlotResource] F where " + TableAlias(Table) + ".PlotID = F.PlotID " +
                            "group by F.PlotID " +
                            "having " + TableAlias(Table) + ".ResourceURI = min(f.ResourceURI)) ";
                        break;
                }
            }
            Image.AddImage("", DiversitySamplingPlots.Properties.Resources.Icones);
            Sheet.AddDataTable(Image);
            return Image;
        }

        private static void AddProjectTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Plot)
        {
            string Table = "SamplingProject";
            DiversityWorkbench.Spreadsheet.DataTable Project = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Project, ref Sheet);
            Project.setParentTable(Plot);
            Project.SqlRestrictionClause = " ProjectID IN (SELECT ProjectID FROM ProjectList) ";
            Project.AddImage("", DiversitySamplingPlots.Properties.Resources.Project);
            Sheet.AddDataTable(Project);
        }


        #endregion

        #region Common

        public static SheetTarget? StarterTarget()
        {
            string FileName = DiversityWorkbench.Spreadsheet.Setting.SpreadsheetDirectory() + "\\Start.xml";
            System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
            if (FI.Exists)
            {
                System.Xml.XmlReaderSettings xSettings = new System.Xml.XmlReaderSettings();
                System.Xml.Linq.XElement SettingsDocument = null;
                try
                {
                    xSettings.CheckCharacters = false;
                    SettingsDocument = System.Xml.Linq.XElement.Load(System.Xml.XmlReader.Create(FI.FullName, xSettings));
                    if (SettingsDocument.HasAttributes)
                    {
                        if (SettingsDocument.FirstAttribute.Name == "Target")
                        {
                            switch (SettingsDocument.FirstAttribute.Value.ToString())
                            {
                                case "Plot":
                                    return SheetTarget.Plot;
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
            return null;
        }

        public static void SetStart(SheetTarget? Target)
        {
            string FileName = DiversityWorkbench.Spreadsheet.Setting.SpreadsheetDirectory() + "\\Start.xml";
            if (Target == null)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
                if (FI.Exists)
                    FI.Delete();
            }
            else
            {
                System.Xml.XmlWriter W = null;
                try
                {
                    System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                    settings.Encoding = System.Text.Encoding.UTF8;
                    W = System.Xml.XmlWriter.Create(FileName, settings);
                    W.WriteStartDocument();
                    W.WriteStartElement("Start");
                    W.WriteAttributeString("Target", Target.ToString());
                    W.WriteEndElement();//Start
                    W.WriteEndDocument();
                    W.Flush();
                    W.Close();
                }
                catch (System.Exception ex)
                {
                }
                finally
                {
                    if (W != null)
                    {
                        W.Flush();
                        W.Close();
                    }
                }
            }
        }

        private static System.Drawing.Color TableColor(string TableName)
        {
            switch (TableName)
            {
                case "SamplingPlot":
                case "SamplingPlotResource":
                case "SamplingPlotLocalisation":
                case "SamplingPlotProperty":
                case "SamplingProject":
                default:
                    return System.Drawing.Color.White;
            }
        }

        private static System.Collections.Generic.Dictionary<string, string> _TableAlias;

        public static string TableAlias(string TableName)
        {
            string Alias = "";

            if (_TableAlias == null)
            {
                _TableAlias = new Dictionary<string, string>();
                try
                {
                    _TableAlias.Add("SamplingPlot", "B0_P");
                    _TableAlias.Add("SamplingPlotLocalisation", "B1_PL");
                    _TableAlias.Add("SamplingPlotProperty", "B2_PP");
                    _TableAlias.Add("SamplingPlotResource", "B3_PR");
                    _TableAlias.Add("SamplingProject", "B4_SP");
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            if (_TableAlias.ContainsKey(TableName))
                Alias = _TableAlias[TableName];
            else
            {

            }
            return Alias;
        }

        #endregion


    }
}
