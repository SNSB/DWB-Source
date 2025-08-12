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
using System.Windows.Media;
using System.Windows.Controls;
// using System.Windows.Media.Media3D;

namespace WpfSamplingPlotPage
{
    #region Public enumerations

    /// <summary>
    /// Enumeration of the supported sample types (Image and MS SQL geometry objects).
    /// </summary>
    public enum SampleType 
    {
        /// <summary>
        /// A bitmap image, usually a background map which is used to show the location of geometry objects.
        /// </summary>
        IMAGE,
        /// <summary>
        /// A single point, illustrated with a point symbol.
        /// </summary>
        POINT,
        /// <summary>
        /// A collection of points, illustrated with the same point symbols.
        /// </summary>
        MULTIPOINT,
        /// <summary>
        /// One set of concatenated lines.
        /// </summary>
        LINESTRING,
        /// <summary>
        /// A collection of line strings.
        /// </summary>
        MULTILINESTRING,
        /// <summary>
        /// One closed polygon, illustrating an area.
        /// </summary>
        POLYGON,
        /// <summary>
        /// A collection of closed polygons, showing different areas.
        /// </summary>
        MULTIPOLYGON,
        /// <summary>
        /// A collection of geometry objects of different types.
        /// </summary>
        GEOMETRYCOLLECTION,
        /// <summary>
        /// Unknown type of sample.
        /// </summary>
        Unknown 
    };

    /// <summary>
    /// Enumeration of point resentation symbols (to be extended).
    /// </summary>
    public enum PointSymbol 
    {
        /// <summary>
        /// A pin symbol.
        /// </summary>
        Pin,
        /// <summary>
        /// A cross symbol.
        /// </summary>
        Cross,
        /// <summary>
        /// An "X" symbol.
        /// </summary>
        X,
        /// <summary>
        /// A circle symbol.
        /// </summary>
        Circle,
        /// <summary>
        /// A square symbol.
        /// </summary>
        Square,
        /// <summary>
        /// A square symbol.
        /// </summary>
        SquareSmall,
        /// <summary>
        /// A diamond symbol.
        /// </summary>
        Diamond,
        /// <summary>
        /// A pyramid symbol.
        /// </summary>
        Pyramid,
        /// <summary>
        /// A cone symbol.
        /// </summary>
        Cone,
        /// <summary>
        /// A minus symbol.
        /// </summary>
        Minus,
        /// <summary>
        /// A Questionmark symbol.
        /// </summary>
        Questionmark,
        /// <summary>
        /// A circle with centerpoint symbol.
        /// </summary>
        Circlepoint,

        /// <summary>
        /// A Assel icon.
        /// </summary>
        Assel_Icon,
        /// <summary>
        /// A Bird icon.
        /// </summary>
        Bird_Icon,
        /// <summary>
        /// A Bryophyt icon.
        /// </summary>
        Bryophyt_Icon,
        /// <summary>
        /// A Echinoderm icon.
        /// </summary>
        Echinoderm_Icon,
        /// <summary>
        /// A Evertebrate icon.
        /// </summary>
        Evertebrate_Icon,
        /// <summary>
        /// A Fish icon.
        /// </summary>
        Fish_Icon,
        /// <summary>
        /// A Fungus icon.
        /// </summary>
        Fungus_Icon,
        /// <summary>
        /// A Insect icon.
        /// </summary>
        Insect_Icon,
        /// <summary>
        /// A Lichen icon.
        /// </summary>
        Lichen_Icon,
        /// <summary>
        /// A Mammal icon.
        /// </summary>
        Mammal_Icon,
        /// <summary>
        /// A Mollusc icon.
        /// </summary>
        Mollusc_Icon,
        /// <summary>
        /// A Myxomycete icon.
        /// </summary>
        Myxomycete_Icon,
        /// <summary>
        /// A Plant icon.
        /// </summary>
        Plant_Icon,
        /// <summary>
        /// A Reptile icon.
        /// </summary>
        Reptile_Icon,
        /// <summary>
        /// A Vertebrate icon.
        /// </summary>
        Vertebrate_Icon,
        /// <summary>
        /// A Vertebrate icon.
        /// </summary>
        RedNeedle_Icon,
        /// <summary>
        /// No symbol.
        /// </summary>
        None 
    };

    #endregion // Public enumerations

    /// <summary>
    /// Sample base class for Image, Polygons, Lines and Points samples.
    /// </summary>
    class Sample : FrameworkElement
    {
        #region Constants

        /// <summary>
        /// Defines the matching distance between cursor position and point.
        /// </summary>
        internal const double MatchDiff = 3.0;

        #endregion // Constants

        #region Fields

        /// <summary>
        /// The type of the sample.
        /// </summary>
        internal SampleType m_SampleType = SampleType.Unknown;
        /// <summary>
        /// The sample identifier.
        /// </summary>
        internal string m_Identifier = string.Empty;
        /// <summary>
        /// The description of the sample.
        /// </summary>
        internal string m_DisplayText = string.Empty;
        /// <summary>
        /// The stroke color of the sample.
        /// </summary>
        internal SolidColorBrush m_StrokeBrush = Brushes.Red;
        /// <summary>
        /// The fill color of the sample.
        /// </summary>
        internal SolidColorBrush m_FillBrush = Brushes.Red;
        /// <summary>
        /// The stroke transparency of the sample (0..255).
        /// </summary>
        internal byte m_StrokeTransparency = 255;
        /// <summary>
        /// The fill transparency of the sample (0..255).
        /// </summary>
        internal byte m_FillTransparency = 64;
        /// <summary>
        /// The thickness of the sample's stroke.
        /// </summary>
        internal double m_StrokeThickness = 2.0;
        /// <summary>
        /// The opacity of the switched off sample (0..255).
        /// </summary>
        internal byte m_SampleOffTransparency = 64;
        /// <summary>
        /// A flag indicating whether the sample is visible or not.
        /// </summary>
        internal bool m_SampleVisibility = true;
        /// <summary>
        /// The symbol of an object marker (only valid for type POINT and MULTIPOINT).
        /// </summary>
        internal PointSymbol m_SamplePointSymbol = PointSymbol.Pin;
        /// <summary>
        /// The size factor of the point symbol.
        /// </summary>
        internal double m_SamplePointSymbolSize = 1.0;
        
        // Sample points
        /// <summary>
        /// The current sample point collection.
        /// </summary>
        internal PointCollection m_SamplePointCollection = new PointCollection();
        /// <summary>
        /// The list of the sample's point collections.
        /// </summary>
        private List<PointCollection> m_SamplePointCollectionList = new List<PointCollection>();

        // Sample world points
        /// <summary>
        /// The current sample world point collection.
        /// </summary>
        internal PointCollection m_SampleWorldPointCollection = new PointCollection();
        /// <summary>
        /// The list of the sample's world point collections.
        /// </summary>
        private List<PointCollection> m_SampleWorldPointCollectionList = new List<PointCollection>();

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets the type of the sample.
        /// </summary>
        /// <value>The type.</value>
        public SampleType TypeOfSample
        {
            get { return m_SampleType; }
            set { m_SampleType = value; }
        }

        /// <summary>
        /// Gets or sets the identifier of the sample.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier
        {
            get { return m_Identifier; }
            set { m_Identifier = value; }
        }

        /// <summary>
        /// Gets or sets the display text of the sample.
        /// </summary>
        /// <value>The display text.</value>
        public string DisplayText
        {
            get { return m_DisplayText; }
            set { m_DisplayText = value; }
        }

        /// <summary>
        /// Gets or sets the sample stroke brush.
        /// </summary>
        /// <value>The stroke brush.</value>
        public Brush SampleStrokeBrush
        {
            get { return m_StrokeBrush; }
            set { m_StrokeBrush = (SolidColorBrush)value; }
        }

        /// <summary>
        /// Gets or sets the sample fill brush.
        /// </summary>
        /// <value>The fill brush.</value>
        public Brush SampleFillBrush
        {
            get { return m_FillBrush; }
            set { m_FillBrush = (SolidColorBrush)value; }
        }

        /// <summary>
        /// Gets or sets the sample stroke transparency.
        /// </summary>
        /// <value>The stroke transparency.</value>
        public byte SampleStrokeTransparency
        {
            get { return m_StrokeTransparency; }
            set { m_StrokeTransparency = value; }
        }

        /// <summary>
        /// Gets or sets the sample fill transparency.
        /// </summary>
        /// <value>The fill transparency.</value>
        public byte SampleFillTransparency
        {
            get { return m_FillTransparency; }
            set { m_FillTransparency = value; }
        }

        /// <summary>
        /// Gets or sets the sample stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double SampleStrokeThickness
        {
            get { return m_StrokeThickness; }
            set { m_StrokeThickness = value; }
        }
        
        // Visibility Flag
        /// <summary>
        /// Gets a value indicating whether this sample is visible.
        /// </summary>
        /// <value><c>true</c> if this sample is visible; otherwise, <c>false</c>.</value>
        public bool IsSampleVisible
        {
            get { return m_SampleVisibility; }
        }

        /// <summary>
        /// Gets or sets the opacity of the switched off sample.
        /// </summary>
        /// <value>The sample opacity if switched off.</value>
        public byte SampleOffTransparency
        {
            get { return m_SampleOffTransparency; }
            set { m_SampleOffTransparency = value; }
        }

        /// <summary>
        /// Gets or sets the point collection of the sample.
        /// </summary>
        /// <value>The sample point collection.</value>
        public PointCollection SamplePointCollection
        {
            get { return m_SamplePointCollection; }
            set { m_SamplePointCollection = value; }
        }

        /// <summary>
        /// Gets or sets the sample's point collection list.
        /// </summary>
        /// <value>The sample point collection list.</value>
        public List<PointCollection> SamplePointCollectionList
        {
            get { return m_SamplePointCollectionList; }
            set { m_SamplePointCollectionList = value; }
        }

        /// <summary>
        /// Gets or sets the sample's world point collection list.
        /// </summary>
        /// <value>The sample world point collection list.</value>
        public List<PointCollection> SampleWorldPointCollectionList
        {
            get { return m_SampleWorldPointCollectionList; }
            set { m_SampleWorldPointCollectionList = value; }
        }

        /// <summary>
        /// Gets or sets the sample point symbol.
        /// </summary>
        /// <value>The sample point symbol.</value>
        public PointSymbol SamplePointSymbol
        {
            get { return m_SamplePointSymbol; }
            set { m_SamplePointSymbol = value; }
        }

        /// <summary>
        /// Gets or sets the size of the point symbol.
        /// </summary>
        /// <value>The size of the point.</value>
        public double SamplePointSymbolSize
        {
            get { return m_SamplePointSymbolSize; }
            set { m_SamplePointSymbolSize = value; }
        }

        /// <summary>
        /// Gets the number of sample points of the complete sample.
        /// </summary>
        /// <value>Number of points.</value>
        public int SamplePointCountAll
        {
            get
            {
                int count = m_SamplePointCollection.Count;
                if (count >= 2 && m_SamplePointCollection[0] == m_SamplePointCollection[1])
                    count--;
                foreach (PointCollection pointCollection in m_SamplePointCollectionList)
                    count += pointCollection.Count;
                return count;
            }
        }

        /// <summary>
        /// Gets the number of sample points of the current sample shape.
        /// </summary>
        /// <value>Number of points.</value>
        public int SamplePointCountCurrent
        {
            get
            {
                int count = m_SamplePointCollection.Count;
                if (count >= 2 && m_SamplePointCollection[0] == m_SamplePointCollection[1])
                    count--;
                return count;
            }
        }

        #endregion // Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Sample"/> class.
        /// </summary>
        public Sample()
        {
            m_SampleOffTransparency = WpfSamplingPlotPage.Properties.Settings.Default.SampleOffTransparency;
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Adds a point to the point collection.
        /// </summary>
        /// <param name="point">The point.</param>
        public void SetPoint(Point point)
        {
            // Add point to point collection
            m_SamplePointCollection.Add(point);
        }

        /// <summary>
        /// Inserts a point at the given index of the point collection.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="point">The point.</param>
        public void SetPoint(int index, Point point)
        {
            // Add point to point collection
            m_SamplePointCollection.Insert(index, point);
        }

        /// <summary>
        /// Replaces the last point of the point collection.
        /// </summary>
        /// <param name="point">The point.</param>
        internal void ChangePoint(Point point)
        {
            // Replace last point of point collection
            m_SamplePointCollection.RemoveAt(m_SamplePointCollection.Count - 1);
            m_SamplePointCollection.Add(point);
        }

        /// <summary>
        /// Replaces the specific point of the given point collection index.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="pointIndex">Index of the point to replace.</param>
        internal void ChangePoint(Point point, int pointIndex)
        {
            // Replace specified point of current point collection
            m_SamplePointCollection.RemoveAt(pointIndex);
            m_SamplePointCollection.Insert(pointIndex, point);
        }

        /// <summary>
        /// Replaces the specified point of specified point collection.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="pointIndex">Index of the point.</param>
        /// <param name="listIndex">Index of the collection.</param>
        internal void ChangePoint(Point point, int pointIndex, int listIndex)
        {
            // Replace specified point of specified point collection
            m_SamplePointCollectionList[listIndex].RemoveAt(pointIndex);
            m_SamplePointCollectionList[listIndex].Insert(pointIndex, point);
        }

        /// <summary>
        /// Removes the specific point at position index of the current point collection.
        /// </summary>
        /// <param name="index">The index.</param>
        internal void ClearPoint(int index)
        {
            // Remove point of point collection index
            m_SamplePointCollection.RemoveAt(index);
        }

        /// <summary>
        /// Clears the entire current point collection.
        /// </summary>
        public void ClearPoints()
        {
            // Clear point collection
            m_SamplePointCollection.Clear();
        }

        /// <summary>
        /// Adds the current point collection to the point collection list.
        /// </summary>
        internal void AddPointsToList()
        {
            m_SamplePointCollectionList.Add(m_SamplePointCollection);
        }

        /// <summary>
        /// Creates a new sample point collection as the current collection.
        /// </summary>
        internal void NewPointCollection()
        {
            m_SamplePointCollection = new PointCollection();
        }

        /// <summary>
        /// Check if the given input position matches a point in any point collection of the sample.
        /// </summary>
        /// <param name="cursorPos">The input cursor position.</param>
        /// <param name="listIndex">The output index of the matching point collection of the collection list.</param>
        /// <param name="pointIndex">The output index of the matching point of the collection.</param>
        /// <returns><c>true</c> if matches, <c>false</c> if no match.</returns>
        internal bool MatchPoint(Point cursorPos, ref int listIndex, ref int pointIndex)
        {
            foreach (PointCollection pointCollection in m_SamplePointCollectionList)
                if (MatchPoint(cursorPos, ref pointIndex, pointCollection))
                {
                    listIndex = m_SamplePointCollectionList.IndexOf(pointCollection);
                    return true;
                }
            return false;
        }

        // Check for point match in actual m_SamplePointCollection
        /// <summary>
        /// Check if the given input position matches a point in the current point collection of the sample.
        /// </summary>
        /// <param name="cursorPos">The input cursor position.</param>
        /// <param name="pointIndex">The output index of the matching point of the current collection.</param>
        /// <returns></returns>
        internal bool MatchPoint(Point cursorPos, ref int pointIndex)
        {
            return MatchPoint(cursorPos, ref pointIndex, m_SamplePointCollection);
        }

        // Check for point match in a single PointCollection of the m_SamplePointCollectionList
        /// <summary>
        /// Check if the given input position matches a point in the given single point collection of the sample.
        /// </summary>
        /// <param name="cursorPos">The input cursor position.</param>
        /// <param name="pointIndex">The output index of the matching point of the collection.</param>
        /// <param name="pointCollection">The input point collection to check.</param>
        /// <returns></returns>
        internal bool MatchPoint(Point cursorPos, ref int pointIndex, PointCollection pointCollection)
        {
            foreach (Point point in pointCollection)
                if (Math.Abs(point.X - cursorPos.X) < MatchDiff && Math.Abs(point.Y - cursorPos.Y) < MatchDiff)
                {
                    pointIndex = pointCollection.IndexOf(point);
                    return true;
                }
            return false;
        }


        // Store the genuine world coordinates, aditionally to the converted screen coordinates in pointcollections

        /// <summary>
        /// Adds a point to the world point collection.
        /// </summary>
        /// <param name="point">The point.</param>
        public void SetWorldPoint(Point point)
        {
            // Add point to point collection
            m_SampleWorldPointCollection.Add(point);
        }

        /// <summary>
        /// Adds a point collection to the world point collection.
        /// </summary>
        /// <param name="points">The point collection.</param>
        public void SetWorldPoint(PointCollection points)
        {
            // Create complete world point set
            foreach (Point point in points)
                m_SampleWorldPointCollection.Add(point);
            AddWorldPointsToList();
            NewWorldPointCollection();
        }

        /// <summary>
        /// Adds the current point collection to the point collection list.
        /// </summary>
        internal void AddWorldPointsToList()
        {
            m_SampleWorldPointCollectionList.Add(m_SampleWorldPointCollection);
        }

        /// <summary>
        /// Creates a new sample point collection as the current collection.
        /// </summary>
        internal void NewWorldPointCollection()
        {
            m_SampleWorldPointCollection = new PointCollection();
        }

        /// <summary>
        /// Animate the sample of the selected toggle button.
        /// </summary>
        /// <param name="animate">Change color if true, reset if false.</param>
        internal void Animate(bool animate)
        {
            switch (this.m_SampleType)
            {
                case SampleType.POINT:
                case SampleType.MULTIPOINT:
                    (this as SamplePoints).Animate(animate);
                    break;

                case SampleType.LINESTRING:
                case SampleType.MULTILINESTRING:
                    (this as SampleLines).Animate(animate);
                    break;

                case SampleType.POLYGON:
                case SampleType.MULTIPOLYGON:
                    (this as SamplePolygon).Animate(animate);
                    break;

            }
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
            tt.FontFamily = new FontFamily("Consolas");
            tt.Content = text;
            return tt;
        }

        #endregion // Methods
    }
}
