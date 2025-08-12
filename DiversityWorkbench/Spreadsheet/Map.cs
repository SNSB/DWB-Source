using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public class Map
    {
        public enum MapType { WGS84, TK25, Map, Undefined }

        private DiversityWorkbench.Spreadsheet.Sheet _Sheet;

        public Map(DiversityWorkbench.Spreadsheet.Sheet Sheet)
        {
            this._Sheet = Sheet;
        }


        //public DiversityWorkbench.Spreadsheet.Setting Setting()
        //{
        //    if (this._Setting == null)
        //    {
        //        this._Setting = new Spreadsheet.Setting(ref this._Sheet);
        //    }
        //    return this._Setting;
        //}

        /// <summary>
        /// Getting a list of the geographial objects according to MapKeyObjects
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> GeoObjectsFiltered(
            int iGeographyPosition,
            int iGeographyKeyPosition,
            int iGeographySymbolPosition,
            int iGeographyColorPosition,
            Sheet Sheet,
            bool ShowDetailsInMap = true,
            MapType MapType = MapType.Undefined,
            System.Collections.Generic.Dictionary<string, string> DisplayedColumns = null)
        {
            this._Sheet = Sheet;
            System.Collections.Generic.Dictionary<string, MapKeyObject> MapKeyObjects = new Dictionary<string, MapKeyObject>();

            // Testing every row
            foreach (System.Data.DataRow R in this._Sheet.DT().Rows)
            {
                // if no geography available
                if (iGeographyKeyPosition < 0 || iGeographyKeyPosition > R.Table.Columns.Count || R[iGeographyKeyPosition].Equals(System.DBNull.Value))
                    continue;
                try
                {
                    // the key for the object
                    string Key = R[iGeographyKeyPosition].ToString();
                    string ValueOfSymbol = R[iGeographySymbolPosition].ToString();
                    if (this._Sheet.MapSymbols().ContainsKey(ValueOfSymbol))
                    {
                        MapSymbol MS = this._Sheet.MapSymbols()[ValueOfSymbol];
                        if (MS.IsExcluded)
                            continue;
                    }
                    if (MapKeyObjects.ContainsKey(Key))
                    {
                        // Object already present - so it may have to be changed
                        if (MapType == MapType.TK25)
                            MapKeyObjects[Key].EvaluateContent(R, DisplayedColumns);
                        else
                            MapKeyObjects[Key].UpdateContent(R);
                    }
                    else
                    {
                        // Object missing so far - so a new one will be created
                        string Value = "";
                        if (iGeographySymbolPosition > -1)
                            Value = R[iGeographySymbolPosition].ToString();
                        if (!this._Sheet.MapSymbols().ContainsKey(Value) && this._Sheet.MapSymbols().ContainsKey(Value.ToUpper()))
                            Value = Value.ToUpper();
                        if (!this._Sheet.MapSymbols().ContainsKey(Value) && this._Sheet.MapSymbols().ContainsKey(Value.ToLower()))
                            Value = Value.ToLower();
                        if (iGeographySymbolPosition > -1 && !this._Sheet.MapSymbols().ContainsKey(Value))
                            continue;
                        MapKeyObject MFO = new MapKeyObject(Key, this._Sheet, R, iGeographyPosition, iGeographySymbolPosition, iGeographyColorPosition);
                        MapKeyObjects.Add(Key, MFO);
                        if (MapType == MapType.Undefined)
                            MapKeyObjects[Key].UpdateContent(R);
                        else if (MapType == MapType.TK25)
                        {
                            if (!DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityGazetteer"))
                            {
                                DiversityWorkbench.Gazetteer G = new Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                            }
                            MapKeyObjects[Key].EvaluateContent(R, DisplayedColumns);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Objects = new List<WpfSamplingPlotPage.GeoObject>();
            foreach (System.Collections.Generic.KeyValuePair<string, MapKeyObject> KV in MapKeyObjects)
            {
                try
                {
                    if (KV.Value.GeographyValue() == null)
                    {
                        //iMissingGeography++;
                        continue;
                    }
                    if (KV.Value.SybolValue() == null)
                        continue;
                    if (KV.Value.SybolValue() != "")
                    {
                        string Val = KV.Value.SybolValue();
                    }
                    WpfSamplingPlotPage.GeoObject GO = new WpfSamplingPlotPage.GeoObject();
                    if (this._Sheet.ShowDetailsInMap)
                        GO.DisplayText = KV.Value.InfoDisplayText(DisplayedColumns);
                    else
                        GO.DisplayText = KV.Value.Key();
                    GO.Identifier = KV.Value.Key();
                    GO.GeometryData = KV.Value.GeographyValue();
                    MapColor MC = this._Sheet.getMapColor(KV.Value.ColorValue());
                    GO.FillBrush = MC.Brush;
                    GO.StrokeBrush = MC.Brush;
                    GO.StrokeTransparency = this._Sheet.GeographyTransparency;
                    string SymbolValue = KV.Value.SybolValue();
                    if (!this._Sheet.MapSymbols().ContainsKey(SymbolValue))
                        SymbolValue = SymbolValue.ToUpper();
                    if (!this._Sheet.MapSymbols().ContainsKey(SymbolValue))
                        SymbolValue = SymbolValue.ToLower();
                    MapSymbol MS = null;
                    if (!this._Sheet.MapSymbols().ContainsKey(SymbolValue))
                    {
                        MS = new MapSymbol("", this._Sheet.GeographySymbolSize, "Circle filled");
                        MS.SymbolFilled = true;
                    }
                    else if (this._Sheet.MapSymbols().ContainsKey(SymbolValue))
                    {
                        MS = this._Sheet.MapSymbols()[SymbolValue];
                    }
                    if (MS.Symbol == WpfSamplingPlotPage.PointSymbol.None && MS.SymbolTitle == "Hide")
                        continue;
                    if (KV.Value.SybolValue().Length == 0 && iGeographySymbolPosition > -1)
                        MS = this._Sheet.MapSymbolForMissing;
                    GO.PointType = MS.Symbol;
                    if (MS.SymbolFilled)
                    {
                        GO.FillTransparency = this._Sheet.GeographyTransparency;
                        GO.StrokeThickness = 0;
                    }
                    else
                    {
                        GO.FillTransparency = 0;
                        GO.StrokeThickness = MS.SymbolSize;// this._Sheet.GeographyStrokeThickness;
                    }
                    GO.PointSymbolSize = MS.SymbolSize;
                    Objects.Add(GO);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return Objects;
        }

        //#region TK25

        //private static DiversityWorkbench.Spreadsheet.Sheet TK25Sheet()
        //{
        //    DiversityWorkbench.Spreadsheet.Sheet Sheet = new DiversityWorkbench.Spreadsheet.Sheet("DiversityCollection: TK25", "TK25");
        //    //_SheetTarget = SheetTarget.TK25;
        //    InitTableAliasForTK25();

        //    Sheet.setProjectSqlSoure("SELECT Project, ProjectID FROM ProjectListNotReadOnly ORDER BY Project");
        //    Sheet.setProjectReadOnlySqlSoure("SELECT P.Project, U.ProjectID " +
        //        "FROM dbo.ProjectUser U INNER JOIN " +
        //        "dbo.ProjectProxy P ON U.ProjectID = P.ProjectID " +
        //        "WHERE U.LoginName = USER_NAME() AND (U.[ReadOnly] = 1) " +
        //        "GROUP BY P.Project, U.ProjectID " +
        //        "ORDER BY P.Project");

        //    string Table = "CollectionEvent";
        //    DiversityWorkbench.Spreadsheet.DataTable Event = new DiversityWorkbench.Spreadsheet.DataTable("B0_E", Table, "Event", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
        //    try
        //    {
        //        Event.SetDescription("The collection event during which the organisms had been observed or collected");
        //        Event.setColorBack(System.Drawing.Color.Blue);// TableColor(Event.Name));
        //        System.Collections.Generic.List<string> Ce = new List<string>();
        //        Ce.Add("CollectionYear");
        //        Ce.Add("LocalityDescription");
        //        Event.DataColumns()["LocalityDescription"].DisplayText = "Locality";
        //        Event.DataColumns()["LocalityDescription"].Width = 100;
        //        Event.DataColumns()["CollectionYear"].DisplayText = "Year";
        //        Event.DataColumns()["CollectionYear"].Width = 40;

        //        Event.AddColumn("AverageYear", "cast(cast(case when [#TableAlias#].[CollectionEndYear] is null then [#TableAlias#].[CollectionYear] else case when [#TableAlias#].[CollectionYear] is null then [#TableAlias#].[CollectionEndYear] else ([#TableAlias#].[CollectionYear] + [#TableAlias#].[CollectionEndYear] )/2 end end as int) as varchar(4))", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
        //        Event.DataColumns()["AverageYear"].setReadOnly(true);
        //        Event.DataColumns()["AverageYear"].Column.DataType = Event.DataColumns()["CollectionYear"].Column.DataType;
        //        Event.DataColumns()["AverageYear"].Width = 40;
        //        Ce.Add("AverageYear");

        //        Event.setDisplayedColumns(Ce);

        //        Event.DataColumns()["CollectionDate"].setReadOnly(true);
        //        Event.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
        //        Event.DataColumns()["SeriesID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT [SeriesID] as Value " +
        //        ",case when [DateStart] is null then '' else convert(varchar(10), [DateStart], 120) + case when [DateEnd] is null or cast([DateStart] as varchar(10)) = cast([DateEnd] as varchar(10)) then '' else ' - ' + convert(varchar(10), [DateEnd], 120) end + ': ' end + " +
        //        "+ case when [SeriesCode] is null then '' else [SeriesCode] + '; ' end " +
        //        "+ case when [Description] is null then '' else substring([Description], 1, 50) end AS Display " +
        //        "FROM [dbo].[CollectionEventSeries] ORDER BY Display";
        //        Event.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Event);
        //        Event.IsRequired = true;
        //        Sheet.AddDataTable(Event);

        //        // Adding TK25
        //        TK25AddTK25(ref Sheet, ref Event);
        //        // Adding WGS84
        //        TK25AddWGS84(ref Sheet, ref Event);

        //        // Adding Specimen
        //        TK25AddSpecimen(ref Sheet, ref Event);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }

        //    return Sheet;
        //}

        //private static void TK25AddTK25(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Event)
        //{
        //    string Table = "CollectionEventLocalisation";
        //    DiversityWorkbench.Spreadsheet.DataTable TK25 = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "TK25", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
        //    try
        //    {
        //        // Adding TK25
        //        TK25.SetDescription("The topographical map (1:25000) and the quadrant where the organisms where collected or observed");
        //        TK25.setColorBack(System.Drawing.Color.Black); // TableColor(TK25.Name));
        //        TK25.DataColumns()["Location1"].DisplayText = "TK25";
        //        TK25.DataColumns()["Location1"].Width = 40;
        //        TK25.DataColumns()["Location2"].DisplayText = "Quad.";
        //        TK25.DataColumns()["Location2"].Width = 40;
        //        TK25.DataColumns()["Geography"].setReadOnly(true);
        //        TK25.DataColumns()["Geography"].Width = 50;
        //        TK25.DataColumns()["Geography"].DisplayText = "Center";
        //        TK25.DataColumns()["Geography"].SqlForColumn = " case when LEN([#TableAlias#].[Location2]) = 1 then [#TableAlias#].[Geography].EnvelopeCenter().ToString() else null end ";
        //        TK25.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.MTB);
        //        System.Collections.Generic.List<string> RestrictTK25 = new List<string>();
        //        RestrictTK25.Add("LocalisationSystemID");
        //        TK25.RestrictionColumns = RestrictTK25;
        //        TK25.DataColumns()["LocalisationSystemID"].RestrictionValue = "3";
        //        TK25.setParentTable(TK25Event);
        //        TK25.AddImage("3", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Specimen.ImageForLocalisationSystem(3));
        //        TK25.DisplayText = "TK25";
        //        TK25.IsRequired = true;

        //        TK25.AddColumn("SymbolSize", "case when len([#TableAlias#].[Location2]) = 0 then 1 else cast(1 as float)/power(2, len([#TableAlias#].[Location2])) end", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
        //        TK25.DataColumns()["SymbolSize"].setReadOnly(true);

        //        TK25.AddColumn("TK25/Quad.", "case when len([#TableAlias#].[Location2]) = 0 then NULL else [#TableAlias#].Location1 + '/' + substring([#TableAlias#].[Location2], 1, 1) end", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
        //        TK25.DataColumns()["TK25/Quad."].setReadOnly(true);
        //        TK25.DataColumns()["TK25/Quad."].Width = 60;
        //        TK25.DataColumns()["TK25/Quad."].Column.DataType = "nvarchar(6)";

        //        TK25.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

        //        System.Collections.Generic.List<string> Cw = new List<string>();
        //        Cw.Add("Location1");
        //        Cw.Add("Location2");
        //        Cw.Add("TK25/Quad.");
        //        //Cw.Add("SymbolSize");
        //        //Cw.Add("Geography");
        //        TK25.setDisplayedColumns(Cw);

        //        TK25.IsRequired = true;

        //        Sheet.AddDataTable(TK25);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private static void TK25AddWGS84(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Event)
        //{
        //    string Table = "CollectionEventLocalisation";
        //    DiversityWorkbench.Spreadsheet.DataTable WGS84 = new DiversityWorkbench.Spreadsheet.DataTable("A2_EWGS", Table, "WGS84", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
        //    try
        //    {
        //        // Adding TK25
        //        WGS84.SetDescription("The WGS84 coordinates of the collection event");
        //        WGS84.setColorBack(System.Drawing.Color.Black); // TableColor(WGS84.Name));
        //        WGS84.DataColumns()["Geography"].Width = 70;
        //        WGS84.DataColumns()["Geography"].DisplayText = "Geography";
        //        WGS84.DataColumns()["Geography"].setReadOnly(true);
        //        WGS84.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Localisation);
        //        System.Collections.Generic.List<string> RestrictWGS84 = new List<string>();
        //        RestrictWGS84.Add("LocalisationSystemID");
        //        WGS84.RestrictionColumns = RestrictWGS84;
        //        WGS84.DataColumns()["LocalisationSystemID"].RestrictionValue = "8";
        //        WGS84.setParentTable(TK25Event);
        //        WGS84.AddImage("8", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Specimen.ImageForLocalisationSystem(8));
        //        WGS84.DisplayText = "WGS84";
        //        WGS84.IsRequired = false;

        //        WGS84.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

        //        WGS84.AddColumn("Center", "[#TableAlias#].[Geography].EnvelopeCenter().ToString()", DiversityWorkbench.Spreadsheet.DataColumn.RetrievalType.ViewOnly);
        //        WGS84.DataColumns()["Center"].setReadOnly(true);
        //        WGS84.DataColumns()["Center"].Width = 60;

        //        Sheet.AddDataTable(WGS84);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private static void TK25AddSpecimen(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Event)
        //{
        //    string Table = "CollectionSpecimen";
        //    DiversityWorkbench.Spreadsheet.DataTable Specimen = null;
        //    try
        //    {
        //        Specimen = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Specimen", DiversityWorkbench.Spreadsheet.DataTable.TableType.Root, ref Sheet);
        //        //Specimen = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.RootHidden, ref Sheet);
        //        Specimen.SetDescription("The specimen containing the organisms");
        //        Specimen.setParentTable(TK25Event);
        //        Specimen.setColorBack(System.Drawing.Color.Black); // TableColor(Specimen.Name));
        //        Specimen.DataColumns()["ExternalDatasourceID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT ExternalDatasourceID AS Value, ExternalDatasourceName + CASE WHEN [ExternalDatasourceVersion] IS NULL THEN '' ELSE '(' + [ExternalDatasourceVersion] + ')' END AS Display " +
        //            "FROM CollectionExternalDatasource ORDER BY Display ";
        //        Specimen.DataColumns()["CollectionEventID"].SqlLookupSource = "SELECT NULL AS Value, NULL AS Display UNION SELECT DISTINCT E.[CollectionEventID] AS Value " +
        //        ",case when [CollectionYear] is null then '' else cast([CollectionYear] as varchar) + case when [CollectionMonth] is null then '' else '-' + cast([CollectionMonth] as varchar) end + case when [CollectionDay] is null then '' else '-' + cast([CollectionDay] as varchar) end + ': ' end " +
        //        "+ case when [CollectorsEventNumber] is null then '' else [CollectorsEventNumber] + '; ' end " +
        //        "+ case when [LocalityDescription] is null then '' else substring([LocalityDescription], 1, 200)  end AS Display " +
        //        "FROM [dbo].[CollectionEvent] E , CollectionSpecimen S, CollectionSpecimenID_Available P " +
        //        "WHERE E.CollectionEventID = S.CollectionEventID AND S.CollectionSpecimenID = P.CollectionSpecimenID ORDER BY Display";
        //        Specimen.DataColumns()["DepositorsAgentURI"].setRemoteLinks("DepositorsName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
        //        Specimen.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);
        //        Specimen.DataColumns()["ExsiccataURI"].setRemoteLinks("ExsiccataAbbreviation", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityExsiccatae);
        //        Specimen.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.CollectionSpecimen);
        //        Specimen.IsRequired = true;

        //        Sheet.AddDataTable(Specimen);

        //        Map.AddProjectTable(ref Sheet, Specimen);

        //        Sheet.MasterQueryColumn = Specimen.DataColumns()["CollectionSpecimenID"];

        //        TK25AddExternalSource(ref Sheet, Specimen);

        //        TK25AddAgent(ref Sheet, Specimen);

        //        TK25AddUnit(ref Sheet, ref Specimen);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private static void AddProjectTable(
        //    ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
        //    DiversityWorkbench.Spreadsheet.DataTable Specimen)
        //{
        //    string Table = "CollectionProject";
        //    DiversityWorkbench.Spreadsheet.DataTable Project = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Project, ref Sheet);
        //    Project.setParentTable(Specimen);
        //    Project.SqlRestrictionClause = " ProjectID IN (SELECT ProjectID FROM ProjectListNotReadOnly) ";
        //    Project.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Project);
        //    Sheet.AddDataTable(Project);
        //}



        //private static void TK25AddAgent(
        //    ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
        //    DiversityWorkbench.Spreadsheet.DataTable Specimen)
        //{
        //    string Table = "CollectionAgent";
        //    DiversityWorkbench.Spreadsheet.DataTable Agent = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collector", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
        //    Agent.SetDescription("The collector or observer of the organisms");
        //    Agent.setParentTable(Specimen);
        //    Agent.setColorBack(System.Drawing.Color.Black); // TableColor(Agent.Name));
        //    System.Collections.Generic.List<string> Ca = new List<string>();
        //    Ca.Add("CollectorsName");
        //    Agent.setDisplayedColumns(Ca);
        //    Agent.DataColumns()["CollectorsAgentURI"].setRemoteLinks("CollectorsName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
        //    Agent.SqlRestrictionClause = " exists (select * from [CollectionAgent] F where " + TableAlias(Table) + ".CollectionSpecimenID = F.CollectionSpecimenID " +
        //        "group by F.CollectionSpecimenID " +
        //        "having " + TableAlias(Table) + ".CollectorsSequence = min(f.CollectorsSequence) and " + TableAlias(Table) + ".CollectorsName = min(f.CollectorsName))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
        //    Agent.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Agent);

        //    //System.Collections.Generic.List<string> AgentURIFixSourceSetting = new List<string>();
        //    //AgentURIFixSourceSetting.Add("ModuleSource");
        //    //AgentURIFixSourceSetting.Add("CollectionAgent");
        //    //Agent.DataColumns()["CollectorsAgentURI"].SetFixSourceSetting(AgentURIFixSourceSetting, true);

        //    Sheet.AddDataTable(Agent);
        //}

        //private static void TK25AddExternalSource(
        //    ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
        //    DiversityWorkbench.Spreadsheet.DataTable Specimen)
        //{
        //    string Table = "CollectionExternalDatasource";
        //    DiversityWorkbench.Spreadsheet.DataTable Source = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Source", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
        //    Source.SetDescription("The source of the data, e.g. an external database imported into DiversityCollection");
        //    Source.setParentTable(Specimen);
        //    Source.setColorBack(System.Drawing.Color.Black); // TableColor(Source.Name));
        //    Source.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Import);
        //    System.Collections.Generic.List<string> Ca = new List<string>();
        //    Ca.Add("ExternalDatasourceName");
        //    Source.DataColumns()["ExternalDatasourceName"].setReadOnly(true);
        //    Source.setDisplayedColumns(Ca);
        //    Sheet.AddDataTable(Source);
        //}

        //private static void TK25AddUnit(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Specimen)
        //{
        //    string Table = "IdentificationUnit";
        //    DiversityWorkbench.Spreadsheet.DataTable Unit = null;
        //    try
        //    {
        //        Unit = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Target, ref Sheet);
        //        Unit.SetDescription("The organism collected or observed");
        //        Unit.setParentTable(TK25Specimen);
        //        Unit.setColorBack(System.Drawing.Color.Black); // TableColor(Unit.Name));
        //        System.Collections.Generic.List<string> Us = new List<string>();
        //        Us.Add("FamilyCache");
        //        Us.Add("OrderCache");
        //        Us.Add("HierarchyCache");
        //        Us.Add("LastIdentificationCache");
        //        Us.Add("TaxonomicGroup");
        //        Unit.setDisplayedColumns(Us);
        //        Unit.DataColumns()["LastIdentificationCache"].DisplayText = "Taxon";
        //        Unit.DataColumns()["LastIdentificationCache"].Width = 150;
        //        Unit.DataColumns()["LastIdentificationCache"].setReadOnly(true);
        //        Unit.DataColumns()["LastIdentificationCache"].DefaultForAdding = "?";
        //        Unit.DataColumns()["FamilyCache"].IsHidden = true;
        //        Unit.DataColumns()["OrderCache"].IsHidden = true;
        //        Unit.DataColumns()["HierarchyCache"].IsHidden = true;
        //        Unit.DataColumns()["TaxonomicGroup"].IsHidden = true;
        //        Unit.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Plant);
        //        Unit.IsRequired = true;
        //        Unit.DisplayText = "Observation";
        //        Sheet.AddDataTable(Unit);

        //        TK25AddNewIdentification(ref Sheet, Unit);
        //        TK25AddFirstIdentification(ref Sheet, Unit);
        //        TK25AddLastIdentification(ref Sheet, Unit);

        //        TK25AddAnalysis(ref Sheet, ref Unit);
        //        TK25AddGeoAnalysis(ref Sheet, ref Unit);

        //        TK25AddUnitInPart(ref Sheet, ref Unit);

        //        TK25AddUnitImage(ref Sheet, ref Unit);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        

        //    // binding for DiversityTaxonNames
        //    DiversityWorkbench.Spreadsheet.RemoteColumnBinding FamilyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
        //    FamilyCache.Column = Unit.DataColumns()["FamilyCache"];
        //    FamilyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
        //    FamilyCache.RemoteParameter = "Family";

        //    DiversityWorkbench.Spreadsheet.RemoteColumnBinding OrderCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
        //    OrderCache.Column = Unit.DataColumns()["OrderCache"];
        //    OrderCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
        //    OrderCache.RemoteParameter = "Order";

        //    DiversityWorkbench.Spreadsheet.RemoteColumnBinding HierarchyCache = new DiversityWorkbench.Spreadsheet.RemoteColumnBinding();
        //    HierarchyCache.Column = Unit.DataColumns()["HierarchyCache"];
        //    HierarchyCache.ModeOfUpdate = DiversityWorkbench.Spreadsheet.RemoteColumnBinding.UpdateMode.Allways;
        //    HierarchyCache.RemoteParameter = "Hierarchy";

        //    System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding> TaxBindList = new List<DiversityWorkbench.Spreadsheet.RemoteColumnBinding>();
        //    TaxBindList.Add(FamilyCache);
        //    TaxBindList.Add(OrderCache);
        //    TaxBindList.Add(HierarchyCache);
        //    System.Collections.Generic.List<DiversityWorkbench.Spreadsheet.RemoteLink> TaxRemLinkList = new List<DiversityWorkbench.Spreadsheet.RemoteLink>();
        //    DiversityWorkbench.Spreadsheet.RemoteLink RemoteLinkTaxon = new DiversityWorkbench.Spreadsheet.RemoteLink(
        //        DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames,
        //        null,
        //        TaxBindList);
        //    TaxRemLinkList.Add(RemoteLinkTaxon);
        //    Ident.DataColumns()["NameURI"].setRemoteLinks("TaxonomicName", TaxRemLinkList);
        //    Ident.DataColumns()["NameURI"].DisplayText = "New name";
        //    System.Collections.Generic.List<string> NameURIFixSourceSetting = new List<string>();
        //    NameURIFixSourceSetting.Add("ModuleSource");
        //    NameURIFixSourceSetting.Add("Identification");
        //    NameURIFixSourceSetting.Add("TaxonomicGroup");
        //    NameURIFixSourceSetting.Add(Unit.DataColumns()["TaxonomicGroup"].DataTable().Alias() + "." + Unit.DataColumns()["TaxonomicGroup"].Name);
        //    Ident.DataColumns()["NameURI"].FixedSourceSetSetting(NameURIFixSourceSetting, true);

        //    Ident.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);

        //    Ident.DataColumns()["ReferenceURI"].setRemoteLinks("ReferenceTitle", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityReferences);

        //    Ident.DataColumns()["TaxonomicName"].DefaultForAdding = "?";
        //    Ident.DataColumns()["IdentificationSequence"].SqlQueryForDefaultForAdding = "SELECT case when MAX(T.IdentificationSequence) is null then 1 else MAX(T.IdentificationSequence) + 1 end FROM Identification AS T WHERE T.IdentificationUnitID = #IdentificationUnitID# ";
        //    Ident.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Identification);
        //    Sheet.AddDataTable(Ident);
        //}

        //private static void TK25AddFirstIdentification(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Unit)
        //{
        //    string Table = "Identification";
        //    DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable("E1_IF", Table, "First_ident", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
        //    Ident.SetDescription("The first identification of the organism");
        //    Ident.setParentTable(Unit);
        //    Ident.setColorBack(System.Drawing.Color.Black); // TableColor(Ident.Name));
        //    Ident.DisplayText = "First ident.";
        //    //System.Collections.Generic.List<string> Is = new List<string>();
        //    //Is.Add("TaxonomicName");
        //    //Ident.setDisplayedColumns(Is);
        //    Ident.DataColumns()["TaxonomicName"].setReadOnly(true);
        //    Ident.DataColumns()["TaxonomicName"].DisplayText = "First name";
        //    Ident.DataColumns()["TaxonomicName"].Width = 150;
        //    Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
        //    Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where E1_IF.CollectionSpecimenID = L.CollectionSpecimenID " +
        //        "and E1_IF.IdentificationUnitID = L.IdentificationUnitID " +
        //        "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
        //        "having E1_IF.IdentificationSequence = min(L.IdentificationSequence)) ";

        //    Ident.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Identification);
        //    Sheet.AddDataTable(Ident);
        //}

        //private static void TK25AddLastIdentification(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, DiversityWorkbench.Spreadsheet.DataTable Unit)
        //{
        //    string Table = "Identification";
        //    DiversityWorkbench.Spreadsheet.DataTable Ident = new DiversityWorkbench.Spreadsheet.DataTable("E1_IL", Table, "Last_ident", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
        //    Ident.SetDescription("The last identification of the organism");
        //    Ident.setParentTable(Unit);
        //    Ident.setColorBack(System.Drawing.Color.Black); // TableColor(Ident.Name));
        //    Ident.DisplayText = "Last ident.";
        //    System.Collections.Generic.List<string> Is = new List<string>();
        //    //Is.Add("TaxonomicName");
        //    Is.Add("NameURI");
        //    Ident.setDisplayedColumns(Is);
        //    Ident.DataColumns()["TaxonomicName"].setReadOnly(true);
        //    Ident.DataColumns()["IdentificationDate"].setReadOnly(true);
        //    Ident.DataColumns()["NameURI"].setReadOnly(true);
        //    Ident.DataColumns()["NameURI"].DisplayText = "Filter taxa";
        //    Ident.DataColumns()["NameURI"].setRemoteLinks("TaxonomicName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames);
        //    //Ident.DataColumns()["NameURI"].LinkedModule = DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityTaxonNames;
        //    Ident.SqlRestrictionClause = " exists (select * from [dbo].[Identification] L where E1_IL.CollectionSpecimenID = L.CollectionSpecimenID " +
        //        "and E1_IL.IdentificationUnitID = L.IdentificationUnitID " +
        //        "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
        //        "having E1_IL.IdentificationSequence = max(L.IdentificationSequence)) ";

        //    Ident.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Identification);
        //    Sheet.AddDataTable(Ident);
        //}

        //private static void TK25AddAnalysis(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Unit)
        //{
        //    string Table = "IdentificationUnitAnalysis";
        //    try
        //    {
        //        //DiversityWorkbench.Spreadsheet.DataTable AnaLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
        //        DiversityWorkbench.Spreadsheet.DataTable AnaLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Parallel, ref Sheet);
        //        AnaLast.SetDescription("The last analysis of the organism");
        //        AnaLast.setColorBack(System.Drawing.Color.Black); // TableColor(AnaLast.Name));
        //        AnaLast.TemplateAlias = AnaLast.Alias();
        //        System.Collections.Generic.List<string> RestAnaLast = new List<string>();
        //        RestAnaLast.Add("AnalysisID");
        //        AnaLast.RestrictionColumns = RestAnaLast;
        //        string SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
        //            "FROM Analysis ORDER BY DisplayText";
        //        AnaLast.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
        //        AnaLast.SqlRestrictionClause = " exists (select * from [dbo].[IdentificationUnitAnalysis] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
        //            "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
        //            "and " + TableAlias(Table) + ".AnalysisID = L.AnalysisID " +
        //            "group by L.CollectionSpecimenID, L.IdentificationUnitID, L.AnalysisID " +
        //            "having " + TableAlias(Table) + ".AnalysisNumber = max(L.AnalysisNumber))  ";
        //        System.Collections.Generic.List<string> Us = new List<string>();
        //        Us.Add("Result");
        //        AnaLast.setDisplayedColumns(Us);
        //        AnaLast.DataColumns()["AnalysisNumber"].DisplayText = "LastAnalysisNumber";
        //        AnaLast.DataColumns()["AnalysisNumber"].DefaultForAdding = "1";
        //        AnaLast.DataColumns()["AnalysisNumber"].SqlQueryForDefaultForAdding = "select top 1 case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) is null and T.AnalysisNumber = '' " +
        //            "then '1' else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) < 9  " +
        //            "then cast(try_Parse(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar)  " +
        //            "else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) = 9  " +
        //            "then 'A' else char(ascii(substring(T.AnalysisNumber, 1, 1)) + 1) end end end " +
        //            "FROM IdentificationUnitAnalysis AS T " +
        //            "where t.IdentificationUnitID = #IdentificationUnitID# " +
        //            "order by char(ascii(substring(T.AnalysisNumber, 1, 1))) desc ";
        //        AnaLast.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
        //        SQL = "SELECT MIN(DisplayText) FROM Analysis";
        //        AnaLast.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //        AnaLast.setParentTable(TK25Unit);
        //        AnaLast.DisplayText = "Last analy.";
        //        AnaLast.SetDescription("Last analysis of the organism");
        //        AnaLast.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.AnalysisHierarchy);
        //        Sheet.AddDataTable(AnaLast);

        //        DiversityWorkbench.Spreadsheet.DataTable AnaNew = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table) + "_N", Table, "Last analysis", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
        //        AnaNew.SetDescription("Adding a new analysis for the organism");
        //        AnaNew.setColorBack(System.Drawing.Color.Black); // TableColor(AnaNew.Name));
        //        AnaNew.TemplateAlias = AnaNew.Alias();
        //        System.Collections.Generic.List<string> RestAnaNew = new List<string>();
        //        RestAnaNew.Add("AnalysisID");
        //        AnaNew.RestrictionColumns = RestAnaNew;
        //        SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
        //            "FROM Analysis ORDER BY DisplayText";
        //        AnaNew.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
        //        AnaNew.SqlRestrictionClause = " " + TableAlias(Table) + "_N" + ".AnalysisNumber IS NULL  ";

        //        //if (Specimen.DefaultUseAnalyisisResponsible)
        //        //{
        //        //    if (Specimen.DefaultUseCurrentUserAsDefault)
        //        //    {
        //        //        AnaNew.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
        //        //        AnaNew.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
        //        //    }
        //        //    else
        //        //    {
        //        //        AnaNew.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
        //        //        AnaNew.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
        //        //    }
        //        //}

        //        System.Collections.Generic.List<string> Un = new List<string>();
        //        Un.Add("Result");
        //        AnaNew.setDisplayedColumns(Un);
        //        AnaNew.DataColumns()["AnalysisNumber"].DisplayText = "NewAnalysisNumber";
        //        AnaNew.DataColumns()["AnalysisNumber"].SqlQueryForDefaultForAdding = "select top 1 case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) is null and T.AnalysisNumber = '' " +
        //            "then '1' else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) < 9  " +
        //            "then cast(try_Parse(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar)  " +
        //            "else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) = 9  " +
        //            "then 'A' else char(ascii(substring(T.AnalysisNumber, 1, 1)) + 1) end end end " +
        //            "FROM IdentificationUnitAnalysis AS T " +
        //            "where t.IdentificationUnitID = #IdentificationUnitID# " +
        //            "order by char(ascii(substring(T.AnalysisNumber, 1, 1))) desc ";//  select top 1 case when T.AnalysisNumber is null then '1' else cast(cast(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar) end FROM IdentificationUnitAnalysis AS T where T.IdentificationUnitID = #IdentificationUnitID# order by T.AnalysisNumber desc ";
        //        AnaNew.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
        //        SQL = "SELECT MIN(DisplayText) FROM Analysis";
        //        AnaNew.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //        AnaNew.setParentTable(TK25Unit);
        //        AnaNew.DisplayText = "New analy.";
        //        AnaNew.SetDescription("New analysis of the organism");
        //        AnaNew.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.AnalysisHierarchy);
        //        SQL = "SELECT AnalysisResult AS Value, DisplayText AS Display " +
        //            "FROM AnalysisResult WHERE AnalysisID = #AnalysisID# ORDER BY Display";
        //        AnaNew.DataColumns()["AnalysisResult"].SqlLookupSource = SQL;
        //        Sheet.AddDataTable(AnaNew);

        //        DiversityWorkbench.Spreadsheet.DataTable AnaNewStatus = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table) + "_N_Status", Table, "Neuer Status", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
        //        AnaNewStatus.SetDescription("Status Bayernflora: Setting a new status for the organism - this table is designed for the entry of new data for the project BayernFlora and is meaningless outside of this project");
        //        AnaNewStatus.setColorBack(System.Drawing.Color.Black); // TableColor(AnaNewStatus.Name));
        //        AnaNewStatus.TemplateAlias = AnaNewStatus.Alias();
        //        System.Collections.Generic.List<string> RestAnaNewStatus = new List<string>();
        //        RestAnaNewStatus.Add("AnalysisID");
        //        AnaNewStatus.RestrictionColumns = RestAnaNewStatus;
        //        SQL = "SELECT AnalysisID AS Value, DisplayText AS Display " +
        //            "FROM Analysis WHERE AnalysisID = 2 ORDER BY DisplayText";
        //        AnaNewStatus.DataColumns()["AnalysisID"].SqlLookupSource = SQL;
        //        AnaNewStatus.DataColumns()["AnalysisID"].RestrictionValue = "2";
        //        AnaNewStatus.DataColumns()["AnalysisID"].ColumnDefault = "2";
        //        AnaNewStatus.SqlRestrictionClause = " " + TableAlias(Table) + "_N_Status" + ".AnalysisNumber IS NULL  ";

        //        //if (Specimen.DefaultUseAnalyisisResponsible)
        //        //{
        //        //    if (Specimen.DefaultUseCurrentUserAsDefault)
        //        //    {
        //        //        AnaNewStatus.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserName();
        //        //        AnaNewStatus.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityWorkbench.Settings.CurrentUserUri();
        //        //    }
        //        //    else
        //        //    {
        //        //        AnaNewStatus.DataColumns()["ResponsibleName"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleName;
        //        //        AnaNewStatus.DataColumns()["ResponsibleAgentURI"].DefaultForAdding = DiversityCollection.Specimen.DefaultResponsibleURI;
        //        //    }
        //        //}

        //        System.Collections.Generic.List<string> UnStatus = new List<string>();
        //        UnStatus.Add("Result");
        //        AnaNewStatus.setDisplayedColumns(UnStatus);
        //        AnaNewStatus.DataColumns()["AnalysisNumber"].DisplayText = "Neue Nummer";
        //        AnaNewStatus.DataColumns()["AnalysisNumber"].SqlQueryForDefaultForAdding = "select top 1 case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) is null and T.AnalysisNumber = '' " +
        //            "then '1' else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) < 9  " +
        //            "then cast(try_Parse(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar)  " +
        //            "else case when try_Parse(substring(T.AnalysisNumber, 1, 1) as int) = 9  " +
        //            "then 'A' else char(ascii(substring(T.AnalysisNumber, 1, 1)) + 1) end end end " +
        //            "FROM IdentificationUnitAnalysis AS T " +
        //            "where t.IdentificationUnitID = #IdentificationUnitID# " +
        //            "order by char(ascii(substring(T.AnalysisNumber, 1, 1))) desc ";//  select top 1 case when T.AnalysisNumber is null then '1' else cast(cast(substring(T.AnalysisNumber, 1, 1) as int) + 1 as varchar) end FROM IdentificationUnitAnalysis AS T where T.IdentificationUnitID = #IdentificationUnitID# order by T.AnalysisNumber desc ";
        //        AnaNewStatus.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
        //        SQL = "SELECT MIN(DisplayText) FROM Analysis WHERE AnalysisID = 2 AND AnalysisResult IN ('-', '*', '?', '+', '99', 'E', 'I', 'K', 'U', 'Z')";
        //        AnaNewStatus.DisplayText = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //        AnaNewStatus.setParentTable(TK25Unit);
        //        AnaNewStatus.DisplayText = "N. Status";
        //        AnaNewStatus.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.AnalysisHierarchy);
        //        SQL = "SELECT AnalysisResult AS Value, DisplayText AS Display " +
        //            "FROM AnalysisResult WHERE AnalysisID = 2 AND AnalysisResult IN ('-', '*', '?', '+', '99', 'E', 'I', 'K', 'U', 'Z') ORDER BY Display";
        //        AnaNewStatus.DataColumns()["AnalysisResult"].SqlLookupSource = SQL;
        //        Sheet.AddDataTable(AnaNewStatus);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private static void TK25AddGeoAnalysis(ref DiversityWorkbench.Spreadsheet.Sheet Sheet, ref DiversityWorkbench.Spreadsheet.DataTable TK25Unit)
        //{
        //    string Table = "IdentificationUnitGeoAnalysis";
        //    try
        //    {
        //        DiversityWorkbench.Spreadsheet.DataTable AnaLast = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Last geography", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
        //        AnaLast.SetDescription("The last geographical position of the organism");
        //        AnaLast.setColorBack(System.Drawing.Color.Black); // TableColor(AnaLast.Name));
        //        AnaLast.TemplateAlias = AnaLast.Alias();
        //        AnaLast.SqlRestrictionClause = " exists (select * from [dbo].[IdentificationUnitGeoAnalysis] L where " + TableAlias(Table) + ".CollectionSpecimenID = L.CollectionSpecimenID " +
        //            "and " + TableAlias(Table) + ".IdentificationUnitID = L.IdentificationUnitID " +
        //            "group by L.CollectionSpecimenID, L.IdentificationUnitID " +
        //            "having " + TableAlias(Table) + ".AnalysisDate = max(L.AnalysisDate))  ";
        //        //System.Collections.Generic.List<string> Us = new List<string>();
        //        //Us.Add("Geography");
        //        //AnaLast.setDisplayedColumns(Us);
        //        AnaLast.DataColumns()["ResponsibleAgentURI"].setRemoteLinks("ResponsibleName", DiversityWorkbench.Spreadsheet.RemoteLink.LinkedModule.DiversityAgents);
        //        AnaLast.DataColumns()["AnalysisDate"].SqlQueryForDefaultForAdding = " select convert(nvarchar(20), getdate(), 120)";
        //        AnaLast.setParentTable(TK25Unit);
        //        AnaLast.DisplayText = "Last geo.";
        //        AnaLast.SetDescription("Last geographical position of the organism");
        //        AnaLast.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Localisation);
        //        Sheet.AddDataTable(AnaLast);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private static void TK25AddUnitImage(
        //    ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
        //    ref DiversityWorkbench.Spreadsheet.DataTable Unit)
        //{
        //    string Table = "CollectionSpecimenImage";
        //    DiversityWorkbench.Spreadsheet.DataTable Image = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Image", DiversityWorkbench.Spreadsheet.DataTable.TableType.Single, ref Sheet);
        //    Image.SetDescription("The image of the organism");
        //    Image.setParentTable(Unit);
        //    Image.setColorBack(System.Drawing.Color.Black); // TableColor(Unit.Name));
        //    System.Collections.Generic.List<string> Ca = new List<string>();
        //    Ca.Add("URI");
        //    Image.DataColumns()["URI"].IsHidden = true;
        //    Image.setDisplayedColumns(Ca);

        //    Image.SqlRestrictionClause = " exists (select * from [CollectionSpecimenImage] F where " + Unit.Alias() + ".IdentificationUnitID = F.IdentificationUnitID " +
        //        " and " + TableAlias(Table) + ".IdentificationUnitID = F.IdentificationUnitID " +
        //        "group by F.IdentificationUnitID " +
        //        "having " + TableAlias(Table) + ".URI = min(f.URI) and " + TableAlias(Table) + ".IdentificationUnitID = min(f.IdentificationUnitID))"; // and " + TableAlias(Table) + ".CollectorsName = min(F.CollectorsName)) ";
        //    Image.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Icones);
        //    Sheet.AddDataTable(Image);
        //}

        //#region Part related tables

        //private static DiversityWorkbench.Spreadsheet.DataTable TK25AddUnitInPart(
        //    ref DiversityWorkbench.Spreadsheet.Sheet Sheet,
        //    ref DiversityWorkbench.Spreadsheet.DataTable Unit)
        //{
        //    string Table = "IdentificationUnitInPart";
        //    DiversityWorkbench.Spreadsheet.DataTable UnitInPart = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
        //    UnitInPart.SetDescription("The parts of a specimen, that contain an organism of the specimen");
        //    UnitInPart.setParentTable(Unit);
        //    UnitInPart.setColorBack(System.Drawing.Color.Black); // TableColor(UnitInPart.Name));
        //    UnitInPart.AddImage("", DiversityWorkbench.Properties.Resources.Legend);//DiversityCollection.Resource.UnitInPart);
        //    Sheet.AddDataTable(UnitInPart);

        //    Table = "CollectionSpecimenPart";
        //    DiversityWorkbench.Spreadsheet.DataTable Part = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
        //    Part.SetDescription("The parts of a specimen");
        //    Part.setParentTable(UnitInPart);
        //    Part.setColorBack(System.Drawing.Color.Black); // TableColor(Part.Name));
        //    Part.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Specimen);
        //    Sheet.AddDataTable(Part);

        //    Table = "Collection";
        //    DiversityWorkbench.Spreadsheet.DataTable Collection = new DiversityWorkbench.Spreadsheet.DataTable(TableAlias(Table), Table, "Collection", DiversityWorkbench.Spreadsheet.DataTable.TableType.Lookup, ref Sheet);
        //    Collection.SetDescription("The collection where the parts are stored");
        //    Collection.setParentTable(Part);
        //    Collection.setColorBack(System.Drawing.Color.Black); // TableColor(Collection.Name));
        //    System.Collections.Generic.List<string> Cp = new List<string>();
        //    Cp.Add("CollectionName");
        //    Collection.setDisplayedColumns(Cp);
        //    Collection.AddImage("", DiversityWorkbench.Properties.Resources.Legend);// DiversityCollection.Resource.Collection);
        //    Sheet.AddDataTable(Collection);

        //    return UnitInPart;
        //}


        //#endregion

        //private static System.Collections.Generic.Dictionary<string, string> _TableAlias;

        //private static void InitTableAliasForTK25()
        //{
        //    _TableAlias = new Dictionary<string, string>();

        //    _TableAlias.Add("CollectionEvent", "A1_E");
        //    _TableAlias.Add("CollectionEventLocalisation", "A2_EL");

        //    _TableAlias.Add("CollectionSpecimen", "C0_S");
        //    _TableAlias.Add("CollectionProject", "C1_P");
        //    _TableAlias.Add("CollectionExternalDatasource", "C2_E");

        //    _TableAlias.Add("CollectionAgent", "D1_A");

        //    _TableAlias.Add("IdentificationUnit", "E0_U");
        //    _TableAlias.Add("Identification", "E1_I");
        //    _TableAlias.Add("IdentificationUnitAnalysis", "E2_UA");
        //    //_TableAlias.Add("IdentificationUnitAnalysis", "E2_UA_Status");
        //    _TableAlias.Add("IdentificationUnitGeoAnalysis", "E2_UGA");
        //    _TableAlias.Add("CollectionSpecimenImage", "E3_Im");

        //    _TableAlias.Add("IdentificationUnitInPart", "G0_UP");
        //    _TableAlias.Add("CollectionSpecimenPart", "G1_P");
        //    _TableAlias.Add("Collection", "G2_C");
        //}

        //public static string TableAlias(string TableName)
        //{
        //    string Alias = "";

        //    if (_TableAlias == null)
        //    {
        //        _TableAlias = new Dictionary<string, string>();
        //        _TableAlias.Add("CollectionEventSeries", "A0_ES");
        //        _TableAlias.Add("CollectionEventSeriesImage", "A1_ESI");

        //        _TableAlias.Add("CollectionEvent", "B0_E");
        //        _TableAlias.Add("CollectionEventLocalisation", "B1_EL");
        //        _TableAlias.Add("CollectionEventProperty", "B2_EP");
        //        _TableAlias.Add("CollectionEventImage", "B3_EI");
        //        _TableAlias.Add("CollectionEventMethod", "B4_EM");
        //        _TableAlias.Add("CollectionEventParameterValue", "B5_EV");

        //        _TableAlias.Add("CollectionSpecimen", "C0_S");
        //        _TableAlias.Add("CollectionProject", "C1_P");
        //        //_TableAlias.Add("CollectionSpecimenImage", "C2_I");
        //        _TableAlias.Add("CollectionSpecimenReference", "C3_Ref");
        //        _TableAlias.Add("CollectionSpecimenRelation", "C4_R");
        //        _TableAlias.Add("CollectionExternalDatasource", "C5_ED");

        //        _TableAlias.Add("CollectionAgent", "D1_A");

        //        _TableAlias.Add("IdentificationUnit", "E0_U");
        //        _TableAlias.Add("Identification", "E1_I");
        //        _TableAlias.Add("IdentificationUnitAnalysis", "E2_UA");
        //        _TableAlias.Add("IdentificationUnitAnalysisMethod", "E3_UAM");
        //        _TableAlias.Add("IdentificationUnitAnalysisMethodParameter", "E4_UAMP");
        //        _TableAlias.Add("IdentificationUnitGeoAnalysis", "E5_UG");


        //        _TableAlias.Add("CollectionSpecimenPart", "F0_P");
        //        _TableAlias.Add("CollectionSpecimenPartDescription", "F1_PD");
        //        _TableAlias.Add("CollectionSpecimenProcessing", "F2_P");
        //        _TableAlias.Add("CollectionSpecimenProcessingMethod", "F3_PM");
        //        _TableAlias.Add("CollectionSpecimenProcessingMethodParameter", "F4_PMP");
        //        _TableAlias.Add("Collection", "F5_C");
        //        _TableAlias.Add("CollectionSpecimenTransaction", "F6_T");
        //        _TableAlias.Add("Transaction", "F7_T");

        //        /// allways after part and unit
        //        _TableAlias.Add("IdentificationUnitInPart", "H1_UIP");

        //        /// at the very end of the table of the corresponding domain
        //                _TableAlias.Add("CollectionSpecimenImage", "E6_Im");
        //        //if (_SheetTarget != SheetTarget.Image)

        //        _TableAlias.Add("ExternalIdentifier", "K0_EI");

        //        _TableAlias.Add("Annotation", "L0_A");
        //    }
        //    if (_TableAlias.ContainsKey(TableName))
        //        Alias = _TableAlias[TableName];
        //    return Alias;
        //}



        //#endregion

    }
}
