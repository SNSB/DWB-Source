using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfSamplingPlotPage;
using System.Windows.Forms;

namespace DiversitySamplingPlots.UserControls
{
    public partial class UserControlGeography : UserControl
    {

        #region Parameter

        private Forms.FormDiversitySamplingPlots My_Parent;

        /// <summary>
        /// the WPF control for the GIS operations
        /// </summary>
        private WpfSamplingPlotPage.WpfControl _wpfControl;

        /// <summary>
        /// the mode of the control, if it is used for editing, browsing or viewing
        /// </summary>
        public enum MapMode { Browse, Edit, View };
        private MapMode _MapMode = MapMode.Browse;
        private MapMode _MapModeLast = MapMode.Browse;

        private List<GeoObject> _GeoObjectsEditor;
        private GeoObject _GeoObject;

        private System.Data.DataRow _DataRowSamplingPlot_GeoCache;
        private System.Data.DataRow _DataRowSamplingPlot;

        public System.Collections.Generic.List<int> _QueryListOfSelectedIDs = new System.Collections.Generic.List<int>();
        private System.Data.DataTable _dtGeo = new DataTable();

        #endregion // Fields

        #region Properties

        public MapMode MapDisplayMode
        {
            get { return _MapMode; }
        }

        public System.Data.DataRow DataRowSamplingPlot_GeoCache
        {
            get
            {
                return this._DataRowSamplingPlot_GeoCache;
            }
            set
            {
                this._DataRowSamplingPlot_GeoCache = value;
            }
        }

        public System.Data.DataRow DataRowSamplingPlot
        {
            get
            {
                return this._DataRowSamplingPlot;
            }
            set
            {
                this._DataRowSamplingPlot = value;
            }
        }

        private System.Windows.Media.Brush BrushStroke
        {
            get
            {
                System.Windows.Media.Brush b = System.Windows.Media.Brushes.Red;
                return b;
            }
        }

        private System.Windows.Media.Brush BrushFill
        {
            get
            {
                System.Windows.Media.Brush b = System.Windows.Media.Brushes.Yellow;
                return b;
            }
        }

        #endregion // Properties

        #region Construction
        public UserControlGeography()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods 

        #region Initialize GIS and controls

        public void InitControl(Forms.FormDiversitySamplingPlots parent)
        {
            this.initWPFcontrol();
            this.setVisibilityOfControls();
            //web Browser.ScrollBarsEnabled = false;
            this.userControlWebView.HorizontalScroll.Enabled = false;
            this.userControlWebView.VerticalScroll.Enabled = false;
            My_Parent = parent;
        }

        /// <summary>
        /// initialize the WPF control
        /// </summary>
        private void initWPFcontrol()
        {
            // Create WPF control instance
            _wpfControl = new WpfSamplingPlotPage.WpfControl(DiversityWorkbench.Settings.Language);

            // Add to our Form
            elementHost.Child = _wpfControl;
        }

        /// <summary>
        /// setting the visibility of the controls in dependence of the GisState and data
        /// </summary>
        private void setVisibilityOfControls()
        {
            this.splitContainerMaps.Visible = true;
            if (this._GeoObjectsEditor == null)
                this._GeoObjectsEditor = new List<GeoObject>();

            // splitContainerMaps
            if (this._MapMode == MapMode.Browse)
            {
                this.splitContainerMaps.Panel1Collapsed = false;
                this.splitContainerMaps.Panel2Collapsed = true;
            }
            else
            {
                this.splitContainerMaps.Panel1Collapsed = true;
                this.splitContainerMaps.Panel2Collapsed = false;
            }

            //toolStripButtonSave
            if (this._MapMode == MapMode.Edit) // && this._GeoObjectsEditor.Count == 1)
            {
                this.toolStripButtonSave.Visible = true;
            }
            else
                this.toolStripButtonSave.Visible = false;

            // handling of the GIS editor
            if (this._MapMode == MapMode.View)
            {
                this.elementHost.Child = _wpfControl.WpfGetCanvas(this.panelForWpfControl);
            }
            else if (this._MapMode == MapMode.Edit)
            {
                elementHost.Child = null;
                // Add canvas again to wpfControl
                _wpfControl.WpfSetCanvas();
                // Add wpfControl to ctrlHost
                elementHost.Child = _wpfControl;
            }
        }

        #endregion //  Initialize GIS and controls

        #region GIS WPF interface methods

        /// <summary>
        /// Call interface method to transmit GeoObjects
        /// </summary>
        public bool SendGeoObjects(List<GeoObject> GeoObjects)
        {
            // Call interface method to transmit GeoObjects
            return _wpfControl.WpfSetGeoObjects(GeoObjects);
        }

        /// <summary>
        /// Call interface method to receive GeoObjects
        /// </summary>
        public bool ReceiveGeoObjects(out List<GeoObject> GeoObjects)
        {
            // Call interface method to transmit GeoObjects
            return _wpfControl.WpfGetGeoObjects(out GeoObjects);
        }

        #endregion // GIS WPF interface methods

        #region GIS Browser methods

        /// <summary>
        /// setting the map in the browser
        /// </summary>
        /// <param name="URL"></param>
        private void setMapInBrowser(WpfSamplingPlotPage.GeoObject GeoObject)
        {
            string URL = this.GetCoordinatesOfGeoObject(GeoObject);
            try
            {
                if (URL.Length > 2000)
                {
                    URL = this.GetCenterCoordinatesOfGeoObject(GeoObject);
                    //System.Windows.Forms.MessageBox.Show("Too many items, please reduce selection");
                    //return;
                }
                if (URL.Length > 0)
                {
                    //URL = System.Web.HttpUtility.UrlEncode(URL);
                    URL = global::DiversitySamplingPlots.Properties.Settings.Default.GoogleMapsV4BaseUrl + URL;
                }
                else
                {
                    URL = global::DiversitySamplingPlots.Properties.Settings.Default.GoogleMapsV4BaseUrl + "Lat=48.160234&Lng=11.500093&Zoom=10";
                }
                System.Uri URI = new Uri(URL);
                this.userControlWebView.Url = null;
                this.userControlWebView.Navigate(URI);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string GetCoordinatesOfGeoObject(GeoObject GeoObject)
        {
            if (GeoObject.GeometryData == null || GeoObject.GeometryData.Length == 0)
                return "";

            string URL = "";
            string Long = "";
            string Lat = "";
            string Label = "";
            try
            {
                if (GeoObject.GeometryData.Length > 0
                    && GeoObject.GeometryData.ToUpper().StartsWith("POINT"))
                {
                    string GeoData = GeoObject.GeometryData;
                    if (this.AddCoordinatesOfGeoPoint(GeoData, ref Lat, ref Long))
                        Label += GeoObject.Identifier;
                }
                else if (GeoObject.GeometryData.Length > 0)
                {
                    string SQL = "DECLARE @I int " +
                        "DECLARE @Geo geography " +
                        "SET @Geo = '" + GeoObject.GeometryData + "' " +
                        "SET @I = (SELECT @Geo.STNumPoints()); " +
                        "DECLARE @DtGeo TABLE (Point nvarchar(1000) COLLATE Latin1_General_CI_AS NOT NULL); " +
                        "WHILE @I > 0 " +
                        "BEGIN " +
                        "INSERT INTO @DtGeo (Point) SELECT @Geo.STPointN(@I).ToString() " +
                        "SET @I = @I - 1 " +
                        "END " +
                        "SELECT * FROM @DtGeo;";
                    System.Data.DataTable dtPolyPoints = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dtPolyPoints);
                    foreach (System.Data.DataRow R in dtPolyPoints.Rows)
                    {
                        if (this.AddCoordinatesOfGeoPoint(R[0].ToString(), ref Lat, ref Long))
                        {
                            if (Label.Length > 0)
                                Label += "|";
                            Label += GeoObject.DisplayText;
                        }
                    }
                }
                if (Long.Length > 0 && Lat.Length > 0 && Label.Length > 0)
                {
                    string zoom = "";
                    if (!Lat.Contains("|"))
                    {
                        zoom = "&Zoom=15";
                    }
                    if (this.checkBoxZoomLevel.Checked)
                    {
                        zoom = "&Zoom=" + this.numericUpDownZoomLevel.Value.ToString();
                    }
                    URL = "LatPoint=" + Lat
                        + "&LngPoint=" + Long
                        + "&Label=" + Label
                        + zoom;
                    // + "&Height=" + this.web Browser.Height.ToString()
                    // + "&Width=" + this.web Browser.Width.ToString();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return URL;
        }

        private string GetCenterCoordinatesOfGeoObject(GeoObject GeoObject)
        {
            string URL = "";
            string Long = "";
            string Lat = "";
            string Label = "";
            try
            {
                if (GeoObject.GeometryData.Length > 0
                    && GeoObject.GeometryData.ToUpper().StartsWith("POINT"))
                {
                    string GeoData = GeoObject.GeometryData;
                    if (this.AddCoordinatesOfGeoPoint(GeoData, ref Lat, ref Long))
                        Label += GeoObject.Identifier;
                }
                else if (GeoObject.GeometryData.Length > 0)
                {
                    string SQL = "DECLARE @Geo geography " +
                        "SET @Geo = '" + GeoObject.GeometryData + "' " +
                        "SELECT 'POINT (' + CAST(@Geo.EnvelopeCenter().Long AS varchar) + ' ' + CAST(@Geo.EnvelopeCenter().Lat AS varchar) + ')' AS GEOPOINT;";
                    System.Data.DataTable dtPolyPoints = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dtPolyPoints);
                    foreach (System.Data.DataRow R in dtPolyPoints.Rows)
                    {
                        if (this.AddCoordinatesOfGeoPoint(R[0].ToString(), ref Lat, ref Long))
                        {
                            if (Label.Length > 0)
                                Label += "|";
                            Label += GeoObject.DisplayText;
                        }
                    }
                }
                if (Long.Length > 0 && Lat.Length > 0 && Label.Length > 0)
                {
                    string zoom = "15";
                    if (this.checkBoxZoomLevel.Checked)
                    {
                        zoom = this.numericUpDownZoomLevel.Value.ToString();
                    }
                    // URL = "Lat=" + Lat
                    //     + "&Lng=" + Long
                    URL = "LatPoint=" + Lat
                        + "&LngPoint=" + Long
                        + "&Label=" + Label
                        + "&Zoom=" + zoom;
                    // + "&Height=" + this.web Browser.Height.ToString()
                    // + "&Width=" + this.web Browser.Width.ToString();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return URL;
        }

        private bool AddCoordinatesOfGeoPoint(string GeoPoint, ref string Latitude, ref string Longitude)
        {
            bool OK = false;
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            try
            {
                string[] GeoDataParts = GeoPoint.Split(new char[] { ' ' });
                string sLong = GeoDataParts[1].ToString(InvC).Replace("(", "");
                string sLat = GeoDataParts[2].ToString(InvC).Replace(")", "");
                double dLong = 0;
                double dLat = 0;
                if (double.TryParse(sLong.ToString(InvC).Replace(",", "."), System.Globalization.NumberStyles.Float, InvC, out dLong)
                    && double.TryParse(sLat.ToString(InvC).Replace(",", "."), System.Globalization.NumberStyles.Float, InvC, out dLat)
                    && dLong <= 180
                    && dLong >= -180
                    && dLat <= 180
                    && dLat >= -180)
                {
                    if (Longitude.Length > 0)
                    {
                        Longitude += "|";
                        Latitude += "|";
                    }
                    Longitude += sLong;
                    Latitude += sLat;
                    OK = true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return OK;
        }

        private string GetCoordinatesOfGeoObject(System.Collections.Generic.List<GeoObject> GeoObjectList)
        {
            string URL = "";
            string Long = "";
            string Lat = "";
            string Label = "";
            System.Globalization.CultureInfo InvC = new System.Globalization.CultureInfo("");
            try
            {
                foreach (GeoObject GeoObject in GeoObjectList)
                {
                    if (GeoObject.GeometryData.Length > 0
                        && GeoObject.GeometryData.ToUpper().StartsWith("POINT"))
                    {
                        string GeoData = GeoObject.GeometryData;
                        string[] GeoDataParts = GeoData.Split(new char[] { ' ' });
                        string sLong = GeoDataParts[1].Replace("(", "");
                        string sLat = GeoDataParts[2].Replace(")", "");
                        double dLong = 0;
                        double dLat = 0;
                        if (double.TryParse(sLong.ToString(InvC).Replace(",", "."), System.Globalization.NumberStyles.Float, InvC, out dLong)
                            && double.TryParse(sLat.ToString(InvC).Replace(",", "."), System.Globalization.NumberStyles.Float, InvC, out dLat)
                            && dLong <= 180
                            && dLong >= -180
                            && dLat <= 180
                            && dLat >= -180)
                        {
                            if (Long.Length > 0)
                            {
                                Long += "|";
                                Lat += "|";
                                Label += "|";
                            }
                            Long += sLong;
                            Lat += sLat;
                            Label += GeoObject.Identifier;
                        }
                    }
                }
                if (Long.Length > 0 && Lat.Length > 0 && Label.Length > 0)
                {
                    URL = "Lat=" + Lat
                        + "&Lng=" + Long
                        + "&Label=" + Label
                    + "&Height=" + this.userControlWebView.Height.ToString() // this.web Browser.Height.ToString()
                    + "&Width=" + this.userControlWebView.Width.ToString(); // this.web Browser.Width.ToString();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return URL;
        }

        #endregion // GIS Browser methods

        #region GIS Editor Methods

        /// <summary>
        /// Display of the selected plots according to the GIS control
        /// </summary>
        public void ShowPlots()
        {
            this.setVisibilityOfControls();

            // Check if multiple plots selected in query list
            if (_QueryListOfSelectedIDs.Count > 1)
            {
                // Display all selected plots in GIS-Editor
                fillGeoObjects(_QueryListOfSelectedIDs);
            }
            else
            {
                // Just display the single selected plot
                this.DisplayGeoObjects();
            }

        }

        // ===WR===
        public void DisplayGeoObjects(List<GeoObject> geoObjectList)
        {
            if (this._MapMode == MapMode.View || this._MapMode == MapMode.Edit)
            {
                this.toolStripButtonSave.Enabled = false;
                this._GeoObjectsEditor = new List<GeoObject>(geoObjectList);
                // this._GeoObjectsEditor.Add(this._GeoObject);

                this._wpfControl.WpfClearAllSamples(true);
                if (this.checkBoxZoomLevel.Checked)
                    this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor, (int)this.numericUpDownZoomLevel.Value);
                else
                    this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);

                if (_MapMode == MapMode.View || _MapMode == MapMode.Edit)
                {
                    // Display a "Wait for map" mssage and add the sample
                    this._wpfControl.WpfAddSample();
                }
            }
        }


        public void DisplayGeoObjects()
        {
            this.SetGeoObject();
            if (this._GeoObject.GeometryData != null && this._GeoObject.GeometryData.Length > 0)
            {
                switch (this._MapMode)
                {
                    case MapMode.Browse:
                        this.setMapInBrowser(this._GeoObject);
                        break;
                    case MapMode.Edit:
                        this.toolStripButtonSave.Enabled = true;
                        this.toolStripButtonSave.Visible = true;
                        this._GeoObjectsEditor = new List<GeoObject>();
                        this._GeoObjectsEditor.Add(this._GeoObject);

                        this._wpfControl.WpfClearAllSamples(true);
                        if (this.checkBoxZoomLevel.Checked)
                            this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor, (int)this.numericUpDownZoomLevel.Value);
                        else
                            this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
                        // Display a "Wait for map" mssage and add the sample
                        this._wpfControl.WpfAddSample();
                        break;
                    case MapMode.View:
                        this._GeoObjectsEditor = new List<GeoObject>();
                        this._GeoObjectsEditor.Add(this._GeoObject);

                        this._wpfControl.WpfClearAllSamples(true);
                        if (this.checkBoxZoomLevel.Checked)
                            this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor, (int)this.numericUpDownZoomLevel.Value);
                        else
                            this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
                        // Display a "Wait for map" mssage and add the sample
                        this._wpfControl.WpfAddSample();
                        break;
                    default:
                        this._GeoObjectsEditor = new List<GeoObject>();
                        this._GeoObjectsEditor.Add(this._GeoObject);

                        this._wpfControl.WpfClearAllSamples(true);
                        if (this.checkBoxZoomLevel.Checked)
                            this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor, (int)this.numericUpDownZoomLevel.Value);
                        else
                            this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
                        this._wpfControl.WpfAddSample();
                        break;
                }
            }
            else // NO Data
            {
                switch (this._MapMode)
                {
                    case MapMode.Browse:
                        WpfSamplingPlotPage.GeoObject G = new GeoObject();
                        this.setMapInBrowser(G);
                        //this.setMapInBrowser("");
                        break;
                    default:
                        this._GeoObjectsEditor = new List<GeoObject>();
                        this._GeoObjectsEditor.Add(this._GeoObject);

                        this._wpfControl.WpfClearAllSamples(true);
                        this._wpfControl.WpfSetMode(1);
                        //this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjects, (int)this.numericUpDownZoomLevel.Value);
                        //this._wpfControl.WpfAddSample();
                        break;
                }

            }
            _MapModeLast = _MapMode;
        }

        private void ResetGeography()
        {
            this._GeoObjectsEditor = null;
        }

        private void SetGeoObject()
        {
            this._GeoObject = new GeoObject();
            _GeoObject.PointType = PointSymbol.None;
            _GeoObject.PointSymbolSize = 0.0;
            try
            {
                if (this.DataRowSamplingPlot_GeoCache != null)
                {
                    if (!this.DataRowSamplingPlot_GeoCache["Geography"].Equals(System.DBNull.Value)
                       && this.DataRowSamplingPlot_GeoCache["Geography"].ToString().Length > 0)
                    {
                        string DisplayText = "";
                        if (this.DataRowSamplingPlot !=null
                            && this.DataRowSamplingPlot.Table.Columns.Contains("PlotIdentifier")
                            && !this.DataRowSamplingPlot["PlotIdentifier"].Equals(System.DBNull.Value)
                            && this.DataRowSamplingPlot["PlotIdentifier"].ToString().Length > 0)
                            DisplayText = this.DataRowSamplingPlot["PlotIdentifier"].ToString();
                        else
                        {
                            if (this.DataRowSamplingPlot != null
                            && this.DataRowSamplingPlot.Table.Columns.Contains("Description"))
                                DisplayText = this.DataRowSamplingPlot["Description"].ToString();
                        }
                        if (DisplayText.Length > 20) DisplayText = DisplayText.Substring(0, 20) + "...";

                        this._GeoObject = this.setupGeoObject(this.DataRowSamplingPlot_GeoCache["PlotID"].ToString()
                            , DisplayText
                            , this.DataRowSamplingPlot_GeoCache["Geography"].ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // ===WR===
        public void fillGeoObjects(List<int> PlotIDList)
        {
            try
            {
                string WhereClause = string.Empty;
                bool first = true;
                foreach (int id in PlotIDList)
                {
                    if (!first)
                        WhereClause += "OR ";
                    first = false;
                    WhereClause += "(I.PlotID = " + id.ToString() +
                        " AND I.PlotID IN (SELECT U.PlotID FROM PlotID_UserAvailable U)) ";
                }

                // string SQL = "SELECT PlotID, PlotGeography_Cache.ToString() AS PlotGeography_Cache, PlotIdentifier " + "FROM SamplingPlot WHERE " + WhereClause;
                string SQL = "SELECT I.PlotID, I.Geography, P.PlotIdentifier " + "FROM SamplingPlotGeoInfo I, SamplingPlot P WHERE I.PlotID = P.PlotID AND (" + WhereClause + ")";

                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString + ";Connect Timeout=600");
                this._dtGeo.Clear();
                ad.Fill(this._dtGeo);


                List<GeoObject> GeoObjects = new List<GeoObject>();

                int count = _dtGeo.Rows.Count;

                foreach (DataRow row in _dtGeo.Rows)
                {
                    if (row[1] != null)
                    {
                        GeoObject geoObject = new GeoObject();
                        geoObject.Identifier = row[0].ToString();
                        geoObject.DisplayText = row[2].ToString();
                        geoObject.StrokeBrush = System.Windows.Media.Brushes.Red;
                        geoObject.FillBrush = System.Windows.Media.Brushes.Yellow;
                        geoObject.StrokeTransparency = 255;
                        geoObject.FillTransparency = 64;
                        geoObject.StrokeThickness = 1.0;
                        geoObject.PointType = WpfSamplingPlotPage.PointSymbol.None;
                        geoObject.PointSymbolSize = 0.0;
                        geoObject.GeometryData = row[1].ToString();
                        if (geoObject.GeometryData.Length > 0)
                            GeoObjects.Add(geoObject);
                    }
                }
                if (GeoObjects.Count > 0)
                    DisplayGeoObjects(GeoObjects);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error getting geography data.", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        #endregion // GIS Editor Methods

        #region Setup GeoObjects

        /// <summary>
        /// Set up GeoObject structure
        /// </summary>
        /// <param name="Identifier">the identifier for the object</param>
        /// <param name="DisplayText">the text displayed in the user interface</param>
        /// <param name="data">the geographical data</param>
        /// <returns></returns>
        private GeoObject setupGeoObject(string Identifier, string DisplayText, string data)
        {
            GeoObject geoObject = new GeoObject();
            geoObject.Identifier = Identifier;
            geoObject.DisplayText = DisplayText;
            geoObject.GeometryData = data;
            geoObject.FillBrush = this.BrushFill;
            geoObject.StrokeBrush = this.BrushStroke;
            geoObject.PointType = PointSymbol.None;
            geoObject.PointSymbolSize = 0.0;
            geoObject.StrokeThickness = 1.0;
            geoObject.StrokeTransparency = 255;
            geoObject.FillTransparency = 64;
            return geoObject;
        }

        // --- currently not used ---

        /// <summary>
        /// Fill up GeoObject structure
        /// </summary>
        /// <param name="Identifier">the identifier for the object</param>
        /// <param name="DisplayText">the text displayed in the user interface</param>
        /// <param name="data">the geographical data</param>
        /// <param name="brush">the color for the object</param>
        /// <returns></returns>
        private GeoObject setupGeoObject(string Identifier, string DisplayText, string data, System.Windows.Media.Brush brush)
        {
            GeoObject geoObject = new GeoObject();
            geoObject.Identifier = Identifier;
            geoObject.DisplayText = DisplayText;
            geoObject.StrokeBrush = brush;
            geoObject.FillBrush = brush;
            geoObject.GeometryData = data;
            return geoObject;
        }

        #endregion // Setup GeoObjects

        #endregion // Methods

        #region Events

        private void toolStripMenuItemEditModeBrowser_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonEditMode.Image = this.toolStripMenuItemEditModeBrowser.Image;
            this.toolStripDropDownButtonEditMode.Text = this.toolStripMenuItemEditModeBrowser.Text;
            this.toolStripDropDownButtonEditMode.ToolTipText = "Use browser to display the map";
            this._MapMode = MapMode.Browse;
            this.setVisibilityOfControls();
            // Call after setVisibilityOfControls() to get the correct heigth and width of the webBroser control
            this.DisplayGeoObjects();
        }

        private void toolStripMenuItemEditModeGisNoEdit_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonEditMode.Image = this.toolStripMenuItemEditModeGisNoEdit.Image;
            this.toolStripDropDownButtonEditMode.Text = this.toolStripMenuItemEditModeGisNoEdit.Text;
            this.toolStripDropDownButtonEditMode.ToolTipText = "Use GIS control to display the map";
            this._MapMode = MapMode.View;
            this.setVisibilityOfControls();
            this.DisplayGeoObjects();
        }

        private void toolStripMenuItemEditModeGIS_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonEditMode.Image = this.toolStripMenuItemEditModeGIS.Image;
            this.toolStripDropDownButtonEditMode.Text = this.toolStripMenuItemEditModeGIS.Text;
            this.toolStripDropDownButtonEditMode.ToolTipText = "Use GIS control to display the map";
            this._MapMode = MapMode.Edit;
            this.setVisibilityOfControls();
            this.DisplayGeoObjects();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (this._wpfControl.WpfGetGeoObjects(out this._GeoObjectsEditor)
                && this._GeoObjectsEditor.Count == 1
                && this._GeoObjectsEditor[0].GeometryData.Length > 0)
            {
                string Geography = this._GeoObjectsEditor[0].GeometryData;
                // this._DataRowSamplingPlot_GeoCache["PlotGeography_Cache"] = this._GeoObjectsEditor[0].GeometryData;
                this._DataRowSamplingPlot_GeoCache["Geography"] = this._GeoObjectsEditor[0].GeometryData;

                My_Parent.setGeography(Geography);
                /*
                string SQL = "UPDATE SamplingPlot SET PlotGeography_Cache = '" + Geography + "' WHERE PlotID = " + this._DataRowSamplingPlot["PlotID"].ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                */
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {

            if (_QueryListOfSelectedIDs.Count > 1)
            {
                fillGeoObjects(_QueryListOfSelectedIDs);
            }
            else
            {
                this.DisplayGeoObjects();
            }
        }

        public void SizeOfControlChanged()
        {
            if (_MapMode == MapMode.Browse)
                DisplayGeoObjects();
        }

        #endregion // Events

    }
}
