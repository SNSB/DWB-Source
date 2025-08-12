using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Geography
{
    public class Geography
    {
        private System.Collections.Generic.List<GeoConversion.GeodeticPosition> _geoPoints;
        public enum GeometryType { POINT, MULTIPOINT, LINESTRING, POLYGON };
        private GeometryType _GeometryType = GeometryType.POINT;
        public enum DisplayType { numeric, degree };
        private DisplayType _DisplayType = DisplayType.numeric;
        private string _GeographyAsString;
        private bool _IncludeAltitude = false;

        public bool IncludeAltitude { get => _IncludeAltitude; set => _IncludeAltitude = value; }


        //private int _DecimalPlacesLatitude = 6;
        //private int _DecimalPlacesLongtude = 6;

        public Geography(string geography)
        {
            try
            {
                if (geography.Length > 0)
                {
                    string Type = geography.Substring(0, geography.IndexOf('(')).Trim().ToUpper();
                    switch (Type)
                    {
                        case "POINT":
                            this._GeometryType = GeometryType.POINT;
                            string XYZ = geography.Substring(geography.IndexOf('(') + 1, geography.IndexOf(')') - geography.IndexOf('(') - 1);
                            GeoConversion.GeodeticPosition geoPoint = this.geoPoint(XYZ);
                            this._geoPoints = new List<GeoConversion.GeodeticPosition>();
                            this._geoPoints.Add(geoPoint);
                            break;
                        case "MULTIPOINT":
                            this._GeometryType = GeometryType.MULTIPOINT;
                            string multipoint = geography.Substring(geography.IndexOf("((") + 2);
                            this._geoPoints = new List<GeoConversion.GeodeticPosition>();
                            while (multipoint.Length > 0)
                            {
                                string point = "";
                                if (multipoint.IndexOf(')') > -1)
                                    point = multipoint.Substring(0, multipoint.IndexOf(')'));
                                else if (multipoint.IndexOf("))") > -1)
                                {
                                    point = multipoint.Substring(0, multipoint.IndexOf("))"));
                                    multipoint = "";
                                }
                                GeoConversion.GeodeticPosition geoPolyPoint = this.geoPoint(point);
                                this._geoPoints.Add(geoPolyPoint);
                                if (multipoint.Length > 0)
                                    multipoint = multipoint.Substring(point.Length);
                                if (multipoint.StartsWith("), ("))
                                    multipoint = multipoint.Substring(4).Trim();
                                if (multipoint == "))")
                                    multipoint = "";
                            }
                            break;
                        case "LINESTRING":
                            this._GeometryType = GeometryType.LINESTRING;
                            string line = geography.Substring(geography.IndexOf("(") + 1);
                            this._geoPoints = new List<GeoConversion.GeodeticPosition>();
                            while (line.Length > 0)
                            {
                                string point = "";
                                if (line.IndexOf(',') > -1)
                                    point = line.Substring(0, line.IndexOf(','));
                                else if (line.IndexOf(')') > -1)
                                {
                                    point = line.Substring(0, line.IndexOf(')'));
                                    line = "";
                                }
                                GeoConversion.GeodeticPosition geoPolyPoint = this.geoPoint(point);
                                this._geoPoints.Add(geoPolyPoint);
                                if (line.Length > 0)
                                    line = line.Substring(point.Length);
                                if (line.StartsWith(","))
                                    line = line.Substring(1).Trim();
                            }
                            break;
                        case "POLYGON":
                            this._GeometryType = GeometryType.POLYGON;
                            string area = geography.Substring(geography.IndexOf("((") + 2);
                            this._geoPoints = new List<GeoConversion.GeodeticPosition>();
                            while (area.Length > 0)
                            {
                                string point = "";
                                if (area.IndexOf(',') > -1)
                                    point = area.Substring(0, area.IndexOf(','));
                                else if (area.IndexOf(')') > -1)
                                {
                                    point = area.Substring(0, area.IndexOf(')'));
                                    area = "";
                                }
                                GeoConversion.GeodeticPosition geoPolyPoint = this.geoPoint(point);
                                this._geoPoints.Add(geoPolyPoint);
                                if (area.Length > 0)
                                    area = area.Substring(point.Length);
                                if (area.StartsWith(","))
                                    area = area.Substring(1).Trim();
                            }
                            break;
                    }
                }
                //else
                //{
                //    this._GeometryType = GeometryType.POINT;
                //}
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public string GeographyAsString()
        {
            if (_GeographyAsString != null && _GeographyAsString.Length > 0)
                return _GeographyAsString;
            string Geo = "";
            try
            {
                foreach (GeoConversion.GeodeticPosition Point in _geoPoints)
                {
                    if (Geo.Length > 0)
                        Geo += ", ";
                    string P = Point.lon.ToString() + " " + Point.lat.ToString();
                    //string P = this.RoundValue(Point.lon).ToString() + " " + this.RoundValue(Point.lat).ToString();
                    if (IncludeAltitude)
                        P += " " + Point.h.ToString();
                    switch (this._GeometryType)
                    {
                        case DiversityWorkbench.Geography.Geography.GeometryType.MULTIPOINT:
                            Geo += "(" + P + ")";
                            break;
                        default:
                            Geo += P;
                            break;
                    }
                    if (this._GeometryType == GeometryType.POINT)
                        break;
                }
                if (this._GeometryType == GeometryType.POLYGON && (this._geoPoints.First().lat != this._geoPoints.Last().lat || this._geoPoints.First().lon != this._geoPoints.Last().lon))
                {
                    Geo += ", " + this._geoPoints.First().lon.ToString() + " " + this._geoPoints.First().lat.ToString();
                    if (IncludeAltitude)
                        Geo += " " + this._geoPoints.First().h.ToString();
                }
                switch (this._GeometryType)
                {
                    case DiversityWorkbench.Geography.Geography.GeometryType.POINT:
                        Geo = "POINT (" + Geo + ")";
                        break;
                    case DiversityWorkbench.Geography.Geography.GeometryType.MULTIPOINT:
                        Geo = "MULTIPOINT (" + Geo + ")";
                        break;
                    case DiversityWorkbench.Geography.Geography.GeometryType.LINESTRING:
                        Geo = "LINESTRING (" + Geo + ")";
                        break;
                    case DiversityWorkbench.Geography.Geography.GeometryType.POLYGON:
                        Geo = "POLYGON ((" + Geo + "))";
                        break;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            _GeographyAsString = Geo;
            return _GeographyAsString;
        }


        public bool SaveGeography(string Table, string Column, string WhereClause)
        {
            bool OK = true;
            try
            {
                string SQL = "UPDATE T SET " + Column + " = geography::STGeomFromText('" + this.GeographyAsString() + "', 4326) ";
                switch (Table.ToLower())
                {
                    case "collectioneventlocalisation":
                        SQL += ", AverageLatitudeCache = geography::STGeomFromText('" + this.GeographyAsString() + "', 4326).EnvelopeCenter().Lat" +
                            ", AverageLongitudeCache = geography::STGeomFromText('" + this.GeographyAsString() + "', 4326).EnvelopeCenter().Long" + 
                            ", Location2 = geography::STGeomFromText('" + this.GeographyAsString() + "', 4326).EnvelopeCenter().Lat" +
                            ", Location1 = geography::STGeomFromText('" + this.GeographyAsString() + "', 4326).EnvelopeCenter().Long";
                        break;
                    default:
                        //SQL += "', 4326) ";
                        break;
                }
                SQL += " FROM " + Table + " AS T WHERE " + WhereClause;
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        public bool WriteGeoPoints(System.Collections.Generic.List<GeoConversion.GeodeticPosition> Points)
        {
            bool OK = true;
            try
            {
                if (this._geoPoints == null)
                    this._geoPoints = new List<GeoConversion.GeodeticPosition>();
                this._geoPoints.Clear();
                foreach (GeoConversion.GeodeticPosition Point in Points)
                    this._geoPoints.Add(Point);
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        public void setTypeOfDisplay(string Type)
        {
            if (Type.ToLower() == "numeric")
                this._DisplayType = DisplayType.numeric;
            else
                this._DisplayType = DisplayType.degree;
        }

        public DisplayType TypeOfDisplay
        {
            get { return this._DisplayType; }
            set { this._DisplayType = value; }
        }

        public void setTypeOfGeometry(string Type)
        {
            if (this.TypeOfGeometry.ToString().ToLower() != Type.ToLower())
                _GeographyAsString = "";
            switch (Type)
            {
                case "POINT":
                    this.TypeOfGeometry = DiversityWorkbench.Geography.Geography.GeometryType.POINT;
                    break;
                case "MULTIPOINT":
                    this.TypeOfGeometry = DiversityWorkbench.Geography.Geography.GeometryType.MULTIPOINT;
                    break;
                case "LINESTRING":
                    this.TypeOfGeometry = DiversityWorkbench.Geography.Geography.GeometryType.LINESTRING;
                    break;
                case "POLYGON":
                    this.TypeOfGeometry = DiversityWorkbench.Geography.Geography.GeometryType.POLYGON;
                    break;
            }
        }

        public GeometryType TypeOfGeometry
        {
            get { return this._GeometryType; }
            set
            {
                if (this._GeometryType != value)
                    _GeographyAsString = "";
                this._GeometryType = value;
            }
        }

        private GeoConversion.GeodeticPosition geoPoint(string Content)
        {
            string[] xyz = Content.Split(new char[] { ' ' });
            GeoConversion.GeodeticPosition geoPoint = new GeoConversion.GeodeticPosition(0, 0, 0);
            double _long;
            if (double.TryParse(xyz[0], out _long))
            {
                geoPoint.lon = _long;// this.RoundValue(_long);
                double _lat;
                if (double.TryParse(xyz[1], out _lat))
                {
                    geoPoint.lat = _lat;// this.RoundValue(_lat);
                    double _alt;
                    if (xyz.Length > 2 && double.TryParse(xyz[2], out _alt) && this.IncludeAltitude)
                        geoPoint.h = _alt;
                }
            }
            return geoPoint;
        }

        public double RoundValue(double Value, int DecialPlaces = 6)
        {
            double D = Value % 1;
            int DecPlace = D.ToString().Length - 2;
            if (DecPlace > DecialPlaces)
                Value = System.Math.Round(Value, DecialPlaces);
            return Value;
        }

        public System.Collections.Generic.List<GeoConversion.GeodeticPosition> getGeoPoints()
        {
            if (this._geoPoints == null)
            {
                this._geoPoints = new List<GeoConversion.GeodeticPosition>();
                GeoConversion.GeodeticPosition position = new GeoConversion.GeodeticPosition(0, 0, 0);
                this._geoPoints.Add(position);
            }
            return this._geoPoints;
        }
    }
}
