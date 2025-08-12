using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DiversityWorkbench
{
    public class SamplingPlot : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Parameter

        private string _LatForMap;
        private string _LonForMap;
        private System.Collections.Generic.Dictionary<string, string> _ServiceList;

        #endregion

        #region Construction

        public SamplingPlot(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        public SamplingPlot(DiversityWorkbench.ServerConnection ServerConnection, System.Uri URL)
            : base(ServerConnection, URL)
        {
            //string ID = URL.Substring(this.ServerConnection.BaseURL.Length);
            //string x = URL.Host;
            //int i;
            //if (!int.TryParse(ID, out i))
            //{
            //    //this.ServerConnection.DatabaseServer;
            //}
        }

        #endregion

        #region Interface

        public override string ServiceName() { return "DiversitySamplingPlots"; }

        /// <summary>
        /// Additional services provided by webservices by external providers
        /// </summary>
        /// <returns></returns>
        public override System.Collections.Generic.Dictionary<string, string> AdditionalServicesOfModule()
        {
            if (this._ServiceList == null)
            {
                this._ServiceList = new Dictionary<string, string>();
                _ServiceList.Add("CacheDB", "Cache database");
            }
            return _ServiceList;
        }


        public override System.Collections.Generic.List<DiversityWorkbench.DatabaseService> DatabaseServices()
        {
            System.Collections.Generic.List<DiversityWorkbench.DatabaseService> ds = new List<DatabaseService>();

#if DEBUG
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
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
#else

            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
            {
                if (KV.Value.ConnectionIsValid)
                {
                    string SQLDS = "SELECT Project AS DisplayText, ProjectID FROM ProjectList ORDER BY Project";
                    Microsoft.Data.SqlClient.SqlDataAdapter adDS = new Microsoft.Data.SqlClient.SqlDataAdapter(SQLDS, KV.Value.ConnectionString);
                    System.Data.DataTable dtDS = new System.Data.DataTable();
                    adDS.Fill(dtDS);
                    foreach (System.Data.DataRow R in dtDS.Rows)
                    {
                        DiversityWorkbench.DatabaseService DS = new DatabaseService(KV.Value.DatabaseName);
                        DS.IsWebservice = false;
                        DS.IsListInDatabase = true;
                        DS.ListName = R["DisplayText"].ToString();
                        DS.RestrictionForListInDatabase = "ProjectID = " + R["ProjectID"].ToString();
                        if (KV.Value.DatabaseServer != DiversityWorkbench.Settings.DatabaseServer)
                            DS.Server = KV.Value.DatabaseServer;
                        ds.Add(DS);
                    }
                }
            }
#endif

            return ds;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            try
            {
                char[] removeSequencer = { '|' };

                string Prefix = "";
                if (this._ServerConnection.LinkedServer.Length > 0)
                    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
                else Prefix = "dbo.";

                if (this._ServerConnection.ConnectionString.Length > 0)
                {
                    string SQL = "";
                    if (this._ServerConnection.LinkedServer.Length == 0)
                    {
                        SQL = "SELECT P.PlotID AS _URI, U.BaseURL + cast(P.PlotID as varchar) AS Link, S.PlotHierarchyCache AS _DisplayText, P.PlotDescription AS Description, P.PlotType AS Type, P.PlotIdentifier AS [Plot identifier]  " +
                            "FROM " + Prefix + "SamplingPlotSuperiorNodes(" + ID.ToString() + ") S, SamplingPlot P, " + Prefix + "ViewBaseURL AS U " +
                            "WHERE P.PlotID = " + ID.ToString() + " AND S.plotID = P.PlotID ";
                    }
                    else
                    {
                        SQL = "SELECT P.PlotID AS _URI, U.BaseURL + cast(P.PlotID as varchar) AS Link, H.DisplayText AS _DisplayText, P.PlotDescription AS Description, P.PlotType AS Type, P.PlotIdentifier AS [Plot identifier]  " +
                            "FROM " + Prefix + "ViewSamplingPlot P, " + Prefix + "ViewSamplingPlotHierarchy H, " + Prefix + "ViewBaseURL AS U " +
                            "WHERE P.PlotID = " + ID.ToString() + " AND H.plotID = P.PlotID ";
                    }
                    this.getDataFromTable(SQL, ref Values);

                    SQL = "SELECT Latitude, Longitude, [Accuracy angle], AccuracyAngle, AreaM2, Area, Geography, CountryCache " +
                        "FROM " + Prefix + "SamplingPlotGeoInfo I " +
                        "WHERE I.PlotID = " + ID.ToString();
                    this.getDataFromTable(SQL, ref Values);

                    if (this._ServerConnection.LinkedServer.Length == 0)
                    {
                        SQL = "DECLARE @TA Table (Altitude float, PlotID int); " +
                            "INSERT INTO @TA (Altitude, PlotID)" +
                            "SELECT " +
                            "CASE WHEN SL.LocalisationSystemID = 4 AND NOT SL.AverageAltitudeCache IS NULL " +
                            "THEN SL.AverageAltitudeCache " +
                            "ELSE " +
                            "CASE WHEN PL.LocalisationSystemID = 4 AND NOT PL.AverageAltitudeCache IS NULL " +
                            "THEN PL.AverageAltitudeCache " +
                            "ELSE " +
                            "CASE WHEN NOT S.PlotGeography_Cache.EnvelopeCenter().Z IS NULL " +
                            "THEN S.PlotGeography_Cache.EnvelopeCenter().Z " +
                            "ELSE P.PlotGeography_Cache.EnvelopeCenter().Z END " +
                            "END " +
                            "END AS Altitude, " +
                            "CASE WHEN SL.LocalisationSystemID = 4 AND NOT SL.AverageAltitudeCache IS NULL " +
                            "THEN SL.PlotID " +
                            "ELSE " +
                            "CASE WHEN PL.LocalisationSystemID = 4 AND NOT PL.AverageAltitudeCache IS NULL " +
                            "THEN PL.PlotID " +
                            "ELSE " +
                            "CASE WHEN NOT S.PlotGeography_Cache.EnvelopeCenter().Z IS NULL " +
                            "THEN S.PlotID " +
                            "ELSE P.PlotID END " +
                            "END " +
                            "END AS PlotID " +
                            "FROM  " + Prefix + "SamplingPlot AS P " +
                            "RIGHT OUTER JOIN " + Prefix + "SamplingPlot AS S ON P.PlotID = S.PartOfPlotID " +
                            "LEFT OUTER JOIN " + Prefix + "SamplingPlotLocalisation AS SL ON S.PlotID = SL.PlotID " +
                            "LEFT OUTER JOIN " + Prefix + "SamplingPlotLocalisation AS PL ON P.PlotID = PL.PlotID WHERE S.PlotID = " + ID.ToString() + " " +
                            "ORDER BY CASE WHEN SL.LocalisationSystemID = 4 AND NOT SL.AverageAltitudeCache IS NULL " +
                            "THEN SL.AverageAltitudeCache ELSE CASE WHEN PL.LocalisationSystemID = 4 AND NOT PL.AverageAltitudeCache IS NULL " +
                            "THEN PL.AverageAltitudeCache ELSE CASE WHEN NOT S.PlotGeography_Cache.EnvelopeCenter().Z IS NULL " +
                            "THEN S.PlotGeography_Cache.EnvelopeCenter().Z ELSE P.PlotGeography_Cache.EnvelopeCenter().Z END END END " +
                            "DESC; " +
                            "DECLARE @Alt float " +
                            "SET @Alt = (SELECT MIN(Altitude) FROM @TA T WHERE T.PlotID = " + ID.ToString() + " AND NOT T.Altitude IS NULL) " +
                            "IF (@Alt IS NULL) " +
                            "BEGIN " +
                            "SET @Alt = (SELECT MIN(Altitude) FROM @TA T WHERE NOT T.Altitude IS NULL) " +
                            "END " +
                            "SELECT @Alt AS Altitude ";
                        this.getDataFromTable(SQL, ref Values);
                    }

                    if (this._ServerConnection.LinkedServer.Length == 0)
                    {
                        SQL = "SELECT cast(SP.PlotGeography_Cache.EnvelopeCenter().STDistance(geography::Point(SP.PlotGeography_Cache.EnvelopeCenter().Lat " +
                            " + SP.PlotGeography_Cache.EnvelopeAngle(), SP.PlotGeography_Cache.EnvelopeCenter().Long " +
                            " + SP.PlotGeography_Cache.EnvelopeAngle(), 4326)) AS VARCHAR) + ' m' AS Accuracy " +
                        "FROM " + Prefix + "SamplingPlot SP " +
                        "WHERE NOT PlotGeography_Cache IS NULL AND SP.PlotID = " + ID.ToString();
                        this.getDataFromTable(SQL, ref Values);
                    }
                    else
                    {
                        //SQL = "SELECT cast(SP.PlotGeography_Cache.EnvelopeCenter().STDistance(geography::Point(SP.PlotGeography_Cache.EnvelopeCenter().Lat " +
                        //    " + SP.PlotGeography_Cache.EnvelopeAngle(), SP.PlotGeography_Cache.EnvelopeCenter().Long " +
                        //    " + SP.PlotGeography_Cache.EnvelopeAngle(), 4326)) AS VARCHAR) + ' m' AS Accuracy " +
                        //"FROM " + Prefix + "ViewSamplingPlot SP " +
                        //"WHERE NOT PlotGeography_Cache IS NULL AND SP.PlotID = " + ID.ToString();
                        //this.getDataFromTable(SQL, ref Values);
                    }

                    if (this._ServerConnection.LinkedServer.Length == 0)
                    {
                        // Toni 20230629: following initialization was commented out below. Is neccessary for subsequent calls of ModuleConnections/UnitValues
                        _LatForMap = "";
                        _LonForMap = "";

                        //Markus 19.02.2018 Table SamplingPlotPoint does not exist

                        //System.Data.DataTable dtPoints = new System.Data.DataTable();
                        //System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
                        //SQL = "SELECT PointSequence AS PointID, PointGeography.Lat AS Latitude, PointGeography.Long AS Longitude " +
                        //    "FROM " + Prefix + "SamplingPlotPoint " +
                        //    "WHERE PlotID = " + ID.ToString() +
                        //    " ORDER BY PointSequence";
                        //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                        //try
                        //{
                        //    ad.Fill(dtPoints);
                        //}
                        //catch (System.Exception ex)
                        //{
                        //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        //}
                        //int MaxPoints = 130;
                        //if (dtPoints.Rows.Count > MaxPoints)
                        //    dtPoints = DiversityWorkbench.SamplingPlot.compressGeoPoints(dtPoints, MaxPoints, "PointID", "Latitude", "Longitude");
                        //_LatForMap = "";
                        //_LonForMap = "";
                        //if (dtPoints.Rows.Count > 0)
                        //{
                        //    foreach (System.Data.DataRow R in dtPoints.Rows)
                        //    {
                        //        _LatForMap += R["Latitude"].ToString().ToString(InvC).Replace(",", ".") + "|";
                        //        _LonForMap += R["Longitude"].ToString().ToString(InvC).Replace(",", ".") + "|";
                        //    }
                        //}
                        //else
                        {
                            SQL = "SELECT PlotGeography_Cache.STAsText() AS Geo, PlotGeography_Cache.STGeometryType() AS GeoType " +
                               "FROM " + Prefix + "SamplingPlot WHERE PlotID = " + ID.ToString();
                            System.Data.DataTable dtGeo = new System.Data.DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                            ad.SelectCommand.CommandText = SQL;
                            ad.Fill(dtGeo);
                            System.Data.DataTable dtGeoPoints = new System.Data.DataTable();
                            if (dtGeo.Rows.Count > 0)
                            {
                                System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
                                string Type = dtGeo.Rows[0]["GeoType"].ToString().ToUpper();
                                string Geometry = dtGeo.Rows[0]["Geo"].ToString();
                                System.Data.DataTable dtGeoPointsFromCache = DiversityWorkbench.SamplingPlot.dtGeoPointsFromSTAsText(Geometry, Type);
                                if (dtGeoPointsFromCache.Rows.Count < 130)
                                {
                                    foreach (System.Data.DataRow R in dtGeoPointsFromCache.Rows)
                                    {
                                        _LatForMap += R["Latitude"].ToString().ToString(InvC).Replace(",", ".") + "|";
                                        _LonForMap += R["Longitude"].ToString().ToString(InvC).Replace(",", ".") + "|";
                                    }
                                }
                                else
                                {
                                    dtGeoPointsFromCache = DiversityWorkbench.SamplingPlot.compressGeoPoints(dtGeoPointsFromCache, 130, "ID", "Latitude", "Longitude");
                                    if (dtGeoPointsFromCache.Rows.Count > 0)
                                    {
                                        foreach (System.Data.DataRow R in dtGeoPointsFromCache.Rows)
                                        {
                                            _LatForMap += R["Latitude"].ToString().ToString(InvC).Replace(",", ".") + "|";
                                            _LonForMap += R["Longitude"].ToString().ToString(InvC).Replace(",", ".") + "|";
                                        }
                                    }
                                }
                            }
                        }
                        if (_LatForMap != null)
                            _LatForMap = _LatForMap.TrimEnd(removeSequencer);
                        if (_LonForMap != null)
                            _LonForMap = _LonForMap.TrimEnd(removeSequencer);

                        //Markus 19.02.2018 auf Wunsch von Florian Raub um den Typ ergänzt
                        if (Prefix == "dbo." && 1 == 0) // Wieder ausgebaut weil zu langsam fuer Export
                        {
                            SQL = "SELECT S.DisplayText AS Property, P.PropertyName AS PropertyType, S.PropertyURI AS [Link to DiversityScientificTerms] " +
                            "FROM dbo.SamplingPlotAllProperties() S, dbo.Property P " +
                            "WHERE PlotID = " + ID.ToString() +
                            " AND S.PropertyID = P.PropertyID " +
                            " ORDER BY S.DisplayText ";
                            this.getDataFromTable(SQL, ref Values);
                        }
                        else
                        {
                            SQL = "SELECT S.DisplayText AS Property, P.PropertyName AS PropertyType, S.PropertyURI AS [Link to DiversityScientificTerms] " +
                            "FROM " + Prefix + "SamplingPlotProperty S, " + Prefix + "Property P " +
                            "WHERE PlotID = " + ID.ToString() +
                            " AND S.PropertyID = P.PropertyID " +
                            " ORDER BY S.DisplayText ";
                            this.getDataFromTable(SQL, ref Values);
                        }

                        // Markus 17.12.2018: auf Wunsch von Alexander Bach eingebaut
                        SQL = "SELECT [Location2] AS [Link to DiversityGazetteer] " +
                            "FROM " + Prefix + "[SamplingPlotLocalisation] P inner join " + Prefix + "[LocalisationSystem] L ON P.LocalisationSystemID = L.LocalisationSystemID " +
                            "WHERE L.ParsingMethodName = 'Gazetteer' " +
                            "AND  P.PlotID =  " + ID.ToString() +
                            "AND P.Location2 LIKE 'http%'";
                        this.getDataFromTable(SQL, ref Values);

                        //// Markus 29.4.2019: auf Wunsch von Florian Raub eingebaut
                        //SQL = "SELECT P.DisplayText + ':' " +
                        //    " + case when P.Location1 is null then '' else case when L.DisplayTextLocation1 is null then '' else ' ' + L.DisplayTextLocation1 + ' ' end " +
                        //    " + P.Location1  + case when L.DefaultMeasurementUnit is null then '' else ' ' + L.DefaultMeasurementUnit end end " +
                        //    " + case when P.Location2 is null then '' else case when L.DisplayTextLocation2 is null then '' else ' ' + L.DisplayTextLocation2 + ' ' end " + 
                        //    " + P.Location2  + case when L.DefaultMeasurementUnit is null then '' else ' ' + L.DefaultMeasurementUnit end end " +
                        //    "AS Localisation " +
                        //    "FROM " + Prefix + "ViewSamplingPlotAllLocalisations P, LocalisationSystem L  " +
                        //    "WHERE P.LocalisationSystemID = L.LocalisationSystemID and P.PlotID =  " + ID.ToString();
                        //this.getDataFromTable(SQL, ref Values);

                        // Markus 13.08.2019: Rückbau wegen Performance problemen
                        SQL = "SELECT L.DisplayText + ':' " +
                            " + case when P.Location1 is null then '' else case when L.DisplayTextLocation1 is null then '' else ' ' + L.DisplayTextLocation1 + ' ' end " +
                            " + P.Location1  + case when L.DefaultMeasurementUnit is null then '' else ' ' + L.DefaultMeasurementUnit end end " +
                            " + case when P.Location2 is null then '' else case when L.DisplayTextLocation2 is null then '' else ' ' + L.DisplayTextLocation2 + ' ' end " +
                            " + P.Location2  + case when L.DefaultMeasurementUnit is null then '' else ' ' + L.DefaultMeasurementUnit end end " +
                            "AS Localisation " +
                            "FROM " + Prefix + "SamplingPlotLocalisation P, LocalisationSystem L  " +
                            "WHERE P.LocalisationSystemID = L.LocalisationSystemID and P.PlotID =  " + ID.ToString();
                        this.getDataFromTable(SQL, ref Values);
                    }
                    else
                    {
                        _LatForMap = "";
                        _LonForMap = "";
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this._UnitValues = Values;
            //if (this._UnitValues == null)
            //if (this._UnitValues.Count < Values.Count)
            //    this._UnitValues = Values;
            return Values;
        }

        public System.Data.DataTable SamplingPlotHierarchy(int ID)
        {
            string SQL = "SELECT dbo.BaseURL() + CAST(PlotID AS varchar) AS PlotID, dbo.BaseURL() + CAST(PartOfPlotID AS varchar) AS PartOfPlotID, PlotIdentifier AS DisplayText " +
                "FROM dbo.SamplingPlotHierarchy(" + ID.ToString() + ")";
            System.Data.DataTable dtData = new System.Data.DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                ad.Fill(dtData);
            }
            catch
            {
            }
            return dtData;
        }

        public System.Data.DataTable SamplingPlotChildHierarchy(int ID)
        {
            string SQL = "SELECT dbo.BaseURL() + CAST(PlotID AS varchar) AS PlotID, dbo.BaseURL() + CAST(PartOfPlotID AS varchar) AS PartOfPlotID, PlotIdentifier AS DisplayText " +
                " FROM dbo.SamplingPlot P WHERE P.PlotID = " + ID.ToString() +
                " UNION " +
                " SELECT dbo.BaseURL() + CAST(PlotID AS varchar) AS PlotID, dbo.BaseURL() + CAST(PartOfPlotID AS varchar) AS PartOfPlotID, PlotIdentifier AS DisplayText " +
                " FROM dbo.SamplingPlotChildNodes(" + ID.ToString() + ")";
            System.Data.DataTable dtData = new System.Data.DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                ad.Fill(dtData);
            }
            catch
            {
            }
            return dtData;
        }

        public string MainTable() { return "SamplingPlotList"; }

        public override string SourceURI(DiversityWorkbench.UserControls.UserControlWebView WebBrowser)
        {
            if (this._LatForMap == null || this._LatForMap.Length == 0) return "";
            string URL = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
            if (string.IsNullOrEmpty(URL))
                return "";
            if (this._LatForMap.Length > 0)
                URL += "?LatPoint=" + this._LatForMap.ToString().Replace(",", ".") + "&LngPoint=" + this._LonForMap.ToString().Replace(",", ".");
            else
                URL += "";
            if (!_LatForMap.Contains("|"))
            {
                URL += "&zoom=15";
            }
            return URL;
        }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[7];
            QueryDisplayColumns[0].DisplayText = "Identifier";
            QueryDisplayColumns[0].DisplayColumn = "PlotIdentifier";

            QueryDisplayColumns[1].DisplayText = "Description";
            QueryDisplayColumns[1].DisplayColumn = "PlotDescription";

            QueryDisplayColumns[2].DisplayText = "Hierarchy";
            QueryDisplayColumns[2].DisplayColumn = "DisplayText";

            QueryDisplayColumns[3].DisplayText = "Type";
            QueryDisplayColumns[3].DisplayColumn = "PlotType";

            QueryDisplayColumns[4].DisplayText = "Country";
            QueryDisplayColumns[4].DisplayColumn = "CountryCache";

            QueryDisplayColumns[5].DisplayText = "Locality";
            QueryDisplayColumns[5].DisplayColumn = "Locality";
            QueryDisplayColumns[5].TableName = "ViewLocality";

            QueryDisplayColumns[6].DisplayText = "Property";
            QueryDisplayColumns[6].DisplayColumn = "DisplayText";
            QueryDisplayColumns[6].TableName = "SamplingPlotProperty";

            //QueryDisplayColumns[7].DisplayText = "Plot";
            //QueryDisplayColumns[7].DisplayColumn = "PlotIdentifier";
            //QueryDisplayColumns[7].TableName = "SamplingPlot";

            for (int i = 0; i < QueryDisplayColumns.Length; i++)
            {
                if (QueryDisplayColumns[i].DisplayText == null)
                    QueryDisplayColumns[i].DisplayText = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].OrderColumn == null)
                    QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].IdentityColumn == null)
                    QueryDisplayColumns[i].IdentityColumn = "PlotID";
                if (QueryDisplayColumns[i].TableName == null)
                    QueryDisplayColumns[i].TableName = "ViewSamplingPlotListForCurrentProject";
                if (QueryDisplayColumns[i].TipText == null)
                    QueryDisplayColumns[i].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(QueryDisplayColumns[i].TableName, QueryDisplayColumns[i].DisplayColumn);
                QueryDisplayColumns[i].Module = "DiversitySamplingPlots";
            }

            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            string Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversitySamplingPlots"].ServerConnection.DatabaseName;
            string SQL = "";
            string Description = "";
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";


            #region Project

            System.Data.DataTable dtProject = new System.Data.DataTable();
            SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                "FROM " + Prefix + "ProjectList " +
                "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtProject); }
                catch { }
            }
            if (dtProject.Rows.Count > 1)
            {
                dtProject.Clear();
                //SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS [Value], Project AS Display " +
                //    "FROM ProjectList " +
                //    "ORDER BY Display";
                SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                    "FROM " + Prefix + "ProjectList " +
                    "ORDER BY Display";
                a.SelectCommand.CommandText = SQL;
                try { a.Fill(dtProject); }
                catch { }
            }
            if (dtProject.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtProject.Columns.Add(Value);
                dtProject.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
            DiversityWorkbench.QueryCondition qProject = new DiversityWorkbench.QueryCondition(true, "SamplingProject", "PlotID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
            qProject.ServerConnection = this.getServerConnection();
            qProject.SourceIsFunction = true;
            QueryConditions.Add(qProject);

            #endregion

            #region SamplingPlot

            string SourceTable = "SamplingPlot";
            if (this._ServerConnection.LinkedServer.Length > 0)
                SourceTable = "ViewSamplingPlot";

            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "PlotIdentifier");
            DiversityWorkbench.QueryCondition qPlotIdentifier = new DiversityWorkbench.QueryCondition(true, SourceTable, "PlotID", "PlotIdentifier", "Plot", "Plot", "Sampling plot", Description);
            QueryConditions.Add(qPlotIdentifier);

            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "PlotID");
            DiversityWorkbench.QueryCondition qPlotID = new DiversityWorkbench.QueryCondition(false, SourceTable, "PlotID", "PlotID", "Plot", "ID", "PlotID", Description, false, false, true, false);
            QueryConditions.Add(qPlotID);

            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "PlotType");
            DiversityWorkbench.QueryCondition qPlotType = new DiversityWorkbench.QueryCondition(true, SourceTable, "PlotID", "PlotType", "Plot", "Type", "Plot type", Description);
            QueryConditions.Add(qPlotType);

            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "PlotDescription");
            DiversityWorkbench.QueryCondition qPlotDescription = new DiversityWorkbench.QueryCondition(true, SourceTable, "PlotID", "PlotDescription", "Plot", "Description", "Plot description", Description);
            QueryConditions.Add(qPlotDescription);

            try
            {
                if (this._ServerConnection.LinkedServer.Length == 0)
                {
                    Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "PartOfPlotID");
                    DiversityWorkbench.QueryCondition qHierarchy = new QueryCondition(true, "SamplingPlot", "ViewSamplingPlotListForCurrentProject", "PlotID", "Plot", "Hierarchy", "Hierarchy of the plot", Description, true, "PlotID", "PartOfPlotID", "PlotIdentifier", "DisplayText");
                    qHierarchy.ServerConnection = this.getServerConnection();
                    qHierarchy.QueryType = QueryCondition.QueryTypes.Hierarchy;
                    qHierarchy.DependsOnCurrentProjectID = true;
                    QueryConditions.Add(qHierarchy);
                }
                //qHierarchy.RequeryHierarchySource();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            //try
            //{
            //    // Hierarchy search for SamplingPlot
            //    System.Data.DataTable dtPlotHierarchy = new System.Data.DataTable();
            //    SQL = "SELECT NULL AS [PlotID], NULL AS [PartOfPlotID], NULL AS DisplayText " +
            //        "UNION " +
            //        "SELECT PlotID AS [PlotID], PartOfPlotID AS [PartOfPlotID], DisplayText AS DisplayText " +
            //        "FROM [dbo].[SamplingPlotListForCurrentProject] () " +
            //        "ORDER BY DisplayText ";
            //    string ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            //    if (this.ServerConnection != null && this.ServerConnection.ConnectionString != null && this.ServerConnection.ConnectionString.Length > 0)
            //        ConnectionString = this.ServerConnection.ConnectionString;
            //    Microsoft.Data.SqlClient.SqlDataAdapter aColl = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
            //    if (ConnectionString.Length > 0)
            //    {
            //        try { aColl.Fill(dtPlotHierarchy); }
            //        catch (System.Exception ex) 
            //        {
            //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //        }
            //    }
            //    if (dtPlotHierarchy.Columns.Count == 0)
            //    {
            //        System.Data.DataColumn Value = new System.Data.DataColumn("PlotID");
            //        System.Data.DataColumn ParentValue = new System.Data.DataColumn("PartOfPlotID");
            //        System.Data.DataColumn Display = new System.Data.DataColumn("DisplayText");
            //        //System.Data.DataColumn DisplayOrder = new System.Data.DataColumn("DisplayOrder");
            //        dtPlotHierarchy.Columns.Add(Value);
            //        dtPlotHierarchy.Columns.Add(ParentValue);
            //        dtPlotHierarchy.Columns.Add(Display);
            //        //dtPlotHierarchy.Columns.Add(DisplayOrder);
            //    }
            //    System.Collections.Generic.List<DiversityWorkbench.QueryField> FF = new List<QueryField>();
            //    DiversityWorkbench.QueryField CS_C = new QueryField("SamplingPlot", "PlotID", "PlotID");
            //    FF.Add(CS_C);
            //    Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "PartOfPlotID");
            //    DiversityWorkbench.QueryCondition qPlotHierarchy = new DiversityWorkbench.QueryCondition(true, FF, "Plot", "Hierarchy", "Hierarchy of the sampling plot", Description, dtPlotHierarchy, false, "DisplayText", "PartOfPlotID", "DisplayText", "PlotID");
            //    qPlotHierarchy.DependsOnCurrentProjectID = true;
            //    qPlotHierarchy.SourceIsFunction = true;
            //    qPlotHierarchy.QueryType = QueryCondition.QueryTypes.Hierarchy;
            //    QueryConditions.Add(qPlotHierarchy);
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}


            #region Geography

            try
            {
                Description = DiversityWorkbench.Functions.ColumnDescription(SourceTable, "PlotGeography_Cache");
                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation ON CollectionSpecimen.CollectionEventID = CollectionEventLocalisation.CollectionEventID " +
                //    " WHERE (NOT CollectionEventLocalisation.Geography IS NULL)";
                DiversityWorkbench.QueryCondition _qGeography = new QueryCondition();
                //_qGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qGeography.Entity = "CollectionEventLocalisation.Geography";
                //_qGeography.SqlFromClause = SQL;
                //_qGeography.DisplayText = DiversityWorkbench.CollectionSpecimenText.WGS84_present;// "WGS84 present";
                //_qGeography.Description = DiversityWorkbench.CollectionSpecimenText.If_any_WGS84_coordinates_are_present;// "If any WGS84 coordinates are present";
                _qGeography.TextFixed = true;
                _qGeography.Description = Description;
                _qGeography.DisplayText = "Geography";
                _qGeography.Table = "SamplingPlot";
                _qGeography.IdentityColumn = "PlotID";
                //_qGeography.ForeignKey = "CollectionEventID";
                _qGeography.Column = "PlotGeography_Cache";
                //_qGeography.ForeignKeyTable = "CollectionSpecimen";
                //_qGeography.Restriction = "LocalisationSystemID = 8";
                _qGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qGeography.QueryGroup = "Plot";
                //_qGeography.IsGeography = true;
                DiversityWorkbench.QueryCondition qGeography = new QueryCondition(_qGeography);
                QueryConditions.Add(qGeography);
            }
            catch (System.Exception ex) { }
            //DiversityWorkbench.QueryCondition qGeography = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "Geography", "Event localisation", "Geography.", "Geography", "Geography", false, false, true, false, "CollectionEventID");
            //QueryConditions.Add(qGeography);

            #endregion

            System.Data.DataTable dtUser = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT LoginName, CombinedNameCache " +
                "FROM " + Prefix + "UserProxy " +
                "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter aUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aUser.Fill(dtUser); }
                catch { }
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "LogCreatedBy");
            DiversityWorkbench.QueryCondition qLogCreatedBy = new DiversityWorkbench.QueryCondition(false, SourceTable, "PlotID", "LogCreatedBy", "Plot", "Creat. by", "The user that created the dataset", Description, dtUser, false);
            QueryConditions.Add(qLogCreatedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "LogCreatedWhen");
            DiversityWorkbench.QueryCondition qLogCreatedWhen = new DiversityWorkbench.QueryCondition(false, SourceTable, "PlotID", "LogCreatedWhen", "Plot", "Creat. date", "The date when the dataset was created", Description, true);
            QueryConditions.Add(qLogCreatedWhen);

            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "LogUpdatedBy");
            DiversityWorkbench.QueryCondition qLogUpdatedBy = new DiversityWorkbench.QueryCondition(false, SourceTable, "PlotID", "LogUpdatedBy", "Plot", "Changed by", "The last user that changed the dataset", Description, dtUser, false);
            QueryConditions.Add(qLogUpdatedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlot", "LogUpdatedWhen");
            DiversityWorkbench.QueryCondition qLogUpdatedWhen = new DiversityWorkbench.QueryCondition(false, SourceTable, "PlotID", "LogUpdatedWhen", "Plot", "Changed at", "The last date when the dataset was changed", Description, true);
            QueryConditions.Add(qLogUpdatedWhen);

            /*
            Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlotProperty", "PropertyHierarchyCache");
            DiversityWorkbench.QueryCondition qPropertyHierarchyCache = new DiversityWorkbench.QueryCondition(true, SourceTable, "PlotID", "PropertyHierarchyCache", "Plot", "Hierarchy Cache", "Property Hierarchy Cache", Description);
            QueryConditions.Add(qPropertyHierarchyCache);

            SQL = " FROM " + Prefix + "SamplingPlotProperty " +
                " WHERE (SamplingPlotProperty.PropertyID IN (41))";
            DiversityWorkbench.QueryCondition qBiotopTyp = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "BiotopTyp.", "Biotop type", "Biotop type",
                false, false, false, false, "PlotID", "Property.PropertyID.41");
            qBiotopTyp.Restriction = "PropertyID IN (41)";
            QueryConditions.Add(qBiotopTyp);

            */

            #endregion

            //#region Geography

            //Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlotPoint", "PointGeography");
            ////SQL = " FROM SamplingPlot INNER JOIN " +
            ////    " SamplingPlotPoint ON SamplingPlot.PlotID = SamplingPlotPoint.PlotID " +
            ////    " WHERE (NOT SamplingPlotPoint.PointGeography IS NULL)";
            //DiversityWorkbench.QueryCondition _qGeography = new QueryCondition();
            //_qGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            //_qGeography.Entity = "SamplingPlotPoint.PointGeography";
            //_qGeography.SqlFromClause = SQL;
            ////_qGeography.DisplayText = DiversityWorkbench.CollectionSpecimenText.WGS84_present;// "WGS84 present";
            ////_qGeography.Description = DiversityWorkbench.CollectionSpecimenText.If_any_WGS84_coordinates_are_present;// "If any WGS84 coordinates are present";
            //_qGeography.TextFixed = false;
            //_qGeography.Description = Description;
            //_qGeography.Table = "SamplingPlotPoint";
            //_qGeography.IdentityColumn = "PlotID";
            ////_qGeography.ForeignKey = "PlotID";
            //_qGeography.Column = "PointGeography";
            //_qGeography.ForeignKeyTable = "SamplingPlot";
            ////_qGeography.Restriction = "LocalisationSystemID = 8";
            //_qGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            //_qGeography.QueryGroup = "Geography";
            //_qGeography.IsGeography = true;
            //DiversityWorkbench.QueryCondition qGeography = new QueryCondition(_qGeography);
            //QueryConditions.Add(qGeography);
            ////DiversityWorkbench.QueryCondition qGeography = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "Geography", "Event localisation", "Geography.", "Geography", "Geography", false, false, true, false, "CollectionEventID");
            ////QueryConditions.Add(qGeography);

            //#endregion

            //-----WR-----
            #region Property

            //SQL = " FROM " + Prefix + "SamplingPlotAllProperties() ";
            //DiversityWorkbench.QueryCondition qProperty = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
            //    "PlotID", true, SQL, "DisplayText", "Site property", "Property", "Property", "Property",
            //    false, false, false, false, "PlotID", "Property");
            //QueryConditions.Add(qProperty);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (41))";
            DiversityWorkbench.QueryCondition qBiotopTyp = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "BiotopTyp.", "Biotop type", "Biotop type",
                false, false, false, false, "PlotID", "Property.PropertyID.41");
            qBiotopTyp.Restriction = "PropertyID IN (41)";
            QueryConditions.Add(qBiotopTyp);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (20))";
            DiversityWorkbench.QueryCondition qChronostratigraphy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "Chron.strat", "Chronostratigraphy",
                "Chronostratigraphy", false, false, false, false, "PlotID", "Property.PropertyID.20");
            qChronostratigraphy.Restriction = "PropertyID IN (20)";
            QueryConditions.Add(qChronostratigraphy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (1))";
            DiversityWorkbench.QueryCondition qEUNIS = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "Eunis", "Eunis", "Eunis",
                false, false, false, false, "PlotID", "Property.PropertyID.1");
            qEUNIS.Restriction = "PropertyID IN (1)";
            QueryConditions.Add(qEUNIS);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (2))";
            DiversityWorkbench.QueryCondition qEUNIS2012 = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "Eunis2012", "Eunis 2012", "Eunis 2012",
                false, false, false, false, "PlotID", "Property.PropertyID.2");
            qEUNIS2012.Restriction = "PropertyID IN (2)";
            QueryConditions.Add(qEUNIS2012);


            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (10))";
            DiversityWorkbench.QueryCondition qGeographicRegions = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "Geo.reg.", "Geographic regions", "Geographic regions",
                false, false, false, false, "PlotID", "Property.PropertyID.10");
            qGeographicRegions.Restriction = "PropertyID IN (10)";
            QueryConditions.Add(qGeographicRegions);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (40))";
            DiversityWorkbench.QueryCondition qLebensraumTyp = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "Leb.r.typ.", "Lebensraumtyp", "Lebensraumtyp",
                false, false, false, false, "PlotID", "Property.PropertyID.40");
            qLebensraumTyp.Restriction = "PropertyID IN (40)";
            QueryConditions.Add(qLebensraumTyp);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (30))";
            DiversityWorkbench.QueryCondition qLithoStratigraphy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "Litho.strat.", "Lithostratigraphy",
                "Lithostratigraphy", false, false, false, false, "PlotID", "Property.PropertyID.30");
            qLithoStratigraphy.Restriction = "PropertyID IN (30)";
            QueryConditions.Add(qLithoStratigraphy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (50))";
            DiversityWorkbench.QueryCondition qPflanzenges = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "DisplayText", "Site property", "Pflanzenges.", "Pflanzengesellschaften", "Pflanzengesellschaften",
                false, false, false, false, "PlotID", "Property.PropertyID.50");
            qPflanzenges.Restriction = "PropertyID IN (50)";
            QueryConditions.Add(qPflanzenges);


            #region Presence

            DiversityWorkbench.QueryCondition _qBiotop_present = new QueryCondition();
            _qBiotop_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qBiotop_present.Entity = "Property.PropertyID.41";
            _qBiotop_present.DisplayText = "BiotopTyp.";
            _qBiotop_present.Description = "If any Biotop type is present";
            _qBiotop_present.TextFixed = true;
            _qBiotop_present.Table = "SamplingPlotProperty";
            _qBiotop_present.IdentityColumn = "PlotID";
            _qBiotop_present.Column = "PropertyID";
            _qBiotop_present.Restriction = "PropertyID = 41";
            _qBiotop_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qBiotop_present.QueryGroup = "Site property presence";
            _qBiotop_present.useGroupAsEntityForGroups = true;
            DiversityWorkbench.QueryCondition qBiotop_present = new QueryCondition(_qBiotop_present);
            QueryConditions.Add(qBiotop_present);

            DiversityWorkbench.QueryCondition _qChronostratigraphy_present = new QueryCondition();
            _qChronostratigraphy_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qChronostratigraphy_present.Entity = "Property.PropertyID.20";
            _qChronostratigraphy_present.DisplayText = "Chron.strat.";
            _qChronostratigraphy_present.Description = "If any Chronostratigraphy is present";
            _qChronostratigraphy_present.TextFixed = true;
            _qChronostratigraphy_present.Table = "SamplingPlotProperty";
            _qChronostratigraphy_present.IdentityColumn = "PlotID";
            _qChronostratigraphy_present.Column = "PropertyID";
            _qChronostratigraphy_present.Restriction = "PropertyID = 20";
            _qChronostratigraphy_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qChronostratigraphy_present.QueryGroup = "Site property presence";
            _qChronostratigraphy_present.useGroupAsEntityForGroups = true;
            DiversityWorkbench.QueryCondition qChronostratigraphy_present = new QueryCondition(_qChronostratigraphy_present);
            QueryConditions.Add(qChronostratigraphy_present);

            DiversityWorkbench.QueryCondition _qEunis2003present = new QueryCondition();
            _qEunis2003present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qEunis2003present.Entity = "Property.PropertyID.1";
            _qEunis2003present.DisplayText = "Eunis";
            _qEunis2003present.Description = "If any EUNIS 2003 is present";
            _qEunis2003present.TextFixed = true;
            _qEunis2003present.Table = "SamplingPlotProperty";
            _qEunis2003present.IdentityColumn = "PlotID";
            _qEunis2003present.Column = "PropertyID";
            _qEunis2003present.Restriction = "PropertyID = 1";
            _qEunis2003present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qEunis2003present.QueryGroup = "Site property presence";
            _qEunis2003present.useGroupAsEntityForGroups = true;
            DiversityWorkbench.QueryCondition qEunis2003present = new QueryCondition(_qEunis2003present);
            QueryConditions.Add(qEunis2003present);

            DiversityWorkbench.QueryCondition _qEunis2012present = new QueryCondition();
            _qEunis2012present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qEunis2012present.Entity = "Property.PropertyID.2";
            _qEunis2012present.DisplayText = "Eunis2012";
            _qEunis2012present.Description = "If any EUNIS 2012 is present";
            _qEunis2012present.TextFixed = true;
            _qEunis2012present.Table = "SamplingPlotProperty";
            _qEunis2012present.IdentityColumn = "PlotID";
            _qEunis2012present.Column = "PropertyID";
            _qEunis2012present.Restriction = "PropertyID = 2";
            _qEunis2012present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qEunis2012present.QueryGroup = "Site property presence";
            _qEunis2012present.useGroupAsEntityForGroups = true;
            DiversityWorkbench.QueryCondition qEunis2012present = new QueryCondition(_qEunis2012present);
            QueryConditions.Add(qEunis2012present);

            DiversityWorkbench.QueryCondition _qGeographicRegions_present = new QueryCondition();
            _qGeographicRegions_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qGeographicRegions_present.Entity = "Property.PropertyID.10";
            _qGeographicRegions_present.DisplayText = "Geo.reg.";
            _qGeographicRegions_present.Description = "If any Geographic region is present";
            _qGeographicRegions_present.TextFixed = true;
            _qGeographicRegions_present.Table = "SamplingPlotProperty";
            _qGeographicRegions_present.IdentityColumn = "PlotID";
            _qGeographicRegions_present.Column = "PropertyID";
            _qGeographicRegions_present.Restriction = "PropertyID = 10";
            _qGeographicRegions_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qGeographicRegions_present.QueryGroup = "Site property presence";
            _qGeographicRegions_present.useGroupAsEntityForGroups = true;
            DiversityWorkbench.QueryCondition qGeographicRegions_present = new QueryCondition(_qGeographicRegions_present);
            QueryConditions.Add(qGeographicRegions_present);

            DiversityWorkbench.QueryCondition _qLebensraumTyp_present = new QueryCondition();
            _qLebensraumTyp_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qLebensraumTyp_present.Entity = "Property.PropertyID.40";
            _qLebensraumTyp_present.DisplayText = "Leb.r.typ.";
            _qLebensraumTyp_present.Description = "If any Lebensraumtyp is present";
            _qLebensraumTyp_present.TextFixed = true;
            _qLebensraumTyp_present.Table = "SamplingPlotProperty";
            _qLebensraumTyp_present.IdentityColumn = "PlotID";
            _qLebensraumTyp_present.Column = "PropertyID";
            _qLebensraumTyp_present.Restriction = "PropertyID = 40";
            _qLebensraumTyp_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qLebensraumTyp_present.QueryGroup = "Site property presence";
            _qLebensraumTyp_present.useGroupAsEntityForGroups = true;
            DiversityWorkbench.QueryCondition qLebensraumTyp_present = new QueryCondition(_qLebensraumTyp_present);
            QueryConditions.Add(qLebensraumTyp_present);

            DiversityWorkbench.QueryCondition _qLithostratigraphy_present = new QueryCondition();
            _qLithostratigraphy_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qLithostratigraphy_present.Entity = "Property.PropertyID.30";
            _qLithostratigraphy_present.DisplayText = "Litho.strat.";
            _qLithostratigraphy_present.Description = "If any Lithostratigraphy is present";
            _qLithostratigraphy_present.TextFixed = true;
            _qLithostratigraphy_present.Table = "SamplingPlotProperty";
            _qLithostratigraphy_present.IdentityColumn = "PlotID";
            _qLithostratigraphy_present.Column = "PropertyID";
            _qLithostratigraphy_present.Restriction = "PropertyID = 30";
            _qLithostratigraphy_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qLithostratigraphy_present.QueryGroup = "Site property presence";
            _qLithostratigraphy_present.useGroupAsEntityForGroups = true;
            DiversityWorkbench.QueryCondition qLithostratigraphy_present = new QueryCondition(_qLithostratigraphy_present);
            QueryConditions.Add(qLithostratigraphy_present);

            DiversityWorkbench.QueryCondition _qPflanzenges_present = new QueryCondition();
            _qPflanzenges_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qPflanzenges_present.Entity = "Property.PropertyID.50";
            _qPflanzenges_present.DisplayText = "Pflanzenges.";
            _qPflanzenges_present.Description = "If any Pflanzengesellschaft is present";
            _qPflanzenges_present.TextFixed = true;
            _qPflanzenges_present.Table = "SamplingPlotProperty";
            _qPflanzenges_present.IdentityColumn = "PlotID";
            _qPflanzenges_present.Column = "PropertyID";
            _qPflanzenges_present.Restriction = "PropertyID = 50";
            _qPflanzenges_present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qPflanzenges_present.QueryGroup = "Site property presence";
            _qPflanzenges_present.useGroupAsEntityForGroups = true;
            DiversityWorkbench.QueryCondition qPflanzenges_present = new QueryCondition(_qPflanzenges_present);
            QueryConditions.Add(qPflanzenges_present);

            #endregion

            #endregion

            #region Link to ScientificTerms

            DiversityWorkbench.QueryCondition _qTermSelection = new QueryCondition();
            _qTermSelection.QueryType = QueryCondition.QueryTypes.Module;
            DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
            _qTermSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)ST;
            _qTermSelection.DisplayText = "Terms";
            _qTermSelection.DisplayLongText = "Selection of terms";
            _qTermSelection.Table = "SamplingPlotProperty";
            _qTermSelection.IdentityColumn = "PlotID";
            _qTermSelection.Column = "PropertyURI";
            _qTermSelection.UpperValue = "DisplayText";
            _qTermSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            _qTermSelection.QueryGroup = "Property";
            _qTermSelection.Description = "All terms from the list";
            _qTermSelection.QueryType = QueryCondition.QueryTypes.Module;
            QueryConditions.Add(_qTermSelection);

            #endregion

            #region PropertyHierarchy

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (41))";
            DiversityWorkbench.QueryCondition qBiotopTypHierarchy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "PropertyHierarchyCache", "Site property hierarchy", "BiotopTyp.", "Biotop type", "Biotop type",
                false, false, false, false, "PlotID", "Property.PropertyID.41");
            qBiotopTypHierarchy.Restriction = "PropertyID IN (41)";
            QueryConditions.Add(qBiotopTypHierarchy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (20))";
            DiversityWorkbench.QueryCondition qChronostratigraphyHierarchy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "PropertyHierarchyCache", "Site property hierarchy", "Chron.strat", "Chronostratigraphy",
                "Chronostratigraphy", false, false, false, false, "PlotID", "Property.PropertyID.20");
            qChronostratigraphyHierarchy.Restriction = "PropertyID IN (20)";
            QueryConditions.Add(qChronostratigraphyHierarchy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (1))";
            DiversityWorkbench.QueryCondition qEUNISHierarchy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "PropertyHierarchyCache", "Site property hierarchy", "Eunis", "Eunis", "Eunis",
                false, false, false, false, "PlotID", "Property.PropertyID.1");
            qEUNISHierarchy.Restriction = "PropertyID IN (1)";
            QueryConditions.Add(qEUNISHierarchy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (2))";
            DiversityWorkbench.QueryCondition qEUNIS2012Hierarchy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "PropertyHierarchyCache", "Site property hierarchy", "Eunis2012", "Eunis 2012", "Eunis 2012",
                false, false, false, false, "PlotID", "Property.PropertyID.2");
            qEUNIS2012Hierarchy.Restriction = "PropertyID IN (2)";
            QueryConditions.Add(qEUNIS2012Hierarchy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (10))";
            DiversityWorkbench.QueryCondition qGeographicRegionsHierarchy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "PropertyHierarchyCache", "Site property hierarchy", "Geo.reg.", "Geographic regions", "Geographic regions",
                false, false, false, false, "PlotID", "Property.PropertyID.10");
            qGeographicRegionsHierarchy.Restriction = "PropertyID IN (10)";
            QueryConditions.Add(qGeographicRegionsHierarchy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (40))";
            DiversityWorkbench.QueryCondition qLebensraumTypHierarchy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "PropertyHierarchyCache", "Site property hierarchy", "Leb.r.typ.", "Lebensraumtyp", "Lebensraumtyp",
                false, false, false, false, "PlotID", "Property.PropertyID.40");
            qLebensraumTypHierarchy.Restriction = "PropertyID IN (40)";
            QueryConditions.Add(qLebensraumTypHierarchy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (30))";
            DiversityWorkbench.QueryCondition qLithoStratigraphyHierarchy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "PropertyHierarchyCache", "Site property hierarchy", "Litho.strat.", "Lithostratigraphy",
                "Lithostratigraphy", false, false, false, false, "PlotID", "Property.PropertyID.30");
            qLithoStratigraphyHierarchy.Restriction = "PropertyID IN (30)";
            QueryConditions.Add(qLithoStratigraphyHierarchy);

            SQL = " FROM " + Prefix + "SamplingPlotAllProperties() " +
                " WHERE (PropertyID IN (50))";
            DiversityWorkbench.QueryCondition qPflanzengesHierarchy = new DiversityWorkbench.QueryCondition(false, "SamplingPlotProperty",
                "PlotID", true, SQL, "PropertyHierarchyCache", "Site property hierarchy", "Pflanzenges.", "Pflanzengesellschaften", "Pflanzengesellschaften",
                false, false, false, false, "PlotID", "Property.PropertyID.50");
            qPflanzengesHierarchy.Restriction = "PropertyID IN (50)";
            QueryConditions.Add(qPflanzengesHierarchy);

            #endregion

            #region Localisation

            //#region Coordinates

            //#region Geography

            //Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlotLocalisation", "Geography");
            ////SQL = " FROM SamplingPlot INNER JOIN " +
            ////    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID " +
            ////    " WHERE (NOT SamplingPlotLocalisation.Geography IS NULL)";
            //DiversityWorkbench.QueryCondition _qLocalisationGeography = new QueryCondition();
            //_qLocalisationGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            ////_qLocalisationGeography.Entity = "SamplingPlotLocalisation.Geography";
            ////_qLocalisationGeography.SqlFromClause = SQL;
            //_qLocalisationGeography.TextFixed = false;
            //_qLocalisationGeography.Description = Description;
            //_qLocalisationGeography.Table = "SamplingPlotLocalisation";
            //_qLocalisationGeography.IdentityColumn = "PlotID";
            //_qLocalisationGeography.Column = "Geography";
            ////_qLocalisationGeography.ForeignKeyTable = "SamplingPlot";
            //_qLocalisationGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            //_qLocalisationGeography.QueryGroup = "Plot localisation";
            //_qLocalisationGeography.QueryType = QueryCondition.QueryTypes.Geography;
            //DiversityWorkbench.QueryCondition qLocalisationGeography = new QueryCondition(_qLocalisationGeography);
            //QueryConditions.Add(qLocalisationGeography);

            //#endregion

            //#region Average Values

            //SQL = "";
            ////FROM SamplingPlot INNER JOIN " +
            ////    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID " +
            ////    " WHERE (NOT SamplingPlotLocalisation.AverageLatitudeCache IS NULL)";
            //DiversityWorkbench.QueryCondition qLatitude = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation", "PlotID", true, SQL, "AverageLatitudeCache", "Plot localisation", "Av. Lat.", "Average latitude", "Average latitude calculated from the given values", false, false, true, false, "PlotID");
            //QueryConditions.Add(qLatitude);

            //SQL = "";
            ////FROM SamplingPlot INNER JOIN " +
            ////    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID " +
            ////    " WHERE (NOT SamplingPlotLocalisation.AverageLongitudeCache IS NULL)";
            //DiversityWorkbench.QueryCondition qLongitude = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation", "PlotID", true, SQL, "AverageLongitudeCache", "Plot localisation", "Av. Lon.", "Average longitude", "Average longitude calculated from the given values", false, false, true, false, "PlotID");
            //QueryConditions.Add(qLongitude);

            //#endregion

            //#region Presence

            //DiversityWorkbench.QueryCondition _qWGS84present = new QueryCondition();
            //_qWGS84present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            //_qWGS84present.Entity = "LocalisationSystem.LocalisationSystemID.8";
            //_qWGS84present.DisplayText = DiversityWorkbench.CollectionSpecimenText.WGS84_present;// "WGS84 present";
            //_qWGS84present.Description = DiversityWorkbench.CollectionSpecimenText.If_any_WGS84_coordinates_are_present;// "If any WGS84 coordinates are present";
            //_qWGS84present.TextFixed = true;
            //_qWGS84present.Table = "SamplingPlotLocalisation";
            //_qWGS84present.IdentityColumn = "PlotID";
            ////_qWGS84present.ForeignKey = "PlotID";
            //_qWGS84present.Column = "PlotID";
            ////_qWGS84present.ForeignKeyTable = "SamplingPlot";
            //_qWGS84present.Restriction = "LocalisationSystemID = 8";
            //_qWGS84present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            //_qWGS84present.QueryGroup = "Plot localisation";
            //DiversityWorkbench.QueryCondition qWGS84present = new QueryCondition(_qWGS84present);
            //QueryConditions.Add(qWGS84present);

            //#endregion

            //#endregion

            #region Altitude

            //DiversityWorkbench.QueryCondition qAltitudeFrom = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation", "PlotID", true, "", "Location1", "Plot localisation", "From Alt.", "Altitude", "Altitude, lower level", false, false, false, false, "PlotID");
            //qAltitudeFrom.Restriction = "LocalisationSystemID IN (4, 5)";
            //QueryConditions.Add(qAltitudeFrom);

            //DiversityWorkbench.QueryCondition qAltitudeTo = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation", "PlotID", true, "", "Location2", "Plot localisation", "To Alt.", "Altitude", "Altitude, upper level", false, false, false, false, "PlotID");
            //qAltitudeTo.Restriction = "LocalisationSystemID IN (4, 5)";
            //QueryConditions.Add(qAltitudeTo);
            //SQL = "";

            //FROM SamplingPlot INNER JOIN " +
            //    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID " +
            //    " WHERE (SamplingPlotLocalisation.LocalisationSystemID IN (4, 5))";
            DiversityWorkbench.QueryCondition qAltitude = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation", "PlotID", true, SQL, "AverageAltitudeCache", "Plot localisation", "Av. Alt.", "Average altitude", "Average altitude calculated from the given values", false, false, true, false, "PlotID");
            qAltitude.Restriction = "LocalisationSystemID IN (4, 5)";
            QueryConditions.Add(qAltitude);

            DiversityWorkbench.QueryCondition _qAltitidepresent = new QueryCondition();
            _qAltitidepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qAltitidepresent.Entity = "LocalisationSystem.LocalisationSystemID.4";
            _qAltitidepresent.Description = DiversityWorkbench.CollectionSpecimenText.If_the_altitude_is_present;// "If the altitude is present";
            _qAltitidepresent.DisplayText = DiversityWorkbench.CollectionSpecimenText.Alt_present;// "Alt. present";
            _qAltitidepresent.TextFixed = true;
            _qAltitidepresent.Table = "SamplingPlotLocalisation";
            _qAltitidepresent.IdentityColumn = "PlotID";
            //_qAltitidepresent.ForeignKey = "PlotID";
            _qAltitidepresent.Column = "PlotID";
            //_qAltitidepresent.ForeignKeyTable = "SamplingPlot";
            _qAltitidepresent.Restriction = "LocalisationSystemID = 4";
            _qAltitidepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qAltitidepresent.QueryGroup = "Plot localisation";
            DiversityWorkbench.QueryCondition qAltitidepresent = new QueryCondition(_qAltitidepresent);
            QueryConditions.Add(qAltitidepresent);

            #endregion

            #region Exposition and slope

            SQL = "";
            //FROM SamplingPlot INNER JOIN " +
            //    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID " +
            //    " WHERE (SamplingPlotLocalisation.LocalisationSystemID = 10)";
            DiversityWorkbench.QueryCondition qExposition = new DiversityWorkbench.QueryCondition(false,
                "SamplingPlotLocalisation", "PlotID", true, SQL, "Location1",
                "Plot localisation", "Exposition", "Exposition", "Exposition", false, false, true, false,
                "PlotID", "LocalisationSystem.LocalisationSystemID.10");
            qExposition.Restriction = "LocalisationSystemID = 10";
            QueryConditions.Add(qExposition);

            SQL = "";
            //FROM SamplingPlot INNER JOIN " +
            //    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID " +
            //    " WHERE (SamplingPlotLocalisation.LocalisationSystemID = 11)";
            DiversityWorkbench.QueryCondition qSlope = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location1", "Plot localisation", "Slope", "Slope", "Slope", false, false, true,
                false, "PlotID", "LocalisationSystem.LocalisationSystemID.11");
            qSlope.Restriction = "LocalisationSystemID = 11";
            QueryConditions.Add(qSlope);

            #endregion

            #region Place Hierarchy

            SQL = "";
            DiversityWorkbench.QueryCondition qPlaceHierarchy = new DiversityWorkbench.QueryCondition(true, "ViewSamplingPlotAllLocalisations", // "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location1", "Plot localisation hierarchy", "Place", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.7");
            qPlaceHierarchy.Restriction = "LocalisationSystemID = 7";
            QueryConditions.Add(qPlaceHierarchy);

            SQL = "";
            DiversityWorkbench.QueryCondition qPlaceHierarchy2 = new DiversityWorkbench.QueryCondition(true, "ViewSamplingPlotAllLocalisations",
                "PlotID", true, SQL, "Location1", "Plot localisation hierarchy", "Place 2", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.18");
            qPlaceHierarchy2.Restriction = "LocalisationSystemID = 18";
            QueryConditions.Add(qPlaceHierarchy2);


            SQL = "";
            DiversityWorkbench.QueryCondition qPlaceHierarchy3 = new DiversityWorkbench.QueryCondition(true, "ViewSamplingPlotAllLocalisations",
                "PlotID", true, SQL, "Location1", "Plot localisation hierarchy", "Place 3", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.19");
            qPlaceHierarchy3.Restriction = "LocalisationSystemID = 19";
            QueryConditions.Add(qPlaceHierarchy3);

            SQL = "";
            DiversityWorkbench.QueryCondition qPlaceHierarchy4 = new DiversityWorkbench.QueryCondition(true, "ViewSamplingPlotAllLocalisations",
                "PlotID", true, SQL, "Location1", "Plot localisation hierarchy", "Place 4", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.20");
            qPlaceHierarchy4.Restriction = "LocalisationSystemID = 20";
            QueryConditions.Add(qPlaceHierarchy4);

            DiversityWorkbench.QueryCondition qPlaceHierarchy5 = new DiversityWorkbench.QueryCondition(true, "ViewSamplingPlotAllLocalisations",
                "PlotID", "Location1", "Plot localisation hierarchy", "Place 5", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer");
            qPlaceHierarchy5.Restriction = "LocalisationSystemID = 21";
            QueryConditions.Add(qPlaceHierarchy5);

            #endregion

            #region Place

            SQL = "";
            DiversityWorkbench.QueryCondition qPlace = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location1", "Plot localisation", "Place", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.7");
            qPlace.Restriction = "LocalisationSystemID = 7";
            QueryConditions.Add(qPlace);

            SQL = "";
            DiversityWorkbench.QueryCondition qLinkToGazetteer = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location2", "Plot localisation", "Gazetteer", "Link to gazetteer",
                "The link to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.7");
            qLinkToGazetteer.Restriction = "LocalisationSystemID = 7";
            QueryConditions.Add(qLinkToGazetteer);

            DiversityWorkbench.QueryCondition _qPlacepresent = new QueryCondition();
            _qPlacepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qPlacepresent.Entity = "LocalisationSystem.LocalisationSystemID.7";
            _qPlacepresent.Description = DiversityWorkbench.CollectionSpecimenText.If_a_named_place_is_present;// "If a named place is present";
            _qPlacepresent.DisplayText = DiversityWorkbench.CollectionSpecimenText.Place_present;// "Place present";
            _qPlacepresent.TextFixed = true;
            _qPlacepresent.Table = "SamplingPlotLocalisation";
            _qPlacepresent.IdentityColumn = "PlotID";
            _qPlacepresent.ForeignKey = "PlotID";
            _qPlacepresent.Column = "PlotID";
            _qPlacepresent.ForeignKeyTable = "SamplingPlot";
            _qPlacepresent.Restriction = "LocalisationSystemID = 7";
            _qPlacepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qPlacepresent.QueryGroup = "Plot localisation";
            DiversityWorkbench.QueryCondition qPlacepresent = new QueryCondition(_qPlacepresent);
            QueryConditions.Add(qPlacepresent);

            SQL = "";
            DiversityWorkbench.QueryCondition qPlace2 = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location1", "Plot localisation", "Place 2", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.18");
            qPlace2.Restriction = "LocalisationSystemID = 18";
            QueryConditions.Add(qPlace2);

            SQL = "";
            DiversityWorkbench.QueryCondition qPlace3 = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location1", "Plot localisation", "Place 3", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.19");
            qPlace3.Restriction = "LocalisationSystemID = 19";
            QueryConditions.Add(qPlace3);

            SQL = "";
            DiversityWorkbench.QueryCondition qPlace4 = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location1", "Plot localisation", "Place 4", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.20");
            qPlace4.Restriction = "LocalisationSystemID = 20";
            QueryConditions.Add(qPlace4);

            //SQL = "";
            //DiversityWorkbench.QueryCondition qPlace5 = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
            //    "PlotID", true, SQL, "Location1", "Plot localisation", "Place 5", "Name of a place",
            //    "The name of a place for datasets linked to the DiversityGazetteer", "PlotID", "LocalisationSystem.LocalisationSystemID.21");
            //qPlace5.Restriction = "LocalisationSystemID = 21";
            //QueryConditions.Add(qPlace5);

            DiversityWorkbench.QueryCondition qPlace5 = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
                "PlotID", "Location1", "Plot localisation", "Place 5", "Name of a place",
                "The name of a place for datasets linked to the DiversityGazetteer");
            qPlace5.Restriction = "LocalisationSystemID = 21";
            QueryConditions.Add(qPlace5);


            #endregion

            #region TK25

            SQL = "";
            //FROM SamplingPlot INNER JOIN " +
            //    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID " +
            //    " WHERE (SamplingPlotLocalisation.LocalisationSystemID = 3)";
            DiversityWorkbench.QueryCondition qTK25 = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location1", "Plot localisation", "TK25", "TK25",
                "The UTM grid used in D, A and CH based on the maps 1:25000, former MTB", "PlotID", "LocalisationSystem.LocalisationSystemID.3");
            qTK25.Restriction = "LocalisationSystemID = 3";
            QueryConditions.Add(qTK25);

            SQL = "";
            //FROM SamplingPlot INNER JOIN " +
            //    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID " +
            //    " WHERE (SamplingPlotLocalisation.LocalisationSystemID = 3)";
            DiversityWorkbench.QueryCondition qQuadrant = new DiversityWorkbench.QueryCondition(true, "SamplingPlotLocalisation",
                "PlotID", true, SQL, "Location2", "Plot localisation", "Quadrant", "Quadrant",
                "", "PlotID", "LocalisationSystem.LocalisationSystemID.3.Quadrant");
            qQuadrant.Restriction = "LocalisationSystemID = 3";
            QueryConditions.Add(qQuadrant);

            DiversityWorkbench.QueryCondition _qTK25present = new QueryCondition();
            _qTK25present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qTK25present.Entity = "LocalisationSystem.LocalisationSystemID.3";
            _qTK25present.Description = DiversityWorkbench.CollectionSpecimenText.If_a_TK25_entry_is_present;// "If a TK25 entry is present";
            _qTK25present.DisplayText = DiversityWorkbench.CollectionSpecimenText.TK25_present;// "TK25 present";
            _qTK25present.TextFixed = true;
            _qTK25present.Table = "SamplingPlotLocalisation";
            _qTK25present.IdentityColumn = "PlotID";
            //_qTK25present.ForeignKey = "PlotID";
            _qTK25present.Column = "PlotID";
            //_qTK25present.ForeignKeyTable = "SamplingPlot";
            _qTK25present.Restriction = "LocalisationSystemID = 3";
            _qTK25present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
            _qTK25present.QueryGroup = "Plot localisation";
            DiversityWorkbench.QueryCondition qTK25present = new QueryCondition(_qTK25present);
            QueryConditions.Add(qTK25present);

            #endregion

            //Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlotLocalisation", "Location1");
            //SQL = "";
            ////FROM SamplingPlot INNER JOIN " +
            ////    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID ";
            //DiversityWorkbench.QueryCondition qLocation1 = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation", "PlotID", true, SQL, "Location1", "Plot localisation", "Loc.1", "Location 1", Description, false, false, false, false, "PlotID");
            //QueryConditions.Add(qLocation1);

            //Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlotLocalisation", "Location2");
            //SQL = "";
            ////FROM SamplingPlot INNER JOIN " +
            ////    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID ";
            //DiversityWorkbench.QueryCondition qLocation2 = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation", "PlotID", true, SQL, "Location2", "Plot localisation", "Loc.2", "Location 2", Description, false, false, false, false, "PlotID");
            //QueryConditions.Add(qLocation2);

            //Description = DiversityWorkbench.Functions.ColumnDescription("SamplingPlotLocalisation", "LocationNotes");
            //SQL = "";
            ////FROM SamplingPlot INNER JOIN " +
            ////    " SamplingPlotLocalisation ON SamplingPlot.PlotID = SamplingPlotLocalisation.PlotID ";
            //DiversityWorkbench.QueryCondition qLocationNotes = new DiversityWorkbench.QueryCondition(false, "SamplingPlotLocalisation", "PlotID", true, SQL, "LocationNotes", "Plot localisation", "Notes", "Location notes", Description, false, false, false, false, "PlotID");
            //QueryConditions.Add(qLocationNotes);

            #endregion

            return QueryConditions;
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
        //            this._ServerConnection.DatabaseName = "DiversitySamplingPlots";
        //        }
        //        this._ServerConnection.ModuleName = "DiversitySamplingPlots";
        //    }
        //}

        //public override System.Collections.Generic.List<string> DatabaseList()
        //{
        //    if (this._DatabaseList == null)
        //    {
        //        this._DatabaseList = new List<string>();
        //        this._DatabaseList.Add("DiversitySamplingPlots");
        //    }
        //    return this._DatabaseList;
        //}
        #endregion

        #region TK25

        public System.Collections.Generic.Dictionary<string, string> TK25(float Longitude, float Latitude, int DepthForQuadrant)
        {
            string Lng = Longitude.ToString().Replace(",", ".");
            string Lat = Latitude.ToString().Replace(",", ".");
            System.Collections.Generic.Dictionary<string, string> TK25 = new Dictionary<string, string>();
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "DECLARE @Point geography; " +
                "SET @Point = geography::STGeomFromText('POINT (" + Lng + " " + Lat + ")', 4326); " +
                "SELECT      " +
                "  PlotID " +
                ", PlotGeography.STAsText() AS Polygon " +
                ", SUBSTRING(PlotIdentifier, 6, 4) AS TK25 " +
                ", PlotDescription AS TK25_Name " +
                ", PlotGeography.STPointN(1).ToString() AS Point1 " +
                ", PlotGeography.STPointN(2).ToString() AS Point2 " +
                ", PlotGeography.STPointN(3).ToString() AS Point3 " +
                ", PlotGeography.STPointN(4).ToString() AS Point4 " +
                "FROM  SamplingPlot " +
                "WHERE (PlotIdentifier LIKE 'TK25_%') " +
                "AND @Point.STDisjoint(PlotGeography) = 0";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                System.Drawing.RectangleF Quadrant = new System.Drawing.RectangleF();
                TK25.Add("TK25", dt.Rows[0]["TK25"].ToString());
                TK25.Add("Name", dt.Rows[0]["TK25_Name"].ToString());
                TK25.Add("Quadrant", this.QuadrantForTK25(Longitude, Latitude, dt.Rows[0]["Point1"].ToString(), dt.Rows[0]["Point2"].ToString(), dt.Rows[0]["Point3"].ToString(), dt.Rows[0]["Point4"].ToString(), DepthForQuadrant, ref Quadrant));
            }
            return TK25;
        }

        public System.Drawing.RectangleF TK25Quadrant(float Longitude, float Latitude, int DepthForQuadrant)
        {
            System.Drawing.RectangleF Quadrant = new System.Drawing.RectangleF();
            string Lng = Longitude.ToString().Replace(",", ".");
            string Lat = Latitude.ToString().Replace(",", ".");
            System.Collections.Generic.Dictionary<string, string> TK25 = new Dictionary<string, string>();
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "DECLARE @Point geography; " +
                "SET @Point = geography::STGeomFromText('POINT (" + Lng + " " + Lat + ")', 4326); " +
                "SELECT      " +
                "  PlotID " +
                ", PlotGeography.STAsText() AS Polygon " +
                ", SUBSTRING(PlotIdentifier, 6, 4) AS TK25 " +
                ", PlotDescription AS TK25_Name " +
                ", PlotGeography.STPointN(1).ToString() AS Point1 " +
                ", PlotGeography.STPointN(2).ToString() AS Point2 " +
                ", PlotGeography.STPointN(3).ToString() AS Point3 " +
                ", PlotGeography.STPointN(4).ToString() AS Point4 " +
                "FROM  SamplingPlot " +
                "WHERE (PlotIdentifier LIKE 'TK25_%') " +
                "AND @Point.STDisjoint(PlotGeography) = 0";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                System.Drawing.RectangleF _Quadrant = new System.Drawing.RectangleF();
                this.QuadrantForTK25(Longitude, Latitude, dt.Rows[0]["Point1"].ToString(), dt.Rows[0]["Point2"].ToString(), dt.Rows[0]["Point3"].ToString(), dt.Rows[0]["Point4"].ToString(), ref DepthForQuadrant, ref _Quadrant);
            }
            return Quadrant;
        }

        public string QuadrantForTK25(float Longitude, float Latitude, string Point1FromSql, string Point2FromSql, string Point3FromSql, string Point4FromSql, ref int Depth, ref System.Drawing.RectangleF Quadrant)
        {
            if (Depth == 0)
                return "";
            System.Drawing.PointF Point1 = this.PointFromSqlString(Point1FromSql);
            System.Drawing.PointF Point2 = this.PointFromSqlString(Point2FromSql);
            System.Drawing.PointF Point3 = this.PointFromSqlString(Point3FromSql);
            System.Drawing.PointF Point4 = this.PointFromSqlString(Point4FromSql);
            float MinX = Point1.X;
            if (Point2.X < MinX) MinX = Point2.X;
            if (Point3.X < MinX) MinX = Point3.X;
            if (Point4.X < MinX) MinX = Point4.X;
            float MaxX = Point1.X;
            if (Point2.X > MaxX) MaxX = Point2.X;
            if (Point3.X > MaxX) MaxX = Point3.X;
            if (Point4.X > MaxX) MaxX = Point4.X;
            float MinY = Point1.Y;
            if (Point2.Y < MinY) MinY = Point2.Y;
            if (Point3.Y < MinY) MinY = Point3.Y;
            if (Point4.Y < MinY) MinY = Point4.Y;
            float MaxY = Point1.Y;
            if (Point2.Y > MaxY) MaxY = Point2.Y;
            if (Point3.Y > MaxY) MaxY = Point3.Y;
            if (Point4.Y > MaxY) MaxY = Point4.Y;
            System.Drawing.RectangleF Rect = new System.Drawing.RectangleF(MinX, MinY, MaxX - MinX, MaxY - MinY);
            string Q = this.QuadrantForTK25(Longitude, Latitude, Rect, ref Quadrant, ref Depth);
            return Q;
        }

        private string QuadrantForTK25(float Longitude, float Latitude, string Point1FromSql, string Point2FromSql, string Point3FromSql, string Point4FromSql, int Depth, ref System.Drawing.RectangleF Quadrant)
        {
            string Q = "";
            System.Drawing.PointF Point1 = this.PointFromSqlString(Point1FromSql);
            System.Drawing.PointF Point2 = this.PointFromSqlString(Point2FromSql);
            System.Drawing.PointF Point3 = this.PointFromSqlString(Point3FromSql);
            System.Drawing.PointF Point4 = this.PointFromSqlString(Point4FromSql);
            float MinX = Point1.X;
            if (Point2.X < MinX) MinX = Point2.X;
            if (Point3.X < MinX) MinX = Point3.X;
            if (Point4.X < MinX) MinX = Point4.X;
            float MaxX = Point1.X;
            if (Point2.X > MaxX) MaxX = Point2.X;
            if (Point3.X > MaxX) MaxX = Point3.X;
            if (Point4.X > MaxX) MaxX = Point4.X;
            float MinY = Point1.Y;
            if (Point2.Y < MinY) MinY = Point2.Y;
            if (Point3.Y < MinY) MinY = Point3.Y;
            if (Point4.Y < MinY) MinY = Point4.Y;
            float MaxY = Point1.Y;
            if (Point2.Y > MaxY) MaxY = Point2.Y;
            if (Point3.Y > MaxY) MaxY = Point3.Y;
            if (Point4.Y > MaxY) MaxY = Point4.Y;
            System.Drawing.RectangleF Rect = new System.Drawing.RectangleF(MinX, MinY, MaxX - MinX, MaxY - MinY);
            Q = this.QuadrantForTK25(Longitude, Latitude, Rect, ref Quadrant, ref Depth);
            return Q;
        }

        private string QuadrantForTK25(float Longitude, float Latitude, System.Drawing.RectangleF Rect, ref System.Drawing.RectangleF Quadrant, ref int Depth)
        {
            string Q = "";
            System.Drawing.PointF PointObject = new System.Drawing.PointF(Longitude, Latitude);
            System.Drawing.RectangleF RectQuadrant = new System.Drawing.RectangleF();
            System.Drawing.PointF PointQuadrant = new System.Drawing.PointF();
            if (PointObject.X > Rect.X + Rect.Width / 2) // Quadrant 2, 4
            {
                // set the starting point for the Quadrant to the middle of the x-axis
                PointQuadrant.X = Rect.X + Rect.Width / 2;
                if (PointObject.Y > Rect.Y + Rect.Height / 2) // 4
                {
                    Q = "2";
                    PointQuadrant.Y = Rect.Y + Rect.Height / 2;
                }
                else
                {
                    Q = "4";
                    PointQuadrant.Y = Rect.Y;
                }
            }
            else
            {
                PointQuadrant.X = Rect.X;
                if (PointObject.Y > Rect.Y + Rect.Height / 2)
                {
                    Q = "1";
                    PointQuadrant.Y = Rect.Y + Rect.Height / 2;
                }
                else
                {
                    Q = "3";
                    PointQuadrant.Y = Rect.Y;
                }
            }
            RectQuadrant.X = PointQuadrant.X;
            RectQuadrant.Y = PointQuadrant.Y;
            RectQuadrant.Width = Rect.Width / 2;
            RectQuadrant.Height = Rect.Height / 2;

            Quadrant.X = PointQuadrant.X;
            Quadrant.Y = PointQuadrant.Y;
            Quadrant.Width = Rect.Width / 2;
            Quadrant.Height = Rect.Height / 2;
            Depth--;
            if (Depth > 0)
                Q += this.QuadrantForTK25(Longitude, Latitude, RectQuadrant, ref Quadrant, ref Depth);
            return Q;
        }

        private System.Drawing.PointF PointFromSqlString(string Point)
        {
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            System.Drawing.PointF P = new System.Drawing.PointF();
            string Long = Point.Substring(Point.IndexOf("(") + 1);
            string Lat = Long.Substring(Long.IndexOf(" ")).Trim();
            Lat = Lat.Substring(0, Lat.IndexOf(")"));
            Long = Long.Substring(0, Long.IndexOf(" ")).Trim();
            Lat = Lat.ToString().ToString(InvC).Replace(",", ".");
            Long = Long.ToString().ToString(InvC).Replace(",", ".");
            float fLong = 0;
            float fLat = 0;
            if (float.TryParse(Long, out fLong) && float.TryParse(Lat, out fLat))
            {
                P.X = fLong;
                P.Y = fLat;
            }
            return P;
        }

        #endregion

        #region Auxillary static functions to compress points

        public static System.Data.DataTable compressGeoPoints(System.Data.DataTable dtGeoPoints, int MaxPoints, string PointIDColumn, string ColumnLatitude, string ColumnLongitude)
        {
            System.Data.DataTable dtGeoPointsCopy = dtGeoPoints.Copy();

            System.Data.DataTable dtGeoPointsCompressed = new System.Data.DataTable();
            int i = dtGeoPoints.Rows.Count;
            int Rounding = 5;
            Double Precision = 0.005;
            Double Latitude;
            Double Longitude;
            while (dtGeoPointsCopy.Rows.Count > MaxPoints)
            {
                if (Rounding < 3)
                {
                    System.Collections.Generic.List<System.Data.DataRow> PointsToDelete = new List<System.Data.DataRow>();
                    for (int P = 0; P < dtGeoPointsCopy.Rows.Count; P++)
                    {
                        if (P < dtGeoPointsCopy.Rows.Count - 1 && P > 0)
                        {
                            Double Lat1;
                            Double Lon1;
                            Double Lat2;
                            Double Lon2;
                            Double Lat3;
                            Double Lon3;
                            if (Double.TryParse(dtGeoPointsCopy.Rows[P - 1][ColumnLatitude].ToString(), out Lat1) &&
                                Double.TryParse(dtGeoPointsCopy.Rows[P - 0][ColumnLatitude].ToString(), out Lat2) &&
                                Double.TryParse(dtGeoPointsCopy.Rows[P + 1][ColumnLatitude].ToString(), out Lat3) &&
                                Double.TryParse(dtGeoPointsCopy.Rows[P - 1][ColumnLongitude].ToString(), out Lon1) &&
                                Double.TryParse(dtGeoPointsCopy.Rows[P - 0][ColumnLongitude].ToString(), out Lon2) &&
                                Double.TryParse(dtGeoPointsCopy.Rows[P + 1][ColumnLongitude].ToString(), out Lon3))
                            {
                                Double Seite12 = System.Math.Sqrt(System.Math.Pow((Lat1 - Lat2), 2) + System.Math.Pow((Lon1 - Lon2), 2));
                                Double Seite23 = System.Math.Sqrt(System.Math.Pow((Lat3 - Lat2), 2) + System.Math.Pow((Lon3 - Lon2), 2));
                                Double Seite31 = System.Math.Sqrt(System.Math.Pow((Lat1 - Lat3), 2) + System.Math.Pow((Lon1 - Lon3), 2));
                                //Double Precision = 0.02;
                                //if (Rounding > 0) Precision = Precision / (Double)Rounding;
                                //if (Rounding < 0) Precision = Precision * System.Math.Sqrt(System.Math.Pow(Rounding, 2));
                                //if (Precision > 0.1)
                                //    Precision = 0.1;
                                if ((Seite12 + Seite23 - Seite31) < Seite31 * Precision)
                                    PointsToDelete.Add(dtGeoPointsCopy.Rows[P]);
                                if ((dtGeoPointsCopy.Rows.Count - PointsToDelete.Count) < MaxPoints)
                                    break;
                            }
                        }
                    }
                    foreach (System.Data.DataRow R in PointsToDelete)
                    {
                        R.Delete();
                    }
                    dtGeoPointsCopy.AcceptChanges();
                }
                if (dtGeoPointsCopy.Rows.Count < MaxPoints)
                    break;
                int Point = 0;
                dtGeoPointsCompressed.Clear();
                if (dtGeoPointsCompressed.Columns.Count == 0)
                {
                    System.Data.DataColumn CPoint = new System.Data.DataColumn(PointIDColumn, System.Type.GetType("System.Int16"));
                    System.Data.DataColumn CLat = new System.Data.DataColumn(ColumnLatitude, System.Type.GetType("System.Double"));
                    System.Data.DataColumn CLon = new System.Data.DataColumn(ColumnLongitude, System.Type.GetType("System.Double"));
                    dtGeoPointsCompressed.Columns.Add(CPoint);
                    dtGeoPointsCompressed.Columns.Add(CLat);
                    dtGeoPointsCompressed.Columns.Add(CLon);
                }
                if (Rounding > 2)
                {
                    foreach (System.Data.DataRow R in dtGeoPointsCopy.Rows)
                    {
                        Latitude = Double.Parse(R[ColumnLatitude].ToString());
                        Latitude = System.Math.Round(Latitude, Rounding);

                        Longitude = Double.Parse(R[ColumnLongitude].ToString());
                        Longitude = System.Math.Round(Longitude, Rounding);

                        if (Point == 0 ||
                            (Point > 0 &&
                            (dtGeoPointsCompressed.Rows[Point - 1][ColumnLatitude].ToString() != Latitude.ToString() ||
                            dtGeoPointsCompressed.Rows[Point - 1][ColumnLongitude].ToString() != Longitude.ToString())))
                        {

                            System.Data.DataRow RGeo = dtGeoPointsCompressed.NewRow();
                            RGeo[PointIDColumn] = Point;
                            RGeo[ColumnLatitude] = Latitude;
                            RGeo[ColumnLongitude] = Longitude;
                            dtGeoPointsCompressed.Rows.Add(RGeo);
                            Point++;
                        }
                    }
                    dtGeoPointsCopy = dtGeoPointsCompressed.Copy();
                }
                Rounding--;
                Precision += 0.005;
            }
            return dtGeoPointsCopy;
            //if (dtGeoPointsCompressed.Rows.Count == 0 && dtGeoPointsCopy.Rows.Count > 0)
            //{
            //    //System.Windows.Forms.MessageBox.Show("Too many points in GIS-object. Approximation with number of points reduced from " + dtGeoPoints.Rows.Count.ToString() + " to " + dtGeoPointsCopy.Rows.Count.ToString());
            //    return dtGeoPointsCopy;
            //}
            ////if (dtGeoPointsCompressed.Rows.Count < dtGeoPoints.Rows.Count)
            //    //System.Windows.Forms.MessageBox.Show("Too many points in GIS-object. Approximation with number of points reduced from " + dtGeoPoints.Rows.Count.ToString() + " to " + dtGeoPointsCompressed.Rows.Count.ToString());
            //return dtGeoPointsCompressed;
        }

        public static System.Data.DataTable dtGeoPointsFromSTAsText(string Geography, string GeographyType)
        {
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            System.Data.DataTable dtGeoPoints = new System.Data.DataTable();
            if (Geography.Length > 0)
            {
                string Lon = "";
                string Lat = "";
                int Point = 0;
                string Type = GeographyType.ToUpper();
                string Geometry = Geography;
                if (Geometry.StartsWith(Type))
                    Geometry = Geometry.Substring(Type.Length);
                Geometry = Geometry.Replace("(", "");
                Geometry = Geometry.Replace(")", "");
                string[] GeoPoints = Geometry.Split(new Char[] { ',' });
                foreach (string GeoPoint in GeoPoints)
                {
                    string[] Coordinates = GeoPoint.Trim().Split(new Char[] { ' ' });
                    Lon = Coordinates[0];
                    Lat = Coordinates[1];
                    Double Latitude;
                    Double Longitude;
                    if (double.TryParse(Lat, System.Globalization.NumberStyles.Float, InvC, out Latitude) &&
                        double.TryParse(Lon, System.Globalization.NumberStyles.Float, InvC, out Longitude))
                    {

                        System.Data.DataRow RGeo = dtGeoPoints.NewRow();
                        if (dtGeoPoints.Columns.Count == 0)
                        {
                            System.Data.DataColumn CPoint = new System.Data.DataColumn("ID", System.Type.GetType("System.Int16"));
                            System.Data.DataColumn CLat = new System.Data.DataColumn("Latitude", System.Type.GetType("System.Double"));
                            System.Data.DataColumn CLon = new System.Data.DataColumn("Longitude", System.Type.GetType("System.Double"));
                            dtGeoPoints.Columns.Add(CPoint);
                            dtGeoPoints.Columns.Add(CLat);
                            dtGeoPoints.Columns.Add(CLon);
                        }
                        RGeo["ID"] = Point;
                        RGeo["Latitude"] = Latitude;
                        RGeo["Longitude"] = Longitude;
                        dtGeoPoints.Rows.Add(RGeo);
                        Point++;
                    }
                }
                //while (Geometry.Length > 0)
                //{
                //    Lon = Geometry.Substring(0, Geometry.IndexOf(" ")).Trim();
                //    Geometry = Geometry.Substring(Geometry.IndexOf(" ")).Trim();
                //    if (Geometry.IndexOf(",") > -1)
                //    {
                //        Lat = Geometry.Substring(0, Geometry.IndexOf(",")).Trim();
                //        Geometry = Geometry.Substring(Geometry.IndexOf(",") + 1).Trim();
                //    }
                //    else
                //    {
                //        Lat = Geometry.Substring(0, Geometry.IndexOf(")")).Trim();
                //        Geometry = "";
                //    }
                //    Double Latitude;
                //    Double Longitude;
                //    if (double.TryParse(Lat, System.Globalization.NumberStyles.Float, InvC, out Latitude) &&
                //        double.TryParse(Lon, System.Globalization.NumberStyles.Float, InvC, out Longitude))
                //    {

                //        System.Data.DataRow RGeo = dtGeoPoints.NewRow();
                //        if (dtGeoPoints.Columns.Count == 0)
                //        {
                //            System.Data.DataColumn CPoint = new System.Data.DataColumn("ID", System.Type.GetType("System.Int16"));
                //            System.Data.DataColumn CLat = new System.Data.DataColumn("Latitude", System.Type.GetType("System.Double"));
                //            System.Data.DataColumn CLon = new System.Data.DataColumn("Longitude", System.Type.GetType("System.Double"));
                //            dtGeoPoints.Columns.Add(CPoint);
                //            dtGeoPoints.Columns.Add(CLat);
                //            dtGeoPoints.Columns.Add(CLon);
                //        }
                //        RGeo["ID"] = Point;
                //        RGeo["Latitude"] = Latitude;
                //        RGeo["Longitude"] = Longitude;
                //        dtGeoPoints.Rows.Add(RGeo);
                //        Point++;
                //    }
                //}
            }
            return dtGeoPoints;
        }

        #endregion

        #region static functions

        /// <summary>
        /// Getting the terms underneath a given term including the given term
        /// </summary>
        /// <param name="URL">The URL of the term (= BaseURL + PlotID)</param>
        /// <returns>A dictionary containing the URLs and the terms</returns>
        public static System.Collections.Generic.Dictionary<string, string> SubPlots(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartPlot(URL);
            getSubPlots(ref dt);
            getPlots(ref DD, dt);
            return DD;
        }


        #region Auxillary

        private static int? _ProjectID;
        //private static int ProjectID()
        //{
        //    if (_ProjectID == null)
        //    {
        //        string SQL = "SELECT ProjectID, Project FROM " + _SC.Prefix() + "ProjectList ORDER BY Project";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        ad.Fill(dt);
        //        if (dt.Rows.Count > 1)
        //        {
        //            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Project", "Please select a project");
        //            f.ShowDialog();
        //            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
        //            {
        //                int i;
        //                if (int.TryParse(f.SelectedValue, out i))
        //                    _ProjectID = i;
        //            }
        //        }
        //        else
        //            _ProjectID = int.Parse(dt.Rows[0][0].ToString());
        //    }
        //    return (int)_ProjectID;
        //}

        private static int ProjectID(string IDs)
        {
            if (_ProjectID == null)
            {
                string SQL = "SELECT DISTINCT ProjectID FROM SamplingProject P WHERE PlotID IN (" + IDs + ")";
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

        private static System.Data.DataTable getStartPlot(string URL)
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
                string SQL = "SELECT PlotID FROM " + _SC.Prefix() + "SamplingPlot N WHERE PlotID = " + ID;
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

        public static void getPlots(ref System.Collections.Generic.Dictionary<string, string> DD, System.Data.DataTable DT)
        {
            string SQL = "";
            try
            {
                foreach (System.Data.DataRow R in DT.Rows)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += R[0].ToString();
                }
                SQL = "SELECT U.BaseURL + cast(T.PlotID as varchar) AS URL, T.PlotIdentifier AS SamplingPlot FROM " + _SC.Prefix() + "SamplingPlot T, " + _SC.Prefix() + "ViewBaseURL U WHERE T.PlotID IN (" + SQL + ")";
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

        private static void getSubPlots(ref System.Data.DataTable DT)
        {
            string IDs = "";
            foreach (System.Data.DataRow R in DT.Rows)
            {
                if (IDs.Length > 0) IDs += ", ";
                IDs += R[0].ToString();
            }
            string SQL = "SELECT R.PlotID FROM " + _SC.Prefix() + "SamplingPlot T, " +
                _SC.Prefix() + "SamplingPlot R, " +
                _SC.Prefix() + "SamplingProject P " +
                " WHERE R.PlotID = T.PlotID " +
                " AND P.PlotID = T.PlotID " +
                " AND T.PartOfPlotID IN ( " + IDs + ") " +
                " AND P.ProjectID = " + ProjectID(IDs) + " " +
                " AND T.PlotID NOT IN (" + IDs + ")";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ad.Fill(DT);
                getSubPlots(ref DT);
            }
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

        //public static System.Collections.Generic.Dictionary<int, string> Sections(int ProjectID, DiversityWorkbench.ServerConnection SC)
        //{
        //    System.Collections.Generic.Dictionary<int, string> Sect = SamplingPlot.ChartSections(ProjectID, SC);
        //    return Sect;
        //}

        //public static Chart GetChart(string URI, int? ListID = null, int? Height = null, int? Width = null, int? ColumnWidth = null)
        //{
        //    if (_Charts == null)
        //        _Charts = new Dictionary<string, Chart>();

        //    DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);
        //    if (SC == null)
        //        return null;

        //    // Getting the ProjectID
        //    int ProjectID = 0;
        //    if (!int.TryParse(URI.Substring(URI.LastIndexOf("/") + 1), out ProjectID))
        //        return null;

        //    try
        //    {
        //        // Getting the colors
        //        System.Collections.Generic.Dictionary<int, System.Drawing.Color> ChartColors = new System.Collections.Generic.Dictionary<int, System.Drawing.Color>(); // SamplingPlot.ChartColors(ProjectID, SC); //  new Dictionary<int, System.Drawing.Color>();

        //        //System.Collections.Generic.Dictionary<int, System.Drawing.Color> ForeColors = SamplingPlot.ForeColors(ProjectID, SC); //  new Dictionary<int, System.Drawing.Color>();

        //        // Getting the images                                                                                                                                                // Getting the images
        //        System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> ChartImages = SamplingPlot.ChartImages(ProjectID, SC);

        //        //// Getting the groups
        //        ////System.Collections.Generic.List<int> ChartGroups = SamplingPlot.IDsOfGroups(ProjectID, SC);// SamplingPlot.ChartGroups(ProjectID, SC);

        //        System.Collections.Generic.List<int> IDsInSection = new List<int>();
        //        string Section = "";
        //        //if (ListID != null && ListID > 0)
        //        //{
        //        //    string SQL = "SELECT PlotID FROM SamplingProject WHERE (ProjectID = " + ListID.ToString() + ")";
        //        //    System.Data.DataTable dtSection = new System.Data.DataTable();
        //        //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSection);
        //        //    foreach (System.Data.DataRow R in dtSection.Rows)
        //        //    {
        //        //        int id;
        //        //        if (int.TryParse(R[0].ToString(), out id))
        //        //            IDsInSection.Add(id);
        //        //    }
        //        //    SQL = "SELECT Project FROM TaxonNameListProjectProxy WHERE(ProjectID = " + ListID.ToString() + ")";
        //        //    Section = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //        //}

        //        // Getting the data
        //        System.Data.DataTable dtTaxon = SamplingPlot.ChartData(ProjectID, SC);

        //        // getting the chart
        //        System.Collections.Generic.List<string> SortingColumns = new List<string>();
        //        SortingColumns.Add("PlotIdentifier");

        //        // getting the BaseURL
        //        string BaseURL = DiversityWorkbench.WorkbenchUnit.getBaseURIfromURI(URI);

        //        DiversityWorkbench.Chart C = new DiversityWorkbench.Chart(BaseURL, dtTaxon, "PlotID", "PartOfPlotID", "PlotIdentifier",
        //            ChartColors, SortingColumns, ChartImages,
        //            null,
        //            ListID, IDsInSection, Section,
        //            Width, Height, ColumnWidth);
        //        if (!_Charts.ContainsKey(URI))
        //            _Charts.Add(URI, C);

        //        return C;
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return null;
        //}


        //private static System.Collections.Generic.Dictionary<int, System.Drawing.Color> ChartColors(int ProjectID, DiversityWorkbench.ServerConnection SC)
        //{
        //    System.Collections.Generic.Dictionary<int, System.Drawing.Color> Colors = new System.Collections.Generic.Dictionary<int, System.Drawing.Color>();
        //    return Colors;
        //}

        //private static System.Collections.Generic.Dictionary<int, System.Drawing.Color> ForeColors(int ProjectID, DiversityWorkbench.ServerConnection SC)
        //{
        //    System.Collections.Generic.Dictionary<int, System.Drawing.Color> Colors = new System.Collections.Generic.Dictionary<int, System.Drawing.Color>();
        //    try
        //    {
        //        string SQL = "SELECT A.NameID, -16744448 AS ARGB " +
        //            "FROM TaxonName AS N INNER JOIN " +
        //            "TaxonNameTaxonomicRank_Enum AS R ON N.TaxonomicRank = R.Code INNER JOIN " +
        //            "TaxonAcceptedName AS A ON N.NameID = A.NameID " +
        //            "WHERE (ProjectID =  " + ProjectID.ToString() +
        //            ") AND (A.IgnoreButKeepForReference = 0) AND (R.DisplayOrder < 200)";
        //        System.Data.DataTable dtColor = new System.Data.DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtColor, SC.ConnectionString);
        //        foreach (System.Data.DataRow R in dtColor.Rows)
        //        {
        //            int ID;
        //            if (!int.TryParse(R[0].ToString(), out ID))
        //                continue;
        //            if (Colors.ContainsKey(ID))
        //                continue;
        //            int Color;
        //            if (int.TryParse(R[1].ToString(), out Color))
        //            {
        //                System.Drawing.SolidBrush S = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(Color));
        //                Colors.Add(ID, S.Color);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return Colors;
        //}

        //private static int? _ListID = null;
        private static System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> ChartImages(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<ChartImage>> Images = new Dictionary<int, System.Collections.Generic.List<ChartImage>>();
            try
            {
                int Height = Chart.ImageMaxHeight;
                int Width = Chart.ImageMaxWidth;
                string Message = "";
                string SQL = "SELECT PlotID, MIN(ResourceURI) AS ImageUri " +
                    "FROM SamplingPlotResource " +
                    "WHERE (ResourceURI LIKE '%.png' OR ResourceURI LIKE '%.jpg' OR ResourceURI LIKE '%.gif' ) " +
                    "GROUP BY PlotID";
                System.Data.DataTable dtImage = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtImage, SC.ConnectionString);
                //Images = Chart.getImages(dtImage);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Images;
        }


        //private static System.Collections.Generic.Dictionary<int, string> ChartSections(int ProjectID, DiversityWorkbench.ServerConnection SC)
        //{
        //    System.Collections.Generic.Dictionary<int, string> Sections = new Dictionary<int, string>();
        //    try
        //    {
        //        string SQL = "SELECT S.ProjectID, 'List ' + S.Project " +
        //            "FROM TaxonNameListProjectProxy AS S " +
        //            "ORDER BY S.Project";
        //        System.Data.DataTable dtSection = new System.Data.DataTable();
        //        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSection, SC.ConnectionString);
        //        foreach (System.Data.DataRow R in dtSection.Rows)
        //        {
        //            int ID;
        //            if (!int.TryParse(R[0].ToString(), out ID))
        //                continue;
        //            if (Sections.ContainsKey(ID))
        //                continue;
        //            Sections.Add(ID, R[1].ToString());
        //        }
        //        if (Sections.Count > 0)
        //        {
        //            string Terminology = "";
        //            SQL = "SELECT ' ' + Project FROM ProjectProxy WHERE ProjectID = " + ProjectID.ToString();
        //            Terminology = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
        //            Sections.Add(-1, Terminology);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return Sections;
        //}


        private static System.Data.DataTable ChartData(int ProjectID, DiversityWorkbench.ServerConnection SC)
        {
            string Project = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT Project FROM ProjectProxy " +
                " WHERE ProjectID = " + ProjectID.ToString()).Replace(" ", "_");
            System.Data.DataTable dtPlot = new System.Data.DataTable(Project);
            try
            {
                string SQL = "SELECT S.PlotID, S.PartOfPlotID, S.PlotIdentifier " +
                    "FROM SamplingPlot AS S INNER JOIN SamplingProject AS P ON S.PlotID = P.PlotID " +
                    "WHERE(P.ProjectID = " + ProjectID.ToString() + " " +
                    "ORDER BY N.PlotIdentifier";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtPlot, SC.ConnectionString);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return dtPlot;
        }

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


        #endregion


    }
}
