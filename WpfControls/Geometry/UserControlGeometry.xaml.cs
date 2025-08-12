//#define NoPolygon

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControls.Geometry
{
    /// <summary>
    /// Interaktionslogik für UserControlGeometry.xaml
    /// </summary>
    public partial class UserControlGeometry : UserControl
    {
        #region Parameter

        private double _ZoomFactor = 1;
        private ScaleTransform _Scale = new ScaleTransform(1, 1);
        private TransformGroup _Transform = new TransformGroup();
        private Geometry _Geometry = null;
        private bool _MouseButtonPressed = false;
        private Point _LastPoint = new Point(-1, -1);

        // Rectangle
        internal Rectangle _Rectangle = new Rectangle();
        internal double _RectangleWidth = 99;
        internal double _RectangleHeight = 77;
        private bool _RectangleMouseButtonPressed = false;
        private Point _RectangleLastPoint = new Point(-1, -1);
        internal Point _RectangleLastPosition = new Point(0, 0);
        internal double _RectangleLastWidth = 99;
        internal double _RectangleLastHeight = 99;
        private bool _RectangleMove = false;
        private RectangleMode _CaptureFrameMode = RectangleMode.ResizeBottomRight;

        // Edit points
        private bool _EditPoints = false;
        private bool _EditAdd = false;
        private bool _EditDelete = false;
        private bool _EditPointsMouseButtonPressed = false;
        private Point _PolygonLastPoint = new Point(-1, -1);
        private PolygonMode _polygonMode = PolygonMode.Move;

        // ScaleLine
        internal Line _ScaleLine = new Line();
        internal double _ScaleLineWidth = 88;
        internal double _ScaleLineHeight = 8;
        private bool _ScaleLineMouseButtonPressed = false;
        private bool _ScaleLineMove = false;
        private Point _ScaleLineLastPoint = new Point(-1, -1);
        internal Point _ScaleLineLastPosition = new Point(2, 6);
        internal double _ScaleLineLastWidth = 88;
        private ScaleLineMode _ScaleLineMode = ScaleLineMode.ResizeRight;

        // Canvas thickness property
        private Thickness _CanvasThickness = new Thickness(0, 0, 0, 0);

        // parameter to change orientation of image - kept for future functionality
        private int _InverseX = 1;
        private int _InverseY = 1;

        private enum ZoomType { Adapt, Detail, Ori, None }
        private ZoomType _ZoomType = ZoomType.None;

        private Point? lastMousePositionOnTarget;
        private Point? lastCenterPositionOnTarget;

        #endregion

        #region Construction

        public UserControlGeometry()
        {
            InitializeComponent();

            this._Geometry = new Geometry(this.canvas);

            _Transform.Children.Add(_Scale);
            canvas.RenderTransform = _Transform;

            // Zoom
            slider.ValueChanged += Slider_ValueChanged;
            button100.Click += Button100_Click;
            //buttonZoomAdapt.Click += ButtonZoomAdapt_Click;
            //buttonZoomDetail.Click += ButtonZoomDetail_Click;

            // Scale
            buttonScale.Click += ButtonScale_Click;

            // Rectangle
            buttonAddRectangle.Click += ButtonAddRectangle_Click;

            // Clear
            buttonClear.Click += ButtonClear_Click;

            // Polygon
#if NoPolygon
            buttonAddPolygone.Visibility = Visibility.Hidden;
            buttonAddPolygone.Height = 0;
#endif

            // Next
            buttonAddNext.Click += ButtonAddNext_Click;

            // Point
            //buttonAddPoint.Click += ButtonAddPoint_Click;
            //buttonAddPoint.Visibility = Visibility.Hidden;
            //buttonAddPoint.Height = 0;

            // Edit points
            buttonEditPoints.Click += ButtonEditPoints_Click;
            buttonAdd.Click += ButtonEditAdd_Click;
            buttonDelete.Click += ButtonEditDelete_Click;

            // image
            image.MouseMove += Image_MouseMove;
            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
            image.MouseLeave += Image_MouseLeave;
            image.PreviewMouseWheel += Image_PreviewMouseWheel;

            // canvas
            canvas.MouseLeave += Canvas_MouseLeave;
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.PreviewMouseWheel += Image_PreviewMouseWheel;
            //buttonScale.IsEnabled = false;
            this.ReorientCanvas();
#if !DEBUG
            //buttonEditPoints.IsEnabled = false;
#endif
        }

        #endregion

        #region Interface

        public enum State { Edit, Positions, ReadOnly }
        private State _State;
        public void setState(State state)
        {
            this._State = state; 
            switch (_State)
            {
                case State.ReadOnly:
                    //this.buttonAddPolygone.Visibility= Visibility.Hidden;
                    this.buttonAddRectangle.Visibility= Visibility.Hidden;
                    this.buttonEditPoints.Visibility= Visibility.Hidden;
                    this.buttonAdd.Visibility = Visibility.Hidden;
                    this.buttonDelete.Visibility= Visibility.Hidden;
                    this.buttonClear.Visibility = Visibility.Hidden;

                    this.slider.Visibility= Visibility.Visible;
                    this.slider.IsEnabled = true;
                    this.button100.Visibility= Visibility.Visible;
                    this.button100.IsEnabled= true;

                    this.buttonScale.Visibility= Visibility.Hidden;

                    break;
                case State.Positions:
                    this.buttonAddRectangle.Visibility = Visibility.Visible;
                    this.buttonAddRectangle.Foreground = Brushes.Red;
                    this.buttonAddRectangle.IsEnabled = true;

                    this.slider.Visibility = Visibility.Visible;
                    this.slider.IsEnabled = true;

                    this.button100.Visibility = Visibility.Visible;
                    this.button100.IsEnabled = true;

                    //this.buttonAddPoint.Visibility = Visibility.Hidden;
                    //this.buttonAddPolygone.Visibility = Visibility.Hidden;
                    this.buttonEditPoints.Visibility = Visibility.Hidden;
                    this.buttonAdd.Visibility = Visibility.Hidden;
                    this.buttonDelete.Visibility = Visibility.Hidden;

                    this.buttonScale.Visibility = Visibility.Hidden;
                    break;
                case State.Edit:
                    //this.buttonAddPoint.Visibility = Visibility.Hidden;
                    // #221
                    this.buttonAddRectangle.Visibility = Visibility.Visible;
                    this.buttonEditPoints.Visibility = Visibility.Visible;
                    this.buttonAdd.Visibility = Visibility.Visible;
                    this.buttonDelete.Visibility = Visibility.Visible;
                    this.buttonClear.Visibility = Visibility.Visible;

                    this.slider.Visibility = Visibility.Visible;
                    this.slider.IsEnabled = true;
                    this.button100.Visibility = Visibility.Visible;
                    this.button100.IsEnabled = true;

                    this.buttonScale.Visibility = Visibility.Visible;
                    break;
            }
        }

        #endregion

        #region Canvas

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point newPoint = e.GetPosition(canvas);
            if (!_RectangleMouseButtonPressed)
            {
                // Select Cursor according to position
                if (_Geometry != null)
                {
                    if (_Geometry._Rectangle.IsMouseOver)
                    {
                        if (this.EditPoints)
                            this.SetPolygonCursor(_Geometry.ChangePolygonMode(newPoint, _Geometry._Polygon, slider.Value, !EditAdd));
                        else
                            SetRectangleCursor(_Geometry.ChangeRectangleMode(newPoint, slider.Value));
                    }
                    else if (_Geometry._Polygon.IsMouseOver)
                    {
                        if (this.EditPoints)
                            this.SetPolygonCursor(_Geometry.ChangePolygonMode(newPoint, _Geometry._Polygon, slider.Value, !EditAdd));
                    }
                    else if (_Geometry._ScaleLine.IsMouseOver)
                    {
                        if (this.EditScaleLine)
                        {
                            ScaleLineMode captureFrameMode = _Geometry.ScaleLine_Mode(newPoint, slider.Value);
                            switch (captureFrameMode)
                            {
                                default:
                                case ScaleLineMode.Move:
                                    canvas.Cursor = Cursors.Hand;
                                    break;
                                //case ScaleLineMode.ResizeLeft:
                                //    canvas.Cursor = Cursors.ScrollW;
                                //    break;
                                case ScaleLineMode.ResizeRight:
                                    canvas.Cursor = Cursors.ScrollAll;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        canvas.Cursor = Cursors.Arrow;
                    }
                }
                else
                {
                    canvas.Cursor = Cursors.Arrow;
                }
            }
            else if (_RectangleMove)
            {
                // Move capture frame
                double xDif = newPoint.X - _RectangleLastPoint.X;
                double yDif = newPoint.Y - _RectangleLastPoint.Y;
                double width;
                double height;

                switch (_CaptureFrameMode)
                {
                    default:
                    case RectangleMode.Move:
                        _Geometry.RectanglePosition = new Point(_RectangleLastPosition.X + xDif, _RectangleLastPosition.Y + yDif);
                        break;
                    case RectangleMode.ResizeBottomLeft:
                        width = _RectangleLastWidth - xDif;
                        height = _RectangleLastHeight + yDif;
                        _Geometry.SetRectangleSize(width, height);
                        if (height > _Rectangle.MinHeight && width > _Rectangle.MinWidth)
                            _Geometry.RectanglePosition = new Point(_RectangleLastPosition.X + xDif, _RectangleLastPosition.Y);
                        break;
                    case RectangleMode.ResizeBottomRight:
                        width = _RectangleLastWidth + xDif;
                        height = _RectangleLastHeight + yDif;
                        _Geometry.SetRectangleSize(width, height);
                        break;
                    case RectangleMode.ResizeTopLeft:
                        width = _RectangleLastWidth - xDif;
                        height = _RectangleLastHeight - yDif;
                        _Geometry.SetRectangleSize(width, height);
                        if (height > _Rectangle.MinHeight && width > _Rectangle.MinWidth)
                            _Geometry.RectanglePosition = new Point(_RectangleLastPosition.X + xDif, _RectangleLastPosition.Y + yDif);
                        break;
                    case RectangleMode.ResizeTopRight:
                        width = _RectangleLastWidth + xDif;
                        height = _RectangleLastHeight - yDif;
                        _Geometry.SetRectangleSize(width, height);
                        if (height > _Rectangle.MinHeight && width > _Rectangle.MinWidth)
                            _Geometry.RectanglePosition = new Point(_RectangleLastPosition.X, _RectangleLastPosition.Y + yDif);
                        break;
                }
            }
            else if (!_ScaleLineMouseButtonPressed)
            {
                if (_Geometry != null && _Geometry._ScaleLine.IsMouseOver)
                {
                    this.SetScaleLineCursor(_Geometry.ScaleLine_Mode(newPoint, slider.Value));
                }
                else
                {
                    canvas.Cursor = Cursors.Arrow;
                }
            }
            else if (_ScaleLineMove && this._Geometry._ScaleText.Text.Length == 0) // && ! this._Geometry.ScaleTextVisible)
            {
                double xDif = newPoint.X - _ScaleLineLastPoint.X;
                double yDif = newPoint.Y - _ScaleLineLastPoint.Y;
                double width;
                switch (_ScaleLineMode)
                {
                    case ScaleLineMode.Move:
                        _Geometry.ScaleLinePosition = new Point(_ScaleLineLastPosition.X + xDif, _ScaleLineLastPosition.Y + yDif);
                        break;
                    case ScaleLineMode.ResizeLeft:
                        width = _ScaleLineLastWidth - xDif;
                        _Geometry.SetScaleLineSize(width);// _ScaleLineLastWidth + xDif, false);//, _ScaleLineLastWidth - yDif);
                        if (width > _ScaleLine.MinWidth)
                            _Geometry.ScaleLinePosition = new Point(_ScaleLineLastPosition.X + xDif, _ScaleLineLastPosition.Y);
                        break;
                    case ScaleLineMode.ResizeRight:
                        width = _ScaleLineLastWidth + xDif;
                        _Geometry.SetScaleLineSize(width);// _ScaleLineLastWidth + xDif, true);//, _ScaleLineLastWidth - yDif);
                        break;
                }
            }
            else if (!_EditPointsMouseButtonPressed)
            {

            }
            else if (_EditPoints)
            {
                if (_EditPointsMouseButtonPressed)
                {
                    double xDif = newPoint.X - _PolygonLastPoint.X;
                    double yDif = newPoint.Y - _PolygonLastPoint.Y;

                    switch (_polygonMode)
                    {
                        case PolygonMode.Move:
                            _Geometry.PolygonMove(xDif, yDif);
                            _PolygonLastPoint = new Point(newPoint.X, newPoint.Y);
                            //_Geometry.PolygonMove(_PolygonLastPoint, newPoint);
                            break;
                        case PolygonMode.EditPoint:
                            _Geometry.PolygonEditPoint(_PolygonLastPoint, newPoint, slider.Value);
                            //_Geometry.PolygonEditPoint(newPoint, xDif, yDif);
                            _PolygonLastPoint = new Point(newPoint.X, newPoint.Y);
                            break;
                        case PolygonMode.AddPoint:
                            break;
                        case PolygonMode.RemovePoint:
                            break;
                    }
                }
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ResetEditParameters();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Save current position
            _RectangleLastPoint = e.GetPosition(canvas);
            _RectangleMouseButtonPressed = true;
            if (EditPoints)
                _PolygonLastPoint = e.GetPosition(canvas);

            if (EditPoints)
                _EditPointsMouseButtonPressed = true;

            if (_Geometry != null)
            {
                _RectangleLastPosition = _Geometry.RectanglePosition;
                _RectangleLastWidth = _Geometry.RectangleWidth;
                _RectangleLastHeight = _Geometry.RectangleHeight;
                if (_Geometry._Rectangle.IsMouseOver)
                {
                    SetRectangleCursor(_CaptureFrameMode = _Geometry.ChangeRectangleMode(_RectangleLastPoint, slider.Value));
                    _RectangleMove = true;
                }
                if (EditPoints)
                {
                    if (_Geometry._Polygon.IsMouseOver)
                    {
                        int p = 0;
                        SetPolygonCursor(_polygonMode = _Geometry.ChangePolygonMode(_PolygonLastPoint, _Geometry._Polygon, slider.Value));
                        if (EditDelete)
                        {
                            _Geometry.PolygonDeletePoint(_PolygonLastPoint, slider.Value);
                            EditDelete = false;
                        }
                        else if (EditAdd)
                        {
                            _Geometry.PolygonAddPoint(_PolygonLastPoint);
                            EditAdd = false;
                        }
                    }
                }
            }

            _ScaleLineLastPoint = e.GetPosition(canvas);
            _ScaleLineMouseButtonPressed = true;

            if (_Geometry != null)
            {
                _ScaleLineLastPosition = _Geometry.ScaleLinePosition;
                _ScaleLineLastWidth = _Geometry.ScaleLineLength;
                if (_Geometry._ScaleLine.IsMouseOver)
                {
                    SetScaleLineCursor(_ScaleLineMode = _Geometry.ScaleLine_Mode(_ScaleLineLastPoint, slider.Value));
                    _ScaleLineMove = true;
                }
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ResetEditParameters();
        }

        private void ResetEditParameters()
        {
            _RectangleMove = false;
            _RectangleMouseButtonPressed = false;
            _ScaleLineMove = false;
            _ScaleLineMouseButtonPressed = false;
            //EditPoints = false;
            _EditPointsMouseButtonPressed = false;
            //EditScaleLine = false;
        }

        #endregion

        #region Image

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            _MouseButtonPressed = false;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _MouseButtonPressed = false;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Save current position
            _LastPoint = e.GetPosition(image);
            _MouseButtonPressed = true;
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            Point newPoint = e.GetPosition(image);

            if (_MouseButtonPressed)
            {
                // Compute shift vector
                double xDif = (newPoint.X - _LastPoint.X) * slider.Value;
                double yDif = (newPoint.Y - _LastPoint.Y) * slider.Value;

                // Adapt canvas position
                _CanvasThickness.Left += xDif * _InverseX;
                _CanvasThickness.Right -= xDif * _InverseX;
                _CanvasThickness.Top += yDif * _InverseY; ;
                _CanvasThickness.Bottom -= yDif * _InverseY;

                // Set new position
                canvas.Margin = _CanvasThickness;

                this._Geometry.ScaleLinePosition = new Point(-canvas.Margin.Left / slider.Value, -canvas.Margin.Top / slider.Value);
                //this._ScaleLine.X1 = -canvas.Margin.Left;
                //this._ScaleLine.Y1 = -canvas.Margin.Top;
                //this._ScaleLine.Y2 = -canvas.Margin.Top;

            }
            //else if (_EditPoints)
            //{
            //    if (!_EditPointsMouseButtonPressed)
            //    {

            //    }
            //}
        }

        private void Image_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(image);
            double zoom = 0.1 * slider.Value;
#if DEBUG
            //zoom = 0.1;
#endif
            if (e.Delta > 0)
            {
                slider.Value += zoom;
            }
            if (e.Delta < 0)
            {
                slider.Value -= zoom;
            }

            e.Handled = true;
        }



        private string _CurrentImageFile = "";
        private static System.Collections.Generic.Dictionary<string, Point> _ImageDimensions = new Dictionary<string, Point>();

        public void SetImage(string FilePath)
        {
            if (FilePath == _CurrentImageFile && this._imageHeight != null && this._imageWidth != null)
                return;
            try
            {
                bool ClearGeometry = FilePath.Length == 0 || (image.Source != null && FilePath != image.Source.ToString());
                if (FilePath.Length > 0)// && !FilePath.StartsWith("http"))
                {
                    image.Stretch = Stretch.Uniform;
                    if (!FilePath.StartsWith("http"))
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(FilePath);
                        if (!file.Exists)
                        {
                            image.Source = null;
                            _CurrentImageFile = "";
                            return;
                        }
                    }
                    BitmapImage bitmapImage = new BitmapImage(new Uri(FilePath, UriKind.RelativeOrAbsolute));
                    image.Source = bitmapImage;
                    // not possible for websites with zoomed images. Seems necessary to get image a second time
                    if (bitmapImage.Width == 1 && bitmapImage.Height == 1)
                    {
                        //ImageSource I_CCV = bitmapImage.CloneCurrentValue();
                        //Freezable I_C = bitmapImage.GetCurrentValueAsFrozen();

                        if (_ImageDimensions.ContainsKey(FilePath))
                        {
                            this._imageWidth = _ImageDimensions[FilePath].X;
                            this._imageHeight = _ImageDimensions[FilePath].Y;
                        }
                        else
                        {
                            this._imageWidth = null;
                            this._imageHeight = null;
                        }
                    }
                    else
                    {
                        this._imageWidth = bitmapImage.Width;
                        this._imageHeight = bitmapImage.Height;
                        if (!_ImageDimensions.ContainsKey(FilePath))
                        {
                            _ImageDimensions.Add(FilePath, new Point((double)this._imageWidth, (double)this._imageHeight));
                        }
                    }
                    this.ReorientCanvas();
#if xDEBUG
                    switch(this._ZoomType)
                    {
                        case ZoomType.Adapt:
                            this.ZoomAdapt();
                            break;
                        case ZoomType.Detail:
                            this.ZoomDetail();
                            break;
                    }
#endif
                }
                else
                {
                    image.Source = null;
                    _Geometry.ClearScaleLine();
                }
                if (ClearGeometry)
                {
                    _Geometry.ClearParentGeometry();
                    _Geometry.ClearRectangleGeometry();
                    _Geometry.ClearScaleLine();
                }
                _CurrentImageFile = FilePath;
            }
            catch(System.Exception ex)
            {
                image.Source = null;
                _CurrentImageFile = "";
#if DEBUG
                System.Windows.MessageBox.Show(ex.Message);
#endif
            }
        }

        private double? _imageWidth = null;
        private double? _imageHeight = null;

        public double? ImageWidth()
        {
            return this._imageWidth;
        }

        #endregion

        #region Geometry
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            _Geometry.ClearRectangleGeometry();
        }


        private void ButtonAddRectangle_Click(object sender, RoutedEventArgs e)
        {
            _Geometry.ClearRectangleGeometry();
            //_Geometry.SetRectanglePosition(_CanvasThickness.Left, _CanvasThickness.Top);
            _Geometry.SetRectangleSize(_RectangleWidth/slider.Value, _RectangleHeight/slider.Value);//, -_CanvasThickness.Left, -_CanvasThickness.Top);
            _Geometry.ShowRectangle(-_CanvasThickness.Left / slider.Value, -_CanvasThickness.Top / slider.Value);
            this.SetAddButtonsEnabled();
        }

        public void SetAddButtonsEnabled(bool Enable = true)
        {
#if !NoPolygone
            //this.buttonAddPolygone.IsEnabled = Enable;
#endif
            this.buttonAddRectangle.IsEnabled = Enable;
            if (!Enable)
            {
                this.buttonAddRectangle.Foreground = Brushes.Gray;
                //this.buttonAddPolygone.Foreground = Brushes.Gray;
            }
            //this.buttonAddNext.IsEnabled = Enable;
            EditPoints = false;
        }

        private void ButtonAddNext_Click(object sender, RoutedEventArgs e)
        {
            _Geometry.SetRectangleSize(_RectangleWidth, _RectangleHeight);//, -_CanvasThickness.Left, -_CanvasThickness.Top);
            double CenterX = canvas.ActualWidth / 2 + _CanvasThickness.Left;
            double CenterY = canvas.ActualHeight / 2 + _CanvasThickness.Top;
            double RectX = CenterX + _CanvasThickness.Left;
            double RectY = CenterY + _CanvasThickness.Top;
            _Geometry.ShowRectangle(-_CanvasThickness.Left, -_CanvasThickness.Top);
        }

        private void ButtonAddPoint_Click(object sender, RoutedEventArgs e)
        {
            //_Geometry.ClearRectangleGeometry();
            //_Geometry.SetRectangleSize(_RectangleWidth, _RectangleHeight);//, -_CanvasThickness.Left, -_CanvasThickness.Top);
            //_Geometry.ShowRectangle(-_CanvasThickness.Left, -_CanvasThickness.Top);
            //this.SetAddButtonsEnabled();
        }

        #endregion

        #region Zoom

        private void Button100_Click(object sender, RoutedEventArgs e)
        {
            lastMousePositionOnTarget = null;
            this.ReorientCanvas();
        }

        private void ReorientCanvas()
        {
            _CanvasThickness = new Thickness(0, 0, 0, 0);
            canvas.Margin = _CanvasThickness;
            _ZoomFactor = 1;
            _Scale.ScaleX = _Scale.ScaleY = _ZoomFactor;
            slider.Value = 1;
            this.button100.Foreground = Brushes.Black;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this._Geometry.Zoomfactor = slider.Value;

            this.Zoom(slider.Value);
            if (this.lastMousePositionOnTarget != null)
            {
                Point pointOld = (Point)lastMousePositionOnTarget;
                Point pointNew = Mouse.GetPosition(image);

                double DiffX = -(pointNew.X - pointOld.X) * slider.Value;
                double DiffY = -(pointNew.Y - pointOld.Y) * slider.Value;

                // Adapt canvas position
                _CanvasThickness.Left -= DiffX;
                _CanvasThickness.Right += DiffX;
                _CanvasThickness.Top -= DiffY;
                _CanvasThickness.Bottom += DiffY;

                // Set new position
                canvas.Margin = _CanvasThickness;

                if (this._Geometry.ScaleLineVisible)
                {
                    this._Geometry.ScaleLinePosition = new Point(-_CanvasThickness.Left / slider.Value, -_CanvasThickness.Top / slider.Value);
                    //_ScaleLine.StrokeThickness = 4.0 / slider.Value;// (slider.Value * 2);
                    //_ScaleLine.Opacity = 0.5;
                }

            }
            this.lastMousePositionOnTarget = null;
        }

        private void Zoom(double Value)
        {
            _ZoomFactor = Value;
            if (Value != slider.Value)
                slider.Value = Value;
            // Scale canvas according to zoom factor
            _Scale.ScaleX = _Scale.ScaleY = _ZoomFactor;
            ZoomFactor.Text = System.Math.Round(_ZoomFactor * 100, 0).ToString() + "%";
            this.button100.Foreground = Brushes.Gray;
            if (this._Geometry.ScaleLineVisible)
            {
                //this._Geometry._ScaleLine.StrokeThickness = 8 / Value;


                //this._Geometry.ScaleLinePosition = new Point(-_CanvasThickness.Left / slider.Value, -_CanvasThickness.Top / slider.Value);

                //this._Geometry._ScaleLine.X1 = 4 + _CanvasThickness.Left;
                //this._Geometry._ScaleLine.Y1 = 4 + _CanvasThickness.Top;
                //this._Geometry._ScaleLine.Y2 = 4 + _CanvasThickness.Top;
                if (this._Geometry.ScaleTextVisible)
                {
                    this._Geometry._ScaleText.FontSize = 12 / Value;
                    //this._Geometry.ScaleLinePosition = new Point((-_CanvasThickness.Left) / Value, 4 - (_CanvasThickness.Top) / Value);
                    //this._Geometry.ScaleLinePosition = new Point((-_CanvasThickness.Left) * Value, 4 - (_CanvasThickness.Top) * Value);
                    //this._Geometry.ScaleLinePosition = new Point((-_CanvasThickness.Left), (_CanvasThickness.Top));
                }
            }
        }

        private void ButtonZoomAdapt_Click(object sender, RoutedEventArgs e)
        {
            lastMousePositionOnTarget = null;
            this.ZoomAdapt();
            setZoomType(ZoomType.Adapt);
        }

        private void ZoomAdapt()
        {
#if xDEBUG
            // funktioniert leider nicht - andere Loesung gesucht
            if (this.image.Source != null && _imageWidth == null && _imageHeight == null)
            {
                string Source = this.image.Source.ToString();
                this.image.Source = null;
                this.SetImage(Source);
            }
#endif
            if (_imageWidth != null && _imageHeight != null)
            {
                _CanvasThickness = new Thickness(0, 0, 0, 0);
                canvas.Margin = _CanvasThickness;
                double dW = this.canvas.ActualWidth / (double)_imageWidth;
                double dH = this.canvas.ActualHeight / (double)_imageHeight;
                double factor = dW;
                if (dW > dH) factor = dH;
                if (factor < 0.1) factor = 0.1;
                this.Zoom(factor);
            }
            else if (this.image.Source != null)
            {
                // Show only once
                if (!_ZoomAdaptMessageShown)
                {
#if DEBUG
                    //MessageBox.Show("The original version of current image seems to have a zooming interface.\r\n\r\nImage is rescanned to capture the size of the image", "Missing image size", MessageBoxButton.OK, MessageBoxImage.Error);
#endif
                }
                _ZoomAdaptMessageShown = true;
            }
            //else this.Zoom(1);
        }

        private bool _ZoomAdaptMessageShown = false;

        private void setZoomType(ZoomType type)
        {
#if xDEBUG
            this._ZoomType = type;
            switch(type)
            {
                case ZoomType.Adapt:
                    this.button100.Foreground = Brushes.Gray;
                    this.buttonZoomAdapt.Background = Brushes.Pink;
                    this.buttonZoomDetail.Background = Brushes.LightGray;
                    break;
                case ZoomType.Detail:
                    this.button100.Foreground = Brushes.Gray;
                    this.buttonZoomDetail.Background = Brushes.Pink;
                    this.buttonZoomAdapt.Background = Brushes.LightGray;
                    break;
                case ZoomType.Ori:
                    break;
                default:
                    break;
            }
#endif
        }

        private void ButtonZoomDetail_Click(object sender, RoutedEventArgs e)
        {
            this.ZoomDetail();
            this.setZoomType(ZoomType.Detail);
        }

        private void ZoomDetail(string ParentGeometry = "", double Border = 0.1)
        {
            if (HasPolygon() && _imageWidth != null && _imageHeight != null)
            {
                _CanvasThickness = new Thickness(0, 0, 0, 0);
                canvas.Margin = _CanvasThickness;
                double x = 0;
                double X = 0;
                double y = 0;
                double Y = 0;
                foreach(Point point in this._Geometry._Polygon.Points)
                {
                    if (point.X > X) X = point.X;
                    if (x == 0 || x > point.X) x = point.X;
                    if (point.Y > Y) Y = point.Y;
                    if (y == 0 || y > point.Y) y = point.Y;
                }
                if (ParentGeometry.Length > 0)
                {
                    Polygon parent = this.GetPolygon(ParentGeometry);
                    foreach (Point point in parent.Points)
                    {
                        if (point.X > X) X = point.X;
                        if (x == 0 || x > point.X) x = point.X;
                        if (point.Y > Y) Y = point.Y;
                        if (y == 0 || y > point.Y) y = point.Y;
                    }
                }
                if (Border > 0 && Border < 1)
                {
                    double borderWidth = (((X - x) + (Y - y)) / 2) * Border;
                    X += borderWidth;
                    x -= borderWidth;
                    Y += borderWidth;
                    y -= borderWidth;
                }
                double DetailWidth = (X - x);
                double DetailHeight = (Y - y);
                //double diW = this.canvas.ActualWidth / (double)_imageWidth;
                //double diH = this.canvas.ActualHeight / (double)_imageHeight;
                //double ddW = DetailWidth / (double)_imageWidth;
                //double ddH = DetailHeight / (double)_imageHeight;
                double dcW = this.canvas.ActualWidth / DetailWidth;
                double dcH = this.canvas.ActualHeight / DetailHeight;
                //double dW = (X-x) / (double)_imageWidth;
                //double dH = (Y-y) / (double)_imageHeight;

                //double factor = diW / ddW;
                //if (diW / ddW > diH / ddH) factor = diH / ddH;
                //if (factor < 0.1) factor = 0.1;

                double factor = dcW; if (dcW > dcH) factor = dcH;

                double shiftX = factor * -x;
                double shiftY = factor * -y;

                _CanvasThickness = new Thickness(shiftX, shiftY, -shiftX, -shiftY); //-868.16484536593873  -141.2403765971431
                canvas.Margin = _CanvasThickness;
                this.Zoom(factor);
            }
        }

        private void ZoomDetail(double x, double X, double y, double Y)
        {
            double DetailWidth = (X - x);
            double DetailHeight = (Y - y);
            double dcW = this.canvas.ActualWidth / DetailWidth;
            double dcH = this.canvas.ActualHeight / DetailHeight;

            double factor = dcW; if (dcW > dcH) factor = dcH;

            double shiftX = factor * -x;
            double shiftY = factor * -y;

            _CanvasThickness = new Thickness(shiftX, shiftY, -shiftX, -shiftY);
            canvas.Margin = _CanvasThickness;
            this.Zoom(factor);
        }

        #endregion

        #region Rectangle and Polygon

        public bool RectanglePresent() { return this._Geometry.RectanglePresent; }
        public bool PolygonPresent() { return this._Geometry.PolygonPresent; }

        public RectangleGeometry GetRectangle() { return this._Geometry.GetRectangle(); }

        public void SetRectangleAndPolygonGeometry(string Geometry, string ParentGeometry = "")
        {
            try
            {
                // Remove object from previous query
                _Geometry.ClearRectangleGeometry();

                // Parent should be in background - so this has to be drawn first
                if (ParentGeometry.Length > 0)
                {
                    Polygon parent = this.GetPolygon(ParentGeometry);
                    if (this.PolygonHasPoints(parent))
                        _Geometry.PolygonSetParent(parent);
                    else
                        _Geometry.ClearParentGeometry();
                }
                else
                {
                    _Geometry.ClearParentGeometry();
                }

                if (Geometry.Length > 0)
                {
                    Polygon polygon = this.GetPolygon(Geometry);

                    if (!this.PolygonHasPoints(polygon))
                    {
                        this.buttonEditPoints.IsEnabled = false;
                    }
                    else
                    {

                        _Geometry.SetPolygon(polygon);
                        this.buttonEditPoints.IsEnabled = true;

                        this.ZoomDetail(ParentGeometry);

                        if (false)
                        {
                            // ensure visibility of polygon in canvas
                            // get center and dimension of polygon
                            double xPos = 0;
                            double yPos = 0;
                            double xLeft = 0;
                            double xRight = 0;
                            double yTop = 0;
                            double yBot = 0;

                            foreach (Point P in polygon.Points)
                            {
                                xPos += P.X;
                                yPos += P.Y;
                                if (xLeft > P.X || xLeft == 0)
                                    xLeft = P.X;
                                if (xRight < P.X)
                                    xRight = P.X;
                                if (yTop > P.Y || yTop == 0)
                                    yTop = P.Y;
                                if (yBot < P.Y)
                                    yBot = P.Y;

                            }
                            xPos = xPos / polygon.Points.Count;
                            yPos = yPos / polygon.Points.Count;
                            // adapt to zoom
                            xPos = xPos * _ZoomFactor;
                            yPos = yPos * _ZoomFactor;
                            yTop = yTop * _ZoomFactor;
                            yBot = yBot * _ZoomFactor;
                            xLeft = xLeft * _ZoomFactor;
                            xRight = xRight * _ZoomFactor;
                            // visible Canvas range
                            double CanvasTop = -_CanvasThickness.Top;
                            double CanvasBot = _CanvasThickness.Bottom + canvas.ActualHeight;
                            double CanvasLeft = -_CanvasThickness.Left;
                            double CanvasRight = _CanvasThickness.Right + canvas.ActualWidth;
                            // check if visible
                            bool TopVisible = CanvasTop < yTop;
                            bool BotVisible = CanvasBot > yBot;
                            bool LeftVisible = CanvasLeft < xLeft;
                            bool RightVisible = CanvasRight > xRight;
                            // only move if not visible
                            if (!TopVisible || !BotVisible || !LeftVisible || !RightVisible)
                            {
                                double ShiftX = 0;
                                double ShiftY = 0;
                                if (!TopVisible || !BotVisible)
                                {
                                    double CanvasY = (CanvasTop + CanvasBot) / 2;
                                    ShiftY = yPos - CanvasY;
                                    if (ShiftY < _CanvasThickness.Top)
                                        ShiftY = _CanvasThickness.Top;
                                    _CanvasThickness.Bottom += ShiftY;
                                    _CanvasThickness.Top -= ShiftY;
                                }
                                if (!LeftVisible || !RightVisible)
                                {
                                    double CanvasX = (CanvasLeft + CanvasRight) / 2;
                                    ShiftX = xPos - CanvasX;
                                    if (ShiftX < _CanvasThickness.Left)
                                        ShiftX = _CanvasThickness.Left;
                                    _CanvasThickness.Right += ShiftX;
                                    _CanvasThickness.Left -= ShiftX;
                                }
                                // move canvas according to polygon
                                canvas.Margin = _CanvasThickness;
                            }
                        }

                    }
                }
                else
                {
                    EditPoints = false;
                    this.ZoomAdapt();
                }
            }
            catch(System.Exception ex)
            {

            }
        }

        private bool PolygonHasPoints(Polygon Poly)
        {
            foreach (Point p in Poly.Points)
            {
                if (p.Y > 0 || p.X > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasPolygon()
        {
            if (this._Geometry._Polygon != null && this._Geometry._Polygon.Points.Count > 3)
                return true;
            else
                return false;
        }

        public Polygon GetPolygon() { return this._Geometry._Polygon; }

        private Polygon GetPolygon(string Geometry)
        {
            Polygon polygon = null;
            try
            {
                if (Geometry.Length > 0)
                {
                    if (Geometry.StartsWith("POLYGON "))
                    {
                        System.Collections.Generic.List<Point> points = new List<Point>();
                        string PP = Geometry.Substring(Geometry.IndexOf("((") + 2);
                        PP = PP.Replace("))", "");
                        string[] pp = PP.Split(new char[] { ',' });
                        foreach (string p in pp)
                        {
                            string[] xy = p.Trim().Split(new char[] { ' ' });
                            double x;
                            double y;
                            if (double.TryParse(xy[0], out x) && double.TryParse(xy[1], out y))
                            {
                                Point point = new Point(x, y);
                                points.Add(point);
                            }
                        }
                        polygon = new Polygon();
                        foreach (Point P in points)
                            polygon.Points.Add(P);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return polygon;
        }

        private void SetRectangleCursor(RectangleMode captureFrameMode)
        {
            switch (captureFrameMode)
            {
                default:
                case RectangleMode.Move:
                    canvas.Cursor = Cursors.Hand;
                    break;
                case RectangleMode.ResizeBottomLeft:
                    canvas.Cursor = Cursors.ScrollSW;
                    break;
                case RectangleMode.ResizeTopRight:
                    canvas.Cursor = Cursors.ScrollNE;
                    break;
                case RectangleMode.ResizeBottomRight:
                    canvas.Cursor = Cursors.ScrollSE;
                    break;
                case RectangleMode.ResizeTopLeft:
                    canvas.Cursor = Cursors.ScrollNW;
                    break;
                case RectangleMode.EditPoint:
                    canvas.Cursor = Cursors.Pen;
                    break;
            }
        }

        private void SetPolygonCursor(PolygonMode captureFrameMode)
        {
            switch (captureFrameMode)
            {
                default:
                case PolygonMode.Move:
                    canvas.Cursor = Cursors.Hand;
                    break;
                //case PolygonMode.AddPoint:
                //    canvas.Cursor = Cursors.UpArrow;
                //    break;
                case PolygonMode.RemovePoint:
                    canvas.Cursor = Cursors.Arrow;
                    break;
                case PolygonMode.EditPoint:
                    if(EditAdd)
                        canvas.Cursor = Cursors.Cross;
                    else if (EditDelete)
                        canvas.Cursor = Cursors.UpArrow;
                    else
                        canvas.Cursor = Cursors.Pen;
                    break;
            }
        }

        #region Edit

        private void ButtonEditPoints_Click(object sender, RoutedEventArgs e)
        {
            EditPoints = !EditPoints;
        }

        private void ButtonEditAdd_Click(object sender, RoutedEventArgs e)
        {
            EditAdd = !EditAdd;
        }

        private void ButtonEditDelete_Click(object sender, RoutedEventArgs e)
        {
            EditDelete = !EditDelete;
        }

        private bool EditPoints
        {
            get { return _EditPoints; }
            set
            {
                _EditPoints = value;
                if (_EditPoints)
                {
                    buttonEditPoints.BorderBrush = Brushes.Orange;
                    buttonEditPoints.Background = Brushes.Yellow;
                    buttonEditPoints.BorderThickness = new System.Windows.Thickness(2);
                    buttonAdd.IsEnabled = true;
                    buttonDelete.IsEnabled = true;
                    this.EditScaleLine = false;
                }
                else
                {
                    buttonEditPoints.BorderBrush = Brushes.Gray;
                    buttonEditPoints.Background = Brushes.LightGray;// "#FF707070";
                    buttonEditPoints.BorderThickness = new System.Windows.Thickness(1);
                    buttonAdd.IsEnabled = false;
                    buttonDelete.IsEnabled = false;
                }
                EditAdd = false;
                EditDelete = false;
                this.buttonEditPoints.IsEnabled = this.HasPolygon();
            }
        }

        private bool EditAdd
        {
            get { return _EditAdd; }
            set
            {
                _EditAdd = value;
                if (_EditAdd)
                {
                    buttonAdd.BorderBrush = Brushes.Green;
                    buttonAdd.Background = Brushes.LightGreen;
                    buttonAdd.BorderThickness = new System.Windows.Thickness(2);
                    EditDelete = false;
                    buttonEditPoints.BorderBrush = Brushes.Green;
                }
                else
                {
                    buttonAdd.BorderBrush = Brushes.Gray;
                    buttonAdd.Background = Brushes.LightGray;// "#FF707070";
                    buttonAdd.BorderThickness = new System.Windows.Thickness(1);
                    if (!EditDelete)
                    {
                        if (_EditPoints)
                            buttonEditPoints.BorderBrush = Brushes.Orange;
                        else
                            buttonEditPoints.BorderBrush = Brushes.Gray;
                    }
                }
                if (_EditPoints)
                    buttonAdd.Foreground = Brushes.Green;
                else
                    buttonAdd.Foreground = Brushes.Gray;
            }
        }

        private bool EditDelete
        {
            get { return _EditDelete; }
            set
            {
                _EditDelete = value;
                if (_EditDelete)
                {
                    buttonDelete.BorderBrush = Brushes.Red;
                    buttonDelete.Background = Brushes.Pink;
                    buttonDelete.BorderThickness = new System.Windows.Thickness(2);
                    EditAdd = false;
                    buttonEditPoints.BorderBrush = Brushes.Red;
                }
                else
                {
                    buttonDelete.BorderBrush = Brushes.Gray;
                    buttonDelete.Background = Brushes.LightGray;// "#FF707070";
                    buttonDelete.BorderThickness = new System.Windows.Thickness(1);
                    if (!EditAdd)
                    {
                        if (_EditPoints)
                            buttonEditPoints.BorderBrush = Brushes.Orange;
                        else
                            buttonEditPoints.BorderBrush = Brushes.Gray;
                    }
                }
                if (_EditPoints)
                    buttonDelete.Foreground = Brushes.Red;
                else
                    buttonDelete.Foreground = Brushes.Gray;
            }
        }

        #endregion

        #endregion

        #region Scale

        public bool ScaleLineVisible() { return this._Geometry.ScaleLineVisible; }
        public bool ScaleTextVisible() { return this._Geometry.ScaleTextVisible; }

        public LineGeometry GetScaleLine() { return this._Geometry.GetScaleLine(); }

        public void SetScaleLine(string Geometry)
        {
            try
            {
                if (Geometry.Length > 0)
                {
                    double width;
                    if (double.TryParse(Geometry, out width))
                    {
                        if (this.ImageWidth() != null)
                        {
                            double widthImage = (double)this.ImageWidth();
                            double widthMeter = widthImage / width;
                            double ScaleFactor = 1;
                            if (widthMeter < widthImage)
                            {
                                while (widthImage / (widthMeter * ScaleFactor) > 10)
                                {
                                    ScaleFactor = ScaleFactor * 10;
                                }
                            }
                            else
                            {
                                while (widthImage / (widthMeter * ScaleFactor) < 1)
                                {
                                    ScaleFactor = ScaleFactor / 10;
                                }
                            }
                            _ScaleLineLastPoint = new Point(0, 4);
                            _ScaleLineLastPosition = new Point(0, 4);

                            _Geometry.SetScaleLineSize(widthMeter * ScaleFactor);
                            _Geometry.ScaleLinePosition = _ScaleLineLastPoint;
                            _Geometry.ShowScaleLine(ScaleFactor.ToString() + " m", this.slider.Value);
                        }
                    }
                }
                else
                {
                    _Geometry.ClearScaleLine();
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        //bool _SettingScale = false;
        private void ButtonScale_Click(object sender, RoutedEventArgs e)
        {
            _Geometry.ClearScaleLine();
            //_ScaleLineLastPoint = new Point(2, 6);
            //_ScaleLineLastPosition = new Point(2, 6);
            _ScaleLineLastPoint = new Point(2 - _CanvasThickness.Left / slider.Value, 6 -_CanvasThickness.Top / slider.Value);
            _ScaleLineLastPosition = new Point(2 - _CanvasThickness.Left / slider.Value, 6 - _CanvasThickness.Top / slider.Value);
            _Geometry.SetScaleLineSize(88 / slider.Value);
            _Geometry.ScaleLinePosition = _ScaleLineLastPoint;
            _Geometry.ShowScaleLine("", slider.Value);
            EditScaleLine = true;
        }

        private bool _EditScaleLine = false;
        public bool EditScaleLine
        {
            get { return _EditScaleLine; }
            set { _EditScaleLine = value; }
        }

        public void ScaleCanEdit(bool CanEdit)
        {
            this.buttonScale.IsEnabled = CanEdit;
        }

        private void SetScaleLineCursor(ScaleLineMode scaleMode)
        {
            switch (scaleMode)
            {
                default:
                case ScaleLineMode.Move:
                    canvas.Cursor = Cursors.Hand;
                    break;
                //case ScaleLineMode.ResizeLeft:
                //    canvas.Cursor = Cursors.ScrollW;
                //    break;
                case ScaleLineMode.ResizeRight:
                    canvas.Cursor = Cursors.ScrollAll;
                    break;
            }
        }

        #endregion

        #region Points

        public PointCollection GetPoints() { return this._Geometry.GetPoints(); }

        public void SetPoints(string Geometry)
        {
            try
            {
                // Remove object from previous query
                _Geometry.ClearRectangleGeometry();

                if (Geometry.Length > 0)
                {
                    Polygon polygon = this.GetPolygon(Geometry);

                    if (!this.PolygonHasPoints(polygon))
                    {
                        //_Geometry.ClearRectangleGeometry();
                        this.buttonEditPoints.IsEnabled = false;
                    }
                    else
                    {
                        //_Geometry.ClearRectangleGeometry();
                        _Geometry.SetPolygon(polygon);
                        this.buttonEditPoints.IsEnabled = true;

                        // ensure visibility of polygon in canvas
                        // get center and dimension of polygon
                        double xPos = 0;
                        double yPos = 0;
                        double xLeft = 0;
                        double xRight = 0;
                        double yTop = 0;
                        double yBot = 0;

                        foreach (Point P in polygon.Points)
                        {
                            xPos += P.X;
                            yPos += P.Y;
                            if (xLeft > P.X || xLeft == 0)
                                xLeft = P.X;
                            if (xRight < P.X)
                                xRight = P.X;
                            if (yTop > P.Y || yTop == 0)
                                yTop = P.Y;
                            if (yBot < P.Y)
                                yBot = P.Y;

                        }
                        xPos = xPos / polygon.Points.Count;
                        yPos = yPos / polygon.Points.Count;
                        // adapt to zoom
                        xPos = xPos * _ZoomFactor;
                        yPos = yPos * _ZoomFactor;
                        yTop = yTop * _ZoomFactor;
                        yBot = yBot * _ZoomFactor;
                        xLeft = xLeft * _ZoomFactor;
                        xRight = xRight * _ZoomFactor;
                        // visible Canvas range
                        double CanvasTop = -_CanvasThickness.Top;
                        double CanvasBot = _CanvasThickness.Bottom + canvas.ActualHeight;
                        double CanvasLeft = -_CanvasThickness.Left;
                        double CanvasRight = _CanvasThickness.Right + canvas.ActualWidth;
                        // check if visible
                        bool TopVisible = CanvasTop < yTop;
                        bool BotVisible = CanvasBot > yBot;
                        bool LeftVisible = CanvasLeft < xLeft;
                        bool RightVisible = CanvasRight > xRight;
                        // only move if not visible
                        if (!TopVisible || !BotVisible || !LeftVisible || !RightVisible)
                        {
                            double ShiftX = 0;
                            double ShiftY = 0;
                            if (!TopVisible || !BotVisible)
                            {
                                double CanvasY = (CanvasTop + CanvasBot) / 2;
                                ShiftY = yPos - CanvasY;
                                if (ShiftY < _CanvasThickness.Top)
                                    ShiftY = _CanvasThickness.Top;
                                _CanvasThickness.Bottom += ShiftY;
                                _CanvasThickness.Top -= ShiftY;
                            }
                            if (!LeftVisible || !RightVisible)
                            {
                                double CanvasX = (CanvasLeft + CanvasRight) / 2;
                                ShiftX = xPos - CanvasX;
                                if (ShiftX < _CanvasThickness.Left)
                                    ShiftX = _CanvasThickness.Left;
                                _CanvasThickness.Right += ShiftX;
                                _CanvasThickness.Left -= ShiftX;
                            }
                            // move canvas according to polygon
                            canvas.Margin = _CanvasThickness;
                        }

                    }
                }
                else
                {
                    //_Geometry.ClearRectangleGeometry();
                    EditPoints = false;
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        #endregion

    }
}
