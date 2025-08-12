using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Web.WebView2.Core;
using System.Security.Policy;
using System.Web;

namespace DiversityWorkbench.Forms
{
    public partial class FormGoogleMapsCoordinates : Form
    {

        #region Parameter

        private double _Latitude = 400;
        private double _Longitude = 400;
        private double _UpperLatitude = 400;
        private double _LowerLatitude = 400;
        private double _LeftLongitude = 400;
        private double _RightLongitude = 400;
        private double _LatitudeAccuracy = 0;
        private double _LongitudeAccuracy = 0;
        private System.Drawing.Point _Point;

        //Microsoft.Toolkit.Forms.UI.Controls.WebView _webView;

        #endregion

        #region Construction

        public FormGoogleMapsCoordinates()
        {
            InitializeComponent();
            this.initForm();
        }

        public FormGoogleMapsCoordinates(double Latitude, double Longitude)
        {
            InitializeComponent();
            this._Latitude = Latitude;
            this._Longitude = Longitude;
            if (Latitude != 0 && Longitude != 0)
            {
                this._Point = new Point((int)(Latitude * 100), (int)(Longitude * 100));
                this.buttonOK.BackColor = System.Drawing.Color.Pink;
                this.buttonOK.Width = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(110);
                this.buttonOK.TextAlign = ContentAlignment.MiddleLeft;
                this.buttonOK.Text = "     Set coordinates";
            }
            else
            {
                this.buttonOK.Image = null;
                this.buttonOK.Width = 60;
            }
            this.initForm();
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                this.SetURI();
                this._Longitude = 400;
                this._Latitude = 400;
                //this.userControlDialogPanel.buttonOK.Click += new System.EventHandler(this.GetCoordinatesFromClipboard);
                this.splitContainerBrowser.Panel1Collapsed = false;
                this.splitContainerBrowser.Panel2Collapsed = true;
                //#if DEBUG
                //#else
                //                this.splitContainerBrowser.Panel2Collapsed = true;
                //                this.splitContainerBrowser.Panel1Collapsed = false;
                //#endif
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Properties

        // Change with WebView2 control: Data are 
        private string _LastCoordinatesFromMap = "";


        private string LastCoordinatesFromMap
        {
            get
            {
                string tmp = string.Empty;
                try
                {
                    tmp = _LastCoordinatesFromMap;
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
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return tmp;
            }
        }

        private bool _CoordinatesCouldBeParsedFromClipboardContent = false;
        private string _LastCoordinates = "";

        private bool setValues()
        {
            if (this._CoordinatesCouldBeParsedFromClipboardContent && this.LastCoordinatesFromMap == _LastCoordinates)
                return true;
            bool OK = true;
            string s = "";
            string Message = "";
            try
            {
                CultureInfo InvC = new CultureInfo("");

                int i = 0;
                s = this.LastCoordinatesFromMap;
                _LastCoordinates = s;
                if (s != null && s.Length > 0)
                {
                    string sLowLat = s.Substring(3, s.IndexOf(" ") - 4).Trim();
                    string sLeftLon = s.Substring(s.IndexOf(" "), s.IndexOf(")") - s.IndexOf(" ")).Trim();
                    s = s.Substring(s.IndexOf("), (") + 4).Trim();
                    string sUpLat = s.Substring(0, s.IndexOf(" ") - 1).Trim();
                    string sRightLon = s.Substring(s.IndexOf(" "), s.IndexOf(")") - s.IndexOf(" ")).Trim();

                    if (!double.TryParse(sLowLat, NumberStyles.Float, InvC, out this._LowerLatitude))
                    {
                        if (!double.TryParse(sLowLat.Replace(",", "."), NumberStyles.Float, InvC, out this._LowerLatitude))
                        {
                            this._LowerLatitude = 0.0;
                            OK = false;
                        }
                    }
                    if (!double.TryParse(sLeftLon, NumberStyles.Float, InvC, out this._LeftLongitude))
                    {
                        if (!double.TryParse(sLeftLon.Replace(",", "."), NumberStyles.Float, InvC, out this._LeftLongitude))
                        {
                            this._LeftLongitude = 0.0;
                            OK = false;
                        }
                    }
                    if (!double.TryParse(sUpLat, NumberStyles.Float, InvC, out this._UpperLatitude))
                    {
                        if (!double.TryParse(sUpLat.Replace(",", "."), NumberStyles.Float, InvC, out this._UpperLatitude))
                        {
                            this._UpperLatitude = 0.0;
                            OK = false;
                        }
                    }
                    if (!double.TryParse(sRightLon, NumberStyles.Float, InvC, out this._RightLongitude))
                    {
                        if (!double.TryParse(sRightLon.Replace(",", "."), NumberStyles.Float, InvC, out this._RightLongitude))
                        {
                            this._RightLongitude = 0.0;
                            OK = false;
                        }
                    }

                    if (System.Math.Sign(this._LeftLongitude) != System.Math.Sign(this._RightLongitude))
                    {
                        double LngL;// = 180 - this._LeftLongitude;
                        double LngR;// = this._RightLongitude + 180;
                        double LngM;// = (LngL + LngR) / 2;
                        if (this._LeftLongitude > 180 || this._RightLongitude > 180 || this._LeftLongitude < -180 || this._RightLongitude < -180)
                        {
                            LngL = 180 - this._LeftLongitude;
                            LngR = this._RightLongitude + 180;
                            LngM = (LngL + LngR) / 2;
                            if (System.Math.Abs(this._LeftLongitude) > System.Math.Abs(this._RightLongitude))
                                LngM = LngM * System.Math.Sign(this._RightLongitude) + this._RightLongitude;
                            else
                                LngM = LngM * System.Math.Sign(this._LeftLongitude) + this._LeftLongitude;
                        }
                        else
                        {
                            LngL = this._LeftLongitude;
                            LngR = this._RightLongitude;
                            LngM = (LngL + LngR) / 2;
                        }
                        this._Longitude = LngM;
                    }
                    else
                        this._Longitude = (this._LeftLongitude + this._RightLongitude) / 2;
                    this._LongitudeAccuracy = System.Math.Abs((this._LeftLongitude - this._RightLongitude) / (double)this.userControlWebView.Width);

                    this._Latitude = (this._LowerLatitude + this._UpperLatitude) / 2;
                    this._LatitudeAccuracy = System.Math.Abs((this._LowerLatitude - this._UpperLatitude) / (double)this.userControlWebView.Height);
                }
                else
                {
                    OK = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("FormGoogleMapsCoordinates", "setValues", "Retrieval of clipboard failed");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, s);
                OK = false;
                Message = ex.Message + "\r\n";
            }
            if (!OK)
            {
                if (s != null)
                {
                    Message += "Another process interfered with the retrieval of the coordinates.\r\n";
                    if (s != null && s.Length > 0)
                        Message += "The clipboard contained the value\r\n\r\n" + s + "\r\n";
                    Message += "\r\nPlease move the map to restart the retrieval of the coordinates";
                    System.Windows.Forms.MessageBox.Show(Message, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this._CoordinatesCouldBeParsedFromClipboardContent = OK;
            return OK;
        }

        public double Longitude
        {
            get
            {
                //if (this._Longitude > 180 || this._Longitude < -180)
                this.setValues();
                return (double)((float)this._Longitude);
            }
        }

        public double Latitude
        {
            get
            {
                //if (this._Latitude > 180 || this._Latitude < -180)
                this.setValues();
                return (double)((float)this._Latitude);
            }
        }

        public double LatitudeAccuracy
        {
            get
            {
                if (this._LatitudeAccuracy == 0)
                    this.setValues();
                return (double)((float)this._LatitudeAccuracy);
            }
        }

        public double LongitudeAccuracy
        {
            get
            {
                if (this._LongitudeAccuracy == 0)
                    this.setValues();
                return (double)((float)this._LongitudeAccuracy);
            }
        }

        public double Accuracy
        {
            get
            {
                if (this._LatitudeAccuracy == 0)
                    this.setValues();
                double A = this._LatitudeAccuracy * 111120 * 2;

                // Auf Wunsch Unschaerfe erhoeht um Faktor 10
                A = A * 10;

                if (A < 1)
                    A = Math.Round(A, 1);
                else
                    A = Math.Round(A, 0);
                return A;
            }
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

        #region Button events

        private void buttonSetStartCoordinates_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGoogleMapsCoordinatesSettings.Default.Latitude = this.Latitude;
                DiversityWorkbench.Forms.FormGoogleMapsCoordinatesSettings.Default.Longitude = this.Longitude;
                DiversityWorkbench.Forms.FormGoogleMapsCoordinatesSettings.Default.Save();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBoxLocality.Text.Length == 0)
                    System.Windows.Forms.MessageBox.Show("Please enter a locality");
                else
                {
                    this.setValues();
                    this.SetURI();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetURI()
        {
            try
            {
                string URL_base = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
                if (string.IsNullOrEmpty(URL_base))
                    return;
                string URI = URL_base + "?";
                if (this.textBoxLocality.Text.Length > 0)
                    URI += "&Loc=" + this.textBoxLocality.Text + "";
                if (this._Latitude != 0 || this._Longitude != 0)
                    URI += "&Lat=" + this._Latitude.ToString().Replace(",", ".") + "&Lng=" + this._Longitude.ToString().Replace(",", ".");
                else
                    URI += "&Lat=" + DiversityWorkbench.Forms.FormGoogleMapsCoordinatesSettings.Default.Latitude.ToString().Replace(',', '.')
                        + "&Lng=" + DiversityWorkbench.Forms.FormGoogleMapsCoordinatesSettings.Default.Longitude.ToString().Replace(',', '.');
                //URI += "&SizeX=" + this.webBrowser.Width.ToString() + "&SizeY=" + this.webBrowser.Height.ToString();
                URI += "&Cross=1";
                System.Uri u = new Uri(URI);
                this.userControlWebView.Url = u;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void userControlWebView_MessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            // Store reported coordinates
            _LastCoordinatesFromMap = e.WebMessageAsJson;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.setValues())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion
    }
}