using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using DWBServices;
using DWBServices.WebServices;
using DWBServices.WebServices.GeoServices.Geonames;
using DWBServices.WebServices.TaxonomicServices.CatalogueOfLife;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.Logging;

namespace DiversityWorkbench
{
    public struct GeoPoint
    {
        public double Latitude, Lonigitude, Altitude;
        string Label;
    }

    public struct GeoCoordinate
    {
        //public GeoFunctions.GeoDirection Direction;
        public bool NumericIsOriginalValue;
        //North-South values
        public double NumericNS;
        public int DegreeNS;
        public int MinutesNS;
        public double SecondsNS;
        //east-west values
        public double NumericEW;
        public int DegreeEW;
        public int MinutesEW;
        public double SecondsEW;
        /// <summary>
        /// the accuray in m
        /// </summary>
        public double Accuracy;
        public double AccuracySpanNS;
        public double AccuracySpanEW;
        public string AccuracyText;
    }

    public class GeoFunctions
    {
        
        public enum GeoInfo { PlaceName, Country, CountrySubdivision, City, Altitude, AltitudeSource, Longitude, Latitude, Distance, Direction, Source }

        public enum GeoNamesOrgServices { findNearbyPlaceName, countrySubdivision, neighbourhood, srtm3, gtopo30 }

        public enum GeoDirection {Latitude, Longitude};

        public static bool GeoNamesAvailable
        {
            get
            {
                double Latitude = 0.0;
                double Longitude = 0.0;
                try
                {
                    string geoUri = global::DiversityWorkbench.Properties.Settings.Default.APIGeonamesURL;
                    string username = global::DiversityWorkbench.Properties.Settings.Default.GeonamesUsername;
                    string URI = geoUri + "findNearbyPlaceName?lat=" + Latitude.ToString().Replace(",", ".") + "&lng=" + Longitude.ToString().Replace(",", ".") + "&username=" + username + "&style=full";
                    System.Uri u = new Uri(URI);
                    System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                    dom.Load(URI);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static System.Collections.Generic.Dictionary<GeoInfo, string> getGeoInfos(double Latitude, double Longitude)
        {
            bool ServiceAvailable = true;
            System.Collections.Generic.Dictionary<GeoInfo, string> GeoInfos = DiversityWorkbench.GeoFunctions.getGeoInfos(Latitude, Longitude, ref ServiceAvailable);
            if (!ServiceAvailable)
            {
                string Message = "The service is not available.";
                System.Windows.Forms.MessageBox.Show(Message);
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("GeoFunctions.getGeoInfos() Service is not available.");
            }

            return GeoInfos;
        }

        //private string CreateSearchUrl(double Latitude, double Longitude)
        //{
        //    var queryRestriction = QueryRestriction(double Latitude, double Longitude);
        //    try
        //    {
        //        return _api?.DwbApiQueryUrlString(currentDwbService, queryRestriction, offset, MaxRecords) ?? string.Empty;
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        MessageBox.Show("You must enter a valid name as an option. This must be at least three characters long. :" + ex.Message, "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        ExceptionHandling.WriteToErrorLogFile("You must enter a valid name as an option. This must be at least three characters long. :" + ex);
        //        return "";
        //    }
        //}

        public static System.Collections.Generic.Dictionary<GeoInfo, string> getGeoInfos(double Latitude, double Longitude, ref bool ServiceAvailable)
        {
            ServiceAvailable = true;
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            double DiffLatitude = 0.0;
            double DiffLongitude = 0.0;
            System.Collections.Generic.Dictionary<GeoInfo, string> GeoInfos = new Dictionary<GeoInfo, string>();
            try
            {
                IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api =
                DwbServiceProviderAccessor.GetDwbWebservice(DWBServices.WebServices.DwbServiceEnums.DwbService.Geonames);
                if (_api == null)
                {
                    ServiceAvailable = false;
                    return GeoInfos;
                }

                string _BaseURI = _api.GetBaseAddress();
                string URI = _BaseURI + string.Format("{0}?lat={1}&lng={2}", "findNearbyPlaceName", Latitude, Longitude);
                URI = _api.DwbApiQueryUrlString(DwbServiceEnums.DwbService.Geonames, URI, 0, 50);
                GeoInfos.Add(GeoInfo.Source, _BaseURI);
                System.Uri u = new Uri(URI);
                System.Xml.XmlDocument dom = new System.Xml.XmlDocument();
                dom.Load(URI);
                if (dom.DocumentElement.ChildNodes.Count > 0)
                {
                    foreach (System.Xml.XmlNode inXmlNode in dom.DocumentElement)
                    {
                        if (inXmlNode.Name.ToLower() == "geoname")
                        {
                            foreach (System.Xml.XmlNode c1 in inXmlNode.ChildNodes)
                            {
                                if (c1.Name.ToLower() == "name")
                                {
                                    GeoInfos.Add(GeoInfo.PlaceName, c1.InnerText);
                                    continue;
                                }
                                if (c1.Name.ToLower() == "lat")
                                {
                                    GeoInfos.Add(GeoInfo.Latitude, c1.InnerText);
                                    if (double.TryParse(c1.InnerText, System.Globalization.NumberStyles.Float, InvC, out DiffLatitude))
                                        DiffLatitude = DiffLatitude - Latitude;
                                    continue;
                                }
                                if (c1.Name.ToLower() == "lng")
                                {
                                    GeoInfos.Add(GeoInfo.Longitude, c1.InnerText);
                                    if (double.TryParse(c1.InnerText, System.Globalization.NumberStyles.Float, InvC, out DiffLongitude))
                                        DiffLongitude = DiffLongitude - Longitude;
                                    continue;
                                }
                                if (c1.Name.ToLower() == "countryname")
                                {
                                    GeoInfos.Add(GeoInfo.Country, c1.InnerText);
                                    continue;
                                }
                                if (c1.Name.ToLower() == "distance")
                                {
                                    string Distance = c1.InnerText.ToString(InvC);
                                    double distance = 0.0;
                                    if (double.TryParse(Distance, System.Globalization.NumberStyles.Float, InvC, out distance))
                                    {
                                        distance = distance * 1000;
                                        GeoInfos.Add(GeoInfo.Distance, distance.ToString(InvC) + " m");
                                    }
                                    continue;
                                }
                            }
                        }
                    }
                }
                //// Subdivision
                //URI = geoUri + "countrySubdivision?lat=" + Latitude.ToString().Replace(",", ".") + "&lng=" + Longitude.ToString().Replace(",", ".") + "&username=" + username + "&style=full";
                //System.Xml.XmlDocument domSub = new System.Xml.XmlDocument();
                //domSub.Load(URI);
                //if (domSub.DocumentElement.ChildNodes.Count > 0)
                //{
                //    foreach (System.Xml.XmlNode inXmlNode in domSub.DocumentElement)
                //    {
                //        if (inXmlNode.Name.ToLower() == "countrysubdivision")
                //        {
                //            foreach (System.Xml.XmlNode c1 in inXmlNode.ChildNodes)
                //            {
                //                if (c1.Name.ToLower() == "adminname1")
                //                {
                //                    GeoInfos.Add(GeoInfo.CountrySubdivision, c1.InnerText);
                //                    continue;
                //                }
                //            }
                //        }
                //    }
                //}

                //// City
                //URI = geoUri + "neighbourhood?lat=" + Latitude.ToString().Replace(",", ".") + "&lng=" + Longitude.ToString().Replace(",", ".") + "&username=" + username + "&style=full";
                //System.Xml.XmlDocument domNei = new System.Xml.XmlDocument();
                //domNei.Load(URI);
                //if (domSub.DocumentElement.ChildNodes.Count > 0)
                //{
                //    foreach (System.Xml.XmlNode inXmlNode in domNei.DocumentElement)
                //    {
                //        if (inXmlNode.Name.ToLower() == "neighbourhood")
                //        {
                //            foreach (System.Xml.XmlNode c1 in inXmlNode.ChildNodes)
                //            {
                //                if (c1.Name.ToLower() == "city")
                //                {
                //                    GeoInfos.Add(GeoInfo.City, c1.InnerText);
                //                    continue;
                //                }
                //            }
                //        }
                //    }
                //}

                //// Direction
                //if (DiffLongitude != 0.0 && DiffLatitude != 0.0)
                //{
                //    string Direction = "";
                //    double RelDirection = DiffLatitude / DiffLongitude;
                //    double Angle = System.Math.Atan2(DiffLatitude, DiffLongitude) * (180 / Math.PI);
                //    if ((Angle > 332.5 && Angle <= 360) || (Angle > 0 && Angle <= 22.5))
                //        Direction = "W";
                //    else if (Angle > 22.5 && Angle <= 67.5)
                //        Direction = "SW";
                //    else if (Angle > 67.5 && Angle <= 112.5)
                //        Direction = "S";
                //    else if (Angle > 112.5 && Angle <= 157.5)
                //        Direction = "SE";
                //    else if (Angle > 157.5 && Angle <= 202.5)
                //        Direction = "E";
                //    else if (Angle > 202.5 && Angle <= 247.5)
                //        Direction = "NE";
                //    else if (Angle > 247.5 && Angle <= 292.5)
                //        Direction = "N";
                //    else if (Angle > 292.5 && Angle <= 332.5)
                //        Direction = "NW";

                //    else if ((Angle > -22.5 && Angle <= 0) || (Angle > 0 && Angle <= 22.5))
                //        Direction = "W";
                //    else if (Angle < -22.5 && Angle >= -67.5)
                //        Direction = "NW";
                //    else if (Angle < -67.5 && Angle >= -112.5)
                //        Direction = "N";
                //    else if (Angle < -112.5 && Angle >= -157.5)
                //        Direction = "NE";
                //    else if (Angle < -157.5 && Angle >= -180)
                //        Direction = "E";

                //    if (Direction.Length > 0)
                //        GeoInfos.Add(GeoInfo.Direction, Direction);
                //}

                //// Altitude
                //string Altitude = "";
                //if (Latitude < 60 && Latitude > -54)
                //{
                //    Altitude = DiversityWorkbench.GeoFunctions.WebResponse(GeoNamesOrgServices.srtm3, Latitude, Longitude);
                //    float fAlt;
                //    if (float.TryParse(Altitude, out fAlt))
                //    {
                //        if (fAlt < -400)
                //        {
                //            Altitude = DiversityWorkbench.GeoFunctions.WebResponse(GeoNamesOrgServices.gtopo30, Latitude, Longitude);
                //            GeoInfos.Add(GeoInfo.AltitudeSource, DiversityWorkbench.GeoFunctions.GeoInfoSource + " (global digital elevation model)");
                //        }
                //        else
                //        {
                //            GeoInfos.Add(GeoInfo.AltitudeSource, DiversityWorkbench.GeoFunctions.GeoInfoSource + " (Shuttle Radar Topography Mission)");
                //        }
                //    }
                //}
                //else
                //{
                //    Altitude = DiversityWorkbench.GeoFunctions.WebResponse(GeoNamesOrgServices.gtopo30, Latitude, Longitude);
                //    GeoInfos.Add(GeoInfo.AltitudeSource, DiversityWorkbench.GeoFunctions.GeoInfoSource + " (global digital elevation model)");
                //}
                //if (Altitude.Length > 0)
                //{
                //    if (Altitude == "-9999")
                //        GeoInfos.Remove(GeoInfo.AltitudeSource);
                //    else
                //    {
                //        if (Altitude.Length > 0)
                //        {
                //            GeoInfos.Add(GeoInfo.Altitude, Altitude);
                //        }
                //    }
                //}

                //GeoInfos.Add(GeoInfo.Source, DiversityWorkbench.GeoFunctions.GeoInfoSource);
            }
            catch (WebException wex)
            {
                ServiceAvailable = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return GeoInfos;
        }

        public static string WebResponse(GeoFunctions.GeoNamesOrgServices Service, double Latitude, double Longitude)
        {
            if (Service != GeoNamesOrgServices.gtopo30
                && Service != GeoNamesOrgServices.srtm3)
                return "";
            string Response = "";
            try
            {
                string geoUri = global::DiversityWorkbench.Properties.Settings.Default.APIGeonamesURL;
                string username = global::DiversityWorkbench.Properties.Settings.Default.GeonamesUsername;
                string URI = geoUri + Service.ToString() + "?lat=" + Latitude.ToString().Replace(",", ".") + "&lng=" + Longitude.ToString().Replace(",", ".") + "&username=" + username + "&style=full";

                System.Net.WebRequest webrq = System.Net.WebRequest.Create(URI);
                WebRequest myWebRequest = WebRequest.Create(URI);
                WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);
                Response = readStream.ReadToEnd(); ;
                readStream.Close();
                myWebResponse.Close();
                if (Response.Length > 0)
                    Response = Response.Substring(0, Response.Length - 2);
            }
            catch { }

            return Response;
        }

        public static System.Collections.Generic.List<GeoPoint> GeoPointsFromGeography(Microsoft.SqlServer.Types.SqlGeography Geography)
        {
            System.Collections.Generic.List<GeoPoint> Points = new List<GeoPoint>();
            return Points;
        }

        public static System.Collections.Generic.List<GeoPoint> GeoPointsFromGeography(string Geography)
        {
            System.Collections.Generic.List<GeoPoint> Points = new List<GeoPoint>();
            return Points;
        }

        public static System.Collections.Generic.List<string> getGeographyStringsForGoogleMaps(string STAsText)
        {
            System.Collections.Generic.List<string> GeographyStringForGoogleMaps = new List<string>();

            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            string LatForMap = "Lat=";
            string LonForMap = "&Lng=";
            string LatDotForMap = "&LatPoint=";
            string LonDotForMap = "&LngPoint=";
            string LabelForPoints = "&Label=";
            string URI = "";
            string Points = "";
            string Label = "";
            string GeoObjectType = STAsText.Substring(0, STAsText.IndexOf("("));
            string Coordinates = STAsText.Substring(STAsText.IndexOf("(") + 1);
            Coordinates = Coordinates.Substring(0, Coordinates.Length - 1);
            string[] CoordinatePoints = Coordinates.Split(new char[] { ',' });
            for (int i = 0; i < CoordinatePoints.Length; i++)
            {
                try
                {
                    string[] LatLon = CoordinatePoints[i].Trim().Split(new char[] { ' ' });
                    if (GeoObjectType.ToUpper() == "POINT")
                    {
                        LatDotForMap += LatLon[1].ToString(InvC).Replace(",", ".") + "|";
                        LonDotForMap += LatLon[0].ToString(InvC).Replace(",", ".") + "|";
                        LabelForPoints += i.ToString() + "|";
                    }
                    else
                    {
                        LatForMap += LatLon[1].ToString(InvC).Replace(",", ".") + "|";
                        LonForMap += LatLon[0].ToString(InvC).Replace(",", ".") + "|";
                    }
                }
                catch { }
            }
            if (LatDotForMap.Length > 0)
            {
                Points = LatDotForMap.Substring(0, LatDotForMap.Length - 1) + LonDotForMap.Substring(0, LonDotForMap.Length - 1);
                Label = LabelForPoints.Substring(0, LabelForPoints.Length - 1);
            }
            URI = LatForMap.Substring(0, LatForMap.Length - 1) + LonForMap.Substring(0, LonForMap.Length - 1);

            GeographyStringForGoogleMaps.Add(URI);
            GeographyStringForGoogleMaps.Add("");
            GeographyStringForGoogleMaps.Add(Points);
            GeographyStringForGoogleMaps.Add(Label);

            return GeographyStringForGoogleMaps;
        }

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
        }

        public static System.Collections.Generic.List<GeoPoint> compressGeoPoints(System.Collections.Generic.List<GeoPoint> Points, int MaxPoints, string PointIDColumn, string ColumnLatitude, string ColumnLongitude)
        {
            System.Data.DataTable dtGeoPoints = new System.Data.DataTable();
            System.Data.DataColumn CPoint = new System.Data.DataColumn(PointIDColumn, System.Type.GetType("System.Int16"));
            System.Data.DataColumn CLat = new System.Data.DataColumn(ColumnLatitude, System.Type.GetType("System.Double"));
            System.Data.DataColumn CLon = new System.Data.DataColumn(ColumnLongitude, System.Type.GetType("System.Double"));
            dtGeoPoints.Columns.Add(CPoint);
            dtGeoPoints.Columns.Add(CLat);
            dtGeoPoints.Columns.Add(CLon);
            for (int i = 1; i < Points.Count; i++ )
            {
                System.Data.DataRow RGeo = dtGeoPoints.NewRow();
                RGeo[PointIDColumn] = i;
                RGeo[ColumnLatitude] = Points[i].Latitude;
                RGeo[ColumnLongitude] = Points[i].Lonigitude;
                dtGeoPoints.Rows.Add(RGeo);
            }

            System.Data.DataTable dtGeoPointsCompressed = DiversityWorkbench.GeoFunctions.compressGeoPoints(dtGeoPoints, MaxPoints, PointIDColumn, ColumnLatitude, ColumnLongitude);
            System.Collections.Generic.List<GeoPoint> PP = new List<GeoPoint>();
            foreach (System.Data.DataRow R in dtGeoPointsCompressed.Rows)
            {
                GeoPoint P = new GeoPoint();
                P.Latitude = double.Parse(R[ColumnLatitude].ToString());
                P.Lonigitude = double.Parse(R[ColumnLongitude].ToString());
                PP.Add(P);
            }
            return PP;
        }

        public static void ConvertDegreeToNumeric(string CoordinateNS, string CoordinateEW, ref GeoCoordinate GeoCoordinate, ref bool ConversionSuccessful)
        {
            //double C = 0.0;
            //int Border = 180;
            //int CurrentPositionInString = 0;
            ConversionSuccessful = true;
            GeoCoordinate.NumericIsOriginalValue = false;
            //System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            //int Sign = 1;
            try
            {
                GeoConversion.GeoCon Converter = new GeoConversion.GeoCon();

                // North/South
                if (CoordinateNS.IndexOf(" - ") > -1)
                {
                    string[] CC = CoordinateNS.Split(new char[] { '-' });
                    double CC0 = ConvertDegreeToNumeric(CC[0], ref ConversionSuccessful, ref GeoCoordinate.NumericIsOriginalValue);
                    if (!ConversionSuccessful)
                    {
                        if (!Converter.ConvertCoordinateToDecimal(CC[0], ref CC0))
                            return;
                        else
                            ConversionSuccessful = true;
                    }
                    double CC1 = ConvertDegreeToNumeric(CC[1], ref ConversionSuccessful, ref GeoCoordinate.NumericIsOriginalValue);
                    if (!ConversionSuccessful)
                    {
                        if (!Converter.ConvertCoordinateToDecimal(CC[1], ref CC1))
                            return;
                        else
                            ConversionSuccessful = true;
                    }
                    if (ConversionSuccessful)
                    {
                        GeoCoordinate.NumericNS = (CC0 + CC1) / 2;
                        GeoCoordinate.AccuracySpanNS = System.Math.Sign(CC0 - CC1) * (CC0 - CC1);
                    }
                }
                else
                {
                    GeoCoordinate.NumericNS = DiversityWorkbench.GeoFunctions.ConvertDegreeToNumeric(CoordinateNS, ref ConversionSuccessful, ref GeoCoordinate.NumericIsOriginalValue);
                    if (!ConversionSuccessful)
                    {
                        if (!Converter.ConvertCoordinateToDecimal(CoordinateNS, ref GeoCoordinate.NumericNS))
                            return;
                        else
                            ConversionSuccessful = true;
                    }
                }

                // East/West
                if (CoordinateEW.IndexOf(" - ") > -1)
                {
                    string[] CC = CoordinateEW.Split(new char[] { '-' });
                    double CC0 = ConvertDegreeToNumeric(CC[0], ref ConversionSuccessful, ref GeoCoordinate.NumericIsOriginalValue);
                    if (!ConversionSuccessful)
                    {
                        if (!Converter.ConvertCoordinateToDecimal(CC[0], ref CC0))
                            return;
                        else
                            ConversionSuccessful = true;
                    }
                    double CC1 = ConvertDegreeToNumeric(CC[1], ref ConversionSuccessful, ref GeoCoordinate.NumericIsOriginalValue);
                    if (!ConversionSuccessful)
                    {
                        if (!Converter.ConvertCoordinateToDecimal(CC[1], ref CC1))
                            return;
                        else
                            ConversionSuccessful = true;
                    }
                    if (ConversionSuccessful)
                    {
                        GeoCoordinate.NumericEW = (CC0 + CC1) / 2;
                        GeoCoordinate.AccuracySpanEW = System.Math.Sign(CC0 - CC1) * (CC0 - CC1);
                    }
                }
                else
                {
                    GeoCoordinate.NumericEW = DiversityWorkbench.GeoFunctions.ConvertDegreeToNumeric(CoordinateEW, ref ConversionSuccessful, ref GeoCoordinate.NumericIsOriginalValue);
                    if (!ConversionSuccessful)
                    {
                        if (!Converter.ConvertCoordinateToDecimal(CoordinateEW, ref GeoCoordinate.NumericEW))
                            return;
                        else
                            ConversionSuccessful = true;
                    }
                }
                DiversityWorkbench.GeoFunctions.SetDegreeFromNumeric(ref GeoCoordinate, ref ConversionSuccessful);
                DiversityWorkbench.GeoFunctions.AccuracySetValues(ref GeoCoordinate, ref ConversionSuccessful);


            }
            catch { ConversionSuccessful = false; }
            //GeoCoordinate.NumericNS = C * Sign;
            //return C * Sign;
        }

        public static double ConvertDegreeToNumeric(string Coordinate, ref bool ConversionSuccessful, ref bool SourceIsNumeric)
        {
            int Test = 0;
            System.Collections.Generic.Dictionary<string, int> SignList = new Dictionary<string, int>();
            SignList.Add("W", -1);
            SignList.Add("E", 1);
            SignList.Add("O", 1);
            SignList.Add("N", 1);
            SignList.Add("S", -1);
            DiversityWorkbench.GeoFunctions.GeoDirection Direction = GeoDirection.Longitude;
            // finding the sign
            int Sign = 1;
            foreach (char c in Coordinate)
            {
                if (SignList.ContainsKey(c.ToString().ToUpper()))
                {
                    Sign = SignList[c.ToString().ToUpper()];
                    if (c.ToString().ToUpper() == "N" || c.ToString().ToUpper() == "S")
                        Direction = GeoDirection.Latitude;
                    break;
                }
            }

            double C = 0.0;
            int Border = 180;
            if (Direction == GeoDirection.Latitude) Border = 90;
            int CurrentPositionInString = 0;
            ConversionSuccessful = true;
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            try
            {
                if (double.TryParse(Coordinate, System.Globalization.NumberStyles.Number, InvC, out C))
                {
                    SourceIsNumeric = true;
                    if (C > Border || C < -Border)
                    {
                        if (double.TryParse(Coordinate.Replace(',', '.'), System.Globalization.NumberStyles.Number, InvC, out C))
                        {
                            SourceIsNumeric = true;
                            if (C > Border && C < -Border)
                            {
                                ConversionSuccessful = false; ;
                            }
                        }
                    }
                    if (ConversionSuccessful)
                    {
                        return C;
                    }
                }
                else SourceIsNumeric = false;

                // finding the degrees
                string Degree = "";
                // finding the first numeric
                for (int i = 0; i < Coordinate.Length; i++)
                {
                    if (int.TryParse(Coordinate[i].ToString(), out Test))
                    {
                        CurrentPositionInString = i;
                        break;
                    }
                }
                for (int i = CurrentPositionInString; i < Coordinate.Length; i++)
                {

                    if (Coordinate[i] == '°')
                    {
                        CurrentPositionInString = i;
                        break;
                    }
                    else if (int.TryParse(Coordinate[i].ToString(), out Test))
                        Degree += Coordinate[i];
                    else if (Coordinate[i] == ' ' && Degree.Length > 0)
                    {
                        CurrentPositionInString = i;
                        break;
                    }
                }

                // finding the minutes
                string Minutes = "";
                for (int i = CurrentPositionInString; i < Coordinate.Length; i++)
                {

                    if (Coordinate[i].ToString() == "'"
                        || Coordinate[i].ToString() == "′")
                    {
                        CurrentPositionInString = i;
                        break;
                    }
                    else if (int.TryParse(Coordinate[i].ToString(), out Test))
                        Minutes += Coordinate[i];
                    else if (Coordinate[i] == ' ' && Minutes.Length > 0)
                    {
                        CurrentPositionInString = i;
                        break;
                    }
                }

                // finding the seconds
                string Seconds = "";
                for (int i = CurrentPositionInString; i < Coordinate.Length; i++)
                {

                    if (Coordinate[i].ToString() == "''"
                        || Coordinate[i].ToString() == "″")
                    {
                        CurrentPositionInString = i;
                        break;
                    }
                    else if (int.TryParse(Coordinate[i].ToString(), out Test)
                        || Coordinate[i] == ','
                        || Coordinate[i] == '.')
                        Seconds += Coordinate[i];
                }
                double dDegree;
                if (double.TryParse(Degree, out dDegree))
                {
                    C = dDegree;
                }
                else ConversionSuccessful = false;
                if (C > Border || C < -Border)
                    ConversionSuccessful = false;
                double dMinutes;
                if (double.TryParse(Minutes, out dMinutes))
                {
                    if (dMinutes > 60)
                        ConversionSuccessful = false;
                    dMinutes = dMinutes / 60;
                    C += dMinutes;
                    double dSeconds;
                    if (double.TryParse(Seconds.Replace(',', '.'), System.Globalization.NumberStyles.Number, InvC, out dSeconds))
                    {
                        if (dSeconds > 60)
                            ConversionSuccessful = false;
                        dSeconds = dSeconds / (60 * 60);
                        C += dSeconds;
                    }
                }
                else ConversionSuccessful = false;
            }
            catch { ConversionSuccessful = false; }
            return C * Sign;
        }

        private static void SetDegreeFromNumeric(ref GeoCoordinate GeoCoordinate, ref bool ConversionSuccessful)
        {
            try
            {
                if (GeoCoordinate.NumericNS != null
                    && GeoCoordinate.NumericNS != 0)
                {
                    if (DiversityWorkbench.GeoFunctions.SetDegreeFromNumeric(GeoCoordinate.NumericNS, ref GeoCoordinate.DegreeNS, ref GeoCoordinate.MinutesNS, ref GeoCoordinate.SecondsNS))
                        ConversionSuccessful = true;
                    else ConversionSuccessful = false;
                    if (ConversionSuccessful &&
                        GeoCoordinate.NumericEW != null
                        && GeoCoordinate.NumericEW != 0)
                    {
                        if (DiversityWorkbench.GeoFunctions.SetDegreeFromNumeric(GeoCoordinate.NumericEW, ref GeoCoordinate.DegreeEW, ref GeoCoordinate.MinutesEW, ref GeoCoordinate.SecondsEW))
                            ConversionSuccessful = true;
                        else ConversionSuccessful = false;
                    }
                }
            }
            catch (Exception)
            {
                ConversionSuccessful = false;
            }
        }

        private static bool SetDegreeFromNumeric(double Coordinate, ref int Degree, ref int Minutes, ref double Seconds)
        {
            try
            {
                if (Coordinate != null
                    && Coordinate != 0)
                {
                    // Deg
                    Degree = (int)Coordinate * Math.Sign(Coordinate);
                    // Min
                    double MinD = Coordinate * Math.Sign(Coordinate) - Degree;
                    MinD = MinD * 0.6;
                    MinD = ((MinD * 100));
                    Minutes = (int)MinD;
                    // Sec
                    double SecD = MinD - Minutes;
                    SecD *= 60;
                    decimal dSec = (decimal)SecD;
                    dSec = Math.Round(dSec, 9);
                    SecD = (double)dSec;
                    if (SecD >= 60)
                    {
                        SecD -= 60;
                        Minutes += 1;
                    }
                    decimal dSecD = (decimal)SecD;
                    dSecD = System.Math.Round(dSecD, 9);
                    SecD = (double)dSecD;
                    Seconds = SecD;
                    Degree = System.Math.Abs(Degree);
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool ConvertNumericToDegree(double Latitude, double Longitude, ref string DegreeNS, ref string DegreeEW, int RoundSeconds)
        {
            bool OK = true;
            try
            {
                int LatPrefix = 1;
                int LatDeg = 0;
                int LatMin = 0;
                double LatSek = 0;
                ConvertNumericToDegree(Latitude, ref LatPrefix, ref LatDeg, ref LatMin, ref LatSek);
                string sLatDeg = LatDeg.ToString();
                if (LatPrefix < 0) sLatDeg = "- " + sLatDeg;
                DegreeNS = sLatDeg + "° ";
                if (LatMin > 0) DegreeNS += LatMin.ToString() + "' ";
                if (LatSek > 0)
                {
                    if (Math.Round(LatSek, RoundSeconds) > 0)
                    {
                        DegreeNS += Math.Round(LatSek, RoundSeconds).ToString() + "''";
                    }
                }

                int LongPrefix = 1;
                int LongDeg = 0;
                int LongMin = 0;
                double LongSek = 0;
                ConvertNumericToDegree(Longitude, ref LongPrefix, ref LongDeg, ref LongMin, ref LongSek);
                string sLongDeg = LongDeg.ToString();
                if (LongPrefix < 0) sLongDeg = "- " + sLongDeg;
                DegreeEW = sLongDeg + "° ";
                if (LongMin > 0) DegreeEW += LongMin.ToString() + "' ";
                if (LongSek > 0)
                {
                    if (Math.Round(LongSek, RoundSeconds)  > 0)
                        DegreeEW += Math.Round(LongSek, RoundSeconds).ToString() + "''";
                }

            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        public static bool ConvertNumericToDegree(double Numeric, ref int Prefix, ref int Deg, ref int Min, ref double Sec)
        {
            bool OK = true;
            try
            {
                //Prefix
                if (Numeric < 0) Prefix = -1;
                else Prefix = 1;
                // Deg
                Deg = (int)Numeric * Math.Sign(Numeric);
                // Min
                double MinD = Numeric * Math.Sign(Numeric) - Deg;
                MinD = MinD * 0.6;
                MinD = ((MinD * 100));
                Min = (int)MinD;
                // Sec
                double SecD = MinD - Min;
                SecD *= 60;
                decimal dSec = (decimal)SecD;
                dSec = Math.Round(dSec, 9);
                SecD = (double)dSec;
                if (SecD >= 60)
                {
                    SecD -= 60;
                    Min += 1;
                }
                decimal dSecD = (decimal)SecD;
                dSecD = System.Math.Round(dSecD, 9);
                SecD = (double)dSecD;
                Sec = SecD;
                Deg = System.Math.Abs(Deg);

            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        #region Accuracy

        public static bool AccuracySetValues(ref GeoCoordinate Coordinate, ref bool ConversionSuccessful)
        {
            if (Coordinate.NumericIsOriginalValue)
            {
                if (Coordinate.AccuracySpanEW > 0 || Coordinate.AccuracySpanNS > 0)
                {
                    if (!DiversityWorkbench.GeoFunctions.AccuracyFromNumericRange(ref Coordinate))
                        ConversionSuccessful = false;
                }
                else
                {
                    DiversityWorkbench.GeoFunctions.AccuracyFromNumeric(Coordinate.NumericNS, Coordinate.NumericEW, ref Coordinate.Accuracy, ref Coordinate.AccuracyText);
                }
            }
            else
            {
                if (Coordinate.AccuracySpanEW > 0 || Coordinate.AccuracySpanNS > 0)
                {
                    if (!DiversityWorkbench.GeoFunctions.AccuracyFromNumericRange(ref Coordinate))
                        ConversionSuccessful = false;
                }
                else
                {
                    Coordinate.AccuracyText = DiversityWorkbench.GeoFunctions.AccuracyFromDegMinSec(Coordinate.DegreeNS, Coordinate.MinutesNS, Coordinate.SecondsNS, Coordinate.DegreeEW, Coordinate.MinutesEW, Coordinate.SecondsEW);
                }
            }
            return true;
        }
        
        /// <summary>
        /// Approximation in parts derived from Wieczorek, J. 2001. MaNIS/HerpNet/ORNIS Georeferencing Guidelines. 
        /// University of California, Berkeley: Museum of Vertebrate Zoology.
        /// rounded to values between 0 and 30° Latitude
        /// </summary>
        /// <param name="DegNS"></param>
        /// <param name="MinNS"></param>
        /// <param name="SecNS"></param>
        /// <param name="DegEW"></param>
        /// <param name="MinEW"></param>
        /// <param name="SecEW"></param>
        /// <returns></returns>
        public static string AccuracyFromDegMinSec(int DegNS, int MinNS, double SecNS, int DegEW, int MinEW, double SecEW)
        {
            // int AMax = 111120;
            if (SecNS % 0.5 > 0 || SecEW % 0.5 > 0) return "1 m";       // Milliseconds
            if (SecNS % 1 > 0 || SecEW % 1 > 0) return "3 m";       // Milliseconds
            if (SecNS % 5 > 0 || SecEW % 5 > 0) return "40 m";      // +-  1 Second
            if (SecNS % 10 > 0 || SecEW % 10 > 0) return "200 m";     // +-  5 Seconds
            if (SecNS % 30 > 0 || SecEW % 30 > 0) return "400 m";     // +- 10 Seconds
            if (SecNS > 0 || SecEW > 0) return "1.2 km";    // +- 30 Seconds
            if (MinNS % 5 > 0 || MinEW % 5 > 0) return "2.5 km";    // +-  1 Minute
            if (MinNS % 10 > 0 || MinEW % 10 > 0) return "12 km";   // +-  5 Minutes
            if (MinNS % 30 > 0 || MinEW % 30 > 0) return "25 km";   // +- 10 Minutes
            if (MinNS > 0 || MinEW > 0) return "75 km";   // +- 30 Minutes
            if (DegNS % 5 > 0 || DegEW % 5 > 0) return "150 km";  // +-  1 Grad
            return "500000 m";
        }

        /// <summary>
        /// Calulation of the accuracy within a GeoCoordinate
        /// </summary>
        /// <param name="GeoCoordinate">The geocoordate</param>
        /// <returns>The text containing the unit</returns>
        public static string AccuracyFromDegMinSec(ref GeoCoordinate GeoCoordinate)
        {
            if (GeoCoordinate.NumericNS != null
                && GeoCoordinate.NumericNS != 0
                && (GeoCoordinate.DegreeNS == null || GeoCoordinate.DegreeNS == 0))
            {
                if (GeoCoordinate.NumericNS % 0.00005 > 0) GeoCoordinate.Accuracy = 2;       // +-0,00001 Degrees
                else if (GeoCoordinate.NumericNS % 0.0001 > 0) GeoCoordinate.Accuracy = 3;       // +- 0,00005 Degrees
                else if (GeoCoordinate.NumericNS % 0.0005 > 0) GeoCoordinate.Accuracy = 1;       // +- 0,0001 Degrees
                else if (GeoCoordinate.NumericNS % 0.001 > 0) GeoCoordinate.Accuracy = 30;       // +- 0,0005 Degrees
                else if (GeoCoordinate.NumericNS % 0.005 > 0) GeoCoordinate.Accuracy = 150;       // +- 0,001 Degrees
                else if (GeoCoordinate.NumericNS % 0.01 > 0) GeoCoordinate.Accuracy = 300;       // +- 0,005 Degrees
                else if (GeoCoordinate.NumericNS % 0.05 > 0) GeoCoordinate.Accuracy = 1500;       // +- 0,01 Degrees
                else if (GeoCoordinate.NumericNS % 0.1 > 0) GeoCoordinate.Accuracy = 3000;       // +- 0,05 Degrees
                else if (GeoCoordinate.NumericNS % 0.5 > 0) GeoCoordinate.Accuracy = 15000;       // +- 0.1 Degrees
                else if (GeoCoordinate.NumericNS % 1 > 0) GeoCoordinate.Accuracy = 30000;       // +- 0,5 Degrees
                else if (GeoCoordinate.NumericNS % 5 > 0) GeoCoordinate.Accuracy = 150000;       // +- 1 Degree
                else if (GeoCoordinate.NumericNS % 10 > 0) GeoCoordinate.Accuracy = 300000;       // +- 5 Degrees
                else GeoCoordinate.Accuracy = 111120;

                //Prefix
                //if (GeoCoordinate.Value < 0) GeoCoordinate.Sign = -1;
                //else GeoCoordinate.Sign = 1;
                // Deg
                GeoCoordinate.DegreeNS = (int)GeoCoordinate.NumericNS * Math.Sign(GeoCoordinate.NumericNS);
                // Min
                double MinD = GeoCoordinate.NumericNS * Math.Sign(GeoCoordinate.NumericNS) - GeoCoordinate.DegreeNS;
                MinD = MinD * 0.6;
                MinD = ((MinD * 100));
                GeoCoordinate.MinutesNS = (int)MinD;
                // Sec
                double SecD = MinD - GeoCoordinate.MinutesNS;
                SecD *= 60;
                decimal dSec = (decimal)SecD;
                dSec = Math.Round(dSec, 9);
                SecD = (double)dSec;
                if (SecD >= 60)
                {
                    SecD -= 60;
                    GeoCoordinate.MinutesNS += 1;
                }
                decimal dSecD = (decimal)SecD;
                dSecD = System.Math.Round(dSecD, 9);
                SecD = (double)dSecD;
                GeoCoordinate.SecondsNS = SecD;
                GeoCoordinate.DegreeNS = System.Math.Abs(GeoCoordinate.DegreeNS);
            }
            else
            {
                if (GeoCoordinate.SecondsNS % 0.5 > 0) GeoCoordinate.Accuracy = 1;       // Milliseconds
                else if (GeoCoordinate.SecondsNS % 1 > 0) GeoCoordinate.Accuracy = 3;       // Milliseconds
                else if (GeoCoordinate.SecondsNS % 5 > 0) GeoCoordinate.Accuracy = 40;      // +-  1 Second
                else if (GeoCoordinate.SecondsNS % 10 > 0) GeoCoordinate.Accuracy = 200;     // +-  5 Seconds
                else if (GeoCoordinate.SecondsNS % 30 > 0) GeoCoordinate.Accuracy = 400;     // +- 10 Seconds
                else if (GeoCoordinate.SecondsNS > 0) GeoCoordinate.Accuracy = 1200;    // +- 30 Seconds
                else if (GeoCoordinate.MinutesNS % 5 > 0) GeoCoordinate.Accuracy = 2500;    // +-  1 Minute
                else if (GeoCoordinate.MinutesNS % 10 > 0) GeoCoordinate.Accuracy = 12000;   // +-  5 Minutes
                else if (GeoCoordinate.MinutesNS % 30 > 0) GeoCoordinate.Accuracy = 25000;   // +- 10 Minutes
                else if (GeoCoordinate.MinutesNS > 0) GeoCoordinate.Accuracy = 75000;   // +- 30 Minutes
                else if (GeoCoordinate.DegreeNS % 5 > 0) GeoCoordinate.Accuracy = 150000;  // +-  1 Grad
                else GeoCoordinate.Accuracy = 500000;
            }
            if (GeoCoordinate.Accuracy > 0)
            {
                if (GeoCoordinate.Accuracy < 1000)
                    GeoCoordinate.AccuracyText = GeoCoordinate.Accuracy.ToString() + " m";
                else
                    GeoCoordinate.AccuracyText = GeoCoordinate.AccuracyText = (GeoCoordinate.Accuracy / 1000).ToString() + " km";
                return GeoCoordinate.AccuracyText;
            }
            else
                return "500000 m";
        }

        /// <summary>
        /// Approximation in parts derived from Wieczorek, J. 2001. MaNIS/HerpNet/ORNIS Georeferencing Guidelines. 
        /// University of California, Berkeley: Museum of Vertebrate Zoology.
        /// rounded to values between 0 and 30° Latitude
        /// </summary>
        /// <param name="Deg"></param>
        /// <param name="Min"></param>
        /// <param name="Sec"></param>
        /// <returns></returns>
        public static string AccuracyFromDegMinSec(int Deg, int Min, double Sec)
        {
            // int AMax = 111120;
            if (Sec % 0.5 > 0 ) return "1 m";       // Milliseconds
            if (Sec % 1 > 0) return "3 m";       // Milliseconds
            if (Sec % 5 > 0) return "40 m";      // +-  1 Second
            if (Sec % 10 > 0) return "200 m";     // +-  5 Seconds
            if (Sec % 30 > 0) return "400 m";     // +- 10 Seconds
            if (Sec > 0) return "1.2 km";    // +- 30 Seconds
            if (Min % 5 > 0) return "2.5 km";    // +-  1 Minute
            if (Min % 10 > 0) return "12 km";   // +-  5 Minutes
            if (Min % 30 > 0) return "25 km";   // +- 10 Minutes
            if (Min > 0) return "75 km";   // +- 30 Minutes
            if (Deg % 5 > 0) return "150 km";  // +-  1 Grad
            return "500000 m";
        }


        /// <summary>
        /// Approximation derived from Wieczorek, J. 2001. MaNIS/HerpNet/ORNIS Georeferencing Guidelines. 
        /// University of California, Berkeley: Museum of Vertebrate Zoology.
        /// rounded to values between 0 and 30° Latitude
        /// </summary>
        /// <param name="DegNS"></param>
        /// <param name="DegEW"></param>
        /// <returns></returns>
        public static string AccuracyFromDegNumeric(double DegNS, double DegEW)
        {
            if (DegNS % 0.00005 > 0 || DegEW % 0.00005 > 0) return "2 m";       // +-0,00001 Degrees
            if (DegNS % 0.0001 > 0 || DegEW % 0.0001 > 0) return "3 m";       // +- 0,00005 Degrees
            if (DegNS % 0.0005 > 0 || DegEW % 0.0005 > 0) return "15 m";       // +- 0,0001 Degrees
            if (DegNS % 0.001 > 0 || DegEW % 0.001 > 0) return "30 m";       // +- 0,0005 Degrees
            if (DegNS % 0.005 > 0 || DegEW % 0.005 > 0) return "150 m";       // +- 0,001 Degrees
            if (DegNS % 0.01 > 0 || DegEW % 0.01 > 0) return "300 m";       // +- 0,005 Degrees
            if (DegNS % 0.05 > 0 || DegEW % 0.05 > 0) return "1500 m";       // +- 0,01 Degrees
            if (DegNS % 0.1 > 0 || DegEW % 0.1 > 0) return "3 km";       // +- 0,05 Degrees
            if (DegNS % 0.5 > 0 || DegEW % 0.5 > 0) return "15 km";       // +- 0.1 Degrees
            if (DegNS % 1 > 0 || DegEW % 1 > 0) return "30 km";       // +- 0,5 Degrees
            if (DegNS % 5 > 0 || DegEW % 5 > 0) return "150 km";       // +- 1 Degree
            if (DegNS % 10 > 0 || DegEW % 10 > 0) return "300 km";       // +- 5 Degrees
            return "111120 m";
        }

        public static void AccuracyFromNumeric(double DegNS, double DegEW, ref double Accuracy, ref string AccuracyText)
        {
            if (DegNS % 0.00005 > 0 || DegEW % 0.00005 > 0) Accuracy = 2;       // +-0,00001 Degrees
            else if (DegNS % 0.0001 > 0 || DegEW % 0.0001 > 0) Accuracy = 3;       // +- 0,00005 Degrees
            else if (DegNS % 0.0005 > 0 || DegEW % 0.0005 > 0) Accuracy = 15;       // +- 0,0001 Degrees
            else if (DegNS % 0.001 > 0 || DegEW % 0.001 > 0) Accuracy = 30;       // +- 0,0005 Degrees
            else if (DegNS % 0.005 > 0 || DegEW % 0.005 > 0) Accuracy = 150;       // +- 0,001 Degrees
            else if (DegNS % 0.01 > 0 || DegEW % 0.01 > 0) Accuracy = 300;       // +- 0,005 Degrees
            else if (DegNS % 0.05 > 0 || DegEW % 0.05 > 0) Accuracy = 1500;       // +- 0,01 Degrees
            else if (DegNS % 0.1 > 0 || DegEW % 0.1 > 0) Accuracy = 3000;       // +- 0,05 Degrees
            else if (DegNS % 0.5 > 0 || DegEW % 0.5 > 0) Accuracy = 15000;       // +- 0.1 Degrees
            else if (DegNS % 1 > 0 || DegEW % 1 > 0) Accuracy = 30000;       // +- 0,5 Degrees
            else if (DegNS % 5 > 0 || DegEW % 5 > 0) Accuracy = 150000;       // +- 1 Degree
            else if (DegNS % 10 > 0 || DegEW % 10 > 0) Accuracy =  300000;       // +- 5 Degrees
            else Accuracy = 111120;
            if (Accuracy > 0)
            {
                if (Accuracy < 1000)
                    AccuracyText = Accuracy.ToString() + " m";
                else
                    AccuracyText = AccuracyText = (Accuracy / 1000).ToString() + " km";
            }
        }

        public static bool AccuracyFromNumericRange(ref GeoCoordinate GeoCoordinate)
        {
            try
            {
                string SQL = "DECLARE @g1 geography; " +
                    "SET @g1 = geography::STPointFromText('POINT(" + GeoCoordinate.NumericNS.ToString() + " " + GeoCoordinate.NumericEW.ToString() + ")', 4326); " +
                    "DECLARE @g2 geography; " +
                    "SET @g2 = geography::STPointFromText('POINT(" + (GeoCoordinate.NumericNS + GeoCoordinate.AccuracySpanNS).ToString() + " " + (GeoCoordinate.NumericEW + GeoCoordinate.AccuracySpanEW).ToString() + ")', 4326); " +
                    "SELECT @g1.STDistance(@g2);";
                double Accuracy = 0;
                if (double.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out Accuracy))
                {
                    GeoCoordinate.Accuracy = Accuracy;
                    if (Accuracy > 1000)
                        GeoCoordinate.AccuracyText = System.Math.Round((Accuracy / 1000), 1).ToString() + " km";
                    else GeoCoordinate.AccuracyText = Accuracy.ToString() + " m";
                    return true;
                }
                else return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Approximation derived from Wieczorek, J. 2001. MaNIS/HerpNet/ORNIS Georeferencing Guidelines. 
        /// University of California, Berkeley: Museum of Vertebrate Zoology.
        /// rounded to values between 0 and 30° Latitude
        /// </summary>
        /// <param name="Deg">the value of the coordinate</param>
        /// <returns></returns>
        public static string AccuracyFromDegNumeric(double Deg)
        {
            if (Deg % 0.00005 > 0) return "2 m";       // +-0,00001 Degrees
            if (Deg % 0.0001 > 0) return "3 m";       // +- 0,00005 Degrees
            if (Deg % 0.0005 > 0) return "15 m";       // +- 0,0001 Degrees
            if (Deg % 0.001 > 0) return "30 m";       // +- 0,0005 Degrees
            if (Deg % 0.005 > 0) return "150 m";       // +- 0,001 Degrees
            if (Deg % 0.01 > 0) return "300 m";       // +- 0,005 Degrees
            if (Deg % 0.05 > 0) return "1500 m";       // +- 0,01 Degrees
            if (Deg % 0.1 > 0) return "3 km";       // +- 0,05 Degrees
            if (Deg % 0.5 > 0) return "15 km";       // +- 0.1 Degrees
            if (Deg % 1 > 0) return "30 km";       // +- 0,5 Degrees
            if (Deg % 5 > 0) return "150 km";       // +- 1 Degree
            if (Deg % 10 > 0) return "300 km";       // +- 5 Degrees
            return "111120 m";
        }

        public static int AccuracyFromDegree(int From, int To)
        {
            if ((From % 2 > 0 && From % 5 > 0) || (To % 2 > 0 && To % 5 > 0)) return 1;       // +-1 Degree
            if (From % 5 > 0 || To % 5 > 0) return 3;       // +-3 Degrees
            if (From % 15 > 0 || To % 15 > 0) return 5;       // +-5 Degrees
            if (From % 30 > 0 || To % 30 > 0) return 15;       // +-15 Degrees
            if (From % 45 > 0 || To % 45 > 0) return 30;       // +-30 Degrees
            if (From % 90 > 0 || To % 90 > 0) return 45;       // +-45 Degrees
            return 45;
        }

        public static string AccuracyFromOrientation(string From, string To)
        {
            if (From.ToString().Length > 2 || To.ToString().Length > 2)
                return "23°";
            if (From.ToString().Length > 1 || To.ToString().Length > 1)
                return "45°";
            else return "90°";
        }

        /// <summary>
        /// Calculation of the uncertainity in parts according to Wieczorek, J., Q. Guo, and R. Hijmans. 2004. 
        /// The point-radius method for georeferencing locality descriptions and calculating associated uncertainty. 
        /// International Journal of Geographical Information Science. 18: 745-767.
        /// </summary>
        /// <param name="Value1">The first value of a range</param>
        /// <param name="Value2">The first value of a range</param>
        /// <param name="Unit">The unit as text, e.g. m, km</param>
        /// <returns></returns>
        public static decimal AccuracyFromValues(decimal Value1, decimal Value2)
        {
            decimal d;
            Value1 = Value1 * System.Math.Sign(Value1);
            Value2 = Value2 * System.Math.Sign(Value2);
            for (d = (decimal)0.00001; d < (decimal)100000000; d *= (decimal)10)
            {
                if ((Value1 * 2) % d > 0)
                {
                    if (Value1 == d / 10) d *= (decimal)0.25;
                    else d *= (decimal)0.5;
                    break;
                }
                if ((Value2 * 2) % d > 0)
                {
                    if (Value2 == d / 10) d *= (decimal)0.25;
                    else d *= (decimal)0.5;
                    break;
                }
                if (Value1 % d > 0)
                {
                    if (d < Value1) d *= (decimal)2.5;
                    else d *= (decimal)0.5;
                    break;
                }
                if (Value2 % d > 0)
                {
                    if (d < Value2) d *= (decimal)2.5;
                    else d *= (decimal)0.5;
                    break;
                }
                //d *= i;
            }
            d *= (decimal)0.1;
            d = System.Math.Round(d, 9);
            double dD = (double)d;
            d = (decimal)dD;
            return d;
        }

        /// <summary>
        /// Calculation of the uncertainity in parts according to Wieczorek, J., Q. Guo, and R. Hijmans. 2004. 
        /// The point-radius method for georeferencing locality descriptions and calculating associated uncertainty. 
        /// International Journal of Geographical Information Science. 18: 745-767.
        /// </summary>
        /// <param name="Value1">The first value of a range</param>
        /// <param name="Value2">The first value of a range</param>
        /// <param name="Unit">The unit as text, e.g. m, km</param>
        /// <returns></returns>
        public static double AccuracyFromValues(double Value1, double Value2)
        {
            double d = (double)DiversityWorkbench.GeoFunctions.AccuracyFromValues((decimal)Value1, (decimal)Value2);
            return d;
        }

        #endregion


        /// <summary>
        /// Helmert Transformation von UTM-Koordinaten mit Potsdam Elysoid nach WGS84
        /// siehe http://de.wikipedia.org/wiki/7-Parameter-Transformation
        /// funktioniert leider nicht ?!
        /// </summary>
        /// <param name="GeoPointPotsdam">Point with Latitutde, Longitute, Altitude</param>
        /// <returns>Point with Latitutde, Longitute, Altitude</returns>
        //public static GeoPoint ConvertPostdamToWGS84(GeoPoint GeoPointPotsdam)
        //{
        //    GeoPoint GeoPointWGS84 = new GeoPoint();
        //    double cx = 598.1;
        //    double cy = 73.7;
        //    double cz = 418.2;
        //    double m = 6.7;
        //    double rx = -0.202;
        //    double ry = -0.045;
        //    double rz = 2.455;

        //    try
        //    {
        //        // X = cx + (1 + m) * X - rz * Y + ry * Z
        //        GeoPointWGS84.Lonigitude = cx + (1 + m) * GeoPointPotsdam.Lonigitude - rz * GeoPointPotsdam.Latitude + rz * GeoPointPotsdam.Altitude;

        //        // Y = cy + rz * X + (1 + m) * Y - rx * Z
        //        GeoPointWGS84.Latitude = cy + rz * GeoPointPotsdam.Lonigitude + (1 + m) * GeoPointPotsdam.Latitude - rz * GeoPointPotsdam.Altitude;

        //        // Z = cz - ry * X + rx * Y + (1 + m) * Z
        //        if (GeoPointPotsdam.Altitude != 0)
        //            GeoPointWGS84.Altitude = cz - ry * GeoPointPotsdam.Lonigitude + rx * GeoPointPotsdam.Latitude + (1 + m) * GeoPointPotsdam.Altitude;
        //        else GeoPointWGS84.Altitude = 0; 

        //    }
        //    catch { }

        //    return GeoPointWGS84;
        //}
    }


}
