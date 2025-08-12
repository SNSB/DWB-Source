using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Web;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlGoogleMaps : UserControl
    {
        #region Parameter
        private System.Uri _URI;
        private float _Latitude = 400f;
        private float _Longitude = 400f;
        private float _LatitudeAccuracy = 0f;
        private float _LongitudeAccuracy = 0f;
        private float _UpperLatitude = 400;
        private float _LowerLatitude = 400;
        private float _LeftLongitude = 400;
        private float _RightLongitude = 400;

        // Toni 20230408: Field to store received data from Google maps
        private string _LastReceivedData = "";

        #endregion

        #region Construction
        public UserControlGoogleMaps()
        {
            InitializeComponent();
        }

        #endregion

        #region Public functions

        public void setUrlForPolygon(string URL)
        {
            if (URL.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("No coordinates found");
                //this.webViewCompatible.Navigate("about:blank");
                this.webBrowser.Url = new Uri("about:blank");
                this.userControlWebView.Url = new Uri("about:blank");
                return;
            }
            if (URL.Length > 2000)
            {
                System.Windows.Forms.MessageBox.Show("Too many items, please reduce selection");
                //this.webViewCompatible.Navigate("about:blank");
                this.userControlWebView.Url = new Uri("about:blank");
                this.webBrowser.Url = new Uri("about:blank");
                return;
            }
            if (URL.Length > 0)
            {
                try
                {
                    // URL = System.Web.HttpUtility.UrlEncode(URL);
                    if (!URL.StartsWith("http://"))
                    {
                        string URL_base = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
                        if (string.IsNullOrEmpty(URL_base))
                            return;
                        URL = URL_base + "?" + URL;
                    }
                    //URL += "&Height=" + (this.Height).ToString() + "&Width=" + (this.Width).ToString();
                    this._URI = new Uri(URL);
                    //this.webBrowser.ScrollBarsEnabled = false;
                    this.userControlWebView.Url = this._URI;
                    this.webBrowser.Url = this._URI;
                    //this.webViewCompatible.Navigate(this._URI);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    //this.webViewCompatible.Navigate("about:blank");
                    this.userControlWebView.Url = new Uri("about:blank");
                    this.webBrowser.Url = new Uri("about:blank");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Could not find coordinates to generate a map");
                //this.webViewCompatible.Navigate("about:blank");
                this.userControlWebView.Url = new Uri("about:blank");
                this.webBrowser.Url = new Uri("about:blank");
            }
        }

        public void setUrlForPolygon(string Coordinates, string CoordinatesParent, string CoordinatesPoints, string LabelForPoints)
        {
            if (Coordinates.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("No coordinates found");
                return;
            }

            if (Coordinates.Length + CoordinatesParent.Length > 2000)
            {
                //System.Collections.Generic.List<DiversityWorkbench.GeoPoint> Points = new List<GeoPoint>();

                //string[] LatLon = Coordinates.Split(new char[] { '=' });
                //string[] CooLatList = LatLon[1].Substring(0, LatLon[1].IndexOf(")&Lng")-1).Split(new char[] { '|' });
                //string[] CooLongList = LatLon[2].Substring(1).Split(new char[] { '|' });
                //double dLat;
                //double dLong;
                //for (int i = 0; i < CooLongList.Length; i++)
                //{

                //    if (double.TryParse(CooLongList[i].Replace('.', ','), out dLong) &&
                //        double.TryParse(CooLatList[i].Replace('.', ','), out dLat))
                //    {
                //        DiversityWorkbench.GeoPoint GP = new GeoPoint();
                //        GP.Latitude = System.Math.Round(dLat, 2);
                //        GP.Lonigitude = System.Math.Round(dLong, 2);
                //        if (i == 0 || Points.Count == 0) Points.Add(GP);
                //        else
                //        {
                //            if (GP.Lonigitude != Points[Points.Count - 1].Lonigitude ||
                //                GP.Latitude != Points[Points.Count - 1].Latitude)
                //            {
                //                Points.Add(GP);
                //            }
                //        }
                //    }
                //}
                //string LatNew = "Lat=";
                //string LongNew = "&Lng=";
                //foreach (DiversityWorkbench.GeoPoint P in Points)
                //{
                //    LatNew += P.Latitude + "|";
                //    LongNew += P.Lonigitude + "|";
                //}
                //Coordinates = LatNew.Substring(0, LatNew.Length - 1).Replace(',', '.') + LongNew.Substring(0, LongNew.Length - 1).Replace(',', '.');
                if (Coordinates.Length + CoordinatesParent.Length > 2000)
                {
                    System.Windows.Forms.MessageBox.Show("Too many items, please reduce selection");
                    this._URI = new Uri("about:blank");
                    //this.webBrowser.ScrollBarsEnabled = false;
                    this.userControlWebView.Url = this._URI;
                    this.webBrowser.Url = this._URI;
                    //this.webViewCompatible.Navigate(this._URI);

                    return;
                }
            }
            string CoordinatePointsFromCoordinates = Coordinates;
            string LabelForPointsFormCoordinates = "";
            string DataInCoordinates = Coordinates.Replace("Lng", "");
            DataInCoordinates = DataInCoordinates.Replace("Lat", "");
            DataInCoordinates = DataInCoordinates.Replace("&", "");
            DataInCoordinates = DataInCoordinates.Replace("=", "");
            if (DataInCoordinates.Length == 0) Coordinates = "";
            //else Coordinates = System.Web.HttpUtility.UrlEncode(Coordinates);

            string DataInPoints = CoordinatesPoints.Replace("&LatPoint", "");
            DataInPoints = DataInPoints.Replace("&LngPoint", "");
            DataInPoints = DataInPoints.Replace("=", "");
            if (DataInPoints.Length == 0 && CoordinatePointsFromCoordinates.IndexOf('|') > -1) CoordinatesPoints = "";
            else if (DataInPoints.Length == 0 && CoordinatePointsFromCoordinates.IndexOf('|') == -1)
            {
                CoordinatesPoints = CoordinatePointsFromCoordinates.Replace("Lat", "&LatPoint");
                CoordinatesPoints = CoordinatesPoints.Replace("Lng", "LngPoint");
                LabelForPointsFormCoordinates = CoordinatePointsFromCoordinates.Replace("&", "-");
                LabelForPointsFormCoordinates = LabelForPointsFormCoordinates.Replace("=", ":");
                CoordinatesPoints = System.Web.HttpUtility.UrlEncode(CoordinatesPoints);
            }
            //else CoordinatesPoints = System.Web.HttpUtility.UrlEncode(CoordinatesPoints);

            string DataInParent = CoordinatesParent.Replace("&LatPar", "");
            DataInParent = DataInParent.Replace("&LngPar", "");
            DataInParent = DataInParent.Replace("=", "");
            if (DataInParent.Length == 0) CoordinatesParent = "";
            //else CoordinatesParent = System.Web.HttpUtility.UrlEncode(CoordinatesParent);

            string DataInLabel = LabelForPoints.Replace("&Label", "");
            DataInLabel = DataInLabel.Replace("=", "");
            if (DataInLabel.Length == 0) LabelForPoints = "";
            if (LabelForPoints.Length == 0 && LabelForPointsFormCoordinates.Length > 0)
                LabelForPoints = "&Label=" + LabelForPointsFormCoordinates;
            //if (LabelForPoints.Length > 0) 
            //    LabelForPoints = System.Web.HttpUtility.UrlEncode(LabelForPoints);

            if (Coordinates.Length > 0 || (DataInCoordinates.Length > 0 || DataInPoints.Length > 0))
            {
                try
                {
                    string URL_base = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
                    if (string.IsNullOrEmpty(URL_base))
                        return;
                    string URL = URL_base + "?" + Coordinates + CoordinatesParent + CoordinatesPoints + LabelForPoints;
                    this._URI = new Uri(URL);
                    //this.webBrowser.ScrollBarsEnabled = false;
                    this.userControlWebView.Visible = true;
                    this.webBrowser.Visible = false;
                    this.userControlWebView.Url = null;
                    this.userControlWebView.Navigate(this._URI);
                    this.userControlWebView.AllowScripting = true;
                    this.userControlWebView.Dock = DockStyle.Fill;
                    //this.webBrowser.Url = this._URI;
                    //this.webViewCompatible.Navigate(this._URI);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Could not find coordinates to generate a map");
                this._URI = new Uri("about:blank");
                this.userControlWebView.Url = this._URI;
                this.webBrowser.Url = this._URI;
                //this.webViewCompatible.Navigate(this._URI);
            }
        }

        public void setUrlForCoordinateRetrieval(float Latitude, float Longitude)
        {
            this._Latitude = Latitude;
            this._Longitude = Longitude;
            string URL_base = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
            if (string.IsNullOrEmpty(URL_base))
                return;
            string URI = URL_base;
            if (this._Latitude != 0 || this._Longitude != 0)
                URI += "?Lat=" + this._Latitude.ToString().Replace(",", ".") + "&Lng=" + this._Longitude.ToString().Replace(",", ".");
            else
                URI += "?Lat=48.16385096&Lng=11.50062829";
            //URI += "&SizeX=" + this.webBrowser.Width.ToString() + "&SizeY=" + this.webBrowser.Height.ToString();
            URI += "&Cross=1";
            System.Uri u = new Uri(URI);
            this.userControlWebView.Url = u;
            this.webBrowser.Url = u;
            //this.webViewCompatible.Navigate(this._URI);
        }


        private string LastCoordinatesFromClipboard
        {
            get
            {
                string tmp = string.Empty;

                // Toni 20230408: Read last received data; JSON string included in extra quotes...
                tmp = _LastReceivedData.Trim('"');
                if (tmp.Length == 0)
                {
                    try
                    {
                        // Request data from map
                        this.userControlWebView.CallJavaScript("window.setClipboardData();");
                        System.Windows.Forms.MessageBox.Show("Map coordinates have not been received.\r\nRetry last action or move the map.\r\n\r\n" +
                            "If this message appears again, the Google API might have changed.\r\nBugfix will be provided in upcoming version", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                //try
                //{
                //    System.Windows.Forms.IDataObject myData = System.Windows.Forms.Clipboard.GetDataObject();
                //    tmp = (string)myData.GetData(typeof(string));
                //    // Workaround for Windows7/8 problem from July 2013: Clipboard now is cleared after reading!
                //    System.Windows.Forms.Clipboard.SetDataObject(tmp, true);
                //}
                //catch (System.Exception ex)
                //{
                //}
                return tmp;
            }
        }



        public void ReadValuesFromCurrentPosition()
        {
            try
            {
                CultureInfo InvC = new CultureInfo("");

                //System.Uri Uri = this.webBrowser.Url;
                //this.webBrowser.Url = Uri;
                string s = this.LastCoordinatesFromClipboard;// System.Windows.Forms.Clipboard.GetText();
                //System.Windows.Forms.Clipboard.SetText(s);
                if (s.Length > 0)
                {
                    string sLowLat = s.Substring(2, s.IndexOf(" ") - 3).Trim();
                    string sLeftLon = s.Substring(s.IndexOf(" "), s.IndexOf(")") - s.IndexOf(" ")).Trim();
                    s = s.Substring(s.IndexOf("), (") + 4).Trim();
                    string sUpLat = s.Substring(0, s.IndexOf(" ") - 1).Trim();
                    string sRightLon = s.Substring(s.IndexOf(" "), s.IndexOf(")") - s.IndexOf(" ")).Trim();

                    if (!float.TryParse(sLowLat, NumberStyles.Float, InvC, out this._LowerLatitude))
                    {
                        if (!float.TryParse(sLowLat.Replace(",", "."), NumberStyles.Float, InvC, out this._LowerLatitude))
                            this._LowerLatitude = 0.0f;
                    }
                    if (!float.TryParse(sLeftLon, NumberStyles.Float, InvC, out this._LeftLongitude))
                    {
                        if (!float.TryParse(sLeftLon.Replace(",", "."), NumberStyles.Float, InvC, out this._LeftLongitude))
                            this._LeftLongitude = 0.0f;
                    }
                    if (!float.TryParse(sUpLat, NumberStyles.Float, InvC, out this._UpperLatitude))
                    {
                        if (!float.TryParse(sUpLat.Replace(",", "."), NumberStyles.Float, InvC, out this._UpperLatitude))
                            this._UpperLatitude = 0.0f;
                    }
                    if (!float.TryParse(sRightLon, NumberStyles.Float, InvC, out this._RightLongitude))
                    {
                        if (!float.TryParse(sRightLon.Replace(",", "."), NumberStyles.Float, InvC, out this._RightLongitude))
                            this._RightLongitude = 0.0f;
                    }

                    this._Longitude = (this._LeftLongitude + this._RightLongitude) / 2;
                    this._Latitude = (this._LowerLatitude + this._UpperLatitude) / 2;
                    this._LongitudeAccuracy = System.Math.Abs((this._LeftLongitude - this._RightLongitude) / (float)this.webBrowser.Width);
                    this._LatitudeAccuracy = System.Math.Abs((this._LowerLatitude - this._UpperLatitude) / (float)this.webBrowser.Height);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public double Longitude
        {
            get
            {
                if (this._Longitude > 180 || this._Longitude < -180)
                    this.ReadValuesFromCurrentPosition();
                return (double)((float)this._Longitude);
            }
        }

        public double Latitude
        {
            get
            {
                if (this._Latitude > 180 || this._Latitude < -180)
                    this.ReadValuesFromCurrentPosition();
                return (double)((float)this._Latitude);
            }
        }

        public double LatitudeAccuracy
        {
            get
            {
                if (this._LatitudeAccuracy == 0)
                    this.ReadValuesFromCurrentPosition();
                return (double)((float)this._LatitudeAccuracy);
            }
        }

        public double LongitudeAccuracy
        {
            get
            {
                if (this._LongitudeAccuracy == 0)
                    this.ReadValuesFromCurrentPosition();
                return (double)((float)this._LongitudeAccuracy);
            }
        }

        public double Accuracy
        {
            get
            {
                if (this._LatitudeAccuracy == 0)
                    this.ReadValuesFromCurrentPosition();
                double A = this._LatitudeAccuracy * 111120 * 2;
                if (A < 1)
                    A = Math.Round(A, 1);
                else
                    A = Math.Round(A, 0);
                return A;
            }
        }

        public void ClearMap()
        {
            this._URI = new Uri("about:blank");
            this.webBrowser.Url = this._URI;
            //this.webViewCompatible.Navigate(this._URI);
        }

        private void userControlWebView_MessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                // Toni 20230408: Store data received from Google maps in buffer
                _LastReceivedData = e.WebMessageAsJson;
            }
            catch (Exception)
            { }
        }

        #endregion
    }
}
