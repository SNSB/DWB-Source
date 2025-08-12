using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.Spreadsheet
{

    public class Target
    {

        public enum SheetTarget { Organisms, Parts, Specimen, Event, TK25, WGS84, Collector, Image, Analysis, Minerals, CollectionTask }
        private static SheetTarget _SheetTarget;
        //private static SheetTarget sheetTarget
        //{
        //    get => _SheetTarget;
        //    set 
        //    {
        //        _SheetTarget = value;
        //        _TableAlias = TargetTableAlias[_SheetTarget];
        //    }
        //}
        private enum IncludedEventTable { WGS84, Gazetteer }

        public static DiversityWorkbench.Spreadsheet.Sheet TargetSheet(SheetTarget Target)
        {
            //sheetTarget = Target;
            ////_TableAlias = _TargetTableAlias[_SheetTarget];
            if (Target == SheetTarget.TK25)
            {
                return Spreadsheet.Target.TK25Sheet();
            }
            else if (Target == SheetTarget.WGS84)
            {
                return Spreadsheet.Target.WGS84Sheet();
            }
            else if (Target == SheetTarget.Minerals)
            {
                return Spreadsheet.Target.MineralSheet();
            }
#if !DEBUG
#endif
            else if (Target == SheetTarget.CollectionTask)
                return Spreadsheet.Target.CollectionTaskSheet();
            else if (Target == SheetTarget.Collector)
                return Spreadsheet.Target.CollectorSheet();

            //string SQL = "";
            DiversityWorkbench.Spreadsheet.Sheet Sheet;
            _TableAlias = null;
            // Markus 31.5.23: Explizite Zuweisung des _SheetTarget damit _TableAlias korrekt gefüllt wird
            switch (Target)
            {
                case SheetTarget.Minerals:
                    _SheetTarget = SheetTarget.Minerals;
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Minerals", "Minerals");
                    break;
                case SheetTarget.Organisms:
                    _SheetTarget = SheetTarget.Organisms;
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Organisms", "Organisms");
                    break;
                case SheetTarget.Parts:
                    _SheetTarget = SheetTarget.Parts;
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Parts", "Parts");
                    break;
                case SheetTarget.Event:
                    _SheetTarget = SheetTarget.Event;
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Event", "Event");
                    break;
                case SheetTarget.Specimen:
                    _SheetTarget = SheetTarget.Specimen;
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Specimen", "Specimen");
                    break;
                case SheetTarget.Image:
                    _SheetTarget = SheetTarget.Image;
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Image", "Image");
                    break;
                case SheetTarget.Analysis:
                    _SheetTarget = SheetTarget.Analysis;
                    return DiversityCollection.Spreadsheet.Target.AnalysisSheet();
                case SheetTarget.Collector:
                    _SheetTarget = SheetTarget.Collector;
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Collector", "Collector");
                    break;
                default:
                    Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Specimen", "Specimen");
                    break;
            }
            
            Sheet.setProjectSqlSoure("SELECT Project, ProjectID FROM ProjectListNotReadOnly ORDER BY Project");
            Sheet.setProjectReadOnlySqlSoure("SELECT P.Project, U.ProjectID " +
                "FROM dbo.ProjectUser U INNER JOIN " +
                "dbo.ProjectProxy P ON U.ProjectID = P.ProjectID " +
                "WHERE U.LoginName = USER_NAME() AND (U.[ReadOnly] = 1) " +
                "GROUP BY P.Project, U.ProjectID " +
                "ORDER BY P.Project");

            System.Collections.Generic.List<IncludedEventTable> Include = new List<IncludedEventTable>();
            Include.Add(IncludedEventTable.WGS84);
            DiversityWorkbench.Spreadsheet.DataTable Event = Spreadsheet.Target.AddEventTables(ref Sheet, Include);

            DiversityWorkbench.Spreadsheet.DataTable Specimen = Spreadsheet.Target.AddSpecimenTable(ref Sheet, Event, Target);

            DiversityCollection.Spreadsheet.Target.AddProjectTable(ref Sheet, Specimen);

            DiversityCollection.Spreadsheet.Target.AddExternalDatasourceTable(ref Sheet, Specimen);

            DiversityCollection.Spreadsheet.Target.AddAgentTable(ref Sheet, Specimen);

            DiversityWorkbench.Spreadsheet.DataTable Part = null;
            DiversityWorkbench.Spreadsheet.DataTable Unit = null;
            DiversityWorkbench.Spreadsheet.DataTable Image = null;

            switch (Target)
            {
                case SheetTarget.Minerals:
                case SheetTarget.Organisms:
                    Part = DiversityCollection.Spreadsheet.Target.AddPartTable(ref Sheet, Specimen, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                    Unit = DiversityCollection.Spreadsheet.Target.AddUnitTable(ref Sheet, Specimen, Part, DiversityWorkbench.Spreadsheet.DataTable.TableType.Target);
                    Image = DiversityCollection.Spreadsheet.Target.AddImageTable(ref Sheet, Unit, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                    break;

                case SheetTarget.Parts:
                    Part = DiversityCollection.Spreadsheet.Target.AddPartTable(ref Sheet, Specimen, DiversityWorkbench.Spreadsheet.DataTable.TableType.Target);
                    Unit = DiversityCollection.Spreadsheet.Target.AddUnitTable(ref Sheet, Specimen, Part, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                    Image = DiversityCollection.Spreadsheet.Target.AddImageTable(ref Sheet, Part, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                    break;

                case SheetTarget.Image:
                    Image = DiversityCollection.Spreadsheet.Target.AddImageTable(ref Sheet, Specimen, DiversityWorkbench.Spreadsheet.DataTable.TableType.Target);
                    Image.SetDescription("The images of a specimen, may be linked to a organism or part");
                    Part = DiversityCollection.Spreadsheet.Target.AddPartTable(ref Sheet, Specimen, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup);
                    Part.SetDescription("The first part linked to the image");
                    Part.setParentTable(Image);
                    System.Collections.Generic.Dictionary<string, string> FRimage = new Dictionary<string,string>();
                    FRimage.Add("CollectionSpecimenID", "CollectionSpecimenID");
                    FRimage.Add("SpecimenPartID", "SpecimenPartID");
                    Part.setForeignRelationsToParent("CollectionSpecimenImage", FRimage);

                    Unit = DiversityCollection.Spreadsheet.Target.AddUnitTable(ref Sheet, Specimen, Part, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup);
                    Unit.SetDescription("The first organism linked to the image");
                    FRimage.Clear();
                    FRimage.Add("CollectionSpecimenID", "CollectionSpecimenID");
                    FRimage.Add("IdentificationUnitID", "IdentificationUnitID");
                    Unit.setForeignRelationsToParent("CollectionSpecimenImage", FRimage);
                    Unit.setParentTable(Image);

                    //Part = DiversityCollection.Spreadsheet.Target.AddPartTable(ref Sheet, Image, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup);
                    //Unit = DiversityCollection.Spreadsheet.Target.AddUnitTable(ref Sheet, Image, Part, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup);
                    break;

                case SheetTarget.Event:
                    Image = DiversityCollection.Spreadsheet.Target.AddImageTable(ref Sheet, Specimen, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                    Part = DiversityCollection.Spreadsheet.Target.AddPartTable(ref Sheet, Specimen, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                    Unit = DiversityCollection.Spreadsheet.Target.AddUnitTable(ref Sheet, Specimen, Part, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                    break;

                case SheetTarget.Collector:
                    //Collector = DiversityCollection.Spreadsheet.Target.AddAgentTable(ref Sheet, )
                    break;
            }


            DiversityCollection.Spreadsheet.Target.AddIdentificationTable(ref Sheet, Unit);


            DiversityCollection.Spreadsheet.Target.AddAnalysisTable(ref Sheet, Unit);


            DiversityCollection.Spreadsheet.Target.AddGeoAnalysisTable(ref Sheet, Unit);


            return Sheet;

        }

#region Administrative tables

        private static DiversityWorkbench.Spreadsheet.Sheet AnalysisSheet()
        {
            DiversityWorkbench.Spreadsheet.Sheet Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Analysis", "Analysis");
            // Analysis
            DiversityWorkbench.Spreadsheet.DataTable Analysis = null;
            string Table = "Analysis";
            Analysis = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
            System.Collections.Generic.List<string> Cs = new List<string>();
            Cs.Add("DisplayText");
            Analysis.setDisplayedColumns(Cs);
            Analysis.AddImage("", DiversityCollection.Resource.Analysis);
            Analysis.setColorBack(TableColor(Analysis.Name));
            Sheet.AddDataTable(Analysis);
            // Project
            Table = "ProjectAnalysis";
            DiversityWorkbench.Spreadsheet.DataTable Project = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Project, ref Sheet);
            Project.setParentTable(Analysis);
            Project.SqlRestrictionClause = " ProjectID IN (SELECT ProjectID FROM ProjectListNotReadOnly) ";
            Project.AddImage("", DiversityCollection.Resource.Project);
            Sheet.AddDataTable(Project);
            // Result
            Table = "AnalysisResult";
            DiversityWorkbench.Spreadsheet.DataTable Result = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
            Result.setParentTable(Analysis);
            Result.AddImage("", DiversityCollection.Resource.AnalysisHierarchy);
            Sheet.AddDataTable(Result);

            Sheet.setProjectSqlSoure("SELECT DISTINCT ProjectListNotReadOnly.Project, ProjectListNotReadOnly.ProjectID " +
                "FROM ProjectAnalysis INNER JOIN " +
                "ProjectListNotReadOnly ON ProjectAnalysis.ProjectID = ProjectListNotReadOnly.ProjectID " +
                "ORDER BY ProjectListNotReadOnly.Project");

            Sheet.MasterQueryColumn = Analysis.DataColumns()["AnalysisID"];

            return Sheet;
        }
        
#endregion

#region Detail tables

        private static DiversityWorkbench.Spreadsheet.DataTable AddEventTables(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, System.Collections.Generic.List<IncludedEventTable> IncludedTables = null)
        {
            string Table = "CollectionEvent";
            DiversityWorkbench.Spreadsheet.DataTable Event = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Event", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
            try
            {
                Event.setColorBack(TableColor(Event.Name));
                System.Collections.Generic.List<string> Ce = new List<string>();
                Ce.Add("LocalityDescription");
                //Event.DataColumns()["LocalityDescription"].DisplayText = "Locality";
                Event.setDisplayedColumns(Ce);
                Event.DataColumns()["CollectionDate"].setReadOnly(true);
                Event.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
                Event.DataColumns()["SeriesID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT [SeriesID] as Value " +
                ",case when [DateStart] is null then '' else convert(varchar(10), [DateStart], 120) + case when [DateEnd] is null or cast([DateStart] as varchar(10)) = cast([DateEnd] as varchar(10)) then '' else ' - ' + convert(varchar(10), [DateEnd], 120) end + ': ' end + " +
                "+ case when [SeriesCode] is null then '' else [SeriesCode] + '; ' end " +
                "+ case when [Description] is null then '' else substring([Description], 1, 50) end AS Display " +
                "FROM [dbo].[CollectionEventSeries] ORDER BY Display";
                Event.AddImage("", DiversityCollection.Resource.Event);
                Sheet.AddDataTable(Event);

                Table = "CollectionEventLocalisation";
                DiversityWorkbench.Spreadsheet.DataTable WGS84 = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "WGS84", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
                WGS84.setColorBack(TableColor(WGS84.Name));
                WGS84.TemplateAlias = WGS84.Alias();
                WGS84.setParentTable(Event);
                WGS84.SetDescription("Localisation of the event");
                if (IncludedTables != null && IncludedTables.Contains(IncludedEventTable.WGS84))
                {
                    System.Collections.Generic.List<string> Cw = new List<string>();
                    Cw.Add("Location1");
                    Cw.Add("Location2");
                    WGS84.setDisplayedColumns(Cw);
                }
                WGS84.DataColumns()["Location1"].DisplayText = "Long.";
                WGS84.DataColumns()["Location2"].DisplayText = "Lat.";
                WGS84.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

                for (int i = 2; i < 22; i++)
                {
                    try
                    {
                        WGS84.AddImage(i.ToString(), DiversityCollection.Specimen.ImageForLocalisationSystem(i));
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
                GazCountry.Column = Event.DataColumns()["CountryCache"];
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
                PlotCountry.Column = Event.DataColumns()["CountryCache"];
                PlotCountry.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.IfNotEmptyAskUser;
                PlotCountry.RemoteParameter = "Country";

                PlotRemList.Add(PlotCountry);

                string[] IDSP = new string[1] { "13" };
                DiversityWorkbench.Spreadsheet.RemoteLink RemoteLinkPlot = new DiversityWorkbench.Spreadsheet.RemoteLink(
                    DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversitySamplingPlots,
                    IDSP,
                    PlotRemList);

                // the optional links for column Location2
                System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteLink> OptionalLinks = new List<DiversityWorkbench.Spreadsheet.RemoteLink>();
                OptionalLinks.Add(RemoteLinksGaz);
                OptionalLinks.Add(RemoteLinkPlot);

                WGS84.DataColumns()["Location2"].setRemoteLinks(true, "LocalisationSystemID", "Location1", OptionalLinks, DiversityWorkbench.Spreadsheet.DataColumn.LinkType.OptionalLinkToDiversityWorkbenchModule);

                System.Collections.Generic.List<string> RestWGS = new List<string>();
                RestWGS.Add("LocalisationSystemID");
                WGS84.RestrictionColumns = RestWGS;
                WGS84.DataColumns()["LocalisationSystemID"].RestrictionValue = "8";

                string SQL = "SELECT LocalisationSystemID AS Value, DisplayText AS Display " +
                    "FROM LocalisationSystem ORDER BY Display";
                WGS84.DataColumns()["LocalisationSystemID"].SqlLookupSource = SQL;
                Sheet.AddDataTable(WGS84);

                Table = "CollectionEventImage";
                DiversityWorkbench.Spreadsheet.DataTable EventImage = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Image of the Event", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                EventImage.setColorBack(TableColor(EventImage.Name));
                EventImage.AddImage("", DiversityCollection.Resource.EventImage);
                EventImage.SetDescription("The first image of the collection event");
                EventImage.setParentTable(Event);
                EventImage.DataColumns()["URI"].TypeOfLink = DiversityWorkbench.Spreadsheet.DataColumn.LinkType.Resource;
                if (Sheet.Target() == "Event")
                {
                    System.Collections.Generic.List<string> Im = new List<string>();
                    Im.Add("URI");
                    EventImage.setDisplayedColumns(Im);
                }
                EventImage.SqlRestrictionClause = " exists (select * from [CollectionEventImage] F where " + TableAlias(Table) + ".CollectionEventID = F.CollectionEventID " +
                    "group by F.CollectionEventID " +
                    "having " + TableAlias(Table) + ".URI = min(f.URI)) ";
                Sheet.AddDataTable(EventImage);

                Table = "CollectionEventProperty";
                DiversityWorkbench.Spreadsheet.DataTable Eunis2012 = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "EUNIS 2012", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
                Eunis2012.setColorBack(TableColor(Eunis2012.Name));
                Eunis2012.TemplateAlias = Eunis2012.Alias();
                Eunis2012.setParentTable(Event);
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
                PropertyID.Add(41);
                PropertyID.Add(42);
                PropertyID.Add(43);
                PropertyID.Add(50);

                foreach (int i in PropertyID)
                {
                    try
                    {
                        Eunis2012.AddImage(i.ToString(), DiversityCollection.Specimen.ImageForCollectionEventProperty(i));
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

                Table = "CollectionEventMethod";
                DiversityWorkbench.Spreadsheet.DataTable CollectionEventMethod = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Method of the Event", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
                CollectionEventMethod.setColorBack(TableColor(CollectionEventMethod.Name));
                SQL = "SELECT MethodID AS Value, DisplayText AS Display " +
                   "FROM Method ORDER BY DisplayText";
                CollectionEventMethod.DataColumns()["MethodID"].SqlLookupSource = SQL;
                CollectionEventMethod.AddImage("", DiversityCollection.Resource.Tools);
                CollectionEventMethod.SetDescription("The method of the collection event");
                CollectionEventMethod.setParentTable(Event);
                CollectionEventMethod.TemplateAlias = CollectionEventMethod.Alias();
                Sheet.AddDataTable(CollectionEventMethod);

                Table = "Method";
                DiversityWorkbench.Spreadsheet.DataTable Method = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Method", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
                Method.SetDescription("The methods available for events");
                Method.setParentTable(CollectionEventMethod);
                Method.setColorBack(TableColor(Method.Name));
                Method.AddImage("", DiversityCollection.Resource.Tools);
                Method.TemplateAlias = Method.Alias();
                Sheet.AddDataTable(Method);


                Table = "CollectionEventParameterValue";
                DiversityWorkbench.Spreadsheet.DataTable CollectionEventParameterValue = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Method parameters of the Event", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
                CollectionEventParameterValue.setColorBack(TableColor(CollectionEventParameterValue.Name));
                CollectionEventParameterValue.AddImage("", DiversityCollection.Resource.Parameter);
                CollectionEventParameterValue.SetDescription("The method parameters of the collection event");
                CollectionEventParameterValue.setParentTable(CollectionEventMethod);
                CollectionEventParameterValue.TemplateAlias = CollectionEventParameterValue.Alias();
                Sheet.AddDataTable(CollectionEventParameterValue);


            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Event;
        }

        private static DiversityWorkbench.Spreadsheet.DataTable AddSpecimenTable(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Event, SheetTarget Target)
        {
            string Table = "CollectionSpecimen";
            DiversityWorkbench.Spreadsheet.DataTable Specimen = null;
            try
            {
                if (Target == SheetTarget.Event)
                {
                    Specimen = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Specimen", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                    Specimen.SqlRestrictionClause = " exists " +
                        " (select * from [CollectionSpecimen] s " +
                        "where " + Specimen.Alias() + ".CollectionEventID = s.CollectionEventID  " +
                        "group by s.CollectionEventID " +
                        "having " + Specimen.Alias() + ".CollectionSpecimenID = min(s.CollectionSpecimenID)) ";
                    Specimen.IsRequired = true;
                }
                else
                    Specimen = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Specimen", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
                Specimen.setParentTable(Event);
                Specimen.setColorBack(TableColor(Specimen.Name));
                System.Collections.Generic.List<string> Cs = new List<string>();
                Cs.Add("AccessionNumber");
                Specimen.setDisplayedColumns(Cs);
                Specimen.DataColumns()["ExternalDatasourceID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT ExternalDatasourceID AS Value, ExternalDatasourceName + CASE WHEN [ExternalDatasourceVersion] IS NULL THEN '' ELSE '(' + [ExternalDatasourceVersion] + ')' END AS Display " +
                    "FROM CollectionExternalDatasource ORDER BY Display ";
                Specimen.DataColumns()["CollectionEventID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT DISTINCT E.[CollectionEventID] AS Value " +
                ",case when [CollectionYear] is null then '' else cast([CollectionYear] as varchar) + case when [CollectionMonth] is null then '' else '-' + cast([CollectionMonth] as varchar) end + case when [CollectionDay] is null then '' else '-' + cast([CollectionDay] as varchar) end + ': ' end " +
                "+ case when [CollectorsEventNumber] is null then '' else [CollectorsEventNumber] + '; ' end " +
                "+ case when [LocalityDescription] is null then '' else substring([LocalityDescription], 1, 200)  end AS Display " +
                "FROM [dbo].[CollectionEvent] E , CollectionSpecimen S, CollectionSpecimenID_Available P " +
                "WHERE E.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = P.CollectionSpecimenID ORDER BY Display";
                Specimen.DataColumns()["DepositorsAgentURI"].setRemoteLinks("DepositorsName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                Specimen.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
                Specimen.DataColumns()["ReferenceURI"].IsOutdated = true;
                Specimen.DataColumns()["ReferenceTitle"].IsOutdated = true;
                Specimen.DataColumns()["ReferenceDetails"].IsOutdated = true;
                Specimen.AddImage("", DiversityCollection.Resource.CollectionSpecimen);
                Sheet.AddDataTable(Specimen);

                Sheet.MasterQueryColumn = Specimen.DataColumns()["CollectionSpecimenID"];
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Specimen;
        }

        private static DiversityWorkbench.Spreadsheet.DataTable AddImageTable(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Parent, DiversityWorkbench.Spreadsheet.DataTable.TableType Type)
        {
            string Table = "CollectionSpecimenImage";
            string Title = "Specimen image";
            switch (Sheet.Target())
            {
                case "Parts":
                    Title = "Part image";
                    break;
                case "Organisms":
                    Title = "Unit image";
                    break;
                case "Minerals":
                    Title = "Mineral image";
                    break;
            }
            DiversityWorkbench.Spreadsheet.DataTable Image = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, Title, Type, ref Sheet);
            Image.setParentTable(Parent);
            switch (Sheet.Target())
            {
                case "Parts":
                    Image.setColorBack(TableColor("CollectionSpecimenPart"));
                    Image.SetDescription("The first image of the part within a specimen");
                    break;
                case "Organisms":
                    Image.setColorBack(TableColor("IdentificationUnit"));
                    Image.SetDescription("The first image of the organism within a specimen");
                    break;
                case "Minerals":
                    Image.setColorBack(TableColor("IdentificationUnit"));
                    Image.SetDescription("The first image of the mineral within a specimen");
                    break;
                default:
                    Image.setColorBack(TableColor(Parent.Name));
                    Image.SetDescription("The first image of a specimen");
                    break;
            }
            System.Collections.Generic.List<string> Ims = new List<string>();
            Ims.Add("URI");
            Image.DataColumns()["URI"].TypeOfLink = DiversityWorkbench.Spreadsheet.DataColumn.LinkType.Resource;
            Image.DataColumns()["CreatorAgentURI"].setRemoteLinks("CreatorAgent", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Image.DataColumns()["LicenseHolderAgentURI"].setRemoteLinks("LicenseHolder", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            if (Sheet.Target() != "Event")
                Image.setDisplayedColumns(Ims);
            if (Type == DiversityWorkbench.Spreadsheet.DataTable.TableType.Single)
            {
                //if (_TableAlias == null && _TargetTableAlias.ContainsKey(_SheetTarget))
                //    _TableAlias = _TargetTableAlias[_SheetTarget];
                //string alias = TableAlias(Table);
                switch (Sheet.Target())
                {
                    case "Parts":
                        Image.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + _TableAlias["CollectionSpecimenPart"] + ".SpecimenPartID = F.SpecimenPartID " +
                            " and " + TableAlias(Table) + ".SpecimenPartID = F.SpecimenPartID " +
                            "group by F.SpecimenPartID " +
                            "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".SpecimenPartID = min(f.SpecimenPartID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
                        break;
                    case "Minerals":
                    case "Organisms":
                        Image.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + _TableAlias["IdentificationUnit"] + ".IdentificationUnitID = F.IdentificationUnitID " +
                            " and " + TableAlias(Table) + ".IdentificationUnitID = F.IdentificationUnitID " +
                            "group by F.IdentificationUnitID " +
                            "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".IdentificationUnitID = min(f.IdentificationUnitID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
                        break;
                    default:
                        Image.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + TableAlias(Table) + ".CollectionSpecimenID = F.CollectionSpecimenID " +
                            "group by F.CollectionSpecimenID " +
                            "having " + TableAlias(Table) + ".URI = min(f.URI)) ";
                        break;
                }
            }
            Image.AddImage("", DiversityCollection.Resource.Icones);
            Sheet.AddDataTable(Image);
            return Image;
        }

#region Unit related tables

        private static DiversityWorkbench.Spreadsheet.DataTable AddUnitTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen,
            DiversityWorkbench.Spreadsheet.DataTable Part,
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type)
        {
            string Table = "IdentificationUnit";
            DiversityWorkbench.Spreadsheet.DataTable Unit = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Unit", Type, ref Sheet);
            switch (_SheetTarget)
            {
                case SheetTarget.Event:
                    Unit.SetDescription("The first organism collected with the event that is as least present in one collected sample");
                    Unit.SqlRestrictionClause = " exists(select * from [IdentificationUnit] F, [IdentificationUnitInPart] UP " +
                        "where F.CollectionSpecimenID = " + TableAlias(Table) + ".CollectionSpecimenID " +
                        "AND UP.CollectionSpecimenID = F.CollectionSpecimenID AND UP.IdentificationUnitID = F.IdentificationUnitID " +
                        "group by F.CollectionSpecimenID " +
                        "having " + TableAlias(Table) + ".IdentificationUnitID = min(F.IdentificationUnitID)) ";
                    break;
                case SheetTarget.Organisms:
                    //Unit.SqlRestrictionClause = " exists(select * from [IdentificationUnit] F, [IdentificationUnitInPart] UP " +
                    //    "where F.CollectionSpecimenID = " + TableAlias(Table) + ".CollectionSpecimenID " +
                    //    "AND UP.CollectionSpecimenID = F.CollectionSpecimenID AND UP.IdentificationUnitID = F.IdentificationUnitID " +
                    //    "group by F.CollectionSpecimenID " +
                    //    "having " + TableAlias(Table) + ".IdentificationUnitID = min(F.IdentificationUnitID)) ";
                    break;
                case SheetTarget.Parts:
                    Unit.SetDescription("The first organism contained in the specimen part");
                    Unit.SqlRestrictionClause = " exists(select * from [IdentificationUnit] F, [IdentificationUnitInPart] UP " +
                        "where F.CollectionSpecimenID = " + TableAlias(Table) + ".CollectionSpecimenID " +
                        "AND UP.CollectionSpecimenID = F.CollectionSpecimenID AND UP.IdentificationUnitID = F.IdentificationUnitID " +
                        "AND UP.SpecimenPartID = " + Part.Alias() + ".SpecimenPartID " +
                        "group by F.CollectionSpecimenID " +
                        "having " + TableAlias(Table) + ".IdentificationUnitID = min(F.IdentificationUnitID)) ";
                    break;
                case SheetTarget.Image:
                    Unit.SetDescription("The first organism contained in the image");
                    Unit.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + _TableAlias["IdentificationUnit"] + ".IdentificationUnitID = F.IdentificationUnitID " +
                            " and " + TableAlias(Table) + ".IdentificationUnitID = F.IdentificationUnitID " +
                            "group by F.IdentificationUnitID " +
                            "having " + TableAlias(Table) + ".IdentificationUnitID = min(f.IdentificationUnitID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";

                    //switch (Sheet.Target())
                    //{
                    //    case "Image":
                    //        Part.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + _TableAlias["CollectionSpecimenPart"] + ".SpecimenPartID = F.SpecimenPartID " +
                    //            " and " + TableAlias(Table) + ".SpecimenPartID = F.SpecimenPartID " +
                    //            "group by F.SpecimenPartID " +
                    //            "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".SpecimenPartID = min(f.SpecimenPartID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
                    //        break;
                    //    //case "Organisms":
                    //    //    Part.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + _TableAlias["IdentificationUnit"] + ".IdentificationUnitID = F.IdentificationUnitID " +
                    //    //        " and " + TableAlias(Table) + ".IdentificationUnitID = F.IdentificationUnitID " +
                    //    //        "group by F.IdentificationUnitID " +
                    //    //        "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".IdentificationUnitID = min(f.IdentificationUnitID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
                    //    //    break;
                    //    default:
                    //        Part.SqlRestrictionClause = " exists(select * from [CollectionSpecimenPart] F where F.CollectionSpecimenID = " + TableAlias(Table) + ".CollectionSpecimenID " +
                    //            "group by F.CollectionSpecimenID " +
                    //            "having " + TableAlias(Table) + ".SpecimenPartID = min(F.SpecimenPartID)) ";
                    //        break;
                    //}

                    break;



            }
            Unit.setParentTable(Specimen);
            Unit.setColorBack(TableColor(Unit.Name));
            System.Collections.Generic.List<string> Us = new List<string>();
            Us.Add("TaxonomicGroup");
            Unit.setDisplayedColumns(Us);
            Unit.DataColumns()["LastIdentificationCache"].DefaultForAdding = "?";
            Unit.DataColumns()["LastIdentificationCache"].setReadOnly(true);
            Unit.AddImage("", DiversityCollection.Resource.Plant);
            Sheet.AddDataTable(Unit);

            return Unit;
        }

        private static void AddIdentificationTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "Identification";
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type = DiversityWorkbench.Spreadsheet.DataTable.TableType.Single;
            if (Sheet.Target() == "Image")
                Type = DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup;
            DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, DiversityCollection.Spreadsheet.TargetText.Last_ident, Type, ref Sheet);
            Ident.SetDescription("The first identification of the organism");
            Ident.setParentTable(Unit);
            Ident.setColorBack(TableColor(Ident.Name));
            System.Collections.Generic.List<string> Is = new List<string>();
            Is.Add("TaxonomicName");
            Is.Add("NameURI");
            Ident.setDisplayedColumns(Is);
            Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
            //Ident.View = "Identification_Last";
            //Ident.DataColumns()["NameURI"].DisplayText = "URI";
            Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
                "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                "having " + TableAlias(Table) + ".IdentificationSequence = max(L.IdentificationSequence)) ";

            // binding for DiversityTaxonNames
            DiversityWorkbench.Spreadsheet.RemoteColumnBinding FamilyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            FamilyCache.Column = Unit.DataColumns()["FamilyCache"];
            FamilyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            FamilyCache.RemoteParameter = "Family";

            DiversityWorkbench.Spreadsheet.RemoteColumnBinding OrderCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            OrderCache.Column = Unit.DataColumns()["OrderCache"];
            OrderCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            OrderCache.RemoteParameter = "Order";

            DiversityWorkbench.Spreadsheet.RemoteColumnBinding HierarchyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            HierarchyCache.Column = Unit.DataColumns()["HierarchyCache"];
            HierarchyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            HierarchyCache.RemoteParameter = "Hierarchy";

            System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding> TaxBindList = new List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding>();
            TaxBindList.Add(FamilyCache);
            TaxBindList.Add(OrderCache);
            TaxBindList.Add(HierarchyCache);
            System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteLink> TaxRemLinkList = new List<DiversityWorkbench.Spreadsheet.RemoteLink>();
            DiversityWorkbench.Spreadsheet.RemoteLink RemoteLinkTaxon = new DiversityWorkbench.Spreadsheet.RemoteLink(
                DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames,
                null,
                TaxBindList);
            TaxRemLinkList.Add(RemoteLinkTaxon);
            Ident.DataColumns()["NameURI"].setRemoteLinks("TaxonomicName", TaxRemLinkList);

            Ident.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            if (Specimen.DefaultUseIdentificationResponsible)
            {
                if (Specimen.DefaultUseCurrentUserAsDefault)
                {
                    Ident.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                    Ident.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                }
                else
                {
                    Ident.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                    Ident.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                }
            }

            Ident.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
            Ident.DataColumns()["ReferenceURI"].IsOutdated = true;
            Ident.DataColumns()["ReferenceTitle"].IsOutdated = true;
            Ident.DataColumns()["ReferenceDetails"].IsOutdated = true;

            //Ident.DataColumns()["TaxonomicName"].DisplayText = "Taxon";
            Ident.DataColumns()["TaxonomicName"].DefaultForAdding = "?";
            Ident.DataColumns()["IdentificationSequence"].SqlQueryForDefaultForAdding = "SELECT case when MAX(T.IdentificationSequence) is null then 1 else MAX(T.IdentificationSequence) + 1 end FROM Identification AS T ";
            Ident.AddImage("", DiversityCollection.Resource.Identification);
            Sheet.AddDataTable(Ident);

            //DiversityWorkbench.Spreadsheet.DataTable Ident1 = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table) + "_1", Table, DiversityCollection.Spreadsheet.TargetText.First_ident, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            //Ident1.setParentTable(Unit);
            //Ident1.setColorBack(TableColor(Ident1.Name));
            //Ident1.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where " + TableAlias(Table) + "_1.CollectionSpecimenID = L.CollectionSpecimenID " +
            //    "and " + TableAlias(Table) + "_1.IdentificationUnitID = L.IdentificationUnitID " +
            //    "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
            //    "having " + TableAlias(Table) + "_1.IdentificationSequence = min(L.IdentificationSequence) AND COUNT(L.IdentificationSequence) > 1)  ";
            //Ident1.DataColumns()["IdentificationSequence"].FilterExclude = true;
            //Ident1.DataColumns()["NameURI"].setRemoteLinks("TaxonomicName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames);

            //Ident1.AddImage("", DiversityCollection.Resource.Identification);
            //Sheet.AddDataTable(Ident1);

        }

        private static void AddAnalysisTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "IdentificationUnitAnalysis";
            // Markus 15.1.2019 -nur single, sonst passen Einschraenkungen nicht - geht evtl. doch ...
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type = DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel;
            if (Sheet.Target() == "Image")
                Type = DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup;
            DiversityWorkbench.Spreadsheet.DataTable Ana = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Analysis", Type, ref Sheet);
            Ana.setColorBack(TableColor(Ana.Name));
            Ana.TemplateAlias = Ana.Alias();
            System.Collections.Generic.List<string> RestAna = new List<string>();
            RestAna.Add("AnalysisID");
            RestAna.Add("AnalysisNumber");
            Ana.RestrictionColumns = RestAna;
            string SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                "FROM Analysis ORDER BY DisplayText";
            Ana.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
            Ana.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
            Ana.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

            if (Specimen.DefaultUseAnalyisisResponsible)
            {
                if (Specimen.DefaultUseCurrentUserAsDefault)
                {
                    Ana.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                    Ana.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                }
                else
                {
                    Ana.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                    Ana.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                }
            }

            Ana.setParentTable(Unit);
            Ana.AddImage("", DiversityCollection.Resource.Analysis);
            Ana.SetDescription("Analysis of the organism");
            Sheet.AddDataTable(Ana);

            // Markus 15.1.2019 -nur single, sonst passen Einschraenkungen nicht - geht evtl. doch ...
            DiversityWorkbench.Spreadsheet.DataTable AnaLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table) + "_Z", Table, "Last analysis", Type, ref Sheet);
            AnaLast.setColorBack(TableColor(AnaLast.Name));
            AnaLast.TemplateAlias = AnaLast.Alias();
            System.Collections.Generic.List<string> RestAnaLast = new List<string>();
            RestAnaLast.Add("AnalysisID");
            AnaLast.RestrictionColumns = RestAnaLast;
            SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                "FROM Analysis ORDER BY DisplayText";
            AnaLast.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
            AnaLast.SqlRestrictionClause = " exists (select * from [dbo].[IdentificationUnitAnalysis] L where " + TableAlias(Table) + "_Z.CollectionSpecimenID = L.CollectionSpecimenID " +
                "and " + TableAlias(Table) + "_Z.IdentificationUnitID = L.IdentificationUnitID " +
                "and " + TableAlias(Table) + "_Z.AnalysisID = L.AnalysisID " +
                "group by L.CollectionSpecimenID, L.IdentificationUnitID, L.AnalysisID " +
                "having " + TableAlias(Table) + "_Z.AnalysisNumber = max(L.AnalysisNumber))  ";
            
            AnaLast.DataColumns()["AnalysisNumber"].DisplayText = "LastAnalysisNumber";
            AnaLast.DataColumns()["AnalysisNumber"].DisplayText = "LastAnalysisNumber";
            AnaLast.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
            AnaLast.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            SQL = "SELECT MIN(DisplayText) FROM Analysis";
            AnaLast.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            AnaLast.setParentTable(Unit);
            AnaLast.SetDescription("Last analysis of the organism");
            AnaLast.AddImage("", DiversityCollection.Resource.AnalysisHierarchy);
            Sheet.AddDataTable(AnaLast);

            Table = "IdentificationUnitAnalysisMethod";
            DiversityWorkbench.Spreadsheet.DataTable AnaMet = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Method", Type, ref Sheet);
            AnaMet.setColorBack(TableColor(AnaMet.Name));
            AnaMet.TemplateAlias = AnaMet.Alias();
            System.Collections.Generic.List<string> RestAnaMet = new List<string>();
            RestAnaMet.Add("MethodID");
            RestAnaMet.Add("MethodMarker");
            RestAnaMet.Add("AnalysisID");
            RestAnaMet.Add("AnalysisNumber");
            AnaMet.RestrictionColumns = RestAnaMet;
            SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                "FROM Analysis ORDER BY DisplayText";
            AnaMet.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
            SQL = "SELECT MethodID AS Value, DisplayText AS Display " +
                "FROM Method ORDER BY DisplayText";
            AnaMet.DataColumns()["MethodID"].SqlLookupSource = SQL;
            AnaMet.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
            AnaMet.setParentTable(Ana);
            AnaMet.SetDescription("Method used for the analysis");
            AnaMet.AddImage("", DiversityCollection.Resource.Tools);
            Sheet.AddDataTable(AnaMet);

            Table = "IdentificationUnitAnalysisMethodParameter";
            DiversityWorkbench.Spreadsheet.DataTable AnaMetPar = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Parameter", Type, ref Sheet);
            AnaMetPar.setColorBack(TableColor(AnaMetPar.Name));
            AnaMetPar.TemplateAlias = AnaMetPar.Alias();
            System.Collections.Generic.List<string> RestAnaMetPar = new List<string>();
            RestAnaMetPar.Add("MethodID");
            RestAnaMetPar.Add("MethodMarker");
            RestAnaMetPar.Add("AnalysisID");
            RestAnaMetPar.Add("AnalysisNumber");
            RestAnaMetPar.Add("ParameterID");
            AnaMetPar.RestrictionColumns = RestAnaMetPar;
            SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                "FROM Analysis ORDER BY DisplayText";
            AnaMetPar.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
            SQL = "SELECT MethodID AS Value, DisplayText AS Display " +
                "FROM Method ORDER BY DisplayText";
            AnaMetPar.DataColumns()["MethodID"].SqlLookupSource = SQL;
            SQL = "SELECT ParameterID AS Value, M.DisplayText + ': ' + P.DisplayText AS Display " +
                "FROM Parameter P, Method M WHERE P.MethodID = P.MethodID ORDER BY M.DisplayText, P.DisplayText";
            AnaMetPar.DataColumns()["ParameterID"].SqlLookupSource = SQL;
            AnaMetPar.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
            AnaMetPar.setParentTable(AnaMet);
            AnaMetPar.SetDescription("Parameter of the method for analysis");
            AnaMetPar.AddImage("", DiversityCollection.Resource.Parameter);
            Sheet.AddDataTable(AnaMetPar);

        }

        private static void AddGeoAnalysisTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "IdentificationUnitGeoAnalysis";
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type = DiversityWorkbench.Spreadsheet.DataTable.TableType.Single;
            if (Sheet.Target() == "Image")
                Type = DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup;
            DiversityWorkbench.Spreadsheet.DataTable AnaGeo = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Geography", Type, ref Sheet);
            AnaGeo.SetDescription("The latest geographic position of the organism");
            AnaGeo.setColorBack(TableColor(AnaGeo.Name));
            AnaGeo.TemplateAlias = AnaGeo.Alias();
            AnaGeo.setParentTable(Unit);
            AnaGeo.AddImage("", DiversityCollection.Resource.GeoAnalysis);
            AnaGeo.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            AnaGeo.SqlRestrictionClause = " exists (select * from [dbo].[IdentificationUnitGeoAnalysis] L where " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                "group by L.IdentificationUnitID " +
                "having " + TableAlias(Table) + ".AnalysisDate = max(L.AnalysisDate)) ";
            Sheet.AddDataTable(AnaGeo);

        }

#endregion

#region Part related tables

        private static DiversityWorkbench.Spreadsheet.DataTable AddPartTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen,
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type)
        {
            string Table = "CollectionSpecimenPart";
            DiversityWorkbench.Spreadsheet.DataTable Part = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Part", Type, ref Sheet);
            switch (Type)
            {
                case DiversityWorkbench.Spreadsheet.DataTable.TableType.Single:
                case DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup:
                    switch (Sheet.Target())
                    {
                        case "Image":
                            Part.SetDescription("The first part contained in the image");
                            Part.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F " +
                                "where " + TableAlias(Table) + ".SpecimenPartID = F.SpecimenPartID " +
                                "group by F.SpecimenPartID " +
                                "having " + TableAlias(Table) + ".SpecimenPartID = min(f.SpecimenPartID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
                            break;
                        //case "Organisms":
                        //    Part.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + _TableAlias["IdentificationUnit"] + ".IdentificationUnitID = F.IdentificationUnitID " +
                        //        " and " + TableAlias(Table) + ".IdentificationUnitID = F.IdentificationUnitID " +
                        //        "group by F.IdentificationUnitID " +
                        //        "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".IdentificationUnitID = min(f.IdentificationUnitID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
                        //    break;
                        default:
                            Part.SetDescription("The first part of the specimen");
                            Part.SqlRestrictionClause = " exists(select * from [CollectionSpecimenPart] F where F.CollectionSpecimenID = " + TableAlias(Table) + ".CollectionSpecimenID " +
                                "group by F.CollectionSpecimenID " +
                                "having " + TableAlias(Table) + ".SpecimenPartID = min(F.SpecimenPartID)) ";
                            break;
                    }



                    //Part.SqlRestrictionClause = " exists(select * from [CollectionSpecimenPart] F where F.CollectionSpecimenID = " + TableAlias(Table) + ".CollectionSpecimenID " +
                    //    "group by F.CollectionSpecimenID " +
                    //    "having " + TableAlias(Table) + ".SpecimenPartID = min(F.SpecimenPartID)) ";
                    break;
                case DiversityWorkbench.Spreadsheet.DataTable.TableType.Target:
                    break;
            }
            Part.setParentTable(Specimen);
            Part.setColorBack(TableColor(Part.Name));
            System.Collections.Generic.List<string> Cp = new List<string>();
            Cp.Add("StorageLocation");
            //Cp.Add("CollectionID");
            Part.setDisplayedColumns(Cp);
            //Part.DataColumns()["StorageLocation"].DisplayText = "Stor.Loc.";

            string SQL = "SELECT NULL AS Value, NULL AS Display  " +
                "UNION " +
                "SELECT CollectionID AS Value, CollectionName AS Display " +
                "FROM Collection ORDER BY Display";
            Part.DataColumns()["CollectionID"].SqlLookupSource = SQL;
            Part.DataColumns()["DerivedFromSpecimenPartID"].InternalRelationDisplay = "LTRIM(CASE WHEN StorageLocation IS NULL THEN '' ELSE StorageLocation END + CASE WHEN AccessionNumber <> '' THEN ' Nr.: ' + AccessionNumber ELSE '' END)";
            Part.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Part.AddImage("", DiversityCollection.Resource.Specimen);
            Sheet.AddDataTable(Part);

            Table = "Collection";
            DiversityWorkbench.Spreadsheet.DataTable Collection = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collection", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Collection.SetDescription("The collection containing the part of the specimen");
            Collection.setParentTable(Part);
            Collection.setColorBack(TableColor(Collection.Name));
            Collection.AddImage("", DiversityCollection.Resource.Collection);
            if (Type == DiversityWorkbench.Spreadsheet.DataTable.TableType.Target)
            {
                System.Collections.Generic.List<string> CC = new List<string>();
                Collection.DataColumns()["CollectionName"].DisplayText = "Name";
                CC.Add("CollectionName");
                Collection.setDisplayedColumns(CC);
            }
            Sheet.AddDataTable(Collection);

            if (Type == DiversityWorkbench.Spreadsheet.DataTable.TableType.Target)
            {
                DiversityCollection.Spreadsheet.Target.AddPartDescriptionTable(ref Sheet, Part);

                DiversityCollection.Spreadsheet.Target.AddProcessingTable(ref Sheet, Part);

                DiversityCollection.Spreadsheet.Target.AddTransactionTable(ref Sheet, Part);

                DiversityCollection.Spreadsheet.Target.AddExternalIdentifierTable(ref Sheet, Part);
            }

            return Part;
        }

        private static void AddTransactionTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Part)
        {
            string Table = "CollectionSpecimenTransaction";
            DiversityWorkbench.Spreadsheet.DataTable ST = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            ST.SetDescription("The latest transaction linked to the part");
            ST.setColorBack(TableColor(ST.Name));
            ST.TemplateAlias = ST.Alias();
            ST.SqlRestrictionClause = " exists (select * from [dbo].[CollectionSpecimenTransaction] L where " + TableAlias(Table) + ".SpecimenPartID = L.SpecimenPartID " +
                "group by L.SpecimenPartID " +
                "having " + TableAlias(Table) + ".TransactionID = max(L.TransactionID)) ";
            ST.setParentTable(Part);
            ST.AddImage("", DiversityCollection.Resource.Transaction);
            Sheet.AddDataTable(ST);

            Table = "Transaction";
            DiversityWorkbench.Spreadsheet.DataTable Transaction = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Transaction.SetDescription("Detailed information about the transaction");
            Transaction.DataColumns()["FromTransactionPartnerAgentURI"].setRemoteLinks("FromTransactionPartnerName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Transaction.DataColumns()["ToTransactionPartnerAgentURI"].setRemoteLinks("ToTransactionPartnerName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Transaction.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Transaction.setParentTable(ST);
            Transaction.setColorBack(TableColor(Transaction.Name));
            Transaction.AddImage("", DiversityCollection.Resource.Transaction);
            Sheet.AddDataTable(Transaction);

        }

        private static void AddProcessingTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Part)
        {
            string Table = "CollectionSpecimenProcessing";
            DiversityWorkbench.Spreadsheet.DataTable SPP = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            SPP.SetDescription("The latest processing of the part");
            SPP.setColorBack(TableColor(SPP.Name));
            SPP.TemplateAlias = SPP.Alias();
            SPP.SqlRestrictionClause = " exists (select * from [dbo].[CollectionSpecimenProcessing] L where " + TableAlias(Table) + ".SpecimenPartID = L.SpecimenPartID " +
                "group by L.SpecimenPartID " +
                "having " + TableAlias(Table) + ".SpecimenProcessingID = max(L.SpecimenProcessingID)) ";
            SPP.setParentTable(Part);
            SPP.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            SPP.AddImage("", DiversityCollection.Resource.Processing);
            Sheet.AddDataTable(SPP);

            if (Specimen.DefaultUseProcessingResponsible)
            {
                if (Specimen.DefaultUseCurrentUserAsDefault)
                {
                    SPP.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                    SPP.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                }
                else
                {
                    SPP.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                    SPP.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                }
            }

            Table = "CollectionSpecimenProcessingMethod";
            DiversityWorkbench.Spreadsheet.DataTable SPPM = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            SPPM.setColorBack(TableColor(SPPM.Name));
            SPPM.TemplateAlias = SPPM.Alias();
            SPPM.SqlRestrictionClause = " exists (select * from [dbo].[CollectionSpecimenProcessingMethod] L where " + TableAlias(Table) + ".SpecimenPartID = L.SpecimenPartID " +
                "group by L.SpecimenPartID " +
                "having " + TableAlias(Table) + ".SpecimenProcessingID = max(L.SpecimenProcessingID)) ";
            SPPM.setParentTable(SPP);
            SPPM.AddImage("", DiversityCollection.Resource.Tools);
            Sheet.AddDataTable(SPPM);
        }

        private static void AddPartDescriptionTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Part)
        {
            string Table = "CollectionSpecimenPartDescription";
            DiversityWorkbench.Spreadsheet.DataTable SPD = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            SPD.SetDescription("The latest description of the part");
            SPD.setColorBack(TableColor(SPD.Name));
            SPD.TemplateAlias = SPD.Alias();
            System.Collections.Generic.List<string> RestSPD = new List<string>();
            SPD.SqlRestrictionClause = " exists (select * from [dbo].[CollectionSpecimenPartDescription] L where " + TableAlias(Table) + ".SpecimenPartID = L.SpecimenPartID " +
                "group by L.SpecimenPartID " +
                "having " + TableAlias(Table) + ".PartDescriptionID = max(L.PartDescriptionID)) ";
            SPD.DataColumns()["DescriptionTermURI"].setRemoteLinks("Description", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityScientificTerms);
            SPD.setParentTable(Part);
            SPD.DataColumns()["DescriptionTermURI"].setRemoteLinks("Description", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            SPD.AddImage("", DiversityCollection.Resource.Dictionary);
            Sheet.AddDataTable(SPD);
        }

#endregion

        private static void AddExternalIdentifierTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable ReferencedTable)
        {
            string Table = "ExternalIdentifier";
            DiversityWorkbench.Spreadsheet.DataTable ExIdPart = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Referencing, ref Sheet);
            ExIdPart.SetDescription("The latest external identifier of the part");
            ExIdPart.setColorBack(TableColor(ReferencedTable.Name));
            ExIdPart.TemplateAlias = ExIdPart.Alias();
            ExIdPart.SqlRestrictionClause = " " + TableAlias(Table) + ".ReferencedTable = 'CollectionSpecimenPart' " +
                "AND " + ReferencedTable.Alias() + "." + ReferencedTable.IdentityColumn + " = " + TableAlias(Table) + ".ReferencedID " +
                "AND exists (select * from [dbo].[ExternalIdentifier] L where " + TableAlias(Table) + ".ReferencedID = L.ReferencedID " +
                "group by L.ReferencedID " +
                "having " + TableAlias(Table) + ".ReferencedID = max(L.ReferencedID)) ";
            ExIdPart.DataColumns()["Type"].SqlLookupSource = "SELECT [TYPE] AS Display, [TYPE] AS Value FROM ExternalIdentifierType";
            ExIdPart.setParentTable(ReferencedTable);
            ExIdPart.DataColumns()["ReferencedTable"].DefaultForAdding = ReferencedTable.Name; //"CollectionSpecimenPart";
            ExIdPart.DataColumns()["ReferencedID"].ForeignRelationColumn = ReferencedTable.IdentityColumn;// "SpecimenPartID";
            ExIdPart.DataColumns()["ReferencedID"].ForeignRelationTable = ReferencedTable.Name;// "CollectionSpecimenPart";
            ExIdPart.AddImage("", DiversityCollection.Resource.Identifier);
            Sheet.AddDataTable(ExIdPart);
        }

        private static void AddAgentTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen,
            DiversityWorkbench.Spreadsheet.DataTable.TableType tableType = DiversityWorkbench.Spreadsheet.DataTable.TableType.Single)
        {
            string Table = "CollectionAgent";
            DiversityWorkbench.Spreadsheet.DataTable Agent = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collector", tableType, ref Sheet);
            Agent.setParentTable(Specimen);
            Agent.setColorBack(TableColor(Agent.Name));
            if (tableType == DiversityWorkbench.Spreadsheet.DataTable.TableType.Single)
                Agent.SetDescription("The first collector of the specimen");
            else
                Agent.SetDescription("The collector of the specimen");
            System.Collections.Generic.List<string> Ca = new List<string>();
            Ca.Add("CollectorsName");
            Agent.setDisplayedColumns(Ca);

            Agent.DataColumns()["CollectorsAgentURI"].setRemoteLinks("CollectorsName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            if (_SheetTarget != SheetTarget.Collector)
            {
                Agent.SqlRestrictionClause = " exists (select * from [CollectionAgent] F where " + TableAlias(Table) + ".CollectionSpecimenID = F.CollectionSpecimenID " +
                    "group by F.CollectionSpecimenID " +
                    "having " + TableAlias(Table) + ".CollectorsSequence = min(f.CollectorsSequence) and " + TableAlias(Table) + ".CollectorsName = min(f.CollectorsName))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
            }
            Agent.AddImage("", DiversityCollection.Resource.Agent);
            Sheet.AddDataTable(Agent);
        }

        private static void AddProjectTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen)
        {
            string Table = "CollectionProject";
            DiversityWorkbench.Spreadsheet.DataTable Project = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Project, ref Sheet);
            Project.setParentTable(Specimen);
            Project.SqlRestrictionClause = " ProjectID IN (SELECT ProjectID FROM ProjectListNotReadOnly) ";
            Project.AddImage("", DiversityCollection.Resource.Project);
            Sheet.AddDataTable(Project);
        }

        private static void AddExternalDatasourceTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen)
        {
            string Table = "CollectionExternalDatasource";
            DiversityWorkbench.Spreadsheet.DataTable ExternalDatasource = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "External datasource", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            ExternalDatasource.setParentTable(Specimen);
            ExternalDatasource.AddImage("", DiversityCollection.Resource.Import);
            ExternalDatasource.setColorBack(TableColor(ExternalDatasource.Name));
            ExternalDatasource.SetDescription("The external source for the data");
            ExternalDatasource.AddImage("", DiversityCollection.Resource.Database);
            Sheet.AddDataTable(ExternalDatasource);
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
                                case "Minerals":
                                    return SheetTarget.Minerals;
                                case "Organisms":
                                    return SheetTarget.Organisms;
                                case "Parts":
                                    return SheetTarget.Parts;
                                case "Event":
                                    return SheetTarget.Event;
                                case "Image":
                                    return SheetTarget.Image;
                                case "TK25":
                                    return SheetTarget.TK25;
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
                case "CollectionEvent":
                case "CollectionEventImage":
                case "CollectionEventLocalisation":
                case "CollectionEventProperty":
                case "CollectionEventMethod":
                case "CollectionEventParameterValue":
                    return System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                case "CollectionSpecimen":
                case "CollectionSpecimenReference":
                case "CollectionSpecimenImage":
                case "CollectionSpecimenRelation":
                case "CollectionExternalDatasource":
                    return System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                case "Collection":
                case "CollectionSpecimenPart":
                case "CollectionSpecimenPartDescription":
                case "Processing":
                case "CollectionSpecimenProcessing":
                case "CollectionSpecimenProcessingMethod":
                case "CollectionSpecimenProcessingMethodParameter":
                    return System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
                case "Transaction":
                case "CollectionSpecimenTransaction":
                    return System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(146)))), ((int)(((byte)(252)))));
                case "CollectionAgent":
                    return System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                case "Identification":
                case "IdentificationUnit":
                case "IdentificationUnitInPart":
                case "Analysis":
                case "IdentificationUnitAnalysis":
                case "IdentificationUnitAnalysisMethod":
                case "IdentificationUnitAnalysisMethodParameter":
                case "IdentificationUnitGeoAnalysis":
                    return System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
                case "Regulation":
                case "CollectionSpecimenPartRegulation":
                    return System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
                case "CollectionTask":
                case "Task":
                    return System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(114)))));
                default:
                    return System.Drawing.Color.White;
            }
        }

        //private static System.Collections.Generic.Dictionary<SheetTarget, System.Collections.Generic.Dictionary<string, string>> _TargetTableAlias;

        //private static System.Collections.Generic.Dictionary<SheetTarget, System.Collections.Generic.Dictionary<string, string>> TargetTableAlias
        //{
        //    get
        //    {
        //        if (_TargetTableAlias == null) _TargetTableAlias = new Dictionary<SheetTarget, Dictionary<string, string>>();
        //        if (!_TargetTableAlias.ContainsKey(_SheetTarget))
        //        {
        //            _TargetTableAlias.Add(_SheetTarget, TableAliases());
        //        }
        //        _TableAlias = _TargetTableAlias[_SheetTarget];
        //        return _TargetTableAlias;
        //    }
        //}


        private static System.Collections.Generic.Dictionary<string, string> _TableAlias;

        public static string TableAlias(string TableName)
        {

#if DEBUG
            //string alias = "";
            //if (TargetTableAlias[_SheetTarget].ContainsKey(TableName))
            //    alias = TargetTableAlias[_SheetTarget][TableName];
            //return alias;

#endif

            string Alias = "";

            if (_TableAlias == null)
            {
                _TableAlias = new Dictionary<string, string>();
                try {
                    _TableAlias.Add("CollectionEventSeries", "A0_ES");
                    _TableAlias.Add("CollectionEventSeriesImage", "A1_ESI");

                    _TableAlias.Add("CollectionEvent", "B0_E");
                    _TableAlias.Add("CollectionEventLocalisation", "B1_EL");
                    _TableAlias.Add("CollectionEventProperty", "B2_EP");
                    _TableAlias.Add("CollectionEventImage", "B3_EI");
                    _TableAlias.Add("CollectionEventMethod", "B4_EM");
                    _TableAlias.Add("CollectionEventParameterValue", "B5_EV");

                    _TableAlias.Add("CollectionSpecimen", "C0_S");
                    _TableAlias.Add("CollectionProject", "C1_P");
                    //_TableAlias.Add("CollectionSpecimenImage", "C2_I");
                    _TableAlias.Add("CollectionSpecimenReference", "C3_Ref");
                    _TableAlias.Add("CollectionSpecimenRelation", "C4_R");
                    _TableAlias.Add("CollectionExternalDatasource", "C5_ED");

                    _TableAlias.Add("CollectionAgent", "D1_A");

                    /// switching alphabetical sequence of Unit dependent of type
                    /// because either units or parts should be related to the target
                    switch (_SheetTarget)
                    {
                        // display Unit before Part
                        case SheetTarget.Image:
                        case SheetTarget.Event:
                        case SheetTarget.Organisms:
                        case SheetTarget.Minerals:
                            _TableAlias.Add("IdentificationUnit", "E0_U");
                            _TableAlias.Add("Identification", "E1_I");
                            _TableAlias.Add("IdentificationUnitAnalysis", "E2_UA");
                            _TableAlias.Add("IdentificationUnitAnalysisMethod", "E3_UAM");
                            _TableAlias.Add("IdentificationUnitAnalysisMethodParameter", "E4_UAMP");
                            _TableAlias.Add("IdentificationUnitGeoAnalysis", "E5_UG");
                            break;
                        // display Unit after Part
                        case SheetTarget.Parts:
                            //_TableAlias.Add("IdentificationUnitInPart", "G0_P");
                            _TableAlias.Add("IdentificationUnit", "G1_U");
                            _TableAlias.Add("Identification", "G2_I");
                            _TableAlias.Add("IdentificationUnitAnalysis", "G3_UA");
                            _TableAlias.Add("IdentificationUnitAnalysisMethod", "G4_UAM");
                            _TableAlias.Add("IdentificationUnitAnalysisMethodParameter", "G5_UAMP");
                            _TableAlias.Add("IdentificationUnitGeoAnalysis", "G6_UG");
                            break;
                        case SheetTarget.Analysis:
                            _TableAlias.Add("Analysis", "A");
                            _TableAlias.Add("AnalysisResult", "AR");
                            _TableAlias.Add("ProjectAnalysis", "AP");
                            _TableAlias.Add("AnalysisTaxonomicGroup", "ATG");
                            break;
                    }


                    _TableAlias.Add("CollectionTask", "F0_CT");
                    _TableAlias.Add("Task", "F0_CTA");
                    _TableAlias.Add("TaskModule", "F1_TM");
                    _TableAlias.Add("TaskResult", "F1_TR");
                    _TableAlias.Add("CollectionTaskImage", "F0_CTI");
                    _TableAlias.Add("CollectionSpecimenPart", "F0_P");
                    _TableAlias.Add("CollectionSpecimenPartDescription", "F1_PD");
                    _TableAlias.Add("CollectionSpecimenProcessing", "F2_P");
                    _TableAlias.Add("CollectionSpecimenProcessingMethod", "F3_PM");
                    _TableAlias.Add("CollectionSpecimenProcessingMethodParameter", "F4_PMP");
                    _TableAlias.Add("Collection", "F5_C");
                    _TableAlias.Add("CollectionSpecimenTransaction", "F6_T");
                    _TableAlias.Add("Transaction", "F7_T");

                    switch (_SheetTarget)
                    {
                        case SheetTarget.Event:
                            _TableAlias.Add("Method", "B6_EM");
                            break;
                        default:
                            _TableAlias.Add("Method", "M");
                            break;
                    }

                    /// allways after part and unit
                    _TableAlias.Add("IdentificationUnitInPart", "H1_UIP");

                    /// at the very end of the table of the corresponding domain
                    switch (_SheetTarget)
                    {
                        case SheetTarget.Image:
                            _TableAlias.Add("CollectionSpecimenImage", "D0_Im");
                            break;
                        case SheetTarget.Organisms:
                        case SheetTarget.Minerals:
                            _TableAlias.Add("CollectionSpecimenImage", "E6_Im");
                            break;
                        case SheetTarget.Parts:
                            _TableAlias.Add("CollectionSpecimenImage", "F8_Im");
                            break;
                        case SheetTarget.Event:
                        case SheetTarget.Specimen:
                            _TableAlias.Add("CollectionSpecimenImage", "C6_Im");
                            break;
                    }
                    //if (_SheetTarget != SheetTarget.Image)

                    switch (_SheetTarget)
                    {
                        case SheetTarget.Parts:
                            _TableAlias.Add("ExternalIdentifier", "F9_EI");
                            break;
                        case SheetTarget.Event:
                            _TableAlias.Add("ExternalIdentifier", "B8_EI");
                            break;
                        case SheetTarget.Organisms:
                        case SheetTarget.Minerals:
                            _TableAlias.Add("ExternalIdentifier", "K0_EI");
                            break;
                    }

                    _TableAlias.Add("Annotation", "L0_A");
                }
                catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            if (_TableAlias.ContainsKey(TableName))
                    Alias = _TableAlias[TableName];
                else
                {

                }
            return Alias;
        }

        private static Dictionary<string, string> TableAliases()
        {

            Dictionary<string, string>  Alias = new Dictionary<string, string>();
            switch(_SheetTarget)
            {
                case SheetTarget.TK25:
                    Alias.Add("CollectionEvent", "A1_E");
                    Alias.Add("CollectionEventLocalisation", "A2_EL");

                    Alias.Add("CollectionSpecimen", "C0_S");
                    Alias.Add("CollectionProject", "C1_P");
                    Alias.Add("CollectionExternalDatasource", "C2_E");
                    Alias.Add("CollectionSpecimenReference", "C3_R");

                    Alias.Add("CollectionAgent", "D1_A");

                    Alias.Add("IdentificationUnit", "E0_U");
                    Alias.Add("Identification", "E1_I");
                    Alias.Add("IdentificationUnitAnalysis", "E2_UA");
                    //Alias.Add("IdentificationUnitAnalysis", "E2_UA_Status");
                    Alias.Add("IdentificationUnitGeoAnalysis", "E2_UGA");
                    Alias.Add("CollectionSpecimenImage", "E3_Im");

                    Alias.Add("IdentificationUnitInPart", "G0_UP");
                    Alias.Add("CollectionSpecimenPart", "G1_P");
                    Alias.Add("Collection", "G2_C");
                    break;
                default:
                    try
                    {
                        Alias.Add("CollectionEventSeries", "A0_ES");
                        Alias.Add("CollectionEventSeriesImage", "A1_ESI");

                        Alias.Add("CollectionEvent", "B0_E");
                        Alias.Add("CollectionEventLocalisation", "B1_EL");
                        Alias.Add("CollectionEventProperty", "B2_EP");
                        Alias.Add("CollectionEventImage", "B3_EI");
                        Alias.Add("CollectionEventMethod", "B4_EM");
                        Alias.Add("CollectionEventParameterValue", "B5_EV");

                        Alias.Add("CollectionSpecimen", "C0_S");
                        Alias.Add("CollectionProject", "C1_P");
                        //_TableAlias.Add("CollectionSpecimenImage", "C2_I");
                        Alias.Add("CollectionSpecimenReference", "C3_Ref");
                        Alias.Add("CollectionSpecimenRelation", "C4_R");
                        Alias.Add("CollectionExternalDatasource", "C5_ED");

                        Alias.Add("CollectionAgent", "D1_A");

                        /// switching alphabetical sequence of Unit dependent of type
                        /// because either units or parts should be related to the target
                        switch (_SheetTarget)
                        {
                            // display Unit before Part
                            case SheetTarget.Image:
                            case SheetTarget.Event:
                            case SheetTarget.Organisms:
                            case SheetTarget.Minerals:
                                Alias.Add("IdentificationUnit", "E0_U");
                                Alias.Add("Identification", "E1_I");
                                Alias.Add("IdentificationUnitAnalysis", "E2_UA");
                                Alias.Add("IdentificationUnitAnalysisMethod", "E3_UAM");
                                Alias.Add("IdentificationUnitAnalysisMethodParameter", "E4_UAMP");
                                Alias.Add("IdentificationUnitGeoAnalysis", "E5_UG");
                                break;
                            // display Unit after Part
                            case SheetTarget.Parts:
                                //_TableAlias.Add("IdentificationUnitInPart", "G0_P");
                                Alias.Add("IdentificationUnit", "G1_U");
                                Alias.Add("Identification", "G2_I");
                                Alias.Add("IdentificationUnitAnalysis", "G3_UA");
                                Alias.Add("IdentificationUnitAnalysisMethod", "G4_UAM");
                                Alias.Add("IdentificationUnitAnalysisMethodParameter", "G5_UAMP");
                                Alias.Add("IdentificationUnitGeoAnalysis", "G6_UG");
                                break;
                            case SheetTarget.Analysis:
                                Alias.Add("Analysis", "A");
                                Alias.Add("AnalysisResult", "AR");
                                Alias.Add("ProjectAnalysis", "AP");
                                Alias.Add("AnalysisTaxonomicGroup", "ATG");
                                break;
                        }


                        Alias.Add("CollectionTask", "F0_CT");
                        Alias.Add("Task", "F0_CTA");
                        Alias.Add("TaskModule", "F1_TM");
                        Alias.Add("TaskResult", "F1_TR");
                        Alias.Add("CollectionTaskImage", "F0_CTI");
                        Alias.Add("CollectionSpecimenPart", "F0_P");
                        Alias.Add("CollectionSpecimenPartDescription", "F1_PD");
                        Alias.Add("CollectionSpecimenProcessing", "F2_P");
                        Alias.Add("CollectionSpecimenProcessingMethod", "F3_PM");
                        Alias.Add("CollectionSpecimenProcessingMethodParameter", "F4_PMP");
                        Alias.Add("Collection", "F5_C");
                        Alias.Add("CollectionSpecimenTransaction", "F6_T");
                        Alias.Add("Transaction", "F7_T");

                        switch (_SheetTarget)
                        {
                            case SheetTarget.Event:
                                Alias.Add("Method", "B6_EM");
                                break;
                            default:
                                Alias.Add("Method", "M");
                                break;
                        }

                        /// allways after part and unit
                        Alias.Add("IdentificationUnitInPart", "H1_UIP");

                        /// at the very end of the table of the corresponding domain
                        switch (_SheetTarget)
                        {
                            case SheetTarget.Image:
                                Alias.Add("CollectionSpecimenImage", "D0_Im");
                                break;
                            case SheetTarget.Organisms:
                            case SheetTarget.Minerals:
                                Alias.Add("CollectionSpecimenImage", "E6_Im");
                                break;
                            case SheetTarget.Parts:
                                Alias.Add("CollectionSpecimenImage", "F8_Im");
                                break;
                            case SheetTarget.Event:
                            case SheetTarget.Specimen:
                                Alias.Add("CollectionSpecimenImage", "C6_Im");
                                break;
                        }
                        //if (_SheetTarget != SheetTarget.Image)

                        switch (_SheetTarget)
                        {
                            case SheetTarget.Parts:
                                Alias.Add("ExternalIdentifier", "F9_EI");
                                break;
                            case SheetTarget.Event:
                                Alias.Add("ExternalIdentifier", "B8_EI");
                                break;
                            case SheetTarget.Organisms:
                            case SheetTarget.Minerals:
                                Alias.Add("ExternalIdentifier", "K0_EI");
                                break;
                        }

                        Alias.Add("Annotation", "L0_A");
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                    break;
            }
            return Alias;
        }

#endregion

#region TK25

        private static DiversityWorkbench.Spreadsheet.Sheet TK25Sheet()
        {
            DiversityWorkbench.Spreadsheet.Sheet Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: TK25", "TK25");
#if xxDEBUG
            sheetTarget = SheetTarget.TK25;
#else
            _SheetTarget = SheetTarget.TK25;
            //if (TargetTableAlias.ContainsKey(_SheetTarget))
            //    _TableAlias = TargetTableAlias[_SheetTarget];
#endif
            InitTableAliasForTK25();

            Sheet.setProjectSqlSoure("SELECT Project, ProjectID FROM ProjectListNotReadOnly ORDER BY Project");
            Sheet.setProjectReadOnlySqlSoure("SELECT P.Project, U.ProjectID " +
                "FROM dbo.ProjectUser U INNER JOIN " +
                "dbo.ProjectProxy P ON U.ProjectID = P.ProjectID " +
                "WHERE U.LoginName = USER_NAME() AND (U.[ReadOnly] = 1) " +
                "GROUP BY P.Project, U.ProjectID " +
                "ORDER BY P.Project");

            string Table = "CollectionEvent";
            DiversityWorkbench.Spreadsheet.DataTable Event = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Event", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
            try
            {
                Event.SetDescription("The collection event during which the organisms had been observed or collected");
                Event.setColorBack(TableColor(Event.Name));
                System.Collections.Generic.List<string> Ce = new List<string>();
                Ce.Add("CollectionYear");
                Ce.Add("LocalityDescription");
                Event.DataColumns()["LocalityDescription"].DisplayText = "Locality";
                Event.DataColumns()["LocalityDescription"].Width = 100;
                Event.DataColumns()["CollectionYear"].DisplayText = "Year";
                Event.DataColumns()["CollectionYear"].Width = 40;

                Event.AddColumn("AverageYear", "cast(cast(case when [#TableAlias#].[CollectionEndYear] is null then [#TableAlias#].[CollectionYear] else case when [#TableAlias#].[CollectionYear] is null then [#TableAlias#].[CollectionEndYear] else ([#TableAlias#].[CollectionYear] + [#TableAlias#].[CollectionEndYear] )/2 end end as int) as varchar(4))", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
                Event.DataColumns()["AverageYear"].setReadOnly(true);
                Event.DataColumns()["AverageYear"].Column.DataType = Event.DataColumns()["CollectionYear"].Column.DataType;
                Event.DataColumns()["AverageYear"].Width = 40;
                Ce.Add("AverageYear");

                Event.setDisplayedColumns(Ce);
               
                Event.DataColumns()["CollectionDate"].setReadOnly(true);
                Event.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
                Event.DataColumns()["SeriesID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT [SeriesID] as Value " +
                ",case when [DateStart] is null then '' else convert(varchar(10), [DateStart], 120) + case when [DateEnd] is null or cast([DateStart] as varchar(10)) = cast([DateEnd] as varchar(10)) then '' else ' - ' + convert(varchar(10), [DateEnd], 120) end + ': ' end + " +
                "+ case when [SeriesCode] is null then '' else [SeriesCode] + '; ' end " +
                "+ case when [Description] is null then '' else substring([Description], 1, 50) end AS Display " +
                "FROM [dbo].[CollectionEventSeries] ORDER BY Display";
                Event.AddImage("", DiversityCollection.Resource.Event);
                Event.IsRequired = true;
                Sheet.AddDataTable(Event);

                // Adding TK25
                TK25AddTK25(ref Sheet, ref Event);
                // Adding WGS84
                TK25AddWGS84(ref Sheet, ref Event);

                // Adding Specimen
                TK25AddSpecimen(ref Sheet, ref Event);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return Sheet;
        }

        private static void TK25AddTK25(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Event)
        {
            string Table = "CollectionEventLocalisation";
            DiversityWorkbench.Spreadsheet.DataTable TK25 = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "TK25", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            try
            {
                // Adding TK25
                TK25.SetDescription("The topographical map (1:25000) and the quadrant where the organisms where collected or observed");
                TK25.setColorBack(TableColor(TK25.Name));
                TK25.DataColumns()["Location1"].DisplayText = "TK25";
                TK25.DataColumns()["Location1"].Width = 40;
                TK25.DataColumns()["Location2"].DisplayText = "Quad.";
                TK25.DataColumns()["Location2"].Width = 40;
                TK25.DataColumns()["Geography"].setReadOnly(true);
                TK25.DataColumns()["Geography"].Width = 50;
                TK25.DataColumns()["Geography"].DisplayText = "Center";
                TK25.DataColumns()["Geography"].SqlForColumn = " case when LEN([#TableAlias#].[Location2]) = 1 then [#TableAlias#].[Geography].EnvelopeCenter().ToString() else null end ";
                TK25.AddImage("", DiversityCollection.Resource.MTB);
                System.Collections.Generic.List<string> RestrictTK25 = new List<string>();
                RestrictTK25.Add("LocalisationSystemID");
                TK25.RestrictionColumns = RestrictTK25;
                TK25.DataColumns()["LocalisationSystemID"].RestrictionValue = "3";
                TK25.setParentTable(TK25Event);
                TK25.AddImage("3", DiversityCollection.Specimen.ImageForLocalisationSystem(3));
                TK25.DisplayText = "TK25";
                TK25.IsRequired = true;

                TK25.AddColumn("SymbolSize", "case when len([#TableAlias#].[Location2]) = 0 then 1 else cast(1 as float)/power(2, len([#TableAlias#].[Location2])) end", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
                TK25.DataColumns()["SymbolSize"].setReadOnly(true);

                TK25.AddColumn("TK25/Quad.", "case when len([#TableAlias#].[Location2]) = 0 then NULL else [#TableAlias#].Location1 + '/' + substring([#TableAlias#].[Location2], 1, 1) end", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
                TK25.DataColumns()["TK25/Quad."].setReadOnly(true);
                TK25.DataColumns()["TK25/Quad."].Width = 60;
                TK25.DataColumns()["TK25/Quad."].Column.DataType = "nvarchar(6)";

                TK25.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

                System.Collections.Generic.List<string> Cw = new List<string>();
                Cw.Add("Location1");
                Cw.Add("Location2");
                Cw.Add("TK25/Quad.");
                //Cw.Add("SymbolSize");
                //Cw.Add("Geography");
                TK25.setDisplayedColumns(Cw);

                TK25.IsRequired = true;

                Sheet.AddDataTable(TK25);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void TK25AddWGS84(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Event)
        {
            string Table = "CollectionEventLocalisation";
            DiversityWorkbench.Spreadsheet.DataTable WGS84 = new DiversityWorkbench.Spreadsheet.DataTable("A2_EWGS", Table, "WGS84", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            try
            {
                // Adding TK25
                WGS84.SetDescription("The WGS84 coordinates of the collection event");
                WGS84.setColorBack(TableColor(WGS84.Name));
                WGS84.DataColumns()["Geography"].Width = 70;
                WGS84.DataColumns()["Geography"].DisplayText = "Geography";
                WGS84.DataColumns()["Geography"].setReadOnly(true);
                WGS84.AddImage("", DiversityCollection.Resource.Localisation);
                System.Collections.Generic.List<string> RestrictWGS84 = new List<string>();
                RestrictWGS84.Add("LocalisationSystemID");
                WGS84.RestrictionColumns = RestrictWGS84;
                WGS84.DataColumns()["LocalisationSystemID"].RestrictionValue = "8";
                WGS84.setParentTable(TK25Event);
                WGS84.AddImage("8", DiversityCollection.Specimen.ImageForLocalisationSystem(8));
                WGS84.DisplayText = "WGS84";
                WGS84.IsRequired = false;

                WGS84.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

                WGS84.AddColumn("Center", "[#TableAlias#].[Geography].EnvelopeCenter().ToString()", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
                WGS84.DataColumns()["Center"].setReadOnly(true);
                WGS84.DataColumns()["Center"].Width = 60;

                Sheet.AddDataTable(WGS84);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void TK25AddSpecimen(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Event)
        {
            string Table = "CollectionSpecimen";
            DiversityWorkbench.Spreadsheet.DataTable Specimen = null;
            try
            {
                Specimen = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Specimen", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
                //Specimen = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.RootHidden, ref Sheet);
                Specimen.SetDescription("The specimen containing the organisms");
                Specimen.setParentTable(TK25Event);
                Specimen.setColorBack(TableColor(Specimen.Name));
                Specimen.DataColumns()["ExternalDatasourceID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT ExternalDatasourceID AS Value, ExternalDatasourceName + CASE WHEN [ExternalDatasourceVersion] IS NULL THEN '' ELSE '(' + [ExternalDatasourceVersion] + ')' END AS Display " +
                    "FROM CollectionExternalDatasource ORDER BY Display ";
                Specimen.DataColumns()["CollectionEventID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT DISTINCT E.[CollectionEventID] AS Value " +
                ",case when [CollectionYear] is null then '' else cast([CollectionYear] as varchar) + case when [CollectionMonth] is null then '' else '-' + cast([CollectionMonth] as varchar) end + case when [CollectionDay] is null then '' else '-' + cast([CollectionDay] as varchar) end + ': ' end " +
                "+ case when [CollectorsEventNumber] is null then '' else [CollectorsEventNumber] + '; ' end " +
                "+ case when [LocalityDescription] is null then '' else substring([LocalityDescription], 1, 200)  end AS Display " +
                "FROM [dbo].[CollectionEvent] E , CollectionSpecimen S, CollectionSpecimenID_Available P " +
                "WHERE E.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = P.CollectionSpecimenID ORDER BY Display";
                Specimen.DataColumns()["DepositorsAgentURI"].setRemoteLinks("DepositorsName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                //Specimen.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
                Specimen.DataColumns()["ExsiccataURI"].setRemoteLinks("ExsiccataAbbreviation", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityExsiccatae);
                Specimen.AddImage("", DiversityCollection.Resource.CollectionSpecimen);
                Specimen.IsRequired = true;
                Specimen.DataColumns()["ReferenceTitle"].IsOutdated = true;
                Specimen.DataColumns()["ReferenceURI"].IsOutdated = true;
                Specimen.DataColumns()["ReferenceDetails"].IsOutdated = true;

                System.Collections.Generic.List<string> Ca = new List<string>();
                Ca.Add("AccessionNumber");
                Specimen.setDisplayedColumns(Ca);

                Sheet.AddDataTable(Specimen);

                DiversityCollection.Spreadsheet.Target.AddProjectTable(ref Sheet, Specimen);

                Sheet.MasterQueryColumn = Specimen.DataColumns()["CollectionSpecimenID"];

                TK25AddExternalSource(ref Sheet, Specimen);

                TK25AddAgent(ref Sheet, Specimen);

                TK25AddSpecimenReference(ref Sheet, Specimen);

                TK25AddUnit(ref Sheet, ref Specimen);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void TK25AddAgent(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen)
        {
            string Table = "CollectionAgent";
            DiversityWorkbench.Spreadsheet.DataTable Agent = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collector", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            Agent.SetDescription("The collector or observer of the organisms");
            Agent.setParentTable(Specimen);
            Agent.setColorBack(TableColor(Agent.Name));
            System.Collections.Generic.List<string> Ca = new List<string>();
            Ca.Add("CollectorsName");
            Ca.Add("RowGUID");
            Agent.setDisplayedColumns(Ca);
            Agent.DataColumns()["CollectorsAgentURI"].setRemoteLinks("CollectorsName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Agent.DataColumns()["RowGUID"].IsHidden = true;
            Agent.SqlRestrictionClause = " exists (select * from [CollectionAgent] F where " + TableAlias(Table) + ".CollectionSpecimenID = F.CollectionSpecimenID " +
                "group by F.CollectionSpecimenID " +
                "having " + TableAlias(Table) + ".CollectorsSequence = min(f.CollectorsSequence))"; // Markus 18.7.23: Fehler mit nicht eindeutiger Sequence in Maintenance + Trigger behoben and " + TableAlias(Table) + ".CollectorsName = min(f.CollectorsName))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
            Agent.AddImage("", DiversityCollection.Resource.Agent);

            //System.Collections.Generic.List<string> AgentURIFixSourceSetting = new List<string>();
            //AgentURIFixSourceSetting.Add("ModuleSource");
            //AgentURIFixSourceSetting.Add("CollectionAgent");
            //Agent.DataColumns()["CollectorsAgentURI"].SetFixSourceSetting(AgentURIFixSourceSetting, true);

            Sheet.AddDataTable(Agent);
        }

        private static void TK25AddExternalSource(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen)
        {
            string Table = "CollectionExternalDatasource";
            DiversityWorkbench.Spreadsheet.DataTable Source = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Source", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Source.SetDescription("The source of the data, e.g. an external database imported into DiversityCollection");
            Source.setParentTable(Specimen);
            Source.setColorBack(TableColor(Source.Name));
            Source.AddImage("", DiversityCollection.Resource.Import);
            System.Collections.Generic.List<string> Ca = new List<string>();
            Ca.Add("ExternalDatasourceName");
            Source.DataColumns()["ExternalDatasourceName"].setReadOnly(true);
            Source.setDisplayedColumns(Ca);
            Sheet.AddDataTable(Source);
        }

        private static void TK25AddSpecimenReference(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen)
        {
            string Table = "CollectionSpecimenReference";
            DiversityWorkbench.Spreadsheet.DataTable Reference = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Reference", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            Reference.SetDescription("The references for a specimen");
            Reference.setParentTable(Specimen);
            Reference.setColorBack(TableColor(Reference.Name));
            Reference.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
            //Reference.DataColumns()["IdentificationUnitID"].RestrictionValue = "IS NULL";
            //Reference.DataColumns()["IdentificationUnitID"].IsRestrictionColumn = true;
            Reference.SqlRestrictionClause = TableAlias(Table) + ".IdentificationUnitID IS NULL";
            //string Test = Reference.DataColumns()["IdentificationUnitID"].RestrictionValue;
            Reference.AddImage("", DiversityCollection.Resource.References);
            System.Collections.Generic.List<string> Ca = new List<string>();
            Ca.Add("ReferenceTitle");
            Reference.setDisplayedColumns(Ca);
            Sheet.AddDataTable(Reference);
        }


        private static void TK25AddUnit(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Specimen)
        {
            string Table = "IdentificationUnit";
            DiversityWorkbench.Spreadsheet.DataTable Unit = null;
            try
            {
                Unit = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Target, ref Sheet);
                Unit.SetDescription("The organism collected or observed");
                Unit.setParentTable(TK25Specimen);
                Unit.setColorBack(TableColor(Unit.Name));
                System.Collections.Generic.List<string> Us = new List<string>();
                Us.Add("FamilyCache");
                Us.Add("OrderCache");
                Us.Add("HierarchyCache");
                Us.Add("LastIdentificationCache");
                Us.Add("TaxonomicGroup");
                Unit.setDisplayedColumns(Us);
                Unit.DataColumns()["LastIdentificationCache"].DisplayText = "Taxon";
                Unit.DataColumns()["LastIdentificationCache"].Width = 150;
                Unit.DataColumns()["LastIdentificationCache"].setReadOnly(true);
                Unit.DataColumns()["LastIdentificationCache"].DefaultForAdding = "?";
                Unit.DataColumns()["FamilyCache"].IsHidden = true;
                Unit.DataColumns()["OrderCache"].IsHidden = true;
                Unit.DataColumns()["HierarchyCache"].IsHidden = true;
                Unit.DataColumns()["TaxonomicGroup"].IsHidden = true;
                Unit.AddImage("", DiversityCollection.Resource.Plant);
                Unit.IsRequired = true;
                Unit.DisplayText = "Observation";
                Sheet.AddDataTable(Unit);

                TK25AddNewIdentification(ref Sheet, Unit);
                TK25AddFirstIdentification(ref Sheet, Unit);
                TK25AddLastIdentification(ref Sheet, Unit);

                TK25AddAnalysis(ref Sheet, ref Unit);
                TK25AddGeoAnalysis(ref Sheet, ref Unit);

                TK25AddUnitInPart(ref Sheet, ref Unit);

                TK25AddUnitImage(ref Sheet, ref Unit);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void TK25AddNewIdentification(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "Identification";
            DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, DiversityCollection.Spreadsheet.TargetText.Last_ident, DiversityWorkbench.Spreadsheet.DataTable.TableType.InsertOnly, ref Sheet);
            Ident.SetDescription("Inserting a new identification for the organism");
            Ident.setParentTable(Unit);
            Ident.setColorBack(TableColor(Ident.Name));
            Ident.DisplayText = "New ident.";
            System.Collections.Generic.List<string> Is = new List<string>();
            Is.Add("TaxonomicName");
            Is.Add("NameURI");
            Ident.setDisplayedColumns(Is);
            Ident.DataColumns()["NameURI"].DisplayText = "New name";
            Ident.DataColumns()["TaxonomicName"].IsHidden = true;
            Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
            Ident.DataColumns()["ReferenceTitle"].IsOutdated = true;
            Ident.DataColumns()["ReferenceURI"].IsOutdated = true;
            Ident.DataColumns()["ReferenceDetails"].IsOutdated = true;

            Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
                "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                "having " + TableAlias(Table) + ".IdentificationSequence > max(L.IdentificationSequence)) ";

            if (Specimen.DefaultUseIdentificationResponsible)
            {
                if (Specimen.DefaultUseCurrentUserAsDefault)
                {
                    Ident.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                    Ident.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                }
                else
                {
                    Ident.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                    Ident.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                }
            }

            // binding for DiversityTaxonNames
            DiversityWorkbench.Spreadsheet.RemoteColumnBinding FamilyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            FamilyCache.Column = Unit.DataColumns()["FamilyCache"];
            FamilyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            FamilyCache.RemoteParameter = "Family";

            DiversityWorkbench.Spreadsheet.RemoteColumnBinding OrderCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            OrderCache.Column = Unit.DataColumns()["OrderCache"];
            OrderCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            OrderCache.RemoteParameter = "Order";

            DiversityWorkbench.Spreadsheet.RemoteColumnBinding HierarchyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            HierarchyCache.Column = Unit.DataColumns()["HierarchyCache"];
            HierarchyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            HierarchyCache.RemoteParameter = "Hierarchy";

            System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding> TaxBindList = new List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding>();
            TaxBindList.Add(FamilyCache);
            TaxBindList.Add(OrderCache);
            TaxBindList.Add(HierarchyCache);
            System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteLink> TaxRemLinkList = new List<DiversityWorkbench.Spreadsheet.RemoteLink>();
            DiversityWorkbench.Spreadsheet.RemoteLink RemoteLinkTaxon = new DiversityWorkbench.Spreadsheet.RemoteLink(
                DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames,
                null,
                TaxBindList);
            TaxRemLinkList.Add(RemoteLinkTaxon);
            Ident.DataColumns()["NameURI"].setRemoteLinks("TaxonomicName", TaxRemLinkList);
            Ident.DataColumns()["NameURI"].DisplayText = "New name";
            System.Collections.Generic.List<string> NameURIFixSourceSetting = new List<string>();
            NameURIFixSourceSetting.Add("ModuleSource");
            NameURIFixSourceSetting.Add("Identification");
            NameURIFixSourceSetting.Add("TaxonomicGroup");
            NameURIFixSourceSetting.Add(Unit.DataColumns()["TaxonomicGroup"].DataTable().Alias() + "." + Unit.DataColumns()["TaxonomicGroup"].Name);
            Ident.DataColumns()["NameURI"].FixedSourceSetSetting(NameURIFixSourceSetting, true);

            Ident.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

            Ident.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
            Ident.DataColumns()["ReferenceURI"].IsOutdated = true;
            Ident.DataColumns()["ReferenceTitle"].IsOutdated = true;
            Ident.DataColumns()["ReferenceDetails"].IsOutdated = true;

            Ident.DataColumns()["TaxonomicName"].DefaultForAdding = "?";
            Ident.DataColumns()["IdentificationSequence"].SqlQueryForDefaultForAdding = "SELECT case when MAX(T.IdentificationSequence) is null then 1 else MAX(T.IdentificationSequence) + 1 end FROM Identification AS T WHERE T.IdentificationUnitID = #IdentificationUnitID# ";
            Ident.AddImage("", DiversityCollection.Resource.Identification);
            Sheet.AddDataTable(Ident);
        }

        private static void TK25AddFirstIdentification(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "Identification";
            DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable("E1_IF", Table, DiversityCollection.Spreadsheet.TargetText.First_ident, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Ident.SetDescription("The first identification of the organism");
            Ident.setParentTable(Unit);
            Ident.setColorBack(TableColor(Ident.Name));
            Ident.DisplayText = "First ident.";
            //System.Collections.Generic.List<string> Is = new List<string>();
            //Is.Add("TaxonomicName");
            //Ident.setDisplayedColumns(Is);
            Ident.DataColumns()["TaxonomicName"].setReadOnly(true);
            Ident.DataColumns()["TaxonomicName"].DisplayText = "First name";
            Ident.DataColumns()["TaxonomicName"].Width = 150;
            Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
            Ident.DataColumns()["ReferenceTitle"].IsOutdated = true;
            Ident.DataColumns()["ReferenceURI"].IsOutdated = true;
            Ident.DataColumns()["ReferenceDetails"].IsOutdated = true;
            Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where E1_IF.CollectionSpecimenID = L.CollectionSpecimenID " +
                "and E1_IF.IdentificationUnitID = L.IdentificationUnitID " +
                "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                "having E1_IF.IdentificationSequence = min(L.IdentificationSequence)) ";

            Ident.AddImage("", DiversityCollection.Resource.Identification);
            Sheet.AddDataTable(Ident);
        }

        private static void TK25AddLastIdentification(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "Identification";
            DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable("E1_IL", Table, DiversityCollection.Spreadsheet.TargetText.Last_ident, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Ident.SetDescription("The last identification of the organism");
            Ident.setParentTable(Unit);
            Ident.setColorBack(TableColor(Ident.Name));
            Ident.DisplayText = "Last ident.";
            System.Collections.Generic.List<string> Is = new List<string>();
            //Is.Add("TaxonomicName");
            Is.Add("NameURI");
            Ident.setDisplayedColumns(Is);
            Ident.DataColumns()["TaxonomicName"].setReadOnly(true);
            Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
            Ident.DataColumns()["NameURI"].setReadOnly(true);
            Ident.DataColumns()["NameURI"].DisplayText = "Filter taxa";
            Ident.DataColumns()["NameURI"].setRemoteLinks("TaxonomicName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames);
            //Ident.DataColumns()["NameURI"].LinkedModule = DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames;
            Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where E1_IL.CollectionSpecimenID = L.CollectionSpecimenID " +
                "and E1_IL.IdentificationUnitID = L.IdentificationUnitID " +
                "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                "having E1_IL.IdentificationSequence = max(L.IdentificationSequence)) ";

            Ident.AddImage("", DiversityCollection.Resource.Identification);
            Ident.DataColumns()["ReferenceTitle"].IsOutdated = true;
            Ident.DataColumns()["ReferenceURI"].IsOutdated = true;
            Ident.DataColumns()["ReferenceDetails"].IsOutdated = true;
            Sheet.AddDataTable(Ident);
        }

        private static void TK25AddAnalysis(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Unit)
        {
            string Table = "IdentificationUnitAnalysis";
            try
            {
                //DiversityWorkbench.Spreadsheet.DataTable AnaLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                DiversityWorkbench.Spreadsheet.DataTable AnaLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
                AnaLast.SetDescription("The last analysis of the organism");
                AnaLast.setColorBack(TableColor(AnaLast.Name));
                AnaLast.TemplateAlias = AnaLast.Alias();
                System.Collections.Generic.List<string> RestAnaLast = new List<string>();
                RestAnaLast.Add("AnalysisID");
                AnaLast.RestrictionColumns = RestAnaLast;
                string SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                    "FROM Analysis ORDER BY DisplayText";
                AnaLast.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
                AnaLast.SqlRestrictionClause = " exists (select * from [dbo].[IdentificationUnitAnalysis] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
                    "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                    "and " + TableAlias(Table) + ".AnalysisID = L.AnalysisID " +
                    "group by L.CollectionSpecimenID, L.IdentificationUnitID, L.AnalysisID " +
                    "having " + TableAlias(Table) + ".AnalysisNumber = max(L.AnalysisNumber))  ";
                System.Collections.Generic.List<string> Us = new List<string>();
                Us.Add("Result");
                AnaLast.setDisplayedColumns(Us);
                AnaLast.DataColumns()["AnalysisNumber"].DisplayText = "LastAnalysisNumber";
                AnaLast.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
                AnaLast.DataColumns()["AnalysisNumber"].SqlQueryForDefaultForAdding = "select top 1 case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) is null and T.AnalysisNumber = '' " +
                    "then '1' else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) < 9  " +
                    "then cast(try_Parse(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar)  " +
                    "else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) = 9  " +
                    "then 'A' else char(ascii(substring(T.AnalysisNumber, 1, 1)) + 1) end end end " +
                    "FROM IdentificationUnitAnalysis AS T " +
                    "where t.IdentificationUnitID = #IdentificationUnitID# " +
                    "order by char(ascii(substring(T.AnalysisNumber, 1, 1))) desc ";
                AnaLast.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                SQL = "SELECT MIN(DisplayText) FROM Analysis";
                AnaLast.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                AnaLast.setParentTable(TK25Unit);
                AnaLast.DisplayText = "Last analy.";
                AnaLast.SetDescription("Last analysis of the organism");
                AnaLast.AddImage("", DiversityCollection.Resource.AnalysisHierarchy);
                Sheet.AddDataTable(AnaLast);

                DiversityWorkbench.Spreadsheet.DataTable AnaNew = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table) + "_N", Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                AnaNew.SetDescription("Adding a new analysis for the organism");
                AnaNew.setColorBack(TableColor(AnaNew.Name));
                AnaNew.TemplateAlias = AnaNew.Alias();
                System.Collections.Generic.List<string> RestAnaNew = new List<string>();
                RestAnaNew.Add("AnalysisID");
                AnaNew.RestrictionColumns = RestAnaNew;
                SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                    "FROM Analysis ORDER BY DisplayText";
                AnaNew.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
                AnaNew.SqlRestrictionClause = " " + TableAlias(Table) + "_N" + ".AnalysisNumber IS NULL  ";

                if (Specimen.DefaultUseAnalyisisResponsible)
                {
                    if (Specimen.DefaultUseCurrentUserAsDefault)
                    {
                        AnaNew.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                        AnaNew.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                    }
                    else
                    {
                        AnaNew.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                        AnaNew.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                    }
                }

                System.Collections.Generic.List<string> Un = new List<string>();
                Un.Add("Result");
                AnaNew.setDisplayedColumns(Un);
                AnaNew.DataColumns()["AnalysisNumber"].DisplayText = "NewAnalysisNumber";
                AnaNew.DataColumns()["AnalysisNumber"].SqlQueryForDefaultForAdding = "select top 1 case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) is null and T.AnalysisNumber = '' " +
                    "then '1' else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) < 9  " +
                    "then cast(try_Parse(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar)  " +
                    "else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) = 9  " +
                    "then 'A' else char(ascii(substring(T.AnalysisNumber, 1, 1)) + 1) end end end " +
                    "FROM IdentificationUnitAnalysis AS T " +
                    "where t.IdentificationUnitID = #IdentificationUnitID# " +
                    "order by char(ascii(substring(T.AnalysisNumber, 1, 1))) desc ";//  select top 1 case when T.AnalysisNumber is null then '1' else cast(cast(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar) end FROM IdentificationUnitAnalysis AS T where T.IdentificationUnitID = #IdentificationUnitID# order by T.AnalysisNumber desc ";
                AnaNew.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                SQL = "SELECT MIN(DisplayText) FROM Analysis";
                AnaNew.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                AnaNew.setParentTable(TK25Unit);
                AnaNew.DisplayText = "New analy.";
                AnaNew.SetDescription("New analysis of the organism");
                AnaNew.AddImage("", DiversityCollection.Resource.AnalysisHierarchy);
                SQL = "SELECT AnalysisResult AS Value, DisplayText AS Display " +
                    "FROM AnalysisResult WHERE AnalysisID = #AnalysisID# ORDER BY Display";
                AnaNew.DataColumns()["AnalysisResult"].SqlLookupSource = SQL;
                Sheet.AddDataTable(AnaNew);

                DiversityWorkbench.Spreadsheet.DataTable AnaNewStatus = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table) + "_N_Status", Table, "Neuer Status", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                AnaNewStatus.SetDescription("Status Bayernflora: Setting a new status for the organism - this table is designed for the entry of new data for the project BayernFlora and is meaningless outside of this project");
                AnaNewStatus.setColorBack(TableColor(AnaNewStatus.Name));
                AnaNewStatus.TemplateAlias = AnaNewStatus.Alias();
                System.Collections.Generic.List<string> RestAnaNewStatus = new List<string>();
                RestAnaNewStatus.Add("AnalysisID");
                AnaNewStatus.RestrictionColumns = RestAnaNewStatus;
                SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                    "FROM Analysis WHERE AnalysisID = 2 ORDER BY DisplayText";
                AnaNewStatus.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
                AnaNewStatus.DataColumns()["AnalysisID"].RestrictionValue = "2";
                AnaNewStatus.DataColumns()["AnalysisID"].ColumnDefault = "2";
                AnaNewStatus.SqlRestrictionClause = " " + TableAlias(Table) + "_N_Status" + ".AnalysisNumber IS NULL  ";

                if (Specimen.DefaultUseAnalyisisResponsible)
                {
                    if (Specimen.DefaultUseCurrentUserAsDefault)
                    {
                        AnaNewStatus.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                        AnaNewStatus.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                    }
                    else
                    {
                        AnaNewStatus.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                        AnaNewStatus.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                    }
                }

                System.Collections.Generic.List<string> UnStatus = new List<string>();
                UnStatus.Add("Result");
                AnaNewStatus.setDisplayedColumns(UnStatus);
                AnaNewStatus.DataColumns()["AnalysisNumber"].DisplayText = "Neue Nummer";
                AnaNewStatus.DataColumns()["AnalysisNumber"].SqlQueryForDefaultForAdding = "select top 1 case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) is null and T.AnalysisNumber = '' " +
                    "then '1' else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) < 9  " +
                    "then cast(try_Parse(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar)  " +
                    "else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) = 9  " +
                    "then 'A' else char(ascii(substring(T.AnalysisNumber, 1, 1)) + 1) end end end " +
                    "FROM IdentificationUnitAnalysis AS T " +
                    "where t.IdentificationUnitID = #IdentificationUnitID# " +
                    "order by char(ascii(substring(T.AnalysisNumber, 1, 1))) desc ";//  select top 1 case when T.AnalysisNumber is null then '1' else cast(cast(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar) end FROM IdentificationUnitAnalysis AS T where T.IdentificationUnitID = #IdentificationUnitID# order by T.AnalysisNumber desc ";
                AnaNewStatus.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                SQL = "SELECT MIN(R.DisplayText) FROM Analysis A INNER JOIN AnalysisResult R ON A.AnalysisID = R.AnalysisID AND A.AnalysisID = 2 AND R.AnalysisResult IN ('-', '+', '2', '3', '4', '5', '6', '7', '8', '9', '99', 'D', 'E', 'I', 'K', 'U', 'Z')";
                AnaNewStatus.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                AnaNewStatus.setParentTable(TK25Unit);
                AnaNewStatus.DisplayText = "N. Status";
                AnaNewStatus.AddImage("", DiversityCollection.Resource.AnalysisHierarchy);
                SQL = "SELECT AnalysisResult AS Value, DisplayText AS Display " +
                    "FROM AnalysisResult WHERE AnalysisID = 2 AND AnalysisResult IN ('-', '+', '2', '3', '4', '5', '6', '7', '8', '9', '99', 'D', 'E', 'I', 'K', 'U', 'Z') ORDER BY Display";
                AnaNewStatus.DataColumns()["AnalysisResult"].SqlLookupSource = SQL;
                Sheet.AddDataTable(AnaNewStatus);

                //DiversityWorkbench.Spreadsheet.DataTable AnalysisLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
                //AnalysisLast.SetDescription("The last analysis of the organism");
                //AnalysisLast.setColorBack(TableColor(AnalysisLast.Name));
                //AnalysisLast.TemplateAlias = AnalysisLast.Alias();
                //System.Collections.Generic.List<string> RestAnalysisLast = new List<string>();
                //RestAnalysisLast.Add("AnalysisID");
                //AnalysisLast.RestrictionColumns = RestAnalysisLast;
                //SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                //    "FROM Analysis ORDER BY DisplayText";
                //AnalysisLast.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
                //AnalysisLast.SqlRestrictionClause = " exists (select * from [dbo].[IdentificationUnitAnalysis] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
                //    "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                //    "and " + TableAlias(Table) + ".AnalysisID = L.AnalysisID " +
                //    "group by L.CollectionSpecimenID, L.IdentificationUnitID, L.AnalysisID " +
                //    "having " + TableAlias(Table) + ".AnalysisNumber = max(L.AnalysisNumber))  ";
                //AnalysisLast.DataColumns()["AnalysisNumber"].DisplayText = "LastAnalysisNumber";
                //AnalysisLast.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
                //AnalysisLast.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                //SQL = "SELECT MIN(DisplayText) FROM Analysis";
                //AnalysisLast.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //AnalysisLast.setParentTable(TK25Unit);
                //AnalysisLast.DisplayText = "Last analy.";
                //AnalysisLast.SetDescription("Last analysis of the organism");
                //AnalysisLast.AddImage("", DiversityCollection.Resource.AnalysisHierarchy);
                //Sheet.AddDataTable(AnalysisLast);


            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void TK25AddGeoAnalysis(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Unit)
        {
            string Table = "IdentificationUnitGeoAnalysis";
            try
            {
                DiversityWorkbench.Spreadsheet.DataTable AnaLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Last geography", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                AnaLast.SetDescription("The last geographical position of the organism");
                AnaLast.setColorBack(TableColor(AnaLast.Name));
                AnaLast.TemplateAlias = AnaLast.Alias();
                AnaLast.SqlRestrictionClause = " exists (select * from [dbo].[IdentificationUnitGeoAnalysis] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
                    "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                    "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                    "having " + TableAlias(Table) + ".AnalysisDate = max(L.AnalysisDate))  ";
                //System.Collections.Generic.List<string> Us = new List<string>();
                //Us.Add("Geography");
                //AnaLast.setDisplayedColumns(Us);
                AnaLast.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                AnaLast.DataColumns()["AnalysisDate"].SqlQueryForDefaultForAdding = " select convert(nvarchar(20), getdate(), 120)";
                AnaLast.setParentTable(TK25Unit);
                AnaLast.DisplayText = "Last geo.";
                AnaLast.SetDescription("Last geographical position of the organism");
                AnaLast.AddImage("", DiversityCollection.Resource.Localisation);
                Sheet.AddDataTable(AnaLast);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void TK25AddUnitImage(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            ref DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "CollectionSpecimenImage";
            DiversityWorkbench.Spreadsheet.DataTable Image = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Image", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            Image.SetDescription("The image of the organism");
            Image.setParentTable(Unit);
            Image.setColorBack(TableColor(Unit.Name));
            System.Collections.Generic.List<string> Ca = new List<string>();
            Ca.Add("URI");
            Image.DataColumns()["URI"].IsHidden = true;
            Image.setDisplayedColumns(Ca);

            Image.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + Unit.Alias() + ".IdentificationUnitID = F.IdentificationUnitID " +
                " and " + TableAlias(Table) + ".IdentificationUnitID = F.IdentificationUnitID " +
                "group by F.IdentificationUnitID " +
                "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".IdentificationUnitID = min(f.IdentificationUnitID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
            Image.AddImage("", DiversityCollection.Resource.Icones);
            Sheet.AddDataTable(Image);
        }

#region Part related tables

        private static DiversityWorkbench.Spreadsheet.DataTable TK25AddUnitInPart(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            ref DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "IdentificationUnitInPart";
            DiversityWorkbench.Spreadsheet.DataTable UnitInPart = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            UnitInPart.SetDescription("The parts of a specimen, that contain an organism of the specimen");
            UnitInPart.setParentTable(Unit);
            UnitInPart.setColorBack(TableColor(UnitInPart.Name));
            UnitInPart.AddImage("", DiversityCollection.Resource.UnitInPart);
            Sheet.AddDataTable(UnitInPart);

            Table = "CollectionSpecimenPart";
            DiversityWorkbench.Spreadsheet.DataTable Part = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Part.SetDescription("The parts of a specimen");
            Part.setParentTable(UnitInPart);
            Part.setColorBack(TableColor(Part.Name));
            Part.AddImage("", DiversityCollection.Resource.Specimen);
            Sheet.AddDataTable(Part);

            Table = "Collection";
            DiversityWorkbench.Spreadsheet.DataTable Collection = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collection", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Collection.SetDescription("The collection where the parts are stored");
            Collection.setParentTable(Part);
            Collection.setColorBack(TableColor(Collection.Name));
            System.Collections.Generic.List<string> Cp = new List<string>();
            Cp.Add("CollectionName");
            Collection.setDisplayedColumns(Cp);
            Collection.AddImage("", DiversityCollection.Resource.Collection);
            Sheet.AddDataTable(Collection);

            return UnitInPart;
        }


#endregion

        private static void InitTableAliasForTK25()
        {
            _TableAlias = new Dictionary<string, string>();

            _TableAlias.Add("CollectionEvent", "A1_E");
            _TableAlias.Add("CollectionEventLocalisation", "A2_EL");

            _TableAlias.Add("CollectionSpecimen", "C0_S");
            _TableAlias.Add("CollectionProject", "C1_P");
            _TableAlias.Add("CollectionExternalDatasource", "C2_E");
            _TableAlias.Add("CollectionSpecimenReference", "C3_R");

            _TableAlias.Add("CollectionAgent", "D1_A");

            _TableAlias.Add("IdentificationUnit", "E0_U");
            _TableAlias.Add("Identification", "E1_I");
            _TableAlias.Add("IdentificationUnitAnalysis", "E2_UA");
            //_TableAlias.Add("IdentificationUnitAnalysis", "E2_UA_Status");
            _TableAlias.Add("IdentificationUnitGeoAnalysis", "E2_UGA");
            _TableAlias.Add("CollectionSpecimenImage", "E3_Im");

            _TableAlias.Add("IdentificationUnitInPart", "G0_UP");
            _TableAlias.Add("CollectionSpecimenPart", "G1_P");
            _TableAlias.Add("Collection", "G2_C");
        }
        
#endregion

#region WGS84

        private static DiversityWorkbench.Spreadsheet.Sheet WGS84Sheet()
        {
            DiversityWorkbench.Spreadsheet.Sheet Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: WGS84", "WGS84");
#if xxDEBUG
            sheetTarget = SheetTarget.WGS84;
#else
            _SheetTarget = SheetTarget.WGS84;
#endif
            InitTableAliasForWGS84();

            Sheet.setProjectSqlSoure("SELECT Project, ProjectID FROM ProjectListNotReadOnly ORDER BY Project");

            string Table = "CollectionEvent";
            DiversityWorkbench.Spreadsheet.DataTable Event = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
            try
            {
                Event.setColorBack(TableColor(Event.Name));
                System.Collections.Generic.List<string> Ce = new List<string>();
                Ce.Add("CollectionYear");
                Ce.Add("LocalityDescription");
                Event.DataColumns()["LocalityDescription"].DisplayText = "Locality";
                Event.DataColumns()["LocalityDescription"].Width = 100;
                Event.DataColumns()["CollectionYear"].DisplayText = "Year";
                Event.DataColumns()["CollectionYear"].Width = 40;

                Event.AddColumn("AverageYear", "case when [#TableAlias#].[CollectionEndYear] is null then [#TableAlias#].[CollectionYear] else ([#TableAlias#].[CollectionYear] + [#TableAlias#].[CollectionEndYear] )/2 end", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
                Event.DataColumns()["AverageYear"].setReadOnly(true);
                Event.DataColumns()["AverageYear"].Width = 40;
                Ce.Add("AverageYear");

                Event.setDisplayedColumns(Ce);

                Event.DataColumns()["CollectionDate"].setReadOnly(true);
                Event.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
                Event.DataColumns()["SeriesID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT [SeriesID] as Value " +
                ",case when [DateStart] is null then '' else convert(varchar(10), [DateStart], 120) + case when [DateEnd] is null or cast([DateStart] as varchar(10)) = cast([DateEnd] as varchar(10)) then '' else ' - ' + convert(varchar(10), [DateEnd], 120) end + ': ' end + " +
                "+ case when [SeriesCode] is null then '' else [SeriesCode] + '; ' end " +
                "+ case when [Description] is null then '' else substring([Description], 1, 50) end AS Display " +
                "FROM [dbo].[CollectionEventSeries] ORDER BY Display";
                Event.AddImage("", DiversityCollection.Resource.Event);
                Event.IsRequired = true;
                Sheet.AddDataTable(Event);

                // Adding WGS84
                WGS84AddWGS84(ref Sheet, ref Event);

                // Adding Specimen
                WGS84AddSpecimen(ref Sheet, ref Event);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return Sheet;
        }

        private static void WGS84AddWGS84(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable WGS84Event)
        {
            string Table = "CollectionEventLocalisation";
            DiversityWorkbench.Spreadsheet.DataTable WGS84 = new DiversityWorkbench.Spreadsheet.DataTable("A3_EL", Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            try
            {
                // Adding WGS84
                WGS84.setColorBack(TableColor(WGS84.Name));
                WGS84.DataColumns()["Location1"].DisplayText = "Long.";
                WGS84.DataColumns()["Location1"].Width = 40;
                WGS84.DataColumns()["Location2"].DisplayText = "Lat.";
                WGS84.DataColumns()["Location2"].Width = 40;
                WGS84.DataColumns()["Geography"].setReadOnly(true);
                WGS84.AddImage("", DiversityCollection.Resource.Localisation);
                System.Collections.Generic.List<string> RestrictWGS84 = new List<string>();
                RestrictWGS84.Add("LocalisationSystemID");
                WGS84.RestrictionColumns = RestrictWGS84;
                WGS84.DataColumns()["LocalisationSystemID"].RestrictionValue = "8";
                WGS84.setParentTable(WGS84Event);
                WGS84.AddImage("8", DiversityCollection.Specimen.ImageForLocalisationSystem(8));
                WGS84.DisplayText = "WGS84";
                WGS84.IsRequired = false;

                WGS84.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

                System.Collections.Generic.List<string> Cw = new List<string>();
                Cw.Add("Location1");
                Cw.Add("Location2");
                //Cw.Add("Geography");
                WGS84.setDisplayedColumns(Cw);

                //WGS84.IsRequired = true;

                Sheet.AddDataTable(WGS84);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void WGS84AddSpecimen(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable WGS84Event)
        {
            string Table = "CollectionSpecimen";
            DiversityWorkbench.Spreadsheet.DataTable Specimen = null;
            try
            {
                //Specimen = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
                Specimen = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.RootHidden, ref Sheet);
                Specimen.setParentTable(WGS84Event);
                Specimen.setColorBack(TableColor(Specimen.Name));
                Specimen.DataColumns()["ExternalDatasourceID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT ExternalDatasourceID AS Value, ExternalDatasourceName + CASE WHEN [ExternalDatasourceVersion] IS NULL THEN '' ELSE '(' + [ExternalDatasourceVersion] + ')' END AS Display " +
                    "FROM CollectionExternalDatasource ORDER BY Display ";
                Specimen.DataColumns()["CollectionEventID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT DISTINCT E.[CollectionEventID] AS Value " +
                ",case when [CollectionYear] is null then '' else cast([CollectionYear] as varchar) + case when [CollectionMonth] is null then '' else '-' + cast([CollectionMonth] as varchar) end + case when [CollectionDay] is null then '' else '-' + cast([CollectionDay] as varchar) end + ': ' end " +
                "+ case when [CollectorsEventNumber] is null then '' else [CollectorsEventNumber] + '; ' end " +
                "+ case when [LocalityDescription] is null then '' else substring([LocalityDescription], 1, 200)  end AS Display " +
                "FROM [dbo].[CollectionEvent] E , CollectionSpecimen S, CollectionSpecimenID_Available P " +
                "WHERE E.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = P.CollectionSpecimenID ORDER BY Display";
                Specimen.DataColumns()["DepositorsAgentURI"].setRemoteLinks("DepositorsName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                Specimen.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
                Specimen.DataColumns()["ReferenceURI"].IsOutdated = true;
                Specimen.DataColumns()["ReferenceTitle"].IsOutdated = true;
                Specimen.DataColumns()["ReferenceDetails"].IsOutdated = true;
                Specimen.AddImage("", DiversityCollection.Resource.CollectionSpecimen);
                Specimen.IsRequired = true;

                Sheet.AddDataTable(Specimen);

                DiversityCollection.Spreadsheet.Target.AddProjectTable(ref Sheet, Specimen);

                Sheet.MasterQueryColumn = Specimen.DataColumns()["CollectionSpecimenID"];

                WGS84AddExternalSource(ref Sheet, Specimen);

                WGS84AddAgent(ref Sheet, Specimen);

                WGS84AddUnit(ref Sheet, ref Specimen);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void WGS84AddAgent(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen)
        {
            string Table = "CollectionAgent";
            DiversityWorkbench.Spreadsheet.DataTable Agent = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collector", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
            Agent.setParentTable(Specimen);
            Agent.setColorBack(TableColor(Agent.Name));
            System.Collections.Generic.List<string> Ca = new List<string>();
            Ca.Add("CollectorsName");
            Agent.setDisplayedColumns(Ca);
            //DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            //string SQL = "select AgentUri from [UserProxy] U where U.LoginName = USER_NAME();";
            //string CurrentAgentUri = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //string CurrentAgentName = "";
            //if (CurrentAgentUri.Length == 0)
            //{
            //    SQL = "select AgentUri from [UserProxy] U where U.LoginName = SUSER_SNAME();";
            //    CurrentAgentUri = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //}
            //if (CurrentAgentUri.Length > 0)
            //{
            //    CurrentAgentName = A.AgentName(CurrentAgentUri, "Ii, G.");
            //    if (CurrentAgentName.Length == 0)
            //    {
            //        SQL = "select CombinedNameCache from [UserProxy] U where U.LoginName = USER_NAME();";
            //        CurrentAgentName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //        if (CurrentAgentName.Length == 0)
            //        {
            //            SQL = "select CombinedNameCache from [UserProxy] U where U.LoginName = SUSER_SNAME();";
            //            CurrentAgentName = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //        }
            //    }
            //}
            //Agent.DataColumns()["CollectorsAgentURI"].setRemoteLinks("CollectorsName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            //Agent.DataColumns()["CollectorsName"].ColumnDefault = CurrentAgentName;
            //Agent.DataColumns()["CollectorsAgentURI"].ColumnDefault = CurrentAgentUri;

            Agent.SqlRestrictionClause = " exists (select * from [CollectionAgent] F where " + TableAlias(Table) + ".CollectionSpecimenID = F.CollectionSpecimenID " +
                "group by F.CollectionSpecimenID " +
                "having " + TableAlias(Table) + ".CollectorsSequence = min(f.CollectorsSequence))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
            Agent.AddImage("", DiversityCollection.Resource.Agent);
            Sheet.AddDataTable(Agent);
        }

        private static void WGS84AddExternalSource(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen)
        {
            string Table = "CollectionExternalDatasource";
            DiversityWorkbench.Spreadsheet.DataTable Source = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Source", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Source.setParentTable(Specimen);
            Source.setColorBack(TableColor(Source.Name));
            System.Collections.Generic.List<string> Ca = new List<string>();
            Ca.Add("ExternalDatasourceName");
            Source.DataColumns()["ExternalDatasourceName"].setReadOnly(true);
            Source.setDisplayedColumns(Ca);
            Sheet.AddDataTable(Source);
        }


        private static void WGS84AddUnit(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable WGS84Specimen)
        {
            string Table = "IdentificationUnit";
            DiversityWorkbench.Spreadsheet.DataTable Unit = null;
            try
            {
                Unit = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Target, ref Sheet);
                Unit.setParentTable(WGS84Specimen);
                Unit.setColorBack(TableColor(Unit.Name));
                System.Collections.Generic.List<string> Us = new List<string>();
                Us.Add("FamilyCache");
                Us.Add("OrderCache");
                Us.Add("HierarchyCache");
                Us.Add("LastIdentificationCache");
                Us.Add("TaxonomicGroup");
                Unit.setDisplayedColumns(Us);
                Unit.DataColumns()["LastIdentificationCache"].DisplayText = "Taxon";
                Unit.DataColumns()["LastIdentificationCache"].Width = 150;
                Unit.DataColumns()["LastIdentificationCache"].setReadOnly(true);
                Unit.DataColumns()["FamilyCache"].IsHidden = true;
                Unit.DataColumns()["OrderCache"].IsHidden = true;
                Unit.DataColumns()["HierarchyCache"].IsHidden = true;
                Unit.DataColumns()["TaxonomicGroup"].IsHidden = true;
                Unit.AddImage("", DiversityCollection.Resource.Plant);
                Unit.IsRequired = true;
                Unit.DisplayText = "Observation";
                Sheet.AddDataTable(Unit);

                WGS84AddNewIdentification(ref Sheet, Unit);
                WGS84AddFirstIdentification(ref Sheet, Unit);
                WGS84AddLastIdentification(ref Sheet, Unit);

                WGS84AddAnalysis(ref Sheet, ref Unit);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void WGS84AddNewIdentification(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "Identification";
            DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, DiversityCollection.Spreadsheet.TargetText.Last_ident, DiversityWorkbench.Spreadsheet.DataTable.TableType.InsertOnly, ref Sheet);
            Ident.setParentTable(Unit);
            Ident.setColorBack(TableColor(Ident.Name));
            Ident.DisplayText = "New ident.";
            System.Collections.Generic.List<string> Is = new List<string>();
            Is.Add("TaxonomicName");
            Is.Add("NameURI");
            Ident.setDisplayedColumns(Is);
            Ident.DataColumns()["NameURI"].DisplayText = "New name";
            Ident.DataColumns()["TaxonomicName"].IsHidden = true;
            Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
            Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
                "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                "having " + TableAlias(Table) + ".IdentificationSequence > max(L.IdentificationSequence)) ";

            if (Specimen.DefaultUseIdentificationResponsible)
            {
                if (Specimen.DefaultUseCurrentUserAsDefault)
                {
                    Ident.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                    Ident.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                }
                else
                {
                    Ident.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                    Ident.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                }
            }

            // binding for DiversityTaxonNames
            DiversityWorkbench.Spreadsheet.RemoteColumnBinding FamilyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            FamilyCache.Column = Unit.DataColumns()["FamilyCache"];
            FamilyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            FamilyCache.RemoteParameter = "Family";

            DiversityWorkbench.Spreadsheet.RemoteColumnBinding OrderCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            OrderCache.Column = Unit.DataColumns()["OrderCache"];
            OrderCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            OrderCache.RemoteParameter = "Order";

            DiversityWorkbench.Spreadsheet.RemoteColumnBinding HierarchyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
            HierarchyCache.Column = Unit.DataColumns()["HierarchyCache"];
            HierarchyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
            HierarchyCache.RemoteParameter = "Hierarchy";

            System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding> TaxBindList = new List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding>();
            TaxBindList.Add(FamilyCache);
            TaxBindList.Add(OrderCache);
            TaxBindList.Add(HierarchyCache);
            System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteLink> TaxRemLinkList = new List<DiversityWorkbench.Spreadsheet.RemoteLink>();
            DiversityWorkbench.Spreadsheet.RemoteLink RemoteLinkTaxon = new DiversityWorkbench.Spreadsheet.RemoteLink(
                DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames,
                null,
                TaxBindList);
            TaxRemLinkList.Add(RemoteLinkTaxon);
            Ident.DataColumns()["NameURI"].setRemoteLinks("TaxonomicName", TaxRemLinkList);
            Ident.DataColumns()["NameURI"].DisplayText = "New name";
            System.Collections.Generic.List<string> NameURIFixSourceSetting = new List<string>();
            NameURIFixSourceSetting.Add("ModuleSource");
            NameURIFixSourceSetting.Add("Identification");
            NameURIFixSourceSetting.Add("TaxonomicGroup");
            NameURIFixSourceSetting.Add(Unit.DataColumns()["TaxonomicGroup"].DataTable().Alias() + "." + Unit.DataColumns()["TaxonomicGroup"].Name);
            Ident.DataColumns()["NameURI"].FixedSourceSetSetting(NameURIFixSourceSetting, true);
            //DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
            //Ident.DataColumns()["NameURI"].IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;

            Ident.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

            Ident.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
            Ident.DataColumns()["ReferenceURI"].IsOutdated = true;
            Ident.DataColumns()["ReferenceTitle"].IsOutdated = true;
            Ident.DataColumns()["ReferenceDetails"].IsOutdated = true;

            Ident.DataColumns()["TaxonomicName"].DefaultForAdding = "?";
            Ident.DataColumns()["IdentificationSequence"].SqlQueryForDefaultForAdding = "SELECT case when MAX(T.IdentificationSequence) is null then 1 else MAX(T.IdentificationSequence) + 1 end FROM Identification AS T WHERE T.IdentificationUnitID = #IdentificationUnitID# ";
            Ident.AddImage("", DiversityCollection.Resource.Identification);
            Sheet.AddDataTable(Ident);
        }

        private static void WGS84AddFirstIdentification(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "Identification";
            DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable("E1_IF", Table, DiversityCollection.Spreadsheet.TargetText.First_ident, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Ident.setParentTable(Unit);
            Ident.setColorBack(TableColor(Ident.Name));
            Ident.DisplayText = "First ident.";
            System.Collections.Generic.List<string> Is = new List<string>();
            Is.Add("TaxonomicName");
            Ident.setDisplayedColumns(Is);
            Ident.DataColumns()["TaxonomicName"].setReadOnly(true);
            Ident.DataColumns()["TaxonomicName"].DisplayText = "First name";
            Ident.DataColumns()["TaxonomicName"].Width = 150;
            Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
            Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where E1_IF.CollectionSpecimenID = L.CollectionSpecimenID " +
                "and E1_IF.IdentificationUnitID = L.IdentificationUnitID " +
                "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                "having E1_IF.IdentificationSequence = min(L.IdentificationSequence)) ";

            Ident.AddImage("", DiversityCollection.Resource.Identification);
            Sheet.AddDataTable(Ident);
        }

        private static void WGS84AddLastIdentification(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            string Table = "Identification";
            DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable("E1_IL", Table, DiversityCollection.Spreadsheet.TargetText.Last_ident, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Ident.setParentTable(Unit);
            Ident.setColorBack(TableColor(Ident.Name));
            Ident.DisplayText = "Last ident.";
            System.Collections.Generic.List<string> Is = new List<string>();
            //Is.Add("TaxonomicName");
            Is.Add("NameURI");
            Ident.setDisplayedColumns(Is);
            Ident.DataColumns()["TaxonomicName"].setReadOnly(true);
            Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
            Ident.DataColumns()["NameURI"].setReadOnly(true);
            Ident.DataColumns()["NameURI"].DisplayText = "Filter taxa";
            Ident.DataColumns()["NameURI"].setRemoteLinks("TaxonomicName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames);
            //Ident.DataColumns()["NameURI"].LinkedModule = DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames;
            Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where E1_IL.CollectionSpecimenID = L.CollectionSpecimenID " +
                "and E1_IL.IdentificationUnitID = L.IdentificationUnitID " +
                "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                "having E1_IL.IdentificationSequence = max(L.IdentificationSequence)) ";

            Ident.AddImage("", DiversityCollection.Resource.Identification);
            Sheet.AddDataTable(Ident);
        }

        private static void WGS84AddAnalysis(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable WGS84Unit)
        {
            string Table = "IdentificationUnitAnalysis";
            try
            {
                DiversityWorkbench.Spreadsheet.DataTable AnaLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                AnaLast.setColorBack(TableColor(AnaLast.Name));
                AnaLast.TemplateAlias = AnaLast.Alias();
                System.Collections.Generic.List<string> RestAnaLast = new List<string>();
                RestAnaLast.Add("AnalysisID");
                AnaLast.RestrictionColumns = RestAnaLast;
                string SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                    "FROM Analysis ORDER BY DisplayText";
                AnaLast.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
                AnaLast.SqlRestrictionClause = " exists (select * from [dbo].[IdentificationUnitAnalysis] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
                    "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                    "and " + TableAlias(Table) + ".AnalysisID = L.AnalysisID " +
                    "group by L.CollectionSpecimenID, L.IdentificationUnitID, L.AnalysisID " +
                    "having " + TableAlias(Table) + ".AnalysisNumber = max(L.AnalysisNumber))  ";
                System.Collections.Generic.List<string> Us = new List<string>();
                Us.Add("Result");
                AnaLast.setDisplayedColumns(Us);
                AnaLast.DataColumns()["AnalysisNumber"].DisplayText = "LastAnalysisNumber";
                AnaLast.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
                AnaLast.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                SQL = "SELECT MIN(DisplayText) FROM Analysis";
                AnaLast.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                AnaLast.setParentTable(WGS84Unit);
                AnaLast.DisplayText = "Last analy.";
                AnaLast.SetDescription("Last analysis of the organism");
                AnaLast.AddImage("", DiversityCollection.Resource.AnalysisHierarchy);
                Sheet.AddDataTable(AnaLast);

                DiversityWorkbench.Spreadsheet.DataTable AnaNew = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table) + "_N", Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                AnaNew.setColorBack(TableColor(AnaNew.Name));
                AnaNew.TemplateAlias = AnaNew.Alias();
                System.Collections.Generic.List<string> RestAnaNew = new List<string>();
                RestAnaNew.Add("AnalysisID");
                AnaNew.RestrictionColumns = RestAnaNew;
                SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
                    "FROM Analysis ORDER BY DisplayText";
                AnaNew.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
                AnaNew.SqlRestrictionClause = " " + TableAlias(Table) + "_N" + ".AnalysisNumber IS NULL  ";

                if (Specimen.DefaultUseAnalyisisResponsible)
                {
                    if (Specimen.DefaultUseCurrentUserAsDefault)
                    {
                        AnaNew.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                        AnaNew.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                    }
                    else
                    {
                        AnaNew.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                        AnaNew.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                    }
                }

                System.Collections.Generic.List<string> Un = new List<string>();
                Un.Add("Result");
                AnaNew.setDisplayedColumns(Un);
                AnaNew.DataColumns()["AnalysisNumber"].DisplayText = "NewAnalysisNumber";
                //AnaNew.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
                AnaNew.DataColumns()["AnalysisNumber"].SqlQueryForDefaultForAdding = "select top 1 case when try_Parse(T.AnalysisNumber as int) is null " +
                    "then '1' else cast(try_Parse(T.AnalysisNumber as int) + 1 as varchar) end " +
                    "FROM IdentificationUnitAnalysis AS T " +
                    "where t.IdentificationUnitID = #IdentificationUnitID# " +
                    "order by try_Parse(T.AnalysisNumber as int) desc ";//  select top 1 case when T.AnalysisNumber is null then '1' else cast(cast(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar) end FROM IdentificationUnitAnalysis AS T where T.IdentificationUnitID = #IdentificationUnitID# order by T.AnalysisNumber desc ";
                AnaNew.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                SQL = "SELECT MIN(DisplayText) FROM Analysis";
                AnaNew.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                AnaNew.setParentTable(WGS84Unit);
                AnaNew.DisplayText = "New analy.";
                AnaNew.SetDescription("New analysis of the organism");
                AnaNew.AddImage("", DiversityCollection.Resource.AnalysisHierarchy);
                Sheet.AddDataTable(AnaNew);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void InitTableAliasForWGS84()
        {
            _TableAlias = new Dictionary<string, string>();

            _TableAlias.Add("CollectionEvent", "A1_E");
            _TableAlias.Add("CollectionEventLocalisation", "A2_EL");

            _TableAlias.Add("CollectionSpecimen", "C0_S");
            _TableAlias.Add("CollectionProject", "C1_P");
            _TableAlias.Add("CollectionExternalDatasource", "C2_E");

            _TableAlias.Add("CollectionAgent", "D1_A");

            _TableAlias.Add("IdentificationUnit", "E0_U");
            _TableAlias.Add("Identification", "E1_I");
            _TableAlias.Add("IdentificationUnitAnalysis", "E2_UA");
        }

#endregion

#region Minerals

        private static DiversityWorkbench.Spreadsheet.Sheet MineralSheet()
        {
            DiversityWorkbench.Spreadsheet.Sheet Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Minerals", "Minerals");
#if xxDEBUG
            sheetTarget = SheetTarget.Minerals;
#else
            _SheetTarget = SheetTarget.Minerals;
#endif
            //InitTableAliasForTK25();
            _TableAlias = null;

            Sheet.setProjectSqlSoure("SELECT Project, ProjectID FROM ProjectListNotReadOnly ORDER BY Project");
            Sheet.setProjectReadOnlySqlSoure("SELECT P.Project, U.ProjectID " +
                "FROM dbo.ProjectUser U INNER JOIN " +
                "dbo.ProjectProxy P ON U.ProjectID = P.ProjectID " +
                "WHERE U.LoginName = USER_NAME() AND (U.[ReadOnly] = 1) " +
                "GROUP BY P.Project, U.ProjectID " +
                "ORDER BY P.Project");
            try
            {
                DiversityWorkbench.Spreadsheet.DataTable Event = Spreadsheet.Target.AddEventTables(ref Sheet);

                DiversityWorkbench.Spreadsheet.DataTable Specimen = Spreadsheet.Target.AddSpecimenTable(ref Sheet, Event, _SheetTarget);

                DiversityCollection.Spreadsheet.Target.AddProjectTable(ref Sheet, Specimen);

                DiversityCollection.Spreadsheet.Target.AddExternalDatasourceTable(ref Sheet, Specimen);

                DiversityCollection.Spreadsheet.Target.AddAgentTable(ref Sheet, Specimen);

                DiversityWorkbench.Spreadsheet.DataTable Part = DiversityCollection.Spreadsheet.Target.AddPartTable(ref Sheet, Specimen, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);
                DiversityWorkbench.Spreadsheet.DataTable Unit = DiversityCollection.Spreadsheet.Target.AddMineralTable(ref Sheet, Specimen, Part, DiversityWorkbench.Spreadsheet.DataTable.TableType.Target);

                DiversityCollection.Spreadsheet.Target.AddMineralTermTable(ref Sheet, Unit);

                DiversityWorkbench.Spreadsheet.DataTable Image = DiversityCollection.Spreadsheet.Target.AddImageTable(ref Sheet, Unit, DiversityWorkbench.Spreadsheet.DataTable.TableType.Single);

                DiversityCollection.Spreadsheet.Target.AddAnalysisTable(ref Sheet, Unit);

                DiversityCollection.Spreadsheet.Target.AddTransactionTable(ref Sheet, Part);

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return Sheet;
        }

        private static DiversityWorkbench.Spreadsheet.DataTable AddMineralTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Specimen,
            DiversityWorkbench.Spreadsheet.DataTable Part,
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type)
        {
            string Table = "IdentificationUnit";
            DiversityWorkbench.Spreadsheet.DataTable Unit = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Mineral", Type, ref Sheet);
            Unit.setParentTable(Specimen);
            Unit.setColorBack(TableColor(Unit.Name));
            System.Collections.Generic.List<string> Us = new List<string>();
            Us.Add("TaxonomicGroup");
            Unit.setDisplayedColumns(Us);
            Unit.DataColumns()["LastIdentificationCache"].DefaultForAdding = "?";
            Unit.DataColumns()["LastIdentificationCache"].setReadOnly(true);
            Unit.AddImage("", DiversityCollection.Specimen.TaxonImage(false, "mineral"));
            Sheet.AddDataTable(Unit);

            return Unit;
        }

        private static void AddMineralTermTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Unit)
        {
            try
            {
                string Table = "Identification";
                DiversityWorkbench.Spreadsheet.DataTable.TableType Type = DiversityWorkbench.Spreadsheet.DataTable.TableType.Single;
                DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, DiversityCollection.Spreadsheet.TargetText.Last_ident, Type, ref Sheet);
                Ident.SetDescription("The last identification of the mineral");
                Ident.setParentTable(Unit);
                Ident.setColorBack(TableColor(Ident.Name));
                System.Collections.Generic.List<string> Is = new List<string>();
                Is.Add("VernacularTerm");
                Is.Add("TermURI");
                Ident.setDisplayedColumns(Is);
                Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
                Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
                    "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
                    "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
                    "having " + TableAlias(Table) + ".IdentificationSequence = max(L.IdentificationSequence)) ";

                // binding for DiversityTaxonNames
                //DiversityWorkbench.Spreadsheet.RemoteColumnBinding FamilyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                //FamilyCache.Column = Unit.DataColumns()["FamilyCache"];
                //FamilyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
                //FamilyCache.RemoteParameter = "Family";

                //DiversityWorkbench.Spreadsheet.RemoteColumnBinding OrderCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                //OrderCache.Column = Unit.DataColumns()["OrderCache"];
                //OrderCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
                //OrderCache.RemoteParameter = "Order";

                //DiversityWorkbench.Spreadsheet.RemoteColumnBinding HierarchyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
                //HierarchyCache.Column = Unit.DataColumns()["HierarchyCache"];
                //HierarchyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
                //HierarchyCache.RemoteParameter = "Hierarchy";

                //System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding> TaxBindList = new List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding>();
                //TaxBindList.Add(FamilyCache);
                //TaxBindList.Add(OrderCache);
                //TaxBindList.Add(HierarchyCache);
                System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteLink> TaxRemLinkList = new List<DiversityWorkbench.Spreadsheet.RemoteLink>();
                DiversityWorkbench.Spreadsheet.RemoteLink RemoteLinkTaxon = new DiversityWorkbench.Spreadsheet.RemoteLink(
                    DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityScientificTerms,
                    null,
                    null);
                TaxRemLinkList.Add(RemoteLinkTaxon);
                Ident.DataColumns()["TermUri"].setRemoteLinks("VernacularTerm", TaxRemLinkList);

                Ident.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
                if (Specimen.DefaultUseIdentificationResponsible)
                {
                    if (Specimen.DefaultUseCurrentUserAsDefault)
                    {
                        Ident.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
                        Ident.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
                    }
                    else
                    {
                        Ident.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
                        Ident.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
                    }
                }

                Ident.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
                Ident.DataColumns()["ReferenceURI"].IsOutdated = true;
                Ident.DataColumns()["ReferenceTitle"].IsOutdated = true;
                Ident.DataColumns()["ReferenceDetails"].IsOutdated = true;

                Ident.DataColumns()["VernacularTerm"].DefaultForAdding = "?";
                Ident.DataColumns()["IdentificationSequence"].SqlQueryForDefaultForAdding = "SELECT case when MAX(T.IdentificationSequence) is null then 1 else MAX(T.IdentificationSequence) + 1 end FROM Identification AS T ";
                Ident.AddImage("", DiversityCollection.Resource.Identification);
                Sheet.AddDataTable(Ident);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }




#endregion

#region Collector

        private static DiversityWorkbench.Spreadsheet.Sheet CollectorSheet()
        {
            DiversityWorkbench.Spreadsheet.Sheet Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Collector", "Collector");
#if xxDEBUG
            sheetTarget = SheetTarget.Collector;
#else
            _SheetTarget = SheetTarget.Collector;
#endif

            Sheet.setProjectSqlSoure("SELECT Project, ProjectID FROM ProjectListNotReadOnly ORDER BY Project");
            Sheet.setProjectReadOnlySqlSoure("SELECT P.Project, U.ProjectID " +
                "FROM dbo.ProjectUser U INNER JOIN " +
                "dbo.ProjectProxy P ON U.ProjectID = P.ProjectID " +
                "WHERE U.LoginName = USER_NAME() AND (U.[ReadOnly] = 1) " +
                "GROUP BY P.Project, U.ProjectID " +
                "ORDER BY P.Project");

            try
            {
                DiversityWorkbench.Spreadsheet.DataTable Event = DiversityCollection.Spreadsheet.Target.AddEventTables(ref Sheet);

                DiversityWorkbench.Spreadsheet.DataTable Specimen = DiversityCollection.Spreadsheet.Target.AddSpecimenTable(ref Sheet, Event, SheetTarget.Collector);

                DiversityCollection.Spreadsheet.Target.AddProjectTable(ref Sheet, Specimen);

                DiversityCollection.Spreadsheet.Target.AddAgentTable(ref Sheet, Specimen, DiversityWorkbench.Spreadsheet.DataTable.TableType.Target);


            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }


            return Sheet;
        }


#endregion

#region CollectionTasks

        private static DiversityWorkbench.Spreadsheet.Sheet CollectionTaskSheet()
        {
            DiversityWorkbench.Spreadsheet.Sheet Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: Collection tasks", "Tasks");
            //sheetTarget = SheetTarget.CollectionTask;

            try
            {

                DiversityWorkbench.Spreadsheet.DataTable CollectionTask = DiversityCollection.Spreadsheet.Target.AddCollectionTaskTable(ref Sheet, DiversityWorkbench.Spreadsheet.DataTable.TableType.Root);

                DiversityWorkbench.Spreadsheet.DataTable Part = DiversityCollection.Spreadsheet.Target.AddTaskPartTable(ref Sheet, CollectionTask, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup);
                System.Collections.Generic.Dictionary<string, string> ForeignRelationToTask = new Dictionary<string, string>();
                ForeignRelationToTask.Add("SpecimenPartID", "SpecimenPartID");
                Part.setForeignRelationsToParent("CollectionTask", ForeignRelationToTask);

                DiversityWorkbench.Spreadsheet.DataTable Transaction = DiversityCollection.Spreadsheet.Target.AddTaskTransactionTable(ref Sheet, CollectionTask, DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup);
                System.Collections.Generic.Dictionary<string, string> ForeignRelationForTransactionToTask = new Dictionary<string, string>();
                ForeignRelationForTransactionToTask.Add("TransactionID", "TransactionID");
                Transaction.setForeignRelationsToParent("CollectionTask", ForeignRelationForTransactionToTask);

                DiversityCollection.Spreadsheet.Target.AddTaskImageTable(ref Sheet, CollectionTask);

                Sheet.MasterQueryColumn = CollectionTask.DataColumns()["CollectionTaskID"];

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }


            return Sheet;
        }

        private static DiversityWorkbench.Spreadsheet.DataTable AddCollectionTaskTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type)
        {
            string Table = "CollectionTask";
            DiversityWorkbench.Spreadsheet.DataTable CollectionTask = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collection Task", Type, ref Sheet);
            CollectionTask.setColorBack(TableColor(CollectionTask.Name));

            string SQL = "SELECT NULL AS Value, NULL AS Display  " +
                "UNION " +
                "SELECT TransactionID AS Value, TransactionTitle AS Display " +
                "FROM [Transaction] ORDER BY Display";
            CollectionTask.DataColumns()["TransactionID"].SqlLookupSource = SQL;

            SQL = "SELECT NULL AS Value, NULL AS Display  " +
                "UNION " +
                "SELECT CollectionID AS Value, CollectionName AS Display " +
                "FROM Collection ORDER BY Display";
            CollectionTask.DataColumns()["CollectionID"].SqlLookupSource = SQL;

            CollectionTask.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

            CollectionTask.AddImage("", DiversityCollection.Specimen.ImageForTable(Table, false));
            Sheet.AddDataTable(CollectionTask);

            Table = "Task";
            DiversityWorkbench.Spreadsheet.DataTable Task = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Task", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Task.SetDescription("The task where details of the collection tasks are defined");
            Task.setParentTable(CollectionTask);
            Task.setColorBack(TableColor(Task.Name));
            System.Collections.Generic.List<string> Cp = new List<string>();
            Cp.Add("DisplayText");
            Task.setDisplayedColumns(Cp);
            Task.AddImage("", DiversityCollection.Specimen.ImageForTable(Table, false));
            Sheet.AddDataTable(Task);


            return CollectionTask;
        }


        private static void AddTaskImageTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable CollectionTask)
        {
            try
            {
                string Table = "CollectionTaskImage";
                DiversityWorkbench.Spreadsheet.DataTable Image = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Image", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
                Image.SetDescription("The image of the collection task");
                Image.setParentTable(CollectionTask);
                Image.setColorBack(TableColor(CollectionTask.Name));
                System.Collections.Generic.List<string> Ca = new List<string>();
                Ca.Add("URI");
                Image.DataColumns()["URI"].IsHidden = true;
                Image.setDisplayedColumns(Ca);

                Image.SqlRestrictionClause = " exists (select * from [CollectionTaskImage] F where " + CollectionTask.Alias() + ".CollectionTaskID = F.CollectionTaskID " +
                    " and " + TableAlias(Table) + ".CollectionTaskID = F.CollectionTaskID " +
                    "group by F.CollectionTaskID " +
                    "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".CollectionTaskID = min(f.CollectionTaskID))"; 
                Image.AddImage("", DiversityCollection.Resource.Icones);
                Sheet.AddDataTable(Image);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private static DiversityWorkbench.Spreadsheet.DataTable AddTaskPartTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Task,
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type)
        {
            string Table = "CollectionSpecimenPart";
            DiversityWorkbench.Spreadsheet.DataTable Part = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Part", Type, ref Sheet);
            switch (Type)
            {
                case DiversityWorkbench.Spreadsheet.DataTable.TableType.Single:
                case DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup:
                    switch (Sheet.Target())
                    {
                        case "Image":
                            Part.SetDescription("The first part contained in the image");
                            Part.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F " +
                                "where " + TableAlias(Table) + ".SpecimenPartID = F.SpecimenPartID " +
                                "group by F.SpecimenPartID " +
                                "having " + TableAlias(Table) + ".SpecimenPartID = min(f.SpecimenPartID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
                            break;
                        //case "Organisms":
                        //    Part.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + _TableAlias["IdentificationUnit"] + ".IdentificationUnitID = F.IdentificationUnitID " +
                        //        " and " + TableAlias(Table) + ".IdentificationUnitID = F.IdentificationUnitID " +
                        //        "group by F.IdentificationUnitID " +
                        //        "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".IdentificationUnitID = min(f.IdentificationUnitID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
                        //    break;
                        default:
                            Part.SetDescription("The first part of the specimen");
                            Part.SqlRestrictionClause = " exists(select * from [CollectionSpecimenPart] F where F.CollectionSpecimenID = " + TableAlias(Table) + ".CollectionSpecimenID " +
                                "group by F.CollectionSpecimenID " +
                                "having " + TableAlias(Table) + ".SpecimenPartID = min(F.SpecimenPartID)) ";
                            break;
                    }



                    //Part.SqlRestrictionClause = " exists(select * from [CollectionSpecimenPart] F where F.CollectionSpecimenID = " + TableAlias(Table) + ".CollectionSpecimenID " +
                    //    "group by F.CollectionSpecimenID " +
                    //    "having " + TableAlias(Table) + ".SpecimenPartID = min(F.SpecimenPartID)) ";
                    break;
                case DiversityWorkbench.Spreadsheet.DataTable.TableType.Target:
                    break;
            }
            Part.setParentTable(Task);
            Part.setColorBack(TableColor(Part.Name));
            System.Collections.Generic.List<string> Cp = new List<string>();
            Cp.Add("StorageLocation");
            //Cp.Add("CollectionID");
            Part.setDisplayedColumns(Cp);
            //Part.DataColumns()["StorageLocation"].DisplayText = "Stor.Loc.";

            string SQL = "SELECT NULL AS Value, NULL AS Display  " +
                "UNION " +
                "SELECT CollectionID AS Value, CollectionName AS Display " +
                "FROM Collection ORDER BY Display";
            Part.DataColumns()["CollectionID"].SqlLookupSource = SQL;
            Part.DataColumns()["DerivedFromSpecimenPartID"].InternalRelationDisplay = "LTRIM(CASE WHEN StorageLocation IS NULL THEN '' ELSE StorageLocation END + CASE WHEN AccessionNumber <> '' THEN ' Nr.: ' + AccessionNumber ELSE '' END)";
            Part.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Part.AddImage("", DiversityCollection.Resource.Specimen);
            Sheet.AddDataTable(Part);

            Table = "Collection";
            DiversityWorkbench.Spreadsheet.DataTable Collection = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collection", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
            Collection.SetDescription("The collection containing the part of the specimen");
            Collection.setParentTable(Part);
            Collection.setColorBack(TableColor(Collection.Name));
            Collection.AddImage("", DiversityCollection.Resource.Collection);
            if (Type == DiversityWorkbench.Spreadsheet.DataTable.TableType.Target)
            {
                System.Collections.Generic.List<string> CC = new List<string>();
                Collection.DataColumns()["CollectionName"].DisplayText = "Name";
                CC.Add("CollectionName");
                Collection.setDisplayedColumns(CC);
            }
            Sheet.AddDataTable(Collection);

            return Part;
        }


        private static DiversityWorkbench.Spreadsheet.DataTable AddTaskTransactionTable(
            ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
            DiversityWorkbench.Spreadsheet.DataTable Task,
            DiversityWorkbench.Spreadsheet.DataTable.TableType Type)
        {
            string Table = "Transaction";
            DiversityWorkbench.Spreadsheet.DataTable Transaction = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Transaction", Type, ref Sheet);
            Transaction.SetDescription("The first transaction");
            Transaction.SqlRestrictionClause = " exists(select * from [Transaction] F where F.TransactionID = " + TableAlias(Table) + ".TransactionID " +
                "group by F.TransactionID " +
                "having " + TableAlias(Table) + ".TransactionID = min(F.TransactionID)) ";
            Transaction.setParentTable(Task);
            Transaction.setColorBack(TableColor(Transaction.Name));
            System.Collections.Generic.List<string> Cp = new List<string>();
            Cp.Add("TransactionTitle");
            Transaction.setDisplayedColumns(Cp);

            string SQL = "SELECT NULL AS Value, NULL AS Display  " +
                "UNION " +
                "SELECT CollectionID AS Value, CollectionName AS Display " +
                "FROM Collection ORDER BY Display";
            Transaction.DataColumns()["AdministratingCollectionID"].SqlLookupSource = SQL;
            Transaction.DataColumns()["FromCollectionID"].SqlLookupSource = SQL;
            Transaction.DataColumns()["ToCollectionID"].SqlLookupSource = SQL;
            Transaction.DataColumns()["FromTransactionPartnerAgentURI"].setRemoteLinks("FromTransactionPartnerName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Transaction.DataColumns()["ToTransactionPartnerAgentURI"].setRemoteLinks("ToTransactionPartnerName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Transaction.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
            Transaction.AddImage("", DiversityCollection.Resource.Transaction);
            Sheet.AddDataTable(Transaction);

            return Transaction;
        }



#endregion


    }
}
