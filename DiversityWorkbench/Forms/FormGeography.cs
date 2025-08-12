using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WpfSamplingPlotPage;

namespace DiversityWorkbench.Forms
{

    public partial class FormGeography : Form
    {

        public enum ViewMode { Edit, View }
        private ViewMode _ViewMode = ViewMode.Edit;

        private WpfSamplingPlotPage.WpfControl _wpfControl;

        #region Construction

        public FormGeography()
        {
            InitializeComponent();
            initForm();
        }

        //public FormGeography(List<GeoObject> GeoObjects)
        //    : this()
        //{
        //    // Call interface method to transmit GeoObjects
        //    _wpfControl.WpfSetGeoObjects(GeoObjects);
        //}

        public FormGeography(List<GeoObject> GeoObjects)
            : this()
        {
            try
            {
                // Call interface method to transmit GeoObjects
                _wpfControl.WpfSetMapAndGeoObjects(GeoObjects);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //public FormGeography(List<GeoObject> GeoObjects, bool AddSamples)
        //    : this()
        //{
        //    _wpfControl.WpfSetSilentMode(true);
        //    // Call interface method to transmit GeoObjects
        //    _wpfControl.WpfSetMapAndGeoObjects(GeoObjects);
        //    if (AddSamples)
        //        _wpfControl.WpfAddSample();
        //}

        public FormGeography(List<GeoObject> GeoObjects, string MapPath)
            : this()
        {
            try
            {
                _wpfControl.WpfSetSilentMode(false);
                // Call interface method to transmit GeoObjects and map
                _wpfControl.WpfSetMapAndGeoObjects(GeoObjects, MapPath);
                _wpfControl.WpfAddSample();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public FormGeography(List<GeoObject> GeoObjects, string MapPath, string Title, ViewMode Mode)
        {
            try
            {
                InitializeComponent();
                this._ViewMode = Mode;
                this.Text = Title;
                switch(this._ViewMode)
                {
                    case ViewMode.Edit:
                        initForm();
                        _wpfControl.WpfSetSilentMode(false);
                        // Call interface method to transmit GeoObjects and map
                        _wpfControl.WpfSetMapAndGeoObjects(GeoObjects, MapPath);
                        _wpfControl.WpfAddSample();
                        break;
                    case ViewMode.View:
                        // Create WPF control instance
                        _wpfControl = new WpfSamplingPlotPage.WpfControl();

                        // Add to our Form
                        elementHostGeography.Child = _wpfControl;
                        this.elementHostGeography.Dock = DockStyle.Fill;

                        this.FormGeography_Paint(null, null);

                        this._wpfControl.WpfShowRefMapDelButton(false);
                        //this.elementHostGeography.Child = _wpfControl.WpfGetCanvas(this);
                        _wpfControl.WpfSetSilentMode(true);
                        // Call interface method to transmit GeoObjects and map
                        _wpfControl.WpfSetMapAndGeoObjects(GeoObjects, MapPath);
                        _wpfControl.WpfAddSample();
                        break;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private void initForm()
        {
            try
            {
                //elementHostGeography.Child = null;

                // Create WPF control instance
                _wpfControl = new WpfSamplingPlotPage.WpfControl();

                // Add to our Form
                elementHostGeography.Child = _wpfControl;
                this.elementHostGeography.Dock = DockStyle.Fill;

                this.FormGeography_Paint(null, null);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
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
        /// Call interface method to receive GeoObjects as a geometry collection
        /// </summary>
        public bool ReceiveGeoObjects(out List<GeoObject> GeoObjects)
        {
            // Call interface method to transmit GeoObjects
            return _wpfControl.WpfGetGeoObjects(out GeoObjects);
        }

        /// <summary>
        /// Call interface method to receive GeoObjects as list of single objects
        /// </summary>
        public bool ReceiveGeoObjectsAsList(out List<GeoObject> GeoObjects)
        {
            // Call interface method to transmit GeoObjects
            return _wpfControl.WpfGetGeoObjects(out GeoObjects, false);
        }

        /// <summary>
        /// If the frame in the map should be shown
        /// </summary>
        /// <param name="ShowFrame">Show it or not</param>
        public void ShowFrame(bool ShowFrame)
        {
            _wpfControl.WpfSetFrame(ShowFrame);
        }

        public string Geography
        {
            get
            {
                string Geo = "";
                List<GeoObject> GeoObjects;
                if (ReceiveGeoObjects(out GeoObjects))
                {
                    if (GeoObjects.Count > 0)
                    {
                        Geo = GeoObjects[0].GeometryData;
                    }
                }
                return Geo;
            }
        }

        private void FormGeography_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
