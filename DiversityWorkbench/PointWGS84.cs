using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DiversityWorkbench
{
    /// <summary>
    /// Class to handle WGS84 coordinates.
    /// </summary>
    public class PointWGS84
    {
        #region Constants

        // Conversion factors
        private const double MinFactor = 1.0 / 60.0;
        private const double SecFactor = 1.0 / 3600.0;
        private const string StrDegMinSec = "{0}° {1}' {2}\"";
        // MTB calculation
        // private const double MtbLonMin = 5.83276865;
        // private const double MtbLonMax = 22.33064488;
        // private const double MtbLatMin = 46.09948899;
        // private const double MtbLatMax = 55.99843824;

        #endregion

        #region Fields

        // Longitude variables
        private double m_Longitude = 0.0;
        private int m_LongitudeDeg = 0;
        private int m_LongitudeMin = 0;
        private int m_LongitudeSec = 0;
        private double m_LongitudeMinDec = 0.0;
        private double m_LongitudeSecDec = 0.0;

        // Latitude variables
        private double m_Latitude = 0.0;
        private int m_LatitudeDeg = 0;
        private int m_LatitudeMin = 0;
        private int m_LatitudeSec = 0;
        private double m_LatitudeMinDec = 0.0;
        private double m_LatitudeSecDec = 0.0;

        private bool m_IsValid = true;

        #endregion

        #region Properties

        // Get or set PointWGS84 coordinates

        /// <summary>
        /// Set and get decimal longitude value.
        /// </summary>
        public double LongitudeDecimal
        {
            get { return m_Longitude; }
            set 
            {
                m_Longitude = value;
                if (!InvalidCoordinates())
                {
                    SetLongitudeFields();
                }
            }
        }

        /// <summary>
        /// Set and get decimal latitude value.
        /// </summary>
        public double LatitudeDecimal
        {
            get { return m_Latitude; }
            set 
            {
                m_Latitude = value;
                if (!InvalidCoordinates())
                {
                    SetLatitudeFields();
                }
            }
        }
        /// <summary>
        /// Set and get coordinates point value.
        /// </summary>
        public Point LonLatPointDecimal
        {
            get { return new Point(m_Longitude, m_Latitude); }
            set
            {
                m_Longitude = value.X;
                m_Latitude = value.Y;
                if (!InvalidCoordinates())
                {
                    SetLongitudeFields();
                    SetLatitudeFields();
                }
            }
        }

        // Get PointWGS84 coordinate fields

        /// <summary>
        /// Get longitude integer degrees.
        /// </summary>
        public int LongitudeDegrees
        {
            get { return m_LongitudeDeg; }
        }

        /// <summary>
        /// Get latitude integer degrees.
        /// </summary>
        public int LatitudeDegrees
        {
            get { return m_LatitudeDeg; }
        }

        /// <summary>
        /// Get longitude integer minutes.
        /// </summary>
        public int LongitudeMinutes
        {
            get { return m_LongitudeMin; }
        }

        /// <summary>
        /// Get latitude integer minutes.
        /// </summary>
        public int LatitudeMinutes
        {
            get { return m_LatitudeMin; }
        }

        /// <summary>
        /// Get longitude integer seconds.
        /// </summary>
        public int LongitudeSeconds
        {
            get { return m_LongitudeSec; }
        }

        /// <summary>
        /// Get latitude integer seconds.
        /// </summary>
        public int LatitudeSeconds
        {
            get { return m_LatitudeSec; }
        }

        /// <summary>
        /// Get longitude decimal minutes.
        /// </summary>
        public double LongitudeMinutesDecimal
        {
            get { return m_LongitudeMinDec; }
        }

        /// <summary>
        /// Get latitude decimal minutes.
        /// </summary>
        public double LatitudeMinutesDecimal
        {
            get { return m_LatitudeMinDec; }
        }

        /// <summary>
        /// Get longitude decimal seconds.
        /// </summary>
        public double LongitudeSecondsDecimal
        {
            get { return m_LongitudeSecDec; }
        }

        /// <summary>
        /// Get latitude decimal seconds.
        /// </summary>
        public double LatitudeSecondsDecimal
        {
            get { return m_LatitudeSecDec; }
        }

        /// <summary>
        /// Get longitude minutes seconds as a string.
        /// </summary>
        public string LongitudeDegMinSecString
        {
            get { return string.Format(StrDegMinSec, m_LongitudeDeg, m_LongitudeMin, m_LongitudeSec); }
        }

        /// <summary>
        /// Get latitude  minutes seconds as a string.
        /// </summary>
        public string LatitudeDegMinSecString
        {
            get { return string.Format(StrDegMinSec, m_LatitudeDeg, m_LatitudeMin, m_LatitudeSec); }
        }

        /// <summary>
        /// Get longitude hemisphere.
        /// </summary>
        public string LongitudeHemisphere
        {
            get { return (m_Longitude < 0) ? "W" : "E"; }
        }

        /// <summary>
        /// 
        /// Get latitude hemisphere.
        /// </summary>
        public string LatitudeHemisphere
        {
            get { return (m_Latitude < 0) ? "S" : "N"; }
        }

        /// <summary>
        /// Returns valid coordinates flag.
        /// </summary>
        public bool IsValid
        {
            get { return m_IsValid; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Create an instance by decimal longitude and latitude.
        /// </summary>
        /// <param name="longitude">Longitude</param>
        /// <param name="latitude">Latitude</param>
        public PointWGS84(double longitude, double latitude)
        {
            // Set decimal coordinate values
            m_Longitude = longitude;
            m_Latitude = latitude;
            // Check for valid values
            if (InvalidCoordinates())
                return;
            // Adapt coordinate fields
            SetLongitudeFields();
            SetLatitudeFields();
        }

        /// <summary>
        /// Create an instance by POINT value.
        /// </summary>
        /// <param name="lonLatPoint">POINT(longitude, latitude)</param>
        public PointWGS84(Point lonLatPoint)
        {
            // Set and check decimal coordinate values and adapt fields
            LonLatPointDecimal = lonLatPoint;
        }

        /// <summary>
        /// Create an instance by integer longitude/latitude degrees values and decimal minutes values.
        /// </summary>
        /// <param name="lonDegrees">Longitude degrees</param>
        /// <param name="lonMinutes">Longitude decimal minutes</param>
        /// <param name="latDegrees">Latitude degrees</param>
        /// <param name="latMinutes">Latitude decimal minutes</param>
        public PointWGS84(int lonDegrees, double lonMinutes, int latDegrees, double latMinutes)
        {
            // Set decimal coordinate values
            if (!DegMinToDecimal(out m_Longitude, lonDegrees, lonMinutes))
                return;
            if (!DegMinToDecimal(out m_Latitude, latDegrees, latMinutes))
                return;
            // Check for valid values
            if (InvalidCoordinates())
                return;
            // Adapt coordinate fields
            SetLongitudeFields();
            SetLatitudeFields();
        }

        /// <summary>
        /// Create an instance by integer longitude/latitude degrees and minutes values and decimal seconds values.
        /// </summary>
        /// <param name="lonDegrees">Longitude degrees</param>
        /// <param name="lonMinutes">Longitude minutes</param>
        /// <param name="lonSeconds">Longitude decimal seconds</param>
        /// <param name="latDegrees">Latitude degrees</param>
        /// <param name="latMinutes">Latitude minutes</param>
        /// <param name="latSeconds">Latitude decimal seconds</param>
        public PointWGS84(int lonDegrees, int lonMinutes, double lonSeconds, int latDegrees, int latMinutes, double latSeconds)
        {
            // Set decimal coordinate values
            if (!DegMinSecToDecimal(out m_Longitude, lonDegrees, lonMinutes, lonSeconds))
                return;
            if (!DegMinSecToDecimal(out m_Latitude, latDegrees, latMinutes, latSeconds))
                return;
            // Check for valid values
            if (InvalidCoordinates())
                return;
            // Adapt coordinate fields
            SetLongitudeFields();
            SetLatitudeFields();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check whether assigned coordinates are invalid.
        /// </summary>
        /// <returns>true, if invalid.</returns>
        private bool InvalidCoordinates()
        {
            if (m_Longitude < -180.0 || m_Longitude > 180.0)
            {
                m_Longitude = 0.0;
                m_IsValid = false;
                return true;
            }
            if (m_Latitude < -90.0 || m_Latitude > 90.0)
            {
                m_Latitude = 0.0;
                m_IsValid = false;
                return true;
            }
            m_IsValid = true;
            return false;
        }

        /// <summary>
        /// Set associated fields according to longitude.
        /// </summary>
        /// <returns>true, if successful.</returns>
        private bool SetLongitudeFields()
        {
            // Set degrees and decimal minutes
            DecimalToDegMin(m_Longitude, out m_LongitudeDeg, out m_LongitudeMinDec);
            // Set degrees, minutes and decimal seconds
            DecimalToDegMinSec(m_Longitude, out m_LongitudeDeg, out m_LongitudeMin, out m_LongitudeSecDec);
            // Set seconds
            m_LongitudeSec = (int)m_LongitudeSecDec;

            return true;
        }

        /// <summary>
        /// Set associated fields according to latitude.
        /// </summary>
        /// <returns>true, if successful.</returns>
        private bool SetLatitudeFields()
        {
            // Set degrees and decimal minutes
            DecimalToDegMin(m_Latitude, out m_LatitudeDeg, out m_LatitudeMinDec);
            // Set degrees, minutes and decimal seconds
            DecimalToDegMinSec(m_Latitude, out m_LatitudeDeg, out m_LatitudeMin, out m_LatitudeSecDec);
            // Set seconds
            m_LatitudeSec = (int)m_LatitudeSecDec;

            return true;
        }

        /// <summary>
        /// Calculate decimal coordinate from degrees and decimal minutes.
        /// </summary>
        /// <param name="dec">Decimal coordinate output value.</param>
        /// <param name="degrees">Degrees input value.</param>
        /// <param name="minutes">Decimal minutes input value.</param>
        /// <returns>true, if successful.</returns>
        private bool DegMinToDecimal(out double dec, int degrees, double minutes)
        {
            if (minutes < 0)
            {
                dec = 0.0;
                m_IsValid = false;
                return false;
            }

            if (degrees < 0)
            {
                dec = degrees - minutes * MinFactor;
            }
            else
            {
                dec = degrees + minutes * MinFactor;
            }            
            return true;
        }

        /// <summary>
        /// Calculate degrees and decimal minutes from decimal coordinate.
        /// </summary>
        /// <param name="dec">Decimal coordinate input value.</param>
        /// <param name="degrees">Degrees output value.</param>
        /// <param name="minutes">Decimal minutes output value.</param>
        /// <returns>true, if successful.</returns>
        private bool DecimalToDegMin(double dec, out int degrees, out double minutes)
        {
            degrees = (int)dec;
            minutes = (dec - (double)degrees) / MinFactor;
            minutes = Math.Abs(minutes);
            return true;
        }

        /// <summary>
        /// Calculate decimal coordinate from degrees, minutes and decimal seconds.
        /// </summary>
        /// <param name="dec">Decimal coordinate output value.</param>
        /// <param name="degrees">Degrees input value.</param>
        /// <param name="minutes">Minutes input value.</param>
        /// <param name="seconds">Decimal seconds input value.</param>
        /// <returns></returns>
        private bool DegMinSecToDecimal(out double dec, int degrees, int minutes, double seconds)
        {
            if (minutes < 0 || seconds < 0)
            {
                dec = 0.0;
                m_IsValid = false;
                return false;
            }

            if (degrees < 0)
            {
                dec = degrees - minutes * MinFactor - seconds * SecFactor;
            }
            else
            {
                dec = degrees + minutes * MinFactor + seconds * SecFactor;
            }
            return true;
        }

        /// <summary>
        /// Calculate degrees, minutes and decimal seconds from decimal coordinate.
        /// </summary>
        /// <param name="dec">Decimal coordinate input value.</param>
        /// <param name="degrees">Degrees output value.</param>
        /// <param name="minutes">Minutes output value.</param>
        /// <param name="seconds">Decimal seconds output value.</param>
        /// <returns></returns>
        private bool DecimalToDegMinSec(double dec, out int degrees, out int minutes, out double seconds)
        {
            degrees = (int)dec;
            double tempMin = (dec - (double)degrees) / MinFactor;
            minutes = (int)tempMin;
            seconds = (tempMin - (double)minutes) / MinFactor;
            minutes = Math.Abs(minutes);
            seconds = Math.Abs(seconds);
            return true;
        }

        #endregion
    }
}
