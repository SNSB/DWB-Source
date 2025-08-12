
// --------------------------------------------------------------------------------------------------------
// 
// GIS-Editor - a tool to create, visualize, edit and archive samples within a geographical environment.
// Copyright (C) 2011 by Wolfgang Reichert, Botanische Staatssammlung München, mailto: reichert@bsm.mwn.de
//
// This program is free software; you can redistribute it and/or modify it under the terms of the 
// GNU General Public License as published by the Free Software Foundation; 
// either version 2 of the License, or (at your option) any later version. 
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
// See the GNU General Public License for more details. 
// 
// You should have received a copy of the GNU General Public License along with this program;
// if not, write to the Free Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110, USA
//
// --------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Interop;
using WpfSamplingPlotPage.Properties;
using System.Resources;
using System.Web;
using System.Windows.Threading;
using System.Data;
using System.Data.OleDb;
using System.ComponentModel;
using System.Xml;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

/*  Noch einbauen?
Distanzberechnung zwischen Lat/Lon Punkten:
d=2*asin(sqrt((sin((lat1-lat2)/2))^2 + cos(lat1)*cos(lat2)*(sin((lon1-lon2)/2))^2))
alternativ (Rundungsfehler bei kurzen Distanzen):
d=acos(sin(lat1)*sin(lat2)+cos(lat1)*cos(lat2)*cos(lon1-lon2))
*/

namespace WpfSamplingPlotPage
{

    #region Public structs

    /// <summary>
    /// Struct for geometry object interface.
    /// </summary>
    public struct GeoObject
    {
        /// <summary>
        /// Identifier for the geometry object element.
        /// </summary>
        public string Identifier;
        /// <summary>
        /// Decription for the geometry object element.
        /// </summary>
        public string DisplayText;
        /// <summary>
        /// Stroke color.
        /// </summary>
        public Brush StrokeBrush;
        /// <summary>
        /// Fill color.
        /// </summary>
        public Brush FillBrush;
        /// <summary>
        /// Stroke transparency (0..255).
        /// </summary>
        public byte StrokeTransparency;
        /// <summary>
        /// Fill transparency (0..255).
        /// </summary>
        public byte FillTransparency;
        /// <summary>
        /// Stroke thickness.
        /// </summary>
        public double StrokeThickness;
        /// <summary>
        /// Type of the point symbol.
        /// </summary>
        public PointSymbol PointType;
        /// <summary>
        /// Size factor of the point symbol (e.g. 2.0 = double size).
        /// </summary>
        public double PointSymbolSize;
        /// <summary>
        /// Geometry data string (MS SQL notation).
        /// </summary>
        public string GeometryData;
    }


    #endregion // Public structs

    /// <summary>
    /// Interaktionslogik für UserControl1.xaml
    /// </summary>
    public partial class WpfControl : UserControl, IDisposable
    {

        #region Public enumerations

        /// <summary>
        /// Enumeration of the Editor Modes.
        /// </summary>
        public enum MouseMode
        {
            /// <summary>
            /// Undefined mode.
            /// </summary>
            None,
            /// <summary>
            /// In this mode a reference map can be selected (provided by the SNSB Google Maps webservice).
            /// </summary>
            MapServer,
            /// <summary>
            /// In this mode the working area can be shifted or zoomed.
            /// </summary>
            ShiftCanvas,
            /// <summary>
            /// Mode to create a polygon (area).
            /// </summary>
            DrawPolygon,
            /// <summary>
            /// Mode to create a string of concatenated lines.
            /// </summary>
            DrawLine,
            /// <summary>
            /// Mode to set a number of object markers.
            /// </summary>
            DrawSamplePoints,
            /// <summary>
            /// In this mode all visible samples at the working area (including images) can be modified.
            /// </summary>
            EditSamples,
            /// <summary>
            /// Used to calibrate elder maps using a reference map (create coordinates for the maps).
            /// </summary>
            MapAdapt
        };

        /// <summary>
        /// Enumeration of Geonames services.
        /// </summary>
        public enum GeoNamesOrgServices
        {
            /// <summary>
            /// Not used.
            /// </summary>
            findNearbyPlaceName,
            /// <summary>
            /// Not used.
            /// </summary>
            countrySubdivision,
            /// <summary>
            /// Not used.
            /// </summary>
            neighbourhood,
            /// <summary>
            /// Shuttle Radar Topography Mission (SRTM) elevation data with a horizontal and 
            /// vertical grid spacing of 3-arc-second (approximately 90 meters).
            /// The dataset covers land areas between 60 degrees north and 56 degrees south.
            /// </summary>
            srtm3,
            /// <summary>
            /// A global digital elevation model (DEM) with a horizontal grid spacing 
            /// of 30 arc seconds (approximately 1 kilometer).
            /// </summary>
            gtopo30
        };

        /// <summary>
        /// Enumeration of map world coordinates check results.
        /// </summary>
        public enum CheckResult
        {
            /// <summary>
            /// The map coordinates are correct.
            /// </summary>
            OK,
            /// <summary>
            /// The map coordinates are not strictly north aligned.
            /// </summary>
            Unaligned,
            /// <summary>
            /// The map coordinates are outside the reference map
            /// </summary>
            Unbound,
            /// <summary>
            /// An error has occurred.
            /// </summary>
            Failure
        }

        #endregion // Public enumerations

        #region Constants

        // Filter for File dialogs
        private const string StrFilterAll = "All files(*.*)|*.*|Images(*.jpg;*.jpeg;*.bmp;*.tif;*.png;*.gif)|*.jpg;*.jpeg;*.bmp;*.tif;*.tiff;*.png;*.gif|Shapes(*.shp1;*.shp2;*.gpx)|*.shp1;*.shp2;*.gpx";
        private const string StrFilterShape = "All files(*.*)|*.*|Shapes(*.shp1;*.shp2;*.gpx)|*.shp1;*.shp2;*.gpx";
        private const string StrFilterImage = "All files(*.*)|*.*|Maps(*.png)|*.png";

        // Extension for ArcGis Shape and Index files
        private const string StrArcGisShapeFileExtension = ".shp";
        private const string StrArcGisIndexFileExtension = ".shx";
        private const string StrArcGisDBaseFileExtension = ".dbf";
        // Extension for Diversity Shape files
        private const string StrShapeFileExtension = ".shp1";
        private const string StrShape2FileExtension = ".shp2";
        private const string StrGpxFileExtension = ".gpx";
        // Extension for Diversity Shape files
        private const string StrImportFileExtension = ".txt";
        // Default filename for saving reference maps
        private const string StrSaveFilename = "MyMap";
        // Default filename for saving reference maps
        private const string StrInitialDirectory = "E:\\";
        // Number of lines for one sample in Div. shapes file
        private const int SampleEntryLength = 10;

        // Extension for Map files
        private const string StrMapFileExtension = ".png";
        // Extension for Image attribute files
        private const string StrImageXmlFileExtension = ".xml";
        // Extension for Glopus calibration files
        private const string StrImageKalFileExtension = ".kal";
        // Carriage-Return Separator string
        internal const string StrCR = "\r\n";
        // TAB Separator string
        internal const string StrTAB = "\t";
        // GeometryData Separator strings
        private const string StrBlank = " ";
        private const string StrComma = ",";
        private const string StrOpen = "(";
        private const string StrClose = ")";

        // Size of the Toggle buttons
        private const int ToggleButtonWidth = 24;
        private const int ToggleButtonHeight = 20;
        private const int ToggleButtonLeft = 17; // 21;
        private const int ToggleButtonDistance = 2;
        private const int DeleteButtonWidth = 17;
        private const int DeleteButtonHeight = 17;
        private const int DeleteButtonLeft = 0; // 4;
        private const int IdLabelHeight = 15;
        private const int IdLabelDistance = 4;

        // Set zoom slider region (factor 0.6 to 3)
        private const double SliderZoomMin = 0.6;
        private const double SliderZoomMax = 3.0;
        // Note: When factor < 0.52, there are problems with shifting the canvas (flickering etc)

        private const byte DefaultStrokeTransparency = 255;
        private const byte DefaultFillTransparency = 64;
        private const byte AdaptModeMinTransparency = 26; // 10%

        // Viewer path
        private static readonly string MapModeViewerOSM = global::WpfSamplingPlotPage.Properties.Settings.Default.MapModeViewerOSM;
        private static readonly string MapModeViewerGoogle = global::WpfSamplingPlotPage.Properties.Settings.Default.MapModeViewerGoogle;
        // Add Points to WebBrowser 
        private static readonly string MapModeViewer1OSM = global::WpfSamplingPlotPage.Properties.Settings.Default.MapModeViewer1OSM;
        private static readonly string MapModeViewer1Google = global::WpfSamplingPlotPage.Properties.Settings.Default.MapModeViewer1Google;
        private const string WebBrowserPath2 = "&LngPoint=";
        private const string WebBrowserPath3 = "&Label=";
        private const string WebBrowserPath4 = "&Height={0}&Width={1}&Scan={2}&MapType={3}&MinZoom=2";

        private const int WebBrowserBorder = 0;
        private const int WebBrowserHeightDiff = 2;
        private int m_WebBrowserWidthDiff = 0;
        private const int WebBrowserWidthDiffGoogle = 0; // 16;
        private const int WebBrowserWidthDiffOSM = 16;

        // Settings depending on MapModeViewer
        private const int WebBrowserAdaptWidthOSM = 70; // ###54; // OSM Settings!! --> adapt if shifting the Webbrowser window!
        private const int WebBrowserAdaptWidthGoogle = 70; // Google Settings!! --> adapt if shifting the Webbrowser window!
        private const int WebBrowserAdaptHeightOSM = 75; // OSM Settings!!
        private const int WebBrowserAdaptHeightGoogle = 75; // Google settings!! (old: 71)
        // WebBrowserHeightDiff + WebBrowserAdaptHeight = m_CanvasClip.Canvas.Top (75) !? 

        private const int WebBrowserCanvasLeft = 70; // --> adapt if shifting the Webbrowser window!
        // WebBrowserWidthDiff + WebBrowserAdaptWidth = WebBrowserCanvasLeft = m_CanvasClip.Canvas.Left (70) !?

        // Element tags
        private const string tagRef = "reference";
        private const string tagDel = "delete";

        // Max. latitude for geo object display warning
        private const int MaxLatitudeWarning = 75;
        // Max number of pick points used to change the instruction message 
        private const int PickPointPinListCount = 6;
        // Default thickness of sample strokes
        internal const double DefaultPolygonStrokeThickness = 1.0;
        internal const double DefaultLineStrokeThickness = 1.0;
        internal const double DefaultPointsStrokeThickness = 1.0;
        internal const PointSymbol DefaultPointSymbol = PointSymbol.Pin;
        internal const double DefaultPointSymbolSize = 1.0;
        internal const byte DefaultSampleOffTransparency = 64;
        internal const string DefaultGpsBaudrate = "9600";

        private const string StrIdBlank = "Blank";
        private const string StrTextBackground = "Background";

        private bool m_SilentErrorMode = false;

        #endregion

        #region Control Flags

        // TEST Flag
        internal bool m_Testcase = false;
        internal string m_TestcaseDescription = "";

        // Flag to be set if GPS position should be tracked as a line string on background map
        internal bool m_TrackGpsData = false;
        // Flag to be set if no altitude should be retrieved from geonames web server (too many points, no connection ...)
        internal bool m_GetAltitude = false;
        // Create ArcGis dBase file using MS OLEDB Adapter. Will be set to false, if Adapter is not installed.
        private bool m_CreateDbaseFile = true;
        // Flag and values for reading ArcView Shapes in UTM Format
        internal bool m_ReadArcViewShapeFormatUtm = false;
        internal int m_ReadArcViewShapeFormatUtmZone = 32;
        internal string m_ReadArcViewShapeFormatUtmHemi = "N";
        internal bool m_SplitShapes = false;
        // Flag to be set if shapes should be saved in SQL GeoObject file
        internal bool m_WriteShapesToSqlFile = true;
        // Flag to be set if shapes should be saved in ArcView file
        internal bool m_WriteShapesToArcFile = false;
        // Flag to be set if shapes should be saved in Tab separated text file
        internal bool m_WriteShapesToImportFile = false;
        // Flag to be set if image coordinates should be saved in Glopus calibration file
        internal bool m_WriteImageToKalFile = true;
        // Flag to be set if the complete working area including visible shapes should be saved to a bitmap file
        internal bool m_SaveWorkingArea = false;
        // Flag to be set if a region of the working area marked by a frame should be saved to a bitmap file
        internal bool m_AreaFrame = false;
        // Flag to be set if the GIS Editor is set to the "GIS View Mode" by interface function
        internal bool m_ViewerMode = false;
        // Flag to determine the Mapmode Viewer (Google Maps, Open Street Maps)
        internal int m_MapModeViewer = 0;
        // Flag to be set if a debug text file should be created when reading ArcView shapes
        private bool m_DebugWrite = false;
        // Max length of debug string to be written to debug file
        private int m_MaxDebugWrite = 200000;

        // Color adjustment for Sample detection
        internal byte m_LimitRedMin = 0;
        internal byte m_LimitGreenMin = 0;
        internal byte m_LimitBlueMin = 0;
        internal byte m_LimitRedMax = 255;
        internal byte m_LimitGreenMax = 255;
        internal byte m_LimitBlueMax = 255;
        internal double m_LimitGreyMin = 0;
        internal double m_LimitGreyMax = 255;
        internal int m_PointDistanceMin = 3;
        internal bool m_SeparateSamples = false;
        internal string m_IdText = "Sample";
        internal string m_IdNumber = "00001";

        #endregion // Control Flags

        #region Fields

        // Conversion Methods for geographic coordinates
        internal GeoConversion.GeoCon m_GeoCon = new GeoConversion.GeoCon();

        // Declaration of sample types
        private SampleImage m_SampleImage = null;
        private SamplePolygon m_SamplePolygon = null;
        private SamplePoints m_SamplePoints = null;
        private SampleLines m_SampleLine = null;

        // Temporary blank background map for loading unaligned maps
        private SampleImage m_BlankMap = null;
        // Sample for loading shapes from file
        private Sample m_LoadSample = null;

        // GPS related fields
        private Gps m_Gps = null;
        private SamplePoints m_GpsPosition = null;
        private SampleLines m_GpsTrack = null;
        private Point m_LastGpsPos = new Point();
        internal string m_GpsBaudrate = DefaultGpsBaudrate;
        // private int m_GpsMedium = 10;

        // List of sample objects
        private List<Sample> m_SampleList = new List<Sample>();

        // Supported image formats
        private string[] m_SupportedFormats = { ".jpg", ".jpeg", ".bmp", ".tif", ".tiff", ".png", ".gif" };
        private string m_WorkingAreaSaveFileName = StrSaveFilename;
        private string m_SaveFilename = StrSaveFilename;
        private int m_SaveFileCount = 0;

        // Separators for splitting strings
        private string[] stringSeparators1 = new string[] { "\r\n" };
        private char[] charSeparators2 = new char[] { ',' };
        private char[] charSeparators3 = new char[] { '_' };
        private char[] charSeparators4 = new char[] { '(', ')', ',', ' ', '|' };
        private char[] charSeparators5 = new char[] { '(', ')' };
        private char[] charSeparators6 = new char[] { ' ', ')' };
        private char[] charSeparators7 = new char[] { 'P', 'M', 'L' };
        private char[] charSeparators8 = new char[] { '(', ',' };
        private char[] charSeparators9 = new char[] { '\t' };
        private char[] TrimChars = new char[] { ',', ' ' };

        // Horizontal position of images, could be adapted for shifting subsequent images (currently not used)
        private double m_XPos = 0.0;

        // Definition of Canvas world coordinates (Longitude - Left/Right, Latitude - Top/Bottom, Lon.Accuracy - XFactor, Lat.Accuracy - YFactor)
        private double m_WorldLeft = 0;
        private double m_WorldTop = 0;
        private double m_WorldRight = 0;
        private double m_WorldBottom = 0;
        private double m_XFactor = 1;
        private double m_YFactor = 1;

        // Counter for added images or shapes
        private int m_ImageCount = 0;

        // Identifier and display text background color handling
        private SolidColorBrush m_BrushHighlight = Brushes.Pink;
        private SolidColorBrush m_BrushClear = Brushes.White;
        // private int m_SampleIdCount = 0;
        private Thickness m_LabelPadding = new Thickness(1, 0, 1, 0);

        // Cursor screen coordinates
        private Point m_LastPoint = new Point();
        // --Distance-- 
        private Point m_DistPoint = new Point(0, 0);
        private Point m_PickPointSource = new Point();
        private Point m_PickPointTarget = new Point();
        // Flags for Adapt Mode
        private bool m_PickPointSourceSet = false;
        private bool m_PickPointTargetSet = false;
        private bool m_PickPointLastSource = false;

        private bool m_MouseButtonPressed = false;
        private Point m_Zero = new Point(0, 0);

        // Canvas thickness property
        private Thickness m_CanvasThickness = new Thickness(0, 0, 0, 0);
        private Thickness m_CanvasThicknessZero = new Thickness(0, 0, 0, 0);
        private SampleImage m_RefMap = null;

        // Canvas mouse mode
        private MouseMode m_MouseMode = MouseMode.None;

        // Canvas zoom transfomation
        private ScaleTransform m_Scale = new ScaleTransform();

        // Edit samples
        private SamplePoints m_CatchedSamplePoints = null;
        private SamplePolygon m_CatchedPoly = null;
        private SampleLines m_CatchedLine = null;
        private SampleImage m_CatchedSampleImage = null;
        private int m_CatchedPointIndex = -1;
        private int m_CatchedListIndex = -1;
        private Cursor m_CursorOrg;

        // Adjust samples
        // Set Ellipse as Pin for selected Pickpoints
        // private Ellipse m_PickPointPin = null;
        // private List<Ellipse> m_PickPointPinList = new List<Ellipse>();
        private System.Windows.Shapes.Path m_PickPointPin = null;
        private List<System.Windows.Shapes.Path> m_PickPointPinList = new List<System.Windows.Shapes.Path>();

        // Preset Brush for shapes
        SolidColorBrush m_Brush = Brushes.Red;
        internal SolidColorBrush m_LastStrokeBrush = Brushes.Red;
        internal SolidColorBrush m_LastFillBrush = Brushes.Red;

        // Keep list of supported brushes
        internal PropertyInfo[] m_BrushesProps = typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static);

        // Thickness of sample strokes
        internal double m_PolygonStrokeThickness = DefaultPolygonStrokeThickness;
        internal double m_LineStrokeThickness = DefaultLineStrokeThickness;
        internal double m_PointsStrokeThickness = DefaultPointsStrokeThickness;

        // Current PointSymbol
        internal PointSymbol m_PointSymbol = DefaultPointSymbol;
        internal double m_PointSymbolSize = DefaultPointSymbolSize;

        // Transparency of Stroke and Fill areas
        private byte m_Transparency = DefaultStrokeTransparency;
        internal byte m_StrokeTransparency = DefaultStrokeTransparency;
        internal byte m_FillTransparency = DefaultFillTransparency;

        // The opacity of the switched off sample (0..255).
        internal byte m_SampleOffTransparency = DefaultSampleOffTransparency;

        // Zoom factor of canvas
        private double m_ZoomFactor = 1.0;

        // Which colors and transparencies to set
        private bool m_SetStroke = false;
        private bool m_SetFill = false;

        // Google Maps Webbrowser
        // WebBrowser m_WebBrowser = new WebBrowser();
        private WebView2 m_WebBrowser = new WebView2();
        private bool m_WebBrowserNavigated = false;
        private string m_WebBrowserPath = string.Empty;
        private string m_WebView2Coordinates = string.Empty;
        private bool m_WebView2Coordinates_Received = false;

        // Preset Google Maps start location and settings
        private double m_WebBrowserPosLon = 11.500093;
        private double m_WebBrowserPosLat = 48.160234;
        private int m_WebZoomlevel = 5;
        private int m_WebScan = 0;
        private int m_WebMapType = 3;
        // Points to be displayed as Pins in WebBrowser (for debug purpose)
        private PointCollection m_WebBrowserPoints = null;
        // Settings depending on selected MapModeViewer
        private string m_MapModeViewerPath = MapModeViewerOSM;
        private string m_MapModeViewerPath1 = MapModeViewer1OSM;
        private int m_WebBrowserAdaptHeight = WebBrowserAdaptHeightOSM;
        private int m_WebBrowserAdaptWidth = WebBrowserAdaptWidthOSM;

        // Flag for disabling the Delete button in case of reference map, if set to false by interface function
        private bool m_WpfShowRefMapDelButton = true;

        // Adapt cursor array
        private Cursor[] m_AdaptCursors = new Cursor[7];
        private int m_CurrentCursorSource = 0;
        private int m_CurrentCursorTarget = 1;
        // Shift custom cursor
        private Cursor m_ShiftCursor;

        // Capture frame for saving working area
        private CaptureFrame m_CaptureFrame = null;
        // Size of the frame
        internal double m_AreaFrameWidth = 100;
        internal double m_AreaFrameHeight = 100;
        internal Point m_LastFramePosition = new Point(0, 0);
        internal double m_LastFrameWidth = 0;
        internal double m_LastFrameHeight = 0;
        private CaptureFrameMode m_CaptureFrameMode = CaptureFrameMode.ResizeBottomRight;
        // Move frame
        private bool m_CaptureFrameMove = false;

        // Show warning if map too large for exact coords
        private bool m_LatLonWarning = false;
        // Stop long term actions flag
        bool m_ProgressCancel = false;
        // Preset maximum count of progress update values (~ 100%)
        int m_ProgressMaxCount = 0;
        // Step in percent to update the bar
        int m_ProgressUpdateStep = 5;
        int m_ProgressStepCounter = 0;
        // Progress counter (0 - maximum)
        int m_ProgressCount = 0;

        private string m_HelpFile = string.Empty;
        private System.Diagnostics.Process m_HelpProcess = null;

        // EnvelopeCenter of Point collection
        internal bool m_EnvelopeCenterEnabled = false;
        internal double m_EnvelopeCenterXmin = 0;
        internal double m_EnvelopeCenterXmax = 0;
        internal double m_EnvelopeCenterYmin = 0;
        internal double m_EnvelopeCenterYmax = 0;
        internal double m_EnvelopeCenterX = 0;
        internal double m_EnvelopeCenterY = 0;

        private static string _UserDataFolder = null;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// ViewerMode
        /// </summary>
        public bool ViewerMode
        {
            get { return m_ViewerMode; }
            set { m_ViewerMode = value; ShowCaptureFrame(!value); }
        }

        #endregion // Properties

        #region Methods

        #region Construction

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WpfControl"/> class for a certain culture info.
        /// </summary>
        public WpfControl(string cultureInfo)
        {
            // Specific localization could be added here: e.g. "en-US"
            CultureInfo ci = new CultureInfo(cultureInfo);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo);
            InitializeWebView();
            Initialize();
        }

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="WpfControl"/> class.
        /// </summary>
        public WpfControl()
        {
            InitializeWebView();
            Initialize();
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="WpfControl"/> is reclaimed by garbage collection.
        /// </summary>
        ~WpfControl()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: false);

            WpfSamplingPlotPage.Properties.Settings.Default.WebBrowserPosLon = m_WebBrowserPosLon;
            WpfSamplingPlotPage.Properties.Settings.Default.WebBrowserPosLat = m_WebBrowserPosLat;
            WpfSamplingPlotPage.Properties.Settings.Default.SampleOffTransparency = m_SampleOffTransparency;
        }

        private bool disposedValue;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="WpfControl"/> is reclaimed by garbage collection.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                try
                {
                    if (m_Gps != null)
                    {
                        m_Gps.CloseGpsSerialPort();
                        m_Gps = null;
                    }
                    if (m_WebBrowser != null)
                    {
                        m_WebBrowser.Dispose();
                        m_WebBrowser = null;
                    }

                    // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                    // TODO: Große Felder auf NULL setzen
                    disposedValue = true;
                }
                catch (Exception)
                { }
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="WpfControl"/> is reclaimed by garbage collection.
        /// </summary>
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Initializes WebView.
        /// </summary>
        private async void InitializeWebView(string url = null)
        {
#if DEBUG
            // Start of form designer crashes if async init is done, therefore do not perform this in debug mode
            return;
#endif

            // Set the user data location folder to temp folder
            // MessageBox.Show("InitializeWebView");
            if (_UserDataFolder == null)
                _UserDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DiversityWorkbench\NETWebView2";

            var env = await CoreWebView2Environment.CreateAsync(null, _UserDataFolder);
            await m_WebBrowser.EnsureCoreWebView2Async(env);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            m_WebBrowserPosLon = (double)WpfSamplingPlotPage.Properties.Settings.Default.WebBrowserPosLon;
            m_WebBrowserPosLat = (double)WpfSamplingPlotPage.Properties.Settings.Default.WebBrowserPosLat;
            m_ViewerMode = false;
            // Initialize WPF Control
            InitializeComponent();

            try
            {
                // Add event handlers
                m_WebBrowser.WebMessageReceived += new System.EventHandler<CoreWebView2WebMessageReceivedEventArgs>(M_WebBrowser_WebMessageReceived);
                m_WebBrowser.NavigationStarting += M_WebBrowser_NavigationStarting;
                m_WebBrowser.NavigationCompleted += M_WebBrowser_NavigationCompleted;

                m_Canvas.MouseLeftButtonDown += new MouseButtonEventHandler(m_Canvas_MouseLeftButtonDown);
                m_Canvas.MouseRightButtonDown += new MouseButtonEventHandler(m_Canvas_MouseRightButtonDown);
                m_Canvas.MouseLeftButtonUp += new MouseButtonEventHandler(m_Canvas_MouseLeftButtonUp);
                m_Canvas.MouseMove += new MouseEventHandler(m_Canvas_MouseMove);
                m_Canvas.MouseLeave += new MouseEventHandler(m_Canvas_MouseLeave);
                m_UserControl.SizeChanged += new SizeChangedEventHandler(m_UserControl_SizeChanged);
                // m_Canvas.LayoutUpdated += new EventHandler(m_Canvas_LayoutUpdated);
                // m_WebBrowser.LayoutUpdated += new EventHandler(m_WebBrowser_LayoutUpdated);

                // Set zoom slider region (factor 0.6 to 3)
                sliderZoom.Minimum = SliderZoomMin;
                sliderZoom.Maximum = SliderZoomMax;
                sliderZoom.Value = 1.0;

                // Stroke and Fill flags
                m_SetStroke = checkBoxStroke.IsChecked.Value;
                m_SetFill = checkBoxFill.IsChecked.Value;

                // Fill colors combobox
                BrushesBoxFactory();

                // Set button icons
                buttonLoad.Content = GetImageFromFile("OpenFolder.ico");
                buttonSave.Content = GetImageFromFile("Save.ico");
                buttonAdd.Content = GetImageFromFile("Add.ico");
                buttonPrint.Content = GetImageFromFile("Print.ico");
                buttonDeleteAll.Content = GetImageFromFile("Delete.ico");
                buttonSettings.Content = GetImageFromFile("Settings.ico");
                buttonDetection.Content = GetImageFromFile("Detection.ico");
                buttonGps.Content = GetImageFromFile("Gps.ico");

                // Reset ID and Text boxes
                ResetIdentifierText(string.Empty, string.Empty, m_BrushHighlight);

                // Set tooltips for buttons
                buttonLoad.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonLoad);
                buttonSave.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonSave);
                buttonAdd.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonAdd);
                // Avoid capture of the Add button tooltip when freezing the map
                (buttonAdd.ToolTip as ToolTip).VerticalOffset = -30;
                buttonPrint.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonPrint);
                buttonDeleteAll.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonDeleteAll);
                buttonGps.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonGps);
                buttonSettings.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonSettings);
                buttonDetection.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonDetection);
                radioButtonShift.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttRadioButtonShift);
                radioButtonMap.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttRadioButtonMap);
                radioButtonEdit.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttRadioButtonEdit);
                radioButtonAdapt.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttRadioButtonAdapt);
                radioButtonDrawPolygon.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttRadioButtonArea);
                radioButtonDrawLine.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttRadioButtonLine);
                radioButtonDrawObjects.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttRadioButtonObjects);
                buttonMultiShape.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonMultiShape);
                checkBoxStroke.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxStroke);
                checkBoxFill.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttCheckBoxFill);
                comboBoxBrushes.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttComboBoxBrushes);
                sliderZoom.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttSliderZoom);
                progressBarSaveSamples.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttProgressBarSaveSamples);
                buttonProgressCancel.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonProgressCancel);


                // Start editor with Map Mode
                radioButtonMap.IsChecked = true;

                // Save original Cursor appearance
                m_CursorOrg = Cursors.Arrow;

                CustomCursorsLoad();

                // Set GIS Editor flags according to saved Settings
                m_ReadArcViewShapeFormatUtm = WpfSamplingPlotPage.Properties.Settings.Default.ReadArcViewShapeFormatUtm;
                m_ReadArcViewShapeFormatUtmZone = WpfSamplingPlotPage.Properties.Settings.Default.UtmZone;
                m_ReadArcViewShapeFormatUtmHemi = WpfSamplingPlotPage.Properties.Settings.Default.UtmHemi;
                m_SplitShapes = WpfSamplingPlotPage.Properties.Settings.Default.SplitShapes;
                m_WriteShapesToSqlFile = WpfSamplingPlotPage.Properties.Settings.Default.WriteShapesToSqlFile;
                m_WriteShapesToArcFile = WpfSamplingPlotPage.Properties.Settings.Default.WriteShapesToArcFile;
                m_WriteShapesToImportFile = WpfSamplingPlotPage.Properties.Settings.Default.WriteShapesToImportFile;
                m_WriteImageToKalFile = WpfSamplingPlotPage.Properties.Settings.Default.WriteImageToKalFile;
                m_SaveWorkingArea = WpfSamplingPlotPage.Properties.Settings.Default.SaveWorkingArea;
                m_TrackGpsData = WpfSamplingPlotPage.Properties.Settings.Default.GpsTrack;

                // DISABLED - only ~ 33 calls in sequence are successfull, not suitable for many points !!
                m_GetAltitude = WpfSamplingPlotPage.Properties.Settings.Default.GetAltitude;

                m_PolygonStrokeThickness = WpfSamplingPlotPage.Properties.Settings.Default.PolygonStrokeThickness;
                m_LineStrokeThickness = WpfSamplingPlotPage.Properties.Settings.Default.LineStrokeThickness;
                m_PointsStrokeThickness = WpfSamplingPlotPage.Properties.Settings.Default.PointsStrokeThickness;
                SetStrokeThickness();
                m_PointSymbol = (PointSymbol)WpfSamplingPlotPage.Properties.Settings.Default.PointSymbol;
                m_PointSymbolSize = WpfSamplingPlotPage.Properties.Settings.Default.PointSymbolSize;
                SetPointSymbol();
                m_PolygonStrokeThickness = WpfSamplingPlotPage.Properties.Settings.Default.PolygonStrokeThickness;
                m_PolygonStrokeThickness = WpfSamplingPlotPage.Properties.Settings.Default.PolygonStrokeThickness;
                // if (!m_SaveWorkingArea)
                //     m_AreaFrame = false;
                // else
                m_AreaFrame = WpfSamplingPlotPage.Properties.Settings.Default.AreaFrame;
                m_AreaFrameWidth = (double)WpfSamplingPlotPage.Properties.Settings.Default.AreaFrameWidth;
                m_AreaFrameHeight = (double)WpfSamplingPlotPage.Properties.Settings.Default.AreaFrameHeight;
                SetCaptureFrame();
                m_MapModeViewer = WpfSamplingPlotPage.Properties.Settings.Default.MapModeViewer;
                SetMapModeViewer(m_MapModeViewer);
                m_GpsBaudrate = WpfSamplingPlotPage.Properties.Settings.Default.Baudrate;

                // Set detection parameters according to saved Settings
                m_LimitRedMin = WpfSamplingPlotPage.Properties.Settings.Default.LimitRedMin;
                m_LimitGreenMin = WpfSamplingPlotPage.Properties.Settings.Default.LimitGreenMin;
                m_LimitBlueMin = WpfSamplingPlotPage.Properties.Settings.Default.LimitBlueMin;
                m_LimitGreyMin = WpfSamplingPlotPage.Properties.Settings.Default.LimitGreyMin;
                m_LimitRedMax = WpfSamplingPlotPage.Properties.Settings.Default.LimitRedMax;
                m_LimitGreenMax = WpfSamplingPlotPage.Properties.Settings.Default.LimitGreenMax;
                m_LimitBlueMax = WpfSamplingPlotPage.Properties.Settings.Default.LimitBlueMax;
                m_LimitGreyMax = WpfSamplingPlotPage.Properties.Settings.Default.LimitGreyMax;
                m_PointDistanceMin = WpfSamplingPlotPage.Properties.Settings.Default.PointDistanceMin;
                m_SeparateSamples = WpfSamplingPlotPage.Properties.Settings.Default.SeparateSamples;
                m_IdText = WpfSamplingPlotPage.Properties.Settings.Default.IdText;
                m_IdNumber = WpfSamplingPlotPage.Properties.Settings.Default.IdNumber;

                // Set Assignment parameters according to saved Settings
                m_Assignment[0] = WpfSamplingPlotPage.Properties.Settings.Default.Assign0;
                m_Assignment[1] = WpfSamplingPlotPage.Properties.Settings.Default.Assign1;
                m_Assignment[2] = WpfSamplingPlotPage.Properties.Settings.Default.Assign2;
                m_Assignment[3] = WpfSamplingPlotPage.Properties.Settings.Default.Assign3;
                m_Assignment[4] = WpfSamplingPlotPage.Properties.Settings.Default.Assign4;
                m_Assignment[5] = WpfSamplingPlotPage.Properties.Settings.Default.Assign5;
                m_Assignment[6] = WpfSamplingPlotPage.Properties.Settings.Default.Assign6;
                m_Assignment[7] = WpfSamplingPlotPage.Properties.Settings.Default.Assign7;
                m_Assignment[8] = WpfSamplingPlotPage.Properties.Settings.Default.Assign8;
                m_Assignment[9] = WpfSamplingPlotPage.Properties.Settings.Default.Assign9;
                m_Assignment[10] = WpfSamplingPlotPage.Properties.Settings.Default.Assign10;
                m_Assignment[11] = WpfSamplingPlotPage.Properties.Settings.Default.Assign11;
                m_Assignment[12] = WpfSamplingPlotPage.Properties.Settings.Default.Assign12;
                m_CenterPoint = WpfSamplingPlotPage.Properties.Settings.Default.CenterPoint;

                m_SampleOffTransparency = WpfSamplingPlotPage.Properties.Settings.Default.SampleOffTransparency;

                // Get path of executable
                m_HelpFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\DiversityGisEditor.chm";
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }


        /// <summary>
        /// Loads the custom cursors.
        /// </summary>
        private void CustomCursorsLoad()
        {
            m_AdaptCursors[0] = new Cursor(new MemoryStream(WpfSamplingPlotPage.Properties.Resources._1g_32));
            m_AdaptCursors[1] = new Cursor(new MemoryStream(WpfSamplingPlotPage.Properties.Resources._1r_32));
            m_AdaptCursors[2] = new Cursor(new MemoryStream(WpfSamplingPlotPage.Properties.Resources._2g_32));
            m_AdaptCursors[3] = new Cursor(new MemoryStream(WpfSamplingPlotPage.Properties.Resources._2r_32));
            m_AdaptCursors[4] = new Cursor(new MemoryStream(WpfSamplingPlotPage.Properties.Resources._3g_32));
            m_AdaptCursors[5] = new Cursor(new MemoryStream(WpfSamplingPlotPage.Properties.Resources._3r_32));
            m_AdaptCursors[6] = new Cursor(new MemoryStream(WpfSamplingPlotPage.Properties.Resources.fin_32));
            m_ShiftCursor = new Cursor(new MemoryStream(WpfSamplingPlotPage.Properties.Resources._3dsmove));
        }



        /// <summary>
        /// Sets a tool tip with given text.
        /// </summary>
        /// <param name="text">The text of the tool tip.</param>
        /// <returns></returns>
        internal ToolTip SetToolTip(string text)
        {
            text = text.Replace("\\r\\n", "\r\n");
            ToolTip tt = new ToolTip();
            // tt.FontFamily = new FontFamily("Comic Sans MS");
            tt.Content = text;
            return tt;
        }

        /// <summary>
        /// Creates a combo box keeping a list of all defined Windows Color-Brushes.
        /// </summary>
        private void BrushesBoxFactory()
        {
            // For all static brushes
            foreach (PropertyInfo prop in m_BrushesProps)
            {
                // Create combobox item
                ComboBoxItem item = new ComboBoxItem();
                // Add current brush name
                item.Content = prop.Name;
                // Get brush from list
                SolidColorBrush brush = (SolidColorBrush)prop.GetValue(null, null);
                // Set item background to brush color
                item.Background = brush;
                // Set foreground white or black, according to background luminance
                bool isBlack = 0.222 * brush.Color.R + 0.707 * brush.Color.G + 0.071 * brush.Color.G > 128;
                item.Foreground = isBlack ? Brushes.Black : Brushes.White;
                // Add item to combobox
                comboBoxBrushes.Items.Add(item);
                // If item is current brush, set selection to it
                if (brush == m_Brush)
                {
                    comboBoxBrushes.SelectedItem = item;
                }
            }
        }

        /// <summary>
        /// Sets the current brush by name.
        /// </summary>
        /// <param name="name">The name of the Brush.</param>
        /// <returns></returns>
        private SolidColorBrush SetBrushFromName(string name)
        {
            foreach (PropertyInfo prop in m_BrushesProps)
            {
                if (prop.Name == name)
                {
                    m_Brush = (SolidColorBrush)prop.GetValue(null, null);
                    break;
                }
            }

            return m_Brush;
        }

        /// <summary>
        /// Returns the name of a given brush.
        /// </summary>
        /// <param name="brush">The brush.</param>
        /// <returns>Name of the brush.</returns>
        internal string SetNameOfBrush(SolidColorBrush brush)
        {
            string name = string.Empty;
            foreach (PropertyInfo prop in m_BrushesProps)
            {
                if ((SolidColorBrush)prop.GetValue(null, null) == brush)
                {
                    name = prop.Name;
                    break;
                }
            }

            return name;
        }

        #endregion // Construction

        #region WebBrowser

        /// <summary>
        /// Add the Webbrowser to the canvas.
        /// </summary>
        private void WebBrowserStart()
        {
            // WLine("WebBrowserStart()");
            m_Canvas.Children.Add(m_WebBrowser);
        }

        /// <summary>
        /// Remove the Webbrowser from the canvas.
        /// </summary>
        private void WebBrowserStop()
        {
            // WLine("WebBrowserStop()");
            m_Canvas.Children.Remove(m_WebBrowser);
        }

        /// <summary>
        /// Adapt the Webbrowser size to fill completely the user control working area.
        /// Set position, zoom level, map type and other parameters for the map display.
        /// </summary>
        private void WebBrowserAdapt()
        {
            try
            {
                double height = 0;
                double width = 0;

                // Adapt Webbrowser size to hosting control
                if (m_Canvas.Parent == m_CanvasClip)
                {
                    height = m_UserControl.ActualHeight - m_WebBrowserAdaptHeight;
                    width = m_UserControl.ActualWidth - m_WebBrowserAdaptWidth;
                }
                else
                {
                    if (m_CanvasParent != null)
                    {
                        height = m_CanvasParent.Height - m_CanvasParent.Margin.Top - m_CanvasParent.Margin.Bottom - 6;
                        width = m_CanvasParent.Width + m_WebBrowserWidthDiff - m_CanvasParent.Margin.Left - m_CanvasParent.Margin.Right - 4;
                    }
                    else
                    {
                        height = m_UserControl.ActualHeight + WebBrowserBorder;
                        width = m_UserControl.ActualWidth + m_WebBrowserWidthDiff - WebBrowserBorder;
                    }
                }

                if (height <= 0 || width <= 0)
                    return;

                // WLine("WebBrowserAdapt( " + height.ToString() + ", " + width.ToString() + " )");
                if (m_WebBrowserPoints == null)
                {
                    m_WebBrowserPath = string.Format(m_MapModeViewerPath,
                        m_WebBrowserPosLat.ToString("F6", CultureInfo.InvariantCulture),
                        m_WebBrowserPosLon.ToString("F6", CultureInfo.InvariantCulture),
                        ((int)height - WebBrowserHeightDiff).ToString(),
                        ((int)width - m_WebBrowserWidthDiff).ToString(),
                        m_WebZoomlevel,
                        m_WebScan,
                        m_WebMapType);

                    // m_WebBrowserPath = "E:/_Subversion/WebPages/wwwroot/SnsbInfo/GoogleMaps/TestV3GoogleMaps.html";
                }
                else
                {
                    // Test only: set reference points to webbrowser map
                    WebBrowserAddPoints(height, width);
                }
                m_WebBrowser.Source = new Uri(m_WebBrowserPath, UriKind.RelativeOrAbsolute);
                m_WebBrowser.Height = height;
                m_WebBrowser.Width = width;
                // WLine("calling --> WebBrowserAdapt(" + height.ToString() + ", " + width.ToString() + "): " + m_WebBrowserPath);
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Sets the Webbrowser parameters to show the location of all points of a list.
        /// </summary>
        /// <param name="height">The height of the browser.</param>
        /// <param name="width">The width of the browser.</param>
        private void WebBrowserAddPoints(double height, double width)
        {
            m_WebBrowserPath = m_MapModeViewerPath1;
            foreach (Point p1 in m_WebBrowserPoints)
            {
                m_WebBrowserPath += p1.Y.ToString("F6", CultureInfo.InvariantCulture) + "|";
            }
            m_WebBrowserPath = m_WebBrowserPath.TrimEnd('|');
            m_WebBrowserPath += WebBrowserPath2;
            foreach (Point p2 in m_WebBrowserPoints)
            {
                m_WebBrowserPath += p2.X.ToString("F6", CultureInfo.InvariantCulture) + "|";
            }
            m_WebBrowserPath = m_WebBrowserPath.TrimEnd('|');
            m_WebBrowserPath += WebBrowserPath3 + "1";
            for (int ind = 2; ind <= m_WebBrowserPoints.Count; ind++)
            {
                m_WebBrowserPath += "|" + ind.ToString();
            }
            m_WebBrowserPath += string.Format(WebBrowserPath4,
                ((int)height - WebBrowserHeightDiff).ToString(),
                ((int)width - m_WebBrowserWidthDiff).ToString(),
                m_WebScan.ToString(),
                m_WebMapType.ToString());

            // Clear points again
            m_WebBrowserPoints = null;
        }

        #endregion // WebBrowser

        #region Event handlers

        #region Radio button handlers

        /// <summary>
        /// Handles the Checked event of the radioButtonShift control.
        /// Switches GIS Editor to ShiftCanvas mode, updates slider, status, instructions and mouse cursor image.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonShift_Checked(object sender, RoutedEventArgs e)
        {
            // WLine("-- radioButtonShift_Checked");
            // Switch mouse mode to shift the image
            m_MouseMode = MouseMode.ShiftCanvas;
            UpdateSlider();
            StatusPos(m_Zero);
            UpdateWebBrowserUriByPos();
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrShiftCanvasModeInstruction, false);
            m_Canvas.Cursor = m_ShiftCursor; // Cursors.ScrollAll;
            SetCaptureFrame();
            if (m_ViewerMode)
                ShowCaptureFrame(false);
            else
                ShowCaptureFrame(true);

            if (m_CaptureFrame != null)
                m_CaptureFrame.BringToTop();
        }

        /// <summary>
        /// Handles the Checked event of the radioButtonEdit control.
        /// Switches GIS Editor to EditSamples mode, updates slider, instructions and mouse cursor image.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonEdit_Checked(object sender, RoutedEventArgs e)
        {
            // Switch mouse mode to edit the samples
            m_MouseMode = MouseMode.EditSamples;
            UpdateSlider();
            ResetPickPoints();
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrEditSamplesModeInstruction, false);
            m_Canvas.Cursor = Cursors.Arrow;
            ShowCaptureFrame(false);
        }

        /// <summary>
        /// Handles the Checked event of the radioButtonDrawPolygon control.
        /// Switches GIS Editor to DrawPolygon mode, updates slider, instructions and mouse cursor image.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonDrawPolygon_Checked(object sender, RoutedEventArgs e)
        {
            // Switch mouse mode to draw a Polygon
            m_MouseMode = MouseMode.DrawPolygon;
            // Clear non saved objects
            CheckUnsavedSamples();
            UpdateSlider();
            ResetPickPoints();
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrDrawPolygonModeInstruction, false);
            m_Canvas.Cursor = Cursors.Cross;
            ShowCaptureFrame(false);
        }

        /// <summary>
        /// Handles the Checked event of the radioButtonDrawLine control.
        /// Switches GIS Editor to DrawLine mode, updates slider, instructions and mouse cursor image.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonDrawLine_Checked(object sender, RoutedEventArgs e)
        {
            // Switch mouse mode to draw a Polygon
            m_MouseMode = MouseMode.DrawLine;
            // Clear non saved objects
            CheckUnsavedSamples();
            UpdateSlider();
            ResetPickPoints();
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrDrawLineModeInstruction, false);
            m_Canvas.Cursor = Cursors.Cross;
            ShowCaptureFrame(false);
        }

        /// <summary>
        /// Handles the Checked event of the radioButtonDrawObjects control.
        /// Switches GIS Editor to DrawSamplePoints mode, updates slider, instructions and mouse cursor image.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonDrawObjects_Checked(object sender, RoutedEventArgs e)
        {
            // Switch mouse mode to draw objects (points)
            m_MouseMode = MouseMode.DrawSamplePoints;
            // Clear non saved Polygon
            CheckUnsavedSamples();
            UpdateSlider();
            ResetPickPoints();
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrDrawSamplePointsModeInstruction, false);
            m_Canvas.Cursor = Cursors.Cross;
            ShowCaptureFrame(false);
        }

        /// <summary>
        /// Handles the Checked event of the radioButtonAdapt control.
        /// Switches GIS Editor to MapAdapt mode, updates slider, instructions and mouse cursor image.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonAdapt_Checked(object sender, RoutedEventArgs e)
        {
            m_MouseMode = MouseMode.MapAdapt;
            if (m_SampleImage == null)
                SetError(WpfSamplingPlotPage.Properties.Resources.ErrNoAdaptImageLoaded);
            UpdateSlider();
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrMapAdaptModeSetInstruction, false);
            m_Canvas.Cursor = Cursors.Arrow;
            ShowCaptureFrame(false);
        }

        /// <summary>
        /// Handles the Unchecked event of the radioButtonAdapt control.
        /// No action at the moment.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonAdapt_Unchecked(object sender, RoutedEventArgs e)
        {
            // Clear Pickpoints and reset variables
            // ResetPickPoints();
        }

        /// <summary>
        /// Handles the Checked event of the radioButtonMap control.
        /// Switches GIS Editor to MapServer mode, disables edit controls, updates slider, status and instructions.
        /// Resets zoom and shift of the working area, handles reference coordinates and starts Webbrowser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonMap_Checked(object sender, RoutedEventArgs e)
        {
            // WLine("-- radioButtonMap_Checked");
            // Switch mouse mode to Webbrowser
            m_MouseMode = MouseMode.MapServer;
            if (m_SampleImage != null)
            {
                m_SampleImage.ClearSample();
                m_SampleImage = null;
                if (WorldHasDisappeared())
                    ClearCoordinates();
            }
            // Clear non saved objects
            CheckUnsavedSamples();
            // Reset Zoom, margin, pick points
            m_Canvas.Margin = m_CanvasThickness = m_CanvasThicknessZero;
            m_ZoomFactor = 1;
            UpdateSlider();
            StatusPos(m_Zero);
            ResetPickPoints();

            // Disable all other radio buttons, if no ref map 
            if (m_RefMap == null)
                EnableEditControls(false);
            // Write instruction label
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrMapServerModeInstruction, false);

            m_WebScan = 0;
            // Start Webbrowser
            WebBrowserStart();
            // Adapt Webbrowser size to hosting control
            WebBrowserAdapt();
            ShowCaptureFrame(false);
        }

        /// <summary>
        /// Handles the Unchecked event of the radioButtonMap control.
        /// Stops Webbrowser and enables edit controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void radioButtonMap_Unchecked(object sender, RoutedEventArgs e)
        {
            // Stop Webbrowser
            WebBrowserStop();

            // Enable all other radio buttons
            EnableEditControls(true);
            // Clear instruction label
            SetInstructionLabel(string.Empty, false);
        }

        /// <summary>
        /// Checks unsaved samples and asks, if they should be added to the sample list or discarded.
        /// </summary>
        private void CheckUnsavedSamples()
        {
            if ((m_SamplePoints != null && m_MouseMode != MouseMode.DrawSamplePoints) ||
                (m_SampleLine != null && m_MouseMode != MouseMode.DrawLine) ||
                (m_SamplePolygon != null && m_MouseMode != MouseMode.DrawPolygon) ||
                (m_SampleImage != null))
            {
                if (SetRequest(WpfSamplingPlotPage.Properties.Resources.ErrSaveSample))
                {
                    // Temporarily change the MouseMode to avoid in case of "MapServer"-Mode the unwanted capturing of the Viewer when saving the unsaved Sample
                    MouseMode mMode = m_MouseMode;
                    m_MouseMode = MouseMode.ShiftCanvas;
                    buttonAddShape_Click(null, null);
                    // Sample has been saved, resume the MouseMode
                    m_MouseMode = mMode;
                }
                else
                    ClearUnsavedSamples();
            }
        }

        /// <summary>
        /// Clears the unsaved samples.
        /// </summary>
        private void ClearUnsavedSamples()
        {
            if (m_SamplePoints != null && m_MouseMode != MouseMode.DrawSamplePoints)
            {
                m_SamplePoints.ClearSample();
                m_SamplePoints = null;
            }
            if (m_SampleLine != null && m_MouseMode != MouseMode.DrawLine)
            {
                m_SampleLine.ClearSample();
                m_SampleLine = null;
            }
            if (m_SamplePolygon != null && m_MouseMode != MouseMode.DrawPolygon)
            {
                m_SamplePolygon.ClearSample();
                m_SamplePolygon = null;
            }
            if (m_SampleImage != null)
            {
                m_SampleImage.ClearSample();
                m_SampleImage = null;
                if (WorldHasDisappeared())
                    ClearCoordinates();
            }
        }

        /// <summary>
        /// Updates the zoom/transparency slider and status depending on the mode.
        /// </summary>
        private void UpdateSlider()
        {
            if (sliderZoom == null)
                return;

            try
            {
                switch (m_MouseMode)
                {
                    case MouseMode.ShiftCanvas:
                        sliderZoom.Value = m_ZoomFactor;
                        break;
                    case MouseMode.MapServer:
                        SetSliderLabel();
                        break;
                    default:
                        sliderZoom.Value = (double)(m_Transparency + 0.5) / (255.0 / (SliderZoomMax - SliderZoomMin)) + SliderZoomMin;
                        break;
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }

        }

        /// <summary>
        /// Displays the slider status.
        /// </summary>
        private void SetSliderLabel()
        {
            if (labelSlider == null)
                return;
            try
            {
                switch (m_MouseMode)
                {
                    case MouseMode.ShiftCanvas:
                        labelSlider.Content = WpfSamplingPlotPage.Properties.Resources.StrZoom + "  " + m_ZoomFactor.ToString("F3");
                        break;
                    case MouseMode.MapServer:
                        labelSlider.Content = string.Empty;
                        break;
                    default:
                        labelSlider.Content = WpfSamplingPlotPage.Properties.Resources.StrTransparency + "  " + (m_Transparency / 2.55).ToString("F0") + " %"; ;
                        break;
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Displays the instruction line.
        /// </summary>
        /// <param name="str">The text of the instruction.</param>
        /// <param name="highlight">Highlight the text if set to <c>true</c>.</param>
        public void SetInstructionLabel(string str, bool highlight)
        {
            if (labelInstruction != null)
            {
                str = str.Replace("\\r\\n", "\r\n");
                labelInstruction.Content = str;
                if (highlight)
                {
                    labelInstruction.Foreground = Brushes.Red;
                    // labelInstruction.FontWeight = System.Windows.FontWeights.Bold;
                }
                else
                {
                    labelInstruction.Foreground = Brushes.Blue;
                    // labelInstruction.FontWeight = System.Windows.FontWeights.Normal;
                }
            }
        }

        /// <summary>
        /// Enables or disables the edit controls.
        /// </summary>
        /// <param name="enabled">Enables the edit controls if set to <c>true</c>, otherwise disables them.</param>
        private void EnableEditControls(bool enabled)
        {
            try
            {
                // TEST only
                // enabled = true;
                // if (radioButtonShift != null)
                //     radioButtonShift.IsEnabled = enabled;
                if (radioButtonEdit != null)
                    radioButtonEdit.IsEnabled = enabled;
                if (radioButtonAdapt != null)
                    radioButtonAdapt.IsEnabled = enabled;
                if (radioButtonDrawPolygon != null)
                    radioButtonDrawPolygon.IsEnabled = enabled;
                if (radioButtonDrawLine != null)
                    radioButtonDrawLine.IsEnabled = enabled;
                if (radioButtonDrawObjects != null)
                    radioButtonDrawObjects.IsEnabled = enabled;
                if (buttonMultiShape != null)
                    buttonMultiShape.IsEnabled = enabled;
                if (checkBoxStroke != null)
                    checkBoxStroke.IsEnabled = enabled;
                if (checkBoxFill != null)
                    checkBoxFill.IsEnabled = enabled;
                if (labelColor != null)
                    labelColor.IsEnabled = enabled;
                if (comboBoxBrushes != null)
                    comboBoxBrushes.IsEnabled = enabled;
                if (labelSlider != null)
                    labelSlider.IsEnabled = enabled;
                if (sliderZoom != null)
                    sliderZoom.IsEnabled = enabled;
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Enables or disables the draw controls (depending of the availability of world coordinates).
        /// </summary>
        /// <param name="enabled">Enables the drawing controls if set to <c>true</c>, otherwise disables them.</param>
        private void EnableDrawControls(bool enabled)
        {
            try
            {
                if (radioButtonAdapt != null)
                    radioButtonAdapt.IsEnabled = enabled;
                if (radioButtonDrawPolygon != null)
                    radioButtonDrawPolygon.IsEnabled = enabled;
                if (radioButtonDrawLine != null)
                    radioButtonDrawLine.IsEnabled = enabled;
                if (radioButtonDrawObjects != null)
                    radioButtonDrawObjects.IsEnabled = enabled;
                if (buttonMultiShape != null)
                    buttonMultiShape.IsEnabled = enabled;
                if (checkBoxStroke != null)
                    checkBoxStroke.IsEnabled = enabled;
                if (checkBoxFill != null)
                    checkBoxFill.IsEnabled = enabled;
                if (labelColor != null)
                    labelColor.IsEnabled = enabled;
                if (comboBoxBrushes != null)
                    comboBoxBrushes.IsEnabled = enabled;
                // if (radioButtonMap != null)
                //     radioButtonMap.IsEnabled = !enabled;
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        #endregion // Radio button handlers

        #region Check box handlers

        // use databinding instead of flags??

        /// <summary>
        /// Handles the Checked event of the checkBoxStroke control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxStroke_Checked(object sender, RoutedEventArgs e)
        {
            m_SetStroke = true;
        }

        /// <summary>
        /// Handles the Unchecked event of the checkBoxStroke control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxStroke_Unchecked(object sender, RoutedEventArgs e)
        {
            m_SetStroke = false;
        }

        /// <summary>
        /// Handles the Checked event of the checkBoxFill control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxFill_Checked(object sender, RoutedEventArgs e)
        {
            m_SetFill = true;
        }

        /// <summary>
        /// Handles the Unchecked event of the checkBoxFill control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void checkBoxFill_Unchecked(object sender, RoutedEventArgs e)
        {
            m_SetFill = false;
        }

        #endregion // Check box handlers

        #region Mouse button handlers

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the m_Canvas control.
        /// Keep mouse position and set points if in a drawing mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void m_Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Save current position
                m_LastPoint = e.GetPosition(m_Canvas);

                if (m_AreaFrame && m_CaptureFrame != null)
                {
                    m_LastFramePosition = m_CaptureFrame.FramePosition;
                    m_LastFrameWidth = m_CaptureFrame.FrameWidth;
                    m_LastFrameHeight = m_CaptureFrame.FrameHeight;
                }

                switch (m_MouseMode)
                {
                    case MouseMode.ShiftCanvas:
                        if (m_CaptureFrame != null && m_CaptureFrame.m_Frame.IsMouseOver)
                        {
                            SetCaptureFrameCursor(m_CaptureFrameMode = m_CaptureFrame.ChangeMode(m_LastPoint));
                            m_CaptureFrameMove = true;
                        }
                        break;

                    case MouseMode.DrawPolygon:
                        // Create sample line if none available
                        if (m_SamplePolygon == null)
                        {
                            m_SamplePolygon = new SamplePolygon(m_Canvas);
                            // Set first point to show the line when moving the mouse
                            m_SamplePolygon.SetSamplePoint(m_LastPoint);
                            // SetComboboxBrush();
                            m_SamplePolygon.StrokeBrush = m_LastStrokeBrush;
                            m_SamplePolygon.FillBrush = m_LastFillBrush;
                            SetStrokeThickness();
                            ResetCheckBoxes();
                        }
                        // Set next point of Polygon
                        m_SamplePolygon.SetSamplePoint(m_LastPoint);
                        break;

                    case MouseMode.DrawLine:
                        // Create sample line if none available
                        if (m_SampleLine == null)
                        {
                            m_SampleLine = new SampleLines(m_Canvas);
                            // Set first point to show the line when moving the mouse
                            m_SampleLine.SetSamplePoint(m_LastPoint);
                            // SetComboboxBrush();
                            m_SampleLine.StrokeBrush = m_LastStrokeBrush;
                            SetStrokeThickness();
                            ResetCheckBoxes();
                            m_DistPoint = m_LastPoint;
                        }
                        // Set next point of Polygon
                        m_SampleLine.SetSamplePoint(m_LastPoint);

                        // --Distance-- Show
                        ShowDistance(m_LastPoint);
                        // m_DistPoint = m_LastPoint;

                        break;

                    case MouseMode.DrawSamplePoints:
                        // Create shape if none available
                        if (m_SamplePoints == null)
                        {
                            m_SamplePoints = new SamplePoints(m_Canvas);
                            // SetComboboxBrush();
                            m_SamplePoints.StrokeBrush = m_LastStrokeBrush;
                            m_SamplePoints.FillBrush = m_LastFillBrush;
                            SetStrokeThickness();
                            m_SamplePoints.SamplePointSymbol = m_PointSymbol;
                            m_SamplePoints.SamplePointSymbolSize = m_PointSymbolSize;
                            ResetCheckBoxes();
                        }
                        // Set next point of sample points
                        m_SamplePoints.SetSamplePoint(m_LastPoint);

                        /* =================== TEST =====================
                        if (m_WebBrowserPoints == null)
                            m_WebBrowserPoints = new PointCollection();
                        m_WebBrowserPoints.Add( CanvasToWorld( m_LastPoint));
                        // ============================================== */

                        // =================== TEST =====================
                        // labelInstruction.Content = GetAltitude(CanvasToWorld(m_LastPoint).X, CanvasToWorld(m_LastPoint).Y);
                        // ==============================================
                        break;

                    case MouseMode.MapAdapt:
                        SetPickPoint(m_LastPoint);
                        break;

                    default:
                        break;
                }

                // Set flag for mouse button pressed
                m_MouseButtonPressed = true;
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Shows the distance of the last Linestring segment.
        /// </summary>
        private void ShowDistance(Point newPoint)
        {
            // --Distance-- 
            if (m_DistPoint != m_Zero)
            {
                double dist = Distance(CanvasToWorld(m_DistPoint), CanvasToWorld(newPoint));
                string unit = WpfSamplingPlotPage.Properties.Resources.StrUnitM;
                if (dist >= 1000)
                {
                    dist /= 1000;
                    unit = WpfSamplingPlotPage.Properties.Resources.StrUnitKm;
                }

                SetInstructionLabel(string.Format(WpfSamplingPlotPage.Properties.Resources.StrDistance, dist.ToString("F3")) + unit, false);
            }
        }

        /// <summary>
        /// Calculates the distance in meters out of 2 world coordinates.
        /// </summary>
        /// <param name="first">The first coordinate.</param>
        /// <param name="last">The last coordinate.</param>
        /// <returns></returns>
        private double Distance(Point first, Point last)
        {
            // Koordinaten umrechnen            
            Point start = first;
            Point stop = last;
            start.X = start.X / 180 * Math.PI;
            start.Y = start.Y / 180 * Math.PI;
            stop.X = stop.X / 180 * Math.PI;
            stop.Y = stop.Y / 180 * Math.PI;

            // Einheitskugel
            double orb1 = 0.0;
            // Distanz in Metern
            double dist = 0.0;

            // Einheitskugel
            orb1 = Math.Acos(Math.Sin(start.Y) * Math.Sin(stop.Y) + Math.Cos(start.Y) * Math.Cos(stop.Y) * Math.Cos(stop.X - start.X));
            // Multipliziert mit Äquatorradius 
            dist = orb1 * 6378137;

            return dist;
        }


        /// <summary>
        /// Resets the sample identifier and description text boxes to default strings.
        /// </summary>
        /// <param name="identifier">The identifier string.</param>
        /// <param name="text">The description string.</param>
        /// <param name="brush">The background brush.</param>
        private void ResetIdentifierText(string identifier, string text, SolidColorBrush brush)
        {
            SetIdentifierText(identifier, text);
            textBoxIdentifier.Background = brush;
            textBoxSampleText.Background = brush;
        }
        // ResetIdentifierText(StrSampleID + (m_SampleIdCount++).ToString(), StrSampleTextObjects, m_BrushHighlight);


        /// <summary>
        /// Sets the samples identifier and description text boxes.
        /// </summary>
        /// <param name="identifier">The identifier text.</param>
        /// <param name="text">The description text.</param>
        private void SetIdentifierText(string identifier, string text)
        {
            textBoxIdentifier.Text = identifier;
            textBoxSampleText.Text = text;
        }

        /// <summary>
        /// Sets the capture frame cursor according to position.
        /// </summary>
        /// <param name="captureFrameMode">The capture frame mode.</param>
        private void SetCaptureFrameCursor(CaptureFrameMode captureFrameMode)
        {
            switch (captureFrameMode)
            {
                default:
                case CaptureFrameMode.Move:
                    m_Canvas.Cursor = Cursors.Hand;
                    break;
                case CaptureFrameMode.ResizeBottomLeft:
                    m_Canvas.Cursor = Cursors.ScrollSW;
                    break;
                case CaptureFrameMode.ResizeTopRight:
                    m_Canvas.Cursor = Cursors.ScrollNE;
                    break;
                case CaptureFrameMode.ResizeBottomRight:
                    m_Canvas.Cursor = Cursors.ScrollSE;
                    break;
                case CaptureFrameMode.ResizeTopLeft:
                    m_Canvas.Cursor = Cursors.ScrollNW;
                    break;
            }
        }

        /// <summary>
        /// Handles the MouseMove event of the m_Canvas control.
        /// Shift working area, catches points or draw lines, depending on mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void m_Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                // Get current mouse coords
                Point newPoint = e.GetPosition(m_Canvas);
                // Display current screen and world coordinates in the label
                StatusPos(newPoint);

                switch (m_MouseMode)
                {
                    case MouseMode.ShiftCanvas:
                        if (!m_MouseButtonPressed)
                        {
                            // Select Cursor according to position
                            if (m_CaptureFrame != null && m_CaptureFrame.m_Frame.IsMouseOver)
                            {
                                SetCaptureFrameCursor(m_CaptureFrame.ChangeMode(newPoint));
                            }
                            else
                                m_Canvas.Cursor = m_ShiftCursor;
                        }
                        else if (m_CaptureFrameMove)
                        {
                            // Move capture frame
                            double xDif = newPoint.X - m_LastPoint.X;
                            double yDif = newPoint.Y - m_LastPoint.Y;

                            switch (m_CaptureFrameMode)
                            {
                                default:
                                case CaptureFrameMode.Move:
                                    m_CaptureFrame.FramePosition = new Point(m_LastFramePosition.X + xDif, m_LastFramePosition.Y + yDif);
                                    break;
                                case CaptureFrameMode.ResizeBottomLeft:
                                    m_AreaFrameWidth = m_LastFrameWidth - xDif;
                                    m_AreaFrameHeight = m_LastFrameHeight + yDif;
                                    m_CaptureFrame.SetSize(m_AreaFrameWidth, m_AreaFrameHeight);
                                    if (m_AreaFrameWidth > m_CaptureFrame.MinWidth)
                                        m_CaptureFrame.FramePosition = new Point(m_LastFramePosition.X + xDif, m_LastFramePosition.Y);
                                    break;
                                case CaptureFrameMode.ResizeBottomRight:
                                    m_AreaFrameWidth = m_LastFrameWidth + xDif;
                                    m_AreaFrameHeight = m_LastFrameHeight + yDif;
                                    m_CaptureFrame.SetSize(m_AreaFrameWidth, m_AreaFrameHeight);
                                    break;
                                case CaptureFrameMode.ResizeTopLeft:
                                    m_AreaFrameWidth = m_LastFrameWidth - xDif;
                                    m_AreaFrameHeight = m_LastFrameHeight - yDif;
                                    m_CaptureFrame.SetSize(m_AreaFrameWidth, m_AreaFrameHeight);
                                    if (m_AreaFrameWidth <= m_CaptureFrame.MinWidth)
                                        xDif = m_LastFrameWidth - m_CaptureFrame.MinWidth;
                                    if (m_AreaFrameHeight <= m_CaptureFrame.MinHeight)
                                        yDif = m_LastFrameHeight - m_CaptureFrame.MinHeight;
                                    m_CaptureFrame.FramePosition = new Point(m_LastFramePosition.X + xDif, m_LastFramePosition.Y + yDif);
                                    break;
                                case CaptureFrameMode.ResizeTopRight:
                                    m_AreaFrameWidth = m_LastFrameWidth + xDif;
                                    m_AreaFrameHeight = m_LastFrameHeight - yDif;
                                    m_CaptureFrame.SetSize(m_AreaFrameWidth, m_AreaFrameHeight);
                                    if (m_AreaFrameHeight > m_CaptureFrame.MinHeight)
                                        m_CaptureFrame.FramePosition = new Point(m_LastFramePosition.X, m_LastFramePosition.Y + yDif);
                                    break;
                            }
                            m_AreaFrameWidth = m_CaptureFrame.FrameWidth;
                            m_AreaFrameHeight = m_CaptureFrame.FrameHeight;
                        }
                        else if (m_Canvas.Parent == m_CanvasClip)
                        {
                            // Shift canvas
                            m_Canvas.Cursor = m_ShiftCursor;
                            // Compute shift vector
                            double xDif = newPoint.X - m_LastPoint.X;
                            double yDif = newPoint.Y - m_LastPoint.Y;

                            // Adapt canvas position
                            m_CanvasThickness.Left += xDif;
                            m_CanvasThickness.Right -= xDif;
                            m_CanvasThickness.Top += yDif;
                            m_CanvasThickness.Bottom -= yDif;
                            // Set new position
                            m_Canvas.Margin = m_CanvasThickness;
                            // Debug output
                            // labelPos.Content = m_CanvasThickness.Left.ToString() + " - " + m_CanvasThickness.Top.ToString();
                        }
                        break;

                    case MouseMode.DrawPolygon:
                        if (m_MouseButtonPressed && m_SamplePolygon != null)
                        {
                            // Show line from last point to current mouse position
                            m_SamplePolygon.ChangeSamplePoint(newPoint);
                        }
                        break;

                    case MouseMode.DrawLine:
                        if (m_MouseButtonPressed && m_SampleLine != null)
                        {
                            // Show line from last point to current mouse position
                            m_SampleLine.ChangeSamplePoint(newPoint);

                            // --Distance-- Show
                            ShowDistance(newPoint);
                        }
                        break;

                    case MouseMode.EditSamples:
                        if (m_MouseButtonPressed)
                        {
                            MoveSamplePoint(newPoint);
                        }
                        else
                        {
                            m_CatchedPointIndex = -1;
                            m_CatchedPoly = null;
                            m_CatchedLine = null;
                            m_CatchedSamplePoints = null;
                            CatchNextSamplePoint(newPoint);
                        }
                        break;

                    case MouseMode.MapAdapt:
                        if (m_SampleImage != null && m_SampleImage.m_Image != null && m_RefMap != null && m_RefMap.m_Image != null)
                        {
                            if (m_Canvas.InputHitTest(newPoint) == m_SampleImage.m_Image && m_SampleImage.Transparency > AdaptModeMinTransparency)
                                m_Canvas.Cursor = m_AdaptCursors[m_CurrentCursorSource];
                            else // if (m_Canvas.InputHitTest(newPoint) == m_RefMap.m_Image)
                                m_Canvas.Cursor = m_AdaptCursors[m_CurrentCursorTarget];
                        }
                        else
                        {
                            m_Canvas.Cursor = m_CursorOrg;
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Handles the MouseLeftButtonUp event of the m_Canvas control.
        /// Just resets the m_MouseButtonPressed flag.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void m_Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            m_MouseButtonPressed = false;
            m_CaptureFrameMove = false;
            // Release eventually catched Samples
            m_CatchedSamplePoints = null;
            m_CatchedPoly = null;
            m_CatchedLine = null;
            m_CatchedSampleImage = null;

            if (m_MouseMode == MouseMode.DrawLine)
            {
                // --Distance-- Show
                // ShowDistance();
                m_DistPoint = e.GetPosition(m_Canvas);
            }
        }

        /// <summary>
        /// Handles the MouseLeave event of the m_Canvas control.
        /// Just resets the m_MouseButtonPressed flag.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void m_Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            // Release image grabbing when leaving canvas area
            m_MouseButtonPressed = false;
        }

        /// <summary>
        /// Handles the MouseRightButtonDown event of the m_Canvas control.
        /// Clears the last point set, if in drawing mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void m_Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                switch (m_MouseMode)
                {
                    case MouseMode.DrawPolygon:
                        if (m_SamplePolygon != null)
                        {
                            // Clear last sample line
                            m_SamplePolygon.ClearSamplePoint();
                        }
                        break;

                    case MouseMode.DrawLine:
                        if (m_SampleLine != null)
                        {
                            // Clear last sample line
                            m_SampleLine.ClearSamplePoint();

                            // --Distance-- Reset
                            m_DistPoint = m_Zero;
                            SetInstructionLabel(string.Format(WpfSamplingPlotPage.Properties.Resources.StrDistance, "n/a"), false);
                        }
                        break;

                    case MouseMode.DrawSamplePoints:
                        if (m_SamplePoints != null)
                        {
                            // Clear last sample point
                            m_SamplePoints.ClearSamplePoint();
                        }
                        break;

                    case MouseMode.ShiftCanvas:
                        // Point pnt = e.GetPosition(m_Canvas);
                        // PixelColor(pnt);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }


        /// <summary>
        /// Displays the status line including icon, current screen and world coordinates, if any.
        /// Disables Save button if no world coordinates are present.
        /// </summary>
        /// <param name="screenPos">The screen pos.</param>
        private void StatusPos(Point screenPos)
        {
            try
            {
                // --Distance-- Reset
                if (m_MouseMode != MouseMode.DrawLine)
                    m_DistPoint = m_Zero;

                if (m_MouseMode == MouseMode.MapServer)
                {
                    labelPosX.Content = labelCordX.Content = labelPosY.Content = labelCordY.Content = string.Empty;
                    if (m_MapModeViewer == 0)
                        labelWorld.Content = GetImageFromFile("Google.bmp");
                    else
                        labelWorld.Content = GetImageFromFile("OSM.bmp");
                    buttonSave.IsEnabled = true;
                }
                else
                {
                    if (m_WorldTop != 0)
                    {
                        Point worldPos = CanvasToWorld(screenPos);
                        labelWorld.Content = GetImageFromFile("World.ico");
                        labelPosX.Content = "Position X:" + StrCR + "Longitude:";
                        labelCordX.Content = screenPos.X.ToString("F1") + StrCR + worldPos.X.ToString("F6");
                        labelPosY.Content = "Position Y:" + StrCR + "Latitude:";
                        labelCordY.Content = screenPos.Y.ToString("F1") + StrCR + worldPos.Y.ToString("F6");
                        buttonSave.IsEnabled = true;
                        EnableEditControls(true);
                    }
                    else if (labelWorld != null)
                    {
                        labelWorld.Content = GetImageFromFile("Screen.ico");
                        labelPosX.Content = "Position X:";
                        labelCordX.Content = screenPos.X.ToString("F1");
                        labelPosY.Content = "Position Y:";
                        labelCordY.Content = screenPos.Y.ToString("F1");
                        buttonSave.IsEnabled = false;
                        EnableEditControls(false);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion // Mouse button handlers

        #region Size and Slider handlers

        /// <summary>
        /// Handles the SizeChanged event of the m_UserControl control.
        /// Adapts the Webbrowser size if in MapServer mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void m_UserControl_SizeChanged(Object sender, SizeChangedEventArgs e)
        {
            // Adapt size of Webbrowser, if active
            if (m_MouseMode == MouseMode.MapServer)
            {
                UpdateWebBrowserUriByPos();
                // WLine("UserControl_SizeChanged: Height = " + e.NewSize.Height.ToString() + ", Width = " + e.NewSize.Width.ToString());
                WebBrowserAdapt();
            }
            // Adapt size of SampleList ScrollViewer
            double height = e.NewSize.Height - m_WebBrowserAdaptHeight - 2;
            m_SampleListScrollViewer.Height = (height > 0 ? height : 0);
        }

        /// <summary>
        /// Handles the ValueChanged event of the sliderZoom control.
        /// Sets working area zoom or sample transparency, depending on mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs instance containing the event data.</param>
        private void sliderZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (m_MouseMode == MouseMode.ShiftCanvas)
                {
                    m_ZoomFactor = sliderZoom.Value;
                    // Scale canvas according to zoom factor
                    m_Canvas.RenderTransform = new ScaleTransform(m_ZoomFactor, m_ZoomFactor);
                }
                else
                {
                    m_Transparency = (byte)((sliderZoom.Value - SliderZoomMin) * (255 / (SliderZoomMax - SliderZoomMin)));
                    // Change transparency (only if different)
                    if (m_SetStroke && m_Transparency != m_StrokeTransparency)
                        m_StrokeTransparency = m_Transparency;
                    if (m_SetFill && m_Transparency != m_FillTransparency)
                        m_FillTransparency = m_Transparency;
                    SetTransparency();
                }
                SetSliderLabel();
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the sliderZoom control.
        /// Prints an error if Stroke and Fill checkboxes are not selected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void sliderZoom_GotFocus(object sender, RoutedEventArgs e)
        {
            // Check if slider is in transparency mode and Fill is not selected
            if (m_MouseMode != MouseMode.ShiftCanvas && !m_SetFill)
            {
                // If Stroke is not selected, too, print an error
                if (!m_SetStroke)
                    SetError(WpfSamplingPlotPage.Properties.Resources.ErrNoCheckBoxSelectedTrans);
                // If current object is an image, print a warning (needs Fill selected to change transparency!)
                else if (m_SampleImage != null)
                    SetError(WpfSamplingPlotPage.Properties.Resources.ErrNoFillCheckBoxSelected);
            }
        }

        /// <summary>
        /// Sets the transparency of the current shape or image, or for all visible shapes, if in edit mode.
        /// </summary>
        private void SetTransparency()
        {
            // Set transparency for current polygon
            if (m_SamplePolygon != null)
            {
                if (m_SetStroke)
                    m_SamplePolygon.StrokeTransparency = m_StrokeTransparency;
                if (m_SetFill)
                    m_SamplePolygon.FillTransparency = m_FillTransparency;
            }
            // Set transparency for current polyline
            else if (m_SampleLine != null)
            {
                if (m_SetStroke)
                    m_SampleLine.StrokeTransparency = m_StrokeTransparency;
            }
            // Set transparency for current sample points
            else if (m_SamplePoints != null)
            {
                if (m_SetStroke)
                    m_SamplePoints.StrokeTransparency = m_StrokeTransparency;
                if (m_SetFill)
                    m_SamplePoints.FillTransparency = m_FillTransparency;
            }
            // Set transparency for current image
            else if (m_SampleImage != null)
            {
                m_SampleImage.Transparency = m_FillTransparency;
            }
            // Set transparency for all visible shapes if in Edit mode
            if (m_MouseMode == MouseMode.EditSamples)
            {
                foreach (Sample sample in m_SampleList)
                {
                    if (sample.IsSampleVisible)
                    {
                        switch (sample.TypeOfSample)
                        {
                            case SampleType.POLYGON:
                            case SampleType.MULTIPOLYGON:
                                if (m_SetStroke)
                                    (sample as SamplePolygon).StrokeTransparency = m_StrokeTransparency;
                                if (m_SetFill)
                                    (sample as SamplePolygon).FillTransparency = m_FillTransparency;
                                break;
                            case SampleType.LINESTRING:
                            case SampleType.MULTILINESTRING:
                                if (m_SetStroke)
                                    (sample as SampleLines).StrokeTransparency = m_StrokeTransparency;
                                break;
                            case SampleType.POINT:
                            case SampleType.MULTIPOINT:
                                if (m_SetStroke)
                                    (sample as SamplePoints).StrokeTransparency = m_StrokeTransparency;
                                if (m_SetFill)
                                    (sample as SamplePoints).FillTransparency = m_FillTransparency;
                                break;
                            case SampleType.IMAGE:
                                if (m_SetFill)
                                    if ((sample as SampleImage) != m_RefMap)
                                    {
                                        (sample as SampleImage).Transparency = m_FillTransparency;
                                    }
                                break;
                            default:
                                break;
                        }
                    }
                }
                UpdateToggleButtons();
            }
        }

        /// <summary>
        /// Sets the stroke thickness of the current shape, or for all visible shapes, if in edit mode.
        /// </summary>
        internal void SetStrokeThickness()
        {
            // Set thickness for current polygon
            if (m_SamplePolygon != null)
            {
                m_SamplePolygon.StrokeThickness = m_PolygonStrokeThickness;
            }
            // Set thickness for current polyline
            else if (m_SampleLine != null)
            {
                m_SampleLine.StrokeThickness = m_LineStrokeThickness;
            }
            // Set thickness for current sample points
            else if (m_SamplePoints != null)
            {
                m_SamplePoints.StrokeThickness = m_PointsStrokeThickness;
            }
            // Set thickness for all visible shapes if in Edit mode
            if (m_MouseMode == MouseMode.EditSamples)
            {
                foreach (Sample sample in m_SampleList)
                {
                    if (sample.IsSampleVisible)
                    {
                        switch (sample.TypeOfSample)
                        {
                            case SampleType.POLYGON:
                            case SampleType.MULTIPOLYGON:
                                (sample as SamplePolygon).StrokeThickness = m_PolygonStrokeThickness;
                                break;
                            case SampleType.LINESTRING:
                            case SampleType.MULTILINESTRING:
                                (sample as SampleLines).StrokeThickness = m_LineStrokeThickness;
                                break;
                            case SampleType.POINT:
                            case SampleType.MULTIPOINT:
                                (sample as SamplePoints).StrokeThickness = m_PointsStrokeThickness;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the point symbol of the current SamplePoints, or for all visible SamplePoints, if in edit mode.
        /// </summary>
        internal void SetPointSymbol()
        {
            // Sets the point symbol for current sample points
            if (m_SamplePoints != null)
            {
                m_SamplePoints.SamplePointSymbolSize = m_PointSymbolSize;
                m_SamplePoints.SamplePointSymbol = m_PointSymbol;
                m_SamplePoints.RedrawSamplePoints();

                if (m_SamplePoints.m_ImageList.Count > 0)
                {
                    foreach (Image image in m_SamplePoints.m_ImageList)
                    {
                        image.MouseLeftButtonDown += sample_MouseLeftButtonDown;
                        image.MouseRightButtonDown += sample_MouseRightButtonDown;
                    }
                }
                else
                {
                    m_SamplePoints.m_Path.MouseLeftButtonDown += sample_MouseLeftButtonDown;
                    m_SamplePoints.m_Path.MouseRightButtonDown += sample_MouseRightButtonDown;
                    m_SamplePoints.m_Path.MouseEnter += sample_MouseEnter;
                    m_SamplePoints.m_Path.MouseLeave += sample_MouseLeave;
                }
            }
            // Set transparency for all visible shapes if in Edit mode
            if (m_MouseMode == MouseMode.EditSamples)
            {
                foreach (Sample sample in m_SampleList)
                {
                    if (sample.IsSampleVisible)
                    {
                        switch (sample.TypeOfSample)
                        {
                            case SampleType.POINT:
                            case SampleType.MULTIPOINT:
                                (sample as SamplePoints).SamplePointSymbolSize = m_PointSymbolSize;
                                (sample as SamplePoints).SamplePointSymbol = m_PointSymbol;
                                (sample as SamplePoints).RedrawSamplePoints();
                                if ((sample as SamplePoints).m_ImageList.Count > 0)
                                {
                                    foreach (Image image in (sample as SamplePoints).m_ImageList)
                                    {
                                        image.MouseLeftButtonDown += sample_MouseLeftButtonDown;
                                        image.MouseRightButtonDown += sample_MouseRightButtonDown;
                                    }
                                }
                                else
                                {
                                    (sample as SamplePoints).m_Path.MouseLeftButtonDown += sample_MouseLeftButtonDown;
                                    (sample as SamplePoints).m_Path.MouseRightButtonDown += sample_MouseRightButtonDown;
                                    (sample as SamplePoints).m_Path.MouseEnter += sample_MouseEnter;
                                    (sample as SamplePoints).m_Path.MouseLeave += sample_MouseLeave;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                UpdateToggleButtons();
            }
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the sliderZoom control.
        /// Resets zoom and position of the working area (ShiftCanvas mode) or current transparency value (other modes).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void sliderZoom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (m_MouseMode == MouseMode.ShiftCanvas)
            {
                // Reset Zoom and margin
                m_Canvas.Margin = m_CanvasThickness = m_CanvasThicknessZero;
                m_ZoomFactor = 1;
            }
            else
                m_Transparency = m_FillTransparency = 64;
            UpdateSlider();
        }

        #endregion // Size and Slider handlers

        #region Button handlers

        /// <summary>
        /// Handles the Click event of the buttonAddShape control.
        /// Adds the current shape or image to the sample list and creates a toggle button.
        /// Handles reference world coordinates in case of image or Webbrowser map.
        /// Adds all imported GeoObjects (from interface function) to the sample list, if any.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonAddShape_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // ===================== DEBUG =========================
                // string str = System.Windows.Forms.Clipboard.GetText(System.Windows.Forms.TextDataFormat.Text);
                // WLine(str);

                // At least 3 points to define a polygon sample!
                if (m_SamplePolygon != null && m_SamplePolygon.SamplePointCountAll >= SamplePolygon.MinPoints)
                {
                    try
                    {
                        // MessageBox.Show("Clockwise = " + m_SamplePolygon.PolygonIsClockwise().ToString());
                        m_SamplePolygon.AddSample();
                        m_SamplePolygon.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                        SaveSample(m_SamplePolygon);
                        m_SamplePolygon.m_Poly.DataContext = AddToggleButton(m_SamplePolygon);
                        m_SamplePolygon = null;
                    }
                    catch (Exception)
                    {
                        m_SamplePolygon = null;
                    }
                }
                // At least 2 points to define a line string sample!
                if (m_SampleLine != null && m_SampleLine.SamplePointCountAll >= SampleLines.MinPoints)
                {
                    m_SampleLine.AddSample();
                    m_SampleLine.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                    SaveSample(m_SampleLine);
                    AddToggleButton(m_SampleLine);
                    m_SampleLine = null;
                }
                // At least 1 point to define a points sample!
                if (m_SamplePoints != null && m_SamplePoints.SamplePointCountAll >= SamplePoints.MinPoints)
                {
                    m_SamplePoints.AddSample();
                    if (m_SamplePoints.DisplayText == string.Empty)
                        m_SamplePoints.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                    // ====== Show Points on Google Maps ======
                    // m_WebBrowserPoints = CanvasToWorld(m_SamplePoints.SamplePointCollection);
                    SaveSample(m_SamplePoints);
                    AddToggleButton(m_SamplePoints);
                    m_SamplePoints = null;
                }
                if (m_SampleImage != null)
                {
                    m_SampleImage.AddSample();
                    m_SampleImage.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                    // Save image to list
                    SaveSample(m_SampleImage);
                    // Preserve reference map when image with coords is added to sample list
                    if (m_SampleImage.Tag != null)
                        if (m_SampleImage.Tag.ToString() == tagRef)
                            if (m_RefMap == null)
                                m_RefMap = m_SampleImage;
                    // Add a Toggle Button to switch the shape on and off
                    AddToggleButton(m_SampleImage);
                    // Write image attributes (coordinates, ...) to xml file
                    // WriteImageAttributesToXmlFile(m_SampleImage.ImageFilePath);
                    m_SampleImage = null;

                    // Load imported GeoObjects (when called by interface function)
                    if (m_WpfGeoObjectList != null)
                    {
                        ProgressBarInit(m_WpfGeoObjectList.Count + " " + WpfSamplingPlotPage.Properties.Resources.StrLoadingShapesInstruction, m_WpfGeoObjectList.Count);
                        // Create shapes from GeoObject list
                        foreach (GeoObject geoObject in m_WpfGeoObjectList)
                            CreateShapeFromGeoObject(geoObject);
                        m_WpfGeoObjectList = null;
                    }
                    //
                }
                if (m_MouseMode == MouseMode.MapServer)
                {
                    //(buttonAdd.ToolTip as ToolTip).Visibility = System.Windows.Visibility.Collapsed;
                    // Capture Webserver map and add it to list
                    if (LoadSingleImage(CaptureMap(), 0, 0, 255))
                    {
                        //(buttonAdd.ToolTip as ToolTip).Visibility = System.Windows.Visibility.Visible;
                        radioButtonShift.IsChecked = true;
                    }
                    else
                    {
                        //(buttonAdd.ToolTip as ToolTip).Visibility = System.Windows.Visibility.Visible;
                        return;
                    }

                    m_SampleImage = null;
                }

                // Load imported GeoObjects (when called by interface function)
                if (m_WpfGeoObjectList != null)
                {
                    ProgressBarInit(m_WpfGeoObjectList.Count + " " + WpfSamplingPlotPage.Properties.Resources.StrLoadingShapesInstruction, m_WpfGeoObjectList.Count);
                    // Create shapes from GeoObject list
                    foreach (GeoObject geoObject in m_WpfGeoObjectList)
                        CreateShapeFromGeoObject(geoObject);
                    m_WpfGeoObjectList = null;
                }
                // Bring capture frame to top
                if (m_CaptureFrame != null)
                    m_CaptureFrame.BringToTop();
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
            finally
            {
                ProgressBarClose();
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonMultiShape control.
        /// Saves current polygon or line of a shape and creates a new one, if in DrawPolygon or DrawLine mode.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonMultiShape_Click(object sender, RoutedEventArgs e)
        {
            // Store current shape (if enough points have been set) and create new one to build up a multi shape sample
            if (m_SamplePolygon != null)
            {
                m_SamplePolygon.AddSample();
                m_SamplePolygon.NewSample();
            }
            if (m_SampleLine != null)
            {
                m_SampleLine.AddSample();
                m_SampleLine.NewSample();
                m_DistPoint = m_Zero;
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonSaveShape control.
        /// Adds the current shape or image to the sample list and creates a toggle button.
        /// Additionally opens a file dialog to save all visible samples to a data storage medium.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonSaveShape_Click(object sender, RoutedEventArgs e)
        {
            if (m_MouseMode == MouseMode.MapServer)
            {
                // Capture Webserver map and add it to list
                Image img = CaptureMap();
                if (LoadSingleImage(img, m_XPos, 0, 255))
                {
                    if (!m_WriteImageToKalFile)
                        radioButtonShift.IsChecked = true;
                    SaveSingleImage(img);
                }
                m_SampleImage = null;
            }
            else
            {
                buttonAddShape_Click(sender, e);
                // Open dialog to save shapes in files
                SaveSampleFiles();
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonLoad control.
        /// Opens a file dialog to load shapes or images from a data storage medium.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            // Open dialog to read images or shapes from files
            OpenSampleFiles();
        }

        /// <summary>
        /// Handles the Checked event of the toggleButton control.
        /// Sets opacity to 1.0 (completely visible).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void toggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            btn.Opacity = 1.0;
        }

        /// <summary>
        /// Handles the Unchecked event of the toggleButton control.
        /// Sets opacity to 0.3 (shining through).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void toggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            btn.Opacity = 0.3;
        }

        /// <summary>
        /// Handles the MouseRightButtonDown event of the toggleButton control.
        /// Checks or unchecks all toggle buttons except the one which has been right clicked, depending on its state.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void toggleButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool uncheckOtherButtons = false;
            ToggleButton clickedButton = sender as ToggleButton;
            if (clickedButton.IsChecked.Value)
            {
                uncheckOtherButtons = true;
            }

            int count = m_SampleListCanvas.Children.Count;
            ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressSelectingSamples, count);
            count = 0;

            foreach (UIElement element in m_SampleListCanvas.Children)
            {
                string str = element.ToString();
                if (str.Contains("System.Windows.Controls.Primitives.ToggleButton"))
                {
                    ToggleButton btn = element as ToggleButton;

                    UIElement tglSample = btn.DataContext as UIElement;
                    if (tglSample != m_RefMap && btn != clickedButton)
                        btn.IsChecked = !uncheckOtherButtons;
                }

                count++;
                ProgressBarUpdate(count);
            }

            ProgressBarUpdate(m_SampleListCanvas.Children.Count);
            ProgressBarClose();

            clickedButton.IsChecked = uncheckOtherButtons;
        }

        /// <summary>
        /// Handles the MouseEnter event of the toggleButton control.
        /// Animates the related UI element of the working area.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        void btn_MouseEnter(object sender, MouseEventArgs e)
        {
            ToggleButton clickedButton = sender as ToggleButton;
            Sample sample = clickedButton.DataContext as Sample;
            sample.Animate(true);
        }

        /// <summary>
        /// Handles the MouseLeave event of the toggleButton control.
        /// Switches off the animation of the related UI element of the working area.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        void btn_MouseLeave(object sender, MouseEventArgs e)
        {
            ToggleButton clickedButton = sender as ToggleButton;
            Sample sample = clickedButton.DataContext as Sample;
            sample.Animate(false);
        }

        void sample_MouseEnter(object sender, MouseEventArgs e)
        {
            /*
            ToggleButton clickedButton = null;

            if (m_MouseMode == MouseMode.ShiftCanvas)
            {
                switch (sender.ToString())
                {
                    case "System.Windows.Shapes.Polygon":
                        clickedButton = (sender as Polygon).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Shapes.Polyline":
                        clickedButton = (sender as Polyline).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Shapes.Path":
                        clickedButton = (sender as System.Windows.Shapes.Path).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Controls.Image":
                        clickedButton = (sender as System.Windows.Controls.Image).DataContext as ToggleButton;
                        break;
                }
            }
            textBoxIdentifier.Text = (clickedButton.DataContext as Sample).Identifier; 
            textBoxSampleText.Text = (clickedButton.DataContext as Sample).DisplayText;
            // sample.Animate(true);
            */
        }

        void sample_MouseLeave(object sender, MouseEventArgs e)
        {
            /*
            textBoxIdentifier.Text = string.Empty;
            textBoxSampleText.Text = string.Empty;
            // sample.Animate(false);
            */
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the sample's UI element.
        /// Checks or unchecks the toggle button regarding to the sample which has been left clicked, switching the UI element on or off.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        internal void sample_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (m_Testcase)
                return;

            // Disable in GIS View Mode
            if (m_ViewerMode)
                return;

            ToggleButton clickedButton = null;

            if (m_MouseMode == MouseMode.ShiftCanvas)
            {
                switch (sender.ToString())
                {
                    case "System.Windows.Shapes.Polygon":
                        clickedButton = (sender as Polygon).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Shapes.Polyline":
                        clickedButton = (sender as Polyline).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Shapes.Path":
                        clickedButton = (sender as System.Windows.Shapes.Path).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Controls.Image":
                        clickedButton = (sender as System.Windows.Controls.Image).DataContext as ToggleButton;
                        break;
                }
                // Sample sample = clickedButton.DataContext as Sample;
                if (clickedButton != null)
                {
                    clickedButton.IsChecked = !clickedButton.IsChecked;
                }
            }
        }


        /// <summary>
        /// Handles the MouseRightButtonDown event of the sample's UI element.
        /// Unchecks all toggle buttons except the one regarding to the sample which has been right clicked, depending on its state.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        internal void sample_MouseRightButtonDown(object sender, MouseEventArgs e)
        {
            if (m_Testcase)
                return;

            // Disable in GIS View Mode
            if (m_ViewerMode)
                return;

            ToggleButton clickedButton = null;

            if (m_MouseMode == MouseMode.ShiftCanvas)
            {
                switch (sender.ToString())
                {
                    case "System.Windows.Shapes.Polygon":
                        clickedButton = (sender as Polygon).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Shapes.Polyline":
                        clickedButton = (sender as Polyline).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Shapes.Path":
                        clickedButton = (sender as System.Windows.Shapes.Path).DataContext as ToggleButton;
                        break;
                    case "System.Windows.Controls.Image":
                        clickedButton = (sender as System.Windows.Controls.Image).DataContext as ToggleButton;
                        break;
                }
                // Sample sample = clickedButton.DataContext as Sample;
                if (clickedButton != null)
                {
                    // clickedButton.SetBinding(ToggleButton.IsCheckedProperty, "SampleVisibility");
                    toggleButton_MouseRightButtonDown(clickedButton, null);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the deleteButton control.
        /// Clears appropriate sample and removes it from the sample list.
        /// Removes sample controls (toggle button, delete button and ID) and reorders other sample buttons.
        /// Handles world coordinates in case of an image, if it was the reference map.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            string labelInstructionText = labelInstruction.Content.ToString();
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrDeletingShapesInstruction, true);
            DoEvents();
            try
            {
                Button btn = sender as Button;

                // Remove sample from list
                Sample sample = btn.DataContext as Sample;
                switch (sample.TypeOfSample)
                {
                    case SampleType.POLYGON:
                    case SampleType.MULTIPOLYGON:
                        (sample as SamplePolygon).ClearSample();
                        break;
                    case SampleType.LINESTRING:
                    case SampleType.MULTILINESTRING:
                        (sample as SampleLines).ClearSample();
                        break;
                    case SampleType.POINT:
                    case SampleType.MULTIPOINT:
                        (sample as SamplePoints).ClearSample();
                        break;
                    case SampleType.IMAGE:
                        if ((sample as SampleImage) == m_RefMap)
                            AssignRefMapToNextImage(sample);
                        (sample as SampleImage).ClearSample();
                        break;
                }
                m_SampleList.Remove(sample);
                // Reset world coordinates if no more reference map
                if (WorldHasDisappeared())
                    ClearCoordinates();
                RemoveSampleControls(sample);
                ReadjustToggleButtons();
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
            SetInstructionLabel(labelInstructionText, false);
        }

        /// <summary>
        /// Assigns the ref map to the next image of the list.
        /// </summary>
        /// <param name="refMapImage">The current ref map image.</param>
        private void AssignRefMapToNextImage(Sample refMapImage)
        {
            m_RefMap = null;

            // Find next sample image
            foreach (Sample sample in m_SampleList)
            {
                if (sample.TypeOfSample == SampleType.IMAGE)
                {
                    if (sample != refMapImage)
                    {
                        // Keep reference Map
                        m_RefMap = sample as SampleImage;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if there are still images with world coordinates in list.
        /// </summary>
        /// <returns><c>true</c> if no more world coordinates are available, otherwise <c>false</c>.</returns>
        private bool WorldHasDisappeared()
        {
            // Check samples in sample list
            foreach (Sample sample in m_SampleList)
            {
                if (sample != null && sample.TypeOfSample == SampleType.IMAGE && (sample as SampleImage).Tag != null)
                    if ((sample as SampleImage).Tag.ToString() == tagRef)
                        return false;
            }
            StatusPos(m_Zero);
            m_RefMap = null;
            return true;
        }

        /// <summary>
        /// Determines the height and width of the Webbrowser map to be captured.
        /// </summary>
        /// <param name="height">Output value of the height to capture.</param>
        /// <param name="width">Output value of the width to capture.</param>
        private void SetCaptureHeight(out Int32 height, out Int32 width)
        {
            // Get size of capture area
            if (m_Canvas.Parent == m_CanvasClip)
            {
                // int diff = (m_MapModeViewer == 0 ? 2 : 0);  // Do we need this offset for Google ???
                height = (Int32)m_UserControl.ActualHeight - m_WebBrowserAdaptHeight; // - diff; 
                width = (Int32)m_UserControl.ActualWidth - WebBrowserCanvasLeft;
            }
            else
            {
                if (m_CanvasParent != null)
                {
                    height = m_CanvasParent.Height - m_CanvasParent.Margin.Top - m_CanvasParent.Margin.Bottom - 4;
                    width = m_CanvasParent.Width - m_CanvasParent.Margin.Left - m_CanvasParent.Margin.Right - 4;
                }
                else
                {
                    height = (Int32)m_UserControl.ActualHeight + WebBrowserBorder;
                    width = (Int32)m_UserControl.ActualWidth + m_WebBrowserWidthDiff - WebBrowserBorder;
                }
            }
        }

        /// <summary>
        /// Captures a Google Map from the WebBrowser to be stored as sample (image).
        /// </summary>
        /// <returns>Captured image.</returns>
        private Image CaptureMap()
        {
            Image capturedImage = new Image();
            try
            {
                // Set Cursor to wait
                this.Cursor = m_Canvas.Cursor = Cursors.Wait;
                // Use progress bar to allow tool tips to fade out under Windows 7

                Int32 height = 0;
                Int32 width = 0;
                SetCaptureHeight(out height, out width);

                // Create Bitmap for capture
                System.Drawing.Bitmap captureBitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                // Create graphics to host capture bitmap
                System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(captureBitmap);

                double dpiFactorX = graphics.DpiX / 96.0;
                double dpiFactorY = graphics.DpiY / 96.0;

                // Check device independent units versa pixels

                if (graphics.DpiX != 96 || graphics.DpiY != 96)
                {
                    // Adapt capture area to non standard dpi settings
                    // Create Bitmap for capture
                    captureBitmap = new System.Drawing.Bitmap((int)(width * dpiFactorX), (int)(height * dpiFactorY), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    // Create graphics to host capture bitmap
                    graphics = System.Drawing.Graphics.FromImage(captureBitmap);
                }


                // Calculate current Webbrowser window position on screen
                Point pnt = m_Canvas.PointToScreen(new Point(0, 0));

                // Copy Webbrowser bitmap from screen
                graphics.CopyFromScreen((int)pnt.X, (int)pnt.Y, 0, 0, captureBitmap.Size);

                // Save to harddisk
                // captureBitmap.Save("temp.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

                // Save to memory steam
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                captureBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                // Create image source and bitmap image
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                // Create Image element
                // Image capturedImage = new Image();
                // Set attributes 
                capturedImage.Source = bi;
                capturedImage.Stretch = Stretch.None;

                graphics.Dispose();
                // Cannot close memory stream, connected to capturedImage!
                // ms.Close();

                // Restore Cursor
                this.Cursor = m_Canvas.Cursor = m_CursorOrg;
            }
            catch (Exception ex)
            {
                SetError(ex.Message);
            }

            buttonAdd.ToolTip = SetToolTip(WpfSamplingPlotPage.Properties.Resources.ttButtonAdd);
            // Avoid capture of the Add button tooltip when freezing the map
            (buttonAdd.ToolTip as ToolTip).VerticalOffset = -30;

            return capturedImage;

            /*
            // Set pixels of raw image according bitmap
            int k = 0;

            int mod = (height / 25) + 1;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    rawImage[k++] = captureBitmap.GetPixel(j, i).B;
                    rawImage[k++] = captureBitmap.GetPixel(j, i).G;
                    rawImage[k++] = captureBitmap.GetPixel(j, i).R;
                    rawImage[k++] = captureBitmap.GetPixel(j, i).A;
                }

                if (i%mod == 0)
                    ProgressBarUpdate((100 * i )/ height);
            }
            */
        }


        /// <summary>
        /// Creates a blank map to be stored as sample (image).
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>Captured image.</returns>
        private Image BlankMap(int width, int height)
        {
            // Set Cursor to wait
            this.Cursor = m_Canvas.Cursor = Cursors.Wait;

            // Create raw image (byte array) from captured bitmap
            int rawStride = (width * PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * height];

            // Set white pixels of raw image 
            for (int i = 0; i < height * width * 4; i++)
                rawImage[i] = 255;

            // Create Bitmap source from raw image
            BitmapSource src = BitmapSource.Create((int)width, (int)height, 96.0, 96.0, PixelFormats.Bgr32, null, rawImage, rawStride);
            // Create Controls Image and assign source
            Image capturedImage = new Image();
            capturedImage.Source = src;
            capturedImage.Opacity = 0.1;

            // Restore Cursor
            this.Cursor = m_Canvas.Cursor = m_CursorOrg;

            return capturedImage;
        }

        /// <summary>
        /// Handles the Click event of the buttonPrint control.
        /// Print the complete working area including all visible samples.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintCanvas();
        }

        /// <summary>
        /// Opens a print dialog and prints the working area.
        /// </summary>
        private void PrintCanvas()
        {
            PrintDialog dlg = new PrintDialog();
            if ((bool)dlg.ShowDialog().GetValueOrDefault())
            {
                // Markus 20.12.2023 - ermöglicht Druck von gesamter Karte - wieder ausgebaut weil unnoetig
                //if(this.m_AreaFrameHeight > m_Canvas.ActualHeight || this.m_AreaFrameWidth > m_Canvas.ActualWidth)
                //{
                //    string Message = "The current printed area is \r\n" + m_Canvas.ActualHeight.ToString() + " x " + m_Canvas.ActualWidth.ToString() + "\r\n\r\n"
                //        + "Do you want to expand it to the area defined in the settings:\r\n" 
                //        + m_AreaFrameHeight.ToString() + " x " + m_AreaFrameWidth.ToString();
                //    if (MessageBox.Show(Message, "Change area", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                //    {
                //        m_Canvas.Width = m_AreaFrameWidth;
                //        m_Canvas.Height = m_AreaFrameHeight;
                //    }
                //}
                dlg.PrintVisual(m_Canvas, "Sampling Plots");
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonDeleteAll control.
        /// Deletes all samples in the list except the reference map, if any.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            if (m_SampleList.Count > 0 && SetRequest(WpfSamplingPlotPage.Properties.Resources.ErrDeleteAllSamples))
                DeleteAllSamples(false);
        }

        /// <summary>
        /// Handles the Click event of the buttonSettings control.
        /// Opens a Settings dialog window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            // Dialog window to adjust the settings:
            // - Save file as text-shape, Arcview shape, Kal file
            // - Calculate Altitude for coordinates yes, no
            // - more...

            int mapModeViewer = m_MapModeViewer;

            // Create Settings window
            Settings m_Settings = new Settings(this);
            // Show Dialog
            m_Settings.ShowDialog();

            // Switch MapModeViewer if necessary
            if (mapModeViewer != m_MapModeViewer)
                SetMapModeViewer(m_MapModeViewer);
        }


        /// <summary>
        /// Handles the Click event of the buttonDetection control.
        /// Opens a Detection paramneters dialog window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonDetection_Click(object sender, RoutedEventArgs e)
        {
            CheckImage();
        }


        /// <summary>
        /// Sets the map mode viewer webbrowser path and settings.
        /// </summary>
        /// <param name="viewer">The viewer to set.</param>
        private void SetMapModeViewer(int viewer)
        {
            switch (viewer)
            {
                // Google maps (default)
                default:
                case 0:
                    m_MapModeViewerPath = MapModeViewerGoogle;
                    m_MapModeViewerPath1 = MapModeViewer1Google;
                    m_WebBrowserAdaptHeight = WebBrowserAdaptHeightGoogle;
                    m_WebBrowserAdaptWidth = WebBrowserAdaptWidthGoogle;
                    m_WebBrowserWidthDiff = WebBrowserWidthDiffGoogle;
                    labelWorld.Content = GetImageFromFile("Google.bmp");
                    break;
                // Open Street Maps
                case 1:
                    m_MapModeViewerPath = MapModeViewerOSM;
                    m_MapModeViewerPath1 = MapModeViewer1OSM;
                    m_WebBrowserAdaptHeight = WebBrowserAdaptHeightOSM;
                    m_WebBrowserAdaptWidth = WebBrowserAdaptWidthOSM;
                    m_WebBrowserWidthDiff = WebBrowserWidthDiffOSM;
                    labelWorld.Content = GetImageFromFile("OSM.bmp");
                    break;
            }
            UpdateWebBrowserUriByPos();
            WebBrowserAdapt();
        }


        // GPS Dispatcher timer
        private DispatcherTimer m_TimerGps = new DispatcherTimer();

        /// <summary>
        /// Handles the Click event of the buttonGps control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonGps_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_Gps == null)
                {
                    if (m_GpsBaudrate == "Demo mode")
                        m_Gps = new Gps(m_WorldTop, m_WorldBottom, m_WorldLeft, m_WorldRight);
                    else
                        m_Gps = new Gps(Convert.ToInt32(m_GpsBaudrate));

                    if (!m_Gps.FindGpsSerialPort())
                    {
                        m_Gps.CloseGpsSerialPort();
                        m_Gps = null;
                        return;
                    }

                    buttonGps.Content = GetImageFromFile("GpsRed.ico");

                    m_GpsPosition = new SamplePoints(m_Canvas);
                    m_GpsPosition.SamplePointSymbol = PointSymbol.Circle;
                    m_GpsPosition.SamplePointSymbolSize = 1.5;
                    m_GpsPosition.FillBrush = Brushes.Yellow;
                    m_GpsPosition.StrokeThickness = 2.5;
                    m_GpsPosition.FillTransparency = 127;

                    if (m_TrackGpsData)
                    {
                        m_GpsTrack = new SampleLines(m_Canvas);
                        m_GpsTrack.StrokeThickness = m_LineStrokeThickness;
                        m_GpsTrack.StrokeBrush = m_LastStrokeBrush;
                    }

                    // Setup timer
                    m_TimerGps.Interval = TimeSpan.FromMilliseconds(1000);
                    m_TimerGps.Tick += TimerGps_Tick;
                    m_TimerGps.Start();
                }
                else
                {
                    m_TimerGps.Stop();
                    m_TimerGps.Tick -= TimerGps_Tick;
                    m_Gps.CloseGpsSerialPort();
                    m_Gps = null;

                    // Add Track to sample list
                    // At least 2 points to define a line string sample!
                    if (m_GpsTrack != null && m_GpsTrack.SamplePointCountAll >= SampleLines.MinPoints)
                    {
                        m_GpsTrack.AddSample();
                        m_GpsTrack.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                        SaveSample(m_GpsTrack);
                        AddToggleButton(m_GpsTrack);
                        m_GpsTrack = null;
                    }
                    m_GpsPosition.ClearSample();
                    m_GpsPosition = null;

                    buttonGps.Content = GetImageFromFile("Gps.ico");
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// Handles the Tick event of the TimerGps control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TimerGps_Tick(object sender, EventArgs e)
        {
            // Console.Beep(800, 50);
            // m_Gps.GetStatus;
            try
            {
                if (m_Gps.ReadGpsCoordinates())
                {
                    int sats = m_Gps.GetSatellites;
                    if (sats < 4)
                        buttonGps.Content = GetImageFromFile("GpsRed.ico");
                    else if (sats < 6)
                        buttonGps.Content = GetImageFromFile("GpsYellow.ico");
                    else
                        buttonGps.Content = GetImageFromFile("GpsGreen.ico");

                    if (m_WorldTop != 0 && sats >= 4)
                    {
                        Point curWorldPos = new Point(m_Gps.GetLongitude, m_Gps.GetLatitude);
                        Point curScreenPos = WorldToCanvas(curWorldPos);

                        if (m_GpsPosition.SamplePointCountCurrent == 0)
                        {
                            m_GpsPosition.SetSamplePoint(curScreenPos);
                            if (m_TrackGpsData)
                                m_GpsTrack.SetSamplePoint(curScreenPos);
                            m_LastGpsPos = curScreenPos;
                        }
                        else if (Convert.ToInt32(curScreenPos.X) != Convert.ToInt32(m_LastGpsPos.X) || Convert.ToInt32(curScreenPos.Y) != Convert.ToInt32(m_LastGpsPos.Y))
                        {
                            m_GpsPosition.ClearSamplePoint();
                            m_GpsPosition.SetSamplePoint(curScreenPos);
                            if (m_TrackGpsData)
                                m_GpsTrack.SetSamplePoint(curScreenPos);
                            m_LastGpsPos = curScreenPos;
                        }
                    }
                    labelInstruction.Content = string.Format(WpfSamplingPlotPage.Properties.Resources.StrGpsInfo, m_Gps.GetSatellites.ToString(), m_Gps.GetLongitude.ToString("F6", CultureInfo.InvariantCulture), m_Gps.GetLatitude.ToString("F6", CultureInfo.InvariantCulture), m_Gps.GetAltitude.ToString("F1", CultureInfo.InvariantCulture));
                    // labelInstruction.Content = "GPS: Sats: " + m_Gps.GetSatellites.ToString() + "  Lon = " + m_Gps.GetLongitude.ToString("F6", CultureInfo.InvariantCulture) + "  Lat = " + m_Gps.GetLatitude.ToString("F6", CultureInfo.InvariantCulture) + "  Alt = " + m_Gps.GetAltitude.ToString("F1", CultureInfo.InvariantCulture);
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// Handles the Click event of the buttonProgressCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonProgressCancel_Click(object sender, RoutedEventArgs e)
        {
            m_ProgressCancel = true;
        }

        #endregion // Button handlers

        #region Combo and Text box handlers

        /// <summary>
        /// Handles the SelectionChanged event of the comboBoxBrushes control.
        /// Sets the color of a shape accordingly (preset before dropdown action).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void comboBoxBrushes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetComboboxBrush();
        }

        // comboBoxBrushes_DropDownClosed - set color of shape accordingly (no matter if changed or not!)
        /// <summary>
        /// Handles the DropDownClosed event of the comboBoxBrushes control.
        /// Sets the color of a shape accordingly (no matter if changed or not!).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void comboBoxBrushes_DropDownClosed(object sender, EventArgs e)
        {
            SetComboboxBrush();
        }

        /// <summary>
        /// Assigns the selected color of the combo box to the current sample, or to all visible samples, if in EditSamples mode.
        /// </summary>
        private void SetComboboxBrush()
        {
            ComboBoxItem item = comboBoxBrushes.SelectedItem as ComboBoxItem;
            if (item != null)
            {
                // Select Text color according to background brightness
                m_Brush = item.Background as SolidColorBrush;
                // comboBoxBrushes.Background = item.Background;
                bool isBlack = 0.222 * m_Brush.Color.R + 0.707 * m_Brush.Color.G + 0.071 * m_Brush.Color.G > 128;
                comboBoxBrushes.Foreground = Brushes.Black; // isBlack ? Brushes.Black : Brushes.White;
                labelColor.Background = item.Background;
                labelColor.Foreground = isBlack ? Brushes.Black : Brushes.White;

                // Set current brush for stroke and fill
                if (m_SetStroke)
                    m_LastStrokeBrush = m_Brush;
                if (m_SetFill)
                    m_LastFillBrush = m_Brush;

                // Assign selected color to sample
                if (m_SamplePolygon != null)
                {
                    if (m_SetStroke)
                        m_SamplePolygon.StrokeBrush = m_Brush;
                    if (m_SetFill)
                        m_SamplePolygon.FillBrush = m_Brush;
                }
                else if (m_SampleLine != null)
                {
                    if (m_SetStroke)
                        m_SampleLine.StrokeBrush = m_Brush;
                }
                else if (m_SamplePoints != null)
                {
                    if (m_SetStroke)
                        m_SamplePoints.StrokeBrush = m_Brush;
                    if (m_SetFill)
                        m_SamplePoints.FillBrush = m_Brush;
                }
                // Assign to all visible samples in Edit mode
                if (m_MouseMode == MouseMode.EditSamples)
                {
                    foreach (Sample sample in m_SampleList)
                        if (sample.IsSampleVisible)
                            switch (sample.TypeOfSample)
                            {
                                case SampleType.POLYGON:
                                case SampleType.MULTIPOLYGON:
                                    if (m_SetStroke)
                                        (sample as SamplePolygon).StrokeBrush = m_Brush;
                                    if (m_SetFill)
                                        (sample as SamplePolygon).FillBrush = m_Brush;
                                    break;
                                case SampleType.LINESTRING:
                                case SampleType.MULTILINESTRING:
                                    if (m_SetStroke)
                                        (sample as SampleLines).StrokeBrush = m_Brush;
                                    break;
                                case SampleType.POINT:
                                case SampleType.MULTIPOINT:
                                    if (m_SetStroke)
                                        (sample as SamplePoints).StrokeBrush = m_Brush;
                                    if (m_SetFill)
                                        (sample as SamplePoints).FillBrush = m_Brush;
                                    break;
                                default:
                                    break;
                            }
                    UpdateToggleButtons();
                }
            }
            // Remove Focus from comboBox to display selected item in the correct color
            textBoxIdentifier.Focus();
        }

        /// <summary>
        /// Resets the check boxes for Stroke and Fill color to <c>checked</c>.
        /// </summary>
        private void ResetCheckBoxes()
        {
            checkBoxStroke.IsChecked = false;
            checkBoxFill.IsChecked = false;
        }

        // Reset background after input
        /// <summary>
        /// Handles the TextChanged event of the textBoxIdentifier control.
        /// Resets the background of the ID text box to white after input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void textBoxIdentifier_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxIdentifier.Text == string.Empty)
                textBoxIdentifier.Background = m_BrushHighlight;
            else
                textBoxIdentifier.Background = m_BrushClear;
        }

        /// <summary>
        /// Handles the TextChanged event of the textBoxSampleText control.
        /// Resets the background of the Description text box to white after input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> instance containing the event data.</param>
        private void textBoxSampleText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxSampleText.Text == string.Empty)
                textBoxSampleText.Background = m_BrushHighlight;
            else
                textBoxSampleText.Background = m_BrushClear;
        }

        #endregion //  Combo and Text box handlers

        #region Help handler

        void HelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (m_HelpProcess == null || m_HelpProcess.HasExited)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // System.Windows.Forms.Help.ShowHelp(null, "DiversityGazetteers.chm", System.Windows.Forms.HelpNavigator.KeywordIndex, "License");
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(m_HelpFile);
                info.UseShellExecute = true;
                m_HelpProcess = System.Diagnostics.Process.Start(info);
            }
            catch // (Exception ex)
            {
                // SetError(WpfSamplingPlotPage.Properties.Resources.ErrNoHelpFile + ex.Message);
                // SetError("Fehler beim Öffnen der Hilfe-Datei DiversityGisEditor.chm:\r\n" + ex.Message);
            }
        }

        #endregion // Help handler

        #endregion // Event handlers

        #region Add and remove shapes (toggle buttons)

        /// <summary>
        /// Add a sample to the sample list.
        /// </summary>
        /// <param name="sample">The sample.</param>
        private void SaveSample(Sample sample)
        {
            // Add sample to sample list (base class)
            m_SampleList.Add(sample);
        }

        /// <summary>
        /// Create a toggle button for a sample, fill it with a copy of the shape 
        /// and add it to the canvas (including delete button and ID label).
        /// </summary>
        /// <param name="element">The element.</param>
        private ToggleButton AddToggleButton(FrameworkElement element)
        {
            // Create new ToggleButton instance
            ToggleButton btn = new ToggleButton();
            btn.Checked += new RoutedEventHandler(toggleButton_Checked);
            btn.Unchecked += new RoutedEventHandler(toggleButton_Unchecked);
            btn.MouseRightButtonDown += new MouseButtonEventHandler(toggleButton_MouseRightButtonDown);
            btn.MouseEnter += btn_MouseEnter;
            btn.MouseLeave += btn_MouseLeave;
            // Increment button counter
            m_ImageCount++;

            SampleType sampleType = (element as Sample).TypeOfSample;
            // Picture buttons
            switch (sampleType)
            {
                case SampleType.IMAGE:
                    // Create copy of image
                    Image img = (element as SampleImage).m_Image;
                    Image img1 = new Image();
                    img1.Source = img.Source;
                    img1.Stretch = Stretch.Uniform;
                    // Assign it to button
                    btn.Content = img1;
                    break;

                case SampleType.POLYGON:
                case SampleType.MULTIPOLYGON:
                    // Create copy of polygon
                    Polygon poly = (element as SamplePolygon).m_PolyList[0];
                    Polygon poly1 = new Polygon();
                    poly1.Points = poly.Points;
                    poly1.Stroke = poly.Stroke;
                    poly1.Fill = poly.Fill;
                    // poly1.StrokeThickness = poly.StrokeThickness;
                    poly1.Stretch = Stretch.Uniform;
                    // Assign it to button
                    btn.Content = poly1;
                    // Assign Event handlers to polygons
                    foreach (Polygon poly2 in (element as SamplePolygon).m_PolyList)
                    {
                        poly2.DataContext = btn;
                        poly2.MouseLeftButtonDown += sample_MouseLeftButtonDown;
                        poly2.MouseRightButtonDown += sample_MouseRightButtonDown;
                        poly2.MouseEnter += sample_MouseEnter;
                        poly2.MouseLeave += sample_MouseLeave;
                    }
                    break;

                case SampleType.LINESTRING:
                case SampleType.MULTILINESTRING:
                    // Create copy of polyline
                    Polyline line = (element as SampleLines).m_PolyList[0];
                    Polyline line1 = new Polyline();
                    line1.Points = line.Points;
                    line1.Stroke = line.Stroke;
                    line1.Fill = line.Fill;
                    // line1.StrokeThickness = line.StrokeThickness;
                    line1.Stretch = Stretch.Uniform;
                    // Assign it to button
                    btn.Content = line1;
                    // Assign Event handlers to linestrings
                    foreach (Polyline line2 in (element as SampleLines).m_PolyList)
                    {
                        line2.DataContext = btn;
                        line2.MouseLeftButtonDown += sample_MouseLeftButtonDown;
                        line2.MouseRightButtonDown += sample_MouseRightButtonDown;
                        line2.MouseEnter += sample_MouseEnter;
                        line2.MouseLeave += sample_MouseLeave;
                    }
                    break;

                case SampleType.POINT:
                case SampleType.MULTIPOINT:
                    // Do we have icons?
                    if ((element as SamplePoints).m_ImageList.Count > 0)
                    {
                        Image image = new Image();
                        image.Source = (element as SamplePoints).m_ImageList[0].Source;
                        image.Stretch = Stretch.Uniform;
                        // Assign it to button
                        btn.Content = image;
                        // Assign Event handlers to linestrings
                        foreach (Image image2 in (element as SamplePoints).m_ImageList)
                        {
                            image2.DataContext = btn;
                            image2.MouseLeftButtonDown += sample_MouseLeftButtonDown;
                            image2.MouseRightButtonDown += sample_MouseRightButtonDown;
                        }
                    }
                    // No, we have shapes!
                    else
                    {
                        // Create copy of objects path
                        System.Windows.Shapes.Path path = (element as SamplePoints).m_Path;
                        System.Windows.Shapes.Path path1 = new System.Windows.Shapes.Path();
                        path1.Data = path.Data;
                        path1.Stroke = path.Stroke;
                        path1.Fill = path.Fill;
                        // path1.StrokeThickness = path.StrokeThickness;
                        path1.Stretch = Stretch.Uniform;
                        // Assign it to button
                        btn.Content = path1;
                        path.DataContext = btn;
                        path.MouseLeftButtonDown += sample_MouseLeftButtonDown;
                        path.MouseRightButtonDown += sample_MouseRightButtonDown;
                        path.MouseEnter += sample_MouseEnter;
                        path.MouseLeave += sample_MouseLeave;
                    }
                    break;

                default:
                    // Just assign the sample count (not applicable)
                    btn.Content = m_ImageCount.ToString();
                    break;
            }

            // Set position, witdth, height, content to index 
            btn.Width = ToggleButtonWidth;
            btn.Height = ToggleButtonHeight;
            Canvas.SetLeft(btn, ToggleButtonLeft);
            Canvas.SetTop(btn, (ToggleButtonDistance * ToggleButtonHeight * (m_ImageCount - 1)) + ToggleButtonHeight);
            AdjustSampleListCanvasHeight();
            // Set databinding of Button to Opacity
            btn.SetBinding(ToggleButton.IsCheckedProperty, "SampleVisibility");
            btn.DataContext = element;
            btn.ToolTip = SetToolTip((element as Sample).Identifier + StrCR + (element as Sample).DisplayText);
            (btn.ToolTip as ToolTip).FontFamily = new FontFamily("Consolas");
            ToolTipService.SetShowDuration(btn, 300000);

            // Add to CanvasPanel
            m_SampleListCanvas.Children.Add(btn);
            AddDeleteButton(element, sampleType);
            AddSampleIdentifier(element, sampleType);

            return btn;
        }


        /// <summary>
        /// Adjust the height of the canvas holding the Sample list buttons according to the user control height. 
        /// This is necessary to ensure that the scroll bar of the parent control will only appear, if the 
        /// buttons do not fit into the visible working area.
        /// </summary>
        private void AdjustSampleListCanvasHeight()
        {
            m_SampleListCanvas.Height = ToggleButtonDistance * ToggleButtonHeight * m_ImageCount;
        }

        /// <summary>
        /// Create a delete button for a sample and add it to the canvas.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="sampleType">Type of the sample.</param>
        private void AddDeleteButton(FrameworkElement element, SampleType sampleType)
        {
            // Create new ToggleButton instance
            Button btn = new Button();
            btn.Click += new RoutedEventHandler(deleteButton_Click);
            btn.Content = GetImageFromFile("Delete.ico");
            btn.Tag = tagDel;

            // Set position, witdth, height, content to index 
            btn.Width = DeleteButtonWidth;
            btn.Height = DeleteButtonHeight;
            Canvas.SetLeft(btn, DeleteButtonLeft);
            int dist = (ToggleButtonDistance * ToggleButtonHeight * (m_ImageCount - 1)) + 1 + ToggleButtonHeight;
            Canvas.SetTop(btn, dist);
            // Set data context to sample
            btn.DataContext = element;

            // Check if delete button should not be provided
            if ((element as SampleImage) != null)
                if ((element as SampleImage) == m_RefMap && !m_WpfShowRefMapDelButton)
                {
                    btn.IsEnabled = false;
                    btn.Opacity = 0.3;
                }
            // Add to CanvasPanel
            m_SampleListCanvas.Children.Add(btn);
        }

        /// <summary>
        /// Create a sample identifier label and add it to the canvas.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="sampleType">Type of the sample.</param>
        private void AddSampleIdentifier(FrameworkElement element, SampleType sampleType)
        {
            // Create new ToggleButton instance
            Label idLabel = new Label();
            idLabel.Background = Brushes.LemonChiffon;

            idLabel.Content = (element as Sample).Identifier;
            // ------------- Andreas Special: Show DisplayText instead of Idetifier -----------------
            // ------------- idLabel.Content = (element as Sample).DisplayText;

            idLabel.Padding = m_LabelPadding;
            idLabel.FontSize = 11;
            idLabel.Tag = tagDel;
            // label.ToolTip = SetToolTip((element as Sample).Identifier + StrCR + (element as Sample).DisplayText);

            // Set position, witdth, height
            idLabel.Width = DeleteButtonWidth + ToggleButtonWidth;
            idLabel.Height = IdLabelHeight;
            Canvas.SetLeft(idLabel, DeleteButtonLeft);
            Canvas.SetTop(idLabel, (ToggleButtonDistance * ToggleButtonHeight * (m_ImageCount - 1)) + IdLabelDistance);
            // Set data context to sample
            idLabel.DataContext = element;

            // Add to CanvasPanel
            m_SampleListCanvas.Children.Add(idLabel);
        }

        /// <summary>
        /// Readjusts the other toggle buttons, after one has been deleted.
        /// </summary>
        private void ReadjustToggleButtons()
        {
            try
            {
                // Readjust buttons
                // double top = Canvas.GetTop(m_SampleListCanvas);
                int toggleButtonCount = 0;
                int deleteButtonCount = 0;
                int idLabelCount = 0;
                foreach (UIElement element in m_SampleListCanvas.Children)
                {
                    string str = element.ToString();
                    if (str.Contains("System.Windows.Controls.Primitives.ToggleButton"))
                    {
                        Canvas.SetTop(element, (ToggleButtonDistance * ToggleButtonHeight * toggleButtonCount + ToggleButtonHeight));
                        ToggleButton btn = element as ToggleButton;
                        toggleButtonCount++;
                    }
                    // Identify the delete buttons by tag "delete" (tagDel)
                    if (str == "System.Windows.Controls.Button")
                    {
                        Button btn = element as Button;
                        if (btn.Tag == null)
                            continue;
                        if (btn.Tag.ToString() != tagDel)
                            continue;
                        Canvas.SetTop(element, (ToggleButtonDistance * ToggleButtonHeight * deleteButtonCount) + 1 + ToggleButtonHeight);
                        deleteButtonCount++;
                    }
                    if (str.Contains("System.Windows.Controls.Label"))
                    {
                        Label idLabel = element as Label;
                        if (idLabel.Tag == null)
                            continue;
                        if (idLabel.Tag.ToString() != tagDel)
                            continue;
                        Canvas.SetTop(element, (ToggleButtonDistance * ToggleButtonHeight * idLabelCount) + IdLabelDistance);
                        idLabelCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Remove related UI elements from Canvas, if a sample has been deleted.
        /// </summary>
        /// <param name="sample">The sample.</param>
        private void RemoveSampleControls(Sample sample)
        {
            // Check all elements
            UIElement removeToggleButton = null;
            UIElement removeLabel = null;
            UIElement removeDeleteButton = null;
            foreach (UIElement element in m_SampleListCanvas.Children)
            {
                string str = element.ToString();
                // Identify ToggleButton
                if (str.Contains("System.Windows.Controls.Primitives.ToggleButton"))
                {
                    ToggleButton tglbtn = element as ToggleButton;
                    UIElement tglSample = tglbtn.DataContext as UIElement;
                    if (tglSample == sample)
                        removeToggleButton = element;
                }
                // Identify Label
                if (str.Contains("System.Windows.Controls.Label"))
                {
                    Label label = element as Label;
                    UIElement tglSample = label.DataContext as UIElement;
                    if (tglSample == sample)
                        removeLabel = element;
                }
                // Identify DeleteButton
                if (str == "System.Windows.Controls.Button")
                {
                    Button btn = element as Button;
                    if (btn.Tag != null && btn.Tag.ToString() == tagDel && btn.DataContext == sample)
                        removeDeleteButton = element;
                }

                if (removeToggleButton != null && removeLabel != null && removeDeleteButton != null)
                    break;
            }
            // remove ToggleButton, Delete button and Label from canvas panel
            if (removeToggleButton != null)
            {
                m_SampleListCanvas.Children.Remove(removeToggleButton);
                m_ImageCount--;
                if (removeLabel != null)
                    m_SampleListCanvas.Children.Remove(removeLabel);
                if (removeDeleteButton != null)
                    m_SampleListCanvas.Children.Remove(removeDeleteButton);
            }
            AdjustSampleListCanvasHeight();
        }

        /// <summary>
        /// Updates the content of the toggle buttons (polygon, line...) when the original sample has been changed after edit (new color...).
        /// </summary>
        private void UpdateToggleButtons()
        {
            try
            {
                foreach (UIElement element in m_SampleListCanvas.Children)
                {
                    string str = element.ToString();
                    if (str.Contains("System.Windows.Controls.Primitives.ToggleButton"))
                    {
                        ToggleButton btn = element as ToggleButton;
                        if (btn.IsChecked == true)
                        {
                            if ((btn.Content as Polygon) != null)
                            {
                                if (m_SetStroke)
                                    (btn.Content as Polygon).Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_Brush.Color.R, m_Brush.Color.G, m_Brush.Color.B));
                                if (m_SetFill)
                                    (btn.Content as Polygon).Fill = new SolidColorBrush(Color.FromArgb(m_FillTransparency, m_Brush.Color.R, m_Brush.Color.G, m_Brush.Color.B));
                            }
                            if ((btn.Content as Polyline) != null)
                            {
                                if (m_SetStroke)
                                    (btn.Content as Polyline).Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_Brush.Color.R, m_Brush.Color.G, m_Brush.Color.B));
                            }
                            if ((btn.Content as System.Windows.Shapes.Path) != null)
                            {
                                if (m_SetStroke)
                                    (btn.Content as System.Windows.Shapes.Path).Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_Brush.Color.R, m_Brush.Color.G, m_Brush.Color.B));
                                if (m_SetFill)
                                    (btn.Content as System.Windows.Shapes.Path).Fill = new SolidColorBrush(Color.FromArgb(m_FillTransparency, m_Brush.Color.R, m_Brush.Color.G, m_Brush.Color.B));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Switches the accessibility of the Delete button for the reference map according to the appropriate flag.
        /// </summary>
        private void UpdateRefMapDeleteButtons()
        {
            try
            {
                foreach (UIElement element in m_SampleListCanvas.Children)
                {
                    string str = element.ToString();
                    // Identify the delete buttons by tag "delete" (tagDel)
                    if (str == "System.Windows.Controls.Button")
                    {
                        Button btn = element as Button;
                        if (btn.Tag == null)
                            continue;
                        if (btn.Tag.ToString() != tagDel)
                            continue;
                        // Sample is data context
                        if ((btn.DataContext as SampleImage) == m_RefMap)
                        {
                            btn.IsEnabled = m_WpfShowRefMapDelButton;
                            btn.Opacity = (m_WpfShowRefMapDelButton ? 1.0 : 0.3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a picture from a file to be assigned to a button or label.
        /// </summary>
        /// <param name="fileName">Name of the file with the picture.</param>
        /// <returns></returns>
        public Image GetImageFromFile(string fileName)
        {
            // Create Icon for button
            Uri uri = new Uri(fileName, UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(uri);
            Image image = new Image();
            image.Source = bitmap;
            image.Stretch = Stretch.Uniform;
            return image;
        }

        #endregion // Add and remove shapes (toggle buttons)

        #region Edit shapes

        /// <summary>
        /// Catches a corner point of any visible sample of the working area, 
        /// if it is close to the given cursor position, and changes the curser image.
        /// </summary>
        /// <param name="cursorPos">The current cursor position.</param>
        private void CatchNextSamplePoint(Point cursorPos)
        {
            try
            {
                m_Canvas.Cursor = m_CursorOrg;
                int listIndex = -1;
                int pointIndex = -1;

                // Check samples in sample list
                foreach (Sample sample in m_SampleList)
                {
                    // Catch sample point if visible and image is not ref map
                    if (sample.IsSampleVisible && sample.MatchPoint(cursorPos, ref listIndex, ref pointIndex) && ((sample as SampleImage) != m_RefMap || (sample as SampleImage) == null))
                    {
                        // Switch according sample type
                        switch (sample.TypeOfSample)
                        {
                            case SampleType.POLYGON:
                            case SampleType.MULTIPOLYGON:
                                SamplePolygon samplePolygon = sample as SamplePolygon;
                                m_CatchedPoly = samplePolygon;
                                m_CatchedListIndex = listIndex;
                                m_CatchedPointIndex = pointIndex;
                                m_Canvas.Cursor = Cursors.Hand;
                                return;

                            case SampleType.LINESTRING:
                            case SampleType.MULTILINESTRING:
                                SampleLines sampleLine = sample as SampleLines;
                                m_CatchedLine = sampleLine;
                                m_CatchedListIndex = listIndex;
                                m_CatchedPointIndex = pointIndex;
                                m_Canvas.Cursor = Cursors.Hand;
                                return;

                            case SampleType.POINT:
                            case SampleType.MULTIPOINT:
                                m_CatchedSamplePoints = sample as SamplePoints;
                                m_CatchedPointIndex = pointIndex;
                                m_Canvas.Cursor = Cursors.Hand;
                                return;

                            case SampleType.IMAGE:
                                m_CatchedSampleImage = sample as SampleImage;
                                m_CatchedPointIndex = pointIndex;
                                m_Canvas.Cursor = Cursors.Hand;
                                return;

                            // Ignore other sample types
                            default:
                                break;
                        }
                    }
                }

                // check for current unsaved (multi) sample
                if (m_SamplePolygon != null && (m_SamplePolygon.MatchPoint(cursorPos, ref listIndex, ref pointIndex) || m_SamplePolygon.MatchPoint(cursorPos, ref pointIndex)))
                {
                    // Success, change cursor image and return to caller
                    m_CatchedPoly = m_SamplePolygon;
                    m_CatchedListIndex = listIndex;
                    m_CatchedPointIndex = pointIndex;
                    m_Canvas.Cursor = Cursors.Hand;
                    return;
                }
                if (m_SampleLine != null && (m_SampleLine.MatchPoint(cursorPos, ref listIndex, ref pointIndex) || m_SampleLine.MatchPoint(cursorPos, ref pointIndex)))
                {
                    // Success, change cursor image and return to caller
                    m_CatchedLine = m_SampleLine;
                    m_CatchedListIndex = listIndex;
                    m_CatchedPointIndex = pointIndex;
                    m_Canvas.Cursor = Cursors.Hand;
                    return;
                }
                if (m_SamplePoints != null && m_SamplePoints.MatchPoint(cursorPos, ref pointIndex))
                {
                    // Success, change cursor image and return to caller
                    m_CatchedSamplePoints = m_SamplePoints;
                    m_CatchedPointIndex = pointIndex;
                    m_Canvas.Cursor = Cursors.Hand;
                    return;
                }
                if (m_SampleImage != null && m_SampleImage.MatchPoint(cursorPos, ref pointIndex) && m_SampleImage != m_RefMap)
                {
                    // Success, change cursor image and return to caller
                    m_CatchedSampleImage = m_SampleImage;
                    m_CatchedPointIndex = pointIndex;
                    m_Canvas.Cursor = Cursors.Hand;
                    return;
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Moves a catched sample point to the new given cursor position.
        /// </summary>
        /// <param name="cursorPos">The new cursor position.</param>
        private void MoveSamplePoint(Point cursorPos)
        {
            if (m_CatchedPoly != null)
            {
                if (m_CatchedListIndex == -1)
                    m_CatchedPoly.ChangeSamplePoint(cursorPos, m_CatchedPointIndex);
                else
                    m_CatchedPoly.ChangeSamplePoint(cursorPos, m_CatchedPointIndex, m_CatchedListIndex);
            }
            else if (m_CatchedLine != null)
            {
                if (m_CatchedListIndex == -1)
                    m_CatchedLine.ChangeSamplePoint(cursorPos, m_CatchedPointIndex);
                else
                    m_CatchedLine.ChangeSamplePoint(cursorPos, m_CatchedPointIndex, m_CatchedListIndex);
            }
            else if (m_CatchedSamplePoints != null)
            {
                m_CatchedSamplePoints.ClearSamplePoint(m_CatchedPointIndex);
                m_CatchedSamplePoints.SetSamplePoint(m_CatchedPointIndex, cursorPos);
            }
            else if (m_CatchedSampleImage != null)
            {
                m_CatchedSampleImage.ChangePosition(cursorPos, m_CatchedPointIndex);
            }
        }

        #endregion // Edit shapes

        #region Adjust images

        /// <summary>
        /// Sets a reference point for the map adaption. Clear the pick points when finished.
        /// </summary>
        /// <param name="pickPoint">The new pick point.</param>
        private void SetPickPoint(Point pickPoint)
        {
            if (m_SampleImage == null || m_RefMap == null)
                return;

            // Pick point is source
            if (m_Canvas.InputHitTest(pickPoint) == m_SampleImage.m_Image && m_SampleImage.Transparency > AdaptModeMinTransparency)
            {
                // Check if points have to be sent
                if (!m_PickPointLastSource && m_PickPointTargetSet && m_PickPointSourceSet)
                {
                    // Send and reset
                    m_SampleImage.AddPickPoint(m_PickPointSource, m_PickPointTarget);
                    m_PickPointTargetSet = false;
                    m_PickPointSourceSet = false;
                }
                // Set or change source point
                m_PickPointSource = pickPoint;
                if (!m_PickPointSourceSet)
                {
                    SetPin(m_PickPointSource, true);
                    m_PickPointSourceSet = true;
                }
                else
                {
                    ChangePin(m_PickPointSource, true);
                }
                m_PickPointLastSource = true;
            }
            // Pick point is target
            else // if (m_Canvas.InputHitTest(pickPoint) == m_RefMap.m_Image)
            {
                // Check if points have to be sent
                if (m_PickPointLastSource && m_PickPointTargetSet && m_PickPointSourceSet)
                {
                    // Send and reset
                    m_SampleImage.AddPickPoint(m_PickPointSource, m_PickPointTarget);
                    m_PickPointSourceSet = false;
                    m_PickPointTargetSet = false;
                }
                // Set or change target point
                m_PickPointTarget = pickPoint;
                if (!m_PickPointTargetSet)
                {
                    SetPin(m_PickPointTarget, false);
                    m_PickPointTargetSet = true;
                }
                else
                {
                    ChangePin(m_PickPointTarget, false);
                }
                m_PickPointLastSource = false;
            }

            // All pins are set, change instruction
            if (m_PickPointPinList.Count == PickPointPinListCount)
            {
                SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrMapAdaptModeEndInstruction, true);
            }

            // finish
            if (m_PickPointPinList.Count == (PickPointPinListCount + 1))
            {
                ResetPickPoints();
                radioButtonShift.IsChecked = true;
            }
        }

        /// <summary>
        /// Clear the pick points and pins.
        /// </summary>
        private void ResetPickPoints()
        {
            ClearPins();
            m_PickPointPin = null;
            m_PickPointSourceSet = false;
            m_PickPointTargetSet = false;
            m_CurrentCursorSource = 0;
            m_CurrentCursorTarget = 1;
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrMapAdaptModeSetInstruction, false);
        }

        /// <summary>
        /// Displays a pin at the given pick point position.
        /// </summary>
        /// <param name="pickPoint">The pick point to set.</param>
        /// <param name="source">Select green color, if <c>true</c> (source point), select red color if <c>false</c> (target point).</param>
        private void SetPin(Point pickPoint, bool source)
        {
            // Create a Path object with the Pin to display on the canvas
            m_PickPointPin = CreatePickPointPin(source ? Brushes.Green : Brushes.Red, Brushes.Yellow, m_PickPointPinList.Count);
            // Set to the right position
            Canvas.SetLeft(m_PickPointPin, pickPoint.X);
            Canvas.SetTop(m_PickPointPin, pickPoint.Y);
            // Add it to canvas
            m_Canvas.Children.Add(m_PickPointPin);
            // Add it to Pin list
            m_PickPointPinList.Add(m_PickPointPin);

            // Adapt cursor shape: Increase pick point cursor index according to type of added Pin
            if (m_PickPointPinList.Count > 1)
            {
                if (source)
                {
                    m_CurrentCursorTarget += 2;
                    if (m_CurrentCursorTarget > 6)
                        m_CurrentCursorTarget = 6;
                }
                else
                {
                    m_CurrentCursorSource += 2;
                    if (m_CurrentCursorSource > 6)
                        m_CurrentCursorSource = 6;
                }
            }
        }

        /// <summary>
        /// Creates the pick point pin.
        /// </summary>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="index">The index of the pick point in the list.</param>
        /// <returns>The Path object containing the created Pin shape.</returns>
        private System.Windows.Shapes.Path CreatePickPointPin(Brush stroke, Brush fill, int index)
        {
            // Create a path to draw a geometry.
            System.Windows.Shapes.Path myPath = new System.Windows.Shapes.Path();
            // Create path geometry
            PathGeometry pathGeometry = new PathGeometry();
            myPath.Stroke = stroke;
            myPath.StrokeThickness = 2;
            myPath.Fill = fill;
            myPath.Data = pathGeometry;

            // Calculate the number of points to display in the Pin
            int num = index / 2 + 1;

            // Create a StreamGeometry to use to specify myPath.
            StreamGeometry streamGeometry = new StreamGeometry();
            streamGeometry.FillRule = FillRule.Nonzero;

            // Open a StreamGeometryContext that can be used to describe this StreamGeometry object's contents.
            using (StreamGeometryContext ctx = streamGeometry.Open())
            {
                // Begin the triangle at the point specified. Notice that the shape is set to 
                // be closed so only two lines need to be specified below to make the triangle.
                ctx.BeginFigure(new Point(0, 0), true /* is filled */, true /* is closed */);

                // Draw a line to the next specified point.
                ctx.LineTo(new Point(-10, -30), true /* is stroked */, false /* is smooth join */);

                // Draw another line to the next specified point.
                ctx.LineTo(new Point(10, -30), true /* is stroked */, false /* is smooth join */);
            }

            // Freeze the geometry (make it unmodifiable) for additional performance benefits.
            streamGeometry.Freeze();
            pathGeometry.AddGeometry(streamGeometry);

            // Create points to be shown in the pin indicating the index of the pick point for surce and target
            for (int i = 0; i < num; i++)
            {
                EllipseGeometry ellipseGeometry = new EllipseGeometry(new Point(0, -(13 + i * 6)), 1, 1);
                // Add geometry to objects path geometry collection
                pathGeometry.AddGeometry(ellipseGeometry);
            }
            // Return the Pin shape
            return myPath;
        }


        /// <summary>
        /// Moves the last set pin to the new pick point position.
        /// </summary>
        /// <param name="pickPoint">The pick point.</param>
        /// <param name="source">Select green color, if <c>true</c> (source point), select red color if <c>false</c> (target point).</param>
        private void ChangePin(Point pickPoint, bool source)
        {
            if (m_PickPointPin != null)
            {
                Canvas.SetLeft(m_PickPointPin, pickPoint.X);
                Canvas.SetTop(m_PickPointPin, pickPoint.Y);
            }
        }

        /// <summary>
        /// Clears all pins and removes them from the canvas.
        /// </summary>
        private void ClearPins()
        {
            foreach (System.Windows.Shapes.Path ellipse in m_PickPointPinList)
            {
                m_Canvas.Children.Remove(ellipse);
            }
            m_PickPointPinList.Clear();
        }

        #endregion // Adjust images

        #region Load and save shapes in files

        #region Open and save dialogs

        // Open file dialog
        /// <summary>
        /// Opens a file dialog to select shapes or images from disk.
        /// Loads the selected ones to the working area and adds it to the sample list.
        /// </summary>
        private void OpenSampleFiles()
        {
            try
            {
                // Open file dialog
                OpenFileDialog dlg = new OpenFileDialog();
                // Filter settings
                dlg.Filter = StrFilterAll;
                // Just single files can be selected
                dlg.Multiselect = true;
                dlg.Title = WpfSamplingPlotPage.Properties.Resources.StrOpenFileDialog;
                // Show dialog
                bool? result = dlg.ShowDialog();
                if (result == false)
                    return;
                // Get selected filenames
                string[] str = dlg.FileNames;
                // (The following is a bit obsolete, because we now just support one single image file to be opended)
                // Open image files first
                foreach (string path in str)
                {
                    if (isSupportedImageFile(path))
                    {
                        if (LoadSingleImage(path, m_XPos, 0, 255))
                            radioButtonShift.IsChecked = true;
                    }
                }
                // Open shapes files next to be not covered by images
                foreach (string path in str)
                {
                    if (isSupportedShapeFile(path))
                    {
                        /*
                        if (WorldHasDisappeared())
                        {
                            SetError(WpfSamplingPlotPage.Properties.Resources.ErrMissingReferenceMap);
                            return;
                        }
                        else
                         */
                        {
                            if (path.EndsWith(StrArcGisShapeFileExtension))
                                ReadShapesFromArcFile(path, m_SplitShapes);
                            else if (path.EndsWith(StrShapeFileExtension))
                                ReadShapeFromFile(path);
                            else if (path.EndsWith(StrGpxFileExtension))
                                ReadShapeFromGpxXmlFile(path);
                            else
                                ReadShapeFromTextFile(path);
                        }
                    }
                    else if (!isSupportedImageFile(path))
                    {
                        SetError(string.Format(WpfSamplingPlotPage.Properties.Resources.ErrInvalidFilename, path));
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Opens a SaveFileDialog and saves all visible shapes, image coordinates 
        /// and a scan of the working area (including the samples) to disk.
        /// </summary>
        private void SaveSampleFiles()
        {
            try
            {
                string filename = string.Empty;
                bool sampleListHasShapes = false;

                // Save coordinates of (adapted) images in xml files
                foreach (Sample sample in m_SampleList)
                {
                    if (!sample.IsSampleVisible)
                        continue;

                    if (sample.TypeOfSample == SampleType.IMAGE)
                    {
                        if (sample as SampleImage == m_RefMap)
                            continue;
                        // Write image attributes (coordinates, ...) to xml file
                        WriteImageAttributesToXmlFile(sample as SampleImage);
                        // Write Glopus calibration file
                        if (m_WriteImageToKalFile)
                            WriteImageAttributesToKalFile(sample as SampleImage);
                    }
                    else
                    {
                        sampleListHasShapes = true;
                    }
                }

                // Check if shapes are available or working area should be saved
                if (!m_SaveWorkingArea && !sampleListHasShapes)
                    return;

                // Reset Zoom and margin for scanning working area or frame
                m_Canvas.RenderTransform = new ScaleTransform(1, 1);
                if (m_AreaFrame)
                {
                    // Adapt canvas position
                    Thickness frameCanvasThickness = new Thickness(0, 0, 0, 0);
                    Point framePosition = m_CaptureFrame.FramePosition;
                    frameCanvasThickness.Left -= framePosition.X;
                    frameCanvasThickness.Right += framePosition.X; ;
                    frameCanvasThickness.Top -= framePosition.Y;
                    frameCanvasThickness.Bottom += framePosition.Y; ;
                    // Set new position
                    m_Canvas.Margin = frameCanvasThickness;
                }
                else
                {
                    m_Canvas.Margin = m_CanvasThicknessZero;
                }

                // Save file dialog
                SaveFileDialog dlg = new SaveFileDialog();
                // Filter settings
                dlg.Filter = StrFilterAll;
                dlg.Title = WpfSamplingPlotPage.Properties.Resources.StrSaveShapeDialog;

                // Use the last filename and add/increase a count number
                if (m_WorkingAreaSaveFileName.Length > 4 && m_WorkingAreaSaveFileName[m_WorkingAreaSaveFileName.Length - 4] == '_')
                    m_WorkingAreaSaveFileName = m_WorkingAreaSaveFileName.Remove(m_WorkingAreaSaveFileName.Length - 4);

                if (m_Testcase)
                    dlg.FileName = m_WorkingAreaSaveFileName; //  + m_SaveFileCount.ToString("D3");
                else
                    dlg.FileName = m_WorkingAreaSaveFileName + "_" + m_SaveFileCount.ToString("D3");

                // Show dialog
                if (dlg.ShowDialog() == false)
                    return;
                if (dlg.FileName.EndsWith(StrShapeFileExtension) || dlg.FileName.EndsWith(StrMapFileExtension))
                    dlg.FileName = dlg.FileName.Remove(dlg.FileName.Length - 4);
                if (dlg.FileName.EndsWith("."))
                    dlg.FileName = dlg.FileName.Remove(dlg.FileName.Length - 1);
                int index = dlg.FileName.LastIndexOf('\\');
                m_WorkingAreaSaveFileName = dlg.FileName.Substring(index + 1);
                filename = dlg.FileName;

                // Save shapes in OGC XML file
                // WriteShapeCollectionToXmlFile(dlg.FileName + ".xml");

                // Save shapes in SQL geo object file
                if (m_WriteShapesToSqlFile)
                    WriteShapesToSqlFile(filename);                 // Samples as multiple objects
                //WriteShapesToSeparateSqlFiles(filename);      // One separate file per Sample
                // WriteShapeCollectionToSqlFile(filename);     // alternatively: Samples as one GEOMETRYCOLLECTION
                // Save shapes in ArcView binary files
                if (m_WriteShapesToArcFile)
                    WriteShapesToArcFile(filename);
                // Save shapes in TAB separated import file
                if (m_WriteShapesToImportFile)
                    WriteShapesToImportFile(filename);
                // WriteShapesToSeparateImportFiles(filename);      // One separate file per Sample
                // Save working area to map file
                if (m_SaveWorkingArea)
                    SaveWorkingArea(filename);
                m_SaveFileCount++;
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
            finally
            {
                // Restore Zoom and margin
                m_Canvas.Margin = m_CanvasThickness;
                m_Canvas.RenderTransform = new ScaleTransform(m_ZoomFactor, m_ZoomFactor);
            }
        }

        /// <summary>
        /// Pre-checks if the file is supported.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if the given filename's extension is a supported format; otherwise, <c>false</c>.</returns>
        private bool isSupportedShapeFile(string fileName)
        {
            try
            {
                if ((fileName.ToLower()).EndsWith(StrShapeFileExtension))
                    return true;
                if ((fileName.ToLower()).EndsWith(StrArcGisShapeFileExtension))
                    return true;
                if ((fileName.ToLower()).EndsWith(StrShape2FileExtension))
                    return true;
                if ((fileName.ToLower()).EndsWith(StrGpxFileExtension))
                    return true;
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }

            return false;
        }

        #endregion // Open and save dialogs

        #region Read Samples from AcrView shape file

        /// <summary>
        /// Reads Geoobjects from a binary ArcView shape file at the given path and add them to the sample list.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="split">if set to <c>true</c> split shape file object into separate samples, else save as one single sample.</param>
        private void ReadShapesFromArcFile(string path, bool split)
        {
            if (File.Exists(path))
            {
                uint length = 0;
                uint wCount = 50;
                uint recLen = 0;
                // int count = 0;

                // Read dBase attributes for shape, if available
                List<string> descriptionList = new List<string>();
                List<string> idList = new List<string>();
                if (ReadArcDBaseFile(path, out descriptionList, out idList) == false)
                    return;
                int indDescriptionList = 0;
                int indIdList = 0;

                // invalidate current load sample
                m_LoadSample = null;

                // debug
                DebugAdd(string.Empty);

                using (BinaryReader binReader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    // Get Map type from web site
                    UpdateWebBrowserUriByPos();

                    length = ReadArcFileHeader(binReader);

                    // string labelInstructionText = labelInstruction.Content.ToString();
                    // SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrLoadingShapesInstruction, true);
                    // DateTime start = DateTime.Now;
                    // DoEvents();

                    ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrLoadingShapesInstruction, (int)length);

                    while (wCount < length)
                    {
                        recLen = ReadArcRecord(binReader);
                        wCount += 4 + recLen;

                        // Create separate samples for each shape object
                        if (split)
                        {
                            // Add samples to list...
                            if (m_LoadSample != null)
                            {
                                // Preset Displaytext
                                try
                                {
                                    // m_LoadSample.DisplayText = (path.Substring(path.LastIndexOf('\\') + 1)).Replace(".shp", "");
                                    // m_LoadSample.DisplayText += " (" + (++count).ToString() + ")";
                                    if (indDescriptionList < descriptionList.Count)
                                        m_LoadSample.DisplayText = descriptionList[indDescriptionList];
                                    if (indIdList < idList.Count)
                                        m_LoadSample.Identifier = idList[indIdList];
                                    if (m_LoadSample.DisplayText == string.Empty)
                                    {
                                        m_LoadSample.DisplayText = (path.Substring(path.LastIndexOf('\\') + 1)).Replace(".shp", " ") + (indDescriptionList + 1).ToString();
                                    }
                                    if (m_LoadSample.Identifier == string.Empty)
                                    {
                                        m_LoadSample.Identifier = (indIdList + 1).ToString();
                                    }
                                    // m_LoadSample.Identifier = m_LoadSample.SamplePointCountAll.ToString() + " pnts";
                                    SetIdentifierText(m_LoadSample.Identifier, m_LoadSample.DisplayText);
                                    indDescriptionList++;
                                    indIdList++;
                                }
                                catch
                                {
                                }
                                // Save only if there are points in the list!
                                if (m_LoadSample.SamplePointCountAll > 0)
                                {
                                    // Add Sample to list, if POINT
                                    if (m_LoadSample.TypeOfSample == SampleType.POINT)
                                        (m_LoadSample as SamplePoints).AddSample();
                                    // Save it to list
                                    SaveSample(m_LoadSample);
                                    // Add a Toggle Button to switch the shape on and off
                                    AddToggleButton(m_LoadSample);
                                    switch (m_LoadSample.TypeOfSample)
                                    {
                                        case SampleType.POINT:
                                        case SampleType.MULTIPOINT:
                                            (m_LoadSample as SamplePoints).AddId(m_LoadSample.Identifier, m_LoadSample.DisplayText);
                                            break;
                                        case SampleType.LINESTRING:
                                        case SampleType.MULTILINESTRING:
                                            (m_LoadSample as SampleLines).AddId(m_LoadSample.Identifier, m_LoadSample.DisplayText);
                                            break;
                                        case SampleType.POLYGON:
                                        case SampleType.MULTIPOLYGON:
                                            (m_LoadSample as SamplePolygon).AddId(m_LoadSample.Identifier, m_LoadSample.DisplayText);
                                            break;
                                    }

                                    // Show progress each 1 second
                                    // if (DateTime.Now - start > TimeSpan.FromSeconds(1))
                                    // {
                                    //     start = DateTime.Now;
                                    //     DoEvents();
                                    // }
                                }
                                // Invalidate object
                                m_LoadSample = null;
                            }
                        }
                        ProgressBarUpdate((int)wCount);
                    }

                    // Create one single sample of shape
                    if (!split)
                    {
                        // Add samples to list...
                        if (m_LoadSample != null)
                        {
                            // Preset Displaytext
                            try
                            {
                                if (indDescriptionList < descriptionList.Count)
                                    m_LoadSample.DisplayText = descriptionList[indDescriptionList];
                                if (indIdList < idList.Count)
                                    m_LoadSample.Identifier = idList[indIdList];
                                if (m_LoadSample.DisplayText == string.Empty)
                                {
                                    m_LoadSample.DisplayText = (path.Substring(path.LastIndexOf('\\') + 1)).Replace(".shp", "");
                                    m_LoadSample.Identifier = m_LoadSample.SamplePointCountAll.ToString() + " pnts";
                                }
                                SetIdentifierText(m_LoadSample.Identifier, m_LoadSample.DisplayText);
                                indDescriptionList++;
                                indIdList++;
                            }
                            catch
                            {
                            }
                            // Save only if there are points in the list!
                            if (m_LoadSample.SamplePointCountAll > 0)
                            {
                                // Add Sample to list, if POINT
                                if (m_LoadSample.TypeOfSample == SampleType.POINT)
                                    (m_LoadSample as SamplePoints).AddSample();
                                // Save it to list
                                SaveSample(m_LoadSample);
                                // Add a Toggle Button to switch the shape on and off
                                AddToggleButton(m_LoadSample);
                                switch (m_LoadSample.TypeOfSample)
                                {
                                    case SampleType.POINT:
                                    case SampleType.MULTIPOINT:
                                        (m_LoadSample as SamplePoints).AddId(m_LoadSample.Identifier, m_LoadSample.DisplayText);
                                        break;
                                    case SampleType.LINESTRING:
                                    case SampleType.MULTILINESTRING:
                                        (m_LoadSample as SampleLines).AddId(m_LoadSample.Identifier, m_LoadSample.DisplayText);
                                        break;
                                    case SampleType.POLYGON:
                                    case SampleType.MULTIPOLYGON:
                                        (m_LoadSample as SamplePolygon).AddId(m_LoadSample.Identifier, m_LoadSample.DisplayText);
                                        break;
                                }
                            }
                            // Invalidate object
                            m_LoadSample = null;
                        }
                    }

                    ProgressBarClose();

                    // SetInstructionLabel(labelInstructionText, false);
                }

                // Write to file
                DebugWrite(path + ".txt");
            }
        }

        private int r_Offset = 0;

        /// <summary>
        /// Reads the ArcView binary shape file header.
        /// </summary>
        /// <param name="binReader">The binary reader.</param>
        /// <returns>The total length of the ArcView file in 16-bit words.</returns>
        private uint ReadArcFileHeader(BinaryReader binReader)
        {
            uint length = 0;
            uint code = 0;
            uint version = 0;
            uint type = 0;
            Point minCoord = new Point();
            Point maxCoord = new Point();
            // double[] bounds = new double[4];
            try
            {
                r_Offset = 0;
                code = ConvertEndian(binReader.ReadUInt32());
                ConvertEndian(binReader.ReadUInt32());
                ConvertEndian(binReader.ReadUInt32());
                ConvertEndian(binReader.ReadUInt32());
                ConvertEndian(binReader.ReadUInt32());
                ConvertEndian(binReader.ReadUInt32());
                length = ConvertEndian(binReader.ReadUInt32());
                version = binReader.ReadUInt32();
                type = binReader.ReadUInt32();

                // Reading boundaries, r_Offset will be incremented by 8 each!
                ReadArcPoint(binReader, ref minCoord);
                ReadArcPoint(binReader, ref maxCoord);
                /*
                bounds[0] = binReader.ReadDouble();
                bounds[1] = binReader.ReadDouble();
                bounds[2] = binReader.ReadDouble();
                bounds[3] = binReader.ReadDouble();
                */
                binReader.ReadDouble();
                binReader.ReadDouble();
                binReader.ReadDouble();
                binReader.ReadDouble();
                DebugAdd(string.Format("=={8}==: File header: code = {0}, length = {1}, version = {2}, type = {3}, bounds = {4} {5} {6} {7}", code, length, version, type, minCoord.X, minCoord.Y, maxCoord.X, maxCoord.Y, r_Offset) + StrCR);
                // r_Offset += 50;
                r_Offset += 34;

                // Adapt map to boundaries, if no background map already set
                if (WorldHasDisappeared())
                {
                    WpfSetMap(minCoord, maxCoord);
                    // CallJavaScript();

                    MessageBoxResult result = MessageBox.Show(WpfSamplingPlotPage.Properties.Resources.StrWaitForMap, WpfSamplingPlotPage.Properties.Resources.ErrCaption, MessageBoxButton.YesNoCancel);
                    if (result != MessageBoxResult.Cancel)
                    {
                        if (result == MessageBoxResult.No)
                            OpenSampleFiles();
                        Thread.Sleep(200);
                        // ReadCoordinatesFromClipboard();
                        buttonAddShape_Click(null, null);
                    }
                    else
                        return 0;
                }
            }
            catch
            {
                return 0;
            }
            return length;
        }

        private void DebugWrite(string path)
        {
            if (m_DebugWrite)
            {
                // Write to file
                using (TextWriter streamWriter = new StreamWriter(path))
                {
                    streamWriter.WriteLine(m_ArcShapeStr);
                }
            }
        }

        private void DebugAdd(string str)
        {
            if (m_DebugWrite && m_ArcShapeStr.Length < m_MaxDebugWrite)
            {
                if (str == string.Empty)
                    // Reset debug string
                    m_ArcShapeStr = str;
                else
                    // Add text to debug string
                    m_ArcShapeStr += str;
            }
        }

        /// <summary>
        /// Reads an ArcView binary file record.
        /// </summary>
        /// <param name="binReader">The binary reader.</param>
        /// <returns>The length of the record content in 16-bit words.</returns>
        private uint ReadArcRecord(BinaryReader binReader)
        {
            uint recLength = 0;
            uint recType = 0;
            try
            {
                uint recNum = ConvertEndian(binReader.ReadUInt32());
                // read length of record content (in words)
                recLength = ConvertEndian(binReader.ReadUInt32());
                // Read type of shape
                recType = binReader.ReadUInt32();
                DebugAdd(string.Format("=={3}==: Record #{0}: recLength = {1}, recType = {2}", recNum, recLength, recType, r_Offset) + StrCR);
                r_Offset += 6;

                // Switch according shape type
                switch (recType)
                {
                    case ArcTypePoint:
                    case ArcTypePointZ:
                    case ArcTypePointM:
                        // Read single point
                        Point point = new Point();
                        ReadArcPoint(binReader, ref point);

                        // Skip file data according to shape type
                        uint numDouble = 0;
                        if (recType == ArcTypePointM)
                            numDouble = 1;
                        if (recType == ArcTypePointZ)
                            numDouble = 2;
                        // Read Z value and M value from file
                        for (uint z = 0; z < numDouble; z++)
                        {
                            binReader.ReadDouble();
                            r_Offset += 4;
                        }

                        // Create sample if not yet available
                        if (m_LoadSample == null)
                        {
                            m_LoadSample = new SamplePoints(m_Canvas);
                            (m_LoadSample as SamplePoints).StrokeBrush = m_LastStrokeBrush;
                            (m_LoadSample as SamplePoints).FillBrush = m_LastFillBrush;
                            (m_LoadSample as SamplePoints).StrokeThickness = m_PointsStrokeThickness;
                            (m_LoadSample as SamplePoints).SamplePointSymbol = m_PointSymbol;
                            (m_LoadSample as SamplePoints).SamplePointSymbolSize = m_PointSymbolSize;
                        }
                        // Set next point of sample points
                        (m_LoadSample as SamplePoints).SetSamplePoint(WorldToCanvas(point));
                        m_LoadSample.SetWorldPoint(point);
                        break;

                    case ArcTypeMultiPoint:
                    case ArcTypeMultiPointZ:
                    case ArcTypeMultiPointM:
                        // Read multiple points
                        PointCollection points = new PointCollection();
                        ReadArcMultiPoint(binReader, ref points, recType);
                        // Create sample if not yet available
                        if (m_LoadSample == null)
                        {
                            m_LoadSample = new SamplePoints(m_Canvas);
                            (m_LoadSample as SamplePoints).StrokeBrush = m_LastStrokeBrush;
                            (m_LoadSample as SamplePoints).FillBrush = m_LastFillBrush;
                            (m_LoadSample as SamplePoints).StrokeThickness = m_PointsStrokeThickness;
                            (m_LoadSample as SamplePoints).SamplePointSymbol = m_PointSymbol;
                            (m_LoadSample as SamplePoints).SamplePointSymbolSize = m_PointSymbolSize;
                        }
                        (m_LoadSample as SamplePoints).SetSamplePoint(WorldToCanvas(points));
                        m_LoadSample.SetWorldPoint(points);
                        break;

                    case ArcTypePolyLine:
                    case ArcTypePolyLineZ:
                    case ArcTypePolyLineM:
                        // Read polyline
                        List<PointCollection> LinePointCollectionList = new List<PointCollection>();
                        ReadArcPolyLine(binReader, ref LinePointCollectionList, recType);
                        // Create sample if not yet available
                        if (m_LoadSample == null)
                        {
                            m_LoadSample = new SampleLines(m_Canvas);
                            (m_LoadSample as SampleLines).StrokeBrush = m_LastStrokeBrush;
                            (m_LoadSample as SampleLines).StrokeThickness = m_LineStrokeThickness;
                        }
                        // Fill it with points
                        foreach (PointCollection polyPoints in LinePointCollectionList)
                        {
                            (m_LoadSample as SampleLines).SetSamplePoint(WorldToCanvas(polyPoints));
                            m_LoadSample.SetWorldPoint(polyPoints);
                            // Add Polyline as (Multi-)Sample
                            (m_LoadSample as SampleLines).AddSample();
                            (m_LoadSample as SampleLines).NewSample();
                        }
                        break;

                    case ArcTypePolygon:
                    case ArcTypePolygonZ:
                    case ArcTypePolygonM:
                        // Read polyline
                        List<PointCollection> PolyPointCollectionList = new List<PointCollection>();
                        // Read polygon
                        ReadArcPolygon(binReader, ref PolyPointCollectionList, recType);
                        // Create sample if not yet available
                        if (m_LoadSample == null)
                        {
                            m_LoadSample = new SamplePolygon(m_Canvas);
                            (m_LoadSample as SamplePolygon).StrokeBrush = m_LastStrokeBrush;
                            (m_LoadSample as SamplePolygon).FillBrush = m_LastFillBrush;
                            (m_LoadSample as SamplePolygon).StrokeThickness = m_PolygonStrokeThickness;
                        }

                        // Fill it with points
                        foreach (PointCollection polyPoints in PolyPointCollectionList)
                        {
                            (m_LoadSample as SamplePolygon).SetSamplePoint(WorldToCanvas(polyPoints));

                            // Check if polygon has the right ring orientation, if not: Reverse ist
                            /* !!! Don't know, why Clockwise() has to return false for that !!! (usually true)
                            if (!(m_LoadSample as SamplePolygon).PolygonIsClockwise(polyPoints))
                            {
                                // Reverse collection to fullfill SQL geography restrictions
                                List<Point> samplePointCollection = polyPoints.Reverse().ToList();
                                polyPoints.Clear();
                                foreach (Point pnt in samplePointCollection)
                                    polyPoints.Add(pnt);
                            }
                            */

                            m_LoadSample.SetWorldPoint(polyPoints);
                            // (m_LoadSample as SamplePolygon).SetSamplePoint(WorldToCanvas(CleanUpPointCollection(polyPoints)));
                            // Add Polygon as (Multi-)Sample
                            (m_LoadSample as SamplePolygon).AddSample();
                            (m_LoadSample as SamplePolygon).NewSample();
                        }
                        break;
                }
            }
            catch
            {
                return 0;
            }
            return recLength;
        }

        /// <summary>
        /// Remove duplicate coordinates from point collection to.
        /// </summary>
        /// <param name="polyPoints">The poly points.</param>
        /// <returns></returns>
        private PointCollection CleanUpPointCollection(PointCollection polyPoints)
        {
            double factor = 5;
            PointCollection newPoints = new PointCollection();
            Point refPnt = new Point();
            refPnt = polyPoints[0];
            refPnt.X = (double)((int)(polyPoints[0].X * factor)) / factor;
            refPnt.Y = (double)((int)(polyPoints[0].Y * factor)) / factor;
            newPoints.Add(polyPoints[0]);


            foreach (Point pnt in polyPoints)
            {
                Point pnt2 = new Point((double)((int)(pnt.X * factor)) / factor, (double)((int)(pnt.Y * factor)) / factor);
                if (pnt2 != refPnt || polyPoints.Count < 20)
                {
                    newPoints.Add(pnt);
                    refPnt = pnt2;
                }
            }
            newPoints.Add(polyPoints[0]);

            return newPoints;
        }

        /// <summary>
        /// Reads single point from ArcView file record.
        /// </summary>
        /// <param name="binReader">The binary reader.</param>
        /// <param name="point">Returns the read point.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool ReadArcPoint(BinaryReader binReader, ref Point point)
        {
            try
            {
                double lat = 0;
                double lon = 0;
                // Read point (assume geographic coordinats format)
                point.X = binReader.ReadDouble();
                point.Y = binReader.ReadDouble();

                // Convert to UTM Format, if radio button is selected
                if (m_ReadArcViewShapeFormatUtm)
                {
                    if (Math.Abs(point.X) > 180 || Math.Abs(point.Y) > 90)
                    {
                        // Just need to decide whether hemisphere is north or south. If north, set UTM band to "N", else set UTM band to "M".
                        string zone = m_ReadArcViewShapeFormatUtmZone.ToString() + (m_ReadArcViewShapeFormatUtmHemi == "N" ? "N" : "M");
                        m_GeoCon.CoordWgs84UtmToGeo(zone, point.X, point.Y, ref lon, ref lat);
                        point.X = lon;
                        point.Y = lat;
                    }
                }
                // Assume Gauss-Krüger Format, if not in geographic coordinates range
                else if (Math.Abs(point.X) > 180 || Math.Abs(point.Y) > 90)
                {
                    // Convert GK coords to geographic
                    m_GeoCon.CoordPotsdamGkToGeo(point.X, point.Y, ref lon, ref lat);
                    // Convert Potsdam datum to WGS84
                    m_GeoCon.DatumPotsdamToWgs84(lon, lat, ref lon, ref lat);
                    // GKtoGeo(point.X, point.Y, ref lon, ref lat);
                    point.X = lon;
                    point.Y = lat;
                }

                // Offset for next point to read from file
                r_Offset += 8;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads multiple points from ArcView file record.
        /// </summary>
        /// <param name="binReader">The binary reader.</param>
        /// <param name="points">Returns the read point collection.</param>
        /// <param name="recType">Type of the shape.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool ReadArcMultiPoint(BinaryReader binReader, ref PointCollection points, uint recType)
        {
            uint recNumPoints = 0;
            try
            {
                // Read bounds
                double xMin = binReader.ReadDouble();
                double yMin = binReader.ReadDouble();
                double xMax = binReader.ReadDouble();
                double yMax = binReader.ReadDouble();
                // Read number of points
                recNumPoints = binReader.ReadUInt32();
                for (int i = 0; i < recNumPoints; i++)
                {
                    // Read point
                    Point point = new Point();
                    ReadArcPoint(binReader, ref point);
                    points.Add(point);
                }

                // Skip file data according to shape type
                uint numDouble = 0;
                if (recType == ArcTypeMultiPointM)
                    numDouble = recNumPoints + 2;
                if (recType == ArcTypeMultiPointZ)
                    numDouble = recNumPoints * 2 + 4;
                // Read Z range and array, M range and array from file
                for (uint z = 0; z < numDouble; z++)
                {
                    binReader.ReadDouble();
                    r_Offset += 4;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads a Polyline from ArcView file record.
        /// </summary>
        /// <param name="binReader">The binary reader.</param>
        /// <param name="pointCollectionList">Returns the read point collection list.</param>
        /// <param name="recType">The record type.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool ReadArcPolyLine(BinaryReader binReader, ref List<PointCollection> pointCollectionList, uint recType)
        {
            uint recNumParts = 0;
            uint recNumPoints = 0;
            try
            {
                // Read bounds
                double xMin = binReader.ReadDouble();
                double yMin = binReader.ReadDouble();
                double xMax = binReader.ReadDouble();
                double yMax = binReader.ReadDouble();
                // Read number of lines
                recNumParts = binReader.ReadUInt32();
                // Read total number of points
                recNumPoints = binReader.ReadUInt32();
                // Create array of lines indices
                uint[] lineParts = new uint[recNumParts + 1];
                for (int i = 0; i < recNumParts; i++)
                {
                    lineParts[i] = binReader.ReadUInt32();
                }
                // Add limiter
                lineParts[recNumParts] = recNumPoints;

                // For all line parts
                int pointIndex = 0;
                for (int p = 1; p <= recNumParts; p++)
                {
                    PointCollection points = new PointCollection();
                    // For all points in part
                    for (; pointIndex < lineParts[p]; pointIndex++)
                    {
                        Point point = new Point();
                        ReadArcPoint(binReader, ref point);
                        points.Add(point);
                    }
                    pointCollectionList.Add(points);
                }

                // Skip file data according to shape type
                uint numDouble = 0;
                if (recType == ArcTypePolyLineM)
                    numDouble = recNumPoints + 2;
                if (recType == ArcTypePolyLineZ)
                    numDouble = recNumPoints * 2 + 4;
                // Read Z range and array, M range and array from file
                for (uint z = 0; z < numDouble; z++)
                {
                    binReader.ReadDouble();
                    r_Offset += 4;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads a Polygon from ArcView file record.
        /// </summary>
        /// <param name="binReader">The binary reader.</param>
        /// <param name="pointCollectionList">Returns the read point collection list.</param>
        /// <param name="recType">The record type.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool ReadArcPolygon(BinaryReader binReader, ref List<PointCollection> pointCollectionList, uint recType)
        {
            uint recNumParts = 0;
            uint recNumPoints = 0;

            try
            {
                // Read bounds
                double xMin = binReader.ReadDouble();
                double yMin = binReader.ReadDouble();
                double xMax = binReader.ReadDouble();
                double yMax = binReader.ReadDouble();
                DebugAdd(string.Format("bounds: {0}, {1}, {2}, {3}", xMin, yMin, xMax, yMax) + StrCR);
                // Read number of lines
                recNumParts = binReader.ReadUInt32();
                // Read total number of points
                recNumPoints = binReader.ReadUInt32();
                DebugAdd(string.Format("recNumParts = {0}, recNumPoints = {1}", recNumParts, recNumPoints) + StrCR);
                r_Offset += 20;
                // Create array of lines indices
                DebugAdd("Indices:" + StrCR);
                uint[] lineParts = new uint[recNumParts + 1];
                for (int i = 0; i < recNumParts; i++)
                {
                    lineParts[i] = binReader.ReadUInt32();
                    DebugAdd(string.Format("lineParts[{0}]: {1}", i, lineParts[i]) + StrCR);
                    r_Offset += 2;
                }
                // Add limiter
                lineParts[recNumParts] = recNumPoints;
                DebugAdd("Points:");
                // For all polygon parts
                int pointIndex = 0;
                for (int p = 1; p <= recNumParts; p++)
                {
                    DebugAdd(string.Format("part {0}:", p) + StrCR);
                    PointCollection points = new PointCollection();
                    // For all points in part
                    for (; pointIndex < lineParts[p]; pointIndex++)
                    {
                        Point point = new Point();
                        ReadArcPoint(binReader, ref point);
                        if (pointIndex == 0 || pointIndex == (lineParts[p] - 1))
                            DebugAdd(StrCR + string.Format("point[{0}]: {1}, {2}", pointIndex, point.X, point.Y) + StrCR);
                        else
                            DebugAdd(".");
                        points.Add(point);
                    }
                    pointCollectionList.Add(points);
                }

                // Skip file data according to shape type
                uint numDouble = 0;
                if (recType == ArcTypePolygonM)
                    numDouble = recNumPoints + 2;
                if (recType == ArcTypePolygonZ)
                    numDouble = recNumPoints * 2 + 4;
                // Read Z range and array, M range and array from file
                for (uint z = 0; z < numDouble; z++)
                {
                    binReader.ReadDouble();
                    r_Offset += 4;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Transforms Gauss-Krüger to geographical coordinates.
        /// </summary>
        /// <param name="rw">The horizontal input value.</param>
        /// <param name="hw">The vertical input value.</param>
        /// <param name="lon">Returns the calculated longitude value.</param>
        /// <param name="lat">Returns the calculated latitude value.</param>
        private void GKtoGeo(double rw, double hw, ref double lon, ref double lat)
        {
            // Assume that they are Gauss-Krüger coords
            double rho = 180 / Math.PI;
            double e2 = 0.0067192188;
            double c = 6398786.849;
            int mKen = (int)(rw / 1000000);
            double rm = rw - mKen * 1000000 - 500000;
            double bI = hw / 10000855.7646;
            double bII = bI * bI;
            double bf = 325632.08677 * bI * ((((((0.00000562025 * bII - 0.00004363980) * bII + 0.00022976983)
                      * bII - 0.00113566119) * bII + 0.00424914906) * bII - 0.00831729565) * bII + 1);
            bf = bf / 3600 / rho;
            double co = Math.Cos(bf);
            double g2 = e2 * (co * co);
            double g1 = c / Math.Sqrt(1 + g2);
            double t = Math.Sin(bf) / Math.Cos(bf);
            double fa = rm / g1;
            double gb = bf - fa * fa * t * (1 + g2) / 2 + fa * fa * fa * fa * t * (5 + 3 * t * t + 6 * g2 - 6 * g2 * t * t) / 24;
            // "Geographische Breite" (Latitude):
            lat = gb * rho;
            double dl = fa - fa * fa * fa * (1 + 2 * t * t + g2) / 6 + fa * fa * fa * fa * fa * (1 + 28 * t * t + 24 * t * t * t * t) / 120;
            // "Geographische Länge" (Longitude):
            lon = dl * rho / co + mKen * 3;
        }

        #endregion // Read Samples from AcrView shape file

        #region Read Samples from SQL shape file

        /// <summary>
        /// Reads Geoobjects from an ASCII file (SQL notation) at the given path and add them to the sample list.
        /// </summary>
        /// <param name="path">The file path.</param>
        private void ReadShapeFromFile(string path)
        {
            try
            {
                using (TextReader streamReader = new StreamReader(path))
                {
                    // Read complete text file
                    string str = streamReader.ReadToEnd();
                    // Get Map type from web site
                    UpdateWebBrowserUriByPos();
                    // MessageBox.Show(m_WebMapType.ToString());
                    // Create a shape according to identifier string
                    CreateShapeFromString(str);
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        #endregion // Read Samples from SQL shape file

        #region Read Samples from TAB separated text file

        internal int[] m_Assignment = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        internal bool m_CenterPoint = false;

        /// <summary>
        /// Reads Geoobjects from an TAB separated text file at the given path and add them to the sample list.
        /// </summary>
        /// <param name="path">The file path.</param>
        private void ReadShapeFromTextFile(string path)
        {
            try
            {
                int fileLength = 0;
                using (TextReader streamReader = new StreamReader(path))
                {
                    // Read complete file to calculate the length
                    string line = streamReader.ReadToEnd();
                    fileLength = line.Length;
                }


                using (TextReader streamReader = new StreamReader(path))
                {
                    // Progressbar parameters
                    int numBytesRead = 0;
                    int lineNumber = 0;
                    bool saveAsSingleSample = false;
                    string error = string.Empty;

                    // Read first line of text file
                    string line = streamReader.ReadLine();
                    numBytesRead += line.Length;

                    // Check if empty file
                    if (line == null)
                        return;

                    // Select column values from string and convert to SQL-Format

                    List<string> row = new List<string>();
                    string[] tabStrings = line.Split(charSeparators9, StringSplitOptions.None);
                    foreach (string str in tabStrings)
                    {
                        row.Add(str);
                    }

                    // Assign Geoobject Parameters dialog
                    AssignGeoobjectParameters m_AssignGeoobjectParameters = new AssignGeoobjectParameters(this, row);

                    // Show Dialog
                    bool? dialogResult = m_AssignGeoobjectParameters.ShowDialog();
                    if (dialogResult == false)
                        return;

                    // Check if single sample
                    saveAsSingleSample = m_AssignGeoobjectParameters.SaveAsSingleSample;

                    // Display progress bar
                    ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrReadingSamplesFromFile, fileLength);

                    lineNumber++;
                    // Get result string with Geo-Object from first line
                    string strSample = m_AssignGeoobjectParameters.GetResult(row);

                    if (strSample == string.Empty)
                    {
                        error = WpfSamplingPlotPage.Properties.Resources.ErrInvalidLines + StrCR + WpfSamplingPlotPage.Properties.Resources.ErrHeaderLine;
                    }


                    string tmpStr = strSample;

                    if (saveAsSingleSample)
                        strSample = tmpStr.Replace("POINT", "MULTIPOINT");

                    // -- no, do it line by line!  Create shapes according to sample string
                    // CreateShapeFromString(strSample);

                    // For all lines remaining
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        // Update progress bar
                        numBytesRead += line.Length;
                        ProgressBarUpdate(numBytesRead);
                        // Cancel action
                        if (m_ProgressCancel)
                            break;

                        // Get sample string
                        row.Clear();
                        tabStrings = line.Split(charSeparators9, StringSplitOptions.None);
                        foreach (string str in tabStrings)
                        {
                            row.Add(str);
                        }

                        lineNumber++;
                        // Assemble sample string
                        string retStr = m_AssignGeoobjectParameters.GetResult(row);
                        if (retStr == string.Empty)
                        {
                            if (error == string.Empty)
                                error = WpfSamplingPlotPage.Properties.Resources.ErrInvalidLines + StrCR + lineNumber.ToString();
                            else
                                error += ", " + lineNumber.ToString();
                        }
                        else
                            strSample += retStr;
                    }

                    // Finished
                    ProgressBarUpdate(fileLength);
                    ProgressBarClose();

                    // Finally create single sample, replace last ", " string by ")"
                    if (saveAsSingleSample)
                    {
                        tmpStr = strSample.Substring(0, strSample.Length - 2);
                        strSample = tmpStr + ")" + StrCR;
                    }

                    bool showSamples = true;
                    if (error != string.Empty)
                    {
                        error += StrCR + WpfSamplingPlotPage.Properties.Resources.StrShowSamples;
                        showSamples = SetRequest(error);
                    }

                    if (showSamples)
                    {
                        UpdateWebBrowserUriByPos();
                        CreateShapeFromString(strSample);
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        #endregion // Read Samples from SQL shape file

        #region Read Samples from GPX file

        private string[] stringSeparators2 = new string[] { "<trkseg>", "</trkseg>" };
        private string[] stringSeparators3 = new string[] { "<trkpt", "</trkpt>" };
        //  " lat=\"47.5685453\" lon=\"11.0766435\"><ele>842</ele>"
        private string[] stringSeparators4 = new string[] { " ", "lat=", "lon=", "\"", "><ele>", "</ele>" };
        private char[] trimLastComma = new char[] { ' ', ',' };
        private char[] trimLastBlank = new char[] { ' ' };

        /*
                private void ReadShapeFromGpxFile(string path)
                {
                    try
                    {
                        int fileLength = 0;
                        string fileContent = string.Empty;

                        // Read complete file
                        using (TextReader streamReader = new StreamReader(path))
                        {
                            // Read complete file to calculate the length
                            fileContent = streamReader.ReadToEnd();
                            fileLength = fileContent.Length;
                        }

                        // Split content into segments (trkseg)
                        List<string> segments = new List<string>();
                        string[] segmentStrings = fileContent.Split(stringSeparators2, StringSplitOptions.None);
                        foreach (string str in segmentStrings)
                        {
                            string str2 = str.Replace("\r\n", "");
                            string str3 = str.Replace("\n", "");
                            if (str3.StartsWith("<trkpt"))
                                segments.Add(str3);
                        }

                        // For all segments: Get points (latitude, longitude, elevation)
                        foreach (string str in segments)
                        {
                            // Split content into trank points (trkpt)
                            List<string> trackPoints = new List<string>();
                            string[] trackPointStrings = str.Split(stringSeparators3, StringSplitOptions.RemoveEmptyEntries);

                            //  " lat=\"47.5685453\" lon=\"11.0766435\"><ele>842</ele>"
                            // For all trackpoint strings: Split into latitude, longitude and elevation (optional)
                            if (trackPointStrings.Length > 1)
                            {
                                string newLineString = "01" + StrCR + 
                                    "Test" + StrCR + 
                                    m_LastStrokeBrush.ToString() + StrCR + 
                                    m_LastFillBrush.ToString()  + StrCR + 
                                    m_StrokeTransparency.ToString(CultureInfo.InvariantCulture)  + StrCR + 
                                    m_FillTransparency.ToString(CultureInfo.InvariantCulture) + StrCR + 
                                    m_LineStrokeThickness.ToString(CultureInfo.InvariantCulture) + StrCR + 
                                    m_PointSymbol.ToString() + StrCR + 
                                    m_PointSymbolSize.ToString(CultureInfo.InvariantCulture) + StrCR + 
                                    "LINESTRING(";

                                foreach (string str1 in trackPointStrings)
                                {
                                    string[] strLonLatAlt = str1.Split(stringSeparators4, StringSplitOptions.RemoveEmptyEntries);
                                    if (strLonLatAlt.Length < 2)
                                        continue;
                                    newLineString += strLonLatAlt[1] + " " + strLonLatAlt[0];
                                    if (strLonLatAlt.Length >= 3)
                                    {
                                        newLineString += " " + strLonLatAlt[2];
                                    }
                                    newLineString += ", ";
                                }

                                newLineString = newLineString.TrimEnd(trimLastComma) + ")" + StrCR;

                                UpdateWebBrowserUriByPos();
                                CreateShapeFromString(newLineString);
                                break;

                            }
                           //  if (trackPointStrings.Length > 1)
                        }
        
        */
        /*

                          // Progressbar parameters
                          int numBytesRead = 0;
                          int lineNumber = 0;
                          bool saveAsSingleSample = false;
                          string error = string.Empty;

                          // Check if empty file
                          if (line == null)
                              return;

                          // Select column values from string and convert to SQL-Format

                          List<string> row = new List<string>();
                          string[] tabStrings = line.Split(charSeparators9, StringSplitOptions.None);
                          foreach (string str in tabStrings)
                          {
                              row.Add(str);
                          }

                          // Assign Geoobject Parameters dialog
                          AssignGeoobjectParameters m_AssignGeoobjectParameters = new AssignGeoobjectParameters(this, row);

                          // Show Dialog
                          bool? dialogResult = m_AssignGeoobjectParameters.ShowDialog();
                          if (dialogResult == false)
                              return;

                          // Check if single sample
                          saveAsSingleSample = m_AssignGeoobjectParameters.SaveAsSingleSample;

                          // Display progress bar
                          ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrReadingSamplesFromFile, fileLength);

                          lineNumber++;
                          // Get result string with Geo-Object from first line
                          string strSample = m_AssignGeoobjectParameters.GetResult(row);

                          if (strSample == string.Empty)
                          {
                              error = WpfSamplingPlotPage.Properties.Resources.ErrInvalidLines + StrCR + WpfSamplingPlotPage.Properties.Resources.ErrHeaderLine;
                          }


                          string tmpStr = strSample;

                          if (saveAsSingleSample)
                              strSample = tmpStr.Replace("POINT", "MULTIPOINT");

                          // -- no, do it line by line!  Create shapes according to sample string
                          // CreateShapeFromString(strSample);

                          // For all lines remaining
                          while ((line = streamReader.ReadLine()) != null)
                          {
                              // Update progress bar
                              numBytesRead += line.Length;
                              ProgressBarUpdate(numBytesRead);
                              // Cancel action
                              if (m_ProgressCancel)
                                  break;

                              // Get sample string
                              row.Clear();
                              tabStrings = line.Split(charSeparators9, StringSplitOptions.None);
                              foreach (string str in tabStrings)
                              {
                                  row.Add(str);
                              }

                              lineNumber++;
                              // Assemble sample string
                              string retStr = m_AssignGeoobjectParameters.GetResult(row);
                              if (retStr == string.Empty)
                              {
                                  if (error == string.Empty)
                                      error = WpfSamplingPlotPage.Properties.Resources.ErrInvalidLines + StrCR + lineNumber.ToString();
                                  else
                                      error += ", " + lineNumber.ToString();
                              }
                              else
                                  strSample += retStr;
                          }

                          // Finished
                          ProgressBarUpdate(fileLength);
                          ProgressBarClose();

                          // Finally create single sample, replace last ", " string by ")"
                          if (saveAsSingleSample)
                          {
                              tmpStr = strSample.Substring(0, strSample.Length - 2);
                              strSample = tmpStr + ")" + StrCR;
                          }

                          bool showSamples = true;
                          if (error != string.Empty)
                          {
                              error += StrCR + WpfSamplingPlotPage.Properties.Resources.StrShowSamples;
                              showSamples = SetRequest(error);
                          }

                          if (showSamples)
                          {
                              UpdateWebBrowserUriByPos();
                              CreateShapeFromString(strSample);
                          }

       */
        /*

                 }
                 catch (Exception ex)
                 {
                     SetError(ex.ToString() + " - " + ex.Message);
                 }
             }
      */

        private void ReadShapeFromGpxXmlFile(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@path);

                // Add a namespace prefix if a default namespace xmlns has been defined in the input file, otherwise SelectNodes will fail
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                string nsmgrPrefix = string.Empty;
                string name = string.Empty;
                string desc = string.Empty;

                if (doc.DocumentElement.Attributes["xmlns"] != null)
                {
                    string xmlns = doc.DocumentElement.Attributes["xmlns"].Value;
                    nsmgr.AddNamespace("MsBuild", xmlns);
                    nsmgrPrefix = "MsBuild:";
                }

                // Extract track: <gpx> - <trk> - <name>,<desc>,<trkseg> - <trkpt>...

                string m_RootElement = string.Format("//{0}gpx/{0}trk", nsmgrPrefix);
                XmlNodeList nodeTrackList = doc.DocumentElement.SelectNodes(m_RootElement, nsmgr);
                if (nodeTrackList != null && nodeTrackList.Count > 0)
                {
                    name = ReadNode(nodeTrackList[0], string.Format("{0}name", nsmgrPrefix), nsmgr);
                    desc = ReadNode(nodeTrackList[0], string.Format("{0}desc", nsmgrPrefix), nsmgr);
                }

                m_RootElement = string.Format("//{0}gpx/{0}trk/{0}trkseg/{0}trkpt", nsmgrPrefix);
                XmlNodeList nodesTrack = doc.DocumentElement.SelectNodes(m_RootElement, nsmgr);

                m_RootElement = string.Format("//{0}wpt", nsmgrPrefix);
                XmlNodeList nodesWaypoints = doc.DocumentElement.SelectNodes(m_RootElement, nsmgr);

                int nodesCount = nodesTrack.Count + nodesWaypoints.Count;
                int count = 0;

                // Display progress bar
                ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrReadingSamplesFromFile, nodesCount);

                string newLineString = name + StrCR +
                    desc + StrCR +
                    m_LastStrokeBrush.ToString() + StrCR +
                    m_LastFillBrush.ToString() + StrCR +
                    m_StrokeTransparency.ToString(CultureInfo.InvariantCulture) + StrCR +
                    m_FillTransparency.ToString(CultureInfo.InvariantCulture) + StrCR +
                    m_LineStrokeThickness.ToString(CultureInfo.InvariantCulture) + StrCR +
                    m_PointSymbol.ToString() + StrCR +
                    m_PointSymbolSize.ToString(CultureInfo.InvariantCulture) + StrCR +
                    "LINESTRING(";

                if (nodesTrack.Count > 1)
                {
                    foreach (XmlNode node in nodesTrack)
                    {
                        //XmlNodeList subNodes = doc.DocumentElement.SelectNodes("/" + subNode);

                        string lon = node.Attributes["lon"].Value;
                        string lat = node.Attributes["lat"].Value;
                        string ele = ReadNode(node, string.Format("{0}ele", nsmgrPrefix), nsmgr);
                        string ret = (string.Format("{0} {1} {2}", lon, lat, ele)).TrimEnd(trimLastBlank);
                        newLineString += ret + ", ";

                        count++;
                        ProgressBarUpdate(count);
                    }
                    newLineString = newLineString.TrimEnd(trimLastComma) + ")" + StrCR;

                    // Create and show GeoObject
                    UpdateWebBrowserUriByPos();
                    CreateShapeFromString(newLineString);
                }

                // Extract way points

                string waypoints = string.Empty;
                string newPointString = "{0}" + StrCR +
                    "{1}" + StrCR +
                    m_LastStrokeBrush.ToString() + StrCR +
                    m_LastFillBrush.ToString() + StrCR +
                    m_StrokeTransparency.ToString(CultureInfo.InvariantCulture) + StrCR +
                    m_FillTransparency.ToString(CultureInfo.InvariantCulture) + StrCR +
                    m_PointsStrokeThickness.ToString(CultureInfo.InvariantCulture) + StrCR +
                    m_PointSymbol.ToString() + StrCR +
                    m_PointSymbolSize.ToString(CultureInfo.InvariantCulture) + StrCR;
                /*
                if (nodes.Count > 1)
                    newPointString += "MULTIPOINT(";
                else
                */
                newPointString += "POINT(";
                if (nodesWaypoints.Count > 0)
                {
                    int index = 0;
                    foreach (XmlNode node in nodesWaypoints)
                    {
                        //XmlNodeList subNodes = doc.DocumentElement.SelectNodes("/" + subNode);
                        index++;
                        string lon = node.Attributes["lon"].Value;
                        string lat = node.Attributes["lat"].Value;
                        string ele = ReadNode(node, string.Format("{0}ele", nsmgrPrefix), nsmgr);
                        name = ReadNode(node, string.Format("{0}name", nsmgrPrefix), nsmgr);
                        desc = ReadNode(node, string.Format("{0}desc", nsmgrPrefix), nsmgr);
                        string ret = (string.Format("{0} {1} {2}", lon, lat, ele)).TrimEnd(trimLastBlank);
                        // newPointString += ret + ", ";
                        waypoints += string.Format(newPointString, (name == string.Empty ? index.ToString() : name), desc) + ret + ")" + StrCR;

                        count++;
                        ProgressBarUpdate(count);
                    }
                    // newPointString = newPointString.TrimEnd(trimLastComma) + ")" + StrCR;

                    ProgressBarUpdate(nodesCount);
                    ProgressBarClose();

                    // Create and show GeoObject
                    UpdateWebBrowserUriByPos();
                    CreateShapeFromString(waypoints);
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }


        private string ReadNode(XmlNode node, string path, XmlNamespaceManager nsmgr)
        {
            string result = string.Empty;
            XmlNode subNode = node.SelectSingleNode(path, nsmgr);

            if (subNode != null)
            {
                result = subNode.InnerText.ToString(CultureInfo.InvariantCulture);
            }
            return result;
        }

        #endregion // Read Samples from GPX file

        #region Write Samples to AcrView shape file

        // Constants
        private const uint ArcFileCode = 9994;
        private const uint ArcVersion = 1000;
        private const uint ArcTypePoint = 1;
        private const uint ArcTypePointZ = 11;
        private const uint ArcTypePointM = 21;
        private const uint ArcTypeMultiPoint = 8;
        private const uint ArcTypeMultiPointZ = 18;
        private const uint ArcTypeMultiPointM = 28;
        private const uint ArcTypePolyLine = 3;
        private const uint ArcTypePolyLineZ = 13;
        private const uint ArcTypePolyLineM = 23;
        private const uint ArcTypePolygon = 5;
        private const uint ArcTypePolygonZ = 15;
        private const uint ArcTypePolygonM = 25;
        private string m_ArcShapeStr = string.Empty;

        /// <summary>
        /// Save all visible Geoobjects of the sample list in ArcView compatible binary files.
        /// Creates a shape file (.shp), an index file (.shx) and a dBase file (.dbx) holding ID and description.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool WriteShapesToArcFile(string path)
        {
            try
            {
                int count = NumberOfPointsInSampleList();
                // Init Progressbar with maxCount
                ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressSavingShape, count);

                int num = 1;
                // Create a separate shape file for each visible sample
                foreach (Sample sample in m_SampleList)
                {
                    if (!sample.IsSampleVisible)
                        continue;

                    /* Cut path file name to 8 chars + 3 ext and add numbers for each sample
                    string newPath = path;
                    int index1 = path.LastIndexOf("\\") + 1;
                    if (path.Length - index1 > 5)
                        newPath = path.Substring(0, index1 + 5);
                    string shapePath = String.Format("{0}~{1}{2}", newPath, (num).ToString("D2"), StrArcGisShapeFileExtension);
                    string indexPath = String.Format("{0}~{1}{2}", newPath, (num++).ToString("D2"), StrArcGisIndexFileExtension);
                    */

                    // Extract File name from path
                    int index1 = path.LastIndexOf("\\") + 1;
                    int index2 = path.LastIndexOf(".");
                    string name = path.Substring(index1, path.Length - index1);
                    string dBasePath = path.Substring(0, index1 - 1);
                    if (index2 > index1)
                    {
                        name = path.Substring(index1, index2 - index1);
                    }
                    else
                    {
                        name = path.Substring(index1, path.Length - index1);
                    }

                    // Remove file name extension if any
                    if (index2 > 0)
                        path = path.Substring(0, index2);
                    // Add .shp/.shx extensions to file name
                    string shapePath = String.Format("{0}_{1}{2}", path, (num).ToString("D2"), StrArcGisShapeFileExtension);
                    string indexPath = String.Format("{0}_{1}{2}", path, (num).ToString("D2"), StrArcGisIndexFileExtension);
                    string dBaseFile = String.Format("{0}_{1}{2}", name, (num).ToString("D2"), StrArcGisDBaseFileExtension);

                    // Preset
                    uint fileLength = 50;
                    uint shapeType = 0;
                    double[] shapeBounds = new double[] { 0, 0, 0, 0 }; // xMin, yMin, xMax, yMax
                    uint recLength = 0;
                    uint recIndex = 0;
                    uint numPoints = 0;
                    uint numParts = 0;
                    bool init = true;

                    // Create a separate ArcFile for each sample
                    switch (sample.TypeOfSample)
                    {
                        // Create type "Point" with one record fo each point
                        case SampleType.POINT:
                        case SampleType.MULTIPOINT:
                            shapeType = ArcTypePoint;
                            // Records content length (without record header)
                            recLength = 10;
                            SamplePoints samplePoint = sample as SamplePoints;
                            numPoints = (uint)samplePoint.SamplePointCollection.Count;
                            if (numPoints <= 0)
                                continue;
                            fileLength += (uint)samplePoint.SamplePointCollection.Count * (recLength + 4);
                            Point first = CanvasToWorld(samplePoint.SamplePointCollection[0]);
                            shapeBounds[0] = shapeBounds[2] = first.X;
                            shapeBounds[1] = shapeBounds[3] = first.Y;
                            foreach (Point pnt in samplePoint.SamplePointCollection)
                            {
                                Point wPnt = CanvasToWorld(pnt);
                                shapeBounds[0] = (wPnt.X < shapeBounds[0] ? wPnt.X : shapeBounds[0]);
                                shapeBounds[1] = (wPnt.Y < shapeBounds[1] ? wPnt.Y : shapeBounds[1]);
                                shapeBounds[2] = (wPnt.X > shapeBounds[2] ? wPnt.X : shapeBounds[2]);
                                shapeBounds[3] = (wPnt.Y > shapeBounds[3] ? wPnt.Y : shapeBounds[3]);
                            }
                            // Write to shape file
                            using (BinaryWriter streamWriter = new BinaryWriter(File.Open(shapePath, FileMode.Create)))
                            {
                                WriteArcFileHeader(streamWriter, fileLength, shapeType, shapeBounds);
                                foreach (Point pnt in samplePoint.SamplePointCollection)
                                {
                                    WriteArcFileRecord(streamWriter, ++recIndex, recLength, shapeType, pnt);
                                }
                            }
                            // Write to index file
                            using (BinaryWriter streamWriter = new BinaryWriter(File.Open(indexPath, FileMode.Create)))
                            {
                                WriteArcFileHeader(streamWriter, 50 + (4 * recIndex), shapeType, shapeBounds);
                                uint startIndex = 50;
                                for (int i = 0; i < recIndex; i++)
                                {
                                    WriteArcFileRecord(streamWriter, startIndex, recLength);
                                    startIndex += 4 + recLength;
                                }
                            }
                            // Write to dBase file
                            WriteArcDBaseFile(dBasePath, dBaseFile, sample);
                            num++;
                            break;

                        /*
                        case SampleType.MULTIPOINT:
                            shapeType = ArcTypeMultiPoint;
                            SamplePoints samplePoints = sample as SamplePoints;
                            numPoints = (uint)samplePoints.SamplePointCollection.Count;
                            if (numPoints <= 0)
                                continue;
                            // Records content length (without record header)
                            recLength = 20 + numPoints * 2;
                            fileLength += recLength + 4;
                            Point mFirst = CanvasToWorld(samplePoints.SamplePointCollection[0]);
                            shapeBounds[0] = shapeBounds[2] = mFirst.X;
                            shapeBounds[1] = shapeBounds[3] = mFirst.Y;
                            foreach (Point pnt in samplePoints.SamplePointCollection)
                            {
                                Point wPnt = CanvasToWorld(pnt);
                                shapeBounds[0] = (wPnt.X < shapeBounds[0] ? wPnt.X : shapeBounds[0]);
                                shapeBounds[1] = (wPnt.Y < shapeBounds[1] ? wPnt.Y : shapeBounds[1]);
                                shapeBounds[2] = (wPnt.X > shapeBounds[2] ? wPnt.X : shapeBounds[2]);
                                shapeBounds[3] = (wPnt.Y > shapeBounds[3] ? wPnt.Y : shapeBounds[3]);
                            }
                            // Write to shape file
                            using (BinaryWriter streamWriter = new BinaryWriter(File.Open(shapePath, FileMode.Create)))
                            {
                                WriteArcFileHeader(streamWriter, fileLength, shapeType, shapeBounds);
                                WriteArcFileRecord(streamWriter, ++recIndex, recLength, shapeType, numPoints, shapeBounds, samplePoints.SamplePointCollection);
                            }
                            // Write to index file
                            using (BinaryWriter streamWriter = new BinaryWriter(File.Open(indexPath, FileMode.Create)))
                            {
                                WriteArcFileHeader(streamWriter, 50 + (4 * recIndex), shapeType, shapeBounds);
                                WriteArcFileRecord(streamWriter, 50, recLength);
                            }
                            // Write to dBase file
                            WriteArcDBaseFile(dBasePath);
                            break;
                        */
                        // Create type "PolyLine" with one record and several parts
                        case SampleType.LINESTRING:
                        case SampleType.MULTILINESTRING:
                            shapeType = ArcTypePolyLine;
                            SampleLines sampleLines = sample as SampleLines;
                            numParts = (uint)sampleLines.SamplePointCollectionList.Count;
                            if (numParts == 0)
                                continue;
                            // Records content length (without record header)
                            recLength = 22 + numParts * 2;

                            foreach (PointCollection pntCol in sampleLines.SamplePointCollectionList)
                            {
                                numPoints += (uint)pntCol.Count;
                                if (init)
                                {
                                    Point lFirst = CanvasToWorld(pntCol[0]);
                                    shapeBounds[0] = shapeBounds[2] = lFirst.X;
                                    shapeBounds[1] = shapeBounds[3] = lFirst.Y;
                                    init = false;
                                }
                                foreach (Point pnt in pntCol)
                                {
                                    Point wPnt = CanvasToWorld(pnt);
                                    shapeBounds[0] = (wPnt.X < shapeBounds[0] ? wPnt.X : shapeBounds[0]);
                                    shapeBounds[1] = (wPnt.Y < shapeBounds[1] ? wPnt.Y : shapeBounds[1]);
                                    shapeBounds[2] = (wPnt.X > shapeBounds[2] ? wPnt.X : shapeBounds[2]);
                                    shapeBounds[3] = (wPnt.Y > shapeBounds[3] ? wPnt.Y : shapeBounds[3]);
                                }
                            }
                            recLength += numPoints * 8;
                            fileLength += recLength + 4;

                            // Write to shape file
                            using (BinaryWriter streamWriter = new BinaryWriter(File.Open(shapePath, FileMode.Create)))
                            {
                                WriteArcFileHeader(streamWriter, fileLength, shapeType, shapeBounds);
                                WriteArcFileRecord(streamWriter, ++recIndex, recLength, shapeType, numPoints, shapeBounds, sampleLines.SamplePointCollectionList);
                            }
                            // Write to index file
                            using (BinaryWriter streamWriter = new BinaryWriter(File.Open(indexPath, FileMode.Create)))
                            {
                                WriteArcFileHeader(streamWriter, 50 + (4 * recIndex), shapeType, shapeBounds);
                                WriteArcFileRecord(streamWriter, 50, recLength);
                            }
                            // Write to dBase file
                            WriteArcDBaseFile(dBasePath, dBaseFile, sample);
                            num++;
                            break;

                        // Create type "Polygon" with one record and several parts
                        case SampleType.POLYGON:
                        case SampleType.MULTIPOLYGON:
                            shapeType = ArcTypePolygon;
                            SamplePolygon samplePolygon = sample as SamplePolygon;
                            numParts = (uint)samplePolygon.SamplePointCollectionList.Count;
                            if (numParts == 0)
                                continue;
                            // Records content length (without record header)
                            recLength = 22 + numParts * 2;

                            foreach (PointCollection pntCol in samplePolygon.SamplePointCollectionList)
                            {
                                numPoints += (uint)pntCol.Count;
                                if (init)
                                {
                                    Point pFirst = CanvasToWorld(pntCol[0]);
                                    shapeBounds[0] = shapeBounds[2] = pFirst.X;
                                    shapeBounds[1] = shapeBounds[3] = pFirst.Y;
                                    init = false;
                                }
                                foreach (Point pnt in pntCol)
                                {
                                    Point wPnt = CanvasToWorld(pnt);
                                    shapeBounds[0] = (wPnt.X < shapeBounds[0] ? wPnt.X : shapeBounds[0]);
                                    shapeBounds[1] = (wPnt.Y < shapeBounds[1] ? wPnt.Y : shapeBounds[1]);
                                    shapeBounds[2] = (wPnt.X > shapeBounds[2] ? wPnt.X : shapeBounds[2]);
                                    shapeBounds[3] = (wPnt.Y > shapeBounds[3] ? wPnt.Y : shapeBounds[3]);
                                }
                            }
                            recLength += numPoints * 8;
                            fileLength += recLength + 4;

                            // Write to shape file
                            using (BinaryWriter streamWriter = new BinaryWriter(File.Open(shapePath, FileMode.Create)))
                            {
                                WriteArcFileHeader(streamWriter, fileLength, shapeType, shapeBounds);
                                WriteArcFileRecord(streamWriter, ++recIndex, recLength, shapeType, numPoints, shapeBounds, samplePolygon.SamplePointCollectionList);
                            }
                            // Write to index file
                            using (BinaryWriter streamWriter = new BinaryWriter(File.Open(indexPath, FileMode.Create)))
                            {
                                WriteArcFileHeader(streamWriter, 50 + (4 * recIndex), shapeType, shapeBounds);
                                WriteArcFileRecord(streamWriter, 50, recLength);
                            }
                            // Write to dBase file
                            WriteArcDBaseFile(dBasePath, dBaseFile, sample);
                            num++;
                            break;
                        default:
                            continue;
                    }

                }
            }
            catch (Exception ex)
            {
                ProgressBarClose();
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }
            finally
            {
                // Close Progressbar
                ProgressBarClose();
            }

            return true;
        }

        /// <summary>
        /// Creates and writes the ArcView binary file header.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        /// <param name="fileLength">Length of the file.</param>
        /// <param name="shapeType">Type of the shape.</param>
        /// <param name="shapeBounds">The shape bounds.</param>
        private void WriteArcFileHeader(BinaryWriter streamWriter, uint fileLength, uint shapeType, double[] shapeBounds)
        {
            uint iZero = 0;
            double dZero = 0;
            streamWriter.Write(ConvertEndian(ArcFileCode));
            streamWriter.Write(ConvertEndian(iZero));
            streamWriter.Write(ConvertEndian(iZero));
            streamWriter.Write(ConvertEndian(iZero));
            streamWriter.Write(ConvertEndian(iZero));
            streamWriter.Write(ConvertEndian(iZero));
            streamWriter.Write(ConvertEndian(fileLength));
            streamWriter.Write(ArcVersion);
            streamWriter.Write(shapeType);
            streamWriter.Write(shapeBounds[0]);
            streamWriter.Write(shapeBounds[1]);
            streamWriter.Write(shapeBounds[2]);
            streamWriter.Write(shapeBounds[3]);
            streamWriter.Write(dZero);
            streamWriter.Write(dZero);
            streamWriter.Write(dZero);
            streamWriter.Write(dZero);
        }

        /// <summary>
        /// Writes an ArcView binary file record containing a single point.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        /// <param name="recIndex">Index of the record.</param>
        /// <param name="recLength">Length of the record.</param>
        /// <param name="shapeType">Type of the shape.</param>
        /// <param name="pnt">The point coordinates.</param>
        private void WriteArcFileRecord(BinaryWriter streamWriter, uint recIndex, uint recLength, uint shapeType, Point pnt)
        {
            Point wPnt = CanvasToWorld(pnt);
            streamWriter.Write(ConvertEndian(recIndex));
            streamWriter.Write(ConvertEndian(recLength));
            streamWriter.Write(shapeType);
            streamWriter.Write(wPnt.X);
            streamWriter.Write(wPnt.Y);
            m_ProgressCount++;
        }

        /// <summary>
        /// Writes an ArcView binary file record containing a point collection.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        /// <param name="recIndex">Index of the record.</param>
        /// <param name="recLength">Length of the record.</param>
        /// <param name="shapeType">Type of the shape.</param>
        /// <param name="points">The total number of points in the record.</param>
        /// <param name="shapeBounds">The shape bounds.</param>
        /// <param name="pointCollection">The point collection.</param>
        private void WriteArcFileRecord(BinaryWriter streamWriter, uint recIndex, uint recLength, uint shapeType,
                                        uint points, double[] shapeBounds, PointCollection pointCollection)
        {
            streamWriter.Write(ConvertEndian(recIndex));
            streamWriter.Write(ConvertEndian(recLength));
            streamWriter.Write(shapeType);
            streamWriter.Write(shapeBounds[0]);
            streamWriter.Write(shapeBounds[1]);
            streamWriter.Write(shapeBounds[2]);
            streamWriter.Write(shapeBounds[3]);
            streamWriter.Write(points);
            foreach (Point pnt in pointCollection)
            {
                Point wPnt = CanvasToWorld(pnt);
                streamWriter.Write(wPnt.X);
                streamWriter.Write(wPnt.Y);
                m_ProgressCount++;
            }
        }

        /// <summary>
        /// Writes an ArcView binary file record containing a PolyLine or a Polygon.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        /// <param name="recIndex">Index of the record.</param>
        /// <param name="recLength">Length of the record.</param>
        /// <param name="shapeType">Type of the shape.</param>
        /// <param name="points">The total number of points in the record.</param>
        /// <param name="shapeBounds">The shape bounds.</param>
        /// <param name="pointCollectionList">The point collection list.</param>
        private void WriteArcFileRecord(BinaryWriter streamWriter, uint recIndex, uint recLength, uint shapeType,
                                        uint points, double[] shapeBounds, List<PointCollection> pointCollectionList)
        {
            streamWriter.Write(ConvertEndian(recIndex));
            streamWriter.Write(ConvertEndian(recLength));
            streamWriter.Write(shapeType);
            streamWriter.Write(shapeBounds[0]);
            streamWriter.Write(shapeBounds[1]);
            streamWriter.Write(shapeBounds[2]);
            streamWriter.Write(shapeBounds[3]);
            streamWriter.Write((uint)pointCollectionList.Count);
            streamWriter.Write(points);
            uint index = 0;
            foreach (PointCollection pntCol in pointCollectionList)
            {
                streamWriter.Write(index);
                index += (uint)pntCol.Count;
            }
            foreach (PointCollection pntCol in pointCollectionList)
            {
                foreach (Point pnt in pntCol)
                {
                    Point wPnt = CanvasToWorld(pnt);
                    streamWriter.Write(wPnt.X);
                    streamWriter.Write(wPnt.Y);
                    m_ProgressCount++;
                }
            }
        }

        /// <summary>
        /// Writes an ArcView Index file record.
        /// </summary>
        /// <param name="streamWriter">The stream writer.</param>
        /// <param name="recIndex">Index of the record.</param>
        /// <param name="recLength">Length of the record.</param>
        private void WriteArcFileRecord(BinaryWriter streamWriter, uint recIndex, uint recLength)
        {
            streamWriter.Write(ConvertEndian(recIndex));
            streamWriter.Write(ConvertEndian(recLength));

            ProgressBarUpdate(m_ProgressCount);
        }


        /// <summary>
        /// Convert little-endian int to big-endian uint value.
        /// </summary>
        /// <param name="input">The input value.</param>
        /// <returns>The result of the conversion.</returns>
        private uint ConvertEndian(uint input)
        {
            uint output =
                (input >> 24) |
                (((input >> 16) & 0x000000ff) << 8) |
                (((input >> 8) & 0x000000ff) << 16) |
                (((input) & 0x000000ff) << 24);
            return output;
        }

        /// <summary>
        /// Create an ArcView dBase File containing the shapes ID and description.
        /// </summary>
        /// <param name="path">The destination path.</param>
        /// <param name="file">The dBase file name.</param>
        /// <param name="sample">The sample holding its ID and description.</param>
        private void WriteArcDBaseFile(string path, string file, Sample sample)
        {
            string sourcePath = string.Empty;
            try
            {
                // Get executing directory containing the dBase template file
                string execDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6);
                // Create source path for template file
                sourcePath = execDir + "\\ArcTemp.dbf";
                // Check if dBase file should be created (OLEDB adapter installation needed)
                if (m_CreateDbaseFile)
                {
                    // Delete template file in destination directory
                    // File.Delete(path + "\\ArcTemp.dbf");
                    // Copy template dbf file to destination directory
                    File.Copy(sourcePath, path + "\\ArcTemp1.dbf", true);
                    // Note: There are restrictions regarding the database (file) name (e.g. no "-" chars allowed), 
                    // thus we must ensure that the name is valid and we are editing the "ArcTemp.dbf" file first,
                    // before we rename it to the user selected shape file name (e.g. "Somefile-01.dbf").

                    // Create data adapter
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                    // Create connection string
                    string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=DBASE III;";
                    // string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=DBASE III;";
                    // "Driver={Microsoft dBASE Driver (*.dbf)};datasource=E:\\";    
                    // Create connection
                    OleDbConnection connection = new OleDbConnection(connectionString);
                    // Create select command
                    dataAdapter.SelectCommand = new OleDbCommand("SELECT * FROM ArcTemp1.dbf", connection);
                    OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);
                    // Create data table from empty database file
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    connection.Close();

                    // Add row with sample info
                    DataRow newrow = dataTable.NewRow();
                    newrow[0] = sample.Identifier;
                    newrow[1] = sample.DisplayText;
                    dataTable.Rows.Add(newrow);

                    //Update dBase File
                    dataAdapter.Update(dataTable);

                    File.Copy(path + "\\ArcTemp1.dbf", path + "\\" + file, true);
                    File.Delete(path + "\\ArcTemp1.dbf");
                }
                else
                {
                    // No OLEDB adapter installed. Just copy empty default dBase file.
                    File.Copy(sourcePath, path + "\\" + file, true);
                    File.Delete(path + "\\ArcTemp1.dbf");
                }

                /* Show rows in box
                String showRow = String.Empty;
                int indx = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    showRow += "Row[" + (indx++).ToString() + "]:  ";
                    foreach (object item in row.ItemArray)
                    {
                        showRow += item.ToString() + "  ";
                    }
                    showRow += StrCR;
                }
                MessageBox.Show(showRow);
                */

                /*
                string connectionString = "Driver={Microsoft dBASE Driver (*.dbf)};datasource=E:\\"; 
                                          //  "Driver={Microsoft Access Driver (*.mdb)};DBQ=C:\\Samples\\Northwind.mdb";
                using (OdbcConnection connection = new OdbcConnection(connectionString))
                {
                    connection.Open();
                    OdbcCommand command = new OdbcCommand("select * from Test");
                    command.Connection = connection;
                    num = command.ExecuteNonQuery();
                    if (num == 0)
                        num = 1;
                }
                OdbcCommand command = new OdbcCommand("select * from E:/Test.dbf");
                String strConn = "Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277;Dbq=E:\\Test;";
                // String strConn = "Driver={Microsoft dBase Driver (*.dbf)};datasource=dBase Files;";
                //  "Provider=MSDASQL; Driver={Microsoft dBase Driver (*.*)}; DBQ=E:\\Test";
                OdbcConnection OdbcConn = new OdbcConnection(strConn);
                command.Connection = OdbcConn;
                OdbcConn.Open();
                num = command.ExecuteNonQuery();
                OdbcConn.Close();
                */
            }
            catch (Exception ex)
            {
                File.Copy(sourcePath, path + "\\" + file, true);
                SetError(WpfSamplingPlotPage.Properties.Resources.ErrNoOleDBAdapterInstalled + ex.Message);
                m_CreateDbaseFile = false;
            }
        }

        private bool ReadArcDBaseFile(string path, out List<string> descriptionList, out List<string> idList)
        {
            List<List<string>> rowList = new List<List<string>>();
            descriptionList = new List<string>();
            idList = new List<string>();

            string sourcePath = string.Empty;
            try
            {

                // Extract File name from path
                int index1 = path.LastIndexOf("\\") + 1;
                int index2 = path.LastIndexOf(".");
                string name = path.Substring(index1, path.Length - index1);
                string dBasePath = path.Substring(0, index1 - 1);
                if (index2 > index1)
                {
                    name = path.Substring(index1, index2 - index1);
                }
                else
                {
                    name = path.Substring(index1, path.Length - index1);
                }
                // Add .dbf extension to file name
                string dBaseFile = String.Format("{0}\\{1}{2}", dBasePath, name, StrArcGisDBaseFileExtension);

                // Create data adapter
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                // Create connection string
                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dBasePath + ";Extended Properties=\"dBASE IV;CharacterSet=Windows-1252;\"";
                // "Driver={Microsoft dBASE Driver (*.dbf)};datasource=E:\\";    
                // Create connection
                OleDbConnection connection = new OleDbConnection(connectionString);

                // Create select command
                // dataAdapter.SelectCommand = new OleDbCommand("SELECT * FROM " + dBaseFile, connection);
                dataAdapter.SelectCommand = new OleDbCommand("SELECT * FROM  " + dBaseFile, connection);
                OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);
                // Create data table from empty database file
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                connection.Close();

                // Show rows in box
                String showRow = String.Empty;

                int indx = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    List<string> itemList = new List<string>();

                    showRow += "Row[" + indx.ToString() + "]:  ";
                    foreach (object item in row.ItemArray)
                    {
                        string tmp = item.ToString() + "  ";
                        showRow += tmp;
                        itemList.Add(item.ToString());
                    }
                    showRow += StrCR;

                    rowList.Add(itemList);
                    indx++;
                }
                /*
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf16 = Encoding.GetEncoding("UTF-16");
                byte[] utfBytes = utf16.GetBytes(showRow);
                byte[] isoBytes = Encoding.Convert(utf16, iso, utfBytes);
                string msg = iso.GetString(isoBytes);

                Encoding utf8 = Encoding.UTF8;
                byte[] utfBytes = utf8.GetBytes(showRow);
                string isocontent = Encoding.GetEncoding("Windows-1252").GetString(utfBytes, 0, utfBytes.Length);
                byte[] isobytes = Encoding.GetEncoding("Windows-1252").GetBytes(isocontent);
                byte[] ubytes = Encoding.Convert(Encoding.GetEncoding("Windows-1252"), Encoding.Unicode, isobytes);
                string msg = Encoding.Unicode.GetString(ubytes, 0, ubytes.Length);
                */

                if (rowList.Count >= 1)
                {
                    // Create Settings window
                    SelectAttribute m_SelectAttribute = new SelectAttribute(rowList);
                    // Show Dialog
                    m_SelectAttribute.ShowDialog();

                    descriptionList = m_SelectAttribute.DescriptionList;
                    idList = m_SelectAttribute.IdList;
                }
                // MessageBox.Show(showRow);

            }
            catch // (Exception ex)
            {
                // SetError(WpfSamplingPlotPage.Properties.Resources.ErrNoOleDBAdapterForRead + ex.Message);
                MessageBoxResult result = MessageBox.Show((WpfSamplingPlotPage.Properties.Resources.ErrNoOleDBAdapterForRead).Replace("\\r\\n", "\r\n"), WpfSamplingPlotPage.Properties.Resources.ErrCaption, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                    return false;
                else
                    return true;
            }
            return true;
        }

        #endregion // Write Samples to AcrView shape file

        #region Write Samples to SQL shape file

        private int NumberOfPointsInSampleList()
        {
            int count = 0;

            // Pre-calculate number of total points
            foreach (Sample sample in m_SampleList)
            {
                if (sample.IsSampleVisible && sample.TypeOfSample != SampleType.IMAGE)
                {
                    foreach (PointCollection col in sample.SamplePointCollectionList)
                    {
                        count += col.Count;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Save all visible shapes of the sample list as individual Geoobjects to an ASCII file (in SQL notation).
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool WriteShapesToSqlFile(string path)
        {
            bool retVal = true;

            try
            {
                GeoObject geoObject = new GeoObject();
                string shapeString = string.Empty;

                int count = NumberOfPointsInSampleList();

                /* Show warning if big number of points
                if (count > 100000)
                {
                    if (SetRequest(string.Format(WpfSamplingPlotPage.Properties.Resources.ErrTooManyPoints, count.ToString())) == false)
                        return false;
                }                
                */

                // Init Progressbar with maxCount
                ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressSavingShape, count);

                foreach (Sample sample in m_SampleList)
                {
                    if (!sample.IsSampleVisible)
                        continue;

                    if (sample.TypeOfSample != SampleType.IMAGE)
                    {
                        // Create Shape-String for file output
                        if (!SetupGeoObject(out geoObject, sample))
                        {
                            retVal = false;
                            continue;
                        }

                        shapeString += geoObject.Identifier + StrCR
                                     + geoObject.DisplayText + StrCR
                                     + SetNameOfBrush((SolidColorBrush)geoObject.StrokeBrush) + StrCR
                                     + SetNameOfBrush((SolidColorBrush)geoObject.FillBrush) + StrCR
                                     + geoObject.StrokeTransparency.ToString() + StrCR
                                     + geoObject.FillTransparency.ToString() + StrCR
                                     + geoObject.StrokeThickness.ToString("F2", CultureInfo.InvariantCulture) + StrCR
                                     + geoObject.PointType.ToString() + StrCR
                                     + geoObject.PointSymbolSize.ToString("F2", CultureInfo.InvariantCulture) + StrCR
                                     + geoObject.GeometryData + StrCR;
                    }
                }
                // Write to file, if any shape available
                if (shapeString != string.Empty)
                {
                    string newPath = String.Format("{0}{1}", path, StrShapeFileExtension);
                    using (TextWriter streamWriter = new StreamWriter(newPath))
                    {
                        streamWriter.WriteLine(shapeString);
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                retVal = false;
            }
            finally
            {
                // Close Progressbar
                ProgressBarClose();
            }

            return retVal;
        }
        /// <summary>
        /// Save all visible shapes of the sample list as individual Geoobjects to an ASCII file (in SQL notation).
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool WriteShapesToSeparateSqlFiles(string path)
        {
            bool retVal = true;

            try
            {
                GeoObject geoObject = new GeoObject();
                string shapeString = string.Empty;

                int count = NumberOfPointsInSampleList();

                /* Show warning if big number of points
                if (count > 100000)
                {
                    if (SetRequest(string.Format(WpfSamplingPlotPage.Properties.Resources.ErrTooManyPoints, count.ToString())) == false)
                        return false;
                }                
                */

                // Init Progressbar with maxCount
                ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressSavingShape, count);

                foreach (Sample sample in m_SampleList)
                {
                    if (!sample.IsSampleVisible)
                        continue;

                    if (sample.TypeOfSample != SampleType.IMAGE)
                    {
                        // Create Shape-String for file output
                        if (!SetupGeoObject(out geoObject, sample))
                        {
                            retVal = false;
                            continue;
                        }

                        shapeString += geoObject.Identifier + StrCR
                                     + geoObject.DisplayText + StrCR
                                     + SetNameOfBrush((SolidColorBrush)geoObject.StrokeBrush) + StrCR
                                     + SetNameOfBrush((SolidColorBrush)geoObject.FillBrush) + StrCR
                                     + geoObject.StrokeTransparency.ToString() + StrCR
                                     + geoObject.FillTransparency.ToString() + StrCR
                                     + geoObject.StrokeThickness.ToString("F2", CultureInfo.InvariantCulture) + StrCR
                                     + geoObject.PointType.ToString() + StrCR
                                     + geoObject.PointSymbolSize.ToString("F2", CultureInfo.InvariantCulture) + StrCR
                                     + geoObject.GeometryData;


                        int index = path.LastIndexOf("\\");

                        string newPath = String.Format("{0}\\{1}{2}", path.Substring(0, index), geoObject.Identifier.Replace("/", "-"), StrShapeFileExtension);
                        using (TextWriter streamWriter = new StreamWriter(newPath))
                        {
                            streamWriter.WriteLine(shapeString);
                        }
                        shapeString = string.Empty;

                    }

                }

            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                retVal = false;
            }
            finally
            {
                // Close Progressbar
                ProgressBarClose();
            }

            return retVal;
        }

        /// <summary>
        /// Save all visible shapes of the sample list as one GEOMETRYCOLLECTION to an ASCII file (in SQL notation).
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool WriteShapeCollectionToSqlFile(string path)
        {
            try
            {
                GeoObject geoObject = new GeoObject();
                if (!SetupGeoObject(out geoObject, m_SampleList))
                    return false;

                string shapeString = geoObject.Identifier + StrCR
                                   + geoObject.DisplayText + StrCR
                                   + SetNameOfBrush((SolidColorBrush)geoObject.StrokeBrush) + StrCR
                                   + SetNameOfBrush((SolidColorBrush)geoObject.FillBrush) + StrCR
                                   + geoObject.StrokeTransparency.ToString() + StrCR
                                   + geoObject.FillTransparency.ToString() + StrCR
                                   + geoObject.StrokeThickness.ToString("F2", CultureInfo.InvariantCulture) + StrCR
                                   + geoObject.PointType.ToString() + StrCR
                                   + geoObject.PointSymbolSize.ToString("F2", CultureInfo.InvariantCulture) + StrCR
                                   + geoObject.GeometryData + StrCR;

                // Write to file
                string newPath = String.Format("{0}_{1}", path, StrShapeFileExtension);
                using (TextWriter streamWriter = new StreamWriter(newPath))
                {
                    streamWriter.WriteLine(shapeString);
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }

            return true;
        }

        #endregion // Write Samples to SQL shape file

        #region Write Shapes to TAB separated Import file

        /// <summary>
        /// Save all visible Geoobjects of the sample list in TAB separated text file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool WriteShapesToImportFile(string path)
        {
            bool retVal = true;

            try
            {
                GeoObject geoObject = new GeoObject();
                string shapeString = string.Empty;

                EnableEnvelopeCenter(true);
                ClearEnvelopeCenter();


                // SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrProgressSavingShape, true);

                int count = NumberOfPointsInSampleList();

                /*
                if (count > 100000)
                {
                    if (SetRequest(string.Format(WpfSamplingPlotPage.Properties.Resources.ErrTooManyPoints, count.ToString())) == false)
                        return false;
                }
                */

                // Init Progressbar with maxCount
                ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressSavingShape, count);

                foreach (Sample sample in m_SampleList)
                {
                    if (!sample.IsSampleVisible)
                        continue;

                    if (sample.TypeOfSample != SampleType.IMAGE)
                    {
                        if (!SetupGeoObject(out geoObject, sample))
                        {
                            retVal = false;
                            continue;
                        }

                        shapeString += geoObject.Identifier + StrTAB
                                     + geoObject.DisplayText + StrTAB
                                     + SetNameOfBrush((SolidColorBrush)geoObject.StrokeBrush) + StrTAB
                                     + SetNameOfBrush((SolidColorBrush)geoObject.FillBrush) + StrTAB
                                     + geoObject.StrokeTransparency.ToString() + StrTAB
                                     + geoObject.FillTransparency.ToString() + StrTAB
                                     + geoObject.StrokeThickness.ToString("F2", CultureInfo.InvariantCulture) + StrTAB
                                     + geoObject.PointType.ToString() + StrTAB
                                     + geoObject.PointSymbolSize.ToString("F2", CultureInfo.InvariantCulture) + StrTAB
                                     + geoObject.GeometryData;

                        // Add the first longitude and latitude parameters to table
                        // shapeString += ExtractLonLatFromGeography(geoObject.GeometryData);
                        // Add envelope center as longitude and latitude parameters to table
                        shapeString += StrTAB + m_EnvelopeCenterX.ToString("F10", CultureInfo.InvariantCulture) + StrTAB + m_EnvelopeCenterY.ToString("F10", CultureInfo.InvariantCulture);
                        ClearEnvelopeCenter();
                        shapeString += StrCR;
                    }
                }

                // Write to file, if any shape available
                if (shapeString != string.Empty)
                {
                    int index = shapeString.LastIndexOf(StrCR);
                    string cutShapeStr = shapeString.Substring(0, index);
                    string newPath = String.Format("{0}{1}", path, StrShape2FileExtension);
                    using (TextWriter streamWriter = new StreamWriter(newPath))
                    {
                        streamWriter.WriteLine(cutShapeStr);
                    }
                }

                // SetInstructionLabel(path + " has been saved.", false);

            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                retVal = false;
            }
            finally
            {
                EnableEnvelopeCenter(false);
                // Close Progressbar
                ProgressBarClose();
            }

            return retVal;
        }

        #region Save each shape to a separate file

        /// <summary>
        /// Save all visible Geoobjects of the sample list in TAB separated text file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool WriteShapesToSeparateImportFiles(string path)
        {
            bool retVal = true;
            EnableEnvelopeCenter(true);
            ClearEnvelopeCenter();

            try
            {
                GeoObject geoObject = new GeoObject();
                string shapeString = string.Empty;

                // SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrProgressSavingShape, true);

                int count = NumberOfPointsInSampleList();

                /*
                if (count > 100000)
                {
                    if (SetRequest(string.Format(WpfSamplingPlotPage.Properties.Resources.ErrTooManyPoints, count.ToString())) == false)
                        return false;
                }
                */

                // Init Progressbar with maxCount
                ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressSavingShape, count);

                foreach (Sample sample in m_SampleList)
                {
                    if (!sample.IsSampleVisible)
                        continue;

                    if (sample.TypeOfSample != SampleType.IMAGE)
                    {
                        if (!SetupGeoObject(out geoObject, sample))
                        {
                            retVal = false;
                            continue;
                        }

                        shapeString += geoObject.Identifier + StrTAB
                                     + geoObject.DisplayText + StrTAB
                                     + SetNameOfBrush((SolidColorBrush)geoObject.StrokeBrush) + StrTAB
                                     + SetNameOfBrush((SolidColorBrush)geoObject.FillBrush) + StrTAB
                                     + geoObject.StrokeTransparency.ToString() + StrTAB
                                     + geoObject.FillTransparency.ToString() + StrTAB
                                     + geoObject.StrokeThickness.ToString("F2", CultureInfo.InvariantCulture) + StrTAB
                                     + geoObject.PointType.ToString() + StrTAB
                                     + geoObject.PointSymbolSize.ToString("F2", CultureInfo.InvariantCulture) + StrTAB
                                     + geoObject.GeometryData;

                        // Add the first longitude and latitude parameters to table
                        // shapeString += ExtractLonLatFromGeography(geoObject.GeometryData);
                        // Add envelope center as longitude and latitude parameters to table
                        shapeString += StrTAB + m_EnvelopeCenterX.ToString("F10", CultureInfo.InvariantCulture) + StrTAB + m_EnvelopeCenterY.ToString("F10", CultureInfo.InvariantCulture);
                        ClearEnvelopeCenter();

                        int index = path.LastIndexOf("\\");

                        string newPath = String.Format("{0}\\{1}{2}", path.Substring(0, index), geoObject.Identifier.Replace("/", "-"), StrShape2FileExtension);
                        using (TextWriter streamWriter = new StreamWriter(newPath))
                        {
                            streamWriter.WriteLine(shapeString);
                        }
                        shapeString = string.Empty;

                        /*--------------- write image coordinate file -----------------

                        string retStr = string.Empty;
                        string[] splitStr = geoObject.GeometryData.Split(m_Separator1, StringSplitOptions.RemoveEmptyEntries);

                        PointCollection points = new PointCollection();
                        if (splitStr.Length > 8)
                        {
                            points.Add(new Point(Convert.ToDouble(splitStr[5], CultureInfo.InvariantCulture), Convert.ToDouble(splitStr[6], CultureInfo.InvariantCulture)));
                            points.Add(new Point(Convert.ToDouble(splitStr[1], CultureInfo.InvariantCulture), Convert.ToDouble(splitStr[2], CultureInfo.InvariantCulture)));
                            points.Add(new Point(Convert.ToDouble(splitStr[7], CultureInfo.InvariantCulture), Convert.ToDouble(splitStr[8], CultureInfo.InvariantCulture)));
                            points.Add(new Point(Convert.ToDouble(splitStr[3], CultureInfo.InvariantCulture), Convert.ToDouble(splitStr[4], CultureInfo.InvariantCulture)));
                        }
                        WriteImageAttributesToXmlFile(newPath, geoObject.DisplayText, points);
                        */
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                retVal = false;
            }
            finally
            {
                EnableEnvelopeCenter(false);
                // Close Progressbar
                ProgressBarClose();
            }

            return retVal;
        }

        #endregion

        char[] m_Separator1 = { '(', ')', ' ', ',' };

        private string ExtractLonLatFromGeography(string geometryData)
        {
            string retStr = string.Empty;

            string[] splitStr = geometryData.Split(m_Separator1, StringSplitOptions.RemoveEmptyEntries);

            if (splitStr.Length > 2)
            {
                retStr = StrTAB + splitStr[1] + StrTAB + splitStr[2];
            }

            /*
            int start = geometryData.IndexOf('(');
            if (geometryData[start + 1] == '(')
                start++;
            if (geometryData[start + 1] == '(')
                start++;


            int end1 = geometryData.IndexOf(')', start);
            int end2 = geometryData.IndexOf(',', start);
            int end = -1;
            if (end2 > start)
                end = end2;
            if (end1 > start && end1 < end2)
                end = end1;
            if (start < end)
                retStr = StrTAB + geometryData.Substring(start, end - 1);
            */

            return retStr;
        }

        #endregion // Write Shapes to TAB separated Import file

        #region Write image coordinates to XML file

        // Bayreuth Image XML scheme
        private const string XmlImageHeader = "<?xml version=\"1.0\"?>";
        private const string XmlImageComment = "<!--Map-Image Options-->";
        private const string XmlImageOptions = "<ImageOptions>";
        private const string XmlImageOptionsX = "</ImageOptions>";
        private const string XmlImageName = "  <Name>{0}</Name>";
        private const string XmlImageDescription = "  <Description>{0}</Description>";
        private const string XmlImageNWLat = "  <NWLat>{0}</NWLat>";
        private const string XmlImageNWLong = "  <NWLong>{0}</NWLong>";
        private const string XmlImageSELat = "  <SELat>{0}</SELat> ";
        private const string XmlImageSELong = "  <SELong>{0}</SELong>";
        private const string XmlImageSWLat = "  <SWLat>{0}</SWLat> ";
        private const string XmlImageSWLong = "  <SWLong>{0}</SWLong>";
        private const string XmlImageNELat = "  <NELat>{0}</NELat>";
        private const string XmlImageNELong = "  <NELong>{0}</NELong>";
        private const string XmlImageZoomLevel = "  <ZoomLevel />";
        private const string XmlImageTransparency = "  <Transparency>{0}</Transparency>";

        /// <summary>
        /// Saves the attributes and coordinates of a sample image to an XML file.
        /// </summary>
        /// <param name="sampleImage">The sample image.</param>
        private void WriteImageAttributesToXmlFile(SampleImage sampleImage)
        {
            try
            {
                if (sampleImage != null)
                {
                    // File path
                    string path = sampleImage.ImageFilePath;
                    if (path == string.Empty)
                        return;

                    string name = string.Empty;
                    // Get corner points
                    PointCollection points = CanvasToWorld(sampleImage.SamplePointCollection);
                    if (points.Count != 4)
                        return;

                    // Extract File name from path
                    int index1 = path.LastIndexOf("\\") + 1;
                    int index2 = path.LastIndexOf(".");
                    name = path.Substring(index1, path.Length - index1);
                    if (index2 > index1)
                        name = path.Substring(index1, index2 - index1);
                    else
                        name = path.Substring(index1, path.Length - index1);

                    // Remove file name extension if any
                    if (index2 > 0)
                        path = path.Substring(0, index2);
                    // Add .xml extension to file name
                    path += StrImageXmlFileExtension;

                    // Create XML file content
                    string imageString = XmlImageHeader + StrCR + XmlImageComment + StrCR + XmlImageOptions + StrCR;
                    imageString += string.Format(XmlImageName, name) + StrCR;

                    if (m_Testcase)
                        imageString += string.Format(XmlImageDescription, m_TestcaseDescription) + StrCR;
                    else
                        imageString += string.Format(XmlImageDescription, sampleImage.DisplayText) + StrCR;

                    imageString += string.Format(XmlImageNWLat, points[0].Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(XmlImageNWLong, points[0].X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(XmlImageSELat, points[1].Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(XmlImageSELong, points[1].X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(XmlImageSWLat, points[2].Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(XmlImageSWLong, points[2].X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(XmlImageNELat, points[3].Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(XmlImageNELong, points[3].X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += XmlImageZoomLevel + StrCR;
                    imageString += string.Format(XmlImageTransparency, sampleImage.Transparency.ToString()) + StrCR;
                    imageString += XmlImageOptionsX + StrCR;

                    // Write to file
                    using (TextWriter streamWriter = new StreamWriter(path))
                    {
                        streamWriter.WriteLine(imageString);
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /// <summary>
        /// Saves the attributes and coordinates of a sample image frame to an XML file with given coordinates.
        /// </summary>
        /// <param name="sampleImage">The sample image.</param>
        /// <param name="topLeft">The top left screen coordinate.</param>
        /// <param name="bottomRight">The bottom right screen coordinate.</param>
        private void WriteImageAttributesToXmlFile(SampleImage sampleImage, Point topLeft, Point bottomRight)
        {
            try
            {
                // File path
                string path = sampleImage.ImageFilePath;
                if (path == string.Empty)
                    return;

                string name = string.Empty;
                // Get corner points
                Point worldTopLeft = CanvasToWorld(topLeft);
                Point worldBottomRight = CanvasToWorld(bottomRight);

                // Extract File name from path
                int index1 = path.LastIndexOf("\\") + 1;
                int index2 = path.LastIndexOf(".");
                name = path.Substring(index1, path.Length - index1);
                if (index2 > index1)
                    name = path.Substring(index1, index2 - index1);
                else
                    name = path.Substring(index1, path.Length - index1);

                // Remove file name extension if any
                if (index2 > 0)
                    path = path.Substring(0, index2);
                // Add .xml extension to file name
                path += StrImageXmlFileExtension;

                // Create XML file content
                string imageString = XmlImageHeader + StrCR + XmlImageComment + StrCR + XmlImageOptions + StrCR;
                imageString += string.Format(XmlImageName, name) + StrCR;

                if (m_Testcase)
                    imageString += string.Format(XmlImageDescription, m_TestcaseDescription) + StrCR;
                else
                    imageString += string.Format(XmlImageDescription, sampleImage.DisplayText) + StrCR;

                imageString += string.Format(XmlImageNWLat, worldTopLeft.Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageNWLong, worldTopLeft.X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageSELat, worldBottomRight.Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageSELong, worldBottomRight.X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageSWLat, worldBottomRight.Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageSWLong, worldTopLeft.X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageNELat, worldTopLeft.Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageNELong, worldBottomRight.X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += XmlImageZoomLevel + StrCR;
                imageString += string.Format(XmlImageTransparency, sampleImage.Transparency.ToString()) + StrCR;
                imageString += XmlImageOptionsX + StrCR;


                // Write to file
                using (TextWriter streamWriter = new StreamWriter(path))
                {
                    streamWriter.WriteLine(imageString);
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /*
        /// <summary>
        /// Saves the attributes and coordinates of a sample image to an XML file.
        /// </summary>
        /// <param name="sampleImage">The sample image.</param>
        private void WriteImageAttributesToXmlFile(string path, string description, PointCollection points)
        {
            try
            {

                if (path == string.Empty)
                    return;

                string name = string.Empty;

                if (points.Count != 4)
                    return;

                // Extract File name from path
                int index1 = path.LastIndexOf("\\") + 1;
                int index2 = path.LastIndexOf(".");
                name = path.Substring(index1, path.Length - index1);
                if (index2 > index1)
                    name = path.Substring(index1, index2 - index1);
                else
                    name = path.Substring(index1, path.Length - index1);

                // Remove file name extension if any
                if (index2 > 0)
                    path = path.Substring(0, index2);
                // Add .xml extension to file name
                path += StrImageXmlFileExtension;

                // Create XML file content
                string imageString = XmlImageHeader + StrCR + XmlImageComment + StrCR + XmlImageOptions + StrCR;
                imageString += string.Format(XmlImageName, name) + StrCR;
                imageString += string.Format(XmlImageDescription, description) + StrCR;
                imageString += string.Format(XmlImageNWLat, points[0].Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageNWLong, points[0].X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageSELat, points[1].Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageSELong, points[1].X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageSWLat, points[2].Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageSWLong, points[2].X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageNELat, points[3].Y.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(XmlImageNELong, points[3].X.ToString("F8", CultureInfo.InvariantCulture)) + StrCR;
                imageString += XmlImageZoomLevel + StrCR;
                imageString += string.Format(XmlImageTransparency, "255") + StrCR;
                imageString += XmlImageOptionsX + StrCR;

                // Write to file
                using (TextWriter streamWriter = new StreamWriter(path))
                {
                    streamWriter.WriteLine(imageString);
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }
        */

        #endregion // Write image coordinates to file

        #region Write image coordinates to KAL file

        // Glopus calibration file format
        // private const string KalZeile01 = "[Kalibrierung]";
        // private const string KalZeile02 = "ScaleY = {0}";
        // private const string KalZeile03 = "ScaleX = {0}";
        // private const string KalZeile04 = "OffLaenge = {0}";
        // private const string KalZeile05 = "OffBreite = {0}";
        private const string KalZeile06 = "[Calibration Point 1]";
        private const string KalZeile07 = "Longitude = {0}";
        private const string KalZeile08 = "Latitude = {0}";
        private const string KalZeile09 = "Pixel = POINT(0,0)";
        private const string KalZeile10 = "[Calibration Point 2]";
        private const string KalZeile11 = "Longitude = {0}";
        private const string KalZeile12 = "Latitude = {0}";
        private const string KalZeile13 = "Pixel = POINT({0},0)";
        private const string KalZeile14 = "[Calibration Point 3]";
        private const string KalZeile15 = "Longitude = {0}";
        private const string KalZeile16 = "Latitude = {0}";
        private const string KalZeile17 = "Pixel = POINT({0},{1})";
        private const string KalZeile18 = "[Calibration Point 4]";
        private const string KalZeile19 = "Longitude = {0}";
        private const string KalZeile20 = "Latitude = {0}";
        private const string KalZeile21 = "Pixel = POINT(0,{0})";
        private const string KalZeile22 = "[Map]";
        private const string KalZeile23 = "Bitmap = {0}";
        private const string KalZeile24 = "Size = SIZE({0},{1})";

        // Write shape collection to Glopus-formatted text file
        private void WriteImageAttributesToKalFile(SampleImage sampleImage)
        {
            try
            {
                if (sampleImage != null)
                {
                    string path = sampleImage.ImageFilePath;
                    if (path == string.Empty)
                        return;

                    int width = (int)sampleImage.m_Image.Source.Width;
                    int height = (int)sampleImage.m_Image.Source.Height;

                    string name = string.Empty;
                    PointCollection points = CanvasToWorld(sampleImage.SamplePointCollection);
                    if (points.Count != 4)
                        return;

                    // Extract File name from path
                    int index1 = path.LastIndexOf("\\") + 1;
                    int index2 = path.LastIndexOf(".");
                    string imageName = path.Substring(index1, path.Length - index1);
                    if (index2 > index1)
                        name = path.Substring(index1, index2 - index1);
                    else
                        name = path.Substring(index1, path.Length - index1);

                    // Remove file name extension if any
                    if (index2 > 0)
                        path = path.Substring(0, index2);
                    // Add .kal extension to file name
                    path += StrImageKalFileExtension;

                    string imageString = KalZeile06 + StrCR;
                    imageString += string.Format(KalZeile07, points[0].X.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(KalZeile08, points[0].Y.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += KalZeile09 + StrCR;
                    imageString += KalZeile10 + StrCR;
                    imageString += string.Format(KalZeile11, points[3].X.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(KalZeile12, points[3].Y.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(KalZeile13, width) + StrCR;
                    imageString += KalZeile14 + StrCR;
                    imageString += string.Format(KalZeile15, points[1].X.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(KalZeile16, points[1].Y.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(KalZeile17, width, height) + StrCR;
                    imageString += KalZeile18 + StrCR;
                    imageString += string.Format(KalZeile19, points[2].X.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(KalZeile20, points[2].Y.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                    imageString += string.Format(KalZeile21, height) + StrCR;
                    imageString += KalZeile22 + StrCR;
                    imageString += string.Format(KalZeile23, imageName) + StrCR;
                    imageString += string.Format(KalZeile24, width, height) + StrCR;

                    // Write to file
                    using (TextWriter streamWriter = new StreamWriter(path))
                    {
                        streamWriter.WriteLine(imageString);
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        // Write shape collection to Glopus-formatted text file with given coordinates
        private void WriteImageAttributesToKalFile(SampleImage sampleImage, Point topLeft, Point bottomRight, int width, int height)
        {
            try
            {
                string path = sampleImage.ImageFilePath;
                if (path == string.Empty)
                    return;

                string name = string.Empty;
                Point worldTopLeft = CanvasToWorld(topLeft);
                Point worldBottomRight = CanvasToWorld(bottomRight);

                // Extract File name from path
                int index1 = path.LastIndexOf("\\") + 1;
                int index2 = path.LastIndexOf(".");
                string imageName = path.Substring(index1, path.Length - index1);
                if (index2 > index1)
                    name = path.Substring(index1, index2 - index1);
                else
                    name = path.Substring(index1, path.Length - index1);

                // Remove file name extension if any
                if (index2 > 0)
                    path = path.Substring(0, index2);
                // Add .kal extension to file name
                path += StrImageKalFileExtension;

                string imageString = KalZeile06 + StrCR;
                imageString += string.Format(KalZeile07, worldTopLeft.X.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(KalZeile08, worldTopLeft.Y.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                imageString += KalZeile09 + StrCR;
                imageString += KalZeile10 + StrCR;
                imageString += string.Format(KalZeile11, worldBottomRight.X.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(KalZeile12, worldTopLeft.Y.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(KalZeile13, width) + StrCR;
                imageString += KalZeile14 + StrCR;
                imageString += string.Format(KalZeile15, worldBottomRight.X.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(KalZeile16, worldBottomRight.Y.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(KalZeile17, width, height) + StrCR;
                imageString += KalZeile18 + StrCR;
                imageString += string.Format(KalZeile19, worldTopLeft.X.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(KalZeile20, worldBottomRight.Y.ToString("F6", CultureInfo.InvariantCulture)) + StrCR;
                imageString += string.Format(KalZeile21, height) + StrCR;
                imageString += KalZeile22 + StrCR;
                imageString += string.Format(KalZeile23, imageName) + StrCR;
                imageString += string.Format(KalZeile24, width, height) + StrCR;

                // Write to file
                using (TextWriter streamWriter = new StreamWriter(path))
                {
                    streamWriter.WriteLine(imageString);
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        #endregion // Write image coordinates to KAL file

        #region Write samples to OGC XML file

        // OGC compatible(?) XML scheme
        private const string XmlMultiSurface = "<MultiSurface xmlns=\"http://www.opengis.net/gml\">";
        private const string XmlMultiSurfaceX = "</MultiSurface>";
        private const string XmlMembers = "<surfaceMembers>";
        private const string XmlMembersX = "</surfaceMembers>";
        private const string XmlPoint = "<Point{0}>";
        private const string XmlPointX = "</Point>";
        private const string XmlLineString = "<LineString{0}>";
        private const string XmlLineStringX = "</LineString>";
        private const string XmlPolygon = "<Polygon{0}>";
        private const string XmlPolygonX = "</Polygon>";
        private const string XmlLinearRing = "<LinearRing>";
        private const string XmlLinearRingX = "</LinearRing>";
        private const string Xmlpos = "<pos>";
        private const string XmlposX = "</pos>";
        private const string XmlposList = "<posList>";
        private const string XmlposListX = "</posList>";
        private const string Xmlexterior = "<exterior>";
        private const string XmlexteriorX = "</exterior>";
        // Attributes
        private const string XmlIdentifier = " Identifier=\"{0}\"";
        private const string XmlDescription = " Description=\"{0}\"";
        private const string XmlColor = " Color=\"{0}\"";
        private const string XmlTransparency = " Transparency=\"{0}\"";

        // Write shape collection to XML-formatted file
        private void WriteShapeCollectionToXmlFile(string path)
        {
            try
            {
                string shapeString = XmlMultiSurface + StrCR + XmlMembers + StrCR;

                // Save all Polygons and sample points, but no images!
                foreach (Sample sample in m_SampleList)
                {
                    if (sample != null)
                    {
                        string strSampleHeader = string.Empty;

                        // Set correct identifier string
                        if (sample.TypeOfSample == SampleType.POLYGON)
                        {
                            shapeString += string.Format(XmlPolygon,
                                (string.Format(XmlIdentifier, sample.Identifier) +
                                 string.Format(XmlDescription, sample.DisplayText) +
                                 string.Format(XmlColor, SetNameOfBrush(sample.m_StrokeBrush)) +
                                 string.Format(XmlTransparency, sample.SampleStrokeTransparency.ToString()))) + StrCR +
                                 Xmlexterior + StrCR + XmlLinearRing + StrCR + XmlposList + StrCR;

                            foreach (Point point in sample.SamplePointCollection)
                            {
                                Point pnt = CanvasToWorld(point);
                                shapeString += pnt.Y.ToString(CultureInfo.InvariantCulture) + " " + pnt.X.ToString(CultureInfo.InvariantCulture) + StrCR;
                            }
                            shapeString += XmlposListX + StrCR + XmlLinearRingX + StrCR + XmlexteriorX + StrCR + XmlPolygonX + StrCR;
                        }
                        else if (sample.TypeOfSample == SampleType.LINESTRING)
                        {
                            shapeString += string.Format(XmlLineString,
                                (string.Format(XmlIdentifier, sample.Identifier) +
                                 string.Format(XmlDescription, sample.DisplayText) +
                                 string.Format(XmlColor, SetNameOfBrush(sample.m_StrokeBrush)))) + StrCR +
                                 XmlposList + StrCR;

                            foreach (Point point in sample.SamplePointCollection)
                            {
                                Point pnt = CanvasToWorld(point);
                                shapeString += pnt.Y.ToString(CultureInfo.InvariantCulture) + " " + pnt.X.ToString(CultureInfo.InvariantCulture) + StrCR;
                            }
                            shapeString += XmlposListX + StrCR + XmlLineStringX + StrCR;
                        }
                        else if (sample.TypeOfSample == SampleType.POINT || sample.TypeOfSample == SampleType.MULTIPOINT)
                        {
                            foreach (Point point in sample.SamplePointCollection)
                            {
                                Point pnt = CanvasToWorld(point);
                                shapeString += string.Format(XmlPoint,
                                    (string.Format(XmlIdentifier, sample.Identifier) +
                                     string.Format(XmlDescription, sample.DisplayText) +
                                     string.Format(XmlColor, SetNameOfBrush(sample.m_StrokeBrush)))) + StrCR
                                     + Xmlpos + StrCR;
                                shapeString += pnt.Y.ToString(CultureInfo.InvariantCulture) + " " + pnt.X.ToString(CultureInfo.InvariantCulture) + StrCR;
                                shapeString += XmlposX + StrCR + XmlPointX + StrCR;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                shapeString += XmlMembersX + StrCR + XmlMultiSurfaceX + StrCR;
                // Write to file
                using (TextWriter streamWriter = new StreamWriter(path))
                {
                    streamWriter.WriteLine(shapeString);
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        #endregion // Write samples to OGC XML file

        #endregion // Load and save shapes in files

        #region GeoObjects

        /// <summary>
        /// Creates a sample and fills it with GeoObject data (-- not used so far --).
        /// </summary>
        /// <param name="geoObject">The input Geoobject.</param>
        /// <returns>The created Sample instance.</returns>
        private Sample SetupSample(GeoObject geoObject)
        {
            Sample sample = new Sample();
            sample.Identifier = geoObject.Identifier;
            sample.DisplayText = geoObject.DisplayText;
            sample.SampleStrokeBrush = geoObject.StrokeBrush;
            sample.SampleFillBrush = geoObject.FillBrush;
            sample.SampleStrokeTransparency = geoObject.StrokeTransparency;
            sample.SampleFillTransparency = geoObject.FillTransparency;
            sample.SampleStrokeThickness = geoObject.StrokeThickness;
            return sample;
        }

        /// <summary>
        /// Fill up a given GeoObject instance from an input Sample.
        /// </summary>
        /// <param name="geoObject">The Geoobject instance to be filled.</param>
        /// <param name="sample">The input Sample.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool SetupGeoObject(out GeoObject geoObject, Sample sample)
        {
            geoObject.Identifier = sample.Identifier;
            geoObject.DisplayText = sample.DisplayText;
            geoObject.StrokeBrush = sample.SampleStrokeBrush;
            geoObject.FillBrush = sample.SampleFillBrush;
            geoObject.StrokeTransparency = sample.SampleStrokeTransparency;
            geoObject.FillTransparency = sample.SampleFillTransparency;
            geoObject.StrokeThickness = sample.SampleStrokeThickness;
            geoObject.PointType = sample.SamplePointSymbol;
            geoObject.PointSymbolSize = sample.SamplePointSymbolSize;
            geoObject.GeometryData = BuildGeometryDataString(sample, false);
            if (geoObject.GeometryData == string.Empty)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Fill up a given GeoObject instance from an input Sample.
        /// </summary>
        /// <param name="geoObject">The Geoobject instance to be filled.</param>
        /// <param name="sample">The input Sample.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool SetupWorldGeoObject(out GeoObject geoObject, Sample sample)
        {
            geoObject.Identifier = sample.Identifier;
            geoObject.DisplayText = sample.DisplayText;
            geoObject.StrokeBrush = sample.SampleStrokeBrush;
            geoObject.FillBrush = sample.SampleFillBrush;
            geoObject.StrokeTransparency = sample.SampleStrokeTransparency;
            geoObject.FillTransparency = sample.SampleFillTransparency;
            geoObject.StrokeThickness = sample.SampleStrokeThickness;
            geoObject.PointType = sample.SamplePointSymbol;
            geoObject.PointSymbolSize = sample.SamplePointSymbolSize;
            geoObject.GeometryData = BuildGeometryDataString(sample, true);
            if (geoObject.GeometryData == string.Empty)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Fill up a given GeoObject list from an input Sample list.
        /// </summary>
        /// <param name="geoObjectList">The Geoobject list instance to be filled.</param>
        /// <param name="sampleList">The input Sample list.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool SetupGeoObject(out List<GeoObject> geoObjectList, List<Sample> sampleList)
        {
            geoObjectList = new List<GeoObject>();

            // Create GeoObjects for all samples in SampleList and add them to List
            foreach (Sample sample in sampleList)
            {
                GeoObject tempGeoObject = new GeoObject();
                if (sample.TypeOfSample == SampleType.IMAGE || !sample.IsSampleVisible)
                    continue;

                if (!SetupGeoObject(out tempGeoObject, sample))
                    return false;

                geoObjectList.Add(tempGeoObject);
            }
            return true;
        }

        /// <summary>
        /// Fill up a given GeoObject from an input Sample list.
        /// If more than one samples are in the list, the Geoobject will be set up as GEOMETRYCOLLECTION.
        /// </summary>
        /// <param name="geoObject">The Geoobject instance to be filled.</param>
        /// <param name="sampleList">The input Sample list.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool SetupGeoObject(out GeoObject geoObject, List<Sample> sampleList)
        {
            // Preset GeoObject 
            geoObject.GeometryData = geoObject.Identifier = geoObject.DisplayText = string.Empty;
            geoObject.StrokeBrush = geoObject.FillBrush = Brushes.Red;
            geoObject.StrokeTransparency = geoObject.FillTransparency = 0;
            geoObject.StrokeThickness = 1;
            geoObject.PointType = PointSymbol.Pin;
            geoObject.PointSymbolSize = 1;

            // List not empty?
            if (sampleList.Count > 0)
            {
                GeoObject tempGeoObject = new GeoObject();
                int objectCount = 0;
                // Create GeometryData for all samples in list
                foreach (Sample sample in sampleList)
                {
                    if (sample.TypeOfSample == SampleType.IMAGE || !sample.IsSampleVisible)
                        continue;

                    if (!SetupGeoObject(out tempGeoObject, sample))
                        return false;

                    geoObject.GeometryData += tempGeoObject.GeometryData + StrComma + StrBlank;
                    objectCount++;
                }
                geoObject.GeometryData = geoObject.GeometryData.TrimEnd(TrimChars);
                // If more than 1 object, create GEOMETRYCOLLECTION
                if (objectCount > 1)
                {
                    geoObject.GeometryData = SampleType.GEOMETRYCOLLECTION.ToString() + StrBlank + StrOpen + StrBlank
                        + geoObject.GeometryData
                        + StrBlank + StrClose;
                }
                // Finally assign parameters from the last sample
                geoObject.Identifier = tempGeoObject.Identifier;
                geoObject.DisplayText = tempGeoObject.DisplayText;
                geoObject.StrokeBrush = tempGeoObject.StrokeBrush;
                geoObject.FillBrush = tempGeoObject.FillBrush;
                geoObject.StrokeTransparency = tempGeoObject.StrokeTransparency;
                geoObject.FillTransparency = tempGeoObject.FillTransparency;
                geoObject.StrokeThickness = tempGeoObject.StrokeThickness;
                geoObject.PointType = tempGeoObject.PointType;
                geoObject.PointSymbolSize = tempGeoObject.PointSymbolSize;
            }
            return true;
        }

        /// <summary>
        /// Fill up a given GeoObject from an input Sample list.
        /// If more than one samples are in the list, the Geoobject will be set up as GEOMETRYCOLLECTION.
        /// </summary>
        /// <param name="geoObject">The Geoobject instance to be filled.</param>
        /// <param name="sampleList">The input Sample list.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool SetupWorldGeoObject(out GeoObject geoObject, List<Sample> sampleList)
        {
            // Preset GeoObject 
            geoObject.GeometryData = geoObject.Identifier = geoObject.DisplayText = string.Empty;
            geoObject.StrokeBrush = geoObject.FillBrush = Brushes.Red;
            geoObject.StrokeTransparency = geoObject.FillTransparency = 0;
            geoObject.StrokeThickness = 1;
            geoObject.PointType = PointSymbol.Pin;
            geoObject.PointSymbolSize = 1;

            // List not empty?
            if (sampleList.Count > 0)
            {
                GeoObject tempGeoObject = new GeoObject();
                int objectCount = 0;
                // Create GeometryData for all samples in list
                foreach (Sample sample in sampleList)
                {
                    if (sample.TypeOfSample == SampleType.IMAGE || !sample.IsSampleVisible)
                        continue;

                    if (!SetupWorldGeoObject(out tempGeoObject, sample))
                        return false;

                    geoObject.GeometryData += tempGeoObject.GeometryData + StrComma + StrBlank;
                    objectCount++;
                }
                geoObject.GeometryData = geoObject.GeometryData.TrimEnd(TrimChars);
                // If more than 1 object, create GEOMETRYCOLLECTION
                if (objectCount > 1)
                {
                    geoObject.GeometryData = SampleType.GEOMETRYCOLLECTION.ToString() + StrBlank + StrOpen + StrBlank
                        + geoObject.GeometryData
                        + StrBlank + StrClose;
                }
                // Finally assign parameters from the last sample
                geoObject.Identifier = tempGeoObject.Identifier;
                geoObject.DisplayText = tempGeoObject.DisplayText;
                geoObject.StrokeBrush = tempGeoObject.StrokeBrush;
                geoObject.FillBrush = tempGeoObject.FillBrush;
                geoObject.StrokeTransparency = tempGeoObject.StrokeTransparency;
                geoObject.FillTransparency = tempGeoObject.FillTransparency;
                geoObject.StrokeThickness = tempGeoObject.StrokeThickness;
                geoObject.PointType = tempGeoObject.PointType;
                geoObject.PointSymbolSize = tempGeoObject.PointSymbolSize;
            }
            return true;
        }


        /// <summary>
        /// Fill up a given GeoObject from a string array (from SQL sample file) at position index.
        /// </summary>
        /// <param name="geoObject">The Geoobject instance to be filled.</param>
        /// <param name="splitStrings">The input string array.</param>
        /// <param name="index">The index of the string array .</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool SetupGeoObject(out GeoObject geoObject, string[] splitStrings, ref int index)
        {
            // condition (splitStrings.Length >= index + SampleEntryLength) must be checked by caller!
            geoObject.Identifier = splitStrings[index++];
            geoObject.DisplayText = splitStrings[index++];
            geoObject.StrokeBrush = SetBrushFromName(splitStrings[index++]);
            geoObject.FillBrush = SetBrushFromName(splitStrings[index++]);
            geoObject.StrokeTransparency = Convert.ToByte(splitStrings[index++]);
            geoObject.FillTransparency = Convert.ToByte(splitStrings[index++]);
            geoObject.StrokeThickness = Convert.ToDouble(splitStrings[index++], CultureInfo.InvariantCulture);
            geoObject.PointType = SetPointSymbolFromName(splitStrings[index++]);
            geoObject.PointSymbolSize = (splitStrings[index] == string.Empty) ? m_PointSymbolSize : Convert.ToDouble(splitStrings[index], CultureInfo.InvariantCulture);
            index++;
            geoObject.GeometryData = splitStrings[index++];
            return true;
        }


        /// <summary>
        /// Implements a method to access the user interface (e.g. progress bar) while executing time consuming tasks in click events.
        /// </summary>
        public void DoEvents()
        {
            try
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
            }
            catch (Exception ex)
            {
                SetError("Access user interface error: " + ex.Message);
            }
        }

        /// <summary>
        /// Inits the progress bar.
        /// </summary>
        /// <param name="strInfo">The STR info.</param>
        /// <param name="maxCount">Maximum update value (~ 100%).</param>
        private void ProgressBarInit(string strInfo, int maxCount)
        {
            // labelInstruction.Visibility = System.Windows.Visibility.Collapsed;
            labelInstruction.Visibility = System.Windows.Visibility.Collapsed;
            labelProgressPercent.Content = "0 %";
            progressBarSaveSamples.Value = 0;
            labelProgressInfo.Content = strInfo;
            stackPanelProgress.Visibility = System.Windows.Visibility.Visible;
            m_ProgressCancel = false;
            m_ProgressMaxCount = maxCount;
            m_ProgressStepCounter = 0;
            m_ProgressCount = 0;
            DoEvents();
        }

        /// <summary>
        /// Updates the progress bar.
        /// </summary>
        /// <param name="count">The percent.</param>
        private void ProgressBarUpdate(int count)
        {
            // Report percent
            if (m_ProgressMaxCount > 0)
            {
                if (count > m_ProgressMaxCount)
                    count = m_ProgressMaxCount;
                int percent = count * 100 / m_ProgressMaxCount;
                if (percent >= m_ProgressStepCounter)
                {
                    progressBarSaveSamples.Value = percent;
                    labelProgressPercent.Content = string.Format("{0} %", percent.ToString());
                    DoEvents();
                    m_ProgressStepCounter += m_ProgressUpdateStep;
                }
            }
        }

        /// <summary>
        /// Closes the progress bar.
        /// </summary>
        private void ProgressBarClose()
        {
            stackPanelProgress.Visibility = System.Windows.Visibility.Collapsed;
            labelProgressPercent.Content = string.Empty;
            progressBarSaveSamples.Value = 100;
            labelProgressInfo.Content = string.Empty;
            DoEvents();
            // Reset maximum counter
            m_ProgressMaxCount = 0;
            m_ProgressStepCounter = 0;
            m_ProgressCount = 0;
            labelInstruction.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Builds a Geoobject data string from a given Sample according to MS SQL notation.
        /// </summary>
        /// <param name="sample">The input Sample.</param>
        /// <param name="worldCoords">if set to <c>true</c> [world coords].</param>
        /// <returns>The Geoobject data string.</returns>
        private string BuildGeometryDataString(Sample sample, bool worldCoords)
        {
            StringBuilder strBld = new StringBuilder();
            // ProgressBarUpdate(percent);
            try
            {
                // ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressSavingShape, 100);

                strBld.Append(sample.TypeOfSample.ToString());
                // dataString = sample.TypeOfSample.ToString();
                string pointString = string.Empty;
                if (sample.SampleWorldPointCollectionList.Count == 0)
                    worldCoords = false;

                List<PointCollection> pointCollectionList = worldCoords ? sample.SampleWorldPointCollectionList : sample.SamplePointCollectionList;

                // Setup counters
                // int count = 0;
                int numPoints = 0;
                foreach (PointCollection col in pointCollectionList)
                    numPoints += col.Count;

                if (pointCollectionList.Count > 1)
                    // dataString += StrOpen;
                    strBld.Append(StrOpen);
                foreach (PointCollection pointCollection in pointCollectionList)
                {
                    if (pointCollection.Count == 0)
                        continue;

                    // dataString += StrOpen;
                    strBld.Append(StrOpen);
                    if (sample.TypeOfSample == SampleType.POLYGON || sample.TypeOfSample == SampleType.MULTIPOLYGON)
                    {
                        // dataString += StrOpen;
                        strBld.Append(StrOpen);
                        // Duplicate first point as last point, if not identical (needed for Polygons in SQL database update)
                        if (pointCollection[0] != pointCollection[pointCollection.Count - 1])
                            pointCollection.Add(pointCollection[0]);
                    }

                    int countAlt = 0;
                    foreach (Point point in pointCollection)
                    {
                        Point worldPoint = (worldCoords ? point : CanvasToWorld(point));
                        double altitude = 0;
                        string strAltitude = string.Empty;
                        if (m_GetAltitude)
                        {
                            if (GetAltitude(worldPoint.X, worldPoint.Y, out altitude))
                            {
                                countAlt++;
                                strAltitude = StrBlank + altitude.ToString(CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                // No connection to geonames server, do not try again!
                                m_GetAltitude = false;
                            }
                        }

                        // Calculate Envelope Center of Point collections
                        SetEnvelopeCenter(worldPoint);

                        // dataString += worldPoint.X.ToString("F6", CultureInfo.InvariantCulture) + StrBlank
                        //            + worldPoint.Y.ToString("F6", CultureInfo.InvariantCulture) + strAltitude + StrComma + StrBlank;
                        strBld.AppendFormat("{0} {1}{2}, ", worldPoint.X.ToString(CultureInfo.InvariantCulture), worldPoint.Y.ToString(CultureInfo.InvariantCulture), strAltitude);
                        // count++;

                        if (stackPanelProgress.Visibility == System.Windows.Visibility.Visible)
                        {
                            m_ProgressCount++;
                            ProgressBarUpdate(m_ProgressCount);
                            // Cancel action
                            if (m_ProgressCancel)
                                break;
                        }

                    }
                    strBld.Replace(", ", "", strBld.Length - 2, 2);
                    strBld.Append(StrClose);
                    // dataString = dataString.TrimEnd(TrimChars) + StrClose;
                    if (sample.TypeOfSample == SampleType.POLYGON || sample.TypeOfSample == SampleType.MULTIPOLYGON)
                        strBld.Append(StrClose);
                    // dataString += StrClose;
                    // dataString += StrComma + StrBlank;
                    strBld.Append(", ");
                }
                strBld.Replace(", ", "", strBld.Length - 2, 2);
                // dataString = dataString.TrimEnd(TrimChars);
                if (pointCollectionList.Count > 1)
                    // dataString += StrClose;
                    strBld.Append(StrClose);

                // ProgressBarClose();

                // progressBarSaveSamples.Visibility = System.Windows.Visibility.Collapsed;
                return strBld.ToString();
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
            finally
            {
                // ProgressBarClose();
            }
            return string.Empty;
        }

        private void EnableEnvelopeCenter(bool enable)
        {
            m_EnvelopeCenterEnabled = enable;
        }

        private void SetEnvelopeCenter(Point worldPoint)
        {
            if (m_EnvelopeCenterEnabled)
            {
                // Initialize
                if (m_EnvelopeCenterXmin == 0 && m_EnvelopeCenterXmax == 0)
                {
                    m_EnvelopeCenterXmin = m_EnvelopeCenterXmax = worldPoint.X;
                }
                if (m_EnvelopeCenterYmin == 0 && m_EnvelopeCenterYmax == 0)
                {
                    m_EnvelopeCenterYmin = m_EnvelopeCenterYmax = worldPoint.Y;
                }
                // Calculate
                m_EnvelopeCenterXmin = Math.Min(worldPoint.X, m_EnvelopeCenterXmin);
                m_EnvelopeCenterXmax = Math.Max(worldPoint.X, m_EnvelopeCenterXmax);
                m_EnvelopeCenterYmin = Math.Min(worldPoint.Y, m_EnvelopeCenterYmin);
                m_EnvelopeCenterYmax = Math.Max(worldPoint.Y, m_EnvelopeCenterYmax);
                m_EnvelopeCenterX = (m_EnvelopeCenterXmin + m_EnvelopeCenterXmax) / 2.0;
                m_EnvelopeCenterY = (m_EnvelopeCenterYmin + m_EnvelopeCenterYmax) / 2.0;
            }
        }

        private void ClearEnvelopeCenter()
        {
            m_EnvelopeCenterXmin = 0;
            m_EnvelopeCenterXmax = 0;
            m_EnvelopeCenterYmin = 0;
            m_EnvelopeCenterYmax = 0;
            m_EnvelopeCenterX = 0;
            m_EnvelopeCenterY = 0;
        }

        /// <summary>
        /// Returns a PointSymbol object from a given enum name.
        /// </summary>
        /// <param name="name">The enum name.</param>
        /// <returns>The PointSymbol object.</returns>
        private PointSymbol SetPointSymbolFromName(string name)
        {
            if (name == PointSymbol.Pin.ToString())
                return PointSymbol.Pin;
            else if (name == PointSymbol.Cross.ToString())
                return PointSymbol.Cross;
            else if (name == PointSymbol.X.ToString())
                return PointSymbol.X;
            else if (name == PointSymbol.Circle.ToString())
                return PointSymbol.Circle;
            else if (name == PointSymbol.Square.ToString())
                return PointSymbol.Square;
            else if (name == PointSymbol.Diamond.ToString())
                return PointSymbol.Diamond;
            else if (name == PointSymbol.Pyramid.ToString())
                return PointSymbol.Pyramid;
            else if (name == PointSymbol.Cone.ToString())
                return PointSymbol.Cone;
            else if (name == PointSymbol.Minus.ToString())
                return PointSymbol.Minus;
            else if (name == PointSymbol.Questionmark.ToString())
                return PointSymbol.Questionmark;
            else if (name == PointSymbol.Circlepoint.ToString())
                return PointSymbol.Circlepoint;
            else if (name == PointSymbol.Assel_Icon.ToString())
                return PointSymbol.Assel_Icon;
            else if (name == PointSymbol.Bird_Icon.ToString())
                return PointSymbol.Bird_Icon;
            else if (name == PointSymbol.Bryophyt_Icon.ToString())
                return PointSymbol.Bryophyt_Icon;
            else if (name == PointSymbol.Echinoderm_Icon.ToString())
                return PointSymbol.Echinoderm_Icon;
            else if (name == PointSymbol.Evertebrate_Icon.ToString())
                return PointSymbol.Evertebrate_Icon;
            else if (name == PointSymbol.Fish_Icon.ToString())
                return PointSymbol.Fish_Icon;
            else if (name == PointSymbol.Fungus_Icon.ToString())
                return PointSymbol.Fungus_Icon;
            else if (name == PointSymbol.Insect_Icon.ToString())
                return PointSymbol.Insect_Icon;
            else if (name == PointSymbol.Lichen_Icon.ToString())
                return PointSymbol.Lichen_Icon;
            else if (name == PointSymbol.Mammal_Icon.ToString())
                return PointSymbol.Mammal_Icon;
            else if (name == PointSymbol.Mollusc_Icon.ToString())
                return PointSymbol.Mollusc_Icon;
            else if (name == PointSymbol.Myxomycete_Icon.ToString())
                return PointSymbol.Myxomycete_Icon;
            else if (name == PointSymbol.Plant_Icon.ToString())
                return PointSymbol.Plant_Icon;
            else if (name == PointSymbol.Reptile_Icon.ToString())
                return PointSymbol.Reptile_Icon;
            else if (name == PointSymbol.Vertebrate_Icon.ToString())
                return PointSymbol.Vertebrate_Icon;
            else if (name == PointSymbol.RedNeedle_Icon.ToString())
                return PointSymbol.RedNeedle_Icon;
            else if (name == PointSymbol.None.ToString())
                return PointSymbol.None;
            else
                return m_PointSymbol;
        }

        #endregion // GeoObjects

        #region Load and save images in files

        /// <summary>
        /// Load single image from a file path and add it to canvas at the given position.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="xPos">The horizontal offset.</param>
        /// <param name="yPos">The vertical offset.</param>
        /// <param name="transparency">The transparency value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool LoadSingleImage(string path, double xPos, double yPos, byte transparency)
        {
            try
            {
                if (m_SampleImage != null)
                {
                    m_SampleImage.ClearSample();
                    m_SampleImage = null;
                    // Reset world coordinates if no more reference map
                    if (WorldHasDisappeared())
                        ClearCoordinates();
                }

                // Create image object
                Point pointTopLeft = new Point(xPos, yPos);
                m_SampleImage = new SampleImage(m_Canvas, path, pointTopLeft);
                m_Transparency = m_StrokeTransparency = m_FillTransparency = 255;

                // ----------------------------------  New style -----------------------------------

                CheckResult chkResult = ExtractCoordinatesFromXmlFile(path);
                if (chkResult == CheckResult.Unaligned)
                {
                    chkResult = ExtractCoordinatesFromXmlFile(path);
                }
                // Try again with artificial north aligned background map
                if (chkResult != CheckResult.Failure)
                {
                    // Note: Automatically add loaded image to list if it is georeferenced!
                    // If not, give a chance to adjust it!

                    m_SampleImage.AddSample();
                    m_SampleImage.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);

                    if (m_Testcase)
                        m_WorkingAreaSaveFileName = textBoxIdentifier.Text;

                    // Set opacity
                    m_SampleImage.Transparency = transparency;
                    // Save image to list
                    SaveSample(m_SampleImage);
                    // Preserve reference map when image with coords is added to sample list
                    if (m_SampleImage.Tag != null)
                        if (m_SampleImage.Tag.ToString() == tagRef)
                            if (m_RefMap == null)   // ===== XXX Durchreichen der Referenzmap ?? =====
                                m_RefMap = m_SampleImage;
                    // Add a Toggle Button to switch the shape on and off
                    AddToggleButton(m_SampleImage);
                    m_SampleImage = null;
                }


                /* ----------------------------------  Old style -----------------------------------
                // Set world coordinates
                // ExtractCoordinatesFromFilename(path);
                if (ExtractCoordinatesFromXmlFile(path) == CheckResult.Unaligned)
                {
                    // Try again with artificial north aligned background map
                    if (ExtractCoordinatesFromXmlFile(path) != CheckResult.Failure)
                    {
                        // Note: Usually do not automatically add loaded image to list; give a chance to adjust it!

                        m_SampleImage.AddSample();
                        m_SampleImage.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                        // Set opacity
                        m_SampleImage.Transparency = transparency;
                        // Save image to list
                        SaveSample(m_SampleImage);
                        // Preserve reference map when image with coords is added to sample list
                        if (m_SampleImage.Tag != null)
                            if (m_SampleImage.Tag.ToString() == tagRef)
                                // if (m_RefMap == null)
                                    m_RefMap = m_SampleImage;
                        // Add a Toggle Button to switch the shape on and off
                        AddToggleButton(m_SampleImage);
                        m_SampleImage = null;
                    }
                }
                */
                /* ---------------------------------   Test   --------------------------------------
                else
                {


                    m_SampleImage.AddSample();
                    m_SampleImage.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                    // Set opacity
                    m_SampleImage.Transparency = transparency;
                    // Save image to list
                    SaveSample(m_SampleImage);
                    // Preserve reference map when image with coords is added to sample list
                    if (m_SampleImage.Tag != null)
                        if (m_SampleImage.Tag.ToString() == tagRef)
                            if (m_RefMap == null)
                                m_RefMap = m_SampleImage;
                    // Add a Toggle Button to switch the shape on and off
                    AddToggleButton(m_SampleImage);
                    m_SampleImage = null;
                }

                ---------------------------------   Test   -------------------------------------- */

            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load single image from a file path and add it to canvas at the given position.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="xPos">The horizontal offset.</param>
        /// <param name="yPos">The vertical offset.</param>
        /// <param name="transparency">The transparency value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool LoadSingleWebImage(Image image, double xPos, double yPos, byte transparency)
        {
            try
            {
                if (m_SampleImage != null)
                {
                    m_SampleImage.ClearSample();
                    m_SampleImage = null;
                    // Reset world coordinates if no more reference map
                    if (WorldHasDisappeared())
                        ClearCoordinates();
                }

                // Create image object
                Point pointTopLeft = new Point(xPos, yPos);
                m_SampleImage = new SampleImage(m_Canvas, image, pointTopLeft);
                m_Transparency = m_StrokeTransparency = m_FillTransparency = 255;

                // ----------------------------------  New style -----------------------------------

                // Set world coordinates or adapt image to present world
                //CheckWorldPosition(topLeft, bottomRight, bottomLeft, topRight);
                CheckWorldPosition(new Point(0, 1), new Point(1, 0), new Point(0, 0), new Point(1, 1));

                m_SampleImage.AddSample();
                m_SampleImage.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                // Set opacity
                m_SampleImage.Transparency = transparency;
                // Save image to list
                SaveSample(m_SampleImage);
                // Preserve reference map when image with coords is added to sample list
                if (m_SampleImage.Tag != null)
                    if (m_SampleImage.Tag.ToString() == tagRef)
                        // if (m_RefMap == null)
                        m_RefMap = m_SampleImage;
                // Add a Toggle Button to switch the shape on and off
                AddToggleButton(m_SampleImage);
                m_SampleImage = null;

            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }
            return true;
        }


        private bool debugout = false;

        /*
        private void PixelColor(Point point)
        {
            int dif = 20;

            // Reset Zoom and margin for scanning working area or frame
            Transform canvasRenderTransformSave = m_Canvas.RenderTransform;
            Thickness canvasMargin = m_Canvas.Margin;
            m_Canvas.RenderTransform = new ScaleTransform(1, 1);
            m_Canvas.Margin = m_CanvasThicknessZero;

            try
            {


                if (m_RefMap == null || m_RefMap.m_Image == null || m_RefMap.m_Image.Source == null)
                    return;

                int renderWidth = (int)m_RefMap.m_Image.Source.Width;
                int renderHeight = (int)m_RefMap.m_Image.Source.Height;

                // Render image of the given size (frame or ref map)
                RenderTargetBitmap rtb = new RenderTargetBitmap(renderWidth, renderHeight, 96, 96, PixelFormats.Pbgra32);
                // RenderTargetBitmap rtb = new RenderTargetBitmap((int)m_RefMap.m_Image.Source.Width, (int)m_RefMap.m_Image.Source.Height, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(m_Canvas);
                BitmapFrame bmf = BitmapFrame.Create(rtb);
                byte[] pixels = new byte[renderWidth * renderHeight * 4];
                bmf.CopyPixels(pixels, renderWidth * 4, 0);
                int index = (int)point.Y * renderWidth * 4 + (int)point.X * 4;
                byte b = pixels[index];
                byte g = pixels[index + 1];
                byte r = pixels[index + 2];
                Color newCol = new Color();
                newCol.A = 255;
                newCol.R = r;
                newCol.B = b;
                newCol.G = g;
                ellipseColor.Fill = new SolidColorBrush(newCol);
                // MessageBox.Show("Color picked " + newCol.ToString() + ", Point = " + point.ToString());
                int rMin = Math.Max(Convert.ToInt32(r) - dif, 0);
                int gMin = Math.Max(Convert.ToInt32(b) - dif, 0);
                int bMin = Math.Max(Convert.ToInt32(g) - dif, 0);
                int rMax = Math.Min(Convert.ToInt32(r) + dif, 255);
                int gMax = Math.Min(Convert.ToInt32(g) + dif, 255);
                int bMax = Math.Min(Convert.ToInt32(b) + dif, 255);
                m_LimitRedMin = (byte)rMin;
                m_LimitGreenMin = (byte)gMin;
                m_LimitBlueMin = (byte)bMin;
                m_LimitRedMax = (byte)rMax;
                m_LimitGreenMax = (byte)gMax;
                m_LimitBlueMax = (byte)bMax;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                m_Canvas.RenderTransform = canvasRenderTransformSave;
                m_Canvas.Margin = canvasMargin;
            }
        }
        */

        private void CheckImage()
        {
            // Reset Zoom and margin for scanning working area or frame
            Transform canvasRenderTransformSave = m_Canvas.RenderTransform;
            Thickness canvasMargin = m_Canvas.Margin;
            m_Canvas.RenderTransform = new ScaleTransform(1, 1);
            m_Canvas.Margin = m_CanvasThicknessZero;

            try
            {
                ClearUnsavedSamples();

                // Preset ID with reference map file name
                if (m_RefMap != null)
                    m_IdText = m_RefMap.Identifier;

                // Create Settings window
                SampleDetectionParameters parameters = new SampleDetectionParameters(this);
                // Show Dialog
                bool? dialogResult = parameters.ShowDialog();
                if (dialogResult == false)
                    return;

                // Working area coordinates cannot be saved if no ref map is available
                if (m_RefMap == null || m_RefMap.m_Image == null || m_RefMap.m_Image.Source == null)
                {
                    SetError(WpfSamplingPlotPage.Properties.Resources.ErrNoReferenceMap);
                    return;
                }

                int renderWidth = (int)m_RefMap.m_Image.Source.Width;
                int renderHeight = (int)m_RefMap.m_Image.Source.Height;
                // byte limit = 150;
                int count = 0;
                int count2 = 0;

                // Render image of the given size (frame or ref map)
                RenderTargetBitmap rtb = new RenderTargetBitmap(renderWidth, renderHeight, 96, 96, PixelFormats.Pbgra32);
                // RenderTargetBitmap rtb = new RenderTargetBitmap((int)m_RefMap.m_Image.Source.Width, (int)m_RefMap.m_Image.Source.Height, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(m_Canvas);
                BitmapFrame bmf = BitmapFrame.Create(rtb);
                byte[] pixels = new byte[renderWidth * renderHeight * 4];
                bmf.CopyPixels(pixels, renderWidth * 4, 0);
                int index = 0;
                byte[,] points = new byte[renderHeight, renderWidth];

                WLine("Original points:");
                for (int y = 0; y < renderHeight; y++)
                {
                    string arr = y.ToString();
                    for (int x = 0; x < renderWidth; x++)
                    {
                        // Color col = pixels[y + x];

                        byte b = pixels[index];
                        byte g = pixels[index + 1];
                        byte r = pixels[index + 2];
                        points[y, x] = 0;
                        if (b >= m_LimitBlueMin && b <= m_LimitBlueMax && g >= m_LimitGreenMin && g <= m_LimitGreenMax && r >= m_LimitRedMin && r <= m_LimitRedMax)
                        {
                            points[y, x] = 1;
                            count++;
                            // Point world = CanvasToWorld(pnt);
                            // strGeography += world.X.ToString(CultureInfo.InvariantCulture) + " " + world.Y.ToString(CultureInfo.InvariantCulture) + ", ";
                        }
                        if (points[y, x] == 0)
                            arr += ".";
                        else
                            arr += "O";

                        index += 4;
                        // arr += points[y, x].ToString();
                    }
                    WLine(arr);
                }

                int indStart = -1;
                int indEnd = -1;

                WLine("Vertically reduced points:");

                // Convert lines to centerpoints in vertical direction
                for (int x = 0; x < renderWidth; x++)
                {
                    indStart = -1;
                    indEnd = -1;

                    for (int y = 0; y < renderHeight; y++)
                    {
                        if (points[y, x] == 1)
                        {
                            if (indStart == -1)
                            {
                                indStart = y;
                                indEnd = y;
                            }
                            else
                            {
                                indEnd = y;
                            }
                        }
                        else
                        {
                            if (indStart != -1)
                            {
                                for (int i = indStart; i <= indEnd; i++)
                                {
                                    points[i, x] = 0;
                                }
                                int indCenter = indStart + (indEnd - indStart) / 2;
                                points[indCenter, x] = 1;
                                indStart = -1;
                                indEnd = -1;
                            }
                        }
                    }
                    // Finally reduce last section
                    if (indStart != -1)
                    {
                        for (int i = indStart; i <= indEnd; i++)
                        {
                            points[i, x] = 0;
                        }
                        int indCenter = indStart + (indEnd - indStart) / 2;
                        points[indCenter, x] = 1;
                        indStart = -1;
                        indEnd = -1;
                    }

                }

                // Write debug file
                for (int y = 0; y < renderHeight; y++)
                {
                    string arr = y.ToString();
                    for (int x = 0; x < renderWidth; x++)
                        if (points[y, x] == 0)
                            arr += ".";
                        else
                            arr += "O";
                    WLine(arr);
                }


                // Convert lines to centerpoints in horizontal direction
                for (int y = 0; y < renderHeight; y++)
                {
                    indStart = -1;
                    indEnd = -1;

                    for (int x = 0; x < renderWidth; x++)
                    {
                        if (points[y, x] == 1)
                        {
                            if (indStart == -1)
                            {
                                indStart = x;
                                indEnd = x;
                            }
                            else
                            {
                                indEnd = x;
                            }
                        }
                        else
                        {
                            if (indStart != -1)
                            {
                                for (int i = indStart; i <= indEnd; i++)
                                {
                                    points[y, i] = 0;
                                }
                                int indCenter = indStart + (indEnd - indStart) / 2;
                                points[y, indCenter] = 1;

                                if (y > 0)
                                {
                                    if (indCenter > 0 && points[y - 1, indCenter - 1] == 1)
                                        points[y - 1, indCenter - 1] = 0;
                                    if (indCenter < renderWidth - 1 && points[y - 1, indCenter + 1] == 1)
                                        points[y - 1, indCenter + 1] = 0;
                                }

                                indStart = -1;
                                indEnd = -1;
                            }
                        }
                    }
                    // Finally reduce last section
                    if (indStart != -1)
                    {
                        for (int i = indStart; i <= indEnd; i++)
                        {
                            points[y, i] = 0;
                        }
                        int indCenter = indStart + (indEnd - indStart) / 2;
                        points[y, indCenter] = 1;

                        if (y > 0)
                        {
                            if (indCenter > 0 && points[y - 1, indCenter - 1] == 1)
                                points[y - 1, indCenter - 1] = 0;
                            if (indCenter < renderWidth - 1 && points[y - 1, indCenter + 1] == 1)
                                points[y - 1, indCenter + 1] = 0;
                        }

                        indStart = -1;
                        indEnd = -1;
                    }

                }

                // Write debug file
                WLine("Horizontally reduced points:");
                for (int y = 0; y < renderHeight; y++)
                {
                    string arr = y.ToString();
                    for (int x = 0; x < renderWidth; x++)
                        if (points[y, x] == 0)
                            arr += ".";
                        else
                            arr += "O";
                    WLine(arr);
                }


                byte[,] finpoints = new byte[renderHeight, renderWidth];

                for (int y = 0; y < renderHeight; y++)
                {
                    int xcnt = 0;
                    int minDist = m_PointDistanceMin - 1;
                    for (int x = 0; x < renderWidth; x++)
                    {
                        finpoints[y, x] = 0;
                        // There is a point to set
                        if (points[y, x] == 1)
                        {
                            // Is the minimal distance to the last set point at the line OK?
                            if (xcnt > minDist)
                            {
                                //
                                bool setpoint = true;
                                // if (x > minDist && y > minDist && x < renderWidth - minDist)
                                // Check other points set in minimal distance above - left and right of the point
                                if (y > 0)
                                {
                                    int yStartRange = Math.Max(y - minDist, 0);
                                    for (int iy = yStartRange; iy < y; iy++)
                                    {
                                        int xStartRange = Math.Max(x - minDist, 0);
                                        int xEndRange = Math.Min(x + minDist, renderWidth);
                                        for (int ix = xStartRange; ix < xEndRange; ix++)
                                            if (finpoints[iy, ix] == 1)
                                                setpoint = false;
                                    }
                                }
                                if (setpoint)
                                {
                                    Point pnt = new Point((Double)x, (Double)y);
                                    if (m_SeparateSamples)
                                    {
                                        // string str = IdEnumeration(m_IdText, m_IdNumber, count2);
                                        // SetMarkerPoint(pnt, str, ((textBoxSampleText.Text == string.Empty) ? "Point" : textBoxSampleText.Text) + " " + (count2 + 1).ToString());
                                        string str = IdEnumeration("", m_IdNumber, count2);
                                        SetMarkerPoint(pnt, m_IdText, ((textBoxSampleText.Text == string.Empty) ? str : textBoxSampleText.Text));
                                        buttonAddShape_Click(null, null);
                                    }
                                    else
                                    {
                                        // SetMarkerPoint(pnt, m_IdText + m_IdNumber, ((textBoxSampleText.Text == string.Empty) ? "Points" : textBoxSampleText.Text));
                                        SetMarkerPoint(pnt, m_IdText, ((textBoxSampleText.Text == string.Empty) ? m_IdNumber : textBoxSampleText.Text));
                                    }
                                    count2++;
                                    xcnt = 0;
                                    finpoints[y, x] = 1;
                                }
                            }
                        }
                        xcnt++;
                    }
                }

                WLine("Distanz reduced points:");
                for (int y = 0; y < renderHeight; y++)
                {
                    string arr = y.ToString();
                    for (int x = 0; x < renderWidth; x++)
                        if (finpoints[y, x] == 0)
                            arr += ".";
                        else
                            arr += "O";
                    WLine(arr);
                }


                int temp = count2;

                // textBoxIdentifier.Text = count.ToString() + " -> " + count2.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                m_Canvas.RenderTransform = canvasRenderTransformSave;
                m_Canvas.Margin = canvasMargin;
            }
        }

        private string IdEnumeration(string idText, string idNumber, int count)
        {
            string result = idText + idNumber;
            try
            {
                int index = Convert.ToInt32(idNumber) + count;
                string specifier = "D" + idNumber.Length.ToString();
                string newIndex = index.ToString(specifier);
                result = idText + newIndex;
            }
            catch
            {
            }
            return result;
        }

        private void SetMarkerPoint(Point point, string id, string text)
        {
            // Create shape if none available
            if (m_SamplePoints == null)
            {
                m_SamplePoints = new SamplePoints(m_Canvas);
                // SetComboboxBrush();
                // m_SamplePoints.Identifier = id;
                // m_SamplePoints.DisplayText = text;
                m_SamplePoints.StrokeBrush = m_LastStrokeBrush;
                m_SamplePoints.FillBrush = m_LastFillBrush;
                SetStrokeThickness();
                m_SamplePoints.SamplePointSymbol = m_PointSymbol;
                m_SamplePoints.SamplePointSymbolSize = m_PointSymbolSize;
                ResetCheckBoxes();
            }
            // Set next point of sample points
            // m_SamplePoints.Identifier = id;
            m_SamplePoints.AddId(id, text);
            m_SamplePoints.SetSamplePoint(point);
        }


        /// <summary>
        /// Creates a blank background reference map of a given image size and world coordinates.
        /// </summary>
        /// <param name="width">The width of the map.</param>
        /// <param name="height">The height of the map.</param>
        /// <param name="topLeft">The world coordinates of the top left corner.</param>
        /// <param name="bottomRight">The world coordinates of the bottom right corner.</param>
        /// <returns></returns>
        private bool CreateBackgroundMapWithCoordinates(int width, int height, Point topLeft, Point bottomRight)
        {
            try
            {
                // Create blank image of given size (width, height)
                Image blankMap = BlankMap(width, height);

                // Create image object
                Point pointTopLeft = new Point(0, 0);
                SampleImage sampleImage = new SampleImage(m_Canvas, blankMap, pointTopLeft);
                if (sampleImage == null)
                    return false;

                // Background, place underneath all other images
                sampleImage.BringToBottom();

                // Set world coordinates from not aligned map
                // Change World coordinates
                m_WorldLeft = topLeft.X;
                m_WorldTop = topLeft.Y;
                m_WorldRight = bottomRight.X;
                m_WorldBottom = bottomRight.Y;
                // Calculate factor in respect of the 180° border!
                double worldLonDist = m_WorldRight - m_WorldLeft;
                if (m_WorldRight < m_WorldLeft)
                    worldLonDist += 360;
                m_XFactor = worldLonDist / (sampleImage.m_Image.Source.Width);
                m_YFactor = (m_WorldBottom - m_WorldTop) / (sampleImage.m_Image.Source.Height);
                sampleImage.Tag = tagRef;

                // Preserve reference map when Google map is added
                if (m_RefMap == null)
                    m_RefMap = sampleImage;

                sampleImage.AddSample();
                // Add identifier and display text
                sampleImage.AddId(StrIdBlank, StrTextBackground);
                // Set opacity                             
                sampleImage.Transparency = 255;
                // Save it to list
                SaveSample(sampleImage);
                // Add a Toggle Button to switch the shape on and off
                AddToggleButton(sampleImage);
                // Set blank map
                m_BlankMap = sampleImage;
                return true;
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }
        }

        /* ATTENTION: Workaround for Windows7 problem from July 2013: Clipboard seems to be flushed frequently after reading!!
        private string m_LastCoords = string.Empty;
        // private int m_ReadCoords = 0;

        /// <summary>
        /// Reads the coordinates from clipboard.
        /// </summary>
        /// <returns></returns>
        string ReadCoordinatesFromClipboardTwice()
        {
            string retStr = ReadCoordinatesFromClipboard();
            if (retStr != null && retStr.Length <= 0)  //  || retStr[0] != '(')
            {
                // string retFirst = retStr;
                Thread.Sleep(100);
                retStr = ReadCoordinatesFromClipboard();
                // MessageBox.Show("2nd try: " + retFirst + " ~> " + retStr);
            }
            return retStr;
        }
        */

        private void M_WebBrowser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {

            // MessageBox.Show("WebBrowser_WebMessageReceived ?");
            m_WebView2Coordinates = e.WebMessageAsJson.Trim('"');
            m_WebView2Coordinates_Received = true;
            // System.Windows.Forms.Clipboard.SetText(m_WebView2Coordinates);
            // MessageBox.Show("WebBrowser_WebMessageReceived " + m_WebView2Coordinates);
        }

        private void M_WebBrowser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            m_WebBrowserNavigated = false;
        }

        private void M_WebBrowser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            m_WebBrowserNavigated = true;
        }


        public async void CallJavaScript()
        {
            if (m_WebBrowserNavigated)
            {
                m_WebView2Coordinates_Received = false;
                string javaScript = "window.setClipboardData();";
                await m_WebBrowser.ExecuteScriptAsync(javaScript);
            }
        }

        /// <summary>
        /// Reads the coordinates from clipboard.
        /// </summary>
        /// <returns></returns>
        string ReadCoordinatesFromClipboard()
        {
            try
            {
                // Read current clipboard content
                // WLine("ReadCoordinatesFromClipboard");
                // -- obsolete -- ATTENTION: Workaround for Windows7 problem from July 2013: Clipboard seems to be flushed frequently after reading! Preset with last coords!
                string tmp = string.Empty;
                // if (System.Windows.Forms.Clipboard.ContainsText())
                {
                    // MessageBox.Show("ContainsText");
                    // NOTE: Reading data will clear the clipboard on Windows7 since summer 2013 !!
                    // tmp = System.Windows.Forms.Clipboard.GetText();
                    /*------------ using Forms makes Problems under .NET 4.8: Reading clipboard data is unreliable!
                    System.Windows.Forms.IDataObject myData = System.Windows.Forms.Clipboard.GetDataObject();
                    tmp = (string)myData.GetData(typeof(string));
                    System.Windows.Forms.Clipboard.SetDataObject(tmp, true);
                    ------------- */
                    // IDataObject myData = Clipboard.GetDataObject();
                    // tmp = (string)myData.GetData(typeof(string));
                    // MessageBox.Show("GetText: " + tmp);
                    tmp = m_WebView2Coordinates;


                    /*
                    if (tmp == null)
                    {
                        return string.Empty; 
                    }
                    if (tmp != string.Empty)
                    {
                        // MessageBox.Show("GetText not Empty... Coords = " + tmp);
                        // keep last successful read coordinates!
                        m_LastCoords = tmp;
                        // System.Windows.Forms.Clipboard.SetText(m_LastCoords);
                        // WLine("ok: " + tmp);
                    }
                    else
                    {
                        return string.Empty;
                    }
                    // System.Windows.Forms.Clipboard.SetText(m_LastCoords);
                    */
                }

                // m_ReadCoords++;
                // WLine(string.Format("read {0}: {1}", m_ReadCoords.ToString(), tmp));

                return tmp;

                // return m_LastCoords;
            }
            catch (Exception ex)
            {
                SetError("ReadCoordinatesFromClipboard Exception: " + ex.Message + " - " + ex.InnerException.Message);
                return string.Empty;       //  m_LastCoords;
            }

        }


        /// <summary>
        /// Load single image object and add it to canvas at the given position.
        /// </summary>
        /// <param name="image">The Image object.</param>
        /// <param name="xPos">The horizontal offset.</param>
        /// <param name="yPos">The vertical offset.</param>
        /// <param name="transparency">The transparency value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool LoadSingleImage(Image image, double xPos, double yPos, byte transparency)
        {
            // WLine("LoadSingleImage");
            try
            {
                // Create image object
                if (m_SampleImage != null)
                {
                    m_SampleImage.ClearSample();
                    m_SampleImage = null;
                    if (WorldHasDisappeared())
                        ClearCoordinates();
                }
                Point pointTopLeft = new Point(xPos, yPos);
                m_SampleImage = new SampleImage(m_Canvas, image, pointTopLeft);

                string tmp = ReadCoordinatesFromClipboard();

                // Set the world coordinates from clipboard: e.g. "((48.158785932052986, 11.475863456726074), (48.17118013157114, 11.507148742675781))"
                // SetError("Coords from Clipboard: " + checkCB .ToString() + " [" + tmp + "]");
                // WLine("Coordinates = (" + tmp + ")");
                if (!ExtractCoordinatesFromWeb(tmp))
                {
                    // WLine("----- " + tmp);
                    m_SampleImage.ClearSample();
                    m_SampleImage = null;
                    if (WorldHasDisappeared())
                        ClearCoordinates();
                    return false;
                }
                // WLine("----- " + tmp);

                m_SampleImage.Tag = tagRef;
                // Preserve reference map when Google map is added
                if (m_RefMap == null)
                    m_RefMap = m_SampleImage;

                m_SampleImage.AddSample();
                // Add identifier and display text
                m_SampleImage.AddId(textBoxIdentifier.Text, textBoxSampleText.Text);
                // Set opacity
                m_SampleImage.Transparency = transparency;
                // Save it to list
                SaveSample(m_SampleImage);
                // Add a Toggle Button to switch the shape on and off
                AddToggleButton(m_SampleImage);

                // m_SampleImage = null;

                return true;
            }
            catch (Exception ex)
            {
                SetError("LoadSingleImage: " + ex.ToString() + " - " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Saves the entire working area including visible samples to bitmap file on disk.
        /// Saves the coordinates of the area map in an XML file with same name.
        /// </summary>
        /// <param name="filename">The bitmap filename.</param>
        private void SaveWorkingArea(string filename)
        {
            int renderWidth = 0;
            int renderHeight = 0;
            // Working area coordinates cannot be saved if no ref map is available
            if (m_RefMap == null || m_RefMap.m_Image == null || m_RefMap.m_Image.Source == null)
            {
                SetError(WpfSamplingPlotPage.Properties.Resources.ErrNoReferenceMap);
                return;
            }
            if (m_AreaFrame)
            {
                renderWidth = (int)m_AreaFrameWidth;
                renderHeight = (int)m_AreaFrameHeight;
                m_CaptureFrame.ClearFrame();
            }
            else
            {
                renderWidth = (int)m_RefMap.m_Image.Source.Width;
                renderHeight = (int)m_RefMap.m_Image.Source.Height;
            }
            // Render image of the given size (frame or ref map)
            RenderTargetBitmap rtb = new RenderTargetBitmap(renderWidth, renderHeight, 96, 96, PixelFormats.Pbgra32);
            // RenderTargetBitmap rtb = new RenderTargetBitmap((int)m_RefMap.m_Image.Source.Width, (int)m_RefMap.m_Image.Source.Height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(m_Canvas);

            // Encoding the RenderBitmapTarget as a PNG file.
            PngBitmapEncoder png = new PngBitmapEncoder();
            // png.Metadata = new BitmapMetadata("png");

            png.Frames.Add(BitmapFrame.Create(rtb));

            // Write to file
            string shapefilename = String.Format("{0}{1}", filename, StrShapeFileExtension);
            filename = String.Format("{0}{1}", filename, StrMapFileExtension);
            using (Stream stm = File.Create(filename))
            {
                png.Save(stm);
            }
            m_RefMap.ImageFilePath = filename;

            if (m_AreaFrame)
            {
                Point frameBottomRightPosition = new Point();
                frameBottomRightPosition.X = m_CaptureFrame.FramePosition.X + (double)renderWidth;
                frameBottomRightPosition.Y = m_CaptureFrame.FramePosition.Y + (double)renderHeight;
                WriteImageAttributesToXmlFile(m_RefMap, m_CaptureFrame.FramePosition, frameBottomRightPosition);
                if (m_WriteImageToKalFile)
                    WriteImageAttributesToKalFile(m_RefMap, m_CaptureFrame.FramePosition, frameBottomRightPosition, (int)m_CaptureFrame.m_Frame.Width, (int)m_CaptureFrame.m_Frame.Height);
                m_CaptureFrame.ShowFrame();
            }
            else
            {
                WriteImageAttributesToXmlFile(m_RefMap);
                if (m_WriteImageToKalFile)
                    WriteImageAttributesToKalFile(m_RefMap);
            }

        }

        /// <summary>
        /// Creates a frame on the canvas if only a region of the working area should be saved.
        /// </summary>
        internal void SetCaptureFrame()
        {
            if (m_CaptureFrame == null && m_AreaFrame)
            {
                m_CaptureFrame = new CaptureFrame(m_Canvas, m_Zero);
                m_CaptureFrame.m_Frame.MouseLeftButtonUp += m_Frame_MouseLeftButtonUp;
                m_CaptureFrame.m_Frame.MouseRightButtonDown += m_Frame_MouseRightButtonDown;
            }
            else if (m_CaptureFrame != null && !m_AreaFrame)
            {
                m_CaptureFrame.ClearFrame();
                m_CaptureFrame = null;
            }
            if (m_CaptureFrame != null)
            {
                m_CaptureFrame.SetSize(m_AreaFrameWidth, m_AreaFrameHeight);
                // Show frame depending on Mode
                if (m_MouseMode == MouseMode.ShiftCanvas)
                    ShowCaptureFrame(true);
                else
                    ShowCaptureFrame(false);
            }
        }

        internal void ShowCaptureFrame(bool show)
        {
            if (m_CaptureFrame != null)
            {
                if (show)
                    m_CaptureFrame.m_Frame.Visibility = System.Windows.Visibility.Visible;
                else
                    m_CaptureFrame.m_Frame.Visibility = System.Windows.Visibility.Hidden;
            }
        }


        void m_Frame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*
            string result = string.Empty;
            double frameXmin = m_CaptureFrame.FramePosition.X;
            double frameYmin = m_CaptureFrame.FramePosition.Y;
            double frameXmax = frameXmin + m_CaptureFrame.FrameWidth;
            double frameYmax = frameYmin + m_CaptureFrame.FrameHeight;

            DateTime start = DateTime.Now;
            TimeSpan time = DateTime.Now - start;
            long maxTicks = 0;
            TimeSpan ticks;
            DateTime tickStart = DateTime.Now;
            start = DateTime.Now;

            foreach (Sample sample in m_SampleList)
            {
                bool inside = false;
                foreach (PointCollection pointCollection in sample.SamplePointCollectionList)
                {
                    foreach (Point point in pointCollection) // collection aufsplitten??
                    {
                        if (point.X >= frameXmin && point.X <= frameXmax && point.Y >= frameYmin && point.Y <= frameYmax)
                        {
                            inside = true;
                            break;
                        }
                    }
                    if (inside)
                        break;
                }
                tickStart = DateTime.Now;
                // switch sample off/on
                if (inside)
                {
                    SwitchSample(sample, true);
                }
                else
                {
                    SwitchSample(sample, false);
                }
                ticks = DateTime.Now - tickStart;
                if (maxTicks < ticks.Ticks)
                    maxTicks = ticks.Ticks;
            }
            time = DateTime.Now - start;


            // SetInstructionLabel("Time = " + (time.Milliseconds).ToString("F0") + " msec,  maxTicks = " + maxTicks.ToString(), true);
            */

        }

        void m_Frame_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Disable in GIS View Mode
            if (m_ViewerMode)
                return;

            string result = string.Empty;
            double frameXmin = m_CaptureFrame.FramePosition.X;
            double frameYmin = m_CaptureFrame.FramePosition.Y;
            double frameXmax = frameXmin + m_CaptureFrame.FrameWidth;
            double frameYmax = frameYmin + m_CaptureFrame.FrameHeight;

            int count = m_SampleList.Count;
            ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressSelectingSamples, count);
            count = 0;

            foreach (Sample sample in m_SampleList)
            {
                bool inside = false;
                foreach (PointCollection pointCollection in sample.SamplePointCollectionList)
                {
                    foreach (Point point in pointCollection) // collection aufsplitten??
                    {
                        // =========== HIER! ========
                        if (point.X >= frameXmin && point.X <= frameXmax && point.Y >= frameYmin && point.Y <= frameYmax)
                        {
                            inside = true;
                            break;
                        }
                    }
                    if (inside)
                        break;
                }
                // switch sample off/on
                if (inside)
                {
                    // SwitchSample(sample, false);
                    SwitchSample(sample, true);
                }
                else
                {
                    // SwitchSample(sample, true);
                    SwitchSample(sample, false);
                }

                count++;
                ProgressBarUpdate(count);
            }

            ProgressBarUpdate(m_SampleList.Count);
            ProgressBarClose();
        }

        private void SwitchSample(Sample sample, bool switchOn)
        {
            SampleType sampleType = sample.TypeOfSample;
            ToggleButton clickedButton = null;
            // Picture buttons
            switch (sampleType)
            {
                case SampleType.POLYGON:
                case SampleType.MULTIPOLYGON:
                    foreach (Polygon poly2 in (sample as SamplePolygon).m_PolyList)
                    {
                        clickedButton = poly2.DataContext as ToggleButton;
                        clickedButton.IsChecked = switchOn;
                    }

                    break;

                case SampleType.LINESTRING:
                case SampleType.MULTILINESTRING:
                    foreach (Polyline line2 in (sample as SampleLines).m_PolyList)
                    {
                        clickedButton = line2.DataContext as ToggleButton;
                        clickedButton.IsChecked = switchOn;
                    }
                    break;

                case SampleType.POINT:
                case SampleType.MULTIPOINT:
                    // Do we have icons?
                    if ((sample as SamplePoints).m_ImageList.Count > 0)
                    {
                        foreach (Image line2 in (sample as SamplePoints).m_ImageList)
                        {
                            clickedButton = line2.DataContext as ToggleButton;
                            clickedButton.IsChecked = switchOn;
                        }
                    }
                    // No, we have shapes!
                    else
                    {
                        // Create copy of objects path
                        System.Windows.Shapes.Path path = (sample as SamplePoints).m_Path;
                        clickedButton = path.DataContext as ToggleButton;
                        clickedButton.IsChecked = switchOn;
                    }
                    break;

                default:
                    break;
            }
        }

        /* Save working area to bitmap file using a separate file dialog
        void SaveWorkingAreaDialog()
        {
            // Reset Zoom and margin
            m_Canvas.Margin = CanvasThicknessZero;
            m_Canvas.RenderTransform = new ScaleTransform(1, 1);
            // MessageBox.Show("Press to save the working area");

            // Save file dialog
            SaveFileDialog dlg = new SaveFileDialog();
            // Filter settings
            dlg.Filter = StrFilterImage;
            dlg.Title = WpfSamplingPlotPage.Properties.Resources.StrSaveImageDialog;
            // Show dialog
            dlg.ShowDialog();
            if (dlg.FileName.EndsWith(StrMapFileExtension))
                dlg.FileName = dlg.FileName.Remove(dlg.FileName.Length - 4);

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)m_RefMap.m_Image.Source.Width, (int)m_RefMap.m_Image.Source.Height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(m_Canvas);

            // Encoding the RenderBitmapTarget as a PNG file.
            PngBitmapEncoder png = new PngBitmapEncoder();
            // png.Metadata = new BitmapMetadata("png");
            png.Frames.Add(BitmapFrame.Create(rtb));

            // Write to file
            string filename = String.Format("{0}{1}", dlg.FileName, StrMapFileExtension);
            using (Stream stm = File.Create(filename))
            {
                png.Save(stm);
            }
            m_RefMap.ImageFilePath = filename;
            WriteImageAttributesToXmlFile(m_RefMap);

            // Restore Zoom and margin
            m_Canvas.Margin = m_CanvasThickness;
            m_Canvas.RenderTransform = new ScaleTransform(m_ZoomFactor, m_ZoomFactor);

        }
        */

        // Save captured map to file
        /// <summary>
        /// Opens a save file dialog and saves the given Image as a bitmap file on disk.
        /// Saves the coordinates of the image in an XML file with same name.
        /// </summary>
        /// <param name="image">The input image.</param>
        void SaveSingleImage(Image image)
        {
            try
            {
                // Save file dialog
                SaveFileDialog dlg = new SaveFileDialog();
                // Filter settings
                dlg.Filter = StrFilterImage;
                dlg.Title = WpfSamplingPlotPage.Properties.Resources.StrSaveImageDialog;
                // Use the last filename and add/increase a count number
                if (m_SaveFilename.Length > 4 && m_SaveFilename[m_SaveFilename.Length - 4] == '_')
                    m_SaveFilename = m_SaveFilename.Remove(m_SaveFilename.Length - 4);

                dlg.FileName = m_SaveFilename + "_" + m_SaveFileCount.ToString("D3");
                // dlg.InitialDirectory = StrInitialDirectory;
                // Show dialog
                if (dlg.ShowDialog() == false)
                    return;
                if (dlg.FileName.EndsWith(StrMapFileExtension))
                    dlg.FileName = dlg.FileName.Remove(dlg.FileName.Length - 4);
                // Save last filename
                int index = dlg.FileName.LastIndexOf('\\');
                m_SaveFilename = dlg.FileName.Substring(index + 1);

                // The Visual to use as the source of the RenderTargetBitmap.
                DrawingVisual drawingVisual = new DrawingVisual();
                DrawingContext drawingContext = drawingVisual.RenderOpen();
                drawingContext.DrawImage(image.Source, new Rect(0, 0, image.Source.Width, image.Source.Height));
                drawingContext.Close();

                // The BitmapSource that is rendered with a Visual.
                RenderTargetBitmap rtb = new RenderTargetBitmap((int)image.Source.Width, (int)image.Source.Height, 96, 96, PixelFormats.Pbgra32);
                rtb.Render(drawingVisual);

                // Encoding the RenderBitmapTarget as a PNG file.
                PngBitmapEncoder png = new PngBitmapEncoder();
                // png.Metadata = new BitmapMetadata("png");
                png.Frames.Add(BitmapFrame.Create(rtb));

                /* Write Metadata does not work
                string filename = String.Format("{0}{1}", dlg.FileName, ".tiff");
                FileStream stream3 = new FileStream(filename, FileMode.Create);
                BitmapMetadata myBitmapMetadata = new BitmapMetadata("tiff");
                TiffBitmapEncoder encoder3 = new TiffBitmapEncoder();
                myBitmapMetadata.Comment = "Nice Picture";
                myBitmapMetadata.ApplicationName = "Microsoft Digital Image Suite 10";
                encoder3.Metadata = myBitmapMetadata;
                encoder3.Frames.Add(BitmapFrame.Create(rtb));
                encoder3.Save(stream3);
                stream3.Close();
                */

                // Write to file (old style: Coords in the file name)
                string filename = String.Format("{0}{1}", dlg.FileName, StrMapFileExtension /* GetImageCoords(m_SampleImage),StrMapFileExtension*/ );
                if (m_SampleImage != null)
                    m_SampleImage.ImageFilePath = filename;

                using (Stream stm = File.Create(filename))
                {
                    png.Save(stm);
                }
                WriteImageAttributesToXmlFile(m_SampleImage);
                if (m_WriteImageToKalFile)
                    WriteImageAttributesToKalFile(m_SampleImage);
                m_SaveFileCount++;
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }


        /// <summary>
        /// Pre-checks if the file is supported.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if the given filename's extension is a supported format; otherwise, <c>false</c>.</returns>
        private bool isSupportedImageFile(string fileName)
        {
            try
            {
                // check if filename extension is supported
                foreach (string str in m_SupportedFormats)
                {
                    if ((fileName.ToLower()).EndsWith(str))
                        return true;
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }

            return false;
        }

        // Load all images within directory given by path (not used)
        /// <summary>
        /// Load all images within directory given by path (-- not used --).
        /// </summary>
        /// <param name="path">The directory path.</param>
        public void LoadImageDirectory(string path)
        {
            try
            {
                // Check if directory exists
                if (Directory.Exists(path))
                {
                    // Process the list of files found in the directory.
                    string[] fileEntries = Directory.GetFiles(path);
                    foreach (string fileName in fileEntries)
                    {
                        if (isSupportedImageFile(fileName))
                        {
                            if (LoadSingleImage(fileName, m_XPos, 0, 255))
                                radioButtonShift.IsChecked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
        }

        /* Get Google coordinates from Filename (no longer used)
        private bool ExtractCoordinatesFromFilename(string path)
        {
            // Preset with device coordinates
            // m_WorldTop = m_WorldLeft = 0;
            // m_XFactor = m_YFactor = 1;

            if (path == string.Empty)
            {
                // Nothing to extract
                return false;
            }

            try
            {
                string[] splitStrings = path.Split(charSeparators3);
                if (splitStrings.Length > 4)
                {
                    // use world coordinates from Google Maps
                    m_WorldLeft = Convert.ToDouble(splitStrings[splitStrings.Length - 5], CultureInfo.InvariantCulture);
                    m_WorldTop = Convert.ToDouble(splitStrings[splitStrings.Length - 4], CultureInfo.InvariantCulture);
                    m_XFactor = Convert.ToDouble(splitStrings[splitStrings.Length - 3], CultureInfo.InvariantCulture);
                    m_YFactor = Convert.ToDouble(splitStrings[splitStrings.Length - 2], CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }
            return true;
        }
        */

        /// <summary>
        /// Extracts the coordinates of an image from the appropriate XML file.
        /// Positions the image according to the availability of world coordinates.
        /// </summary>
        /// <param name="path">The image file path.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private CheckResult ExtractCoordinatesFromXmlFile(string path)
        {
            try
            {
                double valueLong = 0;
                double valueLat = 0;
                byte byteValue = 0;
                string str = string.Empty;
                string xmlString = string.Empty;

                // Remove file name extension if any
                int index2 = path.LastIndexOf(".");
                if (index2 > 0)
                    path = path.Substring(0, index2);
                // Add .xml extension to file name
                path += StrImageXmlFileExtension;

                // Read complete text file
                using (TextReader streamReader = new StreamReader(path))
                    xmlString = streamReader.ReadToEnd();

                // Read Identifier and display text, if present
                if (GrabString(out str, xmlString, "<Name>", "</Name>"))
                    textBoxIdentifier.Text = str;
                if (GrabString(out str, xmlString, "<Description>", "</Description>"))
                    textBoxSampleText.Text = str;
                // Read transparency
                if (m_SampleImage != null && GrabByte(out byteValue, xmlString, "<Transparency>", "</Transparency>"))
                    m_SampleImage.Transparency = byteValue;
                // use world coordinates from Google Maps
                if (!GrabDouble(out valueLong, xmlString, "<NWLong>", "</NWLong>"))
                    return CheckResult.Failure;
                if (!GrabDouble(out valueLat, xmlString, "<NWLat>", "</NWLat>"))
                    return CheckResult.Failure;
                Point topLeft = new Point(valueLong, valueLat);
                if (!GrabDouble(out valueLong, xmlString, "<SELong>", "</SELong>"))
                    return CheckResult.Failure;
                if (!GrabDouble(out valueLat, xmlString, "<SELat>", "</SELat>"))
                    return CheckResult.Failure;
                Point bottomRight = new Point(valueLong, valueLat);
                if (!GrabDouble(out valueLong, xmlString, "<SWLong>", "</SWLong>"))
                    return CheckResult.Failure;
                if (!GrabDouble(out valueLat, xmlString, "<SWLat>", "</SWLat>"))
                    return CheckResult.Failure;
                Point bottomLeft = new Point(valueLong, valueLat);
                if (!GrabDouble(out valueLong, xmlString, "<NELong>", "</NELong>"))
                    return CheckResult.Failure;
                if (!GrabDouble(out valueLat, xmlString, "<NELat>", "</NELat>"))
                    return CheckResult.Failure;
                Point topRight = new Point(valueLong, valueLat);

                if (m_Testcase)
                    m_TestcaseDescription = textBoxSampleText.Text;

                // Set world coordinates or adapt image to present world
                return CheckWorldPosition(topLeft, bottomRight, bottomLeft, topRight);
            }
            catch (FileNotFoundException)
            {
                return CheckResult.Failure;
                // return CheckResult.OK;
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return CheckResult.Failure;
            }
        }

        /// <summary>
        /// Checks if world coordinates are already available and positions the current image within the reference map.
        /// Prints a warning if the image coordinates are completely out of the reference map area.
        /// Sets image as new reference map if none is currently available and sets the new world coordinates, 
        /// if the image is straight north aligned. Otherwise prints an error.
        /// </summary>
        /// <param name="topLeft">The top left image coordinate.</param>
        /// <param name="bottomRight">The bottom right image coordinate.</param>
        /// <param name="bottomLeft">The bottom left image coordinate.</param>
        /// <param name="topRight">The top right image coordinate.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private CheckResult CheckWorldPosition(Point topLeft, Point bottomRight, Point bottomLeft, Point topRight)
        {
            // Check if there are already world coordinates present
            if (m_SampleImage != null && m_WorldTop != 0)
            {
                // Check if image is inside the reference map area
                if (topLeft.X > m_WorldRight || topLeft.Y < m_WorldBottom ||
                    bottomRight.X < m_WorldLeft || bottomRight.Y > m_WorldTop)
                {
                    // Check if the image corners hit another image or shape
                    if (m_Canvas.InputHitTest(WorldToCanvas(topLeft)) == null &&
                        m_Canvas.InputHitTest(WorldToCanvas(bottomRight)) == null &&
                        m_Canvas.InputHitTest(WorldToCanvas(bottomLeft)) == null &&
                        m_Canvas.InputHitTest(WorldToCanvas(topRight)) == null)
                    {
                        // Isolated image, print a warning
                        // SetError(WpfSamplingPlotPage.Properties.Resources.ErrCoordsNotVisible);
                    }
                }
                // Place image within existing World
                m_SampleImage.ChangePosition(WorldToCanvas(topLeft), 0);
                m_SampleImage.ChangePosition(WorldToCanvas(bottomRight), 1);
                m_SampleImage.ChangePosition(WorldToCanvas(bottomLeft), 2);
                // m_SampleImage.ChangePosition(WorldToCanvas(topRight), 3);
                m_SampleImage.Tag = tagRef;

                // COMMENTED OUT because problems when reference map is not north aligned!! (e.g. TK25 image)
                /* Remove artificial background map again
                if (m_BlankMap != null)
                {
                    m_SampleList.Remove(m_BlankMap);
                    // Reset world coordinates if no more reference map
                    RemoveSampleControls(m_BlankMap);
                    m_RefMap = m_SampleImage;
                    // ReadjustToggleButtons();
                    m_BlankMap = null;
                }
                */
            }
            else
            {
                // Check if map is straight north aligned
                if (topLeft.Y != topRight.Y || bottomLeft.Y != bottomRight.Y ||
                     topLeft.X != bottomLeft.X || topRight.X != bottomRight.X)
                {
                    // SetError(WpfSamplingPlotPage.Properties.Resources.ErrNotSuitableAsReferenceMap);
                    // SetError(WpfSamplingPlotPage.Properties.Resources.ErrCreatingReferenceMap);
                    // Create a background map which fits to the not strictly north aligned image to load
                    Point northWest = new Point(
                        Math.Min(topLeft.X, Math.Min(bottomRight.X, Math.Min(bottomLeft.X, topRight.X))),
                        Math.Max(topLeft.Y, Math.Max(bottomRight.Y, Math.Max(bottomLeft.Y, topRight.Y))));
                    Point southEast = new Point(
                        Math.Max(topLeft.X, Math.Max(bottomRight.X, Math.Max(bottomLeft.X, topRight.X))),
                        Math.Min(topLeft.Y, Math.Min(bottomRight.Y, Math.Min(bottomLeft.Y, topRight.Y))));

                    // Calculate distances of the map in north/south and west/east
                    Point southWest = new Point(northWest.X, southEast.Y);
                    // Point northEast = new Point(northWest.X, southEast.Y);
                    double distWestEast = Distance(southWest, southEast);
                    double distNorthSouth = Distance(northWest, southWest);
                    // Adjust width and height of the artificial background map to this relations
                    int width = (int)m_SampleImage.m_Image.Source.Width;
                    int height = (int)((m_SampleImage.m_Image.Source.Width * distNorthSouth) / distWestEast);
                    // Create artificial background map
                    if (CreateBackgroundMapWithCoordinates(width, height, northWest, southEast))
                        // Return reload code!
                        return CheckResult.Unaligned;
                    else
                        return CheckResult.Failure;
                }


                // Change World coordinates
                m_WorldLeft = topLeft.X;
                m_WorldTop = topLeft.Y;
                m_WorldRight = bottomRight.X;
                m_WorldBottom = bottomRight.Y;
                if (m_SampleImage != null)
                {
                    // Calculate factor in respect of the 180° border!
                    double worldLonDist = m_WorldRight - m_WorldLeft;
                    if (m_WorldRight < m_WorldLeft)
                        worldLonDist += 360;
                    m_XFactor = worldLonDist / (m_SampleImage.m_Image.Source.Width);
                    m_YFactor = (m_WorldBottom - m_WorldTop) / (m_SampleImage.m_Image.Source.Height);
                    m_SampleImage.Tag = tagRef;
                }

                if (m_LatLonWarning && (m_WorldTop > MaxLatitudeWarning || m_WorldBottom < -MaxLatitudeWarning))
                    SetError(string.Format(WpfSamplingPlotPage.Properties.Resources.ErrLatitude, MaxLatitudeWarning.ToString()));
            }
            return CheckResult.OK;
        }

        /// <summary>
        /// Extracts a double value from XML string.
        /// </summary>
        /// <param name="value">The output double value.</param>
        /// <param name="xmlString">The input XML string.</param>
        /// <param name="sep1">The XML start tag of the value.</param>
        /// <param name="sep2">The XML end tag of the value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool GrabDouble(out double value, string xmlString, string sep1, string sep2)
        {
            value = 0;

            try
            {
                int index1 = xmlString.IndexOf(sep1);
                int index2 = xmlString.IndexOf(sep2);
                if (index1 < 0 || index2 < 0 || index2 < (index1 + sep1.Length))
                    return false;
                value = Convert.ToDouble(xmlString.Substring(index1 + sep1.Length, index2 - (index1 + sep1.Length)), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Extracts a byte value from XML string.
        /// </summary>
        /// <param name="value">The output byte value.</param>
        /// <param name="xmlString">The input XML string.</param>
        /// <param name="sep1">The XML start tag of the value.</param>
        /// <param name="sep2">The XML end tag of the value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool GrabByte(out byte value, string xmlString, string sep1, string sep2)
        {
            value = 0;

            try
            {
                int index1 = xmlString.IndexOf(sep1);
                int index2 = xmlString.IndexOf(sep2);
                if (index1 < 0 || index2 < 0 || index2 < (index1 + sep1.Length))
                    return false;
                value = Convert.ToByte(xmlString.Substring(index1 + sep1.Length, index2 - (index1 + sep1.Length)));
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Extracts a substring value from an entire XML string.
        /// </summary>
        /// <param name="value">The output string value.</param>
        /// <param name="xmlString">The input XML string.</param>
        /// <param name="sep1">The XML start tag of the value.</param>
        /// <param name="sep2">The XML end tag of the value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool GrabString(out string value, string xmlString, string sep1, string sep2)
        {
            value = string.Empty;

            try
            {
                int index1 = xmlString.IndexOf(sep1);
                int index2 = xmlString.IndexOf(sep2);
                if (index1 < 0 || index2 < 0 || index2 < (index1 + sep1.Length))
                    return false;
                value = xmlString.Substring(index1 + sep1.Length, index2 - (index1 + sep1.Length));
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Calculate the current Google maps position, zoom level and map type from the values, 
        /// which are written by the webbrowser to the clipboard.
        /// </summary>
        public void UpdateWebBrowserUriByPos()
        {
            // WLine("UpdateWebBrowserUriByPos()");
            try
            {
                // Read coordinates from clipboard
                string coords = ReadCoordinatesFromClipboard();
                // WLine("----- " + coords);

                if (coords != null)
                {
                    // Split coords
                    string[] splitStrings = coords.Split(charSeparators4, StringSplitOptions.RemoveEmptyEntries);
                    if (splitStrings.Length >= 4)
                    {
                        // Use world coordinates from Google Maps
                        double worldLeft = Convert.ToDouble(splitStrings[1], CultureInfo.InvariantCulture);
                        double worldTop = Convert.ToDouble(splitStrings[2], CultureInfo.InvariantCulture);
                        double worldRight = Convert.ToDouble(splitStrings[3], CultureInfo.InvariantCulture);
                        double worldBottom = Convert.ToDouble(splitStrings[0], CultureInfo.InvariantCulture);
                        m_WebBrowserPosLon = worldLeft + ((worldRight - worldLeft) / 2);
                        m_WebBrowserPosLat = worldTop + ((worldBottom - worldTop) / 2);
                        if (splitStrings.Length >= 5)
                            m_WebZoomlevel = Convert.ToInt32(splitStrings[4], CultureInfo.InvariantCulture);
                        if (splitStrings.Length >= 6)
                            m_WebMapType = Convert.ToInt32(splitStrings[5], CultureInfo.InvariantCulture);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Extracts the map coordinates from the input string.
        /// Sets new world coordinates or positions the map within an existing reference map.
        /// </summary>
        /// <param name="coords">The input coordinates string.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        private bool ExtractCoordinatesFromWeb(string coords)
        {
            try
            {
                string[] splitStrings = coords.Split(charSeparators4, StringSplitOptions.RemoveEmptyEntries);
                // SetError(coords + " - Length = " + splitStrings.Length.ToString());
                if (splitStrings.Length >= 4)
                {
                    // Use world coordinates from Google Maps
                    double worldLeft = Convert.ToDouble(splitStrings[1], CultureInfo.InvariantCulture);
                    double worldTop = Convert.ToDouble(splitStrings[2], CultureInfo.InvariantCulture);
                    double worldRight = Convert.ToDouble(splitStrings[3], CultureInfo.InvariantCulture);
                    double worldBottom = Convert.ToDouble(splitStrings[0], CultureInfo.InvariantCulture);

                    // Set accuracy in respect of the 180° border
                    // (necessary here to correct the top-left coords due to the web browser window border)
                    double worldLonDist = worldRight - worldLeft;
                    if (worldLonDist < 0)
                        worldLonDist += 360;
                    double xFactor = worldLonDist / (m_WebBrowser.Width - m_WebBrowserWidthDiff);
                    double yFactor = (worldBottom - worldTop) / (m_WebBrowser.Height - WebBrowserHeightDiff);
                    m_WebBrowserPosLon = worldLeft + ((worldRight - worldLeft) / 2);
                    m_WebBrowserPosLat = worldTop + ((worldBottom - worldTop) / 2);
                    // Add left and top border of grabbed frame to coordinates!!
                    worldLeft -= WebBrowserBorder * xFactor;
                    worldTop -= WebBrowserBorder * yFactor;

                    // Set world coordinates or adapt image to present world
                    CheckWorldPosition(new Point(worldLeft, worldTop), new Point(worldRight, worldBottom), new Point(worldLeft, worldBottom), new Point(worldRight, worldTop));

                    // Check Hemispere *****
                    if (m_WorldRight == 180 && m_WorldLeft == -180)
                        SetError(WpfSamplingPlotPage.Properties.Resources.ErrInvalidWorld);

                    // if (m_WorldRight < m_WorldLeft)
                    //     SetError(WpfSamplingPlotPage.Properties.Resources.ErrLongitude);
                    if (m_LatLonWarning && (m_WorldTop > MaxLatitudeWarning || m_WorldBottom < -MaxLatitudeWarning))
                        SetError(string.Format(WpfSamplingPlotPage.Properties.Resources.ErrLatitude, MaxLatitudeWarning.ToString()));

                    if (splitStrings.Length >= 5)
                        m_WebZoomlevel = Convert.ToInt32(splitStrings[4], CultureInfo.InvariantCulture);
                    if (splitStrings.Length >= 6)
                        m_WebMapType = Convert.ToInt32(splitStrings[5], CultureInfo.InvariantCulture);

                    /* Test only: Show Google Maps projection by 10 equidistant pins from WorldTop to WorldBottom
                    m_WebBrowserPoints = null;
                    m_WebBrowserPoints = new PointCollection();
                    double mid = m_WorldLeft + (m_WorldRight - m_WorldLeft) / 2;
                    double step = (m_WorldTop - m_WorldBottom) / 10;
                    for (double d = m_WorldTop; d >= m_WorldBottom; d -= step)
                    {
                        m_WebBrowserPoints.Add(new Point(mid, d));
                    }
                    */
                    return true;
                }
                else
                {
                    SetError(coords + " - " + WpfSamplingPlotPage.Properties.Resources.ErrNoWorldCoords);
                    return false;
                }
            }
            catch
            {
                SetError(coords + " - " + WpfSamplingPlotPage.Properties.Resources.ErrNoWorldCoords);
                return false;
            }
        }

        // Return a string with coordinates to be used as a filename (-- no longer used --).
        private string GetImageCoords(SampleImage image)
        {
            PointCollection samplePoints = CanvasToWorld(image.SamplePointCollection);
            if (samplePoints.Count > 1)
                return ("_" +
                    samplePoints[0].X.ToString("F8", CultureInfo.InvariantCulture) + "_" +
                    samplePoints[0].Y.ToString("F8", CultureInfo.InvariantCulture) + "_" +
                    samplePoints[1].X.ToString("F8", CultureInfo.InvariantCulture) + "_" +
                    samplePoints[1].Y.ToString("F8", CultureInfo.InvariantCulture) + "_");
            else
                return string.Empty;
        }

        /// <summary>
        /// Clears the world coordinates.
        /// </summary>
        private void ClearCoordinates()
        {
            m_WorldLeft = 0;
            m_WorldTop = 0;
            m_WorldRight = 0;
            m_WorldBottom = 0;
            m_XFactor = 1;
            m_YFactor = 1;
            StatusPos(m_Zero);
        }

        #endregion //  Load and save images

        #region Create shapes

        /// <summary>
        /// Creates a shape from a given Geoobjects input string from ASCII file (in SQL notation).
        /// </summary>
        /// <param name="shapeString">The Geoobjects input string.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool CreateShapeFromString(string shapeString)
        {
            bool retVal = true;

            try
            {
                // Split up Shape string
                string[] splitStrings = shapeString.Split(stringSeparators1, StringSplitOptions.None);
                // If header is missing, return failure
                if (splitStrings.Length < SampleEntryLength)
                    return false;
                int index = 0;

                List<GeoObject> geoObjectList = new List<GeoObject>();

                // Parse all sample entries (each <SampleEntryLength> lines)
                while (splitStrings.Length >= index + SampleEntryLength)
                {
                    try
                    {
                        // Get GeoObject structure out of string array
                        GeoObject geoObject = new GeoObject();
                        SetupGeoObject(out geoObject, splitStrings, ref index);
                        geoObjectList.Add(geoObject);
                    }
                    catch
                    {
                    }
                }

                // Init Progressbar
                ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrLoadingShapesInstruction, geoObjectList.Count);

                if (WorldHasDisappeared())
                {
                    WpfSetMapAndGeoObjects(geoObjectList);
                    WpfAddSample();
                }
                else
                {
                    foreach (GeoObject geoObject in geoObjectList)
                    {
                        // Create sample out of GeoObject structure
                        CreateShapeFromGeoObject(geoObject);

                        // Set ID and Description in control panel
                        SetIdentifierText(geoObject.Identifier, geoObject.DisplayText);
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
                retVal = false;
            }
            finally
            {
                ProgressBarClose();
            }
            return retVal;
        }

        /// <summary>
        /// Creates a shape from a given Geoobjects instance.
        /// Checks the shape type and sets up the current Sample object.
        /// Adds it to the sample list and the working area.
        /// </summary>
        /// <param name="geoObject">The input Geoobjects instance.</param>
        private void CreateShapeFromGeoObject(GeoObject geoObject)
        {
            List<string> objectDataStrings = new List<string>();

            // Set ID and Description in control panel
            SetIdentifierText(geoObject.Identifier, geoObject.DisplayText);

            // If GeoObject is GEOMETRYCOLLECTION, split it up int single objects data strings
            if (geoObject.GeometryData.Contains("GEOMETRYCOLLECTION"))
                objectDataStrings = SplitUpGeometryCollection(geoObject.GeometryData);
            else
                objectDataStrings.Add(geoObject.GeometryData);

            // Show progress on loading GeoObjects
            string labelInstructionText = labelInstruction.Content.ToString();
            SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrLoadingShapesInstruction, true);
            DateTime start = DateTime.Now;

            // ======= Disable DoEvents!!! =======
            // DoEvents();

            foreach (string str in objectDataStrings)
            {
                // Split GeometryData string into sample type and point collections
                string[] splitDataString = str.Split(charSeparators5, StringSplitOptions.RemoveEmptyEntries);

                List<PointCollection> PointCollectionList = new List<PointCollection>();
                for (int i = 1; i < splitDataString.Length; i++)
                {
                    PointCollection pointCollection = ParsePointCollection(splitDataString[i]);
                    if (pointCollection.Count > 0)
                        PointCollectionList.Add(pointCollection);
                }

                // Check for empy string
                if (splitDataString.Length <= 0)
                    continue;

                // Switch accoding to sample type
                switch (splitDataString[0].TrimEnd(TrimChars))
                {
                    case "POLYGON":
                    case "MULTIPOLYGON":
                        // Create sample area object
                        m_SamplePolygon = new SamplePolygon(m_Canvas);
                        // Set Brushes, Transparency, Thickness
                        m_SamplePolygon.StrokeTransparency = geoObject.StrokeTransparency;
                        m_SamplePolygon.FillTransparency = geoObject.FillTransparency;
                        m_SamplePolygon.StrokeThickness = geoObject.StrokeThickness;
                        m_SamplePolygon.StrokeBrush = (SolidColorBrush)geoObject.StrokeBrush;
                        m_SamplePolygon.FillBrush = (SolidColorBrush)geoObject.FillBrush;

                        // Fill it with points
                        foreach (PointCollection points in PointCollectionList)
                        {
                            m_SamplePolygon.SetSamplePoint(WorldToCanvas(points));
                            m_SamplePolygon.AddSample();
                            if (points != PointCollectionList[PointCollectionList.Count - 1])
                                m_SamplePolygon.NewSample();
                        }

                        // Add identifier and display text plus tooltip for all polygons
                        m_SamplePolygon.AddId(geoObject.Identifier, geoObject.DisplayText);
                        // Save it to list
                        SaveSample(m_SamplePolygon);
                        // Add a Toggle Button to switch the shape on and off
                        // COMMENTED OUT if artificial background map has been removed (made problems)
                        AddToggleButton(m_SamplePolygon);
                        // Invalidate object
                        m_SamplePolygon = null;
                        break;

                    case "LINESTRING":
                    case "MULTILINESTRING":
                        // Create sample line object
                        m_SampleLine = new SampleLines(m_Canvas);
                        // Set Brushes, Transparency, Thickness
                        m_SampleLine.StrokeTransparency = geoObject.StrokeTransparency;
                        m_SampleLine.StrokeThickness = geoObject.StrokeThickness;
                        m_SampleLine.StrokeBrush = (SolidColorBrush)geoObject.StrokeBrush;

                        // Fill it with points
                        foreach (PointCollection points in PointCollectionList)
                        {
                            m_SampleLine.SetSamplePoint(WorldToCanvas(points));
                            m_SampleLine.AddSample();
                            if (points != PointCollectionList[PointCollectionList.Count - 1])
                                m_SampleLine.NewSample();
                        }

                        // Add identifier and display text plus tooltip for all polygons
                        m_SampleLine.AddId(geoObject.Identifier, geoObject.DisplayText);
                        // Save it to list
                        SaveSample(m_SampleLine);
                        // Add a Toggle Button to switch the shape on and off
                        AddToggleButton(m_SampleLine);
                        // Invalidate object
                        m_SampleLine = null;
                        break;

                    case "POINT":
                    case "MULTIPOINT":
                        // Create sample points object
                        m_SamplePoints = new SamplePoints(m_Canvas);
                        // Set Brushes, Transparency, Thickness
                        m_SamplePoints.StrokeTransparency = geoObject.StrokeTransparency;
                        m_SamplePoints.FillTransparency = geoObject.FillTransparency;
                        m_SamplePoints.StrokeThickness = geoObject.StrokeThickness;
                        m_SamplePoints.StrokeBrush = (SolidColorBrush)geoObject.StrokeBrush;
                        m_SamplePoints.FillBrush = (SolidColorBrush)geoObject.FillBrush;
                        m_SamplePoints.SamplePointSymbol = (geoObject.PointType == PointSymbol.None) ? m_PointSymbol : geoObject.PointType;
                        m_SamplePoints.SamplePointSymbolSize = (geoObject.PointSymbolSize == 0) ? m_PointSymbolSize : geoObject.PointSymbolSize;

                        // Fill it with points
                        foreach (PointCollection points in PointCollectionList)
                            m_SamplePoints.SetSamplePoint(WorldToCanvas(points));
                        // Add Sample from Points
                        m_SamplePoints.AddSample();
                        // Add identifier and display text plus tooltip for all polygons
                        m_SamplePoints.AddId(geoObject.Identifier, geoObject.DisplayText);
                        // Save it to list
                        SaveSample(m_SamplePoints);
                        // Add a Toggle Button to switch the shape on and off
                        AddToggleButton(m_SamplePoints);
                        // Invalidate object
                        m_SamplePoints = null;
                        break;

                    default:
                        break;
                }

                if (stackPanelProgress.Visibility == System.Windows.Visibility.Visible)
                {
                    m_ProgressCount++;
                    ProgressBarUpdate(m_ProgressCount);
                }

                // Show progress each second
                else if (DateTime.Now - start > TimeSpan.FromSeconds(1))
                {
                    start = DateTime.Now;
                    DoEvents();
                }
            }
            // Restore Instruction label
            SetInstructionLabel(labelInstructionText, false);
        }

        /// <summary>
        /// Splits up a GEOMETRYCOLLECTION string into single Geoobject sub string.
        /// </summary>
        /// <param name="dataString">The input data string.</param>
        /// <returns>List of sub strings containing the single Geoobject strings.</returns>
        private List<string> SplitUpGeometryCollection(string dataString)
        {
            // string[] objectDataStrings = new string[20];
            List<string> objectDataStrings = new List<string>();

            // Find entry point of object search (behind GEOMETRYCOLLECTION key word)
            int indexMid = dataString.IndexOf('(');
            if (indexMid < 0)
                return objectDataStrings;
            // Get start index of first object
            int indexNext = dataString.IndexOfAny(charSeparators7, indexMid);
            if (indexNext < 0)
                return objectDataStrings;
            // Save start index for next object search
            int indexStart = indexNext;
            // Find entry point for next object search (behind object keyword)
            if ((indexMid = dataString.IndexOf('(', indexStart)) < 0)
                return objectDataStrings;

            // Find start index of next object
            while ((indexNext = dataString.IndexOfAny(charSeparators7, indexMid)) > -1)
            {
                // Add previous object to list
                objectDataStrings.Add(dataString.Substring(indexStart, indexNext - indexStart));
                // Save start index for next object search
                indexStart = indexNext;
                // Find entry point for next object search (behind object keyword)
                if ((indexMid = dataString.IndexOf('(', indexStart)) < 0)
                    return objectDataStrings;
            }
            // Add last object to list
            objectDataStrings.Add(dataString.Substring(indexStart));

            return objectDataStrings;
        }

        /// <summary>
        /// Parses a points string to create PointCollection.
        /// </summary>
        /// <param name="collectionString">The input points string.</param>
        /// <returns>The output PointCollection.</returns>
        private PointCollection ParsePointCollection(string collectionString)
        {
            PointCollection pointCollection = new PointCollection();
            string[] splitCollectionString = collectionString.Split(charSeparators2, StringSplitOptions.None);
            foreach (string str in splitCollectionString)
            {
                string[] splitPointString = str.Split(charSeparators6, StringSplitOptions.RemoveEmptyEntries);
                if (splitPointString.Length >= 2)
                {
                    pointCollection.Add(new Point(Convert.ToDouble(splitPointString[0], CultureInfo.InvariantCulture),
                                                  Convert.ToDouble(splitPointString[1], CultureInfo.InvariantCulture)));
                }
            }
            return pointCollection;
        }

        #endregion // Create shapes

        #region Point conversion

        // Convert screen window position to Longitude/Latitude coordinates (e.g. for Google)
        // Attention: The dpi settings of the screen has to be calculated to guarantee the correct conversion,
        // because WPF uses device independent window coordinates! (Works correct if dpi are set to standard value 96)
        // Convert single point

        /// <summary>
        /// Converts PI to angle.
        /// </summary>
        /// <param name="value">The input value in PI notation.</param>
        /// <returns>The output angle value.</returns>
        private double PiToAngle(double value)
        {
            return (value * 180 / Math.PI);
        }

        /// <summary>
        /// Converts angle to PI.
        /// </summary>
        /// <param name="value">The input angle value.</param>
        /// <returns>The output value in PI notation.</returns>
        private double AngleToPi(double value)
        {
            return (value * Math.PI / 180);
        }

        /// <summary>
        /// Converts mercator projection map Y-value to latitude.
        /// </summary>
        /// <param name="linearLat">The linear mercator projection map input value.</param>
        /// <returns>The non linear output latitude.</returns>
        private double YtoLatitude(double linearLat)
        {
            return PiToAngle(Math.Atan(Math.Sinh(AngleToPi(linearLat))));
        }

        /// <summary>
        /// Converts latitude to mercator projection map Y-value.
        /// </summary>
        /// <param name="googleLat">The non linear input latitude value.</param>
        /// <returns>The linear output mercator projection Y-value</returns>
        private double LatitudeToY(double googleLat)
        {
            return PiToAngle(Math.Log((1 + Math.Sin(AngleToPi(googleLat))) / (1 - Math.Sin(AngleToPi(googleLat)))) / 2);
        }

        /// <summary>
        /// Convert latitude from linear range to mercator projection range.
        /// </summary>
        /// <param name="linearLat">The linear latitude input value.</param>
        /// <returns>The converted non-linear mercator projection output value.</returns>
        private double LinearToMercatorConversion(double linearLat)
        {
            // Blow up linear vertical world to non linear mercator projection dimension (still linear!)
            double wTop = LatitudeToY(m_WorldTop);
            double wBot = LatitudeToY(m_WorldBottom);
            // Calculate factor for linear input coordinate to adapt to the new bigger area
            double factor = (wTop - wBot) / (m_WorldTop - m_WorldBottom);
            // Blow up linear input coordinate by factor
            linearLat = wBot + (linearLat - m_WorldBottom) * factor;
            // Convert blown up linear input coordinate to adapt to original sized smaller world,
            // which is now non-linear mercator projection
            double googleLat = YtoLatitude(linearLat);
            // Return converted value
            return googleLat;
        }

        /// <summary>
        /// Convert latitude from mercator projection range to linear range.
        /// </summary>
        /// <param name="googleLat">The non-linear mercator projection input value.</param>
        /// <returns>The converted linear latitude output value.</returns>
        private double MercatorToLinearConversion(double googleLat)
        {
            // Convert top and bottom world coordinates to Mercator projection map coords (bigger range)
            double wTop = LatitudeToY(m_WorldTop);
            double wBot = LatitudeToY(m_WorldBottom);
            // Calculate factor to shrink new bigger projection area to adapt to original area
            double factor = (m_WorldTop - m_WorldBottom) / (wTop - wBot);
            // Calculate current latitude to Mercator projection map coords
            double linearLat = LatitudeToY(googleLat);
            // Shrink and shift down new (linear) map coordinate to fit into original area
            double linDist = (linearLat - wBot) * factor;
            linearLat = m_WorldBottom + linDist;
            // Return converted value
            return linearLat;
        }

        /// <summary>
        /// Ensures that the longitude is corrected when crossing the 180° border.
        /// Needs to be called when converting from screen to world coordinates.
        /// </summary>
        /// <param name="inLon">The input longitude.</param>
        /// <returns>The (corrected) output longitude.</returns>
        private double LongitudeCorrection(double inLon)
        {
            double outLon = inLon;
            if (inLon > 180)
                outLon -= 360;
            else if (inLon < -180)
                outLon += 360;
            return outLon;
        }

        // Ensure that longitude is extended when crossing the 180° border
        /// <summary>
        /// Ensures that longitude is extended when crossing the 180° border.
        /// Needs to be called when converting from world to screen coordinates.
        /// </summary>
        /// <param name="inLon">The input longitude.</param>
        /// <returns>The (extended) output longitude.</returns>
        private double LongitudeExtension(double inLon)
        {
            double outLon = inLon;
            if (m_WorldRight < m_WorldLeft && inLon - m_WorldLeft < 0)
                outLon += 360;
            return outLon;
        }

        /// <summary>
        /// Converts a single point from screen to world coordinates.
        /// </summary>
        /// <param name="inPoint">The input point.</param>
        /// <returns>The converted output point.</returns>
        private Point CanvasToWorld(Point inPoint)
        {
            // Calculate linear latitude range according to screen coordinates range
            Point pnt = new Point(m_WorldLeft + (inPoint.X * m_XFactor), m_WorldTop + (inPoint.Y * m_YFactor));
            // Calculate correct latitude due to non-linear mercator projection
            pnt.Y = LinearToMercatorConversion(pnt.Y);
            // Calculate correct longitude due to 180° border
            pnt.X = LongitudeCorrection(pnt.X);

            return pnt;
        }

        /// <summary>
        /// Converts a collection of points from screen to world coordinates.
        /// </summary>
        /// <param name="inPoints">The input point collection.</param>
        /// <returns>The converted output point collection.</returns>
        private PointCollection CanvasToWorld(PointCollection inPoints)
        {
            PointCollection outPoints = new PointCollection();
            foreach (Point pnt in inPoints)
            {
                outPoints.Add(CanvasToWorld(pnt));
            }
            return outPoints;
        }

        // Convert Longitude/Latitude coordinates (e.g. from Google) to screen (Canvas) position
        // Attention: The dpi settings of the screen has to be calculated to guarantee the correct conversion,
        // because WPF uses device independent window coordinates! (Works correct if dpi are set to standard value 96)

        /// <summary>
        /// Converts a single point from world to screen coordinates.
        /// </summary>
        /// <param name="inPoint">The input point.</param>
        /// <returns>The converted output point.</returns>
        private Point WorldToCanvas(Point inPoint)
        {
            inPoint.Y = MercatorToLinearConversion(inPoint.Y);
            // Adjust longitude in respect to the 180° border
            inPoint.X = LongitudeExtension(inPoint.X);

            Point pnt = new Point((inPoint.X - m_WorldLeft) / m_XFactor, (inPoint.Y - m_WorldTop) / m_YFactor);
            return pnt;
        }

        /// <summary>
        /// Converts a collection of points from world to screen coordinates.
        /// </summary>
        /// <param name="inPoints">The input point collection.</param>
        /// <returns>The converted output point collection.</returns>
        private PointCollection WorldToCanvas(PointCollection inPoints)
        {
            PointCollection outPoints = new PointCollection();
            foreach (Point pnt in inPoints)
            {
                outPoints.Add(WorldToCanvas(pnt));
            }
            return outPoints;
        }

        #endregion // Point conversion

        #region Altitude

        /// <summary>
        /// Returns the altitude for the given world coordinates.
        /// </summary>
        /// <param name="longitude">The input longitude value.</param>
        /// <param name="latitude">The input latitude value.</param>
        /// <param name="altitude">The output altitude value.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool GetAltitude(double longitude, double latitude, out double altitude)
        {
            altitude = 0;

            try
            {
                string strAltitude = string.Empty;

                /* Google API call
                if ((strAltitude = WebAltitude(latitude, longitude)) == string.Empty)
                    return false;
                altitude = Convert.ToDouble(strAltitude, CultureInfo.InvariantCulture);
                */
                // ---- Geonames Server calls are again free of charge ---- !
                // Get more exact values if applicable
                if (latitude < 60 && latitude > -54)
                {
                    if ((strAltitude = WebAltitude1(GeoNamesOrgServices.srtm3, latitude, longitude)) == string.Empty)
                        return false;
                    altitude = Convert.ToDouble(strAltitude, CultureInfo.InvariantCulture);
                    if (altitude < -400)
                    {
                        if ((strAltitude = WebAltitude1(GeoNamesOrgServices.gtopo30, latitude, longitude)) == string.Empty)
                            return false;
                        altitude = Convert.ToDouble(strAltitude);
                    }
                }
                else
                {
                    if ((strAltitude = WebAltitude1(GeoNamesOrgServices.gtopo30, latitude, longitude)) == string.Empty)
                        return false;
                    altitude = Convert.ToDouble(strAltitude);
                }
                //


            }
            catch (Exception ex)
            {
                // Error message already in underlying method
                SetError(ex.ToString() + " - " + ex.Message);
                return false;
            }

            // Successful
            return true;
        }

        /// <summary>
        /// Requests the altitude for the given world coordinates from the web (http://maps.googleapis.com/maps/api/elevation).
        /// </summary>
        /// <param name="Latitude">The input latitude value.</param>
        /// <param name="Longitude">The input longitude value.</param>
        /// <returns>The output string containing the requested altitude value, if successful.</returns>
        public string WebAltitude(double Latitude, double Longitude)
        {
            string Response = "";
            //
            try
            {
                string URI = string.Format("http://maps.googleapis.com/maps/api/elevation/xml?locations={0},{1}&sensor=false",
                    Latitude.ToString().Replace(",", "."), Longitude.ToString().Replace(",", "."));
                System.Net.WebRequest myWebRequest = System.Net.WebRequest.Create(URI);
                System.Net.WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);
                Response = readStream.ReadToEnd(); ;
                readStream.Close();
                myWebResponse.Close();
                int ind1 = Response.IndexOf("<elevation>") + 11;
                int ind2 = Response.IndexOf("</elevation>");
                if (ind2 > ind1)
                    Response = Response.Substring(ind1, ind2 - ind1);
                else
                    Response = string.Empty;
            }
            catch // (Exception ex)
            {
                SetError(WpfSamplingPlotPage.Properties.Resources.ErrAltitude);
            }
            return Response;
        }

        /// <summary>
        /// Requests the altitude for the given world coordinates from the web (http://ws.geonames.org/).
        /// </summary>
        /// <param name="Service">The service which is used for the request.</param>
        /// <param name="Latitude">The input latitude value.</param>
        /// <param name="Longitude">The input longitude value.</param>
        /// <returns>The output string containing the requested altitude value, if successful.</returns>
        public string WebAltitude1(GeoNamesOrgServices Service, double Latitude, double Longitude)
        {
            if (Service != GeoNamesOrgServices.gtopo30
                && Service != GeoNamesOrgServices.srtm3)
                return "";

            string Response = "";
            try
            {
                string geoUri = global::WpfSamplingPlotPage.Properties.Settings.Default.GeonamesUrl;
                string username = global::WpfSamplingPlotPage.Properties.Settings.Default.GeonamesUsername;
                string URI = geoUri + Service.ToString() + "?lat=" + Latitude.ToString().Replace(",", ".") + "&lng=" + Longitude.ToString().Replace(",", ".")
                    + "&username=" + username + "&style=full";

                System.Net.WebRequest myWebRequest = System.Net.WebRequest.Create(URI);
                System.Net.WebResponse myWebResponse = myWebRequest.GetResponse();
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);
                Response = readStream.ReadToEnd(); ;
                readStream.Close();
                myWebResponse.Close();
                if (Response.Length > 1)
                    Response = Response.Substring(0, Response.Length - 2);
            }
            catch // (Exception ex)
            {
                SetError(WpfSamplingPlotPage.Properties.Resources.ErrAltitude);
            }

            return Response;
        }

        #endregion // Altitude

        #region Error handling

        /// <summary>
        /// Displays a message box with error text.
        /// </summary>
        /// <param name="text">The error text.</param>
        public void SetError(string text)
        {
            if (!m_SilentErrorMode)
                MessageBox.Show(text.Replace("\\r\\n", "\r\n"), WpfSamplingPlotPage.Properties.Resources.ErrCaption);
        }

        /// <summary>
        /// Displays a message box with question for user interaction.
        /// </summary>
        /// <param name="text">The question text.</param>
        /// <returns></returns>
        public bool SetRequest(string text)
        {
            bool retVal = (MessageBox.Show(text.Replace("\\r\\n", "\r\n"), WpfSamplingPlotPage.Properties.Resources.ErrCaption,
                MessageBoxButton.YesNo) == MessageBoxResult.Yes);
            return retVal;
        }

        #endregion // Error handling

        #region Interface to Windows Application

        List<GeoObject> m_WpfGeoObjectList = null;
        private System.Windows.Forms.Panel m_CanvasParent = null;

        // bool m_NavFlag = false;

        void m_Canvas_LayoutUpdated(object sender, EventArgs e)
        {
            /* not used
            if (m_NavFlag)
            {
                MessageBox.Show("Adding GeoObjects...");
                buttonAddShape_Click(null, null);
                m_NavFlag = false;
            }
            */
        }

        /// <summary>
        /// Parses a GeoObject data string for Longitude and Latitude coordinates and set minimum and maximum values.
        /// </summary>
        /// <param name="data">The input data string.</param>
        /// <param name="minCoord">The output minimum coordinate.</param>
        /// <param name="maxCoord">The output maximum coordinate.</param>
        /// <returns></returns>
        private int ParseGeoObjectCoords(string data, ref Point minCoord, ref Point maxCoord)
        {
            int count = 0;
            try
            {
                string[] splitStrings = data.Split(charSeparators8, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in splitStrings)
                {
                    string[] splitStr = str.Split(charSeparators6, StringSplitOptions.RemoveEmptyEntries);
                    if (splitStr.Length >= 2)
                    {
                        double xCoord = Convert.ToDouble(splitStr[0], CultureInfo.InvariantCulture);
                        double yCoord = Convert.ToDouble(splitStr[1], CultureInfo.InvariantCulture);
                        if (xCoord < minCoord.X)
                            minCoord.X = xCoord;
                        if (xCoord > maxCoord.X)
                            maxCoord.X = xCoord;
                        if (yCoord < minCoord.Y)
                            minCoord.Y = yCoord;
                        if (yCoord > maxCoord.Y)
                            maxCoord.Y = yCoord;
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
            return count;
        }

        /// <summary>
        /// Switches the GIS Editor mode.
        /// </summary>
        /// <param name="mode">1: MapServer-Mode, 2: ShiftCanvas-Mode</param>
        /// <returns></returns>
        public void WpfSetMode(int mode)
        {
            // WLine("WpfSetMode(int mode)");
            switch (mode)
            {
                case 1:
                    if (m_MouseMode != MouseMode.MapServer)
                    {
                        radioButtonMap.IsChecked = true;
                    }
                    break;
                case 2:
                    if (m_MouseMode != MouseMode.ShiftCanvas)
                    {
                        radioButtonShift.IsChecked = true;
                    }
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// Switches GIS Editor to MapServer-Mode and displays the input area.
        /// </summary>
        /// <param name="bottomLeft">Minimum coordinate (bottom left) of the map area.</param>
        /// <param name="topRight">Maximum coordinate (top right) of the map area.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfSetMap(Point bottomLeft, Point topRight)
        {
            // WLine("WpfSetMap(Point bottomLeft, Point topRight)");
            // Set Map mode
            WpfSetMode(1);

            // Call Webbrowser with found Coordinates
            if (m_WebBrowserPoints == null)
            {
                m_WebBrowserPoints = new PointCollection();
            }
            else
            {
                m_WebBrowserPoints.Clear();
            }
            m_WebBrowserPoints.Add(bottomLeft);
            m_WebBrowserPoints.Add(topRight);

            // m_NavFlag = true;
            m_WebScan = 1;

            // Show WebBrowser with given area
            WebBrowserAdapt();

            return true;
        }

        /// <summary>
        /// Switches GIS Editor to MapServer-Mode and sets the zoom level.
        /// </summary>
        /// <param name="centerCoord">Center coordinate of the map area.</param>
        /// <param name="zoomLevel">Google Maps zoom level to be set.</param>
        /// <returns><c>true</c> if successful, <c>false</c> if zoom level is out of range.</returns>
        public bool WpfSetMap(Point centerCoord, int zoomLevel)
        {
            // WLine("WpfSetMap(Point centerCoord, int zoomLevel)");
            // check for valid zoom level
            if (zoomLevel > 19 || zoomLevel < 3)
                return false;

            // Set Map mode
            WpfSetMode(1);

            // Call Webbrowser with center point and zoom level
            if (m_WebBrowserPoints != null)
            {
                m_WebBrowserPoints.Clear();
            }

            m_WebBrowserPosLat = centerCoord.Y;
            m_WebBrowserPosLon = centerCoord.X;
            m_WebZoomlevel = zoomLevel;

            // m_NavFlag = true;
            m_WebScan = 1;

            // Show WebBrowser with given area
            WebBrowserAdapt();

            return true;
        }

        /// <summary>
        /// Sends a GeoObject-List from Diversity application to be shown on the WPF panel working area.
        /// </summary>
        /// <param name="geoObjectList">List of Geoobjects.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfSetGeoObjects(List<GeoObject> geoObjectList)
        {
            // WLine("WpfSetGeoObjects(List<GeoObject> geoObjectList)");
            // Load imported GeoObjects
            if (geoObjectList == null)
                return false;

            // Create shapes from GeoObject list
            foreach (GeoObject geoObject in geoObjectList)
                CreateShapeFromGeoObject(ValidateGeoObject(geoObject));

            return true;
        }

        /// <summary>
        /// Validates a geo object.
        /// </summary>
        /// <param name="geoObject">The input geo object.</param>
        /// <returns>The validated geo object.</returns>
        private GeoObject ValidateGeoObject(GeoObject geoObject)
        {
            // Set default values, if application has failed to set the geoObject correctly
            if (geoObject.StrokeBrush == null)
                geoObject.StrokeBrush = Brushes.Red;
            if (geoObject.FillBrush == null)
                geoObject.StrokeBrush = Brushes.Red;
            if (geoObject.StrokeThickness == 0)
                geoObject.StrokeThickness = 1;
            if (geoObject.PointSymbolSize == 0)
                geoObject.PointSymbolSize = 1;

            return geoObject;
        }

        /// <summary>
        /// Sends a GeoObject-List from Diversity application to be shown on WPF panel, switches the
        /// GIS Editor to MapServer-Mode and displays the bounding area of the transmitted Geoobjects.
        /// </summary>
        /// <param name="geoObjectList">The input Geoobject list.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfSetMapAndGeoObjects(List<GeoObject> geoObjectList)
        {
            //  WLine("WpfSetMapAndGeoObjects(List<GeoObject> geoObjectList)");
            // Load imported GeoObjects
            if (geoObjectList == null)
                return false;

            // Set Map mode
            WpfSetMode(1);
            textBoxIdentifier.Text = "Map";
            textBoxSampleText.Text = "";

            Point minCoord = new Point(180, 90);
            Point maxCoord = new Point(-180, -90);

            m_WpfGeoObjectList = geoObjectList;
            // Extract Min and Max Coordinate from list
            if (geoObjectList.Count <= 0)
            {
                minCoord = new Point(90, 45);
                maxCoord = new Point(-90, -45);
            }
            else
            {
                foreach (GeoObject geoObject in geoObjectList)
                {
                    if (geoObject.GeometryData != null)
                    {
                        ParseGeoObjectCoords(geoObject.GeometryData, ref minCoord, ref maxCoord);
                        if (m_WorkingAreaSaveFileName == string.Empty)
                            m_WorkingAreaSaveFileName = geoObject.Identifier + " " + geoObject.DisplayText;
                    }
                }
            }

            // Call Webbrowser with found Coordinates
            if (m_WebBrowserPoints == null)
            {
                m_WebBrowserPoints = new PointCollection();
            }
            else
            {
                m_WebBrowserPoints.Clear();
            }
            //  WLine("::: " + minCoord.ToString() + " -- " + maxCoord.ToString() + " :::");
            m_WebBrowserPoints.Add(minCoord);
            m_WebBrowserPoints.Add(maxCoord);

            // m_NavFlag = true;
            m_WebScan = 1;

            // Set wait marker in clipboard
            // System.Windows.Forms.Clipboard.SetText(m_GooglePos);
            CallJavaScript();
            m_WebView2Coordinates = m_GooglePos;
            // Show WebBrowser with given area
            WebBrowserAdapt();

            return true;
        }

        /// <summary>
        /// Sends a GeoObject-List from Diversity application to be shown on WPF panel, switches the
        /// GIS Editor to MapServer-Mode and displays the bounding area of the transmitted Geoobjects.
        /// </summary>
        /// <param name="geoObjectList">The input Geoobject list.</param>
        /// <param name="mapPath">The path of the map to be loaded.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfSetMapAndGeoObjects(List<GeoObject> geoObjectList, string mapPath)
        {
            //  WLine("WpfSetMapAndGeoObjects(List<GeoObject> geoObjectList)");
            // Load imported GeoObjects

            if (geoObjectList == null || mapPath == string.Empty)
                return false;

            m_WpfGeoObjectList = geoObjectList;

            // Set Map mode
            WpfSetMode(2);
            textBoxIdentifier.Text = "Map";
            textBoxSampleText.Text = mapPath;

            if (m_WorkingAreaSaveFileName == string.Empty)
                m_WorkingAreaSaveFileName = "MyMap";

            if (isSupportedImageFile(mapPath))
            {
                if (LoadSingleImage(mapPath, m_XPos, 0, 255))
                    radioButtonShift.IsChecked = true;
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sends a GeoObject-List from Diversity application to be shown on WPF panel, switches the
        /// GIS Editor to MapServer-Mode and displays the corresponding area (center point) with the given zoom level.
        /// </summary>
        /// <param name="geoObjectList">The input Geoobject list.</param>
        /// <param name="zoomLevel">Google Maps zoom level.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfSetMapAndGeoObjects(List<GeoObject> geoObjectList, int zoomLevel)
        {
            //  WLine("WpfSetMapAndGeoObjects(List<GeoObject> geoObjectList, int zoomLevel)");
            // Set Map mode
            WpfSetMode(1);
            textBoxIdentifier.Text = "Map";
            textBoxSampleText.Text = "";

            Point minCoord = new Point(180, 90);
            Point maxCoord = new Point(-180, -90);

            m_WpfGeoObjectList = geoObjectList;
            // Extract Min and Max Coordinate from list
            if (geoObjectList.Count <= 0)
            {
                minCoord = new Point(90, 45);
                maxCoord = new Point(-90, -45);
            }
            else
            {
                foreach (GeoObject geoObject in geoObjectList)
                {
                    ParseGeoObjectCoords(geoObject.GeometryData, ref minCoord, ref maxCoord);
                    if (m_WorkingAreaSaveFileName == string.Empty)
                        m_WorkingAreaSaveFileName = geoObject.Identifier + " " + geoObject.DisplayText;
                    // textBoxSampleText.Text = geoObject.DisplayText;
                }
            }

            // Call Webbrowser with center point and zoom level
            if (m_WebBrowserPoints != null)
            {
                m_WebBrowserPoints.Clear();
            }

            Point centerCoord = new Point((minCoord.X + maxCoord.X) / 2, (minCoord.Y + maxCoord.Y) / 2);

            m_WebBrowserPosLat = centerCoord.Y;
            m_WebBrowserPosLon = centerCoord.X;
            m_WebZoomlevel = zoomLevel;

            // m_NavFlag = true;
            m_WebScan = 1;

            // Set wait marker in clipboard
            // System.Windows.Forms.Clipboard.SetText(m_GooglePos);
            m_WebView2Coordinates = m_GooglePos;
            CallJavaScript();

            // Show WebBrowser
            WebBrowserAdapt();

            return true;
        }

        // Dispatcher timer
        private DispatcherTimer tmr = new DispatcherTimer();
        private string m_GooglePos = "Wait for Map...";
        private int t_Count = 0;
        private int t_Loops = 15;
        private int m_Count = 0;

        /// <summary>
        /// Adds the previously transmitted samples (if any). In case of MapServer mode:
        /// Sets up a timer to wait until the map has been created by the web service.
        /// </summary>
        /// <returns></returns>
        public void WpfAddSample()
        {
            // string str = DateTime.Now.TimeOfDay.ToString() + ": " + "WpfAddSample";
            // WLine(str);
            if (m_MouseMode == MouseMode.MapServer)
            {
                if (m_WebView2Coordinates_Received)
                {
                    // Add sample immediately
                    buttonAddShape_Click(null, null);
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(WpfSamplingPlotPage.Properties.Resources.StrWaitForMap, WpfSamplingPlotPage.Properties.Resources.ErrCaption, MessageBoxButton.YesNoCancel);
                    if (result != MessageBoxResult.Cancel)
                    {
                        if (result == MessageBoxResult.No)
                            OpenSampleFiles();
                        Thread.Sleep(200);

                        buttonAddShape_Click(null, null);
                    }
                    else
                    {
                        CallJavaScript();
                    }
                }
            }
            else
            {
                // Add sample immediately
                buttonAddShape_Click(null, null);
            }
        }

        /// <summary>
        /// The timer-elapsed event handler: Scans the map and adds it (plus all previously transmitted samples)
        /// to the sample list, if timer has been elapsed.
        /// </summary>
        /// <param name="source">The event source.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TimerAddSample(object source, EventArgs e)
        {
            // WLine("=== TimerAddSample");
            string tmp = string.Empty;
            t_Count++;
            m_Count++;
            // Check clipboard for map server response and time limit
            tmp = ReadCoordinatesFromClipboard();
            if (tmp == m_GooglePos && t_Count < t_Loops)
            {
                // WLine("<---- wait ------ " + tmp);
                return;
            }
            // Response has been detected, wait finally to give time for building the new map
            if (t_Count < t_Loops)
            {
                t_Count = t_Loops;
                // tmr.Interval = TimeSpan.FromMilliseconds(500);
                return;
            }
            // Timer has elapsed, stop timer
            tmr.Stop();
            tmr.Tick -= TimerAddSample;
            // Add sample  
            // WLine("<---- finally ------ " + tmp);
            buttonAddShape_Click(null, null);
            // MessageBox.Show(m_Count.ToString() + " loops: " + tmp);
            // string str = DateTime.Now.TimeOfDay.ToString() + ": " + m_Count.ToString() + " loops: TimerAddSample finished ---- finally -----> " + tmp;
            // WLine(str);
        }

        /* Load map from directory - currently not needed
        public bool WpfLoadMapAndGeoObjects(List<GeoObject> geoObjectList)
        {
            //  WLine("WpfSetMapAndGeoObjects(List<GeoObject> geoObjectList)");
            // Load imported GeoObjects
            if (geoObjectList == null)
                return false;

            // Set Map mode
            WpfSetMode(1);

            Point minCoord = new Point(180, 90);
            Point maxCoord = new Point(-180, -90);

            m_WpfGeoObjectList = geoObjectList;
            // Extract Min and Max Coordinate from list
            foreach (GeoObject geoObject in geoObjectList)
            {
                ParseGeoObjectCoords(geoObject.GeometryData, ref minCoord, ref maxCoord);
                if (m_WorkingAreaSaveFileName == string.Empty)
                    m_WorkingAreaSaveFileName = geoObject.Identifier + " " + geoObject.DisplayText;
            }

            // Look for Map in given directory
            // Load File covering the coordinates

            return true;
        }
        */

        /// <summary>
        /// Returnes a GeoObject-List of all visible shapes from WPF panel to be stored by Diversity application.
        /// </summary>
        /// <param name="geoObject">The geo object.</param>
        /// <param name="index">The index.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfGetGeoObjectByIndex(out GeoObject geoObject, ref int index)
        {
            geoObject = new GeoObject();
            while (index < m_SampleList.Count)
            {
                Sample sample = m_SampleList[index];
                if (sample.TypeOfSample == SampleType.IMAGE || !sample.IsSampleVisible)
                {
                    index++;
                    continue;
                }
                return SetupGeoObject(out geoObject, sample);
            }
            return false;
        }

        // -------------- bisher -------------------
        /// <summary>
        /// Returnes a GeoObject-List of all visible shapes from WPF panel to be stored by Diversity application.
        /// </summary>
        /// <param name="geoObjectList">Output list of Geoobjects.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfGetGeoObjects(out List<GeoObject> geoObjectList)
        {
            //  WLine("WpfGetGeoObjects(out List<GeoObject> geoObjectList)");
            GeoObject geoObject = new GeoObject();
            geoObjectList = new List<GeoObject>();
            if (m_SampleList.Count == 0)
                return false;
            SetupGeoObject(out geoObject, m_SampleList);
            geoObjectList.Add(geoObject);
            return true;
        }

        // -------------- neu -------------------
        /// <summary>
        /// Returnes a GeoObject-List of all visible shapes from WPF panel to be stored by Diversity application.
        /// </summary>
        /// <param name="geoObjectList">Output list of Geoobjects.</param>
        /// <param name="returnAsGeometryCollection">If true, all samples will be put together and returned in one collection. If false, the will be returned as List.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfGetGeoObjects(out List<GeoObject> geoObjectList, bool returnAsGeometryCollection)
        {
            //  WLine("WpfGetGeoObjects(out List<GeoObject> geoObjectList)");
            geoObjectList = new List<GeoObject>();
            if (m_SampleList.Count == 0)
                return false;
            if (returnAsGeometryCollection)
            {
                GeoObject geoObject = new GeoObject();
                SetupGeoObject(out geoObject, m_SampleList);
                geoObjectList.Add(geoObject);
            }
            else
            {
                SetupGeoObject(out geoObjectList, m_SampleList);
            }
            return true;
        }

        /// <summary>
        /// Returnes a GeoObject-List with original world coords of all visible shapes from WPF panel to be stored by Diversity application.
        /// </summary>
        /// <param name="geoObjectList">Output list of Geoobjects.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfGetWorldGeoObjects(out List<GeoObject> geoObjectList)
        {
            //  WLine("WpfGetGeoObjects(out List<GeoObject> geoObjectList)");
            GeoObject geoObject = new GeoObject();
            geoObjectList = new List<GeoObject>();
            if (m_SampleList.Count == 0)
                return false;
            SetupWorldGeoObject(out geoObject, m_SampleList);
            geoObjectList.Add(geoObject);
            return true;
        }

        /// <summary>
        /// Deletes all samples of the WPF sample list, eventually including the reference map.
        /// </summary>
        /// <param name="clearRefMap">If set to <c>true</c>, clears the reference map, too; else keeps the reference map.</param>
        /// <returns><c>true</c> if successful, <c>false</c> in case of error.</returns>
        public bool WpfClearAllSamples(bool clearRefMap)
        {
            //  WLine("WpfClearAllSamples(bool clearRefMap)");
            DeleteAllSamples(clearRefMap);
            return true;
        }

        /// <summary>
        /// Deletes all samples from the sample list.
        /// </summary>
        /// <param name="delRefMap">Deletes the reference map, too, if set to <c>true</c>, otherwise keeps it.</param>
        private void DeleteAllSamples(bool delRefMap)
        {
            // string labelInstructionText = labelInstruction.Content.ToString();

            int numPoints = 0;
            if (m_SampleList != null)
            {
                numPoints = m_SampleList.Count();
            }

            try
            {
                int count = 0;
                SampleImage refSample = null;

                if (numPoints > 0)
                {
                    DateTime start = DateTime.Now;

                    // Initialize progressbar
                    ProgressBarInit(WpfSamplingPlotPage.Properties.Resources.StrProgressRemovingSamples, numPoints);
                    // SetInstructionLabel(WpfSamplingPlotPage.Properties.Resources.StrDeletingShapesInstruction, true);
                    // DoEvents();

                    // Remove sample from list
                    foreach (Sample sample in m_SampleList)
                    {
                        bool noDel = false;
                        switch (sample.TypeOfSample)
                        {
                            case SampleType.POLYGON:
                            case SampleType.MULTIPOLYGON:
                                (sample as SamplePolygon).ClearSample();
                                break;
                            case SampleType.LINESTRING:
                            case SampleType.MULTILINESTRING:
                                (sample as SampleLines).ClearSample();
                                break;
                            case SampleType.POINT:
                            case SampleType.MULTIPOINT:
                                (sample as SamplePoints).ClearSample();
                                break;
                            case SampleType.IMAGE:
                                if ((sample as SampleImage) == m_RefMap)
                                {
                                    if (delRefMap)
                                    {
                                        m_RefMap = null;
                                        (sample as SampleImage).ClearSample();
                                    }
                                    else
                                    {
                                        // Keep reference Map
                                        refSample = sample as SampleImage;
                                        m_RefMap = refSample;
                                        noDel = true;
                                    }
                                }
                                else
                                {
                                    (sample as SampleImage).ClearSample();
                                }
                                break;
                        }
                        if (!noDel)
                            RemoveSampleControls(sample);

                        // Update progressbar
                        count++;
                        if (DateTime.Now - start > TimeSpan.FromSeconds(0.1))
                        {
                            ProgressBarUpdate(count);
                            start = DateTime.Now;
                        }
                        // Cancel action
                        if (m_ProgressCancel)
                            break;
                    }
                    ProgressBarUpdate(numPoints);
                    m_SampleList.Clear();
                }

                // Clear current samples
                if (m_SamplePolygon != null)
                    m_SamplePolygon.ClearSample();
                if (m_SampleLine != null)
                    m_SampleLine.ClearSample();
                if (m_SamplePoints != null)
                    m_SamplePoints.ClearSample();


                if (!delRefMap && refSample != null)
                {
                    m_SampleList.Add(refSample);
                    ReadjustToggleButtons();
                }

                // Reset world coordinates if no more reference map
                if (WorldHasDisappeared())
                    ClearCoordinates();
            }
            catch (Exception ex)
            {
                SetError(ex.ToString() + " - " + ex.Message);
            }
            finally
            {
                // Close progressbar
                if (numPoints > 0)
                {
                    ProgressBarClose();
                }
                // SetInstructionLabel(labelInstructionText, false);
            }
        }

        /// <summary>
        /// Isolates and returns the pure working area as a Canvas object to the caller.
        /// Used to eliminate the GIS editor control panel and sample list, just to show the map with samples.
        /// </summary>
        /// <returns>Canvas object of the working area.</returns>
        public Canvas WpfGetCanvas(System.Windows.Forms.Panel parent)
        {
            //  WLine("WpfGetCanvas()");
            m_CanvasParent = parent;

            try
            {
                if (m_Canvas.Parent == m_CanvasClip)
                {
                    m_CanvasClip.Children.Remove(m_Canvas);
                    // Adapt size of Webbrowser, if active
                    if (m_MouseMode == MouseMode.MapServer)
                    {
                        // Adapt WebBrowser area to full mode (2 pix for border)
                        WebBrowserAdapt();
                    }
                    m_ViewerMode = true;
                    ShowCaptureFrame(false);
                }
            }
            catch
            {
            }

            return m_Canvas;
        }

        /// <summary>
        /// Adds the working area to the original parent Canvas, to get the GIS Editor controls and sample list again when called from application.
        /// </summary>
        public void WpfSetCanvas()
        {
            //  WLine("WpfSetCanvas()");
            try
            {
                if (m_Canvas.Parent != m_CanvasClip)
                {
                    if (m_MouseMode == MouseMode.MapServer)
                    {
                        // Reset WebBrowser area to user control mode
                        WebBrowserAdapt();
                    }
                    m_CanvasClip.Children.Add(m_Canvas);
                    m_CanvasParent = null;
                    m_ViewerMode = false;
                }
            }
            catch
            {
            }
        }

        /* Control the cursor from outside - not needed
        public void WpfSetCursor(Cursor cursor)
        {
            m_Canvas.Cursor = cursor;
        }
        */

        /// <summary>
        /// Controls the handling of the delete button in the sample list for the reference map.
        /// </summary>
        /// <param name="show">If set to <c>true</c>, the delete button is enabled; else it is disabled to prevent deleting the reference map by mistake.</param>
        public void WpfShowRefMapDelButton(bool show)
        {
            //  WLine("WpfShowRefMapDelButton(bool show)");
            m_WpfShowRefMapDelButton = show;
            UpdateRefMapDeleteButtons();
        }

        /// <summary>
        /// Loads an image.
        /// </summary>
        /// <param name="path">The image path.</param>
        /// <param name="topLeft">The top left coordinate.</param>
        /// <param name="bottomRight">The bottom right coordinate.</param>
        /// <returns>
        /// 	<c>true</c> if successful, <c>false</c> in case of error.
        /// </returns>
        public bool WpfLoadImage(string path, Point topLeft, Point bottomRight)
        {
            if (isSupportedImageFile(path))
            {
                if (LoadSingleImage(path, topLeft.X, topLeft.Y, 255))
                {
                    radioButtonShift.IsChecked = true;
                    m_SampleImage.m_Image.Width = bottomRight.X;
                    m_SampleImage.m_Image.Height = bottomRight.Y;
                    return true;
                }
            }
            return false;
        }


        //==========================================================================================
        //============= NEU  =======================================================================
        //==========================================================================================

        /// <summary>
        /// Loads an image.
        /// </summary>
        /// <param name="uri">The image web address.</param>
        /// <param name="geoObjectList">The input Geoobject list.</param>
        /// <param name="zoom">Flag for zoom to GeoObject or not.</param>
        /// <returns>
        /// 	<c>true</c> if successful, <c>false</c> in case of error.
        /// </returns>
        public bool WpfSetWebImageAndGeoObjects(string uri, List<GeoObject> geoObjectList, bool zoom)
        {
            if (geoObjectList == null)
                return false;

            try
            {
                if (isSupportedImageFile(uri))
                {
                    // Clear samples and reference map
                    WpfClearAllSamples(true);

                    ResetCanvasPosAndZoom();

                    Point minCoord = new Point(1, 1);
                    Point maxCoord = new Point(0, 0);

                    Image image = new Image();
                    Uri webUri = new Uri(uri);
                    // Load image from web
                    HandleImage(image, webUri);
                    // BitmapImage(image, webUri);
                    // Give some time to receive width and height (otherwise 1 x 1 pixel)
                    Thread.Sleep(200);
                    // Call Events to assign width and height 
                    DoEvents();

                    // Check for dimensions
                    if (image.Source.Width == 1.0)
                        return false;

                    // Preset clipboard with default lon/lat coordinates 0.0° - 1.0° 
                    // System.Windows.Forms.Clipboard.SetDataObject("((0.0, 0.0), (1.0, 1.0))", true);
                    m_WebView2Coordinates = "((0.0, 0.0), (1.0, 1.0))";


                    if (LoadSingleWebImage(image, 0, 0, 255))
                    {
                        // Reset m_SampleImage
                        m_SampleImage = null;

                        // Set polygon parameters
                        m_PolygonStrokeThickness = 2.0;
                        m_LastStrokeBrush = m_LastFillBrush = Brushes.Red;
                        m_FillTransparency = 0;

                        // Switch to shift mode
                        radioButtonShift.IsChecked = true;

                        /* Reset canvas position
                        m_CanvasThickness.Left = 0;
                        m_CanvasThickness.Right = 0;
                        m_CanvasThickness.Top = 0;
                        m_CanvasThickness.Bottom = 0;
                        m_Canvas.Margin = m_CanvasThickness;
                        */

                        // Find min and max coords for GeoObjects
                        foreach (GeoObject geoObject in geoObjectList)
                        {
                            ParseGeoObjectCoords(geoObject.GeometryData, ref minCoord, ref maxCoord);
                        }

                        // Display delivered GeoObjects
                        WpfSetGeoObjects(geoObjectList);

                        double xMinObj = minCoord.X - 0.01;
                        double xMaxObj = maxCoord.X + 0.01;
                        double yMinObj = minCoord.Y - 0.01;
                        double yMaxObj = maxCoord.Y + 0.01;

                        // Actual Width and Height of the Image
                        double imgWidth = image.Source.Width;
                        double imgHeight = image.Source.Height;

                        // Actual Width and Height of the Canvas holding the Image
                        // double canvasWidth = (m_Canvas as FrameworkElement).ActualWidth == 0 ? (m_Canvas as FrameworkElement).Width : (m_Canvas as FrameworkElement).ActualWidth;
                        // double canvasHeight = (m_Canvas as FrameworkElement).ActualHeight == 0 ? (m_Canvas as FrameworkElement).Height : (m_Canvas as FrameworkElement).ActualHeight;
                        double canvasWidth = (m_Canvas as FrameworkElement).ActualWidth;
                        double canvasHeight = (m_Canvas as FrameworkElement).ActualHeight;

                        // Scale factors for Image so the GeoObjects will fit best
                        double scaleX = (xMaxObj - xMinObj);
                        double scaleY = (yMaxObj - yMinObj);

                        // Scale factors for Canvas regarding Image
                        double scaleCanvasX = imgWidth / canvasWidth;
                        double scaleCanvasY = imgHeight / canvasHeight;

                        double zoomFactorGeoObject = 1.0;
                        double zoomFactorCanvas = 1.0;

                        double zoomFactorHorizontal = 1.0;
                        double zoomFactorVertical = 1.0;

                        zoomFactorGeoObject = 1 / scaleX;
                        zoomFactorCanvas = 1 / scaleCanvasX;
                        zoomFactorHorizontal = zoomFactorGeoObject * zoomFactorCanvas;

                        zoomFactorGeoObject = 1 / scaleY;
                        zoomFactorCanvas = 1 / scaleCanvasY;
                        zoomFactorVertical = zoomFactorGeoObject * zoomFactorCanvas;

                        // Check zoom option
                        if (zoom)
                        {
                            // Zoom image according to GeoObjects
                            if (zoomFactorHorizontal > zoomFactorVertical)
                                m_ZoomFactor = zoomFactorVertical;
                            else
                                m_ZoomFactor = zoomFactorHorizontal;

                            double shiftX = xMinObj * imgWidth;
                            double shiftY = (1 - yMaxObj) * imgHeight;

                            // Zoom and scale canvas according to parameters
                            ChangeCanvasPosAndZoom(m_ZoomFactor, shiftX * m_ZoomFactor, shiftY * m_ZoomFactor);
                        }
                        else
                        {
                            // Zoom image according to canvas
                            m_ZoomFactor = (scaleCanvasX > scaleCanvasY) ? (1 / scaleCanvasX) : (1 / scaleCanvasY);
                            // Zoom and scale canvas according to parameters
                            ChangeCanvasPosAndZoom(m_ZoomFactor, 0, 0);
                        }
                        return true;
                    }
                }
            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// Loads an image.
        /// </summary>
        /// <param name="uri">The image web address.</param>
        /// <returns>
        /// 	<c>true</c> if successful, <c>false</c> in case of error.
        /// </returns>
        public bool WpfLoadWebImage(string uri)
        {
            try
            {
                // Clear samples and reference map
                WpfClearAllSamples(true);

                if (isSupportedImageFile(uri))
                {
                    ResetCanvasPosAndZoom();

                    Image image = new Image();
                    Uri webUri = new Uri(uri);
                    // Load image from web
                    HandleImage(image, webUri);
                    // BitmapImage(image, webUri);
                    // Give some time to receive width and height (otherwise 1 x 1 pixel)
                    Thread.Sleep(200);
                    // Call Events to assign width and height 
                    DoEvents();

                    // Check for dimensions
                    if (image.Source.Width == 1.0)
                        return false;

                    // Preset clipboard with default lon/lat coordinates 0.0° - 1.0° 
                    // System.Windows.Forms.Clipboard.SetDataObject("((0.0, 0.0), (1.0, 1.0))", true);
                    m_WebView2Coordinates = "((0.0, 0.0), (1.0, 1.0))";


                    if (LoadSingleWebImage(image, 0, 0, 255))
                    {
                        // Reset m_SampleImage
                        m_SampleImage = null;

                        // Switch to shift mode
                        // radioButtonShift.IsChecked = true;

                        // Set polygon parameters
                        m_PolygonStrokeThickness = 2.0;
                        m_LastStrokeBrush = m_LastFillBrush = Brushes.Red;
                        m_FillTransparency = 0;

                        // Actual Width and Height of the Image
                        double imgWidth = image.Source.Width;
                        double imgHeight = image.Source.Height;

                        // Actual Width and Height of the Canvas holding the Image
                        double canvasWidth = (m_Canvas as FrameworkElement).ActualWidth;
                        double canvasHeight = (m_Canvas as FrameworkElement).ActualHeight;

                        // Scale factors for Canvas regarding Image
                        double scaleCanvasX = imgWidth / canvasWidth;
                        double scaleCanvasY = imgHeight / canvasHeight;

                        m_ZoomFactor = (scaleCanvasX > scaleCanvasY) ? (1 / scaleCanvasX) : (1 / scaleCanvasY);
                        // Zoom and scale canvas according to parameters
                        ChangeCanvasPosAndZoom(m_ZoomFactor, 0, 0);

                        // Switch to area mode
                        radioButtonDrawPolygon.IsChecked = true;

                        // Create SamplePolygon to allow setting the transparency at this place
                        m_SamplePolygon = new SamplePolygon(m_Canvas);
                        m_SamplePolygon.FillTransparency = 0;

                        return true;
                    }
                }
            }
            catch
            {

            }
            return false;
        }


        /// <summary>
        /// Loads an image from web URI.
        /// </summary>
        /// <param name="image">The image instance.</param>
        /// <param name="webUri">The image web address.</param>
        private void HandleImage(Image image, Uri webUri)
        {
            BitmapDecoder bDecoder = BitmapDecoder.Create(webUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);
            if (bDecoder != null && bDecoder.Frames.Count > 0)
            {
                image.Source = bDecoder.Frames[0];
            }
        }

        private void ResetCanvasPosAndZoom()
        {
            // Reset canvas position
            m_CanvasThickness.Left = 0;
            m_CanvasThickness.Right = 0;
            m_CanvasThickness.Top = 0;
            m_CanvasThickness.Bottom = 0;
            m_Canvas.Margin = m_CanvasThickness;
            // Reset zoom factor
            m_ZoomFactor = 1;
            m_Canvas.RenderTransform = new ScaleTransform(m_ZoomFactor, m_ZoomFactor);
        }

        private void ChangeCanvasPosAndZoom(double zoom, double xDif, double yDif)
        {
            // Adapt canvas position (shift image on canvas)
            m_CanvasThickness.Left -= xDif;
            m_CanvasThickness.Right += xDif;
            m_CanvasThickness.Top -= yDif;
            m_CanvasThickness.Bottom += yDif;
            // Set new position
            m_Canvas.Margin = m_CanvasThickness;

            // Scale canvas according to zoom factor
            m_Canvas.RenderTransform = new ScaleTransform(zoom, zoom);
        }

        /*
        /// <summary>
        /// Loads an image from web URI.
        /// </summary>
        /// <param name="image">The image instance.</param>
        /// <param name="webUri">The image web address.</param>
        private void BitmapImage(Image image, Uri webUri)
        {
            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = webUri;

            // To save significant application memory, set the DecodePixelWidth or  
            // DecodePixelHeight of the BitmapImage value of the image source to the desired 
            // height or width of the rendered image. If you don't do this, the application will 
            // cache the image as though it were rendered as its normal size rather then just 
            // the size that is displayed.
            // Note: In order to preserve aspect ratio, set DecodePixelWidth
            // or DecodePixelHeight but not both.
            // myBitmapImage.DecodePixelWidth = Convert.ToInt32(800);

            myBitmapImage.EndInit();

            //set image source
            image.Source = myBitmapImage;

        }
        */

        /// <summary>
        /// Switches to next shape.
        /// </summary>
        public void WpfNextShape()
        {
            // Switch to next area
            buttonMultiShape_Click(null, null);
        }

        /// <summary>
        /// Adds a shape.
        /// </summary>
        public void WpfAddShape()
        {
            // Add shape to sample list
            buttonAddShape_Click(null, null);
        }

        /// <summary>
        /// Switches silent mode.
        /// </summary>
        public void WpfSetSilentMode(bool silent)
        {
            // Add shape to sample list
            m_SilentErrorMode = silent;
        }

        /// <summary>
        /// Switches the selection frame.
        /// </summary>
        public void WpfSetFrame(bool show)
        {
            // Add shape to sample list
            m_AreaFrame = WpfSamplingPlotPage.Properties.Settings.Default.AreaFrame = show;
        }

        /// <summary>
        /// Sets the sample opacity if switched off.
        /// </summary>
        /// <param name="transparency">The sample opacity (0...255).</param>
        public void WpfSetSampleOffTransparency(byte transparency)
        {
            // Set sample opacity
            m_SampleOffTransparency = WpfSamplingPlotPage.Properties.Settings.Default.SampleOffTransparency = transparency;
        }

        #endregion // Interface to Windows Application

        #region Helper methods

        static private StreamWriter m_SW = null;


        /// <summary>
        /// Helper: Write debug text to file
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="path">The path.</param>
        public void WLine(string str, string path)
        {
            string TargetDirectory = path;

            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                if (m_SW == null)
                    m_SW = new StreamWriter(TargetDirectory, true);

                //Write a line of text
                if (m_SW != null)
                    m_SW.WriteLine(str);
            }
            catch
            {
            }
            finally
            {
                m_SW.Close();
                m_SW = null;
            }
        }

        /// <summary>
        /// Helper: Write debug text to file
        /// </summary>
        /// <param name="str">The STR.</param>
        public void WLine(string str)
        {
            if (!debugout)
                return;

            string TargetDirectory = "C:\\Test\\Debug.txt";

            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                if (m_SW == null)
                    m_SW = new StreamWriter(TargetDirectory, true);

                //Write a line of text
                if (m_SW != null)
                    m_SW.WriteLine(str);
            }
            catch
            {
            }
            finally
            {
                if (m_SW != null)
                {
                    m_SW.Close();
                    m_SW = null;
                }
            }
        }

        // Dispatcher timer
        private DispatcherTimer timer = new DispatcherTimer();
        private int m_TimerCount = 0;

        private void TimerStart()
        {
            // Setup timer to wait for response from the map server
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += TimerCheck;
            m_TimerCount = 0;
            timer.Start();
        }

        private void TimerCheck(object source, EventArgs e)
        {
            SetInstructionLabel("Timer " + (++m_TimerCount).ToString(), true);
        }

        private void TimerStop()
        {
            timer.Stop();
            timer.Tick -= TimerCheck;
        }

        #endregion // Helper methods

        #endregion // Methods
    }
}
