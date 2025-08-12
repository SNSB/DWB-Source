using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Web;

namespace DiversityWorkbench.Forms
{
    public partial class FormTK25 : Form
    {

        #region Parameter

        private float _Longitude;
        private float _Latitude;
        private string _TK25 = "";
        private string _TK25_Quadrant = "";
        private string _TK25_Name = "";
        private float _AverageLongitude;
        private float _AverageLatitude;
        private string _Geography;
        private string _ConnectionString;
        private DiversityWorkbench.IWorkbenchUnit _IWorkbenchUnit;
        private DiversityWorkbench.ServerConnection _ServerConnection;
        private string _LinkedServer = "";

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                if (this._ServerConnection == null && this._ConnectionString != null && this._ConnectionString.Length > 0)
                    this._ServerConnection = new ServerConnection(this._ConnectionString);
                return _ServerConnection;
            }
            set { _ServerConnection = value; }
        }

        #endregion

        #region Construction

        public FormTK25(string TK25, string Quadrant, string ConnectionString)
        {
            InitializeComponent();
            if (TK25 == "TK25 (MTB / Quadrant)")
                TK25 = "";
            if (!this.ParamertersAreValid(TK25, Quadrant))
            {
                this.Close();
                return;
            }
            this._ConnectionString = ConnectionString;
            this.TK25 = TK25;
            if (!this.initDatabase())
            {
                System.Windows.Forms.MessageBox.Show("TK " + TK25 + " has not been found in any of the available databases");
                this.Close();
                return;
            }
            this.TK25_Quadrant = Quadrant;
            this.setTK25DisplayText();
            if (this.getCoordinates())
                this.showGeographyMap();
        }

        //public FormTK25(string TK25, string Quadrant, float Longitude, float Latitude, string ConnectionString)
        //{
        //    InitializeComponent();
        //    if (!this.ParamertersAreValid(TK25, Quadrant))
        //    {
        //        this.Close();
        //        return;
        //    }
        //    this._ConnectionString = ConnectionString;
        //    this.TK25 = TK25;
        //    if (!this.initDatabase())
        //    {
        //        System.Windows.Forms.MessageBox.Show("TK " + TK25 + " has not been found in any of the available databases");
        //        this.Close();
        //        return;
        //    }
        //    this.TK25_Quadrant = Quadrant;
        //    this._Latitude = Latitude;
        //    this._Longitude = Longitude;
        //    this.setTK25DisplayText();
        //    if (this.getCoordinates())
        //        this.showGeographyMap();
        //}

        public FormTK25(float Longitude, float Latitude, string ConnectionString)
        {
            InitializeComponent();
            this._ConnectionString = ConnectionString;
            this._Latitude = Latitude;
            this._Longitude = Longitude;
            this.initDatabase();
            if (this.comboBoxDatabase.Items.Count > 0)
                this.comboBoxDatabase.SelectedIndex = 0;
            this.showGeographyMap();
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Validity of parameters

        private bool ParamertersAreValid(string TK25, string Quadrant)
        {
            if (TK25.Length > 0 && !this.TK25isValid(TK25))
            {
                System.Windows.Forms.MessageBox.Show(TK25 + " is not a valid value for TK25");
                return false;
            }
            if (!this.QuadrantIsValid(Quadrant))
            {
                System.Windows.Forms.MessageBox.Show(Quadrant + " is not a valid value for Quadrant");
                return false;
            }
            return true;
        }

        private bool TK25isValid(string TK25)
        {
            int iTK;
            if (int.TryParse(TK25, out iTK) && iTK > 999 && iTK < 10000)
                return true;
            else
                return false;
        }

        private bool QuadrantIsValid(string Quadrant)
        {
            bool OK = true;
            int i;
            foreach (char C in Quadrant)
            {
                if (int.TryParse(C.ToString(), out i))
                {
                    if (i < 1 || i > 4)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            return OK;
        }

        #endregion

        #region Database

        private static string _Prefix;
        private static string Prefix()
        {
            return _Prefix;
        }

        private bool initDatabase()
        {
            if (this._ConnectionString != null && this._ConnectionString.Length > 0)
            {
                DiversityWorkbench.ServerConnection SC = new ServerConnection(this._ConnectionString);
                DiversityWorkbench.Gazetteer G = new Gazetteer(SC);                //G.DatabaseList();
                string Service = G.ServiceName();

                this.comboBoxDatabase.Items.Clear();
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList())
                {
                    if (!this.comboBoxDatabase.Items.Contains(KVconn.Value.DisplayText))
                    {
                        bool OK = false;
                        string SQL = "";
                        if (this.TK25.Length > 0)
                        {
                            SQL = "SELECT COUNT(*) FROM " + KVconn.Value.Prefix() + "ViewTK25 WHERE TK25 = '" + this.TK25 + "'";
                            string Error = "";
                            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                            if (Error.Length == 0 && Result != "0" && Result.Length > 0)
                                OK = true;
                        }
                        else
                        {
                            SQL = "SELECT COUNT(*) FROM " + KVconn.Value.Prefix() + "ViewTK25";
                            string Result = "";
                            Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            int ii = 0;
                            if (int.TryParse(Result, out ii) && ii > 0)
                                OK = true;
                        }
                        if (OK)
                        {
                            this.comboBoxDatabase.Items.Add(KVconn.Value.DisplayText);
                            if (KVconn.Value.ConnectionString == this._ConnectionString)
                                this.comboBoxDatabase.SelectedIndex = i;
                            i++;
                        }
                    }
                }
                if (this.comboBoxDatabase.SelectedIndex == -1)
                {
                    this._ConnectionString = G.ServerConnection.ConnectionString;
                }
                else
                {
                    System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> DD = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList();
                    this.ServerConnection = DD[this.comboBoxDatabase.SelectedItem.ToString()];
                    _Prefix = this.ServerConnection.Prefix();
                }
            }
            if (this.comboBoxDatabase.Items.Count > 0)
                return true;
            else return false;
        }

        private void comboBoxDatabase_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.ServerConnection> DD = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].ServerConnectionList();
                this.ServerConnection = DD[this.comboBoxDatabase.SelectedItem.ToString()];
                _Prefix = this.ServerConnection.Prefix();
                this.showGeographyMap();
            }
            catch (System.Exception ex) { }

        }

        #endregion

        #region TK25

        private void setTK25DisplayText()
        {
            this.labelCurrentPosition.Text = this.TK25;
            if (this.TK25_Quadrant.Length > 0)
                this.labelCurrentPosition.Text += " / " + this.TK25_Quadrant;
        }

        public string TK25
        {
            get { return _TK25; }
            set
            {
                _TK25 = value;
                this.setTK25DisplayText();
            }
        }

        public string TK25_Name
        {
            get { return _TK25_Name; }
        }

        #endregion

        #region Quadrant

        public string TK25_Quadrant
        {
            get { return _TK25_Quadrant; }
            set
            {
                _TK25_Quadrant = value;
                this.QuadrantLevel = this._TK25_Quadrant.Length;
                this.setTK25DisplayText();
            }
        }

        private int QuadrantLevel
        {
            get
            {
                int i = 1;
                switch (this.comboBoxQuadrant.Text)
                {
                    case "1":
                        i = 0;
                        break;
                    case "1/4":
                        i = 1;
                        break;
                    case "1/16":
                        i = 2;
                        break;
                    case "1/64":
                        i = 3;
                        break;
                    case "1/256":
                        i = 4;
                        break;
                    case "1/1024":
                        i = 5;
                        break;
                }
                return i;
            }
            set
            {
                this.comboBoxQuadrant.SelectedIndex = value;
            }
        }

        private void comboBoxQuadrant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._Longitude == 0 && this._Latitude == 0)
            {
                string s = this.LastCoordinatesFromClipboard;// System.Windows.Forms.Clipboard.GetText();
                //System.Windows.Forms.Clipboard.SetText(s);
            }
            this.showGeographyMap();
        }

        private void comboBoxQuadrant_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._TK25_Quadrant = null;
        }

        #endregion

        #region Geography

        private bool getCoordinates()
        {
            if (DiversityWorkbench.Forms.FormTK25.GetGeographyOfTK25(this._TK25, this._TK25_Quadrant, this._ConnectionString, ref this._Geography, ref this._Latitude, ref this._Longitude))
            {
                this._AverageLongitude = this._Longitude;
                this._AverageLatitude = this._Latitude;
                return true;
            }
            else return false;
        }

        private bool showGeographyMap()
        {
            bool OK = true;
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            string Lat = this._Latitude.ToString().ToString(InvC).Replace(",", ".");
            string Lon = this._Longitude.ToString().ToString(InvC).Replace(",", ".");
            string LatForMap = "Lat=";
            string LonForMap = "&Lng=";
            string LatParForMap = "&LatPar=";
            string LonParForMap = "&LngPar=";
            string LatDotForMap = "&LatPoint=" + this._Latitude.ToString(InvC).Replace(",", ".");// +"|" + this._Latitude.ToString(InvC).Replace(",", ".");
            string LonDotForMap = "&LngPoint=" + this._Longitude.ToString(InvC).Replace(",", ".");// +"|" + this._Longitude.ToString(InvC).Replace(",", ".");
            string LabelForPoints = "&Label=";
            string URI = "";

            // Adaption to linked servers
            string SQL = "SELECT TOP 1 PlaceID, SuperiorPlaceID, Polygon, TK25, TK25_Name, Point1, Point2, Point3, Point4, Lat1, Lon1, Lat2, Lon2, Lat3, Lon3, Lat4, Lon4 " +
                "FROM " + Prefix() + "ViewTK25 AS T " +
                "WHERE (" + Lat + " BETWEEN Lat2 AND Lat1) AND (" + Lon + " BETWEEN Lon1 AND Lon4) ";
            if (Lat == "0" && Lon == "0")
            {
                SQL = "SELECT TOP 1 PlaceID, SuperiorPlaceID, Polygon, TK25, TK25_Name, Point1, Point2, Point3, Point4, Lat1, Lon1, Lat2, Lon2, Lat3, Lon3, Lat4, Lon4 " +
                "FROM " + Prefix() + "ViewTK25 AS T ";
                if (TK25.Length > 0)
                    SQL += "WHERE TK25 = '" + TK25 + "' ";
            }

            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                ad.Fill(dt);

                // the outline of the plot
                if (dt.Rows.Count > 0)
                {
                    // the outline of the Quadrant
                    //DiversityWorkbench.SamplingPlot S = new SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                    System.Drawing.RectangleF Quadrant = new RectangleF(); //= S.TK25Quadrant(this._Longitude, this._Latitude, this.QuadrantLevel);
                    //System.Drawing.RectangleF TK25
                    int Depth = this.QuadrantLevel;
                    if (this._TK25_Quadrant == null)
                    {
                        this._TK25_Quadrant = Gazetteer.QuadrantForTK25(this._Longitude, this._Latitude,
                            dt.Rows[0]["Point1"].ToString(),
                            dt.Rows[0]["Point2"].ToString(),
                            dt.Rows[0]["Point3"].ToString(),
                            dt.Rows[0]["Point4"].ToString(),
                            ref Depth,
                            ref Quadrant);
                    }
                    else
                    {
                        Gazetteer.QuadrantForTK25(this._Longitude, this._Latitude,
                            dt.Rows[0]["Point1"].ToString(),
                            dt.Rows[0]["Point2"].ToString(),
                            dt.Rows[0]["Point3"].ToString(),
                            dt.Rows[0]["Point4"].ToString(),
                            ref Depth,
                            ref Quadrant);
                    }
                    LabelForPoints += dt.Rows[0]["TK25"].ToString();
                    if (this._TK25_Quadrant.Length > 0)
                        LabelForPoints += "/" + this._TK25_Quadrant;
                    LabelForPoints += " - " + dt.Rows[0]["TK25_Name"].ToString();
                    this.TK25 = dt.Rows[0]["TK25"].ToString();
                    this._TK25_Name = dt.Rows[0]["TK25_Name"].ToString();
                    if (Quadrant.Y == 0.0 && Quadrant.X == 0.0 && Quadrant.Height == 0.0 && Quadrant.Width == 0.0)
                    {
                        float Lat1;
                        float Lat2;
                        if (float.TryParse(dt.Rows[0]["Lat1"].ToString(), out Lat1) && float.TryParse(dt.Rows[0]["Lat2"].ToString(), out Lat2))
                            this._AverageLatitude = (Lat1 + Lat2) / 2;
                        float Lon1;
                        float Lon2;
                        if (float.TryParse(dt.Rows[0]["Lon1"].ToString(), out Lon1) && float.TryParse(dt.Rows[0]["Lon2"].ToString(), out Lon2))
                            this._AverageLongitude = (Lon1 + Lon2) / 2;
                    }
                    else
                    {
                        this._AverageLatitude = Quadrant.Y + Quadrant.Height / 2;
                        this._AverageLongitude = Quadrant.X + Quadrant.Width / 2;
                    }
                    if (!dt.Rows[0]["Polygon"].Equals(System.DBNull.Value) && this._TK25_Quadrant.Length == 0)
                        this._Geography = dt.Rows[0]["Polygon"].ToString();
                    else
                    {
                        this._Geography = "POLYGON ((" +
                            Quadrant.X.ToString(InvC).Replace(",", ".") + " " + (Quadrant.Y + Quadrant.Height).ToString().ToString(InvC).Replace(",", ".") + ", " +
                            Quadrant.X.ToString(InvC).Replace(",", ".") + " " + Quadrant.Y.ToString(InvC).Replace(",", ".") + ", " +
                            (Quadrant.X + Quadrant.Width).ToString(InvC).Replace(",", ".") + " " + Quadrant.Y.ToString(InvC).Replace(",", ".") + ", " +
                            (Quadrant.X + Quadrant.Width).ToString(InvC).Replace(",", ".") + " " + (Quadrant.Y + Quadrant.Height).ToString().ToString(InvC).Replace(",", ".") + ", " +
                            Quadrant.X.ToString(InvC).Replace(",", ".") + " " + (Quadrant.Y + Quadrant.Height).ToString().ToString(InvC).Replace(",", ".") + "))";
                    }

                    LatForMap += Quadrant.Y.ToString(InvC).Replace(",", ".") + "|";
                    LatForMap += Quadrant.Y.ToString(InvC).Replace(",", ".") + "|";
                    LatForMap += (Quadrant.Y + Quadrant.Height).ToString().ToString(InvC).Replace(",", ".") + "|";
                    LatForMap += (Quadrant.Y + Quadrant.Height).ToString().ToString(InvC).Replace(",", ".") + "|";
                    LatForMap += Quadrant.Y.ToString(InvC).Replace(",", ".") + "|";

                    LonForMap += Quadrant.X.ToString(InvC).Replace(",", ".") + "|";
                    LonForMap += (Quadrant.X + Quadrant.Width).ToString(InvC).Replace(",", ".") + "|";
                    LonForMap += (Quadrant.X + Quadrant.Width).ToString(InvC).Replace(",", ".") + "|";
                    LonForMap += Quadrant.X.ToString(InvC).Replace(",", ".") + "|";
                    LonForMap += Quadrant.X.ToString(InvC).Replace(",", ".") + "|";

                    LatParForMap += dt.Rows[0]["Lat1"].ToString().ToString(InvC).Replace(",", ".") + "|";
                    LonParForMap += dt.Rows[0]["Lon1"].ToString().ToString(InvC).Replace(",", ".") + "|";
                    LatParForMap += dt.Rows[0]["Lat2"].ToString().ToString(InvC).Replace(",", ".") + "|";
                    LonParForMap += dt.Rows[0]["Lon2"].ToString().ToString(InvC).Replace(",", ".") + "|";
                    LatParForMap += dt.Rows[0]["Lat3"].ToString().ToString(InvC).Replace(",", ".") + "|";
                    LonParForMap += dt.Rows[0]["Lon3"].ToString().ToString(InvC).Replace(",", ".") + "|";
                    LatParForMap += dt.Rows[0]["Lat4"].ToString().ToString(InvC).Replace(",", ".") + "|";
                    LonParForMap += dt.Rows[0]["Lon4"].ToString().ToString(InvC).Replace(",", ".") + "|";

                    URI = LatForMap.Substring(0, LatForMap.Length - 1) + LonForMap.Substring(0, LonForMap.Length - 1);
                    string Parent = LatParForMap.Substring(0, LatParForMap.Length - 1) + LonParForMap.Substring(0, LonParForMap.Length - 1);
                    string Points = LatDotForMap + LonDotForMap;
                    this.userControlGoogleMaps.setUrlForPolygon(URI, Parent, Points, LabelForPoints);
                }
                else if (this._Latitude != 0 && this._Longitude != null)
                {
                    System.Windows.Forms.MessageBox.Show("Coordinates of location missing or outside the available range of TK25 maps");
                    this.userControlGoogleMaps.setUrlForCoordinateRetrieval(this._Latitude, this._Longitude);
                    OK = false;
                }
                else if (Lat == "0" && Lon == "0" && TK25.Length > 0)
                {
                    System.Windows.Forms.MessageBox.Show("TK " + TK25 + " is not available in the database");
                    OK = false;
                }
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string Message = ex.Message;
                if (Message.IndexOf("@Point") > -1 || Message.StartsWith("Invalid") || Message.IndexOf("ViewTK25") > -1)
                    Message += "\r\n\r\n You may not be connected to the correct database";
                System.Windows.Forms.MessageBox.Show(Message);
                OK = false;
                //if (this._Latitude > 0 && this._Longitude > 0)
                //    this.Close();
            }
            catch
            {
                string URL_base = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
                if (string.IsNullOrEmpty(URL_base))
                    return OK = false;
                URI = URL_base + "?Lat=48.16385096&Lng=11.50062829&Cross=1";
                this.userControlGoogleMaps.setUrlForPolygon(URI);
                OK = false;
            }
            return OK;
        }

        // ATTENTION: Workaround for Windows7 problem from July 2013: Clipboard seems to be flushed frequently after reading!!
        private string _LastCoordinatesFromClipboard = "";

        private string LastCoordinatesFromClipboard
        {
            get
            {
                string tmp = string.Empty;
                try
                {
                    System.Windows.Forms.IDataObject myData = System.Windows.Forms.Clipboard.GetDataObject();
                    tmp = (string)myData.GetData(typeof(string));
                    // Workaround for Windows7/8 problem from July 2013: Clipboard now is cleared after reading!
                    System.Windows.Forms.Clipboard.SetDataObject(tmp, true);
                }
                catch (System.Exception ex)
                {
                }
                return tmp;
            }
        }

        public float AverageLatitude
        {
            get { return _AverageLatitude; }
            //set { _AverageLatitude = value; }
        }

        public float AverageLongitude
        {
            get { return _AverageLongitude; }
            //set { _AverageLongitude = value; }
        }

        public string Geography
        {
            get
            {
                string SQL = "";
                return this._Geography;
            }
        }

        private void buttonRequery_Click(object sender, EventArgs e)
        {
            // Reset Quadrant
            this._TK25_Quadrant = null;

            this.userControlGoogleMaps.ReadValuesFromCurrentPosition();
            if (this.userControlGoogleMaps.Latitude >= -360 && this.userControlGoogleMaps.Latitude <= 360)
                this._Latitude = (float)this.userControlGoogleMaps.Latitude;
            if (this.userControlGoogleMaps.Longitude >= -90 && this.userControlGoogleMaps.Longitude <= 90)
                this._Longitude = (float)this.userControlGoogleMaps.Longitude;
            this.userControlDialogPanel.buttonOK.Enabled = this.showGeographyMap();
        }

        public static bool GetGeographyOfTK25(string TK25, string Quadrant, string ConnectionString, ref string Geography, ref float Latitude, ref float Longitude)
        {
            bool OK = true;
            try
            {
                string SQL = "SELECT TOP 1 P.PlaceID, P.SuperiorPlaceID , P.Polygon , SUBSTRING(N.Name, 1, 4) AS TK25 , N.Name AS TK25_Name " +
                    ", P.Point1, P.Point2, P.Point3, P.Point4, P.Lat1, P.Lon1, P.Lat2, P.Lon2, P.Lat3, P.Lon3, P.Lat4, P.Lon4   " +
                    "FROM " + Prefix() + "ViewTK25 AS P  " +
                    "INNER JOIN " + Prefix() + "GeoName AS N ON P.PlaceID = N.PlaceID WHERE (N.Name LIKE '" + TK25 + " - %')";

                System.Data.DataTable dt = new DataTable();
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
            return OK;
        }

        public static void ResetTK25()
        {
            _DtTK25 = null;
            _DtTK25quadrant = null;
        }
        private static System.Data.DataTable _DtTK25;
        private static System.Data.DataTable _DtTK25quadrant;

        public static bool GetGeographyOfTK25(string TK25, string Quadrant, DiversityWorkbench.ServerConnection Connection, ref string Geography, ref float Latitude, ref float Longitude)
        {
            bool OK = true;
            string Prefix = Connection.LinkedServer;
            if (Prefix.Length > 0)
            {
                Prefix = "[" + Prefix + "]";
                string DB = Connection.DatabaseName;
                Prefix += "." + DB + ".dbo.";
            }
            try
            {
                string SQL = "";
                string Message = "";
                if (Quadrant.Length == 0)
                {
                    if (_DtTK25 == null || _DtTK25.Rows.Count == 0)
                    {
                        _DtTK25 = new DataTable();
                        SQL = "SELECT Substring(N.Name, 1, 4) AS TK25, P.[Geography], P.[Latitude], P.[Longitude]  " +
                            "FROM " + Prefix + "ViewGeoPlace AS P INNER JOIN " +
                            "" + Prefix + "GeoName AS N ON P.PlaceID = N.PlaceID " +
                            "WHERE (P.PlaceType = N'TK25') ";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtTK25, ref Message);
                    }
                    System.Data.DataRow[] RR = _DtTK25.Select("TK25 = '" + TK25 + "'", "");
                    if (RR.Length > 0)
                    {
                        Geography = RR[0]["Geography"].ToString();
                        OK = float.TryParse(RR[0]["Latitude"].ToString(), out Latitude);
                        OK = float.TryParse(RR[0]["Longitude"].ToString(), out Longitude);
                        return OK;
                    }
                }
                else if (Quadrant.Length == 1)
                {
                    if (_DtTK25quadrant == null || _DtTK25quadrant.Rows.Count == 0)
                    {
                        _DtTK25quadrant = new DataTable();
                        SQL = "SELECT Substring(N.Name, 1, 4) AS TK25, Substring(N.Name, 6, 1) AS Quadrant, P.[Geography], P.[Latitude], P.[Longitude] " +
                            "FROM " + Prefix + "ViewGeoPlace AS P INNER JOIN " +
                            "" + Prefix + "GeoName AS N ON P.PlaceID = N.PlaceID " +
                            "WHERE (P.PlaceType = N'TK25Quad') ";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtTK25quadrant, ref Message);
                    }
                    System.Data.DataRow[] RR = _DtTK25quadrant.Select("TK25 = '" + TK25 + "' AND Quadrant = '" + Quadrant + "'", "");
                    if (RR.Length > 0)
                    {
                        Geography = RR[0]["Geography"].ToString();
                        OK = float.TryParse(RR[0]["Latitude"].ToString(), out Latitude);
                        OK = float.TryParse(RR[0]["Longitude"].ToString(), out Longitude);
                        return OK;
                    }
                    else
                    {
                        SQL = "SELECT Substring(N.Name, 1, 4) AS TK25, Substring(N.Name, 6, 1) AS Quadrant, P.[Geography], P.[Latitude], P.[Longitude] " +
                            "FROM " + Prefix + "ViewGeoPlace AS P INNER JOIN " +
                            "" + Prefix + "GeoName AS N ON P.PlaceID = N.PlaceID " +
                            "WHERE (P.PlaceType = N'TK25Quad') AND Substring(N.Name, 1, 4) = '" + TK25 + "' AND Substring(N.Name, 6, 1) = '" + Quadrant + "'";
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtTK25quadrant, ref Message);
                        System.Data.DataRow[] rr = _DtTK25quadrant.Select("TK25 = '" + TK25 + "' AND Quadrant = '" + Quadrant + "'", "");
                        if (rr.Length > 0)
                        {
                            Geography = rr[0]["Geography"].ToString();
                            OK = float.TryParse(RR[0]["Latitude"].ToString(), out Latitude);
                            OK = float.TryParse(RR[0]["Longitude"].ToString(), out Longitude);
                            return OK;
                        }
                        else
                            OK = false;
                    }
                }
                else
                {
                    SQL = "SELECT TOP 1 P.PlaceID    " +
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
                    "FROM " + Prefix + "GeoPlace AS P INNER JOIN " +
                    "" + Prefix + "GeoName AS N ON P.PlaceID = N.PlaceID " +
                    "WHERE (N.Name LIKE '" + TK25 + " - %') AND (P.PlaceType = N'TK25') ";
                    if (Connection.LinkedServer.Length > 0)
                    {
                        SQL = "SELECT TOP 1 P.PlaceID    " +
                            ", P.SuperiorPlaceID " +
                            ", P.Polygon " +
                            ", SUBSTRING(N.Name, 1, 4) AS TK25 " +
                            ", N.Name AS TK25_Name" +
                            ", P.Point1 " +
                            ", P.Point2 " +
                            ", P.Point3 " +
                            ", P.Point4 " +
                            ", P.Lat1 " +
                            ", P.Lon1  " +
                            ", P.Lat2  " +
                            ", P.Lon2  " +
                            ", P.Lat3  " +
                            ", P.Lon3  " +
                            ", P.Lat4  " +
                            ", P.Lon4  " +
                            "FROM " + Prefix + "ViewTK25 AS P INNER JOIN " +
                            "" + Prefix + "GeoName AS N ON P.PlaceID = N.PlaceID " +
                            "WHERE (N.Name LIKE '" + TK25 + " - %') ";
                    }
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, Connection.ConnectionString);
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
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
            return OK;
        }

        #endregion

    }
}
