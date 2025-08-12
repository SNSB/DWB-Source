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
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfSamplingPlotPage
{
    /// <summary>
    /// Sample class for Image.
    /// </summary>
    class SampleImage : Sample
    {
        #region Fields

        /// <summary>
        /// The image object.
        /// </summary>
        public Image m_Image;

        /// <summary>
        /// The parent control where the image is attached to.
        /// </summary>
        private Canvas m_Parent;
        
        /// <summary>
        /// Creates a image transformation group.
        /// </summary>
        private TransformGroup m_Transform = new TransformGroup();
        /// <summary>
        /// Creates a scale transformation for the image.
        /// </summary>
        private ScaleTransform m_Scale = new ScaleTransform(1, 1);
        /// <summary>
        /// Creates a skew transformation for the image.
        /// </summary>
        private SkewTransform m_Skew = new SkewTransform(0, 0);
        /// <summary>
        /// Creates a rotate transformation for the image (-- not used so far --).
        /// </summary>
        private RotateTransform m_Rotate = new RotateTransform(0);

        /// <summary>
        /// The file path of the image on disk.
        /// </summary>
        private string m_ImageFilePath = string.Empty;

        // PickPoint collection
        /// <summary>
        /// Creates a point collection for the source pick points for image calibration.
        /// </summary>
        private PointCollection m_SourcePickPoints = new PointCollection();
        /// <summary>
        /// Creates a point collection for the target pick points for image calibration.
        /// </summary>
        private PointCollection m_TargetPickPoints = new PointCollection();

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating the visibility of the image.
        /// </summary>
        /// <value><c>true</c> if the sample is visible; otherwise, <c>false</c>.</value>
        public bool SampleVisibility
        {
            get { return m_SampleVisibility; }
            set
            {
                m_SampleVisibility = value;
                m_Image.Opacity = (m_SampleVisibility ? ((double)m_StrokeTransparency / 255.0) : 0.0);
            }
        }

        /// <summary>
        /// Gets or sets the transparency of the image.
        /// </summary>
        /// <value>The image transparency.</value>
        public byte Transparency
        {
            get { return m_StrokeTransparency; }
            set 
            { 
                m_FillTransparency = m_StrokeTransparency = value;
                m_Image.Opacity = (double)m_StrokeTransparency / 255.0;
            }
        }

        /// <summary>
        /// Gets or sets the image file path.
        /// </summary>
        /// <value>The image file path.</value>
        public string ImageFilePath
        {
            get { return m_ImageFilePath; }
            set { m_ImageFilePath = value; }
        }
        
        /// <summary>
        /// Gets or sets the scale transformation of the image.
        /// </summary>
        /// <value>The scale transformation values.</value>
        public ScaleTransform Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        /// <summary>
        /// Gets or sets the skew transformation of the image.
        /// </summary>
        /// <value>The skew transformation values.</value>
        public SkewTransform Skew
        {
            get { return m_Skew; }
            set { m_Skew = value; }
        }


        #endregion // Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleImage"/> class.
        /// Assigns the input image object and initializes the 4 corner points according to
        /// top left position and image source width and height. Assigns the scale and skew transformations
        /// to the image and the image itself to the parent canvas.
        /// </summary>
        /// <param name="canvas">The parent canvas of the image.</param>
        /// <param name="image">The input image object.</param>
        /// <param name="pointTopLeft">The top left corner position of the image within the working area.</param>
        public SampleImage(Canvas canvas, Image image, Point pointTopLeft)
        {
            int index = 0;

            // Set sample type
            m_SampleType = SampleType.IMAGE;
            // m_FillTransparency = m_StrokeTransparency;
            // m_StrokeThickness = 1.0;

            // Set Image element
            m_Image = image;
            // Set attributes 
            m_Image.Stretch = Stretch.None;

            // Set 1st point TopLeft
            SetSamplePoint(pointTopLeft);
            // Set 2nd point BottomRight
            SetSamplePoint(new Point(pointTopLeft.X + m_Image.Source.Width, pointTopLeft.Y + m_Image.Source.Height));
            // Set 3rd point BottomLeft
            SetSamplePoint(new Point(pointTopLeft.X, pointTopLeft.Y + m_Image.Source.Height));
            // Set 4th point TopRight
            SetSamplePoint(new Point(pointTopLeft.X + m_Image.Source.Width, pointTopLeft.Y));
            // Set image position within canvas
            SetPosition(pointTopLeft.X, pointTopLeft.Y);

            // Add transformations to group: First scale, then skew!
            m_Transform.Children.Add(m_Scale);
            m_Transform.Children.Add(m_Skew);
            // m_Transform.Children.Add(m_Rotate);

            // Transform:
            m_Image.RenderTransform = m_Transform;

            // Add to parent control
            m_Parent = canvas;
            // Insert picture right behind the last picture within the canvas children
            foreach (UIElement element in m_Parent.Children)
            {
                if ((element as Image) != null)
                    index++;
            }
            m_Parent.Children.Insert(index, m_Image);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleImage"/> class.
        /// Creates an image object and reads the bitmap from the given file path.
        /// Initializes the 4 corner points according to top left position and image source width and height. 
        /// Assigns the scale and skew transformations to the image and the image itself to the parent canvas.
        /// </summary>
        /// <param name="canvas">The parent canvas of the image.</param>
        /// <param name="path">The file path of the image source.</param>
        /// <param name="pointTopLeft">The top left corner position of the image within the working area.</param>
        public SampleImage(Canvas canvas, string path, Point pointTopLeft)
        {
            int index = 0;

            // Set sample type
            m_SampleType = SampleType.IMAGE;
            m_FillTransparency = m_StrokeTransparency;
            m_StrokeThickness = 1.0;
            m_ImageFilePath = path;

            // Create image source and bitmap image
            BitmapImage bi = new BitmapImage(new Uri(m_ImageFilePath, UriKind.RelativeOrAbsolute));
            // Create Image element
            m_Image = new Image();
            // Set attributes 
            m_Image.Source = bi;
            m_Image.Stretch = Stretch.None;

            // Set 1st point TopLeft
            SetSamplePoint(pointTopLeft);
            // Set 2nd point BottomRight
            SetSamplePoint(new Point(pointTopLeft.X + m_Image.Source.Width, pointTopLeft.Y + m_Image.Source.Height));
            // Set 3rd point BottomLeft
            SetSamplePoint(new Point(pointTopLeft.X, pointTopLeft.Y + m_Image.Source.Height));
            // Set 4th point TopRight
            SetSamplePoint(new Point(pointTopLeft.X + m_Image.Source.Width, pointTopLeft.Y));
            // Set image position within canvas
            SetPosition(pointTopLeft.X, pointTopLeft.Y);
            
            // Add transformations to group: First scale, then skew!
            m_Transform.Children.Add(m_Scale);
            m_Transform.Children.Add(m_Skew);
            // m_Transform.Children.Add(m_Rotate);
            // Transform:
            m_Image.RenderTransform = m_Transform;

            // Add to parent control
            m_Parent = canvas;
            // Insert picture right behind the last picture within the canvas children
            foreach (UIElement element in m_Parent.Children)
            {
                if ((element as Image) != null)
                    index++;
            }
            m_Parent.Children.Insert(index, m_Image);
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Adds a corner point for the image.
        /// The image is defined by 4 corner points in the following order: 
        /// Top left, bottom right, bottom left, top right.
        /// </summary>
        /// <param name="point">The input point.</param>
        public void SetSamplePoint(Point point)
        {
            // Add point to point collection
            SetPoint(point);
        }

        /// <summary>
        /// Sets the position of the image within the working area (canvas).
        /// The position is defined by the first corner point (top left).
        /// </summary>
        /// <param name="xPos">The x position.</param>
        /// <param name="yPos">The y position.</param>
        public void SetPosition(double xPos, double yPos)
        {
            // Set image position within canvas
            Canvas.SetLeft(m_Image, xPos);
            Canvas.SetTop(m_Image, yPos);
        }

        /// <summary>
        /// Changes the position of a corner point of the image (used in edit mode).
        /// When changing the top left corner point, the image will be shifted keeping its aspect ratio and dimensions.
        /// When changing the bottom right corner point (without having changed bottom left and top right yet),
        /// the image will be scaled keeping its aspect ratio.
        /// Changing bottom left or top right corner points will squeeze the image (by scale and skew transformation).
        /// The top left and bottom right corner points will remain at their positions, but the opposite corner of the 
        /// one which is currently moved will change accordingly (because only affine transformations are possible).
        /// Once having changed bottom left or top right, a further change of bottom right corner will also result
        /// in squeezing the image, keeping top left and bottom left corners at their places.
        /// </summary>
        /// <param name="point">The input point position.</param>
        /// <param name="index">The index of the point.</param>
        public void ChangePosition(Point point, int index)
        {
            // Check if point collection exists
            if (m_SamplePointCollection.Count != 4)
                return;

            Point newPoint = new Point();
            double angleX = 0;
            double angleY = 0;
            double diffX = point.X - m_SamplePointCollection[0].X;
            double diffY = point.Y - m_SamplePointCollection[0].Y;
            double factX = diffX / m_Image.Source.Width;
            double factY = diffY / m_Image.Source.Height;

            switch (index)
            {
                case 0:
                    // Move image points:
                    // Origin TopLeft point
                    ChangePoint(point, index);
                    // Adapt BottomRight point
                    newPoint = m_SamplePointCollection[1];
                    newPoint.X += diffX;
                    newPoint.Y += diffY;
                    ChangePoint(newPoint, 1);
                    // Adapt BottomLeft point
                    newPoint = m_SamplePointCollection[2];
                    newPoint.X += diffX;
                    newPoint.Y += diffY;
                    ChangePoint(newPoint, 2);
                    // Adapt TopRight point
                    newPoint = m_SamplePointCollection[3];
                    newPoint.X += diffX;
                    newPoint.Y += diffY;
                    ChangePoint(newPoint, 3);
                    // Set image position within canvas
                    Canvas.SetLeft(m_Image, point.X);
                    Canvas.SetTop(m_Image, point.Y);
                    break;

                case 1:
                    // Change BottomRight coordinate:
                    // Check if the image Skew angles are still 0: --> Scale both directions
                    if (m_Skew.AngleX == 0 && m_Skew.AngleY == 0)
                    {
                        // Set scale factors
                        m_Scale.ScaleX = factX;
                        m_Scale.ScaleY = factY;
                        // Set current point (BottomRight)
                        ChangePoint(point, index);
                        // Adapt point BottomLeft
                        newPoint = m_SamplePointCollection[2];
                        newPoint.Y = m_SamplePointCollection[0].Y + diffY;
                        ChangePoint(newPoint, 2);
                        // Adapt TopRight point
                    }
                    // If the image is already skewed, keep the origin and the bottom left coordinate in place
                    else
                    {
                        // Only ScaleX and SkewAngleY have to be changed!!
                        // ScaleX:
                        diffX = m_SamplePointCollection[2].X - point.X;
                        factX = diffX / -m_Image.Source.Width;
                        m_Scale.ScaleX = factX;
                        // AngleY:
                        diffY = m_SamplePointCollection[2].Y - point.Y;
                        angleY = GetSkewAngle(diffY, diffX);
                        m_Skew.AngleY = angleY;
                        // Set new sample point
                        ChangePoint(point, index);
                        // Adapt TopRight point
                    }
                    newPoint = m_SamplePointCollection[3];
                    newPoint.X = m_SamplePointCollection[0].X + (m_SamplePointCollection[1].X - m_SamplePointCollection[2].X);
                    newPoint.Y = m_SamplePointCollection[0].Y + (m_SamplePointCollection[1].Y - m_SamplePointCollection[2].Y);
                    ChangePoint(newPoint, 3);
                    break;

                case 2:
                    // Change BottomLeft coordinate - keep TopLeft and BottomRight coordinates in place:
                    // ScaleY:
                    diffY = point.Y - m_SamplePointCollection[0].Y;
                    factY = diffY / m_Image.Source.Height;
                    m_Scale.ScaleY = factY;
                    // AngleX:
                    diffX = point.X - m_SamplePointCollection[0].X;
                    angleX = GetSkewAngle(diffX, diffY);
                    m_Skew.AngleX = angleX;
                    // ScaleX:
                    diffX = point.X - m_SamplePointCollection[1].X;
                    factX = diffX / -m_Image.Source.Width;
                    m_Scale.ScaleX = factX;
                    // AngleY:
                    diffY = point.Y - m_SamplePointCollection[1].Y;
                    angleY = GetSkewAngle(diffY, diffX);
                    m_Skew.AngleY = angleY;
                    // Set new sample point
                    ChangePoint(point, index);
                    // Adapt TopRight point
                    newPoint = m_SamplePointCollection[3];
                    newPoint.X = m_SamplePointCollection[0].X + (m_SamplePointCollection[1].X - m_SamplePointCollection[2].X);
                    newPoint.Y = m_SamplePointCollection[0].Y + (m_SamplePointCollection[1].Y - m_SamplePointCollection[2].Y);
                    ChangePoint(newPoint, 3);
                    break;

                case 3:
                    // Change TopRight coordinate - keep TopLeft and BottomRight coordinates in place:
                    // Use mirror point (case 2) as the reference for changing Scale and Skew
                    Point mirror = MirrorPoint(point);
                    // ScaleY:
                    diffY = mirror.Y - m_SamplePointCollection[0].Y;
                    factY = diffY / m_Image.Source.Height;
                    m_Scale.ScaleY = factY;
                    // AngleX:
                    diffX = mirror.X - m_SamplePointCollection[0].X;
                    angleX = GetSkewAngle(diffX, diffY);
                    m_Skew.AngleX = angleX;
                    // ScaleX:
                    diffX = mirror.X - m_SamplePointCollection[1].X;
                    factX = diffX / -m_Image.Source.Width;
                    m_Scale.ScaleX = factX;
                    // AngleY:
                    diffY = mirror.Y - m_SamplePointCollection[1].Y;
                    angleY = GetSkewAngle(diffY, diffX);
                    m_Skew.AngleY = angleY;
                    // Set new sample point
                    ChangePoint(point, index);
                    // Adapt BottomLeft point
                    ChangePoint(mirror, 2);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Returns the position of the opposite point, when squeezing the image.
        /// This is a comfortable way to save a lot of calculation.
        /// </summary>
        /// <param name="point">The input point.</param>
        /// <returns>The output mirror point.</returns>
        private Point MirrorPoint(Point point)
        {
            // Check if point collection exists
            if (m_SamplePointCollection.Count != 4)
                return point;
            return new Point(m_SamplePointCollection[1].X + (m_SamplePointCollection[0].X - point.X), m_SamplePointCollection[1].Y + (m_SamplePointCollection[0].Y - point.Y));
        }

        /// <summary>
        /// Clears the image points and removes the image object from the canvas.
        /// </summary>
        public void ClearSample()
        {
            // Clear point collection
            ClearPoints();
            m_Parent.Children.Remove(m_Image);
        }

        /// <summary>
        /// Adds the point collection to the sample's list (one and only element there).
        /// </summary>
        public void AddSample()
        {
            // Add sample point collection to list
            AddPointsToList();
        }

        /// <summary>
        /// Adds the ID and description to the sample. A tooltip for the image is currently not created,
        /// because it would be present all the time when moving the cursor over the map and possible 
        /// hide the tool tips of the other shapes.
        /// </summary>
        /// <param name="strID">The identifier text.</param>
        /// <param name="strText">The description text.</param>
        public void AddId(string strID, string strText)
        {
            m_Identifier = (strID == string.Empty ? m_SampleType.ToString() : strID);
            m_DisplayText = strText;
            /*
            ToolTip tt = new ToolTip();
            tt.Content = m_Identifier + WpfControl.StrCR + m_DisplayText;
            m_Image.ToolTip = tt;
            */
        }

        /// <summary>
        /// Calculate the angle for the skew transform of an image out of opposite leg and adjacent leg of the triangle.
        /// </summary>
        /// <param name="oppLeg">The opposite leg.</param>
        /// <param name="adjLeg">The adjacent leg.</param>
        /// <returns></returns>
        private double GetSkewAngle(double oppLeg, double adjLeg)
        {
            return Math.Atan(oppLeg / adjLeg) * 180 / Math.PI;
        }

        /// <summary>
        /// Calculate the distance for the skew transform of an image out of angle and adjacent leg of the triangle (-- not used --).
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="adjLeg">The adj leg.</param>
        /// <returns></returns>
        private double GetSkewOppLeg(double angle, double adjLeg)
        {
            return Math.Tan(angle * Math.PI / 180) * adjLeg;
        }

        /// <summary>
        /// Calculate the distance for the skew transform of an image out of angle and opposite leg of the triangle (-- not used --).
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="oppLeg">The opp leg.</param>
        /// <returns></returns>
        private double GetSkewAdjLeg(double angle, double oppLeg)
        {
            return oppLeg / Math.Tan(angle * Math.PI / 180);
        }

        /// <summary>
        /// Adds a pair of reference points for source and target images.
        /// In total 3 reference point pairs are needed to transform a picture by a transformation matrix.
        /// This function is used in Adapt mode, when a map should be calibrated (add coordinates)
        /// using a reference background map. When all points are set, the transformation is calculated
        /// and the source image is transformed to the target image.
        /// </summary>
        /// <param name="sourcePoint">The source point.</param>
        /// <param name="targetPoint">The target point.</param>
        public void AddPickPoint(Point sourcePoint, Point targetPoint)
        {
            m_SourcePickPoints.Add(sourcePoint);
            m_TargetPickPoints.Add(targetPoint);
            // Enough calibration points --> adjust Image within world
            if (m_TargetPickPoints.Count == 3 && m_SamplePointCollection.Count == 4)
            {
                double scaleX = 1;
                double scaleY = 1;
                double skewX = 0;
                double skewY = 0;
                double offX = 0;
                double offY = 0;
                Point pnt0 = new Point();
                Point pnt1 = new Point();
                Point pnt2 = new Point();

                // Get transformation parameters for X
                GetTransformationParams(
                    m_TargetPickPoints[0].X, m_TargetPickPoints[1].X, m_TargetPickPoints[2].X,
                    m_SourcePickPoints[0].X, m_SourcePickPoints[0].Y,
                    m_SourcePickPoints[1].X, m_SourcePickPoints[1].Y,
                    m_SourcePickPoints[2].X, m_SourcePickPoints[2].Y,
                    out scaleX, out skewX, out offX);

                // Get transformation parameters for Y
                GetTransformationParams(
                    m_TargetPickPoints[0].Y, m_TargetPickPoints[1].Y, m_TargetPickPoints[2].Y,
                    m_SourcePickPoints[0].X, m_SourcePickPoints[0].Y,
                    m_SourcePickPoints[1].X, m_SourcePickPoints[1].Y,
                    m_SourcePickPoints[2].X, m_SourcePickPoints[2].Y,
                    out skewY, out scaleY, out offY);

                // Debug: Matrix-Parameters output
                // string str = string.Empty;
                // str = string.Format("scale: {0} {1}  skew: {2} {3}  offs: {4} {5}",
                //      scaleX, scaleY, skewX, skewY, offX, offY);
                // 
                // pnt0.X = scaleX * m_SamplePointCollection[0].X + skewX * m_SamplePointCollection[0].Y + offX;
                // pnt0.Y = skewY * m_SamplePointCollection[0].X + scaleY * m_SamplePointCollection[0].Y + offY;
                // pnt1.X = scaleX * m_SamplePointCollection[1].X + skewX * m_SamplePointCollection[1].Y + offX;
                // pnt1.Y = skewY * m_SamplePointCollection[1].X + scaleY * m_SamplePointCollection[1].Y + offY;
                // pnt2.X = scaleX * m_SamplePointCollection[2].X + skewX * m_SamplePointCollection[2].Y + offX;
                // pnt2.Y = skewY * m_SamplePointCollection[2].X + scaleY * m_SamplePointCollection[2].Y + offY;
                // 

                // Transform params:  scaleX, skewX, (factor)  - skewY, scaleY, (1 = 45°)  -  offsetX, offsetY  (coords)
                MatrixTransform matrixTransform = new MatrixTransform(scaleX, skewY, skewX, scaleY, offX, offY);

                // Debug: Transform source points to check result (target points)
                // pnt0 = matrixTransform.Transform(m_SourcePickPoints[0]);
                // pnt1 = matrixTransform.Transform(m_SourcePickPoints[1]);
                // pnt2 = matrixTransform.Transform(m_SourcePickPoints[2]);

                // Transform the picture corner points
                pnt0 = matrixTransform.Transform(m_SamplePointCollection[0]);
                pnt1 = matrixTransform.Transform(m_SamplePointCollection[1]);
                pnt2 = matrixTransform.Transform(m_SamplePointCollection[2]);

                // Shift Source to Target keeping the first Pickpoint projection
                ChangePosition(pnt0, 0);
                ChangePosition(pnt1, 1);
                ChangePosition(pnt2, 2);
                ClearPickPoints();
            }
        }
        
        /// <summary>
        /// Calculate scale, skew and offset for an image for X or Y from 3 source and 3 target points.
        /// </summary>
        /// <param name="tp0">The target point 0.</param>
        /// <param name="tp1">The target point 1.</param>
        /// <param name="tp2">The target point 2.</param>
        /// <param name="sp0x">The source point 0 in x.</param>
        /// <param name="sp0y">The source point 0 in y.</param>
        /// <param name="sp1x">The source point 1 in x.</param>
        /// <param name="sp1y">The source point 1 in y.</param>
        /// <param name="sp2x">The source point 2 in x.</param>
        /// <param name="sp2y">The source point 2 in y.</param>
        /// <param name="scale">The output scale value.</param>
        /// <param name="skew">The output skew value.</param>
        /// <param name="offs">The output offset value.</param>
        private void GetTransformationParams(
                    double tp0, double tp1, double tp2,
                    double sp0x, double sp0y, double sp1x,
                    double sp1y, double sp2x, double sp2y,
                    out double scale, out double skew, out double offs)
        {
            /* Formulas for matrix transformation parameters regarding target points (tp):
            
            // 1st:
            tp0 = scale * sp0x + skew * sp0y + offs;
            offs = (tp0 - scale * sp0x - skew * sp0y);

            // 2nd:
            tp1 = scale * sp1x + skew * sp1y + tp0 - scale * sp0x - skew * sp0y;
            skew * sp0y - skew * sp1y = tp0 - tp1 + scale * sp1x - scale * sp0x;
            skew * (sp0y - sp1y) = (tp0 - tp1 + scale * sp1x - scale * sp0x);
            skew = ((tp0 - tp1 + scale * sp1x - scale * sp0x) / (sp0y - sp1y));

            // 3rd:
            tp2 = scale * sp2x + ((tp0 - tp1 + scale * sp1x - scale * sp0x) / (sp0y - sp1y)) * sp2y + 
                (tp0 - scale * sp0x - ((tp0 - tp1 + scale * sp1x - scale * sp0x) / (sp0y - sp1y)) * sp0y);

            tp2 = scale * sp2x + tp0 * sp2y / (sp0y - sp1y) - tp1 * sp2y / (sp0y - sp1y) + scale * sp1x * sp2y / (sp0y - sp1y) - scale * sp0x * sp2y / (sp0y - sp1y) +
                tp0 - scale * sp0x - tp0 * sp0y / (sp0y - sp1y) + tp1 * sp0y / (sp0y - sp1y) - scale * sp1x * sp0y / (sp0y - sp1y) + scale * sp0x * sp0y / (sp0y - sp1y));

            scale * sp0x * sp2y / (sp0y - sp1y) - scale * sp2x - scale * sp1x * sp2y / (sp0y - sp1y) + scale * sp0x + scale * sp1x * sp0y / (sp0y - sp1y) - scale * sp0x * sp0y / (sp0y - sp1y) = 
                tp0 * sp2y / (sp0y - sp1y) - tp1 * sp2y / (sp0y - sp1y) + tp0 - tp2 - tp0 * sp0y / (sp0y - sp1y) + tp1 * sp0y / (sp0y - sp1y);

            scale * (sp0x * sp2y / (sp0y - sp1y) - sp2x - sp1x * sp2y / (sp0y - sp1y) + sp0x + sp1x * sp0y / (sp0y - sp1y) - sp0x * sp0y / (sp0y - sp1y)) =
                tp0 * sp2y / (sp0y - sp1y) - tp1 * sp2y / (sp0y - sp1y) + tp0 - tp2 - tp0 * sp0y / (sp0y - sp1y) + tp1 * sp0y / (sp0y - sp1y);

            scale = (tp0 * sp2y / (sp0y - sp1y) - tp1 * sp2y / (sp0y - sp1y) + tp0 - tp2 - tp0 * sp0y / (sp0y - sp1y) + tp1 * sp0y / (sp0y - sp1y)) / (sp0x * sp2y / (sp0y - sp1y) - 
                sp2x - sp1x * sp2y / (sp0y - sp1y) + sp0x + sp1x * sp0y / (sp0y - sp1y) - sp0x * sp0y / (sp0y - sp1y));
            */

            // Scale
            scale = (tp0 - tp2 + (sp2y * (tp0 - tp1) - tp0 * sp0y + tp1 * sp0y ) / (sp0y - sp1y))
                / ( sp0x - sp2x + ( sp0x * sp2y - sp1x * sp2y + sp1x * sp0y - sp0x * sp0y  ) / (sp0y - sp1y) );
            // Skew
            skew = ((tp0 - tp1 + scale * sp1x - scale * sp0x) / (sp0y - sp1y));
            // Offset
            offs = (tp0 - scale * sp0x - skew * sp0y);
        }

        /// <summary>
        /// Clears the complete set of source and target pick points.
        /// </summary>
        public void ClearPickPoints()
        {
            m_TargetPickPoints.Clear();
            m_SourcePickPoints.Clear();
        }

        /// <summary>
        /// Brings the image to top of the canvas.
        /// </summary>
        public void BringToBottom()
        {
            m_Parent.Children.Remove(m_Image);
            m_Parent.Children.Insert(0, m_Image);
        }

        #endregion // Methods
    }
}
