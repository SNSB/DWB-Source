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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfSamplingPlotPage
{
    /// <summary>
    /// Sample class for Points.
    /// </summary>
    class SamplePoints : Sample
    {
        #region Constants

        private const double PinSize = 4.0;
        private const double CrossSize = 5.0;
        private const double XSize = 4.0;
        private const double CircleSize = 4.5;
        private const double CenterpointSize = 0.5;
        private const double SquareSize = 4.0;
        private const double SquareSmallSize = 3.6;
        private const double DiamondSize = 3.8;
        private const double PyramidSize = 5.0;
        private const double ConeSize = 5.0;
        internal const int MinPoints = 1;
        private const double PI = Math.PI;

        #endregion // Constants

        #region Fields

        /// <summary>
        /// The shapes object.
        /// </summary>
        public Path m_Path;

        /// <summary>
        /// The shapes geometry object.
        /// </summary>
        internal PathGeometry m_PathGeometry;

        internal List<Image> m_ImageList = new List<Image>();

        // private RadialGradientBrush m_FillRad = null;

        /// <summary>
        /// The parent control where the polyline is attached to.
        /// </summary>
        private Canvas m_Parent;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets the stroke brush.
        /// </summary>
        /// <value>The stroke brush.</value>
        public SolidColorBrush StrokeBrush
        {
            get { return m_StrokeBrush; }
            set
            {
                m_StrokeBrush = value;
                m_Path.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B));
            }
        }

        /// <summary>
        /// Gets or sets the fill brush.
        /// </summary>
        /// <value>The fill brush.</value>
        public SolidColorBrush FillBrush
        {
            get { return m_FillBrush; }
            set
            {
                m_FillBrush = value;
                m_Path.Fill = new SolidColorBrush(Color.FromArgb(m_FillTransparency, m_FillBrush.Color.R, m_FillBrush.Color.G, m_FillBrush.Color.B));
            }
        }

        /// <summary>
        /// Gets or sets the stroke transparency.
        /// </summary>
        /// <value>The stroke transparency.</value>
        public byte StrokeTransparency
        {
            get { return m_StrokeTransparency; }
            set { m_Path.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency = value, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B)); }
        }

        /// <summary>
        /// Gets or sets the fill transparency.
        /// </summary>
        /// <value>The fill transparency.</value>
        public byte FillTransparency
        {
            get { return m_FillTransparency; }
            set 
            { 
                m_Path.Fill = new SolidColorBrush(Color.FromArgb(m_FillTransparency = value, m_FillBrush.Color.R, m_FillBrush.Color.G, m_FillBrush.Color.B));
                foreach (Image img in m_ImageList)
                    img.Opacity = (double)m_FillTransparency/(double)255;
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness
        {
            get { return m_StrokeThickness; }
            set { m_Path.StrokeThickness = m_StrokeThickness = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating the visibility of the object marker.
        /// </summary>
        /// <value><c>true</c> if the sample is visible; otherwise, <c>false</c>.</value>
        public bool SampleVisibility
        {
            get { return m_SampleVisibility; }
            set
            {
                m_SampleVisibility = value;
                m_Path.Opacity = (m_SampleVisibility ? 1.0 : ((double)m_SampleOffTransparency / 255.0));
                foreach (Image img in m_ImageList)
                    img.Opacity = (m_SampleVisibility ? 1.0 : ((double)m_SampleOffTransparency / 255.0));
            }
        }

        #endregion // Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplePoints"/> class.
        /// Creates the shape and shape geometry instances and adds the shape to the given canvas.
        /// </summary>
        /// <param name="canvas">The parent canvas of the sample's shape.</param>
        public SamplePoints(Canvas canvas)
        {
            /* Prepare radial gradient brush
            RadialGradientBrush radialGradient = new RadialGradientBrush();
            // Set the GradientOrigin to the center of the area being painted.
            radialGradient.GradientOrigin = new Point(0.1, 0.2);
            // Set the gradient center to the center of the area being painted.
            radialGradient.Center = new Point(0.5, 0.5);
            // Set the radius of the gradient circle so that it extends to
            // the edges of the area being painted.
            radialGradient.RadiusX = 0.5;
            radialGradient.RadiusY = 0.5;
            radialGradient.GradientStops.Add(new GradientStop(Colors.White, 0.0));
            radialGradient.GradientStops.Add(new GradientStop(m_Stroke.Color, 1.0));
            m_FillRad = radialGradient;
            */

            // Create shapes object
            m_Path = new Path();
            // Create shapes geometry object
            m_PathGeometry = new PathGeometry();
            // Set path settings
            // Init polygon 
            m_Path.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B));
            m_Path.Fill = new SolidColorBrush(Color.FromArgb(m_FillTransparency, m_FillBrush.Color.R, m_FillBrush.Color.G, m_FillBrush.Color.B));
            m_Path.StrokeThickness = m_StrokeThickness = 1.0;
            // Add path geometry
            m_Path.Data = m_PathGeometry;
            // Set sample type
            m_SampleType = SampleType.POINT;

            // Add to parent control
            m_Parent = canvas;
            m_Parent.Children.Add(m_Path);
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Sets all sample points of the shape from the given point collection.
        /// </summary>
        /// <param name="points">The input point collection.</param>
        public void SetSamplePoint(PointCollection points)
        {
            // Create complete set
            foreach (Point point in points)
                SetSamplePoint(point);
        }

        /// <summary>
        /// Adds a sample point for the shape.
        /// </summary>
        /// <param name="point">The input point.</param>
        public void SetSamplePoint(Point point)
        {
            // Add point to point collection
            SetPoint(point);

            // Create new object geometry
            AddGeometry(point);

        }

        private Image GetImageFromFile(string fileName)
        {
            // Create Icon for button
            Uri uri = new Uri(fileName, UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(uri);
            Image image = new Image();
            image.Source = bitmap;
            image.Stretch = Stretch.Uniform;
            return image;
        }


        /// <summary>
        /// Inserts a sample point at the given position of the sample point collection.
        /// Because of the different handling (using Path and Geometry) compared to Polygons and Lines
        /// we cannot easily change a point position, when it is moved. We have to remove the point
        /// from the old place and add it again at the new position.
        /// </summary>
        /// <param name="index">Index of the point in the collection.</param>
        /// <param name="point">The input point.</param>
        public void SetSamplePoint(int index, Point point)
        {
            // Add point to point collection
            SetPoint(index, point);

            // Create new object geometry
            AddGeometry(point);
        }

        /// <summary>
        /// Creates a geometry at the given position according to the point symbol and adds it
        /// to the geometry collection of the object path (which defines the shape).
        /// </summary>
        /// <param name="point">The input point position.</param>
        private void AddGeometry(Point point)
        {
            switch (m_SamplePointSymbol)
            {
                case PointSymbol.Pin:
                    Point pointPin1 = new Point(point.X + 2.0 * PinSize * m_SamplePointSymbolSize, point.Y - 4 * PinSize * m_SamplePointSymbolSize);
                    Point pointPin2 = new Point(point.X + 1.5 * PinSize * m_SamplePointSymbolSize, point.Y - 3.15 * PinSize * m_SamplePointSymbolSize);
                    Point pointPin3 = new Point(point.X + 1.75 * PinSize * m_SamplePointSymbolSize, point.Y - 3 * PinSize * m_SamplePointSymbolSize);
                    // Create new object geometry
                    EllipseGeometry ellipseGeometryPin = new EllipseGeometry(pointPin1, PinSize * m_SamplePointSymbolSize, PinSize * m_SamplePointSymbolSize);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(ellipseGeometryPin);
                    // Create new object geometry
                    LineGeometry lineGeometryPin1 = new LineGeometry(point, pointPin2);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryPin1);
                    // Create new object geometry
                    LineGeometry lineGeometryPin2 = new LineGeometry(point, pointPin3);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryPin2);
                    break;

                case PointSymbol.Cross:
                    Point pointCross1 = new Point(point.X - CrossSize * m_SamplePointSymbolSize, point.Y);
                    Point pointCross2 = new Point(point.X + CrossSize * m_SamplePointSymbolSize, point.Y);
                    Point pointCross3 = new Point(point.X, point.Y - CrossSize * m_SamplePointSymbolSize);
                    Point pointCross4 = new Point(point.X, point.Y + CrossSize * m_SamplePointSymbolSize);
                    // Create new object geometry
                    LineGeometry lineGeometryCross1 = new LineGeometry(pointCross1, pointCross2);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryCross1);
                    // Create new object geometry
                    LineGeometry lineGeometryCross2 = new LineGeometry(pointCross3, pointCross4);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryCross2);
                    break;

                case PointSymbol.X:
                    Point pointX1 = new Point(point.X - XSize * m_SamplePointSymbolSize, point.Y - XSize * m_SamplePointSymbolSize);
                    Point pointX2 = new Point(point.X + XSize * m_SamplePointSymbolSize, point.Y + XSize * m_SamplePointSymbolSize);
                    Point pointX3 = new Point(point.X - XSize * m_SamplePointSymbolSize, point.Y + XSize * m_SamplePointSymbolSize);
                    Point pointX4 = new Point(point.X + XSize * m_SamplePointSymbolSize, point.Y - XSize * m_SamplePointSymbolSize);
                    // Create new object geometry
                    LineGeometry lineGeometryX1 = new LineGeometry(pointX1, pointX2);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryX1);
                    // Create new object geometry
                    LineGeometry lineGeometryX2 = new LineGeometry(pointX3, pointX4);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryX2);
                    break;

                case PointSymbol.Circle:
                    // Create new object geometry
                    EllipseGeometry ellipseGeometryCircle = new EllipseGeometry(point, CircleSize * m_SamplePointSymbolSize, CircleSize * m_SamplePointSymbolSize);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(ellipseGeometryCircle);
                    break;

                case PointSymbol.Square:
                    Point pointSquare1 = new Point(point.X - SquareSize * m_SamplePointSymbolSize, point.Y - SquareSize * m_SamplePointSymbolSize);
                    Point pointSquare2 = new Point(point.X + SquareSize * m_SamplePointSymbolSize, point.Y + SquareSize * m_SamplePointSymbolSize);
                    Rect rectSquare = new Rect(pointSquare1, pointSquare2);
                    // Create new object geometry
                    RectangleGeometry rectGeometrySquare = new RectangleGeometry(rectSquare);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(rectGeometrySquare);
                    break;

                case PointSymbol.SquareSmall:
                    Point pointSquareSmall1 = new Point(point.X - SquareSmallSize * m_SamplePointSymbolSize, point.Y - SquareSmallSize * m_SamplePointSymbolSize);
                    Point pointSquareSmall2 = new Point(point.X + SquareSmallSize * m_SamplePointSymbolSize, point.Y + SquareSmallSize * m_SamplePointSymbolSize);
                    Rect rectSquareSmall = new Rect(pointSquareSmall1, pointSquareSmall2);
                    // Create new object geometry
                    RectangleGeometry rectGeometrySquareSmall = new RectangleGeometry(rectSquareSmall);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(rectGeometrySquareSmall);
                    break;

                case PointSymbol.Diamond:
                    Point pointDiamond1 = new Point(point.X - DiamondSize * m_SamplePointSymbolSize, point.Y - DiamondSize * m_SamplePointSymbolSize);
                    Point pointDiamond2 = new Point(point.X + DiamondSize * m_SamplePointSymbolSize, point.Y + DiamondSize * m_SamplePointSymbolSize);
                    Rect rectDiamond = new Rect(pointDiamond1, pointDiamond2);
                    RotateTransform rotateTransform = new RotateTransform(45, point.X, point.Y);
                    // Create new object geometry
                    RectangleGeometry rectGeometryDiamond = new RectangleGeometry(rectDiamond, 0, 0, rotateTransform);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(rectGeometryDiamond);
                    break;
/*
                case PointSymbol.Pyramid:                    
                    double heightPyramid = Math.Sqrt(PyramidSize * PyramidSize - (PyramidSize / 2) * PyramidSize / 2);
                    Point pointPyramid1a = new Point(point.X - (PyramidSize * 0.667) * m_SamplePointSymbolSize, point.Y + heightPyramid * 0.667 * m_SamplePointSymbolSize);
                    Point pointPyramid1b = new Point(point.X + (PyramidSize * 0.333) * m_SamplePointSymbolSize, point.Y - heightPyramid * 0.333 * m_SamplePointSymbolSize);
                    Rect rectPyramid1 = new Rect(pointPyramid1a, pointPyramid1b);
                    SkewTransform skewPyramid1Transform = new SkewTransform(-30, 0, point.X, point.Y);
                    RectangleGeometry rectGeometryPyramid1 = new RectangleGeometry(rectPyramid1, 0, 0, skewPyramid1Transform);
                    m_PathGeometry.AddGeometry(rectGeometryPyramid1);

                    Point pointPyramid2a = new Point(point.X - (PyramidSize * 0.333) * m_SamplePointSymbolSize, point.Y + heightPyramid * 0.667 * m_SamplePointSymbolSize);
                    Point pointPyramid2b = new Point(point.X + (PyramidSize * 0.667) * m_SamplePointSymbolSize, point.Y - heightPyramid * 0.333 * m_SamplePointSymbolSize);
                    Rect rectPyramid2 = new Rect(pointPyramid2a, pointPyramid2b);
                    SkewTransform skewPyramid2Transform = new SkewTransform(30, 0, point.X, point.Y);
                    RectangleGeometry rectGeometryPyramid2 = new RectangleGeometry(rectPyramid2, 0, 0, skewPyramid2Transform);
                    m_PathGeometry.AddGeometry(rectGeometryPyramid2);

                    Point pointPyramid3a = new Point(point.X - (PyramidSize * 0.667) * m_SamplePointSymbolSize, point.Y + heightPyramid * 0.667 * m_SamplePointSymbolSize);
                    Point pointPyramid3b = new Point(point.X + (PyramidSize * 0.333) * m_SamplePointSymbolSize, point.Y - heightPyramid * 0.333 * m_SamplePointSymbolSize);
                    Rect rectPyramid3 = new Rect(pointPyramid3a, pointPyramid3b);
                    TransformGroup transformPyramid = new TransformGroup();
                    SkewTransform skewPyramid3Transform = new SkewTransform(-30, 0, point.X, point.Y);
                    RotateTransform rotatePyramid3Transform = new RotateTransform(120, point.X, point.Y);
                    transformPyramid.Children.Add(skewPyramid3Transform);
                    transformPyramid.Children.Add(rotatePyramid3Transform);
                    RectangleGeometry rectGeometryPyramid3 = new RectangleGeometry(rectPyramid3, 0, 0, transformPyramid);
                    m_PathGeometry.AddGeometry(rectGeometryPyramid3);   
                    break;

                case PointSymbol.Cone:                    
                    double heightCone = Math.Sqrt(ConeSize * ConeSize - (ConeSize / 2) * ConeSize / 2);

                    Point pointCone1a = new Point(point.X - (ConeSize * 0.667) * m_SamplePointSymbolSize, point.Y + heightCone * 0.667 * m_SamplePointSymbolSize);
                    Point pointCone1b = new Point(point.X + (ConeSize * 0.333) * m_SamplePointSymbolSize, point.Y - heightCone * 0.333 * m_SamplePointSymbolSize);
                    Rect rectCone1 = new Rect(pointCone1a, pointCone1b);
                    TransformGroup transform1 = new TransformGroup();
                    SkewTransform skewCone1Transform = new SkewTransform(-30, 0, point.X, point.Y);
                    RotateTransform rotateCone1Transform = new RotateTransform(180, point.X, point.Y);
                    transform1.Children.Add(skewCone1Transform);
                    transform1.Children.Add(rotateCone1Transform);
                    RectangleGeometry rectGeometryCone1 = new RectangleGeometry(rectCone1, 0, 0, transform1);
                    m_PathGeometry.AddGeometry(rectGeometryCone1);

                    Point pointCone2a = new Point(point.X - (ConeSize * 0.333) * m_SamplePointSymbolSize, point.Y + heightCone * 0.667 * m_SamplePointSymbolSize);
                    Point pointCone2b = new Point(point.X + (ConeSize * 0.667) * m_SamplePointSymbolSize, point.Y - heightCone * 0.333 * m_SamplePointSymbolSize);
                    Rect rectCone2 = new Rect(pointCone2a, pointCone2b);
                    TransformGroup transform2 = new TransformGroup();
                    SkewTransform skewCone2Transform = new SkewTransform(30, 0, point.X, point.Y);
                    RotateTransform rotateCone2Transform = new RotateTransform(180, point.X, point.Y);
                    transform2.Children.Add(skewCone2Transform);
                    transform2.Children.Add(rotateCone2Transform);
                    RectangleGeometry rectGeometryCone2 = new RectangleGeometry(rectCone2, 0, 0, transform2);
                    m_PathGeometry.AddGeometry(rectGeometryCone2);

                    Point pointCone3a = new Point(point.X - (ConeSize * 0.667) * m_SamplePointSymbolSize, point.Y + heightCone * 0.667 * m_SamplePointSymbolSize);
                    Point pointCone3b = new Point(point.X + (ConeSize * 0.333) * m_SamplePointSymbolSize, point.Y - heightCone * 0.333 * m_SamplePointSymbolSize);
                    Rect rectCone3 = new Rect(pointCone3a, pointCone3b);
                    TransformGroup transformCone = new TransformGroup();
                    SkewTransform skewCone3Transform = new SkewTransform(-30, 0, point.X, point.Y);
                    RotateTransform rotateCone3Transform = new RotateTransform(-60, point.X, point.Y);
                    transformCone.Children.Add(skewCone3Transform);
                    transformCone.Children.Add(rotateCone3Transform);
                    RectangleGeometry rectGeometryCone3 = new RectangleGeometry(rectCone3, 0, 0, transformCone);
                    m_PathGeometry.AddGeometry(rectGeometryCone3);
                    break;
*/
                case PointSymbol.Pyramid:
                    /*
                    // Centerpoint = geometric center
                    double heightPyramid = Math.Sqrt(PyramidSize * PyramidSize - (PyramidSize / 2) * PyramidSize / 2);
                    Point pointPyramid1 = new Point(point.X - (PyramidSize) * m_SamplePointSymbolSize, point.Y + heightPyramid * 0.667 * m_SamplePointSymbolSize);
                    Point pointPyramid2 = new Point(point.X + (PyramidSize) * m_SamplePointSymbolSize, point.Y + heightPyramid * 0.667 * m_SamplePointSymbolSize);
                    Point pointPyramid3 = new Point(point.X, point.Y + heightPyramid * 0.667 * m_SamplePointSymbolSize - heightPyramid * 2 * m_SamplePointSymbolSize);
                     */
                    // Centerpoint = height / 2
                    double heightPyramid = Math.Sqrt(PyramidSize * PyramidSize - (PyramidSize / 2) * PyramidSize / 2);
                    Point pointPyramid1 = new Point(point.X, point.Y + heightPyramid * m_SamplePointSymbolSize);
                    Point pointPyramid2 = new Point(point.X + (PyramidSize) * m_SamplePointSymbolSize, point.Y + heightPyramid * m_SamplePointSymbolSize);
                    Point pointPyramid3 = new Point(point.X, point.Y + heightPyramid * m_SamplePointSymbolSize - heightPyramid * 2 * m_SamplePointSymbolSize);
                    Point pointPyramid4 = new Point(point.X - (PyramidSize) * m_SamplePointSymbolSize, point.Y + heightPyramid * m_SamplePointSymbolSize);
                    PathFigure pyramidFigure = new PathFigure();
                    pyramidFigure.StartPoint = pointPyramid1;
                    LineSegment pyramidSegment1 = new LineSegment(pointPyramid2, true);
                    LineSegment pyramidSegment2 = new LineSegment(pointPyramid3, true);
                    LineSegment pyramidSegment3 = new LineSegment(pointPyramid4, true);
                    LineSegment pyramidSegment4 = new LineSegment(pointPyramid1, true);
                    pyramidFigure.Segments.Add(pyramidSegment1);
                    pyramidFigure.Segments.Add(pyramidSegment2);
                    pyramidFigure.Segments.Add(pyramidSegment3);
                    pyramidFigure.Segments.Add(pyramidSegment4);
                    m_PathGeometry.Figures.Add(pyramidFigure);
                
                    //LineGeometry linePyramid1 = new LineGeometry(pointPyramid1, pointPyramid2);
                    // Add geometry to objects path geometry collection
                    //m_PathGeometry.AddGeometry(linePyramid1);
                    //LineGeometry linePyramid2 = new LineGeometry(pointPyramid2, pointPyramid3);
                    // Add geometry to objects path geometry collection
                    //m_PathGeometry.AddGeometry(linePyramid2);
                    //LineGeometry linePyramid3 = new LineGeometry(pointPyramid3, pointPyramid1);
                    // Add geometry to objects path geometry collection
                    //m_PathGeometry.AddGeometry(linePyramid3);

                    break;

                case PointSymbol.Cone:
                    /*
                    // Centerpoint = geometric center
                    double heightCone = Math.Sqrt(PyramidSize * PyramidSize - (PyramidSize / 2) * PyramidSize / 2);
                    Point pointCone1 = new Point(point.X - (PyramidSize) * m_SamplePointSymbolSize, point.Y + heightCone * 0.333 * m_SamplePointSymbolSize - heightCone * 2 * m_SamplePointSymbolSize);
                    Point pointCone2 = new Point(point.X + (PyramidSize) * m_SamplePointSymbolSize, point.Y + heightCone * 0.333 * m_SamplePointSymbolSize - heightCone * 2 * m_SamplePointSymbolSize);
                    Point pointCone3 = new Point(point.X, point.Y + heightCone * 0.333 * m_SamplePointSymbolSize);
                     */
                    // Centerpoint = height / 2
                    double heightCone = Math.Sqrt(PyramidSize * PyramidSize - (PyramidSize / 2) * PyramidSize / 2);
                    Point pointCone1 = new Point(point.X, point.Y + heightCone * m_SamplePointSymbolSize - heightCone * 2 * m_SamplePointSymbolSize);
                    Point pointCone2 = new Point(point.X + (PyramidSize) * m_SamplePointSymbolSize, point.Y + heightCone * m_SamplePointSymbolSize - heightCone * 2 * m_SamplePointSymbolSize);
                    Point pointCone3 = new Point(point.X, point.Y + heightCone * m_SamplePointSymbolSize);
                    Point pointCone4 = new Point(point.X - (PyramidSize) * m_SamplePointSymbolSize, point.Y + heightCone * m_SamplePointSymbolSize - heightCone * 2 * m_SamplePointSymbolSize);
                    PathFigure coneFigure = new PathFigure();
                    coneFigure.StartPoint = pointCone1;
                    LineSegment coneSegment1 = new LineSegment(pointCone2, true);
                    LineSegment coneSegment2 = new LineSegment(pointCone3, true);
                    LineSegment coneSegment3 = new LineSegment(pointCone4, true);
                    LineSegment coneSegment4 = new LineSegment(pointCone1, true);
                    coneFigure.Segments.Add(coneSegment1);
                    coneFigure.Segments.Add(coneSegment2);
                    coneFigure.Segments.Add(coneSegment3);
                    coneFigure.Segments.Add(coneSegment4);
                    m_PathGeometry.Figures.Add(coneFigure);
                    
                    //LineGeometry lineCone1 = new LineGeometry(pointCone1, pointCone2);
                    // Add geometry to objects path geometry collection
                    //m_PathGeometry.AddGeometry(lineCone1);
                    //LineGeometry lineCone2 = new LineGeometry(pointCone2, pointCone3);
                    // Add geometry to objects path geometry collection
                    //m_PathGeometry.AddGeometry(lineCone2);
                    //LineGeometry lineCone3 = new LineGeometry(pointCone3, pointCone1);
                    // Add geometry to objects path geometry collection
                    //m_PathGeometry.AddGeometry(lineCone3);
                    break;

                case PointSymbol.Minus:
                    Point pointMinus1 = new Point(point.X - CrossSize * m_SamplePointSymbolSize, point.Y);
                    Point pointMinus2 = new Point(point.X + CrossSize * m_SamplePointSymbolSize, point.Y);
                    // Create new object geometry
                    LineGeometry lineGeometryMinus = new LineGeometry(pointMinus1, pointMinus2);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryMinus);
                    break;

                case PointSymbol.Questionmark:
                    PathFigure figure = new PathFigure();
                    figure.StartPoint = new Point(point.X - 2.5 * m_SamplePointSymbolSize, point.Y - 3 * m_SamplePointSymbolSize);
                    // Oberer Halbkreis
                    figure.Segments.Add(
                        new ArcSegment(new Point(point.X + 2.5 * m_SamplePointSymbolSize, point.Y - 2.5 * m_SamplePointSymbolSize), new Size(2.5 * m_SamplePointSymbolSize, 2.5 * m_SamplePointSymbolSize), 0, false, SweepDirection.Clockwise, true));
                    // Rechter 8-tel-Kreis
                    figure.Segments.Add(
                        new ArcSegment(new Point(point.X + 1.25 * m_SamplePointSymbolSize, point.Y - 0.5 * m_SamplePointSymbolSize), new Size(2.5 * m_SamplePointSymbolSize, 2.5 * m_SamplePointSymbolSize), 0, false, SweepDirection.Clockwise, true));
                    // Rechter Gegen-8-tel-Kreis
                    figure.Segments.Add(
                        new ArcSegment(new Point(point.X + 0 * m_SamplePointSymbolSize, point.Y + 1 * m_SamplePointSymbolSize), new Size(2.5 * m_SamplePointSymbolSize, 1.5 * m_SamplePointSymbolSize), 0, false, SweepDirection.Counterclockwise, true));
                    // Unterer Abschluss
                    figure.Segments.Add(
                        new ArcSegment(new Point(point.X - 0.5 * m_SamplePointSymbolSize, point.Y + 1 * m_SamplePointSymbolSize), new Size(0.25 * m_SamplePointSymbolSize, 0.25 * m_SamplePointSymbolSize), 0, false, SweepDirection.Clockwise, true));
                    // Rechter Gegen-8-tel-Kreis
                    figure.Segments.Add(
                        new ArcSegment(new Point(point.X + 0.75 * m_SamplePointSymbolSize, point.Y - 0.7 * m_SamplePointSymbolSize), new Size(2.5 * m_SamplePointSymbolSize, 1.5 * m_SamplePointSymbolSize), 0, false, SweepDirection.Clockwise, true));
                    // Rechter 8-tel-Kreis
                    figure.Segments.Add(
                        new ArcSegment(new Point(point.X + 2 * m_SamplePointSymbolSize, point.Y - 3 * m_SamplePointSymbolSize), new Size(2.5 * m_SamplePointSymbolSize, 2.5 * m_SamplePointSymbolSize), 0, false, SweepDirection.Counterclockwise, true));
                    // Oberer Halbkreis
                    figure.Segments.Add(
                        new ArcSegment(new Point(point.X - 2 * m_SamplePointSymbolSize, point.Y - 3 * m_SamplePointSymbolSize), new Size(2 * m_SamplePointSymbolSize, 2 * m_SamplePointSymbolSize), 0, false, SweepDirection.Counterclockwise, true));
                    // Oberer Abschluss
                    figure.Segments.Add(
                        new ArcSegment(new Point(point.X - 2.5 * m_SamplePointSymbolSize, point.Y - 3 * m_SamplePointSymbolSize), new Size(0.5 * m_SamplePointSymbolSize, 0.5 * m_SamplePointSymbolSize), 0, false, SweepDirection.Clockwise, true));
                    m_PathGeometry.Figures.Add(figure);
                    EllipseGeometry ellipseGeometryCircleX = new EllipseGeometry(new Point(point.X - 0.25 * m_SamplePointSymbolSize, point.Y + 3 * m_SamplePointSymbolSize), 0.5 * m_SamplePointSymbolSize, 0.5 * m_SamplePointSymbolSize);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(ellipseGeometryCircleX);
                    break;

                case PointSymbol.Circlepoint:
                    // Create new object geometry
                    EllipseGeometry ellipseGeometryCirclepoint = new EllipseGeometry(point, CircleSize * m_SamplePointSymbolSize, CircleSize * m_SamplePointSymbolSize);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(ellipseGeometryCirclepoint);
                    EllipseGeometry ellipseGeometryCenterpoint = new EllipseGeometry(point, CenterpointSize * m_SamplePointSymbolSize, CenterpointSize * m_SamplePointSymbolSize);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(ellipseGeometryCenterpoint);
                    break;


                default:
                    Image img = GetImageFromFile(m_SamplePointSymbol.ToString().Replace("_Icon", ".ico"));
                    img.Stretch = Stretch.Uniform;
                    img.Width = m_SamplePointSymbolSize * 10;                    
                    img.Height = m_SamplePointSymbolSize * 10;
                    img.ToolTip = m_Path.ToolTip;
                    m_Parent.Children.Add(img);
                    Canvas.SetLeft(img, point.X - img.Width / 2);
                    Canvas.SetTop(img, point.Y - img.Height / 2);
                    m_ImageList.Add(img);

                    /* Set Pin with icon as top
                    Point pointPin1i = new Point(point.X + 2.0 * PinSize * m_SamplePointSymbolSize, point.Y - 4 * PinSize * m_SamplePointSymbolSize);
                    Point pointPin2i = new Point(point.X + 1.5 * PinSize * m_SamplePointSymbolSize, point.Y - 3.15 * PinSize * m_SamplePointSymbolSize);
                    Point pointPin3i = new Point(point.X + 1.75 * PinSize * m_SamplePointSymbolSize, point.Y - 3 * PinSize * m_SamplePointSymbolSize);
                    // Create new object geometry
                    EllipseGeometry ellipseGeometryPini = new EllipseGeometry(pointPin1i, PinSize * m_SamplePointSymbolSize, PinSize * m_SamplePointSymbolSize);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(ellipseGeometryPini);
                    // Create new object geometry
                    LineGeometry lineGeometryPin1i = new LineGeometry(point, pointPin2i);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryPin1i);
                    // Create new object geometry
                    LineGeometry lineGeometryPin2i = new LineGeometry(point, pointPin3i);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(lineGeometryPin2i);

                    Canvas.SetLeft(img, pointPin1i.X - img.Width / 2);
                    Canvas.SetTop(img, pointPin1i.Y - img.Height / 2);
                    */

                    // Set Point Marker in image icon
                    // Create new object geometry
                    EllipseGeometry ellipseGeometryMarker = new EllipseGeometry(point, 0, 0);
                    // Add geometry to objects path geometry collection
                    m_PathGeometry.AddGeometry(ellipseGeometryMarker);

                    m_Parent.Children.Remove(m_Path);
                    m_Parent.Children.Add(m_Path);
                    //
                    break;
            }
        }
        
        /// <summary>
        /// Clears the point symbol and icons.
        /// </summary>
        private void ClearGeometries()
        {
            // Clear path geometries
            m_PathGeometry.Clear();
            foreach (Image img in m_ImageList)
                m_Parent.Children.Remove(img);
        }

        /// <summary>
        /// Redraws the sample points (with new shape) by clearing the geometries and adding them again for each point.
        /// </summary>
        public void RedrawSamplePoints()
        {
            ToggleButton button = null;
            if (m_ImageList.Count > 0)
                button = m_ImageList[0].DataContext as ToggleButton;
            else
                button = m_Path.DataContext as ToggleButton;
            // Clear path geometries
            ClearGeometries();
            // Create them again
            foreach (Point point in m_SamplePointCollection)
            {
                AddGeometry(point);
            }
            if (m_ImageList.Count > 0)
            {
                foreach (Image image in m_ImageList)
                {
                    image.DataContext = button;
                }
            }
            else
                m_Path.DataContext = button;

        }

        /// <summary>
        /// Removes the last sample point of the point collection.
        /// </summary>
        public void ClearSamplePoint()
        {
            // Remove last point of point collection
            ClearSamplePoint(m_SamplePointCollection.Count - 1);
        }

        /// <summary>
        /// Removes a specific sample point of the point collection.
        /// Clears the complete path geometry and restores the geometries for the remaining shapes.
        /// </summary>
        /// <param name="index">Index of the point to remove.</param>
        public void ClearSamplePoint(int index)
        {
            if (index < 0)
                return;

            // Remove last point of point collection
            ClearPoint(index);

            // Clear path geometry
            ClearGeometries();
            // Set up new Geometries without last one
            foreach (Point point in m_SamplePointCollection)
            {
                AddGeometry(point);
            }
        }

        /// <summary>
        /// Clears the point collection and the entire path geometry.
        /// </summary>
        public void ClearSample()
        {
            // Clear point collection
            ClearPoints();

            // Clear path geometry
            ClearGeometries();
        }

        /// <summary>
        /// Adds the point collection to the sample's list (one and only element there).
        /// Switches sample type to MULTIPOINT, if more than one point is in the collection.
        /// </summary>
        public void AddSample()
        {
            // Add sample point collection to list
            AddPointsToList();
            // Set Multi Type, if applicable
            if (m_SamplePointCollection.Count > 1)
                m_SampleType = SampleType.MULTIPOINT;
        }

        /// <summary>
        /// Adds the ID and description to the sample and creates a tooltip for the path (shape).
        /// </summary>
        /// <param name="strID">The identifier text.</param>
        /// <param name="strText">The description text.</param>
        public void AddId(string strID, string strText)
        {
            m_Identifier = (strID == string.Empty ? m_SampleType.ToString() : strID);
            m_DisplayText = strText;
            ToolTip tt = SetToolTip(m_Identifier + WpfControl.StrCR + m_DisplayText);
            m_Path.ToolTip = tt;
            ToolTipService.SetShowDuration(m_Path, 300000);

            foreach (Image img in m_ImageList)
            {
                img.ToolTip = tt;
                ToolTipService.SetShowDuration(img, 300000);
            }

        }

        /// <summary>
        /// Animate the sample of the selected toggle button.
        /// </summary>
        /// <param name="animate">Change color if true, reset if false.</param>
        internal void Animate(bool animate)
        {
            byte transparency = 255;

            if (animate)
            {
                m_Path.Fill = new SolidColorBrush(Color.FromArgb(transparency, m_FillBrush.Color.B, m_FillBrush.Color.R, m_FillBrush.Color.G));
                m_Path.Stroke = new SolidColorBrush(Color.FromArgb(transparency, m_StrokeBrush.Color.B, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G));
            }
            else
            {
                m_Path.Fill = new SolidColorBrush(Color.FromArgb(m_FillTransparency, m_FillBrush.Color.R, m_FillBrush.Color.G, m_FillBrush.Color.B));
                m_Path.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B));
            }
        }


        #endregion // Methods
    }
}
