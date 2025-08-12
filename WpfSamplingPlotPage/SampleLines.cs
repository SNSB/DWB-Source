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

namespace WpfSamplingPlotPage
{
    /// <summary>
    /// Sample class for Lines.
    /// </summary>
    class SampleLines : Sample
    {
        #region Constants

        internal const int MinPoints = 2;

        #endregion // Constants

        #region Fields

        /// <summary>
        /// The current Polyline object.
        /// </summary>
        public Polyline m_Poly;
        /// <summary>
        /// The list of all Polyline objects of the sample (MULTILINESTRING).
        /// </summary>
        public List<Polyline> m_PolyList = new List<Polyline>();

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
                m_Poly.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B));
                foreach (Polyline poly in m_PolyList)
                    poly.Stroke = m_Poly.Stroke;
            }
        }

        /// <summary>
        /// Gets or sets the stroke transparency.
        /// </summary>
        /// <value>The stroke transparency.</value>
        public byte StrokeTransparency
        {
            get { return m_StrokeTransparency; }
            set 
            { 
                m_Poly.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency = value, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B));
                foreach (Polyline poly in m_PolyList)
                    poly.Stroke = m_Poly.Stroke;
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness
        {
            get { return m_StrokeThickness; }
            set 
            { 
                m_Poly.StrokeThickness = m_StrokeThickness = value;
                foreach (Polyline poly in m_PolyList)
                    poly.StrokeThickness = m_Poly.StrokeThickness;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the visibility of the polyline.
        /// </summary>
        /// <value><c>true</c> if the sample is visible; otherwise, <c>false</c>.</value>
        public bool SampleVisibility
        {
            get { return m_SampleVisibility; }
            set 
            {
                m_SampleVisibility = value;
                foreach (Polyline poly in m_PolyList)
                    poly.Opacity = (m_SampleVisibility ? 1.0 : ((double)m_SampleOffTransparency / 255.0));
            }
        }

        #endregion // Properties

        #region Construction

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleLines"/> class.
        /// Creates a current polyline and adds it to the given canvas.
        /// </summary>
        /// <param name="canvas">The parent canvas of the sample's polylines.</param>
        public SampleLines(Canvas canvas)
        {
            // Create Polyline object
            m_Poly = new Polyline();

            // Init Polyline 
            m_Poly.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B));
            m_Poly.StrokeThickness = m_StrokeThickness = 2.0;
            // Set sample type
            m_SampleType = SampleType.LINESTRING;

            // Add to parent control
            m_Parent = canvas;
            m_Parent.Children.Add(m_Poly);
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Sets all sample points of a polyline from the given point collection.
        /// </summary>
        /// <param name="points">The input point collection.</param>
        public void SetSamplePoint(PointCollection points)
        {
            // Create complete set
            foreach (Point point in points)
                SetSamplePoint(point);
        }

        /// <summary>
        /// Adds a sample point for a polyline.
        /// </summary>
        /// <param name="point">The input point.</param>
        public void SetSamplePoint(Point point)
        {
            // Add point to point collection
            SetPoint(point);

            // Add point to Polyline
            m_Poly.Points.Add(point);
        }

        /// <summary>
        /// Replaces the last point of the current polyline.
        /// </summary>
        /// <param name="point">The input point.</param>
        public void ChangeSamplePoint(Point point)
        {
            // Replace last point of point collection
            ChangePoint(point);

            // Replace last point of Polyline
            m_Poly.Points.RemoveAt(m_Poly.Points.Count - 1);
            m_Poly.Points.Add(point);
        }

        /// <summary>
        /// Replaces a specific point of the current polyline.
        /// </summary>
        /// <param name="point">The input point.</param>
        /// <param name="pointIndex">Index of the point to replace.</param>
        public void ChangeSamplePoint(Point point, int pointIndex)
        {
            // Replace a point of the Pointcollection list
            ChangePoint(point, pointIndex);

            // Replace a point of a Polyline list entry
            m_Poly.Points.RemoveAt(pointIndex);
            m_Poly.Points.Insert(pointIndex, point);
        }

        /// <summary>
        /// Replaces a specific point of a specific polyline of the sample's polyline list.
        /// </summary>
        /// <param name="point">The input point.</param>
        /// <param name="pointIndex">Index of the point to replace.</param>
        /// <param name="listIndex">Index of the point collection list.</param>
        public void ChangeSamplePoint(Point point, int pointIndex, int listIndex)
        {
            // Replace a point of the Pointcollection list
            ChangePoint(point, pointIndex, listIndex);

            // Replace a point of a Polyline list entry
            m_PolyList[listIndex].Points.RemoveAt(pointIndex);
            m_PolyList[listIndex].Points.Insert(pointIndex, point);
        }

        /// <summary>
        /// Removes the last sample point of the current polyline.
        /// </summary>
        public void ClearSamplePoint()
        {
            // Remove last point of point collection
            ClearSamplePoint(m_SamplePointCollection.Count - 1);
        }

        /// <summary>
        /// Removes a specific sample point of the current polyline.
        /// </summary>
        /// <param name="index">Index of the point to remove.</param>
        public void ClearSamplePoint(int index)
        {
            if (index < 0)
                return;

            // Remove last point of point collection
            ClearPoint(index);

            // Remove last point of Polyline
            m_Poly.Points.RemoveAt(index);
        }

        /// <summary>
        /// Clears all polylines of the entire sample.
        /// </summary>
        public void ClearSample()
        {
            // Clear point collection
            ClearPoints();

            // Clear current Polyline and List
            m_Poly.Points.Clear();
            foreach (Polyline poly in m_PolyList)
                m_Parent.Children.Remove(poly);
            m_PolyList.Clear();
        }

        /// <summary>
        /// Removes adjacent identical points of the current polyline.
        /// </summary>
        public void CleanupPoints()
        {
            try
            {
                // Cleanup adjacent identical points
                for (int i = m_SamplePointCollection.Count - 1; i > 0; i--)
                {
                    if (m_SamplePointCollection[i] == m_SamplePointCollection[i - 1])
                        ClearSamplePoint(i);
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        /// <summary>
        /// Adds the current polyline to the sample's polyline list.
        /// Switches sample type to MULTILINESTRING, if more than one sample is in the list.
        /// </summary>
        public void AddSample()
        {
            // Cleanup polyline points
            CleanupPoints();
            // Check for minimum required points
            if (SamplePointCountCurrent >= MinPoints)
            {
                // Add point to point collection
                m_PolyList.Add(m_Poly);
                // Add sample point collection to list
                AddPointsToList();
                // Set Multi Type, if applicable
                if (m_PolyList.Count > 1)
                    m_SampleType = SampleType.MULTILINESTRING;
            }
            else
            {
                // Clear point collection
                ClearPoints();
                m_Poly.Points.Clear();
            }
        }

        /// <summary>
        /// Adds the ID and description to the sample and creates a tooltip for each polyline in the list.
        /// </summary>
        /// <param name="strID">The identifier text.</param>
        /// <param name="strText">The description text.</param>
        public void AddId(string strID, string strText)
        {
            m_Identifier = (strID == string.Empty ? m_SampleType.ToString() : strID);
            m_DisplayText = strText;
            ToolTip tt = SetToolTip(m_Identifier + WpfControl.StrCR + m_DisplayText);
            foreach (Polyline poly in m_PolyList)
            {
                poly.ToolTip = tt;
                ToolTipService.SetShowDuration(poly, 300000);
            }
        }

        /// <summary>
        /// Creates and initializes a new polyline and point collection for the sample.
        /// Adds the polyline to the parent canvas.
        /// </summary>
        public void NewSample()
        {
            // Create Polyline object
            m_Poly = new Polyline();

            // Init new Polyline 
            m_Poly.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B));
            m_Poly.StrokeThickness = m_StrokeThickness;
            // Add to canvas
            m_Parent.Children.Add(m_Poly);
            // Create new sample point collection
            NewPointCollection();
        }

        /// <summary>
        /// Animate the sample of the selected toggle button.
        /// </summary>
        /// <param name="animate">Change color if true, reset if false.</param>
        internal void Animate(bool animate)
        {
            // Too many polygons, do not animate!
            if (m_PolyList.Count > 10)
                return;

            byte transparency = 255;
            if (animate)
            {
                m_Poly.Stroke = new SolidColorBrush(Color.FromArgb(transparency, m_StrokeBrush.Color.B, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G));
                foreach (Polyline poly in m_PolyList)
                    poly.Stroke = m_Poly.Stroke;
            }
            else
            {
                m_Poly.Stroke = new SolidColorBrush(Color.FromArgb(m_StrokeTransparency, m_StrokeBrush.Color.R, m_StrokeBrush.Color.G, m_StrokeBrush.Color.B));
                foreach (Polyline poly in m_PolyList)
                    poly.Stroke = m_Poly.Stroke;
            }
        }

        #endregion // Methods
    }
}
