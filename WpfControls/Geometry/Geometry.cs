using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;


namespace WpfControls.Geometry
{

    #region Public enumerations

    /// <summary>
    /// Enumeration of the change mode for frame move or resize according to catched position
    /// </summary>
    public enum RectangleMode
    {
        /// <summary>
        /// Move capture frame
        /// </summary>
        Move,
        /// <summary>
        /// Reposition top left frame point
        /// </summary>
        ResizeTopLeft,
        /// <summary>
        /// Reposition top right frame point
        /// </summary>
        ResizeTopRight,
        /// <summary>
        /// Reposition bottom left frame point
        /// </summary>
        ResizeBottomLeft,
        /// <summary>
        /// Reposition bottom right frame point
        /// </summary>
        ResizeBottomRight,
        /// <summary>
        /// edit point
        /// </summary>
        EditPoint
    };

    public enum ScaleLineMode
    {
        /// <summary>
        /// Move capture frame
        /// </summary>
        Move,
        /// <summary>
        /// Reposition right point
        /// </summary>
        ResizeRight,
        /// <summary>
        /// Reposition left point
        /// </summary>
        ResizeLeft
    };

    public enum PolygonMode
    {
        /// <summary>
        /// Move capture frame
        /// </summary>
        Move,
        /// <summary>
        /// Reposition point
        /// </summary>
        EditPoint,
        /// <summary>
        /// Add point
        /// </summary>
        AddPoint,
        /// <summary>
        /// Remove point
        /// </summary>
        RemovePoint
    };

    #endregion // Public enumerations


    class Geometry : FrameworkElement
    {

        #region Parameter

        internal Rectangle _Rectangle;
        internal Polygon _Polygon;
        internal Polygon _ParentPolygon;

        //internal System.Windows.Media.PointCollection _PointCollection;
        internal System.Collections.Generic.Dictionary<Point, System.Windows.Controls.TextBlock> _Points;
        //internal PointCollection _Points;

        private Point _RectanglePosition;
        private Canvas _Canvas;
        internal const double MinRectangleWidth = 1;
        internal const double MinRectangleHeight = 1;
        private const double MatchDiff = 8.0;

        internal const double MinScaleLineWidth = 1;
        internal System.Windows.Shapes.Line _ScaleLine;
        internal System.Windows.Controls.TextBlock _ScaleText;
        private Point _ScaleLinePosition = new Point(0, 3);

        public double Zoomfactor = 1.0;

        #endregion

        #region Construction

        public Geometry(Canvas canvas)
        {
            // Create rectangle
            _Rectangle = new Rectangle();

            // Init rectangle 
            _Rectangle.Stroke = Brushes.Red;
            _Rectangle.StrokeThickness = 2.0;
            _Rectangle.Fill = new SolidColorBrush(Color.FromArgb(50, 255, 255, 0));
            _Rectangle.Width = 0;
            _Rectangle.Height = 0;

            _Polygon = new Polygon();
            _Polygon.Stroke = Brushes.Red;
            _Polygon.StrokeThickness = 2.0;
            _Polygon.Fill = new SolidColorBrush(Color.FromArgb(50, 255, 255, 0));

            _ScaleLine = new Line();
            _ScaleLine.Stroke = Brushes.Blue;
            _ScaleLine.StrokeEndLineCap = PenLineCap.Flat;
            _ScaleLine.StrokeStartLineCap = PenLineCap.Flat;
            _ScaleLine.StrokeThickness = 8.0;

            _ScaleText = new TextBlock();
            _ScaleText.Foreground = Brushes.Blue;
            _ScaleText.FontWeight = FontWeights.Bold;
            _ScaleText.FontSize = 12;

            _ParentPolygon = new Polygon();
            _ParentPolygon.Stroke = Brushes.OrangeRed;
            System.Windows.Media.DoubleCollection vs = new DoubleCollection();
            vs.Add(1.0);
            vs.Add(1.0);
            _ParentPolygon.StrokeDashArray = vs;
            _ParentPolygon.StrokeThickness = 2.0;
            _ParentPolygon.Fill = new SolidColorBrush(Color.FromArgb(50, 255, 255, 0));

            _Canvas = canvas;

            // Add to parent control
            //_Canvas.Children.Add(_Rectangle);
        }

        #endregion

        #region Image

        /// <summary>
        /// The file path or uri of the image.
        /// </summary>
        private string _ImageFilePath = string.Empty;

        /// <summary>
        /// Gets or sets the image file path.
        /// </summary>
        /// <value>The image file path.</value>
        public string ImageFilePath
        {
            get { return _ImageFilePath; }
            set { _ImageFilePath = value; }
        }

        #endregion

        #region Matchpoint

        /// <summary>
        /// If a point matches the current position of the cursor
        /// </summary>
        /// <param name="point">The point that should be tested</param>
        /// <param name="cursorPos">The position of the cursor</param>
        /// <param name="Zoom">Zoom factor of the image</param>
        /// <param name="ForScaleLine">Opt.: If the Scale line should be tested</param>
        private bool MatchPoint(Point point, Point cursorPos, double Zoom, bool ForScaleLine = false)
        {
            double matchDiff = MatchDiff / Zoom;
            if (ForScaleLine) matchDiff *= 2;
            if (Math.Abs(point.X - cursorPos.X) < matchDiff && Math.Abs(point.Y - cursorPos.Y) < matchDiff)
                return true;

            return false;
        }

        private bool MatchLine(Polygon polygon, Point cursorPosition, ref int i)
        {
            bool OK = false;
            System.Collections.Generic.Dictionary<int, Line> Lines = new Dictionary<int, Line>();
            for(int p = 0; p < polygon.Points.Count - 1; p++)
            {
                Line L = new Line();
                L.X1 = polygon.Points[p].X;
                L.X2 = polygon.Points[p + 1].X;
                L.Y1 = polygon.Points[p].Y;
                L.Y2 = polygon.Points[p + 1].Y;
                Lines.Add(p, L);
            }
            foreach(System.Collections.Generic.KeyValuePair<int, Line> KV in Lines)
            {
                if (DistanceFromPointToLine(cursorPosition, KV.Value) < 1)
                {
                    OK = true;
                    i = KV.Key;
                    break;
                }
                //if (KV.Value.IsMouseOver)
                //{
                //    OK = true;
                //    i = KV.Key;
                //    break;
                //}
            }
            return OK;
        }

        public static double DistanceFromPointToLine(Point point, Line line)
        {
            // given a line based on two points, and a point away from the line,
            // find the perpendicular distance from the point to the line.
            // see http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
            // for explanation and defination.
            Point l1 = new Point(line.X1, line.Y1);
            Point l2 = new Point(line.X2, line.Y2);

            return Math.Abs((l2.X - l1.X) * (l1.Y - point.Y) - (l1.X - point.X) * (l2.Y - l1.Y)) /
                    Math.Sqrt(Math.Pow(l2.X - l1.X, 2) + Math.Pow(l2.Y - l1.Y, 2));
        }


        #endregion

        #region Object

        //public RectangleMode ChangeObjectMode(Point pos)
        //{
        //    if (MatchPoint(pos, _RectanglePosition))
        //        return RectangleMode.ResizeTopLeft;
        //    else if (MatchPoint(pos, new Point(_RectanglePosition.X, _RectanglePosition.Y + _Rectangle.Height)))
        //        return RectangleMode.ResizeBottomLeft;
        //    else if (MatchPoint(pos, new Point(_RectanglePosition.X + _Rectangle.Width, _RectanglePosition.Y + _Rectangle.Height)))
        //        return RectangleMode.ResizeBottomRight;
        //    else if (MatchPoint(pos, new Point(_RectanglePosition.X + _Rectangle.Width, _RectanglePosition.Y)))
        //        return RectangleMode.ResizeTopRight;
        //    else
        //        return RectangleMode.Move;
        //}

        #endregion

        #region Rectangle

        public bool RectanglePresent { get { return _Canvas.Children.Contains(_Rectangle); } }

        public void SetRectanglePosition(double X, double Y)
        {
            _RectanglePosition = new Point(X, Y);
        }

        public void SetRectangleSize(double width, double height)//, double X = 0, double Y = 0)
        {
            // Check minimum width
            if (width < MinRectangleWidth)
                _Rectangle.Width = MinRectangleWidth;
            else
                _Rectangle.Width = width;
            // Check minimum height
            if (height < MinRectangleHeight)
                _Rectangle.Height = MinRectangleHeight;
            else
                _Rectangle.Height = height;
            //if (X != 0 || Y != 0)
            //{
            //    _RectanglePosition = new Point(X, Y);
            //}
        }

        public void SetRectangle(Rect rectSquare)
        {
            _Rectangle.Height = rectSquare.Height;
            _Rectangle.Width = rectSquare.Width;
            _RectanglePosition = rectSquare.Location;
        }

        public void ShowRectangle(double X = 0, double Y = 0)
        {
            if (!_Canvas.Children.Contains(_Rectangle))
            {
                Canvas.SetLeft(_Rectangle, X);
                Canvas.SetTop(_Rectangle, Y);
                _RectanglePosition = new Point(X, Y);
                _Canvas.Children.Add(_Rectangle);
            }
        }

        public void ClearRectangleGeometry()
        {
            if (_Canvas.Children.Contains(_Rectangle))
                _Canvas.Children.Remove(_Rectangle);
            if (_Canvas.Children.Contains(_Polygon))
                _Canvas.Children.Remove(_Polygon);
            if (_Canvas.Children.Contains(_ParentPolygon))
                _Canvas.Children.Remove(_ParentPolygon);

            if (_Polygon != null)
                _Polygon.Points.Clear();
            //if (_Canvas.Children.Contains(_ScaleLine))
            //    _Canvas.Children.Remove(_ScaleLine);
        }

        public void ClearParentGeometry()
        {
            if (_Canvas.Children.Contains(_ParentPolygon))
                _Canvas.Children.Remove(_ParentPolygon);
        }

        public RectangleMode ChangeRectangleMode(Point pos, double Zoom)
        {
            if (MatchPoint(pos, _RectanglePosition, Zoom))
                return RectangleMode.ResizeTopLeft;
            else if (MatchPoint(pos, new Point(_RectanglePosition.X, _RectanglePosition.Y + _Rectangle.Height), Zoom))
                return RectangleMode.ResizeBottomLeft;
            else if (MatchPoint(pos, new Point(_RectanglePosition.X + _Rectangle.Width, _RectanglePosition.Y + _Rectangle.Height), Zoom))
                return RectangleMode.ResizeBottomRight;
            else if (MatchPoint(pos, new Point(_RectanglePosition.X + _Rectangle.Width, _RectanglePosition.Y), Zoom))
                return RectangleMode.ResizeTopRight;
            else
                return RectangleMode.Move;
        }

        public Point RectanglePosition
        {
            get
            {
                return _RectanglePosition;
            }
            set
            {
                _RectanglePosition = value;
                Canvas.SetLeft(_Rectangle, _RectanglePosition.X);
                Canvas.SetTop(_Rectangle, _RectanglePosition.Y);
            }
        }

        public Point RectanglePosition2
        {
            get
            {
                return new Point(_RectanglePosition.X + _Rectangle.Width, _RectanglePosition.Y + _Rectangle.Height);
            }
        }

        public double RectangleWidth
        {
            set { _Rectangle.Width = value; }
            get { return _Rectangle.Width; }
        }

        public double RectangleHeight
        {
            set { _Rectangle.Height = value; }
            get { return _Rectangle.Height; }
        }

        public RectangleGeometry GetRectangle()
        {
            Rect rectSquare = new Rect(RectanglePosition, RectanglePosition2);
            RectangleGeometry rectGeometry = new RectangleGeometry(rectSquare);
            return rectGeometry;
        }

        #endregion

        #region Polygon

        public bool PolygonPresent { get { return _Canvas.Children.Contains(_Polygon); } }


        public PolygonMode ChangePolygonMode(Point pos, Polygon polygon, double Zoom, bool ForPoints = true)
        {
            if (polygon.Points.Count > 3)
            {
                if (ForPoints)
                {
                    for (int i = 0; i < polygon.Points.Count; i++)
                    {
                        if (MatchPoint(pos, polygon.Points[i], Zoom))
                        {
                            return PolygonMode.EditPoint;
                        }
                    }
                }
                else
                {
                    int p = 0;
                    if (MatchLine(polygon, pos, ref p))
                    {
                        return PolygonMode.EditPoint;
                    }
                }
            }
            //foreach (Point p in polygon.Points)
            //{
            //    if (MatchPoint(pos, p))
            //    {
            //        return PolygonMode.EditPoint;
            //    }
            //}
            return PolygonMode.Move;
        }

        public void PolygonMove(Point from, Point to)
        {
            Polygon polygon = new Polygon();
            double DiffX = from.X - to.X;
            double DiffY = from.Y - to.Y;
            foreach(Point point in _Polygon.Points)
            {
                Point P = new Point(point.X - DiffX, point.Y - DiffY);
                polygon.Points.Add(P);
            }
            this.SetPolygon(polygon);
        }

        public void PolygonMove(double xDiff, double yDiff)
        {
            Polygon polygon = new Polygon();
            foreach (Point point in _Polygon.Points)
            {
                Point P = new Point(point.X + xDiff, point.Y + yDiff);
                polygon.Points.Add(P);
            }
            _Polygon.Points.Clear();
            foreach (Point P in polygon.Points)
                _Polygon.Points.Add(P);
        }

        public void PolygonDeletePoint(Point point, double Zoom)
        {
            if (_Polygon.Points.Count < 4)
                return;
            Polygon polygon = new Polygon();
            bool done = false;
            for (int i = 0; i < _Polygon.Points.Count; i++)
            {
                if (!done && MatchPoint(_Polygon.Points[i], point, Zoom))
                {
                    done = true;
                    continue;
                }
                polygon.Points.Add(_Polygon.Points[i]);
            }
            if (polygon.Points.Last().X != polygon.Points.First().X || polygon.Points.Last().Y != polygon.Points.First().Y)
            {
                Point pointClose = new Point(polygon.Points.First().X, polygon.Points.First().Y);
                polygon.Points.Add(pointClose);
            }
            _Polygon.Points.Clear();
            foreach (Point P in polygon.Points)
                _Polygon.Points.Add(P);
            ShowPolygon();
        }


        public void SetPolygon(Polygon polygon)
        {
            _Polygon.Points.Clear();
            foreach (Point P in polygon.Points)
                _Polygon.Points.Add(P);
            ShowPolygon();
        }

        public void ShowPolygon()
        {
            if (!_Canvas.Children.Contains(_Polygon))
                _Canvas.Children.Add(_Polygon);
        }

        public void PolygonSetParent(Polygon polygon)
        {
            _ParentPolygon.Points.Clear();
            foreach (Point P in polygon.Points)
                _ParentPolygon.Points.Add(P);
            if (!_Canvas.Children.Contains(_ParentPolygon))
                _Canvas.Children.Add(_ParentPolygon);
        }

        public void PolygonEditPoint(Point OldPosition, Point NewPosition, double Zoom)
        {
            bool Is0position = false;
            bool PositionDetected = false;
            Polygon polygon = new Polygon();
            for (int i = 0; i < _Polygon.Points.Count; i++)
            {
                if (MatchPoint(_Polygon.Points[i], OldPosition, Zoom) && !PositionDetected)
                {
                    polygon.Points.Add(NewPosition);
                    PositionDetected = true;
                    if (i == 0)
                        Is0position = true;
                }
                else
                {
                    if (Is0position && i == _Polygon.Points.Count -1)
                        polygon.Points.Add(NewPosition);
                    else
                        polygon.Points.Add(_Polygon.Points[i]);
                }
            }
            _Polygon.Points.Clear();
            foreach (Point P in polygon.Points)
                _Polygon.Points.Add(P);
        }

        public void PolygonEditPoint(Point CursorPosition, double DiffX, double DiffY, double Zoom)
        {
            int i = 0;
            double X = 0, Y = 0;
            bool OK = false;
            foreach (Point P in _Polygon.Points)
            {
                if (MatchPoint(P, CursorPosition, Zoom))
                {
                    X = P.X + DiffX;
                    Y = P.Y + DiffY;
                    OK = true;
                    break;
                }
                i++;
            }
            if (OK)
            {
                PolygonEditPoint(i, X, Y);
            }
        }

        public void PolygonEditPoint(int Index, double X, double Y)
        {
            Polygon polygon = new Polygon();
            for (int i = 0; i < _Polygon.Points.Count; i++)
            {
                if (i == Index)
                    polygon.Points.Add(new Point(X, Y));
                else
                    polygon.Points.Add(_Polygon.Points[i]);
            }
            _Polygon.Points.Clear();
            foreach (Point P in polygon.Points)
                _Polygon.Points.Add(P);
        }

        public void PolygonAddPoint(Point CursorPosition)
        {
            int i = 0;
            this.MatchLine(_Polygon, CursorPosition, ref i);
            Polygon polygon = new Polygon();
            for (int p = 0; p < _Polygon.Points.Count; p++)
            {
                polygon.Points.Add(_Polygon.Points[p]);
                if (p == i)
                    polygon.Points.Add(CursorPosition);
            }
            _Polygon.Points.Clear();
            foreach (Point P in polygon.Points)
                _Polygon.Points.Add(P);


            //bool OK = false;
            //foreach (Point P in _Polygon.Points)
            //{
            //    if (MatchPoint(P, CursorPosition))
            //    {
            //        OK = true;
            //        break;
            //    }
            //    i++;
            //}
            //if (OK)
            //{
            //    Polygon polygon = new Polygon();
            //    for (int p = 0; p < _Polygon.Points.Count; p++)
            //    {
            //        polygon.Points.Add(_Polygon.Points[p]);
            //        if (p == i && p < _Polygon.Points.Count - 1)
            //            polygon.Points.Add(new Point((_Polygon.Points[p].X + _Polygon.Points[p].X) / 2, (_Polygon.Points[p].Y + _Polygon.Points[p].Y) / 2));
            //    }
            //    _Polygon.Points.Clear();
            //    foreach (Point P in polygon.Points)
            //        _Polygon.Points.Add(P);
            //}
        }


        #endregion

        #region Scale

        public ScaleLineMode ScaleLine_Mode(Point pos, double Zoom)
        {
            if (MatchPoint(pos, _ScaleLinePosition, Zoom, true))
                return ScaleLineMode.ResizeLeft;
            else if (MatchPoint(pos, new Point(_ScaleLinePosition.X + ScaleLineLength, _ScaleLinePosition.Y), Zoom, true))
                return ScaleLineMode.ResizeRight;
            else
                return ScaleLineMode.Move;
        }


        public void ClearScaleLine()
        {
            if (_Canvas.Children.Contains(_ScaleLine))
                _Canvas.Children.Remove(_ScaleLine);
            if (_Canvas.Children.Contains(_ScaleText))
                _Canvas.Children.Remove(_ScaleText);
        }

        public bool ScaleLineVisible { get { return _Canvas.Children.Contains(_ScaleLine); } }
        public bool ScaleTextVisible { get { return _Canvas.Children.Contains(_ScaleText); } }


        //public void ResetScaleLine()
        //{
        //    _ScaleLine.Y1 = 5;
        //    _ScaleLine.Y2 = 5;
        //    _ScaleLine.X1 = 1;
        //    _ScaleLine.X2 = 88;
        //    this._ScaleLinePosition = new Point(1, 5);
        //}

        public void SetScaleLineSize(double width)//, double height)
        {
            // Check minimum width
            if (width < MinScaleLineWidth)
                _ScaleLine.Width = MinScaleLineWidth;
            else
            {
                _ScaleLine.Width = width;
                _ScaleLine.X2 = _ScaleLine.X1 + width;
            }
        }


        public System.Windows.Media.LineGeometry GetScaleLine()
        {
            Line ScaleLine = new Line();
            Point P1 = new Point(ScaleLinePosition.X, ScaleLinePosition.Y);
            Point P2 = new Point(ScaleLinePosition2.X, ScaleLinePosition.Y);
            LineGeometry lineGeometry = new LineGeometry(P1, P2);
            return lineGeometry;
        }

        public double ScaleLineLength
        {
            set { _ScaleLine.X2 = _ScaleLine.X1 + value; }
            get
            {
                return System.Math.Abs(_ScaleLine.X2 - _ScaleLine.X1);
            }
        }

        public void ShowScaleLine(string Scale = "", double Zoom = 1)
        {
            try
            {
                _ScaleText.Text = Scale;
                if (!_Canvas.Children.Contains(_ScaleLine))
                    _Canvas.Children.Add(_ScaleLine);
                if (!_Canvas.Children.Contains(_ScaleText))
                {
                    _Canvas.Children.Add(_ScaleText);
                }
                if (Scale.Length > 0)
                {
                    _ScaleLine.Stroke = new SolidColorBrush(Color.FromArgb(150, 0, 100, 255));
                    _ScaleText.Foreground = new SolidColorBrush(Color.FromArgb(200, 0, 100, 255));
                }
                else
                {
                    _ScaleText.Foreground = Brushes.Blue;
                    _ScaleLine.Stroke = Brushes.Blue;
                }
                _ScaleLine.StrokeThickness = 8.0 / Zoom;
                _ScaleText.FontSize = 12 / Zoom;
            }
            catch(System.Exception ex)
            {
            }
        }


        public Point ScaleLinePosition
        {
            get
            {
                return _ScaleLinePosition;
            }
            set
            {
                _ScaleLinePosition = value;
                double Shift = 0;// 10 / Zoomfactor;
                Canvas.SetLeft(_ScaleLine, _ScaleLinePosition.X - Shift);
                Canvas.SetTop(_ScaleLine, _ScaleLinePosition.Y - Shift);
                //if (_ScaleText.Text.Length > 0)// && _Canvas.Children.Contains(_ScaleText))
                {
                    double TextTop = _ScaleLinePosition.Y + 4 + _ScaleLine.StrokeThickness/2;
                    double TextLeft = _ScaleLinePosition.X + ((_ScaleLine.Width /*/ Zoomfactor*/) /**/ / 2) - (_ScaleText.ActualWidth / 2);

                    Canvas.SetTop(_ScaleText, TextTop);
                    Canvas.SetLeft(_ScaleText, TextLeft);

                    //Canvas.SetLeft(_ScaleText, _ScaleLinePosition.X + 4 + _ScaleLine.Width);
                    //Canvas.SetTop(_ScaleText, -4);
                }
            } 
        }


        public Point ScaleLinePosition2
        {
            get
            {
                return new Point(_ScaleLinePosition.X + _ScaleLine.ActualWidth, _ScaleLinePosition.Y + _ScaleLine.ActualHeight);
            }
        }


        #endregion

        #region Points

        public void ClearPointcollectionGeometry()
        {
            if (this._Points.Count > 0)
            {
                foreach(System.Collections.Generic.KeyValuePair<Point, System.Windows.Controls.TextBlock> keyValuePair in this._Points)
                {
                    if (_Canvas.Children.Contains(keyValuePair.Value))
                        _Canvas.Children.Remove(keyValuePair.Value);
                }
            }
            //if (this._Points.Count > 0)
            //{
            //    //    foreach(Point p in this._Points)
            //    //    {
            //    //        //if (_Canvas.Children.Contains(p))
            //    //        //    _Canvas.Children.Remove(_Points);

            //    //    }

            //}
        }

        public PointCollection GetPoints()
        {
            PointCollection points = new PointCollection();
            return points;
        }

        public void ShowPoints(PointCollection points)
        {
            if (this._Points.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<Point, System.Windows.Controls.TextBlock> keyValuePair in this._Points)
                {
                    if (!_Canvas.Children.Contains(keyValuePair.Value))
                        _Canvas.Children.Add(keyValuePair.Value);
                }
            }
            //if (!_Canvas.Children.Contains(_Rectangle))
            //{
            //    Canvas.SetLeft(_Rectangle, X);
            //    Canvas.SetTop(_Rectangle, Y);
            //    _RectanglePosition = new Point(X, Y);
            //    _Canvas.Children.Add(_Rectangle);
            //}
        }



        #endregion

    }
}
