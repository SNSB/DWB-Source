using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using DWBServices.WebServices;

namespace DiversityWorkbench
{

    [Serializable]
    public class GazetteerValues : RemoteValues
    {
        public string URI;
        public string PlaceName;
        public string PlaceType;
        public string Country;
        public double? Longitude;
        public double? Latitude;
        public double? Altitude;

        public GazetteerValues() { }
    }

    public class Gazetteer : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        // Umbau auf neue Version:
        /*
        SELECT dbo.BaseURL() + CAST(GeoPlaceName.NameID AS VARCHAR) AS _URI, GeoPlaceName.Name AS _DisplayText, 
        GeoPlaceName.NameID, GeoPlaceName.PlaceName, GeoPlaceName.Preferred_PlaceName, GeoPlaceName.HierarchyList,  
        CASE WHEN MAX(GeoPlaceName.Continent) <> MIN(GeoPlaceName.Continent) THEN '' ELSE MAX(GeoPlaceName.Continent) END AS Continent, 
        GeoPlaceType.PlaceType 
        FROM  GeoPlaceName INNER JOIN 
        GeoPlaceType ON GeoPlaceName.PlaceTypeID = GeoPlaceType.PlaceTypeID 
        WHERE GeoPlaceName.NameID = 654 GROUP BY GeoPlaceName.NameID, GeoPlaceName.Name, 
        GeoPlaceName.NameID, GeoPlaceName.PlaceName, GeoPlaceName.Preferred_PlaceName, 
        GeoPlaceName.HierarchyList, GeoPlaceType.PlaceType
        */


        //SELECT dbo.BaseURL() + CAST(N.NameID AS VARCHAR) AS _URI, N.Name AS _DisplayText, 
        //N.Name, E.DisplayText AS PlaceType 
        //FROM  GeoName N, GeoPlace P, PlaceType_Enum E
        //where N.PlaceID = P.PlaceID and P.PlaceType = E.Code


        /*
                        SELECT CASE WHEN GeoTopHierarchy_1.NameEn IS NULL 
        THEN dbo.GeoTopHierarchy.NameEn ELSE GeoTopHierarchy_1.NameEn END AS Country 
        FROM dbo.GeoPlaceName INNER JOIN 
        dbo.GeoTopHierarchy ON dbo.GeoPlaceName.HierarchyTopID = dbo.GeoTopHierarchy.TopID LEFT OUTER JOIN 
        dbo.GeoTopHierarchy AS GeoTopHierarchy_1 ON dbo.GeoTopHierarchy.CountryTopID = GeoTopHierarchy_1.TopID 
        where dbo.GeoPlaceName.NameID =  687 GROUP BY GeoTopHierarchy_1.NameEn, GeoTopHierarchy.NameEn
        */


        /*
                        SELECT CASE WHEN GeoPlaceAnalysis.NumLat = 0 AND GeoPlaceAnalysis.NumLong = 0 THEN NULL ELSE GeoPlaceAnalysis.NumLat END AS Latitude, 
        CASE WHEN GeoPlaceAnalysis.NumLat = 0 AND GeoPlaceAnalysis.NumLong = 0 THEN NULL ELSE GeoPlaceAnalysis.NumLong END AS Longitude  
        FROM  GeoNameAnalysis INNER JOIN  
        GeoPlaceAnalysis ON GeoNameAnalysis.PlaceID = GeoPlaceAnalysis.PlaceID  
        WHERE GeoNameAnalysis.NameID =  654
        */

        /*

                        SELECT CASE WHEN GeoPlaceAnalysis.NumLat = 0 AND GeoPlaceAnalysis.NumLong = 0 THEN NULL 
        ELSE 'POINT(' + cast(GeoPlaceAnalysis.NumLong as varchar) + ' ' + cast(GeoPlaceAnalysis.NumLat as varchar) + ')' end AS Geography 
        FROM  GeoNameAnalysis INNER JOIN  
        GeoPlaceAnalysis ON GeoNameAnalysis.PlaceID = GeoPlaceAnalysis.PlaceID  
        WHERE GeoNameAnalysis.NameID =  87189
        */


        /*
        SELECT PlaceTypeID AS [Value], PlaceType  AS Display 
        FROM GeoPlaceType 
        ORDER BY Display
        */

        /*
        SELECT Abbreviation AS [Value], Abbreviation AS Display 
        FROM GeoString_Enums 
        WHERE (Enumeration = N'GazGettyContinents') 
        ORDER BY Display;
        */


        /*
        SELECT NameEn AS ValueColumn, 
        CASE WHEN NameDe IS NULL THEN NameEn ELSE NameDe END AS DisplayColumn 
        FROM DiversityGazetteer.dbo.GeoCountries ORDER BY DisplayColumn;
        */


        #region Parameter

        private string _Latitude;
        private string _Longitude;
        private System.Collections.Generic.Dictionary<string, string> _ServiceList;

        #endregion

        #region Construction

        public Gazetteer(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        #endregion

        #region Interface

        private bool? _IsNewVersionOfDatabase;
        public bool IsNewVersionOfDatabase
        {
            get
            {
                // no support for old version any more
                this._IsNewVersionOfDatabase = true;


                if (this._IsNewVersionOfDatabase == null)
                {
                    this._IsNewVersionOfDatabase = true;
                    try
                    {
                        string SQL = "SELECT CASE WHEN COUNT(*) = 3 THEN 1 ELSE 0 END AS NewVersion " +
                            "FROM INFORMATION_SCHEMA.TABLES AS T " +
                            "WHERE T.TABLE_NAME IN ('GeoName', 'GeoPlace', 'GeoProject')";
                        if (this.ServerConnection.LinkedServer.Length > 0)
                        {
                            SQL = "SELECT CASE WHEN COUNT(*) = 3 THEN 1 ELSE 0 END AS NewVersion " +
                            "FROM ";
                            if (this.ServerConnection.DatabaseName.StartsWith("[" + this.ServerConnection.LinkedServer + "]."))
                                SQL += this.ServerConnection.DatabaseName;
                            else
                                SQL += "[" + this.ServerConnection.LinkedServer + "]." + this.ServerConnection.DatabaseName;
                            SQL += ".INFORMATION_SCHEMA.TABLES AS T " +
                                "WHERE T.TABLE_NAME IN ('GeoName', 'GeoPlace', 'GeoProject')";
                        }
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ServerConnection.ConnectionString);
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList())
                        {
                            if (KV.Value.ConnectionString != "")
                            {
                                con.ConnectionString = KV.Value.ConnectionString;
                                break;
                            }
                        }
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        string Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        if (Result == "1")
                            this._IsNewVersionOfDatabase = true;
                        con.Close();
                        con.Dispose();
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                return (bool)this._IsNewVersionOfDatabase;
            }
        }

        public override string ServiceName() { return "DiversityGazetteer"; }

        public override System.Collections.Generic.Dictionary<string, string> AdditionalServicesOfModule()
        {
            if (this._ServiceList == null)
            {
                this._ServiceList = new Dictionary<string, string>();
                // Iterate over all ServiceTypes and add them to _ServiceList
                var serviceTypeNameDictionary = DwbServiceEnums.GeoServiceInfoDictionary();
                foreach (var entry in serviceTypeNameDictionary)
                {
                    if (entry.Key != DwbServiceEnums.DwbService.None)
                    {
                        _ServiceList.Add(entry.Key.ToString(), entry.Value.Name);
                    }
                }

                _ServiceList.Add("CacheDB", "Cache database");
                //this._ServiceList = new Dictionary<string, string>();
                //_ServiceList.Add("Geonames", "GeoNames");
                //_ServiceList.Add("IsoCountries", "ISO Countries");
                //_ServiceList.Add("WorldSeas", "IHO World Seas");
                //_ServiceList.Add("CacheDB", "Cache database");
            }
            return _ServiceList;
        }
        protected override System.Collections.Generic.Dictionary<string, string> AdditionalServiceURIsOfModule()
        {
            System.Collections.Generic.Dictionary<string, string> _Add = new Dictionary<string, string>();

            var serviceTypeNameDictionary = DwbServiceEnums.GeoServiceInfoDictionary();
            foreach (var entry in serviceTypeNameDictionary)
            {
                if (entry.Key != DwbServiceEnums.DwbService.None)
                {
                    _Add.Add(entry.Key.ToString(), entry.Value.Url);
                }
            }
            // Ariane
            //_Add.Add("Geonames", DiversityWorkbench.WebserviceGfbioGeonames.UriBaseWeb);

            //_Add.Add("IsoCountries", DiversityWorkbench.WebserviceGfbioIsoCountries.UriBaseWeb);

            //_Add.Add("WorldSeas", DiversityWorkbench.WebserviceGfbioWorldSeas.UriBaseWeb);


            return _Add;
        }

        public override System.Collections.Generic.List<DiversityWorkbench.DatabaseService> DatabaseServices()
        {
            System.Collections.Generic.List<DiversityWorkbench.DatabaseService> ds = new List<DatabaseService>();
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

            // Create and add DatabaseServices for each ServiceType in the enum
            foreach (DwbServiceEnums.DwbService serviceType in System.Enum.GetValues(
                         typeof(DwbServiceEnums.DwbService)))
            {
                if (DwbServiceEnums.GeoServiceInfoDictionary().TryGetValue(serviceType, out DwbServiceEnums.DwbServiceInfo serviceInfo))
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
            // Ariane exclude Webserice.. for .NET 8
            //DiversityWorkbench.DatabaseService GN = new DatabaseService("Geonames");
            //GN.IsWebservice = true;
            //GN.WebService = Webservice.WebServices.gfbioGeonames;
            //ds.Add(GN);

            //DiversityWorkbench.DatabaseService IC = new DatabaseService("IsoCountries");
            //IC.IsWebservice = true;
            //IC.DisplayUnitdataInTreeview = true;
            //IC.WebService = Webservice.WebServices.gfbioIsoCountries;
            //ds.Add(IC);

            //DiversityWorkbench.DatabaseService WS = new DatabaseService("WorldSeas");
            //WS.IsWebservice = true;
            //WS.DisplayUnitdataInTreeview = true;
            //WS.WebService = Webservice.WebServices.gfbioWorldSeas;
            //ds.Add(WS);

            return ds;
        }


        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            string Prefix = "";
            // Markus 15.5.23: Bugfix reading serverconnection
            if (this._ServerConnection.DatabaseName.IndexOf(".") > -1 && this._ServerConnection.LinkedServer.Length == 0 && this._ServerConnection.DatabaseName.IndexOf("[") == 0)
            {
                string DB = this._ServerConnection.DatabaseName.Substring(this._ServerConnection.DatabaseName.LastIndexOf(".") + 1);
                string LS = this._ServerConnection.DatabaseName.Substring(0, this._ServerConnection.DatabaseName.LastIndexOf(".")).Replace("[", "").Replace("]", "");
                this._ServerConnection.DatabaseName = DB;
                this._ServerConnection.LinkedServer = LS;
            }

            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this.getServerConnection().ConnectionString.Length > 0)
            {
                string Test = this.getServerConnection().ConnectionString;
                string SQL = "";
                SQL = "select U.BaseURL + CAST(N.NameID AS VARCHAR) AS _URI, U.BaseURL + CAST(N.NameID AS VARCHAR) AS Link, ";
                if (DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry)
                    SQL += "N.Name + CASE WHEN C.HierarchyPlaceToCountry <> '' THEN '" + DiversityWorkbench.Settings.GazetteerHierarchySeparator + "' + REPLACE(C.HierarchyPlaceToCountry, '|', '" + DiversityWorkbench.Settings.GazetteerHierarchySeparator + "') ELSE '' END ";
                else SQL += "CASE WHEN C.HierarchyCountryToPlace <> '' THEN REPLACE(C.HierarchyCountryToPlace, '|', '" + DiversityWorkbench.Settings.GazetteerHierarchySeparator + "') + '" + DiversityWorkbench.Settings.GazetteerHierarchySeparator + "' ELSE '' END + N.Name";
                SQL += " AS _DisplayText, N.NameID, N.Name AS PlaceName, N.ExternalNameID, " +
                    "PN.Name AS Preferred_PlaceName, REPLACE(";
                if (DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry)
                    SQL += "C.HierarchyPlaceToCountry";
                else SQL += "C.HierarchyCountryToPlace";
                SQL += ", '|', '" + DiversityWorkbench.Settings.GazetteerHierarchySeparator + "') AS HierarchyList, case when C.Country is null and P.PlaceType = 'nation' then pn.Name else C.Country end as Country, P.PlaceType " +
                    "FROM " + Prefix + "GeoName N, " +
                    Prefix + "ViewBaseURL U, " +
                    Prefix + "ViewGeoPlace P," +
                    Prefix + "GeoName PN, " +
                    Prefix + "GeoCache C " +
                    "WHERE C.PlaceID = N.PlaceID AND N.PlaceID = P.PlaceID and PN.NameID = P.PreferredNameID and N.NameID = " + ID.ToString();

                //if (this._ServerConnection.LinkedServer.Length > 0)
                //{
                //    SQL = "select U.BaseURL + CAST(N.NameID AS VARCHAR) AS _URI, N.Name AS _DisplayText, N.NameID, N.Name AS PlaceName, " +
                //        "PN.Name AS Preferred_PlaceName, N.HierarchyCache AS HierarchyList/*, '' AS Country, '' AS Continent*/, P.PlaceType " +
                //        "from ";
                //    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, [" 
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewBaseURL U, [" 
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P,[" 
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName PN ";
                //    SQL += " WHERE N.PlaceID = P.PlaceID and PN.NameID = P.PreferredNameID and N.NameID = " + ID.ToString();
                //}
                //else
                //{
                //    SQL = "select dbo.BaseURL() + CAST(N.NameID AS VARCHAR) AS _URI, N.DisplayText AS _DisplayText, " +
                //        "N.NameID, N.Name AS PlaceName, N.PreferredName AS Preferred_PlaceName,  " +
                //        "N.Hierarchy AS HierarchyList, N.Country, N.Continent, N.PlaceType " +
                //        "from dbo.NameSuperior(" + ID.ToString() + ", dbo.HierarchySequenceTopDown(), ', ') N";
                //}
                this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT C.Name " +
                //"FROM " + Prefix + "dbo.GeoName N, "
                //    + Prefix + "dbo.ViewCountry C, "
                //    + Prefix + "dbo.GeoName CN, "
                //    + Prefix + "dbo.ViewGeoPlace P ";
                //SQL += " WHERE N.PlaceID = P.PlaceID AND C.PlaceID = P.CountryPlaceID AND C.PreferredNameID = " +
                //    " AND N.NameID = " + ID.ToString();
                //this.getDataFromTable(SQL, ref Values);


                //if (this._ServerConnection.LinkedServer.Length > 0)
                //{
                //    SQL = "SELECT C.Name " +
                //        "FROM ";
                //    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, ["
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewCountry C, ["
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName CN, ["
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
                //    SQL += " WHERE N.PlaceID = P.PlaceID AND C.PlaceID = P.CountryPlaceID AND C.PreferredNameID = " +
                //        " AND N.NameID = " + ID.ToString();
                //    this.getDataFromTable(SQL, ref Values);
                //}


                //SQL = "select dbo.HierarchyDown(N.HierachyCache, ', ') AS HierarchyDown, " +
                //    "dbo.HierarchyUp(N.HierachyCache, ', ') AS HierarchyUp " +
                //    "from dbo.GeoName N WHERE N.NameID = " + ID.ToString();
                //this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT P.Latitude, P.Longitude " +
                    "FROM " + Prefix + "GeoName N, "
                    + Prefix + "ViewGeoPlace P ";
                SQL += " WHERE N.PlaceID = P.PlaceID " +
                    "AND N.NameID = " + ID.ToString();

                //if (this._ServerConnection.LinkedServer.Length > 0)
                //{
                //    SQL = "SELECT P. Latitude, P.Longitude " +
                //        "FROM ";
                //    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, [" 
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
                //    SQL += " WHERE N.PlaceID = P.PlaceID " +
                //        "AND N.NameID = " + ID.ToString();
                //}
                //else
                //{
                //    SQL = "SELECT P.[Geography].EnvelopeCenter().Lat AS Latitude, P.[Geography].EnvelopeCenter().Long AS Longitude " +
                //        "FROM GeoName N, GeoPlace P " +
                //        "WHERE N.PlaceID = P.PlaceID " +
                //        "AND N.NameID = " + ID.ToString();
                //}

                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT P.[Geography] FROM " + Prefix + "GeoName N, "
                    + Prefix + "ViewGeoPlace P WHERE N.PlaceID = P.PlaceID " +
                    "AND N.NameID = " + ID.ToString();
                //if (this._ServerConnection.LinkedServer.Length > 0)
                //{
                //    SQL = "SELECT P.[Geography] FROM ";
                //    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, [" 
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
                //    SQL += " WHERE N.PlaceID = P.PlaceID " +
                //        "AND N.NameID = " + ID.ToString();
                //}
                //else
                //{
                //    SQL = "SELECT P.[Geography].ToString() AS Geography " +
                //        "FROM GeoName N, GeoPlace P " +
                //        "WHERE N.PlaceID = P.PlaceID " +
                //        "AND N.NameID = " + ID.ToString();
                //}
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT P.Altitude FROM " + Prefix + "GeoName N, "
                    + Prefix + "ViewGeoPlace P WHERE N.PlaceID = P.PlaceID " +
                    "AND N.NameID = " + ID.ToString();

                //if (this._ServerConnection.LinkedServer.Length > 0)
                //{
                //    SQL = "SELECT P.Altitude FROM ";
                //    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, [" 
                //        + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
                //    SQL += " WHERE N.PlaceID = P.PlaceID " +
                //        "AND N.NameID = " + ID.ToString();
                //}
                //else
                //{
                //    SQL = "SELECT P.[Geography].EnvelopeCenter().Z AS Altitude " +
                //        "FROM GeoName N, GeoPlace P " +
                //        "WHERE N.PlaceID = P.PlaceID " +
                //        "AND N.NameID = " + ID.ToString();
                //}
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT Name_0, PlaceType_0, LanguageCode_0, Name_1, PlaceType_1, LanguageCode_1, Name_2, PlaceType_2, LanguageCode_2, " +
                    "Name_3, PlaceType_3, LanguageCode_3, Name_4, PlaceType_4, LanguageCode_4, Name_5, PlaceType_5, LanguageCode_5, Name_6, " +
                    "PlaceType_6, LanguageCode_6, Name_7, PlaceType_7, LanguageCode_7 " +
                    "FROM " + Prefix + "ViewHierarchy " +
                    "WHERE NameID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
                this._UnitValues.Clear();
                bool HasCoordinates = false;
                foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                {
                    this._UnitValues.Add(P.Key, P.Value);
                    if (P.Key == "Latitude" && P.Value.Length > 0)
                    {
                        this._Latitude = P.Value;
                        HasCoordinates = true;
                    }
                    if (P.Key == "Longitude" && P.Value.Length > 0)
                    {
                        this._Longitude = P.Value;
                        HasCoordinates = true;
                    }
                }
                if (!HasCoordinates)
                {
                    this._Latitude = "";
                    this._Longitude = "";
                }
            }
            return Values;
        }

        public override string HtmlUnitValues(Dictionary<string, string> UnitValues)
        {
            // String builder for result
            StringBuilder sb = new StringBuilder();

            try
            {
                // Start XML writer
                using (System.Xml.XmlWriter W = System.Xml.XmlWriter.Create(sb))
                {
                    // Save title
                    string title = this.ServiceName();
                    if (UnitValues["PlaceName"].Length > 0)
                        title = UnitValues["PlaceName"];
                    else if (UnitValues["_DisplayText"].Length > 0)
                        title = UnitValues["_DisplayText"];

                    // Start HTML document
                    W.WriteStartElement("html");
                    W.WriteString("\r\n");
                    W.WriteStartElement("head");
                    W.WriteElementString("title", title);
                    W.WriteEndElement();//head
                    W.WriteString("\r\n");
                    W.WriteStartElement("body");
                    W.WriteString("\r\n");

                    // Write reference title
                    W.WriteStartElement("h2");
                    W.WriteStartElement("font");
                    W.WriteAttributeString("face", "Verdana");
                    DiversityWorkbench.Description.WriteXmlString(W, title);
                    W.WriteEndElement();//font
                    W.WriteEndElement();//h2

                    // Start table
                    W.WriteStartElement("table");
                    //W.WriteAttributeString("width", "900");
                    W.WriteAttributeString("border", "0");
                    W.WriteAttributeString("cellpadding", "1");
                    W.WriteAttributeString("cellspacing", "0");
                    W.WriteAttributeString("class", "small");
                    W.WriteAttributeString("style", "margin-left:0px");

                    foreach (KeyValuePair<string, string> item in UnitValues)
                    {
                        // Skip irrelevant entries
                        if (item.Key.StartsWith("_"))
                            continue;

                        // Insert unit value
                        if (item.Value.Trim() != "")
                        {
                            W.WriteStartElement("tr");
                            //W.WriteAttributeString("style", "padding-top:10px; padding-bottom:20px");

                            // Write first column <td width=_ColumnWidth align="right">
                            W.WriteStartElement("td");
                            W.WriteAttributeString("width", "150");
                            W.WriteAttributeString("align", "right");
                            W.WriteAttributeString("valign", "top");
                            W.WriteAttributeString("style", "padding-right:5px");
                            W.WriteStartElement("font");
                            W.WriteAttributeString("face", "Verdana");
                            W.WriteAttributeString("size", "2");
                            DiversityWorkbench.Description.WriteXmlString(W, string.Format("<b>{0}</b>", item.Key));
                            W.WriteEndElement();//font
                            W.WriteEndElement();//td

                            // Write second column <td style="padding-left:5px">
                            W.WriteStartElement("td");
                            W.WriteAttributeString("style", "padding-left:5px");
                            W.WriteStartElement("font");
                            W.WriteAttributeString("face", "Verdana");
                            W.WriteAttributeString("size", "2");
                            DiversityWorkbench.Description.WriteXmlString(W, item.Value);
                            W.WriteEndElement();//font
                            W.WriteEndElement();//td
                            W.WriteEndElement();//tr
                        }
                    }

                    W.WriteEndElement();//body
                    W.WriteEndElement();//html
                    W.WriteEndDocument();
                    W.Flush();
                    W.Close();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return sb.ToString();
        }

        //public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        //{
        //    System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
        //    this._IsNewVersionOfDatabase = null;
        //    if (this.getServerConnection().ConnectionString.Length > 0)
        //    {
        //        string SQL = "";
        //        if (this.IsNewVersionOfDatabase)
        //        {
        //            if (this._ServerConnection.LinkedServer.Length > 0)
        //            {
        //                SQL = "select U.BaseURL + CAST(N.NameID AS VARCHAR) AS _URI, N.Name AS _DisplayText, N.NameID, N.Name AS PlaceName, " +
        //                    "PN.Name AS Preferred_PlaceName,  N.HierarchyCache AS HierarchyList, '' AS Country, '' AS Continent, P.PlaceType " +
        //                    "from ";
        //                if (this._ServerConnection.DatabaseName.StartsWith("[" + this._ServerConnection.LinkedServer + "]."))
        //                    SQL += this._ServerConnection.DatabaseName + ".dbo.GeoName N, " + this._ServerConnection.DatabaseName + ".dbo.ViewBaseURL U, " + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P, " + this._ServerConnection.DatabaseName + ".dbo.GeoName PN";
        //                else
        //                    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, [" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewBaseURL U, [" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P,[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName PN ";
        //                SQL += " WHERE N.PlaceID = P.PlaceID and PN.NameID = P.PreferredNameID and N.NameID = " + ID.ToString();
        //            }
        //            else
        //            {
        //                SQL = "select dbo.BaseURL() + CAST(N.NameID AS VARCHAR) AS _URI, N.DisplayText AS _DisplayText, " +
        //                    "N.NameID, N.Name AS PlaceName, N.PreferredName AS Preferred_PlaceName,  " +
        //                    "N.Hierarchy AS HierarchyList, N.Country, N.Continent, N.PlaceType " +
        //                    "from dbo.NameSuperior(" + ID.ToString() + ", dbo.HierarchySequenceTopDown(), ', ') N";
        //            }
        //        }
        //        else
        //            SQL = "SELECT dbo.BaseURL() + CAST(GeoPlaceName.NameID AS VARCHAR) AS _URI, GeoPlaceName.Name AS _DisplayText, " +
        //                "GeoPlaceName.NameID, GeoPlaceName.PlaceName, GeoPlaceName.Preferred_PlaceName, GeoPlaceName.HierarchyList,  " +
        //                "CASE WHEN MAX(GeoPlaceName.Continent) <> MIN(GeoPlaceName.Continent) THEN '' ELSE MAX(GeoPlaceName.Continent) END AS Continent, " +
        //                "GeoPlaceType.PlaceType " +
        //                "FROM  GeoPlaceName INNER JOIN " +
        //                "GeoPlaceType ON GeoPlaceName.PlaceTypeID = GeoPlaceType.PlaceTypeID " +
        //                "WHERE GeoPlaceName.NameID = " + ID.ToString() + " GROUP BY GeoPlaceName.NameID, GeoPlaceName.Name, " +
        //                "GeoPlaceName.NameID, GeoPlaceName.PlaceName, GeoPlaceName.Preferred_PlaceName, " +
        //                "GeoPlaceName.HierarchyList, GeoPlaceType.PlaceType";
        //        this.getDataFromTable(SQL, ref Values);

        //        if (this.IsNewVersionOfDatabase && this._ServerConnection.LinkedServer.Length == 0)
        //        {
        //            SQL = "select dbo.HierarchyDown(N.HierachyCache, ', ') AS HierarchyDown, " +
        //                "dbo.HierarchyUp(N.HierachyCache, ', ') AS HierarchyUp " +
        //                "from dbo.GeoName N WHERE N.NameID = " + ID.ToString();
        //            this.getDataFromTable(SQL, ref Values);
        //        }

        //        if (!IsNewVersionOfDatabase)
        //        {
        //            SQL = "SELECT CASE WHEN GeoTopHierarchy_1.NameEn IS NULL " +
        //                "THEN dbo.GeoTopHierarchy.NameEn ELSE GeoTopHierarchy_1.NameEn END AS Country " +
        //                "FROM dbo.GeoPlaceName INNER JOIN " +
        //                "dbo.GeoTopHierarchy ON dbo.GeoPlaceName.HierarchyTopID = dbo.GeoTopHierarchy.TopID LEFT OUTER JOIN " +
        //                "dbo.GeoTopHierarchy AS GeoTopHierarchy_1 ON dbo.GeoTopHierarchy.CountryTopID = GeoTopHierarchy_1.TopID " +
        //                "where dbo.GeoPlaceName.NameID = " + ID.ToString() + " GROUP BY GeoTopHierarchy_1.NameEn, GeoTopHierarchy.NameEn";
        //            this.getDataFromTable(SQL, ref Values);
        //        }

        //        if (IsNewVersionOfDatabase)
        //        {
        //            if (this._ServerConnection.LinkedServer.Length > 0)
        //            {
        //                SQL = "SELECT P. Latitude, P.Longitude " +
        //                    "FROM ";
        //                if (this._ServerConnection.DatabaseName.StartsWith("[" + this._ServerConnection.LinkedServer + "]."))
        //                    SQL += this._ServerConnection.DatabaseName + ".dbo.GeoName N, " + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
        //                else
        //                    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, [" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
        //                SQL += " WHERE N.PlaceID = P.PlaceID " +
        //                    "AND N.NameID = " + ID.ToString();
        //            }
        //            else
        //            {
        //                SQL = "SELECT P.[Geography].EnvelopeCenter().Lat AS Latitude, P.[Geography].EnvelopeCenter().Long AS Longitude " +
        //                    "FROM GeoName N, GeoPlace P " +
        //                    "WHERE N.PlaceID = P.PlaceID " +
        //                    "AND N.NameID = " + ID.ToString();
        //            }
        //        }
        //        else
        //            SQL = "SELECT CASE WHEN GeoPlaceAnalysis.NumLat = 0 AND GeoPlaceAnalysis.NumLong = 0 THEN NULL ELSE GeoPlaceAnalysis.NumLat END AS Latitude, " +
        //                "CASE WHEN GeoPlaceAnalysis.NumLat = 0 AND GeoPlaceAnalysis.NumLong = 0 THEN NULL ELSE GeoPlaceAnalysis.NumLong END AS Longitude  " +
        //                "FROM  GeoNameAnalysis INNER JOIN  " +
        //                "GeoPlaceAnalysis ON GeoNameAnalysis.PlaceID = GeoPlaceAnalysis.PlaceID  " +
        //                "WHERE GeoNameAnalysis.NameID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        if (IsNewVersionOfDatabase)
        //        {
        //            if (this._ServerConnection.LinkedServer.Length > 0)
        //            {
        //                SQL = "SELECT P.[Geography] FROM ";
        //                if (this._ServerConnection.DatabaseName.StartsWith("[" + this._ServerConnection.LinkedServer + "]."))
        //                    SQL += this._ServerConnection.DatabaseName + ".dbo.GeoName N, " + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
        //                else
        //                    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, [" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
        //                SQL += " WHERE N.PlaceID = P.PlaceID " +
        //                    "AND N.NameID = " + ID.ToString();
        //            }
        //            else
        //            {
        //                SQL = "SELECT P.[Geography].ToString() AS Geography " +
        //                    "FROM GeoName N, GeoPlace P " +
        //                    "WHERE N.PlaceID = P.PlaceID " +
        //                    "AND N.NameID = " + ID.ToString();
        //            }
        //        }
        //        else
        //            SQL = "SELECT CASE WHEN GeoPlaceAnalysis.NumLat = 0 AND GeoPlaceAnalysis.NumLong = 0 THEN NULL " +
        //                "ELSE 'POINT(' + cast(GeoPlaceAnalysis.NumLong as varchar) + ' ' + cast(GeoPlaceAnalysis.NumLat as varchar) + ')' end AS Geography " +
        //                "FROM  GeoNameAnalysis INNER JOIN  " +
        //                "GeoPlaceAnalysis ON GeoNameAnalysis.PlaceID = GeoPlaceAnalysis.PlaceID  " +
        //                "WHERE GeoNameAnalysis.NameID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        if (IsNewVersionOfDatabase)
        //        {
        //            if (this._ServerConnection.LinkedServer.Length > 0)
        //            {
        //                SQL = "SELECT P.Altitude FROM ";
        //                if (this._ServerConnection.DatabaseName.StartsWith("[" + this._ServerConnection.LinkedServer + "]."))
        //                    SQL += this._ServerConnection.DatabaseName + ".dbo.GeoName N, " + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
        //                else
        //                    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.GeoName N, [" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.ViewGeoPlace P ";
        //                SQL += " WHERE N.PlaceID = P.PlaceID " +
        //                    "AND N.NameID = " + ID.ToString();
        //            }
        //            else
        //            {
        //                SQL = "SELECT P.[Geography].EnvelopeCenter().Z AS Altitude " +
        //                    "FROM GeoName N, GeoPlace P " +
        //                    "WHERE N.PlaceID = P.PlaceID " +
        //                    "AND N.NameID = " + ID.ToString();
        //            }
        //            this.getDataFromTable(SQL, ref Values);
        //        }

        //        //SQL = " (geography::STGeomFromText('LINESTRING(47.656 -122.360, 47.656 -122.343)', 4326));";

        //        if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
        //        this._UnitValues.Clear();
        //        bool HasCoordinates = false;
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
        //        {
        //            this._UnitValues.Add(P.Key, P.Value);
        //            if (P.Key == "Latitude" && P.Value.Length > 0) 
        //            { 
        //                this._Latitude = P.Value;
        //                HasCoordinates = true;
        //            }
        //            if (P.Key == "Longitude" && P.Value.Length > 0) 
        //            { 
        //                this._Longitude = P.Value;
        //                HasCoordinates = true;
        //            }
        //        }
        //        if (!HasCoordinates)
        //        {
        //            this._Latitude = "";
        //            this._Longitude = "";
        //        }
        //    }
        //    return Values;
        //}

        public string MainTable()
        {
            if (this.IsNewVersionOfDatabase) return "GeoName";
            else return "GeoPlaceName";
        }

        public override string SourceURI(DiversityWorkbench.UserControls.UserControlWebView WebBrowser)
        {
            if (this._Longitude.Length == 0 || this._Latitude.Length == 0)
                return "";
            // Markus 22.8.2016: New website
            string URL_base = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
            if (string.IsNullOrEmpty(URL_base))
                return "";
            string URI = URL_base;
            if (this._Latitude.Length > 0 || this._Longitude.Length > 0)
                URI += "?Lat=" + this._Latitude.ToString().Replace(",", ".") + "&Lng=" + this._Longitude.ToString().Replace(",", ".");
            else
                URI += "";
            // Markus 22.8.2016: New website
            URI += "&Cross=1";
            //URI += "&SizeX=" + WebBrowser.Width.ToString() + "&SizeY=" + WebBrowser.Height.ToString();
            return URI;
        }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[2];
            if (this.IsNewVersionOfDatabase)
            {
                QueryDisplayColumns[0].DisplayText = "Name";
                QueryDisplayColumns[0].DisplayColumn = "Name";
                QueryDisplayColumns[0].OrderColumn = "Name";
                QueryDisplayColumns[0].IdentityColumn = "NameID";
                QueryDisplayColumns[0].TableName = "GeoName";

                QueryDisplayColumns[1].DisplayText = "Hierarchy";
                QueryDisplayColumns[1].DisplayColumn = "HierarchyCache";
                QueryDisplayColumns[1].OrderColumn = "HierarchyCache";
                QueryDisplayColumns[1].IdentityColumn = "NameID";
                QueryDisplayColumns[1].TableName = "GeoName";
            }
            else
            {
                QueryDisplayColumns[0].DisplayText = "PlaceName";
                QueryDisplayColumns[0].DisplayColumn = "PlaceName";
                QueryDisplayColumns[0].OrderColumn = "PlaceName";
                QueryDisplayColumns[0].IdentityColumn = "NameID";
                QueryDisplayColumns[0].TableName = "GeoPlaceName";

                QueryDisplayColumns[1].DisplayText = "PlaceName";
                QueryDisplayColumns[1].DisplayColumn = "PlaceName";
                QueryDisplayColumns[1].OrderColumn = "PlaceName";
                QueryDisplayColumns[1].IdentityColumn = "NameID";
                QueryDisplayColumns[1].TableName = "GeoPlaceName";
            }
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string Description = "";
            string SQL = "";
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            if (this.IsNewVersionOfDatabase)
            {
                // Name
                Description = DiversityWorkbench.Functions.ColumnDescription("GeoName", "Name");
                DiversityWorkbench.QueryCondition qName = new DiversityWorkbench.QueryCondition(true, "GeoName", "NameID", "Name", "Place", "Name", "Name of geographical place", Description);
                QueryConditions.Add(qName);

                // Language
                Description = DiversityWorkbench.Functions.ColumnDescription("GeoName", "LanguageCode");
                DiversityWorkbench.QueryCondition qLanguageCode = new DiversityWorkbench.QueryCondition(false, "GeoName", "NameID", "LanguageCode", "Place", "Language", "Language", Description, "LanguageCode_Enum");
                QueryConditions.Add(qLanguageCode);

                // External Database
                System.Data.DataTable dtExternalDatabase = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                    "SELECT ExternalDatabaseID AS [Value], ExternalDatabaseName  AS Display FROM " + Prefix + "ExternalDatabase ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aExternalDatabase = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aExternalDatabase.Fill(dtExternalDatabase); }
                    catch (System.Exception ex) { }
                }
                if (dtExternalDatabase.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtExternalDatabase.Columns.Add(Value);
                    dtExternalDatabase.Columns.Add(Display);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("GeoName", "ExternalDatabaseID");
                DiversityWorkbench.QueryCondition qExternalDatabase = new DiversityWorkbench.QueryCondition(true, "GeoName", "NameID", "ExternalDatabaseID", "Place", "External DB", "External database", Description, dtExternalDatabase, false);
                //SQL = " FROM " + Prefix + "GeoName N INNER JOIN " + Prefix + "ViewGeoPlace T ON N.PlaceID = T.PlaceID ";
                //qExternalDatabase.SqlFromClause = SQL;
                QueryConditions.Add(qExternalDatabase);

                // Place type
                System.Data.DataTable dtPlaceType = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                    "SELECT Code AS [Value], DisplayText  AS Display " +
                    "FROM " + Prefix + "PlaceType_Enum ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aPlaceType = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aPlaceType.Fill(dtPlaceType); }
                    catch { }
                }
                if (dtPlaceType.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtPlaceType.Columns.Add(Value);
                    dtPlaceType.Columns.Add(Display);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("GeoPlace", "PlaceType");
                string SourceGeoPlace = "GeoPlace";
                if (this._ServerConnection.LinkedServer.Length > 0)
                    SourceGeoPlace = "ViewGeoPlace";
                DiversityWorkbench.QueryCondition qPlaceType = new DiversityWorkbench.QueryCondition(true, SourceGeoPlace, "NameID", "PlaceType", "Place", "PlaceType", "Place type", Description, dtPlaceType, false);
                SQL = " FROM ";
                SQL += Prefix + "GeoName N INNER JOIN " + Prefix + SourceGeoPlace;
                SQL += " T ON N.PlaceID = T.PlaceID ";
                qPlaceType.SqlFromClause = SQL;
                qPlaceType.ForeignKey = "PlaceID";
                qPlaceType.ForeignKeyTable = "GeoPlace";
                QueryConditions.Add(qPlaceType);

                // Project
                System.Data.DataTable dtProject = new System.Data.DataTable();
                SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                    "FROM " + Prefix + "ProjectProxy " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { a.Fill(dtProject); }
                    catch { }
                }
                if (dtProject.Rows.Count > 1)
                {
                    dtProject.Clear();
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS [Value], Project AS Display " +
                        "FROM " + Prefix + "ProjectProxy " +
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
                //Markus 14.1.2021: Try to get comprehensible names
                try
                {
                    DiversityWorkbench.ServerConnection SC = new ServerConnection(this.ServerConnection.ConnectionString);
                    DiversityWorkbench.Project P = new Project(SC);
                    foreach (System.Data.DataRow R in dtProject.Rows)
                    {
                        if (R[0].ToString().Length > 0)
                        {
                            SQL = "SELECT ProjectURI FROM " + Prefix + "ProjectProxy WHERE ProjectID = " + R[0].ToString();
                            string URI = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, SC.ConnectionString);
                            if (URI.Length > 0)
                            {
                                System.Collections.Generic.Dictionary<string, string> Dict = P.UnitValues(URI);
                                string Title = "";
                                if (Dict.ContainsKey("Title") && Dict["Title"].Length > 0)
                                {
                                    Title = Dict["Title"].Trim();
                                }
                                if (Title.Length == 0 && Dict.ContainsKey("Description") && Dict["Description"].Length > 0)
                                {
                                    Title = Dict["Description"].Trim();
                                }
                                if (Title.Length > 50)
                                    Title = Title.Substring(0, 50) + "...";
                                if (Title.Length > 0)
                                {
                                    Title = R["Display"] + ":  " + Title;
                                    R["Display"] = Title;
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
                DiversityWorkbench.QueryCondition qProject = new DiversityWorkbench.QueryCondition(true, "GeoProject", "NameID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
                QueryConditions.Add(qProject);

                // Superior place

                // Country

                // Region



                //System.Data.DataTable dtContinent = new System.Data.DataTable();
                //SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                //    "SELECT Abbreviation AS [Value], Abbreviation AS Display " +
                //    "FROM GeoString_Enums " +
                //    "WHERE (Enumeration = N'GazGettyContinents') " +
                //    "ORDER BY Display";
                //Microsoft.Data.SqlClient.SqlDataAdapter aContinent = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                //{
                //    try { aContinent.Fill(dtContinent); }
                //    catch { }
                //}
                //if (dtContinent.Columns.Count == 0)
                //{
                //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                //    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                //    dtContinent.Columns.Add(Value);
                //    dtContinent.Columns.Add(Display);
                //}
                //Description = "The continent";
                //DiversityWorkbench.QueryCondition qContinent = new DiversityWorkbench.QueryCondition(true, "GeoPlaceName", "NameID", "Continent", "Place", "Continent", "Continent of geographical place", Description, dtContinent, false);
                //QueryConditions.Add(qContinent);

                //Description = "Country";
                //DiversityWorkbench.QueryCondition qCountry = new DiversityWorkbench.QueryCondition(true, "GeoPlaceName", "NameID", "Country", "Place", "Country", "Country", Description);
                //QueryConditions.Add(qCountry);

                //Description = "Geographical hierarchy";
                //DiversityWorkbench.QueryCondition qGeoHierarchy = new DiversityWorkbench.QueryCondition(true, "GeoPlaceName", "NameID", "HierarchyList", "Place", "Hierarchy", "Geographical hierarchy", Description);
                //QueryConditions.Add(qGeoHierarchy);
            }
            else
            {
                Description = DiversityWorkbench.Functions.ColumnDescription("GeoPlaceName", "PlaceName");
                DiversityWorkbench.QueryCondition qPlaceName = new DiversityWorkbench.QueryCondition(true, "GeoPlaceName", "NameID", "PlaceName", "Place", "Name", "Name of geographical place", Description);
                QueryConditions.Add(qPlaceName);

                System.Data.DataTable dtPlaceType = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                    "SELECT PlaceTypeID AS [Value], PlaceType  AS Display " +
                    "FROM ";
                if (this._ServerConnection.LinkedServer.Length > 0)
                    SQL += "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
                SQL += "GeoPlaceType " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aPlaceType = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aPlaceType.Fill(dtPlaceType); }
                    catch { }
                }
                if (dtPlaceType.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtPlaceType.Columns.Add(Value);
                    dtPlaceType.Columns.Add(Display);
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("GeoPlaceType", "PlaceType");
                DiversityWorkbench.QueryCondition qPlaceType = new DiversityWorkbench.QueryCondition(true, "GeoPlaceName", "NameID", "PlaceTypeID", "Place", "PlaceType", "Place type", Description, dtPlaceType, false);
                QueryConditions.Add(qPlaceType);

                System.Data.DataTable dtContinent = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION " +
                    "SELECT Abbreviation AS [Value], Abbreviation AS Display " +
                    "FROM GeoString_Enums " +
                    "WHERE (Enumeration = N'GazGettyContinents') " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter aContinent = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aContinent.Fill(dtContinent); }
                    catch { }
                }
                if (dtContinent.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtContinent.Columns.Add(Value);
                    dtContinent.Columns.Add(Display);
                }
                Description = "The continent";
                DiversityWorkbench.QueryCondition qContinent = new DiversityWorkbench.QueryCondition(true, "GeoPlaceName", "NameID", "Continent", "Place", "Continent", "Continent of geographical place", Description, dtContinent, false);
                QueryConditions.Add(qContinent);

                Description = "Country";
                DiversityWorkbench.QueryCondition qCountry = new DiversityWorkbench.QueryCondition(true, "GeoPlaceName", "NameID", "Country", "Place", "Country", "Country", Description);
                QueryConditions.Add(qCountry);

                Description = "Geographical hierarchy";
                DiversityWorkbench.QueryCondition qGeoHierarchy = new DiversityWorkbench.QueryCondition(true, "GeoPlaceName", "NameID", "HierarchyList", "Place", "Hierarchy", "Geographical hierarchy", Description);
                QueryConditions.Add(qGeoHierarchy);
            }
            return QueryConditions;
        }

        private static bool? _IsNewVersionOfDB;
        public static bool IsNewVersionOfDB
        {
            get
            {
                if (_IsNewVersionOfDB == null || (_IsNewVersionOfDB != null && !(bool)_IsNewVersionOfDB))
                {
                    DiversityWorkbench.Gazetteer G = new Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                    _IsNewVersionOfDB = G.IsNewVersionOfDatabase;
                }
                return (bool)_IsNewVersionOfDB;
            }
        }

        private static DiversityWorkbench.ServerConnection _GazetteerServerConnection;

        public static void SetServerConnection(string DisplayText)
        {
        }

        private static System.Data.DataTable _Countries;

        public static System.Data.DataTable Countries()
        {
            if (DiversityWorkbench.Settings.CountryListSource.Length == 0 && DiversityWorkbench.Settings.ModuleName == "DiversityCollection")
            {
                System.Windows.Forms.MessageBox.Show("To get a list of countries please set the source for the country list:\r\nOpen menu Administration - Customize display - Collection event\r\nThere choose the source for the country list");
            }

            if (_Countries == null || _Countries.Rows.Count == 0)
            {
                try
                {
                    string SQL = "";
                    _Countries = new System.Data.DataTable();
                    if (DiversityWorkbench.Settings.CountryListSource == DiversityWorkbench.Settings.ModuleName)
                    {
                        switch (DiversityWorkbench.Settings.ModuleName)
                        {
                            case "DiversityCollection":
                                SQL = "SELECT NULL AS ValueColumn, NULL AS DisplayColumn " +
                                    " UNION " +
                                    " SELECT DISTINCT CountryCache AS ValueColumn, CountryCache AS DisplayColumn FROM [dbo].[CollectionEvent] WHERE [CountryCache] <> '' ORDER BY [DisplayColumn]"; // #277
                                break;
                        }
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(_Countries);
                    }
                    else if (DiversityWorkbench.Settings.CountryListSource == "IsoCountries") // #279
                    {
                        //DWBServices.WebServices.GeoServices.ISOCountries.

                    }
                    else
                    {
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        bool SourceFound = false;
                        if (IsNewVersionOfDB)
                        {
                            string Prefix = "";
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList())
                            {
                                if (KV.Value.DisplayText == DiversityWorkbench.Settings.CountryListSource)
                                {
                                    ad.SelectCommand.Connection.ConnectionString = KV.Value.ConnectionString;
                                    if (KV.Value.LinkedServer.Length > 0)
                                        Prefix = "[" + KV.Value.LinkedServer + "]." + KV.Value.DatabaseName + ".dbo.";
                                    else Prefix = "dbo.";
                                    SourceFound = true;
                                    break;
                                }
                            }

                            SQL = "SELECT NULL AS ValueColumn, NULL AS DisplayColumn " +
                                " UNION " + "SELECT N.Name AS ValueColumn, N.Name AS DisplayColumn " +
                                "FROM " + Prefix + "ViewGeoPlace P INNER JOIN " + Prefix + "GeoName N ON P.PreferredNameID = N.NameID AND N.PlaceID = P.PlaceID " +
                                "WHERE     (P.PlaceType = N'nation') " +
                                "order by DisplayColumn";
                        }
                        if (SourceFound)
                        {
                            ad.SelectCommand.CommandText = SQL;
                            ad.Fill(_Countries);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return _Countries;
        }

        public static void ResetCountries() { _Countries = null; }


        #endregion

        #region Properties

        #endregion

        #region Public functions

        public static string QuadrantForTK25(float Longitude, float Latitude, string Point1FromSql, string Point2FromSql, string Point3FromSql, string Point4FromSql, ref int Depth, ref System.Drawing.RectangleF Quadrant)
        {
            if (Depth == 0)
                return "";
            System.Drawing.PointF Point1 = PointFromSqlString(Point1FromSql);
            System.Drawing.PointF Point2 = PointFromSqlString(Point2FromSql);
            System.Drawing.PointF Point3 = PointFromSqlString(Point3FromSql);
            System.Drawing.PointF Point4 = PointFromSqlString(Point4FromSql);
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
            string Q = QuadrantForTK25(Longitude, Latitude, Rect, ref Quadrant, ref Depth);
            return Q;
        }

        private static string QuadrantForTK25(float Longitude, float Latitude, System.Drawing.RectangleF Rect, ref System.Drawing.RectangleF Quadrant, ref int Depth)
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
                Q += QuadrantForTK25(Longitude, Latitude, RectQuadrant, ref Quadrant, ref Depth);
            return Q;
        }

        private static System.Drawing.PointF PointFromSqlString(string Point)
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

        public static string CoordinatesForTK25Quadrant(string ConnectionString, string TK25, string Quadrant, ref float Longitude, ref float Latitude)
        {
            bool OK = true;
            string Geography = "";
            try
            {
                string SQL = "SELECT P.PlaceID    " +
                  ", P.SuperiorPlaceID " +
                  ", P.Geography.STAsText() AS Polygon " +
                  ", SUBSTRING(N.Name, 1, 4) AS TK25 " +
                  ", N.Name AS TK25_Name" +
                  ", P.Geography.STPointN(1).ToString() AS Point1 " +
                  ", P.Geography.STPointN(2).ToString() AS Point2 " +
                  ", P.Geography.STPointN(3).ToString() AS Point3 " +
                  ", P.Geography.STPointN(4).ToString() AS Point4 " +
                  ", P.Geography.STPointN(1).Lat as Lat1 " +
                  ", P.Geography.STPointN(1).Long as Lon1  " +
                  ", P.Geography.STPointN(2).Lat as Lat2  " +
                  ", P.Geography.STPointN(2).Long as Lon2  " +
                  ", P.Geography.STPointN(3).Lat as Lat3  " +
                  ", P.Geography.STPointN(3).Long as Lon3  " +
                  ", P.Geography.STPointN(4).Lat as Lat4  " +
                  ", P.Geography.STPointN(4).Long as Lon4  " +
                  "FROM GeoPlace AS P INNER JOIN " +
                  "GeoName AS N ON P.PlaceID = N.PlaceID " +
                  "WHERE (N.Name LIKE '" + TK25 + " - %') AND (P.PlaceType = N'TK25') ";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    float Lat1 = 0;
                    float Long1 = 0;
                    float Lat2 = 0;
                    float Long2 = 0;
                    float Lat3 = 0;
                    float Long3 = 0;
                    float Lat4 = 0;
                    float Long4 = 0;
                    if (float.TryParse(dt.Rows[0]["Lat1"].ToString(), out Lat1)
                        && float.TryParse(dt.Rows[0]["Lon1"].ToString(), out Long1)
                        && float.TryParse(dt.Rows[0]["Lat2"].ToString(), out Lat2)
                        && float.TryParse(dt.Rows[0]["Lon2"].ToString(), out Long2)
                        && float.TryParse(dt.Rows[0]["Lat3"].ToString(), out Lat3)
                        && float.TryParse(dt.Rows[0]["Lon3"].ToString(), out Long3)
                        && float.TryParse(dt.Rows[0]["Lat4"].ToString(), out Lat4)
                        && float.TryParse(dt.Rows[0]["Lon4"].ToString(), out Long4))
                    {
                        for (int i = 0; i < Quadrant.Length; i++)
                        {
                            int Q;
                            if (int.TryParse(Quadrant[i].ToString(), out Q))
                            {
                                switch (Q)
                                {
                                    case 1:
                                        Long4 = Long1 + (Long4 - Long1) / (float)2;
                                        Long3 = Long2 + (Long3 - Long2) / (float)2;
                                        Lat3 = Lat3 + (Lat4 - Lat3) / (float)2;
                                        Lat2 = Lat2 + (Lat1 - Lat2) / (float)2;
                                        break;
                                    case 2:
                                        Long1 = Long1 + (Long4 - Long1) / (float)2;
                                        Long2 = Long2 + (Long3 - Long2) / (float)2;
                                        Lat2 = Lat2 + (Lat1 - Lat2) / (float)2;
                                        Lat3 = Lat3 + (Lat4 - Lat3) / (float)2;
                                        break;
                                    case 3:
                                        Long3 = Long2 + (Long3 - Long2) / (float)2;
                                        Long4 = Long1 + (Long4 - Long1) / (float)2;
                                        Lat4 = Lat3 + (Lat4 - Lat3) / (float)2;
                                        Lat1 = Lat2 + (Lat1 - Lat2) / (float)2;
                                        break;
                                    case 4:
                                        Long2 = Long2 + (Long3 - Long2) / (float)2;
                                        Long1 = Long1 + (Long4 - Long1) / (float)2;
                                        Lat1 = Lat2 + (Lat1 - Lat2) / (float)2;
                                        Lat4 = Lat3 + (Lat4 - Lat3) / (float)2;
                                        break;
                                    default:
                                        OK = false;
                                        break;
                                }
                            }
                            else
                            {
                                OK = false;
                                break;
                            }
                        }
                    }
                    if (OK)
                    {
                        Latitude = (Lat1 + Lat2) / (float)2;
                        Longitude = (Long1 + Long4) / (float)2;
                        Geography = "POLYGON ((" + Long1.ToString() + " " + Lat1.ToString() +
                            ", " + Long2.ToString() + " " + Lat2.ToString() +
                            ", " + Long3.ToString() + " " + Lat3.ToString() +
                            ", " + Long4.ToString() + " " + Lat4.ToString() +
                            ", " + Long1.ToString() + " " + Lat1.ToString() + "))";
                    }
                }
                else
                    OK = false;
            }
            catch (System.Exception ex) { OK = false; }
            return Geography;
        }

        #endregion


    }
}
