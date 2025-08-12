using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WpfSamplingPlotPage;
using System.Web;

namespace DiversityCollection.UserControls
{
    //public struct MapSymbol
    //{
    //    WpfSamplingPlotPage.PointSymbol PointSymbol;
    //    string DisplayText;
    //    double Transparency;
    //    System.Drawing.Font Font;
    //}

    /// <summary>
    /// UserControl zur Anzeige und Verarbeitung von Geoobjekten
    /// - Anzeige von Punkt: 
    ///     Karte mit voreingestelltem Zoomlevel und zentriertem Marker für Position
    ///     Dies wird benötigt wenn der Benutzer durch die Datensätz blättert und die Positionen angezeigt werden sollen
    ///     
    /// - Anzeige von Geoobjekten: 
    /// - Bearbeitung von Punkt:
    /// - Bearbeitung von Geoobjekten:
    /// </summary>
    public partial class UserControlGIS : UserControl
    {
        #region Parameter

        /// <summary>
        /// the WPF control for the GIS operations
        /// </summary>
        private WpfSamplingPlotPage.WpfControl _wpfControl;

        private UserControls.iMainForm _iMainForm;

        /// <summary>
        /// the state of the control in relation to which data are shown or edited. 
        /// Distribution is different from the others as in this state geo objects will be collected
        /// </summary>
        public enum SelectedGisObject { Organism, Event, Series, AnyCoordinate, Distribution, DistributionIncludingOrganisms };
        private SelectedGisObject _SelectedGisObject = SelectedGisObject.Event;

        /// <summary>
        /// the mode of the control, if it is used for editing, browsing or viewing
        /// </summary>
        public enum MapMode { Browse, Edit, View };
        private MapMode _MapMode = MapMode.Browse;
        public MapMode ModeOfMap() { return this._MapMode; }

        //private System.Data.DataTable _DtDistribution;
        private System.Data.DataTable _DtDistributionBrowser;
        //private List<GeoObject> _DistributionObjectList;

        private List<GeoObject> _DistributionObjectListBrowser;
        private List<GeoObject> DistributionObjectListBrowser
        {
            get
            {
                this._DistributionObjectListBrowser = new List<GeoObject>();
                if (this._DtDistributionBrowser != null
                    && this._DtDistributionBrowser.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in this._DtDistributionBrowser.Rows)
                    {
                        this._DistributionObjectListBrowser.Add(this.setupGeoObject(R["Identifier"].ToString()
                            , R["DisplayText"].ToString(), R["Geography"].ToString()));
                    }
                }
                return _DistributionObjectListBrowser;
            }
        }

        //private System.Data.DataTable _DtGeoObjects;
        private List<GeoObject> _DistributionObjectListEditor;
        private List<GeoObject> DistributionObjectListEditor
        {
            get
            {
                if (this._DistributionObjectListEditor == null)
                    this._DistributionObjectListEditor = new List<GeoObject>();
                return _DistributionObjectListEditor;
            }
        }

        private List<GeoObject> _GeoObjectsEditor;
        private GeoObject _GeoObject;

        private int? _CollectionSpecimenID = null;

        private System.Data.DataRow _DataRowIdentificationUnitGeoAnalysisGeo;
        private System.Data.DataRow _DataRowCollectionEventLocalisationGeo;
        private System.Data.DataRow _DataRowCollectionEventSeriesGeo;

        private bool _MapIsFixed = false;

        public bool MapIsFixed
        {
            get { return _MapIsFixed; }
            set 
            { 
                _MapIsFixed = value;
                if (_MapIsFixed)
                {
                    this.buttonMapIsFixed.BackColor = System.Drawing.Color.White;
                    this.buttonMapIsFixed.ForeColor = System.Drawing.Color.Black;
                    this.buttonMapIsFixed.Image = this.imageListHold.Images[0];
                    this.toolTip.SetToolTip(this.buttonMapIsFixed, "Map is fixed");
                }
                else
                {
                    this.buttonMapIsFixed.BackColor = System.Drawing.SystemColors.Control;
                    this.buttonMapIsFixed.ForeColor = System.Drawing.SystemColors.Control;
                    this.buttonMapIsFixed.Image = this.imageListHold.Images[1];
                    this.toolTip.SetToolTip(this.buttonMapIsFixed, "Map is NOT fixed");
                }
            }
        }

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool>> _TablePermissions = null;

        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool>> TablePermissions
        {
            get 
            { 
                if (this._TablePermissions == null)
                {
                    this._TablePermissions = new Dictionary<string, Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool>>();
                    System.Collections.Generic.List<string> Tables = new List<string>();
                    Tables.Add("CollectionEventSeries");
                    Tables.Add("CollectionEventLocalisation");
                    Tables.Add("IdentificationUnitGeoAnalysis");
                    foreach (string T in Tables)
                    {
                        System.Collections.Generic.Dictionary<DiversityWorkbench.Forms.FormFunctions.Permission, bool> PP = DiversityWorkbench.Forms.FormFunctions.TablePermissions(T);
                        this._TablePermissions.Add(T, PP);
                    }
                }
                return _TablePermissions;
            }
            set { _TablePermissions = value; }
        }

        public void setAvailability(bool IsAvailable = true)
        {
            if (IsAvailable)
            {
            }
            else
            {

            }
        }

        #endregion

        #region Construction

        public UserControlGIS(UserControls.iMainForm MainForm)
        {
            try
            {
                InitializeComponent();
                this._iMainForm = MainForm;
                this.initWPFcontrol();
                this.fillSymbolList();
                this.fillColorList();
                this.setVisibilityOfControls();

                ///Markus 21.8.2019: TODO - klappte zuletzt in Version 3.0.9.6 - seitdem nicht mehr. Unklar wieso da keine Änderung bekannt
#if !DEBUG
                // Browser auf envelope center reduziert
                //this.toolStripMenuItemEditModeBrowser.Visible = false;

                //this.toolStripMenuItemEditModeGisNoEdit_Click(null, null);
                this.toolStripButtonAddToDistribution.Visible = false;
                this.toolStripMenuItemStateDistribution.Visible = false;
                this.distInclOrgToolStripMenuItem.Visible = false;
                this.toolStripButtonRequeryDistribution.Visible = false;
#endif
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region public Properties and interface

        /// <summary>
        /// setting the CollectionSpecimenID starts the display of the geography as choosen in the control
        /// </summary>
        public int? CollectionSpecimenID
        {
            set
            {
                this._CollectionSpecimenID = value;
                if (this._SelectedGisObject != SelectedGisObject.Distribution && 
                    this._SelectedGisObject != SelectedGisObject.DistributionIncludingOrganisms)
                {
                    this.setVisibilityOfControls();
                    this.DisplayGeoObjects();
                }
            }
        }

        public System.Data.DataRow DataRowIdentificationUnitGeoAnalysisGeo
        {
            get
            {
                return this._DataRowIdentificationUnitGeoAnalysisGeo;
            }
            set
            {
                this._DataRowIdentificationUnitGeoAnalysisGeo = value;
                if (this._SelectedGisObject == SelectedGisObject.Organism)
                    this.DisplayGeoObjects(); 
            }
        }

        public System.Data.DataRow DataRowCollectionEventLocalisationGeo
        {
            get
            {
                return this._DataRowCollectionEventLocalisationGeo;
            }
            set
            {
                this._DataRowCollectionEventLocalisationGeo = value;
            }
        }

        public System.Data.DataRow DataRowCollectionEventSeriesGeo
        {
            get
            {
                return this._DataRowCollectionEventSeriesGeo;
            }
            set
            {
                this._DataRowCollectionEventSeriesGeo = value;
            }
        }

        /// <summary>
        /// the state of the control in respect to which objects are shown, Organisms, Events, ...
        /// </summary>
        public SelectedGisObject SelectedGISObject
        {
            get { return _SelectedGisObject; }
            set
            {
                if (this._SelectedGisObject == SelectedGisObject.AnyCoordinate && value == SelectedGisObject.Series)
                {
                    // Bugfix - evtl. bessere Loesung notwendig
                    // happens after creating e.g. a new organism
                }
                else
                {
                    this._SelectedGisObject = value;
                    switch (_SelectedGisObject)
                    {
                        case SelectedGisObject.Organism:
                            toolStripMenuItemStateOrganism_Click(null, null);
                            break;
                        case SelectedGisObject.Series:
                            toolStripMenuItemStateSeries_Click(null, null);
                            break;
                        case SelectedGisObject.Event:
                            toolStripMenuItemStateEvent_Click(null, null);
                            break;
                        case SelectedGisObject.Distribution:
                            toolStripMenuItemStateDistribution_Click(null, null);
                            break;
                    }
                    if (this._SelectedGisObject == SelectedGisObject.Distribution)
                    {
                        //this._DtGeoObjects = null;
                        this._DtDistributionBrowser = null;
                    }
                }
            }
        }

        #endregion

        #region Lists for symbols and colors and related functions and properties

        private void fillColorList()
        {
            int w = 30;
            ToolStripItem Tred = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Red.ToString());
            Tred.BackColor = System.Drawing.Color.Red;
            Tred.Tag = System.Windows.Media.Brushes.Red;
            //Tred.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Tred.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);
            //Tred.Width = w;

            ToolStripItem Torange = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Orange.ToString());
            Torange.BackColor = System.Drawing.Color.Orange;
            Torange.Tag = System.Windows.Media.Brushes.Orange;
            //Torange.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Torange.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            ToolStripItem Tyellow = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Yellow.ToString());
            Tyellow.BackColor = System.Drawing.Color.Yellow;
            Tyellow.Tag = System.Windows.Media.Brushes.Yellow;
            //Tyellow.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Tyellow.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            ToolStripItem Tgreen = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Green.ToString());
            Tgreen.BackColor = System.Drawing.Color.Green;
            Tgreen.Tag = System.Windows.Media.Brushes.Green;
            //Tgreen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Tgreen.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            ToolStripItem Tblue = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Blue.ToString());
            Tblue.BackColor = System.Drawing.Color.Blue;
            Tblue.Tag = System.Windows.Media.Brushes.Blue;
            //Tblue.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Tblue.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            ToolStripItem Tviolet = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Violet.ToString());
            Tviolet.BackColor = System.Drawing.Color.Violet;
            Tviolet.Tag = System.Windows.Media.Brushes.Violet;
            //Tviolet.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Tviolet.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            ToolStripItem Tbrown = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Brown.ToString());
            Tbrown.BackColor = System.Drawing.Color.Brown;
            Tbrown.Tag = System.Windows.Media.Brushes.Brown;
            //Tbrown.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Tbrown.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            ToolStripItem Tblack = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Black.ToString());
            Tblack.BackColor = System.Drawing.Color.Black;
            Tblack.Tag = System.Windows.Media.Brushes.Black;
            //Tblack.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Tblack.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            ToolStripItem Tgray = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.Gray.ToString());
            Tgray.BackColor = System.Drawing.Color.Gray;
            Tgray.Tag = System.Windows.Media.Brushes.Gray;
            //Tgray.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Tgray.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            ToolStripItem Twhite = this.toolStripDropDownButtonColor.DropDownItems.Add(System.Windows.Media.Brushes.White.ToString());
            Twhite.BackColor = System.Drawing.Color.White;
            Twhite.Tag = System.Windows.Media.Brushes.White;
            //Twhite.DisplayStyle = ToolStripItemDisplayStyle.Image;
            //Twhite.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);

            foreach (System.Windows.Forms.ToolStripDropDownItem DD in this.toolStripDropDownButtonColor.DropDownItems)
            {
                DD.DisplayStyle = ToolStripItemDisplayStyle.Image;
                DD.Width = 30;
                DD.Click += new System.EventHandler(this.toolStripMenuItemColor_Click);
                DD.AutoSize = false;
            }
            this.toolStripMenuItemColor_Click(Tred, null);
        }

        private void fillSymbolList()
        {
            ToolStripItem Tpin = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.Pin.ToString());
            Tpin.Tag = WpfSamplingPlotPage.PointSymbol.Pin;
            Tpin.Image = this.imageListSymbol.Images[5];
            Tpin.Image.Tag = this.imageListSymbol.Images.Keys[5];

            ToolStripItem Tcross = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.Cross.ToString());
            Tcross.Tag = WpfSamplingPlotPage.PointSymbol.Cross;
            Tcross.Image = this.imageListSymbol.Images[2];
            Tcross.Image.Tag = this.imageListSymbol.Images.Keys[2];

            ToolStripItem Tx = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.X.ToString());
            Tx.Tag = WpfSamplingPlotPage.PointSymbol.X;
            Tx.Image = this.imageListSymbol.Images[8];
            Tx.Image.Tag = this.imageListSymbol.Images.Keys[8];

            ToolStripItem Tcircle = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.Circle.ToString());
            Tcircle.Tag = WpfSamplingPlotPage.PointSymbol.Circle;
            Tcircle.Image = this.imageListSymbol.Images[0];
            Tcircle.Image.Tag = this.imageListSymbol.Images.Keys[0];

            ToolStripItem TcircleFilled = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.Circle.ToString() + "Filled");
            TcircleFilled.Tag = WpfSamplingPlotPage.PointSymbol.Circle;
            TcircleFilled.Image = this.imageListSymbol.Images[1];
            TcircleFilled.Image.Tag = this.imageListSymbol.Images.Keys[1];

            ToolStripItem Tsquare = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.Square.ToString());
            Tsquare.Tag = WpfSamplingPlotPage.PointSymbol.Square;
            Tsquare.Image = this.imageListSymbol.Images[6];
            Tsquare.Image.Tag = this.imageListSymbol.Images.Keys[6];

            ToolStripItem TsquareFilled = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.Square.ToString() + "Filled");
            TsquareFilled.Tag = WpfSamplingPlotPage.PointSymbol.Square;
            TsquareFilled.Image = this.imageListSymbol.Images[7];
            TsquareFilled.Image.Tag = this.imageListSymbol.Images.Keys[7];

            ToolStripItem Tdiamond = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.Diamond.ToString());
            Tdiamond.Tag = WpfSamplingPlotPage.PointSymbol.Diamond;
            Tdiamond.Image = this.imageListSymbol.Images[3];
            Tdiamond.Image.Tag = this.imageListSymbol.Images.Keys[3];

            ToolStripItem TdiamondFilled = this.toolStripDropDownButtonSymbol.DropDownItems.Add(WpfSamplingPlotPage.PointSymbol.Diamond.ToString() + "Filled");
            TdiamondFilled.Tag = WpfSamplingPlotPage.PointSymbol.Diamond;
            TdiamondFilled.Image = this.imageListSymbol.Images[4];
            TdiamondFilled.Image.Tag = this.imageListSymbol.Images.Keys[4];

            foreach (System.Windows.Forms.ToolStripDropDownItem DD in this.toolStripDropDownButtonSymbol.DropDownItems)
            {
                DD.DisplayStyle = ToolStripItemDisplayStyle.Image;
                DD.Click += new System.EventHandler(this.toolStripMenuItemSymbol_Click);
                DD.AutoSize = false;
                DD.Width = 30;
            }

            this.toolStripMenuItemColor_Click(Tpin, null);

        }

        private void toolStripMenuItemColor_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem D = (System.Windows.Forms.ToolStripMenuItem)sender;

            this.toolStripDropDownButtonColor.BackColor = D.BackColor;
            this.toolStripDropDownButtonColor.Text = "     ";
            this.toolStripDropDownButtonColor.Tag = D.Tag;
        }

        private void toolStripMenuItemSymbol_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem D = (System.Windows.Forms.ToolStripMenuItem)sender;

            this.toolStripDropDownButtonSymbol.Text = D.Text;
            this.toolStripDropDownButtonSymbol.Tag = D.Tag;
            this.toolStripDropDownButtonSymbol.Image = D.Image;
            this.toolStripDropDownButtonSymbol.Image.Tag = D.Image.Tag;

            this.toolStripDropDownButtonSymbol.DisplayStyle = D.DisplayStyle;
        }

        private WpfSamplingPlotPage.PointSymbol Symbol
        {
            get
            {
                try
                {
                    return (WpfSamplingPlotPage.PointSymbol)this.toolStripDropDownButtonSymbol.Tag;
                }
                catch
                {
                    return WpfSamplingPlotPage.PointSymbol.Pin;
                }
            }
        }

        private WpfSamplingPlotPage.PointSymbol PointType { get { return this.Symbol; } }
        
        private System.Windows.Media.Brush Brush
        {
            get
            {
                System.Windows.Media.Brush b = System.Windows.Media.Brushes.White;
                try
                {
                    if (this.toolStripDropDownButtonColor.Tag != null)
                        b = (System.Windows.Media.Brush)this.toolStripDropDownButtonColor.Tag;
                }
                catch { }
                return b;
            }
        }

        private System.Windows.Media.Brush FillBrush { get { return this.Brush; } }

        private System.Windows.Media.Brush StrokeBrush { get { return this.Brush; } }

        private byte StrokeTransparency { get { return 255; } }

        private byte FillTransparency 
        { 
            get 
            {
                try
                {
                    byte Trans = 0;
                    if (this.toolStripDropDownButtonSymbol.Image.Tag.ToString() == this.imageListSymbol.Images.Keys[1].ToString()
                        || this.toolStripDropDownButtonSymbol.Image.Tag.ToString() == this.imageListSymbol.Images.Keys[4].ToString()
                        || this.toolStripDropDownButtonSymbol.Image.Tag.ToString() == this.imageListSymbol.Images.Keys[7].ToString())
                        Trans = 255;
                    return Trans;
                }
                catch
                {
                    return 32;
                }
            } 
        }

        private double StrokeThickness 
        { 
            get 
            {
                try
                {
                    double T = 1.0;
                    if (!double.TryParse(this.toolStripTextBoxThickness.Text, out T))
                        T = 1.0;
                    return T;
                }
                catch
                {
                    return 2.0;
                }
            } 
        }

        private double SymbolSize
        {
            get
            {
                try
                {
                    return double.Parse(this.toolStripTextBoxSize.Text);
                }
                catch
                {
                    return 10.0;
                }
            }
        }

        private double PointSymbolSize { get { return this.SymbolSize; } }

        private void toolStripTextBoxSize_Leave(object sender, EventArgs e)
        {
            double d = 1.0;
            if (!double.TryParse(this.toolStripTextBoxSize.Text, out d))
            {
                d = 1;
                toolStripTextBoxSize.Text = d.ToString();
                System.Windows.Forms.MessageBox.Show("Please enter a numeric value\r\nThe value has been set to 1");
            }
            if (d == 0)
            {
                d = 1;
                toolStripTextBoxSize.Text = d.ToString();
                System.Windows.Forms.MessageBox.Show("Please enter a numeric value different from 0\r\nThe value has been set to 1");
            }
        }

        private void toolStripTextBoxThickness_Leave(object sender, EventArgs e)
        {
            double d = 1;
            if (!double.TryParse(this.toolStripTextBoxThickness.Text, out d))
            {
                d = 1;
                toolStripTextBoxThickness.Text = d.ToString();
                System.Windows.Forms.MessageBox.Show("Please enter a numeric value\r\nThe value has been set to 1");
            }
            if (d == 0)
            {
                d = 1;
                toolStripTextBoxThickness.Text = d.ToString();
                System.Windows.Forms.MessageBox.Show("Please enter a numeric value different from 0\r\nThe value has been set to 1");
            }
        }

        #endregion

        #region setting the GIS state and Controls

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
            if (this._MapMode == MapMode.Edit
                && this._GeoObjectsEditor.Count == 1
                && this._SelectedGisObject != SelectedGisObject.Distribution
                && this._SelectedGisObject != SelectedGisObject.DistributionIncludingOrganisms)
            {
                this.toolStripButtonSave.Visible = true;
            }
            else
                this.toolStripButtonSave.Visible = false;

            // Distribution controls
            if (this._SelectedGisObject == SelectedGisObject.Distribution ||
                this._SelectedGisObject == SelectedGisObject.DistributionIncludingOrganisms)
            {
                this.toolStripButtonRequeryDistribution.Visible = true;
                this.toolStripButtonAddToDistribution.Visible = true;
                this.checkBoxIncludeAll.Visible = true;
            }
            else
            {
                this.toolStripButtonRequeryDistribution.Visible = false;
                this.toolStripButtonAddToDistribution.Visible = false;
                this.checkBoxIncludeAll.Visible = false;
            }

            // Controls for symbols and color
            // Controls for the GIS editor
            if (this._MapMode == MapMode.Browse)
            {
                // Symbol
                this.toolStripLabelSymbol.Visible = false;
                this.toolStripDropDownButtonSymbol.Visible = false;
                // Color
                this.toolStripDropDownButtonColor.Visible = false;
                // Size
                this.toolStripLabelSize.Visible = false;
                this.toolStripTextBoxSize.Visible = false;
                // Line
                this.toolStripTextBoxThickness.Visible = false;
                this.toolStripButtonLineThickness.Visible = false;
                // Zoomlevel - is used for browser as well
                this.checkBoxZoomLevel.Visible = true;
                this.numericUpDownZoomLevel.Visible = true;
                // Reload
                //this.buttonReload.Visible = false;
                // Fixed Map
                this.buttonMapIsFixed.Visible = false;
            }
            else
            {
                this.checkBoxZoomLevel.Visible = true;
                this.numericUpDownZoomLevel.Visible = true;
                this.buttonReload.Visible = true;
                this.buttonMapIsFixed.Visible = true;

                bool IsDistributionMap = false;
                if (this._SelectedGisObject == SelectedGisObject.Distribution
                    || this._SelectedGisObject == SelectedGisObject.DistributionIncludingOrganisms)
                {
                    IsDistributionMap = true;
                }
                // Symbol
                this.toolStripLabelSymbol.Visible = IsDistributionMap;
                this.toolStripDropDownButtonSymbol.Visible = IsDistributionMap;
                // Color
                this.toolStripDropDownButtonColor.Visible = IsDistributionMap;
                // Line
                this.toolStripTextBoxThickness.Visible = IsDistributionMap;
                this.toolStripButtonLineThickness.Visible = IsDistributionMap;
                // Size
                this.toolStripTextBoxSize.Visible = IsDistributionMap;
                this.toolStripLabelSize.Visible = IsDistributionMap;
            }

            // controls for the data
            if (this.DataRowCollectionEventSeriesGeo == null)
                this.toolStripMenuItemStateSeries.Enabled = false;
            else this.toolStripMenuItemStateSeries.Enabled = true;

            if (this.DataRowIdentificationUnitGeoAnalysisGeo == null)
                this.toolStripMenuItemStateOrganism.Enabled = false;
            else this.toolStripMenuItemStateOrganism.Enabled = true;

            // handling of the GIS editor
            if (this._MapMode == MapMode.View)
            {
                this._wpfControl.WpfShowRefMapDelButton(false);
                this.elementHost.Child = _wpfControl.WpfGetCanvas(this.panelForWpfControl);
            }
            else if (this._MapMode == MapMode.Edit)
            {
                this._wpfControl.WpfShowRefMapDelButton(true);
                elementHost.Child = null;
                // Add canvas again to wpfControl
                _wpfControl.WpfSetCanvas();
                // Add wpfControl to ctrlHost
                elementHost.Child = _wpfControl;
            }

            // setting the controls to the default of the collection event
            switch (this._SelectedGisObject)
            {
                case SelectedGisObject.Organism:
                    if (this.DataRowIdentificationUnitGeoAnalysisGeo == null)
                        this.toolStripMenuItemStateEvent_Click(null, null);
                    break;
                case SelectedGisObject.Series:
                    if (this.DataRowCollectionEventSeriesGeo == null)
                        this.toolStripMenuItemStateEvent_Click(null, null);
                    break;
            }


        }

        private void DisplayGeoObjects(bool StartNewMap = false)
        {
            if (this._SelectedGisObject == SelectedGisObject.Distribution ||
                this._SelectedGisObject == SelectedGisObject.DistributionIncludingOrganisms)
            {
                if (this._MapMode == MapMode.Browse)
                {
                    if (this._DtDistributionBrowser == null
                        || this._DtDistributionBrowser.Rows.Count < 2)
                    {
                        this.FillDistributionTableBrowser();
                    }
                }
                else
                {

                }
            }
            else if (this._SelectedGisObject == SelectedGisObject.AnyCoordinate)
            {
                if (this._MapMode == MapMode.Browse)
                {
                    //if (this._DtDistributionBrowser == null
                    //    || this._DtDistributionBrowser.Rows.Count < 2)
                    //{
                    //    this.FillDistributionTableBrowser();
                    //}
                }
            }
            else
                this.SetGeoObject();

            #region alter Code
            //this._GeoObject = new GeoObject();
            //try
            //{
            //    // setting the selected GIS Object
            //    switch (this._SelectedGisObject)
            //    {
            //        case SelectedGisObject.Distribution:
            //            if (this._MapMode == MapMode.Browse)
            //            {
            //                if (this._DtDistributionBrowser == null
            //                    || this._DtDistributionBrowser.Rows.Count < 2)
            //                {
            //                    this.FillDistributionTableBrowser();
            //                }
            //            }
            //            else
            //            {

            //            }
            //            break;
            //        case SelectedGisObject.Series:
            //            if (this.DataRowCollectionEventSeriesGeo != null)
            //            {
            //                if (!this.DataRowCollectionEventSeriesGeo["GeographyAsString"].Equals(System.DBNull.Value)
            //                   && this.DataRowCollectionEventSeriesGeo["GeographyAsString"].ToString().Length > 0)
            //                {
            //                    string DisplayText = "";
            //                    if (!this.DataRowCollectionEventSeriesGeo["SeriesCode"].Equals(System.DBNull.Value)
            //                        && this.DataRowCollectionEventSeriesGeo["SeriesCode"].ToString().Length > 0)
            //                        DisplayText = this.DataRowCollectionEventSeriesGeo["SeriesCode"].ToString();
            //                    else DisplayText = this.DataRowCollectionEventSeriesGeo["Description"].ToString();
            //                    if (DisplayText.Length > 20) DisplayText = DisplayText.Substring(0, 20) + "...";

            //                    this._GeoObject = this.setupGeoObject(this.DataRowCollectionEventSeriesGeo["SeriesID"].ToString()
            //                        , DisplayText
            //                        , this.DataRowCollectionEventSeriesGeo["GeographyAsString"].ToString()
            //                        , this.Brush);
            //                }
            //            }
            //            break;
            //        case SelectedGisObject.Event:
            //            if (this.DataRowCollectionEventLocalisationGeo != null)
            //            {
            //                if (!this.DataRowCollectionEventLocalisationGeo["GeographyAsString"].Equals(System.DBNull.Value)
            //                    && this.DataRowCollectionEventLocalisationGeo["GeographyAsString"].ToString().Length > 0)
            //                {
            //                    string DisplayText = DiversityCollection.LookupTable.LocalisationSystemName(int.Parse(this.DataRowCollectionEventLocalisationGeo["LocalisationSystemID"].ToString()));
            //                    if (DisplayText.Length > 20) DisplayText = DisplayText.Substring(0, 20) + "...";

            //                    this._GeoObject = this.setupGeoObject(this.DataRowCollectionEventLocalisationGeo["CollectionEventID"].ToString()
            //                        , DisplayText
            //                        , this.DataRowCollectionEventLocalisationGeo["GeographyAsString"].ToString()
            //                        , this.Brush);
            //                }
            //            }
            //            break;
            //        case SelectedGisObject.Organism:
            //            if (this.DataRowIdentificationUnitGeoAnalysisGeo != null)
            //            {
            //                if (!this.DataRowIdentificationUnitGeoAnalysisGeo["GeographyAsString"].Equals(System.DBNull.Value)
            //                    && this.DataRowIdentificationUnitGeoAnalysisGeo["GeographyAsString"].ToString().Length > 0)
            //                {
            //                    this._GeoObject = this.setupGeoObject(this.DataRowIdentificationUnitGeoAnalysisGeo["IdentificationUnitID"].ToString()
            //                        , this.DataRowIdentificationUnitGeoAnalysisGeo["AnalysisDate"].ToString()
            //                        , this.DataRowIdentificationUnitGeoAnalysisGeo["GeographyAsString"].ToString()
            //                        , this.Brush);
            //                }
            //                else 
            //                    this._GeoObject = new GeoObject();
            //            }
            //            break;
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //}
            #endregion

            if (this._SelectedGisObject != SelectedGisObject.Distribution 
                && this._SelectedGisObject != SelectedGisObject.DistributionIncludingOrganisms
                && this._SelectedGisObject != SelectedGisObject.AnyCoordinate)
            {
                if (this._GeoObject.GeometryData != null
                && this._GeoObject.GeometryData.Length > 0)
                {
                    switch (this._MapMode)
                    {
                        case MapMode.Browse:
                            this.setMapInBrowser(this._GeoObject);
                            break;
                        case MapMode.Edit:
                            this._GeoObjectsEditor = new List<GeoObject>();
                            this._GeoObjectsEditor.Add(this._GeoObject);

                            this._wpfControl.WpfClearAllSamples(!this.MapIsFixed);
                            if (this.checkBoxZoomLevel.Checked)
                                this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor, (int)this.numericUpDownZoomLevel.Value);
                            else
                            {
                                if (this.MapIsFixed)
                                    this._wpfControl.WpfSetGeoObjects(this._GeoObjectsEditor);
                                else
                                    this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
                            }
                            this._wpfControl.WpfAddSample();
                            break;
                        case MapMode.View:
                            this._GeoObjectsEditor = new List<GeoObject>();
                            this._GeoObjectsEditor.Add(this._GeoObject);

                            this._wpfControl.WpfClearAllSamples(!this.MapIsFixed);
                            if (this.checkBoxZoomLevel.Checked)
                                this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor, (int)this.numericUpDownZoomLevel.Value);
                            else
                            {
                                if (this.MapIsFixed)
                                    this._wpfControl.WpfSetGeoObjects(this._GeoObjectsEditor);
                                else
                                    this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
                            }
                            this._wpfControl.WpfAddSample();
                            break;
                        default:
                            this._GeoObjectsEditor = new List<GeoObject>();
                            this._GeoObjectsEditor.Add(this._GeoObject);

                            this._wpfControl.WpfClearAllSamples(!this.MapIsFixed);
                            if (this.checkBoxZoomLevel.Checked)
                                this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor, (int)this.numericUpDownZoomLevel.Value);
                            else
                            {
                                if (this.MapIsFixed)
                                    this._wpfControl.WpfSetGeoObjects(this._GeoObjectsEditor);
                                else
                                    this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
                            }
                            this._wpfControl.WpfAddSample();
                            break;
                    }
                }
                else // NO Distribution + NO Data
                {
                    switch (this._MapMode)
                    {
                        case MapMode.Browse:
                            this.setMapInBrowser("");
                            break;
                        default:
                            this._GeoObjectsEditor = new List<GeoObject>();
                            this._GeoObjectsEditor.Add(this._GeoObject);

                            this._wpfControl.WpfClearAllSamples(!this.MapIsFixed);
                            //this._wpfControl.WpfSetMapAndGeoObjects(this._GeoObjects, (int)this.numericUpDownZoomLevel.Value);
                            //this._wpfControl.WpfAddSample();
                            break;
                    }

                }
            }
            else if (this._SelectedGisObject == SelectedGisObject.AnyCoordinate)
            {
                this.FillGeoObjectListEditor(SelectedGisObject.AnyCoordinate);
                this._wpfControl.WpfClearAllSamples(!this.MapIsFixed);
                if (this.checkBoxZoomLevel.Checked)
                {
                    this._wpfControl.WpfSetMapAndGeoObjects(this._DistributionObjectListEditor, (int)this.numericUpDownZoomLevel.Value);
                }
                else
                {
                    if (this.MapIsFixed)
                        this._wpfControl.WpfSetGeoObjects(this._DistributionObjectListEditor);
                    else
                        this._wpfControl.WpfSetMapAndGeoObjects(this._DistributionObjectListEditor);//._GeoObjectsEditor);
                }
            }
            else // Distribution
            {
                if (this.DistributionObjectListEditor.Count == 0)
                {
                    this.FillGeoObjectListEditor(this._SelectedGisObject);
                }
                else if (StartNewMap)// System.Windows.Forms.MessageBox.Show("Do you want to start a new distribution map", "New map?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if(this._DistributionObjectListEditor != null)
                        this._DistributionObjectListEditor.Clear();
                    if (this._DtDistributionBrowser != null)
                        this._DtDistributionBrowser.Clear();
                    if (this._DistributionObjectListBrowser != null)
                        this._DistributionObjectListBrowser.Clear();

                    this.FillGeoObjectListEditor(this._SelectedGisObject);
                    this.FillDistributionTableBrowser(true);
                }
                else
                {
                    return;
                }
                switch (this._MapMode)
                {
                    case MapMode.Browse:
                        this.setMapInBrowser(this.DistributionObjectListBrowser);
                        break;
                    case MapMode.View:
                        if (this._DistributionObjectListEditor.Count == 0)
                        {
                            this._wpfControl.WpfClearAllSamples(true);
                        }
                        else
                        {
                            this._wpfControl.WpfClearAllSamples(!this.MapIsFixed);
                            if (this.checkBoxZoomLevel.Checked)
                                this._wpfControl.WpfSetMapAndGeoObjects(this._DistributionObjectListEditor, (int)this.numericUpDownZoomLevel.Value);
                            else
                            {
                                if (this.MapIsFixed)
                                    this._wpfControl.WpfSetGeoObjects(this._DistributionObjectListEditor);
                                else
                                    this._wpfControl.WpfSetMapAndGeoObjects(this._DistributionObjectListEditor);
                            }
                            this._wpfControl.WpfAddSample();
                        }
                        break;
                    case MapMode.Edit:
                        if (this._DistributionObjectListEditor.Count == 0)
                        {
                            this._wpfControl.WpfClearAllSamples(true);
                        }
                        else
                        {
                            this._wpfControl.WpfClearAllSamples(!this.MapIsFixed);
                            if (this.checkBoxZoomLevel.Checked)
                                this._wpfControl.WpfSetMapAndGeoObjects(this._DistributionObjectListEditor, (int)this.numericUpDownZoomLevel.Value);
                            else
                            {
                                if (this.MapIsFixed)
                                    this._wpfControl.WpfSetGeoObjects(this._DistributionObjectListEditor);
                                else
                                    this._wpfControl.WpfSetMapAndGeoObjects(this._DistributionObjectListEditor);
                            }
                            this._wpfControl.WpfAddSample();
                        }
                        break;
                    default:
                        if (this._DistributionObjectListEditor.Count == 0)
                        {
                            this._wpfControl.WpfClearAllSamples(true);
                        }
                        else
                        {

                            this._wpfControl.WpfClearAllSamples(!this.MapIsFixed);
                            if (this.checkBoxZoomLevel.Checked)
                                this._wpfControl.WpfSetMapAndGeoObjects(this._DistributionObjectListEditor, (int)this.numericUpDownZoomLevel.Value);
                            else
                            {
                                if (this.MapIsFixed)
                                    this._wpfControl.WpfSetGeoObjects(this._DistributionObjectListEditor);
                                else
                                    this._wpfControl.WpfSetMapAndGeoObjects(this._DistributionObjectListEditor);
                            }
                            this._wpfControl.WpfAddSample();
                        }
                        break;
                }
            }
        }

        private void ResetGeography()
        {
            this._DtDistributionBrowser = null;
            this._GeoObjectsEditor = null;
        }

        private void SetGeoObject()
        {
            this._GeoObject = new GeoObject();
            try
            {
                // setting the selected GIS Object
                switch (this._SelectedGisObject)
                {
                    case SelectedGisObject.Series:
                        if (this.DataRowCollectionEventSeriesGeo != null)
                        {
                            if (!this.DataRowCollectionEventSeriesGeo["GeographyAsString"].Equals(System.DBNull.Value)
                               && this.DataRowCollectionEventSeriesGeo["GeographyAsString"].ToString().Length > 0)
                            {
                                string DisplayText = "";
                                if (!this.DataRowCollectionEventSeriesGeo["SeriesCode"].Equals(System.DBNull.Value)
                                    && this.DataRowCollectionEventSeriesGeo["SeriesCode"].ToString().Length > 0)
                                    DisplayText = this.DataRowCollectionEventSeriesGeo["SeriesCode"].ToString();
                                else DisplayText = this.DataRowCollectionEventSeriesGeo["Description"].ToString();
                                if (DisplayText.Length > 20) DisplayText = DisplayText.Substring(0, 20) + "...";

                                this._GeoObject = this.setupGeoObject(this.DataRowCollectionEventSeriesGeo["SeriesID"].ToString()
                                    , DisplayText
                                    , this.DataRowCollectionEventSeriesGeo["GeographyAsString"].ToString()
                                    , this.Brush);
                            }
                        }
                        break;
                    case SelectedGisObject.Event:
                        if (this.DataRowCollectionEventLocalisationGeo != null)
                        {
                            if (!this.DataRowCollectionEventLocalisationGeo["GeographyAsString"].Equals(System.DBNull.Value)
                                && this.DataRowCollectionEventLocalisationGeo["GeographyAsString"].ToString().Length > 0)
                            {
                                string DisplayText = DiversityCollection.LookupTable.LocalisationSystemName(int.Parse(this.DataRowCollectionEventLocalisationGeo["LocalisationSystemID"].ToString()));
                                if (DisplayText.Length > 20) DisplayText = DisplayText.Substring(0, 20) + "...";

                                this._GeoObject = this.setupGeoObject(this.DataRowCollectionEventLocalisationGeo["CollectionEventID"].ToString()
                                    , DisplayText
                                    , this.DataRowCollectionEventLocalisationGeo["GeographyAsString"].ToString()
                                    , this.Brush);
                            }
                        }
                        break;
                    case SelectedGisObject.Organism:
                        if (this.DataRowIdentificationUnitGeoAnalysisGeo != null)
                        {
                            if (!this.DataRowIdentificationUnitGeoAnalysisGeo["GeographyAsString"].Equals(System.DBNull.Value)
                                && this.DataRowIdentificationUnitGeoAnalysisGeo["GeographyAsString"].ToString().Length > 0)
                            {
                                this._GeoObject = this.setupGeoObject(this.DataRowIdentificationUnitGeoAnalysisGeo["IdentificationUnitID"].ToString()
                                    , this.DataRowIdentificationUnitGeoAnalysisGeo["AnalysisDate"].ToString()
                                    , this.DataRowIdentificationUnitGeoAnalysisGeo["GeographyAsString"].ToString()
                                    , this.Brush);
                            }
                            else
                                this._GeoObject = new GeoObject();
                        }
                        break;
                    //case SelectedGisObject.AnyCoordinate:
                    //    if (this.DataRowCollectionEventSeriesGeo != null)
                    //    {
                    //        if (!this.DataRowCollectionEventSeriesGeo["GeographyAsString"].Equals(System.DBNull.Value)
                    //           && this.DataRowCollectionEventSeriesGeo["GeographyAsString"].ToString().Length > 0)
                    //        {
                    //            string DisplayText = "";
                    //            if (!this.DataRowCollectionEventSeriesGeo["SeriesCode"].Equals(System.DBNull.Value)
                    //                && this.DataRowCollectionEventSeriesGeo["SeriesCode"].ToString().Length > 0)
                    //                DisplayText = this.DataRowCollectionEventSeriesGeo["SeriesCode"].ToString();
                    //            else DisplayText = this.DataRowCollectionEventSeriesGeo["Description"].ToString();
                    //            if (DisplayText.Length > 20) DisplayText = DisplayText.Substring(0, 20) + "...";

                    //            this._GeoObject = this.setupGeoObject(this.DataRowCollectionEventSeriesGeo["SeriesID"].ToString()
                    //                , DisplayText
                    //                , this.DataRowCollectionEventSeriesGeo["GeographyAsString"].ToString()
                    //                , this.Brush);
                    //        }
                    //    }
                    //    break;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region WPF Control 

        /// <summary>
        /// initialize the WPF control
        /// </summary>
        private void initWPFcontrol()
        {
            // Create WPF control instance
            _wpfControl = new WpfSamplingPlotPage.WpfControl(DiversityWorkbench.Settings.Language);
            this._wpfControl.WpfShowRefMapDelButton(false);
            // hide the delete button of the backgroud map
            //_wpfControl.WpfShowRefMapDelButton(false);
            // Add to our Form
            elementHost.Child = _wpfControl;
        }
        
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
        
        #endregion

        #region Browser based map

        /// <summary>
        /// setting the map in the browser
        /// </summary>
        /// <param name="URL"></param>
        private void setMapInBrowser(WpfSamplingPlotPage.GeoObject GeoObject)
        {
            string URL = this.GetCoordinatesOfGeoObject(GeoObject, true, true, true);
            if (URL.Length > 2000)
            {
                //System.Windows.Forms.MessageBox.Show("Too many items, please reduce selection");
                return;
            }
            if (URL.Length > 0)
            {
                try
                {
                    string URL_base = global::DiversityCollection.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
                    if (string.IsNullOrEmpty(URL_base))
                        return;
                    URL = URL_base+"?" + URL;
                    if (this.checkBoxZoomLevel.Checked & URL.IndexOf("&Zoom=") == -1)
                    {
                        //URL += "&Zoom=" + this.numericUpDownZoomLevel.Value.ToString();
                    }
                    System.Uri URI = new Uri(URL);
                    //this.web Browser.Url = URI;
                    this.userControlWebView.Url = null;
                    this.userControlWebView.Navigate(URI);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private string EnvelopeCenter(string Geography)
        {
            string SQL = "DECLARE @g geography = '" + Geography + "';  " +
                "SELECT @g.EnvelopeCenter().ToString();  ";
            string Point = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            return Point;
        }

        private void setMapInBrowser(System.Collections.Generic.List<GeoObject> GeoObjects)
        {
            string URL = this.GetCoordinatesOfGeoObject(GeoObjects);
            if (URL.Length > 2000)
            {
                System.Windows.Forms.MessageBox.Show("Too many items, please reduce selection");
                return;
            }
            if (URL.Length > 0)
            {
                try
                {
                    string URL_base = global::DiversityCollection.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
                    if (string.IsNullOrEmpty(URL_base))
                        return;
                    URL = URL_base + "?" + URL;
                    if (this.checkBoxZoomLevel.Checked & URL.IndexOf("&Zoom=") == -1)
                    {
                        URL += "&Zoom=" + this.numericUpDownZoomLevel.Value.ToString();
                    }

                    System.Uri URI = new Uri(URL);
                    //this.webB rowser.Url = URI;
                    this.userControlWebView.Url = null;
                    this.userControlWebView.Navigate(URI);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                //this.web Browser.Url = new Uri("about:blank");
                this.userControlWebView.Url = null;
                this.userControlWebView.Navigate(new Uri("about:blank"));
            }
        }

        /// <summary>
        /// setting the map in the browser
        /// </summary>
        /// <param name="URL"></param>
        private void setMapInBrowser(string URL)
        {
            if (URL.Length > 2000)
            {
                System.Windows.Forms.MessageBox.Show("Too many items, please reduce selection");
                return;
            }
            if (URL.Length > 0)
            {
                try
                {
                    string URL_base = global::DiversityCollection.Properties.Settings.Default.DiversityWorkbenchGoogleMapsSourceUri;
                    if (string.IsNullOrEmpty(URL_base))
                        return;
                    URL = URL_base + "?" + URL;
                    if (this.checkBoxZoomLevel.Checked & URL.IndexOf("&Zoom=") == -1)
                    {
                        URL += "&Zoom=" + this.numericUpDownZoomLevel.Value.ToString();
                    }
                    System.Uri URI = new Uri(URL);
                    //this.web Browser.Url = URI;
                    this.userControlWebView.Url = null;
                    this.userControlWebView.Navigate(URI);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else if (URL.Length == 0)
            {
                try
                {
                    URL = "about:blank";
                    System.Uri URI = new Uri(URL);
                    //this.web Browser.Url = URI;
                    this.userControlWebView.Url = null;
                    this.userControlWebView.Navigate(URI);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        /// <summary>
        /// Getting the coordinates for the URL if geo objects are shown via the browser and the webpage
        /// DiversityCollection_MapEvent.cfm
        /// </summary>
        /// <returns>the sting with the coordinates etc.</returns>
        private string GetCoordinatesOfURL()
        {
            string URL = "";
            string Long = "";
            string Lat = "";
            string Label = "";
            try
            {
                if (this._GeoObjectsEditor.Count > 0)
                {
                    for (int i = 0; i < this._GeoObjectsEditor.Count; i++)
                    {
                        string GeoData = this._GeoObjectsEditor[i].GeometryData;
                        if (!GeoData.ToUpper().StartsWith("POINT")) continue;
                        string[] GeoDataParts = GeoData.Split(new char[] { ' ' });
                        string sLong = GeoDataParts[1].Replace("(", "");
                        string sLat = GeoDataParts[2];
                        double dLong = 0;
                        double dLat = 0;
                        if (double.TryParse(sLong.Replace(".", ","), out dLong)
                            && double.TryParse(sLat.Replace(".", ","), out dLat)
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
                            Label += _GeoObjectsEditor[i].Identifier;
                        }
                    }
                }
                if (Long.Length > 0 && Lat.Length > 0 && Label.Length > 0)
                {
                    URL = "LatPoint=" + Lat
                            + "&LngPoint=" + Long
                            + "&Label=" + Label;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return URL;
        }

        private string GetCoordinatesOfGeoObject(GeoObject GeoObject, bool IncludeLabel = false, bool IncludeZoom = false, bool Center = false)
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
                else if (Center)
                {
                    string GeoData = this.EnvelopeCenter(GeoObject.GeometryData);
                    if (this.AddCoordinatesOfGeoPoint(GeoData, ref Lat, ref Long))
                        Label += GeoObject.Identifier;
                }
                else if (GeoObject.GeometryData.Length > 0
                    && (GeoObject.GeometryData.ToUpper().StartsWith("POLYGON")
                    ||GeoObject.GeometryData.ToUpper().StartsWith("LINESTRING")) )
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
                    string zoom = "15";
                    if (this.checkBoxZoomLevel.Checked)
                    {
                        zoom = this.numericUpDownZoomLevel.Value.ToString();
                    }

                    URL = "LatPoint=" + Lat
                            + "&LngPoint=" + Long;
                    if (IncludeLabel)
                        URL += "&Label=" + Label;
                    if (IncludeZoom)
                        URL += "&Zoom=" + zoom;
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
                    if (GeoObject.GeometryData.Length > 0)
                    {
                        if (GeoObject.GeometryData.ToUpper().StartsWith("POINT"))
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
                        else
                        {
                            double dLong = 0;
                            double dLat = 0;
                            string SQL = "select cast('" + GeoObject.GeometryData + "' as geography).EnvelopeCenter().Lat";
                            if (double.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out dLat))
                            {
                                SQL = "select cast('" + GeoObject.GeometryData + "' as geography).EnvelopeCenter().Long";
                                if (double.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out dLong)
                                    && dLong <= 180
                                    && dLong >= -180
                                    && dLat <= 90
                                    && dLat >= -90)
                                {
                                    if (Long.Length > 0)
                                    {
                                        Long += "|";
                                        Lat += "|";
                                        Label += "|";
                                    }
                                    Long += Math.Round(dLong, 4).ToString();
                                    Lat += Math.Round(dLat, 4).ToString();
                                    Label += GeoObject.Identifier;
                                }
                            }
                            //string SQL = "DECLARE @I int " +
                            //    "DECLARE @Geo geography " +
                            //    "SET @Geo = '" + GeoObject.GeometryData + "' " +
                            //    "SET @I = (SELECT @Geo.STNumPoints()); " +
                            //    "DECLARE @DtGeo TABLE (Point nvarchar(1000) COLLATE Latin1_General_CI_AS NOT NULL); " +
                            //    "WHILE @I > 0 " +
                            //    "BEGIN " +
                            //    "INSERT INTO @DtGeo (Point) SELECT @Geo.STPointN(@I).ToString() " +
                            //    "SET @I = @I - 1 " +
                            //    "END " +
                            //    "SELECT * FROM @DtGeo;";
                            //System.Data.DataTable dtPolyPoints = new DataTable();
                            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            //ad.Fill(dtPolyPoints);
                            //foreach (System.Data.DataRow R in dtPolyPoints.Rows)
                            //{
                            //    if (this.AddCoordinatesOfGeoPoint(R[0].ToString(), ref Lat, ref Long))
                            //    {
                            //        if (Label.Length > 0)
                            //            Label += "|";
                            //        Label += GeoObject.DisplayText;
                            //    }
                            //}
                        }
                    }
                }
                if (Long.Length > 0 && Lat.Length > 0 && Label.Length > 0)
                {
                    URL = "LatPoint=" + Lat
                            + "&LngPoint=" + Long
                            + "&Label=" + Label;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return URL;
        }
        
        #endregion

        #region GeoObjects
        /// <summary>
        /// Fill up GeoObject structure...
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
            geoObject.StrokeTransparency = this.StrokeTransparency;
            geoObject.FillTransparency = this.FillTransparency;
            geoObject.StrokeThickness = this.StrokeThickness;
            geoObject.PointType = this.Symbol;
            geoObject.PointSymbolSize = this.SymbolSize;
            geoObject.GeometryData = data;
            return geoObject;
        }

        private GeoObject setupGeoObject(string Identifier, string DisplayText, string data)
        {
            GeoObject geoObject = new GeoObject();
            geoObject.Identifier = Identifier;
            geoObject.DisplayText = DisplayText;
            geoObject.StrokeBrush = this.Brush;
            geoObject.FillBrush = this.Brush;
            geoObject.StrokeTransparency = this.StrokeTransparency;
            geoObject.FillTransparency = this.FillTransparency;
            geoObject.StrokeThickness = this.StrokeThickness;
            geoObject.PointType = this.Symbol;
            geoObject.PointSymbolSize = this.SymbolSize;
            geoObject.GeometryData = data;
            return geoObject;
        }
        
        #endregion

        #region Tool strip buttons and Menu buttons

        #region Menu items for setting the state (Organism, ... Distribution)

        private void toolStripMenuItemStateOrganism_Click(object sender, EventArgs e)
        {
            this._SelectedGisObject = SelectedGisObject.Organism;
            this.setSelectedGisObject(this.toolStripMenuItemStateOrganism);
        }

        private void toolStripMenuItemStateEvent_Click(object sender, EventArgs e)
        {
            this._SelectedGisObject = SelectedGisObject.Event;
            this.setSelectedGisObject(this.toolStripMenuItemStateEvent);
        }

        private void toolStripMenuItemStateSeries_Click(object sender, EventArgs e)
        {
            this._SelectedGisObject = SelectedGisObject.Series;
            this.setSelectedGisObject(this.toolStripMenuItemStateSeries);
        }

        private void anyCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._SelectedGisObject = SelectedGisObject.AnyCoordinate;
            this.setSelectedGisObject(this.anyCoordinatesToolStripMenuItem);
        }

        private void toolStripMenuItemStateDistribution_Click(object sender, EventArgs e)
        {
            this._SelectedGisObject = SelectedGisObject.Distribution;
            this.setSelectedGisObject(this.toolStripMenuItemStateDistribution);
        }

        private void distInclOrgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._SelectedGisObject = SelectedGisObject.DistributionIncludingOrganisms;
            this.setSelectedGisObject(this.distInclOrgToolStripMenuItem);
        }

        private void setSelectedGisObject(System.Windows.Forms.ToolStripMenuItem MenuItem)
        {
            this.toolStripDropDownButtonState.Image = MenuItem.Image;
            this.toolStripDropDownButtonState.Text = MenuItem.Text;
            _wpfControl.WpfSetMode(2);
            _wpfControl.WpfClearAllSamples(!this.MapIsFixed);
            this.setVisibilityOfControls();
            this.DisplayGeoObjects();
            if (this.ParentForm != null)
            {
                DiversityCollection.Forms.FormCollectionSpecimen F = (DiversityCollection.Forms.FormCollectionSpecimen)this.ParentForm;
                F.setHeader();
            }
            if (this._SelectedGisObject == SelectedGisObject.Distribution ||
                this._SelectedGisObject == SelectedGisObject.DistributionIncludingOrganisms)
                this.buttonCustomizeMapLocalisations.Visible = true;
            else
                this.buttonCustomizeMapLocalisations.Visible = false;
        }
        
        #endregion

        #region toolstrip buttons for setting the map mode (Browser ... Editor)

        private void toolStripMenuItemEditModeBrowser_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonEditMode.Image = this.toolStripMenuItemEditModeBrowser.Image;
            this.toolStripDropDownButtonEditMode.Text = this.toolStripMenuItemEditModeBrowser.Text;
            this.toolStripDropDownButtonEditMode.ToolTipText = "Use browser to display the map";
            this._MapMode = MapMode.Browse;
            this.setVisibilityOfControls();
            if (this._SelectedGisObject == SelectedGisObject.Distribution || this._SelectedGisObject == SelectedGisObject.DistributionIncludingOrganisms)
                this.setMapInBrowser(this.DistributionObjectListBrowser);
            else if (this._SelectedGisObject == SelectedGisObject.Organism)
            {
                List<GeoObject> GeoObjectListBrowser = new List<GeoObject>();
                if (this._DataRowIdentificationUnitGeoAnalysisGeo != null)
                {
                    GeoObjectListBrowser.Add(this.setupGeoObject("Unit", "Unit", this._DataRowIdentificationUnitGeoAnalysisGeo["GeographyAsString"].ToString()));
                    this.setMapInBrowser(GeoObjectListBrowser);
                }
            }
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
        
        #endregion

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (this._GeoObjectsEditor == null)
                this._GeoObjectsEditor = new List<GeoObject>();
            if (!this._wpfControl.WpfGetGeoObjects(out this._GeoObjectsEditor))
                return;
            if (this._GeoObjectsEditor.Count == 1
                && this._GeoObjectsEditor[0].GeometryData != null
                && this._GeoObjectsEditor[0].GeometryData.Length > 0)
            {
                string Table = "";
                string WhereClause = "";
                string Geography = "";
                switch (this._SelectedGisObject)
                {
                    case SelectedGisObject.Event:
                        if (this._DataRowCollectionEventLocalisationGeo == null) 
                            return;
                        this._DataRowCollectionEventLocalisationGeo["GeographyAsString"] = this._GeoObjectsEditor[0].GeometryData;
                        Geography = this._GeoObjectsEditor[0].GeometryData;
                        Table = "CollectionEventLocalisation";
                        WhereClause = " CollectionEventID = " + this._DataRowCollectionEventLocalisationGeo["CollectionEventID"].ToString() +
                            " AND LocalisationSystemID = " + this._DataRowCollectionEventLocalisationGeo["LocalisationSystemID"];
                        break;
                    case SelectedGisObject.Organism:
                        //this._DataRowIdentificationUnitGeoAnalysis["GeographyAsString"] = this._GeoObjects[0].GeometryData;
                        Geography = this._GeoObjectsEditor[0].GeometryData;
                        Table = "IdentificationUnitGeoAnalysis";
                        System.DateTime DateTime;
                        System.DateTime.TryParse(this._DataRowIdentificationUnitGeoAnalysisGeo["AnalysisDate"].ToString(), out DateTime);
                        //string Date = DateTime.ToString("yyyy-MM-dd HH:mm:ss");//.Year.ToString() + "-";
                        //if (DateTime.Month < 10) Date += "0";
                        //Date += DateTime.Month.ToString() + "-";
                        //if (DateTime.Day < 10) Date += "0";
                        //Date += DateTime.Day.ToString();
                        //if (DateTime.Hour < 10) Date += "0";
                        //Date += " " + DateTime.Hour.ToString() + ":";
                        //if (DateTime.Minute < 10) Date += "0";
                        //Date += DateTime.Minute.ToString() + ":";
                        //if (DateTime.Second < 10) Date += "0";
                        //Date += DateTime.Second.ToString();
                        WhereClause = " IdentificationUnitID = " + this._DataRowIdentificationUnitGeoAnalysisGeo["IdentificationUnitID"].ToString() +
                            " AND CollectionSpecimenID = " + this._DataRowIdentificationUnitGeoAnalysisGeo["CollectionSpecimenID"] +
                            " and YEAR(AnalysisDate) = " + DateTime.Year.ToString() +
                            " and MONTH(AnalysisDate) = " + DateTime.Month.ToString() +
                            " and DAY(AnalysisDate) = " + DateTime.Day.ToString() +
                            " and DATEPART(hour, AnalysisDate) = " + DateTime.Hour.ToString() +
                            " and DATEPART(minute, AnalysisDate) = " + DateTime.Minute.ToString() +
                            " and DATEPART(second, AnalysisDate) = " + DateTime.Second.ToString();
                        break;
                    case SelectedGisObject.Series:

                        //this._DataRowCollectionEventSeries["Geography"] = this._GeoObjects[0].GeometryData;
                        Geography = this._GeoObjectsEditor[0].GeometryData;
                        Table = "CollectionEventSeries";
                        WhereClause = " SeriesID = " + this._DataRowCollectionEventSeriesGeo["SeriesID"].ToString();
                        break;
                }
                string SQL = "UPDATE " + Table + " SET Geography = '" + Geography + "' WHERE " + WhereClause;
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                if (Table == "CollectionEventLocalisation")
                {
                    SQL = "UPDATE L SET " +
                    "L.Location1 = case when L.Location1 = null OR isnumeric(L.Location1) = 0 then L.Geography.EnvelopeCenter().Long else L.Location1 end " +
                    ", L.Location2 = case when L.Location1 = null OR isnumeric(L.Location1) = 0 then L.Geography.EnvelopeCenter().Lat else L.Location2 end " +
                    ", L.AverageLongitudeCache = case when L.Location1 = null OR isnumeric(L.Location1) = 0 then L.Geography.EnvelopeCenter().Long else L.AverageLongitudeCache end " +
                    ", L.AverageLatitudeCache = case when L.Location1 = null OR isnumeric(L.Location1) = 0 then L.Geography.EnvelopeCenter().Lat else L.AverageLatitudeCache end " +
                    "from CollectionEventLocalisation L WHERE " + WhereClause;
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            this.DisplayGeoObjects();
        }

        private void numericUpDownZoomLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!this.checkBoxZoomLevel.Checked)
                this.checkBoxZoomLevel.Checked = true;
        }

        private void buttonMapIsFixed_Click(object sender, EventArgs e)
        {
            this.MapIsFixed = !this.MapIsFixed;
        }

        private void buttonCustomizeMapLocalisations_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCustomizeDisplay f = new Forms.FormCustomizeDisplay(DiversityCollection.Specimen.ImageList, Forms.FormCustomizeDisplay.Customization.MapLocalisations);
            f.setHelp("Customize");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (f.VisibilityOfMapsHasChanges)
                {
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystemsInMaps = f.VisibilityMaps;
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                }
            }
        }

        #endregion

        #region Distribution related functions

        private void toolStripButtonRequeryDistribution_Click(object sender, EventArgs e)
        {
            // clear old entries
            this._DtDistributionBrowser = null;
            this._DistributionObjectListEditor.Clear();
            // insert new entries
            this.FillDistributionTableBrowser();
            this.FillGeoObjectListEditor(SelectedGisObject.Distribution);
            // show the entries
            this.DisplayGeoObjects(true);
        }

        private void toolStripButtonAddToDistribution_Click(object sender, EventArgs e)
        {
            this.FillDistributionTableBrowser();
            this.FillGeoObjectListEditor(SelectedGisObject.Distribution);
            this.DisplayGeoObjects();
        }

        private void FillGeoObjectListEditor(SelectedGisObject Target)
        {
            try
            {
                if (this._DistributionObjectListEditor == null)
                    this._DistributionObjectListEditor = new List<GeoObject>();
                DiversityCollection.Forms.FormCollectionSpecimen F = (DiversityCollection.Forms.FormCollectionSpecimen)this.ParentForm;
                if (Target == SelectedGisObject.Distribution || Target == SelectedGisObject.DistributionIncludingOrganisms)
                {
                    if (!F.IDs.Contains(F.selectedIDs[0]))
                    {
                        F.ResetIDs();
                    }
                    if (F.IDs.Count > 0)
                    {
                        bool IncludeAll = false;
                        if (F.IDs.Count > 1 && F.selectedIDs.Count == 1)
                        {
                            if (this.checkBoxIncludeAll.Checked)// System.Windows.Forms.MessageBox.Show("Only one item has been selected. Do you want to include all items from the list", "All items?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this._SpecimenIDsForDistribution = new List<int>();
                                foreach (int i in F.IDs)
                                    this._SpecimenIDsForDistribution.Add(i);
                                IncludeAll = true;
                            }
                            else
                            {
                                this._SpecimenIDsForDistribution = new List<int>();
                                this._SpecimenIDsForDistribution.Add(F.selectedIDs[0]);
                            }
                        }

                        bool ContainsNewSpecimen = false;
                        string SQL = this.SqlDistributionCoordiates(ref ContainsNewSpecimen, IncludeAll);

                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                        System.Data.DataTable dtNewSpecimen = new DataTable();
                        ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                        try
                        {
                            ad.Fill(dtNewSpecimen);
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                        foreach (System.Data.DataRow R in dtNewSpecimen.Rows)
                        {
                            GeoObject GO = new GeoObject();

                            GO.Identifier = R["Identifier"].ToString();
                            GO.DisplayText = R["DisplayText"].ToString();
                            GO.GeometryData = R["Geography"].ToString();
                            GO.FillBrush = this.FillBrush;
                            GO.FillTransparency = this.FillTransparency;
                            GO.PointSymbolSize = this.PointSymbolSize;
                            GO.PointType = this.PointType;
                            GO.StrokeBrush = this.StrokeBrush;
                            GO.StrokeThickness = this.StrokeThickness;
                            GO.StrokeTransparency = this.StrokeTransparency;

                            this._DistributionObjectListEditor.Add(GO);
                        }
                    }
                }
                else if (Target == SelectedGisObject.AnyCoordinate)
                {
                    this._DistributionObjectListEditor.Clear();
                    string SQL = "";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);

                    // Get Series
                    if (F.SeriesID != null)
                    {
                        int? SeriesID = F.SeriesID;
                        System.Data.DataTable dtSeries = new DataTable();
                        while (SeriesID != null)
                        {
                            int i = 0;
                            SQL = "SELECT CASE WHEN [SeriesCode] IS NULL OR RTRIM([SeriesCode]) = '' THEN CASE WHEN [Description] IS NULL OR RTRIM([Description]) = '' THEN '[' + CAST(S.SeriesID AS varchar) + ')' END ELSE [SeriesCode] END AS DisplayText " +
                                ",[Geography].ToString() AS [Geography], S.SeriesID, S.SeriesParentID, " + i.ToString() + " AS Level " +
                                "FROM [dbo].[CollectionEventSeries] " +
                                "S  WHERE S.SeriesID = " + SeriesID.ToString();
                            ad.SelectCommand.CommandText = SQL;
                            ad.Fill(dtSeries);
                            System.Data.DataRow[] rr = dtSeries.Select("SeriesID = " + SeriesID.ToString());
                            if (!rr[0]["SeriesParentID"].Equals(System.DBNull.Value))
                                SeriesID = int.Parse(rr[0]["SeriesParentID"].ToString());
                            else
                                break;
                            i++;
                        }
                        System.Data.DataRow[] RR = dtSeries.Select("Geography <> '' ");
                        System.Windows.Media.Brush SeriesBrush = System.Windows.Media.Brushes.Blue;
                        foreach (System.Data.DataRow R in RR)
                        {
                            GeoObject GO = new GeoObject();

                            GO.Identifier = R["SeriesID"].ToString();
                            GO.DisplayText = R["DisplayText"].ToString();
                            GO.GeometryData = R["Geography"].ToString();
                            GO.FillBrush = SeriesBrush;
                            GO.FillTransparency = this.FillTransparency;
                            GO.PointSymbolSize = this.PointSymbolSize;
                            GO.PointType = this.PointType;
                            GO.StrokeBrush = SeriesBrush;
                            GO.StrokeThickness = this.StrokeThickness;
                            GO.StrokeTransparency = this.StrokeTransparency;

                            this._DistributionObjectListEditor.Add(GO);
                        }
                    }

                    // Get Localisation
                    if (F.EventID > -1)
                    {
                        System.Data.DataTable dtLocality = new DataTable();
                        SQL = "SELECT E.CollectionEventID, L.DisplayText, E.Geography.ToString() AS [Geography] " +
                            "FROM CollectionEventLocalisation AS E INNER JOIN " +
                            "LocalisationSystem AS L ON L.LocalisationSystemID = E.LocalisationSystemID " +
                            "WHERE (NOT (E.Geography IS NULL)) AND  E.CollectionEventID = " + F.EventID.ToString();
                        ad.SelectCommand.CommandText = SQL;
                        ad.Fill(dtLocality);
                        foreach (System.Data.DataRow R in dtLocality.Rows)
                        {
                            GeoObject GO = new GeoObject();

                            GO.Identifier = R["CollectionEventID"].ToString();
                            GO.DisplayText = R["DisplayText"].ToString();
                            GO.GeometryData = R["Geography"].ToString();
                            GO.FillBrush = this.FillBrush;
                            GO.FillTransparency = this.FillTransparency;
                            GO.PointSymbolSize = this.PointSymbolSize;
                            GO.PointType = this.PointType;
                            GO.StrokeBrush = this.StrokeBrush;
                            GO.StrokeThickness = this.StrokeThickness;
                            GO.StrokeTransparency = this.StrokeTransparency;

                            this._DistributionObjectListEditor.Add(GO);
                        }
                    }

                    // Get Organisms
                    System.Data.DataTable dtUnits = new DataTable();
                    SQL = "SELECT G.IdentificationUnitID AS Identifier, U.LastIdentificationCache AS DisplayText, G.Geography.ToString() AS Geography " +
                        "FROM  IdentificationUnitGeoAnalysis AS G INNER JOIN " +
                        "IdentificationUnit U ON G.CollectionSpecimenID = U.CollectionSpecimenID AND U.IdentificationUnitID = G.IdentificationUnitID " +
                        "WHERE (NOT (G.Geography IS NULL)) AND G.CollectionSpecimenID = " + F.ID.ToString();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtUnits);
                    System.Windows.Media.Brush UnitBrush = System.Windows.Media.Brushes.Green;
                    foreach (System.Data.DataRow R in dtUnits.Rows)
                    {
                        GeoObject GO = new GeoObject();

                        GO.Identifier = R["Identifier"].ToString();
                        GO.DisplayText = R["DisplayText"].ToString();
                        GO.GeometryData = R["Geography"].ToString();
                        GO.FillBrush = UnitBrush;
                        GO.FillTransparency = this.FillTransparency;
                        GO.PointSymbolSize = this.PointSymbolSize;
                        GO.PointType = this.PointType;
                        GO.StrokeBrush = UnitBrush;
                        GO.StrokeThickness = this.StrokeThickness;
                        GO.StrokeTransparency = this.StrokeTransparency;

                        this._DistributionObjectListEditor.Add(GO);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void FillDistributionTableBrowser(bool CreateNewDistribution = false)
        {
            if (this._DtDistributionBrowser == null) this._DtDistributionBrowser = new DataTable();
            this._SpecimenIDsForDistribution = null;
            DiversityCollection.Forms.FormCollectionSpecimen F = (DiversityCollection.Forms.FormCollectionSpecimen)this.ParentForm;
            if (F.IDs.Count > 0)
            {
                bool ContainsNewSpecimen = CreateNewDistribution;
                string SQL = this.SqlDistributionCoordiates(ref ContainsNewSpecimen);
                if (ContainsNewSpecimen)
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    if (this._DtDistributionBrowser.Rows.Count == 0)
                        ad.Fill(this._DtDistributionBrowser);
                    else
                    {
                        System.Data.DataTable dtNewSpecimen = new DataTable();
                        ad.Fill(dtNewSpecimen);
                        foreach (System.Data.DataRow R in dtNewSpecimen.Rows)
                        {
                            if (!R["Latitude"].Equals(System.DBNull.Value) && !R["Longitude"].Equals(System.DBNull.Value))
                            {
                                string Restriction = "Latitude = " + R["Latitude"].ToString().Replace(",", ".") + " AND Longitude = " + R["Longitude"].ToString().Replace(",", ".");
                                System.Data.DataRow[] rr = this._DtDistributionBrowser.Select(Restriction, "");
                                if (rr.Length == 0)
                                {
                                    System.Data.DataRow Rn = this._DtDistributionBrowser.NewRow();
                                    for (int i = 0; i < Rn.Table.Columns.Count; i++)
                                        Rn[i] = R[i];
                                    this._DtDistributionBrowser.Rows.Add(Rn);
                                }
                            }
                        }
                    }
                }
            }
            else
                this._DtDistributionBrowser.Clear();
        }

        private string SqlDistributionCoordiates(ref bool ContainsNewSpecimen, bool IncludeAll = false)
        {
            string SQL = "";
            try
            {
                if (this._DtDistributionBrowser == null)
                    this.FillDistributionTableBrowser();
                string RestrictionClauseCollectionSpecimenID = "";
                if (this.SpecimenIDsForDistribution.Count > 0)
                {
                    foreach (int i in this.SpecimenIDsForDistribution)
                    {
                        if (RestrictionClauseCollectionSpecimenID.Length > 0)
                            RestrictionClauseCollectionSpecimenID += ", ";
                        RestrictionClauseCollectionSpecimenID += i.ToString();
                        if (this._DtDistributionBrowser.Rows.Count > 0)
                        {
                            System.Data.DataRow[] rr = this._DtDistributionBrowser.Select("Identifier = " + i.ToString());
                            if (rr.Length > 0) continue;
                            else ContainsNewSpecimen = true;
                        }
                        else
                            ContainsNewSpecimen = true;
                    }
                    RestrictionClauseCollectionSpecimenID = " AND S.CollectionSpecimenID IN (" + RestrictionClauseCollectionSpecimenID + ") ";
                }
                else
                {
                    RestrictionClauseCollectionSpecimenID = " AND S.CollectionSpecimenID IS NULL "; // Return nothing if no restriction to specimen is possible
                }

                SQL = "SELECT Distinct S.CollectionSpecimenID AS Identifier, " +
                "case when S.AccessionNumber is null or S.AccessionNumber = '' then cast(S.CollectionSpecimenID as varchar) else S.AccessionNumber end AS DisplayText, " +
                "L.Geography.ToString() AS Geography, round(L.Geography.EnvelopeCenter().Lat, 3) AS Latitude, round(L.Geography.EnvelopeCenter().Long, 3) AS Longitude " +
                "FROM CollectionSpecimen AS S INNER JOIN " +
                "CollectionEventLocalisation AS L ON S.CollectionEventID = L.CollectionEventID " +
                "WHERE L.Geography.ToString() <> 'POINT (0 0)' " + RestrictionClauseCollectionSpecimenID;

                // possible restriction of localisation systems
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystemsInMaps.Length > 0)
                {
                    string LocalisationSystems = "";
                    for (int i = 0; i < DiversityCollection.LookupTable.DtLocalisationSystem.Rows.Count; i++)
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystemsInMaps[i].ToString() == "1")
                        {
                            if (LocalisationSystems.Length > 0) LocalisationSystems += ", ";
                            LocalisationSystems += DiversityCollection.LookupTable.DtLocalisationSystem.Rows[i]["LocalisationSystemID"].ToString();
                        }
                    }
                    if (LocalisationSystems.Length > 0)
                        SQL += " AND L.LocalisationSystemID IN (" + LocalisationSystems + ") ";
                    else
                        SQL += " AND L.LocalisationSystemID IS NULL "; // = hide coordinates from event to shown only organisms
                }

                if (this._SelectedGisObject == SelectedGisObject.DistributionIncludingOrganisms)
                {
                    SQL += " UNION " +
                        "SELECT A.IdentificationUnitID AS Identifier, U.LastIdentificationCache AS DisplayText, " +
                        "A.Geography.ToString() AS Geography, ROUND(A.Geography.EnvelopeCenter().Lat, 3) AS Latitude, ROUND(A.Geography.EnvelopeCenter().Long, 3) AS Longitude " +
                        "FROM CollectionSpecimen AS S INNER JOIN IdentificationUnit AS U ON S.CollectionSpecimenID = U.CollectionSpecimenID " +
                        "INNER JOIN IdentificationUnitGeoAnalysis AS A ON U.CollectionSpecimenID = A.CollectionSpecimenID AND U.IdentificationUnitID = A.IdentificationUnitID " +
                        "WHERE A.Geography.ToString() <> 'POINT (0 0)' " + RestrictionClauseCollectionSpecimenID;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return SQL;
        }

        private System.Collections.Generic.List<int> _SpecimenIDsForDistribution;
        private System.Collections.Generic.List<int> SpecimenIDsForDistribution
        {
            get
            {
                if (this._SpecimenIDsForDistribution == null)
                {
                    DiversityCollection.Forms.FormCollectionSpecimen F = (DiversityCollection.Forms.FormCollectionSpecimen)this.ParentForm;
                    if (this.checkBoxIncludeAll.Checked)
                    {
                        this._SpecimenIDsForDistribution = F.IDs;
                    }
                    else
                    {
                        this._SpecimenIDsForDistribution = new List<int>();
                        this._SpecimenIDsForDistribution.Add(F.ID);
                    }
                }
                return this._SpecimenIDsForDistribution;
            }
        }

        //private void buttonDistributionMap_Click(object sender, EventArgs e)
        //{
        //    DiversityCollection.DistributionMap.FormDistributionMap f = new DistributionMap.FormDistributionMap();
        //    f.ShowDialog();
        //}

        #endregion

    }
}
