using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using WpfSamplingPlotPage;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlImage : UserControl
    {
        #region Parameter

        private System.Drawing.Color TransparentRed = System.Drawing.Color.FromArgb(40, System.Drawing.Color.Red);
        /// <summary>
        /// the factor for the zooming using the zoom in and zoom out buttons
        /// </summary>
        private float ZoomStep = (float)1.5;
        /// <summary>
        /// the minimum size of the picture
        /// </summary>
        private int MinPictureSize = 10;
        /// <summary>
        /// the maximum size of the picture
        /// </summary>
        private int MaxPictureSize = 5000;
        /// <summary>
        /// if the square for the selection of the zooming area is currently drawn
        /// </summary>
        private bool DrawingSquare = false;
        /// <summary>
        /// the point where the drawing of the zooming rectangle started
        /// </summary>
        private System.Drawing.Point StartPoint;
        /// <summary>
        /// the rectangle which will be zoomed
        /// </summary>
        private System.Drawing.Rectangle SelectedArea;
        /// <summary>
        /// the pen for drawing the square for the selection
        /// </summary>
        private System.Drawing.Pen SquarePen = new System.Drawing.Pen(System.Drawing.Color.Red, 1);
        private System.Drawing.Pen MarkingPen = new System.Drawing.Pen(System.Drawing.Color.Red, 3);
        private string _MarkingAreaDisplayText = "";

        private DiversityWorkbench.Forms.FormFunctions.Medium _MediumType;

        private bool _MarkingArea = false;

        #region Marking aereas via GIS editor
        /// <summary>
        /// the WPF control for the GIS operations
        /// </summary>
        private WpfSamplingPlotPage.WpfControl _wpfControl;

        private System.Windows.Forms.Integration.ElementHost elementHost;

        public WpfSamplingPlotPage.WpfControl WpfControl()
        {
            //get 
            //{
            if (this._wpfControl == null)
            {
                this.elementHost = new System.Windows.Forms.Integration.ElementHost();
                this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
                //this.elementHost.Location = new System.Drawing.Point(0, 0);
                //this.elementHost.Name = "elementHost";
                //this.elementHost.Size = new System.Drawing.Size(631, 627);
                //this.elementHost.TabIndex = 0;
                //this.elementHost.Text = "elementHost";
                this.panelForWpfControl.Controls.Add(this.elementHost);

                this._wpfControl = new WpfSamplingPlotPage.WpfControl();//DiversityWorkbench.Settings.Language);
                //this._wpfControl.WpfShowRefMapDelButton(false);
                this._wpfControl.WpfSetMode(2);
                this._wpfControl.WpfSetSilentMode(true);
                this.elementHost.Child = this._wpfControl.WpfGetCanvas(this.panelForWpfControl);
                //elementHost.Child = _wpfControl;
            }
            return _wpfControl;
            //}
            //set { _wpfControl = value; }
        }

        private GeoObject _GeoObject;
        private List<GeoObject> _GeoObjectsEditor;

        #endregion

        #endregion

        #region Construction

        public UserControlImage()
        {
            InitializeComponent();
            this.numericUpDownMaxSize.Value = (decimal)DiversityWorkbench.Settings.MaximalImageSizeInKb / (decimal)1000;
        }

        #endregion

        #region Buttons

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonZoomSector_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton100_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.ZoomImage100();
            this.ResumeLayout(false);
        }

        private void toolStripButtonZoomAdapt_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.AdaptZoom();
            this.ResumeLayout(false);
        }

        private void toolStripButtonFlipHorizontal_Click(object sender, EventArgs e)
        {
            this.RotateImage(System.Drawing.RotateFlipType.Rotate180FlipY);
        }

        private void toolStripButtonFlipVertical_Click(object sender, EventArgs e)
        {
            this.RotateImage(System.Drawing.RotateFlipType.Rotate180FlipX);
        }

        private void toolStripButtonRotateRight_Click(object sender, EventArgs e)
        {
            this.RotateImage(System.Drawing.RotateFlipType.Rotate90FlipNone);
        }

        private void toolStripButtonRotateLeft_Click(object sender, EventArgs e)
        {
            this.RotateImage(System.Drawing.RotateFlipType.Rotate270FlipNone);
        }

        private void toolStripButtonOpenInNewForm_Click(object sender, EventArgs e)
        {
            if (this.textBoxImagePath.Text.Length == 0) return;
            DiversityWorkbench.Forms.FormImage f = new DiversityWorkbench.Forms.FormImage(this.textBoxImagePath.Text);
            f.Show();
        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            this.toolStripButtonOpenInNewForm_Click(null, null);
        }


        private void buttonMediaNoImageDownload_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;
        }

        private void buttonMediaNoImageOpenInNewWindow_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxMediaNoImageURL.Text);
            f.Show();
        }

        private void buttonExternalBrowser_Click(object sender, EventArgs e)
        {
            if (this.webBrowserNoMedia.Url != null && this._MediumType != DiversityWorkbench.Forms.FormFunctions.Medium.Ignore)
            {
                //System.Diagnostics.Process.Start(this.webBrowserNoMedia.Url.ToString());
                try
                {
                    string url = this.webBrowserNoMedia.Url.ToString();
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while trying to open with an external browser: {ex.Message}");
                }
            }
        }

        private void buttonScript_Click(object sender, EventArgs e)
        {
            if (this.webBrowserNoMedia.AllowScripting)
            {
                this.webBrowserNoMedia.AllowScripting = false;
                this.buttonScript.Image = Properties.Resources.JSno;
            }
            else
            {
                if (MessageBox.Show("Do you really want to activate scripts?", "Activate scripts", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

                this.webBrowserNoMedia.AllowScripting = true;
                this.buttonScript.Image = Properties.Resources.JS;
            }
        }

        #endregion

        #region Rotate
        /// <summary>
        /// Rotating resp. flipping the image
        /// </summary>
        /// <param name="Rotate">the way the image should be rotated or flipped</param>
        public void RotateImage(System.Drawing.RotateFlipType Rotate)
        {
            if (this.pictureBox.Image != null)
            {
                int X = this.pictureBox.Left;
                int Y = this.pictureBox.Top;
                int MirrorX = this.pictureBox.Width + this.pictureBox.Left - this.panelImage.Width;
                int MirrorY = this.pictureBox.Height + this.pictureBox.Top - this.panelImage.Height;
                this.pictureBox.Visible = false;
                try
                {
                    switch (Rotate)
                    {
                        case System.Drawing.RotateFlipType.Rotate180FlipY: // horizontal flip
                            this.pictureBox.Left = -MirrorX;
                            break;
                        case System.Drawing.RotateFlipType.Rotate180FlipX: // vertical flip
                            this.pictureBox.Top = -MirrorY;
                            break;
                        case System.Drawing.RotateFlipType.Rotate90FlipNone: // rotate right
                            this.pictureBox.Left = -MirrorY;
                            this.pictureBox.Top = X;
                            this.RotatePictureSize();
                            break;
                        case System.Drawing.RotateFlipType.Rotate270FlipNone: // rotate left
                            this.pictureBox.Left = Y;
                            this.pictureBox.Top = -MirrorX;
                            this.RotatePictureSize();
                            break;
                        case RotateFlipType.Rotate180FlipXY:
                            this.pictureBox.Left = -MirrorX;
                            this.pictureBox.Top = -MirrorY;
                            break;
                    }

                }
                catch { }
                this.pictureBox.Image.RotateFlip(Rotate);
                this.pictureBox.Refresh();
                this.AlignImage();
                this.pictureBox.Visible = true;
            }
        }

        /// <summary>
        /// Adapting the size of the picture box to the size of the image after rotation to avoid distortion
        /// </summary>
        private void RotatePictureSize()
        {
            int x = this.pictureBox.Height;
            int y = this.pictureBox.Width;
            this.pictureBox.Width = x;
            this.pictureBox.Height = y;
        }

        #endregion

        #region Area Selection

        private void pictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                //int dX = this.StartPoint.X - e.X;
                //int dY = this.StartPoint.Y - e.Y;
                //dX = dX * System.Math.Sign(dX);
                //dY = dY * System.Math.Sign(dY);
                this.StartPoint.X = e.X;
                this.StartPoint.Y = e.Y;
                //if (dY > 5 && dX > 5)
                this.DrawingSquare = true;
            }
            catch (System.Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message, "Error in pictureBox_MouseDown", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private void pictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            float Zoom;
            try
            {
                System.Drawing.Graphics rectArea = this.pictureBox.CreateGraphics();
                rectArea.DrawRectangle(this.SquarePen, this.SelectedArea);
                this.pictureBox.Cursor = System.Windows.Forms.Cursors.Default;
                this.DrawingSquare = false;
                if (!this.MarkingArea() && this.SelectedArea.Width > 5 && this.SelectedArea.Height > 5)
                {
                    float ZoomH = (float)this.panelImage.Width / (float)this.SelectedArea.Width;
                    float ZoomV = (float)this.panelImage.Height / (float)this.SelectedArea.Height;
                    if (ZoomH < ZoomV) Zoom = ZoomH;
                    else Zoom = ZoomV;
                    System.Drawing.Point pTopLeft = new System.Drawing.Point(this.SelectedArea.Left + this.pictureBox.Left, this.SelectedArea.Top + this.pictureBox.Top);
                    this.ZoomAndShiftImage(pTopLeft, Zoom);
                }
                //else
                //{
                //    this.DrawingSquare = true;
                //}
            }
            catch
            { }
        }

        private void pictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (this.DrawingSquare)
                {
                    this.pictureBox.Refresh();
                    System.Drawing.Graphics rectArea = this.pictureBox.CreateGraphics();
                    if (this.StartPoint.X < e.X)
                    {
                        this.SelectedArea.X = this.StartPoint.X;
                    }
                    else
                    {
                        this.SelectedArea.X = e.X;
                    }
                    if (this.StartPoint.Y < e.Y)
                    {
                        this.SelectedArea.Y = this.StartPoint.Y;
                    }
                    else
                    {
                        this.SelectedArea.Y = e.Y;
                    }
                    this.SelectedArea.Height = System.Math.Abs(this.StartPoint.Y - e.Y);
                    this.SelectedArea.Width = System.Math.Abs(this.StartPoint.X - e.X);
                    rectArea.DrawRectangle(this.SquarePen, this.SelectedArea);
                }

            }
            catch { }
        }

        #endregion

        #region Zoom and adapt

        /// <summary>
        /// Zooming the image and shifting it
        /// </summary>
        /// <param name="pTopLeftSelection">the top left corner of the area selected for zooming within the depicted part of the image</param>
        /// <param name="Zoom">the factor for zooming</param>
        private void ZoomAndShiftImage(System.Drawing.Point pTopLeftSelection, float Zoom)
        {
            if ((Zoom > this.ZoomMaxLimit && this.pictureBox.Width < MaxPictureSize)
                || (Zoom > this.ZoomMaxLimit && this.pictureBox.Height < MaxPictureSize))
            {
                float ZoomMax = MaxPictureSize / this.pictureBox.Height;
                float ZoomNeu = ZoomMax;
                if (ZoomMax > this.ZoomMaxLimit && ZoomMax > 1 && this.ZoomMaxLimit > 1) ZoomNeu = this.ZoomMaxLimit;
                Zoom = ZoomNeu;
            }
            try
            {
                if (Zoom > 1.0)
                {
                    int X = (int)((float)(pTopLeftSelection.X - this.pictureBox.Left) * Zoom);
                    int Y = (int)((float)(pTopLeftSelection.Y - this.pictureBox.Top) * Zoom);
                    this.pictureBox.Left = -X;
                    this.pictureBox.Top = -Y;
                    this.pictureBox.Width = (int)((float)this.pictureBox.Width * Zoom);
                    this.pictureBox.Height = (int)((float)this.pictureBox.Height * Zoom);
                    this.AlignImage();
                }
                else
                    this.ZoomImage((float)0.999);
            }
            catch
            { }
        }

        private float ZoomMaxLimit
        {
            get
            {
                float Limit = (float)0.0;
                try
                {
                    if (this.pictureBox.Image != null)
                    {
                        float LimitWidth = this.pictureBox.Image.Width * 8 / this.pictureBox.Width;
                        float LimitHeight = this.pictureBox.Image.Height * 8 / this.pictureBox.Height;
                        Limit = LimitWidth;
                        if (LimitWidth < LimitHeight) Limit = LimitHeight;
                    }
                    if (Limit < 1.0) Limit = (float)1.0;

                }
                catch { }
                return Limit;
            }
        }

        /// <summary>
        /// Zooming the image
        /// </summary>
        /// <param name="Zoom">the factor for zooming</param>
        private void ZoomImage(float Zoom)
        {
            try
            {
                if (this.pictureBox.Image != null)
                {
                    // max zoom reached
                    if (
                        Zoom > 1 &&
                        (
                        this.pictureBox.Width > MaxPictureSize
                        || this.pictureBox.Height > MaxPictureSize
                        || this.pictureBox.Height > 4 * Zoom * this.pictureBox.Image.Height
                        || this.pictureBox.Width > 4 * Zoom * this.pictureBox.Image.Width
                        ))
                        return;
                    // min zoom reached
                    if (
                        Zoom < 1 &&
                        (
                        this.pictureBox.Width < MinPictureSize
                        || this.pictureBox.Height < MinPictureSize
                        || this.pictureBox.Height < 0.25 * Zoom * this.pictureBox.Image.Height
                        || this.pictureBox.Width < 0.25 * Zoom * this.pictureBox.Image.Width
                        ))
                        return;
                    int ZoomShiftX = (int)(((float)this.panelImage.Width / 2F) * (1F - (1F / Zoom)));
                    int X = (int)(((float)(-this.pictureBox.Left + ZoomShiftX) * Zoom));
                    int ZoomShiftY = (int)(((float)this.panelImage.Height / 2F) * (1F - (1F / Zoom)));
                    int Y = (int)(((float)(-this.pictureBox.Top + ZoomShiftY) * Zoom));
                    this.pictureBox.Left = -X;
                    this.pictureBox.Top = -Y;
                    this.pictureBox.Width = (int)((float)this.pictureBox.Width * Zoom);
                    this.pictureBox.Height = (int)((float)this.pictureBox.Height * Zoom);
                    this.AlignImage();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Zooming the image to 100 percent of the original size
        /// </summary>
        private void ZoomImage100()
        {
            try
            {
                if (this.pictureBox.Image != null)
                {
                    //int ZoomShiftX = (int)(((float)this.panelImage.Width / 2F) * (1F - (1F / Zoom)));
                    //int X = (int)(((float)(-this.pictureBox.Left + ZoomShiftX) * Zoom));
                    //int ZoomShiftY = (int)(((float)this.panelImage.Height / 2F) * (1F - (1F / Zoom)));
                    //int Y = (int)(((float)(-this.pictureBox.Top + ZoomShiftY) * Zoom));
                    //this.pictureBox.Left = -X;
                    //this.pictureBox.Top = -Y;
                    this.pictureBox.Width = this.pictureBox.Image.Width;
                    this.pictureBox.Height = this.pictureBox.Image.Height;
                    this.AlignImage();
                }
            }
            catch
            { }
        }

        private void AdaptZoom()
        {
            try
            {
                if (this._MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Image || this.MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Unknown)
                {
                    this.pictureBox.Top = 0;
                    this.pictureBox.Left = 0;
                    float Zoom;
                    if (this.pictureBox.Image != null)
                    {
                        float ZoomVertical = (float)this.panelImage.Height / (float)this.pictureBox.Image.Size.Height;
                        float ZoomHorizontal = (float)this.panelImage.Width / (float)this.pictureBox.Image.Size.Width;
                        if (ZoomVertical < ZoomHorizontal)
                        {
                            Zoom = ZoomVertical;
                        }
                        else
                        {
                            Zoom = ZoomHorizontal;
                        }
                        this.pictureBox.Width = (int)((float)this.pictureBox.Image.Size.Width * Zoom);
                        this.pictureBox.Height = (int)((float)this.pictureBox.Image.Size.Height * Zoom);
                        this.AlignImage();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void AlignImage()
        {
            try
            {
                if (this.pictureBox.Left < 0 && this.pictureBox.Width < this.panelImage.Width)
                    this.pictureBox.Left = 0;
                if (this.pictureBox.Top < 0 && this.pictureBox.Height < this.panelImage.Height)
                    this.pictureBox.Top = 0;
                if (this.pictureBox.Left > 0 && this.pictureBox.Left + this.pictureBox.Width > this.panelImage.Width)
                    this.pictureBox.Left = 0;
                if (this.pictureBox.Top > 0 && this.pictureBox.Top + this.pictureBox.Height > this.panelImage.Height)
                    this.pictureBox.Top = 0;
                if (this.pictureBox.Left + this.pictureBox.Width < this.panelImage.Width && this.pictureBox.Left < 0)
                    this.pictureBox.Left = this.panelImage.Width - this.pictureBox.Width;
                if (this.pictureBox.Top + this.pictureBox.Height < this.panelImage.Height && this.pictureBox.Top < 0)
                    this.pictureBox.Top = this.panelImage.Height - this.pictureBox.Height;

            }
            catch { }
        }

        private void numericUpDownZoom_ValueChanged(object sender, EventArgs e)
        {
            float z = (float)this.numericUpDownZoom.Value / 100;
            this.ZoomImage(z);
        }
        #endregion

        #region Marking an area in the image

        public void setMarkingTable(string MarkingTable) { this._MarkingTable = MarkingTable; }
        public void setMarkingGeometryColumn(string MarkingColumn) { this._MarkingGeometryColumn = MarkingColumn; }
        public void setMarkingDisplayColumn(string MarkingColumn) { this._MarkingDisplayColumn = MarkingColumn; }
        public void setMarkingWhereClause(string MarkingWhereClause) { this._MarkingWhereClause = MarkingWhereClause; }

        private string _MarkingTable;
        private string _MarkingGeometryColumn;
        private string _MarkingDisplayColumn;
        private string _MarkingWhereClause;


        //public bool MarkingArea
        //{
        //    get { return _MarkingArea; }
        //    set 
        //    { 
        //        _MarkingArea = value;
        //        // new version with GIS-Editor
        //        this.splitContainerImage.Panel1Collapsed = value;
        //        this.splitContainerImage.Panel2Collapsed = !value;
        //        if (_MarkingArea)
        //        {
        //            try
        //            {
        //                //this.WpfControl.WpfShowRefMapDelButton(false);
        //                //this.elementHost.Child = _wpfControl.WpfGetCanvas(this.panelForWpfControl);
        //                this.WpfControl().WpfClearAllSamples(true);
        //                if (this._GeoObject.GeometryData != null
        //                && this._GeoObject.GeometryData.Length > 0)
        //                {
        //                    this._GeoObjectsEditor = new List<GeoObject>();
        //                    this._GeoObjectsEditor.Add(this._GeoObject);
        //                    //this.WpfControl.WpfSetMode(2);
        //                    this.WpfControl().WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
        //                    this.WpfControl().WpfAddSample();
        //                }
        //            }
        //            catch (System.Exception ex)
        //            {
        //            }
        //        }

        //        #region Old version

        //        //this.toolStripButtonMarkArea.Visible = value;
        //        //if (value)
        //        //{
        //        //    this.toolStripButton100_Click(null, null);
        //        //    this.toolStripButtonFlipHorizontal.Enabled = false;
        //        //    this.toolStripButtonFlipVertical.Enabled = false;
        //        //    this.toolStripButtonRotateLeft.Enabled = false;
        //        //    this.toolStripButtonRotateRight.Enabled = false;
        //        //    this.toolStripButtonZoomAdapt.Enabled = false;
        //        //}

        //        #endregion
        //    }
        //}

        public bool MarkingArea()
        {
            return _MarkingArea;
        }

        public void setMarkingArea(bool IsActive)
        {
            _MarkingArea = IsActive;
            // new version with GIS-Editor
            this.splitContainerImage.Panel1Collapsed = IsActive;
            this.splitContainerImage.Panel2Collapsed = !IsActive;
            if (_MarkingArea)
            {
                try
                {
                    //this.WpfControl.WpfShowRefMapDelButton(false);
                    //this.elementHost.Child = _wpfControl.WpfGetCanvas(this.panelForWpfControl);
                    this.WpfControl().WpfClearAllSamples(true);
                    if (this._GeoObject.GeometryData != null
                    && this._GeoObject.GeometryData.Length > 0)
                    {
                        this._GeoObjectsEditor = new List<GeoObject>();
                        this._GeoObjectsEditor.Add(this._GeoObject);
                        //this.WpfControl.WpfSetMode(2);
                        this.WpfControl().WpfSetMapAndGeoObjects(this._GeoObjectsEditor);
                        this.WpfControl().WpfAddSample();
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        public bool LoadImageForMarking()
        {
            bool OK = true;
            this.toolStripTextBoxImageEditorFilename.Text = this.ImagePath;
            this.WpfControl().WpfClearAllSamples(true);
            OK = this.WpfControl().WpfLoadWebImage(this.ImagePath);
            return OK;
        }

        private void toolStripButtonImageEditorAdd_Click(object sender, EventArgs e)
        {
            this.WpfControl().WpfNextShape();
        }

        public bool LoadGeometry()
        {
            bool OK = true;
            try
            {
                string SQL = "SELECT " + this._MarkingDisplayColumn + ", " + this._MarkingGeometryColumn + ".ToString() AS " + this._MarkingGeometryColumn +
                    " FROM " + this._MarkingTable + " " + this._MarkingWhereClause;
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                bool HasGeometry = false;
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                if (dt.Rows.Count > 0)
                {
                    System.Data.DataRow R = dt.Rows[0];
                    if (!R[this._MarkingGeometryColumn].Equals(System.DBNull.Value))
                    {
                        HasGeometry = true;
                        System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Geoobjects = new List<GeoObject>();
                        WpfSamplingPlotPage.GeoObject GO = new GeoObject();
                        GO.Identifier = R[this._MarkingDisplayColumn].ToString();
                        GO.GeometryData = R[this._MarkingGeometryColumn].ToString();
                        GO.DisplayText = R[this._MarkingDisplayColumn].ToString();
                        GO.FillBrush = System.Windows.Media.Brushes.Red;
                        GO.StrokeBrush = System.Windows.Media.Brushes.Red;
                        GO.StrokeThickness = 4;
                        GO.StrokeTransparency = 255;
                        Geoobjects.Add(GO);
                        try
                        {
                            this.WpfControl().WpfClearAllSamples(true);
                            this.WpfControl().WpfSetWebImageAndGeoObjects(this.ImagePath, Geoobjects, false);
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                    else
                    {
                        this.WpfControl().WpfClearAllSamples(true);
                        this.WpfControl().WpfLoadWebImage(this.ImagePath);
                    }
                }
                if (!HasGeometry)
                {
                    this.WpfControl().WpfClearAllSamples(true);
                    this.WpfControl().WpfLoadWebImage(this.ImagePath);
                }
            }
            catch (System.Exception ex)
            {
            }
            return OK;
        }

        private void toolStripButtonImageEditorSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.WpfControl().WpfAddShape();
                System.Collections.Generic.List<WpfSamplingPlotPage.GeoObject> Geoobjects = new List<GeoObject>();
                if (WpfControl().WpfGetGeoObjects(out Geoobjects))
                {
                    string Geometry = Geoobjects[0].GeometryData;
                    string SQL = "Update " + this._MarkingTable + " SET " + this._MarkingTable + "." + this._MarkingGeometryColumn + " = " +
                        "geometry::STGeomFromText('" +
                        Geometry +
                        "', 0) " + this._MarkingWhereClause;
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
            }
            catch (System.Exception ex) { }
        }

        private string GeometyFromWpfControl()
        {
            string Geometry = "";
            return Geometry;
        }

        private void toolStripImageEditor_SizeChanged(object sender, EventArgs e)
        {
            ///TODO: funktioniert nicht, Wert bleibt
            this.toolStripTextBoxImageEditorFilename.Width = this.toolStripImageEditor.Width - 100;
        }


        private void toolStripButtonImageEditorDelete_Click(object sender, EventArgs e)
        {
            string SQL = "UPDATE T SET " + this._MarkingGeometryColumn + " = NULL " +
                " FROM " + this._MarkingTable + " AS T " + this._MarkingWhereClause;
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            this.LoadGeometry();
        }

        #region Old version using DrawRectangle

        /// <summary>
        /// Marking an area in an image
        /// </summary>
        /// <param name="Table">The name of the table in the database</param>
        /// <param name="Column">The column in the table</param>
        /// <param name="WhereClause">The where clause to identify the correct dataset</param>
        /// <param name="Area">The area that should be displayed</param>
        /// <param name="DisplayText">An optional text shown in the area</param>
        public void MarkArea(string Table, string Column, string WhereClause, System.Drawing.Rectangle Area, string DisplayText)
        {
            //this._MarkingArea = false;
            //this.pictureBox_Paint(null, null);
            //this.toolStripButtonZoomAdapt_Click(null, null);
            //this.toolStripButton100_Click(null, null);
            this._MarkingTable = Table;
            this._MarkingGeometryColumn = Column;
            this._MarkingWhereClause = WhereClause;
            this._MarkingAreaDisplayText = DisplayText;
            if (this._MarkingGeometryColumn.Length > 0 &&
                this._MarkingTable.Length > 0 &&
                this._MarkingWhereClause.Length > 0)
                this.setMarkingArea(true);
            else
                this.setMarkingArea(false);
            if (Area.Width > 0 && Area.Height > 0)
            {
                this.SelectedArea.X = Area.X;
                this.SelectedArea.Y = Area.Y;
                this.SelectedArea.Width = Area.Width;
                this.SelectedArea.Height = Area.Height;

                //this.pictureBox.Refresh();
                //System.Drawing.Graphics rectArea = this.pictureBox.CreateGraphics();
                //rectArea.DrawRectangle(this.SquarePen, this.SelectedArea);
                this.DrawMarkingArea();
                //if (this.panelImage.VerticalScroll.Visible)
                //    this.panelImage.VerticalScroll.Value = this.SelectedArea.X;
                //if (this.panelImage.HorizontalScroll.Visible)
                //    this.panelImage.HorizontalScroll.Value = this.SelectedArea.Y;

                if (this.panelImage.VerticalScroll.Visible)
                    this.panelImage.VerticalScroll.Value++;
                if (this.panelImage.HorizontalScroll.Visible)
                    this.panelImage.HorizontalScroll.Value++;

            }
        }


        public void MarkAreas(System.Collections.Generic.Dictionary<string, System.Drawing.Rectangle> Areas)
        {
            this.toolStripButton100_Click(null, null);
            System.Drawing.Graphics rectArea = this.pictureBox.CreateGraphics();
            foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Rectangle> KV in Areas)
            {
                if (KV.Value.Width > 0 && KV.Value.Height > 0)
                {
                    rectArea.DrawRectangle(this.MarkingPen, KV.Value);
                    if (KV.Key.Length > 0)
                    {
                        System.Drawing.Pen TextPen = new System.Drawing.Pen(System.Drawing.Color.Red, 0.1F);
                        System.Drawing.Font F = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular);
                        System.Drawing.Point P = new Point(KV.Value.X + 1, KV.Value.Y + 1);
                        rectArea.DrawString(KV.Key, F, TextPen.Brush, P);
                    }
                }
            }
            //Application.DoEvents();
        }

        //private void DrawMarkingAreaDisplayText(string DisplayText)
        //{
        //    System.Drawing.Graphics Text = this.pictureBox.CreateGraphics();
        //    Text.DrawString(
        //}

        private void DrawMarkingArea()
        {
            System.Drawing.Graphics rectArea = this.pictureBox.CreateGraphics();
            rectArea.DrawRectangle(this.MarkingPen, this.SelectedArea);
            if (this._MarkingAreaDisplayText.Length > 0)
            {
                System.Drawing.Pen TextPen = new System.Drawing.Pen(System.Drawing.Color.Red, 0.1F);
                System.Drawing.Font F = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular);
                System.Drawing.Point P = new Point(this.SelectedArea.X + 1, this.SelectedArea.Y + 1);
                rectArea.DrawString(this._MarkingAreaDisplayText, F, TextPen.Brush, P);
            }
            //this.panelImage.VerticalScroll.Value = this.SelectedArea.X;
            //this.panelImage.HorizontalScroll.Value = this.SelectedArea.Y;
            //Application.DoEvents();
        }

        private void MarkArea()
        {
            try
            {
                string SQL = "Update " + this._MarkingTable + " SET " + this._MarkingTable + "." + this._MarkingGeometryColumn + " = " +
                    "geometry::STGeomFromText('POLYGON ((" +
                    this.SelectedArea.X.ToString() + " " + this.SelectedArea.Y.ToString() + ", " +
                    (this.SelectedArea.X + this.SelectedArea.Width).ToString() + " " + this.SelectedArea.Y.ToString() + ", " +
                    (this.SelectedArea.X + this.SelectedArea.Width).ToString() + " " + (this.SelectedArea.Y + this.SelectedArea.Height).ToString() + ", " +
                    this.SelectedArea.X.ToString() + " " + (this.SelectedArea.Y + this.SelectedArea.Height).ToString() + ", " +
                    this.SelectedArea.X.ToString() + " " + this.SelectedArea.Y.ToString() +
                    "))', 0) " + this._MarkingWhereClause;
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (System.Exception ex) { }
        }

        private void toolStripButtonMarkArea_Click(object sender, EventArgs e)
        {
            this._MarkingArea = false;
            this.MarkArea();
            this.toolStripButtonMarkArea.Visible = false;
            this.toolStripButtonFlipHorizontal.Enabled = true;
            this.toolStripButtonFlipVertical.Enabled = true;
            this.toolStripButtonRotateLeft.Enabled = true;
            this.toolStripButtonRotateRight.Enabled = true;
            this.toolStripButtonZoomAdapt.Enabled = true;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            return;

            if (this.MarkingArea())
            { this.DrawMarkingArea(); }
        }

        #endregion

        #endregion

        #region Properties

        public void AdaptImage() { this.AdaptZoom(); }

        public string HelpNameSpace
        {
            set
            {
                this.helpProvider.HelpNamespace = value;
                this.userControlMediaPlayer.HelpNameSpace = value;
            }
        }

        public string ImagePath
        {
            get
            {
                return this.textBoxImagePath.Text;
            }
            set
            {
                string Path = "";
                this.SuspendLayout();
                try
                {
                    this.labelErrorMessage.Visible = false;
                    int MaxSizeOfImage = (int)this.numericUpDownMaxSize.Value;
                    bool OK = true;
                    this.SuspendLayout();
                    Path = value;
                    string Extension = "";
                    this.textBoxImagePath.Text = value;
                    this.MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(value);
                    if (DiversityWorkbench.Settings.UseWebView) this.MediumType = DiversityWorkbench.Forms.FormFunctions.Medium.Unknown;
                    this.userControlMediaPlayer.File = value;
                    this.initZoomView();

                    // Markus 20.1.2020 wurde vermutlich in Rahmen der Auswahl von Property Areas abgeschaltet - noch nicht gefunden wann und wo
                    if (this.MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Image && !this.MarkingArea() && value.Length > 0)
                    {
                        this.toolStripImage.Enabled = true;
                    }

                    if (Path == "")// && this.pictureBox.Image != null)
                    {
                        try
                        {
                            this.pictureBox.Image = null;

                            this.WpfControl().WpfClearAllSamples(true);
                            this.WpfControl().WpfLoadWebImage("");
                            //this.pictureBox.Image = null;
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            Path = Path.Replace("/", "\\");
                            if (Path.IndexOf("http:") == 0 || Path.IndexOf("https:") == 0)
                            {
                                System.Drawing.Bitmap bmpToBig = (System.Drawing.Bitmap)this.DefaultIconTooBig;
                                System.Drawing.Bitmap bmpNotFound = (System.Drawing.Bitmap)this.DefaultIconWrongPath;
                                string Message = "";
                                switch (this._MediumType)
                                {
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.Image:
                                        this.splitContainerMedia.Panel1Collapsed = false;
                                        this.splitContainerMedia.Panel2Collapsed = true;
                                        this.pictureBox.Image = DiversityWorkbench.Forms.FormFunctions.BitmapFromWeb(Path, ref Message);
                                        if (Message.Length > 0)
                                        {
                                            if (Message == System.Net.WebExceptionStatus.SecureChannelFailure.ToString())
                                            {
                                                // SSL Error: Try to load link in browser
                                                // Toni 20200715: Since some time Wiki links like
                                                // https://upload.wikimedia.org/wikipedia/commons/thumb/d/d7/Eisenhut_blau.JPG/500px-Eisenhut_blau.JPG
                                                // return an error concerning the ssl connection. Anyway, the browser can show the picture..
                                                this.splitContainerMedia.Panel1Collapsed = true;
                                                this.splitContainerMedia.Panel2Collapsed = false;
                                                this.splitContainerNoImage.Panel1Collapsed = true;
                                                this.splitContainerNoImage.Panel2Collapsed = false;
                                                this.textBoxMediaNoImageURL.Text = value;
                                                try
                                                {
                                                    this.textBoxMediaNoImageURL.Text = value;
                                                    System.Uri U = new Uri(value);
                                                    this.webBrowserNoMedia.Url = U;
                                                    if (this.webBrowserNoMedia.Url != null && this.webBrowserNoMedia.Url.AbsoluteUri != value)
                                                    {
                                                        this.webBrowserNoMedia.BeginInvoke(null);
                                                        this.webBrowserNoMedia.Focus();
                                                        this.webBrowserNoMedia.EndInvoke(null);
                                                    }
                                                }
                                                catch (System.Exception ex) { }
                                            }
                                            else
                                            {
                                                this.pictureBox.Visible = false;
                                                this.labelErrorMessage.Visible = true;
                                                this.labelErrorMessage.Text = Message;
                                                this.labelErrorMessage.Dock = DockStyle.Fill;
                                            }
                                        }
                                        else
                                        {
                                            this.pictureBox.Visible = true;
                                            this.labelErrorMessage.Visible = false;
                                            if (this.textBoxImagePath.Text.IndexOf("@") > -1)
                                            {
                                                string PathWithoutCredetials = "http://" + textBoxImagePath.Text.Substring(textBoxImagePath.Text.IndexOf("@") + 1);
                                                textBoxImagePath.Text = PathWithoutCredetials;
                                            }
                                        }
                                        break;
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.Audio:
                                        this.splitContainerMedia.Panel1Collapsed = true;
                                        this.splitContainerMedia.Panel2Collapsed = false;
                                        this.splitContainerNoImage.Panel1Collapsed = false;
                                        this.splitContainerNoImage.Panel2Collapsed = true;
                                        this.userControlMediaPlayer.MediaImage = (System.Drawing.Bitmap)this.DefaultIconAudio;
                                        break;
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.Video:
                                        this.splitContainerMedia.Panel1Collapsed = true;
                                        this.splitContainerMedia.Panel2Collapsed = false;
                                        this.userControlMediaPlayer.MediaImage = (System.Drawing.Bitmap)this.DefaultIconVideo;
                                        this.splitContainerNoImage.Panel1Collapsed = false;
                                        this.splitContainerNoImage.Panel2Collapsed = true;
                                        break;
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.Unknown:
                                        this.splitContainerMedia.Panel1Collapsed = true;
                                        this.splitContainerMedia.Panel2Collapsed = false;
                                        this.splitContainerNoImage.Panel1Collapsed = true;
                                        this.splitContainerNoImage.Panel2Collapsed = false;
                                        this.textBoxMediaNoImageURL.Text = value;
                                        try
                                        {
                                            this.textBoxMediaNoImageURL.Text = value;
                                            System.Uri U = new Uri(value);
                                            this.webBrowserNoMedia.Url = U;
                                            if (this.webBrowserNoMedia.Url != null && this.webBrowserNoMedia.Url.AbsoluteUri != value)
                                            {
                                                this.webBrowserNoMedia.BeginInvoke(null);
                                                this.webBrowserNoMedia.Focus();
                                                this.webBrowserNoMedia.EndInvoke(null);
                                            }
                                        }
                                        catch (System.Exception ex) { }
                                        break;
                                    default:
                                        this.splitContainerMedia.Panel1Collapsed = true;
                                        this.splitContainerMedia.Panel2Collapsed = false;
                                        this.splitContainerNoImage.Panel1Collapsed = true;
                                        this.splitContainerNoImage.Panel2Collapsed = false;
                                        this.webBrowserNoMedia.Url = null;
                                        break;
                                }
                            }
                            else
                            {
                                System.IO.FileInfo File = new System.IO.FileInfo(Path.StartsWith("file:\\\\\\") ? Path.Substring("file:\\\\\\".Length) : Path);
                                //System.IO.FileInfo File = new System.IO.FileInfo(Path);
                                if (File.Exists)
                                {
                                    switch (this.MediumType)
                                    {
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.Image:
                                            this.splitContainerMedia.Panel1Collapsed = false;
                                            this.splitContainerMedia.Panel2Collapsed = true;
                                            try
                                            {
                                                long LengthOfUri = File.Length;
                                                int SizeOfImage = (int)LengthOfUri / 1000;
                                                if (SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                                                {
                                                    System.Drawing.Bitmap bmpToBig = (System.Drawing.Bitmap)this.DefaultIconTooBig;
                                                    this.pictureBox.Image = bmpToBig;
                                                    this.labelErrorMessage.Visible = true;
                                                    string Message = DiversityWorkbench.Forms.FormFunctions.ImageTooBigMessage(SizeOfImage); // resources.GetString("ImageSize") + ":\r\n" + SizeOfImage.ToString() + " KB";
                                                    this.labelErrorMessage.Text = Message;
                                                }
                                                else
                                                {
                                                    System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(value);
                                                    this.pictureBox.Image = bmp;
                                                    if (!this.pictureBox.Visible)
                                                        this.pictureBox.Visible = true;
                                                }
                                            }
                                            catch (System.OutOfMemoryException oomex)
                                            {
                                                DiversityWorkbench.UserControls.UserControlImage uci = new UserControlImage();
                                                this.pictureBox.Image = (Bitmap)uci.DefaultIconTooBig;
                                            }
                                            catch (System.Exception ex)
                                            {
                                                OK = false;
                                            }

                                            break;
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.Audio:
                                            this.splitContainerMedia.Panel1Collapsed = true;
                                            this.splitContainerMedia.Panel2Collapsed = false;
                                            this.splitContainerNoImage.Panel1Collapsed = false;
                                            this.splitContainerNoImage.Panel2Collapsed = true;
                                            this.userControlMediaPlayer.MediaImage = (System.Drawing.Bitmap)this.DefaultIconAudio;
                                            break;
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.Video:
                                            this.splitContainerMedia.Panel1Collapsed = true;
                                            this.splitContainerMedia.Panel2Collapsed = false;
                                            this.splitContainerNoImage.Panel1Collapsed = false;
                                            this.splitContainerNoImage.Panel2Collapsed = true;
                                            this.userControlMediaPlayer.MediaImage = (System.Drawing.Bitmap)this.DefaultIconVideo;
                                            break;
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.Unknown:
                                            this.splitContainerMedia.Panel1Collapsed = true;
                                            this.splitContainerMedia.Panel2Collapsed = false;
                                            this.splitContainerNoImage.Panel1Collapsed = true;
                                            this.splitContainerNoImage.Panel2Collapsed = false;
                                            System.Uri U = new Uri(Path);
                                            this.webBrowserNoMedia.Url = U;
                                            this.textBoxMediaNoImageURL.Text = Path;
                                            break;
                                        default:
                                            this.splitContainerMedia.Panel1Collapsed = true;
                                            this.splitContainerMedia.Panel2Collapsed = false;
                                            this.splitContainerNoImage.Panel1Collapsed = true;
                                            this.splitContainerNoImage.Panel2Collapsed = false;
                                            this.webBrowserNoMedia.Url = null;
                                            this.textBoxMediaNoImageURL.Text = Path;
                                            break;
                                    }
                                }
                                else
                                {
                                    OK = false;
                                    this.pictureBox.Image = null;
                                }

                            }
                        }
                        catch (System.ArgumentException ex)
                        {
                            try
                            {
                                System.Net.WebRequest webrq = System.Net.WebRequest.Create(value);
                                System.Net.WebResponse webResponse = webrq.GetResponse();
                                long LengthOfUri = webResponse.ContentLength;
                                int SizeOfImage = (int)LengthOfUri / 1000;
                                if (SizeOfImage > DiversityWorkbench.Settings.MaximalImageSizeInKb)
                                {
                                    System.Drawing.Bitmap bmpToBig = (System.Drawing.Bitmap)this.DefaultIconTooBig;
                                    this.pictureBox.Image = bmpToBig;
                                    this.labelErrorMessage.Visible = true;
                                    //this.labelErrorMessage.Text = "Image size:\r\n" + SizeOfImage.ToString() + " KB";
                                    string Message = DiversityWorkbench.Forms.FormFunctions.ImageTooBigMessage(SizeOfImage); // resources.GetString("ImageSize") + ":\r\n" + SizeOfImage.ToString() + " KB";
                                    this.labelErrorMessage.Text = Message;
                                }
                                else
                                {
                                    System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webResponse.GetResponseStream());
                                    this.pictureBox.Image = bmp;
                                }
                                webResponse.Close();
                                webrq.Abort();
                            }
                            catch (System.OutOfMemoryException oomex)
                            {
                                DiversityWorkbench.UserControls.UserControlImage uci = new UserControlImage();
                                this.pictureBox.Image = (Bitmap)uci.DefaultIconTooBig;
                            }
                            catch (System.Exception x)
                            {
                                System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)DefaultIconWrongPath; // (System.Drawing.Bitmap)this.imageListDefaults.Images[0];
                                this.pictureBox.Image = bmp;
                                //if (MaxSizeOfImage > 0 && (bmp.PhysicalDimension.Height > MaxSizeOfImage
                                //  || bmp.PhysicalDimension.Width > MaxSizeOfImage))
                                //{
                                //    System.Drawing.Bitmap bmpToBig = (System.Drawing.Bitmap)this.DefaultIconTooBig;
                                //    this.pictureBox.Image = bmpToBig;
                                //}
                                //else
                                //    this.pictureBox.Image = bmp;
                                OK = false;
                                //System.Windows.Forms.MessageBox.Show(x.Message);
                            }
                        }
                        finally
                        {
                            this.AdaptZoom();
                        }
                    }
                    this.ResumeLayout();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                this.ResumeLayout();
            }
        }

        public int MaxKbOfImage
        {
            get
            {
                return (int)this.numericUpDownMaxSize.Value * 1000;
            }
        }

        public bool MaxSizeOfImageVisible
        {
            set
            {
                this.numericUpDownMaxSize.Visible = value;
                this.labelMaxSize.Visible = value;
            }
        }

        public DiversityWorkbench.Forms.FormFunctions.Medium MediumType
        {
            get { return _MediumType; }
            set
            {
                _MediumType = value;
                try
                {
                    if (this._MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Audio ||
                        this._MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Video)
                    {
                        this.splitContainerMedia.Panel1Collapsed = true;
                        this.splitContainerMedia.Panel2Collapsed = false;
                    }
                    else
                    {
                        this.splitContainerMedia.Panel1Collapsed = false;
                        this.splitContainerMedia.Panel2Collapsed = true;
                    }
                }
                catch { }
            }
        }

        public string ImagePathLabel
        {
            set
            {
                this.labelImagePath.Text = value;
            }
        }

        #region Default Images

        public System.Drawing.Image DefaultIconWrongPath
        {
            get { return this.imageListErrors.Images[0]; }
        }

        public System.Drawing.Image DefaultIconTooBig
        {
            get { return this.imageListErrors.Images[1]; }
        }

        public System.Drawing.Image DefaultIconAudio
        {
            get { return this.imageListMediaTypes.Images[0]; }
        }

        public System.Drawing.Image DefaultIconVideo
        {
            get { return this.imageListMediaTypes.Images[1]; }
        }

        public System.Drawing.Image DefaultIconImage
        {
            get { return this.imageListMediaTypes.Images[2]; }
        }

        public System.Drawing.Image DefaultIconUnknown
        {
            get { return this.imageListMediaTypes.Images[3]; }
        }

        #endregion

        #endregion

        #region MaximalSize

        private void numericUpDownMaxSize_ValueChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.MaximalImageSizeInKb = (int)(this.numericUpDownMaxSize.Value * 1000);
        }

        #endregion

        #region ZoomView by krpano

        private void initZoomView()
        {
            try
            {
                this._ZoomViewPath = null;
                if (this.ImagePath.Length > 0 && this.ZoomViewPath.Length > 0)
                {
                    this.buttonZoomView.Visible = true;
                }
                else
                    this.buttonZoomView.Visible = false;
            }
            catch (System.Exception ex) { }
        }

        private void buttonZoomView_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.ZoomViewPath);
            f.Show();
        }

        private string _ZoomViewPath;
        private string ZoomViewPath
        {
            get
            {
                if (this.ImagePath.Length > 0 && this._ZoomViewPath == null)
                {
                    //System.IO.FileInfo f = new System.IO.FileInfo(this.ImagePath);
                    if (this.ImagePath.ToLower().EndsWith(".jpg"))
                    {
                        _ZoomViewPath = this.ImagePath.Replace("http://pictures.", "http://zoomview.").Replace("/web/", "/");
                        _ZoomViewPath = _ZoomViewPath.Remove(_ZoomViewPath.LastIndexOf('.')) + ".html";
                        try
                        {
                            System.Net.WebRequest req = System.Net.WebRequest.Create(_ZoomViewPath);
                            req.Timeout = DiversityWorkbench.Settings.TimeoutWeb;
                            if (DiversityWorkbench.Settings.TimeoutWeb == 0)
                                throw new System.Net.WebException();
                            System.Net.WebResponse res = req.GetResponse();
                        }
                        catch (System.Net.WebException ex)
                        {
                            _ZoomViewPath = "";
                        }
                    }
                    else
                        _ZoomViewPath = "";
                }
                return _ZoomViewPath;
            }
        }

        #endregion

        #region ForceCanonicalPathAndQuery

        private void toolStripButtonForceCanonicalPathAndQuery_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.WorkbenchSettings.Default.ForceCanonicalPathAndQuery)
            {
                DiversityWorkbench.WorkbenchSettings.Default.ForceCanonicalPathAndQuery = false;
            }
            else
            {
                DiversityWorkbench.WorkbenchSettings.Default.ForceCanonicalPathAndQuery = true;
            }
            DiversityWorkbench.WorkbenchSettings.Default.Save();
            this.setForceCanonicalPathAndQuery();
        }

        private void setForceCanonicalPathAndQuery()
        {
            if (DiversityWorkbench.WorkbenchSettings.Default.ForceCanonicalPathAndQuery)
            {
                this.toolStripButtonForceCanonicalPathAndQuery.Text = "%2F";
                this.toolStripButtonForceCanonicalPathAndQuery.ToolTipText = "Force canonical path and query, do not translate escape signs e.g. %2F into /";
            }
            else
            {
                this.toolStripButtonForceCanonicalPathAndQuery.Text = "/";
                this.toolStripButtonForceCanonicalPathAndQuery.ToolTipText = "Translate escape signs, e.g. %2F into /";
            }
        }

        #endregion

        #region Autorotation

        private bool _AutorotationEnabled = false;
        private bool _Autorotate = false;

        /// <summary>
        /// If the autoration according to EXIF should be used
        /// called e.g.:
        /// private void listBoxEventImages_SelectedIndexChanged(object sender, EventArgs e)
        /// {
        ///  ...
        ///  if (this.userControlImageEventImage.AutorotationEnabled && this.userControlImageEventImage.Autorotate)
        ///   {
        ///    System.Drawing.RotateFlipType Rotate = DiversityWorkbench.FormFunctions.ExifRotationInfo(XML);
        ///    if (Rotate != RotateFlipType.RotateNoneFlipNone)
        ///     this.userControlImageEventImage.RotateImage(Rotate);
        ///   }
        /// ...
        /// </summary>
        public bool Autorotate { get { return _Autorotate; } }

        /// <summary>
        /// If the auto rotation according to EXIF info should be enabled. 
        /// Called e.g.: 
        /// private void initControl()
        /// { ... this.userControlImage... .AutorotationEnabled = true; ...
        /// </summary>
        public bool AutorotationEnabled
        {
            get { return _AutorotationEnabled; }
            set
            {
                _AutorotationEnabled = value;
                this.toolStripButtonAutorotate.Visible = value;
            }
        }

        /// <summary>
        /// switching autorotation option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonAutorotate_Click(object sender, EventArgs e)
        {
            this._Autorotate = !this._Autorotate;
            if (_Autorotate) this.toolStripButtonAutorotate.BackColor = System.Drawing.Color.Yellow;
            else this.toolStripButtonAutorotate.BackColor = System.Drawing.SystemColors.Control;
        }

        #endregion
    }
}
