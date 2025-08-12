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
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;


namespace WpfSamplingPlotPage
{
    /// <summary>
    /// Class GPS to check for GPS device and read current coordinates
    /// </summary>
    class Gps
    {
        #region Constants

        private const int SleepTime = 2000;

        #endregion // Constants
        
        #region Fields

        // Local variables used to hold the present
        // position as latitude and longitude
        public double m_Latitude = 0;
        public double m_Longitude = 0;
        public double m_Latitude1 = 0;
        public double m_Longitude1 = 0;
        public double m_Latitude2 = 0;
        public double m_Longitude2 = 0;
        public double m_Altitude = 0;
        public int m_Satellites = 0;
        private SerialPort m_SerialPortGps = null;
        private int m_Baudrate = 9600;
        private string m_Status = string.Empty;
        private bool m_DemoMode = true;

        // Demo mode parameters
        private Random m_Random = new Random();
        private int xDir = 1;
        private int yDir = 1;
        private double xInc = 0;
        private double yInc = 0;
        private double incMax = 1;
        private double borderTop = 0;
        private double borderBottom = 0;
        private double borderLeft = 0;
        private double borderRight = 0;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets the current status.
        /// </summary>
        /// <value>The get status.</value>
        public string GetStatus
        {
            get { return m_Status; }
        }

        /// <summary>
        /// Gets the current latitude.
        /// </summary>
        /// <value>The get latitude.</value>
        public double GetLatitude
        {
            get { return m_Latitude; }
        }

        /// <summary>
        /// Gets the current longitude.
        /// </summary>
        /// <value>The get longitude.</value>
        public double GetLongitude
        {
            get { return m_Longitude; }
        }

        /// <summary>
        /// Gets the current altitude.
        /// </summary>
        /// <value>The get altitude.</value>
        public double GetAltitude
        {
            get { return m_Altitude; }
        }

        /// <summary>
        /// Gets the number of active satellites.
        /// </summary>
        /// <value>The get satellites.</value>
        public int GetSatellites
        {
            get { return m_Satellites; }
        }

        #endregion // Properties

        #region Constructor

        /// <summary>
        /// Constructor for creating a Gps instance.
        /// </summary>
        /// <param name="baudrate">The baudrate to use for the GPS device.</param>
        public Gps(int baudrate)
        {
            m_Baudrate = baudrate;
            m_DemoMode = false;
            // WLine("...Constructor...");
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Detects the GPS serial port.
        /// </summary>
        /// <returns></returns>
        internal bool FindGpsSerialPort()
        {
            // WLine("FindGpsSerialPort");
            if (m_DemoMode)
                return true;

            try
            {
                string data = string.Empty;
                // Get all serial port names
                string[] ports = System.IO.Ports.SerialPort.GetPortNames();
                // WLine(ports.Length.ToString());
                bool[] checkPorts = new bool[ports.Length]; 
                int index = 0;
                SerialPort[] m_SerialPortsArray = new SerialPort[ports.Length];
                // WLine("Ports found:");
                // foreach (string port in ports)
                    // WLine(port);

                // Open all available ports
                foreach (string port in ports)
                {
                    try
                    {
                        // if (port.Length != 4)
                        //     continue;
                        m_Status = "...Open " + port;
                        // WLine(m_Status);

                        m_SerialPortsArray[index] = new SerialPort();
                        m_SerialPortsArray[index].PortName = port;
                        m_SerialPortsArray[index].BaudRate = m_Baudrate;
                        // If port already in use, do not check it
                        if (!m_SerialPortsArray[index].IsOpen)
                        {
                            m_SerialPortsArray[index].Open();
                            checkPorts[index] = true;
                        }
                        else
                        {
                            checkPorts[index] = false;
                        }
                    }
                    catch
                    {
                    }

                    index++;
                }

                // Give GPS device time to fill the serial buffer
                Thread.Sleep(SleepTime);

                // Check pots for GPS data
                for (int i = 0; i < ports.Length; i++)
                {
                    // WLine("... check data..." + i.ToString());
                    // Read data from open ports
                    if (checkPorts[i] && (data = m_SerialPortsArray[i].ReadExisting()) != string.Empty)
                    {
                        // WLine(data);
                        // Check for WMEA pattern
                        if (data.Contains("$GP"))
                        {
                            // GPS device has been detected
                            // Console.Beep(1500, 50);
                            m_Status += ": (" + i.ToString() + ") - found";
                            // WLine(m_Status);
                            index = i;
                            m_SerialPortGps = m_SerialPortsArray[i];
                            break;
                        }
                    }
                }

                // Close non-GPS ports again
                for (int i = 0; i < ports.Length; i++)
                {
                    if (checkPorts[i] && i != index)
                        m_SerialPortsArray[i].Close();
                }

                // Return true if GPS has been detected
                if (m_SerialPortGps != null)
                    return true;
            }
            catch (Exception ex)
            {
                m_Status += ": " + ex.Message;
                return false;
            }

            m_Status += " - not found!";
            return false;
        }

        static private StreamWriter m_SW = null;


        // Helper: Write debug text to file
        public void WLine(string str)
        {
            string TargetDirectory = "C:\\Test\\";
            DateTime time = DateTime.Now;
            
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                if (m_SW == null)
                    m_SW = new StreamWriter(TargetDirectory + "Debug.txt", true);

                //Write a line of text
                if (m_SW != null)
                {
                    m_SW.WriteLine(time.TimeOfDay.ToString().Substring(0,8) + ":  " + str);
                }
            }
            catch
            {
            }
            finally
            {
                m_SW.Close();
                m_SW = null;
            }
        }

        /// <summary>
        /// Closes the GPS serial port.
        /// </summary>
        internal void CloseGpsSerialPort()
        {
            if (!m_DemoMode && m_SerialPortGps != null)
                if (m_SerialPortGps.IsOpen)
                    m_SerialPortGps.Close();
        }

        #region Event handlers

        /// <summary>
        /// Read current GPS coordinates from serial port, if available.
        /// </summary>
        internal bool ReadGpsCoordinates()
        {
            try
            {
                if (m_DemoMode)
                {
                    DemoCoordinates();
                    return true;
                }

                if (m_SerialPortGps == null)
                    return false;

                if (m_SerialPortGps.IsOpen)
                {
                    string data = string.Empty;
                    data = m_SerialPortGps.ReadExisting();
                    // WLine(data);

                    string[] strArr = data.Split('$');
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        string strTemp = strArr[i];
                        string[] lineArr = strTemp.Split(',');
                        if (lineArr[0] == "GPGGA")
                        {

                            try
                            {
                                // Latitude
                                Double dLat = Convert.ToDouble(lineArr[2], CultureInfo.InvariantCulture);
                                dLat = Convert.ToDouble((int)(dLat / 100));
                                // string[] lat = dLat.ToString(CultureInfo.InvariantCulture).Split('.');
                                Double dLatMin = Convert.ToDouble(lineArr[2].Substring(2), CultureInfo.InvariantCulture) / 60;
                                dLat += dLatMin;
                                // string strLat = (lineArr[3] == "S" ? "-" : "") + lat[0].ToString() + "." + dLatMin.ToString("F10", CultureInfo.InvariantCulture);
                                m_Latitude = (lineArr[3] == "S" ? -dLat : dLat);
                                // m_Latitude = MediumLat();

                                // Longitude
                                Double dLon = Convert.ToDouble(lineArr[4], CultureInfo.InvariantCulture);
                                dLon = Convert.ToDouble((int)(dLon / 100));
                                // string[] lon = dLon.ToString(CultureInfo.InvariantCulture).Split('.');
                                Double dLonMin = Convert.ToDouble(lineArr[4].Substring(3), CultureInfo.InvariantCulture) / 60;
                                dLon += dLonMin;
                                // string strLon = (lineArr[5] == "W" ? "-" : "") + lon[0].ToString() + "." + ((Convert.ToDouble(lon[1], CultureInfo.InvariantCulture) / 60)).ToString("#####", CultureInfo.InvariantCulture);
                                m_Longitude = (lineArr[5] == "W" ? -dLon : dLon);
                                // m_Longitude = MediumLon();

                                // Altitude
                                m_Altitude = Convert.ToDouble(lineArr[9], CultureInfo.InvariantCulture);

                                // Satellites
                                m_Satellites = Convert.ToInt32(lineArr[7]);

                                // strTemp = "Data: Sats=" + m_Satellites.ToString() + ",  Lat=[" + lineArr[2] + "] Lon=[" + lineArr[4] + "] '' --> Lat=" + m_Latitude.ToString() + " Lon=" + m_Longitude.ToString() + " °";
                                // WLine(strTemp);

                                return true;
                            }
                            catch (Exception ex)
                            {
                                m_Status = ex.Message;
                            }
                        }
                    }
                }
                else
                {
                    m_Status = m_SerialPortGps.PortName + " is closed!";
                }
            }
            catch (Exception ex)
            {
                m_Status = ex.Message;
            }
            return false;
        }

        #endregion // Event handlers

        #region Demo Mode

        /// <summary>
        /// Initializes a new instance of the <see cref="Gps"/> class for Demo Mode.
        /// </summary>
        /// <param name="worldTop">The reference map top latitude coordinate.</param>
        /// <param name="worldBottom">The reference map bottom latitude coordinate.</param>
        /// <param name="worldLeft">The reference map left longitude coordinate.</param>
        /// <param name="worldRight">The reference map rightlongitude coordinate.</param>
        public Gps(double worldTop, double worldBottom, double worldLeft, double worldRight)
        {
            m_Latitude = (worldTop + worldBottom) / 2;
            m_Longitude = (worldLeft + worldRight) / 2;
            yInc = (worldTop - worldBottom) / 200;
            xInc = (worldRight - worldLeft) / 200;
            incMax = xInc + yInc;
            borderTop = worldTop - yInc * 10;
            borderBottom = worldBottom + yInc * 10;
            borderLeft = worldLeft + xInc * 10;
            borderRight = worldRight - xInc * 10;
            xDir = m_Random.NextDouble() < 0.5 ? -1 : 1;
            yDir = m_Random.NextDouble() < 0.5 ? -1 : 1;
            m_Satellites = 0;

            m_DemoMode = true;
        }


        /// <summary>
        /// Create random coordinates for Demo Mode.
        /// </summary>
        private void DemoCoordinates()
        {
            // Create increments
            xInc = incMax * m_Random.NextDouble();
            yInc = incMax * m_Random.NextDouble();
            // Change direction if close to the border
            if ( m_Latitude > borderTop)
                yDir = -1;
            if ( m_Latitude < borderBottom)
                yDir = 1;
            if (m_Longitude < borderLeft)
                xDir = 1;
            if (m_Longitude > borderRight)
                xDir = -1;
 
            // Random generated values
            m_Longitude += xDir * xInc;
            m_Latitude += yDir * yInc;
            // Constant values
            m_Altitude = 512.3;
            if (m_Satellites < 8)
                m_Satellites++;
        }

        /* Methods to smoothen the track, not used.
        private double MediumLat()
        {
            if (m_Latitude2 != 0)
            {
                double temp = (m_Latitude + m_Latitude1 + m_Latitude2) / 3.0;
                m_Latitude2 = m_Latitude1;
                m_Latitude1 = m_Latitude;
                m_Latitude = temp;
            }
            return m_Latitude;
        }

        private double MediumLon()
        {
            if (m_Longitude2 != 0)
            {
                double temp = (m_Longitude + m_Longitude1 + m_Longitude2) / 3.0;
                m_Longitude2 = m_Longitude1;
                m_Longitude1 = m_Longitude;
                m_Longitude = temp;
            }
            return m_Longitude;
        }
        */

        #endregion // Demo Mode

        #endregion // Methods
    }
}
