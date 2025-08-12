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
        #region Public enumerations

    /// <summary>
    /// Enumeration of the change mode for frame move or resize according to catched position
    /// </summary>
    public enum CaptureFrameMode 
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
        ResizeBottomRight
    };

    #endregion // Public enumerations

    /// <summary>
    /// Frame class for partly capture of working area.
    /// </summary>
    class CaptureFrame : FrameworkElement
    {
        #region Constants

        /// <summary>
        /// Defines the matching distance between cursor position and point.
        /// </summary>
        private const double MatchDiff = 4.0;
        internal const double MinFrameWidth = 10;
        internal const double MinFrameHeight = 10;

        #endregion // Constants

        #region Fields

        /// <summary>
        /// The Rectangle object defining the frame.
        /// </summary>
        internal Rectangle m_Frame;
        private Point m_FramePosition;

        /// <summary>
        /// The parent control where the polygon is attached to.
        /// </summary>
        private Canvas m_Parent;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets the frame position.
        /// </summary>
        /// <value>The frame position.</value>
        public Point FramePosition
        {
            get 
            { 
                return m_FramePosition; 
            }
            set 
            { 
                m_FramePosition = value;
                Canvas.SetLeft(m_Frame, m_FramePosition.X);
                Canvas.SetTop(m_Frame, m_FramePosition.Y);
            }
        }

        public double FrameWidth
        {
            get { return m_Frame.Width; }
        }

        public double FrameHeight
        {
            get { return m_Frame.Height; }
        }

        #endregion // Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptureFrame"/> class.
        /// Creates a frame and adds it to the given canvas.
        /// </summary>
        /// <param name="canvas">The parent canvas of the frame.</param>
        /// <param name="pos">The position of the frame.</param>
        public CaptureFrame(Canvas canvas, Point pos)
        {
            // Create frame
            m_Frame = new Rectangle();

            // Init frame 
            m_Frame.Stroke = Brushes.Red;
            m_Frame.StrokeThickness = 2.0;
            m_Frame.Fill = new SolidColorBrush(Color.FromArgb(50, 255, 255, 0));
            m_Frame.Width = 0;
            m_Frame.Height = 0;

            // Add to parent control
            m_Parent = canvas;
            m_Parent.Children.Add(m_Frame);
        }

        #endregion // Construction

        #region Methods

        public void SetSize(double width, double height)
        {
            // Check minimum width
            if (width < MinFrameWidth)
                m_Frame.Width = MinFrameWidth;
            else
                m_Frame.Width = width;
            // Check minimum height
            if (height < MinFrameHeight)
                m_Frame.Height = MinFrameHeight;
            else
                m_Frame.Height = height;
        }

        /// <summary>
        /// Removes the frame from the canvas.
        /// </summary>
        public void ClearFrame()
        {
            m_Parent.Children.Remove(m_Frame);
        }

        /// <summary>
        /// Adds the frame to the canvas.
        /// </summary>
        public void ShowFrame()
        {
            m_Parent.Children.Add(m_Frame);
        }

        /// <summary>
        /// Brings the frame to top of the canvas.
        /// </summary>
        public void BringToTop()
        {
            m_Parent.Children.Remove(m_Frame);
            m_Parent.Children.Add(m_Frame);
        }

        public CaptureFrameMode ChangeMode(Point pos)
        {
            if (MatchPoint(pos, m_FramePosition))
                return CaptureFrameMode.ResizeTopLeft;
            else if (MatchPoint(pos, new Point(m_FramePosition.X, m_FramePosition.Y + m_Frame.Height)))
                return CaptureFrameMode.ResizeBottomLeft;
            else if (MatchPoint(pos, new Point(m_FramePosition.X + m_Frame.Width, m_FramePosition.Y + m_Frame.Height)))
                return CaptureFrameMode.ResizeBottomRight;
            else if (MatchPoint(pos, new Point(m_FramePosition.X + m_Frame.Width, m_FramePosition.Y)))
                return CaptureFrameMode.ResizeTopRight;
            else
                return CaptureFrameMode.Move;
        }

        private bool MatchPoint(Point point, Point cursorPos)
        {
            if (Math.Abs(point.X - cursorPos.X) < MatchDiff && Math.Abs(point.Y - cursorPos.Y) < MatchDiff)
                return true;

            return false;
        }

        #endregion // Methods
    }
}
